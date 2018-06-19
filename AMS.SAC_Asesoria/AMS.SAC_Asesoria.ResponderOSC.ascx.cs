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


    public partial class ResponderOSC : System.Web.UI.UserControl
    {
        protected DataTable dtRespuesta;
        protected ArrayList archivos = new ArrayList();
        protected string path = ConfigurationSettings.AppSettings["PathToDownloads"];
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexpage"];
        protected DataSet ds = new DataSet();

        protected void Page_Load(object Sender, EventArgs e)
        {
            Cargar_Datos();
            if (!IsPostBack)
            {
                Session.Clear();
                string estadoOrden = DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "");
                if (estadoOrden == "ED" || estadoOrden == "CC" || estadoOrden == "IM")
                {
                    this.Llenar_dtRespuesta_Desarrollo();
                    lbArchivos.Text = Llenar_lbArchivos();
                }
                else if (DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "") == "AS")
                    Llenar_dtRespuesta();
            }
            else
            {
                if (Session["dtRespuesta"] != null)
                    dtRespuesta = (DataTable)Session["dtRespuesta"];
                if (Session["archivos"] != null)
                    archivos = (ArrayList)Session["archivos"];
            }
        }

        protected void dgRespuesta_EditCommand(object Sender, DataGridCommandEventArgs e)
        {
            dgRespuesta.EditItemIndex = e.Item.DataSetIndex;            
            dgRespuesta_DataBind();
            string origenError = ((DataBoundLiteralControl)e.Item.Cells[4].Controls[0]).Text.Trim();
            string tipoRespuesta = ((DataBoundLiteralControl)e.Item.Cells[5].Controls[0]).Text.Trim();
            DropDownList ddlOrigeErr = ((DropDownList)dgRespuesta.Items[dgRespuesta.EditItemIndex].Cells[4].FindControl("ddlOrigenError"));
            DropDownList ddlTipoResp = ((DropDownList)dgRespuesta.Items[dgRespuesta.EditItemIndex].Cells[5].FindControl("ddlTipoRespuesta"));

            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlOrigeErr, "select terr_codigo, terr_nombre from terrorsolucion;");
            bind.PutDatasIntoDropDownList(ddlTipoResp, "select ttipo_resp, ttipo_descripcion from ttiporespuesta order by ttipo_resp desc;");

            if (origenError != "")
            {
                origenError = origenError.Substring(0, 2);
                ddlOrigeErr.ClearSelection();
                ddlOrigeErr.Items.FindByValue(origenError).Selected = true;
            }

            if (tipoRespuesta != "")
            {
                tipoRespuesta = tipoRespuesta.Substring(0, 2);
                ddlTipoResp.ClearSelection();
                ddlTipoResp.Items.FindByValue(tipoRespuesta).Selected = true;
            }
        }
        
        protected void dgRespuesta_CancelCommand(object Sender, DataGridCommandEventArgs e) 
        {
            dgRespuesta.EditItemIndex = -1;
            dgRespuesta_DataBind();
        }

        protected void dgRespuesta_UpdateCommand(object Sender, DataGridCommandEventArgs e)
        {
            if (((TextBox)e.Item.Cells[2].FindControl("tbRes")).Text == "")
                Utils.MostrarAlerta(Response, "No ha especificado una solución");
            else
            {
                DropDownList ddlOriErr = ((DropDownList)e.Item.Cells[4].FindControl("ddlOrigenError"));
                DropDownList ddlTipoRes = ((DropDownList)e.Item.Cells[5].FindControl("ddlTipoRespuesta"));

                dtRespuesta.Rows[e.Item.DataSetIndex][2] = ((TextBox)e.Item.Cells[2].FindControl("tbRes")).Text;
                dtRespuesta.Rows[e.Item.DataSetIndex][3] = ((TextBox)e.Item.Cells[2].FindControl("tbResCli")).Text;
                dtRespuesta.Rows[e.Item.DataSetIndex][4] = ddlOriErr.SelectedValue + " - " + ddlOriErr.SelectedItem;
                dtRespuesta.Rows[e.Item.DataSetIndex][5] = ddlTipoRes.SelectedValue + " - " + ddlTipoRes.SelectedItem;
                
                dgRespuesta.EditItemIndex = -1;
                dgRespuesta_DataBind();
            }
        }

        protected void btnAgregar_Click(object Sender, EventArgs e)
        {
            if (this.uplFile.PostedFile.FileName.Length == 0)
                Utils.MostrarAlerta(Response, "No ha especificado un nombre de archivo");
            else
            {
                HttpPostedFile file = uplFile.PostedFile;
                archivos.Add(file);
                Session["archivos"] = archivos;
                lbArchivos.Text += file.FileName + "<br>";
            }
        }

        protected void btnGuardar_Click(object Sender, EventArgs e)
        {
            string carpetaSol;
            ArrayList sqls = new ArrayList();

            if (!Verificar_dtRespuesta())
            {
                //Borro las respuestas antiguas
                DBFunctions.NonQuery("DELETE FROM dordensolucion WHERE mord_numero=" + Request.QueryString["nosc"] + "");

                //Guardamos las soluciones dadas a la OSC 
                for (int i = 0; i < dtRespuesta.Rows.Count; i++)
                {
                    carpetaSol = DBFunctions.SingleData("SELECT PPRO_ID FROM DSOLICITUD WHERE MSOL_NUMERO =" + Request.QueryString["nosc"] + " AND DSOL_NUMERO =" + i);
                    sqls.Add("INSERT INTO dordensolucion VALUES(" + Request.QueryString["nosc"] + "," + i.ToString() + ",'" + dtRespuesta.Rows[i]["RESPUESTA"].ToString() + "','" + dtRespuesta.Rows[i]["ORIGENERROR"].ToString().Substring(0, 2) + "','" + dtRespuesta.Rows[i]["TIPORESPUESTA"].ToString().Substring(0, 2) + "', '" + dtRespuesta.Rows[i]["RESPUESTACLIENTE"].ToString() + "')");
                    sqls.Add("INSERT INTO DOBJETOECAS VALUES(default," + carpetaSol + ",'" + dtRespuesta.Rows[i]["RESPUESTA"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "'," + Request.QueryString["nosc"] + ")");
                }

                //Si hay archivos agregados, los relacionamos a la solución de la OSC
                if (archivos.Count != 0)
                {
                    string ultimo = DBFunctions.SingleData("select DORD_ID from dbxschema.dordensolucionarchivo where mord_numero = " + Request.QueryString["nosc"] + " order by DORD_ID desc;");
                    int ult = -1;
                    if (!ultimo.Equals(""))
                    {
                        ult = int.Parse(ultimo);
                    }

                    int j = 0;
                    for (int i = (1 + ult); i <= (archivos.Count + ult); i++)
                    {
                        string[] nomArch = ((HttpPostedFile)archivos[j]).FileName.Split('\\');
                        sqls.Add("INSERT INTO dordensolucionarchivo VALUES(" + Request.QueryString["nosc"] + "," + i.ToString() + ",'" + nomArch[nomArch.Length - 1] + "')");
                        j++;
                    }
                }
                
                //Actualización de fechas de Estados de Solicitud.
                string nuevoEstado = Request.QueryString["nuevoEstado"].ToString();
                string sqlUpdateFecha = GetUpdateEstadoSolSQL(nuevoEstado);
                if(sqlUpdateFecha != "")
                    sqls.Add(sqlUpdateFecha);

                //Envio de correo al cliente para confirmacion de que la solicitud en estado Implementada.
                string correoEnviado = "";
                if (nuevoEstado == "IM" && dtRespuesta.Rows[0]["TIPORESPUESTA"].ToString().Substring(0, 2) != "ST")
                {
                    correoEnviado = SetSendMail();
                }
                
                if (DBFunctions.Transaction(sqls))
                {
                    for (int i = 0; i < archivos.Count; i++)
                    {
                        string[] nombre = ((HttpPostedFile)archivos[i]).FileName.Split('\\');
                        ((HttpPostedFile)archivos[i]).SaveAs(path + nombre[nombre.Length - 1]);
                    }
                    Response.Redirect(indexPage + "?process=SAC_Asesoria.MostrarOSC&nosc=" + Request.QueryString["nosc"] + "&inf=1&codCorreoEnv=" + correoEnviado);
                }
                else
                    lb.Text = "Error " + DBFunctions.exceptions;
            }
            else
                Utils.MostrarAlerta(Response, "No ha dado respuesta a ninguna solicitud.\\n Imposible guardar la respuesta");
        }

        protected string GetUpdateEstadoSolSQL(string nuevoEstado)
        {
            string sql = "UPDATE mordenservicio SET test_codigo='" + nuevoEstado + "' ";

            switch (nuevoEstado)
            {
                case "CC":  //Control Calidad
                    sql += ",MORD_FECHACALIDAD = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                           ",MORD_HORACALIDAD='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' ";
                    break;
                case "IM":  //Implementada
                    sql += ",MORD_FECHAIMPLEMENT = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                           ",MORD_HORAIMPLEMENT='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' ";
                    break;
                case "CE":  //Cerrada
                    sql += ",MORD_FECHACIERRE = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                           ",MORD_HORACIERRE='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' ";
                    break;
                default:
                    sql = "";
                    break;
            }

            sql += " WHERE mord_numero=" + Request.QueryString["nosc"] + ";" ;

            if (nuevoEstado == "CE" && !chbCierre.Checked)
                sql = "";

            return sql;
        }

        protected void btnRegresar_Click(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&usuOrde=1");
        }
        protected string SetSendMail()
        {
            string correos = "";
            string mensaje = "";

            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT mn.mnit_email, mn.mnit_nombres CONCAT ' ' CONCAT mn.mnit_nombre2 CONCAT ' ' CONCAT mn.mnit_apellidos CONCAT ' ' CONCAT mn.mnit_apellido2 
                    FROM MCONTACTO mc,   mnit mn WHERE mc.mnit_nitcli = '" + lbnitcli.Text + "' AND   mc.mnit_nitcon = mn.mnit_nit");

            int respuesta = 0;
            for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
            {
                string detallesSolicitud = "";
                mensaje =
                    @"<br>
	                <div style='background-color:#EEEFD9;width: 75%;border-radius: 10px;margin: auto;padding: 20px;'>
	                <b><font size='5'>Notificación de Solicitud Terminada: No. " + lbnumsol.Text + @"</font></b><img style='position: absolute;width: 100px; right: 16%; top: 5%;' src='http://ams.ecas.co/sac/img/logoEcas.png' />
	                <br><br>
	                <b>Sr(a). " + ds.Tables[0].Rows[r][1].ToString() + @"</b>, <br>
	                Nos permitimos informarle que su solicitud, citada en la referencia, ha sido revisada y finalizada correctamente. Le solicitamos verificar que las soluciones dadas están de acuerdo con sus requerimientos. Si tiene alguna duda o inconveniente le agradecemos se sirva informarnos.
	                <br><br>";

                //Construccion de tabla HTML para solicitudes y respuestas.
                detallesSolicitud += "<table border='1'><tr style='background-color:lavender;'><th>Detalles de Solicitud</th><th>Respuesta</th></tr>";
                for (int t = 0; t < dtRespuesta.Rows.Count; t++)
                {
                    string tipoRespuesta = dtRespuesta.Rows[t]["TIPORESPUESTA"].ToString();

                    detallesSolicitud += "<TR style ='background-color:azure;'><TD style = 'width:60%'>";
                    detallesSolicitud += "<B>" + (t + 1) + ".</B>  " + dtRespuesta.Rows[t]["SOLICITUD"].ToString() + "<br></td>";
                    detallesSolicitud += "<td style='width: 40%'>" + dtRespuesta.Rows[t]["RESPUESTACLIENTE"].ToString() + "</td></tr>";
                }
                detallesSolicitud += "</table>";

                mensaje += "<u>Detalles de Solicitud:</u>";
                mensaje += detallesSolicitud + "<br><br>";
                mensaje += @"Agradecemos su atención.<br>Por favor no olvidar cerrar su Orden de Servicio y diligenciar la encuesta.</b>
	                <img style='width: 6%;' src='http://ams.ecas.co/sac/img/SAC.logo.sac.jpg' />
                    <br><br>Cordial saludo,<br>
                    <i>eCAS-SAC. Móvil 316 606 85 31</i>
	                </div>
                    <br><br>";

                correos = ds.Tables[0].Rows[r][0].ToString();

                try
                {
                    respuesta = Tools.Utils.EnviarMail(correos, "Respuesta a Solicitud Ecas No. " + lbnumsol.Text, mensaje, TipoCorreo.HTML, "");
                    if(r == 0) //envia correo copia a ecas solo en la primera vuelta.
                    {
                        Tools.Utils.EnviarMail("sacsistemasecas@gmail.com", "Respuesta a Solicitud Ecas No. " + lbnumsol.Text, mensaje, TipoCorreo.HTML, "");
                    }
                }
                catch (Exception err)
                { }
            }
            
            if (respuesta == 1)
                return lbnitcli.Text;
            else
                return "x";
        }

        protected void btnCancelar_Click(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.AdministrarOSC");
        }

        private void Cargar_dtRespuesta()
        {
            dtRespuesta = new DataTable();
            dtRespuesta.Columns.Add("NUMERO", typeof(int));
            dtRespuesta.Columns.Add("SOLICITUD", typeof(string));
            dtRespuesta.Columns.Add("RESPUESTA", typeof(string));
            dtRespuesta.Columns.Add("RESPUESTACLIENTE", typeof(string));
            dtRespuesta.Columns.Add("ORIGENERROR", typeof(string));
            dtRespuesta.Columns.Add("TIPORESPUESTA", typeof(string));
        }

        private void Llenar_dtRespuesta()
        {
            DataSet ds = new DataSet();

            DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT DOSO.mord_numero as IDORDEN, DOSE.dord_detalle as DETALLE, DOSO.dord_respuesta as RESPUESTATEC, TERR.TERR_CODIGO CONCAT ' - ' CONCAT TERR.TERR_NOMBRE AS ORIGEN_ERR,  DOSO.TTIPO_RESP CONCAT ' - ' CONCAT ttip.TTIPO_DESCRIPCION AS TIPO_RESPUESTA, DOSO.dord_respcliente as RESPUESTACLI 
                FROM dbxschema.dordenservicio DOSE, dbxschema.dordensolucion DOSO, terrorsolucion terr, ttiporespuesta ttip WHERE DOSE.mord_numero = DOSO.mord_numero
                AND DOSE.dord_id = DOSO.dord_iddet AND DOSE.mord_numero = " + Request.QueryString["nosc"] + " and terr.terr_codigo = doso.terr_codigo and DOSO.TTIPO_RESP = ttip.ttipo_resp;");

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT distinct OS.mord_numero as IDORDEN, OS.dord_detalle as DETALLE, ORS.DORD_RESPUESTA as RESPUESTATEC, ORS.TERR_CODIGO CONCAT ' - ' CONCAT TERR.TERR_NOMBRE AS ORIGEN_ERR, ORS.TTIPO_RESP CONCAT ' - ' CONCAT ttip.TTIPO_DESCRIPCION AS TIPO_RESPUESTA, ORS.dord_respcliente as RESPUESTACLI 
                FROM terrorsolucion terr, ttiporespuesta ttip,
                dordenservicio OS left join DORDENSOLUCION ORS ON OS.MORD_NUMERO = ORS.MORD_NUMERO AND OS.DORD_ID = ORS.DORD_IDDET
                WHERE OS.mord_numero = " + Request.QueryString["nosc"] + " ;");
            }

            if (dtRespuesta == null)
                Cargar_dtRespuesta();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow fila = dtRespuesta.NewRow();
                fila[0] = Convert.ToInt32(ds.Tables[0].Rows[i]["IDORDEN"]);
                fila[1] = ds.Tables[0].Rows[i]["DETALLE"].ToString();
                fila[2] = ds.Tables[0].Rows[i]["RESPUESTATEC"].ToString();
                fila[3] = ds.Tables[0].Rows[i]["RESPUESTACLI"].ToString();
                fila[4] = ds.Tables[0].Rows[i]["ORIGEN_ERR"].ToString();
                fila[5] = ds.Tables[0].Rows[i]["TIPO_RESPUESTA"].ToString();
                dtRespuesta.Rows.Add(fila);
                dgRespuesta_DataBind();
            }
        }

        private void Llenar_dtRespuesta_Desarrollo()
        {
            DataSet ds = new DataSet();

            DBFunctions.Request(ds, IncludeSchema.NO,
            @"SELECT DOSO.mord_numero as IDORDEN,DOSE.dord_detalle as DETALLE, DOSO.dord_respuesta as RESPUESTATEC, TERR.TERR_CODIGO CONCAT ' - ' CONCAT TERR.TERR_NOMBRE AS ORIGEN_ERR,  DOSO.TTIPO_RESP CONCAT ' - ' CONCAT ttip.TTIPO_DESCRIPCION AS TIPO_RESPUESTA, DOSO.dord_respcliente as RESPUESTACLI 
            FROM dbxschema.dordenservicio DOSE, dbxschema.dordensolucion DOSO, terrorsolucion terr, ttiporespuesta ttip WHERE DOSE.mord_numero = DOSO.mord_numero
            AND DOSE.dord_id = DOSO.dord_iddet AND DOSE.mord_numero = " + Request.QueryString["nosc"] + " and terr.terr_codigo = doso.terr_codigo and DOSO.TTIPO_RESP = ttip.ttipo_resp;");
            
            if (dtRespuesta == null)
                Cargar_dtRespuesta();

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds = new DataSet();

                DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT distinct DOSE.mord_numero as IDORDEN, DOSE.dord_detalle as DETALLE, DOR.dord_respuesta as RESPUESTATEC, DOR.TERR_CODIGO CONCAT ' - ' CONCAT TERR.TERR_NOMBRE AS ORIGEN_ERR, DOR.TTIPO_RESP CONCAT ' - ' CONCAT ttip.TTIPO_DESCRIPCION AS TIPO_RESPUESTA, DOR.dord_respcliente as RESPUESTACLI 
                FROM terrorsolucion terr, ttiporespuesta ttip, dordenservicio DOSE left join DORDENSOLUCION DOR ON DOSE.MORD_NUMERO = DOR.MORD_NUMERO AND DOSE.DORD_ID = DOR.DORD_IDDET
                WHERE DOSE.mord_numero = " + Request.QueryString["nosc"] + " ;");
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow fila = dtRespuesta.NewRow();
                fila[0] = Convert.ToInt32(ds.Tables[0].Rows[i]["IDORDEN"]);
                fila[1] = ds.Tables[0].Rows[i]["DETALLE"].ToString();
                fila[2] = ds.Tables[0].Rows[i]["RESPUESTATEC"].ToString();
                fila[3] = ds.Tables[0].Rows[i]["RESPUESTACLI"].ToString();
                fila[4] = ds.Tables[0].Rows[i]["ORIGEN_ERR"].ToString();
                fila[5] = ds.Tables[0].Rows[i]["TIPO_RESPUESTA"].ToString();
                dtRespuesta.Rows.Add(fila);
                dgRespuesta_DataBind();
            }
        }

        private string Llenar_lbArchivos()
        {
            string archivos = "";            
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT dord_archivo AS ARCHIVO FROM dordensolucionarchivo WHERE mord_numero=" + Request.QueryString["nosc"] + "");
            if (ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    archivos += ds.Tables[0].Rows[i]["ARCHIVO"].ToString() + "<br>";
            }
            return archivos;
        }

        private void dgRespuesta_DataBind()
        {
            dgRespuesta.DataSource = dtRespuesta;
            dgRespuesta.DataBind();
            Session["dtRespuesta"] = dtRespuesta;
        }

        private bool Verificar_dtRespuesta()
        {
            bool error = false;
            int cont = 0;
            for (int i = 0; i < dtRespuesta.Rows.Count; i++)
            {
                if (dtRespuesta.Rows[i]["RESPUESTA"].ToString() == "")
                    cont++;
                else if (dtRespuesta.Rows[i][4].ToString() == "NN")
                {
                    error = true;
                    Utils.MostrarAlerta(Response, "No ha seleccionado origen.\\n Imposible guardar la respuesta");
                }
            }

            if (dtRespuesta.Rows.Count == cont)
                error = true;
            return error;
        }

        private void Cargar_Datos()
        {
            DataSet ds = new DataSet();
            string solicitud = DBFunctions.SingleData("SELECT msol_numero FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "");
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM msolicitud WHERE msol_numero=" + solicitud + ";" +
             "SELECT * FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + ";");
            //Número de la OSC
            lbnum.Text = Request.QueryString["nosc"];
            //Número de la Solicitud
            lbnumsol.Text = solicitud;
            //Nit del Cliente
            lbnitcli.Text = ds.Tables[0].Rows[0]["MNIT_NITCLI"].ToString();
            //Razón Social del Cliente
            lbnomcli.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0]["MNIT_NITCLI"].ToString() + "'");
            //Cedula del Contacto
            lbcedcon.Text = ds.Tables[0].Rows[0]["MNIT_NITCON"].ToString();
            //Nombre del Contacto
            lbnomcon.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2  FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0]["MNIT_NITCON"].ToString() + "'");
            //Fecha y Hora de la Solicitud
            lbfechorsol.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["MSOL_FECHA"]).ToString("yyyy-MM-dd") + " - " + ds.Tables[0].Rows[0]["MSOL_HORA"].ToString();
            //Via de la Solicitud
            lbsolvia.Text = DBFunctions.SingleData("SELECT tsol_nombre FROM tsolicitud WHERE tsol_codigo='" + ds.Tables[0].Rows[0]["TSOL_CODIGO"].ToString() + "'");
            //Persona que abrio la OSC
            lbaseaper.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2  FROM mnit WHERE mnit_nit='" + ds.Tables[1].Rows[0]["MNIT_NIT"].ToString() + "'");
            //Fecha y Hora de Apertura de la OSC
            if (ds.Tables[1].Rows[0]["MORD_FECHA"].ToString() != "")
                lbfechoraper.Text = Convert.ToDateTime(ds.Tables[1].Rows[0]["MORD_FECHA"]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0]["MORD_HORA"].ToString();
            else
                lbfechoraper.Text = "";
            //Asesor Asignado
            lbaseasig.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2  FROM mnit WHERE mnit_nit='" + ds.Tables[1].Rows[0]["PAS_MNIT_NIT"].ToString() + "'");
            //Fecha y Hora de Asignación
            if (ds.Tables[1].Rows[0]["MORD_FECHAASIG"].ToString() != "")
                lbfechorasig.Text = Convert.ToDateTime(ds.Tables[1].Rows[0]["MORD_FECHAASIG"]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0]["MORD_HORAASIG"].ToString();
            else
                lbfechorasig.Text = "";
            //Estado de la Orden de Servicio
            lbestosc.Text = GetCambioEstado( ds.Tables[1].Rows[0]["TEST_CODIGO"].ToString() );
        }

        protected string GetCambioEstado(string estadoActual)
        {
            chbCierre.Visible = false;
            string nuevoEstado = "";
            if (Request.QueryString["nuevoEstado"] != null)
                nuevoEstado = Request.QueryString["nuevoEstado"].ToString();

            switch (estadoActual)
            {
                case "AS":  //Asignada
                    return "Asignada -> En Desarrollo";
                case "ED":  //En Desarrollo
                    if (nuevoEstado == "CC")
                        return "En Desarrollo -> Control Calidad";
                    else
                        return "Asignada <- En Desarrollo";
                case "CC":  //Control Calidad
                    if (nuevoEstado == "IM")
                        return "Control Calidad -> Implementada";
                    else
                        return "En Desarrollo <- Control Calidad";
                case "IM":  //Implementada
                    if (nuevoEstado == "CE")
                    {
                        chbCierre.Visible = true;
                        return "Implementada -> Cerrada";
                    }
                    else
                        return "Control Calidad <- Implementada";
                        
                default:
                    return "";
            }
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
