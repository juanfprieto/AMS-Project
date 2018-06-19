using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using AMS.Tools;


namespace AMS.Contabilidad
{	
	
	public partial class UnificarNits : System.Web.UI.UserControl 
	{
	
		protected void  Page_Load(object sender , System.EventArgs e )
		{
            if (!IsPostBack)
            {
                
			}
            Ajax.Utility.RegisterTypeForAjax(typeof(AMS.Contabilidad.UnificarNits));
        }
		
		protected  void  ReclasificarButton_onclick(object  sender , EventArgs e )	
		{	
			if(txtNiterrado.Text == txtNitcorrecto.Text)
                Utils.MostrarAlerta(Response, "Nits Iguales");
            else
            {
                if(!DB.DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT = '"+txtNiterrado.Text+"'"))
                {
                    Utils.MostrarAlerta(Response, "EL Nit Errado NO EXISTE ...");
                }
                else
                {
                    if(!DB.DBFunctions.RecordExist("SELECT MNIT_NIT FROM MNIT WHERE MNIT_NIT = '"+txtNitcorrecto.Text.Trim()+"'"))
                    {
                        Utils.MostrarAlerta(Response, "EL Nit Correcto NO EXISTE ...");
                    }
                    else
			        {
				        string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
				        Reclasificar obj = new Reclasificar(txtNitcorrecto.Text,txtNiterrado.Text,"MNIT_NIT","MNIT");
				        if(obj.ProcUnParam())
					        Response.Redirect("" + indexPage + "?process=Contabilidad.UnificarNits");
				        else
					        lb.Text = obj.ProcessMsg;
			        }
                }
			}
		}
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}

		private void InitializeComponent()
		{

		}
        [Ajax.AjaxMethod()]
        public string cambiarNombre(string nit)
        {
            string nombre = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT WHERE MNIT_NIT = '" + nit + "'");
            return nombre;
        }
	}
}

