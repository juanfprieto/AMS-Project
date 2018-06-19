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
    public partial class FacturacionTramite : System.Web.UI.UserControl
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
        string centrocostos = "";
        int sumaserv = 0, sumaDoc = 0, ivaserv = 0, totalivaserv = 0,  sumaiva = 0, totaltramite = 0, valoserv = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        }
        protected void cargarDocumentos_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           if (!DBFunctions.RecordExist("select P.PDOC_CODIGO, P.PDOC_DESCRIPCION FROM PDOCUMENTOHECHO PD, PDOCUMENTO P WHERE PALM_ALMACEN = '" + almacen.Text + "' AND TPRO_PROCESO = 'FT' AND TVIG_VIGENCIA = 'V' AND P.PDOC_CODIGO = PD.PDOC_CODIGO; "))
            {
                prefijoDocumento.Enabled = false;
                prefijoDocumento.Enabled = false;
                aceptar.Enabled = false;
                Utils.MostrarAlerta(Response, "No hay documentos del tipo FC asociados al proceso FT para la sede '" + almacen.Text + "'");
                return;
            }
            else
            {
                bind.PutDatasIntoDropDownList(prefijoDocumento,"SELECT P.PDOC_CODIGO, P.PDOC_DESCRIPCION FROM PDOCUMENTOHECHO PD, PDOCUMENTO P WHERE PALM_ALMACEN = '" + almacen.Text + "' AND TPRO_PROCESO = 'FT' AND TVIG_VIGENCIA = 'V' AND P.TDOC_TIPODOCU = 'FC' AND P.PDOC_CODIGO = PD.PDOC_CODIGO");
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
            //totalivaserv = 0;
            Session.Clear();

            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT MO.PDOC_CODIGO AS CODIGO, MORD_NUMEORDE AS NUMERO, DATE (MORD_CREACION) AS FECHA, TEST_ESTADO AS ESTADO, COALESCE ('CATALOGO:' || ' ' || PCAT_CODIGO || ' - VIN: ' || MO.MCAT_VIN || ' - PLACA: ' || MC.MCAT_PLACA || MO.MORD_OBSERECE, MO.MORD_OBSERECE) AS OBSERVACION, MO.MCAT_VIN AS VIN
                                                        FROM  MORDENTRAMITE MO LEFT JOIN MCATALOGOVEHICULO MC  ON ( MO.MCAT_VIN = MC.MCAT_VIN) WHERE MO.MNIT_NIT = '" + txtNit.Text+"' AND TEST_ESTADO IN ('A','C') ; ");
            Session["Datos"] = ds.Tables[0];
            if (ds.Tables[0].Rows.Count != 0)
                Cargar_Tabla_Tramites(ds.Tables[0]);
            else { Utils.MostrarAlerta(Response, "No existen Ordenes de tramite en Proceso para este Nit"); return; };
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
            tablaDatosTramite = new DataTable();
            DataTable dt = Session["datos"] as DataTable;
            DataSet dsT = new DataSet();
            DataRow filaTr;
            int i = 0;
            if (txtvalorServ.Text == "") txtvalorServ.Text = "0";
            else
            {
                String sumas = Convert.ToString(txtvalorServ.Text).Substring(1).Replace(",","").Trim();
                sumas = sumas.Replace(".00", "");
                sumaserv = Convert.ToInt32(sumas);
            }
            if (txtivaServ.Text == "") txtivaServ.Text = "0";
            else
            {
                String sumas = Convert.ToString(txtivaServ.Text).Substring(1).Replace(",", "").Trim();
                sumas = sumas.Replace(".00", "");
                totalivaserv = Convert.ToInt32(sumas);
            }
            if (txtValorDoc.Text == "") txtivaServ.Text = "0";
            else
            {
                String sumas = Convert.ToString(txtValorDoc.Text).Substring(1).Replace(",", "").Trim();
                sumas = sumas.Replace(".00", "");
                sumaDoc = Convert.ToInt32(sumas);
            };

            DBFunctions.Request(dsT, IncludeSchema.NO, @"SELECT d.PDOC_CODIGO AS CODIGOTRA, d.MORD_NUMEORDE AS NUMEROTRA, pI.PITE_NOMBRE  CONCAT ' - ' CONCAT ' ' CONCAT D.DORD_DOCUREFE AS DETALLETRA, tc.TCAR_NOMBRE AS CARGOTRA,d.TEST_ESTADO AS CODIGOEST, te.TEST_NOMBRE AS ESTADOTRA, D.PIVA_PORCIVA AS IVATRA, D.DORD_VALOTRAM AS VALORTRA, PI.TITE_CODIGO AS TIPOTRA
                                                        FROM DORDENTRAMITE D, TCARGOORDEN TC, TESTADOTRAMITE TE,
                                                        PITEMVENTAVEHICULO PI
                                                        WHERE TC.TCAR_CARGO = D.tcar_cargo
                                                        --AND P.PTRA_CODIGO = PI.PTRA_CODIGO
                                                        and TE.TEST_ESTADO = D.TEST_ESTADO
                                                        AND   PI.PITE_CODIGO = D.PTRA_CODIGO
                                                        AND D.PDOC_CODIGO = '" + dt.Rows[e.Item.DataSetIndex][0].ToString() + "'  AND D.MORD_NUMEORDE = " + dt.Rows[e.Item.DataSetIndex][1].ToString() + ";");
                Session["DatosTramite"] = dsT.Tables[0];
                if (dsT.Tables[0].Rows.Count != 0)
                {
                    if (Session["tabladatostramite"] == null)
                    {
                        Cargar_Datos_Tramite(dsT.Tables[0]);
                        this.Binding_Grilla();
                    }
                    else
                    {
                        tablaDatosTramite = (DataTable)Session["tabladatostramite"];
                        Session["tabladatostramite"] = tablaDatosTramite;
                    }

                }
                ivaserv = 0;
                for (i = 0; i < dsT.Tables[0].Rows.Count; i++)
                {
                    filaTr = tablaDatosTramite.NewRow();

                    filaTr["CODIGOTRA"] = dsT.Tables[0].Rows[i][0].ToString();
                    filaTr["NUMEROTRA"] = dsT.Tables[0].Rows[i][1].ToString();
                    filaTr["DETALLETRA"] = dsT.Tables[0].Rows[i][2].ToString();
                    filaTr["CARGOTRA"] = dsT.Tables[0].Rows[i][3].ToString();
                    filaTr["CODIGOEST"] = dsT.Tables[0].Rows[i][4].ToString();
                    filaTr["ESTADOTRA"] = dsT.Tables[0].Rows[i][5].ToString();
                    if (dsT.Tables[0].Rows[i][6].ToString() != "0.00")
                    {
                        filaTr["IVATRA"] = dsT.Tables[0].Rows[i][6];
                    }
                    else filaTr["IVATRA"] = 0;
                    filaTr["VALORTRA"] = dsT.Tables[0].Rows[i][7].ToString();
                    filaTr["TIPOTRA"] = dsT.Tables[0].Rows[i][8].ToString();
                    tablaDatosTramite.Rows.Add(filaTr);
                    if (dsT.Tables[0].Rows[i][6].ToString() != "0.00")
                    {
                        sumaserv += Convert.ToInt32(filaTr["VALORTRA"]);
                        valoserv = Convert.ToInt32(filaTr["VALORTRA"]);
                        ivaserv = valoserv * Convert.ToInt32(filaTr["IVATRA"]) / 100;
                        totalivaserv += ivaserv;
                    }
                    else
                    {
                        sumaDoc += Convert.ToInt32(filaTr["VALORTRA"]);
                        txtValorDoc.Text = Convert.ToString(sumaDoc);

                    }
                    //ivaserv = int.Parse(txtvalorServ.Text) * Convert.ToInt32(filaTr["IVATRA"])/100;              

                    Session["tablaDatos"] = tablaDatosTramite;
                    gridDatosTramite.DataSource = tablaDatosTramite;
                    gridDatosTramite.DataBind();
                }

                txtivaServ.Text = totalivaserv.ToString("C");
                sumaiva = sumaserv + totalivaserv;
                txtTotal.Text = sumaiva.ToString("C");
                txtvalorServ.Text = sumaserv.ToString("C");
                txtValorDoc.Text = sumaDoc.ToString("C");
                txtIvaDoc.Text = "$0.00";
                txtTotalDoc.Text = sumaDoc.ToString("C");
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
        
        protected void Cargar_Datos_Tramite(DataTable dtable)
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
        }
        protected void Binding_Grilla()
        {
            Session["tabladatostramite"] = tablaDatosTramite;
            gridDatosTramite.DataSource = tablaDatosTramite;
            gridDatosTramite.DataBind();
        }
        protected void cancelar_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            totalivaserv = 0;
            Response.Redirect(pathToMain + "?process=Vehiculos.Facturaciontramite");
        }

        protected void guardar_Click(object sender, System.EventArgs e)
        {
            string cc = (String)Session["CC"];
            int i;          
            if(cc == null)
              cc =  DBFunctions.SingleData("SELECT PCEN_CENTCART FROM PALMACEN WHERE PALM_ALMACEN = '" + almacen.SelectedValue + "'");
            ArrayList sqlRefs = new ArrayList();
            DataTable dtramite = Session["tablaDatos"] as DataTable;
            //Ahora revisamos si la clave del vendedor es valida o no
            if ((DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + ddlVendedor.SelectedValue + "'") != tbClaveVend.Text.Trim()) || (ddlVendedor.SelectedValue == "0"))
            {
                Utils.MostrarAlerta(Response, "La clave de " + ddlVendedor.SelectedItem.Text + " es invalida.\\nRevise Por Favor!");
                return;
            }
            double valorServ = 0, valorDocu = 0;
            if (txtvalorServ.Text.Substring(1) == "(")
                valorServ = Convert.ToDouble(txtvalorServ.Text.Substring(2))*-1;
            else valorServ = Convert.ToDouble(txtvalorServ.Text.Substring(1));
            if (txtTotalDoc.Text.Substring(0).Contains("("))
            {
                txtTotalDoc.Text = txtTotalDoc.Text.Replace(")", "");
                valorDocu = Convert.ToDouble(txtTotalDoc.Text.Substring(2)) * -1;
            }
            else valorDocu = Convert.ToDouble(txtTotalDoc.Text.Substring(1));
            
            sqlRefs.Add("INSERT INTO mfacturacliente VALUES('" + prefijoDocumento.SelectedValue + "'," + numero.Text.ToString() + ",'" + txtNit.Text + "','" + almacen.SelectedValue + "','F','V'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + valorServ + ", " + Convert.ToDouble(txtivaServ.Text.Substring(1)) + ", " + valorDocu + ", 0" +
                                ", 0, 0, 0, 0,'" + cc + "'," +
                                "'"+ detalleTransaccion.Text + "','" + ddlVendedor.SelectedValue + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

   
            DataTable dsT = Session["datos"] as DataTable;

            for (i = 0; i < dtramite.Rows.Count; i++)
            {
                if (i == 0)
                {
                    sqlRefs.Add("INSERT INTO MFACTURACLIENTETRAMITE VALUES ('" + prefijoDocumento.SelectedValue + "', " + numero.Text.ToString() + " , '" + dtramite.Rows[i][0] + "', " + dtramite.Rows[i][1] + ")");
                    sqlRefs.Add("UPDATE DORDENTRAMITE SET TEST_ESTADO = 'F' WHERE PDOC_CODIGO = '" + dtramite.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtramite.Rows[i][1] + "");
                    sqlRefs.Add("UPDATE MORDENTRAMITE SET TEST_ESTADO = 'T' WHERE PDOC_CODIGO = '" + dtramite.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtramite.Rows[i][1] + "");

                }
                else if ((dtramite.Rows[i][0].ToString().CompareTo(dtramite.Rows[i - 1][0].ToString())) == (dtramite.Rows[i][1].ToString().CompareTo(dtramite.Rows[i - 1][1].ToString())))
                {
                }
                else
                { 
                    sqlRefs.Add("INSERT INTO MFACTURACLIENTETRAMITE VALUES ('" + prefijoDocumento.SelectedValue + "', " + numero.Text.ToString() + " , '" + dtramite.Rows[i][0] + "', " + dtramite.Rows[i][1] + ")");
                    sqlRefs.Add("UPDATE DORDENTRAMITE SET TEST_ESTADO = 'F' WHERE PDOC_CODIGO = '" + dtramite.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtramite.Rows[i][1] + "");
                    sqlRefs.Add("UPDATE MORDENTRAMITE SET TEST_ESTADO = 'T' WHERE PDOC_CODIGO = '" + dtramite.Rows[i][0] + "' AND MORD_NUMEORDE = " + dtramite.Rows[i][1] + "");
                }
            }
            sqlRefs.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU+1 WHERE PDOC_CODIGO = '"+prefijoDocumento.SelectedValue+"';");            
            if (DBFunctions.Transaction(sqlRefs))
            {                
                contaOnline.contabilizarOnline(prefijoDocumento.SelectedValue.ToString(), Convert.ToInt32(numero.Text.ToString()), Convert.ToDateTime(txtFecha.Text), almacen.SelectedValue);
                Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + prefijoDocumento.SelectedValue.ToString() + " y número " + numero.Text.ToString() + "");
                formatoFactura = new FormatosDocumentos();
                try
                {
                    formatoFactura.Prefijo = ddlPrefijo.SelectedValue.ToString();
                    formatoFactura.Numero = Convert.ToInt32(numero.Text);
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                    }
                    formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue+ "'");
                    if (formatoFactura.Codigo != string.Empty)
                    {
                        if (formatoFactura.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=500,WIDTH=700');</script>");
                    }
                        string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                        Session.Clear();
                        Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionTramite&prefD=" + prefijoDocumento.SelectedValue + "&numD=" + numero.Text + "");
                }
                catch
                {
                    lbInfo.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                }
            }
        }
    }
}
