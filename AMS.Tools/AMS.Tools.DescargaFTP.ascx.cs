using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;

namespace AMS.Tools
{
    public partial class AMS_Tools_DescargaFTP : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarArchivosFTP();
        }

        protected void CargarArchivosFTP()
        {
            // Get the object used to communicate with the server.
            string ftpPath = DBFunctions.SingleDataGlobal("select gemp_ftp from gempresa where gemp_nombre = '" + GlobalData.EMPRESA.ToLower() + "'");

            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp:// ams. ecas. co/" + GlobalData.EMPRESA + "/");
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath + GlobalData.EMPRESA);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            //request.Credentials = new NetworkCredential("aycodb", "janeDoe@contoso.com");
            request.Credentials = new NetworkCredential("aycodb", "4YC02016.");

            //request.UseBinary = false;
            //request.UsePassive = true;
            
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            List<string> directories = new List<string>();

            var result = new StringBuilder();

            string line = reader.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                directories.Add(line);

                result.Append(line);
                result.Append("\n");

                line = reader.ReadLine();
            }

            DataTable dtFiles = new DataTable();

            dtFiles.Columns.Add(new DataColumn("FECHA"));
            dtFiles.Columns.Add(new DataColumn("HORA"));
            dtFiles.Columns.Add(new DataColumn("NOMBRE"));
            dtFiles.Columns.Add(new DataColumn("TAMANO"));

            if (result.Length > 0)
            {
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                String[] results = result.ToString().Split('\n');
                
                for (int k = 0; k < results.Length; k++)
                {
                    DataRow dr = dtFiles.NewRow();
                    String[] fileInfo = results[k].Split(' ');
                    string fecha = fileInfo[0];
                    string hora = fileInfo[2];
                    string nombre = fileInfo[fileInfo.Length - 1];
                    double peso = Math.Round(Convert.ToDouble(fileInfo[fileInfo.Length - 2]) / 1000, 0); //en Kilobytes

                    dr[0] = fecha;
                    dr[1] = hora;
                    dr[2] = nombre;
                    dr[3] = peso + " KB";

                    dtFiles.Rows.Add(dr);
                }

                dtFiles.DefaultView.Sort = "FECHA desc";
                DataView dv = dtFiles.DefaultView;
                dv.Sort = "FECHA desc";
                DataTable sortedDT = dv.ToTable();

                dgFilesFTP.DataSource = sortedDT;
                dgFilesFTP.DataBind();

                reader.Close();
                response.Close();
            }
            else
            {
                Utils.MostrarAlerta(Response, "El directorio FTP esta vacio.");
                dgFilesFTP.DataSource = dtFiles;
                dgFilesFTP.DataBind();
            }
        }

        protected void DescargarArchivo(object sender, DataGridCommandEventArgs e)
        {
            string fileName = ((System.Web.UI.DataBoundLiteralControl)dgFilesFTP.Items[e.Item.ItemIndex].Cells[2].Controls[0]).Text.Trim();
            string ftpPath = DBFunctions.SingleDataGlobal("select gemp_ftp from gempresa where gemp_nombre = '" + GlobalData.EMPRESA.ToLower() + "'");
            String RemoteFtpPath = ftpPath + GlobalData.EMPRESA + "/" + fileName;

            if (e.CommandName == "Descargar")
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential("aycodb", "4YC02016.");
                request.UseBinary = true;

                //Create a stream for the file
                Stream stream = null;

                //This controls how many bytes to read at a time and send to the client
                int bytesToRead = 504800; // 504 MB

                // Buffer to read bytes in chunk size specified above
                byte[] buffer = new Byte[bytesToRead];

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        using (stream = response.GetResponseStream())
                        {
                            // prepare the response to the client. resp is the client Response
                            var resp = HttpContext.Current.Response;

                            //Name the file 
                            resp.Clear();
                            resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                            resp.AddHeader("Content-Length", response.ContentLength.ToString());
                            resp.ContentType = "application /octet-stream";  //Indicate the type of data being sent

                            int length;
                            do
                            {
                                // Verify that the client is connected.
                                if (resp.IsClientConnected)
                                {
                                    // Read data into the buffer.
                                    length = stream.Read(buffer, 0, bytesToRead);

                                    // and write it out to the response's output stream
                                    resp.OutputStream.Write(buffer, 0, length);

                                    // Flush the data
                                    resp.Flush();

                                    //Clear the buffer
                                    buffer = new Byte[bytesToRead];
                                }
                                else
                                {
                                    // cancel the download if client has disconnected
                                    length = -1;
                                }
                            } while (length > 0); //Repeat until no data is read

                            if (stream != null)
                            {
                                //Close the input stream
                                stream.Close();
                            }

                            resp.Close();
                        }
                    }
                }
                catch(Exception er)
                {
                    Utils.MostrarAlerta(Response, "Error en el servidor FTP. Consultar al administrador del Sistema.");
                }
            }
            else if(e.CommandName == "Eliminar")
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential("aycodb", "4YC02016.");

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine("Delete status: {0}", response.StatusDescription);
                response.Close();

                Utils.MostrarAlerta(Response, "Archivo eliminado correctamente del servidor.");

                CargarArchivosFTP();
            }
            

            //Version con escogencia de archivo desde una direccion URL---------------------------
            ////Create a stream for the file
            //Stream stream = null;
            ////This controls how many bytes to read at a time and send to the client
            //int bytesToRead = 504800;
            //// Buffer to read bytes in chunk size specified above
            //byte[] buffer = new Byte[bytesToRead];
            // The number of bytes read
            //try
            //{
                ////Create a WebRequest to get the file
                //HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);
                ////Create a response for this request
                //HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();
                //if (fileReq.ContentLength > 0)
                //    fileResp.ContentLength = fileReq.ContentLength;
                //Get the Stream returned from the response
                //stream = fileResp.GetResponseStream();
                // prepare the response to the client. resp is the client Response
                //var resp = HttpContext.Current.Response;
                //Indicate the type of data being sent
                // resp.ContentType = "application/octet-stream";
                //Name the file 
                //resp.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
                //resp.AddHeader("Content-Length", fileResp.ContentLength.ToString());
            //    int length;
            //    do
            //    {
            //        // Verify that the client is connected.
            //        if (resp.IsClientConnected)
            //        {
            //            // Read data into the buffer.
            //            length = stream.Read(buffer, 0, bytesToRead);
            //            // and write it out to the response's output stream
            //            resp.OutputStream.Write(buffer, 0, length);
            //            // Flush the data
            //            resp.Flush();
            //            //Clear the buffer
            //            buffer = new Byte[bytesToRead];
            //        }
            //        else
            //        {
            //            // cancel the download if client has disconnected
            //            length = -1;
            //        }
            //    } while (length > 0); //Repeat until no data is read
            //}
            //finally
            //{
            //    if (stream != null)
            //    {
            //        //Close the input stream
            //        stream.Close();
            //    }
            //}
            
            //Version de descarga indicando manualmente path de descarga.
            //string ass = FileUploader.FileName;
            //String RemoteFtpPath = "ftp://ams.ecas.co/AYCO/jaguar.jpg";
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
            //request.Method = WebRequestMethods.Ftp.DownloadFile;
            //request.Credentials = new NetworkCredential("aycodb", "4YC02016.");
            //request.UseBinary = true;
            //using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            //{
            //    using (Stream rs = response.GetResponseStream())
            //    {
            //        using (FileStream ws = new FileStream("jaguar.jpg", FileMode.Create))
            //        {
            //            byte[] buffer = new byte[504800];
            //            int bytesRead = rs.Read(buffer, 0, buffer.Length);
            //            while (bytesRead > 0)
            //            {
            //                ws.Write(buffer, 0, bytesRead);
            //                bytesRead = rs.Read(buffer, 0, buffer.Length);
            //            }
            //        }
            //    }
            //}
        }


    }
}