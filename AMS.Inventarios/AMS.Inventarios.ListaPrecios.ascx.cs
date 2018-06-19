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
using AMS.Tools;


namespace AMS.Inventarios
{
	public partial class AdminListasPrecios : System.Web.UI.UserControl 
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlListasEdit,"SELECT ppre_codigo, ppre_nombre FROM pprecioitem");
				bind.PutDatasIntoDropDownList(ddlListasDelete,"SELECT ppre_codigo, ppre_nombre FROM pprecioitem");
				bind.PutDatasIntoDropDownList(ddlListasFile,"SELECT ppre_codigo, ppre_nombre FROM pprecioitem");

                if (Request.QueryString["hecho"] == "1")
                {
                    Utils.MostrarAlerta(Response, "Información Actualizada!!");
                }
			}
		}
		
		protected void IngresarNuevo(Object Sender, EventArgs E)
		{
			Response.Redirect(""+indexPage+"?process=Inventarios.NuevaLista");
		}
		
		protected void AsignarPrecios(Object Sender, EventArgs E)
		{
			if(this.ddlListasEdit.Items.Count>0)
				Response.Redirect(""+indexPage+"?process=Inventarios.AsignarValoresLista1&codLista="+this.ddlListasEdit.SelectedValue+"");
			else
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna lista para su actualización !");
		}
		
		protected void IngresarArchivo(Object Sender, EventArgs E)
		{
			if(this.ddlListasFile.Items.Count>0)
				Response.Redirect(""+indexPage+"?process=Inventarios.ArchivoLista&codLista="+this.ddlListasFile.SelectedValue+"");
			else
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna lista para su actualización !");
		}
		
		protected void EliminarLista(Object Sender, EventArgs E)
		{
			if(this.ddlListasDelete.Items.Count>0)
			{
				ListaPrecios listaBorrar = new ListaPrecios(this.ddlListasDelete.SelectedValue);
				if(listaBorrar.EliminarLista())
					Response.Redirect(""+indexPage+"?process=Inventarios.ListaPrecios");
				else
					lb.Text += listaBorrar.ProcessMsg;
			}
			else
                Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna lista para su eliminación!");
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
