namespace AMS.Finanzas
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
    using AMS.Documentos;
	using AMS.Forms;
    using AMS.Contabilidad;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Finanzas_AnulacionTesoreria.
	/// </summary>
	public partial class AMS_Finanzas_AnulacionTesoreria : System.Web.UI.UserControl
	{
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		protected string pathToMain=ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoConsignacion;
        ProceHecEco contaOnline = new ProceHecEco();
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and tvig_vigencia='V' order by palm_descripcion;");
				if(Request.QueryString["prefD"]!=null && Request.QueryString["numD"]!=null)
				{
					Response.Write("<script language:javascript>alert('Se ha creado el documento "+Request.QueryString["prefD"]+"-"+Request.QueryString["numD"]+"');</script>");
					try
					{
						formatoConsignacion=new FormatosDocumentos();
						formatoConsignacion.Prefijo=Request.QueryString["prefD"];
						formatoConsignacion.Numero=Convert.ToInt32(Request.QueryString["numD"]);
						formatoConsignacion.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefD"]+"'");
						if(formatoConsignacion.Codigo!=string.Empty)
						{
							if(formatoConsignacion.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoConsignacion.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoConsignacion.Mensajes+"<br>";
					}
				}
                if (!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='AT' and tvig_vigencia = 'V' "))
				{
					Response.Write("<script language:javascript>alert('No hay documentos del tipo AT definidos para anulación');</script>");
					prefijoDocumento.Enabled=false;
					prefijoDocumento.Enabled=false;
					aceptar.Enabled=false;
				}
				else
				{
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='AT' and tvig_vigencia = 'V' ORDER BY pdoc_descripcion ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento , "%", "%", "AT");
                    if (prefijoDocumento.Items.Count > 1)
                    {
                        prefijoDocumento.Items.Insert(0, "Seleccione:");
                    }
                    else
                        prefijoDocumento_SelectedIndexChanged(sender, e);
                //        numeroTesoreria.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
					prefijoDocumento.Enabled=true;
					aceptar.Enabled=true;
				}
			}
			holderAnulaciones.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.Anulaciones.ascx"));
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void prefijoDocumento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            numeroTesoreria.Text  = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoDocumento.SelectedValue+"'");
            Int32 ultimoTesoreria = 0;
            try
            {
                ultimoTesoreria = Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(MTES_NUMERO+1) FROM mtesoreria WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'"));
            }
            catch
            { 
            }

            if (ultimoTesoreria > Convert.ToInt32(numeroTesoreria.Text.ToString()))
                numeroTesoreria.Text = ultimoTesoreria.ToString();
        }

		protected void guardar_Click(object sender, System.EventArgs e)
		{
			Control hijo;
			DataTable tablaDatos=new DataTable();
			Finanzas.Tesoreria.Consignacion miConsignacion=new AMS.Finanzas.Tesoreria.Consignacion();
            hijo = holderAnulaciones.Controls[0];
            if (((HtmlInputHidden)hijo.FindControl("hdnTip")).Value == "DE" || ((HtmlInputHidden)hijo.FindControl("hdnTip")).Value == "DF")
                EstablecerPrefijosNotas(hijo);
            tablaDatos = (DataTable)Session["tablaDatos"];
            miConsignacion = new Finanzas.Tesoreria.Consignacion(tablaDatos);
            
            if (detalleTransaccion.Text == "")
                Response.Write("<script>alert('Debe especificar un detalle');</script>");
            else
            {
                miConsignacion.PrefijoConsignacion = ((DropDownList)hijo.FindControl("tipoDocAnular")).SelectedValue;
                miConsignacion.NumeroConsignacion = Convert.ToInt32(((DropDownList)hijo.FindControl("numeroDocumento")).SelectedValue);
                string fechaConsignacion = DBFunctions.SingleData("select mtes_fecha from mtesoreria where pdoc_codigo = '" + miConsignacion.PrefijoConsignacion.ToString() + "' and mtes_numero = " + miConsignacion.NumeroConsignacion.ToString() + " ");
                fechaConsignacion = Convert.ToDateTime(fechaConsignacion).ToString("yyyy-MM-dd");
                string vigenciaContable  = DBFunctions.SingleData("select pano_ano concat pmes_mes from ccontabilidad ");
                if(vigenciaContable.Length==5) 
                    vigenciaContable = vigenciaContable.Substring(0,4)+'0'+vigenciaContable.Substring(4,1);
                string vigenciaTransaccion = fechaConsignacion.Substring(0, 4) + fechaConsignacion.Substring(5, 2);
                if (Convert.ToDouble(vigenciaTransaccion.ToString()) < Convert.ToDouble(vigenciaContable.ToString()))
                    Response.Write("<script>alert('la fecha de la consingación es Menor a la Vigencia Contable, Transacción NO permitida');</script>");

                else
                {
                    miConsignacion.Almacen = this.almacen.SelectedValue;
                    miConsignacion.CodigoCuenta = "";
                    miConsignacion.Detalle = this.detalleTransaccion.Text;
                    miConsignacion.Fecha   = fechaConsignacion;
                    miConsignacion.NumeroTesoreria = Convert.ToInt32(this.numeroTesoreria.Text);
                    miConsignacion.PrefijoDocumento = this.prefijoDocumento.SelectedValue;
                    miConsignacion.Proceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    miConsignacion.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
                    string tem = ((HtmlInputHidden)hijo.FindControl("hdnTotal")).Value;
                    if (tem == "")
                    {
                        tem = "0";
                    }
                    miConsignacion.Total   = Convert.ToDouble(tem);
                    miConsignacion.TipoMov = ((HtmlInputHidden)hijo.FindControl("hdnTip")).Value;
                    if (((HtmlInputHidden)hijo.FindControl("hdnTip")).Value == "RF")
                        miConsignacion.PrefijoNota = ((DropDownList)hijo.FindControl("ddlPrefDev")).SelectedValue;
                    if (((HtmlInputHidden)hijo.FindControl("hdnTip")).Value == "DF")
                        miConsignacion.PrefijoNota = ((DropDownList)hijo.FindControl("ddlPrefDevProv")).SelectedValue;
                    if (miConsignacion.Guardar_Anulacion())
                    {
                        // contabilización ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(fechaConsignacion), "");
                        lb.Text += miConsignacion.Mensajes;
                        Session.Clear();
                        Response.Redirect(pathToMain + "?process=Finanzas.AnulacionTesoreria&prefD=" + miConsignacion.PrefijoDocumento + "&numD=" + miConsignacion.NumeroTesoreria + "");
                    }
                    else
                        lb.Text += miConsignacion.Mensajes;
                }
            }
		}

		private void EstablecerPrefijosNotas(Control ctl)
		{
			DataGrid dg=null;
			DataTable dt=null;
			if(((HtmlInputHidden)ctl.FindControl("hdnTip")).Value=="DE")
			{
				dg=((DataGrid)ctl.FindControl("dgDev"));
				dt=(DataTable)Session["tablaDatos"];
				for(int i=0;i<dg.Items.Count;i++)
					dt.Rows[i]["PREFIJO NOTA"]=((DropDownList)dg.Items[i].Cells[10].FindControl("ddlPrefNot")).SelectedValue;
			}
			else if(((HtmlInputHidden)ctl.FindControl("hdnTip")).Value=="DF")
			{
				dg=((DataGrid)ctl.FindControl("dgDevRem"));
				dt=(DataTable)Session["tablaDatos"];
				for(int i=0;i<dg.Items.Count;i++)
					dt.Rows[i]["PREFIJO NOTA DEVOLUCION"]=((DropDownList)dg.Items[i].Cells[10].FindControl("ddlPrefNotRem")).SelectedValue;
			}
			Session["tablaDatos"]=dt;
		}

		protected void cancelar_Click(object sender, System.EventArgs e)
		{
			Session.Clear();
			Response.Redirect(pathToMain+"?process=Finanzas.AnulacionTesoreria");
		}

		protected void aceptar_Click(object sender, System.EventArgs e)
		{
			holderAnulaciones.Visible=true;
		}
	}
}
