using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using AMS.DB;

namespace AMS.VIP
{
    public partial class ListadoClientes : System.Web.UI.UserControl
	{
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
            }

            if (Request.QueryString["filtro"] != null)
                prepararTablaClientes(Request.QueryString["filtro"].ToString());
            else
                prepararTablaClientes("");
		}

        protected void filtrar(object sender, EventArgs e)
        {
            //prepararTablaClientes(txtNombre.Text.Trim());
            String redirect = String.Format("{0}?process=VIP.ListadoClientes&filtro={1}",
                    indexPage, txtNombre.Text.Trim());

            Response.Redirect(redirect);
        }

        protected void dgListaClientes_Command(Object sender, DataGridCommandEventArgs e)
        {
            string codCliente = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            
            if (e.CommandName == "editar")
            {
                string codAfil = ((DataBoundLiteralControl)e.Item.Cells[2].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");

                string fwdOpt = codAfil == "HIJO" ? "MODH" : "MODP";
                
                String redirect = String.Format("{0}?process=VIP.ManejoClientes&fwdOpt={1}&codCliente={2}",
                    indexPage, fwdOpt, codCliente);

                Response.Redirect(redirect);
            }
            else if (e.CommandName == "tarjeta")
            {
                solicitarNuevaTarjeta(codCliente);
                String redirect = String.Format("{0}?process=VIP.ListadoClientes",
                    indexPage);

                Response.Redirect(redirect);
            }
        }

        private void solicitarNuevaTarjeta(string codCliente)
        {
            String sql = String.Format("update mviptarjeta set tvig_codigo = 'B' where tvig_codigo = 'V' and mcli_codigo='{0}'", codCliente);
            DBFunctions.NonQuery(sql);

            codCliente = String.Format("'{0}'", codCliente);
            ManejoClientesLogic.guardarTarjetaNueva(codCliente);
        }

        private void prepararTablaClientes(string nomCliente)
        {
            String sql = String.Format("SELECT c.mcli_codigo AS codigo, \n" +
             "       c.mcli_nombrecompleto AS nombre, \n" +
             "       a.tafi_codigo AS codafil, \n" +
             "       a.tafi_nombre AS afiliacion, \n" +
             "       t.mtar_codigo AS tarjeta \n" +
             "FROM mvipcliente c  \n" +
             "  LEFT JOIN tvipafiliacion a ON c.tafi_codigo = a.tafi_codigo  \n" +
             "  LEFT JOIN mviptarjeta t ON c.mcli_codigo = t.mcli_codigo AND t.tvig_codigo = 'V' \n" +
             "WHERE UPPER(c.mcli_nombrecompleto) LIKE UPPER('%{0}%') \n" +
             "ORDER BY c.mcli_sap, c.tafi_codigo DESC"
             , nomCliente);

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            dgListaClientes.DataSource = ds.Tables[0];
            dgListaClientes.DataBind();
        }
	}
}