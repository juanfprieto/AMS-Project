using AMS.Contabilidad;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Vehiculos
{
    public partial class RecepcionAlquiler : System.Web.UI.UserControl
    {
        protected DataTable tablaElementos;
        DatasToControls bind = new DatasToControls();
        ProceHecEco contaOnline = new ProceHecEco();
        protected FormatosDocumentos formatoFactura;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        string usuario = HttpContext.Current.User.Identity.Name;
        double totalOtrosVenta = 0;
        double costoElementos = 0;
        double ivaElementos = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN WHERE TVIG_VIGENCIA = 'V';");
                ddlAlmacen.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                txtFecha.Text = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR WHERE TVEND_CODIGO = 'TT' AND PVEN_VIGENCIA = 'V'");
                this.Preparar_Tabla_Elementos();
                
            }
            else
            {
                if (Session["tablaElementos"] == null)
                {
                    this.Preparar_Tabla_Elementos();
                    
                }
                else
                {
                    tablaElementos = (DataTable)Session["tablaElementos"];
                    Session["tablaElementos"] = tablaElementos;
                }
            }

        }
        protected void Preparar_Tabla_Elementos()
        {
            tablaElementos = new DataTable();
            tablaElementos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));//0
            tablaElementos.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//1
            tablaElementos.Columns.Add(new DataColumn("PLACA", System.Type.GetType("System.String")));//2
            tablaElementos.Columns.Add(new DataColumn("TIEMPO", System.Type.GetType("System.String")));//3
            tablaElementos.Columns.Add(new DataColumn("PERIODO", System.Type.GetType("System.String")));//4
            tablaElementos.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));//5
            tablaElementos.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//6
            tablaElementos.Columns.Add(new DataColumn("TOTAL", System.Type.GetType("System.String")));//7

        }

        protected void dgAccesorioBound(object sender, DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
            {
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].Controls[1]), "SELECT piva_porciva FROM piva");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[4].Controls[1]), "SELECT TUNI_CODIGO, TUNI_DESCRIPCION FROM TUNIDADTIEMPO");
            }
        }
        protected void Documentos_Alquiler(object sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlPrefijoI, "SELECT P.PDOC_CODIGO, P.PDOC_NOMBRE FROM PDOCUMENTO P WHERE P.TDOC_TIPODOCU = 'IA'");
            ddlPrefijoI.Items.Insert(0, "Seleccione..");
            if (ddlPrefijoI.Items.Count < 2)
            {
                Utils.MostrarAlerta(Response, "No ha parametrizado documentos del tipo IA (Recepcion Alquiler)");
                return;
            }
        }
        protected void Consecutivo_Alquiler(object sender, EventArgs e)
        {
            txtNumeroI.Text = DBFunctions.SingleData("SELECT PDOC_ULTIDOCU +1 FROM PDOCUMENTO WHERE PDOC_CODIGO = '" + ddlPrefijoI.SelectedValue + "'");
        }

        protected void Cargue_Documento(Object Sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlFactura, @"SELECT DISTINCT M.PDOC_CODIGO, P.PDOC_DESCRIPCION FROM MORDENALQUILER M, PDOCUMENTO P, DORDENALQUILER D WHERE M.PDOC_CODIGO = P.PDOC_CODIGO AND MNIT_NIT = '" + txtNit.Text + "' AND D.TEST_ESTADO = '2' AND D.PDOC_CODIGO = M.PDOC_CODIGO AND D.MORD_NUMEORDE = M.MORD_NUMEORDE;");

            bind.PutDatasIntoDropDownList(ddlNumFac, @"SELECT DISTINCT M.MORD_NUMEORDE FROM MORDENALQUILER M, PDOCUMENTO P, DORDENALQUILER D WHERE M.PDOC_CODIGO = P.PDOC_CODIGO AND MNIT_NIT = '" + txtNit.Text+ "' AND D.TEST_ESTADO = '2' AND D.PDOC_CODIGO = M.PDOC_CODIGO AND D.MORD_NUMEORDE = M.MORD_NUMEORDE;");

            ddlNumFac.Items.Insert(0, new ListItem("Seleccione...", "0"));
        }
        protected void Cargar_Datos(object sender, EventArgs e)
        {
            btnInfo.Enabled = false;
            PlcElementos.Visible = true;
            tablaElementos = Alquiler();
            Session["tablaAlq"] = tablaElementos;
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();

            for (int i = 0; i < tablaElementos.Rows.Count; i++)
            {
                totalOtrosVenta += (Convert.ToDouble(tablaElementos.Rows[i][3]) * Convert.ToDouble(tablaElementos.Rows[i][4]) + (Convert.ToDouble(tablaElementos.Rows[i][3]) * Convert.ToDouble(tablaElementos.Rows[i][4]) * (Convert.ToDouble(tablaElementos.Rows[i][6]) * 0.01)));
                costoElementos += Convert.ToDouble(tablaElementos.Rows[i][3]) * Convert.ToDouble(tablaElementos.Rows[i][4]);
                ivaElementos += Convert.ToDouble(tablaElementos.Rows[i][3]) * Convert.ToDouble(tablaElementos.Rows[i][4]) * (Convert.ToDouble(tablaElementos.Rows[i][6]) * 0.01);
                grillaElementos.Items[i].Cells[2].HorizontalAlign = HorizontalAlign.Right;
            }


            costoOtrosElementos.Text = costoElementos.ToString("C").Replace(")", "");
            txtIva.Text = (ivaElementos.ToString("C")).Replace(")", "");
            try { totalVenta.Text = totalOtrosVenta.ToString("C").Replace(")", ""); }
            catch { }
        }
        public DataTable Alquiler()
        {
            DataSet Alq = new DataSet();
            DBFunctions.Request(Alq, IncludeSchema.NO, @"SELECT D.MAFJ_CODIACTI AS CODIGO, M.MAFJ_DESCRIPCION AS DESCRIPCION,M.MAFJ_PLACA AS PLACA, COALESCE (D.DORD_VALOR,0) AS VALOR,DORD_TIEMPO AS TIEMPO,D.TUNI_CODIGO AS PERIODO,PIVA_PORCIVA AS IVA, D.DORD_VALOR * D.DORD_TIEMPO +(D.DORD_VALOR * D.DORD_TIEMPO * PIVA_PORCIVA)*0.01 AS TOTAL
                                                          FROM DORDENALQUILER D, MACTIVOFIJO M WHERE D.MAFJ_CODIACTI = M.MAFJ_CODIACTI AND TEST_ESTADO = 2 
                                                          AND D.PDOC_CODIGO = '" + ddlFactura.SelectedValue+ "' AND MORD_NUMEORDE = "+ddlNumFac.SelectedValue+" ");
            return Alq.Tables[0];
        }

        protected void Recepcionar_Alquiler(object sender, EventArgs e)
        {
            bool seleccionado = false;
            seleccionado = ((CheckBox)grillaElementos.Items[0].Cells[8].FindControl("cbRows")).Checked;
            if (seleccionado == false)
            { Utils.MostrarAlerta(Response, "No ha seleccionado Activos para recepcionar. Verifique por favor!"); return; }
            ArrayList sqlRefs = new ArrayList();
            DataTable dtElementos = Session["tablaAlq"] as DataTable;
            if ((DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + ddlVendedor.SelectedValue + "'") != tbClaveVend.Text.Trim()) || (ddlVendedor.SelectedValue == "0"))
            {
                Utils.MostrarAlerta(Response, "La clave de " + ddlVendedor.SelectedItem.Text + " es invalida.\\nRevise Por Favor!");
                return;
            }
            for (int j = 0; j < dtElementos.Rows.Count; j++)
            {
                seleccionado = ((CheckBox)grillaElementos.Items[j].Cells[8].FindControl("cbRows")).Checked;
                if (seleccionado)
                {
                    sqlRefs.Add("UPDATE MALQUILERACTIVOS SET TEST_ESTADO = 1 WHERE MAFJ_CODIACTI = '" + dtElementos.Rows[j][0] + "'");
                    sqlRefs.Add("UPDATE DORDENALQUILER SET DORD_PROCESO = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE PDOC_CODIGO = '"+ ddlFactura.SelectedValue + "' AND MORD_NUMEORDE = " + ddlNumFac.SelectedValue + " AND MAFJ_CODIACTI = '" + dtElementos.Rows[j][0] + "'");

                }
            }

            if (DBFunctions.Transaction(sqlRefs))
            {
                Utils.MostrarAlerta(Response, "Se ha recepcionado el activo con prefijo " + ddlPrefijoI.SelectedValue.ToString() + " y número " + txtNumeroI.Text.ToString() + "");
                formatoFactura = new FormatosDocumentos();
                try
                {
                    formatoFactura.Prefijo = ddlPrefijoI.SelectedValue.ToString();
                    formatoFactura.Numero = Convert.ToInt32(txtNumeroI.Text);
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefijoI.SelectedValue + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                    }
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefijoI.SelectedValue + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=500,WIDTH=700');</script>");
                    }
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Session.Clear();
                    Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionAlquiler&prefD=" + ddlPrefijoI.SelectedValue + "&numD=" + txtNumeroI.Text + "");
                }
                catch
                {
                    lberror.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                }
            }
            else
            {
                lberror.Text = "Error ejecutando:" + DBFunctions.exceptions;
            }
        }
     }
}