// created on 15/02/2005 at 14:16
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
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class EntregaVehiculos : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo=new FormatosDocumentos();
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
            //prueba unitaria
            //DBFunctions.CollectionTest();

			if(!IsPostBack)
			{
                 DatasToControls bind = new DatasToControls();
                if(Request.QueryString["cod_vend"] != null)
                {
                    bind.PutDatasIntoDropDownList(vendedor, "select pven_codigo, pven_nombre from pvendedor where pven_codigo = '" + Request.QueryString["cod_vend"] + "'");
                    CargarVehiculos(sender, e);
                }
                else
                {
                    string sqlVendedores = "SELECT PVEN_CODIGO ,PVEN_NOMBRE  FROM PVENDEDOR where tVEND_CODIGO = 'VV' AND PVEN_VIGENCIA = 'V' ORDER BY 2";

                    Utils.FillDll(vendedor, sqlVendedores, true);
                }
                if(Request.QueryString["exito"] != null)
                {

                    Utils.MostrarAlerta(Response,"Se ha realizado la programación de la entrega exitosamente");
                        //! \\n Prefijo: " + Request.QueryString["prefDev"] + " número: " + Request.QueryString["numDev"]);
                }
                if (Request.QueryString["alerta"] == "ok")
                    Utils.MostrarAlerta(Response, "Se ha realizado la entrega exitosamente");
                // Reimprimir el formato de entrega
                if (Request.QueryString["prefDev"] != null && Request.QueryString["numDev"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha creado la entrega con prefijo " + Request.QueryString["prefDev"] + " y número " + Request.QueryString["numDev"] + ""); 
                       try
                        {
                            Imprimir.ImprimirRPT(Response, Request.QueryString["prefDev"], Convert.ToInt32(Request.QueryString["numDev"]), true);
                        }
                        catch
                        {
                        //lbInfo.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                        Utils.MostrarAlerta(Response, "Error al generar la impresión.Detalles : " + formatoRecibo.Mensajes + " < br > ");
                        }
                }



                //bind.PutDatasIntoDropDownList(vehiculo,Vehiculos.VEHICULOSFACTURADOS);
                //bind.PutDatasIntoDropDownList(vendedor, "SELECT PVEN_CODIGO, PVEN_NOMBRE FROM PVENDEDOR ORDER BY 1");
                //bind.PutDatasIntoDropDownList(vehiculo,"SELECT MVEHICULO.mveh_inventario, '[' CONCAT MC.pcat_codigo CONCAT '] - [' CONCAT MVEHICULO.mcat_vin CONCAT '] ' "+
                //"concat VMNIT.NOMBRE concat ' [ ' concat rtrim(char(MFACTURACLIENTE.MFAC_FACTURA)) concat ' ] ' concat PVENDEDOR.PVEN_NOMBRE "+
                //"FROM dbxschema.mvehiculo,dbxschema.VMNIT, dbxschema.MFACTURACLIENTE, dbxschema.MFACTURAPEDIDOVEHICULO, "+
                //"dbxschema.MASIGNACIONVEHICULO, dbxschema.PVENDEDOR, DBXSCHEMA.MCATALOGOVEHICULO MC "+
                //"WHERE test_tipoesta=40 "+ 
                //"  AND MVEHICULO.MNIT_NIT = VMNIT.MNIT_NIT "+
                //"  AND MFACTURACLIENTE.PVEN_CODIGO = PVENDEDOR.PVEN_CODIGO "+
                //"  AND mvehiculo.MVEH_INVENTARIO = mASIGNACIONvehiculo.MVEH_INVENTARIO "+
                //"  AND mASIGNACIONvehiculo.PDOC_CODIGO = MFACTURAPEDIDOVEHICULO.MPED_CODIGO "+
                //"  AND mASIGNACIONvehiculo.MPED_NUMEPEDI = MFACTURAPEDIDOVEHICULO.MPED_NUMEPEDI "+
                //"  AND MFACTURAPEDIDOVEHICULO.PDOC_CODIGO = MFACTURACLIENTE.PDOC_CODIGO "+
                //"  AND MFACTURAPEDIDOVEHICULO.MFAC_NUMEDOCU = MFACTURACLIENTE.MFAC_NUMEDOCU "+
                //"  AND MC.MCAT_VIN = MVEHICULO.MCat_VIN "+
                //"  AND PVENDEDOR.PVEN_CODIGO " +
                //"  ORDER BY MFACTURACLIENTE.MFAC_FACTURA, PVENDEDOR.PVEN_NOMBRE");
                //DatasToControls bind = new DatasToControls();
                //				bind.PutDatasIntoDropDownList(vehiculo,Vehiculos.VEHICULOSFACTURADOS);
                //bind.PutDatasIntoDropDownList(vehiculo,@"SELECT MVEHICULO.mveh_inventario,  '' CONCAT MVEHICULO.mcat_vin CONCAT ' - [' CONCAT MC.pcat_codigo CONCAT ']' 
                //                                                    concat VMNIT.NOMBRE concat ' [ ' concat rtrim(char(MFACTURACLIENTE.MFAC_FACTURA)) concat ' ] ' concat PVENDEDOR.PVEN_NOMBRE 
                //                                                    FROM dbxschema.mvehiculo,dbxschema.VMNIT, dbxschema.MFACTURACLIENTE, dbxschema.MFACTURAPEDIDOVEHICULO, 
                //                                                    dbxschema.MASIGNACIONVEHICULO, dbxschema.PVENDEDOR, DBXSCHEMA.MCATALOGOVEHICULO MC 
                //                                                    WHERE test_tipoesta=40
                //                                                    AND MVEHICULO.MNIT_NIT = VMNIT.MNIT_NIT 
                //                                                    AND MFACTURACLIENTE.PVEN_CODIGO = PVENDEDOR.PVEN_CODIGO 
                //                                                    AND mvehiculo.MVEH_INVENTARIO = mASIGNACIONvehiculo.MVEH_INVENTARIO 
                //                                                    AND mASIGNACIONvehiculo.PDOC_CODIGO = MFACTURAPEDIDOVEHICULO.MPED_CODIGO 
                //                                                    AND mASIGNACIONvehiculo.MPED_NUMEPEDI = MFACTURAPEDIDOVEHICULO.MPED_NUMEPEDI 
                //                                                    AND MFACTURAPEDIDOVEHICULO.PDOC_CODIGO = MFACTURACLIENTE.PDOC_CODIGO 
                //                                                    AND MFACTURAPEDIDOVEHICULO.MFAC_NUMEDOCU = MFACTURACLIENTE.MFAC_NUMEDOCU 
                //                                                    AND MC.MCAT_VIN = MVEHICULO.MCat_VIN 
                //                                                    ORDER BY PVENDEDOR.PVEN_NOMBRE, MFACTURACLIENTE.MFAC_FACTURA;");
            }
		}
		
		protected void Generar_Formato_Salida(Object  Sender, EventArgs e)
		{
			if(vehiculo.Items.Count>0)
			{
				//string prefPedido = DBFunctions.SingleData("SELECT pdoc_codigo FROM masignacionvehiculo WHERE mveh_inventario="+vehiculo.SelectedValue+"");
				//string numePedi = DBFunctions.SingleData("SELECT mped_numepedi FROM masignacionvehiculo WHERE mveh_inventario="+vehiculo.SelectedValue+"");
                
                DataSet ds = new DataSet();
                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pdoc_codigo,mped_numepedi FROM masignacionvehiculo WHERE mveh_inventario=" + vehiculo.SelectedValue);            
                int lastRow = ds.Tables[0].Rows.Count -1;

                string prefPedido = ds.Tables[0].Rows[lastRow][0].ToString();
                string numePedi = ds.Tables[0].Rows[lastRow][1].ToString();
                
                try
                {
                    formatoRecibo.Prefijo=prefPedido;
					formatoRecibo.Numero=Convert.ToInt32(numePedi);
					formatoRecibo.Codigo="FORENTRVEH";
					if(formatoRecibo.Codigo!=string.Empty)
					{
						if(formatoRecibo.Cargar_Formato())
							Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
					}
				}
				catch
				{
					lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
				}
				//Response.Redirect("" + indexPage + "?process=Vehiculos.EntregaVehiculosFormulario&prefPedi="++"&numePedi="++"&tipPro=G&numeDocu="+numeDocu+"");
			}
			else
            Utils.MostrarAlerta(Response, "No se ha escogido ningún vehículo");
		}

        protected void Entregar_Vehiculo(Object  Sender, EventArgs e)
		{
			if(vehiculo.Items.Count>0)
			{
				//primero debemos revisar que este vehiculo no tenga ningun vehiculo de retoma pendiente
				//primero traemos el numero de pedido de este vehiculo que ese encuentra en masignacion vehiculo
				string prefPed = DBFunctions.SingleData("SELECT pdoc_codigo FROM masignacionvehiculo WHERE mveh_inventario="+vehiculo.SelectedValue+"");
				string numPed = DBFunctions.SingleData("SELECT mped_numepedi FROM masignacionvehiculo WHERE mveh_inventario="+vehiculo.SelectedValue+"");
				//Luego traemos el codigo y numero de factura para revisar si existen vehiculos pendientes de retoma
				string prefFac = DBFunctions.SingleData("SELECT pdoc_codigo FROM mfacturapedidovehiculo WHERE mped_codigo='"+prefPed+"' AND mped_numepedi="+numPed+" ORDER BY mfac_fechcrea DESC");
				string numFac = DBFunctions.SingleData("SELECT mfac_numedocu FROM mfacturapedidovehiculo WHERE mped_codigo='"+prefPed+"' AND mped_numepedi="+numPed+" ORDER BY mfac_fechcrea DESC");
				//y ahora revisamos si existe algun vehiculo pendiente de retoma
				if(DBFunctions.RecordExist("SELECT * FROM mretomavehiculo WHERE pdoc_codigo='"+prefFac+"' AND mfac_numedocu="+numFac+" AND test_estado='S'"))
                    Utils.MostrarAlerta(Response, "Aun Tiene Vehiculos Pendientes de Retoma");
				else
					Response.Redirect("" + indexPage + "?process=Vehiculos.EntregaVehiculosProceso&numeInv="+vehiculo.SelectedValue+"");
			}
			else
            Utils.MostrarAlerta(Response, "No se ha escogido ningún vehículo");
		}
        protected void programar_entrega(object sender, EventArgs z)
        {
            if (vehiculo.Items.Count > 0 && vehiculo.SelectedValue != "")
            {
                Response.Redirect("" + indexPage + "?process=Vehiculos.EntregaVehiculosProceso&programarEntrega=1&vehiInv=" + vehiculo.SelectedValue);
            }
            else
                Utils.MostrarAlerta(Response, "No hay ningún vehículo para este vendedor, o no seleccionó ninguno, por favor revise!");
        }
        protected void CargarVehiculos(Object  Sender, EventArgs e)
        {
            imglupa.Visible = true;

            string sqlVehiculos = String.Format(
                 @"SELECT MVEHICULO.mveh_inventario, MVEHICULO.mcat_vin CONCAT' - [' CONCAT MC.pcat_codigo CONCAT '] ' 
                 concat VMNIT.NOMBRE concat ' [ ' concat rtrim(char(MFACTURACLIENTE.MFAC_FACTURA)) concat ' ] ' concat PVENDEDOR.PVEN_NOMBRE 
                 FROM dbxschema.mvehiculo,dbxschema.VMNIT, dbxschema.MFACTURACLIENTE, dbxschema.MFACTURAPEDIDOVEHICULO,
                 dbxschema.MASIGNACIONVEHICULO, dbxschema.PVENDEDOR, DBXSCHEMA.MCATALOGOVEHICULO MC 
                 WHERE test_tipoesta=40 
                 AND MVEHICULO.MNIT_NIT = VMNIT.MNIT_NIT 
                 AND MFACTURACLIENTE.PVEN_CODIGO = PVENDEDOR.PVEN_CODIGO
                 AND mvehiculo.MVEH_INVENTARIO = mASIGNACIONvehiculo.MVEH_INVENTARIO 
                 AND mASIGNACIONvehiculo.PDOC_CODIGO = MFACTURAPEDIDOVEHICULO.MPED_CODIGO
                 AND mASIGNACIONvehiculo.MPED_NUMEPEDI = MFACTURAPEDIDOVEHICULO.MPED_NUMEPEDI 
                 AND MFACTURAPEDIDOVEHICULO.PDOC_CODIGO = MFACTURACLIENTE.PDOC_CODIGO 
                 AND MFACTURAPEDIDOVEHICULO.MFAC_NUMEDOCU = MFACTURACLIENTE.MFAC_NUMEDOCU 
                 AND MC.MCAT_VIN = MVEHICULO.MCat_VIN 
                 AND PVENDEDOR.PVEN_CODIGO = '" + vendedor.SelectedValue + @"'
                 ORDER BY MFACTURACLIENTE.MFAC_FACTURA, PVENDEDOR.PVEN_NOMBRE");
                Utils.FillDll(vehiculo, sqlVehiculos, true);
            //if (vehiculo.Items.Count > 0)
            //{
            //    vehiculo.Items.Insert(0, new ListItem("Seleccione...", string.Empty));
            //}
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

