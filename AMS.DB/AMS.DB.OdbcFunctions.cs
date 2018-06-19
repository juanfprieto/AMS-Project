// created on 11/03/2004 at 12:36


using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;


namespace AMS.DB
{


    public class OdbcFunctions : IDBFunctions
    {

        protected int affectedRows = 0;
        protected bool insertStatus, deleteStatus, updateStatus;
        protected string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        protected string schema=ConfigurationManager.AppSettings["Schema"];
        protected string sqlSelect;
        protected string exceptions="";
        protected Hashtable typesRelation = new Hashtable();
        protected Hashtable lengthsRelation = new Hashtable();
        protected OdbcConnection connection;
        protected OdbcParameter parm;


        public int AffectedRows { get { return affectedRows; } set { affectedRows = value; } }
        public bool DeleteStatus { get { return deleteStatus; } }
        public bool InsertStatus { get { return insertStatus; } }
        public bool UpdateStatus { get { return updateStatus; } }
        public string Exceptions { get { return exceptions; } set { exceptions = value; } }
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        public string SQLSelect { set { sqlSelect = value; } }
        public OdbcConnection Connection { get { return connection; } }

        public OdbcFunctions()
        {
            typesRelation.Add(typeof(string), OdbcType.VarChar);
            typesRelation.Add(typeof(System.Int16), OdbcType.SmallInt);
            typesRelation.Add(typeof(System.Int32), OdbcType.Int);
            typesRelation.Add(typeof(decimal), OdbcType.Decimal);
            typesRelation.Add(typeof(double), OdbcType.Double);
            typesRelation.Add(typeof(System.DateTime), OdbcType.Date);

            lengthsRelation.Add(OdbcType.SmallInt, 5);
            lengthsRelation.Add(OdbcType.Decimal, 7);
            lengthsRelation.Add(OdbcType.Double, 8);
            lengthsRelation.Add(OdbcType.Int, 11);
            lengthsRelation.Add(OdbcType.Date, 10);
        }

        public OdbcConnection OpenConnection()
        {
            OdbcConnection con = new OdbcConnection(connectionString);
            try
            {
                con.Open();
                SetSchema(con);
                exceptions = "Se ha abierto una conexión con la base de datos:";
                exceptions += con.Database + " usando " + con.Driver + " " + con.ServerVersion + "<br>";
            }
            catch (Exception e)
            {
                exceptions = "Error al conectar con la base de datos especificada.<br><br>";
                exceptions += e.ToString();
            }
            return con;
        }

        public bool SetSchema(OdbcConnection con)
        {
            bool status = false;
            OdbcCommand sql = new OdbcCommand();
            sql.Connection = con;
            sql.CommandText = "SET SCHEMA " + schema;

            try
            {
                sql.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                exceptions = "Error al ejecutar: <br><br>";
                exceptions += e.ToString();

                status = false;
            }

            return status;

        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string sql)
        {
            OdbcConnection lc = OpenConnection();
            try
            {
                OdbcDataAdapter adapter = new OdbcDataAdapter(sql, lc);
                if (isEnum == IncludeSchema.YES)
                    adapter.FillSchema(ds, SchemaType.Mapped);
                adapter.Fill(ds, "result_" + (ds.Tables.Count + 1).ToString());
            }
            catch (Exception e)
            {
                exceptions = "Error ejecutando SQL.<br><br>";
                exceptions += e.ToString();
            }
            CloseConnection(lc);
            return ds;
        }

