using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using bd.WebScheduledTasks;
using AMS.Tools;

namespace AMS.VIP
{
	public partial class IngresoCliente : System.Web.UI.UserControl
	{
        private String tarjeta;
        private String nit;
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];
        bool hasNit;

		protected void Page_Load(object sender, EventArgs e)
		{
            hasNit = Request.QueryString["Nit"] != null;
            if (!IsPostBack)
            {
                if (hasNit)
                {
                    lblNit.Visible = true;
                    txtNit.Visible = true;
                }
            }

            txtTarjeta.Focus();
            txtTarjeta.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + btnAceptar.UniqueID + "','')");
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            tarjeta = txtTarjeta.Text;
            nit = txtNit.Text;

            bool noTar = tarjeta.Equals("");
            bool noNit = nit.Equals("");

            if (noTar && noNit)
            {
                Utils.MostrarAlerta(Response, "Por favor Ingrese un dato");
                return;
            }
            if (!((noTar && !noNit) || (!noTar && noNit)))
            {
                Utils.MostrarAlerta(Response, "Por favor Ingrese un solo dato");
                return;
            }

            Hashtable cliente = null;
            if (noNit && IngresoClienteLogic.tarjetaExist(tarjeta))
                cliente = IngresoClienteLogic.getDatosIngresoClienteTarjeta(tarjeta);
            else if (noTar && IngresoClienteLogic.nitExist(nit))
                cliente = IngresoClienteLogic.getDatosIngresoClienteNit(nit);
            else
            {
                Utils.MostrarAlerta(Response, "No se encuentra al cliente");
                return;
            }

            String fwdOpt = "";
            String opt = Request.QueryString["opt"]; // (C)onsulta / (M)odificación / (F)acturacion / (R)edencion
            String codCliente = cliente["CODIGO"].ToString();
            String tipoCli = cliente["AFILIACION"].ToString();
            String redirect = null;

            if (opt == "R")
            {
                redirect = String.Format("{0}?process=VIP.Redenciones&codCliente={1}",
                    indexPage, codCliente);
            }
            else
            {
                if (tipoCli == "HIJO")
                    if (opt == "M")
                        fwdOpt = ManejoClientes.OPT_MOD_H;
                    else if (opt == "F")
                        fwdOpt = ManejoClientes.OPT_FAC_H;
                    else
                        fwdOpt = ManejoClientes.OPT_CONS_H;
                else
                    if (opt == "M")
                        fwdOpt = ManejoClientes.OPT_MOD_P;
                    else if (opt == "F")
                        fwdOpt = ManejoClientes.OPT_FAC_P;
                    else
                        fwdOpt = ManejoClientes.OPT_CONS_P;

                redirect = String.Format("{0}?process=VIP.ManejoClientes&fwdOpt={1}&codCliente={2}",
                    indexPage, fwdOpt, codCliente);
            }

            Response.Redirect(redirect);
        }
    }
}