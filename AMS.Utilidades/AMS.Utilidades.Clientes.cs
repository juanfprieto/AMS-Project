using System;
using AMS.DB;

namespace AMS.Utilidades
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Clientes
	{
		public Clientes()
		{
		}
		public static double ConsultarSaldo(string nitCliente)
		{
       //     double saldoFC = 0, saldoNC = 0;
            double saldoCartera = 0;
			string saldoS;
            //saldoS=DBFunctions.SingleData("select sum(MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET-MFAC_VALORETE-MFAC_VALOABON) "+
            //	"from dbxschema.mfacturacliente mf, dbxschema.pdocumento pd "+
            //	"where pd.tdoc_tipodocu='FC' and mf.pdoc_codigo=pd.pdoc_codigo and mnit_nit='"+nitCliente+"';");
            //if(saldoS.Length>0)saldoFC=Convert.ToDouble(saldoS);
            //saldoS=DBFunctions.SingleData("select sum(MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET+MFAC_VALOIVAFLET-MFAC_VALORETE-MFAC_VALOABON) "+
            //	"from dbxschema.mfacturacliente mf, dbxschema.pdocumento pd "+
            //	"where pd.tdoc_tipodocu='NC' and mf.pdoc_codigo=pd.pdoc_codigo and mnit_nit='"+nitCliente+"';");
            //if(saldoS.Length>0)saldoNC=Convert.ToDouble(saldoS);
            //saldoCartera=saldoFC-saldoNC;
            saldoS = DBFunctions.SingleData(@"SELECT COALESCE(SUM(SALDO),0) FROM (
                        select CASE WHEN pd.tdoc_tipodocu = 'FC'
                                    THEN SUm(MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON)
                                    ELSE SUm(MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON) * -1
                                END AS SALDO
                 from dbxschema.mfacturacliente mf, dbxschema.pdocumento pd
                 where mf.pdoc_codigo = pd.pdoc_codigo and mnit_nit = '" + nitCliente + @"'
                 GROUP BY pd.tdoc_tipodocu) AS A; ");
            saldoCartera = Convert.ToDouble(saldoS);
            return (saldoCartera);
		}
		public static double ConsultarSaldoMora(string nitCliente)
		{
        //    double saldoFC = 0, saldoNC = 0;
            double saldoCartera = 0;
			string saldoS;
            //saldoS=DBFunctions.SingleData("select sum(MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET-MFAC_VALOIVAFLET-MFAC_VALORETE-MFAC_VALOABON) "+
            //	"from dbxschema.mfacturacliente mf, dbxschema.pdocumento pd "+
            //	"where pd.tdoc_tipodocu='FC' and mf.pdoc_codigo=pd.pdoc_codigo and mnit_nit='"+nitCliente+"' and "+
            //	"mfac_vence<'"+DateTime.Now.ToString("yyyy-MM-dd")+"';");
            //if(saldoS.Length>0)saldoFC=Convert.ToDouble(saldoS);
            //saldoS=DBFunctions.SingleData("select sum(MFAC_VALOFACT+MFAC_VALOIVA+MFAC_VALOFLET-MFAC_VALOIVAFLET-MFAC_VALORETE-MFAC_VALOABON) "+
            //	"from dbxschema.mfacturacliente mf, dbxschema.pdocumento pd "+
            //	"where pd.tdoc_tipodocu='NC' and mf.pdoc_codigo=pd.pdoc_codigo and mnit_nit='"+nitCliente+"' and "+
            //	"mfac_vence<'"+DateTime.Now.ToString("yyyy-MM-dd")+"';");
            //if(saldoS.Length>0)saldoNC=Convert.ToDouble(saldoS);
            //saldoCartera=saldoFC-saldoNC;
            saldoS = DBFunctions.SingleData(@"SELECT COALESCE(SUM(SALDO),0) FROM (
                        select CASE WHEN pd.tdoc_tipodocu = 'FC'
                                    THEN SUm(MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON)
                                    ELSE SUm(MFAC_VALOFACT + MFAC_VALOIVA + MFAC_VALOFLET + MFAC_VALOIVAFLET - MFAC_VALORETE - MFAC_VALOABON) * -1
                                END AS SALDO
                 from dbxschema.mfacturacliente mf, dbxschema.pdocumento pd
                 where mf.pdoc_codigo = pd.pdoc_codigo and mnit_nit = '" + nitCliente + @"' and mfac_vence<'" + DateTime.Now.ToString("yyyy-MM-dd") + @"'
                 GROUP BY pd.tdoc_tipodocu) AS A; ");
            saldoCartera = Convert.ToDouble(saldoS);
            return (saldoCartera);
		}
	}
}
