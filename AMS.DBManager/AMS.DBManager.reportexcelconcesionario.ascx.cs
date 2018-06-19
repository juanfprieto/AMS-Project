using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Drawing;      
using AMS.DB;
using AMS.Forms;

namespace AMS.DBManager
{
    public partial class AMS_DBManager_reportexcelconcesionario : System.Web.UI.UserControl
	{
        //string cod;
        //string ubicacion = "C:\\Inetpub\\wwwroot\\VasColombia\\imp\\excel\\coneccion.xls";
        //DropDownList DropDownList1, DropDownList2, DropDownList3;
        //private void consultas_excel()
        //{

        //    Excel.Application oXL;
        //    Excel._Workbook oWB;
        //    Excel._Worksheet oSheet;
        //    try
        //    {
        //        //Iniciar Excel y obtener el objeto de la aplicación.                
        //        oXL = new Excel.Application();//"c:/uno.xls"
        //        oXL.Visible = true;
        //        //Obtener un nuevo libro.
        //        oWB = (Excel._Workbook)(oXL.Workbooks._OpenXML(ubicacion, 1));
        //        oSheet = (Excel._Worksheet)oWB.ActiveSheet;
        //        //Agregar encabezados de la tabla va célula por célula.
        //        oSheet.Cells[2, 2] = DropDownList1.Text;
        //        oSheet.Cells[3, 2] = DropDownList2.Text;


        //        #region casos
        //        switch (DropDownList3.Text)
        //        {
        //            case "(todas)":
        //                cod = "(todas)";
        //                break;
        //            case "AUTOALEMANIA S.A.":
        //                cod = "CO20004";
        //                break;
        //            case "COLWAGEN S.A.":
        //                cod = "CO20005";
        //                break;
        //            case "COLWAGEN PREMIUM S.A.":
        //                cod = "CO20006";
        //                break;
        //            case "EUROCAR S.A.":
        //                cod = "CO20017";
        //                break;
        //            case "AUTOBLITZ S.A.":
        //                cod = "CO20020";
        //                break;
        //            case "LAS MAQUINAS":
        //                cod = "CO20024";
        //                break;

        //        }
        //        #endregion

        //        oSheet.Cells[4, 2] = cod.ToString();
        //        oXL.Visible = true;
        //        oXL.UserControl = true;
        //    }
        //    catch (Exception theException)
        //    {
        //        String errorMessage;
        //        errorMessage = "Error: ";
        //        errorMessage = String.Concat(errorMessage, theException.Message);
        //        errorMessage = String.Concat(errorMessage, " Line: ");
        //        errorMessage = String.Concat(errorMessage, theException.Source);
        //        //
        //    }
        //}
        protected void Page_Load(object sender, System.EventArgs e)
		{
            
		}
        protected void Button1_Click(object sender, EventArgs e)
        {
            //consultas_excel();
        }
	}
}