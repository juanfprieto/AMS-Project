using System;
using System.Collections;
using System.Data;
using AMS.DB;

namespace AMS.Tools
{
    public class Referencias
    {
        //Referencia nueva devuelve lo que se debe grabar despues de crear
        //						codigoDigitado  ref. NuevoCodigo   Bodega-Marca
        public static bool Crear(string ICod, ref string NICod, string Bdga)
        {
            switch (Bdga)
            {
                case "MZ":	//Mazda
                    if (ICod.Length > 12 || ICod.Length < 9)
                        return false;
                    NICod = ICod.Substring(4, 2) + "-" + ICod.Substring(6, 3);//Grupo, consecutivo
                    if (ICod.Length == 10 || ICod.Length == 12)//Hay Letra
                        NICod += "-" + ICod.Substring(9, 1);
                    NICod += "-" + ICod.Substring(0, 4);//Tipo
                    if (ICod.Length == 11 || ICod.Length == 12)//Color
                        NICod += "-" + ICod.Substring(ICod.Length - 3, 2);
                    break;
                case "HY":	//Hiunday-Toyota
                    NICod = ICod;
                    break;
                case "FR":	//Ford
                    if (ICod.EndsWith("-"))
                        ICod = ICod.Substring(0, ICod.Length - 1);
                    string[] split = ICod.Split('-');
                    if (split.Length != 3 && split.Length != 2) return (false);
                    if (split[0] != "")
                        if (split[0].Length < 1 || split[0].Length > 10) return (false);
                    if (split[1].Length < 1 || split[1].Length > 10) return (false);
                    if (split[1] == "" && split[0] == "") return (false);
                    if (split.Length == 3 && (split[2].Length < 1 || split[2].Length > 8)) return (false);
                    NICod = split[1] + "-" + split[0] + "-";
                    if (split.Length == 3)
                        NICod += split[2];
                    break;
                case "CH":	//Chevrolet
                    if (ICod.Length > 17) return (false);
                    string zs = "00000000000000000";
                    NICod = zs.Substring(0, 17 - ICod.Length) + ICod;
                    break;
                default:	//General
                    NICod = ICod;
                    break;
            }
            return (true);
        }

        //Recibe editado Devuelve Referencia a grabar despues de editar
        public static bool Guardar(string ICod, ref string NGCod, string Bdga)
        {
            string[] split;
            switch (Bdga)
            {
                case "MZ":	//Mazda
                    string lt = "", cl = "";
                    split = ICod.Split('-');
                    if (split.Length < 3 || split.Length > 5) return (false);//FAltan o sobran datos
                    if (split[0].Length != 4 || split[1].Length != 2) return (false);//Tamaño de datos basicos no corresponde
                    if (split[2].Length != 4) if (split[2].Length != 3) if (split[2].Length != 2) return (false);
                    if (split.Length == 4)//Solo letra o solo color
                        if (split[3].Length > 2) return (false);//Ni letra ni color
                        else
                            if (split[3].Length == 1) lt = split[3];//Letra
                            else cl = split[3];//Color
                    if (split.Length == 5)//Letra y color
                        if (split[3].Length != 1 || split[4].Length != 2) return (false);//No validos
                        else { cl = split[4]; lt = split[3]; }
                    NGCod = split[1] + "-" + split[2];//Grupo, consec
                    if (lt.Length > 0) NGCod += "-" + lt;//Letra
                    NGCod += "-" + split[0];//Tipo
                    if (cl.Length > 0)
                        NGCod += "-" + cl;//Color
                    break;
                case "HY":	//Hiunday-Toyota
                    if (ICod.Length <= 5) return (false);
                    if (ICod[5]!='-') return (false);
                    NGCod = ICod.Remove(5,1);
                    break;
                case "FR":	//Ford
                    if (ICod.EndsWith("-"))
                        ICod = ICod.Substring(0, ICod.Length - 1);
                    split = ICod.Split('-');
                    if (split.Length != 3 && split.Length != 2) return (false);
                    if (split[0] != "")
                        if (split[0].Length < 1 || split[0].Length > 10) return (false);
                    if (split[1].Length < 1 || split[1].Length > 10) return (false);
                    if (split[1] == "" && split[0] == "") return (false);
                    if (split.Length == 3 && (split[2].Length < 1 || split[2].Length > 8)) return (false);
                    NGCod = split[1] + "-" + split[0] + "-";
                    if (split.Length == 3)
                        NGCod += split[2];
                    break;
                case "CH":	//Chevrolet
                    if (ICod.Length > 17) return (false);
                    /*string zs="00000000000000000";
                    NGCod=zs.Substring(0,17-ICod.Length)+ICod;*/
                    NGCod = ICod.PadLeft(18, '0');
                    break;
                case "GR": //Generico
                    NGCod = ICod;
                    break;
                case "RN": //Renault
                    NGCod = ICod;
                    break;
                default:	//NO Definida
                    NGCod = ICod;
                    return false;
            }
            return (true);
        }


