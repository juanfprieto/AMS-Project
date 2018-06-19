using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Reportes;


namespace AMS.Contabilidad
{
	
	
public partial class EstCen : System.Web.UI.UserControl
{
	
	protected PlaceHolder  preHeaderHolder ; 
	protected Label  lbEmpresa, lbFecha, lbTitulo, lbParams;
	protected DropDownList ddlView;
	protected DataGrid  dgTest;
	protected DataTable tbReport;
	protected DataRow dr, drAux;
	protected DataColumn dc;
	protected DataSet ds;
	protected HtmlGenericControl spReport;
	protected string tmpText, reportTitle="Estado de Resultados por Centros de Costo";
	protected string[] reportDatas, pr, values;
	protected int i, j;
	
	
	protected void Page_Load(object sender, System.EventArgs e)
	{
		
		Filters bind = new Filters(reportTitle, filterHolder);
		reportInfo.Text = bind.InfoProcess + "<br>";
		
		if(bind.FiltersNum > 0)
		{
			Button filterButton = new Button();
			filterButton.Text = "Generar Reporte";
			filterButton.Click += new EventHandler(this.MakeReport);
			filterHolder.Controls.Add(new LiteralControl("<br><br>"));
			filterHolder.Controls.Add(filterButton);
		}
		
		
	}
	
	
	
