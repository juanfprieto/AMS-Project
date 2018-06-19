// created on 01/09/2008
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
using AMS.Forms;
using AMS.Documentos;
using AMS.Tools;
namespace AMS.Vehiculos
{
	/// <summary>
	///		Descripción breve de AMS_Vehiculos_PedidoClienteMayor.
	/// </summary>
	public partial class AMS_Vehiculos_PedidoClienteMayor : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(tipoDocumento,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='PC'");
                Utils.llenarPrefijos(Response, ref tipoDocumento , "%", "%", "PC");
				//bind.PutDatasIntoDropDownList(tipoDocumentoEdit,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='PC'");
                Utils.llenarPrefijos(Response, ref tipoDocumentoEdit, "%", "%", "PC");
				bind.PutDatasIntoDropDownList(numeroEdicion,"SELECT mped_numepedi FROM MPEDIDOVEHICULOCLIENTEMAYOR WHERE pdoc_codigo='"+tipoDocumentoEdit.SelectedValue+"' AND TEST_TIPOESTA=10");
                if (Request.QueryString["num"] != null && Request.QueryString["pref"] != null)
                {
                    generarInforme2(Request.QueryString["pref"], Request.QueryString["num"]);
                    Utils.MostrarAlerta(Response, "Pedido " + Request.QueryString["pref"] + "-" + Request.QueryString["num"] + " creado correctamente.");
                }
			}
		}

        protected void generarInforme2(string prefijo, string numero)
        {
            FormatosDocumentos formatoRecibo = new FormatosDocumentos();
            try
            {
                formatoRecibo.Prefijo = prefijo;
                formatoRecibo.Numero = Convert.ToInt32(numero);
                formatoRecibo.Codigo = "FORSOLVEH";
                if (formatoRecibo.Cargar_Formato())
                    Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
            }
            catch
            {
                lb.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
            }
        }

		protected void Nuevo_Pedido(Object  Sender, EventArgs e)
		{
			if(tipoDocumento.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Vehiculos.CapPedCliMayor&cons="+opcion.SelectedValue+"&tipoDocu="+tipoDocumento.SelectedValue+"&tip=new&path="+Request.QueryString["path"]+"&num=");
			else
            Utils.MostrarAlerta(Response, "No hay ningún tipo de documento seleccionado");
		}
		
		protected void Cambio_Documento(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroEdicion,"SELECT mped_numepedi FROM MPEDIDOVEHICULOCLIENTEMAYOR WHERE pdoc_codigo='"+tipoDocumentoEdit.SelectedValue+"' AND TEST_TIPOESTA=10");
		}
		
		protected void Editar_Pedido(Object  Sender, EventArgs e)
		{
			if(numeroEdicion.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Vehiculos.CapPedCliMayor&cons=&tipoDocu="+tipoDocumentoEdit.SelectedValue+"&tip=old&num="+numeroEdicion.SelectedValue+"&path="+Request.QueryString["path"]);
			else
            Utils.MostrarAlerta(Response, "No hay ningún pedido seleccionado");
		}
		
		protected void Eliminar_Pedido(Object  Sender, EventArgs e)
		{
			PedidoClienteMayor miPedido = new PedidoClienteMayor();
			miPedido.PrefijoPedido = tipoDocumentoEdit.SelectedValue;
			miPedido.NumeroPedido = numeroEdicion.SelectedValue;
			if(miPedido.Eliminar_Pedido())
				Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClienteMayor&path="+Request.QueryString["path"]);
			else
			{
                Utils.MostrarAlerta(Response, "No se ha podido eliminar el pedido");
				//lb.Text = miPedido.ProcessMsg;
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
