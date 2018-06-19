/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/5/2005
 */

using System;
using System.Data;
using System.Collections;

//Interno
using AMS.DB;

namespace AMS.Interface
{
	/// <summary>
	/// Acceso a datos para MFacturaCliente.
	/// </summary>
	public class MFacturaClienteDAO
	{
		public MFacturaClienteDAO()
		{
		}
		
		/// <summary>
		/// Retorna las facturas de cliente que se ecuentran dentro de un
		/// rango de fechas pasados por parametro
		/// </summary>
		/// <param name="fechaInicio">Fecha de inicio del rango</param>
		/// <param name="fechaFin">Fecha final del rango</param>
		/// <returns></returns>
		public ICollection GetMFacturaCliente(string fechaInicio,string fechaFin)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			MFacturaCliente mFacturaCliente;
			sql = " SELECT "+
				" a.PDOC_CODIGO AS codigoDocumento, a.MFAC_NUMEDOCU AS "+
				" numeroDocumento, a.MNIT_NIT AS nit,"+
				" a.MFAC_FACTURA AS fechaFactura, a.MFAC_VALOIVA AS valorIva,"+
				" a.MFAC_VALOFACT AS valorFactura, a.MFAC_VALORETE AS valorRetencion,"+
				" a.PALM_ALMACEN AS almacen, b.TREG_REGIIVA AS regimenIVA FROM MFACTURACLIENTE a,"+
				" MNIT b WHERE a.MNIT_NIT = b.MNIT_NIT AND MFAC_FACTURA BETWEEN "+
				" '"+fechaInicio+"' AND '"+fechaFin+"'";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				mFacturaCliente = new MFacturaCliente();
				mFacturaCliente.CodigoDocumento = dr["codigoDocumento"].ToString();
				mFacturaCliente.NumeroDocumento = Convert.ToInt32(dr["numeroDocumento"].ToString());
				mFacturaCliente.Nit = dr["nit"].ToString();
				mFacturaCliente.FechaFactura = dr["fechaFactura"].ToString();
				mFacturaCliente.ValorIva = Convert.ToDouble(dr["valorIva"].ToString());
				mFacturaCliente.ValorFactura = Convert.ToDouble(dr["valorFactura"].ToString());
				mFacturaCliente.ValorRetencion = Convert.ToDouble(dr["valorRetencion"].ToString());
				mFacturaCliente.Almacen = dr["almacen"].ToString();
				mFacturaCliente.RegimenIva = dr["regimenIva"].ToString();
				al.Add(mFacturaCliente);
			}
			return al;
		}
		
		/// <summary>
		/// Retorna un Value de una factura de cliente de acuerdo a codigo y numero
		/// de documento pasados por parametro
		/// </summary>
		/// <param name="codigoDocumento">Codigo interno de documento</param>
		/// <param name="numeroDocumento">Numero interno de documento</param>
		/// <returns></returns>
		public MFacturaCliente GetMFacturaClientePK(string codigoDocumento,int numeroDocumento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			MFacturaCliente mFacturaCliente = new MFacturaCliente();
			sql = " SELECT "+
				" a.MNIT_NIT AS nit,"+
				" a.MFAC_FACTURA AS fechaFactura, a.MFAC_VALOIVA AS valorIva,"+
				" a.MFAC_VALOFACT AS valorFactura, a.MFAC_VALORETE AS valorRetencion,"+
				" a.PALM_ALMACEN AS almacen, b.TREG_REGIIVA AS regimenIVA FROM MFACTURACLIENTE a,"+
				" MNIT b WHERE a.MNIT_NIT = b.MNIT_NIT ";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				mFacturaCliente.CodigoDocumento = codigoDocumento;
				mFacturaCliente.NumeroDocumento = numeroDocumento;
				mFacturaCliente.Nit = dr["nit"].ToString();
				mFacturaCliente.FechaFactura = dr["fechaFactura"].ToString();
				mFacturaCliente.ValorIva = Convert.ToDouble(dr["valorIva"].ToString());
				mFacturaCliente.ValorFactura = Convert.ToDouble(dr["valorFactura"].ToString());
				mFacturaCliente.ValorRetencion = Convert.ToDouble(dr["valorRetencion"].ToString());
				mFacturaCliente.Almacen = dr["almacen"].ToString();
				mFacturaCliente.RegimenIva = dr["regimenIva"].ToString();
			}
			return mFacturaCliente;
		}
	}
}
