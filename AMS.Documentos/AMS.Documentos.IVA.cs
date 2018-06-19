using System;
using System.Collections;
using System.Data;
using AMS.DB;

namespace AMS.Documentos
{
	/// <summary>
	/// Define metodos estáticos para el cálculo del impuesto al valor
	/// agregado (IVA)
	/// </summary>
	public class IVA
	{
		#region Constructores

		/// <summary>
		/// Constructor por defecto de la clase
		/// </summary>
		public IVA()
		{
			
		}

		#endregion
        
		#region Métodos

		/// <summary>
		/// Devuelve el regimen de iva que corresponde al nit especificado
		/// </summary>
		/// <param name="nit">string: nit del cliente o proveedor</param>
		/// <returns>string: regimen de iva del nit</returns>
		public static string RegimenIvaNit(string nit)
		{
			string regimen="";
			regimen=DBFunctions.SingleData("SELECT treg_regiiva FROM mnit WHERE mnit_nit='"+nit+"'");
			return regimen;
		}

		/// <summary>
		/// Devuelve la nacionalidad del nit especificado
		/// </summary>
		/// <param name="nit">string: nit del cliente o proveedor</param>
		/// <returns>string: nacionalidad del nit especificado</returns>
		public static string NacionalidadNit(string nit)
		{
			string nac="";
			nac=DBFunctions.SingleData("SELECT tnac_tiponaci FROM mnit WHERE mnit_nit='"+nit+"'");
			return nac;
		}
		
		/// <summary>
		/// Calcula el IVA del valor base
		/// </summary>
		/// <param name="nit">string: nit del cliente o proveedor</param>
		/// <param name="tipoProceso">string: tipo de proceso (C)Cliente (P)Proveedor</param>
		/// <param name="excencion">string: excento del impuesto (S)Si (N)No</param>
		/// <param name="porcIva">double: porcentaje de iva</param>
		/// <param name="valorBase">double: valor sobre el cual se calculara el impuesto</param>
		/// <returns>double: iva calculado</returns>
		public static double CalcularIva(string nit,string tipoProceso,string excencion,double porcIva,double valorBase)
		{
			double totalIva=0;
			//Si es un calculo de iva para un cliente
			if(tipoProceso=="C")
			{
				//Si es un extranjero no se le calcula IVA
				if(NacionalidadNit(nit)!="E")
				{
					if(excencion!="S")
						totalIva=valorBase*(porcIva/100);
				}
			}
			//Si es un calculo de iva para un proveedor
			else if(tipoProceso=="P")
			{
				//Si es un regimen simplificado no le calculo IVA
				if(RegimenIvaNit(nit)!="S")
				{
					if(excencion!="S")
						totalIva=valorBase*(porcIva/100);
				}
			}
			return totalIva;
		}

		/// <summary>
		/// Calcula el valor base de un total con iva
		/// </summary>
		/// <param name="porcIva">double: porcentaje de iva</param>
		/// <param name="valorTotal">double: valor total con iva</param>
		/// <returns>double: valor sin iva</returns>
		public static double CalcularBase(double porcIva,double valorTotal)
		{
			double valorSinIva=0;
			valorSinIva=valorTotal/(1+(porcIva/100));
			return valorSinIva;
		}

		/// <summary>
		/// Calcula el valor total con IVA
		/// </summary>
		/// <param name="nit">string: nit del cliente o proveedor</param>
		/// <param name="tipoProceso">string: tipo de proceso (C)Cliente (P)Proveedor</param>
		/// <param name="excencion">string: excento del impuesto (S)Si (N)No</param>
		/// <param name="porcIva">double: porcentaje de iva</param>
		/// <param name="valorBase">double: valor sobre el cual se calculara el impuesto</param>
		/// <returns>double: total base+iva</returns>
		public static double CalcularTotalConIva(string nit,string tipoProceso,string excencion,double porcIva,double valorBase)
		{
			double valorTotal=0;
			double valorIva=IVA.CalcularIva(nit,tipoProceso,excencion,porcIva,valorBase);
			valorTotal=valorBase+valorIva;
			return valorTotal;
		}

		#endregion
	}
}
