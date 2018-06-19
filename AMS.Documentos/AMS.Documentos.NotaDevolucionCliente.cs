using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;
using AMS.Utilidades;

namespace AMS.Documentos
{
	public class NotaDevolucionCliente
	{
		#region Atributos

        private string prefijoNota, prefijoFactura, flagIndicativo, vigencia, flagTipDoc, almacen, nit, vigenciaFactura;
		private uint numeroNota, numeroFactura;
        private double valorNota, valorIva, valorRetenciones, valorFletes = 0, valorIvaFletes = 0;
		private bool indicativoUsoValorNota,indicativoNotaAdmin=false;
		private DateTime fechaNota;
		DataTable dtIva,dtRetenciones;
		private string usuario, processMsg;
		private ArrayList sqlRels, sqlStrings, sqlRetcs;
        public static string observacionDevolucion;
		#endregion
		
		#region Propiedades

		public string PrefijoNota{get{return prefijoNota;}}
		public string PrefijoFactura{get{return prefijoFactura;}}
		public string FlagIndicativo{get{return flagIndicativo;}}
		public string Vigencia{get{return vigencia;}}
		public string VigenciaFactura{get{return vigenciaFactura;}}
		public string FlagTipDoc{get{return flagTipDoc;}}
        public string ObservacionDevolucion { set { observacionDevolucion = value; } get { return observacionDevolucion; } }
		public uint NumeroNota{get{return numeroNota;}}
		public uint NumeroFactura{get{return numeroFactura;}}
		public double ValorNota{get{return valorNota;}}
		public DateTime FechaNota{get{return fechaNota;}}
		public string Usuario{get{return usuario;}}
		public string ProcessMsg{get{return processMsg;}}
		public ArrayList SqlRels{set{sqlRels = value;}get{return sqlRels;}}
		public ArrayList SqlStrings{get{return sqlStrings;}}
		
		#endregion
		
		#region Constructor

		public NotaDevolucionCliente()
		{
			prefijoNota = "";
			prefijoFactura = "";
			flagIndicativo = "N";
			vigencia = "V";
			sqlRels = new ArrayList();
			indicativoUsoValorNota = false;
            observacionDevolucion = "";
		}
		
        //  nota devolucion Cliente Vehículo
		public NotaDevolucionCliente(
			string prefND, // 0
			string prefFC, // 1
			uint numND, // 2
			uint numFC, // 3
			string flgInd, // 4 
			string flgTD, // 5 
			double valAbon, // 6
			string vig, // 7 
			string vigFac, // 8 
			DateTime fechND, // 9 
			string usr, // 10
			bool indUsoVal // 11
			)
		{
			prefijoNota     = prefND;
			prefijoFactura  = prefFC; 
			numeroNota      = numND;
			numeroFactura   = numFC;
			flagIndicativo  = flgInd;
			flagIndicativo  = "N";  // SI ES NOTA DE RECAJA va con prefijo N
			flagTipDoc      = flgTD;
			valorNota       = valAbon; // <El valor de la nota es es igual al valor de la factura. El valor abonado de la nota y de la factura es el saldo de la factura.>
			vigencia        = vig;
			vigenciaFactura = vigFac;
			fechaNota       = fechND;
			usuario         = usr;
			indicativoUsoValorNota = indUsoVal;
			sqlRels         = new ArrayList();
		}
		
