// created on 16/03/2005 at 15:35
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
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class CPedidoProveedor : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(tipoDocumento,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='PV'");
                Utils.llenarPrefijos(Response, ref tipoDocumento, "%", "%", "PV");
				//bind.PutDatasIntoDropDownList(tipoDocumentoEdit,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='PV'");
                Utils.llenarPrefijos(Response, ref tipoDocumentoEdit, "%", "%", "PV");
				bind.PutDatasIntoDropDownList(numeroEdicion,"SELECT mped_numepedi FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+tipoDocumentoEdit.SelectedValue+"'");
			}
		}
		
		protected void Nuevo_Pedido(Object  Sender, EventArgs e)
		{
			if(tipoDocumento.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Vehiculos.CapPedPro&cons="+opcion.SelectedValue+"&tipoDocu="+tipoDocumento.SelectedValue+"&tip=new&num=");
			else
            Utils.MostrarAlerta(Response, "No hay ningún tipo de documento seleccionado");
		}
		
		protected void Cambio_Documento(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroEdicion,"SELECT mped_numepedi FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+tipoDocumentoEdit.SelectedValue+"'");
		}
		
		protected void Editar_Pedido(Object  Sender, EventArgs e)
		{
			if(numeroEdicion.Items.Count>0)
				Response.Redirect("" + indexPage + "?process=Vehiculos.CapPedPro&cons=&tipoDocu="+tipoDocumentoEdit.SelectedValue+"&tip=old&num="+numeroEdicion.SelectedValue+"");
			else
            Utils.MostrarAlerta(Response, "No hay ningún pedido seleccionado");
		}
		
		protected void Eliminar_Pedido(Object  Sender, EventArgs e)
		{
			PedidoProveedor miPedido = new PedidoProveedor();
			miPedido.PrefijoPedido = tipoDocumentoEdit.SelectedValue;
			miPedido.NumeroPedido = numeroEdicion.SelectedValue;
			if(miPedido.Eliminar_Pedido())
				Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoProveedor");
			else
			{
                Utils.MostrarAlerta(Response, "No se ha podido eliminar el pedido");
				//lb.Text = miPedido.ProcessMsg;
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

