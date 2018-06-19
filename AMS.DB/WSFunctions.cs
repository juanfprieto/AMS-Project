using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;

namespace AMS.DB
{
    class WSFunctions : IDBFunctions
    {
        public string ConnectionString { get; set; }
        public string Exceptions { get; set; }
        private AMSWebService.AMS_WebserviceSoapClient ws;
        private string usr;
        private string pass;

        public WSFunctions()
        {
            ws = new AMSWebService.AMS_WebserviceSoapClient();
            usr = ConfigurationManager.AppSettings["UID"];
            pass = ConfigurationManager.AppSettings["PWD"];
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string sql)
        {
            DataSet ds2 = ws.RequestSQL(usr, pass, sql);
            ds.Tables.Add(ds2.Tables[0].Copy());

            return ds;
        }

        public DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql)
        {
            DataSet ds2 = ws.RequestSQL(usr, pass, sql);
            ds.Tables.Add(ds2.Tables[0].Copy());

            return ds;
        }

        public int NonQuery(string sql)
        {
            return ws.NonQuerySQL(usr, pass, sql);
        }

        public int NonQueryGlobal(string sql, string dataBase)
        {
            return ws.NonQuerySQL(usr, pass, sql);
        }

        public int NonQuery(string sql, byte[] blob)
        {
            return ws.NonQueryBlobSQL(usr, pass, sql, blob); 
        }

        public string SingleData(string sql) 
        {
            return ws.SingleDataSQL(usr, pass, sql); 
        }

        public string SingleDataGlobal(string sql)
        {
            return ws.SingleDataSQL(usr, pass, sql);
        }

        public bool RecordExist(string sql)
        {
            return ws.RecordExistSQL(usr, pass, sql);
        }

        public bool Transaction(ArrayList sql)
        {
            return false;
        }

        public ArrayList RequestAsCollection(string sql)
        {
            DataSet ds = ws.RequestSQL(usr, pass, sql);

            ArrayList list = new ArrayList(ds.Tables[0].Rows.Count);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Hashtable table = new Hashtable();

                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    DataColumn col = ds.Tables[0].Columns[j];
                    Object data = ds.Tables[0].Rows[i][j];
                    table.Add(col.ColumnName, data);
                }
                list.Add(table);
            }

            return list;
        }

        public ArrayList RequestGlobalAsCollection(string sql)
        {
            DataSet ds = ws.RequestSQL(usr, pass, sql);

            ArrayList list = new ArrayList(ds.Tables[0].Rows.Count);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Hashtable table = new Hashtable();

                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    DataColumn col = ds.Tables[0].Columns[j];
                    Object data = ds.Tables[0].Rows[i][j];
                    table.Add(col.ColumnName, data);
                }
                list.Add(table);
            }

            return list;
        }

        public bool SaveHashtable(String tableName, Hashtable hash)
        {
            String sql = "INSERT INTO {0} ({1}) VALUES ({2});";
            int affectedRows = 0;

            string cols = "";
            string values = "";

            foreach (string key in hash.Keys)
            {
                string col = key;
                string value = hash[key].ToString();

                if (!cols.Equals("")) cols += ",";
                if (!values.Equals("")) values += ",";

                cols += col;
                values += value;
            }

            sql = string.Format(sql, tableName, cols, values);

            affectedRows = ws.NonQuerySQL(usr, pass, sql);

            return affectedRows == 1;
        }
        public int UpdateHashtable(string tableName, Hashtable hashData, Hashtable hashPk)
        {
            String sql = "UPDATE {0} SET {1} WHERE {2};";
            int affectedRows = 0;

            string set = "";
            string where = "";

            foreach (string key in hashData.Keys)
            {
                string col = key;
                string value = hashData[key].ToString();

                if (!set.Equals("")) set += ",";

                set += String.Format("{0} = {1}", col, value);
            }

            foreach (string key in hashPk.Keys)
            {
                string col = key;
                string value = hashPk[key].ToString();

                if (!where.Equals("")) where += " AND ";

                where += String.Format("{0} = {1}", col, value);
            }

            sql = string.Format(sql, tableName, set, where);
            affectedRows = ws.NonQuerySQL(usr, pass, sql);

            return affectedRows;
        }

        public int DeleteHashtable(string tableName, Hashtable hashPk)
        {
            String sql = "DELETE FROM {0} WHERE {1};";
            int affectedRows = 0;

            string where = "";

            foreach (string key in hashPk.Keys)
            {
                string col = key;
                string value = hashPk[key].ToString();

                if (!where.Equals("")) where += " AND ";

                where += String.Format("{0} = {1}", col, value);
            }

            sql = string.Format(sql, tableName, where);
            affectedRows = ws.NonQuerySQL(usr, pass, sql);

            return affectedRows;
        }

        public void CloseConnection()
        {
            Exceptions = "Not Implemented";
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string nombreProcedimiento, IDictionaryEnumerator parametros)
        {
            Exceptions = "Not Implemented";
            return null;
        }
    }
}
