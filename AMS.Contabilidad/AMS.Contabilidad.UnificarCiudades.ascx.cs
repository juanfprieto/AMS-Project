namespace AMS.Contabilidad
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Contabilidad_UnificarCiudades.
	/// </summary>
	public partial class AMS_Contabilidad_UnificarCiudades : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
					
				
			}
		}

		protected  void  unificar_Ciudades(object  sender , EventArgs e )	
		{	
			if(this.txtCiudaderrada.Text != this.txtCiudadcorrecta.Text)
			{
				string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
				Reclasificar obj = new Reclasificar(txtCiudadcorrecta.Text,txtCiudaderrada.Text,"PCIU_CODIGO","PCIUDAD");
				if(obj.ProcUnParam())
					Response.Redirect("" + indexPage + "?process=Contabilidad.UnificarCiudades");
				else
					lb.Text = obj.ProcessMsg;
			}
			else
            Utils.MostrarAlerta(Response, "Ciudades Iguales");
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
	}
}
