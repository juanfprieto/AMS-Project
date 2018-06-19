using System;
using System.Configuration;
using System.Web;

namespace AMS.Tools
{
	/// <summary>
	/// Manejador Browser.
	/// </summary>
	public class Browser
	{
		public Browser(){}
		public static bool IsMobileBrowser()
		{
			if(ConfigurationManager.AppSettings["AlwaysMobile"]!=null && Convert.ToBoolean(ConfigurationManager.AppSettings["AlwaysMobile"]))
				return(true);
			HttpContext context = HttpContext.Current;
			if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
				return true;
			if (context.Request.ServerVariables["HTTP_ACCEPT"] != null && context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().IndexOf("wap")>0)
				return true;
			if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
			{
				//Tipos mobiles
				string[] mobiles =
					new string []{
									 //"midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource", "240x320", "opwv", "chtml", "pda", "windows ce", "mmp/", "blackberry", "mib/", "symbian", "wireless", "nokia", "hand", "mobi", "phone", "cdm", "up.b", "audio", "SIE-", "SEC-", "samsung", "HTC", "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx", "NEC", "philips", "mmm", "xx", "panasonic", "sharp", "wap", "sch", "rover", "pocket", "benq", "java", "pt", "pg", "vox", "amoi", "bird", "compal", "kg", "voda", "sany", "kdd", "dbt", "sendo", "sgh", "gradi", "jb", "dddi", "moto", "iphone"};
									 "midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource", "240x320", "opwv", "chtml", "pda", "windows ce", "mmp/", "blackberry", "mib/", "symbian", "wireless", "nokia", "hand", "mobi", "phone", "cdm", "up.b", "audio", "SIE-", "SEC-", "samsung", "HTC", "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx", "NEC", "philips", "mmm", "xx", "panasonic", "sharp", "wap", "sch", "rover", "pocket", "benq", "java", "pg", "vox", "amoi", "bird", "compal", "kg", "voda", "sany", "kdd", "dbt", "sendo", "sgh", "gradi", "jb", "dddi", "moto", "iphone"};
				foreach (string s in mobiles)
				{
					if (context.Request.ServerVariables["HTTP_USER_AGENT"].ToLower().IndexOf(s.ToLower())>0)
						return true;
				}
			}
			return false;
		}
	}
}