        //Referencia al editar, convierte grabado en edicion
        public static bool Editar(string ICod, ref string NECod, string Bdga)
        {
            string[] split;
            switch (Bdga)
            {
                case "MZ":	//Mazda
                    split = ICod.Split('-');
                    if (split.Length < 3 || split.Length > 5) return (false);
                    if (split.Length == 3) { NECod = split[2] + "-" + split[0] + "-" + split[1]; }
                    if (split.Length == 4)
                        if (split[3].Length == 2)//hay color
                        { NECod = split[2] + "-" + split[0] + "-" + split[1] + "-" + split[3]; }
                        else //hay letra
                        { NECod = split[3] + "-" + split[0] + "-" + split[1] + "-" + split[2]; }
                    if (split.Length == 5) NECod = split[3] + "-" + split[0] + "-" + split[1] + "-" + split[2] + "-" + split[4];
                    break;
                case "HY":	//Hiunday-Toyota
                    if (ICod.Length <= 5) return (false);
                    NECod = ICod.Substring(0, 5) + "-" + ICod.Substring(5);
                    break;
                case "FR":	//Ford
                    split = ICod.Split('-');
                    if (split.Length != 3 && split.Length != 2) return (false);
                    if (split[0].Length < 1 || split[0].Length > 10) return (false);
                    if (split[1] != "")
                        if (split[1].Length < 1 || split[1].Length > 10) return (false);
                    if (split[1] == "" && split[0] == "") return (false);
                    //if(split.Length==3&&(split[2].Length<1||split[2].Length>8))return(false);
                    NECod = split[1] + "-" + split[0] + "-";
                    if (split.Length == 3)
                        NECod += split[2];
                    break;
                case "CH":	//Chevrolet
                    NECod = ICod;
                    break;
                default:	//General
                    NECod = ICod;
                    break;
            }
            return (true);
        }

        public static bool RevisionSustitucion(ref string codI, string lineaBodega)
        {
            bool status = false;
            if (!DBFunctions.RecordExist("SELECT * FROM mitems WHERE mite_codigo='" + codI + "'"))
            {
                //Aqui revisamos si fue codigo anterior de alguna otra referencia 
                if (DBFunctions.RecordExist("SELECT * FROM msustitucion WHERE msus_codanterior='" + codI + "' AND plin_codigo='" + lineaBodega + "'"))
                {
                    codI = DBFunctions.SingleData("SELECT msus_codactual FROM msustitucion WHERE msus_codanterior='" + codI + "' AND plin_codigo='" + lineaBodega + "'");
                    status = true;
                }
            }
            else
                status = true;
            return status;
        }

        //Funcion que nos permite consultar la disponibilidad de un item dentro de un almacen espefico
        //recibe como parametro de entrada el codigo del item, codigo de almacen y cantidad solicitada
        //y retorna lo que se le puede asignar, si ahi disponibilidad le asigna lo pedido, sino lo que se
        //le pueda dar

