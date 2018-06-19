using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using AMS.DB;
using AMS.Tools;

namespace AMS.Documentos
{
    public class Retencion
    {
        private string nit, prefijoFac, pc_icaprov, Codica, tipoProceso, tipoSociedad, tipoRetenedor, tipoPersona, mensajes, ica, tipoNacionalidad, regimenIva;
        private int numeroFac;
        private double valorFactura, valorIVA, valorOperaciones, valorIVAOperaciones, valorica;
        private DataTable dtRepuestos;
        private ArrayList pedidos, tipoRetencion, valoresRetenidos, basesRetenidas, sqls;
        private string centavos = DBFunctions.SingleData("SELECT COALESCE(CEMP_LIQUDECI,'N') FROM CEMPRESA"); // ConfigurationManager.AppSettings["ManejoCentavos"];
        public static int numDecimales = 0;
        private bool esVenta;
        private string tipoRetenedorEmpresa;

        #region Propiedades

        public string TipoSociedad { set { tipoSociedad = value; } get { return tipoSociedad; } }
        public string Mensajes { set { mensajes = value; } get { return mensajes; } }
        public ArrayList Sqls { set { sqls = value; } get { return sqls; } }

        #endregion

        #region Constructores
        //Constructor por defecto
        public Retencion(string nit, bool esVenta)
        {

            this.Comunes(nit, esVenta);

            tipoRetencion = new ArrayList();
            valoresRetenidos = new ArrayList();
            basesRetenidas = new ArrayList();
            sqls = new ArrayList();
        }

        //Constructor para Compra/venta de Vehiculos, venta de Repuestos, ventas Servicios
        public Retencion(string nit, string prefFac, int numFac, double valFac, double valIVA, string proceso, bool esVenta)
        {
            prefijoFac = prefFac;
            numeroFac = numFac;
            valorFactura = valFac;
            valorIVA = valIVA;
            tipoProceso = proceso;

            this.Comunes(nit, esVenta);

            tipoRetencion = new ArrayList();
            valoresRetenidos = new ArrayList();
            basesRetenidas = new ArrayList();
            sqls = new ArrayList();
        }

        //Constructor para compra/venta de respuestos
        public Retencion(string nit, string prefFac, int numFac, DataTable dtRep, double valFac, double valIVA, string proceso, bool esVenta)
        {
            prefijoFac = prefFac;
            numeroFac = numFac;
            dtRepuestos = dtRep;
            valorFactura = valFac;
            valorIVA = valIVA;
            tipoProceso = proceso;

            this.Comunes(nit, esVenta);

            pedidos = new ArrayList();
            tipoRetencion = new ArrayList();
            valoresRetenidos = new ArrayList();
            basesRetenidas = new ArrayList();
            sqls = new ArrayList();
        }

        //Constructor para venta de servicios
        public Retencion(string nit, string prefFac, int numFac, DataTable dtRep, double valOper, double valOperIVA, double valFac, double valIVA, string proceso, bool esVenta)
        {
            prefijoFac = prefFac;
            numeroFac = numFac;
            dtRepuestos = dtRep;
            valorOperaciones = valOper;
            valorIVAOperaciones = valOperIVA;
            valorFactura = valFac;
            valorIVA = valIVA;
            tipoProceso = proceso;

            this.Comunes(nit, esVenta);

            pedidos = new ArrayList();
            tipoRetencion = new ArrayList();
            valoresRetenidos = new ArrayList();
            basesRetenidas = new ArrayList();
            sqls = new ArrayList();
        }

        #endregion

