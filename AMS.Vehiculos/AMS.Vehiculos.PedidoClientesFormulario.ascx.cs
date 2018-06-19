// created on 25/01/2005 at 16:44
using Ajax;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Timers;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace AMS.Vehiculos
{
    public partial class PedidoClientesFormulario : System.Web.UI.UserControl
    {
        #region   Atributos
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected DataTable tablaElementos, tablaRetoma;
        protected Label diferencia;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"], proceso;
        protected double porcentajeIva, precioVehMinimo, porcentajeDtoMax, precioOficial;
        protected string catalogoComp;
        string    strDatosClienteA, strDatosClienteB, strDatosClienteC;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hddatosClienteA;
        protected System.Web.UI.HtmlControls.HtmlInputHidden hddatosClienteB;
        DataSet   dsColores;
        bool      grabando;
        String    mcatvin;
        String    esCasaMatriz = "";
        DatasToControls bind = new DatasToControls();
        #endregion

        #region   Eventos

        protected void Page_Load(object sender, System.EventArgs e)
        {
            
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Vehiculos.PedidoClientesFormulario));
            //this.ClearChildViewState();
            proceso = Request.QueryString["proc"];
            

            if (!IsPostBack)
            {
                

                valorDescuento.Text = hdDescuentos.Value = "0";
                plInfoVent.Visible = plInfoPago.Visible = false;
                if (Request.QueryString["Precio"] != null)
                {
                    valorVehiculo.ReadOnly = false;
                    valorVehiculo.Enabled = true;
                }

                Session.Clear();
                
                //Preparamos los datos de que son parte de la informacion especial del pedido
                bind.PutDatasIntoDropDownList(prefijoDocumento, string.Format(Documento.DOCUMENTOSTIPO, "PC"));
                if (prefijoDocumento.Items.Count > 1)
                {
                    prefijoDocumento.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    numeroPedido.Text = "";
                }
                else
                    numeroPedido.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "'");
                bind.PutDatasIntoDropDownList(tipoVehiculo, "SELECT tcla_clase, tcla_nombre FROM tclasevehiculo ORDER BY tcla_clase");
                
                if (tipoVehiculo.Items.Count > 1)
                    tipoVehiculo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                
                // Esta linea carga por defecto los vehiculos usados que tienen por codigo U
                //tipoVehiculo.SelectedValue == "U";
                                 
                

                if (Request.QueryString["compra"] == "1")
                {
                    tipoVehiculo.SelectedValue = Request.QueryString["tipoVeh"];
                    
                    if (Request.QueryString["numCot"] != null && Request.QueryString["prfCot"] != null)
                    {
                        lblCotizacion.Visible = true;
                        lblCotizacion.Text = "    _PEDIDO CON COTIZACIÓN: " + Request.QueryString["prfCot"] + "-" + Request.QueryString["numCot"];

                        FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                        formatoRecibo.Prefijo = Request.QueryString["prfCot"];
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prfCot"] + "'");
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numCot"].ToString());
                        if (formatoRecibo.Cargar_Formato())
                            Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                    }
                }

                Cambio_Tipo_Vehiculo(sender,e);

                //bind.PutDatasIntoDropDownList(anoModelo, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");
                //if (anoModelo.Items.Count > 1)
                //    anoModelo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                bind.PutDatasIntoDropDownList(ddlFinanciera, "Select P.mnit_nit,M.mnit_apellidos from DBXSCHEMA.PFINANCIERA P,dbxschema.mnit M where p.mnit_nit=M.mnit_nit ORDER BY M.mnit_apellidos");
                if (ddlFinanciera.Items.Count > 1)
                    ddlFinanciera.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                bind.PutDatasIntoDropDownList(ddlServicio, "SELECT * from tserviciovehiculo ORDER BY tser_tiposerv");
                if (ddlServicio.Items.Count > 1)
                    ddlServicio.Items.Insert(0, new ListItem("Seleccione...", string.Empty));

                dsColores = new DataSet();
                if (proceso != "modi")
                    DBFunctions.Request(dsColores, IncludeSchema.NO, Colores.COLORES);
                else
                    DBFunctions.Request(dsColores, IncludeSchema.NO, Colores.COLORESTODOS);
                colorPrimario.DataValueField = "pcol_codigo";
                colorPrimario.DataTextField = "descripcion";
                colorPrimario.DataSource = dsColores.Tables[0];
                colorPrimario.DataBind();
                colorOpcional.DataValueField = "pcol_codigo";
                colorOpcional.DataTextField = "descripcion";
                colorOpcional.DataSource = dsColores.Tables[0];
                colorOpcional.DataBind();
                EstablecerColores();
                colorPrimario.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                DatasToControls.EstablecerValueDropDownList(colorPrimario, string.Empty);
                colorOpcional.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                DatasToControls.EstablecerValueDropDownList(colorOpcional, string.Empty);

                
                if(Request.QueryString["cod_vend"] != null)
                {
                    bind.PutDatasIntoDropDownList(almacen, "SELECT PALM_ALMACEN FROM PVENDEDORALMACEN WHERE PVEN_CODIGO = '" + Request.QueryString["cod_vend"] + "'");
                    almacen.Attributes.Remove("OnSelectedIndexChanged");
                    //almacen.Attributes.Remove("AutoPostBack");
                    almacen.AutoPostBack = false;
                    almacen.Attributes.Add("AutoPostBack", "false");
                    bind.PutDatasIntoDropDownList(vendedor, "SELECT PVEN_CODIGO,PVEN_NOMBRE FROM PVENDEDOR WHERE PVEN_CODIGO = '" + Request.QueryString["cod_vend"] + "'");
                }
                else
                {
                    bind.PutDatasIntoDropDownList(almacen, 
                        @"SELECT distinct pa.palm_almacen, palm_descripcion 
                            FROM palmacen pa, pvendedoralmacen pva, PVENDEDOR PV 
                           WHERE (pcen_centvvn is not null or pcen_centvvu is not null) and TVIG_VIGENCIA = 'V' and pa.palm_almacen = pva.palm_almacen and pva.pven_codigo = pv.pven_codigo and pven_vigencia = 'V' AND PV.TVEND_CODIGO IN ('VV','TT')
                        ORDER BY palm_descripcion ; ");
                    bind.PutDatasIntoDropDownList(vendedor, string.Format(Vendedores.VENDEDORESVEHICULOSALMACEN, almacen.SelectedValue));
                    //almacen.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    vendedor.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                }
                if(almacen.Items.Count > 1)
                    almacen.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                //vendedor.Items.Insert(0, new ListItem("Seleccione...", string.Empty));

                if (Request.QueryString["compra"] == "1")
                {
                    string vendedorX = Request.QueryString["vendedor"];
                    almacen.SelectedValue = DBFunctions.SingleData("SELECT PALM_ALMACEN FROM PVENDEDORALMACEN WHERE PVEN_CODIGO = '" + vendedorX + "' fetch first 1 rows only; ");

                }

                bind.PutDatasIntoDropDownList(tipoVenta, "SELECT ptipo_coditipoventa, ptipo_descrventa FROM ptipoventavehiculo ORDER BY ptipo_descrventa");
                if (tipoVenta.Items.Count > 1)
                    tipoVenta.Items.Insert(0, new ListItem("Seleccione...", string.Empty));

                bind.PutDatasIntoDropDownList(claseVenta, "SELECT pclas_codigoventa, pclas_ventadescrip FROM pclaseventavehiculo ORDER BY pclas_ventadescrip");
                if (claseVenta.Items.Count > 1)
                    claseVenta.Items.Insert(0, new ListItem("Seleccione...", string.Empty));


                // try { bind.PutDatasIntoDropDownList(listaUsados, "SELECT mcat_vin FROM mvehiculo WHERE tcla_codigo='U' AND pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' ORDER BY mcat_vin"); }
                //try { bind.PutDatasIntoDropDownList(listaUsados, "SELECT mv.mcat_vin FROM mvehiculo mv, mcatalogovehiculo mc WHERE tcla_codigo='U' and mv.mcat_vin = mc.mcat_vin AND mc.pcat_codigo='" + catalogoVehiculo.SelectedValue.ToString() + "' ORDER BY mcat_vin"); }
                //catch { }
                if (DateTime.Now.Month > 10)
                    DatasToControls.EstablecerDefectoDropDownList(anoModelo, DateTime.Now.AddYears(1).Year.ToString().Trim());
                else
                    DatasToControls.EstablecerDefectoDropDownList(anoModelo, DateTime.Now.Year.ToString().Trim());
                
                fecha.SelectedDate = DateTime.Now;
                fechaEntrega.SelectedDate = DateTime.Now.AddDays(8);
                disp.Attributes.Remove("OnClick");

                //disp.Attributes.Add("OnClick", "ModalDialog(this,'Select * from DBXSCHEMA.VVEHICULOS_DISPONIBILIDAD where pcat_codigo=\\'" + catalogoVehiculo.SelectedValue.ToString() + "\\' ' ,new Array() );");

                if (proceso != "modi")
                {
                    fecha.SelectedDate = DateTime.Now;
                    fechaEntrega.SelectedDate = DateTime.Now.AddDays(8);
                }
                Preparar_Tabla_Elementos();
                Binding_Grilla();
                Preparar_Tabla_Retoma();
                Binding_Grilla_Retoma();
                if (proceso == "modi")
                {
                    //que reinicie el descuento en 0
                    valorDescuento.Text = "0";
                    this.Cargar_Pedido_Existente(Request.QueryString["pref"], Request.QueryString["nume"]);

                    lnkConsultarAbonos.Visible = true;
                    if (Request.QueryString["orig"] != null)
                    {
                        plInfoPed.Visible = plInfoVent.Visible = false;
                        plInfoPago.Visible = true;
                    }
                }
            }
            else
            {
                if (Request.QueryString["compra"] != "1")
                     valorDescuento.Text = "0";
                if (Session["tablaElementos"] == null)
                {
                    this.Preparar_Tabla_Elementos();
                    this.Binding_Grilla();
                }
                else
                {
                    tablaElementos = (DataTable)Session["tablaElementos"];
                    Session["tablaElementos"] = tablaElementos;
                }
                if (Session["tablaRetoma"] == null)
                {
                    this.Preparar_Tabla_Retoma();
                    this.Binding_Grilla_Retoma();
                }
                else
                {
                    tablaRetoma = (DataTable)Session["tablaRetoma"];
                    Session["tablaRetoma"] = tablaRetoma;
                }
                if (Session["precioVehMinimo"] == null)
                    precioVehMinimo = 0;
                else
                {
                    precioVehMinimo = (double)Session["precioVehMinimo"];
                    Session["precioVehMinimo"] = precioVehMinimo;
                }
            }
            if (catalogoVehiculo.Items.Count > 0 && catalogoVehiculo.SelectedValue != "Seleccione..." && catalogoVehiculo.SelectedValue != "")
            {
                string cadena = string.Empty;

                cadena = DBFunctions.SingleData("SELECT piva_porciva FROM pcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue + "'").Trim();

                if (cadena == string.Empty && tipoVehiculo.SelectedValue == "N")
                    Utils.MostrarAlerta(Response, "No se ha configurado el porcentaje de IVA para este catálogo de vehículos!");
                else if (tipoVehiculo.SelectedValue == "N")
                        porcentajeIva = Convert.ToDouble(cadena);

                cadena = DBFunctions.SingleData("SELECT ppre_dtomax FROM PPRECIOVEHICULO WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue + "' AND PANO_ANO = '" + anoModelo.SelectedValue + "'").Trim();

                if (cadena == string.Empty && tipoVehiculo.SelectedValue == "N")
                    Utils.MostrarAlerta(Response, "No se ha configurado el porcentaje máximo de descuento para este catálogo de vehículos!");
                else if (tipoVehiculo.SelectedValue == "N")
                        porcentajeDtoMax = Convert.ToDouble(cadena);
            }
            else if (catalogoVehiculo.Items.Count == 0 && catalogoVehiculo.SelectedValue != "Seleccione..." && catalogoVehiculo.SelectedValue != "")
            {
                    Utils.MostrarAlerta(Response, "No se ha configurado el precio para este catálogo de vehículos !! " + catalogoVehiculo.SelectedValue.ToString());
            }
            strDatosClienteA = datosCliente.Text;
            strDatosClienteB = datosClienteAlterno.Text;
            strDatosClienteC = datosClienteSolicita.Text;

            if (Request.QueryString["compra"] == "1" && !IsPostBack)
            {
                string sqlmnit = "";
                sqlmnit = DBFunctions.SingleData("Select mnit_nit from mnit where mnit_nit = '" + Request.QueryString["nitCliente"] + "'");
                //Si pasa de la cotización al pedido, Se agrega el registro a la tabla MNIT y se borra de MNITCOTIZACION.

                if (sqlmnit == "" || sqlmnit == null)
                {
                    DBFunctions.NonQuery(@"INSERT INTO MNIT SELECT * FROM MNITCOTIZACION WHERE MNIT_NIT = '" + Request.QueryString["nitCliente"] + "'");
                    DBFunctions.NonQuery(@"DELETE FROM MNITCOTIZACION WHERE MNIT_NIT = '" + Request.QueryString["nitCliente"] + "'");

                }
                // aqui debe traer todos los datos de la cotizacion // financiera, valorfinanciado, vendedor, sede 

                tipoVehiculo.SelectedValue     = Request.QueryString["tipoVeh"];
                string catalogoVehiculoX       = Request.QueryString["catalogo"];
                catalogoVehiculoX              = catalogoVehiculoX.Replace(" ","+");
                catalogoVehiculo.SelectedValue = catalogoVehiculoX;
                colorPrimario.SelectedValue    = Request.QueryString["color"];
                colorOpcional.SelectedValue    = Request.QueryString["color"];
                bind.PutDatasIntoDropDownList(anoModelo, "SELECT pano_ano FROM pano WHERE pano_ano = " + Request.QueryString["modelo"]);
                anoModelo.SelectedValue        = Request.QueryString["modelo"];
                ddlServicio.SelectedValue      = Request.QueryString["tipoVenta"];
                claseVenta.SelectedValue       = Request.QueryString["claseVenta"];  // pclaseventavehiculo
                //tipoVenta.SelectedValue      = Request.QueryString["claseVenta"];  // ptipoventavehiculo
                string vendedorX               = Request.QueryString["vendedor"];
                almacen.SelectedValue          = DBFunctions.SingleData("SELECT PALM_ALMACEN FROM PVENDEDORALMACEN WHERE PVEN_CODIGO = '"+ vendedorX +"' fetch first 1 rows only; ");
                
                Cambio_Almacen(sender, e);
                vendedor.SelectedValue         = vendedorX;
                almacen.Enabled                = false;
                vendedor.Enabled               = false;
                txtPrecio.Text                 = Convert.ToDouble(Request.QueryString["precioVenta"].ToString()).ToString("C");
                valorDescuento.Text            = Request.QueryString["valorDescuento"];
                recordDescuento.Text           = Request.QueryString["valorDescuento"];
                datosCliente.Text              = Request.QueryString["nitCliente"];
                datosClienteAlterno.Text       = Request.QueryString["nitCliente"];
                datosClienteSolicita.Text      = Request.QueryString["nitCliente"];
                datosClientea.Text             = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '"+ datosCliente.Text +"'; ");
                datosClienteAlternoa.Text      = datosClientea.Text;
                datosClienteSolicitaa.Text     = datosClientea.Text;
                datosCliente.ReadOnly          = true;
                datosClienteAlterno.ReadOnly   = true;
                datosClienteSolicita.ReadOnly  = true;
                txtEmail.Text                  = Request.QueryString["email"];
                txtEmaila.Text                 = Request.QueryString["email"];
                txtEmails.Text                 = Request.QueryString["email"];
                if (Request.QueryString["opcionVeh"] != null)
                {
                    //DatasToControls bind = new DatasToControls();
                    bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select distinct po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + catalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI;");
                    if (ddlOpcionVeh.Items.Count >= 1)
                    {
                        lblOpcionV.Visible = true;
                        ddlOpcionVeh.Visible = true;
                        //ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                        ddlOpcionVeh.SelectedValue = Request.QueryString["opcionVeh"].ToString();
                        ddlOpcionVeh.Enabled = false;
                    }
                }
            }
            //Habeas Data
            
            if (Request.QueryString["nitCli"] != null && Request.QueryString["nombreCli"] != null) // && Request.QueryString["idDBCli"] != null
            {
                string fecha1 = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                string fecha2 = DateTime.Now.ToString("yyyy-MM-dd");

                //si esta persona ya le hicieron el Habeas data en un rango de 6 meses a partir de hoy, no es necesario volverle a hacer el Habeas data
                if (DBFunctions.RecordExist("SELECT MB.MNIT_NIT, PB.PBAS_DESCRIPCION FROM MBASEDATOS MB, PBASEDATOS PB WHERE MB.MNIT_NIT = '" + Request.QueryString["nitCli"] + "' AND PB.TBAS_CODIGO = 'FV' AND MB.PBAS_CODIGO = PB.PBAS_CODIGO AND MBAS_FECHA BETWEEN '" + fecha1 + "' AND '" + fecha2 + "';")) // SELECT MNIT_NIT FROM MBASEDATOS WHERE MNIT_NIT = '" + Request.QueryString["nitCli"] + "' AND MBAS_FECHA BETWEEN '" + fecha1 + "' AND '" + fecha2 + "'"))
                {
                    return;
                }
                Session["tipoDB"] = "FV"; // Request.QueryString["idDBCli"];
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


        private void EstablecerColores()
        {
            for (int n = 0; n < colorPrimario.Items.Count; n++)
            {
                colorPrimario.Items[n].Attributes.Add("style", "background-color:" + dsColores.Tables[0].Rows[n]["pcol_codrgb"].ToString());
                colorOpcional.Items[n].Attributes.Add("style", "background-color:" + dsColores.Tables[0].Rows[n]["pcol_codrgb"].ToString());
            }
        }

        [Ajax.AjaxMethod]
        public DataSet Cargar_Nombre(string Cedula)
        {
            DataSet Vins = new DataSet();
            DBFunctions.Request(Vins, IncludeSchema.NO, "select mnit_nit from dbxschema.mnit where mnit_nit like '" + Cedula + "%';select mnit_nombres concat ' ' CONCAT COALESCE(mnit_nombre2,'') concat ' 'concat mnit_apellidos concat ' 'concat COALESCE(mnit_apellido2,'') as NOMBRE, MNIT_EMAIL AS EMAIL from dbxschema.mnit where mnit_nit='" + Cedula + "'");
            
            return Vins;

        }
         
        protected void EjecutarAccion(object Sender, EventArgs e)
        {
            double valorbase = double.Parse(hdValorbase.Value);
            //Revisamos que no exista diferencia entre la configuración de pagos y el valor total de la venta
            string respsino;

            try { diferenciaPagos.Text = (Convert.ToDouble(totalVenta.Text.Substring(1)) - Convert.ToDouble(valorTotalPagos.Text.Substring(1))).ToString("C"); }
            catch { }

            if (diferenciaPagos.Text.Substring(0, 1) == "$" && Convert.ToDouble(diferenciaPagos.Text.Substring(1)) == 0)
            {
                ((Button)btnAccion).Enabled = false;

                if (rblcredito.SelectedValue == "Cliente")
                {
                    respsino = "N";
                }
                else
                {
                    respsino = "S";
                }

                if ((pagoFinanciera.Text != "" && pagoFinanciera.Text != "0") && ddlFinanciera.SelectedValue == "Seleccione...")
                {
                    Utils.MostrarAlerta(Response, "Por Favor Ingrese el nombre de la financiera!");
                    return;
                }
                if (valorOtroPago.Text != "0" && valorOtroPago.Text != "0.00" && valorOtroPago.Text != "" && otroPago.Text == "")
                {
                    Utils.MostrarAlerta(Response, "Por Favor Ingrese el detalle de los otros pagos!");
                    return;
                }

                if (grabando == true)
                    return;
                else
                    grabando = true;

                btnAccion.Attributes.Add("OnClick", "Enabled = false");

                PedidoCliente miPedidoCliente = new PedidoCliente(tablaElementos, tablaRetoma);
                miPedidoCliente.CodigoPrefijo = prefijoDocumento.SelectedValue;
                miPedidoCliente.NumeroPedido  = numeroPedido.Text;
                miPedidoCliente.Catalogo      = catalogoVehiculo.SelectedValue;
                miPedidoCliente.ColorPrimario = colorPrimario.SelectedValue;
                miPedidoCliente.ColorOpcional = colorOpcional.SelectedValue;
                miPedidoCliente.FechaCreacion = fecha.SelectedDate.ToString("yyyy-MM-dd");
                miPedidoCliente.EstadoPedido  = "10";
                miPedidoCliente.ClaseVehiculo = tipoVehiculo.SelectedValue;
                miPedidoCliente.AnoModelo     = anoModelo.SelectedItem.ToString();
                miPedidoCliente.FechaPedido   = fecha.SelectedDate.ToString("yyyy-MM-dd");
                miPedidoCliente.FechaProgEntrega = fechaEntrega.SelectedDate.ToString("yyyy-MM-dd");
                miPedidoCliente.Vendedor      = vendedor.SelectedValue;
                miPedidoCliente.Nit           = datosCliente.Text; ;
                miPedidoCliente.NitSolicita   = datosClienteSolicita.Text;
                miPedidoCliente.NitOpcional   = datosClienteAlterno.Text;
                miPedidoCliente.ValorUnitario = Convert.ToDouble(valorVehiculo.Text.Substring(1)).ToString();
                //miPedidoCliente.OpcionVehiculo = ddlOpciVehiDetalle.SelectedValue;
                
                //opción del vehículo
                if(ddlOpciVehiDetalle.Items.Count > 0)
                {
                    if (ddlOpciVehiDetalle.SelectedValue == "0")
                    {
                        miPedidoCliente.OpcionVehiculo = "";
                    }
                    else
                    {
                        miPedidoCliente.OpcionVehiculo = ddlOpciVehiDetalle.SelectedValue;
                    }
                }
                

                //if(tipoVehiculo.SelectedValue == "U")
                //    miPedidoCliente.Catalogo = DBFunctions.SingleData(
                //        String.Format("select pcat_codigo from mcatalogovehiculo where mcat_vin='{0}'"
                //        , catalogoVehiculo.SelectedValue)); // en los usados este ddl trae el vin

                if (recordDescuento.Text.Trim() == String.Empty)
                    miPedidoCliente.ValorDescuento = "0";
                else
                    miPedidoCliente.ValorDescuento = Convert.ToDouble(recordDescuento.Text.Trim()).ToString();

                miPedidoCliente.Obsequios     = descripcionObsequios.Text;
                miPedidoCliente.TipoVenta     = tipoVenta.SelectedValue;
                miPedidoCliente.ClaseVenta    = claseVenta.SelectedValue;
                miPedidoCliente.Credito       = respsino;
                miPedidoCliente.Observaciones = TextObserv.Text;

                if (costoObsequios.Text != "")
                    miPedidoCliente.ValorObsequios = Convert.ToDouble(costoObsequios.Text.Trim()).ToString();
                else
                    miPedidoCliente.ValorObsequios = "null";
                if (pagoEfectivo.Text != "")
                    miPedidoCliente.ValorEfectivo = Convert.ToDouble(pagoEfectivo.Text.Trim()).ToString();
                else
                    miPedidoCliente.ValorEfectivo = "null";
                if (pagoCheques.Text != "")
                    miPedidoCliente.ValorCheques = Convert.ToDouble(pagoCheques.Text.Trim()).ToString();
                else
                    miPedidoCliente.ValorCheques = "null";
                miPedidoCliente.NombreFinanciera = ddlFinanciera.SelectedValue;
                if (pagoFinanciera.Text != "")
                    miPedidoCliente.ValorFinaciera = Convert.ToDouble(pagoFinanciera.Text.Trim()).ToString();
                else
                    miPedidoCliente.ValorFinaciera = "null";
                if (valorOtroPago.Text != "")
                    miPedidoCliente.ValorOtrosPagos = Convert.ToDouble(valorOtroPago.Text.Trim()).ToString();
                else
                    miPedidoCliente.ValorOtrosPagos = "null";
                miPedidoCliente.DetalleOtrosPagos = otroPago.Text;
                miPedidoCliente.TipoServicio = ddlServicio.SelectedValue.ToString();

                if (Request.QueryString["prfCot"] != null && Request.QueryString["numCot"] != null)
                {
                    miPedidoCliente.CodigoCotizacion = Request.QueryString["prfCot"].ToString();
                    miPedidoCliente.NumeroCotizacion = Request.QueryString["numCot"].ToString();
                }
                else
                {
                    miPedidoCliente.CodigoCotizacion = "";
                    miPedidoCliente.NumeroCotizacion = "";
                }

                if (btnAccion.Text == "Crear el Pedido")
                {
                    if (miPedidoCliente.Grabar_Pedido())
                    {
                        string nitCli = miPedidoCliente.Nit;
                        string nombreCli = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + nitCli + "';");

                        Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientes&prefP=" + prefijoDocumento.SelectedValue.ToString() + "&numped=" + miPedidoCliente.NumeroPedido.ToString() + "&nitCli=" + nitCli + "&nombreCli=" + nombreCli);                       
                    }
                    else
                    {
                        lb.Text += miPedidoCliente.ProcessMsg;
                    }
                }
                else if (btnAccion.Text == "Guardar Modificación")
                {
                    if (retomaVehiculo.SelectedValue == "RV")
                    {
                        if (miPedidoCliente.Modificar_Pedido(true))
                        {
                            Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientes&prefP=" + prefijoDocumento.SelectedValue.ToString() + "&numped=" + numeroPedido.Text + "");
                                                    }
                        else
                        {
                            lb.Text += miPedidoCliente.ProcessMsg;
                        }
                    }
                    else if (retomaVehiculo.SelectedValue == "SRV")
                    {
                        if (miPedidoCliente.Modificar_Pedido(false))
                        {
                            Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientes&modificado=" + prefijoDocumento.SelectedValue.ToString() + "&numped=" + numeroPedido.Text + "");
                        }
                        else
                        {                           
                            lb.Text += miPedidoCliente.ProcessMsg;
                        }
                    }
                }
            }
            else
                Utils.MostrarAlerta(Response, "Existe diferencia entre el valor de la venta y la distribución de pagos!");
        }

        #endregion

        #region Manejo de Datos Iniciales

        [Ajax.AjaxMethod]
        public String Cambio_Tipo_Documento(string pdCodigo)
        {
            return DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + pdCodigo + "'");
        }


        protected void Cambio_Tipo_Vehiculo(Object Sender, EventArgs e)
        {
            CambiarTipoVehiculo();
        }

        protected void CambiarTipoVehiculo()
        {
            DatasToControls bind = new DatasToControls();
            string disponibles = @"select MC.PCAT_CODIGO,A.PUBI_NOMBRE,A.PUBI_VIGENCIA,A.PUBI_DIASLLEGADA, C.mcat_vin, 
                                  PC.PCOL_descripcion AS Color  
                                  from dbxschema.pubicacion A,dbxschema.MUBICACIONVEHICULO B,  
                                  dbxschema.mvehiculo C,mcatalogovehiculo mc, PCOLOR PC  
                                  where A.pubi_codigo=B.pubi_codigo and C.mcat_vin=B.mcat_vin AND mC.PCOL_CODIGO = PC.PCOL_CODIGO 
                                  and C.mcat_vin = mc.mcat_vin and c.tcla_codigo = \\'" + tipoVehiculo.SelectedValue.ToString() + @"\\'  
                                  ORDER BY MC.PCAT_CODIGO, PC.PCOL_descripcion, C.mcat_vin; ";
            if (tipoVehiculo.SelectedValue == "U")
                disponibles = @"select mC.PCAT_codigo, A.PUBI_NOMBRE, A.PUBI_VIGENCIA, A.PUBI_DIASLLEGADA, C.mcat_vin, PC.PCOL_descripcion AS Color  
                                from  dbxschema.pubicacion A,dbxschema.MUBICACIONVEHICULO B, dbxschema.mvehiculo C, mcatalogovehiculo mc, PCOLOR PC  
                                where A.pubi_codigo=B.pubi_codigo and C.mcat_vin=B.mcat_vin AND mC.PCOL_CODIGO = PC.PCOL_CODIGO and C.mcat_vin = mc.mcat_vin 
                            	  and c.test_tipoesta < 30 and c.tcla_codigo = \\'" + tipoVehiculo.SelectedValue.ToString() + @"\\'
                                ORDER BY 1, PC.PCOL_descripcion, C.mcat_vin; ";
            disp.Attributes.Remove("OnClick");
            disp.Attributes.Add("OnClick", "ModalDialog(this,'" + disponibles + "',new Array() );");
            
            txtPrecio.Text = "";
            //En caso que sea usado
            
            if (tipoVehiculo.SelectedValue == "U")
            {
                imgLupaCatalogo.Visible = false;
                if (proceso == "modi")
                    {
                    //string vin = DBFunctions.SingleData("select mcat_vin from mvehiculo where mveh_inventario = (SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["nume"] + ");");
                    
                     string vin = DBFunctions.SingleData("SELECT MCAT_VIN FROM mvehiculo WHERE MVEH_INVENTARIO=(select MVEH_INVENTARIO from mpedidovehiculoUSADO  where PDOC_CODIGO='" + Request.QueryString["pref"] + "' AND MPED_NUMEPEDI=" + Request.QueryString["nume"] + ");");    
                     Utils.FillDll(catalogoVehiculo, @"SELECT DISTINCT mcv.mcat_vin, 
                     mcv.mcat_vin || ' - [' || pm.pmar_nombre || '] - ' || pca.pcat_descripcion || ' - Placa ' || mcv.mcat_placa AS descripcion  
                     FROM dbxschema.pcatalogovehiculo pca,  
                     dbxschema.mcatalogovehiculo mcv,  
                     dbxschema.mvehiculo mv,
				     dbxschema.pmarca pm  
                     WHERE mcv.pcat_codigo = pca.pcat_codigo  
                     AND   mcv.mcat_vin = mv.mcat_vin  
                     AND   (mv.test_tipoesta = 10 OR mv.test_tipoesta = 20 OR mv.mcat_vin='" + vin + @"')  
                     AND   mv.tcla_codigo = 'U'
			         and   pca.pmar_codigo = pm.pmar_codigo  
                     ORDER BY descripcion;", true);
                    }
                else
                    Utils.FillDll(catalogoVehiculo, CatalogoVehiculos.CATALOGOVEHICULOSLISTAUSADOS, true);
                
                if (catalogoVehiculo.Items.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "No existen vehiculos usados en el catalogo.");
                    return;
                }
                else if (catalogoVehiculo.Items.Count == 1)
                {
                    catalogoVehiculo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                }
                txtPrecio.Visible = true;
                txtPrecio.Enabled = true;
                anoModelo.Enabled = colorPrimario.Enabled = colorOpcional.Enabled = ddlServicio.Enabled = false;
                PlfechaEntrega.Visible = false;
                // vehiculos usados no aplica descuento para que un vendedor realice un descuento se debe cambiar el precio al vehiculo
                valorDescuento.Enabled = false;
                
            }
            else if (tipoVehiculo.SelectedValue == "N")
            {
                imgLupaCatalogo.Visible = true;
                //Utils.FillDll(catalogoVehiculo, CatalogoVehiculos.CATALOGOVEHICULOSLISTA, true);
                if (Request.QueryString["ListaCatalogos"] != null)
                {
                    bind.PutDatasIntoDropDownList(catalogoVehiculo, CatalogoVehiculos.CATALOGOVEHICULOSLISTA2);
                }
                else
                {
                    bind.PutDatasIntoDropDownList(catalogoVehiculo, CatalogoVehiculos.CATALOGOVEHICULOSLISTA);
                }
                if (catalogoVehiculo.Items.Count > 0)
                    catalogoVehiculo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                txtPrecio.Visible = true;
                txtPrecio.Enabled = false;
                colorPrimario.Enabled = colorOpcional.Enabled = true;
                PlfechaEntrega.Visible = true;
            }
        }

        [Ajax.AjaxMethod]
        protected void Cambio_Tipo_Catalogo(Object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            string tipoVeh = tipoVehiculo.SelectedValue;

            // USADOS
            if (tipoVeh == "U")
            {
                string vin = catalogoVehiculo.SelectedValue; // trae el vin en el caso de los usados
                txtPrecio.Text = DBFunctions.SingleData("SELECT coalesce(MVEH_VALOINFL,0) FROM MVEHICULO WHERE mcat_vin='" + vin + "' order by mveh_inventario DESC");

                if (catalogoVehiculo.Items.Count > 0)
                {
                    DatasToControls.EstablecerValueDropDownList(ddlServicio, DBFunctions.SingleData("SELECT c.tser_tiposerv FROM mcatalogovehiculo C, tserviciovehiculo t  WHERE mcat_vin='" + vin + "' AND C.tser_tiposerv = t.tser_tiposerv"));
                    bind.PutDatasIntoDropDownList(anoModelo, "SELECT mcat_anomode FROM dbxschema.mcatalogovehiculo WHERE mcat_vin='" + vin + "'");
                    //DatasToControls.EstablecerDefectoDropDownList(anoModelo, DBFunctions.SingleData("SELECT mcat_anomode FROM dbxschema.mcatalogovehiculo WHERE mcat_vin='" + vin + "'"));
                    DatasToControls.EstablecerValueDropDownList(colorPrimario, DBFunctions.SingleData("SELECT C.PCOL_CODIGO FROM mcatalogovehiculo C, PCOLOR P  WHERE mcat_vin='" + vin + "' AND C.PCOL_CODIGO = P.PCOL_CODIGO"));
                    DatasToControls.EstablecerValueDropDownList(colorOpcional, DBFunctions.SingleData("SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_vin='" + vin + "'"));
                }
            }
            // NUEVOS
            else if (tipoVeh == "N")
            {

                anoModelo.Enabled = true;
                bind.PutDatasIntoDropDownList(anoModelo, "SELECT DISTINCT PA.PANO_ANO FROM PPRECIOVEHICULO PP, PANO PA WHERE PP.PCAT_CODIGO = '" + catalogoVehiculo.SelectedValue + "' AND PA.PANO_ANO = PP.PANO_ANO;");
                
                if(anoModelo.Items.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "No se han configurado los parámetros de años para los precios de vehículos, Contacte a Ecas");
                }

                //Si el catálogo se encuentra en más de un año no es necesario cargar precio porque se necesita saber cuál año es.
                else if (anoModelo.Items.Count > 1)
                {
                    anoModelo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    txtPrecio.Text = "";
                }
                //En el caso de ser solo un año, igual necesitamos saber si solo tiene una opción de vehículo.
                else
                {
                    bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + catalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI AND PP.PANO_ANO= '" + anoModelo.SelectedValue + "';");
                    
                    //Si hay varias opciones, entonces necesitamos cargar esas opciones y saber cual es, porque dependiedo de eso
                    //Cambia el precio o.o
                    if (ddlOpcionVeh.Items.Count > 1)
                    {
                        lblOpcionV.Visible = true;
                        ddlOpcionVeh.Visible = true;
                        ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                        txtPrecio.Text = "";
                    }
                    else if(ddlOpcionVeh.Items.Count == 1)
                    {
                        lblOpcionV.Visible = false;
                        ddlOpcionVeh.Visible = false;
                        CargarPrecioCatalogo(ddlOpcionVeh.SelectedValue);
                    }
                    else
                    {

                        lblOpcionV.Visible = false;
                        ddlOpcionVeh.Visible = false;
                        CargarPrecioCatalogo("");
                    }
                }
                //anoModelo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                

                //bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + catalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI ;");

                //if (ddlOpcionVeh.Items.Count > 1)
                //{
                //    lblOpcionV.Visible = true;
                //    ddlOpcionVeh.Visible = true;
                //    ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                //}
                //else
                //{
                //    lblOpcionV.Visible = false;
                //    ddlOpcionVeh.Visible = false;
                //    if (anoModelo.Items.Count < 2)
                //    {
                //        anoModelo.ClearSelection();
                //        bind.PutDatasIntoDropDownList(anoModelo, "SELECT PA.PANO_ANO FROM PPRECIOVEHICULO PP, PANO PA WHERE PP.PCAT_CODIGO = '" + catalogoVehiculo.SelectedValue + "' AND PA.PANO_ANO = PP.PANO_ANO;");
                //        CargarPrecioCatalogo("");
                //        anoModelo.Enabled = false;
                //    }
                //    //CargarPrecioCatalogo("");
                //}

                //ValoresImpuestosVehiculos impuestosVeh = new ValoresImpuestosVehiculos(catalogoVehiculo.SelectedValue, true);
                //string catalogo = catalogoVehiculo.SelectedValue;// trae el catalogo en el caso de los nuevos

                //if (ddlServicio.SelectedValue == "2" || ddlServicio.SelectedValue == "P" || esCasaMatriz != "")  //Publico:2
                //    txtPrecio.Text = (impuestosVeh.ValorVenta - impuestosVeh.ValorIpoConsumo).ToString();
                //else

                //    txtPrecio.Text = impuestosVeh.ValorVenta.ToString("C");

                //string disponibles = "select MC.PCAT_CODIGO,A.PUBI_NOMBRE,A.PUBI_VIGENCIA,A.PUBI_DIASLLEGADA, C.mcat_vin, " +
                //                      "PC.PCOL_descripcion AS Color   " +
                //                       "from dbxschema.pubicacion A,dbxschema.MUBICACIONVEHICULO B,   " +
                //                       "dbxschema.mvehiculo C,mcatalogovehiculo mc, PCOLOR PC   " +
                //                       "where A.pubi_codigo=B.pubi_codigo and C.mcat_vin=B.mcat_vin AND mC.PCOL_CODIGO = PC.PCOL_CODIGO  " +
                //                       "and C.mcat_vin = mc.mcat_vin and c.tcla_codigo = \\'" + tipoVeh + "\\'   " +
                //                       "and mc.pcat_codigo= \\'" + catalogo + "\\'   " +
                //                       "ORDER BY MC.PCAT_CODIGO, PC.PCOL_descripcion, C.mcat_vin; ";
                //disp.Attributes.Remove("OnClick");
                //disp.Attributes.Add("OnClick", "ModalDialog(this,'" + disponibles + "',new Array() );");
            }
        }

        public void cambioAno(Object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (tipoVehiculo.SelectedValue == "N")
            {
                if (anoModelo.SelectedValue == "0")
                {
                    Utils.MostrarAlerta(Response, "Seleccione un año del modelo");
                    txtPrecio.Text = "";
                    return;
                }
                //Cargamos las opciones del año seleccionado.
                bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + catalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI AND PP.PANO_ANO= '" + anoModelo.SelectedValue + "';");

                //Si es más de una opción la del vehiculo, podremos elegir qué opción sea la que queremos
                if (ddlOpcionVeh.Items.Count > 1)
                {
                    lblOpcionV.Visible = true;
                    ddlOpcionVeh.Visible = true;
                    ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    txtPrecio.Text = "";
                }

                //Si es solo una opción, cargamos el precio de una vez con esa opción.
                else if (ddlOpcionVeh.Items.Count == 1)
                {
                    lblOpcionV.Visible = false;
                    ddlOpcionVeh.Visible = false;
                    CargarPrecioCatalogo(ddlOpcionVeh.SelectedValue);
                }

                //Si el vehículo no tiene ninguna opción(porque podría no tener) entonces solo carga el precio
                else
                {

                    lblOpcionV.Visible = false;
                    ddlOpcionVeh.Visible = false;
                    CargarPrecioCatalogo("");
                }
                //CargarPrecioCatalogo("");
            }
        }

        protected void Cambio_OpcionVehiculo(Object Sender, EventArgs e)
        {
            CargarPrecioCatalogo(ddlOpcionVeh.SelectedValue.ToString());
        }

        protected void CargarPrecioCatalogo(string opcionVehiculo)
        {
            string tipoVeh = tipoVehiculo.SelectedValue;
            ValoresImpuestosVehiculos impuestosVeh = new ValoresImpuestosVehiculos(catalogoVehiculo.SelectedValue, true, opcionVehiculo, anoModelo.SelectedValue);
            string catalogo = catalogoVehiculo.SelectedValue;// trae el catalogo en el caso de los nuevos

            if (ddlServicio.SelectedValue == "2" || ddlServicio.SelectedValue == "P" || esCasaMatriz != "")  //Publico:2
                txtPrecio.Text = (impuestosVeh.ValorVenta - impuestosVeh.ValorIpoConsumo).ToString();
            else

                txtPrecio.Text = impuestosVeh.ValorVenta.ToString("C");

            string disponibles = "select MC.PCAT_CODIGO,A.PUBI_NOMBRE,A.PUBI_VIGENCIA,A.PUBI_DIASLLEGADA, C.mcat_vin, " +
                                  "PC.PCOL_descripcion AS Color   " +
                                   "from dbxschema.pubicacion A,dbxschema.MUBICACIONVEHICULO B,   " +
                                   "dbxschema.mvehiculo C,mcatalogovehiculo mc, PCOLOR PC   " +
                                   "where A.pubi_codigo=B.pubi_codigo and C.mcat_vin=B.mcat_vin AND mC.PCOL_CODIGO = PC.PCOL_CODIGO  " +
                                   "and C.mcat_vin = mc.mcat_vin and c.tcla_codigo = \\'" + tipoVeh + "\\'   " +
                                   "and mc.pcat_codigo= \\'" + catalogo + "\\'   " +
                                   "ORDER BY MC.PCAT_CODIGO, PC.PCOL_descripcion, C.mcat_vin; ";
            disp.Attributes.Remove("OnClick");
            disp.Attributes.Add("OnClick", "ModalDialog(this,'" + disponibles + "',new Array() );");
        }

        protected void Cambio_Almacen(Object Sender, EventArgs e)
        {
            llenarVendedores();
        }

        protected void ConfirmarDatosIniciales(Object Sender, EventArgs e)
        {
            if(anoModelo.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Existe un problema con respecto al año del modelo, verifique que el catálogo del vehículo tenga un año asociado en la lista de precios oficiales.");
                return;
            }

            //this.btnAnterior2.Visible = false;
            catalogoComp = (string)Session["catalogoComp"];
            string opcionVehiculo = "";
            if(ddlOpcionVeh.Visible == true)
                opcionVehiculo = ddlOpcionVeh.SelectedValue.ToString();

            //Primero verificamos las fechas de los calendarios
            if (tipoVehiculo.SelectedValue == "N")
            {
                if (fechaEntrega.SelectedDate <= fecha.SelectedDate)
                {
                    Utils.MostrarAlerta(Response, "La fecha de entrega es menor que la fecha de creación del pedido!");
                    return;
                }
            }
            
            if (datosCliente.Text == "" || datosClienteAlterno.Text == "" || datosClienteSolicita.Text == "")
            {
                Utils.MostrarAlerta(Response, "No se han ingresado los datos de alguno de los dos clientes!");
                return;
            }
        //Revisamos si estos nits son validos o no
            else if (!DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='" + datosCliente.Text.Trim() + "'"))
            {
                Utils.MostrarAlerta(Response, "El cliente principal ingresado no se encuentra registrado!");
                return;
            }
            else if (!DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='" + datosClienteAlterno.Text.Trim() + "'"))
            {
                Utils.MostrarAlerta(Response, "El cliente alterno ingresado no se encuentra registrado!");
                return;
            }
            //Revisamos que se hayan ingresado todos los mails de los clientes
            if (txtEmail.Text == "" || txtEmaila.Text == "" || txtEmails.Text == "")
            {
                Utils.MostrarAlerta(Response, "No se han ingresado el correo de  alguno de los clientes!.. Revise por favor!");
                return;
            }
                //en caso de que sí estén, se actualiza mnit con los correos.
            else
            {
                if (txtEmail.Text == txtEmaila.Text && txtEmail.Text == txtEmails.Text)
                {
                    DBFunctions.NonQuery("UPDATE MNIT SET MNIT_EMAIL = '" + txtEmail.Text + "' WHERE MNIT_NIT = '" + datosCliente.Text + "';");

                }
                else
                {
                    DBFunctions.NonQuery("UPDATE MNIT SET MNIT_EMAIL = '" + txtEmail.Text + "' WHERE MNIT_NIT = '" + datosCliente.Text + "';");
                    DBFunctions.NonQuery("UPDATE MNIT SET MNIT_EMAIL = '" + txtEmaila.Text + "' WHERE MNIT_NIT = '" + datosClienteAlterno.Text + "';");
                    DBFunctions.NonQuery("UPDATE MNIT SET MNIT_EMAIL = '" + txtEmails.Text + "' WHERE MNIT_NIT = '" + datosClienteSolicita.Text + "';");
                }
                txtEmail.ReadOnly = txtEmaila.ReadOnly = txtEmails.ReadOnly = true;
            }
            bind.PutDatasIntoDropDownList(ddlOpciVehiDetalle, "SELECT POPC_OPCIVEHI, POPC_NOMBOPCI FROM POPCIONVEHICULO");
            if (ddlOpciVehiDetalle.Items.Count <= 0)
            {
                ddlOpciVehiDetalle.Enabled = false;
                ddlOpciVehiDetalle.Items.Insert(0, new ListItem("Sin detalle..", "0"));
            }
            

            string error = "";
            if (prefijoDocumento.Items.Count == 0 || prefijoDocumento.SelectedValue == "")
                error = "Prefijo de Pedido Invalido";
            else if (tipoVehiculo.Items.Count == 0 || tipoVehiculo.SelectedValue == "")
                    error = "Tipo de Vehículo Invalido";
            else if (catalogoVehiculo.Items.Count == 0 || catalogoVehiculo.SelectedValue == "")
                    error = "Catálogo de Vehículo Invalido";
            else if (anoModelo.Items.Count == 0 || anoModelo.SelectedValue == "")
                    error = "Año de Modelo Invalido";
            else if (colorPrimario.Items.Count == 0 || colorPrimario.SelectedValue == "")
                    error = "Color Primario Invalido";
            else if (colorOpcional.Items.Count == 0 || colorOpcional.SelectedValue == "")
                    error = "Color Opcional Invalido";
            else if (vendedor.Items.Count == 0 || vendedor.SelectedValue == "")
                    error = "Vendedor Invalido";
            else if (tipoVenta.Items.Count == 0 || tipoVenta.SelectedValue == "")
                    error = "Tipo Venta Invalida";
            else if (claseVenta.Items.Count == 0 || claseVenta.SelectedValue == "")
                    error = "Clase Venta Invalida";
            if (error != "")
            {
                Utils.MostrarAlerta(Response, "" + error + "!\\nRevise Los Parametros!");
                return;
            }
            //if(listaUsados.Visible && listaUsados.Items.Count == 0)
            //{
            //    Response.Write("<script language:javascript>alert('No se ha seleccionado ningun vehiculo usado!');</script>");
            //    return;
            //}
            //Traemos el precio del vehiculo dependiendo si es de un vehiculo nuevo o usado
            double valorVehiculoSinIva = 0;

            esCasaMatriz = DBFunctions.SingleData("select mnit_nit from pcasamatriz where mnit_nit = '" + datosCliente.Text + "'");

            if (esCasaMatriz != "")
                Utils.MostrarAlerta(Response, "El NIT ingresado esta asociado a una casa matriz, por lo que se ha recalculado el valor del vehiculo sin Impoconsumo.");

            try
            {
                if (tipoVehiculo.SelectedValue == "N")
                {

                    ValoresImpuestosVehiculos impuestosVeh = new ValoresImpuestosVehiculos(catalogoVehiculo.SelectedValue, true, opcionVehiculo, anoModelo.SelectedValue);

                    if (ddlServicio.SelectedValue == "2" || ddlServicio.SelectedValue == "P" || esCasaMatriz != "")  //Publico:2
                    {
                        precioOficial = impuestosVeh.ValorVenta - impuestosVeh.ValorIpoConsumo;
                    }
                    else
                    {                 
                        valorVehiculoSinIva = double.Parse(txtPrecio.Text, NumberStyles.Currency);
                        //precioOficial = valorVehiculoSinIva;
                                            
                         precioOficial = impuestosVeh.ValorVenta;
                    }
                    valorVehiculoSinIva = impuestosVeh.ValorBase;
                    valorBase.Text = valorVehiculoSinIva.ToString("C");
                }
                else  if (tipoVehiculo.SelectedValue == "U")
                {
                    string vin = catalogoVehiculo.SelectedValue; // en los usados trae el vin

                    DBFunctions.NonQuery("UPDATE MVEHICULO SET MVEH_VALOINFL =" + double.Parse(txtPrecio.Text, NumberStyles.Currency) + " WHERE mcat_vin='" + vin + "' AND TEST_TIPOESTA < 40;");
                    //valorVehiculoSinIva = Convert.ToDouble(txtPrecio.Text);
                    valorVehiculoSinIva = double.Parse(txtPrecio.Text, NumberStyles.Currency);

                     //valorVehiculoSinIva = Convert.ToDouble(
                    //    DBFunctions.SingleData("SELECT coalesce(MVEH_VALOINFL,0) FROM MVEHICULO WHERE mcat_vin='" + vin + "' AND TEST_TIPOESTA < 40"));
                    //valorVehiculo.Text = precioOficial.ToString("C");
                     precioOficial = valorVehiculoSinIva;

                }
                lbPrecioOficial.Text = precioOficial.ToString("C");
                valorBase.Text = valorVehiculoSinIva.ToString("C");
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Este vehiculo no tiene un precio configurado!\\nRevise Por Favor!");
                return;
            }

            if (proceso == "modi") //si es edición
                if (catalogoComp != catalogoVehiculo.SelectedValue.ToString() && tipoVehiculo.SelectedValue == "N") //Si el catálogo cambió
                    Utils.MostrarAlerta(Response, "Se cambio el catálogo del   pedido ");
                else //Si el catálogo no cambió
                    valorDescuento.Text = DBFunctions.SingleData(
                        String.Format("select mped_valodesc from mpedidovehiculo where pdoc_codigo='{0}' and mped_numepedi={1}",
                        prefijoDocumento.SelectedValue, numeroPedido.Text));

            string tipoNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI from mnit where mnit_nit = '" + datosCliente.Text + "'");
            if (tipoNacionalidad == "E")
            {
                precioVehMinimo = Math.Abs(Convert.ToDouble(valorBase.Text.Substring(1)));
            }
            else
            {
                precioVehMinimo = Math.Abs(Convert.ToDouble(lbPrecioOficial.Text.Substring(1)));
            }
            precioVehMinimo = Math.Round(precioVehMinimo - (precioVehMinimo * porcentajeDtoMax / 100), 0);

            if (proceso == "modi") //si es edición
                if (tipoVehiculo.SelectedValue == "N")
                {
                    valorBase.Text = valorVehiculoSinIva.ToString("C");
                }
                else {
                    //valorBase.Text = Convert.ToDouble(txtPrecio.Text);
                    //valorBase.Text = valorVehiculo.Text;
                    valorBase.Text = precioOficial.ToString("C");
                    //precioOficial = valorVehiculoSinIva;
                    //valorBase.Text = valorVehiculo.Text;
                }

            //    valorBase.Text = valorVehiculo.Text;
            //else
            //    valorBase.Text = valorVehiculoSinIva.ToString("C");
            double valorIvaVeh;
            double valorvehi;
            
            if (tipoVehiculo.SelectedValue == "N")
            {
                ValoresImpuestosVehiculos impuestosVeh = new ValoresImpuestosVehiculos(catalogoVehiculo.SelectedValue, true, opcionVehiculo, anoModelo.SelectedValue);
                if (ddlServicio.SelectedValue == "2" || ddlServicio.SelectedValue == "P" || esCasaMatriz != "")  //Publico:2
                {
                    if (tipoNacionalidad == "E")
                    {
                        valorIvaVeh = 0;
                        porcentajeIva = 0;
                        Utils.MostrarAlerta(Response, "El NIT ingresado es de nacionalidad Extrangera por lo cual está excento de pagar impuestos.");
                    }
                    else
                    {
                        valorIvaVeh = impuestosVeh.ValorIva;
                        porcentajeIva = impuestosVeh.ValorIvaPorc;
                    }
                }
                else
                {
                    if (tipoNacionalidad == "E")
                    {
                        valorIvaVeh = 0;
                        porcentajeIva = 0;
                        Utils.MostrarAlerta(Response, "El NIT ingresado es de nacionalidad Extrangera por lo cual está excento de pagar impuestos.");
                    }
                    else
                    {
                        valorIvaVeh = impuestosVeh.ValorImpuestos; //iva+ipoconsumo.
                        porcentajeIva = impuestosVeh.ValorIvaPorc + impuestosVeh.ValorIpoConsumoPorc;
                    }
                }
            }
            else
            {
                valorIvaVeh = 0;
            }
            
            if (proceso == "modi")
            {//si es edición

                valorvehi = 0;
                if (tipoVehiculo.SelectedValue == "N")
                {
                    valorvehi = Math.Round(valorVehiculoSinIva + valorIvaVeh, 0);
                    if (tipoNacionalidad == "E") 
                    { 
                        valorIva.Text = "0"; 
                       // Utils.MostrarAlerta(Response, "El NIT ingresado es de nacionalidad extrangera por lo cual está excento de pagar impuesto"); 
                    }
                    else
                    {
                        valorIva.Text = valorIvaVeh.ToString("C");
                    }
                }
                else
                {
                    //valorvehi = Convert.ToDouble(valorVehiculo.Text.Substring(1));
                    valorvehi = precioOficial;
                    //valorvehi=valorVehiculoSinIva;
                }
            }
            else
            {
                valorvehi = Math.Round(valorVehiculoSinIva + valorIvaVeh, 0);
                //valorIva.Text = (valorVehiculoSinIva * (porcentajeIva / 100)).ToString("C");
                if (tipoNacionalidad == "E")
                {
                    valorIva.Text = "0";
                    //Utils.MostrarAlerta(Response, "Las empresan con tipo de nacionalidad Extranjera quedan excentas de pagar impuesto");
                }
                else
                {
                    valorIva.Text = valorIvaVeh.ToString("C");
                }
            }
                 //valorVehiculo.Text = (valorVehiculoSinIva + (valorVehiculoSinIva * (porcentajeIva / 100))).ToString("C");
            valorVehiculo.Text = valorvehi.ToString("C");
            hdvalorVehiculo.Value = valorvehi.ToString("C");

            double descuento = Convert.ToDouble(valorDescuento.Text);
            double valorVehi = Convert.ToDouble(valorVehiculo.Text.Substring(1));
            double costoOtros = 0;
            try
            {                
                 costoOtros = double.Parse(costoOtrosElementos.Text, NumberStyles.Currency);
                //costoOtros = Convert.ToDouble((costoOtrosElementos.Text).Replace("($", "").Replace(")", ""));
           
            }
            catch (Exception err)
            { }
            totalValorVehiculo.Text = (valorVehi - descuento).ToString("C");
            totalVenta.Text = (valorVehi - descuento + costoOtros).ToString("C");

            //precioVehMinimo=double.Parse(valorVehiculo.Text.Substring(1));
            Session["precioVehMinimo"] = precioVehMinimo;
            plInfoPed.Visible = false;
            plInfoVent.Visible = true;
            //RecalculoValorVehiculo();
            valorVehiculo.Attributes.Remove("onkeyup");
            if (tipoVehiculo.SelectedValue == "N")
            {
                valorVehiculo.Attributes.Add("onkeyup", "RecalculoValorVehiculo(" + valorBase.ClientID + "," + valorVehiculo.ClientID + "," + valorIva.ClientID + "," + porcentajeIva.ToString() + "," + totalValorVehiculo.ClientID + "," + hdvalorVehiculo.ClientID + "," + valorDescuento.ClientID + "," + costoOtrosElementos.ClientID + "," + totalVenta.ClientID + ")");
            }
            else
            {
                valorVehiculo.Attributes.Add("onkeyup", "RecalculoValorVehiculo(" + valorBase.ClientID + "," + valorVehiculo.ClientID + ",0,0," + totalValorVehiculo.ClientID + "," + hdvalorVehiculo.ClientID + "," + valorDescuento.ClientID + "," + costoOtrosElementos.ClientID + "," + totalVenta.ClientID + ")");
            }
            valorDescuento.Attributes.Remove("onkeyup");
            //valorDescuento.Attributes.Add("onkeyup","CalcularValorBaseDescuento("+valorDescuento.ClientID+","+valorBase.ClientID+","+valorIva.ClientID+","+valorVehiculo.ClientID+","+totalValorVehiculo.ClientID+","+costoOtrosElementos.ClientID+","+totalVenta.ClientID+","+valorVehiculoSinIva.ToString()+","+porcentajeIva.ToString()+");");
            if (tipoVehiculo.SelectedValue == "N")
            {
                valorDescuento.Attributes.Add("onkeyup", "CalcularValorBaseDescuento(" + valorDescuento.ClientID + "," + valorBaseDescuento.ClientID + "," + valorIvaDescuento.ClientID + "," + valorVehiculoDescuento.ClientID + "," + totalValorVehiculo.ClientID + "," + costoOtrosElementos.ClientID + "," + totalVenta.ClientID + "," + valorVehiculoSinIva.ToString() + "," + porcentajeIva.ToString() + "," + hdvalorVehiculo.ClientID + "," + recordDescuento.ClientID + "," + hdDescuentos.ClientID + ");");
            }
            else
            {
                valorDescuento.Attributes.Add("onkeyup", "CalcularValorBaseDescuento(" + valorDescuento.ClientID + "," + valorBaseDescuento.ClientID + ",0," + valorVehiculoDescuento.ClientID + "," + totalValorVehiculo.ClientID + "," + costoOtrosElementos.ClientID + "," + totalVenta.ClientID + "," + valorVehiculoSinIva.ToString() + ",0," + hdvalorVehiculo.ClientID + "," + recordDescuento.ClientID + "," + hdDescuentos.ClientID + ");");
            }

            grabando = false;
        }

        #endregion

        #region Manejo Informacion Costos

        protected void VolverDatosIniciales(Object Sender, EventArgs e)
        {
            plInfoPed.Visible = true;
            plInfoVent.Visible = false;
        }

        protected void VolverDatosVenta(Object Sender, EventArgs e)
        {

            plInfoVent.Visible = true;
            plInfoPago.Visible = false;
        }

        protected void ConfirmarDatosVenta(Object Sender, EventArgs e)
        {
            if (DBFunctions.SingleData("SELECT CVEH_CREDFINC FROM CVEHICULOS") == "N")
            {
                pagoFinanciera.Enabled = ddlFinanciera.Enabled = false;
            }
            hdValorbase.Value = valorBase.Text.Substring(1);
            precioVehMinimo = (double)Session["precioVehMinimo"];
            double precioVeh = Convert.ToDouble(totalValorVehiculo.Text.Substring(1));
            if (precioVeh < 1)
            {
                Utils.MostrarAlerta(Response, "El Valor del Vehículo y-o descuento está Errado ..., revise por favor!");
                return;
            }
            string tipoNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI from mnit where mnit_nit = '" + datosCliente.Text + "'");
            
            if (Convert.ToDouble(totalValorVehiculo.Text.Substring(1)) < precioVehMinimo)
            {
                Utils.MostrarAlerta(Response, "El Valor del Vehículo es menor al establecido " + precioVehMinimo.ToString("C") + ",revise por favor!");
                return;
            }
            
            if (Convert.ToDouble(totalVenta.Text.Substring(1)) == 0)
            {
                Utils.MostrarAlerta(Response, "El total de venta es $0!");
                return;
            }
            //Session["valorBase"]=valorBase.Text.Substring(1);
            plInfoVent.Visible = false;
            plInfoPago.Visible = true;

            pagoEfectivo.Attributes.Remove("onkeyup");
            pagoEfectivo.Attributes.Add("onkeyup", "CalculoTotalPagos(" + pagoEfectivo.ClientID + "," + pagoFinanciera.ClientID + "," + pagoCheques.ClientID + "," + valorOtroPago.ClientID + "," + valorParcialPagos.ClientID + "," + valorTotalRetoma.ClientID + "," + valorTotalPagos.ClientID + "," + diferenciaPagos.ClientID + "," + Convert.ToDouble(totalVenta.Text.Substring(1)) + ",1);");
            pagoFinanciera.Attributes.Remove("onkeyup");
            pagoFinanciera.Attributes.Add("onkeyup", "CalculoTotalPagos(" + pagoEfectivo.ClientID + "," + pagoFinanciera.ClientID + "," + pagoCheques.ClientID + "," + valorOtroPago.ClientID + "," + valorParcialPagos.ClientID + "," + valorTotalRetoma.ClientID + "," + valorTotalPagos.ClientID + "," + diferenciaPagos.ClientID + "," + Convert.ToDouble(totalVenta.Text.Substring(1)) + ",2);");
            pagoCheques.Attributes.Remove("onkeyup");
            pagoCheques.Attributes.Add("onkeyup", "CalculoTotalPagos(" + pagoEfectivo.ClientID + "," + pagoFinanciera.ClientID + "," + pagoCheques.ClientID + "," + valorOtroPago.ClientID + "," + valorParcialPagos.ClientID + "," + valorTotalRetoma.ClientID + "," + valorTotalPagos.ClientID + "," + diferenciaPagos.ClientID + "," + Convert.ToDouble(totalVenta.Text.Substring(1)) + ",3);");
            valorOtroPago.Attributes.Remove("onkeyup");
            valorOtroPago.Attributes.Add("onkeyup", "CalculoTotalPagos(" + pagoEfectivo.ClientID + "," + pagoFinanciera.ClientID + "," + pagoCheques.ClientID + "," + valorOtroPago.ClientID + "," + valorParcialPagos.ClientID + "," + valorTotalRetoma.ClientID + "," + valorTotalPagos.ClientID + "," + diferenciaPagos.ClientID + "," + Convert.ToDouble(totalVenta.Text.Substring(1)) + ",4);");

            try { diferenciaPagos.Text = (Convert.ToDouble(totalVenta.Text.Substring(1)) - Convert.ToDouble(valorTotalPagos.Text.Substring(1))).ToString("C"); }
            catch { }
            if(tipoVehiculo.SelectedValue == "N")
            { 
                TextObserv.Text = "Fecha Nacimiento Cliente:   Placa terminada en:   " ;
                TextObserv.Text += "Matricular en: "  + DBFunctions.SingleData ("select TRIM(pc.pciu_nombre) from palmacen p, pciudad pc where palm_almacen = '"+ almacen.SelectedValue + "' and pc.PCIU_CODIGO = p.PCIU_CODIGO;");
            }
            /*
                bind.PutDatasIntoDropDownList(ddlOpciVehiDetalle, "SELECT POPC_OPCIVEHI, POPC_NOMBOPCI FROM POPCIONVEHICULO");
                if (ddlOpciVehiDetalle.Items.Count <= 0)
                {
                    ddlOpciVehiDetalle.Enabled = false;
                }
                ddlOpciVehiDetalle.Items.Insert(0, new ListItem("No aplica", "0"));
            */

        }

        protected void Preparar_Tabla_Elementos()
        {
            tablaElementos = new DataTable();
            tablaElementos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));//0
            tablaElementos.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//1
            tablaElementos.Columns.Add(new DataColumn("COSTO", System.Type.GetType("System.Double")));//2
            tablaElementos.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//3
            tablaElementos.Columns.Add(new DataColumn("TIPO_TRAMITE", System.Type.GetType("System.String")));//4
        }

        protected void Binding_Grilla()
        {
            if (tablaElementos.Rows.Count == 0)
            {
                DataSet elementosVentaPedido = new DataSet();
                DBFunctions.Request(elementosVentaPedido, IncludeSchema.NO, "SELECT PITE_CODIGO, PITE_NOMBRE, CASE WHEN PITE_CARGO = 'C' THEN PITE_COSTO ELSE 0 END AS COSTO, CEMP_PORCIVA, PTRA_CODIGO FROM PITEMVENTAVEHICULO, CEMPRESA WHERE PITE_INDIGENE = 'S' ; ");
                if (elementosVentaPedido.Tables.Count > 0)
                { 
                    for (int i = 0; i < elementosVentaPedido.Tables[0].Rows.Count; i++)
                    {
                        DataRow fila = tablaElementos.NewRow();
                        fila[0] = elementosVentaPedido.Tables[0].Rows[i][0].ToString();
                        fila[1] = elementosVentaPedido.Tables[0].Rows[i][1].ToString();
                        fila[2] = Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][2]);
                        fila[3] = Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][3]);
                        fila[4] = elementosVentaPedido.Tables[0].Rows[i][4].ToString();
                        tablaElementos.Rows.Add(fila);
                        Binding_Grilla();
                    }
                }
            }

            Session["tablaElementos"] = tablaElementos;
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();
            double totalOtrosVenta = 0;
            for (int i = 0; i < tablaElementos.Rows.Count; i++)
            {
                totalOtrosVenta += (Convert.ToDouble(tablaElementos.Rows[i][2]) + (Convert.ToDouble(tablaElementos.Rows[i][2]) * (Convert.ToDouble(tablaElementos.Rows[i][3]) / 100)));
                grillaElementos.Items[i].Cells[2].HorizontalAlign = HorizontalAlign.Right;
            }
            costoOtrosElementos.Text = (totalOtrosVenta.ToString("C")).Replace(")", "");
            try { totalVenta.Text    = (totalOtrosVenta + Convert.ToDouble(totalValorVehiculo.Text.Substring(1))).ToString("C"); }
            catch { }
        }

        protected void dgAccesorioBound(object sender, DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]), "SELECT piva_porciva FROM piva");
        }

        protected void dgEvento_Grilla(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AgregarObsequios")
            {

                //Primero verificamos que los campos no sean vacios
                if (((((TextBox)e.Item.Cells[0].Controls[1]).Text) == "") || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "No existe ningun elemento para adicionar O Existe algun problema con el valor");
                else
                {                    
                    //debemos agregar una fila a la tabla asociada y luego volver a pintar la tabla
                    DataRow fila = tablaElementos.NewRow();
                    fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                    fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    fila[2] = Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
                    fila[3] = Convert.ToDouble(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim());
                    fila[4] = DBFunctions.SingleData("SELECT PTRA_CODIGO FROM DBXSCHEMA.pitemventavehiculo where PITE_CODIGO = '" + ((TextBox)e.Item.Cells[0].Controls[1]).Text + "'");
                    if (tablaElementos.Select("CODIGO='" + fila[0].ToString() + "'").Length == 0)//Rows[e.Item.TabIndex][4].ToString().Length > 0)
                    {
                            tablaElementos.Rows.Add(fila);                 
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "El item contiene un código de accesorio-trámite ya definido, revice por favor");
                        return;
                    }
                     Binding_Grilla();                    
                }
            }
            else if (((Button)e.CommandSource).CommandName == "QuitarObsequios")
            {
                    tablaElementos.Rows[e.Item.ItemIndex].Delete();
                    Binding_Grilla();
                }
        }

        #endregion

        #region Manejo Informacion Pagos

        protected void Retoma_Vehiculo(Object Sender, EventArgs e)
        {
            if (retomaVehiculo.SelectedValue == "RV")
            {
                controlesRetomaVehiculos.Visible = true;
                Binding_Grilla_Retoma();
            }
            else if (retomaVehiculo.SelectedValue == "SRV")
            {
                controlesRetomaVehiculos.Visible = false;
                valorTotalPagos.Text = (Convert.ToDouble(valorTotalPagos.Text.Substring(1)) - Convert.ToDouble(valorTotalRetoma.Text.Substring(1))).ToString("C");
                diferenciaPagos.Text = (Convert.ToDouble(valorTotalRetoma.Text.Substring(1)) + Convert.ToDouble(diferenciaPagos.Text.Substring(1))).ToString("C");
                valorTotalRetoma.Text = "$0";
            }
        }

        protected void Preparar_Tabla_Retoma()
        {
            tablaRetoma = new DataTable();
            tablaRetoma.Columns.Add(new DataColumn("TIPOVEHICULO", System.Type.GetType("System.String")));//0
            tablaRetoma.Columns.Add(new DataColumn("NUMEROCONTRATO", System.Type.GetType("System.String")));//1
            tablaRetoma.Columns.Add(new DataColumn("ANOMODELO", System.Type.GetType("System.String")));//2
            tablaRetoma.Columns.Add(new DataColumn("NUMEROPLACA", System.Type.GetType("System.String")));//3
            tablaRetoma.Columns.Add(new DataColumn("CUENTAIMPUESTOS", System.Type.GetType("System.String")));//4
            tablaRetoma.Columns.Add(new DataColumn("VALORRECIBIDO", System.Type.GetType("System.Double")));//5
        }

        protected void Binding_Grilla_Retoma()
        {
            Session["tablaRetoma"] = tablaRetoma;
            grillaRetoma.DataSource = tablaRetoma;
            grillaRetoma.DataBind();
            if (retomaVehiculo.SelectedValue == "RV")
            {
                double totalRetoma = 0;
                for (int i = 0; i < tablaRetoma.Rows.Count; i++)
                {
                    totalRetoma += Convert.ToDouble(tablaRetoma.Rows[i][5]);
                    grillaRetoma.Items[i].Cells[5].HorizontalAlign = HorizontalAlign.Right;
                }
                valorTotalRetoma.Text = totalRetoma.ToString("C");
                try { valorTotalPagos.Text = (totalRetoma + Convert.ToDouble(valorParcialPagos.Text.Substring(1))).ToString("C"); }
                catch { }
                try { diferenciaPagos.Text = (Convert.ToDouble(totalVenta.Text.Substring(1)) - Convert.ToDouble(valorTotalPagos.Text.Substring(1))).ToString("C"); }
                catch { }
            }
            else
            {
                double anteriorValor = Convert.ToDouble(valorTotalRetoma.Text.Substring(1));
                valorTotalRetoma.Text = "$0";
                valorTotalPagos.Text = (Convert.ToDouble(valorTotalPagos.Text.Substring(1)) - Convert.ToDouble(valorTotalRetoma.Text.Substring(1))).ToString("C");
            }
        }

        protected void dgEvento_Grilla_Retoma(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AgregarRetoma")
            {
                //Primero debemos verificar que los campos no sean nulos
                if (((((TextBox)e.Item.Cells[0].Controls[1]).Text) == "") || ((((TextBox)e.Item.Cells[1].Controls[1]).Text) == "") || ((((TextBox)e.Item.Cells[2].Controls[1]).Text) == "") || ((((TextBox)e.Item.Cells[3].Controls[1]).Text) == "") || ((((TextBox)e.Item.Cells[4].Controls[1]).Text) == "") || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "Algún campo es vacio o Invalido, Revise Por Favor");
                else
                {
                    DataSet retoma = new DataSet();
                    DBFunctions.Request(retoma, IncludeSchema.NO,
                        "select test_tipoesta from dbxschema.mvehiculo where mcat_vin = (select mcat_vin from dbxschema.mcatalogovehiculo where mcat_placa='" + ((TextBox)e.Item.Cells[3].Controls[1]).Text + "') and test_tipoesta <=40;");

                    if (retoma.Tables[0].Rows.Count == 0)
                    {
                        //debemos agregar una fila a la tabla de retomas
                        DataRow fila = tablaRetoma.NewRow();
                        fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                        fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                        fila[2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
                        fila[3] = ((TextBox)e.Item.Cells[3].Controls[1]).Text;
                        fila[4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                        fila[5] = System.Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
                        tablaRetoma.Rows.Add(fila);
                        this.Binding_Grilla_Retoma();
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "El vehiculo de retoma, actualmente existe en inventario disponible. Se debe vender primero dicho vehiculo para realizar una retoma.");
                    }
                }
            }
            else if (((Button)e.CommandSource).CommandName == "QuitarRetoma")
            {
                tablaRetoma.Rows[e.Item.ItemIndex].Delete();
                this.Binding_Grilla_Retoma();
            }
        }

        protected void ConsultarAbonos(Object Sender, EventArgs e)
        {
            Response.Redirect("" + indexPage + "?process=Vehiculos.ConsultaPagosPedido&Orig=2&prefPed=" + prefijoDocumento.SelectedValue + "&numPed=" + numeroPedido.Text.Trim() + "");
        }


        #endregion

        #region Metodos

        private void llenarVendedores()
        {
            DatasToControls bind = new DatasToControls();
            String sql = @"SELECT pv.pven_codigo, pv.pven_nombre      
              FROM pvendedor pv      
              INNER JOIN pvendedoralmacen pva ON pva.pven_codigo = pv.pven_codigo      
              WHERE pva.palm_almacen = '" + almacen.SelectedValue + @"'     
              AND   (pv.tvend_codigo = 'VV' OR pv.tvend_codigo = 'TT') 
              AND   pv.pven_vigencia = 'V'  
              ORDER BY pv.PVEN_NOMBRE";
            vendedor.Enabled = true;
            bind.PutDatasIntoDropDownList(vendedor, sql);
            if (vendedor.Items.Count > 0)
                vendedor.Items.Insert(0, "Seleccione...");
        }

        //Funcion que nos carga un pedido de cliente ya existente
        protected void Cargar_Pedido_Existente(string prefijoPedidoModificacion, string numeroPedidoModificacion)
        {
            DatasToControls bind = new DatasToControls();
            //Primero debemos cargar los datos iniciales de tipo de documento, numero, etc
            DatasToControls.EstablecerDefectoDropDownList(prefijoDocumento, DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='" + prefijoPedidoModificacion + "'"));
            prefijoDocumento.Enabled = false;
            numeroPedido.Text = numeroPedidoModificacion;
            numeroPedido.Enabled = false;
            string vin = DBFunctions.SingleData("select mcat_vin from mvehiculo where mveh_inventario = (SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ");");
            // buscar si el credito fue otorgado por el concesionario 
            string tres = DBFunctions.SingleData("Select tres_sino  from DBXSCHEMA.MPEDIDOVEHICULO where pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + " ");
            if (tres == "S")
            {
                rblcredito.Items[1].Selected = true;

            }
            else
            {
                rblcredito.Items[0].Selected = false;
            }

            DatasToControls.EstablecerValueDropDownList(prefijoDocumento, DBFunctions.SingleData("SELECT pdoc_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion));
            DatasToControls.EstablecerValueDropDownList(tipoVehiculo, DBFunctions.SingleData("SELECT tcla_clase  FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion));
            
            if (catalogoVehiculo.Items.Count == 0)
                CambiarTipoVehiculo();

            
            // ESTE ESTABA
            if (tipoVehiculo.SelectedValue == "U")
             {
                 if (catalogoVehiculo!=null)
                     DatasToControls.EstablecerValueDropDownList(catalogoVehiculo, DBFunctions.SingleData("select mcat_vin from mvehiculo where mveh_inventario = (SELECT mveh_inventario FROM mpedidovehiculoUSADO WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")"));
            }
            else
            {
                DatasToControls.EstablecerValueDropDownList(catalogoVehiculo, DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
            }
            DatasToControls.EstablecerValueDropDownList(ddlServicio, DBFunctions.SingleData("SELECT tser_tiposerv FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
            
            //Por si cambian el catalogo.
            
            //DatasToControls.EstablecerValueDropDownList(catalogoVehiculo, DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
           
            catalogoComp = DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo  WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + " ");
            Session["catalogoComp"] = catalogoComp;
            bind.PutDatasIntoDropDownList(anoModelo, "SELECT pano_ano FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            //DatasToControls.EstablecerValueDropDownList(anoModelo, DBFunctions.SingleData("SELECT pano_ano FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
            //DatasToControls.EstablecerDefectoDropDownList(anoModelo, DBFunctions.SingleData("SELECT pano_ano FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
            DatasToControls.EstablecerValueDropDownList(colorPrimario, DBFunctions.SingleData("SELECT pcol_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion));
            DatasToControls.EstablecerValueDropDownList(colorOpcional, DBFunctions.SingleData("SELECT pcol_codigoalte FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion));
            DatasToControls.EstablecerValueDropDownList(almacen, DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "))"));
            bind.PutDatasIntoDropDownList(vendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE pven_vigencia='V' and (tvend_codigo='TT' or (tvend_codigo='VV' AND palm_almacen='" + almacen.SelectedValue + "')) ORDER BY PVEN_NOMBRE");
            DatasToControls.EstablecerValueDropDownList(vendedor, DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")"));
            DatasToControls.EstablecerDefectoDropDownList(tipoVenta, DBFunctions.SingleData("SELECT ptipo_descrventa FROM ptipoventavehiculo WHERE ptipo_coditipoventa=(SELECT ptipo_coditipoventa FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")"));
            DatasToControls.EstablecerDefectoDropDownList(claseVenta, DBFunctions.SingleData("SELECT pclas_ventadescrip FROM pclaseventavehiculo WHERE pclas_codigoventa=(SELECT pclas_codigoventa FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")"));

            try
            {
                if (tipoVehiculo.SelectedValue == "N")
                    porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT piva_porciva FROM pcatalogovehiculo WHERE pcat_codigo='" + catalogoVehiculo.SelectedValue + "'"));
                else if (tipoVehiculo.SelectedValue == "U") 
                    porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT piva_porciva FROM mcatalogovehiculo mcv inner join pcatalogovehiculo pcv on pcv.pcat_codigo = mcv.pcat_codigo WHERE mcat_vin='" + vin + "'"));
            }
            catch (Exception ee)
            {}

            //tipoVenta y claseVenta aun no se almacenan de forma persistente
            fecha.SelectedDate        = Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_fechpedi FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "").Trim());
            fechaEntrega.SelectedDate = Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_fechentrega FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "").Trim());
            datosCliente.Text         = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            datosClientea.Text        = DBFunctions.SingleData("SELECT nombre FROM Vmnit WHERE mnit_nit=(SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")");
            datosClienteAlterno.Text  = DBFunctions.SingleData("SELECT mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            datosClienteAlternoa.Text = DBFunctions.SingleData("SELECT nombre FROM Vmnit WHERE mnit_nit=(SELECT mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")");
            datosClienteSolicita.Text = DBFunctions.SingleData("SELECT mnit_NITsolicita FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            datosClienteSolicitaa.Text= DBFunctions.SingleData("SELECT nombre FROM Vmnit WHERE mnit_nit=(SELECT mnit_NITsolicita FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ")");
            txtEmail.Text             = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MNIT WHERE MNIT_NIT = '" + datosCliente.Text + "'");
            txtEmaila.Text            = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MNIT WHERE MNIT_NIT = '" + datosClienteAlterno.Text + "'");
            txtEmails.Text            = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MNIT WHERE MNIT_NIT = '" + datosClienteSolicita.Text + "'");
            //txtPrecio.Text = Convert.ToDouble(DBFunctions.SingleData("select PPRE_PRECIO from ppreciovehiculO WHERE PCAT_CODIGO = '" + catalogoComp + "' AND PANO_ANO =" + anoVeh); //A futuro.. Buscar la forma de traer la opciòn del vehìculo porque puede pasar que trae dos registros o màs..
            txtPrecio.Text          = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(mped_valounit,0) as valorsindesc FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("C");
            // para vehiculos usados
            if (tipoVehiculo.SelectedValue == "U")
            {
                txtPrecio.ReadOnly = false;
                                
            }
            //else
            //{
            //    valorVehiculo.Text = txtPrecio.Text;
            //}
            valorDescuento.Text       = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(mped_valodesc,0) as valorsindesc FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("C");

            //Ahora Cargamos los datos de los datos secundarios
            controlesRetomaVehiculos.Visible = true;
            btnAccion.Visible         = true;
            btnAccion.Text            = "Guardar Modificación";

            recordDescuento.Text      = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(mped_valodesc,0) FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("N");

            costoOtrosElementos.Text  = Llenar_Grilla_Pedido_Existente(prefijoPedidoModificacion, numeroPedidoModificacion).ToString("C");
            descripcionObsequios.Text = DBFunctions.SingleData("SELECT mped_obsequio FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            costoObsequios.Text       = DBFunctions.SingleData("SELECT mped_valoobse FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");

            //Ahora cargamos los datos relacionados con los pagos
            double valorParcialPgs    = 0;
            if (DBFunctions.SingleData("SELECT mped_valoefec FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "") != "")
            {
                pagoEfectivo.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valoefec FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("N");
                valorParcialPgs += Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valoefec FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
            }
            if (DBFunctions.SingleData("SELECT mped_valofinc FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "") != "")
            {
                pagoFinanciera.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valofinc FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("N");
                valorParcialPgs += Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valofinc FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
                ddlFinanciera.SelectedIndex = ddlFinanciera.Items.IndexOf(ddlFinanciera.Items.FindByValue(DBFunctions.SingleData("SELECT mped_nombfinc FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")));
            }
            if (DBFunctions.SingleData("SELECT mped_valocheq FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "") != "")
            {
                pagoCheques.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valocheq FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("N");
                valorParcialPgs += Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valocheq FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
            }
            if (DBFunctions.SingleData("SELECT mped_valootrpago FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "") != "")
            {
                valorOtroPago.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valootrpago FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "")).ToString("N");
                valorParcialPgs += Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valootrpago FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + ""));
                otroPago.Text = DBFunctions.SingleData("SELECT mped_detaotrpago FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            }
            valorParcialPagos.Text = valorParcialPgs.ToString("C");
            //Ahora cargamos los datos correspondientes a los datos del vehiculo de retoma
            LLenar_Grilla_Retoma_PedidoExistente(prefijoPedidoModificacion, numeroPedidoModificacion);
            valorTotalPagos.Text = (valorParcialPgs + Convert.ToDouble(valorTotalRetoma.Text.Substring(1))).ToString("C");
            diferenciaPagos.Text = (Convert.ToDouble(totalVenta.Text.Substring(1)) - Convert.ToDouble(valorTotalPagos.Text.Substring(1))).ToString("C");
            TextObserv.Text = DBFunctions.SingleData("SELECT mped_observacion FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");

        }

        protected double Llenar_Grilla_Pedido_Existente(string prefijoPedidoModificacion, string numeroPedidoModificacion)
        {
            double valorItemsVenta = 0;
            Preparar_Tabla_Elementos();
            DataSet elementosVentaPedido = new DataSet();
            DBFunctions.Request(elementosVentaPedido, IncludeSchema.NO, "SELECT pite_codigo, dped_valoitem, piva_porciva FROM dpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            for (int i = 0; i < elementosVentaPedido.Tables[0].Rows.Count; i++)
            {
                DataRow fila = tablaElementos.NewRow();
                fila[0] = elementosVentaPedido.Tables[0].Rows[i][0].ToString();
                fila[1] = DBFunctions.SingleData("SELECT pite_nombre FROM pitemventavehiculo WHERE pite_codigo='" + elementosVentaPedido.Tables[0].Rows[i][0].ToString() + "'");
                fila[2] = Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][1]);
                fila[3] = Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][2]);
                tablaElementos.Rows.Add(fila);
                valorItemsVenta += Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][1]);
                Binding_Grilla();
            }
            return valorItemsVenta;
        }

        protected void LLenar_Grilla_Retoma_PedidoExistente(string prefijoPedidoModificacion, string numeroPedidoModificacion)
        {
            DataSet pedidoRetoma = new DataSet();
            DBFunctions.Request(pedidoRetoma, IncludeSchema.NO, "SELECT pcat_codigo, dped_numecont, pano_ano, dped_numeplaca, dped_cuenimpu, dped_valoreci FROM dpedidovehiculoretoma WHERE pdoc_codigo='" + prefijoPedidoModificacion + "' AND mped_numepedi=" + numeroPedidoModificacion + "");
            if (pedidoRetoma.Tables[0].Rows.Count > 0)
            {
                DatasToControls.EstablecerDefectoRadioButtonList(retomaVehiculo, "Retoma Vehiculo");
                controlesRetomaVehiculos.Visible = true;
                for (int i = 0; i < pedidoRetoma.Tables[0].Rows.Count; i++)
                {
                    DataRow fila = tablaRetoma.NewRow();
                    fila[0] = pedidoRetoma.Tables[0].Rows[i][0].ToString();
                    fila[1] = pedidoRetoma.Tables[0].Rows[i][1].ToString();
                    fila[2] = pedidoRetoma.Tables[0].Rows[i][2].ToString();
                    fila[3] = pedidoRetoma.Tables[0].Rows[i][3].ToString();
                    fila[4] = pedidoRetoma.Tables[0].Rows[i][4].ToString();
                    fila[5] = Convert.ToDouble(pedidoRetoma.Tables[0].Rows[i][5].ToString());
                    tablaRetoma.Rows.Add(fila);
                }
                this.Binding_Grilla_Retoma();
            }
            else
            {
                DatasToControls.EstablecerDefectoRadioButtonList(retomaVehiculo, "Sin Retoma Vehiculo");
                controlesRetomaVehiculos.Visible = false;
            }
        }

        [Ajax.AjaxMethod]
        public DataSet Verificar_Cliente(string Cedula)
        {
            DataSet clientes = new DataSet();
            DBFunctions.Request(clientes, IncludeSchema.NO,
                @"select vn.nombre as NOMBRE, mn.mnit_telefono as TELEFONO, mn.mnit_celular as CELULAR, mn.mnit_email as EMAIL, mn.TNIT_TIPONIT as TIPONIT,
                mn.MNIT_REPRESENTANTE AS REPRESENTANTE_LEGAL, mn.MNIT_NITREPRESENTANTE AS NIT_REPRESENTANTE
                   from  dbxschema.mnit mn, dbxschema.vmnit vn where mn.mnit_nit = '" + Cedula + @"' and mn.mnit_Nit = vn.mnit_nit;
                 select    
                 ( dv.dvis_numevisi CONCAT '. Vendedor:' CONCAT pven.pven_nombre CONCAT '. Fecha de Contacto:' CONCAT  dv.dvis_fechprimcontacto CONCAT '. Proximo Contacto:'  
                 CONCAT dv.dvis_proxcontacto CONCAT '. Fecha Proximo:' CONCAT  COALESCE(dv.dvis_fechproxcontacto, '0001-01-01') CONCAT '. Sede:' CONCAT  p.palm_descripcion CONCAT '. Hecho hace:' CONCAT  
                 CAST( ( days (current date) - days (date(dv.dvis_fechprimcontacto)) ) as INT) CONCAT ' días. Observaciones:' CONCAT ddia.dvisc_obsercontacto) as CONTACTOS,
                 DV.PVEN_CODIGO    
                 from  DVISITADIARIACLIENTES dv, pvendedor pven, palmacen p, DVISITADIARIACLIENTEScontacto ddia, cvehiculos cv   
                 where dv.mnit_nit='" + Cedula + @"' and pven.pven_codigo = dv.pven_codigo and p.palm_almacen = pven.palm_almacen  
                  and  CAST( ( days (current date) - days (date(dv.dvis_fechprimcontacto)) ) as INT) <= (coalesce(cv.CVEH_DIASCOTI,30) + 10)  
                  and  ddia.pdoc_codigo = dv.pdoc_codigo and ddia.dvis_numevisi=dv.dvis_numevisi  
                  and  ddia.dvisc_numcontacto = dv.dvis_numecontacto order by dv.dvis_fechprimcontacto DESC;
                SELECT CVEH_COTICLIEEXIS FROM CVEHICULOS;");

//            DBFunctions.Request(clientes, IncludeSchema.NO,
//                @"select mnit_nombres concat ' ' concat mnit_apellidos as NOMBRE, mnit_telefono as TELEFONO,  
//                 mnit_celular as CELULAR, mnit_email as EMAIL from dbxschema.mnit where mnit_nit='" + Cedula + @"'; 
//                 select  
//                 ( dv.dvis_numevisi CONCAT '. Vendedor:' CONCAT pven.pven_nombre CONCAT '. Fecha de Contacto:' CONCAT  dv.dvis_fechprimcontacto CONCAT '. Proximo Contacto:'  
//                 CONCAT dv.dvis_proxcontacto CONCAT '. Fecha Proximo:' CONCAT  dv.dvis_fechproxcontacto CONCAT '. Sede:' CONCAT  p.palm_descripcion CONCAT '. Hecho hace:' CONCAT  
//                 CAST( ( days (current date) - days (date(dv.dvis_fechprimcontacto)) ) as INT) CONCAT ' días. Observaciones:' CONCAT ddia.dvisc_obsercontacto) as CONTACTOS  
//                 from DVISITADIARIACLIENTES dv, pvendedor pven, palmacen p, DVISITADIARIACLIENTEScontacto ddia  
//                 where dv.mnit_nit='" + Cedula + @"' and pven.pven_codigo = dv.pven_codigo and p.palm_almacen = pven.palm_almacen  
//                 and CAST( ( days (current date) - days (date(dv.dvis_fechprimcontacto)) ) as INT) <= 120 
//                 and ddia.pdoc_codigo = dv.pdoc_codigo and ddia.dvis_numevisi=dv.dvis_numevisi  
//                 and ddia.dvisc_numcontacto = dv.dvis_numecontacto order by dv.dvis_fechprimcontacto DESC;");
            return clientes;
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
