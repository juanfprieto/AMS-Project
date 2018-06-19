using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using AMS.DB;
using System.Collections;
using AMS.Forms;
using System.Text.RegularExpressions;
using System.Text;
using System.Data;
using System.Configuration;
using AMS.CriptoServiceProvider;
using System.Net.Mail;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using System.IO;
using System.Web.UI.HtmlControls;

namespace AMS.Tools
{
    public class Utils
    {
        private static DatasToControls bind = new DatasToControls();
        private static Regex regexNumero = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
        private static Regex regexEntero = new Regex(@"^[-+]?[0-9]+$");
        private static Regex regexFecha = new Regex(@"^(19|20)\d\d-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[01])$");
        private static Regex regexMail = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

        public static bool llenarPrefijos(HttpResponse response, ref DropDownList ddl, string proceso, string almacen, string tipoDocu)
        {
            string sql = "";

            if (proceso == "%" && almacen == "%")
                sql = String.Format("SELECT DISTINCT p.pdoc_codigo, " +
                              "       p.pdoc_codigo || '  -  ' || p.pdoc_nombre, " +
                              "       p.pdoc_numefina - p.pdoc_ultidocu as RESTANTES, p.pdoc_nombre, p.pdoc_finfechresofact, p.pdoc_fechhabi  " +
                              "FROM pdocumento AS P " +
                              "WHERE TRIM(p.tdoc_tipodocu) LIKE '{0}' " +
                              "AND   p.TVIG_VIGENCIA='V'  ORDER BY P.PDOC_NOMBRE", // se puede mandar "%" como parámetro para todos los tipos
                              tipoDocu);
            else
                sql = String.Format("SELECT DISTINCT p.pdoc_codigo, " +
                              "       p.pdoc_codigo || '  -  ' || p.pdoc_nombre, " +
                              "       p.pdoc_numefina - p.pdoc_ultidocu as RESTANTES, p.pdoc_nombre, p.pdoc_finfechresofact, p.pdoc_fechhabi   " +
                              "  FROM pdocumento AS P, " +
                              "       pdocumentohecho AS PH " +
                              " WHERE TRIM(p.tdoc_tipodocu) LIKE '{0}' " + // se puede mandar "%" como parámetro para todos los tipos
                              " AND   TRIM(PH.tpro_proceso) LIKE '{1}' " + // se puede mandar "%" como parámetro para todos los procesos
                              " AND   P.PDOC_CODIGO = PH.PDOC_CODIGO " +
                              " AND   TRIM(ph.palm_almacen) LIKE '{2}' " + // se puede mandar "%" como parámetro para todos los almacenes
                              " AND   p.TVIG_VIGENCIA='V' ORDER BY P.PDOC_NOMBRE",
                              tipoDocu,
                              proceso,
                              almacen);

            FillDll(ddl, sql, true);
            ArrayList prefijos = DBFunctions.RequestAsCollection(sql);

            if (ddl.Items.Count == 0 && almacen != "0" && !almacen.ToUpper().Contains("SELECCIONE")) //almacen en "seleccione..."
            {
                string mensaje = "Por favor parametrice documentos del tipo: ";
                if (tipoDocu != "%")
                {
                    string tipoDocuStr = DBFunctions.SingleData(String.Format(
                        "SELECT TDOC_TIPODOCU || ' - ' || TDOC_NOMBDOCU FROM TDOCUMENTO WHERE TDOC_TIPODOCU = '{0}'", tipoDocu));
                    mensaje += String.Format("de tipo {0} (Parametros Generales/Documentos) ", tipoDocuStr);
                }
                if (proceso != "%")
                {
                    string procesoStr = DBFunctions.SingleData(String.Format(
                        "SELECT TPRO_PROCESO || ' - ' || TPRO_NOMBRE  FROM TPROCESODOCUMENTO WHERE TPRO_PROCESO='{0}'", proceso));
                    mensaje += String.Format("asociados al área de proceso {0} ", procesoStr);
                }
                if (almacen != "%")
                {
                    string almacenStr = DBFunctions.SingleData(String.Format(
                        "SELECT PALM_ALMACEN  || ' - ' || PALM_DESCRIPCION FROM PALMACEN WHERE PALM_ALMACEN = '{0}'", almacen));
                    mensaje += String.Format("y almacen {0} ", almacenStr);
                }

                mensaje += "(Parametros Generales/Documentos Asociados)";
                MostrarAlerta(response, mensaje);
            }

            string msjConsecutivos = "";
            foreach(Hashtable prefijo in prefijos)
            {
                int consecutivosRestantes = Convert.ToInt32(prefijo["RESTANTES"]);
                string documento = prefijo["PDOC_CODIGO"].ToString();

                if (consecutivosRestantes <= 0)
                    msjConsecutivos += String.Format("El prefijo {0} está fuera de rango en los consecutivo en {1}. \n ", documento, consecutivosRestantes);
                else
                {
                    if (consecutivosRestantes <= 5 || consecutivosRestantes == 25 || consecutivosRestantes == 50 || consecutivosRestantes == 100)
                        msjConsecutivos += String.Format("El prefijo {0} está a {1} consecutivos de llegar a su fin. \n ", documento, consecutivosRestantes);
                }
                DateTime fechaHoy  = DateTime.Now;
                DateTime fechaReso = DateTime.Now.AddYears(-1); // se predefine la fecha de 1 menos 1 año
                if(prefijo["PDOC_FINFECHRESOFACT"].ToString() != "")
                    fechaReso = Convert.ToDateTime(prefijo["PDOC_FINFECHRESOFACT"]);
                DateTime fechaHabi = DateTime.Now.AddYears(-1); // se predefine la fecha de 1 menos 1 año
                if(prefijo["PDOC_FECHHABI"].ToString() != "")
                    fechaHabi = Convert.ToDateTime(prefijo["PDOC_FECHHABI"]);
                if(tipoDocu == "FC" && ((fechaHoy > fechaReso && fechaHoy > fechaHabi) || prefijo["PDOC_FINFECHRESOFACT"] == null))
                    msjConsecutivos += String.Format("Las fechas de Resolución y Habilitación par el prefijo {0} están fuera de rango. \n ", documento);
            }
            if (msjConsecutivos != "")
            {
                msjConsecutivos += "Por favor revise";
                MostrarAlerta(response, msjConsecutivos);
            }
         return ddl.Items.Count > 0;
        }

