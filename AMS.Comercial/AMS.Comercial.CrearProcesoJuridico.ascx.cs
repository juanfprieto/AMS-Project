namespace AMS.Comercial
{
	using System.IO;
	using System.Text;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
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

	/// <summary>
	///		Descripción breve de AMS_Comercial_CrearProcesoJuridico.
	/// </summary>
	public class AMS_Comercial_CrearProcesoJuridico : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlJuzgado;
		protected System.Web.UI.WebControls.TextBox txtRadicacion;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlProceso;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlSubproceso;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlClase;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtOrigen;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtFechaProxAct;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox txtNormatividad;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.DropDownList ddlEstado;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.HtmlControls.HtmlInputFile txtArchivo;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox txtDemandantea;
		protected System.Web.UI.WebControls.TextBox txtDemandadoa;
		protected System.Web.UI.WebControls.TextBox txtAboDemandantea;
		protected System.Web.UI.WebControls.TextBox txtAboDemandadoa;
		protected System.Web.UI.WebControls.TextBox txtDemandante;
		protected System.Web.UI.WebControls.TextBox txtDemandado;
		protected System.Web.UI.WebControls.TextBox txtAboDemandante;
		protected System.Web.UI.WebControls.TextBox txtAboDemandado;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.TextBox txtFechaProceso;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnCancelar;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//Numero
				string numero=DBFunctions.SingleData("SELECT MAX(NUM_PROCESO)+1 FROM DBXSCHEMA.MPROCESOS_JURIDICOS;");
				if(numero.Length==0)numero="1";
				lblNumero.Text=numero;
				//Juzgados
				bind.PutDatasIntoDropDownList(ddlJuzgado,"SELECT MJUZ_CODIGO,NOMBRE FROM DBXSCHEMA.MJUZGADOS ORDER BY NOMBRE;");
				ddlJuzgado.Items.Insert(0,new ListItem("--selecione--"));
				//Proceso
				bind.PutDatasIntoDropDownList(ddlProceso,"SELECT TPRO_CODIGO,NOMBRE FROM DBXSCHEMA.TPROCESOS_JURIDICOS ORDER BY NOMBRE;");
				ddlProceso.Items.Insert(0,new ListItem("--selecione--"));
				ddlSubproceso.Items.Insert(0,new ListItem("--selecione--"));
				//Estado
				bind.PutDatasIntoDropDownList(ddlEstado,"SELECT TCOD_ESTADO,NOMBRE FROM DBXSCHEMA.TESTADOS_PROCESOS_JURIDICOS ORDER BY TCOD_ESTADO;");
				ddlEstado.Items.Insert(0,new ListItem("--selecione--"));
				if(Request.QueryString["act"]!=null && Request.QueryString["num"]!=null)
                Utils.MostrarAlerta(Response, "El proceso ha sido creado con el número " + Request.QueryString["num"] + ".");
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
			this.ddlProceso.SelectedIndexChanged += new System.EventHandler(this.ddlProceso_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlProceso_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			//SubProceso
			ddlSubproceso.Items.Clear();
			if(ddlProceso.SelectedValue.Length>0)
				bind.PutDatasIntoDropDownList(ddlSubproceso,"SELECT TSPRO_CODIGO,NOMBRE FROM DBXSCHEMA.TSUBPROCESOS_JURIDICOS WHERE TPRO_CODIGO="+ddlProceso.SelectedValue+" ORDER BY NOMBRE;");
			ddlSubproceso.Items.Insert(0,new ListItem("--selecione--"));				
		}

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			#region Validaciones
			string error="";
			DateTime fechaProxAct=new DateTime(),fechaProceso=new DateTime();
			if(ddlJuzgado.SelectedValue.Length==0)
				error+="Debe seleccionar un juzgado. ";
			if(txtRadicacion.Text.Trim().Length==0)
				error+="Debe dar el código de radicación. ";
			if(ddlProceso.SelectedValue.Length==0)
				error+="Debe seleccionar el tipo de proceso. ";
			if(ddlSubproceso.SelectedValue.Length==0)
				error+="Debe seleccionar el tipo de subproceso. ";
			if(ddlClase.SelectedValue.Length==0)
				error+="Debe seleccionar la clase. ";
			if(txtDescripcion.Text.Trim().Length==0)
				error+="Debe dar la descripción. ";
			if(txtOrigen.Text.Trim().Length==0)
				error+="Debe dar el origen. ";
			if(txtDemandado.Text.Length==0)
				error+="Debe dar el NIT del demandado. ";
			if(txtDemandante.Text.Length==0)
				error+="Debe dar el NIT del demandante. ";
			if(txtAboDemandado.Text.Length==0)
				error+="Debe dar el NIT del abogado demandado. ";
			if(txtAboDemandante.Text.Length==0)
				error+="Debe dar el NIT del abogado demandante. ";
			if(ddlEstado.SelectedValue.Length==0)
				error+="Debe dar el estado. ";
			try{fechaProxAct=Convert.ToDateTime(txtFechaProxAct.Text);}
			catch{error+="Fecha de proxima actuación no válida. ";}
			try{fechaProceso=Convert.ToDateTime(txtFechaProceso.Text);}
			catch{error+="Fecha de proceso no válida. ";}
			if(error.Length>0){
                Utils.MostrarAlerta(Response, "" + error + "");
				return;}
			//Archivo
			string fileType="";
			HttpPostedFile pstFile=txtArchivo.PostedFile;
			if(pstFile.FileName!="")
			{
				fileType=pstFile.ContentType.ToLower();
				if(!fileType.EndsWith("msword") && !fileType.EndsWith("pdf"))
				{
                    Utils.MostrarAlerta(Response, "Archivo no válido debe estar en formato word(doc) o acrobat(pdf).");
					return;
				}
				fileType=fileType.EndsWith("msword")?"doc":"pdf";
			}
			#endregion Validaciones
			
			//Insertar
			string numero=DBFunctions.SingleData("SELECT coalesce(MAX(NUM_PROCESO)+1,1) FROM DBXSCHEMA.MPROCESOS_JURIDICOS;");
			ArrayList arrSql=new ArrayList();
			arrSql.Add("INSERT INTO DBXSCHEMA.MPROCESOS_JURIDICOS VALUES(DEFAULT,"+ddlJuzgado.SelectedValue+",'"+txtRadicacion.Text.Trim()+"',"+ddlProceso.SelectedValue+","+ddlSubproceso.SelectedValue+",'"+ddlClase.SelectedValue+"','"+txtDescripcion.Text.Replace("'","''").Trim()+"','"+txtOrigen.Text.Trim()+"',"+"'"+txtDemandante.Text+"','"+txtDemandado.Text+"','"+txtAboDemandante.Text+"','"+txtAboDemandado.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+fechaProxAct.ToString("yyyy-MM-dd")+"','"+txtNormatividad.Text.Replace("'","''")+"','"+fechaProceso.ToString("yyyy-MM-dd")+"',"+ddlEstado.SelectedValue+","+DBFunctions.SingleData("SELECT SUSU_CODIGO from DBXSCHEMA.SUSUARIO WHERE SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name.ToString()+"';")+",'');");
			if(!DBFunctions.Transaction(arrSql)){
                Utils.MostrarAlerta(Response, "Se ha encontrado un error al insertar el proceso.");
				lblError.Text=DBFunctions.exceptions;
				return;}
			//Guardar Archivo
			if(fileType.Length>0){
				string arcArchivo=numero+"."+fileType;
				pstFile.SaveAs(ConfigurationManager.AppSettings["MainPath"]+@"\img\DOC_JURIDICOS\"+arcArchivo);
				//Actualizar registro
				DBFunctions.NonQuery("UPDATE DBXSCHEMA.MPROCESOS_JURIDICOS SET ARCHIVO='"+arcArchivo+"' WHERE NUM_PROCESO="+numero);
			}
			Response.Redirect(indexPage+"?process=Comercial.CrearProcesoJuridico&act=1&path="+Request.QueryString["path"]+"&num="+numero);
		}
	}
}
