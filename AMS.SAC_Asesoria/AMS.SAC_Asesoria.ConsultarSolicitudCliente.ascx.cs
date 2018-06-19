namespace AMS.SAC_Asesoria
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mail;
    using System.Web.Security;
    using System.Web.SessionState;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using AMS.DB;
    using AMS.Forms;
    using AMS.Tools;
    
    public partial class ConsultarSolicitudCliente : System.Web.UI.UserControl
    {

        protected DataGrid dgSols;
        protected DataTable dtSols;
        protected DataSet ds;
        protected Label lb;
        protected string pathToUploads = ConfigurationSettings.AppSettings["Uploads"];
        protected string mainPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        protected string usuario = HttpContext.Current.User.Identity.Name.ToLower();

        protected void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["ce"] != null)
                {
                    Utils.MostrarAlerta(Response, "La solicitud No. " + Request.QueryString["ce"] + " ha sido cerrdada correctamente.");
                }

                Session.Clear();
                Cargar_dtSols();

                string nitUser = DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToLower() + "';");

                DataSet nitEmpresa = new DataSet();
                string consultaDDL = "";
                DBFunctions.Request(nitEmpresa, IncludeSchema.NO, "select mnit_nitcli from mcontacto where mnit_nitcon= (SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "' fetch first row only)");
               
                if (nitEmpresa.Tables[0].Rows.Count > 0)
                {
                    consultaDDL += " AND (";
                    for (int g = 0; g < nitEmpresa.Tables[0].Rows.Count; g++)
                    {
                        consultaDDL += "  MCLI.mnit_nit =" + nitEmpresa.Tables[0].Rows[g][0].ToString() + " OR";
                    }
                    consultaDDL += "@";
                    consultaDDL = consultaDDL.Replace("OR@", "");
                    consultaDDL += ") ";
                }
                
                filtroEmpresa.Visible = true;
                DatasToControls bind = new DatasToControls();
                ViewState["roluser"] = nitUser;

                if (nitUser == "AS")
                {
                    bind.PutDatasIntoDropDownList(ddlEmpresa,
                                @"select MNIT.mnit_nit, MCLI.mcli_marcas FROM dbxschema.mnit MNIT, dbxschema.MCLIENTESAC MCLI 
                                  WHERE MNIT.mnit_nit=MCLI.mnit_nit AND MCLI.TVIG_CODIGO='V' ORder by 2;");
                }
                else
                {
                    bind.PutDatasIntoDropDownList(ddlEmpresa,
                        @"select MNIT.mnit_nit, MCLI.mcli_marcas FROM dbxschema.mnit MNIT, dbxschema.MCLIENTESAC MCLI 
                              WHERE MNIT.mnit_nit=MCLI.mnit_nit AND MCLI.TVIG_CODIGO='V' " + consultaDDL + " Order by 2");
                }
                ddlEmpresa.Items.Insert(0, "Ver Todas...");
                
                if (ViewState["solicitudes"] == null)
                {
                    Llenar_dtSols(0);
                    dgSols_DataBind();
                }
                else
                {
                    ViewState.Remove("solicitudes");
                }
            }
            else
            {
                if (Session["dtSols"] != null)
                    dtSols = (DataTable)Session["dtSols"];
            }
        }

        //Valida la continuidad lineal entre los cambios de estados de las ordenes.
        protected bool ValidarSecuenciaEstadoOrden(string idSolicitud, string nuevoEstadoSol)
        {
            string estadoSolicitud = DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero = " + idSolicitud);
            
            // AS <-> ED <-> CC <-> IM <-> CE
            switch (nuevoEstadoSol)
            {
                case "AS":  //Asignada
                    if (estadoSolicitud == "ED" )
                        return true;
                    else
                        return false;

                case "ED":  //En Desarrollo
                    if (estadoSolicitud == "AS" || estadoSolicitud == "CC")
                        return true;
                    else
                        return false;

                case "CC":  //Control Calidad
                    if (estadoSolicitud == "ED" || estadoSolicitud == "IM")
                        return true;
                    else
                        return false;

                case "IM":  //Implementada
                    if (estadoSolicitud == "CC" || estadoSolicitud == "CE")
                        return true;
                    else
                        return false;

                case "CE":  //Cerrada
                    if (estadoSolicitud == "IM")
                        return true;
                    else
                        return false;

                default:
                    return false;
            }
        }

        protected void Check_MostrarCerradas(object sender, EventArgs e)
        {
            Llenar_dtSols(0);
            dgSols_DataBind();
        }

        //Cambia solicitud a estado: ASIGNADO.
        protected void AsignarSol(object Sender, CommandEventArgs e)
        {
            string[] NumeroSolicitud = e.CommandArgument.ToString().Split('-');
            bool permitir = ValidarSecuenciaEstadoOrden(NumeroSolicitud[0].Trim(), "AS");

            if (permitir)
            {
                ArrayList sqls = new ArrayList();
                if (e.CommandArgument.ToString().Contains("-") == false)
                {
                    sqls.Add("UPDATE MORDENSERVICIO SET TEST_CODIGO = 'AS' WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + "");
                    if (DBFunctions.Transaction(sqls))
                        Response.Redirect(mainPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&usuOrde=1");
                    else
                        lb.Text = "Error " + DBFunctions.exceptions;
                }
                else
                {
                    sqls.Add("UPDATE mordenserviciotareas SET TEST_CODIGO = 'AS' WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + " and mord_tarea = " + NumeroSolicitud[1].Trim() + "");
                    if (DBFunctions.Transaction(sqls))
                        Response.Redirect(mainPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&usuOrde=1");
                    else
                        lb.Text = "Error " + DBFunctions.exceptions;
                }
            }
            else
            {
                Utils.MostrarAlerta(Response, "No." + NumeroSolicitud[0].Trim() + " - Cambio no válido.");
            }
        }

        //Cambia solicitud a estado: EN DESARROLLO.
        protected void DesarrollarSol(object Sender, CommandEventArgs e)
        {
            string[] NumeroSolicitud = e.CommandArgument.ToString().Split('-');
            bool permitir = ValidarSecuenciaEstadoOrden(NumeroSolicitud[0].Trim(), "ED");

            if (permitir)
            {
                ArrayList sqls = new ArrayList();
                if (e.CommandArgument.ToString().Contains("-") == false)
                {
                    sqls.Add("UPDATE MORDENSERVICIO SET TEST_CODIGO = 'ED' " +
                             ",MORD_FECHADESARROLLO = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                             ",MORD_HORADESARROLLO='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' " +
                             "WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + "");
                    if (DBFunctions.Transaction(sqls))
                        Response.Redirect(mainPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&usuOrde=1");
                    else
                        lb.Text = "Error " + DBFunctions.exceptions;
                }
                else
                {
                    sqls.Add("UPDATE mordenserviciotareas SET TEST_CODIGO = 'ED' " +
                             ",MORD_FECHADESARROLLO = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                             ",MORD_HORADESARROLLO='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' " +
                             "WHERE MORD_NUMERO = " + NumeroSolicitud[0].Trim() + " and mord_tarea = " + NumeroSolicitud[1].Trim() + "");
                    if (DBFunctions.Transaction(sqls))
                        Response.Redirect(mainPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&usuOrde=1");
                    else
                        lb.Text = "Error " + DBFunctions.exceptions;
                }
            }
            else
            {
                Utils.MostrarAlerta(Response, "No." + NumeroSolicitud[0].Trim() + " - Cambio no válido.");
            }
        }

        //Cambia solicitud a estado: CONTROL DE CALIDAD.
        protected void ControlCalidadSol(object Sender, CommandEventArgs e)
        {
            string[] NumeroSolicitud = e.CommandArgument.ToString().Split('-');
            CambiarEstadoSolicitud(NumeroSolicitud[0].Trim(), "CC");
        }

        //Cambia solicitud a estado: IMPLEMENTADA.
        protected void ImplementarSol(object Sender, CommandEventArgs e)
        {
            string[] NumeroSolicitud = e.CommandArgument.ToString().Split('-');
            CambiarEstadoSolicitud(NumeroSolicitud[0].Trim(), "IM");
        }

        //Cambia solicitud a estado: CERRADA.
        protected void CerrarSol(object Sender, EventArgs e)
        {
            string numeroSolicitud = hdLabelVal.Value;
            string observacionEnc = txtObservacion.Text;
            bool permitir = ValidarSecuenciaEstadoOrden(numeroSolicitud, "CE");
            if (permitir)
            {
                if (rdExcelente.Checked)
                {
                    RegistrarCierre(numeroSolicitud, "E", observacionEnc);
                }
                else if (rdBuena.Checked)
                {
                    RegistrarCierre(numeroSolicitud, "B", observacionEnc);
                }
                else if (rdRegular.Checked)
                {
                    RegistrarCierre(numeroSolicitud, "R", observacionEnc);
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Por favor diligenciar la encuesta antes de cerrar la Solicitud.");
                    //RegistrarCierre(numeroSolicitud, "", observacionEnc);
                }

                //Limpiar Formulario
                rdExcelente.Checked = false;
                rdBuena.Checked = false;
                rdRegular.Checked = false;
                txtObservacion.Text = "";
            }
            else
            {
                Utils.MostrarAlerta(Response, "No." + numeroSolicitud + " - Cambio no válido.");
            }
        }

        protected void RegistrarCierre(string numeroSolicitud, string valorEncuesta, string observacionEnc)
        {
            if (DBFunctions.RecordExist("select mord_numero from dencuestacalidad where mord_numero =" + numeroSolicitud))
            {
                DBFunctions.NonQuery(
                    "update dencuestacalidad set DENC_RESPUESTA = '" + valorEncuesta + "', DENC_OBSERVACION = '" + observacionEnc + "', " +
                    "DENC_FECHA = DEFAULT where mord_numero = " + numeroSolicitud);
            }
            else
            {
                DBFunctions.NonQuery(
                    "INSERT INTO DBXSCHEMA.DENCUESTACALIDAD(  MORD_NUMERO,  DENC_RESPUESTA,  DENC_OBSERVACION,DENC_FECHA) " +
                    "VALUES(" + numeroSolicitud + ",'" + valorEncuesta + "','" + observacionEnc + "', DEFAULT)");
            }

            DBFunctions.NonQuery("UPDATE MORDENSERVICIO SET TEST_CODIGO = 'CE' " +
                             ",MORD_FECHACIERRE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                             ",MORD_HORACIERRE='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' " +
                             "WHERE MORD_NUMERO = " + numeroSolicitud);
            
            Response.Redirect(mainPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&ce=" + numeroSolicitud);
        }
        
        protected void CambiarEstadoSolicitud(string idSoloicitud, string nuevoEstado)
        {
            bool permitir = ValidarSecuenciaEstadoOrden(idSoloicitud, nuevoEstado);
            if (permitir)
            {
                Response.Redirect(mainPage + "?process=SAC_Asesoria.ResponderOSC&nosc=" + idSoloicitud + "&nuevoEstado=" + nuevoEstado);
            }
            else
            {
                Utils.MostrarAlerta(Response, "No." + idSoloicitud + " - Cambio no válido.");
            }
        }

        protected void dgSols_ItemDataBound(object Sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string[] NumeroSolicitud = dtSols.Rows[e.Item.DataSetIndex][0].ToString().Split('-');
                DataSet ds = new DataSet();
                if (dtSols.Rows[e.Item.DataSetIndex][0].ToString().Contains("-") == false)
                {
                    DBFunctions.Request(ds, IncludeSchema.NO,
                    @"SELECT DSOL.dsol_detalle AS DETALLE, SMP.SMEN_NOMBRE_PRINCIPAL CONCAT '/' CONCAT SMC.SMEN_NOMBRE_CARPETA CONCAT '/' CONCAT SMO.SMEN_NOMBRE_OPCION AS PROGRAMA, coalesce(d.dord_estimacionhora, 0) as HORAS   
                    FROM dbxschema.dsolicitud DSOL, SMENUOPCION SMO, SMENUCARPETA SMC, SMENUPRINCIPAL SMP, DORDENSERVICIO d 
                    WHERE DSOL.ppro_id = SMO.SMEN_OPCION AND SMO.SMEN_CARPETA = SMC.SMEN_CARPETA AND SMC.SMEN_PRINCI = SMP.SMEN_PRINCI AND DSOL.msol_numero =" + NumeroSolicitud[0].Trim() +
                    " and DSOL.msol_numero = d.mord_numero and dsol.dsol_numero = d.dord_id order by d.dord_id;" +
                    "SELECT dsol_nombre AS ARCHIVO FROM dbxschema.dsolicitudarchivo WHERE msol_numero=" + NumeroSolicitud[0].Trim() + ";");
                }
                else
                {
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT MORD_DESCRIPCION AS Detalle, \n" +
                                                            "      'DESARROLO' AS Programa \n" +
                                                            " FROM dbxschema.mordenserviciotareas \n" +
                                                            "WHERE mord_numero = " + NumeroSolicitud[0] + "\n" +
                                                            "  and mord_tarea =" + NumeroSolicitud[1].Trim() + ";" +
                                                            "SELECT dsol_nombre AS ARCHIVO FROM dbxschema.dsolicitudarchivo WHERE msol_numero=" + NumeroSolicitud[0].Trim() + ";");

                }

                if (Request.QueryString["usuOrde"] == "1"  || (ViewState["roluser"].ToString() == "AS" && dtSols.Rows[e.Item.DataSetIndex][7].ToString() != "AB"))
                {
                    ((ImageButton)e.Item.Cells[0].FindControl("imgAsignarSol")).Visible = true;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgDesarrolloSol")).Visible = true;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgControlCalidadSol")).Visible = true;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgImplementadaSol")).Visible = true;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgCerrarSol")).Visible = true;   
                }

                if (dtSols.Rows[e.Item.DataSetIndex][7].ToString() == "AS")
                {
                    ((Panel)e.Item.Cells[0].FindControl("divSolicitud")).Attributes.Add("class", "tarjetaAsig");
                    ((ImageButton)e.Item.Cells[0].FindControl("imgAsignarSol")).Attributes.Add("class", "imgSolSelec");
                }
                
                if (dtSols.Rows[e.Item.DataSetIndex][7].ToString() == "ED")
                {
                    ((Panel)e.Item.Cells[0].FindControl("divSolicitud")).Attributes.Add("class", "tarjetaDesa");
                    ((ImageButton)e.Item.Cells[0].FindControl("imgDesarrolloSol")).Attributes.Add("class", "imgSolSelec");
                }

                if (dtSols.Rows[e.Item.DataSetIndex][7].ToString() == "CC")
                {
                    ((Panel)e.Item.Cells[0].FindControl("divSolicitud")).Attributes.Add("class", "tarjetaCalidad");
                    ((ImageButton)e.Item.Cells[0].FindControl("imgControlCalidadSol")).Attributes.Add("class", "imgSolSelec");
                }

                if (dtSols.Rows[e.Item.DataSetIndex][7].ToString() == "IM")
                {
                    ((Panel)e.Item.Cells[0].FindControl("divSolicitud")).Attributes.Add("class", "tarjetaImplementada");
                    ((ImageButton)e.Item.Cells[0].FindControl("ImgImplementadaSol")).Attributes.Add("class", "imgSolSelec");
                    if (Request.QueryString["usuOrde"] != "1")
                        ((ImageButton)e.Item.Cells[0].FindControl("imgCerrarSol")).Visible = true;
                }

                if (dtSols.Rows[e.Item.DataSetIndex][7].ToString() == "CE")
                {
                    ((Panel)e.Item.Cells[0].FindControl("divSolicitud")).Attributes.Add("class", "tarjetaClose");
                    ((ImageButton)e.Item.Cells[0].FindControl("imgAsignarSol")).Visible = false;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgDesarrolloSol")).Visible = false;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgControlCalidadSol")).Visible = false;
                    ((ImageButton)e.Item.Cells[0].FindControl("ImgImplementadaSol")).Visible = false;
                    ((ImageButton)e.Item.Cells[0].FindControl("imgCerrarSol")).Attributes.Add("class", "imgSolSelec");
                }

                if (dtSols.Rows[e.Item.DataSetIndex][7].ToString() == "AB")
                {
                    ((Panel)e.Item.Cells[0].FindControl("divSolicitud")).Attributes.Add("class", "tarjetaOpen");
                    ((ImageButton)e.Item.Cells[0].FindControl("imgDesarrolloSol")).Attributes.Add("class", "imgSolSelec");
                }

                ((DataGrid)e.Item.Cells[0].FindControl("dgSolicitud")).DataSource = ds.Tables[0];
                ((DataGrid)e.Item.Cells[0].FindControl("dgSolicitud")).DataBind();

                if (Request.QueryString["usuOrde"] == "1")
                    ((DataGrid)e.Item.Cells[0].FindControl("dgSolicitud")).Columns[2].Visible = true;
                else
                    ((DataGrid)e.Item.Cells[0].FindControl("dgSolicitud")).Columns[2].Visible = false;

                ((DataGrid)e.Item.Cells[0].FindControl("dgArchivos")).DataSource = ds.Tables[1];
                ((DataGrid)e.Item.Cells[0].FindControl("dgArchivos")).DataBind();
                string orden = DBFunctions.SingleData("SELECT mord_numero FROM dbxschema.mordenservicio WHERE msol_numero=" + NumeroSolicitud[0].Trim() + "");
                if (orden.Length != 0)
                {
                    ((LinkButton)e.Item.Cells[0].FindControl("lbnorden")).CommandName = "verorden_" + orden;
                    ((LinkButton)e.Item.Cells[0].FindControl("lbnorden")).Text = "Ver Orden de Servicio Asociada >>";
                    ((LinkButton)e.Item.Cells[0].FindControl("lbnorden")).Visible = true;
                    ((LinkButton)e.Item.Cells[0].FindControl("lbnorden")).ForeColor = Color.Black;


                }
                for (int i = 0; i < ((DataGrid)e.Item.Cells[0].FindControl("dgArchivos")).Items.Count; i++)
                {
                    ((HyperLink)((DataGrid)e.Item.Cells[0].FindControl("dgArchivos")).Items[i].Cells[1].FindControl("hpldes")).NavigateUrl = pathToUploads + ((DataGrid)e.Item.Cells[0].FindControl("dgArchivos")).Items[i].Cells[0].Text;
                    ((HyperLink)((DataGrid)e.Item.Cells[0].FindControl("dgArchivos")).Items[i].Cells[1].FindControl("hpldes")).Target = "_blank";
                }
            }
        }

        protected void dgSols_ItemCommand(object Sender, DataGridCommandEventArgs e)
        {
            if ((e.CommandName.Split('_'))[0] == "verorden")
            {
                string[] partes = e.CommandName.Split('_');
                Response.Redirect(mainPage + "?process=SAC_Asesoria.MostrarOSC&nosc=" + partes[1] + "");
            }
        }

        private void Cargar_dtSols()
        {
            dtSols = new DataTable();
            dtSols.Columns.Add("NUMERO", typeof(int));
            dtSols.Columns.Add("NITCLI", typeof(string));
            dtSols.Columns.Add("CLIENTE", typeof(string));
            dtSols.Columns.Add("NITCON", typeof(string));
            dtSols.Columns.Add("CONTACTO", typeof(string));
            dtSols.Columns.Add("FECHA", typeof(string));
            dtSols.Columns.Add("HORA", typeof(string));
            dtSols.Columns.Add("VIA", typeof(string));
        }

        private void dgSols_DataBind()
        {
            dgSols.DataSource = dtSols;
            dgSols.DataBind();
            Session["dtSols"] = dtSols;
        }

        private void Llenar_dtSols(int filtro)
        {
            DataSet ds = new DataSet();
            string nitUsu = DBFunctions.SingleData("SELECT MNIT_NIT FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
            string tipoUsu = DBFunctions.SingleData("SELECT ttipe_codigo FROM dbxschema.susuario WHERE susu_login='" + HttpContext.Current.User.Identity.Name.ToString().ToLower() + "';");
            string conEcas1 = "";
            string conEcas2 = "";
            string consulta = "";
            string ordenar = "";

            if (!tipoUsu.Equals("AS") && !tipoUsu.Equals("UE"))
            {
                if (!nitUsu.Equals(""))
                {
                    DataSet nitEmpresa = new DataSet();
                    if (ddlEmpresa.Items.Count == 0 || tipoUsu.Equals("US"))
                    {
                        DBFunctions.Request(nitEmpresa, IncludeSchema.NO, "select mnit_nitcli from mcontacto where mnit_nitcon='" + nitUsu + "'");
                        consulta += " AND (";
                        for (int g = 0; g < nitEmpresa.Tables[0].Rows.Count; g++)
                        {
                            consulta += " MSOL.MNIT_NITCLI =" + nitEmpresa.Tables[0].Rows[g][0].ToString() + " OR";
                        }
                        consulta += "@";
                        consulta = consulta.Replace("OR@", "");
                        consulta += ") ";
                        ordenar = "DESC";
                    }
                    else
                    {
                        consulta = "AND MSOL.MNIT_NITCLI =" + ddlEmpresa.SelectedValue;
                    }
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Debe relacionar un Nit o Cedula a su Usuario mediante la opción de Modificación de Usuario.");
                    return;
                }
            }
            else 
            {
                conEcas1 = "select * from(";
                conEcas2 = ") AS SOLECAS where VIA != 'CE';";
            }

            string consulta2 = "";
            if (filtro == 1 && ddlEmpresa.SelectedIndex != 0)
            {
                consulta2 = " AND MSOL.mnit_nitcli ='" + ddlEmpresa.SelectedValue + "' ";
            }

            string mostrarCerradas = "";
            if(chkCerradas.Checked == false)
            {
                mostrarCerradas = "AND COALESCE(MO.test_codigo, 'AB') <> 'CE'";
            }
            else
            {
                conEcas1 = "select * from(";
                conEcas2 = ") AS SOLECAS where VIA = 'CE';";
            }

            if (Request.QueryString["usuOrde"] == "1")
            {
                String nitUsuario = DBFunctions.SingleData("select mnit_nit from dbxschema.susuario where susu_login='" + usuario + "';");
                DBFunctions.Request(ds, IncludeSchema.NO,
                        @"select * from (
                        SELECT MSOL.msol_numero AS NUMERO, 
                        MSOL.mnit_nitcli AS NITCLI, 
                        (MCL.mcli_marcas)  AS CLIENTE, 
                        MSOL.mnit_nitcon AS NITCON, 
                        (MN2.mnit_nombres || MN2.mnit_nombre2 || MN2.mnit_apellidos || MN2.mnit_apellido2) AS CONTACTO, 
                        MSOL.msol_fecha AS FECHA, MSOL.msol_hora AS HORA, 
                        COALESCE(MO.test_codigo, 'AB') AS VIA,
                        MO.mord_fechacierre , (DAYS(CURRENT DATE)- DAYS(MSOL.msol_fecha)) CONCAT ' Días'   as diasespera, (DAYS(CURRENT DATE)- DAYS(MSOL.msol_fecha)) as dias
                        FROM 
                          msolicitud MSOL LEFT JOIN dbxschema.mordenservicio MO ON MO.msol_numero = MSOL.msol_numero, mnit MN, mnit MN2, MCLIENTESAC MCL
                        WHERE
                          MCL.mnit_nit = MN.mnit_nit AND MN.mnit_nit = MSOL.mnit_nitcli AND MSOL.mnit_nitcon = MN2.mnit_nit
                          AND MO.pas_mnit_nit = '" + nitUsuario + @"' " + consulta2 + @"ORDER BY MSOL.msol_numero DESC
                        ) AS CONECAS
                        where (VIA = 'CE' AND MORD_FECHACIERRE BETWEEN CURRENT DATE- 15 DAY AND CURRENT DATE ) 
                        OR VIA <> 'CE' ORDER BY dias DESC;");               
            }
            else
            {
                if (consulta2 != "")
                    consulta = "";
                DBFunctions.Request(ds, IncludeSchema.NO,
                              conEcas1 + @"SELECT MSOL.msol_numero AS NUMERO, 
                                MSOL.mnit_nitcli AS NITCLI, 
                                (MCL.mcli_marcas) AS CLIENTE, 
                                MSOL.mnit_nitcon AS NITCON, 
                                (MN2.mnit_nombres || MN2.mnit_nombre2 || MN2.mnit_apellidos || MN2.mnit_apellido2) AS CONTACTO, 
                                MSOL.msol_fecha AS FECHA, MSOL.msol_hora AS HORA, 
                                COALESCE(MO.test_codigo, 'AB') AS VIA, '' as diasespera
                                FROM 
                                msolicitud MSOL LEFT JOIN dbxschema.mordenservicio MO ON MO.msol_numero = MSOL.msol_numero, mnit MN, mnit MN2, MCLIENTESAC MCL
                                WHERE
                                MCL.mnit_nit = MN.mnit_nit AND MN.mnit_nit = MSOL.mnit_nitcli AND MSOL.mnit_nitcon = MN2.mnit_nit " +
                                consulta + consulta2 + " " + mostrarCerradas + " ORDER BY VIA " + ordenar + ", MSOL.msol_numero DESC" + conEcas2
                                );
            }

            if (ds.Tables[0].Rows.Count != 0)
            {
                dtSols = ds.Tables[0];
            }
            else
            {
                Utils.MostrarAlerta(Response, "Actualmente No Tiene Ninguna Solicitud !!!");
                ViewState["solicitudes"] = 0;
                return;
            }
        }

        protected void DdlChanged_Empresa(object Sender, EventArgs e)
        {
            Llenar_dtSols(1);
            dgSols_DataBind();
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
