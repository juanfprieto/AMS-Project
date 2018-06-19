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


namespace AMS.Contabilidad
{
    public partial class MovCom : System.Web.UI.UserControl
    {
        DataSet ds = new DataSet();
        protected Label reportInfo;
        protected OdbcDataReader dr;
        protected string reportTitle = "Movimiento de Comprobantes";
        protected void Page_Load(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            this.ClearChildViewState();
            if (!IsPostBack)
            {
                bind.PutDatasIntoDropDownList(year, "SELECT pano_ano FROM pano where PANO_ANO IN (SELECT DISTINCT PANO_ANO FROM MsaldoCuenta) ORDER BY 1 DESC");
                string yr = DBFunctions.SingleData("SELECT pano_ano FROM ccontabilidad");
                year.SelectedIndex = year.Items.IndexOf(new ListItem(yr, yr));
                bind.PutDatasIntoDropDownList(month, "SELECT pmes_mes, pmes_nombre FROM pmes WHERE pmes_mes != 0 AND pmes_mes != 13 ORDER BY 1");
                bind.PutDatasIntoDropDownList(typeDoc, "SELECT pdoc_codigo, pdoc_codigo  CONCAT ' - ' CONCAT pdoc_nombre FROM pdocumento order by pdoc_nombre;");
                if (typeDoc.Items.Count > 1)
                    typeDoc.Items.Insert(0, "Seleccione:..");
                string mt = DBFunctions.SingleData("SELECT pmes_mes FROM ccontabilidad");
                month.SelectedIndex = month.Items.IndexOf(new ListItem(mt, mt));
            }
            dg.EnableViewState = true;
        }

