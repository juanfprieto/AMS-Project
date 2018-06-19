using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;
using System.Data;
using System.Collections;

namespace AMS.Inventarios
{
	public partial class DeterioroInventarios : System.Web.UI.UserControl
	{
        protected DataTable dtItemsSeleccionados;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session.Remove("dtItemsSeleccionado");
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlLinea, "select plin_codigo, plin_nombre from PLINEAITEM order by plin_nombre;");
                if (ddlLinea.Items.Count > 1)
                    ddlLinea.Items.Insert(0, new ListItem("Seleccione...", string.Empty));

                bind.PutDatasIntoDropDownList(ddlAlmacen, "select palm_almacen, palm_descripcion from palmacen order by palm_descripcion;");
                if (ddlAlmacen.Items.Count > 1)
                    ddlAlmacen.Items.Insert(0, new ListItem("Seleccione...", string.Empty));

                bind.PutDatasIntoDropDownList(ddlYear, "select * from pano;");
                if (ddlYear.Items.Count > 1)
                    ddlYear.Items.Insert(0, new ListItem("Seleccione...", string.Empty));

                bind.PutDatasIntoDropDownList(ddlCentroCosto, "SELECT pcen_codigo, pcen_nombre FROM pcentrocosto order by pcen_nombre;");
                if (ddlCentroCosto.Items.Count > 1)
                    ddlCentroCosto.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
            }
		}

        protected void DefinirTablaItemSeleccionado()
        {
            dtItemsSeleccionados = new DataTable();
            dtItemsSeleccionados.Columns.Add(new DataColumn("ANO", typeof(string)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("ALMACEN", typeof(string)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("CODIGO", typeof(string)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("CANT_ACTUAL", typeof(double)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("COST_PROMEDIO", typeof(double)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("DIAS_ROTACION", typeof(double)));
            dtItemsSeleccionados.Columns.Add(new DataColumn("DETERIORO", typeof(double)));
        }

        protected void CargarItems_Click(object Sender, EventArgs e)
        {
            string filtroSQL = "";
            if (ddlLinea.SelectedValue == "Seleccione...") // cuando no seleccionan una línea específica
                ddlLinea.SelectedValue = "";
            if (ddlAlmacen.SelectedValue == "Seleccione...") // cuando no seleccionan un almacén específico
                ddlAlmacen.SelectedValue = "";
            if (ddlLinea.SelectedValue != "")
                filtroSQL += "and plin_codigo = '" + ddlLinea.SelectedValue + "' ";
            if(txtFamilia.Text != "")
                filtroSQL += "and mi.pfam_codigo='" + txtFamilia.Text + "' ";
            if(ddlAlmacen.SelectedValue != "")
                filtroSQL += "and ms.palm_almacen='" + ddlAlmacen.SelectedValue + "' ";
            if (ddlYear.SelectedValue != "")
                filtroSQL += "and ms.pano_ano = " + ddlYear.SelectedValue + " ";

            if (filtroSQL == "")
            {
                Utils.MostrarAlerta(Response, "Por favor seleccione un filtro!");
                return;
            }

            DataSet dsItems = new DataSet();
            DBFunctions.Request(dsItems, IncludeSchema.NO,
                "select ms.pano_ano as ANO, p.palm_almacen CONCAT '-' CONCAT p.palm_descripcion as ALMACEN, ms.mite_codigo as CODIGO, mi.mite_nombre as DESCRIPCION, ms.msal_cantactual as CANT_ACTUAL, ms.msal_costprom as COST_PROMEDIO, 0 AS DIAS_ROTACION, 0 AS DETERIORO from MSALDOITEMALMACEN ms, mitems mi, palmacen p " +
                "where mi.mite_codigo=ms.mite_codigo " + filtroSQL + " and ms.palm_almacen = p.palm_almacen and ms.msal_cantactual > 0  order by ANO, ALMACEN, DESCRIPCION;");

            plcItems.Visible = true;
            dgItems.DataSource = dsItems;
            dgItems.DataBind();
        }

        protected void DgItems_DataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Session["dtItemsSeleccionado"] != null)
                {
                    string ano = ((DataBoundLiteralControl)e.Item.Cells[0].Controls[0]).Text.Trim();
                    string almacen = ((DataBoundLiteralControl)e.Item.Cells[1].Controls[0]).Text.Trim();
                    string codigo = ((DataBoundLiteralControl)e.Item.Cells[2].Controls[0]).Text.Trim();
                    dtItemsSeleccionados = (DataTable)Session["dtItemsSeleccionado"];
                    DataRow[] drItemsSel = dtItemsSeleccionados.Select("ANO='" + ano + "' AND ALMACEN='" + almacen + "' AND CODIGO='" + codigo + "'");
                    if (drItemsSel.Length > 0)
                    {
                        ((CheckBox)e.Item.Cells[6].FindControl("cbRows")).Checked = true;
                        ((CheckBox)e.Item.Cells[6].FindControl("cbRows")).Enabled = false;
                    }
                }
            }
        }

        protected void DgItems_onCommand(object sender, DataGridCommandEventArgs e)
        {
            if (Session["dtItemsSeleccionado"] == null)
            {
                DefinirTablaItemSeleccionado();
            }
            else
            {
                dtItemsSeleccionados = (DataTable)Session["dtItemsSeleccionado"];
            }

            if (((Button)e.CommandSource).CommandName == "AgrearItems")
            {
                foreach (DataGridItem item in dgItems.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        if (((CheckBox)item.Cells[8].FindControl("cbRows")).Checked &&
                               ((CheckBox)item.Cells[8].FindControl("cbRows")).Enabled == true)
                        {
                            DataRow filaItem = dtItemsSeleccionados.NewRow();
                            filaItem["ANO"] = (((DataBoundLiteralControl)(item.Cells[0].Controls[0]))).Text.Trim();
                            filaItem["ALMACEN"] = (((DataBoundLiteralControl)(item.Cells[1].Controls[0]))).Text.Trim();
                            filaItem["CODIGO"] = (((DataBoundLiteralControl)(item.Cells[2].Controls[0]))).Text.Trim();
                            filaItem["DESCRIPCION"] = (((DataBoundLiteralControl)(item.Cells[3].Controls[0]))).Text.Trim();
                            filaItem["CANT_ACTUAL"] = (((DataBoundLiteralControl)(item.Cells[4].Controls[0]))).Text.Trim();
                            filaItem["COST_PROMEDIO"] = (((DataBoundLiteralControl)(item.Cells[5].Controls[0]))).Text.Trim();
                            filaItem["DIAS_ROTACION"] = (((DataBoundLiteralControl)(item.Cells[6].Controls[0]))).Text.Trim();
                            filaItem["DETERIORO"] = (((DataBoundLiteralControl)(item.Cells[7].Controls[0]))).Text.Trim();

                            dtItemsSeleccionados.Rows.Add(filaItem);
                            ((CheckBox)item.Cells[6].FindControl("cbRows")).Enabled = false;
                            int contItems = Convert.ToInt16(lblContadorItems.Text);
                            contItems++;
                            lblContadorItems.Text = contItems.ToString();
                        }
                    }
                }

                Session["dtItemsSeleccionado"] = dtItemsSeleccionados;
            }
        }

        protected void ConfirmarSelecccion_Click(object Sender, EventArgs e)
        {
            btnCargarItems.Visible = false;
            plcItems.Visible = false;
            plcSeleccion.Visible = true;
            dtItemsSeleccionados = (DataTable)Session["dtItemsSeleccionado"];
            lblSeleccionItems.Text = lblContadorItems.Text;
            dgItemsSeleccion.DataSource = dtItemsSeleccionados;
            dgItemsSeleccion.DataBind();
        }

        protected void DgItSeleccion_onCommand(object sender, DataGridCommandEventArgs e)
        {
            dtItemsSeleccionados = (DataTable)Session["dtItemsSeleccionado"];

            if (((Button)e.CommandSource).CommandName == "EliminarItems")
            {
                int fila = 0;
                foreach (DataGridItem item in dgItemsSeleccion.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        if (((CheckBox)item.Cells[6].FindControl("cbRows")).Checked)
                        {
                            dtItemsSeleccionados.Rows.RemoveAt(fila);

                            int selItems = Convert.ToInt16(lblSeleccionItems.Text);
                            selItems--;
                            lblSeleccionItems.Text = selItems.ToString();
                        }
                        else
                            fila++;
                    }
                }

                dgItemsSeleccion.DataSource = dtItemsSeleccionados;
                dgItemsSeleccion.DataBind();
                Session["dtItemsSeleccionado"] = dtItemsSeleccionados;
            }
        }

        protected void GenerarComprobante_Click(object Sender, EventArgs e)
        {
            DataSet dsListaPrecio = new DataSet();
            ArrayList sqlStrings = new ArrayList();
            string insertSQL = "";
            double sumaDeterioro = 0;
            string cuentaDeteInvDebi = DBFunctions.SingleData("select ccon_deteinvedebi from CCONTABILIDAD");
            string cuentaDeteInvCre = DBFunctions.SingleData("select ccon_deteinvecred from CCONTABILIDAD");
            int contSecuencia = 1;
            string nitEmpresa = DBFunctions.SingleData("select mnit_nit from cempresa");

            //Codigo 99 para lista de precio de Deterior de Inventarios.
            DBFunctions.Request(dsListaPrecio, IncludeSchema.NO, "select distinct mite_codigo as CODIGO, mpre_precio as PRECIO from mprecioitem"); // where ppre_codigo='3';"); //'99';");
            dtItemsSeleccionados = (DataTable)Session["dtItemsSeleccionado"];

            for (int h = 0; h < dtItemsSeleccionados.Rows.Count; h++)
            {

                DataRow[] drListaPrecio = drListaPrecio = dsListaPrecio.Tables[0].Select("CODIGO='" + dtItemsSeleccionados.Rows[h]["CODIGO"] + "'");

                if (drListaPrecio.Length == 0)
                {
                    Utils.MostrarAlerta(Response, "Referencia sin precio! Por favor ingrese VNR para la referencia: " + dtItemsSeleccionados.Rows[h]["CODIGO"]);
                    return;
                }

                double precioListaItem = Convert.ToDouble(drListaPrecio[0].ItemArray[1].ToString());
                double precioPromeItem = Convert.ToDouble(dtItemsSeleccionados.Rows[h]["COST_PROMEDIO"].ToString());
                double diferenciaPrecio = precioPromeItem - precioListaItem;
                if (diferenciaPrecio > 0)
                {
                    string codAlmacen = dtItemsSeleccionados.Rows[h]["ALMACEN"].ToString();
                    codAlmacen = codAlmacen.Substring(0, codAlmacen.IndexOf('-'));
                    double cantidadItem = Convert.ToDouble(dtItemsSeleccionados.Rows[h]["CANT_ACTUAL"].ToString());

                    insertSQL = "INSERT INTO DDETERIOROINVENTARIO VALUES (DEFAULT,";
                    insertSQL += dtItemsSeleccionados.Rows[h]["ANO"].ToString() + ",";
                    insertSQL += "'" + codAlmacen + "',";
                    insertSQL += "'" + dtItemsSeleccionados.Rows[h]["CODIGO"].ToString() + "',";
                    insertSQL += "'" + DBFunctions.SingleData("SELECT plin_codigo FROM MITEMS where mite_codigo='" + dtItemsSeleccionados.Rows[h]["CODIGO"].ToString() + "';") + "',";
                    insertSQL += Math.Round(precioPromeItem) + ",";
                    insertSQL += Math.Round(precioListaItem) + ",";
                    insertSQL += cantidadItem + ",";
                    insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "');";


                    sumaDeterioro += diferenciaPrecio * cantidadItem;
                    sqlStrings.Add(insertSQL);
                }
            }

            //Creacion del comprobante de deterioro
            string numeroComprobante = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM pdocumento WHERE PDOC_CODIGO='1031'");

            insertSQL = "INSERT INTO MCOMPROBANTE VALUES('1031',";
            insertSQL += numeroComprobante + ",";
            insertSQL += DateTime.Now.Year + ",";
            insertSQL += DateTime.Now.Month + ",";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            insertSQL += "'COMPROBANTE NIIF DETERIORO DE INVENTARIOS',";
            insertSQL += "'" + DateTime.Now.ToString("yyyy-MM-dd") + "',";
            insertSQL += "'" + HttpContext.Current.User.Identity.Name.ToLower() + "',";
            insertSQL += sumaDeterioro + ");";

            sqlStrings.Add(insertSQL);

            string numeroDocumento = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU + 1 FROM pdocumento WHERE PDOC_CODIGO='9000'");

            //creacion de detalles.
            for (int k = 0; k < dtItemsSeleccionados.Rows.Count; k++)
            {
                string codAlmacen = dtItemsSeleccionados.Rows[k]["ALMACEN"].ToString();
                codAlmacen = codAlmacen.Substring(0, codAlmacen.IndexOf('-'));

                DataRow[] drListaPrecio = dsListaPrecio.Tables[0].Select("CODIGO='" + dtItemsSeleccionados.Rows[k]["CODIGO"] + "'");
                double precioListaItem = Convert.ToDouble(drListaPrecio[0].ItemArray[1].ToString());
                double precioPromeItem = Convert.ToDouble(dtItemsSeleccionados.Rows[k]["COST_PROMEDIO"].ToString());
                double diferenciaPrecio = precioPromeItem - precioListaItem;
                double cantidadItem = Convert.ToDouble(dtItemsSeleccionados.Rows[k]["CANT_ACTUAL"].ToString());

                //dcuenta debito
                insertSQL = "INSERT INTO DCUENTA VALUES(";
                insertSQL += "'1031',";
                insertSQL += numeroComprobante + ",";
                insertSQL += "'" + cuentaDeteInvDebi + "',"; ;
                insertSQL += "'9000',";
                insertSQL += numeroDocumento + ",";
                insertSQL += contSecuencia + ",";
                insertSQL += "'" + nitEmpresa + "',";
                insertSQL += "'" + codAlmacen + "',";
                insertSQL += "'" + ddlCentroCosto.SelectedValue + "',";
                insertSQL += "'DETERIORO INVENTARIO NIIF',";
                insertSQL += "0,0,0,";
                insertSQL += (diferenciaPrecio * cantidadItem) +",";
                insertSQL += "0);";

                contSecuencia++;
                sqlStrings.Add(insertSQL);

                //dcuenta credito
                insertSQL = "INSERT INTO DCUENTA VALUES(";
                insertSQL += "'1031',";
                insertSQL += numeroComprobante + ",";
                insertSQL += "'" + cuentaDeteInvCre + "',"; ;
                insertSQL += "'9000',";
                insertSQL += numeroDocumento + ",";
                insertSQL += contSecuencia + ",";
                insertSQL += "'" + nitEmpresa + "',";
                insertSQL += "'" + codAlmacen + "',";
                insertSQL += "'" + ddlCentroCosto.SelectedValue + "',";
                insertSQL += "'DETERIORO INVENTARIO NIIF',";
                insertSQL += "0,0,0,";
                insertSQL += "0,";
                insertSQL += (diferenciaPrecio * cantidadItem) + ");";

                contSecuencia++;
                sqlStrings.Add(insertSQL);
            }

            insertSQL = "UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO='1031'";
            sqlStrings.Add(insertSQL);

            insertSQL = "UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO='9000'";
            sqlStrings.Add(insertSQL);

            //Pendiente el calculo del promedio para NIIF...

            if (DBFunctions.Transaction(sqlStrings))
                Utils.MostrarAlerta(Response, "Proceso finalizado con exito! Se ha creado el compronante: 1031-" + numeroComprobante + ". Y el registro 9000-" + numeroDocumento);
            else
            {
                Utils.MostrarAlerta(Response, "Error en el proceso!");
                lb.Text += DBFunctions.exceptions;
            }
        }

	}
}