using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;
using System.Collections;

namespace AMS.VIP
{
    public class IngresoClienteLogic
    {


        internal static bool tarjetaExist(string tarjeta)
        {
            return DBFunctions.RecordExist(String.Format("select mtar_codigo from mviptarjeta where mtar_codigo='{0}'",tarjeta));
        }

        internal static Hashtable getDatosIngresoClienteTarjeta(string tarjeta)
        {
            String sql = "SELECT c.mcli_codigo as CODIGO, \n" +
             "       c.tafi_codigo as AFILIACION \n" +
             "FROM mviptarjeta t \n" +
             "  LEFT JOIN mvipcliente c on t.mcli_codigo = c.mcli_codigo \n" +
             "WHERE mtar_codigo = '{0}'";

            return (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(String.Format(sql, tarjeta)))[0];
        }

        internal static bool nitExist(string nit)
        {
            return DBFunctions.RecordExist(String.Format("select mcli_codigo from mvipcliente where mcli_nit='{0}'", nit));
        }

        internal static Hashtable getDatosIngresoClienteNit(string nit)
        {
            String sql = "SELECT c.mcli_codigo as CODIGO,  \n" +
             "       c.tafi_codigo as AFILIACION  \n" +
             "FROM mvipcliente c \n" +
             "WHERE c.mcli_nit = '{0}'";

            return (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(String.Format(sql, nit)))[0];
        }
    }
}