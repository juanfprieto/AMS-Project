namespace AMS.Automotriz
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
	using Ajax;

	/// <summary>
	///		Descripción breve de AMS_Automotriz_RelacionarDocumentos.
	/// </summary>
	public partial class AMS_Automotriz_RelacionarDocumentos : System.Web.UI.UserControl
	{
		string modeloV=null;
		string CodigoCatalogo=null;
		string nit=null;
		string propietario=null;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(VehiDrop,"Select MCAT_PLACA from DBXSCHEMA.MCATALOGOVEHICULO ORDER BY PCAT_CODIGO,MCAT_VIN,MCAT_MOTOR,MCAT_PLACA,MCAT_VIN");
				imglupa.Attributes.Add("OnClick","ModalDialog("+VehiDrop.ClientID+",'Select MCAT_PLACA as Placa from DBXSCHEMA.MCATALOGOVEHICULO ORDER BY PCAT_CODIGO,MCAT_VIN,MCAT_MOTOR,MCAT_PLACA,MCAT_VIN ' ,new Array() );");
			}
		}
		public void Generar_Click(object sender, System.EventArgs e)
		{
			Panel1.Visible=true;
			Label7.Visible=true;
			nit=DBFunctions.SingleData("select MNIT_NIT from DBXSCHEMA.MCATALOGOVEHICULO WHERE MCAT_PLACA ='"+VehiDrop.SelectedValue.ToString()+"' ");
			propietario=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nit+"'");
			PropLabel.Text=propietario.ToString();
			VehiLabel.Text=VehiDrop.SelectedValue.ToString();
			CodigoCatalogo=DBFunctions.SingleData("SELECT PCAT_CODIGO from DBXSCHEMA.MCATALOGOVEHICULO WHERE MCAT_PLACA='"+VehiDrop.SelectedValue.ToString()+"' ");
			modeloV=DBFunctions.SingleData("select PCAT_DESCRIPCION from DBXSCHEMA.PCATALOGOVEHICULO WHERE PCAT_CODIGO='"+CodigoCatalogo+"'");
			ModeloLabel.Text=modeloV.ToString();
		

		}

		public void Guardar_Click(object sender, System.EventArgs e)
		{
			nit=DBFunctions.SingleData("select MNIT_NIT from DBXSCHEMA.MCATALOGOVEHICULO WHERE MCAT_PLACA ='"+VehiDrop.SelectedValue.ToString()+"' ");
			string Soat=soat.Text.ToString();
			string Propiedad=propiedad.Text.ToString();
			string PolizaG=pgarantia.Text.ToString();
			string DecImportacion=dimportacion.Text.ToString();
			string Improntas=improntas.Text.ToString();
			string PolizaBat=pbateria.Text.ToString();
			string Factura=facturaV.Text.ToString();
			string InveEntrega=inventarioentrega.Text.ToString();
			
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MDOC_VEHICULOS VALUES(DEFAULT,'"+VehiDrop.SelectedValue.ToString()+"','"+Soat+"','"+Propiedad+"','"+PolizaG+"','"+DecImportacion+"','"+Improntas+"','"+PolizaBat+"','"+Factura+"','"+InveEntrega+"')");//query para insertar directo a la base de datos.
			//Label11.Text=DBFunctions.exceptions;  //labael para mostrar las excepciones al momento de insertar o modificar la base de datos.
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
