using System;

namespace AMS.Vehiculos
{
	public class CatalogoVehiculos
	{
		public const string CATALOGOVEHICULOS = "SELECT pcat_codigo, '[' concat pcat_codigo concat '] - [' concat pcat_descripcion concat ']' descripcion FROM dbxschema.pcatalogovehiculo order by pcat_descripcion,pcat_codigo";

        public const string CATALOGOVEHICULOSRECEPCIONADOS = @"SELECT pcat_codigo, '' concat pcat_codigo concat ' - ' concat pcat_descripcion
                                                                FROM dbxschema.pcatalogovehiculo
                                                                WHERE pcat_codigo IN (SELECT DISTINCT MC.pcat_codigo
                                                                                      FROM DBXSCHEMA.MVEHICULO mv,
                                                                                           McatalogoVEHICULO MC
                                                                                      WHERE test_tipoesta IN (10,20)
                                                                                      AND   MC.MCAT_VIN = MV.MCat_VIN)
                                                                ORDER BY pcat_codigo;";

		public const string CATALOGOVEHICULOSENALMACEN = @"SELECT 
	   pcat_codigo, 
	   '[' concat pcat_codigo concat '] - [' concat pcat_descripcion concat ']' descripcion 
FROM 
	 dbxschema.pcatalogovehiculo 
WHERE
	 pcat_codigo IN (Select DISTINCT MC.pcat_codigo from DBXSCHEMA.MVEHICULO MV, McatalogoVEHICULO MC WHERE test_tipoesta IN (10,20,30,40) AND MC.MCAT_VIN = MV.MCat_VIN)
order by
	  pcat_descripcion,
	  pcat_codigo";

		public const string CATALOGOVEHICULOSASIGNADOS = @"SELECT 
	   pcat_codigo, 
	   '[' concat pcat_codigo concat '] - [' concat pcat_descripcion concat ']' descripcion 
FROM 
	 dbxschema.pcatalogovehiculo 
WHERE
	 pcat_codigo IN (Select DISTINCT MC.pcat_codigo from DBXSCHEMA.MVEHICULO MV, McatalogoVEHICULO MC WHERE test_tipoesta = 30 AND MC.MCAT_VIN = MV.MCat_VIN)
order by
	  pcat_descripcion,
	  pcat_codigo";

        public const string CATALOGOVEHICULOSLISTA =
            @"SELECT DISTINCT pc.Pcat_codigo, pmar_nombre concat '  -  ' concat pcat_descripcion concat '  -  ' concat pc.pcat_codigo AS descripcion  
             FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp  
             WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO  
             ORDER BY descripcion;";

        public const string CATALOGOVEHICULOSLISTA2 =
            @"SELECT DISTINCT pc.Pcat_codigo, PC.PCAT_CODIGO concat '  -  ' concat pcat_descripcion concat '  -  ' concat pc.pcat_codigo AS descripcion  
             FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp  
             WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO  
             ORDER BY descripcion;";

        public const string CATALOGOVEHICULOSLISTAUSADOS =
            @"SELECT DISTINCT mcv.mcat_vin, 
                     mcv.mcat_vin || ' - [' || pm.pmar_nombre || '] - ' || pca.pcat_descripcion || ' - Placa ' || mcv.mcat_placa AS descripcion  
             FROM dbxschema.pcatalogovehiculo pca,  
                  dbxschema.mcatalogovehiculo mcv,  
                  dbxschema.mvehiculo mv,
				  dbxschema.pmarca pm  
             WHERE mcv.pcat_codigo = pca.pcat_codigo  
             AND   mcv.mcat_vin = mv.mcat_vin  
             AND   (mv.test_tipoesta = 10 OR mv.test_tipoesta = 20)  
             AND   mv.tcla_codigo = 'U'
			 and   pca.pmar_codigo = pm.pmar_codigo  
             ORDER BY descripcion;";

		public const string CATALOGOVEHICULOSCOMERCIAN = 
       @"SELECT pcat_codigo, pcat_codigo concat ' - ' concat pcat_descripcion as descripcion 
         FROM dbxschema.pcatalogovehiculo 
         WHERE pcat_codigo IN (Select DISTINCT MC.pcat_codigo from DBXSCHEMA.MVEHICULO MV, McatalogoVEHICULO MC WHERE MC.MCAT_VIN = MV.MCat_VIN)
         order by pcat_descripcion, pcat_codigo";

        public const string CATALOGOVEHICULOSRECEPCIONADOSSINFACTURAR =
        @"SELECT PC.pcat_codigo CONCAT SUBSTR(MC.MCAT_VIN,(LENGTH(MC.MCAT_VIN)-2),3)  as pcat_codigo, MC.MCAT_VIN concat ' - ' concat  pMAR_NOMBRE concat ' - '  concat pcat_descripcion as descripcion 
          FROM  dbxschema.pcatalogovehiculo PC, PMARCA PM, dbxschema.mvehiculo MV, McatalogoVEHICULO MC 
          WHERE PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PC.pcat_codigo = MC.PCAT_CODIGO  AND (pdoc_codiordepago IS NULL or pdoc_codiordepago = '') AND MC.MCAT_VIN = MV.MCat_VIN
          order by 2, 1;";
       //@" SELECT PC.pcat_codigo, MC.MCAT_VIN concat ' - ' concat  pMAR_NOMBRE concat ' - '  concat pcat_descripcion as descripcion 
       //   FROM  dbxschema.pcatalogovehiculo PC, PMARCA PM, dbxschema.mvehiculo MV, McatalogoVEHICULO MC 
       //   WHERE PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PC.pcat_codigo = MC.PCAT_CODIGO  AND (pdoc_codiordepago IS NULL or pdoc_codiordepago = '') AND MC.MCAT_VIN = MV.MCat_VIN
       //   order by 2, 1;";
	}

