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
	///		Descripción breve de AMS_Comercial_VentaTiquetesPrepago.
	/// </summary>
	public class AMS_Comercial_VentaTiquetesPrepago : System.Web.UI.UserControl
	{
		#region Controles
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.DropDownList ddlPlanilla;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblRutaPrincipal;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblOrigen;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlDestino;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtTiquete;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.DropDownList txtNITc;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.TextBox txtNIT;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.TextBox txtNITa;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.TextBox txtNITb;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.TextBox txtPrecio;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Panel pnlBus;
		protected System.Web.UI.WebControls.Panel pnlRutas;
		protected System.Web.UI.WebControls.Panel pnlPlanilla;
		protected System.Web.UI.WebControls.Label lblError;
		public string focus="";
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion Controles

		private void Page_Load(object sender, System.EventArgs e)
		{
			/*Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_VentaTiquetesPrepago));	
			if (!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				btnGuardar.Attributes.Add("onclick","return(Comprar());");
				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				ddlDestino.Attributes.Add("onKeyDown", "KeyDownHandler();");
				txtNIT.Attributes.Add("onKeyDown", "KeyDownHandlerNIT();if(event.keyCode==13)return(false);");
				bind.PutDatasIntoDropDownList(txtNITc,"select TNIT_TIPONIT, TNIT_NOMBRE FROM DBXSCHEMA.TNIT;");
				focus="document.getElementById('"+ddlPlanilla.ClientID+"').focus();";
				//Descuento prepago
				double porcentajePrepago;
				try
				{
					porcentajePrepago=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_PORCENTAJE FROM DBXSCHEMA.TPORCENTAJESTRANSPORTES WHERE CLAVE='PREPAGO';"));
					ViewState["PorcentajePrepago"]=porcentajePrepago.ToString("##0.##");}
				catch
				{
					ViewState["PorcentajePrepago"]="0";}
			}*/
		}

/*
		//Cargar puestos
		[Ajax.AjaxMethod]
		public string CargarPuestos(string planilla, string ruta, string tipo, string agencia)
		{
			DataSet dsPuestos= new DataSet();
			//ruta principal
			string mruta=DBFunctions.SingleData("SELECT mp.mrut_codigo FROM dbxschema.MPLANILLAVIAJE mp WHERE mp.mpla_codigo="+planilla);
			//Traer puestos para el numero de viaje asociado a la planilla y para la ruta asociada[0]
			//Traer elementos del bus con nombre de imagenes [1]
			string sqlRuta="select mc.fila fila, mc.columna columna, mc.mele_numepues numpuesto,mc.tele_codigo codelemento,mc.test_codigo estado "+
				"FROM DBXSCHEMA.mconfiguracionpuesto mc, dbxschema.mplanillaviaje mp "+
				"where mp.mpla_codigo="+planilla+" and mp.mviaje_numero=mc.mviaje_numero and mc.MRUT_CODIGO=mp.MRUT_CODIGO;"+
				"select tele_codigo,tele_imgcod from DBXSCHEMA.TELEMENTOBUS;";
			DBFunctions.Request(dsPuestos,IncludeSchema.NO,sqlRuta);
			//Generar JS
			//ver configuracion
			string configuracion=DBFunctions.SingleData("SELECT mv.mcon_cod from dbxschema.mviaje mv, dbxschema.mplanillaviaje mp "+
				"where mp.mpla_codigo="+planilla+" and mp.mviaje_numero=mv.mviaje_numero and mv.mrut_codigo=mp.MRUT_CODIGO;");
			if(configuracion.Length==0)return("No hay viajes planillados para la ruta y numero de planilla especificada.");
			int filas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_filas from dbxschema.mconfiguracionbus where mcon_cod="+configuracion));
			int columnas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_columnas from dbxschema.mconfiguracionbus where mcon_cod="+configuracion));
			string matrizPuestos = filas.ToString()+","+columnas;
			string precioSug=((int)double.Parse(DBFunctions.SingleData("SELECT mrut_valsug from dbxschema.mrutas where mrut_codigo='"+ruta+"';").ToString())).ToString();
			//elementos
			string idImg="";
			for(int i=0;i<filas;i++)
			{
				for(int j=0;j<columnas;j++)
				{
					DataRow[]puesto=dsPuestos.Tables[0].Select("fila="+i+" and columna="+j);
					if(puesto.Length==0)
						matrizPuestos += ",0";
					else
					{
						DataRow[]imagen=dsPuestos.Tables[1].Select("tele_codigo='"+puesto[0]["codelemento"].ToString()+"'");
						if(imagen.Length==0)
							matrizPuestos += ",0";
						else
						{
							if(puesto[0][3].ToString().Equals("SP"))
							{
								if(puesto[0][4].ToString().Equals("DN"))idImg="1";
								else
									if(puesto[0][4].ToString().Equals("OC"))idImg="3";
								else
									if(puesto[0][4].ToString().Equals("RV"))idImg="2";
							}
							else
							{
								if(puesto[0][3].ToString().Equals("BN"))idImg="4";
								else if(puesto[0][3].ToString().Equals("PS"))idImg="0";
								else if(puesto[0][3].ToString().Equals("SC"))idImg="6";
							}
							matrizPuestos+=","+idImg;
						}
					}
				}
			}
			//Viaje
			DataSet dsViaje=new DataSet();
			string sqlViaje="select "+
				"mv.FECHA_SALIDA FECHA, mv.HORA_SALIDA HORA, "+
				"mn.mnit_nombres concat ' ' concat mn.mnit_apellidos concat ' (' concat mv.mnit_conductor concat ')' as CONDUCTOR, "+
				"mv.MCAT_PLACA as PLACA, mb.mbus_numero as NUMERO "+
				"from dbxschema.mviaje mv "+
				"left join dbxschema.mnit mn on mn.mnit_nit=mv.mnit_conductor "+
				"left join dbxschema.mplanillaviaje mp on mp.mrut_codigo=mv.mrut_codigo and mp.mviaje_numero=mv.mviaje_numero "+
				"left join dbxschema.mbusafiliado mb on mb.MCAT_PLACA=mv.MCAT_PLACA "+
				"where mp.mpla_codigo="+planilla+";";
			DBFunctions.Request(dsViaje,IncludeSchema.NO,sqlViaje);
			string fechaV="",horaV="",nitV="",placaV="",noBus="";
			if(dsViaje.Tables[0]!=null && dsPuestos.Tables[0].Rows.Count>0)
			{
				int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA"]);
				fechaV=Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA"]).ToString("yyyy-MM-dd");
				horaV=Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00");
				nitV=dsViaje.Tables[0].Rows[0]["CONDUCTOR"].ToString();
				placaV=dsViaje.Tables[0].Rows[0]["PLACA"].ToString();
				noBus=dsViaje.Tables[0].Rows[0]["NUMERO"].ToString();
			}
			string tiqueteV="";
			return(matrizPuestos+"@"+mruta+"@"+fechaV+"@"+horaV+"@"+nitV+"@"+placaV+"@"+noBus+"@"+precioSug+"@"+tiqueteV);
		}
		

		//Comprar
		[Ajax.AjaxMethod]
		public string Comprar(string tiquete, string agencia, string planilla,string tipo,string ruta,string nit, string tnit, string nombre, string apellido, int precio, string parametrosBus, double descuento)
		{
			if(agencia.Length>0)agencia=agencia;
			string []pPuestos=parametrosBus.Split('|');
			string []pPosicion;
			string numDocumento=tiquete.Trim(),numLineaS="",nitResponsable="";
			string rutaPrincipal,numeroViaje;
			int numLinea,fila=0,columna=0,numeroPuestos=0;
			double porcentajePrepago=descuento;

			if(pPuestos.Length<1)return("0@No se seleccionaron puestos");

			//Responsable
			nitResponsable=DBFunctions.SingleData("SELECT UT.MNIT_NIT FROM DBXSCHEMA.SUSUARIO_TRANSPORTE UT, DBXSCHEMA.SUSUARIO SU "+
				"WHERE SU.susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"' AND "+
				"SU.SUSU_CODIGO=UT.SUSU_CODIGO;");
			if(nitResponsable.Length==0)
				return("0@El usuario responsable ("+HttpContext.Current.User.Identity.Name.ToString().ToLower()+") no tiene un NIT asociado");
			
			//Responsable Agencia
			string nitResponsableAgencia=DBFunctions.SingleData("SELECT mnit_encargado from dbxschema.magencia where mag_codigo="+agencia+";");

			//Verificar planilla no liquidada
			if(!DBFunctions.RecordExist("select mpla_codigo from dbxschema.mplanillaviaje where mag_codigo="+agencia+" AND fecha_liquidacion is null and mpla_codigo="+planilla+";"))
				return("0@Planilla no válida");
			
			//ruta inversa
			string rutaInv="(MRUT_CODIGO='"+ruta+"' OR "+
				"MRUT_CODIGO IN ("+
				" SELECT MRI.MRUT_CODIGO FROM "+
				" DBXSCHEMA.MRUTAS MRI, DBXSCHEMA.MRUTAS MRP "+
				" WHERE MRI.PCIU_COD=MRP.PCIU_CODDES AND MRP.PCIU_COD=MRI.PCIU_CODDES AND "+
				" MRP.MRUT_CODIGO='"+ruta+"')"+
				")";
			//Verificar papeleria
			if(!DBFunctions.RecordExist("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='TIQPRE' AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M' AND NUM_DOCUMENTO="+numDocumento+" AND "+rutaInv+" ORDER BY NUM_DOCUMENTO FETCH FIRST 1 ROWS ONLY"))
				return("0@No hay papeleria disponible. ");

			//Numero de viaje debe estar activo o en venta
			numeroViaje=DBFunctions.SingleData("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			if(!DBFunctions.RecordExist("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MVIAJE WHERE MVIAJE_NUMERO="+numeroViaje+" AND (ESTADO_VIAJE='A' OR ESTADO_VIAJE='V');"))
				return("0@El viaje no se encuentra activo o en venta");

			//Precio
			if(precio<=0)
				return("0@Precio no válido");
			if(!DBFunctions.RecordExist("SELECT MRUT_VALSUG FROM DBXSCHEMA.MRUTAS WHERE MRUT_VALMAX>="+precio+" AND MRUT_VALMIN<="+precio+" AND MRUT_CODIGO='"+ruta+"';"))
				return("0@El precio está fuera del rango válido");
			
			//Ruta principal
			rutaPrincipal=DBFunctions.SingleData("SELECT MRUT_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			
			//Revisar puestos libres
			DataRow []drOcupado;
			DataSet dsPuestos=new DataSet();
			DBFunctions.Request(dsPuestos, IncludeSchema.NO, "SELECT MELE_NUMEPUES, FILA, COLUMNA FROM DBXSCHEMA.MCONFIGURACIONPUESTO WHERE MRUT_CODIGO='"+rutaPrincipal+"' AND MVIAJE_NUMERO="+numeroViaje+" AND TEST_CODIGO<>'DN';");
			for(int c=0;c<pPuestos.Length;c++)
				if(pPuestos[c].Length>0)
				{
					pPosicion=pPuestos[c].Split('-');
					if(pPosicion.Length==2)
					{
						fila=int.Parse(pPosicion[0]);
						columna=int.Parse(pPosicion[1]);
						drOcupado=dsPuestos.Tables[0].Select("FILA="+fila+" AND COLUMNA="+columna);
						if(drOcupado.Length>0)
							return("0@El puesto "+drOcupado[0][0].ToString()+" ya fue asignado");
						numeroPuestos++;
					}
				}
			
			//solo puede ser 1
			if(numeroPuestos!=1)return("0@Solo se puede registrar 1 tiquete prepago a la vez.");

			ArrayList sqlStrings = new ArrayList();
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			//Crear nit si no existe
			nit=nit.Trim();
			if(nit.Length>0)
			{
				if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='"+nit+"';"))
					sqlStrings.Add("INSERT INTO DBXSCHEMA.MNIT VALUES('"+nit+"',NULL,'"+nombre+"',NULL,'"+apellido+"',NULL,'"+tnit+"','"+ciudad+"','N','ND','"+ciudad+"','ND','ND','ND','ND','V','N','N','T');");
				nit=nit;
			}
			else nit=DBFunctions.SingleData("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';");

			//No linea planilla
			numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			if(numLineaS.Length==0)numLinea=1;
			else numLinea=int.Parse(numLineaS)+1;

			//Crear tiquete
			double porcentajePrepagoA=0;
			try{porcentajePrepagoA=Convert.ToDouble(DBFunctions.SingleData("SELECT PORCENTAJE_DESCUENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='TIQPRE' AND NUM_DOCUMENTO="+numDocumento+";"));}
			catch{porcentajePrepagoA=porcentajePrepago;}
			string numPuesto=DBFunctions.SingleData("SELECT DC.MELE_NUMEPUES FROM DBXSCHEMA.DCONFIGURACIONBUS DC,DBXSCHEMA.MVIAJE MV WHERE MV.MRUT_CODIGO='"+rutaPrincipal+"' AND MV.MVIAJE_NUMERO="+numeroViaje+" AND MV.MCON_COD=DC.MCON_COD AND DC.MPUE_POSFILA="+fila+" AND DC.MPUE_POSCOLUMNA"+columna+";");
			sqlStrings.Add("INSERT INTO dbxschema.mtiquete_viaje_prepago values('TIQPRE',"+numDocumento+","+agencia+","+planilla+","+numLinea+",'"+ruta+"',"+fila+","+columna+",'"+numPuesto+"','"+nitResponsableAgencia+"','"+nitResponsable+"','"+nit+"',0,"+precio+","+porcentajePrepagoA+","+((precio*(100-porcentajePrepagoA))/100)+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','V','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
			
			//Cambiar configuracion puesto
			for(int c=0;c<pPuestos.Length;c++)
				if(pPuestos[c].Length>0)
				{
					pPosicion=pPuestos[c].Split('-');
					if(pPosicion.Length==2)
					{
						fila=int.Parse(pPosicion[0]);
						columna=int.Parse(pPosicion[1]);
						sqlStrings.Add("UPDATE DBXSCHEMA.MCONFIGURACIONPUESTO SET TDOC_CODIGO='TIQPRE', NUM_DOCUMENTO="+numDocumento+", TEST_CODIGO='OC' WHERE MRUT_CODIGO='"+rutaPrincipal+"' AND MVIAJE_NUMERO="+numeroViaje+" AND FILA="+fila+" AND COLUMNA="+columna+";");
					}
				}
			//Actualizar no lineas planilla
			sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
			
			//Modificar papeleria(usada)
			sqlStrings.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd")+"',MPLA_CODIGO="+planilla+" WHERE TDOC_CODIGO='TIQPRE' AND NUM_DOCUMENTO="+numDocumento+";");
			
			//Cambiar estado viaje->en venta
			sqlStrings.Add("UPDATE DBXSCHEMA.MVIAJE SET ESTADO_VIAJE='V' WHERE MRUT_CODIGO='"+rutaPrincipal+"' AND MVIAJE_NUMERO="+numeroViaje+";");

			if(DBFunctions.Transaction(sqlStrings))
				return("1@Compra registrada@"+numDocumento+"@"+tipo);
			else
				return("0@Error: "+DBFunctions.exceptions);

		}

		//Traer NIT
		[Ajax.AjaxMethod]
		public string TraaerNIT(string NIT)
		{
			DataSet dsNIT=new DataSet();
			string nombre="",apellido="";
			DBFunctions.Request(dsNIT,IncludeSchema.NO,"select mnit_nombres, mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+NIT+"';");
			if(dsNIT.Tables[0].Rows.Count>0)
			{
				nombre=dsNIT.Tables[0].Rows[0][0].ToString();
				apellido=dsNIT.Tables[0].Rows[0][1].ToString();
			}
			return(nombre+"|"+apellido);
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlBus.Visible=true;
			//Cambia la ruta
			if(agencia.Length==0)
			{
				pnlPlanilla.Visible=false;
				pnlRutas.Visible=false;
				ddlPlanilla.Items.Clear();
				return;
			}
			//Agencia virtual?
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+ddlAgencia.SelectedValue+";");
			//Consultar planillas no liquidadas de la agencia
			ddlPlanilla.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlPlanilla,"select mp.mpla_codigo, char(mp.mpla_codigo) concat "+
				" ' [' concat mp.mrut_codigo concat '] ' concat char(mv.fecha_salida) concat ' ' concat "+
				"rtrim(char(floor(mv.hora_salida/60))) concat ':' "+
				"concat case when (mod(mv.hora_salida,60)<10) then '0' else '' end concat "+
				"rtrim(char(mod(mv.hora_salida,60))) "+
				"from dbxschema.mplanillaviaje mp, dbxschema.mviaje mv, DBXSCHEMA.MAGENCIA_VIAJE mav "+
				"where mp.mag_codigo="+agencia+" AND mp.fecha_liquidacion is null and mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo AND "+
				"mv.mviaje_numero=mav.mviaje_numero and mv.mrut_codigo=mav.mrut_codigo and mav.pciu_codigo='"+ciudad+"' and mav.despachado='N';");
			ListItem itm=new ListItem("---Seleccione---","");
			ddlPlanilla.Items.Insert(0,itm);
			pnlPlanilla.Visible=true;
			pnlRutas.Visible=false;
		}

		private void ddlPlanilla_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue,planilla=ddlPlanilla.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlBus.Visible=true;
			//Cambia la ruta
			if(planilla.Length==0)
			{
				pnlRutas.Visible=false;
				return;
			}
			//Ruta principal
			lblRutaPrincipal.Text=DBFunctions.SingleData("SELECT '[' concat mr.mrut_codigo concat '] ' concat po.pciu_nombre concat ' - ' concat pd.pciu_nombre "+
				"FROM dbxschema.MRUTAS mr, dbxschema.PCIUDAD po, dbxschema.PCIUDAD pd, dbxschema.MPLANILLAVIAJE mp "+
				"WHERE mp.mpla_codigo="+planilla+" and mp.mrut_codigo=mr.mrut_codigo and mr.pciu_cod=po.pciu_codigo and mr.pciu_coddes=pd.pciu_codigo");
			//Origen
			lblOrigen.Text=DBFunctions.SingleData("SELECT po.pciu_nombre "+
				"FROM dbxschema.MAGENCIA ma, dbxschema.PCIUDAD po "+
				"WHERE ma.mag_codigo="+agencia+" and ma.pciu_codigo=po.pciu_codigo");
			//Destinos trae las rutas con origen en la ciudad de la agencia, segun la ruta principal de la planilla y sus subrutas
			bind.PutDatasIntoDropDownList(ddlDestino,"select mr.mrut_codigo, pd.pciu_nombre "+
				"from dbxschema.mrutas mr, dbxschema.mplanillaviaje mp, dbxschema.magencia ma, dbxschema.pciudad pd "+
				"where "+
				"mr.pciu_cod=ma.pciu_codigo and "+
				"(mr.mrut_codigo=mp.MRUT_CODIGO or mr.mrut_codigo in (select mi.mruta_secundaria from  dbxschema.mruta_intermedia mi where mi.mruta_principal=mp.MRUT_CODIGO)) "+
				"and mp.mpla_codigo="+planilla+" and ma.MAG_CODIGO="+agencia+" and pd.pciu_codigo=mr.pciu_coddes "+
				"order by mr.mrut_codigo;");
			ListItem itm=new ListItem("---Seleccione---","");
			ddlDestino.Items.Insert(0,itm);
			focus="document.getElementById('"+ddlDestino.ClientID+"').focus();";
			pnlRutas.Visible=true;
		}*/
	}
}
