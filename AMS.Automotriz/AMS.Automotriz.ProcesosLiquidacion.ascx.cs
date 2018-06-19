// created on 16/11/2004 at 15:32
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.IO;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.Inventarios;
using AMS.DBManager;
using AMS.Tools;
using Ajax.JSON;
using Ajax;
using AMS.Contabilidad;


namespace AMS.Automotriz
{
	public partial class ProcesosLiquidacion : System.Web.UI.UserControl
	{
		#region Atributos

        protected int numDecimales = 0;
        protected bool aplicaRetencionAutomatica = true;
        protected string prefijoOT;
        protected string numOT;
        protected string cargoPrinOT;
        protected string almaOT;
        protected string actividad = "";
        protected string liqGarantiaFabrica = "";
        protected string ivaOperInter = "", cargoCortesia = "";
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		public bool varLiquidacionProcesosDOS = false;

        protected DataTable dtItems;
        public DataTable tablaRtns;
		private DatasToControls bind = new DatasToControls();
		protected TextBox tbPassRecep;
		protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();
        protected ProceHecEco contaOnline = new ProceHecEco();
        bool facturaenCero = true; // definir parametro en ctaller para SI o NO emitir facturas en CERO


        #endregion

        #region Eventos
        protected void Page_Load(object sender, System.EventArgs e)
		{
            //AMS.Automotriz.
            varLiquidacionProcesosDOS = false;
            //varLiquidacionProcesosDOS = Convert.ToBoolean(ConfigurationManager.AppSettings["LiquidacionProcesosDOS"] == "True");
            Ajax.Utility.RegisterTypeForAjax(typeof(ProcesosLiquidacion));//cambio 1 febrero
			btnFctCrg.Attributes.Add("onclick","if(confirm('Esta seguro que desea Realizar este Proceso?')){document.getElementById('"+btnFctCrg.ClientID+"').disabled = true;"+this.Page.GetPostBackEventReference(btnFctCrg)+";}");
			prefijoOT   = Request.QueryString["pref"];
			numOT       = Request.QueryString["num"];
			cargoPrinOT = DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numOT+"");
			almaOT      = DBFunctions.SingleData("SELECT palm_almacen FROM morden WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numOT+"");
            try { liqGarantiaFabrica = DBFunctions.SingleData("SELECT COALESCE(CTAL_IVALIQUGARA,'S') FROM ctaller"); }
            catch { liqGarantiaFabrica = "S"; };

            try { facturaenCero = DBFunctions.RecordExist ("SELECT ctaL_factdeDucero FROM ctaller where ctaL_factdeDucero = 'S'"); }
            catch { facturaenCero = true; };

            try { ivaOperInter = DBFunctions.SingleData("SELECT COALESCE(ctal_ivaopersinter,'N') FROM ctaller"); }
            catch { ivaOperInter = "N"; };

            string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); //ConfigurationManager.AppSettings["ManejoCentavos"];
            if (Utils.EsEntero(centavos))
                numDecimales = Convert.ToInt32(centavos);

            bool usarDecimales = false;
            try
            {
                usarDecimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);
            }
            catch { }
            string numDec = "0";
            try
            {
                numDec = ConfigurationManager.AppSettings["tamanoDecimal"];
            }
            catch
            { }

            if (usarDecimales)
            {
                if (numDec != null && numDec != "")
                {
                    numDecimales = Convert.ToInt16(numDec);
                }
                else
                {
                    numDecimales = 2;
                }
            }

            if (!IsPostBack)
            {
                lbPrefOT.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='" + prefijoOT + "'");
                lbNumOT.Text = numOT;
                lbNitCliente.Text = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                lbNomCliente.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + lbNitCliente.Text + "'");

                String contacto = DBFunctions.SingleData("SELECT MORD_CONTACTO FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                lbNomContacto.Text = (contacto != null) ? contacto : "";

                lbCrgPrnOT.Text = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo=(SELECT tcar_carGO FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + ")");
                if (DBFunctions.RecordExist("SELECT TNAC_TIPONACI FROM MNIT WHERE MNIT_NIT='" + lbNitCliente.Text + "' AND TNAC_TIPONACI='E';"))
                    Utils.MostrarAlerta(Response, "El NIT es extranjero y no le va a liquidar IVA, es su responsabilidad!");
                CargarCargosRelacionados(ddlCrgsRels);
                if (ddlCrgsRels.Items.Count == 0)
                {
                    plInfoFC.Visible = false;
                    plInfoFCAseg.Visible = false;
                    plhInfoFactAsegu.Visible = false;
                    btnLiqCrg.Enabled = false;
                    Utils.MostrarAlerta(Response, "No se han encontrado Cargos para esta orden, por favor Revise");
                    
                }
                else
                {
                    plInfoFC.Visible = plInfoFCAseg.Visible = plhInfoFactAsegu.Visible = false;
                    fechaFactura.SelectedDate = DateTime.Now;

                    bind.PutDatasIntoDropDownList(ddlRecep, "SELECT pv.pven_codigo, pv.pven_nombre FROM pvendedor pv INNER JOIN pvendedoralmacen pva ON pv.pven_codigo=pva.pven_codigo WHERE pva.palm_almacen='" + almaOT + "' AND pv.tvend_codigo='RT'");
                    DatasToControls.EstablecerDefectoDropDownList(ddlRecep, DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo=(SELECT pven_codigo FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + ")"));

                    bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM pvendedor WHERE tvend_codigo IN ('MC','RT','TT','VM','VV') AND pven_vigencia='V' ORDER BY pven_nombre ASC");
                    ddlVendedor.Items.Add(new ListItem("No Aplica..."));

                    tbDiasVig.Text = DBFunctions.SingleData("SELECT COALESCE(MCLI_DIASPLAZ,0) FROM MCLIENTE WHERE MNIT_NIT='" + lbNitCliente.Text + "'");
                    if (tbDiasVig.Text == "")
                    {
                        tbDiasVig.Text = "0";
                    }

                    if (Request.QueryString["factOT"] != null)
                    {
                        string msg = "Se ha creado la factura con prefijo " + Request.QueryString["prefF"] + " y numero " + Request.QueryString["numF"] + " por el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + Request.QueryString["car"] + "'").Trim() + ".');";
                        string msg1 = "Se ha creado la factura con prefijo " + Request.QueryString["prefF1"] + " y numero " + Request.QueryString["numF1"] + " por el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + Request.QueryString["car1"] + "'").Trim() + ".\\nSe ha creado la factura con prefijo " + Request.QueryString["prefF2"] + " y numero " + Request.QueryString["numF2"] + " por el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + Request.QueryString["car2"] + "'").Trim() + ");";
                        if (Request.QueryString["factOT"] == "0")
                            Utils.MostrarAlerta(Response, msg);
                        
                        else if (Request.QueryString["factOT"] == "1")
                                Utils.MostrarAlerta(Response, msg1);
                    }
                    if (IVA.NacionalidadNit(lbNitCliente.Text) == "E")
                    {
                        btnFctCrg.Attributes.Add("onClick", "return confirm('Al cliente " + lbNitCliente.Text + " - " + lbNomCliente.Text + " no se le facturará IVA por ser extranjero. Desea continuar? Es bajo su responsabilidad.');");
                        //tbDescuentoOperaciones.Attributes.Add("onKeyUp", "ReCalcularTotalIVAOperaciones(this,'" + IVA.NacionalidadNit(lbNitCliente.Text) + "');");
                        //tbDescuentoRespuestos.Attributes.Add("onKeyUp", "ReCalcularTotalIVARepuestos(this,'" + IVA.NacionalidadNit(lbNitCliente.Text) + "');");
                    }
                    //else
                    //{
                    //    tbDescuentoOperaciones.Attributes.Add("onKeyUp", "ReCalcularTotalIVAOperaciones(this,'" + IVA.NacionalidadNit(lbNitCliente.Text) + "');");
                    //    tbDescuentoRespuestos.Attributes.Add("onKeyUp", "ReCalcularTotalIVARepuestos(this,'" + IVA.NacionalidadNit(lbNitCliente.Text) + "');");
                    //}

                    string causaRet = DBFunctions.SingleData("select CCON_CAUSARETENOT from ccontabilidad");

