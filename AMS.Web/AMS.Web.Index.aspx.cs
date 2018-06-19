// created on 9/22/2003 at 9:13 AM

using System;
using System.Web;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;
using System.ServiceProcess;
using AMS.DB;
using System.Threading;
using System.Globalization;

namespace AMS.Web
{
/// <summary>
/// ******
/// </summary>
    public partial class Index : System.Web.UI.Page, IIndexPage
	{
		
		protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected string pathToAspxControls = ConfigurationManager.AppSettings["PathToAspxControls"];
        protected string pathToManuales = ConfigurationManager.AppSettings["PathToManuales"];
        protected string nameSystem = ConfigurationManager.AppSettings["SystemName"] + ".";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string process;
		protected string userName;
		protected string userCod;
		protected string tabla;
        bool mensajeMostrado = false;

		public Index()
		{
            Page.PreRender += new EventHandler(Page_PreRender);
            Page.MaintainScrollPositionOnPostBack = true;
	  	}
  	
        protected void Page_PreRender(object sender, System.EventArgs e)
        {
            foreach (Control ctrl in Page.Controls)
                UnLock_Controls(ctrl);
        }
        private void UnLock_Controls(Control ctrlP)
        {
            if (ctrlP is TextBox)
            {
                TextBox txtCtrl=(TextBox)ctrlP;
                if (txtCtrl.ReadOnly){
                    txtCtrl.ReadOnly = false;
                    txtCtrl.Attributes.Add("readonly","readonly"); 
                }
                /*else
                    txtCtrl.Attributes.Remove("readonly");*/
            }
            if(ctrlP.Controls.Count>0)
                foreach (Control ctrl in ctrlP.Controls)
                    UnLock_Controls(ctrl);
        }

        protected override void InitializeCulture()
        {
            string strCulture = "en-US";
            CultureInfo ci = new CultureInfo(strCulture);

            UICulture = strCulture;
            Culture = strCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
           
            base.InitializeCulture();
        }
				
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //int x ;

            ////// Set y to the value of x if x is NOT null; otherwise,
            ////// if x = null, set y to -1.
            //int y = x ?? -1;
            userCod = Request.QueryString["cod"];
			userName = Request.QueryString["name"];
			infoProcess.Text = ConfigurationManager.AppSettings["systemName"];
            string manual = Request.QueryString["manual"];
            string manualOpt = Request.QueryString["manualOpt"];

            if (Request.QueryString["eMsj"] != null && 
                Request.QueryString["eMsj"] != "" &&
                !mensajeMostrado)
                MostrarMensaje(Request.QueryString["eMsj"]);

