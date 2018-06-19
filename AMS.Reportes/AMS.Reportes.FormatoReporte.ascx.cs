using System;
using System.Timers;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using WebChart;
using AMS.CriptoServiceProvider;

namespace AMS.Reportes
{
    public partial class Formato : System.Web.UI.UserControl
    {
        #region Variables
        //tablas
        protected DataSet dsFilters = new DataSet();
        protected Table tbFilters = new Table();
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        public int kks = 0;

        #endregion

        #region Otros
        #region Generar Filtros
        protected void GetSchemaFilters(string idReport)
        {//busqueda
            dsFilters = DBFunctions.Request(dsFilters, IncludeSchema.NO, "SELECT sfil_id, sfil_contasoc, sfil_tabla, sfil_campo, sfil_datocomp1, sfil_tipcompa, sfil_datocomp2, sfil_etiqueta, sfil_valint FROM sfiltro WHERE srep_id=" + idReport + " order by sfil_id ASC");
            //																		0		1				2			3			4				5		       6              7                8	
        }

        protected void SetFormFilters()
        {
            for (int i = 0; i < dsFilters.Tables[0].Rows.Count; i++)
            {
                if (dsFilters.Tables[0].Rows[i][1].ToString() == "DropDownList")
                    MapDdlField(dsFilters.Tables[0].Rows[i]);
                else if (dsFilters.Tables[0].Rows[i][1].ToString() == "TextBox")
                    MapTbField(dsFilters.Tables[0].Rows[i]);
            }
            this.SetButtonGenerate();
            filterHolder.Controls.Add(tbFilters);
        }

        protected void SetButtonGenerate()
        {
            //genera reporte
            Button filterButton = new Button();
            filterButton.Text = "Generar Reporte";
            filterButton.Click += new EventHandler(this.MakeReport);
            TableCell tc = new TableCell();
            tc.Controls.Add(filterButton);
            if (tbFilters.Rows.Count != 0)
                tbFilters.Rows[tbFilters.Rows.Count - 1].Cells.Add(tc);
            else
            {
                TableRow r = new TableRow();
                r.Cells.Add(tc);
                tbFilters.Rows.Add(r);
            }
            chtGrafica.Visible = false;
        }

        protected void MapDdlField(DataRow dr)
        {
            Label lb = PutLabel(dr[7].ToString(), System.Convert.ToInt32(dr[0]));
            DropDownList ddl = PutDropDownList(dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[0]), dr[8].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            tc2.Controls.Add(ddl);
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbFilters.Rows.Add(tr);
        }

        protected void MapTbField(DataRow dr)
        {
            Label lb = PutLabel(dr[7].ToString(), System.Convert.ToInt32(dr[0]));
            TextBox tb = PutTextBox(dr[2].ToString(), dr[3].ToString(), System.Convert.ToInt32(dr[0]));
            TableCell tc1 = new TableCell();
            tc1.Controls.Add(lb);
            TableCell tc2 = new TableCell();
            tc2.Controls.Add(tb);
            string dataType = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE tbname='" + dr[2].ToString().ToUpper() + "' AND name='" + dr[3].ToString().ToUpper() + "'");
            if (dataType == "INTEGER " || dataType == "SMALLINT " || dataType == "BIGINT ")
            {
                RegularExpressionValidator rev = Validator(tb.ID, "[0-9]+");
                tc2.Controls.Add(rev);
            }
            else if (dataType == "DECIMAL " || dataType == "DOUBLE " || dataType == "REAL ")
            {
                RegularExpressionValidator rev = Validator(tb.ID, "[0-9\\.\\-]+");
                tc2.Controls.Add(rev);
            }
            else if (dataType == "DATE ")
            {
                RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{4}-[0-9]{2}-[0-9]{2}");
                tc2.Controls.Add(rev);
            }
            else if (dataType == "TIME ")
            {
                RegularExpressionValidator rev = Validator(tb.ID, "[0-9]{2}:[0-9]{2}:[0-9]{2}");
                tc2.Controls.Add(rev);
            }
            TableRow tr = new TableRow();
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tbFilters.Rows.Add(tr);
        }

        protected Label PutLabel(string text, int pos)
        {
            Label lb = new Label();
            lb.ID = "label_" + pos.ToString();
            lb.Text = text + ": ";
            return lb;
        }

        protected TextBox PutTextBox(string tableRefOrigen, string fieldNameOrigen, int pos)
        {
            TextBox tb = new TextBox();
            tb.ID = "control_" + pos.ToString();
            int l = 0;
            string dataType = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE tbname='" + tableRefOrigen.ToUpper() + "' AND name='" + fieldNameOrigen.ToUpper() + "'").Trim();
            if (dataType == "VARCHAR" || dataType == "CHAR")
                l = System.Convert.ToInt32(DBFunctions.SingleData("SELECT length FROM sysibm.syscolumns WHERE tbname='" + tableRefOrigen.ToUpper() + "' AND name='" + fieldNameOrigen.ToUpper() + "'"));
            else if (dataType == "INTEGER " || dataType == "SMALLINT " || dataType == "BIGINT ")
                l = 8;
            else if (dataType == "DECIMAL " || dataType == "DOUBLE " || dataType == "REAL ")
                l = 20;
            else if (dataType == "DATE ")
            {
                tb.Attributes.Add("onkeyup", "DateMask(this);");
                l = 20;
            }
            else if (dataType == "TIME ")
                l = 8;
            l = 0;
            if (l < 5)
                tb.Width = new Unit(120);
            if (l <= 10 && l >= 5)
                tb.Width = new Unit(l * 30);
            if (l <= 100 && l > 10)
                tb.Width = new Unit(l * 40);
            if (l > 100 && l < 5000)
            {
                tb.Width = new Unit(600);
                tb.Height = new Unit(60);
                tb.TextMode = TextBoxMode.MultiLine;
            }
            if (l >= 5000)
            {
                tb.Width = new Unit(400);
                tb.Height = new Unit(120);
                tb.TextMode = TextBoxMode.MultiLine;
            }
            return tb;
        }

