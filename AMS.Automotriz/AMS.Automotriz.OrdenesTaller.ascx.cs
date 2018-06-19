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
using AMS.Automotriz;

// anotacion hecha el 20 de septiembre de 2004 a las 12:13
// Para lograr conectar el control de usuario principal con los controles secundarios
// que se cargan cada vez que se da click en los botones de imagen se utiliza el session como forma de conexion
// el objeto session. Dentro de cada subcontrol se debe dar click en confirmar si no se hace no funcionara 
// el guardar la orden de trabajo.

namespace AMS.Automotriz
{
	public partial class OrdenesTaller : System.Web.UI.UserControl
	{
       
		protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected DataTable accesoriosGrabar, repuestosGrabar, operacionesGrabar, operacionesPeritajeGrabar;
		protected ArrayList cargoGrabar;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo=new FormatosDocumentos();
		protected string exceptions, manejoRepuestos = "";
        public static int contConfirmar = 0;
        public static bool errorGrabarGrilla = false;


		protected void Page_Load(object sender, System.EventArgs e)
		{
            
            //Utils.MostrarAlerta(Response, "A continuación, llene la información pestaña por pestaña y posteriormente, dar click en Guardar Orden! Gracias!!");
            Page.SmartNavigation = true;
            grabar.Attributes.Add("Onclick", "document.getElementById('" + grabar.ClientID + "').disabled = true;" + this.Page.GetPostBackEventReference(grabar) + ";");

			try
			{
				datosOrigen.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.DatosOrden.ascx"));
				datosPropietario.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.DatosPropietario.ascx"));
				datosVehiculo.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.DatosVehiculo.ascx"));
				otrosDatos.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.OtrosDatos.ascx"));			
				kitsCombos.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.KitsCombos.ascx"));
				estadoOrdenes.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.EstadoOrdenes.ascx"));
				operacionesPeritaje.Controls.Add(LoadControl(pathToControls+"AMS.Automotriz.OrdenTrabajo.OperacionesPeritaje.ascx"));
			}
			catch(Exception exc)
			{
				lb.Text += "Error : "+exc.ToString();
			}
			if(!IsPostBack)
			{
				Session.Clear();
				Session["grupoCatalogo"]="";
                //propietario.Enabled = false;
                //vehiculo.Enabled = false;
                //otros.Enabled = false;
                //botonKits.Enabled = false;
                //opPeritaje.Enabled = false;
                
				//IMPRIMIR REPORTES
				if(Request["prefOT"] != null && Request["prefOT"] != string.Empty && Request["numOT"] != null && Request["numOT"] != string.Empty)
				{
					try
					{
						//ORDEN
						formatoRecibo.Prefijo = Request["prefOT"];
						formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
						formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefOT"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
						{
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
						if(Request["pres"].Equals("S"))
						{
							//ORDEN (PRESUPUESTO)
							formatoRecibo.Prefijo = Request["prefOT"];
							formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
							formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefOT"]+"'");
							if(formatoRecibo.Codigo!=string.Empty)
							{
								if(formatoRecibo.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=750');</script>");
							}
						}
                        if (manejoRepuestos == "S") // Se debe definir en CTALLER y mostrar exactamente el formato que generó la orden NO OTRO 
                        {
						    //TRANSFERENCIA de los items 
						    formatoRecibo.Prefijo = Request["prefTRA"];
						    formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numTRA"]);
						    formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefTRA"]+"'");
						    if(formatoRecibo.Codigo!=string.Empty)
						    {
							    if(formatoRecibo.Cargar_Formato())
								    Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=700');</script>");
						    }
						    //PEDIDO de los items
						    formatoRecibo.Prefijo = Request["tipoPED"];
						    formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numPED"]);
						    formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.PPEDIDO WHERE pped_codigo='"+Request.QueryString["tipoPED"]+"'");
						    if(formatoRecibo.Codigo!=string.Empty)
						    {
							    if(formatoRecibo.Cargar_Formato())
								    Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=650');</script>");
						    }
                        }
					}
					catch
					{
						//lbInfo.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
                        Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
					}
				}
			}
            //grabar.Enabled = false;

            //CargaAutorizacionCliente
            plcAutorizar.Visible = false;
            autorizar.Visible = false;

            //Habeas Data
            
            if (Request.QueryString["nitCli"] != null && Request.QueryString["nombreCli"] != null && Request.QueryString["habeas"].ToLower() == "true") // && Request.QueryString["idDBCli"] != null
            {
                //Session["tipoDB"] = Session["nit"] = Session["nombre"] = null;
                string fecha1 = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                string fecha2 = DateTime.Now.ToString("yyyy-MM-dd");

                //si esta persona ya le hicieron el Habeas data en un rango de 6 meses a partir de hoy, no es necesario volverle a hacer el Habeas data
                if (DBFunctions.RecordExist("SELECT MB.MNIT_NIT, PB.PBAS_DESCRIPCION FROM MBASEDATOS MB, PBASEDATOS PB WHERE MB.MNIT_NIT = '" + Request.QueryString["nitCli"] + "' AND PB.TBAS_CODIGO = 'OT' AND MB.PBAS_CODIGO = PB.PBAS_CODIGO AND MBAS_FECHA BETWEEN '" + fecha1 + "' AND '" + fecha2 + "';"))
                {
                    return;
                }
                Session["tipoDB"] = "OT"; // Request.QueryString["idDBCli"];
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
                    Session["aceptaHabeas"] = null;
                }
            }
            
            //fin CargaAutorizacionCliente
            if (OrdenesTaller.contConfirmar == 5)
            {
                grabar.Enabled = true;
            }
            //Fin Habeas Data
        }

        #region Mostrar/Ocultar Paneles
        //protected void Cargar_DatosOrden(Object Sender, ImageClickEventArgs E)
        //{
        //    if(origen.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        datosOrigen.Visible = true;
        //        origen.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(origen.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        datosOrigen.Visible = false;
        //        origen.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }				
        //}

        //protected void Cargar_DatosPropietario(Object Sender, ImageClickEventArgs E)
        //{
        //    if(propietario.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        datosPropietario.Visible = true;
        //        propietario.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(propietario.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        datosPropietario.Visible = false;
        //        propietario.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }				
        //}

        //protected void Cargar_DatosVehiculo(Object Sender, ImageClickEventArgs E)
        //{
        //    if(vehiculo.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        datosVehiculo.Visible = true;
        //        vehiculo.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(vehiculo.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        datosVehiculo.Visible = false;
        //        vehiculo.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }
        //}

        //protected void Cargar_OtrosDatos(Object Sender, ImageClickEventArgs E)
        //{
        //    if(otros.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        otrosDatos.Visible = true;
        //        otros.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(otros.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        otrosDatos.Visible = false;
        //        otros.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }
        //}	

        //protected void Cargar_KitsCombos(Object Sender, ImageClickEventArgs E)
        //{
        //    if(botonKits.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        kitsCombos.Visible = true;
        //        botonKits.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(botonKits.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        kitsCombos.Visible = false;
        //        botonKits.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }
        //}		

        //protected void Cargar_EstadoOrdenes(Object Sender, ImageClickEventArgs E)
        //{
        //    if(estOrdenes.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        estadoOrdenes.Visible = true;
        //        estOrdenes.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(estOrdenes.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        estadoOrdenes.Visible = false;
        //        estOrdenes.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }
        //}

        //protected void Cargar_Operaciones_Peritaje(Object Sender, ImageClickEventArgs E)
        //{
        //    if(opPeritaje.ImageUrl=="../img/AMS.BotonExpandir.png")
        //    {
        //        operacionesPeritaje.Visible = true;
        //        opPeritaje.ImageUrl="../img/AMS.BotonContraer.png";				
        //    }
        //    else if(opPeritaje.ImageUrl=="../img/AMS.BotonContraer.png")
        //    {
        //        operacionesPeritaje.Visible = false;
        //        opPeritaje.ImageUrl="../img/AMS.BotonExpandir.png";
        //    }
        //}

        //protected void grabar_Orden2 (Object  Sender, EventArgs e) {
        //   Control controlDatosOrden = datosOrigen.Controls[0];
        //   DatosOrden daOr = new DatosOrden();
        //   DatosVehiculo daVeh = new DatosVehiculo();
        //   DatosPropietario daPro = new DatosPropietario();
        //   daPro.Confirmar(Sender, e);
        //   //daOr.Confirmar(Sender, e);

        //}


        #endregion Mostrar/Ocultar Paneles
        //FUNCION PARA GRABAR LA ORDEN
        protected void Grabar_Orden(Object Sender, EventArgs e)
        {

            grabar.Enabled = false;
            //Utils.MostrarAlerta(Response, "Grabación de la Orden. Acepte, espere e imprima los formatos !!!");


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////			
            //A continuacion cargamos los controles que contienen los datos necesarios para grabar la orden de trabajo en la base de datos
            Control controlDatosOrden = datosOrigen.Controls[0];
            Control controlDatosVehiculo = datosVehiculo.Controls[0];
            Control controlDatosPropietario = datosPropietario.Controls[0];
            Control controlAccesorios = otrosDatos.Controls[0];
            Control controlKitsCombos = kitsCombos.Controls[0];
            Control controlPeritaje = operacionesPeritaje.Controls[0];
            if (!DBFunctions.RecordExist("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + ((DropDownList)controlDatosOrden.FindControl("codigoRecep")).SelectedItem.ToString().Trim() + "'"))
            {
                Utils.MostrarAlerta(Response, "No se ha seleccionado un recepcionista, vuelva a la pestaña Datos de la orden  y revise por favor..");
                grabar.Enabled = true;
                return;
            }
            //Aun falta por revisar si hubo cambio de PROPIETARIO, cuando hay cambio de dueño debemos actualizar mcatalogovehiculo
            string nit = ((TextBox)controlDatosPropietario.FindControl("datos")).Text;
            string contacto = ((TextBox)controlDatosPropietario.FindControl("contacto")).Text; //== ""? DBFunctions.SingleData("SELECT MORD_CONTACTO FROM MORDEN WHERE PDOC_CODIGO = '" + ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedValue + "' AND MORD_NUMEORDE = " + ((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim() + ""): ((TextBox)controlDatosPropietario.FindControl("contacto")).Text;
            
            string tipid = ((DropDownList)controlDatosPropietario.FindControl("tipid")).SelectedValue;
            
            string numid = ((TextBox)controlDatosPropietario.FindControl("numid")).Text;
			string vin = ((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text;
            string telfijo= ((TextBox)controlDatosPropietario.FindControl("telFijo")).Text;
            string txtdireccion = ((TextBox)controlDatosPropietario.FindControl("txtdireccion")).Text;
            string txtemail = ((TextBox)controlDatosPropietario.FindControl("txtemail")).Text;
            string tipoUsuario = DBFunctions.SingleData("SELECT tpro_codigo FROM tpropietariotaller WHERE tpro_nombre='" + ((RadioButtonList)controlDatosPropietario.FindControl("tipoCliente")).SelectedItem.ToString().Trim() + "'");
            string telCon = "";
            try
            {
                telCon = ((TextBox)controlDatosPropietario.FindControl("telFijo")).Text;
            }catch
            {
                telCon = DBFunctions.SingleData("SELECT MORD_NUMECONT FROM MORDEN WHERE PDOC_CODIGO = '" + ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedValue + "' AND MORD_NUMEORDE = " + ((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim() + "");
            }
            //if (telCon == "") telCon = "no ingresa";

            if (DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_vin='" + vin + "'") != nit && tipoUsuario == "P")
				DBFunctions.NonQuery("UPDATE mcatalogovehiculo SET mnit_nit='"+nit+"' WHERE mcat_vin='"+vin+"'");

			//Ahora vamos a revisar si la orden de trabajo viene por parte de un seguro, o por cualquiera de los dos tipos de garantia
			string cargo = ((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedValue;
			//
			if(cargo == "S") 
			{
				cargoGrabar = new ArrayList();
				cargoGrabar.Add("seguro");
				cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Text);
				cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("siniestro")).Text);
				cargoGrabar.Add(Convert.ToDouble(((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Text.Trim()).ToString());
				cargoGrabar.Add(Convert.ToDouble(((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Text.Trim()).ToString());
				cargoGrabar.Add(((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Text);
			}
			else if(cargo == "G")
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
			if(!Distribuir_Tablas(((DataGrid)controlAccesorios.FindControl("accesorios")),((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")),DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedItem.ToString()+"'"),((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim())&&(!Distribuir_Tablas_Repuestos()))
			{
                if (errorGrabarGrilla == true)
                {
                    grabar.Enabled = true;
                    return;
                }
				Orden miOrden = null;
				Cotizacion miCotizacion = new Cotizacion();
		        string slqDatos =  @"SELECT ttra_codigo, tcar_cargo, pven_codigo, PA.palm_almacen, ppreta_codigo, ttip_codigo, tnivcomb_codigo
                                       FROM ttrabajoorden,
                                            tcargoorden,
                                            pvendedor,
                                            palmacen PA,
				                            ppreciotaller,
				                            ttipopago,
				                            tnivelcombustible
                                      WHERE ttra_nombre = '"+((DropDownList)controlDatosOrden.FindControl("servicio")).SelectedItem.ToString().Trim()+@"'
                                        AND tcar_nombre = '"+((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedItem.ToString().Trim()+@"'
                                        AND pven_nombre = '" + ((DropDownList)controlDatosOrden.FindControl("codigoRecep")).SelectedItem.ToString().Trim() + @"'
                                        AND palm_descripcion = '"+((DropDownList)controlDatosOrden.FindControl("almacen")).SelectedItem.ToString().TrimEnd()+@"'
                                        AND ppreta_nombre = '"+((DropDownList)controlDatosOrden.FindControl("listaPrecios")).SelectedItem.ToString().Trim()+@"'
                                        AND ttip_nombre = '"+((DropDownList)controlDatosPropietario.FindControl("tipoPago")).SelectedItem.ToString()+@"'
                                        AND tnivcomb_descripcion = '"+((RadioButtonList)controlAccesorios.FindControl("nivelCombustible")).SelectedItem.ToString()+"'; ";
                DataSet datosOrden = new DataSet();
                DBFunctions.Request(datosOrden, IncludeSchema.NO, slqDatos);


                string tipoTrabajo = datosOrden.Tables[0].Rows[0]["TTRA_CODIGO"].ToString(); //DBFunctions.SingleData("SELECT ttra_codigo FROM ttrabajoorden WHERE ttra_nombre='" + ((DropDownList)controlDatosOrden.FindControl("servicio")).SelectedItem.ToString().Trim() + "'");

                if (tipoTrabajo == "P")
				{
					int cantidadGrillas = System.Convert.ToInt32(Session["cantidadGrillas"]);
					DataTable []tablasAsociadas;
					tablasAsociadas = new DataTable[cantidadGrillas];
					for(int i=0;i<cantidadGrillas;i++)
					{
						tablasAsociadas[i] = ((DataTable[])Session["tablasAsociadas"])[i].Copy();
					}
					Distribuir_Tabla_Peritaje(((PlaceHolder)controlPeritaje.FindControl("gruposPeritaje")),tablasAsociadas,((ArrayList)Session["codigosGruposPeritaje"]),cantidadGrillas,DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='"+((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedItem.ToString()+"'"),((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim());
					miOrden = new Orden(operacionesGrabar,accesoriosGrabar,operacionesPeritajeGrabar,cargoGrabar,true);
				}
				else
				    miOrden = new Orden(operacionesGrabar,accesoriosGrabar,repuestosGrabar,cargoGrabar);
				//En caso que sea una orden de trabajo la grabamos aqui	
                miOrden.CodigoPrefijo       = ((DropDownList)controlDatosOrden.FindControl("tipoDocumento")).SelectedValue;
				miOrden.NumeroOrden         = ((TextBox)controlDatosOrden.FindControl("numOrden")).Text.Trim();
				miOrden.Catalogo            = ((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue;
				miOrden.VinIdentificacion   = ((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text;
				miOrden.NitPropietario      = ((TextBox)controlDatosPropietario.FindControl("datos")).Text;
				miOrden.TipoUsuario         = tipoUsuario;
				miOrden.EstadoOrden         = "A";
				miOrden.Cargo               = datosOrden.Tables[0].Rows[0]["TCAR_CARGO"].ToString(); //DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre='"+((DropDownList)controlDatosOrden.FindControl("cargo")).SelectedItem.ToString().Trim()+"'");
                miOrden.TipoTrabajo         = tipoTrabajo; // DBFunctions.SingleData("SELECT ttra_codigo FROM ttrabajoorden WHERE ttra_nombre='"+((DropDownList)controlDatosOrden.FindControl("servicio")).SelectedItem.ToString().Trim()+"'");
				miOrden.FechaEntrada        = ((TextBox)controlDatosOrden.FindControl("fecha")).Text.Trim();
				miOrden.HoraEntrada         = ((TextBox)controlDatosOrden.FindControl("hora")).Text.Trim()+":"+((TextBox)controlDatosOrden.FindControl("minutos")).Text.Trim();
				miOrden.FechaHoraCreacion   = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				miOrden.FechaEntrega        = UtilitarioPlanning.ValidarParametrosFecha(Convert.ToDateTime(((TextBox)controlKitsCombos.FindControl("fechaEstimada")).Text)).ToString("yyyy-MM-dd");
				miOrden.HoraEntrega         = ((TextBox)controlKitsCombos.FindControl("horaEstimada")).Text;
				miOrden.Salida              = "NULL";
				miOrden.NumeroEntrada       = ((TextBox)controlDatosOrden.FindControl("entrada")).Text.Trim();
				miOrden.Kilometraje         = System.Convert.ToDouble(((TextBox)controlDatosVehiculo.FindControl("kilometraje")).Text.Trim()).ToString();
               // miOrden.Vendedor          = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + ((DropDownList)controlDatosOrden.FindControl("codigoVende")).SelectedItem.ToString().Trim() + "'");
                miOrden.Recepcionista       = datosOrden.Tables[0].Rows[0]["PVEN_CODIGO"].ToString(); //DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + ((DropDownList)controlDatosOrden.FindControl("codigoRecep")).SelectedItem.ToString().Trim() + "'");
				miOrden.Taller              = datosOrden.Tables[0].Rows[0]["PALM_ALMACEN"].ToString();  //DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE palm_descripcion='"+((DropDownList)controlDatosOrden.FindControl("almacen")).SelectedItem.ToString().Trim()+"'");
				miOrden.EstadoPrecios       = "A";
				miOrden.NumeroLocker        = ((TextBox)controlDatosOrden.FindControl("locker")).Text.Trim();
				miOrden.EstadoLiquidacion   = "A";
				miOrden.ObsrCliente         = ((TextBox)controlDatosOrden.FindControl("obsrCliente")).Text;
				miOrden.ObsrRecepcionista   = ((TextBox)controlDatosOrden.FindControl("obsrRecep")).Text;
				miOrden.TP                  = ((TextBox)controlAccesorios.FindControl("tp")).Text;
				miOrden.TPP                 = ((TextBox)controlAccesorios.FindControl("tpp")).Text;
				miOrden.ListaPrecios        = datosOrden.Tables[0].Rows[0]["PPRETA_CODIGO"].ToString();  //DBFunctions.SingleData("SELECT ppreta_codigo FROM ppreciotaller WHERE ppreta_nombre='"+((DropDownList)controlDatosOrden.FindControl("listaPrecios")).SelectedItem.ToString().Trim()+"'");
				miOrden.TipoPago            = datosOrden.Tables[0].Rows[0]["TTIP_CODIGO"].ToString();  //DBFunctions.SingleData("SELECT ttip_codigo FROM ttipopago WHERE ttip_nombre='"+((DropDownList)controlDatosPropietario.FindControl("tipoPago")).SelectedItem.ToString()+"'");
				miOrden.NivelCombustible    = datosOrden.Tables[0].Rows[0]["TNIVCOMB_CODIGO"].ToString();  //DBFunctions.SingleData("SELECT tnivcomb_codigo FROM tnivelcombustible WHERE tnivcomb_descripcion='"+((RadioButtonList)controlAccesorios.FindControl("nivelCombustible")).SelectedItem.ToString()+"'");
				miOrden.CodigoEstadoCita    = ((Label)controlDatosOrden.FindControl("lbEstCita")).Text.Trim();
                try
                {
                    miOrden.NitTransferencia = ((DropDownList)controlDatosOrden.FindControl("nitTransferencias")).SelectedItem.ToString();
                }
                catch
                {
                    miOrden.NitTransferencia = "";
                }
                try
                {
                    miOrden.TipoTransferencia = ((DropDownList)controlDatosOrden.FindControl("tipoPedido")).SelectedValue;
                }
                catch
                {
                    miOrden.TipoTransferencia = "";
                }
                try
                {
                    miOrden.PrefijoTransferencia = ((DropDownList)controlDatosOrden.FindControl("prefijoTransferencia")).SelectedValue;
                }
                catch
                {
                    miOrden.PrefijoTransferencia = "";
                }

                miOrden.TotalTransferencia  = this.Calcular_Total_Transferencia();
				miOrden.TotalTransferenciaSinIVA = this.Calcular_Total_Transferencia_SinIVA();
                try
                {
                    miOrden.AlmacenTransferencia = ((DropDownList)controlDatosOrden.FindControl("almacenTransferencia")).SelectedValue;
                }
                catch
                {
                    miOrden.AlmacenTransferencia = "";
                }
                miOrden.KitsAplicados       = (ArrayList)Session["escogidos2"];
                miOrden.Contacto            = contacto;
                miOrden.Tipid               = tipid;
                miOrden.Numid               = numid;
                miOrden.Telfijo             = telCon;
                miOrden.Txtdireccion        = txtdireccion;
                miOrden.Txtemail            = txtemail;

                //  esto se debe realizar por un sistema de coordenas y no por nombres de las partes del vehiculo, para permitir una marcación bien real del vehículo
                //miOrden.Gdelantizq          = ((TextBox)controlAccesorios.FindControl("txt_gdelantizq")).Text;
                //miOrden.Puertadelantizq     = ((TextBox)controlAccesorios.FindControl("txt_pdelantizq")).Text;
                //miOrden.Capo                = ((TextBox)controlAccesorios.FindControl("txt_capo")).Text;
                //miOrden.Gdelantdere         = ((TextBox)controlAccesorios.FindControl("txt_gdelantdere")).Text;
                //miOrden.Puertadelantdere    = ((TextBox)controlAccesorios.FindControl("txt_pdelantdere")).Text;
                //miOrden.Puertatrasizq       = ((TextBox)controlAccesorios.FindControl("txt_ptrasizq")).Text;
                //miOrden.Gtrasizq            = ((TextBox)controlAccesorios.FindControl("txt_gtrasizq")).Text;
                //miOrden.Maletero            = ((TextBox)controlAccesorios.FindControl("txt_maletero")).Text;
                //miOrden.Techo               = ((TextBox)controlAccesorios.FindControl("txt_techo")).Text;
                //miOrden.Puertatrasdere      = ((TextBox)controlAccesorios.FindControl("txt_ptrasder")).Text;
                //miOrden.Gtrasdere           = ((TextBox)controlAccesorios.FindControl("txt_gtrasder")).Text; 

                string nombreCliente = ((TextBox)controlDatosPropietario.FindControl("datosa")).Text + " " + ((TextBox)controlDatosPropietario.FindControl("datosb")).Text;

                if (((DropDownList)controlAccesorios.FindControl("ddlencuesta")).SelectedValue == "S")
                {
                    miOrden.AceptaUsoDatos = true;
                    miOrden.AceptaEncuesta = true;
                    Session["aceptaHabeas"] = "1";
                }
                else
                {
                    miOrden.AceptaUsoDatos = false;
                    miOrden.AceptaEncuesta = false;
                }

                //miOrden.RevisionElevador = ((CheckBox)controlAccesorios.FindControl("chkElevador")).Checked;
                if (((DropDownList)controlAccesorios.FindControl("ddlelevador")).SelectedValue=="S")
					miOrden.RevisionElevador = true;
				else
					miOrden.RevisionElevador = false;
                //miOrden.EntregoPresupuesto = ((CheckBox)controlAccesorios.FindControl("chkPresupuesto")).Checked;
                if (((DropDownList)controlAccesorios.FindControl("ddlpresupuesto")).SelectedValue == "S")
                    miOrden.EntregoPresupuesto = true;
                else
                    miOrden.EntregoPresupuesto = false;

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
                    string numPedido        = DBFunctions.SingleData("Select MPED_NUMERO from DBXSCHEMA.MPEDIDOTRANSFERENCIA where MORD_NUMEORDE=" + miOrden.NumeroOrden + " AND PPED_CODIGO='" + miOrden.TipoTransferencia + "';");
                    string numTransferencia = DBFunctions.SingleData("Select MFAC_NUMERO from DBXSCHEMA.MORDENTRANSFERENCIA where MORD_NUMEORDE=" + miOrden.NumeroOrden + " AND PDOC_FACTURA='" + miOrden.PrefijoTransferencia + "';");
                    string presupuesto      = miOrden.EntregoPresupuesto ? "S" : "N";
                    
                    lb.Text = "<br>BIEN : " + miOrden.ProcessMsg;
                    Session.Clear();
                    
                    //Se envia por parametro nit y nombre cliente para autorizacion de confidencialidad.
                    Response.Redirect("" + indexPage + "?process=Automotriz.OrdenTrabajo&prefOT=" + miOrden.CodigoPrefijo + "&numOT=" + miOrden.NumeroOrden + "&prefTRA=" + miOrden.PrefijoTransferencia + "&numTRA=" + numTransferencia + "&tipoPED=" + miOrden.TipoTransferencia + "&numPED=" + numPedido + "&pres=" + presupuesto + "&nitCli=" + miOrden.NitPropietario + "&nombreCli=" + nombreCliente + "&habeas=" + miOrden.AceptaUsoDatos);
                    
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
			//Empezamos por distribuir la estructura de la tabla de accesorios
			accesoriosGrabar = new DataTable();
			accesoriosGrabar.Columns.Add(new DataColumn("CODIGOPREFIJO",System.Type.GetType("System.String")));//0
			accesoriosGrabar.Columns.Add(new DataColumn("NUMEROORDEN",System.Type.GetType("System.String")));//1
			accesoriosGrabar.Columns.Add(new DataColumn("CODIGOACCESORIO",System.Type.GetType("System.String")));//2
			accesoriosGrabar.Columns.Add(new DataColumn("ACCESORIODESCRIPCION",System.Type.GetType("System.String")));//3
			//distribuimos la estructura de la tabla de operaciones
			operacionesGrabar = new DataTable();
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOPREFIJO",System.Type.GetType("System.String")));//0
			operacionesGrabar.Columns.Add(new DataColumn("NUMEROORDEN",System.Type.GetType("System.String")));//1
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOOPERACION",System.Type.GetType("System.String")));//2
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOCARGO",System.Type.GetType("System.String")));//3
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOMECANICO",System.Type.GetType("System.String")));//4
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOESTADO",System.Type.GetType("System.String")));//5
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOINCIDENTE",System.Type.GetType("System.String")));//6
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOGARANTIA",System.Type.GetType("System.String")));//7
			operacionesGrabar.Columns.Add(new DataColumn("CODIGOREMEDIO",System.Type.GetType("System.String")));//8
			operacionesGrabar.Columns.Add(new DataColumn("CODIGODEFECTO",System.Type.GetType("System.String")));//9
			operacionesGrabar.Columns.Add(new DataColumn("VALOROPERACION",System.Type.GetType("System.Double")));//10
            operacionesGrabar.Columns.Add(new DataColumn("TIEMPOOPERACION", System.Type.GetType("System.Double")));//11
            operacionesGrabar.Columns.Add(new DataColumn("OBSERVACION", System.Type.GetType("System.String")));//12
		}
		
		protected void Preparar_Tabla_Repuestos()
		{
			//distribuimos la estructura de la tabla de repuestos
			repuestosGrabar = new DataTable();
			repuestosGrabar.Columns.Add(new DataColumn("CODIGOITEM",System.Type.GetType("System.String")));//0
			repuestosGrabar.Columns.Add(new DataColumn("CANTIDADITEM",System.Type.GetType("System.Double")));//1
			repuestosGrabar.Columns.Add(new DataColumn("PRECIOITEM",System.Type.GetType("System.Double")));//2
			repuestosGrabar.Columns.Add(new DataColumn("IVAITEM",System.Type.GetType("System.Double")));//3
			repuestosGrabar.Columns.Add(new DataColumn("DESCUENTO",System.Type.GetType("System.Double")));//4
			repuestosGrabar.Columns.Add(new DataColumn("CANTIDADPEDIDA",System.Type.GetType("System.Double")));//5
			repuestosGrabar.Columns.Add(new DataColumn("CARGOITEM",System.Type.GetType("System.String")));//6
		}
		
		//Esta funcion nos permite cargar la tabla especifica para poder guardar las operaciones especificas 
		protected void Preparar_Tabla_Peritaje()
		{
			operacionesPeritajeGrabar = new DataTable();
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("CODIGOPREFIJO",System.Type.GetType("System.String")));
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("NUMEROORDEN",System.Type.GetType("System.String")));
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("GRUPOPERITAJE",System.Type.GetType("System.String")));
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("ITEMPERITAJE",System.Type.GetType("System.String")));
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("DETALLE",System.Type.GetType("System.String")));
			operacionesPeritajeGrabar.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.String")));
		}
		
		/////////////////////////////////////////////////////////////////////////
		
		protected bool Distribuir_Tablas_Repuestos()
		{
			bool itemRepetido = false;
			Preparar_Tabla_Repuestos();
			DataTable tablaItems = (DataTable)Session["items"];
			if(tablaItems!=null)
			{
				for(int i=0;i<tablaItems.Rows.Count;i++)
				{
					DataRow fila = repuestosGrabar.NewRow();
					fila[0] = tablaItems.Rows[i][10].ToString();
					fila[1] = Convert.ToDouble(tablaItems.Rows[i][5]);
					fila[2] = Convert.ToDouble(tablaItems.Rows[i][4]);
					fila[3] = Convert.ToDouble(tablaItems.Rows[i][7]);
					fila[4] = 0;
					fila[5] = Convert.ToDouble(tablaItems.Rows[i][5]);
					fila[6] = DBFunctions.SingleData("SELECT tcar_cargo FROM tcargoorden WHERE tcar_nombre='"+tablaItems.Rows[i][9].ToString().Trim()+"'");
					repuestosGrabar.Rows.Add(fila);
				}
				for(int i=0;i<repuestosGrabar.Rows.Count;i++)
				{
					for(int j=i+1;j<repuestosGrabar.Rows.Count;j++)
					{
						if((repuestosGrabar.Rows[i][0].ToString()) == (repuestosGrabar.Rows[j][0].ToString()))
							itemRepetido = true;
					}
				}
			}
			return itemRepetido;
		}
		
		protected double Calcular_Total_Transferencia()
		{
			double total = 0;
			if(repuestosGrabar!=null)
			{
				for(int i=0;i<repuestosGrabar.Rows.Count;i++)
					total += (System.Convert.ToDouble(repuestosGrabar.Rows[i][2])+(System.Convert.ToDouble(repuestosGrabar.Rows[i][2])*(System.Convert.ToDouble(repuestosGrabar.Rows[i][3])/100)))*(System.Convert.ToDouble(repuestosGrabar.Rows[i][1]));
			}
			return total;
		}
		
		protected double Calcular_Total_Transferencia_SinIVA()
		{
			double total = 0;
			if(repuestosGrabar!=null)
			{
				for(int i=0;i<repuestosGrabar.Rows.Count;i++)
					total += System.Convert.ToDouble(repuestosGrabar.Rows[i][2]);
			}
			return total;
		}
		
		protected bool Distribuir_Tablas(DataGrid accesorios, DataGrid operaciones, string codigoPrefijo, string numeroOrden)
		{
			int i;
			bool operacionRepetida = false;
			//Se preparan las datatables que contendran los datos a grabar en la base de datos
			Preparar_Tablas();
			// A continuacion vamos a llenar la tabla que contendra los accesorios de la orden
			DataTable tablaAccesorios = new DataTable();
			tablaAccesorios = (DataTable)Session["tablaAccesorios"];
			if(tablaAccesorios != null)
			{
				for(i=0;i<tablaAccesorios.Rows.Count;i++)
				{
					if(Convert.ToBoolean(tablaAccesorios.Rows[i][3]))
					{
						DataRow fila = accesoriosGrabar.NewRow();
						fila[0] = codigoPrefijo;
						fila[1] = numeroOrden;
						fila[2] = tablaAccesorios.Rows[i][0].ToString();
						fila[3] = tablaAccesorios.Rows[i][2].ToString();
						accesoriosGrabar.Rows.Add(fila);
					}
				}
			}
			else
                Utils.MostrarAlerta(Response, "Error Fatal En Modulo");
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
                    //fila[4] = DBFunctions.SingleData("SELECT pven_codigo FROM pvendedor WHERE pven_nombre='" + tablaOperaciones.Rows[i][6].ToString().Trim() + "'");
                    fila[4] = tablaOperaciones.Rows[i][6].ToString().Trim();
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
                    {
                        if (((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text.Trim() == "")
                        {
                            Utils.MostrarAlerta(Response, "el valor de la operación no puede estár vacio, Revise por favor..!");
                            errorGrabarGrilla = true;
                            break;
                        }
                        else
                        {
                            errorGrabarGrilla = false;
                            fila[10] = Convert.ToDouble(((TextBox)operaciones.Items[i].Cells[5].Controls[1]).Text.Trim());
                        }
                    }
                    else
                        fila[10] = Convert.ToDouble(tablaOperaciones.Rows[i][4].ToString().Trim());
                    fila[11] = Convert.ToDouble(tablaOperaciones.Rows[i][3]);
                    fila[12] = ((TextBox)operaciones.Items[i].Cells[3].Controls[1]).Text;

                    operacionesGrabar.Rows.Add(fila);
                }
            }
			//Comprobacion de que una operacion esta repetida o no
			for(i=0;i<operacionesGrabar.Rows.Count;i++)
			{
				for(int j=i+1;j<operacionesGrabar.Rows.Count;j++)
				{
					if((operacionesGrabar.Rows[i][2].ToString())==(operacionesGrabar.Rows[j][2].ToString()))
						operacionRepetida = true;
				}
			}
            
            return operacionRepetida;
		}
		
		protected void Distribuir_Tabla_Peritaje(PlaceHolder gruposPeritaje, DataTable []tablasAsociadas, ArrayList codigosGruposPeritaje, int cantidadGrillas, string codigoPrefijo, string numeroOrden)
		{
			this.Preparar_Tabla_Peritaje();
			for(int i=0;i<cantidadGrillas;i++)
			{
				int cantidadOperaciones = tablasAsociadas[i].Rows.Count;
				for(int j=0;j<cantidadOperaciones;j++)
				{
					DataRow fila = operacionesPeritajeGrabar.NewRow();
					fila[0] = codigoPrefijo;
					fila[1] = numeroOrden;					
					fila[2] = codigosGruposPeritaje[i].ToString();
					fila[3] = DBFunctions.SingleData("SELECT pitp_codigo FROM pitemperitaje WHERE pitp_descripcion='"+tablasAsociadas[i].Rows[j][0].ToString()+"' AND pgrp_codigo='"+codigosGruposPeritaje[i].ToString()+"'");
					fila[4] = DBFunctions.SingleData("SELECT tespe_codigo FROM testadoperitaje WHERE tespe_descripcion='"+((RadioButtonList)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[1].Controls[0]).SelectedItem.ToString()+"'");
					if((((TextBox)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[2].Controls[0]).Text)=="")
						fila[5] = "";
					else
						fila[5] = ((TextBox)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[2].Controls[0]).Text;
					if((((TextBox)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[3].Controls[0]).Text)=="")
						fila[6] = "null";
					else
						fila[6] = ((TextBox)((DataGrid)gruposPeritaje.Controls[((i*2)+1)]).Items[j].Cells[3].Controls[0]).Text;
					operacionesPeritajeGrabar.Rows.Add(fila);
				}
			}
		}

		protected void btnCancelar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Automotriz.OrdenTrabajo");
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
