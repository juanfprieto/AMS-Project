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
	///		Descripción breve de AMS_Comercial_PagoGiros.
	/// </summary>
	public class AMS_Comercial_PagoGiros : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaD;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblValorGiro;
		protected System.Web.UI.WebControls.Button btnRegistrar;
		protected System.Web.UI.WebControls.DropDownList ddlDocumento;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaO;
		protected System.Web.UI.WebControls.Label lblDocEmisor;
		protected System.Web.UI.WebControls.Label lblNombreEmisor;
		protected System.Web.UI.WebControls.Label lblApellidoEmisor;
		protected System.Web.UI.WebControls.Label lblTelefonoEmisor;
		protected System.Web.UI.WebControls.Label lblDireccionEmisor;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblDocReceptor;
		protected System.Web.UI.WebControls.Label lblNombreReceptor;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblApellidoReceptor;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblTelefonoReceptor;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label lblDireccionReceptor;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Panel pnlPago;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblFechaR;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblFechaE;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblNumDocumento;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgenciaD);
				bind.PutDatasIntoDropDownList(ddlAgenciaO,
					"select ma.mag_codigo,ma.mage_nombre "+
					"from DBXSCHEMA.magencia ma order by ma.mage_nombre");
				CargarDocumentos();
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El giro ha sido pagado.");
			}
		}
		//Cargar documentos disponibles para las agencias seleccionadas
		private void CargarDocumentos()
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlDocumento,
				"select num_documento "+
				"from DBXSCHEMA.mgiros "+
				"where tipo_documento='V' and fecha_entrega is null and mag_agencia_origen="+ddlAgenciaO.SelectedValue+" and mag_agencia_destino="+ddlAgenciaD.SelectedValue+" order by num_documento");
			ddlDocumento.Items.Insert(0, new ListItem("---seleccione---",""));
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
			this.ddlAgenciaD.SelectedIndexChanged += new System.EventHandler(this.ddlAgenciaD_SelectedIndexChanged);
			this.ddlAgenciaO.SelectedIndexChanged += new System.EventHandler(this.ddlAgenciaO_SelectedIndexChanged);
			this.ddlDocumento.SelectedIndexChanged += new System.EventHandler(this.ddlDocumento_SelectedIndexChanged);
			this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlAgenciaD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarDocumentos();
		}

		private void ddlAgenciaO_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarDocumentos();
		}

		private void ddlDocumento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DataSet dsGiro=new DataSet();
			DataSet dsNits=new DataSet();
			string nitEmisor,nitReceptor;
			
			lblDireccionEmisor.Text=lblDocEmisor.Text=lblNombreEmisor.Text=lblApellidoEmisor.Text=lblTelefonoEmisor.Text="";
			lblDireccionReceptor.Text=lblDocReceptor.Text=lblNombreReceptor.Text=lblApellidoReceptor.Text=lblTelefonoReceptor.Text="";
			lblValorGiro.Text="";
			
			if(ddlDocumento.SelectedValue.Length==0){
				pnlPago.Visible=false;
				return;}
			pnlPago.Visible=true;
			//Cargar datos			
			DBFunctions.Request(dsGiro, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MGIROS WHERE NUM_DOCUMENTO="+ddlDocumento.SelectedValue+" and fecha_entrega is null");
			if(dsGiro.Tables[0].Rows.Count==0)return;
			nitEmisor=dsGiro.Tables[0].Rows[0]["MNIT_EMISOR"].ToString();
			nitReceptor=dsGiro.Tables[0].Rows[0]["MNIT_DESTINATARIO"].ToString();

			DBFunctions.Request(dsNits, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nitEmisor+"';"+
				"SELECT * FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nitReceptor+"';");

			lblDocEmisor.Text=nitEmisor;
			lblDireccionEmisor.Text=dsGiro.Tables[0].Rows[0]["MNIT_EMISOR_DIRECCION"].ToString();
			lblTelefonoEmisor.Text=dsGiro.Tables[0].Rows[0]["MNIT_EMISOR_TELEFONO"].ToString();
			lblDocReceptor.Text=nitReceptor;
			lblDireccionReceptor.Text=dsGiro.Tables[0].Rows[0]["MNIT_DESTINATARIO_DIRECCION"].ToString();
			lblTelefonoReceptor.Text=dsGiro.Tables[0].Rows[0]["MNIT_DESTINATARIO_TELEFONO"].ToString();
			if(dsNits.Tables[0].Rows.Count>0){
				lblNombreEmisor.Text=dsNits.Tables[0].Rows[0]["MNIT_NOMBRES"].ToString();
				lblApellidoEmisor.Text=dsNits.Tables[0].Rows[0]["MNIT_APELLIDOS"].ToString();
			}
			if(dsNits.Tables[1].Rows.Count>0){
				lblNombreReceptor.Text=dsNits.Tables[1].Rows[0]["MNIT_NOMBRES"].ToString();
				lblApellidoReceptor.Text=dsNits.Tables[1].Rows[0]["MNIT_APELLIDOS"].ToString();
			}
			double valorGiro=Convert.ToDouble(dsGiro.Tables[0].Rows[0]["VALOR_GIRO"]);
			lblValorGiro.Text=valorGiro.ToString("#,###");
			lblFechaE.Text=DateTime.Now.ToString("dd/MM/yyyy");
			lblFechaR.Text=(Convert.ToDateTime(dsGiro.Tables[0].Rows[0]["FECHA_RECIBE"])).ToString("dd/MM/yyyy");
			lblNumDocumento.Text=dsGiro.Tables[0].Rows[0]["NUM_DOCUMENTO"].ToString();
		}

		private void btnRegistrar_Click(object sender, System.EventArgs e)
		{
			if(ddlDocumento.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Número de documento no válido.");
				return;
			}
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0){
                Utils.MostrarAlerta(Response, "  El usuario actual (responsable) no tiene un NIT asociado.");
				return;
			}
			//Actualizar giro
			DBFunctions.NonQuery("UPDATE DBXSCHEMA.MGIROS SET FECHA_ENTREGA='"+DateTime.Now.ToString("yyyy-MM-dd")+"',MNIT_RESPONSABLE_ENTREGA='"+nitResponsable+"', TEST_CODIGO='E' WHERE FECHA_ENTREGA IS NULL AND NUM_DOCUMENTO="+ddlDocumento.SelectedValue);
			Response.Redirect(indexPage+"?process=Comercial.PagoGiros&path="+Request.QueryString["path"]+"&act=1");
		}
	}
}
