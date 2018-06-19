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
	///		Descripción breve de AMS_Comercial_PlanillaManual sin control de papaleria.
	/// </summary>
	public class AMS_Comercial_PlanillaManualGaviota : System.Web.UI.UserControl
	{
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.DropDownList ddlTipo;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		// protected System.Web.UI.WebControls.TextBox txtNitResponsable;
		// protected System.Web.UI.WebControls.TextBox txtResponsableAnticipo;
		protected System.Web.UI.WebControls.TextBox txtDescripcionAnticipo;
		protected System.Web.UI.WebControls.TextBox txtDescripcionEncomienda;

		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlHora;
		protected System.Web.UI.WebControls.DropDownList ddlMinuto;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtConductor;
		protected System.Web.UI.WebControls.TextBox txtConductora;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtRelevador;
		protected System.Web.UI.WebControls.TextBox txtRelevadora;
		protected System.Web.UI.WebControls.Panel pnlSeleccionar;
		protected System.Web.UI.WebControls.DataGrid dgrTiquetes;
		protected System.Web.UI.WebControls.Panel pnlElementos;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.TextBox txtPlanilla;
		protected System.Web.UI.WebControls.DataGrid dgrEncomiendas;
		//	protected System.Web.UI.WebControls.DataGrid dgrGiros;
		protected System.Web.UI.WebControls.DataGrid dgrPagos;
		protected System.Web.UI.WebControls.Button btnPlanillar;
		protected System.Web.UI.WebControls.Button btnGrabar;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtNITTiquetero;
		
		protected System.Web.UI.WebControls.Panel pnlViaje;
		protected System.Web.UI.WebControls.Button btnSeleccionar;
		protected System.Web.UI.WebControls.Label lblNumViaje;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblError;
		//protected System.Web.UI.WebControls.TextBox txtPrimerTiquete;
		protected System.Web.UI.WebControls.Button btnSeleccionarPlanilla;

		public string strActScript="";
		private string strTotalTiquetes="";
		private string strTotalEncomiendas="";
		private string strTotalGiros="";
		private string strCostoGiros="";
		private string strTotalTiquetesEsp="";
		private string strTiposTiquetesEsp="";
		private string strAnulacionTiquetes="";
		//	protected System.Web.UI.WebControls.DataGrid dgrTiqueteEsps;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPlacaa;
		private string strTotalPagos="";
		private string strTotalDescuentos="";
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList ddlRutaPrincipal;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.DataGrid dgrDescuentos;
		protected System.Web.UI.WebControls.Label Labe13;
		
		protected System.Web.UI.WebControls.Label lblTotalPlanilla;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.TextBox txtObservacion;
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_PlanillaManual));	
			Response.Cache.SetCacheability(HttpCacheability.NoCache);

			if (!IsPostBack)
			{
				Agencias.TraerAgenciasPrefijoUsuario(ddlAgencia);
				
				txtPlaca.Attributes.Add("onKeyDown", "return(KeyDownHandler(this));");
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");

				//Horas-minutos
				for(int i=0;i<24;i++)
					ddlHora.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				for(int i=0;i<60;i++)
					ddlMinuto.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				
				//Cargar conceptos anulacion etc.
				CargarConceptosAnulacionTiquetes();
				
				//Porcentaje costo giros
				double porcentajeGiro=Giros.PorcentajeCosto();
				ViewState["PorcentajeGiro"]=(porcentajeGiro>0)?porcentajeGiro.ToString("##0.##"):"0";
				//Porcentaje costo prepagos default
				double porcentajePrepago=Tiquetes.PorcentajePrepago();
				ViewState["PorcentajePrepago"]=(porcentajePrepago>0)?porcentajePrepago.ToString("##0.##"):"0";

				//Seleccionar agencia
				if(ddlAgencia.Items.Count>0)
					ddlAgencia_SelectedIndexChanged(sender,e);

				//Actualizo?
				if(Request.QueryString["act"]!=null && Request.QueryString["pln"]!=null)
					Response.Write("<script language='javascript'>alert('La planilla ha sido registrada con el numero "+Request.QueryString["pln"]+".');</script>");
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
			this.ddlRutaPrincipal.SelectedIndexChanged += new System.EventHandler(this.ddlRutaPrincipal_SelectedIndexChanged);
			this.btnSeleccionar.Click += new System.EventHandler(this.btnSeleccionar_Click);
			this.btnSeleccionarPlanilla.Click += new System.EventHandler(this.btnSeleccionarPlanilla_Click);
			this.dgrTiquetes.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrTiquetes_ItemDataBound);
			//temDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrTiqueteEsps_ItemDataBound);
			this.dgrEncomiendas.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrEncomiendas_ItemDataBound);
			//aBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrGiros_ItemDataBound);
			this.dgrPagos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrPagos_ItemDataBound);
			this.dgrDescuentos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrDescuentos_ItemDataBound);
			this.btnPlanillar.Click += new System.EventHandler(this.btnPlanillar_Click);
			//this.btnSeleccionarPlanilla.Click += new System.EventHandler(this.btnSeleccionarPlanilla_Click);
			this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		
		#region AJAX
		//Traer datos bus
		[Ajax.AjaxMethod]
		public DataSet TraerBus(string placa)
		{
			DataSet dsNIT=new DataSet();
			DBFunctions.Request(dsNIT,IncludeSchema.NO,
				"SELECT mn.MNIT_NIT AS NIT,mn.MNIT_NOMBRES CONCAT ' ' CONCAT mn.MNIT_NOMBRE2 CONCAT ' ' CONCAT mn.MNIT_APELLIDOS CONCAT ' ' CONCAT mn.MNIT_APELLIDO2 AS NOMBRE, mb.mbus_numero as NUMERO from DBXSCHEMA.MNIT mn, DBXSCHEMA.MBUSAFILIADO mb WHERE mb.mnit_nitchofer=mn.mnit_nit and mcat_placa='"+placa+"'");
			return(dsNIT);
		}
		#endregion AJAX
		
		#region DropDownLists
		//Cambia la agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ddlRutaPrincipal.Items.Clear();
			pnlViaje.Visible=false;
			pnlElementos.Visible=false;
			pnlRuta.Visible=false;
			pnlSeleccionar.Visible=true;
			btnSeleccionar.Visible=true;
			txtNITTiquetero.Enabled=true;
			//txtPrimerTiquete.Enabled=true;
			if(ddlAgencia.Items.Count==0)return;
			if (this.ddlAgencia.Items.Count != 0)
			{
				string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
				string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			
				CargarRutasPrincipales(ciudad);
				CargarConceptosGastos();
				CargarConceptosDescuentos();
				CargarEmpleadosAgencias();
			}
		}

		//Cambia la ruta principal
		private void ddlRutaPrincipal_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pnlViaje.Visible=false;
			pnlElementos.Visible=false;
			pnlRuta.Visible=false;
			pnlSeleccionar.Visible=true;
			btnSeleccionar.Visible=true;
			txtNITTiquetero.Enabled=true;
			//txtPrimerTiquete.Enabled=true;

			//if(ddlRutaPrincipal.Items.Count==0)return;

			//string ruta=ddlRutaPrincipal.SelectedValue;
			//if(ruta.Length==0)return;
			if ((ddlRutaPrincipal.Items.Count != 0) && (this.ddlRutaPrincipal.SelectedValue.Length != 0))
			{
				//Tabla con agencias de destino
				DataSet dsDestinosA=new DataSet();
				DBFunctions.Request(dsDestinosA,IncludeSchema.NO,
					"select rtrim(char(mag_codigo)) as valor, mage_nombre as texto from DBXSCHEMA.magencia;");
				DataRow drDA=dsDestinosA.Tables[0].NewRow();
				drDA[0]="";
				drDA[1]="---seleccione---";
				dsDestinosA.Tables[0].Rows.InsertAt(drDA,0);
				ViewState.Add("dtAgenciasDestino",dsDestinosA.Tables[0]);
			
				pnlRuta.Visible=true;
			}
		}
		#endregion DropDownLists

		#region Datagrids


		private void dgrTiquetes_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				//DropDownList ddlTipo=(DropDownList)e.Item.FindControl("ddlTipoTiqueteEsp");
				DropDownList dlDestinos=(DropDownList)e.Item.FindControl("ddlDestinoTiquete");
				DropDownList dlAnulacion=(DropDownList)e.Item.FindControl("ddlConceptoAnulacion");

				
				DropDownList dlPersonalAgencia=(DropDownList)e.Item.FindControl("ddlNitResponsableTiq");
				dlPersonalAgencia.DataSource=ViewState["dtEmpleadosAgencia"];
				dlPersonalAgencia.DataTextField="NOMBRE";
				dlPersonalAgencia.DataValueField="NIT";
				dlPersonalAgencia.DataBind();
				dlPersonalAgencia.SelectedIndex=dlPersonalAgencia.Items.IndexOf(dlPersonalAgencia.Items.FindByValue(txtNITTiquetero.Text));

				//TextBox txtResponsable=(TextBox)e.Item.FindControl("txtResponsable");
				TextBox txtValor=(TextBox)e.Item.FindControl("txtValorTiquete");
				TextBox txtCantidad=(TextBox)e.Item.FindControl("txtCantidadTiquete");
				TextBox txtTotal=(TextBox)e.Item.FindControl("txtTotalTiquete");
				CheckBox chkAnulado=(CheckBox)e.Item.FindControl("chkAnlulado");
				//	CheckBox chkPendiente=(CheckBox)e.Item.FindControl("chkPendiente");
				txtValor.Attributes.Add("onkeyup","NumericMask(this);totalTicketes('"+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"');");
				txtCantidad.Attributes.Add("onkeyup","NumericMask(this);totalTicketes('"+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"');");
				chkAnulado.Attributes.Add("onclick","verConcepto(this,'"+dlAnulacion.ClientID+"');");
				dlDestinos.DataSource=ViewState["dtDestino"];
				dlDestinos.DataTextField="texto";
				dlDestinos.DataValueField="valor";
				dlDestinos.DataBind();
				dlAnulacion.DataSource=ViewState["dtConceptoAnula"];
				dlAnulacion.DataTextField="texto";
				dlAnulacion.DataValueField="valor";
				dlAnulacion.DataBind();
				//txtResponsable.Enabled=false;
				strTotalTiquetes+=txtTotal.ClientID+",";
				strAnulacionTiquetes+=chkAnulado.ClientID+"@"+dlAnulacion.ClientID+",";
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotalTiquetes+=((TextBox)e.Item.FindControl("txtTotalTiquetes")).ClientID;
				ViewState["strTotalTiquetes"]=strTotalTiquetes;
				ViewState["strAnulacionTiquetes"]=strAnulacionTiquetes;
			}
		}
		/*
		private void dgrTiqueteEsps_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList ddlTipo=(DropDownList)e.Item.FindControl("ddlTipoTiqueteEsp");
				DropDownList dlDestinos=(DropDownList)e.Item.FindControl("ddlDestinoTiqueteEsp");
				//TextBox txtNITVentaOtro=(TextBox)e.Item.FindControl("txtNITVentaOtro");
				TextBox txtValor=(TextBox)e.Item.FindControl("txtValorTiqueteEsp");
				//TextBox txtCantidad=(TextBox)e.Item.FindControl("txtCantidadTiqueteEsp");
				TextBox txtTotal=(TextBox)e.Item.FindControl("txtTotalTiqueteEsp");
				txtValor.Attributes.Add("onkeyup","NumericMask(this);totalTicketesEsp('1','"+txtValor.ClientID+"','"+txtTotal.ClientID+"');");
				//txtCantidad.Attributes.Add("onkeyup","NumericMask(this);totalTicketesEsp("+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"',2);");
				dlDestinos.DataSource=ViewState["dtDestino"];
				dlDestinos.DataTextField="texto";
				dlDestinos.DataValueField="valor";
				dlDestinos.DataBind();
				  
				dlNitResponsable.DataSource = this.ViewState["dtPersonalAgencia"];
				dlNitResponsable.DataTextField = "texto";
				dlNitResponsable.DataValueField = "valor";
				dlNitResponsable.DataBind();
				dlNitResponsable.SelectedIndex = dlNitResponsable.Items.IndexOf(dlNitResponsable.Items.FindByValue(this.txtNITTiquetero.Text));
				 
				//ddlTipo.Attributes.Add("onchange","verNIT(this.value,'"+txtNITVentaOtro.ClientID+"');totalTicketesEsp('"+ddlTipo.ClientID+"','"+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"',2);");
				//strTiposTiquetesEsp+=ddlTipo.ClientID+"@"+txtNITVentaOtro.ClientID+",";
				strTotalTiquetesEsp+=txtTotal.ClientID+",";
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotalTiquetesEsp+=((TextBox)e.Item.FindControl("txtTotalTiqueteEsps")).ClientID;
				ViewState["strTotalTiquetesEsp"]=strTotalTiquetesEsp;
				//ViewState["strTiposTiquetesEsp"]=strTiposTiquetesEsp;
			}
		}
		*/		 
		
		private void dgrEncomiendas_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlDestinos=(DropDownList)e.Item.FindControl("ddlDestinoEncomienda");
				dlDestinos.DataSource=ViewState["dtDestino"];
				dlDestinos.DataTextField="texto";
				dlDestinos.DataValueField="valor";
				dlDestinos.DataBind();

				DropDownList dlPersonalAgencia=(DropDownList)e.Item.FindControl("ddlNitResponsable");
				dlPersonalAgencia.DataSource = ViewState["dtEmpleadosAgencia"];
				dlPersonalAgencia.DataTextField="NOMBRE";
				dlPersonalAgencia.DataValueField="NIT";
				dlPersonalAgencia.DataBind();
				dlPersonalAgencia.SelectedIndex=dlPersonalAgencia.Items.IndexOf(dlPersonalAgencia.Items.FindByValue(txtNITTiquetero.Text));

				TextBox txtValor=((TextBox)e.Item.FindControl("txtValorEncomienda"));
				TextBox txtDescripcion=((TextBox)e.Item.FindControl("TextDescripcionEncomienda"));
				txtValor.Attributes.Add("onkeyup","NumericMask(this);totalesPrts(tEncomiendas);");
				strTotalEncomiendas+=txtValor.ClientID+",";

			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotalEncomiendas+=((TextBox)e.Item.FindControl("txtTotalEncomiendas")).ClientID;
				ViewState["strTotalEncomiendas"]=strTotalEncomiendas;
			}
		}

		/*
		private void dgrGiros_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlDestinos=(DropDownList)e.Item.FindControl("ddlAgenciaDestinoGiro");
				dlDestinos.DataSource=ViewState["dtAgenciasDestino"];
				dlDestinos.DataTextField="texto";
				dlDestinos.DataValueField="valor";
				dlDestinos.DataBind();
				TextBox txtValor=((TextBox)e.Item.FindControl("txtValorGiro"));
				TextBox txtCosto=((TextBox)e.Item.FindControl("txtCostoGiro"));
				txtCosto.Attributes.Add("onkeyup","NumericMask(this);totalesPrts(tCGiros);");
				txtValor.Attributes.Add("onkeyup","NumericMask(this);costoGiro(this,'"+txtCosto.ClientID+"');totalesPrts(tGiros);totalesPrts(tCGiros);");
				strTotalGiros+=txtValor.ClientID+",";
				strCostoGiros+=txtCosto.ClientID+",";

			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotalGiros+=((TextBox)e.Item.FindControl("txtTotalGiros")).ClientID;
				strCostoGiros+=((TextBox)e.Item.FindControl("txtTotalCostoGiros")).ClientID;
				ViewState["strTotalGiros"]=strTotalGiros;
				ViewState["strCostoGiros"]=strCostoGiros;
			}
		}
		*/
		private void dgrPagos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlConceptos=(DropDownList)e.Item.FindControl("ddlConceptoPago");
				dlConceptos.DataSource=ViewState["dtConcepto"];
				dlConceptos.DataTextField="texto";
				dlConceptos.DataValueField="valor";
				dlConceptos.DataBind();

				DropDownList dlPersonalAgencia=(DropDownList)e.Item.FindControl("ddlResponsableAnticipo");
				dlPersonalAgencia.DataSource=ViewState["dtEmpleadosAgencia"];
				dlPersonalAgencia.DataTextField="NOMBRE";
				dlPersonalAgencia.DataValueField="NIT";
				dlPersonalAgencia.DataBind();
				dlPersonalAgencia.SelectedIndex=dlPersonalAgencia.Items.IndexOf(dlPersonalAgencia.Items.FindByValue(txtNITTiquetero.Text));

				TextBox txtValor=((TextBox)e.Item.FindControl("txtValorPago"));
				txtValor.Attributes.Add("onkeyup","NumericMask(this);totalesPrts(tPagos);");
				strTotalPagos+=txtValor.ClientID+",";

			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotalPagos+=((TextBox)e.Item.FindControl("txtTotalPagos")).ClientID;
				ViewState["strTotalPagos"]=strTotalPagos;
			}
		}

		private void dgrDescuentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlConceptosDescuentos=(DropDownList)e.Item.FindControl("ddlConceptoDescuento");
				dlConceptosDescuentos.DataSource=ViewState["dtConceptoDescuento"];
				dlConceptosDescuentos.DataTextField="texto";
				dlConceptosDescuentos.DataValueField="valor";
				dlConceptosDescuentos.DataBind();

				DropDownList dlPersonalAgencia=(DropDownList)e.Item.FindControl("ddlResponsableDescuento");
				dlPersonalAgencia.DataSource=ViewState["dtEmpleadosAgencia"];
				dlPersonalAgencia.DataTextField="NOMBRE";
				dlPersonalAgencia.DataValueField="NIT";
				dlPersonalAgencia.DataBind();
				dlPersonalAgencia.SelectedIndex=dlPersonalAgencia.Items.IndexOf(dlPersonalAgencia.Items.FindByValue(txtNITTiquetero.Text));

				TextBox txtValor=((TextBox)e.Item.FindControl("txtValorDescuento"));
				txtValor.Attributes.Add("onkeyup","NumericMask(this);totalesPrts(tDescuentos);");
				strTotalDescuentos+=txtValor.ClientID+",";

			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				strTotalDescuentos+=((TextBox)e.Item.FindControl("txtTotalDescuentos")).ClientID;
				ViewState["strTotalDescuentos"]=strTotalDescuentos;
			}
		}
		#endregion Datagrids
		
		//Seleccionar planilla
		private void btnSeleccionarPlanilla_Click(object sender, System.EventArgs e)
		{
			string ruta=ddlRutaPrincipal.SelectedValue;
			string placa=txtPlaca.Text.Trim();
			string nitConductor=txtConductor.Text.Trim();
			string nitRelevador=txtRelevador.Text.Trim();
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			int planilla;
			DateTime fecha;
			string viaje;

			//planilla manual
			if(!DBFunctions.RecordExist("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';"))
			{
				Response.Write("<script language='javascript'>alert('No se ha definido el NIT para planillas manuales.');</script>");
				return;
			}

			//Nit planilla manual
			if(!DBFunctions.RecordExist("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';"))
			{
				Response.Write("<script language='javascript'>alert('No se ha definido el NIT para planillas manuales.');</script>");
				return;
			}

			//Verificar placa
			if(!DBFunctions.RecordExist("select mcat_placa from dbxschema.mbusafiliado where mcat_placa='"+placa+"';"))
			{
				Response.Write("<script language='javascript'>alert('La placa seleccionada no se encuentra registrada.');</script>");
				return;
			}
			//Conductor/relevadores
			//Validar conductor Codmodin a NULL
			if(nitConductor == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				nitConductor = "";
			}
			//Validar conductor
			if(nitConductor.Length>0 && !DBFunctions.RecordExist("Select MNIT_NIT FROM DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+nitConductor+"' AND PCAR_CODICARGO='CO'"))
			{
				Response.Write("<script language='javascript'>alert('No se encontro el conductor.');</script>");
				return;
			}
			
			if(nitRelevador == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				nitRelevador = "";
			}
			if(nitConductor.Length==0 && nitRelevador.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe dar el conductor o el relevador.');</script>");
				return;
			}
			//Conductor
			nitConductor=(nitConductor.Length==0?"NULL":"'"+nitConductor+"'");
		
			//Relevador
			nitRelevador=(nitRelevador.Length==0)?"NULL":"'"+nitRelevador+"'";
			
			if(nitConductor.Equals(nitRelevador.Replace("'","")))
			{
				Response.Write("<script language='javascript'>alert('El conductor y el relevador son la misma persona.');</script>");
				return;
			}
			/*Número inicial tiquete
			if(txtPrimerTiquete.Text.Trim().Length==0)txtPrimerTiquete.Text="1";
			try{int.Parse(txtPrimerTiquete.Text.Trim());}
			catch
			{
				Response.Write("<script language='javascript'>alert('Número de primer tiquete no válido.');</script>");
				return;
			}
			*/
			//Validar Fecha
			try
			{
				fecha=Convert.ToDateTime(txtFecha.Text);}
			catch
			{
				Response.Write("<script language='javascript'>alert('Debe dar una fecha válida.');</script>");
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

			//Validar ruta
			if(ruta.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe seleccionar el destino.');</script>");
				return;
			}
			
			//Validar planilla
			try
			{
				planilla=Convert.ToInt32(txtPlanilla.Text.Trim());
				if(planilla<0)throw(new Exception());
			}
			catch
			{
				Response.Write("<script language='javascript'>alert('Debe dar el número de planilla, este debe ser un número entero.');</script>");
				return;
			}
			//planilla manual
			if(DBFunctions.RecordExist("SELECT mviaje_numero from dbxschema.mplanillaviaje where MPLA_CODIGO="+planilla+";"))
			{
				Response.Write("<script language='javascript'>alert('El numero de la planilla ya existe.');</script>");
				return;
			}
			viaje=Viajes.TraerSiguienteViaje(ruta);
			lblNumViaje.Text=viaje;

			//El viaje no debe estar planillado, aunque puede existir
			if(DBFunctions.RecordExist("SELECT mviaje_numero from dbxschema.mplanillaviaje where mrut_codigo='"+ruta+"' and mviaje_numero="+viaje+";"))
			{
				Response.Write("<script language='javascript'>alert('El viaje ya fue planillado para la ruta seleccionada.');</script>");
				return;
			}
			
			//Ciudad origen
			string ciudadO=DBFunctions.SingleData("Select pciu_codigo from dbxschema.magencia where mag_codigo="+agencia+";");
			
			//Destinos de items (de las rutas con origen en la ciudad que pertenecen a la ruta principal)
			DataSet dsDestinos=new DataSet();
			DBFunctions.Request(dsDestinos,IncludeSchema.NO,
				"select distinct mr.mrut_codigo as valor, pc.pciu_nombre as texto "+
				"from DBXSCHEMA.mrutas mr, DBXSCHEMA.pciudad pc, DBXSCHEMA.MRUTA_INTERMEDIA mi "+
				"where "+
				"mi.mruta_principal='"+ruta+"' and mi.mruta_secundaria=mr.mrut_codigo and mr.pciu_cod='"+ciudadO+"' and mr.pciu_coddes=pc.pciu_codigo "+
				"order by mr.mrut_codigo;");
			DataRow drDA=dsDestinos.Tables[0].NewRow();
			drDA[0]="";
			drDA[1]="---seleccione---";
			dsDestinos.Tables[0].Rows.InsertAt(drDA,0);
			ViewState.Add("dtDestino",dsDestinos.Tables[0]);

			ddlAgencia.Enabled=false;
			txtNITTiquetero.Enabled=false;
			txtPlanilla.Enabled=false;
			txtPlaca.Enabled=false;
			txtPlacaa.Enabled=false;
			txtFecha.Enabled=false;
			ddlHora.Enabled=false;
			ddlMinuto.Enabled=false;
			ddlRutaPrincipal.Enabled=false;
			txtConductor.Enabled=false;
			txtConductora.Enabled=false;
			txtRelevador.Enabled=false;
			txtRelevadora.Enabled=false;
			pnlSeleccionar.Visible=false;
			//txtPrimerTiquete.Enabled=false;
			pnlElementos.Visible=true;
			btnGrabar.Visible=false;
			MostrarTablas();
		}
		//Planillar
		private void btnGrabar_Click(object sender, System.EventArgs e)
		{
			VerificarPlanilla("G");
			
		}
		private void btnPlanillar_Click(object sender, System.EventArgs e)
		{
			VerificarPlanilla("V");
		}
		private void VerificarPlanilla(string Opcion)
		{
			#region Validaciones Generales
			ArrayList sqlUpd=new ArrayList();
			string errores="";
			string viaje;
			string planilla=txtPlanilla.Text.Trim();
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			string placa=txtPlaca.Text.Trim();
			string tiquetero=txtNITTiquetero.Text.Trim();
			long numDocumento=0;
			DateTime fecha=Convert.ToDateTime(txtFecha.Text);
			string conductor=txtConductor.Text.Trim();
			string relevador=txtRelevador.Text.Trim();
			string rutaP=ddlRutaPrincipal.SelectedValue;
			if(conductor == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				conductor = "";
			}
			if(relevador == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				relevador = "";
			}
			relevador=(relevador.Length==0)?"NULL":"'"+relevador+"'";
			conductor=(conductor.Length==0)?"NULL":"'"+conductor+"'";

			//Responsables
			string nitResponsableAgencia=ViewState["NitResponsableAgencia"].ToString();
			string nitResponsable=ViewState["NitResponsable"].ToString();

			//Nit general planilla manual
			string nitPlanillaManual=DBFunctions.SingleData("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';");

			//Validar no. planilla
			if(DBFunctions.RecordExist("select mpla_codigo from dbxschema.mplanillaviaje where mpla_codigo="+planilla+";"))
			{
				Response.Write("<script language='javascript'>alert('Ya existe el número de planilla seleccionado, vuelva a intentar con otro.');</script>");
				return;}

			//papeleria planilla?
			if(DBFunctions.RecordExist("Select NUM_DOCUMENTO from DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='PLA' AND NUM_DOCUMENTO="+planilla))
			{
				Response.Write("<script language='javascript'>alert('Existe control papeleria disponible para la planilla, Revise base de datos.');</script>");
				return;
			}

			viaje=Viajes.TraerSiguienteViaje(rutaP);
			if(DBFunctions.RecordExist("SELECT mviaje_numero from dbxschema.mplanillaviaje where mrut_codigo='"+rutaP+"' and mviaje_numero="+viaje+";"))
			{
				Response.Write("<script language='javascript'>alert('El viaje ya fue planillado para la ruta seleccionada.');</script>");
				return;}

			//Hora
			int hora=(int.Parse(ddlHora.SelectedValue)*60)+int.Parse(ddlMinuto.SelectedValue);
			
			//Afiliado
			string afiliado=DBFunctions.SingleData("SELECT MNIT_ASOCIADO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"'");
			afiliado=(afiliado.Length==0)?"NULL":"'"+afiliado+"'";
			
			//Configuracion
			string configBus=DBFunctions.SingleData("Select MCON_COD from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';");
			if(configBus.Length==0)configBus="NULL";
			
			//Insertar viaje
			sqlUpd.Add("INSERT INTO dbxschema.MVIAJE values('"+rutaP+"',"+viaje+",'"+fecha.ToString("yyyy-MM-dd")+"',"+hora+",NULL,"+agencia+",'"+placa+"',"+configBus+","+conductor+","+relevador+",NULL,"+afiliado+",'"+tiquetero+"',"+hora+",NULL,NULL,NULL,NULL,NULL,'A',NULL);");

			//Insertar planilla
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('PLA',"+planilla+",'M',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+tiquetero+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
			sqlUpd.Add("INSERT INTO dbxschema.MPLANILLAVIAJE values("+planilla+",'"+rutaP+"',"+viaje+","+agencia+",'"+tiquetero+"','"+fecha.ToString("yyyy-MM-dd")+"',NULL,0,0,0,0,NULL);");
			#endregion


	
			int fila=1,linea=1;
			//Totales
			double totalTiquetes=0,totalEncomiendas=0,totalGiros=0,totalPagos=0,valorSeguro;
			//Capacidad del bus
			int capacidadBus=Convert.ToInt16(DBFunctions.SingleData("Select CAPACIDAD_PASAJEROS from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';"));
			//Seguro
			try
			{
				valorSeguro=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_SEGURO FROM DBXSCHEMA.CTRANSPORTES;"));
			}
			catch
			{
				valorSeguro=0;
			}
			#region Tiquetes
			ArrayList arrTiquetes=new ArrayList();
			int prefijo=0, totalPasajeros=0;
			
			//Prefijo tiquetes
			if(ddlAgencia.SelectedValue.EndsWith("|"))
				prefijo=Convert.ToInt16(agencia);
	 		
			foreach(DataGridItem dgrI in dgrTiquetes.Items)
			{
				bool errLin=false;
				
				TextBox txtTiquete=(TextBox)dgrI.FindControl("txtNumTiquete");
				//TextBox txtResponsable=(TextBox)dgrI.FindControl("txtResponsable");
				DropDownList ddlPersonalAgencia=(DropDownList)dgrI.FindControl("ddlNitResponsableTiq");
				DropDownList ddlDestino=(DropDownList)dgrI.FindControl("ddlDestinoTiquete");
				TextBox txtCantidadTiquete=(TextBox)dgrI.FindControl("txtCantidadTiquete");
				TextBox txtValorTiquete=(TextBox)dgrI.FindControl("txtValorTiquete");
				TextBox txtTotalTiquete=(TextBox)dgrI.FindControl("txtTotalTiquete");
				CheckBox chkAnula=(CheckBox)dgrI.FindControl("chkAnlulado");
				CheckBox chkPend=(CheckBox)dgrI.FindControl("chkPendiente");
				DropDownList ddlConceptoAnulacion=(DropDownList)dgrI.FindControl("ddlConceptoAnulacion");
				
				if((txtTiquete.Text.Trim().Length>0 && (ddlDestino.SelectedValue.Length>0 || txtCantidadTiquete.Text.Trim().Length>0 || txtValorTiquete.Text.Trim().Length>0)))
				{
					int cantidad=0;
					double precio=0;
					bool anula=false;
					string ruta=ddlDestino.SelectedValue;
					
					//No. tiquete
					try
					{
						numDocumento=(prefijo*(long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete))+int.Parse(txtTiquete.Text.Trim());}
					catch
					{
						errores+="Número de tiquete no válido en la línea "+fila+". ";errLin=true;}
					
					//Pendiente?
					//if(chkPend.Checked)
					//{
					//	errores+="La línea "+fila+" ha sido marcada como pendiente pero ingresó información. ";errLin=true;}
					
					//Ruta
					if(ruta.Length==0)
					{
						errores+="Destino de tiquete no válido en la línea "+fila+". ";errLin=true;}
					
					//Repetido?
					if(!errLin && arrTiquetes.Contains(numDocumento))
					{
						errores+="El número de tiquete en la línea "+fila+" ya ha sido ingresado. ";errLin=true;}
					else
						arrTiquetes.Add(numDocumento);
					
					//Cantidad
					try
					{
						cantidad=int.Parse(txtCantidadTiquete.Text.Replace(",","").Trim());
						if(cantidad<=0||cantidad>capacidadBus)throw(new Exception());}
					catch
					{
						errores+="Cantidad de tiquetes no válida o supera la capacidad del bus en la línea "+fila+". ";errLin=true;}
					
					//Precio
					try
					{
						precio=double.Parse(txtValorTiquete.Text.Replace(",","").Trim());
						if(precio<=0)throw(new Exception());}
					catch
					{
						errores+="Precio de tiquete no válido en la línea "+fila+". ";
						errLin=true;
					}
					
					//Anulado?
					if(!errLin && chkAnula.Checked)
					{
						if(ddlConceptoAnulacion.SelectedValue.Length==0)
						{
							errores+="Debe seleccionar la causa de la anulacion en la línea "+fila+". ";
							errLin=true;
						}
						else anula=true;
					}

					//Verificar rango precio
					if(!errLin)
						if(!DBFunctions.RecordExist("SELECT MRUT_VALSUG FROM DBXSCHEMA.MRUTAS WHERE MRUT_VALMAX>="+precio+" AND MRUT_VALMIN<="+precio+" AND MRUT_CODIGO='"+ruta+"';"))
						{
							txtValorTiquete.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT MRUT_VALMAX FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+ruta+"';")).ToString("###,###,##0.##");
							errores+="Precio de tiquete fuera del rango válido en la línea "+fila+", el precio ha sido remplazado. ";
							strActScript+="totalTicketes('"+txtCantidadTiquete.ClientID+"','"+txtValorTiquete.ClientID+"','"+txtTotalTiquete.ClientID+"');";
							errLin=true;
						}

					//Verificar Tiquete
					
					if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MTIQUETE_VIAJE WHERE TDOC_CODIGO='TIQ'AND NUM_DOCUMENTO="+numDocumento+";" ))
					{
						errores+="Ya existe el tiquete a ingresar de la línea "+fila+". ";
						errLin=true;
					}
					
					
					if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+numDocumento+";"))
					{
						errores+="Existe control de papeleria para el tiquete de la línea "+fila+" Revisar Base de Datos. ";
						errLin=true;}
					
					//Insertar tiquete
					if(!errLin)
					{
						if(anula)
						{
							//ANULAR
							sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('TIQ',"+numDocumento+",'M',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ddlPersonalAgencia.SelectedValue+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,0,"+ddlConceptoAnulacion.SelectedValue+",'"+ddlPersonalAgencia.SelectedValue+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
							sqlUpd.Add("INSERT INTO dbxschema.mtiquete_viaje values('TIQ',"+numDocumento+","+planilla+","+linea+",'"+ruta+"','"+ddlPersonalAgencia.SelectedValue+"',"+cantidad+",'"+nitPlanillaManual+"',"+valorSeguro.ToString("0")+","+precio+","+precio*cantidad+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','A');");
							sqlUpd.Add("INSERT INTO dbxschema.MTIQUETE_VIAJE_ANULADO values('TIQ',"+numDocumento+","+ddlConceptoAnulacion.SelectedValue+",'"+fecha.ToString("yyyy-MM-dd")+"','"+ddlPersonalAgencia.SelectedValue+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
							
						}
						else
						{
							sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('TIQ',"+numDocumento+",'M',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ddlPersonalAgencia.SelectedValue+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
							sqlUpd.Add("INSERT INTO dbxschema.mtiquete_viaje values('TIQ',"+numDocumento+","+planilla+","+linea+",'"+ruta+"','"+ddlPersonalAgencia.SelectedValue+"',"+cantidad+",'"+nitPlanillaManual+"',"+valorSeguro.ToString("0")+","+precio+","+precio*cantidad+",'"+fecha.ToString("yyyy-MM-dd")+"','V');");
							totalTiquetes+=cantidad*precio;
							totalPasajeros+=cantidad;
						}
						
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			} 
			#endregion

			
			//Cantidad Total
			if(totalPasajeros>capacidadBus)
				errores+="La cantidad total de pasajeros supera la capacidad del bus. ";

			#region Encomiendas
			double porcentajeGiro=Convert.ToDouble(ViewState["PorcentajeGiro"]);
			fila=1;
			CheckBox chkAsignar;
			ArrayList arrEncomiendas=new ArrayList();
			
			foreach(DataGridItem dgrI in dgrEncomiendas.Items)
			{
				bool errLin=false;
				string TipoDoc;
				string ruta;
				TextBox txtNumEncomienda=(TextBox)dgrI.FindControl("txtNumEncomienda");
				DropDownList ddlDestinoEncomienda=(DropDownList)dgrI.FindControl("ddlDestinoEncomienda");
				DropDownList ddlTipo=(DropDownList)dgrI.FindControl("ddlTipo");
				DropDownList ddlNitResponsable=(DropDownList)dgrI.FindControl("ddlNitResponsable");
				TextBox txtValorEncomienda=(TextBox)dgrI.FindControl("txtValorEncomienda");
				TextBox txtTotalEncomiendas=(TextBox)dgrI.FindControl("txtTotalEncomiendas");
				//TextBox txtNitResponsable=(TextBox)dgrI.FindControl("txtNitResponsable");
				TextBox txtDescripcionEncomienda=(TextBox)dgrI.FindControl("txtDescripcionEncomienda");
				chkAsignar = (CheckBox)dgrI.FindControl("chkAsignacion");
				
				if(txtNumEncomienda.Text.Trim().Length>0 || ddlDestinoEncomienda.SelectedValue.Length>0  || txtValorEncomienda.Text.Trim().Length>0)
				{
					double precio=0,costoGiro=0 ;
					
					//Numero encomienda
					try
					{
						numDocumento=(long)int.Parse(txtNumEncomienda.Text.Trim());}
					catch
					{
						errores+="Número de encomienda no válido en la línea "+fila+". ";
						errLin=true;
					}
					if(ddlTipo.SelectedValue=="E")	
                      	TipoDoc = "REM";
					else
					    TipoDoc = "GIR";
					if(chkAsignar.Checked)
					{
						if(ddlTipo.SelectedValue=="E")
						{
							DataSet dsEncomienda=new DataSet();
							
							DBFunctions.Request(dsEncomienda,IncludeSchema.NO,"select TDOC_CODIGO, NUM_DOCUMENTO, MAG_RECIBE,MNIT_RESPONSABLE_RECIBE,coalesce(MPLA_CODIGO,0) as PLANILLA, MRUT_CODIGO,DESCRIPCION_CONTENIDO,VALOR_TOTAL, rtrim(TEST_CODIGO) as TEST_CODIGO , coalesce(MCAT_PLACA,'') as MCAT_PLACA"+
								" from DBXSCHEMA.MENCOMIENDAS where TDOC_CODIGO= '"+TipoDoc+"' and NUM_DOCUMENTO = "+numDocumento+"");
							if (dsEncomienda.Tables[0].Rows.Count > 0)
							{		
								txtDescripcionEncomienda.Text = dsEncomienda.Tables[0].Rows[0]["DESCRIPCION_CONTENIDO"].ToString();
								double valorEncomienda = Convert.ToDouble(dsEncomienda.Tables[0].Rows[0]["VALOR_TOTAL"].ToString());
								txtValorEncomienda.Text  =  valorEncomienda.ToString("###,###,##0");
								//ddlNitResponsable.SelectedIndex=ddlNitResponsable.Items.IndexOf(ddlNitResponsable.Items.FindByValue(dsEncomienda.Tables[0].Rows[0]["MNIT_RESPONSABLE_RECIBE"].ToString()));
								ddlDestinoEncomienda.SelectedIndex=ddlDestinoEncomienda.Items.IndexOf(ddlDestinoEncomienda.Items.FindByValue(dsEncomienda.Tables[0].Rows[0]["MRUT_CODIGO"].ToString()));
								
								long PlanillaEncomienda = Convert.ToInt32(dsEncomienda.Tables[0].Rows[0]["PLANILLA"].ToString());
								if (PlanillaEncomienda > 0)
								{
									errores+="Encomienda a Asignar YA Asignada a planilla en la línea "+fila+". ";
									errLin=true;
								}
								string PlacaEncomienda = dsEncomienda.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
								if (PlacaEncomienda.Length > 0 && PlacaEncomienda != placa )
								{
									errores+="Encomienda tiene Asignada otra placa en la línea "+fila+". ";
									errLin=true;
								}
								string Estado = dsEncomienda.Tables[0].Rows[0]["TEST_CODIGO"].ToString();
								if (Estado != "A")
								{
									errores+="Encomienda a Asignar NO esta Disponible en la línea "+fila+". ";
									errLin=true;
								}
								string AgenciaEncomienda = dsEncomienda.Tables[0].Rows[0]["MAG_RECIBE"].ToString();
								if (AgenciaEncomienda != agencia )
								{
									errores+="Encomienda de otra agencia en la línea "+fila+". ";
									errLin=true;
								}
							}
							else
							{
								errores+="Encomienda a Asignar no Existe en la línea "+fila+". ";
								errLin=true;
							
							}
						}                        
						else			
						{
							// giros
							DataSet dsGiros=new DataSet();
							
							DBFunctions.Request(dsGiros,IncludeSchema.NO,"select TDOC_CODIGO, NUM_DOCUMENTO, MAG_AGENCIA_ORIGEN,MNIT_RESPONSABLE_RECIBE,coalesce(MPLA_CODIGO,0) as PLANILLA, MRUT_CODIGO,'Giro ' as DESCRIPCION_CONTENIDO,VALOR_IVA+COSTO_GIRO as VALOR_TOTAL, rtrim(TEST_CODIGO) as TEST_CODIGO "+
								" from DBXSCHEMA.MGIROS where TDOC_CODIGO= '"+TipoDoc+"' and NUM_DOCUMENTO = "+numDocumento+"");
							if (dsGiros.Tables[0].Rows.Count > 0)
							{		
								txtDescripcionEncomienda.Text = dsGiros.Tables[0].Rows[0]["DESCRIPCION_CONTENIDO"].ToString();
								double valorEncomienda = Convert.ToDouble(dsGiros.Tables[0].Rows[0]["VALOR_TOTAL"].ToString());
								txtValorEncomienda.Text  =  valorEncomienda.ToString("###,###,##0");
								//ddlNitResponsable.SelectedIndex=ddlNitResponsable.Items.IndexOf(ddlNitResponsable.Items.FindByValue(dsGiros.Tables[0].Rows[0]["MNIT_RESPONSABLE_RECIBE"].ToString()));
								ddlDestinoEncomienda.SelectedIndex=ddlDestinoEncomienda.Items.IndexOf(ddlDestinoEncomienda.Items.FindByValue(dsGiros.Tables[0].Rows[0]["MRUT_CODIGO"].ToString()));
								
								long PlanillaGiro = Convert.ToInt32(dsGiros.Tables[0].Rows[0]["PLANILLA"].ToString());
								if (PlanillaGiro > 0)
								{
									errores+="Giro a Asignar YA Asignado a planilla en la línea "+fila+". ";
									errLin=true;
								}
								string Estado = dsGiros.Tables[0].Rows[0]["TEST_CODIGO"].ToString();
								if (Estado != "A")
								{
									errores+="Giro a Asignar NO esta Disponible en la línea "+fila+". ";
									errLin=true;
								}
								string AgenciaGiro = dsGiros.Tables[0].Rows[0]["MAG_AGENCIA_ORIGEN"].ToString();
								if (AgenciaGiro != agencia )
								{
									errores+="Giro a Asignar de otra agencia en la línea "+fila+". ";
									errLin=true;
								}
							}
							else
							{
								errores+="Giro a Asignar no Existe en la línea "+fila+". ";
								errLin=true;
							}
						}
					}
					
					ruta=ddlDestinoEncomienda.SelectedValue;
					
					//Ruta
					if(ruta.Length==0)
					{
						errores+="Destino de encomienda/Giro no válido en la línea "+fila+". ";
						errLin=true;
					}
					//descripcion
					if(txtDescripcionEncomienda.Text.Trim().Length==0)
					{
						errores+="Falta Descripcion de encomienda en la línea "+fila+". ";
						errLin=true;
					}
					
					//Repetido?
					if(!errLin && arrEncomiendas.Contains(numDocumento))
					{
						errores+="El número de encomienda en la línea "+fila+" ya ha sido ingresado. ";errLin=true;}
					else
						arrEncomiendas.Add(numDocumento);
					
					//Precio
					try
					{
						precio=double.Parse(txtValorEncomienda.Text.Replace(",","").Trim());
						if(precio<0)throw(new Exception());
					}
					catch
					{
						errores+="Precio de encomienda/Giro no válido en la línea "+fila+". ";
						errLin=true;
					}

					//Verificar Encomienda
					if(!chkAsignar.Checked)
						if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MENCOMIENDAS WHERE TDOC_CODIGO='REM' AND NUM_DOCUMENTO="+numDocumento+";"))
						{
							errores+="Ya existe la encomienda en la Base de Datos de la línea "+fila+". ";
							errLin=true;
						}
					
					//Insertar
					if(!errLin)
					{
						//Agencia destino
						string agenciaEntrega=DBFunctions.SingleData(
							"SELECT ma.mag_codigo from dbxschema.magencia ma, dbxschema.mrutas mr "+
							"where ma.pciu_codigo=mr.pciu_coddes and mr.mrut_codigo='"+ruta+"' "+
							"FETCH FIRST 1 ROWS ONLY;");
						if(agenciaEntrega.Length==0)agenciaEntrega=agencia;

						//Responsable entrega
						string responsableEntrega=DBFunctions.SingleData("SELECT mnit_encargado from dbxschema.magencia where mag_codigo="+agenciaEntrega+";");
						if(responsableEntrega.Length==0)responsableEntrega=nitResponsable;
						
						if(ddlTipo.SelectedValue=="E")
						{
							totalEncomiendas+=precio;
							//Insertar encomienda
							if(!chkAsignar.Checked)
							{
								sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('REM',"+numDocumento+",'M',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ddlNitResponsable.SelectedValue+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
								sqlUpd.Add("INSERT INTO dbxschema.MENCOMIENDAS values('REM',"+numDocumento+",'M',"+agencia+",'"+ddlNitResponsable.SelectedValue+"',"+agenciaEntrega+",'"+responsableEntrega+"',"+planilla+","+linea+",'"+ruta+"','"+nitPlanillaManual+"',NULL,NULL,'"+nitPlanillaManual+"',NULL,NULL,'"+txtDescripcionEncomienda.Text+"',1,0,0,0,0,"+precio+",0,0,"+precio+",'"+fecha.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','E',NULL,'"+placa+"');");
							}
							else
							{
								sqlUpd.Add("UPDATE dbxschema.MENCOMIENDAS SET MPLA_CODIGO = "+planilla+",MCAT_PLACA = '"+placa+"',MNIT_RESPONSABLE_RECIBE = '"+ddlNitResponsable.SelectedValue+"'  WHERE NUM_DOCUMENTO = "+numDocumento+";");
								sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET MPLA_CODIGO="+planilla+" ,MNIT_RESPONSABLE = '"+ddlNitResponsable.SelectedValue+"' WHERE TDOC_CODIGO='REM' AND NUM_DOCUMENTO="+numDocumento+";");
							}
						}												
						else
						{
							costoGiro = precio;
							totalEncomiendas+=costoGiro;
							//Insertar giro
							precio    = Math.Round(precio/porcentajeGiro,2);
							if(!chkAsignar.Checked)
							{             
								sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('REM',"+numDocumento+",'M',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ddlNitResponsable.SelectedValue+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");					
								sqlUpd.Add("INSERT INTO dbxschema.MGIROS values('REM',"+numDocumento+",'G',"+agencia+",'"+ddlNitResponsable.SelectedValue+"',"+agenciaEntrega+",'"+responsableEntrega+"',"+planilla+","+linea+",NULL,'"+nitPlanillaManual+"',NULL,NULL,'"+nitPlanillaManual+"',NULL,NULL,0,"+porcentajeGiro+",0,"+costoGiro+","+precio+",'"+fecha.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','E');");
							}
							else
							{
								sqlUpd.Add("UPDATE dbxschema.MGIROS SET MPLA_CODIGO = "+planilla+",MNIT_RESPONSABLE_RECIBE = '"+ddlNitResponsable.SelectedValue+"'  WHERE NUM_DOCUMENTO = "+numDocumento+";");
								sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET MPLA_CODIGO="+planilla+",MNIT_RESPONSABLE = '"+ddlNitResponsable.SelectedValue+"' WHERE TDOC_CODIGO='GIR' AND NUM_DOCUMENTO="+numDocumento+";");
							} 				
						}	
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}

			
			#endregion


			#region Giros
			/*
			fila=1;
			ArrayList arrGiros=new ArrayList();
			double porcentajeGiro=Convert.ToDouble(ViewState["PorcentajeGiro"]);
			foreach(DataGridItem dgrI in dgrGiros.Items)
			{
				bool errLin=false;
				TextBox txtNumGiro=(TextBox)dgrI.FindControl("txtNumGiro");
				DropDownList ddlAgenciaDestinoGiro=(DropDownList)dgrI.FindControl("ddlAgenciaDestinoGiro");
				TextBox txtValorGiro=(TextBox)dgrI.FindControl("txtValorGiro");
				TextBox txtCostoGiro=(TextBox)dgrI.FindControl("txtCostoGiro");
				
				if(txtNumGiro.Text.Trim().Length>0 || ddlAgenciaDestinoGiro.SelectedValue.Length>0 || txtValorGiro.Text.Trim().Length>0)
				{
					double precio=0,costo=0;
					string agenciaDest=ddlAgenciaDestinoGiro.SelectedValue;
					
					//No. Giro
					try
					{
						numDocumento=(long)int.Parse(txtNumGiro.Text.Trim());}
					catch
					{
						errores+="Número de giro no válido en la línea "+fila+". ";
						errLin=true;}
					
					//Ruta
					if(agenciaDest.Length==0)
					{
						errores+="Agencia de destino incorrecta en el giro de la línea "+fila+". ";
						errLin=true;}
					
					//Repetido?
					if(!errLin && arrGiros.Contains(numDocumento))
					{
						errores+="El número de giro en la línea "+fila+" ya ha sido ingresado. ";errLin=true;}
					else
						arrGiros.Add(numDocumento);
					
					//Precio
					try
					{
						precio=double.Parse(txtValorGiro.Text.Replace(",","").Trim());
						if(precio<0)throw(new Exception());}
					catch
					{
						errores+="Precio de giro no válido en la línea "+fila+". ";
						errLin=true;
					}

					//Costo
					try
					{
						costo=double.Parse(txtCostoGiro.Text.Replace(",","").Trim());
						if(costo<0)throw(new Exception());}
					catch
					{
						errores+="Costo de giro no válido en la línea "+fila+". ";
						errLin=true;
					}
					if(costo>precio)
					{
						errores+="El costo de giro no puede ser mayor a su precio en la línea "+fila+". ";
						errLin=true;
					}
					
					//Verificar papeleria
					if(!DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='GIR' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M' AND MNIT_RESPONSABLE='"+tiquetero+"' AND MAG_CODIGO="+agencia+" AND NUM_DOCUMENTO="+numDocumento+" ORDER BY NUM_DOCUMENTO FETCH FIRST 1 ROWS ONLY"))
					{
						errores+="No hay papeleria disponible para el giro de la línea "+fila+". ";
						errLin=true;
					}

					if(!errLin)
					{
						//Responsable entrega
						string responsableEntrega=DBFunctions.SingleData("SELECT mnit_encargado from dbxschema.magencia where mag_codigo="+agenciaDest+";");
						if(responsableEntrega.Length==0)responsableEntrega=nitResponsable;
						
						//Costo giro
						double costoGiro=costo;

						//Insertar giro
						sqlUpd.Add("INSERT INTO dbxschema.MGIROS values('GIR',"+numDocumento+",'M',"+agencia+",'"+nitResponsableAgencia+"',"+agenciaDest+",'"+responsableEntrega+"',"+planilla+","+linea+",NULL,'"+nitPlanillaManual+"',NULL,NULL,'"+nitPlanillaManual+"',NULL,NULL,0,"+porcentajeGiro+",0,"+costoGiro+","+precio+",'"+fecha.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','E');");
						
						//Modificar papeleria(usada)
						sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',MPLA_CODIGO="+planilla+" WHERE TDOC_CODIGO='GIR' AND NUM_DOCUMENTO="+numDocumento+";");
						
						totalGiros+=precio;
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
			*/
			#endregion
			
			
						
			#region Pagos: Anticipos/Servicios
			fila=1;
			double totalIngresos=0;
			double totalEgresos   = 0;
			ArrayList arrPagos=new ArrayList();
			foreach(DataGridItem dgrI in dgrPagos.Items)
			{
				bool errLin=false;
				TextBox txtNumPago=(TextBox)dgrI.FindControl("txtNumPago");
				DropDownList ddlConceptoPago=(DropDownList)dgrI.FindControl("ddlConceptoPago");
				DropDownList ddlPersonalAgencia=(DropDownList)dgrI.FindControl("ddlResponsableAnticipo");
				TextBox txtValorPago=(TextBox)dgrI.FindControl("txtValorPago");
				TextBox txtTotalPagos=(TextBox)dgrI.FindControl("txtTotalPagos");
				TextBox TxtDescripcionAnticipo=(TextBox)dgrI.FindControl("TxtDescripcionAnticipo");
				if(txtNumPago.Text.Trim().Length>0 || ddlConceptoPago.SelectedValue.Length>0 || txtValorPago.Text.Trim().Length>0)
				{
					double precio=0;
					string concepto=ddlConceptoPago.SelectedValue;
					
					//No. Anticipo
					try
					{
						numDocumento=(long)int.Parse(txtNumPago.Text.Trim());}
					catch
					{
						errores+="Número de Anticipo/servicio/gasto no válido en la línea "+fila+". ";
						errLin=true;}
					
					//Concepto
					if(concepto.Length==0)
					{
						errores+="Concepto del Anticipo/servicio/gasto no válido en la línea "+fila+". ";
						errLin=true;}
					//descripcion
					if(TxtDescripcionAnticipo.Text.Trim().Length==0)
					{
						errores+="Falta Descripcion del Concepto en la línea "+fila+". ";
						errLin=true;
					}
					//Repetido?
					if(!errLin && arrPagos.Contains(numDocumento))
					{
						errores+="El número de Anticipo/servicio/gasto en la línea "+fila+" ya ha sido ingresado. ";errLin=true;}
					else
						arrPagos.Add(numDocumento);
					
					//Precio
					try
					{
						precio=double.Parse(txtValorPago.Text.Replace(",","").Trim());
						if(precio<=0)throw(new Exception());}
					catch
					{
						errores+="Precio de Anticipo/servicio/gasto no válido en la línea "+fila+". ";
						errLin=true;
					}
					 
					//Verificar papeleria
					if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='ANT' AND NUM_DOCUMENTO="+numDocumento+";"))
					{
						errores+="Existe Control papeleria de anticipo/servicio disponible en la línea "+fila+". Revise la base de datos ";
						errLin=true;
					}
					 
					//Verificar Gasto
					if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MGASTOS_TRANSPORTES WHERE TDOC_CODIGO='ANT'  AND NUM_DOCUMENTO="+numDocumento+";" ))
					{
						errores+="Ya existe Documento en la Base de datos de la línea "+fila+". ";
						errLin=true;
					}
					if(!errLin)
					{
						//Tipo unidad
						string tipoUnidad=DBFunctions.SingleData("SELECT tund_consumo FROM DBXSCHEMA.TGASTOS_TRANSPORTES WHERE TCON_CODIGO="+concepto);
						
						//Insertar gasto-ingreso
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('ANT',"+numDocumento+",'M',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ddlPersonalAgencia.SelectedValue+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
						sqlUpd.Add("INSERT INTO dbxschema.MGASTOS_TRANSPORTES values('ANT',"+numDocumento+",'M','"+fecha.ToString("yyyy-MM-dd")+"','"+placa+"',"+planilla+","+linea+","+concepto+","+agencia+",'"+ddlPersonalAgencia.SelectedValue+"',"+conductor+",'"+TxtDescripcionAnticipo.Text+"',1,'"+tipoUnidad+"',"+precio+","+precio+",'P',NULL);");
				        
						
						// verificar si es ingreso o egreso
						string imputacion=DBFunctions.SingleData("select IMPUTACION_CONTABLE from DBXSCHEMA.TCONCEPTOS_TRANSPORTES  where TCON_CODIGO ="+concepto+";");
						if(imputacion=="D")
							totalIngresos+=precio;
						else	
							totalEgresos+=precio;
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";
					}		
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
			#endregion
			#region Descuentos: DescuentosPlanilla
			fila=1;
			double totalDescuentos=0;
			
				ArrayList arrDescuentos=new ArrayList();
			 
				foreach(DataGridItem dgrI in dgrDescuentos.Items)
				{
					bool errLin=false;
					TextBox txtNumDescuento=(TextBox)dgrI.FindControl("txtNumDescuento");
					DropDownList ddlConceptoDescuento=(DropDownList)dgrI.FindControl("ddlConceptoDescuento");
					TextBox txtValorDescuento=(TextBox)dgrI.FindControl("txtValorDescuento");
					TextBox txtTotalDescuentos=(TextBox)dgrI.FindControl("txtTotalDescuentos");
					DropDownList ddlPersonalAgencia=(DropDownList)dgrI.FindControl("ddlResponsableDescuento");
					TextBox txtDescripcionDescuento=(TextBox)dgrI.FindControl("txtDescripcionDescuento");

					if(txtNumDescuento.Text.Trim().Length>0 || ddlConceptoDescuento.SelectedValue.Length>0 || txtValorDescuento.Text.Trim().Length>0)
					{
						double precio=0;
						string concepto=ddlConceptoDescuento.SelectedValue;
					
						//No. Anticipo
						try
						{
							numDocumento=(long)int.Parse(txtNumDescuento.Text.Trim());}
						catch
						{
							errores+="Número Descuento no válido en la línea "+fila+". ";
							errLin=true;}
					
						//Concepto
						if(concepto.Length==0)
						{
							errores+="Concepto Descuento válido en la línea "+fila+". ";
							errLin=true;
						}
					
						//Repetido?
						if(!errLin && arrDescuentos.Contains(numDocumento))
						{
							errores+="El número del Descuento en la línea "+fila+" ya ha sido ingresado. ";errLin=true;
						}
						else
							arrDescuentos.Add(numDocumento);
					
						//Precio
						try
						{
							precio=double.Parse(txtValorDescuento.Text.Replace(",","").Trim());
							if(precio<=0)throw(new Exception());}
						catch
						{
							errores+="Valor del Descuento no válido en la línea "+fila+". ";
							errLin=true;
						}

					//Verificar Descuento
					if(DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MDESCUENTOS_PLANILLA_VIAJE WHERE TDOC_CODIGO='DESCPLA' AND NUM_DOCUMENTO = "+numDocumento+""))
					{
						errores+="EL Numero del Descuento ya existe: línea "+fila+". ";
						errLin=true;
					}
					if(!errLin)
					{
						
						//Insertar Descuento
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MDESCUENTOS_PLANILLA_VIAJE values('DESCPLA',"+numDocumento+",'M','"+fecha.ToString("yyyy-MM-dd")+"',"+planilla+","+linea+","+concepto+",'"+ddlPersonalAgencia.SelectedValue+"','"+txtDescripcionDescuento.Text+"',"+precio+",'A','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"');");
						/*
						//Insertar papeleria(usada)
						sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',MPLA_CODIGO="+planilla+" ,MNIT_RESPONSABLE = '"+txtResponsableAnticipo.Text+"' WHERE TDOC_CODIGO='ANT' AND NUM_DOCUMENTO="+numDocumento+";");
						// verificar si es ingreso o egreso
						*/
						string imputacion=DBFunctions.SingleData("select IMPUTACION_CONTABLE from DBXSCHEMA.TCONCEPTOS_TRANSPORTES  where TCON_CODIGO ="+concepto+";");
						if(imputacion=="D")
						{
							totalIngresos+=precio;
							totalDescuentos-=precio;
						}

						else
						{
							totalEgresos+=precio;
							totalDescuentos+=precio;
						}
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
			#endregion
			
			double totalIngresosPlanilla = totalTiquetes+totalEncomiendas+totalGiros;
			double totalPlanilla  =  totalIngresosPlanilla - totalEgresos;
			lblTotalPlanilla.Text =  totalPlanilla.ToString("###,###,##0");
			if	(totalDescuentos > totalIngresosPlanilla)
			{
				errores+="Valor descuentos mayor a los ingresos Planilla \n Debe ingresar ingresar un anticipo en \n --> Consolidacion Contable-->Adicion Anticipos y servicios Manuales";
			}
			if(errores.Length==0)
			{
				if(Opcion=="G")
				{
					//Actualizar VIAJE
					sqlUpd.Add("UPDATE dbxschema.MVIAJE SET VALOR_INGRESOS="+totalIngresosPlanilla+", VALOR_EGRESOS="+totalEgresos+", ESTADO_VIAJE='T', FECHA_LIQUIDACION='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
					sqlUpd.Add("CALL DBXSCHEMA.ACTUALIZA_PLANILLA("+planilla+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
					//Actualizar PLANILLA: NO. LINEAS, FECHA LIQUID, INGRESOS, EGRESOS, NO. LINEAS
					sqlUpd.Add("UPDATE dbxschema.MPLANILLAVIAJE SET NUMERO_LINEAS="+linea+", FECHA_LIQUIDACION='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE MPLA_CODIGO="+planilla+";");
					//Actualizar PAPELERIA PLANILLA
					sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' WHERE TDOC_CODIGO='PLA' AND NUM_DOCUMENTO="+planilla+";");
					string Comando = "Manual";
                    string Programa = "Comercial.PlanillaManualGaviota";
					if(txtObservacion.Text.Length !=0)
						sqlUpd.Add("INSERT INTO dbxschema.mplanilla_observacion VALUES("+planilla+",'"+txtObservacion.Text+"');");
					if(DBFunctions.Transaction(sqlUpd))
						//	Response.Redirect(indexPage+"?process=Comercial.PlanillaBus&path="+Request.QueryString["path"]+"&pln="+planilla);
						Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.PlanillaBus&pln="+planilla+ "&comando="+Comando+"&programa="+Programa+"&act=4&path=Planillas Manuales");
					else
						lblError.Text=DBFunctions.exceptions;
				}
					
				else
				{
					btnGrabar.Visible=true;
					lblError.Text= "Ok. Oprima Registrar para grabar planilla";
				}
			}
			else 
			{
				btnGrabar.Visible=false;
				strActScript+="alert('"+errores+"');";
				lblError.Text= errores;
			}
		}
		//Seleccionan tiqueteador
		private void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			string nitTiqueteador=txtNITTiquetero.Text.Trim();
			string planilla,viaje,agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			
			//Responsable agencia
			string nitResponsableAgencia=DBFunctions.SingleData("SELECT mnit_encargado from dbxschema.magencia where mag_codigo="+agencia+";");
			if(nitResponsableAgencia.Length==0)
			{
				Response.Write("<script language='javascript'>alert('La agencia no tiene un responsable asociado.');</script>");
				return;}
			ViewState["NitResponsableAgencia"]=nitResponsableAgencia;

			//Responsable usuario
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsableAgencia.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario actual no tiene un nit asociado.');</script>");
				return;}
			
			//Usuario es responsable de la agencia
			/*
					//Usuario es responsable de la agencia
			if(!nitResponsable.Equals(nitResponsableAgencia))
			{
				Response.Write("<script language='javascript'>alert('El usuario actual no es el responsable de la agencia seleccionada.');</script>");
				return;}
			*/
			//Tiqueteador
			if(!DBFunctions.RecordExist("select mnit_nit from dbxschema.mnit where mnit_nit='"+nitTiqueteador+"';"))
			{
				Response.Write("<script language='javascript'>alert('El tiqueteador no existe.');</script>");
				return;
			}
			ViewState["NitResponsable"] = nitResponsable;
			
			//Planilla
			//planilla=DBFunctions.SingleData("Select NUM_DOCUMENTO from DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='PLA' AND TIPO_DOCUMENTO='M' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND MNIT_RESPONSABLE='"+nitTiqueteador+"' AND MAG_CODIGO="+agencia+" order by num_documento fetch first 1 rows only;");
			//if(planilla.Length==0)
			//{
			//	Response.Write("<script language='javascript'>alert('No hay planillas disponibles para el tiqueteador.');</script>");
			//	return;
			//}

			//Viaje
			viaje=Viajes.TraerSiguienteViaje(ddlRutaPrincipal.SelectedValue);
			if(viaje.Length==0)viaje="1";
			//if(planilla.Length==0)
			//	{
			//		Response.Write("<script language='javascript'>alert('No se puede consultar el número de viaje.');</script>");
			//		return;}
			
			//Primer tiquete disponible
			//txtPrimerTiquete.Text=DBFunctions.SingleData("select cast(right(rtrim(char(NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) from DBXSCHEMA.MCONTROL_PAPELERIA where tdoc_codigo='TIQ' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M' AND MNIT_RESPONSABLE='"+nitTiqueteador+"' AND MAG_CODIGO="+agencia+"  order by num_documento fetch first 1 rows only;");
			//if(txtPrimerTiquete.Text.Length==0)txtPrimerTiquete.Text="1";
			
			lblNumViaje.Text=viaje;
			//txtPlanilla.Text=planilla;
			txtNITTiquetero.Enabled=false;
			btnSeleccionar.Visible=false;
			pnlViaje.Visible=true;
			//txtPrimerTiquete.Enabled=true;
		}

		//Mostrar grillas
		private void MostrarTablas()
		{
			//DataSet dsTiquetesDisponibles=new DataSet();
			DataTable dtTiquetes=new DataTable();
			string nitPlanillaManual=DBFunctions.SingleData("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';");
			string nitTiquetero=txtNITTiquetero.Text.Trim(),agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			int prefijo=0;
			if(ddlAgencia.SelectedValue.EndsWith("|"))
				prefijo=Convert.ToInt16(agencia);
			//	long tiqueteIni=(prefijo*(long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete))+int.Parse(txtPrimerTiquete.Text.Trim());
			
			//Tiquetes disponibles
			//DBFunctions.Request(dsTiquetesDisponibles,IncludeSchema.NO,"select cast(right(rtrim(char(NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) as num_documento from DBXSCHEMA.MCONTROL_PAPELERIA where tdoc_codigo='TIQ' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M' AND MNIT_RESPONSABLE='"+nitTiquetero+"' AND MAG_CODIGO="+agencia+" and num_documento>="+tiqueteIni+" order by num_documento;");
			int PuestosBus=40;
			string placa=txtPlaca.Text.Trim();
			PuestosBus=Convert.ToInt16(DBFunctions.SingleData("select capacidad_pasajeros from dbxschema.mbusafiliado where mcat_placa='"+placa+"';"));
			//Tabla tiquetes normales
			dtTiquetes.Columns.Add("NUMERO",typeof(int));
			dtTiquetes.Columns.Add("PASAJERO",typeof(string));
			dtTiquetes.Columns.Add("TIQUETE",typeof(string));
			for(int n=1;n<=PuestosBus+2;n++)
			{
				DataRow drT=dtTiquetes.NewRow();
				drT[0]=n;
				drT[1]=nitPlanillaManual;
				//try
				//{
				//	drT[2]=dsTiquetesDisponibles.Tables[0].Rows[n-1][0].ToString();}
				//catch
				//{
				drT[2]="";
				dtTiquetes.Rows.Add(drT);
			}
			dgrTiquetes.DataSource=dtTiquetes;
			dgrTiquetes.DataBind();

			//Encomiendas, giros, pagos
			DataTable dtEncomiendas=new DataTable();
			dtEncomiendas.Columns.Add("NUMERO",typeof(int));
			dtEncomiendas.Columns.Add("PASAJERO",typeof(string));
			dtEncomiendas.Columns.Add("CONDUCTOR",typeof(string));
			dtEncomiendas.Columns.Add("RELACIONAR",typeof(bool));
			for(int n=1;n<=10;n++)
			{
				DataRow drT=dtEncomiendas.NewRow();
				drT[0]=n;
				drT[1]=nitPlanillaManual;
				drT[2]=txtConductor.Text.Trim();
				drT[3]=0;
				dtEncomiendas.Rows.Add(drT);
			}
			dgrEncomiendas.DataSource=dtEncomiendas;
			dgrEncomiendas.DataBind();
			/*
			dgrGiros.DataSource=dtEncomiendas;
			dgrGiros.DataBind();
			*/
			//Anticipos y Servicios
			DataTable dtAnticipos=new DataTable();
			dtAnticipos.Columns.Add("NUMERO",typeof(int));
			dtAnticipos.Columns.Add("PASAJERO",typeof(string));
			dtAnticipos.Columns.Add("CONDUCTOR",typeof(string));
			for(int n=1;n<=5;n++)
			{
				DataRow drT=dtAnticipos.NewRow();
				drT[0]=n;
				drT[1]=nitPlanillaManual;
				drT[2]=txtConductor.Text.Trim();
				dtAnticipos.Rows.Add(drT);
			}
			dgrPagos.DataSource=dtAnticipos;
			dgrPagos.DataBind();

		//	dgrTiqueteEsps.DataSource=dtEncomiendas;
		//	dgrTiqueteEsps.DataBind();
			//Descuentos
			DataTable dtDescuentos=new DataTable();
			dtDescuentos.Columns.Add("NUMERO",typeof(int));
			dtDescuentos.Columns.Add("PASAJERO",typeof(string));
			dtDescuentos.Columns.Add("CONDUCTOR",typeof(string));
			for(int n=1;n<=3;n++)
			{
				DataRow drT=dtDescuentos.NewRow();
				drT[0]=n;
				drT[1]=nitPlanillaManual;
				drT[2]=txtConductor.Text.Trim();
				dtDescuentos.Rows.Add(drT);
			}
			dgrDescuentos.DataSource=dtDescuentos;
			dgrDescuentos.DataBind();
			
		}
		//Cargar tablas de conceptos
		private void CargarConceptosGastos()
		{
			//Conceptos Gastos
			DataSet dsConceptos=new DataSet();
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			DBFunctions.Request(dsConceptos,IncludeSchema.NO,
				"SELECT DISTINCT rtrim(char(ct.TCON_CODIGO)) as valor, ct.NOMBRE as texto from DBXSCHEMA.TCONCEPTOS_TRANSPORTES ct,DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE ac " +
				"WHERE Tdoc_CODIGO = 'ANT' and ac.mag_codigo="+agencia+"  and  ct.TCON_CODIGO = ac.TCON_CODIGO ORDER BY NOMBRE;");
			//  "select rtrim(char(TCON_CODIGO)) as valor, NOMBRE as texto from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TIPO_CONCEPTO = 'G' AND TDOC_CODIGO = 'ANT' ORDER BY NOMBRE;");
			//"select rtrim(char(TCON_CODIGO)) as valor, NOMBRE as texto from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TCON_CODIGO NOT IN(SELECT TCON_COD FROM DBXSCHEMA.TDOCU_TRANS WHERE TCON_COD IS NOT NULL) ORDER BY NOMBRE;");
			if (dsConceptos.Tables[0].Rows.Count > 0)
			{
				DataRow drC=dsConceptos.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsConceptos.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtConcepto",dsConceptos.Tables[0]);
			}
		}	
		private void CargarConceptosDescuentos()
		{
			//Conceptos Gastos
			DataSet dsConceptos=new DataSet();
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			DBFunctions.Request(dsConceptos,IncludeSchema.NO,
				"SELECT DISTINCT rtrim(char(ct.TCON_CODIGO)) as valor, ct.NOMBRE as texto from DBXSCHEMA.TCONCEPTOS_TRANSPORTES ct,DBXSCHEMA.MAUTORIZACION_GASTO_TRANSPORTE ac " +
				"WHERE Tdoc_CODIGO = 'DESCPLA' and ac.mag_codigo="+agencia+"  and  ct.TCON_CODIGO = ac.TCON_CODIGO ORDER BY NOMBRE;");
			//  "select rtrim(char(TCON_CODIGO)) as valor, NOMBRE as texto from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TIPO_CONCEPTO = 'G' AND TDOC_CODIGO = 'ANT' ORDER BY NOMBRE;");
			//"select rtrim(char(TCON_CODIGO)) as valor, NOMBRE as texto from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TCON_CODIGO NOT IN(SELECT TCON_COD FROM DBXSCHEMA.TDOCU_TRANS WHERE TCON_COD IS NOT NULL) ORDER BY NOMBRE;");
			if (dsConceptos.Tables[0].Rows.Count > 0)
			{
				DataRow drC=dsConceptos.Tables[0].NewRow();
				drC[0]="";
				drC[1]="---seleccione---";
				dsConceptos.Tables[0].Rows.InsertAt(drC,0);
				ViewState.Add("dtConceptoDescuento",dsConceptos.Tables[0]);
			}
		}
		private void CargarEmpleadosAgencias()
		{
			DataSet dsEmpleadosAgencia=new DataSet();
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			DBFunctions.Request(dsEmpleadosAgencia,IncludeSchema.NO,"SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' "+
				"CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') AS NOMBRE "+ 
				"from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP WHERE MP.MAG_CODIGO= "+agencia+" AND MP.MNIT_NIT=MNIT.MNIT_NIT ;");
			if (dsEmpleadosAgencia.Tables[0].Rows.Count > 0)
				ViewState.Add("dtEmpleadosAgencia",dsEmpleadosAgencia.Tables[0]);
		}
		private void CargarConceptosAnulacionTiquetes()
		{
			//Conceptos Anulaciones Tiquetes
			DataSet dsConceptosAnula=new DataSet();
			DBFunctions.Request(dsConceptosAnula,IncludeSchema.NO,
				"select rtrim(char(TCON_CODIGO)) as valor, TCON_DESCRIPCION as texto from DBXSCHEMA.TCONCEPTO_ANULACION_TIQUETE ORDER BY TCON_DESCRIPCION;");
			DataRow drC=dsConceptosAnula.Tables[0].NewRow();
			drC=dsConceptosAnula.Tables[0].NewRow();
			drC[0]="";
			drC[1]="---seleccione---";
			dsConceptosAnula.Tables[0].Rows.InsertAt(drC,0);
			ViewState.Add("dtConceptoAnula",dsConceptosAnula.Tables[0]);
			
		}

		//Cargar rutas principales que pasan por  la ciudad o inician en ella
		private void CargarRutasPrincipales(string ciudad)
		{
			DataSet dsRutasP=new DataSet();
			DBFunctions.Request(dsRutasP,IncludeSchema.NO,
				"select distinct mr.mrut_codigo as valor, "+
				"'[' concat mr.mrut_codigo concat '] ' concat pco.pciu_nombre concat ' - ' concat pcd.pciu_nombre as texto "+
				"from DBXSCHEMA.mrutas mr, DBXSCHEMA.pciudad pco, DBXSCHEMA.pciudad pcd "+
				"where mr.pciu_coddes=pcd.pciu_codigo and mr.pciu_cod=pco.pciu_codigo and mr.mrut_clase=2 and "+
				"  (mr.pciu_cod='"+ciudad+"' "+
				"  or mr.mrut_codigo in( "+
				"   select mrap.mrut_codigo from DBXSCHEMA.MRUTAS mrap, DBXSCHEMA.MRUTAS mras, DBXSCHEMA.MRUTA_INTERMEDIA mri, DBXSCHEMA.PCIUDAD pci "+
				"   WHERE mras.pciu_cod='"+ciudad+"' and mri.mruta_secundaria=mras.mrut_codigo and mri.mruta_principal=mrap.mrut_codigo and pci.pciu_codigo=mrap.pciu_cod "+
				" )"+
				") order by valor");
			DataRow drS=dsRutasP.Tables[0].NewRow();
			drS[0]="";
			drS[1]="---seleccione---";
			dsRutasP.Tables[0].Rows.InsertAt(drS,0);
			ddlRutaPrincipal.DataSource=dsRutasP.Tables[0];
			ddlRutaPrincipal.DataTextField="texto";
			ddlRutaPrincipal.DataValueField="valor";
			ddlRutaPrincipal.DataBind();
		}
	}
}
	