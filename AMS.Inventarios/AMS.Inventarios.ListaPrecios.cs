using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using AMS.Documentos;
using AMS.DB;
using AMS.Tools;
using System.Configuration;
using AMS.CriptoServiceProvider;
using IBM.Data.DB2;

namespace AMS.Inventarios
{
	public class ListaPrecios
	{
		private string codigoLista, nombreLista, monedaLista, processMsg;
		private DataTable dtItemsPrecios;
		public string CodigoLista{set{codigoLista = value;}get{return codigoLista;}}
		public string NombreLista{set{nombreLista = value;}get{return nombreLista;}}
		public string MonedaLista{set{monedaLista = value;}get{return monedaLista;}}
		public string ProcessMsg{get{return processMsg;}}
        public DataSet ds;

		public ListaPrecios(string codLista, string nomLista, string monLista)
		{
			this.codigoLista = codLista;
			this.nombreLista = nomLista;
			this.monedaLista = monLista;
			this.processMsg = "";
		}
		
		public ListaPrecios(string codLista)
		{
			this.codigoLista = codLista;
			this.nombreLista = DBFunctions.SingleData("SELECT ppre_nombre FROM pprecioitem WHERE ppre_codigo='"+this.codigoLista+"'");
			this.monedaLista = DBFunctions.SingleData("SELECT pmon_moneda FROM pprecioitem WHERE ppre_codigo='"+this.codigoLista+"'");
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mite_codigo AS CODIGOITEM, mpre_precio AS PRECIO FROM mprecioitem WHERE ppre_codigo='"+this.codigoLista+"'");
			this.dtItemsPrecios = ds.Tables[0].Copy();
		}
		
		public bool CrearLista()
		{
			bool status = true;
			//Revisamos si ya existe una lista de precios con este codigo 
			if(DBFunctions.RecordExist("SELECT * FROM pprecioitem WHERE ppre_codigo='"+this.codigoLista+"'"))
			{
				status = false;
				this.processMsg = "Ya existe una lista de precios con este código";
			}
			else
			{
				int cantidadInsercion = DBFunctions.NonQuery("INSERT INTO pprecioitem values('"+this.codigoLista+"','"+this.nombreLista+"','"+this.monedaLista+"')");
				if(cantidadInsercion == 0)
				{
					status = false;
					this.processMsg = "<br>Error : "+DBFunctions.exceptions;
				}
			}
			return status;
		}
		
		public DataTable ActualizacionValores(bool grupoActualizar, DataTable dtFiltros, string valorBase, string tipoOperacion, string valorOperado, ref bool status)
		{
			//Revisamos primero que grupo de Items se van a actualizar, a ver si son todos o un grupo especifo de repuestos
			//cuando es false se van actualizar todos los items y cuando es true se va ha actualizar un grupo especifico es decir se deben seleccionar algunos items nada mas
			//Todos los items deben tener un precio dentro de una lista de precios
			//Si un item no tiene definido el valor base se le tomara como base para la actualizacion el valor que tiene en mitems
			status = true;
			DataTable dtOut = new DataTable();
			this.PrepararDtOut(ref dtOut);
			ArrayList sqlStrings = new ArrayList();
			string consultaPrecio = "SELECT "+this.ConstruirFormula(valorBase,tipoOperacion,valorOperado)+" FROM "+(valorBase.Split('-'))[0]+" WHERE ";
			string consultaItems = "";
			if((valorBase.Split('-'))[2] == "0")
				consultaPrecio += "PANO_ANO = "+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+" AND ";
			else if((valorBase.Split('-'))[2] == "2")
				consultaPrecio += "PPRE_CODIGO = "+this.codigoLista+" AND ";
			if(!grupoActualizar)
				consultaItems = "SELECT MIT.mite_codigo, MIT.mite_nombre , MIT.mite_costrepo, MIT.plin_codigo, DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) FROM mitems MIT, plineaitem PLIN WHERE MIT.plin_codigo = PLIN.plin_codigo";
				//								0				1					2					3	      		4
			else
				consultaItems = this.ConstruirConsultaAvanzada(dtFiltros);
			//this.processMsg += consultaItems;
			//Como se van a actualizar todos los items debemos traer un listado de todos los items que tenemos en nuestro catalogo
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,consultaItems);
			/*this.processMsg += "<br>"+consultaItems;
			this.processMsg += "<br>"+ds.Tables[0].Columns.Count;*/
			
