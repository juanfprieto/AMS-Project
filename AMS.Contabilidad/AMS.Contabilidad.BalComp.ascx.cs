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
using AMS.Tools;

namespace AMS.Contabilidad{
public partial class BalComp : System.Web.UI.UserControl{
	protected PlaceHolder  preHeaderHolder ;
	protected Label  lbEmpresa, lbFecha, lbTitulo, lbParams;
	protected DataGrid  dgTest;
	protected DataTable tbReport;
	protected DataRow dr, drAux;
	protected DataColumn dc;
	protected DataSet ds;
	protected HtmlGenericControl spReport;
	protected string tmpText, reportTitle="Balance Comparativo";
	protected string[] reportDatas, pr, values;
	protected int i, j;
	protected double TCred,TDeb;
   //Total Credito, Debito

	protected void Page_Load(object sender, System.EventArgs e){
		Filters bind = new Filters(reportTitle, filterHolder);
		reportInfo.Text = bind.InfoProcess + "<br>";
		if(bind.FiltersNum > 0){
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
 		}
 	}

    public void Inserta(ArrayList ar,string v,double p,ArrayList av,ArrayList arT){
        int t=v.Length,a;
        string an;
        if(!arT.Contains(v)){   //Todas las cuentas
            arT.Add(v);
        }
        if(!ar.Contains(v)){
            ar.Add(v);
            av.Add(p);
        }
        else{
            a=ar.IndexOf(v);
            av[a]=((double)av[a])+p;
        }
        if(t==1)
            return;
        for(int n=t-2;n>=2;n-=2){
            an=v.Substring(0,n);
            if(!ar.Contains(an)){
                ar.Add(an);
                av.Add(p);
            }
            else{
                a=ar.IndexOf(an);
                av[a]=((double)av[a])+p;
            }
        }
        an=v.Substring(0,1);
        if(!ar.Contains(an)){
            ar.Add(an);
            av.Add(p);
        }
        else{
            a=ar.IndexOf(an);
            av[a]=((double)av[a])+p;
        }
    }

	public void MakeReport(Object Sender, EventArgs E){
		DatasForReport balComp = new DatasForReport(reportTitle);
		ds = balComp.GetDatas(false, GetParams());
        
		//3 tablas:
		// 1. mcue_codipuc, mcue_nombre, tnat_codigo, timp_codigo FROM mcuenta
		// 2. mcuenta.mcue_codipuc as CODIGO, mcuenta.mcue_nombre as NOMBRE, parcial=msal_valodebi-msal_valocred o msal_valocred-msal_valodebi WHERE mcuenta.mcue_codipuc=msaldocuenta.mcue_codipuc AND msaldocuenta.pano_ano=@ AND msaldocuenta.pmes_mes<=@ AND mcuenta.timp_codigo in ('N','P','@') AND mcuenta.tcla_codigo='R'
		// 3. mauxiliarcuenta.mcue_codipuc as COD, mauxiliarcuenta.mnit_nit as NIT, mnit.mnit_nombres+mnit.mnit_apellidos as NOMBRE, DETALLE = mauxiliarcuenta.maux_valodebi-mauxiliarcuenta.maux_valocred o mauxiliarcuenta.maux_valocred-mauxiliarcuenta.maux_valodebi FROM mcuenta, mauxiliarcuenta, mnit WHERE mauxiliarcuenta.mnit_nit=mnit.mnit_nit AND mcuenta.mcue_codipuc=mauxiliarcuenta.mcue_codipuc AND mauxiliarcuenta.pano_ano=@ AND mauxiliarcuenta.pmes_mes<=@ AND mauxiliarcuenta.mnit_nit != '@'
		//reportInfo.Text=ds.Tables.Count.ToString();
		Press frontEnd = new Press(ds, reportTitle);
		frontEnd.PreHeader(tabPreHeader, report.Width, pr);
		frontEnd.Firmas(tabFirmas,report.Width);
		frontEnd.SourceFieldTitles(balComp.PressFields);
		tbReport = frontEnd.TbReport;
		ArrayList arC0 = new ArrayList();
		ArrayList arC1 = new ArrayList();
		ArrayList arC2 = new ArrayList();
	    ArrayList arV0 = new ArrayList();
	    ArrayList arV1 = new ArrayList();
	    ArrayList arV2 = new ArrayList();
		ArrayList arC=null;
	    ArrayList arV=null;
        ArrayList arCT=new ArrayList();
		DataRow[] rows;
		int ano,ai;
		ano=Convert.ToInt16(values[0]);
		for(i=0; i<ds.Tables[1].Rows.Count; i++){
		      ai=ano-Convert.ToInt16(ds.Tables[1].Rows[i][4]);
		      if(ai==0){arC=arC0;arV=arV0;}
		      if(ai==1){arC=arC1;arV=arV1;}
		      if(ai==2){arC=arC2;arV=arV2;}
		      Inserta(arC,ds.Tables[1].Rows[i][0].ToString(),Convert.ToDouble(ds.Tables[1].Rows[i][2]),arV,arCT);
		      if(ds.Tables[1].Rows[i][3].ToString().ToUpper()=="D")TDeb+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
		      if(ds.Tables[1].Rows[i][3].ToString().ToUpper()=="C")TCred+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
		}
		double v0,v1,v2;
		if(arCT.Count>0)
		for(i=0;i<arCT.Count;i++){
		    dr = tbReport.NewRow();
			dr[0] = arCT[i].ToString();
			rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + arCT[i].ToString() + "'");
		    dr[1] = rows[0][1];
		    if(arC0.Contains(arCT[i]))
		      v0=(double)arV0[i];
		    else
		      v0=0;
		    dr[2] = (v0).ToString();
            if(arC1.Contains(arCT[i]))
		      v1=(double)arV1[i];
		    else
		      v1=0;
		    dr[3] = (v1).ToString();
            if(arC2.Contains(arCT[i]))
		      v2=(double)arV2[i];
		    else
		      v2=0;
		    dr[4] = (v2).ToString();
		    if(v0!=0){
                dr[5]=(((v1-v0)*100)/v0).ToString();   // % Año 0-1
                dr[6]=(((v2-v0)*100)/v0).ToString();   // % Año 0-2
            }
            else{
                dr[5]="0.00";
                dr[6]="0.00";
            }
            if(!(v0==0&&v1==0&&v2==0))
                tbReport.Rows.Add(dr);
	    }
		frontEnd.PressFieldTitles(report);
		DataView dv = frontEnd.SortWithNextField(3,0);
		DataTable dtTmp = dv.Table;
		if(values[2] == "A")
			report.DataSource = SetAux(dv,0);
		else if(values[2] == "D")
			report.DataSource = SetAux(dv,14);
		else if(values[2] == "G")
			report.DataSource = SetAux(dv,10);
		else if(values[2] == "M")
			report.DataSource = SetAux(dv,6);
		report.Columns[2].HeaderText=values[0];
		report.Columns[3].HeaderText=(Convert.ToInt16(values[0])-1).ToString();
		report.Columns[4].HeaderText=(Convert.ToInt16(values[0])-2).ToString();
		report.Columns[5].HeaderText="% "+values[0]+"-"+(Convert.ToInt16(values[0])-1).ToString();
		report.Columns[6].HeaderText="% "+values[0]+"-"+(Convert.ToInt16(values[0])-2).ToString();
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

