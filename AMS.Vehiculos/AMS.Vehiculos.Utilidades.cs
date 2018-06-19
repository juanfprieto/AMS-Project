using System;

namespace AMS.Vehiculos
{
	public class Utilidades
	{
		public static string EscaparCadenaAJavascript(string cadena)
		{
			return cadena.Replace("'","\\'");
		}
        public static string validarVendedor(string cod_vend)
        {

            return null;
        }
		public static double ConvertirADouble(string cadena)
		{
			const char separadorDecimal = '.';

			double numeroDouble = 0;

			if (cadena != "")
			{
				string[] partesDoble = cadena.Split(separadorDecimal);

				switch (partesDoble.Length)
				{
					case 1:
						numeroDouble = Convert.ToDouble(cadena);
						break;
					case 2:
						string parteEntera = partesDoble[0];
						string parteDecimal = partesDoble[1];

						string parteDecimalSinCeros = parteDecimal.TrimEnd('0');
					 
						if (parteDecimalSinCeros == "")
							parteDecimalSinCeros = "0";

						numeroDouble = Convert.ToDouble(string.Concat(parteEntera,separadorDecimal,parteDecimalSinCeros));
						break;
				}
			}

			return numeroDouble;
		}

		public static string SepararPorComas(double doble)
		{
			string modena = doble.ToString("C");

			string separadoPorComas = modena.Substring(1);

			return separadoPorComas;
		}
	}
}
