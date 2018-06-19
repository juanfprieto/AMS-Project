using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Tamir.SharpSsh;
using System.Xml;
using AMS.DB;
using System.Configuration;

namespace AMS.FileSync
{
    public class XmlKioskoLinux
    {
        private string imgFolder;
        private string videoFolder;
        private string tempFileFolder;
        private string numTienda;

        private string ipServ;
        private string localAppName;
        private string imgTime;
        private string timeOut;

        private ArrayList images;
        private ArrayList videos;
        private SshExec exec;

        public XmlKioskoLinux(string imgFolder, string videoFolder, string tempFileFolder, string numTienda, string host, string user, string password)
        {
            this.imgFolder = imgFolder;
            this.videoFolder = videoFolder;
            this.tempFileFolder = tempFileFolder;
            this.numTienda = numTienda;

            
            exec = new SshExec(host, user);
            exec.Password = password;
            exec.Connect();

            initVars();
        }

        public XmlKioskoLinux(string imgFolder, string videoFolder, string tempFileFolder, string numTienda, SshExec exec)
        {
            this.imgFolder = imgFolder;
            this.videoFolder = videoFolder;
            this.tempFileFolder = tempFileFolder;
            this.numTienda = numTienda;
            this.exec = exec;

            initVars();
        }

        private void initVars()
        {
            String sql = "SELECT CKIO_TIMEOUT, \n" +
                         "       CKIO_IMGROT, \n" +
                         "       CKIO_IPSERV \n" +
                         "FROM CKIOSKO";

            ArrayList result = (ArrayList)DBFunctions.RequestAsCollection(sql);

            Hashtable conf = (Hashtable)result[0];

            this.ipServ = conf["CKIO_IPSERV"].ToString();
            this.imgTime = conf["CKIO_IMGROT"].ToString();
            this.timeOut = conf["CKIO_TIMEOUT"].ToString();

            this.localAppName = ConfigurationManager.AppSettings["KioskoImgServ"];

            this.images = new ArrayList();
            this.videos = new ArrayList();
        }

        private void fillVideoFiles()
        {
            string output = exec.RunCommand(String.Format("ls {0}", videoFolder));

            foreach (string file in output.Split('\n'))
            {
                if (file == "" || file == " ") continue;
                videos.Add(file);
            }
        }

        private void fillImageFiles()
        {
            string output = exec.RunCommand(String.Format("ls {0}", imgFolder));

            foreach (string file in output.Split('\n'))
            {
                if (file == "" || file == " ") continue;
                images.Add(file);
            }
        }

        public void generateImageXML()
        {
            fillImageFiles();

            XmlDocument doc = new XmlDocument();
            XmlElement playlist = (XmlElement)doc.AppendChild(doc.CreateElement("PLAYLIST"));
            playlist.SetAttribute("TIEMPO", imgTime);

            string relativeFolder = imgFolder.Substring(imgFolder.LastIndexOf('/') + 1);

            foreach(string file in images)
            {
                XmlElement imagen = (XmlElement)playlist.AppendChild(doc.CreateElement("IMAGEN"));
                imagen.SetAttribute("THUMB", String.Format("{0}/{1}", relativeFolder, file));
            }

            doc.Save(String.Format("{0}{1}", tempFileFolder, "imagenes.xml"));
        }

        public void generateVideoXML()
        {
            fillVideoFiles();

            XmlDocument doc = new XmlDocument();
            XmlElement playlist = (XmlElement)doc.AppendChild(doc.CreateElement("PLAYLIST"));

            string relativeFolder = videoFolder.Substring(imgFolder.LastIndexOf('/') + 1);

            foreach(string file in videos)
            {
                XmlElement imagen = (XmlElement)playlist.AppendChild(doc.CreateElement("VIDEOS"));
                imagen.SetAttribute("THUMB", String.Format("{0}/{1}", relativeFolder, file));
            }

            doc.Save(String.Format("{0}{1}", tempFileFolder, "videos.xml"));
        }

        public void generateServerXML(string ipKiosko)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement servidor = (XmlElement)doc.AppendChild(doc.CreateElement("SERVIDOR"));
            servidor.SetAttribute("TIMEOUT", timeOut);
            servidor.SetAttribute("IPSERVIMG", String.Format("http://{0}/{1}", ipServ, localAppName));
            servidor.SetAttribute("IPSERV",  String.Format("http://{0}/kiosko", ipServ));
            servidor.SetAttribute("MY_IP", ipKiosko);
            servidor.SetAttribute("NUMTIENDA", numTienda);

            doc.Save(String.Format("{0}{1}", tempFileFolder, "servidor.xml"));
        }

        public void closeConnection()
        {
            exec.Close();
        }
    }
}