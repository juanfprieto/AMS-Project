using System;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Reportes
{
	public partial class Final : System.Web.UI.UserControl
	{
		#region Atributos
		protected DataTable dtFltrRpt, dtFilas, dtFooter;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
		
		#region Eventos
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				lbTabla.Text = Request.QueryString["tabla"];
				lbConsulta.Text = Request.QueryString["sql"];
				Mostrar_Filas_Configuradas();
				if(Request.QueryString["filters"]!="")
					Mostrar_Filtros_Configurados(Request.QueryString["filters"]);
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlTablas,"SELECT name FROM sysibm.systables WHERE name like 'C%' AND creator = '"+ConfigurationManager.AppSettings["Schema"].ToUpper()+"' ORDER BY name");
				bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
			}
			if(Session["dtFilas"] == null)
				Preparar_dtFilas();
			else
				dtFilas = (DataTable)Session["dtFilas"];
			if(Session["dtFltrRpt"] == null)
				Preparar_dtFltrRpt();
			else
				dtFltrRpt = (DataTable)Session["dtFltrRpt"];
			if(Session["dtFooter"] == null)
				Preparar_dtFooter();
			else
				dtFooter = (DataTable)Session["dtFooter"];
		}
		
		protected void Cambio_Tabla(Object  Sender, EventArgs e)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCampos,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Campo No Comentado') FROM sysibm.syscolumns WHERE tbname='"+ddlTablas.SelectedValue+"'");
		}
		
		protected void Agregar_ValorTx(Object  Sender, EventArgs e)
		{
			if(tbTxtFooter.Text!="")
			{
				DataRow fila = dtFooter.NewRow();
				fila[0] = tbTxtFooter.Text;
				dtFooter.Rows.Add(fila);
				this.Bind_dgFooter();
				tbTxtFooter.Text = "";
			}
			else
            Utils.MostrarAlerta(Response, "Debe Ingresar Algun Valor Textual");
		}
		
		protected void Agregar_Valor(Object  Sender, EventArgs e)
		{
			DataRow fila = dtFooter.NewRow();
			fila[0] = "SELECT "+ddlCampos.SelectedValue+" FROM "+ddlTablas.SelectedValue;
			dtFooter.Rows.Add(fila);
			this.Bind_dgFooter();
		}
		
		protected void Guardar_Reporte(Object  Sender, EventArgs e)
		{
			//Debemos revisar que el nombre del reporte no sea vacio
			if(nombReporte.Text != "")
			{
				Reporte miReporte = new Reporte(dtFltrRpt,dtFilas,dtFooter);
				miReporte.NombreReporte = nombReporte.Text;
				miReporte.SqlReporte = Request.QueryString["sql"];
				miReporte.Masks = Request.QueryString["mask"];
				int resultadoGrabacion = miReporte.Grabar_Reporte();
				if(resultadoGrabacion!=-1)
					Response.Redirect("" + indexPage + "?process=Reportes.MenuReporte&idReporte="+resultadoGrabacion.ToString()+"");
				else
					lb.Text = "<br>Error : "+miReporte.ProcessMsg;
			}
			else
            Utils.MostrarAlerta(Response, "El nombre del reporte es vacio");
		}
		
		
		#endregion
		
		#region Otros
		
		protected void Preparar_dtFooter()
		{
			dtFooter = new DataTable();
			dtFooter.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//0
		}
		
		protected void Bind_dgFooter()
		{
			Session["dtFooter"] = dtFooter;
			dgFooter.DataSource = dtFooter;
			dgFooter.DataBind();
		}
		
		protected void Preparar_dtFltrRpt()
		{
			dtFltrRpt = new DataTable();
			dtFltrRpt.Columns.Add(new DataColumn("TABLA",System.Type.GetType("System.String")));//0
			dtFltrRpt.Columns.Add(new DataColumn("CAMPO",System.Type.GetType("System.String")));//1
			dtFltrRpt.Columns.Add(new DataColumn("ETIQUETA",System.Type.GetType("System.String")));//2
			dtFltrRpt.Columns.Add(new DataColumn("TIPOCOMPARACION",System.Type.GetType("System.String")));//3
			dtFltrRpt.Columns.Add(new DataColumn("TIPODATO",System.Type.GetType("System.String")));//4
			dtFltrRpt.Columns.Add(new DataColumn("CONTROLASOCIADO",System.Type.GetType("System.String")));//5
			dtFltrRpt.Columns.Add(new DataColumn("VALORINTERPRETADO",System.Type.GetType("System.String")));//6
		}
		
		protected void Preparar_dtFilas()
		{
			dtFilas = new DataTable();
			dtFilas.Columns.Add(new DataColumn("POSICION",System.Type.GetType("System.String")));//0
			dtFilas.Columns.Add(new DataColumn("ORDEN",System.Type.GetType("System.String")));//1
			dtFilas.Columns.Add(new DataColumn("ALINEACION",System.Type.GetType("System.String")));//2
			dtFilas.Columns.Add(new DataColumn("VALOR",System.Type.GetType("System.String")));//3
			dtFilas.Columns.Add(new DataColumn("OPCION",System.Type.GetType("System.String")));//4
			dtFilas.Columns.Add(new DataColumn("TABLAS",System.Type.GetType("System.String")));//5
		}
		
		protected void Mostrar_Filtros_Configurados(string filtros)
		{
			this.Preparar_dtFltrRpt();
			//Formato de las lineas
			//Control de Usuario-Campo-Tabla-Etiqueta-Tipo de Comparacion
			//0                 -   1 - 2   -   3   -  4
			string[] lines = filtros.Split('*');
			for(int i=0;i<lines.Length;i++)
			{
				string[] cols = lines[i].Split('-');
				DataRow fila = dtFltrRpt.NewRow();
				fila[0] = cols[2];
				fila[1] = cols[1];
				fila[2] = cols[3];
				fila[3] = cols[4];
				fila[4] = DBFunctions.SingleData("SELECT coltype FROM sysibm.syscolumns WHERE name='"+cols[1]+"' AND tbname='"+cols[2]+"'").Trim();
				fila[5] = cols[0];
				fila[6] = cols[5];
				dtFltrRpt.Rows.Add(fila);
			}
			this.Bind_dgFltrRpt();
		}
		
		protected void Mostrar_Filas_Configuradas()
		{
			if(Session["dtFilas"]!=null)
				dtFilas = (DataTable)Session["dtFilas"];
			else
				this.Preparar_dtFilas();
			Session.Clear();
			this.Bind_dgFilas();
		}
		
		protected void Bind_dgFilas()
		{
			Session["dtFilas"] = dtFilas;
			dgFilas.DataSource = dtFilas;
			dgFilas.DataBind();
		}
		
		protected void Bind_dgFltrRpt()
		{
			Session["dtFltrRpt"] = dtFltrRpt;
			dgFltrRpt.DataSource = dtFltrRpt;
			dgFltrRpt.DataBind();
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