        protected DropDownList PutDropDownList(string tableRefOrigen, string fieldNameOrigen, int pos, string indicadorValor, string dato1, string comparador, string dato2)
        {
            string[] sepRef = FindRefTable(tableRefOrigen, fieldNameOrigen, indicadorValor).Split('*');

            DropDownList ddl = new DropDownList();
            if (sepRef[0] != "" || comparador == "")
            {
                DataSet tempDS = new DataSet();
                if (sepRef[0] != "MNIT" || comparador == "")
                {
                    sepRef[1] = sepRef[1].Replace("?", "'");
                    if (dato1 == "" && dato2 == "")
                    {
                        tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT " + sepRef[1] + " FROM " + sepRef[0] + " ORDER BY " + sepRef[1] + "");
                    }
                    else if (dato1 != "" && dato2 == "")
                    {
                        dato1 = dato1.Replace("?", "'");
                        tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT " + sepRef[1] + " FROM " + sepRef[0] + " " + dato1);
                    }
                    else
                    {
                        tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT " + sepRef[1] + " FROM " + sepRef[0] + " WHERE " + dato1 + comparador + " '" + dato2 + "' ORDER BY " + sepRef[1]);
                    }
                }
                else
                    tempDS = DBFunctions.Request(tempDS, IncludeSchema.YES, "SELECT mnit_nit,  mnit_nit CONCAT ' - ' CONCAT mnit_apellidos CONCAT ' - ' CONCAT mnit_nombres FROM " + sepRef[0] + " ORDER BY mnit_nit");

                ddl.ID = "control_" + pos.ToString();
                try
                {
                    ddl.DataSource = tempDS.Tables[0];
                    if (tempDS.Tables[0].PrimaryKey.Length > 0)
                        ddl.DataValueField = tempDS.Tables[0].PrimaryKey[0].ToString();
                    if (tempDS.Tables[0].Columns.Count > 1)
                        ddl.DataTextField = tempDS.Tables[0].Columns[1].ToString();
                    ddl.DataBind();
                }
                catch (Exception e)
                {
                    lbInfo.Text += e.ToString() + "<br>";
                }
                tempDS.Clear();
                return ddl;
            }
            else
                lbInfo.Text += "<br>Error " + tableRefOrigen + " " + fieldNameOrigen;
            return ddl;
        }

        protected string FindRefTable(string tableRefOrigen, string fieldNameOrigen, string indicadorValor)
        {
            string tableRef = "";
            string tabla = tableRefOrigen, campo = fieldNameOrigen;
            while (DBFunctions.SingleData("SELECT * FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='" + tabla + "' AND colname='" + campo + "')").Trim() != "")
            {
                string tablaReferencia = DBFunctions.SingleData("SELECT reftbname FROM SYSIBM.SYSRELS WHERE relname IN (SELECT constname FROM SYSIBM.SYSKEYCOLUSE WHERE tbname='" + tabla + "' AND colname='" + campo + "')");
                DataSet fkcolname = new DataSet();
                DBFunctions.Request(fkcolname, IncludeSchema.NO, "SELECT colname FROM sysibm.syskeycoluse WHERE constname IN (SELECT relname FROM sysibm.sysrels WHERE tbname='" + tabla + "' AND reftbname='" + tablaReferencia + "') ORDER BY colseq");
                string rlcolname = DBFunctions.SingleData("SELECT colnames FROM sysibm.sysindexes WHERE name IN (SELECT refkeyname FROM sysibm.sysrels WHERE tbname='" + tabla + "' AND reftbname='" + tablaReferencia + "')");
                string[] rlcolnames = rlcolname.Split('+');
                if (fkcolname.Tables[0].Rows.Count == rlcolnames.Length - 1)
                {
                    for (int j = 0; j < fkcolname.Tables[0].Rows.Count; j++)
                    {
                        if (campo == fkcolname.Tables[0].Rows[j][0].ToString().Trim())
                            campo = rlcolnames[j + 1].Trim();
                    }
                }
                tabla = tablaReferencia;
            }
            if (indicadorValor == "N")
                tableRef = tabla + "*" + campo + "*" + campo;
            else if (indicadorValor == "S")
                tableRef = tabla + "*" + campo + "*" + DBFunctions.SingleData("SELECT name FROM SYSIBM.SYSCOLUMNS WHERE tbname='" + tabla + "' AND colno=1");
            return tableRef;
        }

