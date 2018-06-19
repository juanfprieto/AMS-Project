using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;
using System.Configuration;
using AMS.Tools;
   
namespace AMS.Documentos
{   //
	public class FacturaCliente 
	{
		#region Atributos
		//El indicativo de factura nos permite determinar si la factura es de taller, de vehiculos, de repuestos, etc. 
		private string  indicativoFactura, prefijoFactura, nitCliente, codigoAlmacen, flagFactura, vigencia,alerta;
		private uint    numeroFactura, diasPlazo;
        private int     numDecimales = 0;
		private DateTime fechaFactura, fechaVencimiento, fechaPago;
		//El valor de la factura es igual a valorNeto + valorIva, al igual que valorFletes
		private double  valorFactura, valorIva, valorFletes, valorIvaFletes, valorRetenciones, valorAbonos, costoFactura, ipoimpuesto, ipoPorc;
        private string[] centrosCosto;
		private string  centroCosto, observacion, codigoVendedor, usuario, processMsg;
		private DataTable dtPagos;
		private ArrayList sqlRels, sqlStrings;
		private const int diasFinalizacionResolucion=10; 
		private const int consecutivosFaltantes=50;
		private bool    errorResolucion,errorConsecutivo;
		private string  prefAccesorios = "";
		private int     numAccesorios = 0;
		private double  valorAccesorios, ivaAccesorios, factorDeducible;
        private string  prefNotaFavor = "";
        private int     numNotaFavor = 0;
        private double  valorNotaFavor = 0;
        public string   timestamp, kitPed=null;
		#endregion
		
		#region Propiedades
		public string IndicativoFactura{get{return indicativoFactura;}}
		public string PrefijoFactura{get{return prefijoFactura;}}
		public string NitCliente{get{return nitCliente;}}
		public string CodigoAlmacen{get{return codigoAlmacen;}}
		public string Alerta{get{return alerta;}}
		public uint NumeroFactura{get{return numeroFactura;}}
		public uint DiasPlazo{get{return diasPlazo;}}
		public DateTime FechaFactura{get{return fechaFactura;}}
		public DateTime FechaVencimiento{get{return fechaVencimiento;}}
		public DateTime FechaPago{get{return fechaPago;}}
		public double ValorFactura{get{return valorFactura;}}
		public double ValorIva{get{return valorIva;}}
		public double ValorFletes{get{return valorFletes;}}
		public double ValorIvaFletes{get{return valorIvaFletes;}}
		public double ValorRetenciones{get{return valorRetenciones;}}
		public double ValorAbonos{get{return valorAbonos;}}
		public double CostoFactura{get{return costoFactura;}}
        public string[] CentrosCosto { get{ return centrosCosto; } }
		public string CentroCosto{get{return centroCosto;}}
		public string Observacion{get{return observacion;}}
		public string CodigoVendedor{get{return codigoVendedor;}}
		public string Usuario{get{return usuario;}}
		public string ProcessMsg{get{return processMsg;}}
		public DataTable DtPagos{get{return dtPagos;}}
		public ArrayList SqlRels{set{sqlRels = value;}get{return sqlRels;}}
		public ArrayList SqlStrings{get{return sqlStrings;}}
		public bool ErrorResolucion{get{return errorResolucion;}}
		public bool ErrorConsecutivo{get{return errorConsecutivo;}}
		
		public double ValorIpoconsumo { set { ipoimpuesto = value; } get { return ipoimpuesto; } }
		public double PorcIpoconsumo { set { ipoPorc = value; } get { return ipoPorc; } }

		public string PrefAccesorios { set { prefAccesorios = value; } get { return prefAccesorios; } }
		public int NumAccesorios { set { numAccesorios = value; } get { return numAccesorios; } }
		public double ValorAccesorios { set { valorAccesorios = value; } get { return valorAccesorios; } }
		public double IvaAccesorios { set { ivaAccesorios = value; } get { return ivaAccesorios; } }

        public string PrefNotaFavor { set { prefNotaFavor = value; } get { return prefNotaFavor; } }
        public int NumNotaFavor { set { numNotaFavor = value; } get { return numNotaFavor; } }
        public double ValorNotaFavor { set { valorNotaFavor = value; } get { return valorNotaFavor; } }
		#endregion
		
		#region Constructor
		
