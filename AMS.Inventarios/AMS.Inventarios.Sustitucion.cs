
using System;
using System.Web.UI;
using System.Data;
using System.Collections;
using AMS.DB;
using AMS.Forms;


namespace AMS.Inventarios
{
    public class Sustitucion // : System.Web.UI.UserControl
	{
		public string prefijoSustitucion, processMsg, fecha, usuario;
		private uint numeroSustitucion;
		private DataTable dtSustitucion;  
		
		public string Fecha{set{fecha = value;}get{return fecha;}}
		public string Usuario{set{usuario = value;}get{return usuario;}}
		public string ProcessMsg{get{return processMsg;}}
		
		
		public Sustitucion(string prefSus, uint numSus, DataTable dtInt)
		{
			prefijoSustitucion = prefSus;
			numeroSustitucion = numSus;
			dtSustitucion = new DataTable();
			dtSustitucion = dtInt;
		}
      

		public string Analizar_Campo(string campo,string tipo)
		{
			string valor="";
			if(tipo=="System.Decimal" || tipo=="System.Int16" || tipo=="System.Int32" || tipo=="System.Int64" || tipo=="System.Double" || tipo=="System.Single")
				valor=campo;
			else if(tipo=="System.DateTime" || tipo=="System.TimeSpan" || tipo=="System.String")
				valor="'"+campo+"'";
			else if(tipo=="System.DBNull")
				valor="null";
			return valor;
		}

