using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;

namespace AMS.DB
{
	public class DBFunctions
	{
		protected const string cambioLinea = "<br>";
        protected string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
		public static bool deleteStatus = false;
		public static bool insertStatus = false;
		public static bool updateStatus = false;
		public static string exceptions="";
        protected static string dbConnectionType = ConfigurationManager.AppSettings["DBConnectionType"];
        protected static DBAdapters dba = (dbConnectionType == "DB2") ? DBAdapters.DB2 : (dbConnectionType == "SQLSERVER") ? DBAdapters.SQLSERVER : DBAdapters.ODBC;
		public static string sqlSelect="";

        private static IDBFunctions func = DBFunctionsFactory.CreateDBFunctions(dba);
		
		public string ConnectionString{ get{return connectionString;}set{connectionString = value;} }
		public string Exceptions{ get{return DBFunctions.exceptions;}set{DBFunctions.exceptions = value;} }

		public DBFunctions()
		{
        }
	
		public static bool RecordExist(string sql)
		{
			bool exist=false;

            exist = func.RecordExist(sql);
            exceptions = func.Exceptions;
			
			return exist;
		}
		
		public bool CheckRecordConds(string table, string field, string data, string conds)
		{
			return true;
		}
		
		public static string SingleData(string sql)
		{
			string val="";
            val = func.SingleData(sql);
            exceptions = func.Exceptions;
			
			return val;
		}

        public static string SingleDataGlobal(string sql)
        {
            string val = "";
            val = func.SingleDataGlobal(sql);
            exceptions = func.Exceptions;

            return val;
        }
		
		public string LastRecord(string table, string field)
		{
			
			string num="";
			
			return num;
		}
		
		public static DataSet Request(DataSet ds, IncludeSchema isEnum, string sql)
		{
            ds = func.Request(ds, isEnum, sql);
            exceptions = func.Exceptions;

			return ds;
		}

        public static DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql)
        {
            ds = func.RequestGlobal(ds, isEnum, dataBase, sql);
            exceptions = func.Exceptions;

            return ds;
        }

        public static DataSet Request(DataSet ds, IncludeSchema isEnum, string nombreProcedimiento, IDictionaryEnumerator parametros)
		{
            ds = func.Request(ds, isEnum, nombreProcedimiento, parametros);
            exceptions = func.Exceptions;

			return ds;
		}


        public static ArrayList RequestAsCollection(string sql)
        {
            ArrayList result = null;

            result = func.RequestAsCollection(sql);
            exceptions = func.Exceptions;

            return result;
        }

        public static ArrayList RequestGlobalAsCollection(string sql)
        {
            ArrayList result = null;

            result = func.RequestGlobalAsCollection(sql);
            exceptions = func.Exceptions;

            return result;
        }

        public static int NonQuery(string sql)
		{
			int affectedRows=0;

            affectedRows = func.NonQuery(sql);
            exceptions = func.Exceptions;
			
			return affectedRows;
		}

        public static int NonQueryGlobal(string sql, string dataBase)
        {
            int affectedRows = 0;

            affectedRows = func.NonQueryGlobal(sql, dataBase);
            exceptions = func.Exceptions;

            return affectedRows;
        }

        public static int NonQuery(string sql, byte[] blob)
        {
            int affectedRows=0;

            affectedRows = func.NonQuery(sql, blob);
            exceptions = func.Exceptions;

            return affectedRows;
        }
		
		public static bool Transaction(ArrayList sql)
		{
			bool status=false;

            status = func.Transaction(sql);
            exceptions = func.Exceptions;
			
			return status;
		}

        public static void closeConnection()
        {
            func.CloseConnection();
            exceptions = func.Exceptions;
        }

        public static bool SaveHashtable(String tableName, Hashtable hash)
        {
            bool status = false;

            status = func.SaveHashtable(tableName, hash);
            exceptions = func.Exceptions;

            return status;
        }

        public static int UpdateHashtable(String tableName, Hashtable hashData, Hashtable hashPk)
        {
            int status = -1;

            status = func.UpdateHashtable(tableName, hashData, hashPk);
            exceptions = func.Exceptions;

            return status;
        }

        public static int DeleteHashtable(string tableName, Hashtable hashPk)
        {
            int status = -1;

            status = func.DeleteHashtable(tableName, hashPk);
            exceptions = func.Exceptions;

            return status;
        }
    }
}
