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
	public partial class GeneradorPlanillaPila : System.Web.UI.UserControl
	{
        protected System.Web.UI.WebControls.DropDownList DDLANO;
        protected System.Web.UI.WebControls.DropDownList DDLMES;
        protected System.Web.UI.WebControls.HyperLink hl;
        protected System.Web.UI.WebControls.Button btnGenerar;
        protected System.Web.UI.WebControls.Label info;

		protected string nombreArchivo = "PlanillaPila.txt";
		protected	string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];
        protected string path = ConfigurationManager.AppSettings["VirtualPathToDownloads"];
        protected	StreamWriter sw;
		string  textoArch = "";
		DatasToControls bind = new DatasToControls();

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
                bind.PutDatasIntoDropDownList(DDLANO, "SELECT PANO_ANO, PANO_ANO FROM dbxschema.PANO ORDER BY PANO_ANO desc;");
                bind.PutDatasIntoDropDownList(DDLMES, "Select pMES_MES, pMES_NOMBRE from dbxschema.pMES ORDER BY pMES_MES;");
            }
        }

        private void InitializeComponent()
		{
			this.btnGenerar.Click += new System.EventHandler(this.IniciarProceso);
			this.Load += new System.EventHandler(this.Page_Load);
		}

        protected void IniciarProceso(object sender, EventArgs e)
		{
            sw = File.CreateText(directorioArchivo + DDLANO.SelectedItem + DDLMES.SelectedValue + nombreArchivo);

			DataSet DatosCuerpo= new DataSet();
            DBFunctions.Request(DatosCuerpo, IncludeSchema.NO, "call dbxschema.planilla_pila(" + DDLANO.SelectedItem + "," + DDLMES.SelectedValue + ")");
            textoArch = "";

            for (int i=0;i<DatosCuerpo.Tables[0].Rows.Count;i++)
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
            hl.NavigateUrl = "../dwl/" + DDLANO.SelectedItem + DDLMES.SelectedValue + nombreArchivo;
			hl.Visible=true;
            hl.Attributes.Add("download", "../dwl/" + DDLANO.SelectedItem + DDLMES.SelectedValue + nombreArchivo);
			hl.Text="Descargar Archivo";
            info.Visible = true;
		}

        private void GenerarPlanilla(int año, int mes)
        {
            string result = "";
            DateTime auxDate = new DateTime(año, mes, 1);
            auxDate.AddMonths(1);
            int mes2 = auxDate.Month;
            int año2 = auxDate.Year;

            /////////////////////////////
            /////////ENCABEZADO//////////
            /////////////////////////////

            string sqlDatosEmpresa = 
                "SELECT cemp_nombre AS NOMBRE, \n" +
                "       mnit_nit AS NIT, \n" +
                "       cemp_digito AS DIGITO \n" +
                "FROM cempresa";
            string sqlARP = 
                "SELECT PARP_ENTIARP AS ARP \n" +
                "FROM parp";
            String sqlValoresTotales = String.Format(
                "SELECT count(DQ.MEMP_CODIEMPL) AS NUMEMPLEADOS, \n" +
                "       sum(DQ.DQUI_APAGAR - DQ.DQUI_ADESCONTAR) AS TOTALNOMINA  \n" +
                "FROM DQUINCENA DQ  \n" +
                "  INNER JOIN PCONCEPTONOMINA PC ON PC.PCON_CONCEPTO = DQ.PCON_CONCEPTO  \n" +
                "  LEFT JOIN MQUINCENAS MQ ON DQ.MQUI_CODIQUIN = MQ.MQUI_CODIQUIN \n" +
                "WHERE PC.TRES_AFEC_EPS = 'S' \n" +
                "AND   MQ.MQUI_ANOQUIN = {0} \n" +
                "AND   MQ.MQUI_MESQUIN = {1}"
                , año
                , mes);

            Hashtable datosEmpresa = (Hashtable)DBFunctions.RequestAsCollection(sqlDatosEmpresa)[0]; // siempre trae un solo registro
            Hashtable datosARP = (Hashtable)DBFunctions.RequestAsCollection(sqlARP)[0]; // siempre trae un solo registro, o solo tomamos el primero
            Hashtable datosValores = (Hashtable)DBFunctions.RequestAsCollection(sqlValoresTotales)[0]; // siempre trae un solo registro, o solo tomamos el primero

            // 1. Tipo de Registro
            result += "01"; // Obligatorio
            // 2A - 2B. Modalidad de la Planilla - Secuencia
            result += "00001"; // Obligatorio
            // 3. Nombre o razón social del aportante
            result += Utils.CompletarCampos(datosEmpresa["NOMBRE"].ToString(), 200, " ", true);
            // 4, 5, 6. Tipo documento del aportante, Número de Identificación del aportante, Dígito de Verificación aportante
            result += "NI" + Utils.CompletarCampos(datosEmpresa["NIT"].ToString(), 16, " ", true) + datosEmpresa["DIGITO"].ToString();
            // 7, 8. Tipo de planilla, Número de la planilla asociada a esta planilla
            result += "E" + Utils.CompletarCampos("", 10, "0", true);
            // 9. Fecha de pago Planilla asociada a esta planilla. (AAAA-MM-DD).
            result += Utils.CompletarCampos("", 10, " ", true);
            // 10, 11, 12. Forma de presentación, Código de la Sucursal del aportante, Nombre de la Sucursal
            result += "U" + Utils.CompletarCampos("", 10, " ", true) + Utils.CompletarCampos("", 40, " ", true);
            // 13. Código de la ARP a la cual el aportante se encuentra afiliado.
            result += Utils.CompletarCampos(datosARP["ARP"].ToString(), 6, " ", true);
            // 14. Período de pago para los sistemas diferentes al de salud.
            result += Utils.CompletarCampos(año.ToString(), 4, " ", true) + "-" + Utils.CompletarCampos(mes.ToString(), 2, "0", false);
            // 15. Período de pago para el sistema de salud.
            result += Utils.CompletarCampos(año2.ToString(), 4, " ", true) + "-" + Utils.CompletarCampos(mes2.ToString(), 2, "0", false);
            // 16. Período de pago para el sistema de salud.
            result += Utils.CompletarCampos("", 10, "0", true);
            // 17. Fecha de pago (aaaa-mm-dd).
            result += Utils.CompletarCampos("", 10, " ", true);
            // 18. Número total de empleados.
            result += Utils.CompletarCampos(datosValores["NUMEMPLEADOS"].ToString(), 5, "0", false);
        }

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
        }
        #endregion

    }
}
