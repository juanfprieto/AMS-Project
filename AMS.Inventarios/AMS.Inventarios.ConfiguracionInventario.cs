using System;
using AMS.DB;

namespace AMS.Inventarios
{
	/// <summary>
	/// Descripción breve de ConfiguracionInventario.
	/// </summary>
	public class ConfiguracionInventario
	{
		public static string Ano
		{
			get
			{
				string ano = string.Empty; 

				try
				{
					ano = DBFunctions.SingleData("Select pano_ano from DBXSCHEMA.CINVENTARIO");
				}
				catch { }

				return ano;
			}
		}

		public static string Mes
		{
			get
			{
				string mes = string.Empty; 

				try
				{
					mes = DBFunctions.SingleData("SELECT pmes_mes from cinventario");
				}
				catch { }

				return mes;
			}
		}
	}
}