        #region Metodos
        private void Calcular_ReteFuente()
        {

            if (esVenta && tipoPersona == "N") return;
            if (tipoNacionalidad == "E") return;              // extranjero no paga impuestos
            if (regimenIva == "G" && !esVenta) return;        // gran contribuyente autoretenedor no se le causa impuestos

            // Solo las grandes contribuyentes NO autortenedores y los regimen comun causan retencion en la FUENTE EN VENTA
            if (esVenta && (tipoRetenedorEmpresa != "H" && tipoRetenedorEmpresa != "C")) return;

            if (!esVenta && tipoProceso == "V" && "U" == DBFunctions.SingleData("select TCLA_CODIGO from mvehiculo where pdoc_codiordepago = '" + prefijoFac + "' and mfac_numeordepago = " + numeroFac + " "))
                return; // en las compras, consignaciones y retomas de vehiculos USADOS NO SE CAUSA RETENCION porque esta se paga al momento del traspaso. NO HACE NADA PORQUE AUN NO HA GRABDO LA DB

            double valor = 0;
            DataSet ds = null;
            sqls.Clear();
            double valorX = 0, valorY = 0, valorOtros = 0, valorRteX = 0, valorRteY = 0, valorRteOtros = 0;
            DataRow[] tipoX = null, tipoY = null, otroTipo = null;

            if (esVenta)   // CAUSACION de retencion en VENTA
            {
                ds = new DataSet();
                double valorRte = 0;

                DBFunctions.Request(ds, IncludeSchema.NO,
                   @"SELECT pret_codigo,pret_porcendecl,pret_porcennodecl,pret_tope FROM pretencion 
                    WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='"+tipoProceso+"' AND tret_codigo='RF';");

                try {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (tipoProceso == "V")
                        {   // en usados el campo base de retencion es el valor de los fletes, ahi se graba el valor de la comision 
                            valorFactura = Convert.ToDouble(DBFunctions.SingleData(@"SELECT CASE WHEN TCLA_CODIGO = 'U' THEN MF.MFAC_VALOFLET ELSE MFAC_VALOFACT END 
                                       FROM MVEHICULO MV, MFACTURAPEDIDOVEHICULO MFV,MFACTURACLIENTE MF 
                                       WHERE MV.MVEH_INVENTARIO = MFV.MVEH_INVENTARIO 
                                       AND MFV.PDOC_CODIGO = MF.PDOC_CODIGO AND MFV.MFAC_NUMEDOCU = MF.MFAC_NUMEDOCU 
                                       AND MF.PDOC_CODIGO = '" + prefijoFac + "' AND MF.MFAC_NUMEDOCU = " + numeroFac + "; "));
                        }

                        switch (tipoRetenedor)
                        {
                            case "S":
                                valorRte = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                break;
                            case "T":
                                if (valorFactura >= Convert.ToDouble(ds.Tables[0].Rows[0][3]))
                                    valorRte = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                break;
                            default:
                                if (valorFactura >= Convert.ToDouble(ds.Tables[0].Rows[0][3]))
                                    valorRte = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                break;
                        }

                        //if (centavos == "N") 
                            valorRte = Math.Round(valorRte,numDecimales);

                        if (valorRte > 0)
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                            valoresRetenidos.Add(valorRte);
                            basesRetenidas.Add(valorFactura);
                        }
                    }
                }
                catch { throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de venta de " + "servicios y persona " + tipoPersona + ""); }
                    
            }
            //               CAUSACION de retencion en COMPRA
            else if (tipoProceso == "V")//Si es una compra de vehiculos nuevos
            {
                if (tipoPersona == "J")//Solo se puede hacer si es persona juridica
                {
                    if (tipoRetenedor == "S")//Si es retenedor siempre
                    {
                        //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                        //tipo de persona y el porcentaje a retener
                        ds = new DataSet();
                        DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RF'");
                        if (ds.Tables[0].Rows.Count == 0)
                            mensajes += "No hay retenciones para este tipo de proceso y persona";
                        else
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                            valor = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                        //    if (centavos == "N")
                                valor = Math.Round(valor, numDecimales);
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorFactura);
                        }
                    }
                    else if (tipoRetenedor == "T")//Si es retenedor por tope
                    {
                        //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                        //tipo de persona, el porcentaje a retener y el tope desde donde se empieza a cobrar
                        //retención
                        ds = new DataSet();
                        DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo, COALESCE(pret_porcendecl,0), COALESCE(pret_tope,0) FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RF'");
                        if (valorFactura >= Convert.ToDouble(ds.Tables[0].Rows[0][2]))
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                            valor = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                        //    if (centavos == "N")
                                valor = Math.Round(valor, numDecimales); 
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorFactura);
                        }
                        else
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                        //     if (centavos == "N")
                                valor = Math.Round(valor, numDecimales);
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorFactura);
                        }
                    }
                }
            }
            else if (tipoProceso == "R")//Si es una compra de repuestos, incluye lubricantes y servicios
            {
                if (dtRepuestos == null) return;
                if (dtRepuestos.Rows.Count == 0) return;

                //Saco los item relacionados, su origen, la cantidad facturada, el valor unitario, 
                //el porcentaje de descuento y el calculo del total, luego divido los items
                //dependiendo de su origen
                if (tipoPersona == "J")//Si es persona juridica, puedo hacer las 3 cosas
                {
                    if (tipoRetenedor == "S")//Si es retenedor siempre
                    {
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            if (dtRepuestos.Rows.Count == 0) return;

                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();

                            for (int i = 0; i < pedidos.Count; i++)
                            {

                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\" FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='L' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RF';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    tipoY = ds.Tables[0].Select("tori_codigo='Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorX = valorX + valorItem;
                                            //valorX=valorX+();//Convert.ToDouble(tipoX[j][9]);
                                        }
                                    }
                                    if (tipoY.Length != 0)//Si tipo Y->Lubricantes
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoY.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoY[j][4]) * Convert.ToDouble(tipoY[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoY[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorY = valorY + valorItem;
                                            //valorY=valorY+Convert.ToDouble(tipoY[j][9]);
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorOtros = valorOtros + valorItem;
                                            //valorOtros=valorOtros+Convert.ToDouble(otroTipo[j][9]);
                                        }
                                    }
                                }
                            }
                            //Saco la retención para el resto R
                            if (ds.Tables[1].Rows.Count > 0)
                                valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            //Saco la retención para el tipo Y
                            if (ds.Tables[2].Rows.Count > 0)
                                valorRteY = valorY * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de lubricantes y persona " + tipoPersona + "");
                            //Saco la retención para el tipo X
                            if (ds.Tables[3].Rows.Count > 0)
                                valorRteX = valorX * Convert.ToDouble(ds.Tables[3].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //     if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteY > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteY = Math.Round(valorRteY, numDecimales);
                                valoresRetenidos.Add(valorRteY);
                                basesRetenidas.Add(valorY);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[3].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                    else if (tipoRetenedor == "T")
                    {
                        //Como es retenedor por tope debo verificar que el valor total de la factura supere
                        //el tope de retencion para poder realizar el proceso, sino lo hace, pongo una retencion
                        //de $0
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();
                            for (int i = 0; i < pedidos.Count; i++)
                            {

                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, el porcentaje y el tope para retener
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='L' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RF';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    tipoY = ds.Tables[0].Select("tori_codigo='Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorX = valorX + valorItem;
                                        }
                                    }
                                    if (tipoY.Length != 0)//Si tipo Y->Lubricantes
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoY.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoY[j][4]) * Convert.ToDouble(tipoY[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoY[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorY = valorY + valorItem;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorOtros = valorOtros + valorItem;
                                        }
                                    }
                                }
                            }
                            //Si el valor de la factura es mayor o igual al tope especificado, calculo retencion
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (valorFactura >= Convert.ToDouble(ds.Tables[1].Rows[0][2]))
                                {
                                    //Saco la retención para el resto R
                                    valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                if (valorFactura >= Convert.ToDouble(ds.Tables[2].Rows[0][2]))
                                {
                                    //Saco la retención para el tipo Y
                                    valorRteY = valorY * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de lubricantes y persona " + tipoPersona + "");
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                if (valorFactura >= Convert.ToDouble(ds.Tables[3].Rows[0][2]))
                                {
                                    //Saco la retención para el tipo X
                                    valorRteX = valorX * Convert.ToDouble(ds.Tables[3].Rows[0][1]) / 100;
                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteY > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteY = Math.Round(valorRteY, numDecimales);
                                valoresRetenidos.Add(valorRteY);
                                basesRetenidas.Add(valorY);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[3].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                }
                else if (tipoPersona == "N")//Si es persona natural, solo se permite repuestos y servicios
                {
                    if (tipoRetenedor == "S")// Si es retenedor siempre
                    {
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();

                            ds = new DataSet();

                            for (int i = 0; i < pedidos.Count; i++)
                            {
                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {

                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_porcenNOdecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcendecl, pret_porcenNOdecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RF';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorX = valorX + valorItem;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorOtros = valorOtros + valorItem;
                                        }
                                    }
                                }
                            }
                            //Saco la retención para el resto R
                            if (ds.Tables[1].Rows.Count > 0)
                                if (tipoSociedad == "N")
                                    valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][2]) / 100;
                                else
                                    valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            //Saco la retención para el tipo X
                            if (ds.Tables[2].Rows.Count > 0)
                                if (tipoSociedad == "N")
                                    valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][2]) / 100;
                                else
                                    valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                             //   if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                    else if (tipoRetenedor == "T")
                    {
                        //Como es tipo de persona natural solo se permite compra de lubricantes y otros
                        //Como es retenedor por tope debo verificar que el valor total de la factura supere
                        //el tope de retencion para poder realizar el proceso, sino lo hace, pongo una retencion
                        //de $0
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();
                            for (int i = 0; i < pedidos.Count; i++)
                            {

                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, el porcentaje y el tope para retener
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_porcenNOdecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcendecl,pret_porcenNOdecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RF';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorX = valorX + valorItem;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorOtros = valorOtros + valorItem;
                                        }
                                    }
                                }
                            }
                            //Si el valor de la factura es mayor o igual al tope especificado, calculo retencion
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (valorFactura >= Convert.ToDouble(ds.Tables[1].Rows[0][3]))
                                {
                                    //Saco la retención para el resto R
                                    if (tipoSociedad == "N")
                                        valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][2]) / 100;
                                    else
                                        valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de repuestos y persona " + tipoPersona + "");

                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                if (valorFactura >= Convert.ToDouble(ds.Tables[2].Rows[0][3]))
                                {
                                    //Saco la retención para el tipo X
                                    if (tipoSociedad == "N")
                                        valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][2]) / 100;
                                    else
                                        valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;

                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es diferente a 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                    else if (tipoRetenedor == "")//Si es no declarante
                    {
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();
                            for (int i = 0; i < pedidos.Count; i++)
                            {
                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcennodecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RF';" +
                                        "SELECT pret_codigo,pret_porcennodecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RF';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorX = valorX + valorItem;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorOtros = valorOtros + valorItem;
                                        }
                                    }
                                }
                            }
                            //Saco la retención para el resto R
                            if (ds.Tables[1].Rows.Count > 0)
                                valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            //Saco la retención para el tipo X
                            if (ds.Tables[2].Rows.Count > 0)
                                valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                }
            }
            else if (tipoProceso == "S")//Si es una venta de operaciones
            {
                ds = new DataSet();
                double valorRte = 0;


                DBFunctions.Request(ds, IncludeSchema.NO,
                    "SELECT pret_codigo,pret_porcendecl,pret_porcenNOdecl,pret_tope FROM pretencion WHERE ttip_codigo='" +
                    tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RF';");

                if (ds.Tables[0] != null)
                {
                    switch (tipoRetenedor)
                    {
                        case "S":
                            if (tipoSociedad == "N")
                                valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][2]) / 100;
                            else
                                valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                            break;
                        case "T":
                            if (valorOperaciones >= Convert.ToDouble(ds.Tables[0].Rows[0][3])) 
                                if (tipoSociedad == "N")
                                    valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][2]) / 100;
                                else
                                    valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                            break;
                        case "":
                            if (tipoSociedad == "N")
                                valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][2]) / 100;
                            else
                                valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                            break;
                    }

                    //if (centavos == "N") 
                        valorRte = Math.Round(valorRte,numDecimales);

                    if (valorRte > 0)
                    {
                        tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                        valoresRetenidos.Add(valorRte);
                        basesRetenidas.Add(valorOperaciones);
                    }

                }
                else
                    throw new IndexOutOfRangeException("No se encontró una retención en la fuente para el proceso de venta de " +
                        "servicios y persona " + tipoPersona + "");


                tipoProceso = "R";
                Calcular_ReteFuente();
                tipoProceso = "S";

            }
        }
      
        private void Calcular_ReteICA()
        {
            if(!DBFunctions.RecordExist("select tret_codigo from tretencion where tret_codigo = 'RC' ")) return; // No se calcula ICA porque no APLICA
            if (esVenta && tipoPersona == "N") return;
            if (esVenta && regimenIva == "S") return;
            if (tipoNacionalidad == "E") return;  // extranjero no paga impuestos
            if (regimenIva == "G" && !esVenta) return;        // gran contribuyente autoretenedor no causa impuestos
            if (regimenIva == "H" && !esVenta) return;        // gran contribuyente NO autoretenedor no causa impuesto ICA
      //      if (tipoRetenedorEmpresa == "C" && regimenIva == "C") return;        // Empresa y proveedor son regimen comun
            if (tipoRetenedorEmpresa == "S" && regimenIva == "C" && !esVenta) return;   // Empresa es regimen Simplificado y proveedor es regimen comun

            string ciudadProveedor = DBFunctions.SingleData("SELECT MN.PCIU_CODIGO FROM MNIT MN, CEMPRESA CE WHERE MN.MNIT_NIT = '" + nit + "' AND MN.PCIU_CODIGO = CE.CEMP_CIUDAD ");
            if (ciudadProveedor.Length == 0) 
                return;    // el proveedor es de otra ciudad diferente a la ciudad de la empresa por lo cual NO CAUSA ICA
            else 
                ica = "S"; // el proveedor es de la misma cuidad y no esta parametrizado para ica se forza a S
            double valor = 0;
            DataSet ds = null;
                    
            double valorX = 0, valorY = 0, valorOtros = 0, valorRteX = 0, valorRteY = 0, valorRteOtros = 0;
            DataRow[] tipoX = null, tipoY = null, otroTipo = null;
            if (ica == "S")
            {
                pc_icaprov = "";
                Codica = "";
                if (!esVenta)
                {
                    pc_icaprov = DBFunctions.SingleData("SELECT PICA_PCXMIL FROM PICA P,MPROVEEDOR MP WHERE MP.MNIT_NIT = '" + nit + "' AND MP.PICA_ICA = P.PICA_ICA;"); // toda la actividad de repsuesto se toma de la tabla de retenciones
                    Codica = DBFunctions.SingleData("SELECT P.PICA_ICA FROM PICA P,MPROVEEDOR MP WHERE MP.MNIT_NIT = '" + nit + "' AND MP.PICA_ICA = P.PICA_ICA;");
                }
                else
                {
                    DataSet dsReteIca = new DataSet();
                    DBFunctions.Request(dsReteIca, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_proceso='R' AND ttip_codigo='" + tipoPersona + "' AND tret_codigo='RC'") ;

                    tipoRetencion.Add(dsReteIca.Tables[0].Rows[0][0].ToString());
                    valor = valorFactura * Convert.ToDouble(dsReteIca.Tables[0].Rows[0][1]) / 100;
                    valor = Math.Round(valor, numDecimales);
                    valoresRetenidos.Add(valor);
                    basesRetenidas.Add(valorFactura);

                    return;  // sale cálculo rete_ica en venta
                     
                }

                if (pc_icaprov.Length != 0)
                {
                    double tope_monto = Convert.ToDouble(DBFunctions.SingleData("SELECT PICA_TOPEMONTO FROM PICA P,MPROVEEDOR MP WHERE MP.MNIT_NIT = '" + nit + "' AND MP.PICA_ICA = P.PICA_ICA;"));
                    if (valorFactura >= tope_monto)
                    {
                        valor = valorFactura * Convert.ToDouble(pc_icaprov) / 1000;
                        string cod_reteica = DBFunctions.SingleData("SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_proceso='R' AND ttip_codigo='" + tipoPersona + "' AND tret_codigo='RC'");
                        if(cod_reteica != "")
                        {
                            tipoRetencion.Add(cod_reteica);
                        //    if (centavos == "N")
                                valor = Math.Round(valor, numDecimales);
                            valorica = valor;
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorFactura);
                        }
                    }
                }
                else
                {
                    if (tipoProceso == "V")//Si es una compra de vehiculos nuevos
                    {
                        if (tipoPersona == "J")//Solo se puede hacer si es persona juridica
                        {
                            if (tipoRetenedor == "S")//Si es retenedor siempre
                            {
                                //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                                //tipo de persona y el porcentaje a retener
                                ds = new DataSet();
                                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RC'");
                                if (ds.Tables[0].Rows.Count == 0)
                                    mensajes += "No hay retenciones para este tipo de proceso y persona";
                                else
                                {
                                    tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                                    valor = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                //    if (centavos == "N")
                                        valor = Math.Round(valor, numDecimales);
                                    valoresRetenidos.Add(valor);
                                    basesRetenidas.Add(valorFactura);
                                }
                            }
                            else if (tipoRetenedor == "T")//Si es retenedor por tope
                            {
                                //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                                //tipo de persona, el porcentaje a retener y el tope desde donde se empieza a cobrar
                                //retención
                                ds = new DataSet();
                                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RC'");
                                if (valorFactura >= Convert.ToDouble(ds.Tables[0].Rows[0][2]))
                                {
                                    tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                                    valor = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                //    if (centavos == "N")
                                        valor = Math.Round(valor, numDecimales);
                                    valoresRetenidos.Add(valor);
                                    basesRetenidas.Add(valorFactura);
                                }
                                else
                                {
                                    tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                                //   if (centavos == "N")
                                        valor = Math.Round(valor, numDecimales);
                                    valoresRetenidos.Add(valor);
                                    basesRetenidas.Add(valorFactura);
                                }
                            }
                        }
                    }
                    else if (tipoProceso == "R")//Si es una compra de repuestos, incluye lubricantes y servicios
                    {
                        if (dtRepuestos == null) return;
                        if (dtRepuestos.Rows.Count == 0) return;

                        //Saco los item relacionados, su origen, la cantidad facturada, el valor unitario, 
                        //el porcentaje de descuento y el calculo del total, luego divido los items
                        //dependiendo de su origen
                        if (tipoPersona == "J")//Si es persona juridica, puedo hacer las 3 cosas
                        {
                            if (tipoRetenedor == "S")//Si es retenedor siempre
                            {
                                if (dtRepuestos != null)//Verifico q la tabla de items exista
                                {

                                    this.Llenar_ArrayList_Pedidos();

                                    ds = new DataSet();
                                    for (int i = 0; i < pedidos.Count; i++)
                                    {
                                        if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                        {
                                            //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                            this.Agregar_Tabla(ds);
                                            //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                            //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                            DBFunctions.Request(ds, IncludeSchema.NO, 
                                                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='L' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RC';");
                                            tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                            tipoY = ds.Tables[0].Select("tori_codigo='Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoX.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorX = valorX + valorItem;
                                                }
                                            }
                                            if (tipoY.Length != 0)//Si tipo Y->Lubricantes
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoY.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoY[j][4]) * Convert.ToDouble(tipoY[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoY[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorY = valorY + valorItem;
                                                }
                                            }
                                            if (otroTipo.Length != 0)//Si hay de otro tipo
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < otroTipo.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorOtros = valorOtros + valorItem;
                                                }
                                            }
                                        }
                                    }
                                    //Saco la retención para el resto R
                                    if (ds.Tables[1].Rows.Count > 0)
                                        valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                                    //Saco la retención para el tipo Y
                                    if (ds.Tables[2].Rows.Count > 0)
                                        valorRteY = valorY * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de lubricantes y persona " + tipoPersona + "");
                                    //Saco la retención para el tipo X
                                    if (ds.Tables[3].Rows.Count > 0)
                                        valorRteX = valorX * Convert.ToDouble(ds.Tables[3].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de servicios y persona " + tipoPersona + "");
                                    //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                                    //retenciones causadas
                                    if (valorRteOtros > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                        valoresRetenidos.Add(valorRteOtros);
                                        basesRetenidas.Add(valorOtros);
                                    }
                                    if (valorRteY > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteY = Math.Round(valorRteY, numDecimales);
                                        valoresRetenidos.Add(valorRteY);
                                        basesRetenidas.Add(valorY);
                                    }
                                    if (valorRteX > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[3].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteX = Math.Round(valorRteX, numDecimales);
                                        valoresRetenidos.Add(valorRteX);
                                        basesRetenidas.Add(valorX);
                                    }
                                }
                            }
                            else  //  if (tipoRetenedor == "T")  que lo haga a todas las personas juridicas
                            {
                                //Como es retenedor por tope debo verificar que el valor total de la factura supere
                                //el tope de retencion para poder realizar el proceso, sino lo hace, pongo una retencion
                                //de $0
                                if (dtRepuestos != null)//Verifico q la tabla de items exista
                                {
                                    this.Llenar_ArrayList_Pedidos();

                                    ds = new DataSet();
                                    for (int i = 0; i < pedidos.Count; i++)
                                    {
                                        if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                        {
                                            //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                            this.Agregar_Tabla(ds);
                                            //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                            //Saco el codigo de la retención aplicada a esa persona y ese proceso, el porcentaje y el tope para retener
                                            DBFunctions.Request(ds, IncludeSchema.NO, 
                                                "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='L' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RC';");
                                            tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                            tipoY = ds.Tables[0].Select("tori_codigo='Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoX.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorX = valorX + valorItem;
                                                }
                                            }
                                            if (tipoY.Length != 0)//Si tipo Y->Lubricantes
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoY.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoY[j][4]) * Convert.ToDouble(tipoY[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoY[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorY = valorY + valorItem;
                                                }
                                            }
                                            if (otroTipo.Length != 0)//Si hay de otro tipo
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < otroTipo.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorOtros = valorOtros + valorItem;
                                                }
                                            }
                                        }
                                    }
                                    //Si el valor de la factura es mayor o igual al tope especificado, calculo retencion
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (valorFactura >= Convert.ToDouble(ds.Tables[1].Rows[0][2]))
                                        {
                                            //Saco la retención para el resto R
                                            valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                        }
                                    }
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        if (valorFactura >= Convert.ToDouble(ds.Tables[2].Rows[0][2]))
                                        {
                                            //Saco la retención para el tipo Y
                                            valorRteY = valorY * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                        }
                                    }
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de lubricantes y persona " + tipoPersona + "");
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        if (valorFactura >= Convert.ToDouble(ds.Tables[3].Rows[0][2]))
                                        {
                                            //Saco la retención para el tipo X
                                            valorRteX = valorX * Convert.ToDouble(ds.Tables[3].Rows[0][1]) / 100;
                                        }
                                    }
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de servicios y persona " + tipoPersona + "");
                                    //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                                    //retenciones causadas
                                    if (valorRteOtros > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                        valoresRetenidos.Add(valorRteOtros);
                                        basesRetenidas.Add(valorOtros);
                                    }
                                    if (valorRteY > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteY = Math.Round(valorRteY, numDecimales);
                                        valoresRetenidos.Add(valorRteY);
                                        basesRetenidas.Add(valorY);
                                    }
                                    if (valorRteX > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[3].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteX = Math.Round(valorRteX, numDecimales);
                                        valoresRetenidos.Add(valorRteX);
                                        basesRetenidas.Add(valorX);
                                    }
                                }
                            }
                        }
                        else if (tipoPersona == "N")//Si es persona natural, solo se permite repuestos y servicios
                        {
                            if (tipoRetenedor == "S")// Si es retenedor siempre
                            {
                                if (dtRepuestos != null)//Verifico q la tabla de items exista
                                {
                                    this.Llenar_ArrayList_Pedidos();

                                    ds = new DataSet();
                                    for (int i = 0; i < pedidos.Count; i++)
                                    {
                                        if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                        {
                                            //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                            this.Agregar_Tabla(ds);
                                            //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                            //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RC';");
                                            tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                            otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoX.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorX = valorX + valorItem;
                                                }
                                            }
                                            if (otroTipo.Length != 0)//Si hay de otro tipo
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < otroTipo.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorOtros = valorOtros + valorItem;
                                                }
                                            }
                                        }
                                    }
                                    //Saco la retención para el resto R
                                    if (ds.Tables[1].Rows.Count > 0)
                                        valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                                    //Saco la retención para el tipo X
                                    if (ds.Tables[2].Rows.Count > 0)
                                        valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de servicios y persona " + tipoPersona + "");
                                    //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                                    //retenciones causadas
                                    if (valorRteOtros > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                        valoresRetenidos.Add(valorRteOtros);
                                        basesRetenidas.Add(valorOtros);
                                    }
                                    if (valorRteX > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteX = Math.Round(valorRteX, numDecimales);
                                        valoresRetenidos.Add(valorRteX);
                                        basesRetenidas.Add(valorX);
                                    }
                                }
                            }
                            else if (tipoRetenedor == "T")
                            {
                                //Como es tipo de persona natural solo se permite compra de lubricantes y otros
                                //Como es retenedor por tope debo verificar que el valor total de la factura supere
                                //el tope de retencion para poder realizar el proceso, sino lo hace, pongo una retencion
                                //de $0
                                if (dtRepuestos != null)//Verifico q la tabla de items exista
                                {
                                    this.Llenar_ArrayList_Pedidos();

                                    ds = new DataSet();
                                    for (int i = 0; i < pedidos.Count; i++)
                                    {
                                        if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                        {
                                            //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                            this.Agregar_Tabla(ds);
                                            //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                            //Saco el codigo de la retención aplicada a esa persona y ese proceso, el porcentaje y el tope para retener
                                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RC';");
                                            tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                            otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoX.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorX = valorX + valorItem;
                                                }
                                            }
                                            if (otroTipo.Length != 0)//Si hay de otro tipo
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < otroTipo.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorOtros = valorOtros + valorItem;
                                                }
                                            }
                                        }
                                    }
                                    //Si el valor de la factura es mayor o igual al tope especificado, calculo retencion
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (valorFactura >= Convert.ToDouble(ds.Tables[1].Rows[0][2]))
                                        {
                                            //Saco la retención para el resto R
                                            valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                        }
                                    }
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        if (valorFactura >= Convert.ToDouble(ds.Tables[2].Rows[0][2]))
                                        {
                                            //Saco la retención para el tipo X
                                            valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                        }
                                    }
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de servicios y persona " + tipoPersona + "");
                                    //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                                    //retenciones causadas
                                    if (valorRteOtros > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                        valoresRetenidos.Add(valorRteOtros);
                                        basesRetenidas.Add(valorOtros);
                                    }
                                    if (valorRteX > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteX = Math.Round(valorRteX, numDecimales);
                                        valoresRetenidos.Add(valorRteX);
                                        basesRetenidas.Add(valorX);
                                    }
                                }
                            }
                            else if (tipoRetenedor == "")//Si es no declarante
                            {
                                if (dtRepuestos != null)//Verifico q la tabla de items exista
                                {
                                    this.Llenar_ArrayList_Pedidos();

                                    ds = new DataSet();
                                    for (int i = 0; i < pedidos.Count; i++)
                                    {
                                        if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                        {
                                            //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                            this.Agregar_Tabla(ds);
                                            //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                            //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                            DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcennodecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RC';" +
                                                "SELECT pret_codigo,pret_porcennodecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RC';");
                                            tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                            otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                            if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < tipoX.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorX = valorX + valorItem;
                                                }
                                            }
                                            if (otroTipo.Length != 0)//Si hay de otro tipo
                                            {
                                                //Sumo el valor total para este tipo
                                                for (int j = 0; j < otroTipo.Length; j++)
                                                {
                                                    double valorItem = 0, valorItemDesc = 0;
                                                    valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                                    valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                                    valorItem = valorItem - valorItemDesc;
                                                    valorOtros = valorOtros + valorItem;
                                                }
                                            }
                                        }
                                    }
                                    //Saco la retención para el resto R
                                    if (ds.Tables[1].Rows.Count > 0)
                                        valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                                    //Saco la retención para el tipo X
                                    if (ds.Tables[2].Rows.Count > 0)
                                        valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                    else
                                        throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de compra de servicios y persona " + tipoPersona + "");
                                    //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                                    //retenciones causadas
                                    if (valorRteOtros > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                        valoresRetenidos.Add(valorRteOtros);
                                        basesRetenidas.Add(valorOtros);
                                    }
                                    if (valorRteX > 0)
                                    {
                                        tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                                    //    if (centavos == "N")
                                            valorRteX = Math.Round(valorRteX, numDecimales);
                                        valoresRetenidos.Add(valorRteX);
                                        basesRetenidas.Add(valorX);
                                    }
                                }
                            }
                        }
                    }
                    else if (tipoProceso == "S")//Si es una venta de operaciones
                    {
                        ds = new DataSet();
                        double valorRte = 0;


                        DBFunctions.Request(ds, IncludeSchema.NO,
                            "SELECT pret_codigo,pret_porcendecl,pret_porcennodecl,pret_tope FROM pretencion WHERE ttip_codigo='" +
                            tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RC';");

                        if (ds.Tables[0] != null)
                        {
                            switch (tipoRetenedor)
                            {
                                case "S":
                                    valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                    break;
                                case "T":
                                    if (valorOperaciones >= Convert.ToDouble(ds.Tables[0].Rows[0][3]))
                                        valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                                    break;
                                case "":
                                    valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][2]) / 100;
                                    break;
                            }

                            //if (centavos == "N") 
                                valorRte = Math.Round(valorRte,numDecimales);

                            if (valorRte > 0)
                            {
                                tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                                valoresRetenidos.Add(valorRte);
                                basesRetenidas.Add(valorOperaciones);
                            }

                        }
                        else
                            throw new IndexOutOfRangeException("No se encontró una retención de ICA para el proceso de venta de " +
                                "servicios y persona " + tipoPersona + "");


                        tipoProceso = "R";
                        Calcular_ReteICA();
                        tipoProceso = "S";
                    }
                }
            }
        }

        private void Calcular_ReteIVA()
        {
            string ivaNit = DBFunctions.SingleData("select pret_reteiva from pretencionivanit where mnit_nit = '" + nit + "'; ");

            string causaRetencion = DBFunctions.SingleData("select COALESCE(ccon_causaretenot,'N') from CCONTABILIDAD");
            if (esVenta)
            {
                if (causaRetencion == "S")
                {

                }
                else
                    return; // SI en el parametro de causar retenciones en la venta en CCONTABILIDAD ESTA en N NO SE CAUSAN LA RETE-FUENTE EN LA VENTA
            }
            if (esVenta && tipoPersona == "N") return;
            if (tipoNacionalidad == "E") return;  // extranjero no paga impuestos
            if (regimenIva == "G" && ivaNit == "") return;        // gran contribuyente autoretenedor no causa impuestos
            if (regimenIva == "H") return;        // es una compra a gran Contribuyente no causa rete iva
            if (regimenIva == "C")
            {
                string tipoRetenedorIva = DBFunctions.SingleData("SELECT treg_regiiva from mnit, cempresa where cempresa.mnit_nit = mnit.mnit_nit;");
                if (tipoRetenedorIva == "C") return; //proveedor y empresa ambos regimen comun
            }
            double valor = 0;
            DataSet ds = null;
            double valorX = 0, valorY = 0, valorOtros = 0, valorRteX = 0, valorRteY = 0, valorRteOtros = 0;
            double valorBaseX = 0, valorBaseY = 0, valorBaseOtros = 0;
            
            DataRow[] tipoX = null, tipoY = null, otroTipo = null;
            if (tipoProceso == "V")//Si es una compra de vehiculos nuevos
            {
                if (tipoPersona == "J")//Solo se puede hacer si es persona juridica
                {
                    if (tipoRetenedor == "S" || tipoRetenedorEmpresa == "G")//Si es retenedor siempre o la empresa es GRAN CONTRIBUYENTE
                    {
                        //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                        //tipo de persona y el porcentaje a retener                      
                        DBFunctions.Request(ds, IncludeSchema.NO, "select pret_reteiva from pretencionivanit where mnit_nit = '" + nit + "");
                        if (ds == null || nit == "900466209")
                            mensajes += "No hay retenciones para este tipo de proceso y persona";
                        else
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                            if (ivaNit != "")
                                valor = valorIVA * Convert.ToDouble(ivaNit) / 100;
                            else
                                valor = valorIVA * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                            valor = Math.Round(valor, numDecimales);
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorIVA);
                        }
                    }
                    else if (tipoRetenedor == "T") //Si es retenedor por tope
                    {
                        //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                        //tipo de persona, el porcentaje a retener y el tope desde donde se empieza a cobrar
                        //retención
                        ds = new DataSet();
                        DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RI'");
                        if (valorIVA >= Convert.ToDouble(ds.Tables[0].Rows[0][2]))
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                            if (ivaNit != "")
                                valor = valorIVA * Convert.ToDouble(ivaNit) / 100;
                            else
                                valor = valorIVA * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                        //    if (centavos == "N")
                                valor = Math.Round(valor, numDecimales);
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorIVA);
                        }
                        else
                        {
                            tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                            if (ivaNit != "")
                                valor = valorIVA * Convert.ToDouble(ivaNit) / 100;
                        //    if (centavos == "N")
                                valor = Math.Round(valor, numDecimales);
                            valoresRetenidos.Add(valor);
                            basesRetenidas.Add(valorIVA);
                        }
                    }
                }
            }
            else if (tipoProceso == "R")//Si es una compra de repuestos, incluye lubricantes y servicios
            {
                if (dtRepuestos == null) return;
                if (dtRepuestos.Rows.Count == 0) return;

                #region retiva_repuestos_juridica

                //Saco los item relacionados, su origen, la cantidad facturada, el valor unitario, 
                //el porcentaje de descuento y el calculo del total, luego divido los items
                //dependiendo de su origen
                if (tipoPersona == "J")//Si es persona juridica, puedo hacer las 3 cosas
                {
                    //   if (tipoRetenedor == "S")//Si es retenedor siempre
                    //   {
                    if (dtRepuestos != null) //Verifico q la tabla de items exista
                    {
                        this.Llenar_ArrayList_Pedidos();
                        ds = new DataSet();
                        for (int i = 0; i < pedidos.Count; i++)
                        {
                            if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                            {
                                //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                this.Agregar_Tabla(ds);
                                //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total,MITE.piva_porciva AS \"Porc IVA\" FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                //														0						1						2												3										4																		5																	6																											
                                //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RI';" +
                                    "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='L' AND tret_codigo='RI';" +
                                    "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RI';");
                                tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                tipoY = ds.Tables[0].Select("tori_codigo='Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                {
                                    //Sumo el valor total para este tipo
                                    for (int j = 0; j < tipoX.Length; j++)
                                    {
                                        double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                        valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                        valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                        valorItem = valorItem - valorItemDesc;
                                        valorIva = valorItem * (Convert.ToDouble(tipoX[j][7]) / 100);
                                        valorX = valorX + valorIva;
                                        valorBaseX = valorBaseX + valorItem;
                                    }
                                }
                                if (tipoY.Length != 0)//Si tipo Y->Lubricantes
                                {
                                    //Sumo el valor total para este tipo
                                    for (int j = 0; j < tipoY.Length; j++)
                                    {
                                        double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                        valorItem = Convert.ToDouble(tipoY[j][4]) * Convert.ToDouble(tipoY[j][5]);
                                        valorItemDesc = valorItem * (Convert.ToDouble(tipoY[j][6]) / 100);
                                        valorItem = valorItem - valorItemDesc;
                                        valorIva = valorItem * (Convert.ToDouble(tipoY[j][7]) / 100);
                                        valorY = valorY + valorIva;
                                        valorBaseY = valorBaseY + valorItem;
                                    }
                                }
                                if (otroTipo.Length != 0)//Si hay de otro tipo
                                {
                                    //Sumo el valor total para este tipo
                                    for (int j = 0; j < otroTipo.Length; j++)
                                    {
                                        double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                        valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                        valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                        valorItem = valorItem - valorItemDesc;
                                        valorIva = valorItem * (Convert.ToDouble(otroTipo[j][7]) / 100);
                                        valorOtros = valorOtros + valorIva;
                                        valorBaseOtros = valorBaseOtros + valorItem;
                                    }
                                }
                            }
                        }
                        //Saco la retención para el resto R
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                //          if ((tipoRetenedor == "S") || (tipoRetenedor == "T" && valorBaseOtros >= Convert.ToDouble(ds.Tables[1].Rows[0][2])))
                            if ((tipoRetenedor != "T") || (tipoRetenedor == "T" && valorBaseOtros >= Convert.ToDouble(ds.Tables[1].Rows[0][2])))
                                if (ivaNit != "" && Convert.ToDouble(ds.Tables[1].Rows[0][1]) > Convert.ToDouble(ivaNit))
                                    valorRteOtros = valorOtros * Convert.ToDouble(ivaNit) / 100;
                                else
                                    valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                        }
                        else
                            throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                        
                        //Saco la retención para el tipo Y
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            if ((tipoRetenedor == "S") || (tipoRetenedor == "T" && valorBaseY >= Convert.ToDouble(ds.Tables[2].Rows[0][2])))
                                if (ivaNit != "" && Convert.ToDouble(ds.Tables[2].Rows[0][1]) > Convert.ToDouble(ivaNit))
                                    valorRteY = valorY * Convert.ToDouble(ivaNit) / 100;
                                else
                                    valorRteY = valorY * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                        }
                        else
                            throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de lubricantes y persona " + tipoPersona + "");
                        
                        //Saco la retención para el tipo X
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            if ((tipoRetenedor == "S") || (tipoRetenedor == "T" && valorBaseX >= Convert.ToDouble(ds.Tables[3].Rows[0][2])))
                                if (ivaNit != "" && Convert.ToDouble(ds.Tables[3].Rows[0][1]) > Convert.ToDouble(ivaNit))
                                    valorRteX = valorX * Convert.ToDouble(ivaNit) / 100;
                                else
                                    valorRteX = valorX * Convert.ToDouble(ds.Tables[3].Rows[0][1]) / 100;
                    }
                        else
                            throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de servicios y persona " + tipoPersona + "");
                        
                        //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                        //retenciones causadas
                        if (valorRteOtros > 0)
                        {
                            tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                        //    if (centavos == "N")
                                valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                            valoresRetenidos.Add(valorRteOtros);
                            basesRetenidas.Add(valorOtros);
                        }
                        if (valorRteY > 0)
                        {
                            tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                        //    if (centavos == "N")
                                valorRteY = Math.Round(valorRteY, numDecimales);
                            valoresRetenidos.Add(valorRteY);
                            basesRetenidas.Add(valorY);
                        }
                        if (valorRteX > 0)
                        {
                            tipoRetencion.Add(ds.Tables[3].Rows[0][0].ToString());
                        //    if (centavos == "N")
                                valorRteX = Math.Round(valorRteX, numDecimales);
                            valoresRetenidos.Add(valorRteX);
                            basesRetenidas.Add(valorX);
                        }
                    }
                    #endregion
                    #region retiva_repuestos_apagada
                    /*
                   }
               
               else if (tipoRetenedor == "T")
               {
                   //Como es retenedor por tope debo verificar que el valor total de la factura supere
                   //el tope de retencion para poder realizar el proceso, sino lo hace, pongo una retencion
                   //de $0
                   if (dtRepuestos != null)//Verifico q la tabla de items exista
                   {
                       this.Llenar_ArrayList_Pedidos();
                       ds = new DataSet();
                       for (int i = 0; i < pedidos.Count; i++)
                       {
                           if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                           {
                               //string[] partes=pedidos[i].ToString().Trim().Split('-');

                               this.Agregar_Tabla(ds);
                               //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total,MITE.piva_porciva AS \"Porc IVA\" FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                               //Saco el codigo de la retención aplicada a esa persona y ese proceso, el porcentaje y el tope para retener
                               DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RI';" +
                                   "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='L' AND tret_codigo='RI';" +
                                   "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RI';");
                               tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                               tipoY = ds.Tables[0].Select("tori_codigo='Y' AND num_ped='" + pedidos[i].ToString() + "'");
                               otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                               if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                               {
                                   //Sumo el valor total para este tipo
                                   for (int j = 0; j < tipoX.Length; j++)
                                   {
                                       double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                       valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                       valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                       valorItem = valorItem - valorItemDesc;
                                       valorIva = valorItem * (Convert.ToDouble(tipoX[j][7]) / 100);
                                       valorX = valorX + valorIva;
                                   }
                               }
                               if (tipoY.Length != 0)//Si tipo Y->Lubricantes
                               {
                                   //Sumo el valor total para este tipo
                                   for (int j = 0; j < tipoY.Length; j++)
                                   {
                                       double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                       valorItem = Convert.ToDouble(tipoY[j][4]) * Convert.ToDouble(tipoY[j][5]);
                                       valorItemDesc = valorItem * (Convert.ToDouble(tipoY[j][6]) / 100);
                                       valorItem = valorItem - valorItemDesc;
                                       valorIva = valorItem * (Convert.ToDouble(tipoY[j][7]) / 100);
                                       valorY = valorY + valorIva;
                                   }
                               }
                               if (otroTipo.Length != 0)//Si hay de otro tipo
                               {
                                   //Sumo el valor total para este tipo
                                   for (int j = 0; j < otroTipo.Length; j++)
                                   {
                                       double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                       valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                       valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                       valorItem = valorItem - valorItemDesc;
                                       valorIva = valorItem * (Convert.ToDouble(otroTipo[j][7]) / 100);
                                       valorOtros = valorOtros + valorIva;
                                   }
                               }
                           }
                       }
                       //Si el valor de la factura es mayor o igual al tope especificado, calculo retencion
                       if (ds.Tables[1].Rows.Count > 0)
                       {
                           if (valorIVA >= Convert.ToDouble(ds.Tables[1].Rows[0][2]))
                           {
                               //Saco la retención para el resto R
                               valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                           }
                       }
                       else
                           throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                       if (ds.Tables[2].Rows.Count > 0)
                       {
                           if (valorIVA >= Convert.ToDouble(ds.Tables[2].Rows[0][2]))
                           {
                               //Saco la retención para el tipo Y
                               valorRteY = valorY * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                           }
                       }
                       else
                           throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de lubricantes y persona " + tipoPersona + "");
                       if (ds.Tables[3].Rows.Count > 0)
                       {
                           if (valorIVA >= Convert.ToDouble(ds.Tables[3].Rows[0][2]))
                           {
                               //Saco la retención para el tipo X
                               valorRteX = valorX * Convert.ToDouble(ds.Tables[3].Rows[0][1]) / 100;
                           }
                       }
                       else
                           throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de servicios y persona " + tipoPersona + "");
                       //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                       //retenciones causadas
                       if (valorRteOtros > 0)
                       {
                           tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                           if (centavos == "N")
                               valorRteOtros = Math.Round(valorRteOtros, 0);
                           valoresRetenidos.Add(valorRteOtros);
                           basesRetenidas.Add(valorOtros);
                       }
                       if (valorRteY > 0)
                       {
                           tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                           if (centavos == "N")
                               valorRteY = Math.Round(valorRteY, 0);
                           valoresRetenidos.Add(valorRteY);
                           basesRetenidas.Add(valorY);
                       }
                       if (valorRteX > 0)
                       {
                           tipoRetencion.Add(ds.Tables[3].Rows[0][0].ToString());
                           if (centavos == "N")
                               valorRteX = Math.Round(valorRteX, 0);
                           valoresRetenidos.Add(valorRteX);
                           basesRetenidas.Add(valorX);
                       }
                   }
               } 
                   
                */
                #endregion
                }
              
                #region retiva_repuestos_natural
                
                else if (tipoPersona == "N")//Si es persona natural, solo se permite repuestos y servicios
                {
                    if (tipoRetenedor == "S")// Si es retenedor siempre
                    {
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();
                            for (int i = 0; i < pedidos.Count; i++)
                            {

                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total,MITE.piva_porciva AS \"Porc IVA\" FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RI';" +
                                        "SELECT pret_codigo,pret_porcendecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RI';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorIva = valorItem * (Convert.ToDouble(tipoX[j][7]) / 100);
                                            valorX = valorX + valorIva;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorIva = valorItem * (Convert.ToDouble(otroTipo[j][7]) / 100);
                                            valorOtros = valorOtros + valorIva;
                                        }
                                    }
                                }
                            }
                            //Saco la retención para el resto R
                            if (ds.Tables[1].Rows.Count > 0)
                                valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            //Saco la retención para el tipo X
                            if (ds.Tables[2].Rows.Count > 0)
                                valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                    else if (tipoRetenedor == "T")
                    {
                        //Como es tipo de persona natural solo se permite compra de servicios y otros
                        //Como es retenedor por tope debo verificar que el valor total de la factura supere
                        //el tope de retencion para poder realizar el proceso, sino lo hace, pongo una retencion
                        //de $0
                        if (dtRepuestos != null)//Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();
                            for (int i = 0; i < pedidos.Count; i++)
                            {

                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total,MITE.piva_porciva AS \"Porc IVA\" FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, el porcentaje y el tope para retener
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RI';" +
                                        "SELECT pret_codigo,pret_porcendecl,pret_tope FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RI';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorIva = valorItem * (Convert.ToDouble(tipoX[j][7]) / 100);
                                            valorX = valorX + valorIva;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorIva = valorItem * (Convert.ToDouble(otroTipo[j][7]) / 100);
                                            valorOtros = valorOtros + valorIva;
                                        }
                                    }
                                }
                            }
                            //Si el valor de la factura es mayor o igual al tope especificado, calculo retencion
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (valorIVA >= Convert.ToDouble(ds.Tables[1].Rows[0][2]))
                                {
                                    //Saco la retención para el resto R
                                    valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                if (valorIVA >= Convert.ToDouble(ds.Tables[2].Rows[0][2]))
                                {
                                    //Saco la retención para el tipo X
                                    valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                                }
                            }
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                    else if (tipoRetenedor == "") //Si es no declarante
                    {
                        if (dtRepuestos != null)  //Verifico q la tabla de items exista
                        {
                            this.Llenar_ArrayList_Pedidos();
                            ds = new DataSet();
                            for (int i = 0; i < pedidos.Count; i++)
                            {

                                if (!Verificar_Cantidad_Facturada(pedidos[i].ToString()))
                                {
                                    //string[] partes=pedidos[i].ToString().Trim().Split('-');

                                    this.Agregar_Tabla(ds);
                                    //DBFunctions.Request(ds,IncludeSchema.NO,"SELECT MITE.mite_codigo AS Item,MITE.tori_codigo AS Origen,DPED.dped_cantfact AS \"Cantidad Facturada\",DPED.dped_valounit AS \"Valor Unitario\",DPED.dped_porcdesc AS \"Porcentaje Descuento\",(DPED.dped_valounit*DPED.dped_cantfact-DPED.dped_valounit*DPED.dped_porcdesc/100) AS Total,MITE.piva_porciva AS \"Porc IVA\" FROM dbxschema.mitems MITE,dbxschema.dpedidoitem DPED WHERE MITE.mite_codigo=DPED.mite_codigo AND DPED.pped_codigo='"+partes[0]+"' AND DPED.mped_numepedi="+partes[1]+";"+
                                    //Saco el codigo de la retención aplicada a esa persona y ese proceso, y el porcentaje
                                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo,pret_porcennodecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='R' AND tret_codigo='RI';" +
                                        "SELECT pret_codigo,pret_porcennodecl FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RI';");
                                    tipoX = ds.Tables[0].Select("tori_codigo='X' AND num_ped='" + pedidos[i].ToString() + "'");
                                    otroTipo = ds.Tables[0].Select("tori_codigo<>'X' AND tori_codigo<>'Y' AND num_ped='" + pedidos[i].ToString() + "'");
                                    if (tipoX.Length != 0)//Si hay de tipo X->Servicios
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < tipoX.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                            valorItem = Convert.ToDouble(tipoX[j][4]) * Convert.ToDouble(tipoX[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(tipoX[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorIva = valorItem * (Convert.ToDouble(tipoX[j][7]) / 100);
                                            valorX = valorX + valorIva;
                                        }
                                    }
                                    if (otroTipo.Length != 0)//Si hay de otro tipo
                                    {
                                        //Sumo el valor total para este tipo
                                        for (int j = 0; j < otroTipo.Length; j++)
                                        {
                                            double valorItem = 0, valorItemDesc = 0, valorIva = 0;
                                            valorItem = Convert.ToDouble(otroTipo[j][4]) * Convert.ToDouble(otroTipo[j][5]);
                                            valorItemDesc = valorItem * (Convert.ToDouble(otroTipo[j][6]) / 100);
                                            valorItem = valorItem - valorItemDesc;
                                            valorIva = valorItem * (Convert.ToDouble(otroTipo[j][7]) / 100);
                                            valorOtros = valorOtros + valorIva;
                                        }
                                    }
                                }
                            }
                            //Saco la retención para el resto R
                            if (ds.Tables[1].Rows.Count > 0)
                                valorRteOtros = valorOtros * Convert.ToDouble(ds.Tables[1].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de repuestos y persona " + tipoPersona + "");
                            //Saco la retención para el tipo X
                            if (ds.Tables[2].Rows.Count > 0)
                                valorRteX = valorX * Convert.ToDouble(ds.Tables[2].Rows[0][1]) / 100;
                            else
                                throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de compra de servicios y persona " + tipoPersona + "");
                            //Si el valor de la retención para los distintos tipos es distinto de 0,lo agrego a las
                            //retenciones causadas
                            if (valorRteOtros > 0)
                            {
                                tipoRetencion.Add(ds.Tables[1].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteOtros = Math.Round(valorRteOtros, numDecimales);
                                valoresRetenidos.Add(valorRteOtros);
                                basesRetenidas.Add(valorOtros);
                            }
                            if (valorRteX > 0)
                            {
                                tipoRetencion.Add(ds.Tables[2].Rows[0][0].ToString());
                            //    if (centavos == "N")
                                    valorRteX = Math.Round(valorRteX, numDecimales);
                                valoresRetenidos.Add(valorRteX);
                                basesRetenidas.Add(valorX);
                            }
                        }
                    }
                }
                #endregion
            }
            else if (tipoProceso == "S")//Si es una venta de operaciones
            {
                ds = new DataSet();
                double valorRte = 0;


                DBFunctions.Request(ds, IncludeSchema.NO,
                    "SELECT pret_codigo,pret_porcendecl,pret_porcennodecl,pret_tope FROM pretencion WHERE ttip_codigo='" +
                    tipoPersona + "' AND ttip_proceso='S' AND tret_codigo='RI';");

                if (ds.Tables[0] != null)
                {
                    switch (tipoRetenedor)
                    {
                        case "S":
                            valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                            break;
                        case "T":
                            if (valorOperaciones >= Convert.ToDouble(ds.Tables[0].Rows[0][3]))
                                valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                            break;
                        case "":
                            valorRte = valorOperaciones * Convert.ToDouble(ds.Tables[0].Rows[0][2]) / 100;
                            break;
                    }

                    //if (centavos == "N") 
                        valorRte = Math.Round(valorRte,numDecimales);

                    if (valorRte > 0)
                    {
                        tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                        valoresRetenidos.Add(valorRte);
                        basesRetenidas.Add(valorOperaciones);
                    }

                }
                else
                    throw new IndexOutOfRangeException("No se encontró una retención de IVA para el proceso de venta de " +
                        "servicios y persona " + tipoPersona + "");


                tipoProceso = "R";
                Calcular_ReteIVA();
                tipoProceso = "S";
            }
        }

        private void Calcular_ReteCREE()  // impuesto sobre la renta para la Equidad
        {
            if (esVenta) return;
            if (esVenta && tipoPersona == "N") return;
            if (tipoNacionalidad == "E") return;  // extranjero RESIDENTE EN EL EXTERIOR no paga impuestos
            if (regimenIva == "G") return;        // gran contribuyente autoretenedor no causa impuestos
            
            
            double valor = 0;
            DataSet ds = null;

            string pc_CREEprov = "";  // SE BUSCA SI EL PROVEEDOR TIENE PARAMETRIZADA LA ACTIVAD DE ICA 
            pc_CREEprov = DBFunctions.SingleData("SELECT PICA_PORCCREE FROM PICA P,MPROVEEDOR MP WHERE MP.MNIT_NIT = '" + nit + "' AND MP.PICA_ICA = P.PICA_ICA;");

            if (pc_CREEprov.Length != 0)
            {
                valor = valorFactura * Convert.ToDouble(pc_CREEprov) / 100;
                string cod_reteCREE = DBFunctions.SingleData("SELECT pret_codigo FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' and ttip_proceso = '" + tipoProceso + "' AND tret_codigo='RE'");
                tipoRetencion.Add(cod_reteCREE);
            //    if (centavos == "N")
                    valor = Math.Round(valor, numDecimales);
                valoresRetenidos.Add(valor);
                basesRetenidas.Add(valorFactura);
            }
            else
            {   // SE CALCULA EL CREE de acuerdo a la tabla de retenciones
                if (tipoPersona == "J") //Solo se puede hacer si es persona juridica
                {
                    //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                    //tipo de persona y el porcentaje a retener
                    ds = new DataSet();
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo, COALESCE(pret_porcendecl,0) FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RE'");
                    if (ds.Tables[0].Rows.Count == 0)
                        mensajes += "No hay retenciones CREE para este tipo de proceso y persona";
                    else
                    {
                        tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                        valor = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                    //    if (centavos == "N")
                            valor = Math.Round(valor, numDecimales);
                        valoresRetenidos.Add(valor);
                        basesRetenidas.Add(valorFactura);
                    }
                }

                else
                {
                    //Para este retenedor saco el codigo de retención que aplica para este tipo de proceso y
                    //tipo de persona y el porcentaje a retener
                    ds = new DataSet();
                    DBFunctions.Request(ds, IncludeSchema.NO, "SELECT pret_codigo, COALESCE(pret_porcenNOdecl,0) FROM pretencion WHERE ttip_codigo='" + tipoPersona + "' AND ttip_proceso='" + tipoProceso + "' AND tret_codigo='RE'");
                    if (ds.Tables[0].Rows.Count == 0)
                        mensajes += "No hay retenciones CREE para este tipo de proceso y persona";
                    else
                    {
                        tipoRetencion.Add(ds.Tables[0].Rows[0][0].ToString());
                        valor = valorFactura * Convert.ToDouble(ds.Tables[0].Rows[0][1]) / 100;
                    //    if (centavos == "N")
                            valor = Math.Round(valor, numDecimales);
                        valoresRetenidos.Add(valor);
                        basesRetenidas.Add(valorFactura);
                    }
                }
            }
        }

        //Función q me permite saber los distintos prefijos y los números de los pedidos incluidos
        // en una factura
        private void Llenar_ArrayList_Pedidos()
        {
            pedidos.Clear();
            if (dtRepuestos != null)
            {
                for (int i = 0; i < dtRepuestos.Rows.Count; i++)
                {
                    if (pedidos.Count == 0)
                        pedidos.Add(dtRepuestos.Rows[i][0].ToString().Trim());
                    else
                    {
                        if (!Buscar_Pedido(dtRepuestos.Rows[i][0].ToString().Trim()))
                            pedidos.Add(dtRepuestos.Rows[i][0].ToString().Trim());
                    }
                }
            }
        }

        /*Función q me permite verificar si un pedido ya existe dentro del arraylist*/
        private bool Buscar_Pedido(string pedido)
        {
            bool encontrado = false;
            for (int i = 0; i < pedidos.Count; i++)
            {
                if (pedido == pedidos[i].ToString())
                {
                    encontrado = true;
                    break;
                }
            }
            return encontrado;
        }

        //Función q me permite mirar si se facturaron items en ese pedido, si hay item facturados
        //devuelve false, si no hay items facturados devuelve true
        private bool Verificar_Cantidad_Facturada(string pedido)
        {
            bool error = false;
            int cont = 0, contItems = 0;
            //Cuento cuantos items distintos trae el pedido
            for (int i = 0; i < this.dtRepuestos.Rows.Count; i++)
            {
                if (dtRepuestos.Rows[i][0].ToString().Trim() == pedido)
                    cont++;
            }
            //Cuento cuantos items no tuvieron cantidad facturada
            for (int i = 0; i < this.dtRepuestos.Rows.Count; i++)
            {
                if (dtRepuestos.Rows[i][0].ToString().Trim() == pedido)
                {
                    if (dtRepuestos.Rows[i][4].ToString() == "0")
                        contItems++;
                }
            }
            //Si los dos conteos son los mismos, devuelvo error
            if (cont == contItems)
                error = true;

            return error;
        }

        //Función q agrega la tabla de dtRepuestos al DataSet con el q se trabaja en toda la clase
        //y ademas mira cual es el origen de la referencia citada y lo agrega a la tabla
        private void Agregar_Tabla(DataSet datos)
        {
            if (!dtRepuestos.Columns.Contains("tori_codigo"))
            {
                dtRepuestos.Columns.Add("tori_codigo", typeof(string));
                string refI = "";
                for (int i = 0; i < dtRepuestos.Rows.Count; i++)
                {
                    Referencias.Guardar(dtRepuestos.Rows[i]["mite_codigo"].ToString(), ref refI, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo='" + dtRepuestos.Rows[i]["plin_codigo"] + "'"));

                    string origen = DBFunctions.SingleData("SELECT tori_codigo FROM mitems WHERE mite_codigo='" + refI + "'");
                    dtRepuestos.Rows[i]["tori_codigo"] = origen;
                    dtRepuestos.AcceptChanges();
                }
                datos.Tables.Add(dtRepuestos.Copy());
            }
            else
                datos.Tables.Add(dtRepuestos.Copy());
        }

        //Cargar retenciones de un datatable
        public void CargarRetenciones(DataTable dtRet)
        {
            valoresRetenidos = new ArrayList();
            tipoRetencion = new ArrayList();
            for (int n = 0; n < dtRet.Rows.Count; n++)
            {
                tipoRetencion.Add(dtRet.Rows[n]["CODRET"].ToString());
                valoresRetenidos.Add(Convert.ToDouble(dtRet.Rows[n]["VALOR"]));
                basesRetenidas.Add(Convert.ToDouble(dtRet.Rows[n]["VALORBASE"]));
            }
        }

        //Guardar retenciones y copiar los arraylists de codigos y valores
        public bool Guardar_Retenciones(bool grabar, ref ArrayList arlV, ref ArrayList arlB, ref ArrayList arlT)
        {
            bool status;
            status = Guardar_Retenciones(grabar);
            arlV = new ArrayList(valoresRetenidos);
            arlB = new ArrayList(basesRetenidas);
            arlT = new ArrayList(tipoRetencion);
            return (status);
        }

        //Cargar retenciones desde una tabla y guardarlas
        public bool Guardar_Retenciones(bool grabar, DataTable dtR)
        {
            bool procesado = false;
            CargarRetenciones(dtR);
            GenerarScriptRetenciones();
            if (grabar)
            {
                if (DBFunctions.Transaction(sqls))
                {
                    this.mensajes += "Bien " + DBFunctions.exceptions + "<br>";
                    procesado = true;
                }
                else
                    this.mensajes += "Mal " + DBFunctions.exceptions + "<br>";
            }
            return procesado;
        }

        //Función q guarda las retenciones calculadas en la base de datos, realiza la actualización en
        //mfacturaproveedor e inserta nuevas filas en mfacturaproveedorretencion, una por cada retención
        //calculada
        public bool Guardar_Retenciones(bool grabar)
        {
            bool procesado = false;
            string causaRetencion = "N";
            try
            {
                causaRetencion = DBFunctions.SingleData("select COALESCE(ccon_causaretenot,'N') from CCONTABILIDAD");
            }
            catch
            {
                causaRetencion = "N";
            }

            if (esVenta)
            {
                if (causaRetencion == "S")
                {

                }
                else
                {
                    procesado = true;
                    return procesado; // SI en el parametro de causar retenciones en la venta en CCONTABILIDAD ESTA en N NO SE CAUSAN LA RETE-FUENTE EN LA VENTA
                }
            }
            this.Calcular_ReteFuente();
            this.Calcular_ReteICA();
            this.Calcular_ReteIVA();
       //     this.Calcular_ReteCREE();   / derogado a partir de Sept.1.2013
            GenerarScriptRetenciones();
            if (grabar)
            {
                if (DBFunctions.Transaction(sqls))
                {
                    this.mensajes += "Bien " + DBFunctions.exceptions + "<br>";
                    procesado = true;
                }
                else
                    this.mensajes += "Mal " + DBFunctions.exceptions + "<br>";
            }
            return procesado;
        }

        //Generar scripts para guardar retenciones
        private void GenerarScriptRetenciones()
        {
            double totalRtns = 0;
            string tipoAutoretenedor = DBFunctions.SingleData("SELECT cemp_indiretefuen FROM cempresa;");

            //     Aqui mostrar el cuadro de las retenciones calculadas y permitir adicionar, cambiar y eliminar retenciones
            //     si el proveedor es del EXTERIOR NO SE CALCULAN RETENCIONES

            string tipoDoc = DBFunctions.SingleData("SELECT tdoc_tipodocu FROM pdocumento WHERE pdoc_codigo='" + this.prefijoFac + "';");

            if (tipoDoc.Equals("FP") || tipoDoc.Equals("NP"))
            {
                if ((Codica != null && Codica != "") || (pc_icaprov != null &&  pc_icaprov != ""))
                {
                    sqls.Add("INSERT INTO mfacturaproveedorretencionica VALUES('" + this.prefijoFac + "'," + this.numeroFac + ",'" + this.Codica + "'," + valorica + "," + valorFactura + ")");
                 }
                for (int i = 0; i < tipoRetencion.Count; i++)
                {
                    sqls.Add("INSERT INTO mfacturaproveedorretencion VALUES('" + this.prefijoFac + "'," + this.numeroFac + ",'" + tipoRetencion[i].ToString() + "'," + Convert.ToDouble(valoresRetenidos[i]).ToString() + "," + Convert.ToDouble(basesRetenidas[i]).ToString() + ")");
                    totalRtns = totalRtns + Convert.ToDouble(valoresRetenidos[i]);
                }
                sqls.Add("UPDATE mfacturaproveedor SET mfac_valorete=" + totalRtns + " WHERE pdoc_codiordepago='" + this.prefijoFac + "' AND mfac_numeordepago=" + this.numeroFac + "");
            }
            else if ((tipoDoc.Equals("FC") || tipoDoc.Equals("NC")) && tipoAutoretenedor != "A" && tipoAutoretenedor != "B" )
            {  
                    for (int i = 0; i < tipoRetencion.Count; i++)
                    {
                        sqls.Add("INSERT INTO mfacturaclienteretencion VALUES('" + this.prefijoFac + "'," + this.numeroFac + ",'" + tipoRetencion[i].ToString() + "'," + Convert.ToDouble(valoresRetenidos[i]).ToString() + "," + Convert.ToDouble(basesRetenidas[i]).ToString() + ")");
                        totalRtns = totalRtns + Convert.ToDouble(valoresRetenidos[i]); 
                    }
                sqls.Add("UPDATE mfacturacliente SET mfac_valorete=" + totalRtns + " WHERE pdoc_codigo='" + this.prefijoFac + "' AND mfac_numedocu=" + this.numeroFac + "");
            }
        }

        private void Comunes(string nit, bool esVenta)
        {
            this.nit = nit;
            this.esVenta = esVenta;
            if (centavos == "S")
                numDecimales = 2;

            tipoNacionalidad = DBFunctions.SingleData("SELECT tnac_tiponaci FROM mnit WHERE mnit_nit='" + nit + "'");

            tipoRetenedorEmpresa = DBFunctions.SingleData("SELECT CEMP_INDIREGIIVA FROM CEMPRESA ");

            if (tipoNacionalidad == "E")
            {
                this.mensajes += "Proveedor EXTRANJERO NO CAUSA RETENCIONES !!! " + DBFunctions.exceptions + "<br>";
            }
            else
            {
                regimenIva = DBFunctions.SingleData("SELECT treg_regiiva        FROM mnit WHERE mnit_nit='" + nit + "'");
                string ivaNit = "";
                try
                {
                    ivaNit = DBFunctions.SingleData("select pret_reteiva from pretencionivanit where mnit_nit = '" + nit + "'; ");
                }
                catch
                {
                    ivaNit = "";
                }
                if (regimenIva == "G" && ivaNit == "")
                {
                    this.mensajes += "Proveedor GRAN CONTRIBUYENTE AUTO-RETENEDOR NO CAUSA RETENCIONES !!! " + DBFunctions.exceptions + "<br>";
                }
                else
                {
                    tipoSociedad = DBFunctions.SingleData("SELECT tsoc_sociedad FROM mnit WHERE mnit_nit='" + nit + "'");
                    switch (tipoSociedad)
                    {
                        case "A"://Anonima
                            tipoPersona = "J";
                            break;
                        case "S"://Anonima
                            tipoPersona = "J";
                            break;
                        case "C"://Comandita
                            tipoPersona = "J";
                            break;
                        case "L"://Limitada
                            tipoPersona = "J";
                            break;
                        case "O"://Oficial
                            tipoPersona = "J";
                            break;
                        case "N"://Particular No Declarante
                            tipoPersona = "N";
                            break;
                        case "E"://Particular No Declarante Empleado de la empresa
                            tipoPersona = "N";
                            break;
                        case "P"://Particular Declarante
                            tipoPersona = "N";
                            break;
                        case "U"://Unipersonal
                            tipoPersona = "N";
                            break;
                    }

                    string auxTabla = (esVenta) ? "mcliente" : "mproveedor",
                            auxCampo = (esVenta) ? "mcli_icaenciud" : "mpro_icaenciud";

                    if (DBFunctions.RecordExist("SELECT * FROM " + auxTabla + " WHERE mnit_nit='" + nit + "'"))
                    {
                        try
                        {
                            tipoRetenedor = DBFunctions.SingleData("SELECT coalesce(trtf_codigo,'T') FROM " + auxTabla + " WHERE mnit_nit='" + nit + "'");
                        }
                        catch
                        {
                            tipoRetenedor ="T";
                        }
                    }
                    else
                        tipoRetenedor = "T";

                    if (DBFunctions.RecordExist("SELECT * FROM " + auxTabla + " WHERE mnit_nit='" + nit + "'"))
                    {
                        if (auxTabla == "mcliente")
                        {
                            ica = DBFunctions.SingleData("SELECT " + auxCampo + " FROM mcliente WHERE mnit_nit='" + nit + "'");
                        }
                        else
                        {
                            ica = DBFunctions.SingleData("SELECT " + auxCampo + " FROM mproveedor WHERE mnit_nit='" + nit + "'");
                        }
                    }
                    else
                        ica = "N";
                }
            }
        }
        #endregion
    }
}
