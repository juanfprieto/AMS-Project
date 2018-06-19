/*
 * Created by INGESAP EU.
 * User: Vladimir Oviedo
 * Date: 12/6/2005
 */

using System;
using System.Data;
using System.Collections;

//Interno
using AMS.DB;

namespace AMS.Interface
{
	/// <summary>
	/// RetencionICADAO.
	/// </summary>
	public class RetencionICADAO
	{
		public RetencionICADAO()
		{
		}
		
		public RetencionICA GetRetencionICA(string nit)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			RetencionICA retencionICA = new RetencionICA();
			sql = " SELECT "+
				" b.TREG_REGIIVA as regimenIVA, a.PICA_ICA AS codigoICA," +
				" c.MCUE_CODIPUCSIMP AS codigoCuentaSimplificado," +
				" c.MCUE_CODIPUCCOMU AS codigoCuentaComun " +
				" FROM MPROVEEDOR a, MNIT b, PICA c WHERE " +
				" a.MNIT_NIT = b.MNIT_NIT AND a.PICA_ICA = c.PICA_ICA AND " +
				" a.MNIT_NIT = '"+nit+"'";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				retencionICA.CodigoICA = dr["codigoICA"].ToString();
				retencionICA.CodigoCuentaSimplificado = dr["codigoCuentaSimplificado"].ToString();
				retencionICA.CodigoCuentaComun = dr["codigoCuentaComun"].ToString();
				retencionICA.Nit = nit;
			}
			return retencionICA;
		}
	}
}
