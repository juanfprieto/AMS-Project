namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;

	/// <summary>
	///		Descripción breve de AMS_Comercial_Reservas.
	/// </summary>
	public class AMS_Comercial_Reservas : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label resulBusq;
		protected System.Web.UI.WebControls.ListBox rutasDisponibles;
		protected System.Web.UI.WebControls.CheckBox parFecha;
		protected System.Web.UI.WebControls.Calendar fechaRuta;
		protected System.Web.UI.WebControls.RadioButtonList especificar;
		protected System.Web.UI.WebControls.DropDownList ciudadOrigen;
		protected System.Web.UI.WebControls.CheckBox parOri;
		protected System.Web.UI.WebControls.DropDownList ciudadDestino;
		protected System.Web.UI.WebControls.CheckBox parDes;
		protected System.Web.UI.WebControls.Button buscar;
		protected System.Web.UI.WebControls.Label lb;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();	
				bind.PutDatasIntoDropDownList(ciudadOrigen,"select tciu_codigo,tciu_nombre from dbxschema.tciudad order by tciu_nombre");
				bind.PutDatasIntoDropDownList(ciudadDestino,"select tciu_codigo,tciu_nombre from dbxschema.tciudad order by tciu_nombre");
				fechaRuta.SelectedDate=DateTime.Now;
				//fechaRuta.SelectedDate = DateTime.Now;
				//string fechoract = DateTime.Now.Date.ToString("yyyy-MM-dd")+" "+DateTime.Now.TimeOfDay.ToString().Substring(0,8);
				//bind.PutDatasIntoDropDownList(DDLDepartamento,"Select PDEP_NOMBRE From DBXSCHEMA.PDEPARTAMENTO");
				//bind.PutDatasIntoListBox(rutasDisponibles,"SELECT DRUT.drut_codigo, PCIU1.pciu_nombre CONCAT '-' CONCAT PCIU2.pciu_nombre CONCAT '  ' CONCAT CAST(DRUT.drut_fechhorsal AS char(30)) FROM dbxschema.druta DRUT, dbxschema.mruta MRUT, dbxschema.pciudad PCIU1, dbxschema.pciudad PCIU2 WHERE DRUT.mrut_codigo = MRUT.mrut_codigo AND MRUT.pciu_codiori = PCIU1.pciu_codigo AND MRUT.pciu_codidest = PCIU2.pciu_codigo AND DRUT.drut_fechhorsal>='"+fechoract+"'");
				//bind.PutDatasIntoListBox(rutasDisponibles,"SELECT DRUT.drut_codigo, MRUT.mrut_descripcion FROM dbxschema.druta DRUT, dbxschema.mruta MRUT, dbxschema.pciudad PCIU1, dbxschema.pciudad PCIU2 WHERE DRUT.mrut_codigo = MRUT.mrut_codigo AND MRUT.pciu_codiori = PCIU1.pciu_codigo AND MRUT.pciu_codidest = PCIU2.pciu_codigo AND DRUT.drut_fechhorsal>='"+fechoract+"'");
				//bind.PutDatasIntoDropDownList(ciudadOrigen,"SELECT pciu_codigo, pciu_nombre FROM pciudad");		
				//bind.PutDatasIntoDropDownList(ciudadDestino,"SELECT pciu_codigo, pciu_nombre FROM pciudad");
				
				
			}
		}
		protected void Realizar_Busqueda(Object  Sender, EventArgs e)
		{
			//primero creamos el encabezado de este select y luego lo vamos a ir llenado deacuerdo con los requerimientos del cliente
			string select="";
			select = "SELECT distinct DRUT.drut_codigo, tciu1.tciu_nombre CONCAT '-' CONCAT tciu2.tciu_nombre CONCAT '  ' CONCAT cast(DRUT.drut_fecha AS char(10)) concat' 'concat drut.drut_horasal FROM dbxschema.mruta MRUT,dbxschema.ttiporuta ttipo, dbxschema.druta DRUT, dbxschema.tciudad tciu1, dbxschema.tciudad tciu2 WHERE DRUT.mrut_codigo = MRUT.mrut_codigo AND MRUT.tciu_cod = tciu1.tciu_codigo AND MRUT.tciu_coddes = tciu2.tciu_codigo ";
			if(parFecha.Checked)
			{
				string fechaBusqueda = fechaRuta.SelectedDate.Date.ToString("yyyy-MM-dd")+" "+DateTime.Now.TimeOfDay.ToString().Substring(0,8);
				select += " AND DRUT.drut_fecha"+especificar.SelectedValue.ToString()+"'"+fechaBusqueda+"'";
			}
			if(parOri.Checked)
				select += " AND MRUT.tciu_cod='"+ciudadOrigen.SelectedValue.ToString()+"'";
			if(parDes.Checked)
				select += " AND MRUT.tciu_coddes='"+ciudadDestino.SelectedValue.ToString()+"'  ";
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoListBox(rutasDisponibles,select);
			resulBusq.Text = "Se han encontrado "+rutasDisponibles.Items.Count.ToString()+" registros que coinciden";
			if(rutasDisponibles.Items.Count==0)
			{
				rutasDisponibles.Visible=false;
			}
			else
				rutasDisponibles.Visible=true;
		
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
