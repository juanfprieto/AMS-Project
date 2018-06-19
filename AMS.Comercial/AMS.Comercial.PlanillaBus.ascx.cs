namespace AMS.Comercial
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.IO;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using System.Configuration;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Comercial_PlanillaBus.
	/// </summary>
	public class AMS_Comercial_PlanillaBus : System.Web.UI.UserControl
	{
		#region Controles
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.Label lblPlanilla;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label lblDestino;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label lblAgencia;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label lblFecha;
		protected System.Web.UI.WebControls.Label Label7;
		protected System.Web.UI.WebControls.Label lblHora;
		protected System.Web.UI.WebControls.Label Label6;
		protected System.Web.UI.WebControls.Label Label10;
		protected System.Web.UI.WebControls.Label Label12;
		protected System.Web.UI.WebControls.Label lblConductor;
		protected System.Web.UI.WebControls.Label Label13;
		protected System.Web.UI.WebControls.Label lblRelevador;
		protected System.Web.UI.WebControls.DataGrid dgrTiquetes;
		protected System.Web.UI.WebControls.DataGrid dgrTiquetesPre;
		protected System.Web.UI.WebControls.DataGrid dgrRemesas;
		protected System.Web.UI.WebControls.DataGrid dgrGiros;
		protected System.Web.UI.WebControls.DataGrid dgrAnticipos;
		protected System.Web.UI.WebControls.Label Label14;
		protected System.Web.UI.WebControls.Label lblIngresos;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label31;
		protected System.Web.UI.WebControls.Label lblOrigen;
		protected System.Web.UI.WebControls.Label lblPlaca;
		protected System.Web.UI.WebControls.Label lblNumeroBus;
		protected System.Web.UI.WebControls.Label lblEgresos;
		protected System.Web.UI.WebControls.Label lblValorGiros;
		protected System.Web.UI.WebControls.Label lblDescuentos;
		protected double totalTiquetes=0;
		protected double totalPasajeros=0;
		protected double totalPasajerosPrep=0;
		protected double totalPrepago=0;
		protected double totalEncomiendas=0;
		protected double totalCostoGiros=0;
		protected double totalValorGiros=0;
        protected double TotalIngresos=0;
		protected double TotalOtrosIngresos=0;
        protected double TotalEgresos=0;
		protected double TotalDescuentos=0;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label lblPasajeros;
		protected System.Web.UI.WebControls.Panel pnlGemelas;
		protected System.Web.UI.WebControls.Panel ImprimirPlanillaMovil;
		protected System.Web.UI.WebControls.Label Labe13;
		protected System.Web.UI.WebControls.Button btnRegresar;
		protected System.Web.UI.WebControls.Panel Regresar;
		protected System.Web.UI.WebControls.Panel ImprimirPlanilla;
		protected System.Web.UI.WebControls.Label Label11;
		protected System.Web.UI.WebControls.Label Label25;
		protected System.Web.UI.WebControls.Label txtObservacion;
		protected System.Web.UI.WebControls.DataGrid dgrDescuentos;
		protected System.Web.UI.WebControls.Label Label9;
		protected System.Web.UI.WebControls.Button btnBorrarPlanilla;
		protected System.Web.UI.WebControls.Panel PnlBorrarPlanilla;
		protected System.Web.UI.WebControls.Panel pnlImpMovil;
		public string viaje, strPlanilla;
		#endregion
		protected System.Web.UI.WebControls.Label lblP;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if (Tools.Browser.IsMobileBrowser())
				{
					this.strPlanilla = this.Generar_Planilla(base.Request.QueryString["pln"]);
					btnBorrarPlanilla.Visible = false;
				}

				if(Request.QueryString["pln"]!=null)
					VerPlanilla(Request.QueryString["pln"]);
			}
		}

		public void VerPlanilla(string planilla)
		{
			ViewState["Planilla"]=planilla;
			DataSet dsPlanilla=new DataSet();
			DataSet dsViaje=new DataSet();

			DBFunctions.Request(dsPlanilla,IncludeSchema.NO, "SELECT *  FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla+";");
			if(dsPlanilla.Tables[0].Rows.Count==0)return;
			
			string agencia=dsPlanilla.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			string rutaP=dsPlanilla.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();
			viaje=dsPlanilla.Tables[0].Rows[0]["MVIAJE_NUMERO"].ToString();
			string origen=DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";");
			string destino=DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';");
			string agenciaR=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");

			DBFunctions.Request(dsViaje,IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MVIAJE WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
			if(dsViaje.Tables[0].Rows.Count==0)return;

			int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"]);
			string placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			string numVehiculo=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE	MCAT_PLACA='"+placa+"';");
			string horaS=(hora/60).ToString("00")+":"+(hora%60).ToString("00");
			string conductor=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';");
			string relevador=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';");
			lblPlanilla.Text=dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			lblOrigen.Text=origen;
			lblDestino.Text=destino;
			lblAgencia.Text=agenciaR;
			lblFecha.Text=Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd");
			lblHora.Text=horaS;
			lblPlaca.Text=placa;
			lblNumeroBus.Text=numVehiculo;
			lblConductor.Text=conductor;
			lblRelevador.Text=relevador;
		//	lblIngresos.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_INGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla)).ToString("###,###,###");
		//	lblEgresos.Text=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_EGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla)).ToString("###,###,###");
			
			//Tiquetes
			DataSet dsTiquetes=new DataSet();
			DBFunctions.Request(dsTiquetes,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.NUMERO_PUESTOS,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE  MV.TEST_CODIGO    <> 'A' AND MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+planilla+";");
			dgrTiquetes.DataSource=dsTiquetes.Tables[0];
			dgrTiquetes.DataBind();
		// OOJO
		//	lblPasajeros.Text=DBFunctions.SingleData("Select sum(MV.NUMERO_PUESTOS) "+
		//		"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
		//		"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+planilla+";");

			//Tiquetes Prepago
			DataSet dsTiquetesPre=new DataSet();
			DBFunctions.Request(dsTiquetesPre,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE_PREPAGO MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MV.TESTADO_TIQUETE = 'V' AND MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+planilla+";");
			dgrTiquetesPre.DataSource=dsTiquetesPre.Tables[0];
			dgrTiquetesPre.DataBind();

			//Encomiendas
			DataSet dsEncomiendas=new DataSet();
			DBFunctions.Request(dsEncomiendas,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_ENCOMIENDA,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MENCOMIENDAS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE  MV.TEST_CODIGO  <> 'R' AND MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+planilla+";");
			dgrRemesas.DataSource=dsEncomiendas.Tables[0];
			dgrRemesas.DataBind();

			//Giros
			DataSet dsGiros=new DataSet();
			DBFunctions.Request(dsGiros,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_GIRO,MV.VALOR_GIRO "+
				"FROM DBXSCHEMA.MGIROS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MAGENCIA MA "+
				"WHERE MV.TEST_CODIGO <> 'R' AND MA.MAG_CODIGO=MV.MAG_AGENCIA_DESTINO AND PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MV.MPLA_CODIGO="+planilla+";");
			dgrGiros.DataSource=dsGiros.Tables[0];
			dgrGiros.DataBind();

			//Anticipos
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,TG.NOMBRE AS CONCEPTO, CASE when(IMPUTACION_CONTABLE = 'D') then VALOR_TOTAL_AUTORIZADO  ELSE 0  END AS  VALOR_TOTAL_INGRESO, "+
				"CASE when(IMPUTACION_CONTABLE = 'C') then VALOR_TOTAL_AUTORIZADO ELSE 0   END AS  VALOR_TOTAL_EGRESO "+
				"FROM DBXSCHEMA.MGASTOS_TRANSPORTES MV, DBXSCHEMA.TCONCEPTOS_TRANSPORTES TG "+
				"WHERE MV.TEST_CODIGO     <> 'R'  AND MV.TCON_CODIGO=TG.TCON_CODIGO AND MV.MPLA_CODIGO="+planilla+";");
			dgrAnticipos.DataSource=dsAnticipos.Tables[0];
            dgrAnticipos.DataBind();
			//Descuentos
			DataSet dsDescuentos=new DataSet();
			DBFunctions.Request(dsDescuentos,IncludeSchema.NO, "Select dp.NUM_DOCUMENTO,TG.NOMBRE AS CONCEPTO, CASE when(IMPUTACION_CONTABLE = 'D') then VALOR_DESCUENTO *-1  ELSE VALOR_DESCUENTO  END AS  VALOR_DESCUENTO "+
				"FROM DBXSCHEMA.MDESCUENTOS_PLANILLA_VIAJE dp, DBXSCHEMA.TCONCEPTOS_TRANSPORTES TG "+
				"WHERE dp.TEST_CODIGO     <> 'R'  AND dp.TCON_CODIGO=TG.TCON_CODIGO AND dp.MPLA_CODIGO="+planilla+";");
			dgrDescuentos.DataSource=dsDescuentos.Tables[0];
			dgrDescuentos.DataBind();
			//Totales
			totalPasajeros += totalPasajerosPrep;
			lblPasajeros.Text = totalPasajeros.ToString("#,##0");
			TotalIngresos = totalTiquetes + totalEncomiendas + totalCostoGiros;
			lblIngresos.Text=TotalIngresos.ToString("###,###,##0");
			lblEgresos.Text=TotalEgresos.ToString("###,###,##0");
			lblDescuentos.Text=TotalDescuentos.ToString("###,###,##0");
			lblValorGiros.Text=totalValorGiros.ToString("###,###,##0");
			txtObservacion.Text = DBFunctions.SingleData("select OBSERVACION_PLANILLA from dbxschema.mplanilla_observacion where mpla_codigo="+planilla+";");
			if(Request.QueryString["comando"]== "Despachar")
			{
				if (!Tools.Browser.IsMobileBrowser())
				{
                    Utils.MostrarAlerta(Response, "La planilla " + ViewState["Planilla"] + " se ha impreso.');" +
                        "window.open('../aspx/AMS.Comercial.Planilla.aspx?pln=" + ViewState["Planilla"] + "', 'PLANILLA', 'width=340,height=600,top=0,left=0,toolbar=no,menubar=no,status=no,scrollbars=no,history=no");
 
					ImprimirPlanilla.Visible=true;
					ImprimirPlanillaMovil.Visible = pnlImpMovil.Visible = false;
					if(DBFunctions.RecordExist("SELECT MPLA_CODIGO FROM DBXSCHEMA.MPLANILLA_VIAJE_GEMELA WHERE MPLA_CODIGO="+planilla+";"))
						pnlGemelas.Visible=true;
				}
			}
			if (Tools.Browser.IsMobileBrowser())
			{
				ImprimirPlanilla.Visible = false;
				ImprimirPlanillaMovil.Visible = pnlImpMovil.Visible = true;
			}
			if(Request.QueryString["comando"]== "BorrarPlanilla")
				PnlBorrarPlanilla.Visible=true;
			

			if(Request.QueryString["comando"]== "VerDespacho")
				Regresar.Visible=true;

		}
		protected void btnBorrarPlanilla_Click(Object sender, EventArgs e)
		{
			string mensaje;
			DataSet dsMensaje=new DataSet();

			DBFunctions.Request(dsMensaje,IncludeSchema.NO,"CALL DBXSCHEMA.BORRAR_PLANILLA_COMPLETA("+ViewState["Planilla"]+","+Request.QueryString["agencia"]+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			if(dsMensaje.Tables[0].Rows.Count>0)
				mensaje = "MENSAJE: " + dsMensaje.Tables[0].Rows[0]["MENSAJE"].ToString() + "PLANILLA: " + dsMensaje.Tables[0].Rows[0]["PLANILLA"].ToString() + "AGENCIA: " + dsMensaje.Tables[0].Rows[0]["AGENCIA"].ToString() + "Codigo Retorno: " + dsMensaje.Tables[0].Rows[0]["NRO_MENSAJE"].ToString();
			else 
				mensaje = " OJO Revisar Planilla--> comunicarse col el soporte Ecas";
			btnBorrarPlanilla.Visible=false;
			Response.Redirect(indexPage+"?process=Comercial.BorrarPlanilla&path="+Request.QueryString["path"]+"&act="+(Request.QueryString["act"])+"&agencia="+(Request.QueryString["agencia"])+"&mensaje="+mensaje+"+&pla="+(Request.QueryString["pln"]));
			
		}

		protected void btnRegresar_Click(Object sender, EventArgs e)
		{
			if(Tools.Browser.IsMobileBrowser())
			{
				Response.Redirect(ConfigurationManager.AppSettings["MainMobileIndexPage"]+"?process=Comercial.VentaTiquetesMovil&path="+Request.QueryString["path"]+"&act="+(Request.QueryString["act"])+"&num="+viaje+"+&pla="+(Request.QueryString["pln"]));
			}
			else
			{
				if(Request.QueryString["comando"]== "BorrarPlanilla"){
					string mensaje = " Planilla No Borrada ";
					Response.Redirect(indexPage+"?process=Comercial.BorrarPlanilla&path="+Request.QueryString["path"]+"&act="+(Request.QueryString["act"])+"&agencia="+(Request.QueryString["agencia"])+"&mensaje="+mensaje+"+&pla="+(Request.QueryString["pln"]));
				}
				else
					Response.Redirect(indexPage+"?process="+Request.QueryString["programa"]+"&path="+Request.QueryString["path"]+"&act="+(Request.QueryString["act"])+"&num="+viaje+"+&pla="+(Request.QueryString["pln"]));
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
			this.dgrTiquetes.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrTiquetes_ItemDataBound);
			this.dgrTiquetesPre.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrTiquetesPre_ItemDataBound);
			this.dgrRemesas.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrRemesas_ItemDataBound);
			this.dgrGiros.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrGiros_ItemDataBound);
			this.dgrAnticipos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrAnticipos_ItemDataBound);
			this.dgrDescuentos.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgrDescuentos_ItemDataBound);
			this.btnBorrarPlanilla.Click += new System.EventHandler(this.btnBorrarPlanilla_Click);
			this.btnRegresar.Click += new System.EventHandler(this.btnRegresar_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void dgrTiquetes_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				totalTiquetes+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[4].ToString());
				totalPasajeros+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString());
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[4].Text=totalTiquetes.ToString("#,#");
				e.Item.Cells[2].Text=totalPasajeros.ToString("#,#");
			}
			
		}

		private void dgrTiquetesPre_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				totalPasajerosPrep+=1;
				totalPrepago+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[3].ToString());
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=totalPasajerosPrep.ToString("#,#");
				e.Item.Cells[3].Text=totalPrepago.ToString("#,#");
			}
		}

		private void dgrRemesas_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
				totalEncomiendas+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[3].ToString());
			if(e.Item.ItemType==ListItemType.Footer)
				e.Item.Cells[3].Text=totalEncomiendas.ToString("#,#");
		}

		private void dgrGiros_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				totalCostoGiros+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString());
				totalValorGiros+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[3].ToString());
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=totalCostoGiros.ToString("#,#");
				e.Item.Cells[3].Text=totalValorGiros.ToString("#,#");
			}
		}

		private void dgrAnticipos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				TotalOtrosIngresos+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString());
				TotalIngresos+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString());
				TotalEgresos+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[3].ToString());
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=TotalOtrosIngresos.ToString("#,#");
				e.Item.Cells[3].Text=TotalEgresos.ToString("#,#");
			}
		}
		private void dgrDescuentos_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType==ListItemType.Item ||e.Item.ItemType==ListItemType.AlternatingItem )
			{
				TotalDescuentos+=Convert.ToDouble(((DataRowView)e.Item.DataItem).Row.ItemArray[2].ToString());
				
			}
			if(e.Item.ItemType==ListItemType.Footer)
			{
				e.Item.Cells[2].Text=TotalDescuentos.ToString("#,#");
				
			}
		}

		public string Generar_Planilla(string numero)
		{
			string plantilla="";
			string nlchar="\\n",redChar="";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaPlanillaMovil.txt");
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
                Utils.MostrarAlerta(Response, "No se ha creado la plantilla de planillas, no se pudo imprimir.");

				return("");
			}
			plantilla=plantilla.Replace("<RED>",redChar);

			return(Comercial.Plantillas.GenerarPlanilla(numero,plantilla,nlchar,redChar,anchoTiquete));
		}
	}
}

