using System;
using System.Collections;

namespace AMS.Tools
{
	public class Converter
	{
		protected string exceptions;
		
		public Converter()
		{
			exceptions = "";
		}
		
		public string Convertir_Decimal_Hexadecimal(int numero)
		{
			ArrayList resultado = new ArrayList();
			int temporal = numero;
			while(temporal>=16)
			{
				int resultadoModulo = (temporal%16);
				resultado.Add(this.Valor_Hexadecimal_Letra(resultadoModulo));
				temporal = temporal/16;
			}
			resultado.Add(this.Valor_Hexadecimal_Letra(temporal));
			string resultadoHexadecimal = "";
			for(int i=resultado.Count-1;i>=0;i--)
				resultadoHexadecimal += resultado[i].ToString();
			if(resultadoHexadecimal.Length==1)
				resultadoHexadecimal = "0"+resultadoHexadecimal;
			return resultadoHexadecimal;
		}
		
		public string Valor_Hexadecimal_Letra(int numero)
		{
			string retorno = "";
			if(numero==10)
				retorno="a";
			else if(numero==11)
				retorno="b";
			else if(numero==12)
				retorno="c";
			else if(numero==13)
				retorno="d";
			else if(numero==14)
				retorno="e";
			else if(numero==15)
				retorno="f";
			else if(numero==16)
				retorno="0";
			else
				retorno = numero.ToString();
			return retorno;
		}
	}
}
