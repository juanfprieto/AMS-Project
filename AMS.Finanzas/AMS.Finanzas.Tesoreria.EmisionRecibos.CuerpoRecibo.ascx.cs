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
using AMS.DBManager;
using AMS.Forms;
using AMS.Documentos;
using System.Configuration;
using AMS.Contabilidad;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class CuerpoRecibo : System.Web.UI.UserControl
	{
		protected string pathToControls=ConfigurationManager.AppSettings["PathToControls"];
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
        ProceHecEco contaOnline = new ProceHecEco();
	
		protected void Page_Load(object Sender,EventArgs e)
		{
			phEncabezado.Controls.Add(LoadControl(pathToControls+"AMS.Tesoreria.EmisionRecibos.Encabezados.ascx"));
			phDocumentos.Controls.Add(LoadControl(pathToControls+"AMS.Tesoreria.EmisionRecibos.Documentos.ascx"));
			phVarios.Controls.Add(LoadControl(pathToControls+"AMS.Tesoreria.EmisionRecibos.Varios.ascx"));
			phNoCausados.Controls.Add(LoadControl(pathToControls+"AMS.Tesoreria.EmisionRecibos.NoCausados.ascx"));
			phPagos.Controls.Add(LoadControl(pathToControls+"AMS.Tesoreria.EmisionRecibos.Pagos.ascx"));
			phCancelacionObligFin.Controls.Add(LoadControl(pathToControls+"AMS.Tesoreria.EmisionRecibos.Obligaciones.ascx"));
            
            if (Request.QueryString["cruce"] != null)
            {
                Control encabezado = phEncabezado.Controls[0];
                ((DropDownList)encabezado.FindControl("tipoRecibo")).Visible = false;
                ((HtmlGenericControl)encabezado.FindControl("divTipo")).Visible = false;
                ((HtmlGenericControl)encabezado.FindControl("lblFlujo")).InnerText = "Flujo de transacción:";

                Control pagos = phPagos.Controls[0];
                ((DataGrid)pagos.FindControl("gridRtns")).Visible = false;
                HtmlGenericControl pagos_GridPagos = ((HtmlGenericControl)pagos.FindControl("divflag_eliminar"));
                pagos_GridPagos.InnerText = "eliminar";
                //ddlTipPago.SelectedValue = "DC";
                //ddlTipPago.Enabled = false;
            }
			
            if(!IsPostBack)
			{
				phEncabezado.Visible=true;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnDocumentos.Enabled=false;
				btnVarios.Enabled   =false;
				btnNoCausados.Enabled=false;
				btnPagos.Enabled    =false;

                if (Request.QueryString["cruce"] != null)
                    lbEnc.Text = "Encabezado de la Nota";
                else if (Request.QueryString["tipo"] == "RC")
                    lbEnc.Text = "Encabezado del Recibo de Caja";
                else if (Request.QueryString["tipo"] == "CE")
                    lbEnc.Text = "Encabezado del Comprobante de Egreso";
                else if (Request.QueryString["tipo"] == "RP")
                    lbEnc.Text = "Encabezado del Recibo Provisional";

			}
		}
		
		protected void btnEncabezado_Click(object Sender,ImageClickEventArgs e)
		{
			if(btnEncabezado.ImageUrl=="../img/AMS.BotonExpandir.png")
			{
				phEncabezado.Visible=true;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;	
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnEncabezado.ImageUrl="../img/AMS.BotonContraer.png";
			}
			else if(btnEncabezado.ImageUrl=="../img/AMS.BotonContraer.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;	
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnEncabezado.ImageUrl="../img/AMS.BotonExpandir.png";
			}
				
		}
		
		protected void btnDocumentos_Click(object Sender,ImageClickEventArgs e)
		{
			if(btnDocumentos.ImageUrl=="../img/AMS.BotonExpandir.png")
			{
				phEncabezado.Visible=false;
				if(Session["TIPO_COMPROBANTE"].ToString().Equals("O"))
					phDocumentos.Visible=false;
				else
					phDocumentos.Visible=true;
				phVarios.Visible    =false;
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnDocumentos.ImageUrl="../img/AMS.BotonContraer.png";
			}
			else if(btnDocumentos.ImageUrl=="../img/AMS.BotonContraer.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;	
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnDocumentos.ImageUrl="../img/AMS.BotonExpandir.png";
			}
		}
		
		protected void btnVarios_Click(object Sender,ImageClickEventArgs e)
		{
			if(btnVarios.ImageUrl=="../img/AMS.BotonExpandir.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =true;
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnVarios.ImageUrl="../img/AMS.BotonContraer.png";
			}
			else if(btnVarios.ImageUrl=="../img/AMS.BotonContraer.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;	
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnVarios.ImageUrl="../img/AMS.BotonExpandir.png";
			}
		}
		
		protected void btnNoCausados_Click(object Sender,ImageClickEventArgs e)
		{
			if(btnNoCausados.ImageUrl=="../img/AMS.BotonExpandir.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;
				phNoCausados.Visible=true;
				if(Session["TIPO_COMPROBANTE"].ToString().Equals("O"))
					phCancelacionObligFin.Visible=true;
				phPagos.Visible     =false;
				btnNoCausados.ImageUrl="../img/AMS.BotonContraer.png";
			}
			else if(btnNoCausados.ImageUrl=="../img/AMS.BotonContraer.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;	
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnNoCausados.ImageUrl="../img/AMS.BotonExpandir.png";
			}
		}
		
		protected void btnPagos_Click(object Sender,ImageClickEventArgs e)
		{
			if(btnPagos.ImageUrl=="../img/AMS.BotonExpandir.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =true;
				btnPagos.ImageUrl="../img/AMS.BotonContraer.png";
			}
			else if(btnPagos.ImageUrl=="../img/AMS.BotonContraer.png")
			{
				phEncabezado.Visible=false;
				phDocumentos.Visible=false;
				phVarios.Visible    =false;	
				phNoCausados.Visible=false;
				phCancelacionObligFin.Visible=false;
				phPagos.Visible     =false;
				btnPagos.ImageUrl="../img/AMS.BotonExpandir.png";
			}
			
		}
			
		protected void Validar_DataTables(ref DataTable tablaPagar,ref DataTable tablaNC,ref DataTable tablaPagos,ref DataTable tablaRtns,ref DataTable tablaAbonos,ref DataTable tablaPostFechados,ref DataTable tablaAbonosDev)
		{
			//Función para asegurar que una tabla tenga por lo menos una fila
			if(tablaPagar!=null)
			{
				if(tablaPagar.Rows.Count==0)
					tablaPagar=null;
			}
			if(tablaNC!=null)
			{
				if(tablaNC.Rows.Count==0)
					tablaNC=null;
			}
			if(tablaPagos!=null)
			{
				if(tablaPagos.Rows.Count==0)
					tablaPagos=null;
			}
			if(tablaRtns!=null)
			{
				if(tablaRtns.Rows.Count==0)
					tablaRtns=null;
			}
			if(tablaAbonos!=null)
			{
				if(tablaAbonos.Rows.Count==0)
					tablaAbonos=null;
			}
			if(tablaPostFechados!=null)
			{
				if(tablaPostFechados.Rows.Count==0)
					tablaPostFechados=null;
			}
			if(tablaAbonosDev!=null)
			{
				if(tablaAbonosDev.Rows.Count==0)
					tablaAbonosDev=null;
			}
		}
		
		protected bool Validaciones_Especificas(string tipoRecibo,ref string error,DataTable tablaPagar,DataTable tablaNC,DataTable tablaPagos,DataTable tablaRtns,DataTable tablaAbonos,DataTable tablaPostFechados,DataTable tablaAbonosDev)
		{
			bool estado=false;
			//Si es anticipo (repuestos/taller) o anticipo general
			if(tipoRecibo=="A" || tipoRecibo=="G" || tipoRecibo=="S")
			{
				if(tablaPagos==null)
				{
					estado=true;
					error="No hay pagos";
				}
			}
			//Si es ingreso definitivo o legalizacion de provisional o egreso definitivo
			else if(tipoRecibo=="I" || tipoRecibo=="P" || tipoRecibo=="E")
			{
				if(Request.QueryString["tipo"]!="RP")
				{
					if(tablaPagar==null && tablaNC==null)
					{
						estado=true;
						error="No hay abonos a documentos ni conceptos. Revise sus datos";
					}
					else if(tablaPagos==null)
					{
						estado=true;
                        //estado=MultiView.Equals();

						error="No hay pagos";
					}
				}
				else
				{
					if(tablaPagos==null)
					{
						estado=true;
						error="No hay pagos";
					}
				}
			}
			//Si es una reconsignacion de chques devueltos
			else if(tipoRecibo=="R")
			{
				if(tablaPagar==null || tablaPagos==null)
				{
					estado=true;
					error="No hay pagos o no hay documentos, por favor revise.";
				}
			}
			//Si es un anticipo a vehiculos
			else if(tipoRecibo=="V")
			{
				int contador=0;
				for(int i=0;i<tablaAbonos.Rows.Count;i++)
				{
					if(Convert.ToBoolean(tablaAbonos.Rows[i][5])==false)
						contador++;
				}
				if(contador==tablaAbonos.Rows.Count)
				{
					estado=true;
					error="No realizó abonos o se produjo un error interno del sistema. \\nImposible crear el Recibo. Reinicie el proceso";
				}
			}
			//Si es una devolucion de pedidos
			else if(tipoRecibo=="D")
			{
				if(tablaPagos==null || tablaAbonosDev==null)
				{
					estado=true;
					error="No hay pagos o no hay pedidos";
				}
			}
			return estado;
            
		}
		
		protected void guardar_Click(object Sender,EventArgs e)
		{
			string    error="";
			FormatosDocumentos formatoRecibo=new FormatosDocumentos();
			Control   encabezado=phEncabezado.Controls[0];
			Control   pagos=phPagos.Controls[0];
			Recibo    miRecibo=new Recibo();
			DataTable tablaPagar=new DataTable();
			DataTable tablaNC=new DataTable();
			DataTable tablaPagos=new DataTable();
			DataTable tablaRtns=new DataTable();
			DataTable tablaAbonos=new DataTable();
			DataTable tablaPostFechados=new DataTable();
			DataTable tablaAbonosDev=new DataTable();
			tablaPagar=(DataTable)Session["tablaPagar"];
			tablaNC   =(DataTable)Session["tablaNC"];
			tablaPagos=(DataTable)Session["tablaPagos"];
			tablaRtns =(DataTable)Session["tablaRtns"];
			tablaAbonos=(DataTable)Session["tablaAbonos"];
			tablaPostFechados=(DataTable)Session["tablaPost"];
			tablaAbonosDev=(DataTable)Session["tablaAbonosDev"];
			this.Validar_DataTables(ref tablaPagar,ref tablaNC,ref tablaPagos,ref tablaRtns,ref tablaAbonos,ref tablaPostFechados,ref tablaAbonosDev);
			if(Validaciones_Especificas(((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue, ref error,tablaPagar,tablaNC,tablaPagos,tablaRtns,tablaAbonos,tablaPostFechados,tablaAbonosDev))
                Utils.MostrarAlerta(Response, ""+error+"");
			else
			{
				if(Request.QueryString["tipo"]=="RC")
					miRecibo = new Recibo(tablaPagar,tablaNC,tablaPagos,tablaRtns,tablaAbonos,tablaPostFechados);
				else if(Request.QueryString["tipo"]=="CE")
					miRecibo = new Recibo(tablaPagar,tablaNC,tablaPagos,tablaRtns,tablaAbonosDev);
				else if(Request.QueryString["tipo"]=="RP")
					miRecibo = new Recibo(tablaPagos);
				miRecibo.PrefijoRecibo=((DropDownList)encabezado.FindControl("prefijoRecibo")).SelectedValue;
				miRecibo.NumeroRecibo=Convert.ToInt32(((TextBox)encabezado.FindControl("numeroRecibo")).Text);
				miRecibo.Fecha=((TextBox)encabezado.FindControl("fecha")).Text;
				miRecibo.TipoRecibo=((DropDownList)encabezado.FindControl("tipoRecibo")).SelectedValue;
				miRecibo.DatCli=((TextBox)encabezado.FindControl("datCli")).Text;
				miRecibo.DatBen=((TextBox)encabezado.FindControl("datBen")).Text;
				miRecibo.Concepto=((TextBox)encabezado.FindControl("concepto")).Text;
				miRecibo.ValorBruto=Convert.ToDouble(((TextBox)pagos.FindControl("valorBruto")).Text.Substring(1));
				miRecibo.ValorAbonado=Convert.ToDouble(((TextBox)pagos.FindControl("valorBruto")).Text.Substring(1));
				miRecibo.ValorNeto=Convert.ToDouble(((TextBox)pagos.FindControl("valorNeto")).Text.Substring(1));
				miRecibo.Almacen=((DropDownList)encabezado.FindControl("almacen")).SelectedValue;
				miRecibo.Usuario=HttpContext.Current.User.Identity.Name.ToString();
				miRecibo.FechaCreacion=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				miRecibo.ClaseRecibo=Request.QueryString["tipo"];
				miRecibo.PrefijoFactura=((DropDownList)pagos.FindControl("ddlPrefijoFactura")).SelectedValue;
				miRecibo.PrefijoNotaCliente=((DropDownList)pagos.FindControl("ddlPrefNot")).SelectedValue;
				miRecibo.PrefijoNotaProveedor=((DropDownList)pagos.FindControl("ddlPrefNotPro")).SelectedValue;
				miRecibo.SqlPagosDev=(ArrayList)Session["pagosDev"];
				miRecibo.FlujoEspecifico=(((DropDownList)encabezado.FindControl("ddlFlujo")).SelectedValue.Split('-'))[1].ToString();
				miRecibo.FlujoGeneral=(((DropDownList)encabezado.FindControl("ddlFlujo")).SelectedValue.Split('-'))[0].ToString();

                if ((Request.QueryString["tipo"] == "RC" || (Request.QueryString["tipo"] == "RP")) && (miRecibo.PrefijoNotaCliente == "" || miRecibo.PrefijoNotaCliente == null))
                {
                    Utils.MostrarAlerta(Response, "Debe seleccionar (parametrizar) un prefijo de Nota Cliente antes de grabar.");
                    return;
                }

                if (Request.QueryString["tipo"]=="CE" && (miRecibo.PrefijoNotaProveedor == "" || miRecibo.PrefijoNotaProveedor == null))
                {
                    Utils.MostrarAlerta(Response, "Debe seleccionar (parametrizar) un prefijo de Nota Proveedor antes de grabar.");
                    return;
                }

				//Validaciones: Causación Obligaciones
				if(Session["TIPO_COMPROBANTE"].ToString().Equals("O"))
				{
					double totalO=Convert.ToDouble(Session["TOT_OBLIGACION"]);
					miRecibo.Obligaciones=totalO;
					if(Math.Abs(miRecibo.ValorBruto-miRecibo.Obligaciones)!=0.0) // hector diferencia = ceros
					{
                        Utils.MostrarAlerta(Response, "El valor bruto no es igual al total de las obligaciones");
						return;
					}
					miRecibo.dtObligaciones=(DataTable)Session["DT_OBLIGACIONES"];
				}
				if(miRecibo.Guardar_Recibo())
				{
                    // contabilización ON LINE
                    contaOnline.contabilizarOnline(miRecibo.PrefijoRecibo.ToString(), Convert.ToInt32(miRecibo.NumeroRecibo.ToString()), Convert.ToDateTime(miRecibo.Fecha), "");
                    
                    lb.Text += miRecibo.Mensajes;
					Session.Clear();
                    string cruce = "";
                    if (Request.QueryString["cruce"] != null)
                    {
                        cruce = "&cruce=1";
                    }
					if(Request.QueryString["cnd"]!=null && Request.QueryString["cnd"]=="1")
                        Response.Redirect(indexPage + "?process=Tesoreria.EmisionRecibos&cas=1&pre=" + miRecibo.PrefijoRecibo + "&num=" + miRecibo.NumeroRecibo + "&tip=" + Request.QueryString["tipo"] + cruce);
					else
                        Response.Redirect(indexPage + "?process=Tesoreria.EmisionRecibos&pre=" + miRecibo.PrefijoRecibo + "&num=" + miRecibo.NumeroRecibo + "&tip=" + Request.QueryString["tipo"] + cruce);
				}
				else
					lb.Text+=miRecibo.Mensajes;
			}
		}
		
		protected void cancelar_click(object Sender,EventArgs e)
		{
			Session.Clear();
			if(Request.QueryString["cnd"]!=null && Request.QueryString["cnd"]=="1")
				Response.Redirect(indexPage+"?process=Tesoreria.EmisionRecibos&cas=1");
			else
				Response.Redirect(indexPage+"?process=Tesoreria.EmisionRecibos");
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