			try
			{
				if(Session["AMS_USER"]==null)
					Session["AMS_USER"]=DBFunctions.SingleData("SELECT U.SUSU_NOMBRE CONCAT ' (' CONCAT U.SUSU_LOGIN CONCAT ')<BR>' CONCAT P.TTIPE_DESCRIPCION FROM SUSUARIO U,TTIPOPERFIL P WHERE U.TTIPE_CODIGO=P.TTIPE_CODIGO AND U.SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name+"';");
				lblUsuario.Text=Session["AMS_USER"].ToString();

                //Manual anterior fisico en pdf
                if (manual != null)
                {
                    lblLinkManual.Text = String.Format("<a href=\"{0}{1}#{2}\">Manual</a>", pathToManuales, manual, manualOpt);
                    lblLinkManual.Visible = true;
                }

                //Nuevo manual virtual en base de datos, es Boton.
                string htmlManualID = ObtenerIDManual();

                if (htmlManualID != "")
                {
                    btnManual.Visible = true;
                }
                

            }
			catch(Exception ex)
			{
				lblUsuario.Text=ex.Message;
			}
			
			if(!IsPostBack)
            {
                //if (Session["vendedorActivo"] != null)
                //{
                //    string tipo_cod_vendedor = DBFunctions.SingleData("SELECT PVEN_CODIGO || '~' || TVEND_CODIGO FROM PVENDEDOR WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                //    Session.Clear();
                //    if (tipo_cod_vendedor != "" && tipo_cod_vendedor.Split('~')[1] == "VV")
                //    {
                //        //regresarPrincipalVendedores.Visible = false;
                //        Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"] + "?process=Vehiculos.GestionVendedor&cod_vend=" + tipo_cod_vendedor.Split('~')[0]);
                //    }
                //    else
                //    {
                //        Response.Redirect("AMS.Web.CerrarSesion.aspx&errorVendedor=1");
                //    }
                //}
                string ruta,padre,hijo;
				bool err=false;
				int nm=0;
				ruta=Request.QueryString["path"];
				hijo=ruta;
				while(!err && nm++<3){
                    padre = DBFunctions.SingleData("SELECT sp.smen_opcion FROM smenu sp, smenu sh WHERE sh.smen_opcion='" + hijo + "' and sp.smen_opcion=sh.smen_padre ORDER BY sp.smen_opcion;");
					if(padre.Length==0)break;
					hijo=padre;
					ruta=padre + " / "+ruta;
				}
				ViewState["AMS_RUTA"]=ruta;
			}
			if(ViewState["AMS_RUTA"]==null)
				ViewState["AMS_RUTA"]="AMS";
            //if (Session["ISMOBILE"] == null)
            //    Session["ISMOBILE"] = (((System.Web.Configuration.HttpCapabilitiesBase)Request.Browser).IsMobileDevice);
            //if(Convert.ToBoolean(Session["ISMOBILE"]) && ConfigurationManager.AppSettings["MainMobileIndexPage"]!=null)
            //    Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process="+Request.QueryString["process"]);
			
			if(Request.QueryString["process"] != null)
			{
				process = Request.QueryString["process"];
				tabla = Request.QueryString["table"];
				ControlsFromProcess();
			}
			else
			{
				Image img = new Image();
				img.AlternateText = "Bienvenido al sistema eCASweb AMS";
				img.ImageUrl = ConfigurationManager.AppSettings["BootImage"];
				img.ImageAlign = ImageAlign.Left;
				gridHolder.Controls.Add(img);
			}
            if (Session["btnRegresarActivo"] != null)
            {
                regresarPrincipalVendedores.Visible = true;
                //Session["btnRegresarActivo"] = null;
            }
		}

		private void InitializeComponent()
		{

		}

        protected void MotrarManual(object Sender, EventArgs e)
        {
            string htmlManual = ObtenerIDManual();

            divManual.InnerHtml = htmlManual;
            divContManual.Visible = true;
            btnCerrarManual.Visible = true;
        }

        protected string ObtenerIDManual()
        {
            string [] arrayManual = Request.QueryString["process"].Split('.');

            return DBFunctions.SingleDataGlobal(@"select gman_contenido from gmanual where smen_opcion =
                                        (select mobj_id from MOBJETOECAS where " +
                                        "(mobj_nombre like '%" + arrayManual[0] + "." + arrayManual[1] + ".ascx%' or mobj_nombre like '%" + arrayManual[0] + "." + arrayManual[1] + ".aspx%') and " +
                                        "(tpro_id='ASCX' or tpro_id='ASPX') );");
        }

        protected void CerrarManual(object Sender, EventArgs e)
        {
            divManual.InnerHtml = "";
            divContManual.Visible = false;
            btnCerrarManual.Visible = false;
        }
        public void regresarControl(object Sender, EventArgs e)
        {
            string codUsuario = DBFunctions.SingleData("SELECT SUSU_CODIGO FROM SUSUARIO WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
            string tipo_cod_vendedor = DBFunctions.SingleData("SELECT PVEN_CODIGO || '~' || TVEND_CODIGO FROM PVENDEDOR WHERE SUSU_CODIGO = '" + codUsuario + "'");
            Session.Clear();
            if (tipo_cod_vendedor != "" && tipo_cod_vendedor.Split('~')[1] == "VV")
            {
                //regresarPrincipalVendedores.Visible = false;
                Response.Redirect(indexPage + "?process=Vehiculos.GestionVendedor&cod_vend=" + tipo_cod_vendedor.Split('~')[0]);
            }
            else
            {
                Response.Redirect("AMS.Web.CerrarSesion.aspx&errorVendedor=1");
            }
            
        }

        private void ControlsFromProcess()
		{
			menuHolder.Controls.Add(new Panel());
            gridHolder.Controls.Add(LoadControl("" + pathToControls + nameSystem + process + ".ascx"));
           
			if(ViewState["AMS_RUTA"]!=null)
				infoProcess.Text=ViewState["AMS_RUTA"].ToString();
			else
				infoProcess.Text=Request.QueryString["path"];

		}

        public void MostrarMensaje(string msj)
        {
            Response.Write(String.Format("<script language='javascript'>alert('{0}');</script>", msj));
            mensajeMostrado = true;
        }
    }
}
