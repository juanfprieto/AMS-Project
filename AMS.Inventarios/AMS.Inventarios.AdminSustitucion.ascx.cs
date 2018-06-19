using System;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class AdminSustitucion : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
                Utils.llenarPrefijos(Response, ref ddlPrefDocu, "%", "%", "AS");
				tbNumDocu.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu +1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefDocu.SelectedValue+"'");
			}
		}
		
		protected void CambioDocumento(object Sender, EventArgs E)
		{
			tbNumDocu.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu +1 FROM pdocumento WHERE pdoc_codigo='"+ddlPrefDocu.SelectedValue+"'");
		}
		
		protected void CrearSustitucion(object Sender, EventArgs E)
		{
			if(ddlPrefDocu.Items.Count == 0)
			{
				
                Utils.MostrarAlerta(Response, "No se ha seleccionado ningun prefijo de documento de sustitucion.\\nRevise Por Favor.");
				return;
			}
			//Revisamos si ya existe una sustitucion con este prefijo y numero
			if(DBFunctions.RecordExist("SELECT * FROM msustitucion WHERE pdoc_codigo='"+ddlPrefDocu.SelectedValue+"' AND msus_numero="+tbNumDocu.Text+""))
			{
                Utils.MostrarAlerta(Response, "Ya existe una sustitucion con prefijo " + ddlPrefDocu.SelectedItem.Text + " y numero " + tbNumDocu.Text + ".\\nRevise Por favor.!");
				return;
			}
			Response.Redirect(indexPage + "?process=Inventarios.Sustitucion&tipPr=N&prefDoc="+ddlPrefDocu.SelectedValue+"&numDoc="+tbNumDocu.Text+"");
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
