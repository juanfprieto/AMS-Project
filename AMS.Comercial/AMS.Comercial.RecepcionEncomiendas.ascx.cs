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
	///		Descripción breve de AMS_Comercial_RecepcionEncomiendas.
	/// </summary>
	public class AMS_Comercial_RecepcionEncomiendas : System.Web.UI.UserControl
	{
		#region Conroles, variables
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlPlanilla;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
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
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label lblPorcentajeIVA;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblValorIVA;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Button btnRegistrar;
		protected System.Web.UI.WebControls.Button btnAtras;
		protected System.Web.UI.WebControls.Panel pnlConfirmar;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Panel pnlEncomienda;
		protected System.Web.UI.WebControls.Label lblRuta;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtUnidades;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.TextBox txtPeso;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtVolumen;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtValorAvaluo;
		protected System.Web.UI.WebControls.Label lblDescripcion;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label lblUnidades;
		protected System.Web.UI.WebControls.Label Label29;
		protected System.Web.UI.WebControls.Label lblPeso;
		protected System.Web.UI.WebControls.Label Label31;
		protected System.Web.UI.WebControls.Label lblVolumen;
		protected System.Web.UI.WebControls.Label Label33;
		protected System.Web.UI.WebControls.Label lblAvaluo;
		protected System.Web.UI.WebControls.Panel pnlInicial;
		protected System.Web.UI.WebControls.Label lblCostoEncomienda;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label28;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label30;
		protected System.Web.UI.WebControls.Label lblNumDocumento;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label32;
		protected System.Web.UI.WebControls.TextBox txtNumDocReferencia;
		protected System.Web.UI.WebControls.Label Label34;
		protected System.Web.UI.WebControls.Label lblNumDocumentoRef;
		protected System.Web.UI.WebControls.Label Label35;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Panel pnlCrear;
		protected System.Web.UI.WebControls.Panel pnlImprimir;
		protected System.Web.UI.WebControls.Label lblTotal;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		public string strFactorPeso,strFactorVolumen;
		#endregion
		protected System.Web.UI.WebControls.Label Label36;
		protected System.Web.UI.WebControls.TextBox txtCosto;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Comercial.AMS_Comercial_RecepcionEncomiendas));
			if (!IsPostBack){
				DatasToControls bind=new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				//Rutas
				if(ddlAgencia.Items.Count>0)
					ddlAgencia_SelectedIndexChanged(sender,e);
				//Tipo Docs
				bind.PutDatasIntoDropDownList(txtNITEmisorc,"select TNIT_TIPONIT, TNIT_NOMBRE FROM DBXSCHEMA.TNIT;");
				txtNITEmisorc.SelectedIndex= txtNITEmisorc.Items.IndexOf(txtNITEmisorc.Items.FindByValue("C"));
				bind.PutDatasIntoDropDownList(txtNITReceptorc,"select TNIT_TIPONIT, TNIT_NOMBRE FROM DBXSCHEMA.TNIT;");
				txtNITReceptorc.SelectedIndex= txtNITReceptorc.Items.IndexOf(txtNITReceptorc.Items.FindByValue("C"));
				//NIT
				txtNITEmisor.Attributes.Add("onKeyDown", "return(KeyDownHandlerNIT(this));");
				txtNITReceptor.Attributes.Add("onKeyDown", "return(KeyDownHandlerNIT(this));");
				ListItem it=new ListItem("---seleccione---","");
				ddlRuta.Items.Insert(0,it);
				btnGuardar.Attributes.Add("onClick", "return(validarEncomienda());");
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if (ConfigurationManager.AppSettings["DiferenteFechaRemesa"]== "SI")
					txtFecha.Enabled=true;
				else
					txtFecha.Enabled=false;

				if(Request.QueryString["act"]!=null && Request.QueryString["enc"]!=null)
					Response.Write("<script language='javascript'>alert('La encomienda se ha registrado con el número "+Request.QueryString["enc"]+".');</script>");
				ViewState["Encomienda"]="0";
				ViewState["FactorPeso"]="0";
				ViewState["FactorVolumen"]="0";
			}
			strFactorPeso=ViewState["FactorPeso"].ToString();
			strFactorVolumen=ViewState["FactorVolumen"].ToString();
		}

		//Traer NIT
		[Ajax.AjaxMethod]
		public string TraaerNIT(string NIT)
		{
			DataSet dsNIT=new DataSet();
			string nombre="",apellido="",tipo="",telefono="",direccion="";
			DBFunctions.Request(dsNIT, IncludeSchema.NO, "SELECT MPAS_NOMBRES AS NOMBRE, MPAS_APELLIDOS AS APELLIDOS,MPAS_TELEFONO as telefono, MPAS_DIRECCION as DIRECCION FROM DBXSCHEMA.MPASAJERO WHERE MPAS_NIT='" + NIT + "';");
			if(dsNIT.Tables[0].Rows.Count>0)
			{
				nombre=dsNIT.Tables[0].Rows[0][0].ToString();
				apellido=dsNIT.Tables[0].Rows[0][1].ToString();
				tipo="1";
				telefono=dsNIT.Tables[0].Rows[0][2].ToString();
				direccion=dsNIT.Tables[0].Rows[0][3].ToString();
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
			this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.ddlRuta.SelectedIndexChanged += new System.EventHandler(this.ddlRuta_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
			this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Cambia la ruta
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string ruta=ddlRuta.SelectedValue,peso,volumen;
			DataSet dsRuta=new DataSet();
			ListItem itm=new ListItem("---no definida---","");
			pnlEncomienda.Visible=pnlPlanilla.Visible=false;
			lblRuta.Text="";
			if(ruta.Length==0)
			{
				ddlPlanilla.Items.Clear();
				ddlPlanilla.Items.Add(itm);
			}
			string sqlRuta="select rt.MRUT_DESCRIPCION AS DESC, co.PCIU_NOMBRE AS ORIG, cd.PCIU_NOMBRE AS DEST, rt.valor_peso as peso, rt.valor_volumen as volumen from DBXSCHEMA.mrutas rt, DBXSCHEMA.pciudad cd, DBXSCHEMA.PCIUDAD co WHERE co.PCIU_CODIGO=rt.PCIU_COD AND cd.PCIU_CODIGO=rt.PCIU_CODDES AND rt.MRUT_CODIGO='"+ruta+"';";
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			DBFunctions.Request(dsRuta,IncludeSchema.NO,sqlRuta);
			if(dsRuta.Tables[0].Rows.Count>0)
				lblRuta.Text="<table style='font-size:XX-Small;font-weight:bold;'><tr><td>Descripción:</td><td>"+dsRuta.Tables[0].Rows[0][0].ToString()+"</td></tr><tr><td>Origen:</td><td>"+dsRuta.Tables[0].Rows[0][1].ToString()+"</td></tr><tr><td>Destino:</td><td>"+dsRuta.Tables[0].Rows[0][2].ToString()+"</td></tr></table>";
			else return;
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlPlanilla,"select mpla_codigo,mpla_codigo from mplanillaviaje where (mrut_codigo='"+ruta+"' OR mrut_codigo in(select MRUTA_PRINCIPAL from DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_SECUNDARIA='"+ruta+"')) AND fecha_liquidacion is null and fecha_planilla = current date;");
			ddlPlanilla.Items.Insert(0,itm);
			pnlEncomienda.Visible=pnlPlanilla.Visible=true;
			peso=dsRuta.Tables[0].Rows[0]["peso"].ToString();
			volumen=dsRuta.Tables[0].Rows[0]["volumen"].ToString();
			ViewState["FactorPeso"]=(peso.Length==0)?"0":peso;
			ViewState["FactorVolumen"]=(volumen.Length==0)?"0":volumen;
			strFactorPeso=ViewState["FactorPeso"].ToString();
			strFactorVolumen=ViewState["FactorVolumen"].ToString();
		}

		//Validar y pedir confirmacion
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			string ruta=ddlRuta.SelectedValue,planilla=ddlPlanilla.SelectedValue,agencia=ddlAgencia.SelectedValue;
			string nitEmisor=txtNITEmisor.Text.Trim(), nombreEmisor=txtNITEmisora.Text.Trim(), apellidoEmisor=txtNITEmisorb.Text.Trim(), tnitEmisor=txtNITEmisorc.SelectedValue, telefonoEmisor=txtNITEmisord.Text.Trim(), direccionEmisor=txtNITEmisore.Text.Trim();
			string nitReceptor=txtNITReceptor.Text.Trim(), nombreReceptor=txtNITReceptora.Text.Trim(), apellidoReceptor=txtNITReceptorb.Text.Trim(), tnitReceptor=txtNITReceptorc.SelectedValue, telefonoReceptor=txtNITReceptord.Text.Trim(), direccionReceptor=txtNITReceptore.Text.Trim();
			DateTime fecha= new DateTime();
			//DateTime fecha;
			//Validaciones
			if(ruta.Length==0){
				Response.Write("<script language='javascript'>alert('Debe seleccionar la ruta.');</script>");
				return;
			}
			// Verifica Bus
			string placa=txtPlaca.Text;
			if(placa.Length!=0)
			{
				string estado_bus=DBFunctions.SingleData("select testa_codigo from DBXSCHEMA.MBUSAFILIADO  where MCAT_PLACA='"+placa+"';");
				if(estado_bus.Length==0 || estado_bus=="9")
				{
					Response.Write("<script language='javascript'>alert(' El Bus esta Retirado o NO Existe.');</script>");
					return;
				}
			}
			//fecha
			try
			{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
				Response.Write("<script language='javascript'>alert('Fecha no válida.');</script>");
				return;
			}
			//Verifica cierre mensual
			
			int anio = fecha.Year;
			int mes  = fecha.Month;
			int periodo = anio * 100 + mes;
			
			string estado=DBFunctions.SingleData("select estado_cierre from DBXSCHEMA.periodos_cierre_transporte where numero_periodo="+periodo+";");
			if(estado.Length==0 || estado=="C")
			{
				Response.Write("<script language='javascript'>alert(' El periodo de la fecha esta cerrado o NO Existe.');</script>");
				return;
			}
			// Si existe es porque ya se contabilizo para la Agencia_dia
			if (DBFunctions.RecordExist("select MAG_CODIGO from DBXSCHEMA.DCIERRE_DIARIO_AGENCIA where MAG_CODIGO =  "+agencia+" and FECHA_CONTABILIZACION = '"+fecha.ToString("yyyy-MM-dd")+"';"))
			{
				Response.Write("<script language='javascript'>alert(' La Agencia-fecha ya se Contabilizo ');</script>");
				return;
			}

			//Emisor
			if(nitEmisor.Length==0||nombreEmisor.Length==0||apellidoEmisor.Length==0||tnitEmisor.Length==0||txtNITEmisorc.SelectedValue.Length==0||telefonoEmisor.Length==0||direccionEmisor.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe ingresar todos los datos del emisor.');</script>");
				return;
			}
			//Receptor
			if(nitReceptor.Length==0||nombreReceptor.Length==0||apellidoReceptor.Length==0||tnitReceptor.Length==0||txtNITReceptorc.SelectedValue.Length==0||telefonoReceptor.Length==0||direccionReceptor.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe ingresar todos los datos del receptor.');</script>");
				return;
			}
			//Valor de la remesa
			double unidades,peso,volumen,avaluo,costoEncomienda;
			try
			{
				unidades=double.Parse(txtUnidades.Text.Trim());
				if(unidades<=0)throw(new Exception());
				peso=double.Parse(txtPeso.Text.Trim());
				if(peso<=0)throw(new Exception());
				volumen=double.Parse(txtVolumen.Text.Trim());
				if(volumen<=0)throw(new Exception());
				avaluo=double.Parse(txtValorAvaluo.Text.Trim());
				if(avaluo<=0)throw(new Exception()); 
				costoEncomienda=double.Parse(txtCosto.Text.Trim());
				if(costoEncomienda<0)throw(new Exception());
			}
			catch
			{
				Response.Write("<script language='javascript'>alert('Uno de los valores de la encomienda no es válido.');</script>");
				return;
			}
			//Desabilitar controles y mostrar panel confirmacion
			ddlAgencia.Enabled=false;
			ddlRuta.Enabled=false;
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
			txtPlaca.Enabled=false;
			pnlInicial.Visible=false;
			pnlConfirmar.Visible=true;

			//Calcular valores
			double porcentajeIva;
			double factorPeso,factorVolumen;
			try{porcentajeIva=Convert.ToDouble(DBFunctions.SingleData("SELECT IVA_ENCOMIENDAS FROM DBXSCHEMA.CTRANSPORTES;"));}
			catch{porcentajeIva=0;}
			try{
				factorPeso=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_PESO FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+ruta+"';"));
			}
			catch{
				Response.Write("<script language='javascript'>alert('Factor de peso no válido.');</script>");
				return;
			}
			try{
				factorVolumen=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_VOLUMEN FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+ruta+"';"));
			}
			catch{
				Response.Write("<script language='javascript'>alert('Factor de volumen no válido.');</script>");
				return;
			}
			double valorIVA=(costoEncomienda*porcentajeIva)/100;
			double costoTotal=costoEncomienda+valorIVA;

			lblNumDocumento.Text=Encomiendas.TraerSiguienteEncomiendaVirtual();
			lblCostoEncomienda.Text=costoEncomienda.ToString("#,###,##0");
			lblPorcentajeIVA.Text=(porcentajeIva>0)?porcentajeIva.ToString():"0";
			lblValorIVA.Text=valorIVA.ToString("#,###,##0");
			lblTotal.Text=costoTotal.ToString("#,###,##0");
			lblDescripcion.Text=txtDescripcion.Text;
			lblVolumen.Text=txtVolumen.Text;
			lblPeso.Text=txtPeso.Text;
			lblUnidades.Text=txtUnidades.Text;
			lblAvaluo.Text=txtValorAvaluo.Text;
			lblFecha.Text=fecha.ToString("dd/MM/yyyy");
			lblNumDocumentoRef.Text=txtNumDocReferencia.Text;
		}

		//CAncelar
		private void btnAtras_Click(object sender, System.EventArgs e)
		{
			ddlAgencia.Enabled=true;
			ddlRuta.Enabled=true;
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
			txtPlaca.Enabled=true;
			pnlInicial.Visible=true;
			pnlConfirmar.Visible=false;
		}

		//Registrar remesa
		private void btnRegistrar_Click(object sender, System.EventArgs e)
		{
			//Guardar Remesa
			string ruta=ddlRuta.SelectedValue,planilla=ddlPlanilla.SelectedValue,agencia=ddlAgencia.SelectedValue;
			string nitEmisor=txtNITEmisor.Text.Trim(), nombreEmisor=txtNITEmisora.Text.Trim(), apellidoEmisor=txtNITEmisorb.Text.Trim(), tnitEmisor=txtNITEmisorc.SelectedValue, telefonoEmisor=txtNITEmisord.Text.Trim(), direccionEmisor=txtNITEmisore.Text.Trim();
			string nitReceptor=txtNITReceptor.Text.Trim(), nombreReceptor=txtNITReceptora.Text.Trim(), apellidoReceptor=txtNITReceptorb.Text.Trim(), tnitReceptor=txtNITReceptorc.SelectedValue, telefonoReceptor=txtNITReceptord.Text.Trim(), direccionReceptor=txtNITReceptore.Text.Trim();
			double totalE;
			try{
				totalE=Convert.ToDouble(lblTotal.Text);
				if(totalE<0)throw(new Exception());
			}
			catch{
				Response.Write("<script language='javascript'>alert('Total no valido.');</script>");
				return;
			}
			
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			//Conseguir papeleria
			string numDocumento=Encomiendas.TraerSiguienteEncomiendaVirtual();
			if(numDocumento.Length==0)
			{
				Response.Write("<script language='javascript'>alert('No hay papeleria disponible.');</script>");
				return;
			}
			string numLineaS;
			if(planilla.Length>0){
				numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
				if(numLineaS.Length==0)numLineaS="1";
			}
			else{
				numLineaS="NULL";
				planilla="NULL";
			}
			ruta="'"+ruta+"'";

			ArrayList sqlStrings = new ArrayList();
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			string docReferencia=txtNumDocReferencia.Text.Trim();
			docReferencia=(docReferencia.Length==0)?"NULL":"'"+docReferencia+"'";
			string placa=txtPlaca.Text;

			placa=(placa.Length==0)?"NULL":"'"+placa+"'";
			string NitComodin ="NS";
			string nitEmisor1   =  nitEmisor;
			string nitReceptor1 =  nitReceptor;
			//Guardar NITs si no existen
			//Emisor
			if(nitEmisor == NitComodin)
			{ 
				nitEmisor1      = "REMENV"+numDocumento;
				if(!DBFunctions.RecordExist("SELECT MPAS_NIT FROM dbxschema.mpasajero WHERE MPAS_NIT='"+nitEmisor1+"';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.mpasajero VALUES('" + nitEmisor1 + "','" + nombreEmisor + "',NULL,'" + apellidoEmisor + "',NULL,'" + direccionEmisor + "','" + ciudad + "','" + telefonoEmisor + "','ND','ND');");
			}
			else
				if (!DBFunctions.RecordExist("SELECT MPAS_NIT FROM dbxschema.mpasajero WHERE MPAS_NIT='" + nitEmisor + "';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.mpasajero VALUES('" + nitEmisor + "','" + nombreEmisor + "',NULL,'" + apellidoEmisor + "',NULL,'" + direccionEmisor + "','" + ciudad + "','" + telefonoEmisor + "','ND','ND');");
			//Receptor
			
			if(nitReceptor == NitComodin)
			{ 
				nitReceptor1     = "REMREC"+numDocumento;
				if (!DBFunctions.RecordExist("SELECT MPAS_NIT FROM dbxschema.mpasajero WHERE MPAS_NIT='" + nitReceptor1 + "';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.mpasajero VALUES('" + nitReceptor1 + "','" + nombreReceptor + "',NULL,'" + apellidoReceptor + "',NULL,'" + direccionReceptor + "','" + ciudad + "','" + telefonoReceptor + "','ND','ND');");

			}
			else
				if (nitEmisor != nitReceptor && !DBFunctions.RecordExist("SELECT MPAS_NIT FROM dbxschema.mpasajero WHERE MPAS_NIT='" + nitReceptor + "';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.mpasajero VALUES('" + nitReceptor + "','" + nombreReceptor + "',NULL,'" + apellidoReceptor + "',NULL,'" + direccionReceptor + "','" + ciudad + "','" + telefonoReceptor + "','ND','ND');");

			//Actualizar papeleria
			sqlStrings.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('REM',"+numDocumento+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");

			//Insertar encomienda
			sqlStrings.Add("INSERT INTO DBXSCHEMA.MENCOMIENDAS VALUES('REM',"+numDocumento+",'V',"+agencia+",'"+nitResponsable+"',null,null,"+planilla+","+numLineaS+","+ruta+",'"+nitEmisor1+"','"+direccionEmisor+"','"+telefonoEmisor+"','"+nitReceptor1+"','"+direccionReceptor+"','"+telefonoReceptor+"','"+lblDescripcion.Text+"',"+lblUnidades.Text.Replace(",","")+","+lblPeso.Text.Replace(",","")+","+lblVolumen.Text.Replace(",","")+","+lblPorcentajeIVA.Text.Replace(",","")+","+lblValorIVA.Text.Replace(",","")+","+lblCostoEncomienda.Text.Replace(",","")+",0,"+lblAvaluo.Text.Replace(",","")+","+totalE.ToString("0")+",'"+lblFecha.Text+"',NULL,'A',"+docReferencia+","+placa+");");

			//Actualizar planilla->num linea si es Real
			if(!planilla.Equals("NULL"))
				sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
			
			if(DBFunctions.Transaction(sqlStrings))
			{
				lblNumDocumento.Text=numDocumento;
				pnlImprimir.Visible=true;
				pnlCrear.Visible=false;
				ViewState["Encomienda"]=numDocumento;
			}
			else
			{
				lblError.Text += "Error: " + DBFunctions.exceptions + "<br><br>";
				return;
			}
		}

		//Cambia la agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			/*
			bind.PutDatasIntoDropDownList(ddlRuta,
				"select mr.mrut_codigo "+
				"from DBXSCHEMA.mrutas mr, DBXSCHEMA.MAGENCIA ma where "+
				"ma.mag_codigo="+ddlAgencia.SelectedValue+" and mr.pciu_cod=ma.pciu_codigo "+
				"order by mr.mrut_codigo;");
			*/
			bind.PutDatasIntoDropDownList(ddlRuta,
			   "select mrut_codigo,'[' concat mr.mrut_codigo concat '] ' concat pco.pciu_nombre concat ' - ' concat pcd.pciu_nombre as texto " +
				"from DBXSCHEMA.mrutas mr,DBXSCHEMA.MAGENCIA ma , DBXSCHEMA.pciudad pco, DBXSCHEMA.pciudad pcd " +
				"WHERE ma.mag_codigo="+ddlAgencia.SelectedValue+" and mr.pciu_cod=ma.pciu_codigo "+
				" and  mr.pciu_coddes=pcd.pciu_codigo and mr.pciu_cod=pco.pciu_codigo order by mrut_codigo;");
			ddlRuta.Items.Insert(0,new ListItem("---seleccione---",""));
		}
	}
}
