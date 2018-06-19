using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;

namespace AMS.Automotriz
{
	public partial class GestionDashBoard : System.Web.UI.UserControl
	{
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                divAreaTorre.Visible = false;
                divMenuDash.Visible = false;
            }
		}

        protected void Abrir_Torre(Object Sender, EventArgs e)
        {
            divArea.Visible = false;
            divAreaTorre.Visible = true;
            divMenuDash.Visible = true;
            DateTime fechaHoy = DateTime.Now;
            divFechaTxt.InnerText = fechaHoy.ToString("MMM dd - yyyy");

            //datos TALLER MOVIMIENTO VEHICULAR
            divCitasTxt.InnerText = "26";
            divIngresadosTxt.InnerText = "15";
            divActualTxt.InnerText = "12";
            divAtentidoTxt.InnerText = "124";
        }

        protected void RegresarMenu(Object Sender, EventArgs e)
        {
            Response.Redirect("" + indexPage + "?process=Automotriz.GestionDashBoard");
        }
	}
}