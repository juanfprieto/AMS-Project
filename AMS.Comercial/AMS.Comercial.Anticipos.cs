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
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.DBManager;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Anticipos
	{
		public Anticipos()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Devuelve siguiente planilla virtual arranca en 1000000
		public static string TraerSiguienteAnticipoVirtual()
		{
			string documento=DBFunctions.SingleData("SELECT NUM_DOCUMENTO+1 FROM DBXSCHEMA.MGASTOS_TRANSPORTES WHERE NUM_DOCUMENTO<"+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+"00 AND NUM_DOCUMENTO>="+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+" ORDER BY NUM_DOCUMENTO DESC FETCH FIRST 1 ROWS ONLY;");
			if(documento.Length==0)documento=((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)+1).ToString();
			return(documento);
		}
	}
}