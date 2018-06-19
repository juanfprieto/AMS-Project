using System;
using System.Collections;
using System.Data;
using AMS.DB;

namespace AMS.Automotriz
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Tools
	{
        public static void DeterminarTipoOperacionSuma(string codigoOperacion, double valorOperacion, ref double valorElectricidad, ref double valorElectronica, ref double valorLatoneria, ref double valorManoObra, ref double valorPintura, ref double valorTerceros, ref double valorTapiceria, ref double valorTrabajos)
        {
            
			string tipoOperacion = DBFunctions.SingleData("SELECT tope_codigo FROM ptempario WHERE ptem_operacion='"+codigoOperacion+"'");
			switch(tipoOperacion)
			{
					case "ELE" :
								valorElectricidad = valorElectricidad + valorOperacion;
								break;
					case "ETN" :
								valorElectronica = valorElectronica + valorOperacion;
								break;
					case "LAT" :
								valorLatoneria = valorLatoneria + valorOperacion;
								break;
					case "PIN" :
								valorPintura = valorPintura + valorOperacion;
								break;
					case "TER" :
								valorTerceros = valorTerceros + valorOperacion;
								break;
					case "TYV" :
								valorTapiceria = valorTapiceria + valorOperacion;
								break;
					case "VAR" :
								valorTrabajos = valorTrabajos + valorOperacion;
								break;
                    default:
                                tipoOperacion = "MOB";  // TODAS LAS DEMAS ESPECIALIZACIONES POR DEFECTO SON MANO DE OBRA
								valorManoObra = valorManoObra + valorOperacion;
								break;
			}
            
        }
        public static void DeterminarTipoOperacionSuma(string codigoOperacion, double valorOperacion, ref Hashtable tipoOeraciones)
		{
            /*
			string tipoOperacion = DBFunctions.SingleData("SELECT tope_codigo FROM ptempario WHERE ptem_operacion='"+codigoOperacion+"'");
			switch(tipoOperacion)
			{
					case "ELE" :
								valorElectricidad = valorElectricidad + valorOperacion;
								break;
					case "ETN" :
								valorElectronica = valorElectronica + valorOperacion;
								break;
					case "LAT" :
								valorLatoneria = valorLatoneria + valorOperacion;
								break;
					case "MOB" :
								valorManoObra = valorManoObra + valorOperacion;
								break;
					case "PIN" :
								valorPintura = valorPintura + valorOperacion;
								break;
					case "TER" :
								valorTerceros = valorTerceros + valorOperacion;
								break;
					case "TYV" :
								valorTapiceria = valorTapiceria + valorOperacion;
								break;
					case "VAR" :
								valorTrabajos = valorTrabajos + valorOperacion;
								break;
			}
            */
            
            string tipoOperacion = DBFunctions.SingleData("SELECT TTIP.tope_nombre FROM ttipooperaciontaller TTIP,ptempario PTEM " +
                    "WHERE TTIP.tope_codigo=PTEM.tope_codigo AND ptem_operacion='" + codigoOperacion + "' ");

            string cadenaCargos = "ELEETNMOBLATPINTERTYVVAR";
            string cargoMob = "MOB";
            int cargo = cadenaCargos.IndexOf(cargoMob);
            if (cargo == -1)
                tipoOperacion = "MOB";     // el cargo MOB es por defecto el de todos los demas creados por el usuario diferente a los estandar.

            if (tipoOeraciones.ContainsKey(tipoOperacion))
                tipoOeraciones[tipoOperacion] = (double)tipoOeraciones[tipoOperacion] + valorOperacion;

            else
                tipoOeraciones.Add(tipoOperacion, valorOperacion);
		}

        public static string ModificacionVINporTaller(string catalogoViejo, string vinViejo, string catalogoNuevo, string vinNuevo)
        {
            ArrayList sqlStrings = new ArrayList();
            DataSet dsQuery = new DataSet();
            DBFunctions.Request(dsQuery, IncludeSchema.NO, "SELECT pcat_codigo,mcat_vin,mcat_placa,mcat_motor,mnit_nit,mcat_serie,mcat_chasis,pcol_codigo,mcat_anomode,tser_tiposerv,mcat_vencseguobli,mcat_concvend,mcat_venta,mcat_numekilovent,mcat_numeradio,mcat_numeultikilo,mcat_numekiloprom,mcat_categoria,mcat_password,mcat_fechultikilo FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "';" +
                                                         "SELECT pcat_codigo,mcat_vin,mcat_placa,mcat_motor,mnit_nit,mcat_serie,mcat_chasis,pcol_codigo,mcat_anomode,tser_tiposerv,mcat_vencseguobli,mcat_concvend,mcat_venta,mcat_numekilovent,mcat_numeradio,mcat_numeultikilo,mcat_numekiloprom,mcat_categoria,mcat_password,mcat_fechultikilo FROM mcatalogovehiculo WHERE pcat_codigo='" + catalogoNuevo + "' AND mcat_vin='" + vinNuevo + "';");
            DateTime fechaVenceSeguro = Convert.ToDateTime(null);
            DateTime fechaVenta = Convert.ToDateTime(null);
            try { fechaVenceSeguro = Convert.ToDateTime(dsQuery.Tables[0].Rows[0]["mcat_vencseguobli"]); } catch { }
            try { fechaVenceSeguro = Convert.ToDateTime(dsQuery.Tables[0].Rows[0]["mcat_venta"]); } catch { }
            sqlStrings.Add("UPDATE mcatalogovehiculo SET mcat_placa='666ZZZ', mcat_motor='AAABBB777666ZZ.!' WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "'");
            if (dsQuery.Tables[1].Rows.Count > 0)
                sqlStrings.Add("UPDATE mcatalogovehiculo SET mcat_placa='" + dsQuery.Tables[0].Rows[0]["mcat_placa"] + "',mcat_motor='" + dsQuery.Tables[0].Rows[0]["mcat_motor"] + "',mnit_nit='" + dsQuery.Tables[0].Rows[0]["mnit_nit"] + "',mcat_serie='" + dsQuery.Tables[0].Rows[0]["mcat_serie"] + "',mcat_chasis='" + dsQuery.Tables[0].Rows[0]["mcat_chasis"] + "',pcol_codigo='" + dsQuery.Tables[0].Rows[0]["pcol_codigo"] + "',mcat_anomode=" + dsQuery.Tables[0].Rows[0]["mcat_anomode"] + ",tser_tiposerv='" + dsQuery.Tables[0].Rows[0]["tser_tiposerv"] + "',mcat_vencseguobli='" + fechaVenceSeguro.ToString("yyyy-MM-dd") + "',mcat_concvend='" + dsQuery.Tables[0].Rows[0]["mcat_concvend"] + "',mcat_venta='" + fechaVenta.ToString("yyyy-MM-dd") + "',mcat_numekilovent=" + dsQuery.Tables[0].Rows[0]["mcat_numekilovent"] + ",mcat_numeradio='" + dsQuery.Tables[0].Rows[0]["mcat_numeradio"] + "',mcat_numeultikilo=" + dsQuery.Tables[0].Rows[0]["mcat_numeultikilo"] + ",mcat_numekiloprom=" + dsQuery.Tables[0].Rows[0]["mcat_numekiloprom"] + ",mcat_categoria='" + dsQuery.Tables[0].Rows[0]["mcat_categoria"] + "',mcat_password='" + dsQuery.Tables[0].Rows[0]["mcat_password"] + "',mcat_fechultikilo='" + dsQuery.Tables[0].Rows[0]["mcat_fechultikilo"] + "' WHERE pcat_codigo='" + catalogoNuevo + "' AND mcat_vin='" + vinNuevo + "'");
            else
                sqlStrings.Add("INSERT INTO mcatalogovehiculo(pcat_codigo,mcat_vin,mcat_placa,mcat_motor,mnit_nit,mcat_serie,mcat_chasis,pcol_codigo,mcat_anomode,tser_tiposerv,mcat_vencseguobli,mcat_concvend,mcat_venta,mcat_numekilovent,mcat_numeradio,mcat_numeultikilo,mcat_numekiloprom,mcat_categoria,mcat_password,mcat_fechultikilo, mcat_matricula) VALUES('" + catalogoNuevo + "','" + vinNuevo + "','" + dsQuery.Tables[0].Rows[0]["mcat_placa"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_motor"] + "','" + dsQuery.Tables[0].Rows[0]["mnit_nit"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_serie"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_chasis"] + "','" + dsQuery.Tables[0].Rows[0]["pcol_codigo"] + "'," + dsQuery.Tables[0].Rows[0]["mcat_anomode"] + ",'" + dsQuery.Tables[0].Rows[0]["tser_tiposerv"] + "','" + fechaVenceSeguro.ToString("yyyy-MM-dd") + "','" + dsQuery.Tables[0].Rows[0]["mcat_concvend"] + "','" + fechaVenta.ToString("yyyy-MM-dd") + "'," + dsQuery.Tables[0].Rows[0]["mcat_numekilovent"] + ",'" + dsQuery.Tables[0].Rows[0]["mcat_numeradio"] + "'," + dsQuery.Tables[0].Rows[0]["mcat_numeultikilo"] + "," + dsQuery.Tables[0].Rows[0]["mcat_numekiloprom"] + ",'" + dsQuery.Tables[0].Rows[0]["mcat_categoria"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_password"] + "','" + dsQuery.Tables[0].Rows[0]["mcat_fechultikilo"] + "',null)");
            sqlStrings.Add("UPDATE morden SET mcat_vin='" + vinNuevo + "' WHERE mcat_vin='" + vinViejo + "'");
            sqlStrings.Add("UPDATE mubicacionvehiculo SET pcat_codigo='" + catalogoNuevo + "',mcat_vin='" + vinNuevo + "' WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "'");
            sqlStrings.Add("UPDATE mvehiculo SET mcat_vin='" + vinNuevo + "' WHERE mcat_vin='" + vinViejo + "'");
            sqlStrings.Add("delete from mcatalogovehiculo WHERE pcat_codigo='" + catalogoViejo + "' AND mcat_vin='" + vinViejo + "'");
            string status = String.Empty;
            if (!DBFunctions.Transaction(sqlStrings))
                status = DBFunctions.exceptions;
            return status;
        }
    }
}
