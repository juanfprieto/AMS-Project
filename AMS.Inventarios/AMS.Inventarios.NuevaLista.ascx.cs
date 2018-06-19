using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class NuevaLista : System.Web.UI.UserControl 
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(this.ddlMoneda,"SELECT pmon_moneda, pmon_nombre from pmoneda");
			}
		}
		
		protected void CrearLista(Object Sender, EventArgs E)
		{
			//Creamos un objeto de tipo ListaPrecios y creamos la lista de precios
			ListaPrecios nuevaLista = new ListaPrecios(this.tbCodigoLista.Text,this.tbNombreLista.Text,this.ddlMoneda.SelectedValue);
			if(nuevaLista.CrearLista())
				Response.Redirect(""+indexPage+"?process=Inventarios.ListaPrecios");
			else
                Utils.MostrarAlerta(Response, "" + nuevaLista.ProcessMsg + "");
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
