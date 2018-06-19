using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Reportes;
using AMS.DB;

namespace AMS.Contabilidad
{
	
	
public partial class LibDia : System.Web.UI.UserControl
{
	
	protected PlaceHolder  preHeaderHolder ; 
	protected Label  lbEmpresa, lbFecha, lbTitulo, lbParams;
	protected DataGrid  dgTest;
	protected DataTable tbReport;
	protected DataRow dr, drAux;
	protected DataColumn dc;
	protected DataSet ds;
	protected HtmlGenericControl spReport;
	protected string tmpText, reportTitle="Libro Diario";
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
 		
 		
 		
 	}
	
	
	
	
	public void MakeReport(Object Sender, EventArgs E)
	{

		DatasForReport balGen = new DatasForReport(reportTitle);
		ds = balGen.GetDatas(false, GetParams());
		//reportInfo.Text += balGen.SQL[1] + "<br>";
		//dgTest.DataSource = ds.Tables[1];
		
		
		Press frontEnd = new Press(ds, reportTitle);
		frontEnd.PreHeader(tabPreHeader, report.Width, pr);
		frontEnd.Firmas(tabFirmas,report.Width);
		frontEnd.SourceFieldTitles(balGen.PressFields);
		
		tbReport = frontEnd.TbReport;
		
		double deb1=0, cre1=0, totalDeb=0, totalCre=0;
		string cVal, aVal;
		
		for(i=0; i<ds.Tables[0].Rows.Count; i++)
		{
			
			cVal = ds.Tables[0].Rows[i][2].ToString();
			deb1 += Convert.ToDouble(ds.Tables[0].Rows[i][4]);
			cre1 += Convert.ToDouble(ds.Tables[0].Rows[i][5]);
			if(i != ds.Tables[0].Rows.Count-1)
				aVal = ds.Tables[0].Rows[i+1][2].ToString();
			else
				aVal = "00000";
			
			if(cVal != aVal)
			{
				dr = tbReport.NewRow();
				tbReport.Rows.Add(dr);
				dr[0] = ds.Tables[0].Rows[i][0].ToString();
				dr[1] = ds.Tables[0].Rows[i][1].ToString();
				dr[2] = cVal;
				dr[3] = ds.Tables[0].Rows[i][3].ToString();
				dr[4] = deb1;
				dr[5] = cre1;
				deb1=0;
				cre1=0;
			}
		}
		
		dr = tbReport.NewRow();
		tbReport.Rows.Add(dr);
		
		dr = tbReport.NewRow();
		tbReport.Rows.Add(dr);
		dr[3] = "R E S U M E N";
		
		deb1=0;
		cre1=0;
		
		for(i=0; i<ds.Tables[1].Rows.Count; i++)
		{
			cVal = ds.Tables[1].Rows[i][0].ToString();
			deb1 += Convert.ToDouble(ds.Tables[1].Rows[i][2]);
			cre1 += Convert.ToDouble(ds.Tables[1].Rows[i][3]);
			if(i != ds.Tables[1].Rows.Count-1)
				aVal = ds.Tables[1].Rows[i+1][0].ToString();
			else
				aVal = "00000";
			
			if(cVal != aVal)
			{
				dr = tbReport.NewRow();
				tbReport.Rows.Add(dr);
				dr[2] = ds.Tables[1].Rows[i][0].ToString();
				dr[3] = ds.Tables[1].Rows[i][1].ToString();
				dr[4] = deb1;
				dr[5] = cre1;
				totalDeb = deb1;
				totalCre = cre1;
			}
		}
		
		dr = tbReport.NewRow();
		tbReport.Rows.Add(dr);
		
		dr = tbReport.NewRow();
		tbReport.Rows.Add(dr);
		dr[3] = "TOTAL MOVIMIENTO";
		dr[4] = totalDeb;
		dr[5] = totalCre;
		//reportInfo.Text += ds.Tables.Count.ToString() + "<br>";
		frontEnd.PressFieldTitles(report);
		//DataView dv = frontEnd.SortWithNextField(2,0);
		report.DataSource = ds.Tables[2];
		report.DataBind();

        StringBuilder SB=new StringBuilder();
        StringWriter SW=new StringWriter(SB);
        HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        tabPreHeader.RenderControl(htmlTW);
        report.RenderControl(htmlTW);
        tabFirmas.RenderControl(htmlTW);
        string strRep;
        strRep=SB.ToString();
		Session.Clear();
		Session["Rep"]=strRep;
		toolsHolder.Visible = true;
	}
		
	protected DataTable ApplyFilter(DataView dv, int num)
	{
		DataTable dtReportsGen = new DataTable();
		for(j=0; j<tbReport.Columns.Count; j++)
            	dtReportsGen.Columns.Add(new DataColumn(tbReport.Columns[j].ToString(), tbReport.Columns[j].DataType));
		
		for(i=0; i<dv.Count; i++)
		{
			if(dv[i][0].ToString().Length <= num)
			{
				dr = dtReportsGen.NewRow();
				dtReportsGen.Rows.Add(dr);
				dr[0] = dv[i][0];
				dr[1] = dv[i][1];
				dr[2] = dv[i][2];
				dr[3] = dv[i][3];
				dr[4] = dv[i][4];
				dr[5] = dv[i][5];
				dr[6] = dv[i][6];
				dr[7] = dv[i][7];
			}
		}
		
		return dtReportsGen;
		
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
