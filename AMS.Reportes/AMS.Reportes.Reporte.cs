using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;

namespace AMS.Reportes
{
	public class Reporte
	{
		private string nombreReporte, sqlReporte, idReporte, masks;
		private string processMsg="";
		private bool existe;
		private DataTable filtros, filas, footer;
		
		//Propiedades
		public string IdReporte{get{return idReporte;}}
		public string SqlReporte{set{sqlReporte = value;}get{return sqlReporte;}}
		public string NombreReporte{set{nombreReporte = value;}get{return nombreReporte;}}
		public string Masks{set{masks = value;}get{return masks;}}
		public DataTable Filtros{set{filtros = value;}get{return filtros;}}
		public DataTable Filas{set{filas = value;}get{return filas;}}
		public DataTable Footer{set{footer = value;}get{return footer;}}
		public string ProcessMsg{get{return processMsg;}}
		public bool Existe{get{return existe;}}
		//Constructores
		
		public Reporte()
		{
			filtros = new DataTable();
			filas = new DataTable();
			footer = new DataTable();
		}
		
		public Reporte(DataTable filtrosX, DataTable filasX, DataTable footerX)
		{
			filtros = new DataTable();
			filtros = filtrosX;
			filas = new DataTable();
			filas = filasX;
			footer = new DataTable();
			footer = footerX;
		}
		
		public Reporte(string idReporteX)
		{
			//Debemos comprobar que el reporte existe
			if(DBFunctions.RecordExist("SELECT * FROM sreporte WHERE srep_id="+idReporteX+""))
			{
				//Cargamos los datos necesarios del reporte
				idReporte = idReporteX;
				nombreReporte = DBFunctions.SingleData("SELECT srep_nombre FROM sreporte WHERE srep_id="+idReporteX+"");
				sqlReporte = DBFunctions.SingleData("SELECT srep_sql FROM sreporte WHERE srep_id="+idReporteX+"").Replace("?","'");
				masks = DBFunctions.SingleData("SELECT srep_masks FROM sreporte WHERE srep_id="+idReporteX+"");
				//Cargamos los filtros
				DataSet ds1 = new DataSet();
				DBFunctions.Request(ds1,IncludeSchema.YES,"SELECT sfil_id, sfil_contasoc, sfil_tabla, sfil_campo, sfil_datocomp1, sfil_tipcompa, sfil_datocomp2, sfil_etiqueta, sfil_valint FROM sfiltro WHERE srep_id="+idReporteX+" ORDER BY sfil_id");
				filtros = new DataTable();
				filtros = ds1.Tables[0];
				//Cargamos las filas especiales
				DataSet ds2 = new DataSet();
				DBFunctions.Request(ds2,IncludeSchema.YES,"SELECT sfil_id, sfil_posicion, sfil_orden, sfil_alineacion, sfil_valor, sfil_opcion FROM sfilas WHERE srep_id="+idReporteX+" ORDER BY sfil_id");
				filas = new DataTable();
				filas = ds2.Tables[0];
				//Ahora Cargamos las opciones del footer
				DataSet ds3 = new DataSet();
				DBFunctions.Request(ds3,IncludeSchema.YES,"SELECT sfoo_id, sfoo_valor FROM sfooter WHERE srep_id="+idReporteX+" ORDER BY sfoo_id");
				footer = new DataTable();
				footer = ds3.Tables[0];
				existe = true;
			}
			else
				existe = false;
		}
		
		
		//Metodos
		
