using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;
using AMS.Contabilidad;

namespace AMS.Finanzas.Tesoreria
{
	public class Recibo
	{
		protected string prefijoRecibo,tipoRecibo,datCli,datBen,concepto,almacen,usuario,fecha,mensajes,fechaCreacion,claseRecibo,prefijoFactura,flujoGeneral,flujoEspecifico,prefijoNotaCliente,prefijoNotaProveedor,prefijoAnulacion;
		protected int    numeroRecibo;
		protected double valorBruto,valorNeto,valorAbonado,valorAbonadoNC,valorAbonadoFactura,saldo,saldoDisponible,valorObligaciones;
		protected DataTable facturasaPagar,tablaNoCaus,tablaPagos,tablaRetenciones,tablaAbonos,tablaCheques,tablaAbonosDev;
		protected DataSet notasDevolucionCliente,notasDevolucionProveedor,notasCliente;
		protected ArrayList sqlPagosDev,cheques;
		protected bool grabacionCheques=false, operacionEliminar=false;
		private Consignacion miTransferencia=new Consignacion();
		public DataTable dtObligaciones;
        protected double valorBase = 0, valorSobrante = 0;
        ProceHecEco contaOnline = new ProceHecEco();
		
		public string PrefijoRecibo{set{prefijoRecibo=value;}get{return prefijoRecibo;}}
		public string TipoRecibo{set{tipoRecibo=value;}get{return tipoRecibo;}}
		public string DatCli{set{datCli=value;}get{return datCli;}}
		public string DatBen{set{datBen=value;}get{return datBen;}}
		public string Concepto{set{concepto=value;}get{return concepto;}}
		public string Almacen{set{almacen=value;}get{return almacen;}}
		public string Fecha{set{fecha=value;}get{return fecha;}}
		public string Usuario{set{usuario=value;}get{return usuario;}}
		public string FechaCreacion{set{fechaCreacion=value;}get{return fechaCreacion;}}
		public string Mensajes{set{mensajes=value;}get{return mensajes;}}
		public string ClaseRecibo{set{claseRecibo=value;} get{return claseRecibo;}}
		public string PrefijoFactura{set{prefijoFactura=value;} get{return prefijoFactura;}}
		public string FlujoGeneral{set{flujoGeneral=value;} get{return flujoGeneral;}}
		public string FlujoEspecifico{set{flujoEspecifico=value;} get{return flujoEspecifico;}}
		public string PrefijoNotaCliente{set{prefijoNotaCliente=value;} get{return prefijoNotaCliente;}}
		public string PrefijoNotaProveedor{set{prefijoNotaProveedor=value;} get{return prefijoNotaProveedor;}}
		public string PrefijoAnulacion{set{prefijoAnulacion=value;} get{return prefijoAnulacion;}}
		public int NumeroRecibo{set{numeroRecibo=value;}get{return numeroRecibo;}}
		public double ValorAbonado{set{valorAbonado=value;}get{return valorAbonado;}}
		public double ValorBruto{set{valorBruto=value;}get{return valorBruto;}}
		public double ValorNeto{set{valorNeto=value;}get{return valorNeto;}}
		public double Obligaciones{set{valorObligaciones=value;}get{return valorObligaciones;}}
		public ArrayList SqlPagosDev{set{sqlPagosDev=value;}get{return sqlPagosDev;}}
        public bool OperacionEliminar { set { operacionEliminar = value; } get { return operacionEliminar; } }

		//Constructor por defecto
		public Recibo()
		{
			
		}
		
		public Recibo(DataTable tp,DataTable nc,DataTable p,DataTable rtns,DataTable a,DataTable cp)
		{
			facturasaPagar=new DataTable();
			facturasaPagar=tp;
			tablaNoCaus=new DataTable();
			tablaNoCaus=nc;
			tablaPagos=new DataTable();
			tablaPagos=p;
			tablaRetenciones=new DataTable();
			tablaRetenciones=rtns;
			tablaAbonos=new DataTable();
			tablaAbonos=a;
			tablaCheques=new DataTable();
			tablaCheques=cp;
		}
		
		public Recibo(DataTable tp,DataTable nc,DataTable p,DataTable rtns,DataTable a)
		{
			facturasaPagar=new DataTable();
			facturasaPagar=tp;
			tablaNoCaus=new DataTable();
			tablaNoCaus=nc;
			tablaPagos=new DataTable();
			tablaPagos=p;
			tablaRetenciones=new DataTable();
			tablaRetenciones=rtns;
			tablaAbonosDev=new DataTable();
			tablaAbonosDev=a;
		}
		
		public Recibo(DataTable p)
		{
			tablaPagos=new DataTable();
			tablaPagos=p;
		}
		
		protected void Sacar_Sqls(FacturaCliente factura, ref ArrayList sqlStrings)
		{
			for(int i=0;i<factura.SqlStrings.Count;i++)
				sqlStrings.Add(factura.SqlStrings[i]);
		}
		
		protected void Sacar_Sqls_Notas(NotaDevolucionCliente nota,ref ArrayList sqlStrings)
		{
			for(int i=0;i<nota.SqlStrings.Count;i++)
				sqlStrings.Add(nota.SqlStrings[i]);
		}

		protected void Sacar_Sqls(FacturaProveedor factura, ref ArrayList sqlStrings)
		{
			for(int i=0;i<factura.SqlStrings.Count;i++)
				sqlStrings.Add(factura.SqlStrings[i]);
		}
		
		protected void Sacar_Sqls_Notas(NotaDevolucionProveedor nota,ref ArrayList sqlStrings)
		{
			for(int i=0;i<nota.SqlStrings.Count;i++)
				sqlStrings.Add(nota.SqlStrings[i]);
		}
		
		protected void LlenarArrayList_Encabezado(ref ArrayList sqlStrings)
		{
			sqlStrings.Add("INSERT INTO mcaja VALUES('"+this.prefijoRecibo+"',"+this.numeroRecibo.ToString()+",'"+this.fecha+"','"+this.tipoRecibo+"','"+this.datCli+"','"+this.datBen+"','"+this.concepto+"',"+this.valorBruto.ToString()+","+this.valorNeto.ToString()+",'"+this.almacen+"','"+this.usuario+"','"+this.fechaCreacion+"','A')");
			sqlStrings.Add("INSERT INTO dflujocaja VALUES('"+prefijoRecibo+"',"+numeroRecibo+",'"+flujoGeneral+"','"+flujoEspecifico+"')");
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroRecibo+" WHERE pdoc_codigo='"+this.prefijoRecibo+"'");
		}
		
		protected bool Buscar_Chequera(string chequera)
		{
			bool encontrado=false;
			if(cheques.Count!=0)
			{
				for(int i=0;i<cheques.Count;i++)
				{
					if(cheques[i].ToString()==chequera)
					{
						encontrado=true;
						break;
					}
				}
			}
			return encontrado;
		}
		
