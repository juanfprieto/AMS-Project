using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using System.Configuration;
using System.Globalization;

namespace AMS.Finanzas.Tesoreria
{
	public class Consignacion
	{
		protected string prefijoDocumento,prefijoConsignacion,fecha,almacen,detalle,tipoProceso,codigoCuenta,usuario,proceso,mensajes,codigoCuentaDestino,nit,tipoMov,prefijoNota,autorizacionSobregiro;
		protected int numeroTesoreria,numeroConsignacion,consecutivoPago;
		protected double totalConsignado,totalEfectivo,total,valorTransferencia;
		protected DataTable tablaDatos;
		protected ArrayList prefijos;
		
		public string PrefijoDocumento{set{prefijoDocumento=value;} get{return prefijoDocumento;}}
		public string Fecha{set{fecha=value;} get{return fecha;}}
		public string Almacen{set{almacen=value;} get{return almacen;}}
		public string Detalle{set{detalle=value;} get{return detalle;}}
		public string CodigoCuenta{set{codigoCuenta=value;} get{return codigoCuenta;}}
		public string Usuario{set{usuario=value;} get{return usuario;}}
		public string Proceso{set{proceso=value;} get{return proceso;}}
		public string Mensajes{set{mensajes=value;} get{return mensajes;}}
		public string CodigoCuentaDestino{set{codigoCuentaDestino=value;} get{return codigoCuentaDestino;}}
		public string PrefijoConsignacion{set{prefijoConsignacion=value;} get{return prefijoConsignacion;}}
		public string Nit{set{nit=value;} get{return nit;}}
		public string TipoMov{set{tipoMov=value;} get{return tipoMov;}}
		public string PrefijoNota{set{prefijoNota=value;} get{return prefijoNota;}}
		public string AutorizacionSobregiro{set{autorizacionSobregiro=value;} get{return autorizacionSobregiro;}}
		public int NumeroTesoreria{set{numeroTesoreria=value;} get{return numeroTesoreria;}}
		public int NumeroConsignacion{set{numeroConsignacion=value;} get{return numeroConsignacion;}}
		public double TotalConsignado{set{totalConsignado=value;} get{return totalConsignado;}}
		public double TotalEfectivo{set{totalEfectivo=value;} get{return totalEfectivo;}}
		public double Total{set{total=value;} get{return total;}}
		public int ConsecutivoPago{set{consecutivoPago=value;} get{return consecutivoPago;}}
		public double ValorTransferencia{set{valorTransferencia=value;} get{return valorTransferencia;}}
		
		public Consignacion()
		{
			tablaDatos=null;
		}
		
		public Consignacion(DataTable dt)
		{
			tablaDatos = new DataTable();
			tablaDatos=dt;
		}
		
		public bool Guardar_Consignacion()
		{
			bool estado=false;
			int i;
			ArrayList sqlStrings=new ArrayList();
			//														1						2							3						4					5		6			7					8					9		 10
			sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',NULL,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
			sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.total+","+this.totalConsignado+")");
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
			if(tablaDatos!=null)
			{
				for(i=0;i<tablaDatos.Rows.Count;i++)
				{
					sqlStrings.Add("INSERT INTO dtesoreriadocumentos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"','"+this.tablaDatos.Rows[i][0].ToString()+"',"+this.tablaDatos.Rows[i][1].ToString()+",'"+DBFunctions.SingleData("SELECT ttip_codigo FROM ttipopago WHERE ttip_nombre='"+this.tablaDatos.Rows[i][2].ToString()+"'")+"','"+this.tablaDatos.Rows[i][3].ToString()+"','"+DBFunctions.SingleData("SELECT pban_codigo FROM pbanco WHERE pban_nombre='"+this.tablaDatos.Rows[i][4].ToString()+"'")+"',"+System.Convert.ToDouble(this.tablaDatos.Rows[i][5].ToString().Substring(1))+",'"+this.tablaDatos.Rows[i][7].ToString()+"')");
					sqlStrings.Add("UPDATE mcajapago SET test_estado='G' WHERE pdoc_codigo='"+this.tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+this.tablaDatos.Rows[i][1].ToString()+" AND mcpag_numerodoc='"+this.tablaDatos.Rows[i][3].ToString()+"'");
				}
			}
			if(totalEfectivo>0)
				sqlStrings.Add("INSERT INTO dtesoreriaefectivo VALUES('"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"','E',"+this.totalEfectivo+")");
			if(DBFunctions.Transaction(sqlStrings))
			{
				estado=true;
				this.mensajes=DBFunctions.exceptions;
			}
			else
				this.mensajes="Error : "+DBFunctions.exceptions;
			return estado;
		}

