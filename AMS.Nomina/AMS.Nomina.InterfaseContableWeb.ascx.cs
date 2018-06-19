namespace AMS.Nomina
{
    using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Forms;
	using System.Configuration;
	using AMS.Contabilidad;
    using AMS.Tools;
		

	/// <summary>
	///		Descripción breve de AMS_Nomina_InterfaseContableWeb.
	/// </summary>
	public class AMS_Nomina_InterfaseContableWeb : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox txtFechaComprobante;
		protected System.Web.UI.WebControls.TextBox txtNumeroDocumento;
		protected System.Web.UI.WebControls.DropDownList ddlPrefijo;
		protected System.Web.UI.WebControls.TextBox txtRazon;
		protected System.Web.UI.WebControls.Button btnGenerar;
		protected DataTable dtCabeceras ;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.DropDownList DDLQUIN;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DropDownList DDLANO;
		protected System.Web.UI.WebControls.DropDownList DDLMES;
		protected DataTable dtDetalles;
		protected DataTable dtMovs;
		protected System.Web.UI.WebControls.DataGrid dgMovs;
		protected double valorDebito=0,valorCredito=0;
		protected System.Web.UI.WebControls.DropDownList ddlprefProvisiones;
		protected System.Web.UI.WebControls.TextBox txtNumProvisiones;
		protected System.Web.UI.WebControls.DropDownList ddlprefArp;
		protected System.Web.UI.WebControls.TextBox txtNumArp;
		protected System.Web.UI.WebControls.DropDownList ddlprefPara;
		protected System.Web.UI.WebControls.TextBox txtNumPara;
		protected System.Web.UI.WebControls.Button btnGuardar;
		protected System.Web.UI.WebControls.Label lb;
		string susuario= HttpContext.Current.User.Identity.Name.ToLower();
		string nit;
        bool errorImputacion = false;
        double totalDebito , totalCredito ;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls param = new  DatasToControls();
                param.PutDatasIntoDropDownList(DDLANO,     "SELECT PANO_ANO,PANO_DETALLE FROM PANO order by PANO_ANO desc");
                param.PutDatasIntoDropDownList(DDLMES,     "Select pMES_MES, pMES_NOMBRE from pMES order by Pmes_mes");
                param.PutDatasIntoDropDownList(ddlPrefijo, "Select pdoc_codigo,pdoc_codigo concat '-' concat pdoc_descripcion from DBXSCHEMA.PDOCUMENTO p, DBXSCHEMA.CNOMINA C where tdoc_tipodocu='NM' AND P.PDOC_CODIGO = C.PdOC_COMPNOMI");
                param.PutDatasIntoDropDownList(ddlprefProvisiones, "Select pdoc_codigo,pdoc_codigo concat '-' concat pdoc_descripcion from DBXSCHEMA.PDOCUMENTO p, DBXSCHEMA.CNOMINA C where tdoc_tipodocu='NM' AND P.PDOC_CODIGO = C.PdOC_COMPPROV");
                param.PutDatasIntoDropDownList(ddlprefArp, "Select pdoc_codigo,pdoc_codigo concat '-' concat pdoc_descripcion from DBXSCHEMA.PDOCUMENTO p, DBXSCHEMA.CNOMINA C where tdoc_tipodocu='NM' AND P.PDOC_CODIGO = C.PdoC_COMPARL");
                param.PutDatasIntoDropDownList(ddlprefPara,"Select pdoc_codigo,pdoc_codigo concat '-' concat pdoc_descripcion from DBXSCHEMA.PDOCUMENTO p, DBXSCHEMA.CNOMINA C where tdoc_tipodocu='NM' AND P.PDOC_CODIGO = C.PDoC_COMPPARA");
				param.PutDatasIntoDropDownList(DDLQUIN,    "SELECT TPER_PERIODO, TPER_DESCRIPCION FROM TPERIODONOMINA");
                if (ddlPrefijo.Items.Count == 0 || ddlprefProvisiones.Items.Count == 0 || ddlprefArp.Items.Count == 0 || ddlprefPara.Items.Count == 0)
                    Utils.MostrarAlerta(Response, " Falta definir documentos del tipo NOMINA para esta imputación ...!");
                else
                {

                    String NumDocumento = DBFunctions.SingleData("select COALESCE (MAX(MCOM_NUMEDOCU),0) + 1 FROM MCOMPROBANTE WHERE PDOC_CODIGO = '" + ddlPrefijo.SelectedValue + "' ").ToString();

                    String NumProvisiones = DBFunctions.SingleData("select COALESCE (MAX(MCOM_NUMEDOCU),0) + 1 FROM MCOMPROBANTE WHERE PDOC_CODIGO = '" + ddlprefProvisiones.SelectedValue + "' ").ToString();
                    String NumArp = DBFunctions.SingleData("select COALESCE (MAX(MCOM_NUMEDOCU),0) + 1 FROM MCOMPROBANTE WHERE PDOC_CODIGO = '" + ddlprefArp.SelectedValue + "' ").ToString();
                    String NumPara = DBFunctions.SingleData("select COALESCE (MAX(MCOM_NUMEDOCU),0) + 1 FROM MCOMPROBANTE WHERE PDOC_CODIGO = '" + ddlprefPara.SelectedValue + "' ").ToString();
                    
                    

                    // se agregaron los if para que cambie los nuemeros de provisiones, de arp, parafiscales y documentos 


                    txtNumeroDocumento.Text = NumDocumento;
                    txtNumProvisiones.Text = NumProvisiones;
                    txtNumArp.Text = NumArp;
                    txtNumPara.Text = NumPara;

                    if ((NumDocumento != NumProvisiones) && (NumProvisiones == NumArp) && (NumProvisiones == NumPara))
                    {
                                               
                        int i = Convert.ToInt32(NumArp) +1;
                        txtNumArp.Text = i.ToString();
                        int j = Convert.ToInt32( NumPara) +2;
                        txtNumPara.Text = j.ToString();

                    }
                    else if ((NumProvisiones != NumArp) && (NumArp == NumPara) && (NumArp == NumDocumento))
                    {
                        
                        int i = Convert.ToInt32(NumPara) + 1;
                        txtNumArp.Text = i.ToString();
                        int j = Convert.ToInt32(NumDocumento) + 2;
                        txtNumeroDocumento.Text = j.ToString();

                    }
                    else if ((NumArp != NumPara) && (NumPara == NumProvisiones) && (NumPara == NumDocumento))
                    {

                        int i = Convert.ToInt32(NumProvisiones) + 1;
                        txtNumProvisiones.Text = i.ToString();
                        int j = Convert.ToInt32(NumDocumento) + 2;
                        txtNumeroDocumento.Text = j.ToString();

                    }
                    else if (((NumPara != NumDocumento) && (NumDocumento==NumProvisiones) && (NumDocumento==NumArp)))
                    {

                        int i = Convert.ToInt32(NumProvisiones) + 1;
                        txtNumProvisiones.Text = i.ToString();
                        int j = Convert.ToInt32(NumArp) + 2;
                        txtNumArp.Text = j.ToString();

                    }
                
                }
		    }
			else
			{
				if (Session["dtcabeceras"]!=null)
					dtCabeceras=(DataTable)Session["dtcabeceras"];
				if (Session["dtdetalles"]!=null)
					dtDetalles=(DataTable)Session["dtdetalles"];
			}
		}
		
		protected  void Generar(Object sender, EventArgs e)
		{

            string validaPorcentaje = @"select * from (
                                    select m.memp_codiempl, sum(coalesce(mecc_porcentaje,0)) as porcentaje, '' as concepto from mempleado m left join mempleadopcentrocosto mc 
                                    on m.memp_codiempl = mc.memp_codiempl where m.test_estado <> 4 group by m.memp_codiempl 
                                        union
                                    select memp_codiempl, sum(mecc_porcentaje) as porcentaje, pcon_concepto as concepto from mempleadopcentrocostoconcepto group by memp_codiempl, pcon_concepto) as a
                                    where porcentaje <> 100;";
            DataSet errorPorcentaje = new DataSet();
            DBFunctions.Request(errorPorcentaje, IncludeSchema.NO, validaPorcentaje);
            bool errPorc = false;
            int i;
            string detErrPorc = "";
            string concError = "";
            if (errorPorcentaje.Tables[0].Rows.Count != 0)
            {
                for (i = 0; i < errorPorcentaje.Tables[0].Rows.Count; i++)
                {
                    if (errorPorcentaje.Tables[0].Rows[i][2].ToString() == "")
                        concError = "";
                    else 
                        concError = " Concepto " + errorPorcentaje.Tables[0].Rows[i][2].ToString() ;

                    errPorc = true;
                    detErrPorc += "Distribución de porcentaje errada para el empleado " + errorPorcentaje.Tables[0].Rows[i][0].ToString() + " distribución = " + errorPorcentaje.Tables[0].Rows[i][1].ToString() + concError +". Imposible generar contabilización.  Corrijalo  \\n";
                }
            }
        
            if(errPorc)
            {
                Utils.MostrarAlerta(Response, detErrPorc);
                return;
            }

            int comprobantes = Convert.ToInt16(DBFunctions.SingleData(@"SELECT COUNT(*) FROM MCOMPROBANTE WHERE PANO_ANO = " + DDLANO.SelectedValue + @" AND PMES_MES = " + DDLMES.SelectedValue + @" 
                               AND PDOC_CODIGO IN ('" + ddlPrefijo.SelectedValue + "', '" + ddlprefProvisiones.SelectedValue + "', '" + ddlprefArp.SelectedValue + "', '" + ddlprefPara.SelectedValue + "') "));
            if (comprobantes > 0 && DDLQUIN.SelectedValue == "1" || comprobantes > 1 && DDLQUIN.SelectedValue == "2")
                Utils.MostrarAlerta(Response, " ATENCION: Existen " + comprobantes + " comprobante (s) YA CONTABILIZADOS en este período contable. Posible error por doble contabilización");

            DataSet ajustePeso = new DataSet();
            DBFunctions.Request(ajustePeso, IncludeSchema.NO, "select CC.MCUE_CODIPUC_AJUSPESO, CC.PALM_ALMACEN, CC.PCEN_CODIGO, CE.MNIT_NIT FROM CCONTABILIDAD CC, CEMPRESA CE");

            Estructura_TablaCabecera();
			Estructura_TablaDetalles();

            EscribirArchivo (ajustePeso);
            EscribirArchivoProvisiones (ajustePeso);
            EscribirArchivoARL (ajustePeso);
            EscribirArchivoParafiscales (ajustePeso);

            ConstruirGrilla();  

			dgMovs.Visible=true;
            if (!errorImputacion)
                btnGuardar.Visible = true;
            else
                Utils.MostrarAlerta(Response, " Atención, tiene imputaciones mal parametrizadas en Cuenta, Sede o Centro de Costo. Cuando las imputaciones esten completas se activará el botón para Contabilizar");
		}

		protected void GuardarIntefase(Object sender, EventArgs e)
		{
			DataTable dtcabeceras   = new DataTable();
			DataTable dtdetalles    = new DataTable();
			dtcabeceras             = (DataTable)Session["dtcabeceras"];
			dtdetalles              = (DataTable)Session["dtdetalles"];
			InterfaceContable miInterface=new InterfaceContable();
			miInterface.TablaCabecera=dtcabeceras;
			miInterface.TablaDetalles=dtdetalles;
			miInterface.Anio        = Convert.ToInt32(DDLANO.SelectedValue);
			miInterface.Mes         = Convert.ToInt32(DDLMES.SelectedValue);
            string vigenciaInterfase= (DDLANO.SelectedValue).ToString();
            if (miInterface.Mes < 10)
                vigenciaInterfase   = vigenciaInterfase + "0" + DDLMES.SelectedValue.ToString();
            else
                vigenciaInterfase   = vigenciaInterfase + DDLMES.SelectedValue.ToString();
            string vigenciaContable = (DBFunctions.SingleData("select pano_ano concat pmes_mes from dbxschema.ccontabilidad")).ToString();
            if (vigenciaContable.Length == 5)  // si el mes es < 10 solo trae un dígito en el mes y se debe completar a 2 dígitos
                vigenciaContable    = vigenciaContable.Substring(0, 4) + "0" + vigenciaContable.Substring(4, 1);
            if (string.Compare(vigenciaInterfase, vigenciaContable)<0)
            {
                Utils.MostrarAlerta(Response, " ATENCION: Esta tratando de contabilizar el período " + vigenciaInterfase + " YA cerrado en la CONTABILIDAD. Vigencia: " + vigenciaContable + ", Proceso CANCELADO !!! ");
				return;
            }
            miInterface.GuardarInterface();
			lb.Text=miInterface.Mensajes;
            Utils.MostrarAlerta(Response, " " + miInterface.Mensajes + "  !!! ");
            // AQUI se borra la grilla para que el usuario no pueda contabililizarza mas de una vez
            dgMovs.Visible = false;
            btnGuardar.Visible = false;

        }
		
		protected void Estructura_DtMovs()
		{
			dtMovs=new DataTable();
			dtMovs.Columns.Add("PREFIJO",typeof(string));//0
			dtMovs.Columns.Add("NUMERO",typeof(string));//1
			dtMovs.Columns.Add("AÑO",typeof(string));//2
			dtMovs.Columns.Add("MES",typeof(string));//3
			dtMovs.Columns.Add("FECHA DOC",typeof(string));//4
			dtMovs.Columns.Add("RAZON",typeof(string));//5
			dtMovs.Columns.Add("PROCESADO",typeof(string));//6
			dtMovs.Columns.Add("USUARIO",typeof(string));//7
			dtMovs.Columns.Add("VALOR",typeof(string));//8
		}

		protected void ConstruirHeader(string prefijo,string numero,string anio,string mes,string fecha,string razon,string procesado,string usuario,string valor)
		{
			DataRow fila,filaTitulos;
			fila=dtMovs.NewRow();
			filaTitulos=dtMovs.NewRow();
			fila[0]=prefijo;
			fila[1]=numero;
			fila[2]=anio;
			fila[3]=mes;
			fila[4]=fecha;
			fila[5]=razon;
			fila[6]=procesado;
			fila[7]=usuario;
			fila[8]=valor;
			filaTitulos[0]="<p style=\"COLOR: navy\">Cuenta</p>";
			filaTitulos[1]="<p style=\"COLOR: navy\">Referencia Pref-Num</p>";
			filaTitulos[2]="<p style=\"COLOR: navy\">Nit</p>";
			filaTitulos[3]="<p style=\"COLOR: navy\">Almacen</p>";
			filaTitulos[4]="<p style=\"COLOR: navy\">Centro Costo</p>";
			filaTitulos[5]="<p style=\"COLOR: navy\">Razón</p>";
			filaTitulos[6]="<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Débito</p>";
			filaTitulos[7]="<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Crédito</p>";
			filaTitulos[8]="<p style=\"COLOR: navy; TEXT-ALIGN: right\">Valor Base</p>";
			dtMovs.Rows.Add(fila);
			dtMovs.Rows.Add(filaTitulos);
		}
		
		protected void Ingresar_Datos_Cabecera(string prefijo,string numero,string ano,string mes,string fecha,string razon,string procesado,string usuario,string valor)
		{
			DataRow fila    = dtCabeceras.NewRow();
			fila["PREFIJO"] = prefijo;
			fila["NUMERO"]  = numero;
			fila["ANO"]     = ano;
			fila["MES"]     = mes;
			fila["FECHA"]   = fecha;
			fila["RAZON"]   = razon;
			fila["PROCESADO"]=procesado;
			fila["USUARIO"] = usuario;
			fila["VALOR"]   = Convert.ToDouble(valor).ToString("C");
			dtCabeceras.Rows.Add(fila);
			Session["dtcabeceras"]=dtCabeceras;
			
			//this.dgInterfase.DataSource=tablaCabecera;
			//dgInterfase.DataBind();
			//DatasToControls.Aplicar_Formato_Grilla(dgInterfase);
			//DatasToControls.JustificacionGrilla(dgInterfase,tablaCabecera);
		}

		private void Estructura_TablaCabecera()
		{
			dtCabeceras=new DataTable();
			dtCabeceras.Columns.Add("PREFIJO",typeof(string));
			dtCabeceras.Columns.Add("NUMERO",typeof(string));
			dtCabeceras.Columns.Add("ANO",typeof(string));
			dtCabeceras.Columns.Add("MES",typeof(string));
			dtCabeceras.Columns.Add("FECHA",typeof(string));
			dtCabeceras.Columns.Add("RAZON",typeof(string));
			dtCabeceras.Columns.Add("PROCESADO",typeof(string));
			dtCabeceras.Columns.Add("USUARIO",typeof(string));
			dtCabeceras.Columns.Add("VALOR",typeof(string));
		}

        protected void Ingresar_Datos_Detalles(string prefijo, string numero, string cuenta, string prefijoref, string numeroref, string nit, string almacen, string centrocosto, string razon, string debito, string credito, string based)
        {
            DataRow fila = dtDetalles.NewRow();
            fila["PREFIJO"] = prefijo;
            fila["NUMERO"] = numero;
            fila["CUENTA"] = cuenta;
            fila["PREFIJOREF"] = prefijoref;
            fila["NUMEROREF"] = numeroref;
            fila["NIT"] = nit;
            fila["ALMACEN"] = almacen;
            fila["CENTROCOSTO"] = centrocosto;
            fila["RAZON"] = razon;
            //fila["DEBITO"]=Math.Round(Convert.ToDouble(debito),0).ToString("C");
            fila["DEBITO"] = Convert.ToDouble(debito).ToString("C");
            fila["CREDITO"] = Convert.ToDouble(credito).ToString("C");
            try { fila["BASE"] = Convert.ToDouble(based).ToString("C"); }
            catch { fila["BASE"] = based + "EEERRROORR"; }
            if (Convert.ToDouble(debito).ToString("C") != "$0.00" || Convert.ToDouble(credito).ToString("C") != "$0.00")
            { 
                dtDetalles.Rows.Add(fila);
                Session["dtdetalles"] = dtDetalles;
        }
			//this.dgInterfase.DataSource=tablaDetalles;
			//dgInterfase.DataBind();
			//DatasToControls.Aplicar_Formato_Grilla(dgInterfase);
			//DatasToControls.JustificacionGrilla(dgInterfase,tablaDetalles);
		}
				
		private void Estructura_TablaDetalles()
		{
			dtDetalles=new DataTable();
			dtDetalles.Columns.Add("PREFIJO",typeof(string));
			dtDetalles.Columns.Add("NUMERO",typeof(string));
			dtDetalles.Columns.Add("CUENTA",typeof(string));
			dtDetalles.Columns.Add("PREFIJOREF",typeof(string));
			dtDetalles.Columns.Add("NUMEROREF",typeof(string));
			dtDetalles.Columns.Add("NIT",typeof(string));
			dtDetalles.Columns.Add("ALMACEN",typeof(string));
			dtDetalles.Columns.Add("CENTROCOSTO",typeof(string));
			dtDetalles.Columns.Add("RAZON",typeof(string));
			dtDetalles.Columns.Add("DEBITO",typeof(string));
			dtDetalles.Columns.Add("CREDITO",typeof(string));
			dtDetalles.Columns.Add("BASE",typeof(string));
		}

		protected void ConstruirGrilla()
		{
			bool deb=false,cre=false;
			DatasToControls.Aplicar_Formato_Grilla(dgMovs);
			Estructura_DtMovs();
			for(int i=0;i<dtCabeceras.Rows.Count;i++)
			{
				DataRow[] hijos = dtDetalles.Select("PREFIJO='"+dtCabeceras.Rows[i][0].ToString()+"' AND NUMERO='"+dtCabeceras.Rows[i][1].ToString()+"'");
				valorCredito = valorDebito = 0;
				if(hijos.Length!=0)
				{
					ConstruirHeader(dtCabeceras.Rows[i][0].ToString(),dtCabeceras.Rows[i][1].ToString(),dtCabeceras.Rows[i][2].ToString(),dtCabeceras.Rows[i][3].ToString(),dtCabeceras.Rows[i][4].ToString(),dtCabeceras.Rows[i][5].ToString(),dtCabeceras.Rows[i][6].ToString(),dtCabeceras.Rows[i][7].ToString(),dtCabeceras.Rows[i][8].ToString());
					for(int j=0;j<hijos.Length;j++)
					{
						deb=cre=false;
						if(hijos[j][9].ToString()!=string.Empty)
						{
							try
							{
								valorDebito=valorDebito+Convert.ToDouble(hijos[j][9].ToString().Substring(1));
								deb=true;
							}
							catch{valorDebito=valorDebito+0;}
						}
						if(hijos[j][10].ToString()!=string.Empty)
						{
							try
							{
								valorCredito=valorCredito+Convert.ToDouble(hijos[j][10].ToString().Substring(1));
								cre=true;
							}
							catch{valorCredito=valorCredito+0;}
						}
						if(deb && cre)
						{
							if(Convert.ToDouble(hijos[j][9].ToString().Substring(1))!=0 || Convert.ToDouble(hijos[j][10].ToString().Substring(1))!=0)
								ConstruirItems(hijos[j][2].ToString(),hijos[j][3].ToString(),hijos[j][4].ToString(),hijos[j][5].ToString(),hijos[j][6].ToString(),hijos[j][7].ToString(),hijos[j][8].ToString(),hijos[j][9].ToString(),hijos[j][10].ToString(),hijos[j][11].ToString());
						}
						else 
							ConstruirItems(hijos[j][2].ToString(),hijos[j][3].ToString(),hijos[j][4].ToString(),hijos[j][5].ToString(),hijos[j][6].ToString(),hijos[j][7].ToString(),hijos[j][8].ToString(),hijos[j][9].ToString(),hijos[j][10].ToString(),hijos[j][11].ToString());
					}
					ConstruirTotalComprobante();
				}
			}
			dgMovs.DataSource = dtMovs;
			dgMovs.DataBind();
			//if(hdnCont.Value=="S")
			//CmdContabilizar.Visible=true;
		}
				
		protected void ConstruirItems(string cuenta,string prefijoRef,string numeroRef,string nit,string almacen,string centroCosto,string razon,string debito,string credito,string valorBase)
		{
			DataRow fila;
			fila=dtMovs.NewRow();
            if (cuenta.IndexOf("ERROR") != -1 || almacen.IndexOf("ERROR") != -1 || centroCosto.IndexOf("ERROR") != -1)
                errorImputacion = true;  // se valida que las cuentas, sedes y centros de costro este bien parametrizadas
			fila[0]=cuenta;
			fila[1]=prefijoRef+"-"+numeroRef;
			fila[2]=nit;
			fila[3]=almacen;
			fila[4]=centroCosto;
			fila[5]=razon;
			fila[6]="<p style=\"TEXT-ALIGN: right\">"+debito+"</p>";
			fila[7]="<p style=\"TEXT-ALIGN: right\">"+credito+"</p>";
			fila[8]="<p style=\"TEXT-ALIGN: right\">"+valorBase+"</p>";
			dtMovs.Rows.Add(fila);
		}
			
		protected void ConstruirTotalComprobante()
		{
			DataRow fila,filaIntermedio;
			fila=dtMovs.NewRow();
			filaIntermedio=dtMovs.NewRow();
			fila[0]="<p style=\"COLOR: navy\">Total Comprobante</p>";
			if (Math.Round(valorDebito,2) != Math.Round(valorCredito,2))
				fila[5]="<p style=\"COLOR: red\">Comprobante Descuadrado</p>";
			else
			{
				fila[5]="<p style=\"COLOR: green\">Sumas Iguales</p>";
				//hdnCont.Value="S";
			}
			fila[6]="<p style=\"TEXT-ALIGN: right\">"+valorDebito.ToString("C")+"</p>";
			fila[7]="<p style=\"TEXT-ALIGN: right\">"+valorCredito.ToString("C")+"</p>";
			dtMovs.Rows.Add(fila);
			dtMovs.Rows.Add(filaIntermedio);
		}
		
		protected void EscribirArchivo(DataSet ajustePeso)  // CONTABILIZACION COMPROBANTE DE PAGO NOMINA
		{
			int i=0;
			double sumapagado=0,sumadescontado=0;
			double sumatotalcredito = 0;
			double sumatotaldebito  = 0;
			string codempleado      = "";
			string nombconcepto     = "";
			string nombreEntidad    = "";
            bool   nitxEmpresa      = false;
            bool   distxConcepto    = false;
            try
            {
               if(DBFunctions.SingleData("SELECT CNOM_NITPARAFISCEMPE FROM CNOMINA" ) == "S" )  
             //   if (ConfigurationManager.AppSettings["ImputaNitParafiscalesEmpresa"] == "SI") // del web.config se valida que nit imputa en los parafiscales (salud y pension)
                nitxEmpresa = true;
            }
			catch
            {
                nitxEmpresa = false;
                Utils.MostrarAlerta(Response, " NO HA definido el campo CNOM_NITPARAFISCEMPE en configuración de nómina");
            };	
			int x,y,h;
            totalDebito = totalCredito = 0;
			//averiguar el numero de quincena escogido
            string numQuincena  = DBFunctions.SingleData("select  mqui_codiquin from DBXSCHEMA.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi=" + DDLQUIN.SelectedValue + " and mqui_anoquin=" + DDLANO.SelectedValue + ";");
			
			DataSet conceptos= new DataSet();
			DBFunctions.Request(conceptos,IncludeSchema.NO,"select cnom_concrftecodi,cnom_concepscodi,cnom_concfondcodi,cnom_concfondsolipens,CNOM_CONCFONDPENSVOLU from dbxschema.cnomina ");
            string cuentaPUC = DBFunctions.SingleData("SELECT coalesce(CNOM_CUENTAPUC,'* ERROR CUENTA PUC') FROM DBXSCHEMA.CNOMINA");
            string cuentaBCO = DBFunctions.SingleData("SELECT coalesce(mcue_Codipuc,'* ERROR CUENTA PUC')   FROM DBXSCHEMA.CNOMINA, DBXSCHEMA.pcuentacorriente pc where cnom_numclientebanco = pcue_codigo");
            string cuentaContable = "";
            double valorcentrocosto = 0;
			
			DataSet empleados   = new DataSet();
            string sqlEmpleados =
                   @"select memp.memp_codiempl,memp.peps_codieps,peps.peps_nombeps,peps.mnit_nit,MEMP.pfon_codipens,PFP.pfon_nombpens,PFP.mnit_nit,memp.pfon_codipensvolu,  
                   PFP2.pfon_nombpens,PFP2.mnit_nit, mn.mnit_APELLIDOS CONCAT ' ' CONCAT MN.MNIT_NOMBRES, memp.mnit_nit as nitEmpleado, 
                   COALESCE(PEPS_MCUENTA,'* ERROR CUENTA PUC'), 
                   COALESCE(PFP.MCUE_CODIPUC,'* ERROR CUENTA PUC'),  
                   COALESCE(PFP2.MCUE_codipuc,'* ERROR CUENTA PUC'),
                   memp.palm_almacen,
                   memp.memp_formpago,
                   COALESCE(pcen_codigo,mecc_porcentaje,100) AS pcen_codigo, mecc_porcentaje, memp.MNIT_NIT AS NIT_EMPLEADO 
                   FROM mnit mn, dbxschema.mempleado memp  
				    left join dbxschema.peps peps on memp.peps_codieps=peps.peps_codieps
				    left join dbxschema.pfondopension PFP on PFP.pfon_codipens=MEMP.pfon_codipens
                    LEFT JOIN dbxschema.pfondopension PFP2 ON PFP2.pfon_codipens=memp.pfon_codipensvolu  
                    left join dbxschema.mempleadopcentrocosto mcc on mcc.memp_codiempl = memp.memp_codiempl and mecc_porcentaje = (select max(mecc_porcentaje) from dbxschema.mempleadopcentrocosto mcc where mcc.memp_codiempl = memp.memp_codiempl )
                  WHERE MEMP.mnit_nit = mn.mnit_nit  
                  ORDER BY memp.mnit_nit ";
            DBFunctions.Request(empleados, IncludeSchema.NO, sqlEmpleados);
         
			if (empleados.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					sumapagado=0;
					sumadescontado=0;
					//Averiguar el centro de costo con mayor porcentaje
                    string centCosto = DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='" + empleados.Tables[0].Rows[i][0].ToString() + "') AND MEMP_CODIEMPL='" + empleados.Tables[0].Rows[i][0].ToString() + "'  FETCH FIRST 1 ROWS ONLY");
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
				    string cuentas =
                        @"select DQUI.memp_codiempl,DQUI.mqui_codiquin,DQUI.pcon_concepto,DQUI.dqui_apagar,DQUI.dqui_adescontar,  
                                 DQUI.dqui_saldo,DQUI.dqui_docrefe,coalesce(PPUC.MCUE_CODIPUC,'* ERROR CTA CONCEPTO'), MEMP.PDEP_CODIDPTO,coalesce(MCUE.TCLA_CODIGO,'D'), pcon_nombconc
                         from dbxschema.dquincena DQUI  
                              left join dbxschema.mempleado memp on DQUI.memp_codiempl = memp.memp_codiempl  
                              left join dbxschema.ppucconceptonomina PPUC on DQUI.pcon_concepto = PPUC.pcon_concepto and PPUC.PDEP_CODIDPTO = memp.PDEP_CODIDPTO 
                              left join dbxschema.mcuenta mcue on PPUC.MCUE_CODIPUC = mcue.MCUE_CODIPUC 
                              left join DBXSCHEMA.PCONCEPTONOMINA pcn on pcn.pcon_concepto = DQUI.pcon_concepto 
                              where DQUI.mqui_codiquin=" + numQuincena + @" and dqui.memp_codiempl='" + empleados.Tables[0].Rows[i][0].ToString() + "'; ";

                    DBFunctions.Request(numcuentas, IncludeSchema.NO, cuentas);
                    string ccDefault = empleados.Tables[0].Rows[i]["PCEN_CODIGO"].ToString();   // DBFunctions.SingleData("select pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"' and mecc_porcentaje = (select max(mecc_porcentaje) from dbxschema.mempleadopcentrocosto where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"')");
					codempleado      = empleados.Tables[0].Rows[i][0].ToString();
					 
                    string nombreFondoPension = empleados.Tables[0].Rows[i][5].ToString();
					string cuentPucEps        = empleados.Tables[0].Rows[i][12].ToString();
                    string cuentPucPension    = empleados.Tables[0].Rows[i][13].ToString();
                    string cuentPucPensionVoluntaria = empleados.Tables[0].Rows[i][14].ToString();
                    string nombreEps          = empleados.Tables[0].Rows[i][2].ToString();
					nombreEps                 = "("+nombreEps+")";
                    nombreFondoPension        = "(" + nombreFondoPension + ")";
                    string nitEmpleado        = empleados.Tables[0].Rows[i][11].ToString();

                    if (numcuentas.Tables.Count > 0)
                    {
                      if(numcuentas.Tables[0].Rows.Count != 0 && numcuentas.Tables[0].Rows.Count > 0)
					  {
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
                            if (Convert.ToDouble(numcuentas.Tables[0].Rows[x][3].ToString()) < 0)
                            {
                                numcuentas.Tables[0].Rows[x][4] = Convert.ToDouble(numcuentas.Tables[0].Rows[x][3])*-1;
                                numcuentas.Tables[0].Rows[x][3] = "0";
                            }
                            if (Convert.ToDouble(numcuentas.Tables[0].Rows[x][4].ToString()) < 0)
                            {
                                numcuentas.Tables[0].Rows[x][3] = Convert.ToDouble(numcuentas.Tables[0].Rows[x][4])*-1;
                                numcuentas.Tables[0].Rows[x][4] = "0";
                            }
							 
                            //  PROCESO DE PORCENTAJES.
							if (numcuentas.Tables[0].Rows[x][9].ToString()=="N")
							{
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
                                DBFunctions.Request(porcentajes, IncludeSchema.NO,
                                    @"select ME.memp_codiempl, COALESCE(MC.palm_almacen,'ERROR SEDE'), COALESCE(MC.pcen_codigo,'ERROR CCOSTO'), COALESCE(MC.mecc_porcentaje,100),'' as pcon_concepto 
                                        from dbxschema.mempleado ME LEFT JOIN dbxschema.mempleadopcentrocosto MC ON  ME.memp_codiempl = MC.memp_codiempl 
                                        WHERE ME.memp_codiempl='" + numcuentas.Tables[0].Rows[x][0].ToString() + @"'
                                      UNION
                                      select ME.memp_codiempl, COALESCE(Me.palm_almacen,'ERROR SEDE'), COALESCE(MC.pcen_codigo,'ERROR CCOSTO'), COALESCE(MC.mecc_porcentaje,100), pcon_concepto 
                                        from dbxschema.mempleado ME, dbxschema.mempleadopcentrocostoconcepto  MC
                                       WHERE ME.memp_codiempl = MC.memp_codiempl and ME.memp_codiempl='" + numcuentas.Tables[0].Rows[x][0].ToString() + "' ");
                                if (porcentajes.Tables[0].Rows[0][2].ToString() == "ERROR CCOSTO" || porcentajes.Tables[0].Rows[0][1].ToString() == "ERROR SEDE" || porcentajes.Tables[0].Rows[0][1].ToString() == "")
								{
                                    Utils.MostrarAlerta(Response, " ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][0].ToString() + " NO se le han definido porcentajes. Interfase Nomina ERRADA... ");
                                    errorImputacion = true;

                                }
							
								//debito
								if (double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())!=0) // a pagar
								{
									for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
									{
										nombreEntidad="";
                                        valorcentrocosto = 0;
                                        distxConcepto = false;
                                        for (h = 0; h < porcentajes.Tables[0].Rows.Count; h++)
                                        {
                                            if (numcuentas.Tables[0].Rows[x][2].ToString() == porcentajes.Tables[0].Rows[h][4].ToString())
                                                distxConcepto = true;
                                        }
                                        if(numcuentas.Tables[0].Rows[x][2].ToString() == porcentajes.Tables[0].Rows[y][4].ToString() && distxConcepto) // Si aplica distribución por concepto específico
                                            valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][3].ToString())/100;
                                        else if (porcentajes.Tables[0].Rows[y][4].ToString() == "" && !distxConcepto)
                                                valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][3].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);					
								 
                                        //validar el nit para eps,fpen,fpensvol
										//1.EPs
										if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
										{
                                            if (!nitxEmpresa)
                                                nit = nitEmpleado;
                                            else
                                                nit = empleados.Tables[0].Rows[i][3].ToString();
											nombreEntidad=nombreEps;
										}//2.Fpens
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
										{
                                            if (!nitxEmpresa)
                                                nit = nitEmpleado;
                                            else
                                                nit = empleados.Tables[0].Rows[i][6].ToString();
											nombreEntidad=nombreFondoPension;
										}//3.FpensVolu
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
										{
                                            if (!nitxEmpresa)
                                                nit = nitEmpleado;
                                            else
                                                nit = empleados.Tables[0].Rows[i][9].ToString();
										}//4. Fsolipens
										else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
										{
                                            if (!nitxEmpresa)
                                                nit = nitEmpleado;
                                            else
                                                nit = empleados.Tables[0].Rows[i][6].ToString();
                                            nombreEntidad = nombreFondoPension;
										}
										else
										{
											//nit                                empleado                                            concepto                              valor
											nitConcepto (numcuentas.Tables[0].Rows[x][0].ToString(), numcuentas.Tables[0].Rows[x][2].ToString(), Convert.ToDouble(numcuentas.Tables[0].Rows[x][4]));
											nombreEntidad="";
										}
										nombconcepto = numcuentas.Tables[0].Rows[x]["PCON_NOMBCONC"].ToString();  //DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
										//outputDatosInterfase+=comparacion.Completar_Campos( nombconcepto+nombreEntidad+txtVigencia.Text,30," ",true);										
										string VrBase = "0";
										if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString() )
										{
											VrBase = DBFunctions.SingleData("select COALESCE(sum(dqui_Apagar - dqui_adescontar),0) from dbxschema.Dquincena d, dbxschema.pconceptonomina p where mqui_codiquin=" + numQuincena + " and memp_codiempl='" + numcuentas.Tables[0].Rows[x][0].ToString() + "' and d.pcon_concepto = p.pcon_concepto and p.tres_afecretefte = 'S'");
							            }
                                        string almacen  = porcentajes.Tables[0].Rows[y][1].ToString();
                                        string cCosto   = porcentajes.Tables[0].Rows[y][2].ToString();  
										string razon    = nombconcepto+' '+nombreEntidad+DDLANO.SelectedValue+DDLMES.SelectedValue+DDLQUIN.SelectedValue;
                                        razon = razon + " " + empleados.Tables[0].Rows[i][10].ToString();

										if(valorcentrocosto != 0)
                                            this.Ingresar_Datos_Detalles(ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,numcuentas.Tables[0].Rows[x][7].ToString(),ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,nit,almacen,cCosto,razon,valorcentrocosto.ToString(),"0",VrBase);
									
                                        totalDebito += valorcentrocosto;
										sumapagado=sumapagado+valorcentrocosto;
										sumatotaldebito+=valorcentrocosto;
									}
								}
                                else  // credito
								{
									for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
									{
										nombreEntidad="";
									    valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][3].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);					
										//validar el nit para eps,fpen,fpensvol
										//1.EPs
										if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
										{
											if(!nitxEmpresa)
                                                nit = empleados.Tables[0].Rows[i][11].ToString();
                                            else
                                                nit = empleados.Tables[0].Rows[i][3].ToString();
											nombreEntidad=nombreEps;
										}//2.Fpens
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
										{
                                            if (!nitxEmpresa) 
                                                nit = empleados.Tables[0].Rows[i][11].ToString();
                                            else
                                                nit = empleados.Tables[0].Rows[i][6].ToString();
											nombreEntidad=nombreFondoPension;
										}//3.FpensVolu
										else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
										{
                                            if (!nitxEmpresa) 
                                                nit = empleados.Tables[0].Rows[i][11].ToString();
                                            else
                                                nit = empleados.Tables[0].Rows[i][9].ToString();
										}//4. Fsolipens
										else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
										{
                                            if (!nitxEmpresa) 
                                                nit = empleados.Tables[0].Rows[i][11].ToString();
                                            else
                                                nit = empleados.Tables[0].Rows[i][6].ToString();
                                            nombreEntidad = nombreFondoPension;
										}
										else
										{
											//nit                                empleado                                            concepto                              valor
											nitConcepto (numcuentas.Tables[0].Rows[x][0].ToString(), numcuentas.Tables[0].Rows[x][2].ToString(), Convert.ToDouble(numcuentas.Tables[0].Rows[x][4]));
											nombreEntidad="";
										}
                                            nombconcepto = numcuentas.Tables[0].Rows[x]["PCON_NOMBCONC"].ToString(); // DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
										string  VrBase="0";
										//validar RETFTE
										if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString() )
										{
											VrBase = DBFunctions.SingleData("select COALESCE(sum(dqui_Apagar - dqui_adescontar),0) from dbxschema.Dquincena d, dbxschema.pconceptonomina p where mqui_codiquin=" + numQuincena + " and memp_codiempl='" + numcuentas.Tables[0].Rows[x][0].ToString() + "' and d.pcon_concepto = p.pcon_concepto and p.tres_afecretefte = 'S'");
							            }

                                        string almacen = porcentajes.Tables[0].Rows[y][1].ToString();
                                        string cCosto  = porcentajes.Tables[0].Rows[y][2].ToString();  
										string razon   = nombconcepto+' '+nombreEntidad+DDLANO.SelectedValue+DDLMES.SelectedValue;
                                        razon = razon + " " + empleados.Tables[0].Rows[i][10].ToString();

										this.Ingresar_Datos_Detalles(ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,numcuentas.Tables[0].Rows[x][7].ToString(),ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,nit,almacen,cCosto,razon,"0",valorcentrocosto.ToString(),VrBase);

                                        totalCredito += valorcentrocosto;
                                        sumadescontado  =sumadescontado-valorcentrocosto;
										sumatotalcredito=sumatotalcredito+(valorcentrocosto*-1);
									}
								}
							}
							else
							{
								//PROCESO SIN PORCENTAJES
								//debito
								if (double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())!=0)
								{
									nombreEntidad="";								
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
									valorcentrocosto=Math.Round(valorcentrocosto,2);					
									//validar el nit para eps,fpen,fpensvol
									//1.EPs
									if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
									{
										nit=empleados.Tables[0].Rows[i][3].ToString();
										nombreEntidad=nombreEps;
									}//2.Fpens
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
									{
										nit=empleados.Tables[0].Rows[i][6].ToString();
										nombreEntidad=nombreFondoPension;
									}//3.FpensVolu
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
									{
										nit=empleados.Tables[0].Rows[i][9].ToString();
									}//4. Fsolipens
									else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
									{
										nit=empleados.Tables[0].Rows[i][6].ToString();
                                        nombreEntidad = nombreFondoPension;
									}
									else
									{
										//nit                                empleado                                            concepto                              valor
										nitConcepto (numcuentas.Tables[0].Rows[x][0].ToString(), numcuentas.Tables[0].Rows[x][2].ToString(), Convert.ToDouble(numcuentas.Tables[0].Rows[x][4]));
										nombreEntidad="";
									}
									nombconcepto = numcuentas.Tables[0].Rows[x]["PCON_NOMBCONC"].ToString();  //DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
									//validar RETFTE
									string VrBase="0";
									if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString() )
									{
										VrBase = DBFunctions.SingleData("select COALESCE(sum(dqui_Apagar - dqui_adescontar),0) from dbxschema.Dquincena d, dbxschema.pconceptonomina p where mqui_codiquin=" + numQuincena + " and memp_codiempl='" + numcuentas.Tables[0].Rows[x][0].ToString() + "' and d.pcon_concepto = p.pcon_concepto and p.tres_afecretefte = 'S'");
							        }
                                    string almacen = empleados.Tables[0].Rows[i][15].ToString();  
									string razon =nombconcepto+' '+nombreEntidad+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][10].ToString();

									this.Ingresar_Datos_Detalles(ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,numcuentas.Tables[0].Rows[x][7].ToString(),ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,nit,almacen,ccDefault,razon,valorcentrocosto.ToString(),"0",VrBase);

                                    totalDebito += valorcentrocosto;
                                    sumapagado =sumapagado+valorcentrocosto;
									sumatotaldebito+=valorcentrocosto;
								}
									//es credito
								else
								{
									string cuentaPUCef = numcuentas.Tables[0].Rows[x][7].ToString();
									nombreEntidad="";						
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString());
									valorcentrocosto=Math.Round(valorcentrocosto,2);					
									//validar el nit para eps,fpen,fpensvol
									//1.EPs
									if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][1].ToString())
									{
                                        if (!nitxEmpresa) 
                                            nit = empleados.Tables[0].Rows[i][11].ToString();
                                        else
                                            nit = empleados.Tables[0].Rows[i][3].ToString();
                                        if (empleados.Tables[0].Rows[i][12].ToString().Trim() != "")
                                            cuentaPUCef = empleados.Tables[0].Rows[i][12].ToString();
                                   		nombreEntidad=nombreEps;
									}//2.Fpens
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][2].ToString())
									{
                                        if (!nitxEmpresa) 
                                            nit=empleados.Tables[0].Rows[i][11].ToString();
                                        else
                                            nit = empleados.Tables[0].Rows[i][6].ToString();
                                        if (empleados.Tables[0].Rows[i][13].ToString().Trim() != "")
                                            cuentaPUCef = empleados.Tables[0].Rows[i][13].ToString();
                                      	nombreEntidad=nombreFondoPension;
									}//3.FpensVolu
									else if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][4].ToString())
									{
                                        if (!nitxEmpresa) 
                                            nit=empleados.Tables[0].Rows[i][11].ToString();
                                        else
                                           nit = empleados.Tables[0].Rows[i][9].ToString();
                                        if (empleados.Tables[0].Rows[i][14].ToString().Trim() != "")
                                            cuentaPUCef = empleados.Tables[0].Rows[i][14].ToString();
                                        nombreEntidad = empleados.Tables[0].Rows[i][8].ToString();
									}//4. Fsolipens
									else if(numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][3].ToString())
									{
                                        if (!nitxEmpresa) 
                                            nit = empleados.Tables[0].Rows[i][11].ToString();
                                        else
                                            nit = empleados.Tables[0].Rows[i][6].ToString();
                                        if (nitxEmpresa && empleados.Tables[0].Rows[i][13].ToString().Trim() != "")
                                              cuentaPUCef = empleados.Tables[0].Rows[i][13].ToString();
                                        nombreEntidad = nombreFondoPension;
									}
							 		else
									{
										//nit                                empleado                                            concepto                              valor
										nitConcepto (numcuentas.Tables[0].Rows[x][0].ToString(), numcuentas.Tables[0].Rows[x][2].ToString(), Convert.ToDouble(numcuentas.Tables[0].Rows[x][4]));
										nombreEntidad="";
									}
                                    nombconcepto = numcuentas.Tables[0].Rows[x]["PCON_NOMBCONC"].ToString();  //DBFunctions.SingleData("Select pcon_nombconc from DBXSCHEMA.PCONCEPTONOMINA where pcon_concepto='"+numcuentas.Tables[0].Rows[x][2].ToString()+"'");
                                    string VrBase="0";
									//validar RETFTE
									if (numcuentas.Tables[0].Rows[x][2].ToString()==conceptos.Tables[0].Rows[0][0].ToString())
									{
                                        VrBase = DBFunctions.SingleData("select COALESCE(sum(dqui_Apagar - dqui_adescontar),0) from dbxschema.Dquincena d, dbxschema.pconceptonomina p where mqui_codiquin=" + numQuincena + " and memp_codiempl='" + numcuentas.Tables[0].Rows[x][0].ToString() + "' and d.pcon_concepto = p.pcon_concepto and p.tres_afecretefte = 'S'");
									}
                                    string almacen = empleados.Tables[0].Rows[i][15].ToString(); 
									string razon   = nombconcepto+' '+nombreEntidad+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][10].ToString();

								 	this.Ingresar_Datos_Detalles(ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,cuentaPUCef,ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,nit,almacen,ccDefault,razon,"0",valorcentrocosto.ToString(),VrBase);

                                    totalCredito += valorcentrocosto;
                                    sumadescontado =sumadescontado-valorcentrocosto;
									sumatotalcredito=sumatotalcredito+(valorcentrocosto*-1);
								}
							}
						}//cuentas
						double ajuste=sumapagado+sumadescontado;
                        //nit
                        nit = empleados.Tables[0].Rows[i]["NIT_EMPLEADO"].ToString();  // DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+codempleado+"'");
						if (ajuste>0)
						{
							sumatotalcredito=sumatotalcredito-ajuste;
						}
						else
						{
							sumatotaldebito=sumatotaldebito+ajuste;
						}
                        string almacenL = empleados.Tables[0].Rows[i]["PALM_ALMACEN"].ToString();  // DBFunctions.SingleData("select palm_almacen from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"'"); 
						string razonL = "Liquidación Nómina"+DDLANO.SelectedValue+DDLMES.SelectedValue;
						razonL = razonL+" "+empleados.Tables[0].Rows[i][10].ToString();

                        // aqui se imputa la cuenta del banco a quienes se les consinga o la ctapuc de nomina por pagar a los que no se les consigna
                        if (empleados.Tables[0].Rows[i][16].ToString() == "2" && cuentaBCO != "" && DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA") == "800202170") // FUJIYAMA DE CARTAGENA
                            cuentaContable = cuentaBCO;   // se le consigna en el banco
                        else
                            cuentaContable = cuentaPUC;

                        nit = empleados.Tables[0].Rows[i]["NIT_EMPLEADO"].ToString();
                        this.Ingresar_Datos_Detalles(ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,cuentaContable,ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,nit,almacenL,ccDefault,razonL,"0",ajuste.ToString(),"0");
                        totalCredito += ajuste;
                     }
                 }   // regCuentas
				
				}//empl
                 /*
                     if((Math.Abs(sumatotalcredito))==sumatotaldebito)
                     {
                         Response.Write("<script language:javascript>alert('El comprobante fue generado correctamente,Sumas iguales "+sumatotaldebito+"');</script>");

                     }
                     else
                     {
                         Response.Write("<script language:javascript>alert('El comprobante es incorrecto no tiene sumas iguales Debito:"+sumatotaldebito+" Credito "+sumatotalcredito+"');</script>");
                     }
                 */
                totalDebito -= totalCredito;
                if (totalDebito != 0)
                    this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text, ajustePeso.Tables[0].Rows[0][0].ToString(), ddlprefPara.SelectedValue, txtNumPara.Text, nit, ajustePeso.Tables[0].Rows[0][1].ToString(), ajustePeso.Tables[0].Rows[0][2].ToString(), "Ajuste al Peso", totalDebito.ToString(), "0", "0");

                this.Ingresar_Datos_Cabecera(ddlPrefijo.SelectedValue, txtNumeroDocumento.Text,DDLANO.SelectedValue,DDLMES.SelectedValue,txtFechaComprobante.Text,this.txtRazon.Text,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),susuario,Math.Round(sumatotaldebito,0).ToString());
		
			}//if
		}

		protected void nitConcepto (string empleado, string concepto, double valor)
		{
        //    string nit = "";
            nit = DBFunctions.SingleData("select DISTINCT mnit_nitentibene from mpagosydtosper m where m.pcon_CONcepto = '"+concepto+"' and m.memp_codiempl = '"+empleado+"' and m.mpag_valor = "+valor+"; ");
			if  (nit == null || nit == "")
			{
				nit = DBFunctions.SingleData("select DISTINCT mnit_nitentibene from mprestamoempleados m where m.pcon_CONcepto = '"+concepto+"' and m.memp_codiempl = '"+empleado+"' and m.mpre_valorpres / m.mpre_numecuot = "+valor+" and m.mpre_numecuot > 0; ");
				if  (nit == null || nit == "")	// se debe optimizar devolviendo el nit del empleado sin ir a la DB
					nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl = '"+empleado+"'; ");
			}
	 	// return nit;
		}

		protected void EscribirArchivoProvisiones(DataSet ajustePeso)
		{
            if (DDLQUIN.SelectedValue == "1")
                return;   // solo se contabiliza la segunda quincena
            int i=0;
			double sumatotaldebito=0;
			string codempleado="";
			string nit;
			int x,y;
			double valorcentrocosto=0;
            totalDebito = totalCredito = 0;
			
		//	string numQuincena=DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");
            string numQuincena1 = DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi =1 and mqui_anoquin=" + DDLANO.SelectedValue + "");
            string numQuincena2 = DBFunctions.SingleData("select  mqui_codiquin from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi = 2 and mqui_anoquin=" + DDLANO.SelectedValue + "");
            if (numQuincena1 == "")
                numQuincena1 = "0";
			//  ELIMINO LAS PROVIONES DE LA QUINCENA, LAS CALCULO Y LA INSERTO NUEVAMENTE EN LA TABLA 	
			DBFunctions.NonQuery("delete from dbxschema.mprovisiones where mqui_codiquin="+numQuincena2+" ");
		 
			DataSet provisiones=new DataSet();
	/*		string sqlProvisiones =
               @"select a.mqui_codiquin, a.mEMP_CODIEMPL, a.PPRO_CODIPROV, sum(a.valorprov), a.mnit_nit from (  
				 Select DQUI.mqui_codiquin, memp.mEMP_CODIEMPL, PPRO.PPRO_CODIPROV,   
				  case when (DQUI.pcon_concepto = cnomina.cnom_concsubtcodi and PPRO.PPRO_ttipo >= 4) then 0  
				       else ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*PPRO.PPRO_PORCPROV*0.01),0)  
                   end as valorprov, memp.mnit_nit   
				  from dbxschema.mempleado memp,dbxschema.pprovisionnomina PPRO, dbxschema.pconceptonomina Pcon,  
				       dbxschema.dquincena DQUI, cnomina  
				 where DQUI.mqui_codiquin = "+numQuincena+@"  
				  and DQUI.pcon_concepto = Pcon.pcon_concepto  
				  and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL   
				  and pcon.tres_afecprovision = 'S'   
				  and MEMP.tcon_contrato <> '3'  
				  and memp.tsal_salario <> '1'  
				 GROUP BY DQUI.mqui_codiquin,memp.mEMP_CODIEMPL,PPRO.PPRO_CODIPROV,PPRO.PPRO_ttipo,memp.mnit_nit,  
				  DQUI.pcon_concepto,cnomina.cnom_concsubtcodi  
				 union  
				 Select DQUI.mqui_codiquin, memp.mEMP_CODIEMPL, PPRO.PPRO_CODIPROV,  
				 ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*PPRO.PPRO_PORCPROV*0.01),0)  
                   as valorprov, memp.mnit_nit    
				 from dbxschema.mempleado memp,dbxschema.pprovisionnomina PPRO, dbxschema.pconceptonomina Pcon,  
				      dbxschema.dquincena DQUI, cnomina, ttipoprovision tpro  
				 where DQUI.mqui_codiquin = "+numQuincena+@"   
				  and DQUI.pcon_concepto = Pcon.pcon_concepto  
				  and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL   
				  and pcon.tres_afecprovision = 'S'  
				  and PPRO.ppro_ttipo = tpro.ttip_estado and tpro.ttip_estado >= 4  
				  and memp.tsal_salario = '1'  
				 GROUP BY DQUI.mqui_codiquin,memp.mEMP_CODIEMPL,PPRO.PPRO_CODIPROV,memp.mnit_nit  
                 ) as a group by a.mqui_codiquin, a.mEMP_CODIEMPL, a.PPRO_CODIPROV, a.mnit_nit   
                 ORDER BY a.mEMP_CODIEMPL, a.PPRO_CODIPROV; ";
     */

            string sqlProvisionesMes =
                @"select " + numQuincena2 + @", b.mEMP_CODIEMPL, b.PPRO_CODIPROV, sum(b.valorprov), b.mnit_nit from (
      select  a.mEMP_CODIEMPL, a.PPRO_CODIPROV, sum(a.valorprov) as valorprov , a.mnit_nit   from (  
				 Select 2 AS MQUI_CODIQUIN, memp.mEMP_CODIEMPL, PPRO.PPRO_CODIPROV,   
				  case when (DQUI.pcon_concepto = cnomina.cnom_concsubtcodi and PPRO.PPRO_ttipo >= 4) then 0  
				       else ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*PPRO.PPRO_PORCPROV*0.01),0)  
                   end as valorprov, memp.mnit_nit,dqui.pcon_concepto    
				  from dbxschema.mempleado memp,dbxschema.pprovisionnomina PPRO, dbxschema.pconceptonomina Pcon,  
				       dbxschema.dquincena DQUI, cnomina  
				 where DQUI.mqui_codiquin in (" + numQuincena1 + @", " + numQuincena2 + @") 
				  and DQUI.pcon_concepto = Pcon.pcon_concepto  
				  and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL   
				  and pcon.tres_afecprovision = 'S'   
				  and MEMP.tcon_contrato <> '3'  
				  and memp.tsal_salario <> '1'  
				 GROUP BY DQUI.mqui_codiquin,memp.mEMP_CODIEMPL,PPRO.PPRO_CODIPROV,PPRO.PPRO_ttipo,memp.mnit_nit,  
				    DQUI.pcon_concepto,cnomina.cnom_concsubtcodi 
		 	 	      ) as a group by a.mEMP_CODIEMPL, a.PPRO_CODIPROV, a.mnit_nit  
 			       union  
		select  a.mEMP_CODIEMPL, a.PPRO_CODIPROV, sum(a.valorprov) as valorprov , a.mnit_nit   from (  			   
				 Select 2, memp.mEMP_CODIEMPL, PPRO.PPRO_CODIPROV,  
				 ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*PPRO.PPRO_PORCPROV*0.01),0)  
                   as valorprov, memp.mnit_nit     
				 from dbxschema.mempleado memp,dbxschema.pprovisionnomina PPRO, dbxschema.pconceptonomina Pcon,  
				      dbxschema.dquincena DQUI, cnomina, ttipoprovision tpro  
				 where DQUI.mqui_codiquin in (" + numQuincena1 + @", " + numQuincena2 + @")
				  and DQUI.pcon_concepto = Pcon.pcon_concepto  
				  and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL   
				  and pcon.tres_afecprovision = 'S'  
        		  and PPRO.ppro_ttipo = tpro.ttip_estado and tpro.ttip_estado >= 4  
				  and memp.tsal_salario = '1'  
				 GROUP BY memp.mEMP_CODIEMPL,PPRO.PPRO_CODIPROV,memp.mnit_nit 
		          ) as a group by a.mEMP_CODIEMPL, a.PPRO_CODIPROV, a.mnit_nit    
				   ) as b group by b.mEMP_CODIEMPL, b.PPRO_CODIPROV, b.mnit_nit    
                 ORDER BY mEMP_CODIEMPL, PPRO_CODIPROV, mnit_nit 
                ";

           	DBFunctions.Request(provisiones,IncludeSchema.NO,sqlProvisionesMes);

            if (provisiones.Tables.Count > 0)
            {
                for (i = 0; i < provisiones.Tables[0].Rows.Count; i++)
                {
                    DBFunctions.NonQuery("insert into mprovisiones values ( default," + numQuincena2 + ",'" + provisiones.Tables[0].Rows[i][1].ToString() + "','" + provisiones.Tables[0].Rows[i][2].ToString() + "'," + provisiones.Tables[0].Rows[i][3].ToString() + ")");
                }
            }
	
			DataSet empleados=new DataSet();
			DBFunctions.Request(empleados,IncludeSchema.NO, "select memp_codiempl, mnit_apellidos concat ' ' concat mnit_nombres, palm_almacen from dbxschema.mempleado,dbxschema.mnit where tcon_contrato <> '3' and mempleado.mnit_nit = mnit.mnit_nit");
			string almacen="";
			string centCosto="";
			if (empleados.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
                    almacen = empleados.Tables[0].Rows[i]["PALM_ALMACEN"].ToString(); //DBFunctions.SingleData("select palm_almacen from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"'"); 

                    DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
                    DBFunctions.Request(numcuentas, IncludeSchema.NO, @"select distinct MPROV.mqui_codiquin,MPROV.memp_codiempl,PCONCE.pdep_codidpto,MPROV.pcon_concepto,MPROV.mpro_valor,ppro_nombprov,mcue_codipucdebiprov,mcue_codipuccredprov, MEMP.mnit_nit, mcd.TCLA_CODIGO as TCLA_CODIDEBI, Mcc.TCLA_CODIGO as TCLA_CODICRED
                                                                          from dbxschema.mprovisiones MPROV,dbxschema.pprovisionnomina PPRO,dbxschema.mempleado MEMP,dbxschema.ppucprovisionconcepto PCONCE
                                                                               left join dbxschema.mcuenta mcd on mcue_codipucdebiprov = mcd.mcue_codipuc
                                                                               left join dbxschema.mcuenta mcc on mcue_codipuccredprov = mcc.mcue_codipuc
                                                                         where mqui_codiquin=" + numQuincena2 + " and MPROV.memp_codiempl='" + empleados.Tables[0].Rows[i][0].ToString() + "' and MPROV.pcon_concepto=PPRO.ppro_codiprov and PCONCE.ppro_codiprov=PPRO.ppro_codiprov and MEMP.memp_codiempl=MPROV.memp_codiempl and PCONCE.pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='" + empleados.Tables[0].Rows[i][0].ToString() + "') ");
					//Averiguar el centro de costo con mayor porcentaje
					centCosto=DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"') AND MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"' FETCH FIRST 1 ROWS ONLY");
					codempleado=empleados.Tables[0].Rows[i][0].ToString();
					if (numcuentas.Tables.Count!=0 && numcuentas.Tables[0].Rows.Count!=0)
					{
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
                            //proceso debito
                            string clasecuenta = numcuentas.Tables[0].Rows[x]["TCLA_CODIDEBI"].ToString(); //DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][6].ToString()+"' ");
							//nit
							//nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
                            nit = numcuentas.Tables[0].Rows[x][8].ToString();		
							if (clasecuenta=="N")
							{
								double sumatemp=0;
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
                                DBFunctions.Request(porcentajes, IncludeSchema.NO, "select memp_codiempl,COALESCE(pALM_ALMACEN,'ERROR Sede'),COALESCE(pcen_codigo,'ERROR CCosto'),COALESCE(mecc_porcentaje,100) from dbxschema.mempleadopcentrocosto where memp_codiempl='" + numcuentas.Tables[0].Rows[x][1].ToString() + "'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, " ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes Interfase Provisiones.");
								}
								//for de porcentajes
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//Redondeo
									if (y==porcentajes.Tables[0].Rows.Count-1)
									{
										double ajuste=Math.Round(Convert.ToDouble(numcuentas.Tables[0].Rows[x][4])-sumatemp, 2);
										valorcentrocosto=ajuste;
									}
									else
									{
										//debito***************
										valorcentrocosto=(Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString()))* double.Parse(porcentajes.Tables[0].Rows[y][3].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);					
									}
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									sumatemp+=valorcentrocosto;
									string razon=numcuentas.Tables[0].Rows[x][5].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
                                    this.Ingresar_Datos_Detalles(ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text, numcuentas.Tables[0].Rows[x][6].ToString(), ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text, nit, porcentajes.Tables[0].Rows[y][1].ToString(), porcentajes.Tables[0].Rows[y][2].ToString(), razon, valorcentrocosto.ToString(), "0", "0");
									sumatotaldebito=sumatotaldebito+valorcentrocosto;
                                    totalDebito += valorcentrocosto;
                                }
							}
							else
							{
								//proceso sin porcentaje
								//debito***************
								valorcentrocosto=Math.Round(Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString()),2);
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
								string razon=numcuentas.Tables[0].Rows[x][5].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
								razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
								this.Ingresar_Datos_Detalles(ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text,numcuentas.Tables[0].Rows[x][6].ToString(),ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text,nit,almacen,centCosto,razon,valorcentrocosto.ToString(),"0","0");
								sumatotaldebito=sumatotaldebito+valorcentrocosto;
                                totalDebito += valorcentrocosto;
                            }
							//PROCESO CREDITO
							clasecuenta = numcuentas.Tables[0].Rows[x]["TCLA_CODICRED"].ToString(); //DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][7].ToString()+"' ");
                            if (clasecuenta=="N")
							{
								double sumatemp=0;
								DataSet porcentajes=new DataSet();
                                DBFunctions.Request(porcentajes, IncludeSchema.NO, "select memp_codiempl,COALESCE(palm_almacen,'ERROR SEDE'),COALESCE(pcen_codigo,'ERROR CCOSTO'),mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='" + numcuentas.Tables[0].Rows[x][1].ToString() + "'");
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//credito***********************
									if (y==porcentajes.Tables[0].Rows.Count-1)
									{
										double ajuste=Math.Round(Convert.ToDouble(numcuentas.Tables[0].Rows[x][4])-sumatemp, 2);
										valorcentrocosto=ajuste;
									}
									else
									{
										valorcentrocosto=Math.Round(Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][3].ToString())/100, 2);
									}
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									sumatemp+=valorcentrocosto;
									//nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
                                    string razon=numcuentas.Tables[0].Rows[x][5].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
                                    this.Ingresar_Datos_Detalles(ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text, numcuentas.Tables[0].Rows[x][7].ToString(), ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text, nit, porcentajes.Tables[0].Rows[y][1].ToString(), porcentajes.Tables[0].Rows[y][2].ToString(), razon, "0", valorcentrocosto.ToString(), "0");
                                    totalCredito += valorcentrocosto;
                                }
							}
							else
							{
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][4].ToString());
								valorcentrocosto=Math.Round(valorcentrocosto,2);					
								//credito***********************
								//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
								//nit=DBFunctions.SingleData("select MNIT.mnit_nit,MEMP.memp_codiempl from dbxsChema.mnit MNIT,dbxschema.mempleado MEMP where MNIT.mnit_nit=MEMP.mnit_nit and memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
                                string razon=numcuentas.Tables[0].Rows[x][5].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
								razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
								this.Ingresar_Datos_Detalles(ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text,numcuentas.Tables[0].Rows[x][7].ToString(),ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text,nit,almacen,centCosto,razon,"0",valorcentrocosto.ToString(),"0");
                                totalCredito += valorcentrocosto;
                            }
							
						}//cuentas
					}
				}//for empleados
                totalDebito -= totalCredito;
                if (totalDebito != 0)
                    this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text, ajustePeso.Tables[0].Rows[0][0].ToString(), ddlprefPara.SelectedValue, txtNumPara.Text, ajustePeso.Tables[0].Rows[0][3].ToString(), ajustePeso.Tables[0].Rows[0][1].ToString(), ajustePeso.Tables[0].Rows[0][3].ToString(), "Ajuste al Peso", totalDebito.ToString(), "0", "0");

                this.Ingresar_Datos_Cabecera(ddlprefProvisiones.SelectedValue, txtNumProvisiones.Text,DDLANO.SelectedValue,DDLMES.SelectedValue,txtFechaComprobante.Text,this.txtRazon.Text,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),susuario,Math.Round(sumatotaldebito,0).ToString());
			}
		}

		protected void EscribirArchivoARL(DataSet ajustePeso)  // Riesgos Laborales, antes ARL.
		{
            if (DDLQUIN.SelectedValue == "1")
                return;    // solo se contabiliza el mes completo en la segunda quincena
            int i=0;
			double sumatotaldebito=0;
			string codempleado="";
			string nitEmpleado, nitArp, nitEmpresa = "";
			int x,y;
			double valorcentrocosto=0;
            totalDebito = totalCredito = 0;
            bool nitxEmpresa = false;
            try
            {
                if (DBFunctions.SingleData("SELECT CNOM_NITPARAFISCEMPE FROM CNOMINA") == "S")
                    // del web.config se valida que nit imputa en los parafiscales (salud y pension)
                    nitxEmpresa = true;
            }
            catch { };	
			
		//	string numQuincena=DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" and mqui_anoquin="+DDLANO.SelectedValue+"");
            string numQuincena1 = DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi=1 and mqui_anoquin=" + DDLANO.SelectedValue + "");
            string numQuincena2 = DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi=2 and mqui_anoquin=" + DDLANO.SelectedValue + "");
            if (numQuincena1 == "")
                numQuincena1 = "0";
			//  ELIMINO LAS ARL DE LA QUINCENA, LAS CALCULO Y LA INSERTO NUEVAMENTE EN LA TABLA 	
			DBFunctions.NonQuery("delete from dbxschema.dprovapropiaciones where mqui_codiquin="+numQuincena2+" and papo_codiapor = (select papo_codiapor from dbxschema.paportepatronal where papo_tipoaporte = 3)");
		 
			DataSet aportesArp = new DataSet();
		/*	string sqlArp =
                @"Select DQUI.mqui_codiquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor,  
                  case when MEMP.TSAL_SALARIO = '1'   
                       then ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*Prie.Prie_PORCentaje*0.01*0.70),-2)  
                       else case when sum(DQUI.dqui_apagar-DQUI.dqui_adescontar) < (cnom_salaminiactu * 0.5) and cnom_opciquinomens = 1 and memp_peripago = '1'  
                 	                  and mq.mqui_anoquin <> year(memp.memp_fechIngreso) and mq.mqui_mesquin <> month(memp.memp_fechingreso)   
                                 then ROUND((cnom_salaminiactu * 0.5*Prie.Prie_PORCentaje*0.01),-2)  
                       else case when sum(DQUI.dqui_apagar-DQUI.dqui_adescontar) < cnom_salaminiactu and (cnom_opciquinomens = 2 or memp_peripago in ('2','4') ) 
                 	                  and mq.mqui_anoquin <> year(memp.memp_fechIngreso) and mq.mqui_mesquin <> month(memp.memp_fechingreso) 
                                 then ROUND((cnom_salaminiactu*Prie.Prie_PORCentaje*0.01),-2)  
                 	        else ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*Prie.Prie_PORCentaje*0.01),-2) end end end as arp,  
                  memp.tcon_contrato, memp.memp_suelactu, cnom.cnom_salaminiactu     		      
                  from dbxschema.mempleado memp, dbxschema.paportepatronal PAP, dbxschema.pconceptonomina Pcon, dbxschema.dquincena DQUI, 
                       dbxschema.priesgoprofesional prie, dbxschema.cnomina cnom, mquincenas mq  
                  where DQUI.mqui_codiquin=" + numQuincena + @" and DQUI.mqui_codiquin = mq.mqui_codiquin  
                  and DQUI.pcon_concepto = Pcon.pcon_concepto and memp.prie_codiries = prie.prie_codiries  
                  and DQUI.pcon_concepto <> cnom.cnom_concVACAcodi and pcon.pcon_claseconc <> 'L'  
                  and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL and pcon.tres_afec_EPS = 'S' and PAP.papo_tipoaporte = 3  
                  and DQUI.pcon_concepto <> cnom.cnom_concsubtcodi  
                  GROUP BY DQUI.mqui_codiquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor, memp.mnit_nit, TSAL_SALARIO, cnom_opciquinomens, memp_peripago, memp_suelactu,  
                           cnom_salaminiactu, cnom_concsubtcodi, Prie.Prie_PORCentaje, test_estado, mq.mqui_anoquin, mq.mqui_mesquin, memp.memp_fechingreso, tcon_contrato  
                  ORDER BY memp.mnit_nit;" ;
            */

            string sqlArpMes =
                @"Select MQ.mqui_MESquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor,  
                  case when MEMP.TSAL_SALARIO = '1'   
                       then ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*Prie.Prie_PORCentaje*0.01*0.70),-2)  
                       else case when sum(DQUI.dqui_apagar-DQUI.dqui_adescontar) < (cnom_salaminiactu * 0.5) and cnom_opciquinomens = 1 and memp_peripago = '1'  
                 	                  and mq.mqui_anoquin <> year(memp.memp_fechIngreso) and mq.mqui_mesquin <> month(memp.memp_fechingreso)   
                                 then ROUND((cnom_salaminiactu * 0.5*Prie.Prie_PORCentaje*0.01),-2)  
                       else case when sum(DQUI.dqui_apagar-DQUI.dqui_adescontar) < cnom_salaminiactu and (cnom_opciquinomens = 2 or memp_peripago in ('2','4') ) 
                 	                  and mq.mqui_anoquin <> year(memp.memp_fechIngreso) and mq.mqui_mesquin <> month(memp.memp_fechingreso) 
                                 then ROUND((cnom_salaminiactu*Prie.Prie_PORCentaje*0.01),-2)  
                 	        else ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*Prie.Prie_PORCentaje*0.01),-2) end end end as arp,  
                  memp.tcon_contrato, memp.memp_suelactu, cnom.cnom_salaminiactu     		      
                  from  dbxschema.mempleado memp, dbxschema.paportepatronal PAP, dbxschema.pconceptonomina Pcon, dbxschema.dquincena DQUI, 
                        dbxschema.priesgoprofesional prie, dbxschema.cnomina cnom, mquincenas mq  
                  where DQUI.mqui_codiquin in (" + numQuincena1 + @"," + numQuincena2 + @") and DQUI.mqui_codiquin = mq.mqui_codiquin  
                   and  DQUI.pcon_concepto = Pcon.pcon_concepto and memp.prie_codiries = prie.prie_codiries  
                   and  DQUI.pcon_concepto <> cnom.cnom_concVACAcodi and pcon.pcon_claseconc <> 'L'  
                   and  MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL and (pcon.tres_afec_EPS = 'S' or memp.tcon_contrato = '3') and PAP.papo_tipoaporte = 3  
                   and  DQUI.pcon_concepto <> cnom.cnom_concsubtcodi  
                  GROUP BY MQ.mqui_MESquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor, memp.mnit_nit, TSAL_SALARIO, cnom_opciquinomens, memp_peripago, memp_suelactu,  
                           cnom_salaminiactu, cnom_concsubtcodi, Prie.Prie_PORCentaje, test_estado, mq.mqui_anoquin, mq.mqui_mesquin, memp.memp_fechingreso, tcon_contrato  
                  ORDER BY memp.mnit_nit;";  

			DBFunctions.Request(aportesArp,IncludeSchema.NO,sqlArpMes);
			if (aportesArp.Tables.Count!=0 && aportesArp.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<aportesArp.Tables[0].Rows.Count;i++)
				{
                    if (aportesArp.Tables[0].Rows[i][4].ToString() == "3" && Convert.ToDouble(aportesArp.Tables[0].Rows[i][5]) > 0) // 4 tipo contrato (3 = sena), 5 = salario empleado, 6 = salario minimo || los sena se ajustan al minimo
                        aportesArp.Tables[0].Rows[i][3] = Math.Round(Convert.ToDouble(aportesArp.Tables[0].Rows[i][3]) * (Convert.ToDouble(aportesArp.Tables[0].Rows[i][6]) / Convert.ToDouble(aportesArp.Tables[0].Rows[i][5])),2);
					aportesArp.Tables[0].Rows[i][3] = Math.Round(Convert.ToDouble(aportesArp.Tables[0].Rows[i][3]) / 100,2) * 100;
                    DBFunctions.NonQuery("insert into dprovapropiaciones values ( default,"+numQuincena2+",'"+aportesArp.Tables[0].Rows[i][1].ToString()+"','"+aportesArp.Tables[0].Rows[i][2].ToString()+"',"+aportesArp.Tables[0].Rows[i][3].ToString()+")");
				}
			}
			else
                Utils.MostrarAlerta(Response, "ATENCION: NO SE Encontraron Registros para generar ARL en esta quincena");
						
			//   termina de re-generar ARL
	
			DataSet empleados=new DataSet();
