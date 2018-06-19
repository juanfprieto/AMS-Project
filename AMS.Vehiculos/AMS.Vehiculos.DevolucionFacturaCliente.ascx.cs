namespace AMS.Vehiculos
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Documentos;
    using AMS.Tools;
    using System.Collections;
    using System.Web.UI;

	/// <summary>
	///		Descripción breve de AMS_Vehiculos_DevolucionFacturaCliente.
	/// </summary>
	public partial class AMS_Vehiculos_DevolucionFacturaCliente : System.Web.UI.UserControl
	{
		#region Atributos

		protected System.Web.UI.WebControls.Button btnDevPed;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected string proceso = "";  // parametro para detectar si es ELIMINACION
        public bool errorFecha = true;

		#endregion	

		#region Eventos

		protected void Page_Load(object sender, System.EventArgs e)
		{
            
			if(!IsPostBack)
			{
                proceso = Request.QueryString["accion"]; //LEE el parametrs si es ELIMINACION para BORRAR de todo lado la factura
                ViewState["eliminar"] = proceso;
                DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(prefijoPedidoOtro,string.Format(Documento.DOCUMENTOSTIPO,"PC"));
                if (prefijoPedidoOtro.Items.Count > 1)
                    prefijoPedidoOtro.Items.Insert(0, "Seleccione:..");
                else
				    bind.PutDatasIntoDropDownList(numeroPedidoOtro,
					@"SELECT MPV.mped_numepedi  
					 FROM mpedidovehiculo MPV, masignacionvehiculo MAV, mvehiculo MVH  
					 WHERE MPV.test_tipoesta=30 AND MPV.pdoc_codigo='"+prefijoPedidoOtro.SelectedValue+@"' AND MAV.pdoc_codigo=MPV.pdoc_codigo AND MAV.mped_numepedi = MPV.mped_numepedi AND MAV.mveh_inventario = MVH.mveh_inventario AND MVH.test_tipoesta = 40  
					 ORDER BY MPV.mped_numepedi;");
                bind.PutDatasIntoDropDownList(prefijoDevoluciones, "SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NC' and PH.tpro_proceso in ('VN','VU') AND P.PDOC_CODIGO = PH.PDOC_CODIGO and pdoc_numefina > pdoc_numeinic and pdoc_ultidocu < pdoc_numefina");
				if (prefijoDevoluciones.Items.Count == 0)
                    Utils.MostrarAlerta(Response, "No se ha configurado el prefijo para las devoluciones de factura cliente. Revise Por Favor.");
                string notaAcceDescrip = "";
                if (Request.QueryString["prefNotaAcce"] != null && Request.QueryString["prefNotaAcce"] != "")
                {
                    notaAcceDescrip = " Nota credito por Accesorios generada: " + Request.QueryString["prefNotaAcce"] + "-" + Request.QueryString["numNotaAcce"];
                    FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                    formatoRecibo.Prefijo = Request.QueryString["prefNotaAcce"];
                    formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefNotaAcce"] + "'");
                    formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numNotaAcce"].ToString());
                    if (formatoRecibo.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                }
                if (Request.QueryString["prefPed"] != null)
                {
                    Utils.MostrarAlerta(Response, "Factura de Cliente Devuelta Exitosamente, pedido: " + Request.QueryString["prefPed"] + "-" + Request.QueryString["ped"] + ". Nota credito generada: " + Request.QueryString["prefNota"] + "-" + Request.QueryString["numNota"] + notaAcceDescrip + "");
                    FormatosDocumentos formatoRecibo = new FormatosDocumentos();
                    formatoRecibo.Prefijo = Request.QueryString["prefNota"];
                    formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefNota"] + "'");
                    formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numNota"].ToString());
                    if (formatoRecibo.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                }

                // "&prefNotaAcce=" + pedidoDevolucion.PrefNotaAccesorios + "&numNotaAcce=" + pedidoDevolucion.NumeNotaAccesorios);
                imglupa.Attributes.Add("OnClick","ModalDialog("+numeroPedidoOtro.ClientID+",'SELECT rtrim(cast(MPV.mped_numepedi as char(10))) as Pedido FROM mpedidovehiculo MPV, masignacionvehiculo MAV, mvehiculo MVH WHERE MPV.test_tipoesta=30 AND MPV.pdoc_codigo=\\'"+prefijoPedidoOtro.SelectedValue+"\\' AND MAV.pdoc_codigo=MPV.pdoc_codigo AND MAV.mped_numepedi = MPV.mped_numepedi AND MAV.mveh_inventario = MVH.mveh_inventario AND MVH.test_tipoesta <> 60 ORDER BY MPV.mped_numepedi ' ,new Array() );");
                fechNota.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
		}
		
		protected void Page_PreRender(object sender, System.EventArgs e)
		{
			if (this.oprimioBtnDevPed.Value=="true")
			{
				this.RealizarDevolucion(sender,e);
                if (errorFecha)
                    return;
				this.oprimioBtnDevPed.Value = "false";
			}
		}

		protected void RealizarDevolucion(Object  Sender, EventArgs e)
		{
            ArrayList sqlStrings = new ArrayList();

       		bool valido = true;
			if(numeroPedidoOtro.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado un pedido para realizar la devolución. Revise Por Favor.");
				valido = false;
			}
			if(prefijoDevoluciones.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado un prefijo para la nota de devolución. Revise Por Favor.");
				valido = false;
			}
            if (ddlNotaDebito.Items.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se ha seleccionado un prefijo para la nota Débito. Revise Por Favor.");
                valido = false;
            }
			// Validamos que el estado del vehículo este en FACTURADO.
			if(Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.mvehiculo where test_tipoesta=40 and mveh_inventario = (Select mveh_inventario from dbxschema.masignacionvehiculo where pdoc_codigo='"+prefijoPedidoOtro.SelectedValue+"' and mped_numepedi="+numeroPedidoOtro.SelectedValue+")")) == 0)
			{
                Utils.MostrarAlerta(Response, "El vehículo NO se encuentra en estado facturado. Revise Por Favor.");
				valido = false;
			}
			// Validamos que el estado del pedido este en FACTURADO.
			if(Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.mpedidovehiculo where test_tipoesta=30 and pdoc_codigo='"+prefijoPedidoOtro.SelectedValue+"' and mped_numepedi="+numeroPedidoOtro.SelectedValue)) == 0)
			{
                Utils.MostrarAlerta(Response, "El pedido no se encuentra en estado facturado. Revise Por Favor.");
				valido = false;
			}

            // validación de la fecha de devolución
            if (fechNota.Text == "")
            {
                Utils.MostrarAlerta(Response, "Olvidó insertar la fecha de la devolución ..");
                errorFecha = true;
                return;
            }
            DateTime fechaDevolucion = Convert.ToDateTime(fechNota.Text.Trim());
            string anoyMes  = DBFunctions.SingleData("SELECT PANO_ANO concat '-' concat PMES_MES FROM CVEHICULOS");
            string[] anoMes = anoyMes.Split('-');
            if (fechaDevolucion.Year.ToString() != anoMes[0].ToString() || fechaDevolucion.Month.ToString() != anoMes[1].ToString())
            {
                Utils.MostrarAlerta(Response, "La fecha de la devolución " + fechaDevolucion.Year.ToString() + " - " + fechaDevolucion.Month.ToString() + "  es diferente a la vigencia de Vehículos " + anoyMes + ". Revise Por Favor");
                valido = false;
            }

			if (valido)
			{
				PedidoCliente pedidoDevolucion = new PedidoCliente(prefijoPedidoOtro.SelectedValue,
                    numeroPedidoOtro.SelectedValue, prefijoDevoluciones.SelectedValue, fechaDevolucion.ToString());

                pedidoDevolucion.PrefNotaDebito = ddlNotaDebito.SelectedValue;
                pedidoDevolucion.NumeNotaDebito = Convert.ToUInt32(DBFunctions.SingleData("select pdoc_ultidocu + 1 from pdocumento where pdoc_codigo='" + ddlNotaDebito.SelectedValue + "';"));
                pedidoDevolucion.UsuarioActual = HttpContext.Current.User.Identity.Name.ToLower();
                pedidoDevolucion.Observaciones = TextArea1.Value;
                /*Retencion retencion = new Retencion(
                                            pedidoDevolucion.Nit,
                                            pedidoDevolucion.PrefijoNotaDevCli,
                                            (int)facturaVentaVehiculo.NumeroFactura,
                                            facturaVentaVehiculo.ValorFactura,
                                            facturaVentaVehiculo.ValorIva,
                                            "V");
                */
                proceso = ViewState["eliminar"] == null ? "" : ViewState["eliminar"].ToString();
                //proceso = ViewState["eliminar"].ToString(); //esto no funciona porque el viewState puede venir null.

                if ( proceso == "E") // en eliminacion la fecha tiene que ser la misma de la factura
                { 
                    string prefijoFactura = DBFunctions.SingleData("SELECT pdoc_codigo FROM mfacturapedidovehiculo WHERE mped_codigo='" + prefijoPedidoOtro.SelectedValue + "' AND mped_numepedi=" + numeroPedidoOtro.SelectedValue + "");
                    uint numeroFactura = Convert.ToUInt32(DBFunctions.SingleData("SELECT mfac_numedocu FROM mfacturapedidovehiculo WHERE mped_codigo='" + prefijoPedidoOtro.SelectedValue + "' AND mped_numepedi=" + numeroPedidoOtro.SelectedValue + ""));

                    if (!DBFunctions.RecordExist("SELECT MFAC_FACTURA FROM MFACTURACLIENTE MF, CVEHICULOS CV WHERE PDOC_CODIGO = '" + prefijoFactura + "' and mfac_numedocu = " + numeroFactura + " and year(mfac_factura) = cv.pano_ano and month(mfac_factura) = cv.pmes_mes "))
                    {
                        Utils.MostrarAlerta(Response, "La fecha de la Eliminacion No es igual a la fecha Original de la factura ");
                        return;
                    }
                }

                if (pedidoDevolucion.RealizarDevolucion(proceso))
				{
                    sqlStrings.Add(@"UPDATE MPEDIDOVEHICULO SET MPED_OBSERVACION = '" + TextArea1.Value + "' WHERE PDOC_CODIGO = '" + prefijoPedidoOtro.SelectedValue + "' AND   MPED_NUMEPEDI = " + numeroPedidoOtro.SelectedValue);
                    if (!DBFunctions.Transaction(sqlStrings))
                    {
                        Utils.MostrarAlerta(Response, "Error al Realizar Devolucion ");
                    }

                    Response.Redirect("" + indexPage + "?process=Vehiculos.DevolucionFacturaCliente&el=1&prefPed=" + prefijoPedidoOtro.SelectedValue + "&ped=" + numeroPedidoOtro.SelectedValue + "&prefNota=" + pedidoDevolucion.PrefijoNota + "&numNota=" + pedidoDevolucion.NumeroNota + "&prefNotaAcce=" + pedidoDevolucion.PrefNotaAccesorios + "&numNotaAcce=" + pedidoDevolucion.NumeNotaAccesorios);
                }
				else
					lb.Text += pedidoDevolucion.ProcessMsg;
			}
           
		}
		
		protected void Cambio_Tipo_Documento_Otros(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(numeroPedidoOtro, "SELECT MPV.mped_numepedi FROM mpedidovehiculo MPV, masignacionvehiculo MAV, mvehiculo MVH WHERE MPV.test_tipoesta=30 AND MPV.pdoc_codigo='" + prefijoPedidoOtro.SelectedValue.ToString() + "' AND MAV.pdoc_codigo=MPV.pdoc_codigo AND MAV.mped_numepedi = MPV.mped_numepedi AND MAV.mveh_inventario = MVH.mveh_inventario AND MVH.test_tipoesta <> 60 order by MPV.mped_numepedi");
		}

		#endregion

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

		protected void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			//Validar VIN
			pnlDevolver.Visible=false;
			if(!DBFunctions.RecordExist(
				"SELECT * "+
				"FROM mpedidovehiculo MPV, masignacionvehiculo MAV, mvehiculo MVH "+
				"WHERE "+
				"MPV.pdoc_codigo='"+prefijoPedidoOtro.SelectedValue+"' AND MAV.pdoc_codigo = MPV.pdoc_codigo AND "+
				"MPV.mped_numepedi="+numeroPedidoOtro.SelectedValue+" AND MAV.mped_numepedi = MPV.mped_numepedi AND "+
				"MAV.mveh_inventario = MVH.mveh_inventario AND MVH.MCAT_VIN = '"+txtVIN.Text.Trim()+"';"))
            {
                Utils.MostrarAlerta(Response, "El VIN no es correcto. Revise Por Favor.");
				return;
			}
			CargarDatosPedido();
			prefijoPedidoOtro.Enabled=numeroPedidoOtro.Enabled=false;
			btnSeleccionar.Visible=false;
			pnlDevolver.Visible=true;
			txtVIN.ReadOnly=true;
		}

		private void CargarDatosPedido()
		{
			DataSet dsPedido=new DataSet();
			DBFunctions.Request(
				dsPedido,IncludeSchema.NO,		
            @"SELECT MPV. *,
            PV.PVEN_NOMBRE, 
            MN.MNIT_NOMBRES CONCAT ' ' CONCAT MN.MNIT_NOMBRE2 CONCAT ' ' CONCAT MN.MNIT_APELLIDOS CONCAT ' ' CONCAT MN.MNIT_APELLIDO2 AS PROPIETARIO
            FROM MPEDIDOVEHICULO MPV,
                 PVENDEDOR PV, 
                 MNIT MN
            WHERE PV.PVEN_CODIGO = MPV.PVEN_CODIGO 
             AND  MPV.pdoc_codigo = '"+prefijoPedidoOtro.SelectedValue+@"'
             AND  MN.MNIT_NIT  = MPV.MNIT_NIT
             AND  MPV.mped_numepedi = "+numeroPedidoOtro.SelectedValue
			);

            if (dsPedido.Tables[0].Rows.Count > 0)
            {
                lblDatosPedido.Text =
                    "Pedido:<br>" +
                    "&nbsp;Catalogo:&nbsp;" + dsPedido.Tables[0].Rows[0]["PCAT_CODIGO"] + "<br>" +
                    "&nbsp;Fecha:&nbsp;" + dsPedido.Tables[0].Rows[0]["MPED_PEDIDO"] + "<br>" +
                    "&nbsp;Vendedor:&nbsp;" + dsPedido.Tables[0].Rows[0]["PVEN_NOMBRE"] + "<br>" +
                    "&nbsp;Propietario:&nbsp;" + dsPedido.Tables[0].Rows[0]["MNIT_NIT"] + " " + dsPedido.Tables[0].Rows[0]["PROPIETARIO"] + "<br>" +
                    "&nbsp;Observaciones:&nbsp;<br>";
                    TextArea1.Value = Convert.ToString(dsPedido.Tables[0].Rows[0]["MPED_OBSERVACION"]);

                string prefijoFactura = DBFunctions.SingleData("SELECT pdoc_codigo FROM mfacturapedidovehiculo WHERE mped_codigo='" + prefijoPedidoOtro.SelectedValue + "' AND mped_numepedi=" + numeroPedidoOtro.SelectedValue + "");
                uint numeroFactura = Convert.ToUInt32(DBFunctions.SingleData("SELECT mfac_numedocu FROM mfacturapedidovehiculo WHERE mped_codigo='" + prefijoPedidoOtro.SelectedValue + "' AND mped_numepedi=" + numeroPedidoOtro.SelectedValue + ""));
                string almacen = DBFunctions.SingleData("SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura);
                ViewState["almacen"] = almacen;

                Utils.llenarPrefijos(Response, ref ddlNotaDebito, "ND", almacen, "FC");
            }
            else
            {
                Utils.MostrarAlerta(Response, "No se encontró el pedido.");
                return;
            }
		}
	}
}
