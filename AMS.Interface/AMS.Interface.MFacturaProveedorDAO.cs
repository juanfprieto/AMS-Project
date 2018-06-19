/*
 * Autor: Vladimir Oviedo
 * Fecha: 04/12/2005
 */

using System;
using System.Data;
using System.Collections;

//Interno
using AMS.DB;

namespace AMS.Interface
{
	/// <summary>
	/// Acceso a datos para MFacturaProveedor.
	/// </summary>
	public class MFacturaProveedorDAO
	{
		public MFacturaProveedorDAO()
		{
		}
		
		/// <summary>
		/// Retorna las facturas de proveedor que se ecuentran dentro de un
		/// rango de fechas pasados por parametro
		/// </summary>
		/// <param name="fechaInicio">Fecha de inicio del rango</param>
		/// <param name="fechaFin">Fecha final del rango</param>
		/// <returns></returns>
		public ICollection GetMFacturaProveedor(string fechaInicio,string fechaFin)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			MFacturaProveedor mFacturaProveedor;
			sql = " SELECT "+
				" a.PDOC_CODIORDEPAGO AS codigoDocumento, a.MFAC_NUMEORDEPAGO AS "+
				" numeroDocumento, a.MFAC_PREFDOCU AS codigoDocumentoReferencia,"+
				" a.MFAC_NUMEDOCU AS numeroDocumentoReferencia, a.MNIT_NIT AS nit,"+
				" a.MFAC_FACTURA AS fechaFactura, a.MFAC_VALOIVA AS valorIva,"+
				" a.MFAC_VALOFACT AS valorFactura, a.MFAC_VALORETE AS valorRetencion,"+
				" a.PALM_ALMACEN AS almacen, b.TREG_REGIIVA AS regimenIVA FROM MFACTURAPROVEEDOR a,"+
				" MNIT b WHERE a.MNIT_NIT = b.MNIT_NIT AND MFAC_FACTURA BETWEEN "+
				" '"+fechaInicio+"' AND '"+fechaFin+"'";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				mFacturaProveedor = new MFacturaProveedor();
				mFacturaProveedor.CodigoDocumento = dr["codigoDocumento"].ToString();
				mFacturaProveedor.NumeroDocumento = Convert.ToInt32(dr["numeroDocumento"].ToString());
				mFacturaProveedor.CodigoDocumentoReferencia = dr["codigoDocumentoReferencia"].ToString();
				mFacturaProveedor.NumeroDocumentoReferencia = Convert.ToInt32(dr["numeroDocumentoReferencia"].ToString());
				mFacturaProveedor.Nit = dr["nit"].ToString();
				mFacturaProveedor.FechaFactura = dr["fechaFactura"].ToString();
				mFacturaProveedor.ValorIva = Convert.ToDouble(dr["valorIva"].ToString());
				mFacturaProveedor.ValorFactura = Convert.ToDouble(dr["valorFactura"].ToString());
				mFacturaProveedor.ValorRetencion = Convert.ToDouble(dr["valorRetencion"].ToString());
				mFacturaProveedor.Almacen = dr["almacen"].ToString();
				mFacturaProveedor.RegimenIva = dr["regimenIva"].ToString();
				al.Add(mFacturaProveedor);
			}
			return al;
		}
		
		/// <summary>
		/// Retorna un Value de una factura de proveedor de acuerdo a codigo y numero
		/// de documento pasados por parametro
		/// </summary>
		/// <param name="codigoDocumento">Codigo interno de documento</param>
		/// <param name="numeroDocumento">Numero interno de documento</param>
		/// <returns></returns>
		public MFacturaProveedor GetMFacturaProveedorPK(string codigoDocumento,int numeroDocumento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			MFacturaProveedor mFacturaProveedor = new MFacturaProveedor();
			sql = " SELECT "+
				" a.MFAC_PREFDOCU AS codigoDocumentoReferencia,"+
				" a.MFAC_NUMEDOCU AS numeroDocumentoReferencia, a.MNIT_NIT AS nit,"+
				" a.MFAC_FACTURA AS fechaFactura, a.MFAC_VALOIVA AS valorIva,"+
				" a.MFAC_VALOFACT AS valorFactura, a.MFAC_VALORETE AS valorRetencion,"+
				" a.PALM_ALMACEN AS almacen, b.TREG_REGIIVA AS regimenIVA FROM MFACTURAPROVEEDOR a,"+
				" MNIT b WHERE a.MNIT_NIT = b.MNIT_NIT ";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				mFacturaProveedor.CodigoDocumento = codigoDocumento;
				mFacturaProveedor.NumeroDocumento = numeroDocumento;
				mFacturaProveedor.CodigoDocumentoReferencia = dr["codigoDocumentoReferencia"].ToString();
				mFacturaProveedor.NumeroDocumentoReferencia = Convert.ToInt32(dr["numeroDocumentoReferencia"].ToString());
				mFacturaProveedor.Nit = dr["nit"].ToString();
				mFacturaProveedor.FechaFactura = dr["fechaFactura"].ToString();
				mFacturaProveedor.ValorIva = Convert.ToDouble(dr["valorIva"].ToString());
				mFacturaProveedor.ValorFactura = Convert.ToDouble(dr["valorFactura"].ToString());
				mFacturaProveedor.ValorRetencion = Convert.ToDouble(dr["valorRetencion"].ToString());
				mFacturaProveedor.Almacen = dr["almacen"].ToString();
				mFacturaProveedor.RegimenIva = dr["regimenIva"].ToString();
			}
			return mFacturaProveedor;
		}
	}
}