//			DBFunctions.Request(empleados,IncludeSchema.NO,"select memp_codiempl from dbxschema.mempleado  where test_estado='1'");
            DBFunctions.Request(empleados, IncludeSchema.NO, "select memp_codiempl, mnit_apellidos concat ' ' concat mnit_nombres, ME.MNIT_NIT, CE.MNIT_NIT, me.palm_almacen from dbxschema.mempleado me, dbxschema.mnit,  dbxschema.CEMPRESA CE where me.mnit_nit = mnit.mnit_nit");
			if (empleados.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
					//Averiguar el centro de costo con mayor porcentaje
					string centCosto=DBFunctions.SingleData(@"Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO 
                           WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO 
                                WHERE MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"') AND MEMP_CODIEMPL='"+empleados.Tables[0].Rows[i][0].ToString()+"'");
                    string almacen = empleados.Tables[0].Rows[i]["PALM_ALMACEN"].ToString();//DBFunctions.SingleData("select palm_almacen from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"'"); 

                    string sqlcuentasArp =
                        @"Select DPROV.mqui_codiquin,DPROV.memp_codiemp,DPROV.papo_codiapor,DPROV.valor,PAPOR.papo_tipoaporte,MEMP.parp_codiarp,ARP.mnit_nit,  
                           coalesce(mcue_codipucdebiarp,'ERROR PARAMT'), COALESCE(mcue_codipuccredarp,'ERROR PARAMT'), memp.pdep_codidpto,PAPOR.papo_nombapor, MEMP.mnit_nit,
                           COALESCE(MCD.TCLA_CODIGO,'N') AS TCLA_CODIDEBI, COALESCE(MCC.TCLA_CODIGO,'R') AS TCLA_CODICRED      
						 	from dbxschema.dprovapropiaciones DPROV,dbxschema.paportepatronal PAPOR,dbxschema.parp ARP,dbxschema.mempleado MEMP  
						 	     LEFT JOIN dbxschema.ppucarp PPUCARP ON MEMP.parp_codiarp=PPUCARP.parp_codiarp and PPUCARP.pdep_codidpto=memp.pdep_codidpto  
                                 left join dbxschema.mcuenta mcd on mcue_codipucdebiarp = mcd.mcue_codipuc
                                 left join dbxschema.mcuenta mcc on mcue_codipuccredarp = mcc.mcue_codipuc
						 	where DPROV.papo_codiapor=PAPOR.papo_codiapor and papo_tipoaporte=3 and MEMP.memp_codiempl=DPROV.memp_codiemp and ARP.parp_codiarp=MEMP.parp_codiarp  
						 	and mqui_codiquin=" + numQuincena2+" and memp_codiemp='"+empleados.Tables[0].Rows[i][0].ToString()+"';"; 
					DBFunctions.Request(numcuentas,IncludeSchema.NO,sqlcuentasArp);
					codempleado=empleados.Tables[0].Rows[i][0].ToString();
                  				
					if (numcuentas.Tables.Count!=0 && numcuentas.Tables[0].Rows.Count!=0)
					{
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
                            //PROCESO DEBITO
                            string clasecuenta = numcuentas.Tables[0].Rows[x]["TCLA_CODIDEBI"].ToString(); //DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][7].ToString()+"' ");
                            nitEmpleado = numcuentas.Tables[0].Rows[x][11].ToString();
                            nitArp      = numcuentas.Tables[0].Rows[x][6].ToString();
                            nitEmpresa  = empleados.Tables[0].Rows[i][3].ToString();
				
                            if (clasecuenta!="N" && clasecuenta!="R")
								clasecuenta="R";
							if (clasecuenta=="N")
							{
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								double sumatemp=0;
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, " ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes.Interfase ARL");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//debito***************
									if (y==porcentajes.Tables[0].Rows.Count-1)
									{
										double ajuste=Math.Round(Convert.ToDouble(numcuentas.Tables[0].Rows[x][3])-sumatemp, 2);
										valorcentrocosto=ajuste;
									}
									else
									{
										valorcentrocosto=(Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString()))* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);					
									}
                                        /*
                                    if (nitxEmpresa)
                               //         nit = empleados.Tables[0].Rows[i][2].ToString(); // nit del empleado
                                        nit = nitEmpresa;
                                    else
                                        */
                                        nit = nitArp;
									sumatemp += valorcentrocosto;
									string razon =numcuentas.Tables[0].Rows[x][10].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
									this.Ingresar_Datos_Detalles(ddlprefArp.SelectedValue, txtNumArp.Text,numcuentas.Tables[0].Rows[x][7].ToString(),ddlprefArp.SelectedValue, txtNumArp.Text,nit,almacen,porcentajes.Tables[0].Rows[y][1].ToString(),razon,valorcentrocosto.ToString(),"0","0");			
									sumatotaldebito+=valorcentrocosto;
                                    totalDebito += valorcentrocosto;
                                }
							}//ACABO EL IF
							else
							{
								//debito***************
								valorcentrocosto = Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
								valorcentrocosto = Math.Round(valorcentrocosto,2);
								nit = numcuentas.Tables[0].Rows[x][6].ToString(); // Nit de la ARL
                                string razon = numcuentas.Tables[0].Rows[x][10].ToString() + " " +DDLANO.SelectedValue + DDLMES.SelectedValue;
								razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
								this.Ingresar_Datos_Detalles(ddlprefArp.SelectedValue, txtNumArp.Text,numcuentas.Tables[0].Rows[x][7].ToString(),ddlprefArp.SelectedValue, txtNumArp.Text,nit,almacen,centCosto,razon,valorcentrocosto.ToString(),"0","0");			
								sumatotaldebito+=valorcentrocosto;
                                totalDebito += valorcentrocosto;
                                //sumapagado=sumapagado+valorcentrocosto;
                                //Response.Write("<script language:javascript>alert(' SUMA pagado EMPLEADO "+numcuentas.Tables[0].Rows[x][0].ToString()+"VA EN : "+sumapagado+" ');</script>");
                                //sumatotaldebito+=valorcentrocosto;
                            }

                            //proceso credito
                            clasecuenta = numcuentas.Tables[0].Rows[x]["TCLA_CODICRED"].ToString(); //DBFunctions.SingleData("select TCLA_CODIGO from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][8].ToString()+"' ");
                            if (clasecuenta!="N" && clasecuenta!="R")
								clasecuenta="N";
							if (clasecuenta=="N")
							{
								double sumatemp=0;
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "  ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes.Interfase ARL");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//credito***********************
									if (y==porcentajes.Tables[0].Rows.Count-1)
									{
										double ajuste=Math.Round(Convert.ToDouble(numcuentas.Tables[0].Rows[x][3])-sumatemp, 2);
										valorcentrocosto=ajuste;
									}
									else
									{
										valorcentrocosto=(Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString()))* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);					
									}
									//Response.Write("<script language:javascript>alert('cuenta "+numcuentas.Tables[0].Rows[x][2].ToString()+" tanta plata "+valorcentrocosto+" al centro de costo "+porcentajes.Tables[0].Rows[x][1].ToString()+" ');</script>");
									//nit
									string razon =numcuentas.Tables[0].Rows[x][10].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
									this.Ingresar_Datos_Detalles(ddlprefArp.SelectedValue, txtNumArp.Text,numcuentas.Tables[0].Rows[x][8].ToString(),ddlprefArp.SelectedValue, txtNumArp.Text,nit,almacen,porcentajes.Tables[0].Rows[y][1].ToString(),razon,"0",valorcentrocosto.ToString(),"0");
                                    totalCredito += valorcentrocosto;
                                }
							}
							else
							{
								//credito***********************
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
								valorcentrocosto=Math.Round(valorcentrocosto,2);
                                if (nitxEmpresa)
                          //          nit = empleados.Tables[0].Rows[i][2].ToString(); // nit del empleado
                                    nit = nitArp;    // nit de la ARL
                                else
                                    nit = nitEmpresa;
                          	    string razon =numcuentas.Tables[0].Rows[x][10].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
								razon = razon+" "+empleados.Tables[0].Rows[i][1].ToString();
								this.Ingresar_Datos_Detalles(ddlprefArp.SelectedValue, txtNumArp.Text,numcuentas.Tables[0].Rows[x][8].ToString(),ddlprefArp.SelectedValue, txtNumArp.Text,nit,almacen,centCosto,razon,"0",valorcentrocosto.ToString(),"0");  // tenia nitArp	
                                totalCredito += valorcentrocosto;
                            }
						}//CUENTAS
					}
				}
			}
            totalDebito -= totalCredito;
            if (totalDebito != 0)
                this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text, ajustePeso.Tables[0].Rows[0][0].ToString(), ddlprefPara.SelectedValue, txtNumPara.Text, nit, ajustePeso.Tables[0].Rows[0][1].ToString(), ajustePeso.Tables[0].Rows[0][2].ToString(), "Ajuste al Peso", totalDebito.ToString(), "0", "0");

            this.Ingresar_Datos_Cabecera(ddlprefArp.SelectedValue,txtNumArp.Text,DDLANO.SelectedValue,DDLMES.SelectedValue,txtFechaComprobante.Text,this.txtRazon.Text,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),susuario,Math.Round(sumatotaldebito,0).ToString());			
		}

		
		protected void EscribirArchivoParafiscales(DataSet ajustePeso)
		{
            //   solo se genera esta interfase en la 2da quincena porque toma todo el acumulado del mes
            string numAno      = DBFunctions.SingleData("select mqui_anoquin  from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi = 2 and mqui_anoquin=" + DDLANO.SelectedValue + " ");
            string numMes      = DBFunctions.SingleData("select mqui_mesquin  from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi = 2 and mqui_anoquin=" + DDLANO.SelectedValue + " ");
            string numQuincena = DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi = 2 and mqui_anoquin=" + DDLANO.SelectedValue + " ");
            string quincena    = DBFunctions.SingleData("select mqui_TPERNOMI from dbxschema.mquincenas where mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_tpernomi = " + DDLQUIN.SelectedValue + " and mqui_anoquin=" + DDLANO.SelectedValue + " ");
            int  i = 0;
            bool nitxEmpresa = false;
            totalDebito = totalCredito = 0;
            if (quincena != "2")
                return;    // solo se liquidan aportes en la segunda quincena cuando se tiene todo el mes acumulado.

            try
            {
                if (DBFunctions.SingleData("SELECT CNOM_NITPARAFISCEMPE FROM CNOMINA") == "S")
                    // del web.config se valida que nit imputa en los parafiscales (salud y pension)
                    nitxEmpresa = true;
            }
            catch { };	
		
		
        //  ELIMINO LAS Parafiscales DE LA QUINCENA, LAS CALCULO Y LA INSERTO NUEVAMENTE EN LA TABLA 	
			DBFunctions.NonQuery("delete from dbxschema.dprovapropiaciones where mqui_codiquin="+numQuincena+" and papo_codiapor <> (select papo_codiapor from dbxschema.paportepatronal where papo_tipoaporte = 3)");
		 
			DataSet parafiscales=new DataSet();
            string sqlParafiscales =
            @"select a.* from (  
            Select MQUI.mqui_MESquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor,  
            case when (tcon_contrato = '4' AND PAPO_TIPOAPORTE = 5) 
                THEN 0  
                else case when tsal_salario = '1'  
                          then ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*0.7*PAP.Papo_PORCapor*0.01),-2)  
                            else case when SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)) < cnom_salaminiactu and memp.test_estado <> '4' and (year(memp_fechingreso) <> " + numAno + @" or month(memp_fechingreso) <> " + numMes + @")  
                                        then ROUND((cnom_salaminiactu * PAP.Papo_PORCapor*0.01),-2)  
                                        else ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*PAP.Papo_PORCapor*0.01),-2)  
            END end end as netoPagado, 
            case when tsal_salario = '1' 
                then ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*0.7),0) 
	            else SUM(DQUI.dqui_apagar-DQUI.dqui_adescontar) end as base, 
			Pap.Papo_TIPOaporTE,
			tcon_contrato,
			SUM(DQUI.dqui_apagar-DQUI.dqui_adescontar) as neto  
            from  dbxschema.mempleado memp, dbxschema.paportepatronal PAP, dbxschema.pconceptonomina Pcon, dbxschema.Mquincenas MQUI, dbxschema.dquincena DQUI, dbxschema.cnomina cn  
            where MQUI.mqui_ANOQUIN = " + numAno + @" AND MQUI.mqui_MESQUIN = " + numMes + @" AND DQUI.mqui_codiquin=MQUI.mqui_codiquin and DQUI.pcon_concepto = Pcon.pcon_concepto and MEMP.tcon_contrato <> '3'  
              and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL and pcon.tres_afecAPORTES = 'S' and PAP.papo_tipoaporte NOT IN (3,4) 
              and dqui_vacaciones <> 'L'
            GROUP BY MQUI.MQUI_MESQUIN, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor, tsal_salario, cnom_salaminiactu,   
                    PAP.Papo_PORCapor, tcon_contrato, PAPO_TIPOAPORTE, test_estado, memp_fechingreso  
            
			union  
            
			Select MQUI.mqui_MESquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor,  
            case when (tcon_contrato = '4' AND PAPO_TIPOAPORTE = 5) 
                THEN 0  
                else case when tsal_salario = '1'  
                            then ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*0.7*PAP.Papo_PORCapor*0.01),-2)  
                            else case when SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)) < cnom_salaminiactu and memp.test_estado <> '4'  
                                          and (year(memp_fechingreso) <> " + numAno + @" or month(memp_fechingreso) <> " + numMes + @")  
                                        then ROUND((cnom_salaminiactu * PAP.Papo_PORCapor*0.01),-2)  
                                        else ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*PAP.Papo_PORCapor*0.01),-2)  
            END end end as netoPagado,
            case when tsal_salario = '1' then ROUND(SUM((DQUI.dqui_apagar-DQUI.dqui_adescontar)*0.7),0) else SUM(DQUI.dqui_apagar-DQUI.dqui_adescontar) end as base, 
			Pap.Papo_TIPOaporTE,
			tcon_contrato,
			SUM(DQUI.dqui_apagar-DQUI.dqui_adescontar) as neto 
            from dbxschema.mempleado memp,dbxschema.paportepatronal PAP, dbxschema.pconceptonomina Pcon, dbxschema.Mquincenas MQUI, dbxschema.dquincena DQUI, dbxschema.cnomina cn  
            where MQUI.mqui_ANOQUIN = " + numAno + @" AND MQUI.mqui_MESQUIN = " + numMes + @" AND DQUI.mqui_codiquin=MQUI.mqui_codiquin and DQUI.pcon_concepto = Pcon.pcon_concepto and MEMP.tcon_contrato <> '3'  
              and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL and pcon.tres_afecAPORTES = 'S' and PAP.papo_tipoaporte = 4 and pcon.PCON_CLASECONC <> 'L'  
              and dqui_vacaciones <> 'L'
          GROUP BY MQUI.MQUI_MESQUIN, memp.mEMP_CODIEMPL,Pap.Papo_CODIapor,Pap.Papo_nombapor,memp.mnit_nit,tsal_salario,cnom_salaminiactu,cnom_opciquinomens,  
                    PAP.Papo_PORCapor, tcon_contrato, PAPO_TIPOAPORTE, test_estado, memp_fechingreso  
            
			union  
            
			Select MQUI.mqui_MESquin, memp.mEMP_CODIEMPL, Pap.Papo_CODIapor,  
            ROUND((cnom_salaminiactu * PAP.Papo_PORCapor*0.01 + cnom_salaminiactu * cnom_PORCepsempl*0.01),-2) as aporteeps, cnom_salaminiactu, 
			Pap.Papo_TIPOaporTE,
			tcon_contrato,
			SUM(DQUI.dqui_apagar-DQUI.dqui_adescontar) as neto
            from dbxschema.mempleado memp,dbxschema.paportepatronal PAP, dbxschema.pconceptonomina Pcon, dbxschema.Mquincenas MQUI, dbxschema.dquincena DQUI, dbxschema.cnomina cn  
            where MQUI.mqui_ANOQUIN = " + numAno + @" AND MQUI.mqui_MESQUIN = " + numMes + @" AND DQUI.mqui_codiquin=MQUI.mqui_codiquin and DQUI.pcon_concepto = Pcon.pcon_concepto and MEMP.tcon_contrato in ('3','5') 
              and MEMP.MEMP_CODIEMPL = DQUI.MEMP_CODIEMPL and (pcon.tres_afecAPORTES = 'S' or memp.tcon_contrato in ('3','5')) and PAP.papo_tipoaporte = 6  
              and dqui_vacaciones <> 'L'
            GROUP BY MQUI.MQUI_MESQUIN, memp.mEMP_CODIEMPL,Pap.Papo_CODIapor,Pap.Papo_nombapor,PAP.Papo_PORCapor,memp.mnit_nit,cnom_opciquinomens,cnom_salaminiactu,  
                    cnom_PORCepsempl, PAPO_TIPOAPORTE, tcon_contrato 
            ) as a,  cempresa ce, cnomina cn where (a.Papo_TIPOaporTE not in (1,2,6) or (a.Papo_TIPOaporTE in (1,2,6) and neto >= cn.cnom_salaminiactu * 10) or ce.cemp_indiempr = '3' or tcon_contrato in ('3','5'))
            ORDER BY 2,3; ";  

            //   CONTRATO 3  = SENA
            //   TIPO DE APORTE 1 = I.C.B.F.,   2 = SENA,    6  =   SALUD son excentos por IMPOconsumo hasta 10 salarios mínimos de ingreso
            //  or ce.cemp_indiempr = 3   LAS cooperativas no PAGAN e CREE por lo tanto SI deben pagar los aportes parafiscales
			DBFunctions.Request(parafiscales,IncludeSchema.NO,sqlParafiscales);
            if(parafiscales.Tables.Count>0)
            {
			    for (i=0;i<parafiscales.Tables[0].Rows.Count;i++)
			    {
				    DBFunctions.NonQuery("insert into dprovapropiaciones values ( default,"+numQuincena+",'"+parafiscales.Tables[0].Rows[i][1].ToString()+"','"+parafiscales.Tables[0].Rows[i][2].ToString()+"',"+parafiscales.Tables[0].Rows[i][3].ToString()+")");
			    }
            }
             
			//   termina de re-generar parafiscales
	
			i = 0;
			double sumatotaldebito=0;
			string codempleado="";
            string nitEmpleado, nitEmpresa, nitAporte = "";
			int    x,y,h;
            bool   distxConcepto;
			double valorcentrocosto=0;
			
			DataSet empleados=new DataSet();
            DBFunctions.Request(empleados, IncludeSchema.NO,
                @"select memp.memp_codiempl,memp.peps_codieps,peps.peps_nombeps,peps.mnit_nit,MEMP.pfon_codipens,PFP.pfon_nombpens,PFP.mnit_nit,
                 memp.pfon_codipensvolu,PFP2.pfon_nombpens,PFP2.mnit_nit,mn.mnit_APELLIDOS CONCAT ' ' CONCAT MN.MNIT_NOMBRES, memp.mnit_nit, 
                COALESCE(PEPS_MCUENTA,'* ERROR CTA PUC'), COALESCE(PFP.MCUE_CODIPUC,'* ERROR CTA PUC'), COALESCE(PFP2.MCUE_codipuc,'* ERROR CTA PUC'),
                memp.palm_almacen, CE.MNIT_NIT as nit_empresa, memp.MNIT_NIT as nit_empleado 
                from dbxschema.CEMPRESA CE, dbxschema.peps peps, dbxschema.pfondopension PFP, mnit mn, dbxschema.mempleado memp           
                    LEFT JOIN dbxschema.pfondopension PFP2 ON PFP2.pfon_codipens=memp.pfon_codipensvolu 
                where memp.peps_codieps=peps.peps_codieps and PFP.pfon_codipens=MEMP.pfon_codipens and MEMP.mnit_nit = mn.mnit_nit 
                   and (memp.test_estado <> 4 or (memp.test_estado = 4 and memp.memp_fechretiro > '" + txtFechaComprobante.Text + @"'))
                ORDER BY memp.mnit_nit");

            if (empleados.Tables[0].Rows.Count!=0)
			{
				for (i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					DataSet numcuentas=new DataSet();
					//sacar las cuentas asociadas a los conceptos
					//Averiguar el centro de costo con mayor porcentaje
                    string centCosto = DBFunctions.SingleData("Select PCEN_CODIGO from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE mecc_PORCENTAJE=(Select MAX(MECC_PORCENTAJE) from DBXSCHEMA.MEMPLEADOPCENTROCOSTO WHERE MEMP_CODIEMPL='" + empleados.Tables[0].Rows[i][0].ToString() + "') AND MEMP_CODIEMPL='" + empleados.Tables[0].Rows[i][0].ToString() + "'  FETCH FIRST 1 ROWS ONLY");
                    string almacen   = empleados.Tables[0].Rows[i]["PALM_ALMACEN"].ToString(); //DBFunctions.SingleData("select palm_almacen from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"'");
                    string sqlcuentasP =
                    /*
                @"Select DPROV.mqui_codiquin, DPROV.memp_codiemp, DPROV.papo_codiapor, DPROV.valor, PAPOR.papo_tipoaporte,   
                    coalesce(PPUCAPAT.MCUE_CODIPUCDEBIAPOR,'ERROR PARAMT'),   
                    coalesce(PPUCAPAT.MCUE_CODIPUCCREDAPOR,'ERROR PARAMT'),   
                    PPUCAPAT.pdep_codidpto, PAPOR.mnit_nit, PAPOR.papo_nombapor,
                    coalesce(mcn.TCLA_CODIGO,'N') as claseCuentaNominal,
                    coalesce(mcr.TCLA_CODIGO,'R') as claseCuentaReal
                 from dbxschema.paportepatronal PAPOR, dbxschema.dprovapropiaciones DPROV   
                    left join dbxschema.ppucaportepatronal PPUCAPAT on DPROV.papo_codiapor=PPUCAPAT.papo_codiapor   
                    left join dbxschema.mcuenta mcN on mcN.MCUE_CODIPUC = PPUCAPAT.MCUE_CODIPUCDEBIAPOR
                    left join dbxschema.mcuenta mcR on mcR.MCUE_CODIPUC = PPUCAPAT.MCUE_CODIPUCCREDAPOR
                 where DPROV.memp_codiemp='" + empleados.Tables[0].Rows[i][0].ToString() + @"' and DPROV.mqui_codiquin = " + numQuincena + @"  
                  and DPROV.papo_codiapor=PAPOR.papo_codiapor and PAPOR.papo_tipoaporte<>3    
                  and PPUCAPAT.pdep_codidpto=(select pdep_codidpto from dbxschema.mempleado where memp_codiempl='"+empleados.Tables[0].Rows[i][0].ToString()+"');";
                    */
                    @"Select DPROV.mqui_codiquin, DPROV.memp_codiemp, DPROV.papo_codiapor, DPROV.valor, PAPOR.papo_tipoaporte,   
    				 	coalesce(PPUCAPAT.MCUE_CODIPUCDEBIAPOR,'ERROR PARAMT'),   
    				 	coalesce(PPUCAPAT.MCUE_CODIPUCCREDAPOR,'ERROR PARAMT'),   
					 	PPUCAPAT.pdep_codidpto, coalesce(pc.mnit_nit,PAPOR.mnit_nit), PAPOR.papo_nombapor,
                        coalesce(mcn.TCLA_CODIGO,'N') as claseCuentaNominal,
                        coalesce(mcr.TCLA_CODIGO,'R') as claseCuentaReal
					 from dbxschema.mempleado me 
					 	    left join dbxschema.pcajacompensacion pc on me.pcaj_codicaja = pc.pcaj_codicaja
					 	    left join dbxschema.paportepatronal PAPOR on PAPOR.papo_tipoaporte<>3 
					 	    left join dbxschema.dprovapropiaciones DPROV on DPROV.papo_codiapor = PAPOR.papo_codiapor and me.memp_codiempl = DPROV.memp_codiemp 
					      	left join dbxschema.ppucaportepatronal PPUCAPAT on DPROV.papo_codiapor=PPUCAPAT.papo_codiapor and PPUCAPAT.pdep_codidpto=me.pdep_codidpto  
                            left join dbxschema.mcuenta mcN on mcN.MCUE_CODIPUC = PPUCAPAT.MCUE_CODIPUCDEBIAPOR
                        	left join dbxschema.mcuenta mcR on mcR.MCUE_CODIPUC = PPUCAPAT.MCUE_CODIPUCCREDAPOR
                     where me.memp_codiempl = '" + empleados.Tables[0].Rows[i][0].ToString() + @"' and DPROV.mqui_codiquin = " + numQuincena + @"  
					 order by 3 ; ";
                      /*
Select DPROV.mqui_codiquin, DPROV.memp_codiemp, DPROV.papo_codiapor, DPROV.valor, PAPOR.papo_tipoaporte,   
    				 	coalesce(PPUCAPAT.MCUE_CODIPUCDEBIAPOR,'ERROR PARAMT'),   
    				 	coalesce(PPUCAPAT.MCUE_CODIPUCCREDAPOR,'ERROR PARAMT'),   
					 	PPUCAPAT.pdep_codidpto, coalesce(pc.mnit_nit,PAPOR.mnit_nit), PAPOR.papo_nombapor,
                        coalesce(mcn.TCLA_CODIGO,'N') as claseCuentaNominal,
                        coalesce(mcr.TCLA_CODIGO,'R') as claseCuentaReal
					 from dbxschema.mempleado me 
					 	    left join dbxschema.pcajacompensacion pc on me.pcaj_codicaja = pc.pcaj_codicaja,
					 	  dbxschema.paportepatronal PAPOR , 
					 	  dbxschema.dprovapropiaciones DPROV   
					      	left join dbxschema.ppucaportepatronal PPUCAPAT on DPROV.papo_codiapor=PPUCAPAT.papo_codiapor   
                        	left join dbxschema.mcuenta mcN on mcN.MCUE_CODIPUC = PPUCAPAT.MCUE_CODIPUCDEBIAPOR
                        	left join dbxschema.mcuenta mcR on mcR.MCUE_CODIPUC = PPUCAPAT.MCUE_CODIPUCCREDAPOR
                     where me.memp_codiempl = DPROV.memp_codiemp and DPROV.memp_codiemp='" + empleados.Tables[0].Rows[i][0].ToString() + @"' and DPROV.mqui_codiquin = " + numQuincena + @"  
				  	  and DPROV.papo_codiapor=PAPOR.papo_codiapor and PAPOR.papo_tipoaporte<>3  
					  and PPUCAPAT.pdep_codidpto=me.pdep_codidpto; ";
                        */
                    DBFunctions.Request(numcuentas,IncludeSchema.NO,sqlcuentasP);

					codempleado=empleados.Tables[0].Rows[i][0].ToString();
					if (numcuentas.Tables.Count!=0 && numcuentas.Tables[0].Rows.Count!=0)
					{
						for (x=0;x<numcuentas.Tables[0].Rows.Count;x++)
						{
                            //PROCESO DEBITO
                       //     string clasecuenta=DBFunctions.SingleData("select coalesce(TCLA_CODIGO,'N') from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][5].ToString()+"' ");
                            string clasecuenta = numcuentas.Tables[0].Rows[x]["claseCuentaNominal"].ToString();
                            nitEmpleado = empleados.Tables[0].Rows[i][17].ToString();
                            nitEmpresa  = empleados.Tables[0].Rows[i][16].ToString();
                            nitAporte   = numcuentas.Tables[0].Rows[x][8].ToString();
                            if (clasecuenta=="N")
							{
								double sumatemp=0;
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,
                                      @"select ME.memp_codiempl, COALESCE(MC.pcen_codigo,'ERROR CCOSTO'), COALESCE(MC.mecc_porcentaje,100), COALESCE(MC.palm_almacen,'ERROR SEDE'), '' as pcon_concepto 
                                        from dbxschema.mempleado ME LEFT JOIN dbxschema.mempleadopcentrocosto MC ON  ME.memp_codiempl = MC.memp_codiempl 
                                        WHERE ME.memp_codiempl='" + numcuentas.Tables[0].Rows[x][1].ToString() + @"'
                                      UNION
                                      select ME.memp_codiempl, COALESCE(MC.pcen_codigo,'ERROR CCOSTO'), COALESCE(MC.mecc_porcentaje,100), COALESCE(Me.palm_almacen,'ERROR SEDE'), pcon_concepto 
                                        from dbxschema.mempleado ME, dbxschema.mempleadopcentrocostoconcepto  MC
                                       WHERE ME.memp_codiempl = MC.memp_codiempl and ME.memp_codiempl='" + numcuentas.Tables[0].Rows[x][1].ToString() + "' ");

                                if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "  ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes.Interfase Parafiscales");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									//debito***************
									if (y==porcentajes.Tables[0].Rows.Count-1)
									{
										double ajuste=Math.Round(Convert.ToDouble(numcuentas.Tables[0].Rows[x][3])-sumatemp, 2);
										valorcentrocosto=ajuste;
									}
									else
									{
                                        valorcentrocosto = 0;
                                        distxConcepto = false;
                                        for (h = 0; h < porcentajes.Tables[0].Rows.Count; h++)
                                        {
                                            if (numcuentas.Tables[0].Rows[x][2].ToString() == porcentajes.Tables[0].Rows[h][4].ToString())
                                                distxConcepto = true;
                                        }
                                        if (numcuentas.Tables[0].Rows[x][2].ToString() == porcentajes.Tables[0].Rows[y][4].ToString() && distxConcepto) // Si aplica distribución por concepto específico
                                            valorcentrocosto = Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString()) * double.Parse(porcentajes.Tables[0].Rows[y][2].ToString()) / 100;
                                        else if (porcentajes.Tables[0].Rows[y][4].ToString() == "" && !distxConcepto)
                                                valorcentrocosto = Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString()) * double.Parse(porcentajes.Tables[0].Rows[y][2].ToString()) / 100;

								//		valorcentrocosto=Math.Round(Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString()),0)* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);					
									}
  
									//Validar nit para Fpens. y EPs
									sumatemp+=valorcentrocosto;
									if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")    //  pension
										nit=empleados.Tables[0].Rows[i][6].ToString();
									else if (numcuentas.Tables[0].Rows[x][4].ToString()=="6")   //  salud
											nit=empleados.Tables[0].Rows[i][3].ToString();
										 else
											nit=numcuentas.Tables[0].Rows[x][8].ToString();
									sumatotaldebito += valorcentrocosto;							
									string razon = numcuentas.Tables[0].Rows[x][9].ToString()+ " " + DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][10].ToString();
							 	//	if(nitxEmpresa)
                                //        nit = nitEmpresa;  // el gasto va al nit de la empresa cuando se parametrice en el web.config
                                   if(valorcentrocosto != 0)
                                        this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text,numcuentas.Tables[0].Rows[x][5].ToString(),ddlprefPara.SelectedValue, txtNumPara.Text, nit ,almacen.ToString(),porcentajes.Tables[0].Rows[y][1].ToString(),razon,valorcentrocosto.ToString(),"0","0");
                                    totalDebito += valorcentrocosto;
                                }
							}
							else
							{
								//debito***************
								
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
								valorcentrocosto=Math.Round(valorcentrocosto,2);
								//Validar nit para Fpens. y EPs
								if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")
                                {
                                    if (nitxEmpresa)
                                        nit = empleados.Tables[0].Rows[i][6].ToString();
                                    else
                                        nit = empleados.Tables[0].Rows[i][17].ToString();
                                }
                                else if (numcuentas.Tables[0].Rows[x][4].ToString() == "6")   //  salud
                                {
                                    if (nitxEmpresa)
                                        nit = empleados.Tables[0].Rows[i][3].ToString();
                                    else
                                        nit = empleados.Tables[0].Rows[i][17].ToString();
                                }
                                else
                                    nit =numcuentas.Tables[0].Rows[x][8].ToString();
								sumatotaldebito+=valorcentrocosto;
								string razon =numcuentas.Tables[0].Rows[x][9].ToString()+ " " + DDLANO.SelectedValue+DDLMES.SelectedValue;
								razon = razon+" "+empleados.Tables[0].Rows[i][10].ToString();
								this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text,numcuentas.Tables[0].Rows[x][6].ToString(),ddlprefPara.SelectedValue, txtNumPara.Text,nit,almacen.ToString(),centCosto,razon,valorcentrocosto.ToString(),"0","0");
                                totalDebito += valorcentrocosto;
                            }//fin else

							//PROCESO CREDITO
						//	clasecuenta=DBFunctions.SingleData("select coalesce(TCLA_CODIGO,'R') from dbxschema.mcuenta where MCUE_CODIPUC='"+numcuentas.Tables[0].Rows[x][6].ToString()+"' ");
                            clasecuenta = numcuentas.Tables[0].Rows[x]["claseCuentaReal"].ToString();
                            if (clasecuenta=="N")
							{
								double sumatemp=0;
								DataSet porcentajes=new DataSet();
								//sacar los porcentajes
								DBFunctions.Request(porcentajes,IncludeSchema.NO,"select memp_codiempl,pcen_codigo,mecc_porcentaje from dbxschema.mempleadopcentrocosto where memp_codiempl='"+numcuentas.Tables[0].Rows[x][1].ToString()+"'");
								if (porcentajes.Tables[0].Rows.Count==0)
								{
                                    Utils.MostrarAlerta(Response, "   ATENCION: Al empleado  " + numcuentas.Tables[0].Rows[x][1].ToString() + " no se le han definido porcentajes.Interfase Parafiscales");
								}
								for (y=0;y<porcentajes.Tables[0].Rows.Count;y++)
								{
									if (y==porcentajes.Tables[0].Rows.Count-1)
									{
										double ajuste=Math.Round(Convert.ToDouble(numcuentas.Tables[0].Rows[x][3])-sumatemp, 2);
										valorcentrocosto=ajuste;
									}
									else
									{
										valorcentrocosto=(Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())) * double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
										valorcentrocosto=Math.Round(valorcentrocosto,2);
									}
									valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString())* double.Parse(porcentajes.Tables[0].Rows[y][2].ToString())/100;
									//Validar nit para Fpens. y EPs
									sumatemp+=valorcentrocosto;
									if(numcuentas.Tables[0].Rows[x][4].ToString()=="5")
                                    {
                                        if (nitxEmpresa)
                                            nit = empleados.Tables[0].Rows[i][6].ToString();
                                        else
                                            nit = empleados.Tables[0].Rows[i][17].ToString();
                                    }
                                    else if (numcuentas.Tables[0].Rows[x][4].ToString() == "6")   //  salud
                                    {
                                        if (nitxEmpresa)
                                            nit = empleados.Tables[0].Rows[i][3].ToString();
                                        else
                                            nit = empleados.Tables[0].Rows[i][17].ToString();
                                    }
                                    else
                                        nit =numcuentas.Tables[0].Rows[x][8].ToString();
									
									string razon =numcuentas.Tables[0].Rows[x][9].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
									razon = razon+" "+empleados.Tables[0].Rows[i][10].ToString();
									this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue,txtNumPara.Text,numcuentas.Tables[0].Rows[x][6].ToString(),ddlprefPara.SelectedValue, txtNumPara.Text,nit,almacen.ToString(), porcentajes.Tables[0].Rows[y][1].ToString(),razon,"0",valorcentrocosto.ToString(),"0");
                                    totalCredito += valorcentrocosto;
                                }
							}
							else
							{
								// Credito***********************
								valorcentrocosto=Double.Parse(numcuentas.Tables[0].Rows[x][3].ToString());
								valorcentrocosto=Math.Round(valorcentrocosto,2);
                                string cuentaPUCfe = numcuentas.Tables[0].Rows[x][6].ToString();
                                // Validar nit para Fpens. y EPs
                                if (numcuentas.Tables[0].Rows[x][4].ToString() == "5")
                                {
                                    if (!nitxEmpresa)
                                        nit = empleados.Tables[0].Rows[i][11].ToString(); //  empleado
                                    else
                                        nit = empleados.Tables[0].Rows[i][6].ToString(); //  entidad
                                    if (empleados.Tables[0].Rows[i][13].ToString().Trim() != "")
                                        cuentaPUCfe = empleados.Tables[0].Rows[i][13].ToString();
                                }
                                else if (numcuentas.Tables[0].Rows[x][4].ToString() == "6")
                                     {
                                        if (!nitxEmpresa)
                                            nit = empleados.Tables[0].Rows[i][11].ToString();  //  empleado
                                    else
                                            nit = empleados.Tables[0].Rows[i][3].ToString();  //  entidad
                                        if (empleados.Tables[0].Rows[i][12].ToString().Trim() != "")
                                            cuentaPUCfe = empleados.Tables[0].Rows[i][12].ToString();
                                     }
                                     else
                                        nit = numcuentas.Tables[0].Rows[x][8].ToString();
                                //if (nitxEmpresa)
                                //     nit = nitEmpresa;
                                string razon =numcuentas.Tables[0].Rows[x][9].ToString()+" "+DDLANO.SelectedValue+DDLMES.SelectedValue;
								razon = razon+" "+empleados.Tables[0].Rows[i][10].ToString();
								this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text,cuentaPUCfe,ddlprefPara.SelectedValue, txtNumPara.Text,nit,almacen.ToString(), centCosto,razon,"0",valorcentrocosto.ToString(),"0");
                                totalCredito += valorcentrocosto;
                            }
						}
					}
				}
			}
            totalDebito -= totalCredito;
            if(totalDebito != 0)
                this.Ingresar_Datos_Detalles(ddlprefPara.SelectedValue, txtNumPara.Text, ajustePeso.Tables[0].Rows[0][0].ToString(), ddlprefPara.SelectedValue, txtNumPara.Text, nit, ajustePeso.Tables[0].Rows[0][1].ToString(), ajustePeso.Tables[0].Rows[0][2].ToString(), "Ajuste al Peso", totalDebito.ToString(),"0", "0");

            this.Ingresar_Datos_Cabecera(ddlprefPara.SelectedValue,txtNumPara.Text,DDLANO.SelectedValue,DDLMES.SelectedValue,txtFechaComprobante.Text,txtRazon.Text,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),susuario,Math.Round(sumatotaldebito,0).ToString());			
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
