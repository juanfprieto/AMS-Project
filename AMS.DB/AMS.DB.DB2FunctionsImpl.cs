using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using IBM.Data.DB2;
using AMS.CriptoServiceProvider;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AMS.DB
{
    public class DB2FunctionsImpl : IDBFunctions
    {
        protected DB2Connection connection;

        public DB2FunctionsImpl()
        {

        }

        public void OpenConnection()
        {
            string connectionString;

            string servidor = "";// ConfigurationManager.AppSettings["Server"];
            string database= "";//ConfigurationManager.AppSettings["DataBase"];
            string usuario= "";//ConfigurationManager.AppSettings["UID"];
            string password= "";//ConfigurationManager.AppSettings["PWD"];

            //string timeout=ConfigurationManager.AppSettings["ConnectionTimeout"];
            //string port = ConfigurationManager.AppSettings["DataBasePort"];
            //AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            //miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            //miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            //string newPwd=miCripto.DescifrarCadena(password);

            //connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd;
            //if (timeout != null) connectionString += ";Connection Timeout=" + timeout;
            
            //para 2015
            servidor = ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
            database = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
            usuario = ConfigurationManager.AppSettings["UID"];
            password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];


            string timeout = ConfigurationManager.AppSettings["ConnectionTimeout"];
            string port = ConfigurationManager.AppSettings["DataBasePort"];
            AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd = miCripto.DescifrarCadena(password);

            connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd + ";QueryTimeout=3600";
            if (timeout != null) connectionString += ";Connection Timeout=" + timeout;

            connection = new DB2Connection(connectionString);

            connection.Open();
            SetSchema();
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        private void SetSchema()
        {
            string schema = ConfigurationManager.AppSettings["Schema"];
            DB2Command sql = new DB2Command();
            sql.Connection = connection;
            sql.CommandText = "SET SCHEMA " + schema;

            sql.ExecuteNonQuery();
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string sql)
        {
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            DB2DataAdapter adapter = new DB2DataAdapter(sql, connection);
            if (isEnum == IncludeSchema.YES)
            {
                adapter.FillSchema(ds, SchemaType.Mapped);
                adapter.Fill(ds);
            }
            else
                adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());

            return ds;
        }

        public DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql)
        {
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            DB2DataAdapter adapter = new DB2DataAdapter(sql, connection);
            if (isEnum == IncludeSchema.YES)
            {
                adapter.FillSchema(ds, SchemaType.Mapped);
                adapter.Fill(ds);
            }
            else
                adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());

            return ds;
        }

        public ArrayList RequestAsCollection(string sql)
        {
            ArrayList result = null;

            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            DB2DataAdapter adapter = new DB2DataAdapter(sql, connection);
            DataSet ds = new DataSet();

            adapter.Fill(ds);

            if (ds.Tables.Count > 1) throw new Exception("Consulta inválida");

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

            result = list;

            return result;
        }

        public ArrayList RequestGlobalAsCollection(string sql)
        {
            ArrayList result = null;

            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            DB2DataAdapter adapter = new DB2DataAdapter(sql, connection);
            DataSet ds = new DataSet();

            adapter.Fill(ds);

            if (ds.Tables.Count > 1) throw new Exception("Consulta inválida");

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

            result = list;

            return result;
        }

        public int NonQuery(string sql)
        {
            DB2Command com = new DB2Command(sql, connection);
            int affectedRows;

            affectedRows = com.ExecuteNonQuery();

            return affectedRows;
        }

        public int NonQueryGlobal(string sql, string dataBase)
        {

            DB2Command com = new DB2Command(sql, connection);
            int affectedRows;

            affectedRows = com.ExecuteNonQuery();

            return affectedRows;
        }

        public int NonQuery(string sql, byte[] blob)
        {
            DB2Command com = new DB2Command(sql, connection);

            DB2Parameter parm1 = new DB2Parameter();
            parm1.DbType = DbType.Binary;
            parm1.ParameterName = "@blob";
            parm1.Value = (byte[])blob;

            com.Parameters.Add(parm1);

            return com.ExecuteNonQuery();
        }

        public string SingleData(string sql)
        {
            string val;
            DB2Command command = new DB2Command();
            command.Connection = connection;
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;

            val = Convert.ToString(command.ExecuteScalar());

            return val;
        }

        public string SingleDataGlobal(string sql)
        {
            string val;
            DB2Command command = new DB2Command();
            command.Connection = connection;
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;

            val = Convert.ToString(command.ExecuteScalar());

            return val;
        }

        public bool RecordExist(string sql)
        {
            bool exist = false;
            DB2Command command = new DB2Command();
            command.Connection = connection;
            command.CommandText = sql;

            DB2DataReader db2dr = command.ExecuteReader();
            exist = db2dr.Read();
            db2dr.Close();

            return exist;
        }

        public bool Transaction(ArrayList sql)
        {
            bool commit = true;
            String exceptions = "";

            DB2Command command = new DB2Command();
            DB2Transaction trans = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = trans;

            for (int i = 0; i < sql.Count; i++)
            {
                command.CommandText = sql[i].ToString();

                try
                {
                    command.ExecuteNonQuery();
                }
                catch
                {
                    exceptions += String.Format("error ejecutando: {0}\n", sql[i]);
                    commit = false;
                }
            }

            if (commit)
                trans.Commit();

            else
                trans.Rollback();

            Exceptions = exceptions;
            return commit;
        }

        public bool SaveHashtable(String tableName, Hashtable hash)
        {
            DB2Command com = null;
            String sql = "INSERT INTO {0} ({1}) VALUES ({2});";
            int affectedRows = 0;

            string cols = "";
            string values = "";

            foreach (DictionaryEntry de in hash.Keys)
            {
                string col = de.Key.ToString();
                string value = de.Value.GetType().Equals(typeof(string)) ? "'" + de.Value + "'" : de.Value.ToString();

                if (!cols.Equals("")) cols += ",";
                if (!values.Equals("")) values += ",";

                cols += col;
                values += value;
            }

            sql = string.Format(sql, tableName, cols, values);

            com = new DB2Command(sql, connection);
            affectedRows = com.ExecuteNonQuery();

            return affectedRows == 1;
        }

        public int DeleteHashtable(string tableName, Hashtable hashPk)
        {
            DB2Command com = null;
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
            com = new DB2Command(sql, connection);
            affectedRows = com.ExecuteNonQuery();

            return affectedRows;
        }

        public DB2TransactionImpl BeginTransaction()
        {
            return new DB2TransactionImpl(connection);
        }

        public string ConnectionString { get; set; }
        public string Exceptions { get; set; }

        public int UpdateHashtable(string tableName, Hashtable hashData, Hashtable hashPk)
        {
            Exceptions = "Not Implemented";
            return -1;
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string nombreProcedimiento, IDictionaryEnumerator parametros)
        {
            Exceptions = "Not Implemented";
            return null;
        }
    }

    public class DB2TransactionImpl
    {
        private DB2Transaction trans;

        public DB2TransactionImpl(DB2Connection connection)
        {
            trans = connection.BeginTransaction();
        }

        public void commit()
        {
            trans.Commit();
        }

        public void rollback()
        {
            trans.Rollback();
        }
    }
}
