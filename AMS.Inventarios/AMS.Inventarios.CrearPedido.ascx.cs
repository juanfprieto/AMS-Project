using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using AMS.Forms;
using AMS.DB;
using AMS.Tools;
using AMS.Documentos;
using AMS.Utilidades;
using Ajax;
using AMS.Contabilidad;

namespace AMS.Inventarios
{
    public partial class CrearPedido : System.Web.UI.UserControl
    {
        #region   Atributos
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected int numDecimales = 0, year = 0, mes = 0;
        protected DataTable dtInserts;
        protected DataSet dsDetallePedido = new DataSet();
        protected DataSet ds;
        protected DataView dvInserts;
        protected ArrayList types = new ArrayList();
        protected ArrayList lbFields = new ArrayList();
        protected bool facRealizado = false,
                       procesoCombinado = false,
                       mostrarPrecioMinimoYPromedio = false;
        protected string Tipo, tipoPedido, valorReal, nNacionalidad, TipoCargo;
        protected FormatosDocumentos formatoFactura;
        private DatasToControls bind = new DatasToControls();
        FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        ProceHecEco contaOnline = new ProceHecEco();
        protected bool editarIvaPrecio;      //Convert.ToBoolean(ConfigurationSettings.AppSettings["InventarioEditarIvaPrecio"]);
        protected bool siKit= false;
        protected static string codCotizacion;
        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(CrearPedido));
            String test = Request.Form["hidValue"];

            lbInfoLeg1.Visible = lbInfoPct.Visible = false;
            Tipo = Request.QueryString["actor"]; //Debe definir el tipo de pedido: Cliente o proveedor

            if (Request.QueryString["modificar"] == null)
            {
                btnAjusFac.Attributes.Add("onclick", "this.disabled = true;" +
                    Page.ClientScript.GetPostBackEventReference(btnAjusFac, null) + ";");
                btnAjus.Attributes.Add("onclick", "this.disabled = true;" +
                    Page.ClientScript.GetPostBackEventReference(btnAjus, null) + ";");
            }

            /*
            *Si se genera un error aquí, agregar la columna a la base de datos:
            *
            *      alter table cinventario add column TRES_MOSTRARPRECPROM character(1) not null default 'N'
            *      references trespuestasino(tres_sino);
            *
            *      COMMENT ON cinventario
            *      ("TRES_MOSTRARPRECPROM" IS 'Define si se muestra el precio mínimo y promedio');
            *      
            * esta columna definirá si se debe mostrar o no el precio promedio y el precio minimo
            */



