
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
using AMS.Tools;
namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_ModificarNitOt.
	/// </summary>
	public partial class AMS_Automotriz_ModificarNitOt : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
            DatasToControls param = new DatasToControls();
			if (!IsPostBack)
			{
                ddlNumeroOrden.Enabled = false;
                //DatasToControls param = new DatasToControls();
				//param.PutDatasIntoDropDownList(ddlPrefijoOt,"Select * from DBXSCHEMA.PDOCUMENTO where tdoc_tipodocu='OT'");
                Utils.llenarPrefijos(Response, ref ddlPrefijoOt , "%", "%", "OT");
                //param.PutDatasIntoDropDownList(ddlNumeroOrden, "Select mord_numeorde from DBXSCHEMA.MORDEN where test_estado <> 'F' order by mord_numeorde");				
				
			}
            else
            {
                ddlNumeroOrden.Enabled = true;
                param.PutDatasIntoDropDownList(ddlNumeroOrden, "Select mord_numeorde from DBXSCHEMA.MORDEN where test_estado <> 'F' and pdoc_codigo='" + Request.Form[ddlPrefijoOt.UniqueID]+ "'" + " order by mord_numeorde");
            }
			
			// Introducir aquí el código de usuario para inicializar la página
		}

		protected void CambiarNit (object sender, EventArgs e)
		{
			try 
			{
                if (txtNit.Text == "")
                {
                    Utils.MostrarAlerta(Response, "Debe seleccionar un Nit");
                    return;
                }
				DBFunctions.NonQuery("update dbxschema.morden set mnit_nit='"+txtNit.Text+"' where mord_numeorde="+ddlNumeroOrden.SelectedValue+" and pdoc_codigo='"+ddlPrefijoOt.SelectedValue+"'");			
				txtNit.Text="";
                Utils.MostrarAlerta(Response, "Nit modificado");
			}
			catch(Exception ex)
			{
				lb.Text=ex.Message;
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
