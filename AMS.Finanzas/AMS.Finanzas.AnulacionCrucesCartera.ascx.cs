using System;
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


namespace AMS.Finanzas
{
    public partial class AnulacionCrucesCartera : System.Web.UI.UserControl
    {
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected string pathToMain = ConfigurationManager.AppSettings["MainIndexPage"];
        protected FormatosDocumentos formatoConsignacion;
        ProceHecEco contaOnline = new ProceHecEco();
        DatasToControls bind = new DatasToControls();
        protected DataTable tablaDatos;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null  or pcen_centteso is not null) and tvig_vigencia='V' order by palm_descripcion;");
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
                if (!DBFunctions.RecordExist("SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='AX' and tvig_vigencia = 'V' "))
                {
                    Response.Write("<script language:javascript>alert('No hay documentos del tipo AT definidos para anulación');</script>");
                    prefijoDocumento.Enabled = false;
                    prefijoDocumento.Enabled = false;
                    aceptar.Enabled = false;
                }
                else
                {
                    //bind.PutDatasIntoDropDownList(prefijoDocumento, "SELECT pdoc_codigo,pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu='AT' and tvig_vigencia = 'V' ORDER BY pdoc_descripcion ");
                    Utils.llenarPrefijos(Response, ref prefijoDocumento, "%", "%", "AX");
                    if (prefijoDocumento.Items.Count > 1)
                    {
                        prefijoDocumento.Items.Insert(0, "Seleccione:");
                    }
                    else
                        prefijoDocumento_SelectedIndexChanged(sender, e);
                    //        numeroTesoreria.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
                    prefijoDocumento.Enabled = true;
                    aceptar.Enabled = true;
                }
            }
            //holderAnulaciones.Controls.Add(LoadControl(pathToControls + "AMS.Tesoreria.ConsignaCheques.Anulaciones.ascx"));
        }

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

        }
        #endregion

        protected void prefijoDocumento_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            numeroTesoreria.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
            Int32 ultimoTesoreria = 0;
            try
            {
                ultimoTesoreria = Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(MTES_NUMERO+1) FROM mtesoreria WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'"));
            }
            catch
            {
            }
            try { 
            if (ultimoTesoreria > Convert.ToInt32(numeroTesoreria.Text.ToString()))
                numeroTesoreria.Text = ultimoTesoreria.ToString();
            }
            catch { }
        }

        protected void prefijoCruce_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlNumero, "SELECT MCRU_NUMERO FROM MCRUCEDOCUMENTO WHERE PDOC_CODIGO = '"+ddlPrefijo.SelectedValue+"'");
            ddlNumero.Items.Insert(0, "Seleccione...");
        }
        protected void guardar_Click(object sender, System.EventArgs e)
        {
            Control hijo;
            DataTable tablaDatos = new DataTable();
            Finanzas.Tesoreria.Consignacion miCruce = new AMS.Finanzas.Tesoreria.Consignacion();
            //hijo = holderAnulaciones.Controls[0];
            //if (((HtmlInputHidden)hijo.FindControl("hdnTip")).Value == "DE" || ((HtmlInputHidden)hijo.FindControl("hdnTip")).Value == "DF")
            //    EstablecerPrefijosNotas(hijo);
            //tablaDatos = (DataTable)Session["tablaDatos"];
            tablaDatos = (DataTable)Session["Datos"];
            miCruce = new Finanzas.Tesoreria.Consignacion(tablaDatos);

            if (detalleTransaccion.Text == "")
                Response.Write("<script>alert('Debe especificar un detalle');</script>");
            else
            {
                miCruce.PrefijoConsignacion = ddlPrefijo.SelectedValue;
                miCruce.NumeroConsignacion = Convert.ToInt32(ddlNumero.SelectedValue);
                string fechaAnulacion = txtFecha.Text;                
                string vigenciaContable = DBFunctions.SingleData("select pano_ano concat pmes_mes from ccontabilidad ");
                if (vigenciaContable.Length == 5)
                    vigenciaContable = vigenciaContable.Substring(0, 4) + '0' + vigenciaContable.Substring(4, 1);
                string vigenciaTransaccion = fechaAnulacion.Substring(0, 4) + fechaAnulacion.Substring(5, 2);
                if (Convert.ToDouble(vigenciaTransaccion.ToString()) < Convert.ToDouble(vigenciaContable.ToString()))
                    Response.Write("<script>alert('la fecha de la consingación es Menor a la Vigencia Contable, Transacción NO permitida');</script>");

                else
                {
                    miCruce.Almacen = this.almacen.SelectedValue;
                    miCruce.CodigoCuenta = "";
                    miCruce.Detalle = this.detalleTransaccion.Text;
                    miCruce.Fecha = fechaAnulacion;
                    miCruce.NumeroTesoreria = Convert.ToInt32(this.numeroTesoreria.Text);
                    miCruce.PrefijoDocumento = this.prefijoDocumento.SelectedValue;
                    miCruce.Proceso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    miCruce.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
                    string tem = lbValorCruce.Text;
                    if (tem == "")
                    {
                        tem = "0";
                    }
                    miCruce.Total = Convert.ToDouble(tem);
                    miCruce.TipoMov = "ACC";                    
                    if (miCruce.Guardar_Anulacion())
                    {
                        // contabilización ON LINE
                        contaOnline.contabilizarOnline(miCruce.PrefijoDocumento.ToString(), Convert.ToInt32(miCruce.NumeroTesoreria.ToString()), Convert.ToDateTime(fechaAnulacion), "");
                        lb.Text += miCruce.Mensajes;
                        Session.Clear();
                        Response.Redirect(pathToMain + "?process=Finanzas.AnulacionCrucesCartera&prefD=" + miCruce.PrefijoDocumento + "&numD=" + miCruce.NumeroTesoreria + "");
                    }
                    else
                        lb.Text += miCruce.Mensajes;
                }
            }
        }

        private void EstablecerPrefijosNotas(Control ctl)
        {
            DataGrid dg = null;
            DataTable dt = null;
            if (((HtmlInputHidden)ctl.FindControl("hdnTip")).Value == "DE")
            {
                dg = ((DataGrid)ctl.FindControl("dgDev"));
                dt = (DataTable)Session["tablaDatos"];
                for (int i = 0; i < dg.Items.Count; i++)
                    dt.Rows[i]["PREFIJO NOTA"] = ((DropDownList)dg.Items[i].Cells[10].FindControl("ddlPrefNot")).SelectedValue;
            }
            else if (((HtmlInputHidden)ctl.FindControl("hdnTip")).Value == "DF")
            {
                dg = ((DataGrid)ctl.FindControl("dgDevRem"));
                dt = (DataTable)Session["tablaDatos"];
                for (int i = 0; i < dg.Items.Count; i++)
                    dt.Rows[i]["PREFIJO NOTA DEVOLUCION"] = ((DropDownList)dg.Items[i].Cells[10].FindControl("ddlPrefNotRem")).SelectedValue;
            }
            Session["tablaDatos"] = dt;
        }

        protected void cancelar_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            Response.Redirect(pathToMain + "?process=Finanzas.AnulacionCrucesCartera");
        }

        protected void Cargar_Tabla_Cruces(DataTable dtable)
        {
            BoundColumn prefCC, numCC, tipDoccli, numDoccli, tipDocPro, numDocPro, val;
            prefCC = new BoundColumn(); prefCC.DataField = "CODIGO CRUCE"; prefCC.HeaderText = "Codigo Cruce";
            numCC = new BoundColumn(); numCC.DataField = "NUMERO CRUCE"; numCC.HeaderText = "Número Cruce";
            tipDoccli = new BoundColumn(); tipDoccli.DataField = "FACTURA CLIENTE"; tipDoccli.HeaderText = "Factura Cliente";
            numDoccli = new BoundColumn(); numDoccli.DataField = "NUMERO"; numDoccli.HeaderText = "Número Factura";
            tipDocPro = new BoundColumn(); tipDocPro.DataField = "FACTURA PROVEEDOR"; tipDocPro.HeaderText = "Factura Proveedor";
            numDocPro = new BoundColumn(); numDocPro.DataField = "NUM FP"; numDocPro.HeaderText = "Número Factura";                       
            val = new BoundColumn(); val.DataField = "VALOR"; val.DataFormatString = "{0:C}"; val.HeaderText = "Valor";
            tablaDatos = new DataTable();
            tablaDatos.Columns.Add("CODIGO CRUCE", typeof(string));
            tablaDatos.Columns.Add("NUMERO CRUCE", typeof(int));
            tablaDatos.Columns.Add("FACTURA CLIENTE", typeof(string));
            tablaDatos.Columns.Add("NUMERO", typeof(string));
            tablaDatos.Columns.Add("FACTURA PROVEEDOR", typeof(string));
            tablaDatos.Columns.Add("NUM FP", typeof(string));  
            tablaDatos.Columns.Add("VALOR", typeof(double));
            gridDatos.Columns.Add(prefCC); gridDatos.Columns.Add(numCC); gridDatos.Columns.Add(tipDoccli);
            gridDatos.Columns.Add(numDoccli); gridDatos.Columns.Add(tipDocPro); gridDatos.Columns.Add(numDocPro); gridDatos.Columns.Add(val); 
        }
        protected void aceptarTramite_Click(object sender, System.EventArgs e)
        {
            string fechaDoc =DBFunctions.SingleData("SELECT '' ||  MCRU_FECHA FROM MCRUCEDOCUMENTO WHERE  PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' AND MCRU_NUMERO = " + ddlNumero.SelectedValue + "");
            // valdia que la fecha del Documento este entre el dia en el que se hizo y el dia actual
            if (Convert.ToDateTime(fechaDoc) > Convert.ToDateTime(txtFecha.Text) || Convert.ToDateTime(txtFecha.Text) > Convert.ToDateTime (DateTime.Now) )
            {
                Utils.MostrarAlerta(Response, "La fecha de la Devolucion no puede ser menor a la fecha del Documento");
                return;
            }
           
            lbValorCruce.Text = DBFunctions.SingleData("SELECT MCRU_VALOR FROM DCRUCEDOCUMENTO WHERE PDOC_CODIGO = '"+ddlPrefijo.SelectedValue+"' AND MCRU_NUMERO = "+ddlNumero.SelectedValue +" ;");
            ((Panel)FindControl("valoresCruce")).Visible = true;
            guardar.Enabled = true;
            aceptar.Enabled = false;
            DataSet ds = new DataSet();
            int i = 0;
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT DC.PDOC_CODIGO AS \"CODIGO CRUCE\", DC.MCRU_NUMERO AS \"NUMERO CRUCE\", DC.MFAC_CODIGO AS \"FACTURA CLIENTE\", DC.MFAC_NUMERO AS NUMERO, DC.MFAC_CODIORDEPAGO AS \"FACTURA PROVEEDOR\", DC.MFAC_NUMEORDEPAGO AS \"NUM FP\", DCRU_CONSECUTIVO AS CONSECUTIVO, MNIT_NIT AS NIT,  MC.MCRU_FECHA AS \"FECHA ORIGINAL\", MCRU_VALOR AS VALOR, MC.MCRU_USUARIO AS \"USUARIO ORIGINAL\"  FROM DCRUCEDOCUMENTO DC, MCRUCEDOCUMENTO MC  WHERE DC.PDOC_CODIGO = '" + ddlPrefijo.SelectedValue+"' AND  DC.MCRU_NUMERO = "+ddlNumero.SelectedValue+" AND MC.PDOC_CODIGO = DC.PDOC_CODIGO AND MC.MCRU_NUMERO = DC.MCRU_NUMERO;");
            Session["Datos"] = ds.Tables[0];
            if (ds.Tables[0].Rows.Count != 0)
                Cargar_Tabla_Cruces(ds.Tables[0]);
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        tablaDatos.ImportRow(ds.Tables[0].Rows[i]);
                        Session["tablaDatos"] = tablaDatos;
                        gridDatos.DataSource = tablaDatos;
                        gridDatos.DataBind();
        }
        protected void aceptar_Click(object sender, System.EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlPrefijo, "select PDOC_CODIGO,PDOC_NOMBRE from pdocumento where tdoc_tipodocu = 'CX';");
            ddlPrefijo.Items.Insert(0, "Seleccione...");
            ((Panel)FindControl("panelCruces")).Visible = true; 
        }
    }
}

 