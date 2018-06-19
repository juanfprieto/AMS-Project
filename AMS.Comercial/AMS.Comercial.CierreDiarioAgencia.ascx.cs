using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
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
using AMS.Documentos;

namespace AMS.Comercial
{

	/// <summary>
	///		Descripción breve de AMS_Comercial_CierreDiarioAgencia.
	/// </summary>
	public class AMS_Comercial_CierreDiarioAgencia : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Button btnGenerar;
		protected System.Web.UI.WebControls.HyperLink Ver;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgrVentas;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblTotalIngresos;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblTotalEgresos;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblTotal;
		protected System.Web.UI.WebControls.TextBox txtFCierre;
		protected System.Web.UI.WebControls.TextBox txtDocumento;
		protected System.Web.UI.WebControls.Label Label36;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblDiferencia;
		protected System.Web.UI.WebControls.Button btnVerCierre;
		protected System.Web.UI.WebControls.Button btnCerrar;
		protected System.Web.UI.WebControls.TextBox txtValorConsignado;
		protected System.Web.UI.WebControls.Panel pnlConsulta;
		//protected System.Web.UI.WebControls.HyperLink Ver;
		double ValorConsignado;
		int agencia;
		int Documento;
		DateTime fechaCierre;
		string responsable;
        //AMS.documentos impresion;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				txtFCierre.Text=DateTime.Now.ToString("yyyy-MM-dd");
				btnCerrar.Visible=false;

			}
			
			if(IsPostBack)
			{
                //cargarInformacio();
				btnCerrar.Visible=false;
				Ver.Visible = false;
                lblError.Text = " ";
			}
			
}
        private void cargarInformacio(){
        
            agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
            try { fechaCierre = Convert.ToDateTime(txtFCierre.Text); }
				catch
				{
                    Utils.MostrarAlerta(Response, "Fecha Cierre no válida.");
					return;}
				try
				{
					Documento = int.Parse(txtDocumento.Text.Trim());
					if(Documento<=0)throw(new Exception());
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Documento no es válido.");
					return;
				}
				try
				{
					ValorConsignado = double.Parse(txtValorConsignado.Text.Trim());
					if(ValorConsignado<0)throw(new Exception());
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "valor consignado no es válido.");
					return;
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
			this.btnVerCierre.Click += new System.EventHandler(this.btnVerCierre_Click);
			this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
			this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
    
	private void btnVerCierre_Click(object sender, System.EventArgs e)
        {
            cargarInformacio();

		double totalEgresos=0,totalIngresos=0;
		if (DBFunctions.RecordExist("select MAG_CODIGO from DBXSCHEMA.DCIERRE_DIARIO_AGENCIA where MAG_CODIGO =  "+agencia+" and FECHA_CONTABILIZACION = '"+fechaCierre.ToString("yyyy-MM-dd")+"';"))
			btnCerrar.Visible=false;
		else
			btnCerrar.Visible=true;
	    pnlConsulta.Visible=false;
		DataSet dsMovimiento=new DataSet();
		responsable = " ";
		DBFunctions.Request(dsMovimiento,IncludeSchema.NO,"CALL DBXSCHEMA.MOVIMIENTO_AGENCIA("+agencia+",'"+fechaCierre.ToString("yyyy-MM-dd")+"','');");
		dgrVentas.DataSource=dsMovimiento.Tables[0].DefaultView;
		dgrVentas.DataBind();
		pnlConsulta.Visible=true;
		for(int nR=0;nR<dsMovimiento.Tables[0].Rows.Count; nR++)
		{
			totalIngresos+=Convert.ToDouble(dsMovimiento.Tables[0].Rows[nR]["VALOR_DEBITO"]);
			totalEgresos+=Convert.ToDouble(dsMovimiento.Tables[0].Rows[nR]["VALOR_CREDITO"]);
		}
		lblTotalIngresos.Text=totalIngresos.ToString("#,##0");
		lblTotalEgresos.Text=totalEgresos.ToString("#,##0");
        lblTotal.Text = (totalIngresos-totalEgresos).ToString("#,##0");
		lblDiferencia.Text=(totalIngresos-totalEgresos - ValorConsignado).ToString("#,##0");
	}
	private void btnCerrar_Click(object sender, System.EventArgs e)
    {
        cargarInformacio();
			DataSet dsMensaje=new DataSet();
			DBFunctions.Request(dsMensaje,IncludeSchema.NO,"CALL DBXSCHEMA.CERRAR_DIA_AGENCIA("+agencia+",'"+fechaCierre.ToString("yyyy-MM-dd")+"',"+Documento+","+ValorConsignado+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			lblError.Text=dsMensaje.Tables[0].Rows[0]["MENSAJE"].ToString() + dsMensaje.Tables[0].Rows[0]["CODIGO_SQL"].ToString();
            btnCerrar.Visible=false;
		}
	private void btnGenerar_Click(object sender, System.EventArgs e)
    {
        cargarInformacio();
			responsable = " ";
			GenerarReporte(agencia,fechaCierre,responsable);
		}
	private void GenerarReporte(int agencia, DateTime fecha, string responsable)
    {
        cargarInformacio();

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
                string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "ams.comercial.resumenagencia.rpt";
				oRpt.Load(NombreReporte);

                //AMS.Documentos.PreviewReport2("Rpt", "archivo", "PDF", nomuser, passw, "omvistatotal", "nomtotal", "nomformulaletras", "pref", "num");
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
					//currentResponsable.Add(paramResponsable);
					oRpt.DataDefinition.ParameterFields["CODIGO_AGENCIA"].ApplyCurrentValues(currentAgencia);
					oRpt.DataDefinition.ParameterFields["FECHA_INICIAL"].ApplyCurrentValues(currentFecha);
					//oRpt.DataDefinition.ParameterFields["NIT"].ApplyCurrentValues(currentResponsable);
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
