using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using AMS.DB;
using AMS.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Configuration;
using System.Drawing;
using AMS.Tools;
using AMS.Documentos;
using System.Data;
using AMS.CriptoServiceProvider;

namespace AMS.VIP
{
	public partial class ManejoClientes : System.Web.UI.UserControl
	{
        private String fwdOpt;
        private String codCliente;
        public string indexPage = ConfigurationManager.AppSettings["MainAjaxPage"];

        private DatasToControls bind;

        public const String OPT_NEW = "NEW"; // Nuevo cliente
        public const String OPT_MOD_H = "MODH"; // Modificación Hijo
        public const String OPT_MOD_P = "MODP"; // Modificación Padre
        public const String OPT_CONS_H = "CONSH"; // Consulta Hijo
        public const String OPT_CONS_P = "CONSP"; // Consulta Padre
        public const String OPT_FAC_P = "FACP"; // Facturacion Padre
        public const String OPT_FAC_H = "FACH"; // Facturacion Hijo

        private const String PADRE = "P";
        private const String HIJO = "H";

		protected void Page_Load(object sender, EventArgs e)
		{
            fwdOpt = Request.QueryString["fwdOpt"];
            codCliente = Request.QueryString["codCliente"];

            if (!IsPostBack)
            {
                if (Request.QueryString["facturada"] != null)
                    Response.Write("<script language:javascript>w=window.open('" + Request.QueryString["facturada"] + "','','HEIGHT=600,WIDTH=600');</script>");

                bind = new DatasToControls();
                parametrizacionInicial();

                switch (fwdOpt)
                {
                    case OPT_NEW:
                        construirTablaNuevo();
                        break;
                    case OPT_MOD_H:
                        llenarClientes();
                        construirTablaEdicionHijo();
                        guardarCodigosClientes();
                        almacenarDatosOriginales();
                        break;
                    case OPT_MOD_P:
                        llenarClientes();
                        construirTablaEdicionPadre();
                        guardarCodigosClientes();
                        almacenarDatosOriginales();
                        break;
                    case OPT_CONS_H:
                        llenarClientes();
                        construirTablaConsultaHijo();
                        break;
                    case OPT_CONS_P:
                        llenarClientes();
                        construirTablaConsultaPadre();
                        break;
                    case OPT_FAC_P:
                        llenarClientes();
                        construirTablaConsultaPadre();
                        construirFacturacion();
                        break;
                    case OPT_FAC_H:
                        llenarClientes();
                        construirTablaConsultaHijo();
                        construirFacturacion();
                        break;
                }
            }
		}

        private void almacenarDatosOriginales()
        {
            Hashtable padre = armarHashPadre();
            Hashtable hijo1 = existeHijo1() ? armarHashHijo1() : null;
            Hashtable hijo2 = existeHijo2() ? armarHashHijo2() : null;

            ViewState["datosPadre"] = padre;
            ViewState["datosHijo1"] = hijo1;
            ViewState["datosHijo2"] = hijo2;
        }

        private void guardarLogsCambios()
        {
            bool exito = true;
            string tabla = "Clientes";
            TipoLog tipo = TipoLog.Update;

            Hashtable padreOld = ViewState["datosPadre"] != null ? ViewState["datosPadre"] as Hashtable : null;
            Hashtable hijo1Old = ViewState["datosHijo1"] != null ? ViewState["datosHijo1"] as Hashtable : null;
            Hashtable hijo2Old = ViewState["datosHijo2"] != null ? ViewState["datosHijo2"] as Hashtable : null;

            Hashtable padre = armarHashPadre();
            Hashtable hijo1 = existeHijo1() ? armarHashHijo1() : null;
            Hashtable hijo2 = existeHijo2() ? armarHashHijo2() : null;

            if (padre != null && !Utils.HashtableEquals(padreOld, padre)) 
                exito &= Utils.logger(tabla, tipo, padreOld);
            if (hijo1 != null && !Utils.HashtableEquals(hijo1Old, hijo1))
                exito &= Utils.logger(tabla, tipo, hijo1Old);
            if (hijo2 != null && !Utils.HashtableEquals(hijo2Old, hijo2))
                exito &= Utils.logger(tabla, tipo, hijo2Old);

            if (!exito)
                lblErrorClientes.Text = "Hubo un error al guardar el historial de cambios";
        }

        private void guardarCodigosClientes()
        {
            ViewState["codPadre"] = lblCodProdPadre.Text;
            ViewState["codHijo1"] = lblCodProdHijo1.Text;
            ViewState["codHijo2"] = lblCodProdHijo2.Text;
        }