	public class Vehiculos
	{
        public const string VEHICULOS = "SELECT mveh_inventario NUMERO, MV.mcat_vin VIN FROM MVEHICULO MV, MCATALOGOVEHICULO MC WHERE MV.mcat_vin = MC.mcat_vin AND MC.pcat_codigo='{0}' ORDER BY MV.mcat_vin";

        public const string VEHICULOSRECEPCIONADOS = "SELECT mveh_inventario, MV.mcat_vin FROM MVEHICULO MV, MCATALOGOVEHICULO MC WHERE MV.mcat_vin = MC.mcat_vin AND MC.pcat_codigo='{0}' AND test_tipoesta IN (10,20) ORDER BY MV.mcat_vin";

        public const string VEHICULOSENALMACEN = @"SELECT mveh_inventario, MV.mcat_vin || '   ' || pc.pcat_descripcion  || '   ' || test_nombesta
	                                              FROM MVEHICULO MV, MCATALOGOVEHICULO MC, pcatalogovehiculo pc, testadovehiculo te 
	                                              WHERE MV.mcat_vin = MC.mcat_vin AND mv.test_tipoesta IN (10,20,30,40) and mc.pcat_codigo = pc.pcat_codigo and mv.test_tipoesta = te.test_tipoesta
	                                              ORDER BY MV.mcat_vin;";

        public const string VEHICULOSASIGNADOS = "SELECT mveh_inventario, MV.mcat_vin FROM MVEHICULO MV, MCATALOGOVEHICULO MC WHERE MV.mcat_vin = MC.mcat_vin AND MC.pcat_codigo='{0}' AND test_tipoesta = 30 ORDER BY MV.mcat_vin";

        public const string VEHICULOSFACTURADOS = "SELECT mveh_inventario, MC.pcat_codigo CONCAT ' - ' CONCAT MV.mcat_vin FROM dbxschema.mvehiculo MV, MCATALOGOVEHICULO mc WHERE test_tipoesta = 40 AND MC.MCAT_VIN = MV.MCat_VIN ORDER BY MC.pcat_codigo, MV.mcat_vin";

        public const string VEHICULOSRECEPCIONADOSSINFACTURAR = "SELECT MV.mcat_vin FROM mvehiculo MV, MCATALOGOVEHICULO MC WHERE MV.mcat_vin = MC.mcat_vin AND (MC.pcat_codigo CONCAT SUBSTR(MV.MCAT_VIN,(LENGTH(MV.MCAT_VIN)-2),3) )='{0}' AND (pdoc_codiordepago = ''  or pdoc_codiordepago is null ) ORDER BY MV.mcat_vin";
	}

	public class Colores
	{
        public const string COLORES      = "SELECT pcol_codigo, pcol_descripcion concat ' - [' concat ptip_descripcion concat ']' DESCRIPCION, pcol_codrgb FROM dbxschema.pcolor pcol inner join dbxschema.ptipopintura ptpi on pcol.ptip_codigo = ptpi.ptip_codigo where pcol_activo in ('S', 'SI') ORDER BY pcol_descripcion";
        public const string COLORESTODOS = "SELECT pcol_codigo, pcol_descripcion concat ' - [' concat ptip_descripcion concat ']' DESCRIPCION, pcol_codrgb FROM dbxschema.pcolor pcol inner join dbxschema.ptipopintura ptpi on pcol.ptip_codigo = ptpi.ptip_codigo ORDER BY pcol_descripcion";
	}

	public class Documento
	{
        public const string DOCUMENTOSTIPO = "SELECT pdoc_codigo, pdoc_codigo concat ' - ' concat pdoc_nombre descripcion FROM dbxschema.pdocumento WHERE tdoc_tipodocu='{0}' and tvig_vigencia = 'V' ORDER BY descripcion";

        public const string DOCUMENTOSTIPOPROCESO = "SELECT pdoc_codigo, pdoc_codigo concat ' - ' concat pdoc_nombre descripcion FROM dbxschema.pdocumento WHERE tdoc_tipodocu='{0}' and tvig_vigencia = 'V' and pdoc_codigo in (Select pdoc_codigo from dbxschema.pdocumentohecho where tpro_proceso='{1}') ORDER BY descripcion";
	}

	public class Vendedores
	{
        public const string VENDEDORESVEHICULOSALMACEN = "SELECT pven_codigo, pven_nombre FROM dbxschema.pvendedor WHERE ((tvend_codigo='VV' OR tvend_codigo='TT') AND palm_almacen='{0}') and pven_vigencia = 'V' ORDER BY pven_nombre";

        public const string VENDEDORESVEHICULOS = "SELECT pven_codigo, pven_nombre FROM dbxschema.pvendedor WHERE tvend_codigo IN ('VV','TT') and pven_vigencia = 'V' ORDER BY pven_nombre";
	}
}