		/// <summary>
		/// Constructor para Notas Cliente realizadas con Recibos de Caja
		/// </summary>
		/// <param name="prefND">string Prefijo de la Nota</param>
		/// <param name="prefRC">string Prefijo del Recibo de Caja</param>
		/// <param name="numND">uint Número de la Nota</param>
		/// <param name="numRC">uint Número del Recibo de Caja</param>
		/// <param name="flgInd">string Flag Indicativo de Nota (NDC)</param>
		/// <param name="vlrNot">double Valor de la Nota</param>
		/// <param name="flgTD">string Flag Indicativo de Documento Relacionado (RC)</param>
		/// <param name="vig">string Vigencia de la Nota</param>
		/// <param name="fechND">DateTime Fecha de la Nota</param>
		/// <param name="usr">string Usuario que crea la Nota</param>
		/// <param name="alm">string Almacen Almacen donde se crea la nota</param>
		/// <param name="nt">string Nit del Beneficiario de la Nota</param>
		public NotaDevolucionCliente(
			string prefND, // 0
			string prefRC, // 1 
			uint numND, // 2 
			uint numRC, // 3 
			string flgInd, // 4
			double vlrNot, // 5 
			string flgTD, // 6 
			string vig , // 7
			DateTime fechND, // 8 
			string usr, // 9
			string alm, // 10 
			string nt // 11
			)
		{
			prefijoNota     = prefND;
			prefijoFactura  = prefRC;
			numeroNota      = numND;
			numeroFactura   = numRC;
			flagIndicativo  = flgInd;
			flagTipDoc      = flgTD;
			vigencia        = vig;
			fechaNota       = fechND;
			valorNota       = vlrNot;
			usuario         = usr;
			almacen         = alm;
			nit             = nt;
			sqlRels         = new ArrayList();
			indicativoUsoValorNota = false;
		}
		
		/// <summary>
		/// Constructor para las devoluciones de administrativas
		/// </summary>
		/// <param name="prefND">string prefijo de la nota</param>
		/// <param name="prefFac">string prefijo de la factura</param>
		/// <param name="numND">uint número de la nota</param>
		/// <param name="numFac">uint número de la factura</param>
		/// <param name="flgInd">string flag que indica si es una nota (N)</param>
		/// <param name="flgTD">string flag que indica como se trabajará la devolucion (FA)</param>
		/// <param name="valNot">double valor sin iva y sin retenciones de la nota</param>
		/// <param name="valIva">double valor del iva de la nota</param>
		/// <param name="valRet">double valor de las retenciones de la nota</param>
		/// <param name="fec">DateTime fecha de la nota</param>
		/// <param name="usu">string usuario que crea la nota</param>
		public NotaDevolucionCliente(
			string prefND, // 0
			string prefFac,// 1
			uint numND,    // 2
			uint numFac,   // 3
			string flgInd, // 4
			string flgTD,  // 5
			double valNot, // 6
			double valIva, // 7
			double valRet, // 8
			DateTime  fec, // 9
			string    usu, // 10
            DataTable dtR
			)
		{
			prefijoNota     = prefND;
			numeroNota      = numND;
			prefijoFactura  = prefFac;
			numeroFactura   = numFac;
			flagIndicativo  = flgInd;
			flagTipDoc      = flgTD;
			valorNota       = valNot;
			valorIva        = valIva;
			valorRetenciones= valRet;
			fechaNota       = fec;
			usuario         = usu;
			sqlRels         = new ArrayList();
            dtRetenciones   = dtR;
		}

		/// <summary>
		/// Constructor para las devoluciones de administrativas
		/// </summary>
		/// <param name="prefND">string prefijo de la nota</param>
		/// <param name="prefFac">string prefijo de la factura</param>
		/// <param name="numND">uint número de la nota</param>
		/// <param name="numFac">uint número de la factura</param>
		/// <param name="flgInd">string flag que indica si es una nota (N)</param>
		/// <param name="flgTD">string flag que indica como se trabajará la devolucion (FA)</param>
		/// <param name="valNot">double valor sin iva y sin retenciones de la nota</param>
		/// <param name="valIva">double valor del iva de la nota</param>
		/// <param name="valRet">double valor de las retenciones de la nota</param>
		/// <param name="fec">DateTime fecha de la nota</param>
		/// <param name="usu">string usuario que crea la nota</param>
		/// <param name="usu">Tabla IVAS</param>
		/// <param name="usu">Tabla Retenciones</param>
		public NotaDevolucionCliente(
			string prefND, // 0
			string prefFac,// 1
			uint numND,    // 2
			uint numFac,   // 3
			string flgInd, // 4
			string flgTD,  // 5
			double valNot, // 6
			double valIva, // 7
			double valRet, // 8
			double valFlet,
			double valIvaF,
			DateTime fec,  // 9
			string usu,    // 10
			DataTable dtI,
			DataTable dtR, string nt
			)
		{
			prefijoNota     =prefND;
			numeroNota      =numND;
			prefijoFactura  =prefFac;
			numeroFactura   =numFac;
			flagIndicativo  =flgInd;
			flagTipDoc      =flgTD;
			valorNota       =valNot;
			valorIva        =valIva;
			valorFletes     =valFlet;
			valorIvaFletes  =valIvaF;
			valorRetenciones=valRet;
			fechaNota       =fec;
			usuario         =usu;
			sqlRels         =new ArrayList();
			dtIva           =dtI;
			dtRetenciones   =dtR;
			nit             =nt;
			indicativoNotaAdmin=true;
		}

