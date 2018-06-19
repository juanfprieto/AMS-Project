using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;


namespace AMS.Documentos
{
    public class CausalDevolucionLogic
    {
        public const int INVENTARIOS_PROVEEDORES = 1;
        public const int INVENTARIOS_CLIENTES_MOSTRADOR = 2;
        public const int INVENTARIOS_TRANSFERENCIAS = 3;
        public const int VEHICULOS_PROVEEDORES = 4;
        public const int VEHICULOS_CLIENTE_MOSTRADOR = 5;
        public const int VEHICULOS_CLIENTE_MAYOR = 6;
        public const int TALLER = 7;

        private const string MECANICO_GENERAL = "MG";
        private const string RECEPCIONISTA_TALLER = "RT";
        private const string VENDEDOR_MOSTRADOR = "VM";
        private const string VENDEDOR_VEHICULOS = "VV";
        private const string RECEP_ALISTAMIENTO = "RA";
        private const string TODO_TIPO = "TT";
        private const string ANALISTA_GARANTIAS = "AG";

        private ArrayList vendedores;
        private ArrayList causas;
        private ArrayList acciones;
        private int callerType;

        public CausalDevolucionLogic(int callerType)
        {
            this.callerType = callerType;

            String sql = "select * from PCAUSALDEVOLUCION";
            causas = (ArrayList) DBFunctions.RequestAsCollection(sql);

            sql = "select * from PACCIONDEVOLUCION";
            acciones = (ArrayList)DBFunctions.RequestAsCollection(sql);
            
            sql = "SELECT PVEN_CODIGO, " +
             "       PVEN_NOMBRE, " +
             "       PVEN_CLAVE " +
             "FROM PVENDEDOR where ";

            switch (callerType)
            {
                case INVENTARIOS_PROVEEDORES:
                    sql += "TVEND_CODIGO='" + VENDEDOR_MOSTRADOR + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
                case INVENTARIOS_CLIENTES_MOSTRADOR:
                    sql += "TVEND_CODIGO='" + VENDEDOR_MOSTRADOR + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
                case INVENTARIOS_TRANSFERENCIAS:
                    sql += "TVEND_CODIGO='" + VENDEDOR_MOSTRADOR + "' ";
                    sql += "or TVEND_CODIGO='" + RECEPCIONISTA_TALLER + "' ";
                    sql += "or TVEND_CODIGO='" + MECANICO_GENERAL + "' ";
                    sql += "or TVEND_CODIGO='" + VENDEDOR_VEHICULOS + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
                case VEHICULOS_PROVEEDORES:
                    sql += "TVEND_CODIGO='" + VENDEDOR_VEHICULOS + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
                case VEHICULOS_CLIENTE_MOSTRADOR:
                    sql += "TVEND_CODIGO='" + VENDEDOR_VEHICULOS + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
                case VEHICULOS_CLIENTE_MAYOR:
                    sql += "TVEND_CODIGO='" + VENDEDOR_VEHICULOS + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
                case TALLER:
                    sql += "TVEND_CODIGO='" + VENDEDOR_MOSTRADOR + "' ";
                    sql += "or TVEND_CODIGO='" + RECEPCIONISTA_TALLER + "' ";
                    sql += "or TVEND_CODIGO='" + MECANICO_GENERAL + "' ";
                    sql += "or TVEND_CODIGO='" + VENDEDOR_VEHICULOS + "' ";
                    sql += "or TVEND_CODIGO='" + TODO_TIPO + "'";
                    break;
            }

            vendedores = (ArrayList)DBFunctions.RequestAsCollection(sql);
        }

        public ICollection getVendedoresForDdl()
        {
            Hashtable ddlVendedores = new Hashtable();

            foreach (Hashtable h in vendedores)
            {
                String pven_codigo = (String) h["PVEN_CODIGO"];
                String pven_nombre = (String)h["PVEN_NOMBRE"];

                ddlVendedores.Add(pven_codigo, pven_nombre);
            }

            return ddlVendedores;
        }

        public ICollection getCausasForDdl()
        {
            Hashtable ddlCausas = new Hashtable();

            foreach (Hashtable h in causas)
            {
                String pcau_codigo = (String)h["PCAU_CODIGO"];
                String pcau_nombre = (String)h["PCAU_NOMBRE"];

                ddlCausas.Add(pcau_codigo, pcau_nombre);
            }

            return ddlCausas;
        }

        public ICollection getAccionesForDdl()
        {
            Hashtable ddlacciones = new Hashtable();

            foreach (Hashtable h in causas)
            {
                String pacc_codigo = (String)h["PACC_CODIGO"];
                String pacc_nombre = (String)h["PACC_NOMBRE"];

                ddlacciones.Add(pacc_codigo, pacc_nombre);
            }

            return ddlacciones;
        }

        public bool passwordVendedorCorrecto(String pven_codigo, String clave)
        {
            String claveDB = "";
 
            foreach (Hashtable h in vendedores)
                if(h["PVEN_CODIGO"].Equals(pven_codigo))
                {
                    claveDB = (String) h["PVEN_CLAVE"];
                    break;
                }

            return claveDB.Equals(clave);
        }

        public bool causaRequiereEspecificacion(String pcau_codigo)
        {
            String habilitaDetalle = "";

            foreach (Hashtable h in causas)
                if (h["PCAU_CODIGO"].Equals(pcau_codigo))
                {
                    habilitaDetalle = (String)h["PCAU_HABILITADETALLE"];
                    break;
                }

            return "1".Equals(habilitaDetalle); //esto porque db2 no tiene boolean como tipo de dato
        }

        public bool accionRequiereProveedor(String pacc_codigo)
        {
            String habilitaProveedor = "";

            foreach (Hashtable h in causas)
                if (h["PACC_CODIGO"].Equals(pacc_codigo))
                {
                    habilitaProveedor = (String)h["PACC_HABILITAPROV"];
                    break;
                }

            return "1".Equals(habilitaProveedor); //esto porque db2 no tiene boolean como tipo de dato
        }
    }
}