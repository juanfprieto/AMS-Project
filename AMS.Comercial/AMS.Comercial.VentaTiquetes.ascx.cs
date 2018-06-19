/// created on 22/02/2005 at 16:26 
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
using Ajax;
using AMS.DBManager;

namespace AMS.Comercial
{
	public class AMS_Comercial_VentaTiquetes : System.Web.UI.UserControl
	{
		#region Controles, variables
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.PlaceHolder pnlRutas;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.PlaceHolder pnlPlanilla;
		protected System.Web.UI.WebControls.DropDownList ddlPlanilla;
		protected System.Web.UI.WebControls.Label lblRutaPrincipal;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblOrigen;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.DropDownList ddlDestino;
		public string focus="";
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
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.DropDownList txtNITc;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.TextBox txtTiquete;
		protected System.Web.UI.WebControls.Button btnPreDespacho;
		protected System.Web.UI.WebControls.PlaceHolder pnlBus;
		protected System.Web.UI.WebControls.Label Label16;
		protected System.Web.UI.WebControls.Label lblPlacaOriginal;
		protected System.Web.UI.WebControls.Button btnDespachar;
		protected System.Web.UI.WebControls.Label Label17;
		protected System.Web.UI.WebControls.Label Label18;
		protected System.Web.UI.WebControls.CheckBox chkPrepago;
		protected System.Web.UI.WebControls.PlaceHolder pnlVenta;
		protected System.Web.UI.WebControls.PlaceHolder pnlViaje;
		protected System.Web.UI.WebControls.PlaceHolder pnlCiudadOrigen;
		protected System.Web.UI.WebControls.PlaceHolder pnlCiudadDestino;
		protected System.Web.UI.WebControls.Label lblTipoAgencia;
		protected System.Web.UI.WebControls.Label Label19;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label15;
		protected System.Web.UI.WebControls.DropDownList ddlAgencia;
		protected System.Web.UI.WebControls.Label Label20;
		protected System.Web.UI.WebControls.DropDownList ddlFechaPlanilla;
		protected System.Web.UI.WebControls.Button btnPlanillar;
		protected System.Web.UI.WebControls.Button btnConduce;
		protected System.Web.UI.WebControls.Button btnBoleta;
		protected System.Web.UI.WebControls.PlaceHolder PnlConduce;
		protected System.Web.UI.WebControls.Label Label21;
		protected System.Web.UI.WebControls.Button btnVerDespacho;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Label Label22;
		protected System.Web.UI.WebControls.Label lblPlanila;
		protected System.Web.UI.WebControls.DataGrid dgrAsignacion;
		protected System.Web.UI.WebControls.CheckBoxList chkAsignaciones;
		protected System.Web.UI.WebControls.Button btnAsociar;
		string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label23;
		protected System.Web.UI.WebControls.Label lblNroViaje;
		protected System.Web.UI.WebControls.Label Label24;
		protected System.Web.UI.WebControls.Label lblNroBus;
		protected System.Web.UI.WebControls.PlaceHolder pnlDespacho;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.TextBox txtObservacion;
		protected System.Web.UI.WebControls.TextBox txtNITd;
		DataSet dsAsignacion=new DataSet();
		string placa, numeroViaje, placaActual, nitResponsable;
        string numDocumento;
		protected System.Web.UI.WebControls.Label lblAnticipoViaje;
		protected System.Web.UI.WebControls.TextBox txtAnticipoViaje;
		protected System.Web.UI.WebControls.Label Label26;
		#endregion
		
		public void Page_Load(object sender, System.EventArgs e)
		{
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Comercial_VentaTiquetes));

