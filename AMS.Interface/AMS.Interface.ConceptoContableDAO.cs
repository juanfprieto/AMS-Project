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
	/// Acceso a datos para TConceptoContable - PPucDocumento.
	/// </summary>
	public class ConceptoContableDAO
	{
		public ConceptoContableDAO()
		{
		}
		
		/// <summary>
		/// Retorna los valores de cuentas PUC para un codigo de documento y
		/// codigo de concepto contable pasados por parametro
		/// </summary>
		/// <param name="codigoDocumento">Codigo de documento</param>
		/// <param name="codigoConcepto">Codigo de concepto</param>
		/// <returns></returns>
		public ConceptoContable GetConceptoContable(string codigoDocumento,string codigoConcepto)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			ConceptoContable conceptoContable = new ConceptoContable();
			sql = " SELECT "+
				" a.TCON_DESCRIPCION AS descripcion, "+
				" b.MCUE_CODIPUC AS codigoPuc "+
				" FROM TCONCEPTOCONTABLE a, PPUCDOCUMENTO b WHERE a.TCON_CODIGO = b.TCON_CODIGO AND "+
				" a.TCON_CODIGO = '"+codigoConcepto+"' AND b.PDOC_CODIGO = '"+codigoDocumento+"'";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				conceptoContable.CodigoConcepto = codigoConcepto;
				conceptoContable.CodigoDocumento = codigoDocumento;
				conceptoContable.CodigoPuc = dr["codigoPuc"].ToString();
				conceptoContable.Descripcion = dr["descripcion"].ToString();
			}
			return conceptoContable;
		}
		
		/// <summary>
		/// Retorna los valores de cuentas de PUC para un codigo de documento
		/// pasado por parametro
		/// </summary>
		/// <param name="codigoDocumento">Codigo de documento</param>
		/// <returns></returns>
		public ICollection GetConceptoContable(string codigoDocumento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			ConceptoContable conceptoContable;
			sql = " SELECT "+
				" a.TCON_DESCRIPCION AS descripcion, "+
				" b.TCON_CODIGO AS codigoConcepto,b.MCUE_CODIPUC AS codigoPuc "+
				" FROM TCONCEPTOCONTABLE a, PPUCDOCUMENTO b WHERE a.TCON_CODIGO = b.TCON_CODIGO AND "+
				" b.PDOC_CODIGO = '"+codigoDocumento+"'";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				conceptoContable = new ConceptoContable();
				conceptoContable.CodigoConcepto = dr["codigoConcepto"].ToString();
				conceptoContable.CodigoDocumento = codigoDocumento;
				conceptoContable.CodigoPuc = dr["codigoPuc"].ToString();
				conceptoContable.Descripcion = dr["descripcion"].ToString();
				al.Add(conceptoContable);
			}
			return al;
		}
	}
}
