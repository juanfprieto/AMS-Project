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
	/// Acceso a datos para MFacturaProveedorRetencion.
	/// </summary>
	public class RetencionProveedorDAO
	{
		public RetencionProveedorDAO()
		{
		}
		
		public ICollection GetRetencionProveedor(string codigoDocumento,int numeroDocumento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			RetencionProveedor retencionProveedor;
			sql = " SELECT "+
				" TRET_CODIGO AS codigoRetencion, MFAC_VALORETE AS valorRetencion FROM "+
				" MFACTURAPROVEEDORRETENCION WHERE PDOC_CODIORDEPAGO = '"+codigoDocumento+"' "+
				" AND MFAC_NUMEORDEPAGO = "+numeroDocumento.ToString();
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				retencionProveedor = new RetencionProveedor();
				retencionProveedor.CodigoDocumento = codigoDocumento;
				retencionProveedor.NumeroDocumento = numeroDocumento;
				retencionProveedor.CodigoRetencion = dr["codigoRetencion"].ToString();
				retencionProveedor.ValorRetencion = Convert.ToDouble(dr["valorRetencion"].ToString());
				al.Add(retencionProveedor);
			}
			return al;
		}
	}
}
