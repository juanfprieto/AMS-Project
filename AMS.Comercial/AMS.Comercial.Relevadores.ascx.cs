namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Microsoft.Web.UI.WebControls;
	using AMS.DB;
	using AMS.Forms;
	using Ajax;
	using AMS.DBManager;

	/// <summary>
	///		Descripción breve de AMS_Relevadores.
	/// </summary>
	public class AMS_Relevadores : System.Web.UI.UserControl
	{
		#region COnrtroles
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtEmpleado;
		protected System.Web.UI.WebControls.TextBox txtEmpleadoa;
		protected System.Web.UI.WebControls.TextBox txtFechaDesde;
		protected System.Web.UI.WebControls.Button btnAgregar;
		protected System.Web.UI.WebControls.ListBox lstRelevadores;
		protected System.Web.UI.WebControls.Button btnEliminar;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtFechaHasta;
		protected System.Web.UI.WebControls.Label lblError;
		#endregion COnrtroles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				CargarRelevadores();
			}
		}
		//Cargar lista de empleados
		private void CargarRelevadores()
		{
			DataSet dsEmpleados=new DataSet();
			DBFunctions.Request(dsEmpleados,IncludeSchema.NO,
				"SELECT MNIT.MNIT_NIT AS valor, MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') concat ' (' concat MNIT.MNIT_NIT concat ')   ' concat char(fecha_desde) concat' a ' concat char(fecha_hasta) AS texto "+
				"from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MRELEVADORES_TRANSPORTES MR "+
				"WHERE MR.MNIT_NIT=MNIT.MNIT_NIT "+
				"ORDER BY texto;");
			lstRelevadores.DataSource=dsEmpleados.Tables[0];
			lstRelevadores.DataTextField="texto";
			lstRelevadores.DataValueField="valor";
			lstRelevadores.DataBind();
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
			this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
			this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnAgregar_Click(object sender, System.EventArgs e)
		{
			//Validar NIT
			string empleado=txtEmpleado.Text.Trim();
			if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+empleado+"';")){
				Response.Write("<script language:javascript>alert('El relevador que ingresó no existe.');</script>");
				return;
			}
			//Validar Fechas
			DateTime fechaDesde,fechaHasta;
			try{
				fechaDesde=Convert.ToDateTime(txtFechaDesde.Text);
				fechaHasta=Convert.ToDateTime(txtFechaHasta.Text);
			}
			catch{
				Response.Write("<script language:javascript>alert('Debe ingresar fechas válidas.');</script>");
				return;
			}
			if(fechaDesde>fechaHasta){
				Response.Write("<script language:javascript>alert('La fecha final debe ser posterior a la fecha inicial.');</script>");
				return;
			}
			//Eliminar registros
			ArrayList sqlUpd=new ArrayList();
			sqlUpd.Add("DELETE FROM DBXSCHEMA.MRELEVADORES_TRANSPORTES WHERE MNIT_NIT='"+empleado+"';");
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MRELEVADORES_TRANSPORTES VALUES('"+empleado+"','"+fechaDesde.ToString("yyyy-MM-dd")+"','"+fechaHasta.ToString("yyyy-MM-dd")+"');");
			if(DBFunctions.Transaction(sqlUpd))
				CargarRelevadores();
			else
				lblError.Text=DBFunctions.exceptions;
		}

		private void btnEliminar_Click(object sender, System.EventArgs e)
		{
			if(lstRelevadores.SelectedIndex<0)
			{
				Response.Write("<script language='javascript'>alert('Debe seleccionar un relevador de la lista.');</script>");
				return;
			}
			DBFunctions.NonQuery("DELETE FROM DBXSCHEMA.MRELEVADORES_TRANSPORTES WHERE MNIT_NIT='"+lstRelevadores.SelectedValue+"';");
			CargarRelevadores();
		}
	}
}
