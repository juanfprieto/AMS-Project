// created on 11/05/2005 at 14:53

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
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using AMS.Documentos;


namespace AMS.Automotriz
{

	public partial class GarantiasAprobadas : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
        protected FormatosDocumentos formatoFactura = new FormatosDocumentos();
		
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				//bind.PutDatasIntoDropDownList(prefFact,"SELECT pdoc_codigo, pdoc_nombre FROM pdocumento WHERE tdoc_tipodocu='FC'");
                Utils.llenarPrefijos(Response, ref prefFact , "%", "%", "FC");
				//Fecha de la factura
				fecha.SelectedDate = DateTime.Now;
				fechFactura.Text = fecha.SelectedDate.Date.ToString("yyyy-MM-dd");
                if (Request["prefF"] != null && Request["numF"] != null)
                {
                    Utils.MostrarAlerta(Response, "Se ha creado la factura " + Request["prefF"] + " - " + Request["numF"] + "");
                    // impresion del formato
                    try
					{
				        formatoFactura.Prefijo = Request["prefF"];
                        formatoFactura.Numero  = Convert.ToInt32(Request.QueryString["numF"]);
                        formatoFactura.Codigo  = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefF"] + "' ");
                        if (formatoFactura.Codigo != string.Empty)
                        {
                            if (formatoFactura.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=800,WIDTH=800');</script>");
                        }
                        formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["prefF"] + "' ");
                        if (formatoFactura.Codigo != string.Empty)
                        {
                            if (formatoFactura.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                        }

                    }
                    catch
                    {
                        lb.Text += "Error al generar la impresión. Detalles : " + formatoFactura.Mensajes + "<br>" + "El recibo puede ser impreso por la opción Impresion<br>";
                    }
                }
			}
		}
		
		protected void ChangeDate(Object Sender, EventArgs E)
		{
			fechFactura.Text = fecha.SelectedDate.Date.ToString("yyyy-MM-dd");
		}
		
		protected void Nueva_Factura(Object Sender, EventArgs E)
		{
                if(chkGenNum.Checked == true)
				    Response.Redirect("" + indexPage + "?process=Automotriz.GarantiasAprobadasFormato&pref="+prefFact.SelectedValue+"&gen=A&nit="+nitCSA.Text+"&fech="+fechFactura.Text+"");
			    else
				    Response.Redirect("" + indexPage + "?process=Automotriz.GarantiasAprobadasFormato&pref="+prefFact.SelectedValue+"&gen=M&nit="+nitCSA.Text+"&fech="+fechFactura.Text+"");
            
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
