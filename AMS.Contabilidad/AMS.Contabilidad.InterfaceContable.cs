using System;
using System.Collections;
using System.Data;
using AMS.DB;
using AMS.Forms;

namespace AMS.Contabilidad
{
	/// <summary>
	/// Clase controladora del proceso que realiza la interface contable,
	/// se encarga de traer los valores  necesarios para guardar en mcomprobante,
	/// dcuenta, mauxiliarcuenta, msaldocuenta. Todo esto basado en los parametros
	/// introducidos en ppuchechoscabecera y ppuchechosdetalle
	/// 
	/// </summary>
	public class InterfaceContable
	{
		#region Atributos
		private string usuario,almacen,mensajes,mensajesCabecera;
        private int anio, mes, diaInicial, diaFinal; 
        private string fechaInicial = "", fechaFinal = "";
        private DataTable tablaCabecera,tablaDetalles;
		private ArrayList prefijos;
		private DateTime procesado;
		private bool todosAlmacenes;
		private bool anulaciones;
		
		#endregion
		
		#region Propiedades
		
		public ArrayList Prefijos{set{prefijos=value;} get{return prefijos;}}
		public string Usuario{set{usuario=value;}}
		public string Almacen{set{almacen=value;} get{return almacen;}}
		public int Anio{set{anio=value;} get{return anio;}}
		public int Mes{set{mes=value;} get{return mes;}}
		public DataTable TablaCabecera{set{tablaCabecera=value;} get{return tablaCabecera;}}
		public DataTable TablaDetalles{set{tablaDetalles=value;} get{return tablaDetalles;}}
		public DateTime Procesado{set{procesado=value;} get{return procesado;}}
		public int DiaInicial{set{diaInicial=value;} get{return diaInicial;}}
		public int DiaFinal{set{diaFinal=value;} get{return diaFinal;}}
		public bool TodosAlmacenes{set{todosAlmacenes=value;} get{return todosAlmacenes;}}
		public bool Anulaciones{set{anulaciones=value;} get{return anulaciones;}}
		public string Mensajes{get{return mensajes;}}
		public string MensajesCabecera{get{return mensajesCabecera;}}
		public string erroresSql="";
		public bool AnulacionesCaja=false;
		public bool AnulacionesTesoreria=false;
        string tabla = "";
		
		#endregion
		
		#region Métodos

		/// <summary>
		/// InterfaceContable : constructor de la clase, le asigno estructura a las
		/// tablas tablaCabecera y tablaDetalles
		/// </summary>
		public InterfaceContable()
		{
			Estructura_TablaCabecera();
			Estructura_TablaDetalles();
			mensajesCabecera=mensajes="";
		}

		/// <summary>
		/// Traer_Valores : función que me trae los valores de los campos
		/// necesarios para guardar en dcuenta y las auxiliares
		/// </summary>
		/// <param name="prefijo">string que trae el prefijo del documento</param>
		/// <param name="numero">int que trae el número del documento</param>
		/// <param name="campo">string que trae el nombre del campo de donde se sacara el valor, si es vacio es porque el valor toca sacarlo de otra tabla</param>
		/// <param name="tabla">string que me trae el nombre de la tabla contenedora de campo</param>
		/// <param name="llaves">string que trae los campos que hacen referencia de la tabla detalle a la tabla maestra</param>
		/// <param name="campoRef">string que trae el campo de donde se sacara el valor si el parametro campo esta vacio</param>
		/// <param name="tablaRef">string que trae la tabla donde se encuentra campoRef</param>
		/// <param name="llavesRef">string que trae las llaves que relacionan tablaRef con la tabla maestra</param>
		/// <param name="formula">string que trae un condicional que hara parte del where de la consulta</param>
		/// <param name="prefijoRef">string que trae el prefijo del documento referencia </param>
		/// <param name="numeroRef">int que trae el numerp del documento referencia</param>
		/// <param name="aplicarFormulaCasoEspecial">Indica cuando se esta construyendo el path de una consulta se debe aplicar la formula o no</param>
		/// <returns>ICollection con la(s) fila(s) retornada(s) por la consulta</returns>
		public ArrayList Traer_Valores(string prefijo,int numero,string campo,string tabla,string llaves,string campoRef,string tablaRef,string llavesRef,string formula,string prefijoRef,int numeroRef, bool aplicarFormulaCasoEspecial)
		{
			campo = campo.Replace("@","'");
			string sql="", error="";
			DataSet ds=null;
			//Si el campo trae algo quiere decir q el valor lo saco directamente de la
			//tabla
			if(campo!="")
			{
				//Si el campo es una columna y existe en la tabla detalle
				if(campo.ToUpper().StartsWith("C") || campo.ToUpper().StartsWith("D") || campo.ToUpper().StartsWith("M") || campo.ToUpper().StartsWith("P") || campo.ToUpper().StartsWith("S") || campo.ToUpper().StartsWith("T") || campo.ToUpper().StartsWith("("))
				{
					//Ahora reviso las claves de la tabla para realizar la consulta
					if(campo.ToUpper().StartsWith("("))
						sql="SELECT "+campo+" FROM "+tabla;
					else
						sql="SELECT "+tabla+"."+campo+" FROM "+tabla;
					string[] primary=llaves.Split(';');
					if(formula!="")
					{
						formula=formula.Replace("@","'");
						string[] tablas=formula.Split(';');
						for(int i=0;i<tablas.Length-1;i++)
						{
							sql+=","+tablas[i];
						}
						sql+=" WHERE "+tablas[tablas.Length-1]+" AND ";
					}
					else
						sql+=" WHERE ";
					for(int i=0;i<primary.Length;i++)
					{
						sql+=tabla+"."+primary[i]+"=";
						string tipo=Consultar_TipoDato(primary[i],tabla);
						if(tipo=="BIGINT" || tipo=="DECIMAL" || tipo=="DOUBLE" || tipo=="INTEGER" || tipo=="SMALLINT")
							sql+=numero.ToString()+" ";
						else if(tipo=="CHARACTER" || tipo=="DATE" || tipo=="LONG VARCHAR" || tipo=="TIME" || tipo=="TIMESTAMP" || tipo=="VARCHAR")
							sql+="'"+prefijo+"' ";
						if(i!=primary.Length-1)
							sql+="AND ";
					}
					ds=new DataSet();
					if(!Realizar_Consulta(ref ds,sql,ref error))
						mensajes="Error de consulta : <br>"+error;
				}
				else if(campo.ToUpper().StartsWith("@"))
					sql=campo.Replace("@","");
				else
					sql=campo;
			}
				//Si campo esta vacio, es porq el valor no esta directamente en
				//la tabla y toca sacarlo de una tabla referenciada
			else
			{
				/*1. Si la tabla referencia es la misma tabla padre, hago una
				 * consulta del campo requerido a esa tabla
				 * 2. Si la tabla referencia no es la misma tabla padre:
				 * 2.1 Si es una tabla C hago una consulta sin where, puesto que
				 * no tendria sentido
				 * 2.2 Si no es un tabla C, hago una consulta del campo requerido
				 * en la tabla referencia y mando como where un select del campo a 
				 * la tabla padre
				 */
				bool utilizarDocReferencia = false;
				string padre=DBFunctions.SingleData("SELECT stab_nombre FROM ppuchechoscabecera WHERE pdoc_codigo='"+prefijo+"'");
				string[] referencia = null;
				if(llavesRef.StartsWith("$"))
				{
					referencia = llavesRef.Substring(1,llavesRef.Length-1).Split(';');
					utilizarDocReferencia = true;
				}
				else
					referencia = llavesRef.Split(';');
				string prefijoPadre=DBFunctions.SingleData("SELECT ppuc_codigodoc FROM ppuchechoscabecera WHERE pdoc_codigo='"+prefijo+"'");
				string numeroPadre=DBFunctions.SingleData("SELECT ppuc_numedocu FROM ppuchechoscabecera WHERE pdoc_codigo='"+prefijo+"'");
				if(tablaRef!=string.Empty && campoRef!=string.Empty && llavesRef!=string.Empty)
				{
					//Caso 1
					if(tablaRef==padre)
					{
						if(tablaRef.IndexOf("@") == -1)
							sql="SELECT "+campoRef+" FROM "+tablaRef+" WHERE "+prefijoPadre+"='"+prefijo+"' AND "+numeroPadre+"="+numero.ToString()+"";
						else
						{
							string[] splitTableRef = tablaRef.Split('@');
							sql="SELECT "+campoRef+" FROM "+tablaRef+" WHERE "+prefijoPadre+"='"+prefijo+"' AND "+numeroPadre+"="+numero.ToString()+" ORDER BY "+splitTableRef[1];
						}
					}
					else
					{
						//Caso 2
						//Caso 2.1
						if(tablaRef.StartsWith("C") || tablaRef.StartsWith("c"))
						{
							if(tablaRef.IndexOf("@") == -1)
								sql="SELECT "+campoRef+" FROM "+tablaRef;
							else
							{
								string[] splitTableRef = tablaRef.Split('@');
								sql="SELECT "+campoRef+" FROM "+splitTableRef[0]+" ORDER BY "+splitTableRef[1];
							}
						}
							//Caso 2.2
						else
						{
							//Cambio realizado el dia 24 de abril de 2007
							//Se modifica esta opcion para poder acceder desde cualquier nivel, se accede a diccionario mediante una funcion recursiva que me construya el path de acceso al acmpo siempre teniendo en cuenta los valores de la tabla de donde se va adonde llega
							string[] tables = tablaRef.Split(';');
							if(tables.Length == 1)
							{
								string select = "SELECT ";
								string from = " FROM ";
								string where = " WHERE ";
								string orderBy = "";
								if(tablaRef.IndexOf("@") == -1)
								{
									select += campoRef;
									from += tablaRef+",";
									//sql="SELECT "+campoRef+" FROM "+tablaRef+" WHERE ";
								}
								else 
								{
									string[] splitTableRef = tablaRef.Split('@');
									select += campoRef;
									from += splitTableRef[0]+",";
									//sql="SELECT "+campoRef+" FROM "+splitTableRef[0]+" WHERE ";
									orderBy = " ORDER BY "+splitTableRef[1];
								}
								if(!utilizarDocReferencia)
								{
									for(int i=0;i<referencia.Length;i++)
									{
										where += " "+referencia[i]+"=(SELECT "+referencia[i]+" FROM "+padre+" WHERE "+prefijoPadre+"='"+prefijo+"' AND "+numeroPadre+"="+numero.ToString()+") AND";
									}
								}
								else
								{
									for(int i=0;i<referencia.Length;i++)
									{
										if(i==0)
											where += " "+referencia[i]+"='"+prefijoRef+"' AND";
										else if(i==1)
											where += " " + referencia[i]+"="+numeroRef+" AND";
									}
								}
								if(formula!="" && aplicarFormulaCasoEspecial)
								{
									formula = formula.Replace("@","'");
									string[] tablasFormula = formula.Split(';');
									for(int i=0;i<tablasFormula.Length-1;i++)
									{
										if(from.IndexOf(tablasFormula[i]) == -1)
											from += tablasFormula[i]+",";
									}
									where += " "+tablasFormula[tablasFormula.Length-1]+" AND";
								}
								sql = select + from.Substring(0,from.Length-1)+where.Substring(0,where.Length-4)+orderBy;
							}
							else
							{
								try
								{
									string select = "SELECT ";
									string from = " FROM ";
									string where = " WHERE ";
									string orderBy = " ORDER BY ";
									string[] llavesProceso2 = llavesRef.Split('#');
									for(int i=0;i<tables.Length;i++)
									{
										if(tables[i].IndexOf("@") == -1)
										{
											from += tables[i]+",";
											if(i == 0)
												select += tables[i]+"."+campoRef;
											else
											{
												string[] camposReferencia = llavesProceso2[i-1].Split('@');
												string[] camposIzquierda = camposReferencia[0].Split(';');
												string[] camposDerecha = camposReferencia[1].Split(';');
												if(camposIzquierda.Length != camposDerecha.Length)
													throw new Exception("Campos de llave no congruentes");
												for(int j=0;j<camposIzquierda.Length;j++)
												{
													if(tables[i-1].IndexOf("@") == -1)
														where += " "+tables[i-1]+"."+camposIzquierda[j]+"="+tables[i]+"."+camposDerecha[j]+" AND";
													else
														where += " "+(tables[i-1].Split('@'))[0]+"."+camposIzquierda[j]+"="+tables[i]+"."+camposDerecha[j]+" AND";
												}	
											}
										}
										else
										{
											string[] splitTableRef = tables[i].Split('@');
											from += splitTableRef[0]+",";
											orderBy += splitTableRef[0]+"."+splitTableRef[1];
											if(i == 0)
												select += splitTableRef[0]+"."+campoRef;
											else
											{
												string[] camposReferencia = llavesProceso2[i-1].Split('@');
												string[] camposIzquierda = camposReferencia[0].Split(';');
												string[] camposDerecha = camposReferencia[1].Split(';');
												if(camposIzquierda.Length != camposDerecha.Length)
													throw new Exception("Campos de llave no congruentes");
												for(int j=0;j<camposIzquierda.Length;j++)
												{
													if(tables[i-1].IndexOf("@") == -1)
														where += " "+tables[i-1]+"."+camposIzquierda[j]+"="+splitTableRef[0]+"."+camposDerecha[j]+" AND";
													else
														where += " "+(tables[i-1].Split('@'))[0]+"."+camposIzquierda[j]+"="+splitTableRef[0]+"."+camposDerecha[j]+" AND";
												}	
											}
										}
									}
									string[] camposReferenciaPadre = null;
									if(tables.Length < llavesProceso2.Length)
										camposReferenciaPadre = llavesProceso2[llavesProceso2.Length-2].Split('@');
									else
										camposReferenciaPadre = llavesProceso2[llavesProceso2.Length-1].Split('@');
									for(int i=0;i<camposReferenciaPadre.Length;i++)
									{
										if(i==0)
											where += " "+tables[tables.Length-1]+"."+camposReferenciaPadre[i]+"='"+prefijoRef+"' AND";
										else
											where += " "+tables[tables.Length-1]+"."+camposReferenciaPadre[i]+"="+numeroRef+" AND";
									}
									//Cambio hecho el dia 7 de mayo para manejo de referencia de documento padre
									if(tables.Length < llavesProceso2.Length)
									{
										camposReferenciaPadre = null;
										camposReferenciaPadre = llavesProceso2[llavesProceso2.Length-1].Split('@');
										for(int i=0;i<camposReferenciaPadre.Length;i++)
										{
											if(i==0)
												where += " "+tables[tables.Length-1]+"."+camposReferenciaPadre[i]+"='"+prefijo+"' AND";
											else
												where += " "+tables[tables.Length-1]+"."+camposReferenciaPadre[i]+"="+numero+" AND";
										}
									}
									if(formula!="" && aplicarFormulaCasoEspecial)
									{
										formula = formula.Replace("@","'");
										string[] tablasFormula = formula.Split(';');
										for(int i=0;i<tablasFormula.Length-1;i++)
										{
											if(from.IndexOf(tablasFormula[i]) == -1)
												from += tablasFormula[i]+",";
										}
										where += " "+tablasFormula[tablasFormula.Length-1]+" AND";
									}
									sql = select + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-4);
									if(orderBy != " ORDER BY ")
										sql += orderBy;
								}
								catch
								{
									ds=null;
									mensajes="Error de Por favor revise su parametrizacion";
								}
							}
						}
					}
					ds=new DataSet();
					if(!Realizar_Consulta(ref ds,sql,ref error))
					{
						ds=null;
						mensajes="Error de consulta : <br>"+error;
					}
				}
				else
					sql="0";
			}
			ArrayList filas=new ArrayList();
			//Si el dataset es diferente de nulo y la consulta trae resultados
			if(ds!=null && ds.Tables[0].Rows.Count!=0)
			{
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
					filas.Add(ds.Tables[0].Rows[i][0]);
			}
				//Si el dataset es diferente de nulo, pero la consulta no trajo resultados
			else if(ds!=null && ds.Tables[0].Rows.Count==0)
				filas.Add("Error : <br>" + sql);
				//Si el dataset es nulo y la consulta produjo error
			else if(ds==null && error!=string.Empty)
				filas.Add(mensajes);
				//Si ninguna de las anteriores condiciones se cumple, agrego el valor que tenga
				//la variable sql al arraylist
			else
				filas.Add(sql);
			return filas;
		}

