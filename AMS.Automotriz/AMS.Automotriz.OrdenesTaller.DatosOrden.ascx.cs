// created on 20/09/2004 at 10:42
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
using Ajax;
using AMS.Tools;
using AMS.Automotriz;

namespace AMS.Automotriz
{
	public partial class DatosOrden : System.Web.UI.UserControl
	{
		#region Atributos
		protected PlaceHolder own;
		private DatasToControls bind = new DatasToControls();
        string obserCita;
        bool pideRepuestos = false;
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{

			//botones q hace la confirmacion de los datos y cancela el boton para q no se produzca. doble facturacion o etc
			//confirmar.Attributes.Add("onClick","if(confirm('Esta seguro que desea Continuar?')){document.getElementById('"+confirmar.ClientID+"').disabled = true;"+this.Page.GetPostBackEventReference(confirmar)+";return true;}else {return false;}");
			Ajax.Utility.RegisterTypeForAjax(typeof(DatosOrden));
			if(!IsPostBack)
			{
                if (DBFunctions.SingleData("SELECT CTAL_PIDEREPU FROM CTALLER") == "S") //Pide los repuestos desde la creacion de la orden de trabajo
                    pideRepuestos = true;
           //     else
           //         datosTransferencia.Attributes.Add("Visible", "false");

                Utils.FillDll(almacen, "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE (pcen_centtal is not null or pcen_centcoli is not null) AND TVIG_VIGENCIA = 'V' ORDER BY palm_descripcion ASC", true);
                Cambio_Taller(null, null);
                Utils.FillDll(cargo, "SELECT tcar_cargo, tcar_nombre FROM tcargoorden WHERE tcar_cargo <> 'X' ORDER BY tcar_nombre", true);
                Utils.FillDll(servicio, "SELECT ttra_codigo, ttra_nombre FROM ttrabajoorden ORDER BY ttra_nombre", true);
                Utils.FillDll(listaPrecios, "SELECT ppreta_codigo, ppreta_nombre FROM ppreciotaller ORDER BY ppreta_nombre", true);
                Utils.FillDll(listaPreciosItems, "SELECT ppre_codigo, ppre_nombre FROM pprecioitem ORDER BY ppre_nombre", true);
                Utils.FillDll(nitTransferencias, "SELECT distinct pnital_nittaller FROM pnittaller WHERE palm_almacen='" + almacen.SelectedValue + "' order by pnital_nittaller", true);
                Utils.FillDll(tipoPedido, "SELECT pped_codigo, pped_nombre FROM ppedido WHERE tped_codigo='T' order by pped_nombre", true);
                Utils.FillDll(almacenTransferencia, "SELECT palm_almacen, palm_descripcion || ' - ' || palm_almacen FROM palmacen WHERE TVIG_VIGENCIA = 'V' AND pcen_centinv is not null ORDER BY palm_descripcion ASC", true);
                Utils.FillDll(codigoRecep, @"SELECT pv.pven_codigo, 
                                                pv.pven_nombre  
                                         FROM pvendedor pv  
                                         INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo  
                                         WHERE pv.tvend_codigo in ( 'RT', 'RA', 'TT' ) 
                                         AND   pv.pven_vigencia = 'V'  
                                         AND   pva.palm_almacen = '" + almacen.SelectedValue + @"' 
                                         ORDER BY pven_nombre ASC", true);
                
                Utils.llenarPrefijos(Response, ref prefijoTransferencia , "%", "%", "TT");
                numOrden.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento where pdoc_codigo='"+tipoDocumento.SelectedValue+"'");
				DateTime Fecha = DateTime.Now;
				fecha.Text = Fecha.Date.ToString("yyyy-MM-dd");
				hora.Text = Fecha.Hour.ToString();
				minutos.Text = Fecha.Minute.ToString();
				if((minutos.Text.Length) == 1)
					minutos.Text = "0"+minutos.Text;

			}
			else
			{
                RevisionValoresDependientesTaller();
			}
		}

        protected void Cambio_Taller(Object Sender, EventArgs e)
        {
            Utils.FillDll(codigoRecep, @"SELECT pv.pven_codigo,      
                                                pv.pven_nombre      
                                         FROM pvendedor pv      
                                         INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo      
                                         WHERE pv.tvend_codigo IN ( 'RT', 'RA', 'TT')      
                                         AND   pv.pven_vigencia = 'V'      
                                         AND   pva.palm_almacen = '" + almacen.SelectedValue + @"'      
                                         ORDER BY pven_nombre ASC", true);

            Utils.FillDll(nitTransferencias, "SELECT pnital_nittaller FROM pnittaller WHERE palm_almacen='" + almacen.SelectedValue + "'", false);

            if (Session["modificandoOT"] == null)
            {
                Utils.llenarPrefijos(Response, ref tipoDocumento, "OT", almacen.SelectedValue, "%");
                CambioTipoOT(null, null);
            }
        }

        //[Ajax.AjaxMethod()]
        //public ArrayList cambia_Taller(string obj)
        //{
        //    ArrayList arrayList = new ArrayList();
        //    arrayList = DBFunctions.RequestAsCollection(@"SELECT pv.pven_codigo,      
        //                                        pv.pven_nombre      
        //                                 FROM pvendedor pv      
        //                                 INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo      
        //                                 WHERE pv.tvend_codigo IN ( 'RT', 'RA', 'TT')      
        //                                 AND   pv.pven_vigencia = 'V'      
        //                                 AND   pva.palm_almacen = '" + obj + @"'      
        //                                 ORDER BY pven_nombre ASC");

        //    //Utils.FillDll(codigoRecep, @"SELECT pv.pven_codigo,      
        //    //                                    pv.pven_nombre      
        //    //                             FROM pvendedor pv      
        //    //                             INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo      
        //    //                             WHERE pv.tvend_codigo IN ( 'RT', 'RA', 'TT')      
        //    //                             AND   pv.pven_vigencia = 'V'      
        //    //                             AND   pva.palm_almacen = '" + obj + @"'      
        //    //                             ORDER BY pven_nombre ASC", true);

        //    //Utils.FillDll(nitTransferencias, "SELECT pnital_nittaller FROM pnittaller WHERE palm_almacen='" + almacen.SelectedValue + "'", false);

        //    //if (Session["modificandoOT"] == null)
        //    //{
        //    //    Utils.llenarPrefijos(Response, ref tipoDocumento, "OT", almacen.SelectedValue, "%");
        //    //    CambioTipoOT(null, null);
        //    //}
        //    return arrayList;
        //}
        [Ajax.AjaxMethod()]
        public string cambio_TipoOT(string tipoDocu)
        {
            string pdocUltiDoc = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento where pdoc_codigo='" + tipoDocu + "'");
            return pdocUltiDoc;
        }
		
		protected void CambioTipoOT(Object  Sender, EventArgs e)
		{  
            numOrden.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento where pdoc_codigo='" + tipoDocumento.SelectedValue + "'");
		}
		
		public void Confirmar(Object  Sender, EventArgs e)
		{
            string error = RevisionParametros();
			string ordenes="";

            string prefijo = tipoDocumento.SelectedValue;
            string numero = numOrden.Text;

            while (DBFunctions.RecordExist("select * from dbxschema.morden where pdoc_codigo='" + prefijo + "' and mord_numeorde=" + numero + ""))
            {
                numero = Convert.ToString(Convert.ToInt32(numero) + 1);
            }
			if (DBFunctions.RecordExist("select * from dbxschema.morden where pdoc_codigo='"+prefijo+"' and mord_numeorde="+numero+""))
			{
				numero=Convert.ToString(Convert.ToInt32(numero)+1);
                Utils.MostrarAlerta(Response, "El Numero de Orden ya existe, sera asignado el siguiente " + numero + "");
				return;
			}


            if (error != "")
            {
                Utils.MostrarAlerta(Response, "" + error + "");
                return;
            }

			if(ExistenOrdenes(placa.Text,ref ordenes))
			{
                Utils.MostrarAlerta(Response, "Este vehículo tiene las siguientes ordenes de trabajo abiertas \\n" + ordenes + "");
			}

            if (DBFunctions.RecordExist("SELECT MG.MCAT_VIN FROM MCATALOGOVEHICULO MC, MPLANGARANTIA MG WHERE MC.MCAT_PLACA = '"+ placa.Text + "' AND MC.MCAT_VIN = MG.MCAT_VIN AND (MG.PDOC_CODIGO = '' OR MG.PDOC_CODIGO IS NULL)"))
            {
                Utils.MostrarAlerta(Response, "Este vehículo tiene CAMPAÑA de GARANTÍA de FÁBRICA \\n");
            }

            //verificacion de clave valida, a futuro debe modificarse para almacenar claves codificadas

            if (clave.Text == (DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='" + codigoRecep.SelectedValue + "'")) && codigoRecep.SelectedValue != "0")
			{
                
                //Aqui Debemos Habilitar los controles del placeholder datosPropietario
                Control principal = (this.Parent).Parent; //Control principal ascx
				Control controlDatosPropietario = ((PlaceHolder)principal.FindControl("datosPropietario")).Controls[0];
				Control controlDatosVehiculo = ((PlaceHolder)principal.FindControl("datosVehiculo")).Controls[0];
				Control controlKitsCombos = ((PlaceHolder)principal.FindControl("kitsCombos")).Controls[0];                
                
                if(DBFunctions.RecordExist("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'"))
				{
					//Aqui cargamos los datos del cliente en el control respectivo al cliente
					string mnit = DBFunctions.SingleData("SELECT mnit_nit FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'");
					DataSet datosCliente = new DataSet();
                    DBFunctions.Request(datosCliente, IncludeSchema.NO, "SELECT mnit_nit , mnit_nombres concat ' ' concat coalesce(mnit_nombre2,'') , mnit_apellidos concat ' ' concat coalesce(mnit_apellido2,'') , mnit_direccion, pciu_codigo, mnit_telefono, mnit_celular, mnit_email, mnit_web FROM mnit WHERE mnit_nit='" + mnit + "'");
					((TextBox)controlDatosPropietario.FindControl("datos")).Text = datosCliente.Tables[0].Rows[0][0].ToString();
					((TextBox)controlDatosPropietario.FindControl("datosa")).Text = datosCliente.Tables[0].Rows[0][1].ToString();
					((TextBox)controlDatosPropietario.FindControl("datosb")).Text = datosCliente.Tables[0].Rows[0][2].ToString();
					((TextBox)controlDatosPropietario.FindControl("datosc")).Text = datosCliente.Tables[0].Rows[0][3].ToString();
					//string ciudad=Convert.ToString(DBFunctions.SingleData("select pciu_nombre from dbxschema.pciudad where pciu_codigo="+datosCliente.Tables[0].Rows[0][4].ToString()+""));
					((DropDownList)controlDatosPropietario.FindControl("datosd")).SelectedValue = datosCliente.Tables[0].Rows[0][4].ToString();
					((TextBox)controlDatosPropietario.FindControl("datose")).Text = datosCliente.Tables[0].Rows[0][5].ToString();
					((TextBox)controlDatosPropietario.FindControl("datosf")).Text = datosCliente.Tables[0].Rows[0][6].ToString();
					((TextBox)controlDatosPropietario.FindControl("datosg")).Text = datosCliente.Tables[0].Rows[0][7].ToString();
					((TextBox)controlDatosPropietario.FindControl("datosh")).Text = datosCliente.Tables[0].Rows[0][8].ToString();
					((TextBox)controlDatosPropietario.FindControl("datos")).ReadOnly = true;
					((TextBox)controlDatosPropietario.FindControl("datosa")).ReadOnly = true;
					((TextBox)controlDatosPropietario.FindControl("datosb")).ReadOnly = true;
					//((Button)controlDatosPropietario.FindControl("confirmar")).Enabled = true;
					//Aqui cargamos los datos del vehiculo  en el control respectivo del vehiculo
					DataSet datosVehiculo = new DataSet();
					DBFunctions.Request(datosVehiculo,IncludeSchema.NO,"SELECT pcat_codigo, mcat_vin, mcat_motor, mcat_serie, mcat_anomode, pcol_codigo, tser_tiposerv, mcat_concvend, mcat_venta, mcat_numeradio, mcat_numekilovent, mcat_numeultikilo, mcat_numekiloprom, mcat_vencseguobli FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'");
					//                                                            0            1           2           3             4           5                6       7              8                 9                10               11                   12            13
					((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text = datosVehiculo.Tables[0].Rows[0][1].ToString();
					((TextBox)controlDatosVehiculo.FindControl("identificacion")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("motor")).Text = datosVehiculo.Tables[0].Rows[0][2].ToString();
					((TextBox)controlDatosVehiculo.FindControl("motor")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("serie")).Text = datosVehiculo.Tables[0].Rows[0][3].ToString();
					((TextBox)controlDatosVehiculo.FindControl("serie")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("anoModelo")).Text = datosVehiculo.Tables[0].Rows[0][4].ToString();
					((TextBox)controlDatosVehiculo.FindControl("anoModelo")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("consVendedor")).Text = datosVehiculo.Tables[0].Rows[0][7].ToString();
					((TextBox)controlDatosVehiculo.FindControl("consVendedor")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("fechaCompra")).Text = System.Convert.ToDateTime(datosVehiculo.Tables[0].Rows[0][8].ToString()).ToString("yyyy-MM-dd");
					((TextBox)controlDatosVehiculo.FindControl("fechaCompra")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("codRadio")).Text = datosVehiculo.Tables[0].Rows[0][9].ToString();
					((TextBox)controlDatosVehiculo.FindControl("kilometraje")).Text = Convert.ToDouble(datosVehiculo.Tables[0].Rows[0][11].ToString()).ToString("n");
					((TextBox)controlDatosVehiculo.FindControl("kilometrajeCompra")).Text = datosVehiculo.Tables[0].Rows[0][10].ToString();
					((TextBox)controlDatosVehiculo.FindControl("kilometrajeCompra")).Enabled = false;
					//DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosVehiculo.FindControl("modelo")),datosVehiculo.Tables[0].Rows[0][0].ToString().Trim() +" - "+DBFunctions.SingleData("SELECT pcat_descripcion FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo='"+datosVehiculo.Tables[0].Rows[0][0].ToString().Trim()+"'"));
		// AYCO		((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue = datosVehiculo.Tables[0].Rows[0][0].ToString().Trim();
					((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue = datosVehiculo.Tables[0].Rows[0][0].ToString();
					//((DropDownList)controlDatosVehiculo.FindControl("modelo")).SelectedValue = datosVehiculo.Tables[0].Rows[0][0].ToString();
					//DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosVehiculo.FindControl("modelo")),datosVehiculo.Tables[0].Rows[0][0].ToString() +" - "+DBFunctions.SingleData("SELECT pcat_descripcion FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo='"+datosVehiculo.Tables[0].Rows[0][0].ToString()+"'"));
					((DropDownList)controlDatosVehiculo.FindControl("modelo")).Enabled = false;
                    controlDatosVehiculo.FindControl("imglupaCatalogo").Visible = false;
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosVehiculo.FindControl("color")),DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo='"+datosVehiculo.Tables[0].Rows[0][5].ToString()+"'").Trim());
					((DropDownList)controlDatosVehiculo.FindControl("color")).Enabled = false;
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)controlDatosVehiculo.FindControl("tipo")),DBFunctions.SingleData("SELECT tser_nombserv FROM tserviciovehiculo WHERE tser_tiposerv='"+datosVehiculo.Tables[0].Rows[0][6].ToString()+"'").Trim());
					((DropDownList)controlDatosVehiculo.FindControl("tipo")).Enabled = false;						
					//((Button)controlDatosVehiculo.FindControl("confirmar")).Enabled = true;
					//((ImageButton)principal.FindControl("otros")).Enabled = true;
					//((ImageButton)principal.FindControl("botonKits")).Enabled = true;
					((TextBox)controlKitsCombos.FindControl("valToInsertar1EX")).Text = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo='"+datosVehiculo.Tables[0].Rows[0][0].ToString()+"'");
					Session["kitsTabla"] = null;
				}
				else
				{
					//En caso que no exista el automovil que entra al taller
					//((Button)controlDatosPropietario.FindControl("confirmar")).Enabled = true;
					((Button)controlDatosVehiculo.FindControl("confirmar")).Enabled = true;
					//Limpiamos los controles de usuario de propietario y de vehiculos
					((TextBox)controlDatosPropietario.FindControl("datos")).Text = "";
					((TextBox)controlDatosPropietario.FindControl("datos")).ReadOnly=false;
					((TextBox)controlDatosPropietario.FindControl("datosa")).Text = "";
					((TextBox)controlDatosPropietario.FindControl("datosb")).Text = "";
					((TextBox)controlDatosPropietario.FindControl("datosc")).Text = "";
					((TextBox)controlDatosPropietario.FindControl("datose")).Text = "";
					((TextBox)controlDatosPropietario.FindControl("datosg")).Text = "";
					//((Button)controlDatosPropietario.FindControl("confirmar")).Enabled = true;
					////////////////////////////////////////////////////////////////////
					((TextBox)controlDatosVehiculo.FindControl("identificacion")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("motor")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("serie")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("anoModelo")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("consVendedor")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("fechaCompra")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("codRadio")).Text = "";
					((TextBox)controlDatosVehiculo.FindControl("kilometrajeCompra")).Text = "";
					((DropDownList)controlDatosVehiculo.FindControl("modelo")).Enabled = true;
                    controlDatosVehiculo.FindControl("imglupaCatalogo").Visible = true;
					((DropDownList)controlDatosVehiculo.FindControl("color")).Enabled = true;
					((DropDownList)controlDatosVehiculo.FindControl("tipo")).Enabled = true;						
					//((Button)controlDatosVehiculo.FindControl("confirmar")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("identificacion")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("motor")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("serie")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("anoModelo")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("consVendedor")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("fechaCompra")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("codRadio")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("kilometrajeCompra")).Enabled = true;
					//((TextBox)controlDatosVehiculo.FindControl("clave")).Enabled = true;
					/////////////////////////////////////////////////////////////////////////
					/// se colocan los datos botones que manejan el control de usuario de los 
					///  otros datos del carro y de los kits en falso
					//((ImageButton)principal.FindControl("otros")).Enabled = false;
					//((ImageButton)principal.FindControl("botonKits")).Enabled = false;
				}
				if(cargo.SelectedValue == "S")
				{
					//Activo los TextBox Correspondientes a Datos Aseguradora
					((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("siniestro")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("nitCompania")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionGarant")).Enabled = false;
				}
				else if(cargo.SelectedValue == "G")
				{
					//Activo los TextBox Correspondiente a los datos de la garantia
					((TextBox)controlDatosVehiculo.FindControl("nitCompania")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionGarant")).Enabled = true;
					((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("siniestro")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Enabled = false;
				}
				else
				{
					((TextBox)controlDatosVehiculo.FindControl("nitCompania")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionGarant")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("nitAseguradora")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("siniestro")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("porcentajeDeducible")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("valorMinDeducible")).Enabled = false;
					((TextBox)controlDatosVehiculo.FindControl("numeroAutorizacionAsegura")).Enabled = false;
				}	
				//((PlaceHolder)this.Parent).Visible = false;
				//((PlaceHolder)principal.FindControl("datosPropietario")).Visible = true;
                
                //((ImageButton)principal.FindControl("origen")).ImageUrl="../img/AMS.BotonExpandir.png";
                //((ImageButton)principal.FindControl("propietario")).Enabled = true;
                //((ImageButton)principal.FindControl("propietario")).ImageUrl="../img/AMS.BotonContraer.png";
				//((ImageButton)principal.FindControl("vehiculo")).Enabled = true;
				//En caso que nuestro cliente venga por un peritaje, debemos entonces cargar en la grilla de operaciones la operacion de peritaje y desahibilitar los otros controles
				if(servicio.SelectedValue == "P")
				{
                   // ((DataGrid)controlKitsCombos.FindControl("kitsCompletos")).Enabled = false;
                    //((DataGrid)controlKitsCombos.FindControl("kitsItems")).Enabled = false;
                    //((DataGrid)controlKitsCombos.FindControl("kitsItems")).ShowFooter = false;
					//Ahora creamos una tabla que me permita almacenar la operacion de peritaje
					string codigoOperacionPeritaje = DBFunctions.SingleData("SELECT ptem_operacion FROM ctaller");
					DataTable tablaOperaciones = new DataTable();
					tablaOperaciones = this.Preparar_Tabla_Operaciones(tablaOperaciones);
					//Creamos la Fila
					DataRow fila = tablaOperaciones.NewRow();
					fila["CODIGOOPERACION"] = codigoOperacionPeritaje;
					fila["NOMBRE"] = DBFunctions.SingleData("SELECT ptem_descripcion FROM ptempario WHERE ptem_operacion='"+codigoOperacionPeritaje+"'");
					fila["TIEMPOEST"] = 0;
					string excencionIva = DBFunctions.SingleData("SELECT ptem_exceiva FROM ptempario WHERE ptem_operacion='"+codigoOperacionPeritaje+"'");
					if(excencionIva=="S")
						fila["EXCENTOIVA"] = "SI";
					else if(excencionIva=="N")
						fila["EXCENTOIVA"] = "NO";
					fila["MECANICO"] = this.Buscar_Mecanico_Peritaje();
					fila["CARGO"] = cargo.SelectedItem.Text;
                    fila["ESTADO"] = DBFunctions.SingleData("SELECT test_nombre FROM testadooperacion WHERE test_estaOPER = 'A'");
                    fila["VALOROPERACION"] = DBFunctions.SingleData("SELECT ptem_valooper FROM ptempario WHERE ptem_operacion='" + codigoOperacionPeritaje + "'");

					tablaOperaciones.Rows.Add(fila);
					Session["operaciones"] = tablaOperaciones;
					//Final de la creacion de la fila
					//Ahora asociamos la tabla a la grilla
                    //((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")).ShowFooter = false;
					((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")).DataSource = tablaOperaciones;
					((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")).DataBind();
					//Por ahora suponemos que la operacion de peritaje siempre va a ser de tipo Bono
					((TextBox)((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")).Items[0].Cells[5].Controls[1]).Visible = true;
                    //((Button)((DataGrid)controlKitsCombos.FindControl("kitsOperaciones")).Items[0].Cells[14].Controls[1]).Enabled = false;
					//Ahora Activamos el Boton de Confirmacion y agregamos datos a los TextBox de fecha y hora
					((Button)controlKitsCombos.FindControl("confirmar")).Enabled = true;
					((TextBox)controlKitsCombos.FindControl("fechaEstimada")).Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
					((TextBox)controlKitsCombos.FindControl("horaEstimada")).Text = (DateTime.Now.TimeOfDay.ToString()).Substring(0,5);
				}
				//Modificacion hecha el 2005-06-01
				//Se bloqueara el acceso a esta pantalla, para evitar que los usuarios realicen cambios al cargo de la orden, y por lo tanto
				//Se vean afectados los cargos de las operaciones o items
				//((ImageButton)principal.FindControl("origen")).Enabled = false;



				// Se reinician las grillas de kits combos
				KitsCombos inst = (KitsCombos)((PlaceHolder)principal.FindControl("kitsCombos")).Controls[0];
				inst.InicializarGrillas();
                Revision_Cita();
                HiddenField hdTabIndex = ((HiddenField)principal.FindControl("hdTabIndex"));
                hdTabIndex.Value = "1";
			}
			else
                Utils.MostrarAlerta(Response, "Clave Recepcionista Invalida");
				
		}
		
		#endregion
		
		#region Metodos
        
		protected void Revision_Cita()
		{
			Control principal = (this.Parent).Parent; //Control principal ascx
			string revision = "";
			DateTime fechaHoraActual = DateTime.Now;
            obserCita = "";
			if(DBFunctions.RecordExist("SELECT mcit_placa FROM mcitataller WHERE mcit_placa='"+placa.Text+"' AND mcit_fecha='"+fechaHoraActual.Date.ToString("yyyy-MM-dd")+"'"))
			{
				string valorGrupo = String.Empty;
				if(DBFunctions.RecordExist("SELECT mcat_placa FROM mcatalogovehiculo WHERE mcat_placa='"+placa.Text+"'"))
					valorGrupo = DBFunctions.SingleData("SELECT PCA.pgru_grupo FROM pcatalogovehiculo PCA INNER JOIN mcatalogovehiculo MCA ON PCA.pcat_codigo = MCA.pcat_codigo WHERE mcat_placa = '"+placa.Text+"'");
				else
					valorGrupo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo WHERE pcat_codigo = (SELECT pcat_codigo FROM mcitataller WHERE mcit_placa='"+placa.Text+"' AND mcit_fecha='"+fechaHoraActual.Date.ToString("yyyy-MM-dd")+"')");
				((KitsCombos)(principal.FindControl("kitsCombos")).Controls[0]).Distribuir_Kit(DBFunctions.SingleData("SELECT coalesce(pkit_codigo,'') FROM mcitataller WHERE mcit_placa='"+placa.Text+"' AND mcit_fecha='"+fechaHoraActual.Date.ToString("yyyy-MM-dd")+"'"),cargo.SelectedValue,listaPrecios.SelectedValue,valorGrupo);
				TimeSpan horaCita = (Convert.ToDateTime(fechaHoraActual.Date.ToString("yyyy-MM-dd")+" "+DBFunctions.SingleData("SELECT mcit_hora FROM mcitataller WHERE mcit_placa='"+placa.Text+"' AND mcit_fecha='"+fechaHoraActual.Date.ToString("yyyy-MM-dd")+"'"))).TimeOfDay;
				TimeSpan horaActual = fechaHoraActual.TimeOfDay;
                obserCita = DBFunctions.SingleData(("SELECT mcit_observacion FROM mcitataller WHERE mcit_placa='" + placa.Text + "' AND mcit_fecha='" + fechaHoraActual.Date.ToString("yyyy-MM-dd") + "'"));
                if (obsrCliente.Text == "")
                    obsrCliente.Text = obserCita;
                if((horaActual<horaCita)||(horaActual<(horaCita.Add(new TimeSpan(1,0,0)))))
				{
					revision = "Con Cita Cumplida";
					lbEstCita.Text = "C";
					DBFunctions.NonQuery("UPDATE mcitataller SET testcit_estacita='C' WHERE mcit_placa='"+placa.Text+"' AND mcit_fecha='"+fechaHoraActual.Date.ToString("yyyy-MM-dd")+"'");
				}
				else if(horaActual>=(horaCita.Add(new TimeSpan(1,0,0))))
				{
					lbEstCita.Text = "R";
					revision = "Con Retraso en su Cita";
					DBFunctions.NonQuery("UPDATE mcitataller SET testcit_estacita='R' WHERE mcit_placa='"+placa.Text+"' AND mcit_fecha='"+fechaHoraActual.Date.ToString("yyyy-MM-dd")+"'");
				}
			}
			else
			{
				lbEstCita.Text = "I";
				revision = "Sin Cita";
			}
            Utils.MostrarAlerta(Response, "Este Cliente Viene " + revision + "");
		}
		
		protected DataTable Preparar_Tabla_Items(DataTable items)
		{
			items = new DataTable();
			items.Columns.Add(new DataColumn("CODIGOKIT",System.Type.GetType("System.String")));
			items.Columns.Add(new DataColumn("CODIGOREPUESTO",System.Type.GetType("System.String")));
			items.Columns.Add(new DataColumn("REFERENCIA",System.Type.GetType("System.String")));
			items.Columns.Add(new DataColumn("ORIGEN",System.Type.GetType("System.String")));
			items.Columns.Add(new DataColumn("PRECIO",System.Type.GetType("System.Double")));
			items.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));
			items.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			items.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));
			items.Columns.Add(new DataColumn("VALORFACTURADO",System.Type.GetType("System.Double")));
			items.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));
			return items;
		}
		
		protected DataTable Preparar_Tabla_Operaciones(DataTable operaciones)
		{
			operaciones = new DataTable();
			operaciones.Columns.Add(new DataColumn("CODIGOKIT",System.Type.GetType("System.String")));//0
			operaciones.Columns.Add(new DataColumn("CODIGOOPERACION",System.Type.GetType("System.String")));//1
			operaciones.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));//2
			operaciones.Columns.Add(new DataColumn("TIEMPOEST",System.Type.GetType("System.Double")));//3
			operaciones.Columns.Add(new DataColumn("VALOROPERACION",System.Type.GetType("System.Double")));//4
			operaciones.Columns.Add(new DataColumn("EXCENTOIVA",System.Type.GetType("System.String")));//5
			operaciones.Columns.Add(new DataColumn("MECANICO",System.Type.GetType("System.String")));//6
			operaciones.Columns.Add(new DataColumn("CARGO",System.Type.GetType("System.String")));//7
			operaciones.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));//8
			operaciones.Columns.Add(new DataColumn("CODIGOINCIDENTE",System.Type.GetType("System.String")));//9
			operaciones.Columns.Add(new DataColumn("CODIGOGARANTIA",System.Type.GetType("System.String")));//10
			operaciones.Columns.Add(new DataColumn("CODIGOREMEDIO",System.Type.GetType("System.String")));//11
			operaciones.Columns.Add(new DataColumn("CODIGODEFECTO",System.Type.GetType("System.String")));//12
			return operaciones;
		}
		
		protected string Buscar_Mecanico_Peritaje()
		{
			string mecanico = "";
			string taller = almacen.SelectedValue;
			double cantidadHoras = 1000;
			DataSet mecanicos = new DataSet();
            DBFunctions.Request(mecanicos, IncludeSchema.NO, @"SELECT PV.pven_codigo, PV.pven_nombre  
                                                             FROM pvendedor PV  
                                                             INNER JOIN pvendedoralmacen PVA ON PVA.pven_codigo = PV.pven_codigo   
                                                             WHERE PV.tvend_codigo = 'MG'  
                                                             AND   PVA.palm_almacen = '" + taller + "'");
			for(int i=0;i<mecanicos.Tables[0].Rows.Count;i++)
			{
				DataSet operaciones = new DataSet();
                DBFunctions.Request(operaciones, IncludeSchema.NO, "SELECT DOR.ptem_operacion, Mc.pcat_codigo FROM dordenoperacion DOR, morden MO, mcatalogovehiculo mc WHERE DOR.pven_codigo='" + mecanicos.Tables[0].Rows[i].ItemArray[0].ToString() + "' AND DOR.test_estado='A' AND MO.mord_numeorde = DOR.mord_numeorde AND MO.test_estado='A' and mo.mcat_vin = mc.mcat_vin");
				if(operaciones.Tables[0].Rows.Count==0)
				{
					cantidadHoras = 0;
					mecanico = mecanicos.Tables[0].Rows[i].ItemArray[1].ToString();
				}
				else
				{
					double horasMecanicoAsignadas = 0;
					for(int j=0;j<operaciones.Tables[0].Rows.Count;j++)
					{
						string grupoCatalogo = DBFunctions.SingleData("SELECT pgru_grupo FROM pcatalogovehiculo where pcat_codigo='"+operaciones.Tables[0].Rows[j].ItemArray[1].ToString()+"'");
						if(DBFunctions.RecordExist("SELECT * FROM ptiempotaller WHERE ptie_tempario='"+operaciones.Tables[0].Rows[j].ItemArray[0].ToString()+"' AND ptie_grupcata='"+grupoCatalogo+"'"))
							horasMecanicoAsignadas += System.Convert.ToDouble(DBFunctions.SingleData("SELECT ptie_tiemclie FROM ptiempotaller WHERE ptie_tempario='"+operaciones.Tables[0].Rows[j].ItemArray[0].ToString()+"' AND ptie_grupcata='"+grupoCatalogo+"'"));
					}
					if(horasMecanicoAsignadas<cantidadHoras)
					{
						mecanico = mecanicos.Tables[0].Rows[i].ItemArray[1].ToString();
					}
				}
			}
			return mecanico;
		}
		
		protected string RevisionParametros()
		{
			string error = "";
			if(almacen.Items.Count == 0 || almacen.SelectedValue == "0")
				error += "No se ha seleccionado ningun Taller para realizar el proceso.\\n ";
			if(tipoDocumento.Items.Count == 0 || tipoDocumento.SelectedValue == "0")
				error += "No se ha seleccionado un Prefijo para el documento de Orden de trabajo.\\n ";
            if (placa.Text == "")
                error += "Por favor ingrese una Placa.\\n ";
            if (cargo.Items.Count == 0 || cargo.SelectedValue == "0")
                error += "No se ha seleccionado ningun Cargo para realizar el proceso.\\n ";
            if (servicio.Items.Count == 0 || servicio.SelectedValue == "0")
                error += "No se ha seleccionado un Servicio para el documento de orden de trabajo.\\n ";
            if (listaPrecios.Items.Count == 0 || listaPrecios.SelectedValue == "0")
                error += "No se ha seleccionado Lista de Precios para operaciones de taller.\\n ";

            //          if (pideRepuestos) //si pide los repuestos desde la creación de la orden de trabajo
            {
                if (listaPreciosItems.Items.Count == 0 || listaPreciosItems.SelectedValue == "0")
                    error += "No se ha seleccionado Lista de Precios para los Repuestos fuera de los kits.\\n ";
                if (nitTransferencias.Items.Count == 0 || nitTransferencias.SelectedValue == "0")
                    error += "No se ha seleccionado un Nit para las transferencias de repuestos a realizar.\\n ";
                if (tipoPedido.Items.Count == 0 || tipoPedido.SelectedValue == "0")
                    error += "No se ha seleccionado un Tipo de Pedido para los repuestos a solicitar.\\n ";
                if (prefijoTransferencia.Items.Count == 0 || prefijoTransferencia.SelectedValue == "0")
                    error += "No se ha seleccionado un Prefijo para la Transferencia a taller a realizar.\\n ";
                if (almacenTransferencia.Items.Count == 0 || almacenTransferencia.SelectedValue == "0")
                    error += "No se ha seleccionado un Almacén para realizar la solicitud de Repuestos.\\n ";
            }
            if (codigoRecep.Items.Count == 0 || codigoRecep.SelectedValue == "0")
                error += "No se ha seleccionado un Recepcionista .\\n";
			if(Revision_Causales()!="")
				error += Revision_Causales();
			return error;
		}

		protected string Revision_Causales()
		{
			bool revInc=false,revCau=false,revRem=false,revDef=false;
			string errorGarantia="";
			if(cargo.SelectedValue=="G")
			{
				revInc = DBFunctions.RecordExist("SELECT * FROM pgarantia WHERE tcau_codigo='I' ORDER BY pgar_descripcion");
                revCau = DBFunctions.RecordExist("SELECT * FROM pgarantia WHERE tcau_codigo='C' ORDER BY pgar_descripcion");
                revRem = DBFunctions.RecordExist("SELECT * FROM pgarantia WHERE tcau_codigo='R' ORDER BY pgar_descripcion");
                revDef = DBFunctions.RecordExist("SELECT * FROM pgarantia WHERE tcau_codigo='D' ORDER BY pgar_descripcion");
				if(!revInc)
					errorGarantia+="No ha definido incidentes para la garantia. Revise su parametrización \\n";
				if(!revCau)
					errorGarantia+="No ha definido causales para la garantia. Revise su parametrización \\n";
				if(!revRem)
					errorGarantia+="No ha definido remedios para la garantia. Revise su parametrización \\n";
				if(!revDef)
					errorGarantia+="No ha definido defectos para la garantia. Revise su parametrización \\n";
			}
			return errorGarantia;
		}

        //[Ajax.AjaxMethod()]
        //public DataSet CambioTallerCarga(string idAlmacen)
        //{
        //    DataSet dsConsulta = new DataSet();
        //    DBFunctions.Request(dsConsulta,IncludeSchema.NO,"SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo='RT' AND palm_almacen = '"+idAlmacen+"' ORDER BY pven_nombre ASC;"+
        //        "SELECT pnital_nittaller FROM pnittaller WHERE palm_almacen='"+idAlmacen+"'");
        //    //dsConsulta.Tables[0].Columns[0].ColumnName = ""
        //    return dsConsulta;
        //}

		private void RevisionValoresDependientesTaller()
		{
            string valueDDLRecepcionista = Request.Form[codigoRecep.ClientID];
            string valueDDLNits = Request.Form[nitTransferencias.ClientID];
            string valueDDLAlmacen = Request.Form[almacen.ClientID];
			if(valueDDLAlmacen != null)
			{
                Utils.FillDll(codigoRecep, @"SELECT pv.pven_codigo,  
                                                    pv.pven_nombre  
                                             FROM pvendedor pv  
                                             INNER JOIN pvendedoralmacen pva on pv.pven_codigo = pva.pven_codigo 
                                             WHERE (pv.tvend_codigo = 'RT' OR pv.tvend_codigo = 'RA')  
                                             AND   pv.pven_vigencia = 'V'  
                                             AND   pva.palm_almacen = '" + almacen.SelectedValue + @"'  
                                             ORDER BY pven_nombre ASC", true);

                Utils.FillDll(nitTransferencias, "SELECT pnital_nittaller FROM pnittaller WHERE palm_almacen='" + valueDDLAlmacen + "'", false);
				codigoRecep.SelectedValue = valueDDLRecepcionista;
				nitTransferencias.SelectedValue = valueDDLNits;
            }
		}

		private bool ExistenOrdenes(string placa,ref string  ordenes)
		{
			bool existe=false;
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT pdoc_codigo,mord_numeorde, CAST(mord_entrada AS CHAR(10)), TCAR_NOMBRE
                        FROM  DBXSCHEMA.MORDEN MO, DBXSCHEMA.MCATALOGOVEHICULO MV, TCARGOORDEN TC
                        WHERE MO.MCAT_VIN = MV.MCAT_VIN AND MCAT_PLACA = '" + placa + "' AND MO.TCAR_CARGO = TC.TCAR_CARGO AND test_estado='A';");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                 existe = true;
                 for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                     ordenes += ds.Tables[0].Rows[i][0].ToString() + "-" + ds.Tables[0].Rows[i][1].ToString() + "  de  " + ds.Tables[0].Rows[i][2].ToString() + "  con cargo  " + ds.Tables[0].Rows[i][3].ToString() + "\\n";
            }
			return existe;
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
