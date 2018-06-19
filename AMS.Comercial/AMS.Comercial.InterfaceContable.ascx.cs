namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using AMS.CriptoServiceProvider;
	using Ajax;
	using System.Configuration;

	/// <summary>
	///		Descripción breve de AMS_Comercial_InterfaceContable.
	/// </summary>
	public class AMS_Comercial_InterfaceContable : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlProceso;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack){
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlProceso,"SELECT MODULO, NOMBRE FROM DBXSCHEMA.TPROCESOS_CONTABLES_TRANSPORTES;");
				ddlProceso.Items.Insert(0,new ListItem("--seleccione--",""));
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
			this.ddlProceso.SelectedIndexChanged += new System.EventHandler(this.ddlProceso_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlProceso_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(ddlProceso.SelectedValue.Length>0)
				Response.Redirect(indexPage+"?process=Comercial.InterfaceContable"+ddlProceso.SelectedValue+"&path="+Request.QueryString["path"]+"&prc="+ddlProceso.SelectedValue);
		}
	}
}
