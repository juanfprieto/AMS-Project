using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
//using AMS.Inventarios;

namespace AMS.DBManager
{
	public partial class Preview : System.Web.UI.UserControl
	{
		protected DataSet ds = new DataSet();
		protected Table tbForm = new Table();
		protected Table tbFormHeader = new Table();
		protected Table tbFormFooter = new Table();
		protected string table;
		protected string pks;
		protected int i,j;
		protected DataColumn[] primaryKeys;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			hlTableView.NavigateUrl = "../aspx/AMS.Web.Index.aspx"+ "?process=DBManager.Selects&table=" + Request.QueryString["table"] + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&path=" + Request.QueryString["path"] + "";
			table = Request.QueryString["table"];
			pks = Request.QueryString["pks"].Replace("*","'");
			//tbForm.CssClass = "main";
			//if(Request.QueryString["table"] != null && !IsPostBack)
			//{
				Session.Clear();
				tbForm.Width = new Unit(600);
				tbForm.BackColor = Color.FromArgb(240,240,240);
				tbForm.Font.Name = "arial";
				tbFormHeader.Width = new Unit(600);
				tbFormHeader.BackColor = Color.FromArgb(240,240,240);
				tbFormHeader.Font.Name = "arial";
				tbFormFooter.Width = new Unit(600);
				tbFormFooter.BackColor = Color.FromArgb(240,240,240);
				tbFormFooter.Font.Name = "arial";
				GetSchema();
				SetHeader();
				SetForm();
				SetFooter();
				Session["rep"] = RenderHtml();
			//}
		}
		
		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
        	StringWriter SW= new StringWriter(SB);
        	HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
        	phForm.RenderControl(htmlTW);
        	return SB.ToString();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			MailMessage MyMail = new MailMessage();
   		  	MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
     		MyMail.To = tbEmail.Text;
            //MyMail.Subject = "Proceso : "+DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSTABLES WHERE name='"+table+"'");
            MyMail.Subject = "Proceso : " + DBFunctions.SingleData(SQLParser.GetTableNameDescription(table) );
            MyMail.Body = (RenderHtml());
      		MyMail.BodyFormat = MailFormat.Html;
			try{
    	   		//SmtpMail.SmtpServer="ecasltda.com";
      			SmtpMail.Send(MyMail);}
    		catch(Exception e){
    	 	  lbInfo.Text = e.ToString();
    		}
		}
		
		protected void GetSchema()
		{
			//ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" +
		 //   	table + "'; SELECT fkcolnames, reftbname, pkcolnames FROM sysibm.sysrels WHERE tbname='" +
		 //       table + "'");
            ds = DBFunctions.Request(ds, IncludeSchema.NO, SQLParser.GetTableStructureExtended(table) + 
                                                ";" + SQLParser.GetTableForeign(table) );

            ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " FETCH FIRST 1 ROWS ONLY ");
		}
		
		protected void SetForm()
		{
			for(i=0; i<ds.Tables[ds.Tables.Count-1].Columns.Count; i++)
			{
				DataRow[] scRow = ds.Tables[0].Select("NAME = '" + ds.Tables[ds.Tables.Count-1].Columns[i].ColumnName + "'");
				if(scRow.Length > 0)
				{
					DataRow[] fkRow = ds.Tables[1].Select("FKCOLNAMES = ' " + scRow[0][1].ToString() + "'");
					if(fkRow.Length > 0)
						MapFKType(scRow[0], fkRow[0]);
					else
						MapType(scRow[0]);
				}
			}
			phForm.Controls.Add(tbForm);
		}
		
		protected void MapFKType(DataRow dr, DataRow drFK)
		{
			Label lb = PutLabelTitle(dr[0].ToString());
			Label lbD = PutLabelValueFK(drFK);
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(lbD);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
		}
		
		protected Label PutLabelTitle(string text)
		{
			Label lb = new Label();
			lb.ID = "label_" + i.ToString();
			lb.Text = text + ": ";
			return lb;
		}
		
		protected void MapType(DataRow dr)
		{
			Label lb = PutLabelTitle(dr[0].ToString());
			Label lbD = PutLabelValue(dr[1].ToString());
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(lbD);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
		}
		
		protected Label PutLabelValue(string nameCol)
		{
			Label lb = new Label();
			lb.ID = "labelVl_" + i.ToString();
			lb.Text = DBFunctions.SingleData("SELECT "+nameCol+" FROM "+table+" WHERE "+pks+"");
			return lb;
		}
		
		protected Label PutLabelValueFK(DataRow drFK)
		{
			DataSet ds1 = new DataSet();
			Label lb = new Label();
			lb.ID = "labelVl_" + i.ToString();
			if(drFK[1].ToString()!="MNIT")
			{
				ds1 = DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT * FROM "+drFK[1].ToString()+" WHERE "+drFK[2].ToString()+"=(SELECT "+drFK[0].ToString()+" FROM "+table+" WHERE "+pks+")");
				if(ds1.Tables[0].Rows.Count==0)
					lb.Text="";
				else
					lb.Text = ds1.Tables[0].Rows[0][1].ToString();
			}
			else
			{
				ds1 = DBFunctions.Request(ds1,IncludeSchema.NO,"SELECT mnit_nit CONCAT '-' CONCAT mnit_apellidos CONCAT '-' CONCAT mnit_nombres FROM "+drFK[1].ToString()+" WHERE "+drFK[2].ToString()+"=(SELECT "+drFK[0].ToString()+" FROM "+table+" WHERE "+pks+")");
				lb.Text = ds1.Tables[0].Rows[0][0].ToString();
			}
			return lb;
		}
		
		protected void SetHeader()
		{
			int numRows = 2;
			int numCells = 3;
            //string logo=DBFunctions.SingleData("SELECT CEMP_LOGO FROM CEMPRESA;");
            string logo = DBFunctions.SingleDataGlobal("SELECT GEMP_iCONO FROM GEMPRESA WHERE GEMP_NOMBRE='" + GlobalData.getEMPRESA() + "';");
            for (int k=0;k<numRows;k++)
			{
				TableRow r = new TableRow();
				for(int m=0;m<numCells;m++)
				{
					TableCell c = new TableCell();
					if(k == 0 && m == 0){
						c.VerticalAlign=VerticalAlign.Top;
						if(logo.Length>0)
							//c.Text = "<IMG src=\"../rpt/"+logo+"\" border=\"0\">";
                            c.Text = "<IMG src=\"" + logo + "\" border=\"0\">";
						else
							c.Text = "<p>&nbsp;</p>";
					}
					if(k == 0 && m == 1)
					{
						c.Text = "<center>"+DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa")+"</center>";
						c.Text+= "<center>NIT: "+DBFunctions.SingleData("SELECT mnit_nit FROM cempresa")+"</center>";
						c.Text+= "<center>"+DBFunctions.SingleData("SELECT mn.mnit_direccion FROM cempresa ce, mnit mn where ce.mnit_nit=mn.mnit_nit;")+"</center>";
						c.Text+= "<center>Ciudad: "+DBFunctions.SingleData("SELECT pc.pciu_nombre FROM cempresa ce, pciudad pc where ce.cemp_ciudad=pc.pciu_codigo;")+"</center>";
						c.Text+= "<center>PBX: "+DBFunctions.SingleData("SELECT mn.mnit_telefono FROM cempresa ce, mnit mn where ce.mnit_nit=mn.mnit_nit;")+"</center>";
					}
                    if(k == 0 && m == 2)
                        c.Text = "<div style='text-align:right'>SC-PRO-001</div><p><div style='text-align:right'>DEFINICION DEL PROCESO</div></p>";                   
                    if(k == 1 && m == 0)
						c.Text = "<p>&nbsp;</p>";
					if(k == 1 && m == 1)
                        c.Text = "<center>" + DBFunctions.SingleData(SQLParser.GetTableNameDescription(table) ) + "</center>"; 
                        //c.Text = "<center>"+DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSTABLES WHERE name='"+table+"'")+"</center>";
					if(k == 1 && m == 2)
						c.Text = "<div style='text-align:right'>Procesado En : "+DateTime.Now.ToShortDateString()+"</div>";
					c.Width = new Unit("33%");
					r.Cells.Add(c);
				}
				tbFormHeader.Rows.Add(r);
			}
			phForm.Controls.Add(tbFormHeader);
		}
		
		protected void SetFooter()
		{
			TableRow r = new TableRow();
			TableCell c1 = new TableCell();
			TableCell c2 = new TableCell();
			c1.Text = "<br><br><br><br><center>Procesado Por : ____________________________</center>";
			c2.Text = "<br><br><br><br><center>Revisado Por : ____________________________</center>";
			c1.Width = new Unit("50%");
			r.Cells.Add(c1);
			r.Cells.Add(c2);
			tbFormFooter.Rows.Add(r);
			phForm.Controls.Add(tbFormFooter);
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
