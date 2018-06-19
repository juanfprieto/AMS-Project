namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using Ajax;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillaDoble.
	/// </summary>
	public class AMS_Comercial_PlanillaDoble : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtRutaP;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtRuta1;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtRuta2;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtRutaInt1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox txtRutaInt2;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label lblError;
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_PlanillaDoble));	
		}
		[Ajax.AjaxMethod]
		public DataSet TraerRuta(string ruta)
		{
			DataSet dsRuta=new DataSet();
			DBFunctions.Request(dsRuta,IncludeSchema.NO,"SELECT MRUT_CODIGO1,MRUT_CODIGO2,CODIGO_INTERNO1,CODIGO_INTERNO2 FROM DBXSCHEMA.MRUTAS_DOBLE_PLANILLA WHERE MRUT_CODIGO='"+ruta+"'");
			return dsRuta;
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
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			string rutaP=txtRutaP.Text,rutaI1=txtRuta1.Text,rutaI2=txtRuta2.Text;
			//VAlidaciones
			if(!DBFunctions.RecordExist("SELECT MRUT_CODIGO FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+rutaP+"' AND MRUT_CLASE=2"))
			{
                Utils.MostrarAlerta(Response, "No existe la ruta principal.");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT MRUTA_SECUNDARIA FROM DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL='"+rutaP+"' AND MRUTA_SECUNDARIA='"+rutaI1+"'"))
			{
                Utils.MostrarAlerta(Response, "La primera ruta secundaria no hace parte del recorrido de la ruta principal.");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT MRUTA_SECUNDARIA FROM DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL='"+rutaP+"' AND MRUTA_SECUNDARIA='"+rutaI2+"'"))
			{
                Utils.MostrarAlerta(Response, "La segunda ruta secundaria no hace parte del recorrido de la ruta principal.");
				return;
			}
			//Insertar o actualizar registro
			if(DBFunctions.RecordExist("SELECT MRUT_CODIGO FROM DBXSCHEMA.MRUTAS_DOBLE_PLANILLA WHERE MRUT_CODIGO='"+rutaP+"'"))
				DBFunctions.NonQuery("UPDATE DBXSCHEMA.MRUTAS_DOBLE_PLANILLA SET MRUT_CODIGO1='"+rutaI1+"', MRUT_CODIGO2='"+rutaI2+"', CODIGO_INTERNO1='"+txtRutaInt1.Text+"', CODIGO_INTERNO2='"+txtRutaInt2.Text+"' WHERE MRUT_CODIGO='"+rutaP+"';");
			else
				DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MRUTAS_DOBLE_PLANILLA VALUES('"+rutaP+"','"+rutaI1+"','"+rutaI2+"','"+txtRutaInt1.Text+"','"+txtRutaInt2.Text+"',DEFAULT);");
            Utils.MostrarAlerta(Response, "La información ha sido actualizada.");
			return;
		}
	}
}
