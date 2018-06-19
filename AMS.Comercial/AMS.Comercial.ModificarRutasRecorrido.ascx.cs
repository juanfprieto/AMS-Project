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
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ModificarRutasRecorrido.
	/// </summary>
	public class AMS_Comercial_ModificarRutasRecorrido : System.Web.UI.UserControl
	{
		#region Variables, controles

		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.ListBox lstSubRutas;
		protected System.Web.UI.WebControls.Button btnSubir;
		protected System.Web.UI.WebControls.Button btnBajar;
		protected System.Web.UI.WebControls.Button btnAgregar;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Button btnQuitar;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label lblDesc;
		protected System.Web.UI.WebControls.Label lblOrigen;
		protected System.Web.UI.WebControls.Label lblDestino;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.Panel pnlSubrutas;
		protected System.Web.UI.WebControls.TextBox txtRuta;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlRuta,"select mrut_codigo,mrut_codigo from DBXSCHEMA.mrutas WHERE mrut_clase=2 order by mrut_codigo;");
				ListItem itm=new ListItem("---Ruta---","");
				ddlRuta.Items.Insert(0,itm);
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El recorrido ha sido modificado.");
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
			this.ddlRuta.SelectedIndexChanged += new System.EventHandler(this.ddlRuta_SelectedIndexChanged);
			this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
			this.btnSubir.Click += new System.EventHandler(this.btnSubir_Click);
			this.btnBajar.Click += new System.EventHandler(this.btnBajar_Click);
			this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Traer Subrutas de la ruta seleccionada
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string mruta=ddlRuta.SelectedValue.Trim();
			DataSet dsRutas=new DataSet();
			DatasToControls bind=  new DatasToControls();
			pnlRuta.Visible=pnlSubrutas.Visible=false;
			lblDesc.Text=lblDestino.Text=lblOrigen.Text="";
			DBFunctions.Request(dsRutas,IncludeSchema.NO,"select rt.mrut_codigo as COD,rt.mrut_codigo concat ': ' concat rt.mrut_descripcion as NOM from DBXSCHEMA.mrutas rt join DBXSCHEMA.mruta_intermedia mi on mi.mruta_secundaria=rt.mrut_codigo where mi.mruta_principal='"+mruta+"' ORDER BY mi.mruta_secuencia");
			ViewState["SubRutas"]=dsRutas;
			lstSubRutas.DataSource=dsRutas;
			lstSubRutas.DataBind();
			DBFunctions.Request(dsRutas,IncludeSchema.NO,"select rt.MRUT_DESCRIPCION, co.PCIU_NOMBRE, cd.PCIU_NOMBRE from DBXSCHEMA.mrutas rt, DBXSCHEMA.pciudad cd, DBXSCHEMA.PCIUDAD co WHERE co.PCIU_CODIGO=rt.PCIU_COD AND cd.PCIU_CODIGO=rt.PCIU_CODDES AND rt.MRUT_CODIGO='"+mruta+"';");
			if(dsRutas.Tables[1].Rows.Count>0){
				lblDesc.Text=dsRutas.Tables[1].Rows[0][0].ToString();
				lblDestino.Text=dsRutas.Tables[1].Rows[0][1].ToString();
				lblOrigen.Text=dsRutas.Tables[1].Rows[0][2].ToString();
				pnlRuta.Visible=pnlSubrutas.Visible=true;
			}
		}

		//Guardar recorrido
		private void btnAgregar_Click(object sender, System.EventArgs e)
		{
			string mruta=ddlRuta.SelectedValue.Trim();
			string nruta=txtRuta.Text.Trim().ToUpper();
			string desc;
			if(nruta.Length==6)nruta=nruta+" ";
			txtRuta.Text=nruta;
			if(nruta.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar una ruta secundaria.");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT MRUT_CODIGO FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+nruta+"';")){
                Utils.MostrarAlerta(Response, "  La ruta secundaria no existe.");
				return;
			}
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			if(mruta.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Debe seleccionar una ruta principal.");
				return;
			}
			if(lstSubRutas.Items.FindByValue(nruta)==null)
			{
				desc=DBFunctions.SingleData("select rt.MRUT_DESCRIPCION from DBXSCHEMA.mrutas rt WHERE rt.MRUT_CODIGO='"+nruta+"';");
				DataRow dr=dsSubRutas.Tables[0].NewRow();
				dr[0]=nruta;
				dr[1]=nruta+": "+desc;
				dsSubRutas.Tables[0].Rows.Add(dr);
			}
			else
			{
                Utils.MostrarAlerta(Response, "  Ya agrego la subruta especificada.");
				return;
			}
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
		}

		//Subir subruta de orden
		private void btnSubir_Click(object sender, System.EventArgs e){
			int i=lstSubRutas.SelectedIndex;
			string codA,nomA;
			if(i<=0 || lstSubRutas.Items.Count==0)
				return;
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			codA=dsSubRutas.Tables[0].Rows[i][0].ToString();
			nomA=dsSubRutas.Tables[0].Rows[i][1].ToString();
			dsSubRutas.Tables[0].Rows[i][0]=dsSubRutas.Tables[0].Rows[i-1][0];
			dsSubRutas.Tables[0].Rows[i][1]=dsSubRutas.Tables[0].Rows[i-1][1];
			dsSubRutas.Tables[0].Rows[i-1][0]=codA;
			dsSubRutas.Tables[0].Rows[i-1][1]=nomA;
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
			lstSubRutas.SelectedIndex=i-1;
		}

		//Bajar subruta de orden
		private void btnBajar_Click(object sender, System.EventArgs e){
			int i=lstSubRutas.SelectedIndex;
			string codA,nomA;
			if(i<0 || i==lstSubRutas.Items.Count-1 || lstSubRutas.Items.Count==0)
				return;
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			codA=dsSubRutas.Tables[0].Rows[i][0].ToString();
			nomA=dsSubRutas.Tables[0].Rows[i][1].ToString();
			dsSubRutas.Tables[0].Rows[i][0]=dsSubRutas.Tables[0].Rows[i+1][0];
			dsSubRutas.Tables[0].Rows[i][1]=dsSubRutas.Tables[0].Rows[i+1][1];
			dsSubRutas.Tables[0].Rows[i+1][0]=codA;
			dsSubRutas.Tables[0].Rows[i+1][1]=nomA;
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
			lstSubRutas.SelectedIndex=i+1;
		}

		//Guardar
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			//Borrar actuales
			DBFunctions.NonQuery("DELETE FROM DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL='"+ddlRuta.SelectedValue+"'");
			//Insertar actuales
			for(int n=0;n<lstSubRutas.Items.Count;n++)
				DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MRUTA_INTERMEDIA VALUES ('"+ddlRuta.SelectedValue+"','"+lstSubRutas.Items[n].Value+"',"+n+")");
			Response.Redirect(indexPage+"?process=Comercial.ModificarRutasRecorrido&path="+Request.QueryString["path"]+"&act=1");
		}

		//Quitar elemento
		private void btnQuitar_Click(object sender, System.EventArgs e){
			int i=lstSubRutas.SelectedIndex;
			if(i<0 || lstSubRutas.Items.Count==0)
				return;
			DataSet dsSubRutas=(DataSet)ViewState["SubRutas"];
			dsSubRutas.Tables[0].Rows.Remove(dsSubRutas.Tables[0].Rows[i]);
			lstSubRutas.DataSource=dsSubRutas;
			lstSubRutas.DataBind();
			ViewState["SubRutas"]=dsSubRutas;
		}

	}
}
