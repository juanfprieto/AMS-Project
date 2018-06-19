namespace AMS.Inventarios
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;

	/// <summary>
	///		Descripción breve de AMS_Inventarios_CierrePedidos.
	/// </summary>
                  
	public class AMS_Inventarios_CierrePedidos : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label mesVigente;
		protected System.Web.UI.WebControls.Label anoVigente;
		protected System.Web.UI.WebControls.Button btnProceso;
		protected System.Web.UI.WebControls.Label lb;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				anoVigente.Text=DBFunctions.SingleData("SELECT pano_ano FROM cinventario");
				mesVigente.Text=DBFunctions.SingleData("SELECT pmes_nombre from pmes WHERE pmes_mes=(SELECT pmes_mes FROM cinventario)");
				if(Request.QueryString["act"]!=null)
					Response.Write("<script language='javascript'>alert('Los pedidos han sido eliminados.');</script>");
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
			this.btnProceso.Click += new System.EventHandler(this.btnProceso_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		protected void btnProceso_Click(object sender, System.EventArgs e)
		{
			DateTime fechaHasta;

			int mesVig  =Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM cinventario"));
			int ano     =Convert.ToInt32(anoVigente.Text);
			fechaHasta  =new DateTime(ano,mesVig,1);
			fechaHasta  =fechaHasta.AddMonths(1);
			fechaHasta  =fechaHasta.AddDays(-1);
			PedidoFactura cancelarPedido = new PedidoFactura();
			if(cancelarPedido.EliminarPeds(fechaHasta))
			{
				string indexPage = ConfigurationSettings.AppSettings["MainIndexPage"];
				Response.Redirect(indexPage + "?process=Inventarios.CierrePedidos&act=1");
			}
			else
				lb.Text += "<br>Error : "+cancelarPedido.ProcessMsg;

		}
	}
}