		public FacturaCliente()
		{
			ipoimpuesto = 0;
			ipoPorc = 0;
			indicativoFactura = "";
			flagFactura = "F";
			vigencia = "V";
			EstructuraDtPagos();
            kitPed = null;

            sqlRels = new ArrayList();
            this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); //ConfigurationManager.AppSettings["ManejoCentavos"];
            if (Utils.EsEntero(centavos))
                numDecimales = Convert.ToInt32(centavos);
            bool usarDecimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
            if (usarDecimales)
                numDecimales = 2;
		}
		
		public FacturaCliente(string indFact, string prefFact, string nitCl, string codAlm, string flgFct, uint numFact,
			//                        0				1				2				3				4				5
			uint diasPl, DateTime fechFact, DateTime fechVenc, DateTime fechPgo, double valFact, double valIva,
			//	 6						7				8				9				10				11
			double valFlts, double valIvaFlts, double valRtcns, double cstFact, string cntCst,
			//	 12					13				14				15				16
			string obs, string codVend, string usr, string kit)
			//	 17					18			19    20
		{
			indicativoFactura = indFact;
			prefijoFactura = prefFact;
			nitCliente = nitCl;
			codigoAlmacen = codAlm;
			flagFactura = flgFct;
			vigencia = "V";
			numeroFactura = numFact;
			diasPlazo = diasPl;
			fechaFactura = fechFact;
			fechaVencimiento = fechVenc;
			fechaPago = fechPgo;
            kitPed = kit;

            string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); //ConfigurationManager.AppSettings["ManejoCentavos"];
            if (Utils.EsEntero(centavos))
                numDecimales = Convert.ToInt32(centavos);

            bool usarDecimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
            string numDec = ConfigurationManager.AppSettings["tamanoDecimal"];
            if (usarDecimales)
            {
                if (numDec != null && numDec != "")
                {
                    numDecimales = Convert.ToInt16(numDec);
                }
                else
                {
                    numDecimales = 2;
                }
            }

			valorFactura = Math.Round(valFact, numDecimales);
            valorIva = Math.Round(valIva, numDecimales);
            valorFletes = Math.Round(valFlts, numDecimales);
            valorIvaFletes = Math.Round(valIvaFlts, numDecimales);
            valorRetenciones = Math.Round(valRtcns, numDecimales);
            costoFactura = Math.Round(cstFact, numDecimales);
			
			centroCosto = cntCst;
			observacion = obs;
			codigoVendedor = codVend;
			usuario = usr;
			EstructuraDtPagos();
			sqlRels = new ArrayList();
			ipoimpuesto = 0;
			ipoPorc = 0;
            this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

        //  Constructor Factura de Orden de trabajo
        public FacturaCliente(string indFact, string prefFact, string nitCl, string codAlm, string flgFct, uint numFact,
            //                        0				1				2				3				4				5
            uint diasPl, DateTime fechFact, DateTime fechVenc, DateTime fechPgo, double valFact, double valIva,
            //	 6						7				8				9				10				11
            double valFlts, double valIvaFlts, double valRtcns, double cstFact, string cntCst,
            //	 12					13				14				15				16
            string obs, string codVend, string usr, double factorDeducibleSeguros)
        //	 17					18			19
        {
            indicativoFactura = indFact;
            prefijoFactura = prefFact;
            nitCliente = nitCl;
            codigoAlmacen = codAlm;
            flagFactura = flgFct;
            vigencia = "V";
            numeroFactura = numFact;
            diasPlazo = diasPl;
            fechaFactura = fechFact;
            fechaVencimiento = fechVenc;
            fechaPago = fechPgo;
            factorDeducible = factorDeducibleSeguros;
            kitPed = null;

            string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); //ConfigurationManager.AppSettings["ManejoCentavos"];
            if (Utils.EsEntero(centavos))
                numDecimales = Convert.ToInt32(centavos);

            bool usarDecimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
            string numDec = ConfigurationManager.AppSettings["tamanoDecimal"];
            if (usarDecimales)
            {
                if (numDec != null && numDec != "")
                {
                    numDecimales = Convert.ToInt16(numDec);
                }
                else
                {
                    numDecimales = 2;
                }
            }

