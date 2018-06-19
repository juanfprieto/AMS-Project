/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */

using System;
using System.Collections;
using System.Xml;

namespace AMS.Interface
{
	/// <summary>
	/// Comprobante XML.
	/// </summary>
	public class ComprobanteXML
	{
		private MComprobante comprobante;
		private string nombreComprobante;
		private string rutaComprobante;
		
		public ComprobanteXML()
		{
			this.comprobante = new MComprobante();
		}
		
		public MComprobante Comprobante
		{
			get
			{
				return this.comprobante;
			}
			set
			{
				this.comprobante = value;
			}
		}
		
		public string NombreComprobante
		{
			get
			{
				return this.nombreComprobante;
			}
			set
			{
				this.nombreComprobante = value;
			}
		}
		
		public string RutaComprobante
		{
			get
			{
				return this.rutaComprobante;
			}
			set
			{
				this.rutaComprobante = value;
			}
		}
		
		public void ComprobanteAXML()
		{
			XmlTextWriter writer = new XmlTextWriter(RutaComprobante+NombreComprobante+".xml",System.Text.Encoding.UTF8);
			writer.WriteStartDocument();
			//Comprobantes
			writer.WriteStartElement("comprobantes");
			//ComprobanteEspecifico
			writer.WriteStartElement("comprobanteEspecifico");
			//Cabezote
			writer.WriteStartElement("cabezote");
			//prefijo
			writer.WriteElementString("prefijo", comprobante.Prefijo);
			//numero
			writer.WriteElementString("numero", XmlConvert.ToString(comprobante.Numero));
			//anio
			writer.WriteElementString("ano", XmlConvert.ToString(comprobante.Anio));			
			//mes
			writer.WriteElementString("mes", XmlConvert.ToString(comprobante.Mes));
			//fecha
			writer.WriteElementString("fecha", comprobante.Fecha);
			//razon
			writer.WriteElementString("razon", comprobante.Razon);
			//valor
			writer.WriteElementString("valor", XmlConvert.ToString(comprobante.Valor));
			//Cierra Cabezote
			writer.WriteEndElement();
			IEnumerator ie = comprobante.Detalle.GetEnumerator();
			while(ie.MoveNext())
			{
				DComprobante detalle = (DComprobante)ie.Current;
				//Operacion
				writer.WriteStartElement("operacion");
				//cuenta
				writer.WriteElementString("cuenta", detalle.Cuenta);
				//nit
				writer.WriteElementString("nit", detalle.Nit);
				//prefijo referencia
				writer.WriteElementString("prefijoReferencia", comprobante.PrefijoReferencia);
				//numero referencia
				writer.WriteElementString("numeroReferencia", XmlConvert.ToString(comprobante.NumeroReferencia));
				//detalle
				writer.WriteElementString("detalle", comprobante.Razon);
				//sede - almacen
				writer.WriteElementString("sede", detalle.Almacen);
				//centro de costo
				writer.WriteElementString("ccosto", detalle.CentroCosto);
				//debito
				writer.WriteElementString("debito", XmlConvert.ToString(detalle.Debito));
				//credito
				writer.WriteElementString("credito", XmlConvert.ToString(detalle.Credito));
				//baseIva
				writer.WriteElementString("base", XmlConvert.ToString(detalle.BaseIva));
				//Cierra operacion
				writer.WriteEndElement();
			}
			//Cierra ComprobanteEspecifico
			writer.WriteEndElement();
			//Cierra Comprobante
			writer.WriteEndElement();
			//Cierra Documento
			writer.WriteEndDocument();
		}
	}
}
