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
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_PersonalAgencias.
	/// </summary>
	public class AMS_Comercial_PersonalAgencias : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlCargo;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Panel pnlEmpleados;
		protected System.Web.UI.WebControls.TextBox txtEmpleado;
		protected System.Web.UI.WebControls.TextBox txtEmpleadoa;
		protected System.Web.UI.WebControls.Button btnAgregar;
		protected System.Web.UI.WebControls.ListBox lstEmpleados;
		protected System.Web.UI.WebControls.Button btnEliminar;
		protected System.Web.UI.WebControls.Panel pnlCargo;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack){
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				Agencias.TraerCargosEmpleados(ddlCargo);
				ddlCargo.Items.Insert(0,new ListItem("---seleccione---",""));
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender, e);
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.ddlCargo.SelectedIndexChanged += new System.EventHandler(this.ddlCargo_SelectedIndexChanged);
			this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
			this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Cambia Agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			pnlEmpleados.Visible=false;
			pnlCargo.Visible=(agencia.Length>0);
			ddlCargo.SelectedIndex=ddlCargo.Items.IndexOf(ddlCargo.Items.FindByValue(""));
		}

		
		//Cambia cargo
		private void ddlCargo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string cargo=ddlCargo.SelectedValue;
			string agencia=ddlAgencia.SelectedValue;
			pnlEmpleados.Visible=(cargo.Length>0);
			lstEmpleados.Items.Clear();
			if(cargo.Length==0)return; 
			
			//Cargar empleados
			CargarEmpleados(agencia,cargo);
		}

		
		//Agregar Empleados
		private void btnAgregar_Click(object sender, System.EventArgs e)
		{
			string cargo=ddlCargo.SelectedValue;
			string agencia=ddlAgencia.SelectedValue;
			string empleado=txtEmpleado.Text.Trim();
			
			if(empleado.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar el nuevo empleado.");
				return;
			}

			if(DBFunctions.RecordExist(
				"SELECT * FROM DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES "+
				"WHERE MAG_CODIGO="+agencia+" AND MNIT_NIT='"+empleado+"' AND PCAR_CODIGO="+cargo+";")){
                Utils.MostrarAlerta(Response, "  Ya se agregó el usuario a la agencia con el cargo seleccionado.");
				return;
			}
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES VALUES("+agencia+",'"+empleado+"',"+cargo+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
			CargarEmpleados(agencia,cargo);
		}

		
		//Cargar lista de empleados
		private void CargarEmpleados(string agencia, string cargo)
		{
			DataSet dsEmpleados=new DataSet();
			DBFunctions.Request(dsEmpleados,IncludeSchema.NO,
				"SELECT MNIT.MNIT_NIT AS valor, MNIT.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') concat ' (' concat MNIT.MNIT_NIT concat ')' AS texto "+
				"from DBXSCHEMA.MNIT MNIT,DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP "+
				"WHERE MP.MAG_CODIGO="+agencia+" AND MP.MNIT_NIT=MNIT.MNIT_NIT AND MP.PCAR_CODIGO="+cargo+" "+
				"ORDER BY texto;");
			lstEmpleados.DataSource=dsEmpleados.Tables[0];
			lstEmpleados.DataTextField="texto";
			lstEmpleados.DataValueField="valor";
			lstEmpleados.DataBind();
		}

		
		//Eliminar empleado
		private void btnEliminar_Click(object sender, System.EventArgs e)
		{
			string cargo=ddlCargo.SelectedValue;
			string agencia=ddlAgencia.SelectedValue;

			if(lstEmpleados.SelectedIndex<0)
			{
                Utils.MostrarAlerta(Response, "  Debe seleccionar un empleado de la lista.");
				return;
			}

			DBFunctions.NonQuery("DELETE FROM DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES "+
				"WHERE MAG_CODIGO="+agencia+" AND MNIT_NIT='"+lstEmpleados.SelectedValue+"' AND PCAR_CODIGO="+cargo+";");
			CargarEmpleados(agencia,cargo);
		}
	}
}
