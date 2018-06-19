using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
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
using AMS.Tools;
//using AMS.Inventarios;

namespace AMS.DBManager
{
	public partial class MasterTable : System.Web.UI.UserControl
	{
		protected DataSet dsTbRefs;
		protected string table;
		protected string[] nomColsPK;
        protected string[] nomColsAll;

		protected void Page_Load(object sender, EventArgs e)
		{
			table = Request.QueryString["table"].ToUpper();
			nomColsPK = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE tbname='"+table+"'").Split('+');
            if(table == "MCATALOGOVEHICULO")
            {
                string columnas = "+";
                DataSet nomCols = new DataSet();
                DBFunctions.Request(nomCols, IncludeSchema.NO, "SELECT NAME FROM sysIBM.SYSCOLUMNS WHERE tbname='" + table + "'  ORDER BY COLNO");
                for (int i = 0; i < nomCols.Tables[0].Rows.Count; i++ )
                {
                    columnas += nomCols.Tables[0].Rows[i][0] + "+";
                }
                if (columnas.EndsWith("+")) columnas = columnas.Substring(0,columnas.Length - 1); 
                nomColsAll = columnas.Split('+');
            }
            dsTbRefs = new DataSet();
			DBFunctions.Request(dsTbRefs,IncludeSchema.NO,"SELECT DISTINCT tbname FROM sysibm.sysrels WHERE reftbname = '"+table+"'");
			MakeOptions();
			PopulateFilter();
			MakeHelp();
			if(!IsPostBack)
				Session.Clear();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
			if(Session["Rep"] != null)
			{
				MailMessage MyMail = new MailMessage();
				MyMail.From = ConfigurationManager.AppSettings["EmailFrom"];
				MyMail.To = tbEmail.Text;
				MyMail.Subject = "Reporte Prueba";
				MyMail.Body = ((string)Session["Rep"]);
				MyMail.BodyFormat = MailFormat.Html;
				try{SmtpMail.Send(MyMail);}
				catch(Exception e){lbInfo.Text = e.ToString();}
			}
			else
            Utils.MostrarAlerta(Response, "No se ha realizado consulta!");
		}
		
		//Crea filtro para las columnas de la llave primaria de la tabla Maestra
		protected void PopulateFilter()
		{
			int i;
			for(i=1;i<nomColsPK.Length;i++)
			{
				//Revisamos si hace parte de una llave foranea
				if(DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels where relname IN (SELECT constname FROM sysibm.syskeycoluse WHERE tbname='"+table+"' AND colname='"+nomColsPK[i]+"')"))
					MapFkType(table,nomColsPK[i],nomColsPK.Length,i);
				else
				{
					string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+nomColsPK[i]+"' AND tbname='"+table+"'");
					if(tipoDato.IndexOf("CHAR") != -1 || tipoDato.IndexOf("VAR") != -1)
						MapStringType(table,nomColsPK[i],i);
					else if(tipoDato.IndexOf("INT") != -1)
						MapIntType(table,nomColsPK[i],i);
					else if(tipoDato.IndexOf("DECIMAL") != -1 || tipoDato.IndexOf("DOUBLE") != -1)
						MapDecimalType(table,nomColsPK[i],i);
					else if(tipoDato.IndexOf("DATE") != -1)
						MapDateType(table,nomColsPK[i],i);
					else if(tipoDato.IndexOf("TIME") != -1)
						MapTimeType(table,nomColsPK[i],i);
				}
			}
		}
		