        protected RegularExpressionValidator Validator(string controlID, string regex)
        {
            RegularExpressionValidator rev = new RegularExpressionValidator();
            rev.ErrorMessage = "campo no válido";
            rev.ControlToValidate = controlID;
            rev.ValidationExpression = regex;
            return rev;
        }
        #endregion Generar Filtros

        //protected void Header(string nombReport, DataTable filts)
        //{
        //    int numrows = 2;
        //    int numcells = 3;
        //    for (int i = 0; i < numrows; i++)
        //    {
        //        TableRow r = new TableRow();
        //        for (int j = 0; j < numcells; j++)
        //        {
        //            TableCell c = new TableCell();
        //            if (i == 0 && j == 1)
        //                c.Text = "<center>" + DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa") + "</center>";
        //            if (i == 1 && j == 0)
        //                c.Text = "Procesado:<br>" + DateTime.Now.ToString();
        //            if (i == 1 && j == 1)
        //                c.Text = nombReport;
        //            if (i == 1 && j == 2)
        //            {
        //                //c.Text = "<div style='text-align:right'>Usuario : <br>" + HttpContext.Current.User.Identity.Name.ToUpper() + "</div>";
        //                c.Text = "<div style='text-align:right'>";
        //                if (filts.Rows.Count > 0)
        //                {
        //                    for (int k = 0; k < tbFilters.Rows.Count; k++)
        //                    {
        //                        if (tbFilters.Rows[k].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
        //                            c.Text += ((Label)tbFilters.Rows[k].Cells[0].Controls[0]).Text + " " + ((DropDownList)tbFilters.Rows[k].Cells[1].Controls[0]).SelectedItem.Text + "<br>";
        //                        else
        //                            if (tbFilters.Rows[k].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
        //                                c.Text += ((Label)tbFilters.Rows[k].Cells[0].Controls[0]).Text + " " + ((TextBox)tbFilters.Rows[k].Cells[1].Controls[0]).Text + "<br>";
        //                    }
        //                    c.Text = c.Text.Substring(0, c.Text.Length - 4);
        //                }
        //                c.Text += "</div>";
        //            }
        //            c.Width = new Unit("33%");
        //            r.Cells.Add(c);
        //        }
        //        tabHeader.Rows.Add(r);
        //    }
        //    tabHeader.BorderWidth = new Unit("1");
        //    tabHeader.Width = reportA.Width;
        //}

