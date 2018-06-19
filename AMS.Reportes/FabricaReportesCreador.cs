using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace AMS.Reportes
{
    public class FabricaReportesCreador : FabricaReportes
    {
        public override ReporteFiltros FactoryMethod(string codReporte, PlaceHolder phFormulario)
        {
            if (codReporte != "")
                return new FabricaRepProductorCrystal(codReporte, phFormulario);  //Lo insntancia como una fabrica de reportes de Crystal
            else
                return null;  //Para la creación de otro tipo fabrica de reportes (Ej:FabricaRepProductorHTML5)
        }
    }
}