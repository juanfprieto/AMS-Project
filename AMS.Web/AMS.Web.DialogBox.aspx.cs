using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Web
{
	public partial class DialogBox : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.DropDownList ddlNomDir;
		protected System.Web.UI.WebControls.Label lbAgregar1;
		protected System.Web.UI.WebControls.TextBox tbLiteral;
		protected System.Web.UI.WebControls.Label lbAgregar2;
		protected System.Web.UI.WebControls.TextBox tbOut;
		protected System.Web.UI.WebControls.Label lbClear;
		protected System.Web.UI.WebControls.Label lbCerrar;
		
		public DialogBox()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}

		private void InitializeComponent()
		{
		
		}
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				// Cargamos El texto que se va a utilizar para la validacion
				infoDialog.Controls.Add(new LiteralControl(""+Request.QueryString["title"]+""));
				string jsParams = "window.returnValue = 'true';window.close();";
				string jsParams1 = "window.close();";
				btnAcp.Attributes.Add("onclick",jsParams);
				btnCnl.Attributes.Add("onclick",jsParams1);
			}
		}
	}
}
