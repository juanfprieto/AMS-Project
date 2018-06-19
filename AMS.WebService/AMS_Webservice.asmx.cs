using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Data;
using AMS.DB;

namespace AMS.Web
{
    /// <summary>
    /// Summary description for AMS_Webservice  =O
    /// </summary>
    [WebService(Namespace = "http://201.228.212.66/WebService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class AMS_Webservice : System.Web.Services.WebService
    {

        [WebMethod]
        public DataSet RequestSQL(string user, string pass, string sql)
        {
            Usuario usuarioIngreso = new Usuario();

            if (usuarioIngreso.AutenticarUsuario(user, pass)=="true")
            {
                DataSet ds = new DataSet();
                ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
                return ds;
            }

            return null;
        }

        [WebMethod]
        public string SingleDataSQL(string user, string pass, string sql)
        {
            Usuario usuarioIngreso = new Usuario();

            if (usuarioIngreso.AutenticarUsuario(user, pass) == "true")
                return DBFunctions.SingleData(sql);

            return null;//"fallo autentic .:." + DBFunctions.exceptions;
        }

        [WebMethod]
        public int NonQuerySQL(string user, string pass, string sql)
        {
            Usuario usuarioIngreso = new Usuario();

            if (usuarioIngreso.AutenticarUsuario(user, pass) == "true")
                return DBFunctions.NonQuery(sql);

            return 0;
        }

        [WebMethod]
        public int NonQueryBlobSQL(string user, string pass, string sql, byte[] blob)
        {
            Usuario usuarioIngreso = new Usuario();

            if (usuarioIngreso.AutenticarUsuario(user, pass) == "true")
                return DBFunctions.NonQuery(sql);

            return 0;
        }

        [WebMethod]
        public bool RecordExistSQL(string user, string pass, string sql)
        {
            Usuario usuarioIngreso = new Usuario();

            if (usuarioIngreso.AutenticarUsuario(user, pass) == "true")
                return DBFunctions.RecordExist(sql);

            return false;
        }

        [WebMethod]
        public bool TransactionSQL(string user, string pass, ArrayList sql)
        {
            Usuario usuarioIngreso = new Usuario();

            if (usuarioIngreso.AutenticarUsuario(user, pass) == "true")
                return DBFunctions.Transaction(sql);

            return false;
        }
    }
}
