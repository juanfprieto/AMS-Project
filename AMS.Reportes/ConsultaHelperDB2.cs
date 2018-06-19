using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using AMS.DB;
using AMS.Tools;
using System.Data;


namespace AMS.Reportes
{
    [Serializable]
    public class ConsultaHelperDB2 : IConsultaHelper
    {
        private bool usaGlobal = false;
        public ConsultaHelperDB2(bool uGlobal)
        {
            usaGlobal = uGlobal;
        }

        public RelacionFiltro CrearRelacionFiltro(string table, string fieldId, string fieldLabel, string filtro, string orderID)
        {
            string tabla = table;
            string campoId = fieldId;
            string campoLabel = fieldLabel;

            ArrayList datos = new ArrayList();

            string sql = null;

            if (campoLabel == null || campoLabel == "")
                sql = String.Format(
                      @"SELECT {0} AS ID,  
                             {0} AS LABEL  
                      FROM {1}  
                      {2}  
                      ORDER BY "+ orderID +@" "
                      , campoId
                      , tabla
                      , filtro);
            else
                sql = String.Format(
                      @"SELECT {0} AS ID,  
                              '[' || {0} || '] - ' || {1} AS LABEL  
                        FROM {2}  
                       {3}  
                        ORDER BY "+ orderID +@" "
                      , campoId
                      , campoLabel
                      , tabla
                      , filtro);

            sql = sql.Replace("?", "'");
            ArrayList sqlData = DBFunctions.RequestAsCollection(sql);

            if (sqlData == null)
                throw new Exception("Yogui Meditation E32: Existe un Problema con los Filtros, revise las tablas relacionadas a los DropDownLists");

            foreach (Hashtable hash in sqlData)
                datos.Add(new DictionaryEntry(hash["ID"], hash["LABEL"]));

            return new RelacionFiltro()
            {
                Tabla = tabla,
                CampoId = campoId,
                CampoMuestra = campoLabel,
                Data = datos
            };
        }

        public ArrayList ObtenerFiltrosReporte(string idReporte)
        {
                                    
            ArrayList filtrosConsulta = new ArrayList();
            
            String sql = String.Format(
                 "SELECT SFIL_ID, \n" +
                 "       SFIL_CONTASOC, \n" +
                 "       SFIL_TABLA, \n" +
                 "       SFIL_CAMPO, \n" +
                 "       SFIL_DATOCOMP1, \n" +
                 "       SFIL_ETIQUETA, \n" +
                 "       SFIL_LUPA, \n" +
                 "       SFIL_TIPCOMPA, \n" +
                 "       SFIL_DATOCOMP2 \n" +
                 "FROM SFILTRO \n" +
                 "WHERE SREP_ID = {0} ORDER BY SFIL_ID"
                 , idReporte);

            ArrayList filtrosData;

            if (usaGlobal)
                filtrosData = DBFunctions.RequestGlobalAsCollection(sql);
            else
                filtrosData = DBFunctions.RequestAsCollection(sql);
            
            foreach (Hashtable hash in filtrosData)
            {
                FiltroDescriptor newFiltro = new FiltroDescriptor()
                {
                    Id = Convert.ToInt32(hash["SFIL_ID"]),
                    Label = hash["SFIL_ETIQUETA"].ToString(),
                    Nombre = hash["SFIL_CAMPO"].ToString(),
                    Filtro = hash["SFIL_DATOCOMP1"].ToString(),
                    TipoCampo = ObtenerTipoCampo(hash["SFIL_CONTASOC"].ToString()),
                    TablaLupa = hash["SFIL_LUPA"].ToString(),
                    TipoComparacion = hash["SFIL_TIPCOMPA"].ToString(),
                    DatoComparar = hash["SFIL_DATOCOMP2"].ToString()
                };

                //bool isNum;
                //double retNum;
                //    isNum = Double.TryParse(Convert.ToString(newFiltro.DatoComparar), 
                //    System.Globalization.NumberStyles.Any, 
                //    System.Globalization.NumberFormatInfo.InvariantInfo, 
                //    out retNum);

                if (newFiltro.TipoCampo == TipoCampo.RelacionForanea)
                {
                    string[] camposRelacion = newFiltro.Nombre.Split(',');

                    string fieldId = camposRelacion[0].Trim();
                    string table = hash["SFIL_TABLA"].ToString();
                    string fieldLabel = null;
                    string filtroDDL = newFiltro.Filtro;
                    string orderID = "1";
                    if (camposRelacion.Length == 3)
                        orderID = camposRelacion[2].Trim();
                    

                    if (newFiltro.Filtro != "" && newFiltro.TipoComparacion != "" && newFiltro.DatoComparar != "")
                        filtroDDL = "WHERE " + newFiltro.Filtro + " " + newFiltro.TipoComparacion + " " + newFiltro.DatoComparar;

                    if (camposRelacion.Length > 1)
                    {  
                        fieldLabel = camposRelacion[1].Trim();
                    }

                    newFiltro.Relacion = CrearRelacionFiltro(table, fieldId, fieldLabel, filtroDDL, orderID);
                }

                filtrosConsulta.Add(newFiltro);
            }

            return filtrosConsulta;
        }

        public TipoCampo ObtenerTipoCampo(string campoAsociado)
        {
            TipoCampo campo;
            switch (campoAsociado)
            {
                case "DropDownList": campo = TipoCampo.RelacionForanea;
                    break;
                case "Cadena": campo = TipoCampo.Cadena;
                    break;
                case "Fecha": campo = TipoCampo.Fecha;
                    break;
                case "Numero": campo = TipoCampo.Numero;
                    break;
                case "TimeStamp": campo = TipoCampo.Timestamp;
                    break;
                case "Snippet": campo = TipoCampo.Snippet;
                    break;
                case "TextBox": campo = TipoCampo.Cadena; //por defecto, se deberían usar las otras opciones
                    break;
                default: campo = TipoCampo.Cadena;
                    break;
            }
            return campo;
        }

        public DataSet ObtenerSQL(string idReporte)
        {
            DataSet dsResultado = new DataSet();
            string sql = String.Format(
                 "SELECT SREP_SQL, SREP_AGRUPAR " +
                 "FROM SREPORTE " +
                 "WHERE SREP_ID = {0}"
                 , idReporte);

            if (usaGlobal)
                DBFunctions.RequestGlobal(dsResultado, IncludeSchema.NO, "", sql);
            else
                DBFunctions.Request(dsResultado, IncludeSchema.NO, sql);
            return dsResultado;
        }

        public Hashtable ObtenerMascaras(string idReporte)
        {
            string sql = String.Format(
                 "SELECT SREP_MASKS " +
                 "FROM SREPORTE " +
                 "WHERE SREP_ID = {0}"
                 , idReporte);

            Hashtable masks = new Hashtable();
            string[] strMasks;

            if (usaGlobal)
                strMasks = DBFunctions.SingleDataGlobal(sql).Split(',');
            else
                strMasks = DBFunctions.SingleData(sql).Split(',');

            if (strMasks[0] != "")
                foreach (string mask in strMasks)
                {
                    string[] splMask = mask.Trim().Split('-');
                    int numColumn = Convert.ToInt32(splMask[0]);
                    TipoCampo tipoFiltro = splMask[1] == "FN" ? TipoCampo.Numero :
                                           splMask[1] == "FM" ? TipoCampo.Moneda :
                                           splMask[1] == "FE" ? TipoCampo.Entero : TipoCampo.Cadena;

                    masks.Add(numColumn, tipoFiltro);
                }

            return masks;
        }
    }

}