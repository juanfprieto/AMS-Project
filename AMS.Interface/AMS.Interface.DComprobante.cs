/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// Detalle de comprobante.
	/// </summary>
	public class DComprobante
	{
		private string cuenta;
		private string nit;
		private string almacen;
		private string centroCosto;
		private double debito;
		private double credito;
		private double baseIva;
		
		public DComprobante()
		{
			this.cuenta = "";
			this.nit = "";
			this.almacen = "";
			this.centroCosto = "";
			this.debito = 0;
			this.credito = 0;
			this.baseIva = 0;
		}
		
		public DComprobante(string cuenta,string nit,string almacen,
		                    string centroCosto,double debito,
		                    double credito,double baseIva)
		{
			this.cuenta = cuenta;
			this.nit = nit;
			this.almacen = almacen;
			this.centroCosto = centroCosto;
			this.debito = debito;
			this.credito = credito;
			this.baseIva = baseIva;
		}
		
		public string Cuenta
		{
			get
			{
				return this.cuenta;
			}
			set
			{
				this.cuenta = value;
			}
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
		
		public string Almacen
		{
			get
			{
				return this.almacen;
			}
			set
			{
				this.almacen = value;
			}
		}
		
		public string CentroCosto
		{
			get
			{
				return this.centroCosto;
			}
			set
			{
				this.centroCosto = value;
			}
		}
		
		public double Debito
		{
			get
			{
				return this.debito;
			}
			set
			{
				this.debito = value;
			}
		}
		
		public double Credito
		{
			get
			{
				return this.credito;
			}
			set
			{
				this.credito = value;
			}
		}
		
		public double BaseIva
		{
			get
			{
				return this.baseIva;
			}
			set
			{
				this.baseIva = value;
			}
		}
	}
}
