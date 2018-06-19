using System;
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
	public partial class PrincipalRelacionItemsGrupo : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		protected DatasToControls bind = new DatasToControls();
		protected string valueDDLReferenciaEdicion;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(PrincipalRelacionItemsGrupo));
			if(!IsPostBack)
			{
				if(Request.QueryString["inst"]!=null)
                    Utils.MostrarAlerta(Response, "Relación Grabada Satisfactoriamente");
				bind.PutDatasIntoDropDownList(ddlGrupo,"SELECT pgru_grupo, pgru_grupo FROM pgrupocatalogo");
				ddlGrupo.Items.Insert(0,new ListItem("Selecciona Uno..",""));
			}
			RevisionValoresDDLReferenciaEdicion();
		}

		protected void ingresarTorre_Click(object sender, System.EventArgs e)
		{ 
			Response.Redirect("" + indexPage + "?process=Inventarios.RelacionItemsGrupos");
		}

		protected void btnEditar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.RelacionItemsGrupos&modEdit=1&codGru="+ddlGrupo.SelectedValue+"&codItemOri="+ddlReferencia.SelectedValue+"&codItemMod="+ddlReferencia.SelectedItem.Text+"");
		}

		[Ajax.AjaxMethod()]
		public DataSet CambioGrupoCarga(string grupoNuevo)
		{
			return ConsultarItemsRelacionadosGrupo(grupoNuevo);
		}

		private DataSet ConsultarItemsRelacionadosGrupo(string grupoNuevo)
		{
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.YES,"SELECT MIT.mite_codigo, DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) FROM mitems MIT INNER JOIN plineaitem PLIN ON MIT.plin_codigo=PLIN.plin_codigo INNER JOIN mitemsgrupo MIG ON MIT.mite_codigo=MIG.mite_codigo WHERE MIG.pgru_grupo='"+grupoNuevo+"' ORDER By MIT.mite_codigo");
			DataSet dsSalida = new DataSet();
			DataTable dtSalida = new DataTable();
			dtSalida.Columns.Add(new DataColumn("CodigoOriginal",typeof(string)));
			dtSalida.Columns.Add(new DataColumn("CodigoModificado",typeof(string)));
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow dr = dtSalida.NewRow();
				dr[0] = ds.Tables[0].Rows[i][0];
				dr[1] = ds.Tables[0].Rows[i][1];
				dtSalida.Rows.Add(dr);
			}
			dsSalida.Tables.Add(dtSalida);
			return dsSalida;
		}

		protected void RevisionValoresDDLReferenciaEdicion()
		{
			valueDDLReferenciaEdicion = Request.Form[ddlReferencia.UniqueID];
			if(ddlGrupo.SelectedValue != "")
			{
				bind.PutDatasIntoDropDownList(ddlReferencia,"SELECT MIT.mite_codigo, DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) FROM mitems MIT INNER JOIN plineaitem PLIN ON MIT.plin_codigo=PLIN.plin_codigo INNER JOIN mitemsgrupo MIG ON MIT.mite_codigo=MIG.mite_codigo WHERE MIG.pgru_grupo='"+ddlGrupo.SelectedValue+"' ORDER By MIT.mite_codigo");
				ddlReferencia.SelectedValue = valueDDLReferenciaEdicion;
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
	}
}
