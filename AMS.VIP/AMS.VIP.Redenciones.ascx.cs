using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using AMS.DB;
using AMS.CriptoServiceProvider;
using AMS.Forms;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.VIP
{
	public partial class Redenciones : System.Web.UI.UserControl
	{
        private String codCliente;
        private String codBodega;
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            codCliente = Request.QueryString["codCliente"];
            codBodega = ViewState["codBodega"] != null ? ViewState["codBodega"].ToString() : null;
            string codReporte = Request.QueryString["reporte"];

            if (!IsPostBack)
            {
                verificacionInicial();
            }

            prepararTablaPorRedimir();
            prepararTablaRedimidos();

            if (codReporte != null && codReporte != "")
                Response.Write("<script language:javascript>w=window.open('" + codReporte + "','','HEIGHT=600,WIDTH=600');</script>");
                
		}

        private void verificacionInicial()
        {
            string currUser = HttpContext.Current.User.Identity.Name;

            String sql = String.Format(
             "SELECT s.pvbod_codigo \n" +
             "FROM mvipusuariossede s  \n" +
             "  INNER JOIN susuario u ON s.susu_codigo = u.susu_codigo \n" +
             "WHERE UPPER(u.susu_login) = UPPER('{0}')"
             , currUser);

            codBodega = DBFunctions.SingleData(sql);

            if (codBodega == null || codBodega == "")
            {
                lblUsrNoAutorizado.Visible = true;
                lblUsrNoAutorizado.Text = String.Format("El usuario {0} no está asignado a ninguna sede. Debe Asignarlo para poder redimir.", currUser);

                dgPorRedimir.Visible = false;
                dgRedimidos.Visible = false;
            }
            else
            {
                ViewState["codBodega"] = codBodega;
            }
        }

        protected void redimir(Object sender, DataGridCommandEventArgs e)
        {
            string codRedencion = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0]).Text;

            string sql = String.Format("select mvite_codigo from mvipredencion where mred_consecutivo={0}",codRedencion);
            string codItem = DBFunctions.SingleData(sql);

            sql = String.Format("select COALESCE(mvsal_cantidad, 0) from mvipsaldoitem where mvite_codigo='{0}' and pvbod_codigo='{1}'", codItem, codBodega);
            string cant = DBFunctions.SingleData(sql);
            int cantidad = cant == null || cant == "" ? 0 : Convert.ToInt32(cant);

            if (cantidad <= 0)
                Utils.MostrarAlerta(Response, "Este item no tiene existencias!");
            else
            {
                cantidad--;
                sql = String.Format("update mvipsaldoitem set mvsal_cantidad={0} where mvite_codigo='{1}' and pvbod_codigo='{2}'"
                    , cantidad
                    , codItem
                    , codBodega);

                DBFunctions.NonQuery(sql);

                int codUsuario = ManejoClientesLogic.getCodUsuario();
                sql = String.Format("update mvipredencion set tred_codigo='REDIM', susu_codigo={1}, mred_fecha='{2}' where mred_consecutivo={0}"
                    ,codRedencion
                    ,codUsuario
                    ,DateTime.Now.ToString("yyyy-MM-dd"));
                DBFunctions.NonQuery(sql);

                imprimir(sender, e);

                String redirect = String.Format("{0}?process=VIP.Redenciones&codCliente={1}&reporte={2}",
                        indexPage, codCliente, ViewState["reporte"]);

                Response.Redirect(redirect);
            }
        }

        protected void imprimir(Object sender, DataGridCommandEventArgs e)
        {
            string codRedencion = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            Imprimir reporte = new Imprimir();

            Label lbvacio = new Label();
            DataTable dt = new DataTable();
            string[] Formulas = new string[0];
            string[] ValFormulas = new string[0];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string password = ConfigurationManager.AppSettings["PWD"];
            Crypto miCripto = new Crypto(Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd=miCripto.DescifrarCadena(password);
            string nomA = String.Format("Redencion_{0}", codRedencion);

            dt.Columns.Add("NOMBRE", typeof(string));
            dt.Columns.Add("VALOR", typeof(object));
            DataRow row = dt.NewRow();
            row[0] = "Codigo";
            row[1] = codRedencion;
            dt.Rows.Add(row);

            reporte.DtValPar = dt;
            reporte.PreviewReport2("VIP.Redencion", "", "", 1, 1, 1, "", "", nomA, "pdf", usuario, newPwd, Formulas, ValFormulas, lbvacio);
            Response.Write("<script language:javascript>w=window.open('" + reporte.Documento + "','','HEIGHT=600,WIDTH=600');</script>");

            ViewState["reporte"] = reporte.Documento;
        }

        private void prepararTablaRedimidos()
        {
            String sql = String.Format(
             "SELECT r.mred_consecutivo AS CODIGO, \n" +
             "       c.mcli_nombrecompleto AS CLIENTE, \n" +
             "       t.tred_nombre AS ESTADO, \n" +
             "       i.mvite_nombre AS ITEM \n" +
             "FROM mvipredencion r  \n" +
             "  LEFT JOIN mvipcliente c ON r.mcli_codigo = c.mcli_codigo  \n" +
             "  LEFT JOIN mvipitem i ON r.mvite_codigo = i.mvite_codigo  \n" +
             "  LEFT JOIN tvipredencion t ON r.tred_codigo = t.tred_codigo \n" +
             "WHERE r.mcli_codigo = '{0}' \n" +
             "AND   r.tred_codigo = 'REDIM'"
             , codCliente);

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            dgRedimidos.DataSource = ds.Tables[0];
            dgRedimidos.DataBind();
        }

        private void prepararTablaPorRedimir()
        {
            String sql = String.Format(
             "SELECT r.mred_consecutivo AS CODIGO, \n" +
             "       c.mcli_nombrecompleto AS CLIENTE, \n" +
             "       t.tred_nombre AS ESTADO, \n" +
             "       i.mvite_nombre AS ITEM \n" +
             "FROM mvipredencion r  \n" +
             "  LEFT JOIN mvipcliente c ON r.mcli_codigo = c.mcli_codigo  \n" +
             "  LEFT JOIN mvipitem i ON r.mvite_codigo = i.mvite_codigo  \n" +
             "  LEFT JOIN tvipredencion t ON r.tred_codigo = t.tred_codigo \n" +
             "WHERE r.mcli_codigo = '{0}' \n" +
             "AND   r.tred_codigo = 'PEND'"
             , codCliente);

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            dgPorRedimir.DataSource = ds.Tables[0];
            dgPorRedimir.DataBind();
        }
	}
}