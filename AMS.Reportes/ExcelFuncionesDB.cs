using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.Reportes
{
    public class ExcelFuncionesDB
    {
        // VEHICULOS VENDIDOS
        // VehVend...Params:   
        // {0},Año  
        // {1},Mes  
        // {2},CatalogoVehiculo
        public const string VEHICULOSVENDIDOS =
            @"select  coalesce(COUNT(*),0)
            from  mfacturacliente mf, mfacturapedidovehiculo mfp, mvehiculo mv, mcatalogovehiculo mc
            where year(mf.mfac_factura) = {0} and month(MF.MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIGO = MF.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFP.MFAC_NUMEDOCU
            AND  MFP.MVEH_INVENTARIO = MV.MVEH_INVENTARIO AND MFP.MFAC_TIPCLIE = 'C'
            AND  MV.MCAT_VIN = MC.MCAT_VIN AND MC.PCAT_CODIGO IN ({2})";

        // NUMERO DE ENTRADA DE TALLER   
        // EntraTaller...Params:   
        // {0},Año  
        // {1},Mes  
        // {2},Cargo -> C-S  o  G  o  A-I-T  (CLIENTES, GARANTIAS, INTERNAS)
        public const string NUMEENTRADATALLER =
            @"SELECT COALESCE(COUNT(*),0) AS TOTAL_REPTOS
            FROM  MORDEN MO WHERE YEAR(MO.MORD_ENTRADA) = {0} AND MONTH(MO.MORD_ENTRADA) = {1}
            AND  MO.TTRA_CODIGO IN ('M') AND  MO.TCAR_CARGO IN ({2});";

        // VALOR REPUESTOS ORIGEN FABRICA FACTURADOS EN MANTENIMIENTOS PERIODICO CLIENTE Y SEGUROS ORIGEN FABRICA
        // ValRepFabrica...Params:
        // {0},Año  
        // {1},Mes  
        // {2},Cargo -> C-S  o  G  o  A-I-T  (CLIENTES, GARANTIAS, INTERNAS)
        // {3},Tipo Trabajo -> R  o  L  o  M-S-X-P  (REVISIONES PERIODICAS, LATONERIA, MECANICA)
        public const string VALOREPUESTOSFABRICA =
            //            @"SELECT COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
            //            FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO, MORDENTRANSFERENCIA MOT, MITEMS MI, DITEMS DI
            //            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            //            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            //            AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
            //            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            //            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
            //            AND  DI.MITE_CODIGO = MI.MITE_CODIGO   AND MI.TORI_CODIGO != 'Y'
            //            AND  MFT.TCAR_CARGO IN ({2})
            //            AND  MO.TTRA_CODIGO IN ({3})";
            @"SELECT COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
            FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO , mcatalogovehiculo mc, pcatalogovehiculo pc, MORDENTRANSFERENCIA MOT, MITEMS MI, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO   AND MI.TORI_CODIGO != 'Y'
            and  mot.tcar_cargo = mft.tcar_cargo
            AND  mO.Mcat_VIN    = Mc.Mcat_VIN      AND mc.pcat_codigo = pc.pcat_codigo 
            AND  MFT.TCAR_CARGO IN ({2})
            AND  MO.TTRA_CODIGO IN ({3})
            AND PGAM_CODIGO IN ({4})";

        // FACTURACION COMBUSTIBLES Y LUBRICANTES - VALOR REPUESTOS ORIGEN FABRICA FACTURADOS EN MANTENIMIENTOS PERIODICO CLIENTE Y SEGUROS ORIGEN FABRICA
        // FactCombustible...Params:
        // {0},Año  
        // {1},Mes  
        public const string FACTURACOMBUSTIBLELUBRI =
        //            @"SELECT COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
        //            FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO, MORDENTRANSFERENCIA MOT, MITEMS MI, DITEMS DI
        //            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
        //            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
        //            AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
        //            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
        //            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
        //            AND  DI.MITE_CODIGO = MI.MITE_CODIGO   AND MI.TORI_CODIGO = 'Y'";

        @"SELECT COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
            FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO,mcatalogovehiculo mc, pcatalogovehiculo pc, MORDENTRANSFERENCIA MOT, MITEMS MI, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
            and  mot.tcar_cargo = mft.tcar_cargo
            AND  mO.Mcat_VIN    = Mc.Mcat_VIN      AND mc.pcat_codigo = pc.pcat_codigo 
            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO   AND MI.TORI_CODIGO = 'Y'
            AND PGAM_CODIGO IN ('{2}');";

        // VALOR DE LAS DEVOLUCIONES EFECTUADAS AL PROVEEDOR (NIT SELECCIONADO)
        // ValDevolProv...Params:
        // {0},Año  
        // {1},Mes 
        // {2},NIT proveedor
        public const string VALODEVOLUCIONPROV =
            @"SELECT COALESCE(SUM(di.DITE_CANTIDAD*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
            FROM  MFACTURAPROVEEDOR MF, DITEMS DI
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND   MF.PDOC_CODIORDEPAGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEORDEPAGO = DI.DITE_NUMEDOCU AND DI.TMOV_TIPOMOVI = 31 AND DI.MNIT_NIT = '{2}'";

        // VALOR DESCUENTO EFECTUADO A LOS CLIENTES POR REPUESTOS
        // ValCliRep...Params:
        // {0},Año  
        // {1},Mes 
        public const string VALORDESCTOCLTEREPTOSTALL =
            //            @"SELECT COALESCE(SUM(mfac_descrepuestos),0) AS TOTAL_descto FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT
            //            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND YEAR(MF.MFAC_FACTURA) = {0} 
            //            AND MONTH(MFAC_FACTURA) = {1}";

                @"SELECT COALESCE(SUM(mfac_descrepuestos),0) AS TOTAL_descto FROM  MFACTURACLIENTE MF, 
						MFACTURACLIENTETALLER MFT, 
						MCATALOGOVEHICULO MC, 
						PCATALOGOVEHICULO PC, 
						MORDEN MO
                        WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO 
                        AND MO.PDOC_CODIGO = MFT.PDOC_PREFORDETRAB
                        AND MO.MORD_NUMEORDE = MFT.MORD_NUMEORDE
                        AND MO.MCAT_VIN = MC.MCAT_VIN
                        AND MC.PCAT_CODIGO = PC.PCAT_CODIGO
                        AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU 
                        AND YEAR(MF.MFAC_FACTURA) = {0}
                        AND MONTH(MFAC_FACTURA) = {1}
                        AND PC.PGAM_CODIGO IN ('{2}');";
        // VALOR DESCUENTO EFECTUADO A LOS CLIENTES POR mano de OBRA MAS REPUESTOS
        // ValCliRep...Params:
        // {0},Año  
        // {1},Mes 
        public const string VALORDESCTOCLTEMOBRATALL =
            @"SELECT COALESCE(SUM(MFAC_DESCREPUESTOS+MFAC_DESCOPERACIONES),0) AS TOTAL_descto 
			FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, morden mo, mcatalogovehiculo mc, pcatalogovehiculo pc
            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU 
			 AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1} AND PGAM_CODIGO IN ({2})
			 and mft.pdoc_prefordetrab = mo.pdoc_codigo and mft.mord_numeorde = mo.mord_numeorde
			 and mo.mcat_vin = mc.mcat_vin
			 and mc.pcat_codigo = pc.pcat_codigo ; ";

        // COSTO DE VENTA REPUESTOS POR TALLER
        // CostVentRep...Params:
        // {0},Año  
        // {1},Mes 
        public const string COSTOVENTAREPUESTOSTALL =
            @"SELECT COALESCE(SUM((DI.DITE_CANTIDAD - COALESCE(DID.DITE_CANTIDAD,0))*DI.DITE_COSTPROM),0) AS TOTAL_REPTOS
            FROM  MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO,mcatalogovehiculo mc, pcatalogovehiculo pc, MORDENTRANSFERENCIA MOT, MITEMS MI, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
            and mc.pcat_codigo = pc.pcat_codigo
            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO
            and  mot.tcar_cargo = mft.tcar_cargo
            AND  mO.Mcat_VIN    = Mc.Mcat_VIN      AND mc.pcat_codigo = pc.pcat_codigo 
            AND  PGAM_CODIGO IN ('{2}')
            ";

        // venta REPUESTOS MOSTRADOR
        // {0}, Año
        // {1}, Mes
        public const string VENTAREPUESTOSMOSTRADOR =
            @"SELECT COALESCE(SUM(DI.DITE_CANTIDAD * DI.DITE_VALOUNIT),0) AS VENTA_TOTAL
            FROM  MFACTURACLIENTE MF, MITEMS MI, DITEMS DI
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO 
			AND  DI.TMOV_TIPOMOVI = 90;";


        // DESCUENTO venta REPUESTOS MOSTRADOR
        // {0}, Año
        // {1}, Mes
        public const string DESCUENTOVENTAREPUESTOSMOSTRADOR =
            @"SELECT COALESCE(SUM(DI.DITE_CANTIDAD * DI.DITE_VALOUNIT * (DITE_PORCDESC*0.01)),0) AS VENTA_TOTAL
            FROM  MFACTURACLIENTE MF, MITEMS MI, DITEMS DI
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO 
			AND  DI.TMOV_TIPOMOVI = 90;";

        // Devolucion VENTA REPUESTOS MOSTRADOR
        // {0}, Año
        // {1}, Mes
        public const string DEVOLUCIONVENTAREPTOSMOSTRADOR =
            @"SELECT COALESCE(SUM(DI.DITE_CANTIDAD * DI.DITE_VALOUNIT * (1 -(DITE_PORCDESC*0.01))),0) AS VENTA_TOTAL
            FROM  MFACTURACLIENTE MF, MITEMS MI, DITEMS DI
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO 
			AND  DI.TMOV_TIPOMOVI = 91;";

        // COSTO VENTAS REPUESTOS MOSTRADOR
        // {0}, Año
        // {1}, Mes
        public const string COSTOVENTAREPTOSMOSTRADOR =
            @"SELECT COALESCE(SUM((DI.DITE_CANTIDAD - COALESCE(DID.DITE_CANTIDAD,0))*DI.DITE_COSTPROM),0) AS TOTAL_REPTOS
            FROM  MFACTURACLIENTE MF, MITEMS MI, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 91
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO 
			AND  DI.TMOV_TIPOMOVI = 90;";

        // CONSUMOS INTERNOS DE REPUESTOS por MOSTRADOR
        // {0}, Año
        // {1}, Mes
        public const string CONSUMOINTERNOREPUESTOS =
            @"SELECT COALESCE(SUM((DI.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))* DI.DITE_VALOUNIT),0) AS VENTA_TOTAL
            FROM  MFACTURACLIENTE MF, MITEMS MI, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO 
			AND  DI.TMOV_TIPOMOVI = 60; ";

       // GASTOS DE VENTAS POR CENTRO DE COSTOS
        // {0}, Año
        // {1}, Mes
        // {2} Centro de Costo
        public const string GASTOSVENTACENTROCOSTO =
            @"select COALESCE(sum(dcue_valodebi - dcue_valocred),0) 
            from  dcuenta d, mcomprobante m
            where d.pdoc_codigo = m.pdoc_codigo and d.mcom_numedocu =  m.mcom_numedocu and pano_ano = {0} and pmes_mes = {1} and mcue_codipuc like '52%'
            and pcen_codigo in ({2}); ";

        // COMPRA ITEMS A PROVEEDOR
        // {0}, Año
        // {1}, Mes
        // {2}, Nit del Proveedor
        public const string COMPRAITEMSAPROVEEDOR =
            @"SELECT COALESCE(SUM((DI.DITE_CANTIDAD - COALESCE(DID.DITE_CANTIDAD,0))*DI.DITE_VALOUNIT * (1 - (DI.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
            FROM  MFACTURAPROVEEDOR MF, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 31
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIORDEPAGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEORDEPAGO = DI.DITE_NUMEDOCU
            AND  DI.TMOV_TIPOMOVI IN (11,30)
			AND  DI.MNIT_NIT in ({2}); ";

        // COMPRA ITEMS A OTROS PROVEEDORES
        // {0}, Año
        // {1}, Mes
        // {2}, Nit de OTROS Proveedores
        public const string COMPRAITEMSOTROSPROVEEDORES =
            @"SELECT COALESCE(SUM((DI.DITE_CANTIDAD - COALESCE(DID.DITE_CANTIDAD,0))*DI.DITE_VALOUNIT * (1 - (DI.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS
            FROM  MFACTURAPROVEEDOR MF, DITEMS DI
            LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 31
            WHERE YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
            AND  MF.PDOC_CODIORDEPAGO = DI.PDOC_CODIGO AND MF.MFAC_NUMEORDEPAGO = DI.DITE_NUMEDOCU
            AND  DI.TMOV_TIPOMOVI IN (11,30)
			AND  DI.MNIT_NIT not in ({2}); ";

        // TOTAL RENGLONES ITEMS PEDIDOS POR LOS CLIENTES
        // {0}, Año
        // {1}, Mes
        public const string TOTALRENGLONESPEDIDOS =
            @"select COALESCE(COUNT(*),0) from dpedidoitem d, mpedidoitem m, PPEDIDO P
            where d.mped_clasregi = m.mped_claseregi and d.mnit_Nit = m.mnit_nit and d.pped_codigo = m.pped_codigo and d.mped_numepedi = m.mped_numepedi
            and m.mped_claseregi = 'C' AND TPED_CODIGO NOT IN ('C','P') AND M.PPED_CODIGO = P.PPED_CODIGO
            and year(m.mped_pedido) = {0} and month(m.mped_pedido) = {1}; ";

        // TOTAL RENGLONES DE ITEMS DESPACHADOS AL TALLER Y MOSTRADOR
        // {0}, Año
        // {1}, Mes
        public const string TOTALRENGLONESDESPACHADOS =
            @"select COALESCE(COUNT(*),0) from dITEMS
            where TMOV_TIPOMOVI IN (80,90) ANd PANO_ANO = {0} and PMES_MES  = {1}; ";

        // TOTAL RENGLONES ITEMS PEDIDOS POR LOS CLIENTES
        // {0}, Año
        // {1}, Mes
        public const string TOTALRENGLONESBACKORDER =
            @"select COALESCE(COUNT(*),0) from dpedidoitem d, mpedidoitem m, PPEDIDO P
            where d.mped_clasregi = m.mped_claseregi and d.mnit_Nit = m.mnit_nit and d.pped_codigo = m.pped_codigo and d.mped_numepedi = m.mped_numepedi
            and m.mped_claseregi = 'C' AND TPED_CODIGO NOT IN ('C','P') AND M.PPED_CODIGO = P.PPED_CODIGO
            and year(m.mped_pedido) = {0} and month(m.mped_pedido) = {1} and dped_cantpedi > (dped_cantasig + dped_cantfact); ";

        // TOTAL RENGLONES ITEMS PEDIDOS POR EL TALLER EXCLUSIVAMENTE
        // {0} Año
        // {1} Mes
        // {2} Cargo
        // {3} Trabajo
        public const string TOTALRENGLONESPEDIDOSTALLER =
            //            @"select COALESCE(COUNT(*),0) from dpedidoitem d, mpedidoitem m, PPEDIDO P, mpedidoTRANSFERENCIA mpo, MORDEN MO
            //            where d.mped_clasregi = m.mped_claseregi and d.mnit_Nit = m.mnit_nit and d.pped_codigo = m.pped_codigo and d.mped_numepedi = m.mped_numepedi
            //            and m.mped_claseregi = 'C' AND TPED_CODIGO IN ('T') AND M.PPED_CODIGO = P.PPED_CODIGO
            //			AND M.PPED_CODIGO = MPO.PPED_CODIGO AND M.MPED_NUMEPEDI = MPO.MPED_NUMERO
            //            and MPO.PDOC_CODIGO = MO.PDOC_CODIGO AND MPO.MORD_NUMEORDE = MO.MORD_NUMEORDE
            //			and year(m.mped_pedido) = {0} and month(m.mped_pedido) = {1} 
            //			AND MPO.TCAR_CARGO IN ({2})
            //			and mo.ttra_codigo in ({3}) ; ";
            @"select COALESCE(COUNT(*),0) from dpedidoitem d, mpedidoitem m, PPEDIDO P, mpedidoTRANSFERENCIA mpo, MORDEN MO, mcatalogovehiculo mc, pcatalogovehiculo pc
            where d.mped_clasregi = m.mped_claseregi and d.mnit_Nit = m.mnit_nit and d.pped_codigo = m.pped_codigo and d.mped_numepedi = m.mped_numepedi
            and m.mped_claseregi = 'C' AND TPED_CODIGO IN ('T') AND M.PPED_CODIGO = P.PPED_CODIGO
			AND M.PPED_CODIGO = MPO.PPED_CODIGO AND M.MPED_NUMEPEDI = MPO.MPED_NUMERO
            and MPO.PDOC_CODIGO = MO.PDOC_CODIGO AND MPO.MORD_NUMEORDE = MO.MORD_NUMEORDE
            and  mo.mcat_vin = mc.mcat_vin and  mc.pcat_codigo = pc.pcat_codigo
			and year(m.mped_pedido) = {0} and month(m.mped_pedido) = {1} 
			AND MPO.TCAR_CARGO IN ({2})
			and mo.ttra_codigo in ({3}) 
			and pc.pgam_codigo in ({4});";

        // TOTAL RENGLONES DE ITEMS DESPACHADOS AL TALLER EXCLUSIVAMENTE
        // {0}, Año
        // {1}, Mes
        public const string TOTALRENGLONESDESPACHADOSTALLER =
            @"select  COALESCE(COUNT(*),0) from dITEMS d, mordentransferencia mot, morden mo, mcatalogovehiculo mc, pcatalogovehiculo pc
            where TMOV_TIPOMOVI IN (80) ANd PANO_ANO = {0} and PMES_MES  = {1} and pgam_codigo in ({2})
			and d.pdoc_codigo = mot.pdoc_factura and d.dite_numedocu = mot.mfac_numero
			and mot.pdoc_codigo = mo.pdoc_codigo and mot.mord_numeorde = mo.mord_numeorde
			and mo.mcat_vin = mc.mcat_vin
			and mc.pcat_codigo = pc.pcat_codigo ;; ";


        // NUMERO DE RENGLONES DE MANTENIMIENTO ENTREGADOS AL TALLER IMMEDIATAMENTE			
        // {0}, Año
        // {1}, Mes
        public const string TOTALRENGLONESDESPACHADOSTALLERINMEDIATO =
            //            @"SELECT COUNT(*)
            //            FROM DITEMS DI, MORDENTRANSFERENCIA MOT, MFACTURACLIENTE MF, MORDEN MO, MPEDIDOITEM MP
            //            WHERE MF.PDOC_CODIGO = MOT.PDOC_FACTURA AND MF.MFAC_NUMEDOCU = MOT.MFAC_NUMERO
            //            AND YEAR (MF.MFAC_FACTURA) = {0}        AND MONTH(MF.MFAC_FACTURA) = {1}
            //            AND MF.PDOC_CODIGO = DI.PDOC_CODIGO     AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            //            AND MOT.PDOC_CODIGO = MO.PDOC_CODIGO    AND MOT.MORD_NUMEORDE = MO.MORD_NUMEORDE  AND MO.TTRA_CODIGO = 'M'
            //            AND DI.DITE_PREFDOCUREFE = MP.PPED_CODIGO AND DI.DITE_NUMEDOCUREFE = MP.MPED_NUMEPEDI
            //            AND MP.MPED_CLASEREGI = 'C'
            //            AND MP.MPED_PEDIDO = DI.DITE_FECHDOCU; ";	

            @"SELECT COUNT(*)
            FROM DITEMS DI, MORDENTRANSFERENCIA MOT, MFACTURACLIENTE MF, MORDEN MO, MPEDIDOITEM MP,mcatalogovehiculo mc, pcatalogovehiculo pc
            WHERE MF.PDOC_CODIGO = MOT.PDOC_FACTURA AND MF.MFAC_NUMEDOCU = MOT.MFAC_NUMERO
            AND YEAR (MF.MFAC_FACTURA) = {0}        AND MONTH(MF.MFAC_FACTURA) = {1}
            and  pc.pgam_codigo in ({2})	 and  mo.mcat_vin = mc.mcat_vin
            and  mc.pcat_codigo = pc.pcat_codigo
            AND MF.PDOC_CODIGO = DI.PDOC_CODIGO     AND MF.MFAC_NUMEDOCU = DI.DITE_NUMEDOCU
            AND MOT.PDOC_CODIGO = MO.PDOC_CODIGO    AND MOT.MORD_NUMEORDE = MO.MORD_NUMEORDE  AND MO.TTRA_CODIGO = 'M'
            AND DI.DITE_PREFDOCUREFE = MP.PPED_CODIGO AND DI.DITE_NUMEDOCUREFE = MP.MPED_NUMEPEDI
            AND MP.MPED_CLASEREGI = 'C'
            AND MP.MPED_PEDIDO = DI.DITE_FECHDOCU;";


        // TOTAL ENTRADAS AL TALLER  - ORDENES CREADAS
        // {0}, Año
        // {1}, Mes
        // {2}, Cargo de la Orden
        // {3}, Trabajo de la orden
        public const string TOTALENTRADASALTALLER =
            @"SELECT COALESCE(COUNT(*),0) AS ENTRADAS_TALLER
            FROM  MORDEN MO, mcatalogovehiculo mc, pcatalogovehiculo pc 
			WHERE YEAR(MO.MORD_ENTRADA) = {0} AND MONTH(MO.MORD_ENTRADA) = {1} and mo.mcat_vin = mc.mcat_vin and mc.pcat_codigo = pc.pcat_codigo
			 AND  MO.TCAR_CARGO IN ({2})
             AND  MO.TTRA_CODIGO IN ({3}) 
             and  pc.pgam_codigo in ({4});";

        // TOTAL ENTRADAS AL TALLER  - ORDENES CREADAS
        // {0}, Año
        // {1}, Mes
        // {2}, Cargo de la Orden
        // {3}, Trabajo de la orden
        // {4}, Gama de vehiculo
        public const string TOTALENTRADASALTALLERFACTURADAS =
            //         @"SELECT COALESCE(COUNT(*),0) AS ENTRADAS_TALLER
            //         FROM  MORDEN MO, mcatalogovehiculo mc, pcatalogovehiculo pc, MFACTURACLIENTETALLER MF
            //WHERE YEAR(MO.MORD_ENTRADA) = {0} AND MONTH(MO.MORD_ENTRADA) = {1}
            // AND  MO.PDOC_CODIGO = MF.PDOC_PREFORDETRAB AND MO.MORD_NUMEORDE = MF.MORD_NUMEORDE
            // AND  MO.TCAR_CARGO = MF.TCAR_CARGO
            //          and  mo.mcat_vin = mc.mcat_vin
            //          and  mc.pcat_codigo = pc.pcat_codigo
            // AND  MO.TCAR_CARGO IN ({2})
            //          AND  MO.TTRA_CODIGO IN ({3}) 
            //          and  pc.pgam_codigo in ({4}); ";
            @"SELECT COALESCE(COUNT(*),0) AS ENTRADAS_TALLER
                            FROM dordenoperacion dp,
                                 morden mo,
                                 MFACTURACLIENTETALLER MFT, 
                                 mcatalogovehiculo mc,
                                 pcatalogovehiculo pc
                            WHERE mo.pdoc_codigo = dp.pdoc_codigo
                            AND   mo.mord_numeorde = dp.mord_numeorde
                            AND   Mo.PDOC_CODIGO = MFT.PDOC_prefordetrab
                            AND   Mo.Mord_NUMEorde = MFt.mord_numeorde
                            AND  MO.TCAR_CARGO = MFT.TCAR_CARGO
                            and  mo.mcat_vin = mc.mcat_vin
                            and  mc.pcat_codigo = pc.pcat_codigo
                            AND   YEAR(MFt.MFAC_FeChcrea) = {0}
                            AND MONTH(mft.MFAC_Fechcrea) ={1}
                            and MFT.TCAR_CARGO in ({2})
                            AND MO.TTRA_CODIGO IN ({3})
                            and pc.pgam_codigo in ({4});";
        // TOTAL ENTRADAS AL TALLER  - ORDENES CREADAS
        // {0}, Año
        // {1}, Mes
        // {2}, Día
        // {3}, Cargo de la Orden
        // {4}, Trabajo de la orden
        public const string TOTALENTRADASALTALLERDIA =
            @"SELECT COALESCE(COUNT(*),0) AS ENTRADAS_TALLER
            FROM  MORDEN MO 
			WHERE YEAR(MO.MORD_ENTRADA) = {0} AND MONTH(MO.MORD_ENTRADA) = {1} AND DAY(MO.MORD_ENTRADA) = {2}
			 AND  MO.TCAR_CARGO IN ({3})
             AND  MO.TTRA_CODIGO IN ({4}) ; ";

        // TOTAL ENTRADAS AL TALLER  - ORDENES CREADAS
        // {0}, Año
        // {1}, Mes
        // {2}, Día
        // {3}, Cargo de la Orden
        // {4}, Trabajo de la orden
        public const string TOTALENTRADASALTALLERFACTURADASDIA =
            @"SELECT COALESCE(COUNT(*),0) AS ENTRADAS_TALLER
            FROM  MORDEN MO, MFACTURACLIENTETALLER MF
			WHERE YEAR(MO.MORD_ENTRADA) = {0} AND MONTH(MO.MORD_ENTRADA) = {1} AND DAY(MO.MORD_ENTRADA) = {2}
			 AND  MO.PDOC_CODIGO = MF.PDOC_PREFORDETRAB AND MO.MORD_NUMEORDE = MF.MORD_NUMEORDE
			 AND  MO.TCAR_CARGO = MF.TCAR_CARGO
			 AND  MO.TCAR_CARGO IN ({3})
             AND  MO.TTRA_CODIGO IN ({4}) ; ";

        // TOTAL ORDENES DE TALLER ESTADO CRITICO DE REPUESTOS = PARALIZADAS POR REPUESTOS
        // {0}, Año
        // {1}, Mes
        public const string TOTALORDENESCRITICOREPUESTOS =
            @"SELECT COALESCE(COUNT(*),0) FROM (
                SELECT DISTINCT M.PDOC_CODIGO||M.MORD_NUMEORDE
                FROM  MORDEN M, DORDENOPERACION D 
			    WHERE YEAR(M.MORD_ENTRADA) = {0} AND MONTH(M.MORD_ENTRADA) = {1}
			    AND  M.PDOC_CODIGO = D.PDOC_CODIGO AND M.MORD_NUMEORDE = D.MORD_NUMEORDE
			    AND  M.TEST_ESTADO IN ('A','B')
			    AND  D.TEST_ESTADO = 'R'); "; 

        // TOTAL HORAS FACTURAS TALLER
        // {0}, Año
        // {1}, Mes
        public const string TOTALHORASFACTURADASTALLER =
            //            @"select COALESCE(sum(dord_tiemliqu),0)
            //            from dordenoperacion dp, morden mo, MFACTURACLIENTETALLER MFT
            //            where mo.pdoc_codigo = dp.pdoc_codigo and mo.mord_numeorde = dp.mord_numeorde 
            //            AND  Mo.PDOC_CODIGO = MFT.PDOC_prefordetrab AND Mo.Mord_NUMEorde = MFt.mord_numeorde 
            //            AND YEAR(MFt.MFAC_FeChcrea) = {0} AND MONTH(mft.MFAC_Fechcrea) = {1} ; ";
                            @"SELECT COALESCE(sum(dord_tiemliqu),0)
                            FROM dordenoperacion dp,
                                 morden mo,
                                 MFACTURACLIENTETALLER MFT, 
                                 mcatalogovehiculo mc, 
                                 pcatalogovehiculo pc
                            WHERE mo.pdoc_codigo = dp.pdoc_codigo
                            AND   mo.mord_numeorde = dp.mord_numeorde
                            AND   Mo.PDOC_CODIGO = MFT.PDOC_prefordetrab
                            AND   Mo.Mord_NUMEorde = MFt.mord_numeorde
                            and   mo.mcat_vin = mc.mcat_vin
                            and   mc.pcat_codigo = pc.pcat_codigo
                            AND   YEAR(MFt.MFAC_FeChcrea) = {0}
                            AND   MONTH(mft.MFAC_Fechcrea) = {1}
                            and   pc.pgam_codigo in ({2})
							and   mft.TCAR_CARGO in ({3})";
 
        // TOTAL HORAS PRODUCTIVAS TALLER = LAS MARCACIONES DE CAMBIO DE ESTADO POR LOS TECNICOS
        // {0}, Año
        // {1}, Mes
        // {2}, Gama de vehiculo
        public const string TOTALHORASPRODUCTIVASTALLER = 
            @"select  COALESCE(sum((df*1440+hf*60+mf) - (di*1440+hi*60+mi)) / 60,0) 
                from (
                select  te2.test_nombre as inicial,' y ', te1.test_nombre as final, 
                        DAYS(destoper_hora) as df, substr(destoper_hora,12,2) as hf, substr(destoper_hora,15,2) as mf, 
                        DAYS(destoper_horaante) as di, substr(destoper_horaante,12,2) as hi, substr(destoper_horaante,15,2) as mi 
                from destadisticaoperacion DE, testadooperacion te1, testadooperacion te2 
                where YEAR(destoper_hora) = {0} AND MONTH(destoper_hora) = {1} and de.test_estado = te1.test_estaoper and de.test_estaante = te2.test_estaoper
                and de.test_estado <> de.test_estaante AND DE.TEST_ESTAANTE <> 'S' ); ";
 
        // TOTAL VENTAS TALLER por CARGO Y TRABAJO FACTURADOS
        // {0}, Año
        // {1}, Mes
        // {2}, Cargo
        // {3}, tipo de trabajo

        public const string TOTALVENTASMOBRATALLER =
            //            @"SELECT COALESCE(ROUND(SUM(TOTAL),0),0) FROM (
            //                SELECT CASE WHEN DP.TCAR_CARGO IN ('C','S') THEN sum(dp.dord_valooper*0.862068) ELSE sum(dp.dord_valooper) END as total
            //                 FROM  MORDEN MO, DORDENOPERACION DP, mfacturaclientetaller mft, mfacturacliente mf 
            //			    WHERE YEAR(Mf.Mfac_factura) = {0} AND MONTH(Mf.Mfac_factura) = {1}
            //			     AND  MO.PDOC_CODIGO = DP.PDOC_CODIGO AND MO.MORD_NUMEORDE = DP.MORD_NUMEORDE
            //			     AND  MO.PDOC_CODIGO = mft.PDOC_prefordetrab AND MO.MORD_NUMEORDE = mft.MORD_NUMEORDE
            //			     and  mft.pdoc_codigo = mf.pdoc_codigo and mft.mfac_numedocu = mf.mfac_numedocu
            //			     AND  DP.TCAR_CARGO = MFT.TCAR_CARGO
            //			     AND  MFT.TCAR_CARGO IN ({2})
            //			     AND  MO.TTRA_CODIGO IN ({3})
            //		        GROUP BY DP.TCAR_CARGO ); "; 
            @"SELECT COALESCE(ROUND(SUM(TOTAL),0),0) FROM (
                SELECT CASE WHEN DP.TCAR_CARGO IN ('C','S') THEN sum(dp.dord_valooper*0.862068) ELSE sum(dp.dord_valooper) END as total
                 FROM  MORDEN MO, DORDENOPERACION DP, mfacturaclientetaller mft, mfacturacliente mf,  mcatalogovehiculo mc, pcatalogovehiculo pc 
			    WHERE YEAR(Mf.Mfac_factura) = {0} AND MONTH(Mf.Mfac_factura) = {1}
			     AND  MO.PDOC_CODIGO = DP.PDOC_CODIGO AND MO.MORD_NUMEORDE = DP.MORD_NUMEORDE
			     AND  MO.PDOC_CODIGO = mft.PDOC_prefordetrab AND MO.MORD_NUMEORDE = mft.MORD_NUMEORDE
			     and  mft.pdoc_codigo = mf.pdoc_codigo and mft.mfac_numedocu = mf.mfac_numedocu
			     and  mo.mcat_vin = mc.mcat_vin and  mc.pcat_codigo = pc.pcat_codigo
			     AND  DP.TCAR_CARGO = MFT.TCAR_CARGO
			     AND  MFT.TCAR_CARGO IN ({2})
			     AND  MO.TTRA_CODIGO IN ({3})
			     and  pc.pgam_codigo in ({4})
		        GROUP BY DP.TCAR_CARGO );";
    
        // TOTAL COSTO DE VENTA DE MANO DE OBRA TALLER
        // {0}, Año
        // {1}, Mes
        // {4}, Gama vehiculo
        public const string COSTOVENTAMOBRATALLER =
            @"SELECT COALESCE(ROUND(SUM(TOTAL_COSTO),0),0) FROM (
                SELECT CASE WHEN DT.TCAR_CARGO IN ('C','S') 
	   		            THEN SUM((DT.DORD_VALOOPER * 0.862069 * COALESCE(PV.PVEN_PORCCOMI,0) * 0.01)
                            +(DT.DORD_TIEMLIQU * COALESCE(PV.PVEN_VALOUNIDVENT,0)))
				            + COALESCE(ME.MEMP_SUELACTU,0)
			            ELSE SUM((DT.DORD_VALOOPER * COALESCE(PV.PVEN_PORCCOMI,0)*0.01)
                            +(DT.DORD_TIEMLIQU * COALESCE(PV.PVEN_VALOUNIDVENT,0)))
				            + COALESCE(ME.MEMP_SUELACTU,0)
			           END  AS TOTAL_COSTO
                FROM MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO, DORDENOPERACION DT, PVENDEDOR PV
			    LEFT JOIN MEMPLEADO ME ON PV.MEMP_CODIEMP = ME.MEMP_CODIEMPL
                WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU 
			     AND YEAR(MF.MFAC_FACTURA) = {0} AND MONTH(MFAC_FACTURA) = {1}
                 AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
                 AND  MO.PDOC_CODIGO = DT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = DT.MORD_NUMEORDE
			     AND  DT.PVEN_CODIGO = PV.PVEN_CODIGO 
			    GROUP BY DT.TCAR_CARGO, ME.MEMP_SUELACTU);";        

        // inventario de repuestos en proceso en el taller	  
        // {0}, Almacén = Sede
        // {1}, Fecha de Corte
        public const string INVENTARIOENPROCESOTALLER =
            //            @"SELECT SUM(TOTAL_REPTOS_PROCESO) AS TOTAL_REPTOS_PROCESO FROM (
            //            SELECT  MO.PDOC_CODIGO, MO.MORD_NUMEORDE, MOT.TCAR_CARGO, MFT.TCAR_CARGO, MO.MORD_ENTRADA, 
            //	                CAST(COALESCE(MFAC_FECHCREA,'2099-12-31') AS CHAR(10)) AS FECHA_FACTURA, 
            //                    COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS_PROCESO
            //            FROM MORDEN MO,MORDENTRANSFERENCIA MOT 
            //				 LEFT JOIN MFACTURACLIENTETALLER MFT ON MFT.PDOC_PREFORDETRAB = MOT.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            //				    AND MOT.TCAR_CARGO = MFT.TCAR_CARGO,
            //			     MITEMS MI, DITEMS DI
            //                 	LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            //            WHERE MO.TEST_ESTADO IN ('A','B','F','E')
            //            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            //            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
            //            AND  DI.MITE_CODIGO = MI.MITE_CODIGO
            //            AND  MO.PALM_ALMACEN IN ({0})   
            //            GROUP BY MO.PDOC_CODIGO, MO.MORD_NUMEORDE, MOT.TCAR_CARGO, MFT.TCAR_CARGO, MO.MORD_ENTRADA, MFAC_FECHCREA 
            //            ) AS A
            //            WHERE MORD_ENTRADA <= '{1}' AND FECHA_FACTURA > '{1}' ; ";

            @"SELECT SUM(TOTAL_REPTOS_PROCESO) AS TOTAL_REPTOS_PROCESO FROM (
            SELECT  MO.PDOC_CODIGO, MO.MORD_NUMEORDE, MOT.TCAR_CARGO, MFT.TCAR_CARGO, MO.MORD_ENTRADA, 
	                CAST(COALESCE(MFAC_FECHCREA,'{1}') AS CHAR(10)) AS FECHA_FACTURA, 
                    COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01))),0) AS TOTAL_REPTOS_PROCESO
            FROM  mcatalogovehiculo mc, pcatalogovehiculo pc, MORDEN MO,MORDENTRANSFERENCIA MOT 
				 LEFT JOIN MFACTURACLIENTETALLER MFT ON MFT.PDOC_PREFORDETRAB = MOT.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
				    AND MOT.TCAR_CARGO = MFT.TCAR_CARGO,
			     MITEMS MI, DITEMS DI
                 	LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            WHERE MO.TEST_ESTADO IN ('A','B','F','E')
            AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
            AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
            AND  DI.MITE_CODIGO = MI.MITE_CODIGO
            and  mo.mcat_vin = mc.mcat_vin
            and  mc.pcat_codigo = pc.pcat_codigo
            AND  MO.PALM_ALMACEN IN ({0})   
            and  pc.pgam_codigo in ({2})
            GROUP BY MO.PDOC_CODIGO, MO.MORD_NUMEORDE, MOT.TCAR_CARGO, MFT.TCAR_CARGO, MO.MORD_ENTRADA, MFAC_FECHCREA 
            ) AS A
            WHERE MORD_ENTRADA <= '{1}' AND FECHA_FACTURA > '{1}' ;";

        // valor GARANTIAS APROBADAS Y FACTURADAS A LA CASA MATRIZ
        // {0}  Año
        // {1}  Mes
        // {2}  Gama Del Vehiculo
        public const string GARANTIASFACTURADAS =
         @"select coalesce(sum(Mfac_valoFACT),0) 
		from mfacturacliente mf, dfacturagarantias	dg, mfacturaclientetaller mft, morden mo, mcatalogovehiculo mc, pcatalogovehiculo pc
		where mf.pdoc_codigo = dg.pdoc_codigo and mf.mfac_numedocu = dg.mfac_numedocu
		and year(mfac_factura) = {0} and month(mfac_factura) = {1} and pgam_codigo in ({2})
		and dg.pdoc_codiref = mft.pdoc_codigo and dg.mfac_numeref = mft.mfac_numedocu
		and mft.pdoc_prefordetrab = mo.pdoc_codigo and mft.mord_numeorde = mo.mord_numeorde
		and mo.mcat_vin = mc.mcat_vin
		and mc.pcat_codigo = pc.pcat_codigo ; ";

        // valor GARANTIAS liquidas SIN FACTURAR A LA CASA MATRIZ
        // {0}  Año
        // {1}  Mes
        public const string GARANTIASSINFACTURAR =
         @"select coalesce(sum(Mfac_valoFACT),0) 
		from mfacturacliente mf, mfacturaclientetaller mft, morden mo, mcatalogovehiculo mc, pcatalogovehiculo pc
		where mf.pdoc_codigo = mft.pdoc_codigo and mf.mfac_numedocu = mft.mfac_numedocu
		and year(mfac_factura) = {0} and month(mfac_factura) = {1} and pgam_codigo in ({2})
		and mft.tcar_cargo = 'G' AND MF.PDOC_CODIGO || MF.MFAC_NUMEDOCU NOT IN (SELECT DISTINCT PDOC_CODIGO||MFAC_NUMEDOCU FROM DFACTURAGARANTIAS)
		and mft.pdoc_prefordetrab = mo.pdoc_codigo and mft.mord_numeorde = mo.mord_numeorde
		and mo.mcat_vin = mc.mcat_vin
		and mc.pcat_codigo = pc.pcat_codigo ; ";

        // valor REPUESTOS facturados por un taller en un dia
        // {0}  Fecha
        // {1}  Taller
        public const string VENTAREPUESTOSDIATALLER =
        @"SELECT COALESCE(ROUND(SUM(TOTAL_REPTOS),0),0) FROM (
            SELECT COALESCE(SUM((di.DITE_CANTIDAD - coalesce(did.dite_cantidad,0))*di.DITE_VALOUNIT*(1 - (di.DITE_PORCDESC*0.01)))*MFAC_FACTORDEDUCIBLE,0) AS TOTAL_REPTOS
             FROM MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO, MORDENTRANSFERENCIA MOT, MITEMS MI, DITEMS DI
             LEFT JOIN DITEMS DID ON DI.PDOC_CODIGO = DID.DITE_PREFDOCUREFE AND DI.DITE_NUMEDOCU = DID.DITE_NUMEDOCUREFE AND DI.MITE_CODIGO = DID.MITE_CODIGO AND DID.TMOV_TIPOMOVI = 81
            WHERE MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU AND MF.MFAC_FACTURA = '{0}' AND MF.PALM_ALMACEN = '{1}'
			 AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE 
             AND  MO.PDOC_CODIGO = MOT.PDOC_CODIGO  AND MO.MORD_NUMEORDE = MOT.MORD_NUMEORDE 
             AND  MOT.PDOC_FACTURA = DI.PDOC_CODIGO AND MOT.MFAC_NUMERO = DI.DITE_NUMEDOCU
             AND  DI.MITE_CODIGO = MI.MITE_CODIGO   AND MFT.TCAR_CARGO = MOT.TCAR_CARGO
		GROUP BY MFAC_FACTORDEDUCIBLE) as a; ";

        // valor MANO DE OBRA facturada por un taller en un dia
        // {0}  Fecha
        // {1}  Taller
        public const string VENTAMOBRADIATALLER =
            @"SELECT COALESCE(ROUND(SUM(TOTAL),0),0) FROM (
			    SELECT CASE WHEN DP.TCAR_CARGO IN ('C','S') THEN sum(dp.dord_valooper*0.862068)*MFAC_FACTORDEDUCIBlE ELSE sum(dp.dord_valooper)*MFAC_FACTORDEDUCIBlE END as total
                 FROM  MORDEN MO, DORDENOPERACION DP, mfacturaclientetaller mft, mfacturacliente mf 
			    WHERE Mf.Mfac_factura = '{0}' AND MF.PALM_ALMACEN = '{1}'
			     AND  MO.PDOC_CODIGO = DP.PDOC_CODIGO AND MO.MORD_NUMEORDE = DP.MORD_NUMEORDE
			     AND  MO.PDOC_CODIGO = mft.PDOC_prefordetrab AND MO.MORD_NUMEORDE = mft.MORD_NUMEORDE
			     and  mft.pdoc_codigo = mf.pdoc_codigo and mft.mfac_numedocu = mf.mfac_numedocu
			     AND  DP.TCAR_CARGO = MFT.TCAR_CARGO
			    GROUP BY DP.TCAR_CARGO, MFAC_FACTORDEDUCIBLE ); ";

        // valor DEDUCIBLES facturados por un taller en un dia
        // {0}  Fecha
        // {1}  Taller
        public const string DEDUCIBLESDIATALLER =
            @"SELECT COALESCE(sum(mfac_valofact), 0) FROM MFACTURACLIENTE MF, MFACTURACLIENTETALLER MFT, MORDEN MO 
				WHERE MFAC_FACTURA = '{0}' AND MF.PALM_ALMACEN = '{1}' AND MF.PDOC_CODIGO = MFT.PDOC_CODIGO AND MF.MFAC_NUMEDOCU = MFT.MFAC_NUMEDOCU
				  AND  MFT.PDOC_PREFORDETRAB = MO.PDOC_CODIGO AND MFT.MORD_NUMEORDE = MO.MORD_NUMEORDE AND MO.MNIT_NIT = MF.MNIT_NIT; ";

        // NUMERO DE ENTRADAS al TALLER por dia por taller  
        // EntraTaller...Params:   
        // {0}, Fecha aaaa-mm-dd  
        // {1}, sede o taller  
        public const string NUMEENTRADASDIATALLER =
            @"SELECT COALESCE(COUNT(*),0) AS ENTRADAS_TALLER
                FROM  MORDEN MO WHERE MO.MORD_ENTRADA = '{0}' AND MO.PALM_ALMACEN = '{1}';";

        public const string INVENTARIOCLASIFICADOABCD =
            @"SELECT SUM (MSAL_CANTACTUAL * MSAL_COSTPROM)FROM MSALDOITEM MS, MITEMS M
                WHERE MS.MITE_CODIGO = M.MITE_CODIGO 
                AND PANO_ANO = '{0}'
                AND SUBSTR(MITE_CLASABC ,1,1) IN ({1});";

        public const string VALORMANODEOBRAPAGADOCGIS =
          @"SELECT ROUND( SUM (D.DORD_VALOOPER) * 0.862068, 0) FROM
            MORDEN M,
            DORDENOPERACION D, 
            MFACTURACLIENTETALLER MFT
            WHERE M.PDOC_CODIGO = D.PDOC_CODIGO 
            AND M.MORD_NUMEORDE = D.MORD_NUMEORDE 
            AND D.TCAR_CARGO = MFT.TCAR_CARGO
            AND D.TCAR_CARGO = '{0}'
            AND MFT.PDOC_PREFORDETRAB = M.PDOC_CODIGO
            AND MFT.MORD_NUMEORDE = M.MORD_NUMEORDE
            AND YEAR (MFT.MFAC_FECHCREA) = {1} AND MONTH (MFT.MFAC_FECHCREA) = {2};";

    }

}