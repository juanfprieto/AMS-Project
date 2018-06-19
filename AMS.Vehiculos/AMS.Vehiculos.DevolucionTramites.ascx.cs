using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Contabilidad;
using AMS.Tools;



namespace AMS.Vehiculos
{
    public partial class DevolucionTramites : System.Web.UI.UserControl
    {
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected string pathToMain = ConfigurationManager.AppSettings["MainIndexPage"];
        protected FormatosDocumentos formatoConsignacion;
        ProceHecEco contaOnline = new ProceHecEco();
        DatasToControls bind = new DatasToControls();
        protected DataTable tablaDatos;
        protected DataTable tablaDatosTramite;
        protected FormatosDocumentos formatoFactura;
        string usuario = HttpContext.Current.User.Identity.Name;
        DataSet dsT = new DataSet();
        string centrocostos = "";
        int i = 0, sumaserv = 0, sumaDoc = 0, ivaserv = 0, sumaiva = 0, totaltramite = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtvalorServ.Text = "0";
                txtivaServ.Text = "0";
                Session.Clear();
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and tvig_vigencia='V' order by palm_descripcion;");
                almacen.Items.Insert(0, "Seleccione:");
                if (Request.QueryString["prefD"] != null && Request.QueryString["numD"] != null)
                {
                    Response.Write("<script language:javascript>alert('Se ha creado el documento " + Request.QueryString["prefD"] + "-" + Request.QueryString["numD"] + "');</script>");
                    try
                    {
                        formatoConsignacion = new FormatosDocumentos();
                        formatoConsignacion.Prefijo = Request.QueryString["prefD"];
                        formatoConsignacion.Numero = Convert.ToInt32(Request.QueryString["numD"]);
                        formatoConsignacion.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefD"] + "'");
                        if (formatoConsignacion.Codigo != string.Empty)
                        {
                            if (formatoConsignacion.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoConsignacion.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoConsignacion.Mensajes + "<br>";
                    }
                }
            }
            else
            {
                if (Session["tablaDatosTramite"] == null)
                {
                    Cargar_Datos_Tramite();
                }
                else
                {
                    tablaDatosTramite = (DataTable)Session["tablaDatosTramite"];
                    Session["tablaDatosTramite"] = tablaDatosTramite;
                }
            }
        }
        protected void cargarDocumentos_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!DBFunctions.RecordExist("select P.PDOC_CODIGO, P.PDOC_DESCRIPCION FROM PDOCUMENTOHECHO PD, PDOCUMENTO P WHERE PALM_ALMACEN = '" + almacen.Text + "' AND TPRO_PROCESO = 'FT' AND TVIG_VIGENCIA = 'V' AND P.TDOC_TIPODOCU = 'NC' AND P.PDOC_CODIGO = PD.PDOC_CODIGO and pdoc_numefina > pdoc_numeinic; "))
            {
                prefijoDocumento.Enabled = false;
                prefijoDocumento.Enabled = false;
                aceptar.Enabled = false;
                Utils.MostrarAlerta(Response, "No hay documentos del tipo NC asociados al proceso FT para la sede '" + almacen.Text + "'");
                return;
            }
            else
            {
                bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT P.PDOC_CODIGO, P.PDOC_CODIGO || ' - ' ||P.PDOC_DESCRIPCION FROM PDOCUMENTOHECHO PD, PDOCUMENTO P WHERE PALM_ALMACEN = '" + almacen.Text + "' AND TPRO_PROCESO = 'FT' AND TVIG_VIGENCIA = 'V' AND P.PDOC_CODIGO = PD.PDOC_CODIGO AND TDOC_TIPODOCU = 'NC' order by P.PDOC_DESCRIPCION; ");
                if (prefijoDocumento.Items.Count > 1)
                {
                    prefijoDocumento.Items.Insert(0, "Seleccione:");
                }
                numero.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
                prefijoDocumento.Enabled = true;
                aceptar.Enabled = true;
            }

        }
        protected void cargarConsecutivo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            numero.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
        }

        protected void aceptarTramite_Click(object sender, System.EventArgs e)
        {
            DataSet ds = new DataSet();
            DataRow fila;
            int i = 0;
            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT MO.PDOC_CODIGO AS CODIGO, MORD_NUMEORDE AS NUMERO, DATE (MORD_CREACION) AS FECHA, TEST_ESTADO AS ESTADO, 'CATALOGO:' || ' ' || PCAT_CODIGO || ' - VIN: ' ||  MO.MCAT_VIN || ' - PLACA: ' ||  MC.MCAT_PLACA AS OBSERVACION, MO.MCAT_VIN AS VIN
                                                        FROM  MORDENTRAMITE MO LEFT JOIN MCATALOGOVEHICULO MC  ON ( MO.MCAT_VIN = MC.MCAT_VIN) WHERE MO.MNIT_NIT = '" + txtNit.Text + "' AND   TEST_ESTADO = 'T'; ");
            Session["Datos"] = ds.Tables[0];
            if (ds.Tables[0].Rows.Count != 0)
            {
                Cargar_Tabla_Tramites(ds.Tables[0]);
            }                
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                fila = tablaDatos.NewRow();
                fila["CODIGO"] = ds.Tables[0].Rows[i][0].ToString();
                fila["NUMERO"] = ds.Tables[0].Rows[i][1].ToString();
                fila["FECHA"] = ds.Tables[0].Rows[i][2].ToString();
                fila["ESTADO"] = ds.Tables[0].Rows[i][3].ToString();
                fila["OBSERVACION"] = ds.Tables[0].Rows[i][4].ToString();
                fila["VIN"] = ds.Tables[0].Rows[i][5].ToString();
                tablaDatos.Rows.Add(fila);
                Session["tablaDatos"] = tablaDatos;
                gridDatos.DataSource = tablaDatos;
                gridDatos.DataBind();
            }
            //tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
        }

        protected void Cargar_Tabla_Tramites(DataTable dtable)
        {
            tablaDatos = new DataTable();
            tablaDatos.Columns.Add("CODIGO", typeof(string));
            tablaDatos.Columns.Add("NUMERO", typeof(int));
            tablaDatos.Columns.Add("FECHA", typeof(DateTime));
            tablaDatos.Columns.Add("ESTADO", typeof(string));
            tablaDatos.Columns.Add("OBSERVACION", typeof(string));
            tablaDatos.Columns.Add("VIN", typeof(string));
        }

        protected void Agregar_tramite(object Sender, DataGridCommandEventArgs e)
        {
            ((Panel)FindControl("valoresCruce")).Visible = true;
            ((Button)e.Item.Cells[5].Controls[1]).Enabled = false;
            guardar.Enabled = true;
            aceptar.Enabled = false;
            lbInfo.Visible = true;
            DataTable dt = Session["datos"] as DataTable;         
            DataRow filaTr;
            //dsT.Tables[0] = (DataTable)Session["DatosTramite"];          
            
            //DataTable datos1 = ((DataTable)Session["DatosTramite"]);
            //if(datos1 != null)
            //{
            //    DataTable dtcopy = datos1.Copy();
            //    dsT.Tables.Add(dtcopy);
            //}
            DBFunctions.Request(dsT, IncludeSchema.NO, @"SELECT DISTINCT d.PDOC_CODIGO AS CODIGOTRA, d.MORD_NUMEORDE AS NUMEROTRA, p.PITE_NOMBRE AS DETALLETRA, tc.TCAR_NOMBRE AS CARGOTRA, d.TEST_ESTADO AS CODIGOEST, 'C' AS ESTADOTRA, D.PIVA_PORCIVA AS IVATRA, D.DORD_VALOTRAM AS VALORTRA, P.PITE_CODIGO AS TIPOTRA, D.PTRA_CODIGO AS CODIGO
                                                        FROM DORDENTRAMITE D, PITEMVENTAVEHICULO P, TCARGOORDEN TC, TESTADOTRAMITE TE
                                                        WHERE P.PITE_CODIGO = D.PTRA_CODIGO
                                                        AND TC.TCAR_CARGO = D.tcar_cargo
                                                        --and TE.TEST_ESTADO = D.TEST_ESTADO
                                                        AND D.PDOC_CODIGO = '" + dt.Rows[e.Item.DataSetIndex][0].ToString() + "'  AND D.MORD_NUMEORDE = " + dt.Rows[e.Item.DataSetIndex][1].ToString() + "");
            //Session["DatosTramite"] = dsT.Tables[0];
            for (int j = 0; j < dsT.Tables.Count; j++)
            { 
                //if (dsT.Tables[j].Rows.Count == 0)
                //{
                //    Cargar_Datos_Tramite(dsT.Tables[0]);
                //}
                for (i = 0; i < dsT.Tables[j].Rows.Count; i++)
                {
                    filaTr = tablaDatosTramite.NewRow();
                    filaTr["CODIGOTRA"] = dsT.Tables[j].Rows[i][0].ToString();
                    filaTr["NUMEROTRA"] = dsT.Tables[j].Rows[i][1].ToString();
                    filaTr["DETALLETRA"] = dsT.Tables[j].Rows[i][2].ToString();
                    filaTr["CARGOTRA"] = dsT.Tables[j].Rows[i][3].ToString();
                    filaTr["CODIGOEST"] = dsT.Tables[j].Rows[i][4].ToString();
                    filaTr["ESTADOTRA"] = dsT.Tables[j].Rows[i][5].ToString();
                    if (dsT.Tables[j].Rows[i][6].ToString() != "0.00")
                    {
                        filaTr["IVATRA"] = dsT.Tables[j].Rows[i][6];
                    }
                    else filaTr["IVATRA"] = 0;
                    filaTr["VALORTRA"] = dsT.Tables[j].Rows[i][7].ToString();
                    filaTr["TIPOTRA"] = dsT.Tables[j].Rows[i][8].ToString();
                    filaTr["CODIGO"] = dsT.Tables[j].Rows[i][9].ToString();
                    tablaDatosTramite.Rows.Add(filaTr);
                    Session["tablaDatosTramite"] = tablaDatosTramite;
                    gridDatosTramite.DataSource = tablaDatosTramite;
                    gridDatosTramite.DataBind();
                }              
            }
            foreach (DataRow dr in tablaDatosTramite.Rows)
            {
                if (dr["IVATRA"].ToString() != "0")
                {
                        sumaserv += Convert.ToInt32(dr["VALORTRA"]);
                        txtvalorServ.Text = Convert.ToString(sumaserv);
                        ivaserv = int.Parse(txtvalorServ.Text) * Convert.ToInt32(dr["IVATRA"]) / 100;
                        sumaiva = int.Parse(txtvalorServ.Text) + ivaserv;
                }
                else
                {
                    sumaDoc += Convert.ToInt32(dr["VALORTRA"]);
                    txtValorDoc.Text = Convert.ToString(sumaDoc);
                }  
            }


            txtvalorServ.Text = sumaserv.ToString("C");
            txtivaServ.Text = ivaserv.ToString("C");
            txtValorDoc.Text = sumaDoc.ToString("C");
            txtIvaDoc.Text = "$0.00";
            txtTotalDoc.Text = sumaDoc.ToString("C");
            txtTotal.Text = sumaiva.ToString("C");
            totaltramite = sumaiva + sumaDoc;
            txtTotalTramite.Text = totaltramite.ToString("C");
            ddlVendedor.Visible = true;
            tbClaveVend.Visible = true;
            bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR WHERE TVEND_CODIGO = 'TT' AND PVEN_VIGENCIA = 'V'");
            if (ddlVendedor.SelectedValue == string.Empty || ddlVendedor.SelectedValue == "0")
            {
                Utils.MostrarAlerta(Response, "No se han Definido vendedores del tipo TT revice por favor.");
                return;
            }
            DataTable dtv = Session["datos"] as DataTable;
            DataSet dsc = new DataSet();
            DBFunctions.Request(dsc, IncludeSchema.NO, @"SELECT MAX(MVEH_INVENTARIO) AS INVENTARIO, TCLA_CODIGO AS TIPO_VEHICULO, PA.PCEN_CENTVVN, PA.PCEN_CENTVVU FROM MVEHICULO MV, MORDENTRAMITE MO, PALMACEN PA  
                                                         WHERE MV.MCAT_VIN = '" + dtv.Rows[0][5].ToString() + "' AND MV.MCAT_VIN = MO.MCAT_VIN AND PA.PALM_ALMACEN = '" + almacen.SelectedValue + "' GROUP BY TCLA_CODIGO, PA.PCEN_CENTVVN, PA.PCEN_CENTVVU;");

            //   EL CENTRO DE COSTO LO DEBE TRAER DE LA FACTURA ORIGINAL DE CLIENTE

            if (dsc.Tables[0].Rows.Count != 0)
            {
                if (dsc.Tables[0].Rows[0][1].ToString() == "N")
                {
                    centrocostos = dsc.Tables[0].Rows[0][2].ToString();
                    Session["CC"] = centrocostos;
                }
                else
                {
                    centrocostos = dsc.Tables[0].Rows[0][3].ToString();
                    Session["CC"] = centrocostos;
                }

            }
        }
        protected void Cargar_Datos_Tramite()
        {
            tablaDatosTramite = new DataTable();
            tablaDatosTramite.Columns.Add("CODIGOTRA", typeof(string));
            tablaDatosTramite.Columns.Add("NUMEROTRA", typeof(int));
            tablaDatosTramite.Columns.Add("DETALLETRA", typeof(string));
            tablaDatosTramite.Columns.Add("CARGOTRA", typeof(string));
            tablaDatosTramite.Columns.Add("CODIGOEST", typeof(string));
            tablaDatosTramite.Columns.Add("ESTADOTRA", typeof(string));
            tablaDatosTramite.Columns.Add("IVATRA", typeof(Int32));
            tablaDatosTramite.Columns.Add("VALORTRA", typeof(int));
            tablaDatosTramite.Columns.Add("TIPOTRA", typeof(string));
            tablaDatosTramite.Columns.Add("CODIGO", typeof(string));
        }
        protected void cancelar_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            Response.Redirect(pathToMain + "?process=Vehiculos.Facturaciontramite");
        }

        protected void guardar_Click(object sender, System.EventArgs e)
        {

            string cc = (String)Session["CC"];
            if (cc == null)
                cc = DBFunctions.SingleData("SELECT PCEN_CENTCART FROM PALMACEN WHERE PALM_ALMACEN = '" + almacen.SelectedValue + "'");
            ArrayList sqlRefs = new ArrayList();
            DataTable dtramite = Session["tablaDatosTramite"] as DataTable;
            DataTable dtDatosTr = Session["tablaDatos"] as DataTable;
            if ((DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + ddlVendedor.SelectedValue + "'") != tbClaveVend.Text.Trim()) || (ddlVendedor.SelectedValue == "0"))
            {
                Utils.MostrarAlerta(Response, "La clave de " + ddlVendedor.SelectedItem.Text + " es invalida.\\nRevise Por Favor!");
                return;
            }

            for (int i = 0; i < dtramite.Rows.Count; i++)
            {
                sqlRefs.Add("INSERT INTO DORDENTRAMITEANULACION VALUES('" + dtramite.Rows[i][0] + "'," + dtramite.Rows[i][1] + ",'" + dtramite.Rows[i][9].ToString() + "', 'C','D','',null, '', '' , null, ''," + dtramite.Rows[i][7].ToString() + ", null, '" + dtramite.Rows[i][6].ToString() + "')");
            }

            numero.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");

            sqlRefs.Add("INSERT INTO mfacturacliente VALUES('" + prefijoDocumento.SelectedValue + "'," + numero.Text.ToString() + ",'" + txtNit.Text + "','" + almacen.SelectedValue + "','N','V'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + Convert.ToDouble(txtvalorServ.Text.Substring(1)) + ", " + Convert.ToDouble(txtivaServ.Text.Substring(1)) + ", " + Convert.ToDouble(txtTotalDoc.Text.Substring(1)) + ", 0" +
                                ", 0, 0, 0, 0,'" + cc + "'," +
                                "'Devolucion Tramite','" + ddlVendedor.SelectedValue + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

            for(int i = 0; i < dtDatosTr.Rows.Count; i++)
            {
                sqlRefs.Add("INSERT INTO MANULACIONTRAMITE VALUES ('" + prefijoDocumento.SelectedValue + "', " + numero.Text.ToString() + " , '" + dtDatosTr.Rows[i][0] + "', " + dtDatosTr.Rows[i][1] + ")");
                sqlRefs.Add("DELETE FROM MFACTURACLIENTETRAMITE WHERE PDOC_TRAMITE = '" + dtDatosTr.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtDatosTr.Rows[i][1] + "");
                sqlRefs.Add("UPDATE DORDENTRAMITE SET TEST_ESTADO = 'A' WHERE PDOC_CODIGO = '" + dtDatosTr.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtDatosTr.Rows[i][1] + "");
                sqlRefs.Add("UPDATE MORDENTRAMITE SET TEST_ESTADO = 'A' WHERE PDOC_CODIGO = '" + dtDatosTr.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtDatosTr.Rows[i][1] + "");
            }
            sqlRefs.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU+1 WHERE PDOC_CODIGO = '" + prefijoDocumento.SelectedValue + "';");

            if (DBFunctions.Transaction(sqlRefs))
            {
                contaOnline.contabilizarOnline(prefijoDocumento.SelectedValue, Convert.ToInt32(numero.Text.ToString()), Convert.ToDateTime(txtFecha.Text), "");
                Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + prefijoDocumento.SelectedValue + " y número " + numero.Text.ToString() + "");
                formatoFactura = new FormatosDocumentos();
                try
                {
                    formatoFactura.Prefijo = prefijoDocumento.SelectedValue;
                    formatoFactura.Numero = Convert.ToInt32(numero.Text);
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue.ToString() + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + prefijoDocumento.SelectedValue + "','','HEIGHT=600,WIDTH=800');</script>");
                    }
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=500,WIDTH=700');</script>");
                    }
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Session.Clear();
                    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionTramite&prefD=" + ddlPrefijo.SelectedValue + "&numD=" + numero.Text + "");
                }
                catch
                {
                    lbInfo.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                }
            }
        }
    }
}
