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
using AMS.DB;
using AMS.Forms;
using Ajax;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class EncabezadoRecibo : System.Web.UI.UserControl
	{
		protected Control padre;
		protected Control datosCliente,documentos,varios,noCausados;
		protected AMS.Finanzas.Tesoreria.Documentos controlDocumentos;
		protected AMS.Finanzas.Tesoreria.ManejoVarios controlVarios;
		protected AMS.Finanzas.Tesoreria.NoCausados controlNoCausados;
        protected AMS.Finanzas.Tesoreria.Pagos controlPagos;
        protected HtmlGenericControl lblFlujo = new HtmlGenericControl();
        protected HtmlGenericControl divTipo = new HtmlGenericControl();
		protected string MainPage=ConfigurationManager.AppSettings["MainIndexPage"];
		public static DataSet ds=new DataSet();
        public string verCredito = "";
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(EncabezadoRecibo));

            padre = (this.Parent).Parent;

            documentos = ((PlaceHolder)padre.FindControl("phDocumentos")).Controls[0];
            controlDocumentos = (AMS.Finanzas.Tesoreria.Documentos)((PlaceHolder)padre.FindControl("phDocumentos")).Controls[0];
            controlVarios = (AMS.Finanzas.Tesoreria.ManejoVarios)((PlaceHolder)padre.FindControl("phVarios")).Controls[0];
            controlNoCausados = (AMS.Finanzas.Tesoreria.NoCausados)((PlaceHolder)padre.FindControl("phNoCausados")).Controls[0];
            noCausados = ((PlaceHolder)padre.FindControl("phNoCausados")).Controls[0];
            varios = ((PlaceHolder)padre.FindControl("phVarios")).Controls[0];
            controlPagos = (AMS.Finanzas.Tesoreria.Pagos)((PlaceHolder)padre.FindControl("phPagos")).Controls[0];
            if (Request.QueryString["tipo"] == "RC" && tipoRecibo.SelectedValue.Equals("F")) verCredito = "visible";
            else verCredito = "hidden";

			if(!IsPostBack)
			{
                fecha.Text=DateTime.Now.Date.ToString("yyyy-MM-dd");
                string sql;
        		if(Request.QueryString["tipo"]=="RC")
				{
					lbCli.Text="Cliente: ";
					lbBen.Text="Por Cuenta de: ";

                    sql = "SELECT tcla_claserecaja,tcla_nombre FROM tclaserecaja WHERE tcla_claserecaja NOT IN('E','D','G','S','O') ORDER BY tcla_nombre";
                    Utils.FillDll(tipoRecibo, sql, false);
                    DatasToControls.EstablecerValueDropDownList(tipoRecibo,"I");

                    sql = @"SELECT MCRED_CODIGO,  
                                CHAR(MCRED_CODIGO) CONCAT ': ' CONCAT CASE  
                                  WHEN MNF.TNIT_TIPONIT =  'N' THEN MNF.mnit_apellidos  
                                  ELSE MNF.mnit_apellidos CONCAT  ' ' CONCAT COALESCE(MNF.mnit_apellido2,'') CONCAT ' ' CONCAT MNF.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNF.mnit_NOMBRE2,'')  
                                END CONCAT ' - ' CONCAT CASE  
                                  WHEN MNC.TNIT_TIPONIT = 'N' THEN MNC.mnit_apellidos  
                                  ELSE MNC.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNC.mnit_apellido2,'') CONCAT ' ' CONCAT MNC.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNC.mnit_NOMBRE2,'')  
                                END CONCAT ' ($' CONCAT CHAR(MC.MCRED_VALOAPROB) CONCAT ')'  
                          FROM MCREDITOFINANCIERA MC,  
                               MNIT MNF,  
                               MNIT MNC,   
                               MPEDIDOVEHICULO MP  
                         WHERE MC.MNIT_FINANCIERA = MNF.MNIT_NIT  
                         AND   MP.MNIT_NIT = MNC.MNIT_NIT  
                         AND   MP.PDOC_CODIGO = MC.PDOC_CODIPEDI  
                         AND   MP.MPED_NUMEPEDI = MC.MPED_NUMEPEDI  
                         AND   (MC.TESTA_CODIGO = 2 OR MC.TESTA_CODIGO = 3)  
                         AND   (MC.PDOC_CODICAJA IS NULL AND MC.MCAJ_NUMEDOCU IS NULL)";
                    Utils.FillDll(ddlCredito, sql, false);
				}
				else if(Request.QueryString["tipo"]=="CE")
				{
					lbCli.Text="Proveedor: ";
					lbBen.Text="Por Cuenta de: ";
                    sql = "SELECT tcla_claserecaja,tcla_nombre FROM tclaserecaja WHERE tcla_claserecaja IN('E','D','G','S','O') ORDER BY tcla_nombre";
                    Utils.FillDll(tipoRecibo, sql, false);
					DatasToControls.EstablecerValueDropDownList(tipoRecibo,"E");
				}
				else if(Request.QueryString["tipo"]=="RP")
				{
					lbCli.Text="Cliente: ";
					lbBen.Text="Por Cuenta de: ";
                    sql = "SELECT tcla_claserecaja,tcla_nombre FROM tclaserecaja WHERE tcla_claserecaja IN('I','V','A') ORDER BY tcla_nombre";
                    Utils.FillDll(tipoRecibo, sql, false);
					DatasToControls.EstablecerValueDropDownList(tipoRecibo,"I");
				}
				if(Request.QueryString["cnd"]!=null && Request.QueryString["cnd"]=="1")
					fecha.ReadOnly=false;
                if (Request.QueryString["cruce"] != null)
                {
                    fecha.ReadOnly = false;
                }
                sql = "SELECT palm_almacen,palm_descripcion FROM palmacen where (pcen_centcart is not null or pcen_centteso is not null) and tvig_vigencia='V' order by palm_descripcion;";
                Utils.FillDll(almacen, sql, true);
                if (almacen.Items.Count == 1)
                    Cambiar_Almacen(Sender, e);

                sql = "SELECT pflu_codgen CONCAT '-' CONCAT pflu_codigo,pflu_descripcion FROM dbxschema.pflujocajaespecifico";
                Utils.FillDll(ddlFlujo, sql, true);
            }
		}

        protected void Cambiar_Almacen(object Sender, EventArgs e)
        {
            String tipoProceso = Request.QueryString["tipo"]; // proceso Recibo de Caja, Comprobante de Egreso, Recibo Provisional de Caja
            String tipoDocmto = tipoProceso;  // se iguala el tipo de documento al tipo de proceso, porque RC y CE son iguales
            if (tipoDocmto == "RCP")          // menos el procespo RCP que es RP en el tipo de documento
                tipoDocmto = "RP";            // se pone el tipo de RP al recibo de caja Provisional
            string almacenStr = almacen.SelectedValue;

            if (Request.QueryString["cruce"] != null)
            {
                tipoProceso = "NK";
            }

            bool llenoPrefijos = Utils.llenarPrefijos(Response, ref prefijoRecibo, tipoProceso, almacenStr, tipoDocmto);

            DropDownList ddlPrefijoFactura = (DropDownList)controlPagos.FindControl("ddlPrefijoFactura");
            DropDownList ddlPrefNot = (DropDownList)controlPagos.FindControl("ddlPrefNot");
            DropDownList ddlPrefNotPro = (DropDownList)controlPagos.FindControl("ddlPrefNotPro");

            Utils.llenarPrefijos(Response, ref ddlPrefijoFactura, "NC", almacenStr, "FC");
            Utils.llenarPrefijos(Response, ref ddlPrefNot, "NC", almacenStr, "NC");
            Utils.llenarPrefijos(Response, ref ddlPrefNotPro, "NP", almacenStr, "NP");

            if (llenoPrefijos)
                numeroRecibo.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoRecibo.SelectedValue + "'");
	    }

		
		protected void prefijoRecibo_IndexChanged(object Sender,EventArgs e)
		{
			numeroRecibo.Text=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoRecibo.SelectedValue+"'");
            if (Request.QueryString["tipo"] == "RC" && tipoRecibo.SelectedValue.Equals("F")) 
                verCredito = "visible";
            else verCredito = "hidden";
		}

       
        [Ajax.AjaxMethod]
        public DataSet TraerCredito(string numCred)
        {
            DataSet dsCredito = new DataSet();
            DBFunctions.Request(dsCredito,IncludeSchema.NO,
                "SELECT MNF.MNIT_NIT as NitFinanciera, " +
                "CASE WHEN MNF.TNIT_TIPONIT = \'N\' THEN MNF.mnit_apellidos ELSE MNF.mnit_apellidos CONCAT \' \' CONCAT COALESCE(MNF.mnit_apellido2,\'\') CONCAT \' \' CONCAT MNF.mnit_nombres CONCAT \' \' CONCAT COALESCE(MNF.mnit_NOMBRE2,\'\') END AS NombreFinanciera, "+
                "MNC.MNIT_NIT as NitCliente, " +
                "CASE WHEN MNC.TNIT_TIPONIT = \'N\' THEN MNC.mnit_apellidos ELSE MNC.mnit_apellidos CONCAT \' \' CONCAT COALESCE(MNC.mnit_apellido2,\'\') CONCAT \' \' CONCAT MNC.mnit_nombres CONCAT \' \' CONCAT COALESCE(MNC.mnit_NOMBRE2,\'\') END AS NombreCliente " +
                "FROM MCREDITOFINANCIERA MC, MNIT MNF, MNIT MNC, MPEDIDOVEHICULO MP "+
                "WHERE MC.MNIT_FINANCIERA=MNF.MNIT_NIT AND MP.MNIT_NIT=MNC.MNIT_NIT AND " +
                "MP.PDOC_CODIGO=MC.PDOC_CODIPEDI AND MP.MPED_NUMEPEDI=MC.MPED_NUMEPEDI AND " +
                "MC.MCRED_CODIGO=" + numCred + ";");
            return (dsCredito);
        }
        [Ajax.AjaxMethod]
		public string Cambiar_Numero(string prefijo)
		{
			string numero="";
			numero=DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijo+"'");
			return numero;
		}
		
		protected void Esconder_Controles()
		{
			((PlaceHolder)padre.FindControl("phEncabezado")).Visible=false;
			((ImageButton)padre.FindControl("btnEncabezado")).ImageUrl="../img/AMS.BotonExpandir.png";
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
            //string valida = Tools.General.validarCierreFinanzas(fecha.Text, "C");
            if (!Tools.General.validarCierreFinanzas(fecha.Text, "C"))
            {
                Utils.MostrarAlerta(Response, "La fecha del documento no corresponde a la vigencia del sistema de cartera. Por favor revise.");
                return;
            }

            Session.Remove("TIPO_COMPROBANTE");
			Session["TIPO_COMPROBANTE"]=tipoRecibo.SelectedValue;
            if (Request.QueryString["tipo"] == "RC" && tipoRecibo.SelectedValue.Equals("F")){
                verCredito = "visible";
                if (ddlCredito.Items.Count == 0 || ddlCredito.SelectedValue.Length == 0){
                    Utils.MostrarAlerta(Response, "Debe seleccionar el crédito");
                    return;
                }
            }
            

            else verCredito = "hidden";
			if(concepto.Text.Length<10)
                Utils.MostrarAlerta(Response, "El concepto es demasiado corto. Mínimo 10 caracteres");
			else if(!DatasToControls.ValidarInt(numeroRecibo.Text))
                    Utils.MostrarAlerta(Response, "Número Invalido");
            else if (almacen.SelectedValue == "Seleccione...")
                    Utils.MostrarAlerta(Response, "Almacén NO Seleccionado !!!");
            else if (ddlFlujo.SelectedValue == "0")
                    Utils.MostrarAlerta(Response, "Flujo de Caja NO Seleccionado !!!");
            else if (datCli.Text == "")
                    Utils.MostrarAlerta(Response, "Nit NO Seleccionado !!!");
            else if (datBen.Text == "")
                    Utils.MostrarAlerta(Response, "Nit por Cuenta de NO Seleccionado !!!");
            else
			{
				prefijoRecibo.Enabled=false;
				tipoRecibo.Enabled=false;
                fecha.Enabled = false;
                if (Request.QueryString["cruce"] != null)
                {
                    fecha.Enabled = true;
                }
				almacen.Enabled=false;
				numeroRecibo.ReadOnly=true;
				concepto.ReadOnly=true;
				aceptar.Enabled=false;
				datCli.Enabled=false;
				datBen.Enabled=false;
                ddlCredito.Enabled = false;
				((HtmlInputHidden)documentos.FindControl("hdnCli")).Value=datCli.Text;
				((HtmlInputHidden)documentos.FindControl("hdnBen")).Value=datBen.Text;
				((HtmlInputHidden)varios.FindControl("hdncli")).Value=datCli.Text;
				((HtmlInputHidden)varios.FindControl("hdnben")).Value=datBen.Text;
				controlNoCausados.Cargar_tablaNC();
				controlNoCausados.Mostrar_gridNC();
				//Recibos de Caja
				if(tipoRecibo.SelectedValue=="A")
				{
					//Anticipo a Taller
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
                    ((DataGrid)controlPagos.FindControl("gridRtns")).Visible = false;
                    ((Panel)controlPagos.FindControl("pnlPrefNot")).Visible = true;
					this.Esconder_Controles();
				}
                else if (tipoRecibo.SelectedValue == "F" || tipoRecibo.SelectedValue == "I" || tipoRecibo.SelectedValue == "E" || tipoRecibo.SelectedValue == "O")
				{
					//Ingreso Definitivo
					if(Request.QueryString["tipo"]=="RC" || Request.QueryString["tipo"]=="CE")
					{
						bool existeMFC=DBFunctions.RecordExist("SELECT mnit_nit FROM mfacturacliente WHERE mnit_nit IN ('"+this.datBen.Text+"','"+this.datCli.Text+"') AND tvig_vigencia<>'C'");
						bool existeMFP=DBFunctions.RecordExist("SELECT mnit_nit FROM mfacturaproveedor WHERE mnit_nit IN ('"+this.datBen.Text+"','"+this.datCli.Text+"') AND tvig_vigencia<>'C'");
						if(tipoRecibo.SelectedValue!="O" && (existeMFC || existeMFP))
						{
							if(datCli.Text!=datBen.Text)
                                Utils.MostrarAlerta(Response, "Se cargaran los documentos para estos nits: "+this.datCli.Text+" y "+this.datBen.Text+". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
							else
                                Utils.MostrarAlerta(Response, "Se cargaran los documentos para este nit: " + this.datCli.Text + ". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
							controlDocumentos.CargarGrillaDocumentos();
							((PlaceHolder)padre.FindControl("phDocumentos")).Visible=true;
							((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonContraer.png";
							((ImageButton)padre.FindControl("btnDocumentos")).Enabled=true;
							this.Esconder_Controles();
						}
						else if(tipoRecibo.SelectedValue=="O" || (!existeMFC && !existeMFP)) 
						{
							if(tipoRecibo.SelectedValue!="O")
                                Utils.MostrarAlerta(Response, "No existen documentos para el/los nit(s) especificado(s)");
							this.Esconder_Controles();
							((PlaceHolder)padre.FindControl("phNoCausados")).Visible=true;
							if(Session["TIPO_COMPROBANTE"].ToString().Equals("O"))
								((PlaceHolder)padre.FindControl("phCancelacionObligFin")).Visible=true;
							((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl="../img/AMS.BotonContraer.png";
							((ImageButton)padre.FindControl("btnNoCausados")).Enabled=true;
						}
					}
					else if(Request.QueryString["tipo"]=="RP")
					{
                        ((PlaceHolder)padre.FindControl("phNoCausados")).Visible = true;
                        ((ImageButton)padre.FindControl("btnNoCausados")).ImageUrl = "../img/AMS.BotonContraer.png";
                        ((ImageButton)padre.FindControl("btnNoCausados")).Enabled = true;

                        //((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
                        //((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
                        //((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
                        ((DataGrid)controlPagos.FindControl("gridRtns")).Visible = false;
						this.Esconder_Controles();
					}
				}
				else if(tipoRecibo.SelectedValue=="P")
				{
					//Legalización de un provisional
					bool existeMCP=DBFunctions.RecordExist("SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor/mcpag_valortasacambio,mcpag_valortasacambio,mcpag_valorbase,mcpag_fecha,test_estado FROM mcajapago P,mcaja M,pdocumento D WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND M.pdoc_codigo=D.pdoc_codigo AND (M.mnit_nit IN ('"+this.datCli.Text+"','"+this.datBen.Text+"') OR M.mnit_nitben IN ('"+this.datCli.Text+"','"+this.datBen.Text+"')) AND P.ttip_codigo='C' AND P.test_estado='C' AND D.tdoc_tipodocu='RP'");
					if(existeMCP)
					{
						if(datCli.Text!=datBen.Text)
                            Utils.MostrarAlerta(Response, "Se cargaran los documentos para estos nits: "+this.datCli.Text+" y "+this.datBen.Text+". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
						else
                            Utils.MostrarAlerta(Response, "Se cargaran los documentos para este nit: " + this.datCli.Text + ". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
						controlDocumentos.CargarGrillaDocumentos();
						((PlaceHolder)padre.FindControl("phDocumentos")).Visible=true;
						((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnDocumentos")).Enabled=true;
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No existen recibos provisionales para el/los nit(s) especificado(s)");
				}
				else if(tipoRecibo.SelectedValue=="R")
				{
					//Reconsignación Cheques Devueltos, solo documentos
					bool existeMCP=DBFunctions.RecordExist("SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_valorbase,mcpag_fecha FROM mcajapago P,mcaja M WHERE M.pdoc_codigo=P.pdoc_codigo AND (M.mnit_nit IN ('"+this.datCli.Text+"','"+this.datBen.Text+"') OR M.mnit_nitben IN ('"+this.datCli.Text+"','"+this.datBen.Text+"')) AND M.mcaj_numero=P.mcaj_numero AND ttip_codigo='C' AND test_estado='D'");
					if(existeMCP)
					{
						if(datCli.Text!=datBen.Text)
                            Utils.MostrarAlerta(Response, "Se cargaran los documentos para estos nits: "+this.datCli.Text+" y "+this.datBen.Text+". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
						else
                            Utils.MostrarAlerta(Response, "Se cargaran los documentos para este nit: " + this.datCli.Text + ". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
						((PlaceHolder)padre.FindControl("phDocumentos")).Visible=true;
						((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnDocumentos")).Enabled=true;
						controlDocumentos.CargarGrillaDocumentos();
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No existen cheques devueltos para el/los nit(s) especificado(s)");
				}
				else if(tipoRecibo.SelectedValue=="V")
				{
					 //Anticipo a Vehiculos
					 this.Esconder_Controles();
					 ((PlaceHolder)padre.FindControl("phVarios")).Visible=true;
					 ((Label)padre.FindControl("lbVarios")).Text="Anticipos a Vehiculos";
					 ((Panel)varios.FindControl("panelAbonos")).Visible=true;
					 ((Panel)varios.FindControl("panelPost")).Visible=false;
					 ((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonContraer.png";
					 ((ImageButton)padre.FindControl("btnVarios")).Enabled=true;
					 controlVarios.Llenar_GridAnticipos();
				}
				else if(tipoRecibo.SelectedValue=="C")
				{
					//Actualizacion Prorrogas Cheques
					if(DBFunctions.RecordExist("SELECT P.ttip_codigo FROM mcajapago P,mcaja M WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND (M.mnit_nit IN ('"+this.datCli.Text+"','"+this.datBen.Text+"') OR M.mnit_nitben IN ('"+this.datCli.Text+"','"+this.datBen.Text+"')) AND P.ttip_codigo='C'"))
					{
						((PlaceHolder)padre.FindControl("phVarios")).Visible=true;
						((Panel)varios.FindControl("panelAbonos")).Visible=false;
						((Panel)varios.FindControl("panelPost")).Visible=true;
						((Label)padre.FindControl("lbVarios")).Text="Actualización de Prorrogas de Cheques";
						((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnVarios")).Enabled=true;
                        ((DataGrid)controlPagos.FindControl("gridRtns")).Visible = false;
						controlVarios.Llenar_GrillaProrrogas();
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No hay cheques para el/los nit(s) especificado(s)");
				}
				else if(tipoRecibo.SelectedValue=="D")
				{
					//Devolución Pedido Cliente
                    if (DBFunctions.RecordExist("SELECT M.pdoc_codigo,M.mped_numepedi,SUM(M.mped_valounit - m.mped_valodesc), SUM(V.mant_valorecicaja) FROM mpedidovehiculo M,manticipovehiculo V WHERE M.pdoc_codigo=V.mped_codigo AND M.mped_numepedi=V.mped_numepedi AND M.mnit_nit IN ('" + this.datCli.Text + "','" + this.datBen.Text + "') AND TEST_TIPOESTA = 10 GROUP BY M.pdoc_codigo,M.mped_numepedi "))
					{
                        ((PlaceHolder)padre.FindControl("phVarios")).Visible=true;
						((Panel)varios.FindControl("panelDevPed")).Visible=true;
						((Label)padre.FindControl("lbVarios")).Text="Devolución de Pedidos de Clientes";
						controlVarios.Cargar_GrillaPedidosDevueltos();
						((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnVarios")).Enabled=true;
                        ((DataGrid)controlPagos.FindControl("gridRtns")).Visible = false;
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No hay abonos a pedidos para el/los nit(s) especificado(s) o ESTAN ASIGNADOS ó YA ESTAN FACTURADOS ");
				}
				else if(tipoRecibo.SelectedValue=="G")
				{
					//Anticipo General
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
                    ((DataGrid)controlPagos.FindControl("gridRtns")).Visible = false;
                    ((Panel)controlPagos.FindControl("pnlPrefNotPro")).Visible = true;
					this.Esconder_Controles();
				}
				else if(tipoRecibo.SelectedValue=="S")
				{
					//Préstamo a Cliente
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
                    ((DataGrid)controlPagos.FindControl("gridRtns")).Visible = false;
                    ((Panel)controlPagos.FindControl("panelPrefijo")).Visible = true;
					this.Esconder_Controles();
				}
			}
		}

		[Ajax.AjaxMethod]
		public string Retornar_Nombre(string valor)
		{
			string nombre="";
            nombre=DBFunctions.SingleData("SELECT nombre FROM vmnit WHERE mnit_nit='"+valor+"'");
			if(nombre=="")
				nombre+="Error";
			return nombre;
		}

		[Ajax.AjaxMethod]
		public string Retornar_Nombre2(string valor)
		{
			string nombre="";
			nombre=DBFunctions.SingleData("SELECT nombre FROM vmnit WHERE mnit_nit='"+valor+"'");
			if(nombre=="")
				nombre+="Error";
			return nombre;
		}
		
		////////////////////////////////////////////////
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