        protected void Header(string nombReport, DataTable filts)
        {
            int numrows = 2;
            int numcells = 3;
            for (int i = 0; i < numrows; i++)
            {
                TableRow r = new TableRow();
                for (int j = 0; j < numcells; j++)
                {
                    TableCell c = new TableCell();
                    if ((i == 0) && (j == 1))
                    {
                        c.Text = "<center>" + DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa") + "</center>";
                    }
                    if ((i == 1) && (j == 0))
                    {
                        c.Text = "Procesado:<br>" + DateTime.Now.ToString();
                    }
                    if ((i == 1) && (j == 1))
                    {
                        c.Text = nombReport;
                    }
                    if ((i == 1) && (j == 2))
                    {
                        c.Text = "<div style='text-align:right'>";
                        if (filts.Rows.Count > 0)
                        {
                            for (int k = 0; k < this.tbFilters.Rows.Count; k++)
                            {
                                string texto;
                                if (this.tbFilters.Rows[k].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                                {
                                    texto = c.Text;
                                    c.Text = texto + ((Label) this.tbFilters.Rows[k].Cells[0].Controls[0]).Text + " " + ((DropDownList) this.tbFilters.Rows[k].Cells[1].Controls[0]).SelectedItem.Text + "<br>";
                                }
                                else if (this.tbFilters.Rows[k].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                {
                                    texto = c.Text;
                                    c.Text = texto + ((Label) this.tbFilters.Rows[k].Cells[0].Controls[0]).Text + " " + ((TextBox) this.tbFilters.Rows[k].Cells[1].Controls[0]).Text + "<br>";
                                }
                            }
                            c.Text = c.Text.Substring(0, c.Text.Length - 4);
                        }
                        c.Text = c.Text + "</div>";
                    }
                    c.Width = new Unit("33%");
                    r.Cells.Add(c);
                }
                this.tabHeader.Rows.Add(r);
            }
            this.tabHeader.BorderWidth = new Unit("1");
            this.tabHeader.Width = this.report.Width;
        }

        protected string Sql(string sqlOrigen, int cantFilters)
        {
            string sql = sqlOrigen;
            int i;
            if (cantFilters > 0)
            {
                for (i = 0; i < tbFilters.Rows.Count; i++)
                {
                    string sql1 = sql.Substring(0, sql.IndexOf('@'));
                    string sql2 = sql.Substring(sql.IndexOf('@') + 1);
                    if (tbFilters.Rows[i].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.DropDownList")
                        sql = sql1 + ((DropDownList)tbFilters.Rows[i].Cells[1].Controls[0]).SelectedValue + sql2;
                    else if (tbFilters.Rows[i].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                        sql = sql1 + ((TextBox)tbFilters.Rows[i].Cells[1].Controls[0]).Text + sql2;
                }
            }
            return sql;
        }

        //protected void Body(string sql, DataTable filas, string msks, DataTable filtrs)
        //{
        //    int i;
        //    //Ahora vamos a cuadrar el sql con los valores de los filtros
        //    //Primero la cantidad de filtros que tenemos
        //    sql = this.Sql(sql, filtrs.Rows.Count);
        //    DataSet dsReport = new DataSet();
        //    DBFunctions.Request(dsReport, IncludeSchema.NO, sql);
        //    //Ahora creamos un DataTable clonado solo con la estructura
        //    DataTable dtReport = new DataTable();
        //    if (msks == "")
        //        dtReport = dsReport.Tables[0].Clone();
        //    else
        //        dtReport = this.Preparar_DataTable_Masks(dsReport.Tables[0], dtReport, msks);
        //    //Ahora traemos todas las filas que sean para arriba
        //    //sfil_id, sfil_posicion, sfil_orden, sfil_alineacion, sfil_valor, sfil_opcion
        //    //   0          1              2           3            4             5
        //    for (i = 0; i < filas.Rows.Count; i++)
        //    {
        //        if (filas.Rows[i][1].ToString() == "Ar")
        //            dtReport.Rows.Add(this.Config_Fila(filas.Rows[i][4].ToString(), dtReport, msks, filtrs.Rows.Count));
        //        else if (filas.Rows[i][1].ToString() == "Ab")
        //            dsReport.Tables[0].Rows.Add(this.Config_Fila(filas.Rows[i][4].ToString(), dsReport.Tables[0], msks, filtrs.Rows.Count));
        //    }
        //    for (i = 0; i < dsReport.Tables[0].Rows.Count; i++)
        //    {
        //        if (msks == "")
        //            dtReport.ImportRow(dsReport.Tables[0].Rows[i]);
        //        else
        //            dtReport.Rows.Add(this.ImportarFila(dtReport, dsReport.Tables[0].Rows[i], msks));
        //    }
        //    EstablecerTiposGrilla(dtReport);
           
        //    report.DataSource = dtReport;
        //    reportA.DataSource = dtReport;
        //    Session["RepData"] = dtReport;
        //    Session.Remove("RepGraph");
        //    report.DataBind();
        //    reportA.DataBind();
        //    DatasToControls.JustificacionGrilla(reportA, dtReport);
        //}

        protected void Body(string sql, DataTable filas, string msks, DataTable filtrs)
        {
            sql = this.Sql(sql, filtrs.Rows.Count);
            DataSet dsReport = new DataSet();
            DBFunctions.Request(dsReport, IncludeSchema.NO, sql);
            DataTable dtReport = new DataTable();
            if (msks == "")
            {
                dtReport = dsReport.Tables[0].Clone();
            }
            else
            {
                dtReport = this.Preparar_DataTable_Masks(dsReport.Tables[0], dtReport, msks);
            }
            int i = 0;
            while (i < filas.Rows.Count)
            {
                if (filas.Rows[i][1].ToString() == "Ar")
                {
                    dtReport.Rows.Add(this.Config_Fila(filas.Rows[i][4].ToString(), dtReport, msks, filtrs.Rows.Count));
                }
                else if (filas.Rows[i][1].ToString() == "Ab")
                {
                    dsReport.Tables[0].Rows.Add(this.Config_Fila(filas.Rows[i][4].ToString(), dsReport.Tables[0], msks, filtrs.Rows.Count));
                }
                i++;
            }
            for (i = 0; i < dsReport.Tables[0].Rows.Count; i++)
            {
                if (msks == "")
                {
                    dtReport.ImportRow(dsReport.Tables[0].Rows[i]);
                }
                else
                {
                    dtReport.Rows.Add(this.ImportarFila(dtReport, dsReport.Tables[0].Rows[i], msks));
                }
            }//aaaa
            for (int n = 0; n < dtReport.Columns.Count; n++)
            {
                BoundColumn dgrColumna = new BoundColumn();
                string formato = "";
                string nombreCol = dtReport.Columns[n].ColumnName;
                Type tipo = dtReport.Columns[n].DataType;
                dgrColumna.DataField = nombreCol;
                dgrColumna.HeaderText = nombreCol;
                dgrColumna.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                if (tipo == Type.GetType("System.DateTime"))
                {
                    formato = "{0:yyyy-MM-dd}";
                }
                else if (tipo == Type.GetType("System.Decimal"))
                {
                    formato = "{0:N}";
                    dgrColumna.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                }
                else if (tipo == Type.GetType("System.Double"))
                {
                    formato = "{0:N}";
                    dgrColumna.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                }
                else if (tipo == Type.GetType("System.String"))
                {
                    formato = "{0}";
                }
                if (formato.Length > 0)
                {
                    dgrColumna.DataFormatString = formato;
                }
                this.report.Columns.Add(dgrColumna);
            }

            EstablecerTiposGrilla(dtReport);

            Session["RepData"] = dtReport; //prueba
            Session.Remove("RepGraph");
            this.report.DataSource = dtReport;
            this.report.DataBind();
            reportA.DataSource = dtReport;
            reportA.DataBind();
            this.reportA.Visible = false;
            DatasToControls.JustificacionGrilla(this.report, dtReport);


            //    report.DataSource = dtReport;
            //    reportA.DataSource = dtReport;
            //    Session["RepData"] = dtReport;
            //    Session.Remove("RepGraph");
            //    report.DataBind();
            //    reportA.DataBind();
            //    DatasToControls.JustificacionGrilla(reportA, dtReport);

        }

        private void EstablecerTiposGrilla(DataTable dtReport)
        {
            //Establecer tipos
            report.Columns.Clear();
            reportA.Columns.Clear();

            for (int n = 0; n < dtReport.Columns.Count; n++)
            {
                BoundColumn dgrColumna = new BoundColumn();
                string formato = "";
                string nombreCol = dtReport.Columns[n].ColumnName;
                System.Type tipo = dtReport.Columns[n].DataType;
                dgrColumna.DataField = nombreCol;
                dgrColumna.HeaderText = nombreCol;
                //Revisar tipo
                dgrColumna.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                if (tipo == System.Type.GetType("System.DateTime")) formato = "{0:yyyy-MM-dd}";
                else if (tipo == System.Type.GetType("System.Decimal")) { formato = "{0:N}"; dgrColumna.ItemStyle.HorizontalAlign = HorizontalAlign.Right; }
                else if (tipo == System.Type.GetType("System.Double")) { formato = "{0:N}"; dgrColumna.ItemStyle.HorizontalAlign = HorizontalAlign.Right; }
                else if (tipo == System.Type.GetType("System.String")) { formato = "{0}"; }
                if (formato.Length > 0)
                    dgrColumna.DataFormatString = formato;
                report.Columns.Add(dgrColumna);
                reportA.Columns.Add(dgrColumna);
            }
            TemplateColumn dgrColumnaG = new TemplateColumn();
            dgrColumnaG.ItemTemplate = new DataGridTemplate(ListItemType.Item, "Graficar");
            report.Columns.Add(dgrColumnaG);
        }

        protected DataRow ImportarFila(DataTable template, DataRow source, string msks)
        {
            DataRow fila = template.NewRow();
            for (int i = 0; i < template.Columns.Count; i++)
            {
                string mask = this.Find_Mask(i + 1, msks.Split(','));
                if (mask == "")
                    fila[i] = source[i];
                else if (mask == "FM")
                {
                    if (source[i].ToString() != "")
                    {
                        try { fila[i] = System.Convert.ToDouble(source[i]).ToString("C"); }
                        catch { fila[i] = source[i]; }
                    }
                    else
                        fila[i] = "";
                }
                else if (mask == "FN")
                {
                    if (source[i].ToString() != "")
                    {
                        try { fila[i] = System.Convert.ToDouble(source[i]).ToString("N00"); }
                        catch { fila[i] = source[i]; }
                    }
                    else
                        fila[i] = "";
                }

            }
            return fila;
        }

        protected DataTable Preparar_DataTable_Masks(DataTable template, DataTable obje, string msks)
        {
            for (int i = 0; i < template.Columns.Count; i++) 
            {
                string resulMask = this.Find_Mask(i + 1, msks.Split(','));
                if (resulMask == "")
                    obje.Columns.Add(new DataColumn(template.Columns[i].ColumnName, template.Columns[i].DataType));
                else if (resulMask == "FM" || resulMask == "FN")
                    obje.Columns.Add(new DataColumn(template.Columns[i].ColumnName, System.Type.GetType("System.String")));

            }
            return obje;
        }

        protected string Find_Mask(int position, string[] masks)
        {
            string result = "";
            for (int i = 0; i < masks.Length; i++)
            {
                string[] sep = masks[i].Split('-');
                if (System.Convert.ToInt32(sep[0].Trim()) == position)
                    result = sep[1].Trim();
            }
            return result;
        }

        protected DataRow Config_Fila(string valor, DataTable template, string msks, int filts)
        {
            DataRow fila = template.NewRow();
            string mask = "";
            if (msks != "")
                msks = this.Find_Mask(template.Columns.Count, msks.Split(','));
            if (valor.IndexOf("&") != -1)
            {
                string[] div = valor.Split('&');
                fila[0] = div[0];
                if (div[1].IndexOf("FROM") == -1)
                    fila[template.Columns.Count - 1] = div[1];
                else
                {
                    if (mask == "")
                    {
                        if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.Decimal")
                            fila[template.Columns.Count - 1] = System.Convert.ToDecimal(DBFunctions.SingleData(this.Sql(div[1], filts)));
                        else if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.Double")
                            fila[template.Columns.Count - 1] = System.Convert.ToDouble(DBFunctions.SingleData(this.Sql(div[1], filts)));
                        else if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.Int32")
                            fila[template.Columns.Count - 1] = System.Convert.ToInt32(DBFunctions.SingleData(this.Sql(div[1], filts)));
                        else if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.String")
                            fila[template.Columns.Count - 1] = DBFunctions.SingleData(this.Sql(div[1], filts));
                    }
                    else if (mask == "FM")
                        fila[template.Columns.Count - 1] = System.Convert.ToDouble(DBFunctions.SingleData(this.Sql(div[1], filts))).ToString("C");
                    else if (mask == "FN")
                        fila[template.Columns.Count - 1] = System.Convert.ToDouble(DBFunctions.SingleData(this.Sql(div[1], filts))).ToString("N00");
                }
            }
            else
            {
                if (valor.IndexOf("FROM") == -1)
                    fila[0] = valor;
                else
                {
                    if (mask == "")
                    {
                        if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.Decimal")
                            fila[template.Columns.Count - 1] = System.Convert.ToDecimal(DBFunctions.SingleData(this.Sql(valor, filts)));
                        else if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.Double")
                            fila[template.Columns.Count - 1] = System.Convert.ToDouble(DBFunctions.SingleData(this.Sql(valor, filts)));
                        else if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.Int32")
                            fila[template.Columns.Count - 1] = System.Convert.ToInt32(DBFunctions.SingleData(this.Sql(valor, filts)));
                        else if (template.Columns[template.Columns.Count - 1].DataType.ToString() == "System.String")
                            fila[template.Columns.Count - 1] = DBFunctions.SingleData(this.Sql(valor, filts));
                    }
                    else if (mask == "FM")
                        fila[template.Columns.Count - 1] = System.Convert.ToDouble(DBFunctions.SingleData(this.Sql(valor, filts))).ToString("C");
                    else if (mask == "FN")
                        fila[template.Columns.Count - 1] = System.Convert.ToDouble(DBFunctions.SingleData(this.Sql(valor, filts))).ToString("N00");
                }
            }
            return fila;
        }

        //protected void Footer(DataTable footerReport)
        //{
        //    for (int i = 0; i < footerReport.Rows.Count; i++)
        //    {
        //        TableRow r = new TableRow();
        //        TableCell c = new TableCell();
        //        if (footerReport.Rows[i][1].ToString().IndexOf("FROM") == -1)
        //            c.Text = footerReport.Rows[i][1].ToString();
        //        else
        //            c.Text = DBFunctions.SingleData(footerReport.Rows[i][1].ToString());
        //        r.Cells.Add(c);
        //        tabFooter.Rows.Add(r);
        //    }
        //}

        protected void Footer(DataTable footerReport)
        {
            for (int i = 0; i < footerReport.Rows.Count; i++)
            {
                TableRow r = new TableRow();
                TableCell c = new TableCell();
                if (footerReport.Rows[i][1].ToString().IndexOf("FROM") == -1)
                {
                    c.Text = footerReport.Rows[i][1].ToString();
                }
                else
                {
                    c.Text = DBFunctions.SingleData(footerReport.Rows[i][1].ToString());
                }
                r.Cells.Add(c);
                this.tabFooter.Rows.Add(r);
            }
        }

        protected string Report_Text_Converter()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            tabHeader.RenderControl(htmlTW);
            reportA.RenderControl(htmlTW);
            tabFooter.RenderControl(htmlTW);
            return SB.ToString();
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pErr"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ingresó un valor no registrado!");
                }
                Session.Remove("Rep");
                Session.Remove("RepData");
                plcOpciones.Visible = false;
                plcReport.Visible = false;
            }
            tbFilters.BackColor = Color.Transparent;
            tbFilters.BorderWidth = new Unit(0);
            this.GetSchemaFilters(Request.QueryString["idReporte"]);
            this.SetFormFilters();
            if (Session["RepData"] != null)
            {
                DataTable dtDatos = (DataTable)Session["RepData"];
                EstablecerTiposGrilla(dtDatos);
                report.DataSource = dtDatos;
                report.DataBind();
            }
            //Implementacion de clase Timmer en prueba...
            if (Request.QueryString["tim"] == "1")
            {
                this.MakeReport(sender,e);
                filterHolders.Visible = false;
                toolsHolder.Visible = false;
                plcOpciones.Visible = false;
                Response.AddHeader("REFRESH", "14;URL=" + indexPage + "?process=Reportes.FormatoReporte&idReporte=1013&tim=2&tall=" + Request.QueryString["tall"]);
                //Response.AddHeader("REFRESH", "14;URL=" + "AMS.Automotriz.PlanificacionTallerVeh.aspx?tal=" + Request.QueryString["tall"] + "&pag=0&rdrt=1");
            }
            else if (Request.QueryString["tim"] == "2")
            {
                this.MakeReport(sender, e);
                filterHolders.Visible = false;
                toolsHolder.Visible = false;
                plcOpciones.Visible = false;
                Response.AddHeader("REFRESH", "14;URL=" + "AMS.Automotriz.PlanificacionTallerVeh.aspx?tal=" + Request.QueryString["tall"] + "&pag=1&rdrt=1");
                //Response.AddHeader("REFRESH", "14;URL=AMS.Automotriz.CitasTaller.aspx?rdr=1&serv=1");
            }
            
        }

