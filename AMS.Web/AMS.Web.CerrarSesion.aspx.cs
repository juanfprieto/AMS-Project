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
using System.Configuration;
using AMS.DB;
using System.Web.Security;
using AMS.Tools;

namespace AMS.Web
{
	/// <summary>
	/// Descripción breve de CerrarSesion.
	/// </summary>
	public partial class CerrarSesion : System.Web.UI.Page
	{
		protected string loginPage = ConfigurationManager.AppSettings["LoginPage"];
		protected System.Web.UI.WebControls.Label lb;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string webPage = ConfigurationManager.AppSettings["PathToWebPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				Session.Abandon();
				FormsAuthentication.SignOut();
                if(Request.QueryString["errorVendedor"] != null)
                {
                    Utils.MostrarAlerta(Response, "Usted no es un vendedor autorizado, su sesión se cerrará.");
                }
				if(Request["error"] == null)
				{
					DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='N',susu_ipaddr=null WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'");

                    if (webPage != null && webPage != "")
                        this.RegisterStartupScript("redirect", "<script>parent.location.href = '" + webPage + "';</script>");
                    else
					    this.RegisterStartupScript("redirect","<script>parent.location.href = 'AMS.Web.Inicio.aspx';</script>");
				}
                else
                    if (webPage != null && webPage != "")
                        this.RegisterStartupScript("redirect", "<script>parent.location.href = '" + webPage + "';</script>");
                    else
                        this.RegisterStartupScript("redirect", "<script>parent.location.href = 'AMS.Web.Inicio.aspx?error=" + Request["error"] + "';</script>");
			}
		}		

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}
