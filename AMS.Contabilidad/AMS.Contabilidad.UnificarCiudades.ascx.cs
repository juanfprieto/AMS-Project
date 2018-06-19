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
	///		Descripci�n breve de AMS_Contabilidad_UnificarCiudades.
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
	}
}
