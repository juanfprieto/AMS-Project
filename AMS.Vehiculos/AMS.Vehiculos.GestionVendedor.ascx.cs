using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.Tools;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.DB;
using AMS.Web;
using System.Configuration;

namespace AMS.Vehiculos
{
    public partial class AMS_Vehiculos_GestionVendedor : System.Web.UI.UserControl
    {
        
        Usuario usuario = new Usuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                if (Request.QueryString["cod_vend"] != null)
                {
                    bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR WHERE PVEN_CODIGO = '" + Request.QueryString["cod_vend"] + "'");
                    confirmar(sender, e);
                    Session["vendedorActivo"] = "1";
                }
                else
                {
                    ViewState["noProcess"] = true;
                    bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR WHERE PVEN_VIGENCIA = 'V' AND TVEND_CODIGO = 'VV' ORDER BY PVEN_NOMBRE");
                    //ddlVendedor.Items.Add();
                    ddlVendedor.Items.Insert(0, new ListItem("Seleccione...", ""));
                    ddlVendedor.SelectedValue = "";
                }
                lbUsuario.Text = HttpContext.Current.User.Identity.Name.ToLower();
            }
        }

        protected void confirmar(object sender, EventArgs z)
        {
            string cod_vend = Request.QueryString["cod_vend"] == null? "": Request.QueryString["cod_vend"];
            lbMensaje.Text = "";
            string clave = DBFunctions.SingleData("SELECT PVEN_CLAVE FROM DBXSCHEMA.PVENDEDOR WHERE PVEN_CODIGO='" + ddlVendedor.SelectedValue + "'");
            if(txtPassword.Text == clave || cod_vend != "")
            {
                btnConfirmar.Visible = txtPassword.Enabled = ddlVendedor.Enabled = false;
                plhMain.Visible = true;
            }
            else
            {
                lbMensaje.Text = "Constraseña incorrecta";
            }
        }
        protected void cargarMenu(object sender, EventArgs z)
        {
            string proceso = ((Button)sender).ID;
            string url = ConfigurationManager.AppSettings["MainIndexPage"] + "?process=";
            switch (proceso)
            {
                case "btnCotizacion":
                    url += "Vehiculos.SeguimientoDiario&mod=COTIZACION&cod_vend=" + ddlVendedor.SelectedValue;
                    break;
                case "btnSeguimiento":
                    url += "Vehiculos.SeguimientoDiario&mod=CONTACTO&procVendedor=" + ddlVendedor.SelectedValue;
                    break;
                case "btnCreaPedido":
                    url += "Vehiculos.PedidoClientesFormulario&cod_vend=" + ddlVendedor.SelectedValue;
                    break;
                case "btnModificaPedido":
                    url += "Vehiculos.PedidoClientes&cod_vend=" + ddlVendedor.SelectedValue;
                    break;
                case "btnDisponibilidad":
                    url = "AMS.Reportes.FrogReports.aspx?idReporte=302&vendedor=1";
                    break;
                case "btnAsignaciones":
                    url = "AMS.Reportes.FrogReports.aspx?idReporte=303";
                    break;
                case "btnEntregaVehiculos":
                    url += "Vehiculos.EntregaVehiculos&cod_vend=" + ddlVendedor.SelectedValue;
                    break;
                case "brnFacturaProForma":
                    url += "DBManager.Reportes&CodRep=938";
                    break;
                case "btnTerminar":
                    url = "AMS.Web.CerrarSesion.aspx";
                    break;
            }
            if(url.Length > 0)
            {
                //((Button)Parent.Page.FindControl("regresarPrincipalVendedores")).Visible = true;
                if (ViewState["noProcess"] == null)// si es dierente de null quiere decir que entró como administrador, por lo que el botón no se habilita
                    Session["btnRegresarActivo"] = "true";
                Response.Redirect(url);
            }


        }
    }
}