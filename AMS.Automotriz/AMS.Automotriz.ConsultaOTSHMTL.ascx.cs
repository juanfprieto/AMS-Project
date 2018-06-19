
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
    using AMS.Documentos;
	using AMS.Forms;
	using AMS.DBManager;
using AMS.Tools;

namespace AMS.Automotriz
{

	/// <summary>
	///		Descripci�n breve de AMS_Automotriz_ConsultaOTSHMTL.
	/// </summary>
	public partial class AMS_Automotriz_ConsultaOTSHMTL : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo=new FormatosDocumentos();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//bind.PutDatasIntoDropDownList(ddlPreOT,"SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_nombre FROM dbxschema.pdocumento WHERE tdoc_tipodocu='OT'");
                Utils.llenarPrefijos(Response, ref ddlPreOT , "%", "%", "OT");
				bind.PutDatasIntoDropDownList(ddlNumOT,"SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='"+ddlPreOT.SelectedValue+"'");
			}
		}

		#region C�digo generado por el Dise�ador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Dise�ador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		M�todo necesario para admitir el Dise�ador. No se puede modificar
		///		el contenido del m�todo con el editor de c�digo.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void btnEnviar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Automotriz.VistaImpresion&prefOT="+ddlPreOT.SelectedValue+"&numOT="+ddlNumOT.SelectedValue+"&tipVis=orden&revAse=S");
		}

		protected void ddlPreOT_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumOT,"SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='"+ddlPreOT.SelectedValue+"'");
		}
	}
}
