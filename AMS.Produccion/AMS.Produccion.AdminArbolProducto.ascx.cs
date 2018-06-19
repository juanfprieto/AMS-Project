using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using AMS.Forms;
using AMS.DB;
using AMS.Documentos;
using Ajax;
using AMS.Tools;

namespace AMS.Produccion
{
	/// <summary>
	///		Descripción breve de AMS_Produccion_AdminArbolProducto.
	/// </summary>
	public partial class AMS_Produccion_AdminArbolProducto : System.Web.UI.UserControl
	{
		#region Propiedades

		protected System.Web.UI.WebControls.CustomValidator cvNuevo;
		protected System.Web.UI.WebControls.ValidationSummary vsTotal;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		private DatasToControls bind = new DatasToControls();

		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Produccion_AdminArbolProducto));
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlLineas, "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				bind.PutDatasIntoDropDownList(ddlLineasConsult, "SELECT plin_codigo CONCAT '-' CONCAT plin_tipo, plin_nombre FROM plineaitem");
				tbItemCrear.Attributes.Add("ondblclick","MostrarRefs("+tbItemCrear.ClientID+","+ddlLineas.ClientID+");");
				tbItemCrearConsulta.Attributes.Add("ondblclick","MostrarRefs("+tbItemCrearConsulta.ClientID+","+ddlLineasConsult.ClientID+");");
				tbItemCrear.Attributes.Add("onkeyup","ItemMask("+tbItemCrear.ClientID+","+ddlLineas.ClientID+");");
				tbItemCrearConsulta.Attributes.Add("onkeyup","ItemMask("+tbItemCrearConsulta.ClientID+","+ddlLineasConsult.ClientID+");");
				ddlLineas.Attributes.Add("onchange","ChangeLine("+ddlLineas.ClientID+","+tbItemCrear.ClientID+");");
				ddlLineasConsult.Attributes.Add("onchange","ChangeLine("+ddlLineasConsult.ClientID+","+tbItemCrearConsulta.ClientID+");");
				btnIngresar.Attributes.Add("onclick","return ValidarExistenciaValor();");
				btnIngresarConsulta.Attributes.Add("onclick","return ValidarExistenciaValor2();");
			}
		}

		protected void btnIngresar_Click(object sender, System.EventArgs e)
		{
			if(VerificarExistenciaReferencia(tbItemCrear.Text,(ddlLineas.SelectedValue.Split('-'))[1]))
			{
				string codigoReal = "";
				Referencias.Guardar(tbItemCrear.Text,ref codigoReal,(ddlLineas.SelectedValue.Split('-'))[1]);
				Response.Redirect("" + indexPage + "?process=Produccion.ArbolProducto&codOri="+codigoReal+"&codMod="+tbItemCrear.Text);
			}
			else
				Page.RegisterStartupScript("status","'La referencia "+tbItemCrear.Text+" no se encuentra registrada dentro del sistema. Por favor revise!'");
		}

		protected void btnIngresarConsulta_Click(object sender, System.EventArgs e)
		{
			int salida = VerificarArbolReferencia(tbItemCrearConsulta.Text,(ddlLineasConsult.SelectedValue.Split('-'))[1]);
			if(salida == -1)
			{
				string codigoReal = "";
				Referencias.Guardar(tbItemCrearConsulta.Text,ref codigoReal,(ddlLineasConsult.SelectedValue.Split('-'))[1]);
				Response.Redirect("" + indexPage + "?process=Produccion.ConsultaArbol&codOri="+codigoReal+"&codMod="+tbItemCrearConsulta.Text);
			}
			else if(salida == 1)
				Page.RegisterStartupScript("status","'La referencia "+tbItemCrear.Text+" no se encuentra registrada dentro del sistema. Por favor revise!'");
			else if(salida == 2)
				Page.RegisterStartupScript("status","'La referencia "+tbItemCrear.Text+" no se ha configurado el arbol de producción. Por favor revise!'");
		}

		#endregion

		#region Metodos Ajax
		
		[Ajax.AjaxMethod]
		public bool VerificarExistenciaReferencia(string codigoReferencia, string lineaBodega)
		{
			//Se realiza la conversion de la referencia a su valor originalç
			string codigoReal = "";
			Referencias.Guardar(codigoReferencia,ref codigoReal,lineaBodega);
			return DBFunctions.RecordExist("SELECT mite_codigo FROM mitems WHERE mite_codigo='"+codigoReal+"'");
		}

		[Ajax.AjaxMethod]
		public int VerificarArbolReferencia(string codigoReferenciaArbol, string lineaBodegaArbol)
		{
			int retorno = -1;
			//Se realiza la conversion de la referencia a su valor originalç
			string codigoReal = "";
			Referencias.Guardar(codigoReferenciaArbol,ref codigoReal,lineaBodegaArbol);
			bool revisionReferencia = DBFunctions.RecordExist("SELECT mite_codigo FROM mitems WHERE mite_codigo='"+codigoReal+"'");
			bool revisionArbol = DBFunctions.RecordExist("SELECT map_secuencia FROM marbolproducto WHERE mite_codparte = '"+codigoReal+"'");
			if(revisionReferencia && revisionArbol)
				retorno = -1;
			else if(!revisionReferencia)
				retorno = 1;
			else if(!revisionArbol)
				retorno = 2;
			return retorno;
		}
		#endregion

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
	}
}

