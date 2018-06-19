
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using System.Configuration;


	/// <summary>
	///		Descripción breve de AMS_Calidad_SistemaGestionCalidad.
	/// </summary>
    /// 
namespace AMS.Tools
{
	public partial class LeerPDF : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlform;
		protected System.Web.UI.WebControls.DropDownList ddlproc;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		string documento,archivo="";
		protected System.Web.UI.WebControls.Button Button1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if (!IsPostBack)
			{

				DatasToControls bind=  new DatasToControls();
				if(ddlproc.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlproc,"select MSGC_CODIPROC,MSGC_NOMBPROC from dbxschema.MSGC_PROCEDIMIENTO");
					ListItem it=new ListItem("--Seleccione PROCEDIMIENTO --","0");
					ddlproc.Items.Insert(0,it);
				}
				if(ddlform.Items.Count==0)
				{
					bind.PutDatasIntoDropDownList(ddlform,"select MSGC_CODIFORMA,MSGC_NOMBFORMA from dbxschema.MSGC_FORMATO");
					ListItem it=new ListItem("--Seleccione FORMATO --","0");
					ddlform.Items.Insert(0,it);
				}
				
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.ddlproc.SelectedIndexChanged += new System.EventHandler(this.ddlproc_SelectedIndexChanged);
			this.ddlform.SelectedIndexChanged += new System.EventHandler(this.ddlform_SelectedIndexChanged);
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

        private void ddlproc_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind1=  new DatasToControls();
			archivo="";
			documento="";
			if(ddlform.Items.Count==0)
			{
				bind1.PutDatasIntoDropDownList(ddlform,"select MSGC_CODIFORMA,MSGC_NOMBFORMA from dbxschema.MSGC_FORMATO");
				ListItem it=new ListItem("--Seleccione FORMATO Asociado con Procedimiento --","0");
				ddlform.Items.Insert(0,it);
			}
			Label2.Text="Formatos Relacionados";
			archivo=DBFunctions.SingleData("select MSGC_RUTAARCH from dbxschema.MSGC_PROCEDIMIENTO where MSGC_CODIPROC='"+ddlproc.SelectedValue+"'");
			documento= ConfigurationManager.AppSettings["PathToDocs"] + archivo;
			Response.Write("<script language:javascript>w=window.open('"+documento+"','','scrollbars=1,fullscreen=no');</script>");
			
		}

		private void ddlform_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind2=  new DatasToControls();
			archivo="";
			documento="";
			if(ddlproc.Items.Count==0)
			{
				bind2.PutDatasIntoDropDownList(ddlproc,"select MSGC_CODIPROC,MSGC_NOMBPROC from dbxschema.MSGC_PROCEDIMIENTO ");
				ListItem it=new ListItem("--Seleccione PROCEDIMIENTO --","0");
				ddlproc.Items.Insert(0,it);
			}
			Label1.Text="Procedimientos Relacionados";
			archivo=DBFunctions.SingleData("select MSGC_RUTAFORMATO from dbxschema.MSGC_FORMATO where MSGC_CODIPROC='"+ddlform.SelectedValue+"'");
			documento= ConfigurationManager.AppSettings["PathToDocs"] + archivo;
			Response.Write("<script language:javascript>w=window.open('"+documento+"','','scrollbars=1,fullscreen=no');</script>");
				
		}

		private void Button1_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.LeerPDF");
		}
	}
}
