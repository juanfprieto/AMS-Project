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
using AMS.Tools;

namespace AMS.Contabilidad
{
	
	
public partial class BalPru : System.Web.UI.UserControl 
{
	
	protected PlaceHolder  preHeaderHolder ;
	protected DropDownList year;
	protected DropDownList month;
	protected DataTable tbReport;
	protected DataRow dr;
	protected DataColumn dc;
	protected DataSet ds;
	protected string reportTitle = "Balance de Prueba";
	protected string[] reportDatas, pr, values;
	protected int i, j;
	
	
	protected void Page_Load(object sender, System.EventArgs e)
	{
		//if(!IsPostBack)
		//{
			Filters bind = new Filters(reportTitle, filterHolder);
			//reportInfo.Text = bind.InfoProcess;
			if(bind.FiltersNum > 0)
			{
				Button filterButton = new Button();
				filterButton.Text = "Generar Reporte";
				filterButton.Click += new EventHandler(this.MakeReport);
				filterHolder.Controls.Add(new LiteralControl("<br><br>"));
				filterHolder.Controls.Add(filterButton);
			}
		//}
		
		
	}
	



	protected void Report_ItemDataBound(object sender, DataGridItemEventArgs e)
 	{		
 		
 		if(e.Item.ItemType != ListItemType.Header)
 		{
			//e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Right;
 			//e.Item.Cells[3].HorizontalAlign = HorizontalAlign.Right;
 			//e.Item.Cells[4].HorizontalAlign = HorizontalAlign.Right;
 			//e.Item.Cells[5].HorizontalAlign = HorizontalAlign.Right;
 		}
 		
 		
 	}
	
	
	
		
	public void MakeReport(Object Sender, EventArgs E)
	{
		DatasForReport balPru = new DatasForReport(reportTitle);
		ds = balPru.GetDatas(false, GetParams());
			
		Press frontEnd = new Press(ds, reportTitle);
		frontEnd.PreHeader(tabPreHeader, report.Width, pr);
		frontEnd.Firmas(tabFirmas,report.Width);
		frontEnd.SourceFieldTitles(balPru.PressFields);
	
		tbReport = frontEnd.TbReport;
		
		decimal subtotal=0, total=0;
		string cVal, aVal;
		DataRow[] rows;
		//Operations op = new Operations(ds, balPru.Operations);
		//op.Executer();
		//reportInfo.Text += op.InfoProcess;
		
		for(i=0; i<ds.Tables[1].Rows.Count; i++)
		{
			dr = tbReport.NewRow();
			tbReport.Rows.Add(dr);
			cVal = ds.Tables[1].Rows[i][0].ToString();
			dr[0] = cVal;
			dr[1] = ds.Tables[1].Rows[i][1];
			
			rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal.Substring(0,4) + "'");
			if(rows[0][2].ToString() == "D")
				dr[2] = (Decimal)ds.Tables[1].Rows[i][2];
			else
				dr[5] = (Decimal)ds.Tables[1].Rows[i][2];
		
			subtotal += (Decimal)Convert.ToDecimal(ds.Tables[1].Rows[i][2]);
		
			if(i != ds.Tables[1].Rows.Count-1)
			{
				aVal = ds.Tables[1].Rows[i+1][0].ToString();
				if(cVal.Substring(0,4) != aVal.Substring(0,4))
				{
					dr = tbReport.NewRow();
					tbReport.Rows.Add(dr);
					dr[0] = "TOTAL";
					rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal.Substring(0,4) + "'");
					dr[1] = rows[0][1];
					
					if(rows[0][2].ToString() == "D")
						dr[3] = subtotal;
					else
						dr[6] = subtotal;
					
					total += subtotal;
					subtotal = 0;
				
				}
				if(cVal.Substring(0,2) != aVal.Substring(0,2))
				{
					dr = tbReport.NewRow();
					tbReport.Rows.Add(dr);
					dr[0] = "TOTAL";
					rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal.Substring(0,2) + "'");
					dr[1] = rows[0][1];
					
					if(rows[0][2].ToString() == "D")
						dr[4] = total;
					else
						dr[7] = total;
					
					total = 0;
				
				}
			}
			toolsHolder.Visible = true;
		
		}
		report.DataSource = ds.Tables[2];
		frontEnd.PressFieldTitles(report);
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
	
	protected string[] GetParams(){
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
		for(i=0,k=0; i<values.Length; i++){
			if(values[i] != null){
				k++;
			}
		}
		theParams = new String[k];
		for(i=0,k=0; i<values.Length; i++){
			if(values[i] != null){
				theParams[k] = values[i];
				k++;
			}
		}
		pr = valuesTxt;
		return theParams;
	}
	
	public void SendMail(Object Sender, ImageClickEventArgs E){
     	MakeReport(Sender, E);
     	Utils.MostrarAlerta(Response, Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, report));
        report.EnableViewState=false;
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
