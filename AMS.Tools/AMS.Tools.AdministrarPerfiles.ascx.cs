using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Tools
{
	/// <summary>
	///		Descripci�n breve de AMS_Tools_AdministrarPerfiles.
	/// </summary>
	public partial class AdministrarPerfiles : System.Web.UI.UserControl
	{
		private string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlModPerfil,"SELECT ttipe_codigo,ttipe_descripcion FROM ttipoperfil WHERE ttipe_creacion='C'");
				bind.PutDatasIntoDropDownList(ddlEliPerfil,"SELECT ttipe_codigo,ttipe_descripcion FROM ttipoperfil WHERE ttipe_creacion='C'");
			}
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

		protected void btnCrear_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.CrearPerfil");
		}

		protected void btnModificar_Click(object sender, System.EventArgs e)
		{
			if(ddlModPerfil.Items.Count!=0)
				Response.Redirect(indexPage+"?process=Tools.ModPerfil&pfl="+ddlModPerfil.SelectedValue+"");
			else
            Utils.MostrarAlerta(Response, "No hay perfiles que modificar");
		}

		protected void btnEliminar_Click(object sender, System.EventArgs e)
		{
			if(ddlEliPerfil.Items.Count!=0)
			{
				
			}
			else
            Utils.MostrarAlerta(Response, "No hay perfiles que eliminar'");
		}
	}
}
