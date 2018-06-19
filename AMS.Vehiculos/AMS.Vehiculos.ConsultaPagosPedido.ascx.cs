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

namespace AMS.Vehiculos
{
	public partial class ConsultaPagosPedido : System.Web.UI.UserControl
	{
		protected string prefijoPedidoInfo, numeroPedidoInfo;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			prefijoPedidoInfo = Request.QueryString["prefPed"];
			numeroPedidoInfo = Request.QueryString["numPed"];
			PedidoCliente infoPedido = new PedidoCliente(prefijoPedidoInfo,numeroPedidoInfo);
			prefijoPedido.Text = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+prefijoPedidoInfo+"'");
			numeroPedido.Text = numeroPedidoInfo;
			catalogo.Text = infoPedido.Catalogo;
			colorPrimario.Text = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo='"+infoPedido.ColorPrimario+"'");
			colorOpcional.Text = DBFunctions.SingleData("SELECT pcol_descripcion FROM pcolor WHERE pcol_codigo='"+infoPedido.ColorOpcional+"'");
			claseVehiculo.Text = DBFunctions.SingleData("SELECT tcla_nombre FROM tclasevehiculo WHERE tcla_clase='"+infoPedido.ClaseVehiculo+"'");
			anoModelo.Text = infoPedido.AnoModelo;
			fechaPedido.Text = infoPedido.FechaPedido;
			fechaEntrega.Text = infoPedido.FechaProgEntrega;
			lbVendedor.Text = DBFunctions.SingleData("SELECT pven_nombre FROM pvendedor WHERE pven_codigo='"+infoPedido.Vendedor+"'");
            sedeVenta.Text = DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen=(SELECT palm_almacen FROM pvendedor WHERE pven_codigo='" + infoPedido.Vendedor + "')");
			nitPrincipal.Text = infoPedido.Nit;
			nombrePrincipal.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+infoPedido.Nit+"'");
			nitAlterno.Text = infoPedido.NitOpcional;
			nombreAlterno.Text = DBFunctions.SingleData("SELECT mnit_apellidos CONCAT ' ' CONCAT mnit_nombres FROM mnit WHERE mnit_nit='"+infoPedido.NitOpcional+"'");
			MostrarAnticipos(infoPedido.TablaAnticipos);
			MostrarRetoma(infoPedido.VehiculosRetoma);
		}
		
		protected void MostrarAnticipos(DataTable dtAnticipos)
		{
			dgAnticipos.DataSource = dtAnticipos;
			dgAnticipos.DataBind();
			DatasToControls.JustificacionGrilla(dgAnticipos,dtAnticipos);
		}
		
		protected void MostrarRetoma(DataTable dtRetomas)
		{
			dgRetoma.DataSource = dtRetomas;
			dgRetoma.DataBind();
			DatasToControls.JustificacionGrilla(dgRetoma,dtRetomas);
		}
		
		protected void VolverOrigen(Object  Sender, EventArgs e)
		{
			if(Request.QueryString["Orig"] == "1")
				Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientes");
			else if(Request.QueryString["Orig"] == "2")
				Response.Redirect("" + indexPage + "?process=Vehiculos.PedidoClientesFormulario&proc=modi&pref="+prefijoPedidoInfo+"&nume="+numeroPedidoInfo+"&orig=C");
		}
		
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
