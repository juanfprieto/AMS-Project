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
using IBM.Data.DB2Types;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace AMS.DB
{
    public class SQLServerFunctions : IDBFunctions
    {
        protected const string cambioLinea = "<br>";
        protected int affectedRows = 0;
        protected bool insertStatus, deleteStatus, updateStatus;
        protected string connectionString;// = ConfigurationManager.AppSettings["ConnectionString"];
        protected string schema = ConfigurationManager.AppSettings["Schema"];
        protected string sqlSelect;
        protected string exceptions;
        protected Hashtable typesRelation = new Hashtable();
        protected Hashtable lengthsRelation = new Hashtable();
        protected SqlConnection connection;
        protected SqlParameter parm;
        protected string AplicacionActual;
        protected string Sql = "";

        public const String TABLA_SEGUIR = "MNIT"; //Constante de tabla para hacer seguimiento a operaciones Delete, Insert, Update. (poner MHISTORIAL_CAMBIOS para desactivar...)

        public string p_AplicacionActual { get { return AplicacionActual; } set { AplicacionActual = value; } }
        public int AffectedRows { get { return affectedRows; } set { affectedRows = value; } }
        public bool DeleteStatus { get { return deleteStatus; } }
        public bool InsertStatus { get { return insertStatus; } }
        public bool UpdateStatus { get { return updateStatus; } }
        public string Exceptions { get { return exceptions; } set { exceptions = value; } }
        public string ConnectionString { get { return connectionString; } set { connectionString = value; } }
        public string SQLSelect { set { sqlSelect = value; } }
        public SqlConnection Connection { get { return connection; } }

        public SQLServerFunctions()
        {
            typesRelation.Add(typeof(string), DB2Type.VarChar);
            typesRelation.Add(typeof(System.Int16), DB2Type.SmallInt);
            typesRelation.Add(typeof(System.Int32), DB2Type.Integer);
            typesRelation.Add(typeof(decimal), DB2Type.Decimal);
            typesRelation.Add(typeof(double), DB2Type.Double);
            typesRelation.Add(typeof(System.DateTime), DB2Type.Date);

            lengthsRelation.Add(DB2Type.SmallInt, 5);
            lengthsRelation.Add(DB2Type.Decimal, 7);
            lengthsRelation.Add(DB2Type.Double, 8);
            lengthsRelation.Add(DB2Type.Integer, 11);
            lengthsRelation.Add(DB2Type.Date, 10);
        }

        public SqlConnection OpenConnection()
        {
            //if (connection != null && connection.IsOpen) return connection;
            string algo = HttpContext.Current.Request.Url.AbsolutePath;
            string algo2 = HttpContext.Current.Request.Url.AbsoluteUri;
            string host = HttpContext.Current.Request.Url.Host;


            string servidor = ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
            string database = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string schema = ConfigurationManager.AppSettings["SCHEMA"];
            string password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];
            string timeout = ConfigurationManager.AppSettings["ConnectionTimeout"];
            string port = ConfigurationManager.AppSettings["DataBasePort"];
            AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd = miCripto.DescifrarCadena(password);

            //connectionString = "data source=ECASMIN\\MSSQLSERVER2;initial catalog=MAZKOP;user id=sa;password=.ecas2010.;";
            //connectionString = "data source=LAPTOP1\\caldana2,1433;initial catalog=SQLAKORE;user id=sa;password=.ecas2010.;";
            //connectionString = "data source=" + servidor + "\\caldana2,1433;initial catalog=SQLAKORE;user id=sa;password=.ecas2010.;";
            //connectionString = "data source=50.23.209.233\\db2admin,1433; initial catalog=SQLAKORE"+
            //                    ";Integrated Security=False;user id=sa;password=.ecas2010.; ";

            //connectionString = "data source=192.168.0.5\\caldana2,1433; initial catalog=SQLAKORE" +
            //                   ";Integrated Security=False;user id=sa;password=.ecas2010.; ";
            connectionString = "data source=" + servidor + "\\" + schema + "," + port + "; initial catalog=" + database +
                                ";Integrated Security=False;user id=" + usuario + ";password=.ecas2010.; ";
            string aaa = HttpContext.Current.User.Identity.Name.ToLower();

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //SetSchema(connection);
                exceptions = "Se ha abierto una conexión con la base de datos:";
                exceptions += connection.Database + " en UDB SQL SERVER " + connection.ServerVersion + cambioLinea;
            }
            catch (Exception e)
            {
                exceptions = "Error al conectar con la base de datos especificada." + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, connectionString);
            }
            return connection;
        }

        public void CloseConnection(SqlConnection conn)
        {
            try
            {
                conn.Close();
                exceptions += "se ha cerrado la conexión" + cambioLinea;
            }
            catch (Exception e)
            {
                exceptions = "Error al cerrar la conexión con la base de datos" + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                if (connectionString == null) connectionString = "null connectionString";
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, connectionString);
            }
        }

        public void CloseConnection()
        {
            try
            {
                connection.Close();
                exceptions += "se ha cerrado la conexión" + cambioLinea;
            }
            catch (Exception e)
            {
                exceptions = "Error al cerrar la conexión con la base de datos" + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                if (connectionString == null) connectionString = "null connectionString";
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, connectionString);
            }
        }

        public bool SetSchema(SqlConnection con)
        {
            bool status = false;
            SqlCommand sql = new SqlCommand();
            sql.Connection = con;
            sql.CommandText = "SET SCHEMA " + schema;
            try
            {
                sql.ExecuteNonQuery();
                status = true;
            }
            catch (Exception e)
            {
                exceptions = "Error al ejecutar: " + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql.CommandText, e, exceptions);
                status = false;
            }

            return status;
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string sql)
        {
            SqlConnection lc = OpenConnection();
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                sql = esquemaDB(sql);

                SqlDataAdapter adapter = new SqlDataAdapter(sql, lc);

                if (isEnum == IncludeSchema.YES)
                {
                    adapter.FillSchema(ds, SchemaType.Mapped);
                    adapter.Fill(ds);
                }
                else
                    adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());
            }
            catch { }
            CloseConnection(lc);


            return ds;
        }

        public DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql)
        {
            SqlConnection lc = OpenConnection();
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                sql = esquemaDB(sql);

                SqlDataAdapter adapter = new SqlDataAdapter(sql, lc);

                if (isEnum == IncludeSchema.YES)
                {
                    adapter.FillSchema(ds, SchemaType.Mapped);
                    adapter.Fill(ds);
                }
                else
                    adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());
            }
            catch { }
            CloseConnection(lc);


            return ds;
        }

        public ArrayList RequestAsCollection(string sql)
        {
            SqlConnection lc = OpenConnection();
            ArrayList result = null;
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                sql = esquemaDB(sql);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, lc);
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
            }
            catch { }
            return result;
        }

        public ArrayList RequestGlobalAsCollection(string sql)
        {
            SqlConnection lc = OpenConnection();
            ArrayList result = null;
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                sql = esquemaDB(sql);
                SqlDataAdapter adapter = new SqlDataAdapter(sql, lc);
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
            }
            catch { }
            return result;
        }

        public DataSet Request(DataSet ds, IncludeSchema isEnum, string nombreProcedimiento, IDictionaryEnumerator parametros)
        {
            SqlConnection lc = OpenConnection();
            try
            {
                SqlCommand comm = new SqlCommand(nombreProcedimiento, lc);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                if (parametros != null)
                {
                    while (parametros.MoveNext())
                        comm.Parameters.Add(parametros.Key.ToString(), parametros.Value);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                if (isEnum == IncludeSchema.YES)
                {
                    adapter.FillSchema(ds, SchemaType.Mapped);
                    adapter.Fill(ds);
                }
                else
                    adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());
            }
            catch (Exception e)
            {
                exceptions = "Error ejecutando SQL." + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, nombreProcedimiento, e, exceptions);
            }
            CloseConnection(lc);
            return ds;
        }

        public DataSet Insert(DataSet ds, int index)
        {
            int i = 0;
            string table = "Table";
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

            SqlConnection dc = OpenConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(sqlSelect, dc);
            Sql = "INSERT INTO  " + table + " " + fieldsString + " VALUES " + valuesString + "";
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, Sql, null, string.Empty);
            SqlCommand insertCommand = new SqlCommand(Sql, adapter.SelectCommand.Connection);

            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                if (ds.Tables[index].Columns[i].DataType.ToString() == "System.String")
                    insertCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (SqlDbType)typesRelation[ds.Tables[index].Columns[i].DataType], ds.Tables[index].Columns[i].MaxLength, ds.Tables[index].Columns[i].ColumnName);
                else
                    insertCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (SqlDbType)typesRelation[ds.Tables[index].Columns[i].DataType], (int)lengthsRelation[(SqlDbType)typesRelation[ds.Tables[index].Columns[i].DataType]], ds.Tables[index].Columns[i].ColumnName);
            }

            adapter.InsertCommand = insertCommand;

            insertStatus = false;
            try
            {
                affectedRows = adapter.Update(ds.Tables[index]);
                insertStatus = true;

                HistorialSeguimientoTabla(TABLA_SEGUIR, sqlSelect);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + "INSERT INTO  " + table + " " + fieldsString + " VALUES " + valuesString + "" + cambioLinea;
                exceptions += "parameters: " + insertCommand.Parameters.Count.ToString() + cambioLinea;
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, Sql, e, exceptions);
            }
            CloseConnection(dc);

            return ds;
        }

        public DataSet Delete(DataSet ds, int index)
        {
            int i = 0;
            string table = "Table";
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

            SqlConnection dc = OpenConnection();

            SqlDataAdapter adapter = new SqlDataAdapter(sqlSelect, dc);
            Sql = "DELETE FROM " + table + " " + whereString + "";
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, Sql, null, String.Empty);
            SqlCommand deleteCommand = new SqlCommand(Sql, adapter.SelectCommand.Connection);

            for (i = 0; i < ds.Tables[0].PrimaryKey.Length; i++)
            {
                if (ds.Tables[0].PrimaryKey[i].DataType.ToString() == "System.String")
                    deleteCommand.Parameters.Add("@" + ds.Tables[0].PrimaryKey[i] + "", (SqlDbType)typesRelation[ds.Tables[0].PrimaryKey[i].DataType], ds.Tables[0].PrimaryKey[i].MaxLength, ds.Tables[0].PrimaryKey[i].ColumnName);
                else
                    deleteCommand.Parameters.Add("@" + ds.Tables[0].PrimaryKey[i] + "", (SqlDbType)typesRelation[ds.Tables[0].PrimaryKey[i].DataType], (int)lengthsRelation[(SqlDbType)typesRelation[ds.Tables[0].PrimaryKey[i].DataType]], ds.Tables[0].PrimaryKey[i].ColumnName);
            }

            adapter.DeleteCommand = deleteCommand;

            deleteStatus = false;
            try
            {
                affectedRows = adapter.Update(ds.Tables[index]);
                deleteStatus = true;

                HistorialSeguimientoTabla(TABLA_SEGUIR, sqlSelect);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + cambioLinea;
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, Sql, e, exceptions);
            }
            CloseConnection(dc);

            return ds;
        }

        public DataSet Update(DataSet ds, int index)
        {
            int i = 0;
            string table = "Table";
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

            SqlConnection dc = OpenConnection();

            SqlDataAdapter adapter = new SqlDataAdapter(sqlSelect, dc);
            SqlCommand updateCommand = new SqlCommand("UPDATE " + table + fieldsString + whereString, adapter.SelectCommand.Connection);

            exceptions += "UPDATE " + table + fieldsString + whereString + cambioLinea;
            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                if (ds.Tables[index].Columns[i].DataType.ToString() == "System.String")
                    updateCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (SqlDbType)typesRelation[ds.Tables[index].Columns[i].DataType], ds.Tables[index].Columns[i].MaxLength, ds.Tables[index].Columns[i].ColumnName);
                else
                    updateCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (SqlDbType)typesRelation[ds.Tables[index].Columns[i].DataType], (int)lengthsRelation[(SqlDbType)typesRelation[ds.Tables[index].Columns[i].DataType]], ds.Tables[index].Columns[i].ColumnName);
            }

            for (i = 0; i < ds.Tables[index].PrimaryKey.Length; i++)
            {
                if (ds.Tables[index].PrimaryKey[i].DataType.ToString() == "System.String")
                    parm = updateCommand.Parameters.Add("@old" + ds.Tables[index].PrimaryKey[i] + "", (SqlDbType)typesRelation[ds.Tables[index].PrimaryKey[i].DataType], ds.Tables[index].PrimaryKey[i].MaxLength, ds.Tables[index].PrimaryKey[i].ColumnName);
                else
                    parm = updateCommand.Parameters.Add("@old" + ds.Tables[index].PrimaryKey[i] + "", (SqlDbType)typesRelation[ds.Tables[index].PrimaryKey[i].DataType], (int)lengthsRelation[(SqlDbType)typesRelation[ds.Tables[index].PrimaryKey[i].DataType]], ds.Tables[index].PrimaryKey[i].ColumnName);

                parm.SourceVersion = DataRowVersion.Original;
            }

            exceptions += updateCommand.Parameters.Count.ToString() + " Parameters" + cambioLinea;
            adapter.UpdateCommand = updateCommand;

            updateStatus = false;
            try
            {
                affectedRows = adapter.Update(ds.Tables[index]);
                updateStatus = true;

                HistorialSeguimientoTabla(TABLA_SEGUIR, sqlSelect);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + cambioLinea;
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, Sql, e, exceptions);
            }
            CloseConnection(dc);

            return ds;
        }

        public int NonQuery(string sql)
        {
            SqlConnection ncc = OpenConnection();
            sql = esquemaDB(sql);
            SqlCommand com = new SqlCommand(sql, ncc);
            try
            {
                affectedRows = com.ExecuteNonQuery();

                HistorialSeguimientoTabla(TABLA_SEGUIR, sql);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                //AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, e.Message);
                //exceptions += e.Message + cambioLinea;
                affectedRows = 0;
            }
            finally
            {
                CloseConnection(ncc);
            }
            return affectedRows;
        }

        public int NonQueryGlobal(string sql, string dataBase)
        {
            SqlConnection ncc = OpenConnection();
            sql = esquemaDB(sql);
            SqlCommand com = new SqlCommand(sql, ncc);
            try
            {
                affectedRows = com.ExecuteNonQuery();

                HistorialSeguimientoTabla(TABLA_SEGUIR, sql);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                //AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, e.Message);
                //exceptions += e.Message + cambioLinea;
                affectedRows = 0;
            }
            finally
            {
                CloseConnection(ncc);
            }
            return affectedRows;
        }

        public int NonQuery(string sql, byte[] blob)
        {
            SqlConnection ncc = OpenConnection();
            sql = esquemaDB(sql);
            SqlCommand com = new SqlCommand(sql, ncc);

            try
            {
                DB2Parameter parm1 = new DB2Parameter();
                parm1.DbType = DbType.Binary;
                parm1.ParameterName = "@blob";
                parm1.Value = (byte[])blob;
                com.Parameters.Add(parm1);

                affectedRows = com.ExecuteNonQuery();

                HistorialSeguimientoTabla(TABLA_SEGUIR, sql);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                //AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, e.Message);
                //exceptions += e.Message + cambioLinea;
                affectedRows = 0;
            }
            finally
            {
                CloseConnection(ncc);
            }
            return affectedRows;
        }

        public SqlDataReader DataReader(string sql)
        {
            SqlCommand oc = new SqlCommand();
            try
            {
                sql = esquemaDB(sql);
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                oc.CommandText = sql;
                SqlConnection con = OpenConnection();
                connection = con;
                oc.Connection = con;
                SqlDataReader dr = oc.ExecuteReader();
                CloseConnection(con);

                return dr;
            }
            catch (Exception e)
            {
                exceptions = "Error ejecutando SQL." + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, exceptions);
                return null;
            }
        }

        public string SingleData(string sql)
        {
            string val = "";
            SqlCommand command = new SqlCommand();
            SqlConnection con = OpenConnection();
            command.Connection = con;
            sql = esquemaDB(sql);
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;
            try
            {
                val = Convert.ToString(command.ExecuteScalar());
            }
            catch (Exception e) { }

            CloseConnection(con);
            return val;
        }

        public string SingleDataGlobal(string sql)
        {
            string val = "";
            SqlCommand command = new SqlCommand();
            SqlConnection con = OpenConnection();
            command.Connection = con;
            sql = esquemaDB(sql);
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;
            try
            {
                val = Convert.ToString(command.ExecuteScalar());
            }
            catch { }

            CloseConnection(con);
            return val;
        }

        public bool RecordExist(string sql)
        {
            bool exist = false;
            SqlCommand command = new SqlCommand();
            SqlConnection con = OpenConnection();
            command.Connection = con;
            sql = esquemaDB(sql);
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;
            try
            {
                SqlDataReader db2dr = command.ExecuteReader();
                exist = db2dr.Read();
                db2dr.Close(); // <Observacion>
            }
            catch (Exception e)
            {
                exceptions += e.ToString() + cambioLinea;
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, exceptions);
            }
            CloseConnection(con);
            return exist;
        }

        public bool Transaction(ArrayList sql)
        {
            int numQueries = 0, i = 0;
            bool status = false;

            SqlCommand command = new SqlCommand();
            SqlConnection con = OpenConnection();
            command.Connection = con;

            SqlTransaction trans = con.BeginTransaction();
            command.Transaction = trans;

            for (i = 0; i < sql.Count; i++)
            {
                sql[i] = esquemaDB(sql[i].ToString());
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, (string)sql[i], null, string.Empty);
                command.CommandText = sql[i].ToString();
                try
                {
                    command.ExecuteNonQuery();
                    exceptions += "ejecutando: " + sql[i] + cambioLinea;
                    numQueries++;

                    HistorialSeguimientoTabla(TABLA_SEGUIR, sql[i].ToString());  //Almacenamiento historial de seguimiento a tabla.
                }
                catch (Exception ex)
                {
                    exceptions += String.Format("error ejecutando: {0} \n {1} \n", sql[i], ex.Message);
                    status = false;
                }
            }

            if (numQueries == sql.Count)
            {
                try
                {
                    trans.Commit();
                    exceptions += "ejecutando Commit -- " + cambioLinea;
                    status = true;
                }
                catch (Exception e)
                {
                    exceptions += "Error ejecutando Commit: " + e.ToString();
                    AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, exceptions);
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
                    AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, exceptions);
                }
                numQueries = 0;
            }

            CloseConnection(con);

            return status;
        }

        private void HistorialSeguimientoTabla(String tablaSeguir, String sqlRevision)
        {
            sqlRevision = sqlRevision.Replace("'", "");
            sqlRevision = sqlRevision.ToUpper();
            tablaSeguir = tablaSeguir.ToUpper();

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);
            sqlRevision = regex.Replace(sqlRevision, @" ");

            if ((sqlRevision.Contains("UPDATE " + tablaSeguir) == true || sqlRevision.Contains("INSERT INTO " + tablaSeguir) == true || sqlRevision.Contains("DELETE FROM " + tablaSeguir) == true)
                && sqlRevision.Contains("MHISTORIAL_CAMBIOS") == false)
            {
                string usuario = HttpContext.Current.User.Identity.Name.ToLower();
                string operacion = "";

                if (sqlRevision.Contains("UPDATE"))
                {
                    operacion = "U";
                }
                else if (sqlRevision.Contains("DELETE"))
                {
                    operacion = "D";
                }
                else if (sqlRevision.Contains("INSERT"))
                {
                    operacion = "I";
                }

                SqlCommand command = new SqlCommand();
                SqlConnection con = OpenConnection();
                command.Connection = con;

                SqlTransaction trans = con.BeginTransaction();
                command.Transaction = trans;

                string sqlHistorial = "INSERT INTO MHISTORIAL_CAMBIOS VALUES (DEFAULT,'" + tablaSeguir + "','" + operacion + "','" + sqlRevision + "','" + usuario + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');";

                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sqlHistorial, null, string.Empty);
                command.CommandText = sqlHistorial;
                try
                {
                    command.ExecuteNonQuery();
                    trans.Commit();
                    exceptions += "ejecutando: " + tablaSeguir + cambioLinea;
                }
                catch (Exception ex)
                {
                    exceptions += String.Format("error ejecutando: {0} \n {1} \n", tablaSeguir, ex.Message);
                }
            }
        }

        public bool SaveHashtable(String tableName, Hashtable hash)
        {
            SqlConnection ncc = OpenConnection();
            SqlCommand com = null;
            String sql = "INSERT INTO {0} ({1}) VALUES ({2});";
            int affectedRows = 0;

            try
            {
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

                com = new SqlCommand(sql, ncc);
                affectedRows = com.ExecuteNonQuery();

                QueryCache.removeData(tableName); //limpiando caché de la tabla...

                HistorialSeguimientoTabla(TABLA_SEGUIR, sql);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, e.Message);
                //exceptions += e.ToString() + cambioLinea;
            }
            finally
            {
                CloseConnection(ncc);
            }

            return affectedRows == 1;
        }

        public int UpdateHashtable(String tableName, Hashtable hashData, Hashtable hashPk)
        {
            SqlConnection ncc = OpenConnection();
            SqlCommand com = null;
            String sql = "UPDATE {0} SET {1} WHERE {2};";
            int affectedRows = 0;

            try
            {
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

                com = new SqlCommand(sql, ncc);
                affectedRows = com.ExecuteNonQuery();

                QueryCache.removeData(tableName); //limpiando caché de la tabla...

                HistorialSeguimientoTabla(TABLA_SEGUIR, sql);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, e.Message);
                return -1;
            }
            finally
            {
                CloseConnection(ncc);
            }

            return affectedRows;
        }

        public int DeleteHashtable(string tableName, Hashtable hashPk)
        {
            SqlConnection ncc = OpenConnection();
            SqlCommand com = null;
            String sql = "DELETE FROM {0} WHERE {1};";
            int affectedRows = 0;

            try
            {
                string where = "";
                foreach (string key in hashPk.Keys)
                {
                    string col = key;
                    string value = hashPk[key].ToString();

                    if (!where.Equals("")) where += " AND ";

                    int n = 0;
                    if (int.TryParse(value, out n))
                        where += String.Format("{0} = {1}", col, value);
                    else
                        where += String.Format("{0} = '{1}'", col, value);
                }

                sql = string.Format(sql, tableName, where);
                com = new SqlCommand(sql, ncc);
                affectedRows = com.ExecuteNonQuery();

                QueryCache.removeData(tableName); //limpiando caché de la tabla...

                HistorialSeguimientoTabla(TABLA_SEGUIR, sql);  //Almacenamiento historial de seguimiento a tabla.
            }
            catch (Exception e)
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, sql, e, e.Message);
                return -1;
            }
            finally
            {
                CloseConnection(ncc);
            }

            return affectedRows;
        }

        //Traductor de sentencias DB2 -> SQL SERVER
        public string esquemaDB(string sql)
        {
            sql = sql.ToUpper();

            if(sql.Contains("PAG_NUM_DB_MNGR"))
            {
                sql = sql.Replace("COALESCE(", "COALESCE( CONCAT( ");
                sql = sql.Replace(", ' ')", "), ' ')");
                sql = sql.ToUpper().Replace(" CONCAT ", " , ").Replace("||", " , ");
            }
            else if(sql.Contains(" CONCAT ") && sql.Contains("COALESCE"))
            {
                sql = sql.Replace(" CONCAT ", " + ");
            }
            else if(sql.Contains(" CONCAT ") || sql.Contains("||"))
                sql = ConcatenadorSQL(sql);

            sql = sql.ToUpper().Replace("DBXSCHEMA.", "")  //.Replace(" CONCAT ", " + ").Replace("||", " + ")
                .Replace(" TRIM(", " RTRIM(").Replace("SUBSTR(", "SUBSTRING(").Replace("UCASE(", "UPPER(")
                .Replace("CURRENT DATE","GETDATE()").Replace("DAYS","DAY");

            if (sql.Contains("ROWNUMBER()"))
            {
                sql = sql.Replace("ROWNUMBER()", "ROW_NUMBER()").Replace("OVER()", "OVER(ORDER BY (SELECT 0))")
                .Replace("(SELECT ", "(SELECT TOP(10000000) + ").Replace("( SELECT ", "( SELECT TOP(10000000) + ");
            }

            if (sql.Contains("FETCH FIRST 50"))
            {
                sql = sql.Replace("FETCH FIRST 50 ROWS ONLY", "");
                sql = sql.Replace("SELECT * FROM(", "SELECT TOP(50) * FROM(");
            }

            if (sql.Contains("FETCH FIRST 1"))
            {
                sql = sql.Replace("FETCH FIRST 1 ROWS ONLY", "");
                sql = sql.Replace("SELECT", "SELECT TOP(1)");
            }

            if (sql.Contains("INSERT INTO"))
            {
                sql = sql.Replace("DEFAULT,", "");
            }

            return sql;
        }

        protected string ConcatenadorSQL(string sql)
        {
            string sqlConcat = "";
            sql = sql.Replace(" || ", " CONCAT ");
            string[] arrSql = Regex.Split(sql, "CONCAT");
            
            bool concatenando = false;
            for (int k = 0; k < arrSql.Length; k++)
            {
                arrSql[k] = arrSql[k].Trim();
                if (concatenando == false)
                {
                    //terminar llenado con + luego de la primera pasada, concats en where de selects anidados.
                    if(k > 0)
                    {
                        sqlConcat += " + ";
                        sqlConcat += arrSql[k];
                    }
                    else
                    { 
                        //Ubicar el campo inicial para poner la funcion concat
                        int inicioConcat = arrSql[k].LastIndexOf(",");
                        if (inicioConcat != -1)
                        {
                            sqlConcat += arrSql[k].Insert(inicioConcat + 1, " CONCAT(");
                        }
                        else
                        {
                            inicioConcat = arrSql[k].LastIndexOf("SELECT ");
                            sqlConcat += arrSql[k].Insert(inicioConcat + 7, " CONCAT(");
                        }
                        sqlConcat += ",";
                        concatenando = true;
                    }
                }
                else
                {
                    //Determinar si el siguiente campo cierra o no la funcion concat
                    int finConcatFROM = arrSql[k].IndexOf(" FROM ");
                    if (finConcatFROM != -1)  //FROM
                    {
                        int finConcatCOMA = arrSql[k].IndexOf(",");
                        if (finConcatCOMA != -1 && finConcatCOMA < finConcatFROM)  //COMA
                        {
                            int finConcatAS = arrSql[k].IndexOf(" AS ");
                            if (finConcatAS != -1 && finConcatAS < finConcatCOMA)
                            {
                                sqlConcat += arrSql[k].Insert(finConcatAS, ") "); // )as
                            }
                            else
                            {
                                int finConcatEspacio = arrSql[k].IndexOf(" ");
                                if (finConcatEspacio != -1 && finConcatEspacio < finConcatCOMA)
                                {
                                    sqlConcat += arrSql[k].Insert(finConcatEspacio, ") "); // )_
                                }
                                else
                                {
                                    sqlConcat += arrSql[k].Insert(finConcatCOMA, ") "); // ),
                                }
                            }
                        }
                        else  //No COMA
                        {
                            int finConcatAS = arrSql[k].IndexOf(" AS ");
                            if (finConcatAS != -1 && finConcatAS < finConcatFROM)
                            {
                                sqlConcat += arrSql[k].Insert(finConcatAS, ") "); // )as
                            }
                            else
                            {
                                int finConcatEspacio = arrSql[k].IndexOf(" ");
                                if (finConcatEspacio != -1 && finConcatEspacio < finConcatFROM)
                                {
                                    sqlConcat += arrSql[k].Insert(finConcatEspacio, ") "); // )_
                                }
                                else
                                {
                                    sqlConcat += arrSql[k].Insert(finConcatFROM, ") "); // )from
                                }
                            }
                                
                        }
                        concatenando = false;
                    } //No FROM
                    else
                    {
                        int finConcatCOMA = arrSql[k].IndexOf(",");
                        if (finConcatCOMA != -1)
                        {
                            int finConcatAS = arrSql[k].IndexOf(" AS ");
                            if (finConcatAS != -1 && finConcatAS < finConcatCOMA)
                            {
                                sqlConcat += arrSql[k].Insert(finConcatAS, ")"); // )as
                            }
                            else
                            {
                                int finConcatEspacio = arrSql[k].IndexOf(" ");
                                if (finConcatEspacio != -1 && finConcatEspacio < finConcatCOMA)
                                {
                                    sqlConcat += arrSql[k].Insert(finConcatEspacio, ") "); // )_
                                }
                                else
                                {
                                    sqlConcat += arrSql[k].Insert(finConcatCOMA, ") "); // ),
                                }
                            }
                            concatenando = false;
                        }
                        else
                        {
                            sqlConcat += arrSql[k];
                            sqlConcat += ",";
                            concatenando = true;
                        }
                    }
                }
            }

            return sqlConcat;
        }

        public void SetUserSchema(SqlCommand command)
        {
            try
            {
                command.CommandText = "execute as user = '" + schema + "';";
                command.ExecuteNonQuery();
            }
            catch { }
        }
    }
}
