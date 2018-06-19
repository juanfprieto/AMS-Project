// created on 20/02/2004 at 12:48
using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using System.Threading;
using AMS.Tools;

namespace AMS.Web
{
	
	public partial class ModalDialogIns: System.Web.UI.Page
	{
		protected int i,j, fkCount=0;
		protected string table;
		protected string fksComps = "", ddlSec = "";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected string sels="";
		protected string images = ConfigurationManager.AppSettings["PathToImages"];
		protected string sqlQuery;
		protected Label  lbProcessTitle;
		protected Button  btUpdate;
		protected Table tbForm = new Table();
		protected DataSet ds = new DataSet();
		protected DataSet ds1 = new DataSet();
		protected TextBox tbRefItem = new TextBox();
		protected DropDownList ddlRefItem = new DropDownList();
		protected Label lbBusqueda=new Label();
		protected System.Web.UI.WebControls.Image img=new System.Web.UI.WebControls.Image();
		protected System.Web.UI.WebControls.LinkButton lnkVerTabla;
		protected System.Web.UI.WebControls.Image imghlp=new System.Web.UI.WebControls.Image();
		
		protected void Page_Load(object sender, EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(ModalDialogIns));
            table = Request.QueryString["table"];
            //table = Request.QueryString["table"].Replace((ConfigurationManager.AppSettings["Schema"]+".").ToUpper(),"").Replace((ConfigurationManager.AppSettings["Schema"]+".").ToLower(),"").ToUpper();
			sqlQuery = Request.QueryString["sql"];

            if (Request.QueryString["modal"] == "2" && Request.QueryString["Ins"] == "1" && Request.QueryString["Vals"] != null)
                //bla;
                return;
            else
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + table + "&action=insert&sql=" + sqlQuery + "&cod=" + Request.QueryString["cod"] + "&processTitle=INSERTAR&path=" + Request.QueryString["path"] + "&modal=2&campos=" + Request.QueryString["campos"]);


