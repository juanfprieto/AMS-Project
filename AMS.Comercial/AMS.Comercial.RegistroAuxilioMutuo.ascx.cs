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
	using AMS.DBManager;

	/// <summary>
	///		Descripción breve de AMS_Comercial_RegistroAuxilioMutuo.
	/// </summary>
	public class AMS_Comercial_RegistroAuxilioMutuo : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox txtNumActa;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtNumSiniestro;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtResponsable;
		protected System.Web.UI.WebControls.TextBox txtResponsablea;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtValorSiniestro;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtValorAuxilio;
		protected System.Web.UI.WebControls.TextBox txtFechaSiniestro;
		protected System.Web.UI.WebControls.TextBox txtFechaActa;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label lblError;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
				if(Request.QueryString["act"]!=null)
					Response.Write("<script language='javascript'>alert('El auxilio ha sido registrado.');</script>");
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
			//Validaciones
			int numActa,numSiniestro;
			string placa=txtPlaca.Text,responsable=txtResponsable.Text;
			double valSiniestro,valAuxilio;
			DateTime fechaSiniestro,fechaActa;
			//Numeros
			try{
				numActa=Convert.ToInt32(txtNumActa.Text);}
			catch{
				Response.Write("<script language='javascript'>alert('Número de acta no válido.');</script>");
				return;}
			try{
				numSiniestro=Convert.ToInt32(txtNumSiniestro.Text);}
			catch{
				Response.Write("<script language='javascript'>alert('Número de siniestro no válido.');</script>");
				return;}
			if(DBFunctions.RecordExist("Select MACT_NUMERO_ACTA from DBXSCHEMA.MACTA_AUXILIO_MUTUO WHERE MACT_NUMERO_ACTA="+numActa+";")){
				Response.Write("<script language='javascript'>alert('El número de acta ya existe.');</script>");
				return;}
			if(DBFunctions.RecordExist("Select NUMERO_SINIESTRO from DBXSCHEMA.MACTA_AUXILIO_MUTUO WHERE NUMERO_SINIESTRO="+numSiniestro+";")){
				Response.Write("<script language='javascript'>alert('El número de siniestro ya existe.');</script>");
				return;}
			//Placa
			if(!DBFunctions.RecordExist("Select MCAT_PLACA from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"' AND TESTA_CODIGO>0;")){
				Response.Write("<script language='javascript'>alert('Número de placa no válida.');</script>");
				return;}
			//Responsable
			if(!DBFunctions.RecordExist("Select MNIT_NIT from DBXSCHEMA.MNIT WHERE MNIT_NIT='"+responsable+"';")){
				Response.Write("<script language='javascript'>alert('No existe el responsable.');</script>");
				return;}
			//Valores
			try{
				valSiniestro=Convert.ToDouble(txtValorSiniestro.Text);}
			catch
			{
				Response.Write("<script language='javascript'>alert('Valor de siniestro no válido.');</script>");
				return;}
			try
			{
				valAuxilio=Convert.ToDouble(txtValorAuxilio.Text);}
			catch
			{
				Response.Write("<script language='javascript'>alert('Valor de acta no válido.');</script>");
				return;}
			if(valAuxilio>valSiniestro){
				Response.Write("<script language='javascript'>alert('El valor del auxilio no puede superar el valor del siniestro.');</script>");
				return;}
			//Fecha
			try{
				fechaSiniestro=Convert.ToDateTime(txtFechaSiniestro.Text);}
			catch{
				Response.Write("<script language='javascript'>alert('Fecha de siniestro no válida.');</script>");
				return;}
			try{
				fechaActa=Convert.ToDateTime(txtFechaActa.Text);}
			catch{
				Response.Write("<script language='javascript'>alert('Fecha de acta no válida.');</script>");
				return;}
			if(fechaActa<fechaSiniestro){
				Response.Write("<script language='javascript'>alert('La fecha del siniestro no puede ser posterior a la fecha del acta.');</script>");
				return;}

			//Insetar Acta
			ArrayList sqlUpd=new ArrayList();
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MACTA_AUXILIO_MUTUO VALUES("+numActa+","+numSiniestro+",'"+placa+"','"+responsable+"',"+valSiniestro+","+valAuxilio+",'"+fechaSiniestro.ToString("yyyy-MM-dd")+"','"+fechaActa.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"');");
			sqlUpd.Add("call DBXSCHEMA.AUXILIO_MUTUO("+numActa+","+valAuxilio+","+valSiniestro+",'"+fechaSiniestro.ToString("yyyy-MM-dd")+"');");
			if(DBFunctions.Transaction(sqlUpd)){
				Response.Redirect(indexPage+"?process=Comercial.RegistroAuxilioMutuo&act=1&path=Registro Auxilio Mutuo");
			}
			else{
				lblError.Text=DBFunctions.exceptions;
			}
			

		}
	}
}