	protected DataTable SetAux(DataView dv, int f){
		string cVal,oV="";
		DataRow[] rows;
		double sA=0;
		int n=1;
		DataTable dtReportsAux = new DataTable();
		for(j=0; j<tbReport.Columns.Count; j++)
           	dtReportsAux.Columns.Add(new DataColumn(tbReport.Columns[j].ToString(), tbReport.Columns[j].DataType));
		for(i=0; i<dv.Count; i++){
			oV=dv[i][0].ToString();
			dr = dtReportsAux.NewRow();
			dr[0] = dv[i][0];
			dr[1] = dv[i][1];
			dr[2] = dv[i][2];
			dr[3] = dv[i][3];
			dr[4] = dv[i][4];
			dr[5] = dv[i][5];
			dr[6] = dv[i][6];
			cVal = dv[i][0].ToString();
			rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + cVal + "'");
////////
	      if(i<dv.Count-1 && dv[i][0].ToString()==dv[i+1][0].ToString() && dv[i+1][3]!=DBNull.Value){
		      sA+=Convert.ToDouble(dv[i][3]);
		      n++;
		  }
		  else{
		      if(dv[i][3]!=DBNull.Value)
	               sA+=Convert.ToDouble(dv[i][3]);
	          if(dv[i][4]==DBNull.Value && dv[i][5]==DBNull.Value)
	               if(sA!=0)dr[3] = sA;
	          if(f==0||dr[0].ToString().Length<=f)
		  	       dtReportsAux.Rows.Add(dr);
              sA=0;
              n=1;
		  }
////////
/*			if(f==0&&sl){
				rows = ds.Tables[2].Select("COD = '" + cVal + "'");
				for(j=0; j<rows.Length; j++){
				    if(sl){
					   drAux = dtReportsAux.NewRow();
					   dtReportsAux.Rows.Add(drAux);
					   drAux [0] = "&raquo; " + rows[j][1];
					   drAux [1] = rows[j][2];

					   drAux [2] = Convert.ToDouble(rows[j][3]);
					}
					sl=false;
				}
			}*/
		}
		return dtReportsAux;
	}

	protected string[] GetParams()
	{
		values = new string[filterHolder.Controls.Count];
		string[] valuesTxt = new string[filterHolder.Controls.Count];
		string[] theParams;
		int i,k;
		for(i=0,k=0; i<filterHolder.Controls.Count; i++){
			if(filterHolder.Controls[i].ToString() == "System.Web.UI.WebControls.DropDownList"){
				DropDownList dd = (DropDownList)filterHolder.Controls[i];
				values[k] = dd.SelectedValue;
				valuesTxt[k] = dd.SelectedItem.Text;
				k++;
			}
			if(filterHolder.Controls[i].ToString() == "System.Web.UI.WebControls.TextBox"){
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
    }
    
	#region Web Form Designer generated code
	override protected void OnInit(EventArgs e){
		InitializeComponent();
		base.OnInit(e);
	}

	private void InitializeComponent(){

	}
	#endregion
}
}