        protected void generarCodProdPadre(object sender, EventArgs e)
        {
            if (txtSAP.Text != "" && txtCedPadre.Text != "")
                lblCodProdPadre.Text = txtCedPadre.Text + txtSAP.Text;

            if (txtSAP.Text != "" && txtCedHijo1.Text != "")
                lblCodProdHijo1.Text = txtCedHijo1.Text + txtSAP.Text;

            if (txtSAP.Text != "" && txtCedHijo2.Text != "")
                lblCodProdHijo2.Text = txtCedHijo2.Text + txtSAP.Text;
        }
            
        protected void generarCodProdHijo1(object sender, EventArgs e)
        {
            if (txtSAP.Text != "" && txtCedHijo1.Text != "")
                lblCodProdHijo1.Text = txtCedHijo1.Text + txtSAP.Text;
        }

        protected void generarCodProdHijo2(object sender, EventArgs e)
        {
            if (txtSAP.Text != "" && txtCedHijo2.Text != "")
                lblCodProdHijo2.Text = txtCedHijo2.Text + txtSAP.Text;
        }

        protected void guardarNuevo(object sender, EventArgs e)
        {
            String campos = "";

            if (validarDatosNuevo(ref campos))
            {
                lblErrorClientes.Text = "";
                Hashtable padre = armarHashPadre();
                Hashtable hijo1 = existeHijo1() ? armarHashHijo1() : null;
                Hashtable hijo2 = existeHijo2() ? armarHashHijo2() : null;

                bool bien = ManejoClientesLogic.guardarClientes(padre, hijo1, hijo2);

                if (bien)
                {
                    limpiarDatos();
                    Utils.MostrarAlerta(Response, "Clientes Guardados!");
                }
                Utils.MostrarAlerta(Response, "Ocurrió un error al guardar los datos. Revise los logs para más información");
            }
            else
            {
                Utils.MostrarAlerta(Response, "Existen Errores en los datos ingresados, por favor revise");
                lblErrorClientes.Text = campos;
            }
        }

        private void limpiarDatos()
        {
            lblCodProdPadre.Text = "";
            lblCodProdHijo1.Text = "";
            lblCodProdHijo2.Text = "";
            txtApeHijo1.Text = "";
            txtApeHijo2.Text = "";
            txtApePadre.Text = "";
            txtCapEnd.Text = "";
            txtCedHijo1.Text = "";
            txtCedHijo2.Text = "";
            txtCedPadre.Text = "";
            txtCupoDai.Text = "";
            txtDirHijo1.Text = "";
            txtDirHijo2.Text = "";
            txtDirPadre.Text = "";
            txtFechaCorte.Text = "";
            txtFechaExpHijo1.Text = "";
            txtFechaExpHijo2.Text = "";
            txtFechaExpPadre.Text = "";
            txtFin.Text = "";
            txtNomHijo1.Text = "";
            txtNomHijo2.Text = "";
            txtNomPadre.Text = "";
            txtPtsHijo1.Text = "";
            txtPtsHijo2.Text = "";
            txtPtsPadre.Text = "";
            txtSAP.Text = "";
            txtTelfHijo1.Text = "";
            txtTelfHijo2.Text = "";
            txtTelfPadre.Text = "";
            chkInactivo.Checked = false;
        }

        protected void guardarEdicion(object sender, EventArgs e)
        {
            String campos = "";

            if (validarDatosNuevo(ref campos))
            {
                lblErrorClientes.Text = "";
                Hashtable padre = armarHashPadre();
                Hashtable hijo1 = existeHijo1() ? armarHashHijo1() : null;
                Hashtable hijo2 = existeHijo2() ? armarHashHijo2() : null;

                DictionaryEntry[] keys = new DictionaryEntry[3];
                keys[0] = new DictionaryEntry("mcli_codigo", String.Format("'{0}'", ViewState["codPadre"]));
                keys[1] = new DictionaryEntry("mcli_codigo", String.Format("'{0}'", ViewState["codHijo1"]));
                keys[2] = new DictionaryEntry("mcli_codigo", String.Format("'{0}'",ViewState["codHijo2"]));

                bool bien = ManejoClientesLogic.editarClientes(keys, padre, hijo1, hijo2);

                if (bien)
                {
                    guardarLogsCambios();
                    Utils.MostrarAlerta(Response, "Clientes Guardados!");
                }
                else 
                Utils.MostrarAlerta(Response, "Ocurrió un error al guardar los datos. Revise los logs para más información");
            }
            else
            {
                Utils.MostrarAlerta(Response, "Existen Errores en los datos ingresados, por favor revise");
                lblErrorClientes.Text = campos;
            }
        }

        protected void activarFacturacion(object sender, EventArgs e)
        {
            tblFactura.Visible = true;
            btnFacturar.Visible = true;
            llenarDdlTipoFac(ref ddlTipoFac);
        }

