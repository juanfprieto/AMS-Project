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

namespace AMS.Nomina
{

	public class ImpAcumuladoConceptoGeneral : System.Web.UI.UserControl
	{
		protected DropDownList DDLANO;
		protected Button BTNMOSTRAR;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if (!IsPostBack)
			{
				Session.Clear();
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO,PANO_DETALLE FROM PANO");
				
			}
		}
		protected void btnmostrar(object sender, EventArgs e)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DQUI.pcon_concepto,MQUI.mqui_mesquin,SUM(DQUI.dqui_apagar),SUM(DQUI.dqui_adescontar),MQUI.mqui_anoquin FROM dbxschema.mempleado MEMP,dbxschema.dquincena DQUI,dbxschema.mquincenas MQUI,dbxschema.pconceptonomina PCON,dbxschema.pmes PMES WHERE MEMP.memp_codiempl=DQUI.memp_codiempl AND DQUI.pcon_concepto=PCON.pcon_concepto AND MQUI.mqui_codiquin=DQUI.mqui_codiquin AND MQUI.mqui_mesquin=PMES.pmes_mes and mqui_anoquin="+DDLANO.SelectedValue+" GROUP BY DQUI.pcon_concepto,MQUI.mqui_mesquin,MQUI.mqui_anoquin;select pmes_mes, pmes_nombre from dbxschema.pmes;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
			ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoConceptoGeneral.xsd"));
						
			reporte=new ReportDocument();
			reporte.Load(Path.Combine(Request.PhysicalApplicationPath,"rpt/Nomina.Acumulado.rpte_ImpAcumuladoConceptoGeneral.rpt"));
			reporte.SetDataSource(ds);
			visor.ReportSource=reporte;
			visor.DataBind();
			reporte.ExportToDisk(ExportFormatType.WordForWindows,Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoConceptoGeneral.doc"));
			
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "application/msword";
			Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoConceptoGeneral.doc"));
				
		}
			
			
		
		
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
	
	
	
	
	
