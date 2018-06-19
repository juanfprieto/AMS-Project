using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;
using bd.WebScheduledTasks;
using System.Net;
using System.Configuration;
using System.Collections;
using System.Data;

namespace AMS.Tools
{
    public class WebRequestTask : IScheduleable
    {
        private static DateTime LastRun = DateTime.MinValue;
        private TimeSpan interval = TimeSpan.FromMinutes(2);

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