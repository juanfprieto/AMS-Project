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
using AMS.Documentos;
using AMS.Contabilidad;
using AMS.Tools;
using System.Data.OleDb;
using AMS.Reportes;
using System.Globalization;

namespace AMS.Inventarios
{
	public partial class RecepcionItems : System.Web.UI.UserControl
	{
		#region Atributos
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		protected ArrayList types = new ArrayList();
		protected ArrayList lbFields = new ArrayList();
		protected Label     lblTipoOrden, lblNumOrden, lblTipoPedido,  lblCargo;
		protected DropDownList ddlNumP;
		protected TextBox   txtTotAsig;
		protected DataTable dtSource;
		protected DataSet   ds,dsR;
		protected bool      facRealizado = false;
        protected string    TipoSociedad = "";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private DatasToControls bind = new DatasToControls();
		private FormatosDocumentos formatoFactura=new FormatosDocumentos();
        ProceHecEco contaOnline = new ProceHecEco();
        protected int numDecimales = 0;

        #endregion

        #region Eventos
        //LOAD--------------------------------------------------------
        protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Inventarios.RecepcionItems));
            //this.ClearChildViewState();

            //System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //customCulture.NumberFormat.NumberDecimalSeparator = ",";
            //System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            try { numDecimales = Convert.ToInt16(DBFunctions.SingleData("select cinv_numedeci from cinventario")); }
            catch (Exception er) { numDecimales = 0; }

            if (!IsPostBack)
			{
				Session.Clear();
				dgItemsPRec.EditItemIndex = dgItemsRDir.EditItemIndex = dgItemsLeg.EditItemIndex = -1;
                bind.PutDatasIntoDropDownList(ddlAlmacen, Almacen.ALMACENES);
                if (ddlAlmacen.Items.Count > 1)
                    ddlAlmacen.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));
                txtNumFacE.Text = "0";
                if (ddlPrefE.Items.Count > 1)
                    ddlPrefE.Items.Insert(0, "Seleccione..");
                else
                    if (ddlPrefE.Items.Count == 1)
                        txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
          
                bind.PutDatasIntoDropDownList(ddlVendedor, string.Format(Almacen.VENDEDORESPORALMACEN, ddlAlmacen.SelectedValue));
                if (ddlVendedor.Items.Count > 1)
                    ddlVendedor.Items.Insert(0, "Seleccione..");
                bind.PutDatasIntoDropDownList(ddlPIVA, "SELECT piva_porciva, piva_decreto FROM piva ORDER BY piva_porciva");
				bind.PutDatasIntoDropDownList(ddlTipoPre,string.Format(Documento.DOCUMENTOSTIPO,"PI"));
				IFormatProvider culture = new System.Globalization.CultureInfo("es-CO", true);
				tbDate.Text = DateTime.Now.GetDateTimeFormats()[6];
				calDate.SelectedDate=DateTime.Now;
                //txtFlet.Attributes.Add("onkeyup", "NumericMask(" + txtFlet.ClientID + ");");
				txtFlet.Attributes.Add("onkeyup", "CalculoIva("+txtFlet.ClientID+","+ddlPIVA.ClientID+","+txtTotIF.ClientID+",'"+txtTotal.ClientID+"','"+txtGTot.ClientID+"');");
				ddlPIVA.Attributes.Add("onchange","CambioIva("+txtFlet.ClientID+","+ddlPIVA.ClientID+","+txtTotIF.ClientID+",'"+txtTotal.ClientID+"','"+txtGTot.ClientID+"');");
                txtNIT.Attributes.Add ("onblur",  "CargaNITPRO(this)");
                imgLupa.Attributes.Add("onclick", "ModalDialog(" + txtNIT.ClientID + ", 'Select t1.mnit_nit as NIT, t1.mnit_nombres concat \\' \\' concat t1.mnit_apellidos as Nombre from MNIT as t1,MPROVEEDOR as t2 where t1.mnit_nit=t2.mnit_nit ORDER BY Nombre', new Array());");
                
                if (Request.QueryString["mret"] != null)
                    Utils.MostrarAlerta(Response, "Las retenciones han sido modificadas.");
                btnAjus.Enabled = false;
                if(Request.QueryString["prR"]!=null)
				{
					//Si es un prerecepción
					if(Request.QueryString["prR"]=="0")
					{
                        Utils.MostrarAlerta(Response, "Se ha generado la prerecepción con prefijo " + Request.QueryString["pref"] + " y número " + Request.QueryString["num"] + "");
						formatoFactura=new FormatosDocumentos();
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
							lbInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}
					}
					//Si es una recepción
					else if(Request.QueryString["prR"]=="1")
					{
						formatoFactura=new FormatosDocumentos();
                        Utils.MostrarAlerta(Response, "Se ha generado la factura con prefijo " + Request.QueryString["pref"] + " y número " + Request.QueryString["num"] + "");
						try
						{
							formatoFactura.Prefijo = Request.QueryString["pref"];
							formatoFactura.Numero = Convert.ToInt32(Request.QueryString["num"]);
							formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["pref"]+"'");
							if(formatoFactura.Codigo!=string.Empty)
							{
								if(formatoFactura.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=700');</script>");
							}
                            formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pref"] + "'");
                            if (formatoFactura.Codigo != string.Empty)
                            {
                                if (formatoFactura.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=550,WIDTH=750');</script>");
                            }
						}
						catch
						{
							lbInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}
                        //  contabilizacion de la entrada
                        contaOnline.contabilizarOnline(Request.QueryString["pref"].ToString(), Convert.ToInt32(Request.QueryString["num"].ToString()), DateTime.Now, "");
					}
					//Si es una legalización
					else if(Request.QueryString["prR"]=="2")
					{
						formatoFactura=new FormatosDocumentos();
                        Utils.MostrarAlerta(Response, "Se ha generado la factura con prefijo " + Request.QueryString["pref"] + " y número " + Request.QueryString["num"] + "");
						try
						{
							formatoFactura.Prefijo=Request.QueryString["pref"];
							formatoFactura.Numero=Convert.ToInt32(Request.QueryString["num"]);
							formatoFactura.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["pref"]+"'");
							if(formatoFactura.Codigo!=string.Empty)
							{
								if(formatoFactura.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoFactura.Documento+"','','HEIGHT=600,WIDTH=700');</script>");
							}
                            formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pref"] + "'");
                            if (formatoFactura.Codigo != string.Empty)
                            {
                                if (formatoFactura.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                            }
						}
						catch
						{
							lbInfo.Text="Error al generar el formato. Detalles : <br>"+formatoFactura.Mensajes;
						}
                        //  contabilizacion de la entrada
                        contaOnline.contabilizarOnline(Request.QueryString["pref"].ToString(), Convert.ToInt32(Request.QueryString["num"].ToString()), DateTime.Now, "");
					}
				}
			}
			if((DataTable)Session["dtInsertsRI"] != null)
				dtSource = (DataTable)Session["dtInsertsRI"];
			else
			{
				LoadDataColumns();
				LoadDataTable();
			}
			uint numProc     = 1;
			plhFacE.Visible  = plhFacF.Visible = true;
			plhOpLeg.Visible = plhFile.Visible = plhEmbarque.Visible = false;
       //     btnAjus.Enabled = true;
			if(ddlTProc.SelectedValue == "0")
			{	//prerecepcion
				try{numProc=Convert.ToUInt32(DBFunctions.SingleData("SELECT max(mpre_numero + 1) FROM mprerecepcion"));}
				catch{};
				plhNTPre.Visible = true;
				txtPreRe.Text = numProc.ToString();
		//		plhFacE.Visible = plhFacF.Visible=false;
			}
			{
				plhNTPre.Visible=false;
				if(ddlTProc.SelectedValue=="2")
					plhOpLeg.Visible=true;
			}

            plhFile.Visible = dgItemsRDir.Visible;
		}

        [Ajax.AjaxMethod()]
        public string ConsultarNombreCliente(string nitCliente)
        {
            string nombre = DBFunctions.SingleData("SELECT CASE WHEN TNIT_TIPONIT = 'N' THEN t1.mnit_apellidos ELSE t1.mnit_nombres concat ' ' concat COALESCE(t1.mnit_nombre2,'') concat ' ' concat t1.mnit_apellidos concat ' ' concat COALESCE(t1.mnit_apellido2,'') END AS Nombre FROM MNIT AS t1, MPROVEEDOR AS t2 WHERE t1.mnit_nit = t2.mnit_nit and t1.mnit_nit = '" + nitCliente + "'");
            return nombre;
        }

        [Ajax.AjaxMethod()]
        public string ConsultarPlazo(string nitCliente)
        {
            string diasPlazo = DBFunctions.SingleData("SELECT MPRO_DIASPLAZO FROM MPROVEEDOR WHERE MNIT_NIT = '" + nitCliente + "'");
            if (diasPlazo == "")
                diasPlazo = "0";
     //         txtPlazo.Text = diasPlazo;
            return diasPlazo;
        }

        protected void CambiaProceso(Object Sender, EventArgs E)
        {
            if(ddlTProc.SelectedValue == "0")
                bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "PI", "IP", ddlAlmacen.SelectedValue));
            else
                bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));
            if (ddlPrefE.Items.Count > 1)
                ddlPrefE.Items.Insert(0, "Seleccione..");
            if (ddlPrefE.SelectedValue != "Seleccione..")
                txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
            else
                txtNumFacE.Text = "0";
        }

        //CAMBIAR EL NIT(CONFIRMAR)------------------------------
        protected void CambiaNIT(Object Sender, EventArgs E)
        {
            string processMsg = "";
            bool error = false;
            if (ddlTProc.SelectedValue == "-1")
            {
                processMsg += "Seleccione un proceso \\n";
                error = true;
            }
            if (ddlAlmacen.SelectedValue == "Seleccione.." )
            {
                processMsg += "Seleccione un Almacén válido !  \\n";
                error = true;
            }
            if (ddlVendedor.SelectedValue == "Seleccione..")
            {
                processMsg += "Seleccione un Vendedor (Responsable) válido !  \\n";
                error = true;
            }
            if (ddlPrefE.SelectedValue == "Seleccione.." || ddlPrefE.SelectedValue == "" || ddlPrefE.SelectedValue == null)
            {
                processMsg += "Seleccione un Documento para la entrada de Almacén válido !  \\n";
                error = true;
            }
            else if (Convert.ToUInt32(txtNumFacE.Text) > Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_numefina FROM pdocumento WHERE pdoc_codigo='" + ddlPrefE.Text + "'")))
            { 
                    processMsg += "El número de entrada especificado se encuentra fuera del rango permitido para el prefijo " + ddlPrefE.Text + "  \\n";
                    error = true;
            }
            string nacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "';");
            if (txtNIT.Text.Trim().Length == 0 | txtNIT.Text.Trim().Length>0 && nacionalidad.Length==0)
            {
                processMsg += "No existe la identificación del Proveedor !  \\n";
                error = true;
            }
            if (txtPref.Text.Trim().Length == 0 || txtNumFac.Text.Trim().Length == 0 || txtNumFac.ToString() == "0")
            {
                processMsg += "Prefijo y-o número de la Factura del Proveedor está errado !  \\n";
                error = true;
            }
            else if (ddlTProc.SelectedValue != "0" && DBFunctions.RecordExist("SELECT mfac_numedocu FROM mfacturaproveedor WHERE mfac_prefdocu='" + txtPref.Text.Trim() + "' AND mfac_numedocu=" + txtNumFac.Text.Trim() + " and mnit_nit='" + txtNIT.Text + "';"))
                //Se debe revisar si no existe esa factura de proveedor dentro de mfacturaproveedor
            {
                processMsg += "La factura del proveedor " + txtNIT.Text + " con prefijo-número : " + txtPref.Text.Trim() + "-" + txtNumFac.Text.Trim() + " ya se ha ingresado anteriormente. \\n";
                error = true; 
            }

            ViewState["NACIONALIDAD"]=nacionalidad;
			if(nacionalidad=="E" && !chkImportacion.Checked)
            {
                processMsg += "El PROVEEDOR es extranjero y no ha marcado la importación !  \\n";
                error = true;
            }
            if (nacionalidad != "E" && chkImportacion.Checked)
            {
                processMsg += "El PROVEEDOR NO es extranjero y marcó la importación !  \\n";
                error = true;
            }
            //validar las retenciones para este nit
            TipoSociedad = DBFunctions.SingleData("SELECT TSOC_SOCIEDAD FROM MNIT WHERE MNIT_NIT='"+txtNIT.Text+"'");//Proveedr???????
            if (TipoSociedad == "A" || TipoSociedad == "L" || TipoSociedad == "C" || TipoSociedad == "S")
				TipoSociedad = "J";
			else TipoSociedad = "N";
			// Validación de la tabla de RETENCIONES en la fuente. Solo debe haber una retención por cada proceso para el tipo de sociedad(=persona)
			dsR=new DataSet();
			DBFunctions.Request(dsR,IncludeSchema.NO,
                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='R' AND tret_codigo='RF';" +
                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='L' AND tret_codigo='RF';" +
                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='S' AND tret_codigo='RF';" +
			    "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='R' AND tret_codigo='RI';" +
			    "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='L' AND tret_codigo='RI';" +
			    "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='S' AND tret_codigo='RI';" +
			    "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='R' AND tret_codigo='RC';" +
			    "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='L' AND tret_codigo='RC';" +
			    "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='S' AND tret_codigo='RC';" +
                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad +"' AND ttip_proceso='R' AND tret_codigo='RE';"
                );
            if (dsR.Tables[0].Rows.Count != 1)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de RENTA para el proceso REPUESTOS para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[1].Rows.Count!=1)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de RENTA para el proceso LUBRICANTES para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[2].Rows.Count!=1)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de RENTA para el proceso SERVICIOS (Trabajos de Terceros) para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[3].Rows.Count!=1)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de IVA para el proceso REPUESTOS para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[4].Rows.Count!=1)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de IVA para el proceso LUBRICANTES para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[5].Rows.Count!=1)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de IVA para el proceso SERVICIOS (Trabajos de Terceros) para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
            bool retIca = Convert.ToBoolean(DBFunctions.RecordExist("SELECT TRET_CODIGO FROM TRETENCION WHERE TRET_CODIGO = 'RC';" ));
			if (dsR.Tables[6].Rows.Count!=1 && retIca)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de ICA para el proceso REPUESTOS para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[7].Rows.Count!= 1 && retIca)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de ICA para el proceso LUBRICANTES para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
			if (dsR.Tables[8].Rows.Count!= 1 && retIca)
			{
                processMsg += "Debe definir Solamente UN registro de RETENCION en la FUENTE de ICA para el proceso SERVICIOS (Trabajos de Terceros) para las sociedades tipo " + TipoSociedad + "  \\n";
                error = true;
            }
            //if (dsR.Tables[9].Rows.Count != 1)
            //{
            //    Utils.MostrarAlerta(Response, "Debe definir Solamente UN registro de RETENCION en la FUENTE de CREE para el proceso de compra REPUESTOS para las sociedades tipo " + TipoSociedad + "");
            //    return;
            //}

            if (!error)
            {
                BindDatas(true);
                txtNIT.ReadOnly = true;
                txtNIT.Attributes.Remove("ondblclick");
                txtNITa.Text = DBFunctions.SingleData("Select COALESCE(mnit_nombres,'') concat ' ' concat COALESCE(mnit_apellidos,'') from MNIT where mnit_nit='" + txtNIT.Text + "';");
                btnSelecNIT.Visible = false;
            }
            else
            {
                Utils.MostrarAlerta(Response, processMsg);
            }
		}

        protected void OnChangeTipoCarga(Object Sender, EventArgs E)
        {
            switch (ddlTipoCargaExcel.SelectedValue)
            {
                case "D":
                    txtPrefijoExcel.Visible = false;
                    txtPrefijoExcel.Text = "";
                    break;
                case "F":
                    txtPrefijoExcel.Visible = true;
                    break;
            }
            plhFile.Visible = true;
        }

		//CARGAR ARCHIVO--------------------------------------------	
		protected void CargaArchivo(Object Sender, EventArgs E)
		{
			if(File1.PostedFile==null)
			{
                Utils.MostrarAlerta(Response, "No se existe el archivo!");
				return;
			}
			//Leer
            // hay que mostrar la información del tipo de archivo y estructua que debe contener para poder cargarlo y leerlo
			string StrFileName=File1.PostedFile.FileName.Substring(File1.PostedFile.FileName.LastIndexOf("\\") + 1) ;
			string StrFileType = File1.PostedFile.ContentType ;
			int IntFileSize=File1.PostedFile.ContentLength;
			if(IntFileSize <=0)
			{
                Utils.MostrarAlerta(Response, "No se pudo cargar el archivo!");
				return;
			}
			string Arch=Server.MapPath(".\\../imp/excel\\" +StrFileName);  //  ESTA RUT ETA MAL... deb ser IMP donde quedan todos los archivos a subir
			File1.PostedFile.SaveAs(Arch);//Guardar
			//Leer archivo
			FileInfo archF=new FileInfo(Arch);
            DataSet dataset = new DataSet();

            switch (ddlTipoCargaExcel.SelectedValue)
            {
                case "D":
                    dataset = Utils.CargaExcel(File1, "TABLA");
                    if(dataset == null)
                    {
                        Utils.MostrarAlerta(Response, "No se pudo cargar el archivo! Revisar Excel.");
                        return;
                    }
                    //string myConnection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + archF + ";Extended Properties=\"Excel 12.0 Xml;HDR=NO;IMEX=1\"";
                    //OleDbConnection conn = new OleDbConnection(myConnection);
                    //string strSQL = "SELECT * FROM TABLA";
                    //OleDbCommand cmd = new OleDbCommand(strSQL, conn);
                    //OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                    //adapter.Fill(dataset);
                    break;
                case "F":
                    ExtractorFacturaExcel extractor = new ExtractorFacturaExcel();
                    DataTable dtExcelFactura = extractor.generar(archF);
                    dataset.Tables.Add(dtExcelFactura);
                    break;
            }

            String errores = "";
            InsertarFilaSDataSet(dataset, ref errores);
            if (errores != "")
            {
                lblErr.Text = errores;
                divErr.Visible = true;
            }
            else
            {
                divErr.Visible = false;
            }
            
            //GridViewXYZ.DataSource = dataset; 
            //GridViewXYZ.DataBind();

            //StreamReader stream = archF.OpenText();
			//string linT;
            //linT = stream.ReadLine();
			//int c=0;
			//while(linT!=null)
			//{
                //string[]palT=linT.Split(new Char[]{(char)9});
                //if(palT.Length==7)
                //    if(InsertarFila(palT))
                //        c++;
				//linT=stream.ReadLine();
			//}
			//stream.Close();
			
            //Cargar tabla
            //if(c==0)
            //    Utils.MostrarAlerta(Response, "No se pudo cargar ninguna fila del archivo!");
			//Eliminar archivo
			//File.Delete(Arch);
			BindDatas(true);
		}
		
		//SELECCIONAR EMBARQUE
		protected void SeleccionarEmbarque(Object Sender, EventArgs E)
		{
			DataSet dsEmbarque = new DataSet(); 
			DataTable dtEmbarque;
			DataRow dr;
			string embarque=ddlEmbarque.SelectedValue,tGE,tGN;
			double tasaCambioN,tasaCambioI;
			double valor=0,gravamen,iva,totalValoresCIF=0,totalEmbarqueCIF=0,factorCIF,factorImportacion=0,totalGastosE=0,totalGastosN=0,totalIVA=0,totalEmbarque=0,totalGravamen=0,totalCostoImportacion=0;
			double valorEmbarqueM=Convert.ToDouble(DBFunctions.SingleData(
				"SELECT MLIC_VALOEMBA FROM MEMBARQUE WHERE MEMB_SECUENCIA="+ddlEmbarque.SelectedValue+";"));
            double valorEmbarqueItems = Convert.ToDouble(DBFunctions.SingleData(
                "SELECT SUM(DITE_CANTEMBA*DITE_VALOFOB) FROM DEMBARQUE WHERE MEMB_SECUENCIA=" + ddlEmbarque.SelectedValue + ";"));

            if(Math.Abs(valorEmbarqueM - valorEmbarqueItems) > 1)
            {
                Utils.MostrarAlerta(Response, "El valor declarado en el Embarque " + valorEmbarqueM + " No coincide con la sumaria de los items del embarque que es " + valorEmbarqueItems + ". Proceso NO permitido");
                BindDatas(false);
                return;  
            }

            try { tasaCambioI=Convert.ToDouble(txtTasaCambioI.Text);if(tasaCambioI<=0)throw(new Exception());}
            catch
            {
                Utils.MostrarAlerta(Response, "Tasa de cambio importación no válida!");BindDatas(false);return;
            }
			
			try{tasaCambioN=Convert.ToDouble(txtTasaCambio.Text);if(tasaCambioN<=0)throw(new Exception());}
            catch
            {
                Utils.MostrarAlerta(Response, "Tasa de cambio nacionalización no válida!");BindDatas(false);return;
            }
			
			if(embarque.Length==0){BindDatas(false);return;}
			DBFunctions.Request(dsEmbarque,IncludeSchema.NO,
				"SELECT * FROM DEMBARQUE WHERE MEMB_SECUENCIA="+embarque+";");
			dtEmbarque=dsEmbarque.Tables[0];

			//Cargar gastos
			DataSet dsGastos=new DataSet();
			DBFunctions.Request(dsGastos,IncludeSchema.NO,
				"Select df.PDOC_CODIORDEPAGO PREFO,df.MFAC_NUMEORDEPAGO NUMO,df.pgas_codigo,pg.PGAS_NOMBRE gasto,sum(df.dfac_valorgasto) valor, pg.PGAS_MODENACI "+
				"from DFACTURAGASTOEMBARQUE df, PGASTODIRECTO pg "+
				"where memb_secuencia="+ddlEmbarque.SelectedValue+" and pg.PGAS_CODIGO=df.PGAS_CODIGO "+
				"group by memb_secuencia,df.pgas_codigo,pgas_nombre,df.PDOC_CODIORDEPAGO,df.MFAC_NUMEORDEPAGO,pg.PGAS_MODENACI;");
			dgrGastos.DataSource=dsGastos.Tables[0];
			dgrGastos.DataBind();

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

			string sqlLiquidacion;
			//Agregar Items
			dtSource.Rows.Clear();

			//Primera pasada:
			//Valor=FOB*Cantidad*(1+gravamen/100)
			for(int n=0;n<dtEmbarque.Rows.Count;n++)
			{
				dr=dtSource.NewRow();
				//0 Numero pedido o Prerecepcion
				dr[0]="1";
				//1 Item
				dr[1]=dtEmbarque.Rows[n]["mite_codigo"].ToString();
				//2 Nombre Item
				dr[2]=DBFunctions.SingleData("SELECT MITE_NOMBRE ||' Arancel '|| coalesce(m.para_codigo,' No ') ||' grav '|| coalesce(para_gravame,0) ||' iva '|| coalesce(p.piva_porciva,0) FROM MITEMS M  LEFT JOIN PARANCEL P ON M.PARA_CODIGO = P.PARA_CODIGO WHERE MITE_CODIGO='" + dr[1].ToString()+"';");
				//3 Cantidad Ingresada
				dr[3]=Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);
				//4 Cantidad Facturada
				dr[4]=Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);
				//5 Calcular Valor Cif
				//valor=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"])*Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"])*(1+(gravamen/100));
				valor=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"])*Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);
				totalEmbarque+=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"])*Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);

				dr[5]=valor;
				//Valor US
				dr[12]=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"]);
				//valor+=valor*gravamen/100*;
				//6 Descuento
				dr[6]=0;
				dr[7]=0;
				//8 Ubicacion
				dr[8]="";
				//10 Unidad medida
				dr[10]="";
				//11 Linea de Bodega
				dr[11]="";
				dtSource.Rows.Add(dr);
			}
			//Factor CIF: gastos extranjeros / totalvalores CIF
	//ha		totalValoresCIF+=totalGastosE;

			//Segunda pasada
			//Valor= valor*factorCIF
			for(int n=0;n<dtSource.Rows.Count;n++)
			{
				dr=dtSource.Rows[n];
				valor=Convert.ToDouble(dr[5]);
				//Valor CIF
				valor=valor+(totalGastosE*(valor/totalEmbarque));
	//hac2			dr[5]=valor;
				totalEmbarqueCIF+=valor;

				//Gravamen
				gravamen=Convert.ToDouble(DBFunctions.SingleData(
					"SELECT COALESCE(PA.PARA_GRAVAME,0) FROM MITEMS MI LEFT JOIN PARANCEL PA ON PA.PARA_CODIGO=MI.PARA_CODIGO WHERE MI.MITE_CODIGO='"+dr[1].ToString()+"';"));
				gravamen=valor*(gravamen/100)*tasaCambioI;
				
				//Iva
				//7 IVA viene de PARANCEL
				iva=Convert.ToDouble(DBFunctions.SingleData(
					"SELECT COALESCE(PA.PIVA_PORCIVA,0) FROM MITEMS MI LEFT JOIN PARANCEL PA ON PA.PARA_CODIGO=MI.PARA_CODIGO WHERE MI.MITE_CODIGO='"+dr[1].ToString()+"';"));
				iva=iva/100;
				iva=(gravamen+(tasaCambioI*(valor))) * iva;
				//Response.Write("<script language='javascript'>alert('Gravamen: "+dr[1]+" : "+gravamen+"    IVA: "+iva+"');</script>");

				totalGravamen+=gravamen;
				totalIVA+=iva;
				totalValoresCIF+=valor;
				//iva = 0 para el movimiento
				dr[7]=0;
			}
			factorCIF=1+(totalGastosE/totalValoresCIF);
			totalGastosN+=totalGravamen;
			totalCostoImportacion=((totalEmbarque+totalGastosE)*tasaCambioN)+totalGastosN;
            factorImportacion = totalCostoImportacion / totalEmbarque ;
			
			for(int n=0;n<dtSource.Rows.Count;n++)
			{
				dr=dtSource.Rows[n];
				valor=Convert.ToDouble(dr[5]);
				valor=valor*factorImportacion;
				//9 Total
				dr[9]=valor;
				//Valor unitario
				dr[5]=valor/Convert.ToDouble(dr[3]);
			}

			#region Anterior
			/*
			//Primera pasada:
			//Valor=FOB*Cantidad*(1+gravamen/100)
			for(int n=0;n<dtEmbarque.Rows.Count;n++){
				dr=dtSource.NewRow();
				//0 Numero pedido o Prerecepcion
				dr[0]="1";
				//1 Item
				dr[1]=dtEmbarque.Rows[n]["mite_codigo"].ToString();
				//2 Nombre Item
				dr[2]=DBFunctions.SingleData("SELECT MITE_NOMBRE FROM MITEMS WHERE MITE_CODIGO='"+dr[1].ToString()+"';");
				//3 Cantidad Ingresada
				dr[3]=Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);
				//4 Cantidad Facturada
				dr[4]=Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);
				gravamen=Convert.ToDouble(DBFunctions.SingleData(
					"SELECT COALESCE(PA.PARA_GRAVAME,0) "+
					"FROM MITEMS MI LEFT JOIN PARANCEL PA ON PA.PARA_CODIGO=MI.PARA_CODIGO "+
					"WHERE MI.MITE_CODIGO='"+dr[1].ToString()+"';"));
				//5 Calcular Valor Cif
				valor=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"])*Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"])*(1+(gravamen/100));
				totalGravamen+=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"])*Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"])*(gravamen/100);
				totalEmbarque+=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"])*Convert.ToDouble(dtEmbarque.Rows[n]["dite_cantemba"]);

				dr[5]=valor;
				//Valor US
				dr[12]=Convert.ToDouble(dtEmbarque.Rows[n]["dite_valofob"]);
				//valor+=valor*gravamen/100*;
				totalValoresCIF+=valor;
				//6 Descuento
				dr[6]=0;
				dr[7]=0;
				//8 Ubicacion
				dr[8]="";
				//10 Unidad medida
				dr[10]="";
				//11 Linea de Bodega
				dr[11]="";
				dtSource.Rows.Add(dr);
			}
			//Factor CIF: gastos extranjeros / totalvalores CIF
			factorCIF=1+(totalGastosE/totalValoresCIF);
			totalGravamen=totalGravamen*factorCIF*tasaCambioN;
			//Segunda pasada
			//Valor= valor*factorCIF
			for(int n=0;n<dtSource.Rows.Count;n++){
				dr=dtSource.Rows[n];
				valor=Convert.ToDouble(dr[5]);
				valor=valor*factorCIF;
				dr[5]=valor;
				totalEmbarqueCIF+=valor;
				//Iva
				//7 IVA viene de PARANCEL
				iva=Convert.ToDouble(DBFunctions.SingleData(
					"SELECT COALESCE(PA.PIVA_PORCIVA,0) "+
					"FROM MITEMS MI LEFT JOIN PARANCEL PA ON PA.PARA_CODIGO=MI.PARA_CODIGO "+
					"WHERE MI.MITE_CODIGO='"+dr[1].ToString()+"';"));
				totalIVA+=(iva/100)*valor*tasaCambioI;
				//iva = 0 para la clase de movimiento
				dr[7]=0;
			}
			totalGastosN+=totalGravamen;//+totalIVA;
			//Factor Importacion: gastos  Nacionales / totalEmbarqueCIF
			factorImportacion=1+(totalGastosN/(totalEmbarqueCIF*tasaCambioI));
			
			for(int n=0;n<dtSource.Rows.Count;n++)
			{
				dr=dtSource.Rows[n];
				valor=Convert.ToDouble(dr[5]);
				//Valor US
				//dr[12]=valor*tasaCambio*factorImportacion;
				valor=valor*factorImportacion*tasaCambioI;
				//9 Total
				dr[9]=valor;
				//Valor unitario
				dr[5]=valor/Convert.ToDouble(dr[3]);

			}*/
			#endregion

			//Response.Write("<script language='javascript'>alert('TOTAL VALORES CIF: "+totalValoresCIF+"');</script>");
			//Response.Write("<script language='javascript'>alert('TOTAL GRAVAMEN: "+totalGravamen+"');</script>");
			//Response.Write("<script language='javascript'>alert('TOTAL IVA: "+totalIVA+"');</script>");
			//Response.Write("<script language='javascript'>alert('TOTAL EMBARQUE: "+totalEmbarque+"');</script>");
			//Response.Write("<script language='javascript'>alert('GASTOS MONEDA EX: "+totalGastosE+"');</script>");
			//Response.Write("<script language='javascript'>alert('GASTOS MONEDA NAL: "+totalGastosN+"');</script>");
			//Response.Write("<script language='javascript'>alert('FACTOR CIF: "+factorCIF+"');</script>");
			//Response.Write("<script language='javascript'>alert('FACTOR Importacion: "+factorImportacion+"');</script>");
			//Response.Write("<script language='javascript'>alert('TOTAL Costo Importacion: "+totalCostoImportacion+"');</script>");

			sqlLiquidacion=
					"INSERT INTO DLIQUIDACIONITEMS VALUES("+
					ddlEmbarque.SelectedValue+","+
					totalEmbarque+","+totalEmbarqueCIF+","+totalGravamen+","+
					totalIVA+","+tasaCambioN+","+tasaCambioI+","+totalGastosE+","+
					totalGastosN+","+factorCIF+","+factorImportacion+","+totalCostoImportacion+","+
					"'"+HttpContext.Current.User.Identity.Name+"',"+
					"'"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"');";

			if(Math.Abs(valorEmbarqueM-totalEmbarque)>(valorEmbarqueM/100))
			{
				btnAjus.Enabled=false;
                Utils.MostrarAlerta(Response, "La sumatoria de los detalles del embarque no coincide con el valor total del embarque!");
			}

			ViewState["DLIQUIDACION"]=sqlLiquidacion;
			BindDatas(false);
		}
		//CAMBIAR FECHA-------------------------------------------	
		protected void ChangeDate(Object Sender, EventArgs E)
		{
			if(dtSource.Rows.Count==0)
				tbDate.Text = calDate.SelectedDate.GetDateTimeFormats()[6];
        }

        protected void ddlAlmacen_SelectedIndexChanged(object sender, EventArgs e)
        {
            bind.PutDatasIntoDropDownList(ddlVendedor, string.Format(Almacen.VENDEDORESPORALMACEN, ddlAlmacen.SelectedValue));
          //  bind.PutDatasIntoDropDownList(ddlPrefE, string.Format(Documento.DOCUMENTOSTIPOHECHO, "FP", "IP", ddlAlmacen.SelectedValue));

            CambiaProceso(null,null);

            if (ddlPrefE.Items.Count > 1)
            {
                ddlPrefE.Items.Insert(0, "Seleccione..");
                txtNumFacE.Text = "";
            }
            else if (ddlPrefE.Items.Count > 0)
                    txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO, ddlPrefE.SelectedValue));
                else txtNumFacE.Text = "0";
         
         }

		protected void ddlPrefE_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			txtNumFacE.Text = DBFunctions.SingleData(string.Format(Documento.PROXIMODOCUMENTO,ddlPrefE.SelectedValue));
            
		}

		//REALIZAR PROCESO-------------------------------------	
		protected void NewAjust(Object Sender, EventArgs E)
		{
            //System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //customCulture.NumberFormat.NumberDecimalSeparator = ".";
            //System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            //VALIDACIONES
            uint ndref=0 , ndrefE=0;//Numero Factura - Numero Factura Entrada
            btnAjus.Enabled = false;
			int diasP = 0; //Dias plazo
			double vFlet;//Fletes
			double sto = 0,st = 0;//subtotal obtenido, subtotal guardado
			int n;

			if(ddlTProc.SelectedValue != "0")
			{	//no es prerecepcion
				try{ndref=Convert.ToUInt32(txtNumFac.Text);ndrefE=Convert.ToUInt32(txtNumFacE.Text);}
                catch
                {
                    Utils.MostrarAlerta(Response, "Uno de los números de factura no es valido!");BindDatas(false);return;
                }
				try{diasP=Convert.ToInt16(txtPlazo.Text);}
                catch
                {
                    Utils.MostrarAlerta(Response, "Los días de plazo no son validos!");BindDatas(false);return;
                }
			}

			try
			{
				vFlet=Convert.ToDouble(txtFlet.Text);
			}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor de los fletes no es valido!");
				BindDatas(false);

				return;
			}

			try
			{
				sto=Convert.ToDouble(txtSubTotal.Text.Substring(1));
			}
			catch
			{
                Utils.MostrarAlerta(Response, "El valor del subtotal no es valido!");
				BindDatas(false);
				return;
			}

			for(n=0;n<dtSource.Rows.Count;n++)
			{
				if(ddlTProc.SelectedValue=="1" || ddlTProc.SelectedValue=="2")
					st+=Convert.ToDouble(dtSource.Rows[n]["mite_cantfac"])*Convert.ToDouble(dtSource.Rows[n]["mite_precio"]);
				else if(ddlTProc.SelectedValue=="0")
					st+=Convert.ToDouble(dtSource.Rows[n]["mite_cantped"])*Convert.ToDouble(dtSource.Rows[n]["mite_precio"]);
			}
			st=Math.Round(st, numDecimales);  // se deja a 0 por que genera incongruensia con el primer calculo que es con redondeo a 0.

			if(Math.Abs(st-sto)>0.009)
			{
                Utils.MostrarAlerta(Response, "El valor del subtotal no coincide !");
				BindDatas(false);
				return;
			}

            //Ingreso manual del subtotal para validacion con el subtotal calculado.
            if (txtSubTotalManual.Text != "")
            {
                double valorManual = double.Parse(txtSubTotalManual.Text, NumberStyles.Currency);
                double valorCalculado = double.Parse(txtSubTotal.Text, NumberStyles.Currency);
                double resultado = Math.Abs(valorManual - valorCalculado);
                if (resultado > 100)
                {
                    Utils.MostrarAlerta(Response, "El valor subtotal ingresado manualmente no coincide con el valor subtotal calculado! Por favor revisar datos.");
                    BindDatas(false);
                    return;
                }
            }
            

			//Aqui Iniciamos el proceso como tal, habiendo superado con exito el proceso de validacion
			string pdref = "";  //Prefijo de documento de referencia (documento del cliente)
			uint   numref = 0;  //Numero de documento de referencia (docuemnto del cliente)
			string vend = ddlVendedor.SelectedValue;//Codigo del Vendedor
			string codigoAlmacen = ddlAlmacen.SelectedValue;
            string ccos = DBFunctions.SingleData("SELECT pcen_centinv FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + codigoAlmacen + "'");
			string carg = DBFunctions.SingleData("SELECT TVEND_CODIGO FROM PVENDEDOR WHERE PVEN_CODIGO='"+vend+"'");//Cargo???????
			string prefE = "";//Prefijo Documento Interno (PDOCUMENTO)
			UInt64 numPre = 0;//Numero de Documento Interno (PDOCUMENTO)
			string ano_cinv = ConfiguracionInventario.Ano;

            ArrayList sqlStrings = new ArrayList();
            ArrayList arrRecepciones = new ArrayList();
            FacturaProveedor facturaRepuestos = new FacturaProveedor();
            ArrayList promedios = new ArrayList ();
            

			switch(ddlTProc.SelectedValue)
			{
				case "0":  // PRE-RECEPCION
				{
					#region Proceso Prerecepcon
					//Cuando es prerecepcion se crea un registro en mprerecepcion y se modifican el dped_cantasig en la tabla dpedidoitem
					prefE  = ddlTipoPre.SelectedItem.Value;//Prefijo del Documento de prerecepcion
					numPre = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefE+"'"));//Numero de Prerecepcion
				
					while(DBFunctions.RecordExist("SELECT * FROM mprerecepcion WHERE pdoc_codigo='"+prefE+"' AND mpre_numero="+numPre))
						numPre += 1;
				
					sqlStrings.Add("INSERT INTO mprerecepcion VALUES('"+prefE+"',"+numPre+",'"+txtNIT.Text.Trim()+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','N','"+txtObs.Text+"')");
   				
					//Ahora para cada item entramos a modificar dped_cantasig si es posible, si no simplemente pasamos de largo
					for(int i=0;i<dtSource.Rows.Count;i++)
					{
						//Revisamos que ese pedido se encuentre registrado
						string[] sepPedidoItem = dtSource.Rows[i]["num_ped"].ToString().Split('-');
						pdref = sepPedidoItem[0];

						if(sepPedidoItem.Length == 2)
						{
							try
							{
								numref = Convert.ToUInt32(sepPedidoItem[1]);
							}
							catch { }

							//Comprobamos que exista un registro en dpedidoitem para este item en este pedido
							string codI = "";

							Referencias.Guardar(dtSource.Rows[i]["mite_codigo"].ToString(),ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[i]["plin_codigo"].ToString()+"'"));

                            int cantidadFacturada = Convert.ToInt32(dtSource.Rows[i]["mite_cantfac"]);

                            descargar_pedidos(ref sqlStrings, sepPedidoItem[0], Convert.ToInt32(sepPedidoItem[1].ToString()), codI, cantidadFacturada, Convert.ToInt32(ano_cinv));

                                //                     if (DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'"))
                                //{
                                //	//Debemos traer el código del almacén donde inicialmente se realizo el pedido
                                //	string codAlmacenOriginal        = DBFunctions.SingleData("SELECT palm_almacen FROM mpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi=" +sepPedidoItem[1]+ " and mped_clasregi = 'P'");
                                //	double cantidadEntregada         = Convert.ToDouble(dtSource.Rows[i]["mite_cantped"]);
                                //	double cantidadInicialPedida     = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi FROM dpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'"));
                                //	double cantidadDisponibleEntrada = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi-dped_cantasig-dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'"));
                                //	double cantidadTransito          = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_unidtrans FROM msaldoitem WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+""));
                                //	double cantidadTransitoAlmacen   = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_canttransito FROM msaldoitemalmacen WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+""));

                                //	sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig+ "+cantidadEntregada.ToString()+" WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'");

                                //	if((cantidadTransito - cantidadEntregada) > 0)
                                //		sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=msal_unidtrans-"+cantidadEntregada.ToString()+"  WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+"");
                                //	else
                                //		sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=0 WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+"");

                                //	//Si la cantidadDisponibleEntrada - cantidadEntregada es igual a cero restamos en msaldoitem almacen a la cantidad de pedidos pendientes 1
                                //	if((cantidadDisponibleEntrada - cantidadEntregada) == 0)
                                //		sqlStrings.Add("UPDATE msaldoitem SET msal_peditrans = msal_peditrans-1 WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+"");

                                //	//Ahora realizamos el manejo de los almacenes(msaldoitemalmacen);
                                //	if(codAlmacenOriginal == codigoAlmacen)
                                //	{
                                //		if((cantidadTransitoAlmacen - cantidadEntregada) > 0)
                                //			sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito-"+cantidadEntregada.ToString()+" WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");
                                //		else
                                //			sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=0 WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");
                                //	}
                                //	else
                                //	{
                                //		sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito-"+cantidadDisponibleEntrada.ToString()+" WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");

                                //		/*if(!DBFunctions.RecordExist("SELECT * FROM msaldoitemalmacen WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codigoAlmacen+"' AND PANO_ANO="+ano_cinv))
                                //			sqlStrings.Add("INSERT INTO msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_cantpendiente,msal_canttransito,msal_cantinveinic) VALUES ('"+codI+"','"+codigoAlmacen+"',"+ano_cinv+",0,0,0,0,0,0,0)");*/
                                //		SaldoItem.CrearRegistroSaldoItemAlmacen(codI,ano_cinv,codigoAlmacen);

                                //		if((cantidadInicialPedida - cantidadEntregada) > 0)
                                //			sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito+"+(cantidadInicialPedida - cantidadEntregada).ToString()+" WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");
                                //	}
                                //}
                            }
                        }

					sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numPre+" WHERE pdoc_codigo='"+prefE+"'");
					#endregion
				}
					break;
				case "1":  // RECEPCION DIRECTA
				{
					#region Proceso de Recepcion Directa
					//Guardar la factura de proveedor
					//Revisamos si ya existe una factura con este prefijo-numero
					string prefijoFacturaProveedor = ddlPrefE.SelectedValue;
					UInt64 numeroFacturaProveedor = Convert.ToUInt64(txtNumFacE.Text.Trim());
					prefE  = prefijoFacturaProveedor;
					numPre = numeroFacturaProveedor;
				
					if(DBFunctions.RecordExist("SELECT * FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFacturaProveedor+"' AND mfac_numeordepago="+numeroFacturaProveedor+""))
						numeroFacturaProveedor = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoFacturaProveedor+"'"));
				
					FacturaProveedor facturaRepuestosProv = new FacturaProveedor("FPR",prefijoFacturaProveedor,txtPref.Text.Trim(),txtNIT.Text.Trim(),ddlAlmacen.SelectedValue,"F",numeroFacturaProveedor,Convert.ToUInt64(txtNumFac.Text.Trim()),"V",
                        Convert.ToDateTime(tbDate.Text), Convert.ToDateTime(tbDate.Text).AddDays(Convert.ToDouble(txtPlazo.Text.Trim())),Convert.ToDateTime(null), Convert.ToDateTime(tbDate.Text),
						Convert.ToDouble(txtSubTotal.Text.Substring(1)) - Convert.ToDouble(txtDesc.Text.Substring(1)),Convert.ToDouble(txtIVA.Text.Substring(1)),
						Convert.ToDouble(txtFlet.Text.Trim()),Convert.ToDouble(txtTotIF.Text.Substring(1).Trim()),0,txtObs.Text,HttpContext.Current.User.Identity.Name.ToLower());
				
					facturaRepuestosProv.GrabarFacturaProveedor(false);
					numPre = facturaRepuestosProv.NumeroFactura;
				
					for(int i=0;i<facturaRepuestosProv.SqlStrings.Count;i++)
						sqlStrings.Add(facturaRepuestosProv.SqlStrings[i].ToString());
				
					//Creamos el objeto para retenciones
					try
					{
						Retencion RetencionItems=new Retencion(facturaRepuestosProv.NitProveedor,
							facturaRepuestosProv.PrefijoFactura,
							Convert.ToInt32(facturaRepuestosProv.NumeroFactura),
                            dtSource,
							(facturaRepuestosProv.ValorFactura+facturaRepuestosProv.ValorFletes),
							(facturaRepuestosProv.ValorIva+facturaRepuestosProv.ValorIvaFletes),
							"R",false);
						RetencionItems.Guardar_Retenciones(false);

						for(int i=0;i<RetencionItems.Sqls.Count;i++)
							sqlStrings.Add(RetencionItems.Sqls[i].ToString());
					}
					catch(Exception ex)
					{
                        Utils.MostrarAlerta(Response, "Error en Retenciones. Detalles : \\n" + ex.Message + "");
						return;
					}
                        DataSet lasLineas = new DataSet();
                        DBFunctions.Request(lasLineas, IncludeSchema.NO, "SELECT plin_codigo, plin_tipo FROM plineaitem");
					for(int i=0;i<dtSource.Rows.Count;i++)
					{
						//Determinamos cual es el prefijo y el numero del pedido
						string[] sepPedidoItem = dtSource.Rows[i]["num_ped"].ToString().Split('-');
                        String numPedido = "0";
                        if (sepPedidoItem.Length == 2)
                            numPedido = sepPedidoItem[1].ToString();

                        //Revisamos si existe el pedido con anterioridad
                        string codI = "";

                        Referencias.Guardar(dtSource.Rows[i]["mite_codigo"].ToString(), ref codI, (dtSource.Rows[i]["plin_codigo"].ToString()==""?"":lasLineas.Tables[0].Select("plin_codigo='" + dtSource.Rows[i]["plin_codigo"].ToString() + "'")[0].ItemArray[1].ToString()));//DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtSource.Rows[i]["plin_codigo"].ToString() + "'"));//lasLineas.Tables[0].Select("plin_codigo='" + dtSource.Rows[i]["plin_codigo"].ToString() + "'")[0].ItemArray[1].ToString());
					
		                //				if(sepPedidoItem.Length == 2)
		                //			{
                        //     descargar_pedidos(ref ArrayList sqlStrings, string sepPedidoItem[0], int Convert.ToInt16(sepPedidoItem[1].ToString()), string codI, int Convert.ToInt16(dtSource.Rows[i]["mite_cantfac"].ToString()), int Convert.ToInt16(ano_cinv.ToString());                         );
                        //     descargar_pedidos(ref ArrayList sqlStrings, string pedido, int numepedi, string item, int cantidad_facturada, int ano_cinv)
                        int cantidadFacturada         = Convert.ToInt32(dtSource.Rows[i]["mite_cantfac"]);

                        descargar_pedidos(ref sqlStrings, sepPedidoItem[0], Convert.ToInt32(numPedido), codI, cantidadFacturada, Convert.ToInt32(ano_cinv));
                            

                        //                         if (DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'"))
                        //{
                        //	//Debemos traer el codigo del almacen donde inicialmente se realizo el pedido
                        //	string codAlmacenOriginal        = DBFunctions.SingleData("SELECT palm_almacen FROM mpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+ " and mped_clasregi = 'P'");
                        //	double cantidadFacturada         = Convert.ToDouble(dtSource.Rows[i]["mite_cantfac"]);
                        //	double cantidadInicialPedida     = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi FROM dpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'"));
                        //	double cantidadDisponibleEntrada = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi-dped_cantasig-dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'"));
                        //                         double cantidadTransito = 0;

                        //                         try { cantidadTransito = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(msal_unidtrans,0) FROM msaldoitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + ano_cinv + ""));}
                        //                         catch (Exception er) { cantidadTransito = 0;  }

                        //                         double cantidadTransitoAlmacen = 0;
                        //                         try
                        //                         {
                        //                             Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(msal_canttransito,0) FROM msaldoitemalmacen WHERE mite_codigo='" + codI + "' AND palm_almacen='" + codAlmacenOriginal + "' AND pano_ano=" + ano_cinv + ""));
                        //                         }
                        //                         catch
                        //                         {
                        //                             cantidadTransitoAlmacen = 0;
                        //                         }
                        //                         sqlStrings.Add("UPDATE dpedidoitem SET dped_cantfact=dped_cantfact+ "+cantidadFacturada.ToString()+" WHERE pped_codigo='"+sepPedidoItem[0]+"' AND mped_numepedi="+sepPedidoItem[1]+" AND mite_codigo='"+codI+ "' and mped_clasregi = 'P'");

                        //	if((cantidadTransito - cantidadFacturada) >= 0)
                        //		sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=msal_unidtrans-"+cantidadFacturada.ToString()+"  WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+"");
                        //	else
                        //		sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans=0  WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+"");

                        //	//Si la cantidadDisponibleEntrada - cantidadEntregada es igual a cero restamos 1 en msaldoitem almacen a la cantidad de pedidos pendientes
                        //	if((cantidadDisponibleEntrada - cantidadFacturada)== 0)
                        //		sqlStrings.Add("UPDATE msaldoitem SET msal_peditrans = msal_peditrans-1 WHERE mite_codigo='"+codI+"' AND pano_ano="+ano_cinv+"");

                        //	//Ahora realizamos el manejo de los almacenes(msaldoitemalmacen);							
                        //	if(codAlmacenOriginal == codigoAlmacen)
                        //	{
                        //		if((cantidadTransitoAlmacen - cantidadFacturada) >= 0)
                        //			sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito-"+cantidadFacturada.ToString()+" WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");
                        //		else
                        //			sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=0 WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");
                        //	}
                        //	else
                        //	{
                        //		sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito-"+cantidadFacturada.ToString()+" WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");

                        //		SaldoItem.CrearRegistroSaldoItemAlmacen(codI,ano_cinv,codigoAlmacen);

                        //		if((cantidadInicialPedida - cantidadFacturada) >= 0)
                        //			sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito+"+(cantidadInicialPedida-cantidadFacturada).ToString()+" WHERE mite_codigo='"+codI+"' AND palm_almacen='"+codAlmacenOriginal+"' AND pano_ano="+ano_cinv+"");
                        //	}
                        //}
    //                  }
                    }
					#endregion
				}
					break;
				case "2":  // LEGALIZACION DE PRE-RECEPCION
				{
					#region Proceso de Legalizacion de Prerecepciones
					//Guardar la factura de proveedor
					//Revisamos si ya existe una factura con este prefijo-numero
					string prefijoFacturaProveedor = ddlPrefE.SelectedValue;
					UInt64 numeroFacturaProveedor = Convert.ToUInt64(txtNumFacE.Text.Trim());
					prefE = prefijoFacturaProveedor;
					numPre = numeroFacturaProveedor;
				
					//Creamos el objeto de factura de proveedor 
					FacturaProveedor facturaRepuestosProv = new FacturaProveedor("FPR",prefijoFacturaProveedor,txtPref.Text.Trim(),txtNIT.Text.Trim(),ddlAlmacen.SelectedValue,"F",numeroFacturaProveedor,Convert.ToUInt64(txtNumFac.Text.Trim()),"V",
						calDate.SelectedDate,calDate.SelectedDate.AddDays(Convert.ToDouble(txtPlazo.Text.Trim())),Convert.ToDateTime(null),calDate.SelectedDate,
						Convert.ToDouble(txtSubTotal.Text.Substring(1)) - Convert.ToDouble(txtDesc.Text.Substring(1)),Convert.ToDouble(txtIVA.Text.Substring(1)),
						Convert.ToDouble(txtFlet.Text.Trim()),Convert.ToDouble(txtTotIF.Text.Substring(1).Trim()),0,txtObs.Text,HttpContext.Current.User.Identity.Name.ToLower());
					facturaRepuestosProv.GrabarFacturaProveedor(false);
					numPre = facturaRepuestosProv.NumeroFactura;
				
					for(int i=0;i<facturaRepuestosProv.SqlStrings.Count;i++)
						sqlStrings.Add(facturaRepuestosProv.SqlStrings[i].ToString());
				
					//Creamos el objeto para retenciones
					try
					{
						Retencion RetencionItems=new Retencion(facturaRepuestosProv.NitProveedor,
							facturaRepuestosProv.PrefijoFactura,
							Convert.ToInt32(facturaRepuestosProv.NumeroFactura),
							(facturaRepuestosProv.ValorFactura+facturaRepuestosProv.ValorFletes),
							(facturaRepuestosProv.ValorIva+facturaRepuestosProv.ValorIvaFletes),
							"R",false);

						RetencionItems.Guardar_Retenciones(false);

                            for (int i = 0; i < RetencionItems.Sqls.Count; i++)
                                sqlStrings.Add(RetencionItems.Sqls[i].ToString());
                        }
                        catch (Exception ex)
                        {
                            Utils.MostrarAlerta(Response, "Error en Retenciones. Detalles : \\n " + ex.Message + "");
                            return;
                        }
                        promedios.Clear();
                        for (int i = 0; i < dtSource.Rows.Count; i++)
                        {
                            //Agregamos esta prerecepcion a un arreglo donde estan las prerecepciones
                            if (arrRecepciones.BinarySearch(dtSource.Rows[i]["num_ped"].ToString()) < 0)
                                arrRecepciones.Add(dtSource.Rows[i]["num_ped"].ToString());

						//En el registro de ditems en el campo de cantidaddevuelta aumentarle la cantidad facturada
						//Debemos por cada item actualizar en dpedidoitem : restar a la cantidad asignada y aumentar a la cantidad facturada (si es posible)
						string[] sepPreRecep = dtSource.Rows[i]["num_ped"].ToString().Split('-');
						string codI = "";
                        string prefijoPedido = "";
                        string numeroPedido = "";

                            Referencias.Guardar(dtSource.Rows[i]["mite_codigo"].ToString(), ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtSource.Rows[i]["plin_codigo"].ToString() + "'"));
                            if (sepPreRecep.Length == 3) {
                                                            
                                promedios.Add("update ditems set (dite_costprom, dite_costpromalma, dite_costpromhis, dite_costpromhisalma) " +
                                               " = (select dite_costprom, dite_costpromalma, dite_costpromhis, dite_costpromhisalma "+
                                               " from ditems WHERE pdoc_codigo='" + sepPreRecep[0]  +"-" + sepPreRecep[1] + "' AND dite_numedocu=" + sepPreRecep[2] + " AND mite_codigo='" + codI + "') " +
                                               " WHERE dite_prefdocurefe = '" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND dite_numedocurefe= '" + sepPreRecep[2] + "' AND mite_codigo='" + codI + "';");

                                sqlStrings.Add("UPDATE ditems SET dite_cantdevo=dite_cantdevo+" + Convert.ToDouble(dtSource.Rows[i]["mite_cantfac"]) + " WHERE pdoc_codigo='" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND dite_numedocu=" + sepPreRecep[2] + " AND mite_codigo='" + codI + "'");


                                //prefijoPedido = sepPreRecep[0] + "-" + sepPreRecep[1];
                                numeroPedido=DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND dite_numedocu=" + sepPreRecep[2] + " AND mite_codigo='" + codI + "'");
                                
                                prefijoPedido = DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND dite_numedocu=" + sepPreRecep[2] + " AND mite_codigo='" + codI + "'");
                               //  = DBFunctions.SingleData("SELECT dite_numedocurefe FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND dite_numedocu=" + sepPreRecep[2] + " AND mite_codigo='" + codI + "'");
                            }

                            else if (sepPreRecep.Length == 2)
                            {
                                promedios.Add("update ditems set (dite_costprom, dite_costpromalma, dite_costpromhis, dite_costpromhisalma) " +
                                               " = (select dite_costprom, dite_costpromalma, dite_costpromhis, dite_costpromhisalma " +
                                               " from ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "' AND dite_numedocu=" + sepPreRecep[1] + " AND mite_codigo='" + codI + "') " +
                                               " WHERE dite_prefdocurefe = '" + sepPreRecep[0] + "' AND dite_numedocurefe=" + sepPreRecep[1] + " AND mite_codigo='" + codI + "';");

                                sqlStrings.Add("UPDATE ditems SET dite_cantdevo=dite_cantdevo+" + Convert.ToDouble(dtSource.Rows[i]["mite_cantfac"]) + " WHERE pdoc_codigo='" + sepPreRecep[0] + "' AND dite_numedocu=" + sepPreRecep[1] + " AND mite_codigo='" + codI + "'");
                                 prefijoPedido = DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "' AND dite_numedocu=" + sepPreRecep[1] + " AND mite_codigo='" + codI + "'");
                                numeroPedido = DBFunctions.SingleData("SELECT dite_numedocurefe FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "' AND dite_numedocu=" + sepPreRecep[1] + " AND mite_codigo='" + codI + "'");
                            }                             

                            //Revisamos si existe el registro de dpedidoitems y lo actualizamos
                        //    if (DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND mite_codigo='" + codI + "'"))
                        //        sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig-" + Convert.ToDouble(dtSource.Rows[i]["mite_cantped"]) + "  , dped_cantfact=dped_cantfact+" + Convert.ToDouble(dtSource.Rows[i]["mite_cantfac"]) + "  WHERE pped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND mite_codigo='" + codI + "'");

                            //Si la cantidad facturada es mayor que la recibida es porque existe un faltante mfaltanteentradaitem
                            //Revisamos si es necesario grabar el registro de mfaltanteentradaitem
                            if (Convert.ToDouble(dtSource.Rows[i]["mite_cantfac"]) > Convert.ToDouble(dtSource.Rows[i]["mite_cantped"]))
                                sqlStrings.Add("INSERT INTO mfaltanteentradaitem VALUES('" + prefijoFacturaProveedor + "'," + numeroFacturaProveedor + ",'" + codI + "','" + prefijoPedido + "'," + numeroPedido + "," + (Convert.ToDouble(dtSource.Rows[i]["mite_cantfac"]) - Convert.ToDouble(dtSource.Rows[i]["mite_cantped"])) + ")");
                        }
                        #endregion
                }
                    break;
            }

			#region Movimiento de Kardex
			string ano_cont = ConfiguracionInventario.Ano;

			int tm = 0;

			switch(ddlTProc.SelectedValue)
			{
				case "0":
					tm = 10;//Prerecep.
					break;
				case "1":
					tm = 30;//Entradas de proveedor
					break;
				case "2":
					tm = 11;//Legalizacion Prerecepcion
					break;
			}

			//Creamos el Objeto manejador de movimientos de kardex utilizando el constructor #2
			Movimiento Mov = new Movimiento(prefE,numPre,tm,calDate.SelectedDate,txtNIT.Text,codigoAlmacen,vend,carg,ccos,"N");
			
			DataSet da = new DataSet();
			
			double cant,valU,costP,costPH,costPA,costPHA,invI,invIA,pIVA,pDesc,cantDev,valP;
			
			ArrayList Prrs = new ArrayList();//Prerecepciones de la lista
			
			//DITEMS
			
            for(n=0;n<dtSource.Rows.Count;n++)
			{
				string codI = "";
				string prefijoDocumentoReferencia = "";
				uint numeroDocumentoReferencia = 0;
				string[] sepDocRef = dtSource.Rows[n]["num_ped"].ToString().Split('-');

                if (sepDocRef.Length == 3)
                {
                    prefijoDocumentoReferencia = sepDocRef[0] +"-"+ sepDocRef[1];
                    numeroDocumentoReferencia = Convert.ToUInt32(sepDocRef[2]);
                }


				else if(sepDocRef.Length == 2)
				{
					prefijoDocumentoReferencia = sepDocRef[0];
					numeroDocumentoReferencia = Convert.ToUInt32(sepDocRef[1]);
				}
				else
					prefijoDocumentoReferencia = sepDocRef[0];

				Referencias.Guardar(dtSource.Rows[n]["mite_codigo"].ToString(),ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[n]["plin_codigo"].ToString()+"'"));//Codigo del Item
				
				if(ddlTProc.SelectedValue == "0")
					cant = Convert.ToDouble(dtSource.Rows[n]["mite_cantped"]);//cantidad ingresada
				else
					cant = Convert.ToDouble(dtSource.Rows[n]["mite_cantfac"]);//cantidfad facturada

                valU = Convert.ToDouble(dtSource.Rows[n]["mite_precio"]);//valor unidad
                pIVA = Convert.ToDouble(dtSource.Rows[n]["mite_iva"]); //iva
                pDesc = Convert.ToDouble(dtSource.Rows[n]["mite_desc"]); //descuento
                da.Clear();

            //  cuando es legalizacion de prerecepcion, debe tomar los costos de la prerecepcion
                costP = SaldoItem.ObtenerCostoPromedio(codI, ano_cont);
                costPH = SaldoItem.ObtenerCostoPromedioHistorico(codI, ano_cont);
                costPA = SaldoItem.ObtenerCostoPromedioAlmacen(codI, ano_cont, codigoAlmacen);
                costPHA = SaldoItem.ObtenerCostoPromedioHistoricoAlmacen(codI, ano_cont, codigoAlmacen);
                invI = SaldoItem.ObtenerCantidadActual(codI, ano_cont);
                invIA = SaldoItem.ObtenerCantidadActualAlmacen(codI, ano_cont, codigoAlmacen);

				cantDev = 0;//devolucion
				
				//Valor Publico, Por el momento se va a tomar el costopromedio
				valP = costP;

				Mov.InsertaFila(codI,cant,valU,costP,costPA,pIVA,pDesc,cantDev,costPH,costPHA,valP,invI,invIA,prefijoDocumentoReferencia,numeroDocumentoReferencia);
				//				 0    1		2	3		4	 5		6		7	  8		9	   10	11	  12			13							14
			}
			#endregion

			Mov.RealizarMov(false);

			for(int i=0;i<Mov.SqlStrings.Count;i++)
				sqlStrings.Add(Mov.SqlStrings[i].ToString());
            //string losdatos = "";
            //for (int i = 0; i < sqlStrings.Count; i++)
            //    losdatos += sqlStrings[i];

            if (ddlTProc.SelectedValue == "2")
            {
                string salida = "";
                //Revisamos en caso que sea una legalizacion si las prerecepciones incluidas ya estan legalizadas
                RevisarPreRecepciones(arrRecepciones, ref salida, ref sqlStrings);
                //se actauliza el costo promedio de la legalizacion con el costo de la prerecepcion 
                for (int i = 0; i < promedios.Count; i++)
                    sqlStrings.Add(promedios [i]);
            }

            //Embarques-nacionalizacion
            if (ddlTProc.SelectedValue == "1" && ViewState["NACIONALIDAD"].ToString() == "E" && chkImportacion.Checked)
            {
                sqlStrings.Add("UPDATE MEMBARQUE SET PEST_ESTADO='N' WHERE MEMB_SECUENCIA=" + ddlEmbarque.SelectedValue + ";");
                sqlStrings.Add("UPDATE DEMBARQUE SET DITE_CANTNACI=DITE_CANTEMBA WHERE MEMB_SECUENCIA=" + ddlEmbarque.SelectedValue + ";");
                //Detalles Liquidacion
                try { sqlStrings.Add(ViewState["DLIQUIDACION"].ToString()); }
                catch (Exception err) { }
            }

      
           if (DBFunctions.Transaction(sqlStrings))
           {
               //   lbInfo.Text += "<br>Bien En Parte 1 " + DBFunctions.exceptions;
               dtSource.Clear();
          
               if (ddlTProc.SelectedValue == "0")
                    Response.Redirect("" + indexPage + "?process=Inventarios.RecepcionItems&path=Recepcion de Items&prR=" + ddlTProc.SelectedValue + "&pref=" + prefE + "&num=" + numPre);
               else
               {
                    //lbInfo.Text += "<br>Bien En Parte 1 " + DBFunctions.exceptions;
              
                   Response.Redirect(""+indexPage+"?process=Inventarios.RecepcionItems&path=Recepcin de Items&prR="+ddlTProc.SelectedValue+"&pref="+prefE+"&num="+numPre);
                   // Se elimina la facilidad de modificar las retenciones...
                   // Response.Redirect("" + indexPage + "?process=Cartera.ModificacionRetenciones&path=Recepcion de Items&prR=" + ddlTProc.SelectedValue + "&pref=" + prefE + "&num=" + numPre);
                            
               }
           }
           else

                lbInfo.Text += "<br>Error : Detalles <br>" + DBFunctions.exceptions;
            
        }

        protected void ddlLinea_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ViewState["linea"] = ((DropDownList)sender).SelectedIndex;
        }

        protected void descargar_pedidos(ref ArrayList sqlStrings, string pedido, int numepedi, string item, int cantidad_facturada, int ano_cinv)
        {
            DataSet dsPedidos = new DataSet();
            DBFunctions.Request(dsPedidos, IncludeSchema.NO, @"select di.*, mP.palm_almacen from dpedidoitem dp, Mpedidoitem Mp
                                                                    where dp.mped_clasregi = 'P' and dped_cantpedi - dped_cantasig - dped_cantfact > 0
                                                                      AND DP.MPED_CLASREGI = MP.MPED_CLASEREGI AND DP.MNIT_NIT = MP.MNIT_NIT AND dp.pped_codigo = Mp.pped_codigo AND dp.mped_numepedi = Mp.mped_numepedi
                                                                      and dP.MITE_CODIGO = '" + item + @"' and dp.pped_codigo='" + pedido + @"' AND dp.mped_numepedi=" + numepedi + @" and mped_clasregi = 'P'");
            if (dsPedidos.Tables.Count == 0)
            {
                DBFunctions.Request(dsPedidos, IncludeSchema.YES, @"select dP.*, mP.palm_almacen from dpedidoitem dp, Mpedidoitem Mp
                                                                    where dp.mped_clasregi = 'P' and dped_cantpedi - dped_cantasig - dped_cantfact > 0
                                                                      AND DP.MPED_CLASREGI = MP.MPED_CLASEREGI AND DP.MNIT_NIT = MP.MNIT_NIT AND dp.pped_codigo = Mp.pped_codigo AND dp.mped_numepedi = Mp.mped_numepedi
				                                                      and dP.MITE_CODIGO = '" + item + @"' AND MP.MNIT_NIT = '" + txtNIT.Text + "' order by MPED_PEDIDO, 1 ; ");
            }
            if (dsPedidos.Tables.Count > 0)
            {
                //Debemos traer el codigo del almacen donde inicialmente se realizo el pedido
                double cantidadFacturada  = Convert.ToDouble(cantidad_facturada);
                double cantidadTransito = 0;

                try { cantidadTransito = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(msal_unidtrans,0) FROM msaldoitem WHERE mite_codigo='" + item + "' AND pano_ano=" + ano_cinv + "")); }
                catch (Exception er) { cantidadTransito = 0; }

                double cantidadTransitoAlmacen = 0;

                if ((cantidadTransito - cantidadFacturada) > 0)
                    sqlStrings.Add("UPDATE msaldoitem SET msal_unidtrans = msal_unidtrans-" + cantidadFacturada.ToString() + "  WHERE mite_codigo='" + item + "' AND pano_ano=" + ano_cinv + "");
                else
                    sqlStrings.Add("UPDATE msaldoitem SET msal_UNIDtrans=0, msal_PEDItrans=0  WHERE mite_codigo='" + item + "' AND pano_ano=" + ano_cinv + "");

                for (int i = 0; i < dsPedidos.Tables[0].Rows.Count; i++)
                {
                    string codAlmacenOriginal = dsPedidos.Tables[0].Rows[i][13].ToString();
                    double cantidadPendiente = Convert.ToDouble(dsPedidos.Tables[0].Rows[i][5].ToString()) - Convert.ToDouble(dsPedidos.Tables[0].Rows[i][7].ToString());
                    try
                    {
                        cantidadTransitoAlmacen = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(msal_canttransito,0) FROM msaldoitemalmacen WHERE mite_codigo='" + item + "' AND palm_almacen='" + codAlmacenOriginal + "' AND pano_ano=" + ano_cinv + ""));
                    }
                    catch
                    {
                        cantidadTransitoAlmacen = 0;
                    }
                    if (cantidadPendiente >= cantidadFacturada)
                    {
                        sqlStrings.Add("UPDATE dpedidoitem SET dped_cantfact = dped_cantfact+ " + cantidadFacturada.ToString() + " WHERE pped_codigo='" + dsPedidos.Tables[0].Rows[i][2].ToString() + "' AND mped_numepedi=" + dsPedidos.Tables[0].Rows[i][3].ToString() + " AND mite_codigo='" + item + "' and mped_clasregi = 'P'");
                        sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito=msal_canttransito-" + cantidadFacturada.ToString() + " WHERE mite_codigo='" + item + "' AND palm_almacen='" + codAlmacenOriginal + "' AND pano_ano=" + ano_cinv + " " );
                        cantidadFacturada = 0;
                    }
                    else
                    {
                        sqlStrings.Add("UPDATE dpedidoitem SET dped_cantfact = dped_cantfact+ " + cantidadPendiente.ToString() + " WHERE pped_codigo='" + dsPedidos.Tables[0].Rows[i][2].ToString() + "' AND mped_numepedi=" + dsPedidos.Tables[0].Rows[i][3].ToString() + " AND mite_codigo='" + item + "' and mped_clasregi = 'P'");
                        sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito = msal_canttransito-" + cantidadPendiente.ToString() + " WHERE mite_codigo='" + item + "' AND palm_almacen='" + codAlmacenOriginal + "' AND pano_ano=" + ano_cinv + " ");
                        cantidadFacturada -= cantidadPendiente;
                    }
                    sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_canttransito = 0  WHERE mite_codigo='" + item + "' AND palm_almacen='" + codAlmacenOriginal + "' AND pano_ano=" + ano_cinv + " AND msal_canttransito < 0");

                    if (cantidadFacturada <= 0)
                        return;
                }
            }
        }

        #endregion

        #region Bound Grillas
        //GRILLAS DATABOUND -----------------------------------------------------------
        protected void RLeg_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]),"SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mpre_numero as character(10)) FROM mprerecepcion WHERE mpre_nitprov='"+txtNIT.Text.Trim()+"' AND mpre_legal='N'");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]),"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onclick","CargaNITLEG("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
				((DropDownList)e.Item.Cells[3].Controls[1]).Attributes.Add("onchange","ChangeLine("+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+");");
			}
		}
		
		protected void RPRec_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
				string pedidosObligatorios = DBFunctions.SingleData("SELECT mpro_pedioblig FROM mproveedor WHERE mnit_nit='"+txtNIT.Text+"'");
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownListj(((DropDownList)e.Item.Cells[0].Controls[1]),"SELECT MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi as character(10)) FROM mpedidoitem MPI, dpedidoitem DPI WHERE MPI.mnit_nit='"+txtNIT.Text+"' AND MPI.mped_claseregi='P' AND MPI.pped_codigo=DPI.pped_codigo AND MPI.mped_numepedi=DPI.mped_numepedi AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 ORDER BY MPI.pped_codigo,MPI.mped_numepedi ASC");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]),
					"select codigo from (SELECT MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi as character(10)) as codigo "+
					"FROM DBXSCHEMA.mpedidoitem MPI, DBXSCHEMA.dpedidoitem DPI "+
					"WHERE MPI.mped_claseregi='P' "+
					" AND MPI.pped_codigo=DPI.pped_codigo "+
					" AND MPI.mped_numepedi=DPI.mped_numepedi "+
					" AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 "+
					" AND MPI.mnit_nit='"+txtNIT.Text+"') pedido group by codigo ;"
				);
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]),"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onkeyup","ItemMask("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
				((DropDownList)e.Item.Cells[3].Controls[1]).Attributes.Add("onchange","ChangeLine("+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+");");
				//Debemos determinar si para este proveedor es obligatorio el realizar pedido, si es obligatorio ocultamos el textbox que nos permite ingresar un numero de pedido
				if(pedidosObligatorios == "S")
				{
					((TextBox)e.Item.Cells[0].Controls[3]).Visible = false;
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("ondblclick","CargaNITPRE("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+",null,"+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
				}
				else
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("ondblclick","CargaNITPRE("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[0].Controls[3]).ClientID+","+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
			}
		}
		
		protected void RRDir_ItemDataBound(object sender, DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Footer)
			{
               ((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Remove("OnBlur");
               ((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("OnBlur", "ConsultarInfoReferencia(this,'" + ((DropDownList)e.Item.Cells[3].Controls[1]).ClientID + "','" + ((TextBox)e.Item.Cells[2].Controls[1]).ClientID + "','" + ddlAlmacen.ClientID + "','" + ((TextBox)e.Item.Cells[7].Controls[1]).ClientID + "','" + ((TextBox)e.Item.Cells[8].Controls[1]).ClientID + "','" + ((TextBox)e.Item.Cells[9].Controls[1]).ClientID + "','" + txtNIT.Text + "');");
               //                                                                                 (objSender,                  idDDLLinea,                                                  idObjNombre,                                     idDDLalmacen,                        idObjValor,                                                           idObjDesc,                                                      idObjIva)

                if (ViewState["linea"] != null)
                    ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedIndex = (int)ViewState["linea"];

                if(ViewState["nomPedido"] != null)
                    ((TextBox)e.Item.Cells[0].Controls[3]).Text = (string)ViewState["nomPedido"];

				string pedidosObligatorios = DBFunctions.SingleData("SELECT mpro_pedioblig FROM mproveedor WHERE mnit_nit='"+txtNIT.Text+"'");
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]),"SELECT MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi as character(10)) FROM mpedidoitem MPI, dpedidoitem DPI WHERE MPI.mnit_nit='"+txtNIT.Text+"' AND MPI.mped_claseregi='P' AND MPI.pped_codigo=DPI.pped_codigo AND MPI.mped_numepedi=DPI.mped_numepedi AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 ORDER BY MPI.pped_codigo,MPI.mped_numepedi ASC");
				//bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]),"SELECT distinct Codigo FROM VINVENTARIOS_PEDIDOSPORNIT WHERE mnit_nit='"+txtNIT.Text+"' ORDER BY Codigo ASC");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]),
					"select codigo from (SELECT MPI.pped_codigo CONCAT '-' CONCAT CAST(MPI.mped_numepedi as character(10)) as codigo "+
					"FROM DBXSCHEMA.mpedidoitem MPI, DBXSCHEMA.dpedidoitem DPI "+
					"WHERE MPI.mped_claseregi='P' "+
					" AND MPI.pped_codigo=DPI.pped_codigo "+
					" AND MPI.mped_numepedi=DPI.mped_numepedi "+
					" AND (DPI.dped_cantpedi-DPI.dped_cantasig-DPI.dped_cantfact)>0 "+
					" AND MPI.mnit_nit='"+txtNIT.Text+"') pedido group by codigo;"
					);

				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]),"SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("onkeyup","ItemMask("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
				((DropDownList)e.Item.Cells[3].Controls[1]).Attributes.Add("onchange","ChangeLine("+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+");");
				if(pedidosObligatorios == "S")
				{
					((TextBox)e.Item.Cells[0].Controls[3]).Visible = false;
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("ondblclick","CargaNITREPDIR("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+",null,"+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
				}
				else
					((TextBox)e.Item.Cells[1].Controls[1]).Attributes.Add("ondblclick","CargaNITREPDIR("+((TextBox)e.Item.Cells[1].Controls[1]).ClientID+","+((DropDownList)e.Item.Cells[0].Controls[1]).ClientID+","+((TextBox)e.Item.Cells[0].Controls[3]).ClientID+","+((DropDownList)e.Item.Cells[3].Controls[1]).ClientID+");");
			}
		}
		#endregion
		
		#region Cancelar Grillas
		//GRILLAS CANCELAR EDICION--------------------------------------------- 	
		public void RLeg_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgItemsLeg.EditItemIndex=-1;
			BindDatas(false);
		}
	 	
		public void RPRec_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgItemsPRec.EditItemIndex=-1;
			BindDatas(false);
		}
	 	
		public void RRDir_Cancel(Object sender, DataGridCommandEventArgs e)
		{
			dgItemsRDir.EditItemIndex=-1;
			BindDatas(false);
		}
		#endregion
		
		#region Actualización Grillas
		//ACTUALIZAR GRILLAS---------------------------------------------------
		public void RLeg_Update(Object sender, DataGridCommandEventArgs e)
		{
			if(dtSource.Rows.Count == 0)
				return;
			double cantidadFacturada = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
			if(cantidadFacturada < 0)
				cantidadFacturada = 0;
			string codI = "";
			Referencias.Guardar(dtSource.Rows[dgItemsLeg.EditItemIndex]["mite_codigo"].ToString(),ref codI,DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[dgItemsLeg.EditItemIndex]["plin_codigo"].ToString()+"'"));
			//Revisamos si la cantidad ingresada es valida para la facturacion
			string preRecepNum = dtSource.Rows[dgItemsLeg.EditItemIndex]["num_ped"].ToString();
			string prefijoPedido = DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='"+(preRecepNum.Split('-'))[0]+"' AND dite_numedocu="+(preRecepNum.Split('-'))[1]+" AND mite_codigo='"+codI+"'");
			string numeroPedido = DBFunctions.SingleData("SELECT dite_numedocurefe FROM ditems WHERE pdoc_codigo='"+(preRecepNum.Split('-'))[0]+"' AND dite_numedocu="+(preRecepNum.Split('-'))[1]+" AND mite_codigo='"+codI+"'");
			if(DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+" AND mite_codigo='"+numeroPedido+"'"))
			{
				double cantidadComparacion = 0;
				try{cantidadComparacion = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi - dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+" AND mite_codigo='"+codI+"'"));}
				catch{}
				if(cantidadFacturada > cantidadComparacion)
				{
                    Utils.MostrarAlerta(Response, "La cantidad facturada superada la disponible para facturar.\\nCantidad disponible a facturar " + cantidadComparacion.ToString("N") + "!");
					return;
				}
			}
			double precio    = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
			double iva       = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);
			double descuento = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
			if(descuento < 0) descuento = 0;
			if(descuento > 100) descuento = 100;
			if(iva < 0)iva   = 0;
			if(iva > 100)iva = 100;
			double total = Total(precio,cantidadFacturada,descuento,iva);
			dtSource.Rows[dgItemsLeg.EditItemIndex]["mite_cantfac"] = cantidadFacturada;
			dtSource.Rows[dgItemsLeg.EditItemIndex]["mite_precio"] = precio;
			dtSource.Rows[dgItemsLeg.EditItemIndex]["mite_desc"] = descuento;
            dtSource.Rows[dgItemsLeg.EditItemIndex]["mite_iva"] = iva;
			dtSource.Rows[dgItemsLeg.EditItemIndex]["mite_tot"] = total;
			dgItemsLeg.EditItemIndex=-1;
			BindDatas(true);    	
		}
    	
		public void RPRec_Update(Object sender, DataGridCommandEventArgs e)
		{
			if(dtSource.Rows.Count==0)
				return;
			double cantidad = Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
            if(cantidad<0)
				cantidad=0;
			//Debemos revisar si la cantidad es valida o no
			string[] sepPedido = dtSource.Rows[dgItemsPRec.EditItemIndex]["num_ped"].ToString().Split('-');
			string codI = "";
			Referencias.Guardar(dtSource.Rows[dgItemsPRec.EditItemIndex]["mite_codigo"].ToString(),ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[dgItemsPRec.EditItemIndex]["plin_codigo"].ToString()+"'"));
			if(sepPedido.Length == 2)
			{
				//Revisamos que el item exista en el pedido exista
				if(DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+sepPedido[0]+"' AND mped_numepedi="+sepPedido[1]+" AND mite_codigo='"+codI+"'"))
				{
					double cantidadComparacion = 0;
					try{cantidadComparacion = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi - dped_cantasig - dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+sepPedido[0]+"' AND mped_numepedi="+sepPedido[1]+" AND mite_codigo='"+codI+"'"));}
					catch{}
					if(cantidad > cantidadComparacion)
					{
                        Utils.MostrarAlerta(Response, "La cantidad ingresada superada la cantidad pedida!");
						return;
					}
				}
			}
			double precio = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
			double iva = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);
			double descuento = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
			if(descuento<0)descuento=0;
			if(descuento>100)descuento=100;
			if(iva<0)iva=0;
			if(iva>100)iva=100;
			double total = Total(precio,cantidad,descuento,iva);
			dtSource.Rows[dgItemsPRec.EditItemIndex]["mite_cantped"]=cantidad;
			dtSource.Rows[dgItemsPRec.EditItemIndex]["mite_precio"]=precio;
			dtSource.Rows[dgItemsPRec.EditItemIndex]["mite_iva"]=iva;
			dtSource.Rows[dgItemsPRec.EditItemIndex]["mite_desc"]=descuento;
			dtSource.Rows[dgItemsPRec.EditItemIndex]["mite_tot"]=total;
			dgItemsPRec.EditItemIndex=-1;
			BindDatas(true);    	
		}
    	
		public void RRDir_Update(Object sender, DataGridCommandEventArgs e)
		{//Este selecciona la fila
			if(dtSource.Rows.Count == 0)
				return;
			double cantidadFacturada = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
			if(cantidadFacturada < 0)
				cantidadFacturada = 0;
			//Revisamos que la cantidad a facturar no sea mayor que la cantidad pedida, en caso que exista pedido
			string[] sepPedido = dtSource.Rows[dgItemsRDir.EditItemIndex]["num_ped"].ToString().Split('-');
			string codI = "";
			Referencias.Guardar(dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_codigo"].ToString(),ref codI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[dgItemsRDir.EditItemIndex]["plin_codigo"].ToString()+"'"));
			if(sepPedido.Length == 2)
			{
				//Revisamos que el item exista en el pedido exista
				if(DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+sepPedido[0]+"' AND mped_numepedi="+sepPedido[1]+" AND mite_codigo='"+codI+"'"))
				{
					double cantidadComparacion = 0;
					try{cantidadComparacion = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi - dped_cantasig - dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+sepPedido[0]+"' AND mped_numepedi="+sepPedido[1]+" AND mite_codigo='"+codI+"'"));}
					catch{}
					if(cantidadFacturada > cantidadComparacion)
					{
                        Utils.MostrarAlerta(Response, "La cantidad facturada superada la cantidad pedida!");
						return;
					}
				}
			}
			double cantidadIngresada = Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
			if(cantidadIngresada < 0)
				cantidadIngresada = 0;
			double precio = 0, iva = 0, descuento = 0;
			try{precio = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);}
			catch{}
			try{iva = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);}
			catch{}
            if (!DBFunctions.RecordExist(
                    "select PIVA_PORCIVA from DBXSCHEMA.PIVA " +
                    "where piva_porciva=" + iva + ";"))
            {
                Utils.MostrarAlerta(Response, "No se ha definido el porcentaje de IVA indicado!");
                return;
            }
			try{descuento = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);}
			catch{}
			if(descuento<0)descuento=0;
			if(descuento>100)descuento=100;
			if(iva<0)iva=0;
			if(iva>100)iva=100;
			double total = Total(precio,cantidadFacturada,descuento,iva);
			dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_cantfac"]=cantidadFacturada;
			dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_cantped"]=cantidadIngresada;
			dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_precio"]=precio;
			dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_iva"]=iva;
			dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_desc"]=descuento;
			dtSource.Rows[dgItemsRDir.EditItemIndex]["mite_tot"]=total;
			dgItemsRDir.EditItemIndex=-1;
			BindDatas(true);
		}
		#endregion
		
		#region Edicion Grillas
		//EDITAR GRILLAS-----------------------------------------------------
		public void RLeg_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtSource.Rows.Count>0)
				dgItemsLeg.EditItemIndex=(int)e.Item.ItemIndex;
			BindDatas(false);		
		}
		
		public void RPRec_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtSource.Rows.Count>0)
				dgItemsPRec.EditItemIndex=(int)e.Item.ItemIndex;
			BindDatas(false);		
		}
		
		public void RRDir_Edit(Object sender, DataGridCommandEventArgs e)
		{
			if(dtSource.Rows.Count>0)
				dgItemsRDir.EditItemIndex=(int)e.Item.ItemIndex;
			BindDatas(false);
		}
		#endregion
		
		#region Operaciones Grillas
		//OPERACIONES GRILLAS----------------------------------------------------    
		public void RLeg_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			if(ddlTProc.SelectedValue!="2")
				return;
			if(((Button)e.CommandSource).CommandName == "AddDataRow" && CheckValues(e))
			{
                string[] arrayCodigoPreR = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue.Split('-');
                string prefijoPreRecep = arrayCodigoPreR[0];
                uint numeroPreRecep = 0;
                if (arrayCodigoPreR.Length > 2)
                {
                    prefijoPreRecep += "-" + arrayCodigoPreR[1];
                    numeroPreRecep = Convert.ToUInt32(arrayCodigoPreR[2]);
                }
                else
                {
                    numeroPreRecep = Convert.ToUInt32(arrayCodigoPreR[1]);
                }

                //string prefijoPreRecep = (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue.Split('-'))[0];
                //uint numeroPreRecep = Convert.ToUInt32((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue.Split('-'))[1]);
				
                string codigoItem = ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim();
				string codI = "";
				Referencias.Guardar(((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim(),ref codI,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[1]);
				//Revisamos si este item si pertenece a esta prerecepcion
                if (!DBFunctions.RecordExist("SELECT * FROM ditems WHERE pdoc_codigo='" + prefijoPreRecep + "' AND dite_numedocu=" + numeroPreRecep + " AND mite_codigo='" + codI + "'"))
				{
                    Utils.MostrarAlerta(Response, "El item seleccionado no pertenece a la prerecepción seleccionada!");
					return;
				}
				//Ahora revisamos si ya se ha agregado este item de esta prerecepcion
				if((dtSource.Select("num_ped='"+((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue+"' AND mite_codigo='"+codigoItem+"'")).Length > 0)
				{
                    Utils.MostrarAlerta(Response, "Este item de esta prerecepción ya ha sido agregado.\\nIntente Actualizarlo!");
					return;
				}
				double cantidadIngresada = 0, cantidadFacturada = 0, precio = 0, descuento = 0, iva = 0, total = 0;
				try{cantidadIngresada = Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text.Trim());}
				catch{}
				try{cantidadFacturada = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text.Trim());}
				catch{}
				//Debemos revisar que la cantidad facturada no sea mayor que la cantidad pedida, si existe un pedido
				string prefijoPedido = DBFunctions.SingleData("SELECT dite_prefdocurefe FROM ditems WHERE pdoc_codigo='"+prefijoPreRecep+"' AND dite_numedocu="+numeroPreRecep+" AND mite_codigo='"+codI+"'");
				string numeroPedido = DBFunctions.SingleData("SELECT dite_numedocurefe FROM ditems WHERE pdoc_codigo='"+prefijoPreRecep+"' AND dite_numedocu="+numeroPreRecep+" AND mite_codigo='"+codI+"'");
				//Ahora miramos si existe el registro en dpedidoitem
				if(DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+" AND mite_codigo='"+codI+"'"))
				{
					double cantidadComparacion = 0;
					try{cantidadComparacion = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi - dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+" AND mite_codigo='"+codI+"'"));}
					catch{}
					if(cantidadFacturada>cantidadComparacion)
					{
                        Utils.MostrarAlerta(Response, "La cantidad facturada superada la disponible para facturar.\\nCantidad disponible a facturar " + cantidadComparacion.ToString("N") + "!");
						return;
					}
				}
                
				//fin revision cantidad facturada
				try{precio = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text.Trim());}
				catch{}
				try{descuento = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text.Trim());}
				catch{}
				try{iva = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text.Trim());}
				catch{}
                if (DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "' AND TREG_REGIIVA IN('N','S');")) iva = 0;
				//Revisamos que la cantidad sea igual a cero
				//Si la cantidad facturada es mayor que la ingresada, no indica que ahi un faltante
				if(cantidadIngresada>0)
				{
					if(iva<0)iva=0;
					if(iva>100)iva=100;
					if(descuento<0)descuento=0;
					if(descuento>100)descuento=100;
					total = this.Total(precio,cantidadFacturada,descuento,iva);
					DataRow fila = dtSource.NewRow();
					fila["num_ped"] = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
					fila["mite_codigo"] = codigoItem;
					fila["mite_nombre"] = ((TextBox)e.Item.Cells[2].Controls[1]).Text.Trim();
					fila["mite_cantped"] = cantidadIngresada;
					fila["mite_cantfac"] = cantidadFacturada;
					fila["mite_precio"] = precio;
					fila["mite_desc"] = descuento;
					fila["mite_iva"] = iva;
					fila["mite_ubic"] = "";
					fila["mite_tot"] = total;
					fila["mite_unid"] = DBFunctions.SingleData("SELECT t3.puni_nombre FROM dbxschema.mitems AS t2, dbxschema.punidad AS t3 WHERE t3.puni_codigo=t2.puni_codigo AND t2.mite_codigo='"+codI+"'");
					fila["plin_codigo"] = (((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[0];
					dtSource.Rows.Add(fila);
				}
				else
				{
                    Utils.MostrarAlerta(Response, "Cantidad Ingresada Invalida. Revise Por favor!");
					return;
				}
			}
			if(((Button)e.CommandSource).CommandName == "AddDataRows")
			{
				//Aqui vamos a agregar una prerecepcion completa
				//Revisamos si el dropdownlist de la celda numero 1 esta vacio o no
				if(((DropDownList)e.Item.Cells[0].Controls[1]).Items.Count == 0)
				{
                    Utils.MostrarAlerta(Response, "No se ha seleccionado una prerecepción para agregar!");
					return;
				}
				string prefijoPreRecep = (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue.Split('-'))[0];
				uint numeroPreRecep = Convert.ToUInt32((((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue.Split('-'))[1]);
				//Traemos todos lo items que se encuentran relacionados en esta prerecepción
				DataSet da = new DataSet();
				DBFunctions.Request(da,IncludeSchema.NO,"SELECT DIT.mite_codigo, DBXSCHEMA.EDITARREFERENCIAS(DIT.mite_codigo,PLIN.plin_tipo), MIT.mite_nombre, MIT.plin_codigo, MIT.puni_codigo, DIT.dite_cantidad, DIT.dite_cantidad, DIT.dite_valounit, DIT.piva_porciva, DIT.dite_porcdesc FROM mitems MIT, ditems DIT, plineaitem PLIN WHERE DIT.mite_codigo = MIT.mite_codigo AND DIT.pdoc_codigo = '"+prefijoPreRecep+"' AND DIT.dite_numedocu = "+numeroPreRecep+" AND (DIT.dite_cantidad-DIT.dite_cantdevo)>0 AND PLIN.plin_codigo=MIT.plin_codigo ORDER BY MIT.plin_codigo,DIT.mite_codigo");
                bool sinIva=DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "' AND TREG_REGIIVA IN('N','S');");
				//Ahora vamos agregando item a item dentro de la tabla
				for(int i=0;i<da.Tables[0].Rows.Count;i++)
				{
					double cantidadIngresada = 0, precio = 0, descuento = 0, iva = 0, total = 0;
					string codI = "";
					Referencias.Guardar(da.Tables[0].Rows[i][1].ToString(),ref codI,da.Tables[0].Rows[i][3].ToString());
					//Revisamos si este item ya fue agregado para esta prerecepcion
					if(dtSource.Select("num_ped='"+((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue+"' AND mite_codigo='"+codI+"'").Length>0)
                        Utils.MostrarAlerta(Response, "El item "+codI+" ya se ha agregado para la prerecepción "+((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue+" .\\nPor favor intente actualizando!");
					else
					{
						try{cantidadIngresada = Convert.ToDouble(da.Tables[0].Rows[i][5]);}
						catch{}
                        // PRECIO, DESCUENTO, IVA Y PROMEDIOS LOS DEBE TRAR DE LA PRERECEPCION
					//	try{precio = double.Parse(DBFunctions.SingleData("SELECT msal_ulticost FROM dbxschema.msaldoitem WHERE mite_codigo='"+codI+"'"));}
					//	catch{}
                        try { precio = Convert.ToDouble(da.Tables[0].Rows[i][7]); }
                        catch { }
					    try{descuento = Convert.ToDouble(da.Tables[0].Rows[i][9]);}
						catch{}
						try{iva = Convert.ToDouble(da.Tables[0].Rows[i][8]);}
						catch{}
                        if(sinIva || iva<0)iva=0;
						if(iva>100)iva=100;
						if(descuento<0)descuento=0;
						if(descuento>100)descuento=100;
						total = this.Total(precio,cantidadIngresada,descuento,iva);
						DataRow fila = dtSource.NewRow();
						fila["num_ped"] = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
						fila["mite_codigo"] = da.Tables[0].Rows[i][1].ToString();
						fila["mite_nombre"] = da.Tables[0].Rows[i][2].ToString();
						fila["mite_cantped"] = cantidadIngresada;
						fila["mite_cantfac"] = cantidadIngresada;
						fila["mite_precio"] = precio;
						fila["mite_desc"] = descuento;
						fila["mite_iva"] = iva;
						fila["mite_ubic"] = "";
						fila["mite_tot"] = total;
						fila["mite_unid"] =  da.Tables[0].Rows[i][4].ToString();
						fila["plin_codigo"] =  da.Tables[0].Rows[i][3].ToString();
						dtSource.Rows.Add(fila);
					}
				}
			}
			if(((Button)e.CommandSource).CommandName == "ClearRows")
				dtSource.Rows.Clear();
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					dtSource.Rows.Remove(dtSource.Rows[e.Item.ItemIndex]);
					dgItemsLeg.EditItemIndex=-1;
				}
				catch{};
			}
       		BindDatas(true);
            if(dtSource.Rows.Count==1)
                btnAjus.Enabled = true;
            else
                if (dtSource.Rows.Count == 0)
                    btnAjus.Enabled = false;
		}
		


		protected void RPRec_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			bool pasar = true;
			if(ddlTProc.SelectedValue!="0")
				return;
			if(((Button)e.CommandSource).CommandName == "AddDataAll" )
			{
				//Pedido
				DataSet da = new DataSet();
				double total = 0, cantidadIngresada = 0, iva = 0, precio = 0, descuento = 0;
				string pedido = "";
				string prefijo = "";
				string numeropedido = "";
                bool sinIva = DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "' AND TREG_REGIIVA IN('N','S');");
				pedido = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
				string[] tem = pedido.Split('-');
				numeropedido = tem[1];
				prefijo = tem[0];
				foreach(DataRow Dr in dtSource.Rows)
				{
					if (Dr[0].ToString() == pedido) pasar = false;
				}			        
				if (pasar)
				{
					DBFunctions.Request(da, IncludeSchema.NO,"SELECT a.*,b.mite_nombre,b.plin_codigo,b.puni_codigo,DBXSCHEMA.EDITARREFERENCIAS(a.mite_codigo,(SELECT plin_tipo FROM plineaitem WHERE plin_codigo=b.plin_codigo)) FROM dpedidoItem a,mitems b WHERE a.mped_numepedi="+numeropedido+ " and a.pped_codigo='" + prefijo + "' and b.mite_codigo=a.mite_codigo");
					foreach(DataRow Dr in da.Tables[0].Rows)
					{
                        if (sinIva) iva = 0;
                        else iva = double.Parse(Dr[10].ToString());
						descuento = double.Parse(Dr[9].ToString());
						precio = double.Parse(Dr[8].ToString());
                        if(precio.ToString()=="")
						    precio = double.Parse(DBFunctions.SingleData("SELECT msal_ulticost FROM dbxschema.msaldoitem WHERE mite_codigo='"+Dr[4].ToString()+"'"));
						cantidadIngresada = double.Parse(Dr[5].ToString())-double.Parse(Dr[6].ToString())-double.Parse(Dr[7].ToString());
						if (cantidadIngresada > 0)
						{
							if(iva<0)iva=0;
							if(iva>100)iva=100;
							if(descuento<0)descuento=0;
							if(descuento>100)descuento=100;
							total = Total(precio,cantidadIngresada,descuento,iva);
							DataRow fila = dtSource.NewRow();
							fila["num_ped"] = pedido;
							fila["mite_codigo"] = Dr[16];
							fila["mite_nombre"] = Dr[13].ToString();
							fila["mite_cantped"] = cantidadIngresada;
							fila["mite_cantfac"] = cantidadIngresada;
							fila["mite_precio"] = precio;
							fila["mite_desc"] = descuento;
							fila["mite_iva"] = iva;
							fila["mite_ubic"] = "";
							fila["mite_tot"] = total;
							fila["mite_unid"] = Dr[15];
							fila["plin_codigo"] = Dr[14];
							dtSource.Rows.Add(fila);
						}
					}
				}
				else
				{
                    Utils.MostrarAlerta(Response, "El pedido " + pedido.Trim() + " ya está Agregado!");
					return;
				}
			}
			
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValues(e))
			{
				//Pedido
				
				if(dtSource.Rows.Count==0)
					if(!ValidarDatos())
						return;
				string pedido = "", codI = "";
				string itemS =  ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim();
                bool sinIva = DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "' AND TREG_REGIIVA IN('N','S');");
				if(!Referencias.Guardar(itemS,ref codI,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[1]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " no es valido para la linea de bodega " + ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedItem.Text + ".\\nRevise Por Favor!");
					return;
				}
				if(!Referencias.RevisionSustitucion(ref codI,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[0]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " no se encuentra registrado.\\nRevise Por Favor.!");
					return;
				}
				string itemS2 = "";
				Referencias.Editar(codI,ref itemS2,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[1]);
				if(itemS != itemS2)
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " ha sido sustituido.El codigo actual es " + itemS2 + "!");
				string[] spPedido = null;
				double cantidadComparacion = 0;
				int indicadorPedido = 0;//Si este indicador esta en 0 no revisamos la cantidad pedida, si esta en 1 revisamos que no sea mayor
				//Primero determinamos si el item que voy a agregar pertenece al pedido seleccionado en el dropdownlist
				//O si este es un pedido ingresado por el cuadro de texto 
				if(((TextBox)e.Item.Cells[0].Controls[3]).Text != "")
					pedido = ((TextBox)e.Item.Cells[0].Controls[3]).Text;
				else
				{
					indicadorPedido = 1;
					pedido = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
					spPedido =  pedido.Split('-');
					//Determinamos si el item que itenta agregar se encuentra dentro del pedido seleccionado
					if(!DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='"+spPedido[0]+"' AND mped_numepedi="+spPedido[1]+" AND mite_codigo='"+codI+"'"))
					{
                        Utils.MostrarAlerta(Response, "El item " + itemS2 + " no hace parte del pedido " + ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue + "!");
						return;
					}
					cantidadComparacion = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi - dped_cantasig - dped_cantfact FROM dpedidoitem WHERE pped_codigo='"+spPedido[0]+"' AND mped_numepedi="+spPedido[1]+" AND mite_codigo='"+codI+"'"));
				}
				//Ahora revisamos si dentro de la grilla ya hemos agregado este item para este pedido
				if((dtSource.Select("num_ped='"+pedido+"' AND mite_codigo='"+itemS2+"'")).Length>0)
				{
                    Utils.MostrarAlerta(Response, "el item " + itemS2 + " para el pedido " + pedido + " ya ha sido agregado.\\nIntente modificarlo!");
					return;
				}
				//Ahora vamos a crear la fila y agregarla a la tabla dtSource
				//OJO POR EL MOMENTO SE DEJARA LA UBICACION VACIA
				double total = 0, cantidadIngresada = 0, iva = 0, precio = 0, descuento = 0;
				try{cantidadIngresada = Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);}
				catch{};
				//si el indicador nos dice que debemos revisar la cantidad la revisamos
				if(indicadorPedido == 1)
				{
					if(cantidadIngresada > cantidadComparacion)
					{
                        Utils.MostrarAlerta(Response, "La cantidad que esta intentanto agregar supera la cantidad pedida!");
						return;
					}
				}
				try{precio = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);}
				catch{};
				try{iva = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);}
				catch{};
				try{descuento = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);}
				catch{};
				if(sinIva || iva<0)iva=0;
				if(iva>100)iva=100;
				if(descuento<0)descuento=0;
				if(descuento>100)descuento=100;
				total = Total(precio,cantidadIngresada,descuento,iva);
				DataRow fila = dtSource.NewRow();
				fila["num_ped"] = pedido;
				fila["mite_codigo"] = itemS2;
				if(((TextBox)e.Item.Cells[2].Controls[1]).Text != "")
					fila["mite_nombre"] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
				else
					fila["mite_nombre"] = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
				fila["mite_cantped"] = cantidadIngresada;
				fila["mite_cantfac"] = 0;
				fila["mite_precio"] = precio;
				fila["mite_desc"] = descuento;
				fila["mite_iva"] = iva;
				fila["mite_ubic"] = "";
				fila["mite_tot"] = total;
				fila["mite_unid"] = DBFunctions.SingleData("SELECT t3.puni_nombre FROM dbxschema.mitems AS t2, dbxschema.punidad AS t3 WHERE t3.puni_codigo=t2.puni_codigo AND t2.mite_codigo='"+codI+"'");
				fila["plin_codigo"] = (((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[0];
				dtSource.Rows.Add(fila);
				
			}
			if(((Button)e.CommandSource).CommandName == "ClearRows")
				dtSource.Rows.Clear();
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					dtSource.Rows.Remove(dtSource.Rows[e.Item.ItemIndex]);
					dgItemsPRec.EditItemIndex=-1;
				}
				catch{};
			}
			BindDatas(true);
		}
		


		protected void RRDir_AddAndDel(object sender, DataGridCommandEventArgs e)
		{
			bool pasar = true;

			if(ddlTProc.SelectedValue!="1")
				return;
			if(((Button)e.CommandSource).CommandName == "AddDataAll" )
			{
				//Pedido
				
				DataSet da = new DataSet();
				double total = 0, cantidadIngresada = 0, iva = 0, precio = 0, descuento = 0;
				string pedido = "";
				string prefijo = "";
				string numeropedido = "";
                bool sinIva = DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "' AND TREG_REGIIVA IN('N','S');");
				pedido = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
				string[] tem = pedido.Split('-');
				numeropedido = tem[1];
				prefijo = tem[0];
				foreach(DataRow Dr in dtSource.Rows)
				{
					if (Dr[0].ToString() == pedido) pasar = false;
				}			        
				if (pasar)
				{
					DBFunctions.Request(da, IncludeSchema.NO,"SELECT a.*,b.mite_nombre,b.plin_codigo,b.puni_codigo,DBXSCHEMA.EDITARREFERENCIAS(a.mite_codigo,(SELECT plin_tipo FROM dbxschema.plineaitem WHERE plin_codigo=b.plin_codigo)) FROM dbxschema.dpedidoItem a,dbxschema.mitems b WHERE a.mped_numepedi="+numeropedido+" and a.pped_codigo='"+prefijo+"' and b.mite_codigo=a.mite_codigo");
					
					foreach(DataRow Dr in da.Tables[0].Rows)
					{
                        if (sinIva) iva = 0;
                        else iva = double.Parse(Dr[10].ToString());
						descuento = double.Parse(Dr[9].ToString());
						precio = double.Parse(Dr[8].ToString());
						cantidadIngresada = double.Parse(Dr[5].ToString())-double.Parse(Dr[6].ToString())-double.Parse(Dr[7].ToString());
						if (cantidadIngresada > 0)
						{
							if(iva<0)iva=0;
							if(iva>100)iva=100;
							if(descuento<0)descuento=0;
							if(descuento>100)descuento=100;
							total = Total(precio,cantidadIngresada,descuento,iva);
							DataRow fila = dtSource.NewRow();
							fila[0] = pedido;
							fila[1] = Dr[16].ToString();
							fila[2] = Dr[13].ToString();
							fila[3] = cantidadIngresada;
							fila[4] = cantidadIngresada;
							fila[5] = precio;
							fila[6] = descuento;
							fila[7] = iva;
							fila[8] = "";
							fila[9] = total;
							fila[10] = Dr[15];
							fila[11] = Dr[14];
							dtSource.Rows.Add(fila);
						}
					}
				}
				else
				{
                    Utils.MostrarAlerta(Response, "El pedido " + pedido.Trim() + " ya está Agregado!");
					return;
				}
			}
						
			if(((Button)e.CommandSource).CommandName == "AddDatasRow" && CheckValues(e))
			{
				//Pedido
				if(!ValidarDatos())return;
				string pedido = "", codI = "";
				string itemS =  ((TextBox)e.Item.Cells[1].Controls[1]).Text.Trim();
                bool sinIva = DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "' AND TREG_REGIIVA IN('N','S');");
				if(!Referencias.Guardar(itemS,ref codI,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[1]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " no es valido para la linea de bodega " + ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedItem.Text + ".\\nRevise Por Favor!");
					return;
				}
				if(!Referencias.RevisionSustitucion(ref codI,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[0]))
				{
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " no se encuentra registrado.\\nRevise Por Favor.!");
					return;
				}
                if (DBFunctions.SingleData("SELECT TORI_CODIGO FROM MITEMS WHERE MITE_CODIGO = '" + itemS + "' ") == "Z") // es un servicio, No se graban los acumaulados, solo el kardex
                {
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " corresponde a un Servicio y no se permite comprar SERVICIOS.\\nRevise Por Favor.!");
                    return;
                }
                string itemS2 = "";
				Referencias.Editar(codI,ref itemS2,(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[1]);
				if(itemS != itemS2)
                    Utils.MostrarAlerta(Response, "El codigo " + itemS + " ha sido sustituido.El codigo actual es " + itemS2 + "!");
				string[] spPedido = null;
				double cantidadComparacion = 0;
				int indicadorPedido = 0;//Si este indicador esta en 0 no revisamos la cantidad pedida, si esta en 1 revisamos que no sea mayor
				//Primero determinamos si el item que voy a agregar pertenece al pedido seleccionado en el dropdownlist
				//O si este es un pedido ingresado por el cuadro de texto 
                if (((TextBox)e.Item.Cells[0].Controls[3]).Text != "")
                {
                    pedido = ((TextBox)e.Item.Cells[0].Controls[3]).Text;

                    //guardo el nombre del pedido para volverlo a mostrar en el databind
                    ViewState["nomPedido"] = ((TextBox)e.Item.Cells[0].Controls[3]).Text;
                }
                else
                {
                    indicadorPedido = 1;
                    pedido = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
                    spPedido = pedido.Split('-');
                    //Determinamos si el item que itenta agregar se encuentra dentro del pedido seleccionado
                    if (!DBFunctions.RecordExist("SELECT * FROM dpedidoitem WHERE pped_codigo='" + spPedido[0] + "' AND mped_numepedi=" + spPedido[1] + " AND mite_codigo='" + codI + "'"))
                    {
                        Utils.MostrarAlerta(Response, "El item " + itemS2 + " no hace parte del pedido " + ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue + "!");
                        return;
                    }
                    cantidadComparacion = Convert.ToDouble(DBFunctions.SingleData("SELECT dped_cantpedi - dped_cantasig - dped_cantfact FROM dpedidoitem WHERE pped_codigo='" + spPedido[0] + "' AND mped_numepedi=" + spPedido[1] + " AND mite_codigo='" + codI + "'"));
                }
				//Ahora revisamos si dentro de la grilla ya hemos agregado este item para este pedido
				if((dtSource.Select("num_ped='"+pedido+"' AND mite_codigo='"+itemS2+"'")).Length>0)
				{
                    Utils.MostrarAlerta(Response, "El item " + itemS2 + " para el pedido " + ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue + " ya ha sido agregado.\\nIntente modificarlo!");
					return;
				}
				//OJO POR EL MOMENTO SE DEJARA LA UBICACION VACIA
				double total = 0, cantidadIngresada = 0, cantidadFacturada =0, iva = 0, precio = 0, descuento = 0;
				try{cantidadIngresada = Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);}
				catch{};
				try{cantidadFacturada = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);}
				catch{};
				//si el indicador nos dice que debemos revisar la cantidad la revisamos
				if(indicadorPedido == 1)
				{
					if(cantidadFacturada > cantidadComparacion)
					{
                        Utils.MostrarAlerta(Response, "La cantidad que esta intentanto facturar supera la cantidad pedida!");
						return;
					}
				}
				try{precio = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);}
				catch{};
				try{iva = Convert.ToDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text);}
				catch{};
                if (!DBFunctions.RecordExist(
                    "select PIVA_PORCIVA from DBXSCHEMA.PIVA "+
                    "where piva_porciva="+iva+";")){
                        Utils.MostrarAlerta(Response, "No se ha definido el porcentaje de IVA indicado!");
                        return;
                }
				try{descuento = Convert.ToDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text);}
				catch{};
                if (sinIva || iva < 0) iva = 0;
				if(iva>100)iva=100;
				if(descuento<0)descuento=0;
				if(descuento>100)descuento=100;
				total = Total(precio,cantidadFacturada,descuento,iva);
				DataRow fila = dtSource.NewRow();
				fila[0] = pedido;
				fila[1] = itemS2;
				if(((TextBox)e.Item.Cells[2].Controls[1]).Text != "")
					fila[2] = ((TextBox)e.Item.Cells[2].Controls[1]).Text;
				else
					fila[2] = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codI+"'");
				fila[3] = cantidadIngresada;
				fila[4] = cantidadFacturada;
				fila[5] = precio;
				fila[6] = descuento;
				fila[7] = iva;
				fila[8] = "";
				fila[9] = total;
				fila[10] = DBFunctions.SingleData("SELECT t3.puni_nombre FROM dbxschema.mitems AS t2, dbxschema.punidad AS t3 WHERE t3.puni_codigo=t2.puni_codigo AND t2.mite_codigo='"+codI+"'");
				fila[11] = (((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim().Split('-'))[0];
				dtSource.Rows.Add(fila);
				
			}
			if(((Button)e.CommandSource).CommandName == "ClearRows")
				dtSource.Rows.Clear();
			if(((Button)e.CommandSource).CommandName == "DelDatasRow")
			{
				try
				{
					dtSource.Rows.Remove(dtSource.Rows[e.Item.ItemIndex]);
					dgItemsRDir.EditItemIndex=-1;
				}
				catch{};
			}
			BindDatas(true);
		}
		#endregion
		
		#region Otros
		//VALIDAR DATOS DE FACTURA, NIT, etc.------------------------------------------------------------
		protected bool ValidarDatos()
		{
			uint ndref, ndrefE;
			int diasP;
			if(ddlTProc.SelectedValue!="0") //si el proceso seleccionado no es una prerecepcion
			{
				try{ndref=Convert.ToUInt32(txtNumFac.Text);ndrefE=Convert.ToUInt32(txtNumFacE.Text);}
                catch
                {
                    Utils.MostrarAlerta(Response, "Uno de los numeros de factura no es valido!");return(false);
                }
				try{diasP=Convert.ToInt16(txtPlazo.Text);}
                catch
                {
                    Utils.MostrarAlerta(Response, "Uno de los numeros de factura no es valido!");return(false);
                }
				////Se debe revisar si no existe esa factura de proveedor dentro de mfacturaproveedor
    //            if (DBFunctions.RecordExist("SELECT mfac_numedocu FROM mfacturaproveedor WHERE mfac_prefdocu='" + txtPref.Text.Trim() + "' AND mfac_numedocu=" + txtNumFac.Text.Trim() + " and mnit_nit='" + txtNIT.Text + "';"))
				//{
    //                Utils.MostrarAlerta(Response, "La factura del proveedor " + txtNIT.Text + " con prefijo-número : " + txtPref.Text.Trim() + "-" + txtNumFac.Text.Trim() + " ya se ha ingresado anteriormente.\\nRevise Por Favor!");
				//	return(false);
				//}
			}
            if (txtNIT.Text.Length == 0)
                return(false);
            else
            {
			    if(!DBFunctions.RecordExist("Select t1.mnit_nit from DBXSCHEMA.MNIT as t1, DBXSCHEMA.MPROVEEDOR as t2 where t1.mnit_nit=t2.mnit_nit and t1.mnit_nit='"+txtNIT.Text+"'"))
			    {
                    Utils.MostrarAlerta(Response, "No existe el NIT del proveedor dado!");
				    return(false);
			    }
            }
            DateTime fechaCal = Convert.ToDateTime(tbDate.Text.ToString());
            if (!DBFunctions.RecordExist("Select pano_ano from CINVENTARIO where pano_ano = " + fechaCal.Year.ToString() + " and pmes_mes = " + fechaCal.Month.ToString() + ";"))
            {
                Utils.MostrarAlerta(Response, "El año y-o el mes del documento son diferentes a la vigencia de Inventarios ...!!");
                return (false);
            }
            return (true);   
		}
		
		//BINDDATAS---------------------
		protected void BindDatas(bool recargaR)
		{
            string nacionalidad = DBFunctions.SingleData("SELECT TNAC_TIPONACI FROM MNIT WHERE MNIT_NIT='" + txtNIT.Text + "';");
            ViewState["NACIONALIDAD"] = nacionalidad;

            int i,j;
			bool ed = false;
			
			if(dtSource.Rows.Count==0)
			{
				dgItemsLeg.EditItemIndex=dgItemsRDir.EditItemIndex=dgItemsPRec.EditItemIndex=-1;
				ed=true;
			}
			//En caso que existan filas dentro de la tabla se bloquea el acceso a los controles de configuracion
			chkImportacion.Enabled=ddlVendedor.Enabled = txtNITa.Enabled = txtNumFac.Enabled = tbDate.Enabled = txtPlazo.Enabled = txtNumFacE.Enabled = ddlPrefE.Enabled = btnSelecNIT.Enabled = txtNIT.Enabled = txtPref.Enabled = ddlAlmacen.Enabled = ddlTProc.Enabled = ddlMedio.Enabled = ed;
			btnAjus.Enabled = !ed;
			if(txtNIT.Text.Length==0)
				return;
			dgItemsPRec.Visible = dgItemsRDir.Visible = dgItemsLeg.Visible = false;
			//Se determina cual debe ser la grilla activa
			DataGrid dgAct = null;
			switch(ddlTProc.SelectedValue)
			{
				case "0":
					dgAct = dgItemsPRec;break;	//Prerecepcion
				case "1":
					dgAct = dgItemsRDir;break;	//recepcion directa
				case "2":
					dgAct = dgItemsLeg;break;	//Legalizacion
			}
			dgAct.Visible = true;
			dgAct.DataSource = dtSource;
			dgAct.DataBind();
			//Justificacion
			if(ddlTProc.SelectedValue == "0")
			{
				for(i=5;i<=9;i++)
					for(j=0;j<dgItemsPRec.Items.Count;j++)
						dgItemsPRec.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			}
			if(ddlTProc.SelectedValue == "1")
			{
				for(i=5;i<=10;i++)
					for(j=0;j<dgItemsRDir.Items.Count;j++)
						dgItemsRDir.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			}
			if(ddlTProc.SelectedValue == "2")
			{
				for(i=5;i<=10;i++)
					for(j=0;j<dgItemsLeg.Items.Count;j++)
						dgItemsLeg.Items[j].Cells[i].HorizontalAlign = HorizontalAlign.Right;
			}
            //Fin Justificacion	
            //*************************
            //Inicio Manejo Ubicaciones
            //Debemos consultar cuales son las ubicaciones que se le han asignado a esta referencia dentro del almacen que se esta realizando el proceso
            string almacen, codI;
            DataSet dsDatos = new DataSet();
            DBFunctions.Request(dsDatos, IncludeSchema.NO, "SELECT plin_codigo, plin_tipo FROM dbxschema.plineaitem;");
            string nivel1;
            string nivel2;
            string nivel3;
            for (i=0;i<dtSource.Rows.Count;i++)
			{
				almacen = ddlAlmacen.SelectedValue;
				codI = "";

                Referencias.Guardar(dtSource.Rows[i]["mite_codigo"].ToString(), ref codI, (dtSource.Rows[i]["plin_codigo"].ToString() == ""? "":dsDatos.Tables[0].Select("PLIN_CODIGO = '" + dtSource.Rows[i]["plin_codigo"] + "'")[0].ItemArray[1].ToString()));//DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='"+dtSource.Rows[i]["plin_codigo"]+"'"));
                                                                                                                                                                                                          //Reinicio los DropDownList de las ubicaciones
                                                                                                                                                                                                          //Ahora trameos las ubicaciones que estan asociadas a este item en este almacen

                /*DataSet da = new DataSet();
				DBFunctions.Request(da,IncludeSchema.NO,"SELECT MUB.pubi_codigo FROM mubicacionitem MUB, pubicacionitem PUB WHERE MUB.pubi_codigo = PUB.pubi_codigo AND PUB.palm_almacen = '"+almacen+"' AND MUB.mite_codigo = '"+codI+"'");
				if(ddlTProc.SelectedValue == "0")
					((DropDownList)dgItemsPRec.Items[i].Cells[10].Controls[1]).Items.Clear();
				if(ddlTProc.SelectedValue == "1")
					((DropDownList)dgItemsRDir.Items[i].Cells[11].Controls[1]).Items.Clear();
				if(da.Tables[0].Rows.Count == 0)
				{
					if(ddlTProc.SelectedValue == "0")
						((DropDownList)dgItemsPRec.Items[i].Cells[10].Controls[1]).Items.Add(new ListItem("No Existe Ninguna Ubicación",""));
					if(ddlTProc.SelectedValue == "1")
						((DropDownList)dgItemsRDir.Items[i].Cells[11].Controls[1]).Items.Add(new ListItem("No Existe Ninguna Ubicación",""));
				}
				else
				{
					for(j=0;j< da.Tables[0].Rows.Count;j++)
					{
						//Traemos el nombre de la ubicacion de nivel1, nivel2 y nivel3 de cada ubicacion que se ha traido en da
						nivel1 = DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+ da.Tables[0].Rows[j][0].ToString()+"))");
						nivel2 = DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo=(SELECT pubi_codpad FROM pubicacionitem WHERE pubi_codigo="+ da.Tables[0].Rows[j][0].ToString()+")");
						nivel3 = DBFunctions.SingleData("SELECT pubi_nombre FROM pubicacionitem WHERE pubi_codigo="+ da.Tables[0].Rows[j][0].ToString()+"");
						if(ddlTProc.SelectedValue == "0")
							((DropDownList)dgItemsPRec.Items[i].Cells[10].Controls[1]).Items.Add(new ListItem(nivel1+" "+nivel2+" "+nivel3, da.Tables[0].Rows[j][0].ToString()));
						if(ddlTProc.SelectedValue == "1")
							((DropDownList)dgItemsRDir.Items[i].Cells[11].Controls[1]).Items.Add(new ListItem(nivel1+" "+nivel2+" "+nivel3, da.Tables[0].Rows[j][0].ToString()));
					}
				}*/
                if (ddlTProc.SelectedValue == "0")
					((Label)(dgItemsPRec.Items[i].Cells[10].Controls[3])).Attributes.Add("onclick","ModalDialogUbic('N','"+ddlAlmacen.SelectedValue+"','"+dtSource.Rows[i]["mite_codigo"].ToString()+"','"+codI+"',"+((DropDownList)dgItemsPRec.Items[i].Cells[10].Controls[1]).ClientID+");");
				if(ddlTProc.SelectedValue == "1")
					((Label)(dgItemsRDir.Items[i].Cells[11].Controls[3])).Attributes.Add("onclick","ModalDialogUbic('N','"+ddlAlmacen.SelectedValue+"','"+dtSource.Rows[i]["mite_codigo"].ToString()+"','"+codI+"',"+((DropDownList)dgItemsRDir.Items[i].Cells[11].Controls[1]).ClientID+");");
			}
			//Fin Manejo Ubicaciones
			//*************************
			if(ddlTProc.SelectedValue != "2")
				plhFile.Visible = true;//No legalizar, permite cargar archivo
			Session["dtInsertsRI"] = dtSource;
			double t=0, st=0, td=0 ,ti=0 ,tif=0 ,tf=0;
			//total,subtotal,totaldescuento,totaliva,totalivafletes
			int n=0;
			double nI=0;
			string indiceCantidad = "mite_cantfac";
			if(ddlTProc.SelectedValue == "0")
				indiceCantidad = "mite_cantped";
			//numero Items
			for(n=0;n<dtSource.Rows.Count;n++)
			{
				//t += Convert.ToDouble(dtSource.Rows[n]["mite_tot"]);
				st += Convert.ToDouble(dtSource.Rows[n][indiceCantidad])*Convert.ToDouble(dtSource.Rows[n]["mite_precio"]);
				nI += Convert.ToDouble(dtSource.Rows[n][indiceCantidad]);
				td += (Convert.ToDouble(dtSource.Rows[n][indiceCantidad])*Convert.ToDouble(dtSource.Rows[n]["mite_precio"]))*(Convert.ToDouble(dtSource.Rows[n]["mite_desc"])/100);
				ti += (((Convert.ToDouble(dtSource.Rows[n][indiceCantidad])*Convert.ToDouble(dtSource.Rows[n]["mite_precio"]))-(Convert.ToDouble(dtSource.Rows[n][indiceCantidad])*Convert.ToDouble(dtSource.Rows[n]["mite_precio"]))*(Convert.ToDouble(dtSource.Rows[n]["mite_desc"])/100))*(Convert.ToDouble(dtSource.Rows[n]["mite_iva"])/100));
			}
			txtNumItem.Text = dtSource.Rows.Count.ToString();
			tf = Convert.ToDouble(txtFlet.Text);
            td = Math.Round(td, numDecimales);
            ti = Math.Round(ti, numDecimales);
			tif = Math.Round((tf*Convert.ToDouble(ddlPIVA.SelectedItem.Value)/100),numDecimales);
			st = Math.Round(st, numDecimales);
			t = st-td+ti;
			txtTotIF.Text = tif.ToString("C");
			txtGTot.Text = (t+tif+tf).ToString("C");
			txtNumUnid.Text = nI.ToString();
			//txtSubTotal.Attributes.Add("onFocus","self.status='"+st.ToString("0.00")+"';");
			txtSubTotal.Text = st.ToString("C");
			txtIVA.Text = ti.ToString("C");
			txtDesc.Text = td.ToString("C");
			txtTotal.Text = t.ToString("C");

			//NIT EXTRANJERO?
			if(ddlTProc.SelectedValue=="1" && nacionalidad=="E" && chkImportacion.Checked)
			{
				dgAct.ShowFooter = false;
				dgAct.Columns[dgAct.Columns.Count-2].Visible = false;
				dgAct.Columns[dgAct.Columns.Count-3].Visible = false;
				dgAct.Columns[dgAct.Columns.Count-4].Visible = false;
				dgAct.Columns[0].Visible = false;
				dgAct.Columns[3].Visible = false;
				dgAct.Columns[4].Visible = false;
				plhFile.Visible = false;
				plhEmbarque.Visible = true;
				bind.PutDatasIntoDropDownList(ddlEmbarque, "SELECT DISTINCT MEM.MEMB_SECUENCIA, MEM.MEMB_NUMEEMBA FROM MEMBARQUE MEM,DEMBARQUE DEM WHERE MEM.MEMB_SECUENCIA=DEM.MEMB_SECUENCIA AND MEM.PEST_ESTADO='E';");
			}
			else if(ddlTProc.SelectedValue=="1")
				dgAct.Columns[dgAct.Columns.Count-1].Visible = false;
		}

		//CREAR TABLA------------------------	
		protected void LoadDataColumns()
		{
			lbFields.Add("num_ped");//0 Numero pedido o Prerecepcion
			types.Add(typeof(string));
			lbFields.Add("mite_codigo");//1 codigo Item
			types.Add(typeof(string));
			lbFields.Add("mite_nombre");//2 nombre Item
			types.Add(typeof(string));
			lbFields.Add("mite_cantped");//3 Cantidad ingresada
			types.Add(typeof(double));
			lbFields.Add("mite_cantfac");//4 Cantidad facturada
			types.Add(typeof(double));
			lbFields.Add("mite_precio");//5 Valor unidad
			types.Add(typeof(double));
			lbFields.Add("mite_desc");//6 Descuento
			types.Add(typeof(double));
			lbFields.Add("mite_iva");//7 Iva
			types.Add(typeof(double));
			lbFields.Add("mite_ubic");//8 Ubicacion
			types.Add(typeof(string));
			lbFields.Add("mite_tot");//9 Total
			types.Add(typeof(double));
			lbFields.Add("mite_unid");//10 Unidad medida
			types.Add(typeof(string));
			lbFields.Add("plin_codigo");//11 Linea Bodega
			types.Add(typeof(string));
			lbFields.Add("mite_us");//12 Valor moneda extranjera
			types.Add(typeof(double));
		}
		
		protected void LoadDataTable()
		{
			dtSource = new DataTable();
			for(int i=0; i<lbFields.Count; i++)
				dtSource.Columns.Add(new DataColumn((string)lbFields[i], (Type)types[i]));
		}
		
		//CALCULAR TOTALES------------------------------------------------------
		private double Total(double pre, double cant ,double desc, double iva)
		{
			double tot = 0;
			tot = cant*pre;
			tot = tot-((Math.Round(desc/100,0))*tot);
			tot = tot+(tot*(Math.Round(iva/100,0)));
			return(tot);
		}
	
		//INSERTAR UN PEDIDO---------------------------
		private bool InsertarPedido(string pedido)
		{
			//Revisart que no este:
			int n,m;
			//Insertarlo
			ds=new DataSet();
			DBFunctions.Request(ds, IncludeSchema.NO,"select t1.MITE_CODIGO,t2.mite_nombre,t1.dped_cantpedi-t1.dped_cantfact,t1.dped_valounit,t1.piva_porciva,t1.dped_porcdesc,t3.puni_nombre from dpedidoitem as t1,mitems as t2,punidad as t3 where t3.puni_codigo=t2.puni_codigo and t2.mite_codigo=t1.mite_codigo and t1.mped_clasregi='P' and t1.MNIT_NIT='"+txtNIT.Text+"' AND t1.MPED_NUMEPEDI="+pedido);
			DataRow nr;
			int c=0;
			bool ex;
			for(n=0;n<ds.Tables[0].Rows.Count;n++)
			{
				double tot,cant,iva,pr,desc;
				cant=Convert.ToDouble(ds.Tables[0].Rows[n][2].ToString());
				iva=Convert.ToDouble(ds.Tables[0].Rows[n][4].ToString());
				desc=Convert.ToDouble(ds.Tables[0].Rows[n][5].ToString());
				pr=Convert.ToDouble(ds.Tables[0].Rows[n][3].ToString());
				tot=Total(pr,cant,desc,iva);
				nr=dtSource.NewRow();
				string Icod=ds.Tables[0].Rows[n][0].ToString();
				ex=false;
				for(m=0;m<dtSource.Rows.Count;m++)
					if(dtSource.Rows[m]["num_ped"].ToString()==pedido && dtSource.Rows[m]["mite_codigo"].ToString()==Icod)
					{
						ex=true;
						break;
					}
				if(!ex)
				{
					string Ubic=DBFunctions.SingleData("SELECT mubi_ubicacion FROM mubicacionitem where mite_codigo='"+Icod+"' AND palm_almacen='"+ddlAlmacen.SelectedValue+"'");
					nr[0]=pedido;
					nr[1]=Icod;
					nr[2]=ds.Tables[0].Rows[n][1].ToString();
					nr[3]=cant;
					nr[4]=Convert.ToDouble(ds.Tables[0].Rows[n][2].ToString());
					nr[5]=pr;
                    nr[6]=desc;
					nr[7]=iva;
					nr[8]=Ubic;    		
					nr[9]=tot;
					nr[10]=ds.Tables[0].Rows[n][6].ToString();
					dtSource.Rows.Add(nr);
					c++;
				}
			}
			if(c==0)
				return(false);
			return(true);
		}
	    
		//INSERTA FILA A LA TABLA DE DATOS--------------
        private bool InsertarFilaSDataSet(DataSet datosExcel, ref String errores)
		{  //Verdadero si logra insertar
            //pals[0] : datosExcel.Tables[0].Rows[0][0]
            DataSet dsDatos = new DataSet();
            DBFunctions.Request(dsDatos,IncludeSchema.NO, "select mite_codigo, mite_nombre, PUNI_CODIGO from mitems; SELECT mu.mite_codigo, PALM_ALMACEN, pubi_nombre FROM mubicacionitem mu, pubicacionitem pu where mu.pubi_codigo = pu.pubi_codigo; select puni_nombre, PUNI_CODIGO FROM PUNIDAD; SELECT plin_codigo, plin_tipo FROM plineaitem;");
            string INom = "";
            string Ubic = "";
            string puni = "";
            string ICod, Ped, linea;
            double cantF, cantI, Pre, IVA, Desc, tot;
            DataRow[] drLinea, drUbi;
            for (int i = 0; i < datosExcel.Tables[0].Rows.Count; i++)
            {
                INom = Ubic = puni = ICod = Ped = linea = "";
                cantF = cantI = Pre = IVA = Desc = tot = 0;
                try
                {
                    if (ddlTipoCargaExcel.SelectedValue == "D")
                        Ped = Convert.ToString(datosExcel.Tables[0].Rows[i][0]).Trim();
                    else
                        Ped = txtPrefijoExcel.Text.Trim();
                }
                catch { return (false); }
                try { ICod = Convert.ToString(datosExcel.Tables[0].Rows[i][1]).Trim(); }
                catch { return (false); }
                try { cantF = Convert.ToDouble(datosExcel.Tables[0].Rows[i][2].ToString().Replace(",",".")); }
                catch { return (false); }
                if (cantF <= 0) cantF = 1;
                try { cantI = Convert.ToDouble(datosExcel.Tables[0].Rows[i][3].ToString().Replace(",", ".")); }
                catch { return (false); }
                if (cantI <= 0) cantI = 1;
                try { Pre = Convert.ToDouble(datosExcel.Tables[0].Rows[i][4].ToString().Replace(",", ".")); }
                catch { return (false); }
                try { IVA = Convert.ToDouble(datosExcel.Tables[0].Rows[i][5].ToString().Replace(",", ".")); }
                catch { return (false); }
                try { Desc = Convert.ToDouble(datosExcel.Tables[0].Rows[i][6].ToString().Replace(",", ".")); }
                catch { return (false); }
                try { linea = Convert.ToString(datosExcel.Tables[0].Rows[i][7]).Trim(); }
                catch { return (false); }
                if (IVA > 100) IVA = 100; if (IVA < 0) IVA = 0;
                if (Desc > 100) Desc = 100; if (Desc < 0) Desc = 0;
                /* parece que esto revisa que no exista el mismo item dos veces
                    for (int n = 0; n < dtSource.Rows.Count; n++)
                        if (dtSource.Rows[n]["num_ped"].ToString() == Ped.ToString() && dtSource.Rows[n]["mite_codigo"].ToString() == ICod)
                        {
                            errores += "el item: " + ICod + "está repetido. Por favor revise su excel en la linea: " + (n + 1);
                            return (false);
                        }
                */
                DataRow nr = dtSource.NewRow();
                nr[0] = Ped; nr[4] = cantF; nr[3] = cantI; nr[5] = Pre; nr[6] = IVA; nr[7] = Desc; nr[11] = linea;

                drLinea = dsDatos.Tables[3].Select("PLIN_CODIGO = '" + linea + "'");
                DataRow [] tipoLinea = dsDatos.Tables[3].Select("PLIN_TIPO = '" + linea + "'");

                string codIRef = "";
                if (drLinea[0].ItemArray[1].ToString() == "MZ")
                { 
                    //Revision de cambio de referencia: visual - db // referencias TIPO MAZDA
                    if (ICod.Length >= 14 && ICod.Substring(11, 1) == " " && ICod.Substring(12, 1) != " ")
                    {
                        ICod = ICod.Replace(" ", "-");
                    }
                    else if (ICod.Length >= 14 && ICod.Substring(11, 1) != " " && ICod.Substring(12, 1) != " " && ICod.Substring(12, 1) != "-")
                    {
                        ICod = ICod.Insert(12, "-");
                    }
                    if (ICod.Length >= 11)
                        ICod = ICod.Replace("--", "-");
                }
                
                nr[1] = ICod;

                if (drLinea.Length > 0)
                    Referencias.Guardar(ICod, ref codIRef, drLinea[0].ItemArray[1].ToString());
                else
                    Referencias.Guardar(ICod, ref codIRef, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + linea + "'"));
                //validar que el item no exista en la tabla más de dos veces CON EL MISMO PEDIDO
                if (datosExcel.Tables[0].Select("ITEM = '" + codIRef + "' AND PEDIDO = '" + Ped + "'").Length > 1)
                {
                    errores += "El item: " + codIRef + " está repetido en el excel con el mismo pedido, revise por favor";
                }
                else
                {
                    try
                    {
                        INom = dsDatos.Tables[0].Select("mite_codigo='" + codIRef + "'")[0].ItemArray[1].ToString();
                    }
                    catch
                    {
                        string INom1 = DBFunctions.SingleData("select mite_nombre from mitems where mite_codigo='" + codIRef + "'");
                    }
                    nr[2] = INom;
                    drUbi = dsDatos.Tables[1].Select("mite_codigo='" + codIRef + "' and palm_almacen='" + ddlAlmacen.SelectedValue + "'");

                    if (drUbi.Length > 0)
                        Ubic = drUbi[0].ItemArray[2].ToString();
                    else
                        Ubic = "";
                    /*try
                        {
                            Ubic = dsDatos.Tables[1].Select("mite_codigo='" + codIRef + "' and palm_almacen='" + ddlAlmacen.SelectedValue + "'")[0].ItemArray[2].ToString();
                        }
                        catch
                        {
                            Ubic = DBFunctions.SingleData("SELECT pubi_nombre FROM mubicacionitem mu, pubicacionitem pu where mu.mite_codigo='" + codIRef + "' and mu.pubi_codigo= pu.pubi_codigo and palm_almacen='" + ddlAlmacen.SelectedValue + "' fetch first row only");
                        }
                    */
                    nr[8] = Ubic;
                    try
                    {
                        puni = dsDatos.Tables[2].Select("PUNI_CODIGO='" + dsDatos.Tables[0].Rows[i]["PUNI_CODIGO"] + "'")[0].ItemArray[0].ToString();
                    }
                    catch
                    {
                        puni = DBFunctions.SingleData("select pu.puni_nombre from mitems as mi,punidad as pu where mi.mite_codigo='" + codIRef + "' and pu.puni_codigo = mi.puni_codigo");
                    }
                    nr[10] = puni;
                    tot = Total(Pre, cantI, Desc, IVA);
                    nr[9] = tot;
                    if (INom != "")
                        dtSource.Rows.Add(nr);
                    else
                        errores += "*Item no se agregó: " + ICod + "  No Existe en la maestra. Revise Por Favor! <br />";
                    //Utils.MostrarAlerta(Response, "*Item + '" + ICod + "' +  No Existe en la maestra. \\nRevise Por Favor!");
                }
            }
			return(true);
		}

		private bool InsertarFila(string[]pals)
		{  //Verdadero si logra insertar
			string ICod,Ped;
			double cantF,cantI,Pre,IVA,Desc;
			double tot=0;
			try{Ped=Convert.ToString(pals[0]);}
			catch{return(false);}
			try{ICod=Convert.ToString(pals[1]);}
			catch{return(false);}
			try{cantF=Convert.ToDouble(pals[2]);}
			catch{return(false);}
			if(cantF<=0)cantF=1;
			try{cantI=Convert.ToDouble(pals[3]);}
			catch{return(false);}
			if(cantI<=0)cantI=1;
			try{Pre=Convert.ToDouble(pals[4]);}
			catch{return(false);}
			try{IVA=Convert.ToDouble(pals[5]);}
			catch{return(false);}
			try{Desc=Convert.ToDouble(pals[6]);}
			catch{return(false);}
			if(IVA>100)IVA=100;if(IVA<0)IVA=0;
			if(Desc>100)Desc=100;if(Desc<0)Desc=0;
			for(int n=0;n<dtSource.Rows.Count;n++)
				if(dtSource.Rows[n]["num_ped"].ToString()==Ped.ToString() && dtSource.Rows[n]["mite_codigo"].ToString()==ICod)
					return(false);
			DataRow nr=dtSource.NewRow();
			nr[0]=Ped;nr[1]=ICod;nr[4]=cantF;nr[3]=cantI;nr[5]=Pre;nr[6]=IVA;nr[7]=Desc;
			string INom=DBFunctions.SingleData("select mite_nombre from mitems where mite_codigo='"+ICod+"'");
			nr[2]=INom;
			string Ubic=DBFunctions.SingleData("SELECT mubi_ubicacion FROM mubicacionitem where mite_codigo='"+ICod+"' AND palm_almacen='"+ddlAlmacen.SelectedValue+"'");
			nr[8]=Ubic;
			string puni=DBFunctions.SingleData("select t3.puni_nombre from mitems as t2,punidad as t3 where t3.puni_codigo=t2.puni_codigo and t2.mite_codigo='"+ICod+"'");
			nr[10]=puni;
			tot=Total(Pre,cantI,Desc,IVA);
			nr[9]=tot;
			dtSource.Rows.Add(nr);
			return(true);
		}

		//REVISAR SI HAY pedido-item O prerecepcion PARA AGREGAR
		protected bool CheckValues(DataGridCommandEventArgs e)
		{
			bool check=true;
			if(ddlTProc.SelectedValue=="2")
			{
				if(((DropDownList)e.Item.Cells[0].Controls[1]).Items.Count==0) 
				{
                    Utils.MostrarAlerta(Response, "No se ha seleccionado un documento valido!");
					check=false;
				}
				if(((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text))
				{
                    Utils.MostrarAlerta(Response, "Existe algun valor invalido para la inserción.\\nRevise Por Favor!");
					check=false;
					
				}
			}
			else
			{
				if(((DropDownList)e.Item.Cells[0].Controls[1]).Items.Count == 0 && ((TextBox)e.Item.Cells[0].Controls[3]).Text.Length == 0)
				{
                    Utils.MostrarAlerta(Response, "Opción de documento invalida!");
					check=false;
				}
				if(ddlTProc.SelectedValue=="0")
				{
					if(((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text))
					{
                        Utils.MostrarAlerta(Response, "Existe algun valor invalido para la inserción.\\nRevise Por Favor!");
						check=false;
						
					}
				}
				else if(ddlTProc.SelectedValue=="1")
				{
					if(((TextBox)e.Item.Cells[1].Controls[1]).Text == "" || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[8].Controls[1]).Text) || !DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[9].Controls[1]).Text))
					{
                        Utils.MostrarAlerta(Response, "Existe algun valor invalido para la inserción.\\nRevise Por Favor!");
						check=false;
					}
				}
			}
			return check;
		}
		
		protected bool RevisarPreRecepciones(ArrayList preRececepciones, ref string strout,ref ArrayList sqlStrings)
		{
			bool status = false;
			for(int i=0;i<preRececepciones.Count;i++)
			{
				string[] sepPreRecep = preRececepciones[i].ToString().Split('-');
				//Vamos a determinar si aun existen items sin legalizar de esta prerecepcion
				DataSet da = new DataSet();
                if (sepPreRecep.Length == 3) // ver si este caso es cuando el prefijo tiene dentro un -
                { 
                     DBFunctions.Request(da, IncludeSchema.NO, "SELECT * FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND dite_numedocu=" + sepPreRecep[2] + " AND (dite_cantidad-dite_cantdevo)>0");
                     if (da.Tables[0].Rows.Count > 0)
                        sqlStrings.Add("UPDATE mprerecepcion SET mpre_legal='S' WHERE pdoc_codigo='" + sepPreRecep[0] + "-" + sepPreRecep[1] + "' AND mpre_numero=" + sepPreRecep[2] + "");
                }
                else if (sepPreRecep.Length == 2)
                {
                    DBFunctions.Request(da, IncludeSchema.NO, "SELECT * FROM ditems WHERE pdoc_codigo='" + sepPreRecep[0] + "' AND dite_numedocu=" + sepPreRecep[1] + " AND (dite_cantidad-dite_cantdevo)>0");

                    if (da.Tables[0].Rows.Count > 0)
                        sqlStrings.Add("UPDATE mprerecepcion SET mpre_legal='S' WHERE pdoc_codigo='" + sepPreRecep[0] + "' AND mpre_numero=" + sepPreRecep[1] + "");
                }
            }



			return status;
		}

		[Ajax.AjaxMethod]
		public string CargarNumero(string prefijo)
		{
			string valor="";
			valor=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
			return valor;
		}

        [Ajax.AjaxMethod()]
        public string ConsultarSustitucion(string itmO)
        {
            string susItm = "";
            susItm = DBFunctions.SingleData("SELECT msus_codsustit FROM MSUSTITUCION WHERE msus_codorigen='" + itmO + "'");
            if (susItm.Length > 0)
                return (susItm);
            else
                return (itmO);
        }

        [Ajax.AjaxMethod()]
        public string ConsultarNombreItem(string codigo, string linea)
        {
            string salida = "";
            string codI = "";
            Referencias.Guardar(codigo, ref codI, linea);
            salida = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='" + codI + "'");
            return salida;
        }


        [Ajax.AjaxMethod]
        public string ConsultarPrecioItem(string codigo, string linea, string almacen)
        {
            string valor = "";
            string codI = "";
            string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
            Referencias.Guardar(codigo, ref codI, linea);
            
            double precio = 0, 
                costoPromedio = 0;

            try { 
                precio = Convert.ToDouble(DBFunctions.SingleData("SELECT mpre_precio FROM MPRECIOITEM WHERE mite_codigo='" + codI + "'")); 
                
                costoPromedio = Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_COSTPROM FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='" + 
                codI + "' AND PALM_ALMACEN='" + almacen + "' AND PANO_ANO=" + ano_cinv)); 
            }
            catch { };

            if (precio == 0)
                precio = costoPromedio;

            valor = (Math.Round(precio, 2)).ToString();

            return valor;
        }

        [Ajax.AjaxMethod]
        public string ConsultarIvaItem(string codigo, string linea, string nit)
        {
            string codI = "";
           Referencias.Guardar(codigo, ref codI, linea);

            double iva = 0;

            try
            {
                string regIVA = DBFunctions.SingleData("select treg_regiiva from mnit where mnit_nit ='" + nit + "';");
                if (regIVA != "S" && regIVA != "N")
                    iva = Convert.ToDouble(DBFunctions.SingleData("select PIVA_PORCIVA from mitems WHERE mite_codigo='" + codI + "'"));
            }
            catch { };

            return iva.ToString();
        }

		#endregion
		
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{

		}
		#endregion


        public string CargaNITPRO { get; set; }
     }
}
