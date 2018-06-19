using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Xml;
using System.Data;
using AMS.DB;
using AMS.Tools;
using System.Web;

namespace AMS.Automotriz
{
    public partial class ConsumoSuraWS : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
                CallWebService();
        }
        protected void CallWebService()
        {
            string resultado = "";
            XmlDocument xml = new XmlDocument();
            DataSet ds = new DataSet();            
            ds = DBFunctions.Request(ds, IncludeSchema.NO, @"SELECT M.MNIT_NIT, 
                                                               MC.MCAT_PLACA,
                                                               DS.MORD_SINIESTRO,
                                                               '1',
                                                               VARCHAR_FORMAT(M.MORD_ENTRADA, 'YYYY-MM-DD'),
                                                               M.MORD_HORAENTR
                                                         FROM MORDEN M,
                                                              MCATALOGOVEHICULO MC,
                                                              DORDENSEGUROS DS
                                                        WHERE MC.MCAT_VIN = M.MCAT_VIN
                                                          AND M.PDOC_CODIGO = DS.PDOC_CODIGO
                                                          AND M.MORD_NUMEORDE = DS.MORD_NUMEORDE
                                                          AND MORD_entrada = CURRENT DATE
                                                          AND M.TCAR_CARGO = 'S'
                                                          AND DS.MNIT_NITSEGUROS = '890903407'
                                                          UNION
                                                        SELECT distinct M.MNIT_NIT,
                                                               MC.MCAT_PLACA,
                                                               DS.MORD_SINIESTRO,
                                                               CASE WHEN D.TEST_ESTADO = 'R' THEN 'A031' ELSE CASE WHEN D.PTEM_OPERACION not in ('4','11','12','13','14','15','16','17','18','19') THEN '16' else D.PTEM_OPERACION end END AS PTEM_OPERACION,
                                                               VARCHAR_FORMAT(DESTOPER_HORA, 'YYYY-MM-DD'),
                                                               M.MORD_HORAENTR
                                                         FROM MORDEN M,
                                                              MCATALOGOVEHICULO MC,
                                                              DORDENSEGUROS DS,
                                                              DESTADISTICAOPERACION D,
	                                                          DORDENOPERACION DP,
	                                                          ptempario pt
                                                        WHERE MC.MCAT_VIN = M.MCAT_VIN
                                                          AND M.PDOC_CODIGO = DS.PDOC_CODIGO
                                                          AND M.MORD_NUMEORDE = DS.MORD_NUMEORDE
                                                          AND M.PDOC_CODIGO = D.PDOC_CODIGO
                                                          AND M.MORD_NUMEORDE = D.MORD_NUMEORDE
                                                          AND DS.PDOC_CODIGO = D.PDOC_CODIGO
                                                          AND DS.MORD_NUMEORDE = D.MORD_NUMEORDE
                                                          AND DATE (DESTOPER_HORA) = DATE (CURRENT DATE) 
                                                          AND DP.PDOC_CODIGO = D.PDOC_CODIGO 
                                                          AND DP.MORD_NUMEORDE = D.MORD_NUMEORDE
                                                          AND DP.PTEM_OPERACION = D.PTEM_OPERACION
                                                          AND DP.PTEM_OPERACION = pt.PTEM_OPERACION
                                                          AND DP.TCAR_CARGO = 'S'  
                                                          AND D.TEST_ESTADO IN ('C','R')
                                                          AND DS.MNIT_NITSEGUROS = '890903407' 
                                                        union 
                                                         SELECT M.MNIT_NIT, 
                                                               MC.MCAT_PLACA,
                                                               DS.MORD_SINIESTRO,
                                                               '2',
                                                               VARCHAR_FORMAT(M.MORD_ENTREGAR, 'YYYY-MM-DD'),
                                                               M.MORD_HORAENTG
                                                         FROM MORDEN M,
                                                              MCATALOGOVEHICULO MC,
                                                              DORDENSEGUROS DS
                                                        WHERE MC.MCAT_VIN = M.MCAT_VIN
                                                          AND M.PDOC_CODIGO = DS.PDOC_CODIGO
                                                          AND M.MORD_NUMEORDE = DS.MORD_NUMEORDE
                                                          AND MORD_entrada = CURRENT DATE
                                                          AND M.TCAR_CARGO = 'S'
                                                          AND M.TEST_ESTADO = 'A' AND M.PDOC_CODIGO || M.MORD_NUMEORDE NOT IN 
                                                          (SELECT PDOC_PREFORDETRAB||MORD_NUMEORDE FROM MFACTURACLIENTETALLER MFT WHERE TCAR_CARGO ='S' AND M.PDOC_CODIGO = MFT.PDOC_PREFORDETRAB
                                                          AND M.MORD_NUMEORDE = MFT.MORD_NUMEORDE )
                                                          AND DS.MNIT_NITSEGUROS = '890903407';");


            //Defino la url de conexion y la accion de la peticion
            //la _url de pruebas de SUBOCOL es: 64.76.90.223:8081 la _url oficial de SUBOCOL es: 64.76.90.222:8081
            var _url = "http://64.76.90.222:8081/taller";
            var _action = "http://com.subocol.sat.services/SoapSatTaller/updateEvent";
            
            //Verifico que el dataset tenga datos
            if (ds.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se han encontrado registros Revice por favor.");
                Label1.Text = "No se han encontrado registros revice Por favor.";
                Label1.ForeColor = System.Drawing.Color.Red;
                return;
            }
            else
            { //Se valida que el vehiculo tenga un Numero de siniestro
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][2].ToString() == "")
                    {
                        resultado += "Se ha encontado que el Nit: " +ds.Tables[0].Rows[i][0] + " con la Placa: " +ds.Tables[0].Rows[i][1] + " no tiene un numero de siniestro relacionado, EL PROCESO SERA CANCELADO, revice por favor." + "<br>";
                    }
                }
                if (resultado != "")
                {
                    Label1.ForeColor = System.Drawing.Color.Red;
                    Label1.Text = resultado;
                    return;
                }

            }
            

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dsFila = ds.Tables[0].Rows[i];
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(dsFila);
                HttpWebRequest webRequest = CreateWebRequest(_url, _action);
                InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);


                // inicia la llamada asíncrona a la solicitud web.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);


                // suspende este subproceso hasta que se complete la llamada. 
                asyncResult.AsyncWaitHandle.WaitOne();


                // obtengo la respuesta de la solicitud web completada.
                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {
                        soapResult = rd.ReadToEnd();
                        resultado += soapResult + "<br>" + "Propietario: " + ds.Tables[0].Rows[i][0] + "  Placa: " + ds.Tables[0].Rows[i][1] + "  Sinisestro: " + ds.Tables[0].Rows[i][2] + "<br>";                        
                    }
                }              
               
            }
            if (resultado.Contains("EXITO"))
            {
                Label1.ForeColor = System.Drawing.Color.Green;
                Label1.Text = resultado;
            }
            else
            {
                Label1.ForeColor = System.Drawing.Color.Red;
                Label1.Text = "Error en la transmisión. Revice sus datos.";
            }

        }
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            //Se definen los parametros requeridos por el webService 
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            webRequest.Host = "64.76.90.222:8081"; //La de pruebas es: 64.76.90.223:8081 la oficial es: 64.76.90.222:8081
            return webRequest;
        }
        private static XmlDocument CreateSoapEnvelope(DataRow dsFila)
        { //definicion del XML que se va a enviar.
            XmlDocument soapEnvelop = new XmlDocument();
            string Nitempresa = DBFunctions.SingleData("SELECT MNIT_NIT || CEMP_DIGITO FROM CEMPRESA;");
            soapEnvelop.LoadXml(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soap=""http://com.subocol.sat.services/SoapSatTaller/"" xmlns:sat=""http://www.subocol.com/satschema"">
                                           <soapenv:Header/>
                                           <soapenv:Body>
                                              <soap:updateEventRequest>
                                                 <sat:nit>"+Nitempresa+@"</sat:nit>
                                                 <sat:plate>"+dsFila[1].ToString().Trim()+@"</sat:plate>
                                                 <sat:sinister>"+dsFila[2].ToString().Trim()+@"</sat:sinister>
                                                 <sat:statusCode>"+dsFila[3].ToString().Trim()+@"</sat:statusCode>
                                                 <sat:date>"+dsFila[4].ToString().Trim()+@"</sat:date>
                                                 <sat:time>"+dsFila[5].ToString().Trim()+ @"</sat:time>
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