			if (!IsPostBack)
			{
				Agencias.TraerAgenciasUsuario(ddlAgencia);
				DatasToControls bind=new DatasToControls();
				nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
                ViewState["nitResponsable"] = nitResponsable;
				btnGuardar.Attributes.Add("onclick","return(Comprar());");
				ddlDestino.Attributes.Add("onKeyDown", "KeyDownHandler();");
                ddlDestino.Attributes.Add("onChange", "CambiaRuta();");
                chkPrepago.Attributes.Add("onClick", "Prepago(this.checked);");
				txtNIT.Attributes.Add("onKeyDown", "KeyDownHandlerNIT();if(event.keyCode==13)return(false);");
				txtPrecio.Attributes.Add("onKeyDown", "if(event.keyCode==13){document.getElementById('"+btnGuardar.ClientID+"').click();return(false);};");

				if(ddlAgencia.Items.Count>0)ddlAgencia_SelectedIndexChanged(sender,e);
				
				bind.PutDatasIntoDropDownList(txtNITc,"select TNIT_TIPONIT, TNIT_NOMBRE FROM DBXSCHEMA.TNIT;");
				
				txtNITc.SelectedIndex= txtNITc.Items.IndexOf(txtNITc.Items.FindByValue("C"));
				focus="document.getElementById('"+ddlFechaPlanilla.ClientID+"').focus();";

				double porcentajePrepago=Tiquetes.PorcentajePrepago();
				ViewState["PorcentajePrepago"]=(porcentajePrepago>0)?porcentajePrepago.ToString("##0.##"):"0";
				int concepto_conduce = Convert.ToInt16(DBFunctions.SingleData("(SELECT  CONCEPTO_CONDUCE from DBXSCHEMA.CTRANSPORTES FETCH FIRST 1 ROWS ONLY);+"));
				ViewState["concepto_conduce"]= concepto_conduce;
				if(Request.QueryString["act"]!=null)
				{
					//return(CambiaRuta());
					if(Request.QueryString["act"]=="1")  //regresar de Nuevo Viaje
					{
						Response.Write("<script language='javascript'>alert('El viaje "+Request.QueryString["num"]+" ha sido creado con el número de planilla "+Request.QueryString["pla"]+".');</script>");
						//Response.Write("<script language='javascript'>alert('El viaje "+Request.QueryString["num"]+" ha sido creado con el número de planilla "+Request.QueryString["pla"]+".');"+
					    //			"window.open('../aspx/AMS.Comercial.Viaje.aspx?pla="+Request.QueryString["pla"]+"', 'VIAJE', 'width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");
					}
					else
					if(Request.QueryString["act"]=="2")  //regresar de de ver la planilla
						Response.Write("<script language='javascript'>alert('Volver a seleccionar Planilla "+Request.QueryString["pla"]+" de Viaje "+Request.QueryString["num"]+"');</script>");
					else
						if(Request.QueryString["act"]=="3") //regresar del Despacho
							Response.Write("<script language='javascript'>alert('El bus placa  ha sido despachado Planilla "+Request.QueryString["pla"]+" de Viaje "+Request.QueryString["num"]+".');</script>");
				}
			}
		}
		
		
		#region AJAX
		//Cargar puestos
		[Ajax.AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
		public string CargarPuestos(string planilla, string ruta, string tipo, string agencia, string parametrosBus)
		{
			DataSet dsInfo = new DataSet();
			DataSet dsPuestos = new DataSet();
			DataSet dsHermanos = new DataSet();
			ArrayList arrPuestos =new ArrayList(parametrosBus.Split('|'));
			
			//Informacion ruta principal, viaje padre
			DBFunctions.Request(dsInfo,IncludeSchema.NO,
				"SELECT mp.mrut_codigo, mv.mviaje_padre, mv.fecha_salida "+
				"FROM MPLANILLAVIAJE mp, MVIAJE mv "+
				"where mv.MVIAJE_NUMERO=mp.MVIAJE_NUMERO and mv.mrut_codigo=mp.mrut_codigo and mp.mpla_codigo="+planilla);
			string mruta=dsInfo.Tables[0].Rows[0]["mrut_codigo"].ToString();
			string noViajePadre=dsInfo.Tables[0].Rows[0]["mviaje_padre"].ToString();
			DateTime fchSalida=Convert.ToDateTime(dsInfo.Tables[0].Rows[0]["fecha_salida"]);

			//Secuencia de origen y destino en ruta principal
			int secOrig=-1,secDes=-1;
			if(noViajePadre.Length>0){
				try
				{
					secOrig=Convert.ToInt16(DBFunctions.SingleData(
						"SELECT SECUENCIA FROM MRUTA_CIUDAD MR, MAGENCIA MA "+
						"WHERE MA.MAG_CODIGO="+agencia+" and MA.PCIU_CODIGO=MR.PCIU_CODIGO and MR.MRUT_CODIGO='"+mruta+"';"));
					secDes=Convert.ToInt16(DBFunctions.SingleData(
						"SELECT SECUENCIA FROM MRUTA_CIUDAD MRC, MRUTAS MR "+
						"WHERE MRC.MRUT_CODIGO='"+mruta+"' and MR.MRUT_CODIGO='"+ruta+"' and "+
						"MR.PCIU_CODDES=MRC.PCIU_CODIGO;"));}
				catch
				{
					return("No ha configurado correctamente el recorrido de la ruta para el destino seleccionado.");
				}
			}

			//Traer puestos para el numero de viaje asociado a la planilla y para la ruta asociada[0]
			//Traer elementos del bus con nombre de imagenes [1]
			string sqlRuta="select mc.fila fila, mc.columna columna, mc.mele_numepues numpuesto,mc.tele_codigo codelemento,mc.test_codigo estado "+
				"FROM DBXSCHEMA.mconfiguracionpuesto mc, dbxschema.mplanillaviaje mp "+
				"where mp.mpla_codigo="+planilla+" and mp.mviaje_numero=mc.mviaje_numero and mc.MRUT_CODIGO=mp.MRUT_CODIGO;"+
				"select tele_codigo,tele_imgcod from DBXSCHEMA.TELEMENTOBUS;";
			DBFunctions.Request(dsPuestos,IncludeSchema.NO,sqlRuta);

			//Puestos ocupados de rutas hermanas
			//fchSalida,noViajePadre,mruta,ruta, agencia
			if(noViajePadre.Length>0){
				string sqlTomados="SELECT "+
					"MCP.FILA,MCP.COLUMNA,MCP.MELE_NUMEPUES,MCP.TELE_CODIGO, MCP.TEST_CODIGO, MCP.TDOC_CODIGO, MCP.NUM_DOCUMENTO, "+
					"mrd.pciu_cod, mco.secuencia secorig, mrd.pciu_coddes, mcd.secuencia secdes "+
					"FROM MCONFIGURACIONPUESTO MCP, MTIQUETE_VIAJE MT, MRUTAS MR, MRUTA_CIUDAD MCO, MRUTA_CIUDAD MCD, mviaje MV, MRUTAS MRD "+
					"WHERE "+
					"(days('"+fchSalida.ToString("yyyy-MM-dd")+"')-days(MV.FECHA_SALIDA))<30 and "+
					"(MV.MVIAJE_PADRE="+noViajePadre+" OR MV.MVIAJE_NUMERO="+noViajePadre+") and MV.MRUT_CODIGO='"+mruta+"' AND "+
					"MCP.MRUT_CODIGO=MV.MRUT_CODIGO and MCP.MVIAJE_NUMERO=MV.MVIAJE_NUMERO and MR.MRUT_CODIGO=MV.MRUT_CODIGO and "+
					"MCP.TELE_CODIGO='SP' AND MCP.TEST_CODIGO='OC' AND MT.TDOC_CODIGO=MCP.TDOC_CODIGO and MT.NUM_DOCUMENTO=MCP.NUM_DOCUMENTO AND "+
					"MRD.MRUT_CODIGO=MT.MRUT_CODIGO AND mrd.pciu_cod=mco.pciu_codigo and mrd.pciu_coddes=mcd.pciu_codigo and mco.mrut_codigo='"+mruta+" ' and "+
					"mcd.mrut_codigo='"+mruta+"' and mco.secuencia < "+secDes+"  and  mcd.secuencia > "+secOrig+" @PUESTO@ "+
					"order by mco.secuencia;";
				System.Web.HttpContext.Current.Session["SQL_PUESTOS_"+planilla]=sqlTomados;
				DBFunctions.Request(dsHermanos,IncludeSchema.NO,sqlTomados.Replace("@PUESTO@",""));
			}
			else
				System.Web.HttpContext.Current.Session["SQL_PUESTOS_"+planilla]="";
			//Generar JS
			//ver configuracion
			string configuracion=DBFunctions.SingleData("SELECT mv.mcon_cod from dbxschema.mviaje mv, dbxschema.mplanillaviaje mp "+
															"where mp.mpla_codigo="+planilla+" and mp.mviaje_numero=mv.mviaje_numero and mv.mrut_codigo=mp.MRUT_CODIGO;");
			if(configuracion.Length==0)return("No hay viajes planillados para la ruta y número de planilla especificada.");
			int filas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_filas from dbxschema.mconfiguracionbus where mcon_cod="+configuracion));
			int columnas = System.Convert.ToInt32(DBFunctions.SingleData("SELECT mbus_columnas from dbxschema.mconfiguracionbus where mcon_cod="+configuracion));
			string matrizPuestos = filas.ToString()+","+columnas;
			string matrizNumPuestos = filas.ToString()+","+columnas;
			
			string precioSug=((int)double.Parse(DBFunctions.SingleData("SELECT mrut_valsug from dbxschema.mrutas where mrut_codigo='"+ruta+"';").ToString())).ToString();

			//elementos
			int puestosLibres=0,sO;
			string idImg="";
			DataRow[]ocupado=new DataRow[0];
			for(int i=0;i<filas;i++){
				for(int j=0;j<columnas;j++){
					DataRow[]puesto=dsPuestos.Tables[0].Select("fila="+i+" and columna="+j);
					if(puesto.Length==0)
					{
						matrizPuestos += ",0";
						matrizNumPuestos += ",0";
					}
					else
					{
						DataRow[]imagen=dsPuestos.Tables[1].Select("tele_codigo='"+puesto[0]["codelemento"].ToString()+"'");
						if(imagen.Length==0)
						{
							matrizPuestos += ",0";
							matrizNumPuestos += ",0";
						}
						else
						{
							//Puesto
							if(puesto[0]["codelemento"].ToString().Equals("SP"))
							{
								//Viajes Hermanos
								if(noViajePadre.Length>0)
									ocupado=dsHermanos.Tables[0].Select("fila="+i+" and columna="+j);

								//Disponibles
								if(puesto[0]["estado"].ToString().Equals("DN")){
									//Puesto vendidos en rutas hermanas?
									if(ocupado.Length>0)
									{
										sO=Convert.ToInt16(ocupado[0]["secOrig"]);
										idImg="3"+sO;
										//sO=Convert.ToInt16(ocupado[0]["secOrig"]);
										//sD=Convert.ToInt16(ocupado[0]["secDes"]);

									}
									else{
										if(arrPuestos.Contains(i+"-"+j))
											idImg="5";
										else
											idImg="1";
										puestosLibres++;
									}
								}
								else
									if(puesto[0]["estado"].ToString().Equals("OC"))idImg="3";
									else
										if(puesto[0]["estado"].ToString().Equals("RV"))idImg="2";
								matrizNumPuestos += ","+puesto[0][2].ToString();
							}
							else
							{
								if(puesto[0]["codelemento"].ToString().Equals("BN"))idImg="4";
								else if(puesto[0]["codelemento"].ToString().Equals("PS"))idImg="0";
								else if(puesto[0]["codelemento"].ToString().Equals("SC"))idImg="6";
								matrizNumPuestos += ",0";
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
				"rtrim(mn.mnit_nombres concat ' ' concat mn.mnit_apellidos concat ' (' concat mv.mnit_conductor concat ')') as CONDUCTOR, "+
				"mv.MCAT_PLACA as PLACA, mb.mbus_numero as NUMERO, "+
				"mnr.mnit_nombres concat ' ' concat mnr.mnit_apellidos concat ' (' concat mv.mnit_relevador1 concat ')' as RELEVADOR "+
				"from dbxschema.mviaje mv "+
				"left join dbxschema.mnit mn on mn.mnit_nit=mv.mnit_conductor "+
				"left join dbxschema.mnit mnr on mnr.mnit_nit= mv.mnit_relevador1  "+
				"left join dbxschema.mplanillaviaje mp on mp.mrut_codigo=mv.mrut_codigo and mp.mviaje_numero=mv.mviaje_numero "+
				"left join dbxschema.mbusafiliado mb on mb.MCAT_PLACA=mv.MCAT_PLACA "+
				"where mp.mpla_codigo="+planilla+";";
			DBFunctions.Request(dsViaje,IncludeSchema.NO,sqlViaje);

			//Recorrido
			DataSet dsRecorrido=new DataSet();
			string recorrido="", sqlRecorrido="select mrc.secuencia, pc.pciu_nombre "+
				"from MRUTA_CIUDAD mrc, pciudad pc "+
				"where mrc.mrut_codigo='"+mruta+"' and pc.pciu_codigo=mrc.pciu_codigo order by mrc.secuencia;";
			DBFunctions.Request(dsRecorrido,IncludeSchema.NO,sqlRecorrido);
			for(int i=0;i<dsRecorrido.Tables[0].Rows.Count;i++)
				recorrido+=Convert.ToInt16(dsRecorrido.Tables[0].Rows[i][0]).ToString("D2")+"."+dsRecorrido.Tables[0].Rows[i][1].ToString()+"<br>";

			string fechaV="",horaV="",conductorV="",placaV="",noBus="",relevadorV="";
			if(dsViaje.Tables[0]!=null && dsPuestos.Tables[0].Rows.Count>0)
			{
				int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA"]);
				fechaV=Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA"]).ToString("yyyy-MM-dd");
				horaV= (hora/60).ToString("00")+":"+(hora%60).ToString("00");
				//horaV=Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00");
				conductorV=dsViaje.Tables[0].Rows[0]["CONDUCTOR"].ToString();
				relevadorV=dsViaje.Tables[0].Rows[0]["RELEVADOR"].ToString();
				placaV=dsViaje.Tables[0].Rows[0]["PLACA"].ToString();
				noBus=dsViaje.Tables[0].Rows[0]["NUMERO"].ToString();
			}
			//total vendido
			double totalTiquetes=Convert.ToDouble(DBFunctions.SingleData("Select coalesce(sum(valor_total),0) from DBXSCHEMA.MTIQUETE_VIAJE where TEST_CODIGO='V' AND mpla_codigo="+planilla+";"));
			int puestosVendidos=Convert.ToInt16(DBFunctions.SingleData("Select coalesce(sum(numero_puestos),0) from DBXSCHEMA.MTIQUETE_VIAJE where TEST_CODIGO='V' AND mpla_codigo="+planilla+";"));
			int totalPuestos=puestosLibres+puestosVendidos;

			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			string tiqueteV=Tiquetes.TraerSiguienteTiqueteVirtual();
			//tiqueteV=tiqueteV.Substring(tiqueteV.Length-6);
			string tiqueteM=tiqueteV;
			return(matrizPuestos+"@"+mruta+"@"+fechaV+"@"+horaV+"@"+conductorV+"@"+relevadorV+"@"+placaV+"@"+noBus+"@"+precioSug+"@"+tiqueteM+"@"+tiqueteV+"@"+puestosVendidos+"@"+totalTiquetes.ToString("#,###,##0")+"@"+puestosLibres+"@"+totalPuestos+"@"+matrizNumPuestos+"@"+recorrido);
			    /*             0         1          2         3              4              5          6         7             8            9           10                  11                                    12                  13               14                   15                     16      17*/ 
		} 
		

		//Comprar
		[Ajax.AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
		public string Comprar(string tiquete, string agencia, string planilla,string tipo,string ruta,string nit, string tnit, string nombre, string apellido,string telefono, int precio, string parametrosBus, bool prepago, double descuento)
		{ 
			if(agencia.Length>0)agencia=agencia;
			string []pPuestos=parametrosBus.Split('|');
			string []pPosicion;
			string numLineaS="",nitResponsable="";
			string rutaPrincipal,numeroViaje;
			int numLinea,fila=0,columna=0,numeroPuestos=0;
			double porcentajePrepago=descuento;
			double valorSeguro;
			if(pPuestos.Length<1)return("0@No se seleccionaron puestos");

			//Responsable
			nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
				return("0@El usuario responsable ("+HttpContext.Current.User.Identity.Name.ToString().ToLower()+") no tiene un NIT asignado");

			//Responsable Agencia
			string nitResponsableAgencia=DBFunctions.SingleData("SELECT mnit_encargado from dbxschema.magencia where mag_codigo="+agencia+";");

			//Verificar planilla no liquidada
			if(!DBFunctions.RecordExist("select mpla_codigo from dbxschema.mplanillaviaje where mag_codigo="+agencia+" AND fecha_liquidacion is null and mpla_codigo="+planilla+";"))
				return("0@Planilla no válida o ya despachada");
			
			//Numero de viaje debe estar activo o en venta
			numeroViaje=DBFunctions.SingleData("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			if(!DBFunctions.RecordExist("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MVIAJE WHERE MVIAJE_NUMERO="+numeroViaje+" AND (ESTADO_VIAJE='A' OR ESTADO_VIAJE='V');"))
				return("0@El viaje no se encuentra activo o en venta");

			//Precio
			if(precio<=0)
				return("0@Precio no válido");
			DataSet dsPrecios=new DataSet();
			string sqlPrecios="SELECT MRUT_VALSUG,MRUT_VALMAX,MRUT_VALMIN FROM DBXSCHEMA.MRUTAS WHERE MRUT_CODIGO='"+ruta+"';";
			DBFunctions.Request(dsPrecios,IncludeSchema.NO,sqlPrecios);
			double PrecioMinimo = Convert.ToDouble(dsPrecios.Tables[0].Rows[0]["MRUT_VALMIN"].ToString());
			double PrecioMaximo = Convert.ToDouble(dsPrecios.Tables[0].Rows[0]["MRUT_VALMAX"].ToString());
			double PrecioSugerido = Convert.ToDouble(dsPrecios.Tables[0].Rows[0]["MRUT_VALSUG"].ToString());
			double PrecioTiquete  = Convert.ToDouble(precio.ToString());

			if (PrecioTiquete > PrecioMaximo || PrecioTiquete < PrecioMinimo) 
				return("0@El precio está fuera del rango válido:--Sugerido:$"+PrecioSugerido.ToString("#,###,###")+"  --Minimo:$"+PrecioMinimo.ToString("#,###,###")+" --Maximo:$"+PrecioMaximo.ToString("#,###,###")+"");
			//Seguro
			try{
				valorSeguro=Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_SEGURO FROM DBXSCHEMA.CTRANSPORTES;"));
			}
			catch{
				valorSeguro=0;
			}
			//Ruta principal
			rutaPrincipal=DBFunctions.SingleData("SELECT MRUT_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			
			//Contar puestos
			for(int c=0;c<pPuestos.Length;c++)
				if(pPuestos[c].Length>0)
				{
					pPosicion=pPuestos[c].Split('-');
					if(pPosicion.Length==2)
						numeroPuestos++;
				}

			//solo puede ser 1 prepago
			if(prepago && numeroPuestos!=1)return("0@Solo se puede registrar 1 tiquete prepago a la vez.");

			ArrayList sqlStrings = new ArrayList();
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");
			//Crear nit si no existe
			nit=nit.Trim();

			if(nit.Length>0){
				if(!DBFunctions.RecordExist("SELECT MPAS_NIT FROM DBXSCHEMA.MPASAJERO WHERE MPAS_NIT='"+nit+"';"))
                    sqlStrings.Add("INSERT INTO DBXSCHEMA.MPASAJERO VALUES('"+nit+"','"+nombre+"',NULL,'"+apellido+"',NULL,'ND','"+ciudad+"','"+telefono+"','ND','ND');");
                    //sqlStrings.Add("INSERT INTO DBXSCHEMA.MNIT VALUES('"+nit+"',NULL,'"+nombre+"',NULL,'"+apellido+"',NULL,'"+tnit+"','"+ciudad+"','N','ND','"+ciudad+"','"+telefono+"','ND','ND','ND','V','N','N','T');");
				else
                    sqlStrings.Add("UPDATE DBXSCHEMA.MPASAJERO SET MPAS_TELEFONO = '" + telefono + "' WHERE MPAS_NIT='" + nit + "';");
				nit=nit;
			}
			else nit=DBFunctions.SingleData("select mpas_nit from dbxschema.mpasajero where mpas_nombres='Planilla Manual';");

			//No linea planilla
			numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla);
			if(numLineaS.Length==0)numLinea=1;
			else numLinea=int.Parse(numLineaS)+1;

			//Crear tiquete
			string numDocumento="";
			if(!prepago){
				numDocumento=Tiquetes.TraerSiguienteTiqueteVirtual();
				//papeleria automatica
				sqlStrings.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('TIQ',"+numDocumento+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+planilla+",'"+ruta+"',NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
				//tiquete
				sqlStrings.Add("INSERT INTO DBXSCHEMA.MTIQUETE_VIAJE VALUES('TIQ',"+numDocumento+","+planilla+","+numLinea+",'"+ruta+"','"+nitResponsable+"',"+numeroPuestos+",'"+nit+"',"+valorSeguro.ToString("0")+","+precio+","+(precio*numeroPuestos)+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','V');");
			}
			else{
				//PREPAGO esto se comentario mientras se trabaja con la venta de prepagos
				//ruta inversa
				/* OJO ESTO HAY QUE DEJARLO CUANDO ESTE AL DIA la venta de prepagos en version nueva
				string rutaInv="(MRUT_CODIGO='"+ruta+"' OR "+
					"MRUT_CODIGO IN ("+
					" SELECT MRI.MRUT_CODIGO FROM "+
					" DBXSCHEMA.MRUTAS MRI, DBXSCHEMA.MRUTAS MRP "+
					" WHERE MRI.PCIU_COD=MRP.PCIU_CODDES AND MRP.PCIU_COD=MRI.PCIU_CODDES AND "+
					" MRP.MRUT_CODIGO='"+ruta+"')"+
					")";
				*/	
				//Existe la papeleria de prepago? NO VALIDA RUTA NI ASIGNACION MIENTRAS TANTO estan todas las agencias en produccion
				//numDocumento=DBFunctions.SingleData("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='TIQPRE' AND NUM_DOCUMENTO="+tiquete+" AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M' AND "+rutaInv+" ORDER BY NUM_DOCUMENTO FETCH FIRST 1 ROWS ONLY;");
				numDocumento=DBFunctions.SingleData("SELECT NUM_DOCUMENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='TIQPRE' AND NUM_DOCUMENTO="+tiquete+" AND FECHA_ANULACION IS NULL AND FECHA_USO IS NULL AND TIPO_DOCUMENTO='M' ORDER BY NUM_DOCUMENTO FETCH FIRST 1 ROWS ONLY;");
				if(numDocumento.Length==0)
					return("0@No hay Tiquete prepago disponible (control papeleria)");
				//Guardar
				double porcentajePrepagoA=0;
				try{porcentajePrepagoA=Convert.ToDouble(DBFunctions.SingleData("SELECT PORCENTAJE_DESCUENTO FROM DBXSCHEMA.MCONTROL_PAPELERIA WHERE TDOC_CODIGO='TIQPRE' AND NUM_DOCUMENTO="+numDocumento+";"));}
				catch{porcentajePrepagoA=porcentajePrepago;}
				string numPuesto=DBFunctions.SingleData("SELECT DC.MELE_NUMEPUES FROM DBXSCHEMA.DCONFIGURACIONBUS DC,DBXSCHEMA.MVIAJE MV WHERE MV.MRUT_CODIGO='"+rutaPrincipal+"' AND MV.MVIAJE_NUMERO="+numeroViaje+" AND MV.MCON_COD=DC.MCON_COD AND DC.MPUE_POSFILA="+fila+" AND DC.MPUE_POSCOLUMNA"+columna+";");
				sqlStrings.Add("INSERT INTO dbxschema.mtiquete_viaje_prepago values('TIQPRE',"+numDocumento+","+agencia+","+planilla+","+numLinea+",'"+ruta+"',"+fila+","+columna+",'"+numPuesto+"','"+nitResponsableAgencia+"','"+nitResponsable+"','"+nit+"',"+valorSeguro.ToString("0")+","+precio+","+porcentajePrepagoA+","+((precio*(100-porcentajePrepagoA))/100)+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','V','"+DateTime.Now.ToString("yyyy-MM-dd")+"');");
				//usar papeleria prepago
				sqlStrings.Add("UPDATE DBXSCHEMA.MCONTROL_PAPELERIA SET FECHA_USO='"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',MPLA_CODIGO="+planilla+",MRUT_CODIGO='"+ruta+"',PORCENTAJE_DESCUENTO = "+porcentajePrepagoA+" WHERE TDOC_CODIGO='TIQPRE' AND NUM_DOCUMENTO="+numDocumento+";");
			}
			string tipoT=prepago?"TIQPRE":"TIQ";
			//Cambiar configuracion puesto ocupado
			for(int c=0;c<pPuestos.Length;c++)
				if(pPuestos[c].Length>0)
				{
					pPosicion=pPuestos[c].Split('-');
					if(pPosicion.Length==2){
						fila=int.Parse(pPosicion[0]);
						columna=int.Parse(pPosicion[1]);
						sqlStrings.Add("UPDATE DBXSCHEMA.MCONFIGURACIONPUESTO SET TDOC_CODIGO='"+tipoT+"', NUM_DOCUMENTO="+numDocumento+", TEST_CODIGO='OC' WHERE MRUT_CODIGO='"+rutaPrincipal+"' AND MVIAJE_NUMERO="+numeroViaje+" AND FILA="+fila+" AND COLUMNA="+columna+";");
					}
				}
			//Actualizar no lineas planilla
			sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");

			//Cambiar estado viaje->en venta
			sqlStrings.Add("UPDATE DBXSCHEMA.MVIAJE SET ESTADO_VIAJE='V' WHERE MRUT_CODIGO='"+rutaPrincipal+"' AND MVIAJE_NUMERO="+numeroViaje+";");

			//Revisar puestos libres
			DataRow []drOcupado;
			DataSet dsPuestos=new DataSet();
			//Puestos de rutas hermanas
			string sqlPuestos=System.Web.HttpContext.Current.Session["SQL_PUESTOS_"+planilla].ToString().Replace("@PUESTO@","");
			if(sqlPuestos.Length==0)
				sqlPuestos="SELECT MELE_NUMEPUES, FILA, COLUMNA FROM DBXSCHEMA.MCONFIGURACIONPUESTO WHERE MRUT_CODIGO='"+rutaPrincipal+"' AND MVIAJE_NUMERO="+numeroViaje+" AND TEST_CODIGO<>'DN';";
			DBFunctions.Request(dsPuestos, IncludeSchema.NO, sqlPuestos);
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
							return("0@El puesto "+drOcupado[0]["MELE_NUMEPUES"].ToString()+" ya fue asignado");
					}
				}

			if(DBFunctions.Transaction(sqlStrings)){
				return("1@Compra registrada@"+numDocumento+"@"+tipo+"@"+prepago);}
			else
				return("0@No se pudo comprar el tiquete.");
		}

		
		//Traer NIT
		[Ajax.AjaxMethod]
		public string TraaerNIT(string NIT)
		{
			DataSet dsNIT=new DataSet();
			string nombre="",apellido="",telefono="";
			DBFunctions.Request(dsNIT,IncludeSchema.NO,"select MPAS_NOMBRES concat ' ' concat coalesce(MPAS_NOMBRE2 ,'') as nombres, " +
                        "coalesce(MPAS_APELLIDOS,'') concat ' ' concat coalesce(MPAS_APELLIDO2,'') as apellidos, " +
                        "MPAS_TELEFONO  FROM DBXSCHEMA.MPASAJERO WHERE MPAS_NIT='"+NIT+"';");
			if(dsNIT.Tables[0].Rows.Count>0){
				nombre=dsNIT.Tables[0].Rows[0][0].ToString();
				apellido=dsNIT.Tables[0].Rows[0][1].ToString();
				telefono=dsNIT.Tables[0].Rows[0][2].ToString();
			}
			return(nombre+"|"+apellido+"|"+telefono);
		}

		//Generar Tiquete
		[Ajax.AjaxMethod]
		public string GenerarTiquete(string numero)
		{
			try
			{
				string plantilla="";
				string nlchar="\\n";
				int anchoTiquete=Tiquetes.anchoTiqueteMovil;
				try
				{
					string strLinea="";
					StreamReader strArchivo;
					strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaTiqueteMovil.txt");
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
					Response.Write("<script language='javascript'>alert('No se ha creado la plantilla de tiquetes, no se pudo imprimir.');</script>");
					return("Error: No se ha creado la planilla.");
				}
				string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
				if(nitResponsable.Length==0)
					return("NO TIENE PERMISO");

				plantilla=AMS.Comercial.Plantillas.GenerarTiquete(numero,plantilla,nlchar,nitResponsable,anchoTiquete);
				return(plantilla);
			}
			catch(Exception er)
			{
				return(er.Message);
			}
		}
		//Consultar datos tiquete vendido
		[Ajax.AjaxMethod(HttpSessionStateRequirement.ReadWrite)]
		public string DatosVenta(string planilla, string fila, string columna)
		{
			string datosV="",sqlVenta,sqlTiquete;
			DataSet dsTiquete=new DataSet();
			DataSet dsVenta=new DataSet();
			sqlTiquete=System.Web.HttpContext.Current.Session["SQL_PUESTOS_"+planilla].ToString();
			if(sqlTiquete.Length>0)
				sqlTiquete=sqlTiquete.Replace("@PUESTO@"," and MCP.FILA="+fila+" and MCP.COLUMNA="+columna+" ");
			else
				sqlTiquete="SELECT mt.tdoc_codigo,mt.num_documento "+
					"from MTIQUETE_VIAJE mt, MPLANILLAVIAJE mp, MCONFIGURACIONPUESTO mc "+
					"where mp.mpla_codigo="+planilla+" and mt.mpla_codigo=mp.mpla_codigo and "+
					"mc.mrut_codigo=mp.mrut_codigo and mc.mviaje_numero=mp.mviaje_numero and "+
					"mc.tdoc_codigo=mt.tdoc_codigo and mc.num_documento=mt.num_documento and "+
					"mc.fila="+fila+" and mc.columna="+columna+";";
			DBFunctions.Request(dsTiquete,IncludeSchema.NO,sqlTiquete);
			if(dsTiquete.Tables[0].Rows.Count==0)
				return("No hay datos.");
			datosV="["+dsTiquete.Tables[0].Rows[0]["num_documento"].ToString()+"]<br>";
            //sqlVenta="select mt.MRUT_CODIGO,"+
            //    "coalesce(MNIT_RESPONSABLE,'') AS NIT_TIQUETERO, "+
            //    "rtrim(coalesce(coalesce(mnr.mnit_APELLIDOS,'') concat ' ' concat coalesce( mnr.mnit_APELLIDO2,'') concat '<br>&nbsp;'  "+
            //    " concat mnr.mnit_NOMBRES concat ' ' concat coalesce(mnr.mnit_NOMBRE2 ,''),'')) AS TIQUETERO, "+
            //    "coalesce(MNIT_COMPRADOR,'') AS NIT_COMPRADOR, "+
            //    "rtrim(coalesce(coalesce(mnc.mnit_APELLIDOS,'') concat ' ' concat coalesce( mnc.mnit_APELLIDO2,'') concat '<br>&nbsp;'  "+
            //    " concat mnc.mnit_NOMBRES concat ' ' concat coalesce(mnc.mnit_NOMBRE2 ,''),'')) AS COMPRADOR, "+
            //    "coalesce(mnc.MNIT_TELEFONO,'') as TELEFONO, coalesce(mt.VALOR_PASAJE,0) as VALOR, coalesce(char(mt.FECHA_REPORTE),'') AS FECHA "+
            //    "from MTIQUETE_VIAJE mt "+
            //    "left join DBXSCHEMA.MNIT mnr on mt.MNIT_RESPONSABLE = mnr.MNIT_NIT "+
            //    "left join DBXSCHEMA.MNIT mnc on mt.MNIT_COMPRADOR = mnc.MNIT_NIT "+
            //    "where mt.tdoc_codigo='"+dsTiquete.Tables[0].Rows[0]["TDOC_CODIGO"].ToString()+"' and "+
            //    "mt.num_documento="+dsTiquete.Tables[0].Rows[0]["NUM_DOCUMENTO"].ToString()+";";

            sqlVenta = "select mt.MRUT_CODIGO, " +
                "coalesce(MNIT_RESPONSABLE,'') AS NIT_TIQUETERO,  " +
                "rtrim(coalesce(coalesce(mnr.MPAS_APELLIDOS,'') concat ' ' concat coalesce( mnr.MPAS_APELLIDO2,'') concat '<br>&nbsp;'   " +
                "concat mnr.MPAS_NOMBRES concat ' ' concat coalesce(mnr.MPAS_NOMBRE2 ,''),'')) AS TIQUETERO,  " +
                "coalesce(MNIT_COMPRADOR,'') AS NIT_COMPRADOR,  " +
                "rtrim(coalesce(coalesce(mnc.MPAS_APELLIDOS,'') concat ' ' concat coalesce( mnc.MPAS_APELLIDO2,'') concat '<br>&nbsp;'   " +
                "concat mnc.MPAS_NOMBRES concat ' ' concat coalesce(mnc.MPAS_NOMBRE2 ,''),'')) AS COMPRADOR,  " +
                "coalesce(mnc.MPAS_TELEFONO,'') as TELEFONO, coalesce(mt.VALOR_PASAJE,0) as VALOR, coalesce(char(mt.FECHA_REPORTE),'') AS FECHA  " +
                "from MTIQUETE_VIAJE mt  " +
                "left join DBXSCHEMA.mpasajero mnr on mt.MNIT_RESPONSABLE = mnr.MPAS_NIT  " +
                "left join DBXSCHEMA.mpasajero mnc on mt.MNIT_COMPRADOR = mnc.MPAS_NIT  " +
                "where mt.tdoc_codigo='" + dsTiquete.Tables[0].Rows[0]["TDOC_CODIGO"].ToString() + "' and " +
                "mt.num_documento=" + dsTiquete.Tables[0].Rows[0]["NUM_DOCUMENTO"].ToString() + ";";

			DBFunctions.Request(dsVenta,IncludeSchema.NO,sqlVenta);
			if(dsVenta.Tables[0].Rows.Count==0)
				return("No se encontró información.");
			datosV+=dsVenta.Tables[0].Rows[0]["MRUT_CODIGO"].ToString()+"<br>";
			datosV+="$"+Convert.ToDouble(dsVenta.Tables[0].Rows[0]["VALOR"]).ToString()+"<br>";
			datosV+=dsVenta.Tables[0].Rows[0]["FECHA"].ToString()+"<br>";
			datosV+="Comprador:<br>&nbsp;"+dsVenta.Tables[0].Rows[0]["NIT_COMPRADOR"].ToString()+"<br>&nbsp;"+dsVenta.Tables[0].Rows[0]["COMPRADOR"].ToString()+"<br>&nbsp;"+dsVenta.Tables[0].Rows[0]["TELEFONO"].ToString()+"<br>";
			datosV+="Tiquetero:<br>&nbsp;"+dsVenta.Tables[0].Rows[0]["NIT_TIQUETERO"].ToString()+"<br>&nbsp;"+dsVenta.Tables[0].Rows[0]["TIQUETERO"].ToString()+"<br>";
			return(datosV);
		}
		#endregion AJAX

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
			this.btnPlanillar.Click += new System.EventHandler(this.btnPlanillar_Click);
			//this.ddlAgencia.SelectedIndexChanged += new System.EventHandler(this.ddlAgencia_SelectedIndexChanged);
			this.ddlFechaPlanilla.SelectedIndexChanged += new System.EventHandler(this.ddlFechaPlanilla_SelectedIndexChanged);
			this.ddlPlanilla.SelectedIndexChanged += new System.EventHandler(this.ddlPlanilla_SelectedIndexChanged);
			this.btnConduce.Click += new System.EventHandler(this.btnConduce_Click);
			this.btnBoleta.Click += new System.EventHandler(this.btnBoleta_Click);
			//this.txtPrecio.TextChanged += new System.EventHandler(this.txtPrecio_TextChanged);
			this.btnPreDespacho.Click += new System.EventHandler(this.btnPreDespacho_Click);
			this.dgrAsignacion.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.dgrAsignacion_PageIndexChanged);
			this.btnAsociar.Click += new System.EventHandler(this.btnAsociar_Click);
			this.btnVerDespacho.Click += new System.EventHandler(this.btnVerDespacho_Click);
			this.btnDespachar.Click += new System.EventHandler(this.btnDespachar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region DropDownLists
		//Cambia la agencia
		public void ddlAgencia_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue;
			DatasToControls bind=new DatasToControls();
			pnlDespacho.Visible=false;
			pnlBus.Visible=true;
			lblTipoAgencia.Text="";
			//Cambia la ruta
			if(agencia.Length==0)
			{
				pnlPlanilla.Visible=false;
				pnlRutas.Visible=false;
				ddlFechaPlanilla.Items.Clear();
				ddlPlanilla.Items.Clear();
				return;
			}
			//Agencia virtual?
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+ddlAgencia.SelectedValue+";");
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			/*string tAgencia=DBFunctions.SingleData("SELECT MAGE_SISTEMA FROM MAGENCIA WHERE MAG_CODIGO="+agencia);
			tAgencia=tAgencia.Equals("S")?"V":"M";
			ddlTipo.SelectedIndex=ddlTipo.Items.IndexOf(ddlTipo.Items.FindByValue(tAgencia));*/
			//Consultar planillas no liquidadas de la agencia
			string Fecha=DateTime.Now.ToString("yyyy-MM-dd");
			ddlFechaPlanilla.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlFechaPlanilla,"select distinct char(mv.fecha_salida), char(mv.fecha_salida)"+
				"from dbxschema.mplanillaviaje mp, dbxschema.mviaje mv "+
				"where mp.mag_codigo="+agencia+" AND mp.fecha_liquidacion is null and "+
				"mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo and "+
				"mv.fecha_salida >='"+Fecha+"'  "+
			//	"AND mp.mnit_responsable='"+nitResponsable+"' "+
				" ORDER BY char(mv.fecha_salida)");
			
			ListItem itm=new ListItem(Fecha,Fecha);
			ddlFechaPlanilla.Items.Insert(0,itm);
			//cargar_planillas();
			string Fecha_planilla=ddlFechaPlanilla.SelectedValue;

		   // planilla=ddlPlanilla.SelectedValue;
			
			ddlPlanilla.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlPlanilla,"select mp.mpla_codigo, '[' concat coalesce(rtrim(char(mb.mbus_numero)),'000') concat '] [' concat coalesce(mv.MCAT_PLACA,'******') concat '] [' concat rtrim(char(mp.mpla_codigo)) concat "+ 
				" '] [' concat mp.mrut_codigo concat '] [' concat char(mv.fecha_salida) concat '] [' concat "+  
				" rtrim(char(floor(mv.hora_salida/60))) concat ':' "+  
				" concat case when (mod(mv.hora_salida,60)<10) then '0' else '' end concat "+  
				" rtrim(char(mod(mv.hora_salida,60)))  concat ']' "+
				" from dbxschema.mplanillaviaje mp, dbxschema.mviaje mv  LEFT JOIN dbxschema.mbusafiliado mb ON mb.mcat_placa=mv.mcat_placa "+ 
				" where mp.mag_codigo="+agencia+" AND mp.fecha_liquidacion is null and "+
				" mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo "+
				" AND mv.fecha_salida='"+Fecha_planilla+"' "+
				//	"AND mp.mnit_responsable='"+nitResponsable+"' "+
				" ORDER BY mv.fecha_salida,mp.mrut_codigo,mp.mpla_codigo; ");
			ListItem itm1=new ListItem("---Seleccione---","");
			ddlPlanilla.Items.Insert(0,itm1);
			pnlPlanilla.Visible=true;
			pnlRutas.Visible=false;
			//pnlPlanilla.Visible=false;
			//pnlRutas.Visible=false;
		}
		public void ddlFechaPlanilla_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string Fecha_planilla=ddlFechaPlanilla.SelectedValue;
			string agencia=ddlAgencia.SelectedValue;
			
			DatasToControls bind=new DatasToControls();
			pnlDespacho.Visible=false;
			pnlBus.Visible=true;
			lblTipoAgencia.Text="";
			//Cambia la ruta
			if(Fecha_planilla.Length==0)
			{
				pnlPlanilla.Visible=false;
				pnlRutas.Visible=false;
				ddlPlanilla.Items.Clear();
				return;
			}
			//Agencia virtual?
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+ddlAgencia.SelectedValue+";");
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			/*string tAgencia=DBFunctions.SingleData("SELECT MAGE_SISTEMA FROM MAGENCIA WHERE MAG_CODIGO="+agencia);
			tAgencia=tAgencia.Equals("S")?"V":"M";
			ddlTipo.SelectedIndex=ddlTipo.Items.IndexOf(ddlTipo.Items.FindByValue(tAgencia));*/
			//Consultar planillas no liquidadas de la agencia
			// cargar_planillas();
						
			ddlPlanilla.Items.Clear();
			bind.PutDatasIntoDropDownList(ddlPlanilla,"select mp.mpla_codigo, '[' concat coalesce(rtrim(char(mb.mbus_numero)),'000') concat '] [' concat coalesce(mv.MCAT_PLACA,'******') concat '] [' concat rtrim(char(mp.mpla_codigo)) concat "+ 
				"'] [' concat mp.mrut_codigo concat '] [' concat char(mv.fecha_salida) concat '] [' concat "+  
				" rtrim(char(floor(mv.hora_salida/60))) concat ':' "+  
				" concat case when (mod(mv.hora_salida,60)<10) then '0' else '' end concat "+  
				" rtrim(char(mod(mv.hora_salida,60)))  concat ']' "+
				" from dbxschema.mplanillaviaje mp, dbxschema.mviaje mv  LEFT JOIN dbxschema.mbusafiliado mb ON mb.mcat_placa=mv.mcat_placa "+ 
				" where mp.mag_codigo="+agencia+" AND mp.fecha_liquidacion is null and "+
				" mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo "+
				" AND mv.fecha_salida='"+Fecha_planilla+"' "+
				//	"AND mp.mnit_responsable='"+nitResponsable+"' "+
				" ORDER BY mv.fecha_salida,mp.mrut_codigo,mp.mpla_codigo; ");
			ListItem itm=new ListItem("---Seleccione---","");
			ddlPlanilla.Items.Insert(0,itm);
			pnlPlanilla.Visible=true;
			pnlRutas.Visible=false;
		}

		//Cambia la planilla
		public void ddlPlanilla_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue,planilla=ddlPlanilla.SelectedValue;
			string tipo="V";
			lblTipoAgencia.Text="";
			DatasToControls bind=new DatasToControls();
			pnlDespacho.Visible=false;
			if(planilla.Length==0)
			{
				pnlRutas.Visible=false;
				return;
			}
			pnlBus.Visible=true;

			// Ver si genera el conduce.
			DataSet dsViaje=new DataSet();
			DBFunctions.Request(dsViaje,IncludeSchema.NO,"select VALOR_CONDUCE,MCAT_PLACA,mnit_conductor,mnit_relevador1,mp.mrut_codigo from DBXSCHEMA.MVIAJE mv,dbxschema.mplanillaviaje mp,dbxschema.mrutas mr "+ 
				"where mp.mpla_codigo= "+planilla+"  "+
				 "and mp.mrut_codigo = mv.mrut_codigo "+
				 "and mp.MVIAJE_NUMERO = mv.MVIAJE_NUMERO "+
				 "and fecha_salida = current date "+
				 "and mp.mrut_codigo = mr.mrut_codigo;");
			if(dsViaje.Tables[0].Rows.Count==0) 
				PnlConduce.Visible=false;
			else
			{
				string placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
				double ValorConduce = Convert.ToDouble(dsViaje.Tables[0].Rows[0]["VALOR_CONDUCE"]);
				ViewState["ValorConduce"] = ValorConduce;
				ViewState["NitConductor"] = dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"].ToString();
				ViewState["NitRelevador"] = dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"].ToString();
                ViewState["Placa"] = placa;
				ViewState["Ruta"] = dsViaje.Tables[0].Rows[0]["MRUT_CODIGO"].ToString(); 
				if(ValorConduce > 0 && placa.Length > 0)
				{
					if(!DBFunctions.RecordExist(
						"Select * from DBXSCHEMA.MGASTOS_TRANSPORTES "+ 
						"where  mpla_codigo= "+planilla+" and tcon_codigo in (select concepto_conduce from DBXSCHEMA.CTRANSPORTES where CTRANS_CODIGO = 1);"))
						PnlConduce.Visible=true;
					else 
						PnlConduce.Visible=false;
				}
				else
					PnlConduce.Visible=false;
			}
			if(Convert.ToBoolean(Session["ISMOBILE"]))
				PnlConduce.Visible=false;
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
				"mr.pciu_cod=ma.pciu_codigo and mr.mrut_paradero='N' and "+
				"(mr.mrut_codigo in (select mi.mruta_secundaria from  dbxschema.mruta_intermedia mi where mi.mruta_principal=mp.MRUT_CODIGO)) "+
				"and mp.mpla_codigo="+planilla+" and ma.MAG_CODIGO="+agencia+" and pd.pciu_codigo=mr.pciu_coddes "+
				"order by mr.mrut_codigo;");
			ListItem itm=new ListItem("---Seleccione---","");
			ddlDestino.Items.Insert(0,itm);
			focus="document.getElementById('"+ddlDestino.ClientID+"').focus();cambiaTipo('"+tipo+"');";
			pnlRutas.Visible=true;
			DateTime FechaHoy = new DateTime();
            DateTime FechaSalida = new DateTime();
			string Fecha  = DateTime.Now.ToString("yyyy-MM-dd");
			FechaHoy = Convert.ToDateTime(Fecha);
			FechaSalida = Convert.ToDateTime(ddlFechaPlanilla.SelectedValue);
			if (FechaHoy != FechaSalida)
			    btnPreDespacho.Enabled=false;
			//Tipo agencia
			if(!DBFunctions.RecordExist(
				"select * from dbxschema.magencia ma, dbxschema.mrutas mr, dbxschema.mplanillaviaje mp "+
				"where ma.pciu_codigo=mr.pciu_cod and ma.mag_codigo="+agencia+
				" and mp.mpla_codigo="+planilla+" and mr.mrut_codigo=mp.mrut_codigo"))
				lblTipoAgencia.Text="[ AGENCIA DE PASO ]";
			else
				lblTipoAgencia.Text="[ AGENCIA DE ORIGEN ]";
		}

		
		#endregion DropDownLists
		#region Botones
		public void btnBoleta_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue,planilla=ddlPlanilla.SelectedValue;
						Response.Write("<script language='javascript'>alert('El conduce de viaje "+ddlPlanilla.SelectedValue+" se ha impreso.');"+
				"window.open('../aspx/AMS.Comercial.Viaje.aspx?pla="+ddlPlanilla.SelectedValue+"', 'VIAJE', 'width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");		
		}
		public void btnConduce_Click(object sender, System.EventArgs e)
		{
			string agencia=ddlAgencia.SelectedValue,planilla=ddlPlanilla.SelectedValue;
			
			double ValorConduce = Convert.ToDouble(ViewState["ValorConduce"].ToString()); 
			int concepto_conduce = Convert.ToInt16(ViewState["concepto_conduce"].ToString());
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			Response.Write("<script language='javascript'>alert('El conduce de viaje "+ddlPlanilla.SelectedValue+" se ha impreso.');"+
				"window.open('../aspx/AMS.Comercial.Viaje.aspx?pla="+ddlPlanilla.SelectedValue+"', 'VIAJE', 'width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");
			double ValorConduceMinimo = Convert.ToDouble(ConfigurationManager.AppSettings["ValorMinimoConduce"].ToString());
			if(ValorConduce >= ValorConduceMinimo)
			{
				//Actualizar papeleria
				string numDocumento=Anticipos.TraerSiguienteAnticipoVirtual();
				ArrayList sqlStrings = new ArrayList();
				string numLineaS=DBFunctions.SingleData("SELECT NUMERO_LINEAS+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla+";");
				sqlStrings.Add("INSERT INTO DBXSCHEMA.MCONTROL_PAPELERIA  VALUES('ANT',"+numDocumento+",'V',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,"+agencia+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+nitResponsable+"',0,'"+DateTime.Now.ToString("yyyy-MM-dd")+"',NULL,NULL,NULL,NULL,NULL,NULL,NULL,"+ddlPlanilla.SelectedValue+",NULL,NULL,'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"');");
				//Insertar servicio/anticipo
				string NitResponsableRecibe = ViewState["NitConductor"].ToString();
				if(NitResponsableRecibe.Length==0)
					NitResponsableRecibe = ViewState["NitRelevador"].ToString(); 
				
				sqlStrings.Add("INSERT INTO DBXSCHEMA.MGASTOS_TRANSPORTES VALUES('ANT',"+numDocumento+",'V','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+ViewState["Placa"].ToString()+"',"+ddlPlanilla.SelectedValue+","+numLineaS+","+concepto_conduce+","+agencia+",'"+nitResponsable+"','"+NitResponsableRecibe+"','Boleta Conduce',1,'UND',"+ValorConduce+","+ValorConduce+",'A',NULL);");

				//Actualizar planilla->num linea si es Real
			
				sqlStrings.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET NUMERO_LINEAS=NUMERO_LINEAS+1 WHERE MPLA_CODIGO="+planilla+";");
			
				if(DBFunctions.Transaction(sqlStrings))
					Response.Write("<script language='javascript'>alert('El anticipo/servicio ha sido registrado con el número "+numDocumento+".');"+
						"window.open('../aspx/AMS.Comercial.Anticipo.aspx?ant="+numDocumento+"','ANTICIPO"+numDocumento+"',\"width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no\");"+
						"</script>");
				else
				{
					lblError.Text += "Error: " + DBFunctions.exceptions;
					return;
				}
			}

			PnlConduce.Visible=false;
			//string parametrosBus="";
			//string tipo = "V";
			//return(CambiaRuta());	
			//btnConduce.Attributes.Add("onclick","return(CambiaRuta());");
			//CargarPuestos(planilla,ViewState["Ruta"].ToString(),tipo,agencia,parametrosBus);		
				
		}
		
		public void btnPreDespacho_Click(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			DataSet dsPlanillaViaje=new DataSet();
			DBFunctions.Request(dsPlanillaViaje, IncludeSchema.NO,("SELECT mp.mpla_codigo,COALESCE(mv.mcat_placa,' ') AS PLACA, "+
				"mp.mviaje_numero as numero_viaje,mp.mrut_codigo as RUTA_VIAJE FROM dbxschema.MVIAJE mv, dbxschema.mplanillaviaje mp "+
				"WHERE mp.mpla_codigo="+ddlPlanilla.SelectedValue+" and mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo;"));
			placa=dsPlanillaViaje.Tables[0].Rows[0]["PLACA"].ToString();
			lblPlacaOriginal.Text = placa;
			lblNroViaje.Text = dsPlanillaViaje.Tables[0].Rows[0]["NUMERO_VIAJE"].ToString();
			lblNroBus.Text = DBFunctions.SingleData("SELECT mbus_numero from dbxschema.mbusafiliado where mcat_placa='"+placa+"';");
			lblPlanila.Text = dsPlanillaViaje.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			pnlBus.Visible=false;
			pnlDespacho.Visible=true;
			lblPlacaOriginal.Text=placa;
			pnlCiudadOrigen.Visible=false;
			ddlAgencia.Enabled=false;
			ddlFechaPlanilla.Enabled=false;
			ddlPlanilla.Enabled=false;
			btnPlanillar.Visible=false;
			PnlConduce.Visible=false;

			string sqlC ="CALL DBXSCHEMA.ASIGNACION_DOCUMENTOS_PLANILLA("+ddlPlanilla.SelectedValue+");";
			ViewState["QUERY"]=sqlC;
			ViewState["PAGINA"] = 0;
			//Consulta
			dgrAsignacion.CurrentPageIndex=0;
						
			DBFunctions.Request(dsAsignacion, IncludeSchema.NO,sqlC);
			dgrAsignacion.DataSource=dsAsignacion.Tables[0];
			dgrAsignacion.DataBind();
			txtObservacion.Text = DBFunctions.SingleData("select OBSERVACION_PLANILLA from dbxschema.mplanilla_observacion where mpla_codigo="+ddlPlanilla.SelectedValue+";");
		}
		public void btnAsociar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlD=new ArrayList();
			DBFunctions.Request(dsAsignacion,IncludeSchema.NO,ViewState["QUERY"].ToString());
			CheckBox chkAsignar;
			
			for(int n=0;n<dgrAsignacion.Items.Count;n++)
			{
				chkAsignar=(CheckBox)dgrAsignacion.Items[n].FindControl("chkAsignacion");
				int Posicion = Convert.ToInt16(ViewState["PAGINA"])* 20 + n;
				if(chkAsignar.Checked)
				{
					if (dsAsignacion.Tables[0].Rows[Posicion]["TIPO"].ToString() == "REM")
						sqlD.Add("UPDATE dbxschema.MENCOMIENDAS SET MPLA_CODIGO = "+ddlPlanilla.SelectedValue+",MCAT_PLACA = '"+lblPlacaOriginal.Text+"'  WHERE NUM_DOCUMENTO = "+dsAsignacion.Tables[0].Rows[Posicion]["NUMERO"].ToString()+" ;");
					else
						if (dsAsignacion.Tables[0].Rows[Posicion]["TIPO"].ToString() == "ANT")
						sqlD.Add("UPDATE DBXSCHEMA.MGASTOS_TRANSPORTES SET MPLA_CODIGO = "+ddlPlanilla.SelectedValue+",MCAT_PLACA = '"+lblPlacaOriginal.Text+"' WHERE NUM_DOCUMENTO = "+dsAsignacion.Tables[0].Rows[Posicion]["NUMERO"].ToString()+" ;");
					else
						sqlD.Add("UPDATE DBXSCHEMA.MGIROS SET MPLA_CODIGO = "+ddlPlanilla.SelectedValue+" WHERE NUM_DOCUMENTO = "+dsAsignacion.Tables[0].Rows[Posicion]["NUMERO"].ToString()+";");
				}
				else
				{
					if (dsAsignacion.Tables[0].Rows[Posicion]["TIPO"].ToString() == "REM")
						sqlD.Add("UPDATE dbxschema.MENCOMIENDAS SET MPLA_CODIGO = NULL  WHERE NUM_DOCUMENTO = "+dsAsignacion.Tables[0].Rows[Posicion]["NUMERO"].ToString()+" ;");
					else
						if (dsAsignacion.Tables[0].Rows[Posicion]["TIPO"].ToString() == "ANT")
						sqlD.Add("UPDATE DBXSCHEMA.MGASTOS_TRANSPORTES SET MPLA_CODIGO = NULL WHERE NUM_DOCUMENTO = "+dsAsignacion.Tables[0].Rows[Posicion]["NUMERO"].ToString()+" ;");
					else
						sqlD.Add("UPDATE DBXSCHEMA.MGIROS SET MPLA_CODIGO = NULL WHERE NUM_DOCUMENTO = "+dsAsignacion.Tables[0].Rows[Posicion]["NUMERO"].ToString()+";");
				}
			}
			
			if(!DBFunctions.Transaction(sqlD))
			{
				lblError.Text=DBFunctions.exceptions;
				return;
			}
			//ObservacionPlanilla();
			//	DBFunctions.Request(dsAsignacion,IncludeSchema.NO,ViewState["QUERY"].ToString());
			//	dgrAsignacion.DataSource=dsAsignacion.Tables[0];
			//	dgrAsignacion.DataBind();
		}
		
		public void btnVerDespacho_Click(object sender, System.EventArgs e)
		{
			string Comando = "VerDespacho";
			string Programa = "Comercial.VentaTiquetes";
			ObservacionPlanilla();
			if(Tools.Browser.IsMobileBrowser())
				Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.PlanillaBus&pln="+ddlPlanilla.SelectedValue+ "&comando=" +Comando+"&programa="+Programa+"&act=2&path=Venta Tiquetes en Taquilla");
			else
				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.PlanillaBus&pln="+ddlPlanilla.SelectedValue+ "&comando=" +Comando+"&programa="+Programa+"&act=2&path=Venta Tiquetes en Taquilla");
			

			
		}

		public void btnDespachar_Click(object sender, System.EventArgs e)

		{
			string Comando = "Despachar";
			string Programa = "Comercial.VentaTiquetes";
			ObservacionPlanilla();
			placaActual=DBFunctions.SingleData("SELECT mv.mcat_placa "+
				"FROM dbxschema.MVIAJE mv, dbxschema.mplanillaviaje mp "+
				"WHERE mp.mpla_codigo="+ddlPlanilla.SelectedValue+" and mp.mviaje_numero=mv.mviaje_numero and mp.mrut_codigo=mv.mrut_codigo");
			if(placaActual.Length==0)
				Response.Write("<script language='javascript'>alert('Debe Asignar la placa del bus para el viaje Seleccionando .');</script>");
			else
			{
				string resDespacho=Buses.Despachar(ddlPlanilla.SelectedValue,ddlAgencia.SelectedValue,placaActual);
				ViewState["Placa"] = placaActual;
				//ViewState["Planilla"] = ddlPlanilla.SelectedValue;
				if(resDespacho.Length==0)
				{
					//Response.Write("<script language='javascript'>alert('La planilla "+ddlPlanilla.SelectedValue+" se ha impreso.');</script>");
					//Response.Write("<script language='javascript'>alert('La planilla "+ddlPlanilla.SelectedValue+" se ha impreso.');"+
					//	"window.open('../aspx/AMS.Comercial.Planilla.aspx?pln="+ViewState["Planilla"]+"', 'PLANILLA', 'width=340,height=600,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");
					//	"window.open('../aspx/AMS.Comercial.Planilla.aspx?pln="+ddlPlanilla.SelectedValue+"', 'PLANILLA'+'"+ddlPlanilla.SelectedValue+"', 'width=340,height=600,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");
					//Response.Write("<script language='javascript'>alert('El anticipo/servicio ha sido registrado con el número "+numDocumento+".');"+
					//	"window.open('../aspx/AMS.Comercial.Anticipo.aspx?ant="+numDocumento+"','ANTICIPO"+numDocumento+"',\"width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no\");"+
					//	"</script>");
					//window.open('../aspx/AMS.Comercial.Planilla.aspx?pln='+<%=ViewState["Planilla"]%>, 'PLANILLA'+<%=ViewState["Planilla"]%>, "width=340,height=600,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
					//Response.Write("<script language='javascript'>alert('El conduce de viaje "+ddlPlanilla.SelectedValue+" se ha impreso.');"+
					//"window.open('../aspx/AMS.Comercial.Viaje.aspx?pla="+ddlPlanilla.SelectedValue+"', 'VIAJE', 'width=340,height=290,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no');</script>");
					if(Tools.Browser.IsMobileBrowser())
						Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.PlanillaBus&pln="+ddlPlanilla.SelectedValue+ "&comando=" +Comando+"&programa="+Programa+"&act=3&path=Venta Tiquetes en Taquilla");
					else
						Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.PlanillaBus&pln="+ddlPlanilla.SelectedValue+ "&comando=" +Comando+"&programa="+Programa+"&act=3&path=Venta Tiquetes en Taquilla");

				}
				else
				{
					lblError.Text=resDespacho;
					Response.Write("<script language='javascript'>alert('"+resDespacho+"');</script>");
				}
			}
		}

		public void btnPlanillar_Click(object sender, System.EventArgs e)
		{
			if(Tools.Browser.IsMobileBrowser())
				Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.CrearViaje&act=1&path=Crear Viaje");
			else
				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Comercial.CrearViaje&act=1&path=Crear Viaje");
		}

		#endregion Botones
		public void dgrAsignacion_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{
			ViewState["PAGINA"]= e.NewPageIndex;
			dgrAsignacion.CurrentPageIndex=  Convert.ToInt16(ViewState["PAGINA"]);
			//DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAsignacion, IncludeSchema.NO,ViewState["QUERY"].ToString());
			dgrAsignacion.DataSource=dsAsignacion.Tables[0];
			dgrAsignacion.DataBind();

		}
		
		public void ObservacionPlanilla()
		{
			string anticipoViaje		= DBFunctions.SingleData("select tcon_codigo from dbxschema.tconceptos_transportes where tcon_codigo = (select CONCEPTO_ANTICIPO_VIAJE FROM DBXSCHEMA.CTRANSPORTES);");
			string anticipoViajeNombre	= DBFunctions.SingleData("select nombre      from dbxschema.tconceptos_transportes where tcon_codigo = (select CONCEPTO_ANTICIPO_VIAJE FROM DBXSCHEMA.CTRANSPORTES);");
            //lblAnticipoViaje.Text = anticipoViajeNombre;
            if (anticipoViaje.ToString().Length > 0)
			{
				lblAnticipoViaje.Visible = true;
				txtAnticipoViaje.Visible = true;
			}
			else 
			{
				lblAnticipoViaje.Visible = false;
				txtAnticipoViaje.Visible = false;
                txtAnticipoViaje.Text = "0";
			}
            //Campo de numero documento
            
			ArrayList sqlD=new ArrayList();
            if (anticipoViaje.ToString().Length > 0 && txtAnticipoViaje.Text != "0")
			{
				string NitResponsableRecibe = ViewState["NitConductor"].ToString();
                placaActual = ViewState["Placa"].ToString();
                nitResponsable = ViewState["nitResponsable"].ToString();
                sqlD.Add("INSERT INTO DBXSCHEMA.MGASTOS_TRANSPORTES VALUES('" + anticipoViaje + "'," + ddlPlanilla.SelectedValue + ",'V','" + ddlFechaPlanilla.SelectedValue + "','" + placaActual + "'," + ddlPlanilla.SelectedValue + ",null," + anticipoViaje.ToString() + "," + ddlAgencia.SelectedValue + ",'" + nitResponsable + "','" + NitResponsableRecibe + "','" + anticipoViajeNombre.ToString() + "',1,null," + txtAnticipoViaje.Text.Replace(",", "") + "," + txtAnticipoViaje.Text.Replace(",", "") + ",'A','" + ddlPlanilla.SelectedValue + "');");
			}
			bool ExisteObservacion = DBFunctions.RecordExist("select mpla_codigo from dbxschema.mplanilla_observacion where mpla_codigo="+ddlPlanilla.SelectedValue+";");
				
			if(txtObservacion.Text.Length==0)
			{
				if (ExisteObservacion) 
					sqlD.Add("DELETE FROM dbxschema.mplanilla_observacion WHERE MPLA_CODIGO="+ddlPlanilla.SelectedValue+";");
			}
			else
			{
				if (ExisteObservacion) 
					sqlD.Add("UPDATE dbxschema.mplanilla_observacion SET observacion_planilla='"+txtObservacion.Text+"' WHERE MPLA_CODIGO="+ddlPlanilla.SelectedValue+";");
				else 
					sqlD.Add("INSERT INTO dbxschema.mplanilla_observacion VALUES("+ddlPlanilla.SelectedValue+",'"+txtObservacion.Text+"');");
			}

			if(!DBFunctions.Transaction(sqlD))
			{
				lblError.Text=DBFunctions.exceptions;
				return;
			}
		}
	}
}