using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Documentos
{
	public partial class CausalDevolucion : System.Web.UI.UserControl
	{
        private CausalDevolucionLogic logic;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                int callerType = Convert.ToInt32(Request.QueryString["callerType"]);
                logic = new CausalDevolucionLogic(callerType);

                ddlVendedor.DataValueField = "Key";
                ddlVendedor.DataTextField = "Value";
                ddlAccion.DataValueField = "Key";
                ddlAccion.DataTextField = "Value";
                ddlCausa.DataValueField = "Key";
                ddlCausa.DataTextField = "Value";

                ddlVendedor.DataSource = logic.getVendedoresForDdl();
                ddlAccion.DataSource = logic.getAccionesForDdl();
                ddlCausa.DataSource = logic.getCausasForDdl();

                ddlVendedor.DataBind();
                ddlAccion.DataBind();
                ddlCausa.DataBind();
            }
		}

        protected void ddlCausa_indexChanged(object sender, EventArgs e)
        {
            String selValue = ddlCausa.SelectedValue;
            bool reqEspec = logic.causaRequiereEspecificacion(selValue);

            txtDetalleCausa.Visible = reqEspec;
            lblDetalleCausa.Visible = reqEspec;
        }

        protected void ddlAccion_indexChanged(object sender, EventArgs e)
        {
            String selValue = ddlAccion.SelectedValue;
            bool reqServ = logic.accionRequiereProveedor(selValue);

            txtProveedor.Visible = reqServ;
            lblProveedor.Visible = reqServ;
        }

        protected void btnGuardar_click(object sender, EventArgs e)
        {
            // TODO
        }
	}
}