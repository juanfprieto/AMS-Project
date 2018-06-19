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


    public partial class AdministrarOSC : System.Web.UI.UserControl
    {
        protected string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];

        protected void Page_Load(object Sender, EventArgs e)
        {
            string sqlAsesor = "SELECT m.mord_numero concat ' - ' CONCAT ma.mnit_nombres CONCAT ' ' CONCAT ma.mnit_nombre2 CONCAT ' ' CONCAT ma.mnit_apellidos CONCAT ' ' CONCAT ma.mnit_apellido2 concat ' - ' CONCAT mc.mnit_nombres CONCAT ' ' CONCAT mc.mnit_nombre2 CONCAT ' ' CONCAT mc.mnit_apellidos CONCAT ' ' CONCAT mc.mnit_apellido2" +
                               " FROM mordenservicio m, mnit ma, mnit mc, msolicitud ms " +
                               " WHERE m.test_codigo IN ('AS','ED') and m.pas_mnit_nit = ma.mnit_nit and m.msol_numero = ms.msol_numero " +
                               " and ms.mnit_nitcli = MC.MNIT_NIT ORDER BY m.pas_mnit_nit; ";

            if (!IsPostBack)
            {
                if (ViewState["mostar"] == null && Request.QueryString["nsolic"] != null)
                {
                    ViewState["mostar"] = 1;
                    Utils.MostrarAlerta(Response, "No se Encontró Ninguna Solicitud Pendiente");
                }

                DatasToControls bind = new DatasToControls();
                //bind.PutDatasIntoDropDownList(ddloscasig,"SELECT mord_numero FROM mordenservicio WHERE test_codigo='AB'");
                //bind.PutDatasIntoDropDownList(ddloscresp, sqlAsesor);
                //bind.PutDatasIntoDropDownList(ddlasesor, "SELECT PASE.mnit_nit,PASE.mnit_nit CONCAT ' - ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT MNIT.mnit_nombre2 CONCAT ' ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT MNIT.mnit_apellido2 FROM pasesor PASE,mnit MNIT WHERE MNIT.mnit_nit=PASE.mnit_nit ORDER BY PASE.mnit_nit ASC");
                bind.PutDatasIntoDropDownList(ddlreabosc, "SELECT mord_numero FROM mordenservicio WHERE test_codigo IN ('CE')");
            }
        }

        protected void btnCrear_Click(object Sender, EventArgs e)
        {
            Response.Redirect(indexPage + "?process=SAC_Asesoria.CrearOSC");
        }

        protected void btnAsignar_Click(object Sender, EventArgs e)
        {
            //if(ddloscasig.Items.Count!=0)
            //{
            //    ArrayList sqls=new ArrayList();
            //    sqls.Add("UPDATE mordenservicio SET pas_mnit_nit='"+ddlasesor.SelectedValue+"',mord_fechaasig='"+DateTime.Now.Date.ToString("yyyy-MM-dd")+"',mord_horaasig='"+DateTime.Now.TimeOfDay.ToString().Substring(0,8)+"',test_codigo='AS' WHERE mord_numero="+ddloscasig.SelectedValue+"");
            //    if(DBFunctions.Transaction(sqls))
            //        //Response.Redirect(indexPage+"?process=Asesoria.AdministrarOSC");
            Response.Redirect(indexPage + "?process=SAC_Asesoria.CrearOSC&nosc=1");
            //    else
            //        lb.Text="Error "+DBFunctions.exceptions;
            //}
            //else
            //    Utils.MostrarAlerta(Response, "No hay ordenes de servicio creadas para asignar");
        }

        //protected void btnResponder_Click(object Sender, EventArgs e)
        //{
        //    Response.Redirect(indexPage + "?process=SAC_Asesoria.ResponderOSC&nosc=" + ddloscresp.SelectedValue.Split('-')[0].Trim() + "");
        //}

        protected void btnReAbrir_Click(object Sender, EventArgs e)
        {
            string sqlUpdateEstado = "UPDATE mordenservicio SET test_codigo='CC' " +
                                     ",MORD_FECHACALIDAD = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' " +
                                     ",MORD_HORACALIDAD='" + DateTime.Now.TimeOfDay.ToString().Substring(0, 8) + "' " +
                                     "WHERE mord_numero = " + ddlreabosc.SelectedValue + "";

            if (DBFunctions.NonQuery(sqlUpdateEstado) != -1)
                Response.Redirect(indexPage + "?process=SAC_Asesoria.AdministrarOSC");
            else
                lb.Text = "Error" + DBFunctions.exceptions;
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
