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

	public class ImpAcumuladoPorMes: System.Web.UI.UserControl
	{
		protected DropDownList DDLMES,DDLEMPLEADOS,DDLEMPLEADO;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
		protected DataSet ds;
		
		
		
		
		
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(DDLMES,"SELECT * FROM dbxschema.TMES");
				param.PutDatasIntoDropDownList(DDLEMPLEADOS,"SELECT M.MEMP_CODIEMPL, M.MEMP_CODIEMPL CONCAT ' ' CONCAT N.MNIT_APELLIDOS CONCAT ' ' CONCAT N.MNIT_NOMBRES FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado='1' ");
			}
			
		}
		
			protected void cambioaempleados (object sender , EventArgs e )
		{
			if (Convert.ToInt32(DDLEMPLEADO.SelectedValue.ToString())==1)
			DDLEMPLEADOS.Visible=true;
			else
			DDLEMPLEADOS.Visible=false;	
		}
		
		
		
		
		protected void btnmostrar(object sender, EventArgs e)
		{
			//empleado
			if (DDLEMPLEADO.SelectedValue=="1")
			{
				Response.Write("<script language:javascript>w=window.open('AMS.Nomina.Acumulado.MostrarReportesPorMes.aspx?Mes="+DDLMES.SelectedValue+"&Empleado="+DDLEMPLEADO.SelectedValue+"&DDLEMPLEADOS="+DDLEMPLEADOS.SelectedValue+"');</script>");
			}
			//todo el archivo
			else if (DDLEMPLEADO.SelectedValue=="0")
			{
				Response.Write("<script language:javascript>w=window.open('AMS.Nomina.Acumulado.MostrarReportesPorMes.aspx?Mes="+DDLMES.SelectedValue+"&Empleado="+DDLEMPLEADO.SelectedValue+"');</script>");
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
