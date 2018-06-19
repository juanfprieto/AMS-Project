using System;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;

namespace AMS.Web
{
	public partial class ModalDialogPagos : System.Web.UI.Page
	{
		protected TextBox insTabla,insSQL;
		protected Button btInserta;
		protected OdbcDataReader dr;
		protected DataSet ds = new DataSet();
		protected string jsParams;
		protected string[] param;
		protected ArrayList searchFields = new ArrayList();
		protected int i, k=0;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
    	     	BindDatas();
			    if(Request.QueryString["params"] != null)
				{
					ArrayList cols = new ArrayList();
					for(i=0; i<ds.Tables[0].Columns.Count; i++)
						if(ds.Tables[0].Columns[i].DataType.ToString() == "System.String")
							cols.Add(ds.Tables[0].Columns[i].ColumnName);
					ddlCols.DataSource = cols;
					ddlCols.DataBind();
				}
			}
		}
	
		protected void BindDatas()
		{
			try
			{
				if(Request.QueryString["numeros"]!=null)
				{
					string[] numeros=Request.QueryString["numeros"].Split('b');
					ds = DBFunctions.Request(ds, IncludeSchema.NO, Request.QueryString["params"]);
					for(int i=0;i<numeros.Length;i++)
					{
						ds.Tables[0].Rows[i][3]=(numeros[i].Split('a'))[0];
						ds.Tables[0].Rows[i][4]=(numeros[i].Split('a'))[1];
						ds.AcceptChanges();
					}
					dgTable.DataSource = ds.Tables[0];
					dgTable.DataBind();
				}
				else
				{
					ds = DBFunctions.Request(ds, IncludeSchema.NO, Request.QueryString["params"]);
					dgTable.DataSource = ds.Tables[0];
					dgTable.DataBind();
				}
			}
			catch(Exception e){lb.Text += e.Message;}
		}
	
		protected void DgHelp_ItemDataBound(object sender, DataGridItemEventArgs e)
 		{
			if(e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
	 		{
 				jsParams = "javascript: GetValue('";
 				for(i=0; i<e.Item.Cells.Count; i++)
 				{
	 			   jsParams += e.Item.Cells[i].Text;
 					if(i != e.Item.Cells.Count-1)
 						jsParams += ",";
 				}
				jsParams += "')";
	 			for(i=0; i<e.Item.Cells.Count; i++)
 				  e.Item.Cells[i].Attributes["onclick"] = jsParams;
	 			e.Item.Attributes["onmouseover"] = "this.style.background='#aaaadd'";
 				e.Item.Attributes["onmouseout"] = "this.style.background='#DEDFDE'";
 			}
 		}
	
		protected void Search(Object Sender, EventArgs E)
		{
        	dgTable.CurrentPageIndex= 0;
	   		ds = DBFunctions.Request(ds, IncludeSchema.NO, Request.QueryString["params"]);
			DataView dv = new DataView(ds.Tables[0]);
			if(tbWord.Text != "")
				dv.RowFilter = "" + ddlCols.SelectedItem.Text + " like '" + tbWord.Text + "'";
			dv.Sort=ddlCols.SelectedItem.Text;
			dgTable.DataSource = dv;
			dgTable.DataBind();
		}


		protected void dgHelp_Page(Object sender, DataGridPageChangedEventArgs e)
		{
	    	dgTable.CurrentPageIndex= e.NewPageIndex;
	   		ds = DBFunctions.Request(ds, IncludeSchema.NO, Request.QueryString["params"]);
			DataView dv = new DataView(ds.Tables[0]);
			if(tbWord.Text != "")
				dv.RowFilter = "" + ddlCols.SelectedItem.Text + " like '" + tbWord.Text + "'";
			dv.Sort=ddlCols.SelectedItem.Text;
			dgTable.DataSource = dv;
			dgTable.DataBind();
		}
		
		////////////////////////////////////////////////
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

