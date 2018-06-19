using AMS.CriptoServiceProvider;
using AMS.DB;
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Collections;

namespace AMS.Tools
{
    public partial class AMS_Tools_Email : System.Web.UI.UserControl
    {
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected static string nombreReporte, correo;
        protected static string rutaArchivo;
        protected static string rutaImagen;
        protected static DataSet dsExcel;
        //protected static string attachment;
        protected System.Data.DataSet ds = new System.Data.DataSet();
        protected static int async;

        public string RutaImagen { set { rutaImagen = value;  } get { return rutaImagen; } }

        public string Correo { set { correo = value; } get { return correo; } }
        public DataSet DsExcel { set { dsExcel = value; } get { return dsExcel; } }
        public string NombreReporte { set { nombreReporte = value; } get { return nombreReporte; } }
        public string RutaArchivo { set { rutaArchivo = value; } get { return rutaArchivo; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(NombreReporte != null && NombreReporte != "")
            {
                string[] arrayReporte = NombreReporte.Split('-');
                try
                {
                    string tipoDocumento = DBFunctions.SingleData("SELECT TDOC_TIPODOCU FROM PDOCUMENTO WHERE PDOC_CODIGO = '" + arrayReporte[1] + "'");
                    if (tipoDocumento == "CV")
                    {
                        tbMail.Text = DBFunctions.SingleData("SELECT DVIS_EMAIL FROM DVISITADIARIACLIENTES WHERE PDOC_CODIGO = '" + arrayReporte[1] + "' AND DVIS_NUMEVISI = '" + arrayReporte[2] + "';");
                    }
                    else if (tipoDocumento == "RC")
                    {
                        tbMail.Text = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MCAJA MC, MNIT M WHERE M.MNIT_NIT = MC.MNIT_NIT AND PDOC_CODIGO = '" + arrayReporte[1] + "' AND MCAJ_NUMERO = '" + arrayReporte[2] + "';");
                    }
                    else if (tipoDocumento == "FC")
                    {
                        tbMail.Text = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MFACTURACLIENTE MF, MNIT M WHERE M.MNIT_NIT = MF.MNIT_NIT AND PDOC_CODIGO = '" + arrayReporte[1] + "' AND MFAC_NUMEDOCU = '" + arrayReporte[2] + "';");
                    }
                    else if (tipoDocumento == "FP")
                    {
                        tbMail.Text = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MFACTURAPROVEEDOR MFP, MNIT M WHERE M.MNIT_NIT = MFP.MNIT_NIT AND PDOC_CODIORDEPAGO = '" + arrayReporte[1] + "' AND MFAC_NUMEORDEPAGO = '" + arrayReporte[2] + "';");
                    }
                    else if (tipoDocumento == "OT")
                    {
                        tbMail.Text = DBFunctions.SingleData("SELECT MNIT_EMAIL FROM MORDEN MO, MNIT M WHERE M.MNIT_NIT = MO.MNIT_NIT AND PDOC_CODIGO = '" + arrayReporte[1] + "' AND MORD_NUMEORDE = '" + arrayReporte[2] + "';");
                    }
                }
                catch (Exception err) { }
            }
        }

        public void ImageButton1_Click(Object Sender, EventArgs e)
        {
        AMS.CriptoServiceProvider.Crypto miCripto = new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string password = ConfigurationManager.AppSettings["PasswordEMail"];
            string empresa = DBFunctions.SingleDataGlobal("select gemp_descripcion from gempresa where gemp_nombre='" + GlobalData.getEMPRESA() + "';");

            if (rutaImagen != null)
            {
                if(rutaArchivo == "")
                {
                    return;
                }
                try
                {
                    //string newPwd=miCripto.DescifrarCadena(password);
                    string newPwd = password;

                    string[] arrayReporte = NombreReporte.Split('.');
                    string nReporte = arrayReporte[arrayReporte.Length - 1];
                    //string rutaArchivo = ConfigurationManager.AppSettings["PathToReports"] + NombreReporte + "_" + HttpContext.Current.User.Identity.Name.ToLower() + "." ;
                    string urlImagen = "http://ecas.co/images/" + GlobalData.getEMPRESA() + ".png";

                    string mensaje =
                        @"<div style='position: absolute; background-color:#EEEFD9;width: 35%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
	                	    <img style='width: 20%; position: absolute; right: 2%;' src='" + urlImagen + @"' /><br><br>
		                    <b><font size='5'>Reporte Generado:</font></b>
		                    <br>" + nReporte +
                           @"<br><br>
		                    <b>Reciba un cordial saludo</b>, <br>
		                    Ha recibido un reporte usando el Sistema Ecas <br>
                            Dicho reporte se encuentra disponible como archivo <br>
                            adjunto en este correo.
		                    <br><br>
	                        <b>Gracias por su atención.</b>
		                    <br>
		                    <i>eCAS-AMS.</i>
	                    </div>
                        <br><br>";
                    if(rutaArchivo != null && rutaArchivo != "")
                    {
                        enviarMail(tbMail.Text, "Ha recibido un reporte de " + empresa + ": " + NombreReporte, mensaje, TipoCorreo.HTML, rutaArchivo);
                    }
                    else if(rutaImagen != null && rutaImagen != "")
                    {
                        enviarMail(tbMail.Text, "Ha recibido un reporte de " + empresa + ": " + NombreReporte, mensaje, TipoCorreo.HTML, rutaImagen);
                    }

                    Utils.MostrarAlerta(Response, "Email con Reporte ha sido enviado correctamente a: " + tbMail.Text);
                }
                catch (Exception er)
                {
                    lb.Text = "El Servidor de correos no ha sido configurado. Contactar Administrador de sistemas.";
                }
            }
            else if(dsExcel != null)
            {
                ds = dsExcel;
                Random num = new Random();
                string nombre = "" + DBFunctions.SingleData("SELECT REMARKS FROM sysibm.SYSTABLES WHERE name='" + ds.DataSetName + "' ORDER BY NAME ASC;");
                if (nombre == "") nombre = ds.DataSetName + "_" + num.Next(0, 9999);
                else nombre += "_" + num.Next(0, 9999);

                string urlImagen = "http://ecas.co/images/" + GlobalData.getEMPRESA() + ".png";

                string mensajeExcel =
                        @"<div style='position: absolute; background-color:#EEEFD9;width: 35%;border-radius: 10px;margin: auto;padding: 20px;box-shadow: 1px 7px 9px #888888;'>
	                	    <img style='width: 20%; position: absolute; right: 2%;' src='" + urlImagen + @"' /><br><br>
		                    <b><font size='5'>Excel Generado:</font></b>
		                    <br>" + nombre.Split('_')[0] +
                           @"<br><br>
		                    <b>Reciba un cordial saludo</b>, <br>
		                    Ha recibido un Excel usando el Sistema Ecas <br>
                            Dicho Excel se encuentra disponible como archivo <br>
                            adjunto en este correo.
		                    <br><br>
	                        <b>Gracias por su atención.</b>
		                    <br>
		                    <i>eCAS-AMS.</i>
	                    </div>
                        <br><br>";

                DataTable dt = dsExcel.Tables[0];
                for(int i = 0; i < dt.Columns.Count; i++)
                {
                    string nom = dt.DataSet.Tables[0].Columns[i].ColumnName;

                    string columnName = "" + DBFunctions.SingleData("SELECT REMARKS FROM SYSIBM.SYSCOLUMNS WHERE TBname='" + ds.DataSetName + "' AND NAME = '" + nom + "';");

                    if (columnName == "")
                    {
                        dt.Columns[i].ColumnName = nom;
                    }
                    else
                    {
                        dt.Columns[i].ColumnName = columnName;
                    }
                }
                //Set DataTable Name which will be the name of Excel Sheet.

                dt.TableName = ds.DataSetName;
                //Create a New Workbook.

                XLWorkbook wb = new XLWorkbook();

                //Add the DataTable as Excel Worksheet.

                wb.Worksheets.Add(dt);



                MemoryStream memoryStream = new MemoryStream();

                //Save the Excel Workbook to MemoryStream.

                wb.SaveAs(memoryStream);



                //Convert MemoryStream to Byte array.

                byte[] bytes = memoryStream.ToArray();

                memoryStream.Close();
                try
                {
                    if(correo != "" && correo != null)
                    {
                        enviarMail(correo, "Ha recibido un Reporte Excel de " + empresa, mensajeExcel, TipoCorreo.HTML, bytes);
                        Utils.MostrarAlerta(Response, "Email enviado satisfactoriamente a: " + correo);
                    }
                    else
                    {
                        enviarMail(tbMail.Text, "Ha recibido un Reporte Excel de " + empresa, mensajeExcel, TipoCorreo.HTML, bytes);
                        Utils.MostrarAlerta(Response, "Email enviado satisfactoriamente a: " + tbMail.Text);
                    }

                    //Response.Redirect(indexPage + "?process=DBManager.Selects&table=" + ds.DataSetName);
                }
                catch(Exception z)
                {
                    lblResult.Text = z.Message;
                }
                
    
            }
        }

        
        /*
            Reportes
        */
        public static string enviarMail(string to, string subject, string body, TipoCorreo bodyFormat, string rutaArchivo)
        {
            string rta = "";
            string from = ConfigurationManager.AppSettings["EmailFrom"];
            string SMTPserver = ConfigurationManager.AppSettings["MailServer"];
            int SMTPport = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
            string password = ConfigurationManager.AppSettings["PasswordEMail"];
            
            try
            {
                SmtpClient client = new SmtpClient(SMTPserver, SMTPport);
                client.EnableSsl = true; //comentar para caso normal. quitar comment para gmail.
                client.UseDefaultCredentials = false; //comentar caso normal. quitar comment para gmail.
                client.Credentials = new NetworkCredential(from, password);
                //client.Timeout = 10000;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage oMsg = new MailMessage(from, to, subject, body);
                oMsg.IsBodyHtml = bodyFormat == TipoCorreo.HTML;
                oMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                //aqui hay que haer un for para recorrer un arreglo nuevo de ruta archivo (',')
                if (rutaArchivo != "")
                    oMsg.Attachments.Add(new Attachment(rutaArchivo));

                client.Send(oMsg);
                client.Dispose();
            }
            catch (Exception e)
            {
                rta = String.Format("Error al enviar el correo [From:{0};To:{1};SMTP:{2}:{3}] excepcion: {4}",
                    from, to, SMTPserver, SMTPport, e.Message);
            }
            return rta;

        }

        /*
            Excel
        */
        public static int enviarMail(string to, string subject, string body, TipoCorreo bodyFormat, byte[] bytes)
        {
            string tipoArchivo = "";
            string from = ConfigurationManager.AppSettings["EmailFrom"];
            string SMTPserver = ConfigurationManager.AppSettings["MailServer"];
            int SMTPport = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
            string password = ConfigurationManager.AppSettings["PasswordEMail"];
            int resultado = 0;

            try
            {
                SmtpClient client = new SmtpClient(SMTPserver, SMTPport);
                client.EnableSsl = true; //comentar para caso normal. quitar comment para gmail.
                client.UseDefaultCredentials = false; //comentar caso normal. quitar comment para gmail.
                client.Credentials = new NetworkCredential(from, password);

                MailMessage oMsg = new MailMessage(from, to, subject, body);
                oMsg.IsBodyHtml = bodyFormat == TipoCorreo.HTML;
                oMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                //if(dsExcel != null && dsExcel.Tables[0].Rows.Count > 0)
                //{
                //    tipoArchivo = "GridView.xlsx";
                //}
                //else
                //{
                //    tipoArchivo = "imgAdjunta.png";
                //}
                oMsg.Attachments.Add(new Attachment(new MemoryStream(bytes), "GridView.xlsx"));
                client.Send(oMsg);
                client.Dispose();
                resultado = 1;
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Error al enviar el correo [From:{0};To:{1};SMTP:{2}:{3}] excepcion: {4}",
                    from, to, SMTPserver, SMTPport, e.Message));
            }
            return resultado;

        }

