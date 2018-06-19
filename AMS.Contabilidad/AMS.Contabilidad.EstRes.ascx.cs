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


namespace AMS.Contabilidad
{


public partial class EstRes : System.Web.UI.UserControl
{

	protected PlaceHolder  preHeaderHolder ;
	protected Label  lbEmpresa, lbFecha, lbTitulo, lbParams;
	protected DataGrid  dgTest;
	protected DataTable tbReport;
	protected DataRow dr, drAux;
	protected DataColumn dc;
	protected DataSet ds;
	protected HtmlGenericControl spReport;
	protected string tmpText, reportTitle="Estado de Resultados";
	protected string[] reportDatas, pr, values;
	protected int i, j;
    protected double[] TDeb;
    protected double[] TCred;

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
 		}
 	}
	public void mirar(string v,int tot)
	{
		if (v == "523")
		{
			string t = v;
		}
		if (tot == 212)
		{
			string t = v;
		}
	}

    public void Inserta(ArrayList ar,string va,double p,ArrayList av){
		
		string v = va.Trim();
        int t=v.Length,a;
		string an;
        if(!ar.Contains(v)){
			mirar(v,ar.Count);
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
				mirar(v,ar.Count);
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
			mirar(v,ar.Count);
            ar.Add(an);
            av.Add(p);
        }
        else{
            a=ar.IndexOf(an);
            av[a]=((double)av[a])+p;
        }
    }

	public void MakeReport(Object Sender, EventArgs E){
		DatasForReport balGen = new DatasForReport(reportTitle);
		ds = balGen.GetDatas(false, GetParams());
		//3 tablas:
		// 1. mcue_codipuc, mcue_nombre, tnat_codigo, timp_codigo FROM mcuenta
		// 2. mcuenta.mcue_codipuc as CODIGO, mcuenta.mcue_nombre as NOMBRE, parcial=msal_valodebi-msal_valocred o msal_valocred-msal_valodebi WHERE mcuenta.mcue_codipuc=msaldocuenta.mcue_codipuc AND msaldocuenta.pano_ano=@ AND msaldocuenta.pmes_mes<=@ AND mcuenta.timp_codigo in ('N','P','@') AND mcuenta.tcla_codigo='R'
		// 3. mauxiliarcuenta.mcue_codipuc as COD, mauxiliarcuenta.mnit_nit as NIT, mnit.mnit_nombres+mnit.mnit_apellidos as NOMBRE, DETALLE = mauxiliarcuenta.maux_valodebi-mauxiliarcuenta.maux_valocred o mauxiliarcuenta.maux_valocred-mauxiliarcuenta.maux_valodebi FROM mcuenta, mauxiliarcuenta, mnit WHERE mauxiliarcuenta.mnit_nit=mnit.mnit_nit AND mcuenta.mcue_codipuc=mauxiliarcuenta.mcue_codipuc AND mauxiliarcuenta.pano_ano=@ AND mauxiliarcuenta.pmes_mes<=@ AND mauxiliarcuenta.mnit_nit != '@'
		//reportInfo.Text=ds.Tables.Count.ToString();

		Press frontEnd = new Press(ds, reportTitle);
		frontEnd.PreHeader(tabPreHeader, report.Width, pr);
		frontEnd.Firmas(tabFirmas,report.Width);
		frontEnd.SourceFieldTitles(balGen.PressFields);
		tbReport = frontEnd.TbReport;
		ArrayList arC = new ArrayList();
	    ArrayList arV = new ArrayList();
		DataRow[] rows;
		int l;
		TCred=new double[10];
		TDeb=new double[10];
		for(i=0;i<10;i++){TCred[i]=0;TDeb[i]=0;};
		for(i=0; i<ds.Tables[1].Rows.Count; i++){
		      Inserta(arC,ds.Tables[1].Rows[i][0].ToString(),Convert.ToDouble(ds.Tables[1].Rows[i][2]),arV);
		      //Calcular Utilidades aca:
		      //UBruta,UEjer,UOper
		      /*if(Convert.ToInt16(ds.Tables[1].Rows[i][3])==1)UBruta+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
		      if(Convert.ToInt16(ds.Tables[1].Rows[i][3])==2)UEjer+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
		      if(Convert.ToInt16(ds.Tables[1].Rows[i][3])==3)UOper+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);*/
		      if(ds.Tables[1].Rows[i][4].ToString().ToUpper()=="D")TDeb[Convert.ToInt16(ds.Tables[1].Rows[i][0].ToString().Substring(0,1))]+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
		      if(ds.Tables[1].Rows[i][4].ToString().ToUpper()=="C")TCred[Convert.ToInt16(ds.Tables[1].Rows[i][0].ToString().Substring(0,1))]+=Convert.ToDouble(ds.Tables[1].Rows[i][2]);
		}
		//arC.Sort();
		int tot = arC.Count;
		for(i=0;i<tot;i++){
		    //reportInfo.Text+=arC[i].ToString()+"<br>";
		    l=arC[i].ToString().Length;
		    dr = tbReport.NewRow();
		
			dr[0] = arC[i].ToString();
			rows = ds.Tables[0].Select("MCUE_CODIPUC = '" + arC[i].ToString() + "'");
			if (rows.Length > 0)
			{
				dr[1] = rows[0][1];
			}
			if(l<3)
			{
				dr[5] = ((double)arV[i]).ToString();
			}
			else
			{
				if(l==4)
				{
					dr[4] = ((double)arV[i]).ToString();
				}
				else
				{
					if(l==6)
					{
						dr[3] = ((double)arV[i]).ToString();
					}
					else
					{
						dr[2] = ((double)arV[i]).ToString();
					}
				}
			}
			if(((double)arV[i])!=0)tbReport.Rows.Add(dr);
	    }

		frontEnd.PressFieldTitles(report);
		DataView dv = frontEnd.SortWithNextField(3,0);
		DataTable dtTmp = dv.Table;
    	if(values[3] == "A"){
			report.DataSource = SetAux(dv,0);
			report.DataBind();
		}
		else if(values[3] == "D"){
			report.DataSource = SetAux(dv,14);
			report.DataBind();
		}
		else if(values[3] == "G"){
			report.DataSource = SetAux(dv,6);
			report.DataBind();
		}
		else if(values[3] == "M"){
			report.DataSource = SetAux(dv,4);
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

	protected DataTable SetAux(DataView dv, int f){
		string cVal,oV="";
		DataRow[] rows;
		double sA=0,t=0;
		bool sl=false;
		bool ub=false,uo=false;
		int n=1;

		DataTable dtReportsAux = new DataTable();
		for(j=0; j<tbReport.Columns.Count; j++)
           	dtReportsAux.Columns.Add(new DataColumn(tbReport.Columns[j].ToString(), tbReport.Columns[j].DataType));
		for(i=0; i<dv.Count; i++){
			oV=dv[i][0].ToString();
			if(oV.Trim().Length==1){
                if(!ub)    //Utilidad bruta
                    if(Convert.ToInt16(oV.Trim())>4){
                        t=0;
                        dr = dtReportsAux.NewRow();
                        dr[0] = "UTILIDAD BRUTA:";
                        for(j=0;j<=4;j++)
                            t+=TCred[j]-TDeb[j];
                        dr[5] = t.ToString();
                        dtReportsAux.Rows.Add(dr);
                        ub=true;
                    }
                if(!uo)    //Utilidad bruta
                    if(Convert.ToInt16(oV.Trim())>5){
                        t=0;
                        dr = dtReportsAux.NewRow();
                        dr[0] = "UTILIDAD OPERACIONAL:";
                        for(j=0;j<=5;j++)
                            t+=TCred[j]-TDeb[j];
                        dr[5] = t.ToString();
                        dtReportsAux.Rows.Add(dr);
                        uo=true;
                    }
            }
			dr = dtReportsAux.NewRow();
			dr[0] = dv[i][0];
			dr[1] = dv[i][1];
			dr[2] = dv[i][2];
			dr[3] = dv[i][3];
			dr[4] = dv[i][4];
			dr[5] = dv[i][5];
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
	          if(f==0||dr[0].ToString().Length<=f){
		  	       dtReportsAux.Rows.Add(dr);
		  	       sl=true;
		  	  }
              sA=0;
              n=1;
		  }
////////
			if(f==0&&sl){
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
			}
		}
        if(!ub){    //Utilidad bruta
            t=0;
            dr = dtReportsAux.NewRow();
            dr[0] = "UTILIDAD BRUTA:";
            for(j=0;j<=4;j++)
                t+=TCred[j]-TDeb[j];
            dr[5] = t.ToString();
            dtReportsAux.Rows.Add(dr);
            ub=true;
        }
        if(!uo){    //Utilidad operacional
            t=0;
            dr = dtReportsAux.NewRow();
            dr[0] = "UTILIDAD OPERACIONAL:";
            for(j=0;j<=5;j++)
                t+=TCred[j]-TDeb[j];
            dr[5] = t.ToString();
            dtReportsAux.Rows.Add(dr);
            uo=true;
        }
        dr = dtReportsAux.NewRow();
        dr[0] = "UTILIDAD DEL EJERCICIO:";
        t=0;
        for(j=0;j<10;j++)
            t+=TCred[j]-TDeb[j];
        dr[5] = t.ToString();
        dtReportsAux.Rows.Add(dr);
		return dtReportsAux;
	}

	protected string[] GetParams(){
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
		pr[0]=pr[2]+" "+pr[0];
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
