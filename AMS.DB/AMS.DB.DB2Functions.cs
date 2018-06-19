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

namespace AMS.DB
{
    public class DB2Functions : IDBFunctions
    {
        protected const string cambioLinea = "<br>";
        protected int affectedRows = 0;
        protected bool insertStatus, deleteStatus, updateStatus;
        protected string connectionString;// = ConfigurationManager.AppSettings["ConnectionString"];
        protected string schema=ConfigurationManager.AppSettings["Schema"];
        protected string sqlSelect;
        protected string exceptions;
        protected Hashtable typesRelation = new Hashtable();
        protected Hashtable lengthsRelation = new Hashtable();
        protected DB2Connection connection;
        protected DB2Parameter parm;
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
        public DB2Connection Connection { get { return connection; } }

        public DB2Functions()
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

        public DB2Connection OpenConnection()
        {
            string servidor;
            string database;
            string usuario;
            string password;

            //para 2015
            
            servidor = ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
            database = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
            usuario = ConfigurationManager.AppSettings["UID"];
            password = ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];

            string timeout=ConfigurationManager.AppSettings["ConnectionTimeout"];
            string port = ConfigurationManager.AppSettings["DataBasePort"];
            AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd=miCripto.DescifrarCadena(password);

            connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd + ";QueryTimeout=3600";
            if (timeout != null) connectionString += ";Connection Timeout=" + timeout;

            string aaa = HttpContext.Current.User.Identity.Name.ToLower();