        protected void facturar(object sender, EventArgs e)
        {
            String campos = "";

            if (validarDatosFacturar(ref campos))
            {
                lblErrorFacturacion.Text = "";
                Hashtable factura = armarHashFactura();

                bool bien = ManejoClientesLogic.cupoHabilitado(codCliente, factura);

                if (bien)
                {
                    bien = ManejoClientesLogic.guardarFactura(factura);

                    if (bien)
                    {
                        long puntos = Convert.ToInt64(factura["mvfac_puntosacum"]);
                        ManejoClientesLogic.sumarPuntos(codCliente, puntos);

                        string facturada = imprimir(factura["mvfac_codigo"].ToString().Replace("'",""));
                        String redirect = String.Format("{0}?process=VIP.ManejoClientes&fwdOpt={1}&codCliente={2}&facturada={3}",
                            indexPage, fwdOpt, codCliente, facturada);

                        Response.Redirect(redirect);
                    }
                    else Utils.MostrarAlerta(Response, "Ocurrió un error al guardar los datos. Revise los logs para más información");
                }
                else
                Utils.MostrarAlerta(Response, "El cliente no tiene cupo disponible");
            }
            else
            {
                Utils.MostrarAlerta(Response, "Existen Errores en los datos ingresados, por favor revise");
                lblErrorFacturacion.Text = campos;
            }
        }

        private string imprimir(string codFactura)
        {
            Imprimir reporte = new Imprimir();

            Label lbvacio = new Label();
            DataTable dt = new DataTable();
            string[] Formulas = new string[0];
            string[] ValFormulas = new string[0];
            string usuario = ConfigurationManager.AppSettings["UID"];
            string password = ConfigurationManager.AppSettings["PWD"];
            Crypto miCripto = new Crypto(Crypto.CryptoProvider.TripleDES);
            miCripto.IV = ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
            miCripto.Key = ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
            string newPwd=miCripto.DescifrarCadena(password);
            string nomA = String.Format("Factura_{0}", codFactura);

            dt.Columns.Add("NOMBRE", typeof(string));
            dt.Columns.Add("VALOR", typeof(object));
            DataRow row = dt.NewRow();
            row[0] = "Codigo";
            row[1] = codFactura;
            dt.Rows.Add(row);

            reporte.DtValPar = dt;
            reporte.PreviewReport2("VIP.Facturacion", "", "", 1, 1, 1, "", "", nomA, "pdf", usuario, newPwd, Formulas, ValFormulas, lbvacio);

            return reporte.Documento;
        }

        private void agregarMensajeHabilitado()
        {
            String errorFac = "";

            if (ManejoClientesLogic.validarClienteParaFacturacion(codCliente, ref errorFac) && !chkInactivo.Checked)
            {
                lblHabilitadoFacturacion.Visible = true;
                lblHabilitadoFacturacion.ForeColor = Color.Green;
                lblHabilitadoFacturacion.Text = "Habilitado, agradezcale al cliente";
            }
            else
            {
                lblHabilitadoFacturacion.Visible = true;
                lblHabilitadoFacturacion.ForeColor = Color.Red;
                lblHabilitadoFacturacion.Text = "NO HABILITADO PARA TRANSACCIÓN";
                lblErrorFacturacion.Text = errorFac;
            }
        }

        private void construirFacturacion()
        {
            String errorFac = "";

            if (ManejoClientesLogic.validarClienteParaFacturacion(codCliente, ref errorFac) && !chkInactivo.Checked)
                btnActivarFacturacion.Visible = true;

            txtPtsPadre.Attributes.Add("style", "color:Green;font-size:Medium;font-weight:bold;");
            txtPtsHijo1.Attributes.Add("style", "color:Green;font-size:Medium;font-weight:bold;");
            txtPtsHijo2.Attributes.Add("style", "color:Green;font-size:Medium;font-weight:bold;");
        }

        private bool existeHijo1()
        {
            if(txtApeHijo1.Text != "") return true;
            if(txtCedHijo1.Text != "") return true;
            if(txtDirHijo1.Text != "") return true;
            if(txtFechaExpHijo1.Text != "") return true;
            if(txtNomHijo1.Text != "") return true; 
            if(txtPtsHijo1.Text != "") return true;
            if (txtTelfHijo1.Text != "") return true;

            return false;
        }

        private bool existeHijo2()
        {
            if (txtApeHijo2.Text != "") return true;
            if (txtCedHijo2.Text != "") return true;
            if (txtDirHijo2.Text != "") return true;
            if (txtFechaExpHijo2.Text != "") return true;
            if (txtNomHijo2.Text != "") return true;
            if (txtPtsHijo2.Text != "") return true;
            if (txtTelfHijo2.Text != "") return true;

            return false;
        }

        private bool validarDatosNuevo(ref String campos)
        {
            String camposPadre = "";
            String camposHijo1 = "";
            String camposHijo2 = "";

            validarDatosPadre(ref camposPadre);
            if (existeHijo1()) validarDatosHijo1(ref camposHijo1);
            if (existeHijo2()) validarDatosHijo2(ref camposHijo2);

            campos = camposPadre + camposHijo1 + camposHijo2;

            return campos.Length == 0;
        }

