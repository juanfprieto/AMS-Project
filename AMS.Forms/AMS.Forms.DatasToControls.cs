using System;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.Odbc;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;
using System.Collections;


namespace AMS.Forms
{
	public class DatasToControls
	{
		#region Instancia

		#region Variables

		protected string table, msg;
		protected OdbcDataReader dr;
		protected DataSet ds = new DataSet();
		protected DataTable dt;
		protected DataRow drow;
		protected string sqlSta = "",dato1,dato2,datokey;
		protected int i;
		
		#endregion

		#region Propiedades

		public string Msg{get{return msg;}}
		public string SqlSta{get{return sqlSta;}}
		public DatasToControls(string srcTable){table = srcTable;}
		
		#endregion

		#region Metodos

		/*public void WithNextFields(params string[] fields)
		{
			/*sql = new BuildSqlStrings(table);
			sql.BuildSelect(fields);
			sql.OrderBy(fields[0], "ASC");
			sqlSta += sql.SqlStatement;	
		}*/
		
		/*public void WithNextFieldsAndCond(string cond, params string[] fields)
		{
			/*sql = new BuildSqlStrings(table);
			sql.BuildSelect(fields);
			sql.AddAllConditions(cond);
			sqlSta += sql.SqlStatement;	
		}*/
		
		/*public void GetDatas(string sqlString)
		{
			/*connection = new DbAdapter();
			connection.Connect();
			connection.SetSchema();
			dr = connection.SqlQueryDR(sqlString);
		}*/
		
		public void PutDatasIntoListBox(ListBox lb, string sql)
		{
			DataSet ds = new DataSet();
			try
			{
				ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
				msg = DBFunctions.exceptions;
				
				lb.DataSource = ds;
				if(ds.Tables[0].Columns.Count > 1)
					lb.DataTextField = ds.Tables[0].Columns[1].ColumnName;
				else
					lb.DataTextField = ds.Tables[0].Columns[0].ColumnName;
				lb.DataValueField = ds.Tables[0].Columns[0].ColumnName;
				try
				{
					lb.DataBind();
				}
				catch(Exception e)
				{
					msg = e.ToString();
				}
			}
			catch(Exception e)
			{
				msg = e.ToString();
			}
			ds.Clear();
			
			
		}
		
		public void PutDatasIntoDropDownList(DropDownList ddl, string sql)
		{
			
			DataSet ds = new DataSet();
			try
			{
				ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
				msg = DBFunctions.exceptions;
				
				ddl.DataSource = ds;
				if(ds.Tables[0].Columns.Count > 1)
					ddl.DataTextField = ds.Tables[0].Columns[1].ColumnName;
				else
					ddl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
				ddl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
				try
				{
					ddl.DataBind();
				}
				catch(Exception e)
				{
					msg = e.ToString();
				}
			}
			catch(Exception e)
			{
				msg = e.ToString();
			}
			ds.Clear();
			
		}

		public void PutDatasIntoRadioButtonList(RadioButtonList rbl, string sql)
		{
			DataSet ds = new DataSet();
			try
			{
				ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
				msg = DBFunctions.exceptions;
				
				rbl.DataSource = ds;
				
				if(ds.Tables[0].Columns.Count > 1)
					rbl.DataTextField = ds.Tables[0].Columns[1].ColumnName;
				else
					rbl.DataTextField = ds.Tables[0].Columns[0].ColumnName;
				rbl.DataValueField = ds.Tables[0].Columns[0].ColumnName;
				try
				{
					rbl.DataBind();
					rbl.SelectedIndex = 0;
				}
				catch(Exception e)
				{
					msg = e.ToString();
				}
			}
			catch(Exception e)
			{
				msg = e.ToString();
			}
			ds.Clear();
		}
	
