using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Tools;
using AMS.Forms;
using System.Configuration;
using System.Collections;


namespace AMS.Calidad
{
	/// <summary>
	///		Descripción breve de AMS_Calidad_SistemaGestionCalidad.
	/// </summary>
	public class SistemaGestionCalidad : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlform;
		protected System.Web.UI.WebControls.DropDownList ddlproc;
        protected System.Web.UI.WebControls.DropDownList ddlprocDocs;
        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		string documento,archivo="";
		protected System.Web.UI.WebControls.Button Button1;
        protected System.Web.UI.WebControls.Button btnMostrarDoc;
        DatasToControls bind = new DatasToControls();
        private void Page_Load(object sender, System.EventArgs e)
		{
            //Ajax
            Ajax.Utility.RegisterTypeForAjax(typeof(SistemaGestionCalidad));
            
            // Introducir aquí el código de usuario para inicializar la página
            if (!IsPostBack)
            {
                
                //bind.PutDatasIntoDropDownList(ddlproc, "select MSGC_CODIPROC,MSGC_NOMBPROC from dbxschema.MSGC_PROCEDIMIENTO order by MSGC_NOMBPROC");
                bind.PutDatasIntoDropDownList(ddlproc, "SELECT PARE_CODIGO, PARE_NOMBRE FROM  PAREAPROCEDIMIENTO;");
                //bind.PutDatasIntoDropDownList(ddlprocDocs, "select MSGC_CODIPROC,MSGC_NOMBPROC from dbxschema.MSGC_PROCEDIMIENTO order by MSGC_NOMBPROC");
                ListItem it = new ListItem("--Seleccione AREA de PROCEDIMIENTO --", "0");
                ddlproc.Items.Insert(0, it);

                bindFormatos(null);
			}
		}
        private void bindFormatos(string proceso)
        {
            DatasToControls bind = new DatasToControls();

           string sql = "select MSGC_CODIFORMA,MSGC_NOMBFORMA from dbxschema.MSGC_FORMATOS";

            if (proceso != null && proceso != "")
                sql += String.Format(" where MSGC_CODIPROC = '{0}'", proceso);

            bind.PutDatasIntoDropDownList(ddlform, sql);
            ListItem it = new ListItem("--Seleccione FORMATO --", "0");
            ddlform.Items.Insert(0, it);
        }

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
        //private void InitializeComponent()
        //{
        //    this.ddlproc.SelectedIndexChanged += new System.EventHandler(this.ddlproc_SelectedIndexChanged);
        //    this.ddlform.SelectedIndexChanged += new System.EventHandler(this.ddlform_SelectedIndexChanged);
        //    this.Button1.Click += new System.EventHandler(this.Button1_Click);
        //    this.Load += new System.EventHandler(this.Page_Load);

        //}
		#endregion

		protected void ddlproc_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			archivo="";
			documento="";
            string proceso = ddlproc.SelectedValue;
            //Label2.Text="Formatos Relacionados";
            try
            {  bind.PutDatasIntoDropDownList(ddlprocDocs, "select MSGC_CODIPROC,MSGC_NOMBPROC from dbxschema.MSGC_PROCEDIMIENTO where PARE_CODIGO = '" + ddlproc.SelectedValue + "' order by MSGC_NOMBPROC;");
                ddlprocDocs.Items.Insert(0, "seleccione...");
            }                
            catch 
            { ddlprocDocs.ClearSelection() ;
              ddlprocDocs.Items.Insert(0, "seleccione...");
            }   
            bindFormatos(proceso); 
            archivo = DBFunctions.SingleData("select MSGC_RUTAARCH from dbxschema.MSGC_PROCEDIMIENTO where MSGC_CODIPROC='" + proceso + "'");
            if (archivo == null || archivo == "") return;

