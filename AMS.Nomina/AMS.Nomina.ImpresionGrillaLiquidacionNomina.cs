using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;


namespace AMS.Nomina
{	

	public class ImpresionGrillaLiquidacionNomina: System.Web.UI.Page
	{
		
		protected DataGrid DataGrid2;
			
		public void Page_Load(object sender , EventArgs e)
		{
			DataGrid2.DataSource=(DataTable)Session["DataTable1"];
			DataGrid2.DataBind();
			
		}
	
	//protected HtmlInputFile fDocument;
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	
		
	
	}
	
}
