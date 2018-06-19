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
	///		Descripción breve de AMS_Marketing_RevisarResultadosEncuestas.
	/// </summary>
	public partial class AMS_Marketing_RevisarResultadosEncuestas : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlencuesta,"SELECT menc_codigo,menc_nombre FROM mencuesta");
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

		}
		#endregion

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			
		}
	}
}
