using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;

namespace AMS.Marketing
{
    public class SeguimientoClienteEspecificoLogic
    {

        public String getIdVendedor(String nit)
        {
            String sql = String.Format("SELECT pven_codigo \n" +
             "FROM mcliente \n" +
             "WHERE mnit_nit = '{0}'"
             ,nit);

            String pven_codigo = DBFunctions.SingleData(sql);

            return pven_codigo;
        }

        public String getNitFromPlaca(String placa)
        {
            String sql = String.Format("SELECT mnit_nit \n" +
             "FROM mcatalogovehiculo \n" +
             "WHERE mcat_placa = '{0}'"
             ,placa);

            String nit = DBFunctions.SingleData(sql);

            return nit;
        }

        public bool nitCorrecto(String nit)
        {
            bool resp = true;

            if (!nit.Equals(""))
            {
                String sql = String.Format("SELECT * \n" +
                         "FROM mnit \n" +
                         "WHERE mnit_nit = '{0}'",
                         nit);

                bool recordExist = DBFunctions.RecordExist(sql);

                resp = recordExist;
            }
            else resp = false;

            return resp;
        }
    }
}