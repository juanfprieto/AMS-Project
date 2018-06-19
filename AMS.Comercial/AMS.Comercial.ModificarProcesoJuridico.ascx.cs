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
	///		Descripción breve de AMS_Comercial_ModificarProcesoJuridico.
	/// </summary>
	public class AMS_Comercial_ModificarProcesoJuridico : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.TextBox txtNumero;
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		protected System.Web.UI.WebControls.Panel pnlSeleccion;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlJuzgado;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.TextBox txtRadicacion;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlProceso;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlSubproceso;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlClase;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtOrigen;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtDemandante;
		protected System.Web.UI.WebControls.TextBox txtDemandantea;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtDemandado;
		protected System.Web.UI.WebControls.TextBox txtDemandadoa;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtAboDemandante;
		protected System.Web.UI.WebControls.TextBox txtAboDemandantea;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtAboDemandado;
		protected System.Web.UI.WebControls.TextBox txtAboDemandadoa;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtFechaProxAct;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.TextBox txtNormatividad;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.TextBox txtFechaProceso;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.DropDownList ddlEstado;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Panel pnlInformacion;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label lblNumero;
		protected System.Web.UI.WebControls.HyperLink lnkArchivo;
		protected System.Web.UI.WebControls.Button btnCancelar;
		protected System.Web.UI.HtmlControls.HtmlInputFile txtArchivo;
		protected System.Web.UI.WebControls.TextBox txtDescripcionA;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.TextBox txtAsistentesA;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.DataGrid dgrActuaciones;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.DropDownList ddlEstadoA;
		protected System.Web.UI.WebControls.Button btnAgregar;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//Juzgados
				bind.PutDatasIntoDropDownList(ddlJuzgado,"SELECT MJUZ_CODIGO,NOMBRE FROM DBXSCHEMA.MJUZGADOS ORDER BY NOMBRE;");
				//Proceso
				bind.PutDatasIntoDropDownList(ddlProceso,"SELECT TPRO_CODIGO,NOMBRE FROM DBXSCHEMA.TPROCESOS_JURIDICOS ORDER BY NOMBRE;");
				//Estado
				bind.PutDatasIntoDropDownList(ddlEstado,"SELECT TCOD_ESTADO,NOMBRE FROM DBXSCHEMA.TESTADOS_PROCESOS_JURIDICOS ORDER BY TCOD_ESTADO;");
				bind.PutDatasIntoDropDownList(ddlEstadoA,"SELECT TCOD_ESTADO,NOMBRE FROM DBXSCHEMA.TESTADOS_PROCESOS_JURIDICOS ORDER BY TCOD_ESTADO;");
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El proceso " + Request.QueryString["num"] + " ha sido actualizado.");
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
			this.ddlProceso.SelectedIndexChanged += new System.EventHandler(this.ddlProceso_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
			this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			DataSet dsProceso=new DataSet();
			DatasToControls bind=new DatasToControls();
			DBFunctions.Request(dsProceso,IncludeSchema.NO,"SELECT * FROM DBXSCHEMA.MPROCESOS_JURIDICOS WHERE NUM_PROCESO="+txtNumero.Text+";");
			if(dsProceso.Tables.Count==0 || dsProceso.Tables[0].Rows.Count==0)
			{
                Utils.MostrarAlerta(Response, "  El proceso no existe.");
				return;
			}
			pnlInformacion.Visible=true;
			pnlSeleccion.Visible=false;
			//Cargar Controles
			lblNumero.Text=dsProceso.Tables[0].Rows[0]["NUM_PROCESO"].ToString();
			ddlJuzgado.SelectedIndex=ddlJuzgado.Items.IndexOf(ddlJuzgado.Items.FindByValue(dsProceso.Tables[0].Rows[0]["MJUZ_CODIGO"].ToString()));
			txtRadicacion.Text=dsProceso.Tables[0].Rows[0]["COD_RADICACION"].ToString().Trim();
			ddlProceso.SelectedIndex=ddlProceso.Items.IndexOf(ddlProceso.Items.FindByValue(dsProceso.Tables[0].Rows[0]["TPRO_CODIGO"].ToString()));
			bind.PutDatasIntoDropDownList(ddlSubproceso,"SELECT TSPRO_CODIGO,NOMBRE FROM DBXSCHEMA.TSUBPROCESOS_JURIDICOS WHERE TPRO_CODIGO="+ddlProceso.SelectedValue+" ORDER BY NOMBRE;");
			ddlSubproceso.SelectedIndex=ddlSubproceso.Items.IndexOf(ddlSubproceso.Items.FindByValue(dsProceso.Tables[0].Rows[0]["TSPRO_CODIGO"].ToString()));
			ddlClase.SelectedIndex=ddlClase.Items.IndexOf(ddlClase.Items.FindByValue(dsProceso.Tables[0].Rows[0]["PROC_CLASE"].ToString()));
			txtDescripcion.Text=dsProceso.Tables[0].Rows[0]["PROC_DESCRIPCION"].ToString();
			txtOrigen.Text=dsProceso.Tables[0].Rows[0]["PROC_ORIGEN"].ToString();
			txtDemandante.Text=dsProceso.Tables[0].Rows[0]["MNIT_DEMANDANTE"].ToString();
			txtDemandado.Text=dsProceso.Tables[0].Rows[0]["MNIT_DEMANDADO"].ToString();
			txtAboDemandante.Text=dsProceso.Tables[0].Rows[0]["MNIT_ABOGADO_DEMANDANTE"].ToString();
			txtAboDemandado.Text=dsProceso.Tables[0].Rows[0]["MNIT_ABOGADO_DEMANDADO"].ToString();
			txtFechaProxAct.Text=Convert.ToDateTime(dsProceso.Tables[0].Rows[0]["FECHA_ACTUACION"]).ToString("yyyy-MM-dd");
			txtNormatividad.Text=dsProceso.Tables[0].Rows[0]["NORMATIVIDAD"].ToString();
			txtFechaProceso.Text=Convert.ToDateTime(dsProceso.Tables[0].Rows[0]["FECHA_PROC_JURIDICO"]).ToString("yyyy-MM-dd");
			ddlEstado.SelectedIndex=ddlEstado.Items.IndexOf(ddlEstado.Items.FindByValue(dsProceso.Tables[0].Rows[0]["TCOD_ESTADO"].ToString()));
			if(dsProceso.Tables[0].Rows[0]["ARCHIVO"].ToString().Length>0){
				lnkArchivo.Visible=true;
				lnkArchivo.Text="Ver archivo";
				lnkArchivo.NavigateUrl="../img/DOC_JURIDICOS/"+dsProceso.Tables[0].Rows[0]["ARCHIVO"].ToString();
				lnkArchivo.Target="_blank";
			}
			CargarActuaciones();
		}

		private void ddlProceso_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			//SubProceso
			ddlSubproceso.Items.Clear();
			if(ddlProceso.SelectedValue.Length>0)
				bind.PutDatasIntoDropDownList(ddlSubproceso,"SELECT TSPRO_CODIGO,NOMBRE FROM DBXSCHEMA.TSUBPROCESOS_JURIDICOS WHERE TPRO_CODIGO="+ddlProceso.SelectedValue+" ORDER BY NOMBRE;");
		}

		private void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Comercial.ModificarProcesoJuridico&path="+Request.QueryString["path"]);
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
			if(error.Length>0)
			{
                Utils.MostrarAlerta(Response, "  " + error + "");
				return;}
			//Archivo
			string fileType="";
			HttpPostedFile pstFile=txtArchivo.PostedFile;
			if(pstFile.FileName!="")
			{
				fileType=pstFile.ContentType.ToLower();
				if(!fileType.EndsWith("msword") && !fileType.EndsWith("pdf"))
				{
                    Utils.MostrarAlerta(Response, "  Archivo no válido debe estar en formato word(doc) o acrobat(pdf).");
					return;
				}
				fileType=fileType.EndsWith("msword")?"doc":"pdf";
			}
			#endregion Validaciones
			
			//Insertar
			string numero=lblNumero.Text;
			ArrayList arrSql=new ArrayList();
			arrSql.Add("UPDATE DBXSCHEMA.MPROCESOS_JURIDICOS SET "+
						"MJUZ_CODIGO="+ddlJuzgado.SelectedValue+", COD_RADICACION='"+txtRadicacion.Text.Trim()+"',TPRO_CODIGO="+ddlProceso.SelectedValue+",TSPRO_CODIGO="+ddlSubproceso.SelectedValue+",PROC_CLASE='"+ddlClase.SelectedValue+"',PROC_DESCRIPCION='"+txtDescripcion.Text.Replace("'","''").Trim()+"',PROC_ORIGEN='"+txtOrigen.Text.Trim()+"',MNIT_DEMANDANTE="+"'"+txtDemandante.Text+"',MNIT_DEMANDADO='"+txtDemandado.Text+"',MNIT_ABOGADO_DEMANDANTE='"+txtAboDemandante.Text+"',MNIT_ABOGADO_DEMANDADO='"+txtAboDemandado.Text+"',FECHA_PROXACTUACION='"+fechaProxAct.ToString("yyyy-MM-dd")+"',NORMATIVIDAD='"+txtNormatividad.Text.Replace("'","''")+"',FECHA_PROC_JURIDICO='"+fechaProceso.ToString("yyyy-MM-dd")+"',TCOD_ESTADO="+ddlEstado.SelectedValue+",SUSU_CODIGO="+DBFunctions.SingleData("SELECT SUSU_CODIGO from DBXSCHEMA.SUSUARIO WHERE SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name.ToString()+"';")+" "+
						"WHERE NUM_PROCESO="+numero+";");
			if(!DBFunctions.Transaction(arrSql)){
                Utils.MostrarAlerta(Response, "  Se ha encontrado un error al actualizar el proceso.");
				lblError.Text=DBFunctions.exceptions;
				return;}
			//Guardar Archivo
			if(fileType.Length>0)
			{
				string arcArchivo=numero+"."+fileType;
				pstFile.SaveAs(ConfigurationManager.AppSettings["MainPath"]+@"\img\DOC_JURIDICOS\"+arcArchivo);
				//Actualizar registro
				DBFunctions.NonQuery("UPDATE DBXSCHEMA.MPROCESOS_JURIDICOS SET ARCHIVO='"+arcArchivo+"' WHERE NUM_PROCESO="+numero);
			}
			Response.Redirect(indexPage+"?process=Comercial.ModificarProcesoJuridico&act=1&path="+Request.QueryString["path"]+"&num="+numero);
		}

		private void btnAgregar_Click(object sender, System.EventArgs e)
		{
			if(txtAsistentesA.Text.Trim().Length==0){
                Utils.MostrarAlerta(Response, "  Debe digitar los asistentes.");
				return;}
			if(txtDescripcionA.Text.Trim().Length==0){
                Utils.MostrarAlerta(Response, "  Debe dar la descripción.");
				return;}
			if(ddlEstadoA.SelectedValue.Length==0){
                Utils.MostrarAlerta(Response, "  Debe dar el estado.");
				return;}
			//Insertar
			DBFunctions.NonQuery("INSERT INTO DBXSCHEMA.MACTUACIONES_PROCESOS_JURIDICOS VALUES("+lblNumero.Text+",DEFAULT,'"+txtAsistentesA.Text+"','"+txtDescripcionA.Text+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"',"+ddlEstadoA.SelectedValue+");");
			CargarActuaciones();
		}

		private void CargarActuaciones()
		{
			DataSet dsActuaciones=new DataSet();
			DBFunctions.Request(dsActuaciones,IncludeSchema.NO,"SELECT * FROM DBXSCHEMA.MACTUACIONES_PROCESOS_JURIDICOS MA, DBXSCHEMA.TESTADOS_PROCESOS_JURIDICOS TE WHERE TE.TCOD_ESTADO=MA.TCOD_ESTADO AND MA.NUM_PROCESO="+lblNumero.Text+" ORDER BY FECHA_PROCESO DESC, NUM_PROCESO DESC;");
			dgrActuaciones.DataSource=dsActuaciones;
			dgrActuaciones.DataBind();
			dgrActuaciones.Visible=(dsActuaciones.Tables[0].Rows.Count>0);
		}
	}
}