        /*
            imagenes
        */
        public int enviarMail(string to, string subject, string body, TipoCorreo bodyFormat, ArrayList listaBytes, ArrayList listaNombres)
        {
            string from = ConfigurationManager.AppSettings["EmailFrom"];
            string SMTPserver = ConfigurationManager.AppSettings["MailServer"];
            int SMTPport = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
            string password = ConfigurationManager.AppSettings["PasswordEMail"];
            int resultado = 0;

            try
            {
                SmtpClient client = new SmtpClient(SMTPserver, SMTPport);
                client.EnableSsl = true; //comentar para caso normal. quitar comment para gmail.
                client.UseDefaultCredentials = false; //comentar caso normal. quitar comment para gmail.
                client.Credentials = new NetworkCredential(from, password);

                MailMessage oMsg = new MailMessage(from, to, subject, body);
                oMsg.IsBodyHtml = bodyFormat == TipoCorreo.HTML;
                oMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if(listaNombres != null)
                {
                    for (int i = 0; i < listaBytes.Count; i++)
                    {
                        if (listaNombres[i].ToString().EndsWith(".png") || listaNombres[i].ToString().EndsWith(".PNG"))
                            oMsg.Attachments.Add(new Attachment(new MemoryStream((byte[])listaBytes[i]), listaNombres[i].ToString() + ".png"));
                        if (listaNombres[i].ToString().EndsWith(".jpg") || listaNombres[i].ToString().EndsWith(".JPG"))
                            oMsg.Attachments.Add(new Attachment(new MemoryStream((byte[])listaBytes[i]), listaNombres[i].ToString() + ".jpg"));
                    }
                }
                else
                {
                    for (int i = 0; i < listaBytes.Count; i++)
                    {
                        if (listaNombres[i].ToString().EndsWith(".png") || listaNombres[i].ToString().EndsWith(".PNG"))
                            oMsg.Attachments.Add(new Attachment(new MemoryStream((byte[])listaBytes[i]), "pngimagen_" + i + ".png"));
                        if (listaNombres[i].ToString().EndsWith(".jpg") || listaNombres[i].ToString().EndsWith(".JPG"))
                            oMsg.Attachments.Add(new Attachment(new MemoryStream((byte[])listaBytes[i]), "jpgimagen_" + i + ".jpg"));
                    }
                }
                
                client.Send(oMsg);
                client.Dispose();
                resultado = 1;
            }
            catch (Exception e)
            {
                resultado = 0;
                throw new Exception(String.Format("Error al enviar el correo [From:{0};To:{1};SMTP:{2}:{3}] excepcion: {4}",
                    from, to, SMTPserver, SMTPport, e.Message));
                
            }
            return resultado;

        }