		//Agrega popup a los textboxes del filtro
		protected void MakeHelp()
		{
			int i;
			for(i=0;i<tbFilters.Rows.Count;i++)
			{
				if(tbFilters.Rows[i].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
				{
					string sqlQuery = CallModalDialog(i);
					((TextBox)tbFilters.Rows[i].Cells[1].Controls[0]).Attributes.Add("ondblclick","ModalDialog("+tbFilters.Rows[i].Cells[1].Controls[0].ClientID+",'"+sqlQuery+"', new Array());");
				}
			}
		}
		
		//Crea los links a la informacion de la tabla Maestra y a los listados de las tablas referencia
		protected void MakeOptions()
		{
			int i;
			TableRow tr = new TableRow();
			//Agregamos la celda correspondiente a la información del elemento
			TableCell tc1 = new TableCell();
			tc1.HorizontalAlign = HorizontalAlign.Left;
			LinkButton lnkSelf = new LinkButton();
			lnkSelf.Text = "Información del Elemento";
			lnkSelf.CommandArgument = "opcion_0";
			lnkSelf.Command += new CommandEventHandler(MostrarInformacion);
			tc1.Controls.Add(lnkSelf);
			tr.Cells.Add(tc1);
			//Ahora agregamos las celdas de las tablas relacionadas a la tabla maestra
			for(i=0;i<dsTbRefs.Tables[0].Rows.Count;i++)
			{
				TableCell tc2 = new TableCell();
				tc2.HorizontalAlign = HorizontalAlign.Left;
				LinkButton lnkRels = new LinkButton();
				//lnkRels.Text = dsTbRefs.Tables[0].Rows[i][0].ToString()+"   "+DBFunctions.SingleData("SELECT COALESCE(remarks,name) FROM sysibm.systables WHERE name='"+dsTbRefs.Tables[0].Rows[i][0].ToString()+"'");
				lnkRels.Text = DBFunctions.SingleData("SELECT COALESCE(remarks,name) FROM sysibm.systables WHERE name='"+dsTbRefs.Tables[0].Rows[i][0].ToString()+"'");
				lnkRels.CommandArgument = "opcion_"+(i+1).ToString();
				lnkRels.Command += new CommandEventHandler(MostrarInformacion);
				tc2.Controls.Add(lnkRels);
				tr.Cells.Add(tc2);
			}
			tbOpciones.Rows.Add(tr);
		}
		
		//Devuelve el sql del popup de un textbox de los filtros
		protected string CallModalDialog(int i)
		{
            if (table == "MCATALOGOVEHICULO" && nomColsAll.Length > 0)
            {
                string select = "SELECT ";
                string cadenaCols = "";
                string from = " FROM " + table;
                string where = " WHERE ";
                string orderBy = " ORDER BY " + table + "." + nomColsPK[i + 1] + " ASC";
                string sqlQuery = "";
                uint contadorCondicional = 0;

                for (int z = 0; z < nomColsAll.Length - 1; z++)
                {
                    cadenaCols += nomColsAll[z + 1] + " AS " + DBFunctions.SingleData("SELECT COALESCE(remarks,name) FROM sysibm.syscolumns WHERE name='" + nomColsAll[z + 1] + "' AND tbname='" + table + "'").Replace(" ", "_") + ","; //FROM sysibm.syscolumns WHERE name='" + nomColsPK[i + 1] + "' AND tbname='" + table + "'").Replace(" ", "_") + " "; 
                }

                select += nomColsAll[16] + ", ";
                cadenaCols = cadenaCols.Replace("MCAT_VIN AS Identificacion_VIN,", " ");
                if (cadenaCols.EndsWith(",")) select += cadenaCols.Substring(0, cadenaCols.Length - 1);
                select += from; //+ "FROM sysibm.syscolumns WHERE name='" + nomColsPK[i + 1] + "' AND tbname='" + table + "'").Replace(" ", "_") +;
                int j;
                for (j = 0; j < tbFilters.Rows.Count; j++)
                {
                    if (tbFilters.Rows[j].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                    {
                        contadorCondicional += 1;
                        string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='" + nomColsAll[j] + "' AND tbname='" + table + "'");
                        if (tipoDato.IndexOf("CHAR") != -1 || tipoDato.IndexOf("VAR") != -1 || tipoDato.IndexOf("DATE") != -1 || tipoDato.IndexOf("TIME") != -1)
                            where += table + "." + nomColsAll[j] + "=\\''+" + tbFilters.Rows[j].Cells[1].Controls[0].ClientID + ".value+'\\' AND";
                        else
                            where += table + "." + nomColsAll[j] + "='+" + tbFilters.Rows[j].Cells[1].Controls[0].ClientID + ".value+' AND";
                    }
                }
                if (table == "MCATALOGOVEHICULO") sqlQuery = select;
                else if (contadorCondicional == 0)
                    sqlQuery = select + from + orderBy;
                else
                    sqlQuery = select + from + where.Substring(0, where.Length - 3) + orderBy;
                sqlQuery = sqlQuery.Replace("(V.I.N.)", "VIN");
                return sqlQuery;
            }
            else
            {
                string select = "SELECT DISTINCT " + table + "." + nomColsPK[i + 1] + " AS " + DBFunctions.SingleData("SELECT COALESCE(remarks,name) FROM sysibm.syscolumns WHERE name='" + nomColsPK[i + 1] + "' AND tbname='" + table + "'").Replace(" ", "_") + " ";
                string from = " FROM " + table;
                string where = " WHERE ";
                string orderBy = " ORDER BY " + table + "." + nomColsPK[i + 1] + " ASC";
                string sqlQuery = "";
                uint contadorCondicional = 0;
                int j;
                for (j = 0; j < tbFilters.Rows.Count; j++)
                {
                    if (tbFilters.Rows[j].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                    {
                        contadorCondicional += 1;
                        string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='" + nomColsPK[j + 1] + "' AND tbname='" + table + "'");
                        if (tipoDato.IndexOf("CHAR") != -1 || tipoDato.IndexOf("VAR") != -1 || tipoDato.IndexOf("DATE") != -1 || tipoDato.IndexOf("TIME") != -1)
                            where += table + "." + nomColsPK[j + 1] + "=\\''+" + tbFilters.Rows[j].Cells[1].Controls[0].ClientID + ".value+'\\' AND ";
                        else
                            where += table + "." + nomColsPK[j + 1] + "='+" + tbFilters.Rows[j].Cells[1].Controls[0].ClientID + ".value+' AND ";
                    }
                }
                if (contadorCondicional == 0)
                    sqlQuery = select + from + orderBy;
                else
                    sqlQuery = select + from + where.Substring(0, where.Length - 4) + orderBy;
                sqlQuery = sqlQuery.Replace("(V.I.N.)", "VIN");
                return sqlQuery;
            }
		}
		
		//Generar Informacion
		protected void MostrarInformacion(Object sender, CommandEventArgs e)
		{
			int opcion = Convert.ToInt32((e.CommandArgument.ToString().Split('_'))[1]);
			BuildHeader(opcion);
			if(opcion == 0)
				InfoMasterTable();
			else
				InfoRelTable(opcion-1);
			Session["Rep"] = HTMLRender();
			tbHeader.Width = dgReport.Width;
		}
		//Encabezado del reporte
		protected void BuildHeader(int opc)
		{
			int i,j;
			//Primero creamos la fila con el nombre de la empresa
			TableRow tr1 = new TableRow();
			TableCell tc1 = new TableCell();
			tc1.ColumnSpan = 3;
			tc1.HorizontalAlign = HorizontalAlign.Center;
			tc1.Text = DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa")+"<br>Nit : "+DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
			tr1.Cells.Add(tc1);
			tbHeader.Rows.Add(tr1);
			//Ahora creamos la segunda fila con tres celdas
			TableRow tr2 = new TableRow();
			for(i=1;i<=3;i++)
			{
				TableCell tc2 = new TableCell();
				if(i == 1)//En esta celda creamos solamante colocamos la fecha de proceso
					tc2.Text += "Procesado en : "+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
				else if(i == 2)//En esta celda colocamos el comentario de la tabla consultada
				{
					if(opc == 0)
						tc2.Text += "Reporte : Información del elemento de "+DBFunctions.SingleData("SELECT COALESCE(remarks,name) FROM sysibm.systables WHERE name='"+table+"'");
					else
						tc2.Text += "Reporte : "+DBFunctions.SingleData("SELECT COALESCE(remarks,name) FROM sysibm.systables WHERE name='"+dsTbRefs.Tables[0].Rows[opc-1][0].ToString()+"'");
					tc2.HorizontalAlign = HorizontalAlign.Center;
				}
				else if(i == 3)//Aqui colocamos la información del filtro de la busqueda
				{
					for(j=0;j<tbFilters.Rows.Count;j++)
					{
						if(tbFilters.Rows[j].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
							tc2.Text += ((Label)tbFilters.Rows[j].Cells[0].Controls[0]).Text + " "+ ((TextBox)tbFilters.Rows[j].Cells[1].Controls[0]).Text +"<br>";
						else if(tbFilters.Rows[j].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
							tc2.Text += ((Label)tbFilters.Rows[j].Cells[0].Controls[0]).Text + " "+ ((DropDownList)tbFilters.Rows[j].Cells[1].Controls[0]).SelectedItem.Text+"<br>";
					}
					tc2.HorizontalAlign = HorizontalAlign.Right;
				}
				tc2.Width = new Unit("33%");
				tr2.Cells.Add(tc2);
			}
			tbHeader.Rows.Add(tr2);
		}
		
		//Informacion de la tabla padre
		protected bool InfoMasterTable()
		{
			int i,j;
			//Se prepara la tabla del reporte
			DataTable dtReport = new DataTable();
			dtReport.Columns.Add(new DataColumn("CAMPO",System.Type.GetType("System.String")));
			dtReport.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));
			//Se trae un DataSet con los campos de la tabla 
			DataSet da = new DataSet();
			DBFunctions.Request(da,IncludeSchema.NO,"SELECT name, COALESCE(remarks,name) FROM sysibm.syscolumns WHERE tbname='" +table+ "'  ORDER BY COLNO");
            if (table == "MFACTURAPROVEEDOR" || table == "MFACTURACLIENTE")
            {
                DataRow drTotaliza = da.Tables[0].NewRow();
                DataRow drSaldo = da.Tables[0].NewRow();
                drTotaliza[0] = "TOTALIZA_FACT";
                drTotaliza[1] = "TOTAL DOCUMENTO";
                drSaldo[0]    = "SALDO_FACT";
                drSaldo[1]    = "SALDO";
                
                da.Tables[0].Rows.Add(drTotaliza);
                da.Tables[0].Rows.Add(drSaldo);
            }
            for(i=0;i<da.Tables[0].Rows.Count;i++)
			{
				string select = "SELECT ";
				string from = " FROM "+table+",";
				string where = BuildConditional();
				string sqlQuery = "";
				string tipDato = "";
                if (i == 0 && !DBFunctions.RecordExist("SELECT * from " + table + " " + where.Substring(0, where.Length - 3)))
                    return (false);
				if(!DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE relname IN (SELECT constname FROM sysibm.syskeycoluse WHERE tbname='"+table+"' AND colname='"+da.Tables[0].Rows[i][0].ToString()+"')"))
				{
					sqlQuery = select + table + "." + da.Tables[0].Rows[i][0].ToString() + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
					tipDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+da.Tables[0].Rows[i][0].ToString()+"' AND tbname='"+table+"'");
				}
				else
				{
					string nombreTabla = table;
					string nombreCampo = da.Tables[0].Rows[i][0].ToString();
					while(DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')").Trim() != "")
					{
						string tablaReferencia = DBFunctions.SingleData("SELECT reftbname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')");
						DataSet fkcolname = new DataSet();
						DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"' AND fkcolnames LIKE '%"+nombreCampo+"%') ORDER BY colseq");
						string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name IN (SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"')");
						string[] rlcolnames = rlcolname.Split('+');
						if(fkcolname.Tables[0].Rows.Count == rlcolnames.Length-1)
						{
							for(j=0;j<fkcolname.Tables[0].Rows.Count;j++)
							{
								if(from.IndexOf(tablaReferencia) == -1)
									from += " "+tablaReferencia+",";
								where += " "+nombreTabla+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+tablaReferencia+"."+rlcolnames[j+1].Trim()+" AND";
								if(nombreCampo == fkcolname.Tables[0].Rows[j][0].ToString().Trim())
									nombreCampo = rlcolnames[j+1].Trim();
							}
						}
						else
						{
							if(from.IndexOf(tablaReferencia) == -1)
								from += " "+tablaReferencia+",";
                            try
                            {
                                where += " " + nombreTabla + "." + fkcolname.Tables[0].Rows[0][0].ToString().Trim() + "=" + tablaReferencia + "." + rlcolnames[1].Trim() + " AND";
                                nombreCampo = rlcolnames[1].Trim();
                            }
                            catch (Exception e) //Cuando sysindexes (tabla sistema DB2) no tiene el registro de la llave foranea presente en refkeyname (tabla sistema DB2).
                            {
                                where += " " + nombreTabla + "." + fkcolname.Tables[0].Rows[0][0].ToString().Trim() + "=" + tablaReferencia + "." + fkcolname.Tables[0].Rows[0][0].ToString().Trim() + " AND";
                                nombreCampo = fkcolname.Tables[0].Rows[0][0].ToString().Trim();
                            }
						}
						nombreTabla = tablaReferencia;
					}
					if(nombreTabla == "MNIT")
					{
						string tipoNit= DBFunctions.SingleData("SELECT tnit_tiponit "+ from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3)+" ");
						if(tipoNit=="N")
							sqlQuery = select + "MNIT.mnit_nit CONCAT ' ' CONCAT '-' CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_nombres " + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
						else
							sqlQuery = select + "MNIT.mnit_nit CONCAT ' ' CONCAT '-' CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.mnit_apellido2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.MNIT_NOMBRE2,'') " + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
						tipDato = "CHAR";
					}
					else
					{
						sqlQuery = select + nombreTabla + "." + DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+nombreTabla+"' AND colno=1") + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
						tipDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE tbname='"+nombreTabla+"' AND colno=1");
					}
				}
				//Creación y adición de la fila a la tabla
				DataRow fila = dtReport.NewRow();
				fila[0] = da.Tables[0].Rows[i][1].ToString();
                if (da.Tables[0].Rows[i][0].ToString() == "TOTALIZA_FACT" || da.Tables[0].Rows[i][0].ToString() == "SALDO_FACT")
                {
                    if (da.Tables[0].Rows[i][0].ToString() == "SALDO_FACT")
                    {
                        double total = 0;
                        total = Convert.ToDouble(dtReport.Rows[10][1]) + Convert.ToDouble(dtReport.Rows[11][1]) + Convert.ToDouble(dtReport.Rows[12][1]) + Convert.ToDouble(dtReport.Rows[13][1]) - Convert.ToDouble(dtReport.Rows[09][1]) - Convert.ToDouble(dtReport.Rows[14][1]);
                        fila[1] = total.ToString("C");
                    }
                    else
                    {
                        double total = 0;
                        total = Convert.ToDouble(dtReport.Rows[10][1]) + Convert.ToDouble(dtReport.Rows[11][1]) + Convert.ToDouble(dtReport.Rows[12][1]) + Convert.ToDouble(dtReport.Rows[13][1]) - Convert.ToDouble(dtReport.Rows[14][1]);
                        fila[1] = total.ToString("C");
                    }
                }
                
                else
                {
                    if (tipDato.IndexOf("DECIMAL") != -1 || tipDato.IndexOf("DOUBLE") != -1)
                        try { fila[1] = Convert.ToDouble(DBFunctions.SingleData(sqlQuery).Trim()).ToString("N"); }
                        catch { }
                    else if (tipDato.IndexOf("DATE") != -1)
                        try { fila[1] = Convert.ToDateTime(DBFunctions.SingleData(sqlQuery).Trim()).ToString("yyyy-MM-dd"); }
                        catch { }
                    else if (tipDato.IndexOf("TIMESTMP") != -1)
                        try { fila[1] = Convert.ToDateTime(DBFunctions.SingleData(sqlQuery).Trim()).ToString("yyyy-MM-dd hh:mm:ss"); }
                        catch { }
                    else
                        fila[1] = DBFunctions.SingleData(sqlQuery);
                }
				dtReport.Rows.Add(fila);
			}
			dgReport.DataSource = dtReport;
			dgReport.DataBind();
            return (true);
		}
		
		//Listado de una table de referencia
		protected void InfoRelTable(int numTabla)
		{
			int i;
			string tableRef = dsTbRefs.Tables[0].Rows[numTabla][0].ToString();
			string select = "SELECT ";
			string from = " FROM "+tableRef+","+table+" ";
			string where = BuildConditional();
			DataSet dsColsRels = new DataSet();
			//Debemos determinar si existe mas de una relacion a esta tabla, si esa asi simplemente tomamos como la relacion principal la primera que se encuentra
			if(Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(COUNT(*),0) FROM sysibm.sysrels WHERE tbname='"+tableRef+"' AND REFTBNAME='"+table+"'").Trim()) == 1)
			{
				DBFunctions.Request(dsColsRels,IncludeSchema.NO,"SELECT DISTINCT colname, colseq FROM sysibm.syskeycoluse where constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+tableRef+"' AND REFTBNAME='"+table+"') order by colseq asc;"+
					"SELECT name, remarks, coltype, colno FROM sysibm.syscolumns WHERE tbname='"+tableRef+"' AND name NOT IN (SELECT colname AS COLRELACION FROM sysibm.syskeycoluse where constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+tableRef+"' AND REFTBNAME='"+table+"')) ORDER BY colno ASC;");
			}
			else
			{
				//Aqui solo traemos la relacion que primero encuentre
				string relname = DBFunctions.SingleData("SELECT relname FROM sysibm.sysrels WHERE tbname='"+tableRef+"' AND REFTBNAME='"+table+"'").Trim();
				DBFunctions.Request(dsColsRels,IncludeSchema.NO,"SELECT DISTINCT colname, colseq FROM sysibm.syskeycoluse where constname = '"+relname+"' order by colseq asc;"+
					"SELECT name, remarks, coltype, colno FROM sysibm.syscolumns WHERE tbname='"+tableRef+"' AND name NOT IN (SELECT colname AS COLRELACION FROM sysibm.syskeycoluse where constname = '"+relname+"') ORDER BY colno ASC;");
			}
			for(i=0;i<dsColsRels.Tables[1].Rows.Count;i++)
				select += tableRef+"."+dsColsRels.Tables[1].Rows[i][0].ToString()+",";
            for (i = 0; i < dsColsRels.Tables[0].Rows.Count; i++)
            {
                try
                {
                    if(tableRef == "DINVENTARIOFISICOLECTOR") where += " " + tableRef + "." + "MITE_CODIGO" + "=" + table + "." + nomColsPK[i + 1] + " AND";  //PARA LA TABLA DINVENTARIOFISICOLECTOR
                    else where += " " + tableRef + "." + dsColsRels.Tables[0].Rows[i][0].ToString() + "=" + table + "." + nomColsPK[i + 1] + " AND";
                }
                catch { } // Aquí entra cuando el índice queda fuera de rango, Para evitar que mande pantalla amarilla o.o
            }
			string sqlQuery = select.Substring(0,select.Length-1) + from + where.Substring(0,where.Length-3);
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,sqlQuery);
            try
            {
                BindTabRel(ds.Tables[0], dsColsRels.Tables[1], tableRef);
            }
            catch { }
			//if(ds.Tables.Count == 0)
			//	lbInfo.Text = sqlQuery+"<br><br><br>"+DBFunctions.exceptions;
			//else
			//	//lbInfo.Text = sqlQuery;
			//	BindTabRel(ds.Tables[0],dsColsRels.Tables[1],tableRef);
		}
		
		protected void BindTabRel(DataTable dtQuery, DataTable dtCols,string tableRef)
		{
			int i,j;
			DataTable dtReport = new DataTable();
			for(i=0;i<dtCols.Rows.Count;i++)
				dtReport.Columns.Add(new DataColumn(dtCols.Rows[i][1].ToString(),System.Type.GetType("System.String")));
			ArrayList access = new ArrayList();
			BuildAccessRelsTables(dtCols, tableRef ,ref access);
			for(i=0;i<dtQuery.Rows.Count;i++)
			{
				DataRow fila = dtReport.NewRow();
				for(j=0;j<dtCols.Rows.Count;j++)
				{
					if(!DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE relname IN (SELECT constname FROM sysibm.syskeycoluse WHERE tbname='"+tableRef+"' AND colname='"+dtCols.Rows[j][0].ToString()+"')"))
					{
						if(dtCols.Rows[j][2].ToString().IndexOf("DECIMAL") != -1 || dtCols.Rows[j][2].ToString().IndexOf("DOUBLE") != -1)
							try{fila[j] = Convert.ToDouble(dtQuery.Rows[i][j]).ToString("N");}
							catch{fila[j] = dtQuery.Rows[i][j].ToString();}
						else if(dtCols.Rows[j][2].ToString().IndexOf("DATE") != -1)
							try{fila[j] = Convert.ToDateTime(dtQuery.Rows[i][j]).ToString("yyyy-MM-dd");}
							catch{fila[j] = dtQuery.Rows[i][j].ToString();}
						else if(dtCols.Rows[j][2].ToString().IndexOf("TIMESTMP") != -1)
							try{fila[j] = Convert.ToDateTime(dtQuery.Rows[i][j]).ToString("yyyy-MM-dd hh:mm:ss");}
							catch{fila[j] = dtQuery.Rows[i][j].ToString();}
						else
							fila[j] = dtQuery.Rows[i][j].ToString();
					}
					else
					{
						if(dtQuery.Rows[i][j].ToString() != "")
						{
							if(access[j].ToString().Trim() != "NINGUNA")
							{
								string sqlQuery = access[j].ToString().Replace("@",dtQuery.Rows[i][j].ToString());
								fila[j] = DBFunctions.SingleData(sqlQuery);
							}
							else
								fila[j] = dtQuery.Rows[i][j].ToString();
						}
					}
				}
				dtReport.Rows.Add(fila);
			}
			dgReport.DataSource = dtReport;
			dgReport.DataBind();
			for(i=0;i<dgReport.Items.Count;i++)
			{
				for(j=0;j<dtQuery.Columns.Count;j++)
				{
					if(dtQuery.Columns[j].DataType.ToString() == "System.Decimal" || dtQuery.Columns[j].DataType.ToString() == "System.Double" || dtQuery.Columns[j].DataType.ToString() == "System.Int32")
						dgReport.Items[i].Cells[j].HorizontalAlign = HorizontalAlign.Right;
				}
			}
		}
		
		protected void BuildAccessRelsTables(DataTable dtCols, string tableRef ,ref ArrayList access)
		{
			int i,j;
			for(i=0;i<dtCols.Rows.Count;i++)
			{
				string sqlQuery = "";
				if(!DBFunctions.RecordExist("SELECT * FROM sysibm.sysrels WHERE relname IN (SELECT constname FROM sysibm.syskeycoluse WHERE tbname='"+tableRef+"' AND colname='"+dtCols.Rows[i][0].ToString()+"')"))
					access.Add(sqlQuery);
				else
				{
					string from = " FROM ";
					string where = " WHERE ";
					if(dtCols.Rows[i][2].ToString().IndexOf("DATE") != -1 || dtCols.Rows[i][2].ToString().IndexOf("VAR") != -1 || dtCols.Rows[i][2].ToString().IndexOf("CHAR") != -1 || dtCols.Rows[i][2].ToString().IndexOf("TIME") != -1)
						where += tableRef+"."+dtCols.Rows[i][0].ToString()+"='@' AND";
					else
						where += tableRef+"."+dtCols.Rows[i][0].ToString()+"=@ AND";
					///////////////////////////////////////////////////////////////
					string nombreTabla = tableRef;
					string nombreCampo = dtCols.Rows[i][0].ToString();
                    //try{Convert.ToInt32(DBFunctions.SingleData("SELECT colseq FROM SYSIBM.SYSKEYCOLUSE WHERE constname = (SELECT relname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')) AND colname='"+nombreCampo+"'").Trim());}catch{lbInfo.Text += "<br><br>SELECT colseq FROM SYSIBM.SYSKEYCOLUSE WHERE constname = (SELECT relname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')) AND colname='"+nombreCampo+"'";}
                    try
                    {
                        if (Convert.ToInt32(DBFunctions.SingleData("SELECT DISTINCT colseq FROM SYSIBM.SYSKEYCOLUSE WHERE constname = (SELECT DISTINCT relname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='" + nombreTabla + "' AND colname='" + nombreCampo + "')) AND colname='" + nombreCampo + "'").Trim()) > 1 && (dtCols.Rows[i][2].ToString().IndexOf("INT") != -1 || dtCols.Rows[i][2].ToString().IndexOf("DECIMAL") != -1 || dtCols.Rows[i][2].ToString().IndexOf("DOUBLE") != -1))
                            sqlQuery = "NINGUNA";   
					else
					{
						while(DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')").Trim() != "")
						{
							string tablaReferencia = DBFunctions.SingleData("SELECT reftbname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')");
							DataSet fkcolname = new DataSet();
							DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"') ORDER BY colseq");
							string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name IN (SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"')");
							string[] rlcolnames = rlcolname.Split('+');
							if(fkcolname.Tables[0].Rows.Count == rlcolnames.Length-1)
							{
								for(j=0;j<fkcolname.Tables[0].Rows.Count;j++)
								{
									if(from.IndexOf(tablaReferencia) == -1)
										from += " "+tablaReferencia+",";
									where += " "+nombreTabla+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+tablaReferencia+"."+rlcolnames[j+1].Trim()+" AND";
									if(nombreCampo == fkcolname.Tables[0].Rows[j][0].ToString().Trim())
										nombreCampo = rlcolnames[j+1].Trim();
								}
							}
							else
							{
								if(from.IndexOf(tablaReferencia) == -1)
									from += " "+tablaReferencia+",";
								where += " "+nombreTabla+"."+fkcolname.Tables[0].Rows[0][0].ToString().Trim()+"="+tablaReferencia+"."+rlcolnames[1].Trim()+" AND";
								nombreCampo = rlcolnames[1].Trim();
							}
							nombreTabla = tablaReferencia;
						}
						////////////////////////////////////////////////////////////// 
						from += tableRef+",";
						if(nombreTabla == "MNIT")
							sqlQuery = "SELECT "+
								"CASE WHEN MNIT.TNIT_TIPONIT = 'N' THEN MNIT.MNIT_NIT CONCAT '-' CONCAT MNIT.MNIT_DIGITO CONCAT ' - ' CONCAT MNIT.MNIT_APELLIDOS "+
								"ELSE MNIT.MNIT_NIT CONCAT ' - ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT.MNIT_APELLIDO2 CONCAT ' ' CONCAT MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT MNIT.MNIT_NOMBRE2 END "+
								from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
						else
						{
							if(nombreTabla == "MCUENTA")
								sqlQuery = "SELECT MCUENTA.MCUE_CODIPUC concat ' - ' concat MCUENTA.MCUE_NOMBRE " + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
							else
							{
								if(nombreTabla == "PCENTROCOSTO")
									sqlQuery = "SELECT PCENTROCOSTO.PCEN_CODIGO CONCAT ' - ' CONCAT PCENTROCOSTO.PCEN_NOMBRE " + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
								else
									sqlQuery = "SELECT "+ nombreTabla + "." + DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+nombreTabla+"' AND colno=1") + from.Substring(0,from.Length-1) + where.Substring(0,where.Length-3);
							}
						}
					}
                    }
                    catch (Exception)
                    {
                    }
                    access.Add(sqlQuery);
				}
			}
		}
		
		protected string BuildConditional()
		{
			int i;
			string where = " WHERE ";
			for(i=0;i<tbFilters.Rows.Count;i++)
			{
				string valorControl = "";
				if(tbFilters.Rows[i].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
					valorControl = ((TextBox)tbFilters.Rows[i].Cells[1].Controls[0]).Text;
				else if(tbFilters.Rows[i].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
					valorControl = ((DropDownList)tbFilters.Rows[i].Cells[1].Controls[0]).SelectedValue;
				string tipoDato = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+nomColsPK[i+1]+"' AND tbname='"+table+"'");
				if(tipoDato.IndexOf("CHAR") != -1 || tipoDato.IndexOf("VAR") != -1 || tipoDato.IndexOf("DATE") != -1 || tipoDato.IndexOf("TIME") != -1)
					where += " "+table+"."+nomColsPK[i+1]+"='"+valorControl+"' AND";
				else
					where += " "+table+"."+nomColsPK[i+1]+"="+valorControl+" AND";
			}
			return where;
		}
		
		#region Map Types
		protected void MapFkType(string tbName, string colName, int colsPK, int i)
		{
			Label lb = new Label();
			lb.Text = DBFunctions.SingleData("SELECT coalesce(remarks,name) FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'")+" : ";
			DropDownList ddl = PutDropDownList(tbName,colName);
			ddl.ID = "control_" + i.ToString();
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(ddl);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbFilters.Rows.Add(tr);
		}
		
		protected void MapStringType(string tbName, string colName, int i)
		{
			Label lb = new Label();
			lb.Text = DBFunctions.SingleData("SELECT coalesce(remarks,name) FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'")+" : ";
			int length = Convert.ToInt32(DBFunctions.SingleData("SELECT length FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'"));
			TextBox tb = PutTextBox(length);
			tb.ID = "control_" + i.ToString();
			tb.MaxLength = length;
			RequiredFieldValidator rfv = RequiredValidator(tb.ID);
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rfv);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbFilters.Rows.Add(tr);
		}
		
		protected void MapIntType(string tbName, string colName, int i)
		{
			Label lb = new Label();
			lb.Text = DBFunctions.SingleData("SELECT coalesce(remarks,name) FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'")+" : ";
			TextBox tb = PutTextBox(8);
			tb.ID = "control_" + i.ToString();
			tb.CssClass = "AlineacionDerecha";
			TableCell tc1 = new TableCell();
			TableCell tc2 = new TableCell();
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9]+");
			RequiredFieldValidator rfv = RequiredValidator(tb.ID);
			tc1.Controls.Add(lb);
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rev);
			tc2.Controls.Add(rfv);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbFilters.Rows.Add(tr);
		}
		
