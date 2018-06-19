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
using AMS.Forms;
using AMS.DB;
using System.Text;
using AMS.Tools;
using System;

namespace AMS.Tools
{
	public partial class Encabezado : System.Web.UI.UserControl
	{
        
        protected string userName;
        protected string userCod;

		protected void Page_Load(object sender, EventArgs e)
		{
            userCod = Request.QueryString["cod"];
            userName = Request.QueryString["name"];
            infoProcess.Text = ConfigurationManager.AppSettings["systemName"];

            try
            {
                if (Session["AMS_USER"] == null)
                    Session["AMS_USER"] = DBFunctions.SingleData("SELECT U.SUSU_NOMBRE CONCAT ' (' CONCAT U.SUSU_LOGIN CONCAT ')<BR>' CONCAT P.TTIPE_DESCRIPCION FROM SUSUARIO U,TTIPOPERFIL P WHERE U.TTIPE_CODIGO=P.TTIPE_CODIGO AND U.SUSU_LOGIN='" + HttpContext.Current.User.Identity.Name + "';");
                lblUsuario.Text = Session["AMS_USER"].ToString();
            }
            catch (Exception ex)
            {
                lblUsuario.Text = ex.Message;
            }

            if (!IsPostBack)
            {
                string ruta, padre, hijo;
                bool err = false;
                int nm = 0;
                ruta = Request.QueryString["path"];
                hijo = ruta;
                while (!err && nm++ < 3)
                {
                    padre = DBFunctions.SingleData("SELECT sp.smen_opcion FROM smenu sp, smenu sh WHERE sh.smen_opcion='" + hijo + "' and sp.smen_opcion=sh.smen_padre ORDER BY sp.smen_opcion;");
                    //padre = DBFunctions.SingleData("SELECT smen_opcion FROM smenu WHERE smen_opcion = 'Vehículos nuevos de los últimos meses' ORDER BY smen_opcion;");
                    if (padre.Length == 0) break;
                    hijo = padre;
                    ruta = padre + " / " + ruta;
                }
                ViewState["AMS_RUTA"] = ruta;
            }

            if (ViewState["AMS_RUTA"] != null)
                infoProcess.Text = ViewState["AMS_RUTA"].ToString();


		}

	}
}