		public void PutDatasIntoCheckBox(CheckBoxList Lb, string sql)
		{
			DataSet ds = new DataSet();
			try
			{
				ds = DBFunctions.Request(ds, IncludeSchema.NO, sql);
				msg = DBFunctions.exceptions;
				
				Lb.DataSource = ds;
				
				if(ds.Tables[0].Columns.Count > 1)
					Lb.DataTextField = ds.Tables[0].Columns[1].ColumnName;
				else
					Lb.DataTextField = ds.Tables[0].Columns[0].ColumnName;
				Lb.DataValueField = ds.Tables[0].Columns[0].ColumnName;
				try
				{
					Lb.DataBind();
					//rbl.SelectedIndex = 0;
				}
				catch(Exception e)
				{
					msg = e.ToString();
				}
			}
			catch(Exception e)
			{
				msg = e.ToString();
			}
			ds.Clear();
		}
		
		/*public void PutDatasIntoLabel(string sql, Label lb)
		{
			/*DbAdapter lbc = new DbAdapter();
			lbc.Connect();
			lbc.SetSchema();
			ds = lbc.SqlQueryDRToDS(sql);
			lbc.Close();
			
			lb.Text = ds.Tables[0].Rows[0][0].ToString();
			connection.Close();
		}*/
		
		/*public void PutTableIntoDataGrid(DataGrid dg)
		{
			/*DataRow[] dRow;
			sql = new BuildSqlStrings(table);
			sql.BuildSelect();
			sqlSta += sql.SqlStatement;
			
			DbAdapter con = new DbAdapter();
			con.Connect();
			con.SetSchema();
			ds = con.SqlQueryDS(sqlSta);
			con.SqlQueryAddDS(ds, "SELECT name, remarks FROM sysibm.syscolumns WHERE tbname='" + table + "'");
			
			
			for(i=0; i<ds.Tables[0].Columns.Count; i++)
			{
				dRow = ds.Tables[1].Select("name='" + ds.Tables[0].Columns[i].ColumnName + "'");
				ds.Tables[0].Columns[i].ColumnName = dRow[0][1].ToString();
			}
			DataView dv = ds.Tables[0].DefaultView;
			dv.Sort = ds.Tables[0].Columns[0].ToString();
			dg.DataSource = ds.Tables[0].DefaultView;
			dg.DataBind();
			con.Close();
		}*/
		
				
		public void PutTableIntoDataGridWF(DataGrid dg, params string[] fields)
		{
			/*sql = new BuildSqlStrings(table);
			sql.BuildSelect(fields);
			
			if(fields[0].IndexOf(" as") != -1)
			sql.OrderBy(fields[0].Substring(0,fields[0].IndexOf(" as")), "ASC");
			else
				sql.OrderBy(fields[0], "ASC");
			
			sqlSta += sql.SqlStatement;
			
			DbAdapter con = new DbAdapter();
			con.Connect();
			con.SetSchema();
			ds = con.SqlQueryDS(sqlSta);		
			dg.DataSource = ds.Tables[0].DefaultView;	
			dg.DataBind();
			con.Close();*/
		}
		
		
		/*public void BindFromSqlConcat(DropDownList lb)
		{
			/* GetDatas(sqlSta);
			 dt= new DataTable();		   
			 dt.Columns.Add(new DataColumn("CODIGO" ,typeof(string)));
			 dt.Columns.Add(new DataColumn("DESCRIPCION" ,typeof(string)));
		  
			 if  (dr.HasRows)
			 {
			   while (dr.Read())
			   { 
				 dato1=dr.GetString(0);
				 dato2=dr.GetString(1);
				 datokey=dr.GetString(2);
				 drow=dt.NewRow();
				 drow[0]=datokey;
				 drow[1]=dato1 + ":::" + dato2;
				
				 dt.Rows.Add(drow);
			   }
			 }
		
			 lb.DataSource = dt;
			 lb.DataTextField ="DESCRIPCION";
			 lb.DataValueField ="CODIGO";
			
			 lb.DataBind();	
			 connection.Close();
		}*/
		
