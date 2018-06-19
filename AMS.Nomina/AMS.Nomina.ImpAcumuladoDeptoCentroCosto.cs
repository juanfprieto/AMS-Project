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

	public class ImpAcumuladoDeptoCentroCosto: System.Web.UI.UserControl
	
	{
		protected DropDownList DDLCONCEPTO,DDLDEPTO,DDLCENTROCOSTO;	
		protected DropDownList DDLCONCEPTOS,DDLDEPTOS,DDLCENTROCOSTOS,DDLANO,DDLQUINCENA;
		protected Label LBCONCEPTO,LBDEPTO,LBCENTROCOSTO;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected DataSet ds;
		
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
			
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentos.doc"));
				break;
				
				case 2:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDepto.doc"));
				break;
				
				
				case 3:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConc.doc"));
				break;
				
				
				case 4:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno.doc"));
				break;
				
				
				case 5:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno1Q.doc"));
				break;
				
				case 6:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno2Q.doc"));
				break;
				
				
				case 7:
				Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosConcEsp.doc"));
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
			//Reporte para el caso de todos los departamentos,todos los conceptos,todos los años,ambas quincenas.
			ano=DBFunctions.SingleData("select cnom_ano from dbxschema.CNOMINA");
			if (DDLDEPTO.SelectedValue=="0" && DDLCONCEPTO.SelectedValue=="0" && DDLANO.SelectedValue=="0" && DDLQUINCENA.SelectedValue=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl and MQUI.mqui_mesquin=TMES.tmes_mes and MQUI.mqui_anoquin=PANO.pano_ano and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentos.xsd"));
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentos");
				NumRep=1;
                Utils.MostrarAlerta(Response, "entro bien");
			
			}
			
						
			//Reporte para el caso departamento ESPECIFICO,todos los conceptos,todos los años,ambas quincenas.
			else if (DDLDEPTO.SelectedValue=="1" && DDLCONCEPTO.SelectedValue=="0" && DDLANO.SelectedValue=="0" && DDLQUINCENA.SelectedValue=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl and MQUI.mqui_mesquin=TMES.tmes_mes and MQUI.mqui_anoquin=PANO.pano_ano and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto  and PDEPTO.pdep_codidpto='"+DDLDEPTOS.SelectedValue+"' group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDepto.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDepto");
				NumRep=2;		
			}
			
			//Reporte para el caso departamento ESPECIFICO,concepto ESPECIFICO,todos los años,ambas quincenas.
			else if (DDLDEPTO.SelectedValue=="1" && DDLCONCEPTO.SelectedValue=="1" && DDLANO.SelectedValue=="0" && DDLQUINCENA.SelectedValue=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl and MQUI.mqui_mesquin=TMES.tmes_mes and MQUI.mqui_anoquin=PANO.pano_ano and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto  and PDEPTO.pdep_codidpto='"+DDLDEPTOS.SelectedValue+"' and PCON.pcon_concepto='"+DDLCONCEPTOS.SelectedValue+"' group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConc.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConc");
				NumRep=3;		
			}
			
			//Reporte para el caso todos los departamentos,concepto ESPECIFICO,todos los años,ambas quincenas.
			else if (DDLDEPTO.SelectedValue=="0" && DDLCONCEPTO.SelectedValue=="1" && DDLANO.SelectedValue=="0" && DDLQUINCENA.SelectedValue=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto  from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO  where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl  and MQUI.mqui_mesquin=TMES.tmes_mes   and MQUI.mqui_anoquin=PANO.pano_ano  and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto and PCON.pcon_concepto='"+DDLCONCEPTOS.SelectedValue+"' group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosConcEsp.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosConcEsp");
				NumRep=7;		
			}
			
			
			
			
			
			//Reporte para el caso departamento ESPECIFICO,concepto ESPECIFICO,año ESPECIFICO ,ambas quincenas.
			else if (DDLDEPTO.SelectedValue=="1" && DDLCONCEPTO.SelectedValue=="1" && DDLANO.SelectedValue!="0" && DDLQUINCENA.SelectedValue=="0" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl and MQUI.mqui_mesquin=TMES.tmes_mes and MQUI.mqui_anoquin=PANO.pano_ano and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto  and PDEPTO.pdep_codidpto='"+DDLDEPTOS.SelectedValue+"' and PCON.pcon_concepto='"+DDLCONCEPTOS.SelectedValue+"' and MQUI.mqui_anoquin="+DDLANO.SelectedValue+" group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno");
				NumRep=4;		
			}
			
			
			//Reporte para el caso departamento ESPECIFICO,concepto ESPECIFICO,año ESPECIFICO ,PRIMERA quincena.
			else if (DDLDEPTO.SelectedValue=="1" && DDLCONCEPTO.SelectedValue=="1" && DDLANO.SelectedValue!="0" && DDLQUINCENA.SelectedValue=="1" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl and MQUI.mqui_mesquin=TMES.tmes_mes and MQUI.mqui_anoquin=PANO.pano_ano and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto  and PDEPTO.pdep_codidpto='"+DDLDEPTOS.SelectedValue+"' and PCON.pcon_concepto='"+DDLCONCEPTOS.SelectedValue+"' and MQUI.mqui_anoquin="+DDLANO.SelectedValue+"  and MQUI.mqui_tpernomi=1  group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno1Q.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno1Q");
				NumRep=5;		
			}
			
			
			//Reporte para el caso departamento ESPECIFICO,concepto ESPECIFICO,año ESPECIFICO ,SEGUNDA quincena.
			else if (DDLDEPTO.SelectedValue=="1" && DDLCONCEPTO.SelectedValue=="1" && DDLANO.SelectedValue!="0" && DDLQUINCENA.SelectedValue=="2" )
			{
				DBFunctions.Request(ds,IncludeSchema.NO,"select distinct PCON.pcon_concepto,PCON.pcon_nombconc,sum(DQUI.dqui_apagar),sum(DQUI.dqui_adescontar),PDEPTO.pdep_nombdpto from  dbxschema.pconceptonomina PCON, dbxschema.dquincena DQUI,dbxschema.pdepartamentoempresa PDEPTO, dbxschema.mquincenas MQUI, dbxschema.mempleado MEMP, dbxschema.tmes TMES,dbxschema.pano PANO where MQUI.mqui_codiquin=DQUI.mqui_codiquin and PCON.pcon_concepto=DQUI.pcon_concepto and DQUI.memp_codiempl=MEMP.memp_codiempl and MQUI.mqui_mesquin=TMES.tmes_mes and MQUI.mqui_anoquin=PANO.pano_ano and PDEPTO.pdep_codidpto=MEMP.pdep_codidpto  and PDEPTO.pdep_codidpto='"+DDLDEPTOS.SelectedValue+"' and PCON.pcon_concepto='"+DDLCONCEPTOS.SelectedValue+"' and MQUI.mqui_anoquin="+DDLANO.SelectedValue+"  and MQUI.mqui_tpernomi=2  group by PCON.pcon_concepto,PCON.pcon_nombconc,PDEPTO.pdep_nombdpto;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
				ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno2Q.xsd"));
				//mostrar el reporte en la pagina.
				this.MostrarReporte("Nomina.Acumulado.rpte_ImpAcumuladoDepartamentosPorDeptoConcAno2Q");
				NumRep=6;		
			}
			
			
		
			
			
			
			
			
			
			
			
			return NumRep;
				
		}
		
		
		
		
		
		
		
		
		
			protected void btnmostrar(object sender, EventArgs e)
		{
			this.ArchivoWord(this.generarreporte());
			//Response.Write("<script language:javascript>alert('SELECT PCON.PCON_NOMBCONC , SUM(DQUI.DQUI_APAGAR) , SUM(DQUI.DQUI_ADESCONTAR) , MQUI.MQUI_ANOQUIN ,EMP.MEMP_CODIEMPL FROM DBXSCHEMA.PCONCEPTONOMINA PCON, DBXSCHEMA.DQUINCENA DQUI, DBXSCHEMA.MQUINCENAS MQUI , DBXSCHEMA.MEMPLEADO EMP WHERE DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND DQUI.MQUI_CODIQUIN=MQUI.MQUI_CODIQUIN AND DQUI.MEMP_CODIEMPL=EMP.MEMP_CODIEMPL AND PCON.PCON_CONCEPTO=\\'"+DDLCONCEPTO.SelectedValue+"\\' AND MQUI.MQUI_ANOQUIN="+DDLANO.SelectedValue+" AND MQUI.MQUI_MESQUIN="+DDLMES.SelectedValue+" GROUP BY PCON.PCON_NOMBCONC,MQUI.MQUI_ANOQUIN,EMP.MEMP_CODIEMPL;select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;select EMP.memp_codiempl, M.mnit_nit,M.mnit_nombres, M.mnit_apellidos,DEP.pdep_nombdpto,CEN.pcen_nombre from dbxschema.mempleado EMP, dbxschema.mnit M,dbxschema.pdepartamentoempresa DEP,dbxschema.pcentrocosto CEN where EMP.mnit_nit=M.mnit_nit and DEP.pdep_codidpto=EMP.pdep_codidpto and CEN.pcen_codigo=EMP.pcen_codigo');</script>");	
			//Response.Write("<script language:javascript>alert('works');</script>");	
				
				
		}
			
			protected void cambioconcepto(object sender, EventArgs e)
			{
				if (Convert.ToInt32(DDLCONCEPTO.SelectedValue.ToString())==1)
				{
					DDLCONCEPTOS.Visible=true;
					LBCONCEPTO.Visible=true;
				}
				else
				{
					DDLCONCEPTOS.Visible=false;
					LBCONCEPTO.Visible=false;
				}
			}
		
			protected void cambiodepto(object sender, EventArgs e)
			{
				if (Convert.ToInt32(DDLDEPTO.SelectedValue.ToString())==1)
				{
					DDLDEPTOS.Visible=true;
					LBDEPTO.Visible=true;
				}
				else
				{
					DDLDEPTOS.Visible=false;
					LBDEPTO.Visible=false;
				
				}
					
				
			}
		
			protected void cambiocentrocosto(object sender, EventArgs e)
			{
					if (Convert.ToInt32(DDLCENTROCOSTO.SelectedValue.ToString())==1)
				{
					DDLCENTROCOSTOS.Visible=true;
					LBCENTROCOSTO.Visible=true;
				}
				else
				{
					DDLCENTROCOSTOS.Visible=false;
					LBCENTROCOSTO.Visible=false;
				}
				
				
								
			}
		
		
		public void Page_Load(object sender , EventArgs e)
		{
			if (!IsPostBack)
			{
			DatasToControls param = new DatasToControls();
			param.PutDatasIntoDropDownList(DDLCONCEPTOS,"SELECT PCON_CONCEPTO,PCON_NOMBCONC FROM dbxschema.PCONCEPTONOMINA");
			param.PutDatasIntoDropDownList(DDLDEPTOS,"SELECT * FROM dbxschema.PDEPARTAMENTOEMPRESA");
			param.PutDatasIntoDropDownList(DDLCENTROCOSTOS,"select PCEN_CODIGO,PCEN_NOMBRE from dbxschema.pcentrocosto");
			if(DDLANO.Items.Count==0)
			{
				param.PutDatasIntoDropDownList(DDLANO,"select * from pano");
				ListItem it=new ListItem("Todos los Años","0");
				DDLANO.Items.Insert(0,it);
			}
			}
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
