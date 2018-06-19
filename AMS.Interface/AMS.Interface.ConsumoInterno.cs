/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/13/2005
 */

using System;
using System.Collections;
using System.Data;

//Interno
using AMS.DB;

namespace AMS.Interface
{
	/// <summary>
	/// Proceso para consumos internos de inventario
	/// </summary>
	public class ConsumoInterno
	{
		private MComprobante encabezadoComprobante;
		private ComprobanteXML comprobanteXML;
		private DitemDAO dItemDAO;
		private ConceptoContableDAO conceptoContableDAO;
		private CempresaDAO cEmpresaDAO;
		private string codigoDocumento;
		private int numeroDocumento;
		private bool devolucion;
		
		public ConsumoInterno()
		{
			this.encabezadoComprobante = new MComprobante();
			this.comprobanteXML = new ComprobanteXML();
			this.dItemDAO = new DitemDAO();
			this.conceptoContableDAO = new ConceptoContableDAO();
			this.cEmpresaDAO = new CempresaDAO();
			this.codigoDocumento = "";
			this.numeroDocumento = 0;
			this.devolucion = false;
		}
		
		public ConsumoInterno(string codigoDocumento,int numeroDocumento,bool devolucion)
		{
			this.encabezadoComprobante = new MComprobante();
			this.comprobanteXML = new ComprobanteXML();
			this.dItemDAO = new DitemDAO();
			this.conceptoContableDAO = new ConceptoContableDAO();
			this.cEmpresaDAO = new CempresaDAO();
			this.codigoDocumento = codigoDocumento;
			this.numeroDocumento = numeroDocumento;
			this.devolucion = devolucion;
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
		
		public void SetEncabezadoConsumoInterno()
		{
			int anio,mes;
			anio = 2005;
			mes = 11;
			encabezadoComprobante.Anio = anio;//Temporal
			encabezadoComprobante.Mes = mes;//Temporal
			encabezadoComprobante.Fecha = "2005-11-01";//Temporal
			encabezadoComprobante.Numero = NumeroDocumento;
			encabezadoComprobante.NumeroReferencia = NumeroDocumento;
			encabezadoComprobante.Prefijo = CodigoDocumento;
			encabezadoComprobante.PrefijoReferencia = CodigoDocumento;
			encabezadoComprobante.Razon = "Interface Consumo Interno";
			encabezadoComprobante.Valor = 0;
			this.SetDetalleConsumoInterno();
		}
		
		public void SetDetalleConsumoInterno()
		{
			double inventarios = 0;
			double ajustesInflacion = 0;
			double iva = 0;
			double gastoInterno = 0;
			//Auxiliares
			double valorUnitario = 0;
			double valorHistorico = 0;
			DComprobante dComprobante;
			Cempresa cEmpresa;
			ConceptoContable conceptoContable;
			string centroCosto = "";
			string almacen = "";
			string nit = "";
			ICollection ic;
			ArrayList ar;
			if(devolucion)
			{
				ic = dItemDAO.GetDitem(61);
			}
			else
			{
				ic = dItemDAO.GetDitem(60);
			}
			IEnumerator ie = ic.GetEnumerator();
			Ditem items;
			while(ie.MoveNext())
			{
				items = (Ditem)ie.Current;
				valorUnitario = items.Cantidad * items.CostoPromedio;
				inventarios += valorUnitario;
				valorHistorico = items.Cantidad * items.CostoPromedioHistorico;
				ajustesInflacion += valorUnitario - valorHistorico;
				iva += valorUnitario * items.PorcentajeIva;
				gastoInterno += valorUnitario;
				centroCosto = items.CentroCosto;
				almacen = items.Almacen;
			}
			cEmpresa = cEmpresaDAO.GetCempresa();
			cEmpresa = (Cempresa)ie.Current;
			nit = cEmpresa.Nit;
			ic = conceptoContableDAO.GetConceptoContable(CodigoDocumento);
			ie = ic.GetEnumerator();
			while(ie.MoveNext())
			{
				dComprobante = new DComprobante();
				dComprobante.Almacen = almacen;
				dComprobante.CentroCosto = centroCosto;
				dComprobante.Nit = nit;
				conceptoContable = (ConceptoContable)ie.Current;
				dComprobante.Cuenta = conceptoContable.CodigoPuc;
				switch(conceptoContable.CodigoConcepto)
				{
					case "INVITE":
						if(devolucion)
						{
							dComprobante.Debito = inventarios;
						}
						else
						{
							dComprobante.Credito = inventarios;
						}
						break;
					case "IVA":
						if(devolucion)
						{
							dComprobante.Debito = iva;
						}
						else
						{
							dComprobante.Credito = iva;
						}
						break;
					case "INVITEAMOINF":
						if(devolucion)
						{
							dComprobante.Debito = ajustesInflacion;
						}
						else
						{
							dComprobante.Credito = ajustesInflacion;
						}
						break;
					case "GTOINVINT":
						if(devolucion)
						{
							dComprobante.Credito = gastoInterno;
						}
						else
						{
							dComprobante.Debito = gastoInterno;
						}
						break;
				}
				encabezadoComprobante.Valor = 0;
				ar = (ArrayList)encabezadoComprobante.Detalle;
				ar.Add(dComprobante);
			}
		}
		
		public void ProcesarInterface()
		{
			this.SetEncabezadoConsumoInterno();
			comprobanteXML.Comprobante = encabezadoComprobante;
			comprobanteXML.RutaComprobante = "c:\\comprobantes\\";//temporal
			comprobanteXML.NombreComprobante = CodigoDocumento+NumeroDocumento.ToString();
			comprobanteXML.ComprobanteAXML();
		}
	}
}
