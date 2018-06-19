using System;
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
using System.Configuration;

namespace AMS.Nomina
{
	public class horas : System.Web.UI.UserControl
	{
		protected TextBox numeroFactura,fecha,nit,almacen,centroCosto,vendedor,observacion;
		protected DropDownList prefijoFactura,tipoGasto;
		protected Calendar calendarioFecha;
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
				
		protected void Page_Load(object Sender,EventArgs e)
		{
			
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}

