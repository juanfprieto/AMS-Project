using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using System.Data;
using WebChart;
using System.Drawing;
using AMS.Tools;

namespace AMS.Marketing
{
	public partial class AnalisisEncuestas : System.Web.UI.UserControl
	{
        public int numEncuestas;

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                DatasToControls bind = new DatasToControls();
                bind.PutDatasIntoDropDownList(ddlEncuesta, "SELECT menc_codiencu, menc_nombencu FROM dbxschema.mencuesta;");
                ddlEncuesta.Items.Insert(0, new ListItem("--Seleccione--", ""));

                plcSeleccion.Visible = true;
                plcResultadosEnc.Visible = false;
                rptPreguntas.Visible = false;
                chtGrafica.Visible = false;
                plcGrafica.Visible = false;
            }
		}

        //Evento al cargar una encuesta
        protected void btnCargar_Click(object sender, System.EventArgs e)
        {
            string validar = validarDatos();
            
            if (validar.Equals(""))
            {
                plcSeleccion.Visible = false;
                DataSet dsEncuesta=new DataSet();
                DBFunctions.Request(dsEncuesta, IncludeSchema.NO,
                    "SELECT * FROM dbxschema.mencuesta WHERE menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "';");
                if (dsEncuesta.Tables[0].Rows.Count != 0)
                {
                    lblEncuesta.Text = dsEncuesta.Tables[0].Rows[0][0].ToString() + " - " + dsEncuesta.Tables[0].Rows[0][1].ToString();
                    lblFechCreacion.Text = Convert.ToDateTime(dsEncuesta.Tables[0].Rows[0][2].ToString()).ToString("yyyy-MM-dd");
                    lblObjetivoEnc.Text = dsEncuesta.Tables[0].Rows[0][4].ToString();
                    lblResponsable.Text = dsEncuesta.Tables[0].Rows[0][3].ToString();
                    lblFechaIni.Text = txtFechaInicio.Text;
                    lblFechafin.Text = txtFechaFin.Text;

                    numEncuestas = Convert.ToInt32(DBFunctions.SingleData(
                       "SELECT COUNT (ppre_codipreg) FROM DBXSCHEMA.DENCUESTARESULTADOS " +
                       "WHERE menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "' and denc_fechresu >= '" + txtFechaInicio.Text + "' and denc_fechresu <= '" + txtFechaFin.Text + "' " +
                       "and ppre_codipreg in (select ppre_codipreg from dbxschema.ddisenoencuesta where menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "') " +
                       "and ppre_codipreg=(select ppre_codipreg from dbxschema.ddisenoencuesta where menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() +"' " +
                       "FETCH FIRST 1 ROW ONLY);"));

                    lblCantEncuesta.Text = numEncuestas.ToString();
                    plcResultadosEnc.Visible = true;

                    cargarPreguntas();
                }

            }
            else
            {
                Utils.MostrarAlerta(Response, "" + validar + "");
                return;
            }
        }

        //validacion de datos, encuesta, fecha inicio, fecha final
        private String validarDatos()
        {
            if (ddlEncuesta.SelectedIndex != 0)
            {
                if (!txtFechaInicio.Text.Equals("") && !txtFechaFin.Text.Equals(""))
                {
                    DateTime dIni = Convert.ToDateTime(txtFechaInicio.Text);
                    DateTime dFin = Convert.ToDateTime(txtFechaFin.Text);
                    if (dFin >= dIni)
                    {
                        return ("");
                    }
                    else
                    {
                        return("La fecha de inicio debe ser menor o igual a la fecha de finalizacion");
                    }
                }
                else
                {
                    return ("Uno o mas campos de Fecha, aun faltan por llenar.");
                }
            }
            else
            {
                return ("Seleccione una encuesta");
            }
        }

        //Se cargan las preguntas y bind de datos
        private void cargarPreguntas()
        {
            DataSet dsPreguntas = new DataSet();
            DBFunctions.Request(dsPreguntas, IncludeSchema.NO,
                    "SELECT rownumber() over() as numero, pr.codigo as codigo, pr.descripcion as pregunta " +
                    "from (SELECT DISTINCT pp.ppre_codipreg as codigo, pp.ppre_descpreg  as descripcion " +
                    "FROM DBXSCHEMA.DENCUESTARESULTADOS as de, DBXSCHEMA.pPreguntaencuesta as pp " +
                    "WHERE de.menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "' and de.denc_fechresu >= '" + txtFechaInicio.Text + "' and de.denc_fechresu <= '" + txtFechaFin.Text + "' " +
                    "and de.ppre_codipreg in (select ppre_codipreg from dbxschema.ddisenoencuesta where menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "') " +
                    "and de.ppre_codipreg = pp.ppre_codipreg and pp.ttip_codigo='C') pr order by numero;");

            if (dsPreguntas.Tables[0].Rows.Count == 0)
            {
                Utils.MostrarAlerta(Response, "No se han encontrado registros para esta encuesta!");
                return;
            }

            rptPreguntas.DataSource = dsPreguntas.Tables[0];
            rptPreguntas.DataBind();
            rptPreguntas.Visible = true;
        }

        //Carga contenido de preguntas
        public void rptPreguntas_Bound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string codPreg = ((DataRowView)e.Item.DataItem).Row.ItemArray[1].ToString();
                DataGrid dgrRespu = (DataGrid)e.Item.FindControl("dgrRespuestas");
                DataSet dsRes = new DataSet();

                DBFunctions.Request(dsRes, IncludeSchema.NO,
                            "select sel.pres_descresp,sum(sel.valor) as porcentaje, sum(sel.valor) as respuestas, sel.pres_descresp as codigo " +
                            "from (SELECT pres.pres_codiresp, pres.pres_descresp, 1 as VALOR " +
                            "FROM DBXSCHEMA.DENCUESTARESULTADOS as de,  DBXSCHEMA.pPreguntaencuesta as pp, DBXSCHEMA.prespuestaencuesta as pres " +
                            "WHERE de.menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "' and de.denc_fechresu >= '" + txtFechaInicio.Text + "' and de.denc_fechresu <= '" + txtFechaFin.Text + "' " +
                            "and de.ppre_codipreg in (select ppre_codipreg from dbxschema.ddisenoencuesta where menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "') " +
                            "and de.ppre_codipreg = pp.ppre_codipreg and pp.ttip_codigo='C' and de.ppre_codipreg='" + codPreg + "' " +
                            "and pres.pres_codiresp = de.pres_codiresp and de.pres_codiresp in (select pres_codiresp " +
                            "from dpreguntarespuesta where ppre_codipreg='" + codPreg + "')) as sel " +
                            "group by pres_descresp " +
                            "union " +
                            "select p.pres_descresp, 0 as porcentaje, 0 as respuestas, p.pres_descresp as codigo " +
                            "from dpreguntarespuesta d, prespuestaencuesta p " +
                            "where d.ppre_codipreg='" + codPreg + "' and  d.pres_codiresp not in " +
                            "(SELECT DISTINCT pres_codiresp " +
                            "FROM dencuestaresultados de " +
                            "WHERE de.menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "' and de.denc_fechresu >= '" + txtFechaInicio.Text + "' and de.denc_fechresu <= '" + txtFechaFin.Text + "'  " +
                            "and de.ppre_codipreg='" + codPreg + "') " +
                            "and d.pres_codiresp = p.pres_codiresp;");


                foreach (DataRow drC in dsRes.Tables[0].Rows)
                {
                    if (drC["porcentaje"].ToString() != "0")
                    {
                        drC["porcentaje"] = (Convert.ToDouble(drC["porcentaje"]) / numEncuestas) * 100;
                        drC["codigo"] = codPreg;
                    }
                }

                dgrRespu.DataSource = dsRes.Tables[0];
                dgrRespu.DataBind();
                dgrRespu.Visible = dsRes.Tables[0].Rows.Count > 0;
            }
        }

        //Generar grafica y obtencion de codigo de pregunta
        public void Graph(Object Sender, RepeaterCommandEventArgs e)
        {
            if (((ImageButton)e.CommandSource).CommandName == "btnGrafica")
            {
                var lpregu = (Label)e.Item.FindControl("lblPreg");
                string codi = lpregu.Text;

                var ddgraf = ((DropDownList)e.Item.FindControl("ddlTipoGrafica"));
                string grafica = ddgraf.SelectedValue.ToString();

                Graficar(codi, grafica);
            }
        }

        //Creacion de la grafica
        public void Graficar(string codigo, string tipoGrafica)
        {
            int i = 0;
            DataTable dtGrafica;

            
            DataSet dsDatos = new DataSet();
            DBFunctions.Request(dsDatos, IncludeSchema.NO,
                            "select sel.pres_descresp,sum(sel.valor) as respuestas " +
                            "from (SELECT pres.pres_codiresp, pres.pres_descresp, 1 as VALOR " +
                            "FROM DBXSCHEMA.DENCUESTARESULTADOS as de,  DBXSCHEMA.pPreguntaencuesta as pp, DBXSCHEMA.prespuestaencuesta as pres " +
                            "WHERE de.menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "' and de.denc_fechresu >= '" + txtFechaInicio.Text + "' and de.denc_fechresu <= '" + txtFechaFin.Text + "' " +
                            "and de.ppre_codipreg in (select ppre_codipreg from dbxschema.ddisenoencuesta where menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "') " +
                            "and de.ppre_codipreg = pp.ppre_codipreg and pp.ttip_codigo='C' and de.ppre_codipreg='" + codigo + "' " +
                            "and pres.pres_codiresp = de.pres_codiresp and de.pres_codiresp in (select pres_codiresp " +
                            "from dpreguntarespuesta where ppre_codipreg='" + codigo + "')) as sel " +
                            "group by pres_descresp " +
                            "union " +
                            "select p.pres_descresp, 0 as respuesta " +
                            "from dpreguntarespuesta d, prespuestaencuesta p " +
                            "where d.ppre_codipreg='" + codigo + "' and  d.pres_codiresp not in " +
                            "(SELECT DISTINCT pres_codiresp " +
                            "FROM dencuestaresultados de " +
                            "WHERE de.menc_codiencu='" + ddlEncuesta.SelectedValue.ToString() + "' and de.denc_fechresu >= '" + txtFechaInicio.Text + "' and de.denc_fechresu <= '" + txtFechaFin.Text + "'  " +
                            "and de.ppre_codipreg='" + codigo + "') " +
                            "and d.pres_codiresp = p.pres_codiresp;");

            dtGrafica = pivotDataTable((DataTable)dsDatos.Tables[0], DBFunctions.SingleData("select ppre_descpreg from ppreguntaencuesta where ppre_codipreg = '" + codigo + "'"));

            string titulo;
            Random rndCol = new Random();
            i = 0;

            foreach (DataRow drI in dtGrafica.Rows)
            {
                titulo = drI[0].ToString();
                switch (tipoGrafica)
                {
                    case "L":
                        chtGrafica.Charts.Add(CreateLineChart(GenerarPuntos(dtGrafica, i), Color.FromArgb(rndCol.Next(250), rndCol.Next(250), rndCol.Next(250)), titulo));
                        break;
                    case "B":
                        chtGrafica.Charts.Add(CreateColumnChart(GenerarPuntos(dtGrafica, i), Color.FromArgb(rndCol.Next(250), rndCol.Next(250), rndCol.Next(250)), titulo));
                        break;
                    case "P":
                        chtGrafica.Charts.Add(CreatePieChart(GenerarPuntos(dtGrafica, i), Color.FromArgb(rndCol.Next(250), rndCol.Next(250), rndCol.Next(250)), titulo));
                        break;
                }
                i++;
            }

            chtGrafica.YValuesInterval = 1;
            chtGrafica.YTitle.Text = "V O T O S ";
            chtGrafica.XTitle.Text = "RESPUESTAS DE LA PREGUNTA SELECCIONADA";
            Escalar();
            chtGrafica.Legend.Position = LegendPosition.Bottom;
            chtGrafica.RedrawChart();
            chtGrafica.Visible = true;
            rptPreguntas.Visible = false;
            plcGrafica.Visible = true;
        }

        //Configuraciones de grafica
        private ColumnChart CreateColumnChart(ChartPointCollection points, Color clrFill, string legend)
        {
            ColumnChart ch = new ColumnChart(points, clrFill);
            ch.Legend = legend;
            ch.Fill.Color = clrFill;
            ch.LineMarker = new CircleLineMarker(1, clrFill, clrFill);
            return ch;
        }

        //Configuraciones de grafica
        private PieChart CreatePieChart(ChartPointCollection points, Color clrFill, string legend)
        {
            PieChart ch = new PieChart(points, clrFill);
            ch.Legend = legend;
            ch.Fill.Color = clrFill;
            ch.LineMarker = new CircleLineMarker(1, clrFill, clrFill);
            return ch;
        }

        //Configuraciones de grafica
        private LineChart CreateLineChart(ChartPointCollection points, Color clrFill, string legend)
        {
            LineChart ch = new LineChart(points, clrFill);
            ch.Legend = legend;
            ch.Fill.Color = clrFill;
            ch.Line.Width = 5;
            ch.LineMarker = new CircleLineMarker(10, clrFill, clrFill);
            return ch;
        }

        //Configuraciones de grafica
        private ChartPointCollection GenerarPuntos(DataTable dtDatos, int fila)
        {
            ChartPointCollection data = new ChartPointCollection();
            float valor = 0;

            for (int c = 1; c < dtDatos.Columns.Count; c++)
            {
                try { valor = (float)Convert.ToDecimal(dtDatos.Rows[fila][c]); }
                catch { valor = 0; }
                data.Add(new ChartPoint(dtDatos.Columns[c].ColumnName, valor));
            }

            return data;
        }

        //Escalar grafica
        private void Escalar()
        {
            int ancho, alto;
            try
            {
                ancho = Convert.ToInt16(txtAncho.Text);
                if (ancho < 1 || ancho >= 10000)
                    throw (new Exception());
            }
            catch
            {
                ancho = 800;
                txtAncho.Text = "800";
            }
            try
            {
                alto = Convert.ToInt16(txtAlto.Text);
                if (alto < 1 || alto >= 10000)
                    throw (new Exception());
            }
            catch
            {
                alto = 600;
                txtAlto.Text = "600";
            }
            chtGrafica.Width = ancho;
            chtGrafica.Height = alto;
        }

        //Boton de retorno a las preguntas, desde el grafico
        public void btnVolver_Click(Object Sender, EventArgs E)
        {
            chtGrafica.Visible = false;
            plcGrafica.Visible = false;
            rptPreguntas.Visible = true;
        }

        public DataTable pivotDataTable(DataTable dsTabla, String nombre)
        {
            DataTable tablaPivot = new DataTable();
            
            if (dsTabla.Rows.Count != 0)
            {
                tablaPivot.Columns.Add(new DataColumn("Votos", System.Type.GetType("System.String")));

                for(int i=0; i < dsTabla.Rows.Count; i++)
                {
                    tablaPivot.Columns.Add(new DataColumn(dsTabla.Rows[i][0].ToString(), System.Type.GetType("System.Double")));
                }

                DataRow row = tablaPivot.NewRow();
                row[0] = nombre;

                for (int j = 0; j < dsTabla.Rows.Count; j++)
                {
                    row[j + 1] = dsTabla.Rows[j][1].ToString();
                }

                tablaPivot.Rows.Add(row);

                return tablaPivot;
            }
            else
            {
                return null;
            }
        }




	}
}