        private bool validarDatosPadre(ref String campos)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            DateTime dummy;

            if(txtNomPadre.Text == "") campos += "Debe llenar el nombre del Padre<br>";
            if (txtApePadre.Text == "") campos += "Debe llenar el apellido del Padre<br>";
            if (txtSAP.Text == "") campos += "Debe llenar el código SAP del Padre<br>";
            if (txtCedPadre.Text == "") campos += "Debe llenar la cédula del Padre<br>";
            if (txtFechaExpPadre.Text == "") campos += "Debe llenar la fecha de expedición de la Cédula del Padre<br>";
            else if (!DateTime.TryParseExact(txtFechaExpPadre.Text, "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dummy))
                campos += "La fecha de expedición de la Cédula del Padre tiene un formato inválido, por favor use yyyy-MM-dd<br>";
            if (txtCapEnd.Text == "") campos += "Debe llenar la capacidad de endeudamiento del Padre<br>";
            if (txtCupoDai.Text == "") campos += "Debe llenar el cupo Daimler del Padre<br>";

            if (txtCapEnd.Text != "" && !regex.IsMatch(txtCapEnd.Text)) campos += "La capacidad de endeudamiento debe ser un número<br>";
            if (txtCupoDai.Text != "" && !regex.IsMatch(txtCupoDai.Text)) campos += "El cupo Daimler debe ser un número<br>";
            if (txtFechaCorte.Text != "")
                if(!regex.IsMatch(txtFechaCorte.Text))
                    campos += "El día de corte debe ser un número<br>";
                else{
                    int dias = Convert.ToInt32(txtFechaCorte.Text);
                    if (dias > 30 || dias < 1) campos += "El día de corte debe ser un día de mes válido [1-30]<br>";
                }

            return campos.Length == 0;
        }

        private bool validarDatosHijo1(ref String campos)
        {
            DateTime dummy;

            if (txtNomHijo1.Text == "") campos += "Debe llenar el nombre del Hijo 1<br>";
            if (txtApeHijo1.Text == "") campos += "Debe llenar el apellido del Hijo 1<br>";
            if (txtCedHijo1.Text == "") campos += "Debe llenar la cédula del Hijo 1<br>";
            if (txtFechaExpHijo1.Text == "") campos += "Debe llenar la fecha de expedición de la Cédula del Hijo 1<br>";
            else if (!DateTime.TryParseExact(txtFechaExpHijo1.Text, "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dummy))
                campos += "La fecha de expedición de la Cédula del Hijo 1 tiene un formato inválido, por favor use yyyy-MM-dd<br>";

            return campos.Length == 0;
        }

        private bool validarDatosHijo2(ref String campos)
        {
            DateTime dummy;

            if (txtNomHijo2.Text == "") campos += "Debe llenar el nombre del Hijo 2<br>";
            if (txtApeHijo2.Text == "") campos += "Debe llenar el apellido del Hijo 2<br>";
            if (txtCedHijo2.Text == "") campos += "Debe llenar la cédula del Hijo 2<br>";
            if (txtFechaExpHijo2.Text == "") campos += "Debe llenar la fecha de expedición de la Cédula del Hijo 2<br>";
            else if (!DateTime.TryParseExact(txtFechaExpHijo2.Text, "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dummy))
                campos += "La fecha de expedición de la Cédula del Hijo 2 tiene un formato inválido, por favor use yyyy-MM-dd<br>";

            return campos.Length == 0;
        }

        private bool validarDatosFacturar(ref String campos)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            DateTime dummy;

            if (txtCodFac.Text == "") campos += "Debe llenar el código de factura<br>";
            if (txtAval.Text == "") campos += "Debe llenar el aval de Fenalco<br>";
            if (txtValorFac.Text == "") campos += "Debe llenar el valor de la factura<br>";
            else if (!regex.IsMatch(txtValorFac.Text)) campos += "El valor de la factura debe ser un valor numérico<br>";
            if (txtFechaPagoFac.Text != "")
                if (!DateTime.TryParseExact(txtFechaPagoFac.Text, "yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out dummy))
                    campos += "La fecha de pago tiene un formato inválido, por favor use yyyy-MM-dd<br>";

            return campos.Length == 0;
        }

        private void parametrizacionInicial()
        {
            llenarDdlTipoAfil(ref ddlTipoAfilHijo1, HIJO);
            llenarDdlTipoAfil(ref ddlTipoAfilHijo2, HIJO);
            llenarDdlTipoAfil(ref ddlTipoAfilPadre, PADRE);
        }

        private void construirTablaConsultaHijo()
        {
            lblPadre.Text = "PADRE";
            lblHijo1.Text = "HIJO";
            lblHijo2.Visible = false;

            tblPadre.Visible = true;
            tblHijo1.Visible = true;
            tblHijo2.Visible = false;

            txtApeHijo1.ReadOnly = true;
            txtApeHijo2.ReadOnly = true;
            txtApePadre.ReadOnly = true;
            txtCapEnd.ReadOnly = true;
            txtCedHijo1.ReadOnly = true;
            txtCedHijo2.ReadOnly = true;
            txtCedPadre.ReadOnly = true;
            txtCupoDai.ReadOnly = true;
            txtDirHijo1.ReadOnly = true;
            txtDirHijo2.ReadOnly = true;
            txtDirPadre.ReadOnly = true;
            txtFechaCorte.ReadOnly = true;
            txtFechaExpHijo1.ReadOnly = true;
            txtFechaExpHijo2.ReadOnly = true;
            txtFechaExpPadre.ReadOnly = true;
            txtFin.ReadOnly = true;
            txtNomHijo1.ReadOnly = true;
            txtNomHijo2.ReadOnly = true;
            txtNomPadre.ReadOnly = true;
            txtPtsHijo1.ReadOnly = true;
            txtPtsHijo2.ReadOnly = true;
            txtPtsPadre.ReadOnly = true;
            txtSAP.ReadOnly = true;
            txtTelfHijo1.ReadOnly = true;
            txtTelfHijo2.ReadOnly = true;
            txtTelfPadre.ReadOnly = true;

            txtCupoUtil.ReadOnly = true;
            txtCupoDisponible.ReadOnly = true;

            ddlTipoAfilHijo1.Enabled = false;
            ddlTipoAfilHijo2.Enabled = false;
            ddlTipoAfilPadre.Enabled = false;

            chkInactivo.Enabled = false;

            agregarMensajeHabilitado();
        }

        private void construirTablaConsultaPadre()
        {
            lblPadre.Text = "PADRE";
            lblHijo1.Text = "HIJO 1";
            lblHijo2.Text = "HIJO 2";

            tblPadre.Visible = true;
            tblHijo1.Visible = true;
            tblHijo2.Visible = true;

            txtApeHijo1.ReadOnly = true;
            txtApeHijo2.ReadOnly = true;
            txtApePadre.ReadOnly = true;
            txtCapEnd.ReadOnly = true;
            txtCedHijo1.ReadOnly = true;
            txtCedHijo2.ReadOnly = true;
            txtCedPadre.ReadOnly = true;
            txtCupoDai.ReadOnly = true;
            txtDirHijo1.ReadOnly = true;
            txtDirHijo2.ReadOnly = true;
            txtDirPadre.ReadOnly = true;
            txtFechaCorte.ReadOnly = true;
            txtFechaExpHijo1.ReadOnly = true;
            txtFechaExpHijo2.ReadOnly = true;
            txtFechaExpPadre.ReadOnly = true;
            txtFin.ReadOnly = true;
            txtNomHijo1.ReadOnly = true;
            txtNomHijo2.ReadOnly = true;
            txtNomPadre.ReadOnly = true;
            txtPtsHijo1.ReadOnly = true;
            txtPtsHijo2.ReadOnly = true;
            txtPtsPadre.ReadOnly = true;
            txtSAP.ReadOnly = true;
            txtTelfHijo1.ReadOnly = true;
            txtTelfHijo2.ReadOnly = true;
            txtTelfPadre.ReadOnly = true;

            txtCupoUtil.ReadOnly = true;
            txtCupoDisponible.ReadOnly = true;

            ddlTipoAfilHijo1.Enabled = false;
            ddlTipoAfilHijo2.Enabled = false;
            ddlTipoAfilPadre.Enabled = false;

            chkInactivo.Enabled = false;

            agregarMensajeHabilitado();
        }

        private void construirTablaEdicionHijo()
        {
            lblPadre.Text = "PADRE";
            lblHijo1.Text = "HIJO";
            lblHijo2.Visible = false;

            btnEditar.Visible = true;

            tblPadre.Visible = true;
            tblHijo1.Visible = true;
            tblHijo2.Visible = false;

            txtApePadre.ReadOnly = true;
            txtCapEnd.ReadOnly = true;
            txtCedPadre.ReadOnly = true;
            txtCupoDai.ReadOnly = true;
            txtDirPadre.ReadOnly = true;
            txtFechaCorte.ReadOnly = true;
            txtFechaExpPadre.ReadOnly = true;
            txtFin.ReadOnly = true;
            txtPtsPadre.ReadOnly = true;
            txtSAP.ReadOnly = true;
            txtNomPadre.ReadOnly = true;
            txtTelfPadre.ReadOnly = true;
            ddlTipoAfilPadre.Enabled = false;
            txtCupoUtil.ReadOnly = true;
            txtCupoDisponible.ReadOnly = true;

            txtApeHijo1.Enabled = true;
            txtCedHijo1.Enabled = true;
            txtDirHijo1.Enabled = true;
            txtFechaExpHijo1.Enabled = true;
            txtNomHijo1.Enabled = true;
            txtPtsHijo1.Enabled = true;
            txtTelfHijo1.Enabled = true;
            ddlTipoAfilHijo1.Enabled = true;
            
            txtApeHijo2.ReadOnly = true;
            txtCedHijo2.ReadOnly = true;
            txtDirHijo2.ReadOnly = true;
            txtFechaExpHijo2.ReadOnly = true;
            txtNomHijo2.ReadOnly = true;
            txtPtsHijo2.ReadOnly = true;
            txtTelfHijo2.ReadOnly = true;
            ddlTipoAfilHijo2.Enabled = false;

            chkInactivo.Enabled = false;
        }

        private void construirTablaEdicionPadre()
        {
            lblPadre.Text = "PADRE";
            lblHijo1.Text = "HIJO 1";
            lblHijo2.Text = "HIJO 2";

            btnEditar.Visible = true;

            tblPadre.Visible = true;
            tblHijo1.Visible = true;
            tblHijo2.Visible = true;

            tblPadre.Visible = true;
            tblHijo1.Visible = true;
            tblHijo2.Visible = true;

            txtApePadre.Enabled = true;
            txtCapEnd.Enabled = true;
            txtCedPadre.Enabled = true;
            txtCupoDai.Enabled = true;
            txtDirPadre.Enabled = true;
            txtFechaCorte.Enabled = true;
            txtFechaExpPadre.Enabled = true;
            txtFin.Enabled = true;
            txtPtsPadre.Enabled = true;
            txtSAP.Enabled = true;
            txtNomPadre.Enabled = true;
            txtTelfPadre.Enabled = true;
            ddlTipoAfilPadre.Enabled = true;
            txtCupoUtil.ReadOnly = true;
            txtCupoDisponible.ReadOnly = true;

            txtApeHijo1.Enabled = true;
            txtCedHijo1.Enabled = true;//txtCedHijo1.Text == "" ? true : false;
            txtDirHijo1.Enabled = true;
            txtFechaExpHijo1.Enabled = true;
            txtNomHijo1.Enabled = true;
            txtPtsHijo1.Enabled = true;
            txtTelfHijo1.Enabled = true;
            ddlTipoAfilHijo1.Enabled = true;

            txtApeHijo2.Enabled = true;
            txtCedHijo2.Enabled = true;//txtCedHijo2.Text == "" ? true : false; ;
            txtDirHijo2.Enabled = true;
            txtFechaExpHijo2.Enabled = true;
            txtNomHijo2.Enabled = true;
            txtPtsHijo2.Enabled = true;
            txtTelfHijo2.Enabled = true;
            ddlTipoAfilHijo2.Enabled = true;

            chkInactivo.Enabled = true;
        }

        private void construirTablaNuevo()
        {
            lblPadre.Text = "PADRE";
            lblHijo1.Text = "HIJO 1";
            lblHijo2.Text = "HIJO 2";

            tblPadre.Visible = true;
            tblHijo1.Visible = true;
            tblHijo2.Visible = true;

            lblPtsPadre.Visible = false;
            lblPtsHijo1.Visible = false;
            lblPtsHijo2.Visible = false;
            txtPtsPadre.Visible = false;
            txtPtsHijo1.Visible = false;
            txtPtsHijo2.Visible = false;

            txtCupoUtil.Visible = false;
            txtCupoDisponible.Visible = false;

            btnAgregarNuevo.Visible = true;

            chkInactivo.Enabled = true;
        }

        private void llenarClientes()
        {
            Hashtable[] clientes = ManejoClientesLogic.obtenerClientes(codCliente);

            cargarPadre(clientes[0]);
            if (clientes[1] != null) cargarHijo1(clientes[1]);
            if (clientes[2] != null) cargarHijo2(clientes[2]);
        }

        private void cargarPadre(Hashtable table)
        {
            txtNomPadre.Text = table["MCLI_NOMBRE"].ToString();
            txtApePadre.Text = table["MCLI_APELLIDO"].ToString();
            ddlTipoAfilPadre.SelectedValue = table["TAFI_CODIGO"].ToString();
            txtSAP.Text = table["MCLI_SAP"].ToString();
            txtCedPadre.Text = table["MCLI_NIT"].ToString();
            lblCodProdPadre.Text = table["MCLI_CODIGO"].ToString();
            txtFechaExpPadre.Text = ((DateTime)table["MCLI_FECHEXPE"]).ToString("yyyy-MM-dd");
            txtTelfPadre.Text = table["MCLI_TELEFONO"].ToString();
            txtDirPadre.Text = table["MCLI_DIRECCION"].ToString();
            txtCapEnd.Text = table["MCLI_CAPENDEU"].ToString();
            txtCupoDai.Text = table["MCLI_CUPOCLI"].ToString();
            txtFechaCorte.Text = table["MCLI_DIACORTE"].ToString();
            txtFin.Text = table["MCLI_FINANCIERA"].ToString();
            txtPtsPadre.Text = table["MCLI_PUNTOS"].ToString();

            txtCupoUtil.Text = ManejoClientesLogic.getCupoUtilizado(table["MCLI_CODIGO"].ToString());
            txtCupoDisponible.Text = (Convert.ToInt64(txtCupoDai.Text) - Convert.ToInt64(txtCupoUtil.Text)).ToString();

            chkInactivo.Checked = table["MCLI_INACTIVO"].ToString().Equals("SI") ? true : false;
        }

        private void cargarHijo1(Hashtable table)
        {
            txtNomHijo1.Text = table["MCLI_NOMBRE"].ToString();
            txtApeHijo1.Text = table["MCLI_APELLIDO"].ToString();
            ddlTipoAfilHijo1.SelectedValue = table["TAFI_CODIGO"].ToString();
            txtCedHijo1.Text = table["MCLI_NIT"].ToString();
            lblCodProdHijo1.Text = table["MCLI_CODIGO"].ToString(); ;
            txtFechaExpHijo1.Text = ((DateTime)table["MCLI_FECHEXPE"]).ToString("yyyy-MM-dd");
            txtTelfHijo1.Text = table["MCLI_TELEFONO"].ToString();
            txtDirHijo1.Text = table["MCLI_DIRECCION"].ToString();
            txtPtsHijo1.Text = table["MCLI_PUNTOS"].ToString();
        }

        private void cargarHijo2(Hashtable table)
        {
            txtNomHijo2.Text = table["MCLI_NOMBRE"].ToString();
            txtApeHijo2.Text = table["MCLI_APELLIDO"].ToString();
            ddlTipoAfilHijo2.SelectedValue = table["TAFI_CODIGO"].ToString();
            txtCedHijo2.Text = table["MCLI_NIT"].ToString();
            lblCodProdHijo2.Text = table["MCLI_CODIGO"].ToString(); ;
            txtFechaExpHijo2.Text = ((DateTime)table["MCLI_FECHEXPE"]).ToString("yyyy-MM-dd");
            txtTelfHijo2.Text = table["MCLI_TELEFONO"].ToString();
            txtDirHijo2.Text = table["MCLI_DIRECCION"].ToString();
            txtPtsHijo2.Text = table["MCLI_PUNTOS"].ToString();
        }

        private void llenarDdlTipoAfil(ref DropDownList ddl, string nivel)
        {
            String sql = String.Format("select tafi_codigo, tafi_nombre from tvipafiliacion where tafi_nivel='{0}'",nivel);
            bind.PutDatasIntoDropDownList(ddl,sql);
        }

        private void llenarDdlTipoFac(ref DropDownList ddl)
        {
            bind = new DatasToControls();
            String sql = "select tvfac_codigo, tvfac_nombre from tvipfactura";
            bind.PutDatasIntoDropDownList(ddl, sql);
        }

        private Hashtable armarHashPadre()
        {
            Hashtable hash = new Hashtable();

            hash.Add("mcli_nombre", String.Format("'{0}'", txtNomPadre.Text.Trim()));
            hash.Add("mcli_apellido", String.Format("'{0}'", txtApePadre.Text.Trim()));
            hash.Add("mcli_nombrecompleto", String.Format("'{0} {1}'", txtNomPadre.Text.Trim(), txtApePadre.Text.Trim()));
            hash.Add("mcli_codigo", String.Format("'{0}'", lblCodProdPadre.Text.Trim()));
            hash.Add("tafi_codigo", String.Format("'{0}'", ddlTipoAfilPadre.SelectedValue));
            hash.Add("mcli_sap", String.Format("'{0}'", txtSAP.Text.Trim()));
            hash.Add("mcli_nit", String.Format("'{0}'", txtCedPadre.Text.Trim()));
            hash.Add("mcli_fechexpe", String.Format("'{0}'", txtFechaExpPadre.Text));
            hash.Add("mcli_telefono", String.Format("'{0}'", txtTelfPadre.Text.Trim()));
            hash.Add("mcli_direccion", String.Format("'{0}'", txtDirPadre.Text.Trim()));
            hash.Add("mcli_puntos", txtPtsPadre.Text == "" ? "0" : txtPtsPadre.Text.Trim());
            hash.Add("mcli_fecharegi", String.Format("'{0}'",DateTime.Now.ToString("yyyy-MM-dd")));

            hash.Add("mcli_financiera", String.Format("'{0}'", txtFin.Text.Trim()));
            hash.Add("mcli_capendeu", txtCapEnd.Text.Trim());
            hash.Add("mcli_cupocli", txtCupoDai.Text.Trim());
            hash.Add("mcli_diacorte", txtFechaCorte.Text == "" ? "null" : txtFechaCorte.Text);
            hash.Add("mcli_inactivo", chkInactivo.Checked == true ? "'SI'" : "'NO'");
            return hash;
        }

        private Hashtable armarHashHijo1()
        {
            Hashtable hash = new Hashtable();

            hash.Add("mcli_nombre", String.Format("'{0}'", txtNomHijo1.Text.Trim()));
            hash.Add("mcli_apellido", String.Format("'{0}'", txtApeHijo1.Text.Trim()));
            hash.Add("mcli_nombrecompleto", String.Format("'{0} {1}'", txtNomHijo1.Text.Trim(), txtApeHijo1.Text.Trim()));
            hash.Add("mcli_codigo", String.Format("'{0}'", lblCodProdHijo1.Text.Trim()));
            hash.Add("tafi_codigo", String.Format("'{0}'", ddlTipoAfilHijo1.SelectedValue));
            hash.Add("mcli_sap", String.Format("'{0}'", txtSAP.Text.Trim()));
            hash.Add("mcli_nit", String.Format("'{0}'", txtCedHijo1.Text.Trim()));
            hash.Add("mcli_fechexpe", String.Format("'{0}'", txtFechaExpHijo1.Text));
            hash.Add("mcli_telefono", String.Format("'{0}'", txtTelfHijo1.Text.Trim()));
            hash.Add("mcli_direccion", String.Format("'{0}'", txtDirHijo1.Text.Trim()));
            hash.Add("mcli_puntos", txtPtsHijo1.Text == "" ? "0" : txtPtsHijo1.Text.Trim());
            hash.Add("mcli_fecharegi", String.Format("'{0}'", DateTime.Now.ToString("yyyy-MM-dd")));

            return hash;
        }

        private Hashtable armarHashHijo2()
        {
            Hashtable hash = new Hashtable();

            hash.Add("mcli_nombre", String.Format("'{0}'", txtNomHijo2.Text.Trim()));
            hash.Add("mcli_apellido", String.Format("'{0}'", txtApeHijo2.Text.Trim()));
            hash.Add("mcli_nombrecompleto", String.Format("'{0} {1}'", txtNomHijo2.Text.Trim(), txtApeHijo2.Text.Trim()));
            hash.Add("mcli_codigo", String.Format("'{0}'", lblCodProdHijo2.Text.Trim()));
            hash.Add("tafi_codigo", String.Format("'{0}'", ddlTipoAfilHijo2.SelectedValue));
            hash.Add("mcli_sap", String.Format("'{0}'", txtSAP.Text.Trim()));
            hash.Add("mcli_nit", String.Format("'{0}'", txtCedHijo2.Text.Trim()));
            hash.Add("mcli_fechexpe", String.Format("'{0}'", txtFechaExpHijo2.Text));
            hash.Add("mcli_telefono", String.Format("'{0}'", txtTelfHijo2.Text.Trim()));
            hash.Add("mcli_direccion", String.Format("'{0}'", txtDirHijo2.Text.Trim()));
            hash.Add("mcli_puntos", txtPtsHijo2.Text == "" ? "0" : txtPtsHijo2.Text.Trim());
            hash.Add("mcli_fecharegi", String.Format("'{0}'", DateTime.Now.ToString("yyyy-MM-dd")));

            return hash;
        }

        private Hashtable armarHashFactura()
        {
            Hashtable hash = new Hashtable();
            double valFac = Convert.ToDouble(txtValorFac.Text);

            hash.Add("mvfac_codigo", String.Format("'{0}'", txtCodFac.Text.Trim()));
            hash.Add("mcli_codigo", String.Format("'{0}'", lblCodProdPadre.Text));
            hash.Add("mcli_codigocliente", String.Format("'{0}'", codCliente));
            hash.Add("mvfac_nombrecliente", String.Format("'{0}'", ManejoClientesLogic.getNombreCliente(codCliente)));
            hash.Add("tvfac_codigo", String.Format("'{0}'", ddlTipoFac.SelectedValue));
            hash.Add("tvest_codigo", String.Format("'{0}'", "AD"));
            hash.Add("mvfac_valor", String.Format("{0}", valFac));
            hash.Add("mvfac_fechatrans", String.Format("'{0}'", DateTime.Now.ToString("yyyy-MM-dd")));
            hash.Add("mvfac_fechapago", String.Format("'{0}'", ManejoClientesLogic.calcularFechaPago(txtFechaCorte.Text, txtFechaPagoFac.Text, valFac)));
            hash.Add("mvfac_diasprorroga", String.Format("{0}", 0));
            hash.Add("mvfac_puntosacum", String.Format("{0}", ManejoClientesLogic.obtenerPuntosFactura(valFac)));
            hash.Add("mvfac_numaval", String.Format("'{0}'", txtAval.Text.Trim()));
            hash.Add("susu_codigo", ManejoClientesLogic.getCodUsuario());

            return hash;
        }
	}
}