        //tipoRecibo: RC: Usado para ingresos de dinero. CE: Usado para salida de dinero.
        //valorEfectivo: -1: No aplica adicion de valor y toma tabla de Emision recibos, >=0: Aplica adicion de valor y toma tabla de Consignaciones. 
        //valorEfectivo: -2: Para anulaciones e invertir el proceso.
        public bool RegistrarSaldoCaja(String usuario, String tipoRecibo, double valorEfectivo)
        {
            bool completo = false;
            double valorMovimiento = 0;
            int indexValor = 4;
            
            ////Determinar estructura del Origen de la tabla (Emision RC, CE) o (Anulaciones)
            if (valorEfectivo == -2) //Anulaciones
            {
                indexValor = 5;
            }
            else  if (valorEfectivo != -1)
            {
                valorMovimiento = valorEfectivo;
            }

            //Obtencion Valor de movimiento.
            for (int k = 0; tablaDatos != null && k < tablaDatos.Rows.Count && valorEfectivo < 0; k++)
            {
                string tipoPago = tablaDatos.Rows[k][0].ToString().Trim();
                if (valorEfectivo == -2) //Anulaciones
                {
                    tipoPago = DBFunctions.SingleData("SELECT ttip_codigo FROM ttipopago where ttip_nombre = '" + tipoPago + "';");
                }
                
                //valor de ingresos. (Efectivo, Cheques, Tarjeta Debito, Tarjeta Credito).
                if (tipoRecibo == "RC" && (tipoPago == "E" || tipoPago == "C" || tipoPago == "D" || tipoPago == "T"))
                {
                    valorMovimiento += double.Parse(tablaDatos.Rows[k][indexValor].ToString(), NumberStyles.Currency);
                }
                //valor de egresos. (Efectivo).
                else if (tipoRecibo == "CE" && (tipoPago == "E" || tipoPago == "CC"))
                {
                    if(valorEfectivo == -2) //anulacion
                        valorMovimiento -= double.Parse(tablaDatos.Rows[k][indexValor].ToString(), NumberStyles.Currency);
                    else
                        valorMovimiento += double.Parse(tablaDatos.Rows[k][indexValor].ToString(), NumberStyles.Currency);
                }
            }

            if (valorEfectivo == -2) //Anulaciones
            {
                //Se invierten para generar la anulacion.
                if (tipoRecibo == "RC")
                    tipoRecibo = "CE";
                else
                    tipoRecibo = "RC";
            }

            String fechaRecibo = DateTime.Now.ToString("yyyy-MM-dd");  // tiene que traer la fecha del documento porque cuando se hacen de otras fechas queda mal el saldo
            if (usuario.Contains("@") ) //|| valorEfectivo == -2) //Anulaciones, cambia el usuario  y fecha para quien creó la factura, no para el que elimina.
            {
                string[] usuarioFecha = usuario.Split('@');
                usuario = usuarioFecha[0];
                fechaRecibo = usuarioFecha[1];
             //   valorEfectivo = -2;
            }

            //Verificacion de existencia de registro
            bool existe = DBFunctions.RecordExist("SELECT mcaj_id from MCAJASALDOCAJERO where mcaj_fecha = '" + fechaRecibo + "' and mcaj_usuario = '" + usuario + "';");
            ArrayList sqlStrings = new ArrayList();
            if (existe)
            {
                //valorEfectivo != -2  para descartar anulaciones.
                if (tipoRecibo == "RC" && valorEfectivo != -2)
                {
                    sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo + (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fechaRecibo + "';");
                }
                else if (tipoRecibo == "CE" && valorEfectivo != -2)
                {
                        sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo - (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fechaRecibo + "';");
                }
                     else  // LAS ANULACIONES DE TESORERIA
                        sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo - (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fechaRecibo + "';");
     

                //RealizarAjusteFechasPosteriores();

                // aqui puedes hacer ese ajuste a los saldos posteriores en una sola sentencia, asi : and mcaj_fecha > '" + fechaRecibo + "' ;");

                DataSet dsFechasPosteriores = new DataSet();
                DBFunctions.Request(dsFechasPosteriores, IncludeSchema.NO, "select mcaj_fecha from MCAJASALDOCAJERO where mcaj_usuario ='" + usuario + "' and mcaj_fecha > '" + fechaRecibo + "' order by mcaj_fecha;");
                //for (int p = 0; p < dsFechasPosteriores.Tables[0].Rows.Count && valorEfectivo == -2; p++) //Solo anulaciones.
                for (int p = 0; p < dsFechasPosteriores.Tables[0].Rows.Count ; p++) //Solo anulaciones.
                    {
                    DateTime fecha = Convert.ToDateTime(dsFechasPosteriores.Tables[0].Rows[p][0].ToString());   

                    if (tipoRecibo == "RC")
                    {
                        sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo + (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fecha.ToString("yyyy-MM-dd") + "';");
                    }
                    else if (tipoRecibo == "CE")
                    {
                        sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo - (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fecha.ToString("yyyy-MM-dd") + "';");
                    }
                    else  // LAS ANULACIONES DE TESORERIA
                        sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo - (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fecha.ToString("yyyy-MM-dd") + "';");
        
                }
            }
            else
            {
                double saldoAnterior = 0;

                try  //Validar existencia de registro inicial.
                {
                    saldoAnterior = Convert.ToDouble(DBFunctions.SingleData("SELECT mcaj_saldo from MCAJASALDOCAJERO where mcaj_usuario = '" + usuario + "' and mcaj_fecha = (select max(mcaj_fecha) from MCAJASALDOCAJERO where mcaj_usuario = '" + usuario + "' and mcaj_fecha < '" + fechaRecibo + "');"));
                }
                catch (Exception er)
                { }

                if (tipoRecibo == "RC")
                {
                    saldoAnterior += valorMovimiento;
                }
                else if (tipoRecibo == "CE")
                {
                    saldoAnterior -= valorMovimiento;
                }
                else
                    saldoAnterior = valorMovimiento;  // consignaciones deben venir con su signo


                sqlStrings.Add("INSERT INTO MCAJASALDOCAJERO(MCAJ_USUARIO, MCAJ_SALDO, MCAJ_FECHA)VALUES( '" + usuario + "'," + saldoAnterior + ",'" + fechaRecibo + "');");

                //RealizarAjusteFechasPosteriores();
                DataSet dsFechasPosteriores = new DataSet();
                DBFunctions.Request(dsFechasPosteriores, IncludeSchema.NO, "select mcaj_fecha from MCAJASALDOCAJERO where mcaj_usuario ='" + usuario + "' and mcaj_fecha > '" + fechaRecibo + "' order by mcaj_fecha;");
                //for (int p = 0; p < dsFechasPosteriores.Tables[0].Rows.Count && valorEfectivo == -2; p++) //Solo anulaciones.
                if (dsFechasPosteriores.Tables.Count > 0)
                {
                    for (int p = 0; p < dsFechasPosteriores.Tables[0].Rows.Count; p++) //Solo anulaciones.
                    {
                        DateTime fecha = Convert.ToDateTime(dsFechasPosteriores.Tables[0].Rows[p][0].ToString());

                        if (tipoRecibo == "RC")
                        {
                            sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo + (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fecha.ToString("yyyy-MM-dd") + "';");
                        }
                        else if (tipoRecibo == "CE")
                        {
                            sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo - (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fecha.ToString("yyyy-MM-dd") + "';");
                        }
                        else  // LAS ANULACIONES DE TESORERIA
                            sqlStrings.Add("UPDATE MCAJASALDOCAJERO set mcaj_saldo = mcaj_saldo - (" + valorMovimiento + ") where mcaj_usuario = '" + usuario + "' and mcaj_fecha='" + fecha.ToString("yyyy-MM-dd") + "';");

                    }
                }
            }

            completo = DBFunctions.Transaction(sqlStrings);

            return completo;
        }
		