		public void BindFromDirIntoLB(DropDownList lb, string path)
		{
			try
			{
				string[] files = Directory.GetFiles(path);
				string[] splitInfoFiles;
			
				dt = new DataTable();
				dt.Columns.Add(new DataColumn("path", typeof(string)));
				dt.Columns.Add(new DataColumn("name", typeof(string)));
			
			
			
				for(int i=0; i<files.Length; i++)
				{
					drow = dt.NewRow();
					splitInfoFiles = files[i].Split("\\".ToCharArray());
					drow[0] = files[i];
					drow[1] = splitInfoFiles[splitInfoFiles.Length-1];
					dt.Rows.Add(drow);
				}
				
				lb.DataValueField = "path";
				lb.DataTextField = "name";
				lb.DataSource = dt;
				lb.DataBind();
			}
			catch(Exception e)
			{
				msg = e.ToString();
			}
		}
		
		public void SetFocusOnStart(Control control)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\r\n<script language='JavaScript'>\r\n");
			sb.Append("<!--\r\n");
			sb.Append("\tdocument.");
			Control p = control.Parent;
			while (!(p is System.Web.UI.HtmlControls.HtmlForm)) p = p.Parent;
			sb.Append(p.ClientID);
			sb.Append("['");
			sb.Append(control.UniqueID);
			sb.Append("'].focus();\r\n");
			sb.Append("// -->\r\n");
			sb.Append("</script>");
			control.Page.RegisterStartupScript("SetFocus", sb.ToString());
		}

		#endregion

		#region Constructores

		public DatasToControls()
		{
			
		}
		
		#endregion
		
		#endregion

		#region Estatico

		#region Metodos

		public static void EstablecerDefectoDropDownList(DropDownList source, string campo)
		{
			for(int i=0;i<source.Items.Count;i++)
			{
				if((source.Items[i].ToString().Trim().ToUpper())==campo.Trim().ToUpper())
					source.SelectedIndex = i;				 
			}
            
		}		
		
		public static void EstablecerValueDropDownList(DropDownList source, string valor)
		{
			for(int i=0;i<source.Items.Count;i++)
			{
				if((source.Items[i].Value.Trim())==valor.Trim())
					source.SelectedIndex = i;				 
			}
		}		
		
		public static void EstablecerDefectoRadioButtonList(RadioButtonList source, string campo)
		{
			for(int i=0;i<source.Items.Count;i++)
				if((source.Items[i].ToString().Trim().ToUpper())==campo.Trim().ToUpper())
					source.SelectedIndex = i;
		}		
		
		public static bool ValidarDouble(string doubleProbar)
		{
			double doubleResultado;
			bool resultado = true;
			try
			{
				doubleResultado = System.Convert.ToDouble(doubleProbar);
			}
			catch
			{
				resultado = false;
			}
			return resultado;
		}		
		
		public static bool ValidarDateTime(string fechaProbar)
		{
			DateTime fechaResultado;
			bool resultado = true;
			try
			{
				fechaResultado = System.Convert.ToDateTime(fechaProbar);
			}
			catch
			{
				resultado = false;
			}
			return resultado;
		}
				
		public static bool ValidarInt(string intProbar)
		{
			Int64 intResultado;
			bool resultado = true;
			try
			{
				intResultado = System.Convert.ToInt64(intProbar);
			}
			catch
			{
				resultado = false;
			}
			return resultado;
		}

        public static bool ValidarLong(string longProbar)
        {
            long longResultado;
            bool resultado = true;
            try
            {
                longResultado = System.Convert.ToInt64(longProbar);
            }
            catch
            {
                resultado = false;
            }
            return resultado;
        }
				
