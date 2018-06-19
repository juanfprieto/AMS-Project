using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using AMS.Tools;

namespace AMS.Reportes
{
    public class Graficas
    {
        Chart grafica = new Chart();
        DataTable pDatos;
        String ptipoGraf;
        ArrayList pvecEncabezados;
        Boolean pXY;
        Random randonGen = new Random();
        Boolean pTodasFilas;
        Obout.Grid.Grid grid;
        ArrayList pSeleccionados;

        //Obtiene un objeto con toda la configuracion de la grafica realizada.
        public Chart ObtenerGrafica(DataTable datos, String tipoGraf, ArrayList vecEncabezados, Boolean xy, Boolean todasfilas, ArrayList seleccionados)
        {
            grafica = new Chart();
            pDatos = datos;
            ptipoGraf = tipoGraf;
            pvecEncabezados = vecEncabezados;
            pXY = xy;
            pTodasFilas = todasfilas;
            pSeleccionados = seleccionados;

            prepararSpline(tipoGraf);

            if (tipoGraf == "Columnas 3D" || tipoGraf == "Piramide")
                grafica.ChartAreas["AreaGrafica"].Area3DStyle.Enable3D = true;
            else
                grafica.ChartAreas["AreaGrafica"].Area3DStyle.Enable3D = false;

            return grafica;
        }

        //Configura todos los componentes y datos necesarios para visualizar la grafica.
        protected void prepararSpline(String tipoGraf)
        {
            grafica.ID = "Grafica";
            grafica.Titles.Add("Grafica de Resultados");
            grafica.Legends.Add("Legend1");
            grafica.Palette = ChartColorPalette.BrightPastel;
            grafica.BackColor = Color.WhiteSmoke;
            grafica.Width = 600;
            grafica.Height = 400;
            grafica.BorderlineDashStyle = ChartDashStyle.Solid;
            grafica.BackGradientStyle = GradientStyle.TopBottom;
            grafica.BorderlineWidth = 2;
            grafica.BorderlineColor = Color.DarkGreen;
            grafica.ImageType = ChartImageType.Png;
            //grafica.ImageLocation = "../grid/localization/ChartPic_#SEQ(300,3)";
            //grafica.ImageStorageMode = ImageStorageMode.UseImageLocation;
            
            //Creacion de las series de datos.
            for (int k = 1; k < pvecEncabezados.Count; k++)
            {
                DictionaryEntry entry = (DictionaryEntry)pvecEncabezados[k];
                Series series = crearSerie(entry.Key.ToString(), (TipoCampo)entry.Value, (DictionaryEntry)pvecEncabezados[0]);
                grafica.Series.Add(series);
            }

            ChartArea area = new ChartArea("AreaGrafica");
            area.BorderColor = Color.Azure;
            area.BackSecondaryColor = Color.Brown;
            area.BackColor = Color.OldLace;
            area.ShadowColor = Color.Green;
            area.BackGradientStyle = GradientStyle.TopBottom;
            area.Area3DStyle.Rotation = 10;
            area.Area3DStyle.Perspective = 10;
            area.Area3DStyle.Inclination = 18;
            area.Area3DStyle.IsRightAngleAxes = false;
            area.Area3DStyle.WallWidth = 3;
            area.Area3DStyle.IsClustered = false;
            area.Area3DStyle.PointDepth = 200;
            area.Area3DStyle.PointGapDepth = 50;
            area.AxisX.Interval = 1;
            area.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;

            if (tipoGraf == "Torta")
            {
                area.AxisY2.MajorGrid.Enabled = false;
                area.AxisY2.MajorTickMark.Enabled = false;
                area.AxisX2.MajorGrid.Enabled = false;
                area.AxisX2.MajorTickMark.Enabled = false;
                area.AxisY.MajorGrid.Enabled = false;
                area.AxisY.MajorTickMark.Enabled = false;
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.MajorTickMark.Enabled = false;
                area.Area3DStyle.PointGapDepth = 900;
                area.Area3DStyle.Rotation = 162;
                area.Area3DStyle.WallWidth = 25;
            }
            
            grafica.ChartAreas.Add(area);
        }