            //rutina for para actualizar dato por dato
            for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = dtOut.NewRow();				
				fila[0] = ds.Tables[0].Rows[i][4].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				//Calculamos el nuevo precio
				double nuevoPrecio = 0;
				try  
                {   nuevoPrecio = Convert.ToDouble(DBFunctions.SingleData(consultaPrecio+"MITE_CODIGO = '"+ds.Tables[0].Rows[i][0].ToString()+"'"));
                    fila[3]=Convert.ToDouble(DBFunctions.SingleData("SELECT "+(valorBase.Split('-'))[1]+" FROM "+(valorBase.Split('-'))[0]+" "+consultaPrecio.Substring(consultaPrecio.IndexOf("WHERE"))+"MITE_CODIGO='"+ds.Tables[0].Rows[i][0].ToString()+"'")).ToString("C");
                }
				catch
                {   nuevoPrecio = this.CalcularValor(Convert.ToDouble(ds.Tables[0].Rows[i][2]),tipoOperacion,Convert.ToDouble(valorOperado));
                    fila[3]=Convert.ToDouble(ds.Tables[0].Rows[i][2]).ToString("C");
                }
				//Primero vamos a realizar el trabajo en la base de datos y luego nos encargamos del datatable de salida como reporte
				//Primero revisamos si existe un registro dentro de mprecioitem lo actualizamos, sino lo agregamos
				if((this.dtItemsPrecios.Select("CODIGOITEM='"+ds.Tables[0].Rows[i][0].ToString()+"'")).Length > 0)
				{
					sqlStrings.Add("UPDATE mprecioitem SET mpre_precio="+nuevoPrecio.ToString()+" WHERE mite_codigo='"+ds.Tables[0].Rows[i][0].ToString()+"' AND ppre_codigo='"+this.codigoLista+"'");
					fila[2] = Convert.ToDouble((this.dtItemsPrecios.Select("CODIGOITEM='"+ds.Tables[0].Rows[i][0].ToString()+"'"))[0][1]).ToString("C");
				}
				else
				{
					sqlStrings.Add("INSERT INTO mprecioitem VALUES('"+ds.Tables[0].Rows[i][0].ToString()+"','"+this.codigoLista+"',"+nuevoPrecio.ToString()+")");
					fila[2] = "No definido en esta lista anteriormente";
				}
				//Ahora agregamos la fila para mostrar la informacion al usuario de los cambios
				fila[4] = nuevoPrecio.ToString("C");
				dtOut.Rows.Add(fila);
			}


