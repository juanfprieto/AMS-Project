using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.DBManager;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Viajes
	{
		public Viajes()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Devuelve siguiente numero de viaje
		public static string TraerSiguienteViaje(string mruta)
		{
			string viaje=DBFunctions.SingleData("SELECT MVIAJE_NUMERO+1 FROM DBXSCHEMA.MVIAJE WHERE MRUT_CODIGO='"+mruta+"' ORDER BY MVIAJE_NUMERO DESC FETCH FIRST 1 ROWS ONLY;");
			if(viaje.Length==0)viaje="1";
			return(viaje);
		}
	}
}
