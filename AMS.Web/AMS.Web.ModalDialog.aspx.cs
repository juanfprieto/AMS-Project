using System;
using System.Data;
using System.Linq;
using System.Data.Odbc;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;
using AMS.Tools;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Ajax;
using System.Configuration;

namespace AMS.Web
{
	public partial class ModalDialog : System.Web.UI.Page
	{
		#region Controles, variables
		protected OdbcDataReader dr;
		protected DataSet ds = new DataSet();
		protected string jsParams;
		protected string[] param;
		protected ArrayList searchFields = new ArrayList();
		protected int i, k=0;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles, variables
		//Constructor
		public ModalDialog()
		{
			Page.Init += new System.EventHandler(Page_Load);
		}

		//Carga pagina
		protected void Page_Load(object sender, System.EventArgs e)
		{            
           if(!IsPostBack)
			{
                if (!Request.QueryString["Params"].ToString().Contains("SELECT") && Request.QueryString["Params"] != null
                    && !Request.QueryString["Params"].Contains("**"))
                if (Request.QueryString["Vals"] != null) Response.Write("<script language='javascript'></script>");
                if (Request.QueryString["orden"] != null)
                {
                    
                }
                if (Request.QueryString["Reg"] == "1")
                {
                    Response.Write("<script language='javascript'>alert('Registro actualizado.');</script>");
                    return;
                }
                else if (Request.QueryString["Reg"] == "0") Response.Write("<script language='javascript'>alert('No se pudo actualizar el registro.');</script>");
                     else if (!Request.QueryString["Params"].ToString().Contains("SELECT") && Request.QueryString["Params"] != null && !Request.QueryString["Params"].Contains("**"))
                {
                    string[] datos = Request.QueryString["Params"].Split('*');
                    string tabla = datos[1];
                        string pk = datos[2].Trim();
                    DataSet dsFK = new DataSet();
                        DBFunctions.Request(dsFK, IncludeSchema.NO, "SELECT COLNAME FROM SYSCAT.COLUMNS WHERE  TABNAME = '" + tabla + "' AND KEYSEQ = 1;");
                    string llave = dsFK.Tables[0].Rows[0]["COLNAME"].ToString().Trim();
                    string fks = llave + "!" + pk;
                        //Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + tabla + "&action=insert&sql=" + Request.QueryString["params"] + "&processTitle=INSERTAR&fks=" + fks + "&modal=1");
                        Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + tabla + "&action=insert&sql=" + Request.QueryString["params"] + " & processTitle=INSERTAR&fks=" + fks + " & processTitle=INSERTAR&path=" + Request.QueryString["path"] + "&modal=1");
                }

			    BindDatas();

                if (Request.QueryString["valor"] != null)
                    tbWord.Text = Request.QueryString["valor"].ToString();
                
			    if(Request.QueryString["Ins"] == null)
			    	btInserta.Visible = false;
				if(Request.QueryString["params"] != null)
				{
                    string[] sp;
                    if (Request.QueryString["params"].Contains("**"))
                    {
                        string opcion = Request.QueryString["params"].Replace("**", "");
                        string sql = DB.SQLConsultaGlobal.Buscar(opcion);
                        sp = sql.Split(' ');
                    }
                    else { sp = Request.QueryString["params"].Split(' '); }
					string table="";

					for(i=0; i<sp.Length; i++)
					{
						if(sp[i].ToUpper() == "FROM")
						{
							table = sp[i+1];
                            table = table.ToUpper().Replace("DBXSCHEMA.","");
                            if (table == "VMNIT")
                                table = "MNIT";
                            if (table != "MNIT" && table != "MITEMS" && table != "MNITCOTIZACION")
                                LimpiarCache(sender, e);
                            insTabla.Text=table;
							break;
						}
					}
					ArrayList cols = new ArrayList();
                    try
                    {
                        //ds.Tables[0].Columns[i].DataType.ToString() == "System.Int32". En algunos casos la pk datatype no es un string
                        //Entonces se agrega en la condición para que también pueda recibir enteros.. y poder llenar el ddl en cuestión :)
                        for (i = 0; i < ds.Tables[0].Columns.Count; i++)
                            if (ds.Tables[0].Columns[i].DataType.ToString() == "System.String" || ds.Tables[0].Columns[i].DataType.ToString() == "System.Int32")
                                cols.Add(ds.Tables[0].Columns[i].ColumnName);
                        ddlCols.DataSource = cols;
                        ddlCols.DataBind();
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Select: " + Request.QueryString["params"] + "<BR>");
                        Response.Write("Erro: " + ex.InnerException + "<BR>" + ex.Message + "<BR>" + ex.StackTrace + "<BR>");
                    }
				}
				else
				{
					DatasToControls bind = new DatasToControls("mitems");
					bind.PutTableIntoDataGridWF(dgTable, "mite_codigo", "mite_nombre", "puni_codigo");
				}
			}
		}
	
