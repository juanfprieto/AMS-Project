using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Web
{
	public partial class WizardDirection : System.Web.UI.Page
	{
		
		public WizardDirection()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlNomDir,"SELECT * FROM pdireccion ORDER BY PDIR_NOMBRE");
				lbAgregar1.Attributes.Add("onclick","AgregarNom('"+ddlNomDir.ClientID+"','"+tbOut.ClientID+"');");
				lbAgregar2.Attributes.Add("onclick","AgregarLit('"+tbLiteral.ClientID+"','"+tbOut.ClientID+"');");
				lbClear.Attributes.Add("onclick","LimpiarControles('"+tbLiteral.ClientID+"','"+tbOut.ClientID+"');");
				//lbCerrar.Attributes.Add("onclick","parent.CerrarVentana('" + tbOut.Text + "');");
                if (Request.QueryString["valD"] != null)
                    tbLiteral.Text = Request.QueryString["valD"].ToString();
			}
		}
	}
}
	
	