		#endregion
		
		#region Metodos
		
		//Función que nos permite la grabacion de una nota de devolucion a cliente 
		//Parametros : Un Boleano que nos indica si se realizara o no la grabacion directa en la base de datos
		//Retorno : Un Boleano que nos indica se se tuvo exito o no en el proceso
      
		public bool GrabarNotaDevolucionCliente(bool guardar)
		{
			processMsg  = "";
			bool status = false;
			int i       = 0;
			sqlStrings  = new ArrayList();
			sqlStrings.Clear();
            sqlRetcs = new ArrayList();
            sqlRetcs.Clear();

            // desde aqui, esto sobra porque el parámetro ya trae el numero de la nota
            // traemos el numero de nota que sigue y revisamos que ese numero de la nota no haya sido utilizado 
            uint notaAcce = numeroNota;
            string nitEmpresa = DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA");
            if (flagTipDoc == "RC" && nitEmpresa == "890924684") // en los ANTICIPOS por RECAJA de SOMERAUTO el numero de la nota es el mismo consecutivo generado en el recibo de caja
                numeroNota = numeroFactura;
            else
            {
                if(numeroNota <= 1)
                {
                    numeroNota = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ULTIDOCU FROM pdocumento WHERE pdoc_codigo='" + prefijoNota + "'"));
                    numeroNota += 1;
                }
                
            }
            //if(notaAcce == 1)          //Aumenta el doble y genera inconcistencia con el consecutivo
            //    numeroNota += 1;
            //   hasta aqui

            while(DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='"+prefijoNota+"' AND mfac_numedocu="+numeroNota+""))
				numeroNota += 1;
			//Revisamos que el numero de la nota se encuentre dentro del rango permitido
			if(numeroNota > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'")) || numeroNota < Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_numeinic FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'")) )
                processMsg += "<br>Error : El número especificado se encuentra fuera del rango permitido para el prefijo '" + prefijoNota + "' ";
			else
			{
                string observacionDevo = ObservacionDevolucion == null ? "" : ObservacionDevolucion.ToString();
				switch (flagTipDoc)
				{
					case "FC":
					{
						//Revisamos si existe o no la factura que vamos a devolver
						if(Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"")) > 0)
						{
							//Ahora traemos la informacion que necesitamos de la factura que se va a anular
							DataSet dsFactura = new DataSet();
							DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, pcen_codigo, pven_codigo FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+" ");
							//														  0				1				2			3				4			5					6			7			  8				9
							double valorFactura = Convert.ToDouble(dsFactura.Tables[0].Rows[0]["mfac_valofact"]);
							double valorIVA     = Convert.ToDouble(dsFactura.Tables[0].Rows[0]["mfac_valoiva"]);
							double valorFletes  = Convert.ToDouble(dsFactura.Tables[0].Rows[0]["mfac_valoflet"]);
							double valorIVAFletes = Convert.ToDouble(dsFactura.Tables[0].Rows[0]["mfac_valoivaflet"]);
							double valorRetenciones = Convert.ToDouble(dsFactura.Tables[0].Rows[0]["mfac_valorete"]);
							double valorAbono   = Convert.ToDouble(dsFactura.Tables[0].Rows[0]["mfac_valoabon"]);
                       //     valorAbono = valorAbono - valorNota; // se resta el anticipo al vehiculo porque los SQL aun no se ejecutan
							double valorSaldo   = 0; // Siempre Cero.
						
							//if(indicativoUsoValorNota) // < Para documentar.
							//	valorSaldo = valorNota;
							//else
							//	valorSaldo = valorFactura + valorIVA + valorFletes + valorIVAFletes - valorRetenciones - valorAbono;
						
							if(!(valorFactura==this.valorNota && valorIVA==this.valorIva))
							{
								valorFletes=valorIVAFletes=0;
							}
							//Ahora creamos el registro dentro de mfacturacliente que representa la nota de devolucion
                            if (observacionDevo != null || observacionDevo != "")
                            {
                                sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefijoNota + "'," + numeroNota + ",'" + dsFactura.Tables[0].Rows[0]["mnit_nit"].ToString() + "','" + dsFactura.Tables[0].Rows[0]["palm_almacen"].ToString() + "','" + flagIndicativo + "','" + vigencia + "'," +
                                "'" + fechaNota.ToString("yyyy-MM-dd") + "','" + fechaNota.ToString("yyyy-MM-dd") + "',null," + valorFactura.ToString() + "," + valorIVA.ToString() + "," + dsFactura.Tables[0].Rows[0]["MFAC_VALOFLET"].ToString() + "," + dsFactura.Tables[0].Rows[0]["MFAC_VALOIVAFLET"].ToString() + "," + valorRetenciones.ToString() + "," + valorSaldo.ToString() + ",0,0," +
                                "'" + dsFactura.Tables[0].Rows[0]["pcen_codigo"].ToString() + "','" + observacionDevo + "','" + dsFactura.Tables[0].Rows[0]["pven_codigo"].ToString() + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                            }
                            else 
                            {
                                sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefijoNota + "'," + numeroNota + ",'" + dsFactura.Tables[0].Rows[0]["mnit_nit"].ToString() + "','" + dsFactura.Tables[0].Rows[0]["palm_almacen"].ToString() + "','" + flagIndicativo + "','" + vigencia + "'," +
                                "'" + fechaNota.ToString("yyyy-MM-dd") + "','" + fechaNota.ToString("yyyy-MM-dd") + "',null," + valorFactura.ToString() + "," + valorIVA.ToString() + "," + dsFactura.Tables[0].Rows[0]["MFAC_VALOFLET"].ToString() + "," + dsFactura.Tables[0].Rows[0]["MFAC_VALOIVAFLET"].ToString() + "," + valorRetenciones.ToString() + "," + valorSaldo.ToString() + ",0,0," +
                                "'" + dsFactura.Tables[0].Rows[0]["pcen_codigo"].ToString() + "','Nota Devolucion a " + prefijoFactura + "-" + numeroFactura + "|| " + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0]["pven_codigo"].ToString() + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                            }
							

							//Ahora creamos la relacion entre la factura y la nota de devolucion en mnotacliente
							sqlStrings.Add("INSERT INTO mnotacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
						
							Devolucion devolucion = new Devolucion(prefijoFactura,numeroFactura,prefijoNota,numeroNota);
							devolucion.AplicarNotaCreditoAFactura();

							ManejoArrayList.InsertarArrayListEnArrayList(devolucion.SqlStrings,sqlStrings);

                            AgregarIvasRetenciones(sqlStrings);
							//Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de devolcion) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de nota devolucion
							for(i=0;i<sqlRels.Count;i++)
							{
								string sqlRel = sqlRels[i].ToString();
								sqlRel = sqlRel.Replace("@1",prefijoNota);
								sqlRel = sqlRel.Replace("@2",numeroNota.ToString());
								sqlStrings.Add(sqlRel);
							}
						
						}
						else
							processMsg += "<br>Error : La factura que solicitada para la devolucion no existe";
					}//fc
						break;
					case "RC":
					{
                        sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+nit+"','"+almacen+"','"+flagIndicativo+"','"+vigencia+"',"+
							"'"+fechaNota.ToString("yyyy-MM-dd")+"','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+valorNota+",0,0,0,0,0,0,0,"+
							"'"+DBFunctions.SingleData("SELECT pcen_centteso FROM  palmacen WHERE palm_almacen='"+almacen+"'")+"','Nota Anticipo de "+prefijoFactura+"-"+numeroFactura+"',"+
							"'"+DBFunctions.SingleData("SELECT pven_codigo FROM ccartera")+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						//Ahora creamos la relacion entre la factura y la nota de devolucion mnotacliente
						sqlStrings.Add("INSERT INTO mnotacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
                        AgregarIvasRetenciones(sqlStrings);
						
					}//rc
						break;
					case "FA":  // Desde Vehiculos por Anulacion Factura Venta
						#region Anulacion de Facturas de Venta Vehiculos
					{
						double totalFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet+mfac_valoiva+mfac_valoivaflet-mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+""));
                        double netoFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet FROM mfacturaCLIENTE WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numeDOCU=" + numeroFactura + ""));
                        double valorAbonadoFactura =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+""));
						double saldoFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoflet+mfac_valoiva+mfac_valoivaflet-mfac_valorete)-mfac_valoabon FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+""));
                        double valorRetencionesFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturaCLIENTE WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numeDOCU=" + numeroFactura + ""));
                        //Cambio realizado para el prorateo de las retenciones y su respectiva grabación
                        valorRetenciones = ManejoDevoluciones(prefijoFactura, numeroFactura, valorNota, netoFactura, valorRetencionesFactura, ref sqlRetcs);
                        //Fin Cambio realizado para el prorateo de las retenciones y su respectiva grabación
                        double totalNota =valorNota+valorIva-valorRetenciones;
						//Si el saldo de la factura es mayor al valor total de la nota
						//Creo la nota, la dejo cancelada y abono el total de la nota 
						//al valor de la factura
						if(saldoFactura >= totalNota)
						{
							DataSet dsFactura = new DataSet();
							DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, pcen_codigo, pven_codigo FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
							//Ahora creamos el registro dentro de mfacturacliente que representa la nota de devolucion
							if(!indicativoNotaAdmin)
								sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"','"+flagIndicativo+"','C',"+
									"'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',null,"+valorNota+","+valorIva+",0,0,"+valorRetenciones+","+totalNota+",0,0,"+
									"'"+dsFactura.Tables[0].Rows[0][8].ToString()+"','Nota Devolucion a "+prefijoFactura+"-"+numeroFactura + "|| " + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0][9].ToString()+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
							else{
                                if (fechaNota != null)
                                {
                                    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefijoNota + "'," + numeroNota + ",'" + dsFactura.Tables[0].Rows[0][0].ToString() + "','" + dsFactura.Tables[0].Rows[0][1].ToString() + "','" + flagIndicativo + "','C'," +
                                    "'" + fechaNota.ToString("yyyy-MM-dd") + "','" + fechaNota.ToString("yyyy-MM-dd") + "',null," + valorNota + "," + valorIva + "," + valorFletes + "," + valorIvaFletes + "," + valorRetenciones + "," + totalNota + ",0,0," +
                                    "'" + dsFactura.Tables[0].Rows[0][8].ToString() + "','Nota Devolucion a " + prefijoFactura + "-" + numeroFactura + "|| " + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0][9].ToString() + "','" + usuario + "','" + fechaNota.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                                }
                                else 
                                {
                                    sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefijoNota + "'," + numeroNota + ",'" + dsFactura.Tables[0].Rows[0][0].ToString() + "','" + dsFactura.Tables[0].Rows[0][1].ToString() + "','" + flagIndicativo + "','C'," +
                                    "'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',null," + valorNota + "," + valorIva + "," + valorFletes + "," + valorIvaFletes + "," + valorRetenciones + "," + totalNota + ",0,0," +
                                    "'" + dsFactura.Tables[0].Rows[0][8].ToString() + "','Nota Devolucion a " + prefijoFactura + "-" + numeroFactura + "|| " + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0][9].ToString() + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
                                }
                                
                            }
								
							//Si el valor abonado mas la nota es igual al valor total
							//de la factura, dejo la factura en estado cancelado
							if(valorAbonadoFactura+totalNota == totalFactura)
								sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+totalNota+ ",tvig_vigencia='C',MFAC_PAGO = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE pdoc_codigo='" + prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
								//Sino la dejo en estado abonado
							else
								sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+totalNota+ ",tvig_vigencia='A', MFAC_PAGO = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE pdoc_codigo='" + prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
							//Creo el registro que me enlaza la factura con la nota
							sqlStrings.Add("INSERT INTO mnotacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
							sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+prefijoNota+"',"+numeroNota+","+totalNota+",'Devolución a la Factura')");
							sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+","+totalNota+",'Factura Devuelta')");
							
						}
							//Si el saldo de la factura es menor al total de la nota, se abona la factura por el valor
							//del saldo y la nota queda abonada tambien por ese valor
						else if(saldoFactura < totalNota && saldoFactura > 0)
						{
							DataSet dsFactura = new DataSet();
							DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, pcen_codigo, pven_codigo FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
							//Ahora creamos el registro dentro de mfacturacliente que representa la nota de devolucion
							if(!indicativoNotaAdmin)
								sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"','"+flagIndicativo+"','A',"+
								"'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',null,"+valorNota+","+valorIva+",0,0,"+valorRetenciones+","+saldoFactura+",0,0,"+
								"'"+dsFactura.Tables[0].Rows[0][8].ToString()+"','Nota Devolucion a "+prefijoFactura+"-"+numeroFactura + " || " + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0][9].ToString()+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
							else
								sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"','"+flagIndicativo+"','A',"+
								"'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',null,"+valorNota+","+valorIva+","+valorFletes+","+valorIvaFletes+","+valorRetenciones+","+saldoFactura+",0,0,"+
								"'"+dsFactura.Tables[0].Rows[0][8].ToString()+"','Nota Devolucion a "+prefijoFactura+"-"+numeroFactura + " || " + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0][9].ToString()+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
							sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+saldoFactura+ ",tvig_vigencia='C', MFAC_PAGO = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE pdoc_codigo='" + prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
							sqlStrings.Add("INSERT INTO mnotacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
							sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+prefijoNota+"',"+numeroNota+","+saldoFactura+",'Devolución a la Factura')");
							sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+","+saldoFactura+",'Factura Devuelta')");
							
						}
							//Si el saldo factura es menor al total de la nota, pero ese saldo es cero, entonces, no se le
							//hace nada a la factura, y la nota queda vigente sin abonos
						else if(saldoFactura < totalNota && saldoFactura <= 0)
						{
							DataSet dsFactura = new DataSet();
							DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, pcen_codigo, pven_codigo FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura+"");
							//Ahora creamos el registro dentro de mfacturacliente que representa la nota de devolucion
							if(!indicativoNotaAdmin)
                                //Ahora se inserta la observación/Causa de la devolución ...
								sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"','"+flagIndicativo+"','V',"+
								"'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',null,"+valorNota+","+valorIva+",0,0,"+valorRetenciones+",0,0,0,"+
								"'"+dsFactura.Tables[0].Rows[0][8].ToString()+"','Nota Devolucion a "+prefijoFactura+"-"+numeroFactura + " || " + observacionDevolucion + "', '" + dsFactura.Tables[0].Rows[0][9].ToString()+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
							else
								sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"','"+flagIndicativo+"','V',"+
								"'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',null,"+valorNota+","+valorIva+","+valorFletes+","+valorIvaFletes+","+valorRetenciones+",0,0,0,"+
                                "'"+dsFactura.Tables[0].Rows[0][8].ToString()+"','" + observacionDevolucion + "','" + dsFactura.Tables[0].Rows[0][9].ToString() + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
							sqlStrings.Add("INSERT INTO mnotacliente VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
							sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroNota+" WHERE pdoc_codigo='"+prefijoNota+"'");
						}

						AgregarIvasRetenciones(sqlStrings);
                        //Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de devolucion) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de nota devolucion
                        if (sqlRels != null)
                        {
                            for (i = 0; i < sqlRels.Count; i++)
                            {
                                    string sqlRel = sqlRels[i].ToString();
                                    sqlRel = sqlRel.Replace("@1", prefijoNota);
                                    sqlRel = sqlRel.Replace("@2", numeroNota.ToString());
                                    sqlStrings.Add(sqlRel);
                            }
                        }
                        //Ahora agregamos los sqlRetcs(sqls relacionados con la insercion de las RETENCIONES CAUSASDAS VENTA CLIENTE
                        if (dtRetenciones == null)
                        { 
                            for (i = 0; i < sqlRetcs.Count; i++)
                            {
                                sqlRetcs[i] = sqlRetcs[i].ToString().Replace("@1", prefijoNota);
                                sqlRetcs[i] = sqlRetcs[i].ToString().Replace("@2", numeroNota.ToString());
                                sqlStrings.Add(sqlRetcs[i]);
                            }
                        }
					}//fa
                    //CERRAMOS CASE
                        #endregion
                        break;
				}
                //Ahora actualizamos el ultimo numero de este prefijo si es necesario
                if (numeroNota > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefijoNota + "'")))
                    sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=" + numeroNota + " WHERE pdoc_codigo='" + prefijoNota + "'");

                if (guardar)
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

		private void AgregarIvasRetenciones(ArrayList arlSql)
		{
			//IVAS
			if(dtIva!=null)
				for(int i=0;i<dtIva.Rows.Count;i++)
					arlSql.Add("INSERT INTO DFACTURACLIENTEIVA VALUES ("+
						"'"+prefijoNota+"',"+numeroNota+","+
						Convert.ToDouble(dtIva.Rows[i]["PORCENTAJE"]).ToString()+","+
						"'"+nit+"','"+dtIva.Rows[i]["CUENTA"].ToString()+"',"+
						Convert.ToDouble(dtIva.Rows[i]["VALOR"]).ToString()+");");
			//Retenciones
			if(dtRetenciones!=null)
				for(int i=0;i<dtRetenciones.Rows.Count;i++)
                    if (indicativoNotaAdmin)
                    {
                        arlSql.Add("INSERT INTO MFACTURACLIENTERETENCION VALUES (" +
                            " '" + prefijoNota + "'," + numeroNota + "," +
                            " '" + dtRetenciones.Rows[i][0].ToString() + "'," +
                            Convert.ToDouble(dtRetenciones.Rows[i][3]).ToString() + "," + Convert.ToDouble(dtRetenciones.Rows[i][2]).ToString() + ");");
                        // [0] CODIGO DE LA RETENCION        [3] VALOR RETENIDO         [2] BASE DE LA RETENCION
    
                    }
                    else
                    {
                        arlSql.Add("INSERT INTO MFACTURACLIENTERETENCION VALUES (" +
                            " '" + prefijoNota + "'," + numeroNota + "," +
                             " '" + dtRetenciones.Rows[i][2].ToString() + "'," +
                           Convert.ToDouble(dtRetenciones.Rows[i][3]).ToString() + "," + Convert.ToDouble(dtRetenciones.Rows[i][4]).ToString() + ");");
                        // [2] CODIGO DE LA RETENCION        [3] VALOR RETENIDO         [4] BASE DE LA RETENCION
                    }
		}

        private double ManejoDevoluciones(string prefijoFactura, uint numeroFactura, double valorNetoDevolucion, double valorNetoFactura, double valorRetencionesFactura, ref ArrayList sqlRetcs)
        {
            double totalRetencionesDevolver = (valorNetoDevolucion / valorNetoFactura) * valorRetencionesFactura;
            DataSet dsRetencionesFactura = new DataSet();
            DBFunctions.Request(dsRetencionesFactura, IncludeSchema.NO, "SELECT pret_codigo, mfac_valorete, mfac_valobase FROM mfacturaCLIENTEretencion WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu =" + numeroFactura);
            for (int i = 0; i < dsRetencionesFactura.Tables[0].Rows.Count; i++)
            {
                string codigoRetencion = dsRetencionesFactura.Tables[0].Rows[i][0].ToString();

                double valorOriginalRetencion = Convert.ToDouble(dsRetencionesFactura.Tables[0].Rows[i][1].ToString());
                double proRataInstanciaRetencion = (((valorOriginalRetencion * 100) / valorRetencionesFactura) / 100) * totalRetencionesDevolver;
                if (Double.IsNaN(proRataInstanciaRetencion)) proRataInstanciaRetencion = 0;

                double valorOriginalBase = Convert.ToDouble(dsRetencionesFactura.Tables[0].Rows[i][2].ToString());
                double proRataInstanciaBase = (((valorOriginalBase * 100) / valorRetencionesFactura) / 100) * totalRetencionesDevolver;
                if (Double.IsNaN(proRataInstanciaBase)) proRataInstanciaBase = 0;

                sqlRetcs.Add("INSERT INTO mfacturaCLIENTEretencion VALUES ('@1',@2,'" + codigoRetencion + "'," + proRataInstanciaRetencion + "," + proRataInstanciaBase + ")");
            }
            return totalRetencionesDevolver;
        }

        #endregion
    }
}