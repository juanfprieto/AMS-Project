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
	/// Proceso para ajustes inventario
	/// </summary>
	public class Ajustes
	{
		private MComprobante encabezadoComprobante;
		private ComprobanteXML comprobanteXML;
		private DitemDAO dItemDAO;
		private ConceptoContableDAO conceptoContableDAO;
		private CempresaDAO cEmpresaDAO;
		private string codigoDocumento;
		private int numeroDocumento;
			
		public Ajustes()
		{
			this.encabezadoComprobante = new MComprobante();
			this.comprobanteXML = new ComprobanteXML();
			this.dItemDAO = new DitemDAO();
			this.conceptoContableDAO = new ConceptoContableDAO();
			this.cEmpresaDAO = new CempresaDAO();
			this.codigoDocumento = "";
			this.numeroDocumento = 0;
		}
		
		public Ajustes(string codigoDocumento,int numeroDocumento)
		{
			this.encabezadoComprobante = new MComprobante();
			this.comprobanteXML = new ComprobanteXML();
			this.dItemDAO = new DitemDAO();
			this.conceptoContableDAO = new ConceptoContableDAO();
			this.cEmpresaDAO = new CempresaDAO();
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
		
		public void SetEncabezadoAjustes()
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
			encabezadoComprobante.Razon = "Interface Ajustes inventario";
			encabezadoComprobante.Valor = 0;
			this.SetDetalleAjustes();
		}
		
		public void SetDetalleAjustes()
		{
			double inventarios = 0;
			//Auxiliares
			double valorUnitario = 0;
			DComprobante dComprobante;
			Cempresa cEmpresa;
			ConceptoContable conceptoContable;
			string centroCosto = "";
			string almacen = "";
			string nit = "";
			ICollection ic;
			ArrayList ar;
			ic = dItemDAO.GetDitem(50);
			IEnumerator ie = ic.GetEnumerator();
			Ditem items;
			while(ie.MoveNext())
			{
				items = (Ditem)ie.Current;
				valorUnitario = items.Cantidad * items.CostoPromedio;
				inventarios += valorUnitario;
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
						if(inventarios > 0)
						{
							dComprobante.Credito = inventarios;
						}
						else
						{
							dComprobante.Debito = inventarios * (-1);
						}
						break;
					case "INGAJUINV":
						if(inventarios > 0)
						{
							dComprobante.Debito = inventarios;
						}
						break;
					case "GTOAJUINV":
						if(inventarios < 0)
						{
							dComprobante.Debito = inventarios * (-1);
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
			this.SetEncabezadoAjustes();
			comprobanteXML.Comprobante = encabezadoComprobante;
			comprobanteXML.RutaComprobante = "c:\\comprobantes\\";//temporal
			comprobanteXML.NombreComprobante = CodigoDocumento+NumeroDocumento.ToString();
			comprobanteXML.ComprobanteAXML();
		}
	}
}
