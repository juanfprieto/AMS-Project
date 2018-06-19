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
	public partial class Tablas : System.Web.UI.UserControl 
	{
		#region Atributos
		protected DataTable dtTablas;
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion
			
		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				Session.Clear();
				DatasToControls bind = new DatasToControls();
				bind.PutDatasIntoDropDownList(ddlTablas,"SELECT name, name CONCAT ' - ' CONCAT COALESCE(remarks,'Tabla No Comentada') FROM sysibm.systables WHERE name not like 'T%' AND name not like 'S%' AND creator = '"+ConfigurationManager.AppSettings["Schema"].ToUpper()+"' ORDER BY name");
				this.Preparar_dtTablas();
				Session["dtTablas"] = dtTablas;
			}
			if(Session["dtTablas"]==null)
				this.Preparar_dtTablas();
			else
				dtTablas = (DataTable)Session["dtTablas"];
		}
		
		protected void Agregar_Tabla(Object  Sender, EventArgs e)
		{
			//Primero debemos contar la cantidad de filas que ya tiene si es igual a 0 debemos dejarlo seguir si no enviar un error
			if(dtTablas.Rows.Count==0)
			{
				//Bueno en segundo lugar miramos si ya se ha seleccionado esta nombre de tabla dentro de esta tabla
				bool errorRep = false;
				if(dtTablas.Rows.Count>0)
				{
					DataRow[] selection = dtTablas.Select("NOMBRE = '"+ddlTablas.SelectedValue.Trim()+"'");
					if(selection.Length>0)
						errorRep = true;
				}
				if(errorRep)
                    Utils.MostrarAlerta(Response, "Esta Tabla ya fue Seleccionada");
				else
				{
					DataRow fila = dtTablas.NewRow();
					fila[0] = ddlTablas.SelectedValue;
					fila[1] = DBFunctions.SingleData("SELECT COALESCE(remarks,'Tabla No Comentada') FROM sysibm.systables WHERE name = '"+ddlTablas.SelectedValue+"'");
					fila[2] = Retornar_Referencias(ddlTablas.SelectedValue);
					dtTablas.Rows.Add(fila);
					this.Bind_dgTablas();
					if(!btnAcpt.Visible)
						btnAcpt.Visible = true;
				}
				////////////////////////////////////////////////////////////////////////////////////////////////////////
			}
			else
            Utils.MostrarAlerta(Response, "Solo se puede seleccionar una tabla");
		}
		
		protected void Aceptar_Tablas(Object  Sender, EventArgs e)
		{
			if(dtTablas.Rows.Count>0)
				Response.Redirect("" + indexPage + "?process=Reportes.CamposReporte&tablas="+Tablas_Seleccionadas()+"");
			else
            Utils.MostrarAlerta(Response, "No se ha seleccionado ninguna tabla");
		}
		
		protected void DgTable_Delete(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				dtTablas.Rows.Remove(dtTablas.Rows[e.Item.ItemIndex]);
			}
			catch
			{
				dtTablas.Rows.Clear();
                Utils.MostrarAlerta(Response, "Se ha presentado un error con la tabla");
			}
			this.Bind_dgTablas();
			if(dtTablas.Rows.Count==0)
				btnAcpt.Visible = false;
		}
			
		#endregion
			
		#region Otros	
			
		protected void Preparar_dtTablas()
		{
			dtTablas = new DataTable();
			dtTablas.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			dtTablas.Columns.Add(new DataColumn("COMENTARIO",System.Type.GetType("System.String")));
			dtTablas.Columns.Add(new DataColumn("RELACIONADAS",System.Type.GetType("System.String")));
		}
		protected void Bind_dgTablas()
		{
			Session["dtTablas"] = dtTablas;
			dgTablas.DataSource = dtTablas;
			dgTablas.DataBind();
		}
		
		protected string Retornar_Referencias(string tbname)
		{
			int i;
			DataSet dsTmp = new DataSet();
			DBFunctions.Request(dsTmp,IncludeSchema.NO,"SELECT DISTINCT reftbname FROM sysibm.sysrels WHERE tbname = '"+ddlTablas.SelectedValue+"';"+
			                   						   "SELECT DISTINCT tbname FROM sysibm.sysrels WHERE reftbname = '"+ddlTablas.SelectedValue+"';");
			string refs = "", refs2 = "";
			for(i=0;i<dsTmp.Tables[0].Rows.Count;i++)
				refs += dsTmp.Tables[0].Rows[i][0].ToString()+"-";
			for(i=0;i<dsTmp.Tables[1].Rows.Count;i++)
				refs2 += dsTmp.Tables[1].Rows[i][0].ToString()+"-";
			if(refs != "")
				refs = "HACE REFERENCIA A : "+refs.Substring(0,refs.Length-1);
			if(refs2 != "")
				refs2 = "<br>ESTA REFERENCIADA POR : "+refs2.Substring(0,refs2.Length-1);
			return refs+refs2;
		}
		
		protected string Tablas_Seleccionadas()
		{
			string tbs = "";
			for(int i=0;i<dtTablas.Rows.Count;i++)
				tbs += dtTablas.Rows[i][0].ToString() + "-";
			return tbs.Substring(0,tbs.Length-1);
		}
		
		#endregion
		
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
	}
}
