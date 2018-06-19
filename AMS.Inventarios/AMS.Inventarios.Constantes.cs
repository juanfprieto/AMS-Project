using System;

namespace AMS.Inventarios
{
	public class Almacen
	{
        public const string ALMACENES = "SELECT palm_almacen, palm_descripcion FROM palmacen WHERE PCEN_CENTINV IS NOT NULL and tvig_vigencia='V' ORDER BY PALM_DESCRIPCION";

        //    public const string VENDEDORESPORALMACEN = "SELECT PV.pven_codigo, pven_nombre FROM dbxschema.pvendedor PV, dbxschema.pvendedoralmacen PVA WHERE ((TVEND_CODIGO = 'VM' OR TVEND_CODIGO = 'TT') AND PVA.palm_almacen='{0}' AND PV.PVEN_CODIGO = PVA.PVEN_CODIGO AND PV.PALM_ALMACEN = PVA.PALM_ALMACEN) AND pven_vigencia = 'V' ORDER BY pven_nombre";
        public const string VENDEDORESPORALMACEN =  "SELECT pv.pven_codigo, " +
                                                    "       pv.pven_nombre " +
                                                    "FROM pvendedor pv " +
                                                    "INNER JOIN pvendedoralmacen pva ON pva.pven_codigo = pv.pven_codigo " +
                                                    "WHERE ((TVEND_CODIGO = 'VM' OR TVEND_CODIGO = 'TT')) " +
                                                    "AND   pven_vigencia = 'V' " +
                                                    "AND   pva.palm_almacen = '{0}' " +
                                                    "ORDER BY pven_nombre";
    }
	
	public class Documento
	{
		public const string PROXIMODOCUMENTO = "SELECT pdoc_ultidocu+1 FROM dbxschema.pdocumento WHERE pdoc_codigo='{0}'";

        public const string DOCUMENTOSTIPO = "SELECT pdoc_codigo, pdoc_codigo concat ' - ' concat pdoc_nombre descripcion FROM dbxschema.pdocumento WHERE tdoc_tipodocu='{0}' and tvig_vigencia = 'V' ORDER BY descripcion";

        public const string DOCUMENTOSTIPOHECHO = "SELECT p.pdoc_codigo, p.pdoc_codigo CONCAT ' - ' CONCAT p.pdoc_nombre descripcion FROM pdocumento as P, pdocumentohecho as PH WHERE TRIM(p.tdoc_tipodocu) = '{0}' and p.tvig_vigencia = 'V' and TRIM(PH.tpro_proceso) = '{1}' AND P.PDOC_CODIGO = PH.PDOC_CODIGO AND TRIM(PH.PALM_ALMACEN) ='{2}' ORDER BY descripcion";
	}
}