		public static void Aplicar_Formato_Grilla(DataGrid grilla)
		{
			grilla.BackColor = Color.White;
			grilla.BorderColor = Color.FromArgb(153,153,153);
			grilla.HeaderStyle.BackColor = Color.FromArgb(204,204,221);
			grilla.Font.Size = new FontUnit(8);
			grilla.Font.Name = "Verdana";
			grilla.GridLines = GridLines.Vertical;
			grilla.SelectedItemStyle.Font.Bold = true;
			grilla.SelectedItemStyle.ForeColor = Color.White;
			grilla.SelectedItemStyle.BackColor = Color.FromArgb(0,138,140);
			grilla.AlternatingItemStyle.BackColor = Color.Gainsboro;
			grilla.ItemStyle.ForeColor = Color.Black;
			grilla.ItemStyle.BackColor = Color.FromArgb(238,238,238);
			grilla.HeaderStyle.Font.Bold = true;
			grilla.HeaderStyle.ForeColor = Color.White;
			grilla.HeaderStyle.BackColor = Color.FromArgb(0,0,132);
		}
		
		public static void JustificacionGrilla(DataGrid dg, DataTable dt)
		{
			int i=0, j=0, k=0;
			if(dg.Columns.Count == dt.Columns.Count)
			{
				for(i=0;i<dt.Columns.Count;i++)
				{
					if(dt.Columns[i].DataType.ToString() == "System.Decimal" || dt.Columns[i].DataType.ToString() == "System.Int32" || dt.Columns[i].DataType.ToString() == "System.Double" || dt.Columns[i].DataType.ToString() == "System.Decimal" || (dt.Columns[i].DataType.ToString() == "System.String" && dt.Rows.Count>0 && dt.Rows[0][i].ToString().IndexOf("$")!=-1) || (dt.Columns[i].DataType.ToString() == "System.String" && dt.Rows.Count>0 && dt.Rows[0][i].ToString().IndexOf("%")!=-1))
					{
						for(j=0;j<dg.Items.Count;j++)
							dg.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
					}
				}
			}
			else if(dg.Columns.Count != dt.Columns.Count && dg.Columns.Count>0)
			{
				for(i=0;i<dt.Columns.Count;i++)
				{
					if(dt.Columns[i].DataType.ToString() == "System.Decimal" || dt.Columns[i].DataType.ToString() == "System.Int32" || dt.Columns[i].DataType.ToString() == "System.Double" || dt.Columns[i].DataType.ToString() == "System.Decimal" || (dt.Columns[i].DataType.ToString() == "System.String" && dt.Rows.Count>0 && dt.Rows[0][i].ToString().IndexOf("$")!=-1) || (dt.Columns[i].DataType.ToString() == "System.String" && dt.Rows.Count>0 && dt.Rows[0][i].ToString().IndexOf("%")!=-1))
					{
						for(k=0;k<dg.Columns.Count;k++)
						{
							try
							{
								if(((BoundColumn)dg.Columns[k]).DataField == dt.Columns[i].ColumnName)
									for(j=0;j<dg.Items.Count;j++)
										dg.Items[j].Cells[k].HorizontalAlign = HorizontalAlign.Right;
							}
							catch{}
						}
					}
				}
			}
			else if(dg.Columns.Count != dt.Columns.Count && dg.Columns.Count == 0)
			{
				for(i=0;i<dt.Columns.Count;i++)
				{
					if(dt.Columns[i].DataType.ToString() == "System.Decimal" || dt.Columns[i].DataType.ToString() == "System.Int32" || dt.Columns[i].DataType.ToString() == "System.Double" || dt.Columns[i].DataType.ToString() == "System.Decimal" || (dt.Columns[i].DataType.ToString() == "System.String" && dt.Rows.Count>0 && dt.Rows[0][i].ToString().IndexOf("$")!=-1) || (dt.Columns[i].DataType.ToString() == "System.String" && dt.Rows.Count>0 && dt.Rows[0][i].ToString().IndexOf("%")!=-1))
					{
						for(j=0;j<dg.Items.Count;j++)
							dg.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
					}
				}
			}
		}
		
		#endregion

		#endregion
	}
}
