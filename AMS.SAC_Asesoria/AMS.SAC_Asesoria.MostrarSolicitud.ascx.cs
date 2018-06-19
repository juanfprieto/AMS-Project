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


    public partial class MostrarSolicitud : System.Web.UI.UserControl
    {
        //protected TextBox tbEmail;
        //protected Label lbnum,lbnitcli,lbnitcon,lbfechor,lbnomcli,lbnomcon,lb;
        //protected ImageButton ibMail;
        //protected DataGrid dgSolicitud,dgArchivos;
        //protected PlaceHolder phSolicitud;
        protected string pathToUploads = ConfigurationSettings.AppSettings["Uploads"];
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];

        protected void Page_Load(object Sender, EventArgs e)
        {
            Cargar_Datos();
            Session["rep"] = RenderHtml();
            if (!IsPostBack)
            {
                if (Request.QueryString["inf"] != null)
                    Envio_Solicitud();
            }
        }

        protected void SendMail(object Sender, ImageClickEventArgs e)
        {
            Enviar_Correo();
        }

        private void Cargar_Datos()
        {
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM msolicitud WHERE msol_numero=" + Request.QueryString["num"] + ";" +
                //"SELECT DSOL.dsol_detalle AS \"Detalle\",PPRO.ppro_descripcion AS \"Programa Asociado\" FROM dbxschema.dsolicitud DSOL,dbxschema.pproducto PPRO WHERE DSOL.ppro_id=PPRO.ppro_id AND DSOL.msol_numero="+Request.QueryString["num"]+";"+
                               "SELECT DSOL.dsol_detalle AS Detalle, SMP.SMEN_NOMBRE_PRINCIPAL CONCAT '/' CONCAT SMC.SMEN_NOMBRE_CARPETA CONCAT '/' CONCAT SMO.SMEN_NOMBRE_OPCION AS Programa  \n" +
                               "FROM dbxschema.dsolicitud DSOL, SMENUOPCION SMO, SMENUCARPETA SMC, SMENUPRINCIPAL SMP  \n" +
                               "WHERE DSOL.ppro_id = SMO.SMEN_OPCION AND SMO.SMEN_CARPETA = SMC.SMEN_CARPETA AND SMC.SMEN_PRINCI = SMP.SMEN_PRINCI AND DSOL.msol_numero = " + Request.QueryString["num"] + ";" +
                               "SELECT dsol_nombre AS ARCHIVO FROM dbxschema.dsolicitudarchivo WHERE msol_numero=" + Request.QueryString["num"] + "");
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

        protected string RenderHtml()
        {
            StringBuilder SB = new StringBuilder();
            StringWriter SW = new StringWriter(SB);
            HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
            phSolicitud.RenderControl(htmlTW);
            return SB.ToString();
        }

        protected void Envio_Solicitud()
        {
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT mnit_email FROM mnit WHERE mnit_nit IN('" + lbnitcli + "','" + lbnitcon + "')");
            string to = ConfigurationSettings.AppSettings["EmailFrom"] + ";";
            if (ds.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i != ds.Tables[0].Rows.Count - 1)
                        to += ds.Tables[0].Rows[i][0].ToString() + ";";
                    else
                        to += ds.Tables[0].Rows[i][0].ToString();
                }
                Enviar_Correo(to);
            }
        }

        protected void Enviar_Correo()
        {
            MailMessage MyMail = new MailMessage();
            MyMail.From = ConfigurationSettings.AppSettings["EmailFrom"];
            MyMail.To = tbEmail.Text;
            MyMail.Subject = "Solicitud de Asesoria Nº " + lbnum.Text;
            MyMail.Body = (RenderHtml());
            MyMail.BodyFormat = MailFormat.Html;
            try
            {
                SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["MailServer"];
                SmtpMail.Send(MyMail);
                lb.Text = "Solicitud Nº " + lbnum.Text + " enviada satisfactoriamente  a : " + tbEmail.Text;
            }
            catch (Exception ex)
            {
                lb.Text = "Error al enviar la solicitud " + ex.ToString();
            }
        }

        protected void Enviar_Correo(string to)
        {
            MailMessage MyMail = new MailMessage();
            MyMail.From = ConfigurationSettings.AppSettings["EmailFrom"];
            MyMail.To = to;
            MyMail.Subject = "Solicitud de Asesoria Nº " + lbnum.Text;
            MyMail.Body = (RenderHtml());
            MyMail.BodyFormat = MailFormat.Html;
            try
            {
                SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["MailServer"];
                SmtpMail.Send(MyMail);
                lb.Text = "Solicitud Nº " + lbnum.Text + " enviada satisfactoriamente  a : " + to;
            }
            catch (Exception ex)
            {
                lb.Text = "Error al enviar la solictud " + ex.ToString();
            }
        }

        protected void btnRegresarClick(object sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.ConsultarSolicitudCliente");
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
