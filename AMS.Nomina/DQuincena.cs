using System;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de DQuincena.
	/// </summary>
	public class DQuincena
	{
		private int codigo;
		private string codigoEmpleado;
		private string concepto;
		private int cantidadEventos;
		private int valorEvento;
		private double aPagar;
		private double aDescontar;
		private int descripcionCantidad;
		private double saldo;
		private string documentoReferencia;
		private string vacaciones;

		
		public DQuincena()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

		public int Codigo
		{
			get
			{
				return this.codigo;
			}
			set
			{
				this.codigo=value;
			}
		}

		public string CodigoEmpleado
		{
			get
			{
				return this.codigoEmpleado;
			}
			set
			{
				this.codigoEmpleado=value;
			}
		}

		public string Concepto
		{
			get
			{
				return this.concepto;
			}
			set
			{
				this.concepto=value;
			}
		}

		public int CantidadEventos
		{
			get
			{
				return this.cantidadEventos;
			}
			set
			{
				this.cantidadEventos=value;
			}
		}

		public int ValorEvento
		{
			get
			{
				return this.valorEvento;
			}
			set
			{
				this.valorEvento=value;
			}
		}

		public double APagar
		{
			get
			{
				return this.aPagar;
			}
			set
			{
				this.aPagar=value;
			}
		}

		public double ADescontar
		{
			get
			{
				return this.aDescontar;
			}
			set
			{
				this.aDescontar=value;
			}
		}

		public int DescripcionCantidad
		{
			get
			{
				return this.descripcionCantidad;
			}
			set
			{
				this.descripcionCantidad=value;
			}
		}

		public double Saldo
		{
			get
			{
				return this.saldo;
			}
			set
			{
				this.saldo=value;
			}
		}

		public string DocumentoReferencia
		{
			get
			{
				return this.documentoReferencia;
			}
			set
			{
				this.documentoReferencia=value;
			}
		}

		public string Vacaciones
		{
			get
			{
				return this.vacaciones;
			}
			set
			{
				this.vacaciones=value;
			}
		}

	}
}
