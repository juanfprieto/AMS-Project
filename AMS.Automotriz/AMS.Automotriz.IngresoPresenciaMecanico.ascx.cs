
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.Tools;

namespace AMS.Automotriz
{

	/// <summary>
	///		Descripción breve de AMS_Automotriz_IngresoPresenciaMecanico.
	/// </summary>
	public partial class AMS_Automotriz_IngresoPresenciaMecanico : System.Web.UI.UserControl
	{
		#region Propiedades
		protected System.Web.UI.WebControls.RequiredFieldValidator rqMecanico;
		protected System.Web.UI.WebControls.ValidationSummary vsTotal;
		private DatasToControls bind = new DatasToControls();
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Automotriz_IngresoPresenciaMecanico));
			if(!IsPostBack)
			{
				lbFechHorSis.Text = DateTime.Now.GetDateTimeFormats(new CultureInfo("es-CO"))[11];
				bind.PutDatasIntoDropDownList(ddlMecanico,"SELECT pven_codigo,pven_nombre FROM pvendedor WHERE tvend_codigo = 'MG' AND pven_vigencia = 'V' ORDER BY pven_nombre");
				ddlMecanico.Items.Insert(0,new ListItem("Seleccione ...",string.Empty));
				bind.PutDatasIntoDropDownList(ddlRazonSalida,"SELECT prs_codigo, prs_descripcion FROM prazonsalida ORDER BY prs_descripcion ASC");
				ddlRazonSalida.Items.Insert(0,new ListItem("Seleccione ...",string.Empty));
				btnAceptar.Attributes.Add("onclick","return ValidarEnvio();");
			}
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			if(DBFunctions.SingleData("SELECT pven_clave FROM pvendedor WHERE pven_codigo='"+ddlMecanico.SelectedValue+"'") == tbPass.Text)
			{
				if(hdCodEstado.Value == "E")
					DBFunctions.NonQuery("INSERT INTO dpresenciamecanico(pven_codigo,dprm_fecha,dprm_hora,dprm_evento,susu_usuario,prs_codigo) "+
																 "VALUES('"+ddlMecanico.SelectedValue+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("hh:mm:ss")+"','S','"+HttpContext.Current.User.Identity.Name+"',"+ddlRazonSalida.SelectedValue+")");
				else if(hdCodEstado.Value == "S")
					DBFunctions.NonQuery("INSERT INTO dpresenciamecanico(pven_codigo,dprm_fecha,dprm_hora,dprm_evento,susu_usuario) "+
																 "VALUES('"+ddlMecanico.SelectedValue+"','"+DateTime.Now.ToString("yyyy-MM-dd")+"','"+DateTime.Now.ToString("hh:mm:ss")+"','S','"+HttpContext.Current.User.Identity.Name+"')");
				ddlMecanico.SelectedIndex = 0;
				lbUltimoEstado.Text = String.Empty;
				hdCodEstado.Value = String.Empty;
			}
			else
			{
                Utils.MostrarAlerta(Response, "Clave Invalida");
				lbUltimoEstado.Text = (ConsultaUltimoEstadoMecanico(ddlMecanico.SelectedValue).Split('@'))[1];
			}
		}

		#endregion

		#region Metodos
		#endregion

		#region Metodos Ajax

		[Ajax.AjaxMethod]
		public string ConsultaUltimoEstadoMecanico(string codigoMecanico)
		{
			string codigoUltimoEstado = DBFunctions.SingleData("SELECT dprm_evento FROM dpresenciamecanico where dprm_indice = (SELECT MAX(dprm_indice) FROM dpresenciamecanico WHERE pven_codigo='"+codigoMecanico+"')");
			string fecha = DBFunctions.SingleData("SELECT dprm_fecha FROM dpresenciamecanico where dprm_indice = (SELECT MAX(dprm_indice) FROM dpresenciamecanico WHERE pven_codigo='"+codigoMecanico+"')").Trim();
			string hora = DBFunctions.SingleData("SELECT dprm_hora FROM dpresenciamecanico where dprm_indice = (SELECT MAX(dprm_indice) FROM dpresenciamecanico WHERE pven_codigo='"+codigoMecanico+"')").Trim();
			if(codigoUltimoEstado == string.Empty)
				codigoUltimoEstado = "S@Salida";
			else if(codigoUltimoEstado == "S")
				codigoUltimoEstado += "@Ultima Salida : " + Convert.ToDateTime(fecha+" "+hora).GetDateTimeFormats(new CultureInfo("es-CO"))[11];
			else if(codigoUltimoEstado == "E")
				codigoUltimoEstado += "@Ultima Entrada : " + Convert.ToDateTime(fecha+" "+hora).GetDateTimeFormats(new CultureInfo("es-CO"))[11];
			return codigoUltimoEstado;
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
