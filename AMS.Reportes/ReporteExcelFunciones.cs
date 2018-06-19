using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using OfficeOpenXml.Style.XmlAccess;
using System.Data;
using AMS.DB;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Collections;

namespace AMS.Reportes
{
    public class ReporteExcelFunciones
    {
        protected string PathToImports = ConfigurationSettings.AppSettings["PathToImports"];
        public void generar(string year, string mes)
        {
            FileInfo newFile = new FileInfo(PathToImports + "\\excel\\plantillaExcelGEN.xlsx");
            FileInfo template = new FileInfo(PathToImports + "\\excel\\plantillaExcel.xlsx");

            if (!template.Exists) throw new Exception("Template file does not exist! i.e. sample3template.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                //for (int i = 0; i < 8; i++)
                for (int i = 0; i < xlPackage.Workbook.Worksheets.Count; i++)
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i + 1];
                    if (worksheet != null)
                    {
                        ArrayList funcionesExcel = ObtenerParametrosExcel(worksheet);

                        CargarDatosConsultas(ref worksheet, funcionesExcel);
                        
                        //OPCIONES ADICIONALES...
                        //worksheet.Workbook.CalcMode = ExcelCalcMode.Automatic;
                        // lets set the header text 
                        //worksheet.HeaderFooter.oddHeader.CenteredText = "AdventureWorks Inc. Sales Report";
                        //worksheet.HeaderFooter.OddHeader.CenteredText = "";
                        // add the page number to the footer plus the total number of pages
                        //worksheet.HeaderFooter.oddFooter.RightAlignedText =
                        //    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                        //worksheet.HeaderFooter.OddFooter.RightAlignedText =
                        //    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                        // add the sheet name to the footer
                        //worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                        //worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                        // add the file path to the footer
                        //worksheet.HeaderFooter.oddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
                        //worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
                    }
                }

                // we had better add some document properties to the spreadsheet 
                xlPackage.Workbook.Properties.Title = "Genrador Funciones Excel";
                xlPackage.Workbook.Properties.Author = "AMS ECAS";
                xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
                xlPackage.Workbook.Properties.Keywords = "Office Open XML";
                xlPackage.Workbook.Properties.Category = "Funciones Excel";
                xlPackage.Workbook.Properties.Comments = "Obtencion de funciones y registro";

                // set some extended property values
                xlPackage.Workbook.Properties.Company = "AMS ECAS S.A.S.";
                xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.linkedin.com/pub/0/277/8a5");

                // set some custom property values
                xlPackage.Workbook.Properties.SetCustomPropertyValue("Checked by", "Sistema AMS");

                // save the new spreadsheet
                xlPackage.Save();
                //System.Diagnostics.Process.Start("C:\\IsolatedStorage\\pygTabla.xlsx");
                // For the example

            }

