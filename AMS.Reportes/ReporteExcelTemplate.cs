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
using Microsoft.Office.Interop.Excel;
using System.Data;
using AMS.DB;
using System.Configuration;
using System.Diagnostics;
using System.Globalization; 

namespace AMS.Reportes
{
    public class ReporteExcelTemplate
    {
        protected string PathToImports = ConfigurationSettings.AppSettings["PathToImports"];
        public void generar(string year, string mes)
        {
            FileInfo newFile = new FileInfo(PathToImports + "\\excel\\pygTabla.xlsx");
            FileInfo template = new FileInfo(PathToImports + "\\excel\\pyg.xlsx");
            
            if (!template.Exists) throw new Exception("Template file does not exist! i.e. sample3template.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                //for (int i = 0; i < 8; i++)
                for (int i = 0; i < xlPackage.Workbook.Worksheets.Count; i++)
                {
                    //ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["pyg"+i];
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i+1];
                    if (worksheet != null)
                    {
                        //ExcelCell cell;
                        //String yearExcel = worksheet.Cells[4, 3].Text;
                        String sede = worksheet.Cells[5, 3].Text;
                        String centro = worksheet.Cells[6, 3].Text;

                        //Set años en cabeceras de columnas.
                        worksheet.Cells[4, 3].Value = year;
                        for (int h = 1; h <= 13; h++)
                        {
                            worksheet.Cells[13, h+4].Value = year;
                        }

                        const int startRow = 14;
                        int row = startRow;

                        int contadorEspacios = 0;
                        int excRow = 0;
                        string cuentasDB = worksheet.Cells[14 + excRow, 1].Text;
                        string cuentasSQL_A = "";
                        string cuentasSQL_B = "";
                        string centrosSQL = "";

                        String[] centroArreglo;
                        if (centro.Contains(" A "))
                        {
                            centroArreglo = centro.Split(new string[] { " A " }, StringSplitOptions.None);
                            centrosSQL += " AND dc.pcen_codigo between " + centroArreglo[0] + " and " + centroArreglo[1] + "  ";
                        }
                        else if (centro.Contains(";"))
                        {
                            centroArreglo = centro.Split(';');
                            centrosSQL += " AND (";
                            for (int g = 0; g < centroArreglo.Length; g++)
                            {
                                centrosSQL += " dc.pcen_codigo = " + centroArreglo[g] + " or";
                            }
                            centrosSQL += ";";
                            centrosSQL = centrosSQL.Replace("or;", ") ");
                        }
                        else if (centro != "TODOS")
                        {
                            centrosSQL += " AND pcen_codigo = " + centro + " ";
                        }

                        while (contadorEspacios <= 2)
                        {
                            cuentasDB = worksheet.Cells[14 + excRow, 1].Text;
                            if (cuentasDB != "")
                            {
                                String[] cuentasArreglo;
                                if (cuentasDB.Contains(" A "))
                                {
                                    cuentasArreglo = cuentasDB.Split(new string[] { " A " }, StringSplitOptions.None);
                                    cuentasSQL_A += " WHEN dc.mcue_codipuc between '" + cuentasArreglo[0] + "%' and '" + cuentasArreglo[1] + "%' THEN '" + cuentasDB + "' ";
                                    cuentasSQL_B += " (dc.mcue_codipuc between '" + cuentasArreglo[0] + "%' AND '" + cuentasArreglo[1] + "%') or";
                                    //cuentasSQL_B += " (dc.mcue_codipuc like '" + cuentasArreglo[0] + "%' or dc.mcue_codipuc like '" + cuentasArreglo[1] + "%') or";
                                }
                                else if (cuentasDB.Contains(";"))
                                {
                                    cuentasArreglo = cuentasDB.Split(';');
                                    cuentasSQL_A += " WHEN (";
                                    cuentasSQL_B += " (";
                                    for (int g = 0; g < cuentasArreglo.Length; g++)
                                    {
                                        cuentasSQL_A += " dc.mcue_codipuc like '" + cuentasArreglo[g] + "%' or";
                                        cuentasSQL_B += " dc.mcue_codipuc like '" + cuentasArreglo[g] + "%' or";
                                    }
                                    cuentasSQL_A += ";";
                                    cuentasSQL_A = cuentasSQL_A.Replace("or;", "");
                                    cuentasSQL_A += ") THEN '" + cuentasDB + "' ";

                                    cuentasSQL_B += ";";
                                    cuentasSQL_B = cuentasSQL_B.Replace("or;", "");
                                    cuentasSQL_B += ") or";
                                }
                                else
                                {
                                    cuentasSQL_A += " WHEN dc.mcue_codipuc like '" + cuentasDB + "%' THEN '" + cuentasDB + "' ";
                                    cuentasSQL_B += " dc.mcue_codipuc like '" + cuentasDB + "%' or";
                                }

                                contadorEspacios = 0;
                            }
                            else
                            {
                                contadorEspacios++;
                            }
                            excRow++;
                        }
                        cuentasSQL_B += ";";
                        cuentasSQL_B = cuentasSQL_B.Replace("or;", "");
                        //61350410 A 61350475 (dc.mcue_codipuc between '61350410%' and '61350475%')
                        //61350615;61350618;61350685 (dc.mcue_codipuc like '413504%' or dc.mcue_codipuc like '413504%')

                        string sqlConsulta =
                            @"select cuenta,
                            coalesce( sum(mes1d), 0) as mes1d,
                            coalesce(  sum(mes2d), 0) as mes2d,
                            coalesce(  sum(mes3d), 0) as mes3d,
                            coalesce(  sum(mes4d), 0) as mes4d,
                            coalesce(  sum(mes5d), 0) as mes5d,
                            coalesce(  sum(mes6d), 0) as mes6d,
                            coalesce(  sum(mes7d), 0) as mes7d,
                            coalesce(  sum(mes8d), 0) as mes8d,
                            coalesce(  sum(mes9d), 0) as mes9d,
                            coalesce(  sum(mes10d), 0) as mes10d, 
                            coalesce(  sum(mes11d), 0) as mes11d, 
                            coalesce(  sum(mes12d), 0) as mes12d,
                            coalesce(  sum(mes1c), 0) as mes1c,
                            coalesce(  sum(mes2c), 0) as mes2c,
                            coalesce(  sum(mes3c), 0) as mes3c,
                            coalesce(  sum(mes4c), 0) as mes4c,
                            coalesce(  sum(mes5c), 0) as mes5c,
                            coalesce(  sum(mes6c), 0) as mes6c,
                            coalesce(  sum(mes7c), 0) as mes7c,
                            coalesce(  sum(mes8c), 0) as mes8c,
                            coalesce(  sum(mes9c), 0) as mes9c,
                            coalesce(  sum(mes10c), 0) as mes10c, 
                            coalesce(  sum(mes11c), 0) as mes11c, 
                            coalesce(  sum(mes12c), 0) as mes12c  from
                            (select CASE " +
                                    cuentasSQL_A +
                                @" ELSE 'UNKNOWN DEPARTMENT'
                            END AS Cuenta, 
	                            case when mc.pmes_mes = 1 THEN sum(dc.dcue_valodebi) end as mes1d,
	                            case when mc.pmes_mes = 2 THEN sum(dc.dcue_valodebi) end as mes2d, 
	                            case when mc.pmes_mes = 3 THEN sum(dc.dcue_valodebi) end as mes3d,
	                            case when mc.pmes_mes = 4 THEN sum(dc.dcue_valodebi) end as mes4d, 
	                            case when mc.pmes_mes = 5 THEN sum(dc.dcue_valodebi) end as mes5d,
	                            case when mc.pmes_mes = 6 THEN sum(dc.dcue_valodebi) end as mes6d, 
	                            case when mc.pmes_mes = 7 THEN sum(dc.dcue_valodebi) end as mes7d,
	                            case when mc.pmes_mes = 8 THEN sum(dc.dcue_valodebi) end as mes8d,
	                            case when mc.pmes_mes = 9 THEN sum(dc.dcue_valodebi) end as mes9d,
	                            case when mc.pmes_mes = 10 THEN sum(dc.dcue_valodebi) end as mes10d,
	                            case when mc.pmes_mes = 11 THEN sum(dc.dcue_valodebi) end as mes11d,
	                            case when mc.pmes_mes = 12 THEN sum(dc.dcue_valodebi) end as mes12d,
	                            case when mc.pmes_mes = 1 THEN sum(dcue_valocred) end as mes1c,
	                            case when mc.pmes_mes = 2 THEN sum(dcue_valocred) end as mes2c, 
	                            case when mc.pmes_mes = 3 THEN sum(dcue_valocred) end as mes3c,
	                            case when mc.pmes_mes = 4 THEN sum(dcue_valocred) end as mes4c, 
	                            case when mc.pmes_mes = 5 THEN sum(dcue_valocred) end as mes5c,
	                            case when mc.pmes_mes = 6 THEN sum(dcue_valocred) end as mes6c, 
	                            case when mc.pmes_mes = 7 THEN sum(dcue_valocred) end as mes7c,
	                            case when mc.pmes_mes = 8 THEN sum(dcue_valocred) end as mes8c,
	                            case when mc.pmes_mes = 9 THEN sum(dcue_valocred) end as mes9c,
	                            case when mc.pmes_mes = 10 THEN sum(dcue_valocred) end as mes10c,
	                            case when mc.pmes_mes = 11 THEN sum(dcue_valocred) end as mes11c,
	                            case when mc.pmes_mes = 12 THEN sum(dcue_valocred) end as mes12c
                            from dcuenta dc, mcomprobante mc 
                            where (" + cuentasSQL_B + @") 
                            and mc.pdoc_codigo=dc.pdoc_codigo and mc.mcom_numedocu=dc.mcom_numedocu and 
                            mc.pano_ano=" + year + @" and mc.pmes_mes>=1 and mc.pmes_mes<=" + mes + " " +
                            (sede != "TODOS" ? " and dc.palm_almacen='" + sede + "' " : " ") +
                            centrosSQL +
                            @" group by dc.mcue_codipuc,  mc.pmes_mes) 
                            group by cuenta
                            ;";

                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        //string nombreMes = formatoFecha.GetMonthName(Convert.ToInt16(mes));
                        string nombreMes = "";
                        if (mes == "1")
                            nombreMes = "ENERO";
                        else if (mes == "2")
                            nombreMes = "FEBRERO";
                        else if (mes == "3")
                            nombreMes = "MARZO";
                        else if (mes == "4")
                            nombreMes = "ABRIL";
                        else if (mes == "5")
                            nombreMes = "MAYO";
                        else if (mes == "6")
                            nombreMes = "JUNIO";
                        else if (mes == "7")
                            nombreMes = "JULIO";
                        else if (mes == "8")
                            nombreMes = "AGOSTO";
                        else if (mes == "9")
                            nombreMes = "SEPTIEMBRE";
                        else if (mes == "10")
                            nombreMes = "OCTUBRE";
                        else if (mes == "11")
                            nombreMes = "NOVIEMBRE";
                        else if (mes == "12")
                            nombreMes = "DICIEMBRE";

                        int diasMes = DateTime.DaysInMonth(Convert.ToInt16(year), Convert.ToInt16(mes));

                        worksheet.Cells[10, 2].Value = "A " + nombreMes + " " + diasMes + " DE " + year;

                        DataSet dsSql = new DataSet();
                        DBFunctions.Request(dsSql, IncludeSchema.NO, sqlConsulta);

                        int excelRow = 0;
                        for (int r = 0; r < dsSql.Tables[0].Rows.Count; r++)
                        {
                            int col = 5;

                            string cuenta = worksheet.Cells[14 + excelRow, 1].Text;
                            while (cuenta == "")
                            {
                                excelRow++;
                                cuenta = worksheet.Cells[14 + excelRow, 1].Text;
                            }

                            string operacion = worksheet.Cells[14 + excelRow, col].Text;
                            bool filaNula = false;
                            for (int c = 1; c <= 12; c++)
                            {
                                DataRow[] result = dsSql.Tables[0].Select("cuenta = '" + cuenta + "'");
                                double deb = 0;
                                double cred = 0;

                                if (result.Length != 0)
                                {
                                    deb = Convert.ToDouble(result[0][c].ToString());
                                    cred = Convert.ToDouble(result[0][c + 12].ToString());
                                }
                                else
                                {
                                    filaNula = true;
                                }

                                if (operacion == "DEBICRED")
                                {
                                    worksheet.Cells[14 + excelRow, col].Value = (deb - cred);
                                }
                                else if (operacion == "CREDDEBI")
                                {
                                    worksheet.Cells[14 + excelRow, col].Value = (cred - deb);
                                }

                                col++;
                            }
                            if (filaNula == true)
                            {
                                r--;
                                filaNula = false;
                            }
                            excelRow++;
                            row++;
                        }
                        row--;

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
                // set some core property values
                xlPackage.Workbook.Properties.Title = "PYG por Centro de Costos";
                xlPackage.Workbook.Properties.Author = "AMS ECAS";
                xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
                xlPackage.Workbook.Properties.Keywords = "Office Open XML";
                xlPackage.Workbook.Properties.Category = "Reporte PyG";
                xlPackage.Workbook.Properties.Comments = "Este es un reporte que calcula el balance PyG";

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

    }
}