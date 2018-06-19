// created on 08/03/2005 at 16:08
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;
using AMS.Forms;

namespace AMS.Vehiculos
{
	public partial class PruebaXML : System.Web.UI.UserControl
	{
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			tb1.Attributes.Add("onkeyup","DateMask("+tb1.ClientID+");");
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

