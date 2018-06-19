using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.DB;
using System.Web.Security;
using System.Configuration;
using Ajax;
using AMS.Tools;

namespace AMS.Web
{
	public partial class JMenu : System.Web.UI.Page
	{
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private Boolean yapinto = true;
        protected DataSet ds = new DataSet();

		protected void Page_Load(object sender, EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(JMenu));
            if (!IsPostBack)
            {
                //lblEmpresa.Text = ConfigurationManager.AppSettings["CompanyName"];

                //Distincion de nombre solo cuando esta en modo DEBUG... para 2015
                
                if (GlobalData.getEMPRESA() == "aspx")
                    lblEmpresa.Text = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()] + ":" + ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
                else
                    lblEmpresa.Text = DBFunctions.SingleDataGlobal("SELECT GEMP_DESCRIPCION FROM GEMPRESA WHERE GEMP_NOMBRE='" + GlobalData.getEMPRESA() + "';");
            }
            else yapinto = false;

            Proceso_Logueo();
		}

        private void Proceso_Logueo()
        {
            if (Request.QueryString["first"] == null)
            {
                //Si se ingresa al servidor de ecas con un usuario distinto a invitado
                if (HttpContext.Current.Server.MachineName == "WEBMASTER" && HttpContext.Current.User.Identity.Name.ToLower() != "invitado")
                {
                    //Si el usuario no ha ingresado y no tiene ip registrada
                    if ((DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == "N") && (DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == ""))
                    {
                        //Cargo el menu y pongo el flag de ingreso
                        BuildMenu();
                        DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='L', susu_ipaddr='" + Request.UserHostName.Trim() + "' WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'");
                    }
                    //Si el usuario esta registrado con anterioridad
                    else if (DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == "L")
                    {
                        //Si la ip de donde esta ingresando es la misma que la que ya esta registrada en la BD
                        if (DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == Request.UserHostName)
                            //Cargo el menu, no es necesario poner flag, porq yo lo tiene
                            BuildMenu();
                        else
                        {
                            //Termino la sesión, lo deslogueo, lo mando al default y muestro el error
                            Session.Abandon();
                            FormsAuthentication.SignOut();
                            Response.Redirect(indexPage + "?error=erruser");
                        }
                    }
                }
                //Si se ingresa al servidor de ecas como invitado
                else if (HttpContext.Current.Server.MachineName == "WEBMASTER" && HttpContext.Current.User.Identity.Name.ToLower() == "invitado")
                {
                    //Cargo el menu
                    BuildMenu();
                }
                //Si se ingresa a cualquier otro servidor
                else if (HttpContext.Current.Server.MachineName != "WEBMASTER")
                {
                    //Si el usuario no ha ingresado y no tiene ip registrada
                    //if((DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")=="N")&&(DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'")==""))
                    if ((DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == "N"))
                    {
                        //Borrar IP anterior
                        DBFunctions.NonQuery("UPDATE susuario SET susu_ipaddr=null WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'");
                        //Cargo el menu y pongo el flag de ingreso
                        BuildMenu();
                        DBFunctions.NonQuery("UPDATE susuario SET susu_flagingr='L', susu_ipaddr='" + Request.UserHostName.Trim() + "' WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'");
                    }
                    //Si el usuario esta registrado con anterioridad
                    else if (DBFunctions.SingleData("SELECT susu_flagingr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == "L")
                    {
                        //Si la ip de donde esta ingresando es la misma que la que ya esta registrada en la BD
                        if (DBFunctions.SingleData("SELECT susu_ipaddr FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "'") == Request.UserHostName)
                            //Cargo el menu, no es necesario poner flag, porq yo lo tiene
                            BuildMenu();
                        else
                        {
                            Response.Redirect("AMS.Web.CerrarSesion.aspx?error=erruser");
                        }
                    }
                }
            }
            else
            {
                //process = Request.QueryString["process"];
            }
        }

        [Ajax.AjaxMethod]
        public DataSet Cargar_OpcionesMenu(string Cedula)
        {
            DataSet opcionesMenu = new DataSet();
            //Table[0]: Select padres
            //Table[1]: Select hijos
            //Table[2]: Select nietos
            DBFunctions.Request(opcionesMenu, IncludeSchema.NO,
                    "SELECT SMEN.smen_opcion AS PADRE FROM dbxschema.smenu SMEN WHERE smen.smen_padre = 'ND' " + 
                    " and smen_opcion IN ( " +
                    " SELECT SMEN.smen_padre FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in " + 
                    "  (select distinct smen_padre from dbxschema.smenu SMEN, dbxschema.spermiso SPER " +
                    " WHERE SMEN.smen_codigo = SPER.smen_codigo and ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario " +
                    " WHERE susu_login = '" + HttpContext.Current.User.Identity.Name.ToLower() + "')) " +
                    " ) ORDER BY SMEN.smen_opcion ASC; " +

                    //"SELECT SMEN.smen_opcion AS PADRE FROM dbxschema.smenu SMEN WHERE smen.smen_padre = 'ND' " +
                    //"and smen_opcion IN ( " +
                    //"SELECT SMEN.smen_padre FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in " +
                    //"(select distinct smen_padre from dbxschema.vmenu1 where ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario " +
                    //"WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')) " +
                    //")ORDER BY SMEN.smen_opcion ASC; " +

                    //"SELECT SMEN.smen_opcion as HIJO, SMEN.smen_padre as PADRE, SMEN.smen_url FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in " +
                    //"(select distinct smen_padre from dbxschema.vmenu1 where ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario " +
                    //"WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')) order by smen.smen_padre,smen.smen_opcion; " +

            // Eiminar vista
            "SELECT SMEN.smen_opcion as HIJO, SMEN.smen_padre as PADRE, SMEN.smen_url FROM dbxschema.smenu SMEN WHERE smen_url = 'ND' and SMEN.smen_opcion in  " +
                    "(select distinct smen_padre from dbxschema.smenu SMEN, dbxschema.spermiso SPER " +
                    " WHERE SMEN.smen_codigo = SPER.smen_codigo and ttipe_codigo = (SELECT ttipe_codigo FROM dbxschema.susuario " +
                    " WHERE susu_login = '" + HttpContext.Current.User.Identity.Name.ToLower() + "')) order by smen.smen_padre,smen.smen_opcion; " +

                    "SELECT SMEN.smen_opcion as NIETO, SMEN.smen_padre as HIJO, SMEN.smen_url as URL, SMEN.smen_manual, SMEN.smen_manualopt FROM smenu SMEN, spermiso SPER WHERE SMEN.smen_codigo=SPER.smen_codigo " +
                    "AND SPER.ttipe_codigo=(SELECT ttipe_codigo FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "') " +
                    "ORDER BY SMEN.smen_opcion ASC; SELECT TTIPE_CODIGO AS TIPO_USUARIO FROM SUSUARIO WHERE SUSU_LOGIN = '" + HttpContext.Current.User.Identity.Name.ToLower() + "';");
            DBFunctions.RequestGlobal(opcionesMenu, IncludeSchema.NO, "", "SELECT GEMP_NOMBRE FROM GEMPRESA WHERE GEMP_NOMBRE = '" + GlobalData.EMPRESA.ToLower()+ "'");

            return opcionesMenu;
        }

        protected void BuildMenu(){}
	}
}