        public static void FillDll(DropDownList ddl, string sql, bool tieneSeleccione)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);

                ddl.DataSource = ds;
                if (ds.Tables[0].Columns.Count > 1)
                    ddl.DataTextField = ds.Tables[0].Columns[1].ColumnName;
                else
                    ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
                ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;

                try
                {
                    ddl.DataBind();

                    if (tieneSeleccione && ddl.Items.Count > 1)
                        ddl.Items.Insert(0, new ListItem("Seleccione...", "0"));
                }
                catch { }
            }
            catch { }

            ds.Clear();
        }

        public static void FillDll(DropDownList ddl, string sql, string valorPorDefecto, bool tieneSeleccione)
        {
            FillDll(ddl, sql, tieneSeleccione);
            try
            {
                ddl.SelectedValue = valorPorDefecto;
            }
            catch { }
        }

        public static void FillDll(DropDownList ddl, ArrayList arr, bool tieneSeleccione)
        {
            ddl.DataSource = arr;
            ddl.DataValueField = "Key";
            ddl.DataTextField = "Value";

            try
            {
                ddl.DataBind();

                if (tieneSeleccione && ddl.Items.Count > 1)
                    ddl.Items.Insert(0, new ListItem("Seleccione...", "0"));
            }
            catch { }
        }

        public static bool EstaSeleccionado(DropDownList ddl)
        {
            return ddl.SelectedValue != "0" && !ddl.SelectedItem.Text.ToUpper().Contains("SELECCIONE");
        }

        public static bool EsNumero(string str)
        {
            if (str != "" && regexNumero.IsMatch(str))
                return true;
            else
                return false;
        }

        public static bool EsEntero(string str)
        {
            if (str != "" && regexEntero.IsMatch(str))
                return true;
            else
                return false;
        }

        public static bool EsFecha(string str)
        {
            if (str != "" && regexFecha.IsMatch(str))
                return true;
            else
                return false;
        }

        public static bool EsMail(string str)
        {
            if (str != "" && regexMail.IsMatch(str))
                return true;
            else
                return false;
        }

        public static string MultiplicarString(string source, int multiplier)
        {
            StringBuilder sb = new StringBuilder(multiplier * source.Length);
            for (int i = 0; i < multiplier; i++)
            {
                sb.Append(source);
            }

            return sb.ToString();
        }

        public static bool logger(string tabla, TipoLog tipo, string datosOriginales)
        {
            string usuario = HttpContext.Current.User.Identity.Name;
            string timesamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string tipoStr = tipo == TipoLog.Delete ? "D" : "U";

            string sql = String.Format(
                "INSERT INTO mhistorial_cambios " +
                 "VALUES " +
                 "( " +
                 "  DEFAULT, " +
                 "  '{0}', " +
                 "  '{1}', " +
                 "  '{2}', " +
                 "  '{3}', " +
                 "  '{4}' " +
                 ")",
                 tabla,
                 tipoStr,
                 datosOriginales,
                 usuario,
                 timesamp);

            return DBFunctions.NonQuery(sql) == 1;
        }

        public static bool logger(string tabla, TipoLog tipo, Hashtable datosOriginales)
        {
            if (datosOriginales == null) return true;

            string usuario = HttpContext.Current.User.Identity.Name;
            string timesamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string tipoStr = tipo == TipoLog.Delete ? "D" : "U";

            string datosOriginalesStr = "";
            foreach (DictionaryEntry entry in datosOriginales)
                datosOriginalesStr += String.Format("{0}={1}, ", entry.Key, entry.Value.ToString().Replace("'", ""));

            string sql = String.Format(
                "INSERT INTO mhistorial_cambios " +
                 "VALUES " +
                 "( " +
                 "  DEFAULT, " +
                 "  '{0}', " +
                 "  '{1}', " +
                 "  '{2}', " +
                 "  '{3}', " +
                 "  '{4}' " +
                 ")",
                 tabla,
                 tipoStr,
                 datosOriginalesStr,
                 usuario,
                 timesamp);

            return DBFunctions.NonQuery(sql) == 1;
        }

        public static bool HashtableEquals(Hashtable a, Hashtable b)
        {
            if (a == b) return true;
            if (a == null && b != null) return false;
            if (a != null && b == null) return false;
            if (a.Count != b.Count) return false;

            bool equals = true;
            foreach (DictionaryEntry entry in a)
            {
                equals &= b.ContainsKey(entry.Key);
                equals &= entry.Value.Equals(b[entry.Key]);
                if (!equals) return equals;
            }

            return equals;
        }

        public static unsafe bool ByteArrayEquals(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return false;
            fixed (byte* p1=a1, p2=a2)
            {
                byte* x1=p1, x2=p2;
                int l = a1.Length;
                for (int i=0; i < l / 8; i++, x1 += 8, x2 += 8)
                    if (*((long*)x1) != *((long*)x2)) return false;
                if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;
                return true;
            }
        }

        public static bool IsNullOrEmpty(object o)
        { 
            try
            {
                string str = o.ToString();
                return str == null || str == "";
            }
            catch
            {
                return true;
            }
        }
        //1 si realizó el proceso correctamente
        public static int EnviarMail(string to, string subject, string body, TipoCorreo bodyFormat, string rutaArchivo)
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
                //client.Timeout = 10000;
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                MailMessage oMsg = new MailMessage(from, to, subject, body);
                oMsg.IsBodyHtml = bodyFormat == TipoCorreo.HTML;
                oMsg.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                if (rutaArchivo != "")
                    oMsg.Attachments.Add(new Attachment(rutaArchivo));

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

        public static string CompletarCampos(string cadena, int longitud, string relleno, bool alineadoIzq)
        {
            int cont;
            int cadenalong=cadena.Length;
            int i=0;
            //si la cadena es menor habra que completar con el relleno q nos manden.
            if (cadenalong < longitud)
            {
                //si la justificacion es a la izquierda completo hacia la derecha.
                cont = longitud - cadenalong;
                if (alineadoIzq)
                {
                    for (i = 0; i < cont; i++)
                    {
                        cadena += relleno;
                    }
                }

                //si la justificacion es a la derecha completo hacia la izquierda.
                else
                {
                    string cadena2=cadena;
                    string rellenoini="";
                    for (i = 0; i < cont; i++)
                    {
                        rellenoini += relleno;
                    }
                    cadena = "";
                    cadena = rellenoini + cadena2;
                }

            }
            //si la cadena es mayor habra que truncar.
            if (cadenalong > longitud)
            {
                string cadena3="";
                for (i = 0; i < longitud; i++)
                {
                    cadena3 += cadena[i].ToString();
                }		
                cadena = cadena3;
            }

            return cadena;
        }

        public static void MostrarAlerta(HttpResponse response, string mensaje)
        {
            string script = String.Format("<script language:javascript>alert('{0}');</script>", mensaje);
            response.Write(script);
        }

        public static void MostrarPregunta(HttpResponse response, string mensaje)
        {
            //string script = "<script language:javascript>" +
            //   " if (confirm('"+ mensaje + "')) {" +
            //   "     __doPostBack('', 'YES');" +
            //   " } else {" +
            //   "     __doPostBack('', 'NO')" +
            //   " }" +
            //   " </script>";
            //response.Write(script);
        }

        public static void MostrarPreguntaRespuesta(HttpResponse response, string mensaje)
        {
            string prompt = "<script type='text/javascript'>\n" +
                   "var value = prompt(\"What is your name\", \"Type you name here\"); \n" +
                   "abrir(value);" +
                    "</script>";
            response.Write(prompt);
        }


        public static string ToVarchar(string data)
        {
            return String.Format("'{0}'", data);
        }

        public static DateTime ParseDate(string sqlDate)
        {
            if (!EsFecha(sqlDate))
                throw new Exception("Fecha en formato Incorrecto");

            string[] splited = sqlDate.Split('-');
            int year = Convert.ToInt32(splited[0]);
            int month = Convert.ToInt32(splited[1]);
            int day = Convert.ToInt32(splited[2]);
            
            return new DateTime(year, month, day);
        }

        public static Bitmap ResizeImage(Bitmap original, int iWidth, int iHeight)
        {
            try
            {
                Size size = new Size(iWidth, iHeight);

                int sourceWidth = original.Width;
                int sourceHeight = original.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    nPercent = nPercentH;
                else
                    nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap b = new Bitmap(destWidth, destHeight);
                Graphics g = Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(original, 0, 0, destWidth, destHeight);
                g.Dispose();

                return b;
            }
            catch
            {
                throw;
            }
        }
        
        //Recibe un DataSet y String con el nombre que se pondra al archivo .xlsx  A continuacion lo descarga en el Cliente.
        public static void ImprimeExcel(DataSet ds, string nombre)
        {
            DataTable dt = ds.Tables[0];
            Random num = new Random();
            string ruta = ConfigurationManager.AppSettings["PathToReports"];
            DateTime fecha = DateTime.Now;
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;

            using (XLWorkbook wb = new XLWorkbook())
            {
                ////-----------------Descomentar para Rastrar Caracteres Hexadecimal---------------------------------------------------------------
                string error;
                int fallos = 0;
                //Validamos valores hexadecimales. (Detector de columan con hexadecimal)
                bool exit = false;
                for (int z = 0; z < dt.Rows.Count /*&& exit == false*/; z++) //dt.Rows.Count
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        try
                        {
                            if (haveHexadec((string)dt.Rows[z][i].ToString()))
                            {
                                Utils.MostrarAlerta(response, "el HEXA error ocurre en la columna: " + i + "y la fila: " + z);
                                fallos++;
                                exit = true;
                            }
                        }
                        catch (Exception w)
                        {
                            string a = w.Message;
                        }

                    }
                    //if (haveHexadec((string)dt.Rows[z][4]))
                    //{
                    //    Utils.MostrarAlerta(response, "el HEXA error ocurre en la columna: " + 1 + "y la fila: " + z);
                    //    fallos++;
                    //    exit = true;
                    //}
                    ////for (int i = 0; i < dt.Columns.Count; i++) 
                    //for (int i = 0; i < dt.Columns.Count; i++)
                    //{
                    //    try
                    //    {
                    //        if (haveHexadec((string)dt.Rows[z][i]))
                    //        {
                    //            Utils.MostrarAlerta(response, "el HEXA error ocurre en la columna: " + i + "y la fila: " + z);
                    //            fallos++;
                    //            exit = true;
                    //        }
                    //    }
                    //    catch (Exception y)
                    //    {
                    //        //Utils.MostrarAlerta(response, "el error NULL ocurre en la columna: " + i + "y la fila: " + z);
                    //    }
                    //}
                }
                if (exit)
                    return;
                ////-------------------------------------------------------------------------------------------------------------------

                wb.Worksheets.Add(dt, nombre);

                Encoding encoding = Encoding.UTF8;
                response.Clear();
                response.Charset = encoding.EncodingName;
                response.ContentEncoding = Encoding.Unicode;
                response.ClearContent();
                response.Buffer = false;
                response.BufferOutput = false;

                nombre += "_" + fecha.ToShortDateString() + "_" + num.Next(0, 999);
                response.AddHeader("content-disposition", "attachment;filename=" + nombre + ".xlsx");
                //response.ContentType = "application/vnd.openxmlformats";  // PARA XLSX
                response.ContentType = "application/vnd.ms-excel";   // PARA XLS
                MemoryStream memoryStream = new MemoryStream();
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(response.OutputStream);
                //response.Flush();
                response.End();
            }
        }

        //Recibe un objeto de Archivo HTMLInpuTfile (Un excel),y el nombre de la tabla definido en el espacio de nombres del excel.
        //Retorna el dataSet con el contenido, en caso contrario retorna null.
        public static DataSet CargaExcel(HtmlInputFile archivoExcel, string tablaEspacioNombreExcel)
        {
            DataSet dsExcel = new DataSet();
            Random num = new Random();

            if (archivoExcel.PostedFile.FileName.ToString() == string.Empty)
            {
                return null;
            }
            else
            {
                try
                {
                    string[] file = archivoExcel.PostedFile.FileName.ToString().Split('\\');
                    string fileName = file[file.Length - 1];
                    string[] fileNameParts = fileName.Split('.');
                    string fileNameAux = fileNameParts[fileNameParts.Length - 2];
                    string fileExtension = fileNameParts[fileNameParts.Length - 1];

                    if (fileExtension.ToUpper() != "XLS" && fileExtension.ToUpper() != "XLSX")
                    {
                        return null;
                    }
                    else
                    {
                        int numero = num.Next(0, 9999);
                        archivoExcel.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileNameAux + numero + "." + fileExtension);
                        ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + fileNameAux + numero + "." + fileExtension);
                        bool leiArchivo = false;
                        try
                        {
                            exc.Request(dsExcel, IncludeSchema.NO, "SELECT * FROM " + tablaEspacioNombreExcel);

                            if (dsExcel.Tables[0].Rows.Count == 0)
                            {
                                return null;
                            }
                            else
                            {
                                for (int i = 0; i < dsExcel.Tables[0].Columns.Count; i++)
                                {
                                    dsExcel.Tables[0].Columns[i].ColumnName = dsExcel.Tables[0].Rows[0][i].ToString();
                                }
                                dsExcel.Tables[0].Rows[0].Delete();
                                dsExcel.Tables[0].Rows[0].AcceptChanges(); //Cierra la edición por lo que reajusta la tabla, sin esto, la tabla no se reorganiza.
                                leiArchivo = true;
                            }
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return dsExcel;
        }

            static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        static bool haveHexadec(string test)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            if (System.Text.RegularExpressions.Regex.IsMatch(test, @"[\x00-\x08\x0B\x0C\x0E-\x1F]"))
                return true;
            else return false;
            
            //return System.Text.RegularExpressions.Regex.IsMatch(test, @"\A\b[0-9a-fA-F]+\b\Z");
        }

        public static void MostrarPDF(HttpResponse response, string urlReporte, int width)
        {
            string script = String.Format(
                "<script language:javascript>w=window.open('../aspx/AMS.Web.ModalPDF.aspx?rpt=" + urlReporte.Replace("../rptgen/", "") + 
                "', '','HEIGHT=650,WIDTH=" + width + "');</script>");
            response.Write(script);
        }

    }

    public enum TipoLog 
    { 
        Update, 
        Delete
    }
}