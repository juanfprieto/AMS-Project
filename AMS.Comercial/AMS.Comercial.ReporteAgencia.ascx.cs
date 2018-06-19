using System;
using System.Text;
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
	///		Descripción breve de AMS_Comercial_ReporteAgencia.
	/// </summary>
	public class AMS_Comercial_ReporteAgencia : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Button btnGenerar;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgrVentas;
		protected System.Web.UI.WebControls.Panel pnlConsulta;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.HyperLink Ver;
		int agencia;
		protected System.Web.UI.WebControls.Button btnExportarExcel;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblTotalTiquetes;	
		protected System.Web.UI.WebControls.Label lblTotalRemesas;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblTotalGiros;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblTotalIngresos;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblTotalEgresos;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblTotalConsignar;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label lblTotalValorGiros;
		protected System.Web.UI.WebControls.Label lblTotalDescuentos;
		protected System.Web.UI.WebControls.Label Label8;
		DateTime fechaPlanilla;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
			}
			//Validar
			


		}
        private bool validarDatos(){
            if (ddlAgencia.SelectedValue.Length == 0)
            {
                Response.Write("<script language='javascript'>alert('Debe dar la agencia.');</script>");
                return (false);
            }
            agencia = Convert.ToInt16(ddlAgencia.SelectedValue);
            try
            {
                fechaPlanilla = Convert.ToDateTime(txtFecha.Text);
            }
            catch
            {
                Response.Write("<script language='javascript'>alert('Debe dar una fecha válida.');</script>");
                return(false);
            }
            return (true);
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
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
			this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			double totalTiquetes=0,totalGiros=0,totalEgresos=0,totalRemesas=0,totalIngresos=0,totalValorGiros=0,totalDescuentos=0;
            if(!validarDatos())return;
		
			pnlConsulta.Visible=false;
		           
		
			DataSet dsResumenDia=new DataSet();
			DBFunctions.Request(dsResumenDia,IncludeSchema.NO,"CALL DBXSCHEMA.RESUMEN_DIA_AGENCIA("+agencia+",'"+fechaPlanilla.ToString("yyyy-MM-dd")+"');");
			dgrVentas.DataSource=dsResumenDia.Tables[0].DefaultView;
			dgrVentas.DataBind();
			
			pnlConsulta.Visible=true;
			for(int nR=0;nR<dsResumenDia.Tables[0].Rows.Count; nR++)
			{
				totalTiquetes+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["valor_tiquetes"]);
				totalDescuentos+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["descuentos"]);
				totalRemesas+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["encomiendas"]);
				totalGiros+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["costo_giros"]);
				totalValorGiros+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["valor_giros"]);
				totalIngresos+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["valor_ingresos"]);
				totalEgresos+=Convert.ToDouble(dsResumenDia.Tables[0].Rows[nR]["valor_egresos"]);

			}
			lblTotalTiquetes.Text=totalTiquetes.ToString("#,##0");
			lblTotalDescuentos.Text=totalDescuentos.ToString("#,##0");
			lblTotalRemesas.Text=totalRemesas.ToString("#,##0");
			lblTotalGiros.Text=totalGiros.ToString("#,##0");
			lblTotalIngresos.Text=totalIngresos.ToString("#,##0");
			lblTotalEgresos.Text=totalEgresos.ToString("#,##0");
			
			lblTotalConsignar.Text=((totalTiquetes-totalDescuentos+totalRemesas+totalGiros+totalIngresos)-totalEgresos).ToString("#,##0");
 			lblTotalValorGiros.Text = totalValorGiros.ToString("#,##0");
		}

	
		protected void btnExportarExcel_Click(object sender, EventArgs e) 
		{		
			//MakeReport(this,null);
            if (!validarDatos()) return;
			DateTime fecha = DateTime.Now;
			string nombreArchivo = "ResumenAgencia"+"_"+agencia+"_"+fechaPlanilla.ToString("yyyy-MM-dd")+"_"+fecha.ToString("yyyy-MM-dd HH:mm:ss");
			Response.Clear();
			Response.AddHeader("content-disposition", "attachment;filename="+nombreArchivo+".xls");
			Response.Charset = "Unicode";
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.ContentType = "application/vnd.xls";
			Response.ContentEncoding=System.Text.Encoding.Default; 
			this.EnableViewState=false; 
			DataSet dsResumenDia=new DataSet();
			DBFunctions.Request(dsResumenDia,IncludeSchema.NO,"CALL DBXSCHEMA.RESUMEN_DIA_AGENCIA("+agencia+",'"+fechaPlanilla.ToString("yyyy-MM-dd")+"');");
			DataGrid dg=new DataGrid(); 
			dg.DataSource=dsResumenDia.Tables[0].DefaultView; 
			dg.DataBind(); 
			System.IO.StringWriter tw = new System.IO.StringWriter(); 
			System.Web.UI.HtmlTextWriter hw = new HtmlTextWriter(tw); 
			dg.RenderControl(hw); 
			Response.Write(tw.ToString()); 
			Response.End(); 
		}
			
	
		private void btnGenerar_Click(object sender, System.EventArgs e)
		{

            if (!validarDatos()) return;
            GenerarReporte(agencia,fechaPlanilla);
		}

		//Generar pdf
		private void GenerarReporte(int agencia, DateTime fecha)
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
				string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "AMS.Comercial.ResumenDiaAgencia.rpt";
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
					ParameterValues currentAgencia=new ParameterValues();
					ParameterValues currentFecha=new ParameterValues();
					paramAgencia.Value=agencia;
					paramFecha.Value=fecha;
					currentAgencia.Add(paramAgencia);
					currentFecha.Add(paramFecha);
					oRpt.DataDefinition.ParameterFields["CodigoAgencia"].ApplyCurrentValues(currentAgencia);
					oRpt.DataDefinition.ParameterFields["FechaPlanillas"].ApplyCurrentValues(currentFecha);
				}
				catch(Exception e)
				{
					lblError.Text+=e.Message;
					return;
				}
				
				exportOpts = oRpt.ExportOptions;
				exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
				//exportOpts.ExportFormatType = ExportFormatType.ExcelRecord;
				exportOpts.FormatOptions = PDFFormatOpts;
				//exportOpts.FormatOptions = EXCFormatOpts;

				exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioResumenAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".pdf";
				//diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				exportOpts.DestinationOptions = diskOpts;
				oRpt.Export();
				Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteDiarioResumenAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".pdf";
				//Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				Ver.Visible = true;
			}
			 catch (Exception ex)
			{
				lblError.Text+=ex.Message;
				return;
			}
		}

	}
}
