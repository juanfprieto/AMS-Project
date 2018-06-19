using System;
using System.Diagnostics;
using System.Data;
using System.Collections;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using AMS.Inventarios;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace AMS.Produccion
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class CosteoProduccion
	{
		//codigo de ensamble
		private string codigoEnsamble;
		//Codigo del item base
		private string codigoItem;
		//Valor unitario calculado del item base
		public double valorBase;
		//cantidad de items base
		private double cantidadItem;
		//Tipo de costeo
		private TipoCosteo tipoCosteo;
		//Valor CIF
		private double valorCIF;
		//Valor
		private double valorCargaPrestacional;
		//Horas laboradas
		private string horasMesLaboradas;
		//Costo por hora planta
		private double costoHoraPlanta;
		//Capacidad horas productivas por hora fisico
		private double capacidadPlanta;
		//Tempo total procesos
		private double tiempoTotal;
		//Total items usados
		private double cantidadTotalItems;
		//Costo maquinas
		public double totalMaquinas,totalManoObra,totalTrabajoTerceros,totalMateriaPrima,totalCIF;
		public string debug,error;
        public bool hacerCierre = false;
        public double cantidadOriginal = 0, cantidadActual = 0;
        public DataGrid dgCantidadSubItems = new DataGrid();
		
		//COnstructor
		public CosteoProduccion(string codItm, double cant, string codEns, TipoCosteo tpCosteo, double valCIF, string horasMes, double costPlnt, double capPlnt, double cargaPrest)
		{
			codigoItem=codItm;
			cantidadItem=cant;
			codigoEnsamble=codEns;
			tipoCosteo=tpCosteo;
			valorCIF=valCIF;
			horasMesLaboradas=horasMes;
			tiempoTotal=0;
			costoHoraPlanta=costPlnt;
			capacidadPlanta=capPlnt;
			valorCargaPrestacional=cargaPrest;
			totalMaquinas=0;
			totalManoObra=0;
			totalTrabajoTerceros=0;
			totalMateriaPrima=0;
			totalCIF=0;
			error="";
		}
		//Iniciar calculo
		public double Calcular(){
			double costo=0;
			debug="";
			cantidadTotalItems=0;
			//Sumar costos
			costo=SumarItem(codigoItem,cantidadItem,codigoEnsamble,1,"");
			debug=debug+"<BR><BR><BR> TOTAL ITEMS: "+costo+"<br>";
			//Calcular valor unidad item base
			this.valorBase=costo/cantidadItem;
			//Sumar costo planta
			costo+=(costoHoraPlanta/capacidadPlanta/3600)*tiempoTotal;
			totalMaquinas+=(costoHoraPlanta/capacidadPlanta/3600)*tiempoTotal;
			//this.valorBase+=(costoHoraPlanta/capacidadPlanta/3600)*tiempoTotal;
			debug=debug+" +Costo planta: "+costo+"<br>";
			//CIF
			costo=costo+(valorCIF*tiempoTotal);
			totalCIF=(valorCIF*tiempoTotal);
			//this.valorBase=this.valorBase+(valorCIF*tiempoTotal);
			debug=debug+" +CIF: <h1>"+costo+"</h1><br>";
			return costo;
		}

		//Algoritmo recursivo costeo
		private double SumarItem(string codItem,double cantidad, string ensamble, int contador, string itemsAnt){

			string costoP,costoO,costoM,tiempoO,itemsAnteriores,itmsEnsamble="";
			double costoPromedioM,costoPromedio,costoOperaciones,costoMaquinas,valorU,valorT,tiempoOperaciones,cantidadM;
			DataSet dsSubItems=new DataSet();
			
			//debug+=contador+": ["+codItem+", "+cantidad+"] ";
			//Limitar niveles a 50
			if(contador>50)return(0);
			//Evitar sumar el item varias veces
			itemsAnteriores=itemsAnt;
			//Evitar loop infinito
			if(itemsAnteriores.IndexOf("<"+codItem+"/>")>0)
				return(0);
			itemsAnteriores+="<"+codItem+"/>";

			cantidadTotalItems+=cantidad;

			//Consultar si el ensamble es propio
			string ensamblePropio=DBFunctions.SingleData(
				"SELECT PENS_PROPIO FROM PENSAMBLEPRODUCCION "+
				"WHERE PENS_CODIGO='"+ensamble+"';");
			debug+="<br><h2>"+codItem+"</h2>";
			//Sumar costo promedio hijos
			costoP=DBFunctions.SingleData(
				"SELECT SUM(ms.MSAL_COSTPROM * me.MENS_CANTIDAD) "+
                "FROM MSALDOITEM ms, MENSAMBLEPRODUCCIONITEMS me, CINVENTARIO CI "+
                "WHERE me.MITE_CODIGO=ms.MITE_CODIGO AND MS.PANO_ANO = CI.PANO_ANO AND "+
				"me.PENS_CODIGO='"+ensamble+"';");
			if(costoP.Length>0)
				costoPromedio=Convert.ToDouble(costoP);
			else costoPromedio=0;
			debug+="COSTO PROM. "+costoPromedio+"<br>";
					
			//Sumar operaciones ensamble
			costoO=DBFunctions.SingleData(
				"select "+
				//"sum(ROUND(((pt.PTEM_TIEMPOESTANDAR/3600) * pc.pcar_suelasig)/1000,0)) "+
				"sum(ROUND((pt.PTEM_TIEMPOESTANDAR * pc.pcar_suelasig)/240/3600,0)) "+
				"from MENSAMBLEPRODUCCIONOPERACIONES me, ptemparioproduccion pt, pcargoempleado pc "+
				"where me.pens_codigo='"+ensamble+"' and pc.pcar_codicargo=me.pcar_codicargo and "+
				"me.ptem_operacion=pt.ptem_operacion;");
			if(costoO.Length>0)
			{
				costoOperaciones=Convert.ToDouble(costoO)*(1+(valorCargaPrestacional/100));
			}
			else
				costoOperaciones=0;
			debug+="COSTO OPER. "+costoOperaciones+"<br>";
			totalManoObra+=(costoOperaciones*cantidad);
			
			//Tiempo total usado por Operaciones
			tiempoO=DBFunctions.SingleData(
				"select "+
				"sum(pt.PTEM_TIEMPOESTANDAR) "+
				"from MENSAMBLEPRODUCCIONOPERACIONES me, ptemparioproduccion pt, pcargoempleado pc "+
				"where me.pens_codigo='"+ensamble+"' and pc.pcar_codicargo=me.pcar_codicargo and "+
				"me.ptem_operacion=pt.ptem_operacion;");
			if(tiempoO.Length>0)
				tiempoOperaciones=Convert.ToDouble(tiempoO);
			else
				tiempoOperaciones=0;
			tiempoTotal+=tiempoOperaciones*cantidad;
			debug+="TEMP. OPER. "+tiempoOperaciones+"<br>";
			
			//Costo Maquinas
			costoM=DBFunctions.SingleData(
				"select SUM(ROUND((mmaq_costohora/3600) * PTEM_TIEMPOESTANDAR, 0)) "+
				"from MENSAMBLEPRODUCCIONOPERACIONES me, MMAQUINAS mm, ptemparioproduccion pt "+
				"where me.pens_codigo='"+ensamble+"' and "+
				"mm.MMAQ_CODIGO=me.MMAQ_CODIGO and me.ptem_operacion=pt.ptem_operacion;");
			if(costoM.Length>0)
				costoMaquinas=Convert.ToDouble(costoM);
			else
				costoMaquinas=0;
			debug+="COSTO MAQUINAS. "+costoMaquinas+"<br>";
			totalMaquinas+=(costoMaquinas*cantidad);
			
			//VAlor unidad
			valorU=(costoPromedio+costoOperaciones+costoMaquinas);

			//Valor cuando no tiene hijos
			//valorT=cantidad*valorU;
			valorT=0;

			//Valor cuando tiene hijos(no suma costo promedio)
			//valorTH=cantidad*(costoOperaciones+costoMaquinas);
			
			//Recorrer hijos si ensable es propio
			//bool hijos=false;
			//if(ensamblePropio=="S")
			{
				//Subitems
				DBFunctions.Request(dsSubItems, IncludeSchema.NO,
					"SELECT mp.MITE_CODIGO CODIGO, mp.MENS_CANTIDAD CANTIDAD "+
					"FROM MENSAMBLEPRODUCCIONITEMS mp, PENSAMBLEPRODUCCION pe "+
					"WHERE pe.PENS_CODIGO=mp.PENS_CODIGO AND pe.PENS_CODIGO='"+ensamble+"' and pe.pens_vigente='S';");
				
				//\\Recorrer SubItems
				debug+=" TOTAL ITEM: @"+codItem+"@<br>";
				debug+=" NIVEL: "+contador+"<br>";
				debug+=" CANTIDAD: "+cantidad+"<br>";
				debug+=" ITEMS:<br>#"+codItem+"#<br>";
				string ensambleA;
				for(int n=0;n<dsSubItems.Tables[0].Rows.Count;n++)
				{
					ensambleA=DBFunctions.SingleData(
						"SELECT PENS_CODIGO "+
						"FROM PENSAMBLEPRODUCCION "+
						"WHERE MITE_CODIGO='"+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+"' AND "+
						"PENS_VIGENTE='S';");
					if(ensambleA.Length>0)
					{
						//NO RECORRER SI ES TIPO E
						string tipoP=DBFunctions.SingleData(
							"SELECT TPRO_CODIGO "+
							"FROM MENSAMBLEPRODUCCIONITEMS "+
							"WHERE PENS_CODIGO='"+ensamble+"' AND "+
							"MITE_CODIGO='"+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+"'"+
							";");
						if(tipoP.Equals("E"))
						{
							//No recorrer si es tipo E
							costoPromedioM=0;
							cantidadM=Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]);
							try
							{
								costoPromedioM=Convert.ToDouble(
									DBFunctions.SingleData(
									"SELECT COALESCE(ms.MSAL_COSTPROM,0) "+
									"FROM MSALDOITEM ms, CINVENTARIO CI "+
                                    "WHERE MS.PANO_ANO = CI.PANO_ANO AND ms.MITE_CODIGO='" + dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString() + "';")
									);
								if(costoPromedioM<=0)throw(new Exception());
							}
							catch
							{
								error+="No se puede costear el item "+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+", no tiene costo promedio. ";
							}
							valorT+=cantidad*costoPromedioM*cantidadM;
							totalMateriaPrima+=cantidad*costoPromedioM*cantidadM;

							itmsEnsamble+="&nbsp;&nbsp;"+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+"&nbsp;&nbsp;&nbsp;"+dsSubItems.Tables[0].Rows[n]["CANTIDAD"]+"&nbsp;X&nbsp;"+cantidad+"   -> "+" "+cantidad+" X "+cantidadM+" X "+costoPromedioM+" = "+(cantidad*cantidadM*costoPromedioM).ToString()+"&nbsp;&nbsp;&nbsp;&nbsp;(Tipo E)<BR>";
						}
						else
						{
							//Sumar los hijos
							valorT+=
								SumarItem(dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString(),
								cantidad*Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]),
								ensambleA, contador+1, itemsAnteriores);
							//hijos=true;
						}
					}
					else 
					{
						cantidadM=Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]);
						cantidadTotalItems+=cantidad*cantidadM;
						costoPromedioM=0;
						try
						{
							costoPromedioM=Convert.ToDouble(
								DBFunctions.SingleData(
								"SELECT COALESCE(ms.MSAL_COSTPROM,0) "+
								"FROM MSALDOITEM ms, CINVENTARIO CI "+
                                "WHERE MS.PANO_ANO = CI.PANO_ANO AND ms.MITE_CODIGO='" + dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString() + "';")
								);
							if(costoPromedioM<=0)throw(new Exception());
						}
						catch
						{
							error+="No se puede costear el item "+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+", no tiene costo promedio. ";
						}

                        //Ajuste de costos de items en caso de hacer devoluciones de items
                        if (hacerCierre)
                        {
                            double cantSubItemOriginal = Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"].ToString()) * 100;
                            double cantidadDevolucion = 0;

                            if (dgCantidadSubItems.Items.Count > 0)
                                if (((TextBox)dgCantidadSubItems.Items[n].FindControl("txtCantidad")).Text != "")
                                    cantidadDevolucion = Convert.ToDouble(((TextBox)dgCantidadSubItems.Items[n].FindControl("txtCantidad")).Text); 

                            if (cantidadActual == cantidadOriginal) //Si se producen TODOS los items solicitados
                            {
                                if (cantidadDevolucion > 0) //Si hay Subitems devueltos
                                {
                                    double porcDescuento = cantidadDevolucion / cantSubItemOriginal;
                                    porcDescuento = 1 - porcDescuento;
                                    double costoReal = costoPromedioM * porcDescuento;
                                    valorT += (cantSubItemOriginal - cantidadDevolucion) * costoReal;
                                }
                                else //Si NO hay subitems devueltos. Ejecuta el proceso normal
                                {
                                    valorT += cantidad * cantidadM * costoPromedioM;
                                }
                            }
                            else  //Si se producen MENOS de los items solicitados
                            {
                                double devTeorica = ((cantidadOriginal - cantidadActual) * cantSubItemOriginal) / cantidadOriginal;
                                double sobrante = devTeorica - cantidadDevolucion; // (-) se disminuye coste, (+) aumenta costo
                                double factor = sobrante / (cantSubItemOriginal - cantidadDevolucion);
                                factor = 1 + factor;
                                double costoReal = costoPromedioM * factor;
                                valorT += (cantSubItemOriginal - cantidadDevolucion) * costoReal;
                            }
                        }
                        else
                        {
                            //Ejecuta el proceso normal
                            valorT += cantidad * cantidadM * costoPromedioM;
                        }

						if(DBFunctions.SingleData(
							"SELECT TORI_CODIGO FROM MITEMS "+
							"WHERE MITE_CODIGO='"+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+"';").Equals("X"))
							totalTrabajoTerceros+=cantidad*cantidadM*costoPromedioM;
						else
							totalMateriaPrima+=cantidad*cantidadM*costoPromedioM;
						itmsEnsamble+="&nbsp;&nbsp;"+dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString()+"&nbsp;&nbsp;&nbsp;"+dsSubItems.Tables[0].Rows[n]["CANTIDAD"]+"&nbsp;X&nbsp;"+cantidad+"   -> "+" "+cantidad+" X "+cantidadM+" X "+costoPromedioM+" = "+(cantidad*cantidadM*costoPromedioM).ToString()+"<BR>";
					}
				}
			}

			//if(hijos)valorT=valorTH;
			//else 
			valorT+=cantidad*(costoOperaciones+costoMaquinas);
			debug=debug.Replace("@"+codItem+"@",valorT.ToString());
			debug=debug.Replace("#"+codItem+"#",itmsEnsamble);
			return(valorT);
		}
		//Consultar factor CIF Planta en segundos: (Debitos-Creditos)/horas_laboradas_mes_anterior/3600
		public static double TraerCIF(string planta,int ano, int mes)
		{
			double cif,horasP;
			string cifS=DBFunctions.SingleData(
				"SELECT SUM(DC.DCUE_VALODEBI-DC.DCUE_VALOCRED) "+
				"FROM MCOMPROBANTE MC, DCUENTA DC, MCIF MF "+
				"WHERE DC.PDOC_CODIGO=MC.PDOC_CODIGO AND DC.MCOM_NUMEDOCU=MC.MCOM_NUMEDOCU AND "+
				"MF.MPLA_CODIGO='"+planta+"' AND DC.MCUE_CODIPUC = MF.MCUE_CODIPUC AND DC.PCEN_CODIGO=MF.PCEN_CODIGO AND "+
				"MC.PANO_ANO="+ano+" AND MC.PMES_MES="+mes+";");
			if(cifS.Length==0)return(0);
			else cif=Convert.ToDouble(cifS);
			//Horas laboradas mes anterior
			horasP=Convert.ToDouble(DBFunctions.SingleData(
				"SELECT MPLA_TOTAHORAMESANTE FROM MPLANTAS WHERE MPLA_CODIGO='"+planta+"';"));
			if(horasP==0)
				return(0);
			return(cif/horasP/3600);
		}
	}
	public enum TipoCosteo{Ensamble,Produccion};

}
