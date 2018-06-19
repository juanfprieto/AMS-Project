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
using System.Data.Odbc;
using AMS.Tools;

namespace AMS.Contabilidad{
public partial class LibCue : System.Web.UI.UserControl{
	protected string reportTitle="Libro de Cuenta y Razon";	
	protected void Page_Load(object sender, System.EventArgs e){
		if(!IsPostBack){
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList( year, "SELECT pano_ano FROM pano");
			bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13");
		}
	}

	protected void Report_ItemDataBound(object sender, DataGridItemEventArgs e){
 		if(e.Item.ItemType != ListItemType.Header){
 			e.Item.Cells[6].HorizontalAlign = HorizontalAlign.Right;
 		}
 	}

	protected void Consulta_Click(object  Sender ,  EventArgs e){
        DataSet ds=new DataSet();
        string []pr=new string[2];
		pr[0]=pr[1]="";
		Press frontEnd = new Press(new DataSet(), reportTitle);
		frontEnd.PreHeader(tabPreHeader, dg.Width, pr);
		frontEnd.Firmas(tabFirmas,dg.Width);
	    string sql="select distinct d1.pdoc_codigo,d1.mcom_numedocu, d1.mcue_codipuc, d1.dcue_codirefe, d1.dcue_numerefe, d1.mnit_nit, d1.palm_almacen, d1.pcen_codigo, d1.dcue_detalle, d1.dcue_valodebi, d1.dcue_valocred, d1.dcue_valobase, m3.mcue_nombre,m3.tnat_codigo from mcomprobante m1, dcuenta d1, mauxiliarcuenta m2, mcuenta m3 where m3.mcue_codipuc=d1.mcue_codipuc AND m1.PDOC_CODIGO = d1.PDOC_CODIGO and m1.mcom_numedocu=d1.mcom_numedocu and m1.pmes_mes="+month.SelectedValue+" AND m1.pano_ano="+year.SelectedValue+" ORDER BY d1.mcue_codipuc";
	                               //          0           1            2                   3                  4               5               6           7                   8               9                   10              11              12              13
        string sql1 = "Select MNIT_NIT, MNIT_NOMBRES || ' ' || coalesce(MNIT_NOMBRE2,''), MNIT_APELLIDOS || ' ' || coalesce(MNIT_APELLIDO2,'') from MNIT";
        CultureInfo bz = new CultureInfo("en-us");
        bool totTer=false;
		if (RadioOpcion.SelectedValue=="DOC")
            sql+=", d1.pdoc_codigo";
		if (RadioOpcion.SelectedValue=="NIT"){
            sql+=", d1.mnit_nit";
            totTer=true;}
        if (RadioOpcion.SelectedValue=="NOM"){
            sql+=", d1.mnit_nit";
            totTer=true;}
		if (RadioOpcion.SelectedValue=="SCC")
            sql+=", d1.palm_almacen, d1.pcen_codigo";
		if (RadioOpcion.SelectedValue=="CRE")
            sql+=", d1.dcue_valocred, d1.dcue_valodebi";
		if (RadioOpcion.SelectedValue=="DEB")
            sql+=", d1.dcue_valodebi, d1.dcue_valocred";
		string sql2="select m1.mcue_codipuc as mcue_codipuc, CASE WHEN m2.tnat_codigo='D' THEN sum(m1.maux_valodebi-m1.maux_valocred) WHEN m2.tnat_codigo='C' THEN sum(m1.maux_valocred-m1.maux_valodebi) END as DETALLE from mauxiliarcuenta as m1, mcuenta as m2 where m1.pano_ano="+year.SelectedValue+" and m1.pmes_mes<"+month.SelectedValue+" and m1.mcue_codipuc=m2.mcue_codipuc group by (m1.mcue_codipuc,m2.tnat_codigo)";    //Saldo anterior meses pasados
		//lblAux.Text=sql1;
        DBFunctions.Request(ds, IncludeSchema.NO,sql);
        DBFunctions.Request(ds, IncludeSchema.NO,sql1);
        DBFunctions.Request(ds, IncludeSchema.NO,sql2);
        string[] titleFields={"Doc-Comp","Cuenta","Nit","Cod-Num Ref.", "Sede-CC","Detalle","Debito","Credito","Base"};
        string[] dataFields={"Doc-Comp","Cuenta","Nit", "Cod-Num Ref.", "Sede-CC","Detalle","Debito","Credito","Base"};
        int i;
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
        DataRow[] rows;
        string uC="",uT="";
        string nomter="";
        double trCCT,trCDT,trCC,trCD,trC,trD;
        DataView dv=ds.Tables[1].DefaultView;
        trCCT=trCDT=trCC=trCD=trC=trD=0;
        for(i=0;i<titleFields.Length;i++)
            dtRep.Columns.Add(new DataColumn(dataFields[i],typeof(string)));
        if(ds.Tables[0].Rows.Count>0)
        {
            for(i=0;i<ds.Tables[0].Rows.Count;i++)
            {
                if(uC!=ds.Tables[0].Rows[i].ItemArray[2].ToString())    //Cuenta diferente
                {
                    dr=dtRep.NewRow();dr[0]="<br>";dtRep.Rows.Add(dr);  //Titulo
                    dr=dtRep.NewRow();dr[0]=ds.Tables[0].Rows[i].ItemArray[2].ToString();dr[1]=ds.Tables[0].Rows[i].ItemArray[12].ToString();
                    rows=ds.Tables[2].Select("MCUE_CODIPUC='"+ds.Tables[0].Rows[i].ItemArray[2].ToString()+"'");
                    dr[5]="Saldo Ant:";
                    if(ds.Tables[0].Rows[i].ItemArray[13].ToString().ToUpper()=="D")    //Saldo anterior
                        if(rows.Length>0)dr[6]=((double)Convert.ToDouble(rows[0][1])).ToString("N");
                        else dr[6]="0.00";
                    else
                        if(rows.Length>0)dr[7]=((double)Convert.ToDouble(rows[0][1])).ToString("N");
                        else dr[7]="0.00";
                    uC=ds.Tables[0].Rows[i].ItemArray[2].ToString();
                    trCC=trCD=0;
                    dtRep.Rows.Add(dr);
                    uT="";
                }
                rows=ds.Tables[1].Select("MNIT_NIT='"+ds.Tables[0].Rows[i].ItemArray[5].ToString()+"'");    //Nombre del tercero
                if(rows.Length>0)
                    nomter=rows[0][1]+" "+rows[0][2];
                else{
                    nomter="No existe.";
                    uT="no";
                    trC=trD=0;}
                if(rows.Length>0&&totTer){   //Titulo terceros
                    if(uT==""||uT!=nomter){
                        dr=dtRep.NewRow();
                        rows=ds.Tables[2].Select("MCUE_CODIPUC='"+ds.Tables[0].Rows[i].ItemArray[2].ToString()+"'");
                        if(ds.Tables[0].Rows[i].ItemArray[13].ToString().ToUpper()=="D"){
                            if(rows.Length>0)
                                dr[6]=((double)Convert.ToDouble(rows[0][1])).ToString("N");
                            else
                                dr[6]="0.00";}
                        else{
                            if(rows.Length>0)
                                dr[7]=((double)Convert.ToDouble(rows[0][1])).ToString("N");
                            else
                                dr[7]="0.00";}
                        dr[0]=ds.Tables[0].Rows[i].ItemArray[5].ToString();
                        dr[1]=nomter;
                        uT=nomter;
                        dr[5]="Saldo A Ter:";
                        dtRep.Rows.Add(dr);
                    }
                }
                dr=dtRep.NewRow();
                dr[1]=nomter;
                dr[0]=ds.Tables[0].Rows[i].ItemArray[0].ToString()+"-"+ds.Tables[0].Rows[i].ItemArray[1].ToString();
                dr[3]=ds.Tables[0].Rows[i].ItemArray[3].ToString()+"-"+ds.Tables[0].Rows[i].ItemArray[4].ToString();
                dr[2]=ds.Tables[0].Rows[i].ItemArray[5].ToString();
                dr[4]=ds.Tables[0].Rows[i].ItemArray[6].ToString()+"-"+ds.Tables[0].Rows[i].ItemArray[7].ToString();
                dr[5]=ds.Tables[0].Rows[i].ItemArray[8].ToString();
                dr[6]=((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[9])).ToString("N");
                dr[7]=((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[10])).ToString("N");
                dr[8]=((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[11])).ToString("N");
                trCD+=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[9]);
                trCC+=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[10]);
                dtRep.Rows.Add(dr);
                if(totTer && uT!="no")//Total terceros
                {
                    if(i==ds.Tables[0].Rows.Count-1||ds.Tables[0].Rows[i].ItemArray[5].ToString()!=ds.Tables[0].Rows[i+1].ItemArray[5].ToString()||ds.Tables[0].Rows[i].ItemArray[2].ToString()!=ds.Tables[0].Rows[i+1].ItemArray[2].ToString())
                    {
                        dr=dtRep.NewRow();
                        dr[5]="Total Ter:";
                        dtRep.Rows.Add(dr);
                        uT=nomter;
                        trC+=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[10]);
                        trD+=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[9]);
                        dr[6]=(trD).ToString("N");
                        dr[7]=(trC).ToString("N");
                        trC=trD=0;
                    }
                    else
                    {
                        trC+=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[10]);
                        trD+=Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[9]);
                    }
                }
                if(i==ds.Tables[0].Rows.Count-1||ds.Tables[0].Rows[i].ItemArray[2].ToString()!=ds.Tables[0].Rows[i+1].ItemArray[2].ToString())
                {
                    dr=dtRep.NewRow();
                    dr[0]="Total:";
                    dr[1]=ds.Tables[0].Rows[i].ItemArray[12].ToString();
                    dr[5]="Movimiento:";
                    dr[6]=(trCD).ToString("N");
                    dr[7]=(trCC).ToString("N");
                    dtRep.Rows.Add(dr);
                    dr=dtRep.NewRow();
                    dr[5]="Saldo:";
                    if(ds.Tables[0].Rows[i].ItemArray[13].ToString().ToUpper()=="D")dr[6]=(trCD-trCC).ToString("N");
                    else dr[7]=(trCC-trCD).ToString("N");
                    trCCT+=trCC;
                    trCDT+=trCD;
                    trCC=trCD=0;
                    dtRep.Rows.Add(dr);
                }
            }
            dr=dtRep.NewRow();
            dr[0]="<br>";
            dtRep.Rows.Add(dr);
            dr=dtRep.NewRow();
            dr[0]="TOTAL:";
            dr[6]=(trCDT).ToString("N");
            dr[7]=(trCCT).ToString("N");
            if(Math.Abs(trCDT-trCCT)<0.01)
                dr[1]="Sumas Iguales";
            else{
                dr[1]="Sumas Desiguales";
                dr[2]="Diferencia:";
                dr[3]=Math.Abs(trCC-trCD).ToString("N");
            }
            dtRep.Rows.Add(dr);
            lblAux.Text="";
            dg.DataSource=dtRep;
            dg.DataBind();
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
			toolsHolder.Visible=true;        	
        }
        else{
            lblAux.Text="No se encontraron datos.";
        }
	}
	public void SendMail(Object Sender, ImageClickEventArgs E){
     	Consulta_Click(Sender, E);
        Utils.MostrarAlerta(Response, Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, dg));
		dg.EnableViewState=false;
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
