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
	///		Descripción breve de AMS_Comercial_EntregaEncomiendas.
	/// </summary>
	public class AMS_Comercial_EntregaEncomiendas : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlDocumento;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label lblDocEmisor;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Label lblNombreEmisor;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label lblApellidoEmisor;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.Label lblTelefonoEmisor;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Label lblDireccionEmisor;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblDocReceptor;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label lblNombreReceptor;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblApellidoReceptor;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblTelefonoReceptor;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label lblDireccionReceptor;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblDescripcion;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblUnidades;
		protected System.Web.UI.WebControls.Label Label29;
		protected System.Web.UI.WebControls.Label lblPeso;
		protected System.Web.UI.WebControls.Label Label31;
		protected System.Web.UI.WebControls.Label lblVolumen;
		protected System.Web.UI.WebControls.Label Label33;
		protected System.Web.UI.WebControls.Label lblAvaluo;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblCostoEncomienda;
		protected System.Web.UI.WebControls.Button btnRegistrar;
		protected System.Web.UI.WebControls.Panel pnlPago;
		protected System.Web.UI.WebControls.Panel pnlDocumento;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label lblRuta;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblFechaR;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label lblFechaE;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblNumDocumento;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if (!IsPostBack){
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				//Rutas
				if(ddlAgencia.Items.Count>0)
					ddlAgencia_SelectedIndexChanged(sender,e);
				ListItem it=new ListItem("---seleccione---","");
				ddlRuta.Items.Insert(0,it);
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "La encomienda se ha entregado.");
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
			this.ddlRuta.SelectedIndexChanged += new System.EventHandler(this.ddlRuta_SelectedIndexChanged);
			this.ddlDocumento.SelectedIndexChanged += new System.EventHandler(this.ddlDocumento_SelectedIndexChanged);
			this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Cambia la agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlRuta,
				"select mr.mrut_codigo "+
				"from DBXSCHEMA.mrutas mr, DBXSCHEMA.MAGENCIA ma where "+
				"ma.mag_codigo="+ddlAgencia.SelectedValue+" and mr.pciu_coddes=ma.pciu_codigo "+
				"order by mr.mrut_codigo;");
			ddlRuta.Items.Insert(0,new ListItem("---seleccione---",""));
			pnlDocumento.Visible=false;
			pnlPago.Visible=false;
			lblRuta.Text="";
		}

		//Cambia el documento
		private void ddlDocumento_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DataSet dsEncomienda=new DataSet();
			DataSet dsNits=new DataSet();
			string nitEmisor,nitReceptor;
			
			lblDireccionEmisor.Text=lblDocEmisor.Text=lblNombreEmisor.Text=lblApellidoEmisor.Text=lblTelefonoEmisor.Text="";
			lblDireccionReceptor.Text=lblDocReceptor.Text=lblNombreReceptor.Text=lblApellidoReceptor.Text=lblTelefonoReceptor.Text="";
			lblDescripcion.Text=lblUnidades.Text=lblPeso.Text=lblVolumen.Text=lblAvaluo.Text=lblCostoEncomienda.Text="";

			if(ddlDocumento.SelectedValue.Length==0){
				pnlPago.Visible=false;
				return;
			}
			pnlPago.Visible=true;

			//Cargar datos			
			DBFunctions.Request(dsEncomienda, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MENCOMIENDAS WHERE NUM_DOCUMENTO="+ddlDocumento.SelectedValue+" and fecha_entrega is null");
			if(dsEncomienda.Tables[0].Rows.Count==0)return;
			nitEmisor=dsEncomienda.Tables[0].Rows[0]["MNIT_EMISOR"].ToString();
			nitReceptor=dsEncomienda.Tables[0].Rows[0]["MNIT_DESTINATARIO"].ToString();

			DBFunctions.Request(dsNits, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nitEmisor+"';"+
				"SELECT * FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+nitReceptor+"';");

			lblDocEmisor.Text=nitEmisor;
			lblDireccionEmisor.Text=dsEncomienda.Tables[0].Rows[0]["MNIT_EMISOR_DIRECCION"].ToString();
			lblTelefonoEmisor.Text=dsEncomienda.Tables[0].Rows[0]["MNIT_EMISOR_TELEFONO"].ToString();
			lblDocReceptor.Text=nitReceptor;
			lblDireccionReceptor.Text=dsEncomienda.Tables[0].Rows[0]["MNIT_DESTINATARIO_DIRECCION"].ToString();
			lblTelefonoReceptor.Text=dsEncomienda.Tables[0].Rows[0]["MNIT_DESTINATARIO_TELEFONO"].ToString();
			if(dsNits.Tables[0].Rows.Count>0)
			{
				lblNombreEmisor.Text=dsNits.Tables[0].Rows[0]["MNIT_NOMBRES"].ToString();
				lblApellidoEmisor.Text=dsNits.Tables[0].Rows[0]["MNIT_APELLIDOS"].ToString();
			}
			if(dsNits.Tables[1].Rows.Count>0)
			{
				lblNombreReceptor.Text=dsNits.Tables[1].Rows[0]["MNIT_NOMBRES"].ToString();
				lblApellidoReceptor.Text=dsNits.Tables[1].Rows[0]["MNIT_APELLIDOS"].ToString();
			}
			lblDescripcion.Text=dsEncomienda.Tables[0].Rows[0]["DESCRIPCION_CONTENIDO"].ToString();
			double unidades=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["UNIDADES"]);
			double peso=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["PESO"]);
			double volumen=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VOLUMEN"]);
			double avaluo=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VALOR_AVALUO"]);
			double total=Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VALOR_TOTAL"]);
			lblUnidades.Text=unidades.ToString("#,###,##0");
			lblPeso.Text=peso.ToString("#,###,##0.##");
			lblVolumen.Text=volumen.ToString("#,###,##0.##");
			lblAvaluo.Text=avaluo.ToString("#,###,##0");
			lblCostoEncomienda.Text=total.ToString("#,###,##0");
			lblFechaE.Text=DateTime.Now.ToString("dd/MM/yyyy");
			lblFechaR.Text=(Convert.ToDateTime(dsEncomienda.Tables[0].Rows[0]["FECHA_RECIBE"])).ToString("dd/MM/yyyy");
			lblNumDocumento.Text=dsEncomienda.Tables[0].Rows[0]["NUM_DOCUMENTO"].ToString();
		}

		//Cambia la ruta
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lblRuta.Text="";
			if(ddlRuta.SelectedValue.Length==0){
				pnlDocumento.Visible=true;
				return;
			}

			DataSet dsRuta=new DataSet();
			string sqlRuta="select rt.MRUT_DESCRIPCION AS DESC, co.PCIU_NOMBRE AS ORIG, cd.PCIU_NOMBRE AS DEST from DBXSCHEMA.mrutas rt, DBXSCHEMA.pciudad cd, DBXSCHEMA.PCIUDAD co WHERE co.PCIU_CODIGO=rt.PCIU_COD AND cd.PCIU_CODIGO=rt.PCIU_CODDES AND rt.MRUT_CODIGO='"+ddlRuta.SelectedValue+"';";
			DBFunctions.Request(dsRuta,IncludeSchema.NO,sqlRuta);
			if(dsRuta.Tables[0].Rows.Count>0)
				lblRuta.Text="<table style='font-size:XX-Small;font-weight:bold;'><tr><td>Descripción:</td><td>"+dsRuta.Tables[0].Rows[0][0].ToString()+"</td></tr><tr><td>Origen:</td><td>"+dsRuta.Tables[0].Rows[0][1].ToString()+"</td></tr><tr><td>Destino:</td><td>"+dsRuta.Tables[0].Rows[0][2].ToString()+"</td></tr></table>";
			else return;
			
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlDocumento,
				"select num_documento "+
				"from DBXSCHEMA.mencomiendas "+
				"where fecha_entrega is null and mrut_codigo='"+ddlRuta.SelectedValue+"' and tipo_remesa='V' order by num_documento");
			ddlDocumento.Items.Insert(0, new ListItem("---seleccione---",""));
			pnlDocumento.Visible=true;
		}

		//Entregar encomienda
		private void btnRegistrar_Click(object sender, System.EventArgs e)
		{
			if(ddlDocumento.SelectedValue.Length==0)
			{
                Utils.MostrarAlerta(Response, "Número de documento no válido");
				return;
			}
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
                Utils.MostrarAlerta(Response, "El usuario actual (responsable) no tiene un NIT asociado");
				return;
			}
			//Actualizar giro
			DBFunctions.NonQuery("UPDATE DBXSCHEMA.MENCOMIENDAS SET FECHA_ENTREGA='"+DateTime.Now.ToString("yyyy-MM-dd")+"',MNIT_RESPONSABLE_ENTREGA='"+nitResponsable+"', MAG_ENTREGA="+ddlAgencia.SelectedValue+", TEST_CODIGO='E' WHERE FECHA_ENTREGA IS NULL AND NUM_DOCUMENTO="+ddlDocumento.SelectedValue);
			Response.Redirect(indexPage+"?process=Comercial.EntregaEncomiendas&path="+Request.QueryString["path"]+"&act=1");
		}
	}
}
