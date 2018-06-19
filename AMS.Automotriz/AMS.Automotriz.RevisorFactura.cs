using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.Forms;
using AMS.DB;

//Clase para extraer los datos relacionados a una Factura OT.
namespace AMS.Automotriz
{
    public class RevisorFactura
    {
        //Constantes.
        private const double IVA = 0.16;

        //Atributos.
        private string codigoFactura, numeroFactura, codigoOT, numeroOT;
        private DataSet dsDatosOperaciones, dsDatosRepuestos, dsDatosBodegaRep;
        private double sumaOperacionesAprox, sumaOperacionesReal, sumaRepuestosAprox, sumaRepuestosReal, sumaBodegaRepAprox, sumaBodegaRepReal;
        private double sumaIvaRep, sumaIvaOpe, sumaIvaBodRe, subTotalDB, ivaDB;

        //Constructores.
        public RevisorFactura(string codFactura, string numfactura)
        {
            this.codigoFactura = codFactura;
            this.numeroFactura = numfactura;
            this.sumaOperacionesAprox = 0;
            this.sumaOperacionesReal = 0;
            this.sumaRepuestosAprox = 0;
            this.sumaIvaOpe = 0;
            this.sumaRepuestosReal = 0;
            this.sumaIvaRep = 0;
            this.sumaBodegaRepAprox = 0;
            this.sumaBodegaRepReal = 0;
            this.sumaIvaBodRe = 0;
            this.codigoOT = "";
            this.numeroOT = "";
            this.subTotalDB = 0;
            this.ivaDB = 0;
            this.dsDatosOperaciones = new DataSet();
            this.dsDatosRepuestos = new DataSet();
            this.dsDatosBodegaRep = new DataSet();

            CargarValoresFactura();
            CargarOperaciones();
            CargarRepuestos();
            CargarBodegaRepuestos();
        }

        //Metodos.
        //Geters & Seters.
        public DataSet Operaciones { get { return dsDatosOperaciones; } }
        public DataSet Repuestos { get { return dsDatosRepuestos; } }
        public DataSet BodegaRepuestos { get { return dsDatosBodegaRep; } }
        public double SumaOperacionesAprox { get { return sumaOperacionesAprox; } }
        public double SumaOperacionesReal { get { return sumaOperacionesReal; } }
        public double SumaOperacionesIVA { get { return sumaIvaOpe; } }
        public double SumaRepuestosAprox { get { return sumaRepuestosAprox; } }
        public double SumaRepuestosReal { get { return sumaRepuestosReal; } }
        public double SumaRepuestosIVA { get { return sumaIvaRep; } }
        public double SumaBodegaRepuestosAprox { get { return sumaBodegaRepAprox; } }
        public double SumaBodegaRepuestosReal { get { return sumaBodegaRepReal; } }
        public double SumaBodegaRepuestosIVA { get { return sumaIvaBodRe; } }
        public double SubTotal_DB { get { return subTotalDB; } }
        public double IVA_DB { get { return ivaDB; } }
        
        //Obtiene los valores de subtotal e iva de la factura.
        private void CargarValoresFactura()
        {
            DataSet dsFactura = new DataSet();
            DBFunctions.Request(dsFactura, IncludeSchema.NO, "select ROUND(mfac_valofact,2), ROUND(mfac_valoiva,2)  from mfacturacliente where pdoc_codigo='" + codigoFactura + "' and mfac_numedocu =" + numeroFactura + ";");

            subTotalDB = Convert.ToDouble(dsFactura.Tables[0].Rows[0][0]);
            ivaDB = Convert.ToDouble(dsFactura.Tables[0].Rows[0][1]);
        }

