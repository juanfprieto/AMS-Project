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
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AMS.CriptoServiceProvider;
using AMS.Tools;

namespace AMS.Comercial
{
	/// <summary>
	///	Descripción breve de AMS_Comercial_IngresosGastos.
	/// </summary>
	public class AMS_Comercial_IngresosGastos : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblAgencia;
		protected System.Web.UI.WebControls.Label lblNombreAgencia;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox txtNITReceptor;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtNumDocReferencia;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtCantidad;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox txtValorUnidad;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.TextBox txtValorTotal;
		protected System.Web.UI.WebControls.TextBox TextPlanilla;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox TextDocumento;
		protected System.Web.UI.WebControls.Button BtnModificar;
		protected System.Web.UI.WebControls.Button BtnBorrar;
		protected System.Web.UI.WebControls.TextBox txtNombreReceptor;
		protected System.Web.UI.WebControls.TextBox txtApellidoReceptor;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button BtnRegresar;
		protected System.Web.UI.WebControls.Button btnImprimir;
		protected System.Web.UI.WebControls.HyperLink Ver;
		
		DateTime fecha;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request.QueryString["Documento"]!=null)
				   VerAnticipo(Request.QueryString["Documento"]);
			}
		}

		public void VerAnticipo(string Documento)
		{
			DatasToControls bind=  new DatasToControls();
			txtCantidad.Attributes.Add("onkeyup","NumericMask(this);Totales();");
			txtValorUnidad.Attributes.Add("onkeyup","NumericMask(this);Totales();");	
			
			//ViewState["Documento"]=Documento;
			DataSet dsAnticipo=new DataSet();
			DataSet dsNits=new DataSet();
			string nitReceptor;
			
			DBFunctions.Request(dsAnticipo,IncludeSchema.NO, "SELECT *  FROM DBXSCHEMA.MGASTOS_TRANSPORTES WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO="+Documento+";");
			if(dsAnticipo.Tables[0].Rows.Count==0)return;
			
			//Conceptos

			bind.PutDatasIntoDropDownList(ddlConcepto,"SELECT TCON_CODIGO, NOMBRE from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE tdoc_codigo = 'ANT';");
			
			//posiciona el concepto leido.
			ddlConcepto.SelectedIndex=ddlConcepto.Items.IndexOf(ddlConcepto.Items.FindByValue(dsAnticipo.Tables[0].Rows[0]["TCON_CODIGO"].ToString()));

			lblAgencia.Text = dsAnticipo.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			string NombreAgencia = DBFunctions.SingleData("select mage_nombre from DBXSCHEMA.MAGENCIA where MAG_CODIGO ="+lblAgencia.Text+";");
			lblNombreAgencia.Text = NombreAgencia.ToString();	
			nitReceptor=dsAnticipo.Tables[0].Rows[0]["MNIT_RESPONSABLE_RECIBE"].ToString();	
			txtPlaca.Text = dsAnticipo.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			TextDocumento.Text = dsAnticipo.Tables[0].Rows[0]["NUM_DOCUMENTO"].ToString();
			TextDocumento.Enabled=false;
			TextPlanilla.Text = dsAnticipo.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			//TextPlanilla.Enabled=false;
			txtNITReceptor.Text=dsAnticipo.Tables[0].Rows[0]["MNIT_RESPONSABLE_RECIBE"].ToString();
			txtNumDocReferencia.Text = dsAnticipo.Tables[0].Rows[0]["NUM_DOC_REFERENCIA"].ToString();
			txtFecha.Text = (Convert.ToDateTime(dsAnticipo.Tables[0].Rows[0]["FECHA_DOCUMENTO"])).ToString("yyyy-MM-dd");
			txtDescripcion.Text = dsAnticipo.Tables[0].Rows[0]["DESCRIPCION"].ToString();
			//dlConcepto.SelectedValue.Text = dsAnticipo.Tables[0].Rows[0]["TCON_CODIGO"].ToString();				
			double Cantidad=Convert.ToDouble(dsAnticipo.Tables[0].Rows[0]["CANTIDAD_CONSUMO"]);
			txtCantidad.Text = Cantidad.ToString("#,###,##0.##");
			double ValorUnidad = Convert.ToDouble(dsAnticipo.Tables[0].Rows[0]["VALOR_UNIDAD"]);
			txtValorUnidad.Text = ValorUnidad.ToString("#,###,##0");  
			double ValorTotal = Convert.ToDouble(dsAnticipo.Tables[0].Rows[0]["VALOR_TOTAL_AUTORIZADO"]);							
			txtValorTotal.Text = ValorTotal.ToString("#,###,##0"); 

			DBFunctions.Request(dsNits, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nitReceptor+"';");
			if(dsNits.Tables[0].Rows.Count>0)
			{
				txtNombreReceptor.Text=dsNits.Tables[0].Rows[0]["MNIT_NOMBRES"].ToString();
				txtApellidoReceptor.Text=dsNits.Tables[0].Rows[0]["MNIT_APELLIDOS"].ToString();
			}
			try
			{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Fecha no válida.");
				return;
			}
			//Verifica cierre mensual y diario
			int anio = fecha.Year;
			int mes  = fecha.Month;
			int periodo = anio * 100 + mes;
			
			string estado=DBFunctions.SingleData("select estado_cierre from DBXSCHEMA.periodos_cierre_transporte where numero_periodo="+periodo+";");
			if(estado.Length==0 || estado=="C")
			{
				BtnModificar.Enabled=false;
				BtnBorrar.Enabled=false;
				Ocultar_campos();
			}
			
			// Si existe es porque ya se contabilizo para la Agencia_dia
			if (DBFunctions.RecordExist("select MAG_CODIGO from DBXSCHEMA.DCIERRE_DIARIO_AGENCIA where MAG_CODIGO =  "+lblAgencia.Text+" and FECHA_CONTABILIZACION = '"+fecha.ToString("yyyy-MM-dd")+"';"))
			{
				BtnModificar.Enabled=false;
				BtnBorrar.Enabled=false;
				Ocultar_campos();
				
			}
			if(Request.QueryString["Comando"]== "Actualizar")
			{
				BtnBorrar.Enabled=false;
			}
			if(Request.QueryString["Comando"]== "Borrar")
			{
				BtnModificar.Enabled=false;
				Ocultar_campos();
			}
			if(Request.QueryString["Comando"]== "Imprimir")
			{
				BtnModificar.Enabled=false;
				BtnBorrar.Enabled=false;
				Ocultar_campos();
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
			this.BtnModificar.Click += new System.EventHandler(this.btnModificar_Click);
			this.BtnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
			this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
			this.BtnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		private void btnModificar_Click(object sender, System.EventArgs e)
		{
			DataSet dsNits=new DataSet();
				
			//Concepto
			if(ddlConcepto.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar el concepto.");
				return;
			}
			//Verificar placa
			if(txtPlaca.Text.Trim().Length > 0)
			{
				if (!DBFunctions.RecordExist("select mcat_placa from dbxschema.mbusafiliado where mcat_placa='" + txtPlaca.Text + "' union select palm_almacen from dbxschema.palmacen where palm_almacen='" + txtPlaca.Text + "'  ;"))
				{
                    Utils.MostrarAlerta(Response, "La placa registrada no existe.");
					return;
				}
			}
			//Validaciones
			
			if(txtNITReceptor.Text.Trim().Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe dar los datos del receptor.");
				return;
			}
			DBFunctions.Request(dsNits, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+txtNITReceptor.Text+"';");
			if(dsNits.Tables[0].Rows.Count>0)
			{
				txtNombreReceptor.Text=dsNits.Tables[0].Rows[0]["MNIT_NOMBRES"].ToString();
				txtApellidoReceptor.Text=dsNits.Tables[0].Rows[0]["MNIT_APELLIDOS"].ToString();
			}
			if(txtDescripcion.Text.Trim().Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe dar la descripción.");
				return;
			}
			try
			{
				if(double.Parse(txtCantidad.Text)<0)throw(new Exception());}
			catch
			{
                Utils.MostrarAlerta(Response, "La cantidad no es válida.");
				return;}
			try
			{
				if(double.Parse(txtValorUnidad.Text)<0)throw(new Exception());}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor de la unidad no es válido.");
				return;}
			try
			{
				if(double.Parse(txtValorTotal.Text)<0)throw(new Exception());}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor total no es válido.");
				return;}

			try
			{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Fecha no válida.");
				return;}
			//Verifica cierre mensual y diario
			int anio = fecha.Year;
			int mes  = fecha.Month;
			int periodo = anio * 100 + mes;
			
			string estado=DBFunctions.SingleData("select estado_cierre from DBXSCHEMA.periodos_cierre_transporte where numero_periodo="+periodo+";");
			if(estado.Length==0 || estado=="C")
			{
                Utils.MostrarAlerta(Response, " El periodo de la fecha esta cerrado o NO Existe.");
				return;
			}
			// Si existe es porque ya se contabilizo para la Agencia_dia
			if (DBFunctions.RecordExist("select MAG_CODIGO from DBXSCHEMA.DCIERRE_DIARIO_AGENCIA where MAG_CODIGO =  "+lblAgencia.Text+" and FECHA_CONTABILIZACION = '"+fecha.ToString("yyyy-MM-dd")+"';"))
			{
                Utils.MostrarAlerta(Response, "  La Agencia-fecha ya se Contabilizo ");
				return;
			}
			
			
			//Actualizar--
			
			string nitReceptor=txtNITReceptor.Text.Trim();
						
			string docReferencia=txtNumDocReferencia.Text.Trim();

			docReferencia=(docReferencia.Length==0)?"NULL":"'"+docReferencia+"'";
			string placa=txtPlaca.Text.Trim();
			placa=(placa.Length==0)?"NULL":"'"+placa+"'";
			string tipoUnidad=DBFunctions.SingleData("SELECT tund_consumo FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue);
			ArrayList sqlStrings = new ArrayList();
			sqlStrings.Add("INSERT INTO DBXSCHEMA.DACTUALIZAGASTOS_TRANSPORTES (select gt.*,'M',CURRENT  TIMESTAMP, '"+HttpContext.Current.User.Identity.Name.ToLower()+"' FROM DBXSCHEMA.MGASTOS_TRANSPORTES gt  WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO = "+TextDocumento.Text+");");
			sqlStrings.Add("UPDATE DBXSCHEMA.MGASTOS_TRANSPORTES SET FECHA_DOCUMENTO = '"+fecha.ToString("yyyy-MM-dd")+"',MCAT_PLACA = "+placa+",TCON_CODIGO = "+ddlConcepto.SelectedValue+",MNIT_RESPONSABLE_RECIBE ='"+nitReceptor+"',DESCRIPCION = '"+txtDescripcion.Text+"',CANTIDAD_CONSUMO = "+txtCantidad.Text.Replace(",","")+",VALOR_UNIDAD = "+txtValorUnidad.Text.Replace(",","")+",VALOR_TOTAL_AUTORIZADO = "+txtValorTotal.Text.Replace(",","")+",NUM_DOC_REFERENCIA = "+docReferencia+",MPLA_CODIGO = " + TextPlanilla.Text + " WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO = "+TextDocumento.Text+";");

			//Actualizar planillar ingresos-egresos
			if(!TextPlanilla.Text.Equals(""))
				sqlStrings.Add("CALL DBXSCHEMA.ACTUALIZA_PLANILLA("+TextPlanilla.Text+", '"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			
			if(!DBFunctions.Transaction(sqlStrings))
				lblError.Text += "Error: " + DBFunctions.exceptions;
			else
				lblError.Text += "Modificacion Satisfactoria";
		}
		private void btnBorrar_Click(object sender, System.EventArgs e)
		{	String nitAnulacion=DBFunctions.SingleData("select mnit_nit from dbxschema.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			ArrayList sqlStrings = new ArrayList();
			
			sqlStrings.Add("INSERT INTO DBXSCHEMA.DACTUALIZAGASTOS_TRANSPORTES (select gt.*,'B',CURRENT  TIMESTAMP, '"+HttpContext.Current.User.Identity.Name.ToLower()+"' FROM DBXSCHEMA.MGASTOS_TRANSPORTES gt  WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO = "+TextDocumento.Text+");");
			sqlStrings.Add("DELETE FROM DBXSCHEMA.MGASTOS_TRANSPORTES  WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO = "+TextDocumento.Text+";");
	
			//  Si es papeleria manual tiene  6 digitos y se debe actualizar, si es por sistema se borra el registro
			if (TextDocumento.Text.Length < 7)
			sqlStrings.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET  MPLA_CODIGO=NULL, FECHA_USO=NULL  WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO = "+TextDocumento.Text+";");
			else
				sqlStrings.Add("DELETE FROM DBXSCHEMA.MCONTROL_PAPELERIA  WHERE TDOC_CODIGO = 'ANT' AND NUM_DOCUMENTO = " + TextDocumento.Text + ";");
			//Actualizar planilla ingresos-egresos
			if(!TextPlanilla.Text.Equals(""))
				sqlStrings.Add("CALL DBXSCHEMA.ACTUALIZA_PLANILLA("+TextPlanilla.Text+", '"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			
			if(!DBFunctions.Transaction(sqlStrings))
				lblError.Text += "Error: " + DBFunctions.exceptions;
			else
				lblError.Text += "Eliminacion Satisfactoria";
			BtnBorrar.Enabled=false;
		}
		protected void btnRegresar_Click(Object sender, EventArgs e)
		{   
			if(Convert.ToInt32(Request.QueryString["auditor"])==0){
			//Response.Redirect(indexPage+ "?process=Comercial.IngresosGastos" + "&path="+Request.QueryString["path"]+ "&comando=" +e.CommandName + "&Documento=" +documento + "&sql="+ViewState["QUERY"] + "&pagina="+ViewState["PAGINA"]);
			Response.Redirect(indexPage+"?process=Comercial.ActualizarAnticipos" + "&Regresar=1" +"&sql=" +Request.QueryString["sql"] + "&pagina=" +Request.QueryString["pagina"] + "&path="+Request.QueryString["path"]);
			}
			else
			Response.Redirect(indexPage+"?process=Comercial.ActualizarAnticiposAuditor" + "&Regresar=1" +"&sql=" +Request.QueryString["sql"] + "&pagina=" +Request.QueryString["pagina"] + "&path="+Request.QueryString["path"]);
		}
		private void btnImprimir_Click(object sender, System.EventArgs e)
		{	
			generar_reporte(Request.QueryString["Documento"]);
		}
		public void generar_reporte(string Documento)
		{
			long Anticipo = Convert.ToInt64(Documento);
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
				
				string NombreReporte = ConfigurationManager.AppSettings["PathToReportsSource"] + "AMS.Comercial.ReporteDocumentoAnticipo.rpt";
				oRpt.Load(NombreReporte);

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
					ParameterDiscreteValue paramAnticipo=new ParameterDiscreteValue();
					ParameterValues currentAnticipo=new ParameterValues();
					paramAnticipo.Value=Anticipo;
					
					currentAnticipo.Add(paramAnticipo);
					
					oRpt.DataDefinition.ParameterFields["Documento"].ApplyCurrentValues(currentAnticipo);
					
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
				diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "AMS.Comercial.ReporteDocumentoAnticipo"+Anticipo+".pdf";
				//diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + "ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				exportOpts.DestinationOptions = diskOpts;
				oRpt.Export();
				Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"AMS.Comercial.ReporteDocumentoAnticipo"+Anticipo+".pdf";
				//Ver.NavigateUrl=ConfigurationManager.AppSettings["PathToPreviews"]+"ReporteDiarioAgencia"+agencia+"_"+fecha.ToString("yyyy-MM-dd")+ ".xls";
				Ver.Visible = true;
			}
			catch (Exception ex)
			{
				lblError.Text+=ex.Message;
			}
		}

	
	
		private void Ocultar_campos()
		{	
			ddlConcepto.Enabled=false;
			txtNITReceptor.Enabled=false;
			txtNITReceptor.Enabled=false;
			txtValorTotal.Enabled=false;
			txtPlaca.Enabled=false;
			txtNumDocReferencia.Enabled=false;
			txtValorUnidad.Enabled=false;
			txtNombreReceptor.Enabled=false;
			txtApellidoReceptor.Enabled=false;
			txtFecha.Enabled=false;
			txtCantidad.Enabled=false;
			txtDescripcion.Enabled=false;
		}
	}
}
