
	using System;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.DBManager;
    using AMS.Documentos;
    using AMS.Tools;
    namespace AMS.Automotriz
{
	/// <summary>
	///		Descripción breve de AMS_Automotriz_ReImpresionOT.
	/// </summary>
	public partial class AMS_Automotriz_ReImpresionOT : System.Web.UI.UserControl
	{
		protected string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected FormatosDocumentos formatoRecibo = new FormatosDocumentos();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				DatasToControls bind=new DatasToControls();
				//bind.PutDatasIntoDropDownList(ddlPreOT,"SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_nombre FROM dbxschema.pdocumento WHERE tdoc_tipodocu='OT'");
                Utils.llenarPrefijos(Response, ref ddlPreOT, "%", "%", "OT");
				//bind.PutDatasIntoDropDownList(ddlNumOT,"SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='"+ddlPreOT.SelectedValue+"'");
				if(Request["prefOT"] != null && Request["prefOT"] != string.Empty && Request["numOT"] != null && Request["numOT"] != string.Empty)
				{
					try
					{
						formatoRecibo.Prefijo = Request["prefOT"];
						formatoRecibo.Numero = Convert.ToInt32(Request.QueryString["numOT"]);
						formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+Request.QueryString["prefOT"]+"'");
						if(formatoRecibo.Codigo!=string.Empty)
						{
							if(formatoRecibo.Cargar_Formato())
								Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=600');</script>");
						}
					}
					catch
					{
						//lbInfo.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>";
                        Utils.MostrarAlerta(Response, "Error al generar la impresión. Detalles : " + formatoRecibo.Mensajes.Replace("'", "") + "");
					}
				}
			}
		}

		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion

		protected void btnEnviar_Click(object sender, System.EventArgs e)
		{
			//Response.Redirect(indexPage+"?process=Automotriz.VistaImpresion&prefOT="+ddlPreOT.SelectedValue+"&numOT="+ddlNumOT.SelectedValue+"&tipVis=orden");
			//Response.Redirect(indexPage+"?process=Automotriz.ReImpresionOT&prefOT="+ddlPreOT.SelectedValue+"&numOT="+ddlNumOT.SelectedValue+"");
            if (txtNumOT.Text != "")
                Response.Redirect(indexPage + "?process=Automotriz.ReImpresionOT&prefOT=" + ddlPreOT.SelectedValue + "&numOT=" + txtNumOT.Text + "");
		}

        //protected void ddlPreOT_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    DatasToControls bind=new DatasToControls();
        //    bind.PutDatasIntoDropDownList(ddlNumOT,"SELECT mord_numeorde FROM dbxschema.morden WHERE pdoc_codigo='"+ddlPreOT.SelectedValue+"'");
        //}
	}
}
