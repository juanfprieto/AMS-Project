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
	public class ActivarQuincena: System.Web.UI.UserControl
	{
		protected DataTable DataTable1;
		protected DataGrid  DATAGRIDACTIVARQ;
		protected Panel PANELBORRARDATOS;
		protected Label LBPRUEBA;
		protected System.Web.UI.WebControls.Button Button1;
		int codquincena;
		
		
		protected void ActivaQuincena(object sender, DataGridCommandEventArgs e)
		{
			int ultimoregistroquincena;
			//Traer informacion del periodo escogido
			DataTable DataTable1 = new DataTable();
			DataTable1=(DataTable)Session["quinactivada"];
			//cargar los datos en variables
			int codigoquincena= int.Parse(DataTable1.Rows[e.Item.ItemIndex][0].ToString());
			int  ano=int.Parse(DataTable1.Rows[e.Item.ItemIndex][1].ToString());
			int mes=int.Parse(DataTable1.Rows[e.Item.ItemIndex][2].ToString());
			int periodonomina=int.Parse(DataTable1.Rows[e.Item.ItemIndex][3].ToString());
			int estado=int.Parse(DataTable1.Rows[e.Item.ItemIndex][4].ToString());
			Session["codquincena"]=codigoquincena;
			DataSet ultimoregistro = new DataSet();
			DBFunctions.Request(ultimoregistro,IncludeSchema.NO,"select max(mqui_codiquin) from dbxschema.mquincenas");
			ultimoregistroquincena=int.Parse(ultimoregistro.Tables[0].Rows[0][0].ToString());
			//activar la quincena actualizando datos en cnomina
			DBFunctions.NonQuery("update dbxschema.cnomina set cnom_ano="+ano+",cnom_mes="+mes+",cnom_quincena="+periodonomina+" ");
            Utils.MostrarAlerta(Response, "QUINCENA ACTIVADA");
			
			
			if (codigoquincena!=ultimoregistroquincena)
			{
				DBFunctions.NonQuery("update dbxschema.mquincenas set mqui_estado="+2+" where mqui_codiquin="+ultimoregistroquincena+" ");
				
			}
			else
			{
				
				DBFunctions.NonQuery("update dbxschema.mquincenas set mqui_estado="+1+" where mqui_codiquin="+ultimoregistroquincena+" ");
				
			}
			
			PANELBORRARDATOS.Visible=true;
		}
		
		protected void BorrarDatos(object sender, EventArgs e)
		{
			int codiquincena=0,i;
			codiquincena=(int)Session["codquincena"];
			DBFunctions.NonQuery("delete from dbxschema.dquincena where mqui_codiquin="+codiquincena+" ");
			DBFunctions.NonQuery("delete from dbxschema.dprovapropiaciones where mqui_codiquin="+codiquincena+" ");
			DBFunctions.NonQuery("delete from dbxschema.dpagaafeceps where mqui_codiquin="+codiquincena+" ");
			DBFunctions.NonQuery("delete from dbxschema.mprovisiones where mqui_codiquin="+codiquincena+" ");
			
			//borrar los dias acumulados a vacaciones en la quincena escogida
			DataSet acvacaciones = new DataSet();
			DBFunctions.Request(acvacaciones,IncludeSchema.NO,"select mvac_secuencia,dquinv_dias from dbxschema.dquinvacadias where mqui_codiquin="+codiquincena+" ");
			for (i=0;i<acvacaciones.Tables[0].Rows.Count;i++)
			{
				//Response.Write("<script language:java>alert('Se restaron los dias secuencia "+acvacaciones.Tables[0].Rows[i][0].ToString()+" tantos dias,"+acvacaciones.Tables[0].Rows[i][1].ToString()+".');</script> ");
				DBFunctions.NonQuery("update dbxschema.mvacaciones set mvac_diascaus=mvac_diascaus-"+int.Parse(acvacaciones.Tables[0].Rows[i][1].ToString())+" where mvac_secuencia="+acvacaciones.Tables[0].Rows[i][0].ToString()+" ");
			}
			//borrar los datos de acumulados en esa quincena.
			DBFunctions.NonQuery("delete from dbxschema.dquinvacadias where mqui_codiquin="+codiquincena+" ");
            Utils.MostrarAlerta(Response, "Datos Borrados Quincena " + codiquincena + "");
		}
		protected void ingresardatos_activarquincena(int codigoquincena,int ano,int mes,int periodonomina,int estado)
		{
			DataRow fila=DataTable1.NewRow();
			fila["CODIGO QUINCENA"]=codigoquincena;
			fila["AÑO"]=ano;
			fila["MES"]=mes;
			fila["PERIODO NOMINA"]=periodonomina;
			fila["ESTADO"]=estado;
			DataTable1.Rows.Add(fila);
			DATAGRIDACTIVARQ.DataSource=DataTable1;
			DATAGRIDACTIVARQ.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DATAGRIDACTIVARQ);
			DatasToControls.JustificacionGrilla(DATAGRIDACTIVARQ,DataTable1);
			for(int i=0;i<DataTable1.Rows.Count;i++)
			{
				if(Convert.ToDouble(DataTable1.Rows[i][4]) ==2)
					((Button)DATAGRIDACTIVARQ.Items[i].Cells[5].Controls[1]).Enabled = true;
			}
		}
		
				
		
		protected void preparargrilla_activarquincena()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("CODIGO QUINCENA",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("AÑO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("MES",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("PERIODO NOMINA",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("ESTADO",System.Type.GetType("System.String")));
					
			
		}
		
		
		
		
		
		protected void Page_Load(object sender , EventArgs e)
		{
			if (!IsPostBack)
			{
				int i;
				this.preparargrilla_activarquincena();
				DataSet activarquincena= new DataSet();
				//DBFunctions.Request(activarquincena,IncludeSchema.NO,"select * from dbxschema.mquincenas where mqui_estado=2");	
				DBFunctions.Request(activarquincena,IncludeSchema.NO,"select * from dbxschema.mquincenas");	
				for (i=0;i<activarquincena.Tables[0].Rows.Count;i++)
				{
					this.ingresardatos_activarquincena(int.Parse(activarquincena.Tables[0].Rows[i][0].ToString()),int.Parse(activarquincena.Tables[0].Rows[i][1].ToString()),int.Parse(activarquincena.Tables[0].Rows[i][2].ToString()),int.Parse(activarquincena.Tables[0].Rows[i][3].ToString()),int.Parse(activarquincena.Tables[0].Rows[i][4].ToString()));
				}
				Session["quinactivada"]=DataTable1;
								
			}
			else
			{
				if (Session["codquincena"]!=null)
					codquincena=(int)Session["codquincena"];
			}
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
