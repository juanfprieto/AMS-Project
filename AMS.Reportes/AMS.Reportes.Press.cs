
using AMS.DB;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AMS.Reportes
{
    public class Press
    {
        protected DataSet dsReport;
        protected DataTable tbReport;
        protected DataColumn dc;
        protected string infoProcess;
        protected string reportTitle;
        protected string[] sourceFields;
        protected string[] titleFields;
        protected int i;
        protected int j;

        public string InfoProcess
        {
            get
            {
                return this.infoProcess;
            }
        }

        public DataTable TbReport
        {
            get
            {
                return this.tbReport;
            }
        }

        public Press(DataSet placeDs, string name)
        {
            this.reportTitle = name;
            this.dsReport = placeDs;
            this.tbReport = this.dsReport.Tables.Add(name);
        }

        public void Firmas(Table tab, Unit wd)
        {
            TableRow row1 = new TableRow();
            TableCell cell1 = new TableCell();
            cell1.Text = DBFunctions.SingleData("SELECT cemp_nombcarggene FROM cempresa") + ":";
            cell1.Width = new Unit("14%");
            row1.Cells.Add(cell1);
            TableCell cell2 = new TableCell();
            cell2.Text = DBFunctions.SingleData("SELECT cemp_geregene FROM cempresa");
            cell2.Width = new Unit("40%");
            row1.Cells.Add(cell2);
            TableCell cell3 = new TableCell();
            cell3.Text = "cc:";
            cell3.Width = new Unit("4%");
            row1.Cells.Add(cell3);
            TableCell cell4 = new TableCell();
            cell4.Text = DBFunctions.SingleData("SELECT cemp_nitgeregene FROM cempresa");
            cell4.Width = new Unit("14%");
            row1.Cells.Add(cell4);
            TableCell cell5 = new TableCell();
            cell5.Text = "<br>";
            cell5.Width = new Unit("14%");
            row1.Cells.Add(cell5);
            TableCell cell6 = new TableCell();
            cell6.Text = "<br>";
            cell6.Width = new Unit("14%");
            row1.Cells.Add(cell6);
            tab.Rows.Add(row1);
            TableRow row2 = new TableRow();
            TableCell cell7 = new TableCell();
            cell7.Text = "Firma:";
            cell7.Width = new Unit("14%");
            row2.Cells.Add(cell7);
            TableCell cell8 = new TableCell();
            cell8.Text = "<br>";
            cell8.Width = new Unit("40%");
            row2.Cells.Add(cell8);
            TableCell cell9 = new TableCell();
            cell9.Text = "<br>";
            cell9.Width = new Unit("4%");
            row2.Cells.Add(cell9);
            TableCell cell10 = new TableCell();
            cell10.Text = "<br>";
            cell10.Width = new Unit("14%");
            row2.Cells.Add(cell10);
            TableCell cell11 = new TableCell();
            cell11.Text = "<br>";
            cell11.Width = new Unit("14%");
            row2.Cells.Add(cell11);
            TableCell cell12 = new TableCell();
            cell12.Text = "<br>";
            cell12.Width = new Unit("14%");
            row2.Cells.Add(cell12);
            tab.Rows.Add(row2);
            TableRow row3 = new TableRow();
            cell12.Text = DBFunctions.SingleData("SELECT ccon_cargcont FROM ccontabilidad") + ":";
            cell12.Width = new Unit("14%");
            row3.Cells.Add(cell12);
            TableCell cell13 = new TableCell();
            cell13.Text = DBFunctions.SingleData("SELECT ccon_nombcont FROM ccontabilidad");
            cell13.Width = new Unit("40%");
            row3.Cells.Add(cell13);
            TableCell cell14 = new TableCell();
            cell14.Text = "cc:";
            cell14.Width = new Unit("4%");
            row3.Cells.Add(cell14);
            TableCell cell15 = new TableCell();
            cell15.Text = DBFunctions.SingleData("SELECT ccon_idencont FROM ccontabilidad");
            cell15.Width = new Unit("14%");
            row3.Cells.Add(cell15);
            TableCell cell16 = new TableCell();
            cell16.Text = "Matricula:";
            cell16.Width = new Unit("14%");
            row3.Cells.Add(cell16);
            TableCell cell17 = new TableCell();
            cell17.Text = DBFunctions.SingleData("SELECT ccon_matrcont FROM ccontabilidad");
            cell17.Width = new Unit("14%");
            row3.Cells.Add(cell17);
            tab.Rows.Add(row3);
            TableRow row4 = new TableRow();
            TableCell cell18 = new TableCell();
            cell18.Text = "Firma:";
            cell18.Width = new Unit("14%");
            row4.Cells.Add(cell18);
            TableCell cell19 = new TableCell();
            cell19.Text = "<br>";
            cell19.Width = new Unit("40%");
            row4.Cells.Add(cell19);
            TableCell cell20 = new TableCell();
            cell20.Text = "<br>";
            cell20.Width = new Unit("4%");
            row4.Cells.Add(cell20);
            TableCell cell21 = new TableCell();
            cell21.Text = "<br>";
            cell21.Width = new Unit("14%");
            row4.Cells.Add(cell21);
            TableCell cell22 = new TableCell();
            cell22.Text = "<br>";
            cell22.Width = new Unit("14%");
            row4.Cells.Add(cell22);
            TableCell cell23 = new TableCell();
            cell23.Text = "<br>";
            cell23.Width = new Unit("14%");
            row4.Cells.Add(cell23);
            tab.Rows.Add(row4);
            TableRow row5 = new TableRow();
            TableCell cell24 = new TableCell();
            cell24.Text = DBFunctions.SingleData("SELECT ccon_cargrevf FROM ccontabilidad") + ":";
            cell24.Width = new Unit("14%");
            row5.Cells.Add(cell24);
            TableCell cell25 = new TableCell();
            cell25.Text = DBFunctions.SingleData("SELECT ccon_nombrevf FROM ccontabilidad");
            cell25.Width = new Unit("40%");
            row5.Cells.Add(cell25);
            TableCell cell26 = new TableCell();
            cell26.Text = "cc:";
            cell26.Width = new Unit("4%");
            row5.Cells.Add(cell26);
            TableCell cell27 = new TableCell();
            cell27.Text = DBFunctions.SingleData("SELECT ccon_idenrevf FROM ccontabilidad");
            cell27.Width = new Unit("14%");
            row5.Cells.Add(cell27);
            TableCell cell28 = new TableCell();
            cell28.Text = "Matricula:";
            cell28.Width = new Unit("14%");
            row5.Cells.Add(cell28);
            TableCell cell29 = new TableCell();
            cell29.Text = DBFunctions.SingleData("SELECT ccon_matrrevf FROM ccontabilidad");
            cell29.Width = new Unit("14%");
            row5.Cells.Add(cell29);
            tab.Rows.Add(row5);
            tab.Rows.Add(row5);
            TableRow row6 = new TableRow();
            TableCell cell30 = new TableCell();
            cell30.Text = "Firma:";
            cell30.Width = new Unit("14%");
            row6.Cells.Add(cell30);
            TableCell cell31 = new TableCell();
            cell31.Text = " ";
            cell31.Width = new Unit("40%");
            row6.Cells.Add(cell31);
            TableCell cell32 = new TableCell();
            cell32.Text = " ";
            cell32.Width = new Unit("4%");
            row6.Cells.Add(cell32);
            TableCell cell33 = new TableCell();
            cell33.Text = " ";
            cell33.Width = new Unit("14%");
            row6.Cells.Add(cell33);
            TableCell cell34 = new TableCell();
            cell34.Text = " ";
            cell34.Width = new Unit("14%");
            row6.Cells.Add(cell34);
            TableCell cell35 = new TableCell();
            cell35.Text = " ";
            cell35.Width = new Unit("14%");
            row6.Cells.Add(cell35);
            tab.Rows.Add(row6);
            tab.BorderWidth = new Unit("1");
            tab.Width = wd;
        }

        public void PreHeader(Table tab, Unit wd, params string[] pr)
        {
            int num1 = 2;
            int num2 = 3;
            for (this.i = 0; this.i < num1; ++this.i)
            {
                TableRow row = new TableRow();
                for (this.j = 0; this.j < num2; ++this.j)
                {
                    TableCell cell = new TableCell();
                    if (this.i == 0 && this.j == 1)
                        cell.Text = "<center>" + DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa") + "</center>";
                    if (this.i == 1 && this.j == 0)
                        cell.Text = "Procesado:<br>" + DateTime.Now.ToString();
                    if (this.i == 1 && this.j == 1)
                        cell.Text = "<center>" + this.reportTitle + "</center>";
                    if (this.i == 1 && this.j == 2)
                        cell.Text = "<div style='text-align:right'>" + pr[1] + "<br>" + pr[0] + "</div>";
                    cell.Width = new Unit("33%");
                    row.Cells.Add(cell);
                }
                tab.Rows.Add(row);
            }
            tab.BorderWidth = new Unit("1");
            tab.Width = wd;
        }

        public void SourceFieldTitles(string[] fields)
        {
            this.sourceFields = fields;
            if (fields.Length != 1)
                return;
            this.titleFields = fields[0].Split(",".ToCharArray());
            this.infoProcess = "parmetros de campos de titulo: " + this.titleFields.Length.ToString();
            for (this.i = 0; this.i < this.titleFields.Length / 2; ++this.i)
            {
                if (this.titleFields[2 * this.i] == "String")
                    this.dc = this.tbReport.Columns.Add(this.titleFields[2 * this.i + 1], typeof(string));
                if (this.titleFields[2 * this.i] == "Double")
                    this.dc = this.tbReport.Columns.Add(this.titleFields[2 * this.i + 1], typeof(double));
            }
        }

        public void PressFieldTitles(DataGrid dgReport)
        {
            for (this.i = 0; this.i < this.titleFields.Length / 2; ++this.i)
            {
                BoundColumn boundColumn = new BoundColumn();
                boundColumn.HeaderText = this.titleFields[2 * this.i + 1];
                boundColumn.DataField = this.dsReport.Tables[this.dsReport.Tables.Count - 1].Columns[this.i].ToString();
                boundColumn.DataFormatString = "{0:N}";
                dgReport.Columns.Add((DataGridColumn)boundColumn);
            }
        }

        public void PressFieldTitlesView(DataGrid dgReport, int num)
        {
            for (this.i = 0; this.i < this.titleFields.Length / 2 - (this.titleFields.Length / 2 - num); ++this.i)
            {
                BoundColumn boundColumn = new BoundColumn();
                boundColumn.HeaderText = this.titleFields[2 * this.i + 1];
                boundColumn.DataField = this.dsReport.Tables[this.dsReport.Tables.Count - 1].Columns[this.i].ToString();
                boundColumn.DataFormatString = "{0:N}";
                dgReport.Columns.Add((DataGridColumn)boundColumn);
            }
            BoundColumn boundColumn1 = new BoundColumn();
            boundColumn1.HeaderText = this.titleFields[this.titleFields.Length - 1];
            boundColumn1.DataField = this.dsReport.Tables[this.dsReport.Tables.Count - 1].Columns[this.titleFields.Length / 2 - 1].ToString();
            boundColumn1.DataFormatString = "{0:N}";
            dgReport.Columns.Add((DataGridColumn)boundColumn1);
        }

        public DataView SortWithNextField(int indTb, int indCol)
        {
            DataView defaultView = this.dsReport.Tables[indTb].DefaultView;
            defaultView.Sort = this.dsReport.Tables[indTb].Columns[indCol].ToString();
            return defaultView;
        }

        public static string PressOnEmail(string title, string ToAddr, Table tph, DataGrid rp)
        {
            Table table = new Table();
            TableRow row1 = new TableRow();
            TableCell cell1 = new TableCell();
            TableRow row2 = new TableRow();
            TableCell cell2 = new TableCell();
            cell1.Controls.Add((Control)tph);
            cell2.Controls.Add((Control)rp);
            row1.Cells.Add(cell1);
            row2.Cells.Add(cell2);
            table.Rows.Add(row1);
            table.Rows.Add(row2);
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter writer = new HtmlTextWriter((TextWriter)new StringWriter(sb));
            tph.RenderControl(writer);
            rp.RenderControl(writer);
            string str = sb.ToString();
            MailMessage message = new MailMessage();
            message.From = ConfigurationSettings.AppSettings["EmailFrom"];
            message.To = ToAddr;
            message.Subject = title;
            message.Body = str;
            message.BodyFormat = MailFormat.Html;
            try
            {
                SmtpMail.Send(message);
            }
            catch (Exception ex)
            {
                return "No se pudo enviar el reporte a " + message.To + " porque " + ex.Message;
            }
            return "Reporte enviado a " + message.To;
        }
    }
}
