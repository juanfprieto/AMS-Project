	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
    using System.IO;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using System.Collections;
    namespace AMS.Inventarios
{
        /// <summary>
	///		Descripci�n breve de AMS_Inventarios_CierreInventarios.
	/// </summary>
	public partial class CierreInventarios : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
        private const int numActualiza = 500;
       
        protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aqu� el c�digo de usuario para inicializar la p�gina
			if(!IsPostBack)
			{
				anoVigente.Text=DBFunctions.SingleData("SELECT pano_ano FROM cinventario");
				mesVigente.Text=DBFunctions.SingleData("SELECT pmes_nombre from pmes WHERE pmes_mes=(SELECT pmes_mes FROM cinventario)");
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

        protected void Proceso_Insertar(int a�o)
        {
            ArrayList sqlStrings = new ArrayList();
            sqlStrings = new ArrayList();
            sqlStrings.Add (@"INSERT INTO dbxschema.msaldoitem  
              SELECT MITE_CODIGO,  
                     PANO_ANO +1,  
                     MSAL_CANTACTUAL,  
                     MSAL_CANTASIG,  
                     MSAL_COSTPROM,  
                     MSAL_COSTPROMHIST,  
                     MSAL_COSTPROM,  
                     MSAL_COSTPROMHIST,  
                     MSAL_CANTACTUAL,  
                     MSAL_ULTIVENT,  
                     MSAL_ULTIINGR,  
                     MSAL_ULTICOST,  
                     MSAL_ULTIPROV,  
                     MSAL_ABCD,  
                     MSAL_PEDITRANS,  
                     MSAL_UNIDTRANS,  
                     MSAL_PEDIPENDI,  
                     MSAL_UNIDPENDI  
              FROM dbxschema.msaldoitem  
              WHERE pano_ano = " + a�o + ";");
            sqlStrings.Add(@"INSERT INTO dbxschema.msaldoitemalmacen  
              SELECT MITE_CODIGO,  
                     PALM_ALMACEN,  
                     PANO_ANO + 1,  
                     MSAL_CANTASIG,  
                     MSAL_CANTACTUAL,  
                     MSAL_COSTPROM,  
                     MSAL_COSTPROM,  
                     MSAL_CANTPENDIENTE,  
                     MSAL_CANTTRANSITO,  
                     MSAL_CANTACTUAL  
              FROM dbxschema.msaldoitemalmacen  
              WHERE PANO_ANO = "+a�o+";");
            if (DBFunctions.Transaction(sqlStrings) == false)
            {
                Response.Write("<script language:java>alert('Error Insertando cantidades!');</script> ");
                lb.Text = DBFunctions.exceptions;
                return;
            }
        }

        
        protected void btnProceso_Click(object sender, System.EventArgs e)
		{

			ArrayList sqlStrings= new ArrayList();
			
			int mesVig=Convert.ToInt32(DBFunctions.SingleData("SELECT pmes_mes FROM cinventario"));
			int mes=0,ano=0,ano1=0;

			ano=Convert.ToInt32(anoVigente.Text);
			if(mesVig==12)
			{
                mes = 1;
                ano1 = ano + 1;
                if(!DBFunctions.RecordExist("SELECT * FROM pano WHERE pano_ano="+ano1.ToString()))
			        {
				        Response.Write("<script language:javascript>alert('El nuevo a�o vigente no existe en la tabla de a�os.\\n Por favor ingr�selo y vuelva a repetir el proceso');</script>");
			        }
			    else
			        {
                         Proceso_Insertar(ano);
                         sqlStrings.Add("UPDATE CINVENTARIO SET PMES_MES = " + mes + " , PANO_ANO = "+ano1+"");
                         if (DBFunctions.Transaction(sqlStrings) == false)
                         {
                             Response.Write("<script language:java>alert('Error Actulizando A�o!');</script> ");
                             lb.Text = DBFunctions.exceptions;
                             return;
                         }
                         else
                         {
                             Response.Write("<script language:java>alert('El cierre anual de inventario se ha realizado, por favor ejecute sus controles de validaci�n !!!');</script> ");
                             Page.Visible = false;
                         }
                    }
			}
			else
			{
                mes=mesVig+1;
				ano1=ano;
                sqlStrings.Add("UPDATE CINVENTARIO SET PMES_MES = " +mes+"");
                if (DBFunctions.Transaction(sqlStrings) == false)
                {
                    Response.Write("<script language:java>alert('Error Actulizando Mes!');</script> ");
                    lb.Text = DBFunctions.exceptions;
                    return;
                }
                else
                {
                    Response.Write("<script language:java>alert('El cierre mensual de inventario se ha realizado, por favor ejecute sus controles de validaci�n !!!');</script> ");
                    Page.Visible = false;
                }
             }
                //Eliminar reportes viejos
                string[] files = Directory.GetFiles(ConfigurationManager.AppSettings["PathToReports"].ToString());
                foreach (string file in files){
                    File.Delete(file);
                }
				/*
									debe crear de cada registro de msaldoitems y de msaldoitemsalamcen un nuevo registro con el saldo del ano 
									al nuevo a�o pasando la existencia actual como actual e inicial del a�o, el costo promedio como actual e historico e hist-hist
									y todos los demas datos del registro exactamente iguales 
				*/
				
			
		}
	}
}
