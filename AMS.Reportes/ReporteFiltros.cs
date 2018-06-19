using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;

namespace AMS.Reportes
{
    public interface ReporteFiltros
    {
        void CargarFiltros();
        Table TablaFiltros();
        Table TablaFiltrosCrystal();
        DataSet DsFiltrosReporte();
    }
}