            valorFactura = Math.Round(valFact, numDecimales);
            valorIva = Math.Round(valIva, numDecimales);
            valorFletes = Math.Round(valFlts, numDecimales);
            valorIvaFletes = Math.Round(valIvaFlts, numDecimales);
            valorRetenciones = Math.Round(valRtcns, numDecimales);
            costoFactura = Math.Round(cstFact, numDecimales);

            centroCosto = cntCst;
            observacion = obs;
            codigoVendedor = codVend;
            usuario = usr;
            EstructuraDtPagos();
            sqlRels = new ArrayList();
            ipoimpuesto = 0;
            ipoPorc = 0;
            this.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

		#endregion
		
		#region Metodos
		private void EstructuraDtPagos()
		{
			dtPagos = new DataTable();
			dtPagos.Columns.Add(new DataColumn("PrefDocPgs",System.Type.GetType("System.String")));//0
			dtPagos.Columns.Add(new DataColumn("NumDocPgs",System.Type.GetType("System.UInt32")));//1
			dtPagos.Columns.Add(new DataColumn("ValDocPgs",System.Type.GetType("System.Double")));//2
			dtPagos.Columns.Add(new DataColumn("ObsrDocPgs",System.Type.GetType("System.String")));//3
		}
		
		private double CalcularTotalPagos()
		{
			double totalPagos = 0;
			for(int i=0;i<dtPagos.Rows.Count;i++)
			{
				if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
					totalPagos += Convert.ToDouble(dtPagos.Rows[i][2]);
				else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
					totalPagos += Math.Round(Convert.ToDouble(dtPagos.Rows[i][2]),0);
			}
			return totalPagos;
		}
		
		//Funcion que nos permite ingresar un pago y agregarlo a la tabla dtPagos
		//Parametros : Los parametros de entrada son el prefijo del documento de pago, el numero de documento, el valor del pago y la obervación
		//Retorno	 : El retorno nos indica si se tuvo exito o no al ingresar el pago
		//Condición especial : No se puede agregar el mismo documento 2 veces
		public bool AgregarPago(string prefDocPgs, uint numDocPgs, double valDocPgs, string observacion)
		{
			if((dtPagos.Select("PrefDocPgs='"+prefDocPgs+"' AND NumDocPgs="+numDocPgs)).Length > 0)
				return (false);
			DataRow fila = dtPagos.NewRow();
			fila[0] = prefDocPgs;
			fila[1] = numDocPgs;
			fila[2] = valDocPgs;
			fila[3] = observacion;
			dtPagos.Rows.Add(fila);
			return (true);
		}
		
		//Funcion para la grabación de una factura de cliente, revise como parametro un ArrayList que contiene todos los 
		//inserts relacionados con la factura que viene con dos comodines @1 el prefijo de la factura y @2 el número de la factura
		//Retorno  : El retorno es un booleano que nos indica si se tuvo exito o no al grabar la factura
		public bool GrabarFacturaCliente(bool indicadorGrabacion)
		{
			bool status = false;
			int i = 0;
			sqlStrings = new ArrayList();
			sqlStrings.Clear();
			valorAbonos = CalcularTotalPagos();
            double abonoVehiculo = 0;
            string nitEmpresa = "";

            double valorSaldo = 0;
            nitEmpresa = DBFunctions.SingleData("select mnit_nit from cempresa");
            if (nitEmpresa.ToString() == nitCliente.ToString())  // vehiculo se registra para ACTIVO FIJO
            {
                valorSaldo = 0;
                abonoVehiculo = valorFactura + valorIva + ValorFletes + valorIvaFletes - valorRetenciones;
                observacion += " Activo Fijo";
            }
            else
                valorSaldo = valorFactura + valorIva + ValorFletes + valorIvaFletes - valorRetenciones - valorAbonos;

            if (valorSaldo == 0)
				vigencia = "C";
			else
				if (valorAbonos > 0)
					vigencia = "A";
			
  
			//Revisamos el numero de la factura y revisamos si ya existe una factura de cliente con el prefijo y numero de factura del objeto
			//Si no vamos agregando uno hasta encontrar un numero que no halla sido utilizado
			while (DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura))
			{
				numeroFactura += 1;
			}

