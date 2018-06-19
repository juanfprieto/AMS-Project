// created on 02/02/2005 at 17:46
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
using AMS.Documentos;
using AMS.Forms;
using AMS.CriptoServiceProvider;
using AMS.Tools;
using Ajax;

namespace AMS.Vehiculos
{
	public partial class AsignacionFacturacionVehiculos : System.Web.UI.UserControl
	{

		#region Atributos
		protected DropDownList   prefijoPedidoOtro, numeroPedidoOtro, prefijoDevoluciones;
		protected System.Web.UI.WebControls.Button btnDevPed;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private FormatosDocumentos formatoRecibo=new FormatosDocumentos();
        protected string pathToControls = ConfigurationManager.AppSettings["PathToControls"];
		//JFSC 11022008 Poner en comentario
		//string ConString;
		string path=System.Configuration.ConfigurationManager.AppSettings["PathToPreviews"];
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack)
			{
                DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(prefijoPedido,string.Format(Documento.DOCUMENTOSTIPO,"PC"));
				bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue.ToString()+"' AND (test_tipoesta=20) ORDER BY mped_numepedi");
                //bind.PutDatasIntoDropDownList(prefijoPedidoOtro,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu ='PC'");
                //bind.PutDatasIntoDropDownList(numeroPedidoOtro,"SELECT MPV.mped_numepedi FROM mpedidovehiculo MPV, masignacionvehiculo MAV, mvehiculo MVH WHERE MPV.test_tipoesta=30 AND MPV.pdoc_codigo='"+prefijoPedidoOtro.SelectedValue+"' AND MAV.pdoc_codigo=MPV.pdoc_codigo AND MAV.mped_numepedi = MPV.mped_numepedi AND MAV.mveh_inventario = MVH.mveh_inventario AND MVH.test_tipoesta <> 60");
                //bind.PutDatasIntoDropDownList(prefijoDevoluciones,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='NC'");
                if (Request.QueryString["facVeh"] != null)
				{
					DBFunctions.NonQuery("DROP VIEW DBXSCHEMA.VVEHICULOS_FACTGASTOVEHICULO");
					string numInventario=DBFunctions.SingleData("Select numinventario from DBXSCHEMA.VVEHICULOS_FACTURADEVENTA where pre_fac='"+Request.QueryString["prefFC"]+"' and nume_fac="+Request.QueryString["numFC"]+" ");
					string catalogoVeh=DBFunctions.SingleData("Select tipo from DBXSCHEMA.VVEHICULOS_FACTURADEVENTA where pre_fac='"+Request.QueryString["prefFC"]+"' and nume_fac="+Request.QueryString["numFC"]+"");
					DBFunctions.NonQuery("CREATE VIEW DBXSCHEMA.VVEHICULOS_FACTGASTOVEHICULO (GASTOS,ANOTALLER,ANOMODELO,PRECIO_TOTAL,VALOR_COMPRA,IVACATALOGO,CLASE)AS SELECT (select sum(dfac_valorgasto) AS GASTOS from DBXSCHEMA.DFACTURAGASTOVEHICULO where mveh_inventario="+numInventario+"),(SELECT pano_ano AS ANOTALLER from DBXSCHEMA.CVEHICULOS),(SELECT ANO AS ANOMODELO FROM DBXSCHEMA.VVEHICULOS_FACTURADEVENTA WHERE PRE_FAC='"+Request.QueryString["prefFC"]+"' AND NUME_FAC="+Request.QueryString["numFC"]+"),(SELECT precio_total FROM DBXSCHEMA.VVEHICULOS_FACTURADEVENTA WHERE PRE_FAC='"+Request.QueryString["prefFC"]+"' AND NUME_FAC="+Request.QueryString["numFC"]+"),(select mveh_valocomp from DBXSCHEMA.MVEHICULO where mveh_inventario="+numInventario+"), piva_porciva,(select clase2 FROM DBXSCHEMA.VVEHICULOS_FACTURADEVENTA WHERE PRE_FAC='"+Request.QueryString["prefFC"]+"' AND NUME_FAC="+Request.QueryString["numFC"]+") FROM dbxschema.pcatalogovehiculo WHERE pcat_codigo='"+catalogoVeh+"'");

                    //Confirmacion de creacion de Nota a favor del cliente
                    if (Request.QueryString["numNotaFavor"] != null && Request.QueryString["numNotaFavor"] != "")
                        Response.Write("<script language:javascript>alert('Se ha creado la Nota a favor del cliente por devolución con prefijo " + Request.QueryString["prefNotaFavor"] + " y el número " + Request.QueryString["numNotaFavor"] + ".');</script>");
						
                    if(Request.QueryString["facVeh"] == "0")
					{
                        Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta de vehiculo con prefijo " + Request.QueryString["prefFC"] + " y el número " + Request.QueryString["numFC"] + ".");
                        if (Request.QueryString["prefAcce"] != "" && Request.QueryString["prefAcce"] != null && Request.QueryString["numAcce"] != "0")
                            Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta de accesorios con prefijo " + Request.QueryString["prefAcce"] + " y el número " + Request.QueryString["numAcce"] + ".");
                        if (Request.QueryString["codTramite"] != null && Request.QueryString["numTramite"] != null)
                            Utils.MostrarAlerta(Response, "se ha creado la orden de tramite con preijo " + Request.QueryString["codtramite"] + "y numero " + Request.QueryString["Numtramite"] + "");
                        try
						{
							formatoRecibo.Prefijo=Request.QueryString["prefFC"];
							formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFC"]);
							formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFC"]+"'");
							if(formatoRecibo.Codigo!=string.Empty)
							{
								if(formatoRecibo.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
							}

                            formatoRecibo.Prefijo = Request.QueryString["prefFC"];
                            formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numFC"]);
                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefFC"] + "'");
                            if (formatoRecibo.Codigo != string.Empty)
                            {
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=700');</script>");
                            }

                            if (Request.QueryString["prefAcce"] != "0")
                            {
                                formatoRecibo.Prefijo = Request.QueryString["prefAcce"];
                                formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numAcce"]);
                                formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefAcce"] + "'");
                                if (formatoRecibo.Codigo != string.Empty)
                                {
                                    if (formatoRecibo.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                                }
                            }
						}
						catch
						{
							lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
						}
					}
					else if(Request.QueryString["facVeh"] == "1")
					{
					//	Response.Write("<script language:javascript>alert('Se ha creado la factura de cliente por venta de vehiculo con prefijo "+Request.QueryString["prefFC"]+" y el número "+Request.QueryString["numFC"]+".\\nSe ha creado la factura de financiera por venta de vehiculo con prefijo "+Request.QueryString["prefFF"]+" y número "+Request.QueryString["numFF"]+"');</script>");
                        Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta de vehiculo con prefijo " + Request.QueryString["prefFC"] + " y el número " + Request.QueryString["numFC"] + ".");
                        if (Request.QueryString["prefAcce"] != "" && Request.QueryString["prefAcce"] != null && Request.QueryString["numAcce"] != "0")
                            Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta de accesorios con prefijo " + Request.QueryString["prefAcce"] + " y el número " + Request.QueryString["numAcce"] + ".");
                        try
						{
							formatoRecibo.Prefijo=Request.QueryString["prefFC"];
							formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFC"]);
							formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFC"]+"'");
							if(formatoRecibo.Codigo!=string.Empty)
							{
								if(formatoRecibo.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
							}
                            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefFC"] + "'");
                            if (formatoRecibo.Codigo != string.Empty)
                            {
                                if (formatoRecibo.Cargar_Formato())
                                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=700');</script>");
                            }
							
                            formatoRecibo.Prefijo=Request.QueryString["prefFF"];
							formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFF"]);
							formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFF"]+"'");
							if(formatoRecibo.Codigo!=string.Empty)
							{
								if(formatoRecibo.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
							}

                            if (Request.QueryString["prefAcce"] != "0")
                            {
                                formatoRecibo.Prefijo = Request.QueryString["prefAcce"];
                                formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numAcce"]);
                                formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefAcce"] + "'");
                                if (formatoRecibo.Codigo != string.Empty)
                                {
                                    if (formatoRecibo.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                                }
                            }
						}
						catch
						{
							lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
						}
					}
						
					else if(Request.QueryString["facVeh"] == "2")
					{
                        Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta de vehiculo con prefijo " + Request.QueryString["prefFC"] + " y el número " + Request.QueryString["numFC"] + ".\\nSe ha creado el Inventario de Consignacion vehiculos por venta de vehiculo con prefijo " + Request.QueryString["prefFR"] + " y número " + Request.QueryString["numFR"] + "");
                        if (Request.QueryString["prefAcce"] != "" && Request.QueryString["prefAcce"] != null && Request.QueryString["numAcce"] != "0")
                            Utils.MostrarAlerta(Response, "Se ha creado la factura de cliente por venta de accesorios con prefijo " + Request.QueryString["prefAcce"] + " y el número " + Request.QueryString["numAcce"] + ".");
                        try
						{
							formatoRecibo.Prefijo=Request.QueryString["prefFC"];
							formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFC"]);
							formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFC"]+"'");
							if(formatoRecibo.Codigo!=string.Empty)
							{
								if(formatoRecibo.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=550');</script>");
							}
							formatoRecibo.Prefijo=Request.QueryString["prefFR"];
							formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["numFR"]);
							formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefFR"]+"'");
							if(formatoRecibo.Codigo!=string.Empty)
							{
								if(formatoRecibo.Cargar_Formato())
									Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=500');</script>");
							}

                            if (Request.QueryString["prefAcce"] != "0")
                            {
                                formatoRecibo.Prefijo = Request.QueryString["prefAcce"];
                                formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numAcce"]);
                                formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefAcce"] + "'");
                                if (formatoRecibo.Codigo != string.Empty)
                                {
                                    if (formatoRecibo.Cargar_Formato())
                                        Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=450');</script>");
                                }
                            }
						}
						catch
						{
							lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
						}
					}
				}
                if (Request.QueryString["proceso"] == "Factura")
                    fldAsignacion.Visible = false;
                else if (Request.QueryString["proceso"] == "Asigna")
                    fldFacturacion.Visible = false;

            }
            
        }

		protected void Ingresar_Proceso(Object  Sender, EventArgs e)
		{
            //System.Threading.Thread.Sleep(1000);
            //
			if(tipoProcesoAsignar.SelectedValue.ToString()=="A")
				Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionVehiculos&acci=A");
			else if(tipoProcesoAsignar.SelectedValue.ToString()=="D")
				Response.Redirect("" + indexPage + "?process=Vehiculos.AsignacionVehiculos&acci=D");
		}
		
		protected void Cambio_Tipo_Documento(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroPedido,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedido.SelectedValue+ "' AND (test_tipoesta=20) ORDER BY mped_numepedi");
		}
		
		protected void Cambio_Tipo_Documento_Otros(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroPedidoOtro,"SELECT MPV.mped_numepedi FROM mpedidovehiculo MPV, masignacionvehiculo MAV, mvehiculo MVH WHERE MPV.test_tipoesta=30 AND MPV.pdoc_codigo='"+prefijoPedidoOtro.SelectedValue.ToString()+ "' AND MAV.pdoc_codigo=MPV.pdoc_codigo AND MAV.mped_numepedi = MPV.mped_numepedi AND MAV.mveh_inventario = MVH.mveh_inventario AND MVH.test_tipoesta <> 60 ORDER BY mped_numepedi");
		}
		
        /*
		protected void RealizarDevolucion(Object  Sender, EventArgs e)
		{
			if(numeroPedidoOtro.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado un pedido para realizar la devolución.\\nRevise Por Favor.");
				return;
			}
			if(prefijoDevoluciones.Items.Count == 0)
			{
                Utils.MostrarAlerta(Response, "No se ha seleccionado un prefijo para la nota de devolución.\\nRevise Por Favor.");
				return;
			}
            PedidoCliente pedidoDevolucion = new PedidoCliente(prefijoPedidoOtro.SelectedValue, numeroPedidoOtro.SelectedValue, prefijoDevoluciones.SelectedValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			if(pedidoDevolucion.RealizarDevolucion())
				lb.Text += pedidoDevolucion.ProcessMsg;
			else
				lb.Text += pedidoDevolucion.ProcessMsg;
		}
        */
		
		protected void Facturar_Pedido(Object  Sender, EventArgs e)
		{
            if (numeroPedido.Items.Count == 0)
                Utils.MostrarAlerta(Response, "El número de pedido es vacio, Revise Por Favor");
            else if (DBFunctions.RecordExist("select mv.mveh_inventario from mvehiculo mv, masignacionvehiculo ma where mv.mveh_inventario = ma.mveh_inventario and ma.pdoc_codigo = '" + prefijoPedido.SelectedValue + "' and ma.mped_numepedi = " + numeroPedido.SelectedValue + " and mv.test_tipoesta <> 30 "))
                    Utils.MostrarAlerta(Response, "El vehículo asignado para este pedido NO está en estado asignado, NO se puede facturar, investigue ese vehículo ... !!");
                 else
                 {
                     //Vamos a redireccionar al control de usuario que se encarga de facturar
                     Response.Redirect("" + indexPage + "?process=Vehiculos.FacturacionPedido&prefP=" + prefijoPedido.SelectedValue + "&numP=" + numeroPedido.SelectedValue + " ");
                 }
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