        #region botones
        //public void MakeReport(Object Sender, EventArgs E)
        //{
        //    //validacion de campos tipo texto
        //    validarTextBox();
        //    //Lo que primero hacemos es cargar el reporte mediante un objeto
        //    Reporte miReporte = new Reporte(Request.QueryString["idReporte"]);
        //    //Ahora cargamos el header del reporte
        //    reportA.Visible = true;
        //    Header(miReporte.NombreReporte, miReporte.Filtros);
        //    Body(miReporte.SqlReporte, miReporte.Filas, miReporte.Masks, miReporte.Filtros);
        //    Footer(miReporte.Footer);
        //    toolsHolder.Visible = true;
        //    Session["Rep"] = this.Report_Text_Converter();
        //    reportA.Visible = false;
        //    plcReport.Visible = true;
        //    plcOpciones.Visible = false;
        //}

        public void MakeReport(object Sender, EventArgs E)
        {
            Reporte miReporte = new Reporte(base.Request.QueryString["idReporte"]);
            this.Header(miReporte.NombreReporte, miReporte.Filtros);
            this.Body(miReporte.SqlReporte, miReporte.Filas, miReporte.Masks, miReporte.Filtros);
            this.Footer(miReporte.Footer);
            this.toolsHolder.Visible = true;
            base.Session["Rep"] = this.Report_Text_Converter();
            plcReport.Visible = true;
            plcOpciones.Visible = false;
        }

