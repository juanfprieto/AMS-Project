namespace AMS.Produccion
{
	using System;
	using System.IO;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Services;
	using System.Web.SessionState;
	using System.Web.Services.Protocols;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Ajax;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Documentos;
	using AMS.Inventarios;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Produccion_ControlCalidadFinal.
	/// </summary>
	public partial class AMS_Produccion_ControlCalidadFinal : System.Web.UI.UserControl
	{
		#region Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.DataGrid dgrGastos;
		protected System.Web.UI.WebControls.PlaceHolder plcEjecutar;
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{
				if(Request.QueryString["upd"]!=null)
                Utils.MostrarAlerta(Response, "El vehículo ha sido actualizado.");
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

		}
		#endregion

		//Seleccionar
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlStrings=new ArrayList();
			string motor=txtMotor.Text.Trim();
			if(txtVIN.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar el VIN.");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT MCAT_VIN FROM MVEHICULO WHERE TEST_TIPOESTA=1 AND MCAT_VIN='"+txtVIN.Text+"';"))
			{
                Utils.MostrarAlerta(Response, "El estado del vehículo no es válido.");
				return;
			}
			if(motor.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe ingresar el numero de motor.");
				return;
			}
			if(DBFunctions.RecordExist("SELECT MCAT_MOTOR FROM MCATALOGOVEHICULO WHERE MCAT_MOTOR='"+motor+"';"))
			{
                Utils.MostrarAlerta(Response, "El número de motor ya se utilizó.");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT MCAT_MOTOR FROM DEMBARQUECKD WHERE MCAT_MOTOR='"+motor+"';"))
			{
                Utils.MostrarAlerta(Response, "El motor no ha sido registrado en ningún embarque.");
				return;
			}
			if(!DBFunctions.RecordExist(
				"SELECT DM.PCAT_CODIGO FROM DEMBARQUECKD DM, MCATALOGOVEHICULO MC "+
				"WHERE DM.MCAT_MOTOR='"+motor+"' AND DM.PCAT_CODIGO=MC.PCAT_CODIGO AND "+
				"MC.MCAT_VIN='"+txtVIN.Text+"';"))
			{
                Utils.MostrarAlerta(Response, "El catálogo no coincide con el motor.");
				return;
			}
			sqlStrings.Add("UPDATE MCATALOGOVEHICULO SET MCAT_MOTOR='"+motor+"' WHERE MCAT_VIN='"+txtVIN.Text+"';");
			sqlStrings.Add("UPDATE MVEHICULO SET TEST_TIPOESTA=3 WHERE MCAT_VIN='"+txtVIN.Text+"';");

			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(""+indexPage+"?process=Produccion.ControlCalidadFinal&path="+Request.QueryString["path"]+"&upd=1");
			else
				lblInfo.Text += "<br>Error : Detalles <br>"+DBFunctions.exceptions;
		}
	}
}
