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
using AMS.Tools;

namespace AMS.Nomina
{
	public class MostrarReportesProvision : System.Web.UI.Page
	{
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		protected DataSet ds;
		
		
		public void Page_Load(object sender,EventArgs e)
		{
			this.ArchivoWord(this.generarreporte());
			
		}
		
		
		protected void MostrarReporte(string ruta)
		{
			//mostrar el reporte en la pagina.			
			reporte=new ReportDocument();
			reporte.Load(Path.Combine(Request.PhysicalApplicationPath,"rpt/"+ruta+".rpt"));
			reporte.SetDataSource(ds);
			visor.ReportSource=reporte;
			visor.DataBind();
			reporte.ExportToDisk(ExportFormatType.WordForWindows,Path.Combine(Request.PhysicalApplicationPath,"rptgen/"+ruta+".doc"));
			
		}
		
		protected void ArchivoWord(int n)
		{
			Response.ClearContent();
			Response.ClearHeaders();
			Response.ContentType = "application/msword";
			switch (n)
			{
					
			
				case 1:
			
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoProvisiones.doc"));
				break;
				
				case 2:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoProvisionesPorMesEmpleadoEspecifico.doc"));
				break;
				
				default:
                Utils.MostrarAlerta(Response, "Devolvio mal el numero ");
				break;
			}
			Response.Flush();
			Response.Close();
			
			
		}
		
		
		protected int generarreporte ()
		{
			int NumRep=0;
			string ano;
			ds=new DataSet();
			//Reporte para el caso de todos los conceptos de provision ,todos los empleados por MES del año vigente.
			ano=DBFunctions.SingleData("select cnom_ano from dbxschema.CNOMINA");
			if (Request.QueryString["Concepto"]=="0" && Request.QueryString["Empleado"]=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MEMP.memp_codiempl,PPRO.ppro_codiprov,SUM(MPRO.mpro_valor),PPRO.ppro_nombprov,MQUI.mqui_anoquin,MQUI.mqui_mesquin FROM dbxschema.mempleado MEMP, dbxschema.mprovisiones MPRO,dbxschema.pprovisionnomina PPRO,dbxschema.mquincenas MQUI WHERE MEMP.memp_codiempl=MPRO.memp_codiempl and PPRO.ppro_codiprov=MPRO.pcon_concepto and MQUI.mqui_codiquin=MPRO.mqui_codiquin and MQUI_ANOQUIN="+ano+"  GROUP BY MEMP.memp_codiempl,PPRO.ppro_codiprov,PPRO.ppro_nombprov,MQUI.mqui_mesquin,MQUI.mqui_anoquin;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoProvisiones.xsd"));
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoProvisiones");
				NumRep=1;
                Utils.MostrarAlerta(Response, "todos");
			
			}
			//Reporte para el caso de todos los conceptos,empleado especifico por MES especifico del año vigente.
			else if (Request.QueryString["Concepto"]=="0" && Request.QueryString["Empleado"]=="1" )
			{
				//Response.Write("<script language:java>alert('Borrando datos.. ');</script> ");
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MEMP.memp_codiempl,PPRO.ppro_codiprov,SUM(MPRO.mpro_valor),PPRO.ppro_nombprov,MQUI.mqui_anoquin,MQUI.mqui_mesquin FROM dbxschema.mempleado MEMP, dbxschema.mprovisiones MPRO,dbxschema.pprovisionnomina PPRO,dbxschema.mquincenas MQUI WHERE MEMP.memp_codiempl=MPRO.memp_codiempl and PPRO.ppro_codiprov=MPRO.pcon_concepto and MQUI.mqui_codiquin=MPRO.mqui_codiquin and MQUI_ANOQUIN="+ano+" and MEMP.memp_codiempl='"+Request.QueryString["DDLEMPLEADOS"]+"' GROUP BY MEMP.memp_codiempl,PPRO.ppro_codiprov,PPRO.ppro_nombprov,MQUI.mqui_mesquin,MQUI.mqui_anoquin;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoProvisionesPorMesEmpleadoEspecifico.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoProvisionesPorMesEmpleadoEspecifico");
				NumRep=2;		
			}
			
			return NumRep;
				
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
