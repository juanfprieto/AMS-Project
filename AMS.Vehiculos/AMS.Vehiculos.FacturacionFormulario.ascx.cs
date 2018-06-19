using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Contabilidad;
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class FacturacionFormulario : System.Web.UI.UserControl
	{
		#region Variables
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		////////////////////////
		protected DataTable dtInfoTec, dtInfoComer, dtInfCompras;
        protected DataTable dtInfoExcel, dtInfoExcelComer, dtInfoExcelTecn;
		protected string prefijoPed, numePed;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.RequiredFieldValidator validatorValRte;
		protected System.Web.UI.WebControls.RegularExpressionValidator validatorValRte2;
		protected DateTime FechaActual=DateTime.Now.Date;
        ProceHecEco contaOnline = new ProceHecEco();
        protected DatasToControls bind = new DatasToControls();

        #endregion

        #region Metodos

        protected void Mostrar_Tablas_Sesion()
		{
			int i=0;
			dtInfoTec = (DataTable)Session["tbInformacionTecnica"];
			dtInfoComer = (DataTable)Session["tbInformacionComercial"];
			dtInfCompras = (DataTable)Session["InfCompras"];
			if(dtInfCompras!=null && dtInfCompras.Rows.Count>0)
			{
				Label2.Visible=true;
				dgInfoVentas.DataSource = dtInfCompras;
				dgInfoVentas.DataBind();
			}
			dgInfoTec.DataSource = dtInfoTec;
			dgInfoComer.DataSource = dtInfoComer;
			dgInfoTec.DataBind();
			dgInfoComer.DataBind();
			//Session.Clear();
			Session["dtInfoTec"] = dtInfoTec;
			Session["dtInfoComer"] = dtInfoComer;
			double valorNetFact = 0, valorIvaFact = 0;
			for(i=0;i<dtInfoComer.Rows.Count;i++)
			{
				double porcIvaCat = 0;
                if (dtInfoComer.Rows[i][7].ToString()=="N")
                    porcIvaCat = Convert.ToDouble(DBFunctions.SingleData("SELECT piva_porciva FROM pcatalogovehiculo WHERE pcat_codigo='"+dtInfoComer.Rows[i][1].ToString()+"'"));
				valorNetFact += Convert.ToDouble(dtInfoComer.Rows[i][14]);
				valorIvaFact += Convert.ToDouble(dtInfoComer.Rows[i][14]) * (porcIvaCat/100);
                valorIvaFact = Math.Round(valorIvaFact * 1, 0);
			}
			valFac.Text = Utilidades.SepararPorComas(valorNetFact);
			tbValIVA.Text = Utilidades.SepararPorComas(valorIvaFact);
		}
		
		protected void MostrarVehiculosSinFacturar()
		{
			int i;
			DataSet vehFacts = new DataSet();
            DBFunctions.Request(vehFacts, IncludeSchema.NO, "SELECT mveh_inventario AS \"Número de Inventario\", pcat_codigo AS Catalogo, mv.mcat_vin AS VIN, mveh_valocomp AS VALOR, tcla_codigo as TIPOVEHI FROM mvehiculo mv, mcatalogovehiculo mc WHERE pdoc_codigopediprov='" + Request.QueryString["pref"] + "' AND mped_numero=" + Request.QueryString["num"] + " AND pdoc_codiordepago IS NULL AND mfac_numeordepago IS NULL AND MC.MCAT_VIN = MV.MCat_VIN");
			dtInfoComer = vehFacts.Tables[0];
			Session["dtInfoComer"] = dtInfoComer;
			lbInfo1.Visible = false;
			dgInfoComer.DataSource = dtInfoComer;
			dgInfoComer.DataBind();
			double valorNetoFact = 0, valorIvaFact = 0;
			for(i=0;i<dtInfoComer.Rows.Count;i++)
			{
                double porcIvaCat = 0;
                if (dtInfoComer.Rows[i][4].ToString() == "N")
                    porcIvaCat = Convert.ToDouble(DBFunctions.SingleData("SELECT piva_porciva FROM pcatalogovehiculo WHERE pcat_codigo='" + dtInfoComer.Rows[i][1].ToString() + "'"));
                else
                    porcIvaCat = 0; 
                valorNetoFact += Convert.ToDouble(dtInfoComer.Rows[i][3]);
				valorIvaFact += Convert.ToDouble(dtInfoComer.Rows[i][3]) * (porcIvaCat/100);
                valorIvaFact = Math.Round(valorIvaFact * 1, 0);
			}
			valFac.Text = Utilidades.SepararPorComas(valorNetoFact);
			tbValIVA.Text = Utilidades.SepararPorComas(valorIvaFact);
		}
		
		#endregion
		
		#region Eventos

        void btnRecargarRets_Click(object sender, EventArgs e)
        {
            PrecargarRetenciones();
        }

		protected void Page_Load(object sender, System.EventArgs e)
		{
            Button btnRecargarRets = new Button();
            btnRecargarRets.Text  = "Recargar Retenciones";
            btnRecargarRets.CausesValidation = false;
            btnRecargarRets.Click += new EventHandler(btnRecargarRets_Click);

            plcRets.Controls.Add(LoadControl(pathToControls+"AMS.Documentos.Retenciones.ascx"));
            plcRets.Controls.Add(new LiteralControl("<br>"));
            plcRets.Controls.Add(btnRecargarRets);
            if (!IsPostBack)
			{
                if(Request.QueryString["excel"] == "1")
                {
                    //&proc=1&ubi=" + ubicacion.SelectedValue + "&val=" + Convert.ToDouble(lbTotal.Text.Substring(1)).ToString() + "&nitProv=" + nitProveedor + "&excel=1"
                    //string v1 = Request.QueryString["excel"];
                    string v2 = Request.QueryString["ubi"];
                    string v3 = Request.QueryString["val"];
                    //string v4 = Request.QueryString["nitProv"];
                    txtInfoProovedor.Text = DBFunctions.SingleData("SELECT '[' CONCAT MNIT_NIT CONCAT '] ' CONCAT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + Request.QueryString["nitProv"] + "'"); //Request.QueryString["nitProv"];
                    txtInfoUbicacion.Text = DBFunctions.SingleData("SELECT '[' concat pubi_codigo concat '] ' concat pubi_nombre FROM pubicacion where pubi_codigo = '" + Request.QueryString["ubi"] + "'");
                    txtTotalFact.Text = Convert.ToDouble(Request.QueryString["val"]).ToString("C");

                    plInfoVeh.Visible = plInfoPed.Visible = infVehiculos.Visible = plcRets.Visible = fldRets.Visible = fldInfoFactura.Visible = fldInfoValorFactura.Visible = false;
                    btnAcpt.Visible = false;
                    btnAcptExcel.Visible = true;
                    DataTable miTablita = (DataTable)Session["infExcel"]; //esta session viene de recepcionFormulario
                    DataTable miTablita2 = (DataTable)Session["sessFacturas"]; //esta session viene de recepcionFormulario
                    if (miTablita == null || miTablita2 == null)
                    {
                        Utils.MostrarAlerta(Response, "Se ha generado un problema con la transferencia del proceso. Es posible que el servidor haya reiniciado el flujo de datos. Vuelva a realizar el proceso anterior con el que llegó hasta acá o contacte al Administrador del sistema");
                        btnAcptExcel.Enabled = false;
                    }
                    else
                    {
                        preparar_dtInfoExcel();
                        preparar_dtInfoExcelComer();
                        preparar_dtInfoExcelTecn();
                        int id = 1;

                        //string prefijo;
                        //ArrayList ultiDocu = DBFunctions.RequestAsCollection("SELECT PDOC_CODIGO, PDOC_ULTIDOCU FROM DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU = 'FP' ORDER BY PDOC_CODIGO;");
                        
                        //prefijo = ((Hashtable)ultiDocu[0])["PDOC_CODIGO"].ToString();

                        DataTable tablaCompleta = (DataTable)Session["tablaExcelCompleta"];//21 columnas
                        string tipoCompra = DBFunctions.SingleData("SELECT tcom_codigo FROM tcompravehiculo where tcom_codigo = 'P'");
                        for (int i = 0; i < miTablita.Rows.Count; i++)
                        {
                            DataRow fila = dtInfoExcel.NewRow();
                            fila[0] = id;
                            fila[1] = miTablita2.Rows[i]["PREFIJO"].ToString().Trim();//prefijo;
                            fila[2] = miTablita2.Rows[i]["PREFIJO FACTURA"].ToString().Trim();
                            fila[3] = miTablita2.Rows[i]["NUMERO FACTURA"].ToString().Trim();
                            fila[4] = miTablita.Rows[i]["VIN"].ToString();
                            fila[5] = miTablita2.Rows[i]["ALMACEN"].ToString().Trim();
                            fila[6] = miTablita2.Rows[i]["FECHA INGRESO"].ToString();
                            fila[7] = miTablita2.Rows[i]["FECHA VENCE"].ToString();
                            fila[8] = miTablita2.Rows[i]["VALOR FACTURA"].ToString();
                            fila[9] = "19.00";
                            dtInfoExcel.Rows.Add(fila);

                            DataRow filaTecn = dtInfoExcelTecn.NewRow();
                            filaTecn[0] = tablaCompleta.Rows[i]["CATALOGO"];
                            filaTecn[1] = tablaCompleta.Rows[i]["VIN"];
                            filaTecn[2] = tablaCompleta.Rows[i]["MOTOR"];
                            filaTecn[3] = tablaCompleta.Rows[i]["SERIE"];
                            filaTecn[4] = tablaCompleta.Rows[i]["CHASIS"];
                            filaTecn[5] = tablaCompleta.Rows[i]["COLOR"];
                            filaTecn[6] = tablaCompleta.Rows[i]["AÑO MODELO"];
                            filaTecn[7] = tablaCompleta.Rows[i]["TIPO SERVICIO"];
                            filaTecn[8] = miTablita.Rows[i]["PLACA"];
                            filaTecn[9] = tablaCompleta.Rows[i]["FECHA RECEPCION"].ToString();
                            dtInfoExcelTecn.Rows.Add(filaTecn);

                            DataRow filaComer = dtInfoExcelComer.NewRow();
                            filaComer[0] = miTablita.Rows[i]["NUM INVENTARIO"]; //numero inventario
                            filaComer[1] = tablaCompleta.Rows[i]["CATALOGO"]; //catalogo
                            filaComer[2] = tablaCompleta.Rows[i]["VIN"]; //vin
                            filaComer[3] = miTablita.Rows[i]["NUM INVENTARIO"]; //numero recepcion
                            filaComer[4] = DateTime.Now.GetDateTimeFormats()[5];
                            filaComer[5] = tablaCompleta.Rows[i]["FECHA DISPONIBLE"]; //fecha disponible
                            filaComer[6] = "0"; //kilometraje 0
                            filaComer[7] = "N"; //clase vehiculo N
                            filaComer[8] = tablaCompleta.Rows[i]["NUMERO MANIFIESTO"]; //numero manifiesto
                            filaComer[9] = tablaCompleta.Rows[i]["FECHA MANIFIESTO"]; //fecha manifiesto
                            filaComer[10] = tablaCompleta.Rows[i]["NUMERO ADUANA"]; //numero aduana
                            filaComer[11] = tablaCompleta.Rows[i]["NUMERO DO"]; //numero DO
                            filaComer[12] = tablaCompleta.Rows[i]["NUMERO LEVANTE"]; //numero levante
                            filaComer[13] = tipoCompra; //tipo compra
                            filaComer[14] = tablaCompleta.Rows[i]["VALOR"]; //valor
                            dtInfoExcelComer.Rows.Add(filaComer);
                            id++;

                            
                        }
                        dgFacturaExcel.DataSource = dtInfoExcel;
                        dgFacturaExcel.DataBind();
                        //int num = 9999;
                        //for (int i = 0; i < dgFacturaExcel.Items.Count; i++)
                        //{
                        //    ((PlaceHolder)dgFacturaExcel.Items[i].Cells[10].Controls[1]).Controls.Add(LoadControl(pathToControls + "AMS.Documentos.Retenciones.ascx"));
                        //    PrecargarRetencionesExcel(tablaCompleta.Rows[i]["VALOR"].ToString(), ((PlaceHolder)dgFacturaExcel.Items[i].Cells[10].Controls[1]), dtInfoExcel.Rows[i]["PREFIJO"].ToString(),num++);
                        //}

                        fldDataGrid.Visible = true;
                        Session["tblFact"] = dtInfoExcel;
                        Session["dtExcelComer"] = dtInfoExcelComer;
                        Session["dtExcelTecn"] = dtInfoExcelTecn;
                    }
                }
                else
                {
                    Label2.Visible = false;
                    plcRets.Visible = true;
                    plInfoPed.Visible = plInfoVeh.Visible = false;
                    prefijoPed = Request.QueryString["pref"];
                    numePed = Request.QueryString["num"];
                    if (Request.QueryString["cat"] != null && Request.QueryString["vin"] != null)
                    {
                        catVeh.Text = Request.QueryString["cat"].Trim();
                        vinVeh.Text = Request.QueryString["vin"];
                    }
                    if (prefijoPed != "" && numePed != "")
                        plInfoPed.Visible = true;
                    //Cargamos el prefijo y el numero del pedido a proveedor
                    prefPed.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='" + Request.QueryString["pref"] + "'");
                    numPed.Text = Request.QueryString["num"];
                    nitProveedor.Text = Request.QueryString["nitProv"];
                    bind.PutDatasIntoDropDownList(prefOrdPago, "SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FP' and PH.tpro_proceso in ('VN','VU') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                    bind.PutDatasIntoDropDownList(almacen, "SELECT palm_almacen, palm_descripcion FROM palmacen where (pcen_centvvn is not null  or pcen_centvvu is not null) and TVIG_VIGENCIA = 'V' order by palm_descripcion;");
                    bind.PutDatasIntoDropDownList(ddlIvaFlt, "SELECT piva_porciva,piva_decreto FROM piva");
                    numOrdPago.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu +1 FROM pdocumento WHERE pdoc_codigo='" + prefOrdPago.SelectedValue + "'");
                    fechFac.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    //tipos de proceso : 1-recepción con facturación 2-facturación con previa recepción 3-Usados????????

                    if (prefOrdPago.Items.Count == 0) Utils.MostrarAlerta(Response, "No ha configurado un prefijo para la Orden de Pago");
                    if (almacen.Items.Count == 0) Utils.MostrarAlerta(Response, "No ha configurado un almacén");
                    if (ddlIvaFlt.Items.Count == 0) Utils.MostrarAlerta(Response, "No ha configurado un iva");

                    if (prefOrdPago.Items.Count == 0 || almacen.Items.Count == 0 || ddlIvaFlt.Items.Count == 0) return;

                    if (Request.QueryString["proc"] == "1")
                    {
                        plInfoVeh.Visible = true;
                        Mostrar_Tablas_Sesion();
                        PrecargarRetenciones();
                    }
                    else if (Request.QueryString["proc"] == "2")
                    {
                        plInfoVeh.Visible = true;
                        infVehiculos.Visible = false;
                        //catVeh.Text   = Request.QueryString["cat"].Trim().Replace(" ","+");
                        catVeh.Text = Request.QueryString["cat"].Trim();
                        vinVeh.Text = Request.QueryString["vin"];
                        obsr.Text += catVeh.Text + "-" + vinVeh.Text + " ";
                        DataSet datosVehic = new DataSet();
                        datosVehic = DBFunctions.Request(datosVehic, IncludeSchema.NO, "SELECT tcla_codigo as CLASE, coalesce(mveh_valocomp,0) as COSTO, mpro_nit as PROV, C.PCAT_CODIGO FROM Mvehiculo V, MCATALOGOVEHICULO C WHERE V.MCAT_vin ='" + vinVeh.Text + "' AND V.MCAT_vin = C.MCAT_vin ");
                        //    ArrayList datosVehic =  DBFunctions.RequestAsCollection("SELECT tcla_codigo as CLASE, coalesce(mveh_valocomp,0) as COSTO, mpro_nit as PROV FROM Mvehiculo WHERE MCAT_vin ='" + vinVeh.Text + "'");

                        double porcIVACat = 0;
                        if (datosVehic.Tables[0].Rows[0][0].ToString() == "N")  // solo para los NUEVOS aplica IVA
                        {
                            catVeh.Text = datosVehic.Tables[0].Rows[0][3].ToString();
                            porcIVACat = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(piva_porciva,19.00) FROM pcatalogovehiculo WHERE pcat_codigo='" + catVeh.Text + "'"));
                        }
                        valFac.Text = Utilidades.SepararPorComas(Convert.ToDouble(datosVehic.Tables[0].Rows[0][1].ToString()));
                        double ValIVAv = (Convert.ToDouble(valFac.Text) * (porcIVACat / 100));
                        //     tbValIVA.Text   = Utilidades.SepararPorComas(Convert.ToDouble(DBFunctions.SingleData("SELECT mveh_valocomp FROM mvehiculo WHERE pcat_codigo='" + Request.QueryString["cat"] + "' AND mcat_vin='" + Request.QueryString["vin"] + "'")) * (porcIVACat / 100));
                        ValIVAv = Math.Round(ValIVAv * 1, 0);
                        tbValIVA.Text = Utilidades.SepararPorComas(ValIVAv);
                        nitProveedor.Text = datosVehic.Tables[0].Rows[0][2].ToString();
                        valFac.ReadOnly = false;
                        //		valFac.Attributes.Add("onKeyUp","CalcularIva("+valFac.ClientID+",'"+Convert.ToDouble(DBFunctions.SingleData("SELECT piva_porciva FROM pcatalogovehiculo WHERE pcat_codigo='"+Request.QueryString["cat"]+"'")).ToString()+"')");
                        valFac.Attributes.Add("onKeyUp", "CalcularIva(" + valFac.ClientID + ",'" + porcIVACat.ToString() + "')");
                        btnAcpt.Attributes.Add("onClick", "if (typeof(Page_ClientValidate) == 'function'){ if (Page_ClientValidate() == true) return confirm('Esta seguro de realizar esta operación con los valores ingresados?');}");
                        PrecargarRetenciones();
                    }
                    else if (Request.QueryString["proc"] == "3")
                    {
                            plInfoVeh.Visible = true;
                            nitProveedor.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculoproveedor WHERE pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["num"] + "");
                            MostrarVehiculosSinFacturar();
                            PrecargarRetenciones();
                    }
                    string diasplazo = DBFunctions.SingleData("Select COALESCE(mpro_diasplazo,0) from DBXSCHEMA.MPROVEEDOR where mnit_nit='" + nitProveedor.Text + "' ");
                    fechIng.Text = FechaActual.ToString("yyyy-MM-dd");

                    if (Session["Retoma"].ToString() == "S")
                    {
                        fechVenc.Text = fechIng.Text;
                        prefFacProv.Text = prefijoPed.ToString();
                        numFacProv.Text = numePed;
                    }
                    else
                    {
                        if (diasplazo != String.Empty)
                        {
                            fechVenc.Text = FechaActual.AddDays(double.Parse(diasplazo)).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            fechVenc.Text = "";
                        }
                    }
                }
            }

            if (Session["dtInfoTec"]==null)
				dtInfoTec = new DataTable();
			else
				dtInfoTec = (DataTable)Session["dtInfoTec"];
			if(Session["dtInfoComer"]==null)
				dtInfoComer = new DataTable();
			else
				dtInfoComer = (DataTable)Session["dtInfoComer"];
			if (Session["InfCompras"]==null)
				dtInfCompras = new DataTable();
			else
				dtInfCompras = (DataTable)Session["InfCompras"];
            if (Session["tblFact"] == null)
                preparar_dtInfoExcel();
            else
                dtInfoExcel = (DataTable)Session["tblFact"];
            if (Session["dtExcelComer"] == null)
                preparar_dtInfoExcelComer();
            else
                dtInfoExcelComer = (DataTable)Session["dtExcelComer"];
            if (Session["dtExcelTecn"] == null)
                preparar_dtInfoExcelTecn();
            else
                dtInfoExcelTecn = (DataTable)Session["dtExcelTecn"];

        }

        protected int validarIncremental()
        {
            int rta = 0;

            return rta;
        }

        private void PrecargarRetenciones()
        {
            ArrayList arlValores = new ArrayList(), arlTipos = new ArrayList(), arlBases = new ArrayList();
            Retenciones ctrRetenciones = (Retenciones)plcRets.Controls[0];
            double valorFactura, valorIva, valorFletes, valorIvaFletes;
            try { valorFactura = Convert.ToDouble(valFac.Text); }
            catch { valorFactura = 0; }
            try { valorIva = Convert.ToDouble(tbValIVA.Text); }
            catch { valorIva = 0; }
            try { valorFletes = Convert.ToDouble(valFlt.Text); }
            catch { valorFletes = 0; }
            try { valorIvaFletes = (Convert.ToDouble(valFlt.Text) * (Convert.ToDouble(ddlIvaFlt.SelectedValue.Trim()) / 100)); }
            catch { valorIvaFletes = 0; }
            Retencion retencion = new Retencion(
                nitProveedor.Text.ToString(),
                prefOrdPago.SelectedValue.ToString(),
                Convert.ToInt32(numOrdPago.Text),
                (valorFactura + valorFletes),
                (valorIva + valorIvaFletes),
                "V",false);
            
            retencion.Guardar_Retenciones(false, ref arlValores, ref arlBases, ref arlTipos);
            ctrRetenciones.PrecargarRetenciones(nitProveedor.Text, "FP", arlValores, arlTipos, arlBases);
        }

        protected void dgExcelDataBound(object sender, DataGridCommandEventArgs e)
        {
            /*
                dgFacturaExcel.DataSource = dtInfoExcel;
                DataBind();
                ((TextBox)e.Item.Cells[10].Controls[1]).Text = ((TextBox)e.Item.Cells[10].Controls[1]).Text;
            */
            //bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].Controls[1]), "SELECT tcla_clase, tcla_nombre FROM tclasevehiculo ORDER BY tcla_clase");
            //if (Session["Retoma"].ToString() == "S")
            //    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[12].Controls[1]), "SELECT tcom_codigo,tcom_tipocompra FROM tcompravehiculo where tcom_codigo = 'R' ORDER BY tcom_tipocompra");
            //else
            //    bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[12].Controls[1]), "SELECT tcom_codigo,tcom_tipocompra FROM tcompravehiculo where tcom_codigo <> 'R' ORDER BY tcom_tipocompra");


            ////((TextBox)e.Item.Cells[2].Controls[1]).Text = (DBFunctions.SingleData("SELECT MAX(mveh_inventario) FROM mvehiculo")).ToString();
            //((TextBox)e.Item.Cells[3].Controls[1]).Text = FechaActual.ToString("yyyy-MM-dd");
            //((TextBox)e.Item.Cells[4].Controls[1]).Text = FechaActual.ToString("yyyy-MM-dd");
            //((TextBox)e.Item.Cells[5].Controls[1]).Text = "0";
            //if (((Button)e.CommandSource).CommandName == "Edit")
            //{
                //string ala = ((DropDownList)e.Item.Cells[1].FindControl("ddlPref")).SelectedValue;
                //bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].FindControl("ddlPref")), "SELECT PDOC_CODIGO, PDOC_CODIGO FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'FP' ORDER BY PDOC_CODIGO;");
                //bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].FindControl("ddlAlmacen")), "SELECT PALM_ALMACEN, PALM_DESCRIPCION FROM PALMACEN WHERE TALM_TIPOALMA = 'A';");
                //bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].FindControl("ddlIva")), "SELECT PIVA_PORCIVA, PIVA_DECRETO FROM PIVA;");
            //}
            //DataBind();
        }

        public void DgInserts_Cancel(Object sender, DataGridCommandEventArgs e)
        {
            dgFacturaExcel.EditItemIndex = -1;
            dgFacturaExcel.DataSource = dtInfoExcel;
            DataBind();
        }

        public void DgInserts_Edit(Object sender, DataGridCommandEventArgs e)
        {
            dgFacturaExcel.EditItemIndex = (int)e.Item.ItemIndex; //activa los campos que están en modo edición (EditItemTemplate).
            dgFacturaExcel.DataSource = dtInfoExcel;
            DataBind();
            
            //bind.PutDatasIntoDropDownList(((DropDownList)dgFacturaExcel.Items[dgFacturaExcel.EditItemIndex].Cells[1].Controls[1]), "SELECT PDOC_CODIGO, PDOC_CODIGO FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'FP' ORDER BY PDOC_CODIGO;");
            
            bind.PutDatasIntoDropDownList(((DropDownList)dgFacturaExcel.Items[dgFacturaExcel.EditItemIndex].Cells[9].Controls[1]), "SELECT PIVA_PORCIVA, PIVA_DECRETO FROM PIVA;");
            //((DropDownList)dgFacturaExcel.Items[dgFacturaExcel.EditItemIndex].Cells[1].Controls[1]).SelectedValue = dtInfoExcel.Rows[dgFacturaExcel.EditItemIndex]["PREFIJO"].ToString();
            
            ((DropDownList)dgFacturaExcel.Items[dgFacturaExcel.EditItemIndex].Cells[9].Controls[1]).SelectedValue = dtInfoExcel.Rows[dgFacturaExcel.EditItemIndex]["IVA"].ToString();
            
        }

        public void DgInserts_Update(Object sender, DataGridCommandEventArgs e)
        {
            string /*prefijo,*/ iva;
            //prefijo = ((DropDownList)dgFacturaExcel.Items[dgFacturaExcel.EditItemIndex].Cells[1].Controls[1]).SelectedValue;
            iva = ((DropDownList)dgFacturaExcel.Items[dgFacturaExcel.EditItemIndex].Cells[9].Controls[1]).SelectedValue;

            //dtInfoExcel.Rows[dgFacturaExcel.EditItemIndex]["PREFIJO"] = prefijo;
            dtInfoExcel.Rows[dgFacturaExcel.EditItemIndex]["IVA"] = iva;
            
            dgFacturaExcel.EditItemIndex = -1;
            dgFacturaExcel.DataSource = dtInfoExcel;
            DataBind();
            Session["tblFact"] = dtInfoExcel;
        }

        protected void preparar_dtInfoExcel()
        {
            dtInfoExcel = new DataTable();
            dtInfoExcel.Columns.Add(new DataColumn("ID", System.Type.GetType("System.Int32")));
            dtInfoExcel.Columns.Add(new DataColumn("PREFIJO", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("PREFIJO FACTURA", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("NUMERO FACTURA", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("VIN", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("ALMACEN", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("FECHA ENTRADA", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("FECHA VENCE", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
            dtInfoExcel.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.String")));
        }
        protected void preparar_dtInfoExcelComer()
        {
            dtInfoExcelComer = new DataTable();
            dtInfoExcelComer.Columns.Add(new DataColumn("NUMERO INVENTARIO", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("CATALOGO", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("VIN", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("NUMERO RECEPCION", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("FECHA RECEPCION", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("FECHA DISPONIBLE", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("KILOMETRAJE RECEPCION", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("CLASE VEHICULO", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("NUMERO MANIFIESTO", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("FECHA MANIFIESTO", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("NUMERO ADUANA", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("NUMERO DO", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("NUMERO LEVANTE", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("TIPO COMPRA", System.Type.GetType("System.String")));
            dtInfoExcelComer.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
        }
        protected void preparar_dtInfoExcelTecn()
        {
            dtInfoExcelTecn = new DataTable();
            dtInfoExcelTecn.Columns.Add(new DataColumn("CATALOGO", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("VIN", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("MOTOR", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("SERIE", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("CHASIS", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("COLOR", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("AÑO MODELO", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("TIPO SERVICIO", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("PLACA", System.Type.GetType("System.String")));
            dtInfoExcelTecn.Columns.Add(new DataColumn("FECHA_RECE", System.Type.GetType("System.String")));
        }

        protected void Aceptar_Facturacion(Object Sender, EventArgs e)
        {
            //Validacion NitProveedor-prefProveedor-NumProveedor
            if (DBFunctions.RecordExist("select pdoc_codiordepago from dbxschema.mfacturaproveedor where (mfac_prefdocu='"
                +prefFacProv.Text+"' or mfac_prefdocu='"+prefFacProv.Text.ToUpper()+"' or mfac_prefdocu='"+prefFacProv.Text.ToLower()+
                "') AND mfac_numedocu="+numFacProv.Text+" and mnit_nit='"+nitProveedor.Text+"'"))
			{
               Utils.MostrarAlerta(Response, "El prefijo y número ingresado de la factura proveedor para el Nit: " + nitProveedor.Text + " ya existe en el sistema. ");
				return;
			}

            if (prefOrdPago.SelectedValue.ToString().Equals(""))
            {
                Utils.MostrarAlerta(Response, "No ha seleccionado un prefijo para la Orden de Pago");
                return;
            }
            if (almacen.SelectedValue.ToString().Equals(""))
            {
                Utils.MostrarAlerta(Response, "No ha seleccionado un almacén");
                return;
            }
            if (ddlIvaFlt.SelectedValue.ToString().Equals(""))
            {
                Utils.MostrarAlerta(Response, "No ha seleccionado un iva");
                return;
            }
            if (fechFac.Text.Trim().Equals("") || fechVenc.Text.Trim().Equals(""))
            {
                Utils.MostrarAlerta(Response, "Fecha Factura o Vencimiento Errada");
                return;
            }
                      
         	double porcentajeIva = Convert.ToDouble(ddlIvaFlt.SelectedValue.Trim());
			//Debemos revisar si las fechas son validas o no
			DateTime fechaFactura = Convert.ToDateTime(fechFac.Text.Trim());
			DateTime fechaVencimiento = Convert.ToDateTime(fechVenc.Text.Trim());
			if(fechaVencimiento < fechaFactura)
			{
                Utils.MostrarAlerta(Response, "La fecha de Vencimiento es menor " + fechVenc.Text.Trim() + " que la fecha de factura " + fechFac.Text.Trim() + ". Revise Por Favor");
				return;
			}
            string anoyMes = DBFunctions.SingleData("SELECT PANO_ANO concat '-' concat PMES_MES FROM CVEHICULOS");
            string[] anoMes = anoyMes.Split('-');
            if (fechaFactura.Year.ToString() != anoMes[0].ToString() || fechaFactura.Month.ToString() != anoMes[1].ToString())
            {
                Utils.MostrarAlerta(Response, "La fecha de la factura "+ fechaFactura.Year.ToString() +" - "+ fechaFactura.Month.ToString() +"  es diferente a la vigencia de Vehículos " + anoyMes + ". Revise Por Favor");
                return;
            }

			Retenciones docRets=(Retenciones)plcRets.Controls[0];
			//Si se va a realizar la facturacion de un pedido recien ingresado
            if (prefFacProv.Text == "" || numFacProv.Text == "" || fechFac.Text == "")
            {
                Utils.MostrarAlerta(Response, "El prefijo, numero o fecha de Factura del Proveedor está errado. Revise Por Favor");
                return;
            }
                        
            btnAcpt.Enabled = false;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append ("<script language=javascript>btnAcpt.Enabled = false;</script>");
            this.RegisterStartupScript("Startup", sb.ToString());
            /*
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language=Javascript>\n");
            sb.Append("function x() { \n");
            sb.Append("....");
            sb.Append("</script>");
                
            Page.RegisterStartupScript("script",sb.ToString()) ;
             */

            if (Request.QueryString["proc"]=="1")
			{
				//Creamos el objeto de recepcion
                string indicativoRetoma = "N";
                if (Session["Retoma"].ToString() == "S")
                    indicativoRetoma = "S";
                Recepcion miRecepcionFacturacion = new Recepcion(dtInfoTec, dtInfoComer, Request.QueryString["pref"], Request.QueryString["num"], Request.QueryString["ubi"], nitProveedor.Text, indicativoRetoma);
				//Ahora a este objeto le configuramos las propiedades para la facturacion
				miRecepcionFacturacion.PrefijoOrdenPago = prefOrdPago.SelectedValue;
				miRecepcionFacturacion.NumeroOrdenPago  = numOrdPago.Text;
				miRecepcionFacturacion.NitProveedor     = nitProveedor.Text;
                miRecepcionFacturacion.NitProveedor2 = Request.QueryString["nitProv"];
				miRecepcionFacturacion.PrefijoFacturaProveedor = prefFacProv.Text;
				miRecepcionFacturacion.NumeroFacturaProveedor = numFacProv.Text;
				miRecepcionFacturacion.FechaFactura     = fechFac.Text;
				miRecepcionFacturacion.Almacen          = almacen.SelectedValue;
				miRecepcionFacturacion.FechaIngreso     = fechIng.Text;
				miRecepcionFacturacion.FechaVencimiento = fechVenc.Text;
				miRecepcionFacturacion.FechaUltimoPago  = "";
				miRecepcionFacturacion.EstadoFacturaProveedor = "V";
				try{miRecepcionFacturacion.ValorFactura = Convert.ToDouble(valFac.Text).ToString();}
				catch{miRecepcionFacturacion.ValorFactura = "0";}
				try{miRecepcionFacturacion.ValorIva     = Convert.ToDouble(tbValIVA.Text).ToString();}
				catch{miRecepcionFacturacion.ValorIva   = "0";}
				miRecepcionFacturacion.ValorRetencion   = "0";
				try{miRecepcionFacturacion.ValorFletes  = Convert.ToDouble(valFlt.Text).ToString();}
				catch{miRecepcionFacturacion.ValorFletes = "0";}
				try{miRecepcionFacturacion.ValorIvaFletes = (Convert.ToDouble(valFlt.Text)*(porcentajeIva/100)).ToString();}
				catch{miRecepcionFacturacion.ValorIvaFletes = "0";}
				miRecepcionFacturacion.Observacion      = obsr.Text;
				miRecepcionFacturacion.Usuario          = HttpContext.Current.User.Identity.Name.ToLower();
                miRecepcionFacturacion.dtRetenciones    = (DataTable)ViewState["TABLERETS"];
               	if(miRecepcionFacturacion.Realizar_Recepcion(true,true))
				{
                    contaOnline.contabilizarOnline(miRecepcionFacturacion.PrefijoOrdenPago.ToString(), Convert.ToInt32(miRecepcionFacturacion.NumeroOrdenPago.ToString()), Convert.ToDateTime(fechFac.Text), "");
                    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturaProveedor&facOpc=0&prefFP=" + miRecepcionFacturacion.PrefijoOrdenPago + "&numFP=" + miRecepcionFacturacion.NumeroOrdenPago);
					//lb.Text += miRecepcionFacturacion.ProcessMsg;

				}
				else
					lb.Text += "Se generó un error al realizar la Recepción: " + miRecepcionFacturacion.ProcessMsg;
				////////////////////////////////////////////////////
			}
			else if(Request.QueryString["proc"]=="2")
			{
				//valFac.Text=Convert.ToDouble(valFac.Text).ToString();
				Recepcion miFacturacion                 = new Recepcion();
				//Ahora a este objeto le configuramos las propiedades para la facturacion
				miFacturacion.PrefijoOrdenPago          = prefOrdPago.SelectedValue;
				miFacturacion.NumeroOrdenPago           = numOrdPago.Text;
				miFacturacion.NitProveedor              = nitProveedor.Text;
				miFacturacion.PrefijoFacturaProveedor   = prefFacProv.Text;
				miFacturacion.NumeroFacturaProveedor    = numFacProv.Text;
				miFacturacion.FechaFactura              = fechFac.Text;
				miFacturacion.Almacen                   = almacen.SelectedValue;
				miFacturacion.FechaIngreso              = fechIng.Text;
				miFacturacion.FechaVencimiento          = fechVenc.Text;
				miFacturacion.FechaUltimoPago           = "";
				miFacturacion.EstadoFacturaProveedor    = "V";
				try{miFacturacion.ValorFactura          = Convert.ToDouble(valFac.Text).ToString();}
				catch{miFacturacion.ValorFactura        = "0";}
				try{miFacturacion.ValorIva              = Convert.ToDouble(tbValIVA.Text).ToString();}
				catch{miFacturacion.ValorIva            = "0";}
				miFacturacion.ValorRetencion            = "0";
				try{miFacturacion.ValorFletes           = Convert.ToDouble(valFlt.Text).ToString();}
				catch{miFacturacion.ValorFletes         = "0";}
				try{miFacturacion.ValorIvaFletes        = (Convert.ToDouble(valFlt.Text)*(porcentajeIva/100)).ToString();}
				catch{miFacturacion.ValorIvaFletes      = "0";}
				miFacturacion.Observacion               = obsr.Text;
				miFacturacion.Usuario                   = HttpContext.Current.User.Identity.Name.ToLower();
				miFacturacion.dtRetenciones             = docRets.tablaRtns;
              	if(miFacturacion.Facturar_Vehiculo_Recibido(Request.QueryString["cat"],Request.QueryString["vin"]))
				{
                    contaOnline.contabilizarOnline(miFacturacion.PrefijoOrdenPago.ToString(), Convert.ToInt32(miFacturacion.NumeroOrdenPago.ToString()), Convert.ToDateTime(fechIng.Text), "");
                    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturaProveedor&facOpc=1&prefFP=" + miFacturacion.PrefijoOrdenPago + "&numFP=" + miFacturacion.NumeroOrdenPago);
					//lb.Text += miFacturacion.ProcessMsg;
				}
				else
					lb.Text += miFacturacion.ProcessMsg;
			}
			else if(Request.QueryString["proc"]=="3")
			{
				Recepcion miFacturacion = new Recepcion();
				//Ahora a este objeto le configuramos las propiedades para la facturacion
				miFacturacion.PrefijoOrdenPago   = prefOrdPago.SelectedValue;
				miFacturacion.NumeroOrdenPago    = numOrdPago.Text;
				miFacturacion.NitProveedor       = nitProveedor.Text;
				miFacturacion.PrefijoFacturaProveedor = prefFacProv.Text;
				miFacturacion.NumeroFacturaProveedor = numFacProv.Text;
				miFacturacion.FechaFactura       = fechFac.Text;
				miFacturacion.Almacen            = almacen.SelectedValue;
				miFacturacion.FechaIngreso       = fechIng.Text;
				miFacturacion.FechaVencimiento   = fechVenc.Text;
				miFacturacion.FechaUltimoPago    = "";
				miFacturacion.EstadoFacturaProveedor = "V";
				try{miFacturacion.ValorFactura   = Convert.ToDouble(valFac.Text).ToString();}
				catch{miFacturacion.ValorFactura = "0";}
                try { miFacturacion.ValorIva     = Convert.ToDouble(tbValIVA.Text).ToString(); }
				catch{miFacturacion.ValorIva     = "0";}
              	miFacturacion.ValorRetencion     = "0";
				try{miFacturacion.ValorFletes    = Convert.ToDouble(valFlt.Text).ToString();}
				catch{miFacturacion.ValorFletes  = "0";}
				try{miFacturacion.ValorIvaFletes = (Convert.ToDouble(valFlt.Text)*(porcentajeIva/100)).ToString();}
				catch{miFacturacion.ValorIvaFletes = "0";}
                miFacturacion.Observacion        = obsr.Text;
				miFacturacion.Usuario            = HttpContext.Current.User.Identity.Name.ToLower();
				miFacturacion.dtRetenciones      = docRets.tablaRtns;
              	if(miFacturacion.Facturar_Vehiculo_Recibido(dtInfoComer))
				{
                    contaOnline.contabilizarOnline(miFacturacion.PrefijoOrdenPago.ToString(), Convert.ToInt32(miFacturacion.NumeroOrdenPago.ToString()), Convert.ToDateTime(fechIng.Text), "");
                    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturaProveedor&facOpc=1&prefFP="+miFacturacion.PrefijoOrdenPago+"&numFP="+miFacturacion.NumeroOrdenPago);
					//lb.Text += miFacturacion.ProcessMsg;
				}
				else
					lb.Text += miFacturacion.ProcessMsg;
			}
		}

        protected void Aceptar_Facturacion_Excel(Object Sender, EventArgs e)
        {
           lb.Text = "";
            DateTime fechaFactura, fechaVencimiento, fechaRecepcion; // = Convert.ToDateTime(fechVenc.Text.Trim());
            string anoyMes = DBFunctions.SingleData("SELECT PANO_ANO concat '-' concat PMES_MES FROM CVEHICULOS");
            string[] anoMes = anoyMes.Split('-');
            int contadorError = 0;
            string nitExcel = "";
            string vinExcel = "";
            string prefFact = "";
            int numeFact    = 0;
            for (int i = 0; i < dtInfoExcel.Rows.Count; i++)
            {
                fechaFactura = Convert.ToDateTime(dtInfoExcel.Rows[i]["FECHA ENTRADA"].ToString().Trim());
                fechaVencimiento = Convert.ToDateTime(dtInfoExcel.Rows[i]["FECHA VENCE"].ToString().Trim());
                fechaRecepcion = Convert.ToDateTime(dtInfoExcelTecn.Rows[i]["FECHA_RECE"].ToString().Trim());
                vinExcel = dtInfoExcelTecn.Rows[i]["VIN"].ToString().Trim();
                nitExcel = Request.QueryString["nitProv"].Trim();
                prefFact = dtInfoExcel.Rows[i]["PREFIJO FACTURA"].ToString().Trim();
                numeFact = Convert.ToInt32(dtInfoExcel.Rows[i]["NUMERO FACTURA"].ToString().Trim());


                if (fechaVencimiento < fechaFactura)
                {
                    lb.Text += "La fecha de Vencimiento: " + fechaVencimiento.GetDateTimeFormats()[5] + " es menor que la fecha de factura: " + fechaFactura.GetDateTimeFormats()[5] + " Fila id: " + dtInfoExcel.Rows[i]["ID"] + ". Revise Por Favor" + "<br />";
                    contadorError++;
                }
                if (fechaFactura.Year.ToString() != anoMes[0].ToString() || fechaFactura.Month.ToString() != anoMes[1].ToString())
                {
                    lb.Text += "La fecha de la factura " + fechaFactura.Year.ToString() + " - " + fechaFactura.Month.ToString() + ", Fila id: " + dtInfoExcel.Rows[i]["ID"] + "  es diferente a la vigencia de Vehículos " + anoyMes + ". Revise Por Favor." + "<br />";
                    contadorError++;
                }
                if(fechaRecepcion < fechaFactura)
                {
                    lb.Text += "La fecha de recepción " + fechaRecepcion.Year.ToString() + " - " + fechaRecepcion.Month.ToString() + ", Fila id: " + dtInfoExcel.Rows[i]["ID"] + "  es menor a la fecha de entrada" + "<br />";
                    contadorError++;
                }
                if (DBFunctions.RecordExist("SELECT MCAT_VIN FROM MVEHICULO WHERE MCAT_VIN = '"+vinExcel+ "' and test_tipoesta < 60"))
                {
                    lb.Text += "El VIN " + vinExcel + "  -  YA está registrado en el sistema , Fila id: " + dtInfoExcel.Rows[i]["ID"] + "  Vehículo NO aceptado" + "<br />";
                    contadorError++;
                }
                if (DBFunctions.RecordExist("SELECT mfac_numeordepago FROM MFACTURAPROVEEDOR WHERE MNIT_NIT = '" + nitExcel + "' and mfac_prefdocu = '" + prefFact + "' and mfac_numedocu = " + numeFact + " "))
                {
                    lb.Text += "La factura del proveedor " + nitExcel + " PREFIJO FACTURA " + prefFact + " NUMERO FACTURA " + numeFact + "  -  YA está registrada en el sistema , Fila id: " + dtInfoExcel.Rows[i]["ID"] + "  Vehículo NO aceptado" + "<br />";
                    contadorError++;
                }
            }
            if(contadorError > 0)
            {
                btnAcptExcel.Enabled = false;
                btnVolverPrincipal.Visible = true;
                return;
            }
            else
            {
                //ArrayList ultiDocu = DBFunctions.RequestAsCollection("SELECT PDOC_CODIGO, PDOC_ULTIDOCU FROM DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU = 'FP' ORDER BY PDOC_CODIGO;");
                int num;
                DataTable tableDocus = DBFunctions.Request(new DataSet(), IncludeSchema.NO, "SELECT PDOC_CODIGO, PDOC_ULTIDOCU, SFOR_CODIGO FROM DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU = 'FP' ORDER BY PDOC_CODIGO;").Tables[0];
                //int numero;
                
                for (int i = 0; i < dgFacturaExcel.Items.Count; i++)
                {
                    num = validarNumero(dtInfoExcel.Rows[i]["PREFIJO"].ToString(), Convert.ToInt32(tableDocus.Select("PDOC_CODIGO = '" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + "'")[0].ItemArray[1].ToString()));//Convert.ToInt32(DBFunctions.SingleData("SELECT PDOC_ULTIDOCU FROM DBXSCHEMA.PDOCUMENTO WHERE PDOC_CODIGO = '" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + "'")));
                    //num = Convert.ToInt32(tableDocus.Select("PDOC_CODIGO = '" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + "'")[0].ItemArray[1]) + 1;

                    double porcentajeIva = Convert.ToDouble(dtInfoExcel.Rows[i]["IVA"]);
                    fechaFactura = Convert.ToDateTime(dtInfoExcel.Rows[i]["FECHA ENTRADA"].ToString().Trim());
                    fechaVencimiento = Convert.ToDateTime(dtInfoExcel.Rows[i]["FECHA VENCE"].ToString().Trim());
                    
                    if (Request.QueryString["proc"] == "1")
                    {
                        //verificación número de orden
                        //Creamos el objeto de recepcion
                        string indicativoRetoma = "N";
                        if (Session["Retoma"].ToString() == "S")
                            indicativoRetoma = "S";
                        Recepcion miRecepcionFacturacion = new Recepcion(dtInfoExcelTecn.Rows[i], dtInfoExcelComer.Rows[i], dtInfoExcel.Rows[i]["PREFIJO"].ToString(), num.ToString(), Request.QueryString["ubi"], Request.QueryString["nitProv"], indicativoRetoma);
                        //Ahora a este objeto le configuramos las propiedades para la facturacion
                        miRecepcionFacturacion.PrefijoOrdenPago = dtInfoExcel.Rows[i]["PREFIJO"].ToString();
                        miRecepcionFacturacion.NumeroOrdenPago = num.ToString(); //VALIDACION NUMERO
                        miRecepcionFacturacion.NitProveedor = Request.QueryString["nitProv"];
                        miRecepcionFacturacion.NitProveedor2 = Request.QueryString["nitProv"];
                        miRecepcionFacturacion.PrefijoFacturaProveedor = dtInfoExcel.Rows[i]["PREFIJO FACTURA"].ToString();
                        miRecepcionFacturacion.NumeroFacturaProveedor = dtInfoExcel.Rows[i]["NUMERO FACTURA"].ToString();
                        //miRecepcionFacturacion.FechaFactura = DateTime.Now.GetDateTimeFormats()[5];
                        miRecepcionFacturacion.FechaFactura = dtInfoExcelTecn.Rows[i]["FECHA_RECE"].ToString();
                        miRecepcionFacturacion.Almacen = dtInfoExcel.Rows[i]["ALMACEN"].ToString();
                        miRecepcionFacturacion.FechaIngreso = dtInfoExcel.Rows[i]["FECHA ENTRADA"].ToString();
                        miRecepcionFacturacion.FechaVencimiento = dtInfoExcel.Rows[i]["FECHA VENCE"].ToString();
                        miRecepcionFacturacion.FechaUltimoPago = "";
                        miRecepcionFacturacion.EstadoFacturaProveedor = "V";
                        try { miRecepcionFacturacion.ValorFactura = Convert.ToDouble(dtInfoExcel.Rows[i]["VALOR"]).ToString(); }
                        catch { miRecepcionFacturacion.ValorFactura = "0"; }
                        try { miRecepcionFacturacion.ValorIva = (Convert.ToDouble(dtInfoExcel.Rows[i]["VALOR"]) * porcentajeIva * 0.01).ToString(); }
                        catch { miRecepcionFacturacion.ValorIva = "0"; }
                        miRecepcionFacturacion.ValorRetencion = "0";
                        try { miRecepcionFacturacion.ValorFletes = Convert.ToDouble(valFlt.Text).ToString(); }
                        catch { miRecepcionFacturacion.ValorFletes = "0"; }
                        try { miRecepcionFacturacion.ValorIvaFletes = (Convert.ToDouble(valFlt.Text) * (porcentajeIva / 100)).ToString(); }
                        catch { miRecepcionFacturacion.ValorIvaFletes = "0"; }
                        miRecepcionFacturacion.Observacion = dtInfoExcelTecn.Rows[i]["CATALOGO"].ToString() + " - " + dtInfoExcelTecn.Rows[i]["VIN"].ToString();
                        miRecepcionFacturacion.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
                        miRecepcionFacturacion.dtRetenciones = (DataTable)ViewState["TABLERETS"];
                       
                        if (miRecepcionFacturacion.Realizar_Recepcion_Excel(true, true))
                        {
                            lb.Text += "<font color='green' size='4px'> Se ha generado la factura con prefijo [" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + " - " + num.ToString() + "] correctamente" + " </font> <br />";
                            contaOnline.contabilizarOnline(miRecepcionFacturacion.PrefijoOrdenPago.ToString(), Convert.ToInt32(miRecepcionFacturacion.NumeroOrdenPago.ToString()), Convert.ToDateTime(DateTime.Now.GetDateTimeFormats()[5]), "");
                            //Utils.MostrarAlerta(Response, "Se ha creado la factura de proveedor con prefijo " + Request.QueryString["prefFP"] + " y el número " + Request.QueryString["numFP"] + ".");

                            FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                            try
                            {
                                formatoRecibo.Prefijo = dtInfoExcel.Rows[i]["PREFIJO"].ToString();
                                formatoRecibo.Numero = num;
                                formatoRecibo.Codigo = tableDocus.Select("PDOC_CODIGO = '" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + "'")[0].ItemArray[2].ToString();//DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + "'");
                                if (formatoRecibo.Codigo != string.Empty)
                                {
                                    if (formatoRecibo.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                                }
                            }
                            catch
                            {
                                lb.Text += "<font color='red' size='3px'> Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "</font> <br />";
                            }
                        }
                        else
                            lb.Text += "<font color='red' size='3px'> ocurrió un error generando la factura con prefijo [" + dtInfoExcel.Rows[i]["PREFIJO"].ToString() + " - " + num.ToString() + "] razón: " + miRecepcionFacturacion.ProcessMsg + "</font> <br />";
                        ////////////////////////////////////////////////////
                    }
                }
                btnAcptExcel.Visible = false;
                dgFacturaExcel.Visible = false;
                plcRets.Visible = false;
                fldRets.Visible = false;
                btnVolver.Visible = true;
                //txtObs.Visible = false;

            }
            
        }
        protected void btn_VolverPrincipal(object sender, EventArgs e)
        {
            Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref=&num=&fact=S&recDir=S");
        }
        protected void CargarPrincipal(object sender, EventArgs e)
        {
            Response.Redirect("" + indexPage + "?process=Vehiculos.FacturaProveedor");
        }

        protected int validarNumero(string prefijo, int num)
        {
            if (num <= Convert.ToInt32(DBFunctions.SingleData("SELECT PDOC_ULTIDOCU FROM DBXSCHEMA.PDOCUMENTO WHERE PDOC_CODIGO = '" + prefijo + "'")))
            {
                num++;
                return validarNumero(prefijo, num);
            }
            else
            {
                return num;
            }
        }

        public virtual void RegisterStartupScript(string Startup, string sb)
        {
            string Sstartup = Startup;
            string ssb = sb;
        }
    
		protected void Cambio_Prefijo(Object  Sender, EventArgs e)
		{
			numOrdPago.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu + 1 FROM pdocumento WHERE pdoc_codigo='"+prefOrdPago.SelectedValue+"'");
		}
		
		protected void Generar_Consecutivo(Object  Sender, EventArgs e)
		{
			if(chkCst.Checked)
			{
				numOrdPago.ReadOnly = true;
				numOrdPago.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu +1 FROM pdocumento WHERE pdoc_codigo='"+prefOrdPago.SelectedValue+"'");
			}
			else
				numOrdPago.ReadOnly = false;
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
