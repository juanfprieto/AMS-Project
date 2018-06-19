/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/5/2005
 */

using System;
using System.Collections;

namespace AMS.Interface
{
	/// <summary>
	/// Interface por periodo para FacturaProveedor.
	/// </summary>
	public class InterfacePeriodoFacturaProveedor
	{
		private string fechaInicial;
		private string fechaFinal;
		public InterfacePeriodoFacturaProveedor(string fechaInicial,string fechaFinal)
		{
			this.fechaInicial = fechaInicial;
			this.fechaFinal = fechaFinal;
		}
		
		public string FechaInicial
		{
			get
			{
				return this.fechaInicial;
			}
			set
			{
				this.fechaInicial = value;
			}
		}
		
		public string FechaFinal
		{
			get
			{
				return this.fechaFinal;
			}
			set
			{
				this.fechaFinal = value;
			}
		}
		
		public void GenerarFacturasProveedorPeriodo()
		{
			MFacturaProveedor facturaProveedor;
			InterfaceFacturaProveedor interfaceFacturaProveedor;
			MFacturaProveedorDAO facturaProveedorDAO = new MFacturaProveedorDAO();
			ICollection ic = facturaProveedorDAO.GetMFacturaProveedor(FechaInicial,FechaFinal);
			IEnumerator ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				facturaProveedor = (MFacturaProveedor)ie.Current;
				interfaceFacturaProveedor = new InterfaceFacturaProveedor(facturaProveedor.CodigoDocumento,facturaProveedor.NumeroDocumento);
				interfaceFacturaProveedor.ProcesarInterface();
			}
		}
	}
}
