using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.Tools;


namespace AMS.Inventarios
{
	public partial class AMS_Inventarios_CierreInventarioFisico : System.Web.UI.UserControl
	{
		#region Atributos
		private DatasToControls bind = new DatasToControls();
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlInventarios,"SELECT INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),COUNT(DINV.dinv_mite_codigo) "+
															"FROM minventariofisico INF INNER JOIN dinventariofisico DINV ON INF.pdoc_codigo = DINV.pdoc_codigo AND INF.minf_numeroinv = DINV.minf_numeroinv "+
															"WHERE  INF.minf_fechacierre IS NULL AND INF.minf_fechainicio <= '"+DateTime.Now.ToString("yyyy-MM-dd")+"' AND minf_fechacierre is null "+
															"GROUP BY INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)), CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)) "+
															"HAVING COUNT(DINV.dinv_mite_codigo) > 0 "+
															"ORDER BY CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10))");
				ddlInventarios.Items.Insert(0,new ListItem("Seleccione...",String.Empty));
				btnCerrar.Attributes.Add("onclick","return ValSelInvFis();");
			}
		}

		protected void btnCerrar_Click(object sender, System.EventArgs e)
		{
			string prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
			int numeroInventario    = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
            string tarjetaSinContar = "";
            DataSet dsTSinContar    = new DataSet();
            string sqlTsinContar    = "SELECT dinv_tarjeta FROM dinventariofisico WHERE pdoc_codigo = '" + prefijoInventario + "' AND MINF_NUMEROINV = " + numeroInventario + " AND " +
                                      "(DINV_contdefinitivo is null or dinv_diferencia is null or dinv_costdiferencia is null) ORDER BY dinv_tarjeta;";
            string tarjetaAjuErrado = "";
            DataSet dsAjusteErrado  = new DataSet();
            string sqlAjusteErrado  = "SELECT dinv_tarjeta FROM dinventariofisico, msaldoitemalmacen msa, cinventario ci " +
                                      "WHERE pdoc_codigo = '" + prefijoInventario + "' AND MINF_NUMEROINV = " + numeroInventario + " AND DINV_palm_almacen = msa.palm_almacen and msa.pano_ano = ci.pano_ano " +
                                      "and dinv_mite_codigo = msa.mite_codigo and abs(dinv_diferencia) > msa.msal_cantactual and dinv_diferencia < 0; ";
            string resultado        = "";
            DBFunctions.Request(dsTSinContar, IncludeSchema.NO, sqlTsinContar);
            DBFunctions.Request(dsAjusteErrado, IncludeSchema.NO, sqlAjusteErrado);
            if (dsTSinContar.Tables.Count > 0 && dsTSinContar.Tables[0].Rows.Count > 0)
            {
                for (int it = 0; it < dsTSinContar.Tables[0].Rows.Count; it++)
                    tarjetaSinContar += dsTSinContar.Tables[0].Rows[it][0].ToString() + ", ";
                Utils.MostrarAlerta(Response, "Falta tarjetas por completar proceso " + tarjetaSinContar + " ");
            }
            else
            {
                if (dsAjusteErrado.Tables.Count > 0 && dsAjusteErrado.Tables[0].Rows.Count > 0)
                {
                    for (int it = 0; it < dsAjusteErrado.Tables[0].Rows.Count; it++)
                        tarjetaAjuErrado += dsAjusteErrado.Tables[0].Rows[it][0].ToString() + ", ";
                    Utils.MostrarAlerta(Response, "Tarjeta " + tarjetaAjuErrado + " con Ajuste Errado, ajuste mayor a la Existencia actual, CORRIJA EL CONTEO y repite el proceso de Cierre ");
                }
                else
                {
                    resultado = InventarioFisico.CerrarInventarioFisico(prefijoInventario, numeroInventario);
                    if(resultado.Length==0)
                        Utils.MostrarAlerta(Response, "Ajustes inventario Fisico realizado Correctamente, Cierre SI ejecutado !!!");
                    else
                        lbInfo.Text = resultado;
                }
            }
            //Response.Redirect("" + indexPage + "?process=Inventarios.CierreInventarioFisico");
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.CierreInventarioFisico");
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