        //Carga todas las operaciones relacionadas a la factura. Dejando por fuera las operaciones tipo REP (repuestos de bodega).
        private void CargarOperaciones()
        {
            DataSet dsOrden = new DataSet();
            DBFunctions.Request(dsOrden, IncludeSchema.NO, "select pdoc_codiorde, mord_numeorde from dordencostos where pdoc_codifac='" + codigoFactura + "'  and mfac_numedocu =" + numeroFactura + ";");

            codigoOT = dsOrden.Tables[0].Rows[0][0].ToString();
            numeroOT = dsOrden.Tables[0].Rows[0][1].ToString();

            DBFunctions.Request(dsDatosOperaciones, IncludeSchema.NO,
                "select do.ptem_operacion as CODIGO, pt.ptem_descripcion as DESCRIPCION, '1' as CANTIDAD, ROUND(do.dord_valooper / 1.16,2) as VAL_APROX, ROUND(do.dord_valooper / 1.16,4) as VAL_REAL, pt.ptem_exceiva as USAIVA " +
                "from dordenoperacion do, ptempario pt " +
                "where do.pdoc_codigo='" + codigoOT + "' and do.mord_numeorde=" + numeroOT + " and do.ptem_operacion = pt.ptem_operacion and pt.tope_codigo <> 'REP';");

            CalcularTotales("Op");
        }

        //Carga tos los Repuestos relacionadas a la factura. Dejando por fuera las operaciones tipo REP (repuestos de bodega).
        private void CargarRepuestos()
        {
            DBFunctions.Request(dsDatosRepuestos, IncludeSchema.NO,
                "select di.mite_codigo as CODIGO, mi.mite_nombre as DESCRIPCION, dite_cantidad as CANTIDAD, ROUND(di.dite_valounit * dite_cantidad,2) as VAL_APROX, " +
                "di.dite_valounit * dite_cantidad as VAL_REAL, mi.piva_porciva as USAIVA, di.pdoc_codigo as PREF_TRANSF, di.dite_numedocu as NUME_TRANSFE " +
                "from ditems di, mitems mi,  MORDENTRANSFERENCIA mt where mi.mite_codigo=di.mite_codigo AND mt.pdoc_codigo='" + codigoOT + "' and mt.mord_numeorde =" + numeroOT + " " +
                "and di.pdoc_codigo=mt.pdoc_factura and di.dite_numedocu = mt.mfac_numero order by codigo;");

            VerificarRelacionItemsTransferencias();
            CalcularTotales("Re");
        }

        //Carga tos los Repuestos de BODEGA relacionadas a la factura. Dejando por fuera las operaciones que no sean tipo REP (repuestos de bodega).
        private void CargarBodegaRepuestos()
        {
            //No multiplica por unidades ( dord_tiemliqu) ya que al parecer en DB llega calculado por el valor por el numero de items.
            DBFunctions.Request(dsDatosBodegaRep, IncludeSchema.NO,
                "select do.ptem_operacion as CODIGO, pt.ptem_descripcion as DESCRIPCION, dord_tiemliqu as CANTIDAD,ROUND(do.dord_valooper / 1.16, 2) as VAL_APROX, ROUND(do.dord_valooper / 1.16, 4) as VAL_REAL, pt.ptem_exceiva as USAIVA " +
                "from dordenoperacion do, ptempario pt " +
                "where do.pdoc_codigo='" + codigoOT + "' and do.mord_numeorde=" + numeroOT + " and do.ptem_operacion = pt.ptem_operacion and pt.tope_codigo = 'REP';");

            CalcularTotales("Bo");
        }

