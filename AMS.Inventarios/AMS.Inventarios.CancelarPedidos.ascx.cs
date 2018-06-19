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

namespace AMS.Inventarios
{	
	public partial class CancelarPedidos : System.Web.UI.UserControl 
	{
		protected string Tipo;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			Tipo = Request.QueryString["actor"];
			if(!IsPostBack)
			{
				if(Tipo=="C")
				{
					bind.PutDatasIntoDropDownList(this.ddlCodPedido,"SELECT pped_codigo, pped_nombre FROM ppedido ORDER BY pped_nombre");
					bind.PutDatasIntoDropDownList(this.ddlNumPedido,"SELECT DISTINCT mped_numepedi FROM dpedidoitem WHERE pped_codigo = '"+ddlCodPedido.SelectedValue+"' AND mped_clasregi = '"+Tipo+"' ORDER BY mped_numepedi DESC");
				}
				else if(Tipo=="P")
				{
					bind.PutDatasIntoDropDownList(this.ddlCodPedido,"SELECT pped_codigo, pped_nombre FROM ppedido ORDER BY pped_nombre");
					bind.PutDatasIntoDropDownList(this.ddlNumPedido,"SELECT DISTINCT mped_numepedi FROM dbxschema.dpedidoitem WHERE mped_clasregi='"+Tipo+"' AND pped_codigo = '"+ddlCodPedido.SelectedValue+"' ORDER BY mped_numepedi DESC");
					plListasEmpaque.Visible = false;
				}
				if(Request.QueryString["com"]!=null)
					if(Request.QueryString["com"].Equals("1"))
                        Utils.MostrarAlerta(Response, "El pedido no se puede borrar completamente porque tiene transferencias o items en una lista de empaque!");
				lbTipPedido.Text = DBFunctions.SingleData("SELECT tped_nombre FROM tpedido WHERE tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+ddlCodPedido.SelectedValue+"')");
			}
			this.ConsultarLista();
		}
		
		protected void CambioPrefijo(Object Sender, EventArgs E)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(this.ddlNumPedido,"SELECT DISTINCT mped_numepedi FROM dpedidoitem WHERE pped_codigo = '"+ddlCodPedido.SelectedValue+"' ORDER BY mped_numepedi DESC");
			lbTipPedido.Text = DBFunctions.SingleData("SELECT tped_nombre FROM tpedido WHERE tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='"+ddlCodPedido.SelectedValue+"')");
			this.ConsultarLista();
		}
		
		protected void CambioNumero(Object Sender, EventArgs E)
		{
			this.ConsultarLista();
		}
		
		protected void CancelarPedido(Object Sender, EventArgs E)
		{
			if(ddlNumPedido.Items.Count>0)
			{
				PedidoFactura cancelarPedido = new PedidoFactura(this.ddlCodPedido.SelectedValue,Convert.ToUInt32(this.ddlNumPedido.SelectedValue),Tipo);
				if(cancelarPedido.EliminarPed())
				{
					string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
					Response.Redirect("" + indexPage + "?process=Inventarios.CancelarPedidos&actor="+Tipo+"&com="+(cancelarPedido.tieneDatos?"1":"0"));
					//lb.Text += "<br>BIEN : "+cancelarPedido.ProcessMsg;
				}
				else
					lb.Text += "<br>Error : "+cancelarPedido.ProcessMsg;
			}
			else

                Utils.MostrarAlerta(Response, "No se ha seleccionado ningun numero de pedido!");
		}
		
		protected void ConsultarLista()
		{
			if(ddlNumPedido.Items.Count>0)
			{
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT DISTINCT mlis_numero AS LISTAPRECIO FROM dlistaempaque WHERE pped_codigo='"+ddlCodPedido.SelectedValue+"' AND mped_numepedi="+ddlNumPedido.SelectedValue+"");
				this.dgListas.DataSource = ds.Tables[0];
				this.dgListas.DataBind();
			}
			else
			{
				this.dgListas.DataSource = null;
				this.dgListas.DataBind();
			}
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
