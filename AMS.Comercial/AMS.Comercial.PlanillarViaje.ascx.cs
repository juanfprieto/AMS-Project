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
	using AMS.DB;
	using AMS.Forms;


	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillarViaje.
	/// </summary>
	public class AMS_Comercial_PlanillarViaje : System.Web.UI.UserControl
	{

		#region Controles, variables
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox txtCodigo;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Panel pnlAgencia;
		protected System.Web.UI.WebControls.Label lblError;
		protected string[]dias={"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"};
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Comercial.AMS_Comercial_PlanillarViaje));	
			if (!IsPostBack)
			{
				//Agencias usuario
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				if(Request.QueryString["act"]!=null && Request.QueryString["pln"]!=null)
					Response.Write("<script language='javascript'>alert('El viaje ha sido planillado con el número de planilla "+Request.QueryString["pln"]+".');</script>");
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
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
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		[Ajax.AjaxMethod]
		public DataSet CargarPlanilla(string planilla)
		{
			DataSet Vins= new DataSet();
			DBFunctions.Request(Vins,IncludeSchema.NO,"select mp.mag_codigo as AGENCIA, mp.FECHA_PLANILLA as FECHA, mp.MNIT_RESPONSABLE AS NIT, mn.MNIT_NOMBRES CONCAT ' ' CONCAT mn.MNIT_APELLIDOS as NOMNIT from dbxschema.MPLANILLAVIAJE mp,dbxschema.MNIT mn where mn.MNIT_NIT=mp.mnit_responsable and mp.mpla_codigo="+planilla+";");
			return Vins;
		}

		//Cambia la ruta y el viaje
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string mrutav=ddlRuta.SelectedValue;
			DatasToControls bind=new DatasToControls();
			//Cambia la ruta
			if(mrutav.Length==0){
				pnlRuta.Visible=false;
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				txtCodigo.Text="";
				return;
			}
			string []prRV=mrutav.Split("-".ToCharArray());
			if(prRV.Length!=2)return;
			//Consultar viaje
			string numero=prRV[0], ruta=prRV[1];
			//nit Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			//Traer no. doc disponible para el responsable, agencia...
			//En este caso las planillas son virtuales 'V'
			txtCodigo.Text=DBFunctions.SingleData("Select NUM_DOCUMENTO from DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='PLA' AND TIPO_DOCUMENTO='V' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND MNIT_RESPONSABLE='"+nitResponsable+"' AND MAG_CODIGO="+ddlAgencia.SelectedValue+" ORDER BY NUM_DOCUMENTO");
			//Numeros
			pnlRuta.Visible=true;
		}

		//Guardar
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			//Validaciones
			string agencia=ddlAgencia.SelectedValue, fecha=txtFecha.Text;
			int numero, codigo;
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			//Numero
			string []prRV=ddlRuta.SelectedValue.Split("-".ToCharArray());
			string ruta=prRV[1];
			numero=int.Parse(prRV[0]);
			//Codigo
			try{
				codigo=int.Parse(txtCodigo.Text.Trim());
				if(codigo<0)throw(new Exception());
			}
			catch{
				Response.Write("<script language='javascript'>alert('Número de planilla no válido, debe ser un número entero.');</script>");
				return;
			}
			//Validar papeleria disponible
			if(!DBFunctions.RecordExist("Select NUM_DOCUMENTO from DBXSCHEMA.MCONTROL_PAPELERIA WHERE TIPO_DOCUMENTO='V' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND MNIT_RESPONSABLE='"+nitResponsable+"' AND MAG_CODIGO="+agencia+" AND TDOC_CODIGO='PLA' AND NUM_DOCUMENTO="+codigo)){
				Response.Write("<script language='javascript'>alert('No hay papeleria disponible para el número de planilla dado.');</script>");
				return;
			}
			//Validar Numero planilla no existe
			if(DBFunctions.RecordExist("SELECT MPLA_CODIGO from DBXSCHEMA.MPLANILLAVIAJE where MPLA_CODIGO="+codigo)){
				Response.Write("<script language='javascript'>alert('El número de planilla ya ha sido registrado.');</script>");
				return;
			}
			//Agencia
			if(agencia.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe seleccionar una agencia.');</script>");
				return;
			}
			//Validar planilla en agencia no existe
			if(DBFunctions.RecordExist("SELECT MPLA_CODIGO from DBXSCHEMA.MPLANILLAVIAJE where MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+numero+" AND MAG_CODIGO="+agencia+";")){
				Response.Write("<script language='javascript'>alert('El viaje ya se planilló en la agencia y ruta seleccionada.');</script>");
				return;
			}
			//Responsable
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario (responsable) no tiene un NIT asignado.');</script>");
				return;
			}
			//Fecha
			DateTime fechaT=DateTime.Now;
			try{
				string []prFch=fecha.Split("-".ToCharArray());
				DateTime fechaN=DateTime.Now;
				fechaT=new DateTime(int.Parse(prFch[0]),int.Parse(prFch[1]),int.Parse(prFch[2]));
				if(fechaT.CompareTo(new DateTime(fechaN.Year,fechaN.Month,fechaN.Day))<0)throw(new Exception());
				DateTime fechaV=Convert.ToDateTime(DBFunctions.SingleData("SELECT fecha_salida FROM dbxschema.mviaje WHERE mrut_codigo='"+ruta+"' and mviaje_numero="+numero));
				if(fechaT.CompareTo(fechaV)<0)throw(new Exception());
			}
			catch{
				Response.Write("<script language='javascript'>alert('Fecha no válida, debe tener el formato aaaa-mm-dd y ser mayor o igual a la fecha actual y a la de salida del viaje.');</script>");
				return;
			}
			//Insertar registro
			ArrayList sqlC=new ArrayList();
			sqlC.Add("insert into dbxschema.mplanillaviaje values("+codigo+",'"+ruta+"',"+numero+","+agencia+",'"+nitResponsable+"','"+fechaT.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,0,0,NULL);");
			//Actualizar papeleria
				sqlC.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE TDOC_CODIGO='PLA' AND NUM_DOCUMENTO="+codigo+";");
			if(DBFunctions.Transaction(sqlC))
				Response.Redirect(indexPage+"?process=Comercial.PlanillarViaje&act=1?path="+Request.QueryString["path"]+"&pln="+codigo);
			else
				lblError.Text=DBFunctions.exceptions;
		}

		//Cambia la agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pnlRuta.Visible=false;
			if(ddlAgencia.SelectedValue.Length==0){
				pnlAgencia.Visible=false;
				return;
			}
			pnlAgencia.Visible=true;
			string ciudad=DBFunctions.SingleData("select pciu_codigo "+
				"from DBXSCHEMA.magencia "+
				"where mag_codigo="+ddlAgencia.SelectedValue);
			if(ciudad.Length==0)return;
			DatasToControls bind=new DatasToControls();
			//Trae viajes activos que pasan por la ciudad de la agencia
			bind.PutDatasIntoDropDownList(ddlRuta,
				"select distinct rtrim(char(mv.mviaje_numero)) concat '-' concat mv.mrut_codigo "+ 
				"from dbxschema.mviaje mv "+
				"where (mv.estado_viaje='A' or mv.estado_viaje='V') and mv.mrut_codigo in "+
				"(select mi.mruta_principal from dbxschema.MRUTA_INTERMEDIA mi, dbxschema.MRUTAS mr "+
				" where mr.mrut_codigo=mi.mruta_secundaria and mr.pciu_cod='"+ciudad+"');");
			ddlRuta.Items.Insert(0,new ListItem("---seleccione---",""));
		}
	}
}