        //validar textBox
        public void validarTextBox()
        {
            DataSet dsTextBox = new DataSet();
            string dataType;
            for (int k = 0; k < tbFilters.Rows.Count && dsFilters.Tables[0].Rows.Count != 0; k++)
            {
                if (tbFilters.Rows[k].Cells[1].Controls[0].GetType().ToString() == "System.Web.UI.WebControls.TextBox" && !dsFilters.Tables[0].Rows[k][5].ToString().Equals(""))
                {
                    dataType = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE tbname='" + dsFilters.Tables[0].Rows[k][2].ToString().ToUpper() + "' AND name='" + dsFilters.Tables[0].Rows[k][3].ToString().ToUpper() + "'");

                    if (dataType == "INTEGER " || dataType.Equals("SMALLINT ") || dataType.Equals("BIGINT ") || dataType.Equals("DECIMAL ") || dataType.Equals("DOUBLE ") || dataType.Equals("REAL "))
                        DBFunctions.Request(dsTextBox, IncludeSchema.NO, "select * from " + dsFilters.Tables[0].Rows[k][2].ToString().ToUpper() + " where " + dsFilters.Tables[0].Rows[k][3].ToString().ToUpper() + " " + dsFilters.Tables[0].Rows[k][5].ToString() + " " + ((TextBox)tbFilters.Rows[k].Cells[1].Controls[0]).Text + "");
                    else
                        DBFunctions.Request(dsTextBox, IncludeSchema.NO, "select * from " + dsFilters.Tables[0].Rows[k][2].ToString().ToUpper() + " where " + dsFilters.Tables[0].Rows[k][3].ToString().ToUpper() + " " + dsFilters.Tables[0].Rows[k][5].ToString() + " '" + ((TextBox)tbFilters.Rows[k].Cells[1].Controls[0]).Text + "'");

                    if (dsTextBox.Tables[0].Rows.Count == 0)
                    {
                        Response.Redirect(indexPage + "?process=Reportes.FormatoReporte&idReporte=1234&pErr=1");
                    }
                }
            }
        }

