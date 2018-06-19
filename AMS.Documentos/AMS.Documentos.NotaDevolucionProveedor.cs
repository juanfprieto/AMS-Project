using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;

namespace AMS.Documentos
{
	public class NotaDevolucionProveedor
	{
		#region Atributos
		private string prefijoNota, prefijoFactura, flagIndicativo, vigencia, flagTipDoc, almacen, nit,  vigenciaFactura, observacion;
		private uint numeroNota, numeroFactura;
        private double valorNota, valorIva, valorRetenciones, valorFletes = 0, valorIvaFletes = 0, valorfleteDevolucion = 0, valorivafleteDevolucion = 0;
		private bool indicativoUsoValorNota,indicativoNotaAdmin=false;
		private DateTime fechaNota;
		private string usuario, processMsg;
		private ArrayList sqlRels, sqlStrings;
        public static string observacionDevolucion;
        DataTable dtIva,dtRetenciones;
		#endregion
		
		#region Propiedades
		public string PrefijoNota{get{return prefijoNota;}}
		public string PrefijoFactura{get{return prefijoFactura;}}
		public string FlagIndicativo{get{return flagIndicativo;}}
		public string Vigencia{get{return vigencia;}}
		public string VigenciaFactura{get{return vigenciaFactura;}}
		public string FlagTipDoc{get{return flagTipDoc;}}
		public uint NumeroNota{get{return numeroNota;}}
		public uint NumeroFactura{get{return numeroFactura;}}
        public string Observacion { set { observacion = value; }  get { return observacion; } }
		public double ValorNota{get{return valorNota;}}
        public double ValorfleteDevolucion { set { valorfleteDevolucion = value; } get { return valorfleteDevolucion; } }
        public double ValorivafleteDevolucion { set { valorivafleteDevolucion = value; } get { return valorivafleteDevolucion; } }
		public DateTime FechaNota{get{return fechaNota;}}
		public string Usuario{get{return usuario;}}
		public string ProcessMsg{get{return processMsg;}}
		public ArrayList SqlRels{set{sqlRels = value;}get{return sqlRels;}}
		public ArrayList SqlStrings{get{return sqlStrings;}}
		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor por Defecto
		/// </summary>
		public NotaDevolucionProveedor()
		{
            prefijoNota = "";
			prefijoFactura = "";
			flagIndicativo = "N";
			vigencia = "V";
			sqlRels = new ArrayList();
			indicativoUsoValorNota = false;
		}
		
		public NotaDevolucionProveedor(string prefND, string prefFP, uint numND, uint numFP, string flgInd,
		                             //0				1			2				3		4
		                             string flgTD, double valAbon, string vig, string vigFac, DateTime fechND, string usr,
									 //5					6			7				8			9				10
									 bool indUsoVal)
									//11
		{
			prefijoNota = prefND;
			prefijoFactura = prefFP;
			numeroNota = numND;
			numeroFactura = numFP;
			flagIndicativo = flgInd;
			flagTipDoc = flgTD;
			if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
			valorNota = valAbon;
			else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
				valorNota = Math.Round(valAbon,0);
			vigencia = vig;
			vigenciaFactura = vigFac;
			fechaNota = fechND;
			usuario = usr;
			indicativoUsoValorNota = indUsoVal;
			sqlRels = new ArrayList();
		}
		
