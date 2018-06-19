using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using AMS.DB;
using AMS.Forms;
using System.Collections;

namespace AMS.VIP
{
    public partial class EstadosTesoreria : System.Web.UI.UserControl
	{
        public string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected DatasToControls bind = new DatasToControls();
        protected Hashtable ddlData;
        protected string estadoFactura;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
            }

            estadoFactura = Request.QueryString["estado"];

            ddlData = getDllData();
            prepararTablaFacturas();
            actualizarDdl();
		}

        protected void guardar(object sender, EventArgs e)
        {
            ArrayList updates = new ArrayList();
            string user = HttpContext.Current.User.Identity.Name.ToString().ToLower();
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string sql = "UPDATE MVIPFACTURA SET TVEST_CODIGO='{0}', " + 
                         "SUSU_CODIGO=(select susu_codigo from dbxschema.susuario where " +
                         "susu_login='" + user + "') " +

                         "WHERE MVFAC_CODIGO='{1}'";
            string sqlHistorial = "INSERT INTO MHISTORIAL_FACTURA " +
                                  "(FACTURA, OPERACION, SUSU_USUARIO, FECHA) " +
                                  "VALUES ('{0}', '{1}', '{2}', '{3}')";

            for (int i=0; i < dgFacturas.Items.Count; i++)
            {
                string idFactura = ((DataBoundLiteralControl)dgFacturas.Items[i].Cells[7].Controls[0]).Text.Replace("\n", "").Replace("\r", "").Replace("\t", "");
                string nuevoEstado = ((DropDownList)dgFacturas.Items[i].Cells[13].Controls[1]).SelectedValue;

                if (nuevoEstado != estadoFactura)
                {
                    updates.Add(String.Format(sql, nuevoEstado, idFactura));
                    updates.Add(String.Format(sqlHistorial, idFactura, nuevoEstado, user, now));
                }
            }

            DBFunctions.Transaction(updates);

            String redirect = String.Format("{0}?process=VIP.EstadosTesoreria&estado={1}",
                    indexPage, estadoFactura);

            Response.Redirect(redirect);
        }

        protected void actualizarDdl()
        {
            for (int i=0; i < dgFacturas.Items.Count; i++)
            {
                DropDownList ddl = (DropDownList)dgFacturas.Items[i].Cells[13].Controls[1];

                ddl.DataValueField = "Key";
                ddl.DataTextField = "Value";
                ddl.DataSource = ddlData;
                ddl.DataBind();

                ddl.SelectedValue = estadoFactura;
            }
        }

        private void prepararTablaFacturas()
        {
            String sql = String.Format("SELECT distinct c.mcli_codigo as CODPADRE, \n" +
             "       c.mcli_nombrecompleto as NOMPADRE, \n" +
             "       c.mcli_nit as CEDPADRE, \n" +
             "       c.mcli_telefono TELEFONO, \n" +
             "       c2.mcli_codigo as CODCLI, \n" +
             "       c2.mcli_nombrecompleto as NOMCLI, \n" +
             "       c2.mcli_nit as CEDCLI, \n" +
             "       f.mvfac_codigo CODFAC, \n" +
             "       t.tvfac_nombre TIPOFAC, \n" +
             "       cast(f.mvfac_fechatrans as char(10)) as FECHTRANS, \n" +
             "       cast(f.mvfac_fechapago as char(10)) as FECHVENC, \n" +
             "       f.mvfac_valor VALOR, \n" +
             "       f.mvfac_numaval AVAL \n" +
             "FROM mvipfactura f  \n" +
             "  INNER JOIN mvipcliente c ON c.mcli_codigo = f.mcli_codigo \n" +
             "    LEFT JOIN mviptarjeta ta ON ta.mcli_codigo = c.mcli_codigo \n" +
             "  INNER JOIN mvipcliente c2 ON c2.mcli_codigo = f.mcli_codigocliente \n" +
             "    LEFT JOIN mviptarjeta ta2 ON ta2.mcli_codigo = c2.mcli_codigo \n" +
             "  LEFT JOIN tvipfactura t ON t.tvfac_codigo = f.tvfac_codigo  \n" +
             "  LEFT JOIN tvipestadofactura e ON e.tvest_codigo = f.tvest_codigo \n" +
             "WHERE f.tvest_codigo='{0}'"
             , estadoFactura);

            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

            dgFacturas.DataSource = ds.Tables[0];
            dgFacturas.DataBind();
        }

        private Hashtable getDllData()
        {
            string sql = null;

            switch (estadoFactura)
            {
                case "AD":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'LI' \n" +
                     "OR    tvest_codigo = 'EX' \n" +
                     "OR    tvest_codigo = 'AD'";
                    break;

                case "PR":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'LI' \n" +
                     "OR    tvest_codigo = 'EX' \n" +
                     "OR    tvest_codigo = 'PR'";
                    break;

                case "PE":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'LI' \n" +
                     "OR    tvest_codigo = 'EX' \n" +
                     "OR    tvest_codigo = 'LL1' \n" +
                     "OR    tvest_codigo = 'PE'";
                    break;

                case "LI":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'EX' \n" +
                     "OR    tvest_codigo = 'LI'";
                    break;

                case "LL1":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'LL2' \n" +
                     "OR    tvest_codigo = 'LI' \n" +
                     "OR    tvest_codigo = 'LL1'";
                    break;

                case "LL2":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'IN' \n" +
                     "OR    tvest_codigo = 'LI' \n" +
                     "OR    tvest_codigo = 'LL2'";
                    break;

                case "IN":
                    sql = "SELECT tvest_codigo codigo, \n" +
                     "       tvest_nombre nombre \n" +
                     "FROM tvipestadofactura \n" +
                     "WHERE tvest_codigo = 'INOK' \n" +
                     "OR    tvest_codigo = 'IN'";
                    break;

                case "INOK":
                    sql = "SELECT tvest_codigo codigo,  \n" +
                     "       tvest_nombre nombre  \n" +
                     "FROM tvipestadofactura  \n" +
                     "WHERE tvest_codigo = 'INOK'  \n" +
                     "OR    tvest_codigo = 'LI' \n" +
                     "OR    tvest_codigo = 'EX'";
                    break;
            }

            ArrayList list = (ArrayList)DBFunctions.RequestAsCollection(sql);
            Hashtable table = new Hashtable();

            foreach (Hashtable h in list)
                table.Add(h["CODIGO"], h["NOMBRE"]);

            return table;
        }
	}
}