        public void SendMail(Object Sender, ImageClickEventArgs E)
        {
            Reporte miReporte = new Reporte(Request.QueryString["idReporte"]);

            try
            {
                Utils.EnviarMail(tbEmail.Text, miReporte.NombreReporte, Session["Rep"].ToString(), TipoCorreo.HTML, "");
            }
            catch (Exception e)
            {
                lbInfo.Text = e.ToString();
            }
            MakeReport(Sender, E);
        }
        //public void Excel(Object Sender, ImageClickEventArgs E)
        //{
        //    MakeReport(Sender, null);

        //    Reporte miReporte = new Reporte(Request.QueryString["idReporte"]);

        //    DateTime fecha = DateTime.Now;
        //    string nombreArchivo = miReporte.NombreReporte + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
            
        //    Response.Clear();
        //    Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
        //    Response.Charset = "Unicode";
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.xls"; 
        //    System.IO.StringWriter stringWrite = new System.IO.StringWriter(); 
        //    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

        //    reportA.RenderControl(htmlWrite);
        //    Response.Write(stringWrite.ToString()); 
        //    Response.End();

        //}

        protected void Excel(object sender, EventArgs e)
        {
            this.MakeReport(this, null);
            this.reportA.Visible = true;
            Reporte miReporte = new Reporte(base.Request.QueryString["idReporte"]);
            DateTime fecha = DateTime.Now;
            string nombreArchivo = miReporte.NombreReporte + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
            base.Response.Clear();
            base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
            base.Response.Charset = "Unicode";
            base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.Response.ContentType = "application/vnd.xls";
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            this.reportA.RenderControl(htmlWrite);
            base.Response.Write(stringWrite.ToString());
            base.Response.End();
            this.reportA.Visible = false;
        }

        public void Volver(Object Sender, EventArgs E)
        {
            MakeReport(Sender, null);
        }
        #endregion eventos
        #endregion

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

