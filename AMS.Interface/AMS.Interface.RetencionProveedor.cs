/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// MFacturaProveedorRetencion.
	/// </summary>
	public class RetencionProveedor
	{
		private string codigoDocumento;
		private int numeroDocumento;
		private string codigoRetencion;
		private double valorRetencion;
		
		public RetencionProveedor()
		{
			this.codigoDocumento = "";
			this.numeroDocumento = 0;
			this.codigoRetencion = "";
			this.valorRetencion = 0;
		}
		
		public RetencionProveedor(string codigoDocumento,int numeroDocumento,
		                          string codigoRetencion,double valorRetencion)
		{
			this.codigoDocumento = codigoDocumento;
			this.numeroDocumento = numeroDocumento;
			this.codigoRetencion = codigoRetencion;
			this.valorRetencion = valorRetencion;
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
		
		public string CodigoRetencion
		{
			get
			{
				return this.codigoRetencion;
			}
			set
			{
				this.codigoRetencion = value;
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
		
		public override string ToString()
		{
			return " -CodigoDocumento: "+this.codigoDocumento+
				" -NumeroDocumento: "+this.numeroDocumento+
				" -CodigoRetencion: "+this.codigoRetencion+
				" -ValorRetencion: "+this.valorRetencion;
		}
	}
}
