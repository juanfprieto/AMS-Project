using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Inventarios
{
    public partial class ImpresionCodigodeBarras : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = String.Format(
                  "select PALM_ALMACEN, PALM_DESCRIPCION " +
                  "  from DBXSCHEMA.PALMACEN " +
                  "  order by PALM_ALMACEN"
                , ddlAlmc.SelectedValue);

                Utils.FillDll(ddlAlmc, sql, false);
            }

        }
        protected void ddlAlmc_OnSelectedIndexChanged(object sender, EventArgs e)
        {
           // Utils.llenarPrefijos(ref ddlPrefijo, "CD", ddlAlmc.SelectedValue, "FP");
            
        }
        protected void ddlNumEntrada_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = String.Format(
                "SELECT MFAC_NUMEORDEPAGO" +
             "FROM DBXSCHEMA.MFACTURAPROVEEDOR" +
             "ORDER BY MFAC_NUMEORDEPAGO"
             , ddlNumEntrada.SelectedValue);
        }
	}

}