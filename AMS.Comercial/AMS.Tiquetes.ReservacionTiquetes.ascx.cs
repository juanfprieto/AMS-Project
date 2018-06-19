namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Descripción breve de AMS_Tiquetes_ReservacionTiquetes.
	/// </summary>
	public class AMS_Tiquetes_ReservacionTiquetes : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox txtNombres;
		protected System.Web.UI.WebControls.TextBox txtAlm;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.TextBox tbDate;
		protected System.Web.UI.WebControls.Calendar calDate;
		protected System.Web.UI.WebControls.Button btnReserva;
		protected System.Web.UI.WebControls.Label lbInfo;

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