                    if (causaRet != null && causaRet == "S")
                        plhRetenciones.Visible = true;
                    else
                        plhRetenciones.Visible = false;

                }
                SeleccionarPrefijoCargo();

                Cargar_Tabla_Rtns();
                ViewState["TABLERETS"] = tablaRtns;
                BindRetenciones();
            }

            if (ViewState["TABLERETS"] != null)
                tablaRtns = (DataTable)ViewState["TABLERETS"];
        }

        protected void SeleccionarPrefijoCargo()
		{
            string Sql = "";
            string tipotem = ddlCrgsRels.SelectedValue.ToString();
          
            switch (tipotem)
            {
                case "S":
                    actividad = "TC";
                    break;
                case "C":
                    actividad = "TC";
                    break;
                case "G":
                    actividad = "TG";
                    break;
                case "I":
                    actividad = "TI";
                    break;
                case "A":
                    actividad = "TA";
                    break;
                case "T":
                    actividad = "TT";
                    break;
            }

            Sql = "SELECT p.pdoc_codigo, p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FC' and PH.tpro_proceso = '" + actividad + "' AND P.PDOC_CODIGO = PH.PDOC_CODIGO and ph.palm_almacen = '" + almaOT + "' ";

            bind.PutDatasIntoDropDownList(ddlPrefFact, Sql);
            if (ddlPrefFact.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se ha definido un documento del tipo: " + tipotem + " del proceso: " + actividad + " para el cargo de taller " + ddlCrgsRels.SelectedValue.ToString() + ", por favor Revise");
                plInfoFC.Visible = plInfoFCAseg.Visible = plhInfoFactAsegu.Visible = false;
                btnFctCrg.Enabled = false;
            }
            else
            {
                tbNumFact.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + ddlPrefFact.SelectedValue + "'");
            }
        }

        protected void LiquidarCargo(Object Sender, EventArgs e)
        {
            string msg2 = "";
            int cargoFacturado = 0;
            if (ddlCrgsRels.SelectedValue.ToString() != "S")
            {
                if (DBFunctions.RecordExist("SELECT * FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo='" + ddlCrgsRels.SelectedValue + "'"))
                    msg2 += "La factura por el cargo " + DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + ddlCrgsRels.SelectedValue + "'").Trim() + " de la orden de trabajo " + lbPrefOT.Text + "-" + numOT + ", ya ha sido realizada.\\nFactura : " + DBFunctions.SingleData("SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mfac_numedocu AS CHARACTER(15)) FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo='" + ddlCrgsRels.SelectedValue + "'").Trim() + "";
            }
            else if (ddlCrgsRels.Items.Count > 1)
            {
                msg2 += "Debe facturar primero los demás cargos y de último el cargo Seguros.";
            }
            else
            {
                cargoFacturado = Convert.ToInt16(DBFunctions.SingleData(@"SELECT COUNT(*) FROM MFACTURACLIENTETALLER MFT, DORDENSEGUROS DS, mfacturacliente mF, MORDEN MO 
                                                                        WHERE MFT.PDOC_PREFORDETRAB = '" + prefijoOT + @"' AND MFT.MORD_NUMEORDE = " + numOT + @"
                                                                         AND MFT.PDOC_PREFORDETRAB =  DS.PDOC_CODIGO  AND MFT.MORD_NUMEORDE = DS.MORD_NUMEORDE
                                                                         AND MFT.PDOC_PREFORDETRAB =  MO.PDOC_CODIGO  AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
                                                                         AND MFT.PDOC_CODIGO = MF.PDOC_CODIGO AND MFT.MFAC_NUMEDOCU = MF.MFAC_nUMEDOCU AND MFT.TCAR_CARGO = 'S' ;"));
                if (cargoFacturado == 1)
                {
                    //      msg2 += "La factura por el cargo SEGUROS de la orden de trabajo " + lbPrefOT.Text + "-" + numOT + ", ya ha sido realizada.\\nFactura : " + DBFunctions.SingleData("SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mfac_numedocu AS CHARACTER(15)) FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo='" + ddlCrgsRels.SelectedValue + "'").Trim() + "";
                    string msgSeg = "Ya se ha emitido una factura por el cargo SEGUROS de la orden de trabajo " + lbPrefOT.Text + "-" + numOT + ", ya ha sido realizada.\\nFactura : " + DBFunctions.SingleData("SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mfac_numedocu AS CHARACTER(15)) FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo='" + ddlCrgsRels.SelectedValue + "'").Trim() + "";
                    Utils.MostrarAlerta(Response, msgSeg);
                }
                else
                {
                     if (cargoFacturado == 2)
                        msg2 += "La factura por el cargo SEGUROS de la orden de trabajo " + lbPrefOT.Text + "-" + numOT + ", ya ha sido realizada.\\nFactura : " + DBFunctions.SingleData("SELECT pdoc_codigo CONCAT '-' CONCAT CAST(mfac_numedocu AS CHARACTER(15)) FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo='" + ddlCrgsRels.SelectedValue + "'").Trim() + "";
                }
            }

            if (msg2 != "")
            {
                Utils.MostrarAlerta(Response, msg2);
            //    Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
            //    return;
            }
            else
            { 
                //Si no existe la factura pasamos al siguiente paso y la creamos
                plInfoOT.Visible = false;
                plInfoFC.Visible = true;
                SeleccionarPrefijoCargo();
                //Revisamos el cargo que se va a facturar
                string cargoFacturar = ddlCrgsRels.SelectedValue;
                lbCrgLiqu.Text = DBFunctions.SingleData("SELECT tcar_nombre FROM tcargoorden WHERE tcar_cargo='" + cargoFacturar + "'");
                //Si el cargo que vamos a facturar es el del cliente o alistamiento simplemente colocamos el nit con que se creo la orden de trabajo
                if (cargoFacturar == "C" || cargoFacturar == "A")
                {
                    tbNitCli.Text = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                    tbNitClia.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + tbNitCli.Text + "'");
                }
                //Ahora revisamos si el cargo es Garantia de Fabrica debemos traer el nit de la fabrica que se encuentra en la tabla dordengarantia
                else if (cargoFacturar == "G")
                {
                    if (DBFunctions.SingleData("SELECT mnit_nitfabrica FROM dordengarantia WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "") != "")
                    {
                        //Si existe el registro de una fabrica que responda por la garantia cargamos esa informacion
                        tbNitCli.Text = DBFunctions.SingleData("SELECT mnit_nitfabrica FROM dordengarantia WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                        tbNitClia.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + tbNitCli.Text + "'");
                    }
                    else//Si no existe un registro de permitimos que escojan un nit de la tabla mproveedor
                        tbNitCli.Attributes.Add("onclick", "ModalDialog(this, 'Select t1.mnit_nit as NIT, t1.mnit_apellidos concat \\' \\' concat t1.mnit_nombres as Nombre from MNIT as t1,PCASAMATRIZ as t2 where t1.mnit_nit=t2.mnit_nit', new Array());");
                }
                //Ahora si el cargo es interno o garantia de taller debemos cargar el nit de la empresa
                else if (cargoFacturar == "I" || cargoFacturar == "T")
                {
                    tbNitCli.Text = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
                    tbNitClia.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + tbNitCli.Text + "'");
                }
                //Por ultimo si el cargo es seguros cargamos el nit que tenemos en la tabla de dordenseguros
                else if (cargoFacturar == "S")
                {
                    //Cambio Revision del dia limite de facturacion relacionado con la tabla PASEGURADORA
                    string textoAdicional = "";
                    int diaLimite = 25;
                    string retorno = DBFunctions.SingleData("SELECT COALESCE(PASE.pase_diacorte,-1) FROM paseguradora PASE RIGHT JOIN dordenseguros DOS ON PASE.mnit_nit = DOS.mnit_nitseguros WHERE DOS.pdoc_codigo = '" + prefijoOT + "' AND DOS.mord_numeorde = " + numOT);
                    if (retorno != "")
                    {
                        diaLimite = Convert.ToInt32(retorno);
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "No se ha definido el día máximo de facturación en la aseguradora de esta Orden de Trabajo.");
                    }
                    int diaActual = DateTime.Now.Day;
                    if (diaLimite == -1)
                        textoAdicional = "ATENCIÓN : La aseguradora relacionada no tiene configurado un día de facturación máximo.\\n ";
                    else if (diaActual < diaLimite)
                            textoAdicional = "";
                    else if (diaActual >= diaLimite)
                            textoAdicional = "ATENCIÓN : La fecha de facturación está por fuera del rango designado por la aseguradora.\\n";
                    //FIN Cambio Revision del dia limite de facturacion relacionado con la tabla PASEGURADORA

                    plInfoFCAseg.Visible = true;
                    tbNitCli.Text  = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                    tbNitClia.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='" + tbNitCli.Text + "'");
                    //Ahora debemos cargar cuatro variables : porcentaje deducible minimo , valor deducible minimo, valor deducible suministros y el nit de la aseguradora
                    try { tbPorDedCli.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_porcdeducible FROM dordenseguros WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "")).ToString("N"); }
                    catch { tbPorDedCli.Text = "0.00"; }
                    try { tbDedOT.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_deduminimo FROM dordenseguros WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "")).ToString("N"); }
                    catch { tbDedOT.Text = "0.00"; }
                    try { tbDedSum.Text = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_dedusumase FROM dordenseguros WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "")).ToString("N"); }
                    catch { tbDedSum.Text = "0.00"; }
                    tbNitAseg.Text = DBFunctions.SingleData("SELECT mnit_nitseguros FROM dordenseguros WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                    if (((tbDedOT.Text == "0.00" && tbPorDedCli.Text == "0.00" && tbDedSum.Text == "0.00") && !facturaenCero) || (cargoFacturado == 1 && cargoFacturar == "S"))
                        if (cargoFacturado == 1 && cargoFacturar == "S")
                            textoAdicional += "Se Generará solo una Factura para la aseguradora porque YA SE EMITIO la FACTURA del cliente ";
                        else
                            textoAdicional += "Se Generará solo una Factura para la aseguradora porque el cliente NO tiene DEDUCIBLE";
                    else
                        textoAdicional += "Se generaran dos facturas:\\n1. Cargo de la Aseguradora (si está dentro del día de corte).\\n2. Cargo del Cliente. \\n";

                    Utils.MostrarAlerta(Response, textoAdicional);

                }
                //Ahora vamos a cargar las operaciones e items relacionadas con este cargo
                CargarOperaciones(cargoFacturar);
                CargarItems(cargoFacturar);
                CambioDescuento(Sender, e);
                tbTotal.Text = (Convert.ToDouble(tbValNetOper.Text.Substring(1, tbValNetOper.Text.Length - 1)) + Convert.ToDouble(tbValIVAOper.Text.Substring(1, tbValIVAOper.Text.Length - 1)) + Convert.ToDouble(tbValNetItem.Text.Substring(1, tbValNetItem.Text.Length - 1)) + Convert.ToDouble(tbValIVAItem.Text.Substring(1, tbValIVAItem.Text.Length - 1))).ToString("C");

                if (cargoFacturar == "S") calculoValorADeducirLbl(Sender, e);
            }
        }

        protected void calculoValorADeducirLbl(Object  Sender, EventArgs e)
        {
            //para el label que muestra el deducible
            double porcentajeDeducible = Convert.ToDouble(tbPorDedCli.Text.Trim());
            double valorMinimoDeducible = Convert.ToDouble(tbDedOT.Text.Trim());
            double valTotalFact = Convert.ToDouble(tbTotalDescuento.Text.Substring(1));
            
            double valADeducir =
                ((valTotalFact * (porcentajeDeducible / 100)) < valorMinimoDeducible) ? 
                                    valorMinimoDeducible : Math.Round(valTotalFact * (porcentajeDeducible / 100));

            lbDeduccion.Text = "El valor DEDUCIBLE asegurado (IVA incluido) es de " + valADeducir.ToString("C");
        }
		
		protected void Volver(Object  Sender, EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden");
		}

		public bool ValidarDiasPlazo()
		{
			bool valido = false;
            if (DatasToControls.ValidarInt(tbDiasVig.Text) && DatasToControls.ValidarInt(tbNitCli.Text))
			{
				valido = true;
			}
			return valido;
		}
       public bool validarvalordeducible()
        {
            bool valido = false;

            double Totalorden = Convert.ToDouble(tbTotal.Text.Substring(1));
            double totaldeducible = 0;
            try
            {
                totaldeducible = Convert.ToDouble(tbDedOT.Text);
            }
            catch
            {
                totaldeducible = 0;
            }
             

            if (Totalorden>=totaldeducible)
            {
                valido = true;
            }
            //else  // hay poner una pregunta para cuando el deducibles es mayor al valro de los trabajos
            //{
            //    if (System.Windows.Forms.MessageBox.Show
            //    ("el valor del Deducible es Mayor al total del servicio, está seguro ?", "Add",
            //    System.Windows.Forms.MessageBoxButtons.YesNo,
            //    System.Windows.Forms.MessageBoxIcon.Question)
            //    == System.Windows.Forms.DialogResult.Yes)
            //        valido = true;
            //    else
            //        valido = false;
            //}

            return valido;
        }

        public bool ValidarRangoAutorizado()
        {
            bool  valido = true;
            Int32 numInicial = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(pdoc_numeinic,0) FROM pdocumento WHERE pdoc_codigo='" + ddlPrefFact.SelectedValue + "'"));
            Int32 numFinal   = Convert.ToInt32(DBFunctions.SingleData("SELECT COALESCE(pdoc_numefina,0) FROM pdocumento WHERE pdoc_codigo='" + ddlPrefFact.SelectedValue + "'"));
            Int32 numActual  = Convert.ToInt32(tbNumFact.Text);
            if (numActual < numInicial || numActual > numFinal)
            {
                valido = false;
            }
            return valido;
        }

       
		protected void FacturarCargo(Object  Sender, EventArgs e)
		{
            Int16 siSeguros = 1;
            string estadoOrden = DBFunctions.SingleData("SELECT test_estado FROM morden WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numOT+"");
            string recepcionistaOrden = DBFunctions.SingleData("SELECT PVEN_CODIGO FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
            if (tbObsr.Text.Trim() == String.Empty)
				tbObsr.Text = DBFunctions.SingleData("SELECT mnit_nit FROM morden WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numOT+"")+ " - "+DBFunctions.SingleData("SELECT MCA.mcat_placa FROM mcatalogovehiculo MCA INNER JOIN morden MOR ON MCA.pcat_codigo = MOR.pcat_codigo AND MCA.mcat_vin = MOR.mcat_vin WHERE MOR.pdoc_codigo='"+prefijoOT+"' AND MOR.mord_numeorde="+numOT+"");

            CambioDescuento(Sender, e);
                       
            if (ValidarRangoAutorizado())
            {
             if(ValidarDiasPlazo())
			 {
                    if (validarvalordeducible())
                    {

                        string observacion = prefijoOT + " - " + numOT + " - " + recepcionistaOrden;
                        if (estadoOrden == "A")
                        {
                            int i = 0;
                            //Ahora revisamos si el cargo que se esta facturando es diferente de aseguradora (S) creamos una factura
                            string cargoFacturar = ddlCrgsRels.SelectedValue;
                            string tipoCargo = cargoFacturar;
                            if (!cargoFacturar.Equals("I") && !cargoFacturar.Equals("A") && !cargoFacturar.Equals("T"))
                                tipoCargo = "F";
                            else
                                tipoCargo = "I";

                            //  Las empresas tipo CASA MATRIZ se trata las Garantias de Fabrica como tipo Interno
                            string tipoEmpresa = "";
                            if (cargoFacturar.Equals("G"))
                            {
                                tipoEmpresa = DBFunctions.SingleData("SELECT CEMP_INDICASAMATR FROM cempresa ");
                                if (tipoEmpresa.Equals("S"))
                                    tipoCargo = "I";
                            }

                            //Regularizar operacion para ajuste decimal x 4
                            if (ViewState["valorOpDecimal"] != null && ViewState["valorOpDecimal"].ToString() != "")
                            {
                                DBFunctions.NonQuery("UPDATE DORDENOPERACION SET DORD_VALOOPER = " + ViewState["valorOpDecimal"] + " WHERE PDOC_CODIGO = '" + prefijoOT + "' AND   MORD_NUMEORDE = " + numOT + " AND PTEM_OPERACION = '" + ViewState["operacionDec"] + "';");
                                //Utils.MostrarAlerta(Response, "Se ajustó el valor decimal de la operación: " + ViewState["operacionDec"] + " en: " + ViewState["valorOpDecimal"]);
                            }

                            if (cargoFacturar != "S")
                            {
                                //Creamos los sqls relacionados con la factura que se va a crear
                                ArrayList sqlRelsFact = new ArrayList();
                                double valorDescuentoOperaciones = 0, valorDescuentoRepuestos = 0, valorDescuentos = 0;
                                double valFact = 0, valIva = 0;

                                if (tbDescuentoOperaciones.Text != String.Empty)
                                    valorDescuentoOperaciones = Convert.ToDouble(tbDescuentoOperaciones.Text);
                                if (tbDescuentoRespuestos.Text != String.Empty)
                                    valorDescuentoRepuestos = Convert.ToDouble(tbDescuentoRespuestos.Text);

                                Pedidos.PedidoServicioTaller(prefijoOT, Convert.ToUInt32(numOT), cargoFacturar, tbObsr.Text, 0, 0, 0, 0, ref sqlRelsFact, 1, valorDescuentoRepuestos, valorDescuentoOperaciones, tbDescuentoAutoriza.Text.Trim());

                                valorDescuentos = valorDescuentoOperaciones + valorDescuentoRepuestos;

                                //Ahora creamos la factura de cliente
                                valFact = Convert.ToDouble(tbValNetOperDescuento.Text.Substring(1)) + Convert.ToDouble(tbValNetItemDescuento.Text.Substring(1));
                                if (tipoCargo == "I" && ivaOperInter == "N")
                                    valIva = 0;
                                else
                                    valIva = Convert.ToDouble(tbValIVAOperDescuento.Text.Substring(1)) + Convert.ToDouble(tbValIVAItemDescuento.Text.Substring(1));

                                FacturaCliente facturaTaller = new FacturaCliente("FOTT", ddlPrefFact.SelectedValue, tbNitCli.Text.Trim(),
                                    almaOT, tipoCargo, Convert.ToUInt32(tbNumFact.Text.Trim()), Convert.ToUInt32(tbDiasVig.Text.Trim()),
                                    fechaFactura.SelectedDate, fechaFactura.SelectedDate.AddDays(Convert.ToInt32(tbDiasVig.Text.Trim())),
                                    Convert.ToDateTime(null), valFact, valIva, 0, 0, 0, 0,
                                    DBFunctions.SingleData("SELECT pcen_centtal FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + almaOT + "'"),
                                    observacion, ddlRecep.SelectedValue, HttpContext.Current.User.Identity.Name.ToLower(), 1);
                                facturaTaller.SqlRels.Add(
                                    "UPDATE MORDEN set MORD_TP='" + rdbExplica.SelectedValue + "' " +
                                    "where pdoc_codigo='" + prefijoOT + "' and mord_numeorde=" + numOT + ";");

                                //Ahora al objeto de factura agregamos los sql asociados
                                for (i = 0; i < sqlRelsFact.Count; i++)
                                    facturaTaller.SqlRels.Add(sqlRelsFact[i].ToString());

                                if (facturaTaller.GrabarFacturaCliente(true))
                                {
                                    Retencion retencion = new Retencion(facturaTaller.NitCliente,
                                        facturaTaller.PrefijoFactura,
                                        (int)facturaTaller.NumeroFactura, repuestosParaRetencion(),
                                        Convert.ToDouble(tbValNetOperDescuento.Text.Substring(1)),
                                        Convert.ToDouble(tbValIVAOperDescuento.Text.Substring(1)),
                                        facturaTaller.ValorFactura + facturaTaller.ValorFletes,
                                        facturaTaller.ValorIva + facturaTaller.ValorIvaFletes, "S", true);

                                    GuardarRetenciones(ddlPrefFact.SelectedValue, tbNumFact.Text);

                                    if (cargoFacturar == "G")
                                    {
                                        ArrayList sql = new ArrayList();
                                        sql.Add("insert into DORDENGARANTIA values ('" + prefijoOT + "'," + numOT + ",'" + tbNitCli.Text.Trim() + "','" + facturaTaller.IndicativoFactura + " " + facturaTaller.NumeroFactura + "')");

                                        if (!DBFunctions.Transaction(sql))
                                            Utils.MostrarAlerta(Response, "No se pudo insertar en Orden Garantia");
                                    }
                                    // ESTO ES CARGO CLIENTE

                                    if (!aplicaRetencionAutomatica || retencion.Guardar_Retenciones(aplicaRetencionAutomatica))  // AQUI CALCULA LAS RETENCIONES
                                    { // ahora guardamos las retenciones CAUSADAS
                           //             Orden.AlmacenarCostosFacturaOT(facturaTaller.PrefijoFactura, facturaTaller.NumeroFactura);

                                        contaOnline.contabilizarOnline(facturaTaller.PrefijoFactura, Convert.ToInt32(facturaTaller.NumeroFactura.ToString()), DateTime.Now, "");

                                        if (RevisionEstadoOT())
                                            Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT=" + facturaTaller.PrefijoFactura + "&numOT=" + facturaTaller.NumeroFactura + "&cargo=" + cargoFacturar + "&dest=0&ImpFac=S");
                                        else
                                            Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT=" + facturaTaller.PrefijoFactura + "&numOT=" + facturaTaller.NumeroFactura + "&cargo=" + cargoFacturar + "&dest=1&ImpFact=S");
                                    }
                                    else
                                        lb.Text += "<br>Error :" + retencion.Mensajes;
                                }
                                else
                                    lb.Text += "<br>Error :" + facturaTaller.ProcessMsg;
                            }
                            else  //cargo de seguros
                            {
                                bool cierraOrden = true;
                                ArrayList sqlFactAse = new ArrayList();
                                //Como el cargo es de aseguradora debemos generar dos facturas una de la aseguradora y otra del cliente
                                double porcentajeDeducible = Convert.ToDouble(tbPorDedCli.Text.Trim());
                                double valorMinimoDeducible = Convert.ToDouble(tbDedOT.Text.Trim());
                                double valorDeducibleSumin = Convert.ToDouble(tbDedSum.Text.Trim());
                                double valCliente = 0, valIvaCliente = 0;
                                double valAsegura = 0, valIvaAsegura = 0;
                                double valTotalFact = Convert.ToDouble(tbTotalDescuento.Text.Substring(1));
                                double valNetoFact = Convert.ToDouble(tbValNetOperDescuento.Text.Substring(1)) + Convert.ToDouble(tbValNetItemDescuento.Text.Substring(1));
                                double valIvaFact = (Convert.ToDouble(tbValIVAOperDescuento.Text.Substring(1)) + Convert.ToDouble(tbValIVAItemDescuento.Text.Substring(1)));
                                if ((valTotalFact * (porcentajeDeducible / 100)) < valorMinimoDeducible)
                                {
                                    valCliente = Math.Round(valorMinimoDeducible / (1 + (valIvaFact / valNetoFact)));
                                    //En este caso debemos determinar que porcentaje del total es el deducible minimo para determinar el porcentaje del iva
                                    double porcentMinDedu = (valorMinimoDeducible * 100) / valTotalFact;
                                    valIvaCliente = Math.Round(valIvaFact * (porcentMinDedu / 100));
                                }
                                else
                                {
                                    valCliente = Math.Round(valNetoFact * (porcentajeDeducible / 100));
                                    valIvaCliente = Math.Round(valIvaFact * (porcentajeDeducible / 100));
                                }

                                if (valNetoFact - valCliente < 0)
                                {
                                    valAsegura = valNetoFact - Math.Round(valorMinimoDeducible / (1 + (valIvaFact / valNetoFact)));
                                    double porcentMinDeduc = (valorMinimoDeducible * 100) / valTotalFact;
                                    valIvaAsegura = valIvaFact - Math.Round(valIvaFact * (porcentMinDeduc / 100));
                                }
                                else
                                {
                                    valAsegura = valNetoFact - valCliente;
                                    valIvaAsegura = valIvaFact - valIvaCliente;
                                }

                                if (double.IsNaN(valCliente))
                                    valCliente = 0;
                                if (double.IsNaN(valIvaCliente))
                                    valIvaCliente = 0;
                                if (double.IsNaN(valAsegura))
                                    valAsegura = 0;
                                if (double.IsNaN(valIvaAsegura))
                                    valIvaAsegura = 0;
                                //Ahora vamos con la creación de los sqlRelsFact
                                double valorDescuentoOperaciones = 0, valorDescuentoRepuestos = 0, valorDescuentos = 0, porcentajeDescIva = 0;

                                if (tbDescuentoOperaciones.Text != String.Empty)
                                    valorDescuentoOperaciones = Convert.ToDouble(tbDescuentoOperaciones.Text);
                                if (tbDescuentoRespuestos.Text != String.Empty)
                                    valorDescuentoRepuestos = Convert.ToDouble(tbDescuentoRespuestos.Text);
                                /*
                                valorDescuentos = valorDescuentoRepuestos + valorDescuentoOperaciones;
                                if (valAsegura > 0)
                                {
                                    porcentajeDescIva = valIvaAsegura / valAsegura;
                                    valAsegura -= valorDescuentos;
                                    valIvaAsegura = valAsegura * porcentajeDescIva;
                                }
                                */

                                ArrayList sqlRelsFact = new ArrayList();
                                // SE DEBE CALCULAR EL FACTOR PROPORCIONAL de la aseguradora y del asegurado y guardarlo en MFACTURACLIENTETALLER
                                // DEBE PERMITIR EN EL CASO DE ASEGURADORAS ELIMINAR SOLO LA FACTURA de la ASEGURADORA y volverla a facturar solo esa
                                double factorDeducible = (valAsegura / (valAsegura + valCliente)); // aqui calcula el factor de la aseguradora
                                Pedidos.PedidoServicioTaller(prefijoOT, Convert.ToUInt32(numOT), cargoFacturar, tbObsr.Text, valorMinimoDeducible, valCliente, valorDeducibleSumin, porcentajeDeducible, ref sqlRelsFact, 1, valorDescuentoRepuestos, valorDescuentoOperaciones, tbDescuentoAutoriza.Text.Trim());
                                string[] archivos = new string[2];

                                //Primero creamos la factura de la aseguradora y le asociamos los sqlRelsFact - VERIFICAMOS que el cargo NO ESTE FACTURADO
                                if (!DBFunctions.RecordExist(@"SELECT * FROM MFACTURACLIENTETALLER MFT, DORDENSEGUROS DS, mfacturacliente mF, MORDEN MO 
                                WHERE MFT.PDOC_PREFORDETRAB = '" + prefijoOT + @"' AND MFT.MORD_NUMEORDE = " + numOT + @" AND MFT.TCAR_CARGO = 'S' 
                                 AND MFT.PDOC_PREFORDETRAB =  DS.PDOC_CODIGO  AND MFT.MORD_NUMEORDE = DS.MORD_NUMEORDE
                                 AND MFT.PDOC_PREFORDETRAB =  MO.PDOC_CODIGO  AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
                                 AND MFT.PDOC_CODIGO = MF.PDOC_CODIGO AND MFT.MFAC_NUMEDOCU = MF.MFAC_nUMEDOCU AND MF.MNIT_NIT = DS.MNIT_NITSEGUROS; "))
                                {
                                    Int16 diaCorte = Convert.ToInt16(DBFunctions.SingleData("SELECT COALESCE(PASE_DIACORTE,31) FROM DORDENSEGUROS DS left join paseguradora ps on ds.mnit_nitseguros = ps.mnit_nit WHERE DS.PDOC_CODIGO = '" + prefijoOT + "' AND DS.MORD_NUMEORDE = " + numOT + ";"));
                                    if (fechaFactura.SelectedDate.Day > diaCorte)
                                    {
                                        //           no se puede facturar posterior a la fecha de corte de la aseguradora                   
                                        Utils.MostrarAlerta(Response, "No se puede emitir Factura de la Aseguradora porque el día de la fecha es superior al dia de Corte ..!");
                                        cierraOrden = false;
                                        siSeguros = 0; // no se incrementa el contador de facturas
                                    }
                                    else
                                    {
                                        FacturaCliente facturaAseguradora = new FacturaCliente("FOTT", ddlPrefFact.SelectedValue, tbNitAseg.Text, almaOT, "F", Convert.ToUInt32(tbNumFact.Text.Trim()),
                                            Convert.ToUInt32(tbDiasVig.Text.Trim()), fechaFactura.SelectedDate, fechaFactura.SelectedDate.AddDays(Convert.ToInt32(tbDiasVig.Text.Trim())), Convert.ToDateTime(null),
                                            valAsegura, valIvaAsegura, 0, 0, 0, 0, DBFunctions.SingleData("SELECT pcen_centtal FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + almaOT + "'"),
                                            observacion, ddlRecep.SelectedValue, HttpContext.Current.User.Identity.Name.ToLower(), factorDeducible);

                                        //Ahora agregamos los sqlRels 
                                        for (i = 0; i < sqlRelsFact.Count; i++)
                                            facturaAseguradora.SqlRels.Add(sqlRelsFact[i].ToString());

                                        //Ahora generamos los sql de la factura y los asociamos al arraylist sqlFactAse
                                        facturaAseguradora.GrabarFacturaCliente(false);
                                        for (i = 0; i < facturaAseguradora.SqlStrings.Count; i++)
                                            sqlFactAse.Add(facturaAseguradora.SqlStrings[i].ToString());

                                        if (DBFunctions.Transaction(sqlFactAse))
                                        {
                                            //  CALCULA RETENCION ASEGURADORA
                                            // Invoco constructor Aseguradora
                                            Retencion retencion = new Retencion(facturaAseguradora.NitCliente,
                                            facturaAseguradora.PrefijoFactura,
                                            (int)facturaAseguradora.NumeroFactura,
                                            valAsegura,
                                            valIvaAsegura, "S", true);


                                            // AQUI DEBE CALCULAR Y GUARDAR LAS RETENCIONES en MFACTURACLIENTE y en MFACTURACLIENTERETENCION
                                            retencion.Guardar_Retenciones(true);

                                        //    Orden.AlmacenarCostosFacturaOT(facturaAseguradora.PrefijoFactura, facturaAseguradora.NumeroFactura);

                                            RevisionEstadoOT();
                                            lbOrdenTrabajo.Text = prefijoOT + "-" + Convert.ToUInt32(numOT);
                                            CargarFacturas(prefijoOT, Convert.ToUInt32(numOT), ddlFacturasGeneradas);
                                            plInfoFC.Visible = false;
                                            plhInfoFactAsegu.Visible = true;
                                            try
                                            {
                                                formatoRecibo.Prefijo = ddlPrefFact.SelectedValue;
                                                formatoRecibo.Numero = Convert.ToInt32(facturaAseguradora.NumeroFactura);
                                                formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefFact.SelectedValue + "'");
                                                if (formatoRecibo.Codigo != string.Empty)
                                                {
                                                    if (formatoRecibo.Cargar_Formato())
                                                        archivos[0] = formatoRecibo.Documento;
                                                }
                                            }
                                            catch
                                            {
                                                Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
                                            }
                                            contaOnline.contabilizarOnline(facturaAseguradora.PrefijoFactura, Convert.ToInt32(facturaAseguradora.NumeroFactura.ToString()), DateTime.Now, "");
                                        }
                                    }
                                }

                                //Ahora generamos la factura del cliente y le asociamos los sqlRelsFact - VERIFICANDO que el cargo NO ESTE FACTURADO
                                sqlFactAse.Clear();                    // se borra el arreglo de los sql de la aseguradora
                                factorDeducible = 1 - factorDeducible; // aqui calcula el factor del asegurado (cliente)
                                if ((factorDeducible > 0.0 || facturaenCero) && !DBFunctions.RecordExist(@"SELECT * FROM MFACTURACLIENTETALLER MFT, DORDENSEGUROS DS, mfacturacliente mF, MORDEN MO 
                                WHERE MFT.PDOC_PREFORDETRAB = '" + prefijoOT + @"' AND MFT.MORD_NUMEORDE = " + numOT + @" AND MFT.TCAR_CARGO = 'S' 
                                 AND MFT.PDOC_PREFORDETRAB =  DS.PDOC_CODIGO  AND MFT.MORD_NUMEORDE = DS.MORD_NUMEORDE
                                 AND MFT.PDOC_PREFORDETRAB =  MO.PDOC_CODIGO  AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
                                 AND MFT.PDOC_CODIGO = MF.PDOC_CODIGO AND MFT.MFAC_NUMEDOCU = MF.MFAC_nUMEDOCU AND MF.MNIT_NIT = MO.MNIT_NIT; "))
                                {
                                    Int32 numFactura = Convert.ToInt32(tbNumFact.Text.Trim());
                                    numFactura += siSeguros;
                                    FacturaCliente facturaClienteAse = new FacturaCliente("FOTT", ddlPrefFact.SelectedValue, tbNitCli.Text.Trim(), almaOT, "F", Convert.ToUInt32(numFactura.ToString().Trim()),
                                    Convert.ToUInt32(tbDiasVig.Text.Trim()), fechaFactura.SelectedDate, fechaFactura.SelectedDate.AddDays(Convert.ToInt32(tbDiasVig.Text.Trim())), Convert.ToDateTime(null),
                                    Math.Round(valCliente, numDecimales), Math.Round(valIvaCliente, numDecimales), 0, 0, 0, 0, DBFunctions.SingleData("SELECT pcen_centtal FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + almaOT + "'"),
                                    observacion, ddlRecep.SelectedValue, HttpContext.Current.User.Identity.Name.ToLower(), factorDeducible);

                                    // igualamos la fecha de proceso del deducible del cliente a la factura de la aseguradora
                                    // facturaClienteAse.timestamp = facturaAseguradora.timestamp;
                                    for (i = 0; i < sqlRelsFact.Count; i++)
                                        facturaClienteAse.SqlRels.Add(sqlRelsFact[i].ToString());

                                    //Ahora generamos los sql de la factura y los asociamos al arraylist sqlFactAse
                                    facturaClienteAse.GrabarFacturaCliente(false);
                                    for (i = 0; i < facturaClienteAse.SqlStrings.Count; i++)
                                        sqlFactAse.Add(facturaClienteAse.SqlStrings[i].ToString());

                                    //Ahora grabamos las facturas
                                    if (DBFunctions.Transaction(sqlFactAse))
                                    {

                                        // CALCULA RETENCION ASEGURADO = CLIENTE DUEÑO DEL CARRO
                                        // Invoco constructor Deducible Asegurado
                                        Retencion retencionAsegurado = new Retencion(tbNitCli.Text.Trim(),
                                        facturaClienteAse.PrefijoFactura,
                                        (int)facturaClienteAse.NumeroFactura + 1,
                                        null,
                                        valCliente,
                                        valIvaCliente, "S", true);

                                        //AQUI DEBE CALCULAR Y GUARDAR LAS RETENCIONES en MFACTURACLIENTE y en MFACTURACLIENTERETENCION
                                        retencionAsegurado.Guardar_Retenciones(true);

                                  //      Orden.AlmacenarCostosFacturaOT(facturaClienteAse.PrefijoFactura, facturaClienteAse.NumeroFactura);

                                        if (cierraOrden)  // se cierra la orden cargo seguros cuando se emiten las dos facturas
                                            RevisionEstadoOT();
                                        lbOrdenTrabajo.Text = prefijoOT + "-" + Convert.ToUInt32(numOT);
                                        CargarFacturas(prefijoOT, Convert.ToUInt32(numOT), ddlFacturasGeneradas);
                                        plInfoFC.Visible = false;
                                        plhInfoFactAsegu.Visible = true;
                                        try
                                        {
                                            formatoRecibo.Prefijo = ddlPrefFact.SelectedValue;
                                            formatoRecibo.Numero = Convert.ToInt32(facturaClienteAse.NumeroFactura);
                                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefFact.SelectedValue + "'");
                                            if (formatoRecibo.Codigo != string.Empty)
                                            {
                                                if (formatoRecibo.Cargar_Formato())
                                                    archivos[1] = formatoRecibo.Documento;
                                            }
                                        }
                                        catch
                                        {
                                            Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
                                        }
                                        contaOnline.contabilizarOnline(facturaClienteAse.PrefijoFactura, Convert.ToInt32(facturaClienteAse.NumeroFactura.ToString()), DateTime.Now, "");
                                    }
                                }

                                if (archivos[0] != string.Empty && archivos[1] != string.Empty)
                                {
                                    Response.Write("<script language:javascript>w=window.open('" + archivos[0] + "','','HEIGHT=600,WIDTH=700');y=window.open('" + archivos[1] + "','','HEIGHT=600,WIDTH=600')</script>");

                                    //      contaOnline.contabilizarOnlineSeguros(facturaAseguradora.PrefijoFactura, Convert.ToInt32(facturaAseguradora.NumeroFactura.ToString()), Convert.ToInt32(facturaClienteAse.NumeroFactura.ToString()), Convert.ToDateTime(fechaFactura.SelectedDate), "");
                                    //      contaOnline.contabilizarOnline(facturaAseguradora.PrefijoFactura, Convert.ToInt32(facturaClienteAse.NumeroFactura.ToString()), DateTime.Now, "");



                                    /*if(RevisionEstadoOT())
                                        Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT="+facturaClienteAse.PrefijoFactura+"&numOT="+facturaClienteAse.NumeroFactura+"&cargo="+cargoFacturar+"&dest=0");
                                        //Response.Redirect("" + indexPage + "?process=Automotriz.LiquidaOrden&factOT=1&prefF1="+facturaClienteAse.PrefijoFactura+"&numF1="+facturaClienteAse.NumeroFactura+"&car1=C&prefF2="+facturaAseguradora.PrefijoFactura+"&numF2="+facturaAseguradora.NumeroFactura+"&car2=S");
                                    else
                                        Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT="+facturaClienteAse.PrefijoFactura+"&numOT="+facturaClienteAse.NumeroFactura+"&cargo="+cargoFacturar+"&dest=1");
                                        //Response.Redirect("" + indexPage + "?process=Automotriz.ProcesosLiquidacion&pref="+prefijoOT+"&num="+numOT+"&factOT=1&prefF1="+facturaClienteAse.PrefijoFactura+"&numF1="+facturaClienteAse.NumeroFactura+"&car1=C&prefF2="+facturaAseguradora.PrefijoFactura+"&numF2="+facturaAseguradora.NumeroFactura+"&car2=S");*/
                                    //28 de Octubre de 2006 - Se modifica esta parte para mostrar al usuario las dos facturas que se han generado por separado para que el usuario las pueda ver y revisar

                                }
                                else
                                    lb.Text += "<br>Error : " + DBFunctions.exceptions;
                            }
                        }
                        else
                            Utils.MostrarAlerta(Response, "La Orden de Trabajo " + prefijoOT + "-" + numOT + " ya fue liquidada anteriormente.");
                    }
                    else
                        Utils.MostrarAlerta(Response, "El Valor del deducible Supera el valor de la Factura, Proceso no permitido Por favor Revice!");
                }              
                else
                    Utils.MostrarAlerta(Response, "Campo Obligatorio: Días de Plazo o Nit Cliente");
            }
         else
                Utils.MostrarAlerta(Response, "El consecutivo está fuera del Rango Autorizado por la DIAN, por favor informe a contabilidad.");
	    }       

        protected void IrVistaImpresion(Object  Sender, EventArgs e)
		{
			if(RevisionEstadoOT())
				Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT="+(ddlFacturasGeneradas.SelectedValue.Split('-'))[0]+"&numOT="+(ddlFacturasGeneradas.SelectedValue.Split('-'))[1].Trim()+"&cargo="+ddlCrgsRels.SelectedValue+"&dest=0");
			else
				Response.Redirect("" + indexPage + "?process=Automotriz.VistaImpresion&tipVis=liquidar&prefOT="+(ddlFacturasGeneradas.SelectedValue.Split('-'))[0]+"&numOT="+(ddlFacturasGeneradas.SelectedValue.Split('-'))[1]+"&cargo="+ddlCrgsRels.SelectedValue+"&dest=1");
		}

		protected void CambioFacturaAseguradora(Object  Sender, EventArgs e)
		{
			if(ddlFacturasGeneradas.Items.Count > 0)
				MostrarInfoFacturaAseguradora((ddlFacturasGeneradas.SelectedValue.Split('-'))[0],Convert.ToUInt32((ddlFacturasGeneradas.SelectedValue.Split('-'))[1]));
		}

        protected void CambioDescuento(Object Sender, EventArgs e)
        {
            bool errorDeValidacion = false;
            double netoDescuentoOperaciones = 0,
                   netoDescuentoRepuestos = 0,
                   ivaDescuentoOperaciones = 0,
                   ivaDescuentoRepuestos = 0,
                   descPreliqMob = 0, descPreliqRep = 0;
            try { descPreliqMob = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(MORD_VALODESCMOB, 0) FROM MORDENDESCUENTO WHERE PDOC_CODIGO = '" + prefijoOT + "' AND MORD_NUMEORDE = " + numOT + " and tcar_cargo = '" + ddlCrgsRels.SelectedValue + "' "));
                    if(descPreliqMob == 0) { tbDescuentoOperaciones.Text = ""; }
                    else { tbDescuentoOperaciones.Text = Convert.ToString(descPreliqMob);}                  
                }
            catch { descPreliqMob = 0; }
            try { descPreliqRep = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(MORD_VALODESCREP, 0) FROM MORDENDESCUENTO WHERE PDOC_CODIGO = '" + prefijoOT + "' AND MORD_NUMEORDE = " + numOT + " and tcar_cargo = '" + ddlCrgsRels.SelectedValue + "' "));
                    if(descPreliqRep == 0){ tbDescuentoRespuestos.Text = "";}
                    else { tbDescuentoRespuestos.Text = Convert.ToString(descPreliqRep); }                
                }
            catch { descPreliqRep = 0; }

            if (!tbDescuentoOperaciones.Text.Equals(""))
            {

                double descuentoOperaciones = Convert.ToDouble(tbDescuentoOperaciones.Text);
                netoDescuentoOperaciones = Convert.ToDouble(hdValNetOper.Value) - descuentoOperaciones;

                if (netoDescuentoOperaciones >= 0)
                {
                    double factor = (double)descuentoOperaciones / Convert.ToDouble(hdValNetOper.Value);
                    ivaDescuentoOperaciones = Math.Round(Convert.ToDouble(hdValIVAOper.Value) * (1 - factor), numDecimales);

                    tbValNetOperDescuento.Text = netoDescuentoOperaciones.ToString("C");
                    tbValIVAOperDescuento.Text = ivaDescuentoOperaciones.ToString("C");
                }
                else
                {
                    errorDeValidacion = true;
                    tbDescuentoOperaciones.Text = "";
                }
            }
            else
            {
                tbValNetOperDescuento.Text = tbValNetOper.Text;
                tbValIVAOperDescuento.Text = tbValIVAOper.Text;

                netoDescuentoOperaciones = Convert.ToDouble(hdValNetOper.Value);
                ivaDescuentoOperaciones = Convert.ToDouble(hdValIVAOper.Value.Trim());
            }

            if (!tbDescuentoRespuestos.Text.Equals("")){

                double descuentoRepuestos = Convert.ToDouble(tbDescuentoRespuestos.Text);
                netoDescuentoRepuestos = Convert.ToDouble(hdValNetItem.Value) - descuentoRepuestos;

                if (netoDescuentoRepuestos >= 0)
                {
                    double factor = (double)descuentoRepuestos / Convert.ToDouble(hdValNetItem.Value);
                    ivaDescuentoRepuestos = Math.Round(Convert.ToDouble(hdValIVAItem.Value) * (1 - factor), numDecimales);

                    tbValNetItemDescuento.Text = netoDescuentoRepuestos.ToString("C");
                    tbValIVAItemDescuento.Text = ivaDescuentoRepuestos.ToString("C");
                }
                else
                {
                    errorDeValidacion = true;
                    tbDescuentoRespuestos.Text = "";
                }
            }
            else
            {
                tbValNetItemDescuento.Text = tbValNetItem.Text;
                tbValIVAItemDescuento.Text = tbValIVAItem.Text;

                netoDescuentoRepuestos = Convert.ToDouble(hdValNetItem.Value);
                ivaDescuentoRepuestos = Convert.ToDouble(hdValIVAItem.Value); 
            }

            tbTotalDescuento.Text = Math.Round(netoDescuentoOperaciones + netoDescuentoRepuestos +
                                    ivaDescuentoOperaciones + ivaDescuentoRepuestos, numDecimales).ToString("C");      
            
            if (errorDeValidacion)
                Utils.MostrarAlerta(Response, "" +
                "El valor de descuento no puede exceder el valor neto");
                
        }

        // RETENCIONES

        public void dgrItems_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            gridRtns.EditItemIndex = -1;
            BindRetenciones();
        }

        public void dgrItems_Edit(Object sender, DataGridCommandEventArgs e)
        {
            if (tablaRtns.Rows.Count > 0)
                gridRtns.EditItemIndex = (int)e.Item.ItemIndex;
            BindRetenciones();
        }

        public void dgrItems_Update(Object sender, DataGridCommandEventArgs e)
        {
            double valorBase;
            try
            {
                valorBase = Convert.ToDouble(((TextBox)e.Item.FindControl("txtEdV")).Text.Replace(",", ""));
                if (valorBase <= 0) throw (new Exception());
                tablaRtns.Rows[e.Item.ItemIndex]["VALORBASE"] = valorBase;
                tablaRtns.Rows[e.Item.ItemIndex]["VALOR"] = Math.Round(valorBase * Convert.ToDouble(tablaRtns.Rows[e.Item.ItemIndex]["PORCRET"]) / 100, numDecimales);
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Valor base no válido.");
            }
            ViewState["TABLERETS"] = tablaRtns;
            gridRtns.EditItemIndex = -1;
            BindRetenciones();
        }

        protected void gridRtns_Item(object Sender, DataGridCommandEventArgs e)
        {
            DataRow fila;
            if (((Button)e.CommandSource).CommandName == "AgregarRetencion")
            {
                if ((((TextBox)e.Item.Cells[0].Controls[1]).Text == ""))
                    Utils.MostrarAlerta(Response, "Debe escoger un tipo de retención");
                else 
                    if (this.Buscar_Retencion(((TextBox)e.Item.Cells[0].Controls[1]).Text))
                        Utils.MostrarAlerta(Response, "Solo puede haber una retención de ese tipo");
                    else 
                        if (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text))
                            Utils.MostrarAlerta(Response, "Valor Invalido");
                        else
                        {
                            fila = tablaRtns.NewRow();
                            fila["CODRET"] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                            fila["NOMBRE"] = DBFunctions.SingleData("SELECT PRET_NOMBRE FROM PRETENCION WHERE PRET_CODIGO='" + fila["CODRET"].ToString() + "';");
                            fila["TRET_NOMBRE"] = DBFunctions.SingleData("SELECT TR.TRET_NOMBRE FROM PRETENCION PR, TRETENCION TR WHERE TR.TRET_CODIGO=PR.TRET_CODIGO AND PR.PRET_CODIGO='" + fila["CODRET"].ToString() + "';");
                            fila["TTIP_PROCESO"] = DBFunctions.SingleData("SELECT TP.TTIP_NOMBRE FROM PRETENCION PR, TTIPOPROCESO TP WHERE PR.TTIP_PROCESO=TP.TTIP_CODIGO AND PR.PRET_CODIGO='" + fila["CODRET"].ToString() + "';");
                            fila["TTIP_NOMBRE"] = DBFunctions.SingleData("SELECT TP.TTIP_NOMBRE FROM PRETENCION PR, TTIPOPERSONA TP WHERE PR.TTIP_CODIGO=TP.TTIP_CODIGO AND PR.PRET_CODIGO='" + fila["CODRET"].ToString() + "';");

                            fila["PORCRET"] = Convert.ToDouble(((TextBox)e.Item.Cells[5].Controls[1]).Text);
                            fila["VALORBASE"] = Convert.ToDouble(((TextBox)e.Item.Cells[6].Controls[1]).Text);
                            fila["VALOR"] = Convert.ToDouble(((TextBox)e.Item.Cells[7].Controls[1]).Text);
                            tablaRtns.Rows.Add(fila);
                            BindRetenciones();
                        }
            }
            else 
                if (((Button)e.CommandSource).CommandName == "RemoverRetencion")
                {
                    tablaRtns.Rows.RemoveAt(e.Item.DataSetIndex);
                    BindRetenciones();
                }
        }

        protected void gridRtns_ItemDataBound(object Sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                TextBox txtBase=(TextBox)e.Item.FindControl("base");
                TextBox txtPorcentaje=(TextBox)e.Item.FindControl("codretb");
                TextBox txtValor=(TextBox)e.Item.FindControl("valor");
                string scrTotal="PorcentajeVal('" + txtPorcentaje.ClientID + "','" + txtBase.ClientID + "','" + txtValor.ClientID + "');";
                txtBase.Attributes.Add("onkeyup", "NumericMaskE(this,event);" + scrTotal);

                ((TextBox)e.Item.Cells[0].Controls[1]).Attributes.Add("onClick", "ModalDialog(this,'SELECT pret_codigo,pret_nombre, pret_porcendecl FROM pretencion ORDER BY tret_codigo',new Array());" + scrTotal);
            }
        }

        #endregion
		
		#region Metodos
        protected void CargarCargosRelacionados(DropDownList ddl)
        {
            int i;
            ddl.Items.Clear();
            //Traemos los cargos relacionados con las operaciones y con los repuestos
            //Tambien se traen las ordenes de seguros cuando se haya facturado solo se haya facturado un solo cliente (asegurado o aseguradora)

            string sql1 = String.Format(
             @"SELECT DISTINCT DOR.tcar_cargo AS CODIGO,  
                     TCA.tcar_nombre AS NOMBRE 
              FROM dordenoperacion DOR, 
                   tcargoorden TCA 
              WHERE DOR.pdoc_codigo = '{0}'  
               AND  DOR.mord_numeorde = {1} 
               AND  DOR.tcar_cargo = TCA.tcar_cargo 
               AND  DOR.tcar_cargo <> 'X' 
               AND  DOR.tcar_cargo NOT IN (SELECT TCAR_CARGO FROM Mfacturaclientetaller mft 
                                           WHERE DOR.PDOC_CODIGO = MFT.PDOC_PREFORDETRAB AND DOR.MORD_numeorde = mft.mord_numeorde)
                union

              SELECT 'S' as CODIGO, 'Seguros' as NOMBRE from (SELECT COUNT(*) as facturas FROM MFACTURACLIENTETALLER  MFT, DORDENSEGUROS DS 
                WHERE TCAR_CARGO = 'S' AND mft.PDOC_PREFORDETRAB = '{0}' AND mft.MORD_NUMEORDE = {1}  AND PDOC_PREFORDETRAB = DS.PDOC_CODIGO and MFT.MORD_NUMEORDE = DS.MORD_NUMEORDE 
                  ) as a where facturas = 1;"
             , prefijoOT
             , numOT);

            string sql2 = String.Format(
            @"SELECT distinct CODIGO, NOMBRE FROM (  
              SELECT DISTINCT MOT.tcar_cargo AS CODIGO, TCA.tcar_nombre AS NOMBRE, d1.mite_codigo, sum(D1.DITE_CANTIDAD) - sum(COALESCE(D2.DITE_CANTIDAD,0)) AS CANTIDAD   
              FROM  tcargoorden TCA, mordentransferencia MOT, ditems d1   
              	   left join ditems d2 on d1.pdoc_codigo = d2.dite_prefdocurefe and d1.dite_numedocu=d2.dite_numedocurefe AND D1.MITE_CODIGO = D2.MITE_CODIGO AND D2.TMOV_TIPOMOVI = 81  
              WHERE MOT.pdoc_codigo = '{0}' AND  MOT.mord_numeorde = {1}  
               AND  d1.pdoc_codigo = mot.pdoc_factura and d1.dite_numedocu = mot.mfac_numero AND d1.tmov_tipomovi=80  
               AND  MOT.tcar_cargo = TCA.tcar_cargo
               AND  MOT.tcar_cargo NOT IN (SELECT TCAR_CARGO FROM Mfacturaclientetaller mft 
                                           WHERE MOT.PDOC_CODIGO = MFT.PDOC_PREFORDETRAB AND MOT.MORD_numeorde = mft.mord_numeorde)
               group by MOT.tcar_cargo, TCA.tcar_nombre, d1.mite_codigo, D1.DITE_CANTIDAD
                ) AS A WHERE CANTIDAD > 0;"
             , prefijoOT
             , numOT);  

            DataSet dsCargos = new DataSet();
            DBFunctions.Request(dsCargos, IncludeSchema.NO, sql1 + sql2);
            for (i = 0; i < dsCargos.Tables[0].Rows.Count; i++)
                ddl.Items.Add(new ListItem(dsCargos.Tables[0].Rows[i][1].ToString(), dsCargos.Tables[0].Rows[i][0].ToString()));
            for (i = 0; i < dsCargos.Tables[1].Rows.Count; i++)
            {
                if ((dsCargos.Tables[0].Select("CODIGO='" + dsCargos.Tables[1].Rows[i][0].ToString() + "'")).Length == 0)
                    ddl.Items.Add(new ListItem(dsCargos.Tables[1].Rows[i][1].ToString(), dsCargos.Tables[1].Rows[i][0].ToString()));
            }
        }

        protected void CargarOperaciones(string cargo)
        {
            double valorElectricidad = 0,
                valorElectronica = 0,
                valorLatoneria = 0,
                valorManoObra = 0,
                valorPintura = 0,
                valorTerceros = 0,
                valorTapiceria = 0,
                valorTrabajos = 0;

            int i = 0;
            DataSet dsOper = new DataSet();
            DBFunctions.Request(dsOper, IncludeSchema.NO,
             "SELECT DOR.ptem_operacion AS CODIGO, \n" +
             "       PTE.ptem_descripcion AS DESCRIPCION, \n" +
             "       PVE.pven_nombre AS MECANICO, \n" +
             "       PTE.ptem_exceiva AS EXCENCION, \n" +
             "       DOR.dord_tiemliqu AS TIEMPO, \n" +
             "       DOR.dord_valooper AS VALOROPERACION, \n" +
             "       tcar_cargo AS cargo, \n" +
             "       PTE.tope_codigo AS TOPECODIGO \n" +
             "FROM dordenoperacion DOR, \n" +
             "     ptempario PTE, \n" +
             "     pvendedor PVE \n" +
             "WHERE DOR.ptem_operacion = PTE.ptem_operacion \n" +
             "AND   DOR.pven_codigo = PVE.pven_codigo \n" +
             "AND   DOR.tcar_cargo = '" + cargo + "' \n" +
             "AND   DOR.pdoc_codigo = '" + prefijoOT + "' \n" +
             "AND   DOR.mord_numeorde = " + numOT);

            double valorOperaciones = 0;
            double valorIva = 0;
            double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));

            double operacionesConIva = 0;
            double operacionesSinIva = 0;
            double operacionesRound = 0;
            cargoCortesia = "X";
            double soloOperacionesRoundAux = 0;   //Para calculo de perdida de decimales. (cuando usa 2 decimales).
            double soloOperacionesRound = 0;      //Para calculo de perdida de decimales. (cuando usa 4 decimales).

            for (i = 0; i < dsOper.Tables[0].Rows.Count; i++)
            {
                double valorOperacion = Convert.ToDouble(dsOper.Tables[0].Rows[i][5]);

                if (dsOper.Tables[0].Rows[i]["CARGO"].ToString() != "X" && dsOper.Tables[0].Rows[i]["VALOROPERACION"].ToString() != "0.00")
                    cargoCortesia = dsOper.Tables[0].Rows[i]["CARGO"].ToString();

                if (dsOper.Tables[0].Rows[i][3].ToString() == "S"   // Operaciones excenta de IVA
                    || ((dsOper.Tables[0].Rows[i][6].ToString() == "A" || dsOper.Tables[0].Rows[i][6].ToString() == "I" || dsOper.Tables[0].Rows[i][6].ToString() == "T") && ivaOperInter == "N") //es cargo Alistamiento o Interno, y no liquida IVA internamente
                    || (dsOper.Tables[0].Rows[i][6].ToString() == "G" && liqGarantiaFabrica == "N")) // es cargo Garantía y no liquida IVA en garantias
                {
                    operacionesSinIva += valorOperacion;
                    if (dsOper.Tables[0].Rows[i][7].ToString() != "REP")
                    {
                        soloOperacionesRoundAux += Math.Round(valorOperacion, 2);
                        soloOperacionesRound += valorOperacion;
                    }
                }
                else // Si liquida IVA
                {
                    //operacionesConIva += valorOperacion;
                    operacionesConIva += Math.Round(valorOperacion * 1, numDecimales);
                    operacionesRound += valorOperacion;
                    if (dsOper.Tables[0].Rows[i][7].ToString() != "REP")
                    {
                        soloOperacionesRoundAux += Math.Round(valorOperacion * 1, 2);
                        soloOperacionesRound += valorOperacion;
                    }
                }

                Tools.DeterminarTipoOperacionSuma(dsOper.Tables[0].Rows[i][0].ToString(), valorOperacion, ref valorElectricidad, ref valorElectronica, ref valorLatoneria, ref valorManoObra, ref valorPintura, ref valorTerceros, ref valorTapiceria, ref valorTrabajos);
            }

            operacionesConIva = operacionesConIva * 1 ;
            //valorOperaciones = Math.Round(operacionesConIva + operacionesSinIva, numDecimales);
            valorOperaciones  = Math.Round(operacionesRound + operacionesSinIva, numDecimales);
            double diferenciaDecimales = Math.Round(soloOperacionesRound - soloOperacionesRoundAux, 2);
            string numDec     = ConfigurationManager.AppSettings["tamanoDecimal"];
            bool usarDecimales = Convert.ToBoolean(ConfigurationManager.AppSettings["UsarDecimales"]);

            if (usarDecimales && diferenciaDecimales >= 0.01 && numDec == "4")
            {
                //Subir decimales a una operacion.
                for (int k = 0; k < dsOper.Tables[0].Rows.Count; k++)
                {
                    if (dsOper.Tables[0].Rows[k][7].ToString() != "REP")
                    {
                        double valorOperacion = Convert.ToDouble(dsOper.Tables[0].Rows[k][5]);
                        if (dsOper.Tables[0].Rows[k][3].ToString() == "S"   // Operaciones excenta de IVA
                           || ((dsOper.Tables[0].Rows[k][6].ToString() == "A" || dsOper.Tables[0].Rows[k][6].ToString() == "I" || dsOper.Tables[0].Rows[k][6].ToString() == "T") && ivaOperInter == "N") //es cargo Alistamiento o Interno, y no liquida IVA internamente
                           || (dsOper.Tables[0].Rows[k][6].ToString() == "G" && liqGarantiaFabrica == "N")) // es cargo Garantía y no liquida IVA en garantias
                        {
                            valorOperacion += diferenciaDecimales;
                        }
                        else
                        {
                            valorOperacion = Math.Round(valorOperacion * 1, 2);
                            valorOperacion += diferenciaDecimales;
                            valorOperacion = valorOperacion * 1;
                        }

                        ViewState["valorOpDecimal"] = valorOperacion;
                        ViewState["operacionDec"] = dsOper.Tables[0].Rows[k][0].ToString();
                        //Actualizar tabla con valor modificado para corregir decimales.
                        //DBFunctions.NonQuery("UPDATE DORDENOPERACION SET DORD_VALOOPER = " + valorOperacion + " WHERE PDOC_CODIGO = '" + prefijoOT + "' AND   MORD_NUMEORDE = " + numOT + " AND PTEM_OPERACION = '" + dsOper.Tables[0].Rows[k][0].ToString() + "';");
                        k = dsOper.Tables[0].Rows.Count;
                    }
                    else
                    {
                        ViewState["valorOpDecimal"] = null;
                        ViewState["operacionDec"] = null;
                    }
                }
            }
            else
            { 
                ViewState["valorOpDecimal"] = null;
                ViewState["operacionDec"] = null;
            }

            valorIva = Math.Round(operacionesConIva * porcentajeIva * 0.01, numDecimales); // IGUAL A / 100

			dgInfoOper.DataSource = dsOper.Tables[0];
			dgInfoOper.DataBind();

            string numDecUsar = ConfigurationManager.AppSettings["tamanoDecimal"];
            if (numDec != null && numDecUsar != "")
            {
                numDecimales = Convert.ToInt16(numDec);
            }
            else
            {
                numDecimales = 2;
            }

			DatasToControls.JustificacionGrilla(dgInfoOper,dsOper.Tables[0]);
            tbValNetOper.Text   = valorOperaciones.ToString("C" + numDecimales);
			hdValNetOper.Value  = valorOperaciones.ToString();
            tbValIVAOper.Text   = valorIva.ToString("C" + numDecimales);
			hdValIVAOper.Value  = valorIva.ToString();
            lbElectricidad.Text = valorElectricidad.ToString("C" + numDecimales);
            lbElectronica.Text  = valorElectronica.ToString("C" + numDecimales);
            lbLatoneria.Text    = valorLatoneria.ToString("C" + numDecimales);
            lbManodeObra.Text   = valorManoObra.ToString("C" + numDecimales);
            lbPintura.Text      = valorPintura.ToString("C" + numDecimales);
            lbTerceros.Text     = valorTerceros.ToString("C" + numDecimales);
            lbTapiceria.Text    = valorTapiceria.ToString("C" + numDecimales);
            lbTerceros.Text     = valorTerceros.ToString("C" + numDecimales);
            lbTrabajosVarios.Text = valorTrabajos.ToString("C" + numDecimales);
		}
		
		protected void CargarItems(string cargo)
		{
			dtItems = PrepararDtRepuestos();
			DataSet dsItems   = new DataSet();
			double valorItems = 0;
			double valorIva   = 0;
			double valorBase  = 0;
            // el iva de debe traer de ditems
            DBFunctions.Request(dsItems, IncludeSchema.NO,
             @"SELECT * FROM (
                SELECT DBXSCHEMA.EDITARREFERENCIAS (DIT.mite_codigo,PLIN.plin_tipo),  
                     MIT.mite_nombre,  
                     DIT.dite_valounit,  
                     DIT.piva_porciva ,  
                     (DIT.dite_cantidad - coalesce(sum(did.dite_cantidad),0)) as DITE_cantidad,  
                     DIT.pdoc_codigo CONCAT '-' CONCAT CAST(DIT.dite_numedocu AS CHARACTER(15)),  
                     DIT.mite_codigo,  
                     DIT.dite_porcdesc,  
                     MOT.TCAR_CARGO  
                FROM mitems MIT,  
                     plineaitem PLIN,  
                     mordentransferencia MOT,
                     ditems DIT LEFT JOIN ditems did on diT.pdoc_codigo = diD.dite_prefdocurefe and diT.dite_numedocu = diD.dite_numedocurefe and dit.mite_codigo = did.mite_codigo and did.tmov_tipomovi = 81
               WHERE DIT.mite_codigo = MIT.mite_codigo  
               AND   MIT.plin_codigo = PLIN.plin_codigo  
               AND   DIT.pdoc_codigo = MOT.pdoc_factura  
               AND   DIT.dite_numedocu = MOT.mfac_numero  
               AND   DIT.dite_cantidad > 0  
               AND   MOT.pdoc_codigo = '" + prefijoOT + @"'  
               AND   MOT.mord_numeorde = " + numOT + @"  
               AND   MOT.tcar_cargo = '" + cargo + @"'  
               AND   DIT.TMOV_tipomovi = 80
               group by DIT.mite_codigo,
  		             PLIN.plin_tipo,  
                     MIT.mite_nombre,  
                     DIT.dite_valounit,  
                     DIT.piva_porciva ,  
		             dit.dite_cantidad,
                     DIT.pdoc_codigo, 
		             DIT.dite_numedocu,  
                     DIT.mite_codigo,  
                     DIT.dite_porcdesc,  
                     MOT.TCAR_CARGO 
                )  as A WHERE A.DITE_CANTIDAD <> 0");
			
            for (int i=0; i < dsItems.Tables[0].Rows.Count; i++)
            {
                double cantidad = Convert.ToDouble(dsItems.Tables[0].Rows[i][4].ToString()); 
                /*
                    Items.ConsultaRelacionItemsTransferencias(
                        dsItems.Tables[0].Rows[i][6].ToString(),
                        (dsItems.Tables[0].Rows[i][5].ToString().Split('-'))[0],
					    Convert.ToInt64((dsItems.Tables[0].Rows[i][5].ToString().Split('-'))[1].Trim()),
                        80,81); */

				if (cantidad > 0)
				{
					DataRow fila = dtItems.NewRow();
					fila[0] = dsItems.Tables[0].Rows[i][0].ToString();
					fila[1] = dsItems.Tables[0].Rows[i][1].ToString();
					fila[2] = Convert.ToDouble(dsItems.Tables[0].Rows[i][2]);
                    fila[3] = (cargo == "G" && liqGarantiaFabrica == "N") ? 0 : Convert.ToDouble(dsItems.Tables[0].Rows[i][3]);
                    fila[4] = cantidad;

					double valorItem = 0;
                    double valorIvaItem = 0;
                    double valorTotal = 0;
					
					valorItem = Convert.ToDouble(dsItems.Tables[0].Rows[i][2]) * cantidad;
                    valorItem = valorItem - (valorItem*(Convert.ToDouble(dsItems.Tables[0].Rows[i][7])/100));
					
                    if(Convert.ToDouble(dsItems.Tables[0].Rows[i][3]) > 0)
						valorBase += valorItem;

                    if (((dsItems.Tables[0].Rows[i][8].ToString() == "A" ||
                        dsItems.Tables[0].Rows[i][8].ToString() == "I" ||
                        dsItems.Tables[0].Rows[i][8].ToString() == "T") && ivaOperInter == "N") ||
                        (dsItems.Tables[0].Rows[i][8].ToString() == "G" && liqGarantiaFabrica == "N"))
                    {
                        valorIvaItem = 0;
                    }
                    else
                    {
                  	    valorIvaItem = valorItem * (Convert.ToDouble(dsItems.Tables[0].Rows[i][3]) / 100);
                    }

					valorTotal  = valorItem + valorIvaItem;
					fila[5]     = Math.Round(valorTotal, numDecimales);
					fila[6]     = dsItems.Tables[0].Rows[i][5].ToString();
					dtItems.Rows.Add(fila);
					valorItems  += valorItem;
					valorIva    += valorIvaItem;
                }
			}
            
			dgInfoItems.DataSource = dtItems;
			dgInfoItems.DataBind();
			DatasToControls.JustificacionGrilla(dgInfoItems,dtItems);

            string numDec = ConfigurationManager.AppSettings["tamanoDecimal"];
            if (numDec != null && numDec != "")
            {
                numDecimales = Convert.ToInt16(numDec);
            }
            else
            {
                numDecimales = 2;
            }

            tbValNetItem.Text   = valorItems.ToString("C" + numDecimales);
			hdValNetItem.Value  = valorItems.ToString();
            tbValIVAItem.Text   = valorIva.ToString("C" + numDecimales);
			hdValIVAItem.Value  = valorIva.ToString();
			hdValBase.Value     = valorBase.ToString();

            ViewState.Add("dtItems", dtItems);
		}
		
		protected DataTable PrepararDtRepuestos()
		{
			DataTable dtRepuestos = new DataTable();
			dtRepuestos.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));//0
			dtRepuestos.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//1
			dtRepuestos.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));//2
			dtRepuestos.Columns.Add(new DataColumn("IVA",System.Type.GetType("System.Double")));//3
			dtRepuestos.Columns.Add(new DataColumn("CANTIDAD",System.Type.GetType("System.Double")));//4
			dtRepuestos.Columns.Add(new DataColumn("TOTAL",System.Type.GetType("System.Double")));//5
			dtRepuestos.Columns.Add(new DataColumn("TRANSFERENCIA",System.Type.GetType("System.String")));//6
			return dtRepuestos;
		}

        protected DataTable repuestosParaRetencion()
        {
            /*
             * ESTO ES SIMPLEMENTE PARA REUTILIZAR EL CODIGO DE
             * RETENCIONES PARA REPUESTOS. VARIAS DE ESTAS COLUMNAS NO SE USARÁN
             */
            DataTable items = (DataTable)ViewState["dtItems"];
            DataTable dtRepuestosretencion = new DataTable();

            dtRepuestosretencion.Columns.Add(new DataColumn("num_ped", typeof(string)));//0 Numero pedido o Prerecepcion
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_codigo", typeof(string)));//1 codigo Item
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_nombre", typeof(string)));//2 nombre Item
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_cantped", typeof(double)));//3 Cantidad ingresada
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_cantfac", typeof(double)));//4 Cantidad facturada
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_precio", typeof(double)));//5 Valor unidad
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_desc", typeof(double)));//6 Descuento
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_iva", typeof(double)));//7 Iva
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_ubic", typeof(string)));//8 Ubicacion
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_tot", typeof(double)));//9 Total
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_unid", typeof(string)));//10 Unidad medida
            dtRepuestosretencion.Columns.Add(new DataColumn("plin_codigo", typeof(string)));//11 Linea Bodega
            dtRepuestosretencion.Columns.Add(new DataColumn("mite_us", typeof(double)));//12 Valor moneda extranjera

            String aux;
            for (int i = 0; i < items.Rows.Count; i++)
            {
                aux = DBFunctions.SingleData("SELECT mcli_porcdescinv FROM mcliente WHERE mnit_nit='" + lbNitCliente.Text + "';");
                aux = (aux == "") ? "0" : aux;

                DataRow row = dtRepuestosretencion.NewRow();
                row[0] = "";
                row[1] = items.Rows[i][0];
                row[2] = items.Rows[i][1];
                row[3] = 0;
                row[4] = items.Rows[i][4];
                row[5] = items.Rows[i][2];
                row[6] = Convert.ToDouble(aux);
                row[7] = items.Rows[i][3];
                row[8] = "";
                row[9] = items.Rows[i][5];
                row[10] = "";
                row[11] = DBFunctions.SingleData("select plin_codigo from mitems where mite_codigo='" + items.Rows[i][0] + "';");
                row[12] = 0;

                dtRepuestosretencion.Rows.Add(row);
            }

            return dtRepuestosretencion;
        }
		
		protected bool RevisionEstadoOT()
		{
			int i;
			bool estadoFactOT = true;
			DataSet dsCargos = new DataSet();
           
			string cargoOT = DBFunctions.SingleData("SELECT tcar_cargo FROM morden WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numOT+"");
            DBFunctions.Request(dsCargos, IncludeSchema.NO, "SELECT DISTINCT tcar_cargo FROM dordenoperacion WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo <> 'X';" +
				"SELECT TCAR_CARGO FROM (SELECT mot.tcar_cargo, SUM(DI.DITE_CANTIDAD)-COALESCE(SUM(DD.DITE_CANTIDAD),0) AS CANTIDAD_ITEMS " +
                "FROM MORDENTRANSFERENCIA MOT,  DITEMS DI " +
                 "LEFT JOIN DITEMS DD ON DI.PDOC_CODIGO = DD.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DD.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DD.MITE_CODIGO " +
                "WHERE DI.PDOC_CODIGO = MOT.PDOC_FACTURA AND DI.DITE_NUMEDOCU = MOT.MFAC_NUMERO " +
                "AND MOT.PDOC_CODIGO = '" + prefijoOT + "' and MOT.mord_numeorde = " + numOT + " " +
                 "group by mot.tcar_cargo ) WHERE CANTIDAD_ITEMS > 0;" +  // DESCARTA LAS TRANSFERENCIAS CON DEVOLUCION
                "SELECT DISTINCT tcar_cargo AS CARGO FROM mfacturaclientetaller WHERE pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numOT + " AND tcar_cargo <> 'X';");
			//Primero comparamos la tabla 0 con la tabla 2
			for(i=0;i<dsCargos.Tables[0].Rows.Count;i++)
			{
				if(estadoFactOT)
				{
					if((dsCargos.Tables[2].Select("CARGO='"+dsCargos.Tables[0].Rows[i][0].ToString()+"'")).Length == 0)
						estadoFactOT = false;
				}
			}
			//Ahora comparamos la tabla 1 con la 2
			for(i=0;i<dsCargos.Tables[1].Rows.Count;i++)
			{
				if(estadoFactOT)
				{
					if((dsCargos.Tables[2].Select("CARGO='"+dsCargos.Tables[1].Rows[i][0].ToString()+"'")).Length == 0)
						estadoFactOT = false;
				}
			}
			//Cambio hecho el dia 29 de mayo de 2007 recorremos la tabla que genera la consulta y de ella determinamos que facturas estan asociadas a los cargos I(Interno) y A(Alistamiento)
			DataSet dsFacturasRelacionadasOT = new DataSet();
			DBFunctions.Request(dsFacturasRelacionadasOT,IncludeSchema.NO,"SELECT MFT.pdoc_codigo, MFT.mfac_numedocu, MFC.mfac_valofact + MFC.mfac_valoiva + MFC.mfac_valoflet + MFC.mfac_valoivaflet - MFC.mfac_valorete, MFT.tcar_cargo FROM mfacturaclientetaller MFT INNER JOIN mfacturacliente MFC ON MFT.pdoc_codigo = MFC.pdoc_codigo AND MFT.mfac_numedocu = MFC.mfac_numedocu WHERE MFT.tcar_cargo = 'I' AND MFT.pdoc_prefordetrab = '"+prefijoOT+"' AND mord_numeorde="+numOT);
			for(i=0;i<dsFacturasRelacionadasOT.Tables[0].Rows.Count;i++)
			{
				if(dsFacturasRelacionadasOT.Tables[0].Rows[i][3].ToString() == "I" || dsFacturasRelacionadasOT.Tables[0].Rows[i][3].ToString() == "A" || dsFacturasRelacionadasOT.Tables[0].Rows[i][3].ToString() == "T")
					DBFunctions.NonQuery("UPDATE mfacturacliente SET tvig_vigencia='C', mfac_valoabon="+dsFacturasRelacionadasOT.Tables[0].Rows[i][2].ToString()+" WHERE pdoc_codigo='"+dsFacturasRelacionadasOT.Tables[0].Rows[i][0].ToString()+"' AND mfac_numedocu="+dsFacturasRelacionadasOT.Tables[0].Rows[i][1].ToString());
			}
            string mercadeista = DBFunctions.SingleData("SELECT PVEN_MERCADEISTA FROM morden WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");

            if (ddlVendedor.SelectedValue != "No Aplica...")
            {
                if (mercadeista != ddlVendedor.SelectedValue)
                {
                    mercadeista = ddlVendedor.SelectedValue;
                    DBFunctions.NonQuery("UPDATE morden SET PVEN_MERCADEISTA='" + mercadeista + "' WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                }
            }
            else
            {
                DBFunctions.NonQuery("UPDATE morden SET PVEN_MERCADEISTA=null WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
            }

            if(estadoFactOT)
			{
                Int16 cargosporFacturar = Convert.ToInt16(DBFunctions.SingleData(@"select coalesce(count(distinct tcar_cargo),0) from (
                                            select distinct tcar_cargo from dordenoperacion where PDOC_CODIGO = '" + prefijoOT + @"' AND MORD_NUMEORDE = " + numOT + @" and tcar_cargo not in 
                                            (SELECT tcar_cargo FROM MFACTURACLIENTETALLER WHERE PDOC_PREFORDETRAB = '" + prefijoOT + @"' AND MORD_NUMEORDE = " + numOT + @") AND TCAR_CARGO <> 'X'
                                             union
                                            select distinct TCAR_CARGO FROM (SELECT mot.tcar_cargo, SUM(DI.DITE_CANTIDAD)-COALESCE(SUM(DD.DITE_CANTIDAD),0) AS CANTIDAD_ITEMS  
                                              FROM MORDENTRANSFERENCIA MOT,  DITEMS DI  
                                              LEFT JOIN DITEMS DD ON DI.PDOC_CODIGO = DD.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DD.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DD.MITE_CODIGO  
                                             WHERE DI.PDOC_CODIGO = MOT.PDOC_FACTURA AND DI.DITE_NUMEDOCU = MOT.MFAC_NUMERO  
                                               AND MOT.PDOC_CODIGO = '" + prefijoOT + @"' and MOT.mord_numeorde = " + numOT + @" 
                                             group by mot.tcar_cargo ) WHERE CANTIDAD_ITEMS > 0 and tcar_cargo not in 
                                            (SELECT tcar_cargo FROM MFACTURACLIENTETALLER WHERE PDOC_PREFORDETRAB = '" + prefijoOT + @"' AND MORD_NUMEORDE = " + numOT + @")); ") );
                if (cargosporFacturar == 0)
                {
                    //cuando la orden sea cargo Interno, Garantia Taller o Alistamiento, se deja en estado entregada
                    if (cargoOT == "I" || cargoOT == "A" || cargoOT == "T")
                        DBFunctions.NonQuery("UPDATE morden SET test_estado='E',mord_estaliqu='L',mord_fechliqu='" + DateTime.Now.ToString("yyyy-MM-dd") + "',mord_salida='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "' WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                    else if (cargoOT == "C" || cargoOT == "G") // Cargo Cliente o Garantía de Fábrica
                            DBFunctions.NonQuery("UPDATE morden SET test_estado='F',mord_estaliqu='L',mord_fechliqu='" + DateTime.Now.ToString("yyyy-MM-dd") + "',mord_salida='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + "' WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                        else// Cargo Seguros
                        {
                            Int16 facturas = Convert.ToInt16(DBFunctions.SingleData("SELECT COALESCE(COUNT(*),0) FROM MFACTURACLIENTETALLER MFT WHERE MFT.PDOC_PREFORDETRAB = '" + prefijoOT + "' AND MFT.MORD_NUMEORDE = " + numOT + " AND TCAR_CARGO = 'S' "));
                            string deducible = DBFunctions.SingleData("SELECT COALESCE(MORD_DEDUMINIMO,0) FROM DORDENSEGUROS DS WHERE DS.PDOC_CODIGO = '" + prefijoOT + "' AND DS.MORD_NUMEORDE = " + numOT + " ");
                            if (facturas == 2 || (facturas == 1 && deducible == "0.00"))
                                DBFunctions.NonQuery("UPDATE morden SET test_estado='F',mord_estaliqu='L',mord_fechliqu='" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE pdoc_codigo='" + prefijoOT + "' AND mord_numeorde=" + numOT + "");
                        }
                }
            }
			return estadoFactOT;
		} 

		protected void CargarFacturas(string prefOT, uint numeroOT, DropDownList ddlFact)
		{
			bind.PutDatasIntoDropDownList(ddlFact,"SELECT MFCT.pdoc_codigo CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)),PDO.pdoc_nombre CONCAT '-' CONCAT CAST(MFCT.mfac_numedocu AS CHARACTER(6)) FROM dbxschema.mfacturaclientetaller MFCT, pdocumento PDO, tcargoorden TCAR WHERE PDO.pdoc_codigo=MFCT.pdoc_codigo AND TCAR.tcar_cargo=MFCT.tcar_cargo AND MFCT.pdoc_prefordetrab='"+prefOT+"' AND mord_numeorde="+numeroOT+"");
            	if(ddlFact.Items.Count > 0)
				    MostrarInfoFacturaAseguradora((ddlFact.SelectedValue.Split('-'))[0],Convert.ToUInt32((ddlFact.SelectedValue.Split('-'))[1]));
		}

		private void MostrarInfoFacturaAseguradora(string prefFact, uint numeroFact)
		{
			DataSet dsFact = new DataSet();
			DBFunctions.Request(dsFact,IncludeSchema.NO,"SELECT MF.mnit_nit, MN.mnit_nombres CONCAT ' ' CONCAT MN.mnit_apellidos, MF.mfac_valofact, MF.mfac_valoiva FROM mfacturacliente MF INNER JOIN mnit MN ON MF.mnit_nit=MN.mnit_nit WHERE MF.pdoc_codigo='"+prefFact+"' AND mfac_numedocu="+numeroFact);
			lbInfoFactura.Text = "Nit cliente factura : "+dsFact.Tables[0].Rows[0][0].ToString();
			lbInfoFactura.Text +="<br>Cliente factura : "+dsFact.Tables[0].Rows[0][1].ToString();
			lbInfoFactura.Text +="<br>Valor factura   : "+Convert.ToDouble(dsFact.Tables[0].Rows[0][2]).ToString("C");
			lbInfoFactura.Text +="<br>IVA factura     : "+Convert.ToDouble(dsFact.Tables[0].Rows[0][3]).ToString("C");
		}

        //RETENCIONES

        protected bool Buscar_Retencion(string rtn)
        {
            bool encontrado=false;
            if (tablaRtns != null && tablaRtns.Rows.Count != 0)
            {
                for (int i=0; i < tablaRtns.Rows.Count; i++)
                {
                    if (tablaRtns.Rows[i][0].ToString() == rtn)
                        encontrado = true;
                }
            }
            return encontrado;
        }

        public void BindRetenciones()
        {
            ViewState["TABLERETS"] = tablaRtns;
            gridRtns.DataSource = tablaRtns;
            if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                gridRtns.Columns[gridRtns.Columns.Count - 2].Visible = gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
            gridRtns.DataBind();
            if (ConfigurationManager.AppSettings["ModificaRetenciones"] != null && !Convert.ToBoolean(ConfigurationManager.AppSettings["ModificaRetenciones"]))
                gridRtns.Columns[gridRtns.Columns.Count - 2].Visible = gridRtns.Columns[gridRtns.Columns.Count - 1].Visible = false;
        }

        protected void Cargar_Tabla_Rtns()
        {
            tablaRtns = new DataTable();
            tablaRtns.Columns.Add(new DataColumn("CODRET", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("NOMBRE", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("TRET_NOMBRE", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("TTIP_PROCESO", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("TTIP_NOMBRE", typeof(string)));
            tablaRtns.Columns.Add(new DataColumn("PORCRET", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("VALORBASE", typeof(double)));
            tablaRtns.Columns.Add(new DataColumn("VALOR", typeof(double)));
        }

        protected bool GuardarRetenciones(string pref, string numFac)
        {

            ArrayList arlSql=new ArrayList();
            double totalR=0;
            //Eliminar actuales
            arlSql.Add("DELETE FROM MFACTURACLIENTERETENCION WHERE PDOC_CODIGO='" + pref + "' AND MFAC_NUMEDOCU=" + numFac + ";");

            for (int n=0; n < tablaRtns.Rows.Count; n++)
            {
                aplicaRetencionAutomatica = false;
                arlSql.Add("INSERT INTO MFACTURACLIENTERETENCION " +
                    "VALUES('" + pref + "'," + numFac + "," +
                    "'" + tablaRtns.Rows[n]["CODRET"].ToString() + "'," +
                    tablaRtns.Rows[n]["VALOR"] + "," + tablaRtns.Rows[n]["VALORBASE"] + ");");
                totalR += Convert.ToDouble(tablaRtns.Rows[n]["VALOR"]);
            }
            arlSql.Add("UPDATE MFACTURACLIENTE SET MFAC_VALORETE=MFAC_VALORETE+" + totalR + " " +
                "WHERE PDOC_CODIGO='" + pref + "' AND MFAC_NUMEDOCU=" + numFac + ";");
            
            return DBFunctions.Transaction(arlSql);
        }

		#endregion

		#region Ajax
		
		[Ajax.AjaxMethod]
		public string CambioPrefFC(string valor)
		{
			return DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+valor+"'");
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
	}
}
