﻿namespace AMS.SAC_Asesoria
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


	public partial class AgendaAsesores : System.Web.UI.UserControl
	{

        protected DropDownList ddlnumero, ddlAsesor;
        protected Button btnconsultar, btnconsultarAse;
        protected PlaceHolder toolsHolder, phOSC;
        protected TextBox tbEmail;
        protected ImageButton ibMail;
        protected Label lb, lbnum, lbnumsol, lbnitcli, lbnomcli, lbcedcon, lbnomcon, lbfechorsol, lbsolvia, lbaseaper, lbfechoraper, lbaseasig, lbfechorasig, lbfechorcie, lbestosc;
        protected DataGrid dgSolRes, dgArc;
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
        protected string pathToDownload = ConfigurationSettings.AppSettings["UrlContentLocation"];

        protected void Page_Load(object Sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Session.Clear();
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlAsesor, "select mn.mnit_nit, mn.mnit_nombres CONCAT ' ' CONCAT mn.mnit_nombre2 CONCAT ' ' CONCAT mn.mnit_apellidos CONCAT ' ' CONCAT mn.mnit_apellido2 from mnit mn,pasesor pa where pa.mnit_nit=mn.mnit_nit");
                bind.PutDatasIntoDropDownList(ddlnumero, "SELECT MORD.mord_numero FROM dbxschema.mordenservicio MORD,dbxschema.msolicitud MSOL WHERE MORD.msol_numero=MSOL.msol_numero AND (MORD.TEST_CODIGO='AS' OR MORD.TEST_CODIGO='ED') AND MORD.PAS_mnit_nit='" + ddlAsesor.SelectedValue + "'");
                //toolsHolder.Visible = phOSC.Visible = false;
            }
        }

        protected void SendMail(object Sender, ImageClickEventArgs e)
        {
            string info = "";
            MailMessage MyMail = new MailMessage();
            MyMail.From = ConfigurationSettings.AppSettings["EmailFrom"];
            MyMail.To = tbEmail.Text;
            MyMail.Subject = "Orden de Servicio Nº " + lbnum.Text;
            MyMail.Body = info + "<br><br><br><br>" + (RenderHtml());
            MyMail.BodyFormat = MailFormat.Html;
            try
            {
                SmtpMail.SmtpServer = "ecasltda.com";
                SmtpMail.Send(MyMail);
            }
            catch (Exception ex)
            {
                lb.Text = ex.ToString();
            }
        }

        protected void btnconsultarAse_Click(object Sender, EventArgs e)
        {
            //  Session.Clear();
            // DatasToControls bind = new DatasToControls();      
            //bind.PutDatasIntoDropDownList(ddlnumero, "SELECT MORD.mord_numero FROM dbxschema.mordenservicio MORD,dbxschema.msolicitud MSOL WHERE MORD.msol_numero=MSOL.msol_numero AND MORD.TEST_CODIGO='AS'AND MORD.PAS_mnit_nit=" + ddlAsesor.SelectedValue + "");
            //ddlnumero.Visible = true;
            //btnconsultar.Visible = true;
        }

        protected void CambioAsesorSelected(object Sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlnumero, "SELECT MORD.mord_numero FROM dbxschema.mordenservicio MORD,dbxschema.msolicitud MSOL WHERE MORD.msol_numero=MSOL.msol_numero AND (MORD.TEST_CODIGO='AS' OR MORD.TEST_CODIGO='ED')AND MORD.PAS_mnit_nit='" + ddlAsesor.SelectedValue + "'");
        }

        protected void btnconsultar_Click(object Sender, EventArgs e)
        {
            if (ddlnumero.Items.Count != 0)
            {
                Cargar_Datos();
                Session["rep"] = RenderHtml();
            }
            else
            Utils.MostrarAlerta(Response, "No hay solicitudes registradas");
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
            toolsHolder.Visible = phOSC.Visible = true;
            string solicitud = DBFunctions.SingleData("SELECT msol_numero FROM mordenservicio WHERE mord_numero=" + ddlnumero.SelectedValue + "");
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM msolicitud WHERE msol_numero=" + solicitud + ";" +
                               "SELECT * FROM mordenservicio WHERE mord_numero=" + ddlnumero.SelectedValue + ";" +
                               "SELECT DOSE.dord_detalle AS SOLICITUD,DOSO.dord_respuesta AS RESPUESTA FROM dbxschema.dordenservicio DOSE,dbxschema.dordensolucion DOSO WHERE DOSE.mord_numero=DOSO.mord_numero AND DOSE.dord_id=DOSO.dord_iddet AND DOSE.mord_numero=" + ddlnumero.SelectedValue + ";" +
                               "SELECT dord_archivo AS ARCHIVO FROM dordensolucionarchivo WHERE mord_numero=" + ddlnumero.SelectedValue + "");
            //Número de la OSC
            lbnum.Text = ddlnumero.SelectedValue;
            //Número de la Solicitud
            lbnumsol.Text = solicitud;
            //Nit del Cliente
            lbnitcli.Text = ds.Tables[0].Rows[0][1].ToString();
            //Razón Social del Cliente
            lbnomcli.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
            //Cedula del Contacto
            lbcedcon.Text = ds.Tables[0].Rows[0][2].ToString();
            //Nombre del Contacto
            lbnomcon.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
            //Fecha y Hora de la Solicitud
            lbfechorsol.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][3]).ToString("yyyy-MM-dd") + " - " + ds.Tables[0].Rows[0][4].ToString();
            //Via de la Solicitud
            lbsolvia.Text = DBFunctions.SingleData("SELECT tsol_nombre FROM tsolicitud WHERE tsol_codigo='" + ds.Tables[0].Rows[0][5].ToString() + "'");

            //Persona que abrio la OSC
            lbaseaper.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[1].Rows[0][2].ToString() + "'");
            //Fecha y Hora de Apertura de la OSC
            if (ds.Tables[1].Rows[0][5].ToString() != "")
                lbfechoraper.Text = Convert.ToDateTime(ds.Tables[1].Rows[0][5]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0][6].ToString();
            else
                lbfechoraper.Text = "";
            //Asesor Asignado
            lbaseasig.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_nombre2 CONCAT ' ' CONCAT mnit_apellidos CONCAT ' ' CONCAT mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[1].Rows[0][3].ToString() + "'");
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
                DBFunctions.Request(dsSol, IncludeSchema.NO, "SELECT DOSE.dord_detalle AS SOLICITUD, '' AS RESPUESTA FROM dbxschema.dordenservicio DOSE WHERE DOSE.mord_numero=" + ddlnumero.SelectedValue + ";");
                dgSolRes.DataSource = dsSol.Tables[0];
            }

            dgSolRes.DataBind();

            //Archivos Adjuntos
            dgArc.DataSource = ds.Tables[3];
            dgArc.DataBind();
            for (int i = 0; i < dgArc.Items.Count; i++)
            {
                ((HyperLink)dgArc.Items[i].Cells[1].FindControl("hpldes")).NavigateUrl = pathToDownload + dgArc.Items[i].Cells[0].Text;
                ((HyperLink)dgArc.Items[i].Cells[1].FindControl("hpldes")).Target = "_blank";
            }

            //Fecha y Hora de Cierre
            if (ds.Tables[1].Rows[0][9].ToString() != "")
                lbfechorcie.Text = Convert.ToDateTime(ds.Tables[1].Rows[0][9]).ToString("yyyy-MM-dd") + " - " + ds.Tables[1].Rows[0][10].ToString();
            else
                lbfechorcie.Text = "";
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

