
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
	///		Descripción breve de AMS_Inventarios_AutiorizacionPedidoMayor.
	/// </summary>
	public partial class AMS_Inventarios_AutiorizacionPedidoMayor : System.Web.UI.UserControl
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
            DataSet dsPedidos = new DataSet();
            DBFunctions.Request(dsPedidos, IncludeSchema.NO,
                /*"SELECT MP.PPED_CODIGO, MP.MPED_NUMEPEDI, MP.MNIT_NIT, "+
				"mn.mnit_nombres concat' 'concat COALESCE(mn.mnit_nombre2,'') concat' 'concat mn.mnit_apellidos concat' 'concat COALESCE(mn.mnit_apellido2,'') NOMBRE, "+
				"CAST(0 AS DECIMAL (12,2)) CUPO, CAST(0 AS DECIMAL (12,2)) CARTERA, CAST(0 AS DECIMAL (12,2)) MORA, "+
				"PV.PVEN_NOMBRE CONCAT ' (' CONCAT PV.PVEN_CODIGO CONCAT ')' VENDEDOR "+
				"FROM MPEDIDOCLIENTEAUTORIZACION MA, MNIT MN, MPEDIDOITEM MP, PVENDEDOR PV "+
				"WHERE MA.MPED_AUTORIZA IS NULL AND MA.PDOC_CODIGO=MP.PPED_CODIGO AND PV.PVEN_CODIGO=MP.PVEN_CODIGO AND "+
				"MA.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MN.MNIT_NIT=MP.MNIT_NIT "+
				"ORDER BY MP.PPED_CODIGO, MP.MPED_NUMEPEDI;"*/
                @"SELECT TB1.PPED_CODIGO,TB1.MPED_NUMEPEDI, FECHA, TB1.MNIT_NIT, NOMBRE, CUPO, CARTERA, MORA, VENDEDOR,
                SUM(ROUND((DP.DPED_CANTPEDI*DP.DPED_VALOUNIT)-((DP.DPED_CANTPEDI*DP.DPED_VALOUNIT)*DP.DPED_PORCDESC/100)+  
                (((DP.DPED_CANTPEDI*DP.DPED_VALOUNIT)-((DP.DPED_CANTPEDI*DP.DPED_VALOUNIT)*DP.DPED_PORCDESC/100))*DP.PIVA_PORCIVA/100)  
                ,0)) AS TOTAL  
                    FROM  
	                    (SELECT MP.PPED_CODIGO, MP.MPED_NUMEPEDI, CAST(MP.MPED_PEDIDO AS CHAR(10)) AS FECHA, MP.MNIT_NIT,  
	                    trim(mn.mnit_nombres concat' 'concat COALESCE(mn.mnit_nombre2,'') concat' 'concat mn.mnit_apellidos concat' 'concat COALESCE(mn.mnit_apellido2,'')) concat ' Fecha Pedido ' concat MPED_PEDIDO NOMBRE,  
	                     CAST(coalesce(MCLI_CUPOCRED,0) AS DECIMAL (12,2)) CUPO, CAST(0 AS DECIMAL (12,2)) CARTERA, CAST(0 AS DECIMAL (12,2)) MORA,   
	                    PV.PVEN_NOMBRE CONCAT ' (' CONCAT PV.PVEN_CODIGO CONCAT ')' VENDEDOR   
	                      FROM MPEDIDOCLIENTEAUTORIZACION MA, MNIT MN, PVENDEDOR PV, MPEDIDOITEM MP
                             left join mcliente mc on mp.mnit_nit = mc.mnit_nit
	                     WHERE MA.MPED_AUTORIZA IS NULL AND MA.PDOC_CODIGO=MP.PPED_CODIGO AND PV.PVEN_CODIGO=MP.PVEN_CODIGO AND   
	                    MA.MPED_NUMEPEDI=MP.MPED_NUMEPEDI AND MN.MNIT_NIT=MP.MNIT_NIT  
	                    ORDER BY MP.PPED_CODIGO, MP.MPED_NUMEPEDI) AS TB1, DPEDIDOITEM DP  
	                    WHERE DP.PPED_CODIGO=TB1.PPED_CODIGO and DP.MPED_NUMEPEDI = TB1.MPED_NUMEPEDI  
	                    GROUP BY TB1.PPED_CODIGO,TB1.MPED_NUMEPEDI,TB1.MNIT_NIT,TB1.NOMBRE,TB1.CUPO,TB1.CARTERA,TB1.MORA,TB1.VENDEDOR, FECHA 
                        order by TB1.PPED_CODIGO,TB1.MPED_NUMEPEDI, nombre; "
            );
             
            bool nitExiste = false;
            if (dsPedidos.Tables.Count > 0)
            {
                nitExiste = false;
                for (int n = 0; n < dsPedidos.Tables[0].Rows.Count; n++)
                {
                    if (n > 0)
                    {
                        for (int x = 0; x < n; x++)
                        {
                            if (dsPedidos.Tables[0].Rows[n]["MNIT_NIT"].ToString() == dsPedidos.Tables[0].Rows[x]["MNIT_NIT"].ToString())
                            {
                                dsPedidos.Tables[0].Rows[n]["CARTERA"] = dsPedidos.Tables[0].Rows[x]["CARTERA"];
                                dsPedidos.Tables[0].Rows[n]["MORA"] = dsPedidos.Tables[0].Rows[x]["MORA"];
                                nitExiste = true;
                                break;
                            }
                        }
                    }
                    if (!nitExiste)
                    {
                        try { dsPedidos.Tables[0].Rows[n]["CARTERA"] = Clientes.ConsultarSaldo(dsPedidos.Tables[0].Rows[n]["MNIT_NIT"].ToString()); }
                        catch { dsPedidos.Tables[0].Rows[n]["CARTERA"] = 0; }
                        try { dsPedidos.Tables[0].Rows[n]["MORA"] = Clientes.ConsultarSaldoMora(dsPedidos.Tables[0].Rows[n]["MNIT_NIT"].ToString()); }
                        catch { dsPedidos.Tables[0].Rows[n]["MORA"] = 0; }
                    }
                }
            }
            dgPedidos.DataSource = dsPedidos.Tables[0];
            dgPedidos.DataBind();
            ViewState["PEDIDOS"] = dsPedidos.Tables[0];
        }

        //Seleccionar
        public void btnSeleccionar_Click(object sender, System.EventArgs e)
		{
            DataTable dtPedidos =(DataTable)ViewState["PEDIDOS"];
            ArrayList sqlStrings=new ArrayList();
			string sel="";
			string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			uint numeroLista = Convert.ToUInt32(DBFunctions.SingleData("SELECT coalesce(MAX(mlis_numero),0) FROM mlistaempaque"))+1;
			for(int n=0;n<dtPedidos.Rows.Count;n++)
			{
				sel=((DropDownList)dgPedidos.Rows[n].FindControl("ddlAccion")).SelectedValue;
				if(sel.Length>0)
					sqlStrings.Add(
						"UPDATE MPEDIDOCLIENTEAUTORIZACION "+
						"SET MPED_AUTORIZA='"+sel+"', SUSU_CODIGO='"+HttpContext.Current.User.Identity.Name.ToLower()+"', MPED_FECHA='"+DateTime.Now.ToString("yyyy-MM-dd")+"' "+
						"WHERE PDOC_CODIGO='"+dtPedidos.Rows[n]["PPED_CODIGO"]+"' AND MPED_NUMEPEDI="+dtPedidos.Rows[n]["MPED_NUMEPEDI"]+";");
				if(sel=="S")
				{
					DataSet dsPedido=new DataSet();
					DataSet dsPedidoI=new DataSet();
					DataRow drP,drI;
					DBFunctions.Request(dsPedido,IncludeSchema.NO,
						"SELECT * FROM MPEDIDOITEM "+
						"WHERE PPED_CODIGO='"+dtPedidos.Rows[n]["PPED_CODIGO"]+"' AND MPED_NUMEPEDI="+dtPedidos.Rows[n]["MPED_NUMEPEDI"]+";");
					DBFunctions.Request(dsPedidoI,IncludeSchema.NO,
						"SELECT * FROM DPEDIDOITEM "+
						"WHERE PPED_CODIGO='"+dtPedidos.Rows[n]["PPED_CODIGO"]+"' AND MPED_NUMEPEDI="+dtPedidos.Rows[n]["MPED_NUMEPEDI"]+";");
					if(dsPedido.Tables[0].Rows.Count==0 || dsPedidoI.Tables[0].Rows.Count==0){
						lblInfo.Text="Error: no se pudo consultar el pedido "+dtPedidos.Rows[n]["PPED_CODIGO"]+"-"+dtPedidos.Rows[n]["MPED_NUMEPEDI"];
						Response.Write("<script language:javascript>alert('Error: no se pudo consultar el pedido "+dtPedidos.Rows[n]["PPED_CODIGO"]+"-"+dtPedidos.Rows[n]["MPED_NUMEPEDI"]+".');</script>");
						return;
					}
					drP=dsPedido.Tables[0].Rows[0];

					ListaEmpaque listaBackOrder = new ListaEmpaque(numeroLista,drP["MNIT_NIT"].ToString(),DateTime.Now,drP["PALM_ALMACEN"].ToString(),HttpContext.Current.User.Identity.Name.ToLower(),"C",null);
					double cantidadDisponible,cantidadAsignada,valorPublico;
					for(int i=0;i<dsPedidoI.Tables[0].Rows.Count;i++){
						drI=dsPedidoI.Tables[0].Rows[i];
						//Cuadrar cantidades
						cantidadAsignada=Convert.ToDouble(drI["DPED_CANTPEDI"])-Convert.ToDouble(drI["DPED_CANTASIG"])-Convert.ToDouble(drI["DPED_CANTFACT"]);
						try{
							cantidadDisponible=Convert.ToDouble(DBFunctions.SingleData("SELECT MSAL_CANTACTUAL - abs(msal_cantasig) FROM MSALDOITEMALMACEN WHERE MITE_CODIGO='"+drI["MITE_CODIGO"].ToString()+"' AND PALM_ALMACEN='"+drP["PALM_ALMACEN"].ToString()+"' AND PANO_ANO="+ano_cinv+";"));}
						catch{
							cantidadDisponible=0;}
						valorPublico=Convert.ToDouble(drI["DPED_VALOUNIT"]);
						if(cantidadDisponible<cantidadAsignada)
							cantidadAsignada=cantidadDisponible;
				//		if(cantidadAsignada==0)
				//		{
				//			Response.Write("<script language:javascript>alert('No hay cantidad disponible para el item "+drI["MITE_CODIGO"].ToString()+" en el pedido "+dtPedidos.Rows[n]["PPED_CODIGO"].ToString()+"-"+dtPedidos.Rows[n]["MPED_NUMEPEDI"].ToString()+".');</script>");
				//			return;
				//		}
						listaBackOrder.AgregarItem(drI["MITE_CODIGO"].ToString(), drI["PPED_CODIGO"].ToString(), drI["MPED_NUMEPEDI"].ToString(), Convert.ToInt32(cantidadAsignada), valorPublico);
					}
					listaBackOrder.AlmacenarLista(false);
					for(int i=0;i<listaBackOrder.SqlStrings.Count;i++)
						sqlStrings.Add(listaBackOrder.SqlStrings[i]);
					numeroLista++;
				}
			}
			if(sqlStrings.Count==0)
			{
				Response.Write("<script language:javascript>alert('No seleccionó pedidos.');</script>");
				return;
			}

			if(DBFunctions.Transaction(sqlStrings))
				Response.Redirect(ConfigurationManager.AppSettings["MainIndexPage"]+"?process=Inventarios.AutorizacionPedidoMayor&path="+Request.QueryString["path"]+"&upd=1");
			else
				lblInfo.Text += "<br>Error : Detalles <br>"+DBFunctions.exceptions;
		}
	}
}
