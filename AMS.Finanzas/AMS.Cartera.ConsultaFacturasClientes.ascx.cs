using System;
using System.Text;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.Tools;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_ConsultaFacturasClientes.
	/// </summary>
	public partial class AMS_Automotriz_ConsultaFacturasClientes : System.Web.UI.UserControl
	{
		#region Propiedades
		protected DataSet lineas;
		protected DataSet lineas2;
		protected DataSet lineaced;
		protected DataTable resultado;
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Automotriz_ConsultaFacturasClientes));
			if(!IsPostBack)
			{
				tbNit.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT DISTINCT MFC.mnit_nit AS NIT, MNI.mnit_apellidos || \\' \\' || MNI.mnit_apellido2 || \\' \\' || MNI.mnit_nombres || \\' \\' || MNI.mnit_nombre2 AS NOMBRES "+
																    " FROM mfacturacliente MFC INNER JOIN mnit MNI ON MFC.mnit_nit = MNI.mnit_nit "+
																    " ORDER BY MNI.mnit_apellidos || \\' \\' || MNI.mnit_apellido2 || \\' \\' || MNI.mnit_nombres || \\' \\' || MNI.mnit_nombre2 ASC', new Array());";
				ddlNumedocu.Items.Insert(0,new ListItem("Seleccione ...",""));
				ddlPrefDocumen.Items.Insert(0,new ListItem("Seleccione ...",""));
			}
			else
			{
				if(hdNitSelec.Value != String.Empty)
				{
					BindDdlPrefDocumen();
					BindDdlNumedocu();
				}
			}
