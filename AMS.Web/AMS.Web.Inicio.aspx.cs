using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;

namespace AMS.Web
{
	/// <summary>
	/// Descripción breve de Inico.
	/// </summary>
	public partial class Inicio : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(Inicio));
            if(Session["vendedorActivo"] != null)
            {
                string codUsuario = DBFunctions.SingleData("SELECT SUSU_CODIGO FROM SUSUARIO WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                string tipo_cod_vendedor = DBFunctions.SingleData("SELECT PVEN_CODIGO || '~' || TVEND_CODIGO FROM PVENDEDOR WHERE SUSU_CODIGO = '" + codUsuario + "'");
                Session.Clear();
                if (tipo_cod_vendedor != "" && tipo_cod_vendedor.Split('~')[1] == "VV")
                {
                    //regresarPrincipalVendedores.Visible = false;
                    Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.GestionVendedor&cod_vend=" + tipo_cod_vendedor.Split('~')[0]);
                }
                else
                {
                    Response.Redirect("AMS.Web.CerrarSesion.aspx&errorVendedor=1");
                }
            }
            /*
             * Hola mundo*/
            // Introducir aquí el código de usuario para inicializar la página
            //Redireccionar si es dispositivo movil
            //if(Convert.ToBoolean(Session["ISMOBILE"])==true)
            //{
            //    if(ConfigurationManager.AppSettings["MainMobileIndexPage"]!=null)
            //        Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]);
            //}
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
