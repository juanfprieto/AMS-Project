using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Ajax;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Contabilidad;
using AMS.Tools;

namespace AMS.Finanzas
{
    /// <summary>
    ///		Descripción breve de AMS_Cartera_DevolucionFacAdmin.
    /// </summary>
    public partial class AMS_Cartera_DevolucionFacAdmin : System.Web.UI.UserControl
    {
        #region Atributos

        private string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private string tipo = "", prefFac = "", numFac = "", tipoFac = "";
        private DataTable dtActivos, dtDiferidos, dtOperativos, tablaNC;
        private double valorNotaDev = 0, valorIvaDev = 0, valorRetDev = 0;
        protected System.Web.UI.WebControls.Label Label1;
        private FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        protected DataTable tablaRtns, dtIva;
        private static string checkeado = "";
        ProceHecEco contaOnline = new ProceHecEco();

        #endregion
        protected DataTable dtInserts;

        protected void Cargar_Tabla_Rtns()
        {
            tablaRtns = new DataTable();
            tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("TIPORETE", typeof(string)));
        }

        protected void Cargar_dtIva()
        {
            dtIva = new DataTable();
            dtIva.Columns.Add("PORCENTAJE", typeof(double));
            dtIva.Columns.Add("VALORBASE", typeof(double));
            dtIva.Columns.Add("VALOR", typeof(double));
            dtIva.Columns.Add("CUENTA", typeof(string));
            dtIva.Columns.Add("NIT", typeof(string));
            dtIva.Columns.Add("TIPO", typeof(string));
        }
        protected void ChangeDate(Object Sender, EventArgs E)
        {
            tbDate.Text = calDate.SelectedDate.GetDateTimeFormats()[6];
        }
        protected void Mostrar_gridRtns()
        {
            gridRtns.DataSource = tablaRtns;
            gridRtns.DataBind();
            Session["tablaRtns"] = tablaRtns;
        }

        protected void Mostrar_dgIva()
        {
            Session["dtIva"] = dtIva;
            dgIva.DataSource = dtIva;
            dgIva.DataBind();
        }

        protected bool Buscar_Retencion(string rtn)
        {
            bool encontrado = false;
            if (tablaRtns != null && tablaRtns.Rows.Count != 0)
            {
                for (int i = 0; i < tablaRtns.Rows.Count; i++)
                {
                    if (tablaRtns.Rows[i][0].ToString() == rtn)
                        encontrado = true;
                }
            }
            return encontrado;
        }

        protected void SumarRetenciones()
        {
            double totalRet = 0;
            if (tablaRtns.Rows.Count > 0)
            {
                for (int i = 0; i < tablaRtns.Rows.Count; i++)
                    totalRet = totalRet + Convert.ToDouble(tablaRtns.Rows[i]["VALOR"]);
            }
            tbValRet.Text = totalRet.ToString("N");

            double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            tbTotal.Text = Valor.ToString("C");
            //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
        }

        protected bool ValidarBasesRetencion(double baseN)
        {
            double baseT = 0;
            double totalF = Convert.ToDouble(lbTotFac.Text.Substring(1));
            baseT += baseN;
            for (int n = 0; n < tablaRtns.Rows.Count; n++)
                baseT += Convert.ToDouble(tablaRtns.Rows[n]["VALOR"]);
            return (baseT <= totalF);
        }

        protected double Calcular_Retenciones()
        {
            double valor = 0;
            if (tablaRtns.Rows.Count != 0)
            {
                for (int i = 0; i < tablaRtns.Rows.Count; i++)
                    valor = valor + Convert.ToDouble(tablaRtns.Rows[i][3]);
            }
            return valor;
        }
        #region Eventos

