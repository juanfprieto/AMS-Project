namespace AMS.Comercial
{
	using System;
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Globalization;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.Mail;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
	using AMS.DB;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ModProcesosJuridicos.
	/// </summary>
	public class AMS_Comercial_ModProcesosJuridicos : System.Web.UI.UserControl
	{
		#region COntroles
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList procesos;
		protected System.Web.UI.WebControls.Button cargar;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label fechaA;
		protected System.Web.UI.WebControls.Label numproI;
		protected System.Web.UI.WebControls.Label placa;
		protected System.Web.UI.WebControls.Label demandado;
		protected System.Web.UI.WebControls.Label demandante;
		protected System.Web.UI.WebControls.Label docdemandante;
		protected System.Web.UI.WebControls.TextBox asunto;
		protected System.Web.UI.WebControls.TextBox nomprocJ;
		protected System.Web.UI.WebControls.TextBox juzgado;
		protected System.Web.UI.WebControls.TextBox pretenciones;
		protected System.Web.UI.WebControls.TextBox observaciones;
		protected System.Web.UI.WebControls.TextBox actividades;
		protected System.Web.UI.WebControls.TextBox obsult;
		protected System.Web.UI.WebControls.Button Actualizar;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.DropDownList estado;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox Año;
		protected System.Web.UI.WebControls.TextBox Mes;
		protected System.Web.UI.WebControls.TextBox DiaF;
		protected System.Web.UI.WebControls.RangeValidator RangeValidator1;
		protected System.Web.UI.WebControls.RangeValidator RangeValidator2;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.TextBox fechaprox;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.TextBox ubicacion;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		#endregion COntroles

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(estado,"select TEST_DESCRIPCION from DBXSCHEMA.TESTADO");
				bind.PutDatasIntoDropDownList(procesos,"select MPRO_NUMEROI from DBXSCHEMA.MPROJURIDICO ORDER BY MPRO_NUMEROI");
				
			}
		}
		public void Cargar_Click(object sender, System.EventArgs e)
		{
			Panel1.Visible=true;
			DatasToControls bind = new DatasToControls();
			string fechacreacion=DBFunctions.SingleData("select MPRO_FECHA from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			fechaA.Text=Convert.ToDateTime(fechacreacion).ToString("yyyy-MM-dd");
			numproI.Text=procesos.SelectedValue.ToString();
			placa.Text=DBFunctions.SingleData("select MPRO_PLACA from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			string nit=DBFunctions.SingleData("select MPRO_DEMANDADO from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			demandado.Text=DBFunctions.SingleData("select MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 from DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nit+"' ");
			demandante.Text=DBFunctions.SingleData("select MPRO_DEMANDANTE from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			docdemandante.Text=DBFunctions.SingleData("select MPRO_DOCDEMANDANTE from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			asunto.Text=DBFunctions.SingleData("select MPRO_ASUNTO from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			juzgado.Text=DBFunctions.SingleData("select MPRO_JUZGADO from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			nomprocJ.Text=DBFunctions.SingleData("select MPRO_NUMPRO from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			pretenciones.Text=DBFunctions.SingleData("select MPRO_PRETENCIONES from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			observaciones.Text=DBFunctions.SingleData("select MPRO_OBSERVACIONES from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			actividades.Text=DBFunctions.SingleData("select MPRO_ACTIVIDADES from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			int año=Convert.ToInt32(DateTime.Now.Year);
			int mes=Convert.ToInt32(DateTime.Now.Month);
			int dia=Convert.ToInt32(DateTime.Now.Day);
			obsult.Text=DBFunctions.SingleData("select MPRO_ULTIMAACTUA from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			Año.Text=año.ToString();
			Mes.Text=mes.ToString();
			DiaF.Text=dia.ToString();
			string proxfecha=DBFunctions.SingleData("select MPRO_PROXIDIL from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			fechaprox.Text=Convert.ToDateTime(proxfecha).ToString("yyyy-MM-dd");
			Label22.Visible=true;
			ubicacion.Text=DBFunctions.SingleData("select MPRO_UBICACIONFISICA from DBXSCHEMA.MPROJURIDICO WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");

		}
		public void Actualizacion_Click(object sender, System.EventArgs e)
		{
			string fecha2= Año.Text.ToString() + "-" + Mes.Text.ToString() + "-" + DiaF.Text.ToString(); 
			int codigoestado=Convert.ToInt32(DBFunctions.SingleData("select TEST_ESTADO from DBXSCHEMA.TESTADO WHERE TEST_DESCRIPCION='"+estado.SelectedValue.ToString()+"' "));
			DBFunctions.NonQuery("UPDATE DBXSCHEMA.MPROJURIDICO SET MPRO_ASUNTO='"+asunto.Text.ToString()+"',MPRO_JUZGADO='"+juzgado.Text.ToString()+"',MPRO_NUMPRO='"+nomprocJ.Text.ToString()+"',MPRO_PRETENCIONES='"+pretenciones.Text.ToString()+"',MPRO_OBSERVACIONES='"+observaciones.Text.ToString()+"',MPRO_ACTIVIDADES='"+actividades.Text.ToString()+"',MPRO_FECHAULTIMA='"+fecha2.ToString()+"',MPRO_ULTIMAACTUA='"+obsult.Text.ToString()+"',MPRO_ESTADO="+codigoestado+",MPRO_PROXIDIL='"+fechaprox.Text.ToString()+"',MPRO_UBICACIONFISICA='"+ubicacion.Text.ToString()+"' WHERE MPRO_NUMEROI="+procesos.SelectedValue.ToString()+" ");
			Label17.Text=DBFunctions.exceptions;  //label para mostrar las excepciones al momento de insertar o modificar la base de datos.
            Utils.MostrarAlerta(Response, "  Proceso Actualizado");
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
