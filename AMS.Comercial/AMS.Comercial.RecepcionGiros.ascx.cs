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

	/// <summary>
	///		Descripción breve de AMS_Comercial_RecepcionGiros.
	/// </summary>
	public class AMS_Comercial_RecepcionGiros : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaO;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.DropDownList ddlAgenciaD;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.DropDownList txtNITEmisorc;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.TextBox txtNITEmisor;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtNITEmisora;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.TextBox txtNITEmisorb;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.TextBox txtNITEmisord;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.TextBox txtNITEmisore;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.DropDownList txtNITReceptorc;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox txtNITReceptor;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.TextBox txtNITReceptora;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.TextBox txtNITReceptorb;
		protected System.Web.UI.WebControls.Label Label26;
		protected System.Web.UI.WebControls.TextBox txtNITReceptord;
		protected System.Web.UI.WebControls.Label Label27;
		protected System.Web.UI.WebControls.TextBox txtNITReceptore;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtValor;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlTipo;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlPlanilla;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
		protected System.Web.UI.WebControls.Panel pnlInicial;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblValorGiro;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblCostoGiro;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblTotal;
		protected System.Web.UI.WebControls.Button btnRegistrar;
		protected System.Web.UI.WebControls.Button btnAtras;
		protected System.Web.UI.WebControls.Panel pnlConfirmar;
		protected System.Web.UI.WebControls.Label lblValorIVA;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label lblPorcentajeGiro;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblPorcentajeIVA;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblNumDocumento;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtCosto;
		protected System.Web.UI.WebControls.Panel pnlCrear;
		protected System.Web.UI.WebControls.Panel pnlImprimir;
		public string strPorcentajeGiro;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Comercial.AMS_Comercial_RecepcionGiros));
			if (!IsPostBack){
				DatasToControls bind=new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgenciaO);
				//Todas las agencias
				bind.PutDatasIntoDropDownList(ddlAgenciaD,
					"select ma.mag_codigo,ma.mage_nombre from DBXSCHEMA.magencia ma order by ma.mage_nombre");
				//Tipo Docs
				bind.PutDatasIntoDropDownList(txtNITEmisorc,"select TNIT_TIPONIT, TNIT_NOMBRE FROM DBXSCHEMA.TNIT;");
				txtNITEmisorc.SelectedIndex= txtNITEmisorc.Items.IndexOf(txtNITEmisorc.Items.FindByValue("C"));
				bind.PutDatasIntoDropDownList(txtNITReceptorc,"select TNIT_TIPONIT, TNIT_NOMBRE FROM DBXSCHEMA.TNIT;");
				txtNITReceptorc.SelectedIndex= txtNITReceptorc.Items.IndexOf(txtNITReceptorc.Items.FindByValue("C"));
				//NIT
				txtNITEmisor.Attributes.Add("onKeyDown", "return(KeyDownHandlerNIT(this));");
				txtNITReceptor.Attributes.Add("onKeyDown", "return(KeyDownHandlerNIT(this));");
				ListItem it=new ListItem("---no definida---","");
				ddlPlanilla.Items.Add(it);
				btnGuardar.Attributes.Add("onClick", "return(validarGiro());");
				if(Request.QueryString["act"]!=null && Request.QueryString["gir"]!=null)
					Response.Write("<script language='javascript'>alert('El giro ha sido registrado con el número "+Request.QueryString["gir"]+".');</script>");
				ViewState["PorcentajeGiro"]=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_PORCENTAJE FROM DBXSCHEMA.TPORCENTAJESTRANSPORTES WHERE CLAVE='GIRO';")).ToString("0");
				ViewState["Giro"]="0";
			}
			strPorcentajeGiro=ViewState["PorcentajeGiro"].ToString();
		}
		
		//Traer NIT
		[Ajax.AjaxMethod]
		public string TraaerNIT(string NIT)
		{
			DataSet dsNIT=new DataSet();
			string nombre="",apellido="",tipo="",telefono="",direccion="";
			DBFunctions.Request(dsNIT,IncludeSchema.NO,"select mnit_nombres, mnit_apellidos, tnit_tiponit, mnit_telefono, mnit_direccion FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+NIT+"';");
			if(dsNIT.Tables[0].Rows.Count>0){
				nombre=dsNIT.Tables[0].Rows[0][0].ToString();
				apellido=dsNIT.Tables[0].Rows[0][1].ToString();
				tipo=dsNIT.Tables[0].Rows[0][2].ToString();
				telefono=dsNIT.Tables[0].Rows[0][3].ToString();
				direccion=dsNIT.Tables[0].Rows[0][4].ToString();
			}
			return(nombre+"|"+apellido+"|"+tipo+"|"+telefono+"|"+direccion);
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
			this.ddlAgenciaO.SelectedIndexChanged += new System.EventHandler(this.ddlAgenciaO_SelectedIndexChanged);
			this.ddlAgenciaD.SelectedIndexChanged += new System.EventHandler(this.ddlAgenciaD_SelectedIndexChanged);
			this.ddlTipo.SelectedIndexChanged += new System.EventHandler(this.ddlTipo_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
			this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		//Validar, confirmar
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			string agenciaO=ddlAgenciaO.SelectedValue,agenciaD=ddlAgenciaD.SelectedValue,tipo=ddlTipo.SelectedValue;
			string nitEmisor=txtNITEmisor.Text.Trim(), nombreEmisor=txtNITEmisora.Text.Trim(), apellidoEmisor=txtNITEmisorb.Text.Trim(), tnitEmisor=txtNITEmisorc.SelectedValue, telefonoEmisor=txtNITEmisord.Text.Trim(), direccionEmisor=txtNITEmisore.Text.Trim();
			string nitReceptor=txtNITReceptor.Text.Trim(), nombreReceptor=txtNITReceptora.Text.Trim(), apellidoReceptor=txtNITReceptorb.Text.Trim(), tnitReceptor=txtNITReceptorc.SelectedValue, telefonoReceptor=txtNITReceptord.Text.Trim(), direccionReceptor=txtNITReceptore.Text.Trim();
			//Validaciones
			//Agencias
			if(agenciaO==agenciaD){
				Response.Write("<script language='javascript'>alert('La agencia de origen es igual a la agencia de destino.');</script>");
				return;
			}
			//Emisor
			if(nitEmisor.Length==0||nombreEmisor.Length==0||apellidoEmisor.Length==0||tnitEmisor.Length==0||txtNITEmisorc.SelectedValue.Length==0||telefonoEmisor.Length==0||direccionEmisor.Length==0){
				Response.Write("<script language='javascript'>alert('Debe ingresar todos los datos del emisor.');</script>");
				return;
			}
			//Receptor
			if(nitReceptor.Length==0||nombreReceptor.Length==0||apellidoReceptor.Length==0||tnitReceptor.Length==0||txtNITReceptorc.SelectedValue.Length==0||telefonoReceptor.Length==0||direccionReceptor.Length==0){
				Response.Write("<script language='javascript'>alert('Debe ingresar todos los datos del receptor.');</script>");
				return;
			}
			//Valor del giro y costo
			double valorGiro,costoGiro;
			try{
				valorGiro=double.Parse(txtValor.Text.Trim());
				if(valorGiro<=1)throw(new Exception());
			}
			catch{
				Response.Write("<script language='javascript'>alert('Valor del giro no válido.');</script>");
				return;
			}
			try{
				costoGiro=double.Parse(txtCosto.Text.Trim());
				if(costoGiro<0)throw(new Exception());
			}
			catch
			{
				Response.Write("<script language='javascript'>alert('Costo del giro no válido.');</script>");
				return;
			}
			string ruta="";
			string planilla=ddlPlanilla.SelectedValue;
			//Validar ruta si el giro es real(debe existir una ruta entre las cioudades de las agencias
			if(tipo.Equals("M")){
				ruta=DBFunctions.SingleData("select mr.mrut_codigo from dbxschema.mrutas mr, dbxschema.magencia mo, dbxschema.magencia md "+
				"where mr.pciu_cod=mo.pciu_codigo and mr.pciu_coddes=md.pciu_codigo and mo.mag_codigo="+agenciaO+" and md.mag_codigo="+agenciaD+" "+
				"order by mr.mrut_codigo;");
				if(ruta.Length==0){
					Response.Write("<script language='javascript'>alert('No existe una ruta entre las agencias seleccionadas.');</script>");
					return;
				}
			}
			//Validar que la ciudad de las agencias no sea la misma
			if(DBFunctions.RecordExist("SELECT mo.mag_codigo from dbxschema.magencia mo, dbxschema.magencia md where mo.mag_codigo="+agenciaO+" and md.mag_codigo="+agenciaD+" and mo.pciu_codigo=md.pciu_codigo;")){
				Response.Write("<script language='javascript'>alert('Las agencias estan ubicadas en la misma ciudad.');</script>");
				return;
			}

			//Consultar numero documento
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			string numDocumento=Giros.TraerSiguienteGiroVirtual();

			//Desabilitar controles y mostrar panel confirmacion
			ddlAgenciaD.Enabled=false;
			ddlAgenciaO.Enabled=false;
			ddlTipo.Enabled=false;
			ddlPlanilla.Enabled=false;
			txtNITEmisor.Enabled=false;
			txtNITEmisora.Enabled=false;
			txtNITEmisorb.Enabled=false;
			txtNITEmisorc.Enabled=false;
			txtNITEmisord.Enabled=false;
			txtNITEmisore.Enabled=false;
			txtNITReceptor.Enabled=false;
			txtNITReceptora.Enabled=false;
			txtNITReceptorb.Enabled=false;
			txtNITReceptorc.Enabled=false;
			txtNITReceptord.Enabled=false;
			txtNITReceptore.Enabled=false;
			pnlInicial.Visible=false;
			pnlConfirmar.Visible=true;

			//Calcular valores
			double porcentajeIva;
			try{porcentajeIva=Convert.ToDouble(DBFunctions.SingleData("SELECT IVA_GIROS FROM DBXSCHEMA.CTRANSPORTES;"));}
			catch{porcentajeIva=0;}
			double porcentajeGiro;
			try{
				porcentajeGiro=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_PORCENTAJE FROM DBXSCHEMA.TPORCENTAJESTRANSPORTES WHERE CLAVE='GIRO';"));
			}
			catch{
				Response.Write("<script language='javascript'>alert('Parámetro de porcentaje giro no válido.');</script>");
				return;
			}
			double valorIVA=((valorGiro+costoGiro)*porcentajeIva)/100;
			double costoTotal=valorGiro+costoGiro+valorIVA;

			lblValorGiro.Text=valorGiro.ToString("#,###,###");
			lblPorcentajeGiro.Text=porcentajeGiro.ToString();
			lblCostoGiro.Text=costoGiro.ToString("#,###,###");
			lblPorcentajeIVA.Text=(porcentajeIva>0)?porcentajeIva.ToString():"0";
			lblValorIVA.Text=(valorIVA>0)?valorIVA.ToString("#,###,###"):"0";
			lblTotal.Text=costoTotal.ToString("#,###,###");
			lblFecha.Text=DateTime.Now.ToString("dd/MM/yyyy");
			lblNumDocumento.Text=numDocumento;
		}

		//Cambia el tipo de giro
		private void ddlTipo_SelectedIndexChanged(object sender, System.EventArgs e){
			pnlPlanilla.Visible=ddlTipo.SelectedValue.Equals("M");
			CargarPlanillas();
		}

		private void ddlAgenciaD_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarPlanillas();
		}

		private void ddlAgenciaO_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarPlanillas();
		}
		//Cargar planillas
		private void CargarPlanillas()
		{
			ddlPlanilla.Items.Clear();
			if(ddlTipo.SelectedValue=="M")
				CargarPlanillasReal();
			ListItem it=new ListItem("---no definida---","");
			ddlPlanilla.Items.Insert(0,it);

		}
		//Carga planillas segun ciudades de agencias
		private void CargarPlanillasReal()
		{

			string agenciaO=ddlAgenciaO.SelectedValue,agenciaD=ddlAgenciaD.SelectedValue;
			DataSet dsCiudades=new DataSet();
			DBFunctions.Request(dsCiudades, IncludeSchema.NO, "SELECT mo.pciu_codigo,md.pciu_codigo from DBXSCHEMA.magencia mo, DBXSCHEMA.magencia md where mo.mag_codigo="+agenciaO+" and md.mag_codigo="+agenciaD+" and mo.pciu_codigo<>md.pciu_codigo;");
			//Ciudades
			string ciudadO,ciudadD;
			if(dsCiudades.Tables[0].Rows.Count==0)return;
			ciudadO=dsCiudades.Tables[0].Rows[0][0].ToString();
			ciudadD=dsCiudades.Tables[0].Rows[0][1].ToString();

			//Planillas con ruta principal que contiene a la subruta seleccionada(origen-destino)
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlPlanilla,
				"select distinct mp.mpla_codigo, mp.mpla_codigo "+
				"from DBXSCHEMA.mplanillaviaje mp, DBXSCHEMA.MRUTA_INTERMEDIA mri, DBXSCHEMA.mrutas mro, DBXSCHEMA.mrutas mrd "+
				"where mp.mrut_codigo=mri.mruta_principal and MNIT_RESPONSABLE='"+nitResponsable+"' AND "+
				"mp.mag_codigo="+agenciaO+" and mri.mruta_principal=mp.mrut_codigo and mri.mruta_secundaria=mrd.mrut_codigo and "+
				"(mrd.pciu_cod='"+ciudadO+"' and mrd.pciu_coddes='"+ciudadD+"') "+
				"AND mp.fecha_liquidacion is null;");
		}

		//Guarda el giro
		private void btnRegistrar_Click(object sender, System.EventArgs e)
		{
			//Guardar giro
			string agenciaO=ddlAgenciaO.SelectedValue,agenciaD=ddlAgenciaD.SelectedValue,tipo=ddlTipo.SelectedValue;
			string nitEmisor=txtNITEmisor.Text.Trim(), nombreEmisor=txtNITEmisora.Text.Trim(), apellidoEmisor=txtNITEmisorb.Text.Trim(), tnitEmisor=txtNITEmisorc.SelectedValue, telefonoEmisor=txtNITEmisord.Text.Trim(), direccionEmisor=txtNITEmisore.Text.Trim();
			string nitReceptor=txtNITReceptor.Text.Trim(), nombreReceptor=txtNITReceptora.Text.Trim(), apellidoReceptor=txtNITReceptorb.Text.Trim(), tnitReceptor=txtNITReceptorc.SelectedValue, telefonoReceptor=txtNITReceptord.Text.Trim(), direccionReceptor=txtNITReceptore.Text.Trim();
			string planilla=ddlPlanilla.SelectedValue;
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0){
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			//Conseguir papeleria
			string numDocumento=Giros.TraerSiguienteGiroVirtual();
			string numLineaS,ruta="";
			//Validar ruta si el giro es real(debe existir una ruta entre las cioudades de las agencias
			if(tipo.Equals("M"))
			{
				ruta=DBFunctions.SingleData("select mr.mrut_codigo from dbxschema.mrutas mr, dbxschema.magencia mo, dbxschema.magencia md "+
					"where mr.pciu_cod=mo.pciu_codigo and mr.pciu_coddes=md.pciu_codigo and mo.mag_codigo="+agenciaO+" and md.mag_codigo="+agenciaD+" "+
					"order by mr.mrut_codigo;");
				if(ruta.Length==0){
					Response.Write("<script language='javascript'>alert('No existe una ruta entre las agencias seleccionadas.');</script>");
					return;
				}
				if(planilla.Length>0){
					numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
					if(numLineaS.Length==0)numLineaS="1";
				}
				else
					numLineaS="NULL";
				ruta="'"+ruta+"'";
			}
			else
			{
				numLineaS="NULL";
				planilla="NULL";
				ruta="NULL";
			}
			if(planilla.Length==0)
				planilla="NULL";
			ArrayList sqlStrings = new ArrayList();
			string ciudadO=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agenciaO+";");
			string ciudadD=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agenciaD+";");

			string strPorcentajeIVA=lblPorcentajeIVA.Text.Replace(",","");
			if(strPorcentajeIVA.Length==0)strPorcentajeIVA="0";
			string strPorcentajeGiro=lblPorcentajeGiro.Text.Replace(",","");
			if(strPorcentajeGiro.Length==0)strPorcentajeGiro="0";
			string strValorIVA=lblValorIVA.Text.Replace(",","");
			if(strValorIVA.Length==0)strValorIVA="0";
			string strCostoGiro=lblCostoGiro.Text.Replace(",","");
			if(strCostoGiro.Length==0)strCostoGiro="0";
			string strValorGiro=lblValorGiro.Text.Replace(",","");
			if(strValorGiro.Length==0)strValorGiro="0";
			
			//Guardar NITs si no existen
			string NitComodin ="NS";
			string nitEmisor1   =  nitEmisor;
			string nitReceptor1 =  nitReceptor;
			//Guardar NITs si no existen
			//Emisor
			if(nitEmisor == NitComodin)
			{ 
				nitEmisor1      = "GIRENV"+numDocumento;
				if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='"+nitEmisor1+"';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.MNIT VALUES('"+nitEmisor1+"',NULL,'"+nombreEmisor+"',NULL,'"+apellidoEmisor+"',NULL,'"+tnitEmisor+"','"+ciudadO+"','N','"+direccionEmisor+"','"+ciudadO+"','"+telefonoEmisor+"','ND','ND','ND','V','S','N','T');");
			}
			else 
				if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='"+nitEmisor+"';"))
				    sqlStrings.Add("INSERT INTO DBXSCHEMA.MNIT VALUES('"+nitEmisor+"',NULL,'"+nombreEmisor+"',NULL,'"+apellidoEmisor+"',NULL,'"+tnitEmisor+"','"+ciudadO+"','N','"+direccionEmisor+"','"+ciudadO+"','"+telefonoEmisor+"','ND','ND','ND','V','N','N','T');");
			//Receptor
			
			if(nitReceptor == NitComodin)
			{ 
				nitReceptor1     = "GIRREC"+numDocumento;
				if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='"+nitReceptor1+"';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.MNIT VALUES('"+nitReceptor1+"',NULL,'"+nombreReceptor+"',NULL,'"+apellidoReceptor+"',NULL,'"+tnitReceptor+"','"+ciudadD+"','N','"+direccionReceptor+"','"+ciudadD+"','"+telefonoReceptor+"','ND','ND','ND','V','S','N','T');");
			}
			else 
				if(nitEmisor != nitReceptor && !DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='"+nitReceptor+"';"))
				   sqlStrings.Add("INSERT INTO DBXSCHEMA.MNIT VALUES('"+nitReceptor+"',NULL,'"+nombreReceptor+"',NULL,'"+apellidoReceptor+"',NULL,'"+tnitReceptor+"','"+ciudadD+"','N','"+direccionReceptor+"','"+ciudadD+"','"+telefonoReceptor+"','ND','ND','ND','V','N','N','T');");
			

			//Actualizar papeleria
			sqlStrings.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('GIR',"+numDocumento+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agenciaO+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
			//Insertar giro
			sqlStrings.Add("INSERT INTO DBXSCHEMA.MGIROS VALUES('GIR',"+numDocumento+",'V',"+agenciaO+",'"+nitResponsable+"',"+agenciaD+",null,"+planilla+","+numLineaS+","+ruta+",'"+nitEmisor1+"','"+direccionEmisor+"','"+telefonoEmisor+"','"+nitReceptor1+"','"+direccionReceptor+"','"+telefonoReceptor+"',"+strPorcentajeIVA+","+strPorcentajeGiro+","+strValorIVA+","+strCostoGiro+","+strValorGiro+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,'A');");

			//Actualizar planilla->num linea si es Real
			if(tipo.Equals("M") && !planilla.Equals("NULL"))
				sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
			
			if(DBFunctions.Transaction(sqlStrings)){
				lblNumDocumento.Text=numDocumento;
				pnlImprimir.Visible=true;
				pnlCrear.Visible=false;
				ViewState["Giro"]=numDocumento;
			}
			else
				lblError.Text += "Error: " + DBFunctions.exceptions + "<br><br>";
		}

		//Volver
		private void btnAtras_Click(object sender, System.EventArgs e)
		{
			ddlAgenciaD.Enabled=true;
			ddlAgenciaO.Enabled=true;
			ddlTipo.Enabled=true;
			ddlPlanilla.Enabled=true;
			txtNITEmisor.Enabled=true;
			txtNITEmisora.Enabled=true;
			txtNITEmisorb.Enabled=true;
			txtNITEmisorc.Enabled=true;
			txtNITEmisord.Enabled=true;
			txtNITEmisore.Enabled=true;
			txtNITReceptor.Enabled=true;
			txtNITReceptora.Enabled=true;
			txtNITReceptorb.Enabled=true;
			txtNITReceptorc.Enabled=true;
			txtNITReceptord.Enabled=true;
			txtNITReceptore.Enabled=true;
			pnlInicial.Visible=true;
			pnlConfirmar.Visible=false;
		}
		
	}
}