            //if(!IsPostBack)
            //{
            //    //if(Request.QueryString["imp"]=="Y")
            //    //	Response.Write("<script language='javascript' src='../js/AMS.Web.DialogBox.js' type='text/javascript'></script><script language:javascript>DialogBox('Desea Imprimir?','DBManager.Preview&table="+table+"&pks="+Request.QueryString["pksImp"]+"');</script>");
            //    tbForm.Width = new Unit(500);
            //    tbForm.CssClass = "filtersInAuto";
            //    //tbForm.BackColor = Color.FromArgb(183, 183, 226);
            //    tbForm.Font.Name = "arial";
            //    GetSchema();
            //    SetForm();
            //}
		}
		
		protected void VerTabla(Object sender, EventArgs e)
		{
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
			Response.Redirect(indexPage + "?process=DBManager.Selects&table="+table);
		}
		
		protected void GetSchema()
		{
			ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" +
				table + "'; SELECT fkcolnames, reftbname, pkcolnames, relname, refkeyname FROM sysibm.sysrels WHERE tbname='" +
				table + "'");
			ds1 = DBFunctions.Request(ds1, IncludeSchema.NO,"SELECT constname, colname, colseq FROM sysibm.syskeycoluse WHERE tbname='"+table+"' AND constname NOT IN(SELECT name FROM sysibm.sysindexes WHERE tbname='"+table+"')");
			if( Request.QueryString["action"] == "update" )
			{
				string pks = Request.QueryString["pks"];
				ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " WHERE " + pks);
			}
			if( Request.QueryString["action"] == "insert" )
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
					DataRow[] fkRowComposite = ds1.Tables[0].Select("COLNAME = '" + scRow[0][1].ToString() + "'");
					if( (fkRow.Length > 0) && (Convert.ToInt32(fkRowComposite[0][2]) == 1))
						MapFKType(scRow[0], fkRow[0]);
					else if(fkRowComposite.Length > 0)
					{
						DataRow[] fkRow2 = ds.Tables[1].Select("RELNAME = '" + fkRowComposite[0][0].ToString().Trim() + "'");
						if(fkRow2.Length>0)
							MapFKType(scRow[0], fkRow2[0], fkRowComposite[0]);
					}
					else
					{
						if(scRow[0][2].ToString().IndexOf("CHAR") != -1 || scRow[0][2].ToString().IndexOf("VAR") != -1)
							MapStringType(scRow[0]);
						if(scRow[0][2].ToString().IndexOf("INT") != -1)
							MapIntType(scRow[0]);
						if(scRow[0][2].ToString().IndexOf("DECIMAL") != -1 || scRow[0][2].ToString().IndexOf("DOUBLE") != -1)
							MapDecimalType(scRow[0]);
						if(scRow[0][2].ToString().IndexOf("DATE") != -1)
							MapDateType(scRow[0]);
						if(scRow[0][2].ToString().Trim()=="TIME")
							MapTimeType(scRow[0]);
						if(scRow[0][2].ToString().Trim()=="TIMESTMP")
							MapTimeStampType(scRow[0]);
					}
				}
			}
			AddValidationSummary();
			phForm.Controls.Add(tbForm);
			//Manejo de las referencias de repuestos
            if (table == "MITEMS")
            {
                tbRefItem.Attributes.Add("onkeyup", "ItemMask(" + tbRefItem.ClientID + "," + ddlRefItem.ClientID + ");");
                ddlRefItem.Attributes.Add("onchange", "ChangeLine(" + ddlRefItem.ClientID + "," + tbRefItem.ClientID + ");");
            }
            else if (table == "MNIT")
            {
                //((TextBox)tbForm.FindControl("control_0")).Attributes.Add("onblur", "digitoVerificacion(this," + ((TextBox)tbForm.FindControl("control_1")).ClientID + ")");
               
                ((TextBox)tbForm.FindControl("control_0")).Text = Request.QueryString["txtInsert"];
                ((TextBox)tbForm.FindControl("control_0")).Attributes.Add("onblur", "Cargar_Nombre(this)");

                ((TextBox)tbForm.FindControl("control_2")).Attributes.Add("onblur", "aMayusculas(this)");
                ((TextBox)tbForm.FindControl("control_3")).Attributes.Add("onblur", "aMayusculas(this)");
                ((TextBox)tbForm.FindControl("control_4")).Attributes.Add("onblur", "aMayusculas(this)");
                ((TextBox)tbForm.FindControl("control_5")).Attributes.Add("onblur", "aMayusculas(this)");

                tbRefItem.ReadOnly = true;
                tbRefItem.Attributes.Add("onclick", "WizardDirection(this);");
            }
            else if (table == "CEMPRESA" || table == "MEMPLEADO" || table == "MNOMINACAPITAL" || table == "MNOMINAFAMILIA" || table == "PALMACEN")
            {
                tbRefItem.ReadOnly = true;
                tbRefItem.Attributes.Add("onclick", "WizardDirection(this);");
            }
		}

        [Ajax.AjaxMethod]
        public string Cargar_Nombre(string nit)
        {

            string n = DBFunctions.SingleData(String.Format("select mnit_nit from dbxschema.mnit where mnit_nit='" + nit + "'"));

            return n;

        }

		
		protected void AddValidationSummary()
		{
			ValidationSummary sum = new ValidationSummary();
			sum.ShowMessageBox = true;
			sum.ShowSummary = false;
			sum.HeaderText = "Existe Algún Problema con los siguientes campos :";
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(sum);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tbForm.Rows.Add(tr);
		}
		
		protected void MapTimeType(DataRow dr)
		{
			Label lb = PutLabel(dr[0].ToString());
			TextBox tb = PutTextBox(8);
			tb.Text = DateTime.Now.TimeOfDay.ToString().Substring(0,8);
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}:[0-9]{2}:[0-9]{2}",dr[0].ToString());
			System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img2);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
		}

		protected void MapTimeStampType(DataRow dr)
		{
			Label lb = PutLabel(dr[0].ToString());
			TextBox tb = PutTextBox(25);
			tb.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}:[0-9]{2}:[0-9]{2} [0-9]{2}:[0-9]{2}:[0-9]{2}",dr[0].ToString());
			System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img2);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
		}
		
		protected void MapStringType(DataRow dr)
		{
			int length = Convert.ToInt32(dr[3]);
			RequiredFieldValidator rfv;
			Label lb= PutLabel(dr[0].ToString());
			TextBox tb = PutTextBox(length);
			tb.MaxLength = length;
			System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			if(dr[4].ToString() == "N")
			{
				rfv = RequiredValidator(tb.ID,dr[0].ToString());
				tc2.Controls.Add(rfv);
			}
			if(dr[6].ToString() != "")
			{
				tb = PutTextBox(7);
				tb.Text = "DEFAULT";
				tb.ReadOnly = true;
			}
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img2);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
			if(table == "MITEMS" && dr[1].ToString() == "MITE_CODIGO")
				tbRefItem = tb;
			else if((table == "MNIT" && dr[1].ToString() == "MNIT_DIRECCION") || (table == "CEMPRESA" && dr[1].ToString() == "CEMP_DIRECCION") || (table == "MEMPLEADO" && dr[1].ToString() == "MEMP_DIRECCION") || (table == "MNOMINACAPITAL" && dr[1].ToString() == "MNOM_DIRECBIEN") || (table == "MNOMINAFAMILIA" && dr[1].ToString() == "MNOM_DIRECCION") || (table == "PALMACEN" && dr[1].ToString() == "PALM_DIRECCION") || (table == "PALMACEN" && dr[1].ToString() == "PALM_DIRECCION"))
				tbRefItem = tb;
		}
		
		protected void MapIntType(DataRow dr)
		{
			Label lb = PutLabel(dr[0].ToString());
			TextBox tb = PutTextBox(8);
			tb.MaxLength = 10;
			System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
			TableCell tc1 = new TableCell();
			TableCell tc2 = new TableCell();
			if(dr[7].ToString() == "A" || dr[7].ToString() == "D")
			{
				tb = PutTextBox(7);
				tb.Text = "DEFAULT";
				tb.ReadOnly = true;
				tc1.Controls.Add(lb);
				tc2.Controls.Add(tb);
			}
			else
			{
				RegularExpressionValidator rev = Validator(tb.ID, "[0-9]+",dr[0].ToString());
				tc1.Controls.Add(lb);
				tc2.Controls.Add(tb);
				tc2.Controls.Add(rev);
			}
			if(dr[4].ToString() == "N")
			{
				RequiredFieldValidator rfv = RequiredValidator(tb.ID,dr[0].ToString());
				tc2.Controls.Add(rfv);
			}
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img2);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tb.CssClass = "AlineacionDerecha";
			tbForm.Rows.Add(tr);
		}
		
		protected void MapDecimalType(DataRow dr)
		{
			Label lb = PutLabel(dr[0].ToString());
			TextBox tb = PutTextBox(20);
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9\\,\\.]+",dr[0].ToString());
			System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);
			tb.Attributes.Add("onkeyup","NumericMaskE(this,event)");
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rev);
			if(dr[4].ToString() == "N")
			{
				RequiredFieldValidator rfv = RequiredValidator(tb.ID,dr[0].ToString());
				tc2.Controls.Add(rfv);
			}
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img2);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tb.CssClass = "AlineacionDerecha";
			tbForm.Rows.Add(tr);
		}
		
		protected void MapDateType(DataRow dr)
		{
			Label lb = PutLabel(dr[0].ToString());
			TextBox tb = PutTextBox(20);
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{4}-[0-9]{2}-[0-9]{2}",dr[0].ToString());
			/*IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
			tb.Text = DateTime.Now.GetDateTimeFormats('d', culture)[0];*/
			tb.Attributes.Add("onkeyup","DateMask(this)");
			System.Web.UI.WebControls.Image img2 = this.PutImageHelp(dr);			
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			if(dr[4].ToString() == "N")
			{
				tb.Text = DateTime.Now.ToString("yyyy-MM-dd");
				RequiredFieldValidator rfv = RequiredValidator(tb.ID,dr[0].ToString());
				tc2.Controls.Add(rfv);
			}
			else
				tb.Text="";
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rev);
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img2);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
		}
		
		protected void MapFKType(DataRow dr, DataRow drFK)
		{
			Label lb = PutLabel(dr[0].ToString());
			DropDownList ddl = PutDropDownList(drFK[1].ToString());
			System.Web.UI.WebControls.Image img2 = PutImage(drFK[1].ToString());
			System.Web.UI.WebControls.Image img3 = this.PutImageHelp(dr);
			if(dr[4].ToString() == "Y")
			{
				ddl.Items.Add(new ListItem("No requiere","null"));
				ddl.SelectedIndex = ddl.Items.Count - 1;
			}
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(ddl);
			tc2.Controls.Add(img2);
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img3);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
			if(table == "MITEMS" && dr[1].ToString() == "PLIN_CODIGO")
				ddlRefItem = ddl;
			img=img2;
		}
		
		
		protected void MapFKType(DataRow dr, DataRow drFK, DataRow drFKComp)
		{
			Label lb = PutLabel(dr[0].ToString());
			DropDownList ddl = new DropDownList();
			System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
			System.Web.UI.WebControls.Image img3 = this.PutImageHelp(dr);
			if(Convert.ToInt32(drFKComp[2]) == 1)
			{
				ddl = PutDropDownList(drFK[1].ToString(),DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='"+drFK[4]+"' AND colseq="+drFKComp[2]+""),"");
				ddl.SelectedIndexChanged += new EventHandler(this.ChangedFK);
				ddl.AutoPostBack = true;
				fksComps +=	drFK[3]+"-"+ddl.ID+"-"+drFK[1]+"-"+DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='"+drFK[4]+"' AND colseq="+drFKComp[2]+"")+"-"+DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name=(SELECT colname FROM sysibm.syskeycoluse WHERE constname='"+drFK[4]+"' AND colseq="+drFKComp[2]+" AND tbname='"+drFK[1]+"')")+"*";
				img2 = PutImage(drFK[1].ToString());
			}
			else
			{
				string condicional = "";
				string[] sepMain = fksComps.Split('*');
				for(int k=0;k<sepMain.Length;k++)
				{
					string[] sepSec = sepMain[k].Split('-');
					if(sepSec[0].ToString().Trim() == drFK[3].ToString().Trim())
					{
						if(sepSec[4].ToString().Trim() == "CHARACTER" || sepSec[4].ToString().Trim() == "VARCHAR" || sepSec[4].ToString().Trim() == "TIME" || sepSec[4].ToString().Trim() == "DATE")
							condicional = sepSec[3]+" = '"+this.ValueDdl(sepSec[1].ToString(),tbForm)+"' AND";
						else
							condicional = sepSec[3]+" = "+this.ValueDdl(sepSec[1].ToString(),tbForm)+" AND";
					}
				}
				ddl = PutDropDownList(drFK[1].ToString(),DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='"+drFK[4]+"' AND colseq="+drFKComp[2]+""),condicional.Substring(0,condicional.Length-3));
				img2=PutImage(drFK[1].ToString());
				ddlSec += drFK[3]+"-"+ddl.ID+"-"+drFK[1]+"-"+DBFunctions.SingleData("SELECT colname FROM sysibm.syskeycoluse WHERE constname='"+drFK[4]+"' AND colseq="+drFKComp[2]+"")+"-"+DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name=(SELECT colname FROM sysibm.syskeycoluse WHERE constname='"+drFK[4]+"' AND colseq="+drFKComp[2]+" AND tbname='"+drFK[1]+"')")+"*";
			}
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(ddl);
			tc2.Controls.Add(img2);
			tc2.Controls.Add(new LiteralControl("   "));
			tc2.Controls.Add(img3);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbForm.Rows.Add(tr);
			img=img2;
		}
		
		protected System.Web.UI.WebControls.Image PutImage(string tableName)
		{
			System.Web.UI.WebControls.Image img=new System.Web.UI.WebControls.Image();
			img.ID = "img_" + i.ToString();
			img.ImageUrl = images + "AMS.Search.png";
			string newsels=sels.Replace("'","\\'");
			if(newsels.IndexOf("MITEMS")!=-1)
				img.Attributes.Add("onClick","ModalDialog(control_" + i.ToString() +",'SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as CODIGO, MIT.mite_nombre as NOMBRE, PLIN.plin_tipo as LINEA FROM mitems MIT, plineaitem PLIN  WHERE MIT.plin_codigo=PLIN.plin_codigo ORDER BY MIT.mite_codigo',new Array(),null,1);");
			else
				img.Attributes.Add("onClick","ModalDialog(control_" + i.ToString() +",'"+newsels+"',new Array());");
			img.ToolTip="Busqueda";
			return img;
		}
		
		protected System.Web.UI.WebControls.Image PutImageHelp(DataRow dr)
		{
			System.Web.UI.WebControls.Image img=new System.Web.UI.WebControls.Image();
			img.ID = "imghelp_" + i.ToString();
			img.ImageUrl = images + "AMS.Help.png";
			img.Attributes.Add("onClick","ModalDialogAyuda('"+Request.QueryString["table"]+"','"+dr[1].ToString()+"');");
			img.ToolTip="Haga Click para Ver la Ayuda";
			return img;
		}
		
		protected Label PutLabel(string text)
		{
			Label lb = new Label();
			lb.ID = "label_" + i.ToString();
			lb.Text = text + ": ";
            lb.Width = 120;
			return lb;
		}
		
		protected TextBox PutTextBox(int l)
		{
			TextBox tb = new TextBox();
			tb.ID = "control_" + i.ToString();
			if(l < 5)
				tb.Width = new Unit(25);
            if (l <= 10 && l >= 5) 
            {
                tb.CssClass = "tmediano";
                //tb.Width = new Unit(l*11);
            }
            if (l <= 100 && l > 10)
            {
                //tb.Width = new Unit(l*7);
                tb.CssClass = "tmediano";
            }
			if(l > 100 && l < 5000)
			{
				//tb.Width = new Unit(600);
				//tb.Height = new Unit(60);
                tb.CssClass = "amediano";
				tb.TextMode = TextBoxMode.MultiLine;
			}
			if(l >= 5000)
			{
				//tb.Width = new Unit(400);
				//tb.Height = new Unit(120);
                tb.CssClass = "amediano";
				tb.TextMode = TextBoxMode.MultiLine;
			}
			return tb;
		}
		
		protected DropDownList PutDropDownList(string tableName)
		{
			DataSet tempDS = new DataSet();
			if(tableName == "MNIT")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT mnit_nit,mnit_nit CONCAT ' - ' CONCAT mnit_apellidos concat ' ' concat coalesce(mnit_apellido2,'') concat ' ' concat mnit_nombres concat ' ' concat coalesce(mnit_nombre2 ,'') FROM "+tableName+" ORDER BY mnit_nit");
				sels="SELECT NIT,NOMBRES FROM DBXSCHEMA.VTESORERIA_MNIT ORDER BY NIT";
			}
			else if(tableName == "MCUENTA")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT mcue_codipuc,  mcue_codipuc CONCAT ' - ' CONCAT mcue_nombre FROM " + tableName+" WHERE timp_codigo <> 'N' order by mcue_codipuc");
				sels="SELECT * FROM DBXSCHEMA.VMCUENTA ORDER BY CODIGO";
			}
			else if(tableName == "MPROVEEDOR")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MPR.mnit_nit, MPR.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos ' ' concat coalesce(mnit_apellido2,'') concat ' ' CONCAT ' - ' CONCAT MNI.mnit_nombres ' ' concat coalesce(mnit_NOMBRE2,'') FROM mproveedor MPR, mnit MNI WHERE MNI.mnit_nit = MPR.mnit_nit");
				sels="SELECT MPR.mnit_nit, MPR.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos ' ' concat coalesce(mnit_apellido2,'') concat ' ' CONCAT ' - ' CONCAT MNI.mnit_nombres ' ' concat coalesce(mnit_NOMBRE2,'') FROM mproveedor MPR, mnit MNI WHERE MNI.mnit_nit = MPR.mnit_nit";
			}
			else if(tableName == "MCLIENTE")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MCL.mnit_nit, MCL.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos ' ' concat coalesce(mnit_apellido2,'') concat ' ' CONCAT ' ' CONCAT MNI.mnit_nombres ' ' concat coalesce(mnit_NOMBRE2,'') FROM mcliente MCL, mnit MNI WHERE MNI.mnit_nit = MCL.mnit_nit");
				sels="SELECT MCL.mnit_nit, MCL.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos ' ' concat coalesce(mnit_apellido2,'') concat ' ' CONCAT MNI.mnit_nombres ' ' concat coalesce(mnit_NOMBRE2,'') FROM mcliente MCL, mnit MNI WHERE MNI.mnit_nit = MCL.mnit_nit";
			}
			else if(tableName == "PIVA")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT piva_porciva, SUBSTR(CAST(piva_porciva AS character(5)),2,LENGTH(CAST(piva_porciva AS character(5)))-1) CONCAT '- ' CONCAT piva_decreto FROM piva");
				sels="SELECT piva_porciva, SUBSTR(CAST(piva_porciva AS character(5)),2,LENGTH(CAST(piva_porciva AS character(5)))-1) CONCAT '- ' CONCAT piva_decreto FROM piva;";
			}
			else if(tableName == "PLINEAITEM")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem ORDER BY plin_codigo");
				sels="SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem ORDER BY plin_codigo";
			}
			else if(tableName == "PARANCEL")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT para_codigo, para_codigo CONCAT '-' CONCAT para_nombre FROM parancel ORDER BY para_codigo ASC");
				sels="SELECT para_codigo, para_codigo CONCAT '-' CONCAT para_nombre FROM parancel ORDER BY para_codigo ASC";
			}
			else if(tableName == "PESPACIONOMBRES")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT pesp_nombre FROM pespacionombres ORDER BY pesp_nombre ASC");
				sels="SELECT pesp_nombre FROM pespacionombres ORDER BY pesp_nombre ASC";
			}
			else if(tableName == "MEMPLEADO")
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT MEMP.memp_codiempl, MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_nombres ' ' concat coalesce(mnit_apellido2,'') concat ' ' CONCAT ' ' CONCAT MNIT.mnit_apellidos ' ' concat coalesce(mnit_apellido2,'') FROM dbxschema.mempleado MEMP, dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit ORDER BY MEMP.memp_codiempl ASC");
				sels="SELECT MEMP.memp_codiempl, MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_nombres ' ' concat coalesce(mnit_apellido2,'') concat ' ' CONCAT ' ' CONCAT MNIT.mnit_apellidos ' ' concat coalesce(mnit_NOMBRE2,'') FROM dbxschema.mempleado MEMP, dbxschema.mnit MNIT WHERE MEMP.mnit_nit=MNIT.mnit_nit ORDER BY MEMP.memp_codiempl ASC";
			}
			else if(tableName == "MITEMS")
			{
				tempDS=DBFunctions.Request(tempDS,IncludeSchema.YES,"SELECT MITE.mite_codigo,DBXSCHEMA.EDITARREFERENCIAS(MITE.mite_codigo,PLIN.plin_tipo) CONCAT ' - ' CONCAT MITE.mite_nombre FROM dbxschema.mitems MITE,dbxschema.plineaitem PLIN WHERE PLIN.plin_codigo=MITE.plin_codigo");
				sels="SELECT * FROM " + tableName+" ORDER BY "+DBFunctions.SingleData("SELECT NAME FROM sysibm.syscolumns WHERE tbname='"+tableName+"' AND colno=1")+" ASC";
			}
			else if(tableName == "PTEMPARIO")
			{
				tempDS=DBFunctions.Request(tempDS,IncludeSchema.YES,"SELECT ptem_operacion,ptem_operacion CONCAT ' - ' CONCAT ptem_descripcion FROM ptempario ORDER BY ptem_operacion");
				sels="SELECT ptem_operacion,ptem_descripcion FROM dbxschema.ptempario";
			}
			else
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT * FROM " + tableName+" ORDER BY "+DBFunctions.SingleData("SELECT NAME FROM sysibm.syscolumns WHERE tbname='"+tableName+"' AND colno=1")+" ASC");
				sels="SELECT * FROM " + tableName+" ORDER BY "+DBFunctions.SingleData("SELECT NAME FROM sysibm.syscolumns WHERE tbname='"+tableName+"' AND colno=1")+" ASC";
			}
			DropDownList ddl = new DropDownList();
			ddl.ID = "control_" + i.ToString();
            ddl.CssClass = "dmediano";
			try
			{
				ddl.DataSource = tempDS.Tables[0];
				if(tableName != "PLINEAITEM")
				{
					if(tempDS.Tables[0].PrimaryKey.Length > 0)
						ddl.DataValueField = tempDS.Tables[0].PrimaryKey[0].ToString();
					if(tempDS.Tables[0].Columns.Count > 1)
						ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
				}
				else
				{
					ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
					ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
				}
				ddl.DataBind();
			}
			catch(Exception e)
			{
				lbInfo.Text += e.ToString() + "<br>"+DBFunctions.exceptions;
			}
			tempDS.Clear();
			fkCount++;
			return ddl;
		}
		
		protected DropDownList PutDropDownList(string tableName, string fieldName, string condicional)
		{
			DataSet tempDS = new DataSet();
			if(condicional=="")
			{
				if(tableName=="MFACTURAPROVEEDOR")
				{
					tempDS=DBFunctions.Request(tempDS, IncludeSchema.YES,"SELECT DISTINCT MFAC."+fieldName+",MFAC."+fieldName+" CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM "+tableName+" MFAC,pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo ORDER BY "+fieldName+" ASC");
					sels="SELECT DISTINCT MFAC."+fieldName+",MFAC."+fieldName+" CONCAT ' - ' CONCAT PDOC.pdoc_descripcion FROM "+tableName+" MFAC,pdocumento PDOC WHERE MFAC.pdoc_codiordepago=PDOC.pdoc_codigo ORDER BY "+fieldName+" ASC";
				}
				else
				{
					tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT DISTINCT "+fieldName+" FROM " + tableName+" ORDER BY "+fieldName+" ASC");
					sels="SELECT DISTINCT "+fieldName+" FROM " + tableName+" ORDER BY "+fieldName+" ASC";
				}
				
			}
			else
			{
				tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT DISTINCT "+fieldName+" FROM " + tableName+" WHERE "+condicional+" ORDER BY "+fieldName+" ASC");
				sels="SELECT DISTINCT "+fieldName+" FROM " + tableName+" WHERE "+condicional+" ORDER BY "+fieldName+" ASC";
			}
			DropDownList ddl = new DropDownList();
			ddl.ID = "control_" + i.ToString();
            ddl.CssClass = "dmediano";
            try
			{
				if(tempDS.Tables[0].Columns.Count==1)
				{
					ddl.DataSource = tempDS.Tables[0];
					ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
					ddl.DataTextField = tempDS.Tables[0].Columns[0].ToString();
					ddl.DataBind();
				}
				else
				{
					ddl.DataSource = tempDS.Tables[0];
					ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
					ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
					ddl.DataBind();
				}
			}
			catch(Exception e)
			{
				lbInfo.Text +="<br>"+ e.ToString() + "<br>";
			}
			tempDS.Clear();
			fkCount++;
			return ddl;
		}
		
		protected void PutDropDownList(string idDdl, string idDdlConditional, string tableName, string fieldName, string fieldConditional, string typeConditional)
		{
			DatasToControls bind = new DatasToControls();
			string select = "SELECT "+fieldName+" FROM "+tableName+" WHERE "+fieldConditional+"=";
			if(typeConditional == "CHARACTER" || typeConditional == "VARCHAR" || typeConditional == "TIME" || typeConditional == "DATE")
				select +="'"+this.ValueDdl(idDdlConditional,tbForm)+"'";
			else
				select +=""+this.ValueDdl(idDdlConditional,tbForm)+"";
			for(int m=0;m<tbForm.Rows.Count;m++)
			{
				if(tbForm.Rows[m].Cells[1].Controls[0].GetType() == typeof(DropDownList))
				{
					if(tbForm.Rows[m].Cells[1].Controls[0].ID.Trim() == idDdl)
					{
						bind.PutDatasIntoDropDownList(((DropDownList)tbForm.Rows[m].Cells[1].Controls[0]),select);
						sels=select;
					}
				}
			}
		}
		
		protected RegularExpressionValidator Validator(string controlID, string regex, string textError)
		{
			RegularExpressionValidator rev = new RegularExpressionValidator();
			rev.ErrorMessage = textError+ ". No Valido .";
			rev.ControlToValidate = controlID;
			rev.ValidationExpression = regex;
			rev.Display = ValidatorDisplay.None;
			return rev;
		}
		
		protected RequiredFieldValidator RequiredValidator(string controlID, string textError)
		{
			RequiredFieldValidator rfv = new RequiredFieldValidator();
			rfv.ErrorMessage = textError+ ". Requerido.";
			rfv.ControlToValidate = controlID;
			rfv.Display = ValidatorDisplay.None;
			return rfv;
		}
		
		private void BR()
		{
			phForm.Controls.Add(new LiteralControl("<br>"));
		}
		
		protected void PopulateForm()
		{
			for(i=0; i<ds.Tables[ds.Tables.Count-1].Columns.Count; i++)
			{
				Control ctl = tbForm.FindControl("control_" + i.ToString());
				if(ctl!=null)
				{
					if(ctl.ToString() == "System.Web.UI.WebControls.TextBox")
					{
						if(ds.Tables[ds.Tables.Count-1].Columns[i].DataType.ToString() == "System.DateTime" && ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString()!="")
						{
							string valordt=ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString();
							string[] valordtsp=valordt.Split(' ');
							if(valordtsp[1]=="12:00:00" && valordtsp[2]!=null &&valordtsp[2]=="AM")
								valordt=Convert.ToDateTime(ds.Tables[ds.Tables.Count-1].Rows[0][i]).ToString("yyyy-MM-dd");
							else
								valordt=Convert.ToDateTime(ds.Tables[ds.Tables.Count-1].Rows[0][i]).ToString("yyyy-MM-dd HH:mm:ss");
							try{((TextBox)ctl).Text = valordt;}
							catch{((TextBox)ctl).Text = "";/*IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);((TextBox)ctl).Text = DateTime.Now.GetDateTimeFormats('d', culture)[0];*/}
						}
						else if(ds.Tables[ds.Tables.Count-1].Columns[i].DataType.ToString() == "System.Decimal" || ds.Tables[ds.Tables.Count-1].Columns[i].DataType.ToString() == "System.Double")
						{
							try
							{
								NumberFormatInfo info=new CultureInfo("en-US",false).NumberFormat;
								string format=ds.Tables[0].Rows[i][5].ToString();
								info.NumberDecimalDigits=Convert.ToInt32(format);
								((TextBox)ctl).Text = Convert.ToDouble(ds.Tables[ds.Tables.Count-1].Rows[0][i]).ToString("F",info);
							}
							catch
							{
								((TextBox)ctl).Text = "";
							}
						}
						else
						{
							if(table == "MITEMS" && i==0)
							{
								string codI = "";
								Referencias.Editar(ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString(),ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+ds.Tables[ds.Tables.Count-1].Rows[0][2].ToString().Trim()+"'"));
								((TextBox)ctl).Text = codI;
							}
							else
								((TextBox)ctl).Text = ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString();
						}
					}
					else if(ctl.ToString() == "System.Web.UI.WebControls.DropDownList")
					{
						try
						{
							if(table == "MITEMS" && i==2)
								((DropDownList)ctl).SelectedValue = ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString().Trim()+"-"+DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString().Trim()+"'");
							else
							{
								if(ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString() != "")
									((DropDownList)ctl).SelectedValue = ds.Tables[ds.Tables.Count-1].Rows[0][i].ToString();
								else
									((DropDownList)ctl).SelectedIndex = ((DropDownList)ctl).Items.Count - 1;
							}
						}
						catch(Exception e)
						{
							//catch{
							lbInfo.Text += "Advertencia: " + e.ToString();
						}
					}
				}
				else
					lbInfo.Text = "control_" + i.ToString();
			}
			//REVISAR SI LA TABLA ES MITEMS, cambiar valor de codigo con Referencia.Editar
			//Referencias Items
			try
			{
				if(table.ToUpper().Trim()=="MITEMS")
				{
					string itm=((TextBox)tbForm.FindControl("control_0")).Text.Trim();
					string bdga=((DropDownList)tbForm.FindControl("control_2")).SelectedValue;
					string edItm="";
					if(!Referencias.Editar(itm,ref edItm,bdga))
					{
						Response.Write("<script language='javascript'>alert('Advertencia, el codigo del item no cumple con el formato de la bodega!');</script>");
						edItm=itm;
					}
					((TextBox)tbForm.FindControl("control_0")).Text=edItm;
					//Response.Write("<script language='javascript'>alert('Item:"+itm+"   Bdga:"+bdga+"    NItem:"+edItm+"');</script>");
				}
			}
			catch(Exception e)
			{
				lbInfo.Text = e.ToString();
			}
		}
		
		protected void InsertRecords(Object sender, EventArgs e)
		{
			ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT remarks, name, coltype, length, nulls, scale, default, generated FROM sysibm.syscolumns WHERE tbname='" +table + "'");
			ds = DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM " + table + " FETCH FIRST 1 ROWS ONLY ");
			string sql="INSERT INTO "+table+" VALUES (";
			//Referencias Items
			string nitm="";
			string pars = "";
			string itm="",bdga="";
			bool insItm=false;
			if(table.ToUpper().Trim()=="MITEMS")
			{
				itm = Request.Form["control_0"].Trim();
				bdga = (Request.Form["control_2"].Trim().Split('-'))[1];
				if(!Referencias.Guardar(itm,ref nitm,bdga))
				{
					Response.Write("<script language='javascript'>alert('Codigo de item no valido!');</script>");
					//btInsert.Visible = false;
					return;
				}
				insItm=true;
			}
			int a=0;
			if(insItm)
			{
				sql+="'"+nitm+"', ";
				a++;
				if(sqlQuery.IndexOf("MITE_CODIGO") != -1 || sqlQuery.IndexOf("mite_codigo") != -1)
					pars += itm+"*MITE_CODIGO+";
			}
			for(i=a; i<ds.Tables[ds.Tables.Count-1].Columns.Count; i++)
			{
				DataRow[] scRow = ds.Tables[0].Select("NAME = '" + ds.Tables[ds.Tables.Count-1].Columns[i].ColumnName + "'");
				if(scRow.Length > 0)
				{
					if(scRow[0][2].ToString().IndexOf("CHAR") != -1 || scRow[0][2].ToString().IndexOf("VAR") != -1 || scRow[0][2].ToString().IndexOf("TIME") != -1)
					{
						if(ds.Tables[ds.Tables.Count-1].Columns[i].ColumnName == "PLIN_CODIGO" && table.ToUpper().Trim() == "MITEMS")
						{
							sql += "'"+ (Request.Form["control_" + i.ToString().Trim() + ""].Split('-'))[0]+"',";
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars+=Request.Form["control_" + i.ToString().Trim() + ""]+"*"+scRow[0][1]+"+";
						}
						else
						{
							if(Request.Form["control_" + i.ToString().Trim() + ""] == "null")
								sql += ""+ Request.Form["control_" + i.ToString().Trim() + ""]+", ";
							else
								sql += "'"+ Request.Form["control_" + i.ToString().Trim() + ""]+"', ";
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars+=Request.Form["control_" + i.ToString().Trim() + ""]+"*"+scRow[0][1]+"+";
						}
					}
					else if(scRow[0][2].ToString().IndexOf("DATE") != -1)
					{
						if(Request.Form["control_" + i.ToString() + ""]!="")
						{
							string[] splitDate = Request.Form["control_" + i.ToString() + ""].Split('-');
							DateTime dt = new DateTime(Convert.ToInt16(splitDate[0]), Convert.ToInt16(splitDate[1]), Convert.ToInt16(splitDate[2]));
							sql+="'"+dt.Year.ToString()+"-"+dt.Month.ToString()+"-"+dt.Day.ToString()+"', ";
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars += dt.Year.ToString()+"-"+dt.Month.ToString()+"-"+dt.Day.ToString()+"*"+scRow[0][1]+"+";
						}
						else
						{
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars += "*"+scRow[0][1]+"+";
							sql+="null, ";
						}
					}
					else if(scRow[0][2].ToString().IndexOf("INT") != -1)
					{
						if(Request.Form["control_" + i.ToString().Trim() + ""] != "")
						{
							sql+=Request.Form["control_" + i.ToString().Trim() + ""]+", ";
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars += Request.Form["control_" + i.ToString().Trim() + ""]+"*"+scRow[0][1]+"+";
						}
						else
						{
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars += "*"+scRow[0][1]+"+";
							sql+="null, ";
						}
					}
					else if((scRow[0][2].ToString().IndexOf("DECIMAL") != -1) || (scRow[0][2].ToString().IndexOf("DOUBLE") != -1))
					{
						if(Request.Form["control_" + i.ToString().Trim() + ""]!="")
						{
							sql+=Convert.ToDouble(Request.Form["control_" + i.ToString().Trim() + ""]).ToString()+", ";
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars += Request.Form["control_" + i.ToString().Trim() + ""]+"*"+scRow[0][1]+"+";
						}
						else
						{
							if(sqlQuery.IndexOf(scRow[0][1].ToString().ToUpper()) != -1 || sqlQuery.IndexOf(scRow[0][1].ToString().ToLower()) != -1)
								pars += "*"+scRow[0][1]+"+";
							sql+="null, ";
						}
					}
				}
			}
			sql=sql.Substring(0,sql.Length-2);
			sql+=");";
			ArrayList sqls=new ArrayList();
			sqls.Add(sql);
			int indiceQuery = 0;
			if(sqlQuery.IndexOf("WHERE") != -1)
				indiceQuery = sqlQuery.IndexOf("WHERE");
			else if(sqlQuery.IndexOf("where") != -1)
				indiceQuery = sqlQuery.IndexOf("where");
			else if(sqlQuery.IndexOf("ORDER") != -1)
				indiceQuery = sqlQuery.IndexOf("ORDER");
			else if(sqlQuery.IndexOf("order") != -1)
				indiceQuery = sqlQuery.IndexOf("order");
			if(indiceQuery <= 0)
				indiceQuery = sqlQuery.Length;
			string regA, vals="", colAgr="";
			string[] divPars = pars.Substring(0,pars.Length-1).Split('+');
			string[] divSqlQuery = sqlQuery.Substring(0,indiceQuery).Split(',');
			for(int i=0;i<divSqlQuery.Length;i++)
			{
				string subSqlQuery = divSqlQuery[i];
				for(int j=0;j<divPars.Length;j++)
				{
					string nomCol = (divPars[j].Split('*'))[1];
					if(colAgr.IndexOf(nomCol.ToUpper().Trim()) == -1) //si no miré la columna todavia
					{
						if(subSqlQuery.IndexOf(nomCol.ToUpper()) != -1 || subSqlQuery.IndexOf(nomCol.ToLower()) != -1) //si la columna esta en la consulta original
						{
							if((divPars[j].Split('*'))[0].IndexOf(',') == -1)
                                if (j < divPars.Length - 1)
                                    if (subSqlQuery.ToUpper().Contains(divPars[j+1].Split('*')[1]))
                                        vals += (divPars[j].Split('*'))[0] + " ";
                                    else
                                        vals += (divPars[j].Split('*'))[0] + ",";
                                
                                else
                                    try { vals += Convert.ToDouble((divPars[j].Split('*'))[0]).ToString() + ","; }
                                    catch { vals += (divPars[j].Split('*'))[0] + ","; }
							colAgr += nomCol.ToUpper().Trim()+"*";
						}
					}
				}
			}

            if (!DBFunctions.Transaction(sqls))
            {
                QueryCache.removeData(table);
                Response.Redirect("AMS.Web.ModalDialog.aspx?params=" + Request.QueryString["sql"] + "&Ins=1&Reg=0");
            }
            else
            {
                Response.Redirect("AMS.Web.ModalDialog.aspx?Vals=" + vals.Substring(0, vals.Length - 1) + "&params=" + Request.QueryString["sql"] + "&Ins=1&Reg=1");
            }
		}
				
		protected string ValueDdl(string idDdl, Table tbl)
		{
			string valor = "";
			for(int m=0;m<tbl.Rows.Count;m++)
			{
				//lbInfo.Text+="<br>"+tbl.Rows[m-1].Cells[1].Controls[0].ToString();
				if(tbl.Rows[m].Cells[1].Controls[0].GetType() == typeof(DropDownList))
				{
					if(tbl.Rows[m].Cells[1].Controls[0].ID.Trim() == idDdl.Trim())
						valor = ((DropDownList)tbl.Rows[m].Cells[1].Controls[0]).SelectedValue;
				}
			}
			return valor;
		}
		
		protected void ChangedFK(Object  Sender, EventArgs e)
		{
			this.FindDdlSecundary(((DropDownList)Sender).ID.Trim());
		}
		
		protected void FindDdlSecundary(string idDdlMain)
		{
			string idSecundaryControl = "";
			string []sepMain1 = fksComps.Split('*');
			string tableMain = "", fieldMain = "", fieldCondition = "", typeCondition = "";
			for(int m=0;m<sepMain1.Length-1;m++)
			{
				string[] sepSec1 = sepMain1[m].Split('-');
				if(sepSec1[1].Trim() == idDdlMain)
				{
					string[] sepMain2 = ddlSec.Split('*');
					for(int k=0;k<sepMain2.Length-1;k++)
					{
						string[] sepSec2 = sepMain2[k].Split('-');
						if(sepSec1[0].Trim()==sepSec2[0].Trim())
						{
							idSecundaryControl = sepSec2[1].Trim();
							tableMain = sepSec2[2].Trim();
							fieldMain = sepSec2[3].Trim();
							fieldCondition = sepSec1[3].Trim();
							typeCondition = sepSec1[4].Trim();
						}
					}
				}
			}
			this.PutDropDownList(idSecundaryControl,idDdlMain,tableMain,fieldMain,fieldCondition,typeCondition);
		}

        protected override void InitializeCulture()
        {
            string strCulture = "en-US";
            CultureInfo ci = new CultureInfo(strCulture);

            UICulture = strCulture;
            Culture = strCulture;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            base.InitializeCulture();
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