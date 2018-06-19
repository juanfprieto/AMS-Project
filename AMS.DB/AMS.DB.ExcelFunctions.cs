using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace AMS.DB
{
	public class ExcelFunctions
	{
		protected const string cambioLinea = "<br>";
		private string exceptions, dataSource;
		
		public string Exceptions{set{exceptions = value;}get{return exceptions;}}
		
		public ExcelFunctions(string datSou)
		{
			exceptions = "";
			dataSource = datSou;
		}
		
		private OleDbConnection OpenConnection()
		{
            OleDbConnection con;

            if (dataSource.EndsWith("X") || dataSource.EndsWith("x")) //Microsoft.ACE.OLEDB.12.0
                con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dataSource + ";Extended Properties=\"Excel 12.0 Xml;HDR=NO;IMEX=1\"");                           
            else
                con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dataSource + ";Extended Properties=Excel 8.0;");
            
			try
			{
				con.Open();
				exceptions += cambioLinea + "Se ha abierto el archivo "+dataSource;
			}
			catch(Exception e)
            {
                exceptions += cambioLinea + "Error al abrir el archivo "+dataSource;
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, exceptions);
            }
			return con;
		}
		
		private void CloseConnection(OleDbConnection con)
		{
			try
			{
				con.Close();
				exceptions += cambioLinea + "Se ha cerrado el Archivo";
			}
			catch(Exception e)
			{
				exceptions += cambioLinea + e.ToString();
			}
		}
		
		public DataSet Request(DataSet ds, IncludeSchema isEnum, string sql)
		{
			OleDbConnection con = this.OpenConnection();
			try
			{
				OleDbDataAdapter adapter = new OleDbDataAdapter(sql,con);
				if(isEnum == IncludeSchema.YES)
				{
					adapter.FillSchema(ds,SchemaType.Mapped);
					adapter.Fill(ds);
				}
				else
					adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());
			}
			catch(Exception e)
			{
				exceptions = "Error ejecutando SQL." + cambioLinea + cambioLinea;
				exceptions += e.ToString();
			}
			this.CloseConnection(con);
			return ds;
		}
		
		public int NonQuery(string sql)
		{
			OleDbConnection con = this.OpenConnection();
			OleDbCommand com = new OleDbCommand(sql,con);
			int affectedRows = 0;
			try
			{
				try
				{
					affectedRows = com.ExecuteNonQuery();
				}
				catch(Exception e)
				{
					exceptions += e.ToString() + cambioLinea;	
				}
			}
			catch(Exception e)
			{
				exceptions += e.ToString() + cambioLinea;
			}
			this.CloseConnection(con);
			return affectedRows;
		}
		
		public string SingleData(string sql)
		{
			string val="";
			OleDbConnection con = this.OpenConnection();
			OleDbCommand com = new OleDbCommand(sql,con);
			try
			{
				val = Convert.ToString(com.ExecuteScalar());
			}
			catch(Exception e)
			{
				exceptions += e.ToString() + cambioLinea;
			}
			this.CloseConnection(con);
			return val;
		}
		
		public bool RecordExist(string sql)
		{
			bool exist = false;
			OleDbConnection con = this.OpenConnection();
			OleDbCommand com = new OleDbCommand(sql,con);
			try
			{
				OleDbDataReader rd = com.ExecuteReader();
				exist = rd.Read();
			}
			catch(Exception e)
			{
				exceptions += e.ToString() + cambioLinea;
			}
			this.CloseConnection(con);
			return false;
		}
		
		public bool Transaction(ArrayList sql)
		{
			int numQueries=0, i=0;
			bool status=false;
			OleDbConnection con = this.OpenConnection();
			OleDbCommand com = new OleDbCommand();
			com.Connection = con;
			OleDbTransaction trans = con.BeginTransaction();
			com.Transaction = trans;
			for(i=0; i<sql.Count; i++)
			{
				try
				{
					com.CommandText = sql[i].ToString();
					exceptions += "ejecutando: " + sql[i] + cambioLinea;
					numQueries++;
				}
				catch(Exception e)
				{
					exceptions += "error ejecutando: " + sql[i] + " ::: ";
					exceptions += e.ToString() + cambioLinea;
					status = false;
					break;
				}
			}
			if(numQueries == sql.Count)
			{
				try
				{
					trans.Commit();
					exceptions += "ejecutando Commit -- " + cambioLinea;
					status = true;
				}
				catch(Exception e)
				{
					exceptions += "Error ejecutando Commit: " + e.ToString();
					status = false;
				}
			}
			else
			{
				try
				{
					trans.Rollback();
				}
				catch(Exception e)
				{
					exceptions += "Error ejecutando RollBack: " + e.ToString();
				}
				numQueries = 0;	
			}
			CloseConnection(con);
			return status;
		}
	}
}