		public bool Llenar_ArrayListPrefijos(int posbool, int posicion)
		{
			int i;
			bool estado=false;
			this.prefijos=new ArrayList();
			if(tablaDatos!=null)
			{
				for(i=0;i<tablaDatos.Rows.Count;i++)
				{
					if(Convert.ToBoolean(tablaDatos.Rows[i][posbool]))
					{
                        string prefijo = tablaDatos.Rows[i][posicion].ToString().Split('-')[0];
                        if (tablaDatos.Rows[i][posicion].ToString().Split('-').Length > 2)
                             prefijo = tablaDatos.Rows[i][posicion].ToString().Split('-')[0] + '-' + tablaDatos.Rows[i][posicion].ToString().Split('-')[1];
                        if (prefijos.Count == 0)
                        {
                              prefijos.Add(tablaDatos.Rows[i][posicion].ToString() + "-" + DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'"));
                        }

                        else
                        {
                            int res = prefijos.BinarySearch(tablaDatos.Rows[i][posicion].ToString() + "-" + DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'"));
                            if (res < 0)
                                prefijos.Add(tablaDatos.Rows[i][posicion].ToString() + "-" + DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='" + prefijo + "'"));
                        }
					}
				}
				estado=true;
			}
			return estado;
		}
		
		protected bool Busqueda_Valor(string valor)
		{
			bool encontrado = false;
			for(int i=0;i<prefijos.Count;i++)
			{
				if(prefijos[i].ToString().Trim()==valor.Trim())
					encontrado = true;
			}
			return encontrado;
		}
		
		public bool Guardar_Devolucion()
		{
			bool estado=false;
			ArrayList sqlStrings=new ArrayList();
			FacturaCliente miFactura=new FacturaCliente();
			string completo;
            string [] partes;
			int i,j,k;
			bool encontrado;

			//Guardo en el maestro de tesoreria y actualizo pdocumento
            sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',NULL,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
            sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.codigoCuenta + "',0," + this.total * -1 + ")");
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
			if(tablaDatos!=null)
			{
				if(Llenar_ArrayListPrefijos(8,9))
				{
					for(i=0;i<tablaDatos.Rows.Count;i++)
					{
						encontrado=false;
						j=0;

						while((!encontrado)&&(j<prefijos.Count))
						{
							completo=prefijos[j].ToString();
							partes= completo.Split('-');  // EN TODOS LOS PROCESOS CAMBIAR EL SEPARADOR DE CAMPOS - POR OTRO SIMBOLO QUE NOEXISTA EN EL TECLADO porque muchos prefijos el usuariolos define con - 
                            
                            if (partes.Length > 2)
                            {
                                partes[0] += '-' + partes[1];
                                partes[1] = partes[2];
                            }
                                        
                            if (tablaDatos.Rows[i][9].ToString() == partes[0].ToString())
							{                                
								encontrado=true;
								//Actualizo el estado del documento en mcajapago e inserto una nota de devolucion en mfacturacliente
								sqlStrings.Add("UPDATE mcajapago SET test_estado='D' WHERE pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+" AND mcpag_numerodoc='"+tablaDatos.Rows[i][3].ToString()+"'");
								miFactura=new FacturaCliente("FD",partes[0].ToString().Split('-')[0],tablaDatos.Rows[i][6].ToString(),DBFunctions.SingleData("SELECT palm_almacen FROM mcaja WHERE pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+""),"F",Convert.ToUInt32((partes[1].ToString()))+1,0,Convert.ToDateTime(fecha),Convert.ToDateTime(fecha),Convert.ToDateTime(null),Convert.ToDouble(tablaDatos.Rows[i][5].ToString().Substring(1)),0,0,0,0,0,DBFunctions.SingleData("SELECT pcen_codigo FROM cempresa"),"Devolución de Cheque",DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"),usuario,null);
								//sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+partes[0].ToString()+"',"+(System.Convert.ToInt32(partes[1].ToString())+1).ToString()+",'"+tablaDatos.Rows[i][6].ToString()+"','"+DBFunctions.SingleData("SELECT palm_almacen FROM mcaja WHERE pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+"")+"','A','V','"+this.fecha+"','"+this.fecha+"','"+this.fecha+"',"+System.Convert.ToDouble(tablaDatos.Rows[i][5].ToString().Substring(1))+",0,0,0,0,0,0,0,'"+DBFunctions.SingleData("SELECT pcen_codigo FROM cempresa")+"',null,null,'"+this.usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
								miFactura.GrabarFacturaCliente(false);
								for(k=0;k<miFactura.SqlStrings.Count;k++)
									sqlStrings.Add(miFactura.SqlStrings[k].ToString());
								sqlStrings.Add("INSERT INTO dtesoreriadevoluciones VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+miFactura.PrefijoFactura+"',"+miFactura.NumeroFactura.ToString()+",'"+this.prefijoConsignacion+"',"+this.numeroConsignacion+","+tablaDatos.Rows[i][10].ToString()+")");
								if(this.Busqueda_Valor(prefijos[j].ToString()))
                                {
 									partes[1]=(System.Convert.ToInt32(partes[1].ToString())+1).ToString();
									prefijos.RemoveAt(j); 
									prefijos.Add(tablaDatos.Rows[i][9].ToString()+"-"+partes[1].ToString());
								}
							}
							j+=1;
						}
					}
					/*for(k=0;k<prefijos.Count;k++)
					{
						completo=prefijos[k].ToString();
						partes=completo.Split('-');
						//Actualizo los prefijos de las diferentes notas (si hay diferentes prefijos)
						sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+partes[1].ToString()+" WHERE pdoc_codigo='"+partes[0].ToString()+"'");
					}*/
					if(DBFunctions.Transaction(sqlStrings))
					{
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
				}
			}
			return estado;
		}
		
		public bool Guardar_Transferencia()
		{
			bool estado=false;
			ArrayList sqlStrings=new ArrayList();
			//Actualizo el ultimo número de ese prefijo
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
			//Inserto en el maestro de tesoreria
			sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',null,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
			//Inserto en el maestro de saldos para la cuenta de donde salio el dinero
			sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.totalConsignado*-1+",0)");
			//Inserto en el maestro de saldos para la cuenta donde entro el dinero
			sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuentaDestino+"',"+this.total+",0)");
			//Inserto el detalle del traslado asociando las dos cuentas afectadas
			if(autorizacionSobregiro==null)
				sqlStrings.Add("INSERT INTO dtesoreriatraslados VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"','"+this.codigoCuentaDestino+"',null)");
			else
				sqlStrings.Add("INSERT INTO dtesoreriatraslados VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"','"+this.codigoCuentaDestino+"','"+autorizacionSobregiro+"')");
			if(DBFunctions.Transaction(sqlStrings))
			{
				estado=true;
				this.mensajes=DBFunctions.exceptions;
			}
			else
				this.mensajes="Error : "+DBFunctions.exceptions;
			return estado;
		}
		
		public bool Guardar_Transferencia_Cheque()
		{
			bool estado=false;
			int i;
			ArrayList sqlStrings=new ArrayList();
			if(tablaDatos!=null)
			{
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
				sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',null,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
				//Inserto en el maestro de saldos para la cuenta de donde salio el dinero
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.total*-1+",0)");
				
				for(i=0;i<tablaDatos.Rows.Count;i++)
				{
					if(Convert.ToBoolean(tablaDatos.Rows[i][9]))
					{
						//Inserto en el maestro de saldos para la cuenta donde entro el dinero
						sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuentaDestino+"',"+Convert.ToDouble(tablaDatos.Rows[i][5].ToString())+","+Convert.ToDouble(tablaDatos.Rows[i][5].ToString())+")");
						//Inserto el detalle del traslado asociando las dos cuentas afectadas
						sqlStrings.Add("INSERT INTO dtesoreriatrasladoscheque VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"','"+this.codigoCuentaDestino+"','"+tablaDatos.Rows[i][0].ToString()+"',"+tablaDatos.Rows[i][1].ToString()+",'"+DBFunctions.SingleData("SELECT ttip_codigo FROM ttipopago WHERE ttip_nombre='"+tablaDatos.Rows[i][2].ToString()+"'")+"',"+tablaDatos.Rows[i][3].ToString()+")");
						//Actualizo el estado del cheque
						sqlStrings.Add("UPDATE mcajapago SET test_estado='G' WHERE pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+" AND mcpag_numerodoc='"+tablaDatos.Rows[i][3].ToString()+"'");
					}
				}
				if(DBFunctions.Transaction(sqlStrings))
				{
					estado=true;
					this.mensajes=DBFunctions.exceptions;
				}
				else
					this.mensajes="Error : "+DBFunctions.exceptions;
			}
			return estado;
		}
		
		public bool Guardar_Anulacion()
		{
			bool estado=false;
			ArrayList sqlStrings=new ArrayList();
            Consignacion miConsignacion = new Consignacion();
       
			if(this.tipoMov=="CS")  // Consignaciones
			{
				double valorMov=0,valorCanjeMov=0;
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"))*-1;
				valorCanjeMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldoencanje FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"))*-1;
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+valorMov+","+valorCanjeMov+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso un detalle para la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+total+")");
				for(int i=0;i<tablaDatos.Rows.Count;i++)
					//Actualizo el estado de los documentos y los vuelvo a dejar en caja
					sqlStrings.Add("UPDATE mcajapago set test_estado='C' where pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+" AND pban_codigo='"+tablaDatos.Rows[i][4].ToString()+"' AND ttip_codigo='"+tablaDatos.Rows[i][2].ToString()+"' AND mcpag_numerodoc='"+tablaDatos.Rows[i][3].ToString()+"'");
                string usuarioFecha = DBFunctions.SingleData("select mtes_usuario||'@'||MTES_FECHA from mtesoreria WHERE pdoc_codigo='" + prefijoConsignacion + "' AND mtes_numero=" + numeroConsignacion + "");
                miConsignacion.RegistrarSaldoCaja(usuarioFecha, "CS", double.Parse(valorMov.ToString(), NumberStyles.Currency));  //Registro de saldos de Cajero
  
            }
            else if (tipoMov == "ACC")//Anulacion Cruces de Cartera
            {
                //Borro los datos de la mestra del cruce
                sqlStrings.Add("DELETE FROM MCRUCEDOCUMENTO WHERE PDOC_CODIGO = '" + prefijoConsignacion + "' AND   MCRU_NUMERO = " + numeroConsignacion + ";");
                //Borro los datos de los detalles del cruce
                sqlStrings.Add("DELETE FROM DCRUCEDOCUMENTO WHERE PDOC_CODIGO =  '" + prefijoConsignacion + "' AND   MCRU_NUMERO = " + numeroConsignacion + ";");
                //Inserto en la maestra de anulacion de cruces
                sqlStrings.Add("INSERT INTO mcruceanulacion VALUES('" + prefijoDocumento + "'," + numeroTesoreria + ", '" + this.fecha + "', '" + tablaDatos.Rows[0][7].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','"+ tablaDatos.Rows[0][10].ToString()+ "')");
                //Inserto en el detalle de anulacion de cruces dependiendo si es de clientes o de de proveedor
                if (tablaDatos.Rows[0][2].ToString() != string.Empty && tablaDatos.Rows[0][3].ToString() != string.Empty)
                { 
                    for (int i = 0; i < tablaDatos.Rows.Count; i++)//clientes
                    { 
                        sqlStrings.Add("INSERT INTO dcruceanulacion VALUES('" + prefijoDocumento + "'," + numeroTesoreria + "," + tablaDatos.Rows[i][6].ToString() + ",'" + tablaDatos.Rows[i][2].ToString() + "'," + tablaDatos.Rows[i][3].ToString() + ",null,null,"+ Convert.ToDouble(tablaDatos.Rows[i][9].ToString()) + ")");
                        sqlStrings.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOABON = MFAC_VALOABON - " + Convert.ToDouble(tablaDatos.Rows[i][9].ToString()) + ", TVIG_VIGENCIA = 'A' WHERE PDOC_CODIGO = '" + tablaDatos.Rows[i]["FACTURA CLIENTE"].ToString() + "' AND MFAC_NUMEDOCU = "+tablaDatos.Rows[i]["NUMERO"].ToString()+ " ");
                        //sqlStrings.Add("UPDATE MFACTURAPROVEEDOR SET MFAC_VALOABON = MFAC_VALOABON - " + Convert.ToDouble(tablaDatos.Rows[i][9].ToString()) + ", TVIG_VIGENCIA = A WHERE PDOC_CODIORDEPAGO = '" + tablaDatos.Rows[i]["FACTURA PROVEEDOR"].ToString() + "' AND MFAC_NUMERO = " + tablaDatos.Rows[i]["NUM FP"].ToString() + " AND '" + tablaDatos.Rows[i]["FACTURA PROVEEDOR"].ToString() + "' <>''");
                    }
                }
                else
                {
                    for (int i = 0; i < tablaDatos.Rows.Count; i++)//Proveedores
                    { 
                        sqlStrings.Add("INSERT INTO dcruceanulacion VALUES('" + prefijoDocumento + "'," + numeroTesoreria + "," + tablaDatos.Rows[i][6].ToString() + ",null,null,'" + tablaDatos.Rows[i][4].ToString() + "', " + tablaDatos.Rows[i][5].ToString() + ","+ Convert.ToDouble(tablaDatos.Rows[i][9].ToString()) + ")");
                        //sqlStrings.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOABON = MFAC_VALOABON - " + Convert.ToDouble(tablaDatos.Rows[i][9].ToString()) + ", TVIG_VIGENCIA = A WHERE PDOC_CODIGO = '" + tablaDatos.Rows[i]["FACTURA CLIENTE"].ToString() + "' AND MFAC_NUMERO = " + tablaDatos.Rows[i]["NUMERO"].ToString() + " AND '" + tablaDatos.Rows[i]["FACTURA CLIENTE"].ToString() + "' <>''");
                        sqlStrings.Add("UPDATE MFACTURAPROVEEDOR SET MFAC_VALOABON = MFAC_VALOABON - " + Convert.ToDouble(tablaDatos.Rows[i][9].ToString()) + ", TVIG_VIGENCIA = 'A' WHERE PDOC_CODIORDEPAGO = '" + tablaDatos.Rows[i]["FACTURA PROVEEDOR"].ToString() + "' AND MFAC_NUMEORDEPAGO = " + tablaDatos.Rows[i]["NUM FP"].ToString() + " ");
                    }
                }
                //Actualizo el consecutivo del documento
                sqlStrings.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = PDOC_ULTIDOCU + 1 WHERE PDOC_CODIGO = '"+ prefijoDocumento + "'; ");
             
            }

            else if(tipoMov=="DE") // devoluciones - anulaciones de tesoreria
			{
				double valorMov=0,valorCanjeMov=0;
				double valorFac=0,valorIvaFac=0,valorRetFac=0;
				ArrayList prefNot=new ArrayList();
				int[] numeros=null;
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"))*-1;
				valorCanjeMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldoencanje FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"))*-1;
				AMS.Documentos.NotaDevolucionCliente miNota=new AMS.Documentos.NotaDevolucionCliente();
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+valorMov+","+valorCanjeMov+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso un detalle para la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+total+")");
				prefNot=AgregarPrefijosNotasDevolucionAnulacion();
				numeros=AgregarNumerosNotas(prefNot);
				for(int i=0;i<tablaDatos.Rows.Count;i++)
				{
					for(int j=0;j<prefNot.Count;j++)
					{
						if(tablaDatos.Rows[i][10].ToString()==prefNot[j].ToString())
						{
							//Creo una nota devolución por cada factura creada por devolución
							valorFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[i][3].ToString()+""));
							valorIvaFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva+mfac_valoivaflet FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[i][3].ToString()+""));
							valorRetFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[i][2].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[i][3].ToString()+""));
							numeros[j]=numeros[j]+1;
							miNota=new AMS.Documentos.NotaDevolucionCliente(tablaDatos.Rows[i][10].ToString(),tablaDatos.Rows[i][2].ToString(),Convert.ToUInt32(numeros[j]),Convert.ToUInt32(tablaDatos.Rows[i][3]),"N","FA",valorFac,valorIvaFac,valorRetFac,Convert.ToDateTime(fecha),usuario,null);
							miNota.GrabarNotaDevolucionCliente(false);
							for(int k=0;k<miNota.SqlStrings.Count;k++)
								sqlStrings.Add(miNota.SqlStrings[k].ToString());
							numeros[j]=Convert.ToInt32(miNota.NumeroNota);
						}
					}
					//Dejo el estado del documento en consignado
					sqlStrings.Add("UPDATE mcajapago SET test_estado='G' WHERE pdoc_codigo='"+tablaDatos.Rows[i][6].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][7].ToString()+" AND mcpag_numerodoc='"+tablaDatos.Rows[i][8].ToString()+"' AND pban_codigo='"+tablaDatos.Rows[i][11].ToString()+"'");
				}
			}
			else if(this.tipoMov=="EC")   //  Entrega de Cheques a Proveedores
			{
				double valorMov=0,valorCanjeMov=0;
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"))*-1;
				valorCanjeMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldoencanje FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"))*-1;
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+valorMov+","+valorCanjeMov+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso un detalle para la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+Math.Abs(total)+")");
				for(int i=0;i<tablaDatos.Rows.Count;i++)
					//Actualizo el estado de los documentos y los vuelvo a dejar en caja
					sqlStrings.Add("UPDATE mcajapago set test_estado='C' where pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+" AND pban_codigo='"+tablaDatos.Rows[i][4].ToString()+"' AND mcpag_numerodoc='"+tablaDatos.Rows[i][3].ToString()+"'");
			}
			else if(tipoMov=="ND")    // Notas debito Bancarias
			{
				double valorMov=0,valorCanjeMov=0;
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+(valorMov*-1)+","+valorCanjeMov+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso el detalle de la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+Math.Abs(total)+")");
			}
			else if(tipoMov=="NR")   // 
			{
				double valorMov=0,valorCanjeMov=0;
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+(valorMov*-1)+","+valorCanjeMov+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso el detalle de la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+Math.Abs(total)+")");
			}
			else if(tipoMov=="TC")   // Traslado de Fondos entre cuentas bancarias por Carta
			{
				double valorMov=0,valorCanjeMov=0;
				CodigoCuenta=tablaDatos.Rows[0][1].ToString();
				CodigoCuentaDestino=tablaDatos.Rows[0][2].ToString();
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos para la cuenta origen
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo*-1 FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+valorMov+","+valorCanjeMov+")");
				//Inserto el valor del movimiento en los saldos para la cuenta destino
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo*-1 FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuentaDestino+"'"));
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuentaDestino+"',"+valorMov+","+valorCanjeMov+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso el detalle de la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+Math.Abs(total)+")");
			}
            else if (tipoMov == "RF")  // Remisión de Cheques redescontados a Financieras
			{
				double valorMov=0,valorCanjeMov=0;
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				valorCanjeMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldoencanje FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+(valorMov*-1)+","+(valorCanjeMov*-1)+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso el detalle de la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+Math.Abs(total)+")");
				//Creo una nota a favor de la financiera
				uint   numeroNota =Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'"));
				double valorFac   =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[0][5].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[0][6].ToString()+""));
				double valorIvaFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva+mfac_valoivaflet FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[0][5].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[0][6].ToString()+""));
				double valorRetFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[0][5].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[0][6].ToString()+""));
				NotaDevolucionCliente miNota=new NotaDevolucionCliente(prefijoNota,tablaDatos.Rows[0][5].ToString(),numeroNota,Convert.ToUInt32(tablaDatos.Rows[0][6]),"N","FA",valorFac,valorIvaFac,valorRetFac,Convert.ToDateTime(fecha),usuario,null);
				miNota.GrabarNotaDevolucionCliente(false);
				for(int i=0;i<miNota.SqlStrings.Count;i++)
					sqlStrings.Add(miNota.SqlStrings[i].ToString());
				for(int i=0;i<tablaDatos.Rows.Count;i++)
					//Actualizo el estado de los documentos y los vuelvo a dejar en caja
					sqlStrings.Add("UPDATE mcajapago set test_estado='C' where pdoc_codigo='"+tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][1].ToString()+" AND mcpag_numerodoc='"+tablaDatos.Rows[i][4].ToString()+"'");
			}
			else if(tipoMov=="DF") // Devolucion de Cheques redescontados en Financieras
			{
				double valorMov=0,valorCanjeMov=0;
				NotaDevolucionProveedor miNotaP=new NotaDevolucionProveedor();
				NotaDevolucionCliente miNota=new NotaDevolucionCliente();
				CodigoCuenta=DBFunctions.SingleData("SELECT pcue_codigo FROM mtesoreria WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				valorMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldo FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				valorCanjeMov=Convert.ToDouble(DBFunctions.SingleData("SELECT mtes_saldoencanje FROM mtesoreriasaldos WHERE mtes_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+" AND pcue_codigo='"+codigoCuenta+"'"));
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + this.codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
				//Inserto el valor del movimiento en los saldos
				sqlStrings.Add ("INSERT INTO mtesoreriasaldos VALUES(default,'"+prefijoDocumento+"',"+numeroTesoreria+",'"+codigoCuenta+"',"+(valorMov*-1)+","+(valorCanjeMov*-1)+")");
				//Actualizo el registro de la consignacion y lo dejo anulado
				sqlStrings.Add("UPDATE mtesoreria SET test_estadodoc='N' WHERE pdoc_codigo='"+prefijoConsignacion+"' AND mtes_numero="+numeroConsignacion+"");
				//Ingreso el detalle de la anulación
				sqlStrings.Add("INSERT INTO dtesoreriaanulacion VALUES('"+prefijoDocumento+"',"+numeroTesoreria+",'"+prefijoConsignacion+"',"+numeroConsignacion+","+Math.Abs(total)+")");
				//Anulo la factura de proveedor creada en la devolución
				double valorFac   =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaDatos.Rows[0][2].ToString()+"' AND mfac_numeordepago="+tablaDatos.Rows[0][3].ToString()+""));
				double valorIvaFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva+mfac_valoivaflet FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaDatos.Rows[0][2].ToString()+"' AND mfac_numeordepago="+tablaDatos.Rows[0][3].ToString()+""));
				double valorRetFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='"+tablaDatos.Rows[0][2].ToString()+"' AND mfac_numeordepago="+tablaDatos.Rows[0][3].ToString()+""));
				uint   numeroNota =Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoNota+"'"));
				miNotaP           =new NotaDevolucionProveedor(prefijoNota,tablaDatos.Rows[0][2].ToString(),numeroNota,Convert.ToUInt32(tablaDatos.Rows[0][3]),"N","FA",valorFac,valorIvaFac,valorRetFac,Convert.ToDateTime(fecha),usuario);
				miNotaP.GrabarNotaDevolucionProveedor(false);
				for(int i=0;i<miNotaP.SqlStrings.Count;i++)
					sqlStrings.Add(miNotaP.SqlStrings[i].ToString());
				//Anulo cada una de las facturas creadas por cheque
				for(int i=0;i<tablaDatos.Rows.Count;i++)
				{
					valorFac   =Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact+mfac_valoflet FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[i][8].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[i][9].ToString()+""));
					valorIvaFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva+mfac_valoivaflet FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[i][8].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[i][9].ToString()+""));
					valorRetFac=Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='"+tablaDatos.Rows[i][8].ToString()+"' AND mfac_numedocu="+tablaDatos.Rows[i][9].ToString()+""));
					numeroNota =Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+tablaDatos.Rows[i][10].ToString()+"'"));
					miNota     =new NotaDevolucionCliente(tablaDatos.Rows[i][10].ToString(),tablaDatos.Rows[i][8].ToString(),numeroNota,Convert.ToUInt32(tablaDatos.Rows[i][9]),"N","FA",valorFac,valorIvaFac,valorRetFac,Convert.ToDateTime(fecha),usuario,null);
					miNota.GrabarNotaDevolucionCliente(false);
					for(int j=0;j<miNota.SqlStrings.Count;j++)
						sqlStrings.Add(miNota.SqlStrings[j].ToString());
					sqlStrings.Add("UPDATE mcajapago SET test_estado='G' WHERE pdoc_codigo='"+tablaDatos.Rows[i][4].ToString()+"' AND mcaj_numero="+tablaDatos.Rows[i][5].ToString()+" AND mcpag_numerodoc='"+tablaDatos.Rows[i][6].ToString()+"'");
				}
			}
			if(DBFunctions.Transaction(sqlStrings))
			{
				estado=true;
				mensajes=DBFunctions.exceptions;
			}
			else
				mensajes="Error "+DBFunctions.exceptions;
			return estado;
		}
		
		public bool Guardar_Entrega()
		{
			bool estado=false;
			int i;
			ArrayList sqlStrings=new ArrayList();
			if(this.tablaDatos!=null)
			{
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
				sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',null,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
				//Inserto en el maestro de saldos
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.total*-1+",0)");
				//Por cada cheque entregado inserto en el detalle de entregas y actualizo el estado del cheque
				for(i=0;i<this.tablaDatos.Rows.Count;i++)
				{
					if(Convert.ToBoolean(this.tablaDatos.Rows[i][8]))
					{
						sqlStrings.Add("INSERT INTO dtesoreriaentregas VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.tablaDatos.Rows[i][0].ToString()+"',"+this.tablaDatos.Rows[i][1].ToString()+",'"+this.tablaDatos.Rows[i][6].ToString()+"',"+this.tablaDatos.Rows[i][3].ToString()+")");
						sqlStrings.Add("UPDATE mcajapago SET test_estado='N' WHERE pdoc_codigo='"+this.tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+this.tablaDatos.Rows[i][1].ToString()+" AND mcpag_numerodoc='"+this.tablaDatos.Rows[i][3].ToString()+"'");
					}
				}
				if(DBFunctions.Transaction(sqlStrings))
				{
					estado=true;
					this.mensajes=DBFunctions.exceptions;
				}
				else
					this.mensajes="Error : "+DBFunctions.exceptions;
			}
			return estado;
		}
		
		public bool Guardar_Nota()
		{
			int i;
			bool estado=false;
			ArrayList sqlStrings=new ArrayList();
			if(tablaDatos!=null)
			{
				//Actualizo el ultimo número de ese prefijo
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en el maestro de tesoreria
				sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',null,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
				//Inserto en el maestro de saldos
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.total+",0)");
				for(i=0;i<tablaDatos.Rows.Count;i++)
					//Inserto un registro por cada concepto que tiene la nota
					sqlStrings.Add("INSERT INTO dtesorerianota VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+tablaDatos.Rows[i][0].ToString()+"',"+tablaDatos.Rows[i][1].ToString()+")");
				if(DBFunctions.Transaction(sqlStrings))
				{
					estado=true;
					this.mensajes=DBFunctions.exceptions;
				}
				else
					this.mensajes="Error : "+DBFunctions.exceptions;
			}
			return estado;
		}
		
		public bool Guardar_Remision()
		{
			bool estado=false;
			int i;
			ArrayList sqlStrings=new ArrayList();
			FacturaCliente miFactura=new FacturaCliente();
			if(tablaDatos!=null)
			{
				//Inserto en mtesoreria
				sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',null,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
				//Actualizo en pdocumento
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
				//Inserto en mtesoreriasaldos
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.total+","+this.total+")");
				//Inserto en mfacturacliente
				miFactura=new FacturaCliente("FRM",prefijoConsignacion,nit,almacen,"F",Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+this.prefijoConsignacion+"'")),0,Convert.ToDateTime(fecha),Convert.ToDateTime(fecha),Convert.ToDateTime(null),totalConsignado,0,0,0,0,0,DBFunctions.SingleData("SELECT pcen_codigo FROM cempresa"),"Remisión de Cheque a Financiera",DBFunctions.SingleData("SELECT pven_codigo FROM ccartera"),usuario,null);
				//Inserto en dtesoreriaremision
				miFactura.GrabarFacturaCliente(false);
				for(i=0;i<miFactura.SqlStrings.Count;i++)
					sqlStrings.Add(miFactura.SqlStrings[i].ToString());
				for(i=0;i<tablaDatos.Rows.Count;i++)
				{
					sqlStrings.Add("INSERT INTO dtesoreriaremision VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.tablaDatos.Rows[i][0].ToString()+"',"+this.tablaDatos.Rows[i][1].ToString()+","+this.tablaDatos.Rows[i][2].ToString()+",'"+nit+"','"+this.tablaDatos.Rows[i][3].ToString()+"',"+(Convert.ToDouble(this.tablaDatos.Rows[i][4].ToString())).ToString()+",'"+miFactura.PrefijoFactura+"',"+miFactura.NumeroFactura+")");
					sqlStrings.Add("UPDATE mcajapago SET test_estado='G' WHERE pdoc_codigo='"+this.tablaDatos.Rows[i][0].ToString()+"' AND mcaj_numero="+this.tablaDatos.Rows[i][1].ToString()+" AND mcpag_numerodoc='"+this.tablaDatos.Rows[i][3].ToString()+"'");
				}
				if(DBFunctions.Transaction(sqlStrings))
				{
					estado=true;
					this.mensajes=DBFunctions.exceptions;
				}
				else
					this.mensajes="Error : "+DBFunctions.exceptions;
			}
			return estado;
		}
		
		public bool Guardar_DevFinanciera()
		{
			bool estado=false;
			ArrayList sqlStrings=new ArrayList();
			string completo;
			string []partes;
			int i,j,k;
			bool encontrado;
			FacturaProveedor miNotaP=new FacturaProveedor();
			NotaDevolucionCliente miNota=new NotaDevolucionCliente();
			//Inserto en mtesoreria
			sqlStrings.Add("INSERT INTO mtesoreria VALUES('"+this.codigoCuenta+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.almacen+"','"+this.fecha+"',null,'"+this.usuario+"','"+this.proceso+"','"+this.detalle+"','A')");
			//Actualizo en pdocumento
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+this.numeroTesoreria+" WHERE pdoc_codigo='"+this.prefijoDocumento+"'");
			//Inserto en mtesoreriasaldos
			sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+this.total*-1+","+this.total*-1+")");
			//Factura Proveedor pa la financiera
			miNotaP=new FacturaProveedor("FDR",prefijoConsignacion,prefijoDocumento,nit,almacen,"F",Convert.ToUInt64(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+this.prefijoConsignacion+"'")),Convert.ToUInt64(numeroTesoreria),"V",Convert.ToDateTime(fecha),Convert.ToDateTime(fecha),Convert.ToDateTime(null),Convert.ToDateTime(fecha),total,0,0,0,0,"Devolución de Cheques Financiera",usuario);
			miNotaP.GrabarFacturaProveedor(false);
			for(i=0;i<miNotaP.SqlStrings.Count;i++)
				sqlStrings.Add(miNotaP.SqlStrings[i].ToString());
			//sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES('"+this.prefijoConsignacion+"',"+Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+this.prefijoConsignacion+"'"))+",'"+this.nit+"','"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.fecha+"','"+this.almacen+"','"+this.fecha+"','F','V','"+this.fecha+"','"+this.fecha+"',"+this.total+",0,0,0,0,0,'Devolución de Cheques Financiera','"+this.usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
			if(tablaDatos!=null)
			{
				if(Llenar_ArrayListPrefijos(7,8))
				{
					for(i=0;i<tablaDatos.Rows.Count;i++)
					{
						encontrado=false;
						j=0;
						while((!encontrado)&&(j<prefijos.Count))
						{
							completo=prefijos[j].ToString();
							partes=completo.Split('-');
							if((tablaDatos.Rows[i][8].ToString()==partes[0].ToString()) && Convert.ToBoolean(tablaDatos.Rows[i][7]))
							{
								encontrado=true;
								//Nota Cliente para cada cliente del que se devuelve cheque
								miNota=new NotaDevolucionCliente(partes[0].ToString(),tablaDatos.Rows[i][2].ToString(),Convert.ToUInt32(partes[1])+1,Convert.ToUInt32(tablaDatos.Rows[i][3]),"N",Convert.ToDouble(tablaDatos.Rows[i][5]),"RC","V",Convert.ToDateTime(fecha),usuario,almacen,DBFunctions.SingleData("SELECT M.mnit_nit FROM mcaja M,mcajapago P WHERE P.pdoc_codigo=M.pdoc_codigo AND P.mcaj_numero=M.mcaj_numero AND P.mcpag_numerodoc='"+tablaDatos.Rows[i][4].ToString()+"' AND P.test_estado='G'"));
								miNota.GrabarNotaDevolucionCliente(false);
								for(k=0;k<miNota.SqlStrings.Count;k++)
									sqlStrings.Add(miNota.SqlStrings[k]);
								//sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+partes[0].ToString()+"',"+(System.Convert.ToInt32(partes[1].ToString())+1).ToString()+",'"+DBFunctions.SingleData("SELECT M.mnit_nit FROM mcaja M,mcajapago P WHERE P.pdoc_codigo=M.pdoc_codigo AND P.mcaj_numero=M.mcaj_numero AND P.mcpag_numerodoc='"+tablaDatos.Rows[i][4].ToString()+"' AND P.test_estado='G'")+"','"+this.almacen+"','N','V','"+this.fecha+"','"+this.fecha+"','"+this.fecha+"',"+Convert.ToDouble(tablaDatos.Rows[i][5])+",0,0,0,0,0,0,0,'"+DBFunctions.SingleData("SELECT pcen_codigo FROM cempresa")+"',null,null,'"+this.usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
								//Inserto el detalle de la devolución
								sqlStrings.Add("INSERT INTO dtesoreriadevolucionremision VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+tablaDatos.Rows[i][0].ToString()+"',"+tablaDatos.Rows[i][1].ToString()+",'"+tablaDatos.Rows[i][4].ToString()+"',"+Convert.ToDouble(tablaDatos.Rows[i][5])+",'"+miNota.PrefijoNota+"',"+miNota.NumeroNota+",'"+miNotaP.PrefijoFactura+"',"+miNotaP.NumeroFactura+")");
								//Actualizo mcajapago
								sqlStrings.Add("UPDATE mcajapago SET test_estado='D' WHERE mcpag_numerodoc='"+tablaDatos.Rows[i][4].ToString()+"' AND test_estado='G'");
								if(this.Busqueda_Valor(prefijos[j].ToString()))
								{
									partes[1]=(Convert.ToInt32(partes[1].ToString())+1).ToString();
									prefijos.RemoveAt(j);
									prefijos.Add(tablaDatos.Rows[i][8].ToString()+"-"+partes[1].ToString());
								}
							}
							j+=1;
						}
					}
					for(k=0;k<prefijos.Count;k++)
					{
						completo=prefijos[k].ToString();
						partes=completo.Split('-');
						//Actualizo los prefijos de las diferentes notas (si hay diferentes prefijos)
						sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+partes[1].ToString()+" WHERE pdoc_codigo='"+partes[0].ToString()+"'");
					}
				}
			}
			if(DBFunctions.Transaction(sqlStrings))
			{
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

        public ArrayList Guardar_Transferencia_Bancaria(ref ArrayList sqlStrings, int i, int nTransf)
        {
     //       ArrayList sqlStrings = new ArrayList();
            string tipoDocH = "";
            double valorImp = 0;
            consecutivoPago = i;
            //Inserto en mtesoreria
            if (nTransf == 1)
            {
                sqlStrings.Add("INSERT INTO mtesoreria VALUES('" + codigoCuenta + "','" + this.prefijoDocumento + "'," + this.numeroTesoreria + ",'" + this.almacen + "','" + this.fecha + "',null,'" + this.usuario + "','" + this.proceso + "','" + this.detalle + "','A')");
            }
            //Inserto en mtesoreriasaldos
            tipoDocH = DBFunctions.SingleData("SELECT tdoc_tipodocu FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijoDocumento + "'");
            string tipoDoc = tipoDocH.Trim();
            if (tipoDoc == "CE")  // solo los egresos causan impuesto bancario
            { 
                if (DBFunctions.SingleData("SELECT tres_exenimpuesto FROM pcuentacorriente WHERE pcue_codigo='" + codigoCuenta + "'") == "N")
                {
                    //Si existe algun porcentaje
                    if (Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='" + codigoCuenta + "'")) != 0)
                    {
                        double porc = Convert.ToDouble(DBFunctions.SingleData("SELECT ptes_porcentaje FROM ptesoreria PT,pcuentacorriente PC WHERE PT.ptes_codigo=PC.ptes_codigo AND PC.pcue_codigo='" + codigoCuenta + "'"));
                        valorImp = valorTransferencia * (porc / 100) * 0;  //el impuesto se debe grabar en otro registro que no afecte el valor de la transferencia
                    }
                }
            }
           	if(tipoDoc.Trim()=="RC")
				sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+(valorTransferencia+valorImp).ToString()+",0)");
			else if(tipoDoc.Trim()=="CE")
				    sqlStrings.Add("INSERT INTO mtesoreriasaldos VALUES(default,'"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+this.codigoCuenta+"',"+((valorTransferencia*-1)+valorImp).ToString()+",0)");
			//Inserto en dtesoreriabancaelectronica
			sqlStrings.Add("INSERT INTO dtesoreriabancaelectronica VALUES('"+this.prefijoDocumento+"',"+this.numeroTesoreria+",'"+codigoCuenta+"',"+consecutivoPago+")");
			return sqlStrings;
		}

		private ArrayList AgregarPrefijosNotasDevolucionAnulacion()
		{
			ArrayList prefs=new ArrayList();
			for(int i=0;i<tablaDatos.Rows.Count;i++)
			{
				if(!ExistePrefijo(prefs,tablaDatos.Rows[i][10].ToString()))
					prefs.Add(tablaDatos.Rows[i][10].ToString());
			}
			return prefs;
		}

		private bool ExistePrefijo(ArrayList al, string prefijo)
		{
			bool existe=false;
			for(int i=0;i<al.Count;i++)
			{
				if(al[i].ToString()==prefijo)
				{
					existe=true;
					break;
				}
			}
			return existe;
		}

		private int[] AgregarNumerosNotas(ArrayList arr)
		{
			int[] numeros=new int[arr.Count];
			for(int i=0;i<arr.Count;i++)
				numeros[i]=Convert.ToInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+arr[i].ToString()+"'"));
			return numeros;
		}
	}
}