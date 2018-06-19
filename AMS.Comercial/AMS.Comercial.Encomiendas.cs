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
	/// Descripci�n breve de Encomiendas.
	/// </summary>
	public class Encomiendas
	{
		public Encomiendas()
		{
			//
			// TODO: agregar aqu� la l�gica del constructor
			//
		}
		//Devuelve siguiente encomienda virtual arranca en 1000000
		public static string TraerSiguienteEncomiendaVirtual()
		{
			string documento=DBFunctions.SingleData("SELECT NUM_DOCUMENTO+1 FROM DBXSCHEMA.MENCOMIENDAS WHERE NUM_DOCUMENTO<"+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+"000 AND NUM_DOCUMENTO>="+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+" ORDER BY NUM_DOCUMENTO DESC FETCH FIRST 1 ROWS ONLY;");
			if(documento.Length==0)documento=((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)+1).ToString();
			return(documento);
		}
	}
}
