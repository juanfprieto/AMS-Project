using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Web.Mail;
using AMS.DB;
using AMS.Forms;

namespace AMS.DBManager
{

 public partial class Reporte : System.Web.UI.Page{
	protected int i, j;
  	protected string table,like,order,filtro;
  	protected Label  lblResult;
 	protected TextBox txtMail;
 	protected DataGrid dgTable;
 	protected DataColumn[] primaryKeys;
 	protected OdbcDataReader dr;
 	protected DataSet ds = new DataSet();
    protected DataSet ds1 = new DataSet();
    protected Table tabPreHeader;
    
 	protected void Page_Load(object sender, EventArgs e){
	    //dgTable.DataSource=((string)Session["Rep"]).Copy();
	    string rep;
	    if(Session["Rep"]!=null)
        {
	       rep="<table align='center' style='BACKGROUND-COLOR: white'><tr><td>";
	       rep+=(string)Session["Rep"];
	       rep+="</td></tr><table>";
	       lbInfo.Text =rep;
	    }
	    else
        {
	       lbInfo.Text ="No existe reporte, vuelva a generarlo.";
	    //dgTable.DataBind();
	    //lbInfo.Text +="sdfdssf"+Session["Rep"].Copy();

/*		if(Request.QueryString["table"] != null){
			if(!IsPostBack){
				PrepareDataSource();
				LoadData();
			}*/
		}
	}
/*	protected void LoadData(){
		try{
		    string reportTitle;
		    string [] pr={" "," "};
            dgTable.DataSource = ds.Tables[0];
            reportTitle="Reporte de "+Request.QueryString["path"];
            Press frontEnd = new Press(ds, reportTitle);//Tabla para impresion del reporte
	        frontEnd.PreHeader(tabPreHeader, dgTable.Width, pr);//Encabezado del reporte
	        tabPreHeader.DataBind();
			dgTable.DataBind();
		}
		catch(Exception w){
			lbInfo.Text += w.ToString();
		}
        lblResult.Text=dgTable.Items.Count.ToString()+" filas encontradas.";
    }
	protected void PrepareDataSource(){
		string pkDatas = "",SqlN="";
		ArrayList atrs=new ArrayList();
		ArrayList eatrs=new ArrayList();
		ArrayList ftabs=new ArrayList();
		table = Request.QueryString["table"];
		order=Request.QueryString["order"];
		filtro=Request.QueryString["filt"];
		like=Request.QueryString["like"];
		like=like.Replace("*","%");

		if(like.Length==0)like="%";
        ds1 = DBFunctions.Request(ds1, IncludeSchema.NO,
                                "SELECT remarks, name, coltype, length, nulls, scale, default, generated, colno FROM sysibm.syscolumns WHERE tbname='"+table+"' order by colno"+
                                 "; SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='" +table+
		                         "'; SELECT * FROM "+table);
        for(i=0; i<ds1.Tables[1].Rows.Count;i++){
            atrs.Add(ds1.Tables[1].Rows[i].ItemArray[0].ToString().Trim());
            eatrs.Add(ds1.Tables[1].Rows[i].ItemArray[2].ToString().Trim());
            ftabs.Add(ds1.Tables[1].Rows[i].ItemArray[1].ToString().Trim());
        }
		SqlN+="SELECT ";  //Campos
		int c;
        for(i=0; i<ds1.Tables[0].Rows.Count; i++){
		  if(atrs.Contains(ds1.Tables[0].Rows[i].ItemArray[1].ToString().Trim())){
		      c=atrs.IndexOf(ds1.Tables[0].Rows[i].ItemArray[1].ToString().Trim());
		      DataSet ds2 = new DataSet();
		      ds2 = DBFunctions.Request(ds2, IncludeSchema.NO, "SELECT * FROM "+ftabs[c]);
		      //SqlN+=ftabs[c]+"."+ds2.Tables[0].Columns[1].ColumnName.ToString()+", ";
		      SqlN+="tab"+c.ToString()+"."+ds2.Tables[0].Columns[1].ColumnName.ToString()+", ";
		  }
		  else
		      SqlN+=table+"."+ds1.Tables[0].Rows[i].ItemArray[1].ToString().Trim() +", ";
		}
		SqlN=SqlN.Substring(0,SqlN.Length-2);
		SqlN+=" FROM ";
		for(i=0; i<ds1.Tables[1].Rows.Count; i++)//Tableas ext
			SqlN+=ds1.Tables[1].Rows[i].ItemArray[1]+" AS tab"+i.ToString()+", ";
        SqlN+=table;
        SqlN+=" WHERE ";
        for(i=0; i<ds1.Tables[1].Rows.Count; i++)//Tableas ext
            SqlN+= "tab"+i.ToString()+"."+eatrs[i]+"="+table+"."+atrs[i]+" AND ";
        if(filtro.Length>0){
            if(ds1.Tables[2].Columns[filtro].DataType.ToString() == "System.String"){
                SqlN+="ucase(" +table+"."+filtro+ ") ";
                if(like.IndexOf("%")>=0)
                    SqlN+="LIKE ('"+like.ToUpper()+"') ";
                else
                    SqlN+="='"+like.ToUpper()+"' ";
            }
        }
        else{
            SqlN=SqlN.Substring(0,SqlN.Length-4);
            if(eatrs.Count==0)
                SqlN=SqlN.Substring(0,SqlN.Length-6);
        }
        if(order.Length>0)
            SqlN+=" ORDER BY "+table+"."+order+";";
        lbInfo.Text+=SqlN+"<br>";
		try{
	        ds = DBFunctions.Request(ds, IncludeSchema.YES, SqlN);
			primaryKeys = ds.Tables[0].PrimaryKey;
			if(ds.Tables[0].PrimaryKey.Length == 0){
				DataColumn[] dCol = new DataColumn[ds.Tables[0].Columns.Count];
				for(i=0; i<ds.Tables[0].Columns.Count; i++)
					dCol[i] = ds.Tables[0].Columns[i];
				ds.Tables[0].PrimaryKey = dCol;
			}
			if(ds.Tables[0].PrimaryKey.Length == 1)
				dgTable.DataKeyField = ds.Tables[0].PrimaryKey[0].ToString();
			if(ds.Tables[0].PrimaryKey.Length > 1){
				DataColumn dc = new DataColumn("primaryKey", typeof(string));
				for(i=0; i<ds.Tables[0].PrimaryKey.Length; i++){
					pkDatas += ds.Tables[0].PrimaryKey[i];
					if(i != ds.Tables[0].PrimaryKey.Length-1)
						pkDatas += " + '|' + ";
				}
				dc.Expression = pkDatas;
				ds.Tables[0].Columns.Add(dc);
				dgTable.DataKeyField = "primaryKey";
			}
			dgTable.AutoGenerateColumns=false;
			BuildDataGridColumns();
		}
		catch(Exception e){
			lbInfo.Text += e.ToString();
			lbInfo.Text += DBFunctions.exceptions;
		}
	}

	protected void BuildDataGridColumns()
	{
		string names="";
		for(i=0; i<ds1.Tables[2].Columns.Count; i++)
		{
			BoundColumn bc = new BoundColumn();
			bc.DataField = ds.Tables[0].Columns[i].ColumnName;
			if(ds.Tables[0].Columns[i].DataType.ToString() == "System.DateTime")
				bc.DataFormatString = "{0:d/M/yyyy}";
			names = DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSCOLUMNS WHERE tbname='" + table + "' AND name='" +ds1.Tables[0].Rows[i].ItemArray[1].ToString().Trim()+ "'");
			if(names != null)
				bc.HeaderText = names;
			else
				bc.HeaderText = ds.Tables[0].Columns[i].ColumnName;

			if(ds.Tables[0].Columns[i].ColumnName == "primaryKey")
				bc.Visible = false;

			dgTable.Columns.Add(bc);

		}
	}

	protected void DgTable_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
    }
	public void EnviaMail(Object sender, EventArgs e){
	   //PrepareDataSource();
	   //LoadData();
       //reportTitle="Reporte de "+Request.QueryString["path"];
       //lbInfo.Text+=Press.PressOnEmail("Reporte de "+Request.QueryString["path"], txtMail.Text, tabPreHeader, dgTable);
     	MailMessage MyMail = new MailMessage();
     	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
		MyMail.To = txtMail.Text;
     	MyMail.Subject = "Reporte de "+Request.QueryString["path"];
        MyMail.Body=(string)Session["Rep"];
      	MyMail.BodyFormat = MailFormat.Html;
      	try{
    	   SmtpMail.Send(MyMail);}
    	catch{
    	   lbInfo.Text +="<br>"+"No se pudo enviar el reporte a " + MyMail.To+"<br>";
    	   return;
    	   //return "No se pudo enviar el reporte a " + MyMail.To;
    	}
    	lbInfo.Text +="<br>"+"Reporte enviado a " + MyMail.To+"<br>";
       	//return "Reporte enviado a " + MyMail.To;
	}*/
   

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
