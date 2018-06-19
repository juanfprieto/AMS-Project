// created on 01/03/2005 at 9:09
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;

namespace AMS.Comercial
{
	public class Impresion : System.Web.UI.UserControl
	{
		protected Label lb;
		protected System.Web.UI.WebControls.TextBox tbEmail;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.ImageButton ibMail;
		protected System.Web.UI.WebControls.PlaceHolder toolsHolder;
		protected PlaceHolder tiquetes;
				
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				tiquetes.Controls.Add(new LiteralControl("<iframe id='frameDibujo' src='../aspx/AMS.Tiquete.aspx?tiqs="+Request.QueryString["tiqs"]+"' width='700' height='600'></iframe>"));
				StringBuilder SB= new StringBuilder();
    	    	StringWriter SW= new StringWriter(SB);
     		   	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			    tiquetes.RenderControl(htmlTW);
				Session["Rep"] = SB.ToString();
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}

