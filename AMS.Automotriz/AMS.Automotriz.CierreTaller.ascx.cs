
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
using AMS.Tools;

namespace AMS.Automotriz
{
	/// <summary>
	///		Descripci�n breve de AMS_Automotriz_CierreTaller.
	/// </summary>
	public partial class AMS_Automotriz_CierreTaller : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(!IsPostBack)
			{
				if(Request.QueryString["ext"]!=null)
                    Utils.MostrarAlerta(Response, "Cierre efectuado satisfactoriamente");
				anoVigente.Text=DBFunctions.SingleData("SELECT pano_ano FROM ctaller");
				mesVigente.Text=DBFunctions.SingleData("SELECT pmes_nombre from pmes WHERE pmes_mes=(SELECT pmes_mes FROM ctaller)");
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

		protected void btnProceso_Click(object sender, System.EventArgs e)
		{
			int mesVig=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM ctaller"));
			int mes=0,ano=0;
			ano=Convert.ToInt32(anoVigente.Text);
			if(mesVig==12)
			{
				mes=1;
				ano=Convert.ToInt32(anoVigente.Text)+1;
			}
			else
				mes=mesVig+1;
			if(!DBFunctions.RecordExist("SELECT * FROM pano WHERE pano_ano="+ano+""))
                Utils.MostrarAlerta(Response, "El nuevo a�o vigente no existe en la tabla de a�os.\\n Por favor ingreselo y vuelva a repetir el proceso");
			else
			{
				if(DBFunctions.NonQuery("UPDATE dbxschema.ctaller SET pano_ano="+ano+",pmes_mes="+mes+"")==1)
					Response.Redirect(indexPage+"?process=Automotriz.CierreTaller&ext=1");
			}
		}
	}
}
