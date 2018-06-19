using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AMS.Tools;


namespace AMS.Automotriz
{
    public partial class ConsumoWSSura : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            CallWebService();
        } 
        public static void CallWebService()
        {
            var _url = "http://64.76.90.223:8081/taller";
            var _action = "http://com.subocol.sat.services/SoapSatTaller/updateEvent";

            XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Console.Write(soapResult);
                string response1 = Convert.ToString(((System.Net.HttpWebResponse)webResponse).StatusCode);
            }
        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Host = "64.76.90.223:8081";
            return webRequest;
        }
        private static XmlDocument CreateSoapEnvelope()
        {
            XmlDocument soapEnvelop = new XmlDocument();
            soapEnvelop.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://com.subocol.sat.services/SoapSatTaller/"" xmlns:sat=""http://www.subocol.com/satschema"">
                                   <soapenv:Header/>
                                   <soapenv:Body>
                                      <soap:updateEventRequest>
                                         <sat:nit>900629750</sat:nit>
                                         <sat:plate>HIU521</sat:plate>
                                         <sat:sinister>1234</sat:sinister>
                                         <sat:statusCode>11</sat:statusCode>
                                         <sat:date>2017-04-24</sat:date>
                                         <sat:time>12:00:00</sat:time>
                                      </soap:updateEventRequest>
                                   </soapenv:Body>
                                </soapenv:Envelope>");
            return soapEnvelop;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
    }
}