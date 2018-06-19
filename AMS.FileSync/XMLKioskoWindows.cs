using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Tamir.SharpSsh;
using System.Xml;
using AMS.DB;
using System.Configuration;
using System.IO;

namespace AMS.FileSync
{
    public class XmlKioskoWindows
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

        public XmlKioskoWindows(string imgFolder, string videoFolder, string tempFileFolder, string numTienda)
        {
            this.imgFolder = imgFolder;
            this.videoFolder = videoFolder;
            this.tempFileFolder = tempFileFolder;
            this.numTienda = numTienda;

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
            string [] fileEntries = Directory.GetFiles(videoFolder);
            foreach (string file in fileEntries)
            {
                if (file == "" || file == " ") continue;

                if (file.Contains(".flv"))
                    videos.Add(file.Substring(file.LastIndexOf('\\') + 1));
            }
        }

        private void fillImageFiles()
        {
            string [] fileEntries = Directory.GetFiles(imgFolder);
            foreach (string file in fileEntries)
            {
                if (file == "" || file == " " ) continue;

                if (file.Contains(".jpg"))
                    images.Add(file.Substring(file.LastIndexOf('\\') + 1));
            }
        }

        public void generateImageXML()
        {
            fillImageFiles();

            XmlDocument doc = new XmlDocument();
            XmlElement playlist = (XmlElement)doc.AppendChild(doc.CreateElement("PLAYLIST"));
            playlist.SetAttribute("TIEMPO", imgTime);

            string relativeFolder = imgFolder.Substring(imgFolder.LastIndexOf('\\') + 1);

            foreach(string file in images)
            {
                XmlElement imagen = (XmlElement)playlist.AppendChild(doc.CreateElement("IMAGEN"));
                imagen.SetAttribute("THUMB", String.Format("{0}\\{1}", relativeFolder, file));
            }

            doc.Save(String.Format("{0}{1}", tempFileFolder, "imagenes.xml"));
        }

        public void generateVideoXML()
        {
            fillVideoFiles();

            XmlDocument doc = new XmlDocument();
            XmlElement playlist = (XmlElement)doc.AppendChild(doc.CreateElement("PLAYLIST"));

            string relativeFolder = videoFolder.Substring(videoFolder.LastIndexOf('\\') + 1);

            foreach(string file in videos)
            {
                XmlElement imagen = (XmlElement)playlist.AppendChild(doc.CreateElement("VIDEOS"));
                imagen.SetAttribute("THUMB", String.Format("{0}\\{1}", relativeFolder, file));
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
    }
}