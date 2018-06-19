using System;
using System.Data;
using AMS.DB;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public class Remision
	{
		public static bool CrearRemision(
			string prefijoRemision, 
			ref uint numeroRemision, 
			string almacenOrigen,
			string almacenDestino, 
			DataTable dtItems, 
			string codVendedor, 
			ref string processMsg,
            string observaciones)
		{
			bool status = true;

			string ano_cinv = DBFunctions.SingleData("SELECT pano_ano from cinventario");

            //Primero revisamos si existe algun registro en ditems que tenga como documento de respaldo con este numero
            bool existe = true;
            while (existe)
            {
			    if (DBFunctions.RecordExist("SELECT * FROM ditems WHERE pdoc_codigo='" + prefijoRemision + "' AND dite_numedocu=" + numeroRemision.ToString() + ""))
                    numeroRemision += 1;
                else
                    existe = false;
            }

			//Creamos el objeto manejador de los movimientos de kardex con el constructor #3
			Movimiento Mov = new Movimiento(prefijoRemision,numeroRemision,40,DBFunctions.SingleData("SELECT mnit_nit FROM cempresa"),DateTime.Now,codVendedor,DBFunctions.SingleData("SELECT tvend_codigo FROM pvendedor WHERE pven_codigo='"+codVendedor+"'"),"","N");
            Mov.Observaciones = observaciones;

			for(int i=0;i<dtItems.Rows.Count;i++)
			{
				//En primer lugar se debe recalcular que tanto se puede asignar
				//Es probable que otro proceso halla modificado las cantidades 

				//double ca = Referencias.ConsultarDisponibilidad(dtItems.Rows[i][0].ToString(),almacenOrigen,(int)Convert.ToDouble(dtItems.Rows[i][5]));
				double ca = Referencias.ConsultarDisponibilidad(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),almacenOrigen,0);
				double cantidadDevolver = 0;

				try
				{
					cantidadDevolver = Convert.ToDouble(dtItems.Rows[i]["CANTIDADTRANSLADO"]);
				}
				catch { }
				
				if(ca > 0 && (ca - cantidadDevolver) >= 0)
				{
					#region Salida de Almacén

					double costoPromedio = 0;
					double costoPromedioHistorico = 0;
					double costoPromedioAlmacen = 0;
					double costoPromedioHistoricoAlmacen = 0;
					double cantidadInicialInventario = 0;
					double cantidadInicialInventarioAlmacen = 0;

					costoPromedio = SaldoItem.ObtenerCostoPromedio(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv);
					costoPromedioHistorico = SaldoItem.ObtenerCostoPromedioHistorico(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv);
					costoPromedioAlmacen = SaldoItem.ObtenerCostoPromedioAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenOrigen);
					costoPromedioHistoricoAlmacen = SaldoItem.ObtenerCostoPromedioHistoricoAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenOrigen);
					cantidadInicialInventario = SaldoItem.ObtenerCantidadActual(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv);
					cantidadInicialInventarioAlmacen = SaldoItem.ObtenerCantidadActualAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenOrigen);

					//Ahora creamos este registro de salida del almacen de origen, en este caso la cantidad se hace negativa
					Mov.InsertaFila(
						dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),
						cantidadDevolver*(-1),
						Convert.ToDouble(dtItems.Rows[i]["COSTOPROMEDIOORIGEN"]),
						costoPromedio,
						costoPromedioAlmacen,
						0,
						0,
						0,
						costoPromedioHistorico,
						costoPromedioHistoricoAlmacen,
						Convert.ToDouble(dtItems.Rows[i]["COSTOPROMEDIOORIGEN"]),
						cantidadInicialInventario,
						cantidadInicialInventarioAlmacen,
						almacenOrigen);
					#endregion

					#region Entrada de Almacén
					//Ahora creamos el registro de entrada del almacén de destino, en este caso la cantidad se hace negativa

					costoPromedio = SaldoItem.ObtenerCostoPromedio(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv);
					costoPromedioHistorico = SaldoItem.ObtenerCostoPromedioHistorico(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv);
					costoPromedioAlmacen = Movimiento.CalcularCostoPromedio(
						cantidadDevolver,
						SaldoItem.ObtenerCostoPromedioAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenOrigen),
						SaldoItem.ObtenerCantidadActualAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenDestino),
						SaldoItem.ObtenerCostoPromedioAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenDestino),
						0);
					costoPromedioHistoricoAlmacen = Movimiento.CalcularCostoPromedio(
						cantidadDevolver,
						SaldoItem.ObtenerCostoPromedioAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenOrigen),
						SaldoItem.ObtenerCantidadActualAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenDestino),
						SaldoItem.ObtenerCostoPromedioHistoricoAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenDestino),
						0);
					cantidadInicialInventario = SaldoItem.ObtenerCantidadActual(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv);
					cantidadInicialInventarioAlmacen = SaldoItem.ObtenerCantidadActualAlmacen(dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),ano_cinv,almacenDestino);

					Mov.InsertaFila(
						dtItems.Rows[i]["CODIGOORIGINAL"].ToString(),
						cantidadDevolver,
						Convert.ToDouble(dtItems.Rows[i]["COSTOPROMEDIOORIGEN"]),
						costoPromedio,
						costoPromedioAlmacen,
						0,
						0,
						0,
						costoPromedioHistorico,
						costoPromedioHistoricoAlmacen,
						Convert.ToDouble(dtItems.Rows[i]["COSTOPROMEDIOORIGEN"]),
						cantidadInicialInventario,
						cantidadInicialInventarioAlmacen,
						almacenDestino);
					#endregion
				}
			}

			//Ahora guardamos el registro de Movimientos
			if(Mov.RealizarMov(true))
			{
				processMsg +="<br>Bien <br>"+Mov.ProcessMsg;
				DBFunctions.NonQuery("UPDATE pdocumento SET pdoc_ultidocu="+numeroRemision.ToString()+" WHERE pdoc_codigo='"+prefijoRemision+"'");
			}
			else
			{
				status = false;
				processMsg +="<br>ERROR <br>"+Mov.ProcessMsg;
			}

			return status;
		}
	}
}
