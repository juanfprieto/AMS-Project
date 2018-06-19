/*
 * Autor: Administrador
 * Fecha: 04/12/2005
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// Cuentas de acuerdo a concepto contable.
	/// </summary>
	public class ConceptoContable
	{
		private string codigoConcepto;
		private string codigoDocumento;
		private string codigoPuc;
		private string descripcion;
		
		public ConceptoContable()
		{
			this.codigoConcepto = "";
			this.codigoDocumento = "";
			this.codigoPuc = "";
			this.descripcion = "";
		}
		
		public ConceptoContable(string codigoConcepto, string codigoDocumento,
		                        string codigoPuc, string descripcion)
		{
			this.codigoConcepto = codigoConcepto;
			this.codigoDocumento = codigoDocumento;
			this.codigoPuc = codigoPuc;
			this.descripcion = descripcion;
		}
		
		public string CodigoConcepto
		{
			get
			{
				return this.codigoConcepto;
			}
			set
			{
				this.codigoConcepto = value;	
			}
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
		
		public string CodigoPuc
		{
			get
			{
				return this.codigoPuc;
			}
			set
			{
				this.codigoPuc = value;
			}
		}
		
		public string Descripcion
		{
			get
			{
				return this.descripcion;
			}
			set
			{
				this.descripcion = value;
			}
		}
		
		public override string ToString()
		{
			return " -CodigoConcepto: "+this.codigoConcepto+" -CodigoDocumento: "+
				this.codigoDocumento+" -CodigoPuc: "+this.codigoPuc+
				" -Descripcion: "+this.descripcion;
		}
		
	}
}
