namespace AMS.Comercial 
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Microsoft.Web.UI.WebControls;
	using AMS.DB;
	using AMS.Forms;
    using AMS.Tools;
	using Ajax;
	using AMS.DBManager;

	/// <summary>
	///		Descripción breve de AMS_Comercial_ServiciosAnticipos.
	/// </summary>
	public class AMS_Comercial_ServiciosAnticipos : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlPlanilla;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList ddlConcepto;
		protected System.Web.UI.WebControls.Panel pnlConcepto;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.TextBox txtNITReceptor;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.TextBox txtDescripcion;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtCantidad;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Panel pnlDatos;
		protected System.Web.UI.WebControls.TextBox txtNITReceptora;
		protected System.Web.UI.WebControls.TextBox txtNITReceptorb;
		protected System.Web.UI.WebControls.TextBox txtValorUnidad;
		protected System.Web.UI.WebControls.TextBox txtValorTotal;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblDescripcion;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblCantidad;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblValorUnidad;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label lblValorTotal;
		protected System.Web.UI.WebControls.Button btnRegistrar;
		protected System.Web.UI.WebControls.Panel pnlConfirma;
		protected System.Web.UI.WebControls.Button btnAtras;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblNIT;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label lblPlaca;
		protected System.Web.UI.WebControls.Label lblNumDocumento;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtNumDocReferencia;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label lblNumDocumentoRef;
		public string strImprime;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls bind=  new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				btnGuardar.Attributes.Add("onClick", "return(validarGasto());");
				strImprime="";
				if(Request.QueryString["act"]!=null && Request.QueryString["ant"]!=null)
				{
					if(Tools.Browser.IsMobileBrowser())
					{
						string plantilla="";
						string nlchar="`",redChar="^";
						int anchoTiquete=Tiquetes.anchoTiquete;
						try
						{
							string strLinea="";
							StreamReader strArchivo;
							strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaAnticipoMovil.txt");
							strLinea=strArchivo.ReadLine();
							//La primera linea puede contener el ancho del tiquete
							try
							{
								anchoTiquete=Int16.Parse(strLinea);
								strLinea=strArchivo.ReadLine();}
							catch{}

							while(strLinea!=null)
							{
								plantilla+=strLinea+nlchar;
								strLinea=strArchivo.ReadLine();
							}
							strArchivo.Close();
						}
						catch
						{
							Response.Write("<script language='javascript'>alert('No se ha creado la plantilla de anticipos moviles, no se pudo imprimir.');</script>");
							return;
						}
						strImprime=Plantillas.GenerarAnticipo(Request.QueryString["ant"], plantilla, nlchar, redChar, anchoTiquete);
					}
					else{
						Response.Write("<script language='javascript'>alert('El anticipo/servicio ha sido registrado con el número "+Request.QueryString["ant"]+".');"+
							"window.open('../aspx/AMS.Comercial.Anticipo.aspx?ant="+Request.QueryString["ant"]+"','ANTICIPO"+Request.QueryString["ant"]+"',\"width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no\");"+
							"</script>");
					}
				}
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				if (ConfigurationManager.AppSettings["DiferenteFechaAnticipo"]== "SI")
				    txtFecha.Enabled=true;
				else
					txtFecha.Enabled=false;
				
				txtCantidad.Attributes.Add("onkeyup","NumericMask(this);Totales();");
				txtValorUnidad.Attributes.Add("onkeyup","NumericMask(this);Totales();");
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
			this.ddlPlanilla.SelectedIndexChanged += new System.EventHandler(this.ddlPlanilla_SelectedIndexChanged);
			this.ddlConcepto.SelectedIndexChanged += new System.EventHandler(this.ddlConcepto_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
			this.btnAtras.Click += new System.EventHandler(this.btnAtras_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//CAMBIA LA AGENCIA 
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			pnlDatos.Visible=false;
			pnlPlanilla.Visible=true;
			pnlConcepto.Visible=true;
			ddlConcepto.SelectedIndex=0;
			DatasToControls bind=new DatasToControls();
			//Planillas del usuario
			ddlPlanilla.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlPlanilla,
				"select mp.mpla_codigo, mp.mpla_codigo "+
				"from dbxschema.mplanillaviaje mp "+
				"where mp.mag_codigo="+ddlAgencia.SelectedValue+" and mp.fecha_liquidacion is null and fecha_planilla >= current date;");
			ddlPlanilla.Items.Insert(0,new ListItem("---no definida---",""));
			string CargoUsuario =DBFunctions.SingleData("select pcar_codigo from DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES where mag_codigo="+ddlAgencia.SelectedValue+"  and mnit_nit='"+nitResponsable+"';");
			if(CargoUsuario.Length==0)	CargoUsuario = "0";				
			//Conceptos
			ddlConcepto.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlConcepto,
				"SELECT ct.TCON_CODIGO,ct. NOMBRE from DBXSCHEMA.TCONCEPTOS_TRANSPORTES ct,DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE ac " +
				"WHERE Tdoc_CODIGO = 'ANT' and ac.mag_codigo="+ddlAgencia.SelectedValue+"  and ac.pcar_codigo="+CargoUsuario+"  and  ct.TCON_CODIGO = ac.TCON_CODIGO ORDER BY NOMBRE;");
			ddlConcepto.Items.Insert(0,new ListItem("---seleccione---",""));
		}

		private void ddlPlanilla_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtPlaca.Text=null;
			ddlConcepto.SelectedIndex=0;
			if(ddlPlanilla.SelectedValue.Length==0)return;
			txtPlaca.Text=DBFunctions.SingleData(
				"SELECT mv.mcat_placa from dbxschema.mplanillaviaje mp, dbxschema.mviaje mv "+
				"where mv.mviaje_numero=mp.mviaje_numero and mp.mrut_codigo= mv.mrut_codigo and mp.mpla_codigo="+ddlPlanilla.SelectedValue);
			pnlConcepto.Visible=true;
		}

		private void ddlConcepto_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pnlDatos.Visible=false;
			if(ddlConcepto.SelectedValue.Length==0)return;
			pnlDatos.Visible=true;
			//txtCantidad.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT cantidad_consumo FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue)).ToString("###,###,##0.##");
			
			txtValorUnidad.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(valor_unidad,0) FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue)).ToString("###,###,##0.##");
			//txtValorTotal.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT valor_total FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue)).ToString("###,###,##0.##");
		}

		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
             string agencia=ddlAgencia.SelectedValue;
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			DateTime fecha;
			if(nitResponsable.Length==0){
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			//Concepto
			if(ddlConcepto.SelectedValue.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe seleccionar el concepto.');</script>");
				return;
			}
			//Verificar placa
			if(txtPlaca.Text.Trim().Length>0)
			{
				if(!DBFunctions.RecordExist("select mcat_placa from dbxschema.mbusafiliado where mcat_placa='"+txtPlaca.Text+"';"))
				{
					Response.Write("<script language='javascript'>alert('La placa registrada no existe.');</script>");
					return;
				}
				
			}
			else
				txtPlaca.Text = null;
			//Validaciones
			
			if(txtNITReceptor.Text.Trim().Length==0){
				Response.Write("<script language='javascript'>alert('Debe dar los datos del receptor.');</script>");
				return;
			}
			//Receptor
			if(!DBFunctions.RecordExist("SELECT NIT from DBXSCHEMA.VTRANSPORTE_NITSRELACION where nit='"+txtNITReceptor.Text.Trim()+"';"))
			{
				Response.Write("<script language='javascript'>alert('El nit del receptor no es valido.');</script>");
				return;
			}
			if(txtDescripcion.Text.Trim().Length==0){
				Response.Write("<script language='javascript'>alert('Debe dar la descripción.');</script>");
				return;
			}
			try{
				if(double.Parse(txtCantidad.Text)<0)throw(new Exception());}
			catch{
				Response.Write("<script language='javascript'>alert('La cantidad no es válida.');</script>");
				return;}
			try{
				if(double.Parse(txtValorUnidad.Text)<0)throw(new Exception());}
			catch{
				Response.Write("<script language='javascript'>alert('El valor de la unidad no es válido.');</script>");
				return;}
			try{
				if(double.Parse(txtValorTotal.Text)<0)throw(new Exception());}
			catch{
				Response.Write("<script language='javascript'>alert('El valor total no es válido.');</script>");
				return;}
			try{
				fecha=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
				Response.Write("<script language='javascript'>alert('Fecha no válida.');</script>");
				return;}
			//Verifica cierre mensual y diario
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
			string CargoUsuario =DBFunctions.SingleData("select pcar_codigo from DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES where mag_codigo="+ddlAgencia.SelectedValue+"  and mnit_nit='"+nitResponsable+"';");
			double ValotTotal = double.Parse(txtValorTotal.Text.Replace(",","")); 
			double ValorMaximo = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(valor_maximo_autorizacion,0) from DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE " +
					                              "WHERE mag_codigo="+ddlAgencia.SelectedValue+"  and pcar_codigo="+CargoUsuario+"  and  TCON_CODIGO = "+ddlConcepto.SelectedValue+";"));	
            if(ValotTotal > ValorMaximo) 
			{
				Response.Write("<script language='javascript'>alert('El valor es mayor al maximo autorizado:$"+ValorMaximo.ToString("###,###,###")+" ');</script>");
				return;
			}
			lblFecha.Text=txtFecha.Text;
			lblPlaca.Text=txtPlaca.Text;
			lblDescripcion.Text=txtDescripcion.Text.Trim();
			lblCantidad.Text=txtCantidad.Text;
			lblValorUnidad.Text=txtValorUnidad.Text;
			lblValorTotal.Text=txtValorTotal.Text;
			lblNIT.Text=txtNITReceptor.Text+"<br>"+txtNITReceptora.Text+" "+txtNITReceptorb.Text;
			lblNumDocumento.Text=Anticipos.TraerSiguienteAnticipoVirtual();
			lblNumDocumentoRef.Text=txtNumDocReferencia.Text;
			pnlConfirma.Visible=true;
			pnlDatos.Visible=false;
			ddlAgencia.Enabled=false;
			ddlPlanilla.Enabled=false;
			ddlConcepto.Enabled=false;
			txtPlaca.Enabled=false;
		}

		private void btnAtras_Click(object sender, System.EventArgs e)
		{
			pnlConfirma.Visible=false;
			txtPlaca.Enabled=true;
			pnlDatos.Visible=true;
			ddlAgencia.Enabled=true;
			ddlPlanilla.Enabled=true;
			ddlConcepto.Enabled=true;
		}

		private void btnRegistrar_Click(object sender, System.EventArgs e)
		{
			//Guardar
			string agencia=ddlAgencia.SelectedValue;
			string nitReceptor=txtNITReceptor.Text.Trim();
			string planilla=ddlPlanilla.SelectedValue;
			
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario actual (responsable) no tiene un NIT asociado.');</script>");
				return;
			}
			//Receptor
			if(!DBFunctions.RecordExist("SELECT NIT from DBXSCHEMA.VTRANSPORTE_NITSRELACION where nit='"+nitReceptor+"';"))
			{
				Response.Write("<script language='javascript'>alert('El nit del receptor no es valido.');</script>");
				return;
			}
			//Conseguir papeleria
			string numDocumento=Anticipos.TraerSiguienteAnticipoVirtual();
			string numLineaS;
			string tipoUnidad;
			string docReferencia=txtNumDocReferencia.Text.Trim();
			docReferencia=(docReferencia.Length==0)?"NULL":"'"+docReferencia+"'";

			string placa=lblPlaca.Text.Trim();
			if(placa.Length==0)
			   placa="NULL";
			else 
               placa="'"+placa+"'";
			if(planilla.Length==0){
				planilla="NULL";
				numLineaS="NULL";}
			else
				numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			tipoUnidad=DBFunctions.SingleData("SELECT tund_consumo FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+ddlConcepto.SelectedValue);
			ArrayList sqlStrings = new ArrayList();
			//Actualizar papeleria
			sqlStrings.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('ANT',"+numDocumento+",'V',0,'"+lblFecha.Text+"',0,"+agencia+",'"+lblFecha.Text+"','"+nitResponsable+"',0,'"+lblFecha.Text+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
			//Insertar servicio/anticipo
            if (ddlConcepto.SelectedValue == "288")
                placa = ddlAgencia.SelectedValue;

			sqlStrings.Add("INSERT INTO DBXSCHEMA.MGASTOS_TRANSPORTES VALUES('ANT',"+numDocumento+",'V','"+lblFecha.Text+"',"+placa+","+planilla+","+numLineaS+","+ddlConcepto.SelectedValue+","+agencia+",'"+nitResponsable+"','"+nitReceptor+"','"+lblDescripcion.Text+"',"+lblCantidad.Text.Replace(",","")+",'"+tipoUnidad+"',"+lblValorUnidad.Text.Replace(",","")+","+lblValorTotal.Text.Replace(",","")+",'A',"+docReferencia+");");

			//Actualizar planilla->num linea si es Real
			if(!planilla.Equals("NULL"))
				sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
			
			if(DBFunctions.Transaction(sqlStrings))
			{
				if(Tools.Browser.IsMobileBrowser())
					Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.ServiciosAnticipos&path="+Request.QueryString["path"]+"&act=1&ant="+numDocumento);
				else
					Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.ServiciosAnticipos&path="+Request.QueryString["path"]+"&act=1&ant="+numDocumento);
			}
			else
				lblError.Text += "Error: " + DBFunctions.exceptions;
		}
	}
}