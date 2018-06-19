using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;

namespace AMS.Tools
{
	public partial class CerrarSesion : System.Web.UI.UserControl 
	{
		protected string loginPage = ConfigurationManager.AppSettings["LoginPage"];
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='N',susu_ipaddr=null WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'");
				Session.Abandon();
				FormsAuthentication.SignOut();
				Response.Redirect(indexPage);				
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
