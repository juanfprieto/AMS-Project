using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using AMS.Tools;

namespace AMS.Finanzas.Tesoreria
{
	public partial  class ConsultaCheques : System.Web.UI.UserControl
	{
		protected DataTable dtInfo;
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)	
			{
				DatasToControls bind=new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlPrefRC, "SELECT pdoc_codigo,pdoc_codigo ||'-'|| pdoc_descripcion FROM pdocumento WHERE tdoc_tipodocu IN('RC','CE','RP') and tvig_vigencia = 'V' ");
				bind.PutDatasIntoDropDownList(ddlNumRC,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+ddlPrefRC.SelectedValue+"'");
				bind.PutDatasIntoDropDownList(ddlBanco,"SELECT pban_codigo,pban_codigo||'-'||pban_nombre FROM pbanco WHERE tban_codigo='B'");
				tbNit.Attributes.Add("onDblClick","ModalDialog(this,'SELECT DISTINCT CASE WHEN MCAJ.mnit_nit=MCAJ.mnit_nitben THEN MCAJ.mnit_nitben ELSE MCAJ.mnit_nit END AS Nit,MNIT.mnit_apellidos ||\\' \\'||COALESCE(MNIT.mnit_apellido2,\\'\\')||\\' \\'||MNIT.mnit_nombres||\\' \\'||COALESCE(MNIT.mnit_nombre2,\\'\\') AS NOMBRE FROM dbxschema.mcaja MCAJ,dbxschema.mnit MNIT,dbxschema.mnit MNIT2 WHERE MCAJ.mnit_nit=MNIT.mnit_nit AND MCAJ.mnit_nitben=MNIT2.mnit_nit ORDER BY Nit ASC',new Array())");
				tbCheque.Attributes.Add("onDblClick","CargarCheques(this,'"+ddlBanco.ClientID+"')");
				rbCheque.Attributes.Add("onClick","MostrarTabla('"+tblNit.ClientID+"','"+tblCheque.ClientID+"','"+tblRC.ClientID+"')");
				rbNit.Attributes.Add("onClick","MostrarTabla('"+tblNit.ClientID+"','"+tblCheque.ClientID+"','"+tblRC.ClientID+"')");
				rbRC.Attributes.Add("onClick","MostrarTabla('"+tblNit.ClientID+"','"+tblCheque.ClientID+"','"+tblRC.ClientID+"')");
			}
		}
				
		protected void btnBuscar_Click(object Sender,EventArgs e)
		{
			DataSet ds;
			if(rbNit.Checked)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MCA.mnit_nitben AS Nit,MNI.mnit_apellidos || ' ' || COALESCE(MNI.mnit_apellido2,'') || ' ' || MNI.mnit_nombres || ' ' || COALESCE(MNI.mnit_nombre2,'') AS Nombre, MCP.pdoc_codigo AS Prefijo,MCP.mcaj_numero AS Numero,PBA.pban_nombre AS Banco,MCP.mcpag_numerodoc AS Cheque,TES.test_nombre AS Estado,MCP.mcpag_fecha AS Fecha,MCP.mcpag_valor AS Valor,MCP.mcpag_devoluciones AS Devoluciones,MCP.mcpag_prorrogas AS Prorrogas FROM dbxschema.mcajapago MCP,dbxschema.testadocajapago TES,dbxschema.pbanco PBA,dbxschema.mcaja MCA,dbxschema.mnit MNI WHERE MCP.pdoc_codigo=MCA.pdoc_codigo AND MCP.mcaj_numero=MCA.mcaj_numero AND MCP.test_estado=TES.test_estado AND MCP.pban_codigo=PBA.pban_codigo AND MNI.mnit_nit=MCA.mnit_nitben AND MCA.mnit_nitben='"+tbNit.Text+"' ORDER BY MCP.mcpag_fecha DESC");
				if(ds.Tables[0].Rows.Count!=0)
				{
					dgBusqueda.DataSource=ds.Tables[0];
					dgBusqueda.DataBind();
				}
				else
                Utils.MostrarAlerta(Response, "No se encontraron coincidencias con los parámetros de búsqueda especificados");
				tblNit.Style.Remove("DISPLAY");
				tblCheque.Style.Add("DISPLAY","none");
				tblRC.Style.Add("DISPLAY","none");
			}
			else if(rbCheque.Checked)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MCA.mnit_nitben AS Nit,MNI.mnit_apellidos || ' ' || COALESCE(MNI.mnit_apellido2,'') || ' ' || MNI.mnit_nombres || ' ' || COALESCE(MNI.mnit_nombre2,'') AS Nombre, MCP.pdoc_codigo AS Prefijo,MCP.mcaj_numero AS Numero,PBA.pban_nombre AS Banco,MCP.mcpag_numerodoc AS Cheque,TES.test_nombre AS Estado,MCP.mcpag_fecha AS Fecha,MCP.mcpag_valor AS Valor,MCP.mcpag_devoluciones AS Devoluciones,MCP.mcpag_prorrogas AS Prorrogas FROM dbxschema.mcajapago MCP,dbxschema.testadocajapago TES,dbxschema.pbanco PBA,dbxschema.mcaja MCA,dbxschema.mnit MNI WHERE MCP.pdoc_codigo=MCA.pdoc_codigo AND MCP.mcaj_numero=MCA.mcaj_numero AND MCP.test_estado=TES.test_estado AND MCP.pban_codigo=PBA.pban_codigo AND MNI.mnit_nit=MCA.mnit_nitben AND MCP.pban_codigo='"+ddlBanco.SelectedValue+"' AND MCP.mcpag_numerodoc='"+tbCheque.Text+"' ORDER BY MCP.mcpag_fecha DESC");
				if(ds.Tables[0].Rows.Count!=0)
				{
					dgBusqueda.DataSource=ds.Tables[0];
					dgBusqueda.DataBind();
				}
				else
		        Utils.MostrarAlerta(Response, "No se encontraron coincidencias con los parámetros de búsqueda especificados");
				tblCheque.Style.Remove("DISPLAY");
				tblNit.Style.Add("DISPLAY","none");
				tblRC.Style.Add("DISPLAY","none");
			}
			else if(rbRC.Checked)
			{
				ds=new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MCA.mnit_nitben AS Nit,MNI.mnit_apellidos || ' ' || COALESCE(MNI.mnit_apellido2,'') || ' ' || MNI.mnit_nombres || ' ' || COALESCE(MNI.mnit_nombre2,'') AS Nombre, MCP.pdoc_codigo AS Prefijo,MCP.mcaj_numero AS Numero,PBA.pban_nombre AS Banco,MCP.mcpag_numerodoc AS Cheque,TES.test_nombre AS Estado,MCP.mcpag_fecha AS Fecha,MCP.mcpag_valor AS Valor,MCP.mcpag_devoluciones AS Devoluciones,MCP.mcpag_prorrogas AS Prorrogas FROM dbxschema.mcajapago MCP,dbxschema.testadocajapago TES,dbxschema.pbanco PBA,dbxschema.mcaja MCA,dbxschema.mnit MNI WHERE MCP.pdoc_codigo=MCA.pdoc_codigo AND MCP.mcaj_numero=MCA.mcaj_numero AND MCP.test_estado=TES.test_estado AND MCP.pban_codigo=PBA.pban_codigo AND MNI.mnit_nit=MCA.mnit_nitben AND MCA.pdoc_codigo='"+ddlPrefRC.SelectedValue+"' AND MCA.mcaj_numero="+ddlNumRC.SelectedValue+" ORDER BY MCP.mcpag_fecha DESC");
				if(ds.Tables[0].Rows.Count!=0)
				{
					dgBusqueda.DataSource=ds.Tables[0];
					dgBusqueda.DataBind();
				}
				else
                Utils.MostrarAlerta(Response, "No se encontraron coincidencias con los parámetros de búsqueda especificados");
				tblRC.Style.Remove("DISPLAY");
				tblCheque.Style.Add("DISPLAY","none");
				tblNit.Style.Add("DISPLAY","none");
			}
			btnBuscar.Enabled=true;
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

		protected void ddlPrefRC_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlNumRC,"SELECT mcaj_numero FROM mcaja WHERE pdoc_codigo='"+ddlPrefRC.SelectedValue+"'");
			tblRC.Style.Remove("DISPLAY");
			tblCheque.Style.Add("DISPLAY","none");
			tblNit.Style.Add("DISPLAY","none");
			btnBuscar.Enabled=true;
		}
	}
}