        #region Grafica
        public void Graph(Object Sender, EventArgs E)
        {
            Graph(Sender, (ImageClickEventArgs)null);
        }
        public void Graph(Object Sender, ImageClickEventArgs E)
        {
            int i = 0;
            DataTable dtGrafica;
            if (Session["RepGraph"] == null)
            {
                DataTable dtDatos = (DataTable)Session["RepData"];
                dtGrafica = dtDatos.Clone();

                foreach (DataGridItem dgrI in report.Items)
                {
                    if (((CheckBox)dgrI.Cells[dgrI.Cells.Count - 1].Controls[0]).Checked)
                        dtGrafica.ImportRow(dtDatos.Rows[i]);
                    i++;
                }
                if (dtGrafica.Rows.Count == 0)
                {
                    Utils.MostrarAlerta(Response, "Seleccione las filas que desea graficar y el tipo de gráfica.");
                    plcReport.Visible = true;
                    plcOpciones.Visible = false;
                    MakeReport(Sender, (EventArgs)null);
                    return;
                }
                Session["RepGraph"] = dtGrafica;
            }
            else
                dtGrafica = (DataTable)Session["RepGraph"];

            if (dtGrafica.Rows.Count > 1 && ddlTipoGrafica.SelectedValue == "P")
            {
                Utils.MostrarAlerta(Response, "Seleccione sólo una fila para las gráficas de pastel.");
                plcReport.Visible = true;
                plcOpciones.Visible = false;
                MakeReport(Sender, (EventArgs)null);
                return;
            }

            string titulo;
            Random rndCol = new Random();
            i = 0;

            foreach (DataRow drI in dtGrafica.Rows)
            {
                titulo = drI[0].ToString();
                switch (ddlTipoGrafica.SelectedValue)
                {
                    case "L":
                        chtGrafica.Charts.Add(CreateLineChart(GenerarPuntos(dtGrafica, i), Color.FromArgb(rndCol.Next(250), rndCol.Next(250), rndCol.Next(250)), titulo));
                        break;
                    case "P":
                        chtGrafica.Charts.Add(CreatePieChart(GenerarPuntos(dtGrafica, i), Color.FromArgb(rndCol.Next(250), rndCol.Next(250), rndCol.Next(250)), titulo));
                        break;
                    case "B":
                        chtGrafica.Charts.Add(CreateColumnChart(GenerarPuntos(dtGrafica, i), Color.FromArgb(rndCol.Next(250), rndCol.Next(250), rndCol.Next(250)), titulo));
                        break;
                }
                i++;
            }
            Escalar();
            chtGrafica.Legend.Position = LegendPosition.Bottom;
            chtGrafica.RedrawChart();
            chtGrafica.Visible = true;
            plcOpciones.Visible = true;
            plcReport.Visible = false;
        }
        private void Escalar()
        {
            int ancho, alto;
            try
            {
                ancho = Convert.ToInt16(txtAncho.Text);
                if (ancho < 1 || ancho >= 10000)
                    throw (new Exception());
            }
            catch
            {
                ancho = 800;
                txtAncho.Text = "800";
            }
            try
            {
                alto = Convert.ToInt16(txtAlto.Text);
                if (alto < 1 || alto >= 10000)
                    throw (new Exception());
            }
            catch
            {
                alto = 600;
                txtAlto.Text = "600";
            }
            chtGrafica.Width = ancho;
            chtGrafica.Height = alto;
        }
        private ChartPointCollection GenerarPuntos(DataTable dtDatos, int fila)
        {
            ChartPointCollection data = new ChartPointCollection();
            float valor = 0;

            for (int c = 1; c < dtDatos.Columns.Count; c++)
            {
                try { valor = (float)Convert.ToDecimal(dtDatos.Rows[fila][c]); }
                catch { valor = 0; }
                data.Add(new ChartPoint(dtDatos.Columns[c].ColumnName, valor));
            }

            return data;
        }
        private ColumnChart CreateColumnChart(ChartPointCollection points, Color clrFill, string legend)
        {
            ColumnChart ch = new ColumnChart(points, clrFill);
            ch.Legend = legend;
            ch.Fill.Color = clrFill;
            ch.LineMarker = new CircleLineMarker(1, clrFill, clrFill);
            return ch;
        }
        private PieChart CreatePieChart(ChartPointCollection points, Color clrFill, string legend)
        {
            PieChart ch = new PieChart(points, clrFill);
            ch.Legend = legend;
            ch.Fill.Color = clrFill;
            ch.LineMarker = new CircleLineMarker(1, clrFill, clrFill);
            return ch;
        }
        private LineChart CreateLineChart(ChartPointCollection points, Color clrFill, string legend)
        {
            LineChart ch = new LineChart(points, clrFill);
            ch.Legend = legend;
            ch.Fill.Color = clrFill;
            ch.Line.Width = 5;
            ch.LineMarker = new CircleLineMarker(10, clrFill, clrFill);
            return ch;
        }
        #endregion Grafica
    }

    public class DataGridTemplate : ITemplate
    {
        ListItemType templateType;
        string columnName;

        public DataGridTemplate(ListItemType type, string colname)
        {
            templateType = type;
            columnName = colname;
        }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            Literal lc = new Literal();
            switch (templateType)
            {
                case ListItemType.Item:
                    CheckBox chkGraf = new CheckBox();
                    chkGraf.CausesValidation = false;
                    container.Controls.Add(chkGraf);
                    break;
            }
        }
    }
}