			if(DBFunctions.Transaction(sqlStrings))
				this.processMsg += "<br>Bien "+DBFunctions.exceptions;
			else
			{
				status = false;
				this.processMsg += "<br>ERROR : "+DBFunctions.exceptions;
			}
			return dtOut;
		}
		
		public bool EliminarLista()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = true;
			//Primero eliminamos los registros de la tabla de mprecioitem
			sqlStrings.Add("DELETE FROM mprecioitem WHERE ppre_codigo='"+this.codigoLista+"'");
			//Y Ahora eliminamos el registro de pprecioitem
			sqlStrings.Add("DELETE FROM pprecioitem WHERE ppre_codigo='"+this.codigoLista+"'");
			if(DBFunctions.Transaction(sqlStrings))
				this.processMsg += "<br>Bien "+DBFunctions.exceptions;
			else
			{
				status = false;
				this.processMsg += "<br>ERROR : "+DBFunctions.exceptions;
			}
			return status;
		}

		//Cargamos el Excel y recorremos para dividir cada registro en 3 tablas (los que se insertan(INSERT), los que se actualizan(UPDATE), los que no existen o tienen algo mal
		public string ActualizarListaArchivo(DataTable dtArchivo, ref DataTable dtActualizacion, ref DataTable dtNuevos, ref DataTable dtInconveniente, bool accion)
		{
            bool existeItem = false;
            string status = "";
			ArrayList sqlStrings = new ArrayList();

			dtActualizacion = AplicarFormatoArchivo(dtActualizacion);
			dtNuevos = AplicarFormatoNew(dtNuevos);
			dtInconveniente = AplicarFormatoError(dtInconveniente);

            //en este dataset se cargan todos los items de precios
            DataSet dsItems = new DataSet();
            DBFunctions.Request(dsItems, IncludeSchema.NO, @"SELECT MITE_CODIGO FROM DBXSCHEMA.mitems WHERE TVIG_TIPOVIGE = 'V';
                                                            SELECT DISTINCT MITE_CODIGO FROM DBXSCHEMA.MPRECIOITEM WHERE PPRE_CODIGO = '" + this.codigoLista + "'; SELECT plin_tIPO, PLIN_CODIGO FROM DBXSCHEMA.plineaitem");

            string lineaAnter = "", linea = "";
            for (int i = 0; i < dtArchivo.Rows.Count; i++)
            { 
                string verificacion = "";
                string codItAlma = "";
                if (lineaAnter != dtArchivo.Rows[i][1].ToString().Trim())
                {
                    linea = DBFunctions.SingleData("select PLIN_CODIGO ||'~'|| PLIN_TIPO FROM PLINEAITEM WHERE PLIN_CODIGO = '" + dtArchivo.Rows[i][1].ToString().Trim() + "' ");
                    lineaAnter = dtArchivo.Rows[i][1].ToString().Trim();
                }
                string[] tipoLin = linea.Split('~');
                string refMazda = "";
                if (tipoLin[1]=="MZ")
                {
                    if (dtArchivo.Rows[i][0].ToString().Substring(4, 1) != "-" && dtArchivo.Rows[i][0].ToString().Substring(7, 1) != "-") // crear referencia tipo MAZDA
                    {
                        refMazda = dtArchivo.Rows[i][0].ToString().Substring(0, 4) + "-" + dtArchivo.Rows[i][0].ToString().Substring(4, 2) + "-" + dtArchivo.Rows[i][0].ToString().Substring(6, 3).Trim() ;
                        if (dtArchivo.Rows[i][0].ToString().Length == 10)
                            refMazda = refMazda + "-" + dtArchivo.Rows[i][0].ToString().Substring(09, 1);
                        if (dtArchivo.Rows[i][0].ToString().Length == 12)
                            refMazda = refMazda + dtArchivo.Rows[i][0].ToString().Substring(09, 1) +"-" + dtArchivo.Rows[i][0].ToString().Substring(10, 2);
                        if (dtArchivo.Rows[i][0].ToString().Length == 11)
                            refMazda = refMazda + "-" + dtArchivo.Rows[i][0].ToString().Substring(09, 2);
                    }
                    else
                        refMazda = dtArchivo.Rows[i][0].ToString(); // ya viene con los giones separadores
                    dtArchivo.Rows[i][0] = refMazda;
                }

                if(dsItems.Tables[2].Select("PLIN_CODIGO = '" + dtArchivo.Rows[i][1].ToString().Trim() + "'").Length == 0)
                {
                    return "Se generó un problema al cargar los datos, ya que la linea: " + dtArchivo.Rows[i][1].ToString().Trim() + " No existe en la base de datos. \\nSi es un número(ej: 02) \\nNo olvide en el archivo excel cambiar el formato de la columna de linea a TEXTO";
                    
                }
                bool verificaCodIt = Referencias.Guardar(dtArchivo.Rows[i][0].ToString().Replace('"', ' ').Trim(), ref codItAlma, dsItems.Tables[2].Select("PLIN_CODIGO = '" + dtArchivo.Rows[i][1].ToString().Trim() + "'")[0].ItemArray[0].ToString());//DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtArchivo.Rows[i][1].ToString().Trim() + "'"));
                if (!verificaCodIt)
                    verificacion = "Error con el código. No está en el formato de la línea de bodega especificada.";
                else
                {
                    if(dsItems.Tables[2].Select("PLIN_CODIGO = '" + dtArchivo.Rows[i][1].ToString() + "'").Length == 0)
                        verificacion += "No existe la linea de item ingresada " + dtArchivo.Rows[i] + "<br>";

                }
                //verificacion = this.VerificarFilaArchivo(dtArchivo.Rows[i]);


                //Primero revisamos si este item existe dentro de la tabla de mitems
                if (dsItems.Tables[0].Select("MITE_CODIGO = '" + codItAlma + "'").Length > 0 )//DBFunctions.RecordExist("SELECT COALESCE(PDES_PORCDESC,0) FROM mitems M, PDESCUENTOITEM PD WHERE mite_codigo='" + codItAlma + "' AND M.PDES_CODIGO = PD.PDES_CODIGO;"))
                    existeItem = true;
                else
                    existeItem = false;
                if (existeItem)
                {
                    if (verificacion == "")
                    {
                        //Ahora nos aseguramos de que exista en mprecioitem y después asignamos los registros en las respectivas Datatables
                        if ( dsItems.Tables[1].Select("MITE_CODIGO = '" + codItAlma +"'").Length > 0)//DBFunctions.RecordExist("SELECT MITE_CODIGO FROM MPRECIOITEM WHERE MITE_CODIGO = '" + codItAlma + "' AND PPRE_CODIGO = '" + this.codigoLista + "'"))
                        {
                            DataRow filaExistente = dtActualizacion.NewRow();
                            filaExistente[0] = dtArchivo.Rows[i][0].ToString().Replace('"', ' ').Trim();
                            filaExistente[1] = codItAlma;

                            filaExistente[2] = dtArchivo.Rows[i][1].ToString().Trim();//this.codigoLista;
                            filaExistente[3] = dtArchivo.Rows[i][2];

                            dtActualizacion.Rows.Add(filaExistente);

                        }
                        else
                        {
                            DataRow fila = dtNuevos.NewRow();

                            fila[0] = dtArchivo.Rows[i][0].ToString().Replace('"', ' ').Trim();
                            //fila[1] = codItAlma;
                            fila[1] = dtArchivo.Rows[i][1].ToString().Trim();
                            fila[2] = dtArchivo.Rows[i][2];
                            dtNuevos.Rows.Add(fila);
                        }
                    }
                    else
                    {
                        DataRow fila = dtInconveniente.NewRow();

                        fila[0] = dtArchivo.Rows[i][0].ToString().Replace('"', ' ').Trim(); //codItAlma;
                        fila[1] = dtArchivo.Rows[i][1].ToString().Trim();
                        fila[2] = verificacion;
                        fila[3] = "Antiguo";

                        dtInconveniente.Rows.Add(fila);
                    }
                }
                #region Codigo candidato a eliminar
                //Es porque el item es nuevo
                /*else
                {
                    if(verificacion == "")
                    {
                        DataRow fila = dtNuevos.NewRow();
                        fila[0] = dtArchivo.Rows[i][0].ToString().Replace('"',' ').Trim();
                        fila[1] = dtArchivo.Rows[i][1].ToString().Trim();
                        //fila[2] = dtArchivo.Rows[i][2].ToString().Trim();
                        fila[2] = "";
                        fila[3] = dtArchivo.Rows[i][3].ToString().Trim();
                        fila[4] = "";
                        //fila[4] = dtArchivo.Rows[i][4].ToString().Trim();
                        fila[5] ="";
                        //fila[5] = Convert.ToDouble(dtArchivo.Rows[i][5]);
                        fila[6] ="";
                        //fila[6] = Convert.ToDouble(dtArchivo.Rows[i][6]);
                        //fila[7] = dtArchivo.Rows[i][7].ToString().Trim();
                        fila[7] ="";
                        fila[8] = Convert.ToDouble(dtArchivo.Rows[i][8]);
                        fila[9] = Convert.ToDouble(dtArchivo.Rows[i][9]);
                        fila[10] = 0;
                        dtNuevos.Rows.Add(fila);
                        //Ahora Actualizamos las tablas de mitems y de mprecioitem
                        //if(dtArchivo.Rows[i][2].ToString().Trim() == "")
                        //	sqlStrings.Add("INSERT INTO mitems VALUES('"+codItAlma+"','"+dtArchivo.Rows[i][1].ToString().Trim()+"','"+dtArchivo.Rows[i][3].ToString().Trim()+"','NO','1','1','"+dtArchivo.Rows[i][4].ToString().Trim()+"',null,null,'LT',0,'C3',0,0,'"+dtArchivo.Rows[i][7].ToString().Trim()+"',"+Convert.ToDouble(dtArchivo.Rows[i][6]).ToString()+",null,null,'V','A',"+Convert.ToDouble(dtArchivo.Rows[i][5]).ToString()+",0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',"+Convert.ToDouble(dtArchivo.Rows[i][8]).ToString()+","+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+","+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+","+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+",null,'S')");
                        //else
                        //{
                        //	sqlStrings.Add("INSERT INTO mitems VALUES('"+codItAlma+"','"+dtArchivo.Rows[i][1].ToString().Trim()+"','"+dtArchivo.Rows[i][3].ToString().Trim()+"','NO','1','1','"+dtArchivo.Rows[i][4].ToString().Trim()+"',null,null,'LT',0,'C3',0,0,'"+dtArchivo.Rows[i][7].ToString().Trim()+"',"+Convert.ToDouble(dtArchivo.Rows[i][6]).ToString()+",null,null,'V','A',"+Convert.ToDouble(dtArchivo.Rows[i][5]).ToString()+",0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',"+Convert.ToDouble(dtArchivo.Rows[i][8]).ToString()+","+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+","+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+","+DBFunctions.SingleData("SELECT pano_ano FROM cinventario")+",null,'N')");
                        //	sqlStrings.Add("INSERT INTO mitemsgrupo VALUES('"+codItAlma+"','"+dtArchivo.Rows[i][2].ToString().Trim()+"',null)");
                        //}
                        //Ahora creamos el registro dentro de la tabla de mprecioitem
                        //sqlStrings.Add("INSERT INTO mprecioitem VALUES('"+codItAlma+"','"+this.codigoLista+"',"+Convert.ToDouble(dtArchivo.Rows[i][9]).ToString()+")");
                    }
                    */
                #endregion
                else
                {
                    DataRow fila = dtInconveniente.NewRow();

                    fila[0] = dtArchivo.Rows[i][0].ToString().Replace('"', ' ').Trim();
                    fila[1] = dtArchivo.Rows[i][1].ToString().Trim();
                    fila[2] = verificacion + "Item no existe en maestro.";
                    fila[3] = "Nuevo";

                    dtInconveniente.Rows.Add(fila);
                }

            }
            //timer.Stop();
            //long duration = timer.ElapsedMilliseconds;
            //double duracion = Convert.ToDouble(timer.ElapsedMilliseconds)/1000.0;
            return status;
		}

        public string ActualizarPrecios(DataTable dtActualizacion, DataTable dtNuevos, bool accion)
        {
            string rta = "";

            //Lamentablemente cuando se habla de actualizar datos no sirve bulkcopy, ya que este sólo inserta
            if (dtActualizacion.Rows.Count > 0)
            {
                ArrayList sqlString = new ArrayList();
                foreach(DataRow row in dtActualizacion.Rows)
                {
                    sqlString.Add("UPDATE DBXSCHEMA.MPRECIOITEM SET MPRE_PRECIO = '" + row[3] + "' WHERE MITE_CODIGO = '" + row[1] +"' AND PPRE_CODIGO = '" + this.codigoLista +"'");
                }
                
                if(DBFunctions.Transaction(sqlString))
                {
                    rta = "OK";
                }
                else
                {
                    rta = "falló ACTUALIZACIÓN de datos: " + DBFunctions.exceptions + "<br />";
                }
            }
            //Items nuevos en la tabla mprecioitem
            if(dtNuevos.Rows.Count > 0)
            {
                //dtNuevos.Columns.Remove("MITE_CODIGO");
                //dtNuevos.Columns[0].ColumnName = "MITE_CODIGO";
                //dtNuevos.AcceptChanges();
                DataTable lasLineas = DBFunctions.Request(new DataSet(), IncludeSchema.NO, "SELECT PLIN_TIPO, PLIN_CODIGO FROM DBXSCHEMA.PLINEAITEM").Tables[0];
                for (int i = 0; i < dtNuevos.Rows.Count; i++)
                {
                    string codItAlma = "";
                    try
                    {
                        Referencias.Guardar(dtNuevos.Rows[i][0].ToString().Replace('"', ' ').Trim(), ref codItAlma, lasLineas.Select("PLIN_CODIGO = '" + dtNuevos.Rows[i][1].ToString().Trim() + "'")[0].ItemArray[0].ToString());//DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtNuevos.Rows[i][1].ToString().Trim() + "'"));

                    }catch
                    {
                        Referencias.Guardar(dtNuevos.Rows[i][0].ToString().Replace('"', ' ').Trim(), ref codItAlma, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtNuevos.Rows[i][1].ToString().Trim() + "'"));
                    }
                    dtNuevos.Rows[i][0] = codItAlma;
                    dtNuevos.Rows[i][1] = codigoLista;
                }
                

                if (rta.Length < 3)
                {
                    string servidor = ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
                    string database = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
                    string usuario = ConfigurationManager.AppSettings["UID"];
                    string password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];

                    string timeout = ConfigurationManager.AppSettings["ConnectionTimeout"];
                    string port = ConfigurationManager.AppSettings["DataBasePort"];
                    AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
                    miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
                    miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
                    string newPwd = miCripto.DescifrarCadena(password);
                    string connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd + "";

                    //IBM.Data.DB2.DB2BulkCopy dbBulkcopy = new IBM.Data.DB2.DB2BulkCopy(connectionString, IBM.Data.DB2.DB2BulkCopyOptions.KeepIdentity);
                    using (var dbBulkcopy = new IBM.Data.DB2.DB2BulkCopy(connectionString, IBM.Data.DB2.DB2BulkCopyOptions.Default))
                    {
                        // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                        foreach (DataColumn col in dtNuevos.Columns)
                        {
                            dbBulkcopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }
                        dbBulkcopy.BulkCopyTimeout = 600;
                        dbBulkcopy.DestinationTableName = "MPRECIOITEM";

                        try
                        {
                            dbBulkcopy.WriteToServer(dtNuevos);
                            dbBulkcopy.Close();
                            DBFunctions.closeConnection();
                            rta = "OK";
                        }
                        catch (Exception z)
                        {
                            dbBulkcopy.Close();
                            rta = "<br />" + "falló INSERCIÓN de datos: " + z.Message;
                            DBFunctions.closeConnection();
                        }
                    }
                }
            }
            return rta;
        }

        private string VerificarFilaArchivo(DataRow fila)
		{
			string error = "";

			if(!DBFunctions.RecordExist("SELECT * FROM plineaitem WHERE plin_codigo='" + fila[1].ToString() + "'"))
				error += "No existe la linea de item ingresada " + fila[1].ToString() + "<br>";
			
			#region Código candidato a eliminar
			/*
			if(fila[2].ToString()!="")
			{
				if(!DBFunctions.RecordExist("SELECT * FROM pgrupocatalogo WHERE pgru_grupo='"+fila[2].ToString()+"'"))
					error += "No existe el grupo de catalogo ingresado "+fila[2].ToString()+"<br>";
			}
			if(!DBFunctions.RecordExist("SELECT * FROM pfamiliaitem WHERE pfam_codigo='"+fila[4].ToString()+"'"))
				error += "No existe la familia de item ingresada "+fila[4].ToString()+"<br>";
			if(!DBFunctions.RecordExist("SELECT * FROM piva WHERE piva_porciva="+Convert.ToDouble(fila[6]).ToString()+""))
				error += "No existe el porcentaje de iva ingresado "+fila[6].ToString()+"<br>";
			if(!DBFunctions.RecordExist("SELECT * FROM pdescuentoitem WHERE pdes_codigo='"+fila[7].ToString()+"'"))
				error += "No existe el codigo de descuento ingresado "+fila[7].ToString()+"<br>";
			*/
			#endregion

			return error;
		}
		
		private DataTable AplicarFormatoArchivo(DataTable dgIn)
		{
			dgIn = new DataTable();

			dgIn.Columns.Add(new DataColumn("MITE_CODIGO", System.Type.GetType("System.String")));//0
            dgIn.Columns.Add(new DataColumn("MITE_REF", System.Type.GetType("System.String")));//0
            dgIn.Columns.Add(new DataColumn("PPRE_CODIGO", System.Type.GetType("System.String")));//3
			dgIn.Columns.Add(new DataColumn("MPRE_PRECIO", System.Type.GetType("System.Double")));//9
			
			#region Código candidato a eliminar
			/*
			dgIn.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			dgIn.Columns.Add(new DataColumn("GRUPO",System.Type.GetType("System.String")));//2
			dgIn.Columns.Add(new DataColumn("FAMILIA",System.Type.GetType("System.String")));//4
			dgIn.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//5
			dgIn.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));//6
			dgIn.Columns.Add(new DataColumn("DESCUENTO",System.Type.GetType("System.String")));//7
			dgIn.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.Double")));//8
			dgIn.Columns.Add(new DataColumn("VALORANTERIOR",System.Type.GetType("System.Double")));//10
			*/
			#endregion

			return dgIn;
		}

        private DataTable AplicarFormatoNew(DataTable dgIn)
        {
            dgIn = new DataTable();

            dgIn.Columns.Add(new DataColumn("MITE_CODIGO", System.Type.GetType("System.String")));//0
            dgIn.Columns.Add(new DataColumn("PPRE_CODIGO", System.Type.GetType("System.String")));//3
            dgIn.Columns.Add(new DataColumn("MPRE_PRECIO", System.Type.GetType("System.Double")));//9

            #region Código candidato a eliminar
            /*
			dgIn.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			dgIn.Columns.Add(new DataColumn("GRUPO",System.Type.GetType("System.String")));//2
			dgIn.Columns.Add(new DataColumn("FAMILIA",System.Type.GetType("System.String")));//4
			dgIn.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//5
			dgIn.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));//6
			dgIn.Columns.Add(new DataColumn("DESCUENTO",System.Type.GetType("System.String")));//7
			dgIn.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.Double")));//8
			dgIn.Columns.Add(new DataColumn("VALORANTERIOR",System.Type.GetType("System.Double")));//10
			*/
            #endregion

            return dgIn;
        }

        private DataTable AplicarFormatoError(DataTable dgIn)
		{
			dgIn = new DataTable();
			dgIn.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			dgIn.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			dgIn.Columns.Add(new DataColumn("INCONVENIENTE",System.Type.GetType("System.String")));//2
			dgIn.Columns.Add(new DataColumn("INDICADOR",System.Type.GetType("System.String")));//3
			return dgIn;
		}
		
		private string ConstruirFormula(string valorBase, string tipoOperacion, string valorOperado)
		{
			string formula = "";
			string[] sepValBas = valorBase.Split('-');
			string[] sepTipOpe = tipoOperacion.Replace('@','+').Split(',');
			formula += sepValBas[1];
			if(sepTipOpe.Length == 1)
				formula += sepTipOpe[0]+valorOperado;
			else
				formula += sepTipOpe[0]+"("+sepValBas[1]+"*"+(Convert.ToDouble(valorOperado)/100).ToString()+")";
			return formula;
		}
		
		private double CalcularValor(double valorBase, string tipoOperacion, double valorOperado)
		{
			double valorSalida = 0;
			string[] sepTipOpe = tipoOperacion.Replace('@','+').Split(',');
			if(sepTipOpe.Length == 1)
			{
				if(sepTipOpe[0] == "+")
					valorSalida = valorBase + valorOperado;
				else if(sepTipOpe[0] == "-")
					valorSalida = valorBase - valorOperado;
			}
			else
			{
				if(sepTipOpe[0] == "+")
					valorSalida = valorBase + (valorBase*(valorOperado/100));
				else if(sepTipOpe[0] == "-")
					valorSalida = valorBase - (valorBase*(valorOperado/100));
			}
			return valorSalida;
		}
		
		private void PrepararDtOut(ref DataTable dtOut)
		{
			dtOut = new DataTable();
			dtOut.Columns.Add(new DataColumn("CODIGO ITEM",System.Type.GetType("System.String")));//0
			dtOut.Columns.Add(new DataColumn("NOMBRE ITEM",System.Type.GetType("System.String")));//1
			dtOut.Columns.Add(new DataColumn("ANTIGUO VALOR ITEM",System.Type.GetType("System.String")));//2
			dtOut.Columns.Add(new DataColumn("VALOR BASE TOMADO",System.Type.GetType("System.String")));//3
			dtOut.Columns.Add(new DataColumn("NUEVO VALOR ITEM",System.Type.GetType("System.String")));//4
		}
		
		private string ConstruirConsultaAvanzada(DataTable dtFiltros)
		{
			string select = "SELECT DISTINCT MITE.mite_codigo, MITE.mite_nombre, MITE.mite_costrepo, MITE.plin_codigo ,DBXSCHEMA.EDITARREFERENCIAS(MITE.mite_codigo,PLIN.plin_tipo) ";
			//										0				1				2						3					4
			string from = " FROM mitems MITE, plineaitem PLIN,";
			string where = " WHERE MITE.plin_codigo=PLIN.plin_codigo AND";
			for(int i=0;i<dtFiltros.Rows.Count;i++)
			{
				if(dtFiltros.Rows[i][0].ToString().ToUpper()=="LINEA")
					where += " MITE.plin_codigo = '"+dtFiltros.Rows[i][1].ToString()+"' OR";
				else if(dtFiltros.Rows[i][0].ToString().ToUpper()=="MARCA")
					where += " MITE.pmar_codigo = '"+dtFiltros.Rows[i][1].ToString()+"' OR";
				else if(dtFiltros.Rows[i][0].ToString().ToUpper()=="FAMILIA")
					where += " MITE.pfam_codigo = '"+dtFiltros.Rows[i][1].ToString()+"' OR";
				else if(dtFiltros.Rows[i][0].ToString().ToUpper()=="ORIGEN")
					where += " MITE.tori_codigo = '"+dtFiltros.Rows[i][1].ToString()+"' OR";
				else if(dtFiltros.Rows[i][0].ToString().ToUpper()=="DESCUENTO")
					where += " MITE.pdes_codigo = '"+dtFiltros.Rows[i][1].ToString()+"' OR";
				else if(dtFiltros.Rows[i][0].ToString().ToUpper()=="DESCUENTO")
					where += " MITE.pdes_codigo = '"+dtFiltros.Rows[i][1].ToString()+"' OR";
				else//GRUPO VEHICULO
				{
					//Primero revisamos si ya se ha agregado la referencia a mitemsgrupo
					if(from.IndexOf("mitemsgrupo")==-1)
						from += "mitemsgrupo MIG,";
					where += "(MIG.mite_codigo=MITE.mite_codigo AND MIG.pgru_grupo='"+dtFiltros.Rows[i][1].ToString()+"') OR";
				}
			}
			if(dtFiltros.Rows.Count>0)
				return select+from.Substring(0,from.Length-1)+where.Substring(0,where.Length-2);
			else
				return select+from.Substring(0,from.Length-1)+where.Substring(0,where.Length-3);
		}
	}
}