		protected void MapDecimalType(string tbName, string colName, int i)
		{
			Label lb = new Label();
			lb.Text = DBFunctions.SingleData("SELECT coalesce(remarks,name) FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'")+" : ";
			TextBox tb = PutTextBox(20);
			tb.ID = "control_" + i.ToString();
			TableCell tc1 = new TableCell();
			TableCell tc2 = new TableCell();
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9\\,\\.]+");
			RequiredFieldValidator rfv = RequiredValidator(tb.ID);
			tb.CssClass = "AlineacionDerecha";
			tb.Attributes.Add("onkeyup","NumericMaskE(this,event)");
			tc1.Controls.Add(lb);
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rev);
			tc2.Controls.Add(rfv);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbFilters.Rows.Add(tr);
		}
		
		protected void MapDateType(string tbName, string colName, int i)
		{
			Label lb = new Label();
			lb.Text = DBFunctions.SingleData("SELECT coalesce(remarks,name) FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'")+" : ";
			TextBox tb = PutTextBox(20);
			tb.ID = "control_" + i.ToString();
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}/[0-9]{2}/[0-9]{4}");
			RequiredFieldValidator rfv = RequiredValidator(tb.ID);
			IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
			tb.Text = DateTime.Now.GetDateTimeFormats('d', culture)[0];
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rev);
			tc2.Controls.Add(rfv);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbFilters.Rows.Add(tr);
		}
		
		protected void MapTimeType(string tbName, string colName, int i)
		{
			Label lb = new Label();
			lb.Text = DBFunctions.SingleData("SELECT coalesce(remarks,name) FROM sysibm.syscolumns WHERE tbname='"+tbName+"' AND name='"+colName+"'")+" : ";
			TextBox tb = PutTextBox(8);
			tb.ID = "control_" + i.ToString();
			tb.Text = DateTime.Now.TimeOfDay.ToString().Substring(0,8);
			RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}:[0-9]{2}:[0-9]{2}");
			RequiredFieldValidator rfv = RequiredValidator(tb.ID);
			TableCell tc1 = new TableCell();
			tc1.Controls.Add(lb);
			TableCell tc2 = new TableCell();
			tc2.Controls.Add(tb);
			tc2.Controls.Add(rev);
			tc2.Controls.Add(rfv);
			TableRow tr = new TableRow();
			tr.Cells.Add(tc1);
			tr.Cells.Add(tc2);
			tbFilters.Rows.Add(tr);
		}
		
		#endregion Map Types
		protected DropDownList PutDropDownList(string tbName, string colName)
		{
			DropDownList ddl = new DropDownList();
			string select = "SELECT DISTINCT "+tbName+"."+colName+",";
			string from = " FROM "+tbName+",";
			string where = " WHERE ";
			string orderBy = " ORDER BY "+tbName+"."+colName;
			string nombreTabla = tbName;
			string nombreCampo = colName;
			int contadorJoin = 0;
			while(DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')").Trim() != "")
			{
				contadorJoin += 1;
				string tablaReferencia = DBFunctions.SingleData("SELECT reftbname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='"+nombreTabla+"' AND colname='"+nombreCampo+"')");
				DataSet fkcolname = new DataSet();
				DBFunctions.Request(fkcolname,IncludeSchema.NO,"SELECT colname FROM sysibm.syskeycoluse WHERE constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"') AND tbname='"+nombreTabla+"' ORDER BY colseq");
				string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name IN (SELECT refkeyname FROM sysibm.sysrels WHERE tbname='"+nombreTabla+"' AND reftbname='"+tablaReferencia+"')");
				string[] rlcolnames = rlcolname.Split('+');
				if(fkcolname.Tables[0].Rows.Count == rlcolnames.Length-1)
				{
					for(int j=0;j<fkcolname.Tables[0].Rows.Count;j++)
					{
						if(from.IndexOf(tablaReferencia) == -1)
							from += " "+tablaReferencia+",";
						where += " "+nombreTabla+"."+fkcolname.Tables[0].Rows[j][0].ToString().Trim()+"="+tablaReferencia+"."+rlcolnames[j+1].Trim()+" AND";
						if(nombreCampo == fkcolname.Tables[0].Rows[j][0].ToString().Trim())
							nombreCampo = rlcolnames[j+1].Trim();
					}
				}
				nombreTabla = tablaReferencia;
			}
			string sqlQuery = "";
			switch (nombreTabla)
			{
				case "MNIT":
					select += "MNIT.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' - ' CONCAT MNIT.mnit_nombres,";
					break;
				case "MCUENTA":
					select += "MCUENTA.mcue_codipuc CONCAT ' - ' CONCAT MCUENTA.mcue_nombre,";
					break;
				case "PIVA":
					select += "SUBSTR(CAST(PIVA.piva_porciva AS character(5)),2,LENGTH(CAST(PIVA.piva_porciva AS character(5)))-1) CONCAT '- ' CONCAT PIVA.piva_decreto,";
					break;
				case "PDOCUMENTO":
					select += "PDOCUMENTO.pdoc_codigo concat ' - ' concat PDOCUMENTO.pdoc_nombre descripcion,";
					break;
				case "PLINEAITEM":
					sqlQuery = "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem ORDER BY plin_codigo";
					break;
				default:
					select += nombreTabla+"."+DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+nombreTabla+"' AND colno=1")+",";
					break;
			}
			//CATALOGO
			if(select.IndexOf("PCATALOGOVEHICULO.PCAT_DESCRIPCION")>0)
				select=select.Replace("PCATALOGOVEHICULO.PCAT_DESCRIPCION","PCATALOGOVEHICULO.PCAT_CODIGO concat '  -  ' concat PCATALOGOVEHICULO.PCAT_DESCRIPCION");
			if(nombreTabla != "PLINEAITEM")
			{
				if(where.Equals(" WHERE "))where="   ";
				if(contadorJoin > 0)
					sqlQuery = select.Substring(0,select.Length-1)+from.Substring(0,from.Length-1)+where.Substring(0,where.Length-3)+orderBy;
				else
					sqlQuery = select.Substring(0,select.Length-1)+from.Substring(0,from.Length-1)+orderBy;
			}
			DataSet tempDS = new DataSet();
			DBFunctions.Request(tempDS, IncludeSchema.YES, sqlQuery);
			try
			{
				ddl.DataSource = tempDS.Tables[0];
				if(nombreTabla != "PLINEAITEM")
				{
					if(tempDS.Tables[0].PrimaryKey.Length > 0)
						ddl.DataValueField = tempDS.Tables[0].PrimaryKey[0].ToString();
					if(tempDS.Tables[0].Columns.Count > 1)
					{
						ddl.DataValueField = tempDS.Tables[0].Columns[0].ToString();
						ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
					}
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
			return ddl;
		}
		
		protected TextBox PutTextBox(int l)
		{
			TextBox tb = new TextBox();
			if(l < 5)
				tb.Width = new Unit(25);
			if(l <= 10 && l >= 5)
				tb.Width = new Unit(l*11);
			if(l<=100 && l > 10)
				tb.Width = new Unit(l*7);
			if(l > 100 && l < 5000)
			{
				tb.Width = new Unit(600);
				tb.Height = new Unit(60);
				tb.TextMode = TextBoxMode.MultiLine;
			}
			if(l >= 5000)
			{
				tb.Width = new Unit(400);
				tb.Height = new Unit(120);
				tb.TextMode = TextBoxMode.MultiLine;
			}
			return tb;
		}
		
		protected RequiredFieldValidator RequiredValidator(string controlID)
		{
			RequiredFieldValidator rfv = new RequiredFieldValidator();
			rfv.ErrorMessage = "campo requerido";
			rfv.ControlToValidate = controlID;
			return rfv;
		}
		
		protected RegularExpressionValidator Validator(string controlID, string regex)
		{
			RegularExpressionValidator rev = new RegularExpressionValidator();
			rev.ErrorMessage = "campo no vlido";
			rev.ControlToValidate = controlID;
			rev.ValidationExpression = regex;
			return rev;
		}
		
		protected string HTMLRender()
		{
			StringBuilder SB= new StringBuilder();
			StringWriter SW= new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			plhReport.RenderControl(htmlTW);
			return SB.ToString();
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

        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            if(!InfoMasterTable())
            Utils.MostrarAlerta(Response, "Información incorrecta!");
        }
	}
}
