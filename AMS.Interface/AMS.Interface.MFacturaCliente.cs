/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/5/2005
 * Time: 7:52 AM
 * 
 * 
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// MFacturaCliente.
	/// </summary>
	public class MFacturaCliente
	{
		private string codigoDocumento;
		private int numeroDocumento;
		private string nit;
		private string fechaFactura;
		private double valorIva;
		private double valorFactura;
		private double valorRetencion;
		private string almacen;
		private string regimenIva;
		
		public MFacturaCliente()
		{
			this.codigoDocumento = "";
			this.numeroDocumento = 0;
			this.nit = "";
			this.fechaFactura = "";
			this.valorIva = 0;
			this.valorFactura = 0;
			this.valorRetencion = 0;
			this.almacen = "";
			this.regimenIva = "";
		}
		
		public MFacturaCliente(string codigoDocumento,int numeroDocumento,
		                         string nit,string fechaFactura, double valorIva, double valorFactura,
		                         double valorRetencion,string almacen,string regimenIva)
		{
			this.codigoDocumento = codigoDocumento;
			this.numeroDocumento = numeroDocumento;
			this.nit = nit;
			this.fechaFactura = fechaFactura;
			this.valorIva = valorIva;
			this.valorFactura = valorFactura;
			this.valorRetencion = valorRetencion;
			this.almacen = almacen;
			this.regimenIva = regimenIva;
		}

		public string CodigoDocumento
		{
			get
			{
				return this.codigoDocumento;
			}
			set
			{
				this.codigoDocumento = value;
			}
		}

		public int NumeroDocumento
		{
			get
			{
				return this.numeroDocumento;
			}
			set
			{
				this.numeroDocumento = value;
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

		public string FechaFactura
		{
			get
			{
				return this.fechaFactura;
			}
			set
			{
				this.fechaFactura = value;
			}
		}

		public double ValorFactura
		{
			get
			{
				return this.valorFactura;
			}
			set
			{
				this.valorFactura = value;
			}
		}

		public double ValorIva
		{
			get
			{
				return this.valorIva;
			}
			set
			{
				this.valorIva = value;
			}
		}
		
		public double ValorRetencion
		{
			get
			{
				return this.valorRetencion;
			}
			set
			{
				this.valorRetencion = value;
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
			return " CodigoDocumento: "+this.codigoDocumento+" -NumeroDocumento: "+
				this.numeroDocumento+" -Nit: "+this.nit+" -FechaFactura: "+this.fechaFactura+
				" -ValorFactura: "+this.valorFactura+" -ValorIva: "+this.valorIva+
				" -Almace: "+this.almacen;
		}
	}
}
