// created on 08/02/2005 at 9:41
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Documentos;

namespace AMS.Vehiculos
{
	public class FacturaPedido
	{
		protected string prefijoFactura, numeroFactura, nitCliente, almacen, vigencia, fechaFactura, fechaVencimiento, fechaPago;
		protected string valorFactura, valorIva, valorFletes, valorIvaFletes, valorRetenciones, valorAbonado, valorCosto, diasPlazo, centroCosto;
		protected string observacion, codigoVendedor, usuario, prefijoFacturaFinanciera, nitFinanciera, valorFinanciado, valorIvaFinanciado, numeroInventario, fechaHoraCreacion;
		protected string prefijoPedidoRetoma, processMsg="";
		protected DataTable vehiculosRetoma, elementosVenta;
		
		public string PrefijoFactura{set{prefijoFactura = value;}get{return prefijoFactura;}}
		public string NumeroFactura{set{numeroFactura = value;}get{return numeroFactura;}}
		public string NitCliente{set{nitCliente = value;}get{return nitCliente;}}
		public string Almacen{set{almacen = value;}get{return almacen;}}
		public string Vigencia{set{vigencia = value;}get{return vigencia;}}
		public string FechaFactura{set{fechaFactura = value;}get{return fechaFactura;}}
		public string FechaVencimiento{set{fechaVencimiento = value;}get{return fechaVencimiento;}}
		public string FechaPago{set{fechaPago = value;}get{return fechaPago;}}
		public string ValorFactura{set{valorFactura = value;}get{return valorFactura;}}
		public string ValorIva{set{valorIva = value;}get{return valorIva;}}
		public string ValorFletes{set{valorFletes = value;}get{return valorFletes;}}
		public string ValorIvaFletes{set{valorIvaFletes = value;}get{return valorIvaFletes;}}
		public string ValorRetenciones{set{valorRetenciones = value;}get{return valorRetenciones;}}
		public string ValorAbonado{set{valorAbonado = value;}get{return valorAbonado;}}
		public string ValorCosto{set{valorCosto = value;}get{return valorCosto;}}
		public string DiasPlazo{set{diasPlazo = value;}get{return diasPlazo;}}
		public string CentroCosto{set{centroCosto = value;}get{return centroCosto;}}
		public string Observacion{set{observacion = value;}get{return observacion;}}
		public string CodigoVendedor{set{codigoVendedor = value;}get{return codigoVendedor;}}
		public string Usuario{set{usuario = value;}get{return usuario;}}
		public string PrefijoFacturaFinanciera{set{prefijoFacturaFinanciera = value;}get{return prefijoFacturaFinanciera;}}
		public string NitFinanciera{set{nitFinanciera = value;}get{return nitFinanciera;}}
		public string ValorFinanciado{set{valorFinanciado = value;}get{return valorFinanciado;}}
		public string ValorIvaFinanciado{set{valorIvaFinanciado = value;}get{return valorIvaFinanciado;}}
		public string NumeroInventario{set{numeroInventario = value;}get{return numeroInventario;}}
		public string FechaHoraCreacion{set{fechaHoraCreacion = value;}get{return fechaHoraCreacion;}}
		public string PrefijoPedidoRetoma{set{prefijoPedidoRetoma = value;}get{return prefijoPedidoRetoma;}}
		public string ProcessMsg{set{processMsg = value;}get{return processMsg;}}
		public DataTable VehiculosRetoma{set{vehiculosRetoma = value;}get{return vehiculosRetoma;}}
		
		//Aqui van nuestros constructores
		public FacturaPedido()
		{
			vehiculosRetoma = null;
			elementosVenta = null;
		}
		
		public FacturaPedido(DataTable vehRt, DataTable elemVta)
		{
			vehiculosRetoma = new DataTable();
			vehiculosRetoma = vehRt;
			elementosVenta = new DataTable();
			elementosVenta = elemVta;
		}
		
