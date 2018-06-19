/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */
 
 
using System;
namespace AMS.Interface
{
	/// <summary>
	/// MItem.
	/// </summary>
	public class Mitem
	{
		private string codigo;
		private string origen;
		
		public Mitem()
		{
			this.codigo = "";
			this.origen = "";
		}
		
		public Mitem(string codigo,string origen)
		{
			this.codigo = codigo;
			this.origen = origen;
		}
		
		public string Codigo
		{
			get
			{
				return this.codigo;
			}
			set
			{
				this.codigo = value;
			}
		}
		
		public string Origen
		{
			get
			{
				return this.origen;
			}
			set
			{
				this.origen = value;
			}
		}
		
		public override string ToString()
		{
			return "Codigo: "+Codigo+" - Origen: "+Origen;
		}
	}
}
