
using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
//using System.Management;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AMS.CriptoServiceProvider;
using AMS.DB;
using AMS.Tools;

namespace AMS.Comercial
{

	/// <summary>
	///		Descripción breve de AMS_Comercial_ReportePersonal.
	/// </summary>
	public class AMS_Comercial_BorrarPlanilla : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox NroPlanilla;
		protected System.Web.UI.WebControls.Button btnConsultaPlanilla;
		protected System.Web.UI.WebControls.Button btnReportePlanilla;
		protected System.Web.UI.WebControls.HyperLink Ver;
		protected System.Web.UI.WebControls.Label lblError;
		int  agencia;
		long Planilla;
		bool error_entrada;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				if(Request.QueryString["act"]!=null)
				{
                    Utils.MostrarAlerta(Response, "' " + Request.QueryString["mensaje"] + " .");
					lblError.Text = Request.QueryString["mensaje"] + " Planilla numero: " + Request.QueryString["pla"];
				}
			}
					
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
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnConsultaPlanilla.Click += new System.EventHandler(this.btnConsultarPlanilla_Click);
			this.btnReportePlanilla.Click += new System.EventHandler(this.btnReportePlanilla_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		private void ValidarEntrada()

		{
			agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
			error_entrada = false;	
			lblError.Text = "";
			try
			{
				Planilla = long.Parse(NroPlanilla.Text.Trim());
				if(Planilla<=0)throw(new Exception());
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Numero Planilla no es válido.");
				error_entrada = true;
				lblError.Text = "Numero Planilla no es válido. ";
			}
			string CodigoAgencia = DBFunctions.SingleData("select MAG_CODIGO from DBXSCHEMA.MPLANILLAVIAJE where MPLA_CODIGO="+Planilla+";");
			if(CodigoAgencia.Length==0)
			{
                Utils.MostrarAlerta(Response, "Numero Planilla no Existe.");
				error_entrada  = true;
				lblError.Text += "Numero Planilla no Existe. ";
			}
			else
				if(Convert.ToInt16(CodigoAgencia) !=agencia)
				{
                    Utils.MostrarAlerta(Response, "Numero Planilla No es de la Agencia.");
					error_entrada = true;
					lblError.Text += "Numero Planilla No es de la Agencia. ";	
				}
			Ver.Visible = false;;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
			
		}
		private void btnConsultarPlanilla_Click(object sender, System.EventArgs e)
		{
			ValidarEntrada();
			if(error_entrada == false)
				{			
				string Comando = "BorrarPlanilla";

				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.PlanillaBus&pln="+Planilla+ "&comando=" +Comando +"&agencia=" +agencia+"&act=2&path=Borrar Planilla Viaje");
				}
			else
				lblError.Text += "<Error en datos de entrada>";
		}

				
		private void btnReportePlanilla_Click(object sender, System.EventArgs e)
		{
			ValidarEntrada();
			if(error_entrada == false)
				GenerarReporte();
			else
				lblError.Text += "<Error en datos de entrada>";

		}
		private void GenerarReporte()
		{
			
			ExportOptions exportOpts = new ExportOptions();
			DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
			PdfRtfWordFormatOptions PDFFormatOpts = new PdfRtfWordFormatOptions();
			ExcelFormatOptions EXCFormatOpts = new ExcelFormatOptions();
			TableLogOnInfo InfoConexBd;
			Ver.Visible=false;
			//FormulaFieldDefinitions crFormulas;
			try
			{
				
				string nomuser=ConfigurationManager.AppSettings["UID"];
				string password=ConfigurationManager.AppSettings["PWD"];
				AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
				miCripto.IV=ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
				miCripto.Key=ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
				string passw=miCripto.DescifrarCadena(password);
				
				ReportDocument oRpt = new ReportDocument(); 
				
				string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "AMS.Comercial.ReportePlanillaViaje.rpt";
				oRpt.Load(NombreReporte);


				/*for(int i=0;i<oRpt.DataDefinition.ParameterFields.Count;i++)
				{
					string nombreReportePadre=oRpt.DataDefinition.ParameterFields[i].ReportName;
					if(nombreReportePadre=="")
						lblError.Text+=oRpt.DataDefinition.ParameterFields[i].Name+"<BR>";
				}*/

				string tem = "";
				foreach(CrystalDecisions.CrystalReports.Engine.Table tabla1 in oRpt.Database.Tables)
				{
					tem = tabla1.Name.ToString();
					InfoConexBd = tabla1.LogOnInfo;
					InfoConexBd.ConnectionInfo.UserID = nomuser;
					InfoConexBd.ConnectionInfo.Password = passw;
					InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase"];
					tabla1.ApplyLogOnInfo(InfoConexBd);
				}
				try
				{
					ParameterDiscreteValue paramPlanilla=new ParameterDiscreteValue();
					ParameterValues currentPlanilla=new ParameterValues();
					paramPlanilla.Value=Planilla;
					
					currentPlanilla.Add(paramPlanilla);
					
					oRpt.DataDefinition.ParameterFields["NUMERO PLANILLA"].ApplyCurrentValues(currentPlanilla);
					
				}
				catch(Exception e)
				{
					lblError.Text+=e.Message;
				}
				
				exportOpts = oRpt.ExportOptions;
				exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
				//exportOpts.ExportFormatType = ExportFormatType.ExcelRecord;
				exportOpts.FormatOptions = PDFFormatOpts;
				//exportOpts.FormatOptions = EXCFormatOpts;

				exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReportePlanillaViaje"+Planilla+".pdf";
				//diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				exportOpts.DestinationOptions = diskOpts;
				oRpt.Export();
				Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReportePlanillaViaje"+Planilla+".pdf";
				//Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				Ver.Visible = true;
			}
			catch (Exception ex)
			{
				lblError.Text+=ex.Message;
			}
		}

	}

}
