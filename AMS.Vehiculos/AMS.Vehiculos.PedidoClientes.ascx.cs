// created on 25/01/2005 at 12:42
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using AMS.CriptoServiceProvider;
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class PedidoClientes : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		private FormatosDocumentos formatoRecibo=new FormatosDocumentos();
		string ConString;
		string path=System.Configuration.ConfigurationManager.AppSettings["PathToPreviews"];

        
        protected void Page_Load(object sender, System.EventArgs e)
		{
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Vehiculos.PedidoClientes));
            if (Request.QueryString["cont"] == "1")
            {
                txtContrasena.Visible = false;
                lContrasena.Visible = false;
                
            }
            if (Request.QueryString["modificado"] == "1")
            {
                Utils.MostrarAlerta(Response, "Pedido Modificado Exitosamente");
            }

			if(!IsPostBack)
			{

                DatasToControls bind = new DatasToControls();
                string modificaPedido = "";
                try
                {
                    modificaPedido = DBFunctions.SingleData("select cveh_modifica from DBXSCHEMA.CVEHICULOS;");
                }
                catch (Exception err)
                { }

                if (modificaPedido == "N")
                {
                    plcVendedor.Visible = true;
                }
                else 
                {
                    Cambio_Prefijo(null, null);
                    bind.PutDatasIntoDropDownList(prefijoDocumento, string.Format(Documento.DOCUMENTOSTIPO, "PC"));
                    if (prefijoDocumento.Items.Count > 1)
                        prefijoDocumento.Items.Insert(0, "Seleccione:");
                }

                string  nitCliente = "", nitClienteAlterno = ""; 

                if(Request.QueryString["cod_vend"] != null)
                {
                    bind.PutDatasIntoDropDownList(ddlVendedorAutenticacion, "select pven_codigo, pven_nombre from pvendedor where pven_codigo = '" + Request.QueryString["cod_vend"] + "'");
                    validaDatos(sender, e);
                }
                else
                {
                    bind.PutDatasIntoDropDownList(ddlVendedorAutenticacion, AMS.Vehiculos.Vendedores.VENDEDORESVEHICULOS);     
                }
				if(Request.QueryString["prefP"]!=null)
				{

                    Utils.MostrarAlerta(Response, "Se ha creado el pedido de cliente vehiculo con prefijo " + Request.QueryString["prefP"] + " y el número " + Request.QueryString["numped"] + ".");
					try
					{

                        formatoRecibo.Prefijo=Request.QueryString["prefP"];
						formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numped"]);
						formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefP"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
						{
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
						}
						imprimir_carta(Request.QueryString["prefP"],Request.QueryString["numped"]);
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
					}
                    
                    try
                    {
                        DataSet datosNit = new DataSet();
                        DBFunctions.Request(datosNit, IncludeSchema.NO, "Select mnit_nit,mnit_nit2 from DBXSCHEMA.MPEDIDOVEHICULO MP WHERE PDOC_CODIGO = '"+formatoRecibo.Prefijo+"' AND MPED_NUMEPEDI = "+formatoRecibo.Numero+" ");
                        if (datosNit.Tables.Count == 1 && datosNit.Tables[0].Rows[0][0].ToString() != "")
                        {
                            nitCliente        = datosNit.Tables[0].Rows[0][0].ToString();
                            nitClienteAlterno = datosNit.Tables[0].Rows[0][1].ToString();
                        }
                    }
                    catch
                    {
                        nitCliente = "";
                        nitClienteAlterno = "";
                    }
                    // ActualizarFichaCliente(nitCliente, nitClienteAlterno);  se está reventando ver porque 
                }
			}
            if (Request.QueryString["nitCli"] != null && Request.QueryString["nombreCli"] != null) // && Request.QueryString["idDBCli"] != null
            {
                //string fecha1 = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
                //string fecha2 = DateTime.Now.ToString("yyyy-MM-dd");

                //si esta persona ya le hicieron el Habeas data en un rango de 6 meses a partir de hoy, no es necesario volverle a hacer el Habeas data
                //if (DBFunctions.RecordExist("SELECT MNIT_NIT FROM MBASEDATOS WHERE MNIT_NIT = '" + Request.QueryString["nitCli"] + "' AND MBAS_FECHA BETWEEN '" + fecha1 + "' AND '" + fecha2 + "' AND "))
                //{
                //    return;
                //}
                Session["tipoDB"] = "FV"; // Request.QueryString["idDBCli"];
                Session["nit"] = Request.QueryString["nitCli"];
                Session["nombre"] = Request.QueryString["nombreCli"];
                plcAutorizar.Visible = true;
                autorizar.Visible = true;
                plcAutorizar.Controls.Add(LoadControl(pathToControls + "AMS.Tools.AutorizacionCliente.ascx"));
                string baseNit = DBFunctions.SingleData("select MNIT_NIT from mbasedatos where TBAS_CODIGO  ='" + Session["tipoDB"] + "' AND MNIT_NIT = '" + Session["nit"] + "' ");
                if (baseNit != "" || (Session["finAutorizar"] != null && IsPostBack))
                {
                    plcAutorizar.Visible = false;
                    autorizar.Visible = false;
                }
            }

            imglupa.Attributes.Add("OnClick","ModalDialog("+numeroPedido.ClientID+",'SELECT rtrim(CAST(mped_numepedi as char(10))) as pedido FROM mpedidovehiculo WHERE pdoc_codigo=\\'"+prefijoDocumento.SelectedValue+"\\' AND test_tipoesta not in (40,60) order by mped_numepedi' ,new Array() );");
		}


        [Ajax.AjaxMethod]
        public String Consultar_Cliente(string Nitcli)
        {
            string respuesta = "";
            respuesta = DBFunctions.SingleData("SELECT MNIT_NIT FROM MCLIENTE WHERE MNIT_NIT = '" + Nitcli + "';");
            return respuesta;
        }
        protected void validaDatos(Object sender, EventArgs e)
        {
            DatasToControls bind = new DatasToControls();
            if (Request.QueryString["cod_vend"] != null)
            {
                txtContrasena.Visible = false;
                plhDatosVeh.Visible = true;
                Button1.Visible = false;

                bind.PutDatasIntoDropDownList(prefijoDocumento, string.Format(Documento.DOCUMENTOSTIPO, "PC"));
                Cambio_Prefijo(null, null);

                if (prefijoDocumento.Items.Count > 1)
                    prefijoDocumento.Items.Insert(0, "Seleccione...");
            }
            else
            {
                
                if (Request.QueryString["cont"] != "1")
                {
                    string contrasenaVendedor = DBFunctions.SingleData("select PVEN_CLAVE from DBXSCHEMA.PVENDEDOR WHERE PVEN_CODIGO='" +
                                ddlVendedorAutenticacion.SelectedValue + "'");

                    if (contrasenaVendedor == txtContrasena.Text)
                    {
                        if (contrasenaVendedor.ToString().Trim() == "")
                            Utils.MostrarAlerta(Response, "Este vendedor no tiene una clave asignada, por favor no olvide asignarla.");
                        ddlVendedorAutenticacion.Enabled = false;
                        txtContrasena.Visible = false;
                        plhDatosVeh.Visible = true;
                        Button1.Visible = false;

                        bind.PutDatasIntoDropDownList(prefijoDocumento, string.Format(Documento.DOCUMENTOSTIPO, "PC"));
                        Cambio_Prefijo(null, null);

                        if (prefijoDocumento.Items.Count > 1)
                            prefijoDocumento.Items.Insert(0, "Seleccione...");
                    }
                    else
                    {
                        Utils.MostrarAlerta(Response, "Verifique la contraseña");
                    }
                }
                else
                {
                    Cambio_Prefijo(null, null);
                    bind.PutDatasIntoDropDownList(prefijoDocumento, string.Format(Documento.DOCUMENTOSTIPO, "PC"));
                    if (prefijoDocumento.Items.Count > 1)
                        prefijoDocumento.Items.Insert(0, "Seleccione:");
                }
            }
            
        }

        protected void ActualizarFichaCliente(string nitCliente, string nitClienteAlterno)
        {
            //Validar la apertura de la ventana contra la tabla MCLIENTE.
            if (Convert.ToInt32(DBFunctions.SingleData("Select COUNT(*) from DBXSCHEMA.MCLIENTE where MNIT_NIT='" + nitCliente.ToString() + "'")) == 0)
            {
                Utils.MostrarAlerta(Response, "Por Favor ingrese los datos para el cliente principal en la ventana emergente.");
                Response.Write("<script language:javascript>w=window.open('AMS.Web.index.aspx?process=DBManager.Inserts&table=MCLIENTE&action=insert&fksI=MNIT_NIT!" + nitCliente.ToString() + "&borrarS=NO','','HEIGHT=600,WIDTH=900,scrollbars=YES');</script>");
            }
            else
            {
                Utils.MostrarAlerta(Response, "Por Favor actualice los datos para el cliente principal en la ventana emergente.");
                Response.Write("<script language:javascript>w=window.open('AMS.Web.index.aspx?process=DBManager.Inserts&table=MCLIENTE&action=update&pks=MNIT_NIT=\\'" + nitCliente.ToString() + "\\'','','HEIGHT=600,WIDTH=900,scrollbars=YES');</script>");
            }
            if (nitCliente.ToString() != nitClienteAlterno.ToString())
            {
                if (Convert.ToInt32(DBFunctions.SingleData("Select COUNT(*) from DBXSCHEMA.MCLIENTE where MNIT_NIT='" + nitClienteAlterno.ToString() + "'")) == 0)
                {
                    Utils.MostrarAlerta(Response, "Por Favor ingrese los datos para el cliente alterno en la ventana emergente.");
                    Response.Write("<script language:javascript>w=window.open('AMS.Web.index.aspx?process=DBManager.Inserts&table=MCLIENTE&action=insert&fksI=MNIT_NIT!" + nitClienteAlterno.ToString() + "','','HEIGHT=600,WIDTH=800,scrollbars=YES');</script>");
                }
                else
                {
                    Utils.MostrarAlerta(Response, "Por Favor actualice los datos para el cliente alterno en la ventana emergente.");
                    Response.Write("<script language:javascript>w=window.open('AMS.Web.index.aspx?process=DBManager.Inserts&table=MCLIENTE&action=update&pks=MNIT_NIT=\\'" + nitClienteAlterno.ToString() + "\\'','','HEIGHT=600,WIDTH=800,scrollbars=YES');</script>");
                }
            }
        }

		protected void imprimir_carta(string prefijo,string numero)
		{
			string prefPed=prefijo;
			string numPed=numero;
			DBFunctions.NonQuery("drop view DBXSCHEMA.VVEHICULOS_PEDIDOSDEVEHICULOSCLIENTES_R");
			DBFunctions.NonQuery("CREATE VIEW DBXSCHEMA.VVEHICULOS_PEDIDOSDEVEHICULOSCLIENTES_R  AS SELECT *  FROM DBXSCHEMA.VVEHICULOS_PEDIDOSDEVEHICULOSCLIENTES where pdoc_codigo='"+prefPed+"' and mped_numepedi="+numPed+" ");
			ConString=System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
			Label lbvacio = new Label();
			string[] Formulas = new string[8];
			string[] ValFormulas = new string[8];
			string header = "AMS_HEADER.rpt";
			string footer = "AMS_FOOTER.rpt";
			DataSet tempDS = new DataSet();
			Formulas[0] = "CLIENTE";
			Formulas[1] = "NIT";
			Formulas[2] = "TITULO";
			Formulas[3] = "TITULO1";
			Formulas[4] = "SELECCION1";
			Formulas[5] = "SELECCION2";
			Formulas[6] = "VERSION";
			Formulas[7] = "REPORTE";

			string empresa = DBFunctions.SingleData("select  cemp_nombre from dbxschema.cempresa");
			ValFormulas[0] = ""+empresa+""; //nombre empresa
			string nit = DBFunctions.SingleData("select  mnit_nit from dbxschema.cempresa");

			DataSet datosReporte = new DataSet();
			DBFunctions.Request(datosReporte,IncludeSchema.NO,"Select PMES_NOMBRE,PANO_DETALLE from DBXSCHEMA.CVEHICULOS CVEH , DBXSCHEMA.PANO PANO,DBXSCHEMA.PMES PMES WHERE PANO.PANO_ANO=CVEH.PANO_ANO AND PMES.PMES_MES=CVEH.PMES_MES");
			string mes = datosReporte.Tables[0].Rows[0][0].ToString();
			string ano = datosReporte.Tables[0].Rows[0][1].ToString();
			ValFormulas[1] = ""+nit+"" ;
			ValFormulas[2] = "CARTA DE BIENVENIDA"; //titulo rpt
			ValFormulas[3] = "SISTEMA DE VEHICULOS"; //subtitulo Sistema de Nomina
			ValFormulas[4] = "AÑO:"+ano+" MES:"+mes+" "; //año mes 
			ValFormulas[5] = ""; //
			ValFormulas[6] = "ECAS - AMS VER 3.0.1";
			Imprimir funcion=new Imprimir();
			string servidor=ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
			string database=ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
			string usuario=ConfigurationManager.AppSettings["UID"];
			string password=ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];
			AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
			miCripto.IV=ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
			miCripto.Key=ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
			string newPwd=miCripto.DescifrarCadena(password);
			string nomA = "Carta Bienvenida";
            string cartarpt = DBFunctions.SingleData ("SELECT SFOR_NOMBRPT FROM SFORMATODOCUMENTOCRYSTAL WHERE SFOR_CODIGO = 'FORMCART';");
            try
            {
                funcion.PreviewReport2(cartarpt, header, footer, 1, 1, 1, "", "", nomA, "pdf", usuario, newPwd, Formulas, ValFormulas, lbvacio);
            }
            catch
            {
                funcion.PreviewReport2("AMS.Vehiculos.CartadeBienvenida", header, footer, 1, 1, 1, "", "", nomA, "pdf", usuario, newPwd, Formulas, ValFormulas, lbvacio);
            }
    
            Response.Write("<script language:javascript>w=window.open('"+funcion.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
		}

		protected void Cambio_Prefijo(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(numeroPedido, "SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='" + prefijoDocumento.SelectedValue + "' AND  test_tipoesta in (10,20) AND PVEN_CODIGO = '" + ddlVendedorAutenticacion.SelectedValue + "' order by mped_numepedi");
            if (numeroPedido.Items.Count > 1)
            {
                numeroPedido.Items.Insert(0, "Seleccione...");                
            }
        }
		
		protected void Ingresar_Modificar(Object  Sender, EventArgs e)
		{
			if(numeroPedido.Items.Count>0)
            {
                if (Request.QueryString["Precio"] != null)
                    Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientesFormulario&proc=modi&pref=" + prefijoDocumento.SelectedValue + "&nume=" + numeroPedido.SelectedValue + "&Precio=1");
                else
                    Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientesFormulario&proc=modi&pref=" + prefijoDocumento.SelectedValue + "&nume=" + numeroPedido.SelectedValue + "");
            }
			else
                Utils.MostrarAlerta(Response, "No se ha seleccionado ningún pedido");
		}
		
		protected void ConsultarAbonos(Object  Sender, EventArgs e)
		{
			if(numeroPedido.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Vehiculos.ConsultaPagosPedido&Orig=1&prefPed="+prefijoDocumento.SelectedValue+"&numPed="+numeroPedido.SelectedValue+"");
			else
                Utils.MostrarAlerta(Response, "No se ha seleccionado ningún pedido");
		}

		protected void CancelarPedido(Object sender, EventArgs e)
		{
			//Verificar que el estado sea de 10
			if (DBFunctions.SingleData("Select test_tipoesta from DBXSCHEMA.MPEDIDOVEHICULO where PDOC_CODIGO='"+prefijoDocumento.SelectedValue+"' and MPED_NUMEPEDI="+numeroPedido.SelectedValue+"")=="10")
			{
				//Verificar que no tenga Anticipos de caja
				if (DBFunctions.RecordExist("SELECT * FROM manticipovehiculo WHERE mped_codigo='"+prefijoDocumento.SelectedValue+"' AND mped_numepedi="+numeroPedido.SelectedValue+" and test_estado=10"))
				{
                    Utils.MostrarAlerta(Response, "Este pedido tiene abono en caja creado, no puede ser cancelado");
				}
				else
				{
					DBFunctions.NonQuery("update DBXSCHEMA.MPEDIDOVEHICULO set test_tipoesta=40 where PDOC_CODIGO='"+prefijoDocumento.SelectedValue+"' and MPED_NUMEPEDI="+numeroPedido.SelectedValue+"");
                    Utils.MostrarAlerta(Response, "Pedido Cancelado Exitosamente");
				}
			}
			else
			{
                Utils.MostrarAlerta(Response, "El pedido no tiene el estado apropiado para ser cancelado.");
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
