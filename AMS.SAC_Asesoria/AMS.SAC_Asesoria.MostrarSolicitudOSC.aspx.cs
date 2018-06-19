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

    public partial class MostrarSolicitudOSC : System.Web.UI.Page
    {
        //protected Label lbnum,lbnitcli,lbnitcon,lbfechor,lbnomcli,lbnomcon,lb;
        //protected DataGrid dgSolicitud;
        //protected PlaceHolder phSolicitud;

        protected void Page_Load(object Sender, EventArgs e)
        {
            Cargar_Datos();
        }

        private void Cargar_Datos()
        {
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT * FROM msolicitud WHERE msol_numero=" + Request.QueryString["num"] + ";" +
                               "SELECT dsol_detalle AS DETALLE FROM dsolicitud WHERE msol_numero=" + Request.QueryString["num"] + "");
            lbnum.Text = Request.QueryString["num"];
            lbnitcli.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
            lbnomcli.Text = DBFunctions.SingleData("SELECT mnit_nombres concat ' ' concat  mnit_nombre2 concat ' ' concat  mnit_apellidos concat ' ' concat  mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][1].ToString() + "'");
            lbnitcon.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
            lbnomcon.Text = DBFunctions.SingleData("SELECT mnit_nombres concat ' ' concat  mnit_nombre2 concat ' ' concat  mnit_apellidos concat ' ' concat  mnit_apellido2 FROM mnit WHERE mnit_nit='" + ds.Tables[0].Rows[0][2].ToString() + "'");
            lbfechor.Text = Convert.ToDateTime(ds.Tables[0].Rows[0][3]).ToString("yyyy-MM-dd") + " - " + ds.Tables[0].Rows[0][4].ToString();
            dgSolicitud.DataSource = ds.Tables[1]; ;
            dgSolicitud.DataBind();
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
