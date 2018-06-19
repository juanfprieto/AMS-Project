using System;
using Ajax;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using AMS.Forms;
using AMS.DB;
using System.Collections;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class CapturaDatosLector : System.Web.UI.UserControl
	{
        private DatasToControls bind = new DatasToControls();
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Inventarios.CapturaDatosLector));
            txtCodigoReferencia.Focus();

            if (!IsPostBack)
            {
                // Carga el DropDownList con la lista de inventarios.
                bind.PutDatasIntoDropDownList(ddlInventarios, "SELECT INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),COUNT(DINV.dinv_mite_codigo) " +
                    "FROM minventariofisico INF INNER JOIN dinventariofisico DINV ON INF.pdoc_codigo = DINV.pdoc_codigo AND INF.minf_numeroinv = DINV.minf_numeroinv " +
                    "WHERE INF.minf_fechacierre IS NULL AND INF.minf_fechainicio <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND minf_fechacierre is null " +
                    "GROUP BY INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)), CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)) " +
                    "HAVING COUNT(DINV.dinv_mite_codigo) > 0 " +
                    "ORDER BY CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10))");

                ddlInventarios.Items.Insert(0, new ListItem("Seleccione...", String.Empty));

            }
		}

        protected void btnCancelar_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("" + indexPage + "?process=Inventarios.CapturaDatosLector");
        }

        protected void btnAceptar_Click(object sender, System.EventArgs e)
        {
            if (ddlInventarios.SelectedIndex > 0)
            {
                // Muestra el panel de información de proceso.
                ddlInventarios.Enabled = false;
                pnlInfoProceso.Visible = true;

                // Carga el código del inventario físico.
                string prefijoInventario;
                int numeroInventario;

                if (ddlInventarios.SelectedValue.Split('-').Length == 3)
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

                }
                else
                {
                    prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                    numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
                }

                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen,palm_descripcion FROM palmacen WHERE palm_almacen IN (SELECT dinv_palm_almacen FROM dinventariofisico WHERE pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario + ") and tvig_vigencia='V' ORDER BY palm_descripcion");
                pnlInfoProceso.Visible = true;
                txtCantidadLeida.Text = "1";
               
            }
        }

        protected void btnGuardarConteo_Click(object sender, System.EventArgs e)
        {
            // Carga el código del inventario físico.
            string prefijoInventario;
            int numeroInventario;

            if (ddlInventarios.SelectedValue.Split('-').Length == 3)
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
            }

            ArrayList sqlStrings = new ArrayList();
            sqlStrings.Add("INSERT INTO DINVENTARIOFISICOLECTOR(DINV_SECUENCIA, PDOC_CODIGO, MINF_NUMEROINV, PALM_ALMACEN, DINV_CONTEO, MITE_CODIGO)" +
                            " VALUES( DEFAULT, '" + prefijoInventario + "'," + numeroInventario + ", '" + ddlAlmacen.SelectedValue + "'," + txtCantidadLeida.Text + ", '" + txtCodigoReferencia.Text + "');");
            
            if (DBFunctions.Transaction(sqlStrings))
            {
                lblInfo.Text = "Registrado Item Codigo: " + txtCodigoReferencia.Text;
                txtCodigoReferencia.Text = "";
                txtCantidadLeida.Text = "1";
            }
            else
            {
                lblInfo.Text = "Error al regitrar Item Codigo: " + txtCodigoReferencia.Text;
            }
        }

        [Ajax.AjaxMethod]
        public string Cargar_Item(string codigoItem)
        {
            codigoItem = codigoItem.ToString().Trim();
            codigoItem = codigoItem.ToUpper();
            string nombreItem = DBFunctions.SingleData("select mite_nombre from dbxschema.mitems where mite_codigo='" + codigoItem + "';");
            return nombreItem;
        }

	}
}