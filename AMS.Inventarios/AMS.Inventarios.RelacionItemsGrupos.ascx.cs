using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Documentos;
using Ajax;
using AMS.Tools;

namespace AMS.Inventarios
{
	public partial class RelacionItemsGrupos : System.Web.UI.UserControl
	{
		private DatasToControls bind = new DatasToControls();
		private DataTable dtCatalogos;
		private string indexPage=ConfigurationManager.AppSettings["MainIndexPage"];

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(RelacionItemsGrupos));
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlGrupos,"SELECT pgru_grupo, pgru_grupo || '-' || pgru_nombre FROM pgrupocatalogo");
				bind.PutDatasIntoDropDownList(ddlLinea,"SELECT plin_codigo || '-' || plin_tipo, plin_nombre FROM plineaitem");
				tbItems.Attributes.Add("ondblclick","MuestreoReferencias(this,"+ddlLinea.ClientID+");");
				tbItems.Attributes.Add("onkeyup","ItemMask("+tbItems.ClientID+","+ddlLinea.ClientID+");");
				if(Request.QueryString["modEdit"] != null)
					EstablecerModoEdicion();
			}
			if(ViewState["dtCatalogos"] != null)
				dtCatalogos = (DataTable)ViewState["dtCatalogos"];
			RevisionBotonAceptar();
		}

		private void EstablecerModoEdicion()
		{
			hdModo.Value = "edicion";
			ddlGrupos.SelectedValue = hdCodigoGrupo.Value = Request.QueryString["codGru"];
			tbItems.Text = Request.QueryString["codItemMod"];
			ddlLinea.SelectedValue = DBFunctions.SingleData("SELECT DISTINCT PL.plin_codigo CONCAT '-' CONCAT PL.plin_tipo FROM mitems as MIT INNER JOIN plineaitem as PL ON MIT.plin_codigo = PL.plin_codigo WHERE MIT.mite_codigo='"+Request.QueryString["codItemOri"]+"'");
			ddlGrupos.Enabled = ddlLinea.Enabled = tbItems.Enabled = btnMostrarCatalogos.Enabled = false;
			tbItems.Attributes.Remove("ondblclick");
			tbCantidad.Text = DBFunctions.SingleData("SELECT mig_cantidaduso FROM mitemsgrupo WHERE mite_codigo='"+Request.QueryString["codItemOri"]+"' AND pgru_grupo='"+ddlGrupos.SelectedValue+"'");
			BindDgCatalogos();
		}

		protected void btnMostrarCatalogos_Click(object sender, System.EventArgs e)
		{
			string codI = "";
			Referencias.Guardar(tbItems.Text,ref codI,(ddlLinea.SelectedValue.Split('-'))[1]);
			if(ddlGrupos.SelectedValue != "" && codI != "")
			{
				hdCodigoItem.Value = codI;
				if(DBFunctions.RecordExist("SELECT mite_codigo FROM mitemsgrupo WHERE pgru_grupo = '"+ddlGrupos.SelectedValue+"' AND mite_codigo = '"+codI+"'"))
				{
					hdModo.Value = "edicionNuevo";
					ddlGrupos.Enabled = ddlLinea.Enabled = tbItems.Enabled = btnMostrarCatalogos.Enabled = false;
					tbItems.Attributes.Remove("ondblclick");
					tbCantidad.Text = DBFunctions.SingleData("SELECT mig_cantidaduso FROM mitemsgrupo WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+ddlGrupos.SelectedValue+"'");
				}
				BindDgCatalogos();
				RevisionBotonAceptar();
			}
			else
                Utils.MostrarAlerta(Response, "Por favor seleccione un grupo de catalogos o ingrese una referencia valida");
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			if(hdCodigoGrupo.Value == "" || tbItems.Text == "")
                Utils.MostrarAlerta(Response, "Por Favor Revise los parametros para la creacion de relaciones entre items y grupos");
			else
			{
				dtModificarTablaInsercion();
				if(hdModo.Value == "")
					lb.Text = Items.CrearRelacionItemGrupo(tbItems.Text,(ddlLinea.SelectedValue.Split('-'))[1],ddlGrupos.SelectedValue,tbCantidad.Text,dtCatalogos);
				else
					lb.Text = Items.CrearRelacionItemGrupo(tbItems.Text,(ddlLinea.SelectedValue.Split('-'))[1],ddlGrupos.SelectedValue,tbCantidad.Text,dtCatalogos);
				if(lb.Text.IndexOf("Bien")!=-1)
					Response.Redirect(indexPage+"?process=Inventarios.PrincipalRelacionItemsGrupo&inst=1");
			}
		}

		#endregion

		#region Metodos

		private void BindDgCatalogos()
		{
			hdCodigoGrupo.Value = ddlGrupos.SelectedValue;
			dgCatalogosRelacionados.DataSource = dtCatalogos = Items.ConsultarCatalogosPorGrupo(ddlGrupos.SelectedValue);
			dgCatalogosRelacionados.DataBind();
			ViewState["dtCatalogos"] = dtCatalogos;
		}

		private void RevisionBotonAceptar()
		{
			if(hdCodigoGrupo.Value == "")
			{
				btnAceptar.Visible = false;
				btnAceptar.Attributes.Remove("onclick");
			}
			else
			{
				btnAceptar.Visible = true;
				btnAceptar.Attributes.Add("onclick","if(document.getElementById('"+tbItems.ClientID+"').value != ''){if(RelacionItemsGrupos.ConsultarRelacionExistente(document.getElementById('"+ddlGrupos.ClientID+"').value,(document.getElementById('"+ddlLinea.ClientID+"').value.split('-'))[0],document.getElementById('"+tbItems.ClientID+"').value)==true){alert('Esta relación ya existe');return false;}return confirm(' Grupo Catalogo Relacionado: "+hdCodigoGrupo.Value+"\\n Item Seleccionado:'+document.getElementById('"+tbItems.ClientID+"').value+' ');}else{alert('Por favor revisa los parametros de entrada');return false;}");
			}
		}

		private void dtModificarTablaInsercion()
		{
			dtCatalogos.Columns.Add(new DataColumn("cantidad",typeof(string)));
			dtCatalogos.Columns.Add(new DataColumn("ingreso",typeof(bool)));
			for(int i=0;i<dgCatalogosRelacionados.Items.Count;i++)
			{
				dtCatalogos.Rows[i]["cantidad"] = ((TextBox)dgCatalogosRelacionados.Items[i].Cells[3].FindControl("tbCantidadCata")).Text;
				if(((CheckBox)dgCatalogosRelacionados.Items[i].Cells[2].FindControl("chkAdd")).Checked)
					dtCatalogos.Rows[i]["ingreso"] = true;
				else
					dtCatalogos.Rows[i]["ingreso"] = false;
			}
		}

		private void dgCatalogosRelacionados_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				((CheckBox)e.Item.Cells[2].FindControl("chkAdd")).Attributes.Add("onclick","StateCantidadCatalogo("+((CheckBox)e.Item.Cells[2].FindControl("chkAdd")).ClientID+","+((TextBox)e.Item.Cells[3].FindControl("tbCantidadCata")).ClientID+");");
				if(hdModo.Value != "")
				{
					string codI = "";
					Referencias.Guardar(tbItems.Text, ref codI, (ddlLinea.SelectedValue.Split('-'))[1]);
					double cantidadUso = Items.CantidadItemsPorCatalogo(e.Item.Cells[0].Text,codI);
					if(cantidadUso > 0)
					{
						((CheckBox)e.Item.Cells[2].FindControl("chkAdd")).Checked = true;
						((TextBox)e.Item.Cells[3].FindControl("tbCantidadCata")).Text = cantidadUso.ToString();
						((TextBox)e.Item.Cells[3].FindControl("tbCantidadCata")).ReadOnly = false;
					}
					else
					{
						((CheckBox)e.Item.Cells[2].FindControl("chkAdd")).Checked = false;
						((TextBox)e.Item.Cells[3].FindControl("tbCantidadCata")).Text = "";
						((TextBox)e.Item.Cells[3].FindControl("tbCantidadCata")).ReadOnly = true;
					}
				}
			}
		}

		[Ajax.AjaxMethod()]
		public bool ConsultarRelacionExistente(string grupo, string codigoLinea, string idReferencia)
		{
			bool encontrado = false;
			string codI = "";
			Referencias.Guardar(idReferencia,ref codI,codigoLinea);
			if(DBFunctions.SingleData("SELECT mite_codigo FROM mitemsgrupo WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+grupo+"'") != "")
				encontrado = true;
			return encontrado;
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
			this.dgCatalogosRelacionados.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.dgCatalogosRelacionados_ItemDataBound);

		}
		#endregion
	}
}