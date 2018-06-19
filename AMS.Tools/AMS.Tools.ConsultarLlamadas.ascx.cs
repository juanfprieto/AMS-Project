using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;

namespace AMS.Tools
{
	/// <summary>
	///		Descripción breve de AMS_Tools_ConsultarLlamadas.
	/// </summary>
	public partial class ConsultarLlamadas : System.Web.UI.UserControl
	{

		#region Atributos

		protected Label lbLlam;
		protected DataTable dtLlamadas;
		protected string indexPage=ConfigurationSettings.AppSettings["MainIndexPage"];

		#endregion
		
		#region Eventos

		protected void Page_Load(object Sender,EventArgs e)
		{
			if(!IsPostBack)
				Cargar_dtLlamadas();
			else
			{
				if(Session["dtLlamadas"]!=null)
					dtLlamadas=(DataTable)Session["dtLlamadas"];
			}
		}

		protected void rblLlam_SelectedIndexChanged(object Sender,EventArgs e)
		{
			if(rblLlam.SelectedValue=="C")
			{
				dtLlamadas=Cargar_Llamadas(rblLlam.SelectedValue);
				Construir_Control_Llamadas(dgCreadas,dgCerradas);
			}
			else if(rblLlam.SelectedValue=="D")
			{
				dtLlamadas=Cargar_Llamadas(rblLlam.SelectedValue);
				Construir_Control_Llamadas(dgCerradas,dgCreadas);
			}
			else if(rblLlam.SelectedValue=="R")
			{
				dtLlamadas=Cargar_Llamadas(rblLlam.SelectedValue);
				Construir_Control_Llamadas(dgCreadas,dgCerradas);
			}
		}

