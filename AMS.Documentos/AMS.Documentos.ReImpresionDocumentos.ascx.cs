using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Timers;
using System.Web.SessionState;
using System.Web.UI;
using System.Globalization;

namespace AMS.Documentos
{
	/// <summary>
	///		Descripción breve de AMS_Tools_ReImpresionDocumentos.
	/// </summary>
    public partial class ReImpresionDocumentos : System.Web.UI.UserControl
	{
		private FormatosDocumentos formatoRecibo=new FormatosDocumentos();

		protected void Page_Load(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlPrefijo,"SELECT pdoc_codigo,pdoc_codigo CONCAT ' - ' CONCAT pdoc_nombre FROM  pdocumento WHERE sfor_codigo IS NOT NULL ORDER BY pdoc_codigo");
                if (ddlPrefijo.Items.Count > 0)
                    ddlPrefijo.Items.Insert(0, "Seleccione:..");
                if (tipoPedido.SelectedValue == "C")
                {
                    bind.PutDatasIntoDropDownList(ddlPrefPed, "SELECT pped_codigo,pped_codigo CONCAT ' - ' CONCAT pped_nombre FROM ppedido WHERE sfor_codigo IS NOT NULL and tped_codiuso in ('C','M') ORDER BY pped_codigo");
                    if (ddlPrefPed.Items.Count > 0)
                        ddlPrefPed.Items.Insert(0, "Seleccione:..");
                }
                CargarNumerosPedido();
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

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
            int contador = 0;
			formatoRecibo.Prefijo=ddlPrefijo.SelectedValue;
			formatoRecibo.Numero=Convert.ToInt32(tbnumero.Text);
			formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='"+ddlPrefijo.SelectedValue+"'");
                
            if (formatoRecibo.Codigo!=string.Empty)
			{
                if (formatoRecibo.Cargar_Formato())
                    try
                    {
                        Utils.MostrarPDF(Response, formatoRecibo.Documento, 800);
                    }catch { contador++; }
                        
                //Response.Write("<script language:javascript>w=window.open('../aspx/AMS.Web.ModalPDF.aspx?rpt=" + formatoRecibo.Documento.Replace("../rptgen/","") + "', '','HEIGHT=650,WIDTH=800');</script>");
                //Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
            }
            formatoRecibo.Codigo = DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.pdocumento WHERE pdoc_codigo='" + ddlPrefijo.SelectedValue + "'");
            if (formatoRecibo.Codigo != string.Empty)
            {
                if (formatoRecibo.Cargar_Formato())
                    try
                    {
                        Utils.MostrarPDF(Response, formatoRecibo.Documento, 700);
                    }
                    catch { contador++; }
                        
                //Response.Write("<script language:javascript>w=window.open('../aspx/AMS.Web.ModalPDF.aspx?rpt=" + formatoRecibo.Documento.Replace("../rptgen/", "") + "', '','HEIGHT=650,WIDTH=758');</script>");
                //Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=700');</script>");
            }
            if(contador > 1)
            {
                Utils.MostrarAlerta(Response,
                "No se pudo cargar el Reporte: " + formatoRecibo.Codigo + "-" + formatoRecibo.Prefijo + "-" + formatoRecibo.Numero +
                ". Por favor contactar al administrador del sistema.");
            }
		}

		protected void btnAceptar2_Click(object sender, System.EventArgs e)
		{
			if(ddlPrefPed.Items.Count!=0)
			{
				try
				{
					formatoRecibo.Prefijo=ddlPrefPed.SelectedValue;
					formatoRecibo.Numero=Convert.ToInt32(ddlNumPed.SelectedValue);
					//if(tipoPedido.SelectedValue=="C")
					formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.ppedido WHERE pped_codigo='"+ddlPrefPed.SelectedValue+"'");
                    if (formatoRecibo.Codigo != string.Empty)
                    {
                        if (formatoRecibo.Cargar_Formato())
                            Utils.MostrarPDF(Response, formatoRecibo.Documento, 800);
                            //Response.Write("<script language:javascript>w=window.open('" + formatoRecibo.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                    }
                    //else if(tipoPedido.SelectedValue=="P")
                    formatoRecibo.Codigo=DBFunctions.SingleData("SELECT sfor_codigo2 FROM dbxschema.ppedido WHERE pped_codigo='"+ddlPrefPed.SelectedValue+"'");
					if(formatoRecibo.Codigo!=string.Empty)
					{
						if(formatoRecibo.Cargar_Formato())
                            Utils.MostrarPDF(Response, formatoRecibo.Documento, 700);
                            //Response.Write("<script language:javascript>w=window.open('"+formatoRecibo.Documento+"','','HEIGHT=600,WIDTH=800');</script>");
                    }
				}
				catch
				{
					lb.Text+="Error al generar la impresión. Detalles : "+formatoRecibo.Mensajes+"<br>"+"El recibo puede ser impreso por la opción Impresion<br>";
				}
			}
			else
            Utils.MostrarAlerta(Response, "No hay formatos asociados a los pedidos.\\n Asocie los formatos con los pedidos en Parametros Inventarios -> Tipos de Pedido");
		}

		protected void tipoPedido_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			if(tipoPedido.SelectedValue=="C")
				bind.PutDatasIntoDropDownList(ddlPrefPed,"SELECT pped_codigo,pped_codigo CONCAT ' - ' CONCAT pped_nombre FROM ppedido WHERE sfor_codigo IS NOT NULL and tped_codiuso in ('C','M') ORDER BY pped_codigo");
			else if(tipoPedido.SelectedValue=="P")
                bind.PutDatasIntoDropDownList(ddlPrefPed, "SELECT pped_codigo,pped_codigo CONCAT ' - ' CONCAT pped_nombre FROM ppedido WHERE sfor_codigo2 IS NOT NULL and tped_codiuso in ('P','M') ORDER BY pped_codigo");
            if (ddlPrefPed.Items.Count > 0)
                ddlPrefPed.Items.Insert(0, "Seleccione:..");
			CargarNumerosPedido();
		}

		private void CargarNumerosPedido()
		{
			DatasToControls bind=new DatasToControls();
			if(tipoPedido.SelectedValue=="C")
				bind.PutDatasIntoDropDownList(ddlNumPed,"SELECT mped_numepedi FROM mpedidoitem WHERE pped_codigo='"+ddlPrefPed.SelectedValue+"' AND (mped_claseregi='C' OR mped_claseregi='M') ORDER BY mped_numepedi");
			else if(tipoPedido.SelectedValue=="P")
				bind.PutDatasIntoDropDownList(ddlNumPed,"SELECT mped_numepedi FROM mpedidoitem WHERE pped_codigo='"+ddlPrefPed.SelectedValue+"' AND mped_claseregi='"+tipoPedido.SelectedValue+"' ORDER BY mped_numepedi");
		}

		protected void ddlPrefPed_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CargarNumerosPedido();
		}
	}
}
