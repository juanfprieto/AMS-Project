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


    public partial class CrearOSC : System.Web.UI.UserControl
    {
        //protected DropDownList ddlnumsol,ddlasesor;
        //protected LinkButton lnbabrsol;
        //protected CheckBox chbase;
        //protected Label lbEsc,lb;
        //protected Button btnCrear,btnAceptar;
        //protected DataGrid dgDetalle;
        public DataSet ds = new DataSet();
        protected DataTable dtTareas, dtDetalles;
        protected Panel pnlTareas;
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
        protected string pathToUploads = ConfigurationSettings.AppSettings["Uploads"];


        protected void Page_Load(object Sender, EventArgs e)
        {
            

            

            if (!IsPostBack)
            {
                if (Request.QueryString["nosc"] != "1")
                    Utils.FillDll(ddlnumsol, @"SELECT DISTINCT MS.msol_numero
                                    FROM msolicitud MS FULL OUTER JOIN mordenservicio MO ON (MS.MSOL_NUMERO = MO.MORD_NUMERO)
                                    WHERE ms.msol_numero IS NULL
                                    OR    mo.mord_numero IS NULL;", true);

                if (Request.QueryString["nosc"] == "1")
                {
                    Utils.FillDll(ddlnumsol, "SELECT CAST(MORD_NUMERO AS CHAR(5)) FROM MORDENSERVICIO WHERE TEST_CODIGO <> 'CE' \n" +
                                            "union  \n" +
                                            "select MORD_NUMERO CONCAT '-' CONCAT MORD_TAREA FROM mordenserviciotareas WHERE  TEST_CODIGO <> 'CE'", true);
                    chbase.Checked = true;
                    lbEsc.Visible = ddlasesor.Visible = true;
                }
                else if (ddlnumsol.Items.Count <= 1)
                {
                    cargarSolicitud(Sender, e);
                }

                Session.Clear();
                DatasToControls bind = new DatasToControls();
                if (Request.QueryString["nosc"] == "1")
                {
                    Utils.FillDll(ddlnumsol, "SELECT CAST(MORD_NUMERO AS CHAR(5)) FROM MORDENSERVICIO WHERE TEST_CODIGO <> 'CE' \n" +
                                            "union  \n" +
                                            "select MORD_NUMERO CONCAT '-' CONCAT MORD_TAREA FROM mordenserviciotareas WHERE  TEST_CODIGO <> 'CE'", true);
                    chbase.Checked = true;
                    lbEsc.Visible = ddlasesor.Visible = true;
                    Cargar_dtTareas();
                    DataBind_dgTareas();
                }
                else
                    Utils.FillDll(ddlnumsol, @"SELECT DISTINCT MS.msol_numero
                                    FROM msolicitud MS FULL OUTER JOIN mordenservicio MO ON (MS.MSOL_NUMERO = MO.MORD_NUMERO)
                                    WHERE ms.msol_numero IS NULL
                                    OR    mo.mord_numero IS NULL;", true);

                //bind.PutDatasIntoDropDownList(ddlasesor, "SELECT PASE.mnit_nit,PASE.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2 FROM pasesor PASE,mnit MNIT WHERE MNIT.mnit_nit=PASE.mnit_nit ORDER BY PASE.mnit_nit ASC");
                //bind.PutDatasIntoDropDownList(ddlproyecto, "SELECT MPRO_SECUENCIA, MPRO_NOMBRE FROM MPROYECTOSGC WHERE MPRO_VIGENCIA  = 'S'");
                Utils.FillDll(ddlasesor, "SELECT PASE.mnit_nit,PASE.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2 FROM pasesor PASE,mnit MNIT WHERE MNIT.mnit_nit=PASE.mnit_nit ORDER BY PASE.mnit_nit ASC", true);
                Utils.FillDll(ddlproyecto, "SELECT MPRO_SECUENCIA, MPRO_NOMBRE FROM MPROYECTOSGC WHERE MPRO_VIGENCIA  = 'S'", true);
                Session.Clear();
                Cargar_dtTareas();
                DataBind_dgTareas();

            }
            else
            {
                if (Session["dtDetalles"] != null)
                    dtDetalles = (DataTable)Session["dtDetalles"];
                else
                    LoadDataTableDetalles();
                
                if (Session["dtTareas"] != null)
                    dtTareas = (DataTable)Session["dtTareas"];

            }
        }

        protected void LoadDataTableDetalles()
        {
            dtDetalles = new DataTable();
            dtDetalles.Columns.Add("DETALLE");
            dtDetalles.Columns.Add("PROGRAMA");
            dtDetalles.Columns.Add("HORAS");
            Session["dtDetalles"] = dtDetalles;
        }

        protected void chbase_CheckedChanged(object Sender, EventArgs e)
        {
            if (((CheckBox)Sender).Checked)
                lbEsc.Visible = ddlasesor.Visible = lbproyecto.Visible = ddlproyecto.Visible = true;                
            else if (!((CheckBox)Sender).Checked)
                lbEsc.Visible = ddlasesor.Visible = lbproyecto.Visible = ddlproyecto.Visible =  false;
        }

        protected void btnAceptar_Click(object Sender, EventArgs e)
        {
            //dgDetalle.Visible=true;
            pnlDatos.Enabled = ddlnumsol.Enabled = chbase.Enabled = ddlasesor.Enabled = btnAceptar.Enabled = false;
            //btnCrear.Visible=true;
            //if(dtDetalle==null)
            //    Cargar_dtDetalle();
            //Llenar_dtDetalle();
            btnCrear_Click(Sender, e);
        }

        protected void btnCrear_Click(object Sender, EventArgs e)
        {
            string[] NumeroSolicitud = ddlnumsol.SelectedValue.Split('-');
            int numero = Convert.ToInt32(NumeroSolicitud[0]);
            int NumeroTarea = 0;
            ArrayList sqls = new ArrayList();
            dtDetalles = (DataTable)Session["dtDetalles"];

            if (Request.QueryString["nosc"] != "1")
            {
                ds = (DataSet)ViewState["ds"];
                dtTareas = (DataTable)Session["dtTareas"];
                //int numero=Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN MAX(mord_numero) IS NULL THEN 0 ELSE MAX(mord_numero) END FROM dbxschema.mordenservicio"))+1;
                string nitaper = DBFunctions.SingleData("SELECT mnit_nit FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");
                if (chbase.Checked)
                    sqls.Add("INSERT INTO mordenservicio VALUES(" + numero + "," + ddlnumsol.SelectedValue + ",'" + nitaper + "','" + ddlasesor.SelectedValue + "','AS','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "',null,null,null,null,null,null,null,null)");
                else
                    sqls.Add("INSERT INTO mordenservicio VALUES(" + numero + "," + ddlnumsol.SelectedValue + ",'" + nitaper + "',null,'AB','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "',null,null,null,null,null,null,null,null,null,null)");

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    sqls.Add("INSERT INTO dordenservicio VALUES(" + numero + "," + i.ToString() + ",'" + ds.Tables[1].Rows[i][0].ToString() + "','AS', '" + dtDetalles.Rows[i][2]  + "'," + ddlproyecto.SelectedValue + " )");

                //for (int i = 0; i < dtTareas.Rows.Count; i++)
                //{
                //    NumeroTarea = i + 1;
                //    if (dtTareas.Rows[i][1].ToString() != "")
                //        sqls.Add("INSERT INTO MORDENSERVICIOTAREAS VALUES (" + NumeroTarea + "," + numero + ",'" + dtTareas.Rows[i][1].ToString() + "','AS','" + dtTareas.Rows[i][0].ToString() + "')");
                //    else
                //        sqls.Add("INSERT INTO MORDENSERVICIOTAREAS VALUES (" + NumeroTarea + "," + numero + ",'" + dtTareas.Rows[i][1].ToString() + "','AB','" + dtTareas.Rows[i][0].ToString() + "')");
                //}

                sqls.Add("UPDATE msolicitud SET test_codigo='E' WHERE msol_numero=" + ddlnumsol.SelectedValue + "");

                if (DBFunctions.Transaction(sqls))
                {
                    Response.Redirect(indexPage + "?process=SAC_Asesoria.MostrarOSC&nosc=" + numero + "&inf=1");
                }
                else
                    lb.Text = "Error " + DBFunctions.exceptions;
            }
            else
            {
                if (ddlnumsol.SelectedValue.Contains("-") == false)
                {
                    for (int i = 0; i < dtDetalles.Rows.Count; i++)
                        sqls.Add("UPDATE DBXSCHEMA.DORDENSERVICIO SET DORD_ESTIMACIONHORA=" + dtDetalles.Rows[i][2] + " WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + " AND DORD_ID = " + i + ";");

                    sqls.Add("UPDATE MORDENSERVICIO SET PAS_MNIT_NIT='" + ddlasesor.SelectedValue + "', TEST_CODIGO= 'AS', MORD_FECHAASIG='" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "', MORD_HORAASIG='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' WHERE MORD_NUMERO=" + NumeroSolicitud[0].Trim() + "");

                    if (DBFunctions.Transaction(sqls))
                        Response.Redirect(indexPage + "?process=SAC_Asesoria.AdministrarOSC");
                }
                else
                {
                    sqls.Add("UPDATE mordenserviciotareas SET PAS_MNIT_NIT='" + ddlasesor.SelectedValue + "', TEST_CODIGO= 'AS' WHERE MORD_NUMERO=" + NumeroSolicitud[0].Trim() + " AND MORD_TAREA =" + NumeroSolicitud[1].Trim() + " ");
                    if (DBFunctions.Transaction(sqls))
                        Response.Redirect(indexPage + "?process=SAC_Asesoria.AdministrarOSC");
                }

            }
        }

        protected void btnCancelar_Click(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.AdministrarOSC");
        }


        protected void cargarSolicitud(object Sender, EventArgs e)
        {
            try
            {
                btnAceptar.Visible = true;
                string[] NumeroSolicitud = ddlnumsol.SelectedValue.Split('-');
                if (ddlnumsol.SelectedValue.Contains("-") == false)
                {
                    string Tiposol = DBFunctions.SingleData("SELECT TSOL_CODIGO FROM MSOLICITUD WHERE MSOL_NUMERO = " + NumeroSolicitud[0].Trim() + "");
                    if (Tiposol == "DE" && ViewState["Tareas"] == null)
                    {
                        pnlTareas.Visible = true;
                        Utils.FillDll(ddlAsesores, "SELECT PASE.mnit_nit,PASE.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2 FROM pasesor PASE,mnit MNIT WHERE MNIT.mnit_nit=PASE.mnit_nit ORDER BY PASE.mnit_nit ASC", true);
                        ViewState["Tareas"] = 1;
                    }
                    else if (Tiposol == "ER")
                    {
                        pnlTareas.Visible = false;
                    }
                    if (Tiposol == "DE")
                        pnlTareas.Visible = true;
                    pnlDetalleSolicitud.Visible = true;
                    if (chbase.Checked == true)
                    btnAceptar.Visible = true;

                    string sqlDetallesOrden = "";
                    if (DBFunctions.SingleData("select mord_numero from DORDENSERVICIO where mord_numero = " + NumeroSolicitud[0].Trim() + ";") == "")
                        sqlDetallesOrden =
                            @"SELECT DSOL.dsol_detalle AS DETALLE, SMP.SMEN_NOMBRE_PRINCIPAL CONCAT '/' CONCAT SMC.SMEN_NOMBRE_CARPETA CONCAT '/' CONCAT SMO.SMEN_NOMBRE_OPCION AS PROGRAMA, 0 as HORAS
                            FROM dbxschema.dsolicitud DSOL, SMENUOPCION SMO, SMENUCARPETA SMC, SMENUPRINCIPAL SMP
                            WHERE DSOL.ppro_id = SMO.SMEN_OPCION AND SMO.SMEN_CARPETA = SMC.SMEN_CARPETA AND SMC.SMEN_PRINCI = SMP.SMEN_PRINCI AND DSOL.msol_numero = " + NumeroSolicitud[0].Trim() + "; ";
                    else
                        sqlDetallesOrden =
                            @"SELECT DSOL.dsol_detalle AS DETALLE, SMP.SMEN_NOMBRE_PRINCIPAL CONCAT '/' CONCAT SMC.SMEN_NOMBRE_CARPETA CONCAT '/' CONCAT SMO.SMEN_NOMBRE_OPCION AS PROGRAMA, coalesce(d.dord_estimacionhora, 0) as HORAS 
                            FROM dbxschema.dsolicitud DSOL, SMENUOPCION SMO, SMENUCARPETA SMC, SMENUPRINCIPAL SMP, DORDENSERVICIO d 
                            WHERE DSOL.ppro_id = SMO.SMEN_OPCION AND SMO.SMEN_CARPETA = SMC.SMEN_CARPETA AND SMC.SMEN_PRINCI = SMP.SMEN_PRINCI AND DSOL.msol_numero = " + NumeroSolicitud[0].Trim() + " and " +
                            "DSOL.msol_numero = d.mord_numero and dsol.dsol_numero = d.dord_id order by d.dord_id;";

                    DBFunctions.Request(ds, IncludeSchema.NO, 
                                    "SELECT * FROM msolicitud WHERE msol_numero=" + NumeroSolicitud[0].Trim() + ";" +
                                    sqlDetallesOrden +
                                    "SELECT dsol_nombre AS ARCHIVO FROM dbxschema.dsolicitudarchivo WHERE msol_numero=" + NumeroSolicitud[0].Trim() + ";" +
                                    "SELECT MORD_DESCRIPCION AS DESCRIPCION,  \n" +
                                    "		PAS_MNIT_NIT AS ASESOR  \n" +
                                    "       FROM dbxschema.mordenserviciotareas  \n" +
                                    "WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + "");

                    lbnum.Text = Request.QueryString["num"];
                    lbnitcli.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
                    lbnomcli.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
                    lbnitcon.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
                    lbnomcon.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
                    lbfechor.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][3]).ToString("yyyy-MM-dd") + " - " + ds.Tables[0].Rows[0][4].ToString();

                    Session["dtDetalles"] = ds.Tables[1];
                    dgSolicitud.DataSource = ds.Tables[1];
                    dgSolicitud.DataBind();
                    dgArchivos.DataSource = ds.Tables[2];
                    dgArchivos.DataBind();

                    for (int i = 0; i < dgArchivos.Items.Count; i++)
                    {
                        ((HyperLink)dgArchivos.Items[i].Cells[1].FindControl("hpldes")).NavigateUrl = pathToUploads + dgArchivos.Items[i].Cells[0].Text;
                        ((HyperLink)dgArchivos.Items[i].Cells[1].FindControl("hpldes")).Target = "_blank";
                    }
                    dgTareas.DataSource = ds.Tables[3];
                    dgTareas.DataBind();
                }
                else
                {
                    pnlTareas.Visible = false;
                    pnlDetalleSolicitud.Visible = true;
                    btnAceptar.Visible = true;
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM msolicitud WHERE msol_numero=" + NumeroSolicitud[0].Trim() + ";" +
                                                            "SELECT MORD_DESCRIPCION AS Detalle,  \n" +
                                                            "			 'DESARROLLO' AS Programa  \n" +
                                                            "       FROM dbxschema.mordenserviciotareas  \n" +
                                                            "WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + " \n" +
                                                            "  AND MORD_TAREA = " + NumeroSolicitud[1].Trim() + ";" +
                                                            "SELECT dsol_nombre AS ARCHIVO FROM dbxschema.dsolicitudarchivo WHERE msol_numero=" + NumeroSolicitud[0].Trim() + "");
                    lbnum.Text = Request.QueryString["num"];
                    lbnitcli.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
                    lbnomcli.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
                    lbnitcon.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
                    lbnomcon.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
                    lbfechor.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][3]).ToString("yyyy-MM-dd") + " - " + ds.Tables[0].Rows[0][4].ToString();
                    dgSolicitud.DataSource = ds.Tables[1];
                    dgSolicitud.DataBind();
                    dgArchivos.DataSource = ds.Tables[2];
                    dgArchivos.DataBind();
                    for (int i = 0; i < dgArchivos.Items.Count; i++)
                    {
                        ((HyperLink)dgArchivos.Items[i].Cells[1].FindControl("hpldes")).NavigateUrl = pathToUploads + dgArchivos.Items[i].Cells[0].Text;
                        ((HyperLink)dgArchivos.Items[i].Cells[1].FindControl("hpldes")).Target = "_blank";
                    }
                }
                ViewState["ds"] = ds;
                if (Request.QueryString["nosc"] == "1")
                {
                    string nitAso = "";
                    string proyectoId = "";
                    if (ddlnumsol.SelectedValue.Contains("-") == false)
                    {
                        nitAso = DBFunctions.SingleData("SELECT PAS_MNIT_NIT FROM MORDENSERVICIO WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + "");
                        proyectoId = DBFunctions.SingleData("select mpro_secuencia from dordenservicio where mord_numero = " + NumeroSolicitud[0].Trim());
                        if (nitAso == "")
                        {
                            lbAsignado.Visible = true;
                            lbAsignado.Text = "No Esta Asignada Esta Orden";
                        }
                        else
                        {
                            lbAsignado.Visible = true;
                            lbAsignado.Text = "Solicitud Asignada a: " + DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + nitAso + "'");
                            ddlasesor.SelectedValue = nitAso;
                        }

                        if(proyectoId != "")
                        {
                            ddlproyecto.SelectedValue = proyectoId;
                        }
                    }
                    else
                    {
                        nitAso = DBFunctions.SingleData("SELECT PAS_MNIT_NIT FROM mordenserviciotareas WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + " AND MORD_TAREA = " + NumeroSolicitud[1].Trim() + "");
                        if (nitAso == "")
                        {
                            lbAsignado.Visible = true;
                            lbAsignado.Text = "No Esta Asignada Esta Orden";
                        }
                        else
                        {
                            lbAsignado.Visible = true;
                            lbAsignado.Text = "Solicitud Asignada a: " + DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + nitAso + "'");
                        }
                    }
                }
            }
            catch
            {
                Response.Redirect(indexPage + "?process=SAC_Asesoria.AdministrarOSC&nsolic=1");
            }
        }

        protected void dgTareas_ItemCommand(object Sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "agregar")
            {
                DataRow fila = dtTareas.NewRow();
                //String solicitud = txtRuta.Text + " - " + txtEspecificos.Text + " - " + txtDescripcion.Text;
                fila[0] = txtDetalle.Text;
                fila[1] = ddlAsesores.SelectedItem;
                dtTareas.Rows.Add(fila);
                DataBind_dgTareas();
            }
            else if (e.CommandName == "eliminar")
            {
                dtTareas.Rows[e.Item.DataSetIndex].Delete();
                dtTareas.AcceptChanges();
                DataBind_dgTareas();
            }
        }

        private void DataBind_dgTareas()
        {
            dgTareas.DataSource = dtTareas;
            dgTareas.DataBind();
            Session["dtTareas"] = dtTareas;
        }

        protected void Agregar_Tarea(object Sender, EventArgs e)
        {
            DataRow fila = dtTareas.NewRow();
            string Asesor;
            string alerta = "";
            if (ddlAsesores.SelectedValue == "0")
                Asesor = null;
            else
                Asesor = ddlAsesores.SelectedValue;
            if (txtDetalle.Text == "")
                alerta += "El campo de: datos de la tarea falta por llenar! ";

            if (alerta == "")
            {
                fila[0] = txtDetalle.Text;
                fila[1] = Asesor;
                dtTareas.Rows.Add(fila);
                DataBind_dgTareas();
                txtDetalle.Text = "";
                ddlAsesores.SelectedIndex = 0;
            }
            else
                Utils.MostrarAlerta(Response, "" + alerta + "");
        }

        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            if (dtDetalles.Rows.Count > 0)
                dgSolicitud.EditItemIndex = (int)e.Item.ItemIndex;

            BindDatas();
        }

        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgSolicitud.EditItemIndex = -1;
            BindDatas();
        }

        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {	//Este selecciona la fila- Nos permite la edicion de un item agregado
            if (dtDetalles.Rows.Count == 0)//Como no hay nada, no se pone a actualizar nada
                return;

            string horas =((TextBox)e.Item.Cells[2].Controls[1]).Text;
            dtDetalles.Rows[dgSolicitud.EditItemIndex][2] = horas;

            dgSolicitud.EditItemIndex = -1;
            BindDatas();
        }

        protected void BindDatas()
        {
            dgSolicitud.EnableViewState = true;
            dgSolicitud.DataSource = dtDetalles;
            Session["dtDetalles"] = dtDetalles;
            dgSolicitud.DataBind();
        }

        private void Cargar_dtTareas()
        {
            dtTareas = new DataTable();
            dtTareas.Columns.Add("DESCRIPCION", typeof(string));
            dtTareas.Columns.Add("ASESOR", typeof(string));
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
