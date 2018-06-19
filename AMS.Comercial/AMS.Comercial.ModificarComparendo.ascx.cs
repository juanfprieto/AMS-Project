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
	using System.Configuration;
	using System.Collections;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ModificarComparendo.
	/// </summary>
	public class AMS_Comercial_ModificarComparendo : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox txtNumero;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlInfraccion;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtInfractor;
		protected System.Web.UI.WebControls.TextBox txtInfractora;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.TextBox txtDireccion;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtFechaComparendo;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtRadicacion;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtFechaPago;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtValor;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlAutoridad;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.DropDownList ddlEstado;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox txtObservacion;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Panel pnlDetralle;
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		protected System.Web.UI.WebControls.Panel pnlDetalle;
		protected System.Web.UI.WebControls.Label lblError;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlInfraccion,"SELECT PINF_CODIGO,DESCRIPCION FROM DBXSCHEMA.PINFRACCIONES_TRANSITO ORDER BY DESCRIPCION;");
				bind.PutDatasIntoDropDownList(ddlAutoridad,"SELECT PAUT_CODIGO,DESCRIPCION FROM DBXSCHEMA.PAUTORIDADES_TRANSPORTE ORDER BY DESCRIPCION;");
				bind.PutDatasIntoDropDownList(ddlEstado,"SELECT PEST_CODIGO,DESCRIPCION FROM DBXSCHEMA.PESTADOS_COMPARENDO ORDER BY DESCRIPCION;");
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El comparendo ha sido modificado.");
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
			this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			if(!DBFunctions.RecordExist("SELECT MCOM_NUMERO from DBXSCHEMA.MCOMPARENDO WHERE MCOM_NUMERO="+txtNumero.Text+";")){
                Utils.MostrarAlerta(Response, "  No existe el comparendo.");
				return;
			}
			txtNumero.Enabled=false;
			btnSeleccionar.Visible=false;
			pnlDetalle.Visible=true;
			DataSet dsComparendo=new DataSet();
			DBFunctions.Request(dsComparendo,IncludeSchema.NO,"SELECT * FROM DBXSCHEMA.MCOMPARENDO WHERE MCOM_NUMERO="+txtNumero.Text+";");
			ddlInfraccion.SelectedIndex=ddlInfraccion.Items.IndexOf(ddlInfraccion.Items.FindByValue(dsComparendo.Tables[0].Rows[0]["PINF_CODIGO"].ToString()));
			txtInfractor.Text=dsComparendo.Tables[0].Rows[0]["MNIT_INFRACTOR"].ToString();
			txtInfractora.Text=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' 'CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+txtInfractor.Text.Trim()+"';");
			txtDireccion.Text=dsComparendo.Tables[0].Rows[0]["DIRECCION"].ToString();
			txtPlaca.Text=dsComparendo.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			txtFechaComparendo.Text=Convert.ToDateTime(dsComparendo.Tables[0].Rows[0]["FECHA_COMPARENDO"].ToString()).ToString("yyyy-MM-dd");
			txtRadicacion.Text=Convert.ToDateTime(dsComparendo.Tables[0].Rows[0]["FECHA_RADICACION"].ToString()).ToString("yyyy-MM-dd");
			txtFechaPago.Text=Convert.ToDateTime(dsComparendo.Tables[0].Rows[0]["FECHA_PAGO"].ToString()).ToString("yyyy-MM-dd");
			txtValor.Text=Convert.ToDouble(dsComparendo.Tables[0].Rows[0]["VALOR_PAGADO"]).ToString("#,###,##0.##");
			ddlAutoridad.SelectedIndex=ddlAutoridad.Items.IndexOf(ddlAutoridad.Items.FindByValue(dsComparendo.Tables[0].Rows[0]["PAUT_CODIGO"].ToString()));
			ddlEstado.SelectedIndex=ddlEstado.Items.IndexOf(ddlEstado.Items.FindByValue(dsComparendo.Tables[0].Rows[0]["PEST_CODIGO"].ToString()));
			txtObservacion.Text=dsComparendo.Tables[0].Rows[0]["OBSERVACION"].ToString();

		}

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			//Validar
			int numero;
			string nitInfractor=txtInfractor.Text.Trim();
			string direccion=txtDireccion.Text.Trim();
			string placa=txtPlaca.Text.Trim();
			string observacion=txtObservacion.Text.Trim();
			DateTime fechaComparendo, fechaRadicacion, fechaPago;
			double valorPagado=0;
			double valorInfraccion=0;
			string propietario="";
			#region validaciones
			try
			{
				numero=Convert.ToInt16(txtNumero.Text);
				if(numero<=0)throw(new Exception());
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  Número de comparendo invalido");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT * FROM DBXSCHEMA.MCOMPARENDO WHERE MCOM_NUMERO="+numero+";"))
			{
                Utils.MostrarAlerta(Response, "  No existe el comparendo numero " + numero + "");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nitInfractor+"';"))
			{
                Utils.MostrarAlerta(Response, "  El infractor no existe");
				return;
			}

			if(!DBFunctions.RecordExist("SELECT MCAT_PLACA FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';"))
			{
                Utils.MostrarAlerta(Response, "  El vehículo no existe");
				return;
			}
			propietario=DBFunctions.SingleData("Select MNIT_NIT from DBXSCHEMA.DBUSAFILIADO_PROPIETARIOS WHERE MCAT_PLACA='"+placa+"';");
			valorInfraccion=Convert.ToDouble(DBFunctions.SingleData("Select VALOR from DBXSCHEMA.PINFRACCIONES_TRANSITO WHERE PINF_CODIGO="+ddlInfraccion.SelectedValue+";"));
			try
			{
				valorPagado=Convert.ToDouble(txtValor.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  Valor invalido");
				return;
			}
			try
			{
				fechaPago=Convert.ToDateTime(txtFechaPago.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  Fecha de pago invalida");
				return;
			}
			try
			{
				fechaRadicacion=Convert.ToDateTime(txtRadicacion.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  Fecha de radicacion invalida");
				return;
			}
			try
			{
				fechaComparendo=Convert.ToDateTime(txtFechaComparendo.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "  Fecha de comparendo invalida");
				return;
			}
			#endregion validaciones

			ArrayList arSql=new ArrayList();
			int usuario=Convert.ToInt16(DBFunctions.SingleData("select susu_codigo from susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'"));
			arSql.Add("UPDATE DBXSCHEMA.MCOMPARENDO SET PINF_CODIGO="+ddlInfraccion.SelectedValue+",MNIT_INFRACTOR='"+nitInfractor+"',DIRECCION='"+direccion+"',MCAT_PLACA='"+placa+"',MNIT_PROPIETARIO='"+propietario+"',FECHA_COMPARENDO='"+fechaComparendo.ToString("yyyy-MM-dd")+"',FECHA_RADICACION='"+fechaRadicacion.ToString("yyyy-MM-dd")+"',VALOR_INFRACCION="+valorInfraccion+",VALOR_PAGADO="+valorPagado+",PAUT_CODIGO="+ddlAutoridad.SelectedValue+",OBSERVACION='"+observacion+"',FECHA_PAGO='"+fechaPago.ToString("yyyy-MM-dd")+"',PEST_CODIGO="+ddlEstado.SelectedValue+",FECHA_ESTADO='"+DateTime.Now.ToString("yyyy-MM-dd")+"',SUSU_CODIGO="+usuario+" WHERE MCOM_NUMERO="+numero+";");
			if(!DBFunctions.Transaction(arSql))
				lblError.Text=DBFunctions.exceptions;
			else
				Response.Redirect(indexPage+"?process=Comercial.ModificarComparendo&act=1&path="+Request.QueryString["path"]);
		}
	}
}
