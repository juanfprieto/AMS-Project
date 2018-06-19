// created on 05/02/2005 at 11:03
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.Contabilidad;
using AMS.Tools;


namespace AMS.Vehiculos
{
	public partial class FacturacionPedido : System.Web.UI.UserControl
	{
		#region Atributos
		//Elementos de Datos Sobre la Factura
		//***********************************
		//Elementos de Datos Pedido y Vehiculo
		//***********************************
		//Elementos de Datos de Venta 
		protected DataTable tablaElementos;
		//***********************************
		//Elementos de Datos de Pagos
		protected DataTable tablaAnticipos;
		//***********************************
		//Elementos de Retoma
		protected DataTable tablaRetoma, tablaRetomaGrabacion;
		protected string claseVehiculoInd, esCasaMatriz="", tipoFactura ="";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string ConString, tipovehiculo, fechaFactura,	tem = "";
		protected string path=ConfigurationManager.AppSettings["PathToPreviews"];
		protected bool prefijoFactErrado = false;
        protected ValoresImpuestosVehiculos impuestoVehFac;
        protected bool hacerNota = false;
        protected double valorNota = 0, costoCompra = 0, valorPerdida = 0;
        

        ProceHecEco contaOnline = new ProceHecEco();
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack)
			{
				//Session.Clear();
				DatasToControls bind = new DatasToControls();
				string Sql = "Select b.tcla_codigo from MASIGNACIONVEHICULO a,mvehiculo b where a.MVEH_INVENTARIO = b.MVEH_INVENTARIO and";
					
				Sql = Sql + " a.MPED_NUMEPEDI = " + Request.QueryString["numP"] + " and a.pdoc_codigo = '" + Request.QueryString["prefP"] + "' ";
				tipovehiculo = DBFunctions.SingleData(Sql);
			
				if (tipovehiculo == "N") 
					tem = "VN";
				else 
					tem = "VU";

                //Sql = String.Format(Documento.DOCUMENTOSTIPOPROCESO,"FC",tem);
                tipoFactura = DBFunctions.SingleData("SELECT CASE WHEN MPV.MNIT_NIT = CE.MNIT_NIT THEN 'CI' ELSE 'FC' END FROM MPEDIDOVEHICULO MPV, CEMPRESA CE WHERE MPV.PDOC_CODIGO = '" + Request.QueryString["prefP"] + "' and mpv.mped_numepedi = " + Request.QueryString["numP"] + "; ");
                Sql = @"SELECT distinct PD.PDOC_CODIGO, PD.PDOC_CODIGO || ' - ' || PD.PDOC_NOMBRE
                    FROM PDOCUMENTO PD, MPEDIDOVEHICULO MPV, PDOCUMENTOHEChO PH, PVENDEDORALMACEN PV
                    WHERE MPV.PVEN_CODIGO = PV.PVEN_CODIGO AND Ph.PDOC_CODIGO = PD.PDOC_CODIGO AND PV.PALM_ALMACEN = PH.PALM_ALMACEN
                    and (pH.tpro_proceso = 'VN' AND MPV.TCLA_CLASE = 'N' OR pH.tpro_proceso = 'VU' AND MPV.TCLA_CLASE = 'U')
                    AND PD.TDOC_TIPODOCU = '"+ tipoFactura + @"'
                    AND MPV.PDOC_CODIGO = '"+ Request.QueryString["prefP"] + @"' and mpv.mped_numepedi = "+ Request.QueryString["numP"] + @"; ";
				bind.PutDatasIntoDropDownList(prefijoFactura,Sql);

                string detFactura = "Factura Cliente  FC";
                ViewState["tipoFactura"] = tipoFactura;
                if (tipoFactura == "CI")
                {
                    detFactura = "Consumo Interno  CI";
                    Utils.MostrarAlerta(Response, "Este pedido se registrará como CONSUMO INTERNO Retiro de vehículos para ACTIVO FIJO. Por favor, asegurese que esta selección es la correcta y entregar a contabilidad el soporte para completar la ficha del Activo .. ");
                }

                if (prefijoFactura.Items.Count > 1)
                {
                    prefijoFactura.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                }
                else if (prefijoFactura.Items.Count == 0)
                     {
                        Utils.MostrarAlerta(Response, "No hay documentos del tipo "  + detFactura + " para el proceso " + tem);
                     }

				plInfoPedVeh.Visible = plInfoVenta.Visible = plInfoPagos.Visible = plInfoRetoma.Visible = false;
				numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue.ToString()+"'");
                txtFechaFac.Text = DateTime.Now.ToString("yyyy-MM-dd");
                var tieneFacturaAdmin = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");
                if (tieneFacturaAdmin == "S" && Session["tablaElementos"] != null)
                {

                    string Sqla = String.Format(Documento.DOCUMENTOSTIPOPROCESO, "FC", "VA");
                    try
                    {
                        bind.PutDatasIntoDropDownList(ddlPrefijoFacAcce, Sqla);
                        if (ddlPrefijoFacAcce.Items.Count > 1)
                        {
                            ddlPrefijoFacAcce.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                        }
                        txtNumeroFacAcce.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefijoFacAcce.SelectedValue.ToString() + "'");
                    }
                    catch
                    {
                        Utils.MostrarAlerta(Response, "Se deben parametrizar los prefijos relacionados al tipo de factura para accesorios!");
                    }
                }
              
                Cargar_Datos_Pedido(Request.QueryString["prefP"],Request.QueryString["numP"]);
				Cargar_Datos_Automovil_Asignado(Request.QueryString["prefP"],Request.QueryString["numP"]);
				Cargar_Otros_Datos_Pedido(Request.QueryString["prefP"],Request.QueryString["numP"]);
				Cargar_Datos_Pago(Request.QueryString["prefP"],Request.QueryString["numP"]);
				observaciones.Text = DBFunctions.SingleData(
                   "SELECT CASE WHEN MV.TCLA_CODIGO = 'N' THEN 'Catálogo: ' CONCAT PC.PCAT_DESCRIPCION CONCAT ' VIN: ' CONCAT MC.MCAT_VIN CONCAT ' Motor: ' CONCAT MCAT_MOTOR " +
                   "else 'Catálogo: ' CONCAT PC.PCAT_DESCRIPCION CONCAT ' VIN: ' CONCAT MC.MCAT_VIN CONCAT ' Motor: ' CONCAT MCAT_MOTOR concat ' Placa: ' concat MC.MCAT_PLACA END case " + 
                   "FROM MCATALOGOVEHICULO MC " +
                    "LEFT JOIN PCATALOGOVEHICULO PC ON MC.PCAT_CODIGO = PC.PCAT_CODIGO, " +
                    "     MASIGNACIONVEHICULO MA, " +
                    "     MVEHICULO MV " +
                    "WHERE MA.PDOC_CODIGO = '" + Request.QueryString["prefP"] + "' " +
                    "AND   MA.MPED_NUMEPEDI = " + Request.QueryString["numP"] + " " +
                    "AND   MA.MVEH_INVENTARIO = MV.MVEH_INVENTARIO " +
                    "AND   MC.MCAT_VIN = MV.MCAT_VIN");
                observaciones.Text += DBFunctions.SingleData("SELECT ' Detalle: ' || PO.POPC_NOMBOPCI FROM DBXSCHEMA.MPEDIDOVEHICULO MP, DBXSCHEMA.POPCIONVEHICULO PO WHERE MP.POPC_OPCIVEHI = PO.POPC_OPCIVEHI AND PDOC_CODIGO = '" + Request.QueryString["prefP"] + "' AND MPED_NUMEPEDI = " + Request.QueryString["numP"]);
                
			}
			else
			{
				if(Session["tablaElementos"] == null)
					Preparar_Tabla_Elementos_Venta();
				else if(Session["tablaElementos"] != null)
					    tablaElementos = (DataTable)Session["tablaElementos"];
				if(Session["tablaAnticipos"] == null)
					Preparar_Tabla_Anticipos();
				else if(Session["tablaAnticipos"] != null)
					    tablaAnticipos = (DataTable)Session["tablaAnticipos"];
				if(Session["tablaRetoma"]==null)
					this.Preparar_Tabla_Retoma();
				else if(Session["tablaRetoma"]!=null)
					    tablaRetoma = (DataTable)Session["tablaRetoma"];
			}
		}
		
		protected void Page_PreRender(object sender, System.EventArgs e)
		{
			prefijoFactErrado = Convert.ToBoolean(ViewState["prefijoFactErrado"]);

			if (prefijoFactErrado)
			{
		   //     Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos");
				return;
			}
			else if (this.oprimioBotonAccion.Value=="true")
			{
				this.Facturar_Pedido(sender,e);
				this.oprimioBotonAccion.Value = "false";
			}
		}

		protected void Preparar_Tabla_Retoma_Grabacion()
		{
			tablaRetomaGrabacion = new DataTable();
			tablaRetomaGrabacion.Columns.Add(new DataColumn("TIPOVEHICULO",System.Type.GetType("System.String")));//0
			tablaRetomaGrabacion.Columns.Add(new DataColumn("COLORVEHICULO",System.Type.GetType("System.String")));//1
			tablaRetomaGrabacion.Columns.Add(new DataColumn("VALORUNITARIO",System.Type.GetType("System.String")));//2
			tablaRetomaGrabacion.Columns.Add(new DataColumn("NUMEROCONTRATO",System.Type.GetType("System.String")));//3
			tablaRetomaGrabacion.Columns.Add(new DataColumn("ANOMODELO",System.Type.GetType("System.String")));//4
			tablaRetomaGrabacion.Columns.Add(new DataColumn("PLACA",System.Type.GetType("System.String")));//5
			tablaRetomaGrabacion.Columns.Add(new DataColumn("CUENTAIMPUESTOS",System.Type.GetType("System.String")));//6
		}
		
		protected void Construir_Tabla_Vehiculos_Retoma()
		{
			Preparar_Tabla_Retoma_Grabacion();
			for(int i=0;i<grillaRetoma.Items.Count;i++)
			{
				DataRow fila = tablaRetomaGrabacion.NewRow();
				fila[0] = tablaRetoma.Rows[i][0].ToString();
				fila[1] = ((DropDownList)grillaRetoma.Items[i].Cells[4].Controls[1]).SelectedValue.ToString();
				fila[2] = tablaRetoma.Rows[i][5].ToString();
				fila[3] = tablaRetoma.Rows[i][1].ToString();
				fila[4] = tablaRetoma.Rows[i][2].ToString();
				fila[5] = tablaRetoma.Rows[i][3].ToString();
				fila[6] = tablaRetoma.Rows[i][4].ToString();
				tablaRetomaGrabacion.Rows.Add(fila);
			}
		}

        protected void Facturar_Pedido(Object Sender, EventArgs e)
		{
            DataSet dsCatalogoVehiculo = new DataSet();

            DBFunctions.Request(dsCatalogoVehiculo, IncludeSchema.NO,
                "SELECT mc.pcat_codigo, mc.tser_tiposerv FROM mvehiculo mv, mcatalogovehiculo mc WHERE mc.mcat_Vin = mv.mcat_vin and mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + Request.QueryString["prefP"] + "' AND mped_numepedi= " + Request.QueryString["numP"] + " )");

			prefijoFactErrado = Convert.ToBoolean(ViewState["prefijoFactErrado"]);
            valorNota = Convert.ToDouble( ViewState["valorNota"]);

			if (prefijoFactErrado)
			{
				Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos");
				return;
			}
			string Sql = @"Select b.tcla_codigo from MASIGNACIONVEHICULO a,mvehiculo b where a.MVEH_INVENTARIO = b.MVEH_INVENTARIO and  
			              a.MPED_NUMEPEDI = " + Request.QueryString["numP"] + " and a.pdoc_codigo = '" + Request.QueryString["prefP"] + "'";
			tipovehiculo = DBFunctions.SingleData(Sql);
			string SqlnI = @"Select a.MVEH_INVENTARIO from MASIGNACIONVEHICULO a,mvehiculo b where a.MVEH_INVENTARIO = b.MVEH_INVENTARIO and  
				            a.MPED_NUMEPEDI = " + Request.QueryString["numP"] + " and a.pdoc_codigo = '" + Request.QueryString["prefP"] + "'";
			int numeroInventario = Convert.ToInt32(DBFunctions.SingleData(SqlnI));

            string sqlEstadoVehiculo = "Select test_tipoesta from MVEHICULO WHERE MVEH_INVENTARIO = " + numeroInventario +" ";
            int estadoVehiculo = Convert.ToInt32(DBFunctions.SingleData(sqlEstadoVehiculo));
            if (estadoVehiculo != 30)  // se re-chequea que el vehiculo este asignado
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos"); 
                return;
            }
		
			string Prenda;
			int i=0;
			//Prenda
			if(chkPrenda.Checked)
				Prenda="S";
			else
				Prenda="N";
			//Observaciones contendra catalogo-vin-num motor mas lo que el usuario digito en el proceso.
			string observDig    = observaciones.Text;
            if (observDig.Length < 20) // Revisa si las observaciones fueron modificadas y le adiciona catálogo-vin-motor
			    observaciones.Text  += catalogo.Text+"-"+vin.Text+"-"+numeroMotor.Text+" ";
		//	observaciones.Text  +=observDig;
			string nombreFinanciera=DBFunctions.SingleData("Select mnit_apellidos from DBXSCHEMA.MNIT where mnit_nit='"+nitRealFinanciera.Text+"'");
			nitRealFinancieraa.Text = nombreFinanciera.ToString();
			//Aqui se lleva a cabo el proceso de liquidacion del pedido
			//Primero construimos la tabla con los datos de los vehiculo de retoma
			ArrayList sqlRefs   = new ArrayList();

			double valorAuto    = Math.Round(Convert.ToDouble(valorBaseVeh.Text.Substring(1)),0);
			double valorIvaAuto = Math.Round(Convert.ToDouble(valorIVAautomovil.Text.Substring(1)),0);
            var tieneFacturaAdmi = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");
            int[] registro = new int[grillaElementos.Items.Count];
            int modelador = 0;
            if (grillaElementos.Items.Count > 0)
            { 
                for (int j = grillaElementos.Items.Count -1; j >= 0; j--)
                {
                    if (((CheckBox)grillaElementos.Items[j].Cells[3].Controls[1]).Checked == false && tablaElementos.Rows[j][3].ToString() != "T" )//if (tieneFacturaAdmi == "N")
                    { tablaElementos.Rows[modelador].Delete(); }
                    else
                        modelador++;
                }
                tablaElementos.AcceptChanges();
            }


            //Pedidos.PedidoVehiculosCliente(Request.QueryString["prefP"], Convert.ToUInt32(Request.QueryString["numP"]), "C", tablaElementos, tablaAnticipos, ref sqlRefs, 1);
            Pedidos.PedidoVehiculosCliente(Request.QueryString["prefP"], Convert.ToUInt32(Request.QueryString["numP"]), 
				"C", numeroInventario, tablaElementos, tablaAnticipos, ref sqlRefs, 1,Prenda,valorAuto,valorIvaAuto,
                nombreFinanciera, nitPrincipal.Text, ddlPrefijoFacAcce.SelectedValue, txtNumeroFacAcce.Text);
            string codTramite = Pedidos.tramites[0];
            string numTramite = Pedidos.tramites[1];
            Construir_Tabla_Vehiculos_Retoma();
			if (prefijoFactura.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha configurado el prefijo para el tipo de documento Facturación Cliente en Documentos Asociados a Hechos Económicos para este Proceso !");
				return;
			}

            if (Convert.ToDouble(valorTotalRetoma.Text.Substring(1)) <= Convert.ToDouble(vlrVenta.Text.Substring(1)))
            {
                if ((Convert.ToDouble(vlrVenta.Text.Substring(1)) - Convert.ToDouble(valorTotalPagos.Text.Substring(1)) + valorNota) > 0)
                {
                    Utils.MostrarAlerta(Response, "Diferencia entre el valor de venta y el total de pagos");
                    return;
                }
            }
            string codAlmacen = ddlAlmacen.SelectedValue.ToString();
            //string codAlmacen   = DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and palm_descripcion='" + ddlAlmacen.SelectedItem.Text.Split('-')[2].Trim() + "'");
			string codCentroCosto = "";
			string prefijoFF    = "";
			string numeroFF     = "";
	 
			if (tipovehiculo    == "U")
                codCentroCosto  = DBFunctions.SingleData("SELECT pcen_centvvU FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codAlmacen + "'");
			else
                codCentroCosto  = DBFunctions.SingleData("SELECT pcen_centvvN FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codAlmacen + "'");

            double valorFactura = Math.Round(Convert.ToDouble(valorBaseVeh.Text.Substring(1).Trim()),0);
            double valorVenta   = Math.Round(Convert.ToDouble(vlrVenta.Text.Substring(1).Trim()), 0);
            double valorIVA     = Math.Round(Convert.ToDouble(valorIVAautomovil.Text.Substring(1).Trim()),0);

            double valorComisionUsado = 0;
            double valorIvaComisionUsado = 0;
            if (tipovehiculo == "U")
            {
                //  VALOR FACTURA CORRESPONDE A AL COSTO DE COMPRA DEL VEHICULO
                valorFactura = Convert.ToDouble(DBFunctions.SingleData("Select COALESCE(MVEH_VALOCOMP,0) from MVEHICULO WHERE MVEH_INVENTARIO = " + numeroInventario + "; "));

                //  se ADICIONA EL VALOR DE LOS GASTOS AL COSTO DEL VEHICULO
                try
                {
                    valorFactura += Convert.ToDouble(DBFunctions.SingleData("Select SUM(COALESCE(dfac_VALOrgasto,0)) from dfacturagastovehiculo df, pgastodirecto pg WHERE df.pgas_codigo = pg.pgas_codigo and pgas_indicostvehi = 'S' AND MVEH_INVENTARIO = " + numeroInventario + "; "));
                }
                catch (Exception er)
                { }

                //if (valorVenta < valorFactura)
                //    valorFactura = valorVenta; // el vehículo se vendió a perdida 
                try
                {
                    valorComisionUsado    = Math.Round(Convert.ToDouble(TxtInt.Text.Substring(1).Trim()), 0);
                    valorIvaComisionUsado = Math.Round(Convert.ToDouble(txtIva.Text.Substring(1).Trim()), 0);
                    valorIVA              = Math.Round(Convert.ToDouble(txtImpo.Text.Substring(1).Trim()), 0);
                }
                catch 
                {
                    valorComisionUsado = 0;   // REPETIDO LO DEBE TRAER DEL PROCESO INICIAL
                    valorIVA = 0;
                    valorIvaComisionUsado = 0;
                }
                  
                double ganancia = valorComisionUsado;
                //double anoModelo = Convert.ToDouble(DBFunctions.SingleData("Select mcat_anomode from MVEHICULO mv, McatalogoVEHICULO mc WHERE MVEH_INVENTARIO = " + numeroInventario + " and mv.mcat_vin = mc.mcat_vin "));
        
            }
            else
                codCentroCosto = DBFunctions.SingleData("SELECT pcen_centvvN FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codAlmacen + "'");
		
            //Validador ingreso de fecha
			if (txtFechaFac.Text==String.Empty)
			{
                Utils.MostrarAlerta(Response, "Por favor ingrese una fecha de factura.");
				return;
			}
			//Traer los dias de plazo fecha de vencimiento factura // del cabeto debe traer los dias de plazo
			string mfac_vence   =DBFunctions.SingleData("select ppre_diasplazo from dbxschema.PPRECIOVEHICULO where pcat_codigo='"+catalogo.Text+"'");
			if (mfac_vence  !=String.Empty)
				mfac_vence  = Convert.ToDateTime(txtFechaFac.Text).AddDays(Convert.ToDouble(mfac_vence)).ToString("yyyy-MM-dd");
			else
				mfac_vence  = Convert.ToDateTime(txtFechaFac.Text).ToString("yyyy-MM-dd");
			fechaFactura    = Convert.ToDateTime(txtFechaFac.Text).ToString("yyyy-MM-dd");
			if (DBFunctions.RecordExist("SELECT PDOC_FINFECHRESOFACT FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura.SelectedValue.ToString() + "' AND '" + fechaFactura + "' > PDOC_FINFECHRESOFACT "))
			{
                Utils.MostrarAlerta(Response, "La FECHA DE VENCIMIENTO de la facturación esta fuera de la autorización de la DIAN. Por favor informe a Contabilidad.");
				return;
			}

			if (DBFunctions.RecordExist("SELECT * fROM CVEHICULOS WHERE YEAR('" + fechaFactura + "') <> PANO_ANO OR MONTH('" + fechaFactura + "') <> PMES_MES"))
			{
                Utils.MostrarAlerta(Response, "La FECHA de FACTURACION esta fuera de la VIGENCIA de VEHICULOS. Por favor informe a Contabilidad.");
				return;
			}
            
            lb.Text = "<font size=\"15\" color=\"RED\"> Estoy facturando el vehículo, por favor espere que termine y luego imprima los FORMATOS</font>";

            valorPerdida = Convert.ToDouble(Session["valorPerdida"].ToString());
         	//Revisamos si es necesario crear la factura de financiera, se crea como una factura de cliente al nit de la financiera y se agrega esta factura como un pago a la factura de nuestro cliente
			FacturaCliente facturaVentaVehiculo = new FacturaCliente("FVV",prefijoFactura.SelectedValue, nitPrincipal.Text, codAlmacen , "F" ,Convert.ToUInt32(numeroFactura.Text.Trim()),
				0, Convert.ToDateTime(txtFechaFac.Text), Convert.ToDateTime(mfac_vence), DateTime.Now, (valorFactura - valorPerdida), valorIVA,
                valorComisionUsado, valorIvaComisionUsado, 0, valorPerdida, codCentroCosto, observaciones.Text, vendedor.SelectedValue, HttpContext.Current.User.Identity.Name,null);
			 
			//Agregamos los pagos relacionados con esta factura, lo abonos realizados 
			for(i=0;i<tablaAnticipos.Rows.Count;i++)
				facturaVentaVehiculo.AgregarPago(tablaAnticipos.Rows[i][0].ToString(), Convert.ToUInt32(tablaAnticipos.Rows[i][1]), Convert.ToDouble(tablaAnticipos.Rows[i][3]), "Anticipo A Pedido");

			//Revisamos si es necesario guardar la factura de financiera y si se tiene exito al realizar el proceso, simplemente la agregamos como pago a la factura del vehiculo
			double valorFinanciadoNum = 0;
			try{valorFinanciadoNum = Convert.ToDouble(valorFinanciado.Text.Trim());}
			catch{}
            /*if(valorFinanciadoNum > 0)
			{
				uint numeroFacturaFinaciera = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFacturaFinanciera.SelectedValue+"'"));
				if(prefijoFacturaFinanciera.SelectedValue == prefijoFactura.SelectedValue)
					numeroFacturaFinaciera += 1;
				FacturaCliente facturaFinanciera = new FacturaCliente("FFV",prefijoFacturaFinanciera.SelectedValue, nitRealFinanciera.Text, codAlmacen, "F", numeroFacturaFinaciera,
					0, Convert.ToDateTime(txtFechaFac.Text), DateTime.Now, DateTime.Now, valorFinanciadoNum,0, 
					0, 0, 0, 0, codCentroCosto,observaciones.Text+"-Factura de Financiera", vendedor.SelectedValue, HttpContext.Current.User.Identity.Name);
				//Llamamos la funcion de grabacion de factura
				//Agregamos la factura de la financiera como un pago de la factura de venta del vehiculo
				facturaFinanciera.GrabarFacturaCliente(false);
				facturaVentaVehiculo.AgregarPago(facturaFinanciera.PrefijoFactura, facturaFinanciera.NumeroFactura, facturaFinanciera.ValorFactura, "Factura de Financiera");
				for(i=0;i<facturaFinanciera.SqlStrings.Count;i++)
					sqlRefs.Add(facturaFinanciera.SqlStrings[i].ToString());
				prefijoFF = facturaFinanciera.PrefijoFactura;
				numeroFF = facturaFinanciera.NumeroFactura.ToString();
			}
			*/

            // insertamos en MACTIVOFIJO si es pedido de CONSUMO INTERNO, o sea, una factura al NIT de la empresa
            tipoFactura = ViewState["tipoFactura"].ToString() ;
            if (tipoFactura == "CI")
            {
                string fact = prefijoFactura.SelectedValue + Convert.ToUInt32(numeroFactura.Text.Trim());
                string desc = "Vin : " + vin.Text + " Motor :" + numeroMotor.Text;
                sqlRefs.Add(@"INSERT INTO mACTIVOFIJO (MAFJ_CODIACTI, MAFJ_DESCRIPCION, PCCO_CENTCOST, MAFJ_MARCA, MAFJ_MODELO, MAFJ_PLACA, MAFJ_FECHFACT, MAFJ_INGRESO, MAFJ_FECHINIC, MNIT_NIT, MAFJ_PEDIDO, MAFJ_VALOHIST, MAFJ_VALODOLA, MAFJ_VALOMEJORA, MAFJ_DEPRACUM, MAFJ_CUOTAS, TVIG_VIGENCIA, TDEP_TIPODEPR, mafj_valoactniif, mafj_valomejoraniif, mafj_cuotasniif, mafj_depracumniif, mafj_valoreciniif, mafj_valodeteniif) 
                                               VALUES ('" + fact + @"', '"+ desc + @"','" + codCentroCosto + @"','" + catalogoAsignado.Text + @"','" + anoModeloAsignado.Text + @"','" + fact + @"','" + txtFechaFac.Text + @"','" + txtFechaFac.Text + @"','" + txtFechaFac.Text + @"','"+ nitPrincipal.Text+ @"',"+ numeroPedido.Text + @", "+ Convert.ToDecimal(valorTotalVeh.Text.ToString().Substring(1)) + ", 0,0,0,60,'V', 'L', " + Convert.ToDecimal(valorTotalVeh.Text.ToString().Substring(1)) + ", 0, 60, 0, 0, 0 ) ");
                sqlRefs.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOABON = MFAC_VALOABON + "+ Convert.ToDecimal(valorOtrosPagos.Text) + ", TVIG_VIGENCIA = 'P' WHERE PDOC_CODIGO = '"+ prefijoFactura.SelectedValue + "' AND MFAC_NUMEDOCU = "+ Convert.ToUInt32(numeroFactura.Text.Trim()) +";  ");
                sqlRefs.Add("INSERT INTO DDETALLEFACTURACLIENTE VALUES ('" + prefijoFactura.SelectedValue + "', " + Convert.ToUInt32(numeroFactura.Text.Trim()) + ", '" + prefijoFactura.SelectedValue + "', " + Convert.ToUInt32(numeroFactura.Text.Trim()) + ", "+ Convert.ToDecimal(valorOtrosPagos.Text.ToString()) +", 'Registrado como Activo Fijo') ");
            }

            facturaVentaVehiculo.SqlRels = sqlRefs;
            //Ahora revisamos si existe una retoma por realizar si es asi redireccionamos al control de recepcion de vehiculos
            /*
			if(tablaRetoma.Rows.Count > 0)
			{
				Session.Clear();
				Session["tablaRetomaGrabacion"] = tablaRetomaGrabacion;
				Session["facturaVentaVehiculo"] = facturaVentaVehiculo;
				Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref=&num=&fact=N&recDir=SR");
			}
			else
			*/

            {
                //Como no es necesaria realizar la retoma entonces grabamos las retenciones y la factura directamente    

                //if (tipoFactura != "CI")  // En RETIRO VEHICULO para ACTIVBO FIJO NO GENERA RETENCIONES
                 
                    Retencion retencion = new Retencion(
                                        nitPrincipal.Text,
                                        facturaVentaVehiculo.PrefijoFactura,
                                        (int)facturaVentaVehiculo.NumeroFactura,
                                        facturaVentaVehiculo.ValorFactura,
                                        facturaVentaVehiculo.ValorIva,
                                        "V", true);
                 

                ValoresImpuestosVehiculos impuestoVehDesc = (ValoresImpuestosVehiculos)Session["ImpiestoVehDesc"];
                impuestoVehFac = (ValoresImpuestosVehiculos)Session["ImpiestoVeh"];

                if (tipovehiculo == "U")
                {
                    facturaVentaVehiculo.ValorIpoconsumo = valorIVA;
                    facturaVentaVehiculo.PorcIpoconsumo = 8;
	            }
                else
                {
                    facturaVentaVehiculo.ValorIpoconsumo = impuestoVehDesc.ValorIpoConsumo;
                    facturaVentaVehiculo.PorcIpoconsumo = impuestoVehDesc.ValorIpoConsumoPorc;
                }
                var tieneFacturaAdmin = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");
                if (ddlPrefijoFacAcce.Enabled == true && tieneFacturaAdmin == "S")
                {
                    double valorElementos=0, valorIvaElementos=0;
                    for(i=0;i<tablaElementos.Rows.Count;i++)
				    {
                            valorElementos += Convert.ToDouble(tablaElementos.Rows[i][1].ToString());
                            valorIvaElementos += Convert.ToDouble(tablaElementos.Rows[i][1].ToString()) * Convert.ToDouble(tablaElementos.Rows[i][2].ToString()) * 0.01; // Mejor usar * 0.01 que usar / 100
                    }
                    
                    facturaVentaVehiculo.PrefAccesorios = ddlPrefijoFacAcce.SelectedValue;
                    facturaVentaVehiculo.NumAccesorios = Convert.ToInt32(txtNumeroFacAcce.Text);
                    facturaVentaVehiculo.ValorAccesorios = valorElementos;
                    facturaVentaVehiculo.IvaAccesorios = valorIvaElementos;
                    if(facturaVentaVehiculo.ValorAccesorios > 0) // si el accesorio no se factura no se debe enlazar la factura
                        sqlRefs.Add("INSERT INTO mfacturapedidovehiculo VALUES('" + facturaVentaVehiculo.PrefAccesorios.ToString() + "'," + Convert.ToInt32(facturaVentaVehiculo.NumAccesorios.ToString()) + ",'" + Request.QueryString["prefP"] + "'," + Convert.ToUInt32(Request.QueryString["numP"]) + ",'A'," + numeroInventario + "," + facturaVentaVehiculo.ValorAccesorios + ",0 )");
                }

                if (valorNota > 0)// VALOR NOTA HACE REFERENCIA A UNA NOTA A FAVOR DEL CLIENTE CUANDO EL CLIENTE ABONADO UN MONTO MAYOR AL DEL VALOR DEL VEHICULO
                {
                    facturaVentaVehiculo.ValorNotaFavor = valorNota;
                    facturaVentaVehiculo.PrefNotaFavor = ddlNotaCredito.SelectedValue.ToString();
                    if (tablaAnticipos.Rows.Count > 0 && DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA") == "890924684") // SOMERAUTO usa el numero de la nota igual al primer recibo de caja de anticipo
                    {
                        facturaVentaVehiculo.NumNotaFavor = Convert.ToInt32(tablaAnticipos.Rows[0][1].ToString());
                        txtNumeNota.Text = facturaVentaVehiculo.NumNotaFavor.ToString();
                    }
                    else
                    {
                        int numeNota = Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + ddlNotaCredito.SelectedValue.ToString() + "';"));
                        //facturaVentaVehiculo.NumNotaFavor = Convert.ToInt32(txtNumeNota.Text);
                        facturaVentaVehiculo.NumNotaFavor = numeNota;
                    }
                        
                    sqlRefs.Add("INSERT INTO mfacturapedidovehiculo VALUES('" + ddlNotaCredito.SelectedValue.ToString() + "'," + Convert.ToInt32(facturaVentaVehiculo.NumNotaFavor.ToString()) + ",'" + Request.QueryString["prefP"] + "'," + Convert.ToUInt32(Request.QueryString["numP"]) + ",'N'," + numeroInventario + "," + valorNota + ",0 )");
                }

                if (txtNumeroFacAcce.Text == "")
                {
                    facturaVentaVehiculo.PrefAccesorios = "";
                }

                if (facturaVentaVehiculo.GrabarFacturaCliente(true))
					if (retencion.Guardar_Retenciones(true))
					{ //contabilizacion facturas vehiculos
                        contaOnline.contabilizarOnline(facturaVentaVehiculo.PrefijoFactura.ToString(), Convert.ToInt32(facturaVentaVehiculo.NumeroFactura.ToString()), Convert.ToDateTime(txtFechaFac.Text), "");
                        //contabiliza facturas administrativas de vehiculos
                        if(facturaVentaVehiculo.PrefAccesorios != "")
                            contaOnline.contabilizarOnline(facturaVentaVehiculo.PrefAccesorios.ToString(), Convert.ToInt32(facturaVentaVehiculo.NumAccesorios.ToString()), Convert.ToDateTime(txtFechaFac.Text), "");
                        string paramAcce = "&prefAcce=";
                        if (facturaVentaVehiculo.ValorAccesorios != 0)
                            paramAcce += facturaVentaVehiculo.PrefAccesorios;
                        else
                            paramAcce += "0";
                        
                        if (valorFinanciadoNum > 0)
                            Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos&facVeh=1&prefFC=" + facturaVentaVehiculo.PrefijoFactura + "&numFC=" + facturaVentaVehiculo.NumeroFactura + "&prefFF=" + prefijoFF + "&numFF=" + numeroFF + paramAcce + "&numAcce=" + facturaVentaVehiculo.NumAccesorios + "&prefNotaFavor=" + ddlNotaCredito.SelectedValue.ToString() + "&numNotaFavor=" + txtNumeNota.Text);
						else
                            Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos&facVeh=0&prefFC=" + facturaVentaVehiculo.PrefijoFactura + "&numFC=" + facturaVentaVehiculo.NumeroFactura + paramAcce + "&numAcce=" + facturaVentaVehiculo.NumAccesorios + "&prefNotaFavor=" + ddlNotaCredito.SelectedValue.ToString() + "&numNotaFavor=" + txtNumeNota.Text + "&codTramite="+ codTramite + "&numTramite="+ numTramite );

                    }
					else
						lb.Text += retencion.Mensajes;
				else
					lb.Text += "mal  " + facturaVentaVehiculo.ProcessMsg;
			}
		}
		
		#endregion
		
		#region Manejo de Informacion Factura
		
		protected void CambioTipoFacturacion(Object  Sender, EventArgs e)
		{
			if(tipoFacturacion.SelectedValue.ToString()=="A")
			{
				numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue.ToString()+"'");
                //numeroFactura.ReadOnly = true;
                numeroFactura.Enabled = false;
			}
			else if(tipoFacturacion.SelectedValue.ToString()=="M")
            {
                //numeroFactura.ReadOnly = false;
                numeroFactura.Enabled = true;

            }
				

		}
		
		protected void CambioPrefijoFactura(Object  Sender, EventArgs e)
		{
			numeroFactura.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura.SelectedValue.ToString()+"'");
            if (txtNumeroFacAcce.Text == numeroFactura.Text && numeroFactura.Text != "")
                numeroFactura.Text = (Convert.ToInt32(numeroFactura.Text) + 1).ToString();
		}

        protected void CambioPrefijoFacturaAcces(Object Sender, EventArgs e)
        {
            txtNumeroFacAcce.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefijoFacAcce.SelectedValue.ToString() + "'");
            if (txtNumeroFacAcce.Text == numeroFactura.Text && txtNumeroFacAcce.Text != "" && prefijoFactura.SelectedValue.ToString() == ddlPrefijoFacAcce.SelectedValue.ToString())
                txtNumeroFacAcce.Text = (Convert.ToInt32(txtNumeroFacAcce.Text) + 1).ToString();
        }

        protected void MostrarInfoPedVeh(Object Sender, EventArgs e)
		{
			plInfoFact.Visible = false;
			plInfoPedVeh.Visible = true;
            var tieneFacturaAdmin = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");
			if (prefijoFactura.SelectedValue.ToString() == "" || prefijoFactura.SelectedValue.ToString() == "Seleccione..")
			{
                Utils.MostrarAlerta(Response, "No ha seleccionado el documento de Factura adecuado ...");
				prefijoFactErrado = true;
			}
            else if ( (ddlPrefijoFacAcce.SelectedValue.ToString() == "" || ddlPrefijoFacAcce.SelectedValue.ToString() == "Seleccione..") && ddlPrefijoFacAcce.Enabled == true && tieneFacturaAdmin=="S")
            {
                    Utils.MostrarAlerta(Response, "No ha seleccionado el documento de Factura de Accesorios adecuado ...");
                    prefijoFactErrado = true;
            }
			else if (Convert.ToInt32(numeroFactura.Text) < Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_numeinic FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura.SelectedValue.ToString() + "'")) || Convert.ToInt32(numeroFactura.Text) > Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura.SelectedValue.ToString() + "'")))
			 	 {
                    Utils.MostrarAlerta(Response, "El consecutivo esta fuera de la autorización de la DIAN. Por favor informe a Contabilidad.");
					prefijoFactErrado = true;
				 }
			ViewState["prefijoFactErrado"] = prefijoFactErrado;
		}

		
		#endregion
		
		#region Manejo Informacion Pedido y Asignacion
		
		protected void RetornarInfoFac(Object  Sender, EventArgs e)
		{
			plInfoFact.Visible = true;
			plInfoPedVeh.Visible = false;
		}

		protected void MostrarInfoVent(Object  Sender, EventArgs e)
		{
			prefijoFactErrado = Convert.ToBoolean(ViewState["prefijoFactErrado"]);
			if (prefijoFactErrado)
			{
				Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos");
				return;
			}
			
			plInfoPedVeh.Visible = false;
			plInfoVenta.Visible = true;
		}
		
		protected void Cargar_Datos_Pedido(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
            DatasToControls bind = new DatasToControls();
            prefijoFactErrado = Convert.ToBoolean(ViewState["prefijoFactErrado"]);
			if (prefijoFactErrado)
			{
				Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos");
				return;
			}
			//En esta funcion cargamos los datos basicos del pedido
			prefijoPedido.Text = prefijoPedidoCargar;
			numeroPedido.Text  = numeroPedidoCargar;
            DataSet dsDatosPedido = new DataSet();
            DBFunctions.Request(dsDatosPedido, IncludeSchema.NO,
                 @" SELECT PVEN_CODIGO as codVendedor, pcat_codigo as catalogo, MPV.tcla_clase as claseVehiculoInd, pano_ano as anoModelo, mped_fechpedi as fechaPedido, 
                           mped_fechentrega as fechaEntrega, pcp.pcol_descripcion as colorPrimario, pca.pcol_descripcion as colorOpcional, tcla_nombre as claseVehiculo,
                           MPV.mnit_nit as nitPrincipal, np.nombre as nombrePrincipal, mpv.mnit_nit2 as nitAlterno, na.nombre as nombreAlterno 
                     FROM MPEDIDOVEHICULO mpv, pcolor pcp, pcolor pca, tclasevehiculo tc, vmnit np, vmnit na
                    WHERE mpv.pdoc_codigo = '" + prefijoPedidoCargar+@"' AND mpv.mped_numepedi = "+numeroPedidoCargar+ @"
                     and pcp.pcol_codigo = mpv.pcol_codigo and pca.pcol_codigo = mpv.pcol_codigoalte and mpv.tcla_clase = tc.tcla_clase and mpv.mnit_nit = np.mnit_Nit and mpv.mnit_nit2 = na.mnit_Nit ; ");

            string codVendedor = dsDatosPedido.Tables[0].Rows[0]["CODVENDEDOR"].ToString();      //DBFunctions.SingleData("SELECT PVEN_CODIGO FROM MPEDIDOVEHICULO WHERE pdoc_codigo='" + prefijoPedidoCargar + "' AND mped_numepedi=" + numeroPedidoCargar + "");
            catalogo.Text      = dsDatosPedido.Tables[0].Rows[0]["CATALOGO"].ToString();         //DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
            colorPrimario.Text = dsDatosPedido.Tables[0].Rows[0]["COLORPRIMARIO"].ToString();    //DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+")");
            colorOpcional.Text = dsDatosPedido.Tables[0].Rows[0]["COLOROPCIONAL"].ToString();    //DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigoalte FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+")");
            claseVehiculo.Text = dsDatosPedido.Tables[0].Rows[0]["CLASEVEHICULO"].ToString();    //DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase=(SELECT tcla_clase FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+")");
            claseVehiculoInd   = dsDatosPedido.Tables[0].Rows[0]["CLASEVEHICULOIND"].ToString(); //DBFunctions.SingleData("SELECT tcla_clase FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
            anoModelo.Text     = dsDatosPedido.Tables[0].Rows[0]["ANOMODELO"].ToString();        //DBFunctions.SingleData("SELECT pano_ano FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
            fechaPedido.Text   = System.Convert.ToDateTime(dsDatosPedido.Tables[0].Rows[0]["FECHAPEDIDO"]).ToString("yyyy-MM-dd"); // DBFunctions.SingleData("SELECT mped_fechpedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"")).ToString("yyyy-MM-dd");
			fechaEntrega.Text  = System.Convert.ToDateTime(dsDatosPedido.Tables[0].Rows[0]["FECHAENTREGA"]).ToString("yyyy-MM-dd"); //DBFunctions.SingleData("SELECT mped_fechentrega FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"")).ToString("yyyy-MM-dd");

            //bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PALM_ALMACEN, PALM_ALMACEN CONCAT ' - ' CONCAT PALM_DESCRIPCION FROM PALMACEN WHERE TVIG_VIGENCIA = 'V';");
            bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT PA.PALM_ALMACEN, PA.PALM_ALMACEN CONCAT ' - ' CONCAT PA.PALM_DESCRIPCION FROM PALMACEN PA, PDOCUMENTOHECHO PH WHERE PA.PALM_ALMACEN = PH.PALM_ALMACEN AND PH.PDOC_CODIGO = '" + prefijoPedidoCargar + "';");
            if (ddlAlmacen.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se encontro ninguna sede, Por favor parametrice el prefijo a la sede en  admin cofiguracion/parametros generales/ documentos asociados a hechos heconomicos");
            }
            DatasToControls.EstablecerValueDropDownList(ddlAlmacen, DBFunctions.SingleData("SELECT PALM_ALMACEN FROM PVENDEDOR WHERE PVEN_CODIGO = '" + codVendedor + "'"));
            ddlAlmacen.Enabled = false;
            //sedeVenta.Text     = DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoCargar + "' AND mped_numepedi=" + numeroPedidoCargar + "))");
			nitPrincipal.Text  = dsDatosPedido.Tables[0].Rows[0]["NITPRINCIPAL"].ToString();    //DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
            nombrePrincipal.Text = dsDatosPedido.Tables[0].Rows[0]["NOMBREPRINCIPAL"].ToString();    //DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' mnit_apellido2 FROM mnit WHERE mnit_nit='" + nitPrincipal.Text + "'");
            nitAlterno.Text    = dsDatosPedido.Tables[0].Rows[0]["NITALTERNO"].ToString();    //DBFunctions.SingleData("SELECT mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
            nombreAlterno.Text = dsDatosPedido.Tables[0].Rows[0]["NOMBREALTERNO"].ToString();    //DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' mnit_apellido2 FROM mnit WHERE mnit_nit='" + nitAlterno.Text + "'");
            dsDatosPedido.Clear();

            string notaCredSql = "SELECT DISTINCT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento AS P, pdocumentohecho AS PH WHERE TRIM(p.tdoc_tipodocu) " +
                                     "LIKE 'NC' AND TRIM(PH.tpro_proceso) LIKE 'NC' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND TRIM(ph.palm_almacen) LIKE '" + ddlAlmacen.SelectedValue + "';";
            bind.PutDatasIntoDropDownList(ddlNotaCredito, notaCredSql);

            bind.PutDatasIntoDropDownList(vendedor, "SELECT PV.pven_codigo, pven_nombre FROM pvendedor PV, PVENDEDORALMACEN PVA WHERE tvend_codigo='VV' and pven_vigencia = 'V' AND PV.PVEN_CODIGO = PVA.PVEN_CODIGO AND PVA.palm_almacen='" + ddlAlmacen.SelectedValue + "'");
            //DatasToControls.EstablecerValueDropDownList(vendedor,DBFunctions.SingleData("SELECT PVEN_CODIGO, pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+")"));
            DatasToControls.EstablecerValueDropDownList(vendedor, DBFunctions.SingleData("SELECT PVEN_CODIGO, pven_nombre FROM pvendedor WHERE pven_codigo='" + codVendedor + "'"));
            string mfac_vence  = DBFunctions.SingleData("select ppre_diasplazo from dbxschema.PPRECIOVEHICULO where pcat_codigo='"+catalogo.Text+"'");
		//	if (mfac_vence==String.Empty)
        //        Utils.MostrarAlerta(Response, "El catálogo " + catalogo.Text + " no tiene configurado los días de plazo para el vencimiento de la factura, Se toma cero días.");
		}
		
		protected void Cargar_Datos_Automovil_Asignado(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
            //En esta funcion cargamos los datos sobre el vehiculo que fue asignado
            DataSet dsDatosAutomovil = new DataSet();
            DBFunctions.Request(dsDatosAutomovil, IncludeSchema.NO,
                @"SELECT ma.mveh_inventario as numeroInventario, mc.pcat_codigo as catalogoAsignado, mv.mcat_vin as vin, mcat_motor as numeroMotor, mcat_serie as numeroSerie,
                   mcat_anomode as anoModeloAsignado, tser_nombserv as tipoServicio, pcol_descripcion as colorAsignado, mcat_numeultikilo as kilometrajeAsignado
              FROM masignacionvehiculo MA, mvehiculo mv, mcatalogovehiculo mc, tserviciovehiculo ts, pcolor pc
             WHERE ma.pdoc_codigo = '" + prefijoPedidoCargar + @"' AND ma.mped_numepedi = " + numeroPedidoCargar + @"
               and ma.mveh_inventario = mv.mveh_inventario  and mv.mcat_vin = mc.mcat_vin and mc.tser_tiposerv = ts.tser_tiposerv and pc.pcol_codigo = mc.pcol_codigo ;
            ");
 
            numeroInventario.Text  = dsDatosAutomovil.Tables[0].Rows[0]["NUMEROINVENTARIO"].ToString();    //DBFunctions.SingleData("SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
            catalogoAsignado.Text  = dsDatosAutomovil.Tables[0].Rows[0]["CATALOGOASIGNADO"].ToString();    //DBFunctions.SingleData("SELECT mc.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin = mc.mcat_vin and mv.mveh_inventario="+numeroInventario.Text+"");
            vin.Text               = dsDatosAutomovil.Tables[0].Rows[0]["VIN"].ToString();    //DBFunctions.SingleData("SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numeroInventario.Text+"");
            numeroMotor.Text       = dsDatosAutomovil.Tables[0].Rows[0]["NUMEROMOTOR"].ToString();    //DBFunctions.SingleData("SELECT mcat_motor FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoAsignado.Text+"' AND mcat_vin='"+vin.Text+"'");
            numeroSerie.Text       = dsDatosAutomovil.Tables[0].Rows[0]["NUMEROSERIE"].ToString();    //DBFunctions.SingleData("SELECT mcat_serie FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoAsignado.Text+"' AND mcat_vin='"+vin.Text+"'");
            anoModeloAsignado.Text = dsDatosAutomovil.Tables[0].Rows[0]["ANOMODELOASIGNADO"].ToString();    //DBFunctions.SingleData("SELECT mcat_anomode FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoAsignado.Text+"' AND mcat_vin='"+vin.Text+"'");
            tipoServicio.Text      = dsDatosAutomovil.Tables[0].Rows[0]["TIPOSERVICIO"].ToString();    //DBFunctions.SingleData("SELECT tser_nombserv FROM tserviciovehiculo WHERE tser_tiposerv=(SELECT tser_tiposerv FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoAsignado.Text+"' AND mcat_vin='"+vin.Text+"')");
            colorAsignado.Text     = dsDatosAutomovil.Tables[0].Rows[0]["COLORASIGNADO"].ToString();    //DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo=(SELECT pcol_codigo FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoAsignado.Text+"' AND mcat_vin='"+vin.Text+"')");
            kilometrajeAsignado.Text = dsDatosAutomovil.Tables[0].Rows[0]["KILOMETRAJEASIGNADO"].ToString();    //DBFunctions.SingleData("SELECT mcat_numeultikilo FROM mcatalogovehiculo WHERE pcat_codigo='"+catalogoAsignado.Text+"' AND mcat_vin='"+vin.Text+"'");
            dsDatosAutomovil.Clear();
        }
		#endregion
		
		#region Manejo de Información Sobre Costos
		protected void dgAccesorioBound(object sender, DataGridItemEventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			if(e.Item.ItemType == ListItemType.Footer)
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]),"SELECT piva_porciva FROM piva");
		}
		
		protected void dgEvento_Grilla_Elementos(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AgregarObsequios")
			{
				//Primero verificamos que los campos no sean vacios
				if(((((TextBox)e.Item.Cells[0].Controls[1]).Text)=="")||(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "No existe ningun elemento para adicionar O Existe algun problema con el valor");
				else
				{
					//debemos agregar una fila a la tabla asociada y luego volver a pintar la tabla
					DataRow fila = tablaElementos.NewRow();
					fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila[1] = Convert.ToDouble(((TextBox)e.Item.Cells[1].Controls[1]).Text);
					fila[2] = Convert.ToDouble(((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue.Trim());
					tablaElementos.Rows.Add(fila);
					Bind_Elementos();
				}
			}
			else if(((Button)e.CommandSource).CommandName == "QuitarObsequios")
			{
				    tablaElementos.Rows[e.Item.ItemIndex].Delete();
				    Bind_Elementos();
			}
		}
		
		protected void RetornarInfoPedVeh(Object  Sender, EventArgs e)
		{
			plInfoPedVeh.Visible = true;
			plInfoVenta.Visible = false;
		}
		
		protected void MostrarInfoPagos(Object  Sender, EventArgs e)
		{
            if (tablaAnticipos.Rows.Count != 0)
            {divMensaje.Visible = true; }
          
            var tieneFacturaAdmin = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");
            if (tieneFacturaAdmin == "N")
            {
                Utils.MostrarAlerta(Response, "No ha parametrizado la facturación Administrativa por lo tanto los otros elementos de venta NO serán facturados");
            }
            bool finac    = DBFunctions.RecordExist("SELECT MPED_VALOFINC FROM MPEDIDOVEHICULO WHERE PDOC_CODIGO='" + prefijoPedido.Text + "' AND MPED_NUMEPEDI=" + numeroPedido.Text + " AND COALESCE(MPED_VALOFINC,0) > 0 AND TESTA_CODIGO = 3;");
			string valFin = DBFunctions.SingleData("SELECT COALESCE(MCRED_VALOAPROB,0) FROM MCREDITOFINANCIERA WHERE PDOC_CODIPEDI='" + prefijoPedido.Text + "' AND MPED_NUMEPEDI=" + numeroPedido.Text + " AND TESTA_CODIGO = 3;");
			if (valFin.Length == 0)
			{
				if (finac)
				{
                    Utils.MostrarAlerta(Response, "No ha creado el crédito para el pedido !");
					return;
				}
				valFin = "0";
			} 
			plInfoVenta.Visible = false;
			plInfoPagos.Visible = true;
			//Datos Financiera
			nitRealFinanciera.Text  = DBFunctions.SingleData("SELECT MNIT_FINANCIERA FROM MCREDITOFINANCIERA WHERE PDOC_CODIPEDI='" + prefijoPedido.Text + "' AND MPED_NUMEPEDI=" + numeroPedido.Text + ";");
			nitRealFinancieraa.Text = DBFunctions.SingleData("SELECT MNIT_APELLIDOS FROM MNIT WHERE MNIT_NIT='"+nitRealFinanciera.Text+"';");
			valorFinanciado.Text    = Convert.ToDouble(valFin).ToString("#,##0");
			if (valorFinanciado.Text.Length == 0) valorFinanciado.Text = "0";
		}

        //protected void CerrarDiv(Object Sender, EventArgs e)
        //{
        //    divMensaje.Visible = false;
        //}
        protected void Cargar_Otros_Datos_Pedido(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
			DataSet dsValores       = new DataSet();
            DataSet dsCatalogoVehiculo = new DataSet();
            DataSet dsValoresUsados = new DataSet();

			DBFunctions.Request(dsCatalogoVehiculo,IncludeSchema.NO,
                "SELECT mc.pcat_codigo, mc.tser_tiposerv, mveh_valocomp+mveh_valoiva as costocompra FROM mvehiculo mv, mcatalogovehiculo mc WHERE mc.mcat_Vin = mv.mcat_vin and mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+")");

            DBFunctions.Request(dsValores,IncludeSchema.NO,
                "SELECT coalesce(mped_valounit,0) as mped_valounit,coalesce(mped_valodesc,0) as mped_valodesc,coalesce(mped_valoobse,0) as mped_valoobse, mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoCargar + "' AND mped_numepedi=" + numeroPedidoCargar + ";");

            ddlPrefijoFacAcce.Enabled = false;
            txtNumeroFacAcce.Enabled = false;

            DataSet elementosVentaPedido = new DataSet();

            var tieneFacturaAdmin = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");
            if (tieneFacturaAdmin =="S")  // se se parametriza que facture los accesorios
            {
                 DBFunctions.Request(elementosVentaPedido, IncludeSchema.NO, "SELECT pite_codigo FROM dpedidovehiculo WHERE pdoc_codigo='" + prefijoPedidoCargar + "' AND mped_numepedi=" + numeroPedidoCargar + "");

                if (elementosVentaPedido.Tables[0].Rows.Count > 0)
                {
                    ddlPrefijoFacAcce.Enabled = true;
                    txtNumeroFacAcce.Enabled = true;
                }
            }

            if (dsValores.Tables[0].Rows.Count > 0)
                esCasaMatriz = DBFunctions.SingleData("select mnit_nit from pcasamatriz where mnit_nit ='" + dsValores.Tables[0].Rows[0][3].ToString() + "'");

            double valorAuto = 0;
			double valDesc =  0;
            double valorAutoDesc = 0;
			double valObsequios=0;
			 
            ValoresImpuestosVehiculos impuestoVehDescu;

            string tipoNacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI from mnit where mnit_nit = '" + dsValores.Tables[0].Rows[0][3].ToString() + "'");
            tipoFactura = ViewState["tipoFactura"].ToString();

            if (dsValores.Tables[0].Rows.Count > 0 && claseVehiculo.Text.Trim() != "USADO")
			{
                valorAuto = Convert.ToDouble(dsValores.Tables[0].Rows[0]["mped_valounit"]);
                valDesc = Convert.ToDouble(dsValores.Tables[0].Rows[0]["mped_valodesc"]);
                valorAutoDesc = valorAuto - valDesc;
                valObsequios = Convert.ToDouble(dsValores.Tables[0].Rows[0]["mped_valoobse"]);
                if (tipoFactura == "CI")
                {
                    valorAuto = Convert.ToDouble(dsCatalogoVehiculo.Tables[0].Rows[0]["COSTOCOMPRA"].ToString()); // Consumo Interno toma el costo de compra
                    valDesc = 0;
                }
            }
            else if (dsValores.Tables[0].Rows.Count > 0 && claseVehiculo.Text.Trim() == "USADO")
            {
                DBFunctions.Request(dsValoresUsados, IncludeSchema.NO, "select mveh_valocomp, mveh_fechmani from mvehiculo where mveh_inventario = (select mveh_inventario from masignacionvehiculo where pdoc_codigo='" + prefijoPedidoCargar + "' and mped_numepedi=" + numeroPedidoCargar + ");");
                
                valorAuto = Convert.ToDouble(dsValores.Tables[0].Rows[0]["mped_valounit"]);
                valDesc = Convert.ToDouble(dsValoresUsados.Tables[0].Rows[0]["mveh_valocomp"].ToString());
                valorAutoDesc = valorAuto - valDesc;
                valObsequios = Convert.ToDouble(dsValores.Tables[0].Rows[0]["mped_valoobse"]);
                if (valorAutoDesc < 0)
                {
                    valorAutoDesc = obtenerGanancia(valDesc);
                }
            }

            //En esta funcion cargamos los datos relacionados con el pedido como precios, descuentos y otros
            valorAuto = Math.Round(valorAuto);
            long valorIVAautomovilh = 0;

            double years = 0;
            //Determinar si el vehiculo usado es menor igual a 4 años para aplicar impoconsumo.
            if (claseVehiculo.Text.Trim() == "USADO")
            {
                DateTime oldDate = Convert.ToDateTime(dsValoresUsados.Tables[0].Rows[0]["mveh_fechmani"].ToString());
                DateTime newDate = DateTime.Now;

                TimeSpan ts = newDate - oldDate;
                double diferenciaDias = ts.Days;
                years = (diferenciaDias / 365);

                lblDescuento.Text = "Costo :";
            }

            //Si es público no aplica ipoconsumo o mayor a 4 años ó vehículo nuevo para ACTIVO FIJO
            if (tipoFactura == "CI" || dsCatalogoVehiculo.Tables[0].Rows[0][1].ToString() == "2" || dsCatalogoVehiculo.Tables[0].Rows[0][1].ToString() == "P" || esCasaMatriz != "" || years > 4 )
            {
                impuestoVehFac = new ValoresImpuestosVehiculos(valorAuto, dsCatalogoVehiculo.Tables[0].Rows[0][0].ToString(), false);
                impuestoVehDescu = new ValoresImpuestosVehiculos(valorAutoDesc, dsCatalogoVehiculo.Tables[0].Rows[0][0].ToString(), false);
                LabelIvaInt.Text = "Iva :";
            }   
            else
            {
                impuestoVehFac = new ValoresImpuestosVehiculos(valorAuto, dsCatalogoVehiculo.Tables[0].Rows[0][0].ToString(), true);
                impuestoVehDescu = new ValoresImpuestosVehiculos(valorAutoDesc, dsCatalogoVehiculo.Tables[0].Rows[0][0].ToString(), true);
                LabelIvaInt.Text = "Iva + Impoconsumo :";
            }                

            Session["ImpiestoVeh"] = impuestoVehFac;
            Session["ImpiestoVehDesc"] = impuestoVehDescu;
            Session["years"] = years;
            Session["casaMatriz"] = esCasaMatriz;
            Session["tipoVehiculo"] = dsCatalogoVehiculo.Tables[0].Rows[0][1].ToString();

            valorAuto = impuestoVehFac.ValorVenta;
            valorIVAautomovilh = Convert.ToInt64(impuestoVehFac.ValorImpuestos);

            valorAutomovil.Text = impuestoVehFac.ValorVenta.ToString("C");
            valorDescuento.Text = valDesc.ToString("N");   //Para usados toma el valor de compra.
            valorVentaVeh.Text  = (valorAuto - valDesc).ToString("C");  //Para usados toma el valor de ganancia bruto.

            if (valDesc != 0) //Se aplica si hay descuentos en nuevos, o si es un usado.
            {
                if (tipoNacionalidad == "E")
                {
                    valorIVAautomovilh = 0;
                    valorBaseVeh.Text = (valorAuto - valDesc).ToString("C");
                }
                else
                {
                    valorIVAautomovilh = Convert.ToInt64(impuestoVehDescu.ValorImpuestos);
                    valorBaseVeh.Text = impuestoVehDescu.ValorBase.ToString("C");
                }
            }
            else 
            {
                if (tipoNacionalidad == "E")
                {
                    valorIVAautomovilh = 0;
                    valorBaseVeh.Text = (valorAuto - valDesc).ToString("C");
                }
                else
                {
                    valorIVAautomovilh = Convert.ToInt64(impuestoVehFac.ValorImpuestos);
                    valorBaseVeh.Text = impuestoVehFac.ValorBase.ToString("C");
                }
            }

			valorIVAautomovil.Text = valorIVAautomovilh.ToString("C");
			detalleObsequios.Text  = DBFunctions.SingleData("SELECT mped_obsequio FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
			
            if(DBFunctions.SingleData("SELECT mped_valoobse FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"")!="")
				//valorObsequios.Text = Utilidades.ConvertirADouble(DBFunctions.SingleData("SELECT mped_valoobse FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"")).ToString("N");
				valorObsequios.Text = valObsequios.ToString("N");
			else
				valorObsequios.Text = "0";
			
            //Ahora preparamos la tabla que va guardar los otros elementos de venta
			LLenar_Tabla_Elementos_Venta(prefijoPedidoCargar,numeroPedidoCargar);
			
            //Ahora calculamos el total de la venta (Base + impuestos)
            double totalVenImp = Math.Round((Convert.ToDouble(valorBaseVeh.Text.Substring(1)) + Convert.ToDouble(valorIVAautomovil.Text.Substring(1))), 0);
            valorTotalVeh.Text = totalVenImp.ToString("C");
            //Ahora calculamos el Total de la venta (Incluye accesorios)
            double totalVen = 0;
            if (claseVehiculo.Text.Trim() == "USADO")
                totalVen = Math.Round((Convert.ToDouble(valorAuto) + Convert.ToDouble(valorElementosVenta.Text.Substring(1)) + Convert.ToDouble(valorIVAElementosVenta.Text.Substring(1))), 0);
            else
            {
                totalVen = Math.Round((Convert.ToDouble(valorBaseVeh.Text.Substring(1)) + Convert.ToDouble(valorIVAautomovil.Text.Substring(1)) + Convert.ToDouble(valorElementosVenta.Text.Substring(1)) + Convert.ToDouble(valorIVAElementosVenta.Text.Substring(1))), 0);
            }
            valorTotalVenta.Text  = totalVen.ToString("C");


            //valorDescuento.Attributes.Remove("onkeyup");
            //valorDescuento.Attributes.Add("onkeyup","CalculoTotalVenta2("+valorDescuento.ClientID+","+valorAutomovil.ClientID+","+valorIVAautomovil.ClientID+","+valorElementosVenta.ClientID+","+valorIVAElementosVenta.ClientID+","+valorTotalVenta.ClientID+","+valorAuto.ToString()+","+porcIVACatalogoVehiculo.ToString()+");");
		}
		
		protected void Preparar_Tabla_Elementos_Venta()
		{
			tablaElementos = new DataTable();
			tablaElementos.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
			tablaElementos.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.Double")));
			tablaElementos.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));
            tablaElementos.Columns.Add(new DataColumn("TIPO", System.Type.GetType("System.String")));
            tablaElementos.Columns.Add(new DataColumn("TRAMITE", System.Type.GetType("System.String")));
        }
		
		protected void LLenar_Tabla_Elementos_Venta(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
			Preparar_Tabla_Elementos_Venta();
			//debemos traer los elementos de venta que se encuentran almacenados en la base de datos 
			DataSet elementosVentaPedido = new DataSet();
			DBFunctions.Request(elementosVentaPedido,IncludeSchema.NO, "SELECT D.pite_codigo, dped_valoitem, piva_porciva, TITE_CODIGO,PTRA_CODIGO FROM dpedidovehiculo D, PITEMVENTAVEHICULO P WHERE pdoc_codigo='" + prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+ " AND P.PITE_CODIGO = D.PITE_CODIGO;");
            if (elementosVentaPedido.Tables[0].Rows.Count != 0)
            {
                for (int i=0;i<elementosVentaPedido.Tables[0].Rows.Count;i++)
			    {
				    DataRow fila = tablaElementos.NewRow();
				    fila[0] = DBFunctions.SingleData("SELECT pite_nombre FROM pitemventavehiculo WHERE pite_codigo='"+elementosVentaPedido.Tables[0].Rows[i][0].ToString()+"'");
				    fila[1] = Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][1]);
				    fila[2] = Convert.ToDouble(elementosVentaPedido.Tables[0].Rows[i][2]);
                    fila[3] =elementosVentaPedido.Tables[0].Rows[i][3];
                    fila[4] = elementosVentaPedido.Tables[0].Rows[i][4];
                    tablaElementos.Rows.Add(fila);
			    }
            }
            Bind_Elementos();
		}

        protected void Bind_Elementos()
        {
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();
            grillaElementos.ShowFooter = false;
            Session["tablaElementos"] = tablaElementos;
            double totalOtrosElementos = 0;
            double ivaOtrosElementos = 0;
            var tieneFacturaAdmin = DBFunctions.SingleData("SELECT CVEH_FACTADMI fROM CVEHICULOS");

            for (int i=0;i<tablaElementos.Rows.Count;i++)
			{
                    totalOtrosElementos += Convert.ToDouble(tablaElementos.Rows[i][1]);
				    ivaOtrosElementos += Convert.ToDouble(tablaElementos.Rows[i][1])*(Convert.ToDouble(tablaElementos.Rows[i][2])/100);
				    grillaElementos.Items[i].Cells[1].HorizontalAlign = HorizontalAlign.Right;
               if (tieneFacturaAdmin == "S" && tablaElementos.Rows[i][3].ToString() == "A")
                { 
                    ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Checked = ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Enabled = true;
                }
               else  if (tieneFacturaAdmin == "S" && tablaElementos.Rows[i][3].ToString() == "T")
                {
                    ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Checked = ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Enabled = false;
                }
                else
                {
                    ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Checked = ((CheckBox)grillaElementos.Items[i].Cells[3].Controls[1]).Enabled = false;
                }
            }

            valorElementosVenta.Text = totalOtrosElementos.ToString("C");
			valorIVAElementosVenta.Text = ivaOtrosElementos.ToString("C");
            
			valorTotalVenta.Text = (Convert.ToDouble(valorAutomovil.Text.Substring(1)) + Convert.ToDouble(valorIVAautomovil.Text.Substring(1)) + Convert.ToDouble(valorElementosVenta.Text.Substring(1)) + Convert.ToDouble(valorIVAElementosVenta.Text.Substring(1))).ToString("C");
		}
		#endregion
		
		#region Manejo Informacion Pagos
		
		protected void Cargar_Datos_Pago(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
			Llenar_Tabla_Anticipos(prefijoPedidoCargar,numeroPedidoCargar);
			if(DBFunctions.SingleData("SELECT mped_valofinc FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"") != "")
				valorFinanciado.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valofinc FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"")).ToString("N");
			else
				valorFinanciado.Text = "0";
			detalleOtrosPagos.Text = DBFunctions.SingleData("SELECT mped_detaotrpago FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
			if(DBFunctions.SingleData("SELECT mped_valootrpago FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"") != "")
				valorOtrosPagos.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mped_valootrpago FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"").Trim()).ToString("N");
			Llenar_Tabla_Retoma(prefijoPedidoCargar,numeroPedidoCargar);
			//  **********************************************************
			double valorAnticipos = 0, valorFinan = 0, valorOtrPgs = 0, valorTotPgs = 0, valorTotalVnta = 0, valorRetoma = 0;
			try{valorTotalVnta = Convert.ToDouble(valorTotalVenta.Text.Substring(1));}
			catch{}
			try{valorAnticipos = Convert.ToDouble(valorTotalAnticipos.Text.Substring(1));}
			catch{}
			try{valorFinan = Convert.ToDouble(valorFinanciado.Text);}
			catch{}
			try{valorOtrPgs = Convert.ToDouble(valorOtrosPagos.Text);}
			catch{}
			try{valorRetoma = Convert.ToDouble(valorTotalRetoma.Text.Substring(1));}
			catch{}
            valorTotPgs = valorAnticipos + valorFinan + valorOtrPgs + valorRetoma; // +valorRetoma; el valor de la retoma no se toma como pago REAL, cuando se recepcione se deben cruzar las dos facturas 
			valorTotalPagos.Text = valorTotPgs.ToString("C");
			diferencia.Text = (valorTotalVnta - valorTotPgs).ToString("C");
			vlrVenta.Text = valorTotalVnta.ToString("C");
			ValidarBalance(valorTotalVnta - valorTotPgs);
		}
		
		protected void dgEvento_Grilla_Retoma(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AgregarRetoma")
			{
				//Primero debemos verificar que los campos no sean nulos
				if(((((TextBox)e.Item.Cells[0].Controls[1]).Text)=="")||((((TextBox)e.Item.Cells[1].Controls[1]).Text)=="")||((((TextBox)e.Item.Cells[2].Controls[1]).Text)=="")||((((TextBox)e.Item.Cells[3].Controls[1]).Text)=="")||((((TextBox)e.Item.Cells[5].Controls[1]).Text)=="")||(!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "Algún campo es vacio o Invalido, Revise Por Favor");
				else
				{
					//debemos agregar una fila a la tabla de retomas
					DataRow fila = tablaRetoma.NewRow();
					fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
					fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
					fila[2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
					fila[3] = ((TextBox)e.Item.Cells[3].Controls[1]).Text;
					fila[4] = ((TextBox)e.Item.Cells[5].Controls[1]).Text;
					fila[5] = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
					tablaRetoma.Rows.Add(fila);
					this.Bind_Retoma();
				}
			}
			else if(((Button)e.CommandSource).CommandName == "QuitarRetoma")
			{
				    tablaRetoma.Rows[e.Item.ItemIndex].Delete();
				    Bind_Retoma();
			}
			Validar_Tabla_Retoma();
		}
		
		protected void RetornarInfoVenta(Object  Sender, EventArgs e)
		{
			plInfoVenta.Visible = true;
			plInfoPagos.Visible = false;
		}

        protected void MostrarInfoRetoma(Object Sender, EventArgs e)
        {
            //Proceso solo para vehiculos usados

            double gananciaNeta, gananciaIva;
            if (valorOtrosPagos.Text.Trim().Length > 0 && detalleOtrosPagos.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe dar el detalle del valor de otros pagos!");
                return;
            }
            //EL PEDIDO tiene Credito de financiera?  
            if (DBFunctions.RecordExist("SELECT mped_NOMBFINC FROM MpedidoVehiculo WHERE PDOC_CODIGO='" + prefijoPedido.Text + "' AND MPED_NUMEPEDI=" + numeroPedido.Text + " AND mped_NOMBFINC <> '' AND COALESCE(MPED_VALOFINC,0) > 0;"))
            {
                //CreditoAprobado?  estado credito aprobado = 3
                if (!DBFunctions.RecordExist("SELECT MNIT_FINANCIERA FROM MCREDITOFINANCIERA WHERE PDOC_CODIPEDI='" + prefijoPedido.Text + "' AND MPED_NUMEPEDI=" + numeroPedido.Text + " AND testa_codigo = 3;"))
                {
                    Utils.MostrarAlerta(Response, "El crédito no ha sido aprobado!");
                    return;
                }
            }

            string claseVeh = "VN";  // se carga por defecto Vehiculo Nuevo
            double valorcompra = 0;
            double comision = 0;
            costoCompra = 0;
            double ivaFac = 0;
            double impoFac = 0;
            valorPerdida = 0;
            if (claseVehiculo.Text.Trim() != "NUEVO")
            {
                //Relacionamos Gastos Directos
                claseVeh = "VU";    
                valorcompra = Convert.ToDouble(DBFunctions.SingleData("Select mveh_valocomp from DBXSCHEMA.MVEHICULO where mveh_inventario=" + numeroInventario.Text + ""));
                try
                {
                    valorcompra += Convert.ToDouble(DBFunctions.SingleData("Select SUM(COALESCE(dfac_VALOrgasto,0)) from dfacturagastovehiculo df, pgastodirecto pg WHERE df.pgas_codigo = pg.pgas_codigo and pgas_indicostvehi = 'S' and MVEH_INVENTARIO = " + numeroInventario.Text + "; "));
                }
                catch
                {
                }

                costoCompra = valorcompra;

                //if (gastosDirectos == String.Empty)
                //    costoCompra = Convert.ToDouble(valorcompra);
                //else
                //    costoCompra = Convert.ToDouble(gastosDirectos) + Convert.ToDouble(valorcompra);

                double ganancia = Convert.ToDouble(valorAutomovil.Text.Substring(1)) - costoCompra;

                DataSet dsInfoRetoma = new DataSet();
                DBFunctions.Request(dsInfoRetoma, IncludeSchema.NO,
                    @"Select CV.pano_ano AS anoVigente, mc.pcat_codigo AS catalogoVehiculo, mv.MNIT_NIT as nitProveedor, ce.MNIT_NIT as nitConcesionario,
                             COALESCE(CEMP_porciva,0) as porcIVACatalogoVehiculo, COALESCE(pipo_porcipoc,0) as porcImpoconsumoCatalogoVehiculo, mv.mveh_fechmani as fechaMatricula
                        from DBXSCHEMA.CVEHICULOS CV, mvehiculo mv, mcatalogovehiculo mc, masignacionvehiculo MA, pcatalogovehiculo PC, cempresa ce
                       WHERE mc.mcat_vin = mv.mcat_vin and MV.mveh_inventario=MA.mveh_inventario AND MC.PCAT_CODIGO = PC.PCAT_CODIGO
                         and ma.pdoc_codigo='" + Request.QueryString["prefP"] +@"' AND ma.mped_numepedi=" + Request.QueryString["numP"] +@" ");
                // año vigente
                string anoVigente = dsInfoRetoma.Tables[0].Rows[0]["ANOVIGENTE"].ToString();   // DBFunctions.SingleData("Select pano_ano from DBXSCHEMA.CVEHICULOS");
                int ano = Convert.ToInt32(anoVigente) - 5;
                // año modelo vehiculo menor a 5 años del registrado en cempresa,iva %.
                string catalogoVehiculo = dsInfoRetoma.Tables[0].Rows[0]["CATALOGOVEHICULO"].ToString();   // DBFunctions.SingleData("SELECT mc.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mc WHERE mc.mcat_vin = mv.mcat_vin and mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + Request.QueryString["prefP"] + "' AND mped_numepedi=" + Request.QueryString["numP"] + ")");
                string nitProveedor     = dsInfoRetoma.Tables[0].Rows[0]["NITPROVEEDOR"].ToString();   // DBFunctions.SingleData("SELECT MNIT_NIT FROM mvehiculo mv WHERE mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + Request.QueryString["prefP"] + "' AND mped_numepedi=" + Request.QueryString["numP"] + ")");
                string nitConcesionario = dsInfoRetoma.Tables[0].Rows[0]["NITCONCESIONARIO"].ToString();   // DBFunctions.SingleData("SELECT MNIT_NIT FROM cempresa");
                //double years = Convert.ToInt32(anoVigente) - Convert.ToInt32(anoModeloAsignado.Text);
                //double years = Convert.ToDouble(Session["years"]); //Tomando valor de fecha de matriculacion.
                double ivaPre;
                if (Convert.ToInt32(anoModeloAsignado.Text) == Convert.ToInt32(anoVigente))
                {
                    try
                    {
                        ivaPre = Convert.ToDouble(DBFunctions.SingleData("Select COALESCE(CVEH_IVAPREANOACTUAL,0) from DBXSCHEMA.CVEHICULOS"));
                    } catch {

                        //ivaPre = 5; PARA AUTOEXPO NO APLICA
                        ivaPre = 0;
                    }

                    gananciaNeta = costoCompra * (ivaPre / 100);  // vehículos del año renta presuntiva del 5%
                }
                else
                {
                    try
                    {
                        ivaPre = Convert.ToDouble(DBFunctions.SingleData("Select COALESCE(CVEH_IVAPREANOANTERIOR,0) from DBXSCHEMA.CVEHICULOS"));
                    }
                    catch
                    {
                        //ivaPre = 3; PARA AUTOEXPO NO APLICA
                        ivaPre = 0;
                    }

                    gananciaNeta = costoCompra * (ivaPre / 100);  // (costoCompra = COSTO) vehículos mas de un año renta presuntiva del 3%
                }
                if (gananciaNeta > ganancia && ganancia > 0)
                    ganancia = gananciaNeta;

                if (ganancia < 0)
                {
                    valorPerdida = Math.Abs(ganancia);
                    ganancia = 0; // cuando el vehiculo da pérdida
                    Utils.MostrarAlerta(Response, "Esta vendiendo este vehículo a pérdida, está seguro ? revise por favor.!");
                }

                double porcIVACatalogoVehiculo = Convert.ToDouble(dsInfoRetoma.Tables[0].Rows[0]["PORCIVACATALOGOVEHICULO"].ToString());   // Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(CEMP_porciva,0) FROM CEMPRESA "));
                double porcImpoconsumoCatalogoVehiculo = Convert.ToDouble(dsInfoRetoma.Tables[0].Rows[0]["PORCIMPOCONSUMOCATALOGOVEHICULO"].ToString());  //Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(pipo_porcipoc,0) FROM pcatalogovehiculo where pcat_codigo = '"+ catalogoVehiculo + "'  "));
                double impuestos = 1 + (porcIVACatalogoVehiculo * 0.01);
                if (ganancia <= 0) // si hay perdida no se calcula impuestos
                    impuestos = 0;

                esCasaMatriz = Session["casaMatriz"].ToString();
                string tipoVeh = Session["tipoVehiculo"].ToString();

                string fechaMatriculaVeh = System.Convert.ToDateTime(dsInfoRetoma.Tables[0].Rows[0]["FECHAMATRICULA"]).ToString("yyyy-MM-dd"); // Convert.ToDateTime(DBFunctions.SingleData("Select mv.mveh_fechmani from MVEHICULO mv, McatalogoVEHICULO mc WHERE MVEH_INVENTARIO = " + numeroInventario.Text + " and mv.mcat_vin = mc.mcat_vin "));
                DateTime fechaMatricula = Convert.ToDateTime(fechaMatriculaVeh);
                DateTime fecha = Convert.ToDateTime(txtFechaFac.Text.ToString()); // SE DEBE CALCULAR EXACTAMENTE CONTRA LA FECHA DE MATRICULA INICIAL VS LA FECHA ESTA FACTURA
                double years = 0; // Convert.ToDouble(fecha.Year); // SE DEBE CALCULAR EXACTAMENTE CONTRA LA FECHA DE MATRICULO INICIAL VS LA FECHA ESTA FACTURA
                TimeSpan ts = fecha - fechaMatricula;
                double diferenciaDias = ts.Days;
                years = (diferenciaDias / 365);   
             
                if (years <= 4 && esCasaMatriz == "" && tipoVeh != "2" && tipoVeh != "P" && ganancia > 0)
                    impuestos += porcImpoconsumoCatalogoVehiculo * 0.01; //Aplica Impoconsumo.
                else
                    porcImpoconsumoCatalogoVehiculo = 0;

                if (impuestos > 0)
                    gananciaNeta = Math.Round(ganancia / impuestos, 0);
                else
                    gananciaNeta = 0;

                gananciaIva = ganancia - gananciaNeta;

                gananciaNeta = Math.Round(gananciaNeta, 0);
                gananciaIva = Math.Round(gananciaIva, 0);
                //Mostrar label,textbox
                LabelInt.Visible = true;
                TxtInt.Visible = true;
                TxtInt.Text = gananciaNeta.ToString("C");
                TxtIvaInt.Visible = true;
                TxtIvaInt.Text = gananciaIva.ToString("C");
                LabelIvaInt.Visible = true;

                txtIva.Visible = true;
                txtImpo.Visible = true;
                lblIva.Visible = true;
                lblImpo.Visible = true;

                //calculo iva e impoconsumo
                if (impuestos > 0)
                    comision = Math.Round(ganancia / impuestos, 0);
                else
                    comision = 0;
                ivaFac = Math.Round(comision * porcIVACatalogoVehiculo * 0.01, 0);
                impoFac = Math.Round(comision * porcImpoconsumoCatalogoVehiculo * 0.01, 0);
                if (ganancia < 0)  // Si hay perdida no liquida impuestos
                {
                    ivaFac = 0;
                    impoFac = 0;
                }
                double ajusPeso = ganancia - (comision + ivaFac + impoFac);
                if (Math.Abs(ajusPeso) == 1)
                    comision += ajusPeso;

                TxtInt.Text = comision.ToString("C");
                txtIva.Text = ivaFac.ToString("C");
                txtImpo.Text = impoFac.ToString("C");
                valorTotalVenta.Text = (comision + ivaFac + impoFac).ToString("C");


            }

            Session["valorPerdida"] = valorPerdida.ToString();
            //Debemos validar la prenda
            DataSet valores = new DataSet();
            DBFunctions.Request(valores, IncludeSchema.NO, "Select mped_valofinc,mped_valootrpago from DBXSCHEMA.MPEDIDOVEHICULO  WHERE pdoc_codigo='" + Request.QueryString["prefP"] + "' AND mped_numepedi=" + Request.QueryString["numP"] + " ");
            //Existe valor credito directo o financiera
            if (valores.Tables[0].Rows[0][0] == null && valores.Tables[0].Rows[0][1] == null && chkPrenda.Checked)
            {
                Utils.MostrarAlerta(Response, "Esta seguro que el carro tiene prenda,revise por favor.!");
            }
            else if (valores.Tables[0].Rows[0][0] != null && valores.Tables[0].Rows[0][1] != null && !chkPrenda.Checked)
            {
                Utils.MostrarAlerta(Response, "El carro debe tener prenda,revise por favor.!");
            }
            //Debemos validar la informacion entregada			
            if (valorFinanciado.Text != "" && valorFinanciado.Text != "0")
            {
                double valorF = Convert.ToDouble(valorFinanciado.Text);
                if (valorF > 0)
                {
                    if (nitRealFinanciera.Text == "")
                    {
                        Utils.MostrarAlerta(Response, "No se ha ingresado el nit de la financiera");
                        return;
                    }
                }
            }
            double valorAnticipos = 0, valorFinan = 0, valorOtrPgs = 0, valorTotPgs = 0, valorTotalVnta = 0, valorRetoma = 0;

            if (claseVehiculo.Text.Trim() != "NUEVO")
            {
                valorTotalVnta = comision + ivaFac + impoFac ;
            }
            else
            { 
                try { valorTotalVnta = Convert.ToDouble(valorTotalVeh.Text.Substring(1)); }
                catch { }
            }
			try{valorAnticipos = Convert.ToDouble(valorTotalAnticipos.Text.Substring(1));}
			catch{}
			try{valorFinan = Convert.ToDouble(valorFinanciado.Text);}
			catch{}
			try{valorOtrPgs = Convert.ToDouble(valorOtrosPagos.Text);}
			catch{}
			try{valorRetoma = Convert.ToDouble(valorTotalRetoma.Text.Substring(1));}
			catch{}

            //Recalcular valor total venta de la factura en base a los items facturados.
            double valorElementosOut = 0;
            double ivaElementosOut = 0;
            for (int j = 0; j < tablaElementos.Rows.Count; j++)
            {
                if (((CheckBox)grillaElementos.Items[j].Cells[3].Controls[1]).Checked == true)
                {
                    valorElementosOut += Convert.ToDouble(tablaElementos.Rows[j][1]);
                    ivaElementosOut += Convert.ToDouble(tablaElementos.Rows[j][1]) * (Convert.ToDouble(tablaElementos.Rows[j][2]) / 100);
                }
            }
            valorTotalVnta = valorTotalVnta + valorElementosOut + ivaElementosOut;
            Txtelemevent.Text = valorElementosOut.ToString("C");
            ViewState["Txtelemevent"] = Txtelemevent.Text;
            txtivaeleme.Text = ivaElementosOut.ToString("C");
            ViewState["txtivaeleme"] = Txtelemevent.Text;

            //if (valorAnticipos + valorFinan > valorTotalVnta && valorOtrPgs == 0) se descarta el valor de la financiera porque no hasido un ingreso real a la empresa
            if (valorAnticipos > (valorTotalVnta + costoCompra) && valorOtrPgs == 0)
            {
                //Condicion de si es mayor.... para mostrar el place...
                DatasToControls bind = new DatasToControls();
                //string codAlmacen = DBFunctions.SingleData("SELECT palm_almacen FROM palmacen WHERE tvig_vigencia='V' and palm_descripcion='" + ddlAlmacen.SelectedItem.Text.Split('-')[2].Trim() + "'");
                string codAlmacen = ddlAlmacen.SelectedValue.ToString();

                string notaCredSql = "SELECT DISTINCT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento AS P, pdocumentohecho AS PH WHERE TRIM(p.tdoc_tipodocu) " +
                                     "LIKE 'NC' AND TRIM(PH.tpro_proceso) LIKE '"+ claseVeh +"' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND TRIM(ph.palm_almacen) LIKE '" + codAlmacen + "';";
                bind.PutDatasIntoDropDownList(ddlNotaCredito, notaCredSql);

                if ((ddlNotaCredito.SelectedValue.ToString() != null || ddlNotaCredito.SelectedValue.ToString() != "") && ddlNotaCredito.Items.Count > 0)
                    txtNumeNota.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + ddlNotaCredito.SelectedValue.ToString() + "'");
                else
                {
                    Utils.MostrarAlerta(Response, "NO ha definido el prefijo para crear la Nota Crédito por el sobrante en la sede " + codAlmacen + " para el proceso " + claseVeh + ". ");
                    return;
                }
                plcNotaCred.Visible = true;
                hacerNota = true;
                // valorNota = valorAnticipos + valorFinan - valorTotalVnta;
                // valorNota = valorAnticipos - valorTotalVnta;  // el valor de la financiera NO se tiene en cuenta como sobrabte porque es un ingreso futuro y no ingreso ya efectuado en caja
                // valorNota = Convert.ToDouble(ViewState["valorNota"]) ;
            }
            else if (valorAnticipos + valorOtrPgs > (valorTotalVnta + costoCompra) && valorOtrPgs > 0)
            {
                    Utils.MostrarAlerta(Response, "El valor en Otros Pagos excede el precio de la factura. Por favor ajustar el valor.");
                    return;
            }
            valorTotPgs = valorAnticipos + valorFinan + valorOtrPgs + valorRetoma; //  +valorRetoma;
			valorTotalPagos.Text = valorTotPgs.ToString("C");
            if(claseVehiculo.Text.Trim() == "NUEVO")
                diferencia.Text = (valorTotalVnta - valorTotPgs).ToString();
            else
                diferencia.Text = (valorTotalVnta + costoCompra - valorTotPgs - valorPerdida).ToString();
            valorNota = Convert.ToDouble(diferencia.Text) + valorRetoma;
            if (valorNota > 0)
                valorNota = 0;
            else
                valorNota = Math.Abs(valorNota);

            ViewState["valorNota"] = valorNota;
            diferencia.Text = Convert.ToDouble(diferencia.Text).ToString("C");
            txtNotaCred.Text = valorNota.ToString("C");
            vlrVenta.Text = valorTotalVnta.ToString("C");
            if (valorRetoma > valorTotalVnta)
                ValidarBalance(0);
            else
                ValidarBalance(valorTotalVnta + costoCompra - valorTotPgs + valorNota - valorPerdida);
            //ValidarBalance(valorNota);
            plInfoPagos.Visible = false;
			plInfoRetoma.Visible = true;

            string facturacondiferencia = DBFunctions.SingleData("SELECT CVEH_INDIFACTDIFEPAGO FROM CVEHICULOS;");

            try
            {
               if (facturacondiferencia == "N" && diferencia.Text != "$0.00")
               {
                    plcFacturar.Visible = false;
               }
               else
               {
                    plcFacturar.Visible = true;
               }
            }
            catch
            {
                plcFacturar.Visible = true;
            }

        }

        //Al cambiar el select, cambia al numero de nota siguiente
        protected void CambioNotaNumero(Object Sender, EventArgs e)
        {
            txtNumeNota.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='" + ddlNotaCredito.SelectedValue.ToString() + "'");
        }

        //Obtener ganancia de venta sobre usados.
        private double obtenerGanancia(double pcompra)
        {
            double gananciaNeta = 0;
            double ivaPre;

            // año vigente
            string anoVigente = DBFunctions.SingleData("Select pano_ano from DBXSCHEMA.CVEHICULOS");


            if (Convert.ToInt32(anoModeloAsignado.Text) == Convert.ToInt32(anoVigente))
            {
                try
                {
                    ivaPre = Convert.ToDouble(DBFunctions.SingleData("Select COALESCE(CVEH_IVAPREANOACTUAL,0) from DBXSCHEMA.CVEHICULOS"));
                }
                catch
                {
                    ivaPre = 5;  // renta presuntiva por defecto, pero se deb crear este campo en la tabla CVEHICULOS
                }

                gananciaNeta = pcompra * (ivaPre/100);  // vehículos del año renta presuntiva del 5%
            }
            else
            {
                try
                {
                ivaPre = Convert.ToDouble(DBFunctions.SingleData("Select COALESCE(CVEH_IVAPREANOANTERIOR,0) from DBXSCHEMA.CVEHICULOS"));
                }
                catch
                {
                    ivaPre = 3;  // renta presuntiva por defecto, pero se deb crear este campo en la tabla CVEHICULOS
                }
                gananciaNeta = pcompra * (ivaPre/100);  // vehículos mas de un año renta presuntiva del 3%
            
            }

            return gananciaNeta;
        }

		private void ValidarBalance(double valB)
		{
			plcFacturar.Visible = true;
			if (Math.Round(valB) > 0)
			{
				plcFacturar.Visible = false;
				lblErrorFact.Text = "No hay balance, el total de lo vendido NO es igual al total de las formas de pago: " + valB;

			}
			else
			{
				plcFacturar.Visible = true;
				lblErrorFact.Text = "Tenga cuidado revise los valores antes de facturar. ";
                if (Math.Round(valB) < 0)
			        lblErrorFact.Text = "No hay balance, el total de lo vendido NO es igual al total de las formas de pago: " + valB;
                if (valorNota > 0)
                    lblErrorFact.Text += " .. <font color='red'>Tiene un valor sobrante de : </font> " + valorNota.ToString("C");
			}
		}
		
		protected void RetornarInfoPagos(Object  Sender, EventArgs e)
		{
			plInfoPagos.Visible = true;
			plInfoRetoma.Visible = false;
		}
		
		protected void Preparar_Tabla_Anticipos()
		{
			tablaAnticipos = new DataTable();
			tablaAnticipos.Columns.Add(new DataColumn("PREFIJOANTICIPO",System.Type.GetType("System.String")));
			tablaAnticipos.Columns.Add(new DataColumn("NUMEROANTICIPO",System.Type.GetType("System.String")));
			tablaAnticipos.Columns.Add(new DataColumn("FECHAANTICIPO",System.Type.GetType("System.String")));
			tablaAnticipos.Columns.Add(new DataColumn("VALORANTICIPO",System.Type.GetType("System.Double")));
			tablaAnticipos.Columns.Add(new DataColumn("TIPODOCUMENTO",System.Type.GetType("System.String")));			
		}
		
		protected void Llenar_Tabla_Anticipos(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
			int i;
			Preparar_Tabla_Anticipos();
			//Traemos los datos sobre los anticipos hechos a este pedido
			DataSet anticiposHechos = new DataSet();
			//DBFunctions.Request(anticiposHechos,IncludeSchema.NO,"SELECT pdoc_codigo, mcaj_numero, mant_valorecicaja FROM manticipovehiculo WHERE mped_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+" AND pdoc_codfact IS NULL AND mfac_numedocu IS NULL");
			DBFunctions.Request(anticiposHechos,IncludeSchema.NO,"SELECT MAV.pdoc_codigo, MAV.mcaj_numero, MAV.mant_valorecicaja FROM dbxschema.manticipovehiculo MAV, dbxschema.mcaja MCAJ WHERE MAV.mped_codigo='"+prefijoPedidoCargar+"' AND MAV.mped_numepedi="+numeroPedidoCargar+" AND MAV.pdoc_codfact IS NULL AND MAV.mfac_numedocu IS NULL AND MAV.pdoc_codigo = MCAJ.pdoc_codigo AND MAV.mcaj_numero = MCAJ.mcaj_numero AND test_estadodoc='A'");
			for(i=0;i<anticiposHechos.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaAnticipos.NewRow();
				fila[0] = anticiposHechos.Tables[0].Rows[i][0].ToString();
				fila[1] = anticiposHechos.Tables[0].Rows[i][1].ToString();
				fila[2] = Convert.ToDateTime(DBFunctions.SingleData("SELECT mcaj_fecha FROM mcaja WHERE pdoc_codigo='"+anticiposHechos.Tables[0].Rows[i][0].ToString()+"' AND mcaj_numero="+anticiposHechos.Tables[0].Rows[i][1].ToString()+"")).ToString("yyyy-MM-dd");
				fila[3] = Convert.ToDouble(anticiposHechos.Tables[0].Rows[i][2]);
				fila[4] = DBFunctions.SingleData("SELECT pdoc_descripcion FROM pdocumento WHERE pdoc_codigo='"+anticiposHechos.Tables[0].Rows[i][0].ToString()+"'");
				tablaAnticipos.Rows.Add(fila);
			}
			//Por ahora dejamos asi lo de las notas credito se debe dejar como una opcion de otra forma de pago
			Bind_Anticipos();
		}
		
		protected void Bind_Anticipos()
		{
			Session["tablaAnticipos"] = tablaAnticipos;
			grillaAnticipos.DataSource = tablaAnticipos;
			grillaAnticipos.DataBind();
			double valorAnticipos = 0;
			for(int i=0;i<tablaAnticipos.Rows.Count;i++)
			{
				valorAnticipos += Convert.ToDouble(tablaAnticipos.Rows[i][3]);
				grillaAnticipos.Items[i].Cells[3].HorizontalAlign = HorizontalAlign.Right;
			}
			valorTotalAnticipos.Text = valorAnticipos.ToString("C");
		}
		
		protected void Preparar_Tabla_Retoma()
		{
			tablaRetoma = new DataTable();
			tablaRetoma.Columns.Add(new DataColumn("TIPOVEHICULO",System.Type.GetType("System.String")));//0
			tablaRetoma.Columns.Add(new DataColumn("NUMEROCONTRATO",System.Type.GetType("System.String")));//1
			tablaRetoma.Columns.Add(new DataColumn("ANOMODELO",System.Type.GetType("System.String")));//2
			tablaRetoma.Columns.Add(new DataColumn("NUMEROPLACA",System.Type.GetType("System.String")));//3
			tablaRetoma.Columns.Add(new DataColumn("CUENTAIMPUESTOS",System.Type.GetType("System.String")));//4
			tablaRetoma.Columns.Add(new DataColumn("VALORRECIBIDO",System.Type.GetType("System.Double")));//5
		}
		
		protected void Llenar_Tabla_Retoma(string prefijoPedidoCargar, string numeroPedidoCargar)
		{
			Preparar_Tabla_Retoma();
			//se traen ahora los vehiculos de retoma
			DataSet vehiculosRetoma = new DataSet();
			DBFunctions.Request(vehiculosRetoma,IncludeSchema.NO,"SELECT pcat_codigo, dped_numecont, pano_ano, dped_numeplaca, dped_cuenimpu, dped_valoreci FROM dpedidovehiculoretoma WHERE pdoc_codigo='"+prefijoPedidoCargar+"' AND mped_numepedi="+numeroPedidoCargar+"");
			for(int i=0;i<vehiculosRetoma.Tables[0].Rows.Count;i++)
			{
				DataRow fila = tablaRetoma.NewRow();
				fila[0] = vehiculosRetoma.Tables[0].Rows[i][0].ToString();
				fila[1] = vehiculosRetoma.Tables[0].Rows[i][1].ToString();
				fila[2] = vehiculosRetoma.Tables[0].Rows[i][2].ToString();
				fila[3] = vehiculosRetoma.Tables[0].Rows[i][3].ToString();
				fila[4] = vehiculosRetoma.Tables[0].Rows[i][4].ToString();
				fila[5] = Convert.ToDouble(vehiculosRetoma.Tables[0].Rows[i][5].ToString());
				tablaRetoma.Rows.Add(fila);
			}
			Bind_Retoma();
			Validar_Tabla_Retoma();
		}
		
		protected void Bind_Retoma()
		{
			Session["tablaRetoma"] = tablaRetoma;
			grillaRetoma.DataSource = tablaRetoma;
			grillaRetoma.DataBind();
			double valorRetoma = 0;
			for(int i=0;i<tablaRetoma.Rows.Count;i++)
			{
				valorRetoma += Convert.ToDouble(tablaRetoma.Rows[i][5]);
				grillaRetoma.Items[i].Cells[6].HorizontalAlign = HorizontalAlign.Center;
			}
			valorTotalRetoma.Text = valorRetoma.ToString("C");
			//**********************************************************
			double valorAnticipos = 0, valorFinan = 0, valorOtrPgs = 0, valorTotPgs = 0, valorTotalVnta = 0;
			try{valorTotalVnta = Convert.ToDouble(valorTotalVenta.Text.Substring(1));}
			catch{}
			try{valorAnticipos = Convert.ToDouble(valorTotalAnticipos.Text.Substring(1));}
			catch{}
			try{valorFinan = Convert.ToDouble(valorFinanciado.Text);}
			catch{}
			try{valorOtrPgs = Convert.ToDouble(valorOtrosPagos.Text);}
			catch{}
			try{valorRetoma = Convert.ToDouble(valorTotalRetoma.Text.Substring(1));}
			catch{}
            valorTotPgs = valorAnticipos + valorFinan + valorOtrPgs + valorRetoma; // +valorRetoma;
			valorTotalPagos.Text = valorTotPgs.ToString("C");
			diferencia.Text = (valorTotalVnta - valorTotPgs).ToString("C");
			vlrVenta.Text = valorTotalVnta.ToString("C");
			ValidarBalance(valorTotalVnta - valorTotPgs);
		}
		
		protected void Validar_Tabla_Retoma()
		{
			DatasToControls bind = new DatasToControls();			
			//debemos cargar dentro de la grilla de retomas
			for(int i=0;i<grillaRetoma.Items.Count;i++)
				bind.PutDatasIntoDropDownList(((DropDownList)grillaRetoma.Items[i].Cells[4].Controls[1]),Colores.COLORES);
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
