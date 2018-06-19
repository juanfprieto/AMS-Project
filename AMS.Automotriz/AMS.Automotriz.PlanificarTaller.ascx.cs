
	using System;
	using System.Collections;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.DBManager;
namespace AMS.Automotriz
{	 

	/// <summary>
	///		Descripción breve de AMS_Automotriz_PlanificarTaller.
	/// </summary>
	public partial class AMS_Automotriz_PlanificarTaller : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Button consultaTaller;
		protected System.Web.UI.WebControls.DataGrid GridHorTec;
		protected System.Web.UI.WebControls.DropDownList DropDownList1;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		DataTable dtInserts = new DataTable();
		DataTable dtInserItem=new DataTable();
					
		protected void Page_Load(object sender, System.EventArgs e)
		{
			
			if(!IsPostBack)
			{
				DateTime diaActual = DateTime.Now;
				Label3.Text = diaActual.ToString("dd-MM-yyyy");
				Label5.Text =DateTime.Now.ToString("HH:mm:ss");
				DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(Lista, "SELECT Palm_almacen, palm_descripcion FROM DBXSCHEMA.PALMACEN pa where  (pa.pcen_centtal is not null or pcen_centcoli is not null) and pa.tvig_vigencia='V' order by pa.PALM_DESCRIPCION;");
				
			}
		}

		
		protected void GenerarTabla(object sender, System.EventArgs e)
		{	
            if(Request.QueryString["veh"]!= null)
                Response.Write("<script language:javascript>w=window.open('AMS.Automotriz.PlanificacionTallerVeh.aspx?tal=" + Lista.SelectedValue + "&pag=1&rdrt=0','','scrollbars=1,fullscreen=yes,width=1000,height=710,left=0');</script>");
		    else
                if (Request.QueryString["planning"] != null)
                    Response.Write("<script language:javascript>w=window.open('AMS.Automotriz.PlanificacionTaller.aspx?tal=" + Lista.SelectedValue + "&pag=1&rdrt=0','','scrollbars=1,fullscreen=yes,width=1000,height=710,left=0');</script>");
                else
                    Response.Write("<script language:javascript>w=window.open('AMS.Automotriz.PlanificacionTaller.aspx?tal=" + Lista.SelectedValue + "&pag=1&rdrt=0&planning=" + Request.QueryString["planning"] + "','','scrollbars=1,fullscreen=yes,width=1000,height=710,left=0');</script>");
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
	

  
 
