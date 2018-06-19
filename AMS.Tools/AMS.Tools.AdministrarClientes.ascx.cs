using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{

	public partial class AdministrarClientes : System.Web.UI.UserControl
	{
		
        //protected TextBox tbMod,tbEli;
        //protected Button btnCrear,btnModificar,btnEliminar;
		protected string mainPage=ConfigurationSettings.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender, EventArgs e)
		{
            if (Request.QueryString["Mod"] != null)
            {
                Utils.MostrarAlerta(Response, "Se ha Modificado Correctamente al Cliente: " + Request.QueryString["nomClie"] + ".");
            }
			Session.Clear();
		}
		
		protected void btnCrear_Click(object Sender, EventArgs e)
		{
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM pproducto;SELECT * FROM ptarifas");
			if(ds.Tables[0].Rows.Count==0 || ds.Tables[1].Rows.Count==0)
                Utils.MostrarAlerta(Response, "No ha especificado su catalogo de productos o no ha registrado tarifas al cliente. \\nRevise por favor");
			else
				Response.Redirect(mainPage+"?process=Tools.CrearCliente");
		}
		
		protected void btnModificar_Click(object Sender, EventArgs e)
		{
			if(!DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='"+this.tbMod.Text+"'"))
                Utils.MostrarAlerta(Response, "Nit Inexistente");
			else
				Response.Redirect(mainPage+"?process=Tools.CrearCliente&nit="+this.tbMod.Text+"");
		}
		
		protected void  btnEliminar_Click(object Sender, EventArgs e)
		{
			if(!DBFunctions.RecordExist("SELECT * FROM mnit WHERE mnit_nit='"+this.tbEli.Text+"'"))
                Utils.MostrarAlerta(Response, "Nit Inexistente");
			else
			{
				ArrayList sqls=new ArrayList();
				sqls.Add("UPDATE mclientesac SET tvig_codigo='C' WHERE mnit_nit='"+this.tbEli.Text+"'");
				if(DBFunctions.Transaction(sqls))
					Response.Redirect(mainPage+"?process=Tools.AdministrarClientes");
				else
                Utils.MostrarAlerta(Response, "Ocurrio una anomalia en el sistema vuelva a repetir el proceso");
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
        