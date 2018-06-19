using System.Web.Services;
using System.Data;
using AMS.DB;
using System.Xml;

namespace AMS.WebService
{
    /// <summary>
    /// Summary description for AMS_WSIntegracion
    /// </summary>
    [WebService(Namespace = "http://ams.ecas.co/AMS_WebService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AMS_WSIntegracion : System.Web.Services.WebService
    {

        [WebMethod]
        public int Calculator(int an, int bn)
        {

            return an + bn;
        }

        [WebMethod]
        public XmlDocument GetBloque(string user, string pass)
        {
            if (user == "wsuser" && pass == "1234")
            {
                XmlDocument xml = new XmlDocument();
                DataSet ds = new DataSet();
                ds = DBFunctions.Request(ds, IncludeSchema.NO, "select * from susuario fetch first 5 rows only");    
                ds.DataSetName = "USUARIOS";
                ds.Tables[0].TableName = "REGISTRO";
                xml.LoadXml(ds.GetXml());
                return xml;
            }
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlNode rootNode = xmlDoc.CreateElement("USUARIOS");
                xmlDoc.AppendChild(rootNode);

                XmlNode userNode = xmlDoc.CreateElement("REGISTRO");
                //XmlAttribute attribute = xmlDoc.CreateAttribute("age");
                //attribute.Value = "42";
                //userNode.Attributes.Append(attribute);
                userNode.InnerText = "ERROR LOGIN";
                rootNode.AppendChild(userNode);

                //xmlDoc.Save("test-doc.xml");
                return xmlDoc;
            }
        }
    }
}
