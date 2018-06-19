// created on 27/10/2004 at 14:33
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
using AMS.Tools;


namespace AMS.Automotriz
{
	public partial class EstadoOrdenes : System.Web.UI.UserControl
	{
		protected DataTable ordenesProceso, operacionesOrdenesGrilla, repuestosOrdenesGrilla;
        private DatasToControls bind = new DatasToControls();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.ClearChildViewState();
			if(!IsPostBack)
			{
				this.Distribuir_Ordenes();
			}
			else 
			{
				if (Session["ordenesProceso"]!=null)
					ordenesProceso = (DataTable)Session["ordenesProceso"];
				else if (Session["ordenesProceso"]==null)
					this.Distribuir_Ordenes();
			}
		}
		
		
		protected void dgSeleccion_Ordenes(object sender, DataGridCommandEventArgs e)
		{
			Control principal = (this.Parent).Parent;
			Control controlDatosOrden = ((PlaceHolder)principal.FindControl("datosOrigen")).Controls[0];
			Control controlDatosPropietario = ((PlaceHolder)principal.FindControl("datosPropietario")).Controls[0];
			Control controlDatosVehiculo = ((PlaceHolder)principal.FindControl("datosVehiculo")).Controls[0];
			Control controlDatosAccesorios = ((PlaceHolder)principal.FindControl("otrosDatos")).Controls[0];
			Control controlDatosKitsCombos = ((PlaceHolder)principal.FindControl("kitsCombos")).Controls[0];
			Control controlDatosPeritaje = ((PlaceHolder)principal.FindControl("operacionesPeritaje")).Controls[0];
            //((ImageButton)principal.FindControl("propietario")).Enabled = true;
            //((ImageButton)principal.FindControl("vehiculo")).Enabled = true;
            //((ImageButton)principal.FindControl("otros")).Enabled = true;
            //((ImageButton)principal.FindControl("botonKits")).Enabled = true;		
            try
            {
                if (((Button)e.CommandSource).CommandName == "modificar")
                {
                    ((Button)principal.FindControl("grabar")).Text = "Guardar Modificación";
                    ((Button)principal.FindControl("grabar")).Enabled = true;

                    string cargo = this.Distribuir_Datos_Orden(true, controlDatosOrden, ordenesProceso.Rows[e.Item.ItemIndex][0].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][4].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][2].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][5].ToString());

                    //this.Distribuir_Datos_Propietario(true,controlDatosPropietario,DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+ordenesProceso.Rows[e.Item.ItemIndex][0].ToString()+"'"),ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(),DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='"+ordenesProceso.Rows[e.Item.ItemIndex][4].ToString()+"'"));
                    //this.Distribuir_Datos_Vehiculo(true,controlDatosVehiculo,DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+ordenesProceso.Rows[e.Item.ItemIndex][0].ToString()+"'"),ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(),ordenesProceso.Rows[e.Item.ItemIndex][4].ToString());
                    //this.Distribuir_Datos_Accesorios(true,controlDatosAccesorios,DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+ordenesProceso.Rows[e.Item.ItemIndex][0].ToString()+"'"),ordenesProceso.Rows[e.Item.ItemIndex][1].ToString());
                    //this.Distribuir_Datos_KitsCombos(true,controlDatosKitsCombos,DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+ordenesProceso.Rows[e.Item.ItemIndex][0].ToString()+"'"),ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(),grupoCatalogo,cargo);

