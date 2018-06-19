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
using AMS.DB;
using AMS.Documentos;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial class EmisionRecibos : System.Web.UI.UserControl
	{
		protected string MainPage=ConfigurationManager.AppSettings["MainIndexPage"];
        protected HtmlGenericControl descripcionEncabezado = new HtmlGenericControl();
		FormatosDocumentos formatoRecibo=new FormatosDocumentos();
				
		protected void Page_Load(object Sender,EventArgs e)
		{
			Session.Clear();
            if (Request.QueryString["cruce"] != null)
            {
                tipoRecibo.Items[0].Text = "Lógica de Cliente ó Notas Crédito Proveedor";
                tipoRecibo.Items[1].Text = "Lógica de Proveedor ó Notas Débito Clientes";
                tipoRecibo.Items.RemoveAt(2);
                descripcionEncabezado.InnerText = "Lógica de Cruce a Aplicar: ";
            }

			if(!IsPostBack)
			{
				if(Request.QueryString["pre"]!=null && Request.QueryString["num"]!=null && Request.QueryString["tip"]!=null)
				{
					if(Request.QueryString["tip"]=="RC")
                        Utils.MostrarAlerta(Response, "Se ha creado el recibo de caja con prefijo "+Request.QueryString["pre"]+" y número "+Request.QueryString["num"]+"");
					else if(Request.QueryString["tip"]=="CE")
                        Utils.MostrarAlerta(Response, "Se ha creado el comprobante de egreso con prefijo "+Request.QueryString["pre"]+" y número "+Request.QueryString["num"]+"");
					else if(Request.QueryString["tip"]=="RP")
                        Utils.MostrarAlerta(Response, "Se ha creado el recibo provisional con prefijo " + Request.QueryString["pre"] + " y número " + Request.QueryString["num"] + "");
					try
					{
						formatoRecibo.Prefijo=Request.QueryString["pre"];
						formatoRecibo.Numero=Convert.ToInt32(Request.QueryString["num"]);
						formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["pre"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
						{
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
						}
                        formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + Request.QueryString["pre"] + "'");
                        if (formatoRecibo.Codigo != string.Empty)
                        {
                            if (formatoRecibo.Cargar_Formato())
                                Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=500');</script>");
                        }
					}
					catch
					{
						lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>"+"El recibo puede ser impreso por la opción Impresion<br>";
					}
				}
			}
		}
		
		protected void tipoRecibo_IndexChanged(object Sender,EventArgs e)
		{
			//Si se entra por la opcion dos de recibos de caja
            string paramCruce = "";
            if (Request.QueryString["cruce"] != null)
            {
                paramCruce = "&cruce=1";
            }
            if(Request.QueryString["cas"]!=null && Request.QueryString["cas"]=="1")
			{
				if(tipoRecibo.SelectedValue=="RC")
				{
					if(!DBFunctions.RecordExist("SELECT * FROM dbxschema.ccartera"))
                        Utils.MostrarAlerta(Response, "Para el correcto funcionamiento del proceso, \\n debe primero llenar los Parámetros de Cartera");
					else
                        Response.Redirect(MainPage + "?process=Tesoreria.EmisionRecibos.CuerpoRecibo&tipo=RC&cnd=1" + paramCruce);
				}
				else if(tipoRecibo.SelectedValue=="CE")
				{
					if(!DBFunctions.RecordExist("SELECT * FROM dbxschema.ccartera"))
                        Utils.MostrarAlerta(Response, "Para el correcto funcionamiento del proceso, \\n debe primero llenar los Parámetros de Cartera");
					else
                        Response.Redirect(MainPage + "?process=Tesoreria.EmisionRecibos.CuerpoRecibo&tipo=CE&cnd=1" + paramCruce);
				}
				else if(tipoRecibo.SelectedValue=="RP")
				{
					if(!DBFunctions.RecordExist("SELECT * FROM dbxschema.ccartera"))
                        Utils.MostrarAlerta(Response, "Para el correcto funcionamiento del proceso, \\n debe primero llenar los Parámetros de Cartera");
					else
                        Response.Redirect(MainPage + "?process=Tesoreria.EmisionRecibos.CuerpoRecibo&tipo=RP&cnd=1" + paramCruce);
				}
				else if(tipoRecibo.SelectedValue=="RI")
                    Response.Redirect(MainPage + "?process=Tesoreria.ReImpresionRecibos");
			}
			else
			{
				if(tipoRecibo.SelectedValue=="RC")
				{
					if(!DBFunctions.RecordExist("SELECT * FROM dbxschema.ccartera"))
                        Utils.MostrarAlerta(Response, "Para el correcto funcionamiento del proceso, \\n debe primero llenar los Parámetros de Cartera");
					else
                        Response.Redirect(MainPage + "?process=Tesoreria.EmisionRecibos.CuerpoRecibo&tipo=RC" + paramCruce);

				}
				else if(tipoRecibo.SelectedValue=="CE")
				{
					if(!DBFunctions.RecordExist("SELECT * FROM dbxschema.ccartera"))
                        Utils.MostrarAlerta(Response, "Para el correcto funcionamiento del proceso, \\n debe primero llenar los Parámetros de Cartera");
					else
                        Response.Redirect(MainPage + "?process=Tesoreria.EmisionRecibos.CuerpoRecibo&tipo=CE" + paramCruce);
				}
				else if(tipoRecibo.SelectedValue=="RP")
				{
					if(!DBFunctions.RecordExist("SELECT * FROM dbxschema.ccartera"))
                        Utils.MostrarAlerta(Response, "Para el correcto funcionamiento del proceso, \\n debe primero llenar los Parámetros de Cartera");
					else
                        Response.Redirect(MainPage + "?process=Tesoreria.EmisionRecibos.CuerpoRecibo&tipo=RP" + paramCruce);
				}
				else if(tipoRecibo.SelectedValue=="RI")
					Response.Redirect(MainPage+"?process=Tesoreria.ReImpresionRecibos");
				else if(tipoRecibo.SelectedValue=="TP")
					Response.Redirect(MainPage+"?process=Comercial.EmisionRecibos");
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
