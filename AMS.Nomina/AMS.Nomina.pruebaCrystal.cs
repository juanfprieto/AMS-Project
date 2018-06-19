using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.UI;


namespace AMS.Nomina
{
	public class pruebaCrystal : System.Web.UI.UserControl
	{
		protected DataSet ds,ds2;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		protected DiskFileDestinationOptions crDiskFileDestinationOptions;
		protected ExportOptions crExportOptions;
		protected Button BTNMOSTRAR;
		protected DropDownList DDLQUINCENA;
		
		protected void generarreporte ()
		{
			ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"select distinct D.mqui_codiquin,D.memp_codiempl,D.pcon_concepto,P.pcon_nombconc,D.dqui_valevento,D.dqui_canteventos,D.dqui_apagar,D.dqui_adescontar,D.dqui_saldo,M.mnit_nombres,M.mnit_apellidos,M.mnit_nit,E.memp_suelactu,mqui_anoquin ,mqui_mesquin , mqui_tpernomi  from dbxschema.dquincena D,dbxschema.mnit M,dbxschema.mempleado E, dbxschema.mquincenas MQUI , dbxschema.tmes T ,dbxschema.pconceptonomina P   where D.mqui_codiquin="+DDLQUINCENA.SelectedValue.ToString()+"   and E.mnit_nit=M.mnit_nit  and E.memp_codiempl=D.memp_codiempl and D.mqui_codiquin=MQUI.mqui_codiquin and P.pcon_concepto=D.pcon_concepto");
			ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Impresion.rpte_comppagogen.xsd"));
			
			//mostrar el reporte en la pagina.			
			reporte=new ReportDocument();
			reporte.Load(Path.Combine(Request.PhysicalApplicationPath,"rpt/Nomina.Impresion.rpte_comppagogen.rpt"));
			reporte.SetDataSource(ds);
			visor.ReportSource=reporte;
			visor.DataBind();
			reporte.ExportToDisk(ExportFormatType.WordForWindows,Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Impresion.rpte_comppagogen.doc"));
												
		}
		
		
		protected void Page_Load(object sender , EventArgs e)
		{
			
			if (!IsPostBack)
			{
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(DDLQUINCENA,"select mqui_codiquin,cast (mqui_codiquin as char(4)) concat '-' concat cast (mqui_anoquin as char(4)) concat '-' concat CAST(mqui_mesquin AS char(4)) CONCAT '-' CONCAT CAST(mqui_tpernomi AS char(2)) from dbxschema.mquincenas where mqui_estado=2");
				
			}
			
						
		}
		
		protected void btnmostrar (object sender ,EventArgs e)
		{
			this.generarreporte();
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "application/msword";
			Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Impresion.rpte_comppagogen.doc"));
			Response.Flush();
			Response.Close();
				
			
		}
		
		
		
		//protected HtmlInputFile fDocument;
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	
				
		
	}
}