	protected void Report_ItemDataBound(object sender, DataGridItemEventArgs e)
 	{		
 		
 		if(e.Item.ItemType == ListItemType.Header)
 		{
			for(i=0; i<e.Item.Cells.Count; i++)
			{
				if(e.Item.Cells[i].Text.IndexOf("%") != -1)
				{
					e.Item.Cells[i].Text = "%";
				}
			}
			
 		}
 		
 	}
	
	
	
	
	public void MakeReport(Object Sender, EventArgs E)
	{

		DatasForReport balGen = new DatasForReport(reportTitle);
		ds = balGen.GetDatas(false, GetParams());
		//reportInfo.Text += balGen.InfoMsg;
		//reportInfo.Text += balGen.SQL[0] + "<br>";
		
		Press frontEnd = new Press(ds, reportTitle);
		frontEnd.PreHeader(tabPreHeader, report.Width, pr);
		frontEnd.SourceFieldTitles(balGen.PressFields);
		
		tbReport = frontEnd.TbReport;
		
		double saldo=0, total=0;
		string cVal="", aVal, name="";
		DataRow[] rows, rows2;
		for(i=0; i<ds.Tables[1].Rows.Count; i++)
		{	
			cVal = ds.Tables[1].Rows[i][1].ToString();
			if(i != ds.Tables[1].Rows.Count-1)
				aVal = ds.Tables[1].Rows[i+1][1].ToString();
			else
				aVal = "0000";
			
			
			if(aVal != cVal)
			{
				rows = ds.Tables[1].Select("MCUE_CODIPUC = '" + cVal + "'");
				rows2 = ds.Tables[0].Select("CUENTA = '" + cVal + "'");
				name = rows2[0][1].ToString();
				
				for(j=0; j<rows.Length; j++)
				{
					if(j == 0)
					{
						dr = tbReport.NewRow();
						tbReport.Rows.Add(dr);
						dr[0] = cVal;
						dr[1] = name;
					
					}
					if(rows2[0][2].ToString() == "D")
						saldo = Convert.ToDouble(rows[j][2]) - Convert.ToDouble(rows[j][3]);
					if(rows2[0][2].ToString() == "C")
						saldo = Convert.ToDouble(rows[j][3]) - Convert.ToDouble(rows[j][2]);
					dr[Convert.ToInt32(rows[j][0])*2] = saldo;
					
					total += saldo;
				}
				dr[26] = total;
				total = 0;
			}
			if(cVal.Substring(0,1) != aVal.Substring(0,1))
			{
				
				dr = tbReport.NewRow();
				tbReport.Rows.Add(dr);
				dr[0] = "TOTAL";
				if(cVal.Substring(0,1) == "4")
					dr[1] = "INGRESOS";	
				if(cVal.Substring(0,1) == "5")
					dr[1] = "GASTOS";
				if(cVal.Substring(0,1) == "6")
					dr[1] = "COSTOS";
				if(cVal.Substring(0,1) == "7")
					dr[1] = "GASTOS FINANCIEROS";
			}
			
		}
		
		dr = tbReport.NewRow();
		tbReport.Rows.Add(dr);
		dr[0] = "TOTAL";
		dr[1] = "ADMINISTRACION";
		
		SetSomeValues();
		
		frontEnd.PressFieldTitles(report);
		
		/*if(ddlView.SelectedValue != "ND")
		for(i=0; i<ds.Tables[2].Rows.Count; i++)
			for(j=2; j<ds.Tables[2].Columns.Count; j++)
				if(ds.Tables[2].Rows[i][j].ToString() != "")
					ds.Tables[2].Rows[i][j] = Convert.ToDouble(ds.Tables[2].Rows[i][j])/Convert.ToInt32(ddlView.SelectedValue);
		*/
		report.DataSource = ds.Tables[2];
		report.DataBind();
		
		toolsHolder.Visible = true;
	}
		
	
	
	
	protected void SetSomeValues()
	{
		int dim = 27;
		double[] total=new double[dim], totalIngresos=new double[dim], totalGastos=new double[dim];
		double[] totalCostos=new double[dim], totalFinancieros=new double[dim];
		
		for(i=0; i<ds.Tables[2].Rows.Count; i++)
		{
			
			for(j=2; j<ds.Tables[2].Columns.Count; j++)
			{
				if(ds.Tables[2].Rows[i][j].ToString() != "")
					total[j] += Convert.ToDouble(ds.Tables[2].Rows[i][j]);
				else
					total[j] += 0;
			}
			
			//reportInfo.Text += cVal.Substring(0,1) + ":::" + nVal.Substring(0,1) + "<br>";
			if(ds.Tables[2].Rows[i][0].ToString() == "TOTAL" && ds.Tables[2].Rows[i][1].ToString() != "ADMINISTRACION")
			{
				
				for(j=2; j<ds.Tables[2].Columns.Count/2+1; j++)
				{
					ds.Tables[2].Rows[i][2*j] = total[2*j];
					if(ds.Tables[2].Rows[i][1].ToString() == "INGRESOS")
						totalIngresos[2*j] = total[2*j];
					if(ds.Tables[2].Rows[i][1].ToString() == "GASTOS")
						totalGastos[2*j] = total[2*j];
					if(ds.Tables[2].Rows[i][1].ToString() == "COSTOS")
						totalCostos[2*j] = total[2*j];
					if(ds.Tables[2].Rows[i][1].ToString() == "GASTOS FINANCIEROS")
						totalFinancieros[2*j] = total[2*j];
				
					total[2*j] = 0;
				}
			}
			if(ds.Tables[2].Rows[i][1].ToString() == "ADMINISTRACION")
				for(j=2; j<ds.Tables[2].Columns.Count/2+1; j++)
					ds.Tables[2].Rows[i][2*j] = totalIngresos[2*j]-totalGastos[2*j]-totalCostos[2*j]-totalFinancieros[2*j];
				
			for(j=1; j<ds.Tables[2].Columns.Count/2; j++)
				if(ds.Tables[2].Rows[i][2*j].ToString() != "" && ds.Tables[2].Rows[i][26].ToString() != "")
					ds.Tables[2].Rows[i][2*j+1]	= (Convert.ToDouble(ds.Tables[2].Rows[i][2*j])*100)/Convert.ToDouble(ds.Tables[2].Rows[i][26]);
				
			
		}
	}
	
	
	
	
	protected string[] GetParams()
	{
		values = new string[filterHolder.Controls.Count];
		string[] valuesTxt = new string[filterHolder.Controls.Count];
		string[] theParams;
		int i,k;
		
		for(i=0,k=0; i<filterHolder.Controls.Count; i++)
		{
			if(filterHolder.Controls[i].ToString() == "System.Web.UI.WebControls.DropDownList")
			{
				DropDownList dd = (DropDownList)filterHolder.Controls[i];
				values[k] = dd.SelectedValue;
				valuesTxt[k] = dd.SelectedItem.Text;
				k++;
			}
			if(filterHolder.Controls[i].ToString() == "System.Web.UI.WebControls.TextBox")
			{
				TextBox dd = (TextBox)filterHolder.Controls[i];
				values[k] = dd.Text;
				valuesTxt[k] = dd.Text;
				k++;
			}
				
		}
		
		
		for(i=0,k=0; i<values.Length; i++)
		{
			if(values[i] != null)
			{
				k++;
			}
		}
		
		theParams = new String[k];
		
		for(i=0,k=0; i<values.Length; i++)
		{
			if(values[i] != null)
			{
				theParams[k] = values[i];
				k++;
			}
		}
		pr = valuesTxt;
		return theParams;
	}
	
	
	
	
	public void SendMail(Object Sender, ImageClickEventArgs E)
	{
		
     	MakeReport(Sender, E);
     	reportInfo.Text = Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, report);
     	
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
