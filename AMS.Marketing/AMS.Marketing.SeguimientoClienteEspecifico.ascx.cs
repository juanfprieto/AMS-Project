using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using AMS.Tools;

namespace AMS.Marketing
{
    public partial class SeguimientoClienteEspecifico : System.Web.UI.UserControl
    {
        private String nit;
        private String placa;
        private SeguimientoClienteEspecificoLogic logic;
        public string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

        protected void Page_Load(object sender, EventArgs e)
        {
            logic = new SeguimientoClienteEspecificoLogic();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            nit = txtNit.Text;
            placa = txtPlaca.Text;

            if (!nit.Equals("") && !placa.Equals(""))
            {
                Utils.MostrarAlerta(Response, "Por favor especifique un solo dato");
                return;
            }
            else if (nit.Equals("") && !placa.Equals("")) 
                nit = logic.getNitFromPlaca(placa);

            if (!logic.nitCorrecto(nit))
            {
                Utils.MostrarAlerta(Response, "Por Favor Ingrese un Dato Válido");
                return;
            }

            String idVen = logic.getIdVendedor(nit);
            String redirect = String.Format("{0}?process=Marketing.AccionMarketing&idCli={1}&idVen={2}&path=Accion Marketing",
                indexPage, nit, idVen);

            Response.Redirect(redirect);
        }
    }
}