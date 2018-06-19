using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using AMS.DB;

namespace AMS.VIP
{
    public class ManejoClientesLogic
    {

        private const String TABLA_CLIENTES = "MVIPCLIENTE";
        private const String TABLA_FACTURAS = "MVIPFACTURA";
        private const String TABLA_TARJETAS = "MVIPTARJETA";
        private const String AFIL_HIJO = "HIJO";

        internal static bool guardarClientes(params Hashtable[] clientes)
        {
            bool bien = true;
            foreach (Hashtable cl in clientes)
                if (cl != null && bien)
                {
                    bien &= DBFunctions.SaveHashtable(TABLA_CLIENTES, cl);

                    string codCli = cl["mcli_codigo"].ToString();
                    bien &= guardarTarjetaNueva(codCli);
                }
            return bien;
        }

        internal static bool editarClientes(DictionaryEntry[] keys, params Hashtable[] clientes)
        {
            bool bien = true;

            for (int i = 0; i < 3; i++)
            {
                Hashtable cl = clientes[i];
                DictionaryEntry key = keys[i];
                bool itBien = true;

                if (cl != null)
                {
                    string codCli = cl["mcli_codigo"].ToString();

                    if (key.Value == null || key.Value.ToString() == "''")
                    {
                        itBien &= DBFunctions.SaveHashtable(TABLA_CLIENTES, cl);

                        if (itBien)
                            itBien &= guardarTarjetaNueva(codCli);
                    }
                    else
                    {
                        if (codCli == key.Value.ToString())
                        {
                            Hashtable clKey = new Hashtable();
                            clKey.Add(key.Key, key.Value);

                            itBien &= DBFunctions.UpdateHashtable(TABLA_CLIENTES, cl, clKey)==1;
                        }
                        else
                        {
                            itBien &= DBFunctions.SaveHashtable(TABLA_CLIENTES, cl);

                            if (itBien)
                                actualizarOtrosDatos(codCli, key.Value.ToString());
                        }
                    }

                bien &= itBien;
                }
            }

            return bien;
        }

        private static void actualizarOtrosDatos(string codCli, string codCliAnt)
        {
            String sql = String.Format("update mviptarjeta set mcli_codigo={0} where mcli_codigo={1}", codCli, codCliAnt);
            DBFunctions.NonQuery(sql);
            sql = String.Format("update mvipfactura set mcli_codigo={0} where mcli_codigo={1}", codCli, codCliAnt);
            DBFunctions.NonQuery(sql);
            sql = String.Format("update mvipredencion set mcli_codigo={0} where mcli_codigo={1}", codCli, codCliAnt);
            DBFunctions.NonQuery(sql);

            sql = String.Format("delete from mvipcliente where mcli_codigo={0}", codCliAnt);
            DBFunctions.NonQuery(sql);
        }

        public static bool guardarTarjetaNueva(string codCli)
        {
            Random random = new Random();
            long num1 = codCli.GetHashCode() < 0 ? codCli.GetHashCode() * -1 : codCli.GetHashCode();
            DateTime dt = DateTime.Now;
            long num2 = dt.GetHashCode() < 0 ? dt.GetHashCode() * -1 : dt.GetHashCode();
            string sql = null;
            string codTarjeta = null;

            do
            {
                num2 *= random.Next();
                codTarjeta = num1.ToString() + num2.ToString();
                codTarjeta = codTarjeta.Substring(0, 13);

                sql = String.Format("select mtar_codigo from mviptarjeta where mtar_codigo='{0}'", codTarjeta);

            } while (DBFunctions.RecordExist(sql));

            Hashtable tarjeta = new Hashtable();

            tarjeta["mtar_codigo"] = String.Format("'{0}'", codTarjeta);
            tarjeta["mcli_codigo"] = String.Format("{0}", codCli);
            tarjeta["testt_codigo"] = String.Format("'{0}'", "PI");
            tarjeta["tvig_codigo"] = String.Format("'{0}'", "V");
            tarjeta["mtar_fechaexpi"] = String.Format("'{0}'", dt.AddYears(3).ToString("yyyy-MM-dd"));

            return DBFunctions.SaveHashtable(TABLA_TARJETAS, tarjeta);
        }

