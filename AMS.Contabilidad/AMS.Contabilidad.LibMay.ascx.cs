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
using AMS.Forms;
using AMS.Reportes;
using AMS.DB;

namespace AMS.Contabilidad
{
	
	
public partial class LibMay : System.Web.UI.UserControl
{
	
	protected PlaceHolder  preHeaderHolder ; 
	protected Label  lbEmpresa, lbFecha, lbTitulo, lbParams;
	protected DataGrid  dgTest;
	protected DataTable tbReport;
	protected DataRow dr, drAux, drTotales;
	protected DataColumn dc;
	protected DataSet ds;
	protected HtmlGenericControl spReport;
	protected string tmpText, reportTitle="Libro Mayor";
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

	protected void Report_ItemDataBound(object sender, DataGridItemEventArgs e){	
 		if(e.Item.ItemType != ListItemType.Header){
			e.Item.Cells[2].HorizontalAlign = HorizontalAlign.Right;
 			e.Item.Cells[3].HorizontalAlign = HorizontalAlign.Right;
 			e.Item.Cells[4].HorizontalAlign = HorizontalAlign.Right;
 			e.Item.Cells[5].HorizontalAlign = HorizontalAlign.Right;
 			e.Item.Cells[6].HorizontalAlign = HorizontalAlign.Right;
 			e.Item.Cells[7].HorizontalAlign = HorizontalAlign.Right;
 		}
 		else
 		{
 			e.Item.Cells[2].Text = "<div style='margin-bottom:5; text-align:right'>SALDO</div>DEBITO";
 			e.Item.Cells[3].Text = "<div style='margin-bottom:5'>ANTERIOR</div>CREDITO";
 			e.Item.Cells[4].Text = "<div style='margin-bottom:5; text-align:right'>SALDO</div>DEBITO";
 			e.Item.Cells[5].Text = "<div style='margin-bottom:5'>ACTUAL</div>CREDITO";
 			e.Item.Cells[6].Text = "<div style='margin-bottom:5; text-align:right'>SALDO</div>DEBITO";
 			e.Item.Cells[7].Text = "<div style='margin-bottom:5'>FINAL</div>CREDITO";
 		}
 		
 	}
	
	
	
	
	public void MakeReport(Object Sender, EventArgs E)
	{

		DatasForReport balGen = new DatasForReport(reportTitle);
		ds = balGen.GetDatas(false, GetParams());
		//reportInfo.Text += balGen.SQL[0] + "<br>";
		//dgTest.DataSource = ds.Tables[2];
		//dgTest.DataBind();
		
		Press frontEnd = new Press(ds, reportTitle);
		frontEnd.PreHeader(tabPreHeader, report.Width, pr);
		frontEnd.Firmas(tabFirmas,report.Width);
		frontEnd.SourceFieldTitles(balGen.PressFields);
		
		tbReport = frontEnd.TbReport;
		
		double parcialD1=0, parcialD2=0, parcialD3=0, parcialC1=0, parcialC2=0, parcialC3=0;
		double subtotalD1=0, subtotalD2=0, subtotalD3=0, subtotalC1=0, subtotalC2=0, subtotalC3=0;
		double totalD1=0, totalD2=0, totalD3=0, totalC1=0, totalC2=0, totalC3=0;
		string cVal, aVal;
		string mcue="";
		double saldoAntCuenta=0,saldoAntSubCuenta=0,saldoAntSubSubCuenta=0;
		double saldoCuenta=0,saldoSubCuenta=0,saldoSubSubCuenta=0;
		double saldo=0;
		double totalSaldoAntDeb=0,totalSaldoAntCre=0,totalSaldoActDeb=0,totalSaldoActCre=0,totalSaldoDeb=0,totalSaldoCre=0;
		try{mcue=DBFunctions.SingleData("SELECT mcue_codipuc FROM ccontabilidad");}catch{};
		mcue=mcue.Trim();
		DataRow[] rows;
		
		for(i=0; i<ds.Tables[3].Rows.Count; i++)
		{
			dr = tbReport.NewRow();
			cVal = ds.Tables[3].Rows[i][0].ToString();
			dr[0] = cVal;
			if(dr[0].ToString()!=mcue)
			{
				tbReport.Rows.Add(dr);
				dr[1] = ds.Tables[3].Rows[i][1];
				saldo=CalcularSaldoCuenta(cVal,Convert.ToDouble(ds.Tables[3].Rows[i][2]),Convert.ToDouble(ds.Tables[3].Rows[i][3]));
				if(NaturalezaCuenta(cVal)=="D")
				{
					if(saldo>0)
					{
						dr[6]=saldo;
						totalSaldoDeb=totalSaldoDeb+saldo;
					}
					else if(saldo<0)
					{
						dr[7]=saldo*-1;
						totalSaldoCre=totalSaldoCre+(saldo*-1);
					}
				}
				else if(NaturalezaCuenta(cVal)=="C")
				{
					if(saldo>0)
					{
						dr[7]=saldo;
						totalSaldoCre=totalSaldoCre+saldo;
					}
					else if(saldo<0)
					{
						dr[6]=saldo*-1;
						totalSaldoDeb=totalSaldoDeb+(saldo*-1);
					}
				}
				/*if(Convert.ToDouble(ds.Tables[3].Rows[i][2]) != 0)
					dr[6] = Convert.ToDouble(ds.Tables[3].Rows[i][2]);
				if(Convert.ToDouble(ds.Tables[3].Rows[i][3]) != 0)
					dr[7] = Convert.ToDouble(ds.Tables[3].Rows[i][3]);*/
		
				rows = ds.Tables[2].Select("CODIGO = '" + cVal + "'");
				if(rows.Length > 0)
				{
					dr[4] = Convert.ToDouble(rows[0][2]);
					dr[5] = Convert.ToDouble(rows[0][3]);
					parcialD2 += Convert.ToDouble(rows[0][2]);
					parcialC2 += Convert.ToDouble(rows[0][3]);
					totalSaldoActDeb=totalSaldoActDeb+Convert.ToDouble(rows[0][2]);
					totalSaldoActCre=totalSaldoActCre+Convert.ToDouble(rows[0][3]);
				}

				rows = ds.Tables[1].Select("CODIGO = '" + cVal + "'");
				if(rows.Length > 0)
				{
					dr[2] = Convert.ToDouble(rows[0][2]);
					dr[3] = Convert.ToDouble(rows[0][3]);
					parcialD1 += Convert.ToDouble(rows[0][2]);
					parcialC1 += Convert.ToDouble(rows[0][3]);
					totalSaldoAntDeb=totalSaldoAntDeb+Convert.ToDouble(rows[0][2]);
					totalSaldoAntCre=totalSaldoAntCre+Convert.ToDouble(rows[0][3]);
				}
				
				parcialD3 += Convert.ToDouble(ds.Tables[3].Rows[i][2]);
				parcialC3 += Convert.ToDouble(ds.Tables[3].Rows[i][3]);
				if(i != ds.Tables[3].Rows.Count-1)
					aVal = ds.Tables[3].Rows[i+1][0].ToString();
				else
					aVal = "00000";
				if(cVal.Substring(0,4) != aVal.Substring(0,4))
				{
					dr = tbReport.NewRow();
					tbReport.Rows.Add(dr);					
					dr[0] = cVal.Substring(0,4);
					rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal.Substring(0,4) + "'");
					dr[1] = rows[0][1];
					saldoAntSubSubCuenta=CalcularSaldoCuenta(cVal.Substring(0,4),parcialD1,parcialC1);
					if(NaturalezaCuenta(cVal.Substring(0,4))=="D")
					{
						if(saldoAntSubSubCuenta>0)
							dr[2] = saldoAntSubSubCuenta;
						else if(saldoAntSubCuenta<0)
							dr[3] = saldoAntSubSubCuenta*-1;
					}
					else if(NaturalezaCuenta(cVal.Substring(0,4))=="C")
					{
						if(saldoAntSubSubCuenta>0)
							dr[3] = saldoAntSubSubCuenta;
						else if(saldoAntSubSubCuenta<0)
							dr[2] = saldoAntSubSubCuenta*-1;
					}
					dr[4] = parcialD2;
					dr[5] = parcialC2;
					saldoSubSubCuenta=CalcularSaldoCuenta(cVal.Substring(0,4),parcialD3,parcialC3);
					if(NaturalezaCuenta(cVal.Substring(0,4))=="D")
					{
						if(saldoSubSubCuenta>0)
							dr[6] = saldoSubSubCuenta;
						else if(saldoSubSubCuenta<0)
							dr[7] = saldoSubSubCuenta*-1;
					}
					else if(NaturalezaCuenta(cVal.Substring(0,4))=="C")
					{
						if(saldoSubSubCuenta>0)
							dr[7] = saldoSubSubCuenta;
						else if(saldoSubSubCuenta<0)
							dr[6] = saldoSubSubCuenta*-1;
					}
					subtotalD1 += parcialD1; subtotalD2 += parcialD2; subtotalD3 += parcialD3;
					subtotalC1 += parcialC1; subtotalC2 += parcialC2; subtotalC3 += parcialC3;
					parcialD1 = 0; parcialD2 = 0; parcialD3 = 0;
					parcialC1 = 0; parcialC2 = 0; parcialC3 = 0;
				}
				if(cVal.Substring(0,2) != aVal.Substring(0,2))
				{
					dr = tbReport.NewRow();
					tbReport.Rows.Add(dr);
					dr[0] = cVal.Substring(0,2);
					//*rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal.Substring(0,2) + "'");
					try{dr[1] = DBFunctions.SingleData("SELECT mcue_nombre FROM mcuenta WHERE mcue_codipuc='"+cVal.Substring(0,2)+"'");}
					catch{}
					saldoAntSubCuenta=CalcularSaldoCuenta(cVal.Substring(0,2),subtotalD1,subtotalC1);
					if(NaturalezaCuenta(cVal.Substring(0,2))=="D")
					{
						if(saldoAntSubCuenta>0)
							dr[2] = saldoAntSubCuenta;
						else if(saldoAntSubCuenta<0)
							dr[3] = saldoAntSubCuenta*-1;
					}
					else if(NaturalezaCuenta(cVal.Substring(0,2))=="C")
					{
						if(saldoAntSubCuenta>0)
							dr[3] = saldoAntSubCuenta;
						else if(saldoAntSubCuenta<0)
							dr[2] = saldoAntSubCuenta*-1;
					}
					dr[4] = subtotalD2;
					dr[5] = subtotalC2;
					saldoSubCuenta=CalcularSaldoCuenta(cVal.Substring(0,2),subtotalD3,subtotalC3);
					if(NaturalezaCuenta(cVal.Substring(0,2))=="D")
					{
						if(saldoSubCuenta>0)
							dr[6] = saldoSubCuenta;
						else if(saldoSubCuenta<0)
							dr[7] = saldoSubCuenta*-1;
					}
					else if(NaturalezaCuenta(cVal.Substring(0,2))=="C")
					{
						if(saldoSubCuenta>0)
							dr[7] = saldoSubCuenta;
						else if(saldoSubCuenta<0)
							dr[6] = saldoSubCuenta*-1;
					}
					totalD1 += subtotalD1; totalD2 += subtotalD2; totalD3 += subtotalD3;
					totalC1 += subtotalC1; totalC2 += subtotalC2; totalC3 += subtotalC3;
					subtotalD1 = 0; subtotalD2 = 0; subtotalD3 = 0;
					subtotalC1 = 0; subtotalC2 = 0; subtotalC3 = 0; 
				}
			
				if(cVal.Substring(0,1) != aVal.Substring(0,1))
				{
					dr = tbReport.NewRow();
					tbReport.Rows.Add(dr);
					dr[0] = cVal.Substring(0,1);
					try
					{
						rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal.Substring(0,1) + "'");
						dr[1] = rows[0][1];}
					catch
					{
						dr[1] = "";}
					saldoAntCuenta=CalcularSaldoCuenta(cVal.Substring(0,1),totalD1,totalC1);
					if(NaturalezaCuenta(cVal.Substring(0,1))=="D")
					{
						if(saldoAntCuenta>0)
							dr[2] = saldoAntCuenta;
						else if(saldoAntCuenta<0)
							dr[3] = saldoAntCuenta*-1;
					}
					else if(NaturalezaCuenta(cVal.Substring(0,1))=="C")
					{
						if(saldoAntCuenta>0)
							dr[3] = saldoAntCuenta;
						else if(saldoAntCuenta<0)
							dr[2] = saldoAntCuenta*-1;
					}
					dr[4] = totalD2;
					dr[5] = totalC2;
					saldoCuenta=CalcularSaldoCuenta(cVal.Substring(0,1),totalD3,totalC3);
					if(NaturalezaCuenta(cVal.Substring(0,1))=="D")
					{
						if(saldoCuenta>0)
							dr[6] = saldoCuenta;
						else if(saldoCuenta<0)
							dr[7] = saldoCuenta*-1;
					}
					else if(NaturalezaCuenta(cVal.Substring(0,1))=="C")
					{
						if(saldoCuenta>0)
							dr[7] = saldoCuenta;
						else if(saldoCuenta<0)
							dr[6] = saldoCuenta*-1;
					}
					totalD1 = 0; totalD2 = 0; totalD3 = 0;
					totalC1 = 0; totalC2 = 0; totalC3 = 0;
				}
			}
		}

