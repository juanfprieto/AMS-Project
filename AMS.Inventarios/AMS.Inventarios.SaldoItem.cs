using System;
using AMS.DB;

namespace AMS.Inventarios
{
	/// <summary>
	/// Descripción breve de Saldos.
	/// </summary>
	public class SaldoItem
	{
		public static double ObtenerCostoPromedio(string mite_codigo, string pano_ano)
		{
			double costoPromedio = 0;

			try
			{
				costoPromedio = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitem WHERE mite_codigo='"+mite_codigo+"' AND pano_ano="+pano_ano+""));
			}
			catch { }

			return costoPromedio;
		}

		public static double ObtenerCostoPromedioHistorico(string mite_codigo, string pano_ano)
		{
			double costoPromedioHistorico = 0;

			try
			{
				costoPromedioHistorico = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitem WHERE mite_codigo='"+mite_codigo+"' AND pano_ano="+pano_ano+""));
			}
			catch { }

			return costoPromedioHistorico;
		}

		public static double ObtenerCostoPromedioAlmacen(string mite_codigo, string pano_ano, string palm_almacen)
		{
			double costoPromedioAlmacen = 0;

			try
			{
				costoPromedioAlmacen = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitemalmacen WHERE mite_codigo='"+mite_codigo+"' AND palm_almacen='"+palm_almacen+"' AND pano_ano="+pano_ano+""));
			}
			catch { }

			return costoPromedioAlmacen;
		}

		public static double ObtenerCostoPromedioHistoricoAlmacen(string mite_codigo, string pano_ano, string palm_almacen)
		{
			double costoPromedioHistoricoAlmacen = 0;

			try
			{
				costoPromedioHistoricoAlmacen = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costpromhist FROM msaldoitemalmacen WHERE mite_codigo='"+mite_codigo+"' AND palm_almacen='"+palm_almacen+"' AND pano_ano="+pano_ano+""));
			}
			catch { }

			return costoPromedioHistoricoAlmacen;
		}

		public static double ObtenerCantidadActual(string mite_codigo, string pano_ano)
		{
			double cantidadActual = 0;

			try
			{
				cantidadActual = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitem WHERE mite_codigo='"+mite_codigo+"' AND pano_ano="+pano_ano+""));
			}
			catch { }

			return cantidadActual;
		}

		public static double ObtenerCantidadActualAlmacen(string mite_codigo, string pano_ano, string palm_almacen)
		{
			double cantidadActualAlmacen = 0;

			try
			{
				cantidadActualAlmacen = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual FROM msaldoitemalmacen WHERE mite_codigo='"+mite_codigo+"' AND palm_almacen='"+palm_almacen+"' AND pano_ano="+pano_ano+"")) ;
			}
			catch { }

			return cantidadActualAlmacen;
		}

		public static bool CrearRegistroSaldoItemAlmacen(string mite_codigo, string pano_ano, string palm_almacen)
		{
			bool filaAfectada = false;

			if(!DBFunctions.RecordExist("SELECT * FROM msaldoitemalmacen WHERE mite_codigo='"+mite_codigo+"' AND palm_almacen='"+palm_almacen+"' AND PANO_ANO="+pano_ano))
			{
				int numeroFilasAfectadas = DBFunctions.NonQuery("insert into msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_cantpendiente,msal_canttransito,msal_cantinveinic) values ('"+mite_codigo+"','"+palm_almacen+"',"+pano_ano+",0,0,0,0,0,0,0)");

				filaAfectada = (numeroFilasAfectadas > 0);
			}

			return filaAfectada;
		}
	}
}
