namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Microsoft.Web.UI.WebControls;
	using AMS.DB;
	using AMS.Forms;
	using Ajax;
	using AMS.DBManager;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_AsignarAgencias.
	/// </summary>
	public class AMS_Comercial_AsignarAgencias : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlUsuario;
		protected System.Web.UI.WebControls.CheckBoxList chkAgencias;
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.DataGrid dgrAgencias;
		protected System.Web.UI.WebControls.Panel pnlAgencia;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack){
				DatasToControls bind=new DatasToControls();
				//Traer agencias del usuario
                bind.PutDatasIntoDropDownList(ddlUsuario, "select susu_codigo, susu_login from dbxschema.susuario order by susu_login");
				ddlUsuario.Items.Insert(0,new ListItem("---Seleccione---",""));
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "Las agencias del usuario han sido actualizadas.");
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
			this.ddlUsuario.SelectedIndexChanged += new System.EventHandler(this.ddlUsuario_SelectedIndexChanged);
			this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlUsuario_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pnlAgencia.Visible=false;
			if(ddlUsuario.SelectedValue.Length==0)return;
			DataSet dsAgencias=new DataSet();
			DBFunctions.Request(dsAgencias,IncludeSchema.NO,"select ma.mag_codigo as codigo, ma.mage_nombre as nombre, case when su.mag_codigo is null then 0 else 1 end as existe from dbxschema.magencia ma "+
				"left join dbxschema.susuario_transporte_agencia su on su.mag_codigo=ma.mag_codigo and su.susu_codigo="+ddlUsuario.SelectedValue+" "+
				"order by mage_nombre;");
			dgrAgencias.DataSource=dsAgencias.Tables[0];
			dgrAgencias.DataBind();
			pnlAgencia.Visible=true;
		}

		private void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlD=new ArrayList();
			//Borrar actuales
			sqlD.Add("delete from dbxschema.susuario_transporte_agencia where susu_codigo="+ddlUsuario.SelectedValue+";");
			CheckBox chkAgenciaA;
			for(int n=0;n<dgrAgencias.Items.Count;n++){
				chkAgenciaA=(CheckBox)dgrAgencias.Items[n].FindControl("chkAgencia");
				if(chkAgenciaA.Checked)
					sqlD.Add("insert into dbxschema.susuario_transporte_agencia values("+ddlUsuario.SelectedValue+","+dgrAgencias.DataKeys[n]+");");
			}
			if(!DBFunctions.Transaction(sqlD)){
				lblError.Text=DBFunctions.exceptions;
				return;}
			Response.Redirect(indexPage+"?process=Comercial.AsignarAgencias&act=1&path="+Request.QueryString["path"]);
		}
	}
}