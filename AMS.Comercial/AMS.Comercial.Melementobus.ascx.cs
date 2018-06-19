// created on 22/02/2005 at 12:27
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Comercial
{
	public class Melementobus : System.Web.UI.UserControl
	{
		protected DropDownList catalogoBus;
		protected DataGrid grillaConfiguracion;
		protected DataTable tablaConfiguracion;
		protected Button botonGuardar;
		protected Label lb, lbInfo;
		protected System.Web.UI.WebControls.TextBox puesto;
		protected System.Web.UI.WebControls.PlaceHolder literalsControls;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(catalogoBus,"SELECT pcat_codigo FROM pcatalogovehiculo");
				Session.Clear();
			}
			this.Crear_Grilla_Configuracion(1);
		}
		
		protected void Cargar_Grilla(Object  Sender, EventArgs e)
		{
			this.Crear_Grilla_Configuracion(0);
		}
		
		protected void Guardar_Configuracion(Object  Sender, EventArgs e)
		{
			int i,j;
			string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
			ArrayList sqlStrings = new ArrayList();
			for(i=0;i<grillaConfiguracion.Items.Count;i++)
			{
				for(j=1;j<grillaConfiguracion.Items[i].Cells.Count;j++)
				{
					//Ahora aqui debemos realizar la insercion dentro de la tabla melementobus
					sqlStrings.Add("INSERT INTO melementobus VALUES('"+Session["catalogo"].ToString()+"',"+(i+1).ToString()+","+j.ToString()+",'"+(((DropDownList)grillaConfiguracion.Items[i].Cells[j].Controls[0]).SelectedValue)+"','"+(i+1).ToString()+"-"+j.ToString()+"')");
				}
			}
			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect("" + indexPage + "?process=Comercial.Melementobus");
			else
				lb.Text += "Error: " + DBFunctions.exceptions + "<br><br>";
		}
		
		protected void Preparar_Tabla_Configuracion(int numFilas, int numColumnas)
		{
			int i;
			tablaConfiguracion = new DataTable();
			//Primero Creamos la columna  que nos indica las filas
			tablaConfiguracion.Columns.Add(new DataColumn("FILA-COLUMNA",System.Type.GetType("System.String")));
			//Ahora Creamos las columnas de acuerdo a la cantidad de columnas que tiene este vehiculo especifico
			for(i=0;i<numColumnas;i++)
				tablaConfiguracion.Columns.Add(new DataColumn("COLUMNA "+(i+1).ToString()+"",System.Type.GetType("System.String")));
			//Ahora creamos las filas
			for(i=0;i<numFilas;i++)
			{
				DataRow fila = tablaConfiguracion.NewRow();
				fila["FILA-COLUMNA"] = "FILA "+(i+1).ToString()+"";
				tablaConfiguracion.Rows.Add(fila);
			}
		}
		
		protected void Llenar_Grilla_DropDownLists(int numFilas, int numColumnas)
		{
			int i,j;
			for(i=0;i<numFilas;i++)
			{
				for(j=1;j<=numColumnas;j++)
				{
					DropDownList ddlTmp = new DropDownList();
					DatasToControls bind = new DatasToControls();
					bind.PutDatasIntoDropDownList(ddlTmp,"SELECT tele_codigo, tele_descripcion FROM telementobus");
					grillaConfiguracion.Items[i].Cells[j].Controls.Add(ddlTmp);
				}
			}
		}
		
		protected void Crear_Grilla_Configuracion(int razon)
		{
			if(razon==0)
			{
				this.Preparar_Tabla_Configuracion(System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_filaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+catalogoBus.SelectedItem.ToString()+"'")), System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_columnaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+catalogoBus.SelectedItem.ToString()+"'")));
				grillaConfiguracion.DataSource = tablaConfiguracion;
				grillaConfiguracion.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(grillaConfiguracion);
				this.Llenar_Grilla_DropDownLists(System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_filaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+catalogoBus.SelectedItem.ToString()+"'")), System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_columnaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+catalogoBus.SelectedItem.ToString()+"'")));
				Session["catalogo"] = catalogoBus.SelectedItem.ToString();
				botonGuardar.Visible = true;
			}
			else if(razon==1)
			{
				if(Session["catalogo"]!=null)
				{
					this.Preparar_Tabla_Configuracion(System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_filaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+Session["catalogo"].ToString()+"'")), System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_columnaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+Session["catalogo"].ToString()+"'")));
					grillaConfiguracion.DataSource = tablaConfiguracion;
					grillaConfiguracion.DataBind();
					DatasToControls.Aplicar_Formato_Grilla(grillaConfiguracion);
					this.Llenar_Grilla_DropDownLists(System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_filaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+Session["catalogo"].ToString()+"'")), System.Convert.ToInt32(DBFunctions.SingleData("SELECT pcat_columnaspuestos FROM pcatalogovehiculo WHERE pcat_codigo='"+Session["catalogo"].ToString()+"'")));
					botonGuardar.Visible = true;
				}
			}
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
