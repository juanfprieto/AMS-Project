
	using System;
	using System.Collections;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using AMS.DB;
	using AMS.Forms;
	using AMS.Utilidades;
using AMS.Tools;
namespace AMS.Inventarios
{
	/// <summary>
	///		Descripción breve de AMS_Inventarios_AutorizacionPedidoMayorVehiculo.
	/// </summary>
	public partial class AMS_Inventarios_AutorizacionPedidoMayorVehiculo : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!Page.IsPostBack)
			{
				CargarPedidos();
				if(Request.QueryString["upd"]!=null)
                    Utils.MostrarAlerta(Response, "Se han actualizado los pedidos seleccionados.");
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

		//Cargar Vehiculos
		private void CargarPedidos()
		{
			DataSet dsPedidos=new DataSet();
			DBFunctions.Request(dsPedidos,IncludeSchema.NO,
				"SELECT TB1.*, SUM(ROUND( "+
				" (DP.DPED_CANTPEDI*DP.DPED_VALOUNIT) - ((DP.DPED_CANTPEDI*DP.DPED_VALOUNIT)*TB1.MCLI_PORCDESC/100) ,0)) AS TOTAL, "+
				" SUM(ROUND( "+
				" (DP.DPED_CANTPEDI*DP.DPED_VALOUNIT) ,0)) AS TOTALO, TB1.MCLI_PORCDESC "+
				"FROM "+
				"(SELECT MP.PDOC_CODIGO, MP.MPED_NUMEPEDI, MP.MNIT_NIT, COALESCE(MC.MCLI_PORCDESC,0) MCLI_PORCDESC, "+
				"mn.mnit_nombres concat' 'concat COALESCE(mn.mnit_nombre2,'') concat' 'concat mn.mnit_apellidos concat' 'concat COALESCE(mn.mnit_apellido2,'') NOMBRE, "+
				"CAST(0 AS DECIMAL (12,2)) CUPO, CAST(0 AS DECIMAL (12,2)) CARTERA, CAST(0 AS DECIMAL (12,2)) MORA, "+
				"PV.PVEN_NOMBRE CONCAT ' (' CONCAT PV.PVEN_CODIGO CONCAT ')' VENDEDOR "+
				"FROM MPEDIDOCLIENTEAUTORIZACION MA, MPEDIDOVEHICULOCLIENTEMAYOR MP, PVENDEDOR PV, MNIT MN "+
				"LEFT OUTER JOIN MCLIENTE MC ON MC.MNIT_NIT=MN.MNIT_NIT "+
				"WHERE MA.MPED_AUTORIZA IS NULL AND MA.PDOC_CODIGO=MP.PDOC_CODIGO AND PV.PVEN_CODIGO=MP.PVEN_CODIGO AND "+
				"MA.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MN.MNIT_NIT=MP.MNIT_NIT "+
				"ORDER BY MP.PDOC_CODIGO, MP.MPED_NUMEPEDI) AS TB1, DPEDIDOVEHICULOCLIENTEMAYOR DP "+
				"WHERE DP.PDOC_CODIGO=TB1.PDOC_CODIGO and DP.MPED_NUMEPEDI = TB1.MPED_NUMEPEDI "+
				"GROUP BY TB1.PDOC_CODIGO,TB1.MPED_NUMEPEDI,TB1.MNIT_NIT,TB1.NOMBRE,TB1.CUPO,TB1.CARTERA,TB1.MORA,TB1.VENDEDOR, TB1.MCLI_PORCDESC;"
			);
			string nit;
			foreach(DataRow drP in dsPedidos.Tables[0].Rows)
			{
				nit=drP["MNIT_NIT"].ToString();
				try{drP["CUPO"]=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(MCLI_CUPOCRED,0) FROM mcliente WHERE mnit_nit='"+nit+"'"));}
				catch{drP["CUPO"]=0;}
				try{drP["CARTERA"]=Clientes.ConsultarSaldo(nit);}
				catch{drP["CARTERA"]=0;}
				try{drP["MORA"]=Clientes.ConsultarSaldoMora(nit);}
				catch{drP["MORA"]=0;}
			}
			dgPedidos.DataSource=dsPedidos.Tables[0];
			dgPedidos.DataBind();
			ViewState["PEDIDOS"]=dsPedidos.Tables[0];
		}

		//Seleccionar
		public void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
			DataTable dtPedidos=(DataTable)ViewState["PEDIDOS"];
			ArrayList sqlStrings=new ArrayList();
			string sel="";
			for(int n=0;n<dgPedidos.Items.Count;n++){
				sel=((DropDownList)dgPedidos.Items[n].FindControl("ddlAccion")).SelectedValue;
				if(sel.Length>0)
					sqlStrings.Add(
						"UPDATE MPEDIDOCLIENTEAUTORIZACION "+
						"SET MPED_AUTORIZA='"+sel+"', SUSU_CODIGO='"+HttpContext.Current.User.Identity.Name.ToLower()+"', MPED_FECHA='"+DateTime.Now.ToString("yyyy-MM-dd")+"' "+
						"WHERE PDOC_CODIGO='"+dtPedidos.Rows[n]["PDOC_CODIGO"]+"' AND MPED_NUMEPEDI="+dtPedidos.Rows[n]["MPED_NUMEPEDI"]+";");
			}
			if(sqlStrings.Count==0)
			{
                Utils.MostrarAlerta(Response, "No seleccionó pedidos.");
				return;
			}

			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Inventarios.AutorizacionPedidoMayorVehiculo&path="+Request.QueryString["path"]+"&upd=1");
			else
				lblInfo.Text += "<br>Error : Detalles <br>"+DBFunctions.exceptions;
		}
	}
}