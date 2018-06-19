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
	/// Acceso a datos para MFacturaClienteRetencion.
	/// </summary>
	public class RetencionClienteDAO
	{
		public RetencionClienteDAO()
		{
		}
		
		public ICollection GetRetencionCliente(string codigoDocumento,int numeroDocumento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			RetencionCliente retencionCliente;
			sql = " SELECT "+
				" TRET_CODIGO AS codigoRetencion, MFAC_VALORETE AS valorRetencion FROM "+
				" MFACTURACLIENTERETENCION WHERE PDOC_CODIGO = '"+codigoDocumento+"' "+
				" AND MFAC_NUMEDOCU = "+numeroDocumento.ToString();
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				retencionCliente = new RetencionCliente();
				retencionCliente.CodigoDocumento = codigoDocumento;
				retencionCliente.NumeroDocumento = numeroDocumento;
				retencionCliente.CodigoRetencion = dr["codigoRetencion"].ToString();
				retencionCliente.ValorRetencion = Convert.ToDouble(dr["valorRetencion"].ToString());
				al.Add(retencionCliente);
			}
			return al;
		}
	}
}
