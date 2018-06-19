/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/5/2005
 */

using System;
using System.Data;
using System.Collections;

namespace AMS.Interface
{
	/// <summary>
	/// InterfaceFacturaProveedor.
	/// </summary>
	public class InterfaceFacturaProveedor
	{
		private MComprobante encabezadoComprobante;
		private ComprobanteXML comprobanteXML;
		private MFacturaProveedor facturaProveedor;
		private MFacturaProveedorDAO facturaProveedorDAO;
		private DitemDAO dItemDAO;
		private ConceptoContableDAO conceptoContableDAO;
		private CempresaDAO cEmpresaDAO;
		private RetencionProveedorDAO retencionProveedorDAO;
		private string codigoDocumento;
		private int numeroDocumento;
		
		public InterfaceFacturaProveedor(string codigoDocumento,int numeroDocumento)
		{
			this.encabezadoComprobante = new MComprobante();
			this.comprobanteXML = new ComprobanteXML();
			this.facturaProveedorDAO = new MFacturaProveedorDAO();
			this.facturaProveedor = new MFacturaProveedor();
			this.dItemDAO = new DitemDAO();
			this.conceptoContableDAO = new ConceptoContableDAO();
			this.cEmpresaDAO = new CempresaDAO();
			this.retencionProveedorDAO = new RetencionProveedorDAO();
			this.codigoDocumento = codigoDocumento;
			this.numeroDocumento = numeroDocumento;
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
		
		public void SetFacturaProveedor()
		{
			int anio,mes;
			facturaProveedor = facturaProveedorDAO.GetMFacturaProveedorPK(CodigoDocumento,NumeroDocumento);
			anio = 2005;//temporal
			mes = 11;//temporal
			encabezadoComprobante.Anio = anio;
			encabezadoComprobante.Mes = mes;
			encabezadoComprobante.Fecha = facturaProveedor.FechaFactura;
			encabezadoComprobante.Numero = facturaProveedor.NumeroDocumento;
			encabezadoComprobante.Prefijo = facturaProveedor.CodigoDocumento;
			encabezadoComprobante.NumeroReferencia = facturaProveedor.NumeroDocumentoReferencia;
			encabezadoComprobante.PrefijoReferencia = facturaProveedor.CodigoDocumentoReferencia;
			encabezadoComprobante.Razon = "Interface";
			this.SetDetalleFacturaProveedor();
		}
		
		public void SetDetalleFacturaProveedor()
		{
			Ditem items;
			RetencionProveedor retencionProveedor;
			DComprobante dComprobante;
			ConceptoContable conceptoContable;
			RetencionICADAO retencionICADAO = new RetencionICADAO();
			RetencionICA retencionICA;
			double valorInventarios = 0;
			double valorTerceros = 0;
			double valor = 0;
			double descuento = 0;
			double ivaInventarios = 0;
			double ivaTerceros = 0;
            double retencion = 0;
            string centroCosto = "";
            double valorCompra = 0;
            bool devolucion = false;
            ArrayList ar;
			dItemDAO = new DitemDAO();
			ICollection ic = dItemDAO.GetDitem(facturaProveedor.CodigoDocumento,facturaProveedor.NumeroDocumento);
			IEnumerator ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				items = (Ditem)ie.Current;
				if(items.TipoMovimiento == 31)
				{
					devolucion = true;
				}
				valor = items.Cantidad*items.ValorUnitario;
				valorCompra += valor;
				descuento += items.PorcentajeDescuento*valor;
				if(items.Origen.Equals("X"))
				{
					valorTerceros += valor - (items.PorcentajeDescuento*valor);
					ivaTerceros += items.PorcentajeIva*valor;
				}
				else
				{
					valorInventarios += valor - (items.PorcentajeDescuento*valor);
					ivaInventarios += items.PorcentajeIva*valor;
				}
				centroCosto = items.CentroCosto;
			}
			ic = retencionProveedorDAO.GetRetencionProveedor(facturaProveedor.CodigoDocumento,facturaProveedor.NumeroDocumento);
			ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				dComprobante = new DComprobante();
				retencionProveedor = (RetencionProveedor)ie.Current;
    			retencion += retencionProveedor.ValorRetencion;
    			dComprobante.Almacen = facturaProveedor.Almacen;
    			dComprobante.CentroCosto = centroCosto;
    			conceptoContable = conceptoContableDAO.GetConceptoContable(facturaProveedor.CodigoDocumento,retencionProveedor.CodigoRetencion);
    			if(retencionProveedor.CodigoRetencion.Equals("RI"))
    			{
    				retencionICA = retencionICADAO.GetRetencionICA(facturaProveedor.Nit);
    				if(facturaProveedor.RegimenIva.Equals("S"))
    				{
    					dComprobante.Cuenta = retencionICA.CodigoCuentaSimplificado;
    				}
    				else if(facturaProveedor.RegimenIva.Equals("C"))
    				{
    					dComprobante.Cuenta = retencionICA.CodigoCuentaComun;
    				}
    			}
    			else
    			{
   					dComprobante.Cuenta = conceptoContable.CodigoPuc;
    			}
    			if(devolucion)
    			{
    				dComprobante.Credito = retencionProveedor.ValorRetencion;
    			}
    			else
    			{
    				dComprobante.Debito = retencionProveedor.ValorRetencion;
    			}
    			dComprobante.Nit = facturaProveedor.Nit;
    			ar = (ArrayList)encabezadoComprobante.Detalle;
    			ar.Add(dComprobante);
			}
			ic = conceptoContableDAO.GetConceptoContable(facturaProveedor.CodigoDocumento);
			ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				dComprobante = new DComprobante();
				dComprobante.Almacen = facturaProveedor.Almacen;
				dComprobante.CentroCosto = centroCosto;
				dComprobante.Nit = facturaProveedor.Nit;
				conceptoContable = (ConceptoContable)ie.Current;
				dComprobante.Cuenta = conceptoContable.CodigoPuc;
				switch(conceptoContable.CodigoConcepto)
				{
					case "CXP":
						valor = valorCompra - descuento + ivaInventarios + ivaTerceros - retencion;
						if(devolucion)
						{
							dComprobante.Debito = valor;
						}
						else
						{
							dComprobante.Credito = valor;
						}
						break;
					case "IVA":
						valor = ivaInventarios + ivaTerceros;
						if(devolucion)
						{
							dComprobante.Credito = valor;
						}
						else
						{
							dComprobante.Debito = valor;
						}
						break;
					case "INVITE":
						if(devolucion)
						{
							dComprobante.Credito = valorInventarios;
						}
						else
						{
							dComprobante.Debito = valorInventarios;
						}
						break;
					case "INVTRBTER":
						if(devolucion)
						{
							dComprobante.Credito = valorTerceros;
						}
						else
						{
							dComprobante.Debito = valorTerceros;
						}
						break;
				}
				encabezadoComprobante.Valor = valorCompra;
				ar = (ArrayList)encabezadoComprobante.Detalle;
				ar.Add(dComprobante);
			}
		}
		
		public void ProcesarInterface()
		{
			this.SetFacturaProveedor();
			comprobanteXML.Comprobante = encabezadoComprobante;
			comprobanteXML.RutaComprobante = "c:\\comprobantes\\";//temporal
			comprobanteXML.NombreComprobante = facturaProveedor.CodigoDocumento+facturaProveedor.NumeroDocumento.ToString();
			comprobanteXML.ComprobanteAXML();
		}
	}
}