            // if you want to take a look at the XML created in the package, simply uncomment the following lines
            // These copy the output file and give it a zip extension so you can open it and take a look!
            //FileInfo zipFile = new FileInfo(outputDir.FullName + @"\sample3.zip");
            //if (zipFile.Exists) zipFile.Delete();
            //newFile.CopyTo(zipFile.FullName);
        }

        //Busca en las celdas de la plantilla de excel todas las referencias al identificador de parametro, trayendo los valores relacionados.
        public ArrayList ObtenerParametrosExcel(ExcelWorksheet ws)
        {
            ArrayList paramsExcel = new ArrayList();
            ArrayList funcioExcel = new ArrayList();
            if (ws.Dimension == null)
            {
                return funcioExcel;
            }
            int colFin = ws.Dimension.End.Column;
            int rowFin = ws.Dimension.End.Row;
            
            for (int r = 1; r <= rowFin; r++)
            {
                for (int c = 1; c <= colFin; c++)
                {
                    string contenidoCell = ws.Cells[r, c].Text;
                    if (contenidoCell.Contains(">@"))
                    {
                        string[] valores = new string[2];
                        valores[0] = contenidoCell.Replace(">","-");
                        valores[1] = ws.Cells[r, c + 1].Text;//Los valores de los parametros siempre iran al frente de la descripcion del parametro.
                        paramsExcel.Add(valores); 
                    }
                    else if (contenidoCell.Contains(">#"))
                    {
                        string[] valores = new string[2];
                        valores[0] = contenidoCell;
                        valores[1] = r + "," + c;
                        funcioExcel.Add(valores); 
                    }
                }
            }

            for (int p = 0; p < funcioExcel.Count; p++)
            { 
                string[] contenidoFunc = (string[])funcioExcel[p];
                if (contenidoFunc[0].Contains("-@"))
                {
                    for (int q = 0; q < paramsExcel.Count; q++)
                    {
                        string[] contenidoParam = (string[])paramsExcel[q];
                        if (contenidoFunc[0].Contains(contenidoParam[0]))
                        {
                            contenidoFunc[0] = contenidoFunc[0].Replace(contenidoParam[0], contenidoParam[1]);
                            funcioExcel[p] = contenidoFunc;
                            break;
                        }
                    }
                }
            }

            return funcioExcel;
        }

        //Filtra y adiciona los resultados de datos para el worksheet segun el diccionario de funciones.
        public void CargarDatosConsultas(ref ExcelWorksheet ws, ArrayList funcioExcel)
        {
            //Recorrer registros e identificar el tipo de funcion.
            for (int y = 0; y < funcioExcel.Count; y++)
            {
                string[] contenidoFuncion = (string[])funcioExcel[y];

                double valor = IdentificarFuncion(contenidoFuncion[0]);
                string [] coord = contenidoFuncion[1].Split(',');
                int row = Convert.ToInt16(coord[0]);
                int col = Convert.ToInt16(coord[1]);
                ws.Cells[row, col].Value = valor;
            }
        }

        public double IdentificarFuncion(string parametroFunc)
        {
            DataSet dsSql = new DataSet();
            double valor = 0;
            string resultado = "0";

            //Obtencion de nombre de la funcion en Excel.
            //Ejemplo: >#VehVend(2012,1,Mazda)  **para obtener>  VehVend
            string funcion = parametroFunc.Replace(">#", "");
            funcion = funcion.Substring(0, funcion.IndexOf("("));
            funcion = funcion.Trim();
            
            //Obtencion de parametros de la funcion especifica.
            //Ejemplo: >#VehVend(2012,1,Mazda)  **para obtener>  2012,1,Mazda
            int ini = parametroFunc.IndexOf("(") + 1;
            int fin = parametroFunc.IndexOf(")") - ini;
            parametroFunc = parametroFunc.Substring(ini, fin);
            string [] paramsF = parametroFunc.Split(',');

            switch (funcion)
            {
                case "VehVend":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VEHICULOSVENDIDOS, paramsF[0], paramsF[1], paramsF[2]) );
                    break;
                case "EntraTaller":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.NUMEENTRADATALLER, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "ValRepFabrica":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VALOREPUESTOSFABRICA, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
                case "FactCombustible":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.FACTURACOMBUSTIBLELUBRI, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "ValDevolProv":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VALODEVOLUCIONPROV, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "ValDesCliRepTal":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VALORDESCTOCLTEREPTOSTALL, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "ValDesCliMobTal":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VALORDESCTOCLTEMOBRATALL, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "CostVentRepTal":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.COSTOVENTAREPUESTOSTALL, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "VentReptoMostrador":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VENTAREPUESTOSMOSTRADOR, paramsF[0], paramsF[1]));
                    break;
                case "DesctVentReptoMostrador":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.DESCUENTOVENTAREPUESTOSMOSTRADOR, paramsF[0], paramsF[1]));
                    break;
                case "DevolVentReptoMostrador":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.DEVOLUCIONVENTAREPTOSMOSTRADOR, paramsF[0], paramsF[1]));
                    break;
                case "CostoVentReptoMostrador":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.COSTOVENTAREPTOSMOSTRADOR, paramsF[0], paramsF[1]));
                    break;
                case "ConsumoInternoReptos":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.CONSUMOINTERNOREPUESTOS, paramsF[0], paramsF[1]));
                    break;
                case "GastoVentacCosto":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.GASTOSVENTACENTROCOSTO, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "CompraItemsaProveedor":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.COMPRAITEMSAPROVEEDOR, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "CompraItemsOtrosProveedores":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.COMPRAITEMSOTROSPROVEEDORES, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "TotalRenglonesPedidos":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALRENGLONESPEDIDOS, paramsF[0], paramsF[1] ));
                    break;
                case "TotalRenglonesDespachados":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALRENGLONESDESPACHADOS, paramsF[0], paramsF[1]));
                    break;
                case "TotalRenglonesBackOrder":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALRENGLONESBACKORDER, paramsF[0], paramsF[1]));
                    break;
                case "TotalRenglonesPedidosTaller":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALRENGLONESPEDIDOSTALLER, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
                case "TotalRenglonesDespachadosTaller":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALRENGLONESDESPACHADOSTALLER, paramsF[0], paramsF[1], paramsF[2]));
                    break;
               case "TotalRenglDespachTallerInmediato":
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALRENGLONESDESPACHADOSTALLERINMEDIATO, paramsF[0], paramsF[1], paramsF[2]));
                    break;
               case "TotalEntradasalTaller":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALENTRADASALTALLER, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
               case "TotalEntradasalTallerFacturadas":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALENTRADASALTALLERFACTURADAS, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
               case "TotalEntradasalTallerDia":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALENTRADASALTALLERDIA, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
               case "TotalEntradasalTallerFacturadasDia":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALENTRADASALTALLERFACTURADASDIA, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
               case "TotalOrdenesCriticoReptos":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALORDENESCRITICOREPUESTOS, paramsF[0], paramsF[1]));
                    break;
                case "TotalHorasFacturadasTaller":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALHORASFACTURADASTALLER, paramsF[0], paramsF[1], paramsF[2], paramsF[3]));
                    break;
                case "InventarioClasificadoAbcd":
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.INVENTARIOCLASIFICADOABCD, paramsF[0], paramsF[1]));
                    break;
                case "TotalHorasProductivasTaller":
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALHORASPRODUCTIVASTALLER, paramsF[0], paramsF[1]));
                    break;
                case "TotalVentasMobraTaller":
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    paramsF[3] = setMultipleParams(paramsF[3]);
                    paramsF[4] = setMultipleParams(paramsF[4]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.TOTALVENTASMOBRATALLER, paramsF[0], paramsF[1], paramsF[2], paramsF[3], paramsF[4]));
                    break;
                case "InventarioenProcesoTaller":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.INVENTARIOENPROCESOTALLER, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "GarantiasFacturadas":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.GARANTIASFACTURADAS, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "GarantiasSinFacturar":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    paramsF[2] = setMultipleParams(paramsF[2]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.GARANTIASSINFACTURAR, paramsF[0], paramsF[1], paramsF[2]));
                    break;
                case "VentaRepuestosDiaTaller":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VENTAREPUESTOSDIATALLER, paramsF[0], paramsF[1]));
                    break;
                case "VentaMobraDiaTallerR":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VENTAMOBRADIATALLER, paramsF[0], paramsF[1]));
                    break;
                case "DeduciblesDiaTaller":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.DEDUCIBLESDIATALLER, paramsF[0], paramsF[1]));
                    break;
                case "NumeEntradasDiaTaller":
                    paramsF[0] = setMultipleParams(paramsF[0]);
                    paramsF[1] = setMultipleParams(paramsF[1]);
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.NUMEENTRADASDIATALLER, paramsF[0], paramsF[1]));
                    break;
                case "Valormanodeobrapagadocgis":
                    resultado = DBFunctions.SingleData(string.Format(ExcelFuncionesDB.VALORMANODEOBRAPAGADOCGIS, paramsF[0], paramsF[1], paramsF[2]));
                    break;

            }
            if (resultado == "") 
                resultado = "0";
            valor = Convert.ToDouble(resultado);

            

            return valor;
        }

        public string setMultipleParams(string p)
        {
            p = p.Replace("-", "','");
            p = "'" + p + "'";
            return p;
        }

    }
}