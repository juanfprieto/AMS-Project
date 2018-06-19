using System;
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
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial  class ReImpresionRecibos : System.Web.UI.UserControl
	{
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlPrefijo,"SELECT pdoc_codigo,pdoc_descripcion FROM dbxschema.pdocumento WHERE tdoc_tipodocu IN('RC','CE','RP') ORDER BY tdoc_tipodocu");
				bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mcaj_numero FROM dbxschema.mcaja WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
			}
		}
		
		protected void ddlPrefijo_IndexChanged(object Sender,EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumero,"SELECT mcaj_numero FROM dbxschema.mcaja WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
		}
		
		protected void btnAceptar_Click(object Sender,EventArgs e)
		{
			if(ddlPrefijo.Items.Count==0 || ddlNumero.Items.Count==0)
                Utils.MostrarAlerta(Response, "No hay recibos o comprobantes");
			else
			{
				string tipo=DBFunctions.SingleData("SELECT tcla_claserecaja FROM dbxschema.mcaja WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"' AND mcaj_numero="+ddlNumero.SelectedValue+"");
				Response.Write("<script language:javascript>w=window.open('AMS.Tesoreria.ImpresionRC.aspx?pref="+ddlPrefijo.SelectedValue+"&num="+ddlNumero.SelectedValue+"&tipo="+tipo+"');</script>");
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
