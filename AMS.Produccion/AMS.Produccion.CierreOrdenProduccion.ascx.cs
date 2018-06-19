namespace AMS.Produccion
{
	using System;
	using System.IO;
	using System.Collections;
	using System.ComponentModel;
	using System.Configuration;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.Services;
	using System.Web.SessionState;
	using System.Web.Services.Protocols;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Ajax;
	using AMS.Forms;
	using AMS.DB;
	using AMS.DBManager;
	using AMS.Documentos;
	using AMS.Inventarios;
    using AMS.Tools;

	/// <summary>
	///		Descripción breve de AMS_Produccion_CierreOrdenProduccion.
	/// </summary>
	public partial class AMS_Produccion_CierreOrdenProduccion : System.Web.UI.UserControl
	{
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];

        private DataTable dtItems;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request.QueryString["ens"]==null)
					Utils.FillDll(ddlPrefOrden,
						"SELECT distinct(pd.pdoc_codigo) "+
						"from pdocumento pd, mordenproduccion mp, dordenproduccion dp "+
						"where pd.tdoc_tipodocu='OP' and mp.pdoc_codigo=pd.pdoc_codigo and "+
						"mp.pdoc_codigo=dp.pdoc_codigo and mp.mord_numeorde=dp.mord_numeorde and "+
						"dp.pcat_codigo is null and "+
						"mp.test_estado='A' and mp.mord_tipo='P';"
					, false);
				else
                    Utils.FillDll(ddlPrefOrden,
						"SELECT distinct(pd.pdoc_codigo) "+
						"from pdocumento pd, mordenproduccion mp, dordenproduccion dp "+
						"where pd.tdoc_tipodocu='OP' and mp.pdoc_codigo=pd.pdoc_codigo and "+
						"mp.pdoc_codigo=dp.pdoc_codigo and mp.mord_numeorde=dp.mord_numeorde and "+
						"dp.mite_codigo is null and  "+
						"mp.test_estado='A' and mp.mord_tipo='P';"
                    , false);
                
                Utils.llenarPrefijos(Response, ref ddlPrefijoAjuste, "%", "%", "AJ");
                ddlPrefijoAjuste_OnSelectedIndexChanged(null, null);

				CambiaOrden();
				btnCerrar.Attributes.Add("onclick","return confirm('Esta seguro de cerrar la orden seleccionada?');");
				
                if(Request.QueryString["act"]!=null)
				{
                    Utils.MostrarAlerta(Response, "La orden ha sido cerrada");
                    Imprimir.ImprimirRPT(Response, Request.QueryString["prefA"], Convert.ToInt32(Request.QueryString["numA"]), true);
                }
			}

            dtItems = (DataTable)ViewState["dtItems"];
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

		protected void ddlPrefOrden_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			CambiaOrden();
		}

        protected void ddlPrefijoAjuste_OnSelectedIndexChanged(Object Sender, EventArgs E)
        {
            lblNumeroAjuste.Text = DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo = '" + ddlPrefijoAjuste.SelectedValue + "'");
        }

        protected void btnCerrar_Click(object sender, System.EventArgs e)
        {
            ArrayList arlSql = new ArrayList();
            bool datosValidos = actualizarCantidadesItems();
            if (!datosValidos) return;

            if (txtObservacion.Text.Trim().Length == 0)
            {
                Response.Write("<script>alert('Debe dar la observación.');</script>");
                return;
            }

            arlSql.Add(
                "update MORDENPRODUCCION set test_estado='F', MORD_OBSERVACION='" + txtObservacion.Text + "' " +
                "WHERE PDOC_CODIGO='" + ddlPrefOrden.SelectedValue + "' AND MORD_NUMEORDE=" + ddlNumOrden.SelectedValue + ";");

            if (DBFunctions.Transaction(arlSql) && RealizarAjusteInventario())
            {
                if (Request.QueryString["ens"] == null)
                    Response.Redirect(indexPage + "?process=Produccion.CierreOrdenProduccion&act=1&prefA="+ddlPrefijoAjuste.SelectedValue+"&numA="+lblNumeroAjuste.Text);
                else
                    Response.Redirect(indexPage + "?process=Produccion.CierreOrdenProduccion&act=1&ens=1&prefA="+ddlPrefijoAjuste.SelectedValue+"&numA="+lblNumeroAjuste.Text);
            }
            else
                lbInfo.Text += DBFunctions.exceptions;
        }

        protected void ddlNumOrden_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            pnlDetalle.Visible = false;
            CargarDetalle();
            LlenarItems();
        }

		private void CargarDetalle()
		{
			DataSet dsDetalle=new DataSet();
			if(ddlNumOrden.Items.Count==0)
				return;
			if(Request.QueryString["ens"]==null)
				DBFunctions.Request(dsDetalle,IncludeSchema.NO,
					"SELECT DORDENPRODUCCION.*, mite_codigo as DORD_ELEM FROM DORDENPRODUCCION "+
					"WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
			else
				DBFunctions.Request(dsDetalle,IncludeSchema.NO,
					"SELECT DORDENPRODUCCION.*, pcat_codigo as DORD_ELEM FROM DORDENPRODUCCION "+
					"WHERE PDOC_CODIGO='"+ddlPrefOrden.SelectedValue+"' AND MORD_NUMEORDE="+ddlNumOrden.SelectedValue+";");
			dgrDetalle.DataSource=dsDetalle.Tables[0];
			dgrDetalle.DataBind();
			pnlDetalle.Visible=true;

			pnlObservacion.Visible=true;
			//Traer observacion de pedido si existe
			if(DBFunctions.RecordExist(
				"Select mp.mped_observacion from MPEDIDOITEM mp, MORDENPRODUCCION mo "+
				"where mo.pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' and mo.mord_numeorde="+ddlNumOrden.SelectedValue+" and "+
				"mo.pped_codipedi=mp.pped_codigo and mo.mped_numepedi=mp.mped_numepedi;")){
				txtObservacion.Text=DBFunctions.SingleData(
					"Select mp.mped_observacion from MPEDIDOITEM mp, MORDENPRODUCCION mo "+
					"where mo.pdoc_codigo='"+ddlPrefOrden.SelectedValue+"' and mo.mord_numeorde="+ddlNumOrden.SelectedValue+" and "+
					"mo.pped_codipedi=mp.pped_codigo and mo.mped_numepedi=mp.mped_numepedi;");
			}
		}

        private void CambiaOrden()
        {
            if (Request.QueryString["ens"] == null)
                Utils.FillDll(ddlNumOrden,
                    "SELECT distinct(mp.mord_numeorde) " +
                    "from mordenproduccion mp, dordenproduccion dp " +
                    "where " +
                    "mp.pdoc_codigo=dp.pdoc_codigo and mp.mord_numeorde=dp.mord_numeorde and " +
                    "dp.pcat_codigo is null and " +
                    "mp.pdoc_codigo='" + ddlPrefOrden.SelectedValue + "' and " +
                    "mp.test_estado='A' and mp.mord_tipo='P' " +
                    "order by mord_numeorde;", false);
            else
                Utils.FillDll(ddlNumOrden,
                    "SELECT distinct(mp.mord_numeorde) " +
                    "from mordenproduccion mp, dordenproduccion dp " +
                    "where " +
                    "mp.pdoc_codigo=dp.pdoc_codigo and mp.mord_numeorde=dp.mord_numeorde and " +
                    "dp.mite_codigo is null and " +
                    "mp.pdoc_codigo='" + ddlPrefOrden.SelectedValue + "' and " +
                    "mp.test_estado='A' and mp.mord_tipo='P' " +
                    "order by mord_numeorde;", false);
            pnlDetalle.Visible = false;
            CargarDetalle();
            LlenarItems();
        }

        private void LlenarItems()
        {
            string sql = String.Format(
                "SELECT mi.mite_codigo as \"mite_codigo\", \n" +
                "       mi.mite_nombre as \"mite_nombre\", \n" +
                "       mi.plin_codigo as \"mite_linea\", \n" +
                "       pu.puni_nombre as \"puni_nombre\", \n" +
                "       di.dite_cantidad as \"dite_cantidad\", \n" +
                "       di.dite_valounit as \"msal_costprom\" \n" + 
                "FROM dordenproduccion do  \n" +
                "  LEFT JOIN mordenproducciontransferencia mot  \n" +
                "		  LEFT JOIN ditems di  \n" +
                "				  LEFT JOIN mitems mi  \n" +
                "				  		LEFT JOIN punidad pu ON mi.puni_codigo = pu.puni_codigo  \n" +
                "				  ON di.mite_codigo = mi.mite_codigo  \n" +
                "		  ON di.pdoc_codigo = mot.pdoc_factura AND dite_numedocu = mot.mfac_numero  \n" +
                "  ON do.pdoc_codigo = mot.pdoc_codigo AND do.mord_numeorde = mot.mord_numeorde \n" +
                "WHERE do.pdoc_codigo = '{0}' \n" +
                "AND   do.mord_numeorde = {1}" 
                , ddlPrefOrden.SelectedValue
                , ddlNumOrden.SelectedValue);


            try
            {
                dtItems = DBFunctions.Request(new DataSet(), IncludeSchema.NO, sql).Tables[0];
                dtItems.Columns.Add("msal_cantasig");

                dgItems.DataSource = dtItems;
                dgItems.DataBind();
                ViewState["dtItems"] = dtItems;
            }
            catch { }
        }

        private bool actualizarCantidadesItems()
        {
            for (int i = 0; i < dtItems.Rows.Count; i++)
            {
                string cantidadDevolucion = ((TextBox)dgItems.Items[i].FindControl("txtCantidad")).Text;
                double cantidadPedida = Convert.ToDouble(dtItems.Rows[i]["dite_cantidad"]);

                if (!Utils.EsNumero(cantidadDevolucion))
                {
                    Utils.MostrarAlerta(Response, "Por favor ingrese datos numéricos en las catidades a devolver");
                    return false;
                }
                if (cantidadPedida < Convert.ToDouble(cantidadDevolucion))
                {
                    Utils.MostrarAlerta(Response, "No se puede devolver una mayor cantidad a la transferida a planta");
                    return false;
                }

                dtItems.Rows[i]["msal_cantasig"] = cantidadDevolucion;
            }

            return true;
        }

        private bool RealizarAjusteInventario()
        {
            string centroCosto = "";
            string msjError = "";

            if (Request.QueryString["ens"] == null)
                centroCosto = DBFunctions.SingleData("select pcen_centplan from cproduccion");
            else
                centroCosto = DBFunctions.SingleData("select pcen_centensa from cproduccion");

            string sql = String.Format(
                "SELECT mpi.palm_almacen, \n" +
             "       mop.pven_codigo \n" +
             "FROM mordenproduccion mop  \n" +
             "  LEFT JOIN MPEDIDOPRODUCCIONTRANSFERENCIA mpp  \n" +
             "  		LEFT JOIN mpedidoitem mpi ON mpi.pped_codigo = mpp.pped_codigo AND mpi.mped_numepedi = mpp.mped_numero  \n" +
             "  ON mop.pdoc_codigo = mpp.pdoc_codigo AND mop.mord_numeorde = mpp.mord_numeorde \n" +
             "WHERE mop.pdoc_codigo = '{0}' \n" +
             "AND   mop.mord_numeorde = {1}"
                , ddlPrefOrden.SelectedValue
                , ddlNumOrden.SelectedValue);

            Hashtable hashOrdenProd = (Hashtable)DBFunctions.RequestAsCollection(sql)[0]; // debe traer siempre una orden

            bool resultado = AjustesInv.RealizarAjusteInventario(
                ddlPrefijoAjuste.SelectedValue,
                Convert.ToInt32(lblNumeroAjuste.Text),
                ddlPrefOrden.SelectedValue,
                Convert.ToInt32(ddlNumOrden.SelectedValue),
                hashOrdenProd["PALM_ALMACEN"].ToString(),
                DateTime.Now,
                hashOrdenProd["PVEN_CODIGO"].ToString(),
                centroCosto,
                txtObservacion.Text,
                dtItems,
                ref msjError);

            if (!resultado)
                lbInfo.Text += msjError;

            return resultado;
        }

	}
}
