using System;
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
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial  class ConciliacionBancaria : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlCC,"SELECT pcue_codigo,pcue_codigo CONCAT ' - ' CONCAT pban_nombre CONCAT ' - ' CONCAT pcue_numero  FROM dbxschema.pcuentacorriente pc, dbxschema.pbanco pb where pc.pban_banco = pb.pban_codigo ORDER BY PCUE_CODIGO");
				bind.PutDatasIntoDropDownList(ddlMes,"SELECT pmes_mes,pmes_nombre FROM dbxschema.pmes  WHERE pmes_mes BETWEEN 1 AND 12 ORDER BY pmes_mes");
				DatasToControls.EstablecerDefectoDropDownList(ddlMes,DBFunctions.SingleData("SELECT pmes_nombre FROM pmes WHERE pmes_mes="+DateTime.Now.Month+""));
			}
			else
			{
				
			}
		}
		
		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			ddlCC.Enabled=false;
			ddlMes.Enabled=false;
			btnAceptar.Enabled=false;
			pnlInfo.Visible=true;
		}
		
		protected void btnAceptarDoc_Click(object Sender,EventArgs e)
		{
			string[] archivo=uploadFile.PostedFile.FileName.Split('\\');
			if((archivo[archivo.Length-1].Split('.'))[1].Trim().ToUpper()=="XLS")
			{
				uploadFile.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"]+archivo[archivo.Length-1]);
				ExcelFunctions exc=new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"]+archivo[archivo.Length-1]);
				DataSet ds=new DataSet();
				ds=exc.Request(ds,IncludeSchema.NO,"SELECT * FROM CONCILIACION");
				if(ds.Tables[0].Rows.Count!=0)
				{
					if(ds.Tables[0].Columns.Count==3)
					{
						Session["ds3"]=ds;
						Response.Redirect(indexPage+"?process=Tesoreria.ProcesoConciliacion&cnt="+this.ddlCC.SelectedValue+"&col=3&mes="+this.ddlMes.SelectedValue+"");
					}
					else if(ds.Tables[0].Columns.Count==4)
					{
						Session["ds4"]=ds;
						Response.Redirect(indexPage+"?process=Tesoreria.ProcesoConciliacion&cnt="+this.ddlCC.SelectedValue+"&col=4&mes="+this.ddlMes.SelectedValue+"");
					}
					else
                    Utils.MostrarAlerta(Response, "El archivo no tiene el número de columnas esperado. Reviselo");
				}
				else
                Utils.MostrarAlerta(Response, "No hay movimiento o el archivo esta mal creado. Revise el formato del archivo");
			}
			else
            Utils.MostrarAlerta(Response, "Archivo Invalido");
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
		}
		#endregion
	}
}
