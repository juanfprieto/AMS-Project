namespace AMS.SAC_Asesoria
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using System.Web.Security;
    using System.Web.SessionState;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using AMS.DB;
    using AMS.Forms;
    using System.Configuration;
    using AMS.Tools;

    public partial class CrearSolicitud : System.Web.UI.UserControl
    {

        //protected DataGrid dgSolicitud;
        protected DataTable dtSolicitud;
        //protected Button btnEnviar,btnCancelar,btnAgregar;
        //protected Label lb,lbArchivos;
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        protected string path = ConfigurationSettings.AppSettings["PathToUploads"];
        //protected Panel pnlCliente;
        //protected DropDownList ddlnitcli,ddlnitcon,ddltipsol;
        //protected HtmlInputFile uplFile;
        protected ArrayList archivos = new ArrayList();


        protected void Page_Load(object Sender, EventArgs e)
        {
            ER.Checked = true;
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.SAC_Asesoria.CrearSolicitud));

            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                llenarDdlPrincipal();
                ddlMenuPrin_OnSelectedIndexChanged(Sender, e);
               

                if (DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") != "US" )
                {
                    bntGuardarSOl.Visible = false; 
                    pnlCliente.Visible = true;

                    bind.PutDatasIntoDropDownList(ddlnitcli,
                        @"SELECT MNIT.mnit_nit, case when tnit_tiponit = 'N' THEN coalesce(MNIT.mnit_apellidos,'') ELSE coalesce(MNIT.mnit_nombres,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_nombre2,'') CONCAT ' ' CONCAT 
                        coalesce(MNIT.mnit_apellidos,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_apellido2,'') END AS CLIENTE
                        FROM  
                        dbxschema.mnit MNIT, dbxschema.MCLIENTESAC MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit AND MCLI.TVIG_CODIGO='V' ORder by 2");

                    bind.PutDatasIntoDropDownList(ddlnitcon,
                        @"SELECT MNIT.mnit_nit, coalesce(MNIT.mnit_nombres,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_nombre2,'') CONCAT ' ' CONCAT
                            coalesce(MNIT.mnit_apellidos,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_apellido2,'') 
                        FROM  
                            dbxschema.mnit MNIT, dbxschema.mcontacto MCON 
                        WHERE
                            MNIT.mnit_nit=MCON.mnit_nitcon AND MCON.mnit_nitcli='" + ddlnitcli.SelectedValue + "' AND MCON.test_codigo='A'");


                    Session.Clear();
                    Cargar_dtSolicitud();
                    DataBind_dgSolicitud();

                }
                //else if(DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"'")=="US")
                else
                {
                    pnlCliente.Visible = true;

                    string nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "';");
                    
                    bind.PutDatasIntoDropDownList(ddlnitcli,
                          @"SELECT MNIT.mnit_nit, case when tnit_tiponit = 'N' THEN coalesce(MNIT.mnit_apellidos,'') ELSE coalesce(MNIT.mnit_nombres,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_nombre2,'') CONCAT ' ' CONCAT 
                            coalesce(MNIT.mnit_apellidos,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_apellido2,'') END AS CLIENTE 
                            FROM  
                            dbxschema.mnit MNIT, dbxschema.MCLIENTESAC MCLI WHERE MNIT.mnit_nit=MCLI.mnit_nit AND MCLI.TVIG_CODIGO='V' 
                            and MCLI.mnit_nit in (select mnit_nitcli from mcontacto where mnit_nitcon='" + nitCliente + "') ORder by 2;");

                    bind.PutDatasIntoDropDownList(ddlnitcon,
                        @"SELECT MNIT.mnit_nit, coalesce(MNIT.mnit_nombres,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_nombre2,'') CONCAT ' ' CONCAT
                            coalesce(MNIT.mnit_apellidos,'') CONCAT ' ' CONCAT coalesce(MNIT.mnit_apellido2,'') 
                        FROM  
                            dbxschema.mnit MNIT, dbxschema.mcontacto MCON 
                        WHERE
                            MNIT.mnit_nit=MCON.mnit_nitcon AND MCON.mnit_nitcli='" + ddlnitcli.SelectedValue + "' AND MCON.test_codigo='A'");

                    if (DBFunctions.SingleData("SELECT MCLI.tvig_codigo FROM dbxschema.MCLIENTESAC MCLI,dbxschema.mcontacto MCON ,dbxschema.susuario SUSU  WHERE MCON.susu_codigo=SUSU.susu_codigo AND MCLI.mnit_nit=MCON.mnit_nitcli AND MCON.susu_codigo= (SELECT susu_codigo FROM dbxschema.susuario  WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')") == "M")
                    {
                        Utils.MostrarAlerta(Response, "Este cliente se encuentra en mora con Sistemas Ecas");
                        Session.Clear();
                        Cargar_dtSolicitud();
                        DataBind_dgSolicitud();
                    }
                    else if (DBFunctions.SingleData("SELECT MCLI.tvig_codigo FROM dbxschema.MCLIENTESAC MCLI,dbxschema.mcontacto MCON,dbxschema.susuario SUSU WHERE MCLI.mnit_nit=MCON.mnit_nitcli AND MCON.susu_codigo=SUSU.susu_codigo AND MCON.susu_codigo= (SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')") == "C")
                            Utils.MostrarAlerta(Response, "Este cliente no tiene contrato con Sistemas Ecas en la actualidad. Contactese con nosotros");
                         else if (DBFunctions.SingleData("SELECT MCLI.tvig_codigo FROM dbxschema.MCLIENTESAC MCLI,dbxschema.mcontacto MCON,dbxschema.susuario SUSU WHERE MCLI.mnit_nit=MCON.mnit_nitcli AND MCON.susu_codigo=SUSU.susu_codigo AND MCON.susu_codigo=(SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')") == "V")
                        {
                            Session.Clear();
                            Cargar_dtSolicitud();
                            DataBind_dgSolicitud();
                        }
                }

                //if (DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "AS" || DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "UE")
                //  bind.PutDatasIntoDropDownList(ddlModulo, "SELECT PPRO.ppro_id,PPRO.ppro_descripcion FROM dbxschema.pproducto PPRO,dbxschema.MCLIENTESAC MCLI,dbxschema.dclienteproductos DCLI WHERE MCLI.mnit_nit=DCLI.mnit_nit AND PPRO.ppro_id=DCLI.ppro_id AND DCLI.mnit_nit='" + ddlnitcli.SelectedValue + "'");
                //else if (DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "US")
                //   bind.PutDatasIntoDropDownList( "SELECT PPRO.ppro_id,PPRO.ppro_descripcion FROM dbxschema.pproducto PPRO,dbxschema.MCLIENTESAC MCLI,dbxschema.dclienteproductos DCLI WHERE MCLI.mnit_nit=DCLI.mnit_nit AND PPRO.ppro_id=DCLI.ppro_id AND DCLI.mnit_nit=(SELECT mnit_nitcli FROM dbxschema.mcontacto	WHERE susu_codigo=(SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'))");

            }
            else
            {
                if (Session["dtSolicitud"] != null)
                    dtSolicitud = (DataTable)Session["dtSolicitud"];
                if (Session["archivos"] != null)
                    archivos = (ArrayList)Session["archivos"];
            }
        }

        protected void ddlnitcli_IndexChanged(object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlnitcon, "SELECT MNIT.mnit_nit,MNIT.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2  FROM  dbxschema.mnit MNIT, dbxschema.mcontacto MCON WHERE MNIT.mnit_nit=MCON.mnit_nitcon AND MCON.mnit_nitcli='" + ddlnitcli.SelectedValue + "'");
            this.DataBind_dgSolicitud();
        }

        protected void dgSolicitud_ItemCommand(object Sender, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "agregar")
            {
                DataRow fila = dtSolicitud.NewRow();
                //String solicitud = txtRuta.Text + " - " + txtEspecificos.Text + " - " + txtDescripcion.Text;
                fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                fila[1] = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue;
                dtSolicitud.Rows.Add(fila);
                DataBind_dgSolicitud();
                btnEnviar.Enabled = true;
            }
            else if (e.CommandName == "eliminar")
            {
                dtSolicitud.Rows[e.Item.DataSetIndex].Delete();
                dtSolicitud.AcceptChanges();
                if (Verificar_dtSolicitud())
                    btnEnviar.Enabled = false;
                DataBind_dgSolicitud();
            }
        }

        protected void dgSolicitud_ItemDataBound(object Sender, DataGridItemEventArgs e)
        {
            Cargar_ddlprog(e);
        }

        protected void btnAgregar_Click(object Sender, EventArgs e)
        {
            if (this.uplFile.PostedFile.FileName.Length == 0)
                Utils.MostrarAlerta(Response, "No ha especificado un nombre de archivo");
            else if (Verificar_dtSolicitud())
                Utils.MostrarAlerta(Response, "No ha especificado solicitudes");
            else
            {
                HttpPostedFile file = uplFile.PostedFile;
                archivos.Add(file);
                Session["archivos"] = archivos;
                lbArchivos.Text += file.FileName + "<br>";
            }
        }

        protected void btnEnviar_Click(object Sender, EventArgs e)
        {

            string TipoSol;
            if ((ER.Checked || DE.Checked) == false)
            {
                Utils.MostrarAlerta(Response, "Por Favor Seleccione un tipo de solicitud !!!");
            }

            if (ER.Checked)
            {
                TipoSol = "ER";
            }
            else
            {
                TipoSol = "DE";
            }

            //CODIGO DE USUARIO DE SESION ACTUAL
            string codigoUsu = DBFunctions.SingleData("SELECT SUSU_CODIGO FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
            ArrayList sqls = new ArrayList();
            if (!Verificar_dtSolicitud())
            {
                string opcionNumero;
                int numero = Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN MAX(msol_numero) IS NULL THEN 0 ELSE MAX(msol_numero) END FROM dbxschema.msolicitud")) + 1;
                if (DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "AS" || DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "UE")
                {
                    sqls.Add("INSERT INTO msolicitud VALUES(" + numero + ",'" + ddlnitcli.SelectedValue + "','" + ddlnitcon.SelectedValue + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "','" + TipoSol + "',default," + codigoUsu + ")");
                    for (int i = 0; i < dtSolicitud.Rows.Count; i++)
                    {
                        opcionNumero = dtSolicitud.Rows[i][1].ToString();
                        string[] splitOpcion = opcionNumero.Split('-');
                        sqls.Add("INSERT INTO dsolicitud VALUES(" + numero + "," + i.ToString() + ",'" + dtSolicitud.Rows[i][0].ToString() + "'," + splitOpcion[0] + ")");
                    }
                    if (archivos.Count != 0)
                    {
                        for (int i = 0; i < archivos.Count; i++)
                        {
                            string[] nomArch = ((HttpPostedFile)archivos[i]).FileName.Split('\\');
                            sqls.Add("INSERT INTO dsolicitudarchivo VALUES(" + numero + "," + i.ToString() + ",'" + nomArch[nomArch.Length - 1] + "')");
                        }
                    }
                    if (DBFunctions.Transaction(sqls))
                    {
                        for (int i = 0; i < archivos.Count; i++)
                        {
                            string[] nombre = ((HttpPostedFile)archivos[i]).FileName.Split('\\');
                            ((HttpPostedFile)archivos[i]).SaveAs(path + nombre[nombre.Length - 1]);
                        }
                        Response.Redirect(indexPage + "?process=SAC_Asesoria.MostrarSolicitud&num=" + numero + "&inf=1");
                    }
                    else
                        lb.Text = "<br>" + DBFunctions.exceptions;
                }
                else
                {
                    string nitcon = DBFunctions.SingleData("SELECT MCON.mnit_nitcon FROM dbxschema.mcontacto MCON, dbxschema.susuario SUSU WHERE MCON.susu_codigo=SUSU.susu_codigo AND MCON.susu_codigo=(SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
                    string nitcli = DBFunctions.SingleData("SELECT mnit_nitcli FROM mcontacto WHERE mnit_nitcon='" + nitcon + "'");
                    //sqls.Add("INSERT INTO msolicitud VALUES(" + numero + ",'" + nitcli + "','" + nitcon + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "','" + TipoSol + "',default," + codigoUsu + ")");
                    sqls.Add("INSERT INTO msolicitud VALUES(" + numero + ",'" + ddlnitcli.SelectedValue + "','" + nitcon + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "','" + TipoSol + "',default," + codigoUsu + ")");
                    for (int i = 0; i < dtSolicitud.Rows.Count; i++)
                    {
                        opcionNumero = dtSolicitud.Rows[i][1].ToString();
                        string[] splitOpcion = opcionNumero.Split('-');
                        sqls.Add("INSERT INTO dsolicitud VALUES(" + numero + "," + i.ToString() + ",'" + dtSolicitud.Rows[i][0].ToString() + "'," + splitOpcion[0] + ")");
                    }
                    if (archivos.Count != 0)
                    {
                        for (int i = 0; i < archivos.Count; i++)
                        {
                            string[] nomArch = ((HttpPostedFile)archivos[i]).FileName.Split('\\');
                            sqls.Add("INSERT INTO dsolicitudarchivo VALUES(" + numero + "," + i.ToString() + ",'" + nomArch[nomArch.Length - 1] + "');");
                        }
                    }
                    if (DBFunctions.Transaction(sqls))
                    {
                        for (int i = 0; i < archivos.Count; i++)
                        {
                            string[] nombre = ((HttpPostedFile)archivos[i]).FileName.Split('\\');
                            ((HttpPostedFile)archivos[i]).SaveAs(path + nombre[nombre.Length - 1]);
                        }
                        Response.Redirect(indexPage + "?process=SAC_Asesoria.MostrarSolicitud&num=" + numero + "&inf=1");
                    }
                    else
                        lb.Text = "<br>" + DBFunctions.exceptions;
                }
            }
            else
                Utils.MostrarAlerta(Response, "No ha especificado solicitudes");
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ArrayList sqls = new ArrayList();
            int numero = Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN MAX(msol_numero) IS NULL THEN 0 ELSE MAX(msol_numero) END FROM dbxschema.msolicitud")) + 1;
            string nitaper = DBFunctions.SingleData("SELECT mnit_nit FROM susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'");

            sqls.Add("INSERT INTO mordenservicio VALUES(" + numero + "," + numero + ",'" + nitaper + "','" + nitaper + "','AS','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "','" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "','" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "',null,null)");

            for (int i = 0; i < dtSolicitud.Rows.Count; i++)
                sqls.Add("INSERT INTO dordenservicio VALUES(" + numero + "," + i.ToString() + ",'" + dtSolicitud.Rows[i][0].ToString() + "','AS')");

            if (DBFunctions.Transaction(sqls))
            //Response.Redirect(indexPage+"?process=Asesoria.AdministrarOSC");
            {
                Utils.MostrarAlerta(Response, "Se guardo Correctamente");
            }
            btnEnviar_Click(sender, e);
        }


        protected void btnCancelar_Click(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.CrearSolicitud");
        }

        private void Cargar_dtSolicitud()
        {
            dtSolicitud = new DataTable();
            dtSolicitud.Columns.Add("DETALLE", typeof(string));
            dtSolicitud.Columns.Add("PROGRAMA", typeof(string));
        }

        private void DataBind_dgSolicitud()
        {
            dgSolicitud.DataSource = dtSolicitud;
            dgSolicitud.DataBind();
            Session["dtSolicitud"] = dtSolicitud;
        }

        private bool Verificar_dtSolicitud()
        {
            bool nofilas = false;
            if (dtSolicitud.Rows.Count == 0)
                nofilas = true;
            return nofilas;
        }

        protected void Cargar_ddlprog(DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
            {
                if (DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "AS" || DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "UE")
                    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.FindControl("ddlprog")), "SELECT PPRO.ppro_id,PPRO.ppro_descripcion FROM dbxschema.pproducto PPRO,dbxschema.MCLIENTESAC MCLI,dbxschema.dclienteproductos DCLI WHERE MCLI.mnit_nit=DCLI.mnit_nit AND PPRO.ppro_id=DCLI.ppro_id AND DCLI.mnit_nit='" + ddlnitcli.SelectedValue + "'");
                else if (DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'") == "US")
                        bind.PutDatasIntoDropDownList(((DropDownList)e.Item.FindControl("ddlprog")), "SELECT PPRO.ppro_id,PPRO.ppro_descripcion FROM dbxschema.pproducto PPRO,dbxschema.MCLIENTESAC MCLI,dbxschema.dclienteproductos DCLI WHERE MCLI.mnit_nit=DCLI.mnit_nit AND PPRO.ppro_id=DCLI.ppro_id AND DCLI.mnit_nit=(SELECT mnit_nitcli FROM dbxschema.mcontacto	WHERE susu_codigo=(SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "'))");
            }
        }

        protected void Agregar_Descripcion(object Sender, EventArgs e)
        {
            DataRow fila = dtSolicitud.NewRow();
            String solicitud = txtEspecificos.Text + " - " + txtDescripcion.Text;
            string alerta = "";
            if (txtEspecificos.Text == "")
                alerta += "El campo de: Datos especificos, falta por llenar! ";
            else if (txtDescripcion.Text == "")
                alerta += "El campo de: Descripcion de la solicitud, falta por llenar! ";
            else if ((ddlMenuPrincipal.SelectedValue).ToString() == "0")
                alerta += "Por Favor Seleccione El Modulo Principal! ";
            else if ((ddlmenuCarpeta.SelectedValue).ToString() == "0")
                alerta += "Por Favor Seleccione El Modulo Secundario! ";
            else if ((ddlmenuOpcion.SelectedValue).ToString() == "0")
                alerta += "Por Favor Seleccione La Opción! ";

            if (alerta == "")
            {
                fila[0] = solicitud;
                fila[1] = ddlmenuOpcion.SelectedValue + "-" + ddlmenuOpcion.SelectedItem;
                dtSolicitud.Rows.Add(fila);
                DataBind_dgSolicitud();
                btnEnviar.Enabled = true;
                txtEspecificos.Text = "";
                txtDescripcion.Text = "";
                ddlMenuPrincipal.SelectedIndex = 0;
                ddlmenuCarpeta.SelectedIndex = 0;
                ddlmenuOpcion.SelectedIndex = 0;
            }
            else
                Utils.MostrarAlerta(Response, "" + alerta + "");
        }
        private void llenarDdlPrincipal()
        {
            Utils.FillDll(ddlMenuPrincipal, "SELECT SMEN_PRINCI, SMEN_NOMBRE_PRINCIPAL FROM SMENUPRINCIPAL ORDER BY 1", true);
        }
        protected void ddlMenuPrin_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.FillDll(ddlmenuCarpeta, "SELECT SMEN_CARPETA , SMEN_NOMBRE_CARPETA  \n" +
                                         "FROM SMENUCARPETA \n" +
                                         "WHERE SMEN_PRINCI = '" + ddlMenuPrincipal.SelectedValue + "' \n" +
                                         "ORDER BY 1", true);
            ddlmenuCarpeta_OnSelectedIndexChanged(sender, e);
        }

        protected void ddlmenuCarpeta_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            Utils.FillDll(ddlmenuOpcion, "SELECT  SMEN_OPCION ,SMEN_NOMBRE_OPCION  \n" +
                                         "FROM SMENUOPCION \n" +
                                         "WHERE SMEN_CARPETA = '" + ddlmenuCarpeta.SelectedValue + "' \n" +
                                         "AND SMEN_OPCION <> 999999 ORDER BY 1", true);

        }

        protected void Desaparecer_click(object sender, EventArgs e)
        {
            TbSolicitud.Visible = false;
            ddlmenuOpcion.Items.Add("999999");
            ddlmenuOpcion.SelectedValue = "999999";
            ddlMenuPrincipal.Items.Add("999999");
            ddlMenuPrincipal.SelectedValue = "999999";
            ddlmenuCarpeta.Items.Add("999999");
            ddlmenuCarpeta.SelectedValue = "999999";
        }
        protected void Aparecer_click(object sender, EventArgs e)
        {
            TbSolicitud.Visible = true;
            ddlMenuPrincipal.Items.Remove("999999");
            ddlmenuCarpeta.Items.Remove("999999");
            ddlmenuOpcion.Items.Remove("999999");
        }
        //protected void ddlmenuOpcion_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Utils.FillDll(ddlmenuOpcion, "SELECT  SMEN_OPCION ,SMEN_NOMBRE_OPCION  \n" +
        //                                "FROM SMENUOPCION \n" +
        //                                "WHERE SMEN_CARPETA = '" + ddlmenuCarpeta.SelectedValue + "' \n" +
        //                                "ORDER BY 1", true);         
        //}

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
