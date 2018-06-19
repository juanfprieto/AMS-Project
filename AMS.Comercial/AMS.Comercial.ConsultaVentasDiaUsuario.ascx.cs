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

namespace AMS.Comercial
{
	/// <summary>
	///		Descripción breve de AMS_Comercial_ConsultaVentasDiaUsuario.
	/// </summary>
	public class AMS_Comercial_ConsultaVentasDiaUsuario : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.HyperLink Ver;
		
		protected System.Web.UI.WebControls.TextBox txtResponsablea;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		
		#region Controles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtNITTiquetero;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.DataGrid dgrVentas;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblTotalTiquetes;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblTotalRemesas;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblTotalGiros;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblTotal;
		protected System.Web.UI.WebControls.Panel pnlConsulta;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.TextBox txtFechaI;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtFechaF;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblTotalIngresos;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblTotalEgresos;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnGenerar;
		#endregion Controles

		int agencia;
		string responsable;
        DateTime fechaI, fechaF;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				txtFechaI.Text=txtFechaF.Text=DateTime.Now.ToString("yyyy-MM-dd");
			}

			responsable=txtNITTiquetero.Text;
			if(ddlAgencia.SelectedValue.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe dar la agencia.");
				return;
			}
            //cargarinformacion();

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
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);

			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion


        private void cargarinformacion() {

            agencia = Convert.ToInt16(ddlAgencia.SelectedValue);
            responsable = txtNITTiquetero.Text;

            try { fechaI = Convert.ToDateTime(txtFechaI.Text); }
            catch
            {
                Utils.MostrarAlerta(Response, "Fecha inicial no válida.");
                return;
            }
            try { fechaF = Convert.ToDateTime(txtFechaF.Text); }
            catch
            {
                Utils.MostrarAlerta(Response, "Fecha final no válida.");
                return;
            }
            if (fechaF < fechaI)
            {
                Utils.MostrarAlerta(Response, "La fecha final es menor a la fecha inicial.");
                return;
            }
            if (fechaF.Subtract(fechaI).Days > 31)
            {
                Utils.MostrarAlerta(Response, "No puede consultar mas de un mes a la vez.");
                return;
            }
        
        }

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			double totalTiquetes=0,totalGiros=0,totalEgresos=0,totalRemesas=0,totalIngresos=0;
            cargarinformacion();
			pnlConsulta.Visible=false;
		           
