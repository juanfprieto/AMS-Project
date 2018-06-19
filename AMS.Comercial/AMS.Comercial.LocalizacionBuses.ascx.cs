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
	///		Descripción breve de Comercial_LocalizacionBuses.
	/// </summary>
	public class Comercial_LocalizacionBuses : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtPlacaa;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox txtComentarios;
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		protected System.Web.UI.WebControls.Panel pnlAgencia;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlEstado;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label3;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//Traer agencias del usuario
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				bind.PutDatasIntoDropDownList(ddlEstado,"select testa_codigo, testa_descripcion "+
					"from DBXSCHEMA.TESTADOBUSAFILIADO where testa_codigo>0;");
				ddlEstado.Items.Insert(0,new ListItem("---Seleccione---",""));
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El bus ha sido ubicado en la agencia");
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlAgencia.Visible=true;
			//Cambia la ruta
			if(agencia.Length==0)
			{
				pnlAgencia.Visible=false;
				return;
			}
		}

		private void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			string estado=ddlEstado.SelectedValue;
			string placa=txtPlaca.Text.Trim();
			if(placa.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar la placa del bus.");
				return;
			}
			if(estado.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar el estado del bus.");
				return;
			}
			ArrayList sqlD=new ArrayList();
			//Cambiar tabla de localizacion
			sqlD.Add("delete from dbxschema.mbus_localizacion where mcat_placa='"+placa+"';");
			sqlD.Add("insert into dbxschema.mbus_localizacion values('"+placa+"',"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"',"+estado+",'"+txtComentarios.Text+"');");
			sqlD.Add("update dbxschema.mbusafiliado set testa_codigo="+estado+" where mcat_placa='"+placa+"';");
			if(!DBFunctions.Transaction(sqlD)){
				lblError.Text=DBFunctions.exceptions;
				return;}
			Response.Redirect(indexPage+"?process=Comercial.LocalizacionBuses&act=1&path="+Request.QueryString["path"]);
		}
	}
}