            documento = ConfigurationManager.AppSettings["PathToManuales"] + archivo;
            Response.Write("<script type=\"text/javascript\">w=window.open(\"" + documento + "\",\"Documento\",\"width=800,height=600\");</script>");
			
		}

        protected void ddlprocDocs_Changed(object sender, System.EventArgs e)
        {
           // string documentoHTML = "";
            string proceso = ddlprocDocs.SelectedValue;

            try { bind.PutDatasIntoDropDownList(ddlform, "SELECT  DISTINCT * FROM (SELECT  F.MSGC_CODIFORMA, F.MSGC_NOMBFORMA FROM MSGC_FORMATOS F WHERE  F.MSGC_CODIPROC = '"+ ddlprocDocs.SelectedValue + "' UNION SELECT  M.MSGC_CODIFORMA, F.MSGC_NOMBFORMA FROM MSGC_PROCEDIMIENTOFORMATO M, MSGC_FORMATOS F WHERE F.MSGC_CODIPROC = M.MSGC_CODIPROC AND  M.MSGC_CODIPROC = '"+proceso+"') ");
                ddlform.Items.Insert(0, "seleccione...");
                archivo = DBFunctions.SingleData("SELECT MSGC_DOCUADJU ARCHIVO FROM dbxschema.MSGC_PROCEDIMIENTO WHERE MSGC_CODIPROC = '"+ddlprocDocs.SelectedValue+"'");
                if (archivo == null || archivo == "") return;
                documento = ConfigurationManager.AppSettings["Uploads"] + archivo;
                Response.Write("<script type=\"text/javascript\">w=window.open(\"" + documento + "\",\"Documento\",\"width=800,height=600\");</script>");
            }
            catch
            {
                ddlform.ClearSelection();
                ddlform.Items.Insert(0, "No hay Documentos...");
            }
            //documentoHTML = DBFunctions.SingleData("select msgc_documento from msgc_procedimiento where msgc_codiproc = '" + proceso + "'");
            
        }
        protected void ddlform_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			archivo="";
			documento="";
            if (ddlform.SelectedIndex == 0) return;
            // Hashtable data = (Hashtable)((ArrayList)DBFunctions.RequestAsCollection( @"SELECT MSGC_DOCUADJU from MSGC_PROCEDIMIENTO WHERE  MSGC_CODIPROC = '" + ddlprocDocs.SelectedValue + "'"))[0];
            archivo = DBFunctions.SingleData("SELECT MSGC_RUTAFORMATO ARCHIVO FROM dbxschema.MSGC_FORMATOS WHERE MSGC_CODIPROC = '"+ ddlprocDocs.SelectedValue+"'");
            //string procedimiento = data["PROCEDIMIENTO"] as string;
           
            //archivo = data["ARCHIVO"] as string;
         
            //ddlproc.SelectedValue = procedimiento;
            //


            if (archivo == null || archivo == "") return;

            //documento = ConfigurationManager.AppSettings["PathToManuales"] + archivo;
            documento = ConfigurationManager.AppSettings["Uploads"] + archivo;
            //Response.Write("<script language='javascript'>alert('" + documento + "');</script>");
            Response.Write("<script type=\"text/javascript\">w=window.open(\"" + documento + "\",\"Documento\",\"width=800,height=600\");</script>");
				
		}

		protected void Button1_Click(object sender, System.EventArgs e)
		{
            archivo = DBFunctions.SingleData("SELECT MSGC_DOCUADJU from MSGC_PROCEDIMIENTO WHERE  MSGC_CODIPROC = '" + ddlprocDocs.SelectedValue + "'");
		}

        protected void cargarpdf_Click(object sender, System.EventArgs e)
        {
                archivo = DBFunctions.SingleData("SELECT MSGC_DOCUADJU from MSGC_PROCEDIMIENTO WHERE  MSGC_CODIPROC = '" + ddlprocDocs.SelectedValue + "'");
                string PDF = ConfigurationManager.AppSettings["Uploads"] + archivo;
                
        }

        [Ajax.AjaxMethod()]
        public string Abrir_Documento(string valor)
        {
           return DBFunctions.SingleData("select msgc_documento from msgc_procedimiento where msgc_codiproc = '" + valor + "';");
        }

        [Ajax.AjaxMethod()]
        public string Abrir_Formato(string valor)
        {
            return DBFunctions.SingleData("SELECT  MSGC_DISEÑOFORMA FROM MSGC_FORMATOS WHERE MSGC_CODIFORMA ='" + valor + "';");
        }
    }
}
