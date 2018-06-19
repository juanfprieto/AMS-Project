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
using AMS.Tools;


namespace AMS.Contabilidad{
public partial class Terceros : System.Web.UI.UserControl{
	protected string reportTitle="Terceros por Cuenta";

	protected void Page_Load(object sender, System.EventArgs e){
		if(!IsPostBack){
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList( year, "SELECT pano_ano FROM pano");
			bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13");
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

	protected void Consulta_Click(object  Sender ,  EventArgs e)
	{
        int ord=0;
		if (RadioOpcion.SelectedValue=="CUE")
            ord=1;
		if (RadioOpcion.SelectedValue=="NIT")
            ord=2;
        if (RadioOpcion.SelectedValue=="NOM")
            ord=3;
        string sql,sql1,sql2,sql3;
        sql  = @"select m1.mcue_codipuc as mcue_codipuc,m1.mnit_nit,m2.mcue_nombre,case when m2.tnat_codigo='D' then m1.maux_valodebi-m1.maux_valocred when m2.tnat_codigo='C' then m1.maux_valocred-m1.maux_valodebi end as saldo,
                 m3.mnit_nombres || ' ' || coalesce(MNIT_NOMBRE2,'') as mnit_nombres,m3.mnit_apellidos || ' ' || coalesce(MNIT_APELLIDO2,'') as mnit_apellidos 
                 from  mauxiliarcuenta m1,mcuenta m2, mnit m3 
                 where m1.mcue_codipuc=m2.mcue_codipuc AND m3.mnit_nit=m1.mnit_nit AND m1.pmes_mes = " + month.SelectedValue + " AND m1.pano_ano=" + year.SelectedValue;
        
        sql1 = @"select m1.mcue_codipuc as mcue_codipuc,m1.mnit_nit as mnit_nit,case when m2.tnat_codigo='D' then m1.maux_valodebi-m1.maux_valocred when m2.tnat_codigo='C' then m1.maux_valocred-m1.maux_valodebi end as saldo 
                from  mauxiliarcuenta m1,mcuenta m2 
                where m1.mcue_codipuc=m2.mcue_codipuc AND m1.pmes_mes = 0 AND m1.pano_ano="+year.SelectedValue;
        
        sql2 = @"select m1.mcue_codipuc as mcue_codipuc,m1.mnit_nit as mnit_nit,case when m2.tnat_codigo='D' then sum(m1.maux_valodebi-m1.maux_valocred) when m2.tnat_codigo='C' then sum(m1.maux_valocred-m1.maux_valodebi) end as saldo 
                from mauxiliarcuenta m1,mcuenta m2 
                where m1.mcue_codipuc=m2.mcue_codipuc AND m1.pmes_mes > 0 AND m1.pmes_mes < " + month.SelectedValue + " AND m1.pano_ano = " + year.SelectedValue + " group by (m1.mcue_codipuc,m1.mnit_nit,m2.tnat_codigo)";
        
        sql3 = "Select MNIT_NIT, MNIT_NOMBRES || ' ' || coalesce(MNIT_NOMBRE2,''), MNIT_APELLIDOS || ' ' || coalesce(MNIT_APELLIDO2,'') from MNIT";
        if(ord==1)
            sql+=" ORDER BY mcue_codipuc,mnit_nit";
        if(ord==2)
            sql+=" ORDER BY mnit_nit,mcue_codipuc";
        if(ord==3)
            sql+=" ORDER BY mnit_apellidos,mnit_nombres,mnit_nit,mcue_codipuc";
        CultureInfo bz = new CultureInfo("en-us");
		DataSet ds=new DataSet();
        DBFunctions.Request(ds, IncludeSchema.NO,sql);
        DBFunctions.Request(ds, IncludeSchema.NO,sql1);
        DBFunctions.Request(ds, IncludeSchema.NO,sql2);
        DBFunctions.Request(ds, IncludeSchema.NO,sql3);
        string[] titleFields={"Cod","Nombre","Saldo Año Ant","Mvto Año", "Mvto Mes","Saldo"};
        string[] dataFields ={"Cod","Nombre","Saldo Año Ant","Mvto Año", "Mvto Mes","Saldo"};
        int i;
        double d1,d2,d3,d4;
        for(i=0;i<titleFields.Length;i++)
        {
            BoundColumn bc = new BoundColumn();
            bc.HeaderText=titleFields[i];
            bc.DataField=dataFields[i];
            bc.DataFormatString = "{0:N}";
            dg.Columns.Add(bc);
        }
        DataTable dtRep = new DataTable();
        DataRow dr;
        DataRow[] rowsT;
        double sm;
        for(i=0;i<titleFields.Length;i++)
            dtRep.Columns.Add(new DataColumn(dataFields[i],typeof(string)));
        string uCod="";
        if(ds.Tables[0].Rows.Count>0){
            for(i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                if(ord==1){//Cuenta
                    d1=d2=d3=d4=0;
                    dr=dtRep.NewRow();
                    dr[0]="<br>";
                    dtRep.Rows.Add(dr);
                    dr=dtRep.NewRow();
                    dr[0]=ds.Tables[0].Rows[i][0].ToString();
                    dr[1]=ds.Tables[0].Rows[i][2].ToString();
                    dtRep.Rows.Add(dr);
                    uCod=ds.Tables[0].Rows[i][0].ToString();
                    while(i<ds.Tables[0].Rows.Count && ds.Tables[0].Rows[i][0].ToString()==uCod){
                        dr=dtRep.NewRow();
                        dr[0]=ds.Tables[0].Rows[i][1].ToString();
                        rowsT=ds.Tables[3].Select("mnit_nit='"+ds.Tables[0].Rows[i][1].ToString()+"'");
                        if(rowsT.Length>0)
                            dr[1]=rowsT[0][1]+" "+rowsT[0][2];
                        else
                            dr[1]="No Existe";
                        rowsT=ds.Tables[1].Select("mcue_codipuc='"+uCod+"' AND mnit_nit='"+ds.Tables[0].Rows[i][1].ToString()+"'");
                        if(rowsT.Length>0)
                            dr[2]=((double)Convert.ToDouble(rowsT[0][2])).ToString("N");
                        else
                            dr[2]="0.00";
                        rowsT=ds.Tables[2].Select("mcue_codipuc='"+uCod+"' AND mnit_nit='"+ds.Tables[0].Rows[i][1].ToString()+"'");
                        if(rowsT.Length>0)
                            dr[3]=((double)Convert.ToDouble(rowsT[0][2])).ToString("N");
                        else
                            dr[3]="0.00";
                        dr[4]=((double)Convert.ToDouble(ds.Tables[0].Rows[i][3])).ToString("N");
                        sm=Convert.ToDouble(dr[4])+Convert.ToDouble(dr[3])+Convert.ToDouble(dr[2]);
                        dr[5]=sm.ToString("N");
                        d1+=Convert.ToDouble(dr[2]);
                        d2+=Convert.ToDouble(dr[3]);
                        d3+=Convert.ToDouble(dr[4]);
                        d4+=Convert.ToDouble(dr[5]);
                        i++;
                        dtRep.Rows.Add(dr);
                    }
                    i--;
                    dr=dtRep.NewRow();
                    dr[1]="Total:";
                    dr[2]=d1.ToString("N");
                    dr[3]=d2.ToString("N");
                    dr[4]=d3.ToString("N");
                    dr[5]=d4.ToString("N");
                    dtRep.Rows.Add(dr);                    
                }
                else{//NIT-Nombre
                    d1=d2=d3=d4=0;
                    dr=dtRep.NewRow();
                    dr[0]="<br>";
                    dtRep.Rows.Add(dr);
                    dr=dtRep.NewRow();
                    rowsT=ds.Tables[3].Select("mnit_nit='"+ds.Tables[0].Rows[i][1].ToString()+"'");
                    if(rowsT.Length>0)
                       dr[1]=rowsT[0][1]+" "+rowsT[0][2];
                    else
                       dr[1]="No Existe";
                    dr[0]=ds.Tables[0].Rows[i][1].ToString();
                    dtRep.Rows.Add(dr);
                    uCod=ds.Tables[0].Rows[i][1].ToString();
                    while(i<ds.Tables[0].Rows.Count && ds.Tables[0].Rows[i][1].ToString()==uCod){
                        dr=dtRep.NewRow();
                        dr[0]=ds.Tables[0].Rows[i][0].ToString();
                        dr[1]=ds.Tables[0].Rows[i][2].ToString();
                        rowsT=ds.Tables[1].Select("mnit_nit='"+uCod+"' AND mcue_codipuc='"+ds.Tables[0].Rows[i][0].ToString()+"'");
                        if(rowsT.Length>0)
                            dr[2]=((double)Convert.ToDouble(rowsT[0][2])).ToString("N");
                        else
                            dr[2]="0.00";
                        rowsT=ds.Tables[2].Select("mnit_nit='"+uCod+"' AND mcue_codipuc='"+ds.Tables[0].Rows[i][0].ToString()+"'");
                        if(rowsT.Length>0)
                            dr[3]=((double)Convert.ToDouble(rowsT[0][2])).ToString("N");
                        else
                            dr[3]="0.00";
                        dr[4]=((double)Convert.ToDouble(ds.Tables[0].Rows[i][3])).ToString("N");
                        sm=Convert.ToDouble(dr[4])+Convert.ToDouble(dr[3])+Convert.ToDouble(dr[2]);
                        dr[5]=sm.ToString("N");
                        d1+=Convert.ToDouble(dr[2]);
                        d2+=Convert.ToDouble(dr[3]);
                        d3+=Convert.ToDouble(dr[4]);
                        d4+=Convert.ToDouble(dr[5]);
                        i++;
                        dtRep.Rows.Add(dr);
                    }
                    i--;
                    dr=dtRep.NewRow();
                    dr[1]="Total:";
                    dr[2]=d1.ToString("N");
                    dr[3]=d2.ToString("N");
                    dr[4]=d3.ToString("N");
                    dr[5]=d4.ToString("N");
                    dtRep.Rows.Add(dr);
                }
            }
            lblAux.Text="";
            dg.DataSource=dtRep;
            dg.DataBind();
            string [] pr=new string[2];
            pr[1]=month.SelectedItem.ToString();
            pr[0]=year.SelectedValue.ToString();
	        Press frontEnd = new Press(ds, "Terceros por Cuenta");
		    frontEnd.PreHeader(tabPreHeader, dg.Width, pr);
		    frontEnd.Firmas(tabFirmas,dg.Width);
            StringBuilder SB=new StringBuilder();
            StringWriter SW=new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            tabPreHeader.RenderControl(htmlTW);
            dg.RenderControl(htmlTW);
            tabFirmas.RenderControl(htmlTW);
            string strRep;
            strRep=SB.ToString();
            Session.Clear();
            Session["Rep"]=strRep;
            toolsHolder.Visible = true;
        }
        else{
            lblAux.Text="No se encontraron datos.";
        }
	}
	public void SendMail(Object Sender, ImageClickEventArgs E){
        tabFirmas.Visible=false;
     	Consulta_Click(Sender, E);
        Utils.MostrarAlerta(Response, Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, dg));
        tabFirmas.Visible=true;
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