        public static double ConsultarAsignacion(string codI, string codAlm, double cantSolicitada)
        {
            double cantidadAsignada = cantSolicitada;
            double cantidadDisponible = 0;
            string anoInv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
            try { cantidadDisponible = Convert.ToDouble(DBFunctions.SingleData("SELECT case when mi.tori_codigo = 'Z' THEN 1000 ELSE COALESCE(msal_cantactual - msal_cantasig,0) END FROM mitems mi left join msaldoitemalmacen ms on mi.mite_codigo= ms.mite_codigo AND palm_almacen='" + codAlm + "' AND pano_ano=" + anoInv + " where mi.mite_codigo='" + codI + "' ").Trim()); }
            catch { }
            if (cantidadDisponible < 0)  // la disponibilidad nunca puede ser negativa
                cantidadDisponible = 0;
            if (cantSolicitada >= cantidadDisponible)
                cantidadAsignada = cantidadDisponible;
            return cantidadAsignada;
        }

        //Funcion que nos permite consultar la disponibilidad de un item dentro de un almacen espefico
        //recibe como parametro de entrada el codigo del item y codigo de almacen
        //y retorna la diposnibilidad. 
        //Si el indicativo es igual a 0 tomamos msal_cantactual - msal_cantasig FROM msaldoitemalmacen
        //Si el indicativo es igual a 1 tomamos msal_cantactual - msal_cantasig - msal_cantpendiente + msal_canttransito FROM msaldoitemalmacen