        internal static Hashtable[] obtenerClientes(String codCliente)
        {
            Hashtable[] resp = new Hashtable[3];
            String sql = String.Format("select * from mvipcliente where mcli_codigo='{0}'", codCliente);
            Hashtable clienteTar = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

            String tipoAfil = clienteTar["TAFI_CODIGO"].ToString();
            String codSAP =  clienteTar["MCLI_SAP"].ToString();

            if (tipoAfil.Equals("HIJO"))
            {
                sql = String.Format("select * from mvipcliente where mcli_sap='{0}' and tafi_codigo<>'HIJO'", codSAP);
                Hashtable padre = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

                resp[0] = padre;
                resp[1] = clienteTar;
            }
            else
            {
                sql = String.Format("select * from mvipcliente where mcli_sap='{0}' and tafi_codigo='HIJO'", codSAP);
                ICollection hijos = DBFunctions.RequestAsCollection(sql);
                resp[0] = clienteTar;

                ArrayList list = (ArrayList)hijos;

                resp[1] = list.Count > 0 ? (Hashtable)list[0] : null;
                resp[2] = list.Count > 1 ? (Hashtable)list[1] : null;
            }

            return resp;
        }

        internal static bool validarClienteParaFacturacion(string codCliente, ref string errorFac)
        {
            String sql = String.Format("select mcli_sap from mvipcliente where mcli_codigo='{0}'", codCliente);
            String codSAP = DBFunctions.SingleData(sql);

            sql = String.Format("select mcli_codigo from mvipcliente where mcli_sap='{0}' and tafi_codigo<>'HIJO'", codSAP);
            codCliente = DBFunctions.SingleData(sql);

            sql = String.Format("SELECT f.mvfac_codigo AS codigo,  \n" +
             "       e.tvest_nombre AS estado, \n" +
             "       f.mvfac_valor AS valor, \n" +
             "       f.mvfac_fechapago as fecha  \n" +
             "FROM mvipfactura f  \n" +
             "  INNER JOIN tvipestadofactura e ON e.tvest_codigo = f.tvest_codigo \n" +
             "WHERE e.tvest_bloquea = 'B'  \n" +
             "AND   f.mcli_codigo = '{0}'", codCliente);

            ArrayList facturas = (ArrayList)DBFunctions.RequestAsCollection(sql);
            String errStr = "Factura: {0} Estado: {1} Valor: {2} Fecha de Pago: {3}<br>";

            if (facturas.Count == 0) return true;

            foreach (Hashtable hash in facturas)
            {
                String fac = hash["CODIGO"].ToString();
                String estado = hash["ESTADO"].ToString();
                String valor = hash["VALOR"].ToString();
                String fecha = ((DateTime)hash["FECHA"]).ToString("yyyy-MM-dd");

                errorFac += String.Format(errStr, fac, estado, valor, fecha);
            }

            return false;
        }

        internal static string calcularFechaPago(string diaCorte, string fechaFac, double valorFac)
        {
            DateTime fecha = DateTime.Now;

            if (diaCorte != "")
            {
                int diaAct = fecha.Day;
                int diaCorteInt = Convert.ToInt32(diaCorte);

                if (diaAct <= diaCorteInt)
                    fecha = fecha.AddDays(diaCorteInt - diaAct);
                else
                    fecha = fecha.AddDays(30 + diaCorteInt - diaAct);

                if (fecha.Day < diaCorteInt) fecha = fecha.AddDays(1); //para los meses de 31 días

            }
            else if (fechaFac != "")
            {
                return fechaFac;
            }
            else
            {
                string sql = "select cvred_valorquiebre, cvred_diascredbajo, cvred_diascredalto from cvipredencion";
                Hashtable parametros = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

                double valorQuiebre = Convert.ToDouble(parametros["CVRED_VALORQUIEBRE"]);
                int diasBajo = Convert.ToInt32(parametros["CVRED_DIASCREDBAJO"]);
                int diasAlto = Convert.ToInt32(parametros["CVRED_DIASCREDALTO"]);

                if (valorFac <= valorQuiebre) fecha = fecha.AddDays(diasBajo);
                else fecha = fecha.AddDays(diasAlto);
            }

            return fecha.ToString("yyyy-MM-dd");
        }