		/// <summary>
		/// Consultar_TipoDato : función que me retorna el tipo de dato SQL
		/// de un determinado campo en una determinada tabla
		/// </summary>
		/// <param name="campo">string que tiene el nombre del campo en la tabla</param>
		/// <param name="tabla">string que tiene el nombre de la tabla</param>
		/// <returns>string con el nombre del tipo de dato SQL</returns>
		private string Consultar_TipoDato(string campo,string tabla)
		{
			string tipo="";
			tipo=DBFunctions.SingleData("SELECT typename FROM syscat.columns WHERE tabname='"+tabla+"' AND colname='"+campo+"'");
			return tipo;
		}

        public void Armar_Cabecera(string prefijo, int documento)
        {
            string tipoInterfase = "Online";
            Armar_Cabecera_documento(prefijo, tipoInterfase, documento);
     
        }
		/// <summary>
		/// Armar_Cabecera : función que me permite armar las cabeceras que
		/// se guardaran en mcomprobante
		/// </summary>
		/// <param name="prefijo">string que tiene el prefijo del documento</param>
		public void Armar_Cabecera(string prefijo)
		{
			string tipoInterfase = "Batch";
            int numeroDocumento = 0;
        //    tablaCabecera.Clear();
            Armar_Cabecera_documento(prefijo, tipoInterfase, numeroDocumento);
        }

