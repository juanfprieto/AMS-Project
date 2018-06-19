using System;
using System.Collections.Generic;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;

namespace AMS.Reportes
{
	public partial class ExcelReports : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (Request.QueryString["proc"] != null)
            {
                lblTitulo.Text = "Informe Daimler";
                plcContenido.Visible = false;
            }
            else 
            {
                lblTitulo.Text = "Reporte PyG";
                plcContenido.Visible = true;
            }

            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlYear, "SELECT pano_ano FROM pano ORDER BY pano_ano DESC");
                bind.PutDatasIntoDropDownList(ddlMes, "select pmes_mes, pmes_nombre from pmes order by 1;");
            }
		}

        protected void GenerarExcel(Object Sender, EventArgs e)
		{
            string year = ddlYear.SelectedValue;
            string mes = ddlMes.SelectedValue;
            lkDescarga.Visible = true;
            lkDescarga.Target = "_blank";

            if (Request.QueryString["proc"] != null)
            {
                ReporteExcelFunciones repFuncion = new ReporteExcelFunciones();
                repFuncion.generar(year, mes);
                lkDescarga.NavigateUrl = "..\\imp\\excel\\plantillaExcelGEN.xlsx";
            }
            else 
            {
                ReporteExcelTemplate rep = new ReporteExcelTemplate();
                rep.generar(year, mes);
                lkDescarga.NavigateUrl = "..\\imp\\excel\\pygTabla.xlsx";
            }
            
            //ExtractorFacturaExcel extractor = new ExtractorFacturaExcel();
            //string ass = extractor.generar();

        }
        
	}
}
