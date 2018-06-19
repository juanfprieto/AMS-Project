namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Descripci�n breve de AMS_Tiquetes_ReservacionTiquetes.
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
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
		}

		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�todo necesario para admitir el Dise�ador. No se puede modificar
		///		el contenido del m�todo con el editor de c�digo.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
