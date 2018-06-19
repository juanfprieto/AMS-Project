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
    using AMS.Reportes;

    public partial class ConsultarSolicitud_SAC : System.Web.UI.UserControl
    {
        protected Panel pnlFilFec, pnlFilCon, pnlFilCli;
        protected TextBox fecha, fechafin;
        protected Calendar calendarioFecha, calendarioFecha2;
        protected DropDownList ddlnitcli, ddlnitcli2, ddlnitcon;
        protected Button btnconsfec, btnconscon, btnconscli;
        protected DataGrid dgMaeSol, dgDetSol;
        protected DataTable dtMaeSol, dtDetSol;
        protected Label lb;
        protected string select = "";
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        protected PlaceHolder plcExcel;
        protected DataTable dtOSCaux;

        protected void Page_Load(object Sender, EventArgs e)
        {
            //F Fecha N Contacto C Cliente
            //	select="SELECT MSOL.msol_numero,MSOL.mnit_nitcli CONCAT ' - ' CONCAT MNIT.mnit_nombre,MSOL.mnit_nitcon CONCAT ' - ' CONCAT MNIT2.mnit_nombre,MSOL.msol_fecha,MSOL.msol_hora FROM dbxschema.msolicitud MSOL,dbxschema.mnit MNIT,dbxschema.mnit MNIT2 WHERE MSOL.mnit_nitcli=MNIT.mnit_nit AND MSOL.mnit_nitcon=MNIT2.mnit_nit ";
            select = "SELECT MSOL.msol_numero,MSOL.mnit_nitcli CONCAT ' - ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2,MSOL.mnit_nitcon CONCAT ' - ' " +
                   "CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2,MSOL.msol_fecha,MSOL.msol_hora, " +
                   "coalesce(te.test_nombre, 'SIN ASIGNAR') " +
                   "FROM dbxschema.mnit MNIT,dbxschema.mnit MNIT2, dbxschema.msolicitud MSOL " +
                   "LEFT JOIN MORDENSERVICIO MO ON MSOL.msol_numero = MO.msol_numero LEFT JOIN TESTADOORDEN TE ON MO.TEST_CODIGO = TE.TEST_CODIGO " +
                   "WHERE MSOL.mnit_nitcli=MNIT.mnit_nit AND MSOL.mnit_nitcon=MNIT2.mnit_nit ";
            if (!IsPostBack)
            {
                Session.Clear();
                if (Request.QueryString["Usu"] == "Ad")
                {
                    DatasToControls bind = new DatasToControls();
                    if (Request.QueryString["Fil"] == "F")
                    {
                        pnlFilFec.Visible = true;
                        fecha.Text = fechafin.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    }
                    else if (Request.QueryString["Fil"] == "N")
                    {
                        pnlFilCon.Visible = true;
                        bind.PutDatasIntoDropDownList(ddlnitcli, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2 FROM  dbxschema.mnit MNIT, dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit");
                        bind.PutDatasIntoDropDownList(ddlnitcon, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2 FROM  dbxschema.mnit MNIT, dbxschema.mcontacto MCON WHERE MNIT.mnit_nit=MCON.mnit_nitcon AND MCON.mnit_nitcli='" + ddlnitcli.SelectedValue + "'");
                    }
                    else if (Request.QueryString["Fil"] == "C")
                    {
                        pnlFilCli.Visible = true;
                        bind.PutDatasIntoDropDownList(ddlnitcli2, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2 FROM  dbxschema.mnit MNIT, dbxschema.mclientesac MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit;");
                    }
                }
            }
            else
            {
                //plcExcel.Visible = true;
            }
            /*else
            {
                if(Session["dtMaeSol"]!=null)
                    dtMaeSol=(DataTable)Session["dtMaeSol"];
                if(Session["dtDetSol"]!=null)
                    dtDetSol=(DataTable)Session["dtDetSol"];
            }*/
        }

        protected void Cambiar_Fecha(object Sender, EventArgs e)
        {
            fecha.Text = calendarioFecha.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void Cambiar_Fecha2(object Sender, EventArgs e)
        {
            fechafin.Text = calendarioFecha2.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void ddlnitcli_IndexChanged(object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlnitcon, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres concat ' ' CONCAT MNIT.mnit_nombre2 concat ' ' CONCAT MNIT.mnit_apellidos concat ' ' CONCAT MNIT.mnit_apellido2 FROM  dbxschema.mnit MNIT, dbxschema.mcontacto MCON WHERE MNIT.mnit_nit=MCON.mnit_nitcon AND MCON.mnit_nitcli='" + ddlnitcli.SelectedValue + "'");
        }

        protected void btnconsfec_Click(object Sender, EventArgs e)
        {
            dgDetSol.DataBind();
            if (Convert.ToDateTime(fecha.Text) > Convert.ToDateTime(fechafin.Text))
                Utils.MostrarAlerta(Response, "La fecha inicial no se puede ser mayor a la fecha final");
            else
            {
                string where = "AND MSOL.msol_fecha BETWEEN '" + fecha.Text + "' AND '" + fechafin.Text + "' ORDER BY MSOL.msol_numero";
                DataSet ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, select + where);
                ViewState["dsAuxi"] = ds;
                plcExcel.Visible = true;
                Llenar_dtMaeSol(ds);
            }
        }

        protected void btnconscon_Click(object Sender, EventArgs e)
        {
            dgDetSol.DataBind();
            string where = "AND MSOL.mnit_nitcon='" + ddlnitcon.SelectedValue + "' ORDER BY MSOL.msol_numero";
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, select + where);
            ViewState["dsAuxi"] = ds;
            plcExcel.Visible = true;
            Llenar_dtMaeSol(ds);
        }

        protected void btnconscli_Click(object Sender, EventArgs e)
        {
            dgDetSol.DataBind();
            string where = "AND MSOL.mnit_nitcli='" + ddlnitcli2.SelectedValue + "' ORDER BY MSOL.msol_numero";
            //lb.Text=select+where;
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, select + where);
            ViewState["dsAuxi"] = ds;
            plcExcel.Visible = true;
            Llenar_dtMaeSol(ds);
        }

        protected void dgMaeSol_ItemCommand(object Sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "verdetalles")
            {
                DataSet ds = new DataSet();
                string sql = "SELECT " +
                             "MO.MSOL_NUMERO as Solicitud, " +
                             "OS.dord_id as Orden, " +
                             "OS.dord_detalle as Detalle, " +
                             "ORS.DORD_RESPUESTA as respuesta, " +
                             "TE.TEST_NOMBRE as Estado " +
                             "FROM " +
                             "MORDENSERVICIO MO, " +
                             "TESTADOORDEN TE, " +
                             "dordenservicio OS left join DORDENSOLUCION ORS ON OS.MORD_NUMERO = ORS.MORD_NUMERO AND OS.DORD_ID = ORS.DORD_IDDET " +
                             "WHERE " +
                             "MO.MORD_NUMERO = OS.MORD_NUMERO AND " +
                             "MO.TEST_CODIGO = TE.TEST_CODIGO AND " +
                             "OS.mord_numero=" + e.Item.Cells[0].Text + " ";

                DBFunctions.Request(ds, IncludeSchema.NO, sql);
                dgDetSol.DataSource = ds.Tables[0];
                dgDetSol.DataBind();
            }
            else if (e.CommandName == "impresion")
            {
                Response.Redirect(indexPage + "?process=Asesoria.MostrarSolicitud&num=" + e.Item.Cells[0].Text + "");
            }
        }

        private void Cargar_dtMaeSol()
        {
            dtMaeSol = new DataTable();
            dtMaeSol.Columns.Add("NUMERO", typeof(int));
            dtMaeSol.Columns.Add("CLIENTE", typeof(string));
            dtMaeSol.Columns.Add("CONTACTO", typeof(string));
            dtMaeSol.Columns.Add("FECHOR", typeof(string));
            dtMaeSol.Columns.Add("ESTADO", typeof(string));

            dtOSCaux = new DataTable();
            dtOSCaux.Columns.Add("NUMERO", typeof(int));
            dtOSCaux.Columns.Add("CLIENTE", typeof(string));
            dtOSCaux.Columns.Add("CONTACTO", typeof(string));
            dtOSCaux.Columns.Add("FECHOR", typeof(string));
            dtOSCaux.Columns.Add("ESTADO", typeof(string));
        }

        private void Llenar_dtMaeSol(DataSet dat)
        {
            DataRow fila;
            DataRow filaAux;

            if (dat.Tables[0].Rows.Count != 0)
            {
                if (dtMaeSol == null)
                    Cargar_dtMaeSol();
                for (int i = 0; i < dat.Tables[0].Rows.Count; i++)
                {
                    fila = dtMaeSol.NewRow();
                    fila[0] = Convert.ToInt32(dat.Tables[0].Rows[i][0]);
                    fila[1] = dat.Tables[0].Rows[i][1].ToString();
                    fila[2] = dat.Tables[0].Rows[i][2].ToString();
                    fila[3] = Convert.ToDateTime(dat.Tables[0].Rows[i][3]).ToString("yyyy-MM-dd") + " - " + dat.Tables[0].Rows[i][4].ToString();
                    fila[4] = dat.Tables[0].Rows[i][5].ToString();
                    dtMaeSol.Rows.Add(fila);
                    dgMaeSol_BindData();

                    filaAux = dtOSCaux.NewRow();
                    filaAux[0] = Convert.ToInt32(dat.Tables[0].Rows[i][0]);
                    filaAux[1] = dat.Tables[0].Rows[i][1].ToString();
                    filaAux[2] = dat.Tables[0].Rows[i][2].ToString();
                    filaAux[3] = Convert.ToDateTime(dat.Tables[0].Rows[i][3]).ToString("yyyy-MM-dd") + " - " + dat.Tables[0].Rows[i][4].ToString();
                    filaAux[4] = dat.Tables[0].Rows[i][5].ToString();
                    dtOSCaux.Rows.Add(filaAux);
                }
            }
            else
                Utils.MostrarAlerta(Response, "No hay coincidencias para esta busqueda");
        }

        private void dgMaeSol_BindData()
        {
            dgMaeSol.DataSource = dtMaeSol;
            dgMaeSol.DataBind();
        }

        protected void Excel(object sender, EventArgs e)
        {
            //GeneradorExcel g = new GeneradorExcel();
            //g.generar();

            //toca recargar el dttable.
            Cargar_dtMaeSol();
            Llenar_dtMaeSol((DataSet)ViewState["dsAuxi"]);
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
