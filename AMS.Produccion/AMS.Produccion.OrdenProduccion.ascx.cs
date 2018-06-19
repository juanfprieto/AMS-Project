namespace AMS.Produccion
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Documentos;
	using AMS.Forms;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Produccion_OrdenProduccion.
	/// </summary>
	public partial class AMS_Produccion_OrdenProduccion : System.Web.UI.UserControl
	{
		#region Controles
		private DataTable dtProduccion;
		private string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request["prefOP"]!=null && Request["numOP"]!=null && Request["prefT"]!=null && Request["pDoc"]!=null && Request["nDoc"]!=null)
				{
					string strRes=(Request["ens"]!=null)?"ensamble":"producción";
                    Utils.MostrarAlerta(Response, "Se ha creado satisfactoriamente la orden de " + strRes + " " + Request["pDoc"] + "-" + Request["nDoc"] + "");
					//Pedido 1
					FormatosDocumentos formatoFactura=new FormatosDocumentos();
					try{
                        formatoFactura.Prefijo = Request.QueryString["pDoc"];
                        formatoFactura.Numero = Convert.ToInt32(Request.QueryString["nDoc"]);
						formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["pDoc"]+"'");
						if(formatoFactura.Codigo!=string.Empty)
						{
							if(formatoFactura.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lb.Text="Error al generar el formato pedido 1. Detalles : <br>"+formatoFactura.Mensajes;
					}
					//Pedido 2
                    formatoFactura = new FormatosDocumentos();
                    if (Request["dbl"] != null && Request.QueryString["dbl"].ToString().Length > 0)
                        try
                        {
                            formatoFactura.Prefijo = Request.QueryString["prefOP"];
                            formatoFactura.Numero = Convert.ToInt32(Request.QueryString["numOP"]) - 1;
                            formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pDoc"] + "'");
                            if (formatoFactura.Codigo != string.Empty)
                            {
                                if (formatoFactura.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                            }
                        }
                        catch
                        {
                            lb.Text = "Error al generar el formato pedido 2. Detalles : <br>" + formatoFactura.Mensajes;
                        }
					//Transferencia 1
					formatoFactura=new FormatosDocumentos();
					if(Request["numT"]!=null && Request.QueryString["numT"].ToString().Length>0)
					try
					{
						formatoFactura.Prefijo=Request.QueryString["prefT"];
						formatoFactura.Numero=Convert.ToInt32(Request.QueryString["numT"]);
						formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefT"]+"'");
						if(formatoFactura.Codigo!=string.Empty)
						{
							if(formatoFactura.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lb.Text="Error al generar el formato transferencia 1. Detalles : <br>"+formatoFactura.Mensajes;
					}
					//Transferencia 2
                    formatoFactura = new FormatosDocumentos();
                    if (Request["dbl"] != null && Request.QueryString["dbl"].ToString().Length > 0)
                        try
                        {
                            formatoFactura.Prefijo = Request.QueryString["prefT"];
                            formatoFactura.Numero = Convert.ToInt32(Request.QueryString["numT"]) - 1;
                            formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefT"] + "'");
                            if (formatoFactura.Codigo != string.Empty)
                            {
                                if (formatoFactura.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                            }
                        }
                        catch
                        {
                            lb.Text = "Error al generar el formato transferencia 2. Detalles : <br>" + formatoFactura.Mensajes;
                        }
				}
				
				string campoProg;
				if(Request.QueryString["ens"]!=null)
					Utils.FillDll(ddlPrefijo,"SELECT pdoc_codigo,pdoc_codigo CONCAT '-' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='OP' order by pdoc_codigo,pdoc_descripcion", false);
				else
                    Utils.FillDll(ddlPrefijo, "SELECT pdoc_codigo,pdoc_codigo CONCAT '-' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='OP' order by pdoc_codigo desc,pdoc_descripcion", false);
                try
                {
                    tbNumero.Text = DBFunctions.SingleData("SELECT max(MORD_NUMEORDE)+1 FROM MORDENPRODUCCION WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "'");
                    Utils.FillDll(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo='VM' AND pven_vigencia='V' order by pven_nombre", false);
                    Utils.FillDll(ddlAlmacen, "SELECT PA.palm_almacen, MP.mpla_funcion FROM PALMACEN PA, MPLANTAS MP WHERE PA.palm_almacen=MP.mpla_codigo order by PA.palm_descripcion;", false);
                    Utils.FillDll(ddlAlmMat, "SELECT PA.palm_almacen, PA.palm_descripcion FROM PALMACEN PA order by PA.palm_descripcion;", false);
                    Utils.FillDll(ddlPedido, "SELECT pped_codigo,pped_nombre FROM ppedido WHERE tped_codigo='T' order by pped_nombre", false);
                    Utils.FillDll(ddlTransferencia, "SELECT pdoc_codigo,pdoc_codigo CONCAT '-' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='TT' order by pdoc_codigo,pdoc_descripcion", false);
                    tbFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");               
                if (Request.QueryString["ens"]!=null){
					ViewState["TIPO"]="E";
					lblElemento.Text="Catálogo del Vehículo :";
					campoProg="PCAT_CODIGO";
				}
				else
				{
					ViewState["TIPO"]="P";
					lblElemento.Text="Código del Item :";
					campoProg="MITE_CODIGO";
				}

				//Programa produccion
				DateTime dttFecha = DateTime.Now;
				DateTime dttFecha1 = dttFecha.AddMonths(-1);
				DateTime dttFecha2 = dttFecha.AddMonths(-2);
				DateTime dttFecha2h = dttFecha.AddMonths(-3);
				DateTime dttFecha2hh = dttFecha.AddMonths(-4);
				DateTime dttFecha3 = dttFecha.AddMonths(1);
				DateTime dttFecha4 = dttFecha.AddMonths(2);

                Utils.FillDll(ddlLote,
                    "SELECT mprog_numero, mprog_numero from MPROGRAMAPRODUCCION mp " +
					"where mp.mprog_consecutivo in ("+
					" select dp.mprog_consecutivo "+
					" from DPROGRAMAPRODUCCION dp "+
					" where "+
					" dp.mprog_consecutivo=mp.mprog_consecutivo and "+
					" dp."+campoProg+" IS NOT NULL and dp.dprog_cantidad > dp.dprog_total) ORDER BY mprog_numero;", false);
                }
                catch { }
            }
			else
			{
				if(Session["dtProduccion"]!=null)
					dtProduccion=(DataTable)Session["dtProduccion"];
			}
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
			this.dgProduccion.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgProduccion_ItemCommand);

		}
		#endregion
		#region Botones
		//Seleccionar
		protected void btnConfirmar_Click(object sender, System.EventArgs e)
		{
			if(tbnit.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar el NIT.");
				return;
			}
			if(ddlLote.Items.Count==0 && rbConPrograma.Checked)
			{
                Utils.MostrarAlerta(Response, "No hay programas de producción activos.");
				return;
			}

            string sqlCatalogo = "";
            if (rbConPrograma.Checked) // ensamble enlazado a un programa de producción
            {
                if (ViewState["TIPO"].ToString() == "E")
                {
                    sqlCatalogo = String.Format(
                        "SELECT DISTINCT pc.pcat_codigo, \n" +
                        "       pc.pcat_codigo CONCAT '-' CONCAT pc.pcat_descripcion \n" +
                        "FROM pcatalogovehiculo pc, \n" +
                        "     pensambleproduccion pe, \n" +
                        "     dprogramaproduccion dp, \n" +
                        "     mprogramaproduccion mp \n" +
                        "WHERE pc.pcat_codigo = pe.pcat_codigo \n" +
                        "AND   pc.pcat_codigo = dp.pcat_codigo \n" +
                        "AND   dp.mprog_consecutivo = mp.mprog_consecutivo \n" +
                        "AND   mp.mprog_numero = '{0}' \n" +
                        "ORDER BY pc.pcat_codigo"
                        , ddlLote.SelectedValue);
                }
                else 
                {
                    sqlCatalogo = String.Format(
                        "SELECT DISTINCT mi.mite_codigo, \n" +
                        "       mi.mite_codigo CONCAT '-' CONCAT mi.mite_nombre \n" +
                        "FROM mitems mi, \n" +
                        "     pensambleproduccion pe, \n" +
                        "     dprogramaproduccion dp, \n" +
                        "     mprogramaproduccion mp \n" +
                        "WHERE mi.mite_codigo = pe.mite_codigo \n" +
                        "AND   mi.mite_codigo = dp.mite_codigo \n" +
                        "AND   dp.mprog_consecutivo = mp.mprog_consecutivo \n" +
                        "AND   mp.mprog_numero = '{0}' \n" +
                        "ORDER BY mi.mite_codigo"
                        , ddlLote.SelectedValue);
                }
            }
            else // ensamble sin programa de producción
            {
                if (ViewState["TIPO"].ToString() == "E")
                {
                    sqlCatalogo = "SELECT DISTINCT pc.pcat_codigo, \n" +
                                "       pc.pcat_codigo CONCAT '-' CONCAT pc.pcat_descripcion \n" +
                                "FROM pcatalogovehiculo pc, \n" +
                                "     pensambleproduccion pe \n" +
                                "WHERE pc.pcat_codigo = pe.pcat_codigo \n" +
                                "AND   pe.pens_vigente = 'S' \n" +
                                "ORDER BY pc.pcat_codigo";
                }
                else
                {
                    sqlCatalogo = "SELECT DISTINCT mi.mite_codigo, \n" +
                                "       mi.mite_codigo CONCAT '-' CONCAT mi.mite_nombre \n" +
                                "FROM mitems mi, \n" +
                                "     pensambleproduccion pe \n" +
                                "WHERE mi.mite_codigo = pe.mite_codigo \n" +
                                "AND   pe.pens_vigente = 'S' \n" +
                                "ORDER BY mi.mite_codigo";
                }
            }

            Utils.FillDll(ddlCatalogo, sqlCatalogo, false);
			ddlCatalogo_SelectedIndexChanged(ddlKit,e);
			ConstruirDtProduccion();
			EnlazarGrilla();
			pnlObjetos.Visible=true;
			ddlPrefijo.Enabled=tbNumero.Enabled=tbFecha.Enabled=ddlLote.Enabled=false;
			btnConfirmar.Visible=false;
		}

		protected void btnGrabar_Click(object sender, System.EventArgs e)
		{
			if(ddlKit.Items.Count>1)
			{
                Utils.MostrarAlerta(Response, "Error, existe más de un ensamble activo para el item.");
				return;
			}
			Produccion miProduccion=new Produccion(
                        ddlPrefijo.SelectedValue,
                        int.Parse(tbNumero.Text),
                        tbFecha.Text,
						HttpContext.Current.User.Identity.Name.ToLower(),
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        rbConPrograma.Checked ? ddlLote.SelectedValue : null,
                        tbnit.Text,
						ddlVendedor.SelectedValue,
                        ddlPedido.SelectedValue,
                        ddlTransferencia.SelectedValue,
						ddlAlmacen.SelectedValue,
                        ddlAlmMat.SelectedValue,
                        tbObservacion.Text,
                        dtProduccion,
                        ViewState["TIPO"].ToString());

			if(miProduccion.GuardarOrdenProduccion())
			{
				string numP=DBFunctions.SingleData("SELECT max(MPED_NUMEPEDI) FROM MPEDIDOITEM WHERE PPED_CODIGO='"+ddlPedido.SelectedValue+"';");
				string numT=DBFunctions.SingleData("SELECT max(DITE_NUMEDOCU) FROM DITEMS WHERE DITE_PREFDOCUREFE='"+ddlPedido.SelectedValue+"' AND DITE_NUMEDOCUREFE="+numP+";");
				string dbl="";
				if(miProduccion.numTransferencias>1)
					dbl="&dbl=1";
				if(ViewState["TIPO"].ToString()=="E")
					Response.Redirect(indexPage+"?path="+Request.QueryString["path"]+"&process=Produccion.OrdenProduccion&prefOP="+ddlPedido.SelectedValue+"&numOP="+numP+"&prefT="+ddlTransferencia.SelectedValue+"&numT="+numT+"&pDoc="+miProduccion.PrefijoOrden+"&nDoc="+miProduccion.NumeroOrden+"&ens=1"+dbl);
				else
					Response.Redirect(indexPage+"?path="+Request.QueryString["path"]+"&process=Produccion.OrdenProduccion&prefOP="+ddlPedido.SelectedValue+"&numOP="+numP+"&prefT="+ddlTransferencia.SelectedValue+"&numT="+numT+"&pDoc="+miProduccion.PrefijoOrden+"&nDoc="+miProduccion.NumeroOrden+dbl);
			}
			else
			{
				if(!miProduccion.Disponible)
                    Utils.MostrarAlerta(Response, ""+miProduccion.Mensajes+"");
				else
					lb.Text=miProduccion.Mensajes;		
			}
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Server.Transfer(indexPage+"?process=Produccion.OrdenProduccion");
		}

		//Agregar elemento
		protected void btnAgregar_Click(object sender, System.EventArgs e)
		{
			int cantidad;
			try{
				cantidad=Convert.ToInt32(tbcant.Text.Replace(",",""));}
			catch{
				Utils.MostrarAlerta(Response, "No es un valor válido para la cantidad");
				return;}
			if(ddlCatalogo.Items.Count==0 || ddlCatalogo.Items.Count==0)
				Utils.MostrarAlerta(Response, "No hoy items para el programa de producción seleccionado o no hay ensambles asociados al elemento seleccionado");
			else if(ExisteCatalogo(ddlCatalogo.SelectedValue,ddlKit.SelectedValue))
				Utils.MostrarAlerta(Response, "Este elemento ya fue ingresado");
			else
			{
				DataRow fila=dtProduccion.NewRow();
				if(ViewState["TIPO"].ToString()=="E")
				{
					fila["CATALOGO"]=ddlCatalogo.SelectedValue;
					fila["CANTPROD"]=int.Parse(DBFunctions.SingleData("SELECT CASE WHEN sum(dord_cantxprod)-sum(dord_cantentr) IS NULL THEN 0 ELSE sum(dord_cantxprod)-sum(dord_cantentr) END FROM dbxschema.dordenproduccion WHERE pcat_codigo='"+ddlCatalogo.SelectedValue+"'"));
					fila["CANTSTOCK"] = int.Parse(DBFunctions.SingleData("SELECT count(MCV.pcat_codigo) FROM mvehiculo MV, MCATALOGOVEHICULO MCV WHERE MCV.MCAT_VIN = MV.MCAT_VIN AND MV.test_tipoesta=20"));
					fila["PEDCLI"]=int.Parse(DBFunctions.SingleData("SELECT count(pcat_codigo) FROM mpedidovehiculo WHERE test_tipoesta=10 AND pcat_codigo='"+ddlCatalogo.SelectedValue+"'"));
					fila["DISP"]=(int)fila[1]+(int)fila[2]-(int)fila[3];
					fila["CANTPROC"]=cantidad;
					fila["ENSAMBLE"]=ddlKit.SelectedValue;
				}
				else
				{
					fila["CATALOGO"]=ddlCatalogo.SelectedValue;
					fila["CANTPROD"]=int.Parse(DBFunctions.SingleData("SELECT CASE WHEN sum(dord_cantxprod)-sum(dord_cantentr) IS NULL THEN 0 ELSE sum(dord_cantxprod)-sum(dord_cantentr) END FROM dbxschema.dordenproduccion WHERE mite_codigo='"+ddlCatalogo.SelectedValue+"'"));
					fila["CANTSTOCK"]=int.Parse(DBFunctions.SingleData("SELECT INT(COALESCE(ms.msal_cantactual,0)) FROM mitems mi left join msaldoitem ms on ms.mite_codigo=mi.mite_codigo where mi.mite_codigo='"+ddlCatalogo.SelectedValue+"'"));
					fila["PEDCLI"]=int.Parse(DBFunctions.SingleData("SELECT COALESCE(ms.msal_pedipendi,0) FROM mitems mi left join msaldoitem ms on ms.mite_codigo=mi.mite_codigo where mi.mite_codigo='"+ddlCatalogo.SelectedValue+"'"));
					fila["DISP"]=(int)fila[1]+(int)fila[2]-(int)fila[3];
					fila["CANTPROC"]=cantidad;
					fila["ENSAMBLE"]=ddlKit.SelectedValue;
				}
				dtProduccion.Rows.Add(fila);
				EnlazarGrilla();
				dgProduccion.Visible=true;
				btnGrabar.Enabled=true;
			}
		}

        protected void tipoOrden_OnCheckedChanged(object sender, System.EventArgs e)
        {
            phConPrograma.Visible = rbConPrograma.Checked;
        }

		#endregion
		#region DropdownLists
		//Cambia catalogo
		protected void ddlCatalogo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(ViewState["TIPO"].ToString()=="E")
				bind.PutDatasIntoDropDownList(ddlKit,"SELECT pens_codigo,pens_codigo CONCAT '-' CONCAT pens_descripcion FROM pensambleproduccion WHERE pcat_codigo='"+ddlCatalogo.SelectedValue+"' and pens_vigente='S'");
			else
				bind.PutDatasIntoDropDownList(ddlKit,"SELECT pens_codigo,pens_codigo CONCAT '-' CONCAT pens_descripcion FROM pensambleproduccion WHERE mite_codigo='"+ddlCatalogo.SelectedValue+"' and pens_vigente='S'");
		}

		#endregion
		#region Tabla produccion
		private void dgProduccion_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			dtProduccion.Rows[e.Item.DataSetIndex].Delete();
			EnlazarGrilla();
			if(ContarFilasDtProduccion()==0)
				btnGrabar.Enabled=false;
		}

		private bool ExisteCatalogo(string catalogo,string kit)
		{
			DataRow[] cats=dtProduccion.Select("CATALOGO='"+catalogo+"'");
			if(cats.Length!=0)
				return true;
			else
				return false;
		}

		private int ContarFilasDtProduccion()
		{
			return dtProduccion.Rows.Count;
		}
		private void ConstruirDtProduccion()
		{
			dtProduccion=new DataTable();
			dtProduccion.Columns.Add("CATALOGO",typeof(string));
			dtProduccion.Columns.Add("CANTPROD",typeof(int));
			dtProduccion.Columns.Add("CANTSTOCK",typeof(int));
			dtProduccion.Columns.Add("PEDCLI",typeof(int));
			dtProduccion.Columns.Add("DISP",typeof(int));
			dtProduccion.Columns.Add("CANTPROC",typeof(int));
			dtProduccion.Columns.Add("ENSAMBLE",typeof(string));
		}

		private void EnlazarGrilla()
		{
			dgProduccion.DataSource=dtProduccion;
			dgProduccion.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(dgProduccion);
			Session["dtProduccion"]=dtProduccion;
		}
		#endregion

		protected void ddlPrefijo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			tbNumero.Text=DBFunctions.SingleData("SELECT max(MORD_NUMEORDE)+1 FROM MORDENPRODUCCION WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
		}

	}
}
