// created on 06/05/2005 at 10:04
using System;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls;
using Microsoft.CSharp;
using OWC;

namespace AMS.Estadisticos
{
	public class Graficos
	{
		public static string exceptions="";
		public static string strAbsolutePath = ConfigurationManager.AppSettings["PathToGraphics"]+"esttemp.gif";
		
		public static Image Generador_Grafico(System.Web.UI.WebControls.Image imagen, string nombreGrafico, TipoGraficos isEnum, string lgEjeX, string lgEjeY, params string[] strEntrada)
		{
			bool errorConfiguracion = false;
			bool ejes = false;
			int cantidadSeries = 0;

            //ChartSpaceClass objetoGraficoOffice = new ChartSpaceClass();
            ChartSpace objetoGraficoOffice = new ChartSpace();
			objetoGraficoOffice.DataSourceType = ChartDataSourceTypeEnum.chDataSourceTypeSpreadsheet;
			objetoGraficoOffice.HasChartSpaceTitle = true;
			objetoGraficoOffice.ChartSpaceTitle.Caption = nombreGrafico.ToUpper();
			objetoGraficoOffice.ChartSpaceTitle.Font.set_Name("Tahoma");
 	   	    objetoGraficoOffice.ChartSpaceTitle.Font.set_Size(12);
	   		objetoGraficoOffice.ChartSpaceTitle.Font.set_Bold(true);
			WCChart mapa = objetoGraficoOffice.Charts.Add(0);
			WCAxis Ejes;
			mapa.HasLegend = true;
   	        mapa.HasTitle = false;
			if(isEnum == TipoGraficos.PIE)
			{
				mapa.Type = ChartChartTypeEnum.chChartTypePie;
				mapa.SetData(ChartDimensionsEnum.chDimSeriesNames, (int)ChartSpecialDataSourcesEnum.chDataLiteral, "Elementos");
				//revisamos cual es la serie y cuales son los datos
				//serie , A\tB\tC
				//datos , 1\t2\t3
				for(int i=0;i<strEntrada.Length;i++)
				{
					string[] separador = strEntrada[i].Split(',');
					if(separador[0].ToLower().Trim() == "serie")
						mapa.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimCategories,(int)ChartSpecialDataSourcesEnum.chDataLiteral,separador[1]);
					if(separador[0].ToLower().Trim() == "datos")
					{
						mapa.SeriesCollection[0].SetData(ChartDimensionsEnum.chDimValues,(int)ChartSpecialDataSourcesEnum.chDataLiteral,separador[1]);
						WCDataLabels dlValues = mapa.SeriesCollection[0].DataLabelsCollection.Add();
						dlValues.HasPercentage = true;
						dlValues.HasValue = false;
					}
				}
			}
			if(isEnum == TipoGraficos.COLUMNAS || isEnum == TipoGraficos.LINEAS || isEnum == TipoGraficos.BARRAS || isEnum == TipoGraficos.AREA)
			{
				if(isEnum == TipoGraficos.COLUMNAS)
					mapa.Type = ChartChartTypeEnum.chChartTypeColumnClustered;
				if(isEnum == TipoGraficos.LINEAS)
					mapa.Type = ChartChartTypeEnum.chChartTypeLineStacked;
				if(isEnum == TipoGraficos.BARRAS)
					mapa.Type = ChartChartTypeEnum.chChartTypeBarClustered;
				if(isEnum == TipoGraficos.AREA)
					mapa.Type = ChartChartTypeEnum.chChartTypeAreaStacked;
				//serie,n,nombre serie
				//categoria,n,A\tB\tC
				//valores,n,1\t2\t3
				ejes = true;
				for(int i=0;i<strEntrada.Length;i++)
				{
					string[] separador = strEntrada[i].Split(',');
					if((separador[0].ToLower().Trim() == "serie")&&!errorConfiguracion)
					{
						//Buscamos las categorias y valores asociados a esta serie
						int indiceCategoria = BuscarIndice("categoria",separador[1].Trim(),strEntrada);
						int indiceValor = BuscarIndice("valores",separador[1].Trim(),strEntrada);
						if(indiceCategoria == -1 || indiceValor == -1)
							errorConfiguracion = true;
						else
						{
							string[] sepCategoria = strEntrada[indiceCategoria].Split(',');
							string[] sepValor = strEntrada[indiceValor].Split(',');
							mapa.SeriesCollection.Add(cantidadSeries);
							mapa.SeriesCollection[cantidadSeries].SetData(ChartDimensionsEnum.chDimSeriesNames,(int)ChartSpecialDataSourcesEnum.chDataLiteral,separador[2].Trim());
							mapa.SeriesCollection[cantidadSeries].SetData(ChartDimensionsEnum.chDimCategories,(int)ChartSpecialDataSourcesEnum.chDataLiteral,sepCategoria[2].Trim());
							mapa.SeriesCollection[cantidadSeries].SetData(ChartDimensionsEnum.chDimValues,(int)ChartSpecialDataSourcesEnum.chDataLiteral,sepValor[2].Trim());
							cantidadSeries += 1;
						}
					}
				}
			}
			if(isEnum == TipoGraficos.XY)
			{
				mapa.Type = ChartChartTypeEnum.chChartTypeScatterLine;
				//serie,n,nombreSerie
				//valores,n,X,1\t2\t3
				//valores,n,Y,2\t4\t7
				ejes = true;
				for(int i=0;i<strEntrada.Length;i++)
				{
					string[] separador = strEntrada[i].Split(',');
					if((separador[0].ToLower().Trim() == "serie")&&!errorConfiguracion)
					{
						int indiceEjeX = BuscarIndice("valores",separador[1].Trim(),"X",strEntrada);
						int indiceEjeY = BuscarIndice("valores",separador[1].Trim(),"Y",strEntrada);
						if(indiceEjeY == -1 || indiceEjeX == -1)
							errorConfiguracion = true;
						else
						{
							string[] sepEjeX = strEntrada[indiceEjeX].Split(',');
							string[] sepEjeY = strEntrada[indiceEjeY].Split(',');
							mapa.SeriesCollection.Add(cantidadSeries);
							mapa.SeriesCollection[cantidadSeries].SetData(ChartDimensionsEnum.chDimSeriesNames,(int)ChartSpecialDataSourcesEnum.chDataLiteral,separador[2].Trim());
							mapa.SeriesCollection[cantidadSeries].SetData(ChartDimensionsEnum.chDimXValues,(int)ChartSpecialDataSourcesEnum.chDataLiteral,sepEjeX[3].Trim());
							mapa.SeriesCollection[cantidadSeries].SetData(ChartDimensionsEnum.chDimYValues,(int)ChartSpecialDataSourcesEnum.chDataLiteral,sepEjeY[3].Trim());
							cantidadSeries += 1;
						}
					}
				}
			}
			if(ejes)
			{
				//Eje x
				Ejes = mapa.Axes[(int)ChartAxisPositionEnum.chAxisPositionBottom];
				Ejes.HasTitle = true;
                Ejes.Title.Caption = lgEjeX;
                Ejes.Title.Font.set_Name("Tahoma");
      	        Ejes.Title.Font.set_Size(8);
    	        Ejes.Title.Font.set_Bold(true);
				//Eje Y
				Ejes = mapa.Axes[(int)ChartAxisPositionEnum.chAxisPositionLeft];
				Ejes.HasTitle = true;
                Ejes.Title.Caption = lgEjeY;
                Ejes.Title.Font.set_Name("Tahoma");
       	        Ejes.Title.Font.set_Size(8);
      	        Ejes.Title.Font.set_Bold(true);
			}
			if(!errorConfiguracion)
			{
				try
				{
					objetoGraficoOffice.ExportPicture(strAbsolutePath, "gif", 500, 400);
				}
				catch(Exception e){exceptions += "<br>error:"+e.ToString();}
				imagen.ImageUrl = ConfigurationManager.AppSettings["PathToGraphics"]+"esttemp.gif";
			}
			else
				exceptions += "<br>error: Error de configuracion de series, categorias y valores";
			return imagen;
		}
		
		public static int BuscarIndice(string tipoIndice, string valorSerial, string[] lineas)
		{
			int indice = -1;
			for(int i=0;i<lineas.Length;i++)
			{
				string[] sep = lineas[i].Split(',');
				if((sep[0].ToLower().Trim()==tipoIndice)&&(sep[1].Trim()==valorSerial))
					indice = i;
			}
			return indice;
		}
		
		public static int BuscarIndice(string tipoIndice, string valorSerial, string eje, string[] lineas)
		{
			int indice = -1;
			for(int i=0;i<lineas.Length;i++)
			{
				string[] sep = lineas[i].Split(',');
				if((sep[0].ToLower().Trim()==tipoIndice)&&(sep[1].Trim()==valorSerial)&&(sep[2].ToUpper().Trim()==eje))
					indice = i;
			}
			return indice;
		}

	}
}
