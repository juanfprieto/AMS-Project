using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Vehiculos          // en todos los select falta meter el codigo del pedido ademas del numero del predido
{
	public partial class Creditos : System.Web.UI.UserControl
	{
        string proceso;
        string indexPage = ConfigurationManager.AppSettings["MainIndexPage"].ToString();
        double valorFinanciera = 0;
        protected void Page_Load(object sender, EventArgs e)
		{
            proceso = Request.QueryString["prc"].ToString();
            if (!IsPostBack)
            {
                CargarControles();
                if (proceso.Equals("R"))
                    IniciarRegistro();
                else if (proceso.Equals("M"))
                    IniciarModificacion();
                else if (proceso.Equals("S"))
                    IniciarSolicitud();
                else if (proceso.Equals("A"))
                    IniciarAprobacion();
                else if (proceso.Equals("D"))
                    IniciarDesembolso();
                else if(proceso.Equals("L"))
                    IniciarLegalizacion();
                ViewState["PROC"] = proceso;
            }
        }
        #region Metodos
        private void CargarControles(){
            DatasToControls bind = new DatasToControls();
            //bind.PutDatasIntoDropDownList(ddlPrefPed, "SELECT PDOC_CODIGO, PDOC_NOMBRE FROM PDOCUMENTO WHERE TDOC_TIPODOCU='PC';");
            Utils.llenarPrefijos(Response, ref ddlPrefPed , "%", "%", "PC");
            bind.PutDatasIntoDropDownList(ddlOrigen, "SELECT TORIG_CODIGO,TORIG_DESC FROM TORIGENCREDITO;");
            bind.PutDatasIntoDropDownList(ddlDesembolso, "SELECT TDES_CODIGO, TDES_DESC from TDESEMBOLSO;");
            CambioPrefijoPedido(null, null);
        }

        private void cargarInformacion(String prefijoPedido, String numeroPedido)
        {   
            if (prefijoPedido == "" || numeroPedido == "") return;
            DatasToControls bind = new DatasToControls();
            cc_cliente.Text = DBFunctions.SingleData("select mnit_nit from dbxschema.mpedidovehiculo where pdoc_codigo='" +prefijoPedido + "' and mped_numepedi=" + numeroPedido + ";");

            nom_cliente.Text = DBFunctions.SingleData("select mnit_nombres CONCAT ' ' CONCAT mnit_apellidos  from dbxschema.mnit where mnit_nit='" + cc_cliente.Text + "';");

            tel_cliente.Text = DBFunctions.SingleData("select mnit_telefono from dbxschema.mnit where  mnit_nit='" + cc_cliente.Text + "';");

            String refe = DBFunctions.SingleData("select pcat_codigo from dbxschema.mpedidovehiculo where pdoc_codigo='" +prefijoPedido + "' and mped_numepedi=" + numeroPedido + ";");

            catalogo_vehiculo.Text = DBFunctions.SingleData("select PCAT_CODIGO CONCAT ' ' CONCAT PCAT_DESCRIPCION FROM dbxschema.pcatalogovehiculo where pcat_codigo='" + refe + "';");

            String cc_vendedor = DBFunctions.SingleData("select pven_codigo from dbxschema.mpedidovehiculo where pdoc_codigo='" +prefijoPedido + "' and mped_numepedi=" + numeroPedido + ";");

            nom_vendedor.Text = DBFunctions.SingleData("select pven_nombre  from dbxschema.pvendedor where pven_codigo='" + cc_vendedor + "';");

            LabelFinanciera.Text = DBFunctions.SingleData("select MPED_NOMBFINC from dbxschema.mpedidovehiculo where pdoc_codigo='" +prefijoPedido + "' and mped_numepedi=" + numeroPedido + ";");

            LabelFinancieraa.Text = DBFunctions.SingleData("select MNIT_NOMBRES  from dbxschema.mnit where mnit_nit='" + LabelFinanciera.Text + "';");
            valorFinanciera = Convert.ToDouble(DBFunctions.SingleData("select coalesce(MPED_valofinc,0) from dbxschema.mpedidovehiculo where pdoc_codigo='" +prefijoPedido + "' and mped_numepedi=" + numeroPedido + ";"));

            bind.PutDatasIntoDropDownList(ddlBancoCheque, "select PBAN_CODIGO CONCAT '  ' CONCAT PBAN_DESCRIPCION  from DBXSCHEMA.PBANCO  order by PBAN_CODIGO;");
            ddlBancoCheque.Items.Insert(0, "Seleccione..");
            bind.PutDatasIntoDropDownList(ddlTipoDesembolso, "select TTIP_NOMBRE from DBXSCHEMA.TTIPOPAGO where  TTIP_CODIGO='B' or TTIP_CODIGO='C';");
            ddlTipoDesembolso.Items.Insert(0, "Seleccione..");
       
        }



        private void IniciarRegistro(){
            if (Request.QueryString["crd"]!=null)
            Utils.MostrarAlerta(Response, "El crédito número " + Request.QueryString["crd"] + " ha sido creado.");
            txtFechaSolicitud.Text = DateTime.Now.ToString("yyyy-MM-dd");
            plcRegistro.Visible = true;
            btnCrear.Visible = true;
            lblNumero.Text = DBFunctions.SingleData("SELECT MAX(MCRED_CODIGO)+1 from MCREDITOFINANCIERA;");
            if (lblNumero.Text.Length == 0) lblNumero.Text = "1";
        }
        private void IniciarModificacion(){
            DatasToControls bind = new DatasToControls();
            if (Request.QueryString["crd"] != null)
            Utils.MostrarAlerta(Response, "El crédito número " + Request.QueryString["crd"] + " ha sido modificado.");
            bind.PutDatasIntoDropDownList(ddlCredito,
                "SELECT MC.MCRED_CODIGO, RTRIM(CHAR(MC.MCRED_CODIGO)) CONCAT ': ' CONCAT "+
                "MC.PDOC_CODIPEDI CONCAT '-' CONCAT RTRIM(CHAR(MC.MPED_NUMEPEDI)) CONCAT ' - ' CONCAT "+
                "COALESCE(MN.MNIT_NOMBRES,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDOS,'') " +
                "FROM MCREDITOFINANCIERA MC, MPEDIDOVEHICULO MP "+
                "LEFT JOIN MNIT MN ON MN.MNIT_NIT=MP.MNIT_NITSOLICITA "+
                "WHERE MC.TESTA_CODIGO = 1 AND MC.PDOC_CODIPEDI=MP.PDOC_CODIGO AND MC.MPED_NUMEPEDI=MP.MPED_NUMEPEDI");
            plcCredito.Visible = true;
            btnActualizar.Visible = true;
            plcAnular.Visible = true;
        }
        private void IniciarSolicitud()
        {
            DatasToControls bind = new DatasToControls();
            if (Request.QueryString["crd"] != null)   // Solo carga pedidos en estado CREADO o ASIGNADO
            Utils.MostrarAlerta(Response, "El crédito número " + Request.QueryString["crd"] + " ha sido solicitado.");
            bind.PutDatasIntoDropDownList(ddlCredito,
                "SELECT MC.MCRED_CODIGO, RTRIM(CHAR(MC.MCRED_CODIGO)) CONCAT ': ' CONCAT " +
                "MC.PDOC_CODIPEDI CONCAT '-' CONCAT RTRIM(CHAR(MC.MPED_NUMEPEDI)) CONCAT ' - ' CONCAT " +
                "COALESCE(MN.MNIT_NOMBRES,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDOS,'') " +
                "FROM MCREDITOFINANCIERA MC, MPEDIDOVEHICULO MP " +
                "LEFT JOIN MNIT MN ON MN.MNIT_NIT=MP.MNIT_NITSOLICITA " +
                "WHERE MC.TESTA_CODIGO=1 AND MC.PDOC_CODIPEDI=MP.PDOC_CODIGO AND MC.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MP.TEST_TIPOESTA < 30");
            txtFechaSolicitud.Text = DateTime.Now.ToString("yyyy-MM-dd");
            plcCredito.Visible = true;
            btnSolicitar.Visible = true;
            plcAnular.Visible = true;
            DeshabilitarRegistro();
            
        }
        private void IniciarAprobacion(){
            DatasToControls bind = new DatasToControls();
            if (Request.QueryString["crd"] != null)  // Solo carga pedidos en estado CREADO o ASIGNADO
            Utils.MostrarAlerta(Response, "El crédito número " + Request.QueryString["crd"] + " ha sido aprobado.");
            bind.PutDatasIntoDropDownList(ddlCredito,
                "SELECT MC.MCRED_CODIGO, RTRIM(CHAR(MC.MCRED_CODIGO)) CONCAT ': ' CONCAT " +
                "MC.PDOC_CODIPEDI CONCAT '-' CONCAT RTRIM(CHAR(MC.MPED_NUMEPEDI)) CONCAT ' - ' CONCAT " +
                "COALESCE(MN.MNIT_NOMBRES,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDOS,'') " +
                "FROM MCREDITOFINANCIERA MC, MPEDIDOVEHICULO MP " +
                "LEFT JOIN MNIT MN ON MN.MNIT_NIT=MP.MNIT_NITSOLICITA " +
                "WHERE MC.TESTA_CODIGO=2 AND MC.PDOC_CODIPEDI=MP.PDOC_CODIGO AND MC.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MP.TEST_TIPOESTA < 30");
            plcCredito.Visible = true;
            txtFechaAprobacion.Text = DateTime.Now.ToString("yyyy-MM-dd");
            btnAprobar.Visible = true;
            plcAnular.Visible = true;
            DeshabilitarRegistro();
            DeshabilitarSolicitud();
        }
        private void IniciarDesembolso()
        {
            DatasToControls bind = new DatasToControls();
            if (Request.QueryString["crd"] != null)  // Solo carga pedidos en estado FACTURADO
            Utils.MostrarAlerta(Response, "El crédito número " + Request.QueryString["crd"] + " ha sido recibido el desembolso.");
            bind.PutDatasIntoDropDownList(ddlCredito,
                "SELECT MC.MCRED_CODIGO, RTRIM(CHAR(MC.MCRED_CODIGO)) CONCAT ': ' CONCAT " +
                "MC.PDOC_CODIPEDI CONCAT '-' CONCAT RTRIM(CHAR(MC.MPED_NUMEPEDI)) CONCAT ' - ' CONCAT " +
                "COALESCE(MN.MNIT_NOMBRES,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDOS,'') " +
                "FROM MCREDITOFINANCIERA MC, MPEDIDOVEHICULO MP " +
                "LEFT JOIN MNIT MN ON MN.MNIT_NIT=MP.MNIT_NITSOLICITA " +
                "WHERE MC.TESTA_CODIGO=3 AND MC.PDOC_CODIPEDI=MP.PDOC_CODIGO AND MC.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MP.TEST_TIPOESTA = 30");
            txtFechaDesembolso.Text = DateTime.Now.ToString("yyyy-MM-dd");
            plcCredito.Visible = true;
            btnDesembolso.Visible = true;
            DeshabilitarRegistro();
            DeshabilitarSolicitud();
            DeshabilitarAprobacion();
        }
        private void IniciarLegalizacion() {
            DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(ddlCredito, // Solo carga pedidos en estado FACTURADO
                "SELECT MC.MCRED_CODIGO, RTRIM(CHAR(MC.MCRED_CODIGO)) CONCAT ': ' CONCAT " +
                "MC.PDOC_CODIPEDI CONCAT '-' CONCAT RTRIM(CHAR(MC.MPED_NUMEPEDI)) CONCAT ' - ' CONCAT " +
                "COALESCE(MN.MNIT_NOMBRES,'') CONCAT ' ' CONCAT COALESCE(MN.MNIT_APELLIDOS,'') " +
                "FROM MCREDITOFINANCIERA MC, MPEDIDOVEHICULO MP " +
                "LEFT JOIN MNIT MN ON MN.MNIT_NIT=MP.MNIT_NITSOLICITA " +
                "WHERE MC.TESTA_CODIGO=4 AND MC.TDES_CODIGO=1 AND MC.PDOC_CODIPEDI=MP.PDOC_CODIGO AND MC.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MP.TEST_TIPOESTA = 30");
            plcCredito.Visible = true;
            txtFechaLegalizacion.Text = DateTime.Now.ToString("yyyy-MM-dd");
            btnLegalizar.Visible = true;
            DeshabilitarRegistro();
            DeshabilitarAprobacion();
            DeshabilitarDesembolso();
            DeshabilitarSolicitud();
        }
        private void DeshabilitarRegistro()
        {
            ddlPrefPed.Enabled = false;
            ddlNumPed.Enabled = false;
            txtValSolicitado.Enabled = false;
            txtMeses.Enabled = false;
            ddlOrigen.Enabled = false;
        }
        private void DeshabilitarSolicitud()
        {
            txtFechaSolicitud.Enabled = false;
            txtObsSolicitud.Enabled = false;
        }
        private void DeshabilitarAprobacion()
        {
            txtValAprobado.Enabled = false;
            txtFechaAprobacion.Enabled = false;
            ddlDesembolso.Enabled = false;
            txtNumAprobacion.Enabled = false;
        }
        private void DeshabilitarDesembolso()
        {
            txtFechaDesembolso.Enabled = false;
            txtCheque.Enabled = false;
            txtValorDesembolso.Enabled = false;
        }
        private bool CargarCredito()
        {
            DataSet dsCredito = new DataSet();
            DataRow drCredito;
            proceso = ViewState["PROC"].ToString();
            DBFunctions.Request(dsCredito, IncludeSchema.NO, "SELECT * FROM MCREDITOFINANCIERA WHERE MCRED_CODIGO=" + ddlCredito.SelectedValue + ";");
            if (dsCredito.Tables[0].Rows.Count == 0)
                return (false);
            drCredito=dsCredito.Tables[0].Rows[0];
            plcRegistro.Visible = true;
            plcCredito.Visible = false;
            lblNumero.Text = ddlCredito.SelectedValue;
            LabelFinanciera.Text = drCredito["MNIT_FINANCIERA"].ToString();
            LabelFinancieraa.Text = DBFunctions.SingleData("SELECT mnit_nombres CONCAT ' ' CONCAT mnit_apellidos FROM mnit where mnit_nit = '"+LabelFinanciera.Text+"';");
            ddlPrefPed.SelectedIndex = ddlPrefPed.Items.IndexOf(ddlPrefPed.Items.FindByValue(drCredito["PDOC_CODIPEDI"].ToString()));
            CambioPrefijoPedido(null, null);
            ddlNumPed.SelectedIndex = ddlNumPed.Items.IndexOf(ddlNumPed.Items.FindByValue(drCredito["MPED_NUMEPEDI"].ToString()));

            cargarInformacion(ddlPrefPed.Text, ddlNumPed.Text);

            txtValSolicitado.Text = Convert.ToDouble(drCredito["MCRED_VALOSOLI"]).ToString("#,##0");
            txtMeses.Text = Convert.ToInt32(drCredito["MCRED_MESES"]).ToString();
            ddlOrigen.SelectedIndex = ddlOrigen.Items.IndexOf(ddlOrigen.Items.FindByValue(drCredito["TORIG_CODIGO"].ToString()));
            plcCredito.Visible = false;
            if (proceso == "A" || proceso == "D" || proceso == "L")
            {
                txtFechaSolicitud.Text = Convert.ToDateTime(drCredito["MCRED_FECHASOLI"]).ToString("yyyy-MM-dd");
                txtObsSolicitud.Text = drCredito["MCRED_OBSESOLI"].ToString();
            }
            if (proceso == "D" || proceso == "L")
            {
                txtValAprobado.Text = Convert.ToDouble(drCredito["MCRED_VALOAPROB"]).ToString("#,##0");
                txtFechaAprobacion.Text = Convert.ToDateTime(drCredito["MCRED_FECHASOLI"]).ToString("yyyy-MM-dd");
                ddlDesembolso.SelectedIndex = ddlDesembolso.Items.IndexOf(ddlDesembolso.Items.FindByValue(drCredito["TDES_CODIGO"].ToString()));
                txtNumAprobacion.Text = drCredito["MCRED_NUMEAPROB"].ToString();
            }
            if (proceso == "L")
            {
                txtFechaDesembolso.Text = Convert.ToDateTime(drCredito["MCRED_FECHADESEM"]).ToString("yyyy-MM-dd");
                txtCheque.Text = drCredito["MCRED_CHEQDESEM"].ToString();
                txtValorDesembolso.Text = Convert.ToDouble(drCredito["MCRED_VALOAPROB"]).ToString("#,##0");
            }
            return (true);
        }
        private bool ValidarDatosSolicitud()
        {
            DateTime fechaC;
            try
            {
                fechaC = Convert.ToDateTime(txtFechaSolicitud.Text);
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Fecha inválida.");
                return false;
            }
            if (txtObsSolicitud.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar las observaciones.");
                return false;
            }
            return true;
        }
        private bool ValidarDatosDesembolso()
        {
            DateTime fechaC;
            double valorAprobado;
            try
            {
                valorAprobado = Convert.ToDouble(txtValorDesembolso.Text);
                if (valorAprobado < 0) throw (new Exception());
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Valor del desembolso inválido.");
                return false;
            }
            if ((ddlTipoDesembolso.SelectedValue.Equals("Seleccione..")) || (ddlBancoCheque.SelectedValue.Equals("Seleccione..")))
            {
                Utils.MostrarAlerta(Response, "Debe seleccionar un banco y un tipo de Desembolso.");
                return false;
            }

            try
            {
                fechaC = Convert.ToDateTime(txtFechaDesembolso.Text);
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Fecha inválida.");
                return false;
            }
            if (ddlTipoDesembolso.SelectedValue.Equals("Cheque"))
            {
                if (txtCheque.Text.Trim().Length == 0)
                {
                    Utils.MostrarAlerta(Response, "Debe ingresar el número de cheque.");
                    return false;
                }
            }
            if (valorFinanciera != valorAprobado)
            {
                Utils.MostrarAlerta(Response, "El valor desembolsado  debe ser igual al valor aprobado.");
                return false;
            }
            return true;
        }
        private bool ValidarDatosRegistro()
        {

            if (proceso.Equals("R"))
            {
                if (Convert.ToInt16(DBFunctions.SingleData("select count (*) from MCREDITOFINANCIERA where PDOC_CODIPEDI = '" + ddlPrefPed.SelectedValue + "' and MPED_NUMEPEDI = " + ddlNumPed.Text + " and testa_codigo not in (0,6);")) > 0)
                {
                    Utils.MostrarAlerta(Response, "Este pedido " + ddlNumPed.Text + " tiene otros Créditos en Proceso, Finalicelos !!");
                    return false;
                }
            }
            double valorSolicita;
            int numMeses;
            if (LabelFinanciera.Text.Length == 0){
                Utils.MostrarAlerta(Response, "Debe ingresar la entidad financiera.");
                return false;
            }
            if(ddlPrefPed.Items.Count==0 || ddlNumPed.Items.Count==0){
                Utils.MostrarAlerta(Response, "Debe ingresar el pedido asociado.");
                return false;
            }
            if (ddlOrigen.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el origen.");
                return false;
            }
            try
            {
                valorSolicita = Convert.ToDouble(txtValSolicitado.Text);
                if (valorSolicita < 0) throw (new Exception());
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Valor solicitado inválido.");
                return false;
            }
            try
            {
                numMeses = Convert.ToInt32(txtMeses.Text);
                if (numMeses < 1) throw (new Exception());
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Número de meses inválido.");
                return false;
            }
            return true;
        }
        private bool ValidarDatosAprobacion()
        {
            double valorAprobado;
            if (ddlDesembolso.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el tipo de desembolso.");
                return false;
            }
            try
            {
                valorAprobado = Convert.ToDouble(txtValAprobado.Text);
                if (valorAprobado < 0) throw (new Exception());
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Valor aprobado inválido.");
                return false;
            }
            try
            {
                Convert.ToDateTime(txtFechaAprobacion.Text);
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Fecha inválida.");
                return false;
            }
            if (Convert.ToDateTime(txtFechaAprobacion.Text) < Convert.ToDateTime(txtFechaSolicitud.Text))
            {
                Utils.MostrarAlerta(Response, "La fecha de solicitud no puede ser menor a la fecha de aprobación.");
                return false;
            }
            if (txtNumAprobacion.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el número de aprobación.");
                return false;
            }
      //   if (Convert.ToDouble(txtValSolicitado.Text) != Convert.ToDouble(txtValAprobado.Text))
           if (valorFinanciera != Convert.ToDouble(txtValAprobado.Text))
               {
                Utils.MostrarAlerta(Response, "El valor aprobado debe ser igual al valor solicitado o debe modificar el pedido.");
                return false;
            }
            return true;
        }
        private bool ValidarDatosLegalizacion()
        {
            try
            {
                Convert.ToDateTime(txtFechaLegalizacion.Text);
            }
            catch
            {
                Utils.MostrarAlerta(Response, "Fecha inválida.");
                return false;
            }
            if (Convert.ToDateTime(txtFechaLegalizacion.Text) < Convert.ToDateTime(txtFechaAprobacion.Text))
            {
                Utils.MostrarAlerta(Response, "La fecha de aprobación no puede ser menor a la fecha de legalización.");
                return false;
            }
            if (txtNumCarta.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe ingresar el número de la carta.");
                return false;
            }
            return true;
        }
        #endregion Metodos

        #region Eventos
        protected void Sel_Credito(object sender, EventArgs e)
        {
            proceso = ViewState["PROC"].ToString();
            cargarInformacion(ddlPrefPed.Text, ddlNumPed.Text);
            if(ddlCredito.Items.Count==0 || ddlCredito.SelectedValue.Length==0 || !CargarCredito()){
                Utils.MostrarAlerta(Response, "Crédito inválido.");
                return;
            }
            if (proceso.Equals("D") || proceso.Equals("A") || proceso.Equals("L") || proceso.Equals("S")) plcSolicitud.Visible = true;
            if (proceso.Equals("D") || proceso.Equals("A") || proceso.Equals("L")) plcAprobacion.Visible = true;
            if (proceso.Equals("D") || proceso.Equals("L")) plcDesembolso.Visible = true;
            if (proceso.Equals("L")) plcLegalizacion.Visible = true;
        }

        protected void CambioTipoDesembolso(Object Sender, EventArgs e)
        {
            if(ddlTipoDesembolso.SelectedValue.Equals("B"))
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlBancoCheque, "select PBAN_CODIGO CONCAT '  ' CONCAT PBAN_DESCRIPCION  from DBXSCHEMA.PBANCO  order by PBAN_CODIGO;");
                ddlBancoCheque.Items.Insert(0, "Seleccione..");
                txtCheque.Text = "Tranferencia";
            }
        }


        protected void CambioPrefijoPedido(Object Sender, EventArgs E)
        {
            DatasToControls bind = new DatasToControls();
            string slqPedido = "SELECT MPED_NUMEPEDI FROM MPEDIDOVEHICULO WHERE TEST_TIPOESTA<40 AND PDOC_CODIGO='" + ddlPrefPed.SelectedValue + "' AND MPED_VALOFINC IS NOT NULL AND MPED_VALOFINC > 0 order by MPED_NUMEPEDI; ";
            if (proceso == "R") //  creacion
                slqPedido = "SELECT MPED_NUMEPEDI FROM MPEDIDOVEHICULO WHERE TEST_TIPOESTA < 30 "+
                    " AND PDOC_CODIGO='" + ddlPrefPed.SelectedValue + "' AND MPED_VALOFINC IS NOT NULL AND MPED_VALOFINC > 0 "+
                    " and pdoc_codigo concat cast(mped_numepedi as char(10)) not in (select pdoc_codipedi concat cast(mped_numepedi as char(10)) from MCREDITOFINANCIERA WHERE TESTA_CODIGO IN (1,2,3,5,7))" +
                    " order by MPED_NUMEPEDI; ";

            bind.PutDatasIntoDropDownList(ddlNumPed,slqPedido);
              CambioNumeroPedido(Sender, E);
        }
        protected void CambioNumeroPedido(Object Sender, EventArgs E)
        {
            txtValSolicitado.Text = "";
            if (ddlNumPed.Items.Count == 0) return;
            string valF = DBFunctions.SingleData("SELECT MPED_VALOFINC FROM MPEDIDOVEHICULO WHERE PDOC_CODIGO='" + ddlPrefPed.SelectedValue + "' AND MPED_NUMEPEDI=" + ddlNumPed.SelectedValue + ";");
            cargarInformacion(ddlPrefPed.Text, ddlNumPed.Text);
            if (valF.Length > 0) txtValSolicitado.Text = Convert.ToDouble(valF).ToString("#,##0");
        }
        #region Botones procesos
        public void btnCrear_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            string numero;
            if (!ValidarDatosRegistro())
            {
                Utils.MostrarAlerta(Response, "Error al crear el crédito. ver otros créditos en para este pedido");
                return;
            }
            arlSentencias.Add("INSERT INTO MCREDITOFINANCIERA VALUES (" +
                "default,'" + LabelFinanciera.Text + "','" + ddlPrefPed.SelectedValue + "'," +
                ddlNumPed.SelectedValue + "," + Convert.ToDouble(txtValSolicitado.Text).ToString() + "," +
                txtMeses.Text + "," + ddlOrigen.SelectedValue + ",1," +
                "NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL," + 
                "'"+HttpContext.Current.User.Identity.Name.ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',"+
                "NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL);");
            if (DBFunctions.Transaction(arlSentencias))
            {
                numero = DBFunctions.SingleData("SELECT MAX(MCRED_CODIGO) FROM MCREDITOFINANCIERA");
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=R&crd="+numero+"&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al crear el crédito.");
            }
        }

        public void btnDenegado_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            if (txtDenegado.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe dar la razón de la Negacion.");
                return;
            }
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO<4 AND MCRED_CODIGO=" + lblNumero.Text + ";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para su Negacion.");
                return;
            }
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "TESTA_CODIGO=6," +
                "SUSU_ANULA='" + HttpContext.Current.User.Identity.Name.ToLower() + "', " +
                "MCRED_FECHAANULA='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                "MCRED_RAZOANUL='" + txtDenegado.Text + "' " +
                "WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=M&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al anular el crédito.");
            }
        }
       
        
        public void btnAnular_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            if (txtRazonAnula.Text.Trim().Length == 0)
            {
                Utils.MostrarAlerta(Response, "Debe dar la razón de la anulación.");
                return;
            }
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO<4 AND MCRED_CODIGO=" + lblNumero.Text + ";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para su anulación.");
                return;
            }
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "TESTA_CODIGO=0,"+
                "SUSU_ANULA='" + HttpContext.Current.User.Identity.Name.ToLower() + "', " +
                "MCRED_FECHAANULA='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                "MCRED_RAZOANUL='" + txtRazonAnula.Text + "' "+
                "WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=M&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al anular el crédito.");
            }
        }
        
        public void btnActualizar_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            DataSet dsOriginal=new DataSet();
            string strHistorialCambio = "";
            if (!ValidarDatosRegistro())
            {
                Utils.MostrarAlerta(Response, "El crédito presenta Error, no está en un estado válido para su actualización.");
                return;
            }
         
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO=1 AND MCRED_CODIGO="+lblNumero.Text+";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para su actualización.");
                return;
            }
            //Guardar en MHISTORIALCAMBIOS estado actual credito
            strHistorialCambio = "DEFAULT,'MCREDITOFINANCIERA','U','";
            DBFunctions.Request(dsOriginal, IncludeSchema.NO, "SELECT * FROM MCREDITOFINANCIERA WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            for (int n = 0; n < dsOriginal.Tables[0].Columns.Count; n++)
                strHistorialCambio += dsOriginal.Tables[0].Rows[0][n].ToString() + ",";
            strHistorialCambio = strHistorialCambio.Substring(0, strHistorialCambio.Length - 1) + "',"+
                "'" + HttpContext.Current.User.Identity.Name.ToLower() + "',"+
                "'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"'";
            arlSentencias.Add("INSERT INTO MHISTORIAL_CAMBIOS VALUES(" + strHistorialCambio + ");");
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "MNIT_FINANCIERA='"+LabelFinanciera.Text+"', "+
                "PDOC_CODIPEDI='" + ddlPrefPed.SelectedValue + "', " +
                "MPED_NUMEPEDI=" + ddlNumPed.SelectedValue + ", " +
                "MCRED_VALOSOLI=" + Convert.ToDouble(txtValSolicitado.Text).ToString()+","+
                "MCRED_VALOAPROB=" + Convert.ToDouble(txtValSolicitado.Text).ToString() + "," +
                "MCRED_MESES=" + txtMeses.Text + "," + 
                "TORIG_CODIGO="+ddlOrigen.SelectedValue+","+
                "SUSU_CREA='"+HttpContext.Current.User.Identity.Name.ToLower() + "', "+
                "MCRED_FECHACREA='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"' "+
                "WHERE MCRED_CODIGO="+lblNumero.Text+";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=M&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al modificar el crédito.");
            }
        }
        public void btnSolicitar_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            if (!ValidarDatosSolicitud())
                return;
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO=1 AND MCRED_CODIGO=" + lblNumero.Text + ";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para su solicitud.");
                return;
            }
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "TESTA_CODIGO=2," +
                "MCRED_FECHASOLI='" + txtFechaSolicitud.Text + "'," +
                "MCRED_OBSESOLI='" + txtObsSolicitud.Text + "'," +
                "SUSU_SOLICITA='" + HttpContext.Current.User.Identity.Name.ToLower() + "', " +
                "MCRED_FECHASOLICITA='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=S&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al solicitar el crédito.");
            }
        }
        public void btnAprobar_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            cargarInformacion(ddlPrefPed.SelectedValue, ddlNumPed.SelectedValue);
            if (!ValidarDatosAprobacion())
                return;
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO=2 AND MCRED_CODIGO=" + lblNumero.Text + ";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para su aprobación.");
                return;
            }
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "TESTA_CODIGO=3," +
                "MCRED_VALOAPROB=" + Convert.ToDouble(txtValAprobado.Text).ToString() + "," +
                "MCRED_FECHAPROB='" + txtFechaAprobacion.Text + "'," +
                "TDES_CODIGO=" + ddlDesembolso.SelectedValue + "," +
                "MCRED_NUMEAPROB='" + txtNumAprobacion.Text.Trim() + "'," +
                "SUSU_APRUEBA='" + HttpContext.Current.User.Identity.Name.ToLower() + "', " +
                "MCRED_FECHAAPRUEBA='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=A&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al aprobar el crédito.");
            }
        }
        public void btnDesembolso_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            String infoCheque;
            valorFinanciera = Convert.ToDouble(DBFunctions.SingleData("select coalesce(MPED_valofinc,0) from dbxschema.mpedidovehiculo where pdoc_codigo='" +
                ddlPrefPed.SelectedValue + "' and mped_numepedi=" + ddlNumPed.SelectedValue + ";"));

            if (!ValidarDatosDesembolso())
                return;
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO=3 AND MCRED_CODIGO=" + lblNumero.Text + ";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para realizar el desembolso.");
                return;
            }
            infoCheque = txtCheque.Text + "  " + ddlBancoCheque.SelectedValue;
            if (ddlTipoDesembolso.SelectedValue.Equals("Tf Bancaria")) 
                infoCheque+=" Transferencia";
            
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "TESTA_CODIGO=4," +
                "MCRED_FECHADESEM='" + txtFechaDesembolso.Text + "'," +
                "MCRED_CHEQDESEM='" + infoCheque + "'," +
                "MCRED_VALODESEM=" + Convert.ToDouble(txtValorDesembolso.Text).ToString() + "," +
                "SUSU_DESEMBOLSO='" + HttpContext.Current.User.Identity.Name.ToLower() + "', " +
                "MCRED_FECHADESEMBOLSO='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=D&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al asignar el desembolso el crédito.");
            }
        }
        public void btnLegalizar_Click(object sender, EventArgs e)
        {
            ArrayList arlSentencias = new ArrayList();
            cargarInformacion(ddlPrefPed.SelectedValue,ddlNumPed.SelectedValue);
            
            if (!ValidarDatosLegalizacion())
                return;
            if (!DBFunctions.RecordExist("SELECT MCRED_CODIGO FROM MCREDITOFINANCIERA WHERE TESTA_CODIGO=4 AND TDES_CODIGO=1 AND MCRED_CODIGO=" + lblNumero.Text + ";"))
            {
                Utils.MostrarAlerta(Response, "El crédito no está en un estado válido para su legalización o no es un aval.");
                return;
            }
            arlSentencias.Add("UPDATE MCREDITOFINANCIERA SET " +
                "TESTA_CODIGO=5," +
                "MCRED_VALOAPROB=" + Convert.ToDouble(txtValAprobado.Text).ToString() + "," +
                "MCRED_FECHAPROB='" + txtFechaAprobacion.Text + "'," +
                "TDES_CODIGO=" + ddlDesembolso.SelectedValue + "," +
                "MCRED_NUMEAPROB='" + txtNumAprobacion.Text.Trim() + "'," +
                "SUSU_APRUEBA='" + HttpContext.Current.User.Identity.Name.ToLower() + "', " +
                "MCRED_FECHAAPRUEBA='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' " +
                "WHERE MCRED_CODIGO=" + lblNumero.Text + ";");
            if (DBFunctions.Transaction(arlSentencias))
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.Creditos&prc=A&crd=" + lblNumero.Text + "&path=" + Request.QueryString["path"]);
            }
            else
            {
                lblError.Text = DBFunctions.exceptions;
                Utils.MostrarAlerta(Response, "Error al legalizar el crédito.");
            }
        }
        #endregion
        #endregion
    }
}