        public DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql)
        {
            OdbcConnection lc = OpenConnection();
            try
            {
                OdbcDataAdapter adapter = new OdbcDataAdapter(sql, lc);
                if (isEnum == IncludeSchema.YES)
                    adapter.FillSchema(ds, SchemaType.Mapped);
                adapter.Fill(ds, "result_" + (ds.Tables.Count + 1).ToString());
            }
            catch (Exception e)
            {
                exceptions = "Error ejecutando SQL.<br><br>";
                exceptions += e.ToString();
            }
            CloseConnection(lc);
            return ds;
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string nombreProcedimiento, IDictionaryEnumerator parametros)
        {
            OdbcConnection lc = OpenConnection();
            try
            {
                OdbcCommand comm = new OdbcCommand(nombreProcedimiento, lc);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                if (parametros != null)
                {
                    while (parametros.MoveNext())
                        comm.Parameters.Add(parametros.Key.ToString(), parametros.Value);
                }
                OdbcDataAdapter adapter = new OdbcDataAdapter(comm);
                if (isEnum == IncludeSchema.YES)
                    adapter.FillSchema(ds, SchemaType.Mapped);
                adapter.Fill(ds, "result_" + (ds.Tables.Count + 1).ToString());
            }
            catch (Exception e)
            {
                exceptions = "Error ejecutando SQL.<br><br>";
                exceptions += e.ToString();
            }
            CloseConnection(lc);
            return ds;
        }

        public DataSet Insert(DataSet ds, int index)
        {
            int i=0;
            string table="Table";
            string fieldsString = "(";
            string valuesString = "(";
            string[] sqlWords = sqlSelect.Split(' ');

            for (i = 0; i < sqlWords.Length; i++)
                if (sqlWords[i] == "FROM")
                {
                    table = sqlWords[i + 1];
                    break;
                }

            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                fieldsString += ds.Tables[index].Columns[i].ColumnName;
                valuesString += "?";
                if (i != ds.Tables[index].Columns.Count - 1)
                {
                    fieldsString += ", ";
                    valuesString += ", ";
                }
            }

            fieldsString += ")";
            valuesString += ")";

