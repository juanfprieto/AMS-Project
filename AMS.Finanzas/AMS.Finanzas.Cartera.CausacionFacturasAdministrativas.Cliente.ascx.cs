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
	public partial class CausacionFacturasEncabezadoCliente : System.Web.UI.UserControl
	{
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
				
		protected void Page_Load(object Sender,EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(CausacionFacturasEncabezadoCliente));
			if(!IsPostBack)
			{
                observacion.Attributes.Add("maxlength", observacion.MaxLength.ToString());
                DatasToControls bind=new DatasToControls();
				//bind.PutDatasIntoDropDownList(prefijoFactura,"SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FC' and PH.tpro_proceso in ('AC') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                Utils.llenarPrefijos(Response, ref prefijoFactura , "AC", "%", "FC");
                if (prefijoFactura.Items.Count > 1) { }
                //    prefijoFactura.Items.Insert(0, "Seleccione..");
                else
                {
                    if (prefijoFactura.Items.Count == 1)
                        numeroFactura.Text = (Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura.SelectedValue + "'"))).ToString();
                    else
                    {
                        if (prefijoFactura.Items.Count == 0)
                            Utils.MostrarAlerta(Response, "Debe configurar documentos y hechos para las facturas de cliente.");
                    }
                }
                bind.PutDatasIntoDropDownList(tipoGasto, "SELECT ttip_codigo,ttip_descripcion FROM ttipogasto order by ttip_descripcion");
                bind.PutDatasIntoDropDownList(almacen,   "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                if (tipoGasto.Items.Count > 1)
                    tipoGasto.Items.Insert(0, "Seleccione..");
                if (almacen.Items.Count > 1)
                    almacen.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(vendedor, "SELECT pven_codigo,pven_nombre FROM pvendedor where tvend_codigo in ('VV','VM','RT','TT') AND PVEN_VIGENCIA = 'V' ORDER BY PVEN_NOMBRE");
                if (vendedor.Items.Count > 1)
                    vendedor.Items.Insert(0, "Seleccione..");
              	fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
			}
			else
			{
            //    if (tipoGasto.Items[0].Text != "Seleccione..")
            //        tipoGasto.Items.Insert(0, new ListItem("Seleccione.."));
            }
		}

        [Ajax.AjaxMethod()]
        public string Consultar(string nit)
        {
            string numero = "";
            numero = DBFunctions.SingleData("SELECT MCLI_DIASPLAZ FROM MCLIENTE WHERE MNIT_NIT = '" + nit + "'");
            if (numero == "")
                numero = "0";
            return numero;
        }
		
        //[Ajax.AjaxMethod]
        //public string Cambio_Numero(string prefijo)
        //{
        //    string numero="";
        //    numero=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
        //    return numero;
        //}

        protected void CambioPrefijo(object Sender, EventArgs e)
		{
            string numero="";
            string prefijo = prefijoFactura.SelectedValue;

            numero = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'");

            numeroFactura.Text = numero;
        }

        [Ajax.AjaxMethod]
        public DataSet carga_Nombre(string nit)
        {
            DataSet ds = new DataSet();

            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT Mnit.mnit_nit as nit, MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.mnit_apellido2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') AS NOMBRE FROM dbxschema.mnit MNIT WHERE MNIT.tvig_vigencia = 'V' and mnit_nit = '" + nit + "' ORDER BY MNIT_APELLIDOS;'");
            return ds;
        }

        protected void Cambiar_Gasto(object Sender, EventArgs e)
        {
            //primero validamos cierre mensual, fuera de la fecha de vigencia no se permite ningún proceso
            /*if(!Tools.General.validarCierreFinanzas(fecha.Text, "C"))
            {
                Utils.MostrarAlerta(Response, "La fecha escrita no corresponde a la vigencia del sistema de cartera. Por favor revise.");
                return;
            }*/

            validatorNumero.Visible = false;
            validatorNit.Visible = false;

            if (numeroFactura.Text == "") validatorNumero.Visible = true;
            if (nit.Text == "") validatorNit.Visible = true;
            Control padre = (this.Parent).Parent;
            if (ExisteCliente(nit.Text)
                && ExisteObservacion()
                //&& ValidarFecha() 
                && ExisteVendedor(vendedor.Text)
                && tipoGasto.SelectedValue != "Seleccione.."
                && Tools.General.validarCierreFinanzas(fecha.Text, "C"))
            {
                ((Button)padre.FindControl("btnCargaGastos")).Visible = false;
                ((Panel)padre.FindControl("pnlDet")).Visible = true;
                ((Label)padre.FindControl("lbAlmacen")).Text = "";

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
                    ((DataGrid)padre.FindControl("gridActivosFijos")).DataSource = ((DataTable)Session["tablaActivosFijos"]);
                    ((DataGrid)padre.FindControl("gridActivosFijos")).DataBind();
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
                    ((DataGrid)padre.FindControl("gridDiferidos")).DataSource = ((DataTable)Session["tablaDiferidos"]);
                    ((DataGrid)padre.FindControl("gridDiferidos")).DataBind();
                    ((DataGrid)padre.FindControl("gridNC")).Visible = false;
                }
                else if (tipoGasto.SelectedValue == "3")
                {
                    ((HtmlGenericControl)padre.FindControl("fldActivosFijos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDiferidos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDetalles")).Visible = true;
                    ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = false;
                }
                else if (tipoGasto.SelectedValue == "4")
                {
                    ((HtmlGenericControl)padre.FindControl("fldActivosFijos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDiferidos")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldDetalles")).Visible = false;
                    ((HtmlGenericControl)padre.FindControl("fldNC")).Visible = true;
                    //((Button)padre.FindControl("btnCargaGastos")).Visible = true;
                    //((Label)padre.FindControl("lbAlmacen")).Text = almacen.SelectedValue + " - " + almacen.SelectedItem;
                    ((Label)padre.FindControl("lbInfoImp")).ForeColor = Color.Navy;
                    ((Label)padre.FindControl("lbInfoImp")).Text = "Por favor digite todos los renglones que haran parte del detalle de la factura, no digite el renglón de la cuenta por pagar";

                    ((DataGrid)padre.FindControl("gridNC")).DataSource = ((DataTable)Session["tablaNC"]);
                    ((DataGrid)padre.FindControl("gridNC")).DataBind();
                }

                ((DataGrid)padre.FindControl("gridRtns")).DataSource = ((DataTable)Session["tablaRtns"]);
                ((DataGrid)padre.FindControl("gridRtns")).DataBind();
                ((DataGrid)padre.FindControl("dgIva")).DataSource = ((DataTable)Session["dtIva"]);
                ((DataGrid)padre.FindControl("dgIva")).DataBind();


                prefijoFactura.Enabled = false; numeroFactura.Enabled = false; fecha.Enabled = false;
                nit.Enabled = false; almacen.Enabled = false; vendedor.Enabled = false;
                tbdiasPlazo.Enabled = false; observacion.Enabled = false; tipoGasto.Enabled = false;
                btnAceptar.Enabled = false;
            }
            else
            {
                string mensajeError = "Ocurrio uno de los siguientes errores:";
                if (!ExisteCliente(nit.Text))
                    mensajeError += "\\n1. El nit digitado no existe en el maestro de Clientes.";
                if (!ExisteObservacion())
                    mensajeError += "\\n2. La observación esta vacia.";
                if (!ExisteVendedor(vendedor.Text))
                    mensajeError += "\\n3. Los días de plazo son negativos o inválidos.";
                if(!Tools.General.validarCierreFinanzas(fecha.Text, "C"))
                     mensajeError += "\\n4. La fecha de la factura es inválida o el año no se encuentra registrado. \\n5. La fecha de la factura es menor a la Vigente en Cartera.";
                if(tipoGasto.SelectedValue == "Seleccione..")
                    mensajeError += "\\n6. No ha seleccionado un tipo de Ingreso.";
                mensajeError += "\\ Imposible continuar el proceso.";
                Utils.MostrarAlerta(Response, mensajeError);
            }
        }

		private bool ExisteCliente(string nit)
		{
			if(DBFunctions.RecordExist("SELECT mnit_nit FROM mNIT WHERE mnit_nit='"+nit+"'"))
                return true;  // se permite emitir facturar a todos los nits
			else 
                return false;
        }

        private bool ExisteVendedor(string vendedor)
        {
            if (DBFunctions.RecordExist("SELECT pven_codigo FROM pvendedor WHERE pven_codigo = '" + vendedor + "'"))
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
			//Si la fecha es invalida
			if(!DatasToControls.ValidarDateTime(fecha.Text))
				return false;
			//Si los dias de plazo son invalidos o menores a cero
			else if(!DatasToControls.ValidarInt(tbdiasPlazo.Text) || Convert.ToInt32(tbdiasPlazo.Text) < 0)
				return false;
			//Si la fecha de la factura es menor a la fecha de vencimiento (fecha de factura mas dias de plazo)
			else if(Convert.ToDateTime(fecha.Text) > (Convert.ToDateTime(fecha.Text).AddDays(Convert.ToInt32(tbdiasPlazo.Text))))
				return false;
            else
            // La fecha del documento NO puede ser menor a la veigencia contable
            if (String.Compare(fecha.Text.Substring(0, 7), DBFunctions.SingleData("SELECT CASE WHEN PMES_MES <10 THEN PANO_ANO||'-0'||PMES_MES ELSE PANO_ANO||'-'||PMES_MES END  FROM CCONTABILIDAD")) < 0)
                return false;
			else
            { 
                DateTime fechaA = Convert.ToDateTime(fecha.Text);
                return DBFunctions.RecordExist("SELECT PANO_ANO FROM PANO WHERE PANO_ANO=" + fechaA.Year + ";");
            }
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
