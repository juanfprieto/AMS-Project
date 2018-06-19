using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;

namespace AMS.Reportes
{
    public interface IConsultaHelper
    {
        RelacionFiltro CrearRelacionFiltro(string table, string fieldId, string fieldLabel, string filtro, string orderID);
        ArrayList ObtenerFiltrosReporte(string idReporte);
        DataSet ObtenerSQL(string idReporte);
        Hashtable ObtenerMascaras(string idReporte);
    }
}