        //Realiza la sumatoria de valores Aproximados a 2 decimales, y valores Reales sin redondeo.
        public void CalcularTotales(string tipo)
        {
            switch (tipo)
            {
                case "Op": //Operaciones.
                    sumaOperacionesAprox = 0;
                    sumaOperacionesReal = 0;
                    sumaIvaOpe = 0;
                    for (int k = 0; k < dsDatosOperaciones.Tables[0].Rows.Count; k++)
                    {
                        sumaOperacionesAprox += Convert.ToDouble(dsDatosOperaciones.Tables[0].Rows[k][3]);
                        sumaOperacionesReal += Convert.ToDouble(dsDatosOperaciones.Tables[0].Rows[k][4]);

                        if (dsDatosOperaciones.Tables[0].Rows[k][5].ToString() != "S")
                        {
                            sumaIvaOpe += Convert.ToDouble(dsDatosOperaciones.Tables[0].Rows[k][3]);
                        }
                    }
                    sumaIvaOpe = sumaIvaOpe * IVA;
                    break;
                case "Re": //Repuestos.
                    sumaRepuestosAprox = 0;
                    sumaRepuestosReal = 0;
                    sumaIvaRep = 0;
                    for (int k = 0; k < dsDatosRepuestos.Tables[0].Rows.Count; k++)
                    {
                        sumaRepuestosAprox += Convert.ToDouble(dsDatosRepuestos.Tables[0].Rows[k][3]);
                        sumaRepuestosReal += Convert.ToDouble(dsDatosRepuestos.Tables[0].Rows[k][4]);
                        
                        if(Convert.ToDouble(dsDatosRepuestos.Tables[0].Rows[k][5]) != 0)
                        {
                            sumaIvaRep += Convert.ToDouble(dsDatosRepuestos.Tables[0].Rows[k][3]);
                        }
                    }
                    sumaIvaRep = sumaIvaRep * IVA;
                    break;
                case "Bo": //Bodega Repuestos.
                    sumaBodegaRepAprox = 0;
                    sumaBodegaRepReal = 0;
                    sumaIvaBodRe = 0;
                    for (int k = 0; k < dsDatosBodegaRep.Tables[0].Rows.Count; k++)
                    {
                        sumaBodegaRepAprox += Convert.ToDouble(dsDatosBodegaRep.Tables[0].Rows[k][3]);
                        sumaBodegaRepReal += Convert.ToDouble(dsDatosBodegaRep.Tables[0].Rows[k][4]);

                        if (dsDatosBodegaRep.Tables[0].Rows[k][5].ToString() != "S")
                        {
                            sumaIvaBodRe += Convert.ToDouble(dsDatosBodegaRep.Tables[0].Rows[k][3]);
                        }
                    }
                    sumaIvaBodRe = sumaIvaBodRe * IVA;
                    break;
                default:
                    break;
            }
        }

        public Boolean AjustarFactura()
        {
            Boolean ajusteCorrecto = false;
            double valorNuevo = 0;
            ArrayList sqlUpdates = new ArrayList();

            //Ajuste de Operaciones.
            for (int k = 0; k < dsDatosOperaciones.Tables[0].Rows.Count; k++)
            {
                valorNuevo = Math.Round(Convert.ToDouble(dsDatosOperaciones.Tables[0].Rows[k][4]) * (1 + IVA), 3); //Si cambia a opcion Rows[k][3] Ajuste automatico (Experimental)
                sqlUpdates.Add("UPDATE DORDENOPERACION SET DORD_VALOOPER = " + valorNuevo + " WHERE PDOC_CODIGO = '" + codigoOT + "' AND " +
                               "MORD_NUMEORDE = " + numeroOT + " AND   PTEM_OPERACION = '" + dsDatosOperaciones.Tables[0].Rows[k][0] + "';");
            }

            //Ajuste de Repuestos.
            for (int j = 0; j < dsDatosRepuestos.Tables[0].Rows.Count; j++)
            {
                if (Convert.ToDouble(dsDatosRepuestos.Tables[0].Rows[j][2]) == 1)  //Actualmente solo actualizará Repuestos con cantidad 1.
                {
                    valorNuevo = Convert.ToDouble(dsDatosRepuestos.Tables[0].Rows[j][4]); //Si cambia a opcion Rows[j][3] Ajuste automatico (Experimental)
                    sqlUpdates.Add("UPDATE DITEMS SET DITE_VALOUNIT = " + valorNuevo + " WHERE PDOC_CODIGO = '" + dsDatosRepuestos.Tables[0].Rows[j][6] + "' AND " +
                                   "DITE_NUMEDOCU = " + dsDatosRepuestos.Tables[0].Rows[j][7] + " AND MITE_CODIGO = '" + dsDatosRepuestos.Tables[0].Rows[j][0] + "';");
                }
            }

            //Ajuste Bodega Repuestos.
            for (int m = 0; m < dsDatosBodegaRep.Tables[0].Rows.Count; m++)
            {
                if (Convert.ToDouble(dsDatosBodegaRep.Tables[0].Rows[m][2]) == 1)  //Actualmente solo actualizará Bodegas Repuestos con cantidad 1.
                {
                    valorNuevo = Math.Round(Convert.ToDouble(dsDatosBodegaRep.Tables[0].Rows[m][4]) * (1 + IVA), 3); //Si cambia a opcion Rows[m][3] Ajuste automatico (Experimental)
                    sqlUpdates.Add("UPDATE DORDENOPERACION SET DORD_VALOOPER = " + valorNuevo + " WHERE PDOC_CODIGO = '" + codigoOT + "' AND " +
                                   "MORD_NUMEORDE = " + numeroOT + " AND   PTEM_OPERACION = '" + dsDatosBodegaRep.Tables[0].Rows[m][0] + "';");
                }
            }

            //Ajuste totales finales
            double totalIVA = Math.Round((sumaIvaOpe + sumaIvaRep + sumaIvaBodRe), 4);
            double totalFinal = Math.Round((sumaOperacionesAprox + sumaRepuestosAprox + sumaBodegaRepAprox), 4);
            sqlUpdates.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOFACT = " + totalFinal + ", MFAC_VALOIVA = " + totalIVA +
                           " WHERE PDOC_CODIGO = '" + codigoFactura + "' AND   MFAC_NUMEDOCU = " + numeroFactura + ";");

