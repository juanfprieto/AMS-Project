using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;
using bd.WebScheduledTasks;
using System.Net;
using System.Configuration;
using System.Collections;

namespace AMS.VIP
{
    public class TareasProgramadasVIP : IScheduleable
    {

        #region IScheduleable Members

        public bool IsRunning
        {
            get { return false; }
        }

        public bool IsTimeToRun
        {
            get { return true; }
        }

        public void Run(HttpApplicationState application, System.Web.Caching.Cache cache)
        {
            cambiarEstadoTarjetas();
            cambiarEstadoFacturas();
        }

        private void cambiarEstadoFacturas()
        {
            ArrayList sqlList = new ArrayList();
            string sql = "select coalesce(cvred_diasgracia,0) from cvipredencion";
            int diasGracia = Convert.ToInt32(DBFunctions.SingleData(sql));
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            String fechaVencida = DateTime.Now.AddDays(diasGracia - 1).ToString("yyyy-MM-dd");

            sqlList.Add(String.Format("INSERT INTO MHISTORIAL_FACTURA " +
             "(FACTURA, OPERACION, SUSU_USUARIO, FECHA) " +
             "SELECT mvfac_codigo,'PR','SISTEMA','{1}' " +
             "FROM mvipfactura " +
             "WHERE mvfac_fechapago = '{0}' " +
             "AND   mvfac_diasprorroga > 0 " +
             "AND   tvest_codigo = 'AD'"
             , fechaVencida, now));

            sqlList.Add(String.Format("UPDATE mvipfactura " +
             "   SET mvfac_fechapago = mvfac_fechapago + mvfac_diasprorroga days, " +
             "       tvest_codigo = 'PR' " +
             "WHERE mvfac_fechapago = '{0}' " +
             "AND   mvfac_diasprorroga > 0 " +
             "AND   tvest_codigo = 'AD'"
             , fechaVencida));

            sqlList.Add(String.Format("INSERT INTO MHISTORIAL_FACTURA " +
             "(FACTURA, OPERACION, SUSU_USUARIO, FECHA) " +
             "SELECT mvfac_codigo,'PE','SISTEMA','{1}' " +
             "FROM mvipfactura " +
             "WHERE mvfac_fechapago <= '{0}' " +
             "AND   (tvest_codigo = 'AD' OR tvest_codigo = 'PR')"
             , fechaVencida, now));

            sqlList.Add(String.Format("UPDATE mvipfactura \n" +
             "   SET tvest_codigo = 'PE' \n" +
             "WHERE mvfac_fechapago <= '{0}' \n" +
             "AND   (tvest_codigo = 'AD' OR tvest_codigo = 'PR')"
             , fechaVencida));

            DBFunctions.Transaction(sqlList);
        }

        private void cambiarEstadoTarjetas()
        {
            String sql = String.Format("update mviptarjeta set tvig_codigo='VE' where mtar_fechaexpi='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            DBFunctions.NonQuery(sql);
        }

        #endregion
    }

    public class WebRequestTask : IScheduleable
    {
        private static DateTime LastRun = DateTime.MinValue;
        private TimeSpan interval = TimeSpan.FromMinutes(10);

        #region IScheduleable Members

        public bool IsRunning
        {
            get { return false; }
        }

        public bool IsTimeToRun
        {
            get
            {
                return (DateTime.Now.Subtract(LastRun) > interval);
            }
        }

        public void Run(HttpApplicationState application, System.Web.Caching.Cache cache)
        {
            foreach (string set in ConfigurationManager.AppSettings.AllKeys)
            {
                if (set.ToLower().StartsWith("webrequest"))
                {
                    DoWebRequest(ConfigurationManager.AppSettings[set]);
                }
            }
            LastRun = DateTime.Now;
        }

        #endregion

        private void DoWebRequest(string url)
        {
            WebClient web = new WebClient();
            //web.DownloadData(url);
        }
    }
}