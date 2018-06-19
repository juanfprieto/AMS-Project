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

namespace AMS.Nomina
{
	public class ImpAcumuladoDoceMeses : System.Web.UI.UserControl
	{
		protected DropDownList DDLMES,DDLANO,DDLEMPLEADO,DDLEMPLEADOS,DDLCONCEPTO;
		protected DataSet ds;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		
		
		protected void Page_Load (object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(DDLANO,"SELECT * FROM dbxschema.PANO");
				param.PutDatasIntoDropDownList(DDLEMPLEADOS,"SELECT M.MEMP_CODIEMPL, M.MEMP_CODIEMPL CONCAT ' ' CONCAT N.MNIT_APELLIDOS CONCAT ' ' CONCAT N.MNIT_NOMBRES FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado='1' ");
			}
		}
		
		protected void btnmostrar (object sender ,EventArgs e)
		{
			if (DDLEMPLEADO.SelectedValue=="1")
			{
				Response.Write("<script language:javascript>w=window.open('AMS.Nomina.MostrarReportesNomina12meses.aspx?Concepto="+DDLCONCEPTO.SelectedValue+"&Empleado="+DDLEMPLEADO.SelectedValue+"&DDLANO="+DDLANO.SelectedValue+"&DDLEMPLEADOS="+DDLEMPLEADOS.SelectedValue+"');</script>");
			}
			else if (DDLEMPLEADO.SelectedValue=="0")
				Response.Write("<script language:javascript>w=window.open('AMS.Nomina.MostrarReportesNomina12meses.aspx?Concepto="+DDLCONCEPTO.SelectedValue+"&Empleado="+DDLEMPLEADO.SelectedValue+"&DDLANO="+DDLANO.SelectedValue+"');</script>");
	//	ProcessStartInfo ps = new ProcessStartInfo("cmd");
	//	ps.UseShellExecute = false;
	//	ps.RedirectStandardOutput = true;
	//	//Process p=new Process();
	//	Process p = Process.Start(ps);
	//	p.WaitForExit();
	//	string output = p.StandardOutput.ReadToEnd();
		//Process.Start("IExplore.exe");
		//System.Diagnostics.Process proc = new System.Diagnostics.Process();
	//proc.EnableRaisingEvents=false;
	//proc.StartInfo.FileName="iexplore";
	//proc.StartInfo.Arguments="http://www.microsoft.com";
	//proc.Start();
	//proc.WaitForExit();


			
		}
		
	
		
		protected void ddlempleado(object sender , EventArgs e )
		{
			if (Convert.ToInt32(DDLEMPLEADO.SelectedValue.ToString())==1)
			DDLEMPLEADOS.Visible=true;
			else
			DDLEMPLEADOS.Visible=false;	
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
