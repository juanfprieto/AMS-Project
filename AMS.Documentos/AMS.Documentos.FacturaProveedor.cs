using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;

namespace AMS.Documentos
{
	public class FacturaProveedor
	{
		#region Atributos
		//El indicativo de factura nos permite determinar si la factura es de taller, de vehiculos, de repuestos, etc. 
		private string indicativoFactura, prefijoFactura, prefijoDocumentoRelacion, nitProveedor, codigoAlmacen, flagFactura, vigencia;
		private UInt64 numeroFactura, numeroDocumentoRelacion;
		private DateTime fechaFactura, fechaEntradaAlmacen, fechaVencimiento, fechaPago;
		//El valor de la factura es igual a valorNeto + valorIva, al igual que valorFletes
		private double valorFactura, valorIva, valorFletes, valorIvaFletes, valorRetenciones, valorAbonos;
		private string observacion, usuario, processMsg;
		private DataTable dtPagos;
		private ArrayList sqlRels, sqlStrings;
		#endregion
		
		#region Propiedades
		public string IndicativoFactura{get{return indicativoFactura;}}
		public string PrefijoFactura{get{return prefijoFactura;}}
		public string PrefijoDocumentoRelacion{get{return prefijoDocumentoRelacion;}}
		public string NitProveedor{get{return nitProveedor;}}
		public string CodigoAlmacen{get{return codigoAlmacen;}}
		public UInt64 NumeroFactura{get{return numeroFactura;}}
		public UInt64 NumeroDocumentoRelacion{get{return numeroDocumentoRelacion;}}
		public DateTime FechaFactura{get{return fechaFactura;}}
		public DateTime FechaVencimiento{get{return fechaVencimiento;}}
		public DateTime FechaPago{get{return fechaPago;}}
		public DateTime FechaEntradaAlmacen{get{return fechaEntradaAlmacen;}}
		public double ValorFactura{get{return valorFactura;}}
		public double ValorIva{get{return valorIva;}}
		public double ValorFletes{get{return valorFletes;}}
		public double ValorIvaFletes{get{return valorIvaFletes;}}
		public double ValorRetenciones{get{return valorRetenciones;}}
		public double ValorAbonos{set{valorAbonos=value;} get{return valorAbonos;}}
		public string Observacion{get{return observacion;}}
		public string Usuario{get{return usuario;}}
		public string ProcessMsg{get{return processMsg;}}
		public DataTable DtPagos{get{return dtPagos;}}
		public ArrayList SqlRels{set{sqlRels = value;}get{return sqlRels;}}
		public ArrayList SqlStrings{get{return sqlStrings;}}
		#endregion
		
		#region Constructor
		
