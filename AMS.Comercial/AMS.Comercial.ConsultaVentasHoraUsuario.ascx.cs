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
	public class AMS_Comercial_ConsultaVentasHoraUsuario : System.Web.UI.UserControl
	{
		#region Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.HyperLink Ver;
		protected System.Web.UI.WebControls.TextBox txtResponsablea;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label6;
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
		protected System.Web.UI.WebControls.Panel pnlConsulta;
		protected System.Web.UI.WebControls.Panel pnlAgencia;
		protected System.Web.UI.WebControls.Button btnConsultar;
		protected System.Web.UI.WebControls.TextBox txtFechaI;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtFechaF;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblTotalIngresos;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblTotalEgresos;
		protected System.Web.UI.WebControls.Label lblTotal;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnGenerar;
		protected System.Web.UI.WebControls.Label Label99;
		protected System.Web.UI.WebControls.DropDownList ddlHoraI;
		protected System.Web.UI.WebControls.DropDownList ddlMinutoI;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList ddlHoraF;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label lblCodigoAgencia;
		protected System.Web.UI.WebControls.Label lblNombreAgencia;
		protected System.Web.UI.WebControls.Label lblValorGiros;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.DropDownList ddlDespachador;
		protected System.Web.UI.WebControls.DropDownList ddlMinutoF;
		#endregion Controles

		int agencia;
		DateTime fechaI, fechaF;
			
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				txtFechaI.Text=txtFechaF.Text=DateTime.Now.ToString("yyyy-MM-dd");
				for(int i=0;i<24;i++)
				{
					ddlHoraI.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
					ddlHoraF.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				}
				for(int i=0;i<60;i++)
				{
					ddlMinutoI.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
					ddlMinutoF.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				}
			}

			
		}
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			if(ddlAgencia.SelectedValue.Length==0)
			{	
                Utils.MostrarAlerta(Response, "Debe dar la agencia.");
				return;
			}

			agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
			int CargoDespachador =  Convert.ToInt16(DBFunctions.SingleData("SELECT PCAR_CODIGO FROM DBXSCHEMA.PCARGOS_TRANSPORTES  WHERE PCAR_FILTRO='D';"));
			Agencias.TraerEmpleadosAgencia(ddlDespachador,agencia,CargoDespachador);
			ListItem itm=new ListItem("TODOS"," ");
			ddlDespachador.Items.Insert(0,itm);
			string NitUsuario =  DBFunctions.SingleData("SELECT su.mnit_nit FROM DBXSCHEMA.susuario su WHERE  su.susu_login = '"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(DBFunctions.RecordExist("SELECT * from DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP "+
				"where mp.mag_codigo = "+agencia+"  AND mnit_nit = '"+NitUsuario+"' and pcar_codigo = "+CargoDespachador+";"))
			{
				ddlDespachador.SelectedIndex= ddlDespachador.Items.IndexOf(ddlDespachador.Items.FindByValue(NitUsuario.ToString()));
				ddlDespachador.Enabled=false;
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnConsultar_Click(object sender, System.EventArgs e)
		{
			double totalTiquetes=0,totalGiros=0,totalEgresos=0,totalRemesas=0,totalIngresos=0,totalValorGiros=0;
			agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
			Verificar_fecha();
			pnlConsulta.Visible=false;
			lblCodigoAgencia.Text=agencia.ToString();
			lblNombreAgencia.Text=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");   
			DataSet dsUsuario=new DataSet();
			DBFunctions.Request(dsUsuario,IncludeSchema.NO,"CALL DBXSCHEMA.MOVIMIENTO_TAQUILLERO_HORA("+agencia+",'"+fechaI.ToString("yyyy-MM-dd HH:mm:ss")+"','"+fechaF.ToString("yyyy-MM-dd HH:mm:ss")+"','"+ddlDespachador.SelectedValue+"');");
			dgrVentas.DataSource=dsUsuario.Tables[0].DefaultView;
			dgrVentas.DataBind();
			pnlConsulta.Visible=true;
			for(int nR=0;nR<dsUsuario.Tables[0].Rows.Count; nR++)
			{
				totalTiquetes+=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["VALOR_TIQUETES"]);
				totalRemesas+=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["ENCOMIENDAS"]);
				totalGiros+=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["COSTO_GIROS"]);
				totalValorGiros+=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["VALOR_GIROS"]);
				totalIngresos+=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["VALOR_INGRESOS"]);
				totalEgresos+=Convert.ToDouble(dsUsuario.Tables[0].Rows[nR]["VALOR_EGRESOS"]);
			}
			lblTotalTiquetes.Text=totalTiquetes.ToString("#,##0");
			lblTotalRemesas.Text=totalRemesas.ToString("#,##0");
			lblTotalGiros.Text=totalGiros.ToString("#,##0");
			lblTotalIngresos.Text=totalIngresos.ToString("#,##0");
			lblTotalEgresos.Text=totalEgresos.ToString("#,##0");
			lblTotal.Text=((totalTiquetes+totalRemesas+totalGiros+totalIngresos)-totalEgresos).ToString("#,##0");
			lblValorGiros.Text = totalValorGiros.ToString("#,##0");
 			
		}
		private void btnGenerar_Click(object sender, System.EventArgs e)
		{
			
			agencia=Convert.ToInt16(ddlAgencia.SelectedValue);
			Verificar_fecha();
			GenerarConsulta(agencia,fechaI,fechaF,ddlDespachador.SelectedValue);
		}
		
		private void GenerarConsulta(int agencia, DateTime FechaI, DateTime FechaF, string responsable)
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
				string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "AMS.Comercial.ReporteAgenciaTaquilleroHora.rpt";
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
					ParameterDiscreteValue paramFechaI=new ParameterDiscreteValue();
                    ParameterDiscreteValue paramFechaF=new ParameterDiscreteValue();
					ParameterDiscreteValue paramResponsable=new ParameterDiscreteValue();
					ParameterValues currentAgencia=new ParameterValues();
					ParameterValues currentFechaI=new ParameterValues();
                    ParameterValues currentFechaF=new ParameterValues();
					ParameterValues currentResponsable=new ParameterValues();
					paramAgencia.Value=agencia;
					paramFechaI.Value=FechaI;
                    paramFechaF.Value=FechaF;
					paramResponsable.Value=responsable;
					currentAgencia.Add(paramAgencia);
					currentFechaI.Add(paramFechaI);
                    currentFechaF.Add(paramFechaF);
					currentResponsable.Add(paramResponsable);
					oRpt.DataDefinition.ParameterFields["CODIGO_AGENCIA"].ApplyCurrentValues(currentAgencia);
					oRpt.DataDefinition.ParameterFields["FECHA_INICIAL"].ApplyCurrentValues(currentFechaI);
                    oRpt.DataDefinition.ParameterFields["FECHA_FINAL"].ApplyCurrentValues(currentFechaF);
					oRpt.DataDefinition.ParameterFields["NIT"].ApplyCurrentValues(currentResponsable);
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
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteAgenciaTaquilleroHora"+agencia+"_"+FechaI.ToString("yyyy-MM-dd")+".pdf";
				//diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteAgenciaTaquilleroHora"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				exportOpts.DestinationOptions = diskOpts;
				oRpt.Export();
				Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteAgenciaTaquilleroHora"+agencia+"_"+FechaI.ToString("yyyy-MM-dd")+".pdf";
				//Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteAgenciaTaquilleroHora"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				Ver.Visible = true;
			}
			catch (Exception ex)
			{
				lblError.Text+=ex.Message;
				return;
			}
		
		}
	
	
	public void Verificar_fecha()
	{
		
		if (ddlHoraI.SelectedIndex<0 && ddlMinutoI.SelectedIndex<0)
		{
        Utils.MostrarAlerta(Response, "Debe seleccionar digitar la hora-minuto Inicial");
		return;
		}

		if (ddlHoraF.SelectedIndex<0 && ddlMinutoF.SelectedIndex<0)

		{
            Utils.MostrarAlerta(Response, "Debe seleccionar digitar la hora-minuto Final");
			return;
		}

		try{fechaI=Convert.ToDateTime(txtFechaI.Text+" "+ddlHoraI.SelectedValue+":"+ddlMinutoI.SelectedValue+":00");}
		catch
		{
            Utils.MostrarAlerta(Response, "Fecha inicial no válida.");
			return;}
		try{fechaF=Convert.ToDateTime(txtFechaF.Text+" "+ddlHoraF.SelectedValue+":"+ddlMinutoF.SelectedValue+":00");}
		catch
		{
        Utils.MostrarAlerta(Response, "Fecha final no válida.");
		return;}
		if(fechaF<fechaI)
		{
            Utils.MostrarAlerta(Response, "La fecha final es menor a la fecha inicial.");
			return;}
		if(fechaF.Subtract(fechaI).Days>31)
		{
            Utils.MostrarAlerta(Response, "No puede consultar mas de un mes a la vez.");
			return;
		}
		}
	}
	}
