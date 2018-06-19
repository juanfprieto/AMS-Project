/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// Cempresa.
	/// </summary>
	public class Cempresa
	{
		private string nit;
		private string regimenIva;
		
		public Cempresa()
		{
			this.nit = "";
			this.regimenIva = "";
		}
		
		public Cempresa(string nit,string regimenIva)
		{
			this.nit = nit;
			this.regimenIva = regimenIva;
		}
		
		public string Nit
		{
			get
			{
				return this.nit;
			}
			set
			{
				this.nit = value;
			}
		}
		
		public string RegimenIva
		{
			get
			{
				return this.regimenIva;
			}
			set
			{
				this.regimenIva = value;
			}
		}
		
		public override string ToString()
		{
			return " Nit: "+this.nit+ " -RegimenIva: "+this.regimenIva;
		}
	}
}
