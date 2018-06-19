using System;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Reportes
{
	public partial class Eliminar : System.Web.UI.UserControl
	{
		
		protected DataTable dtReports;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
				this.Llenar_dtReports();
			if(Session["dtReports"] == null)
				this.Llenar_dtReports();
			else
				dtReports = (DataTable)Session["dtReports"];
		}
		
		protected void Volver(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Reportes.AdminReports");
		}
		
		protected void DgTable_Delete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				Reporte miReporte = new Reporte(dtReports.Rows[e.Item.ItemIndex][0].ToString());
				if(miReporte.Eliminar_Reporte())
					dtReports.Rows.Remove(dtReports.Rows[e.Item.ItemIndex]);
				else
				{
                    Utils.MostrarAlerta(Response, "No se ha podido eliminar el reporte");
					lb.Text += miReporte.ProcessMsg;
				}
			}
			catch
			{
				dtReports.Rows.Clear();
                Utils.MostrarAlerta(Response, "Se ha presentado un error con la tabla");
			}
			this.Bind_dgReports();
		}
		
		
		protected void Preparar_dtReports()
		{
			dtReports = new DataTable();
			dtReports.Columns.Add(new DataColumn("IDREPORTE",System.Type.GetType("System.String")));//0
			dtReports.Columns.Add(new DataColumn("NOMBREREPORTE",System.Type.GetType("System.String")));//1
			dtReports.Columns.Add(new DataColumn("IDMENU",System.Type.GetType("System.String")));//2
		}
		
		protected void Llenar_dtReports()
		{
			this.Preparar_dtReports();
			DataSet dsReports = new DataSet();
			DBFunctions.Request(dsReports,IncludeSchema.NO,"SELECT SREP.srep_id, SREP.srep_nombre, SMEN.smen_codigo FROM sreporte SREP, smenureporte SMEN WHERE SREP.srep_id = SMEN.srep_id ORDER BY SREP_ID");
			for(int i=0;i<dsReports.Tables[0].Rows.Count;i++)
			{
				DataRow fila = dtReports.NewRow();
				fila[0] = dsReports.Tables[0].Rows[i][0].ToString();
				fila[1] = dsReports.Tables[0].Rows[i][1].ToString();
				fila[2] = dsReports.Tables[0].Rows[i][2].ToString();
				dtReports.Rows.Add(fila);
			}
			this.Bind_dgReports();
		}
		
		protected void Bind_dgReports()
		{
			Session["dtReports"] = dtReports;
			dgReports.DataSource = dtReports;
			dgReports.DataBind();
		}
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    

		}
		#endregion
	}
}
	
