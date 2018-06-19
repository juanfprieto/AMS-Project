namespace AMS.Vehiculos
{
	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using Ajax;
    using AMS.Tools;


	/// <summary>
	///		Descripción breve de AMS_Vehiculos_ProcesosPostventa.
	/// </summary>
	public partial class AMS_Vehiculos_ProcesosPostventa : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label lblDocPropietario;
		protected System.Web.UI.WebControls.Label lblNombrePropietario;
		protected System.Web.UI.WebControls.Label lblDireccionPropietario;
		protected System.Web.UI.WebControls.Label lblCiudadPropietario;
		protected System.Web.UI.WebControls.Label lblTelefonoPropietario;
		protected System.Web.UI.WebControls.Label lblCelularPropietario;
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{
				string proceso=Request.QueryString["prc"];
				string concesionario=Request.QueryString["cnc"];
				string filtro="",listaPrecios="",error="",nomProceso="";
				DatasToControls bind=new DatasToControls();
				DataSet dsPGarantia=new DataSet();
				DBFunctions.Request(dsPGarantia,IncludeSchema.NO,
					"SELECT PGAR_CODIGO CODIGO,PGAR_DESCRIPCION DESCRIPCION FROM PGARANTIA WHERE TCAU_CODIGO='I';"+
					"SELECT PGAR_CODIGO CODIGO,PGAR_DESCRIPCION DESCRIPCION FROM PGARANTIA WHERE TCAU_CODIGO='C';");
				DataRow drNull=dsPGarantia.Tables[0].NewRow();
				drNull[0]="";
				drNull[1]="--no especificado--";
				dsPGarantia.Tables[0].Rows.InsertAt(drNull,0);
				drNull=dsPGarantia.Tables[1].NewRow();
				drNull[0]="";
				drNull[1]="--no especificado--";
				dsPGarantia.Tables[1].Rows.InsertAt(drNull,0);
				bind.PutDatasIntoDropDownList(txtNITPropietarioe,
					"select pciu_codigo,pciu_nombre from pciudad order by pciu_nombre;");

				if((proceso!="A"&&proceso!="R"&&proceso!="G") || (concesionario!="T"&&concesionario!="P"))
					error="Proceso no valido.";

				if(proceso!="G")
					dgrKitsMantenimientoOperaciones.Columns[4].Visible=dgrKitsMantenimientoOperaciones.Columns[5].Visible=false;
				
				txtPlaca.Enabled=!(proceso!="A");
				//Lista de precios de items de garantia
				listaPrecios=DBFunctions.SingleData("SELECT PPRE_CODIGO FROM CTALLER;");
				if(listaPrecios.Length==0)
					error="Debe definir la lista de precios.";
				
				//Filtro Vehiculos, para alistamiento muestra los facturados, para los demas muestra facturados o disponibles
				if(proceso=="A")
					filtro="SELECT mv.mcat_vin VIN,mc.mcat_motor MOTOR, mc.pcat_codigo catalogo, mc.mcat_placa as placa FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin=mc.mcat_vin and mv.test_tipoesta=40;";
				else
					filtro="SELECT mv.mcat_vin VIN,mc.mcat_motor MOTOR, mc.pcat_codigo catalogo, mc.mcat_placa as placa FROM mvehiculo mv, mcatalogovehiculo mc, MVEHICULOPOSTVENTA mp WHERE mv.mcat_vin=mc.mcat_vin and (mv.test_tipoesta=40 or mv.test_tipoesta=50 or mv.test_tipoesta=60) and mv.mcat_vin=mp.mcat_vin;" ;
				filtro="ModalDialog(txtObj,'"+filtro+"',new Array(),1);";
				
				txtFechaProceso.Text=txtFechaFact.Text=DateTime.Now.ToString("yyyy-MM-dd");
				plcDistribuidorV.Visible=(concesionario=="P");
				
				ViewState["INCIDENTES"]=dsPGarantia.Tables[0];
				ViewState["CAUSALES"]=dsPGarantia.Tables[1];
				ViewState["PROCESO"]=proceso;
				ViewState["CONCESIONARIO"]=concesionario;
				ViewState["FILTRO_VEHICULOS"]=filtro;
				ViewState["LISTA_PRECIOS"]=listaPrecios;
				
				if(error.Length>0){
					lblError.Text=error;
					plcSeleccion.Visible=false;
					return;
				}

				if(Request.QueryString["pO"]!=null && Request.QueryString["nO"]!=null)
				{
					if(proceso=="A")nomProceso="El alistamiento ha sido creado ";
					else if(proceso=="R")nomProceso="La revisión ha sido creada ";
					else if(proceso=="G")nomProceso="La garantía ha sido creada ";
                    Utils.MostrarAlerta(Response, "" + nomProceso + "bajo la orden de trabajo: " + Request.QueryString["pO"] + "-" + Request.QueryString["nO"] + ".");
				}
				
			}
			Page.SmartNavigation=true;
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

		}
		#endregion

		//Seleccionar el vehiculo
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			string proceso=ViewState["PROCESO"].ToString();
			string concesionario=ViewState["CONCESIONARIO"].ToString();
			string nitDistribuidor="",nombreDist;

			#region Seleccion Distribuidor
			//Distribuidor/Concesionario
			//Taller (El usuario debe tener concesionario asignado)
			if(concesionario=="T")
			{
				nitDistribuidor=DBFunctions.SingleData(
					"SELECT MC.MNIT_NIT FROM MCONCESIONARIOUSUARIO MC,SUSUARIO SU "+
					"WHERE SU.SUSU_LOGIN='"+HttpContext.Current.User.Identity.Name.ToLower()+"' AND SU.SUSU_CODIGO=MC.SUSU_CODIGO;");
				if(nitDistribuidor.Length==0)
				{
                    Utils.MostrarAlerta(Response, "El usuario no tiene un concesionario asociado para el proceso.");
					return;
				}
			}
			//Planta (pueden seleccionar cualquier concesionario)
			else if(concesionario=="P")
			{
				nitDistribuidor=txtNitDistribuidor.Text;
				if(nitDistribuidor.Length==0)
				{
                    Utils.MostrarAlerta(Response, "Debe seleccionar el concesionario.");
					return;
				}
			}

			//Categoria
			string categoria=DBFunctions.SingleData("SELECT PC.PCAT_VALOHORA FROM MCONCESIONARIO MC,PCATEGORIACONCESIONARIO PC WHERE MC.MNIT_NIT='"+nitDistribuidor+"' AND PC.PCAT_CATECONC=MC.PCAT_CATECONC;");
			if(categoria.Length==0)
			{
                Utils.MostrarAlerta(Response, "No se ha especificado la categoría del concesionario.");
				return;
			}
			ViewState["VALOR_CATEGORIA"]=Convert.ToDouble(categoria);

			#endregion
			
			#region Validaciones
			//Validar Vehiculo
			if(txtVINVehiculo.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar el vehículo.");
				return;
			}
			
			if(proceso=="A" && !DBFunctions.RecordExist("SELECT mv.mcat_vin VIN,mc.mcat_motor MOTOR FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin='"+txtVINVehiculo.Text+"' and mv.mcat_vin=mc.mcat_vin and mv.test_tipoesta=40;"))
			{
                Utils.MostrarAlerta(Response, "El vehículo no está disponible.");
				return;
			}
			if(proceso=="A" && DBFunctions.RecordExist("select * from MVEHICULOPOSTVENTA where mcat_vin='"+txtVINVehiculo.Text+"';")){
                Utils.MostrarAlerta(Response, "Ya realizó el alistamiento del vehículo.");
				return;
			}
			if(proceso!="A" && !DBFunctions.RecordExist("select * from MVEHICULOPOSTVENTA where mcat_vin='"+txtVINVehiculo.Text+"';")){
                Utils.MostrarAlerta(Response, "No ha realizado el alistamiento del vehículo.");
				return;
			}
			
			//DatosFactura
			if(proceso!="A"){
				DataSet dsFactura=new DataSet();
				DBFunctions.Request(dsFactura,IncludeSchema.NO,
					"select MV.MVEH_PREFFACTVENT PREF, MV.MVEH_NUMEFACTVENT NUMERO, MV.MVEH_FECHVENT FECHA, MV.MNIT_NITDISTR NIT_DIST, MN.MNIT_APELLIDOS NOMBRE_DIST, MN.MNIT_DIRECCION DIRECCION_DIST "+
					"FROM MVEHICULOPOSTVENTA MV, MNIT MN "+
					"WHERE MN.MNIT_NIT=MV.MNIT_NITDISTR AND MV.MCAT_VIN='"+txtVINVehiculo.Text+"';");
				if(dsFactura.Tables[0].Rows.Count==0)
				{
                    Utils.MostrarAlerta(Response, "Error consultando el alistamiento del vehículo.");
					return;
				}
				lblFactDitri.Text="<table><tr><td valign='top'>"+
					dsFactura.Tables[0].Rows[0]["NOMBRE_DIST"].ToString()+"<br>"+
					dsFactura.Tables[0].Rows[0]["NIT_DIST"].ToString()+"<br>"+
					dsFactura.Tables[0].Rows[0]["DIRECCION_DIST"].ToString()+"</td></tr></table>";
				lblFactFecha.Text=Convert.ToDateTime(dsFactura.Tables[0].Rows[0]["FECHA"]).ToString("yyyy-MM-dd");
				lblFactIni.Text=dsFactura.Tables[0].Rows[0]["PREF"].ToString()+" "+dsFactura.Tables[0].Rows[0]["NUMERO"].ToString();
				
				txtPrefFactura.Text=dsFactura.Tables[0].Rows[0]["PREF"].ToString();
				txtNumFactura.Text=dsFactura.Tables[0].Rows[0]["NUMERO"].ToString();
				txtFechaFact.Text=Convert.ToDateTime(dsFactura.Tables[0].Rows[0]["FECHA"]).ToString("yyyy-MM-dd");
				
				plcInfoInicial.Visible=true;
				plcFacturaInicial.Visible=false;
				plcOrdenProceso.Visible=true;
			}
			else{
				plcOrdenProceso.Visible=false;
				plcInfoInicial.Visible=false;
				plcFacturaInicial.Visible=true;
			}
			plcProceso.Visible=true;
			plcSeleccion.Visible=false;
			#endregion
			
			#region Datos Vehiculo
			//Datos Vehiculo
			DataSet dsVehiculo=new DataSet();
			string catalogo;
			DBFunctions.Request(dsVehiculo,IncludeSchema.NO,"SELECT MC.MCAT_PLACA, MV.MCAT_VIN,MC.MCAT_MOTOR, MC.PCAT_CODIGO,PC.PCOL_DESCRIPCION,MC.MCAT_ANOMODE, MC.MCAT_NUMEULTIKILO FROM MCATALOGOVEHICULO MC,MVEHICULO MV,PCOLOR PC WHERE MC.MCAT_VIN=MV.MCAT_VIN AND PC.PCOL_CODIGO=MC.PCOL_CODIGO AND MV.MCAT_VIN='"+txtVINVehiculo.Text+"';");
			lblVINVehiculo.Text=dsVehiculo.Tables[0].Rows[0]["MCAT_VIN"].ToString();
			lblMotorVehiculo.Text=dsVehiculo.Tables[0].Rows[0]["MCAT_MOTOR"].ToString();
			catalogo=dsVehiculo.Tables[0].Rows[0]["PCAT_CODIGO"].ToString();
			lblCatalogoVehiculo.Text=catalogo;
			lblColorVehiculo.Text=dsVehiculo.Tables[0].Rows[0]["PCOL_DESCRIPCION"].ToString();
			lblAnoCatalogo.Text=dsVehiculo.Tables[0].Rows[0]["MCAT_ANOMODE"].ToString();
			txtPlaca.Text=dsVehiculo.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			txtKilometraje.Text=Convert.ToUInt32(dsVehiculo.Tables[0].Rows[0]["MCAT_NUMEULTIKILO"]).ToString("#,##0");
			ViewState["PLACA"]=txtPlaca.Text;
			//Garantia
			DataSet dsGarantia=new DataSet();
			int mesesG,kilosG;
			DBFunctions.Request(dsGarantia,IncludeSchema.NO,"SELECT PCAT_KILOMETRAJEGARANTIA KILOM,PCAT_MESESGARANTIA MESES FROM PCATALOGOVEHICULO PC, MCATALOGOVEHICULO MC WHERE MC.MCAT_VIN='"+txtVINVehiculo.Text+"' AND MC.PCAT_CODIGO=PC.PCAT_CODIGO;");
			if(dsGarantia.Tables[0].Rows.Count>0)
			{
				mesesG=Convert.ToInt16(dsGarantia.Tables[0].Rows[0]["MESES"]);
				kilosG=Convert.ToInt16(dsGarantia.Tables[0].Rows[0]["KILOM"]);
				lblMesesGarantia.Text=mesesG.ToString();
				lblKiloGarantia.Text=kilosG.ToString();
			}
			#endregion
			
			#region Datos Distribuidor
			//Datos Distribuidor
			ViewState["DISTRIBUIDOR"]=nitDistribuidor;
			if(nitDistribuidor.Length>0)
			{
				DataSet dsDistribuidor=new DataSet();
				DBFunctions.Request(dsDistribuidor,IncludeSchema.NO,
					"SELECT MN.MNIT_NIT, MN.TNIT_TIPONIT, MN.MNIT_DIRECCION, MN.MNIT_TELEFONO "+
					"FROM MNIT MN WHERE MN.MNIT_NIT='"+nitDistribuidor+"';");
                nombreDist = DBFunctions.SingleData("select MNIT_apellidoS || ' ' || MNIT_NOMBRES || ' ' || COALESCE(MNIT_ESTABLECIMIENTO,'') from mnit where mnit_nit='" + nitDistribuidor + "';");
				lblNITDistribuidor.Text=nitDistribuidor;
				lblNombreDistribuidor.Text=nombreDist;
				lblDireccionDistribuidor.Text=dsDistribuidor.Tables[0].Rows[0]["MNIT_DIRECCION"].ToString();
				lblTelefonoDistribuidor.Text=dsDistribuidor.Tables[0].Rows[0]["MNIT_TELEFONO"].ToString();
			}
			lblCategoria.Text=DBFunctions.SingleData("SELECT MC.PCAT_CATECONC FROM MCONCESIONARIO MC WHERE MC.MNIT_NIT='"+nitDistribuidor+"';");
			#endregion

			#region Datos Propietario
			//Datos Propietario
			#region Anterior
			/*
			DataSet dsPropietario=new DataSet();
			string nombreProp,nitPropietario;
			string grupoCatalogo=DBFunctions.SingleData("SELECT PGRU_GRUPO FROM PCATALOGOVEHICULO WHERE PCAT_CODIGO='"+lblCatalogoVehiculo.Text+"';");
			
			DBFunctions.Request(dsPropietario,IncludeSchema.NO,
				"SELECT MN.MNIT_NIT, MN.TNIT_TIPONIT, MN.MNIT_DIRECCION, MN.MNIT_TELEFONO, MN.MNIT_CELULAR, MN.PCIU_CODIGO "+
				"FROM MNIT MN, MCATALOGOVEHICULO MC "+
				"WHERE MC.MNIT_NIT=MN.MNIT_NIT AND MC.MCAT_VIN='"+txtVINVehiculo.Text+"';");
			nitPropietario=dsPropietario.Tables[0].Rows[0]["MNIT_NIT"].ToString();
			ViewState["PROPIETARIO"]=nitPropietario;

			if(dsPropietario.Tables[0].Rows[0]["TNIT_TIPONIT"].ToString()=="N")
				nombreProp=DBFunctions.SingleData("select mnit_apellidos from mnit where mnit_nit='"+nitPropietario+"';");
			else
				nombreProp=DBFunctions.SingleData("select MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_NOMBRE2 CONCAT ' ' CONCAT MNIT_APELLIDOS CONCAT ' ' CONCAT MNIT_APELLIDO2 from mnit where mnit_nit='"+nitPropietario+"';");
			txtNITPropietario.Text=dsPropietario.Tables[0].Rows[0]["MNIT_NIT"].ToString();
			txtNITPropietarioa.Text=nombreProp;
			txtNITPropietariob.Text=dsPropietario.Tables[0].Rows[0]["MNIT_DIRECCION"].ToString();
			txtNITPropietarioc.Text=dsPropietario.Tables[0].Rows[0]["MNIT_TELEFONO"].ToString();
			txtNITPropietariod.Text=dsPropietario.Tables[0].Rows[0]["MNIT_CELULAR"].ToString();
			txtNITPropietarioe.SelectedIndex=txtNITPropietarioe.Items.IndexOf(txtNITPropietarioe.Items.FindByValue(dsPropietario.Tables[0].Rows[0]["PCIU_CODIGO"].ToString()));
			*/
			#endregion
			string grupoCatalogo=DBFunctions.SingleData("SELECT PGRU_GRUPO FROM PCATALOGOVEHICULO WHERE PCAT_CODIGO='"+lblCatalogoVehiculo.Text+"';");
			if(proceso!="A"){
				DataSet dsPropietario=new DataSet();
				string nitPropietario;
				
				DBFunctions.Request(dsPropietario,IncludeSchema.NO,
					"SELECT * FROM MVEHICULOPOSTVENTA where mcat_vin='"+txtVINVehiculo.Text+"';");
				
				nitPropietario=dsPropietario.Tables[0].Rows[0]["MNIT_NITCLIE"].ToString();
				ViewState["PROPIETARIO"]=nitPropietario;

				txtNITPropietario.Text=dsPropietario.Tables[0].Rows[0]["MNIT_NITCLIE"].ToString();
				txtNITPropietarioa.Text=dsPropietario.Tables[0].Rows[0]["MNIT_NOMBRECLIE"].ToString();
				txtNITPropietariob.Text=dsPropietario.Tables[0].Rows[0]["MNIT_DIRECCIONCLIE"].ToString();
				txtNITPropietarioc.Text=dsPropietario.Tables[0].Rows[0]["MNIT_TELEFONOCLIE"].ToString();
				txtNITPropietariod.Text=dsPropietario.Tables[0].Rows[0]["MNIT_CELULARCLIE"].ToString();
				txtNITPropietarioe.SelectedIndex=txtNITPropietarioe.Items.IndexOf(txtNITPropietarioe.Items.FindByValue(dsPropietario.Tables[0].Rows[0]["PCIU_CODIGOCLIE"].ToString()));
			}
			#endregion
			
			#region Lista de chequeo
			//Mantenimiento programado
			string kitAlistamiento=DBFunctions.SingleData("SELECT CS.PKIT_KITPSOT FROM CTALLER CS;");
			if(proceso!="G")
			{
				if(proceso!="A")
				{
					//Cargar Kits del grupo catalogo no usados
					DatasToControls bind=new DatasToControls();
					ddlMantenimiento.Items.Clear();
					bind.PutDatasIntoDropDownList(ddlMantenimiento,
						"SELECT PK.PKIT_CODIGO, PK.PKIT_NOMBRE "+
						"FROM PMANTENIMIENTOPROGRAMADO PM, PKIT PK, PCATALOGOVEHICULO PC "+
						"WHERE PM.PGRU_CODIGO=PC.PGRU_GRUPO AND PK.PKIT_CODIGO=PM.PKIT_CODIGO AND "+
						"PC.PGRU_GRUPO='"+grupoCatalogo+"' AND PC.PCAT_CODIGO='"+catalogo+"' AND "+
						"PK.PKIT_CODIGO<>'"+kitAlistamiento+"' AND PK.PKIT_CODIGO NOT IN("+
						" SELECT d.pkit_codigo from DBXSCHEMA.DORDENKIT d, dbxschema.morden mo "+
						" where mcat_vin='"+txtVINVehiculo.Text+"' and d.pdoc_codigo=mo.pdoc_codigo and d.mord_numeorde=mo.mord_numeorde"+
						");");
					ddlMantenimiento.Items.Insert(0,new ListItem("--seleccione--",""));
				}
				else
				{
					//Cargar solo kit de alistamiento
					DatasToControls bind=new DatasToControls();
					ddlMantenimiento.Items.Clear();
					if(kitAlistamiento.Length==0)
					{
                        Utils.MostrarAlerta(Response, "El kit de alistamiento no se ha definido o no esta asociado al grupo de catálogos del vehículo seleccionado.");
						return;
					}
					bind.PutDatasIntoDropDownList(ddlMantenimiento,
						"SELECT PKIT_CODIGO, PKIT_NOMBRE "+
						"FROM PKIT "+
						"WHERE PKIT_CODIGO='"+kitAlistamiento+"';");
					//ddlMantenimiento.Items.Insert(0,new ListItem("--seleccione--",""));
				}
			}
			else
			{
				//Autorizaciones de garantias
				DataSet dsAutorizaciones=new DataSet();
				DBFunctions.Request(dsAutorizaciones,IncludeSchema.NO,"SELECT MGAR_NUMERO CODIGO, MGAR_FECHA FECHA, TRES_SINO ESTADO, MGAR_OBSERVAC OBSERVACION FROM DBXSCHEMA.MGARANTIAAUTORIZACION WHERE MCAT_VIN='"+txtVINVehiculo.Text+"' AND MORD_NUMEORDEN IS NULL;");
				dgrAutorizacionesGarantia.DataSource=dsAutorizaciones.Tables[0];
				dgrAutorizacionesGarantia.DataBind();
			}
			#endregion

			#region Historial
			if(proceso!="A"){
				DataSet dsRevAnt=new DataSet();
				DBFunctions.Request(dsRevAnt,IncludeSchema.NO,
					"Select "+
                    "pk.pkit_codigo codigo, pk.pkit_nombre nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje, mo.mord_obseclie obsecliente, mo.mord_obserece obsetaller " +
					"from DBXSCHEMA.pkit pk, dbxschema.dordenkit dor, dbxschema.morden mo "+
					"where pk.pkit_codigo=dor.pkit_codigo and dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and mcat_vin='"+txtVINVehiculo.Text+"';");
				dgrMantenimientosAnt.DataSource=dsRevAnt.Tables[0];
				dgrMantenimientosAnt.DataBind();
                //txtObservacionesCliente.Text = DBFunctions.SingleData("select mord_obseclie from morden where mcat_vin='" + txtVINVehiculo.Text + "' order by mord_numeorde desc fetch first 1 rows only;");
                //txtObservacionesTaller.Text  = DBFunctions.SingleData("select mord_obserece from morden where mcat_vin='" + txtVINVehiculo.Text + "' order by mord_numeorde desc fetch first 1 rows only;");
                txtObservacionesCliente.Text = "";
                txtObservacionesTaller.Text = "";
                DataSet dsOperAnt=new DataSet();
				DBFunctions.Request(dsOperAnt,IncludeSchema.NO,
					"Select pt.ptem_operacion codigo, pt.ptem_descripcion nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje, pt.ptem_valooper precio "+
					"from dbxschema.dordenoperacion dor, dbxschema.morden mo, dbxschema.ptempario pt "+
					"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and mcat_vin='"+txtVINVehiculo.Text+"' "+
					"and pt.ptem_operacion=dor.ptem_operacion order by mord_creacion desc;");
				dgrOperacionesAnt.DataSource=dsOperAnt.Tables[0];
				dgrOperacionesAnt.DataBind();
				DataSet dsRepAnt=new DataSet();
				DBFunctions.Request(dsRepAnt,IncludeSchema.NO,
					"Select mcat_vin,mi.mite_codigo codigo, mi.mite_NOMBRE nombre, mo.mord_creacion fecha, mo.mord_kilometraje kilometraje "+
					"from dbxschema.dordenitemspostventa dor, dbxschema.morden mo, dbxschema.mitems mi "+
					"where dor.pdoc_codigo=mo.pdoc_codigo and dor.mord_numeorde=mo.mord_numeorde and mcat_vin='"+txtVINVehiculo.Text+"' "+
					"and mi.mite_codigo=dor.mite_codigo order by mord_creacion desc;");
				dgrRepuestosAnt.DataSource=dsRepAnt.Tables[0];
				dgrRepuestosAnt.DataBind();
				plcMantenimientosAnt.Visible=true;
			}
			else plcMantenimientosAnt.Visible=false;
			#endregion

			plcAutorizacionesGarantia.Visible=plcOperaciones.Visible=plcRepuestos.Visible=plcSelProcedimiento.Visible=plcSelRepuestos.Visible=(proceso=="G");
			plcManteniProg.Visible=!(proceso=="G");
			
			ViewState["GRUPO_CATALOGO"]=grupoCatalogo;
			ViewState["REPUESTOS"]=new DataTable();
			ViewState["OPERACIONES"]=new DataTable();

			if(proceso=="A" && ddlMantenimiento.Items.Count>0)
				Cambia_MantenimientoProg(sender,e);
		}

		
		//Cambia el combo de mantenimiento programado
		protected void Cambia_MantenimientoProg(Object  Sender, EventArgs e)
		{
			string grupoCatalogo=ViewState["GRUPO_CATALOGO"].ToString();
			string operacion=ViewState["PROCESO"].ToString();
			string concesionario=ViewState["CONCESIONARIO"].ToString();
			double categoria=Convert.ToDouble(ViewState["VALOR_CATEGORIA"]);
			DataSet dsOperaciones=new DataSet();
			DataSet dsRepuestos=new DataSet();

			//Operaciones
			DBFunctions.Request(dsOperaciones,IncludeSchema.NO,
				"SELECT PT.PTEM_OPERACION CODIGO, PT.PTEM_DESCRIPCION NOMBRE, 1 as usar, "+
				"CASE WHEN PT.PTEM_INDIGENERIC='S' THEN PT.PTEM_TIEMPOESTANDAR "+
				"ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG WHERE PTG.PTIE_GRUPCATA='"+grupoCatalogo+"' AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				"END TIEMPO, "+
				"CASE WHEN PT.PTEM_INDIGENERIC='S' THEN PT.PTEM_TIEMPOESTANDAR "+
				"ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG WHERE PTG.PTIE_GRUPCATA='"+grupoCatalogo+"' AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				"END * "+categoria+" PRECIO, "+
				" '' INCIDENTE, '' CAUSAL "+
				"FROM PTEMPARIO PT, mkitoperacion mko "+
				"where mko.mkit_codikitoper='"+ddlMantenimiento.SelectedValue+"' and pt.ptem_operacion=mko.mkit_operacion;");
			dgrKitsMantenimientoOperaciones.DataSource=dsOperaciones.Tables[0];
			dgrKitsMantenimientoOperaciones.DataBind();
			ViewState["OPERACIONES"]=dsOperaciones.Tables[0];
			plcOperaciones.Visible=(dsOperaciones.Tables[0].Rows.Count>0);

			//Repuestos
			if(operacion!="A")
			{
				DBFunctions.Request(dsRepuestos,IncludeSchema.NO,
					"SELECT mi.mite_nombre nombre, mi.mite_codigo codigo, 1 as cantidad, 1 as usar, mp.mpre_precio as precio "+
					"FROM mkititem mki, mitems mi, mprecioitem mp "+
					"where mki.mkit_codikit='"+ddlMantenimiento.SelectedValue+"' and mi.mite_codigo=mki.mkit_coditem and mi.mite_codigo=mp.mite_codigo and mp.ppre_codigo='"+ViewState["LISTA_PRECIOS"]+"';");
				dgrKitsMantenimientoRepuestos.DataSource=dsRepuestos.Tables[0];
				dgrKitsMantenimientoRepuestos.DataBind();
				ViewState["REPUESTOS"]=dsRepuestos.Tables[0];
				plcRepuestos.Visible=(dsRepuestos.Tables[0].Rows.Count>0);
			}
		}
		
		
		//Agregar un procedimiento
		protected void AgregarProcedimiento(object sender, EventArgs e )
		{
			DataTable dtOperaciones=(DataTable)ViewState["OPERACIONES"];
			string grupoCatalogo=ViewState["GRUPO_CATALOGO"].ToString();
			double categoria=Convert.ToDouble(ViewState["VALOR_CATEGORIA"]);
			
			if(txtProcedimiento.Text.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar la operacion.");
				return;}
			
			DataSet dsOperaciones=new DataSet();
			DBFunctions.Request(dsOperaciones,IncludeSchema.NO,
				"SELECT PT.PTEM_OPERACION CODIGO, PT.PTEM_DESCRIPCION NOMBRE, 1 USAR, "+
				"CASE WHEN PT.PTEM_INDIGENERIC='S' THEN PT.PTEM_TIEMPOESTANDAR "+
				"ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG WHERE PTG.PTIE_GRUPCATA='"+grupoCatalogo+"' AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				"END TIEMPO, "+
				"CASE WHEN PT.PTEM_INDIGENERIC='S' THEN PT.PTEM_TIEMPOESTANDAR "+
				"ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG WHERE PTG.PTIE_GRUPCATA='"+grupoCatalogo+"' AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
				"END * "+categoria+" PRECIO, "+
				"'' INCIDENTE, '' CAUSAL "+
				"FROM PTEMPARIO PT "+
				"where pt.ptem_operacion='"+txtProcedimiento.Text+"';");
			
			if(dsOperaciones.Tables[0].Rows.Count==0){
                Utils.MostrarAlerta(Response, "No se pudo consultar la información de la operación.");
				return;}
			if(dtOperaciones.Rows.Count==0)
				dtOperaciones=dsOperaciones.Tables[0];
			else{
				if(dtOperaciones.Select("CODIGO='"+txtProcedimiento.Text+"'").Length>0)
				{
                    Utils.MostrarAlerta(Response, "Ya agregó la operación.");
					return;}
				DataRow drProcedimiento=dtOperaciones.NewRow();
				for(int n=0;n<dsOperaciones.Tables[0].Columns.Count;n++)
					drProcedimiento[n]=dsOperaciones.Tables[0].Rows[0][n];
				dtOperaciones.Rows.Add(drProcedimiento);
				SeleccionarOperaciones(dgrKitsMantenimientoOperaciones,dtOperaciones);
			}
			dgrKitsMantenimientoOperaciones.DataSource=dtOperaciones;
			dgrKitsMantenimientoOperaciones.DataBind();
			ViewState["OPERACIONES"]=dtOperaciones;
			txtProcedimiento.Text=txtProcedimientoa.Text="";
		}

		//Agregar un repuesto
		protected void AgregarRepuesto(object sender, EventArgs e )
		{
			DataTable dtRepuestos=(DataTable)ViewState["REPUESTOS"];
			string errores;
			if(txtRepuesto.Text.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe seleccionar el repuesto.");
				return;}
			
			DataSet dsRepuestos=new DataSet();
			DBFunctions.Request(dsRepuestos,IncludeSchema.NO,
				"SELECT mi.mite_nombre nombre, mi.mite_codigo codigo, 1 as cantidad, 1 as usar, mp.mpre_precio as precio "+
				"FROM mitems mi, mprecioitem mp "+
				"where mi.mite_codigo='"+txtRepuesto.Text+"' and mi.mite_codigo=mp.mite_codigo and mp.ppre_codigo='"+ViewState["LISTA_PRECIOS"]+"';");
			
			if(dsRepuestos.Tables[0].Rows.Count==0){
                Utils.MostrarAlerta(Response, "No se pudo consultar la información del repuesto, revise que exista en la lista de precios " + ViewState["LISTA_PRECIOS"] + ".");
				return;}
			if(dtRepuestos.Rows.Count==0)
				dtRepuestos=dsRepuestos.Tables[0];
			else
			{
				if(dtRepuestos.Select("CODIGO='"+txtRepuesto.Text+"'").Length>0)
				{
                    Utils.MostrarAlerta(Response, "Ya agregó el repuesto.");
					return;}
				errores=SeleccionarRepuestos(dgrKitsMantenimientoRepuestos,dtRepuestos);
				if(errores.Length>0)
				{
                    Utils.MostrarAlerta(Response, "" + errores + "");
					return;
				}
				DataRow drRepuesto=dtRepuestos.NewRow();
				for(int n=0;n<dsRepuestos.Tables[0].Columns.Count;n++)
					drRepuesto[n]=dsRepuestos.Tables[0].Rows[0][n];
				dtRepuestos.Rows.Add(drRepuesto);
				
			}
			dgrKitsMantenimientoRepuestos.DataSource=dtRepuestos;
			dgrKitsMantenimientoRepuestos.DataBind();
			ViewState["REPUESTOS"]=dtRepuestos;
			txtRepuesto.Text=txtRepuestoa.Text="";
		}
		
		//Guardar proceso
		protected void Aceptar(object sender, EventArgs e )
		{
			string proceso=ViewState["PROCESO"].ToString();
			string concesionario=ViewState["CONCESIONARIO"].ToString();
			string nitDistribuidor=ViewState["DISTRIBUIDOR"].ToString();
			string nitPropietario=txtNITPropietario.Text;
			string prefOrdenA;
			ArrayList sqlA=new ArrayList();
			DataTable dtOperaciones=(DataTable)ViewState["OPERACIONES"];
			DataTable dtRepuestos=(DataTable)ViewState["REPUESTOS"];
			UInt32 numfactura,kilom,numOrdenA;
			bool pagaFabrica=false;
			string errores="";
			#region Validaciones
			//Distribuidor
			if(concesionario=="P")nitDistribuidor=txtNitDistribuidor.Text;
			errores=SeleccionarOperaciones(dgrKitsMantenimientoOperaciones,dtOperaciones)+SeleccionarRepuestos(dgrKitsMantenimientoRepuestos,dtRepuestos);
			if(errores.Length>0){
                Utils.MostrarAlerta(Response, "" + errores + "");
				return;
			}
			//Validaciones
			if(nitPropietario.Length==0){
                Utils.MostrarAlerta(Response, "Debe ingresar el propietario.");
				return;}
			if(nitDistribuidor.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar el distribuidor.");
				return;}
			if(proceso!="G" && ddlMantenimiento.SelectedValue.Length==0){
                Utils.MostrarAlerta(Response, "Debe seleccionar el mantenimiento programado.");
				return;}
			try{
				Convert.ToDateTime(txtFechaProceso.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "Debe ingresar la fecha del proceso en el formato correcto.");
				return;}
			try
			{
				Convert.ToDateTime(txtFechaFact.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "Debe ingresar la fecha de facturación en el formato correcto.");
				return;}
			try
			{
				numfactura=Convert.ToUInt32(txtNumFactura.Text);}
			catch
			{
                Utils.MostrarAlerta(Response, "Número de factura no válido.");
				return;}
			if(proceso=="A"){
				prefOrdenA=txtPrefFactura.Text;
				numOrdenA=numfactura;}
			else{
				try
				{
					prefOrdenA=txtPrefOrdenProc.Text;
					numOrdenA=Convert.ToUInt32(txtNumOrdenProc.Text);}
				catch
				{
                    Utils.MostrarAlerta(Response, "Número de factura de concesionario no válido.");
					return;}
			}
			try
			{
				kilom=Convert.ToUInt32(txtKilometraje.Text.Replace(",",""));
                if (kilom < 1 && proceso != "A")
                {
                    Utils.MostrarAlerta(Response, "El Kilometraje es Obligatorio.");
                    return;
                }
            }
			catch
			{
                Utils.MostrarAlerta(Response, "Kilometraje no válido.");
				return;}
			if(txtPlaca.Text!=txtPlaca.Text.ToUpper())
			{
                Utils.MostrarAlerta(Response, "Debe ingresar la placa en letras mayúsculas.");
				return;}
			
			if(proceso=="R")
				pagaFabrica=(DBFunctions.SingleData("SELECT PMAN_PAGAFABRICA FROM PMANTENIMIENTOPROGRAMADO WHERE PGRU_CODIGO='"+ViewState["GRUPO_CATALOGO"]+"' AND PKIT_CODIGO='"+ddlMantenimiento.SelectedValue+"';")=="S")?true:false;
			else if(proceso=="A")
				pagaFabrica=(DBFunctions.SingleData("SELECT CS.CTAL_PAGAALIST FROM CTALLER CS;")=="S")?true:false;
			else pagaFabrica=true;
			
			ViewState["OPERACIONES"]=dtOperaciones;
			dgrKitsMantenimientoOperaciones.DataSource=dtOperaciones;
			dgrKitsMantenimientoOperaciones.DataBind();
			ViewState["REPUESTOS"]=dtRepuestos;
			dgrKitsMantenimientoRepuestos.DataSource=dtRepuestos;
			dgrKitsMantenimientoRepuestos.DataBind();

			if(dtOperaciones.Rows.Count==0||dtOperaciones.Select("USAR=1").Length==0){
                Utils.MostrarAlerta(Response, "No seleccionó ningúna operación de la lista de chequeo.");
				return;}
			#endregion Validaciones
			
			ProcesosPostventa procP=ProcesosPostventa.Ninguno;
			if (proceso=="A") procP=ProcesosPostventa.Alistamiento;
			else if (proceso=="R") procP=ProcesosPostventa.Revision;
			else if (proceso=="G") procP=ProcesosPostventa.Garantia;

			ProcesoPostventa als1=new ProcesoPostventa(procP,ddlMantenimiento.SelectedValue,txtVINVehiculo.Text,txtPlaca.Text.Trim(),dtOperaciones,dtRepuestos,txtPrefFactura.Text,numfactura,prefOrdenA,numOrdenA,txtFechaFact.Text,txtFechaProceso.Text,nitDistribuidor,nitPropietario,txtNITPropietarioa.Text,txtNITPropietarioc.Text,txtNITPropietariob.Text,txtNITPropietariod.Text,txtNITPropietarioe.SelectedValue,txtObservacionesCliente.Text,txtObservacionesTaller.Text,kilom,pagaFabrica);
			string resultado=als1.Ejecutar();
			if(resultado.Length==0)
				Response.Redirect(indexPage + "?process=Vehiculos.ProcesosPostventa&path="+Request.QueryString["path"]+"&prc="+proceso+"&pO="+als1.prefijoOrden+"&nO="+als1.numeroOrden+"&cnc="+concesionario);
			else{
                Utils.MostrarAlerta(Response, "Revise los errores reportados en la parte inferior.");
				lblError.Text="Errores: <br>"+resultado;}
		}
		//Seleccionar operaciones
		private string SeleccionarOperaciones(DataGrid dgrO,DataTable dtOperaciones){
			int n=0;
			string errores="",incidente,causal;
			foreach(DataGridItem dgi in dgrO.Items)
			{
				if(dgi.ItemType==ListItemType.Item||dgi.ItemType==ListItemType.AlternatingItem)
				{
					if(!((CheckBox)dgi.FindControl("chkUsar")).Checked)dtOperaciones.Rows[n]["USAR"]=0;
					else dtOperaciones.Rows[n]["USAR"]=1;
					if(Request.QueryString["prc"]=="G")
					{
						incidente=((DropDownList)dgi.FindControl("ddlIncidente")).SelectedValue;
						causal=((DropDownList)dgi.FindControl("ddlCausal")).SelectedValue;
						dtOperaciones.Rows[n]["INCIDENTE"]=(incidente.Length>0)?incidente:"";
						dtOperaciones.Rows[n]["CAUSAL"]=(causal.Length>0)?causal:"";
					}
					n++;
				}
			}
			return(errores);
		}
		//Seleccionar Repuestos
		private string SeleccionarRepuestos(DataGrid dgrR, DataTable dtRepuestos)
		{
			int n=0;
			string errores="";
			foreach(DataGridItem dgi in dgrR.Items)
			{
				if(dgi.ItemType==ListItemType.Item||dgi.ItemType==ListItemType.AlternatingItem)
				{
					if(!((CheckBox)dgi.FindControl("chkUsarR")).Checked)dtRepuestos.Rows[n]["USAR"]=0;
					else
					{
						string strCantidad=((TextBox)dgi.FindControl("txtCantidad")).Text.Trim();
						string strPrecio=((TextBox)dgi.FindControl("txtValor")).Text.Trim();
						dtRepuestos.Rows[n]["USAR"]=1;
						try
						{
							int cantidad=Convert.ToInt16(strCantidad);
							if(cantidad<1)throw(new Exception());
							dtRepuestos.Rows[n]["CANTIDAD"]=cantidad;
						}
						catch
						{
							errores+="Cantidad de items no válida en la fila "+(n+1)+". ";
						}
						try
						{
							double precio=Convert.ToDouble(strPrecio);
							if(precio<0)throw(new Exception());
							dtRepuestos.Rows[n]["PRECIO"]=precio;
						}
						catch
						{
							errores+="Valor unitario no válido en la fila "+(n+1)+". ";
						}
					}
					n++;
				}
			}
			return(errores);
		}
		protected void dgrAutorizaciones_Bound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem){
				if(((DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString()=="S")
					e.Item.BackColor=Color.LightGreen;
				else
					e.Item.BackColor=Color.Tomato;
			}
		}
		protected void dgrOpers_Bound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem)
			{
				string incidente=((DataRowView)e.Item.DataItem).Row.ItemArray[5].ToString();
				string causal=((DataRowView)e.Item.DataItem).Row.ItemArray[6].ToString();
				DropDownList ddlI=((DropDownList)e.Item.FindControl("ddlIncidente"));
				DropDownList ddlC=((DropDownList)e.Item.FindControl("ddlCausal"));

				ddlI.DataSource=(DataTable)ViewState["INCIDENTES"];
				ddlI.DataValueField="CODIGO";
				ddlI.DataTextField="DESCRIPCION";
				ddlI.DataBind();

				ddlC.DataSource=(DataTable)ViewState["CAUSALES"];
				ddlC.DataValueField="CODIGO";
				ddlC.DataTextField="DESCRIPCION";
				ddlC.DataBind();

				if(incidente.Length>0) ddlI.SelectedIndex=ddlI.Items.IndexOf(ddlI.Items.FindByValue(incidente));
				if(causal.Length>0) ddlC.SelectedIndex=ddlC.Items.IndexOf(ddlC.Items.FindByValue(causal));

			}
		}
	}
}
