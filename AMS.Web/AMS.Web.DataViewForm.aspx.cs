// created on 9/22/2003 at 9:13 AM

using System;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.ServiceProcess;
using AMS.DB;

namespace AMS.Web
{
	/// <summary>
	/// ******
	/// </summary>
	public partial class DataViewForm : System.Web.UI.Page
	{
		protected Label  lbSystemName, lbCompanyName ;
		
		protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		protected string nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string process;
		protected string userName;
		protected System.Web.UI.WebControls.Label Label1;
		protected string userCod;

		public DataViewForm()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}
  	
				
		protected void Page_Load(object sender, System.EventArgs e)
		{
			lbSystemName.Text = ConfigurationManager.AppSettings["SystemName"];
			lbCompanyName.Text = ConfigurationManager.AppSettings["CompanyName"];
		}

		private void InitializeComponent()
		{
		}

		private void ControlsFromProcess()
		{
			gridHolder.Controls.Add(LoadControl("" + pathToControls + nameSystem + process + ".ascx"));
			infoProcess.Text=Request.QueryString["path"];
		}
	}
}
