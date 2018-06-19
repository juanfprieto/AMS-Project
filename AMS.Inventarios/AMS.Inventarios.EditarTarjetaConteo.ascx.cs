using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class AMS_Inventarios_EditarTarjetaConteo : System.Web.UI.UserControl
	{
		#region Atributos
		private DatasToControls bind = new DatasToControls();
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private Regex regexEntero = new Regex("(?:\\+|-)?\\d+");
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Inventarios_EditarTarjetaConteo));	
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlInventarios,"SELECT INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),COUNT(DINV.dinv_mite_codigo) "+
					"FROM minventariofisico INF INNER JOIN dinventariofisico DINV ON INF.pdoc_codigo = DINV.pdoc_codigo AND INF.minf_numeroinv = DINV.minf_numeroinv "+
					"WHERE  INF.minf_fechacierre IS NULL AND INF.minf_fechainicio <= '"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND minf_fechacierre is null "+
					"GROUP BY INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)), CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)) "+
					"HAVING COUNT(DINV.dinv_mite_codigo) > 0 "+
					"ORDER BY CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10))");
				ddlInventarios.Items.Insert(0,new ListItem("Seleccione...",String.Empty));
				btnAceptar.Attributes.Add("onclick","return ValSelInvFis();");
				btnGuardarEdicion.Attributes.Add("onclick","return ValidarInformacionEdicion();");
			}
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			if(ddlInventarios.SelectedIndex > 0)
			{
				ddlInventarios.Enabled = false;
				pnlInfoProceso.Visible = true;
				string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
				int numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
                bind.PutDatasIntoDropDownList(ddlAlmacen, "SELECT palm_almacen,palm_descripcion FROM palmacen WHERE palm_almacen IN (SELECT dinv_palm_almacen FROM dinventariofisico WHERE pdoc_codigo='" + prefijoInventario + "' AND minf_numeroinv=" + numeroInventario + ") and tvig_vigencia='V' ORDER BY palm_descripcion");
				CargarUbicaciones(ddlUbicacion,ddlAlmacen);
			}
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.EditarTarjetaConteo");
		}

		protected void btnGuardarEdicion_Click(object sender, System.EventArgs e)
		{
			string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
			int numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
			if(hdNumeroTarjeta.Value == String.Empty)
			{
                Utils.MostrarAlerta(Response, "Por favor ingrese una tarjeta registrada en este inventario físico");
				return;
			}
			bool validaValores = true;
			string errores = String.Empty;
			int conteoRelacionado = Convert.ToInt32(hdConteoActual.Value);
			if(conteoRelacionado >= 2)
			{
				if(!regexEntero.IsMatch(tbEdicionConteo1.Text))
				{
					errores += "- Ingrese un valor valido para el primer conteo\\n";
					validaValores = false;
				}
			}
			if(conteoRelacionado >= 3)
			{
				if(!regexEntero.IsMatch(tbEdicionConteo2.Text))
				{
					errores += "- Ingrese un valor valido para el segundo conteo\\n";
					validaValores = false;
				}
			}
			if(conteoRelacionado >= 4)
			{
				if(!regexEntero.IsMatch(tbEdicionConteo3.Text))
				{
					errores += "- Ingrese un valor valido para el tercer conteo\\n";
					validaValores = false;
				}
			}
            string ubicacion = ddlUbicacion.SelectedValue; 
            if(ubicacion=="")
			{
		//		errores += "- Item sin Ubicacion\\n";
		//		validaValores = false;
				ubicacion = "0";   
			}
			if(!validaValores)
			{
                Utils.MostrarAlerta(Response, "" + errores + "");
				return;
			}
			string prefijo = String.Empty;
			if(conteoRelacionado == 2)
				prefijo += InventarioFisico.EditarValorConteoTarjeta(prefijoInventario,numeroInventario,hdCodRef.Value,Convert.ToInt32(hdNumeroTarjeta.Value),0,Convert.ToDouble(tbEdicionConteo1.Text),ddlAlmacen.SelectedValue,Convert.ToInt32(ubicacion));
			else if(conteoRelacionado == 3)
			{
				prefijo += InventarioFisico.EditarValorConteoTarjeta(prefijoInventario,numeroInventario,hdCodRef.Value,Convert.ToInt32(hdNumeroTarjeta.Value),0,Convert.ToDouble(tbEdicionConteo1.Text),ddlAlmacen.SelectedValue,Convert.ToInt32(ubicacion));
				prefijo += InventarioFisico.EditarValorConteoTarjeta(prefijoInventario,numeroInventario,hdCodRef.Value,Convert.ToInt32(hdNumeroTarjeta.Value),1,Convert.ToDouble(tbEdicionConteo2.Text),ddlAlmacen.SelectedValue,Convert.ToInt32(ubicacion));
			}
			else if(conteoRelacionado == 4)
			{
				prefijo += InventarioFisico.EditarValorConteoTarjeta(prefijoInventario,numeroInventario,hdCodRef.Value,Convert.ToInt32(hdNumeroTarjeta.Value),0,Convert.ToDouble(tbEdicionConteo1.Text),ddlAlmacen.SelectedValue,Convert.ToInt32(ubicacion));
				prefijo += InventarioFisico.EditarValorConteoTarjeta(prefijoInventario,numeroInventario,hdCodRef.Value,Convert.ToInt32(hdNumeroTarjeta.Value),1,Convert.ToDouble(tbEdicionConteo2.Text),ddlAlmacen.SelectedValue,Convert.ToInt32(ubicacion));
				prefijo += InventarioFisico.EditarValorConteoTarjeta(prefijoInventario,numeroInventario,hdCodRef.Value,Convert.ToInt32(hdNumeroTarjeta.Value),2,Convert.ToDouble(tbEdicionConteo3.Text),ddlAlmacen.SelectedValue,Convert.ToInt32(ubicacion));
			}
			if(prefijo != String.Empty)
                Utils.MostrarAlerta(Response, ""+prefijo+"");
			else
			{
				tbNumeroTarjeta.Text  = hdNumeroTarjeta.Value = lbCodigoReferencia.Text = hdCodRef.Value = lbNombreReferencia.Text = lbConteoActual.Text = hdConteoActual.Value = String.Empty;
				tbEdicionConteo1.Text = tbEdicionConteo2.Text = tbEdicionConteo3.Text = "0";
				ddlAlmacen.SelectedIndex = 0;
				CargarUbicaciones(ddlUbicacion,ddlAlmacen);
			}
		}
		#endregion

		#region Metodos
		private void CargarUbicaciones(DropDownList ddlRelacionado, DropDownList ddlAlmacenRelacionado)
		{
			DataTable dtUbicaciones = InventarioFisico.ConsultarUbicacionesUltimoNivelPorAlmacen(ddlAlmacenRelacionado.SelectedValue);
			ddlRelacionado.DataSource = dtUbicaciones;
			ddlRelacionado.DataValueField = dtUbicaciones.Columns[0].ColumnName;
			ddlRelacionado.DataTextField = dtUbicaciones.Columns[1].ColumnName;
			ddlRelacionado.DataBind();
		}
		#endregion

		#region Metodos Ajax
		[Ajax.AjaxMethod]
		public DataTable ConsultaUbicacionesPorAlmacen(string codigoAlmacen)
		{
			return InventarioFisico.ConsultarUbicacionesUltimoNivelPorAlmacen(codigoAlmacen);
		}
		
		[Ajax.AjaxMethod]
		public string TraerInformacionTarjetaEdicion(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			string informacionTarjeta = String.Empty;
			ItemsInventarioFisico inst = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);
    //         if (inst.Conteo3 == -1 && inst.ConteoActual == 3) inst.Conteo3 = 0;
	 		if(inst.ConteoActual != -1)
				informacionTarjeta = inst.NumeroTarjeta+"&"+inst.CodigoItemModificado+"&"+inst.CodigoItemRelacionado+"&"+inst.NombreItemRelacionado+"&"+inst.CodigoAlmacen+"&"+inst.CodigoUbicacionInicial+"&"+inst.ConteoActual+"&"+inst.Conteo1+"&"+inst.Conteo2+"&"+inst.Conteo3;
			return informacionTarjeta;
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