		protected bool Verificar_Cheques(ref ArrayList sql)
		{
			bool disp=true;
			cheques=new ArrayList();
			DataRow[] filas;
            int nTrasf = 0;
			//Recorro la tabla de pagos y guardo en un arraylist cada chequera (sin repetir)
			for(int i=0;i<tablaPagos.Rows.Count;i++)
			{
                if (tablaPagos.Rows[i][0].ToString() == "C" || tablaPagos.Rows[i][0].ToString() == "CC")
				{
					if(cheques.Count==0)
						cheques.Add(tablaPagos.Rows[i][1].ToString());
					else
					{
						if(!Buscar_Chequera(tablaPagos.Rows[i][1].ToString()))
							cheques.Add(tablaPagos.Rows[i][1].ToString());
					}
				}
			}
			//Creo un arreglo de enteros y guardo el número de cheques que hayan para esa chequera
			int[] numeros=new int[cheques.Count];
            for (int i = 0; i < cheques.Count && tablaPagos.Rows[i][0].ToString() != "CC"; i++)
			{
				filas=tablaPagos.Select("CODIGOBANCO='"+cheques[i].ToString()+"'");
				numeros[i]=filas.Length;
			}
			//Recorro el arreglo de enteros y miro cual es el ultimo numero girado y sumo ese número al de la cantidad de cheques
			//requeridos, si la suma es menor al ultimo de la chequera, puedo girarlos, sino no.
            for (int i = 0; i < numeros.Length && tablaPagos.Rows[i][0].ToString() != "CC"; i++)
			{
				//  SI LOS VALORES ON NULOS, SE TRAE EL COALESCE POR DEFECTO EN CEROS - Hector Agosto 9 2008
				int ultimo=Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN pche_ultche IS NULL THEN coalesce(pche_numeini-1,0) ELSE coalesce(pche_ultche,0) END FROM dbxschema.pchequera WHERE pche_id="+(cheques[i].ToString().Split('-'))[1]+""));
                try
                {
                    if ((ultimo + numeros[i]) > Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(pche_numefin,0) FROM dbxschema.pchequera WHERE pche_id=" + (cheques[i].ToString().Split('-'))[1] + "")))
                    {
                        disp = false;
                        break;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
                
			}
			if(disp)
			{
                for (int i = 0; i < tablaPagos.Rows.Count; i++)
                {
                    if ((tablaPagos.Rows[i][0].ToString() == "E") || (tablaPagos.Rows[i][0].ToString() == "RIVA") || (tablaPagos.Rows[i][0].ToString() == "RICA") || (tablaPagos.Rows[i][0].ToString() == "RFTE") || (tablaPagos.Rows[i][0].ToString() == "X"))
                        sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "',null,null,null,'" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',null,null,null)");
                    else if ((tablaPagos.Rows[i][0].ToString() == "DC") || (tablaPagos.Rows[i][0].ToString() == "DL"))
                            sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',null,null,null)");
                    else if ((tablaPagos.Rows[i][0].ToString() == "T") || (tablaPagos.Rows[i][0].ToString() == "D"))
                        //	sql.Add("INSERT INTO mcajapago VALUES('"+this.prefijoRecibo+"',"+this.numeroRecibo.ToString()+","+i.ToString()+",'"+tablaPagos.Rows[i][0].ToString()+"','"+tablaPagos.Rows[i][8].ToString()+"','"+(tablaPagos.Rows[i][1].ToString().Split('-'))[0]+"','"+(tablaPagos.Rows[i][1].ToString().Split('-'))[1]+"','"+tablaPagos.Rows[i][2].ToString()+"','"+tablaPagos.Rows[i][7].ToString()+"',"+((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString()))*(System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString()+","+(System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString()+","+Convert.ToDouble(tablaPagos.Rows[i][6].ToString())+",'"+tablaPagos.Rows[i][3].ToString()+"',0,0,null)");
                            sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "',null,'" + (tablaPagos.Rows[i][1].ToString().Split('-'))[1] + "-" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");

                  //  "+(tablaPagos.Rows[i][1].ToString().Split('-'))[1]+"
                    else if (tablaPagos.Rows[i][0].ToString() == "B")
                    {
                        /*Caso re especial, cuando el tipo de pago es transferencia bancaria, inserto
                         * en mcajapago la relación del pago, pero ademas de eso, toca insertar el
                         * registro en mtesoreria, ya que la transf hace directamente el debito o el
                         * credito a la cuenta sin pasar por caja, tambien toca hacer la relación con 
                         * dtesoreriabancaelectronica
                         */
                        sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");
                        //sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',0,0,null)");
                        miTransferencia.CodigoCuenta = (tablaPagos.Rows[i][1].ToString().Split('-'))[1];
                        miTransferencia.ConsecutivoPago = i;
                        miTransferencia.Almacen = almacen;
                        miTransferencia.Detalle = "Transferencia Electrónica realizada con " + prefijoRecibo + "-" + numeroRecibo.ToString();
                        miTransferencia.Fecha = fecha;
                        miTransferencia.NumeroTesoreria = this.numeroRecibo;
                        miTransferencia.PrefijoDocumento = this.prefijoRecibo;
                        miTransferencia.Proceso = this.fechaCreacion;
                        miTransferencia.Usuario = this.usuario;
                        miTransferencia.ValorTransferencia = Convert.ToDouble(tablaPagos.Rows[i][4]) * Convert.ToDouble(tablaPagos.Rows[i][5]);
                        nTrasf += 1;
                   //     for (int n = 0; n < (miTransferencia.Guardar_Transferencia_Bancaria(i)).Count; n++)
                   //         sql.Add((miTransferencia.Guardar_Transferencia_Bancaria(i))[n].ToString());
                        miTransferencia.Guardar_Transferencia_Bancaria(ref sql, i, nTrasf);

                    }
                    else if (tablaPagos.Rows[i][0].ToString() == "C")
                            sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "'," + (tablaPagos.Rows[i][1].ToString().Split('-'))[1] + ",'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");
                    //sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "'," + (tablaPagos.Rows[i][1].ToString().Split('-'))[1] + ",'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',0,0,null)");
                    else if (tablaPagos.Rows[i][0].ToString() == "CC")
                    {
                            string banco = tablaPagos.Rows[i][1].ToString().Split('-')[0];
                            sql.Add("UPDATE MCAJAPAGO SET TEST_ESTADO = 'G' WHERE TTIP_CODIGO = 'C' AND PBAN_CODIGO = '"+banco+"' AND MCPAG_NUMERODOC = '"+tablaPagos.Rows[i][2].ToString()+"' ");
                            // debe grabar la fecha original del cheque recibido del cliente
                            sql.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','N','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");
                    }
                }
                for (int i = 0; i < cheques.Count && tablaPagos.Rows[i][0].ToString() != "CC"; i++)
				{
					sql.Add("UPDATE pchequera SET pche_ultche = CASE WHEN pche_ultche IS NULL THEN "+numeros[i].ToString()+" ELSE pche_ultche+"+numeros[i].ToString()+" END WHERE pche_id="+(cheques[i].ToString().Split('-'))[1]+"");
					numeros[i]=numeros[i]+Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN pche_ultche IS NULL THEN pche_numeini-1 ELSE pche_ultche END FROM pchequera WHERE pche_id="+(cheques[i].ToString().Split('-'))[1]+""));
					if(numeros[i]==Convert.ToInt32(DBFunctions.SingleData("SELECT pche_numefin FROM pchequera WHERE pche_id="+(cheques[i].ToString().Split('-'))[1]+"")))
						sql.Add("UPDATE pchequera SET tvig_vigencia='C' WHERE pche_id="+cheques[i].ToString()+"");
				}

			}
			return disp;
		}
		
		protected void LlenarArrayList_Pagos(ref ArrayList sqlStrings)
		{
			int i=0;
            int nTransf = 0;

			if(tablaPagos!=null)
			{
				if(this.claseRecibo=="CE")
				{
					if(!Verificar_Cheques(ref sqlStrings))
						grabacionCheques=true;
				}
				else
				{
					//Si es una legalización de un provisional, actualizo el estado del cheque del recibo provisional y ademas guardo en el nuevo
					//recibo el pago con estado caja
					if(tipoRecibo=="P")
					{
						for(i=0;i<tablaPagos.Rows.Count;i++)
						{
							if(tablaPagos.Rows[i][8].ToString()=="C")
							{
								sqlStrings.Add("UPDATE mcajapago SET test_estado='L' WHERE mcpag_numerodoc='"+tablaPagos.Rows[i][2].ToString()+"' AND test_estado='C'");
                                sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','C','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");
                              //sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','C','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',0,null,null)");
							}
						}
					}
					//Si es una reconsignacion de un cheque devuelto solo guardo el pago realizado en el nuevo recibo y en el antiguo lo dejo
					//como estaba
					else if(tipoRecibo=="R")
					{
						for(i=0;i<tablaPagos.Rows.Count;i++)
							if(tablaPagos.Rows[i][8].ToString()=="L")
                                sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','C','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,null,null)");
					}
					//Si es cualquier otro tipo de recibo, solo guardo el pago.
					else
					{
						for(i=0;i<tablaPagos.Rows.Count;i++)
						{
							if((tablaPagos.Rows[i][0].ToString()=="E") ||(tablaPagos.Rows[i][0].ToString()=="RIVA")||(tablaPagos.Rows[i][0].ToString()=="RICA")||(tablaPagos.Rows[i][0].ToString()=="RFTE")||(tablaPagos.Rows[i][0].ToString()=="X"))
                                sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "',null,null,null,'" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',null,null,null)");
                                //sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "',null,null,null,'" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',null,null,null)");
                            else if ((tablaPagos.Rows[i][0].ToString() == "DC") || (tablaPagos.Rows[i][0].ToString() == "DL") )
                                    sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',null,null,null)");
							else if(tablaPagos.Rows[i][0].ToString()=="B")
							{
								/*Caso re especial, cuando el tipo de pago es transferencia bancaria, inserto
								* en mcajapago la relación del pago, pero ademas de eso, toca insertar el
								* registro en mtesoreria, ya que la transf hace directamente el debito o el
								* credito a la cuenta sin pasar por caja, tambien toca hacer la relación con 
								* dtesoreriabancaelectronica
								*/
                                sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");
                                //sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + (tablaPagos.Rows[i][1].ToString().Split('-'))[0] + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',0,0,null)");
								miTransferencia.CodigoCuenta=(tablaPagos.Rows[i][1].ToString().Split('-'))[1];
								miTransferencia.ConsecutivoPago=i;
								miTransferencia.Almacen=almacen;
								miTransferencia.Detalle="Transferencia Electronica realizada con "+prefijoRecibo+"-"+numeroRecibo.ToString();
								miTransferencia.Fecha=fecha;
								miTransferencia.NumeroTesoreria=this.numeroRecibo;
								miTransferencia.PrefijoDocumento=this.prefijoRecibo;
								miTransferencia.Proceso=this.fechaCreacion;
								miTransferencia.Usuario=this.usuario;
                             	miTransferencia.ValorTransferencia=Convert.ToDouble(tablaPagos.Rows[i][4])*Convert.ToDouble(tablaPagos.Rows[i][5]);
                                nTransf += 1;
                   //             for (int n=0;n< (miTransferencia.Guardar_Transferencia_Bancaria(nTransf)).Count; n++)
								miTransferencia.Guardar_Transferencia_Bancaria (ref sqlStrings, i, nTransf);
							}
							else
                                sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][3].ToString() + "',0,0,null)");
                                //sqlStrings.Add("INSERT INTO mcajapago VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + "," + i.ToString() + ",'" + tablaPagos.Rows[i][0].ToString() + "','" + tablaPagos.Rows[i][8].ToString() + "','" + tablaPagos.Rows[i][1].ToString() + "',null,'" + tablaPagos.Rows[i][2].ToString() + "','" + tablaPagos.Rows[i][7].ToString() + "'," + ((System.Convert.ToDouble(tablaPagos.Rows[i][4].ToString())) * (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString()))).ToString() + "," + (System.Convert.ToDouble(tablaPagos.Rows[i][5].ToString())).ToString() + "," + Convert.ToDouble(tablaPagos.Rows[i][6].ToString()) + ",'" + tablaPagos.Rows[i][9].ToString() + "',0,0,null)");
						}
					}
				}

                //Registro de saldos de Tesoreria RC
                if (tablaPagos.Rows.Count > 0)
                {
                    Consignacion saldosTesoreria = new Consignacion(tablaPagos);
                    saldosTesoreria.RegistrarSaldoCaja(this.usuario+'@'+fecha, this.claseRecibo, -1);
                }
			}
		}
		
		protected void LlenarArrayList_Retenciones(ref ArrayList sqlStrings)
		{
            if(facturasaPagar != null)
            {
                for (int k = 0; k < facturasaPagar.Rows.Count; k++)
                {
                    if (this.claseRecibo == "RC")
                        try
                        {
                            valorBase += Convert.ToDouble(DBFunctions.SingleData("select mfac_valofact from mfacturacliente where pdoc_codigo='" + facturasaPagar.Rows[k][2].ToString() + "' and mfac_numedocu=" + facturasaPagar.Rows[k][3].ToString() + ";"));
                        }
                        catch
                        {
                        }
                }
            }

            if (tablaRetenciones != null )
            {
                for (int i = 0; i < tablaRetenciones.Rows.Count; i++)
                    sqlStrings.Add("INSERT INTO mcajaretencion VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + ",'" + this.tablaRetenciones.Rows[i][0].ToString() + "'," + System.Convert.ToDouble(this.tablaRetenciones.Rows[i][2].ToString()).ToString() + "," + valorBase + ")");
            }
        }
		
		protected void LlenarArrayList_NoCausados(ref ArrayList sqlStrings,ref double valorAbonado)
		{
			int i;
			valorSobrante=0;
			if(tablaNoCaus!=null)
			{
				for(i=0;i<tablaNoCaus.Rows.Count;i++)
				{
					//Si es debito y tiene valor base
					if(((Convert.ToDouble(tablaNoCaus.Rows[i][7].ToString()))!=0)&&((Convert.ToDouble(tablaNoCaus.Rows[i][9].ToString()))!=0))
						sqlStrings.Add("INSERT INTO dcajavarios VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+tablaNoCaus.Rows[i][6].ToString().Trim()+"','"+tablaNoCaus.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaNoCaus.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaNoCaus.Rows[i][1].ToString().Trim()+"','"+tablaNoCaus.Rows[i][0].ToString().Trim()+"','"+tablaNoCaus.Rows[i][2].ToString().Trim()+"','"+tablaNoCaus.Rows[i][3].ToString().Trim()+"',"+System.Convert.ToDouble(tablaNoCaus.Rows[i][7].ToString()).ToString()+",'D',"+System.Convert.ToDouble(tablaNoCaus.Rows[i][9].ToString()).ToString()+")");
					//Si es debito y no tiene valor base
					else if(((System.Convert.ToDouble(tablaNoCaus.Rows[i][7].ToString()))!=0)&&((System.Convert.ToDouble(tablaNoCaus.Rows[i][9].ToString()))==0))
						sqlStrings.Add("INSERT INTO dcajavarios VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+tablaNoCaus.Rows[i][6].ToString().Trim()+"','"+tablaNoCaus.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaNoCaus.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaNoCaus.Rows[i][1].ToString().Trim()+"','"+tablaNoCaus.Rows[i][0].ToString().Trim()+"','"+tablaNoCaus.Rows[i][2].ToString().Trim()+"','"+tablaNoCaus.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaNoCaus.Rows[i][7].ToString())).ToString()+",'D',0)");
					//Si es credito y tiene valor base
					else if(((System.Convert.ToDouble(tablaNoCaus.Rows[i][8].ToString()))!=0)&&((System.Convert.ToDouble(tablaNoCaus.Rows[i][9].ToString()))!=0))
						sqlStrings.Add("INSERT INTO dcajavarios VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+tablaNoCaus.Rows[i][6].ToString().Trim()+"','"+tablaNoCaus.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaNoCaus.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaNoCaus.Rows[i][1].ToString().Trim()+"','"+tablaNoCaus.Rows[i][0].ToString().Trim()+"','"+tablaNoCaus.Rows[i][2].ToString().Trim()+"','"+tablaNoCaus.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaNoCaus.Rows[i][8].ToString())).ToString()+",'C',"+System.Convert.ToDouble(tablaNoCaus.Rows[i][9].ToString()).ToString()+")");
					//Si es credito y no tiene valor base
					else if(((System.Convert.ToDouble(tablaNoCaus.Rows[i][8].ToString()))!=0)&&((System.Convert.ToDouble(tablaNoCaus.Rows[i][9].ToString()))==0))
						sqlStrings.Add("INSERT INTO dcajavarios VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+tablaNoCaus.Rows[i][6].ToString().Trim()+"','"+tablaNoCaus.Rows[i][4].ToString().Trim()+"',"+System.Convert.ToInt32(tablaNoCaus.Rows[i][5].ToString().Trim()).ToString()+",'"+tablaNoCaus.Rows[i][1].ToString().Trim()+"','"+tablaNoCaus.Rows[i][0].ToString().Trim()+"','"+tablaNoCaus.Rows[i][2].ToString().Trim()+"','"+tablaNoCaus.Rows[i][3].ToString().Trim()+"',"+(System.Convert.ToDouble(tablaNoCaus.Rows[i][8].ToString())).ToString()+",'C',0)");
				}
				for(i=0;i<tablaNoCaus.Rows.Count;i++)
				{
                    if (this.claseRecibo == "RC")
					{
						if((Convert.ToDouble(tablaNoCaus.Rows[i][7].ToString()))!=0)
							valorSobrante=valorSobrante+Convert.ToDouble(tablaNoCaus.Rows[i][7])*-1;
						else if((Convert.ToDouble(tablaNoCaus.Rows[i][8].ToString()))!=0)
							    valorSobrante=valorSobrante+Convert.ToDouble(tablaNoCaus.Rows[i][8]);
					}
					else if(this.claseRecibo=="CE")
					{
						if((Convert.ToDouble(tablaNoCaus.Rows[i][7].ToString()))!=0)
							valorSobrante=valorSobrante+Convert.ToDouble(tablaNoCaus.Rows[i][7]);
						else if((Convert.ToDouble(tablaNoCaus.Rows[i][8].ToString()))!=0)
							    valorSobrante=valorSobrante+Convert.ToDouble(tablaNoCaus.Rows[i][8])*-1;
					}
				}
                valorAbonado = valorAbonado - valorSobrante;
                if (this.tipoRecibo == "V" && (tablaAbonos.Rows.Count > 0)) // descarta los abonos de los pedidos de vehiculos
                {
                    for (i = 0; i < tablaAbonos.Rows.Count; i++)
                        valorAbonado = valorAbonado - Convert.ToDouble((tablaAbonos.Rows[i][4].ToString()).ToString());
                }
				saldo=valorAbonado;
			}
		}
		
		protected void LlenarArrayList_Documentos(ref ArrayList sqlStrings, ref double valorAbonado)
		{
			int i;
			double valorFactura=0,valorAboFac=0;
			FacturaCliente miFactura=new FacturaCliente();
			NotaDevolucionCliente miNota=new NotaDevolucionCliente();
			if(facturasaPagar!=null)
			{
				if(this.claseRecibo=="RC")
				{
					//Recorro la tabla para saber cuales son las que restan del cruce segun el tipo de recibo y
					//adicionar ese valor al saldoDisponible
					for(i=0;i<facturasaPagar.Rows.Count;i++)
						if((facturasaPagar.Rows[i][0].ToString()=="Cliente" && facturasaPagar.Rows[i][6].ToString()=="N") || (facturasaPagar.Rows[i][0].ToString()=="Proveedor" && facturasaPagar.Rows[i][6].ToString()=="F"))
						    saldoDisponible=saldoDisponible+Convert.ToDouble(facturasaPagar.Rows[i][5]);
					saldo=saldoDisponible+this.valorAbonado;
					//Ya teniendo el valor de lo que puedo abonar, empiezo a abonar y cancelar documentos
					for(i=0;i<facturasaPagar.Rows.Count;i++)
					{
						if(facturasaPagar.Rows[i][0].ToString()=="Cliente")
						{
							//Si la factura es de cliente y es una nota, abono de una vez su valor a su saldo y reviso, si
							//el valor abonado es igual al valor total de la factura, la cancelo, sino solo la dejo en estado abonado
							if(facturasaPagar.Rows[i][6].ToString()=="N")
							{
								valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
								valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
								sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
								if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
									sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
								else
									sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
								sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+",'"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+","+(System.Convert.ToDouble(facturasaPagar.Rows[i][5].ToString())).ToString()+",'Abono realizado con "+prefijoRecibo+" - "+numeroRecibo+"')");
							}
							else if(facturasaPagar.Rows[i][6].ToString()=="F")
							{
								if(saldo>0)
								{
									if(saldo>=Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+",'"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+","+(System.Convert.ToDouble(facturasaPagar.Rows[i][5].ToString())).ToString()+",'Abono realizado con "+prefijoRecibo+" - "+numeroRecibo+"')");
										saldo=saldo-Convert.ToDouble(facturasaPagar.Rows[i][5]);
									}
									else if(saldo<Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+saldo+",mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+saldo))
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+",'"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+","+saldo+",'Abono realizado con "+prefijoRecibo+" - "+numeroRecibo+"')");
										saldo=0;
									}
								}
							}
						}
						else if(facturasaPagar.Rows[i][0].ToString()=="Proveedor")
						{
							if(facturasaPagar.Rows[i][6].ToString()=="N")
							{
								if(saldo>0)
								{
									if(saldo>=Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO dcajaproveedor VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+","+Convert.ToDouble(facturasaPagar.Rows[i][5])+")");
										saldo=saldo-Convert.ToDouble(facturasaPagar.Rows[i][5]);
									}
									else if(saldo<Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+saldo+",mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+saldo))
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO dcajaproveedor VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+","+saldo+")");
										saldo=0;
									}
								}
							}
							else if(facturasaPagar.Rows[i][6].ToString()=="F")
							{
								valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
								valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
								sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
								if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
									sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
								else
									sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
								sqlStrings.Add("INSERT INTO dcajaproveedor VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+","+Convert.ToDouble(facturasaPagar.Rows[i][5])+")");
							}
						}
					}
				}
				else if(this.claseRecibo=="CE")
				{
					for(i=0;i<facturasaPagar.Rows.Count;i++)
						if((facturasaPagar.Rows[i][0].ToString()=="Cliente" && facturasaPagar.Rows[i][6].ToString()=="F") || (facturasaPagar.Rows[i][0].ToString()=="Proveedor" && facturasaPagar.Rows[i][6].ToString()=="N"))
						    saldoDisponible=saldoDisponible+Convert.ToDouble(facturasaPagar.Rows[i][5]);
					saldo=saldoDisponible+this.valorAbonado;
					for(i=0;i<facturasaPagar.Rows.Count;i++)
					{
						if(facturasaPagar.Rows[i][0].ToString()=="Cliente")
						{
							//Si la factura es de cliente y es una nota, abono de una vez su valor a su saldo y reviso, si
							//el valor abonado es igual al valor total de la factura, la cancelo, sino solo la dejo en estado abonado
							if(facturasaPagar.Rows[i][6].ToString()=="F")
							{
								valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
								valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
								sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
								if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
									sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
								else
									sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
								sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+",'"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+","+Convert.ToDouble(facturasaPagar.Rows[i][5])+",'Abono realizado con "+prefijoRecibo+" - "+numeroRecibo+"')");
							}
							else if(facturasaPagar.Rows[i][6].ToString()=="N")
							{
								if(saldo>0)
								{
									if(saldo>=Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valorete) FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+",'"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+","+Convert.ToDouble(facturasaPagar.Rows[i][5])+",'Abono realizado con "+prefijoRecibo+" - "+numeroRecibo+"')");
										saldo=saldo-Convert.ToDouble(facturasaPagar.Rows[i][5]);
									}
									else if(saldo<Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturacliente WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon+"+saldo+",mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+saldo))
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codigo='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numedocu="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO ddetallefacturacliente VALUES('"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+",'"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+","+saldo+",'Abono realizado con "+prefijoRecibo+" - "+numeroRecibo+"')");
										saldo=0;
									}
								}
							}
						}
						else if(facturasaPagar.Rows[i][0].ToString()=="Proveedor")
						{
							if(facturasaPagar.Rows[i][6].ToString()=="F")
							{
								if(saldo>0)
								{
									if(saldo>=Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO dcajaproveedor VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+","+Convert.ToDouble(facturasaPagar.Rows[i][5])+")");
										saldo=saldo-Convert.ToDouble(facturasaPagar.Rows[i][5]);
									}
									else if(saldoDisponible<Convert.ToDouble(facturasaPagar.Rows[i][5]))
									{
										valorAboFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
										sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+saldo+",mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										if(valorFactura==(valorAboFac+saldo))
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										else
											sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
										sqlStrings.Add("INSERT INTO dcajaproveedor VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+","+saldo+")");
										saldo=0;
									}
								}
							}
							else if(facturasaPagar.Rows[i][6].ToString()=="N")
							{
								valorAboFac =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
								valorFactura=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM  mfacturaproveedor WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+""));
								sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon+"+Convert.ToDouble(facturasaPagar.Rows[i][5])+",mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
								if(valorFactura==(valorAboFac+Convert.ToDouble(facturasaPagar.Rows[i][5])))
									sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
								else
									sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A',mfac_pago='"+this.fecha+"' WHERE pdoc_codiordepago='"+facturasaPagar.Rows[i][2].ToString()+"' AND mfac_numeordepago="+facturasaPagar.Rows[i][3].ToString()+"");
								sqlStrings.Add("INSERT INTO dcajaproveedor VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+facturasaPagar.Rows[i][2].ToString().Trim()+"',"+System.Convert.ToInt32(facturasaPagar.Rows[i][3].ToString().Trim()).ToString()+","+Convert.ToDouble(facturasaPagar.Rows[i][5])+")");
							}
						}
					}
				}
			}
		}

		private void LlenarArrayList_NotaSobrante(ref ArrayList sqlStrings)
		{
			NotaDevolucionCliente miNota=new NotaDevolucionCliente();
			NotaDevolucionProveedor miNotaP=new NotaDevolucionProveedor();
			//Si sobro plata despues de hacer todos los abonos, genero una nota a favor del cliente o del proveedor
			if(saldo>=(System.Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(ccar_valorfrontera,0) FROM ccartera"))))
			{
				//Si es un recibo de caja, creo la nota de devolucion en mfacturacliente
                if (this.claseRecibo == "RC" && this.tipoRecibo != "A")  // no anticipo
				{
					string prfNot=DBFunctions.SingleData("SELECT pdoc_codigo FROM ccartera");
					uint numNot=Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prfNot+"'"));
					miNota=new NotaDevolucionCliente(prfNot,prefijoRecibo,numNot,Convert.ToUInt32(numeroRecibo),"N",saldo,"RC","V",Convert.ToDateTime(fecha),usuario,almacen,datBen);
					//									0		  1			2						3			  4		5	  6	  7				8				  9		  10	 11
					if(miNota.GrabarNotaDevolucionCliente(false))
						this.Sacar_Sqls_Notas(miNota,ref sqlStrings);
					else
						sqlStrings.Add(miNota.ProcessMsg);
				}
					//Si es un comp. de egreso genero la nota de devolucion en mfacturaproveedor
				else if(this.claseRecibo=="CE" && this.tipoRecibo!="G")  // no anticipo
				{
					 string prefNot=DBFunctions.SingleData("SELECT pdoc_codirdepago FROM ccartera");
					 uint numNot=Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefNot+"'"));
					 miNotaP=new NotaDevolucionProveedor(prefNot,prefijoRecibo,numNot,Convert.ToUInt32(numeroRecibo),"N",saldo,"CE","V",DateTime.Now.Date,usuario,almacen,datBen);
					 if(miNotaP.GrabarNotaDevolucionProveedor(false))
						this.Sacar_Sqls_Notas(miNotaP,ref sqlStrings);
					 else
						sqlStrings.Add(miNota.ProcessMsg);
				}
			}
		}

		public bool Guardar_Recibo()
		{
			ArrayList sqlStrings=new ArrayList();
			int i;
			FacturaCliente miFactura=new FacturaCliente();
			NotaDevolucionCliente miNota=new NotaDevolucionCliente();
			NotaDevolucionProveedor miNotaP=new NotaDevolucionProveedor();
			bool estado=false;
			string rangoFinal = DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='"+this.prefijoRecibo+"'");
            if(rangoFinal.ToString() == "" || rangoFinal.ToString() == null) // si es nulo se pone en cero
                rangoFinal = "0";
            int numeroFinal = System.Convert.ToInt32(rangoFinal.ToString());
			//Lo primero es verificar que el numero del recibo este dentro del rango y que no exista
			while(DBFunctions.RecordExist("SELECT * FROM dbxschema.mcaja WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+""))
				numeroRecibo++;
			if(this.numeroRecibo > numeroFinal)
				this.mensajes="Error : El número de recibo se encuentra fuera del rango permitido";
			else
			{
				this.LlenarArrayList_Encabezado(ref sqlStrings);
				this.LlenarArrayList_Pagos(ref sqlStrings);
				this.LlenarArrayList_Retenciones(ref sqlStrings);
				this.LlenarArrayList_NoCausados(ref sqlStrings,ref this.valorAbonado);
				this.LlenarArrayList_Documentos(ref sqlStrings,ref this.valorAbonado);
				this.LlenarArrayList_NotaSobrante(ref sqlStrings);
				//Dependiendo del tipo y clase de recibo son las acciones que tomo
				//Si es un recibo de caja o un provisional y es un anticipo de taller
				if(((this.claseRecibo=="RC")&&(this.tipoRecibo=="A")) || ((this.claseRecibo=="RP")&&(this.tipoRecibo=="A")))
				{
                    uint numNot = 0;
                    string nit = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
                    if (nit == "890924684") // SOMERAUTO deja el numero de la nota igual al recaja/egreso
                        numNot = Convert.ToUInt32(numeroRecibo);
                    else
                        numNot = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoNotaCliente + "'"));
                    string observacion = "Préstamo a cliente con " + this.prefijoRecibo + "-" + this.numeroRecibo;
                   	miNota = new NotaDevolucionCliente(prefijoNotaCliente,prefijoRecibo,numNot,Convert.ToUInt32(numeroRecibo),"N",valorBruto,"RC","V",Convert.ToDateTime(fecha),usuario,almacen,datBen);
					if(miNota.GrabarNotaDevolucionCliente(false))
						this.Sacar_Sqls_Notas(miNota,ref sqlStrings);
					else
						sqlStrings.Add(miNota.ProcessMsg);
					sqlStrings.Add("INSERT INTO mcajaanticipo VALUES('"+this.prefijoRecibo.Trim()+"',"+this.numeroRecibo.ToString().Trim()+",'"+prefijoNotaCliente+"',"+numNot+","+this.valorBruto+")");
				}
					//Si es un recibo de caja o un recibo provisional y es un anticipo a vehiculo
				else if(((this.claseRecibo=="RC")&&(this.tipoRecibo=="V")) || ((this.claseRecibo=="RP")&&(this.tipoRecibo=="V")))
				{
					double valorAbonosTotal=0;
                    for (i = 0; i < tablaAbonos.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(tablaAbonos.Rows[i][5]) == true)
                        {
                            if (Convert.ToDouble(tablaAbonos.Rows[i][4].ToString()) > 0) // solo se graba cuando el valor del abono al pedido es mayor a cero
                            { 
                                sqlStrings.Add("INSERT INTO manticipovehiculo VALUES('" + this.prefijoRecibo + "'," + this.numeroRecibo.ToString() + ",'" + tablaAbonos.Rows[i][0].ToString() + "'," + tablaAbonos.Rows[i][1].ToString() + "," + (System.Convert.ToDouble(tablaAbonos.Rows[i][4].ToString())).ToString() + "," + DBFunctions.SingleData("SELECT test_tipoesta FROM mpedidovehiculo WHERE pdoc_codigo='" + tablaAbonos.Rows[i][0].ToString() + "' AND mped_numepedi=" + tablaAbonos.Rows[i][1].ToString() + "") + ",null,null)");
                                valorAbonosTotal = valorAbonosTotal + Convert.ToDouble(tablaAbonos.Rows[i][4].ToString());
                            }
                        }
					}
					if((valorAbonosTotal + valorSobrante)<valorBruto && (valorBruto-(valorAbonosTotal + valorSobrante))>=Convert.ToDouble(DBFunctions.SingleData("SELECT ccar_valorfrontera FROM ccartera")))
					{
						string prfNot=DBFunctions.SingleData("SELECT pdoc_codigo FROM ccartera");
						uint numNot=Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prfNot+"'"));
						miNota=new NotaDevolucionCliente(prfNot,prefijoRecibo,numNot,Convert.ToUInt32(numeroRecibo),"N",valorBruto-(valorAbonosTotal+valorSobrante),"RC","V",Convert.ToDateTime(fecha),usuario,almacen,datBen);
						if(miNota.GrabarNotaDevolucionCliente(false))
							this.Sacar_Sqls_Notas(miNota,ref sqlStrings);
						else
							sqlStrings.Add(miNota.ProcessMsg);
					}
				}
					//Si es un recibo de caja y es una prorroga
				else if((this.claseRecibo=="RC")&&(this.tipoRecibo=="C"))
				{
					if(tablaCheques!=null)
					{
						//Actualizo la fecha de prorroga
						for(i=0;i<tablaCheques.Rows.Count;i++)
						{
							if(Convert.ToBoolean(tablaCheques.Rows[i][10]))
							{
								sqlStrings.Add("UPDATE mcajapago SET mcpag_prorrogas=mcpag_prorrogas+1,mcpag_fechaprorrogas='"+tablaCheques.Rows[i][8].ToString()+"' WHERE pban_codigo='"+tablaCheques.Rows[i][1].ToString()+"' AND mcpag_numerodoc='"+tablaCheques.Rows[i][2].ToString()+"'");
								sqlStrings.Add("INSERT INTO dcajaprorrogas VALUES('"+this.prefijoRecibo+"',"+this.numeroRecibo+",'"+tablaCheques.Rows[i][2].ToString()+"','"+tablaCheques.Rows[i][6].ToString()+"','"+tablaCheques.Rows[i][8].ToString()+"')");
							}
						}
					}
				}
				
					//Si es un comprobante de egreso y es una devolucion
				else if((this.claseRecibo=="CE")&&(this.tipoRecibo=="D"))
				{
					for(i=0;i<tablaAbonosDev.Rows.Count;i++)
					{
						if(Convert.ToBoolean(tablaAbonosDev.Rows[i][4]))
						{
							sqlStrings.Add("UPDATE manticipovehiculo SET test_estado=40 WHERE mped_codigo='"+tablaAbonosDev.Rows[i][0].ToString()+"' AND mped_numepedi="+(System.Convert.ToInt32(tablaAbonosDev.Rows[i][1].ToString())).ToString()+"");
							sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=40,mped_observacion='Pedido Cancelado con el CE : "+this.prefijoRecibo+" - "+this.numeroRecibo+"' WHERE pdoc_codigo='"+tablaAbonosDev.Rows[i][0].ToString()+"' AND mped_numepedi="+(System.Convert.ToInt32(tablaAbonosDev.Rows[i][1].ToString())).ToString()+"");
						}
						if(sqlPagosDev!=null)
						{
							for(int j=0;j<sqlPagosDev.Count;j++)
							{
							//	string sqlRel = sqlPagosDev[i].ToString();
                                string sqlRel = sqlPagosDev[j].ToString();
								sqlRel = sqlRel.Replace("@1",prefijoRecibo);
								sqlRel = sqlRel.Replace("@2",numeroRecibo.ToString());
								sqlStrings.Add(sqlRel);
							}
						}
					}
				}
					//Si es un comprobante de egreso y es un anticipo general
				else if((this.claseRecibo=="CE")&&(this.tipoRecibo=="G"))
				{
					uint numNot=Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoNotaProveedor+"'"));
					miNotaP=new NotaDevolucionProveedor(prefijoNotaProveedor,prefijoRecibo,numNot,Convert.ToUInt32(numeroRecibo),"N",valorBruto,"CE","V",DateTime.Now.Date,usuario,almacen,datBen);
					if(miNotaP.GrabarNotaDevolucionProveedor(false))
						this.Sacar_Sqls_Notas(miNotaP,ref sqlStrings);
					else
						sqlStrings.Add(miNota.ProcessMsg);
				}
					//Si es un comprobante de egreso y es un prestamo a cliente
				else if((this.claseRecibo=="CE")&&(this.tipoRecibo=="S"))
				{
                    Int32 numeroNota = Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + this.prefijoFactura + "'"));
                    string nit = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
                    if (nit == "890924684") // SOMERAUTO deja el numero de la nota igual al egreso
                        numeroNota = this.numeroRecibo;
                    string observacion = "Préstamo a cliente con "+this.prefijoRecibo +"-"+ this.numeroRecibo;
                    miFactura = new FacturaCliente("FC", this.prefijoFactura, this.datBen, this.almacen, "F", Convert.ToUInt32(numeroNota), 0, Convert.ToDateTime(this.fecha), Convert.ToDateTime(this.fecha), Convert.ToDateTime(null), this.valorAbonado, 0, 0, 0, 0, 0, DBFunctions.SingleData("SELECT pcen_codigo FROM cempresa"), observacion, DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"), this.usuario,null);
					if(miFactura.GrabarFacturaCliente(false))
						this.Sacar_Sqls(miFactura,ref sqlStrings);
					else
						sqlStrings.Add(miFactura.ProcessMsg);
					sqlStrings.Add("INSERT INTO mcajaanticipo VALUES('"+this.prefijoRecibo+"',"+this.numeroRecibo+",'"+this.prefijoFactura+"',"+numeroNota+","+this.valorAbonado+")");
				}
				else if((this.claseRecibo=="CE")&&(this.tipoRecibo=="O"))
				{
					//Actualizar pagos
					for(int n=0;n<dtObligaciones.Rows.Count;n++)
					{
						sqlStrings.Add("UPDATE DOBLIGACIONFINANCIERAPLANPAGO SET "+
							"DOBL_MONTPAGO=DOBL_MONTPAGO+"+Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTPESOSED"])+","+
							"DOBL_INTEPAGO=DOBL_INTEPAGO+"+Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTINTERESESED"])+" "+
							"WHERE MOBL_NUMERO='"+dtObligaciones.Rows[n]["MOBL_NUMERO"].ToString()+"' AND "+
							"DOBL_NUMEPAGO="+Convert.ToInt32(dtObligaciones.Rows[n]["DOBL_NUMEPAGO"])+";");
						
						sqlStrings.Add("UPDATE MOBLIGACIONFINANCIERA SET "+
							"MOBL_MONTPAGADO=MOBL_MONTPAGADO+"+Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTPESOSED"])+","+
							"MOBL_INTERESPAGADO="+Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTINTERESESED"])+" "+
							"WHERE MOBL_NUMERO='"+dtObligaciones.Rows[n]["MOBL_NUMERO"].ToString()+"';");
						
						sqlStrings.Add("INSERT INTO DOBLIGACIONFINANCIERAPAGOOBLI VALUES("+
							"'"+this.prefijoRecibo+"',"+this.numeroRecibo+","+
							"'"+dtObligaciones.Rows[n]["MOBL_NUMERO"]+"',"+
							Convert.ToInt32(dtObligaciones.Rows[n]["DOBL_NUMEPAGO"])+","+
							Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_SALDO"])+","+
							Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_INTERESCAUSADO"])+","+
							Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTPESOSED"])+","+
							Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_MONTINTERESESED"])+");");
					}
				}
				if(!grabacionCheques)
				{
					if(DBFunctions.Transaction(sqlStrings))
					{
						estado=true;
                        QueryCache.removeData("mcaja");
                        QueryCache.removeData("mcajapago");
						this.mensajes=DBFunctions.exceptions;
					}
					else
						this.mensajes="Error : "+DBFunctions.exceptions;
					/*for(i=0;i<sqlStrings.Count;i++)
					{
						this.mensajes+=sqlStrings[i].ToString()+"<br>";
						estado=true;
					}*/
				}
				else
					this.mensajes="Imposible grabar el comprobante, se produjo uno de los siguientes errores:<br>Una de las chequeras está cancelada ó <br> No hay disponibilidad de cheques en alguna chequera <br>";
			}
			return estado;
		}
		
		public bool Anular_Recibo(string clase)
		{
			bool estado=false;
			string tipoDocumento=DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='"+this.prefijoRecibo+"'");
			int i,consAnu;
			ArrayList sqlStrings=new ArrayList();
			FacturaCliente miFactura=new FacturaCliente();
			NotaDevolucionCliente miNota=new NotaDevolucionCliente();
			DataSet miDS =new DataSet();
			DataSet miDSP=new DataSet();

            string nitEmpresa = DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA");
            if (nitEmpresa == "890924684")  // Somerauto usa el mismo numero del documento original como numero de la anulacion
                consAnu = this.numeroRecibo;
            else
                consAnu = Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + this.prefijoAnulacion + "'"));
			//Anulo el recibo de caja
            if (operacionEliminar == false)
            {
                sqlStrings.Add("UPDATE mcaja SET test_estadodoc='N', mcaj_razon = 'ANULADO ' CONCAT '" + this.Usuario + " " + DateTime.Now + " ' CONCAT mcaj_razon  WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + "");
            }
            else 
            {
                sqlStrings.Add("UPDATE mcaja SET test_estadodoc='N', mcaj_razon = 'ANULADO POR ' CONCAT '" + this.Usuario + " " + DateTime.Now + " ' CONCAT mcaj_razon  WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + "");
            }

            //Cuando sea un tipo CC.
            DataSet dsReciboCC = new DataSet();
            DBFunctions.Request(dsReciboCC, IncludeSchema.NO, "select pban_codigo, mcpag_numerodoc from mcajapago where pdoc_codigo='" + this.prefijoRecibo + "' and mcaj_numero=" + this.numeroRecibo + " and ttip_codigo='CC';");
            if (dsReciboCC.Tables[0].Rows.Count > 0)
            {
                sqlStrings.Add("update mcajapago set test_estado = 'C' where pban_codigo='" + dsReciboCC.Tables[0].Rows[0][0] + "' and mcpag_numerodoc='" + dsReciboCC.Tables[0].Rows[0][1] + "' and ttip_codigo='C';");
            }

            sqlStrings.Add("UPDATE DDETALLEFACTURACLIENTE SET DDET_OBSER = 'ANULADO ' CONCAT DDET_OBSER WHERE pdoc_codDOCREF='" + this.prefijoRecibo + "' AND DDET_numeDOCU=" + this.numeroRecibo + ""); // completa observacion de ANULACION
            sqlStrings.Add("INSERT INTO manulacioncaja VALUES('" + prefijoAnulacion + "'," + consAnu + ",'" + this.prefijoRecibo + "'," + this.numeroRecibo + ",'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + this.usuario + "')");
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+prefijoAnulacion+"'");
			
            //Anulacion tipo A
			if(clase=="Anticipos (Repuestos/Taller)")
			{
				DBFunctions.Request(miDS,IncludeSchema.NO,"SELECT mfac_codigo,mfac_numedocu FROM mcajaanticipo WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+"");
				if(miDS.Tables[0].Rows.Count!=0)
					for(i=0;i<miDS.Tables[0].Rows.Count;i++)
					    sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_valoabon=(mfac_valofact+mfac_valoiva+mfac_valoivaflet-mfac_valorete) WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
			}
			//Anulacion tipo V
			else if(clase=="Anticipo a Pedido de Vehiculos")
			{
				DataSet ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MFPV.mped_codigo,MFPV.mped_numepedi,MFPV.pdoc_codigo,MFPV.mfac_numedocu,MANT.mant_valorecicaja FROM dbxschema.mfacturapedidovehiculo MFPV,dbxschema.mfacturacliente MFAC,dbxschema.manticipovehiculo MANT,dbxschema.mcaja MCAJ,dbxschema.mpedidovehiculo MPED WHERE MFAC.pdoc_codigo=MFPV.pdoc_codigo AND MFAC.mfac_numedocu=MFPV.mfac_numedocu AND MFPV.mped_codigo=MPED.pdoc_codigo AND MFPV.mped_numepedi=MPED.mped_numepedi AND MPED.pdoc_codigo=MANT.mped_codigo AND MPED.mped_numepedi=MANT.mped_numepedi AND MANT.pdoc_codigo=MCAJ.pdoc_codigo AND MANT.mcaj_numero=MCAJ.mcaj_numero AND MCAJ.pdoc_codigo='"+this.prefijoRecibo+"' AND MCAJ.mcaj_numero="+this.numeroRecibo+" AND MFPV.mfac_tipclie='C' AND MFAC.tvig_vigencia IN ('A','V') AND MFAC.mfac_tipodocu='F'");
				for(int h=0;h<ds.Tables[0].Rows.Count;h++)
					sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon-"+ds.Tables[0].Rows[h][4].ToString()+" WHERE pdoc_codigo='"+ds.Tables[0].Rows[h][2].ToString()+"' AND mfac_numedocu="+ds.Tables[0].Rows[h][3].ToString()+"");
				sqlStrings.Add("UPDATE manticipovehiculo SET test_estado=40 WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+"");
			}
			//Anulacion tipo R
			else if(clase=="Reconsignación Cheques Dev.")
			{
				DBFunctions.Request(miDS,IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,ddet_valodocu FROM ddetallefacturacliente WHERE pdoc_coddocref='"+this.prefijoRecibo+"' AND ddet_numedocu="+this.numeroRecibo+"");
				DBFunctions.Request(miDSP,IncludeSchema.NO,"SELECT pdoc_codiordepago,mfac_numeordepago,dcaj_valorecicaja FROM dcajaproveedor WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+"");
				if(miDS.Tables[0].Rows.Count!=0)
				{
					for(i=0;i<miDS.Tables[0].Rows.Count;i++)
					{
						if(DBFunctions.SingleData("SELECT tvig_vigencia FROM mfacturacliente WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"")=="C")
							sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A' WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
						sqlStrings.Add("UPDATE mfacturacliente SET mfac_valoabon=mfac_valoabon-"+miDS.Tables[0].Rows[i][2].ToString()+" WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
					}
				}
				if(miDSP.Tables[0].Rows.Count!=0)
				{
					for(i=0;i<miDSP.Tables[0].Rows.Count;i++)
					{
						if(DBFunctions.SingleData("SELECT tvig_vigencia FROM mfacturaproveedor WHERE pdoc_codiordepago='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numeordepago="+miDS.Tables[0].Rows[i][1].ToString()+"")=="C")
							sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A' WHERE pdoc_codiordepago='"+miDSP.Tables[0].Rows[i][0].ToString()+"' AND mfac_numeordepago="+miDSP.Tables[0].Rows[i][1].ToString()+"");
						sqlStrings.Add("UPDATE mfacturaproveedor SET mfac_valoabon=mfac_valoabon-"+miDSP.Tables[0].Rows[i][2].ToString()+" WHERE pdoc_codiordepago='"+miDSP.Tables[0].Rows[i][0].ToString()+"' AND mfac_numeordepago="+miDSP.Tables[0].Rows[i][1].ToString()+"");
					}
				}
			}
			//Anulacion tipo P, tipo I y tipo E
			else if((clase=="Legalización de un Provisional")||(clase=="Ingreso Definitivo")||(clase=="Egreso Definitivo"))
			{
				//Anulo varios, facturas (cliente y proveedor)
				DBFunctions.Request(miDS, IncludeSchema.NO,"SELECT pdoc_codigo,mfac_numedocu,ddet_valodocu FROM ddetallefacturacliente WHERE pdoc_coddocref='"+this.prefijoRecibo+"' AND ddet_numedocu="+this.numeroRecibo+"");
				DBFunctions.Request(miDSP,IncludeSchema.NO,"SELECT pdoc_codiordepago,mfac_numeordepago,dcaj_valorecicaja FROM dcajaproveedor WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+"");
				if(miDS.Tables[0].Rows.Count!=0)
				{
					for(i=0;i<miDS.Tables[0].Rows.Count;i++)
					{
					//	if(DBFunctions.SingleData("SELECT tvig_vigencia FROM mfacturacliente WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"")=="C")
					//		sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A' WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
                        sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A', mfac_valoabon=mfac_valoabon-" + miDS.Tables[0].Rows[i][2].ToString() + " WHERE pdoc_codigo='" + miDS.Tables[0].Rows[i][0].ToString() + "' AND mfac_numedocu=" + miDS.Tables[0].Rows[i][1].ToString() + "");
					}
				}
				if(miDSP.Tables[0].Rows.Count!=0)
				{
					for(i=0;i<miDSP.Tables[0].Rows.Count;i++)
					{
					//	if(DBFunctions.SingleData("SELECT tvig_vigencia FROM mfacturaproveedor WHERE pdoc_codiordepago='"+miDSP.Tables[0].Rows[i][0].ToString()+"' AND mfac_numeordepago="+miDSP.Tables[0].Rows[i][1].ToString()+"")=="C")
					//	 sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A' WHERE pdoc_codiordepago='"+miDSP.Tables[0].Rows[i][0].ToString()+"' AND mfac_numeordepago="+miDSP.Tables[0].Rows[i][1].ToString()+"");
                        sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='A', mfac_valoabon=mfac_valoabon-" + miDSP.Tables[0].Rows[i][2].ToString() + " WHERE pdoc_codiordepago='" + miDSP.Tables[0].Rows[i][0].ToString() + "' AND mfac_numeordepago=" + miDSP.Tables[0].Rows[i][1].ToString() + "");
                    //   no debe borrar el valor abonada a cada documento, lo que debe hacer es en las consutlas y reportes de proveedores, detectar que si el pago es anulado descarte estos abonos y muestre el esatod ANULADO
                    //   sqlStrings.Add("UPDATE Dcajaproveedor SET Dcaj_valorecicaja = 0 WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + ""); // PONE en CEROS el valor del abono porque NO HAY campo de observacion
                    }
				}
			}
			//Anulacion tipo D
			else if(clase=="Devolución Pedido Cliente")
			{
				DBFunctions.Request(miDS,IncludeSchema.NO,"SELECT PDOC_CODIGO,Mcaj_numerO,mped_codigo,mped_numepedi FROM dcajadevolucionpedido WHERE pdoc_codigo='"+prefijoRecibo+"' and mcaj_numero="+numeroRecibo+"");
				for(i=0;i<miDS.Tables[0].Rows.Count;i++)
				{
					sqlStrings.Add("UPDATE manticipovehiculo SET test_estado=10 WHERE MPED_codigo='"+miDS.Tables[0].Rows[i][2].ToString()+"' AND mped_numepedi="+miDS.Tables[0].Rows[i][3].ToString()+"");
					sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=10 WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][2].ToString()+"' AND mped_numepedi="+miDS.Tables[0].Rows[i][3].ToString()+"");
				}
			}
			//Anulacion tipo G
			else if(clase=="Anticipo General")
			{
				//sqlStrings.Add("UPDATE dcajaproveedor SET test_estado='C' WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+"");
				sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_valoabon=(mfac_valofact+mfac_valoiva+mfac_valoivaflet-mfac_valorete) WHERE pdoc_codiordepago=(SELECT pdoc_codiordepago FROM dcajaproveedor WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+") AND mfac_numeordepago=(SELECT mfac_numeordepago FROM dcajaproveedor WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+")");
			}
			//Anulación tipo S
			else if(clase=="Prestamo a Cliente")
			{
				DBFunctions.Request(miDS,IncludeSchema.NO,"SELECT mfac_codigo,mfac_numedocu FROM dbxschema.mcajaanticipo WHERE pdoc_codigo='"+this.prefijoRecibo+"' AND mcaj_numero="+this.numeroRecibo+"");
				if(miDS.Tables[0].Rows.Count!=0)
				{
					for(i=0;i<miDS.Tables[0].Rows.Count;i++)
					{
						if((Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM dbxschema.mfacturacliente WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+""))!=0) && (Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM dbxschema.mfacturacliente WHERE  pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+""))>=Convert.ToDouble(DBFunctions.SingleData("SELECT ccar_valorfrontera FROM dbxschema.ccartera"))))
						{
							DBFunctions.Request(miDSP,IncludeSchema.NO,"SELECT pdoc_codigo,pdoc_ultidocu FROM dbxschema.pdocumento WHERE tdoc_tipodocu='NC' FETCH FIRST ROW ONLY");
							DBFunctions.Request(miDSP,IncludeSchema.NO,"SELECT * FROM mfacturacliente WHRE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
							if(miDSP.Tables[0].Rows.Count!=0)
							{
								miNota=new NotaDevolucionCliente(miDSP.Tables[0].Rows[0][0].ToString(),prefijoRecibo,(Convert.ToUInt32(miDSP.Tables[0].Rows[0][1].ToString())+1),Convert.ToUInt32(numeroRecibo),"N",Convert.ToDouble(miDSP.Tables[1].Rows[0][14]),"RC","V",Convert.ToDateTime(fecha),usuario,almacen,datBen);
								if(miNota.GrabarNotaDevolucionCliente(false))
									this.Sacar_Sqls_Notas(miNota,ref sqlStrings);
								else
									sqlStrings.Add(miNota.ProcessMsg);
							}
						}
						sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_valoabon=(mfac_valofact+mfac_valoiva+mfac_valoivaflet-mfac_valorete) WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
					}
				}
			}
            //Anulación tipo O
            else if(clase=="Obligaciones Financieras")
            {
                DataSet obligaciones = new DataSet();
                DBFunctions.Request(obligaciones, IncludeSchema.NO, "select *  from DOBLIGACIONFINANCIERAPAGOOBLI where pdoc_codigo='" + this.prefijoRecibo + "' and dobl_numedocu=" + this.numeroRecibo + ";");
                for (i = 0; i < obligaciones.Tables[0].Rows.Count; i++)
                {
                    sqlStrings.Add("update MOBLIGACIONFINANCIERA set mobl_montpagado = mobl_montpagado - " + obligaciones.Tables[0].Rows[i][6] + "," +
                                   "mobl_interespagado = mobl_interespagado - " + obligaciones.Tables[0].Rows[i][7] + " where mobl_numero='" + obligaciones.Tables[0].Rows[i][2] + "' ;");

                    sqlStrings.Add("update dOBLIGACIONFINANCIERAplanpago set dobl_montpago = dobl_montpago - " + obligaciones.Tables[0].Rows[i][6] + "," +
                                              "dobl_intepago = dobl_intepago - " + obligaciones.Tables[0].Rows[i][7] + " where mobl_numero='" + obligaciones.Tables[0].Rows[i][2] + "' and dobl_numepago=" + obligaciones.Tables[0].Rows[i][3] + ";");

                    sqlStrings.Add("insert into DOBLIGACIONFINANCIERAPAGOOBLIANULACION values(" +
                                "'" + obligaciones.Tables[0].Rows[i][0] + "'," + obligaciones.Tables[0].Rows[i][1] + ",'" + obligaciones.Tables[0].Rows[i][2] + "'," +
                                obligaciones.Tables[0].Rows[i][3] + "," + obligaciones.Tables[0].Rows[i][4] + "," + obligaciones.Tables[0].Rows[i][5] + "," +
                                obligaciones.Tables[0].Rows[i][6] + "," + obligaciones.Tables[0].Rows[i][7] + ",'" + this.prefijoAnulacion + "'," + consAnu + ");");
                }
                sqlStrings.Add("DELETE from DOBLIGACIONFINANCIERAPAGOOBLI  where pdoc_codigo='" + this.prefijoRecibo + "' and dobl_numedocu=" + this.numeroRecibo + ";");
            }

			//Cambio realizado el 17-08-2007
			//Se busca si el recibo o comprobante tiene notas por sobrantes y se anulan las mismas
			if(tipoDocumento=="RC" || tipoDocumento=="RP")
			{
				if(DBFunctions.RecordExist("SELECT pdoc_codigo FROM mnotacliente WHERE mnot_prefdocu='"+prefijoRecibo+"' AND mnot_numdocu="+numeroRecibo+""))
				{
					miDS=new DataSet();
					DBFunctions.Request(miDS,IncludeSchema.NO,"SELECT MNOT.pdoc_codigo,MNOT.mnot_numero FROM dbxschema.mfacturacliente MFAC,dbxschema.mnotacliente MNOT WHERE MFAC.pdoc_codigo=MNOT.pdoc_codigo AND MFAC.mfac_numedocu=MNOT.mnot_numero AND MNOT.mnot_prefdocu='"+prefijoRecibo+"' AND MNOT.mnot_numdocu="+numeroRecibo+"");
					for(i=0;i<miDS.Tables[0].Rows.Count;i++)
						sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C',mfac_observacion='Se cancela por anulación del RC "+prefijoRecibo+"-"+numeroRecibo+"',mfac_valoabon=(mfac_valofact+mfac_valoiva+mfac_valoivaflet-mfac_valorete) WHERE pdoc_codigo='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+miDS.Tables[0].Rows[i][1].ToString()+"");
				}
			}
			else if(tipoDocumento=="CE")
			{
				if(DBFunctions.RecordExist("SELECT pdoc_codigo FROM mnotaproveedor WHERE pdoc_prefdocref='"+prefijoRecibo+"' AND mnot_numedocref="+numeroRecibo+""))
				{
					miDS=new DataSet();
					DBFunctions.Request(miDS,IncludeSchema.NO,"SELECT MNP.pdoc_codigo,MNP.mnot_numenot FROM dbxschema.mfacturaproveedor MFP,dbxschema.mnotaproveedor MNP WHERE MFP.pdoc_codiordepago=MNP.pdoc_codigo AND MFP.mfac_numeordepago=MNP.mnot_numenot AND MNP.pdoc_prefdocref='"+prefijoRecibo+"' AND MNP.mnot_numedocref="+numeroRecibo+"");
					for(i=0;i<miDS.Tables[0].Rows.Count;i++)
						sqlStrings.Add("UPDATE mfacturaproveedor SET tvig_vigencia='C',mfac_observacion='Se cancela por anulación del CE "+prefijoRecibo+"-"+numeroRecibo+"',mfac_valoabon=(mfac_valofact+mfac_valoiva+mfac_valoivaflet-mfac_valorete) WHERE pdoc_codiordepago='"+miDS.Tables[0].Rows[i][0].ToString()+"' AND mfac_numeordepago="+miDS.Tables[0].Rows[i][1].ToString()+"");
				}
			}
            //Anulo el recibo de caja
            if (operacionEliminar == true)  // SE ELIMINAN LOS MEDIOS DE PAGO para reusarlos luego Y TODOS LOS SOPORTES DE PAGO
            {
                sqlStrings.Add("DELETE from mcajaPAGO              WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + "");
                sqlStrings.Add("DELETE from DCAJAVARIOS            WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + "");
                sqlStrings.Add("DELETE from DDETALLEFACTURACLIENTE WHERE PDOC_CODDOCREF ='" + this.prefijoRecibo + "' AND DDET_NUMEDOCU=" + this.numeroRecibo + "");
                sqlStrings.Add("DELETE from dcajaproveedor         WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + "");
                sqlStrings.Add("UPDATE Mcaja SET MCAJ_VALONETO = 0 WHERE pdoc_codigo='" + this.prefijoRecibo + "' AND mcaj_numero=" + this.numeroRecibo + "");
            }
  

			if(DBFunctions.Transaction(sqlStrings))
			{
                //contabilización ON LINE
                DateTime fechaFactura = Convert.ToDateTime(DBFunctions.SingleData("select mcaj_FECHA from mcaja where pdoc_codigo='" + this.prefijoRecibo + "' and mcaj_numero=" + this.numeroRecibo + ";"));
                if (operacionEliminar == false)
                {
                    contaOnline.contabilizarOnline(this.prefijoAnulacion, Convert.ToInt32(consAnu), fechaFactura, "");
                }
                else 
                {
                    Comprobante compDelete = new Comprobante();
                    compDelete.Type = this.prefijoRecibo;
                    compDelete.Number = this.numeroRecibo.ToString();
                    compDelete.Year = fechaFactura.Year.ToString();
                    compDelete.Month = fechaFactura.Month.ToString();
                    compDelete.DeleteRecord(this.numeroRecibo.ToString());
                }
                    
				estado=true;
				this.mensajes=DBFunctions.exceptions;
			}
			else
				this.mensajes="Error : "+DBFunctions.exceptions;
			/*for(i=0;i<sqlStrings.Count;i++)
			{
				this.mensajes+=sqlStrings[i].ToString()+"<br>";
				estado=true;
			}*/
			return estado;
		}
	}
}
