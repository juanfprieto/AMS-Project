	// created on 29/10/2003 at 02:16 a.m.
	
	
	using AMS.DB;
	using System;
	using System.Collections;
	using System.Data.Odbc;
	using System.Data;
	
	
	
	namespace AMS.Reclasificacion  
	{
		
		public class Reclasificar 
		{
			private  string  newpar,oldpar,field,processMsg,tablaPrincipal;
			private DataSet tablasDependientes, tablasMaestras;
			private ArrayList operaciones;
				
			public string ProcessMsg{get{return processMsg;}}
		
		
			public Reclasificar(string newparam,string oldparam, string campokey, string tablaBorrado)	
			{
			
				newpar=newparam;
				oldpar=oldparam;	
				tablaPrincipal=tablaBorrado;
				field=campokey;
				operaciones = new ArrayList();
				//A continuacion llenamos un ArrayList llamado table con los nombres de las tablas que tienen de referencia a tablaBorrado
				tablasDependientes = new DataSet();
				DBFunctions.Request(tablasDependientes,IncludeSchema.NO,"SELECT tbname, fkcolnames from SYSIBM.SYSRELS where reftbname = '"+tablaBorrado.ToUpper()+"' AND tbname NOT LIKE 'M%' ORDER BY TBNAME");
				tablasMaestras = new DataSet();
				DBFunctions.Request(tablasMaestras,IncludeSchema.NO,"SELECT tbname, fkcolnames from SYSIBM.SYSRELS where reftbname = '"+tablaBorrado.ToUpper()+"' AND tbname LIKE 'M%' ORDER BY TBNAME");
			}
			
		
			public bool ProcUnParam()
			{
				int i;
				bool status = false;
				ArrayList sqlStrings = new ArrayList();
				//Empezamos recorriendo el DataSet que tiene las tablas que hacen referencia a la tablaBorrado
				for(i=0; i<tablasDependientes.Tables[0].Rows.Count; i++)
				{
					//Ahora debemos revisar si el campo dentro de esta tabla hace parte de la llave primaria
					if(this.Determinar_Llave_Primaria(DBFunctions.SingleData("SELECT colnames FROM SYSIBM.SYSINDEXES WHERE tbname='"+tablasDependientes.Tables[0].Rows[i][0].ToString()+"'"),tablasDependientes.Tables[0].Rows[i][1].ToString()))
					{
						//si hace parte de la llave primaria revisamos si existe un registro con el valor que esta bien
						//si este valor existe debemos eliminar el registro, sino lo actualizamos simplemente
						if(DBFunctions.RecordExist("SELECT * FROM "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"'"))
							sqlStrings.Add("DELETE FROM "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
						else
							sqlStrings.Add("UPDATE "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" SET "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
					}
					else
						sqlStrings.Add("UPDATE "+tablasDependientes.Tables[0].Rows[i][0].ToString()+" SET "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasDependientes.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
				}
				//Ahora realizamos la actualizacion dentro de las tablas maestras y eleminamos el registro
				for(i=0; i<tablasMaestras.Tables[0].Rows.Count; i++)
				{
					if(this.Determinar_Llave_Primaria(DBFunctions.SingleData("SELECT colnames FROM SYSIBM.SYSINDEXES WHERE tbname='"+tablasMaestras.Tables[0].Rows[i][0].ToString()+"'"),tablasMaestras.Tables[0].Rows[i][1].ToString()))
					{
						if(DBFunctions.RecordExist("SELECT * FROM "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"'"))
							sqlStrings.Add("DELETE FROM "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
						else
							sqlStrings.Add("UPDATE "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" SET "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
					}
					else
						sqlStrings.Add("UPDATE "+tablasMaestras.Tables[0].Rows[i][0].ToString()+" SET "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.newpar+"' WHERE "+tablasMaestras.Tables[0].Rows[i][1].ToString()+"='"+this.oldpar+"'");
				}
				sqlStrings.Add("DELETE FROM "+this.tablaPrincipal+" WHERE "+this.field+"='"+this.oldpar+"'");
				if(DBFunctions.Transaction(sqlStrings))
				{
					status = true;
					processMsg += DBFunctions.exceptions + "<br>";
				}
				else
					processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
				return status;
			}
			
			private bool Determinar_Llave_Primaria(string colnames, string name)
			{
				bool encontrado = false;
				string []nombres = colnames.Split('+');
				for(int i=0;i<nombres.Length;i++)
				{
					if(nombres[i]==name)
						encontrado = true;
				}
				return encontrado;
			}
		}
	}
