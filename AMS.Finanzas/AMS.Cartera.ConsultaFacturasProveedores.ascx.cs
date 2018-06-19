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

namespace AMS.Finanzas
{
	public partial class AMS_Cartera_ConsultaFacturasProveedores : System.Web.UI.UserControl
	{
		#region Propiedades
		private DataTable resultado;
		private DataSet lineas;
		private DataSet lineas2;
		//JFSC 11022008 Poner en comentario
		//private DataSet lineaced;
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Cartera_ConsultaFacturasProveedores));
			if(!IsPostBack)
			{
				tbNit.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT DISTINCT MFP.mnit_nit AS NIT, MNI.mnit_apellidos || \\' \\' || MNI.mnit_apellido2 || \\' \\' || MNI.mnit_nombres || \\' \\' || MNI.mnit_nombre2 AS NOMBRE "+
					" FROM mfacturaproveedor MFP INNER JOIN mnit MNI ON MFP.mnit_nit = MNI.mnit_nit "+
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
		}

        protected string Cuerpo_Correo()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            consultasFacturas.RenderControl(htmlTW);
            return SB.ToString();
        }

        protected void Buscar_Click(object sender, System.EventArgs e)
		{
            BindDdlPrefDocumen();
            BindDdlNumedocu();

            CargarDatos("E");
            //DatasToControls bind = new DatasToControls();
            //Panel1.Visible = true;
            //PrepararTabla();
            //lineas = new DataSet();
            //lineas2 = new DataSet();
            //DBFunctions.Request(lineas, IncludeSchema.NO, "SELECT mfp.mnit_nit,mfp.palm_almacen,mfp.tvig_vigencia,mfp.mfac_factura,mfp.mfac_pago,mfp.mfac_valofact,mfp.mfac_valoiva,mfp.mfac_valoabon,mfp.mfac_vence,mfp.mfac_valoflet,mfp.mfac_valorete,mfp.susu_usuario,mfp.mfac_valoivaflet FROM mfacturaproveedor mfp, mnit mn WHERE mfp.mnit_nit = mn.mnit_nit AND pdoc_codiordepago='" + ddlPrefDocumen.SelectedValue + "' AND mfac_numeordepago=" + ddlNumedocu.SelectedValue);
            //for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
            //{
            //    lbDocumento.Text = ddlPrefDocumen.SelectedValue + " " + ddlNumedocu.SelectedValue;
            //    lbFecha.Text     = Convert.ToDateTime(lineas.Tables[0].Rows[i][3]).ToString("yyyy-MM-dd"); 
            //    lbNitNombre.Text = lineas.Tables[0].Rows[i][0]+" "+DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from MNIT WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i][0]+"'");
            //    lbDireccion.Text = DBFunctions.SingleData("SELECT MNIT_DIRECCION from MNIT WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i][0]+"'");
            //    lbTelefono.Text  = DBFunctions.SingleData("SELECT MNIT_TELEFONO from MNIT WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i][0]+"'"); 
            //    lbCelular.Text   = DBFunctions.SingleData("SELECT MNIT_CELULAR from MNIT WHERE MNIT.MNIT_NIT='"+lineas.Tables[0].Rows[i][0]+"'");
            //    lbEstado.Text    = DBFunctions.SingleData("select TVIG_NOMBVIGE from TVIGENCIA WHERE TVIG_VIGENCIA='"+lineas.Tables[0].Rows[i][2]+"'");
            //    lbVenc.Text      = Convert.ToDateTime(lineas.Tables[0].Rows[i][8]).ToString("yyyy-MM-dd");
            //    lbValorDocumento.Text = (Convert.ToDouble(lineas.Tables[0].Rows[i][5])+Convert.ToDouble(lineas.Tables[0].Rows[i][9])).ToString("C");
            //    lbValAbon.Text   = Convert.ToDouble(lineas.Tables[0].Rows[i][7]).ToString("C");
            //    lbIva.Text       = (Convert.ToDouble(lineas.Tables[0].Rows[i][6])+Convert.ToDouble(lineas.Tables[0].Rows[i][12])).ToString("C");
            //    lbAlmacen.Text = DBFunctions.SingleData("SELECT PALM_DESCRIPCION from PALMACEN WHERE tvig_vigencia='V' and PALM_ALMACEN='" + lineas.Tables[0].Rows[i][1] + "'");
            //    lbRetencion.Text = Convert.ToDouble(lineas.Tables[0].Rows[i][10]).ToString("C");
            //    lbUsuario.Text   = lineas.Tables[0].Rows[i][11].ToString();
            //    lbTotFact.Text   = (Convert.ToDouble(lineas.Tables[0].Rows[i][5]) + Convert.ToDouble(lineas.Tables[0].Rows[i][9]) + Convert.ToDouble(lineas.Tables[0].Rows[i][12])  - Convert.ToDouble(lineas.Tables[0].Rows[i][10]) + Convert.ToDouble(lineas.Tables[0].Rows[i][6].ToString())).ToString("C");
            //    lbSaldo.Text     = ((Convert.ToDouble(lineas.Tables[0].Rows[i][5]) + Convert.ToDouble(lineas.Tables[0].Rows[i][9]) + Convert.ToDouble(lineas.Tables[0].Rows[i][12]) - Convert.ToDouble(lineas.Tables[0].Rows[i][10]) + Convert.ToDouble(lineas.Tables[0].Rows[i][6].ToString())) - Convert.ToDouble(lineas.Tables[0].Rows[i][7])).ToString("C");
            //}
            //DBFunctions.Request(lineas2,IncludeSchema.NO,"SELECT DCP.pdoc_codigo as DOC,DCP.mcaj_numero as NUMERO,DCP.dcaj_valorecicaja as VALOR,MCJ.mcaj_fecha as FECHA,MCJ.mcaj_razon as OBSERVACIONES FROM dcajaproveedor DCP INNER JOIN mcaja MCJ ON DCP.pdoc_codigo = MCJ.pdoc_codigo AND DCP.mcaj_numero = MCJ.mcaj_numero WHERE DCP.pdoc_codiordepago='"+ddlPrefDocumen.SelectedValue+"' AND DCP.mfac_numeordepago="+ddlNumedocu.SelectedValue+"");
            //dgPagos.DataSource = lineas2.Tables[0];
            //dgPagos.DataBind();
		}

        protected void Buscar_ClickProve(object sender, System.EventArgs e)
        {
            if (txtPrefProv.Text == "" || txtNumeProv.Text == "")
            {
                Utils.MostrarAlerta(Response, "Debe digitar el prefíjo y número correspondientes al proveedor, para realizar esta busqueda!");
                return;
            }
            else 
            {
                BindDdlPrefDocumen();
                CargarDatos("P");
            }
            
        }

        public void CargarDatos(string tipo)
        {
            DatasToControls bind = new DatasToControls();
            Panel1.Visible = true;
            PrepararTabla();
            lineas = new DataSet();
            lineas2 = new DataSet();

            if(tipo == "E") //entrada
                DBFunctions.Request(lineas, IncludeSchema.NO, "SELECT mfp.mnit_nit,mfp.palm_almacen,mfp.tvig_vigencia,mfp.mfac_factura,mfp.mfac_pago,mfp.mfac_valofact,mfp.mfac_valoiva,mfp.mfac_valoabon,mfp.mfac_vence,mfp.mfac_valoflet,mfp.mfac_valorete,mfp.susu_usuario,mfp.mfac_valoivaflet, mfp.mfac_prefdocu, mfp.mfac_numedocu, mfp.pdoc_codiordepago, mfp.mfac_numeordepago FROM mfacturaproveedor mfp, mnit mn WHERE mfp.mnit_nit = mn.mnit_nit AND pdoc_codiordepago='" + ddlPrefDocumen.SelectedValue + "' AND mfac_numeordepago=" + ddlNumedocu.SelectedValue + " AND mfp.mnit_nit ='" + tbNit.Text + "' ");
            else //proveedor
                DBFunctions.Request(lineas, IncludeSchema.NO, "SELECT mfp.mnit_nit,mfp.palm_almacen,mfp.tvig_vigencia,mfp.mfac_factura,mfp.mfac_pago,mfp.mfac_valofact,mfp.mfac_valoiva,mfp.mfac_valoabon,mfp.mfac_vence,mfp.mfac_valoflet,mfp.mfac_valorete,mfp.susu_usuario,mfp.mfac_valoivaflet, mfp.mfac_prefdocu, mfp.mfac_numedocu, mfp.pdoc_codiordepago, mfp.mfac_numeordepago FROM mfacturaproveedor mfp, mnit mn WHERE mfp.mnit_nit = mn.mnit_nit AND mfac_prefdocu='" + txtPrefProv.Text + "' AND mfac_numedocu=" + txtNumeProv.Text + " AND mfp.mnit_nit ='" + tbNit.Text + "' ");

            if (lineas.Tables.Count == 0)
            {
                Utils.MostrarAlerta(Response, "El prefijo y número de proveedor no coinciden con ningún registro! Por favor verifique.");
                return;
            }

            if (lineas.Tables.Count > 0)
            {
                for (int i = 0; i < lineas.Tables[0].Rows.Count; i++)
                {
                    lbDocumento.Text = lineas.Tables[0].Rows[i][15].ToString() + "-" + lineas.Tables[0].Rows[i][16].ToString() + " / " + lineas.Tables[0].Rows[i][13].ToString() + "-" + lineas.Tables[0].Rows[i][14].ToString();
                    string detalle = DBFunctions.SingleData("select COALESCE(mfac_observacion,'') ||' '|| COALESCE(SUSU_USUARIO,'') from mfacturaPROVEEDOR where pdoc_codiORDEPAgo = '" + ddlPrefDocumen.SelectedValue.ToString() + "' and mfac_numeORDEPAGO = " + ddlNumedocu.SelectedValue.ToString() + " ");
                    detalleLabel.Text =  detalle.ToString();
                    lbFecha.Text = Convert.ToDateTime(lineas.Tables[0].Rows[i][3]).ToString("yyyy-MM-dd");
                    lbNitNombre.Text = lineas.Tables[0].Rows[i][0] + " " + DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT MNIT.MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 from MNIT WHERE MNIT.MNIT_NIT='" + lineas.Tables[0].Rows[i][0] + "'");
                    lbDireccion.Text = DBFunctions.SingleData("SELECT MNIT_DIRECCION from MNIT WHERE MNIT.MNIT_NIT='" + lineas.Tables[0].Rows[i][0] + "'");
                    lbTelefono.Text = DBFunctions.SingleData("SELECT MNIT_TELEFONO from MNIT WHERE MNIT.MNIT_NIT='" + lineas.Tables[0].Rows[i][0] + "'");
                    lbCelular.Text = DBFunctions.SingleData("SELECT MNIT_CELULAR from MNIT WHERE MNIT.MNIT_NIT='" + lineas.Tables[0].Rows[i][0] + "'");
                    lbEstado.Text = DBFunctions.SingleData("select TVIG_NOMBVIGE from TVIGENCIA WHERE TVIG_VIGENCIA='" + lineas.Tables[0].Rows[i][2] + "'");
                    lbVenc.Text = Convert.ToDateTime(lineas.Tables[0].Rows[i][8]).ToString("yyyy-MM-dd");
                    lbValorDocumento.Text = (Convert.ToDouble(lineas.Tables[0].Rows[i][5]) + Convert.ToDouble(lineas.Tables[0].Rows[i][9])).ToString("C");
                    lbValAbon.Text = Convert.ToDouble(lineas.Tables[0].Rows[i][7]).ToString("C");
                    lbIva.Text = (Convert.ToDouble(lineas.Tables[0].Rows[i][6]) + Convert.ToDouble(lineas.Tables[0].Rows[i][12])).ToString("C");
                    lbAlmacen.Text = DBFunctions.SingleData("SELECT PALM_DESCRIPCION from PALMACEN WHERE tvig_vigencia='V' and PALM_ALMACEN='" + lineas.Tables[0].Rows[i][1] + "'");
                    lbRetencion.Text = Convert.ToDouble(lineas.Tables[0].Rows[i][10]).ToString("C");
                    lbUsuario.Text = lineas.Tables[0].Rows[i][11].ToString();
                    lbTotFact.Text = (Convert.ToDouble(lineas.Tables[0].Rows[i][5]) + Convert.ToDouble(lineas.Tables[0].Rows[i][9]) + Convert.ToDouble(lineas.Tables[0].Rows[i][12]) - Convert.ToDouble(lineas.Tables[0].Rows[i][10]) + Convert.ToDouble(lineas.Tables[0].Rows[i][6].ToString())).ToString("C");
                    lbSaldo.Text = ((Convert.ToDouble(lineas.Tables[0].Rows[i][5]) + Convert.ToDouble(lineas.Tables[0].Rows[i][9]) + Convert.ToDouble(lineas.Tables[0].Rows[i][12]) - Convert.ToDouble(lineas.Tables[0].Rows[i][10]) + Convert.ToDouble(lineas.Tables[0].Rows[i][6].ToString())) - Convert.ToDouble(lineas.Tables[0].Rows[i][7])).ToString("C");

                    ddlPrefDocumen.SelectedValue = lineas.Tables[0].Rows[i][15].ToString();
                    BindDdlNumedocu();
                    ddlNumedocu.SelectedValue = lineas.Tables[0].Rows[i][16].ToString();
                }
            }

            

            string prefijo = "";
            string numero = "";
            if (tipo == "E") //entrada
            {
                prefijo = ddlPrefDocumen.SelectedValue;
                numero = ddlNumedocu.SelectedValue;
            }
            else //proveedor
            {
                DataSet dsFactura = new DataSet();
                DBFunctions.Request(dsFactura, IncludeSchema.NO, "select pdoc_codiordepago,  mfac_numeordepago FROM mfacturaproveedor where  mfac_prefdocu='" + txtPrefProv.Text + "' and mfac_numedocu=" + txtNumeProv.Text + ";");
                if (dsFactura.Tables[0].Rows.Count > 0)
                {
                    prefijo = dsFactura.Tables[0].Rows[0][0].ToString();
                    numero = dsFactura.Tables[0].Rows[0][1].ToString();
                }

            }

            DBFunctions.Request(lineas2, IncludeSchema.NO, "SELECT DCP.pdoc_codigo as DOC,DCP.mcaj_numero as NUMERO,DCP.dcaj_valorecicaja as VALOR,MCJ.mcaj_fecha as FECHA,MCJ.mcaj_razon as OBSERVACIONES FROM dcajaproveedor DCP INNER JOIN mcaja MCJ ON DCP.pdoc_codigo = MCJ.pdoc_codigo AND DCP.mcaj_numero = MCJ.mcaj_numero WHERE DCP.pdoc_codiordepago='" + prefijo + "' AND DCP.mfac_numeordepago=" + numero + "");

            dgPagos.DataSource = lineas2.Tables[0];
            dgPagos.DataBind();

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
			resultado.Columns.Add(new DataColumn("OBSERVACIONES",typeof(string)));
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
            string prefijoDoc = "";
            if (Request.Form[ddlPrefDocumen.UniqueID] == "")
                prefijoDoc = ddlPrefDocumen.SelectedValue;
            else
                prefijoDoc = Request.Form[ddlPrefDocumen.UniqueID];
            DataTable dtSource = ConsultarNumerosFacturaRelacionadasPrefijoNit(hdNitSelec.Value, prefijoDoc).Tables[0];
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
		public DataSet ConsultarPrefijoFacturasRelacionadasNit(string nit)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DISTINCT MFP.pdoc_codiordepago as PDOC_CODIGO,MFP.pdoc_codiordepago || '-' || PDO.pdoc_nombre as NOMBRE_DOC FROM mfacturaproveedor MFP INNER JOIN pdocumento PDO ON MFP.pdoc_codiordepago = PDO.pdoc_codigo WHERE MFP.mnit_nit='"+nit+"' ORDER BY MFP.pdoc_codiordepago ASC");
			return ds;
		}

		[Ajax.AjaxMethod]
		public DataSet ConsultarNumerosFacturaRelacionadasPrefijoNit(string nit, string prefijo)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mfac_numeordepago as Numero_Factura FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijo+"' AND mnit_nit='"+nit+"' ORDER BY mfac_numeordepago");
			return ds;
		}

        [Ajax.AjaxMethod]
        public DataSet CargarFacturaProveedor(string nit, string prefijo, string numero)
        {
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_prefdocu as PREF_PROV, mfac_numedocu as NUME_PROV FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefijo + "' and mfac_numeordepago=" + numero + " AND mnit_nit='" + nit + "' ORDER BY mfac_numeordepago;");
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