        public void imprimeExcel()
        {
            Random num = new Random();
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            ds = dsExcel;
            string ruta = ConfigurationManager.AppSettings["PathToReports"];
            string nombre = "" + DBFunctions.SingleData("SELECT REMARKS FROM sysibm.SYSTABLES WHERE name='" + ds.DataSetName + "' ORDER BY NAME ASC;") + "_" + num.Next(0, 9999);
            try
            {
                if (nombre == "") nombre = ds.DataSetName + "_" + num.Next(0, 9999);

                Encoding encoding = Encoding.UTF8;
                DataTable dt = ds.Tables[0];
                string attachment = "attachment; filename=" + nombre + ".xlsx";
                response.Charset = encoding.EncodingName;
                response.ContentEncoding = Encoding.Unicode;
                response.ClearContent();
                //sourceFile.Close();
                response.Buffer = false;
                response.BufferOutput = false;
                response.AddHeader("content-disposition", attachment);
                response.AddHeader("Content-Type", "application/Excel");
                response.ContentType = "application/vnd.xlsx";

                //Validamos que el nombre no exista ya, y si existe generamos otro aleatorio
                string filePath = verificaNombre(nombre);

                string tab = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    string nom = dc.ColumnName;
                    
                    string columnName = "" + DBFunctions.SingleData("SELECT REMARKS FROM SYSIBM.SYSCOLUMNS WHERE TBname='" + ds.DataSetName + "' AND NAME = '" + nom + "';");
                    
                    if (columnName == "")
                    {
                        response.Write(tab + nom);
                    }
                    else
                    {
                        response.Write(tab + columnName);
                    }
                    
                    tab = "\t";
                }
                response.Write("\n");
                int i;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (i = 0; i < dt.Columns.Count; i++)
                    {
                        response.Write(tab + dr[i].ToString());
                        tab = "\t";
                    }
                    response.Write("\n");

                }
                response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                response.Flush();
                Response.Close();
                response.End();
            }
            
            catch(Exception i)
            {
                lblResult.Text = "No se pudo descargar el archivo. - razón: " + i.Message;
                
            }
            
        }

        public String verificaNombre(string nombre)
        {
            Random num = new Random();
            string filePath = string.Format("{0}/{1}", "Downloads\\" , @"" + nombre + ".xls");
            string compFilePath = filePath.Replace("\\/", "\\");

            if (System.IO.File.Exists(compFilePath))
            {
                System.IO.File.Delete(filePath);
                nombre = ds.DataSetName + "_" + num.Next(10000, 15000);
                string resultado = verificaNombre(nombre);
                return resultado;
            }
            else
            {
                return nombre;
            }
        }

    }
}