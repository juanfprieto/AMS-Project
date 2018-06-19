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

namespace AMS.Reportes
{
    public class ExtractorFacturaExcel
    {

        protected string PathToImports = ConfigurationSettings.AppSettings["PathToImports"];
        protected DataTable tablaFactura;

        public DataTable generar(FileInfo template)
        {
            //FileInfo newFile = new FileInfo(PathToImports + "\\excel\\pygTabla.xlsx");
            //FileInfo template = new FileInfo(PathToImports + "\\excel\\FACTURA.xlsx");

            if (!template.Exists) throw new Exception("Template file does not exist! i.e. sample3template.xlsx");
            using (ExcelPackage xlPackage = new ExcelPackage(template))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets["page 1"];
                if (worksheet != null)
                {
                    String datosTexto = worksheet.Cells[3, 1].Text;
                    DataSet dsFactura = new DataSet();
                    PrepararTablaFactura();
                    do
                    {
                        DataRow fila = tablaFactura.NewRow();

                        int espacio = datosTexto.IndexOf(" ");
                        string numero = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1));
                        fila[0] = numero;

                        espacio = datosTexto.IndexOf(" ");
                        string referencia = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        fila[1] = referencia;

                        espacio = datosTexto.IndexOf(" ");
                        string cantidad = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        fila[2] = Convert.ToDouble(cantidad); 

                        espacio = datosTexto.IndexOf("     ");
                        string descripcion = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        //fila[3] = descripcion;
                        fila[3] = Convert.ToDouble(cantidad); 

                        espacio = datosTexto.IndexOf("     ");
                        string precioNoImp = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        fila[4] = Convert.ToDouble(precioNoImp);

                        espacio = datosTexto.IndexOf("     ");
                        string descuento = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        //fila[5] = Convert.ToDouble(descuento);
                        fila[6] = Convert.ToDouble(descuento);

                        espacio = datosTexto.IndexOf("     ");
                        string iva = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        //fila[6] = Convert.ToDouble(iva);
                        fila[5] = Convert.ToDouble(iva);

                        espacio = datosTexto.IndexOf("     ");
                        string otrosImp = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        fila[7] = Convert.ToDouble(otrosImp);

                        espacio = datosTexto.IndexOf("\n");
                        string precioFinal = datosTexto.Substring(0, espacio);
                        datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                        fila[8] = Convert.ToDouble(precioFinal);

                        //caso especial cuando salta de linea el nombre de la descripción.
                        if (datosTexto.IndexOf(" ") > 3 && datosTexto.Substring(0,2) != "**")
                        {
                            espacio = datosTexto.IndexOf("\n");
                            descripcion += datosTexto.Substring(0, espacio);
                            datosTexto = datosTexto.Substring(espacio + 1, datosTexto.Length - (espacio + 1)).TrimStart();
                            //fila[3] = descripcion;
                        }

                        tablaFactura.Rows.Add(fila);
                    }
                    while (datosTexto.Substring(0, 2) != "**");

                    return tablaFactura;
                }
            }

            return null;
        }

        protected void PrepararTablaFactura()
        {
            tablaFactura = new DataTable();
            tablaFactura.Columns.Add(new DataColumn("NUMERO", System.Type.GetType("System.String")));//0
            tablaFactura.Columns.Add(new DataColumn("REFERENCIA", System.Type.GetType("System.String")));//1
            tablaFactura.Columns.Add(new DataColumn("CANTIDADFAC", System.Type.GetType("System.Double")));//2
            //tablaFactura.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//3
            tablaFactura.Columns.Add(new DataColumn("CANTIDADING", System.Type.GetType("System.Double")));//3
            tablaFactura.Columns.Add(new DataColumn("PRECIONOIMP", System.Type.GetType("System.Double")));//4
            tablaFactura.Columns.Add(new DataColumn("DESCUENTO", System.Type.GetType("System.Double")));//5
            tablaFactura.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//6
            tablaFactura.Columns.Add(new DataColumn("OTROSIMP", System.Type.GetType("System.Double")));//7
            tablaFactura.Columns.Add(new DataColumn("PRECIOFINAL", System.Type.GetType("System.Double")));//8
        }

    }
}