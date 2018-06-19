// created on 10/08/2004 at 10:11
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
using AMS.Forms;

namespace AMS.Contabilidad
{
	public partial class Prueba : System.Web.UI.UserControl 
	{
		protected DropDownList rightAccount,errorAccount;
		
		
		protected void  Page_Load(object sender , System.EventArgs e )
		{
			lb.Text="Por que1";
		}

		private void InitializeComponent()
		{
		
		}
		
		protected  void  ReclasificarButton_onclick(object  sender , EventArgs e )
 	
 		{	
 		//ENVIA  EL  VALOR  CORRECTO DEL COMBO,  EL  ERRADO ,  Y  EL  CAMPO A CLAVE.
		//Reclasificar obj = new Reclasificar(errorNit.SelectedValue,rightNit.SelectedValue,"MNIT_NIT" );				
	 	//ENVIA  N TABLAS ADEMAS ACTUALIZA  LA TABLA  ERRADA  POR  LA  NUEVA
 		//obj.ProcUnParam("MNIT","DCUENTA","MAUXILIARCUENTA","DCAJA","MACTIVOFIJO","PAPORTEPATRONAL","PCONTRATO","PEPS","PFONDOCESANTIAS","PFONDOPENSION","PPROVEEDOR","PRIESGOPROFESIONAL");
 		//   "MACTIVOFIJO","PAPORTEPATRONAL","PCONTRATO","PEPS","PFONDOCESANTIAS","PFONDOPENSION","PPROVEEDOR","PRIESGOPROFESIONAL"
 		lb.Text="jeje jojo jiji";
 		//lb.Text=obj.Parametros + "<BR>"; 		
 		
 		}	

	}
}
