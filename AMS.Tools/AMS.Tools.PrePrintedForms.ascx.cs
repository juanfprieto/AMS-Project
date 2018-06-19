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

namespace AMS.Tools
{
	public partial class PrePrintedForms : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Preparar_ddlDocEsp(ddlDocGen.SelectedValue);
			}
		}
		
		protected void Cambio_Documento(Object  Sender, EventArgs e)
		{
			Preparar_ddlDocEsp(ddlDocGen.SelectedValue);
		}
		
		protected void Go_Diseno1(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Tools.PrePrintedForms1&TipGen="+ddlDocGen.SelectedValue+"&TipEsp="+ddlDocEsp.SelectedValue+"");
		}
		
		protected void Preparar_ddlDocEsp(string opc)
		{
			ddlDocEsp.Items.Clear();
			if(opc == "F")
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlDocEsp,"SELECT stfc_codigo, stfc_descripcion FROM stipofactura");
			}
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
