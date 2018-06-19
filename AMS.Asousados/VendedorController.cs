using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Configuration;
using AMS.Tools;
using AMS.DB;

namespace AMS.Asousados
{
    public class VendedorController
    {

        public static void updateVendedor(string codigoVend, string nombreVend, string ubicVend, string telfVend, string mailVend, HttpPostedFile fotoVend)
        {
            Hashtable updateHash = new Hashtable();
            Hashtable updatePk = new Hashtable();
            string tabla = "PVENDEDOR";

            if (fotoVend != null)
            {
                string fileName = fotoVend.FileName;
                string uploads = ConfigurationManager.AppSettings["PathToUploads"];

                if (fileName != "")
                {
                    fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    fotoVend.SaveAs(uploads + fileName);

                    updateHash.Add("PVEN_FOTO", Utils.ToVarchar(fileName));
                }

                updateHash.Add("PVEN_NOMBRE", Utils.ToVarchar(nombreVend));
                updateHash.Add("PUBI_SECUENCIA", ubicVend);
                updateHash.Add("PVEN_TELEFONO", Utils.ToVarchar(telfVend));
                updateHash.Add("PVEN_EMAIL", Utils.ToVarchar(mailVend));

                updatePk.Add("PVEN_SECUENCIA", codigoVend);

                DBFunctions.UpdateHashtable(tabla, updateHash, updatePk);
            }
        }

        public static void nuevoVendedor(string nombreVend, string ubicVend, string telfVend, string mailVend, HttpPostedFile fotoVend)
        {
            Hashtable datosHash = new Hashtable();
            string tabla = "PVENDEDOR";

            if (fotoVend != null)
            {
                string fileName = fotoVend.FileName;
                string uploads = ConfigurationManager.AppSettings["PathToUploads"];

                if (fileName != "")
                {
                    fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                    fotoVend.SaveAs(uploads + fileName);

                    datosHash.Add("PVEN_FOTO", Utils.ToVarchar(fileName));
                }

                datosHash.Add("PVEN_NOMBRE", Utils.ToVarchar(nombreVend));
                datosHash.Add("PUBI_SECUENCIA", ubicVend);
                datosHash.Add("PVEN_TELEFONO", Utils.ToVarchar(telfVend));
                datosHash.Add("PVEN_EMAIL", Utils.ToVarchar(mailVend));

                DBFunctions.SaveHashtable(tabla, datosHash);
            }
        }

        public static void eliminarVendedor(string codigoVend)
        {
            Hashtable deletePk = new Hashtable();
            string tabla = "PVENDEDOR";

            DBFunctions.NonQuery("update mvehiculousado set pven_asignado=null where pven_asignado=" + codigoVend);

            deletePk.Add("PVEN_SECUENCIA", codigoVend);
            DBFunctions.DeleteHashtable(tabla, deletePk);
        }

        private static bool AsignarVendedoresPorUbicacion(string ubicacion, int numAsignacion)
        {
            string tabla = "MVEHICULOUSADO";
            string sql = String.Format("select mveh_secuencia from mvehiculousado where test_tipoesta=20 and pven_asignado is null and pubi_codigo={0}", ubicacion);
            ArrayList vehiculos = DBFunctions.RequestAsCollection(sql);

            sql = String.Format("select pven_secuencia from pvendedor where pubi_secuencia={0}", ubicacion);
            ArrayList vendedores = DBFunctions.RequestAsCollection(sql);

            if (vendedores.Count == 0)
                return true;

            if (vendedores.Count <= numAsignacion)
                numAsignacion %= vendedores.Count;

            foreach (Hashtable vehiculo in vehiculos)
            {
                Hashtable updateHash = new Hashtable();
                Hashtable pkHash = new Hashtable();

                string vendedor = ((Hashtable)vendedores[numAsignacion])["PVEN_SECUENCIA"].ToString();

                updateHash["PVEN_ASIGNADO"] = Utils.ToVarchar(vendedor);
                pkHash["MVEH_SECUENCIA"] = vehiculo["MVEH_SECUENCIA"];

                int res = DBFunctions.UpdateHashtable(tabla, updateHash, pkHash);
                if (res == 1)
                    numAsignacion = (numAsignacion + 1) % vendedores.Count;
                else
                    return false;
            }

            sql = String.Format("update pubicacion set pubi_vendasig={0} where pubi_secuencia={1}", numAsignacion, ubicacion);
            return DBFunctions.NonQuery(sql) == 1;
        }

        public static bool AsignarVendedoresEnCarrusel(string asociado)
        {
            ArrayList sqls = new ArrayList();

            string sql = String.Format("update mconcesionario set mcon_autovend='S' where mnit_nit='{0}'", asociado);
            bool concecionarioActualizado = DBFunctions.NonQuery(sql) == 1;

            if (!concecionarioActualizado)
                return false;

            sql = string.Format(
                "SELECT PUBI_SECUENCIA AS UBICACION, \n" +
                "       pubi_vendasig as ASIGNACIONES \n" +
                "FROM PUBICACION \n" +
                "WHERE MNIT_NITCONCESIONARIO = '{0}'"
                , asociado);

            ArrayList ubicaciones = DBFunctions.RequestAsCollection(sql);

            foreach (Hashtable ubicacionHash in ubicaciones)
            {
                string ubicacion = ubicacionHash["UBICACION"].ToString();
                int numAsignacion = Convert.ToInt32(ubicacionHash["ASIGNACIONES"]);

                bool resp = AsignarVendedoresPorUbicacion(ubicacion, numAsignacion);

                if (!resp) return false;
            }

            return true;
        }
        public static bool DesasignarVendedoresPorCarrusel(string asociado)
        {
            string sql = String.Format("update mconcesionario set mcon_autovend='N' where mnit_nit='{0}'", asociado);
            bool concecionarioActualizado = DBFunctions.NonQuery(sql) == 1;
            if (!concecionarioActualizado)
                return false;

            sql = String.Format("update mvehiculousado set pven_asignado=null where mnit_nit='{0}'", asociado);
            return DBFunctions.NonQuery(sql) >= 0;
        }

        public static bool AsignarCarrosPorUbicacion(string idUbicacion)
        {
            string sql = String.Format(
                "SELECT PU.pubi_vendasig AS ASIGNACIONES \n" +
                "FROM PUBICACION PU  \n" +
                "  INNER JOIN MCONCESIONARIO MC ON PU.MNIT_NITCONCESIONARIO = MC.MNIT_NIT  \n" +
                "WHERE MCON_AUTOVEND = 'S' \n" +
                "AND   PU.PUBI_SECUENCIA =  {0}"
                , idUbicacion);

            ArrayList ubicaciones = DBFunctions.RequestAsCollection(sql);

            foreach (Hashtable ubicacionHash in ubicaciones)
            {
                int numAsignacion = Convert.ToInt32(ubicacionHash["ASIGNACIONES"]);

                bool resp = AsignarVendedoresPorUbicacion(idUbicacion, numAsignacion);

                if (!resp) return false;
            }

            return true;
        }
    }
}