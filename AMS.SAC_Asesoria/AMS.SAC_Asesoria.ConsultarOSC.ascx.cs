namespace AMS.SAC_Asesoria
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mail;
    using System.Web.Security;
    using System.Web.SessionState;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.Configuration;
    using AMS.DB;
    using AMS.Forms;
    using AMS.Tools;

    public partial class ConsultarOSC : System.Web.UI.UserControl
    {
        //protected Panel pnlFilFec, pnlFilEst, pnlFilCli;
        //protected TextBox fecha, fechafin;
        //protected Calendar calendarioFecha, calendarioFecha2;
        //protected DropDownList ddlnitcli, ddlestosc;
        //protected Button btnconsfec, btnconsest, btnconscli;
        //protected DataGrid dgOSC;
        protected PlaceHolder plcExcel;
        protected DataTable dtOSC;
        protected DataTable dtOSCaux;
        //protected Label lb;
        protected string selectmord = "";
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];

        protected void Page_Load(object Sender, EventArgs e)
        {
            selectmord = "SELECT MORD.mord_numero,MSOL.msol_numero,MSOL.mnit_nitcli CONCAT ' - ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2,MSOL.mnit_nitcon CONCAT ' - ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2,TEST.test_nombre,MORD.mord_fecha,MORD.mord_hora FROM dbxschema.mordenservicio MORD,dbxschema.msolicitud MSOL,dbxschema.mnit MNIT,dbxschema.mnit MNIT2,dbxschema.testadoorden TEST WHERE MORD.msol_numero=MSOL.msol_numero AND MORD.test_codigo=TEST.test_codigo AND MSOL.mnit_nitcli=MNIT.mnit_nit AND MSOL.mnit_nitcon=MNIT2.mnit_nit ";
            if (!IsPostBack)
            {
                String tipoUser = DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login = '" + HttpContext.Current.User.Identity.Name.ToLower() + "';");
                DatasToControls bind = new DatasToControls();
                Session.Clear();

                if (tipoUser == "US")
                {
                    pnlFilCli.Visible = true;
                    String nitContacto = DBFunctions.SingleData("SELECT mnit_nit FROM dbxschema.susuario WHERE susu_login = '" + HttpContext.Current.User.Identity.Name.ToLower() + "';");
                    bind.PutDatasIntoDropDownList(ddlnitcli,
                        @"SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2
                        FROM  dbxschema.mnit MNIT, dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit = MCLI.mnit_nit
                        and mcli.mnit_nit in (select mnit_nitcli from mcontacto where mnit_nitcon = '"+ nitContacto + "' );" );
                }
                else if (Request.QueryString["Usu"] == "Ad")
                {
                    if (Request.QueryString["Fil"] == "F")
                    {
                        pnlFilFec.Visible = true;
                        fecha.Text = fechafin.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    }
                    else if (Request.QueryString["Fil"] == "E")
                    {
                        pnlFilEst.Visible = true;
                        bind.PutDatasIntoDropDownList(ddlestosc, "SELECT test_codigo,test_nombre FROM testadoorden");
                    }
                    else if (Request.QueryString["Fil"] == "C")
                    {
                        pnlFilCli.Visible = true;
                        bind.PutDatasIntoDropDownList(ddlnitcli, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2 FROM  dbxschema.mnit MNIT, dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit");
                    }
                }
            }
            else
            {
                plcExcel.Visible = true;
            }
        }

        protected void Cambiar_Fecha(object Sender, EventArgs e)
        {
            fecha.Text = calendarioFecha.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void Cambiar_Fecha2(object Sender, EventArgs e)
        {
            fechafin.Text = calendarioFecha2.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void btnconsfec_Click(object Sender, EventArgs e)
        {
            if (Convert.ToDateTime(fecha.Text) > Convert.ToDateTime(fechafin.Text))
                Utils.MostrarAlerta(Response, "La fecha inicial no se puede ser mayor a la fecha final");
            else
            {
                string where = "AND MORD.mord_fecha BETWEEN '" + fecha.Text + "' AND '" + fechafin.Text + "' ORDER BY MORD.mord_numero";
                DataSet ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, selectmord + where);
                ViewState["dsAuxi"] = ds;
                Llenar_dtOSC(ds);
            }
        }

        protected void btnconsest_Click(object Sender, EventArgs e)
        {
            string where = "AND MORD.test_codigo='" + ddlestosc.SelectedValue + "' ORDER BY MORD.mord_numero";
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, selectmord + where);
            ViewState["dsAuxi"] = ds;
            Llenar_dtOSC(ds);
        }

        protected void btnconscli_Click(object Sender, EventArgs e)
        {
            string where = "AND MSOL.mnit_nitcli='" + ddlnitcli.SelectedValue + "' ORDER BY MORD.mord_numero";
            //lb.Text=select+where;
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, selectmord + where);
            ViewState["dsAuxi"] = ds;
            Llenar_dtOSC(ds);
        }

        protected void dgOSC_ItemCommand(object Sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "vse")
                Response.Redirect(indexPage + "?process=SAC_Asesoria.MostrarSolicitud&num=" + e.Item.Cells[1].Text + "");
            else if (e.CommandName == "voc")
                Response.Redirect(indexPage + "?process=SAC_Asesoria.MostrarOSC&nosc=" + e.Item.Cells[0].Text + "");
        }

        private void Cargar_dtOSC()
        {
            dtOSC = new DataTable();
            dtOSC.Columns.Add("NUMOSC", typeof(int));
            dtOSC.Columns.Add("NUMSOL", typeof(int));
            dtOSC.Columns.Add("CLIENTE", typeof(string));
            dtOSC.Columns.Add("CONTACTO", typeof(string));
            dtOSC.Columns.Add("ESTADO", typeof(string));
            dtOSC.Columns.Add("FECHOR", typeof(string));

            dtOSCaux = new DataTable();
            dtOSCaux.Columns.Add("NUMOSC", typeof(int));
            dtOSCaux.Columns.Add("NUMSOL", typeof(int));
            dtOSCaux.Columns.Add("CLIENTE", typeof(string));
            dtOSCaux.Columns.Add("CONTACTO", typeof(string));
            dtOSCaux.Columns.Add("ESTADO", typeof(string));
            dtOSCaux.Columns.Add("FECHOR", typeof(string));
        }

        private void Llenar_dtOSC(DataSet dat)
        {
            DataRow fila;
            DataRow filaAux;

            if (dat.Tables[0].Rows.Count != 0)
            {
                if (dtOSC == null)
                    Cargar_dtOSC();
                for (int i = 0; i < dat.Tables[0].Rows.Count; i++)
                {
                    fila = dtOSC.NewRow();
                    fila[0] = Convert.ToInt32(dat.Tables[0].Rows[i][0]);
                    fila[1] = Convert.ToInt32(dat.Tables[0].Rows[i][1]);
                    fila[2] = dat.Tables[0].Rows[i][2].ToString();
                    fila[3] = dat.Tables[0].Rows[i][3].ToString();
                    fila[4] = dat.Tables[0].Rows[i][4].ToString();
                    fila[5] = Convert.ToDateTime(dat.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd") + " - " + dat.Tables[0].Rows[i][6].ToString();
                    dtOSC.Rows.Add(fila);
                    dgOSC_BindData();

                    filaAux = dtOSCaux.NewRow();
                    filaAux[0] = Convert.ToInt32(dat.Tables[0].Rows[i][0]);
                    filaAux[1] = Convert.ToInt32(dat.Tables[0].Rows[i][1]);
                    filaAux[2] = dat.Tables[0].Rows[i][2].ToString();
                    filaAux[3] = dat.Tables[0].Rows[i][3].ToString();
                    filaAux[4] = dat.Tables[0].Rows[i][4].ToString();
                    filaAux[5] = Convert.ToDateTime(dat.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd") + " - " + dat.Tables[0].Rows[i][6].ToString();
                    dtOSCaux.Rows.Add(filaAux);
                }
            }
            else
                Utils.MostrarAlerta(Response, "No hay coincidencias para esta busqueda");
        }

        private void dgOSC_BindData()
        {
            dgOSC.DataSource = dtOSC;
            dgOSC.DataBind();
        }

        protected void Excel(object sender, EventArgs e)
        {
            //toca recargar el dttable.
            Cargar_dtOSC();
            Llenar_dtOSC((DataSet)ViewState["dsAuxi"]);
            DateTime fecha = DateTime.Now;
            string nombreArchivo = "ConsultaCliente" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
            base.Response.Clear();
            base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
            base.Response.Charset = "Unicode";
            base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            base.Response.ContentType = "application/vnd.xls";
            StringWriter stringWrite = new StringWriter();
            HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

            DataGrid dgAux = new DataGrid();
            dgAux.DataSource = dtOSCaux;
            dgAux.DataBind();
            dgAux.RenderControl(htmlWrite);

            base.Response.Write(stringWrite.ToString());
            base.Response.End();
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
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}

