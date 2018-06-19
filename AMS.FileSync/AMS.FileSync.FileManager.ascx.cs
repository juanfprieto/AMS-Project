using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Reflection;
using IZ.WebFileManager;
using System.Collections;
using AMS.DB;
using AMS.Tools;
using EO.Web;
using System.Security;
using System.Security.Permissions;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Synchronization.Files;

namespace AMS.FileSync
{
    public partial class FileManager : System.Web.UI.UserControl
    {
        protected string ajaxIndexPage = ConfigurationManager.AppSettings["MainAjaxPage"];

        protected void Page_Load(object sender, EventArgs e)
        {
            //FileManager1.Directory = null;

            FileManager1.Culture = CultureInfo.CurrentCulture;
            
            //FileManager1.DownloadOnDoubleClick = true;
            //FileManager1.RootDirectories[0].ShowRootIndex = false;

            string regPath = @"Software\FileManager\";
            System.Security.Permissions.RegistryPermission permissions =
                    new System.Security.Permissions.RegistryPermission(System.Security.Permissions.RegistryPermissionAccess.AllAccess, @"HKEY_LOCAL_MACHINE\" + regPath);
            bool regPermissions = System.Security.SecurityManager.IsGranted(permissions);

            AppDomain.CurrentDomain.PermissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
        }

        protected void progressBar_RunTask(object sender, ProgressTaskEventArgs e)
        {
            e.UpdateProgress(0);
            string pathToFileSync = ConfigurationManager.AppSettings["PathToFileSync"];
            
            //Process p = new Process();
            //p.StartInfo.File
            //Name = pathToFileSync + "Ecas.MakroFileSync.exe";
            //p.Start();

            String sql = "SELECT MVIPK_IP,  \n" +
             "       MVIPK_IMGFOLDER,  \n" +
             "       MVIPK_VIDEOFOLDER, \n" +
             "       MVIPK_NUMETIENDA \n" +
             "FROM MVIPKIOSKO";

            ArrayList kioskos = DBFunctions.RequestAsCollection(sql);

            e.UpdateProgress(1);

            int razonCambioKiosko = 98 / kioskos.Count;
            int razonCambio = razonCambioKiosko / 9;
            int progActual = 1;

            foreach (Hashtable kiosko in kioskos)
            {
                string host = kiosko["MVIPK_IP"].ToString();
                string imageFolder = kiosko["MVIPK_IMGFOLDER"].ToString();
                string videoFolder = kiosko["MVIPK_VIDEOFOLDER"].ToString();
                string numTienda = kiosko["MVIPK_NUMETIENDA"].ToString();

                e.UpdateProgress(progActual, String.Format("Sincronizando kiosko {0}...", host));

                try
                {
                    //FileSyncOperator.syncKioskoLinux(host, imageFolder, videoFolder, numTienda, e, progActual, razonCambio);
                    FileSyncOperator.syncKioskoWindows(host, imageFolder, videoFolder, numTienda, e, progActual, razonCambio);
                    divError.InnerText = "OK Fyle";
                }
                catch (Exception ex)
                {
                    e.UpdateProgress(progActual, String.Format(".{0}\n{1}", ex.Message, ex.InnerException));
                    divError.InnerText = ex.Message + " " + ex.InnerException;
                }

                progActual += razonCambioKiosko;
            }

            e.UpdateProgress(100, "Sincronización terminada!");

            Utils.MostrarAlerta(Response, "Sincronización completada!");
            //Response.Redirect("" + ajaxIndexPage + "?process=FileSync.FileManager&sync=1");
        }
    }

}