using System;
using AMS.Tools; 
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace AMS.Web
{
    public partial class ModalPDF : System.Web.UI.Page
    {
        protected AMS_Tools_Email miMail = new AMS_Tools_Email();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["rpt"] != null)
            {
                string nReporte = Request.QueryString["rpt"];
                string nomEditado = nReporte.Replace(".pdf", "");
                nomEditado = nomEditado.Substring(0, nomEditado.IndexOf("_"));
                miMail.RutaArchivo = ConfigurationManager.AppSettings["PathToReports"] + nReporte;
                miMail.NombreReporte = nomEditado;
                miMail.Visible = true;
            }
        }
    }
}