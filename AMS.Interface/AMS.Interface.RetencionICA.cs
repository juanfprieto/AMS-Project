/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/6/2005
 */

using System;

namespace AMS.Interface
{
	/// <summary>
	/// RetencionICA.
	/// </summary>
	public class RetencionICA
	{
		private string codigoICA;
		private string codigoCuentaSimplificado;
		private string codigoCuentaComun;
		private string nit;
		
		public RetencionICA()
		{
		}
		
		public string CodigoICA
		{
			get
			{
				return this.codigoICA;
			}
			set
			{
				this.codigoICA = value;
			}
		}
		
		public string CodigoCuentaSimplificado
		{
			get
			{
				return this.codigoCuentaSimplificado;
			}
			set
			{
				this.codigoCuentaSimplificado = value;
			}
		}
		
		public string CodigoCuentaComun
		{
			get
			{
				return this.codigoCuentaComun;
			}
			set
			{
				this.codigoCuentaComun = value;
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
	}
}