//            Session["Rep"] = this.Cuerpo_Correo();

        }

        protected string Cuerpo_Correo()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            consultasFacturas.RenderControl(htmlTW);
            return SB.ToString();
        }

        protected void consultar(Object Sender, EventArgs e)
        {
            BindDdlPrefDocumen();
            BindDdlNumedocu();

            DatasToControls bind = new DatasToControls();
            Panel1.Visible = true;
            this.PrepararTabla();
            lineas = new DataSet();
            lineas2 = new DataSet();
            DBFunctions.Request(lineas, IncludeSchema.NO, "select mfc.MNIT_NIT, mfc.PALM_ALMACEN, mfc.TVIG_VIGENCIA, mfc.MFAC_FACTURA, mfc.MFAC_PAGO, mfc.MFAC_VALOFACT, mfc.MFAC_VALOIVA, mfc.MFAC_VALOABON , mfc.PVEN_CODIGO, MFAC_VENCE, mfc.MFAC_VALOFLET, mfc.MFAC_VALORETE, mfc.MFAC_USUARIO FROM DBXSCHEMA.MFACTURACLIENTE mfc, DBXSCHEMA.mnit mn WHERE mfc.mnit_nit = mn.mnit_nit and PDOC_CODIGO='" + ddlPrefDocumen.SelectedValue.ToString() + "' AND MFAC_NUMEDOCU=" + ddlNumedocu.SelectedValue.ToString() + " ");
            //string query="select MNIT_NIT,PALM_ALMACEN,TVIG_VIGENCIA,MFAC_FACTURA,MFAC_PAGO,MFAC_VALOFACT,MFAC_VALOIVA,MFAC_VALOABON,PVEN_CODIGO,MFAC_VENCE FROM DBXSCHEMA.MFACTURACLIENTE WHERE PDOC_CODIGO='"+PrefDocumen.SelectedValue.ToString()+"' AND MFAC_NUMEDOCU="+Numedocu.SelectedValue.ToString()+" ";
            if (lineas.Tables.Count > 0)
            {
                for (int i = 0; i < lineas.Tables[0].Rows.Count; i++)
                {
                    docLabel.Text = ddlPrefDocumen.SelectedValue.ToString() + " " + ddlNumedocu.SelectedValue.ToString();
                    string detalle = DBFunctions.SingleData("select mfac_observacion from mfacturacliente where pdoc_codigo = '"+ ddlPrefDocumen.SelectedValue.ToString()+"' and mfac_numedocu = "+ ddlNumedocu.SelectedValue.ToString() +" ");
                    detalleLabel.Text = detalle.ToString();
                    fechadocLabel.Text = Convert.ToDateTime(lineas.Tables[0].Rows[i].ItemArray[3]).ToString("yyyy-MM-dd");
                    string nit = lineas.Tables[0].Rows[i].ItemArray[0].ToString();

                    //este codigo es una boleta deberia armarse una sola consulta para todo
                    string nombre = DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='" + nit + "'");
                    nombre = nit + " " + nombre;
                    nitLabel.Text = nombre.ToString();
                    string direccion = DBFunctions.SingleData("SELECT MNIT_DIRECCION from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='" + nit + "'");
                    DirLabel.Text = direccion;
                    string telefono = DBFunctions.SingleData("SELECT  MNIT_TELEFONO from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='" + nit + "'");
                    TelLabel.Text = telefono;
                    string celular = DBFunctions.SingleData("SELECT  MNIT_CELULAR  from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='" + nit + "'");
                    CeluLabel.Text = celular;

                    string estado = DBFunctions.SingleData("select TVIG_NOMBVIGE from DBXSCHEMA.TVIGENCIA WHERE TVIG_VIGENCIA='" + lineas.Tables[0].Rows[i].ItemArray[2].ToString() + "'");
                    estadoLabel.Text = estado.ToString();
                    vencLabel.Text = Convert.ToDateTime(lineas.Tables[0].Rows[i].ItemArray[9]).ToString("yyyy-MM-dd");
                    double Valor = Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[5].ToString());
                    string valorfinal = String.Format("{0:C}", Valor);
                    valodocLabel.Text = valorfinal.ToString();
                    double ValorAbon = Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[7].ToString());
                    string valorfinalAbon = String.Format("{0:C}", ValorAbon);
                    valopaLabel.Text = valorfinalAbon.ToString();
                    rcajaLabel.Text = DBFunctions.SingleData("select PDOC_CODDOCREF concat ' ' concat cast(DDET_NUMEDOCU as character(10)) from DBXSCHEMA.DDETALLEFACTURACLIENTE WHERE PDOC_CODIGO='" + ddlPrefDocumen.SelectedValue.ToString() + "' and MFAC_NUMEDOCU=" + ddlNumedocu.SelectedValue.ToString() + " ");
                    double IVA = Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[6].ToString());
                    string valoriva = String.Format("{0:C}", IVA);
                    ivaLabel.Text = valoriva.ToString();
                    string vendedor = DBFunctions.SingleData("select PVEN_NOMBRE from DBXSCHEMA.PVENDEDOR WHERE PVEN_CODIGO='" + lineas.Tables[0].Rows[i].ItemArray[8].ToString() + "'");
                    vendLabel.Text = vendedor.ToString();
                    string almacen = DBFunctions.SingleData("SELECT PALM_DESCRIPCION from DBXSCHEMA.PALMACEN WHERE tvig_vigencia='V' and PALM_ALMACEN='" + lineas.Tables[0].Rows[i].ItemArray[1].ToString() + "' ");
                    almaLabel.Text = almacen.ToString();
                    double ValorRETE = Convert.ToDouble(lineas.Tables[0].Rows[i].ItemArray[11].ToString());
                    RetLabel.Text = String.Format("{0:C}", ValorRETE);
                    string Usuario = lineas.Tables[0].Rows[i].ItemArray[12].ToString();
                    UsuLabel.Text = Usuario;
                    double total = Valor - ValorRETE + IVA;
                    totalLabel.Text = String.Format("{0:C}", total);
                    double saldo = total - ValorAbon;
                    SaldoLabel.Text = String.Format("{0:C}", saldo);
                }
                                
                DBFunctions.Request(lineas2, IncludeSchema.NO, @" select pdoc_coddocref, ddet_numedocu, CASE WHEN MC.TEST_ESTADODOC = 'N' THEN 0 ELSE ddet_valodocu END, mcaj_fecha, TEST_NOMBRE, ddet_obser, TTIP_NOMBRE AS PAGO
                        from  DBXSCHEMA.DDETALLEFACTURACLIENTE ddf
                             LEFT JOIN DBXSCHEMA.mcaja mc
                                LEFT JOIN TESTADODOCUMENTO T ON MC.TEST_ESTADODOC = T.TEST_ESTADO
                             ON ddf.pdoc_coddocref = mc.pdoc_codigo and ddet_numedocu = mcaj_numero
                                LEFT JOIN MCAJAPAGO MCP ON MC.PDOC_CODIGO = MCP.PDOC_CODIGO AND MC.MCAJ_NUMERO = MCP.MCAJ_NUMERO
                                LEFT JOIN TTIPOPAGO TP ON TP.TTIP_CODIGO = MCP.TTIP_CODIGO
                        where ddf.PDOC_CODIGO = '" + ddlPrefDocumen.SelectedValue.ToString() + "' AND ddf.MFAC_NUMEDOCU=" + ddlNumedocu.SelectedValue.ToString() + ";");

                if (lineas2.Tables.Count > 0)
                {
                    for (int a = 0; a < lineas2.Tables[0].Rows.Count; a++)//			0			1				2			3				4
                    {
                        DataRow fila;
                        fila = resultado.NewRow();
                        fila["DOC"] = lineas2.Tables[0].Rows[a].ItemArray[0].ToString();
                        fila["NUMERO"] = lineas2.Tables[0].Rows[a].ItemArray[1].ToString();
                        fila["VALOR"] = String.Format("{0:C}", lineas2.Tables[0].Rows[a].ItemArray[2]);
                        try
                        {
                            fila["FECHA"] = Convert.ToDateTime(lineas2.Tables[0].Rows[a].ItemArray[3]).ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                        }
                        fila["ESTADO"] = lineas2.Tables[0].Rows[a].ItemArray[4].ToString();
                        fila["OBSERVACIONES"] = lineas2.Tables[0].Rows[a].ItemArray[5].ToString();
                        fila["PAGO"] = lineas2.Tables[0].Rows[0].ItemArray[6].ToString();
                        resultado.Rows.Add(fila);
                    }
                Grid.DataSource = resultado;
                Grid.DataBind();
                }

            }
            else
            {
                Utils.MostrarAlerta(Response, "NO ha seleccionado un Nit o NO ha cargado las facturas del nit especificado o NO ha seleccionado un documento específico a consultar ...!");
            }
            Session["Rep"] = this.Cuerpo_Correo();
        }
		#endregion

		#region Metodos

		public void PrepararTabla()
		{
			resultado = new DataTable();
			resultado.Columns.Add(new DataColumn("DOC",typeof(string)));
			resultado.Columns.Add(new DataColumn("NUMERO",typeof(string)));
			resultado.Columns.Add(new DataColumn("VALOR",typeof(string)));
			resultado.Columns.Add(new DataColumn("FECHA",typeof(string)));
            resultado.Columns.Add(new DataColumn("ESTADO", typeof(string)));
			resultado.Columns.Add(new DataColumn("OBSERVACIONES",typeof(string)));
            resultado.Columns.Add(new DataColumn("PAGO", typeof(string)));
        }

		private void BindDdlPrefDocumen()
		{
			DataTable dtSource = ConsultarPrefijoFacturasRelacionadasNit(hdNitSelec.Value).Tables[0];
			ddlPrefDocumen.DataSource = dtSource;
			ddlPrefDocumen.DataTextField = dtSource.Columns[1].ColumnName;
			ddlPrefDocumen.DataValueField = dtSource.Columns[0].ColumnName;
			ddlPrefDocumen.DataBind();
			ddlPrefDocumen.Items.Insert(0,new ListItem("Seleccione ...",""));
			ddlPrefDocumen.SelectedValue = Request.Form[ddlPrefDocumen.UniqueID];
		}

		private void BindDdlNumedocu()
		{
			DataTable dtSource = ConsultarNumerosFacturaRelacionadasPrefijoNit(hdNitSelec.Value,Request.Form[ddlPrefDocumen.UniqueID]).Tables[0];
			ddlNumedocu.DataSource = dtSource;
			ddlNumedocu.DataValueField = dtSource.Columns[0].ColumnName;
			ddlNumedocu.DataTextField = dtSource.Columns[0].ColumnName;
			ddlNumedocu.DataBind();
			ddlNumedocu.Items.Insert(0,new ListItem("Seleccione ...",""));
			ddlNumedocu.SelectedValue = Request.Form[ddlNumedocu.UniqueID];
		}

		#endregion 

		#region Metodos Ajax

        [Ajax.AjaxMethod]
        protected string validarNitConsulta(Object Sender, EventArgs e)
        {
            if (!Convert.ToBoolean(DBFunctions.SingleData("SELECT MNIT.MNIT_nit from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='" + tbNit + "'")))
            {
                Utils.MostrarAlerta(Response, "El nit especificado NO EXISTE ...!");
                return "NO";
            }
            else
                return "SI";
        }

		[Ajax.AjaxMethod]
		public DataSet ConsultarPrefijoFacturasRelacionadasNit(string nit)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DISTINCT MFC.pdoc_codigo, MFC.pdoc_codigo || '-' || PDO.pdoc_nombre as NOMBRE_DOC FROM mfacturacliente MFC INNER JOIN pdocumento PDO ON MFC.pdoc_codigo = PDO.pdoc_codigo WHERE MFC.mnit_nit='"+nit+"' ORDER BY MFC.pdoc_codigo ASC");
			return ds;
		}

		[Ajax.AjaxMethod]
		public DataSet ConsultarNumerosFacturaRelacionadasPrefijoNit(string nit, string prefijo)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mfac_numedocu as Numero_Factura FROM mfacturacliente WHERE pdoc_codigo='"+prefijo+"' AND mnit_nit='"+nit+"' ORDER BY mfac_numedocu");
			return ds;
		}

		#endregion

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
	}
}