		//Mostrar informacion
		protected void BindDatas()
		{
			try
			{
                string sql = "";
                if(Request.QueryString["params"].Contains("**"))
                {
                    string opcion = Request.QueryString["params"].Replace("**", "");
                    sql = DB.SQLConsultaGlobal.Buscar(opcion);
                }
                else
                {
                    sql = Request.QueryString["params"].Replace("%20", " ");
                }

                ds = QueryCache.getResult(sql);
                DataTable table = ds.Tables[0];

                IEnumerable<DataRow> enumRows = table
                    .AsEnumerable();

                if (enumRows.Count<DataRow>() == 0)
                {
                    table = table.Clone();
                    table.Clear();
                }
                else
                {
                    //if (Request.QueryString["Padre"] != null) enumRows = enumRows.Take(100);

                    table = enumRows
                        .CopyToDataTable<DataRow>();
                }

                dgTable.DataSource = table;
                dgTable.DataBind();
			}
			catch(Exception ex1){lb.Text+="<br>"+Request.QueryString["params"]+"<br>Error:"+ex1.Message;}
		}
	
		//Agregar items de la lista
		protected void DgHelp_ItemDataBound(object sender, DataGridItemEventArgs e)
 		{
			if(Request.QueryString["Padre"]!=null)
			{
				if(e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
	 			{
                    jsParams = "parent.terminarDialogo('";
 					for(i=0; i<e.Item.Cells.Count; i++)
 					{
 						if(i==0)
 						{
 							string refOr="";
 							if(Referencias.Guardar(e.Item.Cells[i].Text.Trim(),ref refOr,e.Item.Cells[i+2].Text.Trim()))
 								jsParams += refOr;
 							else
 								jsParams += e.Item.Cells[i].Text;
 						}
 						else
 							jsParams += e.Item.Cells[i].Text;
 						if(i != e.Item.Cells.Count-1)
 							jsParams += ",";
	 				}
                    if (Request.QueryString["proced"] == "pagos")
                    {
                        jsParams += "','pagos');";
                    }
                    else
                    {
                        jsParams += "','');";
                    }
	 				for(i=0; i<e.Item.Cells.Count; i++)
 					  e.Item.Cells[i].Attributes["onclick"] = jsParams;
	 				e.Item.Attributes["onmouseover"] = "this.style.background='#aaaadd'";
 					e.Item.Attributes["onmouseout"] = "this.style.background='#DEDFDE'";
 				}
			}
			else
			{
				if(e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
		 		{
                    jsParams = "parent.terminarDialogo('";
 					for(i=0; i<e.Item.Cells.Count; i++)
 					{
	 				   jsParams += e.Item.Cells[i].Text;
 						if(i != e.Item.Cells.Count-1)
 							jsParams += ",";
	 				}
                    if (Request.QueryString["proced"] == "pagos")
                    {
                        jsParams += "','pagos');";
                    }
                    else
                    {
                        jsParams += "','');";
                    }

	 				for(i=0; i<e.Item.Cells.Count; i++)
 					  e.Item.Cells[i].Attributes["onclick"] = jsParams;
	 				e.Item.Attributes["onmouseover"] = "this.style.background='#aaaadd'";
	 				e.Item.Attributes["onmouseout"] = "this.style.background='#DEDFDE'";
 				}
			}
        }
	
		//Insertar Click
		protected void Inserta(Object Sender, EventArgs E)
		{
            if (insTabla.Text == "MNITCOTIZACION") Utils.MostrarAlerta(Response, "El nuevo nit a ingresar se guardará \\n en una tabla temporal..");
            String tmpInsert = this.tbWord.Text.Trim().Replace("%", "");
            insTabla.Text = insTabla.Text.Replace("DBXSCHEMA.", "");
            if (Request.QueryString["Value"] != null)
            {
                //Response.Redirect("AMS.Web.ModalDialogIns.aspx?table=" + insTabla.Text + "&action=insert&sql=" + Request.QueryString["params"] + "&value=" + Request.QueryString["Value"] + "&txtInsert=" + tmpInsert);
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + insTabla.Text + "&action=insert&sql=" + Request.QueryString["params"] + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=INSERTAR&path=" + Request.QueryString["path"] + "&modal=1");
            }
            else
            {
                //Response.Redirect("AMS.Web.ModalDialogIns.aspx?table=" + insTabla.Text + "&action=insert&sql=" + Request.QueryString["params"] + "&txtInsert=" + tmpInsert);
                Response.Redirect(indexPage + "?process=DBManager.Inserts&table=" + insTabla.Text + "&action=insert&sql=" + Request.QueryString["params"] + "&cod=" + Request.QueryString["cod"] + "&name=" + Request.QueryString["name"] + "&processTitle=INSERTAR&path=" + Request.QueryString["path"] + "&modal=1");
            }
		}
		//Buscar Click
		protected void Search(Object Sender, EventArgs E)
		{
            if (tbWord.Text != "")
            {
                string sql = "";
                if (Request.QueryString["params"].Contains("**"))
                {
                    string opcion = Request.QueryString["params"].Replace("**", "");
                    sql = DB.SQLConsultaGlobal.Buscar(opcion);
                }
                else
                {
                    sql = Request.QueryString["params"].Replace("%20", " ");
                }
                string column = ddlCols.SelectedItem.Text;
                string content = tbWord.Text;

                dgTable.CurrentPageIndex = 0;

                ds = QueryCache.getResult(sql);
                DataTable table = ds.Tables[0];

                IEnumerable<DataRow> enumRows;

                try
                {
                    enumRows = table
                   .Select(String.Format("{0} LIKE '{1}'", column, content), column)
                   .AsEnumerable();
                }
                catch (Exception err)
                {
                    enumRows = table
                    .Select(String.Format("{0} = {1}", column, content), column)
                    .AsEnumerable();
                }

                //IEnumerable<DataRow> enumRows = table
                //    .Select(String.Format("{0} LIKE '{1}'", column, content), column)
                //    .AsEnumerable();

                if (enumRows.Count<DataRow>() == 0)
                {
                    table = table.Clone();
                    table.Clear();
                }
                else
                {
                    table = enumRows
                        .CopyToDataTable<DataRow>();
                }

                dgTable.DataSource = table;
                dgTable.DataBind();
            }
		}

		//Paginar
		protected void dgHelp_Page(Object sender, DataGridPageChangedEventArgs e)
		{
            string sql = "";
            if (Request.QueryString["params"].Contains("**"))
            {
                string opcion = Request.QueryString["params"].Replace("**", "");
                sql = DB.SQLConsultaGlobal.Buscar(opcion);
            }
            else { sql = Request.QueryString["params"].Replace("%20", " "); }
            string column = ddlCols.SelectedItem.Text;
            string content = tbWord.Text;
            

            dgTable.CurrentPageIndex = e.NewPageIndex;

            ds = QueryCache.getResult(sql);
            DataTable table = ds.Tables[0];
            if (tbWord.Text != "")
            {
            
                IEnumerable<DataRow> enumRows = table
                    .Select(String.Format("{0} LIKE '{1}'", column, content), column)
                    .AsEnumerable();

                if (enumRows.Count<DataRow>() == 0)
                {
                    table = table.Clone();
                    table.Clear();
                }
                else
                {
                    //if (Request.QueryString["Padre"] != null)
                    //    enumRows = enumRows.Take(100);

                    table = enumRows
                        .CopyToDataTable<DataRow>();
                }

                dgTable.DataSource = table;
            }
            else
            { 
                dgTable.DataSource = table
                    .AsEnumerable()
                    .CopyToDataTable<DataRow>();
            }
            dgTable.DataBind();
            

        }

        protected void LimpiarCache(Object sender, EventArgs e)
        {
            QueryCache.removeQuery(Request.QueryString["params"]);
            tbWord.Text = "";

            BindDatas();
        }


		private void InitializeComponent()
		{

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
	}
}