		public FacturaProveedor()
		{
			indicativoFactura = "";
			flagFactura = "F";
			vigencia = "V";
			EstructuraDtPagos();
			sqlRels = new ArrayList();
		}
		public FacturaProveedor(string indFact, string prefFact, string prefDocRel, string nitPr, string codAlm, string flgFct, UInt64 numFact, UInt64 numDocRef,
		                     //  0						1				2				3				4				5             6				7			
		                       string  vigFac, DateTime fechFact, DateTime fechVenc, DateTime fechPgo, DateTime fechEntAlma, double valFact, double valIva,
		                     //	 8						9				10					11						12				13			14
		                      double valFlts, double valIvaFlts, double valRtcns, string obs, string usr)
							//	 15					16					17			18			19
		{
			indicativoFactura = indFact;
			prefijoFactura = prefFact;
			prefijoDocumentoRelacion = prefDocRel;
			nitProveedor = nitPr;
			codigoAlmacen = codAlm;
			flagFactura = flgFct;
			vigencia = vigFac;
			numeroFactura = numFact;
			numeroDocumentoRelacion = numDocRef;
			fechaFactura = fechFact;
			fechaVencimiento = fechVenc;
			fechaPago = fechPgo;
			fechaEntradaAlmacen = fechEntAlma;
			if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
			{
				valorFactura = valFact;
				valorIva = valIva;
				valorFletes = valFlts;
				valorIvaFletes = valIvaFlts;
				valorRetenciones = valRtcns;
			}
			else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
			{
				valorFactura = Math.Ceiling(Math.Round(valFact,0));
				valorIva = Math.Ceiling(Math.Round(valIva,0));
				valorFletes = Math.Ceiling(Math.Round(valFlts,0));
				valorIvaFletes = Math.Ceiling(Math.Round(valIvaFlts,0));
				valorRetenciones = Math.Ceiling(Math.Round(valRtcns,0));
			}
			observacion = obs;
			usuario = usr;
			EstructuraDtPagos();
			sqlRels = new ArrayList();
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
        public bool GrabarFacturaProveedor(bool indicadorGrabacion)
        {

            bool status = false;
			int i       = 0;
			sqlStrings  = new ArrayList();
			sqlStrings.Clear();
			valorAbonos = CalcularTotalPagos();
			//Revisamos el numero de la factura y revisamos si ya existe una factura de cliente con el prefijo y numero de factura del objeto
			//Si no vamos agregando uno hasta encontrar un numero que no halla sido utilizado
			while(DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura))
				numeroFactura += 1;
			//Primero verficamos que el número de la factura se encuentre dentro del rango permitido para ese prefijo
	//		if(numeroFactura > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura+"'")))
    //            processMsg += "<br>Error : El número especificado se encuentra fuera del rango permitido para el prefijo '" + prefijoFactura + "' ";
	//		else

    //     se debe validar en cada módulo en el momento de seleccioanr el documento

			{
				//Ahora creamos el registro dentro de mfacturaroveedor con la informacion ingresada a este objeto
				if(fechaPago.ToString("yyyy-MM-dd") != "0001-01-01")
				{
					sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+nitProveedor+"','"+prefijoDocumentoRelacion+"',"+numeroDocumentoRelacion+","+
					               										 "'"+fechaFactura.ToString("yyyy-MM-dd")+"','"+codigoAlmacen+"','"+fechaEntradaAlmacen.ToString("yyyy-MM-dd")+"','"+flagFactura+"','"+vigencia+"',"+
					               						     			 "'"+fechaVencimiento.ToString("yyyy-MM-dd")+"','"+fechaPago.ToString("yyyy-MM-dd")+"',"+valorFactura+","+valorIva+","+valorFletes+","+
					               									  	 ""+valorIvaFletes+","+valorRetenciones+","+valorAbonos+",'"+observacion+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
				}
				else
				{
					sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+nitProveedor+"','"+prefijoDocumentoRelacion+"',"+numeroDocumentoRelacion+","+
					               										 "'"+fechaFactura.ToString("yyyy-MM-dd")+"','"+codigoAlmacen+"','"+fechaEntradaAlmacen.ToString("yyyy-MM-dd")+"','"+flagFactura+"','"+vigencia+"',"+
					               						     			 "'"+fechaVencimiento.ToString("yyyy-MM-dd")+"',null,"+valorFactura+","+valorIva+","+valorFletes+","+
					               									  	 ""+valorIvaFletes+","+valorRetenciones+","+valorAbonos+",'"+observacion+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
				}

				//Ahora actualizamos el ultimo numero de este prefijo si es necesario
				if(numeroFactura > Convert.ToUInt64(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+prefijoFactura+"'")))
					sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroFactura+" WHERE pdoc_codigo='"+prefijoFactura+"'");
				//Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de factura) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de factura
				for(i=0;i<sqlRels.Count;i++)
				{
					string sqlRel = sqlRels[i].ToString();
					sqlRel = sqlRel.Replace("@1",prefijoFactura);
					sqlRel = sqlRel.Replace("@2",numeroFactura.ToString());
					sqlStrings.Add(sqlRel);
				}
				//Ahora vamos a agregar la relacion de los pagos dentro de la tabla ddetallefacturacliente
				for(i=0;i<dtPagos.Rows.Count;i++)
					sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+dtPagos.Rows[i][0].ToString()+"',"+dtPagos.Rows[i][1].ToString()+","+dtPagos.Rows[i][2].ToString()+",'"+dtPagos.Rows[i][3].ToString()+"')");
				/*for(i=0;i<sqlStrings.Count;i++)
					processMsg += "<br><br>"+sqlStrings[i].ToString();
				status = true;*/
				//Ahora Realizamos la transaccion y verificamos si fue exitosa o no
				if(indicadorGrabacion)
				{
					if(DBFunctions.Transaction(sqlStrings))
					{
						processMsg += "<br>Bien : "+DBFunctions.exceptions;
						status = true;
					}
					else
						processMsg += "<br>Error : "+DBFunctions.exceptions;
				}
				else
					status = true;
			}
			return status;
		}

        public bool GrabarFacturaProveedorExcel(bool indicadorGrabacion)
        {
            
            bool status = false;
            int i = 0;
            sqlStrings = new ArrayList();
            sqlStrings.Clear();
            valorAbonos = CalcularTotalPagos();
            //Revisamos el numero de la factura y revisamos si ya existe una factura de cliente con el prefijo y numero de factura del objeto
            //Si no vamos agregando uno hasta encontrar un numero que no halla sido utilizado
            while (DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefijoFactura + "' AND mfac_numeordepago=" + numeroFactura))
                numeroFactura += 1;
            //Primero verficamos que el número de la factura se encuentre dentro del rango permitido para ese prefijo
            if (numeroFactura > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura + "'")))
                processMsg += "<br>Error : El número especificado se encuentra fuera del rango permitido para el prefijo '" + prefijoFactura + "' ";
            else
            {
                //Ahora creamos el registro dentro de mfacturacliente con la informacion ingresada a este objeto
                if (fechaPago.ToString("yyyy-MM-dd") != "0001-01-01")
                {
                    sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + nitProveedor + "','" + prefijoDocumentoRelacion + "'," + numeroDocumentoRelacion + "," +
                                                                            "'" + fechaFactura.ToString("yyyy-MM-dd") + "','" + codigoAlmacen + "','" + fechaEntradaAlmacen.ToString("yyyy-MM-dd") + "','" + flagFactura + "','" + vigencia + "'," +
                                                                             "'" + fechaVencimiento.ToString("yyyy-MM-dd") + "','" + fechaPago.ToString("yyyy-MM-dd") + "'," + valorFactura + "," + valorIva + "," + valorFletes + "," +
                                                                              "" + valorIvaFletes + "," + valorRetenciones + "," + valorAbonos + ",'" + observacion + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                }
                else
                {
                    sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + nitProveedor + "','" + prefijoDocumentoRelacion + "'," + numeroDocumentoRelacion + "," +
                                                                            "'" + fechaFactura.ToString("yyyy-MM-dd") + "','" + codigoAlmacen + "','" + fechaEntradaAlmacen.ToString("yyyy-MM-dd") + "','" + flagFactura + "','" + vigencia + "'," +
                                                                             "'" + fechaVencimiento.ToString("yyyy-MM-dd") + "',null," + valorFactura + "," + valorIva + "," + valorFletes + "," +
                                                                              "" + valorIvaFletes + "," + valorRetenciones + "," + valorAbonos + ",'" + observacion + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                }
                //Ahora actualizamos el ultimo numero de este prefijo si es necesario
                if (numeroFactura > Convert.ToUInt64(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefijoFactura + "'")))
                    sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=" + numeroFactura + " WHERE pdoc_codigo='" + prefijoFactura + "'");
                //Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de factura) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de factura
                for (i = 0; i < sqlRels.Count; i++)
                {
                    string sqlRel = sqlRels[i].ToString();
                    sqlRel = sqlRel.Replace("@1", prefijoFactura);
                    sqlRel = sqlRel.Replace("@2", numeroFactura.ToString());
                    sqlStrings.Add(sqlRel);
                }
                if (dtPagos.Rows.Count == 0)
                {
                    DataRow fila = dtPagos.NewRow();
                    fila[0] = prefijoFactura;
                    fila[1] = numeroFactura;
                    fila[2] = valorFactura;
                    fila[3] = observacion;
                    dtPagos.Rows.Add(fila);
                }
                //Ahora vamos a agregar la relacion de los pagos dentro de la tabla ddetallefacturacliente
                for (i = 0; i < dtPagos.Rows.Count; i++)
                    sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + dtPagos.Rows[i][0].ToString() + "'," + dtPagos.Rows[i][1].ToString() + "," + dtPagos.Rows[i][2].ToString() + ",'" + dtPagos.Rows[i][3].ToString() + "')");
                /*for(i=0;i<sqlStrings.Count;i++)
					processMsg += "<br><br>"+sqlStrings[i].ToString();
				status = true;*/
                //Ahora Realizamos la transaccion y verificamos si fue exitosa o no
                if (indicadorGrabacion)
                {
                    if (DBFunctions.Transaction(sqlStrings))
                    {
                        processMsg += "<br>Bien : " + DBFunctions.exceptions;
                        status = true;
                    }
                    else
                        processMsg += "<br>Error : " + DBFunctions.exceptions;
                }
                else
                    status = true;
            }
            return status;
        }

        #endregion
    }
}
