using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	/// <summary>
	///		Descripción breve de AMS_Tools_AdministrarLlamadasPersonales.
	/// </summary>
	public partial class AdministrarLlamadasPersonales : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationSettings.AppSettings["MainIndexPage"];

		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlasesor,"SELECT MEMP.memp_codiempl,MEMP.memp_codiempl CONCAT ' - ' CONCAT MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.mnit_apellido2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') FROM dbxschema.mnit MNIT,dbxschema.mempleado MEMP,dbxschema.mempleadosusuario MEMS WHERE MNIT.mnit_nit=MEMP.mnit_nit AND MEMP.memp_codiempl=MEMS.memp_codiempl AND MEMP.test_estado='1'");
				if(Request.QueryString["msg"]!=null)
                Utils.MostrarAlerta(Response, "Llamada guardada satisfactoriamente");
			}
		}

		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.CrearLlamada&ase="+ddlasesor.SelectedValue+"");
		}

		protected void btnConsultar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.ConsultarLlamadas");
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
