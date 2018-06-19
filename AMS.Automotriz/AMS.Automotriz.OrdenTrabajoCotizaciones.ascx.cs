// created on 10/09/2004 at 13:19
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.DBManager;
using AMS.Tools;

// anotacion hecha el 20 de septiembre de 2004 a las 12:13
// Para lograr conectar el control de usuario principal con los controles secundarios
// que se cargan cada vez que se da click en los botones de imagen se utiliza el session como forma de conexion
// el objeto session. Dentro de cada subcontrol se debe dar click en confirmar si no se hace no funcionara 
// el guardar la orden de trabajo.

namespace AMS.Automotriz
{
    public partial class OrdenesTallerCotizaciones : System.Web.UI.UserControl
    {
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected DataTable accesoriosGrabar, repuestosGrabar, operacionesGrabar, operacionesPeritajeGrabar;
        protected ArrayList cargoGrabar;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        protected string exceptions;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            Page.SmartNavigation = true;
            grabar.Attributes.Add("Onclick", "document.getElementById('" + grabar.ClientID + "').disabled = true;" + this.Page.GetPostBackEventReference(grabar) + ";");
            try
            {
                datosOrigen.Controls.Add(LoadControl(pathToControls + "AMS.Automotriz.OrdenTrabajoCotizaciones.DatosOrden.ascx"));
                datosPropietario.Controls.Add(LoadControl(pathToControls + "AMS.Automotriz.OrdenTrabajo.DatosPropietario.ascx"));
                datosVehiculo.Controls.Add(LoadControl(pathToControls + "AMS.Automotriz.OrdenTrabajo.DatosVehiculo.ascx"));
                kitsCombos.Controls.Add(LoadControl(pathToControls + "AMS.Automotriz.OrdenTrabajo.KitsCombos.ascx"));
                operacionesPeritaje.Controls.Add(LoadControl(pathToControls + "AMS.Automotriz.OrdenTrabajo.OperacionesPeritaje.ascx"));
            }
            catch (Exception exc)
            {
                lb.Text += "Error : " + exc.ToString();
            }
            if (!IsPostBack)
            {
                Session.Clear();
                Session["grupoCatalogo"] = "";
                propietario.Enabled = false;
                vehiculo.Enabled = false;
                botonKits.Enabled = false;
                opPeritaje.Enabled = false;

                //IMPRIMIR REPORTES
                if (Request["prefOT"] != null && Request["prefOT"] != string.Empty && Request["numOT"] != null && Request["numOT"] != string.Empty)
                {
                    try
                    {
                        //ORDEN
                        formatoRecibo.Prefijo = Request["prefOT"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefOT"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                        }
                        if (Request["pres"].Equals("S"))
                        {
                            //ORDEN (PRESUPUESTO)
                            formatoRecibo.Prefijo = Request["prefOT"];
                            formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefOT"] + "'");
                            if (formatoRecibo.Codigo != string.Empty)
                            {
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                            }
                        }
                        //TRANSFERENCIA
                        formatoRecibo.Prefijo = Request["prefTRA"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numTRA"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefTRA"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                        }
                        //PEDIDO
                        formatoRecibo.Prefijo = Request["tipoPED"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numPED"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.PPEDIDO WHERE pped_codigo='" + Request.QueryString["tipoPED"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                        }
                    }
                    catch
                    {
                        //lbInfo.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
                        Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
                    }
                }
            }
            grabar.Enabled = false;
        }

        #region Mostrar/Ocultar Paneles
        protected void Cargar_DatosOrden(Object Sender, ImageClickEventArgs E)
        {
            if (origen.ImageUrl == "../img/AMS.BotonExpandir.png")
            {
                datosOrigen.Visible = true;
                origen.ImageUrl = "../img/AMS.BotonContraer.png";
            }
            else if (origen.ImageUrl == "../img/AMS.BotonContraer.png")
            {
                datosOrigen.Visible = false;
                origen.ImageUrl = "../img/AMS.BotonExpandir.png";
            }
        }

        protected void Cargar_DatosPropietario(Object Sender, ImageClickEventArgs E)
        {
            if (propietario.ImageUrl == "../img/AMS.BotonExpandir.png")
            {
                datosPropietario.Visible = true;
                propietario.ImageUrl = "../img/AMS.BotonContraer.png";
            }
            else if (propietario.ImageUrl == "../img/AMS.BotonContraer.png")
            {
                datosPropietario.Visible = false;
                propietario.ImageUrl = "../img/AMS.BotonExpandir.png";
            }
        }

        protected void Cargar_DatosVehiculo(Object Sender, ImageClickEventArgs E)
        {
            if (vehiculo.ImageUrl == "../img/AMS.BotonExpandir.png")
            {
                datosVehiculo.Visible = true;
                vehiculo.ImageUrl = "../img/AMS.BotonContraer.png";
            }
            else if (vehiculo.ImageUrl == "../img/AMS.BotonContraer.png")
            {
                datosVehiculo.Visible = false;
                vehiculo.ImageUrl = "../img/AMS.BotonExpandir.png";
            }
        }

        protected void Cargar_KitsCombos(Object Sender, ImageClickEventArgs E)
        {
            if (botonKits.ImageUrl == "../img/AMS.BotonExpandir.png")
            {
                kitsCombos.Visible = true;
                botonKits.ImageUrl = "../img/AMS.BotonContraer.png";
            }
            else if (botonKits.ImageUrl == "../img/AMS.BotonContraer.png")
            {
                kitsCombos.Visible = false;
                botonKits.ImageUrl = "../img/AMS.BotonExpandir.png";
            }
        }

        protected void Cargar_Operaciones_Peritaje(Object Sender, ImageClickEventArgs E)
        {
            if (opPeritaje.ImageUrl == "../img/AMS.BotonExpandir.png")
            {
                operacionesPeritaje.Visible = true;
                opPeritaje.ImageUrl = "../img/AMS.BotonContraer.png";
            }
            else if (opPeritaje.ImageUrl == "../img/AMS.BotonContraer.png")
            {
                operacionesPeritaje.Visible = false;
                opPeritaje.ImageUrl = "../img/AMS.BotonExpandir.png";
            }
        }

        #endregion Mostrar/Ocultar Paneles
        //FUNCION PARA GRABAR LA ORDEN
        protected void Grabar_Orden(Object Sender, EventArgs e)
        {
            grabar.Enabled = false;
            Utils.MostrarAlerta(Response, "Grabación de la Orden. Acepte, espere e imprima los formatos !!!");

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////			
            //A continuacion cargamos los controles que contienen los datos necesarios para grabar la orden de trabajo en la base de datos
            Control controlDatosOrden = datosOrigen.Controls[0];
            Control controlDatosVehiculo = datosVehiculo.Controls[0];
            Control controlDatosPropietario = datosPropietario.Controls[0];
            Control controlKitsCombos = kitsCombos.Controls[0];
            Control controlPeritaje = operacionesPeritaje.Controls[0];

            //Aun falta por revisar si hubo cambio de PROPIETARIO, cuando hay cambio de dueño debemos actualizar mcatalogovehiculo
            string nit = ((TextBox)controlDatosPropietario.FindControl("datos")).Text;
            string contacto = ((TextBox)controlDatosPropietario.FindControl("contacto")).Text;
            string vin = ((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text;
            string tipoUsuario = DBFunctions.SingleData("SELECT tpro_codigo FROM tpropietariotaller WHERE tpro_nombre='" + ((RadioButtonList)controlDatosPropietario.FindControl("tipoCliente")).SelectedItem.ToString().Trim() + "'");

            if (DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_vin='" + vin + "'") != nit && tipoUsuario == "P")
                DBFunctions.NonQuery("UPDATE mcatalogovehiculo SET mnit_nit='" + nit + "' WHERE mcat_vin='" + vin + "'");

            //Ahora vamos a revisar si la orden de trabajo viene por parte de un seguro, o por cualquiera de los dos tipos de garantia
            string cargo = ((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedValue;
            //
            if (cargo == "S")
            {
                cargoGrabar = new ArrayList();
                cargoGrabar.Add("seguro");
                cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Text);
                cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("siniestro")).Text);
                cargoGrabar.Add(Convert.ToDouble(((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Text.Trim()).ToString());
                cargoGrabar.Add(Convert.ToDouble(((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Text.Trim()).ToString());
                cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Text);
            }
            else if (cargo == "G")
            {
                cargoGrabar = new ArrayList();
                cargoGrabar.Add("garantia");
                cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("nitCompania")).Text);
                cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionGarant")).Text);
            }
            else
                cargoGrabar = null;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////I
            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Fin de revision de los cargos de la orden
            // Se llenan los datables que van a ser enviados a la clase de Orden en el contructor			
            if (!Distribuir_Tablas( ((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")), DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedItem.ToString() + "'"), ((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim()) && (!Distribuir_Tablas_Repuestos()))
            {
                Orden miOrden = null;
                Cotizacion miCotizacion = new Cotizacion();
                //string tipoTrabajo = DBFunctions.SingleData("SELECT ttra_codigo FROM ttrabajoorden WHERE ttra_nombre='" + ((DropDownList)controlDatosOrden.FindControl("servicio")).SelectedItem.ToString().Trim() + "'");
                string tipoTrabajo = ((DropDownList)controlDatosOrden.FindControl("servicio")).SelectedValue;
                if (tipoTrabajo == "P" || tipoTrabajo == "C")
                {
                    int cantidadGrillas = System.Convert.ToInt32(Session["cantidadGrillas"]);
                    DataTable[] tablasAsociadas;
                    tablasAsociadas = new DataTable[cantidadGrillas];
                    for (int i = 0; i < cantidadGrillas; i++)
                    {
                        tablasAsociadas[i] = ((DataTable[])Session["tablasAsociadas"])[i].Copy();
                    }
                    Distribuir_Tabla_Peritaje(((PlaceHolder)controlPeritaje.FindControl("gruposPeritaje")), 
                        tablasAsociadas, 
                        ((ArrayList)Session["codigosGruposPeritaje"]), 
                        cantidadGrillas, 
                        DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedItem.ToString() + "'"), 
                        ((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim());

                    bool grabarTransferencias = tipoTrabajo == "P"; // cuando es peritaje si, cuando es cotización no

                    if (tipoTrabajo == "P")
                        miOrden = new Orden(operacionesGrabar, accesoriosGrabar, operacionesPeritajeGrabar, cargoGrabar, grabarTransferencias);
                    else
                        miOrden = new Orden(operacionesGrabar, accesoriosGrabar, operacionesPeritajeGrabar, cargoGrabar, repuestosGrabar, grabarTransferencias);
                }
                else
                    miOrden = new Orden(operacionesGrabar, accesoriosGrabar, repuestosGrabar, cargoGrabar);
                //En caso que sea una orden de trabajo la grabamos aqui	
                miOrden.CodigoPrefijo = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedItem.ToString() + "'");
                miOrden.NumeroOrden = ((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim();
                miOrden.Catalogo = ((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue;
                miOrden.VinIdentificacion = ((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text;
                miOrden.NitPropietario = ((TextBox)controlDatosPropietario.FindControl("datos")).Text;
                miOrden.TipoUsuario = tipoUsuario;
                miOrden.EstadoOrden = "E";
                miOrden.Cargo = ((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedValue;
                miOrden.TipoTrabajo = tipoTrabajo;
                miOrden.FechaEntrada = ((TextBox)controlDatosOrden.FindControl("fecha")).Text.Trim();
                miOrden.HoraEntrada = ((TextBox)controlDatosOrden.FindControl("hora")).Text.Trim() + ":" + ((TextBox)controlDatosOrden.FindControl("minutos")).Text.Trim();
                miOrden.FechaHoraCreacion = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                miOrden.FechaEntrega = UtilitarioPlanning.ValidarParametrosFecha(Convert.ToDateTime(((TextBox)controlKitsCombos.FindControl("fechaEstimada")).Text)).ToString("yyyy-MM-dd");
                miOrden.HoraEntrega = ((TextBox)controlKitsCombos.FindControl("horaEstimada")).Text;
                miOrden.Salida = "NULL";
                miOrden.NumeroEntrada = "1";
                miOrden.Kilometraje = System.Convert.ToDouble(((TextBox)controlDatosVehiculo.FindControl("kilometraje")).Text.Trim()).ToString();
                miOrden.Recepcionista = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + ((DropDownList)controlDatosOrden.FindControl("codigoRecep")).SelectedItem.ToString().Trim() + "'");
                miOrden.Taller = DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and palm_descripcion='" + ((DropDownList)controlDatosOrden.FindControl("almacen")).SelectedItem.ToString().Trim() + "'");
                miOrden.EstadoPrecios = "A";
                miOrden.NumeroLocker = "1";
                miOrden.EstadoLiquidacion = "L";
                miOrden.NivelCombustible = "1";
                miOrden.ObsrCliente = ((TextBox)controlDatosOrden.FindControl("obsrCliente")).Text;
                miOrden.ObsrRecepcionista = ((TextBox)controlDatosOrden.FindControl("obsrRecep")).Text;
                miOrden.ListaPrecios = DBFunctions.SingleData("SELECT ppreta_codigo FROM ppreciotaller WHERE ppreta_nombre='" + ((DropDownList)controlDatosOrden.FindControl("listaPrecios")).SelectedItem.ToString().Trim() + "'");
                miOrden.TipoPago = DBFunctions.SingleData("SELECT ttip_codigo FROM ttipopago WHERE ttip_nombre='" + ((DropDownList)controlDatosPropietario.FindControl("tipoPago")).SelectedItem.ToString() + "'");
                miOrden.CodigoEstadoCita = ((Label)controlDatosOrden.FindControl("lbEstCita")).Text.Trim();
                miOrden.NitTransferencia = DBFunctions.SingleData("SELECT pnital_nittaller FROM pnittaller WHERE palm_almacen='" + miOrden.Taller + "' fetch first row only");
                miOrden.TipoTransferencia = ((DropDownList)controlDatosOrden.FindControl("tipoPedido")).SelectedValue;
                miOrden.PrefijoTransferencia = DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE tdoc_tipodocu='TT' fetch first row only"); // debería parametrizarse en el web.config
                miOrden.TotalTransferencia = this.Calcular_Total_Transferencia();
                miOrden.TotalTransferenciaSinIVA = this.Calcular_Total_Transferencia_SinIVA();
                miOrden.AlmacenTransferencia = DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and pcen_centinv is not null fetch first row only"); // debería parametrizarse en el web.config
                miOrden.KitsAplicados = (ArrayList)Session["escogidos2"];
                miOrden.Contacto = contacto;
                miOrden.AceptaEncuesta = true;
                miOrden.RevisionElevador = true;
                miOrden.EntregoPresupuesto = true;
                
                if (grabar.Text == "Guardar Modificación")
                {
                    if (miOrden.Actualizar_Orden_Trabajo(true))
                        Utils.MostrarAlerta(Response, "La orden ha sido actualizada");
                    else
                        lb.Text = miOrden.ProcessMsg;
                }
                else if (grabar.Text == "Guardar Orden Preliquidada")
                {
                    if (miOrden.Actualizar_Orden_Trabajo(false))
                        lb.Text = miOrden.ProcessMsg;
                    else
                        lb.Text = miOrden.ProcessMsg;
                }

                else if (miOrden.CommitValues())
                {
                    string numPedido = DBFunctions.SingleData("Select MPED_NUMERO from DBXSCHEMA.MPEDIDOTRANSFERENCIA where MORD_NUMEORDE=" + miOrden.NumeroOrden + " AND PPED_CODIGO='" + miOrden.TipoTransferencia + "';");
                    string numTransferencia = DBFunctions.SingleData("Select MFAC_NUMERO from DBXSCHEMA.MORDENTRANSFERENCIA where MORD_NUMEORDE=" + miOrden.NumeroOrden + " AND PDOC_FACTURA='" + miOrden.PrefijoTransferencia + "';");
                    string presupuesto = miOrden.EntregoPresupuesto ? "S" : "N";
                    lb.Text = "<br>BIEN : " + miOrden.ProcessMsg;
                    Session.Clear();
                    Response.Redirect("" + indexPage + "?process=Automotriz.OrdenTrabajoCotizaciones&prefOT=" + miOrden.CodigoPrefijo + "&numOT=" + miOrden.NumeroOrden + "&prefTRA=" + miOrden.PrefijoTransferencia + "&numTRA=" + numTransferencia + "&tipoPED=" + miOrden.TipoTransferencia + "&numPED=" + numPedido + "&pres=" + presupuesto);
                }
                else
                    lb.Text += "<br>MAL :" + miOrden.ProcessMsg;
            }
            else
                Utils.MostrarAlerta(Response, "Existe Algun Elemento Repetido, Por Favor Verifique las Tablas");
        }

        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        protected void Preparar_Tablas()
        {
            //distribuimos la estructura de la tabla de operaciones
            operacionesGrabar = new DataTable();
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOPREFIJO", System.Type.GetType("System.String")));//0
            operacionesGrabar.Columns.Add(new DataColumn("NUMEROORDEN", System.Type.GetType("System.String")));//1
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOOPERACION", System.Type.GetType("System.String")));//2
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOCARGO", System.Type.GetType("System.String")));//3
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOMECANICO", System.Type.GetType("System.String")));//4
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOESTADO", System.Type.GetType("System.String")));//5
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOINCIDENTE", System.Type.GetType("System.String")));//6
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOGARANTIA", System.Type.GetType("System.String")));//7
            operacionesGrabar.Columns.Add(new DataColumn("CODIGOREMEDIO", System.Type.GetType("System.String")));//8
            operacionesGrabar.Columns.Add(new DataColumn("CODIGODEFECTO", System.Type.GetType("System.String")));//9
            operacionesGrabar.Columns.Add(new DataColumn("VALOROPERACION", System.Type.GetType("System.Double")));//10
            operacionesGrabar.Columns.Add(new DataColumn("TIEMPOOPERACION", System.Type.GetType("System.Double")));//11
            operacionesGrabar.Columns.Add(new DataColumn("OBSERVACION", System.Type.GetType("System.String")));//12
        }

        protected void Preparar_Tabla_Repuestos()
        {
            //distribuimos la estructura de la tabla de repuestos
            repuestosGrabar = new DataTable();
            repuestosGrabar.Columns.Add(new DataColumn("CODIGOITEM", System.Type.GetType("System.String")));//0
            repuestosGrabar.Columns.Add(new DataColumn("CANTIDADITEM", System.Type.GetType("System.Double")));//1
            repuestosGrabar.Columns.Add(new DataColumn("PRECIOITEM", System.Type.GetType("System.Double")));//2
            repuestosGrabar.Columns.Add(new DataColumn("IVAITEM", System.Type.GetType("System.Double")));//3
            repuestosGrabar.Columns.Add(new DataColumn("DESCUENTO", System.Type.GetType("System.Double")));//4
            repuestosGrabar.Columns.Add(new DataColumn("CANTIDADPEDIDA", System.Type.GetType("System.Double")));//5
            repuestosGrabar.Columns.Add(new DataColumn("CARGOITEM", System.Type.GetType("System.String")));//6
        }

        //Esta funcion nos permite cargar la tabla especifica para poder guardar las operaciones especificas 
        protected void Preparar_Tabla_Peritaje()
        {
            operacionesPeritajeGrabar = new DataTable();
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("CODIGOPREFIJO", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("NUMEROORDEN", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("GRUPOPERITAJE", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("ITEMPERITAJE", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("ESTADO", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("DETALLE", System.Type.GetType("System.String")));
            operacionesPeritajeGrabar.Columns.Add(new DataColumn("COSTO", System.Type.GetType("System.String")));
        }

        /////////////////////////////////////////////////////////////////////////

        protected bool Distribuir_Tablas_Repuestos()
        {
            bool itemRepetido = false;
            Preparar_Tabla_Repuestos();
            DataTable tablaItems = (DataTable)Session["items"];
            if (tablaItems != null)
            {
                for (int i = 0; i < tablaItems.Rows.Count; i++)
                {
                    DataRow fila = repuestosGrabar.NewRow();
                    fila[0] = tablaItems.Rows[i][10].ToString();
                    fila[1] = Convert.ToDouble(tablaItems.Rows[i][5]);
                    fila[2] = Convert.ToDouble(tablaItems.Rows[i][4]);
                    fila[3] = Convert.ToDouble(tablaItems.Rows[i][7]);
                    fila[4] = 0;
                    fila[5] = Convert.ToDouble(tablaItems.Rows[i][5]);
                    fila[6] = DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre='" + tablaItems.Rows[i][9].ToString().Trim() + "'");
                    repuestosGrabar.Rows.Add(fila);
                }
                for (int i = 0; i < repuestosGrabar.Rows.Count; i++)
                {
                    for (int j = i + 1; j < repuestosGrabar.Rows.Count; j++)
                    {
                        if ((repuestosGrabar.Rows[i][0].ToString()) == (repuestosGrabar.Rows[j][0].ToString()))
                            itemRepetido = true;
                    }
                }
            }
            return itemRepetido;
        }

        protected double Calcular_Total_Transferencia()
        {
            double total = 0;
            if (repuestosGrabar != null)
            {
                for (int i = 0; i < repuestosGrabar.Rows.Count; i++)
                    total += (System.Convert.ToDouble(repuestosGrabar.Rows[i][2]) + (System.Convert.ToDouble(repuestosGrabar.Rows[i][2]) * (System.Convert.ToDouble(repuestosGrabar.Rows[i][3]) / 100))) * (System.Convert.ToDouble(repuestosGrabar.Rows[i][1]));
            }
            return total;
        }

        protected double Calcular_Total_Transferencia_SinIVA()
        {
            double total = 0;
            if (repuestosGrabar != null)
            {
                for (int i = 0; i < repuestosGrabar.Rows.Count; i++)
                    total += System.Convert.ToDouble(repuestosGrabar.Rows[i][2]);
            }
            return total;
        }

        protected bool Distribuir_Tablas(DataGrid operaciones, string codigoPrefijo, string numeroOrden)
        {
            int i;
            bool operacionRepetida = false;
            //Se preparan las datatables que contendran los datos a grabar en la base de datos
            Preparar_Tablas();

            // A continuacion vamos a llenar la tabla que contendra las operaciones de la orden
            // Debemos revisar si la operacion aun se encuentra activa o no
            DataTable tablaOperaciones = new DataTable();
            tablaOperaciones = (DataTable)Session["operaciones"];
            Control controlDatosOrden = datosOrigen.Controls[0];

            for (i = 0; i < tablaOperaciones.Rows.Count; i++)
            {
                if (((Button)operaciones.Items[i].Cells[14].Controls[1]).Text == "Remover")
                {
                    DataRow fila = operacionesGrabar.NewRow();
                    fila[0] = codigoPrefijo;
                    fila[1] = numeroOrden;
                    fila[2] = tablaOperaciones.Rows[i][1].ToString();
                    fila[3] = DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre='" + tablaOperaciones.Rows[i][7].ToString().Trim() + "'");
                    if (fila[3].ToString().Length == 0)
                        fila[3] = tablaOperaciones.Rows[i][7].ToString().Trim();
                    fila[4] = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + tablaOperaciones.Rows[i][6].ToString().Trim() + "'");
                    fila[5] = DBFunctions.SingleData("SELECT test_estaoper FROM testadooperacion WHERE test_nombre LIKE '%" + tablaOperaciones.Rows[i][8].ToString() + "%'");
                    if (!((DropDownList)operaciones.Items[i].Cells[10].Controls[1]).Visible)
                        //			fila[6] = "0";
                        fila[6] = "Null";
                    else
                        fila[6] = ((DropDownList)operaciones.Items[i].Cells[10].Controls[1]).SelectedValue;
                    if (!((DropDownList)operaciones.Items[i].Cells[11].Controls[1]).Visible)
                        //			fila[7] = "0";
                        fila[7] = "Null";
                    else
                        fila[7] = ((DropDownList)operaciones.Items[i].Cells[11].Controls[1]).SelectedValue;
                    if (!((DropDownList)operaciones.Items[i].Cells[12].Controls[1]).Visible)
                        //			fila[8] = "0";
                        fila[8] = "Null";
                    else
                        fila[8] = ((DropDownList)operaciones.Items[i].Cells[12].Controls[1]).SelectedValue;
                    if (!((DropDownList)operaciones.Items[i].Cells[13].Controls[1]).Visible)
                        //			fila[9] = "0";
                        fila[9] = "Null";
                    else
                        fila[9] = ((DropDownList)operaciones.Items[i].Cells[13].Controls[1]).SelectedValue;
                    //vamos a cargar el valor que tiene la operacion  
                    if (DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='" +
                        tablaOperaciones.Rows[i][1].ToString() + "'") == "B")
                        fila[10] = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text.Trim());
                    else
                        fila[10] = Convert.ToDouble(tablaOperaciones.Rows[i][4].ToString().Trim());
                    fila[11] = Convert.ToDouble(tablaOperaciones.Rows[i][3]);
                    fila[12] = ((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text;
                    operacionesGrabar.Rows.Add(fila);
                }
            }
            //Comprobacion de que una operacion esta repetida o no
            for (i = 0; i < operacionesGrabar.Rows.Count; i++)
            {
                for (int j = i + 1; j < operacionesGrabar.Rows.Count; j++)
                {
                    if ((operacionesGrabar.Rows[i][2].ToString()) == (operacionesGrabar.Rows[j][2].ToString()))
                        operacionRepetida = true;
                }
            }

            return operacionRepetida;
        }

        protected void Distribuir_Tabla_Peritaje(PlaceHolder gruposPeritaje, DataTable[] tablasAsociadas, ArrayList codigosGruposPeritaje, int cantidadGrillas, string codigoPrefijo, string numeroOrden)
        {
            this.Preparar_Tabla_Peritaje();
            for (int i = 0; i < cantidadGrillas; i++)
            {
                int cantidadOperaciones = tablasAsociadas[i].Rows.Count;
                for (int j = 0; j < cantidadOperaciones; j++)
                {
                    DataRow fila = operacionesPeritajeGrabar.NewRow();
                    fila[0] = codigoPrefijo;
                    fila[1] = numeroOrden;
                    fila[2] = codigosGruposPeritaje[i].ToString();
                    fila[3] = DBFunctions.SingleData("SELECT pitp_codigo FROM pitemperitaje WHERE pitp_descripcion='" + tablasAsociadas[i].Rows[j][0].ToString() + "' AND pgrp_codigo='" + codigosGruposPeritaje[i].ToString() + "'");
                    fila[4] = DBFunctions.SingleData("SELECT tespe_codigo FROM testadoperitaje WHERE tespe_descripcion='" + ((RadioButtonList)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[1].Controls[0]).SelectedItem.ToString() + "'");
                    if ((((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[2].Controls[0]).Text) == "")
                        fila[5] = "";
                    else
                        fila[5] = ((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[2].Controls[0]).Text;
                    if ((((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[3].Controls[0]).Text) == "")
                        fila[6] = "null";
                    else
                        fila[6] = ((TextBox)((DataGrid)gruposPeritaje.Controls[((i * 2) + 1)]).Items[j].Cells[3].Controls[0]).Text;
                    operacionesPeritajeGrabar.Rows.Add(fila);
                }
            }
        }

        protected void btnCancelar_Click(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=Automotriz.OrdenTrabajoCotizaciones");
        }

        ////////////////////////////////////////////////
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
