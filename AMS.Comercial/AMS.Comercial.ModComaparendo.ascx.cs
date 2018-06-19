namespace AMS.Comercial
{
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
	using System;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Tools;
	using Ajax;
	//Realizado por:
	//Ing.German Lopera
	//2007-04-30

	/// <summary>
	///		Descripción breve de AMS_Comercial_ModComaparendo.
	/// </summary>
	public class AMS_Comercial_ModComaparendo : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList conductor;
		protected System.Web.UI.WebControls.DropDownList comparendo;
		protected System.Web.UI.WebControls.Button Buscar;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList estado;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox observaciones;
		protected System.Web.UI.WebControls.Label fechalabel;
		protected System.Web.UI.WebControls.Label comparendolabel;
		protected System.Web.UI.WebControls.Label infraccionlabel;
		protected System.Web.UI.WebControls.Button Actualizar;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label bus;
		protected System.Web.UI.WebControls.Label nombrelabel;
		protected System.Web.UI.WebControls.Label RemeasLabel;
		protected DataSet lineas;

		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!Page.IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(conductor,"select DISTINCT(MCOM_CONDUCTOR) FROM DBXSCHEMA.MCOMPARENDOS");
							
				
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
			this.conductor.SelectedIndexChanged += new System.EventHandler(this.conductor_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		
		private void conductor_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(comparendo,"SELECT MCOM_NUMCOM  FROM DBXSCHEMA.MCOMPARENDOS WHERE MCOM_CONDUCTOR='"+conductor.SelectedValue.ToString()+"' ");
		}

		public void Buscar_Click(object sender, System.EventArgs e)
		{
			Panel1.Visible=true;
			DatasToControls bind = new DatasToControls();
			lineas = new DataSet();
			DBFunctions.Request(lineas,IncludeSchema.NO,"select * FROM DBXSCHEMA.MCOMPARENDOS WHERE MCOM_NUMCOM='"+comparendo.SelectedValue.ToString()+"' AND MCOM_CONDUCTOR='"+conductor.SelectedValue.ToString()+"' ");
			for(int i=0;i<lineas.Tables[0].Rows.Count;i++)
			{
			string nombre=DBFunctions.SingleData("SELECT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') from DBXSCHEMA.MNIT MNIT WHERE MNIT.MNIT_NIT='"+conductor.SelectedValue.ToString()+"' ");
			fechalabel.Text=lineas.Tables[0].Rows[i].ItemArray[3].ToString();
			comparendolabel.Text=lineas.Tables[0].Rows[i].ItemArray[4].ToString();
			string infraccion=DBFunctions.SingleData("Select DESC_COMPARENDO from DBXSCHEMA.TCOMPARENDO WHERE COD_COMPARENDO="+lineas.Tables[0].Rows[i].ItemArray[5].ToString()+" ");
			bus.Text=lineas.Tables[0].Rows[i].ItemArray[7].ToString();
			bind.PutDatasIntoDropDownList(estado,"Select DESC_ESTACOMPA from DBXSCHEMA.TESTADO_COMPARENDO");
			observaciones.Text=lineas.Tables[0].Rows[i].ItemArray[12].ToString();
			nombrelabel.Text=nombre.ToString();
			infraccionlabel.Text=infraccion.ToString();
            			
			}


		}

		public void Actualizar_Click(object sender, System.EventArgs e)
		{
			string codigoestado=DBFunctions.SingleData("select COD_ESTACOMPA from DBXSCHEMA.TESTADO_COMPARENDO WHERE DESC_ESTACOMPA='"+estado.SelectedValue.ToString()+"' ");
			int cod_comparendo=Convert.ToInt32(DBFunctions.SingleData("Select MCOM_CODIGO from DBXSCHEMA.MCOMPARENDOS WHERE MCOM_CONDUCTOR='"+conductor.SelectedValue.ToString()+"' and MCOM_NUMCOM='"+comparendo.SelectedValue.ToString()+"' "));
			DBFunctions.NonQuery("UPDATE DBXSCHEMA.MCOMPARENDOS SET MCOM_ESTADO="+codigoestado+",MCOM_OBSERVACIONES='"+observaciones.Text.ToString()+"' WHERE MCOM_CONDUCTOR='"+conductor.SelectedValue.ToString()+"' AND MCOM_NUMCOM='"+comparendo.SelectedValue.ToString()+"' AND  MCOM_CODIGO="+cod_comparendo+" ");
			//Label6.Text=DBFunctions.exceptions;  //label para mostrar las excepciones al momento de insertar o modificar la base de datos.
            Utils.MostrarAlerta(Response, "  Comparendo Actualizado");
		}
	}
}
