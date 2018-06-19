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
	/// Acceso a datos para CEmpresa.
	/// </summary>
	public class CempresaDAO
	{
		public CempresaDAO()
		{
		}
		
		/// <summary>
		/// Retorna un objeto Value de un registro en la tabla Cempresa
		/// </summary>
		/// <param name="nit">Nit de la empresa</param>
		/// <returns></returns>
		public Cempresa GetCempresa(string nit)
		{
			string sql = "";
			DataSet ds = new DataSet();
			Cempresa cEmpresa = new Cempresa();
			sql = " SELECT "+
				" CEMP_REGIIVA AS regimenIva from CEMPRESA WHERE"+
				" MNIT_NIT = '"+nit+"'";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				cEmpresa.Nit = nit;				
				cEmpresa.RegimenIva = dr["regimenIva"].ToString();
			}
			return cEmpresa;
		}
		
		/// <summary>
		/// Retorna un objeto Value de un registro en la tabla Cempresa
		/// </summary>
		/// <returns></returns>
		public Cempresa GetCempresa()
		{
			string sql = "";
			DataSet ds = new DataSet();
			Cempresa cEmpresa = new Cempresa();
			sql = " SELECT "+
				" MNIT_NIT AS nit,CEMP_REGIIVA AS regimenIva from CEMPRESA";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				cEmpresa.Nit = dr["nit"].ToString();
				cEmpresa.RegimenIva = dr["regimenIva"].ToString();
			}
			return cEmpresa;
		}
	}
}