        //Creacion y configuracion de una serie de datos.
        protected Series crearSerie(String nombreCategoria, TipoCampo tipoCampo, DictionaryEntry entryCabecera)
        {
            //Chart2.Series["Series2"].ChartType = SeriesChartType.Bar;
            //Chart2.Titles[0].Text = "Bar Chart";
            
            Series series = new Series(nombreCategoria);

            if (ptipoGraf == "Spline")
                series.ChartType = SeriesChartType.Spline;
            else if (ptipoGraf == "Line")
                series.ChartType = SeriesChartType.Line;
            else if (ptipoGraf == "Barras")
                series.ChartType = SeriesChartType.Bar;
            else if (ptipoGraf == "Columnas")
                series.ChartType = SeriesChartType.Column;
            else if (ptipoGraf == "Columnas 3D")
                series.ChartType = SeriesChartType.Column;
            else if (ptipoGraf == "Torta")
                series.ChartType = SeriesChartType.Pie;
            else if (ptipoGraf == "Piramide")
                series.ChartType = SeriesChartType.Pyramid;

            series.BorderWidth = 3;
            series.ShadowOffset = 2;
            series.Color = Color.FromArgb(randonGen.Next(255), randonGen.Next(255), randonGen.Next(255));
            series.ChartArea = "AreaGrafica";
            series.XValueType = ChartValueType.String;
            series.Font = new Font("Trebuchet MS", 8, FontStyle.Bold);
            series["CollectedThreshold"] = "10"; //8, 10 , 15
            series["CollectedLabel"] = "Other";
            series["CollectedLegendText"] = "Other";
            series["CollectedSliceExploded"] = "true";
            series["CollectedColor"] = "Green";
            series["PointWidth"] = "1.0";
            series["DrawingStyle"] = "Emboss";  //LightToDark,Wedge,Cylinder,Emboss,Default
            series["CollectedToolTip"] = "Other";
            series["ShowMarkerLines"] = "True";
            series.Label = "#PERCENT{P2}";
            
            if (ptipoGraf == "Torta")
            {
                series.CustomProperties = "PieDrawingStyle=Concave, CollectedLabel=Other, MinimumRelativePieSize=20";
                series["PieLabelStyle"] = "Outside";
                series["PieLineColor"] = "Black";
                series.MarkerStyle = MarkerStyle.Circle;
                series.LabelBackColor = Color.Empty;
                series.LegendText = "#VALX";
            }
            else if (ptipoGraf == "Piramide")
            {
                series["PyramidLabelStyle"] = "Outside"; //OutsideInColumn, Outside, Inside, Disabled
                series["PyramidOutsideLabelPlacement"] = "Left"; //Right, Left
                series["PyramidPointGap"] = "1";  //1,2,3
                series["PyramidMinPointHeight"] = "0";
                series["Pyramid3DRotationAngle"] = "10";
                series["Pyramid3DDrawingStyle"] = "SquareBase";  //SquareBase,CircularBase
                series["PyramidValueType"] = "Surface"; //Linear, Surface
                series.LegendText = "#VALX";
                //series["PyramidInsideLabelAlignment"] = "Left"; //Left, Right
            }

            int cabecera = (int)entryCabecera.Value;
            
            for (int col = 0; col < pDatos.Columns.Count; col++)
            {
                if (pDatos.Columns[col].ColumnName.ToUpper() == nombreCategoria.ToUpper())
                {
                    for (int j = 0; j < pDatos.Rows.Count; j++)
                    {
                        if (pTodasFilas)
                        {
                            series.Points.AddY(Double.Parse(pDatos.Rows[j][col].ToString()));
                            series.Points[j].AxisLabel = pDatos.Rows[j][cabecera].ToString();
                        }
                        else
                        {
                            if (pSeleccionados != null)
                            {
                                int cont = 0;
                                foreach (Hashtable oRecord in pSeleccionados)
                                {
                                    String valor = oRecord[nombreCategoria].ToString();
                                    String campo = oRecord[entryCabecera.Key.ToString()].ToString();
                                    series.Points.AddY(Double.Parse(valor));
                                    series.Points[cont].AxisLabel = campo;
                                    cont++;
                                }
                                break;
                            }
                        }
                        
                    }
                }
            }

            return series;
        }


    }
}