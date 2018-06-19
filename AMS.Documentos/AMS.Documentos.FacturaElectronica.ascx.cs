using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using AMS.DB;
using AMS.Tools;
using System.Net;
using System.IO;

namespace AMS.Documentos
{
    public partial class FacturaElectronica:System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CallWebService();
        }
        protected void CallWebService()
            {
                string resultado="";
                XmlDocument xml=new XmlDocument();
                DataSet ds=new DataSet();
                ds=DBFunctions.Request(ds, IncludeSchema.NO, @"");
                //Defino la url de conexion y la accion de la peticion                
                var _url= "https://facturaelectronica.dian.gov.co/habilitacion/B2BIntegrationEngine/FacturaElectronica/facturaElectronica.wsdl";
                var _action="";

            //Verifico que el dataset tenga datos
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    Utils.MostrarAlerta(Response, ""No se han encontrado registros Revice por favor."");
            //    Label1.Text=""No se han encontrado registros revice Por favor."";
            //    Label1.ForeColor=System.Drawing.Color.Red;
            //    return;
            //}
            //else
            //{ //Se valida que la factura o la nota exista en MFACTURACLIENTE
            //    for (int i=0; i <ds.Tables[0].Rows.Count; i++)
            //    {
            //        if (ds.Tables[0].Rows[i][2].ToString() == """")
            //        {
            //            resultado += ""Se ha encontado que el Nit: "" + ds.Tables[0].Rows[i][0] + "" con la factura: "" + ds.Tables[0].Rows[i][1] + "" no esta registrada en el sitema, EL PROCESO SERA CANCELADO, revice por favor."" + ""<br>"";
            //        }
            //    }
            //    if (resultado != """")
            //    {
            //        Label1.ForeColor=System.Drawing.Color.Red;
            //        Label1.Text=resultado;
            //        return;
            //    }

            //}


            //for (int i=0; i <ds.Tables[0].Rows.Count; i++)
            //{
            //DataRow dsFila=ds.Tables[0].Rows[i];
            //DataRow dsFila=ds.Tables[0].Rows[0];
            XmlDocument soapEnvelopeXml=CreateSoapEnvelope();
            //XmlDocument soapEnvelopeXml=CreateSoapEnvelope(dsFila);
                    HttpWebRequest webRequest=CreateWebRequest(_url, _action);
                    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);


                    // inicia la llamada asíncrona a la solicitud web.
                    IAsyncResult asyncResult=webRequest.BeginGetResponse(null, null);


                    // suspende este subproceso hasta que se complete la llamada. 
                    asyncResult.AsyncWaitHandle.WaitOne();


                    // obtengo la respuesta de la solicitud web completada.
                    string soapResult;
            try
            {
                using (WebResponse webResponse=webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd=new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult=rd.ReadToEnd();
                        resultado += soapResult;
                        //resultado += soapResult + "<br>" + "Propietario: " + ds.Tables[0].Rows[i][0] + "  Placa: " + ds.Tables[0].Rows[i][1] + "  Sinisestro: " + ds.Tables[0].Rows[i][2] + "<br>";
                        //INSERT INTO MFACTURAELECTRONICA DEL SOAPRESULT
                    }
                }
            }
            catch (Exception e)
            {
                Label1.Text=Convert.ToString(e);
            };
                //}
                if (resultado.Contains("EXITO"))
                {
                    Label1.ForeColor=System.Drawing.Color.Green;
                    Label1.Text=resultado;
                }
                else
                {
                    Label1.ForeColor=System.Drawing.Color.Red;
                    Label1.Text += "Error en la transmisión. Revice sus datos.";
                }

            }
            private static HttpWebRequest CreateWebRequest(string url, string action)
            {
                //Se definen los parametros requeridos por el webService 
                HttpWebRequest webRequest=(HttpWebRequest)WebRequest.Create(url);
                webRequest.Headers.Add("SOAPAction", action);
                webRequest.ContentType= "text/plain";
                webRequest.Accept="text/xml";
                webRequest.Method="POST";
                webRequest.Host="facturaelectronica.dian.gov.co";
                return webRequest;
            }
            private static XmlDocument CreateSoapEnvelope()
            { //definicion del XML que se va a enviar.
                XmlDocument soapEnvelop=new XmlDocument();

                soapEnvelop.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:rep=""http://www.dian.gov.co/servicios/facturaelectronica/ReportarFactura"">
                                      <soap:Header>	     
                                      <wsse:Security xmlns:wsse=""http://schemas.xmlsoap.org/ws/2003/06/secext"">
                                       <wsse:UsernameToken wsu:Id=""sample"" xmlns:wsu=""http://schemas.xmlsoap.org/ws/2003/06/utility"">
                                        <wsse:Username>sample</wsse:Username>
                                        <wsse:Password Type=""wsse:PasswordText"">oracle</wsse:Password>
                                        <wsu:Created>2004-05-19T08:44:51Z</wsu:Created>
                                       </wsse:UsernameToken>
                                      </wsse:Security>
                                      <wsse:Security soap:actor=""oracle""
                                          xmlns:wsse=""http://schemas.xmlsoap.org/ws/2003/06/secext"">
                                       <wsse:UsernameToken wsu:Id=""oracle""
                                           xmlns:wsu=""http://schemas.xmlsoap.org/ws/2003/06/utility"">
                                        <wsse:Username>oracle</wsse:Username>
                                        <wsse:Password Type =""wsse:PasswordText"">oracle</wsse:Password>
                                        <wsu:Created>2004-05-19T08:46:04Z</wsu:Created>
                                       </wsse:UsernameToken>
                                      </wsse:Security>
                                     </soap:Header>
                                       <soapenv:Body>
                                          <rep:EnvioFacturaElectronicaPeticion>
                                             <rep:NIT>800081328</rep:NIT>
                                             <rep:InvoiceNumber>PRUE980000042</rep:InvoiceNumber>
                                             <rep:IssueDate>2017-05-17T13:30:57</rep:IssueDate>
                                             <rep:Document>cid:documento</rep:Document>
                                          </rep:EnvioFacturaElectronicaPeticion>
                                       </soapenv:Body>
                                    </soapenv:Envelope>");
                return soapEnvelop;
            }
            private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
            {
                    using (Stream stream=webRequest.GetRequestStream())
                    {
                        soapEnvelopeXml.Save(stream);
                    }
            }
        }
    }
