using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
//using AMS.Reports;
using AMS.DB;
using System.Data.Odbc;
using System.Configuration;
using System.Windows.Forms;
using AMS.Tools;

namespace AMS.Nomina
{
	public partial class GeneradorArchivoPlanoPagosaTerceros : System.Web.UI.UserControl
	{
        //protected System.Web.UI.WebControls.TextBox TXTPREFIJO1;
        //protected System.Web.UI.WebControls.TextBox TXTNUMERO;
        protected System.Web.UI.WebControls.HyperLink hl;
        //protected System.Web.UI.WebControls.Button btnGenerar;
        protected System.Web.UI.WebControls.Label info;
        protected string nombreArchivo = "ArchivoPlanoPagoaTerceros.txt";
        protected string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];
        protected string path = ConfigurationManager.AppSettings["VirtualPathToDownloads"];
        protected StreamWriter sw;
        string textoArch = "";
        DatasToControls bind = new DatasToControls();

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                TXTPREFIJO.Text = "";
                TXTNUMERO.Text="";
            }
		}
        private void InitializeComponent()
        {
            //this.btnGenerar.Click += new System.EventHandler(this.IniciarProceso);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        protected void IniciarProceso(object sender, EventArgs e)
        {
            sw = File.CreateText(directorioArchivo + TXTPREFIJO.Text + TXTNUMERO.Text + nombreArchivo);
            int numero = Convert.ToInt16(TXTNUMERO.Text.ToString());
            DataSet DatosCuerpo = new DataSet();
            DBFunctions.Request(DatosCuerpo, IncludeSchema.NO, "CALL DBXSCHEMA.PLANILLA_BANCO_OCCIDENTE(" + "'" + TXTPREFIJO.Text + "'" + "," + numero + ")");
            textoArch = "";

            for (int i = 0; i < DatosCuerpo.Tables[0].Rows.Count; i++)
            {
                string line = DatosCuerpo.Tables[0].Rows[i][0].ToString();
                if (line != "")
                {
                    textoArch += DatosCuerpo.Tables[0].Rows[i][0].ToString();
                    sw.WriteLine(DatosCuerpo.Tables[0].Rows[i][0].ToString());
                }
            }

            //sw.WriteLine(textoArch);
            sw.Close();

            Utils.MostrarAlerta(Response, "Archivo Generado con Exito.");
            hl.NavigateUrl = "../dwl/" + TXTPREFIJO.Text + TXTNUMERO.Text + nombreArchivo;          
            hl.Visible = true;
            hl.Attributes.Add("download", "../dwl/" + TXTPREFIJO.Text + TXTNUMERO.Text + nombreArchivo);            
            hl.Text = "Descargar Archivo";
            info.Visible = true;
        }
	}
}