        public static double ConsultarDisponibilidad(string codI, string codAlm, int indicativo)
        {
            string anoInv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
            double disponibilidad = 0;
            if (indicativo == 0)
            {
                try { disponibilidad = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual - msal_cantasig FROM msaldoitemalmacen WHERE mite_codigo='" + codI + "' AND palm_almacen='" + codAlm + "' AND pano_ano=" + anoInv + "").Trim()); }
                catch { }
            }
            else if (indicativo == 1)
            {
                try { disponibilidad = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual - msal_cantasig - msal_cantpendiente + msal_canttransito FROM msaldoitemalmacen WHERE mite_codigo='" + codI + "' AND palm_almacen='" + codAlm + "' AND pano_ano=" + anoInv + "").Trim()); }
                catch { }
            }
            return disponibilidad;
        }

        public static double ConsultarDisponibilidad(string codI, string codAlm, string anoConsulta, int indicativo)
        {
            double disponibilidad = 0;
            if (indicativo == 0)
            {
                try { disponibilidad = Convert.ToDouble(DBFunctions.SingleData("SELECT case when mi.tori_codigo = 'Z' THEN 1000 ELSE COALESCE(msal_cantactual - msal_cantasig,0) END FROM mitems mi left join msaldoitemalmacen ms on mi.mite_codigo= ms.mite_codigo AND palm_almacen='" + codAlm + "' AND pano_ano=" + anoConsulta + " where mi.mite_codigo='" + codI + "' ").Trim()); }
                catch { }  // los items de ORIGEN Z corresponde a SERICIOS y NO MANEJAS SALDO DE STOCK
            }
            else if (indicativo == 1)
            {
                try { disponibilidad = Convert.ToDouble(DBFunctions.SingleData("SELECT case when mi.tori_codigo = 'Z' THEN 1000 ELSE COALESCE(msal_cantactual - msal_cantasig - msal_cantpendiente + msal_canttransito,0) END FROM mitems mi left join msaldoitemalmacen ms on mi.mite_codigo= ms.mite_codigo AND palm_almacen='" + codAlm + "' AND pano_ano=" + anoConsulta + " where mi.mite_codigo='" + codI + "' ").Trim()); }
                catch { } 
            }
            if (disponibilidad < 0)
                disponibilidad = 0;  // para no permitir asignaciones negativas
            return disponibilidad;
        }

        //Funcion que nos permite consultar cual es el disponible de una referencia, revise como parametros de entrada
        //el codigo del item a consultar,un mes y un año especifico para realizar la consulta, un entero referencia que guarda el pedido sugerido,
        //la funcion retorna un entero indicador del tipo de sugerido que es el item : 
        //Alta Rotacion : 10
        //Media Rotacion : 20
        //Estudio Profundo : 30
        //Sobre Stock : 40
        public static int ConsultarSugerido(DataRow drItem, int mes, int ano, int dia, ref int sugerido, ref double demandaPromedioSalida, DataSet dsParaInven)
        {
            int frecuenciaPedido = 0;
            int mesesDemanda = 0;
            double demandaPromedio = 0, cicloReposicion = 0, stockSeguridad = 0, Qactual = 0, Qtransito = 0, Qbackorder = 0, costoPromedio = 0, valorFrontera = 0;
            bool indicadorEstudioProfundo = false;
            string codI = drItem["mite_codigo"].ToString();
            //Traemos el nit del proveedor de este item
            string nitProveedor = drItem["mnit_nit"].ToString();
            //Ahora determinamos como se van a tomar los acumulados de los items 
            int mesPivote = mes - 1;
            int anoPivote = ano;
            double Qminimo = 0, QDisponible = 0, Qmaximo = 0;
            
                //Traemos la cantidadactual, 
                //la cantidad en transicion y el backorder de clientes
                //try { Qactual = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_cantactual - msal_cantasig + msal_unidtrans FROM msaldoitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + ano + "")); }
                //catch { }
            Qactual = Convert.ToDouble(drItem["qactual"]);
            //try { Qtransito = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_unidtrans FROM msaldoitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + ano + "")); }
            //catch { }
            Qtransito = Convert.ToDouble(drItem["qtrans"]);
            //try { Qbackorder = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_unidpendi FROM msaldoitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + ano + "")); }
            //catch { }
            Qbackorder = Convert.ToDouble(drItem["qbo"]);
            Qbackorder = 0; // TEMPOARAL PORQUE ESTA VARIABLE NO ESTA PERFECTAMENTE BALANCEADA CON LOS DATOS DE DPEDIDOITEM
            //try { costoPromedio = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom FROM msaldoitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + ano + "")); }
            //catch { }
            costoPromedio = Convert.ToDouble(drItem["msal_costprom"]);
            //try { cicloReposicion = Convert.ToDouble(DBFunctions.SingleData("SELECT mpro_clicrepo FROM mproveedor WHERE mnit_nit='" + nitProveedor + "'")); }
            //catch { cicloReposicion = Convert.ToDouble(dsParaInven.Tables[0].Rows[0][8]); }
            cicloReposicion = Convert.ToDouble(drItem["mpro_clicrepo"]);
            //try { frecuenciaPedido = Convert.ToInt32(DBFunctions.SingleData("SELECT mpro_frecpedimes FROM mproveedor WHERE mnit_nit='" + nitProveedor + "'")); }
            //catch { frecuenciaPedido = Convert.ToInt32(dsParaInven.Tables[0].Rows[0][7]); }
            frecuenciaPedido = Convert.ToInt32(drItem["mpro_frecpedimes"]);
            //try { stockSeguridad = Convert.ToDouble(DBFunctions.SingleData("SELECT mpro_stocksegu FROM mproveedor WHERE mnit_nit='" + nitProveedor + "'")); }
            //catch { stockSeguridad = Convert.ToDouble(dsParaInven.Tables[0].Rows[0][9]); }
            stockSeguridad = Convert.ToDouble(drItem["mpro_stocksegu"]);
            try { valorFrontera = Convert.ToInt32(dsParaInven.Tables[0].Rows[0][19]); }
            catch { valorFrontera = 1000000; }

            //if (DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo=(SELECT plin_codigo FROM mitems WHERE mite_codigo='" + codI + "')") != "RN")
            if (drItem["plin_tipo"].ToString() != "RN")
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                ///////////////Manejo de Sugerido Referencias Mazda/////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////
                //Debemos cargar los 6 ponderadores 
                double[] ponderadores = new double[6];
                ponderadores[0] = (Convert.ToDouble(dsParaInven.Tables[0].Rows[0][13]) / 100); //Ponderador 1
                ponderadores[1] = (Convert.ToDouble(dsParaInven.Tables[0].Rows[0][14]) / 100); //Ponderador 2
                ponderadores[2] = (Convert.ToDouble(dsParaInven.Tables[0].Rows[0][15]) / 100); //Ponderador 3
                ponderadores[3] = (Convert.ToDouble(dsParaInven.Tables[0].Rows[0][16]) / 100); //Ponderador 4
                ponderadores[4] = (Convert.ToDouble(dsParaInven.Tables[0].Rows[0][17]) / 100); //Ponderador 5
                ponderadores[5] = (Convert.ToDouble(dsParaInven.Tables[0].Rows[0][18]) / 100); //Ponderador 6
                //Con el nit del proveedor ahora traemos el valor del ciclo de reposicion y la frecuencia de pedido, si los valores son nulos en la base de datos traemos los valores por defecto que almacenamos en cinventario
           
                double[] acumuladosMeses = new double[6];
                //Ahora determinamos desde que mes tomamos el semestre si desde el actual o el anterior
                //Si el acumulado del mes actual es mayor que el del mes anterior el pivote es el mes actual
                //Si el acumulado del mes anterior es mayor que el del mes actual el pivote es el mes anterior
                if (Convert.ToDouble(drItem["D0"].ToString()) >= Convert.ToDouble(drItem["D1"].ToString()))
                {
                    acumuladosMeses[0] = Convert.ToDouble(drItem["D0"].ToString());
                    acumuladosMeses[1] = Convert.ToDouble(drItem["D1"].ToString());
                    acumuladosMeses[2] = Convert.ToDouble(drItem["D2"].ToString());
                    acumuladosMeses[3] = Convert.ToDouble(drItem["D3"].ToString());
                    acumuladosMeses[4] = Convert.ToDouble(drItem["D4"].ToString());
                    acumuladosMeses[5] = Convert.ToDouble(drItem["D5"].ToString());
                }
                else
                {
                    acumuladosMeses[0] = Convert.ToDouble(drItem["D1"].ToString());
                    acumuladosMeses[1] = Convert.ToDouble(drItem["D2"].ToString());
                    acumuladosMeses[2] = Convert.ToDouble(drItem["D3"].ToString());
                    acumuladosMeses[3] = Convert.ToDouble(drItem["D4"].ToString());
                    acumuladosMeses[4] = Convert.ToDouble(drItem["D5"].ToString());
                    acumuladosMeses[5] = Convert.ToDouble(drItem["D6"].ToString());
                }
                //Ya tenemos el mes de pivote y en que año. Ahora debemos calcular cual es la demanda promedio para los ultimos seis meses desde el mes de pivote
                for (int i = 0; i < 6; i++)
                {
                    if(acumuladosMeses[i]>0)
                       mesesDemanda += 1;
                    demandaPromedio += acumuladosMeses[i] * ponderadores[5 - i];
                }
                indicadorEstudioProfundo = Referencias.RevisionDemandaMeses(acumuladosMeses, 6);
                demandaPromedioSalida = demandaPromedio;
                //Ahora calculamos el Qminimo, QDisponible y Qmaximo
                if (cicloReposicion < 0 || cicloReposicion > 10)
                    cicloReposicion = 1;
                if (frecuenciaPedido < 1 || frecuenciaPedido > 15)
                    frecuenciaPedido = 1;
           
                Qminimo     = demandaPromedio * cicloReposicion;
                QDisponible = Qactual + Qtransito - Qbackorder;
                Qmaximo     = Qminimo + (demandaPromedio / frecuenciaPedido);
                if (QDisponible >= Qminimo)
                {
                    sugerido = 0;
                    return (40);  //Sobre Stock
                }
                else
                {
                    sugerido = (int)Math.Round(Qmaximo - QDisponible);
                    if (costoPromedio > valorFrontera || indicadorEstudioProfundo)
                        return (30);
                    else if (mesesDemanda >= 4)
                            return (10);
                        else if (mesesDemanda < 4)
                                return (20);
                }
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                ///////////////Manejo de Sugerido Referencias Renault///////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////
                if (mesPivote <= 0)
                {
                    mesPivote = 12;
                    anoPivote = anoPivote - 1;
                }
                //traemos el acumulado de la demanda de los ultimos seis meses sin contar el mes seleccionado
                double acumuladosDemandasMeses = 0;
                double[] acumuladosMeses = new double[6];
                for (int i = 0; i < 6; i++)
                {
                    double acumMes = 0;
                    try { acumMes = Convert.ToDouble(DBFunctions.SingleData("SELECT mACU_cantidad FROM mACUMULADOitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + anoPivote + " AND pmes_mes=" + mesPivote + " AND TMOV_TIPOMOVI = 70 ")); }
                    catch { }
                    if (acumMes >= 0)
                    {
                        acumuladosDemandasMeses += acumMes;
                        mesesDemanda += 1;
                    }
                    acumuladosMeses[i] = acumMes;
                    mesPivote = mesPivote - 1;
                    if (mesPivote <= 0)
                    {
                        mesPivote = 12;
                        anoPivote = anoPivote - 1;
                    }
                }
                indicadorEstudioProfundo = Referencias.RevisionDemandaMeses(acumuladosMeses, 6);
                //Ahora este acumulado lo dividimos en 6(meses) y luego en 22(cantidad dias manejados sofasa)
                acumuladosDemandasMeses = acumuladosDemandasMeses / 6;
                acumuladosDemandasMeses = acumuladosDemandasMeses / 22;
                //Ahora traemos la cantidad acumulada del mes y que año que entraron como parametro
                //Si el dia de consulta es mayor a 22 se divide por 22 sino por la cantidad de dias transcurridoas
                double cantidadMesActual = 0;
                try { cantidadMesActual = Convert.ToDouble(DBFunctions.SingleData("SELECT mACU_cantidad FROM mACUMULADOitem WHERE mite_codigo='" + codI + "' AND pano_ano=" + ano + " AND pmes_mes=" + mes + " AND TMOV_TIPOMOVI = 70 ")); }
                catch { }
                if (dia > 22)
                    cantidadMesActual = cantidadMesActual / 22;
                else
                    cantidadMesActual = cantidadMesActual / dia;
                //Ahora se suma la demandapromedio diara del mes actual con las demanda diaria promedia de los ultimos seis meses
                double demandaPromedioDiaria = cantidadMesActual + acumuladosDemandasMeses;
                if (frecuenciaPedido == 0)
                    frecuenciaPedido = 1;
                Qmaximo = demandaPromedioDiaria * ((1 / frecuenciaPedido) + cicloReposicion + stockSeguridad);
                Qminimo = Qmaximo;
                QDisponible = Qactual + Qtransito - Qbackorder;
                if (QDisponible > Qmaximo)
                {
                    sugerido = 0;
                    return (40);  //Sobre Stock
                }
                else
                {
                    sugerido = (int)Math.Round(Qmaximo - QDisponible);
                    if (costoPromedio > valorFrontera || indicadorEstudioProfundo)
                        return (30);
                    else if (mesesDemanda >= 4)
                            return (10);
                        else if (mesesDemanda < 4)
                                return (20);
                }
            }
            return 0;
        }


        public static int CalcularMinimoDisponible(string codI, uint mes, int ano)
        {
            int Qminimo = 0;
            double demandaPromedio = 0, cicloReposicion = 0;
            //Consultamos todos los parametros que se encuentran almacenados dentro de la tabla cinventario
            DataSet dsParaInven = new DataSet();
            DataSet dsItems = new DataSet();
            DBFunctions.Request(dsParaInven, IncludeSchema.NO, "SELECT * FROM cinventario");

            string consulta_sugeridos =
            "select mi.mite_codigo, mi.mnit_nit, " +
            " coalesce((msi.msal_cantactual - msi.msal_cantasig + msi.msal_unidtrans),0) qactual, " +
            " coalesce(msi.msal_unidtrans,0) qtrans, " +
            " coalesce(msi.msal_unidpendi,0) qbo, " +
            " coalesce(msi.msal_costprom,0) msal_costprom, " +
            " coalesce(mpr.mpro_clicrepo,0) mpro_clicrepo, " +
            " coalesce(mpr.mpro_frecpedimes,0) mpro_frecpedimes, " +
            " coalesce(mpr.mpro_stocksegu,0) mpro_stocksegu, " +
            " pln.plin_tipo, " +
            " coalesce(mi.mite_stokmini,0) mite_stokmini " +
            "from  plineaitem pln, mitems mi " +
            " left join msaldoitem msi on mi.mite_codigo=msi.mite_codigo " +
            " left join mproveedor mpr on mpr.mnit_nit=mi.mnit_nit " +
            "where mi.tvig_tipovige='V' AND msi.pano_ano={0} AND pln.plin_codigo=mi.plin_codigo " +
                //     " AND mi.tORI_CODIGO NOT IN ('X','T') "+
            " ";

            DBFunctions.Request(dsItems, IncludeSchema.NO, String.Format(consulta_sugeridos, ano) + " AND mi.mite_codigo='" + codI + "'");
         //  if (dsItems.Tables[0].Rows.Count > 0)
         //     Referencias.ConsultarSugerido(dsItems.Tables[0].Rows[0], mes, ano, DateTime.Now.Day, ref sugerido, ref demandaPromedio, dsParaInven);
            //Ahora traemos el ciclo de reposicion 
            try { cicloReposicion = Convert.ToDouble(dsItems.Tables[0].Rows[0]["mpro_clicrepo"]); }
            catch { cicloReposicion = Convert.ToDouble(dsParaInven.Tables[0].Rows[0]["cinv_ciclrepo"]); }
            Qminimo = Convert.ToInt32(demandaPromedio * cicloReposicion);
            if (Qminimo == 0)//Si el minimo es igual a 0, lo traemos de mitems
            {
                try { Qminimo = Convert.ToInt32(dsParaInven.Tables[0].Rows[0]["mite_stokmini"]); }
                catch { Qminimo = 0; }
            }
            return Qminimo;
        }

        //Funcion para la consulta del semaforo de disponiblidad de un item dentro de un almacen 
        //Si es 0 : No hay - Rojo
        //Si es 1 : Cantidad Menor a Minimo - Amarillo
        //Si es 2 : Cantidad Superior a Minimo - Verde
        public static int ConsultaSemaforoDisponibilidad(string codI, string codAlm, uint mes, int ano)
        {
            int indentificadorEstado = -1;
            double disponibilidad = Referencias.ConsultarDisponibilidad(codI, codAlm, ano.ToString(), 0);
            int minimoDisponible = Referencias.CalcularMinimoDisponible(codI, mes, ano);
            if (disponibilidad == 0)
            {
                try
                {
                    disponibilidad = Convert.ToDouble(DBFunctions.SingleData("SELECT case when mi.tori_codigo = 'Z' THEN 0 ELSE COALESCE(msal_canttransito,0) END FROM CINVENTARIO CI, mitems mi, msaldoitemalmacen ms WHERE mi.mite_codigo = ms.mite_codigo AND palm_almacen='" + codAlm + "' AND MS.pano_ano=CI.PANO_ANO AND mi.mite_codigo='" + codI + "' ").Trim());
                }
                catch
                {
                    disponibilidad = 0;
                }
                if (disponibilidad > 0) // hay transito unicamente
                { 
                    indentificadorEstado = 3;
                    disponibilidad = 0;
                }
                else indentificadorEstado = 0;
            }
            else if (disponibilidad >= 1 && disponibilidad <= Convert.ToDouble(minimoDisponible))
                    indentificadorEstado = 2;
            else if (disponibilidad > Convert.ToDouble(minimoDisponible))
                    indentificadorEstado = 1;
            return indentificadorEstado;
        }

        public static bool RevisionDemandaMeses(double[] demandas, int cantidadMeses)
        {
            bool anomalia = false;
            double demandaPromedioMatematico = 0;
            for (int i = 0; i < cantidadMeses; i++)
                demandaPromedioMatematico += demandas[i];
            demandaPromedioMatematico = demandaPromedioMatematico / cantidadMeses;
            //Ahora volvemos a recorrer el arreglo de las demandas mensuales y revisamos si alguna es mayor que el promedio matematico
            for (int i = 0; i < cantidadMeses; i++)
                if (demandas[i] > (demandaPromedioMatematico * 3))
                    anomalia = true;
            return anomalia;
        }

        public static double DemandaMes(string codI, int ano, uint mes)
        {
            double demanda = 0;
            try { demanda = Convert.ToDouble(DBFunctions.SingleData("SELECT macu_cantidad FROM macumuladoitem WHERE mite_codigo='" + codI + "' AND tmov_tipomovi=70 AND pano_ano=" + ano + " AND pmes_mes=" + mes + "")); }
            catch { }
            return demanda;
        }
    }
}
