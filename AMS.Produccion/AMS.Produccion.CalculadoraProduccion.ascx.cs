using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using System.Data;
using AMS.Tools;
using AMS.DB;
using System.Collections;
using System.Configuration;
using System.Drawing;

namespace AMS.Produccion
{
	public partial class CalculadoraProduccion : System.Web.UI.UserControl
	{
        private DataTable ensambles;
        private DataTable items;
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected string images = ConfigurationManager.AppSettings["PathToImages"];

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                ViewState["ensambles"] = CrearTablaEnsamble();
                ViewState["items"] = CrearTablaItems();

                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlEnsambles,
                    "SELECT MITE_CODIGO, MITE_CODIGO concat ' [' concat pens_descripcion concat ']' FROM PENSAMBLEPRODUCCION WHERE PENS_VIGENTE='S' AND MITE_CODIGO IS NOT NULL ORDER BY MITE_CODIGO");

                bind.PutDatasIntoDropDownList(ddlLotes,
                    "SELECT mprog_numero from MPROGRAMAPRODUCCION mp " +
                    "where mp.mprog_consecutivo in (" +
                    " select dp.mprog_consecutivo " +
                    " from DPROGRAMAPRODUCCION dp " +
                    " where " +
                    " dp.mprog_consecutivo=mp.mprog_consecutivo and " +
                    " dp.MITE_CODIGO IS NOT NULL and dp.dprog_cantidad > dp.dprog_total) ORDER BY mprog_numero;");

            }

            ensambles = (DataTable)ViewState["ensambles"];
            items = (DataTable)ViewState["items"];

            creacionOrden.Controls.Add(LoadControl(pathToControls + "AMS.Produccion.OrdenProduccion.ascx"));
		}

        private DataTable CrearTablaEnsamble()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ITEM", typeof(string));
            table.Columns.Add("NOMBRE", typeof(string));
            table.Columns.Add("CANTIDAD", typeof(int));

            return table;
        }

        private DataTable CrearTablaItems()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ITEM", typeof(string));
            table.Columns.Add("CANTIDAD", typeof(float));
            table.Columns.Add("ALMACEN", typeof(string));
            table.Columns.Add("DISPONIBLE", typeof(float));

            return table;
        }

        private void saveAndBind()
        {
            ViewState["ensambles"] = ensambles;
            ViewState["items"] = items;

            AgregarDummyItems();

            DataView dvItems = new DataView(items);
            dvItems.Sort = "ITEM ASC";

            dgEnsambles.DataSource = ensambles;
            dgItems.DataSource = dvItems;

            dgEnsambles.DataBind();
            dgItems.DataBind();


            dgEnsambles.Visible = true;
            dgItems.Visible = true;

            RowSpanItems();
        }

        private void AgregarDummyItems()
        {
            DataRow row = items.NewRow();
            row["ITEM"] = "zzzzzzzzzzzzzZzZz"; //ultimo lugar
            row["CANTIDAD"] = 0;
            row["ALMACEN"] = "";
            row["DISPONIBLE"] = 0;
            items.Rows.Add(row);
        }

        private void actualizarCantidadesEnsambles()
        {
            foreach (DataGridItem item in dgEnsambles.Items)
            { 
                if(item.Cells[2].Controls[1] != null &&
                item.Cells[2].Controls[1].GetType() == typeof(TextBox))
                {
                    string cant = ((TextBox)item.Cells[2].Controls[1]).Text;
                    ensambles.Rows[item.DataSetIndex]["CANTIDAD"] = cant;
                } 
            }
        }

        private void agregarItems(string codItem, string itemPadre, double multiplicador)
        {
            string sql = String.Format(
             "SELECT mep.mite_codigo AS ITEM, " +
             "       mep.mens_cantidad AS CANTIDAD, " +
             "       pep2.pens_codigo AS ENSAMBLE, " +
             "       pal.palm_descripcion AS ALMACEN, " +
             "       msi.msal_cantactual AS DISPONIBLE " +
             "FROM PENSAMBLEPRODUCCION pep  " +
             "  INNER JOIN MENSAMBLEPRODUCCIONITEMS mep " +
             "		  LEFT JOIN MSALDOITEMALMACEN msi " +
             "				  LEFT JOIN palmacen pal ON msi.palm_almacen = pal.palm_almacen " +
             "				  INNER JOIN cinventario cin ON msi.pano_ano = cin.pano_ano " +
             "		  ON mep.mite_codigo = msi.mite_codigo " +
             "		  LEFT JOIN pensambleproduccion pep2 ON mep.mite_codigo = pep2.mite_codigo " +
             "		  									AND pep2.pens_vigente = 'S' AND mep.tpro_codigo = 'P' " +
             "  ON mep.pens_codigo = pep.pens_codigo " +
             "WHERE pep.mite_codigo LIKE '{0}' " +
             "ORDER BY mep.mite_codigo"
             , codItem);

            ArrayList list = (ArrayList)DBFunctions.RequestAsCollection(sql);            

            foreach (Hashtable hash in list)
            {
                string item = itemPadre == null ? hash["ITEM"] as string : String.Format("{0} - {1}", itemPadre, hash["ITEM"]);
                double cantidad = Convert.ToDouble(hash["CANTIDAD"]) * multiplicador;
                string almacen = hash["ALMACEN"] as string;

                DataRow rowsExistente = 
                    (from DataRow c in items.Rows
                    where (string)c["ITEM"] == item && (string)c["ALMACEN"] == almacen
                    select c).FirstOrDefault();

                if (rowsExistente != null)
                {
                    rowsExistente["CANTIDAD"] = Convert.ToDouble(hash["CANTIDAD"]) + cantidad;
                    continue;
                }

                DataRow row = items.NewRow();
                row["ITEM"] = item;
                row["CANTIDAD"] = cantidad;
                row["ALMACEN"] = almacen;
                row["DISPONIBLE"] = hash["DISPONIBLE"];
                items.Rows.Add(row);
            }

            if (!chkRecursivo.Checked) return;

            var subEnsambles =
                    (from Hashtable c in list
                    where c["ENSAMBLE"].GetType() != typeof(DBNull) 
                    select new {item = c["ITEM"], cantidad = c["CANTIDAD"]}).Distinct();

            foreach (var subEnsamble in subEnsambles)
            {
                string item = subEnsamble.item as string;
                string padre = itemPadre == null ? subEnsamble.item as string : String.Format("{0} - {1}", itemPadre, subEnsamble.item);
                double cantidad = Convert.ToDouble(subEnsamble.cantidad) * multiplicador;

                agregarItems(item, padre, cantidad);
            }
        }

        protected void btnAgregarEnsamble_Click(object source, EventArgs e)
        {

            string item = ddlEnsambles.SelectedValue;
            string sql = String.Format(
             "SELECT pens_descripcion " +
             "FROM pensambleproduccion " +
             "WHERE MITE_CODIGO = '{0}'",
             item);

            string name = DBFunctions.SingleData(sql);

            DataRow row = ensambles.NewRow();
            row["ITEM"] = item;
            row["NOMBRE"] = name;
            row["CANTIDAD"] = 1;
            ensambles.Rows.Add(row);

            btnCalc_Click(source, e);
        }

        protected void btnAgregarLote_Click(object source, EventArgs e)
        {

            string lote = ddlLotes.SelectedValue;
            string sql = String.Format(
             "SELECT DISTINCT mi.mite_codigo ITEM, " +
             "       pe.pens_descripcion NOMBRE " +
             "FROM mitems mi, " +
             "     pensambleproduccion pe, " +
             "     dprogramaproduccion dp, " +
             "     mprogramaproduccion mp " +
             "WHERE mi.mite_codigo = pe.mite_codigo " +
             "AND   mi.mite_codigo = dp.mite_codigo " +
             "AND   dp.mprog_consecutivo = mp.mprog_consecutivo " +
             "AND   mp.mprog_numero = '{0}' " +
             "ORDER BY mi.mite_codigo"
             ,lote);

            ArrayList list = (ArrayList)DBFunctions.RequestAsCollection(sql);

            foreach (Hashtable hash in list)
            {
                DataRow row = ensambles.NewRow();
                row["ITEM"] = hash["ITEM"];
                row["NOMBRE"] = hash["NOMBRE"];
                row["CANTIDAD"] = 1;
                ensambles.Rows.Add(row);
            }

            DropDownList ddlLote = (DropDownList)creacionOrden.Controls[0].FindControl("ddlLote");

            if (ddlLote.Enabled)
                ddlLote.SelectedValue = lote;
            
            btnCalc_Click(source, e);
        }

        protected void btnCalc_Click(object source, EventArgs e)
        {
            actualizarCantidadesEnsambles();
            items.Clear();

            foreach (DataRow row in ensambles.Rows)
            {
                double cantidad = 1;
                if(Utils.EsNumero(row["CANTIDAD"].ToString()))
                    cantidad = Convert.ToDouble(row["CANTIDAD"]);

                agregarItems(row["ITEM"].ToString(), null, cantidad);
            }

            saveAndBind();
            marcarItemsFaltantes();
        }

        protected void dgEnsambles_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            ensambles.Rows[e.Item.DataSetIndex].Delete();
            dgEnsambles.DataBind();
            dgItems.Visible = false;
        }

        protected void dgItems_OnItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)
                                                            e.Item.Cells[4].FindControl("imgIntercam");
                string refItem = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0])
                            .Text.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim();

                if (refItem.Contains("-"))
                {
                    string[] aux = refItem.Split('-');
                    refItem = aux[aux.Length - 1].Trim();
                }
                
                string sql1 = String.Format(
                    "SELECT MITE_INERCAM " +
                    "FROM MITEMINTERCAMBIABLE \n" +
                    "WHERE mite_codigo = '{0}'"
                    , refItem);


                //Response.Write("<script language='javascript'>alert('*" + refItem + "*');</script>");

                if (DBFunctions.RecordExist(sql1))
                {

                    string sql = String.Format(
                     "SELECT mie.MITE_INERCAM AS REFERENCIA, " +
                     "pal.palm_descripcion AS ALMACEN, " +
                     "msi.msal_cantactual AS EXISTENCIAS " +
                     "FROM MITEMINTERCAMBIABLE mie  " +
                     "LEFT JOIN MSALDOITEMALMACEN msi " +
                     "LEFT JOIN palmacen pal ON msi.palm_almacen = pal.palm_almacen " +
                     "INNER JOIN cinventario cin ON msi.pano_ano = cin.pano_ano  " +
                     "ON mie.MITE_INERCAM = msi.mite_codigo " +
                     "WHERE mie.mite_codigo LIKE '{0}'"
                     , refItem)
                     .Replace("'", "\\'");

                    img.ImageUrl = images + "AMS.Search.png";
                    img.Attributes.Add("onClick", "ModalDialog(" + img.ClientID + ",'" + sql + "',new Array());");
                    img.ToolTip = String.Format("Intercambiables para {0}", refItem);
                }
                else
                    img.Visible = false;
            }
        }

        private void RowSpanItems()
        {
            int ItemIndex = 0;

            foreach (DataGridItem dgItem in dgItems.Items)
            {
                if (dgItem.ItemIndex > 0)
                {
                    string pre = ((DataBoundLiteralControl)dgItem.Cells[0].Controls[0]).Text;
                    string actual = ((DataBoundLiteralControl)dgItems.Items[dgItem.ItemIndex - 1].Cells[0].Controls[0]).Text;

                    if (pre == actual)
                    {
                        dgItem.Cells[0].Visible = false;
                        dgItem.Cells[1].Visible = false;
                        dgItems.Items[ItemIndex].Cells[0].RowSpan =
                            dgItems.Items[ItemIndex].Cells[0].RowSpan + 1;
                        dgItems.Items[ItemIndex].Cells[1].RowSpan =
                            dgItems.Items[ItemIndex].Cells[1].RowSpan + 1;

                    }
                    else if (dgItems.Items[
                        dgItem.ItemIndex - 1].Cells[0].Visible == true)
                    {

                        ItemIndex = dgItem.ItemIndex;
                    }
                    else
                    {
                        dgItems.Items[ItemIndex].Cells[0].RowSpan =
                            dgItems.Items[ItemIndex].Cells[0].RowSpan + 1;
                        dgItems.Items[ItemIndex].Cells[1].RowSpan =
                            dgItems.Items[ItemIndex].Cells[1].RowSpan + 1;
                        ItemIndex = dgItem.ItemIndex;
                    }
                }
            }

            //escondiendo el dummy... razón desconocida de tener que agregarlo
            dgItems.Items[dgItems.Items.Count - 1].Visible = false;
        }

        private void marcarItemsFaltantes()
        {
            var celdas =
                from DataGridItem k in dgItems.Items
                where (from DataRow c in items.Rows
                       group c by new { item = c["ITEM"], cantidad = c["CANTIDAD"] } into g
                       where g.Sum(s => Convert.ToDecimal(s["DISPONIBLE"].GetType() == typeof(DBNull) ? 0 : s["DISPONIBLE"])) 
                                < Convert.ToDecimal(g.Key.cantidad)
                       select g.Key.item.ToString())
                      .Contains(((DataBoundLiteralControl)k.Cells[0].Controls[0])
                            .Text.Replace("\n", "").Replace("\r", "").Replace("\t", "").Trim())
                select k.Cells[1];

            foreach(TableCell celda in celdas)
                celda.BackColor = Color.Salmon;
        }
    }
}
	
