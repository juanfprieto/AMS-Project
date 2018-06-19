// created on 29/10/2003 at 03:39 a.m.
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.Tools;

namespace  AMS.Contabilidad  

{
	 
 public partial	class	ReclasificarCuentas : System.Web.UI.UserControl
  {
 	


    protected void  Page_Load(object sender , System.EventArgs e )
    {
    	if  (!IsPostBack)
    	{
			DatasToControls bind = new DatasToControls("mcuenta");
			bind.PutDatasIntoDropDownList(errorAccount, "SELECT mcue_codipuc, mcue_codipuc CONCAT ' - ' CONCAT mcue_nombre FROM mcuenta ORDER BY mcue_codipuc");	
 	   		bind.PutDatasIntoDropDownList(rightAccount, "SELECT mcue_codipuc, mcue_codipuc CONCAT ' - ' CONCAT mcue_nombre FROM mcuenta ORDER BY mcue_codipuc");
    	}
    }
 	
 	
 	protected  void  ReclasificarButton_onclick (object  sender , EventArgs e )
 	{
 		if(errorAccount.SelectedValue!=rightAccount.SelectedValue)
 		{
 			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
	 		Reclasificar obj = new Reclasificar(rightAccount.SelectedValue,errorAccount.SelectedValue,"MCUE_CODIPUC","MCUENTA");
 			if(obj.ProcUnParam())
 				Response.Redirect("" + indexPage + "?process=Contabilidad.ReclasificarCuentas");
 			else
 				lb.Text = obj.ProcessMsg;
 		}
 		else
        Utils.MostrarAlerta(Response, "Cuentas Iguales");
 	}
 	
 	override  protected void  OnInit(EventArgs e) 
 	{
 		InitializeComponent();
 		base.OnInit(e);
 		
 	}
 	
 	private  void InitializeComponent()
 	{
	}
  }
}
