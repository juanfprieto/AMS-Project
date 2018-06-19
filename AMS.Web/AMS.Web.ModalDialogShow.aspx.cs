using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Tools;

namespace AMS.Web
{
    public partial class ModalDialogShow : System.Web.UI.Page
    {
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Vals"] != null) return;

            string tipProc = Request.QueryString["tipProc"];
            string codAlma = Request.QueryString["codAlma"];
            string codItem = Request.QueryString["codItem"];
            string codOri = Request.QueryString["codOri"];
            string param = "tipProc=" + tipProc + "&codAlma=" + codAlma + "&codItem=" + codItem + "&codOri=" + codOri;

            Response.Redirect("AMS.Web.Index.aspx?process=Inventarios.AdminUbicaciones&" + param);
        }
    }
}