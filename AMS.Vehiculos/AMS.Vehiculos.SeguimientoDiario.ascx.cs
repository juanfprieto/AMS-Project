namespace AMS.Vehiculos
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
	using AMS.DB;
	using AMS.Forms;
    using Microsoft.VisualBasic;
    using AMS.Documentos;
    using System.Data.SqlClient;
    using AMS.Tools;

    


	/// <summary>
	///		Descripción breve de AMS_Vehiculos_SeguimientoDiario.
	/// </summary>
    public partial class AMS_Vehiculos_SeguimientoDiario : System.Web.UI.UserControl
    {

        #region Constantes

        public const string CADENANULO = "NULL";
        protected DatasToControls bind = new DatasToControls();
        public const string FORMATOFECHA = "yyyy-MM-dd";
        public string strCookie = "";
        public string anoActual;
        protected DataTable tablaVehiculos;
        protected DataTable tablaDescuentos;


        #endregion

        #region Atributos

        protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected DataTable tablaElementos, dtPagos, datosContacto;

        #endregion

        #region Propiedades

        public string VendedorAutenticacion { get { return ddlVendedorAutenticacion.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlVendedorAutenticacion, value); } }
        public string Contrasena { get { return txtContrasena.Text; } set { txtContrasena.Text = value; } }
        public string ContactosDisponibles { get { return ddlContactosDisponibles.SelectedValue; } }
        public string ContactosDisponiblesPrefijo { get { return ContactosDisponibles.Split('-')[0]; } }
        public string ContactosDisponiblesNumero { get { return ContactosDisponibles.Split('-')[1]; } }
        public string strIniScript = "";

        public double CuotaFin;
        public int NumeroCuotasFin;
        public double PorcentajeTasaFin;

        //protected string Modo;
        
        public string Modo
        {
            get
            {
                return txtModo.Text;
            }
            set
            {
                txtModo.Text = value;

                switch (value)
                {
                    case ModoSeguimientoDiario.COTIZACION:
                        NumeroContactos = 0;
                        pAutenticacionUsuario.Visible = false;
                        pContactosDisponibles.Visible = false;
                        pIngresoDatos.Visible = true;
                        break;
                    case ModoSeguimientoDiario.CONTACTO:
                        pAutenticacionUsuario.Visible = true;
                        pContactosDisponibles.Visible = false;
                        pIngresoDatos.Visible = false;
                        break;
                }
            }
        }

        public string Prefijo { get { return ddlPrefijo.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlPrefijo, value); } }
        public int    Numero { get { return InterpretarCadenaVaciaEntero(txtNumero.Text); } set { txtNumero.Text = InterpretarValorNuloAMostrar(value); } }
        public string Nombre { get { return txtNombre.Text; } set { txtNombre.Text = value; } }
        public string TelefonoFijo { get { return txtTelefonoFijo.Text; } set { txtTelefonoFijo.Text = value; } }
        public string TelefonoMovil { get { return txtTelefonoMovil.Text; } set { txtTelefonoMovil.Text = value; } }
        public string TelefonoOficina { get { return txtTelefonoOficina.Text; } set { txtTelefonoOficina.Text = value; } }
        public string Email { get { return txtEmail.Text.ToLower(); } set { txtEmail.Text = value; } }
        public string Medio { get { return ddlMedio.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlMedio, value); } }
        public string Prospecto { get { return ddlProspecto.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlProspecto, value); } }
        public string CatalogoVehiculo { get { return ddlCatalogoVehiculo.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlCatalogoVehiculo, value); } }
        public string Equipamento { get { return txtEquipamento.Text; } set { txtEquipamento.Text = value; } }
        public int    Modelo { get { return InterpretarCadenaVaciaEntero(ddlModelo.SelectedValue); } set { ddlModelo.SelectedIndex = ddlModelo.Items.IndexOf(ddlModelo.Items.FindByValue(InterpretarValorNuloAMostrar(value))); } }
        public double Venta { get { return InterpretarCadenaVaciaDoble(txtVenta.Text); } set { txtVenta.Text = InterpretarValorNuloAMostrar(value); } }
        public double PrecioTotal { get { return InterpretarCadenaVaciaDoble(txtPrecioTotal.Text); } set { txtPrecioTotal.Text = InterpretarValorNuloAMostrar(value); } }
        public double ValorDescuento { get { return InterpretarCadenaVaciaDoble(txtDescuento.Text); } set { txtDescuento.Text = InterpretarValorNuloAMostrar(value); } }
        public double PrecioNeto { get { return InterpretarCadenaVaciaDoble(txtNeto.Text); } set { txtNeto.Text = InterpretarValorNuloAMostrar(value); } }
        public double ValorFinanciacion { get { return InterpretarCadenaVaciaDoble(txtValorFinanciacion.Text); } set { txtValorFinanciacion.Text = InterpretarValorNuloAMostrar(value); } }
        public string TipoCliente { get { return ddlTipoCliente.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlTipoCliente, value); } }
        public string TipoVehiculo { get { return ddlTipoVehiculo.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlTipoVehiculo, value); } }
        public string Color { get { return ddlcolor.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlcolor, value); } }
        public string NitFinaciera { get { return ddlFinanciera.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlFinanciera, value); } }
        public string NitCliente { get { return TextboxN.Text; } set { TextboxN.Text = value; } }
        public string RepresentanteLegal { get { return txtRLegal.Text; } set {  txtRLegal.Text = value; } }
        public string NitRepresentanteLegal { get { return txtNitRLegal.Text; } set { txtNitRLegal.Text = value; } }
    
        public int NumeroContactos
        {
            get
            {
                return Convert.ToInt32(txtNumeroContactos.Text);
            }
            set
            {
                int numeroContactos = 0;

                if (value == 0)
                    ddlVendedor.Enabled = true;
                else
                    ddlVendedor.Enabled = false;

                value++;

                numeroContactos = value;

                txtNumeroContactos.Text = numeroContactos.ToString();
            }
        }

        public string ProximoContacto { get { return ddlProximoContacto.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlProximoContacto, value); ddlProximoContacto_SelectedIndexChanged(this, null); } }

        public DateTime FechaProximoContacto
        {
            get
            {
                if (ProximoContacto == "SI")
                    return cFechaProximoContacto.SelectedDate;
                else
                    return DateTime.MinValue;
            }
            set
            {
                if (ProximoContacto == "SI")
                {
                    cFechaProximoContacto.Enabled = true;
                    cFechaProximoContacto.SelectedDate = value;
                }
                else
                    cFechaProximoContacto.Enabled = false;
            }
        }

        public int ResultadoContacto { get { return InterpretarCadenaVaciaEntero(ddlTipoContacto.SelectedValue); } set { DatasToControls.EstablecerValueDropDownList(ddlTipoContacto, InterpretarValorNuloAMostrar(value)); } }
        public string Vendedor { get { return ddlVendedor.SelectedValue; } set { DatasToControls.EstablecerValueDropDownList(ddlVendedor, value); } }

        #endregion

        #region Metodos

        public string EquipamientoVehiculo (string Catalogo)
        {
            // llamarla solo si es vehiculo nuevo
            // tambien poner en otro campo las especificaciones tecnicas que se tomas de pcatalogovehiculo
            DataSet dsEquipamiento = new DataSet();
            dsEquipamiento = DBFunctions.Request(dsEquipamiento, IncludeSchema.NO, string.Format(
                @"Select pacc_descripcion from paccesorio pa, pcatalogoaccesorio pca where pa.pacc_codigo = pca.pacc_codigo and pca.pcat_codigo = '"+ Catalogo +"';"));
            string equipamiento = "";
            if(dsEquipamiento.Tables[0].Rows.Count>0)
            {
                for(int i=0;i<dsEquipamiento.Tables[0].Rows.Count; i++)
                {
                    equipamiento += dsEquipamiento.Tables[0].Rows[i].ToString();
                    equipamiento += " ";
                }
            }
            return equipamiento; 
        }


        public double InterpretarCadenaVaciaDoble(string cadena)
        {
            if (cadena == "")
                return 0;
            else
                return Convert.ToDouble(cadena);
        }

        public int InterpretarCadenaVaciaEntero(string cadena)
        {
            if (cadena == "")
                return -1;
            else
                return Convert.ToInt32(cadena);
        }

        public DateTime InterpretarCadenaVaciaFecha(string cadena)
        {
            if (cadena == "")
                return DateTime.MinValue;
            else
                return Convert.ToDateTime(cadena);
        }

        public string InterpretarValorNuloAMostrar(object valor)
        {
            string valorValidado = string.Empty;

            if (valor is string)
            {
                if ((string)valor == string.Empty)
                    valorValidado = string.Empty;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is int)
            {
                if ((int)valor == -1)
                    valorValidado = string.Empty;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is double)
            {
                if ((double)valor == -1)
                    valorValidado = string.Empty;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is DateTime)
            {
                if ((DateTime)valor == DateTime.MinValue)
                    valorValidado = string.Empty;
                else
                    valorValidado = Convert.ToDateTime(valor).ToString(FORMATOFECHA);
            }

            return valorValidado;
        }

        public string ValidarValorCadena(object valor)
        {
            string valorValidado = CADENANULO;

            if (valor is string)
            {
                if ((string)valor == string.Empty)
                    valorValidado = CADENANULO;
                else
                    valorValidado = "'" + valor.ToString() + "'";
            }
            else if (valor is int)
            {
                if ((int)valor == -1)
                    valorValidado = CADENANULO;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is double)
            {
                if ((double)valor == -1)
                    valorValidado = CADENANULO;
                else
                    valorValidado = valor.ToString();
            }
            else if (valor is DateTime)
            {
                if ((DateTime)valor == DateTime.MinValue)
                    valorValidado = CADENANULO;
                else
                    valorValidado = "DATE('" + Convert.ToDateTime(valor).ToString("yyyy-MM-dd") + "')";
            }

            return valorValidado;
        }

        public static object ValidarDBNull(object valor, Type tipo)
        {
            object valorValidado = null;

            if (valor != DBNull.Value)
                valorValidado = valor;
            else
            {
                if (tipo.Equals(typeof(string)))
                    valorValidado = string.Empty;
                else if (tipo.Equals(typeof(int)))
                    valorValidado = -1;
                else if (tipo.Equals(typeof(double)))
                    valorValidado = -1;
                else if (tipo.Equals(typeof(decimal)))
                    valorValidado = -1;
                else if (tipo.Equals(typeof(DateTime)))
                    valorValidado = DateTime.MinValue;
            }

            return valorValidado;
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, System.EventArgs e)
        {      
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Vehiculos.AMS_Vehiculos_SeguimientoDiario));

            
            txtVenta.Enabled = false;
            if (ViewState["ano"] == null)
                ViewState["ano"] = DBFunctions.SingleData("SELECT PANO_ANO FROM DBXSCHEMA.CVEHICULOS;");
            //anoActual = ViewState["ano"].ToString();
            if (!Page.IsPostBack)
            {

                ViewState["cambioClase"] = "N";
                txtEquipamento.Attributes.Add("maxlength", txtEquipamento.MaxLength.ToString());   
                string descuento = null;
                try
                {
                    descuento = DBFunctions.SingleData("SELECT DVIS_VALODESC FROM DVISITADIARIACLIENTES fetch first 1 rows only");
                }
                catch
                {
                    Utils.MostrarAlerta(Response, " NO SE HA creado el campo para el valor del descuento de la Cotización. Llamar a eCAS ..!!!");
                    Page.Visible = false;
                    return;
                    // ejecutar esta sentencia en todas las DBs para aplicar esta actualizacion
                    // ALTER TABLE DVISITADIARIACLIENTES ADD COLUMN DVIS_VALODESC DECIMAL (14,2) default 0;
                }

                string validaCotizacion = null;
                try
                {
                    validaCotizacion = DBFunctions.SingleData("select cveh_coticlieexis || CVEH_DIASCOTI from CVEHICULOS");
                }
                catch
                {
                    Utils.MostrarAlerta(Response, "No se ha definido el parámetro de los días de vigencia de la cotización, llamar a eCAS - CVEH_COTICLIEXIS, CVEH_DIASCOTI ");
                    Page.Visible = false;
                    return;
                    /*  alter TABLE DBXSCHEMA.CVEHICULOS add column CVEH_DIASCOTI  integer  DEFAULT 30;
                        alter TABLE DBXSCHEMA.CVEHICULOS add column CVEH_COTICLIEEXIS  CHAR(1) DEFAULT 'N';
                    
                        ALTER TABLE DBXSCHEMA.CVEHICULOS ADD CONSTRAINT FK_CVEH_COTISINO FOREIGN KEY (CVEH_COTICLIEEXIS)
                                REFERENCES DBXSCHEMA.TRESPUESTASINO (TRES_SINO)
                                ON UPDATE RESTRICT
                                ON DELETE RESTRICT;

                        COMMENT ON COLUMN DBXSCHEMA.CVEHICULOS.CVEH_DIASCOTI IS 'Dias Validez Cotizaciones';
                        COMMENT ON COLUMN DBXSCHEMA.CVEHICULOS.CVEH_COTICLIEEXIS IS 'Bloquea Cotizacion a Cliente en Proceos con otro Asesor?';
                    */

                }


                ddlTipoVehiculo.SelectedValue = "N";
                DatasToControls bind = new DatasToControls();

                bind.PutDatasIntoDropDownList(ddlPrefijo, string.Format(Documento.DOCUMENTOSTIPO, "CV"));

                ddlPrefijo_SelectedIndexChanged(this, null);

                bind.PutDatasIntoDropDownList(ddlVendedorAutenticacion, AMS.Vehiculos.Vendedores.VENDEDORESVEHICULOS);
                ddlVendedor.Items.Insert(0, new ListItem("Seleccione el Vendedor...", ""));

                bind.PutDatasIntoDropDownList(ddlMedio, "select PCLAS_CODIGOVENTA,PCLAS_VENTADESCRIP from DBXSCHEMA.PCLASEVENTAVEHICULO ORDER BY PCLAS_VENTADESCRIP");
                ddlMedio.Items.Insert(0, new ListItem("Seleccione el Medio...", ""));

                //bind.PutDatasIntoDropDownList(ddlTipoCliente, "select PCLI_TIPOCLIE, PCLI_DESCRIPCION from DBXSCHEMA.PCLIENTE order by PCLI_TIPOCLIE");
                bind.PutDatasIntoDropDownList(ddlTipoCliente, "SELECT * from dbxschema.tserviciovehiculo ORDER BY tser_tiposerv;");
                ddlTipoCliente.Items.Insert(0, new ListItem("Seleccione el Tipo...", ""));

                bind.PutDatasIntoDropDownList(ddlcolor, "select PCOL_CODIGO, PCOL_DESCRIPCION from DBXSCHEMA.PCOLOR where pcol_activo IN ('S', 'SI') order by PCOL_descripcion");
                ddlcolor.Items.Insert(0, new ListItem("Seleccione el color...", ""));

                bind.PutDatasIntoDropDownList(ddlProspecto, "select PPRO_CODIGO,PPRO_DESCP from DBXSCHEMA.PPROSPECTO  where test_estado=1");
                ddlProspecto.Items.Insert(0, new ListItem("Seleccione el Prospecto...", ""));

                bind.PutDatasIntoDropDownList(ddlTipoVehiculo, "select TCLA_CLASE, TCLA_NOMBRE FROM DBXSCHEMA.TCLASEVEHICULO ORDER BY TCLA_CLASE");
                //ddlTipoVehiculo.Items.Insert(0, new ListItem("Seleccione la Clase de Vehículo ...", ""));

                bind.PutDatasIntoDropDownList(ddlFinanciera, "select m.mnit_nit, m.mnit_nombres concat ' ' concat m.mnit_apellidos from pfinanciera pf, mnit m where pf.mnit_nit = m.mnit_nit");
                ddlFinanciera.Items.Insert(0, new ListItem("Seleccione la Entidad financiera...", ""));

                if (ddlTipoVehiculo.SelectedValue == "N")
                {
                    bind.PutDatasIntoDropDownList(ddlCatalogoVehiculo, AMS.Vehiculos.CatalogoVehiculos.CATALOGOVEHICULOSLISTA);
                    ddlCatalogoVehiculo.Items.Insert(0, new ListItem("Seleccione el Catálogo del Vehículo...", ""));
                }
                else
                {
                    bind.PutDatasIntoDropDownList(ddlCatalogoVehiculo, "select MC.PCAT_CODIGO, select MC.MCAT_ANOMODE || ' - ' || MCAT_PLACA || ' - ' || PCOL_DESCRIPCION FROM MVEHICULO MV, MCATALOGOVEHICULO MC, PCOLOR PC WHERE MV.TEST_TIPOESTA < 30 and mv.tcla_codigo = 'U' AND MC.MCAT_VIN = MV.MCAT_VIN AND PC.PCOL_CODIGO = MC.PCOL_CODIGO");
                    ddlCatalogoVehiculo.Items.Insert(0, new ListItem("Seleccione el Vehículo...", ""));
                }
                if (ddlTipoVehiculo.SelectedValue == "N")
                {
                    bind.PutDatasIntoDropDownList(ddlModelo, "select PANO_ANO,PANO_DETALLE from DBXSCHEMA.PANO order by PANO_ANO DESC");
                    ddlModelo.Items.Insert(0, new ListItem("Seleccione el Año Modelo...", ""));
                }
                else
                    bind.PutDatasIntoDropDownList(ddlModelo, "select PANO_ANO,PANO_DETALLE from DBXSCHEMA.PANO order by PANO_ANO DESC");

                bind.PutDatasIntoDropDownList(ddlTipoContacto, "SELECT presulc_secuencia, presulc_descripcion FROM dbxschema.presultadocontacto order by 1");
                ddlTipoContacto.Items.Insert(0, new ListItem("Seleccione resultado del Contacto...", ""));

                
                ddlVendedor.Items.Insert(0, new ListItem("Seleccione el Vendedor...", ""));
                if(Request.QueryString["cod_vend"] != null)
                {
                    bind.PutDatasIntoDropDownList(ddlVendedor, "SELECT pven_codigo, pven_nombre FROM dbxschema.pvendedor WHERE tvend_codigo IN('VV', 'TT') and pven_vigencia = 'V' and pven_codigo = '" + Request.QueryString["cod_vend"] + "' ORDER BY pven_nombre");
                    //ddlVendedor.SelectedValue = Request.QueryString["cod_vend"];
                    //ddlVendedor.Enabled = false;
                    
                }
                else
                    bind.PutDatasIntoDropDownList(ddlVendedor, Vendedores.VENDEDORESVEHICULOS);

                ddlProximoContacto.Items.Add(new ListItem("Seleccione...", ""));
                ddlProximoContacto.Items.Add("SI");
                ddlProximoContacto.Items.Add("NO");


                txtFechaContacto.Text = InterpretarValorNuloAMostrar(DateTime.Now);

                Preparar_Tabla_Elementos();
                Binding_Grilla();

                datosContacto = new DataTable();
                ViewState["DS_CONTACTOS"] = datosContacto;
                NumeroContactos = 0;

                Modo = Request.QueryString["mod"];
                //llama el modo                      
              

                #region Mostrar Mensaje Operacion Satisfactoria
                string procesoSatisfactorio = Request.QueryString["proSat"];

                if (procesoSatisfactorio != null)
                {
                    switch (Modo)
                    {
                        case ModoSeguimientoDiario.COTIZACION:
                            if (Convert.ToBoolean(procesoSatisfactorio))
                            {
                                Utils.MostrarAlerta(Response, "La cotización fue creada satisfactoriamente con el número " + Request.QueryString["num"] + ".");
                                FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                                formatoRecibo.Prefijo = Request.QueryString["prf"];
                                formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prf"] + "'");
                                formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["num"]);
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                            }
                            else
                                Utils.MostrarAlerta(Response, "La cotización NO pudo ser creada.");
                            break;
                        case ModoSeguimientoDiario.CONTACTO:
                            if (Convert.ToBoolean(procesoSatisfactorio))
                                Utils.MostrarAlerta(Response, "La información de seguimiento del cliente fue actualizada satisfactoriamente.");
                            else
                                Utils.MostrarAlerta(Response, "La información de seguimiento del cliente no pudo ser actualizada.");
                            break;
                    }
                }
                #endregion

                CrearTablas();
                //Cargar vendedor anterior
                if (Session["PREV_VENDEDOR"] != null && Session["PREV_PASSWORD"] != null)
                {
                    CargarVendedor(Session["PREV_VENDEDOR"].ToString(), Session["PREV_PASSWORD"].ToString());
                }
                strCookie = "var d=new Date();document.cookie='mytab1tabber=0;expires='+d.toGMTString()+';' + ';';";

                prepararTablaVehiculos();
            }
            else//postback 
            {
                if (Session["tablaElementos"] == null)
                {
                    this.Preparar_Tabla_Elementos();
                    this.Binding_Grilla();
                }
                else
                {
                    tablaElementos = (DataTable)Session["tablaElementos"];
                    Session["tablaElementos"] = tablaElementos;
                }
                dtPagos = (DataTable)ViewState["DT_PAGOS"];
                //     PrecioTotal = Venta;
                datosContacto = (DataTable)ViewState["DS_CONTACTOS"];
            }
            //Desicion de entrada

            if (Modo == "COTIZACION")
            {
                
                //Response.Write("<script language:javascript>alert('COTIZACION');</script>");
                NumeroContactos = 0;
                pAutenticacionUsuario.Visible = false;
                pContactosDisponibles.Visible = false;
                pIngresoDatos.Visible = true;
            }
            else if (Modo == "CONTACTO")
            {
                if(!IsPostBack && Request.QueryString["procVendedor"] != null)
                {
                    VendedorAutenticacion = Request.QueryString["procVendedor"];
                    cargarDDLContactosDisponibles();
                    pAutenticacionUsuario.Visible = false;
                    pContactosDisponibles.Visible = true;
                    pIngresoDatos.Visible = false;
                    Session["PREV_VENDEDOR"] = Request.QueryString["procVendedor"];
                }
                else if (Session["PREV_VENDEDOR"] != null)
                {
                    pAutenticacionUsuario.Visible = false;
                    pContactosDisponibles.Visible = true;
                    pIngresoDatos.Visible = true;
                }
                else
                {
                    pAutenticacionUsuario.Visible = true;
                    pContactosDisponibles.Visible = false;
                    pIngresoDatos.Visible = false;
                }
            }

            //else
            //{
            //    if (Modo == "CONTACTO")
            //    {
            //        //Response.Write("<script language:javascript>alert('CONTACTO');</script>");
            //        pAutenticacionUsuario.Visible = true;
            //        pContactosDisponibles.Visible = false;
            //        pIngresoDatos.Visible = false;
            //    }
            //}//8
            
            if (Session["tablaVehiculos"] == null)
                prepararTablaVehiculos();
            else
                tablaVehiculos = (DataTable)Session["tablaVehiculos"];
            //if (Session["descuentos"] == null)
            //    prepararTablaDescuentos();
            //else
            //    tablaDescuentos = (DataTable)Session["descuentos"];
            

        }
        protected void prepararTablaVehiculos()
        {
            tablaVehiculos = new DataTable();
            tablaVehiculos.Columns.Add("CLASE_VEHICULO", typeof(System.String));    //0
            tablaVehiculos.Columns.Add("CATALOGO_VEHICULO", typeof(System.String)); //1
            tablaVehiculos.Columns.Add("ANO_MODELO", typeof(System.String));        //2
            tablaVehiculos.Columns.Add("COLOR", typeof(System.String));             //3
            tablaVehiculos.Columns.Add("EQUIP_ADICIONAL", typeof(System.String));   //4
            //tablaVehiculos.Columns.Add("VALOR_IMPUESTO", typeof(System.String));  
            tablaVehiculos.Columns.Add("PRECIO_VENTA", typeof(System.Double));      //5
            tablaVehiculos.Columns.Add("VALOR_DESCUENTO", typeof(System.Double));   //6
            tablaVehiculos.Columns.Add("VALOR_NETO", typeof(System.Double));        //7
            tablaVehiculos.Columns.Add("PLACA", typeof(System.String));             //8
            tablaVehiculos.Columns.Add("VIN_VEHICULO", typeof(System.String));      //9
            tablaVehiculos.Columns.Add("OPCION_VEHICULO", typeof(System.String));   //10   // IMPORTANTE: opcion vehículo(en caso de nuevo), # Inventario(en caso de Usado)
            Session["tablaVehiculos"] = tablaVehiculos;
            dgServicios.DataSource = tablaVehiculos;
            dgServicios.ShowFooter = true;
            dgServicios.DataBind();
        }
        //protected void prepararTablaDescuentos()
        //{
        //    tablaDescuentos = new DataTable();
        //    tablaDescuentos = DBFunctions.Request(new DataSet(), IncludeSchema.NO, "SELECT PCAT_CODIGO, PPRE_DTOMAX, PANO_ANO FROM DBXSCHEMA.PPRECIOVEHICULO;").Tables[0];
        //    Session["descuentos"] = tablaDescuentos;
        //}

        private void CargarVendedor(string vendedorA, string passwordA)
        {
            ddlVendedorAutenticacion.SelectedIndex = ddlVendedorAutenticacion.Items.IndexOf(ddlVendedorAutenticacion.Items.FindByValue(vendedorA));
            Contrasena = passwordA;

            //bind.PutDatasIntoDropDownList(ddlContactosDisponibles, string.Format("select PDOC_CODIGO CONCAT '-' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) VALOR,'[' CONCAT PDOC_CODIGO CONCAT '-' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) CONCAT '] - ['CONCAT DVIS_NOMBRE CONCAT '] - ['CONCAT CHAR(DVIS_FECHPROXCONTACTO) CONCAT ']' DESCRIPCION from DBXSCHEMA.DVISITADIARIACLIENTES where DVIS_PROXCONTACTO = 'SI' AND PVEN_CODIGO = '{0}' order by DVIS_FECHPROXCONTACTO,DVIS_NOMBRE", VendedorAutenticacion));
            //ddlContactosDisponibles.Items.Insert(0, new ListItem("Seleccione el contacto a actualizar", ""));

            cargarDDLContactosDisponibles();

            pAutenticacionUsuario.Visible = false;
            pContactosDisponibles.Visible = true;
        }

        public void btnPagos_Click(object sender, System.EventArgs e)
        {
            double cuota = 0, numMeses, tasa, monto, interes, capital, saldo;
            try
            {
                numMeses = Convert.ToInt16(txtNumCuotas.Text);
                monto = Convert.ToDouble(txtValorFinanciacion.Text);
                tasa = Convert.ToDouble(txtTasaFinanciacion.Text);
                tasa = tasa / 100;
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el monto, el número de cuotas y la tasa.");
                return;
            }
            dtPagos.Rows.Clear();
            DataRow drPago;
            cuota = (monto * tasa) / (1 - Math.Pow(1 + tasa, -numMeses));
            saldo = monto;

            for (int n = 1; n <= numMeses; n++)
            {
                drPago = dtPagos.NewRow();
                interes = saldo * tasa;
                capital = cuota - interes;
                drPago["MCRED_NUMEPAGO"] = n;
                drPago["MCRED_CUOTA"] = Math.Round(cuota, 0);
                drPago["MCRED_INTERES"] = Math.Round(interes, 0);
                drPago["MCRED_CAPITAL"] = Math.Round(capital, 0);
                saldo = Math.Round(saldo - capital, 0);
                if (saldo < 0) saldo = 0;
                drPago["MCRED_SALDO"] = Math.Round(saldo, 0);
                dtPagos.Rows.Add(drPago);
            }

            Bind_Pagos();
        }

        private void Bind_Pagos()
        {
            ViewState["DT_PAGOS"] = dtPagos;
            dgrPagos.DataSource = dtPagos;
            dgrPagos.DataBind();
            dgrPagos.Visible = true;
        }
        protected void guardarCotizacion(object sender, EventArgs e)
        {
            if(!validarCamposVacios())
            {
                Utils.MostrarAlerta(Response, "Faltan campos por llenar, los campos que tenga un * son obligatorios. Revise por favor");
                return;
            }
            if(tablaVehiculos.Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se ha registrado ningún vehículo en la tabla de contizaciones.");
                return;
            }
            if (txtValorFinanciacion.Text != "" && txtNumCuotas.Text != "" && txtTasaFinanciacion.Text != "")
            {
                Double monto = Convert.ToDouble(txtValorFinanciacion.Text);
                NumeroCuotasFin = Convert.ToInt16(txtNumCuotas.Text);
                PorcentajeTasaFin = Convert.ToDouble(txtTasaFinanciacion.Text);
                CuotaFin = (monto * (PorcentajeTasaFin / 100)) / (1 - Math.Pow(1 + (PorcentajeTasaFin / 100), -NumeroCuotasFin));
                CuotaFin = Math.Round(CuotaFin, 0);
            }
            else
            {
                CuotaFin = 0;
                NumeroCuotasFin = 0;
                PorcentajeTasaFin = 0;
            }

            //Grabar.Attributes.Add("OnClientClick", "return apagarBotonGrabar();");
            //Grabar.Attributes.Add("OnClientClick", "this.enabled = 'false';");
            bool esCoti = false;
            ArrayList insertNit = new ArrayList();
            insertNit.Add("INSERT INTO DBXSCHEMA.MNIT (SELECT * FROM DBXSCHEMA.MNITCOTIZACION WHERE MNIT_NIT = '" + NitCliente + "');");
            switch (Modo)
            {
                case ModoSeguimientoDiario.COTIZACION:
                    {
                        DataSet tablaMnit = new DataSet();
                        //string codigoExpNit = "11001";
                        DBFunctions.Request(tablaMnit, IncludeSchema.NO, "select * from mnit where mnit_nit = '" + NitCliente + "';" + "SELECT * FROM MNITCOTIZACION WHERE MNIT_NIT = '" + NitCliente + "';");
                        bool cotizacionBorrada = false;


                        if (tablaMnit.Tables[0].Rows.Count > 0 && tablaMnit.Tables[1].Rows.Count > 0)
                        {
                            DBFunctions.NonQuery(@"DELETE FROM MNITCOTIZACION WHERE MNIT_NIT = '" + NitCliente + "'");
                            cotizacionBorrada = true;
                        }
                        if (tablaMnit.Tables[0].Rows.Count > 0 || tablaMnit.Tables[1].Rows.Count > 0)
                        {
                            //mnit
                            if (tablaMnit.Tables[0].Rows.Count > 0)
                            {
                                if (tablaMnit.Tables[0].Rows[0]["TNIT_TIPONIT"].ToString() == "N")
                                {
                                    DBFunctions.NonQuery("UPDATE MNIT SET MNIT_REPRESENTANTE = '" + RepresentanteLegal + "', MNIT_NITREPRESENTANTE = '" + NitRepresentanteLegal + "', MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");

                                }
                                else
                                    DBFunctions.NonQuery("UPDATE MNIT SET MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");
                            }
                            //mnitcotizacion
                            if (tablaMnit.Tables[1].Rows.Count > 0 && !cotizacionBorrada)
                            {
                                esCoti = true;
                                if (tablaMnit.Tables[1].Rows[0]["TNIT_TIPONIT"].ToString() == "N")
                                {
                                    DBFunctions.NonQuery("UPDATE MNITCOTIZACION SET MNIT_REPRESENTANTE = '" + RepresentanteLegal + "', MNIT_NITREPRESENTANTE = '" + NitRepresentanteLegal + "', MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");

                                }
                                else
                                    DBFunctions.NonQuery("UPDATE MNITCOTIZACION SET MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");
                            }
                        }
                        ArrayList sqlLista = new ArrayList();
                        int ultimoNumero = Convert.ToInt32(DBFunctions.SingleData("select coalesce(max(DVIS_NUMEVISI),0) + 1 from dbxschema.DVISITADIARIACLIENTES where pdoc_codigo='" + ddlPrefijo.SelectedValue + "'"));
                        if (ultimoNumero > Numero)
                            Numero = ultimoNumero;
                        sqlLista.Add(string.Format(@"INSERT INTO DBXSCHEMA.DVISITADIARIACLIENTES 
                        (
				        PDOC_CODIGO, 
				        DVIS_NUMEVISI,
				        DVIS_NOMBRE,
				        DVIS_TELEFIJO,
				        DVIS_TELEMOVIL,
				        DVIS_TELEOFICINA,
				        DVIS_EMAIL,
				        PCLAS_CODIGOVENTA,
				        PCLI_TIPOCLIE,
				        PPRO_CODIGO,
                        DVIS_VALOFINANCIACION,
                        DVIS_NUMECONTACTO,
				        DVIS_PROXCONTACTO,
				        DVIS_FECHPROXCONTACTO,
                        PRESULC_SECUENCIA,
				        PVEN_CODIGO,
                        DVIS_FECHPRIMCONTACTO,
                        DVIS_CUOTA,
                        DVIS_NUMCUOTA,
                        DVIS_PORCTASA,
                        PFIN_MNIT,
                        MNIT_NIT
                        )VALUES 
                        ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},DEFAULT,{16},{17},{18},{19},{20})",
                            ValidarValorCadena(Prefijo), // 0
                            ValidarValorCadena(Numero), // 1
                            ValidarValorCadena(Nombre), // 2
                            ValidarValorCadena(TelefonoFijo), // 3
                            ValidarValorCadena(TelefonoMovil), // 4
                            ValidarValorCadena(TelefonoOficina), // 5
                            ValidarValorCadena(Email), // 6
                            ValidarValorCadena(Medio), // 7
                            ValidarValorCadena(TipoCliente), // 8
                            ValidarValorCadena(Prospecto), // 9
                            ValidarValorCadena(ValorFinanciacion), // 10
                            ValidarValorCadena(NumeroContactos), // 11
                            ValidarValorCadena(ProximoContacto), // 12
                            ValidarValorCadena(FechaProximoContacto), // 13
                            ValidarValorCadena(ResultadoContacto), // 14
                            ValidarValorCadena(Vendedor), // 15
                            ValidarValorCadena(CuotaFin),//16
                            ValidarValorCadena(NumeroCuotasFin),//17
                            ValidarValorCadena(PorcentajeTasaFin),//18
                            ValidarValorCadena(NitFinaciera),//19
                            ValidarValorCadena(NitCliente)//20
                            ));
                        agregarPedidosCotizacion(ref sqlLista);
                        

                        sqlLista.Add(string.Format("UPDATE DBXSCHEMA.PDOCUMENTO SET PDOC_ULTIDOCU = " + Numero + " WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "'", Prefijo));
                        //Items
                        sqlLista.Add("DELETE FROM DVISITADIARIACLIENTESITEMS WHERE PDOC_CODIGO=" + ValidarValorCadena(Prefijo) + " AND DVIS_NUMEVISI=" + ValidarValorCadena(Numero) + ";");
                        for (int n = 0; n < tablaElementos.Rows.Count; n++)
                            sqlLista.Add("INSERT INTO DVISITADIARIACLIENTESITEMS VALUES(" + ValidarValorCadena(Prefijo) + "," + ValidarValorCadena(Numero) + ",'" + tablaElementos.Rows[n][0].ToString() + "'," + tablaElementos.Rows[n][2].ToString() + "," + tablaElementos.Rows[n][3].ToString() + ");");
                        bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
                        if (procesoSatisfactorio)
                            Response.Redirect(indexPage + "?process=Vehiculos.SeguimientoDiario&mod=" + ModoSeguimientoDiario.COTIZACION + "&proSat=" + procesoSatisfactorio.ToString() + "&num=" + Numero.ToString() + "&prf=" + ddlPrefijo.SelectedValue, false);
                        else
                        {
                            lError.Text = DBFunctions.exceptions;
                            return;
                        }
                    }
                    break;
                case ModoSeguimientoDiario.CONTACTO:
                    {
                        ArrayList sqlLista = new ArrayList();
                        sqlLista.Add(string.Format(@"UPDATE DBXSCHEMA.DVISITADIARIACLIENTES SET
				        DVIS_NOMBRE = {0},
				        DVIS_TELEFIJO = {1},
				        DVIS_TELEMOVIL = {2},
				        DVIS_TELEOFICINA = {3},
				        DVIS_EMAIL = {4},
				        PCLAS_CODIGOVENTA = {5},
				        PPRO_CODIGO = {6}, " +
                        /*PCAT_CODIGO = {7},
				        DVIS_EQUIPAMENTO = {8},
				        DVIS_MODELO = {9},
				        DVIS_VENTA = {10},
				        DVIS_PRECTOTAL = {11},*/
                        @"DVIS_VALOFINANCIACION = {7},
				        DVIS_NUMECONTACTO = {8},
				        DVIS_PROXCONTACTO = {9},
				        DVIS_FECHPROXCONTACTO = {10},
				        PRESULC_SECUENCIA = {11},
				        PVEN_CODIGO = {12},
               	        PCLI_TIPOCLIE = {13} " +
                        //TCLA_CLASVEHI = {14}, 
             	        //PCOL_CODIGO = {22},
                        //DVIS_VALODESC = {14}
                        @"WHERE
				        PDOC_CODIGO = {14} AND
				        DVIS_NUMEVISI = {15}",
                        ValidarValorCadena(Nombre),                 // 0
                        ValidarValorCadena(TelefonoFijo),           // 1
                        ValidarValorCadena(TelefonoMovil),          // 2
                        ValidarValorCadena(TelefonoOficina),        // 3
                        ValidarValorCadena(Email),                  // 4
                        ValidarValorCadena(Medio),                  // 5
                        ValidarValorCadena(Prospecto),              // 6
                        /*ValidarValorCadena(CatalogoVehiculo),     // 7
                        ValidarValorCadena(Equipamento),            // 8
                        ValidarValorCadena(Modelo),                 // 9
                        ValidarValorCadena(Venta),                  // 10
                        ValidarValorCadena(PrecioTotal),            // 11*/
                        ValidarValorCadena(ValorFinanciacion),      // 12
                        ValidarValorCadena(NumeroContactos),        // 13
                        ValidarValorCadena(ProximoContacto),        // 14
                        ValidarValorCadena(FechaProximoContacto),   // 15
                        ValidarValorCadena(ResultadoContacto),      // 16
                        ValidarValorCadena(Vendedor),               // 17
                        ValidarValorCadena(TipoCliente),            // 18
                        //ValidarValorCadena(TipoVehiculo),           // 19                       
                        ValidarValorCadena(Prefijo),                // 20
                        ValidarValorCadena(Numero)                  // 21
                        //ValidarValorCadena(Color),                  // 22
                        //ValidarValorCadena(ValorDescuento)          // 23
                        ));
                        agregarPedidosCotizacion(ref sqlLista);
                        //Items
                        sqlLista.Add("DELETE FROM DVISITADIARIACLIENTESITEMS WHERE PDOC_CODIGO=" + ValidarValorCadena(Prefijo) + " AND DVIS_NUMEVISI=" + ValidarValorCadena(Numero) + ";");
                        for (int n = 0; n < tablaElementos.Rows.Count; n++)
                            sqlLista.Add("INSERT INTO DVISITADIARIACLIENTESITEMS VALUES(" + ValidarValorCadena(Prefijo) + "," + ValidarValorCadena(Numero) + ",'" + tablaElementos.Rows[n][0].ToString() + "'," + tablaElementos.Rows[n][2].ToString() + "," + tablaElementos.Rows[n][3].ToString() + ");");
                        bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
                        if (procesoSatisfactorio)
                            Response.Redirect(indexPage + "?process=Vehiculos.SeguimientoDiario&mod=" + ModoSeguimientoDiario.CONTACTO + "&proSat=" + procesoSatisfactorio.ToString(), false);
                        else
                        {
                            lError.Text = DBFunctions.exceptions;
                            return;
                        }
                            
                    }
                    break;
            }
            agregarContacto();

            /*string opcionVeh = "";
            if (ddlOpcionVeh.Visible == true && ddlOpcionVeh.SelectedItem.Text != "0")
            {
                opcionVeh = "&opcionVeh=" + ddlOpcionVeh.SelectedValue.ToString();
            }*/

            if (ddlTipoContacto.SelectedItem.ToString() == "COMPRO" || ddlTipoContacto.SelectedItem.ToString().Contains("CIERRE"))  // pasada de parámetros para crear el pedido
            {
                if (esCoti)
                {
                    string resul = "";
                    resul = insertarEnMNit(insertNit);
                    //esto sólo para saber si sí se insertó el registro.
                }
                string texto = "";
                if (tablaVehiculos.Rows[0]["CLASE_VEHICULO"].ToString() == "U")
                    texto = "?process=Vehiculos.PedidoClientesFormulario&compra=1&tipoVeh="
                        + tablaVehiculos.Rows[0]["CLASE_VEHICULO"] + "&catalogo=" + tablaVehiculos.Rows[0]["VIN_VEHICULO"] + "&color=" + tablaVehiculos.Rows[0]["COLOR"].ToString().Split('[')[0].Trim() + "&modelo="
                        + tablaVehiculos.Rows[0]["ANO_MODELO"] + "&tipoVenta=" + TipoCliente + "&claseVenta=" + Medio + "&precioVenta=" + tablaVehiculos.Rows[0]["PRECIO_VENTA"] + "&nitCliente=" + NitCliente + "&valorDescuento="
                        + tablaVehiculos.Rows[0]["VALOR_DESCUENTO"] + "&vendedor=" + Vendedor + "&numCot=" + Numero.ToString() + "&prfCot=" + ddlPrefijo.SelectedValue /*+ opcionVeh*/ + "&email=" + Email;
                else
                    if (tablaVehiculos.Rows[0]["OPCION_VEHICULO"].ToString() != "ST")
                    texto = "?process=Vehiculos.PedidoClientesFormulario&compra=1&tipoVeh="
                        + tablaVehiculos.Rows[0]["CLASE_VEHICULO"] + "&catalogo=" + tablaVehiculos.Rows[0]["CATALOGO_VEHICULO"] + "&color=" + tablaVehiculos.Rows[0]["COLOR"].ToString().Split('[')[0].Trim() + "&modelo="
                        + tablaVehiculos.Rows[0]["ANO_MODELO"] + "&tipoVenta=" + TipoCliente + "&claseVenta=" + Medio + "&precioVenta=" + tablaVehiculos.Rows[0]["PRECIO_VENTA"] + "&nitCliente=" + NitCliente + "&valorDescuento="
                        + tablaVehiculos.Rows[0]["VALOR_DESCUENTO"] + "&vendedor=" + Vendedor + "&numCot=" + Numero.ToString() + "&prfCot=" + ddlPrefijo.SelectedValue /*+ opcionVeh*/ + "&email=" + Email + "&opcionVeh=" + tablaVehiculos.Rows[0]["OPCION_VEHICULO"];
                else
                    texto = "?process=Vehiculos.PedidoClientesFormulario&compra=1&tipoVeh="
                        + tablaVehiculos.Rows[0]["CLASE_VEHICULO"] + "&catalogo=" + tablaVehiculos.Rows[0]["CATALOGO_VEHICULO"] + "&color=" + tablaVehiculos.Rows[0]["COLOR"].ToString().Split('[')[0].Trim() + "&modelo="
                        + tablaVehiculos.Rows[0]["ANO_MODELO"] + "&tipoVenta=" + TipoCliente + "&claseVenta=" + Medio + "&precioVenta=" + tablaVehiculos.Rows[0]["PRECIO_VENTA"] + "&nitCliente=" + NitCliente + "&valorDescuento="
                        + tablaVehiculos.Rows[0]["VALOR_DESCUENTO"] + "&vendedor=" + Vendedor + "&numCot=" + Numero.ToString() + "&prfCot=" + ddlPrefijo.SelectedValue /*+ opcionVeh*/ + "&email=" + Email;

                Response.Redirect(indexPage + texto);
            }
            //else estoy comentando esto porque aparenta ser sólo un consumo de memoria inutil.
            //{
            //    CargarVendedor(Vendedor, Contrasena); //en vez de vendedor, estaba la variable vendedorAutenticación.
            //}
        }
         protected void agregarPedidosCotizacion(ref ArrayList sqlString)
        {
            for (int i = 0; i < tablaVehiculos.Rows.Count; i++)
            {
                if(tablaVehiculos.Rows[i]["CLASE_VEHICULO"].ToString() == "U")
                {
                    sqlString.Add(string.Format("INSERT INTO DVISITADIARIACLIENTESVEHICULOS (PDOC_CODIGO,DVIS_NUMEVISI,PCAT_CODIGO,TCLA_CLASVEHI,DVIV_EQUIPAMENTO,DVIV_MODELO,MVEH_INVENTARIO,PCOL_CODIGO,DVIV_VENTA,DVIV_DESCUENTO,DVIV_PRECTOTAL) VALUES "
                    + "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                    ValidarValorCadena(Prefijo), // 0
                    ValidarValorCadena(Numero), // 1
                    "'" + tablaVehiculos.Rows[i]["CATALOGO_VEHICULO"] + "'",
                    "'" + tablaVehiculos.Rows[i]["CLASE_VEHICULO"] + "'",
                    "'" + tablaVehiculos.Rows[i]["EQUIP_ADICIONAL"] + "'",
                    "'" + tablaVehiculos.Rows[i]["ANO_MODELO"] + "'",
                     tablaVehiculos.Rows[i]["OPCION_VEHICULO"],//inventario
                    //"'" + tablaVehiculos.Rows[i]["PLACA"] + "'",
                    "'" + tablaVehiculos.Rows[i]["COLOR"].ToString().Split('[')[0].Trim() + "'",
                    tablaVehiculos.Rows[i]["PRECIO_VENTA"],
                    tablaVehiculos.Rows[i]["VALOR_DESCUENTO"],
                    tablaVehiculos.Rows[i]["VALOR_NETO"]
                    ));
                }
                else
                {
                    sqlString.Add(string.Format("INSERT INTO DVISITADIARIACLIENTESVEHICULOS (PDOC_CODIGO,DVIS_NUMEVISI,PCAT_CODIGO,TCLA_CLASVEHI,DVIV_EQUIPAMENTO,DVIV_MODELO,PCOL_CODIGO,DVIV_VENTA,DVIV_DESCUENTO,DVIV_PRECTOTAL,POPC_OPCIVEHI) VALUES "
                    + "({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10})",
                    ValidarValorCadena(Prefijo), // 0
                    ValidarValorCadena(Numero), // 1
                    "'" + tablaVehiculos.Rows[i]["CATALOGO_VEHICULO"] + "'", //2
                    "'" + tablaVehiculos.Rows[i]["CLASE_VEHICULO"] + "'", //3
                    "'" + tablaVehiculos.Rows[i]["EQUIP_ADICIONAL"] + "'", //4
                    "'" + tablaVehiculos.Rows[i]["ANO_MODELO"] + "'", //5
                    "'" + tablaVehiculos.Rows[i]["COLOR"].ToString().Split('[')[0].Trim() + "'", //6
                    tablaVehiculos.Rows[i]["PRECIO_VENTA"], //7
                    tablaVehiculos.Rows[i]["VALOR_DESCUENTO"], //8
                    tablaVehiculos.Rows[i]["VALOR_NETO"], //9 
                    "'" + tablaVehiculos.Rows[i]["OPCION_VEHICULO"] + "'" //10
                    ));
                }
                
            }
        }

        /*public void Grabar_Click(object sender, System.EventArgs e)
        {
            if (!Page.IsValid) return;
            if (ValidarValorCadena(Color)==null)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar un COLOR ...");
                return;
            }
            if (txtObservacionesContacto.Text == null || txtObservacionesContacto.Text == "")
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el comentario del Contacto ...");
                return;
            }
            if ((txtTelefonoFijo.Text == null || txtTelefonoFijo.Text == "") || ((txtTelefonoOficina.Text == null || txtTelefonoOficina.Text == "") && (txtTelefonoMovil.Text == null || txtTelefonoMovil.Text == "")))
            {
                Utils.MostrarAlerta(Response, "Debe ingresar dirección, un numero fijo o movil del Contacto ...");
                return;
            }
            if (txtEmail.Text == null || txtEmail.Text == "")
            {
                Utils.MostrarAlerta(Response, "Debe ingresar E_mail o la palabra NO TIENE ...");
                return;
            }

            if (txtDescuento.Text == "")
            {
                txtDescuento.Text = "0";
            }
         

            if (txtValorFinanciacion.Text != "" && txtNumCuotas.Text != "" && txtTasaFinanciacion.Text != "")
            {
                Double monto = Convert.ToDouble(txtValorFinanciacion.Text);
                NumeroCuotasFin = Convert.ToInt16(txtNumCuotas.Text);
                PorcentajeTasaFin = Convert.ToDouble(txtTasaFinanciacion.Text);
                CuotaFin = (monto * (PorcentajeTasaFin / 100)) / (1 - Math.Pow(1 + (PorcentajeTasaFin / 100), -NumeroCuotasFin));
                CuotaFin = Math.Round(CuotaFin, 0);
            }
            else
            {
                CuotaFin = 0;
                NumeroCuotasFin = 0;
                PorcentajeTasaFin = 0;
            }

            Grabar.Attributes.Add("OnClientClick", "return apagarBotonGrabar();");
            Grabar.Attributes.Add("OnClientClick", "this.enabled = 'false';");
            bool esCoti = false;
            ArrayList insertNit = new ArrayList();
            insertNit.Add("INSERT INTO DBXSCHEMA.MNIT (SELECT * FROM DBXSCHEMA.MNITCOTIZACION WHERE MNIT_NIT = '" + NitCliente + "');");
            switch (Modo)
            {
                case ModoSeguimientoDiario.COTIZACION:
                {
                    DataSet tablaMnit = new DataSet();
                    //string codigoExpNit = "11001";
                    DBFunctions.Request(tablaMnit, IncludeSchema.NO, "select * from mnit where mnit_nit = '" + NitCliente + "';" + "SELECT * FROM MNITCOTIZACION WHERE MNIT_NIT = '" + NitCliente + "';");
                    bool cotizacionBorrada = false;
                    
                    
                    if (tablaMnit.Tables[0].Rows.Count > 0 && tablaMnit.Tables[1].Rows.Count > 0)
                    {
                        DBFunctions.NonQuery(@"DELETE FROM MNITCOTIZACION WHERE MNIT_NIT = '" + NitCliente + "'");
                            cotizacionBorrada = true;
                    }
                    if (tablaMnit.Tables[0].Rows.Count > 0 || tablaMnit.Tables[1].Rows.Count > 0)
                    {
                            //mnit
                        if (tablaMnit.Tables[0].Rows.Count > 0)
                        {
                            if (tablaMnit.Tables[0].Rows[0]["TNIT_TIPONIT"].ToString() == "N")
                            {
                                DBFunctions.NonQuery("UPDATE MNIT SET MNIT_REPRESENTANTE = '" + RepresentanteLegal + "', MNIT_NITREPRESENTANTE = '" + NitRepresentanteLegal + "', MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");
                                
                            }
                            else
                                DBFunctions.NonQuery("UPDATE MNIT SET MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");
                        }
                        //mnitcotizacion
                        if (tablaMnit.Tables[1].Rows.Count > 0 && !cotizacionBorrada)
                        {
                                esCoti = true;
                                if (tablaMnit.Tables[1].Rows[0]["TNIT_TIPONIT"].ToString() == "N")
                            {
                                DBFunctions.NonQuery("UPDATE MNITCOTIZACION SET MNIT_REPRESENTANTE = '" + RepresentanteLegal + "', MNIT_NITREPRESENTANTE = '" + NitRepresentanteLegal + "', MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");
                                
                            }
                            else
                                DBFunctions.NonQuery("UPDATE MNITCOTIZACION SET MNIT_DIRECCION = '" + TelefonoFijo + "', MNIT_TELEFONO = '" + TelefonoOficina + "', MNIT_CELULAR = '" + TelefonoMovil + "', MNIT_EMAIL = '" + Email + "' WHERE MNIT_NIT = '" + NitCliente + "'");
                        }
                    }
                    ArrayList sqlLista = new ArrayList();
                    int ultimoNumero = Convert.ToInt32(DBFunctions.SingleData("select coalesce(max(DVIS_NUMEVISI),0) + 1 from dbxschema.DVISITADIARIACLIENTES where pdoc_codigo='" + ddlPrefijo.SelectedValue + "'"));
                    if (ultimoNumero > Numero)
                        Numero = ultimoNumero;
                    sqlLista.Add(string.Format(@"INSERT INTO DBXSCHEMA.DVISITADIARIACLIENTES 
                        (
				        PDOC_CODIGO, 
				        DVIS_NUMEVISI,
				        DVIS_NOMBRE,
				        DVIS_TELEFIJO,
				        DVIS_TELEMOVIL,
				        DVIS_TELEOFICINA,
				        DVIS_EMAIL,
				        PCLAS_CODIGOVENTA,
				        PCLI_TIPOCLIE,
				        PPRO_CODIGO,
                        TCLA_CLASVEHI,
                        PCAT_CODIGO,
				        DVIS_EQUIPAMENTO,
				        DVIS_MODELO,
				        DVIS_VENTA,
                        DVIS_PRECTOTAL,
                        DVIS_VALOFINANCIACION,
                        DVIS_NUMECONTACTO,
				        DVIS_PROXCONTACTO,
				        DVIS_FECHPROXCONTACTO,
                        PRESULC_SECUENCIA,
				        PVEN_CODIGO,
				        PCOL_CODIGO,
                        DVIS_FECHPRIMCONTACTO,
                        DVIS_CUOTA,
                        DVIS_NUMCUOTA,
                        DVIS_PORCTASA,
                        PFIN_MNIT,
                        MNIT_NIT,
                        DVIS_VALODESC
                        )VALUES 
                        ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},DEFAULT,{23},{24},{25},{26},{27},{28})",
                        ValidarValorCadena(Prefijo), // 0
                        ValidarValorCadena(Numero), // 1
                        ValidarValorCadena(Nombre), // 2
                        ValidarValorCadena(TelefonoFijo), // 3
                        ValidarValorCadena(TelefonoMovil), // 4
                        ValidarValorCadena(TelefonoOficina), // 5
                        ValidarValorCadena(Email), // 6
                        ValidarValorCadena(Medio), // 7
                        ValidarValorCadena(TipoCliente), // 8
                        ValidarValorCadena(Prospecto), // 9
                        ValidarValorCadena(TipoVehiculo), // 10
                        ValidarValorCadena(CatalogoVehiculo), // 11
                        ValidarValorCadena(Equipamento), // 12
                        ValidarValorCadena(Modelo), // 13
                        ValidarValorCadena(Venta), // 14
                        ValidarValorCadena(PrecioTotal), // 15
                        ValidarValorCadena(ValorFinanciacion), // 16
                        ValidarValorCadena(NumeroContactos), // 17
                        ValidarValorCadena(ProximoContacto), // 18
                        ValidarValorCadena(FechaProximoContacto), // 19
                        ValidarValorCadena(ResultadoContacto), // 20
                        ValidarValorCadena(Vendedor), // 21
                        ValidarValorCadena(Color),//22
                        ValidarValorCadena(CuotaFin),//23
                        ValidarValorCadena(NumeroCuotasFin),//24
                        ValidarValorCadena(PorcentajeTasaFin),//25
                        ValidarValorCadena(NitFinaciera),//26
                        ValidarValorCadena(NitCliente),//27
                        ValidarValorCadena(ValorDescuento) // 28
                        ));

                        sqlLista.Add(string.Format("UPDATE DBXSCHEMA.PDOCUMENTO SET PDOC_ULTIDOCU = "+ Numero +" WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "'", Prefijo));
                        //Items
                        sqlLista.Add("DELETE FROM DVISITADIARIACLIENTESITEMS WHERE PDOC_CODIGO=" + ValidarValorCadena(Prefijo) + " AND DVIS_NUMEVISI=" + ValidarValorCadena(Numero) + ";");
                        for (int n = 0; n < tablaElementos.Rows.Count; n++)
                            sqlLista.Add("INSERT INTO DVISITADIARIACLIENTESITEMS VALUES(" + ValidarValorCadena(Prefijo) + "," + ValidarValorCadena(Numero) + ",'" + tablaElementos.Rows[n][0].ToString() + "'," + tablaElementos.Rows[n][2].ToString() + "," + tablaElementos.Rows[n][3].ToString() + ");");
                        bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
                        if (procesoSatisfactorio)
                            Response.Redirect(indexPage + "?process=Vehiculos.SeguimientoDiario&mod=" + ModoSeguimientoDiario.COTIZACION + "&proSat=" + procesoSatisfactorio.ToString() + "&num=" + Numero.ToString() + "&prf=" + ddlPrefijo.SelectedValue, false);
                        else lError.Text = DBFunctions.exceptions;
                    }
                    break;
                case ModoSeguimientoDiario.CONTACTO:
                     {
                        ArrayList sqlLista = new ArrayList();
                        sqlLista.Add(string.Format(@"UPDATE DBXSCHEMA.DVISITADIARIACLIENTES SET
				        DVIS_NOMBRE = {0},
				        DVIS_TELEFIJO = {1},
				        DVIS_TELEMOVIL = {2},
				        DVIS_TELEOFICINA = {3},
				        DVIS_EMAIL = {4},
				        PCLAS_CODIGOVENTA = {5},
				        PPRO_CODIGO = {6},
				        PCAT_CODIGO = {7},
				        DVIS_EQUIPAMENTO = {8},
				        DVIS_MODELO = {9},
				        DVIS_VENTA = {10},
				        DVIS_PRECTOTAL = {11},
				        DVIS_VALOFINANCIACION = {12},
				        DVIS_NUMECONTACTO = {13},
				        DVIS_PROXCONTACTO = {14},
				        DVIS_FECHPROXCONTACTO = {15},
				        PRESULC_SECUENCIA = {16},
				        PVEN_CODIGO = {17},
               	        PCLI_TIPOCLIE = {18},
                        TCLA_CLASVEHI = {19},
             	        PCOL_CODIGO = {22},
                        DVIS_VALODESC = {23}
				        WHERE
				        PDOC_CODIGO = {20} AND
				        DVIS_NUMEVISI = {21}",
                        ValidarValorCadena(Nombre),                 // 0
                        ValidarValorCadena(TelefonoFijo),           // 1
                        ValidarValorCadena(TelefonoMovil),          // 2
                        ValidarValorCadena(TelefonoOficina),        // 3
                        ValidarValorCadena(Email),                  // 4
                        ValidarValorCadena(Medio),                  // 5
                        ValidarValorCadena(Prospecto),              // 6
                        ValidarValorCadena(CatalogoVehiculo),       // 7
                        ValidarValorCadena(Equipamento),            // 8
                        ValidarValorCadena(Modelo),                 // 9
                        ValidarValorCadena(Venta),                  // 10
                        ValidarValorCadena(PrecioTotal),            // 11
                        ValidarValorCadena(ValorFinanciacion),      // 12
                        ValidarValorCadena(NumeroContactos),        // 13
                        ValidarValorCadena(ProximoContacto),        // 14
                        ValidarValorCadena(FechaProximoContacto),   // 15
                        ValidarValorCadena(ResultadoContacto),      // 16
                        ValidarValorCadena(Vendedor),               // 17
                        ValidarValorCadena(TipoCliente),            // 18
                        ValidarValorCadena(TipoVehiculo),           // 19                       
                        ValidarValorCadena(Prefijo),                // 20
                        ValidarValorCadena(Numero),                 // 21
                        ValidarValorCadena(Color),                  // 22
                        ValidarValorCadena(ValorDescuento)          // 23
                        ));
                        //Items
                        sqlLista.Add("DELETE FROM DVISITADIARIACLIENTESITEMS WHERE PDOC_CODIGO=" + ValidarValorCadena(Prefijo) + " AND DVIS_NUMEVISI=" + ValidarValorCadena(Numero) + ";");
                        for (int n = 0; n < tablaElementos.Rows.Count; n++)
                            sqlLista.Add("INSERT INTO DVISITADIARIACLIENTESITEMS VALUES(" + ValidarValorCadena(Prefijo) + "," + ValidarValorCadena(Numero) + ",'" + tablaElementos.Rows[n][0].ToString() + "'," + tablaElementos.Rows[n][2].ToString() + "," + tablaElementos.Rows[n][3].ToString() + ");");
                        bool procesoSatisfactorio = DBFunctions.Transaction(sqlLista);
                        if (procesoSatisfactorio)
                            Response.Redirect(indexPage + "?process=Vehiculos.SeguimientoDiario&mod=" + ModoSeguimientoDiario.CONTACTO + "&proSat=" + procesoSatisfactorio.ToString(), false);
                        else
                            lError.Text = DBFunctions.exceptions;
                    }
                    break;
            }
            agregarContacto();

            string opcionVeh = "";
            if (ddlOpcionVeh.Visible == true && ddlOpcionVeh.SelectedItem.Text != "0")
            {
                opcionVeh = "&opcionVeh=" + ddlOpcionVeh.SelectedValue.ToString();
            }

            if (ddlTipoContacto.SelectedItem.ToString() == "COMPRO" || ddlTipoContacto.SelectedItem.ToString().Contains("CIERRE"))  // pasada de parámetros para crear el pedido
            {
                if (esCoti)
                {
                    string resul = "";
                    resul = insertarEnMNit(insertNit);
                    //esto sólo para saber si sí se insertó el registro.
                }
                Response.Redirect(indexPage + "?process=Vehiculos.PedidoClientesFormulario&compra=1&tipoVeh=" + TipoVehiculo + "&catalogo=" + CatalogoVehiculo + "&color=" + Color + "&modelo=" + Modelo + "&tipoVenta=" + TipoCliente + "&claseVenta=" + Medio + "&precioVenta=" + Venta + "&nitCliente=" + NitCliente + "&valorDescuento=" + ValorDescuento + "&vendedor=" + Vendedor + "&numCot=" + Numero.ToString() + "&prfCot=" + ddlPrefijo.SelectedValue + opcionVeh + "&email=" + Email);
            }
            else
            {
                CargarVendedor(VendedorAutenticacion,Contrasena);
            }
            
        }*/
        

        protected void ddlProximoContacto_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (ProximoContacto == "SI")
            {
                cFechaProximoContacto.Enabled = true;
                FechaProximoContacto = DateTime.Now.AddDays(3);
            }
            else
                cFechaProximoContacto.Enabled = false;
        }

        protected void btnIngresar_Click(object sender, System.EventArgs e)
        {
            string contrasenaVendedor = DBFunctions.SingleData("select PVEN_CLAVE from DBXSCHEMA.PVENDEDOR WHERE PVEN_CODIGO='" + VendedorAutenticacion + "'");

            if (contrasenaVendedor == Contrasena)
            {
                cargarDDLContactosDisponibles();

            }
            else
            {
                Utils.MostrarAlerta(Response, "La contraseña es incorrecta para el usuario seleccionado.");
                return;
            }
            pAutenticacionUsuario.Visible = false;
            pContactosDisponibles.Visible = true;
            pIngresoDatos.Visible = false;

            Session["PREV_VENDEDOR"] = VendedorAutenticacion;
            Session["PREV_PASSWORD"] = Contrasena;

        }

        protected void ddlContactosDisponibles_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DataSet contacto = new DataSet();
            DBFunctions.Request(contacto, IncludeSchema.NO, string.Format(@"select 
                PDOC_CODIGO, DVIS_NUMEVISI,
                DVIS_NOMBRE, DVIS_TELEFIJO,
                DVIS_TELEMOVIL, DVIS_TELEOFICINA,
                DVIS_EMAIL, PCLAS_CODIGOVENTA,
                PPRO_CODIGO,DVIS_VALOFINANCIACION,DVIS_NUMECONTACTO,
                DVIS_PROXCONTACTO, DVIS_FECHPROXCONTACTO,
                PRESULC_SECUENCIA, PVEN_CODIGO,
                PCLI_TIPOCLIE, 
                DVIS_NUMCUOTA,
                DVIS_PORCTASA, PFIN_MNIT,
                MNIT_NIT
                FROM DBXSCHEMA.DVISITADIARIACLIENTES WHERE  PDOC_CODIGO='{0}' AND DVIS_NUMEVISI={1}", ContactosDisponiblesPrefijo, ContactosDisponiblesNumero));
            DBFunctions.Request(contacto, IncludeSchema.NO, string.Format(@"select DISTINCT
                PCAT_CODIGO,TCLA_CLASVEHI,
                DVIV_EQUIPAMENTO,DVIV_MODELO,PCOL_CODIGO,
                DVIV_VENTA,DVIV_DESCUENTO,
                DVIV_PRECTOTAL, POPC_OPCIVEHI FROM DVISITADIARIACLIENTESVEHICULOS WHERE  PDOC_CODIGO='{0}' AND DVIS_NUMEVISI={1}", ContactosDisponiblesPrefijo, ContactosDisponiblesNumero));
            //DBFunctions.Request(contacto, IncludeSchema.NO, string.Format(@"select 
            //PDOC_CODIGO, DVIS_NUMEVISI,
            //DVIS_NOMBRE, DVIS_TELEFIJO,
            //DVIS_TELEMOVIL, DVIS_TELEOFICINA,
            //DVIS_EMAIL, PCLAS_CODIGOVENTA,
            //PPRO_CODIGO, PCAT_CODIGO,
            //DVIS_EQUIPAMENTO, DVIS_MODELO,
            //DVIS_VENTA, DVIS_PRECTOTAL,
            //DVIS_VALOFINANCIACION, DVIS_NUMECONTACTO,
            //DVIS_PROXCONTACTO, DVIS_FECHPROXCONTACTO,
            //PRESULC_SECUENCIA, PVEN_CODIGO,
            //PCLI_TIPOCLIE, TCLA_CLASVEHI,
            //PCOL_CODIGO, DVIS_NUMCUOTA,
            //DVIS_PORCTASA, PFIN_MNIT,
            //MNIT_NIT, DVIS_VALODESC
            //FROM DBXSCHEMA.DVISITADIARIACLIENTES 
            //WHERE  PDOC_CODIGO='{0}' AND DVIS_NUMEVISI={1}", ddlContactosDisponibles.SelectedValue.Split('-')[0], ddlContactosDisponibles.SelectedValue.Split('-')[1]));
            if ((contacto.Tables.Count > 0) && (contacto.Tables[0].Rows.Count > 0))
            {
                DataRow contactoFila = contacto.Tables[0].Rows[0];
                DataRow contactoVehiculo;

                Prefijo = (string)ValidarDBNull(contactoFila["PDOC_CODIGO"], typeof(string));
                Numero = (int)ValidarDBNull(contactoFila["DVIS_NUMEVISI"], typeof(int));
                Nombre = (string)ValidarDBNull(contactoFila["DVIS_NOMBRE"], typeof(string));
                TelefonoFijo = (string)ValidarDBNull(contactoFila["DVIS_TELEFIJO"], typeof(string));
                TelefonoMovil = (string)ValidarDBNull(contactoFila["DVIS_TELEMOVIL"], typeof(string));
                TelefonoOficina = (string)ValidarDBNull(contactoFila["DVIS_TELEOFICINA"], typeof(string));
                Email = (string)ValidarDBNull(contactoFila["DVIS_EMAIL"], typeof(string));
                Medio = (string)ValidarDBNull(contactoFila["PCLAS_CODIGOVENTA"], typeof(string));
                Prospecto = (string)ValidarDBNull(contactoFila["PPRO_CODIGO"], typeof(string));
                //CatalogoVehiculo = (string)ValidarDBNull(contactoFila["PCAT_CODIGO"], typeof(string));
                //Equipamento = (string)ValidarDBNull(contactoFila["DVIS_EQUIPAMENTO"], typeof(string));
                //Modelo = (int)ValidarDBNull(contactoFila["DVIS_MODELO"], typeof(int));
                //Venta = Convert.ToDouble(ValidarDBNull(contactoFila["DVIS_VENTA"], typeof(decimal)));
                //PrecioTotal = Convert.ToDouble(ValidarDBNull(contactoFila["DVIS_PRECTOTAL"], typeof(decimal)));
                //ValorDescuento = Convert.ToDouble(ValidarDBNull(contactoFila["DVIS_VALODESC"], typeof(decimal)));
                /*txtNeto.Text = (PrecioTotal - ValorDescuento).ToString(); //, typeof(decimal);*/
                txtValorFinanciacion.Text = Convert.ToDouble(contactoFila["DVIS_VALOFINANCIACION"]).ToString("#,###");
                ProximoContacto = (string)ValidarDBNull(contactoFila["DVIS_PROXCONTACTO"], typeof(string));
                FechaProximoContacto = (DateTime)ValidarDBNull(contactoFila["DVIS_FECHPROXCONTACTO"], typeof(DateTime));
                NumeroContactos = (int)ValidarDBNull(contactoFila["DVIS_NUMECONTACTO"], typeof(int));
                ResultadoContacto = (int)ValidarDBNull(contactoFila["PRESULC_SECUENCIA"], typeof(int));
                Vendedor = (string)ValidarDBNull(contactoFila["PVEN_CODIGO"], typeof(string));
                TipoCliente = (string)ValidarDBNull(contactoFila["PCLI_TIPOCLIE"], typeof(string));
                //TipoVehiculo = (string)ValidarDBNull(contactoFila["TCLA_CLASVEHI"], typeof(string));
                //Color = (string)ValidarDBNull(contactoFila["PCOL_CODIGO"], typeof(string));
                txtNumCuotas.Text = contactoFila["DVIS_NUMCUOTA"].ToString();
                txtTasaFinanciacion.Text = contactoFila["DVIS_PORCTASA"].ToString();
                NitFinaciera = (string)ValidarDBNull(contactoFila["PFIN_MNIT"], typeof(string));
                NitCliente = (string)ValidarDBNull(contactoFila["MNIT_NIT"], typeof(string)); ;

                pIngresoDatos.Visible = true;
                Llenar_Grilla_Contacto();
                DataRow rowVehiculos;
                if(contacto.Tables[1].Rows.Count > 0)
                {
                    double suma = 0;
                    for(int i = 0; i < contacto.Tables[1].Rows.Count; i ++)
                    {
                        contactoVehiculo = contacto.Tables[1].Rows[i];
                        rowVehiculos = tablaVehiculos.NewRow();//contacto.Tables[i].Rows[i];
                        rowVehiculos[0] = (string)ValidarDBNull(contactoVehiculo["TCLA_CLASVEHI"], typeof(string)); //clase
                        rowVehiculos[1] = (string)ValidarDBNull(contactoVehiculo["PCAT_CODIGO"], typeof(string));  //catalogo
                        rowVehiculos[2] = (int)ValidarDBNull(contactoVehiculo["DVIV_MODELO"], typeof(int)); //ano
                        rowVehiculos[3] = (string)ValidarDBNull(contactoVehiculo["PCOL_CODIGO"], typeof(string)); //color
                        rowVehiculos[4] = (string)ValidarDBNull(contactoVehiculo["DVIV_EQUIPAMENTO"], typeof(string)); //equip
                        rowVehiculos[5] = Convert.ToDouble(ValidarDBNull(contactoVehiculo["DVIV_VENTA"], typeof(decimal))); //precio venta
                        rowVehiculos[6] = Convert.ToDouble(ValidarDBNull(contactoVehiculo["DVIV_DESCUENTO"], typeof(decimal))); //valors desc
                        rowVehiculos[7] = Convert.ToDouble(ValidarDBNull(contactoVehiculo["DVIV_PRECTOTAL"], typeof(decimal))); //valor neto
                        rowVehiculos[8] = ""; //placa
                        rowVehiculos[9] = ""; //vin
                        rowVehiculos[10] = (string)ValidarDBNull(contactoVehiculo["POPC_OPCIVEHI"], typeof(string)); //opcion FALTA ESTE PARA NUEVOS :S
                        suma += Convert.ToDouble(contactoVehiculo["DVIV_PRECTOTAL"].ToString()); //, typeof(decimal);

                        //tablaVehiculos
                        tablaVehiculos.Rows.Add(rowVehiculos);
                    }
                    txtNeto.Text = suma.ToString();
                    dgServicios.DataSource = tablaVehiculos;
                    Session["tablaVehiculos"] = tablaVehiculos;
                    dgServicios.DataBind();
                }

            }
            else
                pIngresoDatos.Visible = false;

            //cargarDDLContactosDisponibles();
            pAutenticacionUsuario.Visible = false;
            pContactosDisponibles.Visible = true;
            prepararContactos();
        }

        protected void ddlPrefijo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            Numero = InterpretarCadenaVaciaEntero(DBFunctions.SingleData("select pdoc_ultidocu + 1 from dbxschema.pdocumento where pdoc_codigo='" + ddlPrefijo.SelectedValue + "'"));
        }

        protected void ddlTipoVehiculo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (ddlTipoVehiculo.SelectedValue == "N")
            {
                imgLupaCatalogo.Visible = true;
                bind.PutDatasIntoDropDownList(ddlCatalogoVehiculo, AMS.Vehiculos.CatalogoVehiculos.CATALOGOVEHICULOSLISTA);
                txtVenta.Enabled = false;
            }
            else
            {
                ddlCatalogoVehiculo.Items.Clear();
                imgLupaCatalogo.Visible = false;
                string sqlUsados = @"SELECT MC.MCAT_VIN, MC.MCAT_ANOMODE || ' - ' || PM.PMAR_NOMBRE || ' - ' || PCV.PCAT_DESCRIPCION || ' - ' || MCAT_PLACA || ' - ' || PCOL_DESCRIPCION 
                                    FROM  MVEHICULO MV, MCATALOGOVEHICULO MC, PCOLOR PC, PMARCA PM, PCATALOGOVEHICULO PCV 
                                    WHERE MV.TEST_TIPOESTA < 30 and mv.tcla_codigo = 'U' AND MC.MCAT_VIN = MV.MCAT_VIN AND PC.PCOL_CODIGO = MC.PCOL_CODIGO AND MC.PCAT_CODIGO = PCV.PCAT_CODIGO 
                                      AND PCV.PMAR_CODIGO = PM.PMAR_CODIGO order by 1;";
                bind.PutDatasIntoDropDownList(ddlCatalogoVehiculo, sqlUsados);
                txtVenta.Enabled = true;
            }
            ddlCatalogoVehiculo.Items.Insert(0, new ListItem("Seleccione el Catálogo del Vehículo...", ""));
        }

        protected void ddlCatalogoVehiculo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            ValoresImpuestosVehiculos impuestosVeh;
            bool impo = true;

            if (ddlTipoCliente.SelectedItem.Text == "Seleccione el Tipo...")
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar el tipo de cliente!");
                return;
            }
            if (ddlTipoCliente.SelectedValue == "2" || ddlTipoCliente.SelectedValue == "P")
                impo = false;

            string signoMas = "+";
            //string disponibles =  "select MC.PCAT_CODIGO,A.PUBI_NOMBRE,A.PUBI_VIGENCIA,A.PUBI_DIASLLEGADA, C.mcat_vin, PC.PCOL_descripcion AS Color  from dbxschema.pubicacion A,dbxschema.MUBICACIONVEHICULO B,  dbxschema.mvehiculo C,mcatalogovehiculo mc, PCOLOR PC where A.pubi_codigo=B.pubi_codigo and C.mcat_vin=B.mcat_vin AND mC.PCOL_CODIGO = PC.PCOL_CODIGO  and C.mcat_vin = mc.mcat_vin and c.tcla_codigo = \\'N\\' and mc.pcat_codigo= \\'" + ddlCatalogoVehiculo.SelectedValue + "\\' ORDER BY MC.PCAT_CODIGO, PC.PCOL_descripcion, C.mcat_vin; ";
            string disponibles = "select REPLACE(\\'" + ddlCatalogoVehiculo.SelectedValue + "\\', \\'\\+\\', \\'_\\'), A.PUBI_NOMBRE,A.PUBI_VIGENCIA,A.PUBI_DIASLLEGADA, C.mcat_vin, PC.PCOL_descripcion AS Color " +
                   "from dbxschema.pubicacion A,dbxschema.MUBICACIONVEHICULO B,   dbxschema.mvehiculo C,mcatalogovehiculo mc, PCOLOR PC " +
                    "where A.pubi_codigo=B.pubi_codigo and C.mcat_vin=B.mcat_vin AND mC.PCOL_CODIGO = PC.PCOL_CODIGO  and C.mcat_vin = mc.mcat_vin and c.tcla_codigo = \\'N\\' " +
                    "and mc.pcat_codigo= \\'" + ddlCatalogoVehiculo.SelectedValue + "\\' " +
                    "ORDER BY MC.PCAT_CODIGO, PC.PCOL_descripcion, C.mcat_vin;";
            disp.Attributes.Remove("onClick");
            disp.Attributes.Add("onClick", "ModalDialog(this,'" + disponibles + "',new Array() );");

            if (ddlTipoVehiculo.SelectedValue == "N")
            {
                ddlModelo.Items.Clear();
                bind.PutDatasIntoDropDownList(ddlModelo, "SELECT PA.PANO_ANO FROM PPRECIOVEHICULO PP, PANO PA WHERE PP.PCAT_CODIGO = '" + ddlCatalogoVehiculo.SelectedValue + "' AND PA.PANO_ANO = PP.PANO_ANO; ");
                if (ddlModelo.Items.Count > 1)
                {
                    ddlModelo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    txtBase.Text = "";
                    txtImpuestos.Text = "";
                    txtVenta.Text = "";
                    ddlModelo.Enabled = true;
                }

                else if (ddlModelo.Items.Count == 1)
                {
                    bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + ddlCatalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI and pp.pano_ano = '" + ddlModelo.SelectedValue + "';");

                    if (ddlOpcionVeh.Items.Count > 1)
                    {
                        lblOpcionV.Visible = true;
                        ddlOpcionVeh.Visible = true;
                        ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                        txtBase.Text = "";
                        txtImpuestos.Text = "";
                        txtVenta.Text = "";
                    }
                    else if (ddlOpcionVeh.Items.Count == 1)
                    {
                        impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, ddlOpcionVeh.SelectedValue, ddlModelo.SelectedValue);
                        lblOpcionV.Visible = false;
                        ddlOpcionVeh.Visible = false;

                        double precio = impuestosVeh.ValorVenta;
                        txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                        txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                        txtVenta.Text = precio.ToString("#,###");
                    }
                    else
                    {
                        impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, null, ddlModelo.SelectedValue);
                        double precio = impuestosVeh.ValorVenta;
                        txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                        txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                        txtVenta.Text = precio.ToString("#,###");
                    }

                }

                else
                {
                    impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, "");
                    double precio = impuestosVeh.ValorVenta;
                    txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                    txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                    txtVenta.Text = precio.ToString("#,###");
                }

                //bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + ddlCatalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI;");
                //if (ddlOpcionVeh.Items.Count >= 1)
                //{
                //    lblOpcionV.Visible = true;
                //    ddlOpcionVeh.Visible = true;
                //    ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                //    txtBase.Text = "";
                //    txtImpuestos.Text = "";
                //    txtVenta.Text = "";
                //}
                //else
                //{
                //    lblOpcionV.Visible = false;
                //    ddlOpcionVeh.Visible = false;

                //    double precio = impuestosVeh.ValorVenta;
                //    txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                //    txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                //    txtVenta.Text = precio.ToString("#,###");
                //}
            }
            else if (ddlTipoVehiculo.SelectedValue != "N")
            {
                //impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, "");
                string vin = ddlCatalogoVehiculo.SelectedValue; // trae el vin en el caso de los usados
                //txtBase.Text = DBFunctions.SingleData("SELECT coalesce(MVEH_VALOCOMP,0) FROM MVEHICULO WHERE mcat_vin='" + vin + "' order by mveh_inventario DESC");
                txtVenta.Enabled = true;
                if (ddlCatalogoVehiculo.Items.Count > 0)
                {
                    //DatasToControls.EstablecerValueDropDownList(ddlServicio, DBFunctions.SingleData("SELECT c.tser_tiposerv FROM mcatalogovehiculo C, tserviciovehiculo t  WHERE mcat_vin='" + vin + "' AND C.tser_tiposerv = t.tser_tiposerv"));
                    //DatasToControls.EstablecerDefectoDropDownList(ddlModelo, DBFunctions.SingleData("SELECT DISTINCT mcat_anomode FROM dbxschema.mcatalogovehiculo WHERE mcat_vin='" + vin + "'"));
                    DatasToControls.EstablecerDefectoDropDownList(ddlModelo, DBFunctions.SingleData("SELECT DISTINCT mcat_anomode FROM dbxschema.mcatalogovehiculo;"));
                    ddlModelo.Enabled = true;
                    DatasToControls.EstablecerValueDropDownList(ddlcolor, DBFunctions.SingleData("SELECT C.PCOL_CODIGO FROM mcatalogovehiculo C, PCOLOR P  WHERE mcat_vin='" + vin + "' AND C.PCOL_CODIGO = P.PCOL_CODIGO"));
                    //DatasToControls.EstablecerValueDropDownList(colorOpcional, DBFunctions.SingleData("SELECT pcol_codigo FROM mcatalogovehiculo WHERE mcat_vin='" + vin + "'"));
                }

                if (ddlCatalogoVehiculo.SelectedValue.Length > 0)
                {
                    //if (ddlTipoVehiculo.SelectedValue != "N")
                    //{
                    //    string catalogoTexto = ddlCatalogoVehiculo.SelectedItem.ToString();
                    //    string anoUsado = catalogoTexto.Substring(0, 4);
                    //    ddlModelo.Enabled = false;
                    //    ddlModelo.Items.Clear();
                    //    ddlModelo.Items.Add(anoUsado);

                    //    double precio = impuestosVeh.ValorVenta;
                    //    txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                    //    txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                    //    txtVenta.Text = precio.ToString("#,###");
                    //}
                    //else
                    //{
                    //    ddlModelo.Enabled = true;
                    //    bind.PutDatasIntoDropDownList(ddlModelo, "SELECT PA.PANO_ANO FROM PPRECIOVEHICULO PP, PANO PA WHERE PP.PCAT_CODIGO = '" + ddlCatalogoVehiculo.SelectedValue + "' AND PA.PANO_ANO = PP.PANO_ANO; ");
                    //    ddlModelo.Items.Insert(0, new ListItem("Seleccione el Año Modelo...", ""));

                    //    //double precio = impuestosVeh.ValorVenta;
                    //    //txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                    //    //txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                    //    //txtVenta.Text = precio.ToString("#,###"); ;
                    //}

                    txtEquipamento.Text = ObtenerEquipamento(ddlCatalogoVehiculo.SelectedValue);

                    if (txtEquipamento.Text == "")
                    {
                        Utils.MostrarAlerta(Response, "No se ha parametrizado ningun accesorio al catalogo seleccionado: " + ddlCatalogoVehiculo.SelectedValue + "");
                        return;
                    }
                }
            }
        }

        protected void cambioAno(object sender, EventArgs e)
        {
            if (ddlTipoVehiculo.SelectedValue == "N")
            {
                DatasToControls bind = new DatasToControls();
                ValoresImpuestosVehiculos impuestosVeh;
                bool impo = true;
                if (ddlTipoCliente.SelectedValue == "2" || ddlTipoCliente.SelectedValue == "P")
                    impo = false;

                //impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, "", ddlModelo.SelectedValue);

                bind.PutDatasIntoDropDownList(ddlOpcionVeh, "select po.POPC_OPCIVEHI, po.POPC_NOMBOPCI from ppreciovehiculo pp, popcionvehiculo po where pp.PCAT_CODIGO = '" + ddlCatalogoVehiculo.SelectedValue + "' and po.POPC_OPCIVEHI = pp.POPC_OPCIVEHI and pp.pano_ano = '" + ddlModelo.SelectedValue + "';");
                if (ddlOpcionVeh.Items.Count > 1)
                {
                    lblOpcionV.Visible = true;
                    ddlOpcionVeh.Visible = true;
                    ddlOpcionVeh.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
                    txtBase.Text = "";
                    txtImpuestos.Text = "";
                    txtVenta.Text = "";
                    ddlModelo.Enabled = false;
                }
                else if (ddlOpcionVeh.Items.Count == 1)
                {
                    impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, ddlOpcionVeh.SelectedValue, ddlModelo.SelectedValue);
                    lblOpcionV.Visible = false;
                    ddlOpcionVeh.Visible = false;

                    double precio = impuestosVeh.ValorVenta;
                    txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                    txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                    txtVenta.Text = precio.ToString("#,###");
                }
                else
                {
                    impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, "", ddlModelo.SelectedValue);
                    lblOpcionV.Visible = false;
                    ddlOpcionVeh.Visible = false;

                    double precio = impuestosVeh.ValorVenta;
                    txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                    txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                    txtVenta.Text = precio.ToString("#,###");
                }
                //else
                //{
                //    lblOpcionV.Visible = false;
                //    ddlOpcionVeh.Visible = false;

                //    double precio = impuestosVeh.ValorVenta;
                //    txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
                //    txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
                //    txtVenta.Text = precio.ToString("#,###");
                //}
            }
        }
        protected void Cambio_OpcionVehiculo(object sender, EventArgs e)
        {
            bool impo = true;
            if (ddlTipoCliente.SelectedValue == "2" || ddlTipoCliente.SelectedValue == "P")
                impo = false;

            ValoresImpuestosVehiculos impuestosVeh = new ValoresImpuestosVehiculos(ddlCatalogoVehiculo.SelectedValue, impo, ddlOpcionVeh.SelectedValue, ddlModelo.SelectedValue);
            double precio = impuestosVeh.ValorVenta;
            txtBase.Text = impuestosVeh.ValorBase.ToString("#,###");
            txtImpuestos.Text = impuestosVeh.ValorImpuestos.ToString("#,###");
            txtVenta.Text = precio.ToString("#,###");
        }

        #endregion

        public string ObtenerEquipamento(string catalogo)
        {
            string cadenaEquip = "";
            DataSet equipo = new DataSet();
            DBFunctions.Request(equipo, IncludeSchema.NO,
                "SELECT pacc_descripcion FROM dbxschema.mcatalogoaccesorio mca, dbxschema.paccesorio pa WHERE mca.pcat_codigo = '" + catalogo + "' AND mca.pacc_codigo=pa.pacc_codigo;");

            if (equipo.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < equipo.Tables[0].Rows.Count; i++)
                {
                    cadenaEquip += equipo.Tables[0].Rows[i][0].ToString() +  "\n";
                }
            }

            return cadenaEquip;
        }
        public bool validarCamposVacios()
        {
            bool rta = ddlTipoCliente.SelectedValue == "" || ddlProspecto.SelectedValue == "" || ddlMedio.SelectedValue == "" || ddlTipoContacto.SelectedValue == ""
                || ddlProximoContacto.SelectedValue == ""
                || TextboxN.Text.Trim() == ""
                || ddlVendedor.SelectedValue == "" ? false : true;

            return rta;
        }
        public void validarClienteVendedor(object sender, System.EventArgs e)
        {
            Utils.MostrarAlerta(Response, "Yogui Meditation :3 ");
        }

        #region Grilla Items
        public void dgAccesorioBound(object sender, DataGridItemEventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (e.Item.ItemType == ListItemType.Footer)
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]), "SELECT piva_porciva FROM piva");
            if (Session["tablaElementos"] != null && tablaElementos != null)
            {
                calcularTotal();
            }
                
        }

        public void dgEvento_Grilla(object sender, DataGridCommandEventArgs e)
        {
            if (((Button)e.CommandSource).CommandName == "AgregarObsequios")
            {
                //Primero verificamos que los campos no sean vacios
                if (((((TextBox)e.Item.Cells[0].Controls[1]).Text) == "") || (!DatasToControls.ValidarDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text)))
                    Utils.MostrarAlerta(Response, "No existe ningun elemento para adicionar O Existe algun problema con el valor");
                else
                {//grilla
                    //debemos agregar una fila a la tabla asociada y luego volver a pintar la tabla
                    DataRow fila = tablaElementos.NewRow();
                    fila[0] = ((TextBox)e.Item.Cells[0].Controls[1]).Text;
                    fila[1] = ((TextBox)e.Item.Cells[1].Controls[1]).Text;
                    fila[2] = Convert.ToDouble(((TextBox)e.Item.Cells[2].Controls[1]).Text);
                    fila[3] = Convert.ToDouble(((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue.Trim());
                    tablaElementos.Rows.Add(fila);
                    Binding_Grilla();
                }
            }
            else if (((Button)e.CommandSource).CommandName == "QuitarObsequios")
            {
                tablaElementos.Rows[e.Item.ItemIndex].Delete();
                Binding_Grilla();
            }
        }

        private void CrearTablas()
        {
            dtPagos = new DataTable();
            dtPagos.Columns.Add("MCRED_NUMEPAGO", typeof(int));
            dtPagos.Columns.Add("MCRED_CUOTA", typeof(double));
            dtPagos.Columns.Add("MCRED_CAPITAL", typeof(double));
            dtPagos.Columns.Add("MCRED_INTERES", typeof(double));
            dtPagos.Columns.Add("MCRED_SALDO", typeof(double));

            ViewState["DT_PAGOS"] = dtPagos;
        }

        protected void Llenar_Grilla_Contacto()
        {
            Session["tablaElementos"] = tablaElementos;
            string sqlGrillaItem = @"select di.pite_codigo, pi.pite_nombre, di.dped_valoitem, di.piva_porciva  
                                    from  dvisitadiariaclientesitems di, pitemventavehiculo pi  
                                    where di.pite_codigo = pi.pite_codigo  
                                     and  di.pdoc_codigo = '" + Prefijo + "' and di.dvis_numevisi = " + Numero + "; ";
            DataSet elementos = new DataSet();

            DBFunctions.Request(elementos, IncludeSchema.NO, sqlGrillaItem);

            if (elementos != null)
            {
                for (int i = 0; i < elementos.Tables[0].Rows.Count; i++)
                {
                    DataRow fila = tablaElementos.NewRow();
                    fila[0] = (elementos.Tables[0].Rows[i][0]);
                    fila[1] = (elementos.Tables[0].Rows[i][1]);
                    fila[2] = Convert.ToDouble((elementos.Tables[0].Rows[i][2]));
                    fila[3] = Convert.ToDouble((elementos.Tables[0].Rows[i][3]));
                    tablaElementos.Rows.Add(fila);
                    grillaElementos.DataBind();
                }
                Binding_Grilla();
            }
        }

        protected void Binding_Grilla()
        {
            Session["tablaElementos"] = tablaElementos;
            grillaElementos.DataSource = tablaElementos;
            grillaElementos.DataBind();
            double totalOtrosVenta = 0;
            
            for (int i = 0; i < tablaElementos.Rows.Count; i++)
            {
                totalOtrosVenta += (Convert.ToDouble(tablaElementos.Rows[i][2]) + (Convert.ToDouble(tablaElementos.Rows[i][2]) * (Convert.ToDouble(tablaElementos.Rows[i][3]) / 100)));
                grillaElementos.Items[i].Cells[2].HorizontalAlign = HorizontalAlign.Right;
            }
            try
            {
                txtElementos.Text = totalOtrosVenta.ToString("#,###");
                txtPrecioTotal.Text = (totalOtrosVenta + Convert.ToDouble(txtVenta.Text)).ToString("#,###");
                PrecioTotal = Convert.ToDouble(txtPrecioTotal.Text.ToString());
            }
            catch { }
                
        }

        protected void prepararContactos()
        {
            DataSet ds = new DataSet();
            ds = DBFunctions.Request(ds, IncludeSchema.NO, string.Format(@"
                SELECT 
                    DVISC_NUMCONTACTO || ' ' || presulc_descripcion AS NUMERO, 
                    DVISC_FECHACONTACTO AS FECHA, 
                    DVISC_OBSERCONTACTO AS OBSERVACION 
                FROM 
                    DBXSCHEMA.DVISITADIARIACLIENTESCONTACTO dv, presultadocontacto pr 
                WHERE dv.presulc_secuencia = pr.presulc_secuencia and 
                    PDOC_CODIGO='{0}' AND DVIS_NUMEVISI={1} ORDER BY 2,1", Prefijo, Numero));

            datosContacto = ds.Tables[0];
            ViewState["DS_CONTACTOS"] = datosContacto;

            dgContactos.DataSource = datosContacto;
            dgContactos.DataBind();
            fldContacto.Visible = true;
        }
        protected void dgServicios_Edicion(object sender, DataGridCommandEventArgs e)
        {
            dgServicios.EditItemIndex = e.Item.ItemIndex;
            dgServicios.DataSource = tablaVehiculos;
            dgServicios.DataBind();
        }

        public void dgContactosBound(object sender, DataGridItemEventArgs e)
        {
            /*String msg = e.Item.Cells[0].Text;
            
            e.Item.Cells[0].Text = msg;
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ((TextBox)e.Item.Cells[0].Controls[1]).Text = NumeroContactos.ToString();
                ((TextBox)e.Item.Cells[1].Controls[1]).Text = InterpretarValorNuloAMostrar(DateTime.Now);

                ((TextBox)e.Item.Cells[0].Controls[1]).Enabled = false;
                ((TextBox)e.Item.Cells[1].Controls[1]).Enabled = false;
            }*/
        }
        protected void dgServicios_Actualizar(object sender, DataGridCommandEventArgs e)
        {
            

        }
        protected void dgServicios_Item(object sender, DataGridCommandEventArgs e)
        {
            //if (((Button)e.CommandSource).CommandName == "ClearRows")
            //{
                

            //}
            if(((Button)e.CommandSource).CommandName == "Update")
            {
                try
                {
                    tablaVehiculos.Rows[e.Item.ItemIndex]["VALOR_DESCUENTO"] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
                }
                catch
                {
                    tablaVehiculos.Rows[e.Item.ItemIndex]["VALOR_DESCUENTO"] = "0";
                }
                tablaVehiculos.AcceptChanges();
                Session["tablaVehiculos"] = tablaVehiculos;
                dgServicios.DataSource = tablaVehiculos;
                dgServicios.EditItemIndex = -1;
                dgServicios.DataBind();
            }
            else if (((Button)e.CommandSource).CommandName == "AddDatasRow")
            {
                if(!validarColumnas(((TextBox)e.Item.Cells[5].FindControl("txtPrecioVenta")).Text, ((TextBox)e.Item.Cells[7].FindControl("txtValorNeto")).Text))
                {
                    Utils.MostrarAlerta(Response, "los campos de precio no pueden estar vacios ni ser 0.");
                    return;
                }
                DataRow fila = tablaVehiculos.NewRow();
                string[] datosCatalogo;
                if (((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue == "U")
                {
                    datosCatalogo = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue.ToString().Split('~');
                    if (datosCatalogo.Length < 6)
                    {
                        Utils.MostrarAlerta(Response, "Se generó un problema cargando la información del vehículo, vuelve a realizar el proceso por favor.");
                        return;
                    }
                    fila[0] = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
                    fila[1] = datosCatalogo[1];

                    fila[2] = datosCatalogo[4]; 
                    fila[3] = datosCatalogo[3];

                    fila[4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                    fila[5] = Convert.ToDouble(((TextBox)e.Item.Cells[5].FindControl("txtPrecioVenta")).Text.ToString());
                    fila[6] = 0; // usados no aplican descuento
                    fila[7] = Convert.ToDouble(((TextBox)e.Item.Cells[7].FindControl("txtValorNeto")).Text.ToString());
                    fila[8] = datosCatalogo[2];
                    fila[9] = datosCatalogo[5];
                    fila[10] = datosCatalogo[0]; // OPCION VEHÍCULO PARA EL CASO DE NUEVOS Y EL INVENTARIO PARA EL VIEJO
                }
                else
                {
                    
                    fila[0] = ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue;
                    fila[1] = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue.Split('~')[0];
                    fila[2] = ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue;
                    fila[3] = ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue + " [" + ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedItem + "]";
                    fila[4] = ((TextBox)e.Item.Cells[4].Controls[1]).Text;
                    fila[5] = Convert.ToDouble(((TextBox)e.Item.Cells[5].FindControl("txtPrecioVenta")).Text.ToString().Substring(1));

                    try
                    {
                        fila[6] = ((TextBox)e.Item.Cells[6].Controls[1]).Text;
                    }
                    catch
                    {
                        fila[6] = 0;
                    }
                    
                    fila[7] = Convert.ToDouble(((TextBox)e.Item.Cells[7].FindControl("txtValorNeto")).Text.ToString().Substring(1));
                    fila[8] = ""; //nuevos no tienen placa
                    fila[9] = ""; // ni VIN
                    fila[10] = ((DropDownList)e.Item.Cells[1].Controls[1]).SelectedValue.Split('~')[1].Trim();
                }
               

                //fila[5] = ((Label)e.Item.Cells[5].Controls[1]).Text;
                

                tablaVehiculos.Rows.Add(fila);
                Session["tablaVehiculos"] = tablaVehiculos;
                
                //ViewState["unAlert"] = tablaVehiculos.Rows.Count == 1 ? "1" : null;
                if (tablaVehiculos.Rows.Count == 1)//ViewState["unAlert"] != null)
                    Utils.MostrarAlerta(Response, "Si el resultado es cierre o compra, El sistema redireccionará al menú de pedido únicamente con el primer registro de la tabla de cotización de vehículos ");
                dgServicios.DataSource = tablaVehiculos;
                dgServicios.DataBind();

            }
            else if(((Button)e.CommandSource).CommandName == "Delete")
            {
                tablaVehiculos.Rows[e.Item.ItemIndex].Delete();
                tablaVehiculos.AcceptChanges();
                Session["tablaVehiculos"] = tablaVehiculos;
                dgServicios.DataSource = tablaVehiculos;
                dgServicios.DataBind();
            }
        }

        protected void dgServicios_Databound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                ((TextBox)e.Item.Cells[6].Controls[1]).Enabled = dgServicios.Columns[3].Visible = dgServicios.Columns[2].Visible = true;
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[0].Controls[1]), "SELECT TCLA_CLASE, TCLA_CLASE || ' - ' || TCLA_NOMBRE FROM DBXSCHEMA.TCLASEVEHICULO ORDER BY TCLA_CLASE ASC");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].Controls[1]), "SELECT DISTINCT pc.Pcat_codigo || '~' || pp.popc_opcivehi, pc.pcat_codigo || '  -  ' || pc.pcat_descripcion  || ' - ' || PO.POPC_NOMBOPCI FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp, popcionvehiculo po WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PO.POPC_OPCIVEHI = PP.POPC_OPCIVEHI;");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[2].Controls[1]), "SELECT PANO_ANO, PANO_DETALLE FROM DBXSCHEMA.PANO");
                bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[3].Controls[1]), "SELECT PCOL_CODIGO, PCOL_DESCRIPCION FROM DBXSCHEMA.PCOLOR WHERE PCOL_ACTIVO LIKE 'S%'");
                ((DropDownList)e.Item.Cells[2].Controls[1]).SelectedValue = ViewState["ano"].ToString();//anoActual
                ((DropDownList)e.Item.Cells[1].Controls[1]).Items.Insert(0, new ListItem("Seleccione...", "0"));
                ((DropDownList)e.Item.Cells[2].Controls[1]).Items.Insert(0, new ListItem("Seleccione...", "0"));//seleccione..
                if (ViewState["cambioClase"] != null)
                {
                    if(ViewState["cambioClase"].ToString() == "U")
                    {
                        ((DropDownList)e.Item.Cells[0].Controls[1]).SelectedValue = ViewState["cambioClase"].ToString();
                        //((TextBox)e.Item.Cells[5].Controls[1]).Enabled = true;
                        bind.PutDatasIntoDropDownList(((DropDownList)e.Item.Cells[1].Controls[1]), @"SELECT Mv.mveh_inventario || '~' || mc.pcat_codigo || '~' || mc.mcat_placa || '~' || MC.pcol_CODIGO || '~' || MC.MCAT_ANOMODE || '~' || MV.MCAT_VIN, PCV.PCAT_CODIGO || ' - ' || PCV.PCAT_DESCRIPCION || ' - ' || pc.pcol_descripcion || ' - ' || mc.mcat_placa || ' - ' || MC.MCAT_ANOMODE FROM DBXSCHEMA.MVEHICULO MV, DBXSCHEMA.MCATALOGOVEHICULO MC, DBXSCHEMA.PCOLOR PC, DBXSCHEMA.PCATALOGOVEHICULO PCV WHERE MV.TEST_TIPOESTA < 30 AND MV.TCLA_CODIGO = 'U' 
                                                        AND MC.MCAT_VIN = MV.MCAT_VIN AND PC.PCOL_CODIGO = MC.PCOL_CODIGO AND PCV.PCAT_CODIGO = MC.PCAT_CODIGO;");
                        
                        if (((DropDownList)e.Item.Cells[1].Controls[1]).Items.Count == 0)
                        {
                            ((Button)e.Item.Cells[8].Controls[1]).Enabled = false;
                        }
                        else
                        {
                            ((DropDownList)e.Item.Cells[1].Controls[1]).Items.Insert(0, new ListItem("Seleccione...", "0"));
                        }
                        //carros usados no podrán realizar descuentos
                        ((TextBox)e.Item.Cells[5].Controls[1]).Attributes.Remove("onkeypress");
                        ((TextBox)e.Item.Cells[5].Controls[1]).Attributes.Add("onkeypress","return numeros(event)");
                        ((TextBox)e.Item.Cells[5].Controls[1]).Attributes.Add("onkeyup", "NumericMaskE(this,event);llenarNeto(this,'" + ((TextBox)e.Item.Cells[7].Controls[1]).ClientID + "')");
                        //((TextBox)e.Item.Cells[5].Controls[1]).Attributes.Add("onkeydown", "");
                        //((TextBox)e.Item.Cells[7].Controls[1]).Attributes.Remove("onkeypress");
                        //((TextBox)e.Item.Cells[7].Controls[1]).Attributes.Add("onkeypress", "return numeros(event)");
                        ((TextBox)e.Item.Cells[5].Controls[1]).Enabled = ((TextBox)e.Item.Cells[7].Controls[1]).Enabled = true;
                        ((TextBox)e.Item.Cells[7].Controls[1]).Attributes.Add("readonly","true");
                        ((TextBox)e.Item.Cells[6].Controls[1]).Enabled = dgServicios.Columns[3].Visible = dgServicios.Columns[2].Visible = false;

                    }
                    else
                    {
                        ((TextBox)e.Item.Cells[5].Controls[1]).Attributes.Remove("onkeypress");
                        ((TextBox)e.Item.Cells[5].Controls[1]).Attributes.Add("onkeypress", "return false;");
                        ((TextBox)e.Item.Cells[7].Controls[1]).Attributes.Remove("onkeypress");
                        ((TextBox)e.Item.Cells[7].Controls[1]).Attributes.Add("onkeypress", "return false;");
                        ((Button)e.Item.Cells[8].Controls[1]).Enabled = true;
                        dgServicios.Columns[3].Visible = dgServicios.Columns[2].Visible = true;

                    }
                    ViewState["cambioClase"] = null;
                    Binding_Grilla();
                }
                //((TextBox)e.Item.Cells[6].Controls[1]).Attributes.Add("onkeypress", "return numberMask(this,event)");
                ((TextBox)e.Item.Cells[6].Controls[1]).Attributes.Add("onkeyup", "validarDescuento(this,'" + ((TextBox)e.Item.FindControl("txtPrecioVenta")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlCatalogoVehi")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlModeloVehi")).ClientID + "','" + ((TextBox)e.Item.FindControl("txtValorNeto")).ClientID + "');");
                ((DropDownList)e.Item.Cells[1].Controls[1]).Attributes.Add("onChange", "cambiarPrecio(" + ((DropDownList)e.Item.Cells[1].Controls[1]).ClientID + ",'" + ((TextBox)e.Item.FindControl("txtPrecioVenta")).ClientID + "', '" + ((TextBox)e.Item.FindControl("txtValorNeto")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlModeloVehi")).ClientID + "','" + ((TextBox)e.Item.FindControl("txtValorDesc")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlClaseVehi")).ClientID + "', this)");
                ((DropDownList)e.Item.Cells[2].Controls[1]).Attributes.Add("onChange", "cambiarPrecio(" + ((DropDownList)e.Item.Cells[1].Controls[1]).ClientID + ",'" + ((TextBox)e.Item.Cells[5].FindControl("txtPrecioVenta")).ClientID + "', '" + ((TextBox)e.Item.Cells[7].Controls[1].FindControl("txtValorNeto")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlModeloVehi")).ClientID + "','" + ((TextBox)e.Item.FindControl("txtValorDesc")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlClaseVehi")).ClientID + "',this)");

                //if (ddlTipoVehiculo.SelectedValue == "N")
                //{
                //    bind.PutDatasIntoDropDownList(ddlCatalogoVehiculo, AMS.Vehiculos.CatalogoVehiculos.CATALOGOVEHICULOSLISTA);
                //    ddlCatalogoVehiculo.Items.Insert(0, new ListItem("Seleccione el Catálogo del Vehículo...", ""));
                //}
                //else
                //{
                //    bind.PutDatasIntoDropDownList(ddlCatalogoVehiculo, "select MC.PCAT_CODIGO, select MC.MCAT_ANOMODE || ' - ' || MCAT_PLACA || ' - ' || PCOL_DESCRIPCION FROM MVEHICULO MV, MCATALOGOVEHICULO MC, PCOLOR PC WHERE MV.TEST_TIPOESTA < 30 and mv.tcla_codigo = 'U' AND MC.MCAT_VIN = MV.MCAT_VIN AND PC.PCOL_CODIGO = MC.PCOL_CODIGO");
                //    ddlCatalogoVehiculo.Items.Insert(0, new ListItem("Seleccione el Vehículo...", ""));
                //}
                calcularTotal();
            }
            else if (e.Item.ItemType == ListItemType.EditItem)
            {
                
                ((TextBox)e.Item.Cells[6].Controls[1]).Text = tablaVehiculos.Rows[e.Item.ItemIndex]["VALOR_DESCUENTO"].ToString();
                ((TextBox)e.Item.Cells[5].Controls[1]).Text = Convert.ToDouble(tablaVehiculos.Rows[e.Item.ItemIndex]["PRECIO_VENTA"]).ToString("C");
                ((TextBox)e.Item.Cells[7].Controls[1]).Text = Convert.ToDouble(tablaVehiculos.Rows[e.Item.ItemIndex]["VALOR_NETO"]).ToString("C");
                ((DropDownList)e.Item.Cells[1].Controls[1]).Items.Add(new ListItem(tablaVehiculos.Rows[e.Item.ItemIndex]["CATALOGO_VEHICULO"].ToString(), tablaVehiculos.Rows[e.Item.ItemIndex]["CATALOGO_VEHICULO"].ToString()));
                ((DropDownList)e.Item.Cells[2].Controls[1]).Items.Add(new ListItem(tablaVehiculos.Rows[e.Item.ItemIndex]["ANO_MODELO"].ToString(), tablaVehiculos.Rows[e.Item.ItemIndex]["ANO_MODELO"].ToString()));
                ((TextBox)e.Item.Cells[6].Controls[1]).Attributes.Add("onkeyup", "validarDescuento(this,'" + ((TextBox)e.Item.FindControl("txtPrecioVenta")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlCatalogoVehi")).ClientID + "','" + ((DropDownList)e.Item.FindControl("ddlModeloVehi")).ClientID + "','" + ((TextBox)e.Item.FindControl("txtValorNeto")).ClientID + "');");
            }
            
            
            
        }
        protected void calcularTotal()
        {
            double suma = 0;
            if(tablaVehiculos != null)
            {
                for (int i = 0; i < tablaVehiculos.Rows.Count; i++)
                {
                    suma += Convert.ToDouble(tablaVehiculos.Rows[i]["VALOR_NETO"]);
                }

                double totalOtrosVenta = 0;
                for (int i = 0; i < tablaElementos.Rows.Count; i++)
                {
                    totalOtrosVenta += (Convert.ToDouble(tablaElementos.Rows[i][2]) + (Convert.ToDouble(tablaElementos.Rows[i][2]) * (Convert.ToDouble(tablaElementos.Rows[i][3]) / 100)));
                }
                try
                {
                    txtElementos.Text = totalOtrosVenta.ToString("C");
                    txtPrecioTotal.Text = (totalOtrosVenta + suma).ToString("C");
                }
                catch { }
            }
            
        }
        protected void cambiaClase(object sender, EventArgs z)
        {
            ViewState["cambioClase"] = ((DropDownList)sender).SelectedValue;
            dgServicios.DataSource = tablaVehiculos;
            DataBind();
        }
        protected void cambiarPrecioVehiculo(object sender, EventArgs z)
        {
            ViewState["precioVehiculo"] = ((DropDownList)sender).SelectedValue;
            dgServicios.DataSource = tablaVehiculos;
        }

        protected bool validarColumnas(string precio, string precioTotal)
        {
            bool rta = true;

            if (precio == String.Empty
                || precioTotal == String.Empty
                || precio == "0" || precioTotal == "0")
                return false;

            return rta;
        }

        protected void Preparar_Tabla_Elementos()
        {
            tablaElementos = new DataTable();
            tablaElementos.Columns.Add(new DataColumn("CODIGO", System.Type.GetType("System.String")));//0
            tablaElementos.Columns.Add(new DataColumn("DESCRIPCION", System.Type.GetType("System.String")));//1
            tablaElementos.Columns.Add(new DataColumn("COSTO", System.Type.GetType("System.Double")));//2
            tablaElementos.Columns.Add(new DataColumn("IVA", System.Type.GetType("System.Double")));//3                  
        }

        protected void cargarDDLContactosDisponibles()
        {
            DataSet dsContactos = new DataSet();
            //string sqlContactos = string.Format("select PDOC_CODIGO CONCAT '-' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) VALOR,'[' CONCAT PDOC_CODIGO CONCAT '-' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) CONCAT '] - ['CONCAT DVIS_NOMBRE CONCAT '] - ['CONCAT CHAR(DVIS_FECHPROXCONTACTO) CONCAT ']' DESCRIPCION, DVIS_FECHPROXCONTACTO FECHA  from DBXSCHEMA.DVISITADIARIACLIENTES where DVIS_PROXCONTACTO = 'SI' AND PVEN_CODIGO = '{0}' order by DVIS_FECHPROXCONTACTO,DVIS_NOMBRE", VendedorAutenticacion);
            string sqlContactos = string.Format("select PDOC_CODIGO CONCAT '-' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) VALOR,'[' CONCAT PDOC_CODIGO CONCAT '-' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) CONCAT '] - ['CONCAT DVIS_NOMBRE CONCAT '] - ['CONCAT CHAR(DVIS_FECHPROXCONTACTO) CONCAT ']' DESCRIPCION, DVIS_FECHPROXCONTACTO FECHA  from DBXSCHEMA.DVISITADIARIACLIENTES where DVIS_PROXCONTACTO = 'SI' AND PVEN_CODIGO = '{0}' order by DVIS_FECHPROXCONTACTO,DVIS_NOMBRE", VendedorAutenticacion);
            DBFunctions.Request(dsContactos, IncludeSchema.NO, sqlContactos);

            ddlContactosDisponibles.Items.Clear();

            for (int k = 0; k < dsContactos.Tables[0].Rows.Count ;k++)
            {
                ListItem item = new ListItem(dsContactos.Tables[0].Rows[k][1].ToString(),dsContactos.Tables[0].Rows[k][0].ToString());
                DateTime fechaContacto = (DateTime)dsContactos.Tables[0].Rows[k][2];

                if (fechaContacto.Date.CompareTo(DateTime.Now.Date) < 0)
                {
                    item.Attributes.Add("style", "background-color:#DD8888");
                }
                else if (fechaContacto.Date.CompareTo(DateTime.Now.Date) == 0)
                {
                    item.Attributes.Add("style", "background-color:#FFFF88");
                }

                ddlContactosDisponibles.Items.Insert(k, item);
            }

            ddlContactosDisponibles.Items.Insert(0, new ListItem("Seleccione el contacto a actualizar", ""));

            string sqlContactosModal =
                    "select PDOC_CODIGO CONCAT \\'-\\' CONCAT RTRIM(CHAR(DVIS_NUMEVISI)) COTIZACION, " +
                    "DVIS_NOMBRE  CLIENTE, DVIS_FECHPROXCONTACTO FECHA from DBXSCHEMA.DVISITADIARIACLIENTES where DVIS_PROXCONTACTO = \\'SI\\' " +
                    "AND PVEN_CODIGO = \\'" + VendedorAutenticacion + "\\' order by DVIS_FECHPROXCONTACTO,DVIS_NOMBRE;";

            imglupa.Attributes.Remove("OnClick");
            imglupa.Attributes.Add("OnClick", "ModalDialog(" + ddlContactosDisponibles.ClientID + ",'" + sqlContactosModal + "',new Array() );");
        }

        [Ajax.AjaxMethod()]
        public DataSet consultarAnos(string catalogo, string opcion)
        {
            DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT PANO_ANO AS CODIGO, PANO_ANO AS DESCRIPCION FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp, DBXSCHEMA.popcionvehiculo po " +
                                                       "WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PO.POPC_OPCIVEHI = PP.POPC_OPCIVEHI AND PP.PCAT_CODIGO = '" + catalogo + "' AND PP.POPC_OPCIVEHI = '" + opcion + "'; ");
            return ds;
        }

        [Ajax.AjaxMethod()]
        public string cargarPrecioVehiculo(string catalogo, string ano, string opcion)
        {
            string rta = "";
            //string consulta = DBFunctions.SingleData("SELECT (PP.PPRE_PRECIO + (PPRE_PRECIO * (PC.PIVA_PORCIVA*0.01))) AS MONTO FROM DBXSCHEMA.PPRECIOVEHICULO PP, DBXSCHEMA.PCATALOGOVEHICULO PC WHERE PP.PCAT_CODIGO = PC.PCAT_CODIGO AND PP.PCAT_CODIGO = '" + catalogo + "' AND PP.PANO_ANO =" + ano + ";");
            string consulta = DBFunctions.SingleData("SELECT CASE WHEN PC.TTIP_TIPOSERV = '1' OR PC.TTIP_TIPOSERV = 'A' THEN (PP.PPRE_PRECIO +(PPRE_PRECIO*((PC.PIVA_PORCIVA+PC.PIPO_PORCIPOC)*0.01)))ELSE (PP.PPRE_PRECIO +(PPRE_PRECIO*(PC.PIVA_PORCIVA*0.01))) END AS MONTO FROM DBXSCHEMA.PPRECIOVEHICULO PP, DBXSCHEMA.PCATALOGOVEHICULO PC WHERE PP.PCAT_CODIGO = PC.PCAT_CODIGO AND PP.PCAT_CODIGO = '" + catalogo + "' AND PP.PANO_ANO =" + ano + " AND PP.POPC_OPCIVEHI = '"+ opcion +"';");
            //rta = DBFunctions.SingleData("SELECT PPRE_PRECIO FROM DBXSCHEMA.PPRECIOVEHICULO WHERE PCAT_CODIGO = '" + catalogo + "' AND PANO_ANO = " + ano + ";");
            try
            {
                rta = Convert.ToDouble(consulta).ToString("C");

            }
            catch { rta = ""; }
            
            //rta += "~" + Convert.ToDouble(Convert.ToDouble(rta.Split('~')[0]) * Convert.ToDouble(rta.Split('~')[1])).ToString("C");
            return rta;
        }

        [Ajax.AjaxMethod()]
        public string cargarPrecioVehiculoUsado(string inventario)
        {
            string elValor = DBFunctions.SingleData(@"SELECT MVEH_VALOINFL FROM DBXSCHEMA.MVEHICULO MV, DBXSCHEMA.MCATALOGOVEHICULO MC, DBXSCHEMA.PCOLOR PC, DBXSCHEMA.PCATALOGOVEHICULO PCV WHERE MV.TEST_TIPOESTA < 30 AND MV.TCLA_CODIGO = 'U' 
                                                    AND MC.MCAT_VIN = MV.MCAT_VIN AND PC.PCOL_CODIGO = MC.PCOL_CODIGO AND PCV.PCAT_CODIGO = MC.PCAT_CODIGO 
                                                    AND MV.MVEH_INVENTARIO = '" + inventario.Split('~')[0] + "';");// Mveh_inventario
            try
            {
                elValor = Convert.ToDouble(elValor).ToString();
            }
            catch
            {
                elValor = "";
            }
            return elValor;
        }

        [Ajax.AjaxMethod()]
        public string calcularDescuento(string valorDesc, string valorTotal, string catalogo, string ano)
        {

            double valorDescuento, descuentoPermitido, totalConDescuento;
            double precioNormal = Convert.ToDouble(valorTotal.Substring(1));

            valorDescuento = Convert.ToDouble(DBFunctions.SingleData("SELECT PPRE_DTOMAX FROM PPRECIOVEHICULO WHERE PCAT_CODIGO = '" + catalogo + "' AND PANO_ANO = " + ano));
            descuentoPermitido = Convert.ToDouble(precioNormal * (valorDescuento/100));//calcular aca e ldescuento total //tablaDescuentos.Select("PCAT_CODIGO = '" + catalogo + "' AND PANO_ANO = " + ano)[0].ItemArray[1].ToString();
            if (Convert.ToDouble(valorDesc) > descuentoPermitido)
            {
                return "-1";
            }
            totalConDescuento = precioNormal - Convert.ToDouble(valorDesc);
            return totalConDescuento.ToString("C");
        }
        [Ajax.AjaxMethod()]
        public string calcularDescuentoUsados(string valorDesc, string valorTotal, string inventario)
        {
            double precioVehiculo;
            string elValor = DBFunctions.SingleData(@"SELECT MVEH_VALOINFL FROM DBXSCHEMA.MVEHICULO MV, DBXSCHEMA.MCATALOGOVEHICULO MC, DBXSCHEMA.PCOLOR PC, DBXSCHEMA.PCATALOGOVEHICULO PCV WHERE MV.TEST_TIPOESTA < 30 AND MV.TCLA_CODIGO = 'U' 
                                                    AND MC.MCAT_VIN = MV.MCAT_VIN AND PC.PCOL_CODIGO = MC.PCOL_CODIGO AND PCV.PCAT_CODIGO = MC.PCAT_CODIGO 
                                                    WHERE MV.MVEH_INVENTARIO = '" + inventario + "';");// Mveh_inventario
            if (elValor == "")
                return "-1";

            precioVehiculo = Convert.ToDouble(elValor);

            return precioVehiculo.ToString("C");
        }

        [Ajax.AjaxMethod]
        public DataSet Verificar_Cliente(string Cedula) 
        {
            var usuarioLogueado = HttpContext.Current.User.Identity.Name.ToLower();
            DataSet clientes = new DataSet();
            DBFunctions.Request(clientes, IncludeSchema.NO,
                @"SELECT MNIT_NIT, COALESCE(MNIT_NOMBRES, '') CONCAT ' ' CONCAT coalesce(MNIT_NOMBRE2, '') CONCAT ' ' CONCAT COALESCE(MNIT_APELLIDOS, '') CONCAT ' ' CONCAT coalesce(MNIT_APELLIDO2, '') AS NOMBRE, 
                MNIT_DIRECCION AS DIRECCION, mnit_telefono as TELEFONO, mnit_CELULAR as CELULAR, mnit_EMAIL as EMAIL, TNIT_TIPONIT as TIPONIT, MNIT_REPRESENTANTE AS REPRESENTANTE_LEGAL, MNIT_NITREPRESENTANTE AS NIT_REPRESENTANTE FROM MNITCOTIZACION 
                WHERE MNIT_NIT = '" + Cedula + @"'
                UNION 
                SELECT MNIT_NIT, COALESCE(MNIT_NOMBRES, '') CONCAT ' ' CONCAT coalesce(MNIT_NOMBRE2, '') CONCAT ' ' CONCAT COALESCE(MNIT_APELLIDOS, '') CONCAT ' ' CONCAT coalesce(MNIT_APELLIDO2, '') AS NOMBRE, 
                MNIT_DIRECCION AS DIRECCION, mnit_telefono as TELEFONO, mnit_CELULAR as CELULAR, mnit_EMAIL as EMAIL, TNIT_TIPONIT as TIPONIT, MNIT_REPRESENTANTE AS REPRESENTANTE_LEGAL, MNIT_NITREPRESENTANTE AS NIT_REPRESENTANTE FROM MNIT
                WHERE MNIT_NIT = '"+ Cedula + @"';
                 select    
                 ( dv.dvis_numevisi CONCAT '. Vendedor:' CONCAT pven.pven_nombre CONCAT '. Fecha de Contacto:' CONCAT  dv.dvis_fechprimcontacto CONCAT '. Proximo Contacto:'  
                 CONCAT dv.dvis_proxcontacto CONCAT '. Fecha Proximo:' CONCAT  COALESCE(dv.dvis_fechproxcontacto, '0001-01-01') CONCAT '. Sede:' CONCAT  p.palm_descripcion CONCAT '. Hecho hace:' CONCAT  
                 CAST( ( days (current date) - days (date(dv.dvis_fechprimcontacto)) ) as INT) CONCAT ' días. Observaciones:' CONCAT ddia.dvisc_obsercontacto) as CONTACTOS,
                 DV.PVEN_CODIGO    
                 from  DVISITADIARIACLIENTES dv, pvendedor pven, palmacen p, dvisitadiariaclientescontacto ddia, cvehiculos cv   
                 where dv.mnit_nit='" + Cedula + @"' and pven.pven_codigo = dv.pven_codigo and p.palm_almacen = pven.palm_almacen  
                  and  CAST( ( days (current date) - days (date(dv.dvis_fechprimcontacto)) ) as INT) <= (coalesce(cv.CVEH_DIASCOTI,30) + 10)  
                  and  ddia.pdoc_codigo = dv.pdoc_codigo and ddia.dvis_numevisi=dv.dvis_numevisi  
                  and  ddia.dvisc_numcontacto = dv.dvis_numecontacto order by dv.dvis_fechprimcontacto DESC;");
            
              // adiciono el codigo del vendedor para poder compararlo con el vendedor de la nueva cotizacion y si es diferente y bloquea = 'S'...VALIDAR y bloquear
            return clientes;
        }

        [Ajax.AjaxMethod]
        public String Consultar_Cliente(string Nitcli)
        {
            string respuesta = "";
            respuesta = DBFunctions.SingleData("SELECT MNIT_NIT FROM MCLIENTE WHERE MNIT_NIT = '" + Nitcli + "';");
            return respuesta;
        }
        protected void validaVendedor(object Sender, EventArgs e)
        {
            DataSet dsVendedor = new DataSet();
            DBFunctions.Request(dsVendedor, IncludeSchema.NO, 
              @"select DV.PVEN_CODIGO from  DVISITADIARIACLIENTES dv, pvendedor pven, palmacen p, dvisitadiariaclientescontacto ddia, cvehiculos cv   
              where dv.mnit_nit='" + TextboxN.Text.ToString() + @"' and pven.pven_codigo = dv.pven_codigo and p.palm_almacen = pven.palm_almacen  
              and  CAST( ( days (current date) - days (dv.dvis_fechprimcontacto) ) as INT) <= (coalesce(cv.CVEH_DIASCOTI,30) + 10)  
              and  ddia.pdoc_codigo = dv.pdoc_codigo and ddia.dvis_numevisi=dv.dvis_numevisi  
              and  ddia.dvisc_numcontacto = dv.dvis_numecontacto order by DV.PVEN_CODIGO DESC;");
            if (dsVendedor.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Este cliente aún no tiene cotiaciones previas");
            }
            else if (dsVendedor.Tables[0].Rows[0]["PVEN_CODIGO"].ToString() != ddlVendedor.SelectedValue)
            {
                TextboxN.Text = "";
                txtNombre.Text = "";
                txtTelefonoFijo.Text= "";
                txtTelefonoMovil.Text = "";
                txtTelefonoOficina.Text = "";
                txtEmail.Text = "";
                //Grabar.Enabled = false;
                Utils.MostrarAlerta(Response, "Este cliente ya tiene una(s) cotizacion(es) previa(s) con otro vendedor..! \\n Revise por favor..");
            }
        }

        [Ajax.AjaxMethod]
        public bool ValidarDescuento(string valorDescuento, string precioVenta, string catalogo)
        {
            bool valido = false;
            string porcentajeDesc = DBFunctions.SingleData("select coalesce(ppre_dtomax,0) from ppreciovehiculo where pcat_codigo = '" + catalogo + "';");
            double precioNormal = Convert.ToDouble(precioVenta.Substring(1));
            double valorMaximoDesc = Math.Round(Convert.ToDouble(precioVenta) * (Convert.ToDouble(porcentajeDesc) * 0.01),0);
            if (valorDescuento.ToString() == "") { valorDescuento = "0"; }
            if (Convert.ToDouble(valorDescuento) <= valorMaximoDesc)
            {
                valido = true;
            }
            return valido;
        }

        #endregion
        protected string insertarEnMNit(ArrayList insert)
        {
            string rta = "";
            if (!DBFunctions.Transaction(insert))
            {
                rta = DBFunctions.exceptions;
            }
            return rta;
        }
        protected void agregarContacto()
        {
            Int32 etapaVenta = Convert.ToInt32(ddlTipoContacto.SelectedValue);
            DBFunctions.NonQuery(string.Format(@"INSERT INTO DVISITADIARIACLIENTESCONTACTO 
            VALUES('{0}', {1}, {2}, '{3}', '{4}', {5})",
            Prefijo,
            Numero,
            NumeroContactos,
            txtFechaContacto.Text,
            txtObservacionesContacto.Text,
            etapaVenta));
            
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
    }

    public class ModoSeguimientoDiario
    {
        public const string COTIZACION = "COTIZACION";
        public const string CONTACTO = "CONTACTO";
       
    }  
}

