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
	///		Descripción breve de AMS_Comercial_PlanillaVirtual.
	/// </summary>
	public class AMS_Comercial_PlanillaVirtual : System.Web.UI.UserControl
	{
		#region Controles
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList ddlRutaPrincipal;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtPlaca;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPlacaa;
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
		protected System.Web.UI.WebControls.Button btnSeleccionarPlanilla;
		protected System.Web.UI.WebControls.Panel pnlSeleccionar;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.DataGrid dgrTiquetes;
		protected System.Web.UI.WebControls.DataGrid dgrTiqueteEsps;
		protected System.Web.UI.WebControls.DataGrid dgrEncomiendas;
		protected System.Web.UI.WebControls.DataGrid dgrGiros;
		protected System.Web.UI.WebControls.DataGrid dgrPagos;
		protected System.Web.UI.WebControls.Button btnPlanillar;
		protected System.Web.UI.WebControls.Panel pnlElementos;
		protected System.Web.UI.WebControls.Label lblError;
		public string strActScript="";
		private string strTotalTiquetes="";
		public string strAnulacionTiquetes="";
		#endregion Controles

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
				CargarConceptos();
				
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
			this.btnSeleccionarPlanilla.Click += new System.EventHandler(this.btnSeleccionarPlanilla_Click);
			this.dgrTiquetes.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrTiquetes_ItemDataBound);
			this.btnPlanillar.Click += new System.EventHandler(this.btnPlanillar_Click);
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
			pnlElementos.Visible=false;
			pnlRuta.Visible=false;
			pnlSeleccionar.Visible=true;
			if(ddlAgencia.Items.Count==0)return;

			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			
			CargarRutasPrincipales(ciudad);
		}

		//Cambia la ruta principal
		private void ddlRutaPrincipal_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			pnlElementos.Visible=false;
			pnlRuta.Visible=false;
			pnlSeleccionar.Visible=true;

			if(ddlRutaPrincipal.Items.Count==0)return;
			string ruta=ddlRutaPrincipal.SelectedValue;
			if(ruta.Length==0)return;

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
		#endregion DropDownLists

		#region Datagrids
		private void dgrTiquetes_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				DropDownList dlDestinos=(DropDownList)e.Item.FindControl("ddlDestinoTiquete");
				DropDownList dlAnulacion=(DropDownList)e.Item.FindControl("ddlConceptoAnulacion");
				TextBox txtValor=(TextBox)e.Item.FindControl("txtValorTiquete");
				TextBox txtCantidad=(TextBox)e.Item.FindControl("txtCantidadTiquete");
				TextBox txtTotal=(TextBox)e.Item.FindControl("txtTotalTiquete");
				CheckBox chkAnulado=(CheckBox)e.Item.FindControl("chkAnlulado");
				CheckBox chkPendiente=(CheckBox)e.Item.FindControl("chkPendiente");
				txtValor.Attributes.Add("onkeyup","NumericMask(this);totalTicketes('"+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"',1);");
				txtCantidad.Attributes.Add("onkeyup","NumericMask(this);totalTicketes('"+txtCantidad.ClientID+"','"+txtValor.ClientID+"','"+txtTotal.ClientID+"',1);");
				chkAnulado.Attributes.Add("onclick","verConcepto(this,'"+dlAnulacion.ClientID+"');");
				dlDestinos.DataSource=ViewState["dtDestino"];
				dlDestinos.DataTextField="texto";
				dlDestinos.DataValueField="valor";
				dlDestinos.DataBind();
				dlAnulacion.DataSource=ViewState["dtConceptoAnula"];
				dlAnulacion.DataTextField="texto";
				dlAnulacion.DataValueField="valor";
				dlAnulacion.DataBind();
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

		#endregion Datagrids
		
		//Seleccionar planilla
		private void btnSeleccionarPlanilla_Click(object sender, System.EventArgs e)
		{
			string ruta=ddlRutaPrincipal.SelectedValue;
			string placa=txtPlaca.Text.Trim();
			string nitConductor=txtConductor.Text.Trim();
			string nitRelevador=txtRelevador.Text.Trim();
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			DateTime fecha;
			

			//Verificar placa
			if(!DBFunctions.RecordExist("select mcat_placa from dbxschema.mbusafiliado where mcat_placa='"+placa+"';"))
			{
				Response.Write("<script language='javascript'>alert('La placa seleccionada no se encuentra registrada.');</script>");
				return;
			}

			//Verificar conductor
			if(nitConductor.Length==0)
			{
				Response.Write("<script language='javascript'>alert('Debe ingresar el conductor.');</script>");
				return;
			}

			//Relevador
			nitRelevador=(nitRelevador.Length==0)?"NULL":"'"+nitRelevador+"'";
			
			if(nitConductor.Equals(nitRelevador.Replace("'","")))
			{
				Response.Write("<script language='javascript'>alert('El conductor y el relevador son la misma persona.');</script>");
				return;
			}
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
			pnlElementos.Visible=true;
			
			MostrarTablas();
		}

		//Planillar
		private void btnPlanillar_Click(object sender, System.EventArgs e)
		{
			#region Validaciones Generales
			ArrayList sqlUpd=new ArrayList();
			string errores="";
			string viaje;
			string agencia=ddlAgencia.SelectedValue.Trim().Replace("|","");
			string placa=txtPlaca.Text.Trim();
			long numDocumento=0;
			DateTime fecha=Convert.ToDateTime(txtFecha.Text);
			string conductor=txtConductor.Text.Trim();
			string relevador=txtRelevador.Text.Trim();
			relevador=(relevador.Length==0)?"NULL":"'"+relevador+"'";
			string rutaP=ddlRutaPrincipal.SelectedValue;
			string nitPlanillaManual=DBFunctions.SingleData("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';");
			
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
			{
				Response.Write("<script language='javascript'>alert('El usuario (responsable) no tiene un NIT asignado.');</script>");
				return;}

			//Hora
			int hora=(int.Parse(ddlHora.SelectedValue)*60)+int.Parse(ddlMinuto.SelectedValue);
			
			//Afiliado
			string afiliado=DBFunctions.SingleData("SELECT MNIT_ASOCIADO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"'");
			afiliado=(afiliado.Length==0)?"NULL":"'"+afiliado+"'";
			
			//Configuracion
			string configBus=DBFunctions.SingleData("Select MCON_COD from DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"';");
			if(configBus.Length==0)configBus="NULL";
			
			string planilla=Planillas.TraerSiguientePlanillaVirtual();
			viaje=Viajes.TraerSiguienteViaje(rutaP);
			//Insertar viaje
			sqlUpd.Add("INSERT INTO dbxschema.MVIAJE values('"+rutaP+"',"+viaje+",'"+fecha.ToString("yyyy-MM-dd")+"',"+hora+",NULL,"+agencia+",'"+placa+"',"+configBus+",'"+conductor+"',"+relevador+",NULL,"+afiliado+",'"+nitResponsable+"',"+hora+",NULL,NULL,NULL,NULL,NULL,'A',NULL);");

			//Insertar papeleria
			sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('PLA',"+planilla+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
			//Insertar planilla
			sqlUpd.Add("insert into dbxschema.mplanillaviaje values("+planilla+",'"+rutaP+"',"+viaje+","+agencia+",'"+nitResponsable+"','"+fecha.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,0,0,NULL);");
			#endregion

			//Fila real y actual
			int fila=1,linea=1;
			//Totales
			double totalTiquetes=0,valorSeguro;
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
			int prefijo=0, totalPasajeros=0;
			
			//Prefijo tiquetes
			if(ddlAgencia.SelectedValue.EndsWith("|"))
				prefijo=Convert.ToInt16(agencia);
			
			numDocumento=Convert.ToInt64(Tiquetes.TraerSiguienteTiqueteVirtual());
			foreach(DataGridItem dgrI in dgrTiquetes.Items)
			{
				bool errLin=false;
				DropDownList ddlDestino=(DropDownList)dgrI.FindControl("ddlDestinoTiquete");
				TextBox txtCantidadTiquete=(TextBox)dgrI.FindControl("txtCantidadTiquete");
				TextBox txtValorTiquete=(TextBox)dgrI.FindControl("txtValorTiquete");
				TextBox txtTotalTiquete=(TextBox)dgrI.FindControl("txtTotalTiquete");
				CheckBox chkAnula=(CheckBox)dgrI.FindControl("chkAnlulado");
				DropDownList ddlConceptoAnulacion=(DropDownList)dgrI.FindControl("ddlConceptoAnulacion");
				
				if((ddlDestino.SelectedValue.Length>0 || txtCantidadTiquete.Text.Trim().Length>0 || txtValorTiquete.Text.Trim().Length>0))
				{
					int cantidad=0;
					double precio=0;
					bool anula=false;
					string ruta=ddlDestino.SelectedValue;
					//Ruta
					if(ruta.Length==0)
					{
						errores+="Destino de tiquete no válido en la línea "+fila+". ";errLin=true;}
					
					//Cantidad
					try{
						cantidad=int.Parse(txtCantidadTiquete.Text.Replace(",","").Trim());}
					catch{
						errores+="Cantidad de tiquetes no válida en la línea "+fila+". ";errLin=true;}
					
					//Precio
					try{
						precio=double.Parse(txtValorTiquete.Text.Replace(",","").Trim());
						if(precio<=0)throw(new Exception());}
					catch{
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
							strActScript+="totalTicketes('"+txtCantidadTiquete.ClientID+"','"+txtValorTiquete.ClientID+"','"+txtTotalTiquete.ClientID+"',1);";
							errLin=true;
						}

					
					//Insertar tiquete
					if(!errLin)
					{
						sqlUpd.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('TIQ',"+numDocumento+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
						if(anula)
						{
							//ANULAR
							sqlUpd.Add("INSERT INTO dbxschema.mtiquete_viaje values('TIQ',"+numDocumento+","+planilla+","+linea+",'"+ruta+"','"+nitResponsable+"',"+cantidad+",'"+nitPlanillaManual+"',"+valorSeguro.ToString("0")+","+precio+","+precio*cantidad+",'"+fecha.ToString("yyyy-MM-dd")+"','A');");
							sqlUpd.Add("INSERT INTO dbxschema.MTIQUETE_VIAJE_ANULADO values('TIQ',"+numDocumento+","+ddlConceptoAnulacion.SelectedValue+",'"+fecha.ToString("yyyy-MM-dd")+"','"+nitResponsable+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
							sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET NUM_ANULACION=0, COD_ANULACION="+ddlConceptoAnulacion.SelectedValue+",MNIT_ANULACION='"+nitResponsable+"', FECHA_ANULACION='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+numDocumento+";");
						}
						else
						{
							sqlUpd.Add("INSERT INTO dbxschema.mtiquete_viaje values('TIQ',"+numDocumento+","+planilla+","+linea+",'"+ruta+"','"+nitResponsable+"',"+cantidad+",'"+nitPlanillaManual+"',"+valorSeguro.ToString("0")+","+precio+","+precio*cantidad+",'"+fecha.ToString("yyyy-MM-dd")+"','V');");
							totalTiquetes+=cantidad*precio;
							totalPasajeros+=cantidad;
						}
						//Modificar papeleria(usada)
						sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd")+"',MPLA_CODIGO="+planilla+" WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+numDocumento+";");
						dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
						linea++;
					}
					else
					{
						dgrI.Cells[0].BackColor=System.Drawing.Color.DarkSalmon;
						if(errores.Length>0)errores+="\\r\\n";}
					numDocumento++;
				}
				else dgrI.Cells[0].BackColor=dgrI.Cells[1].BackColor;
				fila++;
			}
			#endregion

	
			if(errores.Length==0)
			{
				//Actualizar VIAJE
				sqlUpd.Add("UPDATE dbxschema.MVIAJE SET VALOR_INGRESOS="+(totalTiquetes)+", VALOR_EGRESOS=0, ESTADO_VIAJE='T', FECHA_LIQUIDACION='"+fecha.ToString("yyyy-MM-dd")+"' WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
				//Actualizar PLANILLA: NO. LINEAS, FECHA LIQUID, INGRESOS, EGRESOS, NO. LINEAS
				sqlUpd.Add("UPDATE dbxschema.MPLANILLAVIAJE SET VALOR_INGRESOS="+(totalTiquetes)+", VALOR_EGRESOS=0, NUMERO_LINEAS="+linea+", FECHA_LIQUIDACION='"+fecha.ToString("yyyy-MM-dd")+"' WHERE MPLA_CODIGO="+planilla+";");
				//Actualizar PAPELERIA PLANILLA
				sqlUpd.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE TDOC_CODIGO='PLA' AND NUM_DOCUMENTO="+planilla+";");
				if(DBFunctions.Transaction(sqlUpd))
					Response.Redirect(indexPage+"?process=Comercial.PlanillaBus&path="+Request.QueryString["path"]+"&pln="+planilla);
				else
					lblError.Text=DBFunctions.exceptions;
			}
			else strActScript+="alert('"+errores+"');";
		}

		//Mostrar grillas
		private void MostrarTablas()
		{
			DataSet dsTiquetesDisponibles=new DataSet();
			DataTable dtTiquetes=new DataTable();
			string nitPlanillaManual=DBFunctions.SingleData("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';");
			int PuestosBus=40;
            string placa=txtPlaca.Text.Trim();
			PuestosBus=Convert.ToInt16(DBFunctions.SingleData("select capacidad_pasajeros from dbxschema.mbusafiliado where mcat_placa='"+placa+"';"));
			//Tabla tiquetes normales
			dtTiquetes.Columns.Add("PASAJERO",typeof(string));
			dtTiquetes.Columns.Add("TIQUETE",typeof(string));
			for(int n=1;n<=PuestosBus+2;n++)
			{
				DataRow drT=dtTiquetes.NewRow();
				drT[0]=nitPlanillaManual;
				drT[1]="";
				dtTiquetes.Rows.Add(drT);
			}
			dgrTiquetes.DataSource=dtTiquetes;
			dgrTiquetes.DataBind();
		}
		//Cargar tablas de conceptos
		private void CargarConceptos()
		{
			//Conceptos Anulaciones Tiquetes
			DataSet dsConceptosAnula=new DataSet();
			DBFunctions.Request(dsConceptosAnula,IncludeSchema.NO,
				"select rtrim(char(TCON_CODIGO)) as valor, TCON_DESCRIPCION as texto from DBXSCHEMA.TCONCEPTO_ANULACION_TIQUETE ORDER BY TCON_DESCRIPCION;");
			DataRow drC=dsConceptosAnula.Tables[0].NewRow();
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
