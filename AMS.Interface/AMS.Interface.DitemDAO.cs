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
 	/// Objeto de acceso a datos para Ditem
 	/// </summary>
 	public class DitemDAO
 	{
 		public DitemDAO()
 		{
 			
 		}
 		
 		/// <summary>
 		/// Retorna los registros de la tabla DItems cuyo codigo de documento
 		/// y numero de documento correspondan a los pasados por parametro
 		/// </summary>
 		/// <param name="codigoDocumento">Codigo de documento interno</param>
 		/// <param name="numeroDocumento">Numero de documento interno</param>
 		/// <returns></returns>
 		public ICollection GetDitem(string codigoDocumento,int numeroDocumento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			Ditem dItem;
			sql = " SELECT "+
				" a.MITE_CODIGO AS mitemCodigo, a.TORI_CODIGO AS mitemOrigen,"+
				" b.PCEN_CODIGO AS centroCosto, b.DITE_CANTIDAD AS cantidad, "+
				" b.DITE_VALOUNIT AS valorUnitario, b.DITE_COSTPROM AS costoPromedio, "+
				" b.DITE_COSTPROMHIS AS costoPromedioHistorico,"+
				" b.DITE_PORCIVA AS porcentajeIva, b.DITE_PORCDESC AS porcentajeDescuento,"+
				" b.TMOV_TIPOMOVI AS tipoMovimiento, b.PALM_ALMACEN AS almacen FROM "+
				" MITEMS a, DITEMS b  WHERE a.MITE_CODIGO = b.MITE_CODIGO AND "+
				" b.PDOC_CODIGO = '"+codigoDocumento+"' AND b.DITE_NUMEDOCU = "+numeroDocumento.ToString();
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dItem = new Ditem();
				dItem.Codigo = dr["mitemCodigo"].ToString();
				dItem.Origen = dr["mitemOrigen"].ToString();
				dItem.CodigoDocumento = codigoDocumento;
				dItem.NumeroDocumento = numeroDocumento;
				dItem.CentroCosto = dr["centroCosto"].ToString();
				dItem.Cantidad = Convert.ToDouble(dr["cantidad"].ToString());
				dItem.ValorUnitario = Convert.ToDouble(dr["valorUnitario"].ToString());
				dItem.CostoPromedio = Convert.ToDouble(dr["costoPromedio"].ToString());
				dItem.CostoPromedioHistorico = Convert.ToDouble(dr["costoPromedioHistorico"].ToString());
				dItem.PorcentajeIva = Convert.ToDouble(dr["porcentajeIva"].ToString());
				dItem.PorcentajeDescuento = Convert.ToDouble(dr["porcentajeDescuento"].ToString());
				dItem.TipoMovimiento = Convert.ToInt32(dr["tipoMovimiento"].ToString());
				dItem.Almacen = dr["almacen"].ToString();
				al.Add(dItem);
			}
			return al;
		}	
 		
 		/// <summary>
 		/// Retorna los registros de DItems cuyo tipo de movmiento
 		/// sea igual al pasado por parametro
 		/// </summary>
 		/// <param name="tipoMovimiento">Tipo de movimiento</param>
 		/// <returns></returns>
 		public ICollection GetDitem(int tipoMovimiento)
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			Ditem dItem;
			sql = " SELECT "+
				" a.MITE_CODIGO AS mitemCodigo, a.TORI_CODIGO AS mitemOrigen,"+
				" b.PCEN_CODIGO AS centroCosto, b.DITE_CANTIDAD AS cantidad, "+
				" b.DITE_VALOUNIT AS valorUnitario, b.DITE_COSTPROM AS costoPromedio, "+
				" b.DITE_COSTPROMHIS AS costoPromedioHistorico,"+
				" b.DITE_PORCIVA AS porcentajeIva, b.DITE_PORCDESC AS porcentajeDescuento,"+
				" b.PDOC_CODIGO AS codigoDocumento, b.DITE_NUMEDOCU AS numeroDocumento, b.PALM_ALMACEN AS almacen "+
				" FROM "+
				" MITEMS a, DITEMS b  WHERE a.MITE_CODIGO = b.MITE_CODIGO AND "+
				" b.TMOV_TIPOMOVI = "+tipoMovimiento.ToString();
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dItem = new Ditem();
				dItem.Codigo = dr["mitemCodigo"].ToString();
				dItem.Origen = dr["mitemOrigen"].ToString();
				dItem.CodigoDocumento = dr["codigoDocumento"].ToString();
				dItem.NumeroDocumento = Convert.ToInt32(dr["numeroDocumento"].ToString());
				dItem.CentroCosto = dr["centroCosto"].ToString();
				dItem.Cantidad = Convert.ToDouble(dr["cantidad"].ToString());
				dItem.ValorUnitario = Convert.ToDouble(dr["valorUnitario"].ToString());
				dItem.CostoPromedio = Convert.ToDouble(dr["costoPromedio"].ToString());
				dItem.CostoPromedioHistorico = Convert.ToDouble(dr["costoPromedioHistorico"].ToString());
				dItem.PorcentajeIva = Convert.ToDouble(dr["porcentajeIva"].ToString());
				dItem.PorcentajeDescuento = Convert.ToDouble(dr["porcentajeDescuento"].ToString());
				dItem.TipoMovimiento = tipoMovimiento;
				dItem.Almacen = dr["almacen"].ToString();
				al.Add(dItem);
			}
			return al;
		}	
 		
 		/// <summary>
 		/// Retorna todos los registros de Ditems
 		/// </summary>
 		/// <returns></returns>
 		public ICollection GetDitem()
		{
			string sql = "";
			DataSet ds = new DataSet();
			ArrayList al = new ArrayList();
			Ditem dItem;
			sql = " SELECT "+
				" a.MITE_CODIGO AS mitemCodigo, a.TORI_CODIGO AS mitemOrigen,"+
				" b.PCEN_CODIGO AS centroCosto, b.DITE_CANTIDAD AS cantidad, "+
				" b.DITE_VALOUNIT AS valorUnitario, b.DITE_COSTPROM AS costoPromedio, "+
				" b.DITE_COSTPROMHIS AS costoPromedioHistorico,"+
				" b.DITE_PORCIVA AS porcentajeIva, b.DITE_PORCDESC AS porcentajeDescuento,"+
				" b.PDOC_CODIGO AS codigoDocumento, b.DITE_NUMEDOCU AS numeroDocumento,"+
				" b.TMOV_TIPOMOVI AS tipoMovimiento, b.PALM_ALMACEN AS almacen FROM "+
				" MITEMS a, DITEMS b  WHERE a.MITE_CODIGO = b.MITE_CODIGO";
			ds = DBFunctions.Request(ds,IncludeSchema.NO,sql);
			foreach(DataRow dr in ds.Tables[0].Rows)
			{
				dItem = new Ditem();
				dItem.Codigo = dr["mitemCodigo"].ToString();
				dItem.Origen = dr["mitemOrigen"].ToString();
				dItem.CodigoDocumento = dr["codigoDocumento"].ToString();
				dItem.NumeroDocumento = Convert.ToInt32(dr["numeroDocumento"].ToString());
				dItem.CentroCosto = dr["centroCosto"].ToString();
				dItem.Cantidad = Convert.ToDouble(dr["cantidad"].ToString());
				dItem.ValorUnitario = Convert.ToDouble(dr["valorUnitario"].ToString());
				dItem.CostoPromedio = Convert.ToDouble(dr["costoPromedio"].ToString());
				dItem.CostoPromedioHistorico = Convert.ToDouble(dr["costoPromedioHistorico"].ToString());
				dItem.PorcentajeIva = Convert.ToDouble(dr["porcentajeIva"].ToString());
				dItem.PorcentajeDescuento = Convert.ToDouble(dr["porcentajeDescuento"].ToString());
				dItem.TipoMovimiento = Convert.ToInt32(dr["tipoMovimiento"].ToString());
				dItem.Almacen = dr["almacen"].ToString();
				al.Add(dItem);
			}
			return al;
		}
 	}
 }
