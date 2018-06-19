using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace AMS.Reportes
{
    public abstract class FabricaReportes
    {
        public abstract ReporteFiltros FactoryMethod(string codReporte, PlaceHolder phFormulario);
    }
}