        public void Armar_Cabecera_documento(string prefijo, string tipoInterfase, int numeroDocumento)
		{
			DataSet ds = new DataSet();
            
            string proceso = "" ;
            string documento = "";
        //  'RC' 'CE' 'RP' 'CE' 'AN'            =>  MCAJA
        //  'CX'                                =>  MCRUCEDOCUMENTO
        //  'FC' 'NC'                           =>  MFACTURACLIENTE
        //  'FP' 'NP'                           =>  MFACTURAPROVEEDOR
        //  'FC' 'NC'                           =>  MFACTURACLIENTE
        //  'CS' 'EC' 'CB' 'NR' 'ND' 'DE' 'TC'  =>  MTESORERIA
        //  'AJ'                                =>  VINVENTARIO_AJUSTESINVCONTABILIDAD
        //  'CD'                                =>  MDIFERIDO
        //  'DA'                                =>  MACTIVOFIJO
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT PD.PDOC_CODIGO, PD.TDOC_TIPODOCU FROM PDOCUMENTO PD LEFT JOIN ppuchechoscabecera PC ON PC.pdoc_codigo = PD.pdoc_codigo WHERE PD.pdoc_codigo='" + prefijo + "'");
        //  DBFunctions.Request(ds, IncludeSchema.NO, "SELECT PC.PDOC_CODIGO, PD.TDOC_TIPODOCU FROM ppuchechoscabecera PC, PDOCUMENTO PD  WHERE PC.pdoc_codigo = PD.pdoc_codigo AND PC.pdoc_codigo='" + prefijo + "'");
            if (ds.Tables[0].Rows.Count != 0)
			{
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
                    documento = ds.Tables[0].Rows[i][1].ToString();
                    switch (documento)
                    {
                        case "FC" :    //   FACTURA CLIENTE
                             tabla = "MFACTURACLIENTE";
                             proceso = "CL";
                             break;
                        case "NC" :    // DEVOLUCION DE CLIENTE
                             tabla = "MFACTURACLIENTE";
                             proceso = "CL";
                             break;
                        case "TT":     // TRANSFERENCIAS AL TALLER
                             tabla = "MFACTURACLIENTE";
                             proceso = "CL";
                             break;
                        case "NT":     //  DEVOLUCIONES DE TRANSFERENCIAS AL TALLER
                             tabla = "MFACTURACLIENTE";
                             proceso = "CL";
                             break;
                        case "CI":     // Consumo interno Inventarios
                             tabla = "MFACTURACLIENTE";
                             proceso = "IN";
                             break;
                        case "FP" :
                             tabla = "MFACTURAPROVEEDOR";
                             proceso = "PV";
                             break;
                        case "GV":     // Gastos de Vehículos
                             tabla = "DFACTURAGASTOVEHICULO";
                             proceso = "CL";
                             break;
                        case "NP" :
                             tabla = "MFACTURAPROVEEDOR";
                             proceso = "PV";
                             break;    
                        case "RC" :
                             tabla = "MCAJA";
                             proceso = "CJ";
                             break;
                        case "RP" :
                             tabla = "MCAJA";
                             proceso = "CJ";
                             break;
                        case "CE" :
                             tabla = "MCAJA";
                             proceso = "CJ";
                             break;
                        case "AN":    // AX?
                             tabla = "MANULACIONCAJA";
                             proceso = "CJ";
                             AnulacionesCaja = true;
                             break;
                        case "CX":
                             tabla = "MCRUCEDOCUMENTO";
                             proceso = "CJ";
                             break;
                        case "AJ":  // Ajustes por sobrantes y faltantes al Inventario
                             tabla = "MVINVENTARIO_AJUSTESINVCONTABILIDAD";
                             proceso = "IN";
                             break;
                        case "AS":  // Ajustes por SUSTITUCIONES al Inventario
                             tabla = "MVINVENTARIO_AJUSTESINVCONTABILIDAD";
                             proceso = "IN";
                             break;
                        case "IF":  // Ajustes por Inventario Fisico al Inventario
                             tabla = "MVINVENTARIO_AJUSTESINVCONTABILIDAD";
                             proceso = "IN";
                             break;
                        case "CS" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "EC" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "CB" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "NR" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "ND" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "TC" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "DE" :
                             tabla = "MTESORERIA";
                             proceso = "TS";
                             break;
                        case "AT": // AX?  DE?
                             tabla = "DTESORERIAANULACION";
                             proceso = "TS";
                             AnulacionesTesoreria = true;
                             break;
                        case "TE": // GASTOS DE viaje
                             tabla = "MENCOMIENDAS";
                             proceso = "TP";
                             break;
                        case "TP": // GASTOS DE viaje
                             tabla = "MGASTOS_TRANSPORTES";
                             proceso = "TP";
                             break;
                        case "TR": // planillas de viaje
                             tabla = "MPLANILLAVIAJE";
                             proceso = "TP";
                             break;
                        case "TQ": // planillas de Chequo viaje
                             tabla = "MPLANILLAVIAJE_CHEQUEO";
                             proceso = "TP";
                             break;
                        case "TD": // Cierre Diario Agencias de pasajes
                             tabla = "DCIERRE_DIARIO_AGENCIA";
                             proceso = "TP";
                             break;
                        case "EN": // Entreg fisica de vehiculos a cliente
                            tabla = "MVEHICULOENTREGA";
                            proceso = "CL";
                            break;
                        default:
                             Console.WriteLine ("Falta por definir documento en prefijo");
                             return;
                    } // fin seleccion de tabla de casos de imputación
                    DataRow dr=ds.Tables[0].Rows[i];
				    fechaInicial="";
                    fechaFinal="";
                    if (tipoInterfase == "Batch")
                    {
                        fechaInicial += anio.ToString() + "-";
                        fechaFinal += anio.ToString() + "-";
                        if (mes >= 1 && mes <= 9)
                            fechaInicial += "0" + mes.ToString() + "-";
                        else
                            fechaInicial += mes.ToString() + "-";
                        if (diaInicial >= 1 && diaInicial <= 9)
                            fechaInicial += "0" + diaInicial;
                        else
                            fechaInicial += diaInicial;

                        if (mes >= 1 && mes <= 9)
                            fechaFinal += "0" + mes.ToString() + "-";
                        else
                            fechaFinal += mes.ToString() + "-";
                        if (diaFinal >= 1 && diaFinal <= 9)
                            fechaFinal += "0" + diaFinal;
                        else
                            fechaFinal += diaFinal;
                    }
					string sql="";
                    string unAgencia = "";
                    string unAgenciaAux = "";
                    if (todosAlmacenes == false)
                    {
                        unAgencia = " AND MAG_CODIGO='" + almacen + "' ";
                        unAgenciaAux = "AND MAG_RECIBE='" + almacen + "' ";
                    }
                    switch (tabla)
                    {
                        case ("MFACTURACLIENTE"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MFAC_NUMEDOCU, MFAC_FACTURA, MFAC_OBSERVACION,MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET, PALM_ALMACEN, MFAC_USUARIO, MFAC_PROCESO FROM MFACTURACLIENTE WHERE PDOC_CODIGO ='" + prefijo + "' AND MFAC_FACTURA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MFAC_NUMEDOCU";
                        else
                            sql = "SELECT MFAC_NUMEDOCU, MFAC_FACTURA, MFAC_OBSERVACION,MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET, PALM_ALMACEN, MFAC_USUARIO, MFAC_PROCESO FROM MFACTURACLIENTE WHERE PDOC_CODIGO ='" + prefijo + "' AND MFAC_NUMEDOCU = " + numeroDocumento + " ";
                        break;

                        case ("DFACTURAGASTOVEHICULO"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MF.MFAC_NUMEDOCU, MFAC_FACTURA, MFAC_OBSERVACION,MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET, PALM_ALMACEN, MFAC_USUARIO, MFAC_PROCESO FROM MFACTURACLIENTE MF, MFACTURAPEDIDOVEHICULO MFPV WHERE MF.PDOC_CODIGO = MFPV.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFPV.MFAC_NUMEDOCU AND MFAC_FACTURA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MFAC_NUMEDOCU";
                        else
                            sql = "SELECT MFAC_NUMEDOCU, MFAC_FACTURA, MFAC_OBSERVACION,MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET, PALM_ALMACEN, MFAC_USUARIO, MFAC_PROCESO FROM MFACTURACLIENTE WHERE PDOC_CODIGO ='" + prefijo + "' AND MFAC_NUMEDOCU = " + numeroDocumento + " ";
                        break;

                        case ("MFACTURAPROVEEDOR"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MFAC_NUMEORDEPAGO, MFAC_FACTURA, MFAC_OBSERVACION,MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET, PALM_ALMACEN, SUSU_USUARIO, MFAC_PROCESO FROM MFACTURAPROVEEDOR WHERE PDOC_CODIORDEPAGO ='" + prefijo + "' AND MFAC_FACTURA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MFAC_NUMEORDEPAGO";
                        else
                            sql = "SELECT MFAC_NUMEORDEPAGO, MFAC_FACTURA, MFAC_OBSERVACION,MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET, PALM_ALMACEN, SUSU_USUARIO, MFAC_PROCESO FROM MFACTURAPROVEEDOR WHERE PDOC_CODIORDEPAGO ='" + prefijo + "' AND MFAC_NUMEORDEPAGO = " + numeroDocumento + " ";
                        break;

                        case ("MCAJA"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MCAJ_NUMERO, MCAJ_FECHA, MCAJ_RAZON, MCAJ_VALOTOTAL, PALM_ALMACEN, MCAJ_USUARIO, MCAJ_PROCESO FROM MCAJA WHERE PDOC_CODIGO ='" + prefijo + "' AND MCAJ_FECHA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MCAJ_NUMERO";
                        else
                            sql = "SELECT MCAJ_NUMERO, MCAJ_FECHA, MCAJ_RAZON, MCAJ_VALOTOTAL, PALM_ALMACEN, MCAJ_USUARIO, MCAJ_PROCESO FROM MCAJA WHERE PDOC_CODIGO ='" + prefijo + "' AND MCAJ_NUMERO = " + numeroDocumento + " ";
                        break;
                         
                        case ("MANULACIONCAJA"):
                        if (tipoInterfase == "Batch")
                            sql = "";
                 //           sql = "SELECT MANU_NUMEANUL, MANU_FECHA, 'ANULACION ' || MC.PDOC_CODIGO || ' - ' || CAST(MC.MCAJ_NUMERO AS CHAR (08)) || ' ' || MCAJ_RAZON, MCAJ_VALOTOTAL, PALM_ALMACEN, MANU_USUARIO, MANU_PROCESADO "+
                 //                 " FROM ManulacionCAJA MAC, MCAJA MC WHERE MAC.PDOC_CODIANUL ='" + prefijo + "' AND MANU_FECHA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' AND MAC.PDOC_CODIGO = MC.PDOC_CODIGO AND MAC.MCAJ_NUMERO = MC.MCAJ_NUMERO order by MANU_NUMEANUL;";
                        else
                            sql = "SELECT MANU_NUMEANUL, MANU_FECHA, 'ANULACION ' || MC.PDOC_CODIGO || ' - ' || CAST(MC.MCAJ_NUMERO AS CHAR (08)) || ' ' || MCAJ_RAZON, MCAJ_VALOTOTAL, PALM_ALMACEN, MANU_USUARIO, MANU_PROCESADO " +
                                    " FROM ManulacionCAJA MAC, MCAJA MC WHERE MAC.PDOC_CODIANUL ='" + prefijo + "' AND MANU_NUMEANUL = " + numeroDocumento + " AND MAC.PDOC_CODIGO = MC.PDOC_CODIGO AND MAC.MCAJ_NUMERO = MC.MCAJ_NUMERO order by MANU_NUMEANUL;";
                        break;
                        
                        case ("MCRUCEDOCUMENTO"): // FALTA PALMACEN ?
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MCRU_NUMERO, MCRU_FECHA, MNIT_NIT, 01.00, '0', MCRU_USUARIO, MCRU_PROCESO FROM MCRUCEDOCUMENTO WHERE PDOC_CODIGO ='" + prefijo + "' AND MCRU_FECHA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MCRU_NUMERO";
                        else
                            sql = "SELECT MCRU_NUMERO, MCRU_FECHA, MNIT_NIT, 01.00, '0', MCRU_USUARIO, MCRU_PROCESO FROM MCRUCEDOCUMENTO WHERE PDOC_CODIGO ='" + prefijo + "' AND MCRU_NUMERO = " + numeroDocumento + " ";
                        break;
               
                        case ("MTESORERIA"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MTES_NUMERO, MTES_FECHA, MTES_DETALLE, COALESCE(MTES_CREDITOS,1), PALM_ALMACEN, MTES_USUARIO, MTES_PROCESO FROM MTESORERIA WHERE PDOC_CODIGO ='" + prefijo + "' AND MTES_FECHA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MTES_NUMERO";
                        else
                            sql = "SELECT MTES_NUMERO, MTES_FECHA, MTES_DETALLE, COALESCE(MTES_CREDITOS,1), PALM_ALMACEN, MTES_USUARIO, MTES_PROCESO FROM MTESORERIA WHERE PDOC_CODIGO ='" + prefijo + "' AND MTES_NUMERO = " + numeroDocumento + " ";
                        break;

                        case ("MVINVENTARIO_AJUSTESINVCONTABILIDAD"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT NUMERO_AJUSTE, FECHA_DOCUMENTO, 'AJUSTES AL INVENTARIO', COALESCE(VALOR_AJUSTE,1.00), ALMACEN_RELACIONADO, USUARIO, PROCESADO FROM MVINVENTARIO_AJUSTESINVCONTABILIDAD WHERE CODIGO_AJUSTE ='" + prefijo + "' AND FECHA_DOCUMENTO BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by NUMERO_AJUSTE";
                        else
                            sql = "SELECT NUMERO_AJUSTE, FECHA_DOCUMENTO, 'AJUSTES AL INVENTARIO', COALESCE(VALOR_AJUSTE,1.00), ALMACEN_RELACIONADO, USUARIO, PROCESADO FROM MVINVENTARIO_AJUSTESINVCONTABILIDAD WHERE CODIGO_AJUSTE ='" + prefijo + "' AND NUMERO_AJUSTE = " + numeroDocumento + " ";
                        break;

                        case ("MPLANILLAVIAJE"):
                        unAgencia = unAgencia.Replace("MAG_CODIGO","mp.MAG_CODIGO");
                        if (tipoInterfase == "Batch")
                            sql = "SELECT mp.MPLA_CODIGO, mp.FECHA_PLANILLA, mp.MRUT_CODIGO ||CAST(mp.MVIAJE_NUMERO AS CHAR (8)), COALESCE(mp.VALOR_INGRESOS,1.00), mp.MAG_CODIGO, mp.MNIT_RESPONSABLE, mp.FECHA_PLANILLA , mv.mcat_placa, mp.MPLA_CODIGO FROM MPLANILLAVIAJE mp, mviaje mv, MBUSAFILIADO MB WHERE  mp.FECHA_PLANILLA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' " + unAgencia + " and mv.mrut_codigo= mp.mrut_codigo and mv.mviaje_numero=mp.mviaje_numero  AND MV.MCAT_PLACA = MB.MCAT_PLACA order by mp.MPLA_CODIGO;";
                            //sql = "SELECT MPLA_CODIGO, FECHA_PLANILLA, MRUT_CODIGO ||CAST(MVIAJE_NUMERO AS CHAR (8)), COALESCE(VALOR_INGRESOS,1.00), MAG_CODIGO, MNIT_RESPONSABLE, FECHA_PLANILLA FROM MPLANILLAVIAJE WHERE  FECHA_PLANILLA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' " + unAgencia + " order by MPLA_CODIGO";
                        else
                            sql = "SELECT MPLA_CODIGO, FECHA_PLANILLA, MRUT_CODIGO ||CAST(MVIAJE_NUMERO AS CHAR (8)), COALESCE(VALOR_INGRESOS,1.00), MAG_CODIGO, MNIT_RESPONSABLE, FECHA_PLANILLA FROM MPLANILLAVIAJE WHERE MPLA_CODIGO = " + numeroDocumento + " ";
                        break;

                        case ("MPLANILLAVIAJE_CHEQUEO"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT MPLA_CODIGO, FECHA_CHEQUEO, MRUT_CODIGO ||CAST(AGENCIA_CHEQUEADA AS CHAR (8)) || MNIT_INSPECTOR, 1.00, LUGAR_CHEQUEO, MNIT_INSPECTOR, FECHA_CHEQUEO FROM MPLANILLAVIAJE_CHEQUEO WHERE FECHA_CHEQUEO BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by MPLA_CODIGO";
                        else
                            sql = "SELECT MPLA_CODIGO, FECHA_CHEQUEO, MRUT_CODIGO ||CAST(AGENCIA_CHEQUEADA AS CHAR (8)) || MNIT_INSPECTOR, 1.00, LUGAR_CHEQUEO, MNIT_INSPECTOR, FECHA_CHEQUEO FROM MPLANILLAVIAJE_CHEQUEO WHERE MPLA_CODIGO = " + numeroDocumento + " ";
                        break;

                        case ("MENCOMIENDAS"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT NUM_DOCUMENTO, FECHA_RECIBE, DESCRIPCION_CONTENIDO, COALESCE(VALOR_TOTAL,1.00), MAG_RECIBE, MNIT_RESPONSABLE_RECIBE, FECHA_RECIBE, me.MCAT_PLACA, MPLA_CODIGO FROM MENCOMIENDAS me, mbusafiliado mb WHERE FECHA_RECIBE BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' " + unAgenciaAux + " and me.mcat_placa = mb.mcat_placa order by NUM_DOCUMENTO";
                        else
                            sql = "SELECT NUM_DOCUMENTO, FECHA_RECIBE, DESCRIPCION_CONTENIDO, COALESCE(VALOR_TOTAL,1.00), MAG_RECIBE, MNIT_RESPONSABLE_RECIBE, FECHA_RECIBE FROM MENCOMIENDAS WHERE NUM_DOCUMENTO = " + numeroDocumento + " ";
                        break;

                        case ("MGASTOS_TRANSPORTES"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT NUM_DOCUMENTO, FECHA_DOCUMENTO, DESCRIPCION, VALOR_TOTAL_AUTORIZADO, MAG_CODIGO,  MNIT_RESPONSABLE_ENTREGA, FECHA_DOCUMENTO, MCAT_PLACA, CASE COALESCE(MPLA_CODIGO,tcon_codigo) WHEN 288  THEN 0 ELSE MPLA_CODIGO  END   FROM MGASTOS_TRANSPORTES WHERE FECHA_DOCUMENTO BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' " + unAgencia + " order by NUM_DOCUMENTO";
                        else
                            sql = "NUM_DOCUMENTO, FECHA_DOCUMENTO, DESCRIPCION, VALOR_TOTAL_AUTORIZADO, MAG_CODIGO,  MNIT_RESPONSABLE_ENTREGA, FECHA_DOCUMENTO FROM MGASTOS_TRANSPORTES WHERE NUM_DOCUMENTO = " + numeroDocumento + " ";
                        break;

                        case ("DCIERRE_DIARIO_AGENCIA"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT DISTINCT YEAR(FECHA_CONTABILIZACION) CONCAT MONTH(FECHA_CONTABILIZACION) CONCAT DAY(FECHA_CONTABILIZACION), FECHA_CONTABILIZACION, 'CIERRE DIARIO AGENCIAS',1, '9200',  'Contabilidad', FECHA_CONTABILIZACION FROM DCIERRE_DIARIO_AGENCIA WHERE FECHA_CONTABILIZACION BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by FECHA_CONTABILIZACION";
                        else
                            sql = "YEAR(FECHA_CONTABILIZACION) CONCAT MONTH(FECHA_CONTABILIZACION) CONCAT DAY(FECHA_CONTABILIZACION), FECHA_CONTABILIZACION, 'CIERRE DIARIO AGENCIAS',1, '9200',  'Contabilidad', FECHA_CONTABILIZACION FROM MGASTOS_TRANSPORTES WHERE NUM_DOCUMENTO = " + numeroDocumento + " ";
                        break;

                        case ("MVEHICULOENTREGA"):
                        if (tipoInterfase == "Batch")
                            sql = "SELECT Mvee_NUMERO, Mvee_FECHAentrega, mvee_nombnitrecibe, 1, PALM_ALMACEN, 'ENTREGAS', MVEE_FECHAENTREGA FROM Mvehiculoentrega WHERE PDOC_CODIGO ='" + prefijo + "' AND Mvee_FECHAentrega BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' order by Mvee_NUMERO;";
                        else
                            sql = "SELECT Mvee_NUMERO, Mvee_FECHAentrega, mvee_nombnitrecibe, 1, PALM_ALMACEN, 'ENTREGAS', MVEE_FECHAENTREGA FROM Mvehiculoentrega WHERE PDOC_CODIGO ='" + prefijo + "' AND Mvee_FECHAentrega BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' ";
                        break;

                    }
                    // contabiliza anulaciones
                   
                    // termina contabiliza anulaciones
                    //if (!todosAlmacenes && tipoInterfase == "Batch")
                    //    sql+=" AND "+dr[8].ToString()+"='"+almacen+"'";	

					DBFunctions.Request(ds,IncludeSchema.NO,sql);
                   
                    if (ds.Tables.Count > 1)
					{                        

                            
                        if(ds.Tables[1].Rows.Count!=0)
                        {
                            //mensajesCabecera += "Existen registros sin centro de costo definido o sin plianilla registrada. Por favor revisar!" + "<br>";
                                                       
							for(int j=0;j<ds.Tables[1].Rows.Count;j++)
							{
								DataRow fila=tablaCabecera.NewRow();
								fila[0] = prefijo;
								fila[1] = ds.Tables[1].Rows[j][0].ToString();
                                //fila[2] = this.anio.ToString();
                                //fila[3] = this.mes.ToString();
                                fila[2] = Convert.ToDateTime(ds.Tables[1].Rows[j][1]).Year;
                                fila[3] = Convert.ToDateTime(ds.Tables[1].Rows[j][1]).Month;
                                fila[4] = Convert.ToDateTime(ds.Tables[1].Rows[j][1]).ToString("yyyy-MM-dd"); // fecha
                                fila[5] = ds.Tables[1].Rows[j][2].ToString(); // razon;
                                fila[6] = Convert.ToDateTime(ds.Tables[1].Rows[j][6]).ToString("yyyy-MM-dd"); // procesado
								fila[7] = ds.Tables[1].Rows[j][5].ToString(); // usuario
                                fila[8] = ds.Tables[1].Rows[j][3].ToString(); // valor
                                fila[9] = proceso;
								tablaCabecera.Rows.Add(fila);

                                if (ds.Tables[1].Columns.Count > 7 && tabla != ("MGASTOS_TRANSPORTES"))
                                    {
                                        if (ds.Tables[1].Rows[j][7].ToString() == "" || ds.Tables[1].Rows[j][8].ToString() == "")
                                        {
                                            mensajesCabecera += "Existen registros sin centro de costo definido. " + prefijo + "  " + fila[1] + " Por favor revisar!" + "<br>";
                                            return;
                                        }
                                    }

                                else if (ds.Tables[1].Columns.Count > 7 && tabla == ("MGASTOS_TRANSPORTES"))
                                {
                                    if (ds.Tables[1].Rows[j][7].ToString() == "" || ds.Tables[1].Rows[j][8].ToString() == "")
                                    {
                                        string NomAgencia = "";

                                        NomAgencia = DBFunctions.SingleData("select MAGE_NOMBRE  from magencia where mag_codigo = " + ds.Tables[1].Rows[j][4].ToString() + " ;");
                                        mensajesCabecera += NomAgencia + "  codigo agencia  " + ds.Tables[1].Rows[j][4].ToString() + " prefijo:  " + prefijo + " - " + ds.Tables[1].Rows[j][0].ToString() + "<br>";


                                    }
                                }                          
					       }
					}
                       
                    else
                    {
            //           if (!AnulacionesCaja && !AnulacionesTesoreria)
			//			    mensajesCabecera += "Error de Encabezado : El hecho económico "+prefijo+" se encuentra mal parametrizado. Por favor revise su definición <br>";
            //           else
                            if(tipoInterfase == "Online")
                            {
                                if (AnulacionesCaja)
                                {
            //                      Armar_Cabecera_AnulacionesCaja(prefijo);
                                }
                                if (AnulacionesTesoreria)
                                {
                                    Armar_Cabecera_AnulacionesTesoreria(prefijo);
                                }
                            }
                    }
                }
			}
		}
            
        }

        public Hashtable Armar_Cabecera_AnulacionesTesoreria(string prefijo)
		{
			DataSet ds;
			Hashtable HT_prefijos = new Hashtable();
		 	string fechaInicial="",fechaFinal="";
			fechaInicial+=anio.ToString()+"-";
			fechaFinal+=anio.ToString()+"-";
			if(mes>=1 && mes<=9)
				fechaInicial+="0"+mes.ToString()+"-";
			else
				fechaInicial+=mes.ToString()+"-";
			if(diaInicial>=1 && diaInicial<=9)
				fechaInicial+="0"+diaInicial;
			else
				fechaInicial+=diaInicial;

			if(mes>=1 && mes<=9)
				fechaFinal+="0"+mes.ToString()+"-";
			else
				fechaFinal+=mes.ToString()+"-";
			if(diaFinal>=1 && diaFinal<=9)
				fechaFinal+="0"+diaFinal;
			else
				fechaFinal+=diaFinal;

			string Sql=
                "SELECT DTA.*, mts.mtes_fecha, COALESCE(dta.MTES_valoanul,1), mts.MTES_USUARIO, mts.MTES_PROCESO " +
				"FROM DTESORERIAANULACION DTA, MTESORERIA MTS "+
				"WHERE MTS.MTES_FECHA BETWEEN '"+fechaInicial+"' AND '"+fechaFinal+"' AND "+
                "DTA.PDOC_CODIGO=MTS.PDOC_CODIGO AND DTA.MTES_NUMERO=MTS.MTES_NUMERO "+
                " order by DTA.PDOC_CODIGO, DTA.MTES_NUMERO";
			ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,Sql);
			if(ds.Tables[0].Rows.Count!=0)
			{
				for(int j=0;j<ds.Tables[0].Rows.Count;j++)
				{
					HT_prefijos.Add(ds.Tables[0].Rows[j][0].ToString() + "," + ds.Tables[0].Rows[j][1].ToString(),ds.Tables[0].Rows[j][2].ToString() + "," + ds.Tables[0].Rows[j][3].ToString()); 
					DataRow fila=tablaCabecera.NewRow();
					fila[0] = ds.Tables[0].Rows[j][0].ToString();
					fila[1] = ds.Tables[0].Rows[j][1].ToString();
					fila[2] = this.anio.ToString();
					fila[3] = this.mes.ToString();
				    fila[4] = Convert.ToDateTime(ds.Tables[0].Rows[j][5]).ToString("yyyy-MM-dd");
					fila[5] = "ANULACION " + ds.Tables[0].Rows[j][2].ToString() + " - " + ds.Tables[0].Rows[j][3].ToString();
                    fila[6] = Convert.ToDateTime(ds.Tables[0].Rows[j][5]).ToString("yyyy-MM-dd");
                    fila[7] = ds.Tables[0].Rows[j][7].ToString();
					fila[8] = Convert.ToDouble(ds.Tables[0].Rows[j][6].ToString()+"").ToString("C");
                    fila[9] = "";
                    tablaCabecera.Rows.Add(fila);	
				}
			}
			return HT_prefijos;
		}

        public Hashtable Armar_Cabecera_AnulacionesCaja(string prefijo)
		{
			DataSet ds;
			Hashtable HT_prefijos = new Hashtable();
		 	string fechaInicial="",fechaFinal="";
			fechaInicial+=anio.ToString()+"-";
			fechaFinal+=anio.ToString()+"-";
			if(mes>=1 && mes<=9)
				fechaInicial+="0"+mes.ToString()+"-";
			else
				fechaInicial+=mes.ToString()+"-";
			if(diaInicial>=1 && diaInicial<=9)
				fechaInicial+="0"+diaInicial;
			else
				fechaInicial+=diaInicial;

			if(mes>=1 && mes<=9)
				fechaFinal+="0"+mes.ToString()+"-";
			else
				fechaFinal+=mes.ToString()+"-";
			if(diaFinal>=1 && diaFinal<=9)
				fechaFinal+="0"+diaFinal;
			else
				fechaFinal+=diaFinal;

			string Sql="SELECT mac.*, mc.mcaj_valototal FROM MANULACIONCAJA mac, mcaja mc "+
				"WHERE mac.pdoc_codigo = mc.pdoc_codigo and mac.mcaj_numero = mc.mcaj_numero "+
                " and MANU_FECHA BETWEEN '" + fechaInicial + "' AND '" + fechaFinal + "' and MAC.PDOC_CODIANUL ='" + prefijo + "' " +
                " order by mac.PDOC_CODIANUL, mac.MANU_NUMEANUL";
			ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,Sql);
			if(ds.Tables[0].Rows.Count!=0)
			{
				for(int j=0;j<ds.Tables[0].Rows.Count;j++)
				{
					HT_prefijos.Add(ds.Tables[0].Rows[j][0].ToString() + "," + ds.Tables[0].Rows[j][1].ToString(),ds.Tables[0].Rows[j][2].ToString() + "," + ds.Tables[0].Rows[j][3].ToString()); 
					DataRow fila=tablaCabecera.NewRow();
					fila[0] = ds.Tables[0].Rows[j][0].ToString();
					fila[1] = ds.Tables[0].Rows[j][1].ToString();
					fila[2] = this.anio.ToString();
					fila[3] = this.mes.ToString();
                    fila[4] = Convert.ToDateTime(ds.Tables[0].Rows[j][4]).ToString("yyyy-MM-dd");
					fila[5] = "ANULACION " + ds.Tables[0].Rows[j][2].ToString() + " - " + ds.Tables[0].Rows[j][3].ToString();
                    fila[6] = Convert.ToDateTime(ds.Tables[0].Rows[j][5]).ToString("yyyy-MM-dd");
                    fila[7] = ds.Tables[0].Rows[j][6].ToString();
					fila[8] = Convert.ToDouble(ds.Tables[0].Rows[j][7].ToString()).ToString("C");
                    fila[9] = "";
                    tablaCabecera.Rows.Add(fila);	
				}
			}
			return HT_prefijos;
		}

		/// <summary>
		/// Armar_Cabecera : función que me permite armar las cabeceras que
		/// se guardaran en mcomprobante
		/// </summary>
		/// <param name="prefijo">string que tiene el prefijo del documento</param>
		/// <param name="sqlMovimientos">string que trae el sql que me permite generar los movimientos del prefijo</param>
		/// <param name="sqlDetails">string que trae el sql que me permite generar los detalles del prefijo y numero generado</param>
		public void Armar_Cabecera(string prefijo, string sqlMovimientos, string sqlDetails)
		{
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM ppuchechoscabecera WHERE pdoc_codigo='"+prefijo+"'");
			if(ds.Tables[0].Rows.Count!=0)
			{
				for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				{
					DataRow dr=ds.Tables[0].Rows[i];
					DBFunctions.Request(ds,IncludeSchema.NO,sqlMovimientos);
					if(ds.Tables.Count > 1)
					{
						if(ds.Tables[1].Rows.Count!=0)
						{
							for(int j=0;j<ds.Tables[1].Rows.Count;j++)
							{
								DataRow fila=tablaCabecera.NewRow();
								fila[0]=prefijo;
								fila[1]=ds.Tables[1].Rows[j][0].ToString();
								fila[2]=this.anio.ToString();
								fila[3]=this.mes.ToString();
								fila[4]=Convert.ToDateTime(DBFunctions.SingleData("SELECT "+dr[5].ToString()+" FROM "+dr[2].ToString()+" WHERE "+dr[3].ToString()+"='"+prefijo+"' AND "+dr[4].ToString()+"="+ds.Tables[1].Rows[j][0].ToString()+"")).ToString("yyyy-MM-dd");
								fila[5]=DBFunctions.SingleData("SELECT "+dr[6].ToString()+" FROM "+dr[2].ToString()+" WHERE "+dr[3].ToString()+"='"+prefijo+"' AND "+dr[4].ToString()+"="+ds.Tables[1].Rows[j][0].ToString()+"");
								fila[6]=DateTime.Now.Date.ToString("yyyy-MM-dd");//ojo futuro cambio
								fila[7]=this.usuario;
								fila[8]=Convert.ToDouble(DBFunctions.SingleData("SELECT "+dr[7].ToString()+" FROM "+dr[2].ToString()+" WHERE "+dr[3].ToString()+"='"+prefijo+"' AND "+dr[4].ToString()+"="+ds.Tables[1].Rows[j][0].ToString()+"")).ToString("C");
                                fila[9] = dr["TPRO_PROCESO"].ToString();
                                tablaCabecera.Rows.Add(fila);
							}
						}
					}
		//			else
		//				mensajesCabecera += "Error de Encabezado : El hecho económico "+prefijo+" se encuentra mal parametrizado. Por favor revise su definición <br>";
				}
			}
		}


		public void Armar_Detalles_Anulaciones(string prefijo, int numero,string prefijo1,int numero1, string tipo)
		{
			int desde=tablaDetalles.Rows.Count;
			//Armar detalle doc original
			Armar_Detalles(prefijo,numero, numero,tipo);
			int hasta=tablaDetalles.Rows.Count;

			//Invertir valores
			string debito="",credito="";
			for(int n=desde;n<hasta;n++)
			{
			 	tablaDetalles.Rows[n]["PREFIJO"]=prefijo1;
			 	tablaDetalles.Rows[n]["NUMERO"]=numero1;
				tablaDetalles.Rows[n]["RAZON"]="ANULADO: "+tablaDetalles.Rows[n]["RAZON"].ToString();
				debito=tablaDetalles.Rows[n]["DEBITO"].ToString();
				credito=tablaDetalles.Rows[n]["CREDITO"].ToString();
				tablaDetalles.Rows[n]["DEBITO"]=credito;
				tablaDetalles.Rows[n]["CREDITO"]=debito;
			}
		}

		/// <summary>
		/// Armar_Detalles : función que me permite armar los detalles de
		/// cada documento que se guardara en mcomprobante y dcuenta
		/// </summary>
		/// <param name="prefijo">string que tiene el prefijo del documento</param>
		/// <param name="numero">int que tiene el número del documento</param>
        public void Armar_Detalles(string prefijo, int numeroInicial, int numeroFinal, string tipo)
        {
            ArrayList arrDetalleFOTA = new ArrayList();
            
            bool ingresoAjusteTaller = false;
            bool ingresoAjusteDTesoreria = false;
            bool ingresoAjusteDGastosVariosProveedor = false;
            double creditosEncabezado = 0, debitosEncabezado = 0;
            bool errorCuenta = false;
            DataRow filaEliminar = null;
            // desarrollo tabla detalles con un solo select para todos los campos
            // hac 2008-05-20
            DataSet dsI = new DataSet();
            if (tipo.Length > 0)
                DBFunctions.Request(dsI, IncludeSchema.NO, "SELECT SIMP_SQL FROM DBXSCHEMA.SIMPUTACIONCONTABLE"); // WHERE (TPRO_PROCESO='" + tipo + "' OR TPRO_PROCESO IS NULL)");
            else
                DBFunctions.Request(dsI, IncludeSchema.NO, "SELECT SIMP_SQL FROM DBXSCHEMA.SIMPUTACIONCONTABLE");
            for (int i = 0; i < dsI.Tables[0].Rows.Count; i++)
            {
                DataSet dsD = new DataSet();
                string sqlD = dsI.Tables[0].Rows[i]["SIMP_SQL"].ToString();
                sqlD = sqlD.Replace("@NUMEROINICIAL@", numeroInicial.ToString());
                sqlD = sqlD.Replace("@NUMEROFINAL@", numeroFinal.ToString());
                sqlD = sqlD.Replace("@FECHAINICIAL@", fechaInicial.ToString());
                sqlD = sqlD.Replace("@FECHAFINAL@", fechaFinal.ToString());
                sqlD = sqlD.Replace("@PREFIJO@", prefijo);
                sqlD = sqlD.Replace("@", "'");
                sqlD = sqlD.Replace("\r\n", " ");
                sqlD = sqlD.Replace("\t", " ");
                DBFunctions.Request(dsD, IncludeSchema.NO, sqlD);
                try
                {
                    if (dsD.Tables[0].Rows.Count > 0)
                    {
                        for (int d = 0; d < dsD.Tables[0].Rows.Count; d++)
                        {
                            DataRow fila = tablaDetalles.NewRow();
                            /* 
                            valorImputa = Convert.ToInt32(dsD.Tables[0].Rows[d][9].ToString());
                            if (valorImputa < 0.00)
                            {
                                dsD.Tables[0].Rows[d][10] =(valorImputa * -1).ToString();
                                dsD.Tables[0].Rows[d][9] = 0;
                            }
                            valorImputa = Convert.ToInt32(dsD.Tables[0].Rows[d][10].ToString());
                            if (valorImputa < 0.00)
                            {
                                dsD.Tables[0].Rows[d][9] = (valorImputa * -1).ToString();
                                dsD.Tables[0].Rows[d][10] = 0;
                            }
                            valorImputa = Convert.ToInt32(dsD.Tables[0].Rows[d][11].ToString());
                            if (valorImputa < 0.00)
                            {
                                dsD.Tables[0].Rows[d][11] = (valorImputa * -1).ToString();
                            }
                            */
                            for (int r = 0; r < tablaDetalles.Columns.Count && r < dsD.Tables[0].Columns.Count; r++)
                            {
                                // columnas: (9) = Debito fiscal (10) = Credito fiscal (11) = Valor Base (12) = debito niif (13) credito niif
                                fila[r] = ((r == 9 || r == 10 || r == 11 || r == 12 || r == 13) ? Convert.ToDouble(dsD.Tables[0].Rows[d][r]).ToString("C") : dsD.Tables[0].Rows[d][r]);
                            }
                            if (fila[8].ToString().Trim().Length == 0)
                                fila[8] = prefijo + "-" + numeroInicial.ToString();
                            //fila[2].ToString().Length > 2
                            if (Convert.ToDouble(fila[9].ToString().Substring(1)) == 0 && Convert.ToDouble(fila[10].ToString().Substring(1)) == 0 && (fila[2].ToString() == "" || fila[2].ToString().Substring(0,3) == "DES" || fila[2].ToString() == null) )
                                // los registros en ceros sin cuenta valida como los DEScuentos o Nulos o Vacios NO se incluyen
                                errorCuenta = true;
                            else
                            {
                                tablaDetalles.Rows.Add(fila);
                                arrDetalleFOTA.Add(fila);
                            }
                            if (DatasToControls.ValidarDouble(fila[10].ToString().Substring(1)))
                                creditosEncabezado += Convert.ToDouble(fila[10].ToString().Substring(1));
                            if (DatasToControls.ValidarDouble(fila[9].ToString().Substring(1)))
                                debitosEncabezado += Convert.ToDouble(fila[9].ToString().Substring(1));
                        }
                    }
                }
                catch (Exception e1)
                {
                    erroresSql = "Error ejecutando la sentencia: " + sqlD + ". " + DBFunctions.exceptions + ". " + e1.Message + ". " + e1.StackTrace;
                    return;
                }
            }

            if (ingresoAjusteTaller)
                CuadrarComprobanteAjusteAseguradoraTaller(arrDetalleFOTA);
            if (ingresoAjusteDTesoreria)
                CuadrarComprobanteConsignacion(arrDetalleFOTA);
            if (ingresoAjusteDGastosVariosProveedor && filaEliminar != null)
                tablaDetalles.Rows.Remove(filaEliminar);

            //Ajustes por minima diferencia
            if (tablaDetalles.Rows.Count > 0) // verifica si genero registros
            {
                string limAjus      = DBFunctions.SingleData("SELECT CCON_AJUSLIMI FROM CCONTABILIDAD;");
                string cuentaAJUSTE = "FALTA CTA AJUSPE";
                try
                {
                    cuentaAJUSTE = DBFunctions.SingleData("SELECT MCUE_CODIPUC_AJUSPESO FROM CCONTABILIDAD;");
                }
                catch
                {
                    // SE debe definir en ccontabilidad el campo MCUE_CODIPUC_AJUSPESO en ves del campo del numero de sentencia 555
                }
                double limiteAjuste = 0;
                if (limAjus.Length > 0 && cuentaAJUSTE.Length > 0)
                {
                    limiteAjuste = Convert.ToDouble(limAjus);
                    try
                    {
                        for (int hc = 0; hc < tablaCabecera.Rows.Count; hc++) //recorre cada documento = cabecera
                        {
                            creditosEncabezado = debitosEncabezado = 0;
                            string prefijoDoc  = tablaCabecera.Rows[hc][0].ToString() ;
                            Int32  numeroDoc   = Convert.ToInt32(tablaCabecera.Rows[hc][1]);
                            DataRow fila = tablaDetalles.NewRow();
                            for (int c = 0; c < tablaDetalles.Rows.Count; c++)  // recorre todas las filas
                            {
                                if (tablaDetalles.Rows[c][1].ToString () != "NUMERO"
                                    && prefijoDoc == tablaDetalles.Rows[c][0].ToString() 
                                    && numeroDoc == Convert.ToInt32(tablaDetalles.Rows[c][1])
                                   ) // acumula solo las filas que corresponden al comprobante
                                {
                                    fila[0] = tablaDetalles.Rows[c][0]; // prefijo
                                    fila[1] = tablaDetalles.Rows[c][1]; // numero
                                    fila[2] = cuentaAJUSTE;             // cuenta
                                    fila[3] = tablaDetalles.Rows[c][3]; // prefijo Ref
                                    fila[4] = tablaDetalles.Rows[c][4]; // numero Ref
                                    fila[5] = tablaDetalles.Rows[c][5]; // nit
                                    fila[6] = tablaDetalles.Rows[c][6]; // sede
                                    fila[7] = tablaDetalles.Rows[c][7]; // c.costo
                                    fila[8] = tablaDetalles.Rows[c][8]; // detalle
                                    fila[9] = fila[10] = fila[11] = "$0.00";  // debito credito base
                                    try
                                    {
                                        debitosEncabezado += Convert.ToDouble(tablaDetalles.Rows[c][09].ToString().Substring(1));
                                    }
                                    catch
                                    {

                                    };
                                    try
                                    {
                                        creditosEncabezado += Convert.ToDouble(tablaDetalles.Rows[c][10].ToString().Substring(1));
                                    }
                                    catch
                                    {

                                    };
                                }
                            }
                            if (creditosEncabezado != debitosEncabezado && Math.Abs(creditosEncabezado) != 0 && Math.Abs(debitosEncabezado) != 0)
                            {
                                if (creditosEncabezado > debitosEncabezado)
                                    fila[9] = (creditosEncabezado - debitosEncabezado).ToString("C");
                                else
                                    fila[10] = (debitosEncabezado - creditosEncabezado).ToString("C");
                                if (fila[8].ToString().Trim().Length == 0)
                                    fila[8] = prefijo + "-" + tablaDetalles.Rows[0][1].ToString();
                                if (Math.Abs(creditosEncabezado - debitosEncabezado) <= limiteAjuste)
                                {
                                    tablaDetalles.Rows.Add(fila);
                                    arrDetalleFOTA.Add(fila);
                                }
                            }
                        }
                    }
                    catch (Exception e1)
                    {
                        erroresSql = "Error ejecutando el ajuste de diferencia: " + DBFunctions.exceptions + ". " + e1.Message + ". " + e1.StackTrace;
                        return;
                    }
                }
            }
        }	 


		/// <summary>
		/// Estructura_TablaCabecera : función que establece una estructura
		/// para el DataTable tablaCabecera
		/// </summary>
		private void Estructura_TablaCabecera()
		{
			tablaCabecera=new DataTable();
			tablaCabecera.Columns.Add("PREFIJO",typeof(string));//0
			tablaCabecera.Columns.Add("NUMERO",typeof(string));//1
			tablaCabecera.Columns.Add("ANIO",typeof(string));//2
			tablaCabecera.Columns.Add("MES",typeof(string));//3
			tablaCabecera.Columns.Add("FECHA",typeof(string));//4
			tablaCabecera.Columns.Add("RAZON",typeof(string));//5
			tablaCabecera.Columns.Add("PROCESADO",typeof(string));//6
			tablaCabecera.Columns.Add("USUARIO",typeof(string));//7
			tablaCabecera.Columns.Add("VALOR",typeof(string));//8
            tablaCabecera.Columns.Add("TIPO", typeof(string));//9
		}

		/// <summary>
		/// Estructura_TablaDetalles : función que establece una estructura
		/// para el DataTable tablaDetalles
		/// </summary>
		private void Estructura_TablaDetalles()
		{
			tablaDetalles=new DataTable();
			tablaDetalles.Columns.Add("PREFIJO",typeof(string)); //0
			tablaDetalles.Columns.Add("NUMERO",typeof(string));//1
			tablaDetalles.Columns.Add("CUENTA",typeof(string));//2
			tablaDetalles.Columns.Add("PREFIJOREF",typeof(string));//3
			tablaDetalles.Columns.Add("NUMEROREF",typeof(string));//4
			tablaDetalles.Columns.Add("NIT",typeof(string));//5
			tablaDetalles.Columns.Add("ALMACEN",typeof(string));//6
			tablaDetalles.Columns.Add("CENTROCOSTO",typeof(string));//7
			tablaDetalles.Columns.Add("RAZON",typeof(string));//8
			tablaDetalles.Columns.Add("DEBITO",typeof(string));//9
			tablaDetalles.Columns.Add("CREDITO",typeof(string));//10
			tablaDetalles.Columns.Add("BASE",typeof(string));//11
            tablaDetalles.Columns.Add("DEBITONIIF", typeof(string));//12
            tablaDetalles.Columns.Add("CREDITONIIF", typeof(string));//13
        }

		/// <summary>
		/// Realizar Consulta : función que hace una consulta a la base de datos
		/// y verifica si la sentencia sql esta bien formada
		/// </summary>
		/// <param name="ds">DataSet por referencia donde se guardaran los resultados de la consulta</param>
		/// <param name="sql">string que contiene la consulta</param>
		/// <param name="error">string por referencia donde se almacenará el error</param>
		/// <returns>bool true: si la consulta fue exitosa, false: si la consulta tiene errores. N se considera error que la consulta no devuelva datos</returns>
		private bool Realizar_Consulta(ref DataSet ds, string sql,ref string error)
		{
			bool exito=false;
			DBFunctions.Request(ds,IncludeSchema.NO,sql);
			if(ds.Tables.Count!=0)
				exito=true;
			else
				error=sql + "<br>Detalles : "+DBFunctions.exceptions;
			return exito;
		}

		/// <summary>
		/// GuardarInterface : función que realizara el proceso de inserción de
		/// los datos contables en la base de datos
		/// </summary>
		/// <returns>bool true: si el proceso fue exitoso; false: si existe algun error</returns>
		public bool GuardarInterface()
		{
			ArrayList cuentasAgregadasMSC = new ArrayList();
			ArrayList cuentasAgregadasMAC = new ArrayList();
			ArrayList sqlStrings=new ArrayList();
			bool   exito  = true;
            bool sicolumnaNiif = false;
            double totalDebitos = 0, totalCreditos = 0, totalUtilidad = 0, totalDebitosNiif = 0, totalCreditosNiif = 0, totalUtilidadNiif = 0;
            string imputacion = "", naturaleza = "", cuentaUtilidad = "", claseCuenta = "", cuentaNiif = "";
            double parcialDebitos = 0, parcialCreditos = 0, parcialDebitosNiif = 0, parcialCreditosNiif = 0;
            double debito = 0, credito = 0, valorBase = 0, debitoNiif = 0, creditoNiif = 0;
            bool errorCuenta = false;
			
			for(int i=0;i<tablaCabecera.Rows.Count;i++)
			{
                parcialDebitos = parcialCreditos = parcialDebitosNiif = parcialCreditosNiif = 0;
                ArrayList sqlStringsDetail = new ArrayList();
				DataRow[] hijos=tablaDetalles.Select("PREFIJO='"+tablaCabecera.Rows[i][0].ToString()+"' AND NUMERO='"+tablaCabecera.Rows[i][1].ToString()+"'");
				if(hijos.Length!=0)
				{
                    if (!ExisteComprobante(tablaCabecera.Rows[i][0].ToString(), Convert.ToInt32(tablaCabecera.Rows[i][1])))
                    {
                        //if(VerificarCuadreDocumentos(hijos))
                        //{
                        //sqlStringsDetail.Add("INSERT INTO mcomprobante VALUES('"+tablaCabecera.Rows[i][0].ToString().Replace("'","")+"',"+tablaCabecera.Rows[i][1].ToString()+","+tablaCabecera.Rows[i][2].ToString()+","+tablaCabecera.Rows[i][3].ToString()+",'"+(DateTime)tablaCabecera.Rows[i][4]+"','"+tablaCabecera.Rows[i][5].ToString().Replace("'","")+"','"+(DateTime)tablaCabecera.Rows[i][6]+"','"+tablaCabecera.Rows[i][7].ToString()+"',"+Convert.ToDouble(tablaCabecera.Rows[i][8].ToString().Replace("(","").Replace(")","").Substring(1)).ToString()+")");
                        Double valor = Convert.ToDouble(tablaCabecera.Rows[i][8].ToString().Replace("(", "").Replace(")", "").Replace("$", ""));
                        sqlStringsDetail.Add("INSERT INTO mcomprobante VALUES('" + tablaCabecera.Rows[i][0].ToString().Replace("'", "") + "'," + tablaCabecera.Rows[i][1].ToString() + "," + tablaCabecera.Rows[i][2].ToString() + "," + tablaCabecera.Rows[i][3].ToString() + ",'" + tablaCabecera.Rows[i][4].ToString() + "','" + tablaCabecera.Rows[i][5].ToString().Replace("'", "") + "','" + tablaCabecera.Rows[i][6].ToString() + "','" + tablaCabecera.Rows[i][7].ToString() + "'," + valor + ")");
                        //															prefijo doc								numero doc									año										mes										fecha										razon								procesado										usuario															valor														
                        for (int j = 0; j < hijos.Length; j++)
                        {
                            if (hijos[j][9].ToString() != String.Empty && hijos[j][10].ToString() != String.Empty)
                            {
                                debito = credito = valorBase = debitoNiif = creditoNiif = 0;
                                DataSet cuenta = new DataSet();
                                errorCuenta = false;
                                DBFunctions.Request(cuenta, IncludeSchema.NO, "SELECT timp_codigo, tcla_codigo, tcue_codigo, mcue_codipucniif FROM mcuenta WHERE mcue_codipuc='" + hijos[j][2].ToString() + "'");
                                try
                                {
                                    imputacion = cuenta.Tables[0].Rows[0][0].ToString();
                                    naturaleza = cuenta.Tables[0].Rows[0][1].ToString();   // real, nominal
                                    claseCuenta = cuenta.Tables[0].Rows[0][2].ToString();  // fiscal, generica o niif
                                    cuentaNiif = cuenta.Tables[0].Rows[0][3].ToString();
                                }
                                catch  // cuando la cuenta no existe en el puc se ignora ese registro
                                {
                                    errorCuenta = true;
                                }
                                if (errorCuenta)
                                {
                                    mensajes += " Error en definición de cuenta "  + hijos[j][2].ToString() + ", proceso CANCELADO !! ";
                                    break;
                                }
                               
                                if (DatasToControls.ValidarDouble(hijos[j][9].ToString().Substring(1)))
                                    debito = Convert.ToDouble(hijos[j][9].ToString().Substring(1));
                                if (DatasToControls.ValidarDouble(hijos[j][10].ToString().Substring(1)))
                                    credito = Convert.ToDouble(hijos[j][10].ToString().Substring(1));
                                if (DatasToControls.ValidarDouble(hijos[j][11].ToString().Substring(1)))
                                    valorBase = Convert.ToDouble(hijos[j][11].ToString().Substring(1));

                                try // se valida si vienen creadas las columnas para debitoNiif y creditoNiif
                                {
                                    debitoNiif = Convert.ToDouble(hijos[j][12].ToString().Substring(1));
                                    sicolumnaNiif = true;
                                }
                                catch
                                {
                                    debitoNiif = 0;
                                    sicolumnaNiif = false;
                                }

                                try // se valida si vienen creadas las columnas para debitoNiif y creditoNiif
                                {
                                    creditoNiif = Convert.ToDouble(hijos[j][13].ToString().Substring(1));
                                    sicolumnaNiif = true;
                                }
                                catch
                                {
                                    creditoNiif = 0;
                                    sicolumnaNiif = false;
                                }

                                if (!sicolumnaNiif)
                                {
                                    if (claseCuenta == "F" || (claseCuenta == "F" && cuentaNiif != null))
                                    {
                                        debitoNiif = 0;
                                        creditoNiif = 0;
                                    }
                                    else if (claseCuenta == "G" || (claseCuenta == "F" && cuentaNiif == null))
                                    {
                                            debitoNiif = debito;
                                            creditoNiif = credito;
                                    }
                                }
                                // Imputacion de cuentas FISCALES y GENERICAS
                                imputarRegistro(hijos, j, debito, credito, valorBase, debitoNiif, creditoNiif, ref sqlStringsDetail, ref cuentasAgregadasMSC, ref cuentasAgregadasMAC, imputacion, naturaleza, ref parcialDebitos, ref parcialDebitosNiif, ref parcialCreditos, ref parcialCreditosNiif);
                                if (claseCuenta == "F" && cuentaNiif != null)
                                {
                                    debitoNiif = debito;
                                    creditoNiif = credito;
                                    debito = 0;
                                    credito = 0;
                                    // Imputación de cuentas NIIFS exclusivamente
                                    if (hijos[j][2].ToString() != cuentaNiif && cuentaNiif != null && cuentaNiif != "")
                                        hijos[j][2] = cuentaNiif.ToString();  // cuenta equivalente para Niif
                                    imputarRegistro(hijos, j, debito, credito, valorBase, debitoNiif, creditoNiif, ref sqlStringsDetail, ref cuentasAgregadasMSC, ref cuentasAgregadasMAC, imputacion, naturaleza, ref parcialDebitos, ref parcialDebitosNiif, ref parcialCreditos, ref parcialCreditosNiif);
                                }
                            }
                        }
                        bool exitoInsercion = DBFunctions.Transaction(sqlStringsDetail);
                        if (!exitoInsercion)
                        {
                            mensajes += "Error al guardar : Detalles <br>" + DBFunctions.exceptions;
                            exito = false;
                        }
                        else
                        {
                            totalDebitos      += parcialDebitos;
                            totalDebitosNiif  += parcialDebitosNiif;

                            totalCreditos     += parcialCreditos;
                            totalCreditosNiif += parcialCreditosNiif;
                        }
                        //}
                    }
                    else
                        mensajes += " El comprobante : " + tablaCabecera.Rows[i][0].ToString() + " - " + tablaCabecera.Rows[i][1].ToString() + " YA EXISTE en Contabilidad. IGNORADA esta imputación";
				}
			}

			totalUtilidad     = totalCreditos     - totalDebitos;
            totalUtilidadNiif = totalCreditosNiif - totalDebitosNiif;
            //  Utilidad
            //  Cuenta REAL de la Utilidad ... unificar esta ruta con la de comprobantes
			cuentaUtilidad=DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");
			if(ExisteSaldoCuenta(cuentaUtilidad))
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valocred=msal_valocred+" + totalUtilidad.ToString() + ", msal_NIIFcred = msal_NIIFcred + " + totalUtilidadNiif.ToString() + " WHERE pano_ano=" + anio + " AND pmes_mes=" + mes + " AND mcue_codipuc='" + cuentaUtilidad + "'");
				//																			valor credito							año					mes								cuenta
			else
                sqlStrings.Add("INSERT INTO msaldocuenta VALUES(" + anio + "," + mes + ",'" + cuentaUtilidad + "',0," + totalUtilidad.ToString() + ",0,0," + totalUtilidadNiif.ToString() + ")");
			   //												  año	   mes			cuenta		  vd		valor credito		valor inflacion			
            //  Cuenta NOMINAL de la Utilidad ... unificar esta ruta con la de comprobantes
            cuentaUtilidad = DBFunctions.SingleData("SELECT mcue_codipucNOMI FROM ccontabilidad");
            if (ExisteSaldoCuenta(cuentaUtilidad))
                sqlStrings.Add("UPDATE msaldocuenta SET msal_valoDEBI = msal_valoDEBI+" + totalUtilidad.ToString() + ", msal_NIIFDEBI = msal_niifDEBI+" + totalUtilidadNiif.ToString() + " WHERE pano_ano=" + anio + " AND pmes_mes=" + mes + " AND mcue_codipuc='" + cuentaUtilidad + "'");
            //																			valor credito							año					mes								cuenta
            else
                sqlStrings.Add("INSERT INTO msaldocuenta VALUES(" + anio + "," + mes + ",'" + cuentaUtilidad + "'," + totalUtilidad.ToString() + ",0,0," + totalUtilidadNiif.ToString() + ",0)");
            //												  año	   mes			cuenta		  vd		valor credito		valor inflacion			
      
            if (!DBFunctions.Transaction(sqlStrings))
            {
                exito = false;
                mensajes += "Error al guardar : Detalles <br>" + DBFunctions.exceptions;
            }
            else
                mensajes += "Proceso Realizado Exitosamente, Revice su Contabilidad";

			return exito;
		}

        protected void imputarRegistro(DataRow[] hijos, int j,  double debito, double credito, double valorBase, double debitoNiif, double creditoNiif, ref ArrayList sqlStringsDetail, ref ArrayList cuentasAgregadasMSC, ref ArrayList cuentasAgregadasMAC,  string imputacion, string  naturaleza, ref double parcialDebitos, ref double parcialDebitosNiif, ref double parcialCreditos, ref double parcialCreditosNiif)
        {
            if (!DebitoEsCero(debito) || !CreditoEsCero(credito) || !DebitoEsCero(debitoNiif) || !CreditoEsCero(creditoNiif) || j == 0)  // Si el primer registro viene cen ceros tambien se graba en la contabilidad
            {
                // por ahora inserto ceros en el debitoNiif y creditoNiif
                try { sqlStringsDetail.Add("INSERT INTO dcuenta VALUES('" + hijos[j][0].ToString() + "'," + hijos[j][1].ToString() + ",'" + hijos[j][2].ToString() + "','" + hijos[j][3].ToString() + "'," + hijos[j][4].ToString() + "," + (j + 1).ToString() + ",'" + hijos[j][5].ToString() + "','" + hijos[j][6].ToString().Replace("'", "") + "','" + hijos[j][7].ToString().Replace("'", "") + "','" + hijos[j][8].ToString().Replace("'", "") + "'," + debito.ToString() + "," + credito.ToString() + "," + valorBase.ToString() + "," + debitoNiif.ToString() + "," + creditoNiif.ToString() + ")"); }
                catch { sqlStringsDetail.Add("INSERT INTO dcuenta VALUES('" + hijos[j][0].ToString() + "'," + hijos[j][1].ToString() + ",'" + hijos[j][2].ToString() + "','" + hijos[j][3].ToString() + "'," + hijos[j][4].ToString() + "," + (j + 1).ToString() + ",'" + hijos[j][5].ToString() + "','" + hijos[j][6].ToString().Replace("'", "") + "','" + hijos[j][7].ToString().Replace("'", "") + "','" + hijos[j][8].ToString().Replace("'", "") + "'," + hijos[j][9] + "," + hijos[j][10] + "," + hijos[j][11] + "," + debitoNiif.ToString() + "," + creditoNiif.ToString() + ")"); }
                //													prefijo							numero					cuenta						prefijo ref					numero ref				secuencia				nit								almacen						centro costo									detalle												valor debito												valor credito							valor base
                //if(ExisteSaldoCuenta(hijos[j][2].ToString()) || cuentasAgregadasMSC.BinarySearch(hijos[j][2].ToString().Trim()) >= 0)
                if (ExisteSaldoCuenta(hijos[j][2].ToString()) || BuscarValorArrayList(cuentasAgregadasMSC, hijos[j][2].ToString().Trim()))
                {
                    sqlStringsDetail.Add("UPDATE msaldocuenta SET msal_valodebi=msal_valodebi+" + debito.ToString() + ",msal_valocred=msal_valocred+" + credito.ToString() + ", msal_NIIFdebi=msal_NIIFdebi+" + debitoNiif.ToString() + ", msal_NIIFcred=msal_NIIFcred+" + creditoNiif.ToString() + " WHERE pano_ano=" + anio + " AND pmes_mes=" + mes + " AND mcue_codipuc='" + hijos[j][2].ToString() + "'");
                    //																					valor debito									valor credito						debito NIif										                credito Niif                                         año	                   mes							cuenta
                }
                else
                {
                    // por ahora inserto ceros en el debitoNiif y creditoNiif
                    sqlStringsDetail.Add("INSERT INTO msaldocuenta VALUES(" + anio + "," + mes + ",'" + hijos[j][2].ToString() + "'," + debito.ToString() + "," + credito.ToString() + ",0, " + debitoNiif.ToString() + " ," + creditoNiif.ToString() + " )");
                    //												   año	   mes			cuenta									valor debito				valor credito		 valor inflacion debitoNiif              creditoNiif
                    cuentasAgregadasMSC.Add(hijos[j][2].ToString().Trim());
                }
                if (imputacion == "A")
                {
                    if (ExisteCuentaAuxiliar(hijos[j][2].ToString(), hijos[j][5].ToString().Trim()) || BuscarValorArrayList(cuentasAgregadasMAC, anio + "%" + mes + "%" + hijos[j][2].ToString() + "%" + hijos[j][5].ToString().Trim()))
                        sqlStringsDetail.Add("UPDATE mauxiliarcuenta SET maux_valodebi=maux_valodebi+" + debito.ToString() + ",maux_valocred=maux_valocred+" + credito.ToString() + ", maux_niifdebi=maux_niifdebi+" + debitoNiif.ToString() + ",maux_niifcred=maux_niifcred+" + creditoNiif.ToString() + " WHERE pano_ano=" + anio + " AND pmes_mes=" + mes + " AND mcue_codipuc='" + hijos[j][2].ToString() + "' AND mnit_nit='" + hijos[j][5].ToString().Trim() + "'");
                    //																							valor debito									valor credito								        debitoNiif                                           Credito Niif                                          año					     mes									cuenta									nit
                    else
                    {
                        // por ahora inserto ceros en el debitoNiif y creditoNiif
                        sqlStringsDetail.Add("INSERT INTO mauxiliarcuenta VALUES(" + anio + "," + mes + ",'" + hijos[j][2].ToString().Trim() + "','" + hijos[j][5].ToString().Trim() + "'," + debito.ToString() + "," + credito.ToString() + "," + debitoNiif.ToString() + "," + creditoNiif.ToString() + ")");
                        //													  año	  mes				cuenta						nit									valor debito																valor credito
                        cuentasAgregadasMAC.Add(anio + "%" + mes + "%" + hijos[j][2].ToString() + "%" + hijos[j][5].ToString().Trim());
                    }
                }
                if (naturaleza == "N")
                {
                    parcialDebitos = parcialDebitos + debito;
                    parcialDebitosNiif = parcialDebitosNiif + debitoNiif; // calumna de Niif

                    parcialCreditos = parcialCreditos + credito;
                    parcialCreditosNiif = parcialCreditosNiif + creditoNiif;  // columan de Niif
                }
            }
        }


		/// <summary>
		/// VerificarCuadreDocumentos : función que realiza el proceso de verificar si
		/// las sumas de valores debitos y creditos en el comprobante son iguales
		/// </summary>
		/// <param name="hijos">DataRow[] que trae las filas detalle del comprobante</param>
		/// <returns>bool true: si las sumas son iguales; false: si las sumas son desiguales</returns>
		private bool VerificarCuadreDocumentos(DataRow[] hijos)
		{
			bool exito=false;
            double valorDebito = 0, valorCredito = 0, valorDebitoNiif = 0, valorCreditoNiif = 0;
			for(int i=0;i<hijos.Length;i++)
			{
				if(hijos[i][9].ToString() != String.Empty && Convert.ToDouble(hijos[i][9].ToString().Substring(1))>0)
					valorDebito=valorDebito+Convert.ToDouble(hijos[i][9].ToString().Substring(1));
				if(hijos[i][10].ToString()!= String.Empty && Convert.ToDouble(hijos[i][10].ToString().Substring(1))>0)
					valorCredito=valorCredito+Convert.ToDouble(hijos[i][10].ToString().Substring(1));
                // (11) Es la Base
                if (hijos[i][12].ToString() != String.Empty && Convert.ToDouble(hijos[i][12].ToString().Substring(1)) > 0)
                    valorDebitoNiif = valorDebito + Convert.ToDouble(hijos[i][12].ToString().Substring(1));
                if (hijos[i][13].ToString() != String.Empty && Convert.ToDouble(hijos[i][13].ToString().Substring(1)) > 0)
                    valorCreditoNiif = valorCredito + Convert.ToDouble(hijos[i][13].ToString().Substring(1));
			}
            if (valorCredito == valorDebito && valorCreditoNiif == valorDebitoNiif)
				exito=true;
			return exito;
		}

		/// <summary>
		/// ExisteComprobante : función que realiza el proceso de verificar la existencia
		/// de un comprobante en la base de datos
		/// </summary>
		/// <param name="prefijo">string que trae el valor del prefijo del documento</param>
		/// <param name="numero">int que trae el valor del número del documento</param>
		/// <returns>bool true: si el comprobante existe; false: si el comprobante no existe</returns>
		private bool ExisteComprobante(string prefijo,int numero)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT * FROM mcomprobante WHERE pdoc_codigo='"+prefijo+"' AND mcom_numedocu="+numero.ToString()+""))
				existe=true;
			return existe;
		}

		/// <summary>
		/// ExisteCuentaAuxiliar  : función que realiza el proceso de verificar si la
		/// la combinacion año, mes, cuenta, nit existe en mauxiliarcuenta
		/// </summary>
		/// <param name="cuenta">string que trae el codigo de la cuenta contable</param>
		/// <param name="nit">string que trae el nit del paciente XD</param>
		/// <returns>bool true: si la combinacion existe; false: si la combinacion no existe</returns>
		private bool ExisteCuentaAuxiliar(string cuenta,string nit)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT * FROM mauxiliarcuenta WHERE pano_ano="+anio+" AND pmes_mes="+mes+" AND mcue_codipuc='"+cuenta+"' AND mnit_nit='"+nit+"'"))
				existe=true;
			return existe;
		}

		/// <summary>
		/// ExisteSaldoCuenta  : función que realiza el proceso de verificar si la
		/// la combinacion año, mes, cuenta, existe en msaldocuenta
		/// </summary>
		/// <param name="cuenta">string que trae el codigo de la cuenta contable</param>
		/// <returns>bool true: si la combinacion existe; false: si la combinacion no existe</returns>
		private bool ExisteSaldoCuenta(string cuenta)
		{
			bool existe=false;
			if(DBFunctions.RecordExist("SELECT * FROM msaldocuenta WHERE pano_ano="+anio+" AND pmes_mes="+mes+" AND mcue_codipuc='"+cuenta+"'"))
				existe=true;
			return existe;
		}

		/// <summary>
		/// DebitoEsCero : función que verifica si el valor de los débitos de un
		/// detalle es cero
		/// </summary>
		/// <returns>bool true: si el valor es cero; false: si el valor es distinto de cero</returns>
		private bool DebitoEsCero(double valor)
		{
			if(valor==0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// DebitoEsCero : función que verifica si el valor de los débitos de un
		/// detalle es cero
		/// </summary>
		/// <returns>bool true: si el valor es cero; false: si el valor es distinto de cero</returns>
		private bool DebitoEsCero(string valorStr)
		{
			double valor = 0;
			try{valor = Convert.ToDouble(valorStr);}
			catch{}
			if(valor==0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// CreditoEsCero : función que verifica si el valor de los créditos de un
		/// detalle es cero
		/// </summary>
		/// <returns>bool true: si el valor es cero; false: si el valor es distinto de cero</returns>
		private bool CreditoEsCero(double valor)
		{
			if(valor==0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// CreditoEsCero : función que verifica si el valor de los créditos de un
		/// detalle es cero
		/// </summary>
		/// <returns>bool true: si el valor es cero; false: si el valor es distinto de cero</returns>
		private bool CreditoEsCero(string valorStr)
		{
			double valor = 0;
			try{valor = Convert.ToDouble(valorStr);}
			catch{}
			if(valor==0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// TraerNaturaleza : función que realiza la tarea de extraer las distintas
		/// naturalezas que puede tener un detalle de la tabla dcajavarios
		/// </summary>
		/// <param name="prefijo">string que trae el prefijo del documento</param>
		/// <param name="numero">string que trae el número del documento</param>
		/// <returns>ArrayList con las distintas naturalezas encontradas</returns>
		private ArrayList TraerNaturaleza(string prefijo,string numero)
		{
			ArrayList naturalezas=new ArrayList();
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT dcaj_naturaleza FROM dcajavarios WHERE pdoc_codigo='"+prefijo+"' AND mcaj_numero="+numero+"");
			if(ds.Tables.Count!=0)
			{
				if(ds.Tables[0].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						naturalezas.Add(ds.Tables[0].Rows[i][0].ToString());
				}
			}
			return naturalezas;
		}

		/// <summary>
		/// TraerNaturaleza : función que realiza la tarea de extraer las distintas
		/// naturalezas que puede tener un detalle de la tabla dcajavarios
		/// </summary>
		/// <param name="prefijo">string que trae el prefijo del documento</param>
		/// <param name="numero">string que trae el número del documento</param>
		/// <param name="tablaGastosVarios">string con el nombre de la tabla</param>
		/// <returns>ArrayList con las distintas naturalezas encontradas</returns>
		private ArrayList TraerNaturaleza(string prefijo,string numero, string tablaGastosVarios)
		{
			ArrayList naturalezas=new ArrayList();
			DataSet ds=new DataSet();
			if(tablaGastosVarios == "DGASTOSVARIOSCLIENTE")
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT dgas_naturaleza FROM DGASTOSVARIOSCLIENTE WHERE pdoc_codigo='"+prefijo+"' AND mfac_numero="+numero+"");
			else if(tablaGastosVarios == "DGASTOSVARIOSPROVEEDOR")
				    DBFunctions.Request(ds,IncludeSchema.NO,"SELECT dgas_naturaleza FROM DGASTOSVARIOSPROVEEDOR WHERE pdoc_codiordepago='"+prefijo+"' AND mfac_numeordepago="+numero+"");
			if(ds.Tables.Count!=0)
			{
				if(ds.Tables[0].Rows.Count!=0)
				{
					for(int i=0;i<ds.Tables[0].Rows.Count;i++)
						naturalezas.Add(ds.Tables[0].Rows[i][0].ToString());
				}
			}
			return naturalezas;
		}

		/// <summary>
		/// RevisarDevoluciones : función que revisa las devoluciones hechas de un documento sobre las siguientes tablas
		/// mnotacliente, mnotaproveedor
		/// </summary>
		/// <param name="prefijo">string que trae el prefijo del documento</param>
		public void RevisarDevoluciones(string prefijo)
		{
			//Manejo de fechas 
		//	string fechaInicial="",fechaFinal="";
			fechaInicial+=anio.ToString()+"-";
			fechaFinal+=anio.ToString()+"-";
			if(mes>=1 && mes<=9)
				fechaInicial+="0"+mes.ToString()+"-";
			else
				fechaInicial+=mes.ToString()+"-";
			if(diaInicial>=1 && diaInicial<=9)
				fechaInicial+="0"+diaInicial;
			else
				fechaInicial+=diaInicial;

			if(mes>=1 && mes<=9)
				fechaFinal+="0"+mes.ToString()+"-";
			else
				fechaFinal+=mes.ToString()+"-";
			if(diaFinal>=1 && diaFinal<=9)
				fechaFinal+="0"+diaFinal;
			else
				fechaFinal+=diaFinal;
			//Fin manejo de fechas
			//Manejo cabeceras y selects
			DataSet dsCabeceras = new DataSet();
			string sqlReturnsDocRefe = "";
			string sqlReturnsDocRefeDetail = "";
			if(DBFunctions.RecordExist("SELECT MNC.pdoc_codigo, MNC.mnot_numero FROM mnotacliente MNC INNER JOIN mfacturacliente MFC ON MNC.mnot_prefdocu = MFC.pdoc_codigo AND MNC.mnot_numdocu = MFC.mfac_numedocu WHERE MNC.mnot_prefdocu='"+prefijo+"' AND MFC.mfac_factura BETWEEN '"+fechaInicial+"' AND '"+fechaFinal+"'"))
			{
				DBFunctions.Request(dsCabeceras,IncludeSchema.NO,"SELECT DISTINCT MNC.pdoc_codigo FROM mnotacliente MNC INNER JOIN mfacturacliente MFC ON MNC.mnot_prefdocu = MFC.pdoc_codigo AND MNC.mnot_numdocu = MFC.mfac_numedocu WHERE MNC.mnot_prefdocu='"+prefijo+"' AND MFC.mfac_factura BETWEEN '"+fechaInicial+"' AND '"+fechaFinal+"'");
				sqlReturnsDocRefe = "SELECT MNC.mnot_numero FROM mnotacliente MNC INNER JOIN mfacturacliente MFC ON MNC.mnot_prefdocu = MFC.pdoc_codigo AND MNC.mnot_numdocu = MFC.mfac_numedocu WHERE MNC.mnot_prefdocu='"+prefijo+"' AND MNC.pdoc_codigo='@' AND MFC.mfac_factura BETWEEN '"+fechaInicial+"' AND '"+fechaFinal+"'";
				sqlReturnsDocRefeDetail = "SELECT MNC.mnot_prefdocu, MNC.mnot_numdocu FROM mnotacliente MNC INNER JOIN mfacturacliente MFC ON MNC.mnot_prefdocu = MFC.pdoc_codigo AND MNC.mnot_numdocu = MFC.mfac_numedocu WHERE MNC.pdoc_codigo='@1' AND MNC.mnot_numero=@2";
			}
			for(int i=0;i<dsCabeceras.Tables[0].Rows.Count;i++)
				Armar_Cabecera(dsCabeceras.Tables[0].Rows[i][0].ToString(),sqlReturnsDocRefe.Replace("@",dsCabeceras.Tables[0].Rows[i][0].ToString()),sqlReturnsDocRefeDetail);
			//Fin Manejo cabeceras y selects
			//Manejo de detallers
			//Fin manejo de detalles
		}

		private bool BuscarValorArrayList(ArrayList lista, string valor)
		{
			for(int i=0;i<lista.Count;i++)
			{
				if(lista[i].ToString() == valor)
					return true;
			}
			return false;
		}

		private void CuadrarComprobanteAjusteAseguradoraTaller(ArrayList arrDetalleComprobante)
		{
			DataRow drIva = null;
			double valorDebito = 0, valorCredito = 0;
			for(int i=0;i<arrDetalleComprobante.Count;i++)
			{
				DataRow instancia = (DataRow)arrDetalleComprobante[i];
				double valorD = 0, valorC = 0;
				try{valorD = Convert.ToDouble(instancia["DEBITO"].ToString().Substring(1));}
				catch{}
				try{valorC = Convert.ToDouble(instancia["CREDITO"].ToString().Substring(1));}
				catch{}
				//if(i == arrDetalleComprobante.Count-1)
				if(valorC > 0 && drIva == null)
					drIva = instancia;
				valorDebito += Math.Round(valorD,2);
				valorCredito += Math.Round(valorC,2);
			}
			if(Math.Abs(valorDebito-valorCredito) > 0 && Math.Abs(valorDebito-valorCredito) <= 10)
			{
				try
				{
					double diferencia = Math.Round(valorDebito-valorCredito,2);
					double valorIVA = Convert.ToDouble(drIva["CREDITO"].ToString().Substring(1));
					valorIVA += diferencia;
					for(int i=0;i<tablaDetalles.Rows.Count;i++)
					{
						DataRow drIndex = tablaDetalles.Rows[i];
						if(drIndex[0].ToString() == drIva[0].ToString() && drIndex[1].ToString() == drIva[1].ToString() && drIndex[2].ToString() == drIva[2].ToString() && drIndex[3].ToString() == drIva[3].ToString() && drIndex[4].ToString() == drIva[4].ToString() && drIndex[5].ToString() == drIva[5].ToString() && drIndex[6].ToString() == drIva[6].ToString() && drIndex[7].ToString() == drIva[7].ToString() && drIndex[8].ToString() == drIva[8].ToString() && drIndex[9].ToString() == drIva[9].ToString() && drIndex[10].ToString() == drIva[10].ToString())
						{
							tablaDetalles.Rows[i]["CREDITO"] = valorIVA.ToString("C");
						}
					}
				}
				catch{}
			}
		}

		private void CuadrarComprobanteConsignacion(ArrayList arrDetalleComprobante)
		{
			DataRow drIva = null;
			double valorDebito = 0, valorCredito = 0;
			for(int i=0;i<arrDetalleComprobante.Count;i++)
			{
				DataRow instancia = (DataRow)arrDetalleComprobante[i];
				double valorD = 0, valorC = 0;
				try{valorD = Convert.ToDouble(instancia["DEBITO"].ToString().Substring(1));}
				catch{}
				try{valorC = Convert.ToDouble(instancia["CREDITO"].ToString().Substring(1));}
				catch{}
				if(instancia["CUENTA"].ToString() == "13551701") // esta condición aplica solo a un client. cual de todos?
					drIva = instancia;
				valorDebito += Math.Round(valorD,2);
				valorCredito += Math.Round(valorC,2);
			}
			if(Math.Abs(valorDebito-valorCredito) > 0 && Math.Abs(valorDebito-valorCredito) < 1 && drIva != null)
			{
				double valorIVA = 0;
				double diferencia = Math.Round(valorDebito-valorCredito,2);
				try{valorIVA = Convert.ToDouble(drIva["DEBITO"].ToString().Substring(1));}
				catch{return;}
				valorIVA -= diferencia;
				for(int i=0;i<tablaDetalles.Rows.Count;i++)
				{
					DataRow drIndex = tablaDetalles.Rows[i];
					if(drIndex[0].ToString() == drIva[0].ToString() && drIndex[1].ToString() == drIva[1].ToString() && drIndex[2].ToString() == drIva[2].ToString() && drIndex[3].ToString() == drIva[3].ToString() && drIndex[4].ToString() == drIva[4].ToString() && drIndex[5].ToString() == drIva[5].ToString() && drIndex[6].ToString() == drIva[6].ToString() && drIndex[7].ToString() == drIva[7].ToString() && drIndex[8].ToString() == drIva[8].ToString() && drIndex[9].ToString() == drIva[9].ToString() && drIndex[10].ToString() == drIva[10].ToString())
					{
						tablaDetalles.Rows[i]["DEBITO"] = valorIVA.ToString("C");
					}
				}
			}
		}

        public void MonitorearInterface()
        {
            
            // por ahora no se ejecuta
            return;
            
            //Hora en segundos para generacion automatica 11:30pm
            int segGeneracion = Convert.ToInt32(((23 * 60) + 30) * 60);
            int dia = DateTime.Now.Day;
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            int espera;
            while (true){
                espera = (segGeneracion - (((DateTime.Now.Hour * 60) + DateTime.Now.Minute) * 60));
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, new Exception("Esperando " + espera + " segundos."), "Generación Interface Contable Automática");
                if (espera >= 0)
                {
                    System.Threading.Thread.Sleep(espera * 1000); // esperar hasta as 11:30pm
                    try
                    {
                        AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, new Exception("Iniciando generación automática."), "Generación Interface Contable Automática");
                        InterfaceAutomatica(ano, mes, dia);
                        AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, new Exception("Concluida generación automática."), "Generación Interface Contable Automática");
                    }
                    catch (Exception ex)
                    {
                        AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, ex, "Generación Interface Contable Automática");
                    }
                }
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, new Exception("Esperando 6 horas."), "Generación Interface Contable Automática");
                System.Threading.Thread.Sleep((6 * 60 * 60) * 1000); // esperar 6 horas
            }
        }

        public void InterfaceAutomatica(int ano, int mes, int dia)
        {
            InterfaceContable miInterface = new InterfaceContable();
            ArrayList prefijos = new ArrayList();
            DataSet dsPrefijos = new DataSet();
            DBFunctions.Request(dsPrefijos, IncludeSchema.NO, "select distinct pdoc_codigo from dbxschema.ppuchechoscabecera;");
            for (int i = 0; i < dsPrefijos.Tables[0].Rows.Count; i++)
                prefijos.Add(dsPrefijos.Tables[0].Rows[i][0]);
            miInterface.TodosAlmacenes = true;
            miInterface.Anio = ano;
            miInterface.DiaInicial = dia;
            miInterface.DiaFinal = dia;
            miInterface.Mes = mes;
            miInterface.Usuario = "automatico";
            miInterface.Procesado = DateTime.Now.Date;
            miInterface.Prefijos = prefijos;
            //Hago un for para recorrer los distintos prefijos escogidos
            for (int i = 0; i < miInterface.Prefijos.Count; i++)
            {
                //Ahora por cada prefijo armo las distintas cabeceras de
                //los comprobantes
                miInterface.Armar_Cabecera(prefijos[i].ToString());
                //Si se produjo algún error al crear la cabecera lo muestro
                //en el label y rompo ese ciclo para no seguir generando
                //cabeceras
                if (miInterface.MensajesCabecera != string.Empty)
                    throw (new Exception(miInterface.MensajesCabecera));
                //Si no se produjo error al generar cabeceras, empiezo a recorrer dichas cabeceras
                //para sacar los distintos detalles que existan para cada cabecera
                for (int j = 0; j < miInterface.TablaCabecera.Rows.Count; j++)
                {
                    if (miInterface.TablaCabecera.Rows[j][0].ToString() == prefijos[i].ToString())
                        miInterface.Armar_Detalles(miInterface.TablaCabecera.Rows[j][0].ToString(), Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]), Convert.ToInt32(miInterface.TablaCabecera.Rows[j][1]), miInterface.TablaCabecera.Rows[j][9].ToString());
                    if (miInterface.erroresSql.Length > 0)
                        throw (new Exception(miInterface.MensajesCabecera));
                }

                string pref, nume;
                //Anulaciones Caja
                if (miInterface.AnulacionesCaja)
                {
                    int desdeC = miInterface.TablaCabecera.Rows.Count;
                    Hashtable prefijosA = miInterface.Armar_Cabecera_AnulacionesCaja(miInterface.TablaCabecera.Rows[i][0].ToString());
                    int hastaC = miInterface.TablaCabecera.Rows.Count;
                    for (int j = desdeC; j < hastaC; j++)
                    {
                        pref = miInterface.TablaCabecera.Rows[j][0].ToString();
                        nume = miInterface.TablaCabecera.Rows[j][1].ToString();
                        string[] tem = prefijosA[pref + "," + nume].ToString().Split(',');
                        miInterface.Armar_Detalles_Anulaciones(tem[0], Convert.ToInt32(tem[1]), pref, Convert.ToInt32(nume), miInterface.TablaCabecera.Rows[j][9].ToString());
                    }
                }
                //Anulaciones Tesoreria
                if (miInterface.AnulacionesTesoreria)
                {
                    int desdeC = miInterface.TablaCabecera.Rows.Count;
                    Hashtable prefijosA = miInterface.Armar_Cabecera_AnulacionesTesoreria(miInterface.TablaCabecera.Rows[i][0].ToString());
                    int hastaC = miInterface.TablaCabecera.Rows.Count;
                    for (int j = desdeC; j < hastaC; j++)
                    {
                        pref = miInterface.TablaCabecera.Rows[j][0].ToString();
                        nume = miInterface.TablaCabecera.Rows[j][1].ToString();
                        string[] tem = prefijosA[pref + "," + nume].ToString().Split(',');
                        miInterface.Armar_Detalles_Anulaciones(tem[0], Convert.ToInt32(tem[1]), pref, Convert.ToInt32(nume), miInterface.TablaCabecera.Rows[j][9].ToString());
                    }
                }
                if (!miInterface.GuardarInterface())
                    throw (new Exception(miInterface.Mensajes));
            }
        }

		#endregion
	}
}