		drTotales=tbReport.NewRow();
		dr[0]="T O T ";
		dr[1]=" A L E S ";
        dr[2]=totalSaldoAntDeb.ToString("N");
		dr[3]=totalSaldoAntCre.ToString("N");
		dr[4]=totalSaldoActDeb.ToString("N");
		dr[5]=totalSaldoActCre.ToString("N");
		dr[6]=totalSaldoDeb.ToString("N");
		dr[7]=totalSaldoCre.ToString("N");
		tbReport.Rows.Add(drTotales);
		
		frontEnd.PressFieldTitles(report);
		DataView dv = frontEnd.SortWithNextField(4,0);
		
		//DataTable dtTmp = dv.Table;
		
		
		if(values[2] == "D")
		{
			report.DataSource = dv;
			report.DataBind();
		}
		else if(values[2] == "G")
		{
			report.DataSource = ApplyFilter(dv, 6);
			report.DataBind();
		}
		else if(values[2] == "M")
		{
			report.DataSource = ApplyFilter(dv, 4);
			report.DataBind();
		}
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
	
	private double CalcularSaldoCuenta(string cue,double vd,double vc)
	{
		double saldo=0;
		if(NaturalezaCuenta(cue)=="D")
			saldo=vd-vc;
		else if(NaturalezaCuenta(cue)=="C")
			saldo=vc-vd;
		return saldo;
	}

	private string NaturalezaCuenta(string cuenta)
	{
		string naturaleza="";
		naturaleza=DBFunctions.SingleData("SELECT tnat_codigo FROM mcuenta WHERE mcue_codipuc='"+cuenta+"'");
		return naturaleza;
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
