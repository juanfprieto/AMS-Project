namespace AMS.Nomina
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
	using Ajax;
    using AMS.Tools;
	/// <summary>
	///		Descripción breve de AMS_Nomina_ActualizarSueldos.
	/// </summary>
	public class AMS_Nomina_ActualizarSueldos : System.Web.UI.UserControl
	{
		protected Sueldos sueldos = new Sueldos();
			
		protected System.Web.UI.WebControls.TextBox txtSalNew, txtSubTraNew;
		protected System.Web.UI.WebControls.Button btnGuardarAll;
		protected System.Web.UI.WebControls.DropDownList ddlEmpleados;
        protected System.Web.UI.WebControls.Label lbSalminEmpl, lbSubsidioTra;
		protected System.Web.UI.WebControls.TextBox txtSalNewEmpl;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label lb;
		protected System.Web.UI.WebControls.Label lbSalMin;
        protected System.Web.UI.WebControls.TextBox fechaModificacion;
        protected System.Web.UI.WebControls.TextBox fechaModificacion2;


		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Nomina_ActualizarSueldos));
			if(!IsPostBack)
			{
				DatasToControls param = new DatasToControls();
				lbSalMin.Text      = DBFunctions.SingleData("Select cnom_salaminiactu from DBXSCHEMA.CNOMINA");
                lbSubsidioTra.Text = DBFunctions.SingleData("Select cnom_substransactu from DBXSCHEMA.CNOMINA");
                param.PutDatasIntoDropDownList(ddlEmpleados, "SELECT M.MEMP_CODIEMPL,M.MEMP_CODIEMPL CONCAT ' ' CONCAT N.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(N.MNIT_APELLIDO2,'') CONCAT ' ' CONCAT N.MNIT_NOMBRES CONCAT ' ' CONCAT coalesce(N.MNIT_NOMBRE2,'') FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado <>'4' order by M.MEMP_CODIEMPL");
				lbSalminEmpl.Text  = DBFunctions.SingleData("Select memp_suelactu from DBXSCHEMA.mempleado where MEMP_CODIEMPL='"+ddlEmpleados.SelectedValue+"'");				
			}
			else
				lbSalminEmpl.Text="";
		}

		protected void ActTodos(object sender,System.EventArgs e)
		{
            string FechaModificacion = fechaModificacion.Text;	
            Utils.MostrarAlerta(Response, "Se Actualizaran todos los empleados con sueldo MINIMO..");
			if (txtSalNew.Text!=string.Empty)
			{
                sueldos.ActSueldoTodos(double.Parse(txtSalNew.Text), double.Parse(lbSalMin.Text), double.Parse(txtSubTraNew.Text), double.Parse(lbSubsidioTra.Text), FechaModificacion);
			}
			else
			{
                Utils.MostrarAlerta(Response, "Por favor ingrese el nuevo salario..");
				lb.Text=sueldos.Mensajes;
			}
				
		}
		[Ajax.AjaxMethod] 
		public string cambioEmpleado(string codEmpleado)
		{
			//Response.Write("<script language:javascript> alert('cambio empleado..');</script>");			
			string a=DBFunctions.SingleData("Select memp_suelactu from DBXSCHEMA.mempleado where MEMP_CODIEMPL='"+codEmpleado+"'");				
			return a;
		}
		
		
		protected void ActEmpleado(object sender, System.EventArgs e)
		{
            string FechaModificacion = fechaModificacion2.Text;
			if (txtSalNewEmpl.Text!=string.Empty)
			{
				if (sueldos.ActSueldoEmp(ddlEmpleados.SelectedValue,double.Parse(txtSalNewEmpl.Text),FechaModificacion))
				{
                    Utils.MostrarAlerta(Response, "Se Actualizo correctamente el Sueldo..");
				}
				lbSalminEmpl.Text=DBFunctions.SingleData("Select memp_suelactu from DBXSCHEMA.mempleado where MEMP_CODIEMPL='"+ddlEmpleados.SelectedValue+"'");							
			}
			else
			{
				lb.Text=sueldos.Mensajes;
                Utils.MostrarAlerta(Response, "Porfavor ingrese el nuevo salario..");
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
