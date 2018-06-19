using System;
using System.Collections;
using System.Data;
using AMS.DB;
using AMS.Documentos;
using AMS.Tools;

namespace AMS.Inventarios
{
	public class Items
	{
		public static string NombreReferencia(string codigoItem)
		{
			string nombreReferencia = string.Empty;

			try
			{
				nombreReferencia = DBFunctions.SingleData("SELECT mite_nombre FROM mitems WHERE mite_codigo='"+codigoItem+"'");			
			}
			catch { }

			return nombreReferencia;
		}

		public static bool ValidarExistenciaItem(string lineaBodega, string codigoModificado)
		{		
			string codI = "";
			Referencias.Guardar(codigoModificado,ref codI,lineaBodega);
			return DBFunctions.RecordExist("SELECT mite_codigo FROM mitems WHERE mite_codigo='"+codI+"'");
		}

		public static DataTable ConsultarDetalleFactura(string prefijoFactura, string numeroFactura)
		{
			DataSet dsConsultaFactura = new DataSet();
			DBFunctions.Request(dsConsultaFactura,IncludeSchema.NO,"SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) AS REFERENCIA,"+
				"MIT.mite_nombre AS NOMBRE, DIT.dite_cantidad AS CANTIDAD, DIT.dite_valounit AS VALOR_UNIDAD, DIT.piva_porciva AS PORCENTAJE_IVA,"+
				"(DIT.dite_valounit*DIT.piva_porciva/100)*DIT.dite_cantidad AS VALOR_IVA,"+
				"(DIT.dite_cantidad*DIT.dite_valounit)+(DIT.dite_valounit*DIT.piva_porciva/100)*DIT.dite_cantidad AS VALOR "+
				"FROM dbxschema.mitems MIT INNER JOIN dbxschema.plineaitem PLIN ON MIT.plin_codigo = PLIN.plin_codigo "+
				"INNER JOIN dbxschema.ditems DIT ON MIT.mite_codigo = DIT.mite_codigo "+
				"WHERE DIT.pdoc_codigo = '"+prefijoFactura+"' AND DIT.dite_numedocu = "+numeroFactura+"");
			return dsConsultaFactura.Tables[0];
		}

		public static DataTable ConsultarCatalogosPorGrupo(string codigoGrupo)
		{
			DataSet dsConsulta = new DataSet();

			dsConsulta = DBFunctions.Request(dsConsulta,IncludeSchema.NO,"SELECT pcat_codigo, pcat_descripcion FROM pcatalogovehiculo WHERE pgru_grupo='"+codigoGrupo+"' ORDER BY pcat_codigo ASC");

			return dsConsulta.Tables[0];
		}

		public static double CantidadItemsPorCatalogo(string codigoCatalogo, string codigoItem)
		{
			double cantidadSalida = 0;

			try
			{
				cantidadSalida = Convert.ToDouble(DBFunctions.SingleData("SELECT mic_cantidaduso FROM mitemscatalogo WHERE mite_codigo='"+codigoItem+"' AND pcat_codigo='"+codigoCatalogo+"'"));
			}
			catch { }

			return cantidadSalida;
		}

