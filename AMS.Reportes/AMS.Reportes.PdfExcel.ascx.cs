using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Reportes
{
	public partial class PdfExcel : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            try
            {
                string file = "C:\\inetpub\\wwwroot\\AMS\\AMS\\imp\\facturametrokia.pdf";
                if (!File.Exists(file))
                {
                    file = Path.GetFullPath(file);
                    if (!File.Exists(file))
                    {
                        Console.WriteLine("Please give in the path to the PDF file.");
                    }
                }

                PDFParser pdfParser = new PDFParser();
                pdfParser.ExtractText(file, "C:\\inetpub\\wwwroot\\AMS\\AMS\\imp\\facturametrokia.txt");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

		}
	}
}