		public bool CrearSustitucion()
		{
			//Revisamos si existe una sustitucion con este prefijo y este numero
			while(DBFunctions.RecordExist("SELECT * FROM msustitucion WHERE pdoc_codigo='"+prefijoSustitucion+"' AND msus_numero="+numeroSustitucion+""))
				numeroSustitucion += 1;
			bool status = true;
			ArrayList sqlStrings = new ArrayList();
			//Entonces recorremos el DataTable y vamos agregando los que se han configurado
			//De acuerdo al tipo de sustitucion a realizar
			for(int i=0;i<dtSustitucion.Rows.Count;i++)
			{
				//Si es tipo sustitucion anterior (A)
				if(dtSustitucion.Rows[i][0].ToString() == "A")
					sqlStrings.Add("INSERT INTO msustitucion VALUES('"+prefijoSustitucion+"',"+numeroSustitucion+",'"+dtSustitucion.Rows[i][2].ToString()+"','"+fecha+"','"+dtSustitucion.Rows[i][4].ToString()+"','"+dtSustitucion.Rows[i][2].ToString()+"','"+dtSustitucion.Rows[i][2].ToString()+"','"+(dtSustitucion.Rows[i][5].ToString().Split('-'))[0]+"','A','"+usuario+"')");
					//Si es tipo sustitucion posterior (P)
				else if(dtSustitucion.Rows[i][0].ToString() == "P")
				{
					//Traemos la informacion del codigo origen y creamos un registro en mitems
					DataSet ds = new DataSet();
					DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM mitems WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					bool fecha1=DatasToControls.ValidarDateTime(ds.Tables[0].Rows[0][23].ToString());
					//Si existe la fecha
					if(fecha1)
						sqlStrings.Add("INSERT INTO mitems VALUES('"+dtSustitucion.Rows[i][4].ToString()+"','"+ds.Tables[0].Rows[0][1].ToString()+"','"+ds.Tables[0].Rows[0][2].ToString()+"',"+
							""+Analizar_Campo(ds.Tables[0].Rows[0][3].ToString(),ds.Tables[0].Rows[0][3].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][4].ToString(),ds.Tables[0].Rows[0][4].GetType().ToString())+",'"+ds.Tables[0].Rows[0][5].ToString()+"',"+
							"'"+ds.Tables[0].Rows[0][6].ToString()+"',"+Analizar_Campo(ds.Tables[0].Rows[0][7].ToString(),ds.Tables[0].Rows[0][7].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][8].ToString(),ds.Tables[0].Rows[0][8].GetType().ToString())+","+
							"'"+ds.Tables[0].Rows[0][9].ToString()+"',"+ds.Tables[0].Rows[0][10].ToString()+",'"+ds.Tables[0].Rows[0][11].ToString()+"',"+
							""+Analizar_Campo(ds.Tables[0].Rows[0][12].ToString(),ds.Tables[0].Rows[0][12].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][13].ToString(),ds.Tables[0].Rows[0][13].GetType().ToString())+",'"+ds.Tables[0].Rows[0][14].ToString()+"',"+
							""+ds.Tables[0].Rows[0][15].ToString()+","+Analizar_Campo(ds.Tables[0].Rows[0][16].ToString(),ds.Tables[0].Rows[0][16].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][17].ToString(),ds.Tables[0].Rows[0][17].GetType().ToString())+","+
							"'"+ds.Tables[0].Rows[0][18].ToString()+"','"+ds.Tables[0].Rows[0][19].ToString()+"',"+Analizar_Campo(ds.Tables[0].Rows[0][20].ToString(),ds.Tables[0].Rows[0][20].GetType().ToString())+","+
							""+ds.Tables[0].Rows[0][21].ToString()+","+Analizar_Campo(ds.Tables[0].Rows[0][22].ToString(),ds.Tables[0].Rows[0][22].GetType().ToString())+",'"+Convert.ToDateTime(ds.Tables[0].Rows[0][23]).ToString("yyyy-MM-dd")+"',"+
							"'"+(Convert.ToDateTime(ds.Tables[0].Rows[0][24])).ToString("yyyy-MM-dd")+"',"+ds.Tables[0].Rows[0][25].ToString()+","+Analizar_Campo(ds.Tables[0].Rows[0][26].ToString(),ds.Tables[0].Rows[0][26].GetType().ToString())+","+
							""+Analizar_Campo(ds.Tables[0].Rows[0][27].ToString(),ds.Tables[0].Rows[0][27].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][28].ToString(),ds.Tables[0].Rows[0][28].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][29].ToString(),ds.Tables[0].Rows[0][29].GetType().ToString())+","+
							"'"+ds.Tables[0].Rows[0][30].ToString()+"','"+ds.Tables[0].Rows[0][31].ToString()+"',"+Analizar_Campo(ds.Tables[0].Rows[0][32].ToString(),ds.Tables[0].Rows[0][32].GetType().ToString())+","+
							""+Analizar_Campo(ds.Tables[0].Rows[0][33].ToString(),ds.Tables[0].Rows[0][33].GetType().ToString())+",'"+ds.Tables[0].Rows[0][34].ToString()+"','"+ds.Tables[0].Rows[0][35].ToString()+"')");
					else
						sqlStrings.Add("INSERT INTO mitems VALUES('"+dtSustitucion.Rows[i][4].ToString()+"','"+ds.Tables[0].Rows[0][1].ToString()+"','"+ds.Tables[0].Rows[0][2].ToString()+"',"+
							""+Analizar_Campo(ds.Tables[0].Rows[0][3].ToString(),ds.Tables[0].Rows[0][3].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][4].ToString(),ds.Tables[0].Rows[0][4].GetType().ToString())+",'"+ds.Tables[0].Rows[0][5].ToString()+"',"+
							"'"+ds.Tables[0].Rows[0][6].ToString()+"',"+Analizar_Campo(ds.Tables[0].Rows[0][7].ToString(),ds.Tables[0].Rows[0][7].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][8].ToString(),ds.Tables[0].Rows[0][8].GetType().ToString())+","+
							"'"+ds.Tables[0].Rows[0][9].ToString()+"',"+ds.Tables[0].Rows[0][10].ToString()+",'"+ds.Tables[0].Rows[0][11].ToString()+"',"+
							""+Analizar_Campo(ds.Tables[0].Rows[0][12].ToString(),ds.Tables[0].Rows[0][12].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][13].ToString(),ds.Tables[0].Rows[0][13].GetType().ToString())+",'"+ds.Tables[0].Rows[0][14].ToString()+"',"+
							""+ds.Tables[0].Rows[0][15].ToString()+","+Analizar_Campo(ds.Tables[0].Rows[0][16].ToString(),ds.Tables[0].Rows[0][16].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][17].ToString(),ds.Tables[0].Rows[0][17].GetType().ToString())+","+
							"'"+ds.Tables[0].Rows[0][18].ToString()+"','"+ds.Tables[0].Rows[0][19].ToString()+"',"+Analizar_Campo(ds.Tables[0].Rows[0][20].ToString(),ds.Tables[0].Rows[0][20].GetType().ToString())+","+
							""+ds.Tables[0].Rows[0][21].ToString()+","+Analizar_Campo(ds.Tables[0].Rows[0][22].ToString(),ds.Tables[0].Rows[0][22].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][23].ToString(),ds.Tables[0].Rows[0][23].GetType().ToString())+","+
							"'"+(Convert.ToDateTime(ds.Tables[0].Rows[0][24])).ToString("yyyy-MM-dd")+"',"+ds.Tables[0].Rows[0][25].ToString()+","+Analizar_Campo(ds.Tables[0].Rows[0][26].ToString(),ds.Tables[0].Rows[0][26].GetType().ToString())+","+
							""+Analizar_Campo(ds.Tables[0].Rows[0][27].ToString(),ds.Tables[0].Rows[0][27].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][28].ToString(),ds.Tables[0].Rows[0][28].GetType().ToString())+","+Analizar_Campo(ds.Tables[0].Rows[0][29].ToString(),ds.Tables[0].Rows[0][29].GetType().ToString())+","+
							"'"+ds.Tables[0].Rows[0][30].ToString()+"','"+ds.Tables[0].Rows[0][31].ToString()+"',"+Analizar_Campo(ds.Tables[0].Rows[0][32].ToString(),ds.Tables[0].Rows[0][32].GetType().ToString())+","+
							""+Analizar_Campo(ds.Tables[0].Rows[0][33].ToString(),ds.Tables[0].Rows[0][33].GetType().ToString())+",'"+ds.Tables[0].Rows[0][34].ToString()+"','"+ds.Tables[0].Rows[0][35].ToString()+"')");
					//Ahora realizamos la actualizacion de todas las tablas relacionadas con mitems
					DataSet hijas=new DataSet();
					DBFunctions.Request(hijas,IncludeSchema.NO,"SELECT DISTINCT tbname,fkcolnames FROM sysibm.sysrels WHERE reftbname = 'MITEMS'");
					for(int cont=0;cont<hijas.Tables[0].Rows.Count;cont++)
						sqlStrings.Add("UPDATE "+hijas.Tables[0].Rows[cont][0].ToString()+" SET "+hijas.Tables[0].Rows[cont][1].ToString()+"='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE "+hijas.Tables[0].Rows[cont][1].ToString()+"='"+dtSustitucion.Rows[i][2].ToString()+"'");
					/*sqlStrings.Add("UPDATE ditems SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE dlistaempaque SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE dordenitemcotizacion SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE dpedidoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE macumuladoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE macumuladoitemalmacen SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mdemandaitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mdemandaitemalmacen SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mfaltanteentradaitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mitemoperacion SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mitemsgrupo SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mkititem SET mkit_coditem='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mkit_coditem='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mprecioitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE msaldoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE msaldoitemalmacen SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE msugeridoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					sqlStrings.Add("UPDATE mubicacionitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");*/
					//Actualizamos las sustituciones cuyo codigo actual sea el viejo por el nuevo
					sqlStrings.Add("UPDATE msustitucion SET msus_codactual='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE msus_codactual='"+dtSustitucion.Rows[i][2].ToString()+"'");
					//Ahora Agregamos el nuevo registro de msustitucionitem
					sqlStrings.Add("INSERT INTO msustitucion VALUES('"+prefijoSustitucion+"',"+numeroSustitucion+",'"+dtSustitucion.Rows[i][2].ToString()+"','"+fecha+"','"+dtSustitucion.Rows[i][2].ToString()+"','"+dtSustitucion.Rows[i][4].ToString()+"','"+dtSustitucion.Rows[i][4].ToString()+"','"+(dtSustitucion.Rows[i][5].ToString().Split('-'))[0]+"','P','"+usuario+"')");
					//Ahora eliminamos el registro de mitems correspondiente a el codigo de origen
                    sqlStrings.Add("COMMIT"); // para que registre las transacciones y pueda eliminar el registro original
                    sqlStrings.Add("DELETE FROM mitems WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
				}
				else if(dtSustitucion.Rows[i][0].ToString() == "U")
				{
					int j;
					DataSet da = new DataSet();
					//Realizamos las actualizaciones de las tablas existen tablas que necesitan ser actualizadas y otras necesitan ser recalculadas
				
					#region ditems necesita solo actualización
					sqlStrings.Add("UPDATE ditems SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					#endregion

                    #region dinventariofisico necesita solo actualización
                    sqlStrings.Add("UPDATE dinventariofisico SET dinv_mite_codigo='" + dtSustitucion.Rows[i][4].ToString() + "' WHERE dinv_mite_codigo='" + dtSustitucion.Rows[i][2].ToString() + "'");
                    #endregion
					
					#region dlistaempaque necesita realizar sumas de cantidades debemos traer las listas de empaque donde se encuentre el codigo de item de origen
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT mlis_numero, dped_cantasig, pped_codigo, mped_numepedi FROM dlistaempaque WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de esta lista de empaque con el codigo del item de susutitucion
						if(DBFunctions.RecordExist("SELECT * FROM dlistaempaque WHERE mlis_numero="+da.Tables[0].Rows[j][0].ToString()+" AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
						{
							//Si existe actualizamos la cantidad del codigo de sustitucion y eliminamos el registro del codigo original
							sqlStrings.Add("UPDATE dlistaempaque SET dped_cantasig = dped_cantasig + "+da.Tables[0].Rows[j][1].ToString()+" WHERE mlis_numero="+da.Tables[0].Rows[j][0].ToString()+" AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'");
							sqlStrings.Add("DELETE FROM dlistaempaque WHERE mlis_numero="+da.Tables[0].Rows[j][0].ToString()+" AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
						}
						else//Si no existe simplemente actualizamos el registro de dlistaempaque del codigo de origen y lista empaque
							sqlStrings.Add("UPDATE dlistaempaque SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mlis_numero="+da.Tables[0].Rows[j][0].ToString()+" AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					}
					#endregion
				
					#region dordenitemcotizacion necesita realizar sumas de las cantidades necesarias
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pdoc_codigo, mord_numecoti, dcoit_cantidad FROM dordenitemcotizacion WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de esta cotizacion con el codigo del item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM dordenitemcotizacion WHERE pdoc_codigo='"+da.Tables[0].Rows[j][0].ToString()+"' AND mord_numecoti="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
						{
							//Si existe actualizamos la cantidad del codigo de sustitucion y eliminamos el registro del codigo original
							sqlStrings.Add("UPDATE dordenitemcotizacion SET dcoit_cantidad = dcoit_cantidad + "+da.Tables[0].Rows[j][2].ToString()+" WHERE pdoc_codigo='"+da.Tables[0].Rows[j][0].ToString()+"' AND mord_numecoti="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'");
							sqlStrings.Add("DELETE FROM dordenitemcotizacion WHERE pdoc_codigo='"+da.Tables[0].Rows[j][0].ToString()+"' AND mord_numecoti="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
						}
						else//Si no existe simplemente actualizamos el registro de dordenitemcotizaciondel codigo original al sustituido
							sqlStrings.Add("UPDATE dordenitemcotizacion SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE pdoc_codigo='"+da.Tables[0].Rows[j][0].ToString()+"' AND mord_numecoti="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					}
					#endregion
				
					#region dpedidoitem necesita realizar las sumas de las cantidades pedidas, asignadas y facturadas, y recalcular el valor FOB
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT mped_clasregi, mnit_nit, pped_codigo, mped_numepedi, dped_cantpedi, dped_cantasig, dped_cantfact, dped_valounit FROM dpedidoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					//													0				1			2			3			4				5				6				7			
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de esta pedido con el codigo del item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE mped_clasregi='"+da.Tables[0].Rows[j][0].ToString()+"' AND mnit_nit='"+da.Tables[0].Rows[j][1].ToString()+"' AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
						{
							//Si existe actualizamos las cantidades pedida, asignada y facturada y recalculamos el valor FOB 
							double valorFOB = 0;
							double cantidadPedida2 = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi FROM dpedidoitem WHERE mped_clasregi='"+da.Tables[0].Rows[j][0].ToString()+"' AND mnit_nit='"+da.Tables[0].Rows[j][1].ToString()+"' AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"));
							double valor2 = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_valounit FROM dpedidoitem WHERE mped_clasregi='"+da.Tables[0].Rows[j][0].ToString()+"' AND mnit_nit='"+da.Tables[0].Rows[j][1].ToString()+"' AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"));
							try
							{
								if((Convert.ToDouble(da.Tables[0].Rows[j][4]) + cantidadPedida2) > 0)
									valorFOB = ( (Convert.ToDouble(da.Tables[0].Rows[j][4].ToString()) * (Convert.ToDouble(da.Tables[0].Rows[j][7].ToString())) + ( cantidadPedida2 * valor2) ))/ (Convert.ToDouble(da.Tables[0].Rows[j][4].ToString()) + cantidadPedida2) ;
								else
									valorFOB = 0;
							}
							catch{valorFOB = 0;}
							sqlStrings.Add("UPDATE dpedidoitem SET dped_cantpedi = dped_cantpedi + "+da.Tables[0].Rows[j][4].ToString()+", dped_cantasig = dped_cantasig + "+da.Tables[0].Rows[j][5].ToString()+", dped_cantfact = dped_cantfact + "+da.Tables[0].Rows[j][6].ToString()+", dped_valounit="+valorFOB.ToString()+" WHERE mped_clasregi='"+da.Tables[0].Rows[j][0].ToString()+"' AND mnit_nit='"+da.Tables[0].Rows[j][1].ToString()+"' AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'");
							sqlStrings.Add("DELETE FROM dpedidoitem WHERE mped_clasregi='"+da.Tables[0].Rows[j][0].ToString()+"' AND mnit_nit='"+da.Tables[0].Rows[j][1].ToString()+"' AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
						}
						else//Si no existe simplemente actualizamos el registro de dpedidoitem del codigo original al sustituido
							sqlStrings.Add("UPDATE dpedidoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mped_clasregi='"+da.Tables[0].Rows[j][0].ToString()+"' AND mnit_nit='"+da.Tables[0].Rows[j][1].ToString()+"' AND pped_codigo='"+da.Tables[0].Rows[j][2].ToString()+"' AND mped_numepedi="+da.Tables[0].Rows[j][3].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					}
					#endregion
				
					#region macumuladoitem solo se realiza la suma de las cantidades
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT tmov_tipomovi, pano_ano, pmes_mes, macu_cantidad FROM macumuladoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de este acumulado con el item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM macumuladoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+""))
						{
							//Si existe actualizamos las cantidades 
							sqlStrings.Add("UPDATE macumuladoitem SET macu_cantidad = macu_cantidad + "+da.Tables[0].Rows[j][3].ToString()+" WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+"");
							sqlStrings.Add("DELETE FROM macumuladoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+"");
						}
						else//Si no existe simplemente actualizamos el registro de macumuladoitem del codigo original al sustituido
							sqlStrings.Add("UPDATE macumuladoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+"");
					}
					#endregion
	
					#region macumuladoitemalmacen solo se realiza la suma de las cantidades
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT tmov_tipomovi, pano_ano, pmes_mes, palm_almacen, macu_cantidad FROM macumuladoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de este acumulado con el codigo del item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM macumuladoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][3].ToString()+"'"))
						{
							//Si existe actualizamos las cantidades 
							sqlStrings.Add("UPDATE macumuladoitemalmacen SET macu_cantidad = macu_cantidad + "+da.Tables[0].Rows[j][4].ToString()+" WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][3].ToString()+"'");
							sqlStrings.Add("DELETE FROM macumuladoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][3].ToString()+"'");
						}
						else//Si no existe simplemente actualizamos el registro de macumuladoitemalmacen del codigo original al sustituido
							sqlStrings.Add("UPDATE macumuladoitemalmacen SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND tmov_tipomovi="+da.Tables[0].Rows[j][0].ToString()+" AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][2].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][3].ToString()+"'");
					}
					#endregion
					
					#region mdemandaitem solo se realiza la suma de las cantidades
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pano_ano, pmes_mes, mdem_cantidad FROM mdemandaitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de esta demanda con el codigo del item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM mdemandaitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+""))
						{
							//Si existe actualizamos las cantidades 
							sqlStrings.Add("UPDATE mdemandaitem SET mdem_cantidad = mdem_cantidad + "+da.Tables[0].Rows[j][2].ToString()+" WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+"");
							sqlStrings.Add("DELETE FROM mdemandaitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+"");
						}
						else//Si no existe simplemente actualizamos el registro de mdemandaitem del codigo original al sustituido
							sqlStrings.Add("UPDATE mdemandaitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+"");
					}
					#endregion
					
					#region mdemandaitemalmacen solo se realiza la suma de las cantidades
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pano_ano, pmes_mes, palm_almacen, mdem_cantidad FROM mdemandaitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de esta demanda con el codigo del item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM mdemandaitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][2].ToString()+"'"))
						{
							//Si existe actualizamos las cantidades 
							sqlStrings.Add("UPDATE mdemandaitemalmacen SET mdem_cantidad = mdem_cantidad + "+da.Tables[0].Rows[j][3].ToString()+" WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][2].ToString()+"'");
							sqlStrings.Add("DELETE FROM mdemandaitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][2].ToString()+"'");
						}
						else//Si no existe simplemente actualizamos el registro de mdemandaitemalmacen del codigo original al sustituido
							sqlStrings.Add("UPDATE mdemandaitemalmacen SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+" AND pmes_mes="+da.Tables[0].Rows[j][1].ToString()+" AND palm_almacen='"+da.Tables[0].Rows[j][2].ToString()+"'");
					}
					#endregion
					
					#region mfaltanteentradaitem solo se realiza la suma de las cantidades
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pdoc_codiordepago, mfac_numeordepago, mfal_diferencia FROM mfaltanteentradaitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						//Ahora revisamos si existe un registro de este faltante con el codigo del item de sustitucion
						if(DBFunctions.RecordExist("SELECT * FROM mfaltanteentradaitem WHERE pdoc_codiordepago='"+da.Tables[0].Rows[j][0].ToString()+"' AND mfac_numeordepago="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
						{
							//Si existe actualizamos las cantidades 
							sqlStrings.Add("UPDATE mfaltanteentradaitem SET mfal_diferencia = mfal_diferencia + "+da.Tables[0].Rows[j][2].ToString()+" WHERE pdoc_codiordepago='"+da.Tables[0].Rows[j][0].ToString()+"' AND mfac_numeordepago="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'");
							sqlStrings.Add("DELETE FROM mfaltanteentradaitem WHERE pdoc_codiordepago='"+da.Tables[0].Rows[j][0].ToString()+"' AND mfac_numeordepago="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
						}
						else//Si no existe simplemente actualizamos el registro de mdemandaitemalmacen del codigo original al sustituido
							sqlStrings.Add("UPDATE mfaltanteentradaitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE pdoc_codiordepago='"+da.Tables[0].Rows[j][0].ToString()+"' AND mfac_numeordepago="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					}
					#endregion
					
					#region mitemoperacion debemos revisar cuales registros se deben actualizar
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT ptem_operacion FROM mitemoperacion WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM mitemoperacion WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
							sqlStrings.Add("DELETE FROM mitemoperacion WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND ptem_operacion='"+da.Tables[0].Rows[j][0].ToString()+"'");
						else
							sqlStrings.Add("UPDATE mitemoperacion SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND ptem_operacion='"+da.Tables[0].Rows[j][0].ToString()+"'");
					}
					#endregion
					
					#region mitemsgrupo debemos revisar cuales registros se deben actualizar
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pgru_grupo FROM mitemsgrupo WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM mitemsgrupo WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pgru_grupo='"+da.Tables[0].Rows[j][0].ToString()+"'"))
							sqlStrings.Add("DELETE FROM mitemsgrupo WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pgru_grupo='"+da.Tables[0].Rows[j][0].ToString()+"'");
						else
							sqlStrings.Add("UPDATE mitemsgrupo SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pgru_grupo='"+da.Tables[0].Rows[j][0].ToString()+"'");
					}
					#endregion
					
					#region mkititem debemos revisar cuales registros se deben actualizar
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT mkit_codikit FROM mkititem WHERE mkit_coditem='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM mkititem WHERE mkit_codikit='"+da.Tables[0].Rows[j][0].ToString()+"' AND mkit_coditem='"+dtSustitucion.Rows[i][4].ToString()+"'"))
							sqlStrings.Add("DELETE FROM mkititem WHERE mkit_codikit='"+da.Tables[0].Rows[j][0].ToString()+"' AND mkit_coditem='"+dtSustitucion.Rows[i][2].ToString()+"'");
						else
							sqlStrings.Add("UPDATE mkititem SET mkit_coditem='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mkit_codikit='"+da.Tables[0].Rows[j][0].ToString()+"' AND mkit_coditem='"+dtSustitucion.Rows[i][2].ToString()+"'");
					}
					#endregion
					
					#region mprecioitem debemos revisar cuales registros se deben actualizar
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT ppre_codigo FROM mprecioitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM mprecioitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND ppre_codigo='"+da.Tables[0].Rows[j][0].ToString()+"'"))
							sqlStrings.Add("DELETE FROM mprecioitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND ppre_codigo='"+da.Tables[0].Rows[j][0].ToString()+"'");
						else
							sqlStrings.Add("UPDATE mprecioitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND ppre_codigo='"+da.Tables[0].Rows[j][0].ToString()+"'");
					}
					#endregion
					
					#region msaldoitem debemos sumar las cantidades actual, asignadas la cantidad de pedidos y unidades en transito, y cantidades de pedidos y unidades pendientes
					//Ademas debo recalcular los 4 costos que tengo 
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pano_ano, msal_cantactual, msal_cantasig, msal_costprom, msal_costpromhist, msal_costhist, msal_costhisthist, msal_cantinveinic, msal_peditrans, msal_unidtrans, msal_pedipendi, msal_unidpendi FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					//													0				1				2				3				4				5			6				7						8				9			10				11				
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+""))
						{
							double costProm = 0, costPromHis = 0, costHist = 0, costHist2 = 0;
							double cantActual2 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+""));
							double costProm2 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+""));
							double costPromHis2 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+""));
							double costHist12 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costhist FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+""));
							double costHist22 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costhisthist FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+""));
							
							try
							{
								if((Convert.ToDouble(da.Tables[0].Rows[j][1]) + cantActual2) > 0)
									costProm = ((Convert.ToDouble(da.Tables[0].Rows[j][1]) * Convert.ToDouble(da.Tables[0].Rows[j][3])) + (cantActual2 * costProm2)) / ((Convert.ToDouble(da.Tables[0].Rows[j][1])) + (cantActual2));
								else
									costProm = 0;
							}
							catch
							{
								costProm = 0;
							}
							
							try
							{
								if((Convert.ToDouble(da.Tables[0].Rows[j][1]) + cantActual2) > 0)
									costPromHis = ((Convert.ToDouble(da.Tables[0].Rows[j][1]) * Convert.ToDouble(da.Tables[0].Rows[j][4])) + (cantActual2 * costPromHis2)) / ((Convert.ToDouble(da.Tables[0].Rows[j][1])) + (cantActual2));
								else
									costPromHis = 0;
							}
							catch
							{
								costPromHis = 0;
							}
							
							try
							{
								if((Convert.ToDouble(da.Tables[0].Rows[j][1]) + cantActual2) > 0)
									costHist = ((Convert.ToDouble(da.Tables[0].Rows[j][1]) * Convert.ToDouble(da.Tables[0].Rows[j][5])) + (cantActual2 * costHist12)) / ((Convert.ToDouble(da.Tables[0].Rows[j][1])) + (cantActual2));
								else
									costHist = 0;
							}
							catch
							{
								costHist = 0;
							}
							
							try
							{
								if((Convert.ToDouble(da.Tables[0].Rows[j][1]) + cantActual2) > 0)
									costHist2 = ((Convert.ToDouble(da.Tables[0].Rows[j][1]) * Convert.ToDouble(da.Tables[0].Rows[j][6])) + (cantActual2 * costHist22) ) / ((Convert.ToDouble(da.Tables[0].Rows[j][1])) + (cantActual2));
								else
									costHist2 = 0;
							}
							catch
							{
								costHist2 = 0;
							}
							
							sqlStrings.Add("UPDATE msaldoitem SET msal_cantactual = msal_cantactual + "+da.Tables[0].Rows[j][1].ToString()+" , msal_cantasig = msal_cantasig + "+da.Tables[0].Rows[j][2].ToString()+" , msal_costprom = "+costProm+", msal_costpromhist = "+costPromHis+", msal_costhist = "+costHist+", msal_costhisthist = "+costHist2+", msal_cantinveinic = msal_cantinveinic + "+da.Tables[0].Rows[j][7].ToString()+", msal_peditrans = msal_peditrans + "+da.Tables[0].Rows[j][8].ToString()+", msal_unidtrans = msal_unidtrans + "+da.Tables[0].Rows[j][9].ToString()+", msal_pedipendi = msal_pedipendi + "+da.Tables[0].Rows[j][10].ToString()+", msal_unidpendi = msal_unidpendi + "+da.Tables[0].Rows[j][11].ToString()+" WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+"");
							sqlStrings.Add("DELETE FROM msaldoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+"");
						}
						else
							sqlStrings.Add("UPDATE msaldoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][0].ToString()+"");
					}
					#endregion
					
					#region msaldoitemalmacen se suman las catntidades asignadas, actual, pendientes, transito, inventario inicial, y se recalculan el costo promedio y el costo promedio historico
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT palm_almacen, pano_ano, msal_cantasig, COALESCE(msal_cantactual,0), COALESCE(msal_costprom,0), COALESCE(msal_costpromhist,0), COALESCE(msal_cantpendiente,0), COALESCE(msal_canttransito,0), COALESCE(msal_cantinveinic,0) FROM msaldoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					//													0				1		2						3					4										5					6								7							8			
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM msaldoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+""))
						{
							double costProm = 0, costPromHist = 0;
							double cantActual2 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantasig FROM msaldoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+""));
							double costProm2 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+""));
							double costPromHist2 = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+""));
							try
							{
								if((cantActual2 + Convert.ToDouble(da.Tables[0].Rows[j][3])) > 0)
									costProm = ( (Convert.ToDouble(da.Tables[0].Rows[j][3]) * Convert.ToDouble(da.Tables[0].Rows[j][4])) + (cantActual2 * costProm2) )/(cantActual2 + Convert.ToDouble(da.Tables[0].Rows[j][3]));
								else
									costProm = 0;
							}
							catch
							{
								costProm = 0;
							}
							
							try
							{
								if((cantActual2 + Convert.ToDouble(da.Tables[0].Rows[j][3])) > 0)
									costPromHist = ( (Convert.ToDouble(da.Tables[0].Rows[j][3]) * Convert.ToDouble(da.Tables[0].Rows[j][5])) + (cantActual2 * costPromHist2) )/(cantActual2 + Convert.ToDouble(da.Tables[0].Rows[j][3]));
								else
									costPromHist = 0;
							}
							catch
							{
								costPromHist = 0;
							}
							
							sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig = msal_cantasig + "+da.Tables[0].Rows[j][2].ToString()+", msal_cantactual = msal_cantactual + "+da.Tables[0].Rows[j][3].ToString()+", msal_costprom = "+costProm+", msal_costpromhist = "+costPromHist+", msal_cantpendiente = msal_cantpendiente + "+da.Tables[0].Rows[j][6].ToString()+", msal_canttransito = msal_canttransito + "+da.Tables[0].Rows[j][7].ToString()+", msal_cantinveinic = msal_cantinveinic + "+da.Tables[0].Rows[j][8].ToString()+" WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+"");
							sqlStrings.Add("DELETE FROM msaldoitemalmacen WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+"");
						}
						else
							sqlStrings.Add("UPDATE msaldoitemalmacen SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND palm_almacen='"+da.Tables[0].Rows[j][0].ToString()+"' AND pano_ano="+da.Tables[0].Rows[j][1].ToString()+"");
					}
					#endregion
					
					#region msugeridoitem debemos revisar cuales registros se deben actualizar
					if(DBFunctions.RecordExist("SELECT * FROM msugeridoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
						sqlStrings.Add("DELETE FROM msugeridoitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					else
						sqlStrings.Add("UPDATE msugeridoitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					#endregion
					
					#region mubicacionitem debemos revisar cuales registros se deben actualizar
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT pubi_codigo FROM mubicacionitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM mubicacionitem WHERE pubi_codigo="+da.Tables[0].Rows[j][0].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
							sqlStrings.Add("DELETE FROM mubicacionitem WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pubi_codigo="+da.Tables[0].Rows[j][0].ToString());
						else
							sqlStrings.Add("UPDATE mubicacionitem SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pubi_codigo="+da.Tables[0].Rows[j][0].ToString());
					}
					#endregion
					
					#region mitemscatalogo si hay del nuevo, se deja quieto y se borra el viejo, cuando no, actualizo el viejo al nuevo
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT DISTINCT pcat_codigo FROM mitemscatalogo WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					for(j=0;j<da.Tables[0].Rows.Count;j++)
					{
						if(DBFunctions.RecordExist("SELECT * FROM mitemscatalogo WHERE mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' AND pcat_codigo='"+da.Tables[0].Rows[j][0].ToString()+"'"))
							sqlStrings.Add("DELETE FROM mitemscatalogo WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pcat_codigo='"+da.Tables[0].Rows[j][0].ToString()+"'");
						else
							sqlStrings.Add("UPDATE mitemscatalogo SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"' AND pcat_codigo='"+da.Tables[0].Rows[j][0].ToString()+"'");
					}
					#endregion

					#region dprogramaproduccion necesita realizar sumas de cantidades debemos traer los programas de produccion donde se encuentre el codigo de item de origen
					da = new DataSet();
					DBFunctions.Request(da,IncludeSchema.NO,"SELECT dprog_consecutivo, mprog_consecutivo, dprog_cantidad, dprog_total FROM dprogramaproduccion WHERE mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
					if(da.Tables.Count>0)
						for(j=0;j<da.Tables[0].Rows.Count;j++)
						{
							//Ahora revisamos si existe un registro de esta lista de dprogramaproduccion con el codigo del item de susutitucion
							if(DBFunctions.RecordExist("SELECT * FROM dprogramaproduccion WHERE dprog_consecutivo="+da.Tables[0].Rows[j][0].ToString()+" AND mprog_consecutivo="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'"))
							{
								//Si existe actualizamos la cantidad del codigo de sustitucion y eliminamos el registro del codigo original
								sqlStrings.Add("UPDATE dprogramaproduccion SET dprog_cantidad = dprog_cantidad + "+da.Tables[0].Rows[j][2].ToString()+", dprog_total = dprog_total + "+da.Tables[0].Rows[j][3].ToString()+" WHERE dprog_consecutivo="+da.Tables[0].Rows[j][0].ToString()+" AND mprog_consecutivo='"+da.Tables[0].Rows[j][1].ToString()+"' AND mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"'");
								sqlStrings.Add("DELETE FROM dprogramaproduccion WHERE dprog_consecutivo="+da.Tables[0].Rows[j][0].ToString()+" AND mprog_consecutivo="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
							}
							else//Si no existe simplemente actualizamos el registro de dprogramaproduccion del codigo de origen y lista empaque
								sqlStrings.Add("UPDATE dprogramaproduccion SET mite_codigo='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE dprog_consecutivo="+da.Tables[0].Rows[j][0].ToString()+" AND mprog_consecutivo="+da.Tables[0].Rows[j][1].ToString()+" AND mite_codigo='"+dtSustitucion.Rows[i][2].ToString()+"'");
						}
					#endregion
			
					
					//Actualizamos las sustituciones cuyo codigo actual sea el viejo por el nuevo
					sqlStrings.Add("UPDATE msustitucion SET msus_codactual='"+dtSustitucion.Rows[i][4].ToString()+"' WHERE msus_codactual='"+dtSustitucion.Rows[i][2].ToString()+"'");
					//Ahora Agregamos el nuevo registro de msustitucionitem
					sqlStrings.Add("INSERT INTO msustitucion VALUES('"+prefijoSustitucion+"',"+numeroSustitucion+",'"+dtSustitucion.Rows[i][2].ToString()+"','"+fecha+"','"+dtSustitucion.Rows[i][2].ToString()+"','"+dtSustitucion.Rows[i][4].ToString()+"','"+dtSustitucion.Rows[i][4].ToString()+"','"+(dtSustitucion.Rows[i][5].ToString().Split('-'))[0]+"','U','"+usuario+"')");
					//Ahora eliminamos el registro de mitems correspondiente a el codigo de origen
					sqlStrings.Add("COMMIT"); // para que registre las transacciones y pueda eliminar el registro original
                    sqlStrings.Add("DELETE FROM mitems WHERE mite_codigo='" + dtSustitucion.Rows[i][2].ToString() + "'");
                }
			}
			//Realizamos la actualizacion del documento de sustitución
			if(numeroSustitucion > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu FROM pdocumento WHERE pdoc_codigo='"+prefijoSustitucion+"'")))
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu= "+numeroSustitucion+" WHERE pdoc_codigo='"+prefijoSustitucion+"'");
			
			if(DBFunctions.Transaction(sqlStrings))
            {
				this.processMsg += "<br>Bien "+DBFunctions.exceptions;
           //   Response.Write("<script language='javascript'>alert('La sustitucion ha sido realizada Satisfactoriamente !!! ');</script>");
            }
			else
			{
				status = false;
				this.processMsg += "<br>ERROR : "+DBFunctions.exceptions;
			}
			
			return status;
		}
	}
}
