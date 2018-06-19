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
using Ajax;
using WebChart;

namespace AMS.Inventarios
{
	public partial class AMS_Inventarios_ConsultaInventarioFisico : System.Web.UI.UserControl
	{
		#region Atributos
		protected string indexPage = ConfigurationManager.AppSettings["MainIndexPage"];
		private DatasToControls bind = new DatasToControls();
		#endregion

		#region Eventos
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Ajax.Utility.RegisterTypeForAjax(typeof(AMS_Inventarios_ConsultaInventarioFisico));	
			if(!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(ddlInventarios,"SELECT INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10)),CAST(INF.minf_fechainicio AS char(10)) || ' Inventario Numero : ' || INF.pdoc_codigo || '-' || CAST(INF.minf_numeroinv AS char(10))"+
					"FROM minventariofisico INF");
				ddlInventarios.Items.Insert(0,new ListItem("Seleccione...",String.Empty));
				btnAceptar.Attributes.Add("onclick","return ValSelInvFis();");
			}
			else
			{
				if(ddlInventarios.SelectedIndex > 0)
				{
					string prefijoInventario;
                    int numeroInventario;


                    if (ddlInventarios.SelectedValue.Split('-').Length == 3)
                    {
                        prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                        numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

                    }
                    else
                    {
                        prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                        numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
                    }
					InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
					CargarColumnasConteo(inst);
					CargarInformacionTarjeta();
				}
			}
		}

		protected void btnCancelar_Click(object sender, System.EventArgs e)
		{
			Response.Redirect("" + indexPage + "?process=Inventarios.ConsultaInventarioFisico");
		}

		protected void btnAceptar_Click(object sender, System.EventArgs e)
		{
			ddlInventarios.Enabled = false;

			string prefijoInventario; 
			int numeroInventario; 


            if (ddlInventarios.SelectedValue.Split('-').Length == 3)
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
            }
            		
			InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);
			
			CargarInformacionConteos(inst);
			CargarInformacionGeneral(inst);

			pnlInfoProceso.Visible = true;

            CargarColumnasConteo(inst);
		}

		protected void btnCargar_Click(object sender, System.EventArgs e)
		{
			string prefijoInventario;
            int numeroInventario;


            if (ddlInventarios.SelectedValue.Split('-').Length == 3)
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0] + "-" + (ddlInventarios.SelectedValue.Split('-'))[1];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[2]);

            }
            else
            {
                prefijoInventario = (ddlInventarios.SelectedValue.Split('-'))[0];
                numeroInventario = Convert.ToInt32((ddlInventarios.SelectedValue.Split('-'))[1].Trim());
            }


			InventarioFisico inst = new InventarioFisico(prefijoInventario,numeroInventario);

			CargarInformacionConteos(inst);
		}
		#endregion

		#region Metodos
		
		private string ConvertirCantidad(string cantidad)
		{
			string cantidadCadena = "";

			if (cantidad != "-1")
				cantidadCadena = cantidad.ToString();

			return cantidadCadena;
		}

		private void CargarInformacionGeneral(InventarioFisico inst)
		{
			lbPrefijoInventario.Text = inst.PrefijoInventario;
			lbNumeroInventario.Text = inst.NumeroInventario.ToString();

			lbFechaInicioInventario.Text = inst.FechaInicio.ToString("yyyy-MM-dd");
			lbFechaFinInventario.Text = inst.FechaTerminacion.ToString("yyyy-MM-dd");

			lbTipoInventario.Text = DBFunctions.SingleData("SELECT tift_descripcion FROM tinventariofisicotipo WHERE tift_codigo='"+inst.CodigoTipoInventarioTipo+"'");
            lbAlmacenRelacionado.Text = DBFunctions.SingleData("SELECT DISTINCT DINV_PALM_ALMACEN CONCAT ' ' CONCAT PA.PALM_DESCRIPCION FROM DINVENTARIOFISICO DIV INNER JOIN PALMACEN PA ON DIV.DINV_PALM_ALMACEN = PA.PALM_ALMACEN WHERE PDOC_CODIGO = '" + inst.PrefijoInventario + "' AND MINF_NUMEROINV = '"+ inst.NumeroInventario +"'");
			//lbAlmacenRelacionado.Text = DBFunctions.SingleData("SELECT ttifu_descripcion FROM tinventariofisicoubicacion WHERE ttifu_codigo='"+inst.CodigoTipoInventarioUbicacion+"'");
		}
		
		private void CargarColumnasConteo(InventarioFisico inst)
		{
			chtEstadisticasInv.YCustomEnd = 100;
			chtEstadisticasInv.Charts.Add(CreateColumnChart(ObtenerGraficoTarjetasAConteo(inst),Color.IndianRed,"Número de Tarjetas por Contar."));
			chtEstadisticasInv.Charts.Add(CreateColumnChart(ObtenerGraficoTarjetasEnConteo(inst),Color.SeaGreen,"Número de Tarjetas Contadas."));
			chtEstadisticasInv.RedrawChart();
		}

		private ChartPointCollection ObtenerGraficoTarjetasEnConteo(InventarioFisico inst)
		{
			ChartPointCollection data = new ChartPointCollection();

			data.Add(new ChartPoint("Conteo 1",float.Parse(inst.NumeroTarjetasEnConteo1.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));
			data.Add(new ChartPoint("Conteo 2",float.Parse(inst.NumeroTarjetasEnConteo2.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));
			data.Add(new ChartPoint("Conteo 3",float.Parse(inst.NumeroTarjetasEnConteo3.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));
			data.Add(new ChartPoint("Finalizadas",float.Parse(inst.NumeroTotalTarjetasEnConteoDefinitivoInstancia.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));

			return data;
		}

		private ChartPointCollection ObtenerGraficoTarjetasAConteo(InventarioFisico inst)
		{
			ChartPointCollection data = new ChartPointCollection();

			data.Add(new ChartPoint("Conteo 1",float.Parse(inst.NumeroTotalTarjetasAConteo1Instancia.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));
			data.Add(new ChartPoint("Conteo 2",float.Parse(inst.NumeroTotalTarjetasAConteo2Instancia.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));
			data.Add(new ChartPoint("Conteo 3",float.Parse(inst.NumeroTotalTarjetasAConteo3Instancia.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));
			data.Add(new ChartPoint("Finalizadas",float.Parse(inst.NumeroTotalTarjetasAConteoInstancia.ToString()) / float.Parse(inst.NumeroTotalTarjetasInstancia.ToString()) * 100));

			return data;
		}

		private ColumnChart CreateColumnChart(ChartPointCollection points, Color clrFill, string legend)
		{
			ColumnChart ch = new ColumnChart(points,clrFill);

			ch.Legend = legend;
			ch.MaxColumnWidth = 40;
			ch.Fill.Color = clrFill;

			ch.LineMarker = new CircleLineMarker(1, clrFill, clrFill);

			return ch;
		}

		private void CargarInformacionTarjeta()
		{
			if(hdNumeroTarjeta.Value != String.Empty)
			{
				string[] informacionTarjeta = TraerInformacionTarjeta(ddlInventarios.SelectedValue.Split('-')[0],Convert.ToInt32(ddlInventarios.SelectedValue.Split('-')[1]),Convert.ToInt32(hdNumeroTarjeta.Value)).Split('&');
				lbCodigoReferencia.Text = informacionTarjeta[1];
				lbNombreReferencia.Text = informacionTarjeta[2];
				lbAlmacen.Text          = informacionTarjeta[3];
				lbUbicacion.Text        = informacionTarjeta[4];
				lbConteoActual.Text     = (Convert.ToInt32(informacionTarjeta[5])+1).ToString();
				lbCantidadConteo1.Text  = ConvertirCantidad(informacionTarjeta[6]);
				lbCantidadConteo2.Text  = ConvertirCantidad(informacionTarjeta[7]);
				lbCantidadConteo3.Text  = ConvertirCantidad(informacionTarjeta[8]);
			}
		}

		private void CargarInformacionConteos(InventarioFisico inst)
		{
			int conteoRelacionado = Convert.ToInt32(ddlConteosConsultas.SelectedValue);

			DataTable dtSource = new DataTable();

			switch(conteoRelacionado)
			{
				case 0:
					dtSource = inst.TarjetasAConteo1;
					break;
				case 1:
					dtSource = inst.TarjetasAConteo2;
					break;
				case 2:
					dtSource = inst.TarjetasAConteo3;
					break;
				case 3:
					dtSource = inst.TarjetasAConteoDefinitivo;
					break;
			}

			lbCantidadTarjetas.Text = dtSource.Rows.Count.ToString();
			dgConteoInformacion.Visible = (dtSource.Rows.Count > 0);

			dgConteoInformacion.DataSource = dtSource;
			dgConteoInformacion.DataBind();
		}
		
		#endregion

		#region Metodos Ajax
		[Ajax.AjaxMethod]
		public string TraerInformacionTarjeta(string prefijoInventario, int numeroInventario, int numeroTarjeta)
		{
			string informacionTarjeta = String.Empty;
			ItemsInventarioFisico inst = new ItemsInventarioFisico(prefijoInventario,numeroInventario,numeroTarjeta);
			if(inst.ConteoActual != -1)
                informacionTarjeta = inst.NumeroTarjeta + "&" + inst.CodigoItemModificado + "&" + inst.NombreItemRelacionado + "&" + DBFunctions.SingleData("SELECT palm_descripcion FROM palmacen WHERE tvig_vigencia='V' and palm_almacen='" + inst.CodigoAlmacen + "'") + "&" + InventarioFisico.ConsultarNombreUbicacion(Convert.ToInt32(inst.CodigoUbicacionInicial)) + "&" + inst.ConteoActual + "&" + inst.Conteo1 + "&" + inst.Conteo2 + "&" + inst.Conteo3;
			return informacionTarjeta;
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
