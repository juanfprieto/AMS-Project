using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Nomina
{
	public class EliminarAcumulados : System.Web.UI.UserControl
	{
	
		protected DropDownList DDLPROCESO;
		protected DataTable DataTable1;
		protected DataGrid DataGrid1;
		
		string mainpage=ConfigurationManager.AppSettings["MainIndexPage"];
		
		
		protected void BorrarDatos(object sender, DataGridCommandEventArgs e)
		{
			string secuencia;
			int procesop;
			procesop=(int)Session["proceso"];
            Utils.MostrarAlerta(Response, "Borrando datos.. ");
			DataTable DataTable1= new DataTable();
			DataTable1=(DataTable)Session["DataTable1"];
			secuencia=DataTable1.Rows[e.Item.ItemIndex][2].ToString();
            Utils.MostrarAlerta(Response, "SECUENCIA " + secuencia + ".. ");
			if (procesop==1)
			{
            Utils.MostrarAlerta(Response, "procesop cesantias .. ");
			DBFunctions.NonQuery("delete from dbxschema.dcesantias where mces_secuencia="+secuencia+"");
			DBFunctions.NonQuery("delete from dbxschema.mcesantias where mces_secuencia="+secuencia+"");
			
			}
			
			if (procesop==2)
			{
            Utils.MostrarAlerta(Response, "procesop primas .. ");
			DBFunctions.NonQuery("delete from dbxschema.dprimas where mpri_secuencia="+secuencia+"");
			DBFunctions.NonQuery("delete from dbxschema.mprimas where dpri_secuencia="+secuencia+"");
			}
			Response.Redirect(mainpage+"?process=Nomina.EliminarAcumulados");
			
		}
		
		protected void CargarGrilla(object sender,System.EventArgs e )
		{
			int i;
			int proceso=0;
			this.preparargrilla_proceso();
			DataSet procesocesantias = new DataSet();
			DataSet procesoprimas = new DataSet();
			DBFunctions.Request(procesocesantias,IncludeSchema.NO,"select mces_fechinic,mces_fechfina,mces_secuencia from dbxschema.mcesantias ");
			DBFunctions.Request(procesoprimas,IncludeSchema.NO,"select mpri_fechinic,mpri_fechfina,mpri_secuencia from dbxschema.mprimas ");
			//si escogio cesantias
			
			if (DDLPROCESO.SelectedValue=="Cesantias")
			{
                Utils.MostrarAlerta(Response, "Selecciono la opción Cesantias .. ");
				for (i=0;i<procesocesantias.Tables[0].Rows.Count;i++)
				{
				this.ingresar_datos_proceso(procesocesantias.Tables[0].Rows[i][0].ToString(),procesocesantias.Tables[0].Rows[i][1].ToString(),procesocesantias.Tables[0].Rows[i][2].ToString());
				}
				proceso=1;	
			}
			//si escogio primas
			if (DDLPROCESO.SelectedValue=="Primas")
			{
                Utils.MostrarAlerta(Response, "Seleciono la opción Primas .. ");
				for (i=0;i<procesoprimas.Tables[0].Rows.Count;i++)
				{
				this.ingresar_datos_proceso(procesoprimas.Tables[0].Rows[i][0].ToString(),procesoprimas.Tables[0].Rows[i][1].ToString(),procesoprimas.Tables[0].Rows[i][2].ToString());
				}
				proceso=2;	
			}
			
			Session["DataTable1"]=DataTable1;
			Session["proceso"]=proceso;
		}
		
		
		protected void ingresar_datos_proceso(string fechainicio,string fechafinal,string secuencia)
		{
			    DataRow fila = DataTable1.NewRow();
			    DateTime fechainic = new DateTime();
			    DateTime fechafin= new DateTime();
			    fechainic=Convert.ToDateTime(fechainicio);
			    fechafin=Convert.ToDateTime(fechafinal);
			    fila["FECHA INICIO"]=fechainic.ToString("yyyy-MM-dd");
			    fila["FECHA FINAL"]=fechafin.ToString("yyyy-MM-dd");
				fila["SECUENCIA"]=secuencia;
			    DataTable1.Rows.Add(fila);
			    DataGrid1.DataSource = DataTable1;
				DataGrid1.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(DataGrid1);
				DatasToControls.JustificacionGrilla(DataGrid1,DataTable1);
				for(int i=0;i<DataTable1.Rows.Count;i++)
				{
				((Button)DataGrid1.Items[i].Cells[3].Controls[1]).Enabled = true;
				}
		}

		
		
		protected void preparargrilla_proceso()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("FECHA INICIO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("FECHA FINAL",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("SECUENCIA",System.Type.GetType("System.String")));
			
		}
		
		protected void CargarGrillaprimeravez()
		{
			this.preparargrilla_proceso();
			int i,proceso=0;
			DataSet procesocesantias = new DataSet();
			DBFunctions.Request(procesocesantias,IncludeSchema.NO,"select mces_fechinic,mces_fechfina,mces_secuencia from dbxschema.mcesantias ");
			if (DDLPROCESO.SelectedValue=="Cesantias")
			{
				
				for (i=0;i<procesocesantias.Tables[0].Rows.Count;i++)
				{
				this.ingresar_datos_proceso(procesocesantias.Tables[0].Rows[i][0].ToString(),procesocesantias.Tables[0].Rows[i][1].ToString(),procesocesantias.Tables[0].Rows[i][2].ToString());
				}
				proceso=1;	
			}
			Session["proceso"]=proceso;
			
		}

		
		protected void Page_Load(object sender , EventArgs e)
		{
			this.CargarGrillaprimeravez();
		}
		
		
		
		//protected HtmlInputFile fDocument;
		
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
