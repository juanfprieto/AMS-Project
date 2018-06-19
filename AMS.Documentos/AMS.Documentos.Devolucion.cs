using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;

namespace AMS.Documentos
{
	public class Devolucion
	{
		#region Constantes

		private const string VALORESFACTURACLIENTE = "Select MFAC_VALOFACT, MFAC_VALOIVA, MFAC_VALOFLET, MFAC_VALOIVAFLET, MFAC_VALORETE, MFAC_VALOABON, MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON MFAC_VALOSALD from DBXSCHEMA.MFACTURACLIENTE WHERE PDOC_CODIGO='{0}' AND MFAC_NUMEDOCU={1}";

		#endregion

		#region Atributos
		
		private string prefijoFactura, prefijoNotaDevolucion;
		private uint numeroFactura, numeroNotaDevolucion;
		private ArrayList sqlStrings;

		#endregion

		#region Propiedades

		public string PrefijoFactura{get{return prefijoFactura;}}		
		public uint NumeroFactura{get{return numeroFactura;}}		
		public string PrefijoNotaDevolucion{get{return prefijoNotaDevolucion;}}		
		public uint NumeroNotaDevolucion{get{return numeroNotaDevolucion;}}		
		public ArrayList SqlStrings{get{return sqlStrings;}}
		
		#endregion

		#region Constructores

		public Devolucion(string prefijoFactura, uint numeroFactura, string prefijoNotaDevolucion, uint numeroNotaDevolucion)
		{
			this.prefijoFactura = prefijoFactura;
			this.numeroFactura = numeroFactura;
			this.prefijoNotaDevolucion = prefijoNotaDevolucion;
			this.numeroNotaDevolucion = numeroNotaDevolucion;
			this.sqlStrings = new ArrayList();
		}

		#endregion

		#region Metodos

		private bool CargarValoresFacturaCliente(
			string prefijoFactura,
			uint numeroFactura,
			ref decimal valorFactura,
			ref decimal	valorIva,
			ref decimal	valorFlete,
			ref decimal	valorIvaFlete,
			ref decimal	valorRetencion,
			ref decimal	valorAbono,
			ref decimal	valorSaldo
			)
		{
			bool valido = false;
			DataSet dsValoresFacturaCliente = new DataSet();
			DBFunctions.Request(dsValoresFacturaCliente,IncludeSchema.NO,string.Format(VALORESFACTURACLIENTE,prefijoFactura,numeroFactura.ToString()));
			if ((dsValoresFacturaCliente.Tables.Count > 0) && (dsValoresFacturaCliente.Tables[0].Rows.Count > 0))
			{
				valorFactura    = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALOFACT"]);
				valorIva        = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALOIVA"]);
				valorFlete      = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALOFLET"]);
				valorIvaFlete   = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALOIVAFLET"]);
				valorRetencion  = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALORETE"]);
				valorAbono      = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALOABON"]);
				valorSaldo      = Convert.ToDecimal(dsValoresFacturaCliente.Tables[0].Rows[0]["MFAC_VALOSALD"]);
				valido = true;
			}
			return valido;
		}

		public void AplicarNotaCreditoAFactura()
		{
			decimal valorFacturaFactura = 0, valorIvaFactura = 0, valorFleteFactura = 0, valorIvaFleteFactura = 0, valorRetencionFactura = 0, valorAbonoFactura = 0, valorSaldoFactura = 0;
			decimal valorFacturaNotaDevolucion = 0, valorIvaNotaDevolucion = 0, valorFleteNotaDevolucion = 0, valorIvaFleteNotaDevolucion = 0, valorRetencionNotaDevolucion = 0, valorAbonoNotaDevolucion = 0, valorSaldoNotaDevolucion = 0;
			
			sqlStrings.Clear();
			
			CargarValoresFacturaCliente(PrefijoFactura,NumeroFactura,ref valorFacturaFactura,ref valorIvaFactura,ref valorFleteFactura,ref valorIvaFleteFactura,ref valorRetencionFactura,ref valorAbonoFactura,ref valorSaldoFactura);
			CargarValoresFacturaCliente(PrefijoFactura,NumeroFactura,ref valorFacturaNotaDevolucion,ref valorIvaNotaDevolucion,ref valorFleteNotaDevolucion,ref valorIvaFleteNotaDevolucion,ref valorRetencionNotaDevolucion,ref valorAbonoNotaDevolucion,ref valorSaldoNotaDevolucion);
			
			valorAbonoNotaDevolucion = 0;   // que pasa con el saldo del anticpo que se pasa para la nueva factura ?? 

			decimal valorSaldo = 0;
			if (valorSaldoFactura <= valorSaldoNotaDevolucion)
				valorSaldo = valorSaldoFactura;
			else
				valorSaldo = valorSaldoNotaDevolucion;
            decimal vlrTotalNota = valorFacturaFactura + valorIvaFactura + valorFleteFactura + valorIvaFleteFactura - valorRetencionFactura;
            //Ahora creamos la relacion entre la factura y la nota devolucion con el abono.
            sqlStrings.Add("INSERT INTO DBXSCHEMA.DDETALLEFACTURACLIENTE (PDOC_CODIGO,MFAC_NUMEDOCU,PDOC_CODDOCREF,DDET_NUMEDOCU,DDET_VALODOCU,DDET_OBSER) VALUES('"+PrefijoFactura+"',"+NumeroFactura+",'"+PrefijoNotaDevolucion+"',"+NumeroNotaDevolucion+","+ vlrTotalNota.ToString()+",'Abono a la Factura "+PrefijoFactura+"-"+NumeroFactura.ToString()+" por devolucion')");

			//Ahora creamos la relacion entre la nota devolucion y la factura con el abono.
			sqlStrings.Add("INSERT INTO DBXSCHEMA.DDETALLEFACTURACLIENTE (PDOC_CODIGO,MFAC_NUMEDOCU,PDOC_CODDOCREF,DDET_NUMEDOCU,DDET_VALODOCU,DDET_OBSER) VALUES('"+PrefijoNotaDevolucion+"',"+NumeroNotaDevolucion+",'"+PrefijoFactura+"',"+NumeroFactura+","+valorSaldo.ToString()+",'Abono a la Nota Devolucion "+PrefijoNotaDevolucion+"-"+NumeroNotaDevolucion.ToString()+"')");

			//Ahora cambiamos la vigencia de la factura a cancelada (C) y abonamos el saldo obtenido a la factura.
			sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A', mfac_valoabon=mfac_valoabon+"+valorSaldo.ToString()+" WHERE pdoc_codigo='"+PrefijoFactura+"' AND mfac_numedocu="+NumeroFactura+"");

			//Ahora cambiamos la vigencia de la factura a cancelada (A) y abonamos el saldo obtenido a la nota devolucion.
            sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='A', mfac_valoabon=mfac_valoabon+" + valorSaldo.ToString() + ", MFAC_PAGO = MFAC_FACTURA WHERE pdoc_codigo='" + PrefijoNotaDevolucion + "' AND mfac_numedocu=" + NumeroNotaDevolucion + "");
	
			//Ahora cambiamos la vigencia de la factura a cancelada (C) si el saldo obtenido de la nota devolucion es CERO.
            sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE = MFAC_VALOABON AND ( pdoc_codigo='" + PrefijoFactura + "' AND mfac_numedocu=" + NumeroFactura + " OR pdoc_codigo='" + PrefijoNotaDevolucion + "' AND mfac_numedocu=" + NumeroNotaDevolucion + ") ");
		}

		#endregion
	}
}