		protected void dgCreadas_ItemCommand(object Sender,DataGridCommandEventArgs e)
		{
			if(e.CommandName=="guardar")
			{
				//Si escogio revisada
				if(((RadioButtonList)e.Item.FindControl("rblmar")).SelectedValue=="R")
				{
					if(DBFunctions.NonQuery("UPDATE mllamada SET test_codigo='R',mlla_comentario='"+((TextBox)e.Item.FindControl("tbcom")).Text+"' WHERE mlla_id="+dtLlamadas.Rows[e.Item.DataSetIndex][0].ToString()+" AND memp_codiempl=(SELECT memp_codiempl FROM mempleadosusuario WHERE susu_codigo=(SELECT susu_codigo FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"'))")==1)
                    Utils.MostrarAlerta(Response, "La llamada con id " + dtLlamadas.Rows[e.Item.DataSetIndex][0].ToString() + " fue marcada como Revisada");
				}
					//Si escogio cerrada
				else if(((RadioButtonList)e.Item.FindControl("rblmar")).SelectedValue=="D")
				{
					if(DBFunctions.NonQuery("UPDATE mllamada SET test_codigo='D',mlla_comentario='"+((TextBox)e.Item.FindControl("tbcom")).Text+"' WHERE mlla_id="+dtLlamadas.Rows[e.Item.DataSetIndex][0].ToString()+" AND memp_codiempl=(SELECT memp_codiempl FROM mempleadosusuario WHERE susu_codigo=(SELECT susu_codigo FROM susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"'))")==1)
                    Utils.MostrarAlerta(Response, "La llamada con id " + dtLlamadas.Rows[e.Item.DataSetIndex][0].ToString() + " fue marcada como Cerrada");
				}
				dtLlamadas=Cargar_Llamadas(rblLlam.SelectedValue);
				Construir_Control_Llamadas(dgCreadas,dgCerradas);
			}
		}

		protected void btnSalir_Click(object Sender,EventArgs e)
		{
			Response.Redirect(indexPage+"?process=Tools.AdministrarLlamadasPersonales");
		}

		#endregion
		
		#region Métodos

		protected DataTable Cargar_Llamadas(string filtro)
		{
			DataSet ds=new DataSet();
			//DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MLLA.mlla_id AS \"ID\",MNIT.mnit_nombre AS \"PARA\",MNI2.mnit_nombre AS \"RECIBIO\",TACL.tac_nombre AS \"ACCION\",TEST.test_descripcion AS \"ESTADO\",MLLA.mlla_fecha AS \"FECHA\",MLLA.mlla_hora AS \"HORA\",MLLA.mlla_persona AS \"DE\",MLLA.mlla_mensaje AS \"MENSAJE\",MLLA.mlla_comentario AS \"COMENTARIO\" FROM dbxschema.mllamada MLLA,dbxschema.pasesor PASE,dbxschema.pasesor PAS2,dbxschema.taccionllamada TACL,dbxschema.testadollamada TEST,dbxschema.mnit MNIT,dbxschema.MNIT MNI2 WHERE MLLA.mnit_nit=PASE.mnit_nit AND MLLA.pase_nit=PAS2.mnit_nit AND MLLA.tac_codigo=TACL.tac_codigo AND MLLA.test_codigo=TEST.test_codigo AND PASE.mnit_nit=MNIT.mnit_nit AND PAS2.mnit_nit=MNI2.mnit_nit AND MLLA.test_codigo='"+filtro+"' AND MLLA.mnit_nit=(SELECT pase_nit FROM dbxschema.pasesorusuario WHERE susu_codigo=(SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"')) ORDER BY MLLA.mlla_id DESC");			
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MLLA.mlla_id AS \"ID\",MNIT.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNIT.mnit_apellido2,'') CONCAT ' ' CONCAT MNIT.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNIT.mnit_nombre2,'') AS \"PARA\",MNI2.mnit_apellidos CONCAT ' ' CONCAT COALESCE(MNI2.mnit_apellido2,'') CONCAT ' ' CONCAT MNI2.mnit_nombres CONCAT ' ' CONCAT COALESCE(MNI2.mnit_nombre2,'') AS \"RECIBIO\",TACL.tac_nombre AS \"ACCION\",TEST.test_descripcion AS \"ESTADO\",MLLA.mlla_fecha AS \"FECHA\",MLLA.mlla_hora AS \"HORA\",MLLA.mlla_persona AS \"DE\",MLLA.mlla_mensaje AS \"MENSAJE\",MLLA.mlla_comentario AS \"COMENTARIO\" FROM dbxschema.mllamada MLLA,dbxschema.mempleado MEMP,dbxschema.mempleado MEM2,dbxschema.taccionllamada TACL,dbxschema.testadollamada TEST,dbxschema.mnit MNIT,dbxschema.MNIT MNI2 WHERE MLLA.memp_codiempl=MEMP.memp_codiempl AND MLLA.memp_codiemp2=MEM2.memp_codiempl AND MLLA.tac_codigo=TACL.tac_codigo AND MLLA.test_codigo=TEST.test_codigo AND MEMP.mnit_nit=MNIT.mnit_nit AND MEM2.mnit_nit=MNI2.mnit_nit AND MLLA.test_codigo='"+filtro+"' AND MLLA.memp_codiempl=(SELECT memp_codiempl FROM dbxschema.mempleadosusuario WHERE susu_codigo=(SELECT susu_codigo FROM dbxschema.susuario WHERE susu_login='"+HttpContext.Current.User.Identity.Name.ToLower()+"')) ORDER BY MLLA.mlla_id DESC");
			return ds.Tables[0];
		}

		protected void Cargar_dtLlamadas()
		{
			dtLlamadas=new DataTable();
			dtLlamadas.Columns.Add("ID",typeof(string));
			dtLlamadas.Columns.Add("PARA",typeof(string));
			dtLlamadas.Columns.Add("RECIBIO",typeof(string));
			dtLlamadas.Columns.Add("ACCION",typeof(string));
			dtLlamadas.Columns.Add("ESTADO",typeof(string));
			dtLlamadas.Columns.Add("FECHA",typeof(string));
			dtLlamadas.Columns.Add("HORA",typeof(string));
			dtLlamadas.Columns.Add("DE",typeof(string));
			dtLlamadas.Columns.Add("MENSAJE",typeof(string));
			dtLlamadas.Columns.Add("COMENTARIO",typeof(string));
		}

		protected void Construir_Control_Llamadas(DataGrid dg,DataGrid vis)
		{
			dg.DataSource=dtLlamadas;
			dg.DataBind();
			Session["dtLlamadas"]=dtLlamadas;
			dg.Visible=true;
			vis.Visible=false;
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
