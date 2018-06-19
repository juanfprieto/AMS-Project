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
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using AMS.Contabilidad;
using AMS.Tools;
using System.Globalization;

namespace AMS.Finanzas.Tesoreria
{
    public partial class CausacionNotasBancarias : System.Web.UI.UserControl
    {
        protected DataTable tablaDatos;
        protected Button aceptarValores;
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected string pathToMain = ConfigurationManager.AppSettings["MainIndexPage"];
        protected FormatosDocumentos formatoConsignacion;
        ProceHecEco contaOnline = new ProceHecEco();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                if (almacen.Items.Count > 1)
                    almacen.Items.Insert(0, "Seleccione..");

                bind.PutDatasIntoDropDownList(tipoConsignacion, "SELECT tcau_codigo,tcau_nombre FROM ttipoconsignacion WHERE tcau_codigo = 7");
                if (tipoConsignacion.Items.Count > 0)
                    tipoConsignacion.Items.Insert(0, "Seleccione..");

                //    bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='CS' and tvig_vigencia = 'V' ");
                valorConsignado.Text = totalEfectivo.Text = "$0.00";
                if (Request.QueryString["prefD"] != null && Request.QueryString["numD"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha creado el documento " + Request.QueryString["prefD"] + "-" + Request.QueryString["numD"] + "");
                    try
                    {
                        formatoConsignacion = new FormatosDocumentos();
                        formatoConsignacion.Prefijo = Request.QueryString["prefD"];
                        formatoConsignacion.Numero = Convert.ToInt32(Request.QueryString["numD"]);
                        formatoConsignacion.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefD"] + "'");
                        if (formatoConsignacion.Codigo != string.Empty)
                        {
                            if (formatoConsignacion.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoConsignacion.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoConsignacion.Mensajes + "<br>";
                    }
                }
            }
            // este cargue solo debe aplicarlo unicamente al proceso seleccionado 
            holderNotasBancarias.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.NotasBancarias.ascx"));
        }

        protected void Cambiar_Accion(object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu IN ('ND','NR')"))
            {
                Utils.MostrarAlerta(Response, "No hay documentos de este tipo");
                prefijoDocumento.Enabled = false;
                prefijoDocumento.Items.Clear();
                numeroTesoreria.Text = "";
                aceptar.Enabled = false;
            }
            else
            {
                bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu IN ('ND','NR') and tvig_vigencia = 'V' ");
                numeroTesoreria.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
                prefijoDocumento.Enabled = true;
                aceptar.Enabled = true;
            }
        }
        protected void Cambiar_Prefijo(object Sender, EventArgs e)
        {
            numeroTesoreria.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
        }
        protected void Aceptar_Valores(object Sender, EventArgs e)
        {
            if (tipoConsignacion.SelectedValue == "7")
            {
                holderNotasBancarias.Visible = true;

            }
        }
        protected void Guardar_Accion(object Sender, EventArgs e)
        {
            Control hijo;
            DataTable tablaDatos = new DataTable();
            Consignacion miConsignacion = new Consignacion();
            string usuario = HttpContext.Current.User.Identity.Name.ToString();
            if (detalleTransaccion.Text == "")
                Utils.MostrarAlerta(Response, "Debe especificar un detalle");
            else
            {
                if (tipoConsignacion.SelectedValue == "7")
                {
                    hijo = this.holderNotasBancarias.Controls[0];
                    miConsignacion = new Consignacion();
                    tablaDatos = (DataTable)Session["tablaNotas"];
                    miConsignacion = new Consignacion(tablaDatos);
                    miConsignacion.Almacen = this.almacen.SelectedValue;
                    miConsignacion.CodigoCuenta = ((TextBox)hijo.FindControl("codigoCC")).Text;
                    miConsignacion.Detalle = this.detalleTransaccion.Text;
                    miConsignacion.Fecha = ((TextBox)hijo.FindControl("fecha")).Text;
                    miConsignacion.NumeroTesoreria = Convert.ToInt32(this.numeroTesoreria.Text);
                    miConsignacion.PrefijoDocumento = this.prefijoDocumento.SelectedValue;
                    miConsignacion.Proceso = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    miConsignacion.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
                    //Si es una nota debito
                    double valorConsig = double.Parse(valorConsignado.Text, NumberStyles.Currency);
                    if ((DBFunctions.SingleData("SELECT tdoc_tipodocu FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'")) == "ND")
                        miConsignacion.Total = valorConsig;
                    //Si es una nota credito
                    else if ((DBFunctions.SingleData("SELECT tdoc_tipodocu FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'")) == "NR")
                        miConsignacion.Total = valorConsig * -1;
                    if (miConsignacion.Guardar_Nota())
                    {
                        // contabiizacion ON LINE
                        contaOnline.contabilizarOnline(miConsignacion.PrefijoDocumento.ToString(), Convert.ToInt32(miConsignacion.NumeroTesoreria.ToString()), Convert.ToDateTime(miConsignacion.Fecha), "");
                        lb.Text += miConsignacion.Mensajes;
                        Session.Clear();
                        Response.Redirect(pathToMain + "?process=Tesoreria.ConsignaCheques&prefD=" + miConsignacion.PrefijoDocumento + "&numD=" + miConsignacion.NumeroTesoreria + "");
                    }
                    else
                        lb.Text += miConsignacion.Mensajes;
                }
            }

        }

        protected void Cancelar_Accion(object Sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect(pathToMain + "?process=Tesoreria.ConsignaCheques");
        }

    }
}