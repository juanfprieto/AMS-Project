namespace AMS.Inventarios
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Descripción breve de AMS_Inventarios_Facturador.
	/// </summary>
	public class AMS_Inventarios_Facturador : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox detail;
		protected System.Web.UI.WebControls.DataGrid dgInserts;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.WebControls.Button Button3;
		protected System.Web.UI.WebControls.Label lbInfo;
		protected System.Web.UI.WebControls.ListBox accounts;
		protected System.Web.UI.WebControls.ListBox nits;
		protected System.Web.UI.WebControls.ListBox centros;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