		public static string CrearRelacionItemGrupo(string codigoItem, string codigoLineaBodega, string codigoGrupo, string cantidadUsoGrupo, DataTable dtCatalogosEscogidos)
		{
			string processMsg = "";
			ArrayList sql = new ArrayList();
			string codI = "";
			Referencias.Guardar(codigoItem, ref codI, codigoLineaBodega);

			//Se agrega el insert correspondiente a la relacion de grupo con item
			if(!DBFunctions.RecordExist("SELECT mig_cantidaduso FROM mitemsgrupo WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+codigoGrupo+"'"))
			{
				if(cantidadUsoGrupo != "")
					sql.Add("INSERT INTO mitemsgrupo(mite_codigo,pgru_grupo,mig_cantidaduso) VALUES('"+codI+"','"+codigoGrupo+"',"+cantidadUsoGrupo+")");
				else
					sql.Add("INSERT INTO mitemsgrupo(mite_codigo,pgru_grupo,mig_cantidaduso) VALUES('"+codI+"','"+codigoGrupo+"',null)");
			}
			else
			{
				if(cantidadUsoGrupo != "")
					sql.Add("UPDATE mitemsgrupo SET mig_cantidaduso="+cantidadUsoGrupo+" WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+codigoGrupo+"'");
				else
					sql.Add("UPDATE mitemsgrupo SET mig_cantidaduso=NULL WHERE mite_codigo='"+codI+"' AND pgru_grupo='"+codigoGrupo+"'");
			}

			for(int i=0;i<dtCatalogosEscogidos.Rows.Count;i++)
			{
				sql.Add("DELETE FROM mitemscatalogo WHERE mite_codigo='"+codI+"' AND pcat_codigo='"+dtCatalogosEscogidos.Rows[i][0]+"'");

				if(Convert.ToBoolean(dtCatalogosEscogidos.Rows[i][3]))
				{
					if(dtCatalogosEscogidos.Rows[i][2].ToString() != "")
						sql.Add("INSERT INTO mitemscatalogo(mite_codigo,pcat_codigo,mic_cantidaduso) VALUES('"+codI+"','"+dtCatalogosEscogidos.Rows[i][0].ToString()+"',"+dtCatalogosEscogidos.Rows[i][2].ToString()+")");
					else
						sql.Add("INSERT INTO mitemscatalogo(mite_codigo,pcat_codigo,mic_cantidaduso) VALUES('"+codI+"','"+dtCatalogosEscogidos.Rows[i][0].ToString()+"',null)");
				}
			}

			if(DBFunctions.Transaction(sql))
				processMsg += "<br>Bien "+DBFunctions.exceptions;
			else
				processMsg += "<br>ERROR : "+DBFunctions.exceptions;

			return processMsg;
		}

		public static double ConsultaRelacionItemsTransferencias(string referenciaConsultar, string prefijoDocumento, long numeroDocumento, int TipoMovimiento, int TipoMovimientoContrario)
		{
			double cantidadInicial=0;
			double cantidadDevuelta=0;
			double salida = 0;

			//Primero se revisa si existe un registro de devolucion de esta referencia en la tabla ditems y por este documento y con los tipos de movimiento especificos
			if(DBFunctions.RecordExist("SELECT dite_secuencia FROM ditems WHERE dite_prefdocurefe='"+prefijoDocumento+"' AND dite_numedocurefe="+numeroDocumento+" AND tmov_tipomovi="+TipoMovimientoContrario+" AND mite_codigo='"+referenciaConsultar+"'"))
			{
				DataSet ds = new DataSet();
				DBFunctions.Request(ds,IncludeSchema.NO,"SELECT dite_cantidad FROM dbxschema.ditems WHERE pdoc_codigo='"+prefijoDocumento+"' AND dite_numedocu="+numeroDocumento+" and mite_codigo='"+referenciaConsultar+"' AND tmov_tipomovi="+TipoMovimiento+";SELECT dite_cantidad FROM ditems WHERE dite_prefdocurefe='"+prefijoDocumento+"' AND dite_numedocurefe="+numeroDocumento+" AND tmov_tipomovi="+TipoMovimientoContrario+" AND mite_codigo='"+referenciaConsultar+"'");
				cantidadInicial = double.Parse(ds.Tables[0].Rows[0][0].ToString());
				
				for(int i=0;i<ds.Tables[1].Rows.Count;i++)
					cantidadDevuelta = cantidadDevuelta+double.Parse(ds.Tables[1].Rows[i][0].ToString());
				salida = cantidadInicial - cantidadDevuelta;
			}
			else
				salida = double.Parse(DBFunctions.SingleData("SELECT dite_cantidad FROM ditems WHERE pdoc_codigo='"+prefijoDocumento+"' AND dite_numedocu="+numeroDocumento+" AND mite_codigo = '"+referenciaConsultar+"'"));

			return salida;
		}
   }
}
