// created on 26/04/2005 at 14:06
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;

namespace AMS.Vehiculos
{
	public partial class GastoDirecto : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        private FormatosDocumentos formatoRecibo = new FormatosDocumentos();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{

                if (Request.QueryString["pref"] != null && Request.QueryString["num"] != null)
                {
                    try
                    {
                        formatoRecibo.Prefijo = Request.QueryString["pref"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["num"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pref"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }
                        formatoRecibo.Prefijo = Request.QueryString["pref"];
                        formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["num"]);
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pref"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=600');</script>");
                        }
                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes + "<br>";
                    }
                }

				lb.Text = "";
			}
		}
		
		protected void Realizar_Inclusion(Object  Sender, EventArgs e)
		{
			if(tipoInclusion.SelectedValue=="V")
			{
				if(chkConsecutivo.Checked)
					Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirectoVehiculos&cons=A");
				else
					Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirectoVehiculos&cons=M");
			}
			if(tipoInclusion.SelectedValue=="E")
			{
				if(chkConsecutivo.Checked)
					Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirectoEmbarques&cons=A");
				else
					Response.Redirect("" + indexPage + "?process=Vehiculos.GastoDirectoEmbarques&cons=M");
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

		}
		#endregion
	}
}
