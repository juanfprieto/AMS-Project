using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using System.Configuration;

namespace AMS.Inventarios
{
	public partial class ModificarPedidos : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            DatasToControls bind = new DatasToControls();

            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(this.ddlCodPedido, "SELECT pped_codigo, pped_nombre FROM ppedido ORDER BY 2");
                bind.PutDatasIntoDropDownList(this.ddlNumPedido, "SELECT DISTINCT mped_numepedi FROM dpedidoitem WHERE pped_codigo = '" + ddlCodPedido.SelectedValue + "' AND mped_clasregi = 'C' ORDER BY mped_numepedi DESC");
               
                if(Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                {
                    Tools.Utils.MostrarAlerta(Response, "El pedido: " + Request.QueryString["pref"] + "-" + Request.QueryString["num"] + " ha sido modificado correctamente!");
                }
            }
		}

        protected void CambioPrefijo(Object Sender, EventArgs E)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(this.ddlNumPedido, "SELECT DISTINCT mped_numepedi FROM dpedidoitem WHERE pped_codigo = '" + ddlCodPedido.SelectedValue + "' ORDER BY mped_numepedi DESC");
        }

        protected void ModificarPedido(Object Sender, EventArgs E)
        {
            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            Response.Redirect("" + indexPage + "?process=Inventarios.CrearPedido&actor=M&modificar=1&prefPed=" + ddlCodPedido.SelectedValue + "&numePed=" + ddlNumPedido.SelectedValue);
        }
	}
}