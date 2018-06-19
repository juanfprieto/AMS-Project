using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;


namespace AMS.Inventarios
{
	public partial class ParametrosValorLista : System.Web.UI.UserControl 
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				LlenarDdlValorBase(this.ddlValorBase);
				LLenarDdlTipoOper(this.ddlTipoOper);
			}
		}
		
		protected void AceptarParametro(Object Sender, EventArgs E)
		{
			Response.Redirect(""+indexPage+"?process=Inventarios.AsignarValoresLista2&codLista="+Request.QueryString["codLista"]+"&valBase="+this.ddlValorBase.SelectedValue+"&tipOper="+this.ddlTipoOper.SelectedValue+"&valOper="+Convert.ToDouble(this.tbValorModificacion.Text).ToString()+"");
		}
		
		protected void LlenarDdlValorBase(DropDownList ddl)
		{
			ddl.Items.Clear();
			ddl.Items.Add(new ListItem("Costo Promedio","MSALDOITEM-MSAL_COSTPROM-0"));
			ddl.Items.Add(new ListItem("Ultimo Precio Proveedor","MITEMS-MITE_COSTREPO-1"));
			ddl.Items.Add(new ListItem("Precio de la Lista de Precios","MPRECIOITEM-MPRE_PRECIO-2"));
		}
		
		protected void LLenarDdlTipoOper(DropDownList ddl)
		{
			ddl.Items.Clear();
			ddl.Items.Add(new ListItem("Aumentar en un Porcentaje","@,%"));
			ddl.Items.Add(new ListItem("Disminuir en un Porcentaje","-,%"));
			ddl.Items.Add(new ListItem("Aumentar un valor","@"));
			ddl.Items.Add(new ListItem("Disminuir un valor","-"));
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
