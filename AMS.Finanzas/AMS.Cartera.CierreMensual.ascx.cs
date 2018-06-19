namespace AMS.Finanzas
{
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
    using AMS.Tools;

	/// <summary>
	///		Descripci�n breve de AMS_Cartera_CierreMensual.
	/// </summary>
	public partial class AMS_Cartera_CierreMensual : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
                if(Request.QueryString["proceso"] != "T")
                {
                    fldCartera.Visible = true;
                    anoVigenteC.Text = DBFunctions.SingleData("SELECT pano_anovigente FROM ccartera");
                    mesVigenteC.Text = DBFunctions.SingleData("SELECT pmes_nombre from pmes WHERE pmes_mes=(SELECT pmes_mesvigente FROM ccartera)");
                }
                else
                {
                    fldTesoreria.Visible = true;
                    anoVigenteT.Text = DBFunctions.SingleData("SELECT ctes_anovige FROM ctesoreria");
                    mesVigenteT.Text = DBFunctions.SingleData("SELECT pmes_nombre from pmes WHERE pmes_mes=(SELECT ctes_mesvige FROM ctesoreria)");
                }
				if(Request.QueryString["ext"]!=null)
                    Utils.MostrarAlerta(Response, "Cierre efectuado satisfactoriamente");
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
        
		protected void proceso_Cartera(object sender, System.EventArgs e)
		{
			int mesVig=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mesvigente FROM ccartera"));
			int mes=0,ano=0;
			ano=Convert.ToInt32(anoVigenteC.Text);
			if(mesVig==12)
			{
				mes=1;
				ano=Convert.ToInt32(anoVigenteC.Text)+1;
			}
			else
				mes=mesVig+1;
			if(!DBFunctions.RecordExist("SELECT * FROM pano WHERE pano_ano="+ano+""))
                Utils.MostrarAlerta(Response, "El nuevo a�o vigente no existe en la tabla de a�os.\\n Por favor ingr�selo y vuelva a repetir el proceso");
			else
			{
				if(DBFunctions.NonQuery("UPDATE dbxschema.ccartera SET pano_anovigente="+ano+",pmes_mesvigente="+mes+"")==1)
					Response.Redirect(indexPage+"?process=Cartera.CierreMensual&ext=1&proceso=C");
                else
                {
                    Utils.MostrarAlerta(Response, "Fall� el proceso.");
                    lbT.Text = "Se present� una falla debido a que el estado de la tabla principal de Cartera puede encontrarse saturado, bloqueado � le falta alg�n campo, por favor informe al Administrador del sistema.";
                }
			}
		}

        protected void proceso_Tesoreria(object sender, System.EventArgs e)
        {
            int mesVig = Convert.ToInt32(DBFunctions.SingleData("SELECT ctes_mesvige FROM ctesoreria"));
            int mes = 0, ano = 0;
            ano = Convert.ToInt32(anoVigenteT.Text);
            if (mesVig == 12)
            {
                mes = 1;
                ano = Convert.ToInt32(anoVigenteT.Text) + 1;
            }
            else
                mes = mesVig + 1;
            if (!DBFunctions.RecordExist("SELECT * FROM pano WHERE pano_ano=" + ano + ""))
                Utils.MostrarAlerta(Response, "El nuevo a�o vigente no existe en la tabla de a�os.\\n Por favor ingr�selo y vuelva a repetir el proceso");
            else
            {
                if (DBFunctions.NonQuery("UPDATE dbxschema.ctesoreria SET ctes_anovige=" + ano + ", ctes_mesvige=" + mes + "") == 1)
                    Response.Redirect(indexPage + "?process=Cartera.CierreMensual&ext=1&proceso=T");
                else
                {
                    Utils.MostrarAlerta(Response, "Fall� el proceso.");
                    lbT.Text = "Se present� una falla debido a que el estado de la tabla principal de Tesorer�a puede estar saturado, bloqueado � le falta alg�n campo, por favor informe al Administrador del sistema.";
                }
                    
            }
        }
    }
}