            OdbcConnection dc = OpenConnection();

            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlSelect, dc);
            OdbcCommand insertCommand = new OdbcCommand("INSERT INTO  " + table + " " + fieldsString + " VALUES " + valuesString + "", adapter.SelectCommand.Connection);


            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {

                if (ds.Tables[index].Columns[i].DataType.ToString() == "System.String")
                    insertCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (OdbcType)typesRelation[ds.Tables[index].Columns[i].DataType], ds.Tables[index].Columns[i].MaxLength, ds.Tables[index].Columns[i].ColumnName);
                else
                    insertCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (OdbcType)typesRelation[ds.Tables[index].Columns[i].DataType], (int)lengthsRelation[(OdbcType)typesRelation[ds.Tables[index].Columns[i].DataType]], ds.Tables[index].Columns[i].ColumnName);


            }

            adapter.InsertCommand = insertCommand;

            insertStatus = false;
            try
            {
                affectedRows = adapter.Update(ds);
                insertStatus = true;
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "INSERT INTO  " + table + " " + fieldsString + " VALUES " + valuesString + "" + "<br>";
                exceptions += "parameters: " + insertCommand.Parameters.Count.ToString() + "<br>";
            }
            CloseConnection(dc);

            return ds;
        }

        public DataSet Delete(DataSet ds, int index)
        {
            int i=0;
            string table="Table";
            string whereString = " WHERE ";
            string[] sqlWords = sqlSelect.Split(' ');

            for (i = 0; i < sqlWords.Length; i++)
                if (sqlWords[i] == "FROM")
                {
                    table = sqlWords[i + 1];
                    break;
                }

            for (i = 0; i < ds.Tables[0].PrimaryKey.Length; i++)
            {
                whereString += ds.Tables[0].PrimaryKey[i] + " = ?";
                if (i != ds.Tables[0].PrimaryKey.Length - 1)
                    whereString += " AND ";
            }

            OdbcConnection dc = OpenConnection();

            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlSelect, dc);
            OdbcCommand deleteCommand = new OdbcCommand("DELETE FROM " + table + " " + whereString + "", adapter.SelectCommand.Connection);

            for (i = 0; i < ds.Tables[0].PrimaryKey.Length; i++)
            {
                if (ds.Tables[0].PrimaryKey[i].DataType.ToString() == "System.String")
                    deleteCommand.Parameters.Add("@" + ds.Tables[0].PrimaryKey[i] + "", (OdbcType)typesRelation[ds.Tables[0].PrimaryKey[i].DataType], ds.Tables[0].PrimaryKey[i].MaxLength, ds.Tables[0].PrimaryKey[i].ColumnName);
                else
                    deleteCommand.Parameters.Add("@" + ds.Tables[0].PrimaryKey[i] + "", (OdbcType)typesRelation[ds.Tables[0].PrimaryKey[i].DataType], (int)lengthsRelation[(OdbcType)typesRelation[ds.Tables[0].PrimaryKey[i].DataType]], ds.Tables[0].PrimaryKey[i].ColumnName);
            }

            adapter.DeleteCommand = deleteCommand;
            deleteStatus = false;
            try
            {
                affectedRows = adapter.Update(ds);
                deleteStatus = true;
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "<br>";
            }
            CloseConnection(dc);

            return ds;
        }

        public DataSet Update(DataSet ds, int index)
        {
            int i=0;
            string table="Table";
            string fieldsString = " SET ";
            string whereString = " WHERE ";
            string[] sqlWords = sqlSelect.Split(' ');

            for (i = 0; i < sqlWords.Length; i++)
                if (sqlWords[i] == "FROM")
                {
                    table = sqlWords[i + 1];
                    break;
                }

            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                fieldsString += ds.Tables[index].Columns[i].ColumnName + " = ?";
                if (i != ds.Tables[index].Columns.Count - 1)
                    fieldsString += ", ";
            }

            for (i = 0; i < ds.Tables[index].PrimaryKey.Length; i++)
            {
                whereString += ds.Tables[index].PrimaryKey[i] + " = ?";
                if (i != ds.Tables[index].PrimaryKey.Length - 1)
                    whereString += " AND ";
            }



            OdbcConnection dc = OpenConnection();

            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlSelect, dc);
            OdbcCommand updateCommand = new OdbcCommand("UPDATE " + table + fieldsString + whereString, adapter.SelectCommand.Connection);


            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                if (ds.Tables[index].Columns[i].DataType.ToString() == "System.String")
                    updateCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (OdbcType)typesRelation[ds.Tables[index].Columns[i].DataType], ds.Tables[index].Columns[i].MaxLength, ds.Tables[index].Columns[i].ColumnName);
                else
                    updateCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (OdbcType)typesRelation[ds.Tables[index].Columns[i].DataType], (int)lengthsRelation[(OdbcType)typesRelation[ds.Tables[index].Columns[i].DataType]], ds.Tables[index].Columns[i].ColumnName);
            }

            for (i = 0; i < ds.Tables[index].PrimaryKey.Length; i++)
            {
                if (ds.Tables[index].PrimaryKey[i].DataType.ToString() == "System.String")
                    parm = updateCommand.Parameters.Add("@old" + ds.Tables[index].PrimaryKey[i] + "", (OdbcType)typesRelation[ds.Tables[index].PrimaryKey[i].DataType], ds.Tables[index].PrimaryKey[i].MaxLength, ds.Tables[index].PrimaryKey[i].ColumnName);
                else
                    parm = updateCommand.Parameters.Add("@old" + ds.Tables[index].PrimaryKey[i] + "", (OdbcType)typesRelation[ds.Tables[index].PrimaryKey[i].DataType], (int)lengthsRelation[(OdbcType)typesRelation[ds.Tables[index].PrimaryKey[i].DataType]], ds.Tables[index].PrimaryKey[i].ColumnName);

                parm.SourceVersion = DataRowVersion.Original;
            }

            adapter.UpdateCommand = updateCommand;

            updateStatus = false;
            try
            {
                affectedRows = adapter.Update(ds.Tables[index]);
                updateStatus = true;
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "<br>";
            }
            CloseConnection(dc);

            return ds;
        }

        public int NonQuery(string sql)
        {
            OdbcConnection ncc = OpenConnection();
            OdbcCommand com = new OdbcCommand(sql, ncc);
            try
            {
                com.Connection.Open();
                try
                {
                    affectedRows = com.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    exceptions += e.ToString() + "<br>";
                }
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "<br>";
            }
            CloseConnection(ncc);

            return affectedRows;
        }

        public int NonQueryGlobal(string sql, string dataBase)
        {
            OdbcConnection ncc = OpenConnection();
            OdbcCommand com = new OdbcCommand(sql, ncc);
            try
            {
                com.Connection.Open();
                try
                {
                    affectedRows = com.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    exceptions += e.ToString() + "<br>";
                }
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "<br>";
            }
            CloseConnection(ncc);

            return affectedRows;
        }

        public int NonQuery(string sql, byte[] blob)
        {
            exceptions += "Not Implemented";

            return -1;
        }

        public OdbcDataReader DataReader(string sql)
        {
            OdbcCommand oc = new OdbcCommand();
            try
            {
                oc.CommandText = sql;
                OdbcConnection con = new OdbcConnection();
                connection = con;
                oc.Connection = con;
                OdbcDataReader dr = oc.ExecuteReader();
                return dr;
            }
            catch (Exception e)
            {
                exceptions = "Error ejecutando SQL.<br><br>";
                exceptions += e.ToString();
                return null;
            }

        }

        public string SingleData(string sql)
        {
            string val="";
            OdbcCommand command = new OdbcCommand();
            OdbcConnection con = OpenConnection();
            command.Connection = con;
            command.CommandText = sql;

            try
            {
                val = (string)command.ExecuteScalar();
            }
            catch (Exception e)
            {
                exceptions = e.ToString();
            }
            CloseConnection(con);
            return val;
        }

        public string SingleDataGlobal(string sql)
        {
            string val = "";
            OdbcCommand command = new OdbcCommand();
            OdbcConnection con = OpenConnection();
            command.Connection = con;
            command.CommandText = sql;

            try
            {
                val = (string)command.ExecuteScalar();
            }
            catch (Exception e)
            {
                exceptions = e.ToString();
            }
            CloseConnection(con);
            return val;
        }

        public bool RecordExist(string sql)
        {
            bool exist=false;
            OdbcCommand command = new OdbcCommand();
            OdbcConnection con = OpenConnection();
            command.Connection = con;
            command.CommandText = sql;

            try
            {
                OdbcDataReader dr = command.ExecuteReader();

                //dr.Read();
                exist = dr.HasRows;
                //dr.Close();

            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "<br>";
            }
            CloseConnection(con);
            return exist;
        }

        public bool Transaction(ArrayList sql)
        {
            int numQueries=0, i=0;
            bool status=false;

            OdbcCommand command = new OdbcCommand();
            OdbcConnection con = OpenConnection();
            command.Connection = con;

            OdbcTransaction trans = con.BeginTransaction();
            command.Transaction = trans;

            for (i = 0; i < sql.Count; i++)
            {
                command.CommandText = sql[i].ToString();
                try
                {
                    command.ExecuteNonQuery();
                    numQueries++;
                }
                catch (Exception e)
                {
                    exceptions += e.ToString();
                    status = false;
                    break;
                }
            }

            if (numQueries == sql.Count - 1)
            {

                try
                {
                    trans.Commit();
                    status = true;
                }
                catch (Exception e)
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
                catch (Exception e)
                {
                    exceptions += "Error ejecutando RollBack: " + e.ToString();
                }
                numQueries = 0;
            }

            CloseConnection(con);

            return status;

        }

        public void CloseConnection(OdbcConnection con)
        {
            try
            {
                con.Close();
                exceptions += "se ha cerrado la conexión<br>";
            }
            catch (Exception e)
            {
                exceptions += "Error cerrando conexión " + e.ToString() + "<br>";
            }
        }

        public void CloseConnection()
        {
            Exceptions = "Not Implemented";
        }

        public ArrayList RequestAsCollection(string sql)
        {
            Exceptions = "Not Implemented";
            return null;
        }

        public ArrayList RequestGlobalAsCollection(string sql)
        {
            Exceptions = "Not Implemented";
            return null;
        }

        public bool SaveHashtable(String tableName, Hashtable hash)
        {
            Exceptions = "Not Implemented";
            return false;
        }
        public int UpdateHashtable(string tableName, Hashtable hashData, Hashtable hashPk)
        {
            Exceptions = "Not Implemented";
            return -1;
        }
        public int DeleteHashtable(string tableName, Hashtable hashPk)
        {
            Exceptions = "Not Implemented";
            return -1;
        }
    }
}
