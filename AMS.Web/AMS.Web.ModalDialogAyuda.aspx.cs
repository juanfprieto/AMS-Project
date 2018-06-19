using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;

namespace AMS.Web
{
	public partial  class ModalDialogAyuda : System.Web.UI.Page
	{
		
		protected void Page_Load(object Sender, EventArgs e)
		{
			if(Request.QueryString["tabla"]!=null && Request.QueryString["campo"]!=null)
			{
				lbCampo.Text=DBFunctions.SingleData("SELECT scam_comentario FROM dbxschema.scampoayuda WHERE scam_tabla='"+Request.QueryString["tabla"]+"' AND scam_campo='"+Request.QueryString["campo"]+"'")+"<p></p>";
				lbAyuda.Text="<p></p>"+DBFunctions.SingleData("SELECT scam_ayuda FROM dbxschema.scampoayuda WHERE scam_tabla='"+Request.QueryString["tabla"]+"' AND scam_campo='"+Request.QueryString["campo"]+"'")+"<p></p>";
			}
			else
				lb.Text+="Error Interno";
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