		//Aqui vamos a crear la funcion que nos crea la factura dentro de la base de datos
		public bool Grabar_Factura_Pedido(string prefijoPedido, string numeroPedido)
		{
			//FacturaPedido FacturaPedidos = new FacturaPedido();
			//FacturaPedidos.Grabar_Factura_Pedido(string prefijopedi,string numped);
			ArrayList sqlStrings = new ArrayList();
			ArrayList prefijoPedidoRetoma = new ArrayList();
			ArrayList numeroPedidoRetoma = new ArrayList();
			ArrayList estadoActualizacionNumero = new ArrayList();
			bool status = false;
			int i;
			//Revisamos si existe alguna factura de cliente con este prefijo y este numero
			/*while(DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+""))
				numeroFactura = (Convert.ToInt32(numeroFactura)+1).ToString();*/
			//Averiguamos los días de plazo de este cliente.
			uint diasP=Convert.ToUInt32(DBFunctions.SingleData("Select coalesce(mcli_diasplaz,0) from DBXSCHEMA.MCLIENTE where mnit_nit='"+this.nitCliente+"'"));
			if (Convert.ToString(diasP)==String.Empty)
				diasP=0;
			//Ahora vamos a grabar el registro de mfacturacliente
			// old sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+this.prefijoFactura+"',"+this.numeroFactura+",'"+this.nitCliente+"','"+this.almacen+"','F','V','"+this.fechaFactura+"','"+this.fechaVencimiento+"','"+this.fechaPago+"',"+this.valorFactura+","+this.valorIva+","+this.valorFletes+","+this.valorIvaFletes+","+this.valorRetenciones+","+this.valorAbonado+","+this.valorCosto+",0,'"+this.centroCosto+"','"+this.observacion+"','"+this.codigoVendedor+"','"+this.usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
			FacturaCliente FacturaClientes = new FacturaCliente("V",this.prefijoFactura,this.nitCliente,this.almacen,"F",Convert.ToUInt32(this.numeroFactura),diasP,Convert.ToDateTime(this.fechaFactura),Convert.ToDateTime(this.fechaVencimiento),Convert.ToDateTime(this.fechaPago),Convert.ToDouble(this.valorFactura),Convert.ToDouble(this.valorIva),Convert.ToDouble(this.valorFletes),Convert.ToDouble(this.valorIvaFletes),Convert.ToDouble(this.valorRetenciones),Convert.ToDouble(this.valorCosto),this.centroCosto,this.observacion,this.codigoVendedor,this.usuario,null);
            int numeroInventario = Convert.ToInt32(DBFunctions.SingleData("Select mveh_inventario from DBXSCHEMA.MasignacionVEHICULO where PDOC_CODIGO = '" + prefijoPedido + "' AND MPED_NUMEPEDI = " + numeroPedido + ";"));
			
			//Ahora vamos a guardar el registro que nos enlaza la factura con el pedido
			// old sqlStrings.Add("INSERT INTO mfacturapedidovehiculo VALUES('"+this.prefijoFactura+"',"+this.numeroFactura+",'"+prefijoPedido+"',"+numeroPedido+",'C','"+this.fechaHoraCreacion+"')");			
			sqlStrings.Add("INSERT INTO mfacturapedidovehiculo VALUES('@1',@2,'"+prefijoPedido+"',"+numeroPedido+",'C',"+numeroInventario+")");			
			//Ahora se crea un solo pedido para el cliente de retoma 
			sqlStrings.Add("INSERT INTO mpedidovehiculoproveedor VALUES('"+this.prefijoPedidoRetoma+"',"+DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+this.prefijoPedidoRetoma+"'")+",'"+this.fechaFactura+"','"+this.nitCliente+"','Retoma Vehiculo Usado')");
			//Ahora grabamos los registros de dpedidovehiculoproveedor
			for(i=0;i<vehiculosRetoma.Rows.Count;i++)
			{
				sqlStrings.Add("INSERT INTO dpedidovehiculoproveedor VALUES('"+this.prefijoPedidoRetoma+"',"+DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+this.prefijoPedidoRetoma+"'")+",'"+vehiculosRetoma.Rows[i][0].ToString()+"','"+vehiculosRetoma.Rows[i][1].ToString()+"','"+vehiculosRetoma.Rows[i][1].ToString()+"',1,0,"+vehiculosRetoma.Rows[i][2].ToString()+")");
				sqlStrings.Add("INSERT INTO mretomavehiculo VALUES('@1',@2,'"+this.prefijoPedidoRetoma+"',"+DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+this.prefijoPedidoRetoma+"'")+",'"+vehiculosRetoma.Rows[i][0].ToString()+"','"+vehiculosRetoma.Rows[i][1].ToString()+"','"+vehiculosRetoma.Rows[i][3].ToString()+"',"+vehiculosRetoma.Rows[i][4].ToString()+",'"+vehiculosRetoma.Rows[i][5].ToString()+"','"+vehiculosRetoma.Rows[i][6].ToString()+"',default)");
			}
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+this.prefijoPedidoRetoma+"'");
			//Ahora colocamos el vehiculo como facturado
			sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=40 WHERE mveh_inventario="+this.numeroInventario+"");
			//Ahora debemos ingresar los elementos de venta de esta factura en la tabla mfacturaelementosventa
         //   if(1==0) // si configura facturar los elementos de la venta
            {
			    for(i=0;i<elementosVenta.Rows.Count;i++)
				    sqlStrings.Add("INSERT INTO mfacturaelementosventa VALUES('@1',@2,'"+DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='"+elementosVenta.Rows[i][0].ToString()+"'")+"',"+elementosVenta.Rows[i][1].ToString()+")");
			}
                //Ahora revisamos si es necesario que se cree una factura para la financiera
			if(this.prefijoFacturaFinanciera != "")
			{
				string numeroFacturaFinanciera = "";
				if(prefijoFacturaFinanciera == prefijoFactura)
					numeroFacturaFinanciera = (Convert.ToInt32(numeroFactura)+1).ToString();
				else
					DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFacturaFinanciera+"'");
				while(DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFacturaFinanciera+"' AND mfac_numedocu="+numeroFacturaFinanciera+""))
					numeroFacturaFinanciera = (Convert.ToInt32(numeroFacturaFinanciera)+1).ToString();
				//Ahora vamos a grabar el registro de mfacturacliente
				sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+this.prefijoFacturaFinanciera+"',"+numeroFacturaFinanciera+",'"+this.nitFinanciera+"','"+this.almacen+"','F','V','"+this.fechaFactura+"','"+this.fechaVencimiento+"','"+this.fechaPago+"',"+this.valorFinanciado+","+this.valorIvaFinanciado+","+this.valorFletes+","+this.valorIvaFletes+","+this.valorRetenciones+",0,"+this.valorCosto+",0,'"+this.centroCosto+"','Factura A Financiera','"+this.codigoVendedor+"','"+this.usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
				//Ahora vamos a guardar el registro que nos enlaza la factura con el pedido
				sqlStrings.Add("INSERT INTO mfacturapedidovehiculo VALUES('"+this.prefijoFacturaFinanciera+"',"+numeroFacturaFinanciera+",'"+prefijoPedido+"',"+numeroPedido+",'F','"+numeroInventario+"')");
				//Ahora relacionamos la factura del cliente con la factura de la financiera
				sqlStrings.Add("INSERT INTO mfacturaclienteotrospagos VALUES('@1',@2,'"+this.prefijoFacturaFinanciera+"',"+numeroFacturaFinanciera+")");
				//Ahora actualizamos el numero de consecutivo de la factura de financiera generada
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+(Convert.ToInt32(numeroFacturaFinanciera)).ToString()+" WHERE pdoc_codigo='"+this.prefijoFacturaFinanciera+"'");
			}
			//Se actualiza el numero del docuemnto de la factura
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+(Convert.ToInt32(this.numeroFactura)).ToString()+" WHERE pdoc_codigo='"+this.prefijoFactura+"'");
			//Ahora Cambiamos el estado del pedido
			sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=30 WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			//Ahora debemos relacionar los anticipos con la factura creada, Deebemos traer todos los anticipos y relacionarlos
			DataSet anticiposHechos = new DataSet();
			DBFunctions.Request(anticiposHechos,IncludeSchema.NO,"SELECT pdoc_codigo, mcaj_numero, mant_valorecicaja FROM manticipovehiculo WHERE mped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			for(i=0;i<anticiposHechos.Tables[0].Rows.Count;i++)
			{
				//sqlStrings.Add("INSERT INTO dcajacliente VALUES('"+anticiposHechos.Tables[0].Rows[i][0].ToString()+"',"+anticiposHechos.Tables[0].Rows[i][1].ToString()+",'"+this.prefijoFactura+"',"+this.numeroFactura+","+anticiposHechos.Tables[0].Rows[i][2].ToString()+",'A')");
				sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('@1',@2,'"+anticiposHechos.Tables[0].Rows[i][0].ToString()+"',"+anticiposHechos.Tables[0].Rows[i][1].ToString()+","+anticiposHechos.Tables[0].Rows[i][2].ToString()+",'Anticipo a Pedido')");
                sqlStrings.Add("UPDATE MANTICIPOVEHICULO SET TEST_ESTADO = 30 WHERE PDOC_CODIGO = '" + anticiposHechos.Tables[0].Rows[i][0].ToString() + "' AND MCAJ_NUMERO = " + anticiposHechos.Tables[0].Rows[i][1].ToString() + " AND mped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " ");
                FacturaClientes.AgregarPago(anticiposHechos.Tables[0].Rows[i][0].ToString(), Convert.ToUInt32(anticiposHechos.Tables[0].Rows[i][1]), Convert.ToDouble(anticiposHechos.Tables[0].Rows[i][2]), "Anticipo a Pedido");
			}
			
			FacturaClientes.SqlRels=sqlStrings;
			//oldif(DBFunctions.Transaction(sqlStrings))
			if(FacturaClientes.GrabarFacturaCliente(true))
			{
				status = true;
				processMsg += FacturaClientes.ProcessMsg + "<br>";
			}
			else
				processMsg += "Error: " + FacturaClientes.ProcessMsg + "<br><br>";
			return status;
		}
		
	private int Cantidad_Coincidencias(ArrayList prefijos, string prefijoActual)
	{
		int cantidadCoincidencias = 0;
		for(int i=0;i<prefijos.Count;i++)
		{
			if(prefijos[i].ToString()==prefijoActual)
				cantidadCoincidencias +=1;
		}
		return cantidadCoincidencias;
	}
	
	//Aqui vamos a construir una funcion que nos permita hacer la devolucion de la factura hecha para un pedido
	public bool Devolucion_Factura(string prefijoPedido, string numeroPedido, string prefijoNC, string numeroNC, string usuarioDevolucion)
	{
		bool status = false;
		int i,j;
		ArrayList sqlStrings = new ArrayList();
		double porcentajeIva = System.Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
		double valorFinanciadoRestar = 0;
		string fecha = DateTime.Now.Date.ToString("yyyy-MM-dd");
		string fechaHora = DateTime.Now.Date.ToString("yyyy-MM-dd")+" "+DateTime.Now.TimeOfDay.ToString().Substring(0,8);
		//Primero debemos traer las facturas que se encuentren relacionadas con el pedido que se encuentran en la tabla mfacturapedidovehiculo y que sean de tipo Financiera y sean vigentes
		DataSet facturasAsociadasPedido = new DataSet();
		DBFunctions.Request(facturasAsociadasPedido,IncludeSchema.NO,"SELECT MFPV.pdoc_codigo, MFPV.mfac_numedocu FROM dbxschema.mfacturapedidovehiculo MFPV, dbxschema.mfacturacliente MFAC, dbxschema.pdocumento PDOC WHERE MFPV.mped_codigo='"+prefijoPedido+"' AND MFPV.mped_numepedi="+numeroPedido+" AND MFPV.mfac_tipclie='F' AND MFAC.pdoc_codigo = MFPV.pdoc_codigo AND MFAC.mfac_numedocu= MFPV.mfac_numedocu AND MFAC.tvig_vigencia='V' AND PDOC.pdoc_codigo = MFAC.pdoc_codigo AND PDOC.tdoc_tipodocu = 'FC'");
		for(i=0;i<facturasAsociadasPedido.Tables[0].Rows.Count;i++)
		{
			double valorAbonadoFacturaFinanciera = System.Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+""));
			if(valorAbonadoFacturaFinanciera==0)
			{
				//Aqui como el valor abonado es igual a cero debemos borrar la factura y el registro en la tabla mfacturapedidovehiculo
				valorFinanciadoRestar += System.Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+""));
				sqlStrings.Add("DELETE FROM mfacturapedidovehiculo WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+" AND mped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
				sqlStrings.Add("DELETE FROM mfacturaclienteotrospagos WHERE pdoc_codidest='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocudest="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
				sqlStrings.Add("DELETE FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
			}
			else
			{
				//Aqui debemos generar una nota credito para la factura de la financiera y hacer el enlace con mfacturapedidovehiculo
				string valorAbonadoFinanciera = DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
				string valorIvaAbonadoFinanciera = ((porcentajeIva*System.Convert.ToDouble(valorAbonadoFinanciera))/(100+porcentajeIva)).ToString();
				string numeroNCFinanciera = (System.Convert.ToInt32(numeroNC)+1).ToString();
				valorFinanciadoRestar += System.Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")) - System.Convert.ToDouble(valorAbonadoFinanciera);
				sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNC+"',"+numeroNCFinanciera+",'"+DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','"+DBFunctions.SingleData("SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','N','V','"+fecha+"','"+fecha+"','"+fecha+"',"+valorAbonadoFinanciera+","+valorIvaAbonadoFinanciera+",0,0,0,"+valorAbonadoFinanciera+",0,0,'"+DBFunctions.SingleData("SELECT pcen_codigo FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','Nota Credito a Favor de la Financiera','"+DBFunctions.SingleData("SELECT pven_codigo FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','"+usuarioDevolucion+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
				sqlStrings.Add("INSERT INTO mfacturapedidovehiculo VALUES('"+prefijoNC+"',"+numeroNCFinanciera+",'"+prefijoPedido+"',"+numeroPedido+",'C','"+numeroInventario+"')");
				//Ahora colocamos la factura anterior como cancelada
				sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+prefijoNC+"'");
			}
		}
		//En segundo lugar traemos las facturas del cliente
		facturasAsociadasPedido = new DataSet();
		DBFunctions.Request(facturasAsociadasPedido,IncludeSchema.NO,"SELECT MFPV.pdoc_codigo, MFPV.mfac_numedocu FROM dbxschema.mfacturapedidovehiculo MFPV, dbxschema.mfacturacliente MFAC, dbxschema.pdocumento PDOC WHERE MFPV.mped_codigo='"+prefijoPedido+"' AND MFPV.mped_numepedi="+numeroPedido+" AND MFPV.mfac_tipclie='C' AND MFAC.pdoc_codigo = MFPV.pdoc_codigo AND MFAC.mfac_numedocu= MFPV.mfac_numedocu AND MFAC.tvig_vigencia='V' AND PDOC.pdoc_codigo = MFAC.pdoc_codigo AND PDOC.tdoc_tipodocu = 'FC'");
		for(i=0;i<facturasAsociadasPedido.Tables[0].Rows.Count;i++)
		{
			//Si es igual a cliente debemos crear una factura tipo Nota Credito a favor del cliente con el valor de lo que se ha abonado
			string valorAbonado = (System.Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+""))-valorFinanciadoRestar).ToString();
			string valorIvaAbonado = ((porcentajeIva*System.Convert.ToDouble(valorAbonado))/(100+porcentajeIva)).ToString();
			sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNC+"',"+numeroNC+",'"+DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','"+DBFunctions.SingleData("SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','N','V','"+fecha+"','"+fecha+"','"+fecha+"',"+valorAbonado+","+valorIvaAbonado+",0,0,0,"+valorAbonado+",0,0,'"+DBFunctions.SingleData("SELECT pcen_codigo FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','Nota Credito A Favor del Cliente','"+DBFunctions.SingleData("SELECT pven_codigo FROM mfacturacliente WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"")+"','"+usuarioDevolucion+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
			sqlStrings.Add("INSERT INTO mfacturapedidovehiculo VALUES('"+prefijoNC+"',"+numeroNC+",'"+prefijoPedido+"',"+numeroPedido+",'C','"+numeroInventario+"')");
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+prefijoNC+"'");
			//Para Finalizar colocamos la factura del cliente en estado cancelada
			sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
			//Aun Falta eliminar las retomas hechas y los pedidos proveedor y el mvehiculo
			//Estos registros que faltan por eliminar son los que tienen que ver con los vehiculos con los que va a pagar el cliente el auto que va a a comprar
			DataSet vehiculosRetomaFactura = new DataSet();
			DBFunctions.Request(vehiculosRetomaFactura,IncludeSchema.NO,"SELECT mped_codigo, mped_numepedi FROM mretomavehiculo WHERE pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
			for(j=0;j<vehiculosRetomaFactura.Tables[0].Rows.Count;j++)
			{
				//sqlStrings.Add("DELETE FROM mvehiculo WHERE pdoc_codigopediprov='"+vehiculosRetomaFactura.Tables[0].Rows[j][0].ToString()+"' AND mped_numero="+vehiculosRetomaFactura.Tables[0].Rows[j][1].ToString()+"");
				sqlStrings.Add("DELETE FROM mretomavehiculo WHERE mped_codigo='"+vehiculosRetomaFactura.Tables[0].Rows[j][0].ToString()+"' AND mped_numepedi="+vehiculosRetomaFactura.Tables[0].Rows[j][1].ToString()+" AND pdoc_codigo='"+facturasAsociadasPedido.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasAsociadasPedido.Tables[0].Rows[i][1].ToString()+"");
				sqlStrings.Add("DELETE FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+vehiculosRetomaFactura.Tables[0].Rows[j][0].ToString()+"' AND mped_numepedi="+vehiculosRetomaFactura.Tables[0].Rows[j][1].ToString()+"");
			}
		}
		//Ahora debemos dejar el pedido en asignado y el vehiculo tambien
		sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=20 WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
		sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=30 WHERE mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+")");
		if(DBFunctions.Transaction(sqlStrings))
		{
			status = true;
			processMsg += DBFunctions.exceptions + "<br>";
		}
		else
		{
			processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
		}
		return status;
	}
 }
}
