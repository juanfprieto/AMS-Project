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
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class DatosCliente : System.Web.UI.UserControl
	{
		protected Control padre;
		protected Control encabezado,documentos,varios,pagos,noCausados;
				
		protected void Page_Load(object Sender,EventArgs e)
		{
			padre=(this.Parent).Parent;
			encabezado=((PlaceHolder)padre.FindControl("phEncabezado")).Controls[0];
			documentos=((PlaceHolder)padre.FindControl("phDocumentos")).Controls[0];
			noCausados=((PlaceHolder)padre.FindControl("phNoCausados")).Controls[0];
			varios=((PlaceHolder)padre.FindControl("phVarios")).Controls[0];
			pagos=((PlaceHolder)padre.FindControl("phPagos")).Controls[0];
		}
		
		protected void Esconder_Controles()
		{
			this.datCli.Enabled=false;
			this.datBen.Enabled=false;
			((PlaceHolder)padre.FindControl("phDatosCliente")).Visible=false;
			((ImageButton)padre.FindControl("btnCliente")).ImageUrl="../img/AMS.BotonExpandir.png";
		}
		
		protected void aceptar_Click(object Sender,EventArgs e)
		{
			if(datCli.Text==string.Empty || datBen.Text==string.Empty)
                Utils.MostrarAlerta(Response, "Falta un nit por llenar");
			else if(!DBFunctions.RecordExist("SELECT MNIT_NIT FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+datCli.Text+"'") || !DBFunctions.RecordExist("SELECT MNIT_NIT FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+datBen.Text+"'"))
                    Utils.MostrarAlerta(Response, "Algun nit es inexistente");
			else
			{
				datBen.Text=datCli.Text;
				datBena.Text=datClia.Text;
				datBenb.Text=datClib.Text;
				datBenc.Text=datClic.Text;
				datBend.Text=datClid.Text;
				//Recibos de Caja
				if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="A")
				{
					//Anticipo a Taller
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
					((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
					this.Esconder_Controles();
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="I"  || ((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="E")
				{
					//Ingreso Definitivo
					if(Request.QueryString["tipo"]=="RC" || Request.QueryString["tipo"]=="CE")
					{
						bool existeMFC=DBFunctions.RecordExist("SELECT mnit_nit FROM mfacturacliente   WHERE mnit_nit IN ('"+this.datBen.Text+"','"+this.datCli.Text+"') AND MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON <> 0");
						bool existeMFP=DBFunctions.RecordExist("SELECT mnit_nit FROM mfacturaproveedor WHERE mnit_nit IN ('"+this.datBen.Text+"','"+this.datCli.Text+"') AND MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON <> 0");
						if(existeMFC || existeMFP)
						{
                            Utils.MostrarAlerta(Response, "Ahora puede cargar los documentos de este nit : " + this.datBen.Text + ". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
							((Button)documentos.FindControl("cargarDocs")).Enabled=true;
							((PlaceHolder)padre.FindControl("phDocumentos")).Visible=true;
							((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonContraer.png";
							((ImageButton)padre.FindControl("btnDocumentos")).Enabled=true;
							this.Esconder_Controles();
						}
						else
						{
                            Utils.MostrarAlerta(Response, "No existen  documentos para este nit : " + this.datBen.Text + "");
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
						((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
						((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
						((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
						this.Esconder_Controles();
					}
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="P")
				{
					//Legalización de un provisional
					bool existeMCP=DBFunctions.RecordExist("SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor/mcpag_valortasacambio,mcpag_valortasacambio,mcpag_valorbase,mcpag_fecha,test_estado FROM mcajapago P,mcaja M,pdocumento D WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND M.pdoc_codigo=D.pdoc_codigo AND M.mnit_nitben='"+this.datBen.Text+"' AND P.ttip_codigo='C' AND P.test_estado='C' AND D.tdoc_tipodocu='RP'");
					if(existeMCP)
					{
                        Utils.MostrarAlerta(Response, "Ahora puede cargar los documentos de este nit : " + this.datBen.Text + ". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
						((Button)documentos.FindControl("cargarDocs")).Enabled=true;
						((PlaceHolder)padre.FindControl("phDocumentos")).Visible=true;
						((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnDocumentos")).Enabled=true;
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No existen  recibos provisionales para este nit : " + this.datBen.Text + ". Elija otro");
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="R")
				{
					//Reconsignación Cheques Devueltos, solo documentos
					bool existeMCP=DBFunctions.RecordExist("SELECT ttip_codigo,pban_codigo,mcpag_numerodoc,mcpag_tipomoneda,mcpag_valor,mcpag_valortasacambio,mcpag_valorbase,mcpag_fecha FROM mcajapago P,mcaja M WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mnit_nitben='"+this.datBen.Text+"' AND M.mcaj_numero=P.mcaj_numero AND ttip_codigo='C' AND test_estado='D'");
					if(existeMCP)
					{
                        Utils.MostrarAlerta(Response, "Ahora puede cargar los documentos de este nit : " + this.datBen.Text + ". Si no desea hacer abonos a documentos. Solo de click en el botón Aceptar");
						((Button)documentos.FindControl("cargarDocs")).Enabled=true;
						((PlaceHolder)padre.FindControl("phDocumentos")).Visible=true;
						((ImageButton)padre.FindControl("btnDocumentos")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnDocumentos")).Enabled=true;
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No existen cheques devueltos para el nit " + this.datBen.Text + "");
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="V")
				{
					//Anticipo a Vehiculos
					((PlaceHolder)padre.FindControl("phVarios")).Visible=true;
					((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnVarios")).Enabled=true;
					((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
					this.Esconder_Controles();
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="C")
				{
					//Actualizacion Prorrogas Cheques
					if(DBFunctions.RecordExist("SELECT P.ttip_codigo FROM mcajapago P,mcaja M WHERE M.pdoc_codigo=P.pdoc_codigo AND M.mcaj_numero=P.mcaj_numero AND M.mnit_nitben='"+this.datBen.Text+"' AND P.ttip_codigo='C'"))
					{
						((PlaceHolder)padre.FindControl("phVarios")).Visible=true;
						((Panel)varios.FindControl("panelAbonos")).Visible=false;
						((Panel)varios.FindControl("panelPost")).Visible=true;
						((Label)padre.FindControl("lbVarios")).Text="Actualización de Prorrogas de Cheques";
						((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnVarios")).Enabled=true;
						((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No hay cheques de este nit: " + this.datBen.Text + ". Escoja otro.");
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="D")
				{
                    if (DBFunctions.RecordExist("SELECT M.pdoc_codigo,M.mped_numepedi,M.mped_valounit - m.mped_valodesc, SUM(V.mant_valorecicaja) FROM mpedidovehiculo M,manticipovehiculo V WHERE M.pdoc_codigo=V.mped_codigo AND M.mped_numepedi=V.mped_numepedi AND M.mnit_nit='" + this.datBen.Text + "' GROUP BY M.pdoc_codigo,M.mped_numepedi,M.mped_valounit, m.mped_valodesc"))
					{
                        ((PlaceHolder)padre.FindControl("phVarios")).Visible=true;
						((Panel)varios.FindControl("panelDevPed")).Visible=true;
						((Label)padre.FindControl("lbVarios")).Text="Devolución de Pedidos de Clientes";
						((ImageButton)padre.FindControl("btnVarios")).ImageUrl="../img/AMS.BotonContraer.png";
						((ImageButton)padre.FindControl("btnVarios")).Enabled=true;
						((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
						this.Esconder_Controles();
					}
					else
                        Utils.MostrarAlerta(Response, "No hay abonos a pedidos de ese nit");
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="G")
				{
					//Anticipo General
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
					((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
					this.Esconder_Controles();
				}
				else if(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue=="S")
				{
					//Prestamo a Cliente
					((PlaceHolder)padre.FindControl("phPagos")).Visible=true;
					((ImageButton)padre.FindControl("btnPagos")).ImageUrl="../img/AMS.BotonContraer.png";
					((ImageButton)padre.FindControl("btnPagos")).Enabled=true;
					((DataGrid)pagos.FindControl("gridRtns")).Visible=false;
					((Panel)pagos.FindControl("panelPrefijo")).Visible=true;
					this.Esconder_Controles();
				}
			}
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
