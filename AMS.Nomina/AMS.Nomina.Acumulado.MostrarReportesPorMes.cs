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
using System.Diagnostics;
using AMS.Tools;

namespace AMS.Nomina
{

	public class MostrarReportesPorMes: System.Web.UI.Page
	{
		
		protected DataSet ds;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		protected Label lb;
		
		protected void Page_Load (object sender, EventArgs e)
		{
			lb.Text+=Request.QueryString["DDLEMPLEADOS"];
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
			
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoPorMes.doc"));
				break;
				
				case 2:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoPorMesEmpleadoEspecifico.doc"));
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
			//Reporte para el caso de todos los conceptos,todos los empleados por MES especifico del año vigente.
			ano=DBFunctions.SingleData("select cnom_ano from dbxschema.CNOMINA");
			if (Request.QueryString["Empleado"]=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MEMP.memp_codiempl,DQUI.pcon_concepto,SUM(DQUI.dqui_apagar),SUM(DQUI.dqui_adescontar),MQUI.mqui_anoquin,PCON.pcon_signoliq  FROM dbxschema.mempleado MEMP,dbxschema.dquincena DQUI,dbxschema.mquincenas MQUI,dbxschema.pconceptonomina PCON,dbxschema.pmes PMES WHERE MEMP.memp_codiempl=DQUI.memp_codiempl AND DQUI.pcon_concepto=PCON.pcon_concepto AND MQUI.mqui_codiquin=DQUI.mqui_codiquin AND MQUI.mqui_mesquin=PMES.pmes_mes and mqui_anoquin="+ano+" and mqui_mesquin<="+Request.QueryString["Mes"]+" GROUP BY MEMP.memp_codiempl,DQUI.pcon_concepto,MQUI.mqui_anoquin,PCON.pcon_signoliq ORDER BY MEMP.memp_codiempl;select pcon_concepto,pcon_nombconc from dbxschema.pconceptonomina;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto; select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;SELECT MEMP.memp_codiempl,DQUI.pcon_concepto,SUM(DQUI.dqui_apagar),SUM(DQUI.dqui_adescontar),MQUI.mqui_anoquin FROM dbxschema.mempleado MEMP,dbxschema.dquincena DQUI,dbxschema.mquincenas MQUI,dbxschema.pconceptonomina PCON,dbxschema.pmes PMES  WHERE MEMP.memp_codiempl=DQUI.memp_codiempl  AND DQUI.pcon_concepto=PCON.pcon_concepto AND MQUI.mqui_codiquin=DQUI.mqui_codiquin AND MQUI.mqui_mesquin=PMES.pmes_mes and mqui_anoquin="+ano+" and mqui_mesquin="+Request.QueryString["Mes"]+" GROUP BY MEMP.memp_codiempl,DQUI.pcon_concepto,MQUI.mqui_anoquin ORDER BY MEMP.memp_codiempl;select cnom_ano from dbxschema.CNOMINA");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoPorMes.xsd"));
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoPorMes");
				NumRep=1;
                Utils.MostrarAlerta(Response, "todos ");
			
			}
			//Reporte para el caso de todos los conceptos,empleado especifico por MES especifico del año vigente.
			else if (Request.QueryString["Empleado"]=="1" )
			{
				//Response.Write("<script language:java>alert('Borrando datos.. ');</script> ");
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MEMP.memp_codiempl,DQUI.pcon_concepto,SUM(DQUI.dqui_apagar),SUM(DQUI.dqui_adescontar),MQUI.mqui_anoquin,PCON.pcon_signoliq  FROM dbxschema.mempleado MEMP,dbxschema.dquincena DQUI,dbxschema.mquincenas MQUI,dbxschema.pconceptonomina PCON,dbxschema.pmes PMES WHERE MEMP.memp_codiempl=DQUI.memp_codiempl AND DQUI.pcon_concepto=PCON.pcon_concepto AND MQUI.mqui_codiquin=DQUI.mqui_codiquin AND MQUI.mqui_mesquin=PMES.pmes_mes and MEMP.memp_codiempl='"+Request.QueryString["DDLEMPLEADOS"]+"' and mqui_anoquin="+ano+" and mqui_mesquin<="+Request.QueryString["Mes"]+" GROUP BY MEMP.memp_codiempl,DQUI.pcon_concepto,MQUI.mqui_anoquin,PCON.pcon_signoliq ORDER BY MEMP.memp_codiempl;select pcon_concepto,pcon_nombconc from dbxschema.pconceptonomina;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto; select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;SELECT MEMP.memp_codiempl,DQUI.pcon_concepto,SUM(DQUI.dqui_apagar),SUM(DQUI.dqui_adescontar),MQUI.mqui_anoquin FROM dbxschema.mempleado MEMP,dbxschema.dquincena DQUI,dbxschema.mquincenas MQUI,dbxschema.pconceptonomina PCON,dbxschema.pmes PMES  WHERE MEMP.memp_codiempl=DQUI.memp_codiempl  AND DQUI.pcon_concepto=PCON.pcon_concepto AND MQUI.mqui_codiquin=DQUI.mqui_codiquin AND MQUI.mqui_mesquin=PMES.pmes_mes  and MEMP.memp_codiempl='"+Request.QueryString["DDLEMPLEADOS"]+"' and mqui_anoquin="+ano+" and mqui_mesquin="+Request.QueryString["Mes"]+" GROUP BY MEMP.memp_codiempl,DQUI.pcon_concepto,MQUI.mqui_anoquin ORDER BY MEMP.memp_codiempl ");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoPorMesEmpleadoEspecifico.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoPorMesEmpleadoEspecifico");
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
