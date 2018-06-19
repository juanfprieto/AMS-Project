using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using System.Threading;
using AMS.Contabilidad;

namespace AMS 
{
    public class Global : System.Web.HttpApplication
    {

        private System.ComponentModel.IContainer components = null;
        private string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private Thread thrContabilidad;
        private InterfaceContable interfaceAutomatica;

        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
            //routes.MapRoute(
            //    "Default",                                              // Route name
            //    "{controller}/{action}/{id}",                           // URL with parameters
            //    new { controller = indexPage, action = "Index", id = UrlParameter.Optional }  // Parameter defaults
            //);

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //CrystalDecisions.Shared.SharedUtils.RequestLcid = 9226; //set report locale to colombia :3
            //RegisterRoutes(RouteTable.Routes);

            //bd.WebScheduledTasks.Scheduler.RunTimer();  //Comentar para desactivar tareas programadas.
            
            //Configuracion
            if (!Verificar_Configuracion())
            {
                Context.Response.Redirect(indexPage + "?process=Tools.ConfiguracionInicial&cnf=1&first=1");
            }
            //Generacion Automatica Contabilidad
            /*
            if (ConfigurationManager.AppSettings["ContabilidadAutomatica"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["ContabilidadAutomatica"]))
            {
                interfaceAutomatica = new InterfaceContable();
                thrContabilidad = new Thread(new ThreadStart(interfaceAutomatica.MonitorearInterface));
                thrContabilidad.Start();
            }
            */

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private bool Verificar_Configuracion()
        {
            bool config = true;
            string iv = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];//1
            string comp = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];//2
            string email = ConfigurationManager.AppSettings["EMailFrom"];//3
            string pwdemail = ConfigurationManager.AppSettings["PasswordEMail"];//4
            string mserver = ConfigurationManager.AppSettings["MailServer"];//5
            string centavos = ConfigurationManager.AppSettings["ManejoCentavos"];//6
            string moneda = ConfigurationManager.AppSettings["MonedaNacional"];//7
            string dbcon = ConfigurationManager.AppSettings["DBConnectionType"];//8
            string constr = ConfigurationManager.AppSettings["ConnectionString"];//9
            string esq = ConfigurationManager.AppSettings["Schema"];//10
            if (iv == string.Empty || comp == string.Empty || email == string.Empty || pwdemail == string.Empty || mserver == string.Empty || centavos == string.Empty || moneda == string.Empty || dbcon == string.Empty || constr == string.Empty || esq == string.Empty)
                config = false;
            return config;
        }
    }
}