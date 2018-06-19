using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AMS.DB
{
    public static class GlobalData
    {
        public static string EMPRESA = "";
        public static string getEMPRESA()
        {
            if(EMPRESA == "")
            {
                string urlLogin = HttpContext.Current.Request.Url.AbsoluteUri;
                string[] arrayUrlLogin = urlLogin.Split('/');

                if (urlLogin.ToUpper().Contains("ASMX"))
                    EMPRESA = "ASPX";
                else
                    EMPRESA = arrayUrlLogin[3].ToLower();
            }

            return EMPRESA;
        }
    }
}