            if (DBFunctions.Transaction(sqlUpdates))
            {
                ajusteCorrecto = true;
            }

            return ajusteCorrecto;
        }

        public void VerificarRelacionItemsTransferencias()
        {
            double cantidadInicial = 0;
            double cantidadDevuelta = 0;
            double salida = 0;

            //Primero se revisa si existe un registro de devolucion de esta referencia en la tabla ditems y por este documento y con los tipos de movimiento especificos.
            for (int s = 0; s < dsDatosRepuestos.Tables[0].Rows.Count; s++)
            {
                string prefijoTransf = dsDatosRepuestos.Tables[0].Rows[s][6].ToString();
                string numeroTransf = dsDatosRepuestos.Tables[0].Rows[s][7].ToString();
                string codigoItem = dsDatosRepuestos.Tables[0].Rows[s][0].ToString();
                bool existeItemDev = DBFunctions.RecordExist(
                                    "SELECT dite_secuencia FROM ditems WHERE dite_prefdocurefe='" + prefijoTransf + "' AND " +
                                    "dite_numedocurefe=" + numeroTransf + " AND mite_codigo='" + codigoItem + "'"); //AND tmov_tipomovi= (opcional: 81 dev, 80normal)

                if (existeItemDev)
                {
                    DataSet dsCantidadesItems = new DataSet();
                    DBFunctions.Request(dsCantidadesItems, IncludeSchema.NO,
                        "SELECT dite_cantidad FROM ditems WHERE pdoc_codigo='" + prefijoTransf + "' AND dite_numedocu=" + numeroTransf + " and mite_codigo='" + codigoItem + "';" +
                        "SELECT dite_cantidad FROM ditems WHERE dite_prefdocurefe='" + prefijoTransf + "' AND dite_numedocurefe=" + numeroTransf + " AND mite_codigo='" + codigoItem + "';");

                    cantidadInicial = double.Parse(dsCantidadesItems.Tables[0].Rows[0][0].ToString());

                    for (int i = 0; i < dsCantidadesItems.Tables[1].Rows.Count; i++)
                        cantidadDevuelta = cantidadDevuelta + double.Parse(dsCantidadesItems.Tables[1].Rows[i][0].ToString());
                    salida = cantidadInicial - cantidadDevuelta;
                }
                else
                {
                    salida = double.Parse(DBFunctions.SingleData("SELECT dite_cantidad FROM ditems WHERE pdoc_codigo='" + prefijoTransf + "' AND dite_numedocu=" + numeroTransf + " AND mite_codigo = '" + codigoItem + "'"));
                }

                //Si hay por lo menos un item disponible en salida, entonces se toma el item, si da "0" entonces no se toma la transferencia pues se devolvio todo.
                if (salida <= 0)
                {
                    dsDatosRepuestos.Tables[0].Rows.RemoveAt(s);
                    s--;
                }
            }
        }


    }
}