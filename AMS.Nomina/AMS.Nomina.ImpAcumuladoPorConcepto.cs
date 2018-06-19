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
	public class ImpAcumuladoPorConcepto: System.Web.UI.UserControl
	{
		protected DropDownList DDLMES,DDLANO,DDLCONCEPTO;
		protected DataSet ds;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		
		
				
		public void Page_Load(object sender,EventArgs e)
		{
			
			if (!IsPostBack)
			{
			DatasToControls param = new DatasToControls();
			param.PutDatasIntoDropDownList(DDLMES,"SELECT * FROM dbxschema.TMES");
			param.PutDatasIntoDropDownList(DDLANO,"SELECT * FROM dbxschema.PANO");
			param.PutDatasIntoDropDownList(DDLCONCEPTO,"select  pcon_concepto, pcon_concepto concat '-' concat pcon_nombconc from dbxschema.pconceptonomina");
				
			}
			
			
		}
		
		protected void btnmostrar(object sender, EventArgs e)
		{
			/*	
			//empleado
			if (DDLEMPLEADO.SelectedValue=="1" && DDLCONCEPTO.SelectedValue=="0")
			{
				Response.Write("<script language:javascript>w=window.open('AMS.Nomina.Acumulado.MostrarReportesProvisiones.aspx?Concepto="+DDLCONCEPTO.SelectedValue+"&Empleado="+DDLEMPLEADO.SelectedValue+"&DDLEMPLEADOS="+DDLEMPLEADOS.SelectedValue+"');</script>");
			}
			//todo el archivo
			else if (DDLEMPLEADO.SelectedValue=="0" && DDLCONCEPTO.SelectedValue=="0")
			{
				Response.Write("<script language:javascript>w=window.open('AMS.Nomina.Acumulado.MostrarReportesProvisiones.aspx?Concepto="+DDLCONCEPTO.SelectedValue+"&Empleado="+DDLEMPLEADO.SelectedValue+"');</script>");
			}
			*/
			this.ArchivoWord(this.generarreporte());
            Utils.MostrarAlerta(Response, "SELECT PCON.PCON_NOMBCONC , SUM(DQUI.DQUI_APAGAR) , SUM(DQUI.DQUI_ADESCONTAR) , MQUI.MQUI_ANOQUIN ,EMP.MEMP_CODIEMPL FROM DBXSCHEMA.PCONCEPTONOMINA PCON, DBXSCHEMA.DQUINCENA DQUI, DBXSCHEMA.MQUINCENAS MQUI , DBXSCHEMA.MEMPLEADO EMP WHERE DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND DQUI.MQUI_CODIQUIN=MQUI.MQUI_CODIQUIN AND DQUI.MEMP_CODIEMPL=EMP.MEMP_CODIEMPL AND PCON.PCON_CONCEPTO=\\'" + DDLCONCEPTO.SelectedValue + "\\' AND MQUI.MQUI_ANOQUIN=" + DDLANO.SelectedValue + " AND MQUI.MQUI_MESQUIN=" + DDLMES.SelectedValue + " GROUP BY PCON.PCON_NOMBCONC,MQUI.MQUI_ANOQUIN,EMP.MEMP_CODIEMPL;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto,CEN.pcen_nombre from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP,dbxschema.pcentrocosto CEN where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto and CEN.pcen_codigo=EMP.pcen_codigo");
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
				//Response.Write("<script language:javascript>alert('SELECT PCON.PCON_NOMBCONC , SUM(DQUI.DQUI_APAGAR) , SUM(DQUI.DQUI_ADESCONTAR) , MQUI.MQUI_ANOQUIN ,EMP.MEMP_CODIEMPL FROM DBXSCHEMA.PCONCEPTONOMINA PCON, DBXSCHEMA.DQUINCENA DQUI, DBXSCHEMA.MQUINCENAS MQUI , DBXSCHEMA.MEMPLEADO EMP WHERE DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND DQUI.MQUI_CODIQUIN=MQUI.MQUI_CODIQUIN AND DQUI.MEMP_CODIEMPL=EMP.MEMP_CODIEMPL AND PCON.PCON_CONCEPTO='"+DDLCONCEPTO.SelectedValue+"' AND MQUI.MQUI_ANOQUIN="+DDLANO.SelectedValue+" AND MQUI.MQUI_MESQUIN="+DDLMES.SelectedValue+" GROUP BY PCON.PCON_NOMBCONC,MQUI.MQUI_ANOQUIN,EMP.MEMP_CODIEMPL;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto,CEN.pcen_nombre from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP,dbxschema.pcentrocosto CEN where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto and CEN.pcen_codigo=EMP.pcen_codigo');</script>");
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoPorConcepto.doc"));
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
			ds=new DataSet();
			//Reporte para el caso de un concepto especifico ,todos los empleados por año y mes.
			
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT PCON.PCON_NOMBCONC , SUM(DQUI.DQUI_APAGAR) , SUM(DQUI.DQUI_ADESCONTAR) , MQUI.MQUI_ANOQUIN ,EMP.MEMP_CODIEMPL FROM DBXSCHEMA.PCONCEPTONOMINA PCON, DBXSCHEMA.DQUINCENA DQUI, DBXSCHEMA.MQUINCENAS MQUI , DBXSCHEMA.MEMPLEADO EMP WHERE DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND DQUI.MQUI_CODIQUIN=MQUI.MQUI_CODIQUIN AND DQUI.MEMP_CODIEMPL=EMP.MEMP_CODIEMPL AND PCON.PCON_CONCEPTO='"+DDLCONCEPTO.SelectedValue+"' AND MQUI.MQUI_ANOQUIN="+DDLANO.SelectedValue+" AND MQUI.MQUI_MESQUIN="+DDLMES.SelectedValue+" GROUP BY PCON.PCON_NOMBCONC,MQUI.MQUI_ANOQUIN,EMP.MEMP_CODIEMPL;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoPorConcepto.xsd"));
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoPorConcepto");
				NumRep=1;
				//Response.Write("<script language:javascript>alert('SELECT PCON.PCON_NOMBCONC , SUM(DQUI.DQUI_APAGAR) , SUM(DQUI.DQUI_ADESCONTAR) , MQUI.MQUI_ANOQUIN ,EMP.MEMP_CODIEMPL FROM DBXSCHEMA.PCONCEPTONOMINA PCON, DBXSCHEMA.DQUINCENA DQUI, DBXSCHEMA.MQUINCENAS MQUI , DBXSCHEMA.MEMPLEADO EMP WHERE DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND DQUI.MQUI_CODIQUIN=MQUI.MQUI_CODIQUIN AND DQUI.MEMP_CODIEMPL=EMP.MEMP_CODIEMPL AND PCON.PCON_CONCEPTO='"+DDLCONCEPTO.SelectedValue+"' AND MQUI.MQUI_ANOQUIN="+DDLANO.SelectedValue+" AND MQUI.MQUI_MESQUIN="+DDLMES.SelectedValue+" GROUP BY PCON.PCON_NOMBCONC,MQUI.MQUI_ANOQUIN,EMP.MEMP_CODIEMPL;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto,CEN.pcen_nombre from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP,dbxschema.pcentrocosto CEN where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto and CEN.pcen_codigo=EMP.pcen_codigo');</script>");
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
