namespace AMS.Marketing
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;

	/// <summary>
	///		Descripci�n breve de AMS_Marketing_RevisarResultadosEncuestas.
	/// </summary>
	public partial class AMS_Marketing_RevisarResultadosEncuestas : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlencuesta,"SELECT menc_codigo,menc_nombre FROM mencuesta");
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

		}
		#endregion

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			
		}
	}
}
