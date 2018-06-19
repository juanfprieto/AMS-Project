using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Data;
using OfficeOpenXml;

namespace AMS.Tools
{
	public partial class ExcelimportData : System.Web.UI.UserControl
	{
        protected string pathImpExcel = ConfigurationManager.AppSettings["PathToImpExcelData"];
        public DataTable dtExcel = new DataTable();

		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (xlsUpload.HasFile)
            {
                bool uplod = true;
                string fleUpload = Path.GetExtension(xlsUpload.FileName.ToString());
                
                if (fleUpload.Trim().ToLower() == ".xls" || fleUpload.Trim().ToLower() == ".xlsx")
                {
                    xlsUpload.SaveAs(Server.MapPath(pathImpExcel + xlsUpload.FileName.ToString()));
                    string uploadedFilePath = (Server.MapPath(pathImpExcel + xlsUpload.FileName.ToString()));

                    try
                    {
                        FileInfo template = new FileInfo(uploadedFilePath);

                        if (!template.Exists) 
                            throw new Exception("Template file does not exist! i.e. xlsx");

                        using (ExcelPackage xlPackage = new ExcelPackage(template, false) )
                        {
                            if (xlPackage.Workbook.Worksheets.Count > 0)
                            {
                                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                                if (worksheet != null)
                                {
                                    ExcelNamedRange namedRange = xlPackage.Workbook.Names["TABLA"];
                                    int colFin = namedRange.End.Column;
                                    int rowFin = namedRange.End.Row;

                                    //Recorrer todo el contenido del excel (debe estar en formato de tabla y ordenado segun el Grid que se usará.
                                    for (int r = 1; r <= rowFin; r++)
                                    {
                                        DataRow drow = dtExcel.NewRow();
                                        for (int c = 1; c <= colFin; c++)
                                        {
                                            if (r == 1)  //Titulos de columna
                                            {
                                                dtExcel.Columns.Add( worksheet.Cells[r, c].Text );
                                            }
                                            else   //Filas Contenido
                                            {
                                                drow[c - 1] = worksheet.Cells[r, c].Text;
                                            }
                                        }
                                        if(r != 1)
                                            dtExcel.Rows.Add(drow);
                                    }

                                    if (dtExcel.Rows.Count > 0)
                                    {
                                        if (dtExcel.Rows[dtExcel.Rows.Count - 1][0].ToString() == "") //elmina fila en blanco del final.
                                            dtExcel.Rows.RemoveAt(dtExcel.Rows.Count - 1);
                                        Session["dtExcel"] = dtExcel;
                                        Control p = this.Parent;
                                        p.FindControl("btnVincularExcel").Visible = true;
                                        lblResultado.Text = "Carga Completa! >>";
                                        lblResultado.Visible = true;
                                    }

                                }
                            }
                        }

                    }
                    catch (Exception)
                    {
                        //uplod = false;
                        //this.lblMessage.Text = "System uploading Error";
                    }

                    //File.Delete(uploadedFile); // Delete upload Excel
                }

                if (uplod)
                {
                    //string mess1 = "File has successfully uploaded";
                    //this.lblMessage.Text = mess1;
                }
            }
            else
            {
                //this.lblMessage.Text = "Please select file to upload.";
            }

        }
	}
}