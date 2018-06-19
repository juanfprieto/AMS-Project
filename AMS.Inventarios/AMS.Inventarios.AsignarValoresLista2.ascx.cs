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
using AMS.Forms;
using AMS.DB;
using AMS.Tools;


namespace AMS.Inventarios
{
	public partial class AsignaValoresLista : System.Web.UI.UserControl 
	{
		#region Atributos
		protected DataTable dtFiltrosValor;
		
		#endregion
		
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				Session.Clear();
				LlenarDdlFiltros(this.ddlParametro);
				LlenarDdlValor(this.ddlValor,this.ddlParametro.SelectedValue.ToUpper());
				PrepararDtFiltrosValor();
				BindDgFiltrosValor();
			}
			if(Session["dtFiltrosValor"]==null)
				PrepararDtFiltrosValor();
			else
				dtFiltrosValor = (DataTable)Session["dtFiltrosValor"];
		}
		
		protected void CambioGrupoItems(Object Sender, EventArgs e)
		{
			if(this.rblTipoItems.SelectedValue == "T")
				this.plFiltros.Visible = false;
			else if(this.rblTipoItems.SelectedValue == "F")
				this.plFiltros.Visible = true;
		}
		
		protected void CambioParametro(Object Sender, EventArgs e)
		{
			this.LlenarDdlValor(this.ddlValor,this.ddlParametro.SelectedValue.ToUpper());
		}
		
		protected void DgFiltroValorDelete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				this.dtFiltrosValor.Rows[e.Item.DataSetIndex].Delete();
				this.BindDgFiltrosValor();
                
			}
            catch (Exception ex) { Utils.MostrarAlerta(Response, "No se ha podido eliminar el item!");lb.Text += ex.ToString(); }
		}
		
		protected void AgregarFiltro(Object Sender, EventArgs e)
		{
			//Revisamos que no se encuentro dentro de la tabla
			DataRow[] selection = this.dtFiltrosValor.Select("FILTRO='"+this.ddlParametro.SelectedItem.Text+"' AND VALOR='"+this.ddlValor.SelectedValue+"'");
			if(selection.Length>0)
                Utils.MostrarAlerta(Response, "Ya se ha agregado este filtro, con este valor anteriormente!");
			else
			{
				DataRow fila = this.dtFiltrosValor.NewRow();
				fila[0] = this.ddlParametro.SelectedItem.Text;
				fila[1] = this.ddlValor.SelectedValue;
				this.dtFiltrosValor.Rows.Add(fila);
				this.BindDgFiltrosValor();
			}
		}
		
		protected void ActualizarListaPrecios(Object Sender, EventArgs e)
		{
			if(this.plFiltros.Visible && this.dtFiltrosValor.Rows.Count==0)
                Utils.MostrarAlerta(Response, "Por favor ingrese al menos un filtro");
			else
			{
				ListaPrecios listaActualizar = new ListaPrecios(Request.QueryString["codLista"]);
				bool status = true;
				DataTable dtOut = listaActualizar.ActualizacionValores(this.plFiltros.Visible,this.dtFiltrosValor,Request.QueryString["valBase"],Request.QueryString["tipOper"],Request.QueryString["valOper"],ref status);
				if(status)
				{
					dgResultado.DataSource = dtOut;
					dgResultado.DataBind();
					DatasToControls.JustificacionGrilla(dgResultado,dtOut);
					plProceso.Visible = false;
					plResultado.Visible = true;
				}
				else
					lb.Text += listaActualizar.ProcessMsg;
			}
		}
		
		protected void VolverAdmin(Object Sender, EventArgs e)
		{
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
			Response.Redirect(""+indexPage+"?process=Inventarios.ListaPrecios");
		}
		
		#endregion
		
		#region Otros
		
		protected void LlenarDdlFiltros(DropDownList ddl)
		{
			ddl.Items.Clear();
			ddl.Items.Add(new ListItem("Linea","plineaitem"));
			ddl.Items.Add(new ListItem("Marca","pmarcaitem"));
			ddl.Items.Add(new ListItem("Familia","pfamiliaitem"));
			ddl.Items.Add(new ListItem("Origen","torigenitem"));
			ddl.Items.Add(new ListItem("Grupo Vehiculo","pgrupocatalogo"));
			ddl.Items.Add(new ListItem("Descuento","pdescuentoitem"));
		}
		
		protected void LlenarDdlValor(DropDownList ddl ,string tablaValor)
		{
			DatasToControls bind = new DatasToControls();
			string campo0 = DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+tablaValor+"' AND COLNO=0");
			string campo1 = DBFunctions.SingleData("SELECT name FROM sysibm.syscolumns WHERE tbname='"+tablaValor+"' AND COLNO=1");
			if(tablaValor.IndexOf("DESCU")==-1)
				bind.PutDatasIntoDropDownList(ddl,"SELECT "+campo0+","+campo1+" FROM "+tablaValor);
			else
				bind.PutDatasIntoDropDownList(ddl,"SELECT "+campo0+" FROM "+tablaValor);
		}
		
		protected void PrepararDtFiltrosValor()
		{
			this.dtFiltrosValor = new DataTable();
			dtFiltrosValor.Columns.Add(new DataColumn("FILTRO",System.Type.GetType("System.String")));//0
			dtFiltrosValor.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//1
		}
		
		protected void BindDgFiltrosValor()
		{
			this.dgFiltrosValor.DataSource = this.dtFiltrosValor;
			this.dgFiltrosValor.DataBind();
			Session["dtFiltrosValor"] = dtFiltrosValor;
		}
		
		#endregion
		
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