		/// <summary>
		/// Constructor para Notas de Comprobantes de Egreso
		/// </summary>
		/// <param name="prefND">string Prefijo de la Nota</param>
		/// <param name="prefRC">string Prefijo del Comprobante de Egreso</param>
		/// <param name="numND">uint Número de la Nota</param>
		/// <param name="numRC">uint Número del Comprobante de Egreso</param>
		/// <param name="flgInd">string Flag Indicativo (N)</param>
		/// <param name="vlrNot">double Valor de la Nota</param>
		/// <param name="flgTD">string Tipo de Documento Enlazado (CE)</param>
		/// <param name="vig">string Vigencia de la Nota</param>
		/// <param name="fechND">DateTime Fecha de Creación de la Nota</param>
		/// <param name="usr">string Usuario que creo la nota</param>
		/// <param name="alm">string Almacen</param>
		/// <param name="nt">string Nit de la Nota</param>
		public NotaDevolucionProveedor(string prefND, string prefRC, uint numND, uint numRC, string flgInd,
		                             //0				1			2				3		4
		                             double vlrNot, string flgTD, string vig ,DateTime fechND, string usr,
		                             	//5					6			7				8				9
		                             string alm, string nt)
										//10		11	
		{
            prefijoNota = prefND;
			prefijoFactura = prefRC;
			numeroNota = numND;
			numeroFactura = numRC;
			flagIndicativo = flgInd;
			flagTipDoc = flgTD;
			vigencia = vig;
			fechaNota = fechND;
			if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
				valorNota = vlrNot;
			else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
				    valorNota = Math.Round(vlrNot,0);
			usuario = usr;
			almacen = alm;
			nit = nt;
			sqlRels = new ArrayList();
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
		public NotaDevolucionProveedor(string prefND,string prefFac,uint numND,uint numFac,string flgInd,string flgTD,
            double valNot, double valIva, double valRet, DateTime fec, string usu)
		{
            prefijoNota=prefND;
			numeroNota=numND;
			prefijoFactura=prefFac;
			numeroFactura=numFac;
			flagIndicativo=flgInd;
			flagTipDoc=flgTD;
			if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
			{
			    valorNota=valNot;
			    valorIva=valIva;
			    valorRetenciones=valRet;
			}
			else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
			{
				valorNota = Math.Round(valNot,0);
				valorIva = Math.Round(valIva,0);
				valorRetenciones = Math.Round(valRet,0);
			}
			fechaNota=fec;
			usuario=usu;
			sqlRels=new ArrayList();
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
		/// <param name="dtI">IVAS</param>
		/// <param name="dtR">Retenciones</param>
		public NotaDevolucionProveedor(string prefND,string prefFac,uint numND,uint numFac,string flgInd,string flgTD,
			double valNot,double valIva,double valRet,double valFlet,double valIvaF,DateTime fec,string usu, DataTable dtI, DataTable dtR, string nt)
		{
			prefijoNota=prefND;
			numeroNota=numND;
			prefijoFactura=prefFac;
			numeroFactura=numFac;
			flagIndicativo=flgInd;
			flagTipDoc=flgTD;
			if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "S")
			{
				valorNota=valNot;
				valorIva=valIva;
				valorRetenciones=valRet;
			}
			else if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
			{
				valorNota = Math.Round(valNot,0);
				valorIva = Math.Round(valIva,0);
				valorRetenciones = Math.Round(valRet,0);
			}
			valorFletes=valFlet;
			valorIvaFletes=valIvaF;
			fechaNota=fec;
			usuario=usu;
			dtIva=dtI;
			dtRetenciones=dtR;
			nit=nt;
			indicativoNotaAdmin=true;
			sqlRels=new ArrayList();
		}

		#endregion
		
		#region Metodos
		
		private void AgregarIvasRetenciones(ArrayList arlSql)
		{
			//IVAS
			if(dtIva!=null)
				for(int i=0;i<dtIva.Rows.Count;i++)
					arlSql.Add("INSERT INTO DFACTURAPROVEEDORIVA VALUES ("+
						"'"+prefijoNota+"',"+numeroNota+","+
						Convert.ToDouble(dtIva.Rows[i]["PORCENTAJE"]).ToString()+","+
						"'"+ dtIva.Rows[i]["NIT"].ToString() + "','"+dtIva.Rows[i]["CUENTA"].ToString()+"',"+
						Convert.ToDouble(dtIva.Rows[i]["VALOR"]).ToString()+");");
			//Retenciones
			if(dtRetenciones!=null)
				for(int i=0;i<dtRetenciones.Rows.Count;i++)
					arlSql.Add("INSERT INTO MFACTURAPROVEEDORRETENCION VALUES ("+
						"'"+prefijoNota+"',"+numeroNota+","+
						"'"+dtRetenciones.Rows[i]["CODRET"].ToString()+"',"+
						Convert.ToDouble(dtRetenciones.Rows[i]["VALOR"]).ToString()+ "," +
                        Convert.ToDouble(dtRetenciones.Rows[i]["VALORBASE"]).ToString() + ");");
		}
		//Función que nos permite la grabación de una nota de devolución a cliente 
		//Parametros : Un Boleano que nos indica si se realizara o no la grabacion directa en la base de datos
		//Retorno :    Un Boleano que nos indica se se tuvo exito o no en el proceso
		public bool GrabarNotaDevolucionProveedor(bool guardar)
		{
			processMsg  = "";
			bool status = false;
			int i       = 0;
			sqlStrings  = new ArrayList();
			ArrayList sqlRetenciones = new ArrayList();
			sqlStrings.Clear();
            
            //Revisamos que el numero de la nota no haya sido utilizado 
            while (DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoNota+"' AND mfac_numeordepago="+numeroNota+""))
				numeroNota += 1;
			//Revisamos que el numero de la nota se encuentre dentro del rango permitido
			if(numeroNota > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'")))
                processMsg += "<br>Error : El número especificado se encuentra fuera del rango permitido para el prefijo '" + prefijoNota + "' ";
			else
            {
                if (Observacion == null) Observacion = "";

                if (flagTipDoc == "FP")
				{
					//Factura del proveedor
					if(DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""))
					{
                        DataSet dsretencion = new DataSet();
                        double valorRete, valorBase;
                        string codigoRete = "0";
                        DataSet dsFactura = new DataSet();
						DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, mfac_fechentrada FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						//														  0				1				2			3				4			5					6			7				8
						double valorFactura = Convert.ToDouble(dsFactura.Tables[0].Rows[0][2]);
						double valorIVA = Convert.ToDouble(dsFactura.Tables[0].Rows[0][3]);
						double valorFletes = Convert.ToDouble(dsFactura.Tables[0].Rows[0][4]);
						double valorIVAFletes = Convert.ToDouble(dsFactura.Tables[0].Rows[0][5]);
						double valorRetenciones = Convert.ToDouble(dsFactura.Tables[0].Rows[0][6]);
						double valorAbono = Convert.ToDouble(dsFactura.Tables[0].Rows[0][7]);
						if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
						{
							valorFactura = Math.Round(valorFactura,0);
							valorIVA = Math.Round(valorIVA,0);
							valorFletes = Math.Round(valorFletes,0);
							valorIVAFletes = Math.Round(valorIVAFletes,0);
							valorRetenciones = Math.Round(valorRetenciones,0);
							valorAbono = Math.Round(valorAbono,0);
						}
                        double saldoFactura = valorFactura + valorIVA - valorFletes + valorIVAFletes - valorRetenciones - valorAbono;
                        double saldoAbonar = valorNota;
                        if (saldoFactura < valorNota)
                            saldoAbonar = saldoFactura;   // se abona a la nota y a la factura el menor saldo 
                        if (saldoFactura == valorNota)
                            vigencia = "C";
                        else
                            vigencia = "A";

                        //   CAMILO AQUI DEBE GRABAR LA FECHA DE LA NOTA QUE SE PIDE EN LA PANTALLA NO LA FECHA DEL DIA
                        //  y los registros del pago en DCAJAPROVEEDOR o como se llame la tabla de los soportes de pago de proveedores
                        string observacionNota = DBFunctions.SingleData("SELECT 'Devolucion a '|| MFAC_PREFDOCU || ' - ' || MFAC_nUMEDOCU || ' - ' || MFAC_OBSERVACION FROM MFACTURAPROVEEDOR WHERE PDOC_CODIORDEPAGO = '" + prefijoFactura + "' AND MFAC_NUMEORDEPAGO = " + numeroFactura + "");
                        observacionNota += "." + observacion;
                        if(indicativoUsoValorNota)
						{
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
								"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','"+vigencia+"','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
                                "" + valorFactura.ToString() + "," + valorIVA.ToString() + ",0,0," + valorRetenciones.ToString() + "," + saldoAbonar.ToString() + ",'" + observacionNota + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
						}
						else
						{
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+almacen+"',"+
								"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','"+vigencia+"','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
                                "" + valorFactura.ToString() + "," + valorIVA.ToString() + ",0,0," + valorRetenciones.ToString() + "," + saldoAbonar.ToString() + ",'" + observacionNota + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
						}
                                              
                        sqlStrings.Add("INSERT INTO mnotaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");

                        //GUARDAR RETENCIONES EN mfacturaproveedorretencion

                        try
                        {
                            DBFunctions.Request(dsretencion, IncludeSchema.NO, "select pret_codigo, MFAC_VALORETE, MFAC_VALOBASE from mfacturaproveedorretencion where pdoc_codiordepago='" + prefijoFactura + "' AND mfac_numeordepago=" + numeroFactura + "");
                            if (dsretencion.Tables[0].Rows.Count >= 1)
                            {
                                for (int j = 0; j < dsretencion.Tables[0].Rows.Count; j++)
                                {
                                    codigoRete = Convert.ToString(dsretencion.Tables[0].Rows[j][0]);
                                    valorRete = Convert.ToInt32(dsretencion.Tables[0].Rows[j][1]);
                                    valorBase = Convert.ToInt32(dsretencion.Tables[0].Rows[j][2]);
                                    sqlStrings.Add("insert into DBXSCHEMA.mfacturaproveedorretencion values ('" + prefijoNota + "'," + numeroNota + ", '" + codigoRete + "'," + valorRete + "," + valorBase + ")");
                                }
                            }
                        }
                        catch
                        {
                        }

                        if (indicativoUsoValorNota)
							sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='" + vigenciaFactura + "', mfac_valoabon = mfac_valoabon + " + saldoAbonar.ToString() + " WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						else
                            sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='" + vigenciaFactura + "', mfac_valoabon = mfac_valoabon + " + saldoAbonar.ToString() + " WHERE pdoc_codiordepago='" + prefijoFactura + "' AND mfac_numeordepago=" + numeroFactura + "");
						if(numeroNota > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'")))
							sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroNota+" WHERE pdoc_codigo='"+prefijoNota+"'");
                        sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('" + prefijoFactura + "'," + numeroFactura + ",'" + prefijoNota + "'," + numeroNota + "," + saldoAbonar.ToString() + ",'Devolución a la Factura')");
                        sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('" + prefijoNota + "'," + numeroNota + ",'" + prefijoFactura + "'," + numeroFactura + "," + saldoAbonar.ToString() + ",'Factura Devuelta')");
		
                        AgregarIvasRetenciones(sqlStrings);
						//Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de devolcion) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de nota devolucion
						for(i=0;i<sqlRels.Count;i++)
						{
							string sqlRel = sqlRels[i].ToString();
							sqlRel = sqlRel.Replace("@1",prefijoNota);
							sqlRel = sqlRel.Replace("@2",numeroNota.ToString());
							sqlStrings.Add(sqlRel);
						}
						//Ahora realizamos la grabacion si es solicitada
						if(guardar)
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
					else
						processMsg += "<br>Error : La factura que solicitada para la devolucion no existe";
				}
				else if(flagTipDoc == "CE")
				{
					sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+nit+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+almacen+"','"+fechaNota.ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','"+vigencia+"','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+valorNota+",0,0,0,0,0,'Nota Anticipo de "+prefijoFactura+"-"+numeroFactura+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
					//Ahora creamos la relacion entre la factura y la nota de devolucion mnotacliente
					sqlStrings.Add("INSERT INTO mnotaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
					if(numeroNota > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'")))
						sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroNota+" WHERE pdoc_codigo='"+prefijoNota+"'");
					AgregarIvasRetenciones(sqlStrings);
					if(guardar)
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
				else if(flagTipDoc=="FA")
				{
                    double valorRetencionDevolucion = 0;
                    double totalFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet+mfac_valoiva+mfac_valoivaflet-mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
					double netoFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
					double valorAbonadoFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
					double saldoFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoflet+mfac_valoiva+mfac_valoivaflet-mfac_valorete)-mfac_valoabon FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
					double valorRetencionesFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+""));
					double totalNota=valorNota+valorIva-valorRetenciones;
                    //Cambio realizado para el prorateo de las retenciones y su respectiva grabación
                    if(valorRetencionesFactura != 0)
                    {
                        valorRetencionDevolucion = ManejoDevoluciones(prefijoFactura, numeroFactura, valorNota, netoFactura, valorRetencionesFactura, ref sqlRetenciones);
                    }
                    else { valorRetencionDevolucion = 0; }
					//Fin Cambio realizado para el prorateo de las retenciones y su respectiva grabación
					if(DBFunctions.SingleData("SELECT cemp_liqudeci FROM cempresa") == "N")
					{
						totalFactura = Math.Round(totalFactura,0);
						valorAbonadoFactura = Math.Round(valorAbonadoFactura,0);
						saldoFactura = Math.Round(saldoFactura,0);
						totalNota = valorNota+valorIva-valorRetenciones;
						valorRetencionDevolucion = Math.Round(valorRetencionDevolucion,0);
					}
					//Si el saldo de la factura es mayor al valor total de la nota
					//Creo la nota, la dejo cancelada y abono el total de la nota 
					//al valor de la factura
					if(saldoFactura>=totalNota)
					{
						DataSet dsFactura = new DataSet();
						DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, mfac_fechentrada FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						//Ahora creamos el registro dentro de mfacturaproveedor que representa la nota de devolucion
						/*sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
							"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','C','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
							""+valorNota+","+valorIva+",0,0,"+valorRetenciones+","+totalNota+",'Nota Devolución Proveedor a "+prefijoFactura+"-"+numeroFactura+"','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");*/
						if(!indicativoNotaAdmin)
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
								"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','C','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
								""+valorNota+","+valorIva+"," + ValorfleteDevolucion + "," + valorivafleteDevolucion + "," + valorRetencionDevolucion+","+totalNota+",'Nota Devolución Proveedor a "+prefijoFactura+"-"+numeroFactura + "|| " + Observacion + "','" + usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						else
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
								"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','C','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
								""+valorNota+","+valorIva+","+valorFletes+","+valorIvaFletes+","+valorRetenciones+","+totalNota+",'" + Observacion + "','"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						//Si el valor abonado mas la nota es igual al valor total
						//de la factura, dejo la factura en estado cancelado
						if(valorAbonadoFactura+totalNota == totalFactura)
							sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+totalNota+",tvig_vigencia='C' WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
							//Sino la dejo en estado abonado
						else
							sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+totalNota+",tvig_vigencia='A' WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						//Creo el registro que me enlaza la factura con la nota
						sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+prefijoNota+"',"+numeroNota+","+totalNota+",'Devolución a la Factura')");
						sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+","+totalNota+",'Factura Devuelta')");
						sqlStrings.Add("INSERT INTO mnotaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
						sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroNota+" WHERE pdoc_codigo='"+prefijoNota+"'");
					}
					//Si el saldo de la factura es menor al total de la nota y el saldo es mayor a cero
					//abono la factura por su saldo y la cancelo y la nota la abono por el valor del saldo
					//de la factura
					else if(saldoFactura<totalNota && saldoFactura>0)
					{
						DataSet dsFactura = new DataSet();
						DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, mfac_fechentrada FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						//Ahora creamos el registro dentro de mfacturaproveedor que representa la nota de devolucion
						if(!indicativoNotaAdmin)
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
							"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','A','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
							""+valorNota+","+valorIva+",0,0,"+valorRetencionDevolucion+","+saldoFactura+",'Nota Devolución Proveedor a "+prefijoFactura+"-"+numeroFactura + "|| " + Observacion + "','" + usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						else
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
							"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','A','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
							""+valorNota+","+valorIva+","+valorFletes+","+valorIvaFletes+","+valorRetenciones+","+saldoFactura+",'Nota Devolución Proveedor a "+prefijoFactura+"-"+numeroFactura + "|| " + Observacion + "','" + usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+saldoFactura+",tvig_vigencia='C' WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('"+prefijoFactura+"',"+numeroFactura+",'"+prefijoNota+"',"+numeroNota+","+saldoFactura+",'Devolución a la Factura')");
						sqlStrings.Add("INSERT INTO ddetallefacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+","+saldoFactura+",'Factura Devuelta')");
						sqlStrings.Add("INSERT INTO mnotaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
						sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroNota+" WHERE pdoc_codigo='"+prefijoNota+"'");
					}
					//Si el saldo de la factura es menor al total de la nota, pero ese saldo es cero,
					//no le hago nada ni a la nota ni a la factura
					else if(saldoFactura<totalNota && saldoFactura<=0)
					{
						DataSet dsFactura = new DataSet();
						DBFunctions.Request(dsFactura,IncludeSchema.NO,"SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, mfac_fechentrada FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFactura+"' AND mfac_numeordepago="+numeroFactura+"");
						//Ahora creamos el registro dentro de mfacturaproveedor que representa la nota de devolucion
						if(!indicativoNotaAdmin)
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
							"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','V','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
							""+valorNota+","+valorIva+","+valorfleteDevolucion+","+valorivafleteDevolucion+","+valorRetencionDevolucion+",0,'Nota Devolución Proveedor a "+prefijoFactura+"-"+numeroFactura + "|| " + Observacion + "','" + usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						else
							sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+dsFactura.Tables[0].Rows[0][0].ToString()+"','"+prefijoFactura+"',"+numeroFactura+",'"+fechaNota.ToString("yyyy-MM-dd")+"','"+dsFactura.Tables[0].Rows[0][1].ToString()+"',"+
							"'"+Convert.ToDateTime(dsFactura.Tables[0].Rows[0][8]).ToString("yyyy-MM-dd")+"','"+flagIndicativo+"','V','"+fechaNota.ToString("yyyy-MM-dd")+"',null,"+
							""+valorNota+","+valorIva+","+valorFletes+","+valorIvaFletes+","+valorRetenciones+",0,'Nota Devolución Proveedor a "+prefijoFactura+"-"+numeroFactura + "|| " + Observacion + "','" + usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
						sqlStrings.Add("INSERT INTO mnotaproveedor VALUES('"+prefijoNota+"',"+numeroNota+",'"+prefijoFactura+"',"+numeroFactura+")");
						sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroNota+" WHERE pdoc_codigo='"+prefijoNota+"'");
					}
					AgregarIvasRetenciones(sqlStrings);
					//Ahora agregamos los sqlRels(sqls relacionados con la insercion del tipo de devolucion) y cambiamos los caracteres comodin @1 y @2 por los valores de prefijo y numero de nota devolucion
					for(i=0;i<sqlRels.Count;i++)
					{
						string sqlRel = sqlRels[i].ToString();
						sqlRel = sqlRel.Replace("@1",prefijoNota);
						sqlRel = sqlRel.Replace("@2",numeroNota.ToString());
						sqlStrings.Add(sqlRel);
					}
					//Ahora agregamos los sqlRels relacionados con la insercion de las retenciones
					if(dtRetenciones==null)
						for(i=0;i<sqlRetenciones.Count;i++)
						{
							string sqlRel = sqlRetenciones[i].ToString();
							sqlRel = sqlRel.Replace("@1",prefijoNota);
							sqlRel = sqlRel.Replace("@2",numeroNota.ToString());
							sqlStrings.Add(sqlRel);
						}
					if(guardar)
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
			}
			return status;
		}


		private double ManejoDevoluciones(string prefijoFactura, uint numeroFactura, double valorNetoDevolucion, double valorNetoFactura, double valorRetencionesFactura, ref ArrayList sqlRels)
		{
			double totalRetencionesDevolver = (valorNetoDevolucion/valorNetoFactura)*valorRetencionesFactura;
			DataSet dsRetencionesFactura = new DataSet();
            DBFunctions.Request(dsRetencionesFactura, IncludeSchema.NO, "SELECT pret_codigo, mfac_valorete, mfac_valobase FROM mfacturaproveedorretencion WHERE pdoc_codiordepago='" + prefijoFactura + "' AND mfac_numeordepago=" + numeroFactura);
			for(int i=0;i<dsRetencionesFactura.Tables[0].Rows.Count;i++)
			{
				string codigoRetencion = dsRetencionesFactura.Tables[0].Rows[i][0].ToString();

				double valorOriginalRetencion = Convert.ToDouble(dsRetencionesFactura.Tables[0].Rows[i][1].ToString());
				double proRataInstanciaRetencion = (((valorOriginalRetencion*100)/valorRetencionesFactura)/100)*totalRetencionesDevolver;
                if (Double.IsNaN(proRataInstanciaRetencion)) proRataInstanciaRetencion = 0;

                double valorOriginalBase = Convert.ToDouble(dsRetencionesFactura.Tables[0].Rows[i][2].ToString());
                double proRataInstanciaBase = (((valorOriginalBase * 100) / valorRetencionesFactura) / 100) * totalRetencionesDevolver;
                if (Double.IsNaN(proRataInstanciaBase)) proRataInstanciaBase = 0;

				sqlRels.Add("INSERT INTO mfacturaproveedorretencion VALUES ('@1',@2,'" + codigoRetencion + "'," + proRataInstanciaRetencion + "," + proRataInstanciaBase + ")");
			}
			return totalRetencionesDevolver;
		}
		#endregion
	}
}