/*
			sqlC="Select mv.mcat_placa mcat_placa, mb.mbus_numero mbus_numero, mp.mpla_codigo mpla_codigo,"+
			"(SELECT count(*) FROM DBXSCHEMA.MTIQUETE_VIAJE mtn where mtn.mpla_codigo=mp.mpla_codigo and mtn.test_codigo='V') tiquetes,"+
			"(SELECT coalesce(sum(mtt.valor_total),0) FROM DBXSCHEMA.MTIQUETE_VIAJE mtt where mtt.mpla_codigo=mp.mpla_codigo and mtt.test_codigo='V') total_tiquetes,"+
			"(SELECT count(*) FROM DBXSCHEMA.MENCOMIENDAS men where men.mpla_codigo=mp.mpla_codigo) remesas,"+
			"(SELECT coalesce(sum(met.costo_encomienda),0) FROM DBXSCHEMA.MENCOMIENDAS met where met.mpla_codigo=mp.mpla_codigo) total_remesas,"+
			"(SELECT count(*) FROM DBXSCHEMA.MGIROS mgn where mgn.mpla_codigo=mp.mpla_codigo) giros,"+
			"(SELECT coalesce(sum(mgt.costo_giro),0) FROM DBXSCHEMA.MGIROS mgt where mgt.mpla_codigo=mp.mpla_codigo) total_giros,"+
			"(SELECT count(*) FROM DBXSCHEMA.MGASTOS_TRANSPORTES man where man.mpla_codigo=mp.mpla_codigo) anticipos,"+
			"(SELECT coalesce(sum(mat.valor_total_autorizado),0) FROM DBXSCHEMA.MGASTOS_TRANSPORTES mat where mat.mpla_codigo=mp.mpla_codigo) total_anticipos,"+
			"((SELECT coalesce(sum(mttt.valor_total),0) FROM DBXSCHEMA.MTIQUETE_VIAJE mttt where mttt.mpla_codigo=mp.mpla_codigo and mttt.test_codigo='V')+"+
			"(SELECT coalesce(sum(mett.costo_encomienda),0) FROM DBXSCHEMA.MENCOMIENDAS mett where mett.mpla_codigo=mp.mpla_codigo)+"+
			"(SELECT coalesce(sum(mgtt.costo_giro),0) FROM DBXSCHEMA.MGIROS mgtt where mgtt.mpla_codigo=mp.mpla_codigo))-"+
			"((SELECT coalesce(sum(matt.valor_total_autorizado),0) FROM DBXSCHEMA.MGASTOS_TRANSPORTES matt where matt.mpla_codigo=mp.mpla_codigo))"+
			" total "+
			"from DBXSCHEMA.MPLANILLAVIAJE mp,DBXSCHEMA.mviaje mv,DBXSCHEMA.mbusafiliado mb "+
			"where "+
			"mp.mrut_codigo=mv.mrut_codigo and mv.mviaje_numero=mp.mviaje_numero and mv.mcat_placa=mb.mcat_placa and "+
			"(mp.fecha_planilla between '"+fechaI.ToString("yyyy-MM-dd")+"' and '"+fechaF.ToString("yyyy-MM-dd")+"') AND mp.mag_codigo="+agencia+" AND mp.mnit_responsable='"+responsable+"' "+
			"ORDER BY MPLA_CODIGO;";
*/			
			DataSet dsUsuario=new DataSet();
            //DBFunctions.Request(dsUsuario, IncludeSchema.NO, "CALL DBXSCHEMA.CONSULTA_TAQUILLERO('" + fechaI.ToString("yyyy-MM-dd") + "','" + fechaF.ToString("yyyy-MM-dd") + "', " + agencia + ",'" + responsable + "');");
            DBFunctions.Request(dsUsuario, IncludeSchema.NO, "CALL DBXSCHEMA.CONSULTA_TAQUILLERO(" + agencia + ",'" + responsable + "','" + fechaI.ToString("yyyy-MM-dd") + "','" + fechaF.ToString("yyyy-MM-dd") + "');");
			dgrVentas.DataSource=dsUsuario.Tables[0].DefaultView;
			dgrVentas.DataBind();
			pnlConsulta.Visible=true;
			for(int nR=0;nR<dsUsuario.Tables[0].Rows.Count; nR++)
			{
				totalTiquetes   +=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["valor_tiquetes"]);
				totalRemesas    +=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["valor_encomiendas"]);
				totalGiros      +=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["valor_giros"]);
                totalIngresos   +=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["ingresos"]);
				totalEgresos    +=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["egresos"]);
			}
			lblTotalTiquetes.Text =totalTiquetes.ToString("#,##0");
			lblTotalRemesas.Text  =totalRemesas.ToString("#,##0");
			lblTotalGiros.Text    =totalGiros.ToString("#,##0");
			lblTotalIngresos.Text =totalIngresos.ToString("#,##0");
            lblTotalEgresos.Text  =totalEgresos.ToString("#,##0");
			lblTotal.Text         =((totalTiquetes+totalRemesas+totalGiros+totalIngresos)-totalEgresos).ToString("#,##0");
 			
		}
		private void btnGenerar_Click(object sender, System.EventArgs e)
		{
          cargarinformacion();
		  string parar = "boton generar";
          GenerarConsulta(fechaI, fechaF, agencia, responsable);
        }

        private void GenerarConsulta(DateTime FechaI, DateTime FechaF, int agencia, string responsable)
        //private void GenerarConsulta(int agencia, DateTime FechaI, DateTime FechaF, string responsable)
		{
            cargarinformacion();
			ExportOptions exportOpts = new ExportOptions();
			DiskFileDestinationOptions diskOpts   = new DiskFileDestinationOptions();
			PdfRtfWordFormatOptions PDFFormatOpts = new PdfRtfWordFormatOptions();
			ExcelFormatOptions EXCFormatOpts      = new ExcelFormatOptions();
			TableLogOnInfo InfoConexBd;
			Ver.Visible=false;
			//FormulaFieldDefinitions crFormulas;
			try
			{
				string nomuser  =ConfigurationManager.AppSettings["UID"];
				string password =ConfigurationManager.AppSettings["PWD"];
				AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
				miCripto.IV     =ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
				miCripto.Key    =ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
				string passw    =miCripto.DescifrarCadena(password);
				
				ReportDocument oRpt = new ReportDocument();
                string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "AMS.Comercial.ReporteConsultaAgencia.rpt";
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
					ParameterDiscreteValue paramFechaI= new ParameterDiscreteValue();
                    ParameterDiscreteValue paramFechaF= new ParameterDiscreteValue();
					ParameterDiscreteValue paramResponsable=new ParameterDiscreteValue();
					ParameterValues currentAgencia    = new ParameterValues();
					ParameterValues currentFechaI     = new ParameterValues();
                    ParameterValues currentFechaF     = new ParameterValues();
					ParameterValues currentResponsable= new ParameterValues();
					paramAgencia.Value      = agencia;
					paramFechaI.Value       = FechaI;
                    paramFechaF.Value       = FechaF;
					paramResponsable.Value  = responsable;
                    currentFechaI.Add(paramFechaI);
                    currentFechaF.Add(paramFechaF);
					currentAgencia.Add(paramAgencia);
					currentResponsable.Add(paramResponsable);
                    oRpt.DataDefinition.ParameterFields["FECHA_INICIAL"].ApplyCurrentValues(currentFechaI);
                    oRpt.DataDefinition.ParameterFields["FECHA_FINAL"].ApplyCurrentValues(currentFechaF);
					oRpt.DataDefinition.ParameterFields["CODIGO_AGENCIA"].ApplyCurrentValues(currentAgencia);	
					oRpt.DataDefinition.ParameterFields["NIT"].ApplyCurrentValues(currentResponsable);
                    /*oRpt.DataDefinition.ParameterFields["CODIGO_AGENCIA"].ApplyCurrentValues(currentAgencia);
                    oRpt.DataDefinition.ParameterFields["FECHA_INICIAL"].ApplyCurrentValues(currentFechaI);
                    oRpt.DataDefinition.ParameterFields["FECHA_FINAL"].ApplyCurrentValues(currentFechaF);
                    oRpt.DataDefinition.ParameterFields["NIT"].ApplyCurrentValues(currentResponsable);*/
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
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteConsultaAgencia"+agencia+"_"+FechaI.ToString("yyyy-MM-dd")+ ".pdf";
				//diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				exportOpts.DestinationOptions = diskOpts;
				oRpt.Export();
				Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteConsultaAgencia"+agencia+"_"+FechaI.ToString("yyyy-MM-dd")+ ".pdf";
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
