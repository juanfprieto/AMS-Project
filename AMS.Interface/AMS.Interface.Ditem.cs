/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// Ditem.
	/// </summary>
	public class Ditem : Mitem
	{
		private string codigoDocumento;
		private int numeroDocumento;
		private string centroCosto;
		private double cantidad;
		private double valorUnitario;
		private double costoPromedio;
		private double costoPromedioHistorico;
		private double porcentajeIva;
		private double porcentajeDescuento;
		private int tipoMovimiento;
		private string almacen;

		public Ditem()
		{
			this.codigoDocumento = "";
			this.numeroDocumento = 0;
			this.centroCosto = "";
			this.cantidad = 0;
			this.valorUnitario = 0;
			this.costoPromedio = 0;
			this.costoPromedioHistorico = 0;
			this.porcentajeIva = 0;
			this.porcentajeDescuento = 0;
			this.almacen = "";
		}
		
		public Ditem(string codigoDocumento,int numeroDocumento, string centroCosto,
		             double cantidad, double valorUnitario, double costoPromedio,
		             double costoPromedioHistorico,double porcentajeIva,double porcentajeDescuento,string almacen)
		{
			this.codigoDocumento = codigoDocumento;
			this.numeroDocumento = numeroDocumento;
			this.centroCosto = centroCosto;
			this.cantidad = cantidad;
			this.valorUnitario = valorUnitario;
			this.costoPromedio = costoPromedio;
			this.costoPromedioHistorico = costoPromedioHistorico;
			this.porcentajeIva = porcentajeIva;
			this.porcentajeDescuento = porcentajeDescuento;
			this.almacen = almacen;
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

		public double Cantidad
		{
			get
			{
				return this.cantidad;
			}
			set
			{
				this.cantidad = value;
			}
		}

		public double ValorUnitario
		{
			get
			{
				return this.valorUnitario;
			}
			set
			{
				this.valorUnitario = value;
			}
		}
		
		public double CostoPromedio
		{
			get
			{
				return this.costoPromedio;
			}
			set
			{
				this.costoPromedio = value;
			}
		}
		
		public double CostoPromedioHistorico
		{
			get
			{
				return this.costoPromedioHistorico;
			}
			set
			{
				this.costoPromedioHistorico = value;
			}
		}

		public double PorcentajeDescuento
		{
			get
			{
				return this.porcentajeDescuento;
			}
			set
			{
				this.porcentajeDescuento = value;
			}
		}

		public double PorcentajeIva
		{
			get
			{
				return this.porcentajeIva;
			}
			set
			{
				this.porcentajeIva = value;
			}
		}
		
		public int TipoMovimiento
		{
			get
			{
				return this.tipoMovimiento;
			}
			set
			{
				this.tipoMovimiento = value;
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
		
		public override string ToString()
		{
			return " Codigo: "+this.Codigo+" -Origen: "+
				this.Origen+" -CodigoDocumento: "+
				this.CodigoDocumento+" -CentroCosto: "+
				this.centroCosto+" -Cantidad:"+this.cantidad+
			    " -ValorUnitario: "+this.valorUnitario+
				" -CostoPromedio: "+this.costoPromedio+
				" -CostoPromedioHistorico: "+this.costoPromedioHistorico+
				" -PorcentajeIva: "+this.porcentajeIva+
				" -PorcentajeDescuento: "+this.porcentajeDescuento+
				" -TipoMovmiento: "+this.tipoMovimiento;
		}
	}
}
