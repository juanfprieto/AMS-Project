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
    using Tools;

    public partial class MostrarOSC : System.Web.UI.UserControl
    {

        //protected Button btnVolver;
        //protected PlaceHolder toolsHolder,phOSC;
        //protected TextBox tbEmail;
        //protected ImageButton ibMail;
        //protected Label lb,lbnum,lbnumsol,lbnitcli,lbnomcli,lbcedcon,lbnomcon,lbfechorsol,lbsolvia,lbaseaper,lbfechoraper,lbaseasig,lbfechorasig,lbfechorcie,lbestosc;
        //protected DataGrid dgSolRes,dgArc;
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        protected string pathToUploads = ConfigurationSettings.AppSettings["Uploads"];

        protected void Page_Load(object Sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Cargar_Datos();
                Session["rep"] = RenderHtml();
                string estadoCorreo = Request.QueryString["codCorreoEnv"];
                
                if(estadoCorreo == "x")
                {
                    Utils.MostrarAlerta(Response, "Hubo un error al envíar el correo de confirmación. Por favor revisar.");
                }
                else if(estadoCorreo != null && estadoCorreo != "")
                {
                    DataSet dsCorreos = new DataSet();
                    DBFunctions.Request(dsCorreos, IncludeSchema.NO,
                        @"SELECT mn.mnit_email CONCAT ' - ' CONCAT mn.mnit_nombres CONCAT ' ' CONCAT mn.mnit_nombre2 CONCAT ' ' 
                        CONCAT mn.mnit_apellidos CONCAT ' ' CONCAT mn.mnit_apellido2 CONCAT '.\n'
                        FROM MCONTACTO mc,   mnit mn WHERE mc.mnit_nitcli = '" + Request.QueryString["codCorreoEnv"] + "' AND   mc.mnit_nitcon = mn.mnit_nit;");

                    string correoUsuario = "";
                    for (int k = 0; k < dsCorreos.Tables[0].Rows.Count; k++)
                        correoUsuario += dsCorreos.Tables[0].Rows[k][0];

                    Utils.MostrarAlerta(Response, "Correo de Confirmación enviado a: \\n" + correoUsuario);
                }
            }

        }

        protected void SendMail(object Sender, ImageClickEventArgs e)
        {
            string info = "";
            MailMessage MyMail = new MailMessage();
            MyMail.From = ConfigurationSettings.AppSettings["EmailFrom"];
            MyMail.To = tbEmail.Text;
            if (DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "") == "AB")
                info = "Se ha creado la orden de servicio Nº " + Request.QueryString["nosc"] + " pronto recibirá una respuesta a cada una de " +
                    "sus solicitudes. Por favor tenga en cuenta de el número referenciado anteriormente para futuras comunicaciones.";
            else if (DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "") == "AS")
                info = "A la orden de servicio Nº " + Request.QueryString["nosc"] + " se le ha asignado un asesor de servicio al cliente, " +
                    "el sera la persona encargada de dar respuesta a sus solicitudes.";
            else if (DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "") == "ED")
                info = "La orden de servicio Nº " + Request.QueryString["nosc"] + " ha recibido una respuesta por parte del equipo de " +
                    "asesoria y desarrollo de Sistemas Ecas. Por favor revise las soluciones planteadas y siga paso a paso cualquier " +
                    "procedimiento planteado. La orden aun tiene solicitudes sin respuesta, por lo que seguiremos trabajando en ellas.";
            else if (DBFunctions.SingleData("SELECT test_codigo FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "") == "CE")
                info = "La orden de servicio Nº " + Request.QueryString["nosc"] + " ha recibido respuesta completa por parte del equipo de " +
                    "asesoria y desarrollo de Sistemas Ecas. Por favor revise las solicitudes planteadas y las respuestas asociadas a " +
                    "cada una de ellas, siga paso a paso cualquier procedimiento planteado.";
            else
                info = "Orden de Servicio Nº " + Request.QueryString["nosc"];
            MyMail.Subject = "Orden de Servicio Nº " + lbnum.Text;
            MyMail.Body = info + "<br><br><br><br>" + (RenderHtml());
            MyMail.BodyFormat = MailFormat.Html;
            try
            {
                SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["MailServer"];
                SmtpMail.Send(MyMail);
            }
            catch (Exception ex)
            {
                lb.Text = ex.ToString();
            }
        }

        protected string RenderHtml()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            phOSC.RenderControl(htmlTW);
            return SB.ToString();
        }

        private void Cargar_Datos()
        {
            DataSet ds = new DataSet();
            DataSet dsSol = new DataSet();
            //toolsHolder.Visible=phOSC.Visible=true;
            string solicitud = DBFunctions.SingleData("SELECT msol_numero FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + "");
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM msolicitud WHERE msol_numero=" + solicitud + ";" +
                               "SELECT * FROM mordenservicio WHERE mord_numero=" + Request.QueryString["nosc"] + ";" +
                               "SELECT DOSE.dord_detalle AS SOLICITUD,DOSO.dord_respuesta AS RESPUESTATEC, DOSO.dord_respcliente as RESPUESTACLI FROM dbxschema.dordenservicio DOSE,dbxschema.dordensolucion DOSO WHERE DOSE.mord_numero=DOSO.mord_numero AND DOSE.dord_id=DOSO.dord_iddet AND DOSE.mord_numero=" + Request.QueryString["nosc"] + ";" +
                               "SELECT DSOL_NOMBRE AS ARCHIVO FROM DSOLICITUDARCHIVO WHERE MSOL_NUMERO = " + solicitud + " union SELECT dord_archivo AS ARCHIVO FROM dordensolucionarchivo WHERE mord_numero= " + solicitud + ";");
            //Número de la OSC
            lbnum.Text = Request.QueryString["nosc"];
            //Número de la Solicitud
            lbnumsol.Text = solicitud;
            //Nit del Cliente
            lbnitcli.Text = ds.Tables[0].Rows[0][1].ToString();
            //Razón Social del Cliente
            lbnomcli.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
            //Cedula del Contacto
            lbcedcon.Text = ds.Tables[0].Rows[0][2].ToString();
            //Nombre del Contacto
            lbnomcon.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2  FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
            //Fecha y Hora de la Solicitud
            lbfechorsol.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][3]).ToString("yyyy-MM-dd") + " - " + ds.Tables[0].Rows[0][4].ToString();
            //Via de la Solicitud
            lbsolvia.Text = DBFunctions.SingleData("SELECT tsol_nombre FROM tsolicitud WHERE tsol_codigo='" + ds.Tables[0].Rows[0][5].ToString() + "'");

            //Persona que abrio la OSC
            lbaseaper.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2  FROM mnit WHERE mnit_nit='" + ds.Tables[1].Rows[0][2].ToString() + "'");
            //Fecha y Hora de Apertura de la OSC
            if (ds.Tables[1].Rows[0][5].ToString() != "")
                lbfechoraper.Text = Convert.ToDateTime(ds.Tables[1].Rows[0][5]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0][6].ToString();
            else
                lbfechoraper.Text = "";
            //Asesor Asignado
            lbaseasig.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2  FROM mnit WHERE mnit_nit='" + ds.Tables[1].Rows[0][3].ToString() + "'");
            //Fecha y Hora de Asignación
            if (ds.Tables[1].Rows[0][7].ToString() != "")
                lbfechorasig.Text = Convert.ToDateTime(ds.Tables[1].Rows[0][7]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0][8].ToString();
            else
                lbfechorasig.Text = "";
            //Estado de la Orden de Servicio
            lbestosc.Text = DBFunctions.SingleData("SELECT test_nombre FROM testadoorden WHERE test_codigo='" + ds.Tables[1].Rows[0][4].ToString() + "'");

            //Solicitudes Y Respuestas
            if (ds.Tables[2].Rows.Count != 0)
            {
                dgSolRes.DataSource = ds.Tables[2];
            }
            else
            {
                //Si aun no hay respuestas, solo llama solucitues.
                DBFunctions.Request(dsSol, IncludeSchema.NO, "SELECT DOSE.dord_detalle AS SOLICITUD, '' AS RESPUESTATEC, '' AS RESPUESTACLI  FROM dbxschema.dordenservicio DOSE WHERE DOSE.mord_numero=" + Request.QueryString["nosc"] + ";");
                dgSolRes.DataSource = dsSol.Tables[0];
            }

            dgSolRes.DataBind();

            //Archivos Adjuntos
            dgArc.DataSource = ds.Tables[3];
            dgArc.DataBind();
            for (int i = 0; i < dgArc.Items.Count; i++)
            {
                ((HyperLink)dgArc.Items[i].Cells[1].FindControl("hpldes")).NavigateUrl = pathToUploads + dgArc.Items[i].Cells[0].Text;
                ((HyperLink)dgArc.Items[i].Cells[1].FindControl("hpldes")).Target = "_blank";
            }

            //Fecha y Hora de Cierre
            if (ds.Tables[1].Rows[0][9].ToString() != "")
                lbfechorcie.Text = Convert.ToDateTime(ds.Tables[1].Rows[0][9]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0][10].ToString();
            else
                lbfechorcie.Text = "";
        }

        protected void btnRegresarClick(object sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente&usuOrde=1");
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