        protected void Report_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header)
            {
                e.Item.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Item.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Item.Cells[6].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        protected void Consulta_Click(object Sender, EventArgs e)
        {
            
            string[] pr = new string[2];
            pr[0] = pr[1] = "";
            Press frontEnd = new Press(new DataSet(), reportTitle);
            frontEnd.PreHeader(tabPreHeader, dg.Width, pr);
            frontEnd.Firmas(tabFirmas, dg.Width);
            string sql = " select PDOC_CODIGO, MCOM_NUMEDOCU, MCOM_FECHA, MCOM_RAZON, MCOM_FECHAPROC, MCOM_USUARIO, MCOM_VALOR from MCOMPROBANTE  where PMES_MES = " + month.SelectedValue + " AND PANO_ANO =" + year.SelectedValue;
            //0             1            2             3            4                5         6
            //string sql1 = "select MC.MCUE_NOMBRE CONCAT ' - ' CONCAT DC.MCUE_CODIPUC ,DCUE_CODIREFE,DCUE_NUMEREFE,DCUE_DETALLE,PALM_ALMACEN,PCEN_CODIGO || ' - ' || Nit || ' - ' || NOMBRE,DCUE_VALODEBI,DCUE_VALOCRED,DCUE_VALOBASE,  PDOC_CODIGO, MCOM_NUMEDOCU, dcue_niifdebi, dcue_niifcred from dcuenta DC, MCUENTA MC, VMNIT where vmnit.mnit_nit=DC.mnit_nit AND DC.MCUE_CODIPUC = MC.MCUE_CODIPUC";
            string sql1 = "select MCUE_CODIPUC ,DCUE_CODIREFE,DCUE_NUMEREFE,DCUE_DETALLE,PALM_ALMACEN,PCEN_CODIGO || ' - ' || Nit || ' - ' || NOMBRE,DCUE_VALODEBI,DCUE_VALOCRED,DCUE_VALOBASE,  PDOC_CODIGO, MCOM_NUMEDOCU, dcue_niifdebi, dcue_niifcred from dcuenta, VMNIT where vmnit.mnit_nit=DCUENTA.mnit_nit ";
            //0             1            2             3            4                5         6          7             8                 9           10
            CultureInfo bz = new CultureInfo("en-us");
            //Uno
            if (RadioOpcion.SelectedValue == "UCE")
            {
                if (ddlNumComp.Items.Count == 0) return;
                sql += " AND  MCOM_NUMEDOCU = " + ddlNumComp.SelectedValue + " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "'";
                sql1 += " AND MCOM_NUMEDOCU = " + ddlNumComp.SelectedValue + " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "'";
                ViewState["CONSULTAExcel"] = sql1;
            }
            //Un tipo
            if (RadioOpcion.SelectedValue == "UTC")
            {
                if (ddlNumComp.Items.Count == 0) return;
                sql += " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "'";
                sql1 += " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "'";
                ViewState["CONSULTAExcel"] = sql1;
            }
            sql += " ORDER BY PDOC_CODIGO, MCOM_NUMEDOCU";            
            sql1 += " ORDER BY PDOC_CODIGO, MCOM_NUMEDOCU, MCUE_CODIPUC";
            ViewState["CONSULTAExcel"] = sql1;
            DBFunctions.Request(ds, IncludeSchema.NO, sql);
            DBFunctions.Request(ds, IncludeSchema.NO, sql1);
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT MCUE_CODIPUC, MCUE_NOMBRE FROM MCUENTA");
            string[] titleFields = { "Cod Doc.", "Nº Comprobante", "Fecha", "Razon", "Fecha Proc. Débito", "Usuario Crédito", "Valor", "Débito NIIF", "Crédito NIIF" };
            string[] dataFields = { "Cod Doc.", "Nº Comprobante", "Fecha", "Razon", "Fecha Proc. Débito", "Usuario Crédito", "Valor", "Débito NIIF", "Crédito NIIF" };
            int i, j, k;
            for (i = 0; i < titleFields.Length; i++)
            {
                BoundColumn bc = new BoundColumn();
                bc.HeaderText = titleFields[i];
                bc.DataField = dataFields[i];
                bc.DataFormatString = "{0:N}";
                dg.Columns.Add(bc);
            }
            DataTable dtRep = new DataTable();
            for (i = 0; i < titleFields.Length; i++)
                dtRep.Columns.Add(new DataColumn(dataFields[i], typeof(string)));
            DataRow dr;
            DataRow[] rows;
            DataRow[] rowsC;
            double deb, cred, d, c, debNIIF, credNIIF, dNIIF, cNIIF;
            bool res = false;
            ArrayList Cods = new ArrayList();
            ArrayList Debs = new ArrayList();
            ArrayList Creds = new ArrayList();
            ArrayList CodsNIIF = new ArrayList();
            ArrayList DebsNIIF = new ArrayList();
            ArrayList CredsNIIF = new ArrayList();
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = dtRep.NewRow();
                dr[0] = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                dr[1] = ds.Tables[0].Rows[i].ItemArray[1].ToString();
                dr[2] = ds.Tables[0].Rows[i].ItemArray[2].ToString();
                dr[3] = ds.Tables[0].Rows[i].ItemArray[3].ToString();
                dr[4] = ds.Tables[0].Rows[i].ItemArray[4].ToString();
                dr[5] = ds.Tables[0].Rows[i].ItemArray[5].ToString();
                dr[6] = ((double)Convert.ToDouble(ds.Tables[0].Rows[i].ItemArray[6])).ToString("N");
                dtRep.Rows.Add(dr);
                rows = ds.Tables[1].Select("PDOC_CODIGO = '" + ds.Tables[0].Rows[i].ItemArray[0].ToString() + "' and MCOM_NUMEDOCU='" + ds.Tables[0].Rows[i].ItemArray[1].ToString() + "'");
                deb = cred = debNIIF = credNIIF = 0;
                for (j = 0; j < rows.Length; j++)
                {
                    string nombre = DBFunctions.SingleData("SELECT MCUE_NOMBRE FROM MCUENTA WHERE MCUE_CODIPUC = '" + rows[j][0] + "'");
                    dr = dtRep.NewRow();
                    dr[0] = "&raquo;" +  rows[j][0] + " - " + nombre;
                    dr[1] = rows[j][1] + " - " + rows[j][2];
                    dr[2] = rows[j][3];
                    dr[3] = rows[j][4] + " - " + rows[j][5];
                    dr[4] = ((double)Convert.ToDouble(rows[j][6])).ToString("N");
                    dr[5] = ((double)Convert.ToDouble(rows[j][7])).ToString("N");
                    dr[6] = ((double)Convert.ToDouble(rows[j][8])).ToString("N");
                    dr[7] = ((double)Convert.ToDouble(rows[j][11])).ToString("N");
                    dr[8] = ((double)Convert.ToDouble(rows[j][12])).ToString("N");
                    d = Convert.ToDouble(rows[j][6]);
                    c = Convert.ToDouble(rows[j][7]);
                    dNIIF = Convert.ToDouble(rows[j][11]);
                    cNIIF = Convert.ToDouble(rows[j][12]);
                    Inserta(Cods, Debs, Creds, rows[j][0].ToString(), d, c);
                    Inserta(CodsNIIF, DebsNIIF, CredsNIIF, rows[j][0].ToString(), dNIIF, cNIIF);
                    deb += d;
                    cred += c;
                    debNIIF += dNIIF;
                    credNIIF += cNIIF;
                    dtRep.Rows.Add(dr);
                }
                dr = dtRep.NewRow();
                if (Math.Abs(deb - cred) > 0.009 || Math.Abs(debNIIF - credNIIF) > 0.009)
                {
                    dr[2] = "  Sumas desiguales";
                    dr[3] = "  Diferencia:";
                    dr[6] = (deb - cred).ToString("N");
                }
                else
                    dr[2] = "  Sumas iguales";

                dr[4] = (deb).ToString("N");
                dr[5] = (cred).ToString("N");
                dr[7] = (debNIIF).ToString("N");
                dr[8] = (credNIIF).ToString("N");

                dtRep.Rows.Add(dr);
                res = false;
                if (i == ds.Tables[0].Rows.Count - 1) res = true;
                else
                    if (ds.Tables[0].Rows[i].ItemArray[0].ToString() != ds.Tables[0].Rows[i + 1].ItemArray[0].ToString()) res = true;
                if (res && Cods.Count > 0)
                { //Resumen
                    dr = dtRep.NewRow();
                    dr[3] = " RESUMEN:";
                    dtRep.Rows.Add(dr);
                    for (k = 0; k < Cods.Count; k++)
                    {
                        rowsC = ds.Tables[2].Select("MCUE_CODIPUC = '" + Cods[k].ToString().Trim() + "'");
                        dr = dtRep.NewRow();
                        dr[1] = Cods[k];
                        if (rowsC.Length > 0)
                            dr[2] = rowsC[0][1];
                        else
                            dr[2] = "Cuenta no definida en PUC.";
                        dr[4] = ((double)Convert.ToDouble(Debs[k])).ToString("N");
                        dr[5] = ((double)Convert.ToDouble(Creds[k])).ToString("N");
                        dr[7] = ((double)Convert.ToDouble(DebsNIIF[k])).ToString("N");
                        dr[8] = ((double)Convert.ToDouble(CredsNIIF[k])).ToString("N");
                        dtRep.Rows.Add(dr);
                    }
                    Cods.Clear();
                    Debs.Clear();
                    Creds.Clear();
                    CodsNIIF.Clear();
                    DebsNIIF.Clear();
                    CredsNIIF.Clear();
                }
            }
            
            dg.DataSource = dtRep;
            dg.DataBind();
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            tabPreHeader.RenderControl(htmlTW);
            dg.RenderControl(htmlTW);
            tabFirmas.RenderControl(htmlTW);
            string strRep;
            strRep = SB.ToString();
            Session.Clear();
            Session["Rep"] = strRep;
            toolsHolder.Visible = true;
        }

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                ds = DBFunctions.Request(ds, IncludeSchema.YES, ViewState["CONSULTAExcel"].ToString());
                Utils.ImprimeExcel(ds, "ConsultaTabla");
            }
            catch (Exception ex)
            {
                lblResult.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }
        } 
        public void Inserta(ArrayList co, ArrayList deb, ArrayList cred, string cd, double d, double c)
        {
            int a;
            if (!co.Contains(cd))
            {
                co.Add(cd);
                deb.Add(d.ToString());
                cred.Add(c.ToString());
            }
            else
            {
                a = co.IndexOf(cd);
                deb[a] = (Convert.ToDouble(deb[a])) + d;
                cred[a] = (Convert.ToDouble(cred[a])) + c;
            }
        }

        public void SendMail(Object Sender, ImageClickEventArgs E)
        {
            Consulta_Click(Sender, E);
            Utils.MostrarAlerta(Response, Press.PressOnEmail(reportTitle, tbEmail.Text, tabPreHeader, dg));
            dg.EnableViewState = false;
            //this.ClearChildViewState();
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

        protected void opcion_cambio(object sender, System.EventArgs e)
        {
            ddlNumComp.Visible = lblNumComp.Visible = lblTipoComp.Visible = typeDoc.Visible = (RadioOpcion.SelectedValue != "TC");
            if (RadioOpcion.SelectedValue == "UTC")
            {
                lblNumComp.Visible = false;
                ddlNumComp.Visible = false;
            }
            else if (RadioOpcion.SelectedValue == "UTE")
            {
                lblNumComp.Visible = true;
                ddlNumComp.Visible = true;
            }
        }

        protected void typeDoc_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlNumComp, "SELECT mcom_numedocu,mcom_numedocu from dbxschema.mcomprobante where PMES_MES = " + month.SelectedValue + " AND PANO_ANO =" + year.SelectedValue + " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "' order by mcom_numedocu;");
            if (ddlNumComp.Items.Count > 1)
                ddlNumComp.Items.Insert(0, "Seleccione:..");
        }

        protected void year_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlNumComp, "SELECT mcom_numedocu,mcom_numedocu from dbxschema.mcomprobante where PMES_MES = " + month.SelectedValue + " AND PANO_ANO =" + year.SelectedValue + " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "' order by mcom_numedocu;");
            if (ddlNumComp.Items.Count > 1)
                ddlNumComp.Items.Insert(0, "Seleccione:..");
        }

        protected void month_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlNumComp, "SELECT mcom_numedocu,mcom_numedocu from dbxschema.mcomprobante where PMES_MES = " + month.SelectedValue + " AND PANO_ANO =" + year.SelectedValue + " AND PDOC_CODIGO='" + typeDoc.SelectedValue + "' order by mcom_numedocu;");
            if (ddlNumComp.Items.Count > 1)
                ddlNumComp.Items.Insert(0, "Seleccione:..");
        }
    }
}

