using System;
using System.Configuration;

namespace AMS.Tools
{
	/// <summary>
	/// Manejador VINs
	/// </summary>
	public class VINs
	{
		public VINs()
		{}
		
		/// <summary>
		/// Validar VIN
		/// </summary>
		/// <param name="vinOriginal">VIN a validar</param>
		/// <returns>valido?</returns>
		public static bool ValidarVIN(string vinOriginal)
		{
			string vinValida="";
			if(vinOriginal.Length!=17)
				return false;
            if (ConfigurationManager.AppSettings["ValidarVIN"] == null || !Convert.ToBoolean(ConfigurationManager.AppSettings["ValidarVIN"]))
                return true;
			vinValida=GenerarVIN(vinOriginal);
			return vinValida.Equals(vinOriginal);
		}

		/// <summary>
		/// Genarar VIN
		/// </summary>
		/// <param name="vinOriginal">Vin Inicial</param>
		/// <returns>VIN Generado</returns>
		public static string GenerarVIN(string vinInicial)
		{
			string letrasVIN ="ABCDEFGHJKLMNPRSTUVWXYZ";
			string vletrasVIN="12345678123457923456789";
			int [] tiposVIN = new int[17];
			int [] valoresVIN = new int[17];
			int n, multiplo=11, sumaV=0, total=0;
			int []cPosicion={8,7,6,5,4,3,2,10,1,9,8,7,6,5,4,3,2};
			string nuevoVIN="";
			
			//El vin original debe tener 17 caracteres
			if(vinInicial.Length!=17)
				return(vinInicial);
			vinInicial=vinInicial.ToUpper();
			
			//Averiguar tipos
			for(n=0;n<17;n++)
			{
				if(n!=8)
				{
					if(Char.IsNumber(vinInicial[n]))
					{
						tiposVIN[n]=1; //Numero
						valoresVIN[n]=Int32.Parse(vinInicial[n].ToString())*cPosicion[n];
					}
					else
					{
						tiposVIN[n]=2; //Letra
						valoresVIN[n]=Int32.Parse(vletrasVIN[letrasVIN.IndexOf(vinInicial[n])].ToString())*cPosicion[n];
					}
					sumaV+=valoresVIN[n];
					nuevoVIN+=vinInicial[n];
				}
				else
				{
					nuevoVIN+="?";
					tiposVIN[n]=valoresVIN[n]=0;
				}
			}
			total=(int)Math.Floor(((double)sumaV)/multiplo);
			total=-((total*multiplo)-sumaV);
			if(total<=9)
				nuevoVIN=nuevoVIN.Replace("?",total.ToString()[0].ToString());
			else
				nuevoVIN=nuevoVIN.Replace("?","X");
			return(nuevoVIN);
		}
	}
}