        internal static int obtenerPuntosFactura(double valFac)
        {
            string sql = "select cvred_puntosacum, cvred_valoracum from cvipredencion";
            Hashtable parametros = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

            int puntos = Convert.ToInt32(parametros["CVRED_PUNTOSACUM"]);
            int valor = Convert.ToInt32(parametros["CVRED_VALORACUM"]);

            int puntosAcum = ((int)valFac / valor) * puntos;

            return puntosAcum;
        }

        internal static bool guardarFactura(Hashtable factura)
        {
            string user = HttpContext.Current.User.Identity.Name.ToString().ToLower();
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string idFactura = factura["mvfac_codigo"] as string;

            string sqlHistorial = "INSERT INTO MHISTORIAL_FACTURA " +
                                  "(FACTURA, OPERACION, SUSU_USUARIO, FECHA) \n" +
                                  "VALUES ({0}, 'I', '{1}', '{2}')";

            DBFunctions.NonQuery(String.Format(sqlHistorial, idFactura, user, now));

            return DBFunctions.SaveHashtable(TABLA_FACTURAS, factura);
        }

        internal static void sumarPuntos(string codCliente, long puntos)
        {
            string sql = String.Format("select tafi_codigo, mcli_sap, mcli_puntos from mvipcliente where mcli_codigo='{0}'", codCliente);
            Hashtable cliente = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

            string afil = cliente["TAFI_CODIGO"].ToString();
            string codSap = cliente["MCLI_SAP"].ToString();
            long puntosCli = Convert.ToInt64(cliente["MCLI_PUNTOS"]);

            string sqlUpdate = "UPDATE {0} SET MCLI_PUNTOS={1} WHERE MCLI_CODIGO='{2}'";
            
            sql = String.Format(sqlUpdate, TABLA_CLIENTES, puntos + puntosCli, codCliente);
            DBFunctions.NonQuery(sql);

            if (afil.Equals(AFIL_HIJO))
            {
                sql = String.Format("select mcli_codigo, mcli_puntos from mvipcliente where mcli_sap='{0}' and tafi_codigo<>'HIJO'", codSap);
                Hashtable padre = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

                string codPadre = padre["MCLI_CODIGO"].ToString();
                long puntosPadre = Convert.ToInt64(padre["MCLI_PUNTOS"]);

                sql = String.Format(sqlUpdate, TABLA_CLIENTES, puntos + puntosPadre, codPadre);
                DBFunctions.NonQuery(sql);
            }
        }

        internal static int getCodUsuario(){

            string username = HttpContext.Current.User.Identity.Name;

            string sql = String.Format("select susu_codigo from susuario where UPPER(susu_login)=UPPER('{0}')",username);

            int codigo = Convert.ToInt32(DBFunctions.SingleData(sql));

            return codigo;
        }

        internal static bool cupoHabilitado(string codCliente, Hashtable factura)
        {
            String sql = String.Format("select mcli_sap from mvipcliente where mcli_codigo='{0}'", codCliente);
            String codSAP = DBFunctions.SingleData(sql);

            sql = String.Format("select mcli_cupocli from mvipcliente where mcli_sap='{0}' and tafi_codigo<>'HIJO'", codSAP);
            Hashtable cliente = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection(sql))[0];

            sql = String.Format("select COALESCE(sum (mvfac_valor), 0) from mvipfactura where tvest_codigo<>'EX' and mcli_codigo='{0}'", codCliente);
            string valorPendienteStr = DBFunctions.SingleData(sql);

            double valorPendiente = Convert.ToDouble(valorPendienteStr == "" ? "0" : valorPendienteStr);
            double valorFac = Convert.ToDouble(factura["mvfac_valor"]);
            double cupoCli = Convert.ToDouble(cliente["MCLI_CUPOCLI"]);

            return cupoCli >= valorPendiente + valorFac;
        }

        internal static string getNombreCliente(string codCliente)
        {
            string sql = String.Format("select mcli_nombrecompleto from mvipcliente where mcli_codigo='{0}'", codCliente);

            return DBFunctions.SingleData(sql);
        }

        internal static string getCupoUtilizado(string codCli)
        {
            string sql = String.Format("select coalesce(sum (mvfac_valor),0) from mvipfactura where tvest_codigo<>'EX' and mcli_codigo='{0}'", codCli);
            double sum = Convert.ToDouble(DBFunctions.SingleData(sql));
            
            return ((long)sum).ToString();
        }
    }
}