        protected void Page_Load(object sender, System.EventArgs e)
        {
            tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Cartera_DevolucionFacAdmin));
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            prefFac = Request.Form[ddlPrefijo.UniqueID];
            numFac = Request.Form[txtNumero.UniqueID];
            //numFac = txtNumero.Text;
            if (prefFac == null && numFac == null)
            {
                prefFac = ddlPrefijo.SelectedValue;
                numFac = txtNumero.Text;
            }
            if (!IsPostBack)
            {
                Session.Clear();
                if (Request["devuelta"] != null)
                {
                    Utils.MostrarAlerta(Response, "el documento " + Request["prefnumFac"] + " ya esta devuelto, por favor revise !!!");
                }
                DatasToControls bind = new DatasToControls();
                if (Request["eliminado"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha eliminado correctamente la factura!");
                }
                if (Request["prefN"] != null && Request["numN"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha creado la nota devolución con prefijo " + Request["prefN"] + " y número " + Request["numN"] + "");
                    try
                    {
                        formatoRecibo.Prefijo = Request["prefN"];
                        formatoRecibo.Numero = Convert.ToInt32(Request["numN"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request["prefN"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                            //Utils.MostrarAlerta(Response, "" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600");
                        }
                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                    }
                }
                if (rbFC.Checked)
                {
                    //bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='FC' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref ddlPrefijo, "%", "%", "FC");
                    //bind.PutDatasIntoDropDownList(ddlNumero,  "SELECT mfac_numedocu FROM mfacturacliente WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' ORDER BY mfac_numedocu");
                    //bind.PutDatasIntoDropDownList(ddlPrefNota,"SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NC' and PH.tpro_proceso in ('AC') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                    Utils.llenarPrefijos(Response, ref  ddlPrefNota, "AC", "%", "NC");
                    if (ddlPrefNota.Items.Count == 0)
                    {
                        Utils.MostrarAlerta(Response, "NO ha creado y-o asignado un documento para la Nota Devolución de la Factura Administrativa, Creelo, Asocielo y luego ejecute el proceso");
                        return;  //  ver como se aborta aqui el proceso...
                    }
                }
                else
                {
                    //bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='FP' and tvig_vigencia = 'V' ");
                    Utils.llenarPrefijos(Response, ref ddlPrefijo, "%", "%", "FP");
                    //bind.PutDatasIntoDropDownList(ddlNumero,  "SELECT mfac_numeordepago FROM mfacturaproveedor WHERE pdoc_codiordepago='"+ddlPrefijo.SelectedValue+"' ORDER BY mfac_numeordepago");
                    //bind.PutDatasIntoDropDownList(ddlPrefNota,"SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NP' and PH.tpro_proceso in ('AP') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                    Utils.llenarPrefijos(Response, ref ddlPrefNota, "AP", "%", "NP");
                    if (ddlPrefNota.Items.Count == 0)
                    {
                        Utils.MostrarAlerta(Response, "NO ha creado y-o asignado un documento para la Nota Devolución de la Factura Administrativa, Creelo, Asocielo y luego ejecute el proceso");
                        return;  //  ver como se aborta aqui el proceso...
                    }

                }
                tbNotas.Visible = false;
                this.Cargar_tablaNC();
                this.Mostrar_gridNC();
                this.Cargar_Tabla_Rtns();
                this.Mostrar_gridRtns();
                this.Cargar_dtIva();
                this.Mostrar_dgIva();
            }
            else
            {
                EstablecerEstadosControles(prefFac, numFac);
                if (Session["dtActivos"] != null)
                    dtActivos = (DataTable)Session["dtActivos"];
                if (Session["dtDiferidos"] != null)
                    dtDiferidos = (DataTable)Session["dtDiferidos"];
                if (Session["dtOperativos"] != null)
                    dtOperativos = (DataTable)Session["dtOperativos"];
                if (Session["tablaNC"] != null)
                    tablaNC = (DataTable)Session["tablaNC"];
                if (Session["tablaRtns"] != null)
                    tablaRtns = (DataTable)Session["tablaRtns"];
                if (Session["dtIva"] != null)
                    dtIva = (DataTable)Session["dtIva"];
            }
        }

        protected void btnCargar_Click(object sender, System.EventArgs e)
        {
            string sql = null;
            if (rbFC.Checked)
                sql = "SELECT mfac_numedocu AS NUMERO FROM mfacturacliente WHERE pdoc_codigo='" + prefFac + "' and mfac_numedocu=" + numFac + " ORDER BY mfac_numedocu";
            else if (rbFP.Checked)
                sql = "SELECT mfac_numeordepago AS NUMERO FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefFac + "' and mfac_numeordepago=" + numFac + " ORDER BY mfac_numeordepago";

            if (!DBFunctions.RecordExist(sql))
            {
                Utils.MostrarAlerta(Response, "el documento " + prefFac + " - " + numFac + " no existe");
                return;
            }
            string sql2 = null;
            if (rbFC.Checked)
                sql2 = "SELECT mnot_prefdocu as numero FROM mnotacliente where mnot_prefdocu = '" + prefFac + "' and mnot_numdocu=" + numFac + " ORDER BY mnot_numdocu";
            else if (rbFP.Checked)
                sql2 = "SELECT mnot_numedocref as numero FROM mnotaproveedor where PDOC_PREFDOCREF = '" + prefFac + "' and MNOT_NUMEDOCREF=" + numFac + " ORDER BY MNOT_NUMEDOCREF";
            if (DBFunctions.RecordExist(sql2))
            {
                //Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&devuelta=1&prefnumFac=" + prefFac + "-" + numFac);
                Utils.MostrarAlerta(Response, "el documento " + prefFac + " - " + numFac + " ya esta devuelto, por favor revise. Es su responsabilidad continuar con este proceso !!!");
            }
            string clase = "";
            DataSet ds = new DataSet();
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (dtIva.Rows.Count > 0)
                dtIva.Rows.Clear();
            if (tablaRtns.Rows.Count > 0)
                tablaRtns.Rows.Clear();
            Mostrar_gridRtns();
            Mostrar_dgIva();
            SumarRetenciones();
            CalcularIva();
            if (tipo == "C")
            {
                if (EsAdministrativa(prefFac, numFac, ref clase))
                {
                    if (clase == "A")
                    {
                        CargarActivosFijos(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, dtActivos, dgActivos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    else if (clase == "D")
                    {
                        CargarDiferidos(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, dtDiferidos, dgDiferidos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    else if (clase == "O")
                    {
                        CargarOperativos(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, dtOperativos, dgOperativos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    else if (clase == "V")
                    {
                        CargarVarios(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    if (clase != "N")
                    {
                        btnActualizar.Enabled = true;
                        btnAceptar.Enabled = true;
                    }

                    rbFC.Enabled = rbFP.Enabled = ddlPrefijo.Enabled = txtNumero.Enabled = btnCargar.Enabled = false;
                    tbNotas.Visible = true;
                    CargarPrefijosNotas(prefFac);
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact,mfac_valoiva,mfac_valorete,mfac_valoabon,mfac_valoflet,mfac_valoivaflet,mnit_nit FROM mfacturacliente WHERE pdoc_codigo='" + prefFac + "' AND mfac_numedocu=" + numFac + "");
                    lbValFac.Text = Convert.ToDouble(ds.Tables[0].Rows[0][0]).ToString("C");
                    lbIva.Text = Convert.ToDouble(ds.Tables[0].Rows[0][1]).ToString("C");
                    lbRet.Text = Convert.ToDouble(ds.Tables[0].Rows[0][2]).ToString("C");
                    lbValFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][4]).ToString("C");
                    lbIvaFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][5]).ToString("C");
                    lbTotFac.Text = (Convert.ToDouble(ds.Tables[0].Rows[0][0]) + Convert.ToDouble(ds.Tables[0].Rows[0][1]) + Convert.ToDouble(ds.Tables[0].Rows[0][4]) + Convert.ToDouble(ds.Tables[0].Rows[0][5]) - Convert.ToDouble(ds.Tables[0].Rows[0][2])).ToString("C");
                    lbAbon.Text = Convert.ToDouble(ds.Tables[0].Rows[0][3]).ToString("C");
                    ViewState["NIT"] = ds.Tables[0].Rows[0][6].ToString();
                    lbSaldo.Text = (Convert.ToDouble(lbTotFac.Text.Substring(1)) - Convert.ToDouble(lbAbon.Text.Substring(1))).ToString("C");
                    lbCuenta.Text = "Cuenta por Cobrar : ";
                    tbCuenta.Text = DBFunctions.SingleData("SELECT mcue_codipuc FROM mfacturaadministrativacliente WHERE pdoc_codigo='" + prefFac + "' AND mfac_numedocu=" + numFac + "");
                    lbCuenta.Visible = tbCuenta.Visible = true;
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");
                    CargarIVADevolver("C");
                    CargarRetencionDevolver("C");
                    tbTotal.Text = (Convert.ToDouble(tbValor.Text) + Convert.ToDouble(tbFletes.Text) + Convert.ToDouble(tbIvaFletes.Text) + Convert.ToDouble(tbvalIva.Text) - Convert.ToDouble(tbValRet.Text)).ToString("C");
                }
                else
                {
                    rbFC.Enabled = rbFP.Enabled = ddlPrefijo.Enabled = txtNumero.Enabled = btnCargar.Enabled = false;
                    tbNotas.Visible = true;
                    CargarPrefijosNotas(prefFac);
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact,mfac_valoiva,mfac_valorete,mfac_valoabon,mfac_valoflet,mfac_valoivaflet,mnit_nit FROM mfacturacliente WHERE pdoc_codigo='" + prefFac + "' AND mfac_numedocu=" + numFac + "");
                    lbValFac.Text = Convert.ToDouble(ds.Tables[0].Rows[0][0]).ToString("C");
                    lbIva.Text = Convert.ToDouble(ds.Tables[0].Rows[0][1]).ToString("C");
                    lbRet.Text = Convert.ToDouble(ds.Tables[0].Rows[0][2]).ToString("C");
                    lbValFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][4]).ToString("C");
                    lbIvaFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][5]).ToString("C");
                    lbTotFac.Text = (Convert.ToDouble(ds.Tables[0].Rows[0][0]) + Convert.ToDouble(ds.Tables[0].Rows[0][1]) + Convert.ToDouble(ds.Tables[0].Rows[0][4]) + Convert.ToDouble(ds.Tables[0].Rows[0][5]) - Convert.ToDouble(ds.Tables[0].Rows[0][2])).ToString("C");
                    lbAbon.Text = Convert.ToDouble(ds.Tables[0].Rows[0][3]).ToString("C");
                    ViewState["NIT"] = ds.Tables[0].Rows[0][6].ToString();
                    lbSaldo.Text = (Convert.ToDouble(lbTotFac.Text.Substring(1)) - Convert.ToDouble(lbAbon.Text.Substring(1))).ToString("C");
                    lbCuenta.Text = "Cuenta por Pagar : ";
                    tbCuenta.Text = DBFunctions.SingleData("SELECT mcue_codipuc FROM ppucdocumento WHERE pdoc_codigo='" + prefFac + "' AND tcon_codigo = 'CXC' ");
                    lbCuenta.Visible = tbCuenta.Visible = true;
                    btnActualizar.Enabled = false;
                    gridNC.Visible = true;
                }
            }
            else if (tipo == "P")
            {
                if (EsAdministrativa(prefFac, numFac, ref clase))
                {
                    if (clase == "A")
                    {
                        CargarActivosFijos(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, dtActivos, dgActivos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    else if (clase == "D")
                    {
                        CargarDiferidos(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, dtDiferidos, dgDiferidos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    else if (clase == "O")
                    {
                        CargarOperativos(prefFac, numFac);
                        CalcularTotales(prefFac, numFac, dtOperativos, dgOperativos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    }
                    else if (clase == "V")
                    {
                        CargarVarios(prefFac, numFac);
                    }
                    if (clase != "N")
                    {
                        btnActualizar.Enabled = true;
                        btnAceptar.Enabled = true;
                    }

                    rbFC.Enabled = rbFP.Enabled = ddlPrefijo.Enabled = txtNumero.Enabled = btnCargar.Enabled = false;
                    tbNotas.Visible = true;
                    CargarPrefijosNotas(prefFac);
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact,mfac_valoiva,mfac_valorete,mfac_valoabon,mfac_valoflet,mfac_valoivaflet, mnit_nit FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefFac + "' AND mfac_numeordepago=" + numFac + "");
                    lbValFac.Text = Convert.ToDouble(ds.Tables[0].Rows[0][0]).ToString("C");
                    lbIva.Text = Convert.ToDouble(ds.Tables[0].Rows[0][1]).ToString("C");
                    lbRet.Text = Convert.ToDouble(ds.Tables[0].Rows[0][2]).ToString("C");
                    lbTotFac.Text = (Convert.ToDouble(ds.Tables[0].Rows[0][0]) + Convert.ToDouble(ds.Tables[0].Rows[0][1]) + Convert.ToDouble(ds.Tables[0].Rows[0][4]) + Convert.ToDouble(ds.Tables[0].Rows[0][5]) - Convert.ToDouble(ds.Tables[0].Rows[0][2])).ToString("C");
                    lbAbon.Text = Convert.ToDouble(ds.Tables[0].Rows[0][3]).ToString("C");
                    lbSaldo.Text = (Convert.ToDouble(lbTotFac.Text.Substring(1)) - Convert.ToDouble(lbAbon.Text.Substring(1))).ToString("C");
                    lbValFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][4]).ToString("C");
                    lbIvaFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][5]).ToString("C");
                    ViewState["NIT"] = ds.Tables[0].Rows[0][6].ToString();
                    lbCuenta.Text = "Cuenta por Pagar : ";
                    tbCuenta.Text = DBFunctions.SingleData("SELECT mcue_codipuc FROM mfacturaadministrativaproveedor WHERE pdoc_codiordepago='" + prefFac + "' AND mfac_numeordepago=" + numFac + "");
                    lbCuenta.Visible = tbCuenta.Visible = true;
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");
                    CargarIVADevolver("P");
                    CargarRetencionDevolver("P");

                    tbTotal.Text = (Convert.ToDouble(tbValor.Text) + Convert.ToDouble(tbFletes.Text) + Convert.ToDouble(tbIvaFletes.Text) + Convert.ToDouble(tbvalIva.Text) - Convert.ToDouble(tbValRet.Text)).ToString("C");
                }
                else
                {
                    rbFC.Enabled = rbFP.Enabled = ddlPrefijo.Enabled = txtNumero.Enabled = btnCargar.Enabled = false;
                    tbNotas.Visible = true;
                    CargarPrefijosNotas(prefFac);
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact,mfac_valoiva,mfac_valorete,mfac_valoabon,mfac_valoflet,mfac_valoivaflet,mnit_nit FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefFac + "' AND mfac_numeordepago=" + numFac + "");
                    lbValFac.Text = Convert.ToDouble(ds.Tables[0].Rows[0][0]).ToString("C");
                    lbIva.Text = Convert.ToDouble(ds.Tables[0].Rows[0][1]).ToString("C");
                    lbRet.Text = Convert.ToDouble(ds.Tables[0].Rows[0][2]).ToString("C");
                    lbTotFac.Text = (Convert.ToDouble(ds.Tables[0].Rows[0][0]) + Convert.ToDouble(ds.Tables[0].Rows[0][1]) + Convert.ToDouble(ds.Tables[0].Rows[0][4]) + Convert.ToDouble(ds.Tables[0].Rows[0][5]) - Convert.ToDouble(ds.Tables[0].Rows[0][2])).ToString("C");
                    lbAbon.Text = Convert.ToDouble(ds.Tables[0].Rows[0][3]).ToString("C");
                    lbValFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][4]).ToString("C");
                    lbIvaFletes.Text = Convert.ToDouble(ds.Tables[0].Rows[0][5]).ToString("C");
                    ViewState["NIT"] = ds.Tables[0].Rows[0][6].ToString();
                    lbSaldo.Text = (Convert.ToDouble(lbTotFac.Text.Substring(1)) - Convert.ToDouble(lbAbon.Text.Substring(1))).ToString("C");
                    lbCuenta.Text = "Cuenta por Pagar : ";
                    tbCuenta.Text = DBFunctions.SingleData("SELECT mcue_codipuc FROM ppucdocumento WHERE pdoc_codigo='" + prefFac + "' AND tcon_codigo = 'CXC' ");
                    lbCuenta.Visible = tbCuenta.Visible = true;
                    btnActualizar.Enabled = true; // tenia false;
                    gridNC.Visible = true;
                }
            }
            //  Precarga también los valores de la nota de acuerdo al valor original de la factura
            tbValor.Text = lbValFac.Text;
            tbFletes.Text = lbValFletes.Text;
            tbIvaFletes.Text = lbIvaFletes.Text;
            //tbvalIva.Text = lbIva.Text;
            //tbValRet.Text = lbRet.Text; 
            tbTotal.Text = lbTotFac.Text;

            Mostrar_gridRtns();
            Mostrar_dgIva();
        }

        protected void btnCancelar_Click(object sender, System.EventArgs e)
        {
            Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin");
        }

        private void gridNC_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string verBase;
            double vb1 = 0, vporcBase, res;
            DataRow fila;
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (((Button)e.CommandSource).CommandName == "AgregarFilas")
            {
                if (!Validar_Datos(e))
                {
                    if (Session["tablaNC"] == null)
                        this.Cargar_tablaNC();
                    fila = tablaNC.NewRow();
                    fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                    fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    fila[2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
                    fila[3] = ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue;
                    fila[4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                    fila[5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
                    fila[6] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
                    //Si el valor introducido es debito
                    if ((((TextBox)e.Item.Cells[7].Controls[1]).Text != "0") && (((TextBox)e.Item.Cells[8].Controls[1]).Text == "0"))
                    {
                        fila[7] = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
                        fila[8] = Convert.ToDouble("0");
                    }
                    //Si el valor introducido es credito
                    else if ((((TextBox)e.Item.Cells[7].Controls[1]).Text == "0") && (((TextBox)e.Item.Cells[8].Controls[1]).Text != "0"))
                    {
                        fila[7] = System.Convert.ToDouble("0");
                        fila[8] = System.Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
                    }
                    //Ahora verificamos los valores del valor base
                    verBase = DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString() + "' and timp_codigo in ('A','P')");
                    //Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
                    if (verBase == "N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text != "0"))
                    {
                        Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
                        fila[9] = System.Convert.ToDouble("0");
                    }
                    //Si la cuenta no soporta valor base y hay un valor de cero en ese campo
                    else if (verBase == "N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text == "0"))
                            fila[9] = System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                    //Si la cuenta afectada soporta valor base
                    else if (verBase == "B")
                    {
                        //Convierto a double el valor base
                        vb1 = System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                        //Miro en la base de datos cual es el porcentaje de valor base
                        //lo convierto a double y lo divido por 100 para saber el verdadero valor
                        vporcBase = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString() + "'"));
                        //Si el valor introducido es debito entonces se calcula el valor base con base en este
                        if (((TextBox)e.Item.Cells[7].Controls[1]).Text != "0")
                        {
                            res = System.Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) * 100 / vporcBase;
                            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                                fila[9] = vb1;
                            else
                            {
                                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                                fila[9] = res;
                            }
                        }
                        //Si el valor introducido es credito entonces se calcula el valor base con base en este
                        else if (((TextBox)e.Item.Cells[8].Controls[1]).Text != "0")
                        {
                            res = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text) * 100 / vporcBase;
                            if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                                fila[9] = vb1;
                            else
                            {
                                Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                                fila[9] = res;
                            }
                        }
                    }
                    tablaNC.Rows.Add(fila);
                    gridNC.DataSource = tablaNC;
                    gridNC.DataBind();
                    Session["tablaNC"] = tablaNC;
                    if (tipo == "C")
                    {
                        CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                        tbValor.Text = valorNotaDev.ToString("N");
                        //tbvalIva.Text=valorIvaDev.ToString("N");
                        //tbValRet.Text=valorRetDev.ToString("N");

                        double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                        Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                        Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                        Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                        Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));
                        tbTotal.Text = Valor.ToString("C");

                        //tbTotal.Text = Valor.ToString("C");
                        //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                        if (DiferenciaCreditosDebitos())
                            btnAceptar.Enabled = true;
                        else
                            btnAceptar.Enabled = false;
                    }
                    else if (tipo == "P")
                    {
                        CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                        tbValor.Text = valorNotaDev.ToString("N");
                        //tbvalIva.Text=valorIvaDev.ToString("N");
                        //tbValRet.Text=valorRetDev.ToString("N");

                        double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                        Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                        Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                        Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                        Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                        tbTotal.Text = Valor.ToString("C");
                        //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                        if (DiferenciaCreditosDebitos())
                            btnAceptar.Enabled = true;
                        else
                            btnAceptar.Enabled = false;
                    }
                }
            }
            else if (((Button)e.CommandSource).CommandName == "RemoverFilas")
            {
                //Lo unico que hacemos es restar el valor al total y borramos la fila
                tablaNC.Rows[e.Item.DataSetIndex].Delete();
                Mostrar_gridNC();
                tablaNC.AcceptChanges();
                if (tipo == "C")
                {
                    CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text = valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");
                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                    if (DiferenciaCreditosDebitos())
                        btnAceptar.Enabled = true;
                    else
                        btnAceptar.Enabled = false;
                }
                else if (tipo == "P")
                {
                    CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text = valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");
                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                    if (DiferenciaCreditosDebitos())
                        btnAceptar.Enabled = true;
                    else
                        btnAceptar.Enabled = false;
                }
            }
        }

        protected void gridNC_Update(object Sender, DataGridCommandEventArgs e)
        {
            string verBase;
            double vb1 = 0, vporcBase, res;
            DataRow fila;
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (!Validar_Datos(e))
            {
                if (Session["tablaNC"] == null)
                    this.Cargar_tablaNC();
                fila = tablaNC.NewRow();
                fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                fila[2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
                fila[3] = ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue;
                fila[4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                fila[5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
                fila[6] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
                //Si el valor introducido es debito
                if ((((TextBox)e.Item.Cells[7].Controls[1]).Text != "0") && (((TextBox)e.Item.Cells[8].Controls[1]).Text == "0"))
                {
                    fila[7] = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
                    fila[8] = Convert.ToDouble("0");
                }
                //Si el valor introducido es credito
                else if ((((TextBox)e.Item.Cells[7].Controls[1]).Text == "0") && (((TextBox)e.Item.Cells[8].Controls[1]).Text != "0"))
                {
                    fila[7] = System.Convert.ToDouble("0");
                    fila[8] = System.Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
                }
                //Ahora verificamos los valores del valor base
                verBase = DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString() + "' and timp_codigo in ('A','P')");
                //Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
                if (verBase == "N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text != "0"))
                {
                    Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
                    fila[9] = System.Convert.ToDouble("0");
                }
                //Si la cuenta no soporta valor base y hay un valor de cero en ese campo
                else if (verBase == "N" && (((TextBox)e.Item.Cells[9].Controls[1]).Text == "0"))
                        fila[9] = System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                //Si la cuenta afectada soporta valor base
                else if (verBase == "B")
                {
                    //Convierto a double el valor base
                    vb1 = System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                    //Miro en la base de datos cual es el porcentaje de valor base
                    //lo convierto a double y lo divido por 100 para saber el verdadero valor
                    vporcBase = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString() + "'"));
                    //Si el valor introducido es debito entonces se calcula el valor base con base en este
                    if (((TextBox)e.Item.Cells[7].Controls[1]).Text != "0")
                    {
                        res = System.Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) * 100 / vporcBase;
                        if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                            fila[9] = vb1;
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                            fila[9] = res;
                        }
                    }
                    //Si el valor introducido es credito entonces se calcula el valor base con base en este
                    else if (((TextBox)e.Item.Cells[8].Controls[1]).Text != "0")
                    {
                        res = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text) * 100 / vporcBase;
                        if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                            fila[9] = vb1;
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                            fila[9] = res;
                        }
                    }
                }
                tablaNC.Rows.Add(fila);
                gridNC.DataSource = tablaNC;
                gridNC.DataBind();
                Session["tablaNC"] = tablaNC;
                if (tipo == "C")
                {
                    CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));
                    double valor = Convert.ToDouble(Double.Parse(tbTotal.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");
                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                    if (DiferenciaCreditosDebitos())
                        btnAceptar.Enabled = true;
                    else
                        btnAceptar.Enabled = false;
                }
                else if (tipo == "P")
                {
                    CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");
                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                    if (DiferenciaCreditosDebitos())
                        btnAceptar.Enabled = true;
                    else
                        btnAceptar.Enabled = false;
                }
            }
        }


        private void gridNC_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (tipo == "C")
                {
                    ((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='" + prefFac + "' AND mfac_numedocu=" + numFac + "");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlAlmacen")), "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlCentroCosto")), "SELECT pcen_codigo,pcen_codigo ||'-'||pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' order by 1");
                }
                else if (tipo == "P")
                {
                    ((TextBox)e.Item.Cells[6].FindControl("numnittxt")).Text = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefFac + "' AND mfac_numeordepago=" + numFac + "");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlAlmacen")), "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlCentroCosto")), "SELECT pcen_codigo,pcen_codigo ||'-'||pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' order by 1");
                }
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlAlm")), "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                    ((DropDownList)e.Item.Cells[2].FindControl("ddlAlm")).SelectedValue = tablaNC.Rows[e.Item.DataSetIndex][2].ToString();
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].FindControl("ddlCenCos")), "SELECT pcen_codigo,pcen_codigo ||'-'||pcen_nombre FROM pcentrocosto where timp_codigo <> 'N' order by 1");
                    ((DropDownList)e.Item.Cells[2].FindControl("ddlCenCos")).SelectedValue = tablaNC.Rows[e.Item.DataSetIndex][3].ToString();
            }
        }

        protected void btnActualizar_Click(object sender, System.EventArgs e)
        {
            string clase = "";
            DataSet ds = new DataSet();
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (tipo == "C")
            {
                if (EsAdministrativa(prefFac, numFac, ref clase))
                {
                    if (clase == "A")
                        CalcularTotales(prefFac, numFac, dtActivos, dgActivos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    else if (clase == "D")
                        CalcularTotales(prefFac, numFac, dtDiferidos, dgDiferidos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    else if (clase == "O")
                        CalcularTotales(prefFac, numFac, dtOperativos, dgOperativos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    else if (clase == "V")
                        CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    tbvalIva.Text = valorIvaDev.ToString("N");

                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    //tbTotal.Text = (Convert.ToDouble(tbValor.Text) + Convert.ToDouble(tbFletes.Text) + Convert.ToDouble(tbIvaFletes.Text) + Convert.ToDouble(tbvalIva.Text) - Convert.ToDouble(tbValRet.Text)).ToString("C");
                    // tbTotal.Text = (Convert.ToDouble(tbValor.Text) + Convert.ToDouble(tbvalIva.Text) - Convert.ToDouble(tbValRet.Text)).ToString("C");
                    tbTotal.Text = Valor.ToString("C");
                    tbValor.Text = tbValor.Text;
                    tbValRet.Text = tbValRet.Text;
                    //tbTotal.Text = tbTotal.Text;
                    //tbvalIva.Text = tbvalIva.Text;

                    //lbIva.Text = tbvalIva.Text;


                }
            }
            else if (tipo == "P")
            {
                if (EsAdministrativa(prefFac, numFac, ref clase))
                {
                    if (clase == "A")
                        CalcularTotales(prefFac, numFac, dtActivos, dgActivos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    else if (clase == "D")
                            CalcularTotales(prefFac, numFac, dtDiferidos, dgDiferidos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    else if (clase == "O")
                            CalcularTotales(prefFac, numFac, dtOperativos, dgOperativos, 2, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    tbvalIva.Text = valorIvaDev.ToString("N");
                    tbValRet.Text = valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");



                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                }
            }
            dtIva.Rows.Clear();
            tablaRtns.Rows.Clear();
            Mostrar_gridRtns();
            Mostrar_dgIva();
            SumarRetenciones();
            CalcularIva();
        }

        protected void btnAceptar_Click(object sender, System.EventArgs e)
        {
            if(!Tools.General.validarCierreFinanzas(Convert.ToDateTime(tbDate.Text).GetDateTimeFormats()[5], "C"))
            {
                Utils.MostrarAlerta(Response, "La fecha escrita no corresponde a la vigencia del sistema de cartera. Por favor revise.");
                return;
            }
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            double devoluciones = 0;
            double TotalDev = 0;
            double valorfactura = 0;//Convert.ToDouble(lbTotFac.Text);
            double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            if (rbFC.Checked)
            {
                try
                {
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT COALESCE(MFAC_VALOABON,0) FROM MNOTACLIENTE MN,MFACTURACLIENTE MFC WHERE MFC.PDOC_CODIGO = MN.PDOC_CODIGO AND MFC.MFAC_NUMEDOCU = MN.MNOT_NUMERO AND MN.mnot_prefdocu = '" + prefFac + "' AND MN.MNOT_NUMDOCU = " + numFac + "");
                    devoluciones = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                }
                catch
                {
                    devoluciones = 0;
                }
                DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT COALESCE(SUM(MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET-MFAC_VALORETE),0) FROM MFACTURACLIENTE MFC WHERE MFC.PDOC_CODIGO = '" + prefFac + "' AND MFC.MFAC_NUMEDOCU = " + numFac + "");
                valorfactura = Convert.ToDouble(ds1.Tables[0].Rows[0][0]);
                TotalDev = Valor + devoluciones;
                if (TotalDev > valorfactura)
                {
                    Utils.MostrarAlerta(Response, "Valor de las Devoluciones " + TotalDev + " son mayores al SALDO de la factura " + valorfactura + " Por Favor Revise !!!");
                    return;
                }
            }
            else if (rbFP.Checked)
            {
                try
                {
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT COALESCE(MFAC_VALOABON,0) FROM MNOTAPROVEEDOR MN, MFACTURAPROVEEDOR MFP WHERE MFP.PDOC_CODIORDEPAGO = MN.PDOC_PREFDOCREF AND MFP.MFAC_NUMEORDEPAGO = MN.MNOT_NUMEDOCREF AND MN.PDOC_PREFDOCREF = '" + prefFac + "' AND MN.MNOT_NUMEDOCREF = " + numFac + "");
                    //devoluciones = Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                    //devoluciones = Convert.ToDouble(tbvalIva.Text);
                    devoluciones = 0;
                }
                catch
                {
                    devoluciones = 0;
                }
                DBFunctions.Request(ds1, IncludeSchema.NO, "SELECT COALESCE(SUM(MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE),0) FROM MFACTURAPROVEEDOR MFP WHERE MFP.PDOC_CODIORDEPAGO = '" + prefFac + "' AND MFP.MFAC_NUMEORDEPAGO = " + numFac + "");
                valorfactura = Convert.ToDouble(ds1.Tables[0].Rows[0][0]);
                TotalDev = Valor + devoluciones;
                if (TotalDev > valorfactura)
                {
                    Utils.MostrarAlerta(Response, "Valor de las devoluciones " + TotalDev + " son mayores al SALDO de la factura " + valorfactura + " Por Favor Revise !!!");
                    return;
                }
            }
            string clase = "";
            string error = "", nota = "", numnota = "";
            /*dtIva.Rows.Clear();
            tablaRtns.Rows.Clear();
            Mostrar_gridRtns();
            Mostrar_dgIva();*/
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='" + tbCuenta.Text + "' and timp_codigo in ('A','P') and tcue_codigo <> 'N'; "))
            {
                Utils.MostrarAlerta(Response, "La cuenta No existe o es NO imputable o es solo NIIF.");
                return;
            }

            SumarRetenciones();
            CalcularIva();
            if (tipo == "C")
            {
                if (EsAdministrativa(prefFac, numFac, ref clase))
                {
                    if (Request["eliminar"] == null)
                    {
                        if (clase == "A")
                        {
                            if (GuardarNotaActivo(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                //contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), DateTime.Now.Date());
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                        else if (clase == "D")
                        {
                            if (GuardarNotaDiferido(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");

                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                        else if (clase == "O")
                        {
                            if (GuardarNotaOperativo(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                        else if (clase == "V")
                        {
                            if (GuardarNotaVarios(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                    }
                    else 
                    {
                        double valorAbonado = Convert.ToDouble(DBFunctions.SingleData("select COALESCE(mfac_valoabon,0) from mfacturacliente where pdoc_codigo='" + prefFac + "' and mfac_numedocu=" + numFac + " ORDER BY mfac_numedocu"));
                        if (valorAbonado == 0)
                        {
                            ArrayList sqlString = new ArrayList();
                            sqlString.Add("update mfacturacliente set mfac_valofact=0, mfac_valoiva=0, mfac_valoflet=0, mfac_valoivaflet=0, mfac_valorete=0, mfac_costo=0, mfac_observacion= 'ANULADO POR ' CONCAT '" + HttpContext.Current.User.Identity.Name.ToLower() + " ' CONCAT '" + DateTime.Now + " ' CONCAT mfac_observacion where pdoc_codigo='" + prefFac + "' and mfac_numedocu=" + numFac + ";");
                            DBFunctions.Transaction(sqlString);

                            DateTime fechaFactura = Convert.ToDateTime(DBFunctions.SingleData("select  mfac_factura from mfacturacliente where pdoc_codigo='" + prefFac + "' and mfac_numedocu=" + numFac + ";"));
                            Comprobante compDelete = new Comprobante();
                            compDelete.Type = prefFac;
                            compDelete.Number = numFac;
                            compDelete.Year = fechaFactura.Year.ToString();
                            compDelete.Month = fechaFactura.Month.ToString();
                            compDelete.DeleteRecord(numFac);

                            Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&eliminado=1");
                        }
                        else 
                        {
                            Utils.MostrarAlerta(Response, "Esta factura no se puede eliminar ya que tiene un valor abonado.");
                        }
                    }
                }
                else
                {
                    if (Request["eliminar"] == null)
                    {
                        if (GuardarNotaVarios(ref error, ref nota, ref numnota))
                        {
                            // Contabilidad ON LINE;
                            contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                            Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                        }
                        else
                        {
                            if (error.StartsWith("Error"))
                                lb.Text = error;
                            else
                                Utils.MostrarAlerta(Response, "" + error + "");
                        }
                    }
                    else 
                    {
                        Utils.MostrarAlerta(Response, "Esta factura no se puede eliminar ya que No es una factura administrativa.");
                    }
                }
            }
            else if (tipo == "P")
            {
                if (EsAdministrativa(prefFac, numFac, ref clase))
                {
                    if (Request["eliminar"] == null)
                    {
                        if (clase == "A")
                        {
                            if (GuardarNotaActivo(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                        else if (clase == "D")
                        {
                            if (GuardarNotaDiferido(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                        else if (clase == "O")
                        {
                            if (GuardarNotaOperativo(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                        else if (clase == "V")
                        {
                            NotaDevolucionProveedor.observacionDevolucion = txtObservacion.Text;
                            if (GuardarNotaVarios(ref error, ref nota, ref numnota))
                            {
                                // Contabilidad ON LINE;
                                contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                                Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                            }
                            else
                            {
                                if (error.StartsWith("Error"))
                                    lb.Text = error;
                                else
                                    Utils.MostrarAlerta(Response, "" + error + "");
                            }
                        }
                    }
                    else 
                    {
                        double valorAbonado = Convert.ToDouble(DBFunctions.SingleData("select mfac_valoabon from mfacturaproveedor where pdoc_codiordepago='" + prefFac + "' and mfac_numeordepago=" + numFac + ";"));
                        if (valorAbonado == 0)
                        {
                            ArrayList sqlString = new ArrayList();
                            sqlString.Add("update mfacturaproveedor set mfac_valofact=0, mfac_valoiva=0, mfac_valoflet=0, mfac_valoivaflet=0, mfac_valorete=0, mfac_observacion='ANULADO POR ' CONCAT '" + HttpContext.Current.User.Identity.Name.ToLower() + " ' CONCAT '" + DateTime.Now + " ' CONCAT mfac_observacion where pdoc_codiordepago='" + prefFac + "' and mfac_numeordepago=" + numFac + ";");
                            DBFunctions.Transaction(sqlString);

                            DateTime fechaFactura = Convert.ToDateTime(DBFunctions.SingleData("select  mfac_factura from mfacturaproveedor where pdoc_codiordepago='" + prefFac + "' and mfac_numeordepago=" + numFac + ";"));
                            Comprobante compDelete = new Comprobante();
                            compDelete.Type = prefFac;
                            compDelete.Number = numFac;
                            compDelete.Year = fechaFactura.Year.ToString();
                            compDelete.Month = fechaFactura.Month.ToString();
                            compDelete.DeleteRecord(numFac);

                            Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&eliminado=1");
                        }
                        else
                        {
                            Utils.MostrarAlerta(Response, "Esta factura no se puede eliminar ya que tiene un valor abonado.");
                        }
                    }
                }
                else
                {
                    if (Request["eliminar"] == null)
                    {
                        if (GuardarNotaVarios(ref error, ref nota, ref numnota))
                        {
                            // Contabilidad ON LINE;
                            contaOnline.contabilizarOnline(nota.ToString(), Convert.ToInt32(numnota.ToString()), Convert.ToDateTime(tbDate.Text), "");
                            Response.Redirect(indexPage + "?process=Cartera.DevolucionFacAdmin&prefN=" + nota + "&numN=" + numnota + "");
                        }
                        else
                        {
                            if (error.StartsWith("Error"))
                                lb.Text = error;
                            else
                                Utils.MostrarAlerta(Response, "" + error + "");
                        }
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "Esta factura no se puede eliminar ya que No es una factura administrativa.");
                    }
                }
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que devuelve un booleano que determina si el prefijo y el número
        /// de la factura pertenecen a una administrativa
        /// </summary>
        /// <param name="prefijo">string prefijo de la factura</param>
        /// <param name="numero">string numero de la factura</param>
        /// <param name="clase">referencia string clase de factura administrativa A=Activos Fijos, D=Gastos Diferidos, O=Gastos Operativos, V=Gastos Varios, N=No es Administrativa</param>
        /// <returns>bool true: si es una administrativa; false: si no es una administrativa</returns>
        private bool EsAdministrativa(string prefijo, string numero, ref string clase)
        {
            bool existe = false;
            clase = "N";
            if (tipo == "C")
            {
                if (DBFunctions.RecordExist("SELECT * FROM dgastoactivocliente WHERE mfac_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "A";
                }
                else if (DBFunctions.RecordExist("SELECT * FROM dgastodiferidocliente WHERE mfac_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "D";
                }
                else if (DBFunctions.RecordExist("SELECT * FROM dgastooperativocliente WHERE mfac_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "O";
                }
                else if (DBFunctions.RecordExist("SELECT * FROM dgastosvarioscliente WHERE pdoc_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "V";
                }
                else
                {
                    existe=false;
                    //existe = true;
                    clase = "V";
                    Utils.MostrarAlerta(Response, "Este documento NO ES administrativa");
                }
            }
            else if (tipo == "P")
            {
                if (DBFunctions.RecordExist("SELECT * FROM dgastoactivoproveedor WHERE mfac_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "A";
                }
                else if (DBFunctions.RecordExist("SELECT * FROM dgastodiferidoproveedor WHERE mfac_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "D";
                }
                else if (DBFunctions.RecordExist("SELECT * FROM dgastooperativoproveedor WHERE mfac_codigo='" + prefijo + "' AND mfac_numero=" + numero + ""))
                {
                    existe = true;
                    clase = "O";
                }
                else if (DBFunctions.RecordExist("SELECT * FROM dgastosvariosproveedor WHERE pdoc_codiordepago='" + prefijo + "' AND mfac_numeordepago=" + numero + ""))
                {
                    existe = true;
                    clase = "V";
                }
                else
                {
                    existe=false;
                    //existe = true;
                    clase = "V";
                    Utils.MostrarAlerta(Response, "Este documento NO ES administrativa " + prefFac + " " + numero + "");
                }
            }
            return existe;
        }

        private void CargarEstructuraDtActivos()
        {
            dtActivos = new DataTable();
            dtActivos.Columns.Add("CODIGO ACTIVO FIJO", typeof(string));
            dtActivos.Columns.Add("NOMBRE", typeof(string));
            dtActivos.Columns.Add("VALOR", typeof(double));
        }

        private void CargarEstructuraDtDiferidos()
        {
            dtDiferidos = new DataTable();
            dtDiferidos.Columns.Add("CODIGO DIFERIDO", typeof(string));
            dtDiferidos.Columns.Add("NOMBRE", typeof(string));
            dtDiferidos.Columns.Add("VALOR", typeof(double));
        }

        private void CargarEstructuraDtOperativos()
        {
            dtOperativos = new DataTable();
            dtOperativos.Columns.Add("CODIGO GASTO", typeof(string));
            dtOperativos.Columns.Add("DETALLE", typeof(string));
            dtOperativos.Columns.Add("VALOR", typeof(double));
            dtOperativos.Columns.Add("CONSECUTIVO", typeof(int));
        }

        public void Cargar_tablaNC()
        {
            tablaNC = new DataTable();
            tablaNC.Columns.Add(new DataColumn("DESCRIPCION", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("CUENTA", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("SEDE", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("CENTROCOSTO", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("PREFIJO", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("NUMERO", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("NIT", typeof(string)));
            tablaNC.Columns.Add(new DataColumn("VALORDEBITO", typeof(double)));
            tablaNC.Columns.Add(new DataColumn("VALORCREDITO", typeof(double)));
            tablaNC.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
        }

        private void CargarActivosFijos(string prefijo, string numero)
        {
            DataSet ds = new DataSet();
            if (dtActivos == null)
                CargarEstructuraDtActivos();
            if (tipo == "C")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DGAS.mact_codigo AS \"CODIGO ACTIVO FIJO\",MACT.mafj_descripcion AS \"NOMBRE\",DGAS.dgas_valor AS \"VALOR\" FROM dbxschema.dgastoactivocliente DGAS,dbxschema.mactivofijo MACT WHERE DGAS.mact_codigo=MACT.mafj_codiacti AND DGAS.pdoc_codigo='" + prefijo + "' AND DGAS.mfac_numero=" + numero + "");
            else if (tipo == "P")
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DGAS.mact_codigo AS \"CODIGO ACTIVO FIJO\",MACT.mafj_descripcion AS \"NOMBRE\",DGAS.dgas_valor AS \"VALOR\" FROM dbxschema.dgastoactivoproveedor DGAS,dbxschema.mactivofijo MACT WHERE DGAS.mact_codigo=MACT.mafj_codiacti AND DGAS.mfac_codigo='" + prefijo + "' AND DGAS.mfac_numero=" + numero + "");
            dtActivos = ds.Tables[0];
            dgActivos.DataSource = dtActivos;
            DatasToControls.Aplicar_Formato_Grilla(dgActivos);
            dgActivos.DataBind();
            Session["dtActivos"] = dtActivos;
            dgActivos.Visible = true;
            dgDiferidos.Visible = dgOperativos.Visible = dgVarios.Visible = false;
        }

        private void CargarDiferidos(string prefijo, string numero)
        {
            DataSet ds = new DataSet();
            if (dtDiferidos == null)
                CargarEstructuraDtDiferidos();
            if (tipo == "C")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DGAS.mdif_codigo AS \"CODIGO DIFERIDO\",MDIF.mdif_nombdife AS \"NOMBRE\",DGAS.dgas_valor AS \"VALOR\" FROM dbxschema.dgastodiferidocliente DGAS,dbxschema.mdiferido MDIF WHERE DGAS.mdif_codigo=MDIF.mdif_codidife AND DGAS.mfac_codigo='" + prefijo + "' AND DGAS.mfac_numero=" + numero + "");
            else if (tipo == "P")
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DGAS.mdif_codigo AS \"CODIGO DIFERIDO\",MDIF.mdif_nombdife AS \"NOMBRE\",DGAS.dgas_valor AS \"VALOR\" FROM dbxschema.dgastodiferidoproveedor DGAS,dbxschema.mdiferido MDIF WHERE DGAS.mdif_codigo=MDIF.mdif_codidife AND DGAS.mfac_codigo='" + prefijo + "' AND DGAS.mfac_numero=" + numero + "");
            dtDiferidos = ds.Tables[0];
            dgDiferidos.DataSource = dtDiferidos;
            DatasToControls.Aplicar_Formato_Grilla(dgDiferidos);
            dgDiferidos.DataBind();
            Session["dtDiferidos"] = dtDiferidos;
            dgDiferidos.Visible = true;
            dgActivos.Visible = dgOperativos.Visible = dgVarios.Visible = false;
        }

        private void CargarOperativos(string prefijo, string numero)
        {
            DataSet ds = new DataSet();
            if (dtOperativos == null)
                CargarEstructuraDtOperativos();
            if (tipo == "C")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DGAS.ptip_codigo AS \"CODIGO GASTO\",DGAS.dgas_detalle AS \"DETALLE\",DGAS.dgas_valor AS \"VALOR\",DGAS.dgas_consecutivo AS \"CONSECUTIVO\" FROM dbxschema.dgastooperativocliente DGAS WHERE DGAS.mfac_codigo='" + prefijo + "' AND DGAS.mfac_numero=" + numero + "");
            else if (tipo == "P")
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DGAS.ptip_codigo AS \"CODIGO GASTO\",DGAS.dgas_detalle AS \"DETALLE\",DGAS.dgas_valor AS \"VALOR\",DGAS.dgas_consecutivo AS \"CONSECUTIVO\" FROM dbxschema.dgastooperativoproveedor DGAS WHERE DGAS.mfac_codigo='" + prefijo + "' AND DGAS.mfac_numero=" + numero + "");
            dtOperativos = ds.Tables[0];
            dgOperativos.DataSource = dtOperativos;
            DatasToControls.Aplicar_Formato_Grilla(dgOperativos);
            dgOperativos.DataBind();
            Session["dtOperativos"] = dtOperativos;
            dgOperativos.Visible = true;
            dgActivos.Visible = dgDiferidos.Visible = dgVarios.Visible = false;
        }

        private void CargarVarios(string prefijo, string numero)
        {
            DataSet ds = new DataSet();
            if (tablaNC == null)
                Cargar_tablaNC();
            if (tipo == "C")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT dgas_concepto AS \"DESCRIPCION\",mcue_codipuc AS \"CUENTA\",palm_almacen AS \"SEDE\",pcen_codigo AS \"CENTROCOSTO\",dgas_prefdocu AS \"PREFIJO\",dgas_numedocu AS \"NUMERO\",mnit_nit AS \"NIT\",CASE WHEN dgas_naturaleza='D' THEN 0 ELSE dgas_valor END AS \"VALORDEBITO\",CASE WHEN dgas_naturaleza='C' THEN 0 ELSE dgas_valor  END AS \"VALORCREDITO\",dgas_valobase AS \"VALORBASE\",dgas_consecutivo AS \"CONSECUTIVO\" FROM dbxschema.dgastosvarioscliente WHERE pdoc_codigo='" + prefijo + "' AND mfac_numero=" + numero + "");
            else if (tipo == "P")
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT dgas_concepto AS \"DESCRIPCION\",mcue_codipuc AS \"CUENTA\",palm_almacen AS \"SEDE\",pcen_codigo AS \"CENTROCOSTO\",dgas_prefdocu AS \"PREFIJO\",dgas_numedocu AS \"NUMERO\",mnit_nit AS \"NIT\",CASE WHEN dgas_naturaleza='D' THEN 0 ELSE dgas_valor END AS \"VALORDEBITO\",CASE WHEN dgas_naturaleza='C' THEN 0 ELSE dgas_valor END AS \"VALORCREDITO\",dgas_valobase AS \"VALORBASE\",dgas_consecutivo AS \"CONSECUTIVO\" FROM dbxschema.dgastosvariosproveedor WHERE pdoc_codiordepago='" + prefijo + "' AND mfac_numeordepago=" + numero + "");

            if (ds.Tables.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No existe el documento " + prefFac + " - " + numero + "");
                return;
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                tablaNC.ImportRow(ds.Tables[0].Rows[i]);
            Mostrar_gridNC();
            gridNC.Visible = true;
            Mostrar_dgIva();
            dgIva.Visible = true;
            Mostrar_gridRtns();
            gridRtns.Visible = true;
            dgActivos.Visible = dgDiferidos.Visible = dgOperativos.Visible = false;
        }

        private void EstablecerEstadosControles(string prefijo, string numero)
        {
            string tipoDocH = DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'");
            string tipoDoc = tipoDocH.Trim();
            if (tipoDoc == "FC")
                rbFC.Checked = true;
            else if (tipoDoc == "FP")
                    rbFP.Checked = true;
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlPrefijo, "SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='" + tipoDoc + "'");
            DatasToControls.EstablecerValueDropDownList(ddlPrefijo, prefijo);
            //if(tipoDoc=="FC")
            //{
            //    bind.PutDatasIntoDropDownList(ddlNumero, "SELECT mfac_numedocu FROM mfacturacliente WHERE pdoc_codigo='" + prefijo + "' ORDER BY mfac_numedocu");
            //    DatasToControls.EstablecerValueDropDownList(ddlNumero, numero);
            //}
            //if(tipoDoc=="FP")
            //{
            //    bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mfac_numeordepago FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijo+"' ORDER BY mfac_numeordepago");
            //    DatasToControls.EstablecerValueDropDownList(ddlNumero,numero);
            //}
            txtNumero.Text = numero;
        }

        private void CargarPrefijosNotas(string prefijo)
        {
            string tipoDocH = DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'");
            string tipoDoc = tipoDocH.Trim();
            DatasToControls bind = new DatasToControls();
            if (tipoDoc == "FC")
                //bind.PutDatasIntoDropDownList(ddlPrefNota,"SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NC' and PH.tpro_proceso in ('AC') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                Utils.llenarPrefijos(Response, ref ddlPrefNota, "AC", "%", "NC");
            else if (tipoDoc == "FP")
                    //(bind.PutDatasIntoDropDownList(ddlPrefNota,"SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NP' and PH.tpro_proceso in ('AP') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                    Utils.llenarPrefijos(Response, ref ddlPrefNota, "AP", "%", "NP");
        }

        private void CalcularTotales(string prefijo, string numero, DataTable dt, DataGrid dg, int indiceValor, ref double valorNota, ref double valorIva, ref double valorRtns)
        //		private void CalcularTotales(string prefijo,string numero,DataTable dt,            int indiceValorDeb,int indiceValorCre,ref double valorNota,ref double valorIva,ref double valorRtns)
        {
            double vn = 0, vi = 0, vr = 0;
            DataSet ds = new DataSet();
            string tipoDocH = DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'");
            string tipoDoc = tipoDocH.Trim();
            if (tipoDoc == "FC")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact+mfac_valoflet,mfac_valoiva+mfac_valoivaflet,mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='" + prefijo + "' AND mfac_numedocu=" + numero + "");
            else if (tipoDoc == "FP")
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact+mfac_valoflet,mfac_valoiva+mfac_valoivaflet,mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefijo + "' AND mfac_numeordepago=" + numero + "");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    if (((CheckBox)dg.Items[i].Cells[dg.Items[i].Cells.Count - 1].FindControl("cbDev")).Checked)
                        vn = vn + Convert.ToDouble(dt.Rows[i][indiceValor]);
                }
                catch
                {
                    vn = vn + Convert.ToDouble(dt.Rows[i][indiceValor]);
                }
            }
            vi = (vn / Convert.ToDouble(ds.Tables[0].Rows[0][0])) * Convert.ToDouble(ds.Tables[0].Rows[0][1]);
            vr = (vn / Convert.ToDouble(ds.Tables[0].Rows[0][0])) * Convert.ToDouble(ds.Tables[0].Rows[0][2]);
            valorNota = vn;
            valorIva = vi;
            valorRtns = vr;
            if (valorNota + valorIva - valorRtns <= 0)
                btnAceptar.Enabled = false;
            else
                btnAceptar.Enabled = true;
        }

        private void CalcularIva()
        {
            double valIva = 0, valIvaFle = 0;
            for (int i = 0; i < dtIva.Rows.Count; i++)
            {
                if (dtIva.Rows[i][5].ToString() == "IVA")
                    valIva = valIva + Convert.ToDouble(dtIva.Rows[i][2]);
                else if (dtIva.Rows[i][5].ToString() == "IVA Fletes")
                        valIvaFle = valIvaFle + Convert.ToDouble(dtIva.Rows[i][2]);
            }
            tbvalIva.Text = valIva.ToString("N");
        //    tbIvaFletes.Text = valIvaFle.ToString("N");
            double total = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            total += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            total += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            total += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            total -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            tbvalIva.Text = valIva.ToString("N");
            tbTotal.Text = total.ToString("C");
        }

        private void CalcularTotales(string prefijo, string numero, DataTable dt, int indiceValorDeb, int indiceValorCre, ref double valorNota, ref double valorIva, ref double valorRtns)
        {
            double vn = 0, vi = 0, vr = 0, vd = 0, vc = 0;
            DataSet ds = new DataSet();
            string tipoDocH = DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'");
            string tipoDoc = tipoDocH.Trim();
            if (tipoDoc.Trim() == "FC")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact+mfac_valoflet,mfac_valoiva+mfac_valoivaflet,mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='" + prefijo + "' AND mfac_numedocu=" + numero + "");
            else if (tipoDoc.Trim() == "FP")
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_valofact+mfac_valoflet,mfac_valoiva+mfac_valoivaflet,mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefijo + "' AND mfac_numeordepago=" + numero + "");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                vd = vd + Convert.ToDouble(tablaNC.Rows[i][indiceValorDeb]);
                vc = vc + Convert.ToDouble(tablaNC.Rows[i][indiceValorCre]);
            }
            if (tipoDoc.Trim() == "FC")
                vn = vd - vc;
            else if (tipoDoc.Trim() == "FP")
                    vn = vc - vd;

            vi = (vn / Convert.ToDouble(ds.Tables[0].Rows[0][0])) * Convert.ToDouble(ds.Tables[0].Rows[0][1]);
            vr = (vn / Convert.ToDouble(ds.Tables[0].Rows[0][0])) * Convert.ToDouble(ds.Tables[0].Rows[0][2]);
            if (Double.IsNaN(vi)) vi = 0;
            if (Double.IsNaN(vr)) vr = 0;
            if (Double.IsNaN(vn)) vn = 0;

            valorNota = vn;
            valorIva = vi;
            valorRtns = vr;
            if (valorNota + valorIva - valorRtns < 0)
                Utils.MostrarAlerta(Response, "El Balance de las cuentas está NEGATIVO. Revisar los Valores");
            else
                btnActualizar.Enabled = true;
            if (valorNota + valorIva - valorRtns <= 0)
                btnAceptar.Enabled = false;
            else
                btnAceptar.Enabled = true;
        }

        protected bool Validar_Datos(DataGridCommandEventArgs e)
        {
            bool error = false;
            //Si hay algun campo en blanco o no son validos los valores
            if ((((TextBox)e.Item.Cells[0].Controls[1]).Text == "") || (((TextBox)e.Item.Cells[1].Controls[1]).Text == "") || (((DropDownList)e.Item.Cells[2].Controls[1]).Items.Count == 0) || (((DropDownList)e.Item.Cells[3].Controls[1]).Items.Count == 0) || (((TextBox)e.Item.Cells[4].Controls[1]).Text == "") || (!DatasToControls.ValidarInt(((TextBox)e.Item.Cells[5].Controls[1]).Text)) || (((TextBox)e.Item.Cells[6].Controls[1]).Text == "") || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text)) || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text)) || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text)))
            {
                Utils.MostrarAlerta(Response, "Falta un Campo por Llenar o las entradas son Invalidos. Revisa tus Datos");
                error = true;
            }
            //Si en los dos campos valor Debito y valor Credito hay un valor distinto de cero
            else if (((Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) != 0) && (Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text) != 0)))
            {
                Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener un valor de 0. Revisa tus Datos");
                error = true;
            }
            //Si en ninguno de los dos campos hay valor
            else if ((((((TextBox)e.Item.Cells[7].Controls[1]).Text == "0") && (((TextBox)e.Item.Cells[8].Controls[1]).Text == "0"))))
            {
                Utils.MostrarAlerta(Response, "Uno de los campos Valor Debito o Valor Credito debe tener valor. Revisa tus Datos");
                error = true;
            }
            /*
            //Si ya se ingreso el prefijo y numero de documento de referencia
            else if(Buscar_Documento(((TextBox)e.Item.Cells[4].FindControl("prefijotxt")).Text,((TextBox)e.Item.Cells[4].FindControl("numdocutxt")).Text))
            {
                Response.Write("<script language:javascript>alert('El prefijo y el número de documento de referencia ya fueron ingresados anteriormente');</script>");
                error=true;
            }*/
            //Si el nit digitado no existe
            else if (!DBFunctions.RecordExist("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ((TextBox)e.Item.Cells[6].Controls[1]).Text + "'"))
            {
                Utils.MostrarAlerta(Response, "El nit especificado no existe");
                error = true;
            }
            //Si la cuenta digitada no existe
            else if (!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='" + ((TextBox)e.Item.Cells[1].Controls[1]).Text + "' and timp_codigo in ('A','P') and tcue_codigo <> 'N'"))
            {
                    Utils.MostrarAlerta(Response, "La cuenta especificada no existe o es NO Imputable o es solo NIIF");
                    error = true;
            }
            return error;
        }

        public void Mostrar_gridNC()
        {
            Session["tablaNC"] = tablaNC;
            gridNC.DataSource = tablaNC;
            gridNC.DataBind();
        }

        protected bool IgualesCreditosDebitos()
        {
            bool iguales = false;
            double totalDebito = 0, totalCredito = 0;
            for (int i = 0; i < tablaNC.Rows.Count; i++)
            {
                totalDebito = totalDebito + (Convert.ToDouble(tablaNC.Rows[i][7]));
                totalCredito = totalCredito + (Convert.ToDouble(tablaNC.Rows[i][8]));
            }
            if (totalDebito == totalCredito)
                iguales = true;
            return iguales;
        }

        protected bool DiferenciaCreditosDebitos()
        {
            bool diferencia = false;
            double totalDebito = 0, totalCredito = 0;
            for (int i = 0; i < tablaNC.Rows.Count; i++)
            {
                totalDebito = totalDebito + (Convert.ToDouble(tablaNC.Rows[i][7]));
                totalCredito = totalCredito + (Convert.ToDouble(tablaNC.Rows[i][8]));
            }
            if (tipo == "C")
            {
                //Debitos-Creditos
                if (totalDebito > totalCredito)
                    diferencia = true;
            }
            else if (tipo == "P")
            {
                //Creditos-Debitos
                if (totalCredito > totalDebito)
                    diferencia = true;
            }
            return diferencia;
        }

        private bool ExisteDepreciacionActivoFijo(string codAct)
        {
            bool existe = false;
            if (DBFunctions.RecordExist("SELECT * FROM dactivofijo WHERE dafj_codiacti='" + codAct + "'"))
                existe = true;
            return existe;
        }

        private bool ExisteAmortizacionDiferido(string dif)
        {
            bool existe = false;
            if (Convert.ToDouble(DBFunctions.SingleData("SELECT mdif_valoamtz FROM mdiferido WHERE mdif_codidife='" + dif + "'")) > 0)
                existe = true;
            return existe;
        }

        private bool GuardarNotaActivo(ref string error, ref string prefijo, ref string numero)
        {
            bool exito = false;
            bool existeDep = false;
            uint numNota = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefNota.SelectedValue + "'"));
            DataSet ds;
            NotaDevolucionCliente miNotaC = new NotaDevolucionCliente();
            NotaDevolucionProveedor miNotaP = new NotaDevolucionProveedor();

            double tbValorNum = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            double tbFletesNum = Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbIvaFletesNum = Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbvalIvaNum = Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            double tbValRetNum = Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            if (tipo == "C")
            {
                //miNotaC=new NotaDevolucionCliente(ddlPrefNota.SelectedValue,prefFac,numNota,uint.Parse(numFac),"NA","FA",Convert.ToDouble(tbValor.Text),Convert.ToDouble(tbvalIva.Text),Convert.ToDouble(tbValRet.Text),Convert.ToDouble(tbFletes.Text),Convert.ToDouble(tbIvaFletes.Text),DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower(),dtIva,tablaRtns,ViewState["NIT"].ToString());
                miNotaC = new NotaDevolucionCliente(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "NA", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, Convert.ToDateTime(tbDate.Text), HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < dtActivos.Rows.Count; i++)
                {
                    if (((CheckBox)dgActivos.Items[i].Cells[3].FindControl("cbDev")).Checked)
                    {
                        if (!ExisteDepreciacionActivoFijo(dtActivos.Rows[i][0].ToString()))
                        {
                            miNotaC.SqlRels.Add("UPDATE mactivofijo SET tvig_vigencia='V' WHERE mafj_codiacti='" + dtActivos.Rows[i][0].ToString() + "'");
                            ds = new DataSet();
                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ttip_codigo,ptip_codigo FROM dgastoactivocliente WHERE mfac_codigo='" + prefFac + "' AND mfac_numero=" + numFac + " AND mact_codigo='" + dtActivos.Rows[i][0].ToString() + "'");
                            miNotaC.SqlRels.Add("INSERT INTO dgastoactivocliente VALUES('@1',@2," + ds.Tables[0].Rows[0][0].ToString() + "," + ds.Tables[0].Rows[0][1].ToString() + ",'" + dtActivos.Rows[i][0].ToString() + "'," + dtActivos.Rows[i][2].ToString() + ")");
                        }
                        else
                        {
                            existeDep = true;
                            error += "El Activo Fijo " + dtActivos.Rows[i][0].ToString() + " presenta depreciaciones. Imposible continuar el proceso \\n";
                            break;
                        }
                    }
                }
                if (!existeDep)
                {
                    miNotaC.SqlRels.Add("INSERT INTO mfacturaadministrativacliente VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                    if (miNotaC.GrabarNotaDevolucionCliente(true))
                    {
                        exito = true;
                        error += miNotaC.ProcessMsg;
                        prefijo = miNotaC.PrefijoNota;
                        numero = miNotaC.NumeroNota.ToString();
                    }
                    else
                        error += "Error " + miNotaC.ProcessMsg;
                }
            }
            else if (tipo == "P")
            {
                miNotaP = new NotaDevolucionProveedor(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "NA", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, DateTime.Now, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < dtActivos.Rows.Count; i++)
                {
                    if (((CheckBox)dgActivos.Items[i].Cells[3].FindControl("cbDev")).Checked)
                    {
                        if (!ExisteDepreciacionActivoFijo(dtActivos.Rows[i][0].ToString()))
                        {
                            miNotaP.SqlRels.Add("UPDATE mactivofijo SET tvig_vigencia='C' WHERE mafj_codiacti='" + dtActivos.Rows[i][0].ToString() + "'");
                            ds = new DataSet();
                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ttip_codigo,ptip_codigo FROM dgastoactivoproveedor WHERE mfac_codigo='" + prefFac + "' AND mfac_numero=" + numFac + " AND mact_codigo='" + dtActivos.Rows[i][0].ToString() + "'");
                            miNotaP.SqlRels.Add("INSERT INTO dgastoactivoproveedor VALUES('@1',@2," + ds.Tables[0].Rows[0][0].ToString() + "," + ds.Tables[0].Rows[0][1].ToString() + ",'" + dtActivos.Rows[i][0].ToString() + "'," + dtActivos.Rows[i][2].ToString() + ")");
                        }
                        else
                        {
                            existeDep = true;
                            error += "El Activo Fijo " + dtActivos.Rows[i][0].ToString() + " presenta depreciaciones. Imposible continuar el proceso \\n";
                            break;
                        }
                    }
                }
                if (!existeDep)
                {
                    if (miNotaP.GrabarNotaDevolucionProveedor(true))
                    {
                        miNotaP.SqlRels.Add("INSERT INTO mfacturaadministrativaproveedor VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                        exito = true;
                        error += miNotaP.ProcessMsg;
                        prefijo = miNotaP.PrefijoNota;
                        numero = miNotaP.NumeroNota.ToString();
                    }
                    else
                        error += "Error " + miNotaP.ProcessMsg;
                }
            }
            return exito;
        }

        private bool GuardarNotaDiferido(ref string error, ref string prefijo, ref string numero)
        {
            bool exito = false;
            bool existeAmtz = false;
            uint numNota = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefNota.SelectedValue + "'"));
            DataSet ds;
            NotaDevolucionCliente miNotaC = new NotaDevolucionCliente();
            NotaDevolucionProveedor miNotaP = new NotaDevolucionProveedor();

            double tbValorNum = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            double tbFletesNum = Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbIvaFletesNum = Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbvalIvaNum = Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            double tbValRetNum = Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            if (tipo == "C")
            {
                //miNotaC=new NotaDevolucionCliente(ddlPrefNota.SelectedValue,prefFac,numNota,uint.Parse(numFac),"A","FA",Convert.ToDouble(tbValor.Text),Convert.ToDouble(tbvalIva.Text),Convert.ToDouble(tbValRet.Text),Convert.ToDouble(tbFletes.Text),Convert.ToDouble(tbIvaFletes.Text),DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower(),dtIva,tablaRtns,ViewState["NIT"].ToString());
                miNotaC = new NotaDevolucionCliente(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, DateTime.Now, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < dtDiferidos.Rows.Count; i++)
                {
                    if (((CheckBox)dgDiferidos.Items[i].Cells[3].FindControl("cbDev")).Checked)
                    {
                        if (!ExisteAmortizacionDiferido(dtDiferidos.Rows[i][0].ToString()))
                        {
                            miNotaC.SqlRels.Add("UPDATE mdiferido SET tvig_vigencia='V' WHERE mdif_codidife='" + dtDiferidos.Rows[i][0].ToString() + "'");
                            ds = new DataSet();
                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ttip_codigo,ptip_codigo FROM dgastodiferidocliente WHERE mfac_codigo='" + prefFac + "' AND mfac_numero=" + numFac + " AND mdif_codigo='" + dtDiferidos.Rows[i][0].ToString() + "'");
                            miNotaC.SqlRels.Add("INSERT INTO dgastodiferidocliente VALUES('@1',@2," + ds.Tables[0].Rows[0][0].ToString() + "," + ds.Tables[0].Rows[0][1].ToString() + ",'" + dtDiferidos.Rows[i][0].ToString() + "'," + dtDiferidos.Rows[i][2].ToString() + ")");
                        }
                        else
                        {
                            existeAmtz = true;
                            error += "El Gasto Diferido " + dtActivos.Rows[i][0].ToString() + " presenta amortizaciones. Imposible continuar el proceso \\n";
                            break;
                        }
                    }
                }
                if (!existeAmtz)
                {
                    miNotaC.SqlRels.Add("INSERT INTO mfacturaadministrativacliente VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                    if (miNotaC.GrabarNotaDevolucionCliente(true))
                    {
                        exito = true;
                        error += miNotaC.ProcessMsg;
                        prefijo = miNotaC.PrefijoNota;
                        numero = miNotaC.NumeroNota.ToString();
                    }
                    else
                        error += "Error " + miNotaC.ProcessMsg;
                }
            }
            else if (tipo == "P")
            {
                //miNotaP=new NotaDevolucionProveedor(ddlPrefNota.SelectedValue,prefFac,numNota,uint.Parse(numFac),"A","FA",Convert.ToDouble(tbValor.Text),Convert.ToDouble(tbvalIva.Text),Convert.ToDouble(tbValRet.Text),Convert.ToDouble(tbFletes.Text),Convert.ToDouble(tbIvaFletes.Text),DateTime.Now,HttpContext.Current.User.Identity.Name.ToLower(),dtIva,tablaRtns,ViewState["NIT"].ToString());
                miNotaP = new NotaDevolucionProveedor(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, DateTime.Now, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < dtDiferidos.Rows.Count; i++)
                {
                    if (((CheckBox)dgDiferidos.Items[i].Cells[3].FindControl("cbDev")).Checked)
                    {
                        if (!ExisteAmortizacionDiferido(dtDiferidos.Rows[i][0].ToString()))
                        {
                            miNotaP.SqlRels.Add("UPDATE mdiferido SET tvig_vigencia='C' WHERE mdif_codidife='" + dtDiferidos.Rows[i][0].ToString() + "'");
                            ds = new DataSet();
                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ttip_codigo,ptip_codigo FROM dgastodiferidoproveedor WHERE mfac_codigo='" + prefFac + "' AND mfac_numero=" + numFac + " AND mdif_codigo='" + dtDiferidos.Rows[i][0].ToString() + "'");
                            miNotaP.SqlRels.Add("INSERT INTO dgastodiferidoproveedor VALUES('@1',@2," + ds.Tables[0].Rows[0][0].ToString() + "," + ds.Tables[0].Rows[0][1].ToString() + ",'" + dtDiferidos.Rows[i][0].ToString() + "'," + dtDiferidos.Rows[i][2].ToString() + ")");
                        }
                        else
                        {
                            existeAmtz = true;
                            error += "El Gasto Diferido " + dtDiferidos.Rows[i][0].ToString() + " presenta amortizaciones. Imposible continuar el proceso \\n";
                            break;
                        }
                    }
                }
                if (!existeAmtz)
                {
                    miNotaP.SqlRels.Add("INSERT INTO mfacturaadministrativaproveedor VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                    if (miNotaP.GrabarNotaDevolucionProveedor(true))
                    {
                        exito = true;
                        error += miNotaP.ProcessMsg;
                        prefijo = miNotaP.PrefijoNota;
                        numero = miNotaP.NumeroNota.ToString();
                    }
                    else
                        error += "Error " + miNotaP.ProcessMsg;
                }
            }
            return exito;
        }

        private bool GuardarNotaOperativo(ref string error, ref string prefijo, ref string numero)
        {
            bool exito = false;
            uint numNota = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefNota.SelectedValue + "'"));
            DataSet ds;
            NotaDevolucionCliente miNotaC = new NotaDevolucionCliente();
            NotaDevolucionProveedor miNotaP = new NotaDevolucionProveedor();

            double tbValorNum = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            double tbFletesNum = Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbIvaFletesNum = Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbvalIvaNum = Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            double tbValRetNum = Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            if (tipo == "C")
            {
                miNotaC = new NotaDevolucionCliente(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, DateTime.Now, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < dtOperativos.Rows.Count; i++)
                {
                    if (((CheckBox)dgOperativos.Items[i].Cells[3].FindControl("cbDev")).Checked)
                    {
                        ds = new DataSet();
                        DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ttip_codigo,ptip_codigo,dgas_docurefe FROM dgastooperativocliente WHERE mfac_codigo='" + prefFac + "' AND mfac_numero=" + numFac + " AND dgas_consecutivo=" + dtOperativos.Rows[i][3].ToString() + "");
                        miNotaC.SqlRels.Add("INSERT INTO dgastooperativocliente VALUES('@1',@2," + ds.Tables[0].Rows[0][0].ToString() + "," + ds.Tables[0].Rows[0][1].ToString() + ",'" + dtOperativos.Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[0][2].ToString() + "'," + dtOperativos.Rows[i][2].ToString() + "," + i + ")");
                    }
                }
                miNotaC.SqlRels.Add("INSERT INTO mfacturaadministrativacliente VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                if (miNotaC.GrabarNotaDevolucionCliente(true))
                {
                    exito = true;
                    error += miNotaC.ProcessMsg;
                    prefijo = miNotaC.PrefijoNota;
                    numero = miNotaC.NumeroNota.ToString();
                }
                else
                    error += "Error " + miNotaC.ProcessMsg;
            }
            else if (tipo == "P")
            {
                miNotaP = new NotaDevolucionProveedor(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, DateTime.Now, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < dtOperativos.Rows.Count; i++)
                {
                    if (((CheckBox)dgOperativos.Items[i].Cells[3].FindControl("cbDev")).Checked)
                    {
                        ds = new DataSet();
                        DBFunctions.Request(ds, IncludeSchema.NO, "SELECT ttip_codigo,ptip_codigo,dgas_docurefe FROM dgastooperativoproveedor WHERE mfac_codigo='" + prefFac + "' AND mfac_numero=" + numFac + " AND dgas_consecutivo=" + dtOperativos.Rows[i][3].ToString() + "");
                        miNotaP.SqlRels.Add("INSERT INTO dgastooperativoproveedor VALUES('@1',@2," + ds.Tables[0].Rows[0][0].ToString() + "," + ds.Tables[0].Rows[0][1].ToString() + ",'" + dtOperativos.Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[0][2].ToString() + "'," + dtOperativos.Rows[i][2].ToString() + "," + i + ")");
                    }
                }
                miNotaP.SqlRels.Add("INSERT INTO mfacturaadministrativaproveedor VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                if (miNotaP.GrabarNotaDevolucionProveedor(true))
                {
                    exito = true;
                    error += miNotaP.ProcessMsg;
                    prefijo = miNotaP.PrefijoNota;
                    numero = miNotaP.NumeroNota.ToString();
                }
                else
                    error += "Error " + miNotaP.ProcessMsg;
            }
            return exito;
        }

        private bool GuardarNotaVarios(ref string error, ref string prefijo, ref string numero)
        {
            bool exito = false;
            uint numNota = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefNota.SelectedValue + "'"));
            NotaDevolucionCliente miNotaC = new NotaDevolucionCliente();
            NotaDevolucionProveedor miNotaP = new NotaDevolucionProveedor();

            double tbValorNum   = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
            double tbFletesNum  = Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbIvaFletesNum = Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
            double tbvalIvaNum  = Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
            double tbValRetNum  = Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

            if (tipo == "C")
            {
                if (tbDate != null){
                    DateTime fechaNota = Convert.ToDateTime(tbDate.Text.ToString());
                    miNotaC = new NotaDevolucionCliente(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, fechaNota, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                }
                else
                    miNotaC = new NotaDevolucionCliente(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, DateTime.Now, HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());

                for (int i = 0; i < tablaNC.Rows.Count; i++)
                {
                    //Si es debito y tiene valor base
                    if (((Convert.ToDouble(tablaNC.Rows[i][7].ToString())) != 0) && ((Convert.ToDouble(tablaNC.Rows[i][9].ToString())) != 0))
                        miNotaC.SqlRels.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + System.Convert.ToDouble(tablaNC.Rows[i][7].ToString()).ToString() + ",'D'," + System.Convert.ToDouble(tablaNC.Rows[i][9].ToString()).ToString() + ")");
                    //Si es debito y no tiene valor base
                    else if (((System.Convert.ToDouble(tablaNC.Rows[i][7].ToString())) != 0) && ((System.Convert.ToDouble(tablaNC.Rows[i][9].ToString())) == 0))
                        miNotaC.SqlRels.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + (System.Convert.ToDouble(tablaNC.Rows[i][7].ToString())).ToString() + ",'D',0)");
                    //Si es credito y tiene valor base
                    else if (((System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())) != 0) && ((System.Convert.ToDouble(tablaNC.Rows[i][9].ToString())) != 0))
                        miNotaC.SqlRels.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + (System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())).ToString() + ",'C'," + System.Convert.ToDouble(tablaNC.Rows[i][9].ToString()).ToString() + ")");
                    //Si es credito y no tiene valor base
                    else if (((System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())) != 0) && ((System.Convert.ToDouble(tablaNC.Rows[i][9].ToString())) == 0))
                        miNotaC.SqlRels.Add("INSERT INTO dgastosvarioscliente VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + (System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())).ToString() + ",'C',0)");
                }
                miNotaC.SqlRels.Add("INSERT INTO mfacturaadministrativacliente VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                NotaDevolucionCliente.observacionDevolucion = txtObservacion.Text;
                if (miNotaC.GrabarNotaDevolucionCliente(true))
                {
                    exito = true;
                    error += miNotaC.ProcessMsg;
                    prefijo = miNotaC.PrefijoNota;
                    numero = miNotaC.NumeroNota.ToString();
                }
                else
                    error += "Error " + miNotaC.ProcessMsg;
            }
            else if (tipo == "P")
            {
                miNotaP = new NotaDevolucionProveedor(ddlPrefNota.SelectedValue, prefFac, numNota, uint.Parse(numFac), "A", "FA", tbValorNum, tbvalIvaNum, tbValRetNum, tbFletesNum, tbIvaFletesNum, Convert.ToDateTime(tbDate.Text), HttpContext.Current.User.Identity.Name.ToLower(), dtIva, tablaRtns, ViewState["NIT"].ToString());
                for (int i = 0; i < tablaNC.Rows.Count; i++)
                {
                    //Si es debito y tiene valor base
                    if (((Convert.ToDouble(tablaNC.Rows[i][7].ToString())) != 0) && ((Convert.ToDouble(tablaNC.Rows[i][9].ToString())) != 0))
                        miNotaP.SqlRels.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + System.Convert.ToDouble(tablaNC.Rows[i][7].ToString()).ToString() + ",'D'," + System.Convert.ToDouble(tablaNC.Rows[i][9].ToString()).ToString() + ")");
                    //Si es debito y no tiene valor base
                    else if (((System.Convert.ToDouble(tablaNC.Rows[i][7].ToString())) != 0) && ((System.Convert.ToDouble(tablaNC.Rows[i][9].ToString())) == 0))
                        miNotaP.SqlRels.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + (System.Convert.ToDouble(tablaNC.Rows[i][7].ToString())).ToString() + ",'D',0)");
                    //Si es credito y tiene valor base
                    else if (((System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())) != 0) && ((System.Convert.ToDouble(tablaNC.Rows[i][9].ToString())) != 0))
                        miNotaP.SqlRels.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + (System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())).ToString() + ",'C'," + System.Convert.ToDouble(tablaNC.Rows[i][9].ToString()).ToString() + ")");
                    //Si es credito y no tiene valor base
                    else if (((System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())) != 0) && ((System.Convert.ToDouble(tablaNC.Rows[i][9].ToString())) == 0))
                        miNotaP.SqlRels.Add("INSERT INTO dgastosvariosproveedor VALUES('@1',@2," + i.ToString() + ",'" + tablaNC.Rows[i][6].ToString().Trim() + "','" + tablaNC.Rows[i][4].ToString().Trim() + "'," + System.Convert.ToInt64(tablaNC.Rows[i][5].ToString().Trim()).ToString() + ",'" + tablaNC.Rows[i][1].ToString().Trim() + "','" + tablaNC.Rows[i][0].ToString().Trim() + "','" + tablaNC.Rows[i][2].ToString().Trim() + "','" + tablaNC.Rows[i][3].ToString().Trim() + "'," + (System.Convert.ToDouble(tablaNC.Rows[i][8].ToString())).ToString() + ",'C',0)");
                }
                miNotaP.SqlRels.Add("INSERT INTO mfacturaadministrativaproveedor VALUES ('@1',@2,'" + tbCuenta.Text + "')");
                if (miNotaP.GrabarNotaDevolucionProveedor(true))
                {
                    exito = true;
                    error += miNotaP.ProcessMsg;
                    prefijo = miNotaP.PrefijoNota;
                    numero = miNotaP.NumeroNota.ToString();
                }
                else
                    error += "Error " + miNotaP.ProcessMsg;
            }
            return exito;
        }

        #endregion

        #region Métodos Ajax

        [Ajax.AjaxMethod]
        public DataSet CambiarPrefijoFactura(string prefijo, string tipo)
        {
            DataSet ds = new DataSet();
            if (tipo == "rbFC")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_numedocu AS NUMERO FROM mfacturacliente WHERE pdoc_codigo='" + prefijo + "' ORDER BY mfac_numedocu");
            else if (tipo == "rbFP")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mfac_numeordepago AS NUMERO FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefijo + "' ORDER BY mfac_numeordepago");
            return ds;
        }

        [Ajax.AjaxMethod]
        public DataSet CambiarTipoFactura(string tipo)
        {
            checkeado = tipo;
            DataSet ds = new DataSet();
            if (tipo == "rbFC")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pdoc_codigo AS PREFIJO,pdoc_codigo CONCAT ' - ' CONCAT pdoc_NOMBRE AS DESCRIPCION FROM pdocumento WHERE tdoc_tipodocu='FC' and tvig_vigencia = 'V' ORDER BY pdoc_NOMBRE");
            else if (tipo == "rbFP")
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pdoc_codigo AS PREFIJO,pdoc_codigo CONCAT ' - ' CONCAT pdoc_NOMBRE AS DESCRIPCION FROM pdocumento WHERE tdoc_tipodocu='FP' and tvig_vigencia = 'V' ORDER BY pdoc_NOMBRE");
            return ds;
        }

        [Ajax.AjaxMethod]
        public String cargar_Obs(string pref, string num)
        {
            String observacion = "Nota Devolucion a " + pref +" - " + num + " ";
            if (checkeado == "rbFP")
            {
                observacion += DBFunctions.SingleData("select MFAC_OBSERVACION from mfacturaproveedor WHERE PDOC_CODIORDEPAGO = '" + pref + "' AND MFAC_NUMEORDEPAGO = " + num);
            }
            else
            {
                observacion += DBFunctions.SingleData("select MFAC_OBSERVACION from mfacturacliente WHERE PDOC_CODIGO = '" + pref + "' AND MFAC_NUMEDOCU = " + num);
            }
                

            return observacion;
        }
        #endregion

        #region Código generado por el Diseñador de Web Forms
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Método necesario para admitir el Diseñador. No se puede modificar
        ///		el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridNC.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridNC_ItemCommand);
            this.gridNC.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridNC_CancelCommand);
            this.gridNC.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridNC_EditCommand);
            this.gridNC.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridNC_UpdateCommand);
            this.gridNC.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.gridNC_ItemDataBound);
            this.dgIva.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.dgIva_ItemCommand);
            this.dgIva.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgIva_ItemDataBound);

        }
        #endregion

        private void gridNC_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            gridNC.EditItemIndex = -1;
            Mostrar_gridNC();
        }

        private void gridNC_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            gridNC.EditItemIndex = e.Item.DataSetIndex;
            Mostrar_gridNC();
        }

        private void gridNC_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            string verBase;
            double vb1 = 0, vporcBase, res;
            if (rbFC.Checked)
                tipo = "C";
            else if (rbFP.Checked)
                    tipo = "P";
            if (!Validar_Datos(e))
            {
                tablaNC.Rows[e.Item.DataSetIndex][0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                tablaNC.Rows[e.Item.DataSetIndex][1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                tablaNC.Rows[e.Item.DataSetIndex][2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
                tablaNC.Rows[e.Item.DataSetIndex][3] = ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue;
                tablaNC.Rows[e.Item.DataSetIndex][4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                tablaNC.Rows[e.Item.DataSetIndex][5] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
                tablaNC.Rows[e.Item.DataSetIndex][6] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;

                double valDebito = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
                double valCredito = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
                double valBase = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                //Si el valor introducido es debito
                if (valDebito != 0 && valCredito == 0)
                {
                    tablaNC.Rows[e.Item.DataSetIndex][7] = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
                    tablaNC.Rows[e.Item.DataSetIndex][8] = Convert.ToDouble("0");
                }
                //Si el valor introducido es credito
                else if (valDebito == 0 && valCredito != 0)
                {
                    tablaNC.Rows[e.Item.DataSetIndex][7] = System.Convert.ToDouble("0");
                    tablaNC.Rows[e.Item.DataSetIndex][8] = System.Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
                }
                //Ahora verificamos los valores del valor base
                verBase = DBFunctions.SingleData("SELECT tbas_codigo FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString() + "'");
                //Si la cuenta no soporta valor base y hay algun valor distinto de cero en ese campo
                if (verBase == "N" && valBase != 0)
                {
                    Utils.MostrarAlerta(Response, "La cuenta afectada no soporta Valor Base por tanto se guardara un valor de 0");
                    tablaNC.Rows[e.Item.DataSetIndex][9] = System.Convert.ToDouble("0");
                }
                //Si la cuenta no soporta valor base y hay un valor de cero en ese campo
                else if (verBase == "N" && valBase == 0)
                        tablaNC.Rows[e.Item.DataSetIndex][9] = System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                //Si la cuenta afectada soporta valor base
                else if (verBase == "B")
                {
                    //Convierto a double el valor base
                    vb1 = System.Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
                    //Miro en la base de datos cual es el porcentaje de valor base
                    //lo convierto a double y lo divido por 100 para saber el verdadero valor
                    vporcBase = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mcue_basegrav FROM mcuenta WHERE mcue_codipuc='" + (((TextBox)e.Item.Cells[1].Controls[1]).Text).ToString() + "'"));
                    //Si el valor introducido es debito entonces se calcula el valor base con base en este
                    if (valDebito != 0)
                    {
                        res = System.Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) * 100 / vporcBase;
                        if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                            tablaNC.Rows[e.Item.DataSetIndex][9] = vb1;
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                            tablaNC.Rows[e.Item.DataSetIndex][9] = res;
                        }
                    }
                    //Si el valor introducido es credito entonces se calcula el valor base con base en este
                    else if (valCredito != 0)
                    {
                        res = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text) * 100 / vporcBase;
                        if (res >= vb1 * 0.99 && res <= vb1 * 1.01)
                            tablaNC.Rows[e.Item.DataSetIndex][9] = vb1;
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor Base se encontraba mal calculado y se reemplazo por el valor real");
                            tablaNC.Rows[e.Item.DataSetIndex][9] = res;
                        }
                    }
                }
                tablaNC.AcceptChanges();
                gridNC.EditItemIndex = -1;
                Mostrar_gridNC();
                if (tipo == "C")
                {
                    CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");
                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                    if (DiferenciaCreditosDebitos())
                        btnAceptar.Enabled = true;
                    else
                        btnAceptar.Enabled = false;
                }
                else if (tipo == "P")
                {
                    CalcularTotales(prefFac, numFac, tablaNC, 7, 8, ref valorNotaDev, ref valorIvaDev, ref valorRetDev);
                    tbValor.Text = valorNotaDev.ToString("N");
                    //tbvalIva.Text=valorIvaDev.ToString("N");
                    //tbValRet.Text=valorRetDev.ToString("N");

                    double Valor = Convert.ToDouble(Double.Parse(tbValor.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbIvaFletes.Text, System.Globalization.NumberStyles.Currency));
                    Valor += Convert.ToDouble(Double.Parse(tbvalIva.Text, System.Globalization.NumberStyles.Currency));
                    Valor -= Convert.ToDouble(Double.Parse(tbValRet.Text, System.Globalization.NumberStyles.Currency));

                    tbTotal.Text = Valor.ToString("C");
                    //tbTotal.Text=(Convert.ToDouble(tbValor.Text)+Convert.ToDouble(tbFletes.Text)+Convert.ToDouble(tbIvaFletes.Text)+Convert.ToDouble(tbvalIva.Text)-Convert.ToDouble(tbValRet.Text)).ToString("C");
                    if (DiferenciaCreditosDebitos())
                        btnAceptar.Enabled = true;
                    else
                        btnAceptar.Enabled = false;
                }
            }
        }
        protected void gridRtns_Item(object Sender, DataGridCommandEventArgs e)
        {
            DataRow fila;
            if (((Button)e.CommandSource).CommandName == "AgregarRetencion")
            {
                if ((((TextBox)e.Item.FindControl("codret")).Text == ""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
                else if (this.Buscar_Retencion(((TextBox)e.Item.FindControl("codret")).Text))
                    Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
                else if (!DatasToControls.ValidarDouble(((TextBox)e.Item.FindControl("base")).Text))
                    Utils.MostrarAlerta(Response, "Valor Invalido");
                else
                {
                    fila = tablaRtns.NewRow();
                    fila["CODRET"] = ((TextBox)e.Item.FindControl("codret")).Text;
                    fila["PORCRET"] = Convert.ToDouble(((TextBox)e.Item.FindControl("codretb")).Text);
                    fila["VALORBASE"] = Convert.ToDouble(((TextBox)e.Item.FindControl("base")).Text);
                    fila["VALOR"] = Convert.ToDouble(((TextBox)e.Item.FindControl("valor")).Text);
                    fila["TIPORETE"] = ((DropDownList)e.Item.FindControl("ddlTiporet")).SelectedItem;
                    if (!ValidarBasesRetencion(Convert.ToDouble(((TextBox)e.Item.FindControl("valor")).Text)))
                    {
                        Utils.MostrarAlerta(Response, "Los valores base de las retenciones superan el valor de la factura");
                        return;
                    }
                    tablaRtns.Rows.Add(fila);
                    this.Mostrar_gridRtns();
                    SumarRetenciones();
                }
            }
            else if (((Button)e.CommandSource).CommandName == "RemoverRetencion")
            {
                tablaRtns.Rows[e.Item.DataSetIndex].Delete();
                SumarRetenciones();
                this.Mostrar_gridRtns();
            }
        }

        protected void gridRtns_ItemDataBound(object Sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                TextBox txtBase = (TextBox)e.Item.FindControl("base");
                TextBox txtCodRete = (TextBox)e.Item.FindControl("codret");
                TextBox txtPorcentaje = (TextBox)e.Item.FindControl("codretb");
                TextBox txtValor = (TextBox)e.Item.FindControl("valor");
                DropDownList ddlTiporetencion = (DropDownList)e.Item.FindControl("ddlTiporet");

                DatasToControls bind = new DatasToControls();
                ddlTiporetencion.Attributes.Add("onChange", "Cambio_Retencion(this," + ((TextBox)e.Item.FindControl("codret")).ClientID + ",'','" + txtPorcentaje.ClientID + "','" + txtBase.ClientID + "','" + txtValor.ClientID + "', '" + tipo + "'  )");
                bind.PutDatasIntoDropDownList(ddlTiporetencion, "select * from tretencion order by 2;");
                ddlTiporetencion.Items.Insert(0, new ListItem("Seleccione...", "0"));

                string scrTotal = "PorcentajeVal('" + txtPorcentaje.ClientID + "','" + txtBase.ClientID + "','" + txtValor.ClientID + "');";
                txtBase.Attributes.Add("onkeyup", "NumericMaskE(this,event);" + scrTotal);

                if (rbFC.Checked)
                    txtCodRete.Attributes.Add("onClick", "ModalDialog(this,'SELECT pret_codigo,pret_nombre,pret_porcennodecl FROM pretencion ORDER BY tret_codigo',new Array());" + scrTotal);
                    //((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'SELECT pret_codigo,pret_nombre,pret_porcennodecl FROM pretencion ORDER BY tret_codigo',new Array());" + scrTotal);
                else if (rbFP.Checked)
                {
                    if (ViewState["NIT"] != null)
                    {
                        Retencion rtns = new Retencion(ViewState["NIT"].ToString(), false);
                        if (rtns.TipoSociedad == "N" || rtns.TipoSociedad == "P" || rtns.TipoSociedad == "U")
                            txtCodRete.Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo;',new Array());" + scrTotal);
                            //((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcennodecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr where pr.ttip_codigo IN (\\'N\\',\\'T\\') ORDER BY tipo;',new Array());" + scrTotal);
                        else
                            txtCodRete.Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcendecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr ORDER BY tipo;',new Array());" + scrTotal);
                            //((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'SELECT pr.pret_codigo codigo,pr.pret_nombre nombre,pr.pret_porcendecl porcentaje,pr.ttip_proceso proceso, pr.tret_codigo tipo, pr.mcue_codipucprov cuenta FROM pretencion pr ORDER BY tipo;',new Array());" + scrTotal);
                    }
                }
            }
        }


        private void dgIva_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            DataRow fila;
            if (e.CommandName == "AgregarIva")
            {
                if (!DatasToControls.ValidarDouble(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")).SelectedValue) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[0].FindControl("tbValIvaI")).Text))
                    Utils.MostrarAlerta(Response, "Algún dato es erroneo");
                else if (!DBFunctions.RecordExist("SELECT mcue_codipuc FROM mcuenta WHERE mcue_codipuc='" + ((TextBox)e.Item.Cells[2].FindControl("tbCuentaI")).Text + "' and timp_codigo in ('A','P') and tcue_codigo <> 'N' "))
                        Utils.MostrarAlerta(Response, "La cuenta especificada es inexistente o es NO Imputable o es solo NIIF");
                else
                {
                    if (!ExisteIva(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")).SelectedValue, ((DropDownList)e.Item.Cells[3].FindControl("ddlnit")).SelectedValue))
                    {
                        fila = dtIva.NewRow();
                        fila[0] = Convert.ToDouble(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")).SelectedValue);
                        fila[1] = Convert.ToDouble(((TextBox)e.Item.Cells[1].FindControl("tbValIvaBase")).Text);
                        fila[2] = Convert.ToDouble(((TextBox)e.Item.Cells[2].FindControl("tbValIvaI")).Text);
                        fila[3] = ((TextBox)e.Item.Cells[3].FindControl("tbCuentaI")).Text;
                        fila[4] = ((DropDownList)e.Item.Cells[4].FindControl("ddlnit")).SelectedValue;
                        if (((RadioButton)e.Item.Cells[5].FindControl("rbiva1")).Checked)
                            fila[5] = ((RadioButton)e.Item.Cells[5].FindControl("rbiva1")).Text;
                        else if (((RadioButton)e.Item.Cells[5].FindControl("rbiva2")).Checked)
                            fila[5] = ((RadioButton)e.Item.Cells[5].FindControl("rbiva2")).Text;
                        if (!SuperaIvaaBase(Convert.ToDouble(fila[2]), fila[5].ToString()))
                        {
                            dtIva.Rows.Add(fila);
                            Mostrar_dgIva();
                            CalcularIva();
                            //CalcularTotalFactura();
                        }
                        else
                        {
                            Utils.MostrarAlerta(Response, "El valor base del " + fila[5].ToString() + " supera el valor base. Revise por favor");
                            return;
                        }
                    }
                    else
                        Utils.MostrarAlerta(Response, "La relación nit - iva ya existe.");
                }
            }
            else if (e.CommandName == "RemoverIva")
            {
                dtIva.Rows[e.Item.ItemIndex].Delete();
                Mostrar_dgIva();
                CalcularIva();
                //CalcularTotalFactura();
            }
        }

        private void dgIva_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
            {
                LlenarDropDownListNits(((DropDownList)e.Item.FindControl("ddlnit")));
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].FindControl("ddlPorcIva")), "SELECT piva_porciva FROM piva ORDER BY piva_porciva");

                TextBox txtBase = (TextBox)e.Item.FindControl("tbValIvaBase");
                DropDownList ddlPorcIva = (DropDownList)e.Item.FindControl("ddlPorcIva");
                TextBox txtValor = (TextBox)e.Item.FindControl("tbValIvaI");
                string scrTotal = "PorcentajeVal('" + ddlPorcIva.ClientID + "','" + txtBase.ClientID + "','" + txtValor.ClientID + "');";
                txtBase.Attributes.Add("onkeyup", "NumericMaskE(this,event);" + scrTotal);
                ddlPorcIva.Attributes.Add("onchange", scrTotal);
            }
        }
        private void LlenarDropDownListNits(DropDownList ddl)
        {
            ListItem item = null;
            if (ViewState["NIT"] != null)
                if (rbFC.Checked)
                {
                    item = new ListItem(DBFunctions.SingleData("SELECT mnit_nit || ' - ' || mnit_apellidos ||' '|| COALESCE(mnit_apellido2,'') ||' '|| mnit_nombres ||' '|| COALESCE(mnit_nombre2,'') FROM dbxschema.mnit WHERE mnit_nit='" + ViewState["NIT"] + "'"), ViewState["NIT"].ToString());
                    ddl.Items.Add(item);

                }
                else if (rbFP.Checked)
                {
                    item = new ListItem(DBFunctions.SingleData("SELECT mnit_nit || ' - ' || mnit_apellidos ||' '|| COALESCE(mnit_apellido2,'') ||' '|| mnit_nombres ||' '|| COALESCE(mnit_nombre2,'') FROM dbxschema.mnit WHERE mnit_nit='" + ViewState["NIT"] + "'"), ViewState["NIT"].ToString());
                    ddl.Items.Add(item);
                }
        }


        private bool ExisteIva(string iva, string nit)
        {
            DataRow[] ivas = dtIva.Select("PORCENTAJE=" + iva + " AND NIT='" + nit + "'");
            if (ivas.Length != 0)
                return true;
            else
                return false;
        }

        private bool SuperaIvaaBase(double valor, string tipo)
        {
            bool supera = false;
            double vi = 0;
            for (int i = 0; i < dtIva.Rows.Count; i++)
            {
                if (dtIva.Rows[i][5].ToString() == tipo)
                    vi = vi + Convert.ToDouble(dtIva.Rows[i][2]);
            }
            vi = vi + valor;
            if (tipo == "IVA Fletes")
            {
                if (Convert.ToDouble(tbFletes.Text) < vi)
                    supera = true;
            }
            else if (tipo == "IVA")
            {
                if (Convert.ToDouble(lbTotFac.Text.Substring(1)) < vi)
                    supera = true;
            }
            return supera;
        }

        //trae los valores de iva a devolver en caso de que existan de la tabla dfacturaclienteiva relacionados.
        private void CargarIVADevolver(String tipoIVA)
        {
            DataSet dsValoresIVA = new DataSet();
            string consultaSQL = "";
            if(tipoIVA == "C")
                consultaSQL = "select piva_porciva, dfac_valoiva/0.16 as base , dfac_valoiva, mcue_codipuc, mnit_nit, 'IVA' as tipo_iva from dfacturaclienteiva where pdoc_codigo='" + prefFac + "' and mfac_numedocu=" + numFac + " order by mfac_numedocu;";
            else
                consultaSQL = "select piva_porciva, dfac_valoiva/0.16 as base , dfac_valoiva, mcue_codipuc, mnit_nit, 'IVA' as tipo_iva from dfacturaproveedoriva where pdoc_codiordepago='" + prefFac + "' and mfac_numeordepago=" + numFac + " order by mfac_numeordepago;";

            DBFunctions.Request(dsValoresIVA, IncludeSchema.NO, consultaSQL);
            double valorIvaFactura = 0;

            if (dsValoresIVA.Tables[0].Rows.Count > 0)
            {
                DataRow fila; 
                for(int k=0;k<dsValoresIVA.Tables[0].Rows.Count;k++)
                {
                    fila= dtIva.NewRow();
                    fila[0] = Convert.ToDouble(dsValoresIVA.Tables[0].Rows[k][0]);
                    fila[1] = Convert.ToDouble(dsValoresIVA.Tables[0].Rows[k][1]);
                    fila[2] = Convert.ToDouble(dsValoresIVA.Tables[0].Rows[k][2]);
                    fila[3] = dsValoresIVA.Tables[0].Rows[k][3];
                    fila[4] = dsValoresIVA.Tables[0].Rows[k][4];
                    fila[5] = dsValoresIVA.Tables[0].Rows[k][5];
                    
                    dtIva.Rows.Add(fila);
                    valorIvaFactura += Convert.ToDouble(dsValoresIVA.Tables[0].Rows[k][2]);
                }
                Mostrar_dgIva();
                tbvalIva.Text = valorIvaFactura.ToString("N");
            }
        }

        //trae los valores de Retencion a devolver en caso de que existan de la tabla MFACTURACLIENTERETENCION relacionados.
        private void CargarRetencionDevolver(String tipoRet)
        {
            DataSet dsValoresRetencion = new DataSet();
            string consultaSQL = "";
            if (tipoRet == "C")
                consultaSQL = "select m.pret_codigo, (select pret_porcendecl from pretencion where pret_codigo=m.pret_codigo),m.mfac_valobase, m.mfac_valorete  from MFACTURACLIENTERETENCION m where m.pdoc_codigo='" + prefFac + "' and m.mfac_numedocu=" + numFac + " order by m.mfac_numedocu;";
            else
                consultaSQL = "select  m.pret_codigo, (select pret_porcendecl from pretencion where pret_codigo=m.pret_codigo),m.mfac_valobase, m.mfac_valorete   from MFACTURAPROVEEDORRETENCION m where m.pdoc_codiordepago = '" + prefFac + "' and m.mfac_numeordepago=" + numFac + " order by m.mfac_numeordepago;";
            
            DBFunctions.Request(dsValoresRetencion, IncludeSchema.NO, consultaSQL);
            double valorRetencionFactura = 0;

            if (dsValoresRetencion.Tables[0].Rows.Count > 0)
            {
                DataRow fila;
                for (int k = 0; k < dsValoresRetencion.Tables[0].Rows.Count; k++)
                {
                    fila = tablaRtns.NewRow();
                    fila["CODRET"] = dsValoresRetencion.Tables[0].Rows[k][0];
                    fila["PORCRET"] = Convert.ToDouble(dsValoresRetencion.Tables[0].Rows[k][1]);
                    fila["VALORBASE"] = Convert.ToDouble(dsValoresRetencion.Tables[0].Rows[k][2]);
                    fila["VALOR"] = Convert.ToDouble(dsValoresRetencion.Tables[0].Rows[k][3]);

                    tablaRtns.Rows.Add(fila);
                    valorRetencionFactura += Convert.ToDouble(dsValoresRetencion.Tables[0].Rows[k][3]);
                }
                Mostrar_gridRtns();
                tbValRet.Text = valorRetencionFactura.ToString("N");
            }
        }

    }
}