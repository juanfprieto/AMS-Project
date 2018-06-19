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
	///		Descripción breve de AMS_Tools_CrearLlamada.
	/// </summary>
	public partial class CrearLlamada : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationSettings.AppSettings["MainIndexPage"];

		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlaccion,"SELECT * FROM taccionllamada");
				tbfecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				tbhora.Text=DateTime.Now.TimeOfDay.ToString().Substring(0,8);
				lbEmpresa.Text=DBFunctions.SingleData("SELECT cemp_nombre FROM cempresa");
			}
		}

		protected void btnGuardar_Click(object Sender,EventArgs e)
		{
			if(Convert.ToDateTime(tbfecha.Text)>DateTime.Now.Date)
                Utils.MostrarAlerta(Response, "La fecha proporcionada es mayor a la de hoy");
			else if(!DatasToControls.ValidarDateTime(tbhora.Text))
                Utils.MostrarAlerta(Response, "Hora Invalida");
			else if(Convert.ToDateTime(tbhora.Text)>Convert.ToDateTime(DateTime.Now.TimeOfDay.ToString().Substring(0,8)))
                Utils.MostrarAlerta(Response, "La hora es mayor a la hora actual");
			else
			{
				int num=Convert.ToInt32(DBFunctions.SingleData("SELECT CASE WHEN MAX(MLLA_ID) IS NULL THEN 0 ELSE MAX(MLLA_ID) END FROM DBXSCHEMA.MLLAMADA WHERE MEMP_CODIEMPL='"+Request.QueryString["ase"]+"'"))+1;
				if(DBFunctions.NonQuery("INSERT INTO mllamada VALUES("+num+",'"+Request.QueryString["ase"]+"','"+DBFunctions.SingleData("SELECT memp_codiempl FROM mempleadosusuario WHERE susu_codigo=(SELECT susu_codigo FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"')")+"','"+ddlaccion.SelectedValue+"','C','"+tbfecha.Text+"','"+tbhora.Text+"','"+tbpersona.Text+"','"+tbmensaje.Text+"',null)")==1)
				//														id				Para:																					Persona q recibio																																accion a tomar		estado		fecha				hora			quien llamo			mensaje			comentario
					Response.Redirect(indexPage+"?process=Tools.AdministrarLlamadasPersonales&msg=1");
				else
					lb.Text="Error "+DBFunctions.exceptions;
			}
		}

		protected void btnCancelar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.AdministrarLlamadasPersonales");
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
