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
	/// InterfaceFacturaCliente.
	/// </summary>
	public class InterfaceFacturaCliente
	{
		private MComprobante encabezadoComprobante;
		private ComprobanteXML comprobanteXML;
		private MFacturaCliente facturaCliente;
		private MFacturaClienteDAO facturaClienteDAO;
		private DitemDAO dItemDAO;
		private ConceptoContableDAO conceptoContableDAO;
		private CempresaDAO cEmpresaDAO;
		private RetencionClienteDAO retencionClienteDAO;
		private string codigoDocumento;
		private int numeroDocumento;
		private bool contabilizaDescuentos;
		
		public InterfaceFacturaCliente(string codigoDocumento,int numeroDocumento,bool contabilizaDescuentos)
		{
			this.encabezadoComprobante = new MComprobante();
			this.comprobanteXML = new ComprobanteXML();
			this.facturaClienteDAO = new MFacturaClienteDAO();
			this.facturaCliente = new MFacturaCliente();
			this.dItemDAO = new DitemDAO();
			this.conceptoContableDAO = new ConceptoContableDAO();
			this.cEmpresaDAO = new CempresaDAO();
			this.retencionClienteDAO = new RetencionClienteDAO();
			this.codigoDocumento = codigoDocumento;
			this.numeroDocumento = numeroDocumento;
			this.contabilizaDescuentos = contabilizaDescuentos;
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
		
		public void SetFacturaCliente()
		{
			int anio,mes;
			facturaCliente = facturaClienteDAO.GetMFacturaClientePK(CodigoDocumento,NumeroDocumento);
			anio = 2005;//temporal
			mes = 11;//temporal
			encabezadoComprobante.Anio = anio;
			encabezadoComprobante.Mes = mes;
			encabezadoComprobante.Fecha = facturaCliente.FechaFactura;
			encabezadoComprobante.Numero = facturaCliente.NumeroDocumento;
			encabezadoComprobante.Prefijo = facturaCliente.CodigoDocumento;
			encabezadoComprobante.NumeroReferencia = facturaCliente.NumeroDocumento;
			encabezadoComprobante.PrefijoReferencia = facturaCliente.CodigoDocumento;
			encabezadoComprobante.Razon = "Interface";
			this.SetDetalleFacturaCliente();
		}
		
		public void SetDetalleFacturaCliente()
		{
			Ditem items;
			RetencionCliente retencionCliente;
			DComprobante dComprobante;
			ConceptoContable conceptoContable;
			double cuentasPorCobrar = 0;
			double inventarioItems = 0;
			double inventarioTerceros = 0;
			double ivaItems = 0;
			double ivaTerceros = 0;
			double ajusteInflacion = 0;
			double costoItems = 0;
			double costoTerceros = 0;
			double retencion = 0;
			double descuento = 0;
			double ventaItemsGrabado = 0;
			double ventaTercerosGrabado = 0;
			double ventaItemsExcento = 0;
			double ventaTercerosExcento = 0;
			//auxiliares
			string centroCosto = "";
            double totalVenta = 0;
            double valorUnitarios = 0;
            double valorPromedios = 0;
            double valorHistoricos = 0;
            bool devolucion = false;
            ArrayList ar;
			dItemDAO = new DitemDAO();
			ICollection ic = dItemDAO.GetDitem(facturaCliente.CodigoDocumento,facturaCliente.NumeroDocumento);
			IEnumerator ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				items = (Ditem)ie.Current;
				if(items.TipoMovimiento == 91)
				{
					devolucion = true;
				}
				valorUnitarios = items.Cantidad*items.ValorUnitario;
				valorPromedios = items.Cantidad*items.CostoPromedio;
				valorHistoricos = items.Cantidad*items.CostoPromedioHistorico;
				//Valor comprobante
				totalVenta += valorUnitarios;
				//ajuste Inflacion
				ajusteInflacion += valorPromedios - valorHistoricos;
				//Descuento 
				descuento += items.PorcentajeDescuento*valorUnitarios;
				//Terceros
				if(items.Origen.Equals("X"))
				{
					//Inventario Terceros
					inventarioTerceros += valorPromedios;
					//Costo Terceros
					costoTerceros += valorPromedios;
					//IVA Terceros
					ivaTerceros += valorUnitarios * items.PorcentajeIva;
					
					if(items.PorcentajeIva == 0)
					{
						//Venta Terceros Excento
						if(contabilizaDescuentos)
						{
							ventaTercerosExcento += valorUnitarios;
						}
						else
						{
							ventaTercerosExcento += valorUnitarios - (items.PorcentajeDescuento * valorUnitarios);
						}
						
					}
					else
					{
						//Venta Terceros Grabado
						if(contabilizaDescuentos)
						{
							ventaTercerosGrabado += valorUnitarios;
						}
						else
						{
							ventaTercerosGrabado += valorUnitarios - (items.PorcentajeDescuento*valorUnitarios);
						}
						
					}
				}
				//Items
				else
				{
					//Inventario Items
					inventarioItems += valorPromedios;
					//Costo Items
					costoItems += valorPromedios;
					//IVA Items
					ivaItems += valorUnitarios * items.PorcentajeIva;
					
					if(items.PorcentajeIva == 0)
					{
						//Venta Items Excento
						if(contabilizaDescuentos)
						{
							ventaItemsExcento += valorUnitarios;
						}
						else
						{
							ventaItemsExcento += valorUnitarios - (items.PorcentajeDescuento * valorUnitarios);
						}
						
					}
					else
					{
						//Venta Items Grabado
						if(contabilizaDescuentos)
						{
							ventaItemsGrabado += valorUnitarios;
						}
						else
						{
							ventaItemsGrabado += valorUnitarios - (items.PorcentajeDescuento*valorUnitarios);
						}
						
					}
				}
				centroCosto = items.CentroCosto;
			}
			//Retenciones
			ic = retencionClienteDAO.GetRetencionCliente(facturaCliente.CodigoDocumento,facturaCliente.NumeroDocumento);
			ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				dComprobante = new DComprobante();
				retencionCliente = (RetencionCliente)ie.Current;
    			retencion += retencionCliente.ValorRetencion;
    			dComprobante.Almacen = facturaCliente.Almacen;
    			dComprobante.CentroCosto = centroCosto;
    			conceptoContable = conceptoContableDAO.GetConceptoContable(facturaCliente.CodigoDocumento,retencionCliente.CodigoRetencion);
    			dComprobante.Cuenta = conceptoContable.CodigoPuc;
    			if(devolucion)
    			{
    				dComprobante.Credito = retencionCliente.ValorRetencion;
    			}
    			else
    			{
    				dComprobante.Debito = retencionCliente.ValorRetencion;
    			}
    			dComprobante.Nit = facturaCliente.Nit;
    			ar = (ArrayList)encabezadoComprobante.Detalle;
    			ar.Add(dComprobante);
			}
			ic = conceptoContableDAO.GetConceptoContable(facturaCliente.CodigoDocumento);
			ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				dComprobante = new DComprobante();
				dComprobante.Almacen = facturaCliente.Almacen;
				dComprobante.CentroCosto = centroCosto;
				dComprobante.Nit = facturaCliente.Nit;
				conceptoContable = (ConceptoContable)ie.Current;
				dComprobante.Cuenta = conceptoContable.CodigoPuc;
				switch(conceptoContable.CodigoConcepto)
				{
					case "CXC":
						cuentasPorCobrar = totalVenta - descuento + ivaTerceros + ivaItems - retencion;
						if(devolucion)
						{
							dComprobante.Credito = cuentasPorCobrar;
						}
						else
						{
							dComprobante.Debito = cuentasPorCobrar;
						}
						
						break;
					case "IVA":
						if(devolucion)
						{
							dComprobante.Debito = ivaTerceros + ivaItems;
						}
						else
						{
							dComprobante.Credito = ivaTerceros + ivaItems;
						}
						break;
					case "INVITE":
						if(devolucion)
						{
							dComprobante.Debito = inventarioItems;
						}
						else
						{
							dComprobante.Credito = inventarioItems;
						}
						
						break;
					case "INVTRBTER":
						if(devolucion)
						{
							dComprobante.Debito = inventarioTerceros;
						}
						else
						{
							dComprobante.Credito = inventarioTerceros;
						}
						break;
					case "INVITEAMOINF":
						if(devolucion)
						{
							dComprobante.Debito = ajusteInflacion;
						}
						else
						{
							dComprobante.Credito = ajusteInflacion;
						}
						break;
					case "COSVTAITE":
						if(devolucion)
						{
							dComprobante.Credito = costoItems;
						}
						else
						{
							dComprobante.Debito = costoItems;
						}
						break;
					case "COSVTATRBTER":
						if(devolucion)
						{
							dComprobante.Credito = costoTerceros;
						}
						else
						{
							dComprobante.Debito = costoTerceros;
						}
						break;
					case "VTAITEEXE":
						if(devolucion)
						{
							dComprobante.Debito = ventaItemsExcento;
						}
						else
						{
							dComprobante.Credito = ventaItemsExcento;
						}
						break;
					case "VTAITEGRA":
						if(devolucion)
						{
							dComprobante.Debito = ventaItemsGrabado;
						}
						else
						{
							dComprobante.Credito = ventaItemsGrabado;
						}
						break;
					case "VTATRBTEREXE":
						if(devolucion)
						{
							dComprobante.Debito = ventaTercerosExcento;
						}
						else
						{
							dComprobante.Credito = ventaTercerosExcento;
						}
						break;
					case "VTATRBTERGRA":
						if(devolucion)
						{
							dComprobante.Debito = ventaTercerosGrabado;
						}
						else
						{
							dComprobante.Credito = ventaTercerosGrabado;
						}
						break;
					case "DESVTA":
						if(contabilizaDescuentos)
						{
							if(devolucion)
							{
								dComprobante.Debito = descuento;
							}
							else
							{
								dComprobante.Credito = descuento;
							}
						}
						break;
				}
				encabezadoComprobante.Valor = totalVenta;
				ar = (ArrayList)encabezadoComprobante.Detalle;
				ar.Add(dComprobante);
			}
		}
		
		public void ProcesarInterface()
		{
			this.SetFacturaCliente();
			comprobanteXML.Comprobante = encabezadoComprobante;
			comprobanteXML.RutaComprobante = "c:\\comprobantes\\";//temporal
			comprobanteXML.NombreComprobante = facturaCliente.CodigoDocumento+facturaCliente.NumeroDocumento.ToString();
			comprobanteXML.ComprobanteAXML();
		}
	}
}
