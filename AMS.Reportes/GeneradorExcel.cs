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


namespace AMS.Reportes
{
    public class GeneradorExcel
    {

        public void generar()
        {
            FileInfo newFile = new FileInfo(@"C:\sample3.xlsx");
            FileInfo template = new FileInfo(@"C:\ExampleTemplate.xlsx");
            if (!template.Exists) throw new Exception("Template file does not exist! i.e. sample3template.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template)) 
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sales"];
                if (worksheet != null)
                {
                    //ExcelCell cell;
                    String dato1 = worksheet.Cells[1, 1].Text;
                    const int startRow = 5;
                    int row = startRow;

                    DataSet dsSql = new DataSet();
                    DBFunctions.Request(dsSql, IncludeSchema.NO, "select pdoc_codigo, mcaj_numero, mnit_nit, mcaj_razon, mcaj_valototal, mcaj_fecha, mcaj_valoneto from mcaja where pdoc_codigo ='CC' fetch first 3 rows only;");

                    for (int r = 0; r < dsSql.Tables[0].Rows.Count; r++ )
                    {
                        int col = 1;
                        // we have our total formula on row 7, so push them down so we can insert more data
                        if (row > startRow)
                        {
                            //worksheet.InsertRow(row);
                            worksheet.InsertRow(row, 1, startRow);
                        }

                        // our query has the columns in the right order, so simply
                        // iterate through the columns
                        for (int c = 0; c < dsSql.Tables[0].Columns.Count; c++)
                        {
                            // use the email address as a hyperlink for column 1
                            if (dsSql.Tables[0].Columns[c].ColumnName == "PDOC_CODIGO")
                            {
                                // insert the email address as a hyperlink for the name
                                string hyperlink = "mailto:" + dsSql.Tables[0].Rows[r][c] + "@hotmail.com";
                                //worksheet.Cell(row, col).Hyperlink = new Uri(hyperlink, UriKind.Absolute);
                                //worksheet.Cell(row, col).Value = dsSql.Tables[0].Rows[r][c].ToString();
                                worksheet.Cells[row, col].Hyperlink = new Uri(hyperlink, UriKind.Absolute);
                                worksheet.Cells[row, col].Value = dsSql.Tables[0].Rows[r][c].ToString();
                            }
                            else
                            {
                                // do not bother filling cell with blank data 
                                // (also useful if we have a formula in a cell)
                                if (dsSql.Tables[0].Rows[r][c] != null)
                                {
                                    //((worksheet.Cell(row, col).Value = dsSql.Tables[0].Rows[r][c].ToString(); //.Replace('.', ',');
                                    worksheet.Cells[row, col].Value = dsSql.Tables[0].Rows[r][c];
                                    if (col == 6)
                                        worksheet.Cells[row, col].Style.Numberformat.Format = "yyyy-MM-dd";

                                }
                            }
                            col++;
                        }
                        row++;
                    }

                    // delete the two spare rows we have in the template
                    //worksheet.DeleteRow(row, true);
                    //worksheet.DeleteRow(row, true);
                    worksheet.DeleteRow(row,1, true);
                    worksheet.DeleteRow(row,1, true);
                    row--;

                    /* 
                     * The data we just inserted is between startRow and row.
                     * Now we need to apply the same styles and common formula for all these rows.
                     * 
                     * First copy the styles from startRow to the new rows.     */
                    for (int iCol = 1; iCol <= 7; iCol++)
                    {
                        //cell = worksheet.Cell(startRow, iCol);
                        for (int iRow = startRow; iRow <= row; iRow++)
                        {
                            //worksheet.Cell(iRow, iCol).StyleID = cell.StyleID;
                            //worksheet.Cell(iRow, iCol).StyleID = worksheet.Cell(startRow, iCol).StyleID;
                            worksheet.Cells[iRow, iCol].StyleID = worksheet.Cells[startRow, iCol].StyleID;
                        }
                    }

                    // style the first row as they are the top achiever
                    //worksheet.Cell(startRow, 6).Style = "Good";
                    worksheet.Cells[startRow, 5].StyleName = "Good";
                    // style the last row as they are the worst performer
                    //worksheet.Cell(row, 6).Style = "Bad";
                    worksheet.Cells[row, 5].StyleName = "Bad";

                    // now create a *shared* formula based on the formula in the startRow column 5 and 7
                    //worksheet.CreateSharedFormula(worksheet.Cell(startRow, 5), worksheet.Cell(row, 5));
                    //worksheet.CreateSharedFormula(worksheet.Cell(startRow, 7), worksheet.Cell(row, 7));
                    worksheet.Cells[row + 1, 5].Formula = "Sum(" + worksheet.Cells[startRow, 5].Address + ":" + worksheet.Cells[row, 5].Address + ")";
                    worksheet.Cells[row + 1, 7].Formula = "Sum(" + worksheet.Cells[startRow, 7].Address + ":" + worksheet.Cells[row, 7].Address + ")";

                    // to force Excel to re-calculate the formulas in the total line, 
                    // we must remove the values currently in the cells
                    //worksheet.Cell(row + 1, 5).RemoveValue();
                    //worksheet.Cells[row + 1, 5].Reset();
                    
                    //worksheet.Cell(row + 1, 6).RemoveValue();
                    //worksheet.Cell(row + 1, 7).RemoveValue();

                    //worksheet.Workbook.CalcMode = ExcelCalcMode.Automatic;
                    // lets set the header text 
                    //worksheet.HeaderFooter.oddHeader.CenteredText = "AdventureWorks Inc. Sales Report";
                    worksheet.HeaderFooter.OddHeader.CenteredText = "";
                    // add the page number to the footer plus the total number of pages
                    //worksheet.HeaderFooter.oddFooter.RightAlignedText =
                    //    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    worksheet.HeaderFooter.OddFooter.RightAlignedText =
                        string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    // add the sheet name to the footer
                    //worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                    worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                    // add the file path to the footer
                    //worksheet.HeaderFooter.oddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
                    worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;

                    var chart = worksheet.Drawings.AddChart("Grafica de Precios.", OfficeOpenXml.Drawing.Chart.eChartType.ColumnClustered);
                    chart.Title.Text = "Grafica de Precios";
                    chart.SetPosition(3, 0, 8, 0);
                    chart.SetSize(400, 450); // Tamaño de la gráfica
                    chart.Legend.Remove(); // Si desea eliminar la leyenda

                    // Define donde está la información de la gráfica.
                    // Entiendase el nombre de la serie y los valores.
                    var serie = chart.Series.Add(worksheet.Cells["E5:E7"], worksheet.Cells["C5:C7"]);
                }

                // we had better add some document properties to the spreadsheet 
                // set some core property values
                xlPackage.Workbook.Properties.Title = "Sample 3";
                xlPackage.Workbook.Properties.Author = "John Tunnicliffe";
                xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
                xlPackage.Workbook.Properties.Keywords = "Office Open XML";
                xlPackage.Workbook.Properties.Category = "ExcelPackage Samples";
                xlPackage.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 file from scratch using the Packaging API and Office Open XML";

                // set some extended property values
                xlPackage.Workbook.Properties.Company = "AdventureWorks Inc.";
                xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.linkedin.com/pub/0/277/8a5");

                // set some custom property values
                xlPackage.Workbook.Properties.SetCustomPropertyValue("Checked by", "John Tunnicliffe");
                xlPackage.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1147");
                xlPackage.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "ExcelPackage");

                // save the new spreadsheet
                xlPackage.Save();
            }
            // if you want to take a look at the XML created in the package, simply uncomment the following lines
            // These copy the output file and give it a zip extension so you can open it and take a look!
            //FileInfo zipFile = new FileInfo(outputDir.FullName + @"\sample3.zip");
            //if (zipFile.Exists) zipFile.Delete();
            //newFile.CopyTo(zipFile.FullName);
        }

       
    }









    //test
    //public void generar()
    //    {
    //        FileInfo newFile = new FileInfo(@"C:\sample3.xlsx");
    //        FileInfo template = new FileInfo(@"C:\ExampleTemplate.xlsx");
    //        if (!template.Exists) throw new Exception("Template file does not exist! i.e. sample3template.xlsx");
    //        using (ExcelPackage xlPackage = new ExcelPackage(newFile, template)) 
    //        {
    //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["Sales"];
    //            if (worksheet != null)
    //            {
    //                //ExcelCell cell;
                    
    //                const int startRow = 5;
    //                int row = startRow;

    //                DataSet dsSql = new DataSet();
    //                DBFunctions.Request(dsSql, IncludeSchema.NO, "select pdoc_codigo, mcaj_numero, mnit_nit, mcaj_razon, mcaj_valototal, mcaj_fecha, mcaj_valoneto from mcaja where pdoc_codigo ='CC';");

    //                for (int r = 0; r < dsSql.Tables[0].Rows.Count; r++ )
    //                {
    //                    int col = 1;
    //                    // we have our total formula on row 7, so push them down so we can insert more data
    //                    if (row > startRow)
    //                    {
    //                        //worksheet.InsertRow(row);
    //                        worksheet.InsertRow(row, 1);
    //                    }

    //                    // our query has the columns in the right order, so simply
    //                    // iterate through the columns
    //                    for (int c = 0; c < dsSql.Tables[0].Columns.Count; c++)
    //                    {
    //                        // use the email address as a hyperlink for column 1
    //                        if (dsSql.Tables[0].Columns[c].ColumnName == "PDOC_CODIGO")
    //                        {
    //                            // insert the email address as a hyperlink for the name
    //                            string hyperlink = "mailto:" + dsSql.Tables[0].Rows[r][c] + "@hotmail.com";
    //                            //worksheet.Cell(row, col).Hyperlink = new Uri(hyperlink, UriKind.Absolute);
    //                            //worksheet.Cell(row, col).Value = dsSql.Tables[0].Rows[r][c].ToString();
    //                            worksheet.Cells[row, col].Hyperlink = new Uri(hyperlink, UriKind.Absolute);
    //                            worksheet.Cells[row, col].Value = dsSql.Tables[0].Rows[r][c].ToString();
    //                        }
    //                        else
    //                        {
    //                            // do not bother filling cell with blank data 
    //                            // (also useful if we have a formula in a cell)
    //                            if (dsSql.Tables[0].Rows[r][c] != null)
    //                            {
    //                                //((worksheet.Cell(row, col).Value = dsSql.Tables[0].Rows[r][c].ToString(); //.Replace('.', ',');
    //                                worksheet.Cells[row, col].Value = dsSql.Tables[0].Rows[r][c].ToString();
    //                            }
    //                        }
    //                        col++;
    //                    }
    //                    row++;
    //                }

    //                // delete the two spare rows we have in the template
    //                //worksheet.DeleteRow(row, true);
    //                //worksheet.DeleteRow(row, true);
    //                worksheet.DeleteRow(row,1, true);
    //                worksheet.DeleteRow(row,1, true);
    //                row--;

    //                /* 
    //                 * The data we just inserted is between startRow and row.
    //                 * Now we need to apply the same styles and common formula for all these rows.
    //                 * 
    //                 * First copy the styles from startRow to the new rows.     */
    //                for (int iCol = 1; iCol <= 7; iCol++)
    //                {
    //                    //cell = worksheet.Cell(startRow, iCol);
    //                    for (int iRow = startRow; iRow <= row; iRow++)
    //                    {
    //                        //worksheet.Cell(iRow, iCol).StyleID = cell.StyleID;
    //                        //worksheet.Cell(iRow, iCol).StyleID = worksheet.Cell(startRow, iCol).StyleID;
    //                        worksheet.Cells[iRow, iCol].StyleID = worksheet.Cells[startRow, iCol].StyleID;
    //                    }
    //                }

    //                // style the first row as they are the top achiever
    //                //worksheet.Cell(startRow, 6).Style = "Good";
    //                worksheet.Cells[startRow, 6].StyleName = "Good";
    //                // style the last row as they are the worst performer
    //                //worksheet.Cell(row, 6).Style = "Bad";
    //                worksheet.Cells[row, 6].StyleName = "Bad";

    //                // now create a *shared* formula based on the formula in the startRow column 5 and 7
    //                //worksheet.CreateSharedFormula(worksheet.Cell(startRow, 5), worksheet.Cell(row, 5));
    //                //worksheet.CreateSharedFormula(worksheet.Cell(startRow, 7), worksheet.Cell(row, 7));

    //                // to force Excel to re-calculate the formulas in the total line, 
    //                // we must remove the values currently in the cells
    //                //worksheet.Cell(row + 1, 5).RemoveValue();
    //                worksheet.Cells[row + 1, 5].Reset();
                    
    //                //worksheet.Cell(row + 1, 6).RemoveValue();
    //                //worksheet.Cell(row + 1, 7).RemoveValue();
    //                worksheet.Cells[row + 1, 7].Reset();

    //                // lets set the header text 
    //                //worksheet.HeaderFooter.oddHeader.CenteredText = "AdventureWorks Inc. Sales Report";
    //                worksheet.HeaderFooter.OddHeader.CenteredText = "";
    //                // add the page number to the footer plus the total number of pages
    //                //worksheet.HeaderFooter.oddFooter.RightAlignedText =
    //                //    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
    //                worksheet.HeaderFooter.OddFooter.RightAlignedText =
    //                    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
    //                // add the sheet name to the footer
    //                //worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
    //                worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
    //                // add the file path to the footer
    //                //worksheet.HeaderFooter.oddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
    //                worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
    //            }

    //            // we had better add some document properties to the spreadsheet 
    //            // set some core property values
    //            xlPackage.Workbook.Properties.Title = "Sample 3";
    //            xlPackage.Workbook.Properties.Author = "John Tunnicliffe";
    //            xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
    //            xlPackage.Workbook.Properties.Keywords = "Office Open XML";
    //            xlPackage.Workbook.Properties.Category = "ExcelPackage Samples";
    //            xlPackage.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 file from scratch using the Packaging API and Office Open XML";

    //            // set some extended property values
    //            xlPackage.Workbook.Properties.Company = "AdventureWorks Inc.";
    //            xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.linkedin.com/pub/0/277/8a5");

    //            // set some custom property values
    //            xlPackage.Workbook.Properties.SetCustomPropertyValue("Checked by", "John Tunnicliffe");
    //            xlPackage.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1147");
    //            xlPackage.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "ExcelPackage");

    //            // save the new spreadsheet
    //            xlPackage.Save();
    //        }
    //        // if you want to take a look at the XML created in the package, simply uncomment the following lines
    //        // These copy the output file and give it a zip extension so you can open it and take a look!
    //        //FileInfo zipFile = new FileInfo(outputDir.FullName + @"\sample3.zip");
    //        //if (zipFile.Exists) zipFile.Delete();
    //        //newFile.CopyTo(zipFile.FullName);
    //    }

       
    //}


}