            DB2Connection connection = new DB2Connection(connectionString);
            try
            {
                connection.Open();
                SetSchema(connection);
                exceptions = "Se ha abierto una conexión con la base de datos:";
                exceptions += connection.Database + " en UDB DB2 " + connection.ServerVersion + cambioLinea;
            }
            catch (Exception e)
            {
                exceptions = "Error al conectar con la base de datos especificada." + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, connectionString);
            }
            return connection;
        }

        public DB2Connection OpenConnectionGlobal()
        {
            //if (connection != null && connection.IsOpen) return connection;
            //string algo = HttpContext.Current.Request.Url.AbsolutePath;
            //string algo2 = HttpContext.Current.Request.Url.AbsoluteUri;
            //string host = HttpContext.Current.Request.Url.Host;

            string servidor = ConfigurationManager.AppSettings["ServerGlobal"];
            string database = ConfigurationManager.AppSettings["DataBaseGlobal"];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string password = ConfigurationManager.AppSettings["PWDGlobal"];
            string timeout = ConfigurationManager.AppSettings["ConnectionTimeout"];
            string port = ConfigurationManager.AppSettings["DataBasePort"];
            AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd = miCripto.DescifrarCadena(password);

            connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd + ";QueryTimeout=3600";
            if (timeout != null) connectionString += ";Connection Timeout=" + timeout;

            //string aaa = HttpContext.Current.User.Identity.Name.ToLower();

            DB2Connection connection = new DB2Connection(connectionString);
            try
            {
                connection.Open();
                SetSchema(connection);
                exceptions = "Se ha abierto una conexión con la base de datos:";
                exceptions += connection.Database + " en UDB DB2 " + connection.ServerVersion + cambioLinea;
            }
            catch (Exception e)
            {
                exceptions = "Error al conectar con la base de datos especificada." + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, connectionString);
            }
            return connection;
        }

        public DB2Connection OpenConnectionGlobal(string dataBase)
        {
            string servidor = ConfigurationManager.AppSettings["Server" + dataBase];
            string database = ConfigurationManager.AppSettings["DataBase" + dataBase];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string password = ConfigurationManager.AppSettings["PWD" + dataBase];

            string timeout = ConfigurationManager.AppSettings["ConnectionTimeout"];
            string port = ConfigurationManager.AppSettings["DataBasePort"];
            AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd = miCripto.DescifrarCadena(password);

            connectionString = "Server=" + servidor + ":" + port + ";DataBase=" + database + ";UID=" + usuario + ";PWD=" + newPwd + ";QueryTimeout=3600";
            if (timeout != null) connectionString += ";Connection Timeout=" + timeout;
            
            DB2Connection connection = new DB2Connection(connectionString);
            try
            {
                connection.Open();
                SetSchema(connection);
                exceptions = "Se ha abierto una conexión con la base de datos:";
                exceptions += connection.Database + " en UDB DB2 " + connection.ServerVersion + cambioLinea;
            }
            catch (Exception e)
            {
                exceptions = "Error al conectar con la base de datos especificada." + cambioLinea + cambioLinea;
                exceptions += e.ToString();
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Error, string.Empty, e, connectionString);
            }
            return connection;
        }

        public void CloseConnection(DB2Connection conn)
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

        public bool SetSchema(DB2Connection con)
        {
            bool status = false;
            DB2Command sql = new DB2Command();
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
            DB2Connection lc = OpenConnection();
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                DB2DataAdapter adapter = new DB2DataAdapter(sql, lc);

                if (isEnum == IncludeSchema.YES)
                {
                    adapter.FillSchema(ds, SchemaType.Mapped);
                    adapter.Fill(ds);
                }
                else
                    adapter.Fill(ds, "result_ " + ds.Tables.Count.ToString());
            }
            catch (Exception z){ this.exceptions = z.Message; }
            CloseConnection(lc);

            return ds;
        }

        public DataSet RequestGlobal(DataSet ds, IncludeSchema isEnum, string dataBase, string sql)
        {
            DB2Connection lc;
            if (dataBase == "")
                lc = OpenConnectionGlobal();
            else
                lc = OpenConnectionGlobal(dataBase);

            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                DB2DataAdapter adapter = new DB2DataAdapter(sql, lc);

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
            DB2Connection lc = OpenConnection();
            ArrayList result = null;
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                DB2DataAdapter adapter = new DB2DataAdapter(sql, lc);
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
            DB2Connection lc = OpenConnectionGlobal();
            ArrayList result = null;
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                DB2DataAdapter adapter = new DB2DataAdapter(sql, lc);
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
            DB2Connection lc = OpenConnection();
            try
            {
                DB2Command comm = new DB2Command(nombreProcedimiento, lc);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                if (parametros != null)
                {
                    while (parametros.MoveNext())
                        comm.Parameters.Add(parametros.Key.ToString(), parametros.Value);
                }
                DB2DataAdapter adapter = new DB2DataAdapter(comm);
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

            DB2Connection dc = OpenConnection();
            DB2DataAdapter adapter = new DB2DataAdapter(sqlSelect, dc);
            Sql = "INSERT INTO  " + table + " " + fieldsString + " VALUES " + valuesString + "";
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, Sql, null, string.Empty);
            DB2Command insertCommand = new DB2Command(Sql, adapter.SelectCommand.Connection);

            //exceptions += ds.Tables[0].Columns[0].DataType.ToString() + ":::" + cambioLinea;
            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                if (ds.Tables[index].Columns[i].DataType.ToString() == "System.String")
                    insertCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (DB2Type)typesRelation[ds.Tables[index].Columns[i].DataType], ds.Tables[index].Columns[i].MaxLength, ds.Tables[index].Columns[i].ColumnName);
                else
                    insertCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (DB2Type)typesRelation[ds.Tables[index].Columns[i].DataType], (int)lengthsRelation[(DB2Type)typesRelation[ds.Tables[index].Columns[i].DataType]], ds.Tables[index].Columns[i].ColumnName);
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

            DB2Connection dc = OpenConnection();

            DB2DataAdapter adapter = new DB2DataAdapter(sqlSelect, dc);
            Sql = "DELETE FROM " + table + " " + whereString + "";
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, Sql, null, String.Empty);
            DB2Command deleteCommand = new DB2Command(Sql, adapter.SelectCommand.Connection);

            for (i = 0; i < ds.Tables[0].PrimaryKey.Length; i++)
            {
                if (ds.Tables[0].PrimaryKey[i].DataType.ToString() == "System.String")
                    deleteCommand.Parameters.Add("@" + ds.Tables[0].PrimaryKey[i] + "", (DB2Type)typesRelation[ds.Tables[0].PrimaryKey[i].DataType], ds.Tables[0].PrimaryKey[i].MaxLength, ds.Tables[0].PrimaryKey[i].ColumnName);
                else
                    deleteCommand.Parameters.Add("@" + ds.Tables[0].PrimaryKey[i] + "", (DB2Type)typesRelation[ds.Tables[0].PrimaryKey[i].DataType], (int)lengthsRelation[(DB2Type)typesRelation[ds.Tables[0].PrimaryKey[i].DataType]], ds.Tables[0].PrimaryKey[i].ColumnName);
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

            DB2Connection dc = OpenConnection();

            DB2DataAdapter adapter = new DB2DataAdapter(sqlSelect, dc);
            DB2Command updateCommand = new DB2Command("UPDATE " + table + fieldsString + whereString, adapter.SelectCommand.Connection);

            exceptions += "UPDATE " + table + fieldsString + whereString + cambioLinea;
            for (i = 0; i < ds.Tables[index].Columns.Count; i++)
            {
                if (ds.Tables[index].Columns[i].DataType.ToString() == "System.String")
                    updateCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (DB2Type)typesRelation[ds.Tables[index].Columns[i].DataType], ds.Tables[index].Columns[i].MaxLength, ds.Tables[index].Columns[i].ColumnName);
                else
                    updateCommand.Parameters.Add("@" + ds.Tables[index].Columns[i] + "", (DB2Type)typesRelation[ds.Tables[index].Columns[i].DataType], (int)lengthsRelation[(DB2Type)typesRelation[ds.Tables[index].Columns[i].DataType]], ds.Tables[index].Columns[i].ColumnName);
            }

            for (i = 0; i < ds.Tables[index].PrimaryKey.Length; i++)
            {
                if (ds.Tables[index].PrimaryKey[i].DataType.ToString() == "System.String")
                    parm = updateCommand.Parameters.Add("@old" + ds.Tables[index].PrimaryKey[i] + "", (DB2Type)typesRelation[ds.Tables[index].PrimaryKey[i].DataType], ds.Tables[index].PrimaryKey[i].MaxLength, ds.Tables[index].PrimaryKey[i].ColumnName);
                else
                    parm = updateCommand.Parameters.Add("@old" + ds.Tables[index].PrimaryKey[i] + "", (DB2Type)typesRelation[ds.Tables[index].PrimaryKey[i].DataType], (int)lengthsRelation[(DB2Type)typesRelation[ds.Tables[index].PrimaryKey[i].DataType]], ds.Tables[index].PrimaryKey[i].ColumnName);

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
            DB2Connection ncc = OpenConnection();
            DB2Command com = new DB2Command(sql, ncc);
            try
            {
                affectedRows = com.ExecuteNonQuery();
//              string cod = @"BEGIN
//                            declare sqlcode     INTEGER;
//                            declare ANO INTEGER;
//                            declare MES SMALLINT;
//                            DECLARE ANO_AUX     INTEGER;
//                            DECLARE MES_AUX     smallint;
//                            DECLARE CUENTA_AUX  VARCHAR(16);
//                            DECLARE TNAT_CODIGO_AUX  CHARACTER(1);
//                            DECLARE SALDO_debi_AUX  DECIMAL(14, 2);
//                            declare SALDO_CRED_AUX  DECIMAL(14, 2);
//                            DECLARE ANIO_inicial    INTEGER;
//                            DECLARE mes_inicial     smallint;
//                            declare dia_inicial     integer;
//                            DECLARE ANIO_FINAL      INTEGER;
//                            DECLARE mes_final       smallint;
//                            DECLARE CUENTA_PUC          VARCHAR(16);
//                            DECLARE SQLCAD VARCHAR(3000);
//                            DECLARE WORDS varchar(3000);
//                            set ANO = 2016;
//                            set MES = 12; 

//                            EXECUTE IMMEDIATE 'DECLARE GLOBAL TEMPORARY TABLE SESSION.LIBRONETO1
//                            (PANO_ANO       INTEGER     ,
//                            MCUE_CODIPUC   VARCHAR(16) ,
//                            TNAT_CODIGO    CHARACTER(1),
//                            DEBITO_INIC    DECIMAL(14, 2), 
//                            CREDITO_INIC   DECIMAL(14, 2),
//                            DEBITO_MES     DECIMAL(14, 2) ,
//                            CREDITO_MES    DECIMAL(14, 2),
//                            DEBITO_FINAL   DECIMAL(14, 2), 
//                            CREDITO_FINAL  DECIMAL(14, 2)) ON COmMIT PRESERVE ROWS NOT LOGGED WITH REPLACE';

//                            EXECUTE IMMEDIATE 'INSERT INTO SESSION.LIBRONETO1
//                            SELECT  MS.PANO_ANO AS PANO_ANO,MS.MCUE_CODIPUC AS MCUE_CODIPUC,MC.TNAT_CODIGO AS TNAT_CODIGO,
//                            SUM(MSAL_VALODEBI) ,SUM(MSAL_VALOCRED)  ,0 AS DEBITO_MES,
//                            0 AS CREDITO_MES,SUM(MSAL_VALODEBI) AS DEBITO_FINAL,SUM(MSAL_VALOCRED) AS CREDITO_FINAL
//                            FROM DBXSCHEMA.MSALDOCUENTA MS ,DBXSCHEMA.MCUENTA MC,DBXSCHEMA.CCONTABILIDAD CC
//                            where MS.PANO_ANO = ' || ANO ||  '
//                            AND MS.PMES_MES < ' || MES || '
//                            AND MS.MCUE_CODIPUC = MC.MCUE_CODIPUC
//                            AND CC.MCUE_CODIPUC <> MS.MCUE_CODIPUC
//                            GROUP BY  MS.PANO_ANO,MS.MCUE_CODIPUC, MC.TNAT_CODIGO'; 

//                            EXECUTE IMMEDIATE 'INSERT INTO SESSION.LIBRONETO1 
//                            SELECT  MS.PANO_ANO AS PANO_ANO,MS.MCUE_CODIPUC AS MCUE_CODIPUC,MC.TNAT_CODIGO AS TNAT_CODIGO,
//                            0,0,SUM(MS.MSAL_VALODEBI) AS DEBITO_MES ,SUM(MS.MSAL_VALOCRED) AS CREDITO_MES,SUM(MS.MSAL_VALODEBI),
//                            SUM(MS.MSAL_VALOCRED) 
//                            FROM DBXSCHEMA.MSALDOCUENTA MS ,DBXSCHEMA.MCUENTA MC,DBXSCHEMA.CCONTABILIDAD CC
//                            where MS.PANO_ANO = ' || ANO ||  ' 
//                            AND MS.PMES_MES = ' || MES || '
//                            and ms.mcue_codipuc = mc.mcue_codipuc
//                            AND CC.MCUE_CODIPUC <> MS.MCUE_CODIPUC
//                            GROUP BY  MS.PANO_ANO,MS.MCUE_CODIPUC, MC.TNAT_CODIGO'; 
                     
//  set WORDS = 'BEGIN DECLARE C_MAUXILIAR cursor with return to client with hold for 
//                            select PANO_ANO, sum(DEBITO_INIC) as DEBITO_INIC, 
//                            SUM(CREDITO_INIC) as CREDITO_INIC, SUM(DEBITO_MES) as  DEBITO_MES,
//                            SUM(CREDITO_MES)  as CREDITO_MES,  SUM(DEBITO_FINAL) as  DEBITO_FINAL,
//                            SUM(CREDITO_FINAL) as  CREDITO_FINAL,VC.CLSE_CNTA, VC.NMBRE_NVEL1,
//                            VC.GRPO_CNTA,VC.NMBRE_NVEL2,VC.NMRO_CNTA,VC.NMBRE_NVEL3,VC.NMRO_SBCNTA,
//                            VC.NMBRE_NVEL4,VC.NMRO_AUXILIAR,VC.NOMBRE_NVEL5,VC.IMPUTABLE5,VC.MCUE_CODIPUC,
//                            VC.MCUE_NOMBRE,VC.TNIV_CODIGO,VC.TIMP_CODIGO,VC.TCIE_CODIGO,
//                            VC.TCUE_CODIGO,VC.TBAS_CODIGO,VC.TVIG_CODIGO,VC.TNAT_CODIGO,
//                            VC.TCLA_CODIGO
//                            from SESSION.LIBRONETO1 PD,DBXSCHEMA.VCONTABILIDAD_BASECUENTA VC
//                            WHERE (PD.MCUE_CODIPUC = VC.MCUE_CODIPUC)
//                            GROUP BY PANO_ANO,VC.CLSE_CNTA, VC.NMBRE_NVEL1,
//                            VC.GRPO_CNTA,VC.NMBRE_NVEL2,VC.NMRO_CNTA,VC.NMBRE_NVEL3,VC.NMRO_SBCNTA,
//                            VC.NMBRE_NVEL4,VC.NMRO_AUXILIAR,VC.NOMBRE_NVEL5,VC.IMPUTABLE5,VC.MCUE_CODIPUC,
//                            VC.MCUE_NOMBRE,VC.TNIV_CODIGO,VC.TIMP_CODIGO,VC.TCIE_CODIGO,
//                            VC.TCUE_CODIGO,VC.TBAS_CODIGO,VC.TVIG_CODIGO,VC.TNAT_CODIGO,
//                            VC.TCLA_CODIGO;   OPEN C_MAUXILIAR;  end';
//PREPARE S2 from WORDS;
//EXECUTE S2;
//end; ";

                //DB2DataAdapter adapter = new DB2DataAdapter(sql, ncc);
                //DataSet ds1 = new DataSet();
                //adapter.FillSchema(ds1, SchemaType.Mapped);
                //adapter.Fill(ds1);
                ////-----------------

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
            DB2Connection ncc;
            if (dataBase == "")
                ncc = OpenConnectionGlobal();
            else
                ncc = OpenConnectionGlobal(dataBase);
            
            DB2Command com = new DB2Command(sql, ncc);
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
            DB2Connection ncc = OpenConnection();
            DB2Command com = new DB2Command(sql, ncc);

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

        public DB2DataReader DataReader(string sql)
        {
            DB2Command oc = new DB2Command();
            try
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
                oc.CommandText = sql;
                DB2Connection con = OpenConnection();
                connection = con;
                oc.Connection = con;
                DB2DataReader dr = oc.ExecuteReader();
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
            string val="";
            DB2Command command = new DB2Command();
            DB2Connection con;
            con = OpenConnection();

            command.Connection = con;
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;
            try
            {
                val = Convert.ToString(command.ExecuteScalar());
            }
            catch  { }

            CloseConnection(con);
            return val;
        }

        public string SingleDataGlobal(string sql)
        {
            string val = "";
            DB2Command command = new DB2Command();
            DB2Connection con;
            con = OpenConnectionGlobal();

            command.Connection = con;
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
            bool exist=false;
            DB2Command command = new DB2Command();
            DB2Connection con = OpenConnection();
            command.Connection = con;
            AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, sql, null, string.Empty);
            command.CommandText = sql;
            try
            {
                DB2DataReader db2dr = command.ExecuteReader();
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
            int numQueries=0, i=0;
            bool status=false;

            DB2Command command = new DB2Command();
            DB2Connection con = OpenConnection();
            command.Connection = con;

            DB2Transaction trans = con.BeginTransaction();
            command.Transaction = trans;

            for (i = 0; i < sql.Count; i++)
            {
                AdministradorCarpetasRegistro.GrabarLogs(TipoRegistro.Actividad, (string)sql[i], null, string.Empty);
                command.CommandText = sql[i].ToString();
                try
                {
                    command.ExecuteNonQuery();
                    exceptions += "ejecutando: " + sql[i] + cambioLinea;
                    numQueries++;

                    HistorialSeguimientoTabla(TABLA_SEGUIR, sql[i].ToString());  //Almacenamiento historial de seguimiento a tabla.
                }
                catch(Exception ex)
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

            if ( (sqlRevision.Contains("UPDATE " + tablaSeguir) == true || sqlRevision.Contains("INSERT INTO " + tablaSeguir) == true  || sqlRevision.Contains("DELETE FROM " + tablaSeguir) == true )
                && sqlRevision.Contains("MHISTORIAL_CAMBIOS") == false)
            {
                string usuario = HttpContext.Current.User.Identity.Name.ToLower();
                string operacion = "";

                if (sqlRevision.Contains("UPDATE") )
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

                DB2Command command = new DB2Command();
                DB2Connection con = OpenConnection();
                command.Connection = con;

                DB2Transaction trans = con.BeginTransaction();
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
            DB2Connection ncc = OpenConnection();
            DB2Command com = null;
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

                com = new DB2Command(sql, ncc);
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
            DB2Connection ncc = OpenConnection();
            DB2Command com = null;
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

                com = new DB2Command(sql, ncc);
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
            DB2Connection ncc = OpenConnection();
            DB2Command com = null;
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
                com = new DB2Command(sql, ncc);
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

        public bool DBBulkInsert(DataTable tabla, string nombreTablaBD)
        {
            bool rta = false;

            using (var dbBulkcopy = new IBM.Data.DB2.DB2BulkCopy(connectionString, IBM.Data.DB2.DB2BulkCopyOptions.Default))
            {
                // my DataTable column names match my SQL Column names, so I simply made this loop. However if your column names don't match, just pass in which datatable name matches the SQL column name in Column Mappings
                if (tabla.Rows.Count > 0)
                {
                    foreach (DataColumn col in tabla.Columns)
                    {
                        dbBulkcopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    dbBulkcopy.BulkCopyTimeout = 600;
                    dbBulkcopy.DestinationTableName = nombreTablaBD;

                    try
                    {
                        dbBulkcopy.WriteToServer(tabla);
                        dbBulkcopy.Close();
                        DBFunctions.closeConnection();
                        rta = true;
                    }
                    catch (Exception z)
                    {
                        dbBulkcopy.Close();
                        DBFunctions.closeConnection();
                        //rta = false;
                    }
                }
            }

            return rta;
        }
    }
}
