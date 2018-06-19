namespace AMS.Vehiculos
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.Forms;
    using AMS.DB;
	using System.Configuration;
    using AMS.Contabilidad;
    using AMS.Tools;
    using AMS.Documentos;


	/// <summary>
	///		Descripción breve de AMS_Vehiculos_DevolucionPedidoProveedor.
	/// </summary>
	public partial class AMS_Vehiculos_DevolucionPedidoProveedor : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        ProceHecEco contaOnline = new ProceHecEco();
        protected FormatosDocumentos formatoFactura;


		protected void Page_Load(object sender, System.EventArgs e)
		{
            
			if(!IsPostBack)
            {
                tbDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlNotDevProv, "SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre FROM pdocumento as P, pdocumentohecho as PH WHERE p.tdoc_tipodocu = 'NP' and PH.tpro_proceso in ('VN','VU') AND P.PDOC_CODIGO = PH.PDOC_CODIGO");
                if (ddlNotDevProv.Items.Count > 1)
                    ddlNotDevProv.Items.Insert(0, "Seleccione:..");
                else
                    if (ddlNotDevProv.Items.Count == 0)
                    {
                        Utils.MostrarAlerta(Response, "No hay ningun documento para devolver vehículos.\\nRevise Por Favor");
                        return;
                    }
                bind.PutDatasIntoDropDownList(ddlCatVehDev, CatalogoVehiculos.CATALOGOVEHICULOSRECEPCIONADOS);
                if (ddlCatVehDev.Items.Count > 1)
                    ddlCatVehDev.Items.Insert(0, "Seleccione:..");
                else
                {
                    if (ddlCatVehDev.Items.Count == 1)
                        bind.PutDatasIntoDropDownList(ddlVINDev, string.Format(Vehiculos.VEHICULOSRECEPCIONADOS, ddlCatVehDev.SelectedValue));
                    else
                    {
                        Utils.MostrarAlerta(Response, "No hay ningun vehículo para devolver.\\nRevise Por Favor");
                        return;
                    }
                }

                if (Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                    ImprimirFormatoPDF();
                imglupa.Attributes.Add("OnClick", "ModalDialog(" + ddlCatVehDev.ClientID + ",'SELECT pcat_codigo, pcat_descripcion FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo IN (SELECT DISTINCT MC.pcat_codigo FROM DBXSCHEMA.MVEHICULO mv, McatalogoVEHICULO MC WHERE test_tipoesta IN (10,20) AND   MC.MCAT_VIN = MV.MCat_VIN) ORDER BY pcat_codigo',new Array() );");
	        }
		}

        protected void ImprimirFormatoPDF()
        {
            string prefijo = Request.QueryString["pref"];
            string numero = Request.QueryString["num"];

            Utils.MostrarAlerta(Response, "Se ha creado la devolución de la factura del proveedor con prefijo: " + prefijo + " y número: " + numero + "");

            formatoFactura = new FormatosDocumentos();
            try
            {
                formatoFactura.Prefijo = prefijo;
                formatoFactura.Numero = Convert.ToInt32(numero);
                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijo + "'");

                if (formatoFactura.Codigo != string.Empty)
                {
                    if (formatoFactura.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                }
                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + prefijo + "'");
                if (formatoFactura.Codigo != string.Empty)
                {
                    if (formatoFactura.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                }
            }
            catch
            {
                lb.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
            }
        }

		protected void CambioCatalogoDevolucion(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlVINDev,string.Format(Vehiculos.VEHICULOSRECEPCIONADOS,ddlCatVehDev.SelectedValue));
		}

        protected void ChangeDate(Object Sender, EventArgs E)
        {
            tbDate.Text = calDate.SelectedDate.ToString("yyyy-MM-dd");
        }

		protected void RealizarDevolucion(Object  Sender, EventArgs e)
		{
			if(ddlVINDev.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado algun vehículo para devolver.\\nRevise Por Favor");
				return;
			}
			uint numPDVeh = 0;
            Recepcion recepcionNota = new Recepcion();
            DateTime fechaDevolucion = Convert.ToDateTime(tbDate.Text);

            string error = Recepcion.DevolverVehiculoProveedor(ddlVINDev.SelectedValue, ddlNotDevProv.SelectedValue, HttpContext.Current.User.Identity.Name, ref numPDVeh, txtobsv.Text, fechaDevolucion);
            if (error != "")
			{
                Utils.MostrarAlerta(Response, "" + (error.Split('*'))[0] + ".\\nRevise Por Favor");
				lb.Text += "<br>"+(error.Split('*'))[1];
			}
			else if(numPDVeh != 0)
                 {
                     string numeroNota = DBFunctions.SingleData("SELECT MFAC_NUMEORDEPAGO FROM MVEHICULO WHERE MVEH_INVENTARIO = "+ddlVINDev.SelectedValue.ToString()+" ");
                     contaOnline.contabilizarOnline(ddlNotDevProv.SelectedValue.ToString(), Convert.ToInt32(numeroNota.ToString()), fechaDevolucion, "");
                     
                  // Mostrar el formato de la NOTA DE LA DEVOLUCION
                     Utils.MostrarAlerta(Response, "Se ha creado la devolucion de la factura del proveedor con prefijo " + ddlNotDevProv.SelectedValue.ToString() + " y número " + numeroNota.ToString() + "");
                     
                       formatoFactura = new FormatosDocumentos();
                     try
                     {
                         formatoFactura.Prefijo = ddlNotDevProv.SelectedValue.ToString();
                         formatoFactura.Numero  = Convert.ToInt32(numeroNota.ToString());
                         formatoFactura.Codigo  = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlNotDevProv.SelectedValue.ToString() + "'");
                       
                         if (formatoFactura.Codigo != string.Empty)
                         {
                             if (formatoFactura.Cargar_Formato())
                                 Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                         }
                         formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlNotDevProv.SelectedValue.ToString() + "'");
                         if (formatoFactura.Codigo != string.Empty)
                         {
                             if (formatoFactura.Cargar_Formato())
                                 Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                         }
                     }
                     catch
                     {
                         lb.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                     }

                     Response.Redirect("" + indexPage + "?process=Vehiculos.DevolucionPedidoProveedor&pref=" + ddlNotDevProv.SelectedValue + "&num=" + numeroNota);
                 }
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
}
