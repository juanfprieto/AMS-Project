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
	public partial  class ActivosFijos : System.Web.UI.UserControl
	{
		protected string MainPage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		protected void Page_Load(object Sender,EventArgs e)
		{
            if (Request.QueryString["fin"] != null && Request.QueryString["fin"] == "1")
            {
                Utils.MostrarAlerta(Response, "Activo fijo creado correctamente!");
            }
            else if (Request.QueryString["fin"] != null && Request.QueryString["fin"] == "2")
            {
                Utils.MostrarAlerta(Response, "Activo fijo actualizado correctamente!");
            }
		}
		
		protected void btnIngresar_Click(object Sender,EventArgs e)
		{
			Response.Redirect(MainPage+"?process=Tesoreria.ActivosFijos.Ingresar");
		}
		
		protected void btnEditar_Click(object Sender,EventArgs e)
		{
			if(DBFunctions.RecordExist("SELECT * FROM dbxschema.mactivofijo WHERE mafj_codiacti='"+tbEditar.Text+"'"))
				Response.Redirect(MainPage+"?process=Tesoreria.ActivosFijos.Ingresar&act="+tbEditar.Text+"");
			else
            Utils.MostrarAlerta(Response, "Código Inexistente");
		}
		
		protected void btnEliminar_Click(object Sender,EventArgs e)
		{
			if(DBFunctions.RecordExist("SELECT * FROM dbxschema.mactivofijo WHERE mafj_codiacti='"+tbEliminar.Text+"'"))
			{
				ArrayList sql=new ArrayList();
				sql.Add("DELETE FROM dbxschema.mactivofijo WHERE mafj_codiacti='"+tbEliminar.Text+"'");
				if(DBFunctions.Transaction(sql))
					Response.Redirect(MainPage+"?process=Tesoreria.ActivosFijos");
				else
                Utils.MostrarAlerta(Response, "No se puede eliminar el activo fijo. Una relación padre - hijo lo restringe");
			}
			else
            Utils.MostrarAlerta(Response, "Código Inexistente");
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