		public int Grabar_Reporte()
		{
			//Anotacion debemos reemplazar dos caracteres espciales :
			//Vamos a remplazar '(comilla simple) por ?
			//Vamos a remplaza "(comillas) por ¿
			ArrayList sqlStrings = new ArrayList();
			int status = -1;
			int i;
			int ultimoReporte = 0;
			try
			{
				ultimoReporte=System.Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(srep_id) FROM sreporte"));
			}
			catch{}
			//Antes de iniciar el proceso de grabacion debemos arreglar el sqlReporte
			sqlReporte = this.sqlReporte.Replace("'","?");			
			//Primero creamos el registro de la tabla sreporte
			//DBFunctions.NonQuery("INSERT INTO sreporte VALUES(default,'"+this.nombreReporte+"','"+this.sqlReporte+"')");
			sqlStrings.Add("INSERT INTO sreporte VALUES("+(ultimoReporte+1).ToString()+",'"+this.nombreReporte+"','"+this.sqlReporte+"','"+this.masks+"')");
			//Ahora Agregamos los registros de la tabla sfiltros
			for(i=0;i<filtros.Rows.Count;i++)
				sqlStrings.Add("INSERT INTO sfiltro VALUES("+(ultimoReporte+1).ToString()+","+(i+1).ToString()+",'"+filtros.Rows[i][5].ToString()+"','"+filtros.Rows[i][0].ToString()+"','"+filtros.Rows[i][1].ToString()+"','"+filtros.Rows[i][3].ToString()+"','"+filtros.Rows[i][2].ToString()+"','"+filtros.Rows[i][6].ToString()+"')");
			//Ahora agregamos las filas de la tabla de filas
			for(i=0;i<filas.Rows.Count;i++)
			{
				string val = "";
				//Revisamos si el valor de esta fila tiene la palabra reservada FROM que nos indica que el valor es una sentencia select
				if(filas.Rows[i][3].ToString().IndexOf("FROM")!=-1)
				{
					val = filas.Rows[i][3].ToString().Replace("'","?");
				}
				else
					val = filas.Rows[i][3].ToString();
				//Ahora guardamos el registro en la tabla sfilas
				sqlStrings.Add("INSERT INTO sfilas VALUES("+(ultimoReporte+1).ToString()+","+(i+1).ToString()+",'"+filas.Rows[i][0].ToString()+"',"+filas.Rows[i][1].ToString()+",'"+filas.Rows[i][2].ToString()+"','"+val+"','"+filas.Rows[i][4].ToString()+"')");
			}
			//Para terminar la grabacion ahora vamos a guardar los registros de la tabla sfooter
			for(i=0;i<footer.Rows.Count;i++)
			{
				string val = "";
				if(footer.Rows[i][0].ToString().IndexOf("FROM")!=-1)
				{
					val = footer.Rows[i][0].ToString().Replace("'","?");
				}
				else
					val = footer.Rows[i][0].ToString();
				sqlStrings.Add("INSERT INTO sfooter VALUES("+(ultimoReporte+1).ToString()+","+(i+1).ToString()+",'"+val+"')");
			}
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = ultimoReporte+1;
				this.idReporte = (ultimoReporte+1).ToString();
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
			{
				processMsg += DBFunctions.exceptions + "<br>";
			}
			return status;
		}
		
		public bool Eliminar_Reporte()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			string opcionMenu = DBFunctions.SingleData("SELECT smen_codigo FROM smenureporte WHERE srep_id="+this.idReporte+"");
			//Primero eliminamos los registro de las tablas sfilas, sfooter, sfiltro
			sqlStrings.Add("DELETE FROM sfilas WHERE srep_id="+this.idReporte+"");
			sqlStrings.Add("DELETE FROM sfooter WHERE srep_id="+this.idReporte+"");
			sqlStrings.Add("DELETE FROM sfiltro WHERE srep_id="+this.idReporte+"");
			//Ahora eliminamos el registro de la tabla smenuasesoria
			sqlStrings.Add("DELETE FROM smenureporte WHERE srep_id="+this.idReporte+"");
			//Ahora eliminamos el registro de la tabla smenu
			sqlStrings.Add("DELETE FROM smenu WHERE smen_codigo="+opcionMenu+"");
			//Y por ultimo eliminamos el registro de la tabla sreporte
			sqlStrings.Add("DELETE FROM sreporte WHERE srep_id="+this.idReporte+"");
			/////////////////////////////
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				this.processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				this.processMsg+="<BR> Error: Error en Actualizacion <br>"+DBFunctions.exceptions;
			return status;
		}
	}
}
