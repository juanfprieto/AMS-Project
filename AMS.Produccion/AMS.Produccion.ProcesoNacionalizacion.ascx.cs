namespace AMS.Produccion
{
	using System;
	using System.IO;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Services;
	using System.Web.SessionState;
	using System.Web.Services.Protocols;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Ajax;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Documentos;
	using AMS.Inventarios;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Produccion_ProcesoNacionalizacion.
	/// </summary>
	public partial class AMS_Produccion_ProcesoNacionalizacion : System.Web.UI.UserControl
	{
		#region Controles
		public string scrFocus;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			this.Page.SmartNavigation=true;
			if(!Page.IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlEmbarque, 
					"SELECT DISTINCT MEM.MEMB_SECUENCIA, MEM.MEMB_NUMEEMBA "+
					"FROM MEMBARQUE MEM,DEMBARQUECKD DEM "+
					"WHERE MEM.MEMB_SECUENCIA=DEM.MEMB_SECUENCIA AND MEM.PEST_ESTADO='P';");
				//bind.PutDatasIntoDropDownList(ddlPrefDoc,"SELECT pdoc_codigo from pdocumento where tdoc_tipodocu='FP';");
                Utils.llenarPrefijos(Response, ref ddlPrefDoc , "%", "%", "FP");
				txtFecha.Text=DateTime.Now.ToString("yyyy-MM-dd");
				CargarVehiculos();
				ddlEmbarque_SelectedIndexChanged(ddlEmbarque, e);
				ddlPrefDoc_SelectedIndexChanged(ddlPrefDoc,e);
				if(Request.QueryString["upd"]!=null)
                Utils.MostrarAlerta(Response, "Se han nacionalizado los vehiculos.");
				
				#region Reportes
				if(Request.QueryString["pref"]!=null && Request.QueryString["num"]!=null)
				{
					FormatosDocumentos formatoFactura=new FormatosDocumentos();
                    Utils.MostrarAlerta(Response, "Se ha generado la factura con prefijo " + Request.QueryString["pref"] + " y número " + Request.QueryString["num"] + "");
					try
					{
						formatoFactura.Prefijo=Request.QueryString["pref"];
						formatoFactura.Numero=Convert.ToInt32(Request.QueryString["num"]);
						formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["pref"]+"'");
						if(formatoFactura.Codigo!=string.Empty)
						{
							if(formatoFactura.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
					}
					catch
					{
						lblInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
					}
				}
				#endregion Reportes
			}
		}

		//Cargar Vehiculos estado=solicitud nacionalización
		private void CargarVehiculos()
		{
			DataSet dsVehiculos=new DataSet();
			DBFunctions.Request(dsVehiculos,IncludeSchema.NO,
				"SELECT MC.MCAT_VIN, MC.MCAT_MOTOR, PC.PCOL_DESCRIPCION, MC.PCAT_CODIGO, "+
				"1 USAR, cast(0 as decimal(10,4)) VALOR, cast(0 as decimal(10,2)) IVA "+
				"FROM MCATALOGOVEHICULO MC, MVEHICULO MV, PCOLOR PC "+
				"WHERE MC.MCAT_VIN=MV.MCAT_VIN AND MC.PCOL_CODIGO=PC.PCOL_CODIGO AND MV.TEST_TIPOESTA=5 "+
				"ORDER BY MC.PCAT_CODIGO, MC.MCAT_VIN;");
			dgEnsambles.DataSource=dsVehiculos.Tables[0];
			dgEnsambles.DataBind();
			ViewState["VEHICULOS"]=dsVehiculos.Tables[0];
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

		//Seleccionar Embarque
		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlStrings=new ArrayList();
			DataTable dtVehiculos=(DataTable)ViewState["VEHICULOS"];
			string manifiesto=txtManifiesto.Text.Trim();
			string d_o=txtDO.Text.Trim();
			string levante=txtLevante.Text.Trim();
			string aduana=txtAduana.Text.Trim();
			double tasaCambioI=0,tasaCambioN=0,totalEmbarque=0,valor,totalIVA=0,subtotal=0,totalEmbarqueM=0;
			string tGN,tGE;
			double totalGastosE=0,totalGastosN=0;
			double unidadesEmbarque=0;
			DateTime fechaProceso;

			#region Validaciones
			for(int n=0;n<dgEnsambles.Items.Count;n++)
			{
				if(((CheckBox)dgEnsambles.Items[n].FindControl("chkUsarE")).Checked)
				{
					sqlStrings.Add(
						"UPDATE MVEHICULO "+
						"SET MVEH_NUMEMANI='"+txtManifiesto.Text+"', MVEH_NUMED_O='"+txtDO.Text+"', "+
						"MVEH_NUMELEVANTE='"+txtLevante.Text+"', MVEH_ADUANA='"+txtAduana.Text+"', "+
						"TEST_TIPOESTA=20 "+
						"WHERE MCAT_VIN='"+dtVehiculos.Rows[n]["MCAT_VIN"]+"';");
					dtVehiculos.Rows[n]["USAR"]=1;

				}
				else 
					dtVehiculos.Rows[n]["USAR"]=0;
			}
			if(sqlStrings.Count==0)
			{
                Utils.MostrarAlerta(Response, "No seleccionó vehículos.");
				return;
			}
			//Tasa de cambio I
			try
			{
				tasaCambioI=Convert.ToDouble(txtTasaCambioI.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Tasa de cambio no válida.");
				return;
			}
			//Tasa de cambio N
			try
			{
				tasaCambioN=Convert.ToDouble(txtTasaCambio.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Tasa de cambio no válida.");
				return;
			}
			//Embarques
			if(ddlEmbarque.Items.Count==0)
			{
                Utils.MostrarAlerta(Response, "No hay embarques.");
				return;
			}
			//Manifiesto
			if(manifiesto.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe ingresar el manifiesto.");
				return;
			}
			//DO
			if(d_o.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe ingresar el D.O.");
				return;
			}
			//Levante...
			if(levante.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe ingresar el levante.");
				return;
			}
			//Aduana
			if(aduana.Length==0)
			{
                Utils.MostrarAlerta(Response, "Debe la aduana.");
				return;
			}
			//Fecha proceso
			try
			{
				fechaProceso=Convert.ToDateTime(txtFecha.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "Fecha no válida.");
				return;
			}
			#endregion

			#region Consultar Dembarqueckd
			//Detalles
			DataSet dsEmbarque=new DataSet();
			DBFunctions.Request(dsEmbarque, IncludeSchema.NO,
				"SELECT * FROM DEMBARQUECKD WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+";");
			
			btEjecutar.Enabled=false;
			plcNacionalizar.Visible=true;
			#endregion
			
			#region Gastos
			//Cargar gastos
			DataSet dsGastos=new DataSet();
			DBFunctions.Request(dsGastos,IncludeSchema.NO,
				"Select df.PDOC_CODIORDEPAGO PREFO,df.MFAC_NUMEORDEPAGO NUMO,df.pgas_codigo,pg.PGAS_NOMBRE gasto,sum(df.dfac_valorgasto) valor, pg.PGAS_MODENACI "+
				"from DFACTURAGASTOEMBARQUE df, PGASTODIRECTO pg "+
				"where memb_secuencia="+ddlEmbarque.SelectedValue+" and pg.PGAS_CODIGO=df.PGAS_CODIGO "+
				"group by memb_secuencia,df.pgas_codigo,pgas_nombre,df.PDOC_CODIORDEPAGO,df.MFAC_NUMEORDEPAGO,pg.PGAS_MODENACI;");
			dgrGastos.DataSource=dsGastos.Tables[0];
			dgrGastos.DataBind();

			txtManifiesto.Enabled=txtDO.Enabled=txtLevante.Enabled=txtAduana.Enabled=txtFecha.Enabled=txtTasaCambioI.Enabled=txtTasaCambio.Enabled=ddlEmbarque.Enabled=ddlPrefDoc.Enabled=false;
			
			//Gastos moneda extranjera
			tGE=DBFunctions.SingleData(
				"SELECT "+
				"SUM(DF.DFAC_VALORGASTO) "+
				"FROM DFACTURAGASTOEMBARQUE DF, PGASTODIRECTO PG "+
				"WHERE "+
				"PG.PGAS_CODIGO=DF.PGAS_CODIGO AND DF.MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+" AND "+
				"PG.PGAS_MODENACI='N';");
			if(tGE.Length>0)
				totalGastosE=Convert.ToDouble(tGE);

			//Gastos moneda nacional
			tGN=DBFunctions.SingleData(
				"SELECT "+
				"SUM(DF.DFAC_VALORGASTO) "+
				"FROM DFACTURAGASTOEMBARQUE DF, PGASTODIRECTO PG "+
				"WHERE "+
				"PG.PGAS_CODIGO=DF.PGAS_CODIGO AND DF.MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+" AND "+
				"PG.PGAS_MODENACI='S';");
			if(tGN.Length>0)
				totalGastosN=Convert.ToDouble(tGN);
			#endregion
			
			#region Validar Catalogos en embarque
			DataRow[] drVs;
			DataRow drV;
			int unidades=0;
			for(int n=0;n<dtVehiculos.Rows.Count;n++)
			{
				drV=dtVehiculos.Rows[n];
				if(Convert.ToInt16(drV["USAR"])==1)
				{
					if(dsEmbarque.Tables[0].Select("PCAT_CODIGO='"+drV["PCAT_CODIGO"]+"'").Length==0){
                        Utils.MostrarAlerta(Response, "El catálogo " + drV["PCAT_CODIGO"] + " no se encuentra en el embarque.");
						return;
					}

					sqlStrings.Add("UPDATE DEMBARQUECKD SET DITE_CANTNACI=DITE_CANTNACI+1 "+
					"WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+" AND PCAT_CODIGO='"+drV["PCAT_CODIGO"]+"';");
					unidades++;
				}
			}
			#endregion
			
			#region Validar total embarque
			//Detalles embarque totalCIF
			for(int n=0;n<dsEmbarque.Tables[0].Rows.Count;n++)
			{
				drV=dsEmbarque.Tables[0].Rows[n];
				valor=Convert.ToDouble(drV["DITE_VALOFOB"])*Convert.ToDouble(drV["DITE_CANTEMBA"]);
				unidadesEmbarque+=Convert.ToDouble(drV["DITE_CANTEMBA"]);
				totalEmbarque+=valor;
			}
			
			totalEmbarqueM=Convert.ToDouble(DBFunctions.SingleData(
				"SELECT MLIC_VALOEMBA FROM MEMBARQUE WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+";"));
			
			if(Math.Abs(totalEmbarqueM-totalEmbarque)>(totalEmbarqueM/100))
			{
				btEjecutar.Enabled=false;
                Utils.MostrarAlerta(Response, "La sumatoria de los detalles del embarque no coincide con el valor total del embarque!");
			}
			#endregion

			string ensamble="";
			//Planta
			string planta=DBFunctions.SingleData("SELECT MPLA_CODIGO FROM MPLANTAS WHERE TPRO_CODIGO='E' FETCH FIRST 1 ROWS ONLY;");
			//CIF:
			double CIF=CosteoProduccion.TraerCIF(planta,DateTime.Now.Year,DateTime.Now.Month);
			//Horas laboradas al mes
			string horasMes=DBFunctions.SingleData("SELECT cnom_horaslabxmes from CNOMINA");
			//Carga Prestacional
			double cargaPrestacional=Convert.ToDouble(DBFunctions.SingleData("SELECT CNOM_PORCCARGPRES from CNOMINA"));
			//Costo hora planta
			double costoHoraPlanta=Convert.ToDouble(DBFunctions.SingleData("SELECT MPLA_COSTOHORA FROM MPLANTAS WHERE MPLA_CODIGO='"+planta+"';"));
			//Capacidad Planta
			double capacidadPlanta=Convert.ToDouble(DBFunctions.SingleData("SELECT MPLA_CAPACIDAD FROM MPLANTAS WHERE MPLA_CODIGO='"+planta+"';"));
			CosteoProduccion costeo;
			string numD=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefDoc.SelectedValue+"';");
			string prfD=ddlPrefDoc.SelectedValue;
			double cifV=0,gravamenV=0,ivaV=0,gcifV=0,costoUniV=0,factorCIF=0,factorImportacion=0,totalCostoImportacion=0,costeoV=0;

			for(int n=0;n<dtVehiculos.Rows.Count;n++)
			{
				drV=dtVehiculos.Rows[n];
				dtVehiculos.Rows[n]["IVA"]=0;
				if(Convert.ToInt16(drV["USAR"])==1)
				{
					drVs=dsEmbarque.Tables[0].Select("PCAT_CODIGO='"+drV["PCAT_CODIGO"]+"'");
					valor=Convert.ToDouble(drVs[0]["DITE_VALOFOB"]);
					//Response.Write("<H1>"+drV["MCAT_VIN"]+"</H1><br>");
					//Response.Write("fob: "+valor+"<br>");

					cifV=totalEmbarque+totalGastosE;
					//Response.Write("CIF: "+cifV+"<br>");
					gravamenV=Convert.ToDouble(DBFunctions.SingleData(
						"SELECT COALESCE(PA.PARA_GRAVAME,0) "+
						"FROM PCATALOGOVEHICULO PC LEFT JOIN PARANCEL PA ON PA.PARA_CODIGO=PC.PARA_CODIGO "+
						"WHERE PC.PCAT_CODIGO='"+drV["PCAT_CODIGO"]+"';"));
					gravamenV=cifV*tasaCambioN*gravamenV/100;
					//Response.Write("GRAVAMEN: "+gravamenV+"<br>");
					ivaV=Convert.ToDouble(DBFunctions.SingleData(
						"SELECT COALESCE(PA.PIVA_PORCIVA,0) "+
						"FROM PCATALOGOVEHICULO PC LEFT JOIN PARANCEL PA ON PA.PARA_CODIGO=PC.PARA_CODIGO "+
						"WHERE PC.PCAT_CODIGO='"+drV["PCAT_CODIGO"]+"';"))/100;
					ivaV+=(cifV*tasaCambioN+gravamenV)*ivaV;
					//Response.Write("IVA: "+ivaV+"<br>");
					totalIVA+=ivaV;
					gcifV=(cifV*tasaCambioI)+gravamenV+totalGastosN;
					//Response.Write("GCIF: "+gcifV+"<br>");
					costoUniV=(gcifV/totalEmbarque)*valor;
					//Response.Write("Costo unitario: "+costoUniV+"<br>");
					valor=costoUniV;
					

					#region Costeo
					ensamble=DBFunctions.SingleData(
						"SELECT PENS_CODIGO FROM PENSAMBLEPRODUCCION WHERE PCAT_CODIGO='"+dtVehiculos.Rows[n]["PCAT_CODIGO"]+"';"
					);
					costeo=new CosteoProduccion(
						dtVehiculos.Rows[n]["PCAT_CODIGO"].ToString(), 1, ensamble, TipoCosteo.Ensamble,
						CIF, horasMes, costoHoraPlanta, capacidadPlanta,cargaPrestacional);
					costeoV=costeo.Calcular();
					valor+=costeoV;
					//Response.Write("Costeo: "+costeoV+"<br>");
					//Response.Write("TOTAL: "+valor+"<br><br><br>");
					if(costeo.error.Length>0)
					{
                        Utils.MostrarAlerta(Response, "" + costeo.error + "");
						return;
					}

					#endregion
					
					sqlStrings.Add("UPDATE MVEHICULO SET MVEH_VALOCOMP="+valor+" "+
									"where MCAT_VIN='"+drV["MCAT_VIN"]+"';");

					factorCIF=totalGastosE/totalEmbarque;
					factorImportacion=gcifV/totalEmbarque;
					totalCostoImportacion=gcifV-(totalEmbarque*tasaCambioN);
					subtotal+=valor;
					sqlStrings.Add(
						"INSERT INTO DLIQUIDACIONCKD VALUES("+
						"DEFAULT,"+
						ddlEmbarque.SelectedValue+","+
						"'"+drV["PCAT_CODIGO"]+"','"+drV["MCAT_VIN"]+"',"+
						totalEmbarque+","+gcifV/unidadesEmbarque+","+gravamenV/unidadesEmbarque+","+
						ivaV/unidadesEmbarque+","+tasaCambioN+","+tasaCambioI+","+totalGastosE/unidadesEmbarque+","+
						totalGastosN/unidadesEmbarque+","+factorCIF+","+
						factorImportacion+","+totalCostoImportacion/unidadesEmbarque+","+
						"'"+HttpContext.Current.User.Identity.Name+"',"+
						"'"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"');"
					);
					sqlStrings.Add("INSERT INTO DCOSTOPRODUCCION VALUES('"+prfD+"',"+numD+","+
						costeo.totalMateriaPrima+","+costeo.totalTrabajoTerceros+","+costeo.totalManoObra+","+
						costeo.totalCIF+","+costeo.totalMaquinas+",'"+drV["MCAT_VIN"]+"');");
				}
			}
			
			sqlStrings.Add("UPDATE pdocumento set pdoc_ultidocu=pdoc_ultidocu+1 "+
							"WHERE pdoc_codigo='"+prfD+"';");
			
			totalIVA=unidades*(totalIVA/unidadesEmbarque);

			#region Factura Proveedor
			FacturaProveedor facturaRepuestos = new FacturaProveedor();
			string prefijoFacturaProveedor = ddlPrefDoc.SelectedValue;
			UInt64 numeroFacturaProveedor = Convert.ToUInt64(lblNumDoc.Text.Trim());
			string nit=DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA;");
			if(DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFacturaProveedor+"' AND mfac_numeordepago="+numeroFacturaProveedor+""))
				numeroFacturaProveedor = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFacturaProveedor+"'"));
		
			FacturaProveedor facturaRepuestosProv = new FacturaProveedor("FPR",prefijoFacturaProveedor,
				prefijoFacturaProveedor,nit,planta,"F",numeroFacturaProveedor,
				numeroFacturaProveedor,"P",DateTime.Now,
				DateTime.Now,Convert.ToDateTime(null),DateTime.Now,0,
				0,0,0,0,txtLevante.Text,
				HttpContext.Current.User.Identity.Name.ToLower());
		
			facturaRepuestosProv.GrabarFacturaProveedor(false);

			ViewState["NUM_FAC"] = facturaRepuestosProv.NumeroFactura;
			ViewState["PRE_FAC"] = facturaRepuestosProv.PrefijoFactura;
		
			for(int i=0;i<facturaRepuestosProv.SqlStrings.Count;i++)
				sqlStrings.Insert(0,facturaRepuestosProv.SqlStrings[facturaRepuestosProv.SqlStrings.Count-i-1].ToString());
			#endregion Factura Proveedor


			txtTotalEmbarque.Text=totalEmbarque.ToString("C");
			txtTotalUnidades.Text=unidades.ToString("0");
			txtTotalGastosE.Text=totalGastosE.ToString("C");
			txtTotalGastosN.Text=totalGastosN.ToString("C");
			txtIVA.Text=totalIVA.ToString("C");
			txtSubtotal.Text=subtotal.ToString("C");
			txtTotal.Text=(totalIVA+subtotal).ToString("C");

			ViewState["DLIQUIDACION"]=sqlStrings;
			ViewState["VEHICULOS"]=dtVehiculos;
			btEjecutar.Enabled=true;
			btnSeleccionar.Visible=false;
			
			for(int n=0;n<dgEnsambles.Items.Count;n++)
				((CheckBox)dgEnsambles.Items[n].FindControl("chkUsarE")).Enabled=false;

			if(sqlStrings.Count==0){
                Utils.MostrarAlerta(Response, "No seleccionó vehículos.");
				return;
			}
		}

		
		//Ejecutar proceso
		protected void btEjecutar_Click(object sender, System.EventArgs e)
		{
			ArrayList sqlStrings=(ArrayList)ViewState["DLIQUIDACION"];

			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(""+indexPage+"?process=Produccion.ProcesoNacionalizacion&upd=1&path="+Request.QueryString["path"]+"&pref="+ViewState["PRE_FAC"]+"&num="+ViewState["NUM_FAC"]);
			else
				lblInfo.Text += "<br>Error : Detalles <br>"+DBFunctions.exceptions;
		}

		
		//Cambia el embarque
		protected void ddlEmbarque_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(ddlEmbarque.Items.Count==0)
			{
                Utils.MostrarAlerta(Response, "No hay embarques en el puerto local.");
				btnSeleccionar.Enabled=false;
				return;
			}
			string manifiesto=DBFunctions.SingleData(
				"SELECT MLIB_MANIFIESTO FROM MLIBERACIONCKD "+
				"WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+";");
			if(manifiesto.Length==0)
			{
                Utils.MostrarAlerta(Response, "No se encontró el manifiesto.");
				btnSeleccionar.Enabled=false;
				return;
			}
			txtManifiesto.Text=manifiesto;

			txtFecha.Text=Convert.ToDateTime(
				DBFunctions.SingleData(
					"SELECT MLIB_FECHAMANIFIESTO FROM MLIBERACIONCKD "+
					"WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+";")).ToString("yyyy-MM-dd");
			
			txtLevante.Text=DBFunctions.SingleData(
					"SELECT MLIB_LEVANTE FROM MLIBERACIONCKD "+
					"WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+";");
			
			btnSeleccionar.Enabled=true;
		}

		protected void ddlPrefDoc_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lblNumDoc.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefDoc.SelectedValue+"'");
		}
	}
}