			//Primero verficamos que el número de la factura se encuentre dentro del rango permitido para ese prefijo
			if (numeroFactura > Convert.ToUInt32(DBFunctions.SingleData("SELECT coalesce(pdoc_numefina,0) FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura + "' ")) || numeroFactura < Convert.ToUInt32(DBFunctions.SingleData("SELECT coalesce(pdoc_numeinic,1) FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura + "' ")))
			{
                processMsg += "<br>Error : El número especificado " + numeroFactura + " se encuentra fuera del rango permitido para el prefijo '" + prefijoFactura + "' ";
                status = false;
                sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + processMsg + "')");
            }
            else
			{
                if (DBFunctions.RecordExist("SELECT PDOC_FINFECHRESOFACT FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura + "' AND TDOC_TIPODOCU = 'FC' AND (('" + fechaFactura.ToString("yyyy-MM-dd") + "' > PDOC_FINFECHRESOFACT  AND '" + fechaFactura.ToString("yyyy-MM-dd") + "' > PDOC_FECHHABI) OR PDOC_FINFECHRESOFACT IS NULL);"))
                {
                    processMsg += "<br>Error : La fecha de la factura es mayor a la fecha de vencimiento de la resolucion autorizada para el prefijo '" + prefijoFactura + "'. Por favor informe a Contabilidad ";
                    status = false;
                    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + processMsg + "')");
                }
			    else
			    {
                    double valorVehiculo = valorFactura + valorIva + ValorFletes + valorIvaFletes;
				    abonoVehiculo = 0;
				    double abonoAccesorios = 0;

                    if (valorVehiculo - valorAbonos >= 0)
                    {
                        abonoVehiculo = valorAbonos;
                    }
                    else
                    {
                        abonoVehiculo = valorVehiculo;
                        abonoAccesorios = valorAbonos - valorVehiculo - valorNotaFavor;
                    }

				    //Ahora creamos el registro dentro de mfacturacliente con la informacion ingresada a este objeto
				    if(fechaPago.ToString("yyyy-MM-dd") != "0001-01-01")
				    {
					    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+nitCliente+"','"+codigoAlmacen+"','"+flagFactura+"','"+vigencia+"',"+
						    "'"+fechaFactura.ToString("yyyy-MM-dd")+"','"+fechaVencimiento.ToString("yyyy-MM-dd")+"','"+fechaPago.ToString("yyyy-MM-dd")+"',"+valorFactura+","+valorIva+","+valorFletes+","+
                            "" + valorIvaFletes + "," + valorRetenciones + "," + abonoVehiculo + "," + costoFactura + "," + diasPlazo + ",'" + centroCosto + "'," +
						    "'"+observacion+"','"+codigoVendedor+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");

                        if (kitPed != null && kitPed.Length>0)
                            sqlStrings.Add("INSERT INTO DfacturaItemKit VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + kitPed + "') ");

                        if (prefAccesorios != "" && valorAccesorios != 0)
						    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefAccesorios + "'," + numAccesorios + ",'" + nitCliente + "','" + codigoAlmacen + "','" + flagFactura + "','" + vigencia + "'," +
							    "'" + fechaFactura.ToString("yyyy-MM-dd") + "','" + fechaVencimiento.ToString("yyyy-MM-dd") + "','" + fechaPago.ToString("yyyy-MM-dd") + "'," + valorAccesorios + "," + ivaAccesorios + ", 0, 0" +
                                "," + valorRetenciones + "," + abonoAccesorios + "," + costoFactura + "," + diasPlazo + ",'" + centroCosto + "'," +
							    "'" + observacion + "','" + codigoVendedor + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                
                        //En caso que exista un saldo a favor del cliente, se crea la factura de nota credito a favor.
                        if (prefNotaFavor != "" && valorNotaFavor != 0)
                        {
                            sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefNotaFavor + "'," + numNotaFavor + ",'" + nitCliente + "','" + codigoAlmacen + "','" + flagFactura + "','V'," +
                                "'" + fechaFactura.ToString("yyyy-MM-dd") + "','" + fechaVencimiento.ToString("yyyy-MM-dd") + "','" + fechaPago.ToString("yyyy-MM-dd") + "'," + valorNotaFavor + ", 0, 0, 0" +
                                ", 0, 0, 0," + diasPlazo + ",'" + centroCosto + "'," +
                                "'Nota a favor del cliente por Mayor valor pagado ' || '" + prefijoFactura + "' ||" + numeroFactura + ",'" + codigoVendedor + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                            //sqlStrings.Add("INSERT INTO MNOTACLIENTE VALUES('" + prefNotaFavor + "', " + numNotaFavor + ", '" + prefijoFactura + "'," + numeroFactura + " )");
                            if (DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA") != "890924684")   //SOMERAUTO usa el numero de la nota igual al recibo de caja del anticipo
                                sqlStrings.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU =" + numNotaFavor + " WHERE PDOC_CODIGO = '" + prefNotaFavor + "';");
                        }
				    }
				    else
				    {
					    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+nitCliente+"','"+codigoAlmacen+"','"+flagFactura+"','"+vigencia+"',"+
						    "'"+fechaFactura.ToString("yyyy-MM-dd")+"','"+fechaVencimiento.ToString("yyyy-MM-dd")+"',null,"+valorFactura+","+valorIva+","+valorFletes+","+
                            "" + valorIvaFletes + "," + valorRetenciones + "," + abonoVehiculo + "," + costoFactura + "," + diasPlazo + ",'" + centroCosto + "'," +
						    "'"+observacion+"','"+codigoVendedor+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");

                        if ((kitPed != null && kitPed.Length > 0))
                            sqlStrings.Add("INSERT INTO DfacturaItemKit VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + kitPed + "') ");

                        if (prefAccesorios != "" && valorAccesorios != 0)
						    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefAccesorios + "'," + numAccesorios + ",'" + nitCliente + "','" + codigoAlmacen + "','" + flagFactura + "','" + vigencia + "'," +
							    "'" + fechaFactura.ToString("yyyy-MM-dd") + "','" + fechaVencimiento.ToString("yyyy-MM-dd") + "','" + fechaPago.ToString("yyyy-MM-dd") + "'," + valorAccesorios + "," + ivaAccesorios + ", 0, 0" +
                                ","  + valorRetenciones + "," + abonoAccesorios + "," + costoFactura + "," + diasPlazo + ",'" + centroCosto + "'," +
							    "'" + observacion + "','" + codigoVendedor + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
				
				    }

				    //Ahora actualizamos el ultimo numero de este prefijo si es necesario
				    if (numeroFactura > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura + "'")))
				    {
					    sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=" + numeroFactura + " WHERE pdoc_codigo='" + prefijoFactura + "'");

                        if (prefAccesorios != "" && valorAccesorios != 0)
					    {
						    if (numAccesorios > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefAccesorios + "'")))
							    sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=" + numAccesorios + " WHERE pdoc_codigo='" + prefAccesorios + "'");
					    }
				    }
				
				    //Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de factura) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de factura
				    for(i=0;i<sqlRels.Count;i++)
				    {
					    string sqlRel = sqlRels[i].ToString();
                        sqlRel = sqlRel.Replace("@1", prefijoFactura);
					    sqlRel = sqlRel.Replace("@2", numeroFactura.ToString());
                        sqlRel = sqlRel.Replace("@3", factorDeducible.ToString());
					    sqlStrings.Add(sqlRel);
				    }

                    double valorPorPagar = valorVehiculo;
                    Boolean pagoVehiculo = false;

                     //Ahora vamos a agregar la relacion de los pagos dentro de la tabla ddetallefacturacliente
                    for (i = 0; i < dtPagos.Rows.Count; i++)
                    {
                        double valAbono = Convert.ToDouble( dtPagos.Rows[i][2].ToString() );
                        if (valorPorPagar - valAbono >= 0 && pagoVehiculo == false)
                        {
                            valorPorPagar = valorPorPagar - valAbono;
                            sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + dtPagos.Rows[i][0].ToString() + "'," + dtPagos.Rows[i][1].ToString() + "," + dtPagos.Rows[i][2].ToString() + ",'" + dtPagos.Rows[i][3].ToString() + "')");
                        }
                        else if (pagoVehiculo == false)
                             {
                                sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + dtPagos.Rows[i][0].ToString() + "'," + dtPagos.Rows[i][1].ToString() + "," + valorPorPagar + ",'" + dtPagos.Rows[i][3].ToString() + "')");
                                valorPorPagar = (valorPorPagar - valAbono) * (-1);
                                valorPorPagar -= valorNotaFavor;
                          //      ver porque camilo metio este suporte de pago en la nota del mayor valor recibido
                         //       sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('" + prefAccesorios + "'," + numAccesorios + ",'" + dtPagos.Rows[i][0].ToString() + "'," + dtPagos.Rows[i][1].ToString() + "," + valorPorPagar + ",'" + dtPagos.Rows[i][3].ToString() + "')");

                                if (valorPorPagar == 0)
                                    pagoVehiculo = true;
                            }
                            else
                            {
                                // Una vez pagado el vehiculo empieza a abonar a la factura de los accesorios
                                sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('" + prefAccesorios + "'," + numAccesorios + ",'" + dtPagos.Rows[i][0].ToString() + "'," + dtPagos.Rows[i][1].ToString() + "," + dtPagos.Rows[i][2].ToString() + ",'" + dtPagos.Rows[i][3].ToString() + "')");
                            }
                    }

				    //Se registra el valor de impoimpuesto. Pendiente condicion para no cobrar impoimpuesto a mayoristas.

                    if (ipoPorc != 0 && ValorFletes == 0) //Para vehiculos Nuevos
					    sqlStrings.Add("INSERT INTO mfacturaclienteimpuesto VALUES('" + prefijoFactura + "'," + numeroFactura + "," + ipoPorc + "," + ipoimpuesto + "," + valorFactura + ")");
                    else if (ValorFletes != 0 && valorIva != 0) // Para vehiculos usados los cuales registran fletes y valorIVA en caso de tener impoconsumo
                            sqlStrings.Add("INSERT INTO mfacturaclienteimpuesto VALUES('" + prefijoFactura + "'," + numeroFactura + "," + ipoPorc + "," + ipoimpuesto + "," + ValorFletes + ")");

                    if (nitEmpresa.ToString() == nitCliente.ToString())
                    {
                            //  se graba la inclusión del vehículo en los activos FIJOS
                        /*
                            sqlStrings.Add(@"INSERT INTO mACTIVOfijo 
                                            ( MAFJ_CODIACTI, MAFJ_DESCRIPCION, PCCO_CENTCOST, MAFJ_MARCA, MAFJ_MODELO, MAFJ_PLACA, MAFJ_FECHFACT, MAFJ_INGRESO, 
                                            MAFJ_FECHINIC, MNIT_NIT, MAFJ_PEDIDO, MAFJ_VALOHIST, MAFJ_VALODOLA, MAFJ_VALOMEJORA, MAFJ_INFLACUM, MAFJ_DEPRACUM, MAFJ_INFLDEPRACUM, MAFJ_CUOTAS, TVIG_VIGENCIA, 
                                            MAFJ_CUENACTIFIJO, MAFJ_CUENDEPR, MAFJ_CUENGASTDEPR, MAFJ_CUENINFLACTI, MAFJ_CUENINFLDEPR, MAFJ_CUENNIIFACTI, MAFJ_CUENNIIFDEPR, MAFJ_CUENGASTDEPRNIIF, MAFJ_VALOACTNIIF, 
                                            MAFJ_VALOMEJORANIIF, MAFJ_CUOTASNIIF, MAFJ_DEPRACUMNIIF, MAFJ_VALORECINIIF, MAFJ_VALODETENIIF, TDEP_TIPODEPR, MAFJ_UNIDUTIL, MAFJ_HORAUTIL, MAFJ_AVALUADOR, MAFJ_FECHAVAL)
                                            VALUES
                                            ('" + prefijoFactura + "'," + numeroFactura + "," + ipoPorc + "," + ipoimpuesto + "," + ValorFletes + ")");
                        */
                    }

                    /*for(i=0;i<sqlStrings.Count;i++)
                        processMsg += "<br><br>"+sqlStrings[i].ToString();
                    status = true;*/

                    //Ahora Realizamos la transaccion y verificamos si fue exitosa o no
                    if (EmitirAlertaConsecutivos()!="" && !errorConsecutivo)
					    alerta+=EmitirAlertaConsecutivos()+"\\n";
				    if(EmitirAlertaResolucion()!="" && !errorResolucion)
					    alerta+=EmitirAlertaResolucion()+"\\n";
				    if(indicadorGrabacion)
				    {
                        if (DBFunctions.Transaction(sqlStrings))
                        {
                            processMsg += "<br>Bien : " + DBFunctions.exceptions;
                            status = true;
                        }
                        else
                        {
                            processMsg += "<br>Error : " + DBFunctions.exceptions;
                            status = false;
                        }
				    }
				    else
					    status = true;
			    }
			    return status;
		    }
			return status;
		}

		/// <summary>
		/// Retorna el número de dias que faltan para vencimiento de la resolucion de facturación, tomados a partir del momento en que se
		/// invoca el objeto.
		/// </summary>
		/// <returns>
		/// int número de dias faltantes ó -1 si existe algun error
		/// </returns>
		private int DiasFaltantesParaVencimiento()
		{
			DateTime hoy=DateTime.Now.Date;
			DateTime fechaVencResol;
			try
			{
				fechaVencResol=Convert.ToDateTime(DBFunctions.SingleData("SELECT pdoc_finfechresofact FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura+"'"));
				TimeSpan faltante=fechaVencResol-hoy;
				if(faltante.Days>=0)
					return faltante.Days;
				else
					return -1;
			}
			catch
			{
				return -1;
			}
		}

		/// <summary>
		/// Retorna el número de consecutivos que faltan para que se agote el rango de facturación
		/// </summary>
		/// <returns>
		/// int consecutivos faltantes
		/// </returns>
		private int NumerosFaltantesParaAgotamientoConsecutivos()
		{
			int numeroFinal=Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura+"'"));
			int ultDoc=Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura+"'"));
			return (numeroFinal-ultDoc);
		}

		/// <summary>
		/// Retorna el mensaje de error que se mostrara en cualquier lugar donde se invoque el objeto y se cumplan las condiciones dadas para
		/// la resolución
		/// </summary>
		/// <returns>
		///	string mensaje de error; si no se cumplen las condiciones, un string vacio
		/// </returns>
		private string EmitirAlertaResolucion()
		{
			string error="";
			int dias=DiasFaltantesParaVencimiento();
			if(dias!=-1 && dias>0 && dias<=diasFinalizacionResolucion)
				error="Faltan "+dias+" dias para el vencimiento de su resolución de facturación para el prefijo "+prefijoFactura;
			else if(dias==0)
			{
				error="La resolución de facturación para el prefijo "+prefijoFactura+" ha caducado. Imposible continuar con el proceso de facturación. Seleccione otro prefijo";
				errorResolucion=true;
			}
			else if(dias==-1)
			{
				error="Existe un problema con la resolución de facturación del prefijo "+prefijoFactura+". Imposible continuar con el proceso. Seleccione otro prefijo";
				errorResolucion=true;
			}
			return error;
		}

		/// <summary>
		/// Retorna el mensaje de error que se mostrara en cualquier lugar donde se invoque el objeto y se cumplan las condiciones dadas para
		/// los consecutivos
		/// </summary>
		/// <returns>
		///	string mensaje de error; si no se cumplen las condiciones, un string vacio
		/// </returns>
		private string EmitirAlertaConsecutivos()
		{
			string error="";
			int cons=NumerosFaltantesParaAgotamientoConsecutivos();
			if(cons!=-1 && cons>0 && cons<=consecutivosFaltantes)
				error="Su rango de facturación para el prefijo "+prefijoFactura+" se está agotando. Tiene disponibles "+cons;
			else if(cons==0)
			{
				error="Su rango de facturación para el prefijo "+prefijoFactura+" se agotó. Imposible continuar el proceso. Seleccione un prefijo distinto";
				errorConsecutivo=true;
			}
			else if(cons==-1)
			{
				error="Existe un problema con el rango de facturación  para el prefijo "+prefijoFactura+". Imposible continuar el proceso. Seleccione un prefijo distinto";
				errorConsecutivo=true;
			}
			return error;
		}

		/// <summary>
		/// Unifica los llamados de alerta para emitir un solo mensaje de error
		/// </summary>
		/// <returns>
		/// string alerta generada
		/// </returns>
		public string EmitirAlertaGeneral()
		{
			string error="";
			error+=EmitirAlertaConsecutivos();
			if(error!="")
				error+="@n"+EmitirAlertaResolucion();
			else
				error+=EmitirAlertaResolucion();
			return error;
		}

		#endregion
	}
}