            if (Tipo == "P")
            {
                dgItems.Columns[4].Visible = false;
                dgItems.Columns[6].HeaderText = "Precio Compra :";
                dgItems.Columns[5].Visible = false;
                plSugerido.Visible = true;
                plListaPrecios.Visible = false;
                btnAjusFac.Visible = false;
                plcEstadoCliente.Visible = false;
                txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'Select t1.mnit_nit as NIT, t1.mnit_apellidos concat \\' \\' CONCAT COALESCE(t1.mnit_apellido2,\\'\\') concat \\' \\' CONCAT t1.mnit_nombres concat \\' \\' concat COALESCE(t1.mnit_nombre2,\\'\\') as Nombre from MNIT as t1,MPROVEEDOR as t2 where t1.mnit_nit=t2.mnit_nit order by Nombre', new Array());";//Carga los proveedores
            }
            else
            {
                plcEstadoCliente.Visible = true;
                if (Tipo == "C")
                {
                    txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_apellidos concat \\' \\' CONCAT COALESCE(mnit_apellido2,\\'\\') concat \\' \\' CONCAT mnit_nombres concat \\' \\' concat COALESCE(mnit_nombre2,\\'\\') as Nombre FROM mnit WHERE TVIG_VIGENCIA = \'V\' order by Nombre', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                    if (tipoPedido == "T") // las transferencias no controlan cartera
                        plcEstadoCliente.Visible = false;
                }
                else if (Tipo == "M")
                {
                        txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT mn.mnit_nit as NIT, mnit_apellidos concat \\' \\' concat COALESCE(mnit_apellido2,\\'\\') concat \\' \\' concat mnit_nombres concat \\' \\' concat COALESCE(mnit_nombre2,\\'\\') as Nombre FROM mnit mn, mcliente mc WHERE mc.mnit_nit=mn.mnit_nit and mn.tvig_vigencia=\\'V\\' order by Nombre', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                }
            }

            string citasConKits = "";
            if (ViewState["citasConKitsProgramados"] != null)
                citasConKits = ViewState["citasConKitsProgramados"].ToString();
            ClearChildViewState();
            ViewState["citasConKitsProgramados"] = citasConKits;
            LoadDataColumns();
            if (Session["dtInsertsCP"] == null || !IsPostBack)
                LoadDataTable();
            else
                dtInserts = (DataTable)Session["dtInsertsCP"];

            if (Session["dsDetallePedido"] != null)
            {
                dsDetallePedido = (DataSet)Session["dsDetallePedido"];
            }

            if (!IsPostBack)
            {
                //HABEAS DATA

                //    ViewState["num_Decimales"] = numDecimales.ToString(); 

                DataSet cinventario = new DataSet();
                DBFunctions.Request(cinventario, IncludeSchema.NO, "select ci.*, CEMP_LIQUDECI, ce.mnit_nit from cinventario ci, cempresa ce ");
                ViewState["CINVENTARIO"] = cinventario.Tables[0];
                ViewState["CASA_MATRIZ"] = DBFunctions.SingleData("SELECT CEMP_CASAMATR FROM CEMPRESA;");
                ViewState["citasConKitsProgramados"] = "";
                txtObs.Attributes.Add("maxlength", txtObs.MaxLength.ToString());

                DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
                if (cInventarioX.Rows[0]["TRES_MOSTRARPRECPROM"].ToString() == "S")
                    mostrarPrecioMinimoYPromedio = true;
                else
                    mostrarPrecioMinimoYPromedio = false;

                if (cInventarioX.Rows[0]["CINV_MODIPREC"].ToString() == "S")
                    editarIvaPrecio = true;
                else
                    editarIvaPrecio = false;
                 
                //try { mostrarPrecioMinimoYPromedio = DBFunctions.RecordExist("SELECT TRES_MOSTRARPRECPROM FROM CINVENTARIO where TRES_MOSTRARPRECPROM = 'S' "); }
                //catch { mostrarPrecioMinimoYPromedio = true; }
                //try { editarIvaPrecio = DBFunctions.RecordExist("select CINV_MODIPREC FROM CINVENTARIO WHERE CINV_MODIPREC = 'S';"); }
                //catch { editarIvaPrecio = true; }

                if (mostrarPrecioMinimoYPromedio)
                {
                    dgItems.Columns[6].Visible = true;
                }
                else
                {
                    dgItems.Columns[6].Visible = false;
                }

                //string centavos = ConfigurationManager.AppSettings["ManejoCentavos"];
                if (cInventarioX.Rows[0]["CEMP_LIQUDECI"].ToString() == "S")
                    numDecimales = 2;
                else
                    numDecimales = 0;
                //if (Utils.EsEntero(centavos))
                //    numDecimales = Convert.ToInt32(centavos);
                //bool decimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
                //if (decimales)
                //    numDecimales = Convert.ToInt32(ConfigurationManager.AppSettings["tamanoDecimal"]);


                //mostrar opción excel en ambos casos (C y P)
                fldExcel.Visible = true;

                //Revisamos si viene de una creacion anterior numLis
                lbCotiPendt.Visible = true;
                ddlCotiPendt.Visible = true;
                btnConsultaCoti.Visible = true;
                if (Tipo == "C")
                {
                    LbPediPendt.Visible = true;
                    ddlPediPendt.Visible = true;
                    Labelkits.Visible = true;
                    ddlKits.Visible = true;
                    if (Request.QueryString["numLis"] != null)
                    {
                        if (Request.QueryString["numLis"] != "")
                            Utils.MostrarAlerta(Response, "Lista de Empaque Generada Número : " + Request.QueryString["numLis"] + "!");
                        else
                            Utils.MostrarAlerta(Response, "No se ha generado lista de empaque!");
                    }
                    else if (Request.QueryString["subprocess"] != null)
                    {
                        if (Request.QueryString["subprocess"] == "Fact")
                        {
                            Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + Request.QueryString["prefF"] + " y número " + Request.QueryString["numF"] + "");
                            formatoFactura = new FormatosDocumentos();
                            try
                            {
                                formatoFactura.Prefijo = Request.QueryString["prefF"];
                                formatoFactura.Numero = Convert.ToInt32(Request.QueryString["numF"]);
                                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefF"] + "'");
                                if (formatoFactura.Codigo != string.Empty)
                                {
                                    if (formatoFactura.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                                }
                                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefF"] + "'");
                                if (formatoFactura.Codigo != string.Empty)
                                {
                                    if (formatoFactura.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=500,WIDTH=700');</script>");
                                }
                            }
                            catch
                            {
                                lbInfo.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                            }
                        }
                        else if (Request.QueryString["subprocess"] == "Cotiza")
                        {
                            formatoFactura = new FormatosDocumentos();
                            try
                            {
                                formatoFactura.Prefijo = Request.QueryString["prefPed"];
                                formatoFactura.Numero = Convert.ToInt32(Request.QueryString["numPed"]);
                                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pPEDIDO WHERE pPED_codigo='" + Request.QueryString["prefPed"] + "'");
                                if (formatoFactura.Codigo != string.Empty)
                                {
                                    if (formatoFactura.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                                }
                                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pPEDIDO WHERE pPED_codigo='" + Request.QueryString["prefF"] + "'");
                                if (formatoFactura.Codigo != string.Empty)
                                {
                                    if (formatoFactura.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                                }
                            }
                            catch
                            {
                                lbInfo.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                            }
                        }
                    }
                }
                try
                {
                    if (Tipo == "P")
                        bind.PutDatasIntoDropDownList(ddlCodigo, "SELECT pped_codigo, pped_nombre concat ' - ' concat pped_codigo FROM ppedido WHERE tped_codigo NOT IN ('T','Q','E') and tped_codiuso IN ('P','M') order by pped_nombre");//Tipo de pedido(no se incluye: Transferencias, Consultas, Einternos, Ademas solo tipo Proveedor y Mixto)
                    else if (Tipo == "M")
                            bind.PutDatasIntoDropDownList(ddlCodigo, "SELECT pped_codigo, pped_nombre FROM ppedido WHERE tped_codigo IN ('N','P','U') and tped_codiuso IN ('C','M') order by pped_nombre");//Tipo de pedido incluye: Normal, Promocion y Urgente.Ademas solo tipo Proveedor y Mixto)
                    else if (Tipo == "C")
                            bind.PutDatasIntoDropDownList(ddlCodigo, "SELECT pped_codigo, pped_nombre FROM ppedido WHERE tped_codiuso IN ('C','M') order by pped_nombre");//Tipo de pedido: Solo uso Cliente y Mixto
                }

                catch
                {
                    lbInfo.Text = "Error al Seleccionar el Tipo de Pedido, revice su configuracion en uso Cliente, Proveedor o Mixto...!";
                }

                if (ddlCodigo.Items.Count > 1)
                    ddlCodigo.Items.Insert(0, "Seleccione..");
                else if (ddlCodigo.Items.Count == 1)
                        txtNumPed.Text = DBFunctions.SingleData("SELECT pped_ultipedi+1 FROM ppedido WHERE pped_codigo = '" + ddlCodigo.SelectedValue + "'");//Se actualiza el numero del pedido
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen, palm_descripcion FROM palmacen where pCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY palm_DESCRIPCION");//Almacen
                if (ddlAlmacen.Items.Count > 0)
                    ddlAlmacen.Items.Insert(0, "Seleccione..");
                else
                {

                }
                llenarVendedores();
                bind.PutDatasIntoDropDownList(ddlPrecios, "SELECT ppre_codigo, ppre_nombre FROM pprecioitem ORDER BY ppre_nombre");//Lista de precios a manejar
                if (ddlPrecios.Items.Count > 1)
                    ddlPrecios.Items.Insert(0, "Seleccione..");

                //bind.PutDatasIntoDropDownList(ddlTipoOrden, "SELECT pdoc_codigo, pdoc_descripcion FROM pdocumento where tdoc_tipodocu IN ('OT','OP') and tvig_vigencia = 'V' ");//Si es transferencia al taller

                //if(DBFunctions.SingleData("SELECT TDOC_TIPODOCU FROM PDOCUMENTO where pDOC_codigo='"+ddlTipoOrden.SelectedValue+"'") == "OT")
                //    bind.PutDatasIntoDropDownList(ddlNumOrden,"Select mord_numeorde, pdoc_codigo concat '-' concat cast(mord_numeorde as character(20)) from MORDEN           where test_estado='A' AND pdoc_codigo='"+ddlTipoOrden.SelectedValue+"' order by mord_numeorde");//Ordenes de Trabajo Taller Abiertas que se les pueda realizar una transferencia de repuestos
                //else
                //    bind.PutDatasIntoDropDownList(ddlNumOrden,"Select mord_numeorde, pdoc_codigo concat '-' concat cast(mord_numeorde as character(20)) from MORDENPRODUCCION where test_estado='A' AND pdoc_codigo='"+ddlTipoOrden.SelectedValue+"' order by mord_numeorde");//Ordenes de Producciòn Abiertas que se les pueda realizar una transferencia de repuestos
                //ddlNumOrden.Items.Insert(0,new ListItem("Seleccione ...","0"));
                //ddlNumOrden.SelectedIndex = 0;

                ddlCargo.Items.Add(new ListItem("Seleccione Cargo para el Pedido", ""));
                bind.PutDatasIntoDropDownList(ddlTipoSugerido, "SELECT tsug_codigo, tsug_nombre FROM tsugerido WHERE tsug_codigo <> 40");
                if (ddlCodigo.SelectedValue != "" && ddlCodigo.SelectedValue != null && ddlCodigo.SelectedValue != "Seleccione..")
                    tipoPedido = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'").Trim();
                Session["tipoPedido"] = tipoPedido;
                Session.Clear();
                dgItems.EditItemIndex = -1;
                BindDatas();
                IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
                tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
                //calDate.SelectedDate = DateTime.Now;
                if (Request.QueryString["pedCre"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha creado el pedido con prefijo " + Request.QueryString["prefPed"] + " y número " + Request.QueryString["numPed"] + "");
                    try
                    {
                        formatoRecibo.Prefijo = Request.QueryString["prefPed"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numPed"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pPEDIDO WHERE pPED_codigo='" + Request.QueryString["prefPed"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                            {
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");

                            }
                        }
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pPEDIDO WHERE pPED_codigo='" + Request.QueryString["prefPed"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                            {
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");

                            }
                        }
                    }
                    catch
                    {
                        lbInfo.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>" + "El recibo puede ser impreso por la opción Impresion<br>";
                    }
                }

                if (Request.QueryString["factGen"] != null)
                    Utils.MostrarAlerta(Response, "Se ha creado la factura con prefijo " + Request.QueryString["prefF"] + " y número " + Request.QueryString["numF"] + "");

                if (Request.QueryString["modificar"] != null)
                {
                    CargarDatosPedido();
                }
            }



            if (ddlCodigo.SelectedValue != "" && ddlCodigo.SelectedValue != null && ddlCodigo.SelectedValue != "Seleccione..")
            {
                lblTipoPedido.Text = DBFunctions.SingleData("SELECT tped_nombre FROM tpedido WHERE tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "')");//Informacion del tipo de pedido
                tipoPedido = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'").Trim();
                Session["tipoPedido"] = tipoPedido;
                if (DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='" + ddlCodigo.SelectedValue + "'") == "T")
                    ddlNumOrden.Visible = ddlTipoOrden.Visible = lblTipoOrden.Visible = lblNumOrden.Visible = lblPlaca.Visible = lblCargo.Visible = ddlCargo.Visible = true;
                else
                    ddlNumOrden.Visible = ddlTipoOrden.Visible = lblTipoOrden.Visible = lblNumOrden.Visible = lblPlaca.Visible = lblCargo.Visible = ddlCargo.Visible = false;

                if (DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='" + ddlCodigo.SelectedValue + "'") == "T" && ddlNumOrden.SelectedIndex > 0)//En caso de que sea un tipo de pedido transferencia al taller
                {
                    actualizarLblPlaca();
                }
            }

            //Revisamos que tipo de pedido si es tipo G(Garantia) o tipo E(interno) desactivamos la lista de precios
            //           tipoPedido = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'").Trim();
            //           Session["tipoPedido"] = tipoPedido;
            hdTipoPed.Value = tipoPedido;

            //if (tipoPedido == "G" || tipoPedido == "E")  hay que dejar la lista para escoger el item
            //    this.ddlPrecios.Enabled = false;
            //else 
            if (tipoPedido == "P")
            {
                lbInfoLeg1.Visible = lbInfoPct.Visible = true;
                lbInfoPct.Text = DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + this.ddlCodigo.SelectedValue + "'") + "%";
            }
            else
            {
                this.ddlPrecios.Enabled = true;
                if (dtInserts.Rows.Count > 0)
                    ddlPrecios.Enabled = true;
            }

            //    txtNIT.Attributes["ondblclick"] = @"ModalDialog(this, 'SELECT mnit_nit as NIT, case when tsoc_sociedad in ('U','P','N','E') THEN mnit_apellidos concat \\' \\' concat COALESCE(mnit_apellido2,\\'\\') 
            //                                                           concat \\' \\' concat mnit_nombres concat \\' \\' concat COALESCE(mnit_nombre2,\\'\\') else mnit_apellidos end as Nombre 
            //                                                           FROM mnit order by nombre', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes


            if (Tipo == "C" && ddlCodigo.SelectedValue != "" && ddlCodigo.SelectedValue != null && ddlCodigo.SelectedValue != "Seleccione..")
            {
                if (tipoPedido == "T")       // LOS NITS DE TRANSFERENCIA SE DEBEN DEFINIR EN LA TABLA DE NITS DE TRANSFERENCIA
                {
                    txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT DISTINCT PNIT.pnital_nittaller as NIT,MNIT.mnit_apellidos CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_apellido2,\\'\\') CONCAT \\' \\' CONCAT MNIT.mnit_nombres CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_nombre2,\\'\\') AS DESCRIPCION FROM dbxschema.mnit MNIT,dbxschema.pnittaller PNIT WHERE MNIT.mnit_nit=PNIT.pnital_nittaller', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                }
                else if (tipoPedido == "E")  // LOS CONSUMOS INTERNOS SE DEBEN DEFINIR EN LA TABLA DE NITS DE CONSUMO INTERNO
                {
                        txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT PNIT.Mnit_nit AS NIT,MNIT.mnit_apellidos CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_apellido2,\\'\\') CONCAT \\' \\' CONCAT MNIT.mnit_nombres CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_nombre2,\\'\\')  AS DESCRIPCION FROM dbxschema.mnit MNIT,dbxschema.MinternoCCOSTO PNIT WHERE MNIT.mnit_nit=PNIT.Mnit_nit', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                }
                else
                {
                        txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_apellidos concat \\' \\' concat COALESCE(mnit_apellido2,\\'\\') concat \\' \\' concat mnit_nombres concat \\' \\' concat COALESCE(mnit_nombre2,\\'\\') as Nombre FROM mnit order by nombre', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                }
                if (Request.QueryString["nitCli"] != null && Request.QueryString["nombreCli"] != null) // && Request.QueryString["idDBCli"] != null
                {
                    string fecha1 = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                    string fecha2 = DateTime.Now.ToString("yyyy-MM-dd");

                    //si esta persona ya le hicieron el Habeas data en un rango de 6 meses a partir de hoy, no es necesario volverle a hacer el Habeas data
                    if (DBFunctions.RecordExist("SELECT MB.MNIT_NIT, PB.PBAS_DESCRIPCION FROM MBASEDATOS MB, PBASEDATOS PB WHERE MB.MNIT_NIT = '" + Request.QueryString["nitCli"] + "' AND PB.TBAS_CODIGO = 'FI' AND MB.PBAS_CODIGO = PB.PBAS_CODIGO AND MBAS_FECHA BETWEEN '" + fecha1 + "' AND '" + fecha2 + "';"))
                    {

                        return;
                    }
                    Session["tipoDB"] = "FI"; // Request.QueryString["idDBCli"];
                    Session["nit"] = Request.QueryString["nitCli"];
                    Session["nombre"] = Request.QueryString["nombreCli"];
                    plcAutorizar.Visible = true;
                    autorizar.Visible = true;
                    plcAutorizar.Controls.Add(LoadControl(pathToControls + "AMS.Tools.AutorizacionCliente.ascx"));
                    string baseNit = DBFunctions.SingleData("select MNIT_NIT from mbasedatos where TBAS_CODIGO  ='" + Session["tipoDB"] + "' AND MNIT_NIT = '" + Session["nit"] + "' ");
                    if (baseNit != "" || (Session["finAutorizar"] != null && IsPostBack))
                    {
                        plcAutorizar.Visible = false;
                        autorizar.Visible = false;
                    }
                }
            }

            valorReal = Request.Form[hdValor.UniqueID];
            nNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM mnit where MNIT_NIT='" + txtNIT.Text + "'");
            if (nNacionalidad == "E")
                Utils.MostrarAlerta(Response, "Este NIT es EXTRANJERO, NO liquida IVA !!!");
        }

        public void buscarNit(object sender, System.EventArgs e)
        {
            txtNITa.Text = DBFunctions.SingleData("select coalesce(NOMBRE,' NO Registrado') from vmnit WHERE mnit_nit='" + this.txtNIT.Text.Trim() + "' ").ToString();
            if (txtNITa.Text == "")
                txtNITa.Text = "Identificación NO registrada";
        }

        protected void cargarListasPrecios(object sender, EventArgs e)
        {
            try
            {   // Vacio el dropdowlist
                ddlPrecios.Items.Clear();
                //cargo la lista de precios asociadas a un NIT
                bind.PutDatasIntoDropDownList(ddlPrecios, "SELECT P.PPRE_CODIGO, P.PPRE_NOMBRE FROM PPRECIOITEM P, RNITPRECIOITEM R WHERE P.PPRE_CODIGO = R.PPRE_CODIGO AND MNIT_NIT = '" + txtNIT.Text + "' ORDER BY ppre_nombre");
                if (ddlPrecios.Items.Count > 0)
                {
                    if (ddlPrecios.Items.Count > 1)
                        ddlPrecios.Items.Insert(0, "Seleccione..");
                }
                else
                { //En caso de que el nit no tenga lista de precios asociada cargo todas las listas de precios existentes.
                    bind.PutDatasIntoDropDownList(ddlPrecios, "SELECT ppre_codigo, ppre_nombre FROM pprecioitem ORDER BY ppre_nombre");//Lista de precios a manejar
                    if (ddlPrecios.Items.Count > 1)
                        ddlPrecios.Items.Insert(0, "Seleccione..");
                }
            }
            catch
            {
                bind.PutDatasIntoDropDownList(ddlPrecios, "SELECT ppre_codigo, ppre_nombre FROM pprecioitem ORDER BY ppre_nombre");//Lista de precios a manejar
                if (ddlPrecios.Items.Count > 1)
                    ddlPrecios.Items.Insert(0, "Seleccione..");
            }

        }

        //Este método carga los items en los dropDownlist de cotizaciones y pedidos.. 
        protected void consultarCotizaciones(Object Sender, EventArgs e)
        {
            if (txtNIT.Text == "" || txtNIT.Text == null)
            {
                Utils.MostrarAlerta(Response, "El NIT es necesario para poder continuar!!...");
                return;
            }
            if (ddlCodigo.SelectedIndex == 0 || ddlAlmacen.SelectedIndex == 0)
            {
                Utils.MostrarAlerta(Response, "El código pedido y almacén son datos necesarios \\n Revise  por favor!!..");
                return;
            }
            codCotizacion = DBFunctions.SingleData("SELECT PPED_CODIGO FROM PPEDIDO WHERE PPED_NOMBRE LIKE 'COTIZACI%'");
            ddlCotiPendt.Items.Clear();

            if (Tipo == "P")
            {
                bind.PutDatasIntoDropDownList(ddlCotiPendt, "SELECT MPED_NUMEPEDI, PPED_CODIGO || ' - ' ||  MPED_NUMEPEDI || '  ' || (DAYS(CURRENT DATE) - DAYS(MPED_CREACION))|| ' Dias' FROM MPEDIDOITEM WHERE MPED_CLASEREGI='C'  AND PPED_CODIGO = '" + codCotizacion + "' ORDER BY MPED_NUMEPEDI desc");
                if (ddlCotiPendt.Items.Count == 0)
                    Utils.MostrarAlerta(Response, "No hay cotizaciones pendientes.");
                else ddlCotiPendt.Items.Insert(0, new ListItem("Seleccione.."));

            }
            else
            {
                ViewState["txtNit"] = txtNIT.Text;
                if (ViewState["transferencia"] != null)
                {
                    ViewState["txtNit"] = DBFunctions.SingleData("SELECT MNIT_NIT FROM MORDEN WHERE pdoc_codigo = '" + ddlTipoOrden.SelectedValue + "' AND mord_numeorde = " + ddlNumOrden.SelectedValue);
                }

                //si es transferencia hay que cargar la cotizacion del N.I.T de la orden
                bind.PutDatasIntoDropDownList(ddlCotiPendt, "SELECT MPED_NUMEPEDI, PPED_CODIGO || ' - ' ||  MPED_NUMEPEDI || '  ' || (DAYS(CURRENT DATE) - DAYS(MPED_CREACION))|| ' Dias' FROM MPEDIDOITEM WHERE MNIT_NIT = '" + ViewState["txtNit"] + "' AND MPED_CLASEREGI='C'  AND PPED_CODIGO = '" + codCotizacion + "' order by MPED_NUMEPEDI desc");
                string nitPed = txtNIT.Text;
                bind.PutDatasIntoDropDownList(ddlPediPendt, "SELECT DISTINCT mpi.MPED_NUMEPEDI, mpi.PPED_CODIGO || ' - ' ||  MPI.MPED_NUMEPEDI || '  ' || (DAYS(CURRENT DATE) - DAYS(MPED_CREACION))|| ' Dias' FROM MPEDIDOITEM MPI, DPEDIDOITEM DPI, PPEDIDO PP WHERE MPI.PPED_CODIGO = DPI.PPED_CODIGO AND MPI.MPED_NUMEPEDI = DPI.MPED_NUMEPEDI AND DPI.DPED_CANTpedi - DPI.DPED_cantasig - dpI.DPED_cantfact > 0 AND MPI.MNIT_NIT IN ('" + ViewState["txtNit"] + "', '" + nitPed + "') AND PP.TPED_CODIGO NOT IN 'C' AND MPI.PPED_CODIGO = PP.PPED_CODIGO order by MPED_NUMEPEDI desc;");
                if (ddlCotiPendt.Items.Count == 0)
                    Utils.MostrarAlerta(Response, "No hay cotizaciones pendientes para este nit");
                else ddlCotiPendt.Items.Insert(0, new ListItem("Seleccione.."));
                if (ddlPediPendt.Items.Count == 0)
                    Utils.MostrarAlerta(Response, "No hay pedidos pendientes para este nit");
                else ddlPediPendt.Items.Insert(0, new ListItem("Seleccione.."));
                bind.PutDatasIntoDropDownList(ddlKits, "SELECT DISTINCT p.pkit_codigo, p.Pkit_CODIGO || ' - ' || pkit_nombre FROM dbxschema.pkit p, dbxschema.mkititem m where p.pkit_codigo = m.mkit_codikit order by p.pkit_codigo;");
                if (ddlKits.Items.Count == 0)
                    Utils.MostrarAlerta(Response, "No hay Kits configurados");
                else ddlKits.Items.Insert(0, new ListItem("Seleccione.."));
            }
        }

        /** carga los items de las cotizaciones en el grid */
        protected void cargarItemsCotizacion(Object Sender, EventArgs e)
        {
            string numPedido = ddlCotiPendt.SelectedValue;
            dtInserts.Clear();
            DataSet dsCoti = new DataSet();

            //CLiente
            if (Tipo == "C")
            {
                //    ddlPediPendt.SelectedIndex = 0;
                DBFunctions.Request(dsCoti, IncludeSchema.NO, @"SELECT dbxschema.EDITARREFERENCIAs(DP.MITE_CODIGO,plin_tipo) as MITE_CODIGO, MI.MITE_NOMBRE, MI.PLIN_CODIGO,  DP.DPED_CANTPEDI as MITE_CANTIDAD,  DP.DPED_VALOUNIT AS mite_precioinicial, DP.DPED_PORCDESC
                                                            FROM DPEDIDOITEM DP, MITEMS MI, PLINEAITEM PL " +
                                                            "WHERE DP.MITE_CODIGO = MI.MITE_CODIGO AND MI.PLIN_CODIGO = PL.PLIN_CODIGO AND DP.MNIT_NIT= '" + ViewState["txtNit"] + "' AND DP.MPED_CLASREGI = 'C' AND DP.PPED_CODIGO='" + codCotizacion + "' AND MPED_NUMEPEDI= '" + numPedido + "';");

                if (dsCoti.Tables[0].Rows.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "Solo se pueden cargar Cotizaciones de Clientes..! ");
                    return;
                }
            }
            else
            {
                DBFunctions.Request(dsCoti, IncludeSchema.NO, @"SELECT dbxschema.EDITARREFERENCIAs(DP.MITE_CODIGO,plin_tipo) as MITE_CODIGO, MI.MITE_NOMBRE, MI.PLIN_CODIGO,  DP.DPED_CANTPEDI as MITE_CANTIDAD,  DP.DPED_VALOUNIT AS mite_precioinicial, DP.DPED_PORCDESC
                                                            FROM DPEDIDOITEM DP, MITEMS MI, PLINEAITEM PL
                                                            WHERE DP.MITE_CODIGO = MI.MITE_CODIGO AND MI.PLIN_CODIGO = PL.PLIN_CODIGO AND DP.MPED_CLASREGI = 'C' AND DP.PPED_CODIGO='" + codCotizacion + "' AND MPED_NUMEPEDI= '" + numPedido + "';");
                if (dsCoti.Tables[0].Rows.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "no hay items para esta cotización");
                    return;
                }
            }

            DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
            year = Convert.ToInt16(cInventarioX.Rows[0]["PANO_ANO"]);
            mes = Convert.ToInt16(cInventarioX.Rows[0]["PMES_MES"]);
            DateTime fechaCal = Convert.ToDateTime(tbDate.Text.ToString());
            string fecha_actual = DateTime.Today.ToString("dd-MM-yyyy");
            if (year != fechaCal.Year || mes != fechaCal.Month)
            {
                Utils.MostrarAlerta(Response, "Fecha NO vigente! " + year + mes + " vs Server " + fecha_actual);
                if(cInventarioX.Rows[0]["MNIT_NIT"].ToString() == "901087944" && HttpContext.Current.User.Identity.Name.ToLower().ToString() == "abarrios")  //EUROTECK Andrea Barrios temporalmente no controla fecha
                { }
                else
                  return;
            }
            int ivm = 1;

            if (!ValidarNIT(ref ivm))
            {
                return;
            }
            else
            {
                for (int i = 0; i < dsCoti.Tables[0].Rows.Count; i++)
                {
                    InsertaItem(dsCoti.Tables[0].Rows[i]["MITE_CODIGO"].ToString(),
                        dsCoti.Tables[0].Rows[i]["PLIN_CODIGO"].ToString(),
                        Convert.ToDouble(dsCoti.Tables[0].Rows[i]["MITE_CANTIDAD"]),
                        Convert.ToDouble(dsCoti.Tables[0].Rows[i]["MITE_PRECIOINICIAL"]),
                        Convert.ToDouble(dsCoti.Tables[0].Rows[i]["DPED_PORCDESC"]),
                        ivm);
                }
                BindDatas();
            }
        }

        /** carga los items de los pedidos en el grid */
        protected void cargarItemsPedido(Object Sender, EventArgs e)
        {
            try
            {
                ddlCotiPendt.SelectedIndex = 0;
            }
            catch (Exception z)
            { }

            string numPedido = ddlPediPendt.SelectedValue;
            dtInserts.Clear();
            DataSet dsPedi = new DataSet();
            DBFunctions.Request(dsPedi, IncludeSchema.NO, @"SELECT dbxschema.EDITARREFERENCIAs(DP.MITE_CODIGO,plin_tipo) as MITE_CODIGO, MI.MITE_NOMBRE, MI.PLIN_CODIGO,  DP.DPED_CANTPEDI - DP.DPED_CANTASIG - DP.DPED_CANTFACT as MITE_CANTIDAD,  DP.DPED_VALOUNIT AS mite_precioinicial, DP.DPED_PORCDESC, MI.TORI_CODIGO
                                                            FROM DPEDIDOITEM DP, MITEMS MI, PLINEAITEM PL
                                                            WHERE DP.MITE_CODIGO = MI.MITE_CODIGO AND MI.PLIN_CODIGO = PL.PLIN_CODIGO AND DP.DPED_CANTPEDI - DP.DPED_CANTASIG - DP.DPED_CANTFACT > 0 AND DP.MPED_CLASREGI = 'C' AND DP.MNIT_NIT= '" + txtNIT.Text + "'  AND MPED_NUMEPEDI= '" + numPedido + "';");

            if (dsPedi.Tables[0].Rows.Count == 0)
                Utils.MostrarAlerta(Response, "Solo se pueden cargar Pedidos de Clientes..! \\n Revise que el NIT no pertenezca a un proveedor!");
            else
            {
                DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
                year = Convert.ToInt16(cInventarioX.Rows[0]["PANO_ANO"]);
                mes = Convert.ToInt16(cInventarioX.Rows[0]["PMES_MES"]);
                //int year = Convert.ToInt16(DBFunctions.SingleData("select PANO_ANO from CINVENTARIO;"));
                //int mes = Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CINVENTARIO;"));
                DateTime fechaCal = Convert.ToDateTime(tbDate.Text.ToString());
                string fecha_actual = DateTime.Today.ToString("dd-MM-yyyy");
                if (year != fechaCal.Year || mes != fechaCal.Month)
                {
                    Utils.MostrarAlerta(Response, "Fecha NO vigente! " + year + mes + " vs Server " + fecha_actual);
                    if (cInventarioX.Rows[0]["MNIT_NIT"].ToString() == "901087944" && HttpContext.Current.User.Identity.Name.ToLower().ToString() == "abarrios")  //EUROTECK temporalmente no controla fecha
                    { }
                    else
                        return;
                }
                int ivm = 1;

                if (!ValidarNIT(ref ivm))
                {
                    return;
                }
                else
                {
                    for (int i = 0; i < dsPedi.Tables[0].Rows.Count; i++)
                    {
                        InsertaItem(dsPedi.Tables[0].Rows[i]["MITE_CODIGO"].ToString(), dsPedi.Tables[0].Rows[i]["PLIN_CODIGO"].ToString(), Convert.ToDouble(dsPedi.Tables[0].Rows[i]["MITE_CANTIDAD"]), Convert.ToDouble(dsPedi.Tables[0].Rows[i]["MITE_PRECIOINICIAL"]), Convert.ToDouble(dsPedi.Tables[0].Rows[i]["DPED_PORCDESC"]), ivm);
                    }
                    BindDatas();
                }
                //Session["dtInsertsCP"] = dsCoti.Tables[0];
                //dgItems.DataSource = dsCoti.Tables[0];
                //dgItems.DataBind();

            }
        }

        protected void cargarKits(Object Sender, EventArgs e)
        {
            try
            {
                ddlCotiPendt.SelectedIndex = 0;
            }
            catch (Exception z)
            {
            }

            if (!DBFunctions.RecordExist("SELECT PKIT_CODIGO FROM MLISTAEMPAQUE"))
            {
                Utils.MostrarAlerta(Response, "NO se ha creado el campo del control de KITS en las lista de Empaque, Llamar a ECAS");
                siKit = false;
                return;
            }
            else siKit = true;
            dtInserts.Clear();
            Session["KIT"] = ddlKits.SelectedValue;
            DataSet dsPedi = new DataSet();
            string porcDesc = DBFunctions.SingleData("SELECT COALESCE(MCLI_PORCDESCINV,0) FROM MCLIENTE WHERE MNIT_NIT = '" + txtNIT.Text + "';");
            if (porcDesc.Length == 0)
                porcDesc = "0.00";
            DBFunctions.Request(dsPedi, IncludeSchema.NO,
                @"SELECT dbxschema.EDITARREFERENCIAs(MI.MITE_CODIGO,plin_tipo) as MITE_CODIGO, MI.MITE_NOMBRE, MI.PLIN_CODIGO,  
		                COALESCE(MG.MIG_CANTIDADUSO,COALESCE(MI.MITE_USOXVEHI,1)) as MITE_CANTIDAD,  
		                COALESCE(MP.MPRE_PRECIO, -1) AS mite_precioinicial, '" + porcDesc + @"' as DPED_PORCDESC, MI.TORI_CODIGO
                   FROM PKIT PK 
	                LEFT JOIN MKITITEM MK ON PK.PKIT_CODIGO = MK.MKIT_CODIKIT
	                LEFT JOIN MITEMS MI ON MK.MKIT_CODITEM = MI.MITE_CODIGO
	                LEFT JOIN PLINEAITEM PL ON MI.PLIN_CODIGO = PL.PLIN_CODIGO
	                LEFT JOIN MPRECIOITEM MP ON MI.MITE_CODIGO = MP.MITE_CODIGO AND MP.PPRE_CODIGO = '" + ddlPrecios.SelectedValue + @"'
	                LEFT JOIN MITEMSGRUPO MG ON MK.MKIT_CODITEM = MG.MITE_CODIGO AND PK.PGRU_GRUPO = MG.PGRU_GRUPO
                    WHERE MK.MKIT_CODIKIT = '" + ddlKits.SelectedValue + "';");

            if (dsPedi.Tables[0].Rows.Count == 0)
                Utils.MostrarAlerta(Response, "Solo se pueden cargar Kits Pedidos de Clientes..! \\n Revise que el KIT esté configurado !!");
            else
            {
                DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
                year = Convert.ToInt16(cInventarioX.Rows[0]["PANO_ANO"]);
                mes = Convert.ToInt16(cInventarioX.Rows[0]["PMES_MES"]);
                //int year = Convert.ToInt16(DBFunctions.SingleData("select PANO_ANO from CINVENTARIO;"));
                //int mes = Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CINVENTARIO;"));
                DateTime fechaCal = Convert.ToDateTime(tbDate.Text.ToString());
                string fecha_actual = DateTime.Today.ToString("dd-MM-yyyy");
                if (year != fechaCal.Year || mes != fechaCal.Month)
                {
                    Utils.MostrarAlerta(Response, "Fecha NO vigente! " + year + mes + " vs Server " + fecha_actual);
                    if (cInventarioX.Rows[0]["MNIT_NIT"].ToString() == "901087944" && HttpContext.Current.User.Identity.Name.ToLower().ToString() == "abarrios")  //EUROTECK temporalmente no controla fecha
                    {
                    }
                    else
                        return;
                }
                int ivm = 1;

                if (!ValidarNIT(ref ivm))
                {
                    return;
                }
                else
                {
                    for (int i = 0; i < dsPedi.Tables[0].Rows.Count; i++)
                    {
                        InsertaItem(dsPedi.Tables[0].Rows[i]["MITE_CODIGO"].ToString(), dsPedi.Tables[0].Rows[i]["PLIN_CODIGO"].ToString(), Convert.ToDouble(dsPedi.Tables[0].Rows[i]["MITE_CANTIDAD"]), Convert.ToDouble(dsPedi.Tables[0].Rows[i]["MITE_PRECIOINICIAL"]), Convert.ToDouble(dsPedi.Tables[0].Rows[i]["DPED_PORCDESC"]), ivm);
                    }
                    BindDatas();
                }
            }
        }

        protected void cambioOT(Object Sender, EventArgs E)
        {
            ddlCargo.Items.Clear();
            if (ddlNumOrden.SelectedValue != "")
            {
                string PD = ddlTipoOrden.SelectedValue;//Prefijo OT
                string ND = ddlNumOrden.SelectedValue;//Numero OT
                string CargoOrden = DBFunctions.SingleData("SELECT TCAR_CARGO FROM MORDEN WHERE PDOC_CODIGO='" + PD + "' AND MORD_NUMEORDE='" + ND + "'");
                if (CargoOrden.Trim() == "P")
                {
                    bind.PutDatasIntoDropDownList(ddlCargo, "SELECT TCAR_CARGO FROM TCARGOORDEN WHERE TCAR_CARGO='P'");
                }
                else
                {
                    bind.PutDatasIntoDropDownList(ddlCargo, "SELECT TCA.tcar_cargo, TCA.tcar_nombre FROM tcargoorden TCA WHERE TCA.tcar_cargo NOT IN (SELECT tcar_cargo FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + PD + "' AND mord_numeorde=" + ND + ") AND TCA.tcar_cargo <> 'X' AND TCA.tcar_cargo <> 'P' order by TCA.tcar_nombre");
                }
                try
                {
                    ddlCargo.SelectedValue = DBFunctions.SingleData("select tcar_cargo from morden as t2 where t2.pdoc_codigo='" + PD + "' and t2.mord_numeorde=" + ND).Trim();
                    TipoCargo = ddlCargo.SelectedValue;
                    //hdTipoCargo.Value = TipoCargo;
                }
                catch { }
                Utils.MostrarAlerta(Response, "Se ha seleccionado la siguiente ORDEN : " + PD + " - " + ddlNumOrden.SelectedValue + "!");
                actualizarLblPlaca();
            }
            else
            {
                ddlCargo.Items.Add(new ListItem("Seleccione una orden de trabajo", ""));
                TipoCargo = "";
                //hdTipoCargo.Value = TipoCargo;
            }
        }

        protected void CambioAlmacen(Object Sender, EventArgs E)
        {
            llenarVendedores();
            llenarPrefijos();
            if (ddlTipoOrden.Items.Count == 1)
            {
                if (tipoPedido == "T")
                {
                    ddlTipoOrden_SelectedIndexChanged(null, null);
                }
            }
        }

        protected void CambioCargoOrden(Object Sender, EventArgs E)
        {
            Utils.MostrarAlerta(Response, "Seleccionó otro Cargo ...Pilas ..! " + ddlCargo.SelectedValue);
            TipoCargo = ddlCargo.SelectedValue;
        }

        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgItems.EditItemIndex = -1;
            BindDatas();
        }

        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            if (dtInserts.Rows.Count > 0)
                dgItems.EditItemIndex = (int)e.Item.ItemIndex;
            BindDatas();
            if (Tipo == "C" && tipoPedido == "P")  // CLIENTES PEDIDO PROMOCION NO PERMITE MODIFICAR EL DESCUENTO  
                ((TextBox)dgItems.Items[dgItems.EditItemIndex].Cells[7].Controls[1]).ReadOnly = false;
        }

        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {	//Este selecciona la fila- Nos permite la edicion de un item agregado
            if (dtInserts.Rows.Count == 0)//Como no hay nada, no se pone a actualizar nada
                return;
            double cant = 0;
            if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))
            {
                Utils.MostrarAlerta(Response, "Cantidad Invalida!");
                dgItems.EditItemIndex = -1;
                BindDatas();
                return;
            }
            //Se debe revisar si el pedido es tipo transferencia se debe restringir el numero de items a que sea menor o igual que la cantidad configurada
            cant = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
            if (tipoPedido == "T")
            {
                string codI = "";
                Referencias.Guardar(dtInserts.Rows[e.Item.DataSetIndex][0].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtInserts.Rows[e.Item.DataSetIndex][11].ToString() + "'"));
                string grupoVehiculo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT MCV.pcat_codigo FROM morden MO, MCATALOGOVEHICULO MCV WHERE MCV.MCAT_VIN = MO.MCAT_VIN AND MO.pdoc_codigo='" + this.ddlTipoOrden.SelectedValue + "' AND MO.mord_numeorde=" + this.ddlNumOrden.SelectedValue + ")");
            }

            if (cant <= 0)//Si la cantidad pedida es menor o igual que cero se le asigna 1
                cant = 1;

            double pr = 0;
            if (Tipo != "P")
                //pr = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][4]); //Precio
                try { pr = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text); }
                catch { pr = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][4]); }

            else if (Tipo == "P")
            {
                //Validamos si el valor digitado es valido o no
                if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text))
                {
                    Utils.MostrarAlerta(Response, "Precio Ingresado Invalido!");
                    dgItems.EditItemIndex = -1;
                    BindDatas();
                    return;
                }
                pr = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);//Precio
            }

            double desc = 0, descM = 0;
            if (Tipo == "M" && tipoPedido == "P") // pedidos cliente y promocion no se permite cambiar el descuento
                desc = 0;
            else
                desc = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);//Porcentaje de descuento ¿Este no se debe de cargar automaticamente?

            if (desc < 0)
                desc = 0;
            if (desc > 100)
                desc = 100;

            if (Tipo == "M")
            {
                if (tipoPedido == "P")
                    descM = 0;  // LOS PEDIDOS PROMOCION NO TIENEN DESCUENTO
                else descM = Convert.ToDouble(DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "';"));
                if (desc > descM)
                {
                    desc = descM;
                    Utils.MostrarAlerta(Response, "El descuento se ha modificado ya que supera el máximo permitido para el cliente!");
                }
            }
            double cantD = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][8]);//Cantidad Disponible
            double cantA = 0;//Cantidad Asignada
            // Para las cotizaciones solo se graba el pedido sin asignacion ni facturacion ni lista de empaque ni back-order en msaldoitems
            // Las cotizaciones deben tener un formato asociado que debe pedir   si o no  imprime los codigos de los productos..
            // En algun momento, una cotizacion se convierte en un caso para facturar, ahi seria aplicar el procedimiento regular de pedidos
            // pero partiendo de la cotizacion y cargandola en la grilla de los pedidos
            if (tipoPedido != "C")
            {
                if (Tipo != "P")
                {
                    if (cant > cantD)
                        cantA = cantD;
                    else
                        cantA = cant;
                }
                else if (Tipo == "P")
                        cantA = cant;
            }
            double iva;
            if (nNacionalidad == "E")
            { 
                iva = 0.00;
                numDecimales = 3;
            }
            else
                iva = Convert.ToDouble(dtInserts.Rows[dgItems.EditItemIndex][5]);//Iva

            double tot = cant * pr;
            if (Tipo != "P" && tipoPedido != "E" && tipoPedido != "G" && tipoPedido != "P")//Pedido Cliente y distinto de Interno, Garantia y Promoción aplico descuento
                tot = tot - Math.Round((desc / 100) * tot, numDecimales);
            else if (Tipo == "P")//Si es proveedor, aplic descuento
                    tot = tot - Math.Round((desc / 100) * tot, numDecimales);
                else
                {
                    Utils.MostrarAlerta(Response, "El tipo de pedido escogido no permite aplicar descuento");
                    desc = 0;
                }
            tot = tot + Math.Round(tot * (iva / 100), numDecimales);
            double totA = cantA * pr;
            totA = totA + Math.Round(totA * (iva / 100), numDecimales);
            if (Tipo != "P" && tipoPedido != "E" && tipoPedido != "G" && tipoPedido != "P")
                totA = totA - Math.Round((desc / 100) * totA, numDecimales);
            dtInserts.Rows[dgItems.EditItemIndex][2] = cant;
            dtInserts.Rows[dgItems.EditItemIndex][3] = cantA;
            dtInserts.Rows[dgItems.EditItemIndex][4] = pr;
            dtInserts.Rows[dgItems.EditItemIndex][6] = desc;
            dtInserts.Rows[dgItems.EditItemIndex][7] = tot;
            dtInserts.Rows[dgItems.EditItemIndex][8] = cantD;
            dtInserts.Rows[dgItems.EditItemIndex][9] = totA;
            dtInserts.Rows[dgItems.EditItemIndex][10] = pr;
            if (Tipo == "P")
                dtInserts.Rows[dgItems.EditItemIndex][4] = pr;
            dgItems.EditItemIndex = -1;
            BindDatas();
        }

        protected void DgInserts_AddAndDel(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "ClearRows")
            {
                dtInserts.Rows.Clear();

            }
            else if (((Button)e.CommandSource).CommandName == "AddDatasRow")
            {
                if (CheckValues(e))
                {
                    int ivm = 1;
                    double cant = 0;
                    double prec = 0;
                    double desc = 0, descM = 0;

                    //Advertir saldo en mora
                    if (dtInserts.Rows.Count == 0)
                    {
                        try
                        {
                            if (Convert.ToDouble(txtSaldoMoraCartera.Text) > 0)
                                Utils.MostrarAlerta(Response, "Advertencia: existe saldo en mora!");
                        }
                        catch { }
                        try
                        {
                            if (Convert.ToDouble(txtCupo.Text) > 0 && Convert.ToDouble(txtSaldoCartera.Text) > Convert.ToDouble(txtCupo.Text))
                                Utils.MostrarAlerta(Response, "Advertencia: el saldo en cartera supera el cupo!");
                        }
                        catch { }
                    }
                    if (tbDate.Enabled)
                    {
                        DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
                        year = Convert.ToInt16(cInventarioX.Rows[0]["PANO_ANO"]);
                        mes = Convert.ToInt16(cInventarioX.Rows[0]["PMES_MES"]);
                        //ano = Convert.ToInt16(DBFunctions.SingleData("select PANO_ANO from CINVENTARIO;"));
                        //mes = Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CINVENTARIO;"));
                        DateTime fechaCal = Convert.ToDateTime(tbDate.Text.ToString());
                        string fecha_actual = DateTime.Today.ToString("dd-MM-yyyy");

                        //if (ano != calDate.SelectedDate.Year || mes != calDate.SelectedDate.Month)
                        if (year != fechaCal.Year || mes != fechaCal.Month)
                        {
                            Utils.MostrarAlerta(Response, "Fecha NO vigente! " + year + mes + " vs Server " + fecha_actual);
                            if (cInventarioX.Rows[0]["MNIT_NIT"].ToString() == "901087944" && HttpContext.Current.User.Identity.Name.ToLower().ToString() == "abarrios")  //EUROTECK temporalmente no controla fecha
                            { }
                            else
                                return;
                        }
                    }

                    if (!ValidarNIT(ref ivm))
                        return;

                    if (dtInserts.Rows.Count == 0)
                    {
                        if (!ValidarDatosIniciales())
                            return;
                    }

                    if (tipoPedido != "T")
                        cant = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
                    else
                    {
                        double cantidadIngresada = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
                        string codI = "";
                        Referencias.Guardar(((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim(), ref codI, (((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]);
                        if (!Referencias.RevisionSustitucion(ref codI, (((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[1]))
                        {
                            Utils.MostrarAlerta(Response, "El codigo " + ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim() + " NO SE encuentra registrado.\\nRevise Por Favor!");
                            return;
                        }
                        string grupoVehiculo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT MCV.pcat_codigo FROM morden MO, MCATALOGOVEHICULO MCV WHERE MCV.MCAT_VIN = MO.MCAT_VIN AND MO.pdoc_codigo='" + this.ddlTipoOrden.SelectedValue + "' AND mord_numeorde=" + this.ddlNumOrden.SelectedValue + ")");
                        /*if(DBFunctions.SingleData("SELECT mig_cantidaduso FROM mitemsgrupo WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+grupoVehiculo+"'")=="")
                            cant = Convert.ToDouble(DBFunctions.SingleData("SELECT mite_usoxvehi FROM mitems WHERE mite_codigo='"+codI+"'"));
                        else
                            cant = Convert.ToDouble(DBFunctions.SingleData("SELECT mig_cantidaduso FROM mitemsgrupo WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+grupoVehiculo+"'"));*/
                        /*cant=Convert.ToDouble(DBFunctions.SingleData("SELECT DBXSCHEMA.USOXVEH(mite_codigo,'','"+grupoVehiculo+"') FROM dbxschema.mitems WHERE mite_codigo='"+codI+"'"));
                        if(cantidadIngresada < cant)
                            cant = cantidadIngresada;*/
                        cant = cantidadIngresada;

                    }
                    if (Tipo == "C")  // cliente mostrador si puede dar descuento en pedidos promocion
                    {
                        prec = Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text);
                        desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text);
                        if (desc < 0 || desc > 100)
                        {
                            desc = 0;
                        }
                        double demandaPromedio = Convert.ToDouble(DBFunctions.SingleData("select COALESCE(ROUND(avg(dped_cantpedi)+0.49,0), 0) from dpedidoitem where mped_clasregI = 'C' AND MITE_CODIGO = '" + ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim() + "';"));
                        if (cant > demandaPromedio && demandaPromedio > 0)
                            Utils.MostrarAlerta(Response, "La cantidad pedida para el item " + ((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim() + " supera la Demanda Promedio.\\nRevise Por Favor!");
                    }
                    else if (Tipo == "P")
                    {
                        prec = Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text);
                        desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text);
                        if (desc < 0 || desc > 100)
                        {
                            desc = 0;
                        }
                    }
                    else if (Tipo == "M")
                    {
                        if (tipoPedido == "P")
                        {
                            desc = 0;
                            descM = 0;
                        }
                        else
                        {
                            descM = Convert.ToDouble(MyCoalesce(DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "';"), "0"));
                            desc = Convert.ToDouble(((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text);
                        }
                        //		prec=Convert.ToDouble(MyCoalesce(DBFunctions.SingleData("SELECT COALESCE(mpre_precio,0) FROM mprecioitem where mite_codigo='"+((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim()+"' AND ppre_codigo='"+ddlPrecios.SelectedValue+"';"),"0"));  // Comparar contra la referencia  SIN EDITAR
                        prec = Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text);
                        if (desc > descM)
                        {
                            desc = descM;
                            Utils.MostrarAlerta(Response, "El descuento se ha modificado ya que supera el máximo permitido para el cliente!");
                        }
                    }
                    if (prec <= 0)
                        return;
                    else
                        InsertaItem(((TextBox)e.Item.Cells[0].Controls[1]).Text.Trim(), (((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Split('-'))[0], cant, prec, desc, ivm);//Se le pasa el codigo, cantidad solicitada,precio,descuento y un indicativo si es una solicitud de taller
                }
                else
                    Utils.MostrarAlerta(Response, "Algun valor no es válido para la inserción o el precio es cero!");

                ((TextBox)e.Item.Cells[6].FindControl("valToInsert1")).Attributes.Add("focus", "this.focus();");
            }

            BindDatas();
            //if (ViewState["precioMenor"] == null)
            //    BindDatas();
            //else
            //    Session["dtInsertsCP"] = dtInserts;
        }

        private string MyCoalesce(string InPut, string optionalOutPut)
        {
            if (InPut.Trim() != "")
            {
                return InPut.Trim();
            }
            else
            {
                if (optionalOutPut.Trim() != "")
                {
                    return optionalOutPut.Trim();
                }
                else
                {
                    return "";
                }
            }
        }

        protected void DgInserts_Delete(Object sender, DataGridCommandEventArgs e)
        {
            if (Request.QueryString["modificar"] != null)
            {
                ArrayList itemsEliminar = new ArrayList();
                if (Session["itemsEliminar"] != null)
                {
                    itemsEliminar = (ArrayList)Session["itemsEliminar"];
                }
                itemsEliminar.Add(dsDetallePedido.Tables[0].Rows[e.Item.ItemIndex][0] + "@" + dsDetallePedido.Tables[0].Rows[e.Item.ItemIndex][3] + "@" + dsDetallePedido.Tables[0].Rows[e.Item.ItemIndex][4]);
                Session["itemsEliminar"] = itemsEliminar;

                try
                {
                    dsDetallePedido.Tables[0].Rows.Remove(dsDetallePedido.Tables[0].Rows[e.Item.ItemIndex]);
                    dgItems.EditItemIndex = -1;
                }
                catch { };

                dgItems.DataSource = dsDetallePedido;
                dgItems.DataBind();
                Session["dsDetallePedido"] = dsDetallePedido;
            }
            else
            {
                try
                {
                    dtInserts.Rows.Remove(dtInserts.Rows[e.Item.ItemIndex]);
                    dgItems.EditItemIndex = -1;
                }
                catch { };
                BindDatas();
            }
        }

        //AGREGAR SUGERIDOS
        protected void AgregSug(Object Sender, EventArgs E)
        {
            //Ahora traemos los items que se encuentren en el sugerido del respectivo nit seleccionado
            if (txtNIT.Text != "")
            {
                DataSet da = new DataSet();
                DBFunctions.Request(da, IncludeSchema.NO, "SELECT DBXSCHEMA.EDITARREFERENCIAS(MSI.mite_codigo,PLIN.plin_tipo), MIT.plin_codigo,MSI.msuge_cantidad FROM mitems MIT, msugeridoitem MSI, plineaitem PLIN WHERE MSI.mite_codigo = MIT.mite_codigo AND MIT.mnit_nit = '" + txtNIT.Text.Trim() + "' AND MSI.tsug_codigo = " + ddlTipoSugerido.SelectedValue + " AND MIT.plin_codigo=PLIN.plin_codigo");
                int ivm = 1;
                if (!ValidarNIT(ref ivm))
                    return;
                for (int n = 0; n < da.Tables[0].Rows.Count; n++)
                    InsertaItem(da.Tables[0].Rows[n][0].ToString(), da.Tables[0].Rows[n][1].ToString(), Convert.ToDouble(da.Tables[0].Rows[n][2]), ivm);
                BindDatas();
            }
            else
                Utils.MostrarAlerta(Response, "No se ha seleccionado un item!");
        }

        //REALIZA PEDIDO
        protected void NewAjust(Object Sender, EventArgs E)
        {
            if (!ValidarDatos())
                return;

            string mensaje = (string)ViewState["citasConKitsProgramados"];
            if (mensaje != "")
            {
                Utils.MostrarAlerta(Response, "{0}");
            }

            string PD = "", ND = ""; //Prefijo y Numero de OT (en caso que sea una transferencia a taller)
            uint numD = 0;

            string tp = DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='" + ddlCodigo.SelectedValue + "'");

            try
            {
                if (tp == "T")
                {
                    PD = ddlTipoOrden.SelectedValue;
                    ND = ddlNumOrden.SelectedValue;
                    if (this.ddlNumOrden.SelectedIndex == 0)
                    {
                        Utils.MostrarAlerta(Response, "Debe Seleccionar una orden para la transferencia!");
                        return;
                    }
                    if (DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + PD + "' ") == "OT")
                    {
                        if (DBFunctions.SingleData("SELECT test_estado FROM MORDEN WHERE pdoc_codigo='" + PD + "' AND MORD_NUMEORDE=" + ND + " ") != "A")
                        {
                            Utils.MostrarAlerta(Response, "El tipo, numero o estado de la Orden de Trabajo NO es valido!");
                            return;
                        }
                    }
                    else if (DBFunctions.SingleData("SELECT test_estado FROM MORDENPRODUCCION WHERE pdoc_codigo='" + PD + "' AND MORD_NUMEORDE=" + ND + " ") != "A")
                    {
                            Utils.MostrarAlerta(Response, "El tipo, numero o estado de la Orden de Produccion NO es valido!");
                            return;
                    }

                    try { numD = Convert.ToUInt32(ND); }
                    catch { };
                }
            }
            catch (Exception ex) { lbInfo.Text = ex.ToString(); }

            //Constructor Tipo 2 Solo Pedido
            PedidoFactura pedfac = new PedidoFactura(
                tp,                       // 1 Tipo de Pedido
                ddlCodigo.SelectedValue,  // 2 Prefijo Documento
                txtNIT.Text,              // 3 Nit
                ddlAlmacen.SelectedValue, // Almacén
                this.ddlVendedor.SelectedValue, // 5 Vendedor
                Convert.ToUInt32(txtNumPed.Text), // 6 Numero Pedido
                PD,                       // 7 Prefijo OT
                numD,                     // 8 Número OT
                Tipo,                     // 9 Tipo de Pedido
                ddlCargo.SelectedValue,   // 10 Cargo
                Convert.ToDateTime(tbDate.Text.ToString()), //; calDate.SelectedDate,   // 11 Fecha
                txtObs.Text,               // 12 Observaciones
                ddlKits.SelectedValue.ToString()  // kit seleccionado si aplicó
                );

            int n;
            for (n = 0; n < dtInserts.Rows.Count; n++) //Se agregan las filas que detallan el pedido
            {
                string codI = "";
                Referencias.Guardar(dtInserts.Rows[n][0].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtInserts.Rows[n][11].ToString() + "'"));
                pedfac.InsertaFila(
                    codI,                                   // Código de Item
                    0,                                      // Cantidad Facturada
                    Convert.ToDouble(dtInserts.Rows[n][4]), // Precio
                    Convert.ToDouble(dtInserts.Rows[n][5]), // Porcentaje IVA
                    Convert.ToDouble(dtInserts.Rows[n][6]), // Porcentaje Descuento
                    Convert.ToDouble(dtInserts.Rows[n][2]),  // Cantidad Pedida
                    "", // Codigo del Pedido
                    ""  // Numero del pedido
                    );
            }
            bool status = true;
            /*
            if(tp=="C")
            {
                pedfac.Numerolista = "0";
            }
            */
            facRealizado = pedfac.RealizarPed(ref status, true);
            ViewState["codFac"] = pedfac.Coddocumento;
            ViewState["numFac"] = pedfac.Numpedido;

            if (status)
            {
                string valor = "";
                if (Session["btnRegresarActivo"] != null)
                {
                    valor = Session["btnRegresarActivo"].ToString();
                }
                Session.Clear();
                if (!procesoCombinado)
                {
                    string nitCli = txtNIT.Text;
                    string nombreCli = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + nitCli + "';");
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Session["btnRegresarActivo"] = valor == "" ? null : valor;
                    if (tp == "C")
                        Response.Redirect("" + indexPage + "?process=Inventarios.CrearPedido&path=Crear Pedido" + "&actor=" + Tipo + "&numLis=" + pedfac.Numerolista + "&subprocess=Cotiza&pedCre=0&prefPed=" + pedfac.Coddocumento + "&numPed=" + pedfac.Numpedido + "&nitCli=" + nitCli + "&nombreCli=" + nombreCli);
                    else
                        Response.Redirect("" + indexPage + "?process=Inventarios.CrearPedido&path=Crear Pedido" + "&actor=" + Tipo + "&numLis=" + pedfac.Numerolista + "&pedCre=0&prefPed=" + pedfac.Coddocumento + "&numPed=" + pedfac.Numpedido + "&nitCli=" + nitCli + "&nombreCli=" + nombreCli);
                    lbInfo.Text += pedfac.ProcessMsg;
                }
            }
            else
                lbInfo.Text += pedfac.ProcessMsg;
            txtNumPed.Text = DBFunctions.SingleData("SELECT pped_ultipedi+1 FROM ppedido WHERE pped_codigo = '" + ddlCodigo.SelectedValue + "'");
        }

        //REALIZAR PEDIDO Y FACTURAR
        protected void NewAjustFac(Object Sender, EventArgs E)
        {
            procesoCombinado = true;

            string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

            if (!ValidarDatos())
                return;

            //try
            //{
            //    if (!DBFunctions.RecordExist("SELECT PKIT_CODIGO FROM MLISTAEMPAQUE"))
            //    {
            //        Utils.MostrarAlerta(Response, "NO se ha creado el campo del control de KITS en las lista de Empaque ni la tabla DFACTURAITEMKIT, Llamar a ECAS");
            //        return;
            //    }
            //}
            //catch
            //{
            //    Utils.MostrarAlerta(Response, "NO se ha creado el campo del control de KITS en las lista de Empaque ni la tabla DFACTURAITEMKIT, Llamar a ECAS");
            //    return;

            //}

            NewAjust(Sender, E);

            if (!facRealizado)
            {
                BorrarGrilla();
                Utils.MostrarAlerta(Response, "El pedido se creó. No se pudo facturar debido a que no hay items disponibles !");
                return;
            }

            Session.Clear();

  


            string numLis = DBFunctions.SingleData("SELECT MAX(MLIS_NUMERO) FROM MLISTAEMPAQUE WHERE MNIT_NIT='" + txtNIT.Text + "'");
            string TipoR = Tipo;
            if (TipoR != "P") TipoR = "C";

            string kit = ddlKits.SelectedValue.ToString();
            //COMENTADO POR LA REDIRECCION
            //string nitCli = txtNIT.Text;
            //string nombreCli = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + nitCli + "';");
            Response.Redirect("" + indexPage + "?process=Inventarios.Facturacion&path=Facturacion&nped=" + numLis + "&actor=" + TipoR + "&orig=Inventarios.CrearPedido&prefPed=" + ViewState["codFac"] + "&numPed=" + ViewState["numFac"] + "&kitPed=" + kit + "&AnoA="+year+" + &almacen=" + ddlAlmacen.SelectedValue +" ");
        }

        protected void CambioPrefOT(Object Sender, EventArgs E)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlNumOrden, "Select mord_numeorde, pdoc_codigo concat '-' concat cast(mord_numeorde as character(20)) from MORDEN where test_estado='A' AND pdoc_codigo='" + ddlTipoOrden.SelectedValue + "' order by mord_numeorde");//Ordenes de trabajo Abiertas que se les pueda realizar una transferencia de repuestos			
        }

        protected void ChangeDate(Object Sender, EventArgs E)
        {
            if (dtInserts.Rows.Count == 0)
                tbDate.Text = calDate.SelectedDate.GetDateTimeFormats()[6];
        }

        /*
        Cargar excel 
        */
        protected void btnCargar_Click1(object sender, System.EventArgs e)
        {

            bool validaDatosIniciales = ValidarDatosIniciales();
            if (validaDatosIniciales == false)
            {
                return;
            }
            DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
            year = Convert.ToInt16(cInventarioX.Rows[0]["PANO_ANO"]);
            mes = Convert.ToInt16(cInventarioX.Rows[0]["PMES_MES"]);
            //int year = Convert.ToInt16(DBFunctions.SingleData("select PANO_ANO from CINVENTARIO;"));
            //int mes = Convert.ToInt16(DBFunctions.SingleData("select PMES_MES from CINVENTARIO;"));
            DateTime fechaCal = Convert.ToDateTime(tbDate.Text.ToString());
            string fecha_actual = DateTime.Today.ToString("dd-MM-yyyy");
            if (year != fechaCal.Year || mes != fechaCal.Month)
            {
                Utils.MostrarAlerta(Response, "Fecha NO vigente! " + year + mes + " vs Server " + fecha_actual);
                if (cInventarioX.Rows[0]["MNIT_NIT"].ToString() == "901087944" && HttpContext.Current.User.Identity.Name.ToLower().ToString() == "abarrios")  //EUROTECK temporalmente no controla fecha
                { }
                else
                    return;
            }
            int ivm = 1;

            if (!ValidarNIT(ref ivm))
            {
                return;
            }
        //    ValidarDatos();
            Random num = new Random();
            DataSet dsItem = new DataSet();
            if (flArchivoExcel.PostedFile.FileName.ToString() == string.Empty)
            {
                Utils.MostrarAlerta(Response, "No ha seleccionado un archivo'");
                return;
            }
            else
            {
                int filaExcel = 0;
                try
                {
                    string[] file = flArchivoExcel.PostedFile.FileName.ToString().Split('\\');
                    string fileName = file[file.Length - 1];
                    string[] fileNameParts = fileName.Split('.');
                    string fileExtension = fileNameParts[fileNameParts.Length - 1];

                    if (fileExtension.ToUpper() != "XLS" && fileExtension.ToUpper() != "XLSX")
                    {
                        Utils.MostrarAlerta(Response, "No es un archivo de Excel");
                        return;
                    }

                    else
                    {
                        int numero = num.Next(0, 9999);
                        flArchivoExcel.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileName.Split('.')[0] + "_" + numero + "." + fileName.Split('.')[1]);
                        ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileName.Split('.')[0] + "_" + numero + "." + fileName.Split('.')[1]);
                        bool leiArchivo = false;
                        try
                        {
                            exc.Request(dsItem, IncludeSchema.NO, "SELECT * FROM ITEM");
                            if (dsItem.Tables[0].Rows.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontró ninguna tabla, verifique que haya seguido adecuadamente los pasos");
                                return;
                            }
                            else
                            {

                                for (int i = 0; i < dsItem.Tables[0].Columns.Count; i++)
                                {
                                    dsItem.Tables[0].Columns[i].ColumnName = dsItem.Tables[0].Rows[0][i].ToString().Trim();
                                }
                                dsItem.Tables[0].Rows[0].Delete();
                                dsItem.Tables[0].Rows[0].AcceptChanges();//cierra la edición por lo que reajusta la tabla, sin esto, la tabla se no se reorganiza.
                                leiArchivo = true;
                            }

                        }
                        catch (Exception z)
                        {
                            Utils.MostrarAlerta(Response, z.Message + "- No se pudo leer ningún registro en el archivo de Excel");
                            return;
                        }

                        if (leiArchivo)
                        {

                            if (dsItem.Tables[0].Rows.Count == 0)
                            {
                                Utils.MostrarAlerta(Response, "No se encontro ningún registro en el archivo de Excel");
                                return;
                            }
                            else
                            {
                                dtInserts.Clear();
                                lbError.Text = "";
                                string bodegaItem, codEditado;
                                DataSet dsDato = new DataSet();
                                DBFunctions.Request(dsDato, IncludeSchema.NO, "SELECT PLIN_CODIGO, PLIN_TIPO FROM PLINEAITEM; SELECT MITE_CODIGO FROM mitems");
                          //      DataSet dsTodoItems = new DataSet();
                          //      DBFunctions.Request(dsTodoItems, IncludeSchema.NO, "select mt.mite_codigo, mt.mite_nombre, mt.piva_porciva, mt.plin_codigo, pd.pdes_porcdesc, mt.tori_codigo, ce.mnit_nit from cempresa ce, mitems mt, pdescuentoitem pd where pd.pdes_codigo=mt.pdes_codigo; ");
                                //          DataRow cinventario = dsTodoItems.Tables[1].Rows[0];
                                for (int i = 0; i < dsItem.Tables[0].Rows.Count; i++)
                                {
                                    bodegaItem = dsDato.Tables[0].Select("PLIN_CODIGO='" + dsItem.Tables[0].Rows[i]["LINEA"].ToString() + "'")[0].ItemArray[1].ToString(); //= DBFunctions.SingleData("SELECT PLIN_TIPO FROM PLINEAITEM WHERE PLIN_CODIGO='" + dsItem.Tables[0].Rows[i]["LINEA"].ToString() + "'");
                                    filaExcel = i;
                                    codEditado = "";

                                    //por si es un item de tipo mazda o ford o alguno de esos
                                    //Tools.Referencias.Editar(dsItem.Tables[0].Rows[i][0].ToString(), ref codEditado, bodegaItem);
                                    Tools.Referencias.Guardar(dsItem.Tables[0].Rows[i][0].ToString().Trim(), ref codEditado, bodegaItem);
                                    if (dsDato.Tables[1].Select("MITE_CODIGO = '" + codEditado + "'").Length > 0)//DBFunctions.RecordExist("SELECT mite_codigo as item FROM mitems WHERE mite_codigo = '" + codEditado + "'"))
                                    {
                                        if (dsDato.Tables[0].Select("PLIN_CODIGO = '" + dsItem.Tables[0].Rows[i]["LINEA"].ToString() + "'").Length > 0)//DBFunctions.RecordExist("SELECT PLIN_CODIGO AS NOMBRE FROM PLINEAITEM WHERE PLIN_CODIGO = '" + dsItem.Tables[0].Rows[i]["LINEA"].ToString() + "'"))
                                        {
                                            double n;
                                            //if (int.TryParse(dsItem.Tables[0].Rows[i]["PRECIO_INICIAL"].ToString(), out n))
                                            //{
                                            //    //DataRow dr = dtInserts.NewRow();
                                            //    //dr["mite_codigo"] = dsItem.Tables[0].Rows[i][0].ToString();
                                            //    //dr["mite_nombre"] = dsItem.Tables[0].Rows[i][1].ToString();
                                            //    //dr["plin_codigo"] = dsItem.Tables[0].Rows[i][2].ToString();
                                            //    //dr["mite_cantidad"] = dsItem.Tables[0].Rows[i][3].ToString();
                                            //    //dr["mite_precioinicial"] = dsItem.Tables[0].Rows[i][4].ToString();
                                            //    //dr["mite_iva"] = dsItem.Tables[0].Rows[i][5].ToString();
                                            //    //dr["mite_desc"] = dsItem.Tables[0].Rows[i][6].ToString();
                                            //    //dtInserts.Rows.Add(dr);
                                            //    //InsertaItem(dsItem.Tables[0].Rows[i]["MITE_CODIGO"].ToString(), dsItem.Tables[0].Rows[i]["PLIN_CODIGO"].ToString(), Convert.ToDouble(dsItem.Tables[0].Rows[i]["MITE_CANTIDAD"]), Convert.ToDouble(dsItem.Tables[0].Rows[i]["MITE_PRECIOINICIAL"]), Convert.ToDouble(dsItem.Tables[0].Rows[i]["mite_DESC"]), ivm);
                                            //    filaExcel = i;
                                            //}
                                            if (double.TryParse(dsItem.Tables[0].Rows[i]["PRECIO_INICIAL"].ToString(), out n))
                                            {
                                                InsertaItem(dsItem.Tables[0].Rows[i]["CODIGO"].ToString(), dsItem.Tables[0].Rows[i]["LINEA"].ToString(), Convert.ToDouble(dsItem.Tables[0].Rows[i]["CANTIDAD"]), Convert.ToDouble(dsItem.Tables[0].Rows[i]["PRECIO_INICIAL"]), Convert.ToDouble(dsItem.Tables[0].Rows[i]["DESCUENTO"]), ivm);

                                            }
                                            else
                                            {
                                                //lbError.Text += "No se agregó la cuenta " + dsNota.Tables[0].Rows[i][0].ToString() + " porque el valor [ " + dsNota.Tables[0].Rows[i][1].ToString() + " ] está mal escrito. Verifique que sólo contenga números. <br />";
                                                lbError.Text += "<br /> No se agregó el Item " + dsItem.Tables[0].Rows[i][0].ToString() + " porque el valor está mal escrito. Verifique que sólo contenga números. fila Excel: " + filaExcel;
                                            }
                                        }
                                        else
                                        {
                                            lbError.Text += "<br /> No se agregó el Item " + dsItem.Tables[0].Rows[i][0].ToString() + " porque la Linea no existe. Verifique por favor. fila Excel: " + filaExcel;
                                        }

                                    }
                                    else
                                    {
                                        lbError.Text += "<br /> No se agregó el Item [ " + dsItem.Tables[0].Rows[i][0].ToString() + " ]  porque no existe, Por favor verifique. fila Excel: " + filaExcel;
                                    }
                                }
                            }
                            BindDatas();
                            //dgItems.DataSource = dtInserts;
                            //dgItems.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Page.RegisterClientScriptBlock("status", ex.ToString());
                    lbError.Text += "<br /> " + ex.Message + "FILA EXCEL: " + filaExcel;
                }
            }
        }

        protected void DgInsertsDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
                Boolean autoAgregar = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AutoAgregar"]);
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Remove("OnBlur");
                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("OnBlur", "ConsultarInfoReferencia(this,'" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "','" + ((TextBox)e.Item.Cells[1].Controls[1]).ClientID + "','" + Request.QueryString["actor"] + "','" + ddlCodigo.ClientID + "','" + ((TextBox)e.Item.Cells[3].Controls[1]).ClientID + "','" + ddlTipoOrden.ClientID + "','" + ddlNumOrden.ClientID + "','" + hdTipoPed.ClientID + "','" + ddlPrecios.ClientID + "','" + ddlAlmacen.ClientID + "','" + ((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).ClientID + "','" + ((Label)e.Item.Cells[6].FindControl("lbPrecMin")).ClientID + "','" + ddlCodigo.ClientID + "','" + ddlCargo.ClientID + "','" + ((Button)e.Item.Cells[10].FindControl("btnAdd")).ClientID + "','" + autoAgregar + "','" + ((Label)e.Item.Cells[6].FindControl("lbCantActual")).ClientID + "');");
                //                                                                                      (objSender,                   idDDLLinea,                                                   idObjNombre,                                           tipoCliente,					idDDLTipoPedido,                  idObjCantidad,									idDDLOTPref,			        	idDDLOTNum,				    idhdTipPed,						idDDLLista,			         idhdTipoCarg                                     idDDLalmacen,				                                      	idobjValor													idlbPre			    	idPrefPed		              botonAgergar                                                                                                               idobjValor))

                if (ViewState["linea"] != null)
                    ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedIndex = (int)ViewState["linea"];

                txtNIT.Attributes.Remove("OnBlur");
                txtNIT.Attributes.Add("OnBlur", "ConsultasClienteNit(this,'" + ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).ClientID + "','" + txtNITa.ClientID + "','" + hdDescCli.ClientID + "','" + Request.QueryString["actor"] + "','" + txtCupo.ClientID + "','" + txtSaldoCartera.ClientID + "','" + txtSaldoMoraCartera.ClientID + "');");
                nNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM mnit where MNIT_NIT='" + txtNIT.Text + "'");
                //    if (nNacionalidad == "E")
                //         Response.Write("<script language='javascript'>alert('Este NIT es EXTRANJERO, NO liquida IVA !!!');</script>");

                if (hdDescCli.Value == "")
                    ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = "0";
                else
                    ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = hdDescCli.Value;
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]), "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");

                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onkeyup", "CargaNIT2(this," + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + ");");
                try
                {
                    ((System.Web.UI.WebControls.Image)e.Item.Cells[0].Controls[2]).Attributes.Add("onClick", "CargaNIT2(" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "," + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + ");");
                }
                catch
                {
                }

                if (Tipo == "P")
                {
                    ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("ondblclick", "CargaNITP('" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "','" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "','" + cInventarioX.Rows[0]["PANO_ANO"] + "');");
                    ((System.Web.UI.WebControls.Image)e.Item.Cells[0].FindControl("imglupa2")).Attributes.Add("onClick", "CargaNITP('" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "','" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "','" + cInventarioX.Rows[0]["PANO_ANO"] + "');");
                }
                else
                {
                    ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("ondblclick", "CargaNIT('" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "','" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "','" + cInventarioX.Rows[0]["PANO_ANO"] + "','" + ddlPrecios.ClientID + "');");
                    try
                    {
                        ((System.Web.UI.WebControls.Image)e.Item.Cells[0].FindControl("imglupa2")).Attributes.Add("onClick", "CargaNIT('" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "','" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "','" + cInventarioX.Rows[0]["PANO_ANO"] + "','" + ddlPrecios.ClientID + "');");
                    }
                    catch
                    {
                    }
                }
                ((DropDownList)e.Item.Cells[2].Controls[1]).Attributes.Add("onchange", "ChangeLine(document.getElementById('" + ((DropDownList)e.Item.Cells[2].Controls[1]).ClientID + "'),document.getElementById('" + ((TextBox)e.Item.Cells[0].Controls[1]).ClientID + "'));");
                if (Tipo == "P")
                {
                    ((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Visible = true;
                }
               //Se va agregar al boton de reiniciar un confirm de javascript para el boton de reiniciar

               ((Button)e.Item.Cells[10].FindControl("btnClear")).Attributes.Add("onclick", "cargando = false; return confirm('Esta seguro de reiniciar la información del pedido?');");


                if (Tipo == "M")
                {
                    if (ViewState["CASA_MATRIZ"].ToString() == "S")
                    {
                        ((TextBox)e.Item.Cells[7].FindControl("edit_precioc")).Attributes.Add("readonly", "readonly");
                        //((TextBox)e.Item.Cells[7].FindControl("edit_precioc")).Attributes.Remove("readonly");
                        //((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Attributes.Add("readonly","readonly");
                    }
                    try
                    {
                        if (tipoPedido == "P")
                            ((TextBox)e.Item.Cells[9].FindControl("tbfdesc")).Text = "0";
                        else ((TextBox)e.Item.Cells[9].FindControl("tbfdesc")).Text = Convert.ToDouble(DBFunctions.SingleData(
                            "SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "';"
                            )).ToString();
                    }
                    catch
                    {
                        ((TextBox)e.Item.Cells[8].FindControl("tbfdesc")).Text = "0";
                    }
                }
                if (cInventarioX.Rows[0]["CINV_MODIPREC"].ToString() == "S")
                    editarIvaPrecio = true;
                else
                    editarIvaPrecio = false;
                if (!editarIvaPrecio && Request.QueryString["actor"].ToString() != "P")
                {
                    ((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Attributes.Add("readonly", "true"); //Desactiva...
                    //((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Attributes.Add("NumericMaskE", "(this,event)");
                    //((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Attributes.Remove("onKeyUp");
                    //((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Enabled = false;
                }
            }

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                if (!editarIvaPrecio && Request.QueryString["actor"].ToString() != "P")
                {
                    ((TextBox)e.Item.Cells[6].FindControl("edit_precio")).Attributes.Add("readonly", "true");
                    //((TextBox)e.Item.Cells[6].FindControl("edit_precio")).Attributes.Add("NumericMaskE", "(this,event)");
                    //((TextBox)e.Item.Cells[6].FindControl("edit_precio")).Attributes.Remove("onKeyUp");
                    //((TextBox)e.Item.Cells[6].FindControl("edit_precio")).Enabled = false;
                }
                if (Tipo == "M")
                {
                    if (ViewState["CASA_MATRIZ"].ToString() == "S")
                        ((TextBox)e.Item.Cells[6].FindControl("edit_precio")).Attributes.Add("readonly", "true");
                }
            }
        }


        protected void ddlTipoOrden_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (DBFunctions.SingleData("SELECT TDOC_TIPODOCU FROM PDOCUMENTO where pDOC_codigo='" + ddlTipoOrden.SelectedValue + "'") == "OT")
                bind.PutDatasIntoDropDownList(ddlNumOrden, "Select mord_numeorde, pdoc_codigo concat '-' concat cast(mord_numeorde as character(20)) from MORDEN           where test_estado='A' AND pdoc_codigo='" + ddlTipoOrden.SelectedValue + "' order by mord_numeorde");//Ordenes de Trabajo Taller Abiertas que se les pueda realizar una transferencia de repuestos
            else
                bind.PutDatasIntoDropDownList(ddlNumOrden, "Select mord_numeorde, pdoc_codigo concat '-' concat cast(mord_numeorde as character(20)) from MORDENPRODUCCION where test_estado='A' AND pdoc_codigo='" + ddlTipoOrden.SelectedValue + "' order by mord_numeorde");//Ordenes de Producciòn Abiertas que se les pueda realizar una transferencia de repuestos
            ddlNumOrden.Items.Insert(0, new ListItem("Seleccione ...", "0"));
            bool produccion = (ddlTipoOrden.SelectedValue.Equals("OPE") || ddlTipoOrden.SelectedValue.Equals("OPP"));

            ViewState["PRODUCCION"] = produccion;
        }

        protected void ddlCodigo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ddlCodigo.SelectedValue == "C")
            {
                btnConsultaCoti.Enabled = ddlCotiPendt.Enabled = false;
            }
            else
            {
                btnConsultaCoti.Enabled = ddlCotiPendt.Enabled = true;
            }
            txtNumPed.Text = DBFunctions.SingleData("SELECT pped_ultipedi+1 FROM ppedido WHERE pped_codigo = '" + ddlCodigo.SelectedValue + "'");//Se actualiza el numero del pedido
            lblTipoPedido.Text = DBFunctions.SingleData("SELECT tped_nombre FROM tpedido WHERE tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "')");//Informacion del tipo de pedido
            tipoPedido = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'").Trim();
            Session["tipoPedido"] = tipoPedido;
            hdTipoPed.Value = tipoPedido;

            if (tipoPedido == "T")
            {
                ddlNumOrden.Visible = ddlTipoOrden.Visible = lblTipoOrden.Visible = lblNumOrden.Visible = lblPlaca.Visible = lblCargo.Visible = ddlCargo.Visible = true;
                bloque.Visible = true;
                ViewState["transferencia"] = "1";
            }
            else
            {
                ddlNumOrden.Visible = ddlTipoOrden.Visible = lblTipoOrden.Visible = lblNumOrden.Visible = lblPlaca.Visible = lblCargo.Visible = ddlCargo.Visible = false;
                bloque.Visible = false;
                ViewState["transferencia"] = null;
            }

            if (Request.QueryString["modificar"] == null)
            {
                if (Tipo == "M" || tipoPedido == "C")
                {
                    btnAjusFac.Enabled = false;
                    btnAjus.Enabled = true;
                }
                else if (Tipo == "C" && tipoPedido != "C")
                {
                        btnAjus.Enabled = false;
                        btnAjusFac.Enabled = true;
                }
            }
            else
            {
                btnAjusFac.Enabled = false;
                btnAjus.Enabled = false;
            }


            if (Tipo == "C")
            {
                if (tipoPedido == "T")       // LOS NITS DE TRANSFERENCIA SE DEBEN DEFINIR EN LA TABLA DE NITS DE TRANSFERENCIA
                    txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT PNIT.pnital_nittaller as NIT,MNIT.mnit_apellidos CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_apellido2,\\'\\') CONCAT \\' \\' CONCAT MNIT.mnit_nombres CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_nombre2,\\'\\') AS DESCRIPCION FROM dbxschema.mnit MNIT,dbxschema.pnittaller PNIT WHERE MNIT.mnit_nit=PNIT.pnital_nittaller', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                else
                    if (tipoPedido == "E")  // LOS CONSUMOS INTERNOS SE DEBEN DEFINIR EN LA TABLA DE NITS DE CONSUMO INTERNO
                        txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT PNIT.Mnit_nit AS NIT,MNIT.mnit_apellidos CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_apellido2,\\'\\') CONCAT \\' \\' CONCAT MNIT.mnit_nombres CONCAT \\' \\' CONCAT COALESCE(MNIT.mnit_nombre2,\\'\\')  AS DESCRIPCION FROM dbxschema.mnit MNIT,dbxschema.MinternoCCOSTO PNIT WHERE MNIT.mnit_nit=PNIT.Mnit_nit', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
                    else
                        txtNIT.Attributes["ondblclick"] = "ModalDialog(this, 'SELECT mnit_nit as NIT, mnit_apellidos concat \\' \\' concat COALESCE(mnit_apellido2,\\'\\') concat \\' \\' concat mnit_nombres concat \\' \\' concat COALESCE(mnit_nombre2,\\'\\') as Nombre FROM mnit order by nombre', new Array(),1);";//Craga clientes, se debe revisar estructura de clientes
            }
            CambioAlmacen(null, null);
        }

        protected void ddlListas_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ViewState["linea"] = ((DropDownList)sender).SelectedIndex;
        }

        #endregion

        #region Metodos

        private void registrarCitasConItem(string codItem, string bodega)
        {
            string newCodItem = "";
            if (Referencias.Guardar(codItem, ref newCodItem, bodega))
            {
                if (ViewState["citasConKitsProgramados"].ToString().Contains(newCodItem))
                    return;

                ArrayList citas;
                string citasStr;

                //desde hoy hasta dos días después, sin tomar en cuenta el domingo
                DateTime fechaIni = DateTime.Now;
                DateTime fechaFin = fechaIni.DayOfWeek == DayOfWeek.Friday || fechaIni.DayOfWeek == DayOfWeek.Saturday ?
                DateTime.Now.AddDays(3) :
                DateTime.Now.AddDays(2);

                string sql = String.Format(
                    @"SELECT MCI.MCIT_FECHA AS FECHA,  
                        	   MCI.MCIT_HORA AS HORA,  
                     		   PKI.PKIT_NOMBRE AS KIT  
                     FROM MKITITEM MKI   
                       LEFT JOIN PKIT PKI   
                       	LEFT JOIN MCITATALLER MCI ON MCI.PKIT_CODIGO = PKI.PKIT_CODIGO   
                       ON MKI.MKIT_CODIKIT = PKI.PKIT_CODIGO  
                     WHERE MKI.MKIT_CODITEM = '{0}' AND MCI.MCIT_FECHA BETWEEN '{1}' AND '{2}'"
                    , newCodItem
                    , fechaIni.ToString("yyyy-MM-dd")
                    , fechaFin.ToString("yyyy-MM-dd"));

                citas = DBFunctions.RequestAsCollection(sql);

                citasStr = String.Format("Item {0}: ", codItem);
                string newcitaStr = citasStr;
                foreach (Hashtable hash in citas)
                    citasStr += String.Format("kit: {0} el {1} a las {2}, "
                        , hash["KIT"]
                        , ((DateTime)hash["FECHA"]).ToString("dd/MM/yyyy")
                        , hash["HORA"]);

                if (newcitaStr != citasStr)
                    Utils.MostrarAlerta(Response, "Hay reserva de inventarios para el " + citasStr);
            }
        }

        private void llenarPrefijos()
        {
            String proceso = "%";
            if (Tipo == "C")
                proceso = "OT";
            String almacen = ddlAlmacen.SelectedValue;

            Utils.llenarPrefijos(Response, ref ddlTipoOrden, proceso, almacen, "%");
        }

        private void llenarVendedores()
        {
            String sql = @"SELECT pv.pven_codigo,  
                    pv.pven_nombre  
              FROM  pvendedor pv  
              INNER JOIN pvendedoralmacen pva ON pva.pven_codigo = pv.pven_codigo  
              WHERE pva.palm_almacen = '" + ddlAlmacen.SelectedValue + @"'  
              AND   (pv.tvend_codigo = 'VM' OR pv.tvend_codigo = 'TT')  
              AND   pv.pven_vigencia = 'V'   
              ORDER BY pv.PVEN_NOMBRE";

            Utils.FillDll(ddlVendedor, sql, true);
        }


        private void actualizarLblPlaca()
        {

            string PD = ddlTipoOrden.SelectedValue;//Prefijo OT
            string ND = ddlNumOrden.SelectedValue;//Numero OT
            string nom = DBFunctions.SingleData("select mnit_APELLIDOs concat '\' CONCAT COALESCE(mnit_APELLIDO2,'\') concat ' ' concat mnit_NOMBREs CONCAT ' ' concat COALESCE(mnit_NOMBRE2,'\') from mnit as t1, morden as t2 where t2.pdoc_codigo='" + PD + "' and t2.mord_numeorde=" + ND + " and t2.mnit_nit=t1.mnit_nit");//Nombre del cliente cambio 21-10-09

            string nt = DBFunctions.SingleData("select mnit_nit from morden as t2 where t2.pdoc_codigo='" + PD + "' and t2.mord_numeorde=" + ND);//nit del cliente
            string placa = DBFunctions.SingleData("select t1.mcat_placa from MCATALOGOVEHICULO as t1, morden as t2 where t2.mnit_nit='" + nt + "' and t1.mcat_vin=t2.mcat_vin and t2.pdoc_codigo='" + PD + "' and t2.mord_numeorde=" + ND + " and t1.mnit_nit=t2.mnit_nit");//placa del vehiculo
            //Se coloca el cargo que viene de la orden como la opcion por defecto del DropDownList de ddlCargo
            //ddlCargo.SelectedValue = DBFunctions.SingleData("select tcar_cargo from morden as t2 where t2.pdoc_codigo='"+PD+"' and t2.mord_numeorde="+ND).Trim();
            lblPlaca.Text = nom + " : " + placa;
        }

        private double ConsultarCupo(string nitCliente)
        {
            nNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM mnit where MNIT_NIT='" + nitCliente + "'");
            if (nNacionalidad == "E")
                Utils.MostrarAlerta(Response, "Este NIT es EXTRANJERO, NO SE liquida IVA !!!");
            double cupo = 0;
            try
            {
                cupo = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(MCLI_CUPOCRED,0) FROM mcliente WHERE mnit_nit='" + nitCliente + "'"));
            }
            catch
            {
                cupo = 0;
            }
            return cupo;
        }


        protected void BindDatas()
        {
            //if (ddlCodigo.SelectedValue == "Seleccione..")
            //    return;
            int i, j;
            dgItems.EnableViewState = true;
            dvInserts = new DataView(dtInserts);
            dgItems.DataSource = dtInserts;
            Session["dtInsertsCP"] = dtInserts;
            tipoPedido = DBFunctions.SingleData("SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + ddlCodigo.SelectedValue + "'");
            Session["tipoPedido"] = tipoPedido;
            dgItems.DataBind();
            for (i = 0; i < dgItems.Columns.Count; i++)
                if (i >= 3 && i <= 9)
                    for (j = 0; j < dgItems.Items.Count; j++)
                        dgItems.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
            //Debemos revisar si es tipo cliente y asi colocar los respectivos colores del semaforo
            if (Tipo != "P" && Tipo != null)
            {
                for (i = 0; i < dtInserts.Rows.Count; i++)
                {
                    if (dtInserts.Rows[i][12].ToString() == "0")
                        dgItems.Items[i].Cells[4].BackColor = Color.Red;
                    else if (dtInserts.Rows[i][12].ToString() == "3")
                            dgItems.Items[i].Cells[4].BackColor = Color.Yellow;
                    else if (dtInserts.Rows[i][12].ToString() == "1")
                            dgItems.Items[i].Cells[4].BackColor = Color.Green;
                    else if (dtInserts.Rows[i][12].ToString() == "2")
                            dgItems.Items[i].Cells[4].BackColor = Color.Chartreuse; // verde claro
                    else
                        lbInfo.Text += "<br>" + dtInserts.Rows[i][12].ToString();
                }
            }
            try
            {
                txtNumItem.Text = dtInserts.Rows.Count.ToString();
            }
            catch
            {
                txtNumItem.Text = "0";
            }
            double t = 0, ta = 0;
            int n;
            try
            {
                if (dtInserts.Rows.Count > 0)
                {
                    for (n = 0; n < dtInserts.Rows.Count; n++)
                    {
                        t += Convert.ToDouble(dtInserts.Rows[n][7]);
                        ta += Convert.ToDouble(dtInserts.Rows[n][9]);
                    }
                }
                txtTotal.Text = t.ToString("C");
                txtTotAsig.Text = ta.ToString("C");
                if (dtInserts.Rows.Count == 0)
                {
                    dgItems.EditItemIndex = -1;
                    ddlAlmacen.Enabled = ddlPrecios.Enabled = ddlTipoOrden.Enabled = ddlCodigo.Enabled = ddlNumOrden.Enabled = txtNIT.Enabled = txtNITa.Enabled = tbDate.Enabled = txtNumPed.Enabled = ddlCargo.Enabled = true;
                }
                else
                    ddlAlmacen.Enabled = ddlPrecios.Enabled = ddlNumOrden.Enabled = ddlTipoOrden.Enabled = ddlCodigo.Enabled = txtNumPed.Enabled = txtNIT.Enabled = txtNITa.Enabled = tbDate.Enabled = ddlCargo.Enabled = false;
            }
            catch
            { }
            if (Request.QueryString["modificar"] != null)
            {
                InterfazModificacion();
            }
        }

        protected void LoadDataColumns()
        {
            lbFields.Add("mite_codigo");//0
            types.Add(typeof(string));
            lbFields.Add("mite_nombre");//1
            types.Add(typeof(string));
            lbFields.Add("mite_cantidad");//2
            types.Add(typeof(double));
            lbFields.Add("mite_cantasig");//3
            types.Add(typeof(double));
            lbFields.Add("mite_precio");//4
            types.Add(typeof(double));
            lbFields.Add("mite_iva");//5
            types.Add(typeof(double));
            lbFields.Add("mite_desc");//6
            types.Add(typeof(double));
            lbFields.Add("mite_tot");//7
            types.Add(typeof(double));
            lbFields.Add("mite_disp");//8
            types.Add(typeof(double));
            lbFields.Add("mite_totA");//9
            types.Add(typeof(double));
            lbFields.Add("mite_precioinicial");//10
            types.Add(typeof(double));
            lbFields.Add("plin_codigo");//11
            types.Add(typeof(string));
            lbFields.Add("mite_color");//12  
            types.Add(typeof(string));
            lbFields.Add("mite_preciopromedio");//13
            types.Add(typeof(double));
        }

        protected void LoadDataTable()
        {
            int i;
            dtInserts = new DataTable();
            for (i = 0; i < lbFields.Count; i++)
                dtInserts.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
            Session["dtInsertsCP"] = dtInserts;
        }

        protected bool ValidarNIT(ref int ivm)
        {
            //Revisar que exista el nit
            if (dtInserts.Rows.Count == 0)
            {
                txtNIT.Text = txtNIT.Text.Trim();
                if (Tipo == "P")
                {
                    if (!DBFunctions.RecordExist("SELECT * FROM mproveedor WHERE mnit_nit = '" + txtNIT.Text + "'"))
                    {
                        Utils.MostrarAlerta(Response, "El NIT no pertenece a un proveedor!");
                        return (false);
                    }
                }
                string nN = DBFunctions.SingleData("SELECT mnit_APELLIDOS concat ' ' concat COALESCE(mnit_APELLIDO2,'') concat ' ' CONCAT mnit_NOMBRES concat ' ' concat COALESCE(mnit_NOMBRE2,'') FROM mnit where MNIT_NIT='" + txtNIT.Text + "'");
                if (nN.Length == 0)
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El NIT indicado (" + txtNIT.Text + ") no existe!");
                    return (false);
                }
                else
                {
                    txtNITa.Text = nN;
                    string estadoVigenciaNit = DBFunctions.SingleData("SELECT TVIG_VIGENCIA FROM mnit where MNIT_NIT='" + txtNIT.Text + "'");
                    if (estadoVigenciaNit != "V")
                    {
                        Utils.MostrarAlerta(Response, "Este NIT NO está en estado VIGENTE, proceso CANCELADO !!!");
                        return (false);
                    }
                    nNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM mnit where MNIT_NIT='" + txtNIT.Text + "'");
                    if (nNacionalidad == "E")
                        Utils.MostrarAlerta(Response, "Este NIT es EXTRANJERO, NO liquida IVA !!!");
                }
            }
            if (nNacionalidad == "E") ivm = 0;  // Los EXTRANJEROS NO pagan IVA
            else ivm = 1;
            //Revisar que tipo taller tenga nit de taller y si no es taller no tenga nit de taller
            //Se va cambiar la forma de mirar si el nit digitado es de cliente o no, se hace uso de la tabla pnitaller
            string tp = DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='" + ddlCodigo.SelectedValue + "'");
            if (tp == "T")
            {
                // ivm = 0; 
                if (!DBFunctions.RecordExist("SELECT * FROM pnittaller WHERE pnital_nittaller='" + txtNIT.Text + "'"))
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El NIT no pertenece a un taller!");
                    return (false);
                }
            }
            else
            {
                if (DBFunctions.RecordExist("SELECT * FROM pnittaller WHERE pnital_nittaller='" + txtNIT.Text + "'"))
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El NIT pertenece a un taller!");
                    return (false);
                }
            }
            return (true);
        }

        //INSERTAR UN ITEM EN LISTA
        protected void InsertaItem(string CodNIT, string linea, double cant, double prec, double descuento, int ivm)
        {
            ds = new DataSet();
            DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
            if (cInventarioX.Rows[0]["CEMP_LIQUDECI"].ToString() == "S")
                numDecimales = 2;
            else
                numDecimales = 0;

            if (CodNIT.Length > 0)
            {
                year = Convert.ToInt16(cInventarioX.Rows[0]["PANO_ANO"]);
                mes = Convert.ToInt16(cInventarioX.Rows[0]["PMES_MES"]);
                //string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
                //string mes_cinv = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
                string codI = "";
                double prIni = 0, pr = 0;
                string tipoLinea = DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'");
                if (!Referencias.Guardar(CodNIT, ref codI, tipoLinea))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " no es valido para la linea de bodega " + tipoLinea + " " + DBFunctions.SingleData("SELECT plin_nombre FROM plineaitem WHERE plin_codigo='" + linea + " '") + ".\\nRevise Por Favor!");
                    return;
                }
                if (!Referencias.RevisionSustitucion(ref codI, linea))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " NO se encuentra registrado. " + codI + "\\nRevise Por Favor!");
                    return;
                }
                if (Tipo == "M" && !DBFunctions.RecordExist("SELECT mnit_nit FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "';"))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " No se encuentra registrado el cliente!");
                    return;
                }
                string CodNIT2 = "";
                Referencias.Editar(codI, ref CodNIT2, tipoLinea);
                if (CodNIT2 != CodNIT)
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " se ha sustituido.\\nEl codigo actual es " + CodNIT2 + "!");
                //Revisar que no este ya el item en lista
                if ((this.dtInserts.Select("mite_codigo='" + CodNIT + "'")).Length > 0)
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El item ya esta en la lista, intente actualizarlo!\\nCodigo Item :" + CodNIT + "  \\nDescripción:" + DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mitems.mite_codigo='" + codI + "'") + "");
                    return;
                }
                //Que tipo de pedido es, si el pedido es tipo Garantia(G) o Interno(I) el precio del item no se toma de la lista de precios sino es el costo promedio + el factor de garantia o interno
                //Cantidad: Si la cantidad es menor o igual que cero le asigna 1
                if (cant <= 0)
                    cant = 1;

                //ver para que sirve ests DS.  ???

                //Datos del item cod, nom, iva, porcentaje de ganancia
                DBFunctions.Request(ds, IncludeSchema.NO, "select mt.mite_codigo, mt.mite_nombre, mt.piva_porciva, mt.plin_codigo, pd.pdes_porcdesc, mt.tori_codigo, ce.mnit_nit from cempresa ce, mitems mt, pdescuentoitem pd where mt.mite_codigo='" + codI + "' and pd.pdes_codigo=mt.pdes_codigo");//cambio 21-10/09
                //															0					1				        2					3                         4
                if ((ds.Tables[0].Rows[0][6].ToString() == "900969200" || ds.Tables[0].Rows[0][6].ToString() == "900969185") && ds.Tables[0].Rows[0][5].ToString() == "Y" && Tipo == "C" && tipoPedido == "N")
                    ds.Tables[0].Rows[0][2] = "0.0";  //AutoKoreana Y AutoColombiana NO cobran iva en lubricantes en venta por mostrador

                //Ingreso de precio completo, para calcular el precio sin iva. Caso especial bajo. IvaItemsInventario.
                //bool calcularPrecioSinIVA = Convert.ToBoolean(ConfigurationSettings.AppSettings["IvaItemsInventario"]);
                if (rdbCalSinIVA.SelectedValue.ToString() == "A")
                {
                    //double porcIVA = Convert.ToDouble(DBFunctions.SingleData("select mt.piva_porciva from mitems mt where mt.mite_codigo='" + codI + "'"));
                    double porcIVA = Convert.ToDouble(ds.Tables[0].Rows[0][2].ToString());
                    prec = prec / ((porcIVA * 0.01) + 1);
                }

                //Precio del item segun lista de precios
                if (Tipo != "P")
                {
                    //if(Convert.ToDouble(valorReal)!=prec)//23-10-2009
                    //{
                    if (tipoPedido == "E")
                        //prec = prec + (prec * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));23-10-09
                        prec = prec + (prec * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"])) / 100);
                    //}
                }

                prIni = pr = prec;
                //Descuento del cliente
                double desc = 0;
                if (Tipo != "P")
                {
                    /*string descs = DBFunctions.SingleData("SELECT MCLI_PCDESC FROM MCLIENTE WHERE MNIT_NIT='" +txtNIT.Text+"'");
                    if(descs.Length > 0)
                        desc = Convert.ToDouble(descs);*/
                    if (tipoPedido == "E" || tipoPedido == "P") // PEDIDOS de CONSUMO INTERNO y Promocion NO TIENEN DESCUENTO
                        desc = 0;
                    else
                        desc = descuento;
                }
                else if (Tipo == "P")//Aqui traemos el porcentaje de ganancia en este item y sugerimos el precio de compra de este item al proveedor
                        desc = descuento;
                //try{desc = Convert.ToDouble(ds.Tables[0].Rows[0][4]);}catch{}
                //Cantidad disponible en el almacen seleccionado

                double costProm = 0;
                //      if (mostrarPrecioMinimoYPromedio)
                {
                    //try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='" + codI + "' AND PALM_ALMACEN='" + ddlAlmacen.SelectedValue + "' AND PANO_ANO=" + ano_cinv)); }
                    try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + year)); }
                    catch { };

                    ///////estaba en comentarios

                    if (Tipo != "P")
                    {
                        string permitirBajoCosto = Request.QueryString["bCosto"];
                        if (tipoPedido == "E")   // consumo interno
                        {
                            pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"]) / 100));
                            prIni = pr;
                        }
                        else if ((tipoPedido == "G") || (tipoPedido == "T" && ddlCargo.SelectedValue == "G"))  // Garantía de Fábrica
                        {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORGARANTIA"]) / 100));
                                prIni = pr;
                        }
                        else if (tipoPedido == "T" && DBFunctions.RecordExist("SELECT TORI_CODIGO FROM MITEMS WHERE MITE_CODIGO = '" + codI + "' AND TORI_CODIGO = 'X'")) // Trabajo de terceros
                        {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"]) / 100));
                                prIni = pr;
                        }
                        else if (tipoPedido == "P")  //  promoción
                        {
                            if (pr == 0)
                            {
                                BindDatas();
                                Utils.MostrarAlerta(Response, "El item no tiene un precio registrado!");
                                return;
                            }
                            else
                            {
                                pr = pr - (pr * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + this.ddlCodigo.SelectedValue + "'")) / 100));
                                prIni = pr;
                            }
                        }
                        else
                        {
                            //   pr = pr - (pr*(desc/100));
                            if (pr == 0)
                            {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"]) / 100));
                                prIni = pr;
                                if (pr == 0)
                                    Utils.MostrarAlerta(Response, "El item no tiene un precio registrado, por favor de click en actualizar y digitelo");
                                else
                                    Utils.MostrarAlerta(Response, "El item no tenia un precio registrado en la lista de precios " + ddlPrecios.SelectedItem.Text + ", por lo tanto se le calculo");
                            }
                            else if (pr < (costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"]) / 100))))
                            {
                                if (permitirBajoCosto != null)
                                {
                                    Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad!  Ajuste el precio..");
                                    return;
                                }

                                Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad ! Realice el ingreso bajo su responsabilidad.");
                                // return; //este return hay que verificar por que no todas las empresas requieren que impida agregar el item apesar de la condicion...
                                //BindDatas();
                                //Utils.MostrarPreguntaRespuesta(Response, "Digite clave para seguir: ");
                                //Page.RegisterStartupScript("myScript", "<script language=JavaScript>abrir();</script>");
                                //ViewState["precioMenor"] = 1;
                                //Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad !");
                                //return;
                            }
                        }
                    }

                    else if (Tipo == "P")
                    {
                        if (pr == 0)
                            pr = costProm;
                        //    pr = pr - (pr*(desc/100));
                        prIni = pr;
                        if (pr <= 0)
                            Utils.MostrarAlerta(Response, "Por favor modifique el valor del repuesto. No puede ser menor o igual a 0!");
                    }

                    ///////////////////*/
                }
                double cantA = 0;						//Cantidad Asignada
                double cantD = Referencias.ConsultarDisponibilidad(codI, ddlAlmacen.SelectedValue, year.ToString(), 0); //Cantidad Disponible
                if (tipoPedido != "C")
                {
                    if (Tipo != "P")
                    {
                        if (cant > cantD)
                            cantA = cantD;
                        else
                            cantA = cant;
                    }
                    else if (Tipo == "P")
                    {
                        cantA = cant;
                        //Iva
                        if (DBFunctions.SingleData("SELECT MNIT.TREG_REGIIVA FROM MNIT,MITEMS WHERE MITEMS.MNIT_NIT=MNIT.MNIT_NIT AND MITEMS.MITE_CODIGO='" + CodNIT + "'").Equals("S"))
                            ivm = 0;  // cuando el nit está en regimen simplificado no se paga iva
                        else ivm = 1;
                    }
                }

                if (nNacionalidad == "E" || rdbCalSinIVA.SelectedValue.ToString() == "A")
                    ivm = 0;  // los extranejros no pagan iva
                else ivm = 1;
                double iva = Convert.ToDouble(ds.Tables[0].Rows[0][2]) * ivm;//Si se esta realizando una transferencia de taller no se liquida el iva todavia
                //Total
                if (nNacionalidad == "E")
                    numDecimales = 3;
                double tot = cant * pr;
                tot = tot + Math.Round(tot * (iva / 100), numDecimales);
                tot = tot - Math.Round((desc / 100) * tot, numDecimales);
                double totA = cantA * pr;
                totA = totA + Math.Round(totA * (iva / 100), numDecimales);
                totA = totA - Math.Round((desc / 100) * totA, numDecimales);

                tot = Math.Round(tot, numDecimales);
                prIni = Math.Round(prIni, numDecimales);
                totA = Math.Round(totA, numDecimales);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    registrarCitasConItem(CodNIT, tipoLinea);

                    //Llenar nueva fila
                    DataRow dr = dtInserts.NewRow();
                    dr[0] = CodNIT;                 //Codigo
                    dr[1] = ds.Tables[0].Rows[0][1];//Nombre
                    dr[2] = cant;					//Cantidad
                    dr[3] = cantA;					//CantidadAsig
                    dr[4] = prIni;					//Precio
                    dr[5] = iva;					//IVA
                    dr[6] = desc;					//Descuento
                    dr[7] = totA;					//Total
                    dr[8] = cantD;					//Disponible
                    dr[9] = totA;					//Total Asignado
                    dr[10] = prIni;					//Precio Inicial
                    dr[11] = ds.Tables[0].Rows[0][3];//Linea

                    //Vamos a determinar cual es el color del semaforo
                    dr[12] = Referencias.ConsultaSemaforoDisponibilidad(codI, ddlAlmacen.SelectedValue, Convert.ToUInt32(mes), Convert.ToInt32(year)).ToString();
                    dr[13] = costProm;

                    dtInserts.Rows.Add(dr);
                }
                if (hdDescCli.Value == string.Empty)
                    hdDescCli.Value = desc.ToString();
            }

            ViewState["dtInserts"] = dtInserts;
        }
        //sobrecarga del método
        protected void InsertaItem(string CodNIT, string linea, string tipoLin, double cant, double prec, double descuento, int ivm, string ano, string mes, DataSet dsTodo, DataRow cinventario)
        {
            DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
            ds = new DataSet();
            //DBFunctions.Request(ds, IncludeSchema.NO, "select mt.mite_codigo, mt.mite_nombre, mt.piva_porciva, mt.plin_codigo, pd.pdes_porcdesc, mt.tori_codigo, ce.mnit_nit from cempresa ce, mitems mt, pdescuentoitem pd where mt.mite_codigo='" + codI + "' and pd.pdes_codigo=mt.pdes_codigo");//cambio 21-10/09
            if (CodNIT.Length > 0)
            {
                //string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
                //string mes_cinv = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
                string codI = "";
                double prIni = 0, pr = 0;
                //string tipoLinea = DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'");
                if (!Referencias.Guardar(CodNIT, ref codI, tipoLin))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " no es válido para la línea de bodega " + tipoLin + "\\nRevise Por Favor!");
                    return;
                }
                if (!Referencias.RevisionSustitucion(ref codI, linea))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " NO se encuentra registrado. " + codI + "\\nRevise Por Favor!");
                    return;
                }
                if (Tipo == "M")//&& !DBFunctions.RecordExist("SELECT mnit_nit FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "';"))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " No se encuentra registrado el cliente!");
                    return;
                }
                string CodNIT2 = "";
                Referencias.Editar(codI, ref CodNIT2, tipoLin);
                if (CodNIT2 != CodNIT)
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " se ha sustituido.\\nEl codigo actual es " + CodNIT2 + "!");
                //Revisar que no este ya el item en lista
                if ((this.dtInserts.Select("mite_codigo='" + CodNIT + "'")).Length > 0)
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El item ya esta en la lista, intente actualizarlo!\\nCodigo Item :" + CodNIT);
                    return;
                }
                //Que tipo de pedido es, si el pedido es tipo Garantia(G) o Interno(I) el precio del item no se toma de la lista de precios sino es el costo promedio + el factor de garantia o interno
                //Cantidad: Si la cantidad es menor o igual que cero le asigna 1
                if (cant <= 0)
                    cant = 1;
                //Datos del item cod, nom, iva, porcentaje de ganancia
                //															0					1				        2					3                         4
                if ((dsTodo.Tables[0].Rows[0][6].ToString() == "900969200" || ds.Tables[0].Rows[0][6].ToString() == "900969185") && ds.Tables[0].Rows[0][5].ToString() == "Y" && Tipo == "C" && tipoPedido == "N")
                    ds.Tables[0].Rows[0][2] = "0.0";  //AutoKoreana Y AutoColombiana NO cobran iva en lubricantes en venta por mostrador

                //Ingreso de precio completo, para calcular el precio sin iva. Caso especial bajo. IvaItemsInventario.
                //bool calcularPrecioSinIVA = Convert.ToBoolean(ConfigurationSettings.AppSettings["IvaItemsInventario"]);
                if (rdbCalSinIVA.SelectedValue.ToString() == "A")
                {
                    double porcIVA = Convert.ToDouble(ds.Tables[0].Rows[0]["PIVA_PORCIVA"].ToString());
                    prec = prec / ((porcIVA * 0.01) + 1);
                }

                //Precio del item segun lista de precios
                if (Tipo != "P")
                {
                    //if(Convert.ToDouble(valorReal)!=prec)//23-10-2009
                    //{
                    if (tipoPedido == "E")
                        //prec = prec + (prec * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));23-10-09
                        prec = prec + (prec * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));
                    //}
                }

                prIni = pr = prec;
                //Descuento del cliente
                double desc = 0;
                if (Tipo != "P")
                {
                    /*string descs = DBFunctions.SingleData("SELECT MCLI_PCDESC FROM MCLIENTE WHERE MNIT_NIT='" +txtNIT.Text+"'");
                    if(descs.Length > 0)
                        desc = Convert.ToDouble(descs);*/
                    if (tipoPedido == "E" || tipoPedido == "P") // PEDIDOS de CONSUMO INTERNO y Promocion NO TIENEN DESCUENTO
                        desc = 0;
                    else
                        desc = descuento;
                }
                else if (Tipo == "P")//Aqui traemos el porcentaje de ganancia en este item y sugerimos el precio de compra de este item al proveedor
                        desc = descuento;
                //try{desc = Convert.ToDouble(ds.Tables[0].Rows[0][4]);}catch{}
                //Cantidad disponible en el almacen seleccionado

                double costProm = 0;
                //      if (mostrarPrecioMinimoYPromedio)
                {
                    //try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='" + codI + "' AND PALM_ALMACEN='" + ddlAlmacen.SelectedValue + "' AND PANO_ANO=" + ano_cinv)); }
                    try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + ano)); }
                    catch { };

                    ///////estaba en comentarios

                    if (Tipo != "P")
                    {
                        string permitirBajoCosto = Request.QueryString["bCosto"];
                        if (tipoPedido == "E")   // consumo interno
                        {
                            pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));
                            prIni = pr;
                        }
                        else if ((tipoPedido == "G") || (tipoPedido == "T" && ddlCargo.SelectedValue == "G"))  // Garantía de Fábrica
                        {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORGARANTIA"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorgarantia FROM cinventario")) / 100));
                                prIni = pr;
                        }
                        else if (tipoPedido == "T" && DBFunctions.RecordExist("SELECT TORI_CODIGO FROM MITEMS WHERE MITE_CODIGO = '" + codI + "' AND TORI_CODIGO = 'X'")) // Trabajo de terceros
                        {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario")) / 100));
                                prIni = pr;
                        }
                        else if (tipoPedido == "P")  //  promoción
                        {
                            if (pr == 0)
                            {
                                BindDatas();
                                Utils.MostrarAlerta(Response, "El item no tiene un precio registrado!");
                                return;
                            }
                            else
                            {
                                pr = pr - (pr * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + this.ddlCodigo.SelectedValue + "'")) / 100));
                                prIni = pr;
                            }
                        }
                        else
                        {
                            //   pr = pr - (pr*(desc/100));
                            if (pr == 0)
                            {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario").Trim()) / 100));
                                prIni = pr;
                                if (pr == 0)
                                    Utils.MostrarAlerta(Response, "El item no tiene un precio registrado, por favor de click en actualizar y digitelo");
                                else
                                    Utils.MostrarAlerta(Response, "El item no tenia un precio registrado en la lista de precios " + ddlPrecios.SelectedItem.Text + ", por lo tanto se le calculo");
                            }
                            else if (pr < (costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString()) / 100)))) ;//DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario").Trim()) / 100))))
                            {
                                if (permitirBajoCosto != null)
                                {
                                    Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad!  Ajuste el precio..");
                                    return;
                                }

                                Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad ! Realice el ingreso bajo su responsabilidad.");
                                // return; //este return hay que verificar por que no todas las empresas requieren que impida agregar el item apesar de la condicion...
                                //BindDatas();
                                //Utils.MostrarPreguntaRespuesta(Response, "Digite clave para seguir: ");
                                //Page.RegisterStartupScript("myScript", "<script language=JavaScript>abrir();</script>");
                                //ViewState["precioMenor"] = 1;
                                //Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad !");
                                //return;
                            }
                        }
                    }

                    else if (Tipo == "P")
                    {
                        if (pr == 0)
                            pr = costProm;
                        //    pr = pr - (pr*(desc/100));
                        prIni = pr;
                        if (pr <= 0)
                            Utils.MostrarAlerta(Response, "Por favor modifique el valor del repuesto. No puede ser menor o igual a 0!");
                    }

                    ///////////////////*/
                }
                double cantA = 0;						//Cantidad Asignada
                double cantD = Referencias.ConsultarDisponibilidad(codI, ddlAlmacen.SelectedValue, ano, 0); //Cantidad Disponible
                if (tipoPedido != "C")
                {
                    if (Tipo != "P")
                    {
                        if (cant > cantD)
                            cantA = cantD;
                        else
                            cantA = cant;
                    }
                    else if (Tipo == "P")
                    {
                        cantA = cant;
                        //Iva
                        if (DBFunctions.SingleData("SELECT MNIT.TREG_REGIIVA FROM MNIT,MITEMS WHERE MITEMS.MNIT_NIT=MNIT.MNIT_NIT AND MITEMS.MITE_CODIGO='" + CodNIT + "'").Equals("S"))
                            ivm = 0;  // cuando el nit está en regimen simplificado no se paga iva
                        else ivm = 1;
                    }
                }

                if (nNacionalidad == "E" || rdbCalSinIVA.SelectedValue.ToString() == "A") ivm = 0;  // los extranejros no pagan iva
                else ivm = 1;
                double iva = Convert.ToDouble(ds.Tables[0].Rows[0][2]) * ivm;//Si se esta realizando una transferencia de taller no se liquida el iva todavia
                //Total
                double tot = cant * pr;
                if (nNacionalidad == "E")
                    numDecimales = 3;
                tot = tot + Math.Round(tot * (iva / 100), numDecimales);
                tot = tot - Math.Round((desc / 100) * tot, numDecimales);
                double totA = cantA * pr;
                totA = totA + Math.Round(totA * (iva / 100), numDecimales);
                totA = totA - Math.Round((desc / 100) * totA, numDecimales);

                tot = Math.Round(tot, numDecimales);
                prIni = Math.Round(prIni, numDecimales);
                totA = Math.Round(totA, numDecimales);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    registrarCitasConItem(CodNIT, tipoLin);

                    //Llenar nueva fila
                    DataRow dr = dtInserts.NewRow();
                    dr[0] = CodNIT;                 //Codigo
                    dr[1] = ds.Tables[0].Rows[0][1];//Nombre
                    dr[2] = cant;					//Cantidad
                    dr[3] = cantA;					//CantidadAsig
                    dr[4] = prIni;					//Precio
                    dr[5] = iva;					//IVA
                    dr[6] = desc;					//Descuento
                    dr[7] = totA;					//Total
                    dr[8] = cantD;					//Disponible
                    dr[9] = totA;					//Total Asignado
                    dr[10] = prIni;					//Precio Inicial
                    dr[11] = ds.Tables[0].Rows[0][3];//Linea

                    //Vamos a determinar cual es el color del semaforo
                    dr[12] = Referencias.ConsultaSemaforoDisponibilidad(codI, ddlAlmacen.SelectedValue, Convert.ToUInt32(mes), Convert.ToInt32(ano)).ToString();
                    dr[13] = costProm;

                    dtInserts.Rows.Add(dr);
                }
                if (hdDescCli.Value == string.Empty)
                    hdDescCli.Value = desc.ToString();
            }
            ViewState["dtInserts"] = dtInserts;
        }

        //SOBRECARGA DEL MÉTODO
        protected void InsertaItem(string CodNIT, string linea, string tipoLin, double cant, double prec, double descuento, int ivm, string ano, string mes, DataSet dsTodo, DataRow cinventario, string h)
        {
            ds = new DataSet();
            DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
            //DBFunctions.Request(ds, IncludeSchema.NO, "select mt.mite_codigo, mt.mite_nombre, mt.piva_porciva, mt.plin_codigo, pd.pdes_porcdesc, mt.tori_codigo, ce.mnit_nit from cempresa ce, mitems mt, pdescuentoitem pd where mt.mite_codigo='" + codI + "' and pd.pdes_codigo=mt.pdes_codigo");//cambio 21-10/09
            if (CodNIT.Length > 0)
            {
                //string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
                //string mes_cinv = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
                string codI = "";
                double prIni = 0, pr = 0;
                //string tipoLinea = DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'");
                if (!Referencias.Guardar(CodNIT, ref codI, tipoLin))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " no es valido para la linea de bodega " + tipoLin + "\\nRevise Por Favor!");
                    return;
                }
                if (!Referencias.RevisionSustitucion(ref codI, linea))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " NO se encuentra registrado. " + codI + "\\nRevise Por Favor!");
                    return;
                }
                if (Tipo == "M")//&& !DBFunctions.RecordExist("SELECT mnit_nit FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "';"))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " No se encuentra registrado el cliente!");
                    return;
                }
                string CodNIT2 = "";
                Referencias.Editar(codI, ref CodNIT2, tipoLin);
                if (CodNIT2 != CodNIT)
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " se ha sustituido.\\nEl codigo actual es " + CodNIT2 + "!");
                //Revisar que no este ya el item en lista
                if ((this.dtInserts.Select("mite_codigo='" + CodNIT + "'")).Length > 0)
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El item ya esta en la lista, intente actualizarlo!\\nCodigo Item :" + CodNIT);
                    return;
                }
                //Que tipo de pedido es, si el pedido es tipo Garantia(G) o Interno(I) el precio del item no se toma de la lista de precios sino es el costo promedio + el factor de garantia o interno
                //Cantidad: Si la cantidad es menor o igual que cero le asigna 1
                if (cant <= 0)
                    cant = 1;
                //Datos del item cod, nom, iva, porcentaje de ganancia
                //															0					1				        2					3                         4
                if ((dsTodo.Tables[0].Rows[0][6].ToString() == "900969200" || ds.Tables[0].Rows[0][6].ToString() == "900969185") && ds.Tables[0].Rows[0][5].ToString() == "Y" && Tipo == "C" && tipoPedido == "N")
                    ds.Tables[0].Rows[0][2] = "0.0";  //AutoKoreana Y AutoColombiana NO cobran iva en lubricantes en venta por mostrador

                //Ingreso de precio completo, para calcular el precio sin iva. Caso especial bajo. IvaItemsInventario.
                //bool calcularPrecioSinIVA = Convert.ToBoolean(ConfigurationSettings.AppSettings["IvaItemsInventario"]);
                if (rdbCalSinIVA.SelectedValue.ToString() == "A")
                {
                    double porcIVA = Convert.ToDouble(ds.Tables[0].Rows[0]["PIVA_PORCIVA"].ToString());
                    prec = prec / ((porcIVA * 0.01) + 1);
                }

                //Precio del item segun lista de precios
                if (Tipo != "P")
                {
                    //if(Convert.ToDouble(valorReal)!=prec)//23-10-2009
                    //{
                    if (tipoPedido == "E")
                        //prec = prec + (prec * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));23-10-09
                        prec = prec + (prec * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));
                    //}
                }

                prIni = pr = prec;
                //Descuento del cliente
                double desc = 0;
                if (Tipo != "P")
                {
                    /*string descs = DBFunctions.SingleData("SELECT MCLI_PCDESC FROM MCLIENTE WHERE MNIT_NIT='" +txtNIT.Text+"'");
                    if(descs.Length > 0)
                        desc = Convert.ToDouble(descs);*/
                    if (tipoPedido == "E" || tipoPedido == "P") // PEDIDOS de CONSUMO INTERNO y Promocion NO TIENEN DESCUENTO
                        desc = 0;
                    else
                        desc = descuento;
                }
                else if (Tipo == "P")//Aqui traemos el porcentaje de ganancia en este item y sugerimos el precio de compra de este item al proveedor
                        desc = descuento;
                //try{desc = Convert.ToDouble(ds.Tables[0].Rows[0][4]);}catch{}
                //Cantidad disponible en el almacen seleccionado

                double costProm = 0;
                //      if (mostrarPrecioMinimoYPromedio)
                {
                    //try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='" + codI + "' AND PALM_ALMACEN='" + ddlAlmacen.SelectedValue + "' AND PANO_ANO=" + ano_cinv)); }
                    try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + ano)); }
                    catch { };

                    ///////estaba en comentarios

                    if (Tipo != "P")
                    {
                        string permitirBajoCosto = Request.QueryString["bCosto"];
                        if (tipoPedido == "E")   // consumo interno
                        {
                            pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorinterno FROM cinventario")) / 100));
                            prIni = pr;
                        }
                        else if ((tipoPedido == "G") || (tipoPedido == "T" && ddlCargo.SelectedValue == "G"))  // Garantía de Fábrica
                        {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORGARANTIA"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorgarantia FROM cinventario")) / 100));
                                prIni = pr;
                        }
                        else if (tipoPedido == "T" && DBFunctions.RecordExist("SELECT TORI_CODIGO FROM MITEMS WHERE MITE_CODIGO = '" + codI + "' AND TORI_CODIGO = 'X'")) // Trabajo de terceros
                        {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario")) / 100));
                                prIni = pr;
                        }
                        else if (tipoPedido == "P")  //  promoción
                        {
                            if (pr == 0)
                            {
                                BindDatas();
                                Utils.MostrarAlerta(Response, "El item no tiene un precio registrado!");
                                return;
                            }
                            else
                            {
                                pr = pr - (pr * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + this.ddlCodigo.SelectedValue + "'")) / 100));
                                prIni = pr;
                            }
                        }
                        else
                        {
                            //   pr = pr - (pr*(desc/100));
                            if (pr == 0)
                            {
                                pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString())) / 100);//DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario").Trim()) / 100));
                                prIni = pr;
                                if (pr == 0)
                                    Utils.MostrarAlerta(Response, "El item no tiene un precio registrado, por favor de click en actualizar y digitelo");
                                else
                                    Utils.MostrarAlerta(Response, "El item no tenia un precio registrado en la lista de precios " + ddlPrecios.SelectedItem.Text + ", por lo tanto se le calculo");
                            }
                            else if (pr < (costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString()) / 100)))) ;//DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario").Trim()) / 100))))
                            {
                                if (permitirBajoCosto != null)
                                {
                                    Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad!  Ajuste el precio..");
                                    return;
                                }

                                Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad ! Realice el ingreso bajo su responsabilidad.");
                                // return; //este return hay que verificar por que no todas las empresas requieren que impida agregar el item apesar de la condicion...
                                //BindDatas();
                                //Utils.MostrarPreguntaRespuesta(Response, "Digite clave para seguir: ");
                                //Page.RegisterStartupScript("myScript", "<script language=JavaScript>abrir();</script>");
                                //ViewState["precioMenor"] = 1;
                                //Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio más el margen de utilidad !");
                                //return;
                            }
                        }
                    }

                    else if (Tipo == "P")
                    {
                        if (pr == 0)
                            pr = costProm;
                        //    pr = pr - (pr*(desc/100));
                        prIni = pr;
                        if (pr <= 0)
                            Utils.MostrarAlerta(Response, "Por favor modifique el valor del repuesto. No puede ser menor o igual a 0!");
                    }

                    ///////////////////*/
                }
                double cantA = 0;						//Cantidad Asignada
                double cantD = Referencias.ConsultarDisponibilidad(codI, ddlAlmacen.SelectedValue, ano, 0); //Cantidad Disponible
                if (tipoPedido != "C")
                {
                    if (Tipo != "P")
                    {
                        if (cant > cantD)
                            cantA = cantD;
                        else
                            cantA = cant;
                    }
                    else if (Tipo == "P")
                    {
                        cantA = cant;
                        //Iva
                        if (DBFunctions.SingleData("SELECT MNIT.TREG_REGIIVA FROM MNIT,MITEMS WHERE MITEMS.MNIT_NIT=MNIT.MNIT_NIT AND MITEMS.MITE_CODIGO='" + CodNIT + "'").Equals("S"))
                            ivm = 0;  // cuando el nit está en regimen simplificado no se paga iva
                        else ivm = 1;
                    }
                }

                if (nNacionalidad == "E" || rdbCalSinIVA.SelectedValue.ToString() == "A") ivm = 0;  // los extranejros no pagan iva
                else ivm = 1;
                double iva = Convert.ToDouble(ds.Tables[0].Rows[0][2]) * ivm;//Si se esta realizando una transferencia de taller no se liquida el iva todavia
                //Total
                double tot = cant * pr;
                tot = tot + Math.Round(tot * (iva / 100), numDecimales);
                tot = tot - Math.Round((desc / 100) * tot, numDecimales);
                double totA = cantA * pr;
                totA = totA + Math.Round(totA * (iva / 100), numDecimales);
                totA = totA - Math.Round((desc / 100) * totA, numDecimales);

                tot = Math.Round(tot, numDecimales);
                prIni = Math.Round(prIni, numDecimales);
                totA = Math.Round(totA, numDecimales);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    registrarCitasConItem(CodNIT, tipoLin);

                    //Llenar nueva fila
                    DataRow dr = dtInserts.NewRow();
                    dr[0] = CodNIT;                 //Codigo
                    dr[1] = ds.Tables[0].Rows[0][1];//Nombre
                    dr[2] = cant;					//Cantidad
                    dr[3] = cantA;					//CantidadAsig
                    dr[4] = prIni;					//Precio
                    dr[5] = iva;					//IVA
                    dr[6] = desc;					//Descuento
                    dr[7] = totA;					//Total
                    dr[8] = cantD;					//Disponible
                    dr[9] = totA;					//Total Asignado
                    dr[10] = prIni;					//Precio Inicial
                    dr[11] = ds.Tables[0].Rows[0][3];//Linea

                    //Vamos a determinar cual es el color del semaforo
                    dr[12] = Referencias.ConsultaSemaforoDisponibilidad(codI, ddlAlmacen.SelectedValue, Convert.ToUInt32(mes), Convert.ToInt32(ano)).ToString();
                    dr[13] = costProm;

                    dtInserts.Rows.Add(dr);
                }
                if (hdDescCli.Value == string.Empty)
                    hdDescCli.Value = desc.ToString();
            }
            ViewState["dtInserts"] = dtInserts;
        }
        //INSERTAR UN ITEM EN LISTA SUGERIDO
        protected void InsertaItem(string CodNIT, string linea, double cant, int ivm)
        {
            ds = new DataSet();
            DataTable cInventarioX = (DataTable)ViewState["CINVENTARIO"];
            if (CodNIT.Length > 0)
            {
                string ano_cinv = cInventarioX.Rows[0]["PANO_ANO"].ToString();
                string mes_cinv = cInventarioX.Rows[0]["PMES_MES"].ToString();
                string codI = "";
                if (!Referencias.Guardar(CodNIT, ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'")))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " no es valido para la linea de bodega " + DBFunctions.SingleData("SELECT plin_nombre FROM plineaitem WHERE plin_codigo='" + linea + "'") + ".\\nRevise Por Favor!");
                    return;
                }
                if (!Referencias.RevisionSustitucion(ref codI, linea))
                {
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " No se encuentra registrado.\\nRevise Por Favor!");
                    return;
                }
                string CodNIT2 = "";
                Referencias.Editar(codI, ref CodNIT2, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'"));
                if (CodNIT2 != CodNIT)
                    Utils.MostrarAlerta(Response, "El codigo " + CodNIT + " se ha sustituido.\\nEl codigo actual es " + CodNIT2 + "!");
                //Revisar que no este ya el item en lista
                if ((this.dtInserts.Select("mite_codigo='" + CodNIT + "'")).Length > 0)
                {
                    BindDatas();
                    Utils.MostrarAlerta(Response, "El item ya esta en la lista, intente actualizarlo!\\nCodigo Item :" + CodNIT + "  \\nDescripción:" + DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mitems.mite_codigo='" + codI + "'") + "");
                    return;
                }
                //Que tipo de pedido es, si el pedido es tipo Garantia(G) o Interno(I) el precio del item no se toma de la lista de precios sino es el costo promedio + el factor de garantia o interno
                //Cantidad: Si la cantidad es menor o igual que cero le asigna 1
                if (cant <= 0)
                    cant = 1;
                //Datos del item cod, nom, iva, porcentaje de ganancia
                DBFunctions.Request(ds, IncludeSchema.NO, "select mitems.mite_codigo, mitems.mite_nombre, mitems.piva_porciva, mitems.plin_codigo, pdescuentoitem.pdes_porcdesc from mitems, pdescuentoitem where mitems.mite_codigo='" + codI + "' and pdescuentoitem.pdes_codigo=mitems.pdes_codigo");
                //															0					1				2					3                         4
                //Precio del item segun lista de precios
                string prs = DBFunctions.SingleData("SELECT mpre_precio FROM MPRECIOITEM WHERE mite_codigo='" + ds.Tables[0].Rows[0][0].ToString() + "' AND ppre_codigo='" + ddlPrecios.SelectedValue + "'");
                double pr = 0;
                if (prs.Length > 0)
                    pr = Convert.ToDouble(prs);
                //Descuento del cliente
                double prIni = pr;
                double desc = 0;
                if (Tipo == "C")
                {
                    string descs = DBFunctions.SingleData("SELECT MCLI_PORCDESCINV FROM MCLIENTE WHERE MNIT_NIT='" + txtNIT.Text + "'");
                    if (descs.Length > 0)
                        desc = Convert.ToDouble(descs);
                }
                else if (Tipo == "M")
                {
                        desc = Convert.ToDouble(DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + txtNIT.Text + "'"));
                }
                else if (Tipo == "P")//Aqui traemos el porcentaje de ganancia en este item y sugerimos el precio de compra de este item al proveedor
                        desc = 0;

                if (tipoPedido == "P")
                    desc = 0;  // los pedidos de promocion NO TIENEN DESCUENTO, LA LISTA DE PRECIOS DEBE SER DEFINIDA
                //try{desc = Convert.ToDouble(ds.Tables[0].Rows[0][4]);}catch{}
                //Cantidad disponible en el almacen seleccionado
                double costProm = 0;
                //try{costProm=Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='" +codI+"' AND PALM_ALMACEN='"+ddlAlmacen.SelectedValue+"' AND PANO_ANO="+ano_cinv));}
                try { costProm = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + ano_cinv)); }
                catch { };
                if (Tipo != "P")
                {
                    /////////////Esta parte solo aplica para los pedidos de clientes ???????????????
                    if (tipoPedido == "E")
                    {
                        pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORINTERNO"].ToString()) / 100));
                        prIni = pr;
                    }
                    else if (tipoPedido == "G")
                    {
                            pr = costProm + (costProm * (Convert.ToDouble(DBFunctions.SingleData(cInventarioX.Rows[0]["CINV_FACTOGARANTIA"].ToString())) / 100));
                            prIni = pr;
                    }
                    else if (tipoPedido == "P")
                    {
                        if (pr == 0)
                        {
                            BindDatas();
                            Utils.MostrarAlerta(Response, "El item no tiene un precio registrado o su precio es menor al costo promedio del item!");
                            return;
                        }
                        else
                        {
                            pr = pr - (pr * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + this.ddlCodigo.SelectedValue + "'")) / 100));
                            prIni = pr;
                        }
                    }
                    else
                    {
                        //pr = pr - (pr*(desc/100));
                        if (pr == 0)
                        {
                            pr = costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString().Trim()) / 100));
                            prIni = pr;
                            if (pr == 0)
                                Utils.MostrarAlerta(Response, "El item no tiene un precio registrado, por favor de click en actualizar y digitelo");
                            else
                                Utils.MostrarAlerta(Response, "El item no tenia un precio registrado en la lista de precios " + ddlPrecios.SelectedItem.Text + ", por lo tanto se le calculo");
                        }
                        else if (pr < (costProm + (costProm * (Convert.ToDouble(cInventarioX.Rows[0]["CINV_FACTORUTILIDAD"].ToString().Trim()) / 100))))
                        {
                            BindDatas();
                            Utils.MostrarAlerta(Response, "El precio del item es menor al costo promedio!");
                            return;
                        }
                    }
                    //////////////////////////////////////////////////////////////////////////////////////////////
                }
                else if (Tipo == "P")
                {
                    if (pr == 0)
                        pr = costProm;
                    //	pr = pr - Math.Round(pr*(desc/100),0);
                    if (pr <= 0)
                        Utils.MostrarAlerta(Response, "Por favor modifique el valor del repuesto. No puede ser menor o igual a 0!");
                }
                double cantA = 0;						//Cantidad Asignada
                double cantD = Referencias.ConsultarDisponibilidad(codI, ddlAlmacen.SelectedValue, ano_cinv, 0); //Cantidad Disponible
                if (Tipo != "P")
                {
                    if (cant > cantD)
                        cantA = cantD;
                    else
                        cantA = cant;
                }
                else if (Tipo == "P")
                        cantA = cant;
                //Iva
                if (nNacionalidad == "E") ivm = 0; // los extranjeros no pagan iva
                else ivm = 1;
                if (DBFunctions.SingleData("SELECT MNIT.TREG_REGIIVA FROM MNIT,MITEMS " +
                    "WHERE MITEMS.MNIT_NIT=MNIT.MNIT_NIT AND MITEMS.MITE_CODIGO='" + CodNIT + "'").Equals("S") && (Tipo == "P"))
                {
                    ivm = 0;  // cuando el nit está en regimen simplificado no se paga iva
                }
                else ivm = 1;
                double iva = Convert.ToDouble(ds.Tables[0].Rows[0][2]) * ivm;//Si se esta realizando una transferencia de taller no se liquida el iva todavia
                //Total

                double totA = cantA * pr;
                totA = totA + Math.Round(totA * (iva / 100), numDecimales);

                //Llenar nueva fila
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dtInserts.NewRow();
                    dr[0] = CodNIT;//Codigo
                    dr[1] = ds.Tables[0].Rows[0][1];//Nombre
                    dr[2] = cant;					//Cantidad
                    dr[3] = cantA;					//CantidadAsig
                    dr[4] = prIni;					//Precio
                    dr[5] = iva;					//IVA
                    dr[6] = desc;					//Descuento
                    dr[7] = totA;					//Total
                    dr[8] = cantD;					//Disponible
                    dr[9] = totA;					//Total Asignado
                    dr[10] = prIni;					//Precio Inicial
                    dr[11] = ds.Tables[0].Rows[0][3];//Linea
                    //Vamos a determinar cual es el color del semaforo
                    dr[12] = Referencias.ConsultaSemaforoDisponibilidad(codI, ddlAlmacen.SelectedValue, Convert.ToUInt32(mes_cinv), Convert.ToInt32(ano_cinv)).ToString();
                    dtInserts.Rows.Add(dr);
                }
            }
        }

        protected bool Validar_dtInserts()
        {
            //Si el pedido es de cliente y no es de tipo interno(E), garantia(G) o promocion(P)
            //debo verificar q ningun precio sea cero
            bool error = false;
            if (Tipo != "P" && tipoPedido != "E" && tipoPedido != "G" && tipoPedido != "P")
            {
                DataRow[] ceros = dtInserts.Select("mite_precio=0");
                if (ceros.Length == 0)
                    error = false;
                else
                    error = true;
            }
            return error;
        }

        //VALIDAR DATOS
        protected bool ValidarDatosIniciales()
        {
            if (!(txtNIT.Text.Length > 0))
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar un NIT!");
                return (false);
            }
            string tVIG = DBFunctions.SingleData("SELECT TVIG_VIGENCIA FROM MNIT where MNIT_NIT ='" + txtNIT.Text + "'");
            if (tVIG != "V")
            {
                Utils.MostrarAlerta(Response, "Este Nit NO está Vigente !!!");
                return (false);
            }
            Tipo = Request.QueryString["actor"]; //Debe definir el tipo de pedido: Cliente o proveedor
            if (this.ddlPrecios.SelectedValue == "Seleccione.." && Tipo != "P")
            {
                Utils.MostrarAlerta(Response, "Debe Seleccionar una lista de Precios !");
                return (false);
            }

            if (!(txtNumPed.Text.Length > 0))
            {
                Utils.MostrarAlerta(Response, "Debe dar el numero de pedido!");
                return (false);
            }

            string PD = "", ND = ""; //Prefijo y Numero de OT (en caso que sea una transferencia a taller)
            string tp = DBFunctions.SingleData("SELECT TPED_CODIGO FROM ppedido where pped_codigo='" + ddlCodigo.SelectedValue + "'");

            if (tp == "T")   // Valida la Orden de Trabajo 
            {
                PD = ddlTipoOrden.SelectedValue;
                ND = ddlNumOrden.SelectedValue;
                if (this.ddlNumOrden.SelectedIndex == 0)
                {
                    Utils.MostrarAlerta(Response, "Debe Seleccionar una orden para la transferencia!");
                    return (false);
                }
                if (DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + PD + "' ") == "OT")
                {
                    if (DBFunctions.SingleData("SELECT test_estado FROM MORDEN WHERE pdoc_codigo='" + PD + "' AND MORD_NUMEORDE=" + ND + " ") != "A")
                    {
                        Utils.MostrarAlerta(Response, "El tipo, numero o estado de la Orden de Trabajo NO es valido!");
                        return (false);
                    }
                }
                else if (DBFunctions.SingleData("SELECT test_estado FROM MORDENPRODUCCION WHERE pdoc_codigo='" + PD + "' AND MORD_NUMEORDE=" + ND + " ") != "A")
                {
                        Utils.MostrarAlerta(Response, "El tipo, numero o estado de la Orden de Produccion NO es valido!");
                        return (false);
                    }
            }

            return (true);
        }

        protected bool ValidarDatos()
        {
            if (dtInserts.Rows.Count == 0)
            {
                BorrarGrilla();
                Utils.MostrarAlerta(Response, "No hay items en la lista!");
                return (false);
            }
            if (Validar_dtInserts())
            {
                Utils.MostrarAlerta(Response, "Algun precio es cero, revise sus datos");
                return (false);
            }

            //Ahora revisamos si la clave del vendedor es valida o no
            if ((DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + ddlVendedor.SelectedValue + "'") != tbClaveVend.Text.Trim()) || (ddlVendedor.SelectedValue == "0"))
            {
                Utils.MostrarAlerta(Response, "La clave de " + ddlVendedor.SelectedItem.Text + " es invalida.\\nRevise Por Favor!");
                return (false);
            }
            return (true);
        }

        //BORRAR GRILLA ITEMS
        protected void BorrarGrilla()
        {
            dtInserts.Clear();
            dgItems.DataSource = dtInserts;
            dgItems.DataBind();
            Session.Remove("dtInsertsCP");
            BindDatas();
        }

        protected bool CheckValues(DataGridCommandEventArgs e)
        {
            bool check = true;
            if (((TextBox)e.Item.Cells[0].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text))
                check = false;
            else if (Tipo == "P" && !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text))
                    check = false;
            else if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text) || Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("edit_precioc")).Text) == 0)
                    check = false;
            else if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].FindControl("tbfdesc")).Text) || Convert.ToDouble(((TextBox)e.Item.Cells[6].FindControl("tbfdesc")).Text) > 100)
                    check = false;
            return check;
        }

        protected void CargarDatosPedido()
        {
            DataSet dsPedido = new DataSet();
            DBFunctions.Request(dsPedido, IncludeSchema.NO, "select * from MPEDIDOITEM mp where mp.PPED_CODIGO='" + Request.QueryString["prefPed"] + "' and mp.MPED_NUMEPEDI=" + Request.QueryString["numePed"]);
            string nombreNIT = DBFunctions.SingleData("select nombre  from vmnit where mnit_nit='" + dsPedido.Tables[0].Rows[0]["MNIT_NIT"].ToString() + "'");

            InterfazModificacion();

            ddlCodigo.SelectedValue = Request.QueryString["prefPed"];
            txtNumPed.Text = Request.QueryString["numePed"];
            ddlAlmacen.SelectedValue = dsPedido.Tables[0].Rows[0]["PALM_ALMACEN"].ToString();
            llenarVendedores();
            txtNIT.Text = dsPedido.Tables[0].Rows[0]["MNIT_NIT"].ToString();
            txtNITa.Text = nombreNIT;
            txtObs.Text = dsPedido.Tables[0].Rows[0]["MPED_OBSERVACION"].ToString();
            DateTime fechaPedido = Convert.ToDateTime(dsPedido.Tables[0].Rows[0]["MPED_PEDIDO"].ToString());
            tbDate.Text = fechaPedido.ToString("yyyy-MM-dd");
            ddlVendedor.SelectedValue = dsPedido.Tables[0].Rows[0]["PVEN_CODIGO"].ToString();

            DBFunctions.Request(dsDetallePedido, IncludeSchema.NO,
                            "select mi.mite_codigo, mi.mite_nombre, mi.plin_codigo, dp.dped_cantpedi as mite_cantidad, dp.dped_cantasig as mite_cantasig, dp.dped_valounit as mite_precioinicial, " +
                            "dp.dped_valounit as mite_preciopromedio, dp.dped_valounit as mite_precio, dp.piva_porciva as mite_iva, dp.dped_porcdesc as mite_desc, '0' as mite_tot  from DPEDIDOITEM dp, mitems mi " +
                            "where mi.mite_codigo=dp.mite_codigo and dp.PPED_CODIGO='" + Request.QueryString["prefPed"] + "' and dp.MPED_NUMEPEDI=" + Request.QueryString["numePed"]);

            dgItems.DataSource = dsDetallePedido;
            Session["dsDetallePedido"] = dsDetallePedido;
            dgItems.ShowFooter = false;
            dgItems.Columns[12].Visible = false;
            dgItems.Columns[10].Visible = false;
            dgItems.DataBind();
        }

        protected void InterfazModificacion()
        {
            ddlCodigo.Enabled = false;
            txtNumPed.Enabled = false;
            ddlAlmacen.Enabled = false;
            plListaPrecios.Visible = false;
            txtNIT.Enabled = false;
            txtNITa.Enabled = false;
            imglupa1.Visible = false;
            txtObs.Enabled = false;
            tbDate.Enabled = false;
            plcEstadoCliente.Visible = false;
            plcTotales.Visible = false;
            plcVendedor.Visible = false;
            ddlVendedor.Enabled = false;
            tbClaveVend.Enabled = false;
            btnAjus.Visible = false;
            btnAjusFac.Visible = false;
            btnActualizar.Visible = true;
        }

        protected void ActualizarPedido(Object Sender, EventArgs E)
        {
            ArrayList itemsEliminar = new ArrayList();
            if (Session["itemsEliminar"] != null)
            {
                itemsEliminar = (ArrayList)Session["itemsEliminar"];
                string[] fechaPedido = tbDate.Text.Split('-'); //yyyy-mm-dd
                ArrayList sqlUpdate = new ArrayList();

                for (int g = 0; g < itemsEliminar.Count; g++)
                {
                    string[] itemInfo = itemsEliminar[g].ToString().Split('@'); //codigoItem @ cantidadAsignada

                    sqlUpdate.Add("INSERT INTO MHISTORIAL_CAMBIOS( TABLA, OPERACION, ORIGINALES, SUSU_USUARIO, FECHA) " +
                                  "VALUES('DPEDIDOITEM', 'D', '', '" + HttpContext.Current.User.Identity.Name.ToLower() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                    sqlUpdate.Add("update MDEMANDAITEM	        set	mdem_cantidad=mdem_cantidad - " + itemInfo[1] + " WHERE mite_codigo='" + itemInfo[0] + "' AND pano_ano=" + fechaPedido[0] + " and pmes_mes=" + fechaPedido[1]);
                    sqlUpdate.Add("update MDEMANDAITEMALMACEN   set mdem_cantidad=mdem_cantidad - " + itemInfo[1] + " WHERE mite_codigo='" + itemInfo[0] + "' AND pano_ano=" + fechaPedido[0] + " and pmes_mes=" + fechaPedido[1] + " and palm_almacen='" + ddlAlmacen.SelectedValue + "'");
                    sqlUpdate.Add("update macumuladoitem        set macu_cantidad=macu_cantidad - " + itemInfo[1] + " WHERE pano_ano=" + fechaPedido[0] + " and pmes_mes=" + fechaPedido[1] + " and tmov_tipomovi=70 AND mite_codigo = '" + itemInfo[0] + "'");
                    sqlUpdate.Add("update macumuladoitemalmacen set macu_cantidad=macu_cantidad - " + itemInfo[1] + " WHERE pano_ano=" + fechaPedido[0] + "	and	pmes_mes=" + fechaPedido[1] + " and tmov_tipomovi=70 AND mite_codigo = '" + itemInfo[0] + "' AND PALM_ALMACEN='" + ddlAlmacen.SelectedValue + "'");
                    sqlUpdate.Add("update msaldoitem            set msal_PEDIPENDI=msal_PEDIPENDI - 1, msal_UNIDPENDI=msal_UNIDPENDI - " + itemInfo[1] + ", msal_cantasig=msal_cantasig - " + itemInfo[2] + " where mite_codigo='" + itemInfo[0] + "' and pano_ano=" + fechaPedido[0]);
                    sqlUpdate.Add("UPDATE msaldoitemalmacen     SET msal_CANTPENDIENTE=msal_cantPENDIENTE - " + itemInfo[1] + ", msal_cantasig=msal_cantasig - " + itemInfo[2] + " WHERE mite_codigo='" + itemInfo[0] + "' AND pano_ano=" + fechaPedido[0] + " AND palm_almacen='" + ddlAlmacen.SelectedValue + "'");
                    sqlUpdate.Add("DELETE FROM DLISTAEMPAQUE    WHERE  MITE_CODIGO = '" + itemInfo[0] + "' AND   PPED_CODIGO = '" + ddlCodigo.SelectedValue + "' AND   MPED_NUMEPEDI = " + txtNumPed.Text);
                    sqlUpdate.Add("DELETE FROM DPEDIDOITEM      WHERE  MNIT_NIT = '" + txtNIT.Text + "' AND   PPED_CODIGO = '" + ddlCodigo.SelectedValue + "' AND   MPED_NUMEPEDI = " + txtNumPed.Text + " AND   MITE_CODIGO = '" + itemInfo[0] + "'");
               }

               if (DBFunctions.Transaction(sqlUpdate))
               {
                    string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
                    Session.Clear();
                    Response.Redirect("" + indexPage + "?process=Inventarios.ModificarPedidos&path=Modificar Pedidos&pref=" + ddlCodigo.SelectedValue + "&num=" + txtNumPed.Text);
               }
            }
        }

        #endregion

        #region Ajax Methods

        [Ajax.AjaxMethod()]
        public string ConsultarNombreCliente(string nitCliente)
        {
            return DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT COALESCE(mnit_apellido2,'') CONCAT ' ' CONCAT mnit_nombres CONCAT ' ' CONCAT COALESCE(mnit_nombre2,'') FROM mnit WHERE mnit_nit='" + nitCliente + "'");
        }

        [Ajax.AjaxMethod()]
        public string ConsultarSustitucion(string itmO, string linea)
        {
            string susItm = "";
            string codI = "";
            Referencias.Guardar(itmO, ref codI, linea);
            //   susItm = DBFunctions.SingleData("SELECT msus_codACTUAL FROM MSUSTITUCION WHERE msus_codANTERIOR='" + itmO + "'");
            susItm = DBFunctions.SingleData("SELECT DBXSCHEMA.EDITARREFERENCIAS(msus_codACTUAL, '" + linea + "') FROM MSUSTITUCION WHERE msus_codANTERIOR='" + codI + "' ");
            if (susItm.Length > 0)
                return (susItm);
            else
                return (itmO);
        }

        [Ajax.AjaxMethod()]
        public string ConsultarOrigenItem(string idItem)
        {
            string origenItem = DBFunctions.SingleData("select tori_codigo from dbxschema.mitems where mite_codigo='" + idItem + "';");

            return (origenItem);
        }

        [Ajax.AjaxMethod()]
        public string ConsultarDescuentoCliente(string nitCliente)
        {
            return DBFunctions.SingleData("SELECT mcli_porcdescINV FROM mcliente WHERE mnit_nit='" + nitCliente + "'");
        }

        [Ajax.AjaxMethod()]
        public string ConsultarDescuentoClienteMayor(string nitCliente)
        {
            //if (Session["tipoPedido"].ToString() == "T")
            //    return "0";
            return DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + nitCliente + "'");
        }

        [Ajax.AjaxMethod()]
        public string ConsultarCupoCliente(string nitCliente)
        {
            //if (Session["tipoPedido"].ToString() == "T")
            //    return "0";
            string cupo = ConsultarCupo(nitCliente).ToString("#,##0");
            return cupo;
        }

        [Ajax.AjaxMethod()]
        public string ConsultarSaldoCliente(string nitCliente)
        {
            //if (Session["tipoPedido"].ToString() == "T")
            //    return "0";

            string saldo = Utilidades.Clientes.ConsultarSaldo(nitCliente).ToString("#,##0");
            return saldo;
        }

        [Ajax.AjaxMethod()]
        public string ConsultarSaldoMoraCliente(string nitCliente)
        {
            //if (Session["tipoPedido"].ToString() == "T")
            //    return "0";

            string saldo = Utilidades.Clientes.ConsultarSaldoMora(nitCliente).ToString("#,##0");
            return saldo;
        }

        [Ajax.AjaxMethod()]
        public string ConsultarUsoXVehiculo(string prefijoOT, string numOT, string codigo, string linea)
        {
            string codI = "";
            Referencias.Guardar(codigo, ref codI, linea);
            string grupoVehiculo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT MCV.pcat_codigo FROM morden MO, MCATALOGOVEHICULO MCV WHERE MCV.MCAT_VIN = MO.MCAT_VIN  AND MO.pdoc_codigo='" + prefijoOT + "' AND MO.mord_numeorde=" + numOT + ")");
            return DBFunctions.SingleData("SELECT DBXSCHEMA.USOXVEH(mite_codigo,'','" + grupoVehiculo + "') FROM dbxschema.mitems WHERE mite_codigo='" + codI + "'");
        }

        [Ajax.AjaxMethod()]
        public string ConsultarNombreItem(string codigo, string linea)
        {
            string salida = "";
            string codI = "";
            Referencias.Guardar(codigo, ref codI, linea);
            salida = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='" + codI + "'");
            if (salida == "")
                codigo = "";
            return salida;
        }

        [Ajax.AjaxMethod]
        public string ConsultarPrecioItem(string codigo, string linea, string tipoCli, string tipPed, string lista, string almacen, string pedido, bool prod, string tipCarg)
        {
            string valor = "";
            string codI = "";
            string nNacionalidad = "N";
            //   string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");

            DataSet cInventarioX = new DataSet();
            DBFunctions.Request(cInventarioX, IncludeSchema.NO, "select CI.*,CE.CEMP_LIQUDECI, ce.mnit_nit  from cinventario CI, CEMPRESA CE");
            int numDecimales = 0;
            if(cInventarioX.Tables[0].Rows[0]["CINV_NUMEDECI"].ToString().Length > 0)
                numDecimales = Convert.ToInt16(cInventarioX.Tables[0].Rows[0]["CINV_NUMEDECI"]); 
            int year = Convert.ToInt16(cInventarioX.Tables[0].Rows[0]["PANO_ANO"]);
            Referencias.Guardar(codigo, ref codI, linea);
            double precio = 0, costoPromedio = 0;
            if (tipoCli == "C" || tipoCli == "M")
            {
                try { precio = Convert.ToDouble(DBFunctions.SingleData("SELECT mpre_precio FROM MPRECIOITEM WHERE mite_codigo='" + codI + "' AND ppre_codigo='" + lista + "'")); }
                catch { };
                try { costoPromedio = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + year)); }
                catch { };
            }
            else
            {
                try { precio = Convert.ToDouble(DBFunctions.SingleData("SELECT MITE_COSTREPO FROM MITEMS WHERE mite_codigo='" + codI + "' ")); }
                catch { };
                try { precio = precio - (precio * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + pedido + "'")) / 100)); }
                catch { }
                try { precio = precio + (precio * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procreca FROM ppedido WHERE pped_codigo='" + pedido + "'")) / 100)); }
                catch { }
                try { costoPromedio = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + year)); }  // MSALDOitemALMACEN -- AND PALM_ALMACEN='" + almacen + "'  SE TOMA EL COSTO PROMEDIO DE LA EMPRESA
                catch { };
            }

            if (tipoCli == "C" || tipoCli == "M")
            {
                /////////////Esta parte solo aplica para los pedidos de clientes ////////////////////////////
                if (tipPed == "E")
                {
                    try { precio = costoPromedio + (costoPromedio * (Convert.ToDouble(cInventarioX.Tables[0].Rows[0]["TRES_FACTORINTERNO"].ToString()) / 100)); }
                    catch { }
                }
                else if (tipPed == "G")
                {
                    try { precio = costoPromedio + (costoPromedio * (Convert.ToDouble(cInventarioX.Tables[0].Rows[0]["TRES_FACTORGARANTIA"].ToString()) / 100)); }
                    catch { }
                }
                else if (tipPed == "P")
                {
                    try { precio = precio - (precio * (Convert.ToDouble(DBFunctions.SingleData("SELECT pped_procdesc FROM ppedido WHERE pped_codigo='" + pedido + "'")) / 100)); }
                    catch { }
                }
                else if (tipPed == "T")
                {
                    if (tipCarg == "I" || tipCarg == "A" || tipCarg == "T") //Alistamiento, Interno, Grantia Taller
                    {
                        try { precio = costoPromedio + (costoPromedio * (Convert.ToDouble(cInventarioX.Tables[0].Rows[0]["TRES_FACTORINTERNO"].ToString()) / 100)); }
                        catch { }
                    }
                    else if (tipCarg == "G") //Garantia Fabrica
                    {
                        try { precio = costoPromedio + (costoPromedio * (Convert.ToDouble(cInventarioX.Tables[0].Rows[0]["TRES_FACTORGARANTIA"].ToString()) / 100)); }
                        catch { }
                    }
                    else if (tipCarg == "P") //Produccion
                    {
                        precio = costoPromedio;
                    }
                    else //Cliente o seguro
                    {
                        if (precio == 0)
                            precio = costoPromedio + (costoPromedio * (Convert.ToDouble(cInventarioX.Tables[0].Rows[0].Table.Rows[0]["CINV_FACTORUTILIDAD"].ToString()) / 100));
                        else if (tipoCli != "M" && precio < (costoPromedio + (costoPromedio * (Convert.ToDouble(cInventarioX.Tables[0].Rows[0]["CINV_FACTORUTILIDAD"].ToString()) / 100))))
                                precio = -1;
                    }

                    //if (precio == 0)
                    //    precio = costoPromedio + (costoPromedio * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario")) / 100));
                    //else if (precio < (costoPromedio + (costoPromedio * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario")) / 100))))
                    //    precio = -1;
                }
                else
                {
                    if (precio == 0)
                    { }  //precio = costoPromedio + (costoPromedio * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario")) / 100));
                    //else if (precio != 0 && tipoCli != "M" && precio < (costoPromedio + (costoPromedio * (Convert.ToDouble(DBFunctions.SingleData("SELECT cinv_factorutilidad FROM cinventario")) / 100))))
                    else if (precio != 0 && tipoCli != "M" && precio < costoPromedio)
                            precio = -1;
                }
            }
            else if (tipoCli == "P")
            {
                if (precio == 0)
                    precio = costoPromedio;
            }
            if (prod && precio == 0) valor = (Math.Round(costoPromedio, numDecimales)).ToString();
            //	else 
            //     numDecimales = Convert.ToInt32(ViewState["num_Decimales"].ToString());
            if (tipoCli != "P" && nNacionalidad != "E")
                valor = Math.Round(precio, numDecimales).ToString();
            else
                valor = precio.ToString();
            return valor;
        }

        [Ajax.AjaxMethod]
        public string AsignarPrecioMinimoReferencia(string codigo, string linea, string tipoCli, string tipPed, string lista, string almacen, string accion)
        {
            double precio = 0, costoPromedio = 0;// cantActual = 0;
            string cadenaResultado = "", cantActual = "";
            string codI = "";
            DataSet cInventarioX = new DataSet();
            DBFunctions.Request(cInventarioX, IncludeSchema.NO, "select * from cinventario");
            int year = Convert.ToInt16(cInventarioX.Tables[0].Rows[0]["PANO_ANO"]);
            Referencias.Guardar(codigo, ref codI, linea);
            double porcUtil = Convert.ToDouble(cInventarioX.Tables[0].Rows[0]["CINV_FACTORUTILIDAD"].ToString());
            //try{costoPromedio=Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='" +codI+"' AND PALM_ALMACEN='"+almacen+"' AND PANO_ANO="+ano_cinv));}

            if (accion == "PRECIO_REF")
            {
                try { costoPromedio = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + year)); }
                catch { }
                precio = costoPromedio + (costoPromedio * (porcUtil / 100));
                cadenaResultado = "P.M.V  : " + precio.ToString("C") + "<br>C.Prm " + costoPromedio.ToString("C");
            }
            else if (accion == "CANT_ACTUAL")
            {
                //try { cantActual = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM MSALDOITEM WHERE MITE_CODIGO='" + codI + "' AND PANO_ANO=" + year)); }
                try { cantActual = Convert.ToString(DBFunctions.SingleData(@"SELECT case when MSAL_CANTASIG > 0 then  varchar_format (msal_cantactual,999) || ' Asg - ' || MSAL_CANTASIG || ' Disp - ' || SUM (MSAL_CANTACTUAL - MSAL_CANTASIG) 
                                                else varchar_format(msal_cantactual, 999) end AS CANTIDAD FROM MSALDOITEM WHERE MITE_CODIGO = '" + codI + @"' AND PANO_ANO ="+year+" GROUP BY msal_cantactual, MSAL_CANTASIG;")); }

                catch { }
                cadenaResultado = "Act: " + cantActual;
            }

            return cadenaResultado;
        }

        #endregion

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion
    }
}
