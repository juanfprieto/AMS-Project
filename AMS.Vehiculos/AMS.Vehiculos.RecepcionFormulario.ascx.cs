// created on 28/03/2005 at 15:39
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
using Ajax;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.Tools;
using System.Globalization;

namespace AMS.Vehiculos
{
	public partial class RecepcionFormulario : System.Web.UI.UserControl
	{
		#region Variables
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
        protected string recepcionManual;
        protected DataTable tbVehPed, tbInfTcn, tbRtmVeh, tbInfTcnExcel, dataTableRecepcion, dataTableFacturacion;
		protected DataTable tbInformacionTecnica, tbInformacionComercial,tbInformacionCompras, tbFacturas;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected FacturaCliente facturaVentaVehiculo;
		protected System.Web.UI.WebControls.Button Button1;
		protected DateTime FechaActual=DateTime.Now.Date;
        DatasToControls bind = new DatasToControls();

		#endregion

		#region Eventos

		protected void Page_Load(object sender, System.EventArgs e)
        {
            lbExcel.Text = "";
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Vehiculos.RecepcionFormulario));
            if(!Validar_Retenciones(null, null))
                return; 
            plcRets.Controls.Add(LoadControl(pathToControls+"AMS.Documentos.Retenciones.ascx"));

            if (!IsPostBack)
			{
                Session["infExcel"] = null;
				plcRets.Visible     = false;
				infVehPlHl.Visible  = false;

                bind.PutDatasIntoDropDownList(ubicacion, "SELECT pubi_codigo, pubi_nombre FROM pubicacion where PUBI_VIGENCIA = 'V' order by pubi_nombre");
                bind.PutDatasIntoDropDownList(ddlUbiRet, "SELECT pubi_codigo, pubi_nombre FROM pubicacion where PUBI_VIGENCIA = 'V' order by pubi_nombre");
                //bind.PutDatasIntoDropDownList(ddlProv, "SELECT MPR.mnit_nit, MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres FROM mnit MNI, mproveedor MPR WHERE MNI.mnit_nit = MPR.mnit_nit order by MNI.mnit_apellidos");
                bind.PutDatasIntoDropDownList(ddlPrefFactRet, "SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FP' and PH.tpro_proceso in ('VN','VU') AND P.PDOC_CODIGO = PH.PDOC_CODIGO order by p.pdoc_nombre");
				lbFch.Text = DateTime.Now.ToString("yyyy-MM-dd");
				if(Request.QueryString["recDir"] == "N")
				{
					//Cargamos los datos sobre el pedido : el proveedor, la fecha y los vehiculos si se ha tenido en cuenta la existencia de un pedido anteriormente
					prefPed.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+Request.QueryString["pref"]+"'");

                    numPed.Text = Request.QueryString["num"];
                    if (Request.QueryString["usado"] != null)
                    {
                        lbPrPed.Text = DBFunctions.SingleData(
                            @"SELECT mnit_apellidos CONCAT '-' CONCAT mnit_nombres  
                              FROM mnit WHERE mnit_nit=(select mnit_nit  
                              from dbxschema.mpedidovehiculo where pdoc_codigo='" + Request.QueryString["pref"] + @"'  
                               AND mped_numepedi=" + Request.QueryString["num"] + ");");
                        lbFchPed.Text = System.Convert.ToDateTime(DBFunctions.SingleData(
                            "select mped_pedido from dbxschema.mpedidovehiculo where pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["num"] + ";")).ToShortDateString();
                    }
                    else
                    {
                        lbPrPed.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT '-' CONCAT mnit_nombres FROM mnit WHERE mnit_nit=(SELECT mnit_nit FROM mpedidovehiculoproveedor WHERE pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["num"] + ")");
                        lbFchPed.Text = System.Convert.ToDateTime(DBFunctions.SingleData("SELECT mped_pedido FROM mpedidovehiculoproveedor WHERE pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["num"] + "")).ToShortDateString();
                    }
                   
                    Llenar_Tabla_Vehiculos_Pedidos(Request.QueryString["pref"],Request.QueryString["num"]);
					plhInfoPed.Visible = true;
                    
				}
				else if(Request.QueryString["recDir"] == "S")
				{
                    plhInfoRcp.Visible = true;
                    Preparar_Tabla_Vehiculos_Pedidos();
                    Bind_VehPed();
				}
				else if(Request.QueryString["recDir"] == "SR")//SI RETOMA
				{
					Preparar_Tabla_Informacion_Tecnica();
					int i = 0;
					double totalVehiculo = 0;
					//Este es el caso específico para realizar la recepcion de vehiculos de retoma
					//no se muestra el placeholder de vehPedPlHl y se muestra directamente el infVehPlHl
					//y se pide la ubicacion de estos vehiculos de retoma en plUbicVehRet
					plUbicVehRet.Visible = infVehPlHl.Visible = true;
					vehPedPlHl.Visible = false;
                    tbRtmVeh = (DataTable)Session["tablaRetomaGrabacion"];
					facturaVentaVehiculo = (FacturaCliente)Session["facturaVentaVehiculo"];
					tbInformacionCompras = (DataTable)Session["InfCompras"];
					//Agregamos a la tabla de información generado los id desde 1
					for(i=0;i<tbRtmVeh.Rows.Count;i++)
					{
						DataRow fila = tbInfTcn.NewRow();
						fila[0] = Convert.ToInt32(i+1);
						tbInfTcn.Rows.Add(fila);
						totalVehiculo += Convert.ToDouble(tbRtmVeh.Rows[i][2]);
					}
					lbTotal.Text  = totalVehiculo.ToString("C");
					Bind_InformacionTecnica();
					Bind_InformacionComercial();
					Agregar_VinBasico();
					Generar_NumerosInventario();
				}
			}

			if(Session["tbVehPed"] == null)
				Preparar_Tabla_Vehiculos_Pedidos();
			else
				tbVehPed = (DataTable)Session["tbVehPed"];
			if(Session["tbInfTcn"] == null)
				Preparar_Tabla_Informacion_Tecnica();
			else
				tbInfTcn = (DataTable)Session["tbInfTcn"];
			
			if(Session["InfCompras"] == null)
				Preparar_Tabla_InfCompras();
			else
				tbInformacionCompras = (DataTable)Session["InfCompras"];
            if (Session["infExcel"] == null)
                preparar_Tabla_InfExcel();
            else
                tbInfTcnExcel = (DataTable)Session["infExcel"];
            if (Session["sessFacturas"] == null)
                Preparar_Tabla_Factura();
            else
                tbFacturas = (DataTable)Session["sessFactura"];


            if (Session["tablaRetomaGrabacion"] != null)
				tbRtmVeh = (DataTable)Session["tablaRetomaGrabacion"];
			if(Session["facturaVentaVehiculo"] != null)
				facturaVentaVehiculo = (FacturaCliente)Session["facturaVentaVehiculo"];
            
           }



        protected void vamoACargarlo(object sender, EventArgs e)
        {
            if (rbusado.Checked == false && rbnuevo.Checked == false)
            {
                Utils.MostrarAlerta(Response, "Por favor seleccione un tipo de vehículo!");
                return;
            }
            //proceso de validación del Excel.
            if (filUpl.PostedFile.FileName.ToString() == string.Empty)
                Utils.MostrarAlerta(Response, "No ha seleccionado un archivo");
            else
            {
                string[] file = filUpl.PostedFile.FileName.ToString().Split('\\');
                string extencionArchivo = (file[file.Length - 1].Split('.'))[file[file.Length - 1].Split('.').Length - 1].ToUpper();
                if (extencionArchivo != "XLS" && extencionArchivo != "XLSX")
                    Utils.MostrarAlerta(Response, "No es un archivo de Excel");
                else
                {
                    DataSet ds = new DataSet();
                    DataSet ds1 = new DataSet();
                    filUpl.PostedFile.SaveAs(ConfigurationManager.AppSettings["PathToImportsExcel"] + file[file.Length - 1]);
                    ExcelFunctions exc = new ExcelFunctions(ConfigurationManager.AppSettings["PathToImportsExcel"] + file[file.Length - 1]);
                    if (ddlProv.SelectedValue == "900703240") // // Mazda Colombia solo vehículos nuevos ARCHIVO PROPIO
                    {
                        try
                        {
                             exc.Request(ds1, IncludeSchema.NO, "SELECT F1 AS CATALOGO, '" + HiPrefijos.Value + "' AS PREFIJO, F7 AS FACTURA, F4 AS VIN, F3 AS MOTOR, F4 AS SERIE, F4 AS CHASIS, F5 AS \"AÑO MODELO\", F8 AS COLOR, F13 AS VALOR,'A' AS \"TIPO SERVICIO\", '1' AS \"NUMERO MANIFIESTO\", '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AS \"FECHA MANIFIESTO\", '0' AS \"NUMERO ADUANA\", '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AS \"FECHA LEVANTE\", '1' AS \"NUMERO LEVANTE\", F10 AS \"FECHA ENTRADA\", F10 AS \"FECHA VENCE\", F10 AS \"FECHA DISPONIBLE\", 0 AS \"NUMERO DO\", '01'  AS ALMACEN, '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AS \"FECHA RECEPCION\" FROM MCOL; ");
                        }
                        catch
                        {
                            Utils.MostrarAlerta(Response, "No se encontró el archivo Excel con el espacio de nombre MCOL. Verifique que siguió correctamente las instrucciones para subir el archivo Excel.");
                            return;
                        }
                    }
                    else
                    {
                        exc.Request(ds1, IncludeSchema.NO, "SELECT * FROM RECEPCION");
                    }
                    
                    
                    if (ds1.Tables.Count == 0)
                    {
                        Utils.MostrarAlerta(Response, "No se encontró el archivo Excel. Verifique que siguió correctamente las instrucciones para subir el archivo Excel.");
                        return;
                    }
                    if (ds1.Tables[0].Columns.Count != 22)
                    {
                        Utils.MostrarAlerta(Response, "El número de Columnas del archivo Excel es diferente al número de columnas requeridos 22.");
              //          return;
                    }
                    if (ddlProv.SelectedValue != "900703240")
                    {                    
                        for (int i = 0; i < ds1.Tables[0].Columns.Count; i++)
                        {
                            ds1.Tables[0].Columns[i].ColumnName = ds1.Tables[0].Rows[0][i].ToString();
                        }
                    }
                    else // mazda colombia quitamos las comillas de los titulos
                    {
                        for (int i = 0; i < ds1.Tables[0].Columns.Count; i++)
                        {
                            ds1.Tables[0].Columns[i].ColumnName = ds1.Tables[0].Columns[i].ColumnName.Replace("\"","");
                        }
                    }
                    ds1.Tables[0].Rows[0].Delete();
                    ds1.Tables[0].Rows[0].AcceptChanges();
                    if (ddlProv.SelectedValue == "900703240")
                    {
                        ds1.Tables[0].Rows[0].Delete();
                        ds1.Tables[0].Rows[0].AcceptChanges();
                        //DataSet dsc = new DataSet();
                        string color_cod,valor;
                        string[] fechavar;
                        string almacen = DBFunctions.SingleData("select palm_almacen from palmacen where pcen_centvvn is not null fetch first 1 rows only;");
                        for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                        {
                            color_cod = DBFunctions.SingleData("SELECT PCOL_CODIGO FROM PCOLOR WHERE PCOL_DESCRIPCION = '" + ds1.Tables[0].Rows[i]["COLOR"].ToString() + "' FETCH FIRST 1 ROWS ONLY;");
                            ds1.Tables[0].Rows[i]["COLOR"] = color_cod;
                            fechavar = ds1.Tables[0].Rows[i]["FECHA ENTRADA"].ToString().Split('/');
                            ds1.Tables[0].Rows[i]["FECHA ENTRADA"] = fechavar[2].ToString() + '-' + fechavar[1].ToString() + '-' + fechavar[0].ToString();
                            ds1.Tables[0].Rows[i]["FECHA VENCE"] = fechavar[2].ToString() + '-' + fechavar[1].ToString() + '-' + fechavar[0].ToString();
                            ds1.Tables[0].Rows[i]["FECHA DISPONIBLE"] = fechavar[2].ToString() + '-' + fechavar[1].ToString() + '-' + fechavar[0].ToString();
                            valor = ds1.Tables[0].Rows[i]["VALOR"].ToString().Replace(",", "");
                            ds1.Tables[0].Rows[i]["VALOR"] = valor;
                            ds1.Tables[0].Rows[i]["ALMACEN"] = almacen.ToString() ;
                        }
                    }
                    int contFallo = 0;
                    DataSet dsCatalogos = new DataSet();
                    DBFunctions.Request(dsCatalogos, IncludeSchema.NO, @"SELECT PC.PCAT_CODIGO FROM PCATALOGOVEHICULO PC, POPCIONVEHICULO PO, PPRECIOVEHICULO PP WHERE PC.PCAT_CODIGO = PP.PCAT_CODIGO AND PO.POPC_OPCIVEHI = PP.POPC_OPCIVEHI ORDER BY 1; 
                                                                         SELECT PCOL_CODIGO FROM PCOLOR; 
                                                                         SELECT MCAT_VIN FROM MVEHICULO WHERE TEST_TIPOESTA >= 60;
                                                                         SELECT PDOC_CODIGO FROM DBXSCHEMA.PDOCUMENTO WHERE TDOC_TIPODOCU = 'FP' ORDER BY PDOC_CODIGO;");
                    //Validaciones
                    string fecha1, fecha2, fecha3, fecha4, fecha5;
                    //lbExcel.Text = "";
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        DataRow[] existeCat = dsCatalogos.Tables[0].Select("PCAT_CODIGO = '" + ds1.Tables[0].Rows[i]["CATALOGO"].ToString() + "'");
                        DataRow[] existeCol = dsCatalogos.Tables[1].Select("PCOL_CODIGO = '" + ds1.Tables[0].Rows[i]["COLOR"].ToString() + "'");
                        DataRow[] encontroVin = ds1.Tables[0].Select("VIN = '" + ds1.Tables[0].Rows[i]["VIN"].ToString() + "'"); // verificar si el VIN está repetido en la misma tabla
                        DataRow[] encontroMotor = ds1.Tables[0].Select("MOTOR = '" + ds1.Tables[0].Rows[i]["MOTOR"].ToString() + "'");
                        DataRow[] existeVin = dsCatalogos.Tables[2].Select("MCAT_VIN = '" + ds1.Tables[0].Rows[i]["VIN"].ToString() + "'");
                        DataRow[] existePrefijo = dsCatalogos.Tables[3].Select("PDOC_CODIGO = '" + ds1.Tables[0].Rows[i]["PREFIJO"].ToString() + "'");

                        if (ds1.Tables[0].Rows[i]["ALMACEN"].ToString().Trim() == "" || "NUMERO DO" == "" || ds1.Tables[0].Rows[i]["FECHA DISPONIBLE"].ToString().Trim() == "" || "FECHA VENCE" == "" 
                            || ds1.Tables[0].Rows[i]["NUMERO ADUANA"].ToString().Trim() == "" || ds1.Tables[0].Rows[i]["NUMERO LEVANTE"].ToString().Trim() == ""
                            || ds1.Tables[0].Rows[i]["TIPO SERVICIO"].ToString().Trim() =="" || ds1.Tables[0].Rows[i]["VALOR"].ToString().Trim() == "")
                        {
                            Utils.MostrarAlerta(Response, "Ningún campo debe estár vacio.");
                            return;
                        }
                        if (existeCat.Length < 1)
                        {
                            lbExcel.Text += "<font color='red' size='3px'> el catálogo: " + ds1.Tables[0].Rows[i]["CATALOGO"].ToString() + " NO se encontró dentro de la lista de catálogos. </font>" + "<br />";
                            contFallo++;
                        }
                        if (existeCol.Length < 1)
                        {
                            lbExcel.Text += "<font color='red' size='3px'> el código de color: " + ds1.Tables[0].Rows[i]["COLOR"].ToString() + " NO existe en la Base de Datos. </font>" + "<br />";
                            contFallo++;
                        }
                        if (encontroVin.Length > 1)
                        {
                            lbExcel.Text += "<font color='red' size='3px'> el Vin: " + ds1.Tables[0].Rows[i]["VIN"].ToString() + " está repetido en la tabla, Revise su archivo Excel. </font>" + "<br />";
                            contFallo++;
                        }
                        if (encontroMotor.Length > 1)
                        {
                            lbExcel.Text += "<font color='red' size='3px'> el Motor: " + ds1.Tables[0].Rows[i]["MOTOR"].ToString() + " está repetido en la tabla, Revise su archivo Excel. </font>" + "<br />";
                            contFallo++;
                        }
                        if(existeVin.Length > 0)
                        {
                            lbExcel.Text += "<font color='red' size='3px'> el Vin: " + ds1.Tables[0].Rows[i]["VIN"].ToString() + " existe en la Base de Datos. Por favor revise </font>" + "<br />";
                            contFallo++;
                        }
                        if(existePrefijo.Length < 1)
                        {
                            lbExcel.Text += "<font color='red' size='3px'> el Prefijo: " + ds1.Tables[0].Rows[i]["PREFIJO"].ToString() + " NO existe como prefijo de tipo FP(Factura Proveedor) en la Base de Datos. Por favor revise.</font>" + "<br />";
                            contFallo++;
                        }
                        if(!ds1.Tables[0].Rows[i]["FACTURA"].ToString().Contains("-"))
                        {
                            lbExcel.Text += "<font color='red' size='3px'> la Factura " + ds1.Tables[0].Rows[i]["PREFIJO"].ToString() + "está mal escrita, recuerde que debe tener el prefijo y el número separado por un guión ejemplo: F-12345.</font>" + "<br />";
                            contFallo++;
                        }
                        try
                        {
                            //DateTime h = DateTime.ParseExact(ds1.Tables[0].Rows[i]["FECHA ENTRADA"].ToString(), "yyyy-mm-dd", CultureInfo.CurrentCulture);
                            fecha1 = Convert.ToDateTime(ds1.Tables[0].Rows[i]["FECHA MANIFIESTO"]).GetDateTimeFormats()[5]; //.ToString()).ToString("yyyy-MM-dd");
                            fecha2 = Convert.ToDateTime(ds1.Tables[0].Rows[i]["FECHA LEVANTE"]).GetDateTimeFormats()[5]; //.ToString()).ToString("yyyy-MM-dd");
                            fecha3 = Convert.ToDateTime(ds1.Tables[0].Rows[i]["FECHA ENTRADA"]).GetDateTimeFormats()[5]; //.ToString()).ToString("yyyy-MM-dd");
                            fecha4 = Convert.ToDateTime(ds1.Tables[0].Rows[i]["FECHA VENCE"]).GetDateTimeFormats()[5]; //.ToString()).ToString("yyyy-MM-dd");
                            fecha5 = Convert.ToDateTime(ds1.Tables[0].Rows[i]["FECHA RECEPCION"]).GetDateTimeFormats()[5]; //.ToString()).ToString("yyyy-MM-dd");
                            /*
                                if (!ds1.Tables[0].Rows[i]["FECHA MANIFIESTO"].Equals(fecha1) || 
                                    !ds1.Tables[0].Rows[i]["FECHA LEVANTE"].Equals(fecha2) || 
                                    !ds1.Tables[0].Rows[i]["FECHA ENTRADA"].Equals(fecha3) || 
                                    !ds1.Tables[0].Rows[i]["FECHA VENCE"].Equals(fecha4))
                                {
                                    ds1.Tables[0].Rows[i]["FECHA MANIFIESTO"] = fecha1;
                                    ds1.Tables[0].Rows[i]["FECHA LEVANTE"] = fecha2;
                                    ds1.Tables[0].Rows[i]["FECHA ENTRADA"] = fecha3;
                                    ds1.Tables[0].Rows[i]["FECHA VENCE"] = fecha4;
                                }
                            */
                        }
                        catch
                        {

                            lbExcel.Text += " <font color='red' size='3px'> Una de las fechas de la fila " + (i+1) 
                                         + "<b> Catálogo: " + ds1.Tables[0].Rows[i]["CATALOGO"].ToString() 
                                         + " </b> - (" + ds1.Tables[0].Rows[i]["FECHA MANIFIESTO"].ToString() 
                                         + ") - (" + ds1.Tables[0].Rows[i]["FECHA LEVANTE"].ToString()
                                         + ") - (" + ds1.Tables[0].Rows[i]["FECHA ENTRADA"].ToString() + ") - (" 
                                         + ds1.Tables[0].Rows[i]["FECHA VENCE"].ToString() + ") - (" + ds1.Tables[0].Rows[i]["FECHA RECEPCION"].ToString() + ") está mal escrita. Recuerde el formato (yyyy-mm-dd) año-mes-día </font>" + "<br />";
                            //+ z.Message + "<br />"; ;
                            contFallo ++;
                        }
                        try
                        {
                            double valor = Convert.ToDouble(ds1.Tables[0].Rows[i]["VALOR"].ToString());
                        }
                        catch
                        {
                            lbExcel.Text += "<font color='red' size='3px'> El valor " + ds1.Tables[0].Rows[i]["VALOR"].ToString() + " está mal escrito, recuerde que no puede contener puntos ni comas </font>" + "<br />";
                            contFallo++;
                        }
                        if(!ds1.Tables[0].Rows[i]["FACTURA"].ToString().Contains("-"))
                        {
                            lbExcel.Text = "<font color='red' size='3px'> La Factura " + ds1.Tables[0].Rows[i]["FACTURA"].ToString() + "no cumple con el formato adecuado:caracter guión referencia, ej:(V-1234). Por favor revise </font>" + "<br />";
                            contFallo++;
                        }
                    }
                    if (contFallo > 0)
                    {
                        return;
                    }
                    ViewState["tablaExcel"] = ds1.Tables[0];
                    Session["tablaExcelCompleta"] = ds1.Tables[0];
                }
                
                Aceptar_Vehiculos_Excel(sender, e);
            }
        }

        protected void llamar_nuevo (object sender, EventArgs e)
        {
            vehPed.Visible = DBFunctions.SingleData("SELECT CVEH_RECEPMANUAL FROM DBXSCHEMA.CVEHICULOS;").ToUpper() == "N" ? false : true;
            if(vehPed.Visible)
            {
                ViewState["sqlCatalogos"] = "SELECT PC.PCAT_CODIGO, PC.PCAT_CODIGO|| ' ' || PCAT_DESCRIPCION CONCAT ' - ' CONCAT PO.POPC_NOMBOPCI FROM PCATALOGOVEHICULO PC, POPCIONVEHICULO PO, PPRECIOVEHICULO PP WHERE PC.PCAT_CODIGO = PP.PCAT_CODIGO AND   PO.POPC_OPCIVEHI = PP.POPC_OPCIVEHI ORDER BY 1";
                vehPed.DataSource = (DataTable)Session["tbVehPed"];
                vehPed.DataBind();
            }
            /*vehPed.Visible = true;
            recepcionManual = DBFunctions.SingleData("SELECT CVEH_RECEPMANUAL FROM DBXSCHEMA.CVEHICULOS;").ToUpper();
            if(recepcionManual != "S")
            {
                vehPed.Visible = false;
            }
            else
            {
                ViewState["sqlCatalogos"] = "SELECT PC.PCAT_CODIGO, PC.PCAT_CODIGO|| ' ' || PCAT_DESCRIPCION CONCAT ' - ' CONCAT PO.POPC_NOMBOPCI FROM PCATALOGOVEHICULO PC, POPCIONVEHICULO PO, PPRECIOVEHICULO PP WHERE PC.PCAT_CODIGO = PP.PCAT_CODIGO AND   PO.POPC_OPCIVEHI = PP.POPC_OPCIVEHI ORDER BY 1";
                vehPed.DataSource = (DataTable)Session["tbVehPed"];
                vehPed.DataBind();
            }*/
            ddlProv.Visible = true;
            txtProv.Visible = txtProved.Visible = lblnb.Visible = false;
            bind.PutDatasIntoDropDownList(ddlProv, "SELECT MPR.mnit_nit, MNI.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres FROM mnit MNI, mproveedor MPR, PCASAMATRIZ CM WHERE MNI.mnit_nit = MPR.mnit_nit AND MNI.mnit_nit = CM.mnit_nit order by MNI.mnit_apellidos ;");
            ddlProv.Items.Insert(0, "Seleccione:..");
            imglupa1.Visible = true;
            imglupa2.Visible = imglupa3.Visible = false;
            if (Request.QueryString["recDir"] == "S" && Request.QueryString["fact"] == "S")
                chkExcel.Visible = true;
            //bind.PutDatasIntoDropDownList(ddlCatalogo, sqlCatalogos);
			 
        }
        [Ajax.AjaxMethod]
        public DataSet Cargar_Nombre(string Cedula)
        {
            DataSet Vins = new DataSet();
            DBFunctions.Request(Vins, IncludeSchema.NO, "select mnit_nit from dbxschema.mnit where mnit_nit like '" + Cedula + "%';select mnit_nombres concat ' ' CONCAT COALESCE(mnit_nombre2,'') concat ' 'concat mnit_apellidos concat ' 'concat COALESCE(mnit_apellido2,'') as NOMBRE from dbxschema.mnit where mnit_nit='" + Cedula + "'");
            return Vins;
        }
        [Ajax.AjaxMethod]
        public DataSet Cargar_Prefijo(string nit)
        {
            DataSet nit_prefijo = new DataSet();
     //       if (nit == "900703240")  // Mazda Colombia solo vehículos nuevos ARCHIVO PROPIO
            {
                DBFunctions.Request(nit_prefijo, IncludeSchema.NO, "SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'FP' and PH.tpro_proceso in ('VN') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                return nit_prefijo;
            }             
     //           return nit_prefijo;
        }
        protected void llamar_usado (object sender, EventArgs e)
        {
            vehPed.Visible = true;
            /*vehPed.Visible = DBFunctions.SingleData("SELECT CVEH_RECEPMANUAL FROM DBXSCHEMA.CVEHICULOS;").ToUpper() == "N" ? false : true;
            if(vehPed.Visible)
            {
                ViewState["sqlCatalogos"] = "SELECT pcat_codigo, pcat_codigo || ' ' || pmar_nombre || ' '|| pcat_descripcion FROM pcatalogovehiculo pc, pmarca pm where pc.pmar_codigo = pm.pmar_codigo order by 2";
                vehPed.DataSource = (DataTable)Session["tbVehPed"];
                vehPed.DataBind();
            }*/

            chkExcel.Visible = false;
            ddlProv.Visible = false;
            txtProv.Visible = true;
            txtProved.Visible = true;
            lblnb.Visible = true;
            // bind.PutDatasIntoDropDownList(ddlProv, "SELECT MNI.mnit_nit, MNI.mnit_nit CONCAT ' - ' CONCAT MNI.mnit_apellidos CONCAT ' ' CONCAT coalesce(MNI.mnit_apellido2,'') CONCAT ' ' CONCAT MNI.mnit_nombres CONCAT ' ' CONCAT coalesce(MNI.mnit_nombre2,'') FROM mnit MNI order by 1; ");
            imglupa1.Visible = false;
            imglupa2.Visible = false;
            imglupa3.Visible = true;
            ViewState["sqlCatalogos"] = "SELECT pcat_codigo, pcat_codigo || ' ' || pmar_nombre || ' '|| pcat_descripcion FROM pcatalogovehiculo pc, pmarca pm where pc.pmar_codigo = pm.pmar_codigo order by 2";
            vehPed.DataSource = (DataTable)Session["tbVehPed"];
            vehPed.DataBind();
            //         bind.PutDatasIntoDropDownList(ddlCatalogo, sqlCatalogos);
        }
        
        protected bool Validar_Retenciones(object sender, DataGridCommandEventArgs e)
        {
            // Validacion de la tabla de RETENCIONES en la fuente
            string TipoSociedad = "J";
            DataSet dsR = new DataSet();
            DBFunctions.Request(dsR, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad + "' AND ttip_proceso='V' AND tret_codigo='RF';");
            if (dsR.Tables[0].Rows.Count != 1)
            {
                Utils.MostrarAlerta(Response, "Debe definir Solamente UN registro de RETENCION en la FUENTE de RENTA para el proceso VEHICULOS para las sociedades tipo " + TipoSociedad + "");
                return false;
            }
            dsR = new DataSet();
            DBFunctions.Request(dsR, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad + "' AND ttip_proceso='V' AND tret_codigo='RI';");
            if (dsR.Tables[0].Rows.Count != 1)
            {
                Utils.MostrarAlerta(Response, "Debe definir Solamente UN registro de RETENCION en la FUENTE de IVA para el proceso VEHICULOS para las sociedades tipo " + TipoSociedad + "");
                return false;
            }
            dsR = new DataSet();
            DBFunctions.Request(dsR, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad + "' AND ttip_proceso='V' AND tret_codigo='RC';");
            if (dsR.Tables[0].Rows.Count != 1)
            {
                Utils.MostrarAlerta(Response, "Debe definir Solamente UN registro de RETENCION en la FUENTE de ICA para el proceso VEHICULOS para las sociedades tipo " + TipoSociedad + "");
                return false;
            }
            //dsR = new DataSet();
            //DBFunctions.Request(dsR, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + TipoSociedad + "' AND ttip_proceso='V' AND tret_codigo='RE';");
            //if (dsR.Tables[0].Rows.Count != 1)
            //{
            //    Utils.MostrarAlerta(Response, "Debe definir Solamente UN registro de RETENCION en la FUENTE de CREE para el proceso de compra VEHICULOS para las sociedades tipo " + TipoSociedad + "");
            //    return false;
            //}
            return true;
        }

        protected void Cargar_Info(object sender, DataGridCommandEventArgs e)
		{
			int i;
			if (e.CommandName=="Cargar")
			{
				
				if( ((TextBox)infCmc.Items[0].Cells[8].FindControl("numMan")).Text!=String.Empty && ((TextBox)infCmc.Items[0].Cells[2].FindControl("numRcp")).Text!=String.Empty && ((TextBox)infCmc.Items[0].Cells[7].FindControl("fchMan")).Text!=String.Empty && ((TextBox)infCmc.Items[0].Cells[9].FindControl("numAdu")).Text!=String.Empty && ((TextBox)infCmc.Items[0].Cells[10].FindControl("numDO")).Text!=String.Empty && ((TextBox)infCmc.Items[0].Cells[11].FindControl("numLvt")).Text!=String.Empty )
				{
                    for(i=1; i < infCmc.Items.Count; i++)
                    {
                        ((TextBox)infCmc.Items[i].Cells[2].FindControl("numRcp")).Text=((TextBox)infCmc.Items[0].Cells[2].FindControl("numRcp")).Text;
                        ((TextBox)infCmc.Items[i].Cells[8].FindControl("numMan")).Text=((TextBox)infCmc.Items[0].Cells[8].FindControl("numMan")).Text;
                        ((TextBox)infCmc.Items[i].Cells[7].FindControl("fchMan")).Text=((TextBox)infCmc.Items[0].Cells[7].FindControl("fchMan")).Text;
                        ((TextBox)infCmc.Items[i].Cells[9].FindControl("numAdu")).Text=((TextBox)infCmc.Items[0].Cells[9].FindControl("numAdu")).Text;
                        ((TextBox)infCmc.Items[i].Cells[10].FindControl("numDO")).Text=((TextBox)infCmc.Items[0].Cells[10].FindControl("numDO")).Text;
                        ((TextBox)infCmc.Items[i].Cells[11].FindControl("numLvt")).Text=((TextBox)infCmc.Items[0].Cells[11].FindControl("numLvt")).Text;
                    }	
				}
				else
				{
                    Utils.MostrarAlerta(Response, "Por favor Complete los datos Obligatorios");
				}
								
			}
					
		}
		
		protected void Aceptar_Vehiculos(Object  Sender, EventArgs e)
		{
            //validación proveedor para usados 
            if (rbusado.Checked == false && rbnuevo.Checked == false)
            {
                Utils.MostrarAlerta(Response, "Por favor seleccione un tipo de vehículo!");
                return;
            }

            if (rbusado.Checked && plhInfoRcp.Visible == true)
            {
                if (txtProv.Text == "" )
                {
                    //txtProv.Text = ddlProv.SelectedValue;
                    Utils.MostrarAlerta(Response, "Por favor ingrese un proveedor valido!");
                    return;
                }

                if (!DBFunctions.RecordExist("select mnit_nit from mnit where mnit_Nit = '" + txtProv.Text + "' ") || txtProv.Text == "")
                {
                    Utils.MostrarAlerta(Response, "El proveedor Seleccionado NO existe en la tabla de Terceros");
                    return;
                }
            }

            //validación ddlUbicacion
            if (ubicacion.SelectedValue.Equals(""))
            {
                Utils.MostrarAlerta(Response, "No ha Seleccionado Ninguna Ubicación de la Recepción");
                return;
            }

            int cantActivos = 0;
			double totalVehiculo = 0;
			//Primero creamos la tabla informativa
			Preparar_Tabla_Informacion_Tecnica();
			//Ahora recorremos la grilla de los vehiculos relacionados con el pedido
			for(int i=0;i<vehPed.Items.Count;i++)
			{
				//Ahora verificamos que este activado el checkbox
				if(((CheckBox)vehPed.Items[i].Cells[4].Controls[1]).Checked==true)
				{
					cantActivos += 1;
					//si esta activo debemos agregar la fila a la tabla
					DataRow fila = tbInfTcn.NewRow();
					fila[0] = Convert.ToInt32(tbVehPed.Rows[i][0]);
                //    fila[1] = Convert.ToInt32(tbVehPed.Rows[i][2]);  //AQUI VIENE EL COLOR NO LO PUEDE CONVERTIR A ENTERO
                    fila[1] = (tbVehPed.Rows[i][2]);
					tbInfTcn.Rows.Add(fila);
					totalVehiculo += Convert.ToDouble(tbVehPed.Rows[i][3]);
				}
			}
			if(Request.QueryString["fact"]=="N" && Request.QueryString["recDir"] == "SR")
			{
				ArrayList arlValores=new ArrayList(),arlTipos=new ArrayList(),arlBases=new ArrayList();
				plcRets.Visible=true;
				Retenciones ctrRetenciones=(Retenciones)plcRets.Controls[0];
				//Precargar retenciones
				Retencion retencion=new Retencion(facturaVentaVehiculo.NitCliente,facturaVentaVehiculo.PrefijoFactura,
					Convert.ToInt32(facturaVentaVehiculo.NumeroFactura),(Convert.ToDouble(lbTotal.Text.Substring(1))+0),
					(0+0),"V",false);
				retencion.Guardar_Retenciones(false,ref arlValores,ref arlBases,ref arlTipos);
				ctrRetenciones.PrecargarRetenciones(facturaVentaVehiculo.NitCliente,"FP",arlValores,arlTipos,arlBases);
			}
			if(cantActivos > 0)
			{
				lbTotal.Text  = totalVehiculo.ToString("C");
				Bind_InformacionTecnica();
                //Ahora Agregamos los vin basicos de cada uno de los vehiculos retomados
                Agregar_VinBasico();
				//Realizamos el bind de la grilla de la informacion comercial
				Bind_InformacionComercial();
				Generar_NumerosInventario();
				vehPedPlHl.Visible = false;
				infVehPlHl.Visible = true;
			}
			else
                Utils.MostrarAlerta(Response, "No ha Seleccionado Ningun Vehiculo Para Recepción");
		}
		
        protected void Aceptar_Vehiculos_Excel(Object Sender, EventArgs e)
        {
            if (rbusado.Checked && plhInfoRcp.Visible)
            {
                if (txtProv.Text == "")
                {
                    Utils.MostrarAlerta(Response, "Por favor ingrese un proveedor valido!");
                    return;
                }

                if (!DBFunctions.RecordExist("select mnit_nit from mnit where mnit_Nit = '" + txtProv.Text + "' ") || txtProv.Text == "")
                {
                    Utils.MostrarAlerta(Response, "El proveedor Seleccionado NO existe en la tabla de Terceros");
                    return;
                }
            }
            else
            {
                if (ubicacion.SelectedValue.Equals(""))
                {
                    Utils.MostrarAlerta(Response, "No ha Seleccionado Ninguna Ubicación de la Recepción");
                    return;
                }
                //CREAMOS UN TABLA con los atributos y tuplas de la tabla del excel
                DataTable tablaDatosExcel = (DataTable)ViewState["tablaExcel"];

                //Preparar_Tabla_Informacion_Tecnica();
                Preparar_Tabla_Factura();
                string[] facturas;
                int ultimoNumeroInventario = 0;
                int numeroUltimaPlaca = 0;
                ultimoNumeroInventario = System.Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(mveh_inventario) FROM mvehiculo"));
                string ultimaPlaca = DBFunctions.SingleData("SELECT MAX(CAST (SUBSTR(mcat_placa,3) AS INTEGER)) FROM mcatalogovehiculo WHERE mcat_placa LIKE 'N-%'");
                numeroUltimaPlaca = System.Convert.ToInt32(ultimaPlaca);
                
                numeroUltimaPlaca++;
                ultimoNumeroInventario++;
                
                tbFacturas.Clear();
                tbInfTcnExcel.Clear();
                double total = 0;
                
                for (int i = 0; i < tablaDatosExcel.Rows.Count; i++)
                {
                    
                    facturas = tablaDatosExcel.Rows[i]["FACTURA"].ToString().Trim().Split('-');
                    DataRow filaFactura = tbFacturas.NewRow(), fila = tbInfTcnExcel.NewRow();
                    filaFactura[0] = i + 1;
                    filaFactura[1] = tablaDatosExcel.Rows[i]["PREFIJO"];//""; //prefijo no definido porque se carga automáticamente
                    filaFactura[2] = "";//número no definido porque es necesario otro proceso primero
                    filaFactura[3] = facturas[0].ToString();
                    filaFactura[4] = facturas[1].ToString();
                    filaFactura[5] = tablaDatosExcel.Rows[i]["ALMACEN"].ToString();
                    filaFactura[6] = tablaDatosExcel.Rows[i]["FECHA ENTRADA"];
                    filaFactura[7] = tablaDatosExcel.Rows[i]["FECHA VENCE"];
                    filaFactura[8] = Convert.ToDouble(tablaDatosExcel.Rows[i]["VALOR"]).ToString("C").Substring(1);
                    filaFactura[9] = "19.00"; // es fijo porque sólo se carga en una tabla, la cual puede ser editada (opción Excel)

                    fila[0] = i + 1;
                    fila[1] = tablaDatosExcel.Rows[i]["VIN"];
                    fila[2] = tablaDatosExcel.Rows[i]["MOTOR"];
                    fila[3] = tablaDatosExcel.Rows[i]["SERIE"];
                    fila[4] = tablaDatosExcel.Rows[i]["CHASIS"];
                    fila[5] = tablaDatosExcel.Rows[i]["COLOR"];
                    fila[6] = tablaDatosExcel.Rows[i]["AÑO MODELO"];
                    fila[7] = tablaDatosExcel.Rows[i]["TIPO SERVICIO"];
                    fila[8] = tablaDatosExcel.Rows[i]["NUMERO MANIFIESTO"];
                    fila[9] = tablaDatosExcel.Rows[i]["FECHA MANIFIESTO"];
                    fila[10] = tablaDatosExcel.Rows[i]["NUMERO ADUANA"];
                    fila[11] = tablaDatosExcel.Rows[i]["FECHA LEVANTE"];
                    fila[12] = tablaDatosExcel.Rows[i]["NUMERO LEVANTE"];
                    fila[13] = ultimoNumeroInventario;
                    fila[14] = "N-" + numeroUltimaPlaca.ToString();
                    fila[15] = tablaDatosExcel.Rows[i]["FECHA RECEPCION"];

                    tbInfTcnExcel.Rows.Add(fila);
                    tbFacturas.Rows.Add(filaFactura);

                    ultimoNumeroInventario ++;
                    numeroUltimaPlaca ++;
                    total += Convert.ToDouble(tablaDatosExcel.Rows[i]["VALOR"]);
                }
                lbTotal.Text = total.ToString();
                //Bind_InformacionComercialId(tablaIds);
                //Generar_NumerosInventarioExcel();
                ////Generar_NumerosInventario();
                //btnAceptarExcel.Enabled = false;
                //btnAcpt2.Visible = false;
                //btnAceptarExcel2.Visible = true;
                //vehPedPlHl.Visible = false;
                //infVehPlHl.Visible = true;
                Session["infExcel"] = tbInfTcnExcel;
                Session["sessFacturas"] = tbFacturas;

                Aceptar_Informacion_Excel(Sender, e);
            }
        }

        protected void Aceptar_Informacion(Object  Sender, EventArgs e)
		{
			//Aqui Primero debemos verificar la tabla grilla de la informacion tecnica el vin
			string validacionInfTecnica = Verificacion_InfTecnica();
			if(validacionInfTecnica == "")
			{
				string nitProveedor = "";
                if (Request.QueryString["recDir"] == "N")
                {
                    if(Session["Retoma"].ToString() == "S")
                        nitProveedor = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["num"] + "");
                    else
                        nitProveedor = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculoproveedor WHERE pdoc_codigo='" + Request.QueryString["pref"] + "' AND mped_numepedi=" + Request.QueryString["num"] + "");
                }
                else if (Request.QueryString["recDir"] == "S")
                {
                        if (txtProv.Text == "")
                        {
                            txtProv.Text = ddlProv.SelectedValue;
                        }
                        nitProveedor = txtProv.Text; // ddlProv.SelectedValue;
                }
                else if (Request.QueryString["recDir"] == "SR")
                        nitProveedor = facturaVentaVehiculo.NitCliente;
				if(Request.QueryString["fact"]=="N")
				{
					//Ahora creamos las tablas de transporte
					Llenar_Tabla_InformacionTecnica();
					Llenar_Tabla_InformacionComercial();
					if(Request.QueryString["recDir"] != "SR")
					{
						//Creamos el objeto que realiza la recepcion
						Recepcion miRecepcion = new Recepcion(tbInformacionTecnica,tbInformacionComercial,Request.QueryString["pref"],Request.QueryString["num"],ubicacion.SelectedValue,nitProveedor,"N");
						if(miRecepcion.Realizar_Recepcion(false,true))
						{
							lb.Text += "<br>BIEN "+miRecepcion.ProcessMsg;
							Response.Redirect("" + indexPage + "?process=Vehiculos.FacturaProveedor");
						}
						else
							lb.Text += "<br>ERROR "+miRecepcion.ProcessMsg;
					}
					else
					{
						Recepcion miRecepcion = new Recepcion(tbInformacionTecnica,tbInformacionComercial,Request.QueryString["pref"],Request.QueryString["num"],ddlUbiRet.SelectedValue,nitProveedor,"S");
						Retenciones docRets   = (Retenciones)plcRets.Controls[0];
						miRecepcion.PrefijoOrdenPago = ddlPrefFactRet.SelectedValue;
						miRecepcion.NumeroOrdenPago = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefFactRet.SelectedValue+"'");
						miRecepcion.NitProveedor = nitProveedor;
						miRecepcion.PrefijoFacturaProveedor = facturaVentaVehiculo.PrefijoFactura;
						miRecepcion.NumeroFacturaProveedor = facturaVentaVehiculo.NumeroFactura.ToString();
						miRecepcion.FechaFactura = DateTime.Now.ToString("yyyy-MM-dd");
						miRecepcion.Almacen = facturaVentaVehiculo.CodigoAlmacen;
						miRecepcion.FechaIngreso = DateTime.Now.ToString("yyyy-MM-dd");
						miRecepcion.FechaVencimiento = DateTime.Now.ToString("yyyy-MM-dd");
						miRecepcion.FechaUltimoPago = DateTime.Now.ToString("yyyy-MM-dd");
						miRecepcion.ValorFactura = Convert.ToDouble(lbTotal.Text.Substring(1)).ToString();
						miRecepcion.ValorAbono = Convert.ToDouble(lbTotal.Text.Substring(1)).ToString();
						miRecepcion.ValorFletes = "0";
						miRecepcion.ValorRetencion = "0";
						miRecepcion.Observacion = "Factura de Retoma";
						miRecepcion.Usuario = HttpContext.Current.User.Identity.Name.ToLower();
						miRecepcion.dtRetenciones = docRets.tablaRtns;
						// Las consignaciones y las retomas no llevan IVA.
						// double porcentajeIva = Convert.ToDouble(DBFunctions.SingleData("SELECT cemp_porciva FROM cempresa"));
						// miRecepcion.ValorIva = (Convert.ToDouble(lbTotal.Text.Substring(1))*(porcentajeIva/100)).ToString();
						miRecepcion.ValorIva = "0";
						miRecepcion.ValorIvaFletes = "0";
						miRecepcion.EstadoFacturaProveedor = "C";
						facturaVentaVehiculo.AgregarPago(miRecepcion.PrefijoOrdenPago,Convert.ToUInt32(miRecepcion.NumeroOrdenPago),Convert.ToDouble(lbTotal.Text.Substring(1)),"Factura de Retoma de Vehiculo");
						facturaVentaVehiculo.GrabarFacturaCliente(false);
						for(int i=0;i<facturaVentaVehiculo.SqlStrings.Count;i++)
							miRecepcion.SqlRels.Add(facturaVentaVehiculo.SqlStrings[i].ToString());
						if(miRecepcion.Realizar_Recepcion(true,true))
							Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionFacturacionVehiculos&facVeh=2&prefFC="+facturaVentaVehiculo.PrefijoFactura+"&numFC="+facturaVentaVehiculo.NumeroFactura+"&prefFR="+miRecepcion.PrefijoOrdenPago+"&numFR="+miRecepcion.NumeroOrdenPago);
							//lb.Text += "<br><br><br> bien "+miRecepcion.ProcessMsg;
						else
							lb.Text += "<br><br><br> error "+miRecepcion.ProcessMsg;
					}
				}
				else
				{
					//Debemos limpiar el Objeto Session
					//Session.Clear();
					//Ahora creamos las tablas de transporte
					Llenar_Tabla_InformacionTecnica();
					Llenar_Tabla_InformacionComercial();
					//Ahora las enviamos en el Session
					Session["tbInformacionTecnica"] = tbInformacionTecnica;
					Session["tbInformacionComercial"] = tbInformacionComercial;
					Session["InfCompras"]=(DataTable)tbInformacionCompras;
					
					//Ahora redireccionamos al control de usuario que maneja la facturacion
                    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionFormulario&pref=" + Request.QueryString["pref"] + "&num=" + Request.QueryString["num"] + "&proc=1&ubi=" + ubicacion.SelectedValue + "&val=" + Convert.ToDouble(lbTotal.Text.Substring(1)).ToString() + "&nitProv=" + nitProveedor + "&vin=" + tbInformacionComercial.Rows[0][2] + "&cat=" + tbInformacionComercial.Rows[0][1]);
				}
			}
			else
			{
				string[] splitErrors1 = validacionInfTecnica.Split('-');
				for(int i=0;i<splitErrors1.Length-1;i++)
                Utils.MostrarAlerta(Response, "" + splitErrors1[i] + "");
			}
		}

        protected void Aceptar_Informacion_Excel(Object Sender, EventArgs e)
        {
            string nitProveedor = ""; ;
            if (Request.QueryString["recDir"] == "S")
            {
                if (txtProv.Text == "")
                {
                    txtProv.Text = ddlProv.SelectedValue;
                }
                nitProveedor = txtProv.Text; // ddlProv.SelectedValue;
                Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionFormulario&proc=1&ubi=" + ubicacion.SelectedValue + "&val=" + Convert.ToDouble(lbTotal.Text).ToString() + "&nitProv=" + nitProveedor + "&excel=1");
            }
            else
                Utils.MostrarAlerta(Response, "se generó un error al momento de seleccionar el Nit del proveedor, por favor vuelva a intentarlo.");
        }

        protected void dgSeleccion_Evento(object sender, DataGridCommandEventArgs e)
		{
			if(((Button)e.CommandSource).CommandName == "AddDatasRow")
			{
				ArrayList itemsSeleccionados = new ArrayList();
				itemsSeleccionados = this.Revisar_Seleccionados();
				DataRow fila = tbVehPed.NewRow();
				fila[0] = tbVehPed.Rows.Count + 1;
				fila[1] = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue;
				fila[2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
				fila[3] = Convert.ToDouble(((TextBox)e.Item.Cells[3].Controls[1]).Text);
				tbVehPed.Rows.Add(fila);
				Bind_VehPed();
				((CheckBox)vehPed.Items[vehPed.Items.Count-1].Cells[4].Controls[1]).Checked = true;
				Seleccionar_Items(itemsSeleccionados);
			}
		}
		
		protected string Verificacion_InfTecnica()
		{
			//Debemos recorrer la grilla de la informacion tecnica 
			string errors = "";
			Preparar_Tabla_InfCompras();
            try
            {
                for (int i = 0; i < tbInfTcn.Rows.Count; i++)
                {
                    DataRow[] selection = tbVehPed.Select("ID=" + tbInfTcn.Rows[i][0].ToString().Trim() + "");
                    string vinNew   = ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text;
                    string numMotor = ((TextBox)infTcn.Items[i].Cells[2].Controls[1]).Text;
                    string tipoVeh  = ((DropDownList)(infCmc.Items[i].Cells[6].Controls[1])).SelectedValue.ToString();  // Nuevo o Usado
                    string numPlaca = ((TextBox)infTcn.Items[i].Cells[8].Controls[1]).Text;
                    if (tipoVeh == "N")
                        numPlaca = "";
                    if (rbnuevo.Checked && tipoVeh != "N" || rbusado.Checked && tipoVeh != "U")
                        errors += "La clase del Vehículo con ID " + tbInfTcn.Rows[i][0].ToString().Trim() + ", NO corresponde a la Selección inicial - ";
                    if (selection.Length > 0)
                    {
                        string vinBasico = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='" + selection[0][1].ToString() + "'");
                        {
                        try
                        {
                            string vinBasicoUsuario = "";

                            if (Request.QueryString["usado"] == null && tipoVeh == "N")
                            {
                                vinBasicoUsuario = vinNew.Substring(0, vinBasico.Length);
                            }

                            if (Request.QueryString["usado"] == null && vinBasico != vinBasicoUsuario && tipoVeh == "N")
                                errors += "VIN Basico Modificado del Vehiculo con ID " + tbInfTcn.Rows[i][0].ToString().Trim() + "-";
                            else if (Request.QueryString["usado"] == null && vinBasico == vinNew && tipoVeh == "N")
                                    errors += "VIN sin completar del Vehiculo con ID " + tbInfTcn.Rows[i][0].ToString().Trim() + "-";
                            else if (Verificacion_VINExistentes(vinNew, i))
                            {
                                DataSet infoVeh = new DataSet();
                                string numVinMcat = DBFunctions.SingleData("select mcat_vin FROM DBXSCHEMA.MCATALOGOVEHICULO where mcat_motor='" + numMotor + "'");
                                DBFunctions.Request(infoVeh, IncludeSchema.NO, "select mveh.mveh_fechrece, mveh.mveh_valocomp, mnit.mnit_nit, mveh.mveh_kilometr, mnit.mnit_nombres concat ' ' concat coalesce(mnit.mnit_nombre2,'') concat ' ' concat coalesce(mnit.mnit_apellidos,'') concat ' ' concat coalesce(mnit.mnit_apellido2,''), mveh.MVEH_INVENTARIO, mveh.MCAT_VIN, mveh.test_tipoesta from dbxschema.mvehiculo mveh,dbxschema.mnit mnit where (mcat_vin='" + vinNew + "' or mcat_vin='" + numVinMcat + "') and mveh.mnit_nit=mnit.mnit_nit");
                                string numComp = DBFunctions.SingleData("select COUNT(*) FROM DBXSCHEMA.MVEHICULO where mcat_vin='" + vinNew + "' OR mcat_vin='" + numVinMcat + "' ");
                                for (int j = 0; j < infoVeh.Tables[0].Rows.Count; j++)
                                {
                                    LLenar_Tabla_InformacionCompras(infoVeh.Tables[0].Rows[j][6].ToString(), infoVeh.Tables[0].Rows[j][5].ToString(), infoVeh.Tables[0].Rows[j][0].ToString(), infoVeh.Tables[0].Rows[j][4].ToString(), Convert.ToDouble(infoVeh.Tables[0].Rows[j][1].ToString()), Convert.ToDouble(infoVeh.Tables[0].Rows[j][3]));
                                    if (Convert.ToInt64(infoVeh.Tables[0].Rows[j]["TEST_TIPOESTA"]) < 60)
                                        errors += "Este Vehículo ya fue adquirido. Vin:" + infoVeh.Tables[0].Rows[j][6].ToString() + " Motor: " + numMotor + " con Fecha de Recepción: " + Convert.ToDateTime(infoVeh.Tables[0].Rows[j][0].ToString()).ToString("yyyy-MM-dd").Replace("-", "/") + ", el estado actual del vehículo es " + (DBFunctions.SingleData("SELECT test_nombesta from dbxschema.testadovehiculo WHERE test_tipoesta=" + infoVeh.Tables[0].Rows[j]["TEST_TIPOESTA"].ToString() + " ")).Trim() + ", con número de inventario " + infoVeh.Tables[0].Rows[j][5].ToString() + " por lo tanto no se podrá efectuar la Recepción. - ";
                                    else
                                        Utils.MostrarAlerta(Response, "Este Vehículo ya fue adquirido. Vin: " + infoVeh.Tables[0].Rows[j][6].ToString() + " Motor: " + numMotor + " con Fecha de Recepción: " + Convert.ToDateTime(infoVeh.Tables[0].Rows[j][0].ToString()).ToString("yyyy-MM-dd").Replace("-", "/") + ", el estado actual del vehículo es " + DBFunctions.SingleData("SELECT test_nombesta from dbxschema.testadovehiculo WHERE test_tipoesta=" + infoVeh.Tables[0].Rows[j]["TEST_TIPOESTA"].ToString() + " ") + ", con número de inventario " + infoVeh.Tables[0].Rows[j][5].ToString() + "");
                                    //JFSC 11022008 Poner en comentario por no ser usado
                                    //comprado=true;
                                }
                                //if (((DropDownList)(infCmc.Items[i].Cells[6].Controls[1])).SelectedValue=="N" && comprado==true)
                                //errors += "Este Vehiculo ya fue adquirido, por lo tanto no puede ser ingresado Número de Inventario "+((TextBox)(infCmc.Items[i].Cells[1].Controls[1])).Text+" como Nuevo. - ";
                                Session["InfCompras"] = (DataTable)tbInformacionCompras;
                            }
                            if (rbnuevo.Checked == true && tipoVeh != "N" || rbusado.Checked == true && tipoVeh != "U")
                                errors += "El tipo de Vehículo Seleccionado no coincide con el tipo de vehiculo del Proceso";
                        }
                        catch
                        {
                            errors += "VIN Basico Modificado del Vehículo con ID " + tbInfTcn.Rows[i][0].ToString().Trim() + "-";
                        }
                        //Ahora verificamos los numeros de motor que tambien son unicos
                        //Revisamos si no existe este este numero de motor ya registrado
                        }
                        if (Verificacion_NumMotor(vinNew, numMotor, numPlaca, i))
                        {
                            string vinMotor = Session["vinMotor"].ToString();
                            string vinPlaca = Session["vinPlaca"].ToString();
                            if (vinMotor != vinNew)
                                errors += "El número de motor " + numMotor + " para el vehículo con VIN " + vinNew + " ya se encuentra registrado con el VIN " + vinMotor + " Revice !!! -";
                            if (vinPlaca != vinNew && tipoVeh == "U")
                                errors += "El número de placa " + numPlaca + " para el vehículo con VIN " + vinNew + " ya se encuentra registrado con el VIN " + vinPlaca + " Revice !!! -";

                        }
                    }
                }
            }
            catch
            {
                errors += "NO hay información técnica para vehículo(s) " ;
            }
             
            return errors;
     	}
		
		protected bool Verificacion_VINExistentes(string vinNew, int index)
		{
			bool encontrado = false;
			DataSet infoVeh = new DataSet();
			if(DBFunctions.RecordExist("SELECT * FROM mvehiculo WHERE mcat_vin='"+vinNew+"'"))
				encontrado = true;
			else
			{
				for(int i=0;i<infTcn.Items.Count;i++)
				{
					if(i!=index)
					{
						if(vinNew==((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text)
							encontrado = true;
					}
				}
			}
			return encontrado;
		}

        protected bool Verificacion_NumMotor(string vinNew, string numMotor, string numPlaca, int index)
		{
			bool encontrado = false;
            Session["vinMotor"] = vinNew;
            Session["vinPlaca"] = vinNew;
            string vinMotor = DBFunctions.SingleData("SELECT mcat_vin FROM mCATALOGOvehiculo WHERE mcat_motor='" + numMotor + "'");
            if (vinMotor.Length > 1)
            {
                if (vinMotor == vinNew )
                    encontrado = false;
                else
                {
                    encontrado = true;
                    Session["vinMotor"] = vinMotor;
                }
            }
            if (numPlaca.Length > 0)
            {
                string vinPlaca = DBFunctions.SingleData("SELECT mcat_vin FROM mCATALOGOvehiculo WHERE mcat_placa='" + numPlaca + "'");
                if (vinPlaca.Length > 1)
                {
                    if (vinPlaca == vinNew)
                        encontrado = false;
                    else
                    {
                        encontrado = true;
                        Session["vinPlaca"] = vinPlaca;
                    }
                }
            }
            else
            {
                for (int i = 0; i < infTcn.Items.Count; i++)
                {
                    if (i != index)
                    {
                        if (numMotor == ((TextBox)infTcn.Items[i].Cells[2].Controls[1]).Text)
                        {
                            encontrado = true;
                            Session["vinMotor"] = (TextBox)infTcn.Items[i].Cells[2].Controls[1];
                        }
                    }
                }
            }
			return encontrado;
		}
		
		protected ArrayList Revisar_Seleccionados()
		{
			ArrayList resultado = new ArrayList();
			for(int i=0;i<vehPed.Items.Count;i++)
			{
				if(((CheckBox)vehPed.Items[i].Cells[4].Controls[1]).Checked)
					resultado.Add(i.ToString());
			}
			return resultado;
		}
		
		protected void Seleccionar_Items(ArrayList resultado)
		{
			for(int i=0;i<resultado.Count;i++)
				((CheckBox)vehPed.Items[System.Convert.ToInt32(resultado[i])].Cells[4].Controls[1]).Checked = true;
		}

        protected void infTcnItemDataBound(object sender, DataGridItemEventArgs e)
		{
            if (rbnuevo.Checked)
               e.Item.Cells[8].Visible = false;
    
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[5].Controls[1]),Colores.COLORES);
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[7].Controls[1]), "SELECT tser_tiposerv, tser_nombserv FROM tserviciovehiculo order by tser_tiposerv");
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].Controls[1]),"select pano_ano from dbxschema.pano order by pano_ano desc");
                //((DropDownList)e.Item.Cells[5].Controls[1]).SelectedValue = ((System.Data.DataRowView)(e.Item.DataItem)).DataView.Table.Rows[ e.Item.ItemIndex][1].ToString();
                //(TextBox)e.Item.Cells[6].Controls[1].Visible = true;
            }
		}
		
		protected void infCmcItemDataBound(object sender, DataGridItemEventArgs e)
		{
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (rbusado.Checked)
                {
                    e.Item.Cells[8].Text = "FECHA MATRICULA";
                    e.Item.Cells[10].Text = "PLACAS DE";
                   
                }
            }

			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[6].Controls[1]),"SELECT tcla_clase, tcla_nombre FROM tclasevehiculo ORDER BY tcla_clase");
                if(Session["Retoma"].ToString() == "S")
                  bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[12].Controls[1]), "SELECT tcom_codigo,tcom_tipocompra FROM tcompravehiculo where tcom_codigo = 'R' ORDER BY tcom_tipocompra");
				else
                  bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[12].Controls[1]), "SELECT tcom_codigo,tcom_tipocompra FROM tcompravehiculo where tcom_codigo <> 'R' ORDER BY tcom_tipocompra");

                
                //((TextBox)e.Item.Cells[2].Controls[1]).Text = (DBFunctions.SingleData("SELECT MAX(mveh_inventario) FROM mvehiculo")).ToString();
                ((TextBox)e.Item.Cells[3].Controls[1]).Text=FechaActual.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[4].Controls[1]).Text=FechaActual.ToString("yyyy-MM-dd");
				((TextBox)e.Item.Cells[5].Controls[1]).Text="0";
                if (rbusado.Checked)
                {
                //    ((TextBox)e.Item.Cells[8].Controls[1]).Text= "Fecha Matric Inicial";   // poner LOS TITULOS ADECUADOS para los USADOS
                //    ((TextBox)e.Item.Cells[9].Controls[1]).Text=  "Placas de ";
                //   ((TextBox)e.Item.Cells[10].Controls[1]).Text = "Accesorios"; 
                     ((TextBox)e.Item.Cells[10].Controls[1]).MaxLength = 20; // aqui hay que poner el tamaño del campo de la DB para usados se debe ampliar por ahi a 200 
                }
   
			}
		}

		protected void vehPedItemDataBound(object sender, DataGridItemEventArgs e)
		{
          	if(e.Item.ItemType == ListItemType.Footer)
			{
				DatasToControls bind = new DatasToControls();
                string sqlCatalogos = "";

                if(rbnuevo.Checked)   // solo carga los catalogos que estan en la lista de precios
                    sqlCatalogos = @"SELECT PC.PCAT_CODIGO,
                                            PC.PCAT_CODIGO|| ' ' || PCAT_DESCRIPCION CONCAT ' - ' CONCAT PO.POPC_NOMBOPCI
                                    FROM PCATALOGOVEHICULO PC,
                                            POPCIONVEHICULO PO,
                                            PPRECIOVEHICULO PP
                                    WHERE PC.PCAT_CODIGO = PP.PCAT_CODIGO
                                    AND   PO.POPC_OPCIVEHI = PP.POPC_OPCIVEHI
                                    ORDER BY 1;";
                else 
           //         if (rbusado.Checked)  // carga todos los catalogos 
                    sqlCatalogos = "SELECT pcat_codigo, pcat_codigo || ' ' || pmar_nombre || ' '|| pcat_descripcion FROM pcatalogovehiculo pc, pmarca pm where pc.pmar_codigo = pm.pmar_codigo order by 2";

                if (ViewState["sqlCatalogos"] != null)
                    sqlCatalogos = ViewState["sqlCatalogos"].ToString();

                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].Controls[1]),sqlCatalogos);
				bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]),Colores.COLORES);
			}
		}
		
		
		#endregion
		
		#region Manejo de tablas de conexion al otro control
		
		protected void Preparar_Tabla_InfTecEnv()
		{
			tbInformacionTecnica = new DataTable();			
			tbInformacionTecnica.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));//0
			tbInformacionTecnica.Columns.Add(new DataColumn("VIN",System.Type.GetType("System.String")));//1
			tbInformacionTecnica.Columns.Add(new DataColumn("MOTOR",System.Type.GetType("System.String")));//2
			tbInformacionTecnica.Columns.Add(new DataColumn("SERIE",System.Type.GetType("System.String")));//3
			tbInformacionTecnica.Columns.Add(new DataColumn("CHASIS",System.Type.GetType("System.String")));//4
			tbInformacionTecnica.Columns.Add(new DataColumn("COLOR",System.Type.GetType("System.String")));//5
			tbInformacionTecnica.Columns.Add(new DataColumn("AÑO MODELO",System.Type.GetType("System.String")));//6
			tbInformacionTecnica.Columns.Add(new DataColumn("TIPO DE SERVICIO",System.Type.GetType("System.String")));//7
			tbInformacionTecnica.Columns.Add(new DataColumn("PLACA",System.Type.GetType("System.String")));//8   // ACTIVAR ESTE CAMPO CUANDO EL VEHICULO ES USADO
		}
		
		protected void Preparar_Tabla_InfCompras()
		{
			tbInformacionCompras = new DataTable();
			tbInformacionCompras.Columns.Add(new DataColumn("Inventario",System.Type.GetType("System.String")));
			tbInformacionCompras.Columns.Add(new DataColumn("VIN",System.Type.GetType("System.String")));
			tbInformacionCompras.Columns.Add(new DataColumn("Fecha Recepcion",System.Type.GetType("System.String")));
			tbInformacionCompras.Columns.Add(new DataColumn("Propietario",System.Type.GetType("System.String")));
			tbInformacionCompras.Columns.Add(new DataColumn("Valor Compra",System.Type.GetType("System.String")));
			tbInformacionCompras.Columns.Add(new DataColumn("Kilometraje",System.Type.GetType("System.Double")));
			
		}
        protected void Preparar_Tabla_Factura()
        {
            tbFacturas = new DataTable();
            tbFacturas.Columns.Add(new DataColumn("ID", System.Type.GetType("System.Int32")));
            tbFacturas.Columns.Add(new DataColumn("PREFIJO", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("NUMERO", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("PREFIJO FACTURA", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("NUMERO FACTURA", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("ALMACEN", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("FECHA INGRESO", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("FECHA VENCE", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("VALOR FACTURA", System.Type.GetType("System.String")));
            tbFacturas.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.String")));

        }

        protected void LLenar_Tabla_InformacionCompras(string VIN,string Inventario, string FechaRecepcion,string Propietario,double ValorCompra,double Kilometraje)
		{
			DataRow fila =  tbInformacionCompras.NewRow();
			fila["Inventario"]=Inventario;
			fila["VIN"]=VIN;
			fila["Fecha Recepcion"]=Convert.ToDateTime(FechaRecepcion).ToString("yyyy-MM-dd");
			fila["Propietario"]=Propietario;
			fila["Valor Compra"]=ValorCompra.ToString("c");
			fila["Kilometraje"]=Kilometraje;
			tbInformacionCompras.Rows.Add(fila);
		}

		protected void Llenar_Tabla_InformacionTecnica()
		{
			//Para llenar esta tabla debemos recorrer la grilla infTcn
			Preparar_Tabla_InfTecEnv();
			int numeroUltimaPlaca = 0;
			string ultimaPlaca = DBFunctions.SingleData("SELECT MAX(CAST (SUBSTR(mcat_placa,3) AS INTEGER)) FROM mcatalogovehiculo WHERE mcat_placa LIKE 'N-%'");
			try
			{
				numeroUltimaPlaca = System.Convert.ToInt32(ultimaPlaca.Substring(ultimaPlaca.IndexOf('-')+1));
			}
			catch{}
			for(int i=0;i<tbInfTcn.Rows.Count;i++)
			{
				numeroUltimaPlaca += 1;
				DataRow fila = tbInformacionTecnica.NewRow();
				if(Request.QueryString["recDir"] != "SR")
				{
					DataRow[] selection = tbVehPed.Select("ID="+tbInfTcn.Rows[i][0].ToString().Trim()+"");
					fila[0] = selection[0][1].ToString();
				}
				else
					fila[0] = tbRtmVeh.Rows[i][0].ToString();
				fila[1] = ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text;
				fila[2] = ((TextBox)infTcn.Items[i].Cells[2].Controls[1]).Text;
				fila[3] = ((TextBox)infTcn.Items[i].Cells[3].Controls[1]).Text;
				fila[4] = ((TextBox)infTcn.Items[i].Cells[4].Controls[1]).Text;
				fila[5] = ((DropDownList)infTcn.Items[i].Cells[5].Controls[1]).SelectedValue;
				fila[6] = ((DropDownList)infTcn.Items[i].Cells[6].Controls[1]).SelectedValue;
				fila[7] = ((DropDownList)infTcn.Items[i].Cells[7].Controls[1]).SelectedValue;
                if (Request.QueryString["recDir"] != "SR")
                    if (infTcn.Items[i].Cells[8].Visible == false)
                        fila[8] = "N-"+numeroUltimaPlaca.ToString();
                    else
                        fila[8] = ((TextBox)infTcn.Items[i].Cells[8].Controls[1]).Text;
                else
                    fila[8] = tbRtmVeh.Rows[i][5].ToString();
				tbInformacionTecnica.Rows.Add(fila);
			}
		}
		
		protected void Preparar_Tabla_InfComEnv()
		{
			tbInformacionComercial = new DataTable();
			tbInformacionComercial.Columns.Add(new DataColumn("NÚMERO INVENTARIO",System.Type.GetType("System.String")));//0
			tbInformacionComercial.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));//1
			tbInformacionComercial.Columns.Add(new DataColumn("VIN",System.Type.GetType("System.String")));//2
			tbInformacionComercial.Columns.Add(new DataColumn("NÚMERO RECEPCIÓN",System.Type.GetType("System.String")));//3
			tbInformacionComercial.Columns.Add(new DataColumn("FECHA RECEPCIÓN",System.Type.GetType("System.String")));//4
			tbInformacionComercial.Columns.Add(new DataColumn("FECHA DISPONIBLE",System.Type.GetType("System.String")));//5
			tbInformacionComercial.Columns.Add(new DataColumn("KILOMETRAJE RECEPCIÓN",System.Type.GetType("System.String")));//6
			tbInformacionComercial.Columns.Add(new DataColumn("CLASE VEHICULO",System.Type.GetType("System.String")));//7
			tbInformacionComercial.Columns.Add(new DataColumn("NÚMERO MANIFIESTO",System.Type.GetType("System.String")));//8
            if (rbusado.Checked)
            {
                tbInformacionComercial.Columns.Add(new DataColumn("FECHA MATRICULA", System.Type.GetType("System.String")));//9
            }
            else
            {
                tbInformacionComercial.Columns.Add(new DataColumn("FECHA MANIFIESTO", System.Type.GetType("System.String")));//9
            }           
			tbInformacionComercial.Columns.Add(new DataColumn("NÚMERO ADUANA",System.Type.GetType("System.String")));//10
			tbInformacionComercial.Columns.Add(new DataColumn("NÚMERO DO",System.Type.GetType("System.String")));//11
			tbInformacionComercial.Columns.Add(new DataColumn("NÚMERO LEVANTE",System.Type.GetType("System.String")));//12
			tbInformacionComercial.Columns.Add(new DataColumn("TIPO COMPRA",System.Type.GetType("System.String")));//13
			tbInformacionComercial.Columns.Add(new DataColumn("VALOR VEHICULO",System.Type.GetType("System.Double")));//14
		}
		
		protected void Llenar_Tabla_InformacionComercial()
		{
			Preparar_Tabla_InfComEnv();
			double totalVehiculos = 0;
			for(int i=0;i<tbInfTcn.Rows.Count;i++)
			{
				DataRow fila = tbInformacionComercial.NewRow();
				fila[0] = ((TextBox)infCmc.Items[i].Cells[1].Controls[1]).Text;
				if(Request.QueryString["recDir"] != "SR")
				{
					DataRow[] selection = tbVehPed.Select("ID="+tbInfTcn.Rows[i][0].ToString().Trim()+"");
					fila[1] = selection[0][1].ToString();
				}
				else
					fila[1] = tbRtmVeh.Rows[i][0].ToString();
				fila[2] = ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text;
				fila[3] = ((TextBox)infCmc.Items[i].Cells[2].Controls[1]).Text;
				fila[4] = ((TextBox)infCmc.Items[i].Cells[3].Controls[1]).Text;
				fila[5] = ((TextBox)infCmc.Items[i].Cells[4].Controls[1]).Text;
				fila[6] = Convert.ToDouble(((TextBox)infCmc.Items[i].Cells[5].Controls[1]).Text).ToString();
				fila[7] = ((DropDownList)infCmc.Items[i].Cells[6].Controls[1]).SelectedValue;
				
				fila[8] = ((TextBox)infCmc.Items[i].Cells[7].Controls[1]).Text;
				fila[9] = ((TextBox)infCmc.Items[i].Cells[8].Controls[1]).Text;
				fila[10] = ((TextBox)infCmc.Items[i].Cells[9].Controls[1]).Text;
				fila[11] = ((TextBox)infCmc.Items[i].Cells[10].Controls[1]).Text;
				fila[12] = ((TextBox)infCmc.Items[i].Cells[11].Controls[1]).Text;
				fila[13] = ((DropDownList)infCmc.Items[i].Cells[12].Controls[1]).SelectedValue;
				fila[14] = Convert.ToDouble(((TextBox)infCmc.Items[i].Cells[13].Controls[1]).Text.Trim());
				totalVehiculos += Convert.ToDouble(((TextBox)infCmc.Items[i].Cells[13].Controls[1]).Text.Trim());
				tbInformacionComercial.Rows.Add(fila);
			}
			lbTotal.Text  = totalVehiculos.ToString("C");
		}
		
		#endregion
		
		#region Manejo de Grilla de Informacion Comercial
		
		protected void Bind_InformacionComercial()
		{
			Session["tbInfTcn"] = tbInfTcn;
			infCmc.DataSource = tbInfTcn;
			infCmc.DataBind();
		}
        protected void Bind_InformacionComercialId(DataTable tablaID)
        {
            infCmc.DataSource = tablaID;
            infCmc.DataBind();
        }

        protected void Generar_NumerosInventario()
		{
			int ultimoNumeroInventario = 0, i=0;
			try{ultimoNumeroInventario = System.Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(mveh_inventario) FROM mvehiculo"));}
			catch{}
			for(i=0;i<infCmc.Items.Count;i++)
			{
				ultimoNumeroInventario+=1;
				((TextBox)infCmc.Items[i].Cells[1].Controls[1]).Text = ultimoNumeroInventario.ToString();
                // esta linea se adiciono para que cargue en NÚMERO DE RECEPCIÓN el mismo numero de inventario
                ((TextBox)infCmc.Items[i].Cells[2].Controls[1]).Text = ultimoNumeroInventario.ToString();   
            }
           
			
            if(Request.QueryString["recDir"] != "SR")
			{
				//Agregamos el precio de compra ingresada en la grilla de informacion comercial del vehiculo
				for(i=0;i<tbInfTcn.Rows.Count;i++)
				{
					DataRow[] selection = tbVehPed.Select("ID="+tbInfTcn.Rows[i][0].ToString().Trim()+"");
                    if (selection.Length > 0)
                    {
                        ((TextBox)infCmc.Items[i].Cells[13].Controls[1]).Text = Convert.ToDouble(selection[0][3]).ToString("N");
                        if (Request.QueryString["usado"] != null)
                        {
                            ((DropDownList)infCmc.Items[i].Cells[6].Controls[1]).SelectedIndex = 1;
                        }
                    }
				}
			}
			else
			{
				for(i=0;i<tbRtmVeh.Rows.Count;i++)
				{
					((TextBox)infCmc.Items[i].Cells[13].Controls[1]).Text = Convert.ToDouble(tbRtmVeh.Rows[i][2]).ToString("N");
					((TextBox)infCmc.Items[i].Cells[13].Controls[1]).ReadOnly = true;
					((DropDownList)infCmc.Items[i].Cells[6].Controls[1]).SelectedIndex = 1;
				}
			}
		}

        //protected void Generar_NumerosInventarioExcel()
        //{
        //    int ultimoNumeroInventario = 0, i = 0;
        //    try { ultimoNumeroInventario = System.Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(mveh_inventario) FROM mvehiculo")); }
        //    catch { }
        //    DataTable exploracion = (DataTable)ViewState["tablaExcel"];
        //    for (i = 0; i < infCmc.Items.Count; i++)
        //    {
        //        ultimoNumeroInventario += 1;
        //        ((TextBox)infCmc.Items[i].Cells[1].Controls[1]).Text = ultimoNumeroInventario.ToString();
        //        // esta linea se adiciono para que cargue en NÚMERO DE RECEPCIÓN el mismo numero de inventario
        //        ((TextBox)infCmc.Items[i].Cells[2].Controls[1]).Text = ultimoNumeroInventario.ToString();

        //        string fechaManifiesto = "";// exploracion.Rows[0]["FECHA MANIFIESTO"].ToString();
        //        string fechaLevante = "";// exploracion.Rows[0]["FECHA LEVANTE"].ToString();
        //        try
        //        {
        //            fechaManifiesto = (Convert.ToDateTime(tbInfTcnExcel.Rows[i]["FECHA MANIFIESTO"])).ToString("yyyy-MM-dd");
        //            fechaLevante = (Convert.ToDateTime(tbInfTcnExcel.Rows[i]["FECHA MANIFIESTO"])).ToString("yyyy-MM-dd");
        //        }
        //        catch
        //        {
        //            fechaManifiesto = DateTime.Now.Date.GetDateTimeFormats()[5];
        //            fechaLevante = DateTime.Now.Date.GetDateTimeFormats()[5];
        //        }
        //        //------------------------------------
        //        ((TextBox)infCmc.Items[i].Cells[7].Controls[1]).Text = tbInfTcnExcel.Rows[i]["NUMERO MANIFIESTO"].ToString();
        //        ((TextBox)infCmc.Items[i].Cells[8].Controls[1]).Text = fechaManifiesto;
        //        ((TextBox)infCmc.Items[i].Cells[9].Controls[1]).Text = tbInfTcnExcel.Rows[i]["NUMERO ADUANA"].ToString();
        //        ((TextBox)infCmc.Items[i].Cells[10].Controls[1]).Text = fechaLevante;
        //        ((TextBox)infCmc.Items[i].Cells[11].Controls[1]).Text = tbInfTcnExcel.Rows[i]["NUMERO LEVANTE"].ToString();
        //    }


        //    if (Request.QueryString["recDir"] != "SR")
        //    {
        //        //Agregamos el precio de compra ingresada en la grilla de informacion comercial del vehiculo
        //        for (i = 0; i < infCmc.Items.Count; i++)
        //        {
        //            DataRow[] selection = tbVehPed.Select("ID=" + tbInfTcnExcel.Rows[i][0].ToString().Trim() + "");
        //            if (selection.Length > 0)
        //            {
        //                ((TextBox)infCmc.Items[i].Cells[13].Controls[1]).Text = Convert.ToDouble(selection[0][3]).ToString("N");
        //                if (Request.QueryString["usado"] != null)
        //                {
        //                    ((DropDownList)infCmc.Items[i].Cells[6].Controls[1]).SelectedIndex = 1;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (i = 0; i < tbRtmVeh.Rows.Count; i++)
        //        {
        //            ((TextBox)infCmc.Items[i].Cells[13].Controls[1]).Text = Convert.ToDouble(tbRtmVeh.Rows[i][2]).ToString("N");
        //            ((TextBox)infCmc.Items[i].Cells[13].Controls[1]).ReadOnly = true;
        //            ((DropDownList)infCmc.Items[i].Cells[6].Controls[1]).SelectedIndex = 1;
        //        }
        //    }
        //}

        #endregion

        #region Manejo de Grilla de Informacion Tecnica

        protected void Preparar_Tabla_Informacion_Tecnica()
		{
			tbInfTcn = new DataTable();
			tbInfTcn.Columns.Add(new DataColumn("ID",System.Type.GetType("System.Int32")));
            tbInfTcn.Columns.Add(new DataColumn("COLOR", System.Type.GetType("System.String")));
            //tbInfTcn.Columns.Add(new DataColumn("COLOR", System.Type.GetType("System.Int32")));
           
  		}
        
        protected void preparar_Tabla_InfExcel()
        {
            tbInfTcnExcel = new DataTable();
            tbInfTcnExcel.Columns.Add(new DataColumn("ID", System.Type.GetType("System.Int32")));
            tbInfTcnExcel.Columns.Add(new DataColumn("PREFIJO", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("VIN", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("MOTOR", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("SERIE", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("CHASIS", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("COLOR", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("AÑO MODELO", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("TIPO SERVICIO", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("NUMERO MANIFIESTO", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("FECHA MANIFIESTO", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("NUMERO ADUANA", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("FECHA LEVANTE", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("NUMERO LEVANTE", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("NUM INVENTARIO", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("PLACA", System.Type.GetType("System.String")));
            tbInfTcnExcel.Columns.Add(new DataColumn("FECHA RECEPCION", System.Type.GetType("System.String")));
            //tbInfTcnExcel.Columns.Add(new DataColumn("FACTURA", System.Type.GetType("System.String")));
            //tbInfTcnExcel.Columns.Add(new DataColumn("FECHA ENTRADA", System.Type.GetType("System.String")));
            //tbInfTcnExcel.Columns.Add(new DataColumn("FECHA VENCE", System.Type.GetType("System.String")));
            //tbInfTcnExcel.Columns.Add(new DataColumn("VALOR", System.Type.GetType("System.String")));
        }

        protected void bind_Inf_Tcn_Excel()
        {
            Session["infExcel"] = tbInfTcnExcel;
            infTcn.DataSource = tbInfTcnExcel;
            infTcn.DataBind();
        }
        protected void bind_ID_infTcn(DataTable tablaIDs)
        {
            infTcn.DataSource = tablaIDs;
            infTcn.DataBind();
        }
        protected void Bind_InformacionTecnica()
		{
			Session["tbInfTcn"] = tbInfTcn;
           // infTcn.
			infTcn.DataSource = tbInfTcn;
			infTcn.DataBind();
		}
		
		protected void Agregar_VinBasico()
		{
			for(int i=0;i<tbInfTcn.Rows.Count;i++)
			{
				if(Request.QueryString["recDir"] != "SR")
				{
					DataRow[] selection = tbVehPed.Select("ID="+tbInfTcn.Rows[i][0].ToString().Trim()+"");
					if(selection.Length>0)
					{
                        DataSet catalogoExistente = new DataSet();
                        string prefijoPedido = Request.QueryString["pref"].ToString();
                        Int32  numeroPedido  = Convert.ToInt32(0+Request.QueryString["num"].ToString());
                        DBFunctions.Request(catalogoExistente, IncludeSchema.NO,
                            "SELECT mcat_vin, mcat_motor, mcat_serie, mcat_chasis, mcat_anomode " +
                            "FROM mcatalogovehiculo WHERE mcat_placa = (SELECT dped_numeplaca " +
                            "FROM dbxschema.dpedidovehiculoretoma where pdoc_codigo='" + prefijoPedido + "' and mped_numepedi=" + numeroPedido + ");");
                        if (catalogoExistente.Tables[0].Rows.Count != 0)
                        {
                            if (Request.QueryString["usado"] != null)
                            {
                                ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text = catalogoExistente.Tables[0].Rows[0][0].ToString();
                                ((TextBox)infTcn.Items[i].Cells[2].Controls[1]).Text = catalogoExistente.Tables[0].Rows[0][1].ToString();
                                ((TextBox)infTcn.Items[i].Cells[3].Controls[1]).Text = catalogoExistente.Tables[0].Rows[0][2].ToString();
                                ((TextBox)infTcn.Items[i].Cells[4].Controls[1]).Text = catalogoExistente.Tables[0].Rows[0][3].ToString();
                                DatasToControls.EstablecerValueDropDownList(((DropDownList)infTcn.Items[i].Cells[5].Controls[1]), DBFunctions.SingleData("SELECT pcol_codigo FROM dbxschema.pcolor WHERE pcol_descripcion='" + selection[0][2].ToString() + "';").ToString() );
                                DatasToControls.EstablecerDefectoDropDownList(((DropDownList)infTcn.Items[i].Cells[6].Controls[1]), catalogoExistente.Tables[0].Rows[0][4].ToString() );

                                ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Enabled = false;
                                ((TextBox)infTcn.Items[i].Cells[2].Controls[1]).Enabled = false;
                                ((TextBox)infTcn.Items[i].Cells[3].Controls[1]).Enabled = false;
                                ((TextBox)infTcn.Items[i].Cells[4].Controls[1]).Enabled = false;
                                ((DropDownList)infTcn.Items[i].Cells[6].Controls[1]).Enabled = false;
                            }
                            else
                            {
                                ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='" + selection[0][1].ToString() + "'");
                            }
                        }
                        else
                        {
                            ((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='" + selection[0][1].ToString() + "'");
                            DatasToControls.EstablecerValueDropDownList(((DropDownList)infTcn.Items[i].Cells[5].Controls[1]), DBFunctions.SingleData("SELECT pcol_codigo FROM dbxschema.pcolor WHERE pcol_descripcion='" + selection[0][2].ToString() + "';").ToString());
                        }
						
						//DatasToControls.EstablecerDefectoDropDownList(((DropDownList)infTcn.Items[i].Cells[5].Controls[1]),selection[0][2].ToString().Trim());
					}
				}
				else
				{
					((TextBox)infTcn.Items[i].Cells[1].Controls[1]).Text = DBFunctions.SingleData("SELECT pcat_vinbasico FROM pcatalogovehiculo WHERE pcat_codigo='"+tbRtmVeh.Rows[i][0]+"'");
					//((DropDownList)infTcn.Items[i].Cells[6].Controls[1]) = tbRtmVeh.Rows[i][4].ToString();
					DatasToControls.EstablecerDefectoDropDownList(((DropDownList)infTcn.Items[i].Cells[6].Controls[1]),tbRtmVeh.Rows[i][4].ToString());
					DatasToControls.EstablecerValueDropDownList(((DropDownList)infTcn.Items[i].Cells[5].Controls[1]),DBFunctions.SingleData("SELECT pcol_codigo FROM pcolor WHERE pcol_codigo='"+tbRtmVeh.Rows[i][1]+"'"));
				}
			}
		}
		
		#endregion
		
		#region Manejo de Grilla de los Vehiculos del Pedido		
		
		protected void Preparar_Tabla_Vehiculos_Pedidos()
		{
			tbVehPed = new DataTable();
			tbVehPed.Columns.Add(new DataColumn("ID",System.Type.GetType("System.Int32")));//0
			tbVehPed.Columns.Add(new DataColumn("CATALOGO",System.Type.GetType("System.String")));//1
			tbVehPed.Columns.Add(new DataColumn("COLOR",System.Type.GetType("System.String")));//2
			tbVehPed.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.Double")));//3
		}
		
		protected void Llenar_Tabla_Vehiculos_Pedidos(string prefPedido, string numPedido)
		{
			int cantFilasAc = 1;
			Preparar_Tabla_Vehiculos_Pedidos();
            DataSet dsPed = new DataSet();
            if (Request.QueryString["usado"] != null)
            {
                DBFunctions.Request(dsPed, IncludeSchema.NO,
                    @"select dp.pcat_codigo, mc.pcol_codigo, dp.dped_valoreci, 1 as Filas 
                    from dpedidovehiculoretoma dp, mcatalogovehiculo mc  
                    where dp.pdoc_codigo='" + prefPedido + @"' and dp.mped_numepedi=" + numPedido + @" and mc.mcat_placa=dp.dped_numeplaca
                    AND MC.MCAT_VIN NOT IN (Select MCAT_VIN FROM MVEHICULO WHERE TEST_tipoESTA < 60);");

                if (dsPed.Tables[0].Rows.Count == 0)
                {
                    dsPed = new DataSet();
                    DBFunctions.Request(dsPed, IncludeSchema.NO,
                        @"select dp.pcat_codigo, 1 as COLOR, dp.dped_valoreci, 1 as Filas from dpedidovehiculoretoma dp  
                           where dp.DPED_NUMEplaca NOT IN(Select MCAT_placa FROM MVEHICULO mv, McatalogoVEHICULO mc WHERE mc.MCAT_VIN = mv.MCAT_VIN and TEST_tipoESTA < 60)
                             AND dp.pdoc_codigo='" + prefPedido + @"' and dp.mped_numepedi=" + numPedido + @"   ");
                }
            }
            else
            {
                DBFunctions.Request(dsPed, IncludeSchema.NO, "SELECT pcat_codigo, pcol_codigo, dped_valounit, dped_cantpedi - dped_cantingr FROM dpedidovehiculoproveedor WHERE pdoc_codigo='" + prefPedido + "' AND mped_numepedi=" + numPedido + " AND dped_cantpedi<>dped_cantingr");
            }
            
            for(int i=0;i<dsPed.Tables[0].Rows.Count;i++)
			{
				int cantFilas = System.Convert.ToInt32(dsPed.Tables[0].Rows[i][3]);
				for(int j=0;j<cantFilas;j++)
				{
					DataRow fila = tbVehPed.NewRow();
					fila[0] = cantFilasAc;
					fila[1] = dsPed.Tables[0].Rows[i][0].ToString();
					fila[2] = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo='"+dsPed.Tables[0].Rows[i][1].ToString()+"'");
					fila[3] = System.Convert.ToDouble(dsPed.Tables[0].Rows[i][2]);
					tbVehPed.Rows.Add(fila);
					cantFilasAc += 1;
				}
			}
			Bind_VehPed();
		}
		
		protected void Bind_VehPed()
		{
			Session["tbVehPed"] = tbVehPed;
			vehPed.DataSource = tbVehPed;
			vehPed.DataBind();
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
