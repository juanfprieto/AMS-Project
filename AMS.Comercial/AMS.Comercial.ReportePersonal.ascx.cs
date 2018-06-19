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
	///		Descripci�n breve de AMS_Comercial_ReportePersonal.
	/// </summary>
	public class AMS_Comercial_ReportePersonal : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Button btnGenerar;
		protected System.Web.UI.WebControls.HyperLink Ver;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtResponsablea;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		protected System.Web.UI.WebControls.Label lblError;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
			}
		}

		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�todo necesario para admitir el Dise�ador. No se puede modificar
		///		el contenido del m�todo con el editor de c�digo.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnGenerar_Click(object sender, System.EventArgs e)
		{
			//Validar
			DateTime fechaPlanilla;
			string responsable=txtResponsable.Text;
			int agencia;
			if(ddlAgencia.SelectedValue.Length==0)
			{	
				Response.Write("<script language='javascript'>alert('Debe dar la agencia.');</script>");
				return;
			}
			agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
			if(responsable.Length==0)responsable=" ";
			try
			{
				fechaPlanilla=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
				Response.Write("<script language='javascript'>alert('Debe dar una fecha v�lida.');</script>");
				return;}

			GenerarReporte(agencia,fechaPlanilla,responsable);
		}
		//Generar pdf
		private void GenerarReporte(int agencia, DateTime fecha, string responsable)
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
                string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "AMS.Comercial.ReporteAgenciaTaquillero.rpt";
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
					ParameterDiscreteValue paramAgencia=new ParameterDiscreteValue();
					ParameterDiscreteValue paramFecha=new ParameterDiscreteValue();
					ParameterDiscreteValue paramResponsable=new ParameterDiscreteValue();
					ParameterValues currentAgencia=new ParameterValues();
					ParameterValues currentFecha=new ParameterValues();
					ParameterValues currentResponsable=new ParameterValues();
					paramAgencia.Value=agencia;
					paramFecha.Value=fecha;
					paramResponsable.Value=responsable;
					currentAgencia.Add(paramAgencia);
					currentFecha.Add(paramFecha);
					currentResponsable.Add(paramResponsable);
					oRpt.DataDefinition.ParameterFields["CODIGO_AGENCIA"].ApplyCurrentValues(currentAgencia);
					oRpt.DataDefinition.ParameterFields["FECHA"].ApplyCurrentValues(currentFecha);
					oRpt.DataDefinition.ParameterFields["NIT"].ApplyCurrentValues(currentResponsable);
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
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".pdf";
				//diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				exportOpts.DestinationOptions = diskOpts;
				oRpt.Export();
				Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".pdf";
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