namespace AMS.Comercial
{
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using System.Configuration;
	using Ajax;
	    using AMS.Tools;

	/// <summary>
	///	Descripción breve de AMS_Comercial_CrearViaje.
	/// </summary>
	public class AMS_Comercial_CrearViaje : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.DropDownList ddlNumViajePadre;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlProgramacion;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtConductor;
		protected System.Web.UI.WebControls.TextBox txtConductora;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtRelevador1;
		protected System.Web.UI.WebControls.TextBox txtRelevador1a;
		protected System.Web.UI.WebControls.ListBox lstFecha;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label lblNumViaje;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.TextBox txtCodigo;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.CheckBox chkPlanilla;
		protected System.Web.UI.WebControls.PlaceHolder pnlAgencia;
		protected System.Web.UI.WebControls.PlaceHolder pnlRuta;
		protected System.Web.UI.WebControls.Label lblPlanilla;
		protected System.Web.UI.WebControls.TextBox txtNumeroBus;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtNumeroBusa;
		protected System.Web.UI.WebControls.TextBox txtPuestos;
		protected System.Web.UI.WebControls.PlaceHolder pnlPuestos;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.DropDownList ddlHora;
		protected System.Web.UI.WebControls.DropDownList ddlMinuto;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.DropDownList ddlConfiguracion;
		protected System.Web.UI.WebControls.Label Label15;
		protected string[]dias={"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"};
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(Comercial.AMS_Comercial_CrearViaje));	
			if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				txtNumeroBus.Attributes.Add("onKeyDown", "return(KeyDownHandlerBus(this));");
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				bind.PutDatasIntoDropDownList(ddlProgramacion,"select tprog_tipoprog,tprog_nombre from DBXSCHEMA.tprogramacionruta");
				ddlProgramacion.Items.Insert(0,new ListItem("---No asignada---",""));
				for(int i=0;i<24;i++)
					ddlHora.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				for(int i=0;i<60;i++)
					ddlMinuto.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				bind.PutDatasIntoDropDownList(ddlConfiguracion,"select MCON_COD, '[' concat rtrim(char(MCON_COD)) concat ']  [' concat NOMBRE concat ']' from DBXSCHEMA.MCONFIGURACIONBUS ORDER BY NOMBRE;");
				ddlConfiguracion.Items.Insert(0,new ListItem("---Configuracion---",""));
				if(Request.QueryString["act"]!=null && Request.QueryString["num"]!=null){
					string msg="El viaje ha sido creado con el número "+Request.QueryString["num"];
					if(Request.QueryString["pla"]!=null)msg+=" y el número de planilla "+Request.QueryString["pla"];
					msg+=".";
                    Utils.MostrarAlerta(Response, "" + msg + "");
				}

				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
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
			this.ddlProgramacion.SelectedIndexChanged += new System.EventHandler(this.ddlProgramacion_SelectedIndexChanged);
			this.ddlNumViajePadre.SelectedIndexChanged += new System.EventHandler(this.ddlNumViajePadre_SelectedIndexChanged);
			this.lstFecha.SelectedIndexChanged += new System.EventHandler(this.lstFecha_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		[Ajax.AjaxMethod]
		public DataSet CargarPlaca(string mbus)
		{
			DataSet Vins= new DataSet();
			string sqlBus="select mb.mcat_placa as PLACA,mb.mcat_vin as VIN, mb.pcat_codigo as CATALOGO,mb.fec_ingreso AS FECHA,mb.MBUS_NUMERO as NUMERO,mb.MBUS_VALOR AS VALOR,mb.MNIT_NITPROPIETARIO AS PROPIETARIO,mb.MNIT_ASOCIADO AS ASOCIADO, mb.MNIT_NITCHOFER AS CHOFER, mb.MNIT_SEGCONDUCTOR AS CHOFER2, mb.MBUS_REPOSICION AS REPOSICION, mb.MBUS_OBSERVACIONES AS OBSERVACIONES, mb.MBUS_POTENCIA AS POTENCIA, mb.MBUS_CATEGORIA AS CATEGORIA, mb.TESTA_CODIGO AS ESTADO, mb.MCON_COD AS CONFIG, np.MNIT_NOMBRES concat ' ' concat np.MNIT_APELLIDOS AS NOMPROPIETARIO, na.MNIT_NOMBRES concat ' ' concat na.MNIT_APELLIDOS AS NOMASOCIADO, nc1.MNIT_NOMBRES concat ' ' concat nc1.MNIT_APELLIDOS AS NOMCONDUCTOR1, nc2.MNIT_NOMBRES concat ' ' concat nc2.MNIT_APELLIDOS AS NOMCONDUCTOR2 "+
				"from dbxschema.mbusafiliado mb "+
				"LEFT JOIN dbxschema.mnit np ON np.MNIT_NIT=mb.MNIT_NITPROPIETARIO "+
				"LEFT JOIN dbxschema.mnit na ON na.MNIT_NIT=mb.MNIT_ASOCIADO "+
				"LEFT JOIN dbxschema.mnit nc1 ON nc1.MNIT_NIT=mb.MNIT_NITCHOFER "+
				"LEFT JOIN dbxschema.mnit nc2 ON nc2.MNIT_NIT=mb.MNIT_SEGCONDUCTOR "+
				"where mb.mbus_numero="+mbus+" and mb.testa_codigo>0;";
			DBFunctions.Request(Vins,IncludeSchema.NO,sqlBus);
			return Vins;
		}

		//Cambia la ruta
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string mruta=ddlRuta.SelectedValue;
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			//Cambia la ruta
			if(mruta.Length==0)
			{
				pnlRuta.Visible=false;
				lstFecha.Items.Clear();
				return;
			}
			//Consultar ruta
			pnlRuta.Visible=true;
			string viaje=Viajes.TraerSiguienteViaje(mruta);
			bool paso;
			lblNumViaje.Text=viaje;
			lstFecha.Items.Clear();
			ddlProgramacion.SelectedIndex=ddlProgramacion.Items.IndexOf(ddlProgramacion.Items.FindByValue(""));
			//Ciudad de origen o de paso?
			paso=!DBFunctions.RecordExist(
				"select * from dbxschema.magencia ma, dbxschema.mrutas mr "+
				"where ma.pciu_codigo=mr.pciu_cod and mr.mrut_codigo='"+mruta+"' and ma.mag_codigo="+agencia);
			pnlPuestos.Visible=paso;
			ViewState["PUESTOS"]=paso;
			//Traer Rutas Padre posibles
			ddlNumViajePadre.Items.Clear();
			if(paso && !DBFunctions.RecordExist(
				"SELECT MR.MRUT_CODIGO FROM MRUTAS MR, MAGENCIA MA "+
				"WHERE ma.pciu_codigo=mr.pciu_cod and MR.MRUT_CODIGO='"+ddlRuta.SelectedValue+"' and ma.mag_codigo="+agencia+";"))
				
				bind.PutDatasIntoDropDownList(ddlNumViajePadre,
					"select mv.MVIAJE_NUMERO, " +
					"char(mp.mpla_codigo) concat '(' concat char(mv.MVIAJE_NUMERO) CONCAT ' - ' CONCAT MV.mrut_codigo  " +
					"CONCAT ' - ' CONCAT COALESCE(MV.MCAT_PLACA,'sin ASIG') CONCAT ' - ' CONCAT COALESCE(MBA.MBUS_NUMERO,0)  " +
					"CONCAT ' - ' CONCAT MV.FECHA_SALIDA CONCAT ' : ' CONCAT ROUND(HORA_SALIDA/60,0)  " +
					"CONCAT ' : ' concat case when (mod(mv.hora_salida,60)<10) then '0' else '' end CONCAT round(mod(mv.hora_salida,60),2) concat ')'  " +
					"concat ' [' concat mn.mnit_nombres concat ' ' concat mn.mnit_nombre2 concat ' ' concat mn.mnit_apellidos  concat ']' " +
					"from  MNIT MN, MAGENCIA MA, MRUTAS MR, MPLANILLAVIAJE MP, MVIAJE MV   " +
					"LEFT JOIN MBUSAFILIADO MBA ON MV.MCAT_PLACA = MBA.MCAT_PLACA  " +
					"where MV.mrut_codigo='"+ddlRuta.SelectedValue+"' and MV.estado_viaje<>'T' and   " +
					"mp.mrut_codigo=mv.mrut_codigo and mp.mviaje_numero=mv.mviaje_numero and    " +
					"MA.MAG_CODIGO=MV.MAG_CODIGO AND MR.MRUT_CODIGO=MV.mrut_codigo AND   " +
					"MA.PCIU_CODIGO=MR.PCIU_COD  and (days('"+DateTime.Now.ToString("yyyy-MM-dd")+"')- days(MV.FECHA_SALIDA))<1 " +
					"and mn.mnit_nit=COALESCE(mv.mnit_conductor,'1000000000') " +
					"order by MV.FECHA_SALIDA,mv.hora_salida ;");

			ddlNumViajePadre.Items.Insert(0,new ListItem("---No Asociada---",""));
		}

		private void ddlNumViajePadre_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(Convert.ToBoolean(ViewState["PUESTOS"]))
				pnlPuestos.Visible=(ddlNumViajePadre.SelectedValue.Length==0);

			if (ddlNumViajePadre.Items.Count > 1)
			{
				string numero = ddlNumViajePadre.SelectedValue, ruta = ddlRuta.SelectedValue, agencia,programacion;
				string configuracion;
				DataSet dsViaje = new DataSet();
				DBFunctions.Request(dsViaje, IncludeSchema.NO, "Select mv.FECHA_SALIDA,mv.HORA_SALIDA,mv.TPROG_TIPOPROG,mv.MAG_CODIGO,mv.MCAT_PLACA,mv.MCON_COD,mv.MNIT_CONDUCTOR,mv.MNIT_RELEVADOR1,mv.MNIT_RELEVADOR2,mv.MNIT_AFILIADO,mv.MNIT_DESPACHADOR,mv.HORA_DESPACHO,mv.FECHA_LLEGADA,mv.HORA_LLEGADA,mv.FECHA_LIQUIDACION,mv.VALOR_INGRESOS,mv.VALOR_EGRESOS,mc.MNIT_NOMBRES CONCAT ' 'CONCAT mc.MNIT_APELLIDOS AS NOMCONDUCTOR,mr1.MNIT_NOMBRES CONCAT ' 'CONCAT mr1.MNIT_APELLIDOS AS NOMRELEVADOR1,mr2.MNIT_NOMBRES CONCAT ' 'CONCAT mr2.MNIT_APELLIDOS AS NOMRELEVADOR2 FROM DBXSCHEMA.MVIAJE mv LEFT JOIN DBXSCHEMA.MNIT mc ON mc.MNIT_NIT=mv.MNIT_CONDUCTOR LEFT JOIN DBXSCHEMA.MNIT mr1 ON mr1.MNIT_NIT=mv.MNIT_RELEVADOR1 LEFT JOIN DBXSCHEMA.MNIT mr2 ON mr2.MNIT_NIT=mv.MNIT_RELEVADOR2 WHERE MRUT_CODIGO='" + ruta + "' AND MVIAJE_NUMERO=" + numero);

				programacion = dsViaje.Tables[0].Rows[0]["TPROG_TIPOPROG"].ToString();
				ddlProgramacion.SelectedIndex = ddlProgramacion.Items.IndexOf(ddlProgramacion.Items.FindByValue(programacion));

				CargarHorarios(ruta, programacion);
				lstFecha.SelectedIndex = lstFecha.Items.IndexOf(lstFecha.Items.FindByValue(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"].ToString()));

				int Minutos = Convert.ToInt16((dsViaje.Tables[0].Rows[0]["HORA_SALIDA"].ToString()));
				int Horas = Minutos / 60;
				int HoraMinuto = Minutos % 60;
				ddlHora.SelectedIndex = ddlHora.Items.IndexOf(ddlHora.Items.FindByValue(Horas.ToString()));
				ddlMinuto.SelectedIndex = ddlMinuto.Items.IndexOf(ddlMinuto.Items.FindByValue(HoraMinuto.ToString()));

				txtFecha.Text = Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd");

				configuracion = dsViaje.Tables[0].Rows[0]["MCON_COD"].ToString();
				ViewState["Configuracion"] = configuracion;
				ddlConfiguracion.SelectedIndex = ddlConfiguracion.Items.IndexOf(ddlConfiguracion.Items.FindByValue(configuracion));

				if (dsViaje.Tables[0].Rows[0]["MCAT_PLACA"] != null)
				{
					txtNumeroBus.Text = DBFunctions.SingleData("SELECT rtrim(char(mbus_numero)) as numero from DBXSCHEMA.mbusafiliado where mcat_placa ='" + dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString() + "'");
					//txtNumeroBusa.Text = dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
				}
			}
		}

		//Carga horarios segun la ruta y el tipo de programacion
		private void CargarHorarios(string ruta, string tprog)
		{
			int mins, hrs, tmp;
			string itm, diasT;
			DataSet dsHoras = new DataSet();
			DatasToControls bind = new DatasToControls();
			lstFecha.Items.Clear();
			if (tprog.Length > 0)
			{
				DBFunctions.Request(dsHoras, IncludeSchema.NO, "SELECT mrut_hora,mrut_lunes,mrut_martes,mrut_miercoles,mrut_jueves,mrut_viernes,mrut_sabado,mrut_domingo FROM mruta_cuadro WHERE mrut_codigo='" + ruta + "' AND TPROG_TIPOPROG=" + tprog + ";");
				if (dsHoras.Tables[0].Rows.Count > 0)
				{
					for (int i = 0; i < dsHoras.Tables[0].Rows.Count; i++)
					{
						diasT = "";
						tmp = int.Parse(dsHoras.Tables[0].Rows[i][0].ToString());
						hrs = (int)Math.Round((double)tmp / 60, 0);
						mins = tmp % 60;
						itm = hrs.ToString("00") + ":" + mins.ToString("00");
						for (int j = 0; j < 7; j++)
							if (dsHoras.Tables[0].Rows[i][j + 1].ToString().Equals("S"))
								diasT += dias[j] + ", ";
						if (diasT.EndsWith(", "))
							diasT = diasT.Substring(0, diasT.Length - 2);
						lstFecha.Items.Add(new ListItem("(" + itm + ") " + diasT, tmp.ToString()));
					}
				}
			}
		}

		//Cambia la programacion, cargar horarios
		private void ddlProgramacion_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string ruta=ddlRuta.SelectedValue,itm,diasT;
			string tprog=ddlProgramacion.SelectedValue;
			int mins,hrs,tmp;
			DataSet dsHoras=new DataSet();
			DatasToControls bind=new DatasToControls();
			if(ddlProgramacion.SelectedValue.Length==0){
				lstFecha.Items.Clear();
				return;
			}
			lstFecha.Items.Clear();
			DBFunctions.Request(dsHoras, IncludeSchema.NO, "SELECT mrut_hora,mrut_lunes,mrut_martes,mrut_miercoles,mrut_jueves,mrut_viernes,mrut_sabado,mrut_domingo FROM mruta_cuadro WHERE mrut_codigo='"+ruta+"' AND TPROG_TIPOPROG="+tprog);
			for(int i=0;i<dsHoras.Tables[0].Rows.Count;i++)
			{
				diasT="";
				tmp=int.Parse(dsHoras.Tables[0].Rows[i][0].ToString());
				hrs= tmp/60;
				mins=tmp%60;
				itm=hrs.ToString("00")+":"+mins.ToString("00");
				for(int j=0;j<7;j++)
					if(dsHoras.Tables[0].Rows[i][j+1].ToString().Equals("S"))
						diasT+=dias[j]+", ";
				if(diasT.EndsWith(", "))
					diasT=diasT.Substring(0,diasT.Length-2);
				lstFecha.Items.Add(new ListItem("("+itm+") "+diasT,tmp.ToString()));
			}
		}

		//Cambia agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlRuta.Visible=false;
			if(agencia.Length==0)
			{
				pnlAgencia.Visible=false;
				return;
			}
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			CargarRutasPrincipales(ciudad);

			lblPlanilla.Text=Planillas.TraerSiguientePlanillaVirtual();
			pnlAgencia.Visible=true;
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
			ddlRuta.DataSource=dsRutasP.Tables[0];
			ddlRuta.DataTextField="texto";
			ddlRuta.DataValueField="valor";
			ddlRuta.DataBind();
		}

		//Guardar
		private void btnGuardar_Click(object sender, System.EventArgs e)
		{
			
			string ruta=ddlRuta.SelectedValue, agencia=ddlAgencia.SelectedValue, programacion=ddlProgramacion.SelectedValue,configuracion=ddlConfiguracion.SelectedValue, fecha=txtFecha.Text, conductor=txtConductor.Text, relevador1=txtRelevador1.Text, relevador2="", afiliado="", viajePadre=ddlNumViajePadre.SelectedValue;
			int numPuestosLibres=0;
			//string secOrig="",secDes="";
			bool agenciaDePaso=false;
			//Configuracion
			if(configuracion.Length==0 && txtNumeroBus.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar la Configuración O Numero Bus");
				return;
			}
			
			//configuracion..
			string placa="";
			//Bus
			if(txtNumeroBus.Text.Length>0)
			{ 
				placa=DBFunctions.SingleData("SELECT MCAT_PLACA FROM DBXSCHEMA.MBUSAFILIADO WHERE TESTA_CODIGO>0 AND MBUS_NUMERO="+txtNumeroBus.Text.Trim()+";");
				if(placa.Length==0)
				{
                    Utils.MostrarAlerta(Response, "No existe el bus seleccionado o Inactivo.");
					return;
				}
				string configuracionBus=DBFunctions.SingleData("SELECT MCON_COD FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA ='"+placa+"';");
				if(configuracion.Length>0)
				{
					if(configuracionBus !=configuracion)
					{
                        Utils.MostrarAlerta(Response, "La configuracion no corresponde a la del bus.");
						return;
					}
					
				}
				configuracion = configuracionBus;
				ddlConfiguracion.SelectedIndex=ddlConfiguracion.Items.IndexOf(ddlConfiguracion.Items.FindByValue(configuracionBus));
			}

			if(!DBFunctions.RecordExist("SELECT mele_numepues FROM dbxschema.dconfiguracionbus WHERE mcon_cod="+configuracion+";"))
			{
                Utils.MostrarAlerta(Response, "La configuración no ha sido diseñada.");
				return;
			}
			placa=(placa.Length==0)?"NULL":"'"+placa+"'";
			//Configuracion
			
			//if(configuracion.Length==0){
			//	Response.Write("<script language='javascript'>alert('El bus no tiene una configuración asociada.');</script>");
			//	return;
			//}
			
			//Viaje
			string viaje=Viajes.TraerSiguienteViaje(ruta);
			if(viaje.Length==0)viaje="1";
			//Ruta
			if(ruta.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar la ruta.");
				return;
			}
			//Ruta padre
			if(viajePadre.Length>0)
			{
				// ya fue asociada al viaje padre la agencia?
				if(DBFunctions.RecordExist("SELECT mviaje_numero FROM mviaje "+
					"WHERE mrut_codigo='"+ruta+"' and mag_codigo="+agencia+" and MVIAJE_PADRE="+viajePadre+";"))
				{
                    Utils.MostrarAlerta(Response, "Ya se ha asociado el viaje padre " + viajePadre + " a la ruta " + ruta + ", en la agencia " + agencia + ".");
					return;
				}
				//Es el mismo  bus?
				if(!DBFunctions.RecordExist("SELECT mviaje_numero FROM dbxschema.mviaje "+
					"WHERE MRUT_CODIGO='" + ruta + "' and mviaje_numero=" + viajePadre + " and mcat_placa=" + placa + " or MRUT_CODIGO='" + ruta + "' and mviaje_numero=" + viajePadre + " and mcat_placa is null;"))
				{
                    Utils.MostrarAlerta(Response, "El vehiculo del viaje padre no es el mismo.");
					return;
				}
			}
			//Ya existe planilla activa similar?
			if (placa != "NULL")
				if(DBFunctions.RecordExist(
						"SELECT * FROM DBXSCHEMA.MPLANILLAVIAJE MP,DBXSCHEMA.MVIAJE MV "+
						"WHERE MP.FECHA_LIQUIDACION IS NULL AND FECHA_SALIDA = '"+fecha+"' and "+
						"MP.MRUT_CODIGO=MV.MRUT_CODIGO AND MP.MVIAJE_NUMERO=MV.MVIAJE_NUMERO AND "+
						"MP.MRUT_CODIGO='"+ruta+"' AND MV.MCAT_PLACA="+placa+" AND MP.MAG_CODIGO="+agencia+";"))
				{
                    Utils.MostrarAlerta(Response, "Ya existe una planilla activa para la agencia con el número de bus y la ruta dados.");
					return;
				}
			//Agencia
			if(agencia.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar una agencia.");
				return;
			}
			//Configuracion
			if(!DBFunctions.RecordExist("SELECT mele_numepues FROM dbxschema.dconfiguracionbus WHERE mcon_cod="+configuracion+";")){
                Utils.MostrarAlerta(Response, "La configuración del bus no ha sido diseñada.");
				return;
			}
			//Horario
			if(lstFecha.SelectedIndex<0)
			{
				if (ddlHora.SelectedIndex<0 && ddlMinuto.SelectedIndex<0)

				{
                    Utils.MostrarAlerta(Response, "Debe seleccionar un horario. o digitar la hora");
					return;
				}
			}
			//Fecha
			DateTime fechaT=DateTime.Now;
			try{
				string []prFch=fecha.Split("-".ToCharArray());
				DateTime fechaN=DateTime.Now;
				fechaT=new DateTime(int.Parse(prFch[0]),int.Parse(prFch[1]),int.Parse(prFch[2]));
				if(fechaT.CompareTo(new DateTime(fechaN.Year,fechaN.Month,fechaN.Day))<0)throw(new Exception());
			}
			catch{
                Utils.MostrarAlerta(Response, "Fecha no válida, debe tener el formato aaaa-mm-dd y ser mayor o igual a la fecha actual.");
				return;
			}
			//Programado?
			bool programado=programacion.Length>0;
			int horaProg;
			if(programado)
			{
				//el dia es válido?
				if(lstFecha.SelectedIndex==-1)
				{
                    Utils.MostrarAlerta(Response, "Debe dar el horario si asigna una programación.");
					return;
				}
				int posDia=(fechaT.DayOfWeek==DayOfWeek.Monday?0:(fechaT.DayOfWeek==DayOfWeek.Tuesday?1:(fechaT.DayOfWeek==DayOfWeek.Wednesday?2:(fechaT.DayOfWeek==DayOfWeek.Thursday?3:(fechaT.DayOfWeek==DayOfWeek.Friday?4:(fechaT.DayOfWeek==DayOfWeek.Saturday?5:6))))));
				DataSet dsCuadro=new DataSet();
				DBFunctions.Request(dsCuadro,IncludeSchema.NO,"SELECT MRUT_LUNES,MRUT_MARTES,MRUT_MIERCOLES,MRUT_JUEVES,MRUT_VIERNES,MRUT_SABADO,MRUT_DOMINGO FROM DBXSCHEMA.MRUTA_CUADRO WHERE MRUT_CODIGO='"+ruta+"' AND MRUT_HORA="+lstFecha.SelectedValue+" AND TPROG_TIPOPROG="+programacion);
				if(dsCuadro.Tables[0].Rows.Count==0 || !dsCuadro.Tables[0].Rows[0][posDia].ToString().Equals("S"))
				{
                    Utils.MostrarAlerta(Response, "La fecha seleccionada (día " + dias[posDia].ToLower() + ") no existe en el horario seleccionado.");
					return;
				}
				horaProg=int.Parse(lstFecha.SelectedValue);
			}
			else
			{
				//Hora
				try
				{   
					horaProg =int.Parse(ddlHora.SelectedValue)*60+int.Parse(ddlMinuto.SelectedValue);
					
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Hora no valida.");
					return;
				}
				programacion="NULL";
			}
						
			//Puestos libres deben indicarse si no se asocio a un viaje padre
			if(viajePadre.Length==0 && !DBFunctions.RecordExist(
				"select * from dbxschema.magencia ma, dbxschema.mrutas mr "+
				"where ma.pciu_codigo=mr.pciu_cod and mr.mrut_codigo='"+ruta+"' and ma.mag_codigo="+agencia))
			{
				agenciaDePaso=true;
				try
				{
					numPuestosLibres=Convert.ToInt32(txtPuestos.Text.Trim());
					if(numPuestosLibres<0)throw new Exception();
				}
				catch
				{
                    Utils.MostrarAlerta(Response, "Debe dar el número de puestos libres y este debe ser un número entero menor o igual a la capacidad del bus seleccionado.");
					return;
				}
				if(numPuestosLibres>Convert.ToInt32(DBFunctions.SingleData("SELECT COUNT(*) FROM DBXSCHEMA.DCONFIGURACIONBUS WHERE MCON_COD="+configuracion+" AND TELE_CODIGO='SP';")))
				{
                    Utils.MostrarAlerta(Response, "El número de puestos libres supera la cantidad de puestos en el diseño de la configuración.");
					return;
				}
			}
			
			//Conductor/relevadores
			if(conductor.Length>0 && (conductor.Equals(relevador1)||conductor.Equals(relevador2))){
                Utils.MostrarAlerta(Response, "El conductor y los relevadores no pueden ser la misma persona.");
				return;
			}
			if(relevador1.Length>0 && relevador1.Equals(relevador2)){
                Utils.MostrarAlerta(Response, "Los relevadores no pueden ser la misma persona.");
				return;
			}
			//Validar conductor
			if(conductor.Length>0 && !DBFunctions.RecordExist("Select MNIT_NIT FROM DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+conductor+"' AND PCAR_CODICARGO='CO'")){
                Utils.MostrarAlerta(Response, "No se encontro el conductor.");
				return;
			}
			if(conductor.Length==0 && relevador1.Length==0){
                Utils.MostrarAlerta(Response, "Debe dar el conductor o el relevador.");
				return;
			}
			//Afiliado
			if (placa != "NULL" && placa != "")
				afiliado=DBFunctions.SingleData("SELECT MNIT_ASOCIADO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA="+placa+";");
			else 
				afiliado = "";
			if(conductor == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				conductor = "";
			}
			if(relevador1 == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				relevador1 = "";
			}

			afiliado=(afiliado.Length==0)?"NULL":"'"+afiliado+"'";
			//Conductor
			conductor=(conductor.Length==0?"NULL":"'"+conductor+"'");
			//Relevador1
			relevador1=(relevador1.Length==0?"NULL":"'"+relevador1+"'");
			//Relevador2
			relevador2=(relevador2.Length==0?"NULL":"'"+relevador2+"'");
			//Viaje padre
			viajePadre=(viajePadre.Length==0?"NULL":ddlNumViajePadre.SelectedValue);

			ArrayList sqlC=new ArrayList();
			//Insertar registro
			sqlC.Add("insert into dbxschema.mviaje values('"+ruta+"',"+viaje+",'"+fechaT.ToString("yyyy-MM-dd")+"',"+horaProg+","+programacion+","+agencia+","+placa+","+configuracion+","+conductor+","+relevador1+","+relevador2+","+afiliado+",NULL,NULL,NULL,NULL,NULL,NULL,NULL,'A',"+viajePadre+")");
			if(!viajePadre.Equals("NULL"))
				sqlC.Add("update dbxschema.mviaje set mviaje_padre="+viajePadre+" where mrut_codigo='"+ruta+"' and mviaje_numero="+viajePadre+";");
			//Guardar configuracion puestos
			int fil=int.Parse(DBFunctions.SingleData("SELECT MBUS_FILAS FROM DBXSCHEMA.MCONFIGURACIONBUS WHERE MCON_COD="+configuracion));
			int col=int.Parse(DBFunctions.SingleData("SELECT MBUS_COLUMNAS FROM DBXSCHEMA.MCONFIGURACIONBUS WHERE MCON_COD="+configuracion));
			//borrar anteriores
			sqlC.Add("delete from DBXSCHEMA.MCONFIGURACIONPUESTO WHERE MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+viaje+";");

			//insertar disponibles
			sqlC.Add("insert into DBXSCHEMA.MCONFIGURACIONPUESTO "+
				"SELECT '"+ruta+"', "+viaje+", mpue_posfila, mpue_poscolumna,mele_numepues,tele_codigo,'DN', cast(null as varchar(10)), cast(null as integer) "+
				"FROM DCONFIGURACIONBUS WHERE MCON_COD="+configuracion+";");

			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0){
                Utils.MostrarAlerta(Response, "El usuario (responsable) no tiene un NIT asignado.");
				return;}

			//Planilla
			string planilla=Planillas.TraerSiguientePlanillaVirtual();
			//Insertar papeleria
			sqlC.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('PLA',"+planilla+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");

			//Crear planilla
			sqlC.Add("insert into dbxschema.mplanillaviaje values("+planilla+",'"+ruta+"',"+viaje+","+agencia+",'"+nitResponsable+"','"+fechaT.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,0,"+numPuestosLibres+",NULL);");

			//Crear planilla gemela si aplica
			DataSet dsGemela=new DataSet();
			DBFunctions.Request(dsGemela,IncludeSchema.NO,"SELECT MRUT_CODIGO,MRUT_CODIGO1,MRUT_CODIGO2 FROM DBXSCHEMA.MRUTAS_DOBLE_PLANILLA WHERE MRUT_CODIGO='"+ruta+"';");
			if(dsGemela.Tables[0].Rows.Count>0){
				sqlC.Add("INSERT INTO DBXSCHEMA.MPLANILLA_VIAJE_GEMELA VALUES (DEFAULT,"+planilla+",'"+dsGemela.Tables[0].Rows[0]["MRUT_CODIGO1"]+"',DEFAULT);");
				sqlC.Add("INSERT INTO DBXSCHEMA.MPLANILLA_VIAJE_GEMELA VALUES (DEFAULT,"+planilla+",'"+dsGemela.Tables[0].Rows[0]["MRUT_CODIGO2"]+"',DEFAULT);");
			}
			
			if(DBFunctions.Transaction(sqlC)){
				//Ocupar puestos si es de paso
				if(agenciaDePaso){
					int pUsados=0;
					DataSet dsPuestos=new DataSet();
					sqlC=new ArrayList();
					DBFunctions.Request(dsPuestos,IncludeSchema.NO,"SELECT FILA,COLUMNA,TELE_CODIGO FROM DBXSCHEMA.MCONFIGURACIONPUESTO WHERE MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+viaje+";");
					foreach(DataRow drP in dsPuestos.Tables[0].Rows){
						if(drP["TELE_CODIGO"].ToString().Equals("SP"))pUsados++;
						if(pUsados>numPuestosLibres)
							sqlC.Add("update DBXSCHEMA.MCONFIGURACIONPUESTO SET TEST_CODIGO='OC' WHERE MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+viaje+" AND FILA="+drP["FILA"]+" AND COLUMNA="+drP["COLUMNA"]+";");
					}
					DBFunctions.Transaction(sqlC);
				}
				if(Tools.Browser.IsMobileBrowser())
					Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.VentaTiquetesMovil&act=1&path=Venta Tiquetes&num="+viaje+"&pla="+planilla);
				else
					Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.VentaTiquetes&act=1&path=Venta Tiquetes&num="+viaje+"&pla="+planilla);
			}
			else 
				lblError.Text=DBFunctions.exceptions;
		}

		private void lstFecha_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
	
	}
}