                    this.Distribuir_Datos_Propietario(true, controlDatosPropietario, ordenesProceso.Rows[e.Item.ItemIndex][0].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='" + ordenesProceso.Rows[e.Item.ItemIndex][4].ToString() + "'"));
                    this.Distribuir_Datos_Vehiculo(true, controlDatosVehiculo, ordenesProceso.Rows[e.Item.ItemIndex][0].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][4].ToString());
                    this.Distribuir_Datos_Accesorios(true, controlDatosAccesorios, ordenesProceso.Rows[e.Item.ItemIndex][0].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString());
                    string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='" + DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='" + ordenesProceso.Rows[e.Item.ItemIndex][4].ToString() + "'") + "'");
                    this.Distribuir_Datos_KitsCombos(true, controlDatosKitsCombos, ordenesProceso.Rows[e.Item.ItemIndex][0].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), grupoCatalogo, cargo);

                    if (this.Distribuir_Datos_Peritaje(controlDatosPeritaje, DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ordenesProceso.Rows[e.Item.ItemIndex][0].ToString() + "'"), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString()))
                    {
                        //Aqui mostramos el placeholder que tiene los datos del peritaje y desactivamos el boton para que no lo pueda ocultar
                        ((ImageButton)principal.FindControl("opPeritaje")).Enabled = false;
                        ((PlaceHolder)principal.FindControl("operacionesPeritaje")).Visible = true;
                        ((ImageButton)principal.FindControl("opPeritaje")).ImageUrl = "../img/AMS.BotonContraer.png";
                    }

                    ViewState["MODIFICAR"] = true;
                    this.Ocultar_Control();
                }
                else if (((Button)e.CommandSource).CommandName == "preliquidar")
                {
                    ((Button)principal.FindControl("grabar")).Text = "Guardar Orden Preliquidada";
                    ((Button)principal.FindControl("grabar")).Enabled = true;
                    string cargo = this.Distribuir_Datos_Orden(false, controlDatosOrden, DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ordenesProceso.Rows[e.Item.ItemIndex][0].ToString() + "'"), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][4].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][2].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][5].ToString());
                    this.Distribuir_Datos_Propietario(false, controlDatosPropietario, DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ordenesProceso.Rows[e.Item.ItemIndex][0].ToString() + "'"), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='" + ordenesProceso.Rows[e.Item.ItemIndex][4].ToString() + "'"));
                    this.Distribuir_Datos_Vehiculo(false, controlDatosVehiculo, DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ordenesProceso.Rows[e.Item.ItemIndex][0].ToString() + "'"), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), ordenesProceso.Rows[e.Item.ItemIndex][4].ToString());
                    this.Distribuir_Datos_Accesorios(false, controlDatosAccesorios, DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ordenesProceso.Rows[e.Item.ItemIndex][0].ToString() + "'"), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString());
                    string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='" + DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='" + ordenesProceso.Rows[e.Item.ItemIndex][4].ToString() + "'") + "'");
                    this.Distribuir_Datos_KitsCombos(false, controlDatosKitsCombos, DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + ordenesProceso.Rows[e.Item.ItemIndex][0].ToString() + "'"), ordenesProceso.Rows[e.Item.ItemIndex][1].ToString(), grupoCatalogo, cargo);
                    ViewState["MODIFICAR"] = true;
                    this.Ocultar_Control();
                }
            }
            catch { }
			
		}

        protected void DgUpdate_Page(Object sender, DataGridPageChangedEventArgs e)
        {
            this.ordenes.CurrentPageIndex = e.NewPageIndex;
            this.ordenes.DataSource = (DataTable)Session["ordenesProceso"];
            this.ordenes.DataBind();
            DatasToControls.JustificacionGrilla(ordenes, (DataTable)Session["ordenesProceso"]);
        }

        protected void Ocultar_Control()
		{
			Control principal = (this.Parent).Parent; //Control principal ascx
			//((PlaceHolder)this.Parent).Visible = false;
			((PlaceHolder)principal.FindControl("datosOrigen")).Visible = true;
			//((ImageButton)principal.FindControl("estOrdenes")).ImageUrl="../img/AMS.BotonExpandir.png";
			//((ImageButton)principal.FindControl("origen")).ImageUrl="../img/AMS.BotonContraer.png";
		}
		
		///Funcion que distribuye los datos de la orden
		
		protected string Distribuir_Datos_Orden(bool tipoOperacion, Control controlDatosOrden, string prefijo, string numeroOrden, string placa, string entrada,string fechaEntrada)
		{
            Control principal = (this.Parent).Parent;
            string almacenVend = DBFunctions.SingleData("SELECT palm_almacen FROM morden WHERE pdoc_codigo='" + prefijo + "' AND mord_numeorde=" + numeroOrden + "");
            //Traemos el nit de la transferencia, el tipo de transferencia y el prefijo de la factura de taller
            string nitTransferencia = DBFunctions.SingleData("SELECT MPED.mnit_nit FROM mpedidoitem MPED, mpedidotransferencia MTRA WHERE MPED.pped_codigo=MTRA.pped_codigo AND MPED.mped_numepedi=MTRA.mped_numero AND MTRA.pdoc_codigo='"+prefijo+"' AND MTRA.mord_numeorde="+numeroOrden+"");
			string tipoTransferencia = DBFunctions.SingleData("SELECT pped_nombre FROM ppedido WHERE pped_codigo=(SELECT pped_codigo FROM mpedidotransferencia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+")");
			string prefijoFacturaTransferencia = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo=(SELECT pdoc_factura FROM mordentransferencia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+")");

            //Lo primero que hay que hace es berificar que el vendedor sea válido, y que se pueda vincular como el recepcionista
            DropDownList recepcion = ((DropDownList)controlDatosOrden.FindControl("codigoRecep"));
            bind.PutDatasIntoDropDownList(recepcion, "SELECT pv.pven_codigo, \n" +
                                                         "       pv.pven_nombre \n" +
                                                         "FROM pvendedor pv \n" +
                                                         "INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo \n" +
                                                         "WHERE (pv.tvend_codigo = 'RT' OR pv.tvend_codigo = 'TT') \n" +
                                                         "AND   pv.pven_vigencia = 'V' \n" +
                                                         "AND   pva.palm_almacen = '" + almacenVend + "' \n" +
                                                         "ORDER BY pven_nombre ASC");
            try
            {
                recepcion.SelectedValue = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_codigo='" + DBFunctions.SingleData("SELECT pven_codigo FROM morden WHERE pdoc_codigo='" + prefijo + "' AND mord_numeorde=" + numeroOrden + "") + "'").ToString();
                //Utils.FillDll(((DropDownList)controlDatosOrden.FindControl("codigoRecep")), @"SELECT pv.pven_codigo,      
                //                                pv.pven_nombre      
                //                         FROM pvendedor pv      
                //                         INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo      
                //                         WHERE pv.tvend_codigo IN ( 'RT', 'RA', 'TT')      
                //                         AND   pv.pven_vigencia = 'V'      
                //                         AND   pva.palm_almacen = '" + almacenVend + @"'      
                //                         ORDER BY pven_nombre ASC", true);
                //((DropDownList)controlDatosOrden.FindControl("codigoRecep")).DataSource = recepcion.DataSource;
                Utils.MostrarAlerta(Response, "No olvide escribir la clave del recepcionista en la pestaña de Datos Orden.");
            }
            catch (Exception z)
            {
                Utils.MostrarAlerta(Response, "Existe un problema con el estado del vendedor \n por favor revise que sea un recepcionista de Taller o Todo Tipo \n y que exista en el almacen de la órden.");
                ((Button)principal.FindControl("grabar")).Enabled = false;
            }

            //Establecemos los TextBox//
            ((TextBox)controlDatosOrden.FindControl("numOrden")).Text = numeroOrden;
			((TextBox)controlDatosOrden.FindControl("placa")).Text = placa;
			((TextBox)controlDatosOrden.FindControl("entrada")).Text = entrada;
			((TextBox)controlDatosOrden.FindControl("locker")).Text = DBFunctions.SingleData("SELECT mord_locker FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			DateTime fechaOrden = System.Convert.ToDateTime(fechaEntrada);
			((TextBox)controlDatosOrden.FindControl("fecha")).Text = fechaOrden.ToString("yyyy-MM-dd");
			string hora = DBFunctions.SingleData("SELECT mord_horaentr FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");

            if (hora.IndexOf(":") >= 0)
            {
                ((TextBox)controlDatosOrden.FindControl("hora")).Text = hora.Substring(0, hora.IndexOf(":"));
                ((TextBox)controlDatosOrden.FindControl("minutos")).Text = hora.Substring(hora.IndexOf(":") + 1, 2);
            }
			//Falta bloquear el TextBox de la clave
			((TextBox)controlDatosOrden.FindControl("obsrCliente")).Text = DBFunctions.SingleData("SELECT mord_obseclie FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			((TextBox)controlDatosOrden.FindControl("obsrRecep")).Text = DBFunctions.SingleData("SELECT mord_obserece FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			((Label)controlDatosOrden.FindControl("lbEstCita")).Text = DBFunctions.SingleData("SELECT testcit_estacita FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			
            //Establecemos los DropDownList//
            DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("almacen")), DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + DBFunctions.SingleData("SELECT palm_almacen FROM morden WHERE pdoc_codigo='" + prefijo + "' AND mord_numeorde=" + numeroOrden + "") + "'"));
            Session["modificandoOT"] = "1";
            //DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("tipoDocumento")),DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'"));
            //((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).Items.Add(DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'"));
            bind.PutDatasIntoDropDownList(((DropDownList)controlDatosOrden.FindControl("tipoDocumento")), "SELECT pdoc_codigo, pdoc_codigo concat ' - ' concat pdoc_nombre FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'");
			DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("cargo")),DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='"+DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"")+"'"));			
			DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("servicio")),DBFunctions.SingleData("SELECT ttra_nombre FROM ttrabajoorden WHERE ttra_codigo='"+DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"")+"'"));
			DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("listaPrecios")),DBFunctions.SingleData("SELECT ppreta_nombre FROM ppreciotaller WHERE ppreta_codigo='"+DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"")+"'"));
			//DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("codigoRecep")),DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+DBFunctions.SingleData("SELECT pven_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"")+"'"));

            
            //bind.PutDatasIntoDropDownList(recepcion, "SELECT pv.pven_codigo, \n" +
            //                                             "       pv.pven_nombre \n" +
            //                                             "FROM pvendedor pv \n" +
            //                                             "INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo \n" +
            //                                             "WHERE (pv.tvend_codigo = 'RT' OR pv.tvend_codigo = 'VM') \n" +
            //                                             "AND   pv.pven_vigencia = 'V' \n" +
            //                                             "AND   pva.palm_almacen = '" + ((DropDownList)controlDatosOrden.FindControl("almacen")).SelectedValue + "' \n" +
            //                                             "ORDER BY pven_nombre ASC");
            
            

            DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("nitTransferencias")),nitTransferencia);
            if (tipoOperacion)
            {
                //pprecioitem????
                //string listaPrecio = DBFunctions.SingleData("SELECT ppreta_codigo FROM morden WHERE pdoc_codigo='" + prefijo + "' AND mord_numeorde=" + numeroOrden + "");
                //bind.PutDatasIntoDropDownList(((DropDownList)controlDatosOrden.FindControl("listaPreciosItems")), "select ppreta_codigo, ppreta_nombre from ppreciotaller where ppreta_codigo= '" + listaPrecio + "'");
            }
            // DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("tipoTransferencia")),tipoTransferencia);
			DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosOrden.FindControl("prefijoTransferencia")),prefijoFacturaTransferencia);

            DropDownList nitTransfer = ((DropDownList)controlDatosOrden.FindControl("nitTransferencias"));
            bind.PutDatasIntoDropDownList(nitTransfer, "SELECT pnital_nittaller FROM pnittaller WHERE palm_almacen='" + ((DropDownList)controlDatosOrden.FindControl("almacen")).SelectedValue + "'");

            ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).Enabled = false;
            ((TextBox)controlDatosOrden.FindControl("numOrden")).Enabled = false;
			//((TextBox)controlDatosOrden.FindControl("placa")).Enabled = !tipoOperacion;
            ((TextBox)controlDatosOrden.FindControl("placa")).Enabled = true;
			((TextBox)controlDatosOrden.FindControl("entrada")).Enabled = tipoOperacion;
			((TextBox)controlDatosOrden.FindControl("locker")).Enabled = tipoOperacion;
			((TextBox)controlDatosOrden.FindControl("fecha")).Enabled = tipoOperacion;
			((TextBox)controlDatosOrden.FindControl("hora")).Enabled = tipoOperacion;
			((TextBox)controlDatosOrden.FindControl("minutos")).Enabled = tipoOperacion;
			((TextBox)controlDatosOrden.FindControl("obsrCliente")).Enabled = true;
            //mientras alejo
            ((TextBox)controlDatosOrden.FindControl("obsrRecep")).Enabled = true;
			((TextBox)controlDatosOrden.FindControl("clave")).Enabled = tipoOperacion;
			((DropDownList)controlDatosOrden.FindControl("almacen")).Enabled = !tipoOperacion;
            ((DropDownList)controlDatosOrden.FindControl("cargo")).Enabled = !tipoOperacion;
			((DropDownList)controlDatosOrden.FindControl("servicio")).Enabled = tipoOperacion;
			((DropDownList)controlDatosOrden.FindControl("listaPrecios")).Enabled = tipoOperacion;
			((DropDownList)controlDatosOrden.FindControl("codigoRecep")).Enabled = tipoOperacion;

            ((DropDownList)controlDatosOrden.FindControl("listaPreciosItems")).Enabled = tipoOperacion;
			((DropDownList)controlDatosOrden.FindControl("nitTransferencias")).Enabled = !tipoOperacion;
            ((DropDownList)controlDatosOrden.FindControl("tipoPedido")).Enabled = !tipoOperacion;
			//((DropDownList)controlDatosOrden.FindControl("tipoTransferencia")).Enabled = tipoOperacion;
			((DropDownList)controlDatosOrden.FindControl("prefijoTransferencia")).Enabled = tipoOperacion;
            ((DropDownList)controlDatosOrden.FindControl("almacenTransferencia")).Enabled = tipoOperacion;

            //((Button)controlDatosOrden.FindControl("confirmar")).Enabled = tipoOperacion;
            HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
            hdTabIndex.Value = "0";
            return ((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedValue.Trim();
		}
		
		protected void Distribuir_Datos_Propietario(bool tipoOperacion, Control controlDatosPropietario, string prefijo, string numeroOrden, string nit)
		{
			DropDownList ddlCiudad=((DropDownList)controlDatosPropietario.FindControl("datosd"));
			//Establecemos los TextBox
			((TextBox)controlDatosPropietario.FindControl("datos")).Text = nit;
			((TextBox)controlDatosPropietario.FindControl("datosa")).Text = DBFunctions.SingleData("SELECT mnit_nombres FROM mnit WHERE mnit_nit='"+nit+"'");
			((TextBox)controlDatosPropietario.FindControl("datosb")).Text = DBFunctions.SingleData("SELECT mnit_apellidos FROM mnit WHERE mnit_nit='"+nit+"'");
			((TextBox)controlDatosPropietario.FindControl("datosc")).Text = DBFunctions.SingleData("SELECT mnit_direccion FROM mnit WHERE mnit_nit='"+nit+"'");
			//((TextBox)controlDatosPropietario.FindControl("datosd")).Text = DBFunctions.SingleData("SELECT pciu_codigo FROM mnit WHERE mnit_nit='"+nit+"'");
			ddlCiudad.SelectedIndex=ddlCiudad.Items.IndexOf(ddlCiudad.Items.FindByValue(DBFunctions.SingleData("SELECT pciu_codigo FROM mnit WHERE mnit_nit='"+nit+"'")));
			((TextBox)controlDatosPropietario.FindControl("datose")).Text = DBFunctions.SingleData("SELECT mnit_telefono FROM mnit WHERE mnit_nit='"+nit+"'");
			((TextBox)controlDatosPropietario.FindControl("datosf")).Text = DBFunctions.SingleData("SELECT mnit_celular FROM mnit WHERE mnit_nit='"+nit+"'");
			((TextBox)controlDatosPropietario.FindControl("datosg")).Text = DBFunctions.SingleData("SELECT mnit_email FROM mnit WHERE mnit_nit='"+nit+"'");
			((TextBox)controlDatosPropietario.FindControl("datosh")).Text = DBFunctions.SingleData("SELECT mnit_web FROM mnit WHERE mnit_nit='"+nit+"'");
			//Establcemos los otros controles
			DatasToControls.EstablecerDefectoRadioButtonList(((RadioButtonList)controlDatosPropietario.FindControl("tipoCliente")),DBFunctions.SingleData("SELECT tpro_nombre FROM tpropietariotaller WHERE tpro_codigo='"+DBFunctions.SingleData("SELECT tpro_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"")+"'"));						
			DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosPropietario.FindControl("tipoPago")),DBFunctions.SingleData("SELECT ttip_nombre FROM ttipopago WHERE ttip_codigo='"+DBFunctions.SingleData("SELECT ttip_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"")+"'"));
			((TextBox)controlDatosPropietario.FindControl("datosc")).Enabled = tipoOperacion;
			//((TextBox)controlDatosPropietario.FindControl("datosd")).Enabled = tipoOperacion;
			ddlCiudad.Enabled = tipoOperacion;
			((TextBox)controlDatosPropietario.FindControl("datose")).Enabled = tipoOperacion;
			((TextBox)controlDatosPropietario.FindControl("datosf")).Enabled = tipoOperacion;
			((TextBox)controlDatosPropietario.FindControl("datosg")).Enabled = tipoOperacion;
			((TextBox)controlDatosPropietario.FindControl("datosh")).Enabled = tipoOperacion;
			((RadioButtonList)controlDatosPropietario.FindControl("tipoCliente")).Enabled = tipoOperacion;
			((DropDownList)controlDatosPropietario.FindControl("tipoPago")).Enabled = tipoOperacion;
			//((Button)controlDatosPropietario.FindControl("confirmar")).Enabled = tipoOperacion;
		}
		
		protected void Distribuir_Datos_Vehiculo(bool tipoOperacion, Control controlDatosVehiculo, string prefijo, string numeroOrden, string placa)
		{
			//Establecemos los TextBox
			((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text = DBFunctions.SingleData("SELECT mcat_vin FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			((TextBox)controlDatosVehiculo.FindControl("motor")).Text = DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			((TextBox)controlDatosVehiculo.FindControl("serie")).Text = DBFunctions.SingleData("SELECT mcat_serie FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			((TextBox)controlDatosVehiculo.FindControl("anoModelo")).Text = DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			((TextBox)controlDatosVehiculo.FindControl("kilometraje")).Text = DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			((TextBox)controlDatosVehiculo.FindControl("consVendedor")).Text = DBFunctions.SingleData("SELECT mcat_concvend FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			DateTime fechaCompra = System.Convert.ToDateTime((DBFunctions.SingleData("SELECT mcat_venta FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")).Substring(0,(DBFunctions.SingleData("SELECT mcat_venta FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'")).IndexOf(" ")));
			((TextBox)controlDatosVehiculo.FindControl("fechaCompra")).Text = fechaCompra.ToString("yyyy-MM-dd");
			((TextBox)controlDatosVehiculo.FindControl("codRadio")).Text = DBFunctions.SingleData("SELECT mcat_numeradio FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			((TextBox)controlDatosVehiculo.FindControl("kilometrajeCompra")).Text = DBFunctions.SingleData("SELECT mcat_numekilovent FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'");
			//Establecemos los DropDownList
			DropDownList modelo=(DropDownList)controlDatosVehiculo.FindControl("modelo");
			DropDownList color=(DropDownList)controlDatosVehiculo.FindControl("color");
			DropDownList tipo=(DropDownList)controlDatosVehiculo.FindControl("tipo");
			modelo.SelectedIndex=modelo.Items.IndexOf(modelo.Items.FindByValue(DBFunctions.SingleData("SELECT pcat_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'").Trim()));
			color.SelectedIndex=color.Items.IndexOf(color.Items.FindByValue(DBFunctions.SingleData("SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'").Trim()));
			tipo.SelectedIndex=tipo.Items.IndexOf(tipo.Items.FindByValue(DBFunctions.SingleData("SELECT tser_tiposerv FROM mcatalogovehiculo WHERE mcat_placa='"+placa+"'").Trim()));

			//Miramos si la orden de trabajo tiene asociado un cargo por garantia o por seguros
			if(DBFunctions.RecordExist("SELECT * FROM dordengarantia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+""))
			{
				((TextBox)controlDatosVehiculo.FindControl("nitCompania")).Text = DBFunctions.SingleData("SELECT mnit_nitfabrica FROM dordengarantia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionGarant")).Text = DBFunctions.SingleData("SELECT mord_autorizacion FROM dordengarantia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("nitCompania")).Enabled = tipoOperacion;
				((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionGarant")).Enabled = tipoOperacion;
			}
			else if(DBFunctions.RecordExist("SELECT * FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+""))
			{
				((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Text = DBFunctions.SingleData("SELECT mnit_nitseguros FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("siniestro")).Text = DBFunctions.SingleData("SELECT mord_siniestro FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Text = DBFunctions.SingleData("SELECT mord_porcdeducible FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Text = DBFunctions.SingleData("SELECT mord_deduminimo FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Text = DBFunctions.SingleData("SELECT mord_autorizacion FROM dordenseguros WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Enabled = tipoOperacion;
				((TextBox)controlDatosVehiculo.FindControl("siniestro")).Enabled = tipoOperacion;
				((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Enabled = tipoOperacion;
				((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Enabled = tipoOperacion;
				((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Enabled = tipoOperacion;
			}
			((TextBox)controlDatosVehiculo.FindControl("kilometraje")).Enabled = tipoOperacion;
			((TextBox)controlDatosVehiculo.FindControl("codRadio")).Enabled = tipoOperacion;
			//((Button)controlDatosVehiculo.FindControl("confirmar")).Enabled = tipoOperacion;
            //Cargar documentos
            DataSet dsDocs=new DataSet();
            string sql = @"SELECT MC.PDOC_CODIGO PDOC_CODIDOCU, PD.PDOC_NOMBRE PDOC_NOMBRE, 
                        MC.MVEH_NUMEDOCU MVEH_NUMEDOCU,MC.MVEH_FECHENTREGA MVEH_FECHENTREGA  
                        FROM PDOCUMENTOVEHICULO PD, MVEHICULODOCUMENTO MC 
                        WHERE PD.PDOC_CODIGO=MC.PDOC_CODIGO AND MC.MCAT_VIN='" + ((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text + "';";
            DBFunctions.Request(dsDocs, IncludeSchema.NO,sql);

            if (dsDocs.Tables.Count > 0)
                ((DataGrid)controlDatosVehiculo.FindControl("dgrDocumentos")).DataSource = dsDocs.Tables[0];

            ((DataGrid)controlDatosVehiculo.FindControl("dgrDocumentos")).DataBind();
		}
		
		protected void Distribuir_Datos_Accesorios(bool tipoOperacion, Control controlDatosAccesorios, string prefijo, string numeroOrden)
		{
			int i,j;
			//Establecemos los TextBox
			((TextBox)controlDatosAccesorios.FindControl("tp")).Text = DBFunctions.SingleData("SELECT mord_tp FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			((TextBox)controlDatosAccesorios.FindControl("tpp")).Text = DBFunctions.SingleData("SELECT mord_tpp FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			((TextBox)controlDatosAccesorios.FindControl("tp")).Enabled = tipoOperacion;
			((TextBox)controlDatosAccesorios.FindControl("tpp")).Enabled = tipoOperacion;
			DatasToControls.EstablecerDefectoRadioButtonList(((RadioButtonList)controlDatosAccesorios.FindControl("nivelCombustible")),DBFunctions.SingleData("SELECT tnivcomb_descripcion FROM tnivelcombustible WHERE tnivcomb_codigo=(SELECT tnivcomb_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+")"));
			((RadioButtonList)controlDatosAccesorios.FindControl("nivelCombustible")).Enabled = tipoOperacion;
			//Ahora llenamos la Grilla
			//Cargamos todos los accesorios que tiene el auto registrado en la orden de trabajo
			DataSet accesoriosOrden = new DataSet();
			DataTable tablaAccesorios = new DataTable();
			tablaAccesorios = (DataTable)Session["tablaAccesorios"];
			DBFunctions.Request(accesoriosOrden,IncludeSchema.NO,"SELECT pacc_codigo, mord_estado FROM dordenaccesorio WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			for(i=0;i<accesoriosOrden.Tables[0].Rows.Count;i++)
			{
				for(j=0;j<tablaAccesorios.Rows.Count;j++)
				{
					if((accesoriosOrden.Tables[0].Rows[i][0].ToString())==(DBFunctions.SingleData("SELECT pacc_codigo FROM paccesorio WHERE pacc_descripcion='"+tablaAccesorios.Rows[j][0].ToString().Trim()+"'")))
					{
						//me pone en la grilla el valor del estado
						((TextBox)((DataGrid)controlDatosAccesorios.FindControl("accesorios")).Items[j].Cells[1].Controls[1]).Text = accesoriosOrden.Tables[0].Rows[i][1].ToString();
						((TextBox)((DataGrid)controlDatosAccesorios.FindControl("accesorios")).Items[j].Cells[1].Controls[1]).Enabled = tipoOperacion;
						//((Button)controlDatosAccesorios.FindControl("confirmar")).Enabled = tipoOperacion;
					}
				}
			}
		}
		
		protected void Distribuir_Datos_KitsCombos(bool tipoOperacion, Control controlDatosKitsCombos, string prefijo, string numeroOrden, string grupoCatalogo, string cargo)
		{
			int i;
			double totalOrden = 0;
			//Cargamos los Kits Disponibles para este carro
			DataTable kitsDisponibles = new DataTable();
			kitsDisponibles.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			kitsDisponibles.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
			DataSet kitsGrupoCatalogo = new DataSet();
            DBFunctions.Request(kitsGrupoCatalogo, IncludeSchema.NO, "SELECT pkit_codigo, pkit_nombre FROM pkit WHERE PKIT_VIGENCIA = 'V' and pgru_grupo='" + grupoCatalogo + "'");
			for(i=0;i<kitsGrupoCatalogo.Tables[0].Rows.Count;i++)
			{
				DataRow fila;
				fila = kitsDisponibles.NewRow();
				fila["CODIGO"] = kitsGrupoCatalogo.Tables[0].Rows[i].ItemArray[0].ToString();
				fila["DESCRIPCION"] = kitsGrupoCatalogo.Tables[0].Rows[i].ItemArray[1].ToString();
				kitsDisponibles.Rows.Add(fila);
			}
			((DataGrid)controlDatosKitsCombos.FindControl("kitsCompletos")).DataSource = kitsDisponibles;
			((DataGrid)controlDatosKitsCombos.FindControl("kitsCompletos")).DataBind();
			Session["kitsTabla"] = kitsDisponibles;
			//Desactivamos los botones de distribuir los kits en las grillas
			for(i=0;i<((DataGrid)controlDatosKitsCombos.FindControl("kitsCompletos")).Items.Count;i++)
				((Button)((DataGrid)controlDatosKitsCombos.FindControl("kitsCompletos")).Items[i].Cells[2].Controls[1]).Enabled = true;
				//((Button)((DataGrid)controlDatosKitsCombos.FindControl("kitsCompletos")).Items[i].Cells[3].Controls[1]).Enabled = false;
			//Fin de carga de Kits
			//Establecemos los TextBox
            try
            {
                DateTime fechaEstimada = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entregar FROM morden WHERE pdoc_codigo='" + prefijo + "' AND mord_numeorde=" + numeroOrden + ""));
                ((TextBox)controlDatosKitsCombos.FindControl("fechaEstimada")).Text = fechaEstimada.ToString("yyyy-MM-dd");
            }
            catch { }

			((TextBox)controlDatosKitsCombos.FindControl("horaEstimada")).Text = DBFunctions.SingleData("SELECT mord_horaentg FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
			//Ahora cargamos las operaciones de la orden
			//anotacion hecha en 29-10-2004
			this.Preparar_Tabla_Operaciones();
			DataSet operacionesOrden = new DataSet();
            string opersOrden = "SELECT DOR.ptem_operacion, dord_valooper as VALOROPERACION, DOR.pven_codigo, DOR.tcar_cargo, DOR.test_estado, pgar_codigo1, pgar_codigo2, pgar_codigo3, pgar_codigo4, " + 
	                            " ptem_descripcion, ttra_codigo,ttip_codiliqu, ptem_exceiva AS EXCENTOIVA, COALESCE(pven_nombre,' SIN ASIGNAR '), tcar_nombre, test_nombre, "+
	                            " COALESCE (PG1.pgar_descripcion, '') AS CODIGOINCIDENTE, "+
	                            " COALESCE (PG2.pgar_descripcion, '') AS CODIGOGARANTIA, "+
	                            " COALESCE (PG3.pgar_descripcion, '') AS CODIGOREMEDIO, "+
	                            " COALESCE (PG4.pgar_descripcion, '') AS CODIGODEFECTO "+
                                " FROM  ptempario PT, morden MO, tcargoorden TC, testadooperacion TEO, dordenoperacion DOR "+
                                " LEFT JOIN pvendedor PV ON DOR.pven_codigo = PV.PVEN_CODIGO "+
                                " LEFT JOIN pgarantia PG1 ON DOR.pgar_codigo1 = PG1.pgar_codigo AND DOR.pgar_codigo1 IS NOT NULL "+
                                " LEFT JOIN pgarantia PG2 ON DOR.pgar_codigo2 = PG2.pgar_codigo AND DOR.pgar_codigo2 IS NOT NULL "+
                                " LEFT JOIN pgarantia PG3 ON DOR.pgar_codigo3 = PG3.pgar_codigo AND DOR.pgar_codigo3 IS NOT NULL "+
                                " LEFT JOIN pgarantia PG4 ON DOR.pgar_codigo4 = PG4.pgar_codigo AND DOR.pgar_codigo4 IS NOT NULL "+
                                "WHERE DOR.pdoc_codigo='"+prefijo+"' AND DOR.mord_numeorde="+numeroOrden+" AND DOR.ptem_operacion = PT.ptem_operacion "+
                                " AND DOR.pdoc_codigo= MO.pdoc_codigo AND DOR.mord_numeorde = MO.mord_numeorde "+
                                " AND DOR.tcar_cargo = TC.tcar_cargo  AND DOR.test_estaDO = TEO.TEST_ESTAOPER;";
		//	DBFunctions.Request(operacionesOrden,IncludeSchema.NO,"SELECT ptem_operacion, dord_valooper, pven_codigo, tcar_cargo, test_estado, pgar_codigo1, pgar_codigo2, pgar_codigo3, pgar_codigo4 FROM dordenoperacion WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
            DBFunctions.Request(operacionesOrden, IncludeSchema.NO, opersOrden);
            for (i = 0; i < operacionesOrden.Tables[0].Rows.Count; i++)
			{
				DataRow fila = operacionesOrdenesGrilla.NewRow();
				fila["CODIGOKIT"] = "";
				fila["CODIGOOPERACION"] = operacionesOrden.Tables[0].Rows[i][0].ToString();
			//	fila["NOMBRE"] = DBFunctions.SingleData("SELECT ptem_descripcion FROM ptempario WHERE ptem_operacion='"+operacionesOrden.Tables[0].Rows[i][0].ToString()+"'");
                fila["NOMBRE"] = operacionesOrden.Tables[0].Rows[i][09].ToString();
                //Cambio Hecho para la orden de peritaje, problema del tiempo estimado
				//Cambio hecho 2004-12-15
			//	if((DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+""))=="P")
                if (operacionesOrden.Tables[0].Rows[i][10].ToString() == "P")
					fila["TIEMPOEST"] = 0;
				else
				{
					string tiempoA=DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_tempario='"+operacionesOrden.Tables[0].Rows[i][0].ToString()+"'").Trim();
					if   (tiempoA.Length>0) 
                         fila["TIEMPOEST"] = System.Convert.ToDouble(tiempoA);
					else fila["TIEMPOEST"] = 0;
				}
				//Fin de Cambio
				//verificar si es por bono la operacion
			//	string tipoLiquidacion = DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+operacionesOrden.Tables[0].Rows[i][0].ToString()+"'");
                string tipoLiquidacion = operacionesOrden.Tables[0].Rows[i][11].ToString();
                if (tipoOperacion || tipoLiquidacion != "B")
				{
                    fila["VALOROPERACION"] = System.Convert.ToDouble(operacionesOrden.Tables[0].Rows[i][1].ToString());
					totalOrden += System.Convert.ToDouble(operacionesOrden.Tables[0].Rows[i][1].ToString());
				}					
			//	string excepcionIva = DBFunctions.SingleData("SELECT ptem_exceiva FROM ptempario WHERE ptem_operacion='"+operacionesOrden.Tables[0].Rows[i][0].ToString()+"'");
                string excepcionIva = operacionesOrden.Tables[0].Rows[i][12].ToString();
                if (excepcionIva == "S")
					fila["EXCENTOIVA"] = "SI";
				else if(excepcionIva=="N")
					fila["EXCENTOIVA"] = "NO";
			//	fila["MECANICO"] = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+operacionesOrden.Tables[0].Rows[i][2].ToString()+"'");
                fila["MECANICO"] = operacionesOrden.Tables[0].Rows[i][13].ToString();
            //  fila["CARGO"] = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + operacionesOrden.Tables[0].Rows[i][3].ToString() + "'");
                fila["CARGO"] = operacionesOrden.Tables[0].Rows[i][14].ToString();
            //  fila["ESTADO"] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estadooperacion='" + operacionesOrden.Tables[0].Rows[i][4].ToString() + "'");
                fila["ESTADO"] = operacionesOrden.Tables[0].Rows[i][15].ToString();
                if ((operacionesOrden.Tables[0].Rows[i][5].ToString()) == "0")
					fila["CODIGOINCIDENTE"] = "";
				else
			//		fila["CODIGOINCIDENTE"] = DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operacionesOrden.Tables[0].Rows[i][5]+"'");
                    fila["CODIGOINCIDENTE"] = operacionesOrden.Tables[0].Rows[i][16].ToString();
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if((operacionesOrden.Tables[0].Rows[i][6].ToString())=="0")
					fila["CODIGOGARANTIA"] = "";
				else
			//		fila["CODIGOGARANTIA"] = DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operacionesOrden.Tables[0].Rows[i][6]+"'");
                    fila["CODIGOGARANTIA"] = operacionesOrden.Tables[0].Rows[i][17].ToString();
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if((operacionesOrden.Tables[0].Rows[i][7].ToString())=="0")
					fila["CODIGOREMEDIO"] = "";
				else
			//		fila["CODIGOREMEDIO"] = DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operacionesOrden.Tables[0].Rows[i][7]+"'");
                    fila["CODIGOREMEDIO"] = operacionesOrden.Tables[0].Rows[i][18].ToString();
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				if((operacionesOrden.Tables[0].Rows[i][8].ToString())=="0")
					fila["CODIGODEFECTO"] = "";
				else
			//		fila["CODIGODEFECTO"] = DBFunctions.SingleData("SELECT pgar_descripcion FROM pgarantia WHERE pgar_codigo='"+operacionesOrden.Tables[0].Rows[i][8]+"'");
                    fila["CODIGODEFECTO"] = operacionesOrden.Tables[0].Rows[i][19].ToString();
                operacionesOrdenesGrilla.Rows.Add(fila);
			}
			//cargamos la tabla en la grilla
			((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).DataSource = operacionesOrdenesGrilla;
			((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).DataBind();
			if(cargo == "Gtía Fabrica")
				this.Grilla_Operaciones(((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")),operacionesOrdenesGrilla);
			Session["operaciones"] = operacionesOrdenesGrilla;
			//cargamos el total de las operaciones que nos sean por bono
			//Ahora realizamos la comprobacion dentro de la grilla de operaciones
			int cantidadItems = ((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items.Count;
			for(i=0;i<cantidadItems;i++)
			{
				if((DBFunctions.SingleData("SELECT ttip_codiliqu FROM ptempario WHERE ptem_operacion='"+operacionesOrdenesGrilla.Rows[i][1].ToString()+"'"))=="B")
				{
                    //int asd = ((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[0].Cells[3].Controls.Count;
					((TextBox)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[5].Controls[1]).Visible = true;
					((TextBox)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[5].Controls[1]).Text = System.Convert.ToDouble(DBFunctions.SingleData("SELECT dord_valooper FROM dordenoperacion WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+" AND ptem_operacion='"+operacionesOrdenesGrilla.Rows[i][1].ToString()+"'").Trim()).ToString("n");
					if(tipoOperacion)
						((TextBox)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[5].Controls[1]).Enabled = false;
					else
						((TextBox)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[5].Controls[1]).Enabled = true;
				}
                //se permite editar para lograr la modificacion
				((Button)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[14].Controls[1]).Enabled = tipoOperacion;
				//((Button)controlDatosKitsCombos.FindControl("confirmar")).Enabled = tipoOperacion;
				//Cambio hecho para las garantias por fabrica
				if(operacionesOrdenesGrilla.Rows[i][9].ToString()!="")
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[10].Controls[1]),operacionesOrdenesGrilla.Rows[i][9].ToString());
				if(operacionesOrdenesGrilla.Rows[i][10].ToString()!="")
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[11].Controls[1]),operacionesOrdenesGrilla.Rows[i][10].ToString());
				if(operacionesOrdenesGrilla.Rows[i][11].ToString()!="")
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[12].Controls[1]),operacionesOrdenesGrilla.Rows[i][11].ToString());
				if(operacionesOrdenesGrilla.Rows[i][12].ToString()!="")
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Items[i].Cells[13].Controls[1]),operacionesOrdenesGrilla.Rows[i][12].ToString());
			}
            //Se cambian a enabled=true para permitir realizar modificacion.
			((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).ShowFooter = !tipoOperacion;
			((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Enabled = !tipoOperacion;
			((DataGrid)controlDatosKitsCombos.FindControl("kitsItems")).ShowFooter = !tipoOperacion;

            ((DataGrid)controlDatosKitsCombos.FindControl("kitsCompletos")).Enabled = !tipoOperacion;
            ((DataGrid)controlDatosKitsCombos.FindControl("kitsItems")).Enabled = !tipoOperacion;
            ((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")).Enabled = !tipoOperacion;

			this.Ddls_GrillaOperaciones(((DataGrid)controlDatosKitsCombos.FindControl("kitsOperaciones")),tipoOperacion);
			if((DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+""))!="P")
			{
				//Ahora vamos a cargar los repuestos que se encuentran asignados para la orden de trabajo
				//Anotacion hecha en 2005-01-17
				this.Preparar_Tabla_Repuestos();
				//string prefijoPedido = DBFunctions.SingleData("SELECT pped_codigo FROM mpedidotransferencia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				//string numeroPedido = DBFunctions.SingleData("SELECT mped_numero FROM mpedidotransferencia WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");			
				DataSet repuestosOrden = new DataSet();
				//DBFunctions.Request(repuestosOrden,IncludeSchema.NO,"SELECT mite_codigo, dped_cantpedi, dped_cantasig, dped_cantfact, dped_valounit, dped_porcdesc, piva_porciva FROM dpedidoitem WHERE pped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
				DBFunctions.Request(repuestosOrden,IncludeSchema.NO,
                    @"SELECT DPED.mite_codigo, DPED.dped_cantpedi, DPED.dped_cantasig, DPED.dped_cantfact, DPED.dped_valounit, DPED.dped_porcdesc, DPED.piva_porciva, MORD.tcar_cargo,
		                    mi.mite_nombre, TOI.tori_nombre, MI.pdes_codigo, TC.tcar_nombre, MI.PLIN_CODIGO
	                FROM dpedidoitem DPED, mpedidoitem MPED, mpedidotransferencia MORD, mitems MI, torigenitem TOI, tcargoorden TC
	                WHERE MORD.pdoc_codigo='"+prefijo+"@' AND MORD.mord_numeorde="+numeroOrden+@" AND MORD.pped_codigo=MPED.pped_codigo AND MORD.mped_numero=MPED.mped_numepedi 
	                AND MPED.mped_claseregi=DPED.mped_clasregi AND MPED.mnit_nit=DPED.mnit_nit AND MPED.pped_codigo=DPED.pped_codigo AND MPED.mped_numepedi=DPED.mped_numepedi
	                AND DPED.MITE_CODIGO = MI.MITE_CODIGO AND MI.TORI_CODIGO = TOI.TORI_CODIGO AND TC.tcar_cargo = MORD.TCAR_CARGO;");
				for(i=0;i< repuestosOrden.Tables[0].Rows.Count;i++)
				{
					DataRow fila = repuestosOrdenesGrilla.NewRow();
					fila["CODIGOKIT"] = "ninguno";
					fila["CODIGOREPUESTO"] = repuestosOrden.Tables[0].Rows[i][0].ToString();
					//fila["REFERENCIA"] = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+repuestosOrden.Tables[0].Rows[i][0].ToString()+"'");
                    fila["REFERENCIA"] = repuestosOrden.Tables[0].Rows[i][8].ToString();
                    //fila["ORIGEN"] = DBFunctions.SingleData("SELECT tori_nombre FROM torigenitem WHERE tori_codigo=(SELECT tori_codigo FROM mitems WHERE mite_codigo='" + repuestosOrden.Tables[0].Rows[i][0].ToString() + "')");
                    fila["ORIGEN"] = repuestosOrden.Tables[0].Rows[i][9].ToString();
                    fila["PRECIO"] = System.Convert.ToDouble(repuestosOrden.Tables[0].Rows[i][4].ToString());
					fila["CANTIDAD"] = System.Convert.ToDouble(repuestosOrden.Tables[0].Rows[i][3].ToString());
					//fila["CODIGO"] = DBFunctions.SingleData("SELECT pdes_codigo FROM mitems WHERE mite_codigo='"+repuestosOrden.Tables[0].Rows[i][0].ToString()+"'");
                    fila["CODIGO"] = repuestosOrden.Tables[0].Rows[i][10].ToString();
                    fila["IVA"] = System.Convert.ToDouble(repuestosOrden.Tables[0].Rows[i][6].ToString());
					fila["VALORFACTURADO"] = (((System.Convert.ToDouble(fila["PRECIO"]))+((System.Convert.ToDouble(fila["PRECIO"]))*(System.Convert.ToDouble(fila["IVA"])/100)))*(System.Convert.ToDouble(fila["CANTIDAD"])));
					totalOrden += (((System.Convert.ToDouble(fila["PRECIO"]))+((System.Convert.ToDouble(fila["PRECIO"]))*(System.Convert.ToDouble(fila["IVA"])/100)))*(System.Convert.ToDouble(fila["CANTIDAD"])));
					//fila["CARGO"] = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='"+repuestosOrden.Tables[0].Rows[i][7].ToString()+"'");
                    fila["CARGO"] = repuestosOrden.Tables[0].Rows[i][11].ToString();
                    //fila["LINEABODEGA"] = DBFunctions.SingleData("SELECT plin_codigo FROM mitems WHERE mite_codigo='"+repuestosOrden.Tables[0].Rows[i][0].ToString()+"'");
                    fila["LINEABODEGA"] = repuestosOrden.Tables[0].Rows[i][12].ToString();
                    repuestosOrdenesGrilla.Rows.Add(fila);
				}
				((DataGrid)controlDatosKitsCombos.FindControl("kitsItems")).DataSource = repuestosOrdenesGrilla;
				((DataGrid)controlDatosKitsCombos.FindControl("kitsItems")).DataBind();
				Session["items"] = repuestosOrdenesGrilla;
				((DataGrid)controlDatosKitsCombos.FindControl("kitsItems")).Enabled = false;
				((DataGrid)controlDatosKitsCombos.FindControl("kitsItems")).ShowFooter = false;
				//Fin carga de repuestos
			}
			((TextBox)controlDatosKitsCombos.FindControl("total")).Text = totalOrden.ToString("C");
		}
		
		
		//Con esta funcion distribuimos los datos de la orden de trabajo de peritaje dentro de las grillas de peritaje
		//retornamos verdadero si es de peritaje y falso si no
		protected bool Distribuir_Datos_Peritaje(Control controlDatosPeritaje, string prefijo, string numeroOrden)
		{
			int i,j,k;
			bool peritaje = false;
			if((DBFunctions.SingleData("SELECT ttra_codigo FROM morden WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+""))=="P")
			{
				//Primero traemos los datos que se construyen de forma inherente en el control del peritaje
				//Ademas al traemos el placeholder que contiene las grillas
				ArrayList codigosGruposPeritaje = new ArrayList();
				codigosGruposPeritaje = ((ArrayList)Session["codigosGruposPeritaje"]);
				int cantidadGrillas = System.Convert.ToInt32(Session["cantidadGrillas"]);
				DataTable []tablasAsociadas = new DataTable[cantidadGrillas];
				for(i=0;i<cantidadGrillas;i++)
				{
					tablasAsociadas[i] = ((DataTable[])Session["tablasAsociadas"])[i].Copy();
				}
				PlaceHolder gruposPeritaje = new PlaceHolder();
				gruposPeritaje = ((PlaceHolder)controlDatosPeritaje.FindControl("gruposPeritaje"));
				//Fin de carga///////////////////////////////////////////////////////////////////////////////
				//Ahora Vamos A contruir la Grilla de acuerdo a los datos que hay en la base de datos
				DataSet operacionesOrdenPeritaje = new DataSet();
				DBFunctions.Request(operacionesOrdenPeritaje,IncludeSchema.NO,"SELECT pgrp_codigo, pitp_codigo, tespe_codigo, dorpe_detalle, dorpe_costo FROM dordenperitaje WHERE pdoc_codigo='"+prefijo+"' AND mord_numeorde="+numeroOrden+"");
				for(i=0;i<cantidadGrillas;i++)
				{
					int cantidadOperaciones = tablasAsociadas[i].Rows.Count;
					for(j=0;j<cantidadOperaciones;j++)
					{
						for(k=0;k<operacionesOrdenPeritaje.Tables[0].Rows.Count;k++)
						{
							if(((operacionesOrdenPeritaje.Tables[0].Rows[k][1].ToString())==(DBFunctions.SingleData("SELECT pitp_codigo FROM pitemperitaje WHERE pitp_descripcion='"+tablasAsociadas[i].Rows[j][0].ToString()+"'")))&&((operacionesOrdenPeritaje.Tables[0].Rows[k][0].ToString())==(codigosGruposPeritaje[i].ToString())))
							{
								//Aqui metemos los datos en la grilla
								DatasToControls.EstablecerDefectoRadioButtonList(((RadioButtonList)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[1].Controls[0]), DBFunctions.SingleData("SELECT tespe_descripcion FROM testadoperitaje WHERE tespe_codigo='"+operacionesOrdenPeritaje.Tables[0].Rows[k][2].ToString()+"'"));
								((TextBox)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[2].Controls[0]).Text = operacionesOrdenPeritaje.Tables[0].Rows[k][3].ToString();
								((TextBox)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[3].Controls[0]).Text = operacionesOrdenPeritaje.Tables[0].Rows[k][4].ToString();
							}
						}						
					}
				}
				peritaje = true;
			}
			return peritaje;
		}
		
		////////////////////////////////////////////////////////////////////////////////////////
		//Preparamos la tabla qu vamos a asociar a la grilla
		
		protected void Preparar_Tabla_Ordenes()
		{
			ordenesProceso = new DataTable();
			ordenesProceso.Columns.Add(new DataColumn("PREFIJO",System.Type.GetType("System.String")));
			ordenesProceso.Columns.Add(new DataColumn("NUMEROORDEN",System.Type.GetType("System.String")));
			ordenesProceso.Columns.Add(new DataColumn("NUMEROENTRADA",System.Type.GetType("System.String")));
			ordenesProceso.Columns.Add(new DataColumn("ESTADOORDEN",System.Type.GetType("System.String")));
			ordenesProceso.Columns.Add(new DataColumn("PLACA",System.Type.GetType("System.String")));
			ordenesProceso.Columns.Add(new DataColumn("FECHAENTRADA",System.Type.GetType("System.String")));
		}
		
		protected void 	Distribuir_Ordenes()
		{
            if(Session["ordenesProceso"] != null)
            {
                ordenes.DataSource = (DataTable)Session["ordenesProceso"];
            }
            else
            {
                this.Preparar_Tabla_Ordenes();
                int i;
                DataSet ordenesProcesoConsulta = new DataSet();
                /* 
                  string ordproconsulta = "SELECT MO.pdoc_codigo,mord_numeorde,mord_numeentr,MO.test_estado,MO.mcat_vin,mord_entrada, "+
                                        " MO.pdoc_codigo || ' - ' || pdoc_nombre, test_nombre, COALESCE(mcat_placa,'* ERROR *') " +
                                        " FROM PDOCUMENTO PD, testadoorden TEO,morden MO LEFT JOIN mcatalogovehiculo MCV ON MO.mcat_vin=MCV.MCAT_VIN "+
                                        " WHERE MO.test_estado<>'E' AND MO.test_estado<>'F' AND MO.PDOC_CODIGO = PD.PDOC_CODIGO "+
                                        " AND MO.test_estado = TEO.test_estado 	ORDER BY pdoc_codigo, MORD_NUMEORDE; ";
                */
                string ordproconsulta = @"SELECT MO.pdoc_codigo,mord_numeorde,mord_numeentr,MO.test_estado,MO.mcat_vin,mord_entrada, 
                                    MO.pdoc_codigo || ' - ' || pdoc_nombre, test_nombre, COALESCE(mcat_placa, '* ERROR *') 
                                    FROM PDOCUMENTO PD, testadoorden TEO, morden MO LEFT JOIN mcatalogovehiculo MCV ON MO.mcat_vin = MCV.MCAT_VIN 
                                    WHERE MO.test_estado = 'A' AND MO.PDOC_CODIGO = PD.PDOC_CODIGO 
                                    AND MO.test_estado = TEO.test_estado   ORDER BY pdoc_codigo, MORD_NUMEORDE";
                //   DBFunctions.Request(ordenesProcesoConsulta,IncludeSchema.NO,"SELECT pdoc_codigo,mord_numeorde,mord_numeentr,test_estado,mcat_vin,mord_entrada FROM morden WHERE test_estado<>'E' AND test_estado<>'F' ORDER BY MORD_NUMEORDE");
                DBFunctions.Request(ordenesProcesoConsulta, IncludeSchema.NO, ordproconsulta);
                for (i = 0; i < ordenesProcesoConsulta.Tables[0].Rows.Count; i++)
                {
                    DataRow fila = ordenesProceso.NewRow();
                    //	fila["PREFIJO"] = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+ordenesProcesoConsulta.Tables[0].Rows[i][0].ToString()+"'");
                    fila["PREFIJO"] = ordenesProcesoConsulta.Tables[0].Rows[i][0].ToString();
                    fila["NUMEROORDEN"] = ordenesProcesoConsulta.Tables[0].Rows[i][1].ToString();
                    fila["NUMEROENTRADA"] = ordenesProcesoConsulta.Tables[0].Rows[i][2].ToString();
                    //	fila["ESTADOORDEN"] = DBFunctions.SingleData("SELECT test_nombre FROM testadoorden WHERE test_estado='"+ordenesProcesoConsulta.Tables[0].Rows[i][3].ToString()+"'");
                    fila["ESTADOORDEN"] = ordenesProcesoConsulta.Tables[0].Rows[i][7].ToString();
                    //    fila["PLACA"] = DBFunctions.SingleData("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_vin='" + ordenesProcesoConsulta.Tables[0].Rows[i][4].ToString() + "'");
                    fila["PLACA"] = ordenesProcesoConsulta.Tables[0].Rows[i][8].ToString();
                    fila["FECHAENTRADA"] = (ordenesProcesoConsulta.Tables[0].Rows[i][5].ToString()).Substring(0, (ordenesProcesoConsulta.Tables[0].Rows[i][5].ToString()).IndexOf(" "));
                    ordenesProceso.Rows.Add(fila);
                }
                ordenes.DataSource = ordenesProceso;
                ordenes.DataBind();
                Session["ordenesProceso"] = ordenesProceso;
            }
			
		}
		
		protected void Preparar_Tabla_Operaciones()
		{
			//////////////////////////////////////////////////////////////////////
			///Preparamos la tabla que almacenara la informacion sobre las operaciones
			operacionesOrdenesGrilla = new DataTable();
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CODIGOKIT",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CODIGOOPERACION",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));			
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("TIEMPOEST",System.Type.GetType("System.Double")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("VALOROPERACION",System.Type.GetType("System.Double")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("EXCENTOIVA",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("MECANICO",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CODIGOINCIDENTE",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CODIGOGARANTIA",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CODIGOREMEDIO",System.Type.GetType("System.String")));
			operacionesOrdenesGrilla.Columns.Add(new DataColumn("CODIGODEFECTO",System.Type.GetType("System.String")));
		}
		
		protected void Preparar_Tabla_Repuestos()
		{
			//////////////////////////////////////////////////////////////////////////////////////
			/// Preparamos la tabla que almacenara la informacion sobre los repuestos de la orden de trabajo
			repuestosOrdenesGrilla = new DataTable();
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("CODIGOKIT",System.Type.GetType("System.String")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("CODIGOREPUESTO",System.Type.GetType("System.String")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("REFERENCIA",System.Type.GetType("System.String")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("ORIGEN",System.Type.GetType("System.String")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("PRECIO",System.Type.GetType("System.Double")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("VALORFACTURADO",System.Type.GetType("System.Double")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));
			repuestosOrdenesGrilla.Columns.Add(new DataColumn("LINEABODEGA",System.Type.GetType("System.String")));
		}
		
		protected void Grilla_Operaciones(DataGrid dgOperaciones, DataTable dtOperaciones)
		{
			for(int i=0;i<dtOperaciones.Rows.Count;i++)
			{
				if((dtOperaciones.Rows[i][7].ToString().Trim())=="Gtía Fabrica")
				{
					((DropDownList)dgOperaciones.Items[i].Cells[9].Controls[1]).Visible = true;
					((DropDownList)dgOperaciones.Items[i].Cells[10].Controls[1]).Visible = true;
					((DropDownList)dgOperaciones.Items[i].Cells[11].Controls[1]).Visible = true;
					((DropDownList)dgOperaciones.Items[i].Cells[12].Controls[1]).Visible = true;
					DatasToControls bind = new DatasToControls();
                    bind.PutDatasIntoDropDownList(((DropDownList)dgOperaciones.Items[i].Cells[09].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'I' ORDER BY pgar_descripcion");
                    bind.PutDatasIntoDropDownList(((DropDownList)dgOperaciones.Items[i].Cells[10].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'C' ORDER BY pgar_descripcion");
                    bind.PutDatasIntoDropDownList(((DropDownList)dgOperaciones.Items[i].Cells[11].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'R' ORDER BY pgar_descripcion");
                    bind.PutDatasIntoDropDownList(((DropDownList)dgOperaciones.Items[i].Cells[12].Controls[1]), "SELECT pgar_codigo, pgar_descripcion FROM pgarantia WHERE tcau_codigo = 'D' ORDER BY pgar_descripcion");
				}
			}
		}
		
		protected void Ddls_GrillaOperaciones(DataGrid dgOperaciones, bool estado)
		{
			for(int i=0;i<dgOperaciones.Items.Count;i++)
			{
				((DropDownList)dgOperaciones.Items[i].Cells[10].Controls[1]).Enabled = estado;
				((DropDownList)dgOperaciones.Items[i].Cells[11].Controls[1]).Enabled = estado;
				((DropDownList)dgOperaciones.Items[i].Cells[12].Controls[1]).Enabled = estado;
				((DropDownList)dgOperaciones.Items[i].Cells[13].Controls[1]).Enabled = estado;
			}
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
	

