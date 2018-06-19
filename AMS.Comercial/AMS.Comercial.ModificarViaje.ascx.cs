namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Collections;
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
	///	Descripción breve de AMS_Comercial_ModificarViaje.
	/// </summary>
	public class AMS_Comercial_ModificarViaje : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlRuta;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlProgramacion;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.DropDownList ddlPlaca;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.DropDownList ddlConfiguracion;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtConductor;
		protected System.Web.UI.WebControls.TextBox txtConductora;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.TextBox txtRelevador1;
		protected System.Web.UI.WebControls.TextBox txtRelevador1a;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtRelevador2;
		protected System.Web.UI.WebControls.TextBox txtRelevador2a;
		protected System.Web.UI.WebControls.Panel pnlRuta;
		protected System.Web.UI.WebControls.ListBox lstFecha;
		protected System.Web.UI.WebControls.TextBox txtFecha;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.DropDownList ddlNumero;
		protected System.Web.UI.WebControls.Panel pnlNumero;
		protected System.Web.UI.WebControls.Panel pnlAgencia;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList ddlHora;
		protected System.Web.UI.WebControls.DropDownList ddlMinuto;
		protected string[]dias={"Lunes","Martes","Miercoles","Jueves","Viernes","Sabado","Domingo"};
		#endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(Comercial.AMS_Comercial_ModificarViaje));	
			if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				bind.PutDatasIntoDropDownList(ddlProgramacion,"select tprog_tipoprog,tprog_nombre from DBXSCHEMA.tprogramacionruta");
				ddlProgramacion.Items.Insert(0,new ListItem("---Programacion---",""));
				bind.PutDatasIntoDropDownList(ddlConfiguracion,"select MCON_COD, 'Conf:[' concat rtrim(char(MCON_COD)) concat '] Puestos:[' concat rtrim(char(PUESTOS_PASAJEROS)) concat ']  [' concat NOMBRE concat ']' from DBXSCHEMA.MCONFIGURACIONBUS ORDER BY NOMBRE");
				ddlConfiguracion.Items.Insert(0,new ListItem("---Configuracion---",""));
				if(Request.QueryString["act"]!=null)
                Utils.MostrarAlerta(Response, "  El viaje ha sido modificado.");
				for(int i=0;i<24;i++)
					ddlHora.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
				for(int i=0;i<60;i++)
					ddlMinuto.Items.Add(new ListItem(i.ToString("00"),i.ToString()));
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
			this.ddlNumero.SelectedIndexChanged += new System.EventHandler(this.ddlNumero_SelectedIndexChanged);
			this.ddlProgramacion.SelectedIndexChanged += new System.EventHandler(this.ddlProgramacion_SelectedIndexChanged);
			this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		[Ajax.AjaxMethod]
		public DataSet CargarPlaca(string mplaca)
		{
			DataSet Vins= new DataSet();
			string sqlBus="select mb.mcat_vin as VIN, mb.pcat_codigo as CATALOGO,mb.fec_ingreso AS FECHA,mb.MBUS_NUMERO as NUMERO,mb.MBUS_VALOR AS VALOR,mb.MNIT_NITPROPIETARIO AS PROPIETARIO,mb.MNIT_ASOCIADO AS ASOCIADO, mb.MNIT_NITCHOFER AS CHOFER, mb.MNIT_SEGCONDUCTOR AS CHOFER2, mb.MBUS_REPOSICION AS REPOSICION, mb.MBUS_OBSERVACIONES AS OBSERVACIONES, mb.MBUS_POTENCIA AS POTENCIA, mb.MBUS_CATEGORIA AS CATEGORIA, mb.TESTA_CODIGO AS ESTADO, mb.MCON_COD AS CONFIG, np.MNIT_NOMBRES concat ' ' concat np.MNIT_APELLIDOS AS NOMPROPIETARIO, na.MNIT_NOMBRES concat ' ' concat na.MNIT_APELLIDOS AS NOMASOCIADO, nc1.MNIT_NOMBRES concat ' ' concat nc1.MNIT_APELLIDOS AS NOMCONDUCTOR1, nc2.MNIT_NOMBRES concat ' ' concat nc2.MNIT_APELLIDOS AS NOMCONDUCTOR2 from dbxschema.mbusafiliado mb "+
				"LEFT JOIN dbxschema.mnit np ON np.MNIT_NIT=mb.MNIT_NITPROPIETARIO "+
				"LEFT JOIN dbxschema.mnit na ON na.MNIT_NIT=mb.MNIT_ASOCIADO "+
				"LEFT JOIN dbxschema.mnit nc1 ON nc1.MNIT_NIT=mb.MNIT_NITCHOFER "+
				"LEFT JOIN dbxschema.mnit nc2 ON nc2.MNIT_NIT=mb.MNIT_SEGCONDUCTOR "+
				"where mcat_placa='"+mplaca+"';";
			DBFunctions.Request(Vins,IncludeSchema.NO,sqlBus);
			return Vins;
		}

		//Cambia la ruta
		private void ddlRuta_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string mruta=ddlRuta.SelectedValue;
			string agencia = ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlNumero.Visible=false;
			//Cambia la ruta
			if(mruta.Length==0){
				pnlRuta.Visible=false;
				ddlNumero.Items.Clear();
				lstFecha.Items.Clear();
				ddlPlaca.Items.Clear();
				return;
			}
			//Consultar ruta
			//administrador del sistema adminck
			string adminck=DBFunctions.SingleData("select TTIPE_CODIGO from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
	
			//Numeros Viajes
			DateTime fechaActual = DateTime.Now;
			if  (adminck == "AS") 
			{
			 fechaActual = fechaActual.AddDays(-60); 
			}
			else

fechaActual = fechaActual.AddDays(-1); 
			string FechaHoy=fechaActual.ToString("yyyy-MM-dd");
			//bind.PutDatasIntoDropDownList(ddlNumero,"select mviaje_numero,'[' concat rtrim(char(mviaje_numero)) concat '] '" +
			//    " concat (select ' [' concat rtrim(char(mbus_numero)) concat ']' from dbxschema.mbusafiliado b where b.MCAT_PLACA = v.MCAT_PLACA) CONCAT ' [' concat MCAT_PLACA concat '] ' concat char(fecha_salida) concat ' ' concat "+
			//    "rtrim(char(floor(hora_salida/60))) concat ':' "+
			//    "concat case when (mod(hora_salida,60)<10) then '0' else '' end concat "+
			//    "rtrim(char(mod(hora_salida,60)))   as texto from dbxschema.mviaje v "+
			//    "where (estado_viaje='A' OR estado_viaje='V') and mrut_codigo='"+mruta+"' and fecha_salida >= '"+FechaHoy+"' order by fecha_salida,mviaje_numero;");

			bind.PutDatasIntoDropDownList(ddlNumero,
				"select mviaje_numero, '[' concat rtrim(char(mviaje_numero)) concat '] ' " +
				"concat coalesce(rtrim(char(mb.mbus_numero)),'000') concat '] [' concat coalesce(mv.MCAT_PLACA,'******') concat '] ['  " +
				"concat mv.mrut_codigo concat '] [' concat char(mv.fecha_salida) concat '] [' concat  " +
				"rtrim(char(floor(mv.hora_salida/60))) concat ':'  " +
				"concat case when (mod(mv.hora_salida,60)<10) then '0' else '' end concat  " +
				"rtrim(char(mod(mv.hora_salida,60)))  concat ']' concat'[' concat mn.mnit_nombres concat ' ' concat mn.mnit_nombre2 concat ' ' concat mn.mnit_apellidos concat ']' as texto  " +
				"from  dbxschema.mviaje mv  LEFT JOIN dbxschema.mbusafiliado mb ON mb.mcat_placa=mv.mcat_placa, " +
				"dbxschema.mnit mn where (estado_viaje='A' OR estado_viaje='V') and mrut_codigo='" + mruta + "' and  " +
				"fecha_salida >= '" + FechaHoy + "' and mag_codigo=" + agencia + " and  mn.mnit_nit  = COALESCE(mv.mnit_conductor,'1000000000') order by fecha_salida,mviaje_numero;");

				//"select mviaje_numero, '[' concat rtrim(char(mviaje_numero)) concat '] '"+
				//" concat coalesce(rtrim(char(mb.mbus_numero)),'000') concat '] [' concat coalesce(mv.MCAT_PLACA,'******') concat '] [' "+ 
				//" concat mv.mrut_codigo concat '] [' concat char(mv.fecha_salida) concat '] [' concat "+  
				//" rtrim(char(floor(mv.hora_salida/60))) concat ':' "+  
				//" concat case when (mod(mv.hora_salida,60)<10) then '0' else '' end concat "+  
				//" rtrim(char(mod(mv.hora_salida,60)))  concat ']'  as texto "+
				//" from  dbxschema.mviaje mv  LEFT JOIN dbxschema.mbusafiliado mb ON mb.mcat_placa=mv.mcat_placa "+
				//" where (estado_viaje='A' OR estado_viaje='V') and mrut_codigo='" + mruta + "' and fecha_salida >= '" + FechaHoy + "' and mag_codigo=" + agencia + " order by fecha_salida,mviaje_numero; ");
			

			ListItem itm=new ListItem("-----Numero-----","");
			ddlNumero.Items.Insert(0,itm);
			pnlRuta.Visible=true;
			
		}

		//Cambia la programacion, cargar horarios
		private void ddlProgramacion_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string ruta=ddlRuta.SelectedValue;
			string tprog=ddlProgramacion.SelectedValue;
			if(ddlProgramacion.SelectedValue.Length==0){
				lstFecha.Items.Clear();
				return;
			}
			CargarHorarios(ruta,tprog);
		}

		//Carga horarios segun la ruta y el tipo de programacion
		private void CargarHorarios(string ruta,string tprog)
		{
			int mins,hrs,tmp;
			string itm,diasT;
			DataSet dsHoras=new DataSet();
			DatasToControls bind=new DatasToControls();
			lstFecha.Items.Clear();
			if(tprog.Length>0)
			{
				DBFunctions.Request(dsHoras, IncludeSchema.NO, "SELECT mrut_hora,mrut_lunes,mrut_martes,mrut_miercoles,mrut_jueves,mrut_viernes,mrut_sabado,mrut_domingo FROM mruta_cuadro WHERE mrut_codigo='"+ruta+"' AND TPROG_TIPOPROG="+tprog+";");
				if (dsHoras.Tables[0].Rows.Count > 0)
				{
					for(int i=0;i<dsHoras.Tables[0].Rows.Count;i++)
					{
						diasT="";
						tmp=int.Parse(dsHoras.Tables[0].Rows[i][0].ToString());
						hrs=(int)Math.Round((double)tmp/60,0);
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
			}
		}
		//Seleccion agencia
		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlRuta.Visible=false;
			pnlNumero.Visible=false;
			if(agencia.Length==0)
			{
				ddlPlaca.Items.Clear();
				pnlAgencia.Visible=false;
				return;
			}
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			//Rutas
			//bind.PutDatasIntoDropDownList(ddlRuta,"select mrut_codigo,'[' concat mr.mrut_codigo concat '] ' concat pco.pciu_nombre concat ' - ' concat pcd.pciu_nombre as texto  from DBXSCHEMA.mrutas WHERE MRUT_CLASE=2 and pciu_cod='"+ciudad+"' order by mrut_codigo;");
			ddlRuta.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlRuta, "select distinct mr.mrut_codigo as valor, " +
					"'[' concat mr.mrut_codigo concat '] ' concat pco.pciu_nombre concat ' - ' concat pcd.pciu_nombre as texto " +
					"from DBXSCHEMA.mrutas mr, DBXSCHEMA.pciudad pco, DBXSCHEMA.pciudad pcd " +
					"where mr.pciu_coddes=pcd.pciu_codigo and mr.pciu_cod=pco.pciu_codigo and mr.mrut_clase=2 and " +
					"  (mr.pciu_cod='" + ciudad + "' " +
					"  or mr.mrut_codigo in( " +
					"   select mrap.mrut_codigo from DBXSCHEMA.MRUTAS mrap, DBXSCHEMA.MRUTAS mras, DBXSCHEMA.MRUTA_INTERMEDIA mri, DBXSCHEMA.PCIUDAD pci " +
					"   WHERE mras.pciu_cod='" + ciudad + "' and mri.mruta_secundaria=mras.mrut_codigo and mri.mruta_principal=mrap.mrut_codigo and pci.pciu_codigo=mrap.pciu_cod " +
					" )" +
					") order by valor");
					   

			ddlRuta.Items.Insert(0,new ListItem("---Ruta---",""));
			//Buses, trae los que se encuentran en la agencia y estan disponibles
			ddlPlaca.Items.Clear();
			//string sqlP="select mb.mcat_placa,mb.mcat_placa from dbxschema.MBUS_LOCALIZACION mb,dbxschema.MBUSAFILIADO ma where mb.testa_codigo=5 AND mb.mag_codigo="+agencia+" AND ma.mcat_placa=mb.mcat_placa AND ma.testa_codigo=5 ORDER BY mb.mcat_placa";
			string sqlP="select mcat_placa,'[' concat rtrim(char(mbus_numero)) concat ']['  concat  mcat_placa concat '][' concat coalesce(rtrim(char(mcon_cod)),'***') concat ']' as texto from dbxschema.MBUSAFILIADO where testa_codigo >=0  ORDER BY mcat_placa";
			bind.PutDatasIntoDropDownList(ddlPlaca,sqlP);
			ListItem itm=new ListItem("---Placa---","");
			ddlPlaca.Items.Insert(0,itm);
			//nit Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			pnlAgencia.Visible=true;
		}
		//Guardar
		private void btnGuardar_Click(object sender, System.EventArgs e){
			//Validaciones
			string ruta=ddlRuta.SelectedValue, agencia=ddlAgencia.SelectedValue, programacion=ddlProgramacion.SelectedValue, placa=ddlPlaca.SelectedValue, fecha=txtFecha.Text, configuracion=ddlConfiguracion.SelectedValue, conductor=txtConductor.Text, relevador1=txtRelevador1.Text, relevador2=txtRelevador2.Text, afiliado="";
		   
			int numero;
			//Numero
			try{
				numero=int.Parse(ddlNumero.SelectedValue);
				if(numero<0)throw(new Exception());
			}
			catch{
                Utils.MostrarAlerta(Response, "  Número de viaje no válido, debe ser un número entero.");
				return;
			}
			
			//Validar Ruta-Numero en estado activo
			if(!DBFunctions.RecordExist("SELECT MVIAJE_NUMERO from DBXSCHEMA.MVIAJE where MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+numero.ToString()+" AND (ESTADO_VIAJE='A'OR ESTADO_VIAJE='V')")){
                Utils.MostrarAlerta(Response, "  No se encontró el viaje o no se encuentra activo.");
				return;
			}
			
				
			//Agencia
			if(agencia.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar una agencia ubicada en la ciudad de origen de la ruta.");
				return;
			}
			//Programacion
			//if(programacion.Length==0){
			//	Response.Write("<script language='javascript'>alert('Debe seleccionar un tipo de programación.');</script>");
			//	return;
			// }
			//Horario
			if(lstFecha.SelectedIndex<0)
			{
				if (ddlHora.SelectedIndex<0 && ddlMinuto.SelectedIndex<0)

				{
                    Utils.MostrarAlerta(Response, "  Debe seleccionar un horario. o digitar la hora");
					return;
				}
			}
			//Configuracion
			if(configuracion.Length==0){
                Utils.MostrarAlerta(Response, "  Debe seleccionar la configuración.");
				return;
			}
			if(!DBFunctions.RecordExist("SELECT mele_numepues FROM dbxschema.dconfiguracionbus WHERE mcon_cod="+configuracion+";")){
                Utils.MostrarAlerta(Response, "  La configuración no ha sido diseñada.");
				return;
			}
			//configuracion..
			string ConfiguracionBus = configuracion;
			int PuestosOcupados = Convert.ToInt16(ViewState["PuestosOcupados"].ToString());
			if(placa.Length>0)
			{
				if  (PuestosOcupados > 0)
				{
					int PasajerosBusAnt = Convert.ToInt16(ViewState["Pasajeros"].ToString());
					int Pasajeros = Convert.ToInt16(DBFunctions.SingleData("select  PUESTOS_PASAJEROS FROM DBXSCHEMA.MCONFIGURACIONBUS CB,DBXSCHEMA.mbusafiliado B "+
						" where  mcat_placa='"+placa+"' AND CB.MCON_COD = B.MCON_COD;"));
					if  (PuestosOcupados > Pasajeros)
					{
                        Utils.MostrarAlerta(Response, "  Puestos ocupados mayor a La capacidad de configuración bus -Hay Venta-.");
						return;
					}
					ConfiguracionBus = DBFunctions.SingleData("select  MCON_COD FROM DBXSCHEMA.mbusafiliado B where  mcat_placa='"+placa+"';");
				}
				else
				{
					if(!DBFunctions.RecordExist("SELECT MCAT_PLACA FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"' AND MCON_COD="+configuracion+";"))
					{
                        Utils.MostrarAlerta(Response, "  La configuración no es la misma del vehículo de placas " + placa + ".");
						return;
					}
					ConfiguracionBus = configuracion;
				}
			}
			else
			{
				int Pasajeros = Convert.ToInt16(DBFunctions.SingleData("select  PUESTOS_PASAJEROS FROM DBXSCHEMA.MCONFIGURACIONBUS  "+
					" where  MCON_COD = "+configuracion+";"));
				if  (PuestosOcupados > Pasajeros)
				{
                    Utils.MostrarAlerta(Response, "  Puestos ocupados mayor a La capacidad de configuración -Hay Venta-.");
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
                Utils.MostrarAlerta(Response, "  Fecha no válida, debe tener el formato aaaa-mm-dd y ser mayor o igual a la fecha actual.");
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
                    Utils.MostrarAlerta(Response, "  Debe dar el horario si asigna una programación.");
					return;
				}
				int posDia=(fechaT.DayOfWeek==DayOfWeek.Monday?0:(fechaT.DayOfWeek==DayOfWeek.Tuesday?1:(fechaT.DayOfWeek==DayOfWeek.Wednesday?2:(fechaT.DayOfWeek==DayOfWeek.Thursday?3:(fechaT.DayOfWeek==DayOfWeek.Friday?4:(fechaT.DayOfWeek==DayOfWeek.Saturday?5:6))))));
				DataSet dsCuadro=new DataSet();
				DBFunctions.Request(dsCuadro,IncludeSchema.NO,"SELECT MRUT_LUNES,MRUT_MARTES,MRUT_MIERCOLES,MRUT_JUEVES,MRUT_VIERNES,MRUT_SABADO,MRUT_DOMINGO FROM DBXSCHEMA.MRUTA_CUADRO WHERE MRUT_CODIGO='"+ruta+"' AND MRUT_HORA="+lstFecha.SelectedValue+" AND TPROG_TIPOPROG="+programacion);
				if(dsCuadro.Tables[0].Rows.Count==0 || !dsCuadro.Tables[0].Rows[0][posDia].ToString().Equals("S"))
				{
                    Utils.MostrarAlerta(Response, "  La fecha seleccionada (día " + dias[posDia].ToLower() + ") no existe en el horario seleccionado.");
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
                    Utils.MostrarAlerta(Response, "  Hora no valida.");
					return;
				}
				programacion="NULL";
			}
			
			
			//Afiliado
			if (placa	!= "NULL" && placa	!= "")		
			afiliado=DBFunctions.SingleData("SELECT MNIT_ASOCIADO FROM DBXSCHEMA.MBUSAFILIADO WHERE MCAT_PLACA='"+placa+"'");
			else 
				afiliado = "";
			afiliado=(afiliado.Length==0)?"NULL":"'"+afiliado+"'";
			//Placa
			placa=(placa.Length==0?"NULL":"'"+placa+"'");
			//Conductor
			//Conductor/relevadores
			if(conductor.Length>0 && (conductor.Equals(relevador1)||conductor.Equals(relevador2)))
			{
                Utils.MostrarAlerta(Response, "  El conductor y los relevadores no pueden ser la misma persona.");
				return;
			}
			if(relevador1.Length>0 && relevador1.Equals(relevador2))
			{
                Utils.MostrarAlerta(Response, "  Los relevadores no pueden ser la misma persona.");
				return;
			}
			//Validar conductor
			
			if(conductor.Length>0 && !DBFunctions.RecordExist("Select MNIT_NIT FROM DBXSCHEMA.MEMPLEADO WHERE MNIT_NIT='"+conductor+"' AND PCAR_CODICARGO='CO'"))
			{
                Utils.MostrarAlerta(Response, "  No se encontro en empleado el conductor.");
				return;
			}
			if(conductor.Length==0 && relevador1.Length==0)
			{
                Utils.MostrarAlerta(Response, "  Debe dar el conductor o el relevador.");
				return;
			}
			if(conductor == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				conductor = "";
			}
			if(relevador1 == ConfigurationManager.AppSettings["ConductorRelevadorComodin"])
			{
				relevador1 = "";
			}
			conductor=(conductor.Length==0?"NULL":"'"+conductor+"'");
			//Relevador1
			relevador1=(relevador1.Length==0?"NULL":"'"+relevador1+"'");
			//Relevador2
			relevador2=(relevador2.Length==0?"NULL":"'"+relevador2+"'");
			ArrayList sqlC=new ArrayList();
			//Modificar registro
			
			sqlC.Add("update dbxschema.mviaje set FECHA_SALIDA='"+fechaT.ToString("yyyy-MM-dd")+"',HORA_SALIDA="+horaProg+",TPROG_TIPOPROG="+programacion+",MAG_CODIGO="+agencia+",MCAT_PLACA="+placa+",MCON_COD="+ConfiguracionBus+",MNIT_CONDUCTOR="+conductor+",MNIT_RELEVADOR1="+relevador1+",MNIT_RELEVADOR2="+relevador2+",MNIT_AFILIADO="+afiliado+" where mrut_codigo='"+ruta+"' and mviaje_numero="+numero+";");
			sqlC.Add("update  DBXSCHEMA.MGASTOS_TRANSPORTES set MCAT_PLACA="+placa+" where TDOC_CODIGO = 'ANT' AND MPLA_CODIGO IN "+
					 "(SELECT MPLA_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE  where mrut_codigo='"+ruta+"' and mviaje_numero="+numero+");");
			sqlC.Add("update  DBXSCHEMA.MENCOMIENDAS set MCAT_PLACA="+placa+" where TDOC_CODIGO = 'REM' AND MPLA_CODIGO IN "+
					 "(SELECT MPLA_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE  where mrut_codigo='"+ruta+"' and mviaje_numero="+numero+");");
			//DBFunctions.NonQuery(sqlC);
			//Guardar configuracion puestos
			string ConfiguracionAnt = ViewState["Configuracion"].ToString();
			if (ConfiguracionAnt != configuracion && PuestosOcupados == 0)
			{
				int fil=int.Parse(DBFunctions.SingleData("SELECT MBUS_FILAS FROM DBXSCHEMA.MCONFIGURACIONBUS WHERE MCON_COD="+configuracion));
				int col=int.Parse(DBFunctions.SingleData("SELECT MBUS_COLUMNAS FROM DBXSCHEMA.MCONFIGURACIONBUS WHERE MCON_COD="+configuracion));
				//borrar anteriores  
				sqlC.Add("delete from DBXSCHEMA.MCONFIGURACIONPUESTO WHERE MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+numero+";");
				//DBFunctions.NonQuery(sqlC);
				//insertar Nuevos todos disponibles  
				sqlC.Add("insert into DBXSCHEMA.MCONFIGURACIONPUESTO SELECT '"+ruta+"', "+numero+", mpue_posfila, mpue_poscolumna,mele_numepues,tele_codigo,'DN', cast(null as varchar(10)), cast(null as integer) FROM DBXSCHEMA.DCONFIGURACIONBUS WHERE MCON_COD="+configuracion+";");
			}
			if (ConfiguracionAnt != ConfiguracionBus && PuestosOcupados > 0)
			{
				sqlC.Add("CALL DBXSCHEMA.CAMBIA_CONFIGURACION('"+ruta+"',"+numero+","+ConfiguracionBus+","+ConfiguracionAnt+","+placa+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			}
			if(DBFunctions.Transaction(sqlC))
				Response.Redirect(indexPage+"?process=Comercial.ModificarViaje&act=1?path="+Request.QueryString["path"]);
			else 
			{
				string error_sql=DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "  Error sql: " + error_sql + ".");
				return;
			}
		}

		//Seleccion del numero
		private void ddlNumero_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string numero=ddlNumero.SelectedValue, ruta=ddlRuta.SelectedValue, programacion, agencia, configuracion, placa;
			DatasToControls bind=new DatasToControls();
			//Cambia la ruta
			if(numero.Length==0)
			{
				pnlNumero.Visible=false;
				lstFecha.Items.Clear();
				ddlPlaca.Items.Clear();
				return;
			}
			//Viaje
			DataSet dsViaje=new DataSet();
			DBFunctions.Request(dsViaje,IncludeSchema.NO,"Select mv.FECHA_SALIDA,mv.HORA_SALIDA,mv.TPROG_TIPOPROG,mv.MAG_CODIGO,mv.MCAT_PLACA,mv.MCON_COD,mv.MNIT_CONDUCTOR,mv.MNIT_RELEVADOR1,mv.MNIT_RELEVADOR2,mv.MNIT_AFILIADO,mv.MNIT_DESPACHADOR,mv.HORA_DESPACHO,mv.FECHA_LLEGADA,mv.HORA_LLEGADA,mv.FECHA_LIQUIDACION,mv.VALOR_INGRESOS,mv.VALOR_EGRESOS,mc.MNIT_NOMBRES CONCAT ' 'CONCAT mc.MNIT_APELLIDOS AS NOMCONDUCTOR,mr1.MNIT_NOMBRES CONCAT ' 'CONCAT mr1.MNIT_APELLIDOS AS NOMRELEVADOR1,mr2.MNIT_NOMBRES CONCAT ' 'CONCAT mr2.MNIT_APELLIDOS AS NOMRELEVADOR2 FROM DBXSCHEMA.MVIAJE mv LEFT JOIN DBXSCHEMA.MNIT mc ON mc.MNIT_NIT=mv.MNIT_CONDUCTOR LEFT JOIN DBXSCHEMA.MNIT mr1 ON mr1.MNIT_NIT=mv.MNIT_RELEVADOR1 LEFT JOIN DBXSCHEMA.MNIT mr2 ON mr2.MNIT_NIT=mv.MNIT_RELEVADOR2 WHERE MRUT_CODIGO='"+ruta+"' AND MVIAJE_NUMERO="+numero);
			if(dsViaje.Tables[0].Rows.Count==0){
                Utils.MostrarAlerta(Response, "  No se encontró el viaje " + numero + " de la ruta " + ruta + ".");
				return;
			}
			//Cargar controles
			//Agencia
			agencia=dsViaje.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			ddlAgencia.SelectedIndex=ddlAgencia.Items.IndexOf(ddlAgencia.Items.FindByValue(agencia));
			//Programacion
			programacion=dsViaje.Tables[0].Rows[0]["TPROG_TIPOPROG"].ToString();
			ddlProgramacion.SelectedIndex=ddlProgramacion.Items.IndexOf(ddlProgramacion.Items.FindByValue(programacion));
			//Horarios
			CargarHorarios(ruta,programacion);
			lstFecha.SelectedIndex=lstFecha.Items.IndexOf(lstFecha.Items.FindByValue(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"].ToString()));
			int Minutos = Convert.ToInt16((dsViaje.Tables[0].Rows[0]["HORA_SALIDA"].ToString()));
			int Horas   = Minutos / 60;
			int HoraMinuto = Minutos%60;
			ddlHora.SelectedIndex= ddlHora.Items.IndexOf(ddlHora.Items.FindByValue(Horas.ToString()));
			ddlMinuto.SelectedIndex=ddlMinuto.Items.IndexOf(ddlMinuto.Items.FindByValue(HoraMinuto.ToString()));
			//Fecha
			txtFecha.Text=Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd");
			pnlNumero.Visible=true;
			//Configuracion
			configuracion=dsViaje.Tables[0].Rows[0]["MCON_COD"].ToString();
			ViewState["Configuracion"] = configuracion;
			ddlConfiguracion.SelectedIndex=ddlConfiguracion.Items.IndexOf(ddlConfiguracion.Items.FindByValue(configuracion));
			int PuestosOcupados=Convert.ToInt16(DBFunctions.SingleData("select count(*)   FROM DBXSCHEMA.mconfiguracionpuesto "+
				"where  mviaje_numero="+ddlNumero.SelectedValue+" and MRUT_CODIGO='"+ruta+"' and  TELE_CODIGO   = 'SP' and num_documento is not null;"));
		/*	if (PuestosOcupados > 0)
				ddlConfiguracion.Enabled=false;
			else */
				ddlConfiguracion.Enabled=true;
			ViewState["PuestosOcupados"] = PuestosOcupados;
			if(dsViaje.Tables[0].Rows[0]["MCAT_PLACA"]!=null)
			{
				placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
				ddlPlaca.SelectedIndex=ddlPlaca.Items.IndexOf(ddlPlaca.Items.FindByValue(placa));
			}
			ViewState["Pasajeros"] = Convert.ToInt16(DBFunctions.SingleData("select PUESTOS_PASAJEROS  FROM DBXSCHEMA.MCONFIGURACIONBUS "+
					 "where mcon_cod="+configuracion+";")); 
				
			//Conductor
			if(dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]!=null){
				txtConductor.Text=dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"].ToString();
				txtConductora.Text=dsViaje.Tables[0].Rows[0]["NOMCONDUCTOR"].ToString();
			}
			//Relevador1
			if(dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]!=null)
			{
				txtRelevador1.Text=dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"].ToString();
				txtRelevador1a.Text=dsViaje.Tables[0].Rows[0]["NOMRELEVADOR1"].ToString();
			}
			//Relevador2
			if(dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR2"]!=null)
			{
				txtRelevador2.Text=dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR2"].ToString();
				txtRelevador2a.Text=dsViaje.Tables[0].Rows[0]["NOMRELEVADOR2"].ToString();

			}
		}
	}
}