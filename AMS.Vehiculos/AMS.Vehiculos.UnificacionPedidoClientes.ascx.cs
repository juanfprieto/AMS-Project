// created on 01/02/2005 at 16:20
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
using AMS.Tools;

namespace AMS.Vehiculos
{
	public partial class UnificacionPedidoClientes : System.Web.UI.UserControl
	{
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(prefijoPedidoOrigen,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu ='PC'");
                Utils.llenarPrefijos(Response, ref prefijoPedidoOrigen , "%", "%", "PC");
				//bind.PutDatasIntoDropDownList(prefijoPedidoDestino,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu ='PC'");
                Utils.llenarPrefijos(Response, ref prefijoPedidoDestino, "%", "%", "PC");
				bind.PutDatasIntoDropDownList(numeroPedidoOrigen,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoOrigen.SelectedValue.ToString()+"' AND (test_tipoesta=10)");
				bind.PutDatasIntoDropDownList(numeroPedidoDestino,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoDestino.SelectedValue.ToString()+"' AND (test_tipoesta=10 OR test_tipoesta=20)");
				//Ahora debemos cargar los datos de los clientes de origen y destino
				//origen
				this.Cargar_Datos_Origen();
				//destino
				this.Cargar_Datos_Destino();
			}
		}
		
		protected void Cambio_Tipo_Documento_Origen(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroPedidoOrigen,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoOrigen.SelectedValue.ToString()+"' AND (test_tipoesta=10)");
			this.Cargar_Datos_Origen();
		}
		
		protected void Cambio_Tipo_Documento_Destino(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(numeroPedidoDestino,"SELECT mped_numepedi FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoDestino.SelectedValue.ToString()+"' AND (test_tipoesta=10 OR test_tipoesta=20)");
			this.Cargar_Datos_Destino();
		}
		
		protected void Cambio_Numero_Documento_Origen(Object  Sender, EventArgs e)
		{
			this.Cargar_Datos_Origen();
		}
		
		protected void Cambio_Numero_Documento_Destino(Object  Sender, EventArgs e)
		{
			this.Cargar_Datos_Destino();
		}
		
		protected void Cargar_Datos_Origen()
		{
			nitPrincipalOrigen.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoOrigen.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedidoOrigen.SelectedValue.ToString()+"");
			nombrePrincipalOrigen.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitPrincipalOrigen.Text+"'");
			nitOpcionalOrigen.Text = DBFunctions.SingleData("SELECT mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoOrigen.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedidoOrigen.SelectedValue.ToString()+"");
			nombreOpcionalOrigen.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitOpcionalOrigen.Text+"'");
			catalogoOrigen.Text = DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoOrigen.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedidoOrigen.SelectedValue.ToString()+"");
		}
		
		protected void Cargar_Datos_Destino()
		{
			nitPrincipalDestino.Text = DBFunctions.SingleData("SELECT mnit_nit FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoDestino.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedidoDestino.SelectedValue.ToString()+"");
			nombreClientePrincipal.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitPrincipalDestino.Text+"'");
			nitOpcionalDestino.Text = DBFunctions.SingleData("SELECT mnit_nit2 FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoDestino.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedidoDestino.SelectedValue.ToString()+"");
			nombreOpcionalDestino.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+nitOpcionalDestino.Text+"'");
			catalogoDestino.Text = DBFunctions.SingleData("SELECT pcat_codigo FROM mpedidovehiculo WHERE pdoc_codigo='"+prefijoPedidoDestino.SelectedValue.ToString()+"' AND mped_numepedi="+numeroPedidoDestino.SelectedValue.ToString()+"");
		}
		
		protected void Unificar_Pedidos(Object  Sender, EventArgs e)
		{
			//Primero se verifican si los dos pedidos son el mismo
			if(((prefijoPedidoOrigen.SelectedValue.ToString())==(prefijoPedidoDestino.SelectedValue.ToString()))&&((numeroPedidoOrigen.SelectedItem.ToString())==(numeroPedidoDestino.SelectedItem.ToString())))
                Utils.MostrarAlerta(Response, "Pedidos Iguales. Operación No Valida");
			else
			{
				//llamamos la funcion de unificacion de pedidos de la clase PedidoCliente
				PedidoCliente pedidoActualizacion = new PedidoCliente();
				if(pedidoActualizacion.Unificar_Pedidos(prefijoPedidoOrigen.SelectedValue.ToString(),numeroPedidoOrigen.SelectedItem.ToString(), prefijoPedidoDestino.SelectedValue.ToString(), numeroPedidoDestino.SelectedItem.ToString()))
					lb.Text += "<br>Proceso realizado Satisfactoriamente, se han Unificado los pedidos ..!!! ";
				else
					lb.Text += pedidoActualizacion.ProcessMsg;
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
