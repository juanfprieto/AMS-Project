// created on 28/03/2005 at 14:36
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
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class FacturaProveedorControl : System.Web.UI.UserControl
	{
		#region Variables

		protected DropDownList ddlNotDevProv, ddlCatVehDev, ddlVINDev;
		protected System.Web.UI.WebControls.Button btnDevolucion;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();
		
		#endregion
		
		#region Eventos

		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack)
            {
                   DatasToControls bind = new DatasToControls();

                bind.PutDatasIntoDropDownList(catalogo2, CatalogoVehiculos.CATALOGOVEHICULOSRECEPCIONADOSSINFACTURAR);
                bind.PutDatasIntoDropDownList(vinVehiculo, string.Format(Vehiculos.VEHICULOSRECEPCIONADOSSINFACTURAR, catalogo2.SelectedValue ));
                bind.PutDatasIntoDropDownList(prefijoPedido2, string.Format(Documento.DOCUMENTOSTIPO, "PV"));
                if (prefijoPedido2.Items.Count == 1)
                    bind.PutDatasIntoDropDownList(numeroPedido2, "SELECT DISTINCT MPED.mped_numepedi NUMERO FROM mpedidovehiculoproveedor MPED, MVEHICULO MVEH WHERE MPED.pdoc_codigo = '" + prefijoPedido.SelectedValue + "' AND MPED.pdoc_codigo =  MVEH.pdoc_codigopediprov AND MPED.mped_numepedi = MVEH.mped_numero AND MVEH.pdoc_codiordepago  = '' AND MVEH.mfac_numeordepago IS NULL ORDER BY NUMERO");
                else
                    prefijoPedido2.Items.Insert(0, "Seleccione:..");
                chkRecepDir.Attributes.Add("onclick", "CambioRcp(" + chkRecepDir.ClientID + "," + prefijoPedido.ClientID + "," + numeroPedido.ClientID + ");");

                if (Request.QueryString["usado"] != null)
                {
                    Session["Retoma"] = "S";
                    plFacturarN.Visible = false;
                    bind.PutDatasIntoDropDownList(prefijoPedido,
                            "select distinct " +
                            "mp.pdoc_codigo, mp.pdoc_codigo CONCAT ' - ' CONCAT dc.pdoc_nombre descripcion " +
                            "from dbxschema.mpedidovehiculo mp, dbxschema.dpedidovehiculoretoma dp, dbxschema.pdocumento dc " +
                            "where mp.pdoc_codigo=dp.pdoc_codigo AND mp.mped_numepedi=dp.mped_numepedi " +
                            "AND dp.mveh_inventario IS NULL AND mp.pdoc_codigo=dc.pdoc_codigo;");

                    bind.PutDatasIntoDropDownList(numeroPedido,
                            "select mp.mped_numepedi " +
                            "from dbxschema.mpedidovehiculo mp, dbxschema.dpedidovehiculoretoma dp " +
                            "where mp.mped_numepedi=dp.mped_numepedi AND mp.pdoc_codigo='" + prefijoPedido.SelectedValue + "' " +
                            "AND dp.mveh_inventario IS NULL order by mp.mped_numepedi;");

                    factura.Checked = true;
                    factura.Enabled = false;
                    chkRecepDir.Checked = true;
                    chkRecepDir.Enabled = false;
                }
                else
                {
                    Session["Retoma"] = "N";
                    plFacturarN.Visible = true;
                    bind.PutDatasIntoDropDownList(prefijoPedido, string.Format(Documento.DOCUMENTOSTIPO, "PV"));
                    if (prefijoPedido.Items.Count == 1)
                        bind.PutDatasIntoDropDownList(numeroPedido, "SELECT DISTINCT MPED.mped_numepedi NUMERO FROM mpedidovehiculoproveedor MPED, dpedidovehiculoproveedor DPED WHERE MPED.pdoc_codigo = DPED.pdoc_codigo AND MPED.mped_numepedi = DPED.mped_numepedi AND DPED.dped_cantpedi > DPED.dped_cantingr AND MPED.pdoc_codigo='" + prefijoPedido.SelectedValue + "' ORDER BY NUMERO");
                    else
                        prefijoPedido.Items.Insert(0, "Seleccione:..");
                }

                if (Request.QueryString["devVeh"] != null)
                    Utils.MostrarAlerta(Response, "Se ha creado la nota de devolución a proveedor con el prefijo " + Request.QueryString["prefDev"] + " y el número " + Request.QueryString["numDev"] + ".");

                else if (Request.QueryString["facOpc"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha creado la factura de proveedor con prefijo " + Request.QueryString["prefFP"] + " y el número " + Request.QueryString["numFP"] + ".");
                    try
                    {
                        formatoRecibo.Prefijo = Request.QueryString["prefFP"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numFP"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefFP"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                    }
                }
            }
		}
		
		protected void Cambio_Prefijo(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            if (Request.QueryString["usado"] != null)
            {
                bind.PutDatasIntoDropDownList(numeroPedido,
                        "select mp.mped_numepedi " +
                        "from dbxschema.mpedidovehiculo mp, dbxschema.dpedidovehiculoretoma dp " +
                        "where mp.mped_numepedi=dp.mped_numepedi AND mp.pdoc_codigo='" + prefijoPedido.SelectedValue + "' " +
                        "AND dp.mveh_inventario IS NULL order by mp.mped_numepedi;");
            }
            else
            {
                bind.PutDatasIntoDropDownList(numeroPedido, "SELECT DISTINCT MPED.mped_numepedi NUMERO FROM mpedidovehiculoproveedor MPED, dpedidovehiculoproveedor DPED WHERE MPED.pdoc_codigo = DPED.pdoc_codigo AND MPED.mped_numepedi = DPED.mped_numepedi AND DPED.dped_cantpedi > DPED.dped_cantingr AND MPED.pdoc_codigo='" + prefijoPedido.SelectedValue + "' ORDER BY NUMERO");
            }
        }
		
		protected void Cambio_Prefijo2(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroPedido2,"SELECT DISTINCT MPED.mped_numepedi FROM mpedidovehiculoproveedor MPED, MVEHICULO MVEH WHERE MPED.pdoc_codigo = '"+prefijoPedido2.SelectedValue+"' AND MPED.pdoc_codigo =  MVEH.pdoc_codigopediprov AND MPED.mped_numepedi = MVEH.mped_numero AND MVEH.pdoc_codiordepago IS NULL AND MVEH.mfac_numeordepago IS NULL");
		}
		
		protected void Cambio_Catalogo(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
            bind.PutDatasIntoDropDownList(vinVehiculo,string.Format(Vehiculos.VEHICULOSRECEPCIONADOSSINFACTURAR, catalogo2.SelectedValue ));
		}
		
		protected void CambioCatalogoDevolucion(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlVINDev,"SELECT mveh_inventario, mcat_vin FROM mvehiculo WHERE pcat_codigo='"+ddlCatVehDev.SelectedValue+"' AND (test_tipoesta=10 OR test_tipoesta=20)");
		}
		
		protected void Confirmar_Recepcion(Object  Sender, EventArgs e)
		{
			if(factura.Checked)
			{
				if(chkRecepDir.Checked)
				{
					if(numeroPedido.Items.Count > 0)
                        if(Request.QueryString["usado"] != null)
						    Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref=" + prefijoPedido.SelectedValue + "&num=" + numeroPedido.SelectedValue + "&fact=S&recDir=N&usado=1");
                        else
                            Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref=" + prefijoPedido.SelectedValue + "&num=" + numeroPedido.SelectedValue + "&fact=S&recDir=N");
					else
                        Utils.MostrarAlerta(Response, "No se ha seleccionado ningun pedido");
				}
				else
					Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref=&num=&fact=S&recDir=S");
			}
			else
			{
				if(chkRecepDir.Checked)
				{
					if(numeroPedido.Items.Count > 0)
						Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref="+prefijoPedido.SelectedValue+"&num="+numeroPedido.SelectedValue+"&fact=N&recDir=N");
					else
                        Utils.MostrarAlerta(Response, "No se ha seleccionado ningun pedido");
				}
				else
					Response.Redirect("" + indexPage + "?process=Vehiculos.RecepcionFormulario&pref=&num=&fact=N&recDir=S");
			}
		}
		
		protected void Confirmar_Factura(Object  Sender, EventArgs e)
		{
			if(vinVehiculo.Items.Count>0)
			{
                Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionFormulario&proc=2&cat="+ GetCodigo(catalogo2.SelectedValue) + "&vin="+vinVehiculo.SelectedValue+"&pref="+DBFunctions.SingleData("SELECT pdoc_codigopediprov FROM mvehiculo WHERE mcat_vin='"+vinVehiculo.SelectedValue+"'")+"&num="+DBFunctions.SingleData("SELECT mped_numero FROM mvehiculo WHERE mcat_vin='"+vinVehiculo.SelectedValue+"'")+"");
			}
			else
                Utils.MostrarAlerta(Response, "No se ha elegido ningun vehículo");
		}
		
		protected void Confirmar_Factura2(Object  Sender, EventArgs e)
		{
			if(numeroPedido2.Items.Count>0)
			{
				Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionFormulario&proc=3&pref="+prefijoPedido2.SelectedValue+"&num="+numeroPedido2.SelectedValue+"");
			}
			else
                Utils.MostrarAlerta(Response, "No se ha elegido ningun pedido");
		}
		
		protected void RealizarDevolucion(Object  Sender, EventArgs e)
		{
			if(ddlVINDev.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado algun vehículo para devolver.\\nRevise Por Favor");
				return;
			}
			uint numPDVeh = 0;
            string error = Recepcion.DevolverVehiculoProveedor(ddlVINDev.SelectedValue, ddlNotDevProv.SelectedValue, HttpContext.Current.User.Identity.Name, ref numPDVeh, "", DateTime.Now);
			if(error != "")
			{
                Utils.MostrarAlerta(Response, "" + (error.Split('*'))[0] + ".\\nRevise Por Favor");
				lb.Text += "<br>"+(error.Split('*'))[1];
			}
			else 
               if(numPDVeh != 0)
				    Response.Redirect("" + indexPage + "?process=Vehiculos.FacturaProveedor&devVeh=S&prefDev="+ddlNotDevProv.SelectedItem.Text+"&numDev="+numPDVeh);
		}
		
        protected string GetCodigo(string codigoCatalogoDDL)
        {
            return codigoCatalogoDDL.Substring(0, codigoCatalogoDDL.Length - 3);
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
