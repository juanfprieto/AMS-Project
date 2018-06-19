using System;
using System.Data.Common;
using System.Configuration;
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
using AMS.Forms;

namespace AMS.Reportes
{
	public partial class Menu : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(opcionesMenu,"SELECT smen_opcion FROM smenu WHERE smen_padre IN (SELECT smen_opcion FROM smenu WHERE smen_padre='ND') AND smen_padre <> 'AMS'");
			}
		}
		
		protected void Agregar_OpcionMenu(Object  Sender, EventArgs e)
		{
			ArrayList sqlStrings = new ArrayList();
			int numeroOpcion = System.Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(smen_codigo) FROM smenu"))+1;
			sqlStrings.Add("INSERT INTO smenu VALUES("+numeroOpcion.ToString()+",'"+DBFunctions.SingleData("SELECT srep_nombre FROM sreporte WHERE srep_id="+Request.QueryString["idReporte"]+"")+"','"+opcionesMenu.SelectedValue+"','AMS.Web.index.aspx?process=Reportes.FormatoReporte&idReporte="+Request.QueryString["idReporte"]+"')");
			sqlStrings.Add("INSERT INTO smenureporte VALUES("+numeroOpcion.ToString()+","+Request.QueryString["idReporte"]+")");
			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect("" + indexPage + "?process=Reportes.AdminReports");
			else
				lb.Text = DBFunctions.exceptions;
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
