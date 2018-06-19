using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Ajax;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Cartera
{
	public partial class CausacionFacturasEncabezadoProveedor : System.Web.UI.UserControl
	{
		protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(CausacionFacturasEncabezadoProveedor));
			if(!IsPostBack)
			{
                observacion.Attributes.Add("maxlength", observacion.MaxLength.ToString());
                DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FP' and PH.tpro_proceso in ('AP') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                Utils.llenarPrefijos(Response, ref prefijoFactura  , "AP", "%", "FP");
           //     if (prefijoFactura.Items.Count > 0)
           //         prefijoFactura.Items.Insert(0, "Seleccione..");   
                bind.PutDatasIntoDropDownList(tipoGasto,"SELECT ttip_codigo,ttip_descripcion FROM ttipogasto order by 2");
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                if (tipoGasto.Items.Count > 1)
                    tipoGasto.Items.Insert(0, "Seleccione.."); 
                if (almacen.Items.Count > 1)
                    almacen.Items.Insert(0, "Seleccione..");
                if (prefijoFactura.Items.Count > 0 && Utils.EstaSeleccionado(prefijoFactura))
					numeroFactura.Text=Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue+"'")).ToString();

				fecha.Text=System.DateTime.Now.Date.ToString("yyyy-MM-dd");
			}
			else
			{
            //    if (tipoGasto.Items[0].Text != "Seleccione..")
            //        tipoGasto.Items.Insert(0, new ListItem("Seleccione.."));
            //
            }
		}
		
		[Ajax.AjaxMethod]
		public string Cambio_Numero(string prefijo)
		{
			string numero="";
			numero=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
			return numero;
		}


        [Ajax.AjaxMethod()]
        public string ConsultarFecha(string nitFEc, string nit)
        {
            string numero = "";
            DateTime fechaI = Convert.ToDateTime(nitFEc);
            numero = DBFunctions.SingleData("SELECT coalesce(MPRO_DIASPLAZO,0) FROM MPROVEEDOR WHERE MNIT_NIT = '" + nit + "'");
            if (numero == "" || numero == null)
                numero = "0";
            string fechaTotal = (fechaI.AddDays(Convert.ToDouble(numero))).ToString("yyyy-MM-dd");
            return fechaTotal;
        }

        [Ajax.AjaxMethod]
        public DataSet Cargar_Nombre(string Cedula)
        {
            DataSet Vins = new DataSet();
            DBFunctions.Request(Vins, IncludeSchema.NO, "select mnit_nit from dbxschema.mnit where mnit_nit like '" + Cedula + "%';select mnit_nombres concat ' ' CONCAT COALESCE(mnit_nombre2,'') concat ' 'concat mnit_apellidos concat ' 'concat COALESCE(mnit_apellido2,'') as NOMBRE from dbxschema.mnit where mnit_nit='" + Cedula + "'");
            return Vins;

        }
        protected void Cambiar_Gasto(object Sender, EventArgs e)
        {
            validaNumero.Visible = false;
            if (numeroFactura.Text == "") validaNumero.Visible = true;
            Control padre = (this.Parent).Parent;

            if (ExisteProveedor(nit.Text) && ExisteObservacion() && ValidarFecha() && prefijoProveedor.Text != "" && noExisteFacturaProveedor() && numeroFacturaProv(numeroProveedor.Text) && tipoGasto.SelectedValue != "Seleccione.." && almacen.SelectedValue != "Seleccione..")
            {
                ((Button)padre.FindControl("btnCargaGastos")).Visible = false;
                ((Label)padre.FindControl("lbAlmacen")).Text = "";

                ((Panel)padre.FindControl("pnlDet")).Visible = true;
                if (tipoGasto.SelectedValue == "1" || tipoGasto.SelectedValue == "5")
                {
                    ((HtmlGenericControl)padre.FindControl("fldActivosFijos")).Visible = true;
                    ((HtmlGenericControl)padre.FindControl("fldDiferidos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDetalles")).Visible = false;
                    if (tipoGasto.SelectedValue == "5")
                    {
                        ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = false;
                    }
                    else
                    {
                        ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = true;
                    }
                    //((DataGrid)padre.FindControl("gridActivosFijos")).Visible = true;
                    //((DataGrid)padre.FindControl("gridDiferidos")).Visible = false;
                    //((DataGrid)padre.FindControl("gridDetalles")).Visible = false;
                    ((DataGrid)padre.FindControl("gridActivosFijos")).DataSource = ((DataTable)Session["tablaActivosFijos"]);
                    ((DataGrid)padre.FindControl("gridActivosFijos")).DataBind();
                    //((DataGrid)padre.FindControl("gridNC")).Visible = true;
                    ((DataGrid)padre.FindControl("gridNC")).DataSource = ((DataTable)Session["tablaNC"]);
                    ((DataGrid)padre.FindControl("gridNC")).DataBind();
                    ((DataGrid)padre.FindControl("gridRtns")).Visible = true;
                    ((DataGrid)padre.FindControl("dgIva")).Visible = true;
                    ((Panel)padre.FindControl("pnlValores")).Visible = true;
                }
                else if (tipoGasto.SelectedValue == "2")
                {

                    ((HtmlGenericControl)padre.FindControl("fldActivosFijos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDiferidos")).Visible = true;
                    ((HtmlGenericControl)padre.FindControl("fldDetalles")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = false;

                    //((DataGrid)padre.FindControl("gridDiferidos")).Visible = true;
                    //((DataGrid)padre.FindControl("gridActivosFijos")).Visible = false;
                    //((DataGrid)padre.FindControl("gridDetalles")).Visible = false;
                    ((DataGrid)padre.FindControl("gridDiferidos")).DataSource = ((DataTable)Session["tablaDiferidos"]);
                    ((DataGrid)padre.FindControl("gridDiferidos")).DataBind();
                    //((DataGrid)padre.FindControl("gridNC")).Visible = false;
                }
                else if (tipoGasto.SelectedValue == "3")
                {
                    ((HtmlGenericControl)padre.FindControl("fldActivosFijos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDiferidos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDetalles")).Visible = true;
                    ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = false;

                    //((DataGrid)padre.FindControl("gridDetalles")).Visible = true;
                    //((DataGrid)padre.FindControl("gridActivosFijos")).Visible = false;
                    //((DataGrid)padre.FindControl("gridDiferidos")).Visible = false;
                    //((DataGrid)padre.FindControl("gridNC")).Visible = false;
                }
                else if (tipoGasto.SelectedValue == "4")
                {
                    ((HtmlGenericControl)padre.FindControl("fldActivosFijos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDiferidos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDetalles")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = true;
                    ((Button)padre.FindControl("btnCargaGastos")).Visible = true;
                    ((Label)padre.FindControl("lbAlmacen")).Text = almacen.SelectedValue + " - " + almacen.SelectedItem;

                    //((DataGrid)padre.FindControl("gridDetalles")).Visible = false;
                    //((DataGrid)padre.FindControl("gridActivosFijos")).Visible = false;
                    //((DataGrid)padre.FindControl("gridDiferidos")).Visible = false;
                    ((Label)padre.FindControl("lbInfoImp")).ForeColor = Color.Navy;
                    ((Label)padre.FindControl("lbInfoImp")).Text = "Por favor digite todos los renglones que haran parte del detalle de la factura, no digite el renglón de la cuenta por pagar";
                    //((DataGrid)padre.FindControl("gridNC")).Visible = true;
                    ((DataGrid)padre.FindControl("gridNC")).DataSource = ((DataTable)Session["tablaNC"]);
                    ((DataGrid)padre.FindControl("gridNC")).DataBind();
                }
                ((DataGrid)padre.FindControl("gridRtns")).DataSource = ((DataTable)Session["tablaRtns"]);
                ((DataGrid)padre.FindControl("gridRtns")).DataBind();
                ((DataGrid)padre.FindControl("dgIva")).DataSource = ((DataTable)Session["dtIva"]);
                ((DataGrid)padre.FindControl("dgIva")).DataBind();

                prefijoFactura.Enabled = false; numeroFactura.Enabled = false; fecha.Enabled = false;
                tbFecVen.Enabled = false; nit.Enabled = false; almacen.Enabled = false;
                prefijoProveedor.Enabled = false; numeroProveedor.Enabled = false;
                observacion.Enabled = tipoGasto.Enabled = false; btnAceptar.Enabled = false;
            }
            else
            {
                string detError = "";
                if (!ExisteProveedor(nit.Text))
                    detError = "\\n1. El nit digitado no existe en el maestro de Proveedores de la empresa.";
                if (!ExisteObservacion())
                    detError += "\\n2. La observación esta vacia.";
                if (!ValidarFecha())
                    detError += "\\n3. La fecha de la factura o la fecha de vencimiento son inválidas o el año no se encuentra registrado aún o la fecha es menor a la vigencia Contable";
                if (!noExisteFacturaProveedor())
                    detError += "\\n4. El nit-prefijo-numero de proveedor ya está registrado";
                if (prefijoProveedor.Text == "")
                    detError += "\\n5. El Prefijo de Factura del proveedor está vacio";
                if (!numeroFacturaProv(numeroProveedor.Text))
                    detError += "\\n6. El número de Factura del proveedor está errado";
                if (almacen.SelectedValue == "Seleccione..")
                    detError += "\\n7. NO ha seleccionado el Almacén"; 
                if (tipoGasto.SelectedValue == "Seleccione..")
                    detError += "\\n8. NO ha seleccionado el tipo de Gasto";
                 
                Utils.MostrarAlerta(Response, "Ocurrio uno de los siguientes errores. " + detError + "   \\n Imposible continuar el proceso.");
            }
        }

        private bool ExisteProveedor(string nit)
		{
			if(DBFunctions.RecordExist("SELECT mnit_nit FROM mproveedor WHERE mnit_nit='"+nit+"'"))
				return true;
			else
				return false;
		}

        private bool numeroFacturaProv(string numeroProveedor )
		{
            long number1 = 0;
            bool canConvert = long.TryParse(numeroProveedor, out number1);
            return canConvert;
        }

        private bool ExisteVendedor(string nit)
        {
            if (DBFunctions.RecordExist("SELECT mnit_nit FROM mproveedor WHERE mnit_nit='" + nit + "'"))
                return true;
            else
                return false;
        }

		private bool ExisteObservacion()
		{
			if(observacion.Text!=string.Empty)
				return true;
			else
				return false;
		}

		private bool ValidarFecha()
		{
			if(!DatasToControls.ValidarDateTime(fecha.Text) || !DatasToControls.ValidarDateTime(tbFecVen.Text))
				return false;
			else 
                if(Convert.ToDateTime(fecha.Text) > Convert.ToDateTime(tbFecVen.Text))
				    return false;
                else  // La fecha del documento NO puede ser menor a la veigencia contable
                    if (String.Compare(fecha.Text.Substring(0,7),DBFunctions.SingleData("SELECT CASE WHEN PMES_MES <10 THEN PANO_ANO||'-0'||PMES_MES ELSE PANO_ANO||'-'||PMES_MES END  FROM CCONTABILIDAD")) < 0)
                        return false;
                    else
                    {
                        DateTime fechaA = Convert.ToDateTime(fecha.Text);
                        DateTime fechaV = Convert.ToDateTime(tbFecVen.Text);
                        return (DBFunctions.RecordExist("SELECT PANO_ANO FROM PANO WHERE PANO_ANO=" + fechaA.Year + ";") && DBFunctions.RecordExist("SELECT PANO_ANO FROM PANO WHERE PANO_ANO=" + fechaV.Year + ";"));
                    }
		}

        private bool noExisteFacturaProveedor()
        {
           
            string existeFactura = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturaproveedor WHERE mnit_nit='" + nit.Text + "' and mfac_prefdocu = '" + prefijoProveedor.Text + "' and mfac_numedocu = " + numeroProveedor.Text + " ");
            if (existeFactura == "" || existeFactura == null)
                return true;
            else
                return false;
        }
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
