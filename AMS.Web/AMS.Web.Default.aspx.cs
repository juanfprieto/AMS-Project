// created on 11/09/2003 at 15:10

using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using AMS.DB;
using AMS.Tools;

namespace AMS.Web
{
	/// <summary>
	/// Ejecuta el proceso de autenticación y  acceso de un usuario al sistema
	/// </summary>
	public partial class Default : System.Web.UI.Page
	{
		protected string error;
		protected bool isMobile;
	
		public Default()
		{
    		Page.Init += new System.EventHandler(Page_Init);
	  	}
  	
  		protected void Page_Init(Object sender, EventArgs EvArgs) 
		{
            //Obtiene nombre de la empresa. para 2015
            //string urlLogin = HttpContext.Current.Request.Url.AbsoluteUri;
            //string[] arrayUrlLogin = urlLogin.Split('/');
            //GlobalData.EMPRESA = arrayUrlLogin[3].ToLower();

            string urlLogoEmpresa = DBFunctions.SingleDataGlobal("SELECT GEMP_LOGOURL FROM GEMPRESA WHERE GEMP_NOMBRE='" + GlobalData.getEMPRESA() + "';");
            Image3.ImageUrl = urlLogoEmpresa;

            if (Request.QueryString["error"]!=null)
				error = Request.QueryString["error"].ToString();

			if(error=="erruser")
				loginInfoM.Text = loginInfo.Text = "Este usuario ya se encuentra registrado<br>o se ha cerrado mal la sesión.<br>Comuniquese con el Administrador de Sistema.";
		}

		private void InitializeComponent()
        {
		}

		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		public void Validate(Object Sender, EventArgs E)
		{
            loginInfo.Visible = true;
            Usuario usuarioIngreso = new Usuario();
            //string userLogin = user.Text.ToLower();
            string userLogin = GlobalData.getEMPRESA() + "." + user.Text.ToLower();  //Para 2015
            string autenticaUsu = usuarioIngreso.AutenticarUsuario(userLogin, pass.Text);
            if (autenticaUsu.ToString() == "true")
            {
                //Validacion de IP de loggin valida.
                string ipEntrante = Request.UserHostName;
                string ipValida = DBFunctions.SingleData("SELECT susu_ipvalida FROM susuario  WHERE susu_login='" + user.Text.ToLower() + "'");
                
                if (ipValida.Contains(ipEntrante) || ipValida =="" || ipEntrante.Contains("192.168.1."))
                    FormsAuthentication.RedirectFromLoginPage(user.Text.ToLower(), false);
                else
                    loginInfoM.Text = loginInfo.Text = "Esta cuenta no esta configurada para acceder de lugares no registrados.";
            }
			else
            {
                if(autenticaUsu.ToString() == "Borrado")
                {
                    Tools.Utils.MostrarAlerta(Response, "Este usuario fue eliminado del sistema, contacte al administrador para más información");
                }
                else if(autenticaUsu.ToString() == "noEncontrado")
                {
                    Tools.Utils.MostrarAlerta(Response, "Si no puede ingresar, contacte al administrador de sistema."); //toca cambiar el mensaje por uno más entendible.
                }
                loginInfoM.Text = loginInfo.Text = "Datos incorrectos";
            }
            verificarVendedor(user.Text.ToLower());
				
        }

        protected void verificarVendedor(string user)
        {
            string codUsuario = DBFunctions.SingleData("SELECT SUSU_CODIGO FROM SUSUARIO WHERE SUSU_LOGIN = '" + user + "'");
            string tipo_cod_vendedor = DBFunctions.SingleData("SELECT PVEN_CODIGO || '~' || TVEND_CODIGO FROM PVENDEDOR WHERE SUSU_CODIGO = '" + codUsuario + "'");
            if(tipo_cod_vendedor != "" && tipo_cod_vendedor.Split('~')[1] == "VV")
            {
                Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.GestionVendedor&cod_vend=" + tipo_cod_vendedor.Split('~')[0]);
            }

        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                //pnlMobile.Visible = false;
                loginInfo.Visible = false;
                string userStr = Request.QueryString["datosUser"];
                string passStr = Request.QueryString["datosPass"];

                if (userStr != "" && passStr != "" && userStr != null && passStr != null)
                {
                    user.Text = userStr;
                    pass.Text = passStr;

                    Validate(null, null);
                }
            }
        }
	}
}
