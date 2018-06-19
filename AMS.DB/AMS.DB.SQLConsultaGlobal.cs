using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.DB
{
    public class SQLConsultaGlobal
    {
        public static string Buscar(string param)
        {
            string[] arrParams = param.Split('_');
            string resp = "";

            switch (arrParams[0])
            {
                case "NITS":
                    switch (arrParams[1])
                    {
                        case "PROVEEDOR":
                            resp = NITS.PROVEEDOR;
                            break;
                        case "TALLER":
                            resp = NITS.TALLER;
                            break;
                        case "INTERNO":
                            resp = NITS.INTERNO;
                            break;
                        case "CLIENTE":
                            resp = NITS.CLIENTE;
                            break;
                        case "CLIENTELISTAEMPAQUE":
                            resp = NITS.CLIENTELISTAEMPAQUE;
                            break;
                        case "NITENTREGAVEH":
                            resp = NITS.NITENTREGAVEH;
                            break;
                        case "BENEFICIARIO":
                            resp = NITS.BENEFICIARIO;
                            break;
                        case "CEDULABENEFICIARIO":
                            resp = NITS.CEDULABENEFICIARIO;
                            break;
                    }
                    break;

                case "VEHICULOS":
                    switch (arrParams[1])
                    {
                        case "CATALOGOSNUEVOS":
                            resp = VEHICULOS.CATALOGOSNUEVOS;
                            break;
                        case "CATALOGOSUSADOS":
                            resp = VEHICULOS.CATALOGOSUSADOS;
                            break;
                        case "ITEMSVENTAVEHICULOS":
                            resp = VEHICULOS.ITEMSVENTAVEHICULOS;
                            break;
                    }
                    break;

                case "INVENTARIOS":
                    switch (arrParams[1])
                    {
                        case "ITEMSCLIENTE":
                            resp = INVENTARIOS.ITEMSCLIENTE(arrParams[2], arrParams[3], arrParams[4], arrParams[5]);
                            break;
                        case "ITEMSALDO":
                            resp = INVENTARIOS.ITEMSALDO(arrParams[2], arrParams[3]);
                            break;
                    }
                    break;

            }

            return resp;
        }
    }

    public class NITS
    {
        // Todos los cliente menos los de emitir transferencias 
        public const string PROVEEDOR =
            @"SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM MPROVEEDOR MP, Vmnit NIT 
               WHERE NIT.mnit_nit = MP.MNIT_NIT 
                 AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE";

        // solo cliente para emitir transferencias 
        public const string TALLER =
            @"SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM Vmnit NIT 
                WHERE NIT.mnit_nit IN (SELECT PNI.pnital_nittaller FROM pnittaller PNI) 
                  AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE";

        // solo cliente para emitir CONSUMOS INTERNOS 
        public const string INTERNO =
            @"SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM Vmnit NIT, dbxschema.MinternoCCOSTO PNIT
                WHERE NIT.mnit_nit=PNIT.Mnit_nit order by NOMBRE";

        // Todos los cliente menos los de emitir transferencias 
        public const string CLIENTE =
            @"SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM Vmnit NIT 
               WHERE NIT.mnit_nit NOT IN (SELECT PNI.pnital_nittaller FROM pnittaller PNI) 
                 AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE";

        // solo cliente que tiene lista de empaque de inventario para facturar 
        public const string CLIENTELISTAEMPAQUE =
            @"SELECT NIT.mnit_nit AS NIT, Nombre AS NOMBRE FROM mlistaempaque ML, Vmnit NIT WHERE NIT.mnit_nit=ML.mnit_nit 
                 AND NIT.mnit_nit NOT IN (SELECT CEM.mnit_nit FROM cempresa CEM) order by NOMBRE";

        // solo nit autorizados para entregar los vehiculos (proveedor del vehiculo o un empleado de la empresa)
        public const string NITENTREGAVEH =
             @"SELECT mnit_nit as NIT, mnit_apellidos concat ' ' concat coalesce(mnit_apellido2, '') concat ' ' concat mnit_nombres concat ' ' concat coalesce(mnit_nombre2,'') as Nombre 
             FROM mnit where tvig_VIGENCIA = 'V' AND (MNIT_nIT IN (SELECT MNIT_NIT FROM PVENDEDOR WHERE PVEN_VIGENCIA = 'V')
             OR MNIT_NIT IN (SELECT MNIT_nIT FROM MEMPLEADO WHERE TEST_ESTADO = 1))      
             order by 2;";

        // solo Beneficiarios de FUNDACIONES  
        public const string BENEFICIARIO =
            @"SELECT M.MBEN_ID AS CODIGO, VM.NOMBRE AS NOMBRE, M.MNIT_NIT AS IDENTIFICACION, PGRA_DESCRIPCION AS ESCOLARIDAD,PENT_DESCRIPCION AS ENTIDADP,  VM2.NOMBRE AS RESPONSABLE,M.MNIT_MNITRESPONSABLE AS NIT 
              FROM MBENEFICIARIO M, VMNIT VM, VMNIT VM2, PGRADOESCOLARIDAD P, PENTIDAD PE 
              WHERE M.MNIT_NIT = VM.MNIT_NIT AND P.PGRA_CODIGO = M.PGRA_CODIGO AND PE.PENT_CODIGO = M.PENT_CODIGO AND M.MNIT_NIT = VM2.MNIT_NIT;";

        // solo Beneficiarios de FUNDACIONES  
        public const string CEDULABENEFICIARIO =
           @"SELECT  M.MNIT_NIT AS IDENTIFICACION, VM.NOMBRE AS NOMBRE, M.MBEN_ID AS CODIGO,  PGRA_DESCRIPCION AS ESCOLARIDAD,PENT_DESCRIPCION AS ENTIDADP,  VM2.NOMBRE AS RESPONSABLE,M.MNIT_MNITRESPONSABLE AS NIT 
              FROM MBENEFICIARIO M, VMNIT VM, VMNIT VM2, PGRADOESCOLARIDAD P, PENTIDAD PE 
              WHERE M.MNIT_NIT = VM.MNIT_NIT AND P.PGRA_CODIGO = M.PGRA_CODIGO AND PE.PENT_CODIGO = M.PENT_CODIGO AND M.MNIT_NIT = VM2.MNIT_NIT;";

    }

    public class VEHICULOS
    {
        // Todos los catalogos de vehiculos NUEVOS registrados en lista de precios para la venta 
        public const string CATALOGOSNUEVOS =
            @"SELECT DISTINCT pc.Pcat_codigo, pmar_nombre concat '  -  ' concat pcat_descripcion concat '  -  ' concat pFAM_NOMBRE concat '  -  ' concat pc.pcat_codigo AS descripcion   
                           FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, dbxschema.PPRECIOVEHICULO pp, DBXSCHEMA.PFAMILIACATALOGO PF  
                            WHERE pc.pcat_codigo = pp.pcat_CODIGO AND PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PC.PFAM_FAMILIA = PF.PFAM_FAMILIA
                             ORDER BY descripcion;";

        // Todos los catalogos de vehiculos registrados en el sistema
        public const string CATALOGOSUSADOS =
            @"SELECT DISTINCT pc.Pcat_codigo, COALESCE(pmar_nombre,'') concat '  -  ' concat COALESCE(pcat_descripcion,'') concat '  -  ' concat COALESCE(pFAM_NOMBRE,'') concat '  -  ' concat pc.pcat_codigo AS descripcion   
                           FROM dbxschema.pcatalogovehiculo pc, dbxschema.pmarca pm, DBXSCHEMA.PFAMILIACATALOGO PF  
                            WHERE PC.PMAR_CODIGO = PM.PMAR_CODIGO AND PC.PFAM_FAMILIA = PF.PFAM_FAMILIA
                             ORDER BY descripcion;";

        // Todos los ITEMS DE ELEMENTOS en venta de VEHICULOS
        public const string ITEMSVENTAVEHICULOS =
            @"SELECT PITE_CODIGO, PITE_NOMBRE, CASE WHEN PITE_CARGO = 'C' THEN PITE_COSTO ELSE 0 END AS COSTO, PTRA_CODIGO, CEMP_PORCIVA 
                FROM PITEMVENTAVEHICULO, CEMPRESA;";


    }

    public class INVENTARIOS
    {
        internal static string ITEMSCLIENTE(string ano, string tipoLinea, string almacen, string listaPrecios)
        {
            tipoLinea    = tipoLinea.Trim();
            almacen      = almacen.Trim();
            listaPrecios = listaPrecios.Trim();
            string ITEMSCLIENTE1 =
            @"SELECT DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo,PLIN.plin_tipo) as REFERENCIA,  
        MIT.mite_nombre as NOMBRE_DESCRIPCION,  coalesce(MPR.mpre_precio,0) AS PRECIO, PDES_CODIGO AS LETRA_DSCTO,   
        DBXSCHEMA.CANTACTL(MIT.mite_codigo,2018) AS SALDO_TOTAL, coalesce(MSAL_CANTACTUAL,0) SALDO_ALMACEN,  
        pgru_GRUPO AS APLICACION, pmar_nombre as MARCA, pFam_nombre as FAMILIA, PSUBFAM_CODIGO as SUBFAMILIA,  
        MIT.mite_refefabr AS STAMP, MIE.MITE_EQUIVALENTE AS ITEM_EQUIVALENTE, MIE.MITE_MARCAREFE AS EN_MARCA,  
        MSI.MSUS_CODORIGEN AS SUSTITUCION  
       FROM dbxschema.mitems MIT   
        LEFT JOIN dbxschema.mprecioitem       MPR ON MPR.mite_codigo = MIT.mite_codigo AND MPR.ppre_codigo = '" + listaPrecios + @"'
        LEFT JOIN dbxschema.msaldoitemalmacen MSI on MIT.mite_codigo = MSI.mite_codigo AND MSI.pano_ano = " + ano + @" AND MSI.palm_almacen= '" + almacen + @"'
        LEFT JOIN dbxschema.mitemsgrupo       MIG ON MIG.mite_codigo = MIT.mite_codigo  
        LEFT JOIN dbxschema.mitemEQUIVALENTE  MIE ON MIE.mite_codigo = MIT.mite_codigo  
        LEFT JOIN dbxschema.mSUSTITUCION      MSI ON MIT.mite_codigo = MSI.mSUS_codACTUAL  
        LEFT JOIN dbxschema.PFAMILIAitem      PFI ON MIT.pFAM_codigo = PFI.pFAM_codigo 
        LEFT JOIN dbxschema.pmarcaitem        PMI ON MIT.pmar_codigo = PMI.pmar_codigo,  
        dbxschema.plineaitem PLIN  
       WHERE PLIN.plin_tipo= '" + tipoLinea + @"'
        AND MIT.plin_codigo=PLIN.plin_codigo   
       ORDER By MIT.mite_codigo; ";
            return ITEMSCLIENTE1;
        }

        // SALDO DE INVENTARIOS SEGUN TIPO DE LINEA 
        internal static string ITEMSALDO(string ano, string tipoLinea)
        {
            tipoLinea = tipoLinea.Trim();
            string ITEMSCLIENTE1 =
            @"SELECT CASE WHEN PLIN_TIPO = 'GR' THEN MIT.mite_codigo ELSE DBXSCHEMA.EDITARREFERENCIAS(MIT.mite_codigo, PLIN.plin_tipo) END as Codigo, 
                                    MIT.mite_nombre as Nombre, COALESCE(MPR.Msal_costprom,0) as Costo, COALESCE(MPR.MSAL_CANTACTUAL,0) as Saldo  
                                    FROM dbxschema.mitems MIT  
                                      left JOIN dbxschema.msaldoitem MPR ON MPR.mite_codigo=MIT.mite_codigo and MPR.pano_ano = " + ano + @",
                                     dbxschema.plineaitem PLIN  
                                    WHERE MIT.plin_codigo=PLIN.plin_codigo AND PLIN.plin_tipo = '" + tipoLinea + @"'
                                    order by NOMBRE;";
            return ITEMSCLIENTE1;
        }
    }
}

