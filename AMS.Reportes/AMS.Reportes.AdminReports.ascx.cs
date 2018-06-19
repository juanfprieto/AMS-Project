using System;
using System.Data.Common;
using System.Configuration;
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
using AMS.Forms;

namespace AMS.Reportes
{
	public partial class Admin : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
		}
		
		protected void Ingresar_Nuevo(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Reportes.TablasReporte");
		}
		
		protected void Ingresar_Eliminar(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Reportes.EliminarReporte");
		}
		
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
