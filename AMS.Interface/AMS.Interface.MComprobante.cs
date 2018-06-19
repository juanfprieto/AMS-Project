/*
 * Autor: Administrador
 * Fecha: 04/12/2005
 */

using System;
using System.Collections;

namespace AMS.Interface
{
	/// <summary>
	/// Cabezote de comprobante.
	/// </summary>
	public class MComprobante
	{
		private string prefijo;
		private int numero;
		private string prefijoReferencia;
		private int numeroReferencia;
		private int anio;
		private int mes;
		private string fecha;
		private string razon;
		private double valor;
		private ICollection detalle;
		
		public MComprobante()
		{
			this.prefijo = "";
			this.numero = 0;
			this.anio = 0;
			this.mes = 0;
			this.fecha = "";
			this.razon = "";
			this.valor = 0;
		}
		
		public MComprobante(string prefijo,int numero,int anio,int mes,
		                    string fecha,string razon,double valor,
		                    ICollection detalle)
		{
			this.prefijo = prefijo;
			this.numero = numero;
			this.anio = anio;
			this.mes = mes;
			this.fecha = fecha;
			this.razon = razon;
			this.valor = valor;
			this.detalle = detalle;
		}
		
		public string Prefijo
		{
			get
			{
				return this.prefijo;
			}
			set
			{
				this.prefijo = value;
			}
		}
		
		public int Numero
		{
			get
			{
				return this.numero;
			}
			set
			{
				this.numero = value;
			}
		}
		
		public string PrefijoReferencia
		{
			get
			{
				return this.prefijoReferencia;
			}
			set
			{
				this.prefijoReferencia = value;
			}
		}
		
		public int NumeroReferencia
		{
			get
			{
				return this.numeroReferencia;
			}
			set
			{
				this.numeroReferencia = value;
			}
		}
		
		public int Anio
		{
			get
			{
				return this.anio;
			}
			set
			{
				this.anio = value;
			}
		}
		
		public int Mes
		{
			get
			{
				return this.mes;
			}
			set
			{
				this.mes = value;
			}
		}
		
		public string Fecha
		{
			get
			{
				return this.fecha;
			}
			set
			{
				this.fecha = value;
			}
		}
		
				
		public string Razon
		{
			get
			{
				return this.razon;
			}
			set
			{
				this.razon = value;
			}
		}
		
		public double Valor
		{
			get
			{
				return this.valor;
			}
			set
			{
				this.valor = value;
			}
		}
		
		public ICollection Detalle
		{
			get
			{
				return this.detalle;
			}
			set
			{
				this.detalle = value;
			}
		}
	}
}
