using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AMS.DB;
using System.Configuration;
using System.Collections;
using Ajax;

namespace AMS.Tools
{
	public partial class DiccionarioPalabras : System.Web.UI.UserControl
	{
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(DiccionarioPalabras));
            if (!IsPostBack)
            {
            }
		}

        [Ajax.AjaxMethod]
        public string CargaDiccionario(string palabraClave)
        {
            DataSet dsDiccionarioPalabras = new DataSet();
            DBFunctions.Request(dsDiccionarioPalabras, IncludeSchema.NO, "SELECT pdic_palarela FROM dbxschema.pdiccionario where pdic_palaclav = '" + palabraClave + "' order by pdic_palarela;");
            string palabrasRelacionadas = "";

            for (int i = 0; i < dsDiccionarioPalabras.Tables[0].Rows.Count; i++)
            {
                palabrasRelacionadas += dsDiccionarioPalabras.Tables[0].Rows[i][0].ToString() + ",";
            }

            return palabrasRelacionadas;
        }

        [Ajax.AjaxMethod]
        public string AgregarPalabra(string palabraClave)
        {
            string[] palabras = palabraClave.Split('*');
            if (palabras[0] != "")
            {
                if (palabras[1] != "")
                {
                    string existePalabra = DBFunctions.SingleData("SELECT pdic_palarela FROM dbxschema.pdiccionario where pdic_palarela = '" + palabras[1].ToUpper() + "';");
                    if (existePalabra == "")
                    {
                        ArrayList sqlStrings = new ArrayList();
                        sqlStrings.Add("INSERT INTO DBXSCHEMA.PDICCIONARIO VALUES (DEFAULT, '" + palabras[0] + "','" + palabras[1].ToUpper() + "')");
                        
                        if (DBFunctions.Transaction(sqlStrings))
                        {
                            string palabrasAsociadas = CargaDiccionario(palabras[0]);
                            return palabrasAsociadas;
                        }
                        else return "D";
                    }
                    else return "C";
                }
                else return "B";
            }
            else return "A";

            return "";
        }

        [Ajax.AjaxMethod]
        public string EliminarPalabra(string palabraClave)
        {
            string[] palabras = palabraClave.Split('*');
            if (palabras[0] != "")
            {
                if (palabras[1] != "")
                {
                    ArrayList sqlStrings = new ArrayList();
                    sqlStrings.Add("DELETE FROM  DBXSCHEMA.PDICCIONARIO WHERE PDIC_PALACLAV = '" + palabras[0] + "' and pdic_palarela = '" + palabras[1] + "';");

                    if (DBFunctions.Transaction(sqlStrings))
                    {
                        string palabrasAsociadas = CargaDiccionario(palabras[0]);
                        return palabrasAsociadas;
                    }
                    else return "C";
                }
                else return "B";
            }
            else return "A";

            return "";
        }

	}
}