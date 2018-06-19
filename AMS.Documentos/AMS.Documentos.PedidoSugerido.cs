using System;
using System.Collections;
using System.Data;
using AMS.DB;
using AMS.Tools;

namespace AMS.Documentos
{
	public class PedidoSugerido
	{
		public static string CONSULTA_SUGERIDOS=
            "select mi.mite_codigo, mi.mnit_nit, "+
            " coalesce((msi.msal_cantactual - msi.msal_cantasig + msi.msal_unidtrans),0) qactual, "+
            " coalesce(msi.msal_unidtrans,0) qtrans, "+
            " coalesce(msi.msal_unidpendi,0) qbo, "+
            " coalesce(msi.msal_costprom,0) msal_costprom, "+
            " coalesce(mpr.mpro_clicrepo,0) mpro_clicrepo, "+
            " coalesce(mpr.mpro_frecpedimes,0) mpro_frecpedimes, "+
            " coalesce(mpr.mpro_stocksegu,0) mpro_stocksegu, "+
            " pln.plin_tipo, "+
            " coalesce(mi.mite_stokmini,0) mite_stokmini " +
            "from  plineaitem pln, mitems mi " +
            " left join msaldoitem msi on mi.mite_codigo=msi.mite_codigo " +
            " left join mproveedor mpr on mpr.mnit_nit=mi.mnit_nit " +
            "where mi.tvig_tipovige='V' AND msi.pano_ano={0} AND pln.plin_codigo=mi.plin_codigo " +
       //     " AND mi.tORI_CODIGO NOT IN ('X','T') "+
            " ";

           public static string CrearPedidoSugerido(int mes, int ano, string linea)
		{
			string processMsg = "";
			ArrayList sqlStrings = new ArrayList();
			//Lo que primero debemos hacer es eliminar todos los registros que se encuentren dentro de la tabla de msugeridoitem
			sqlStrings.Add("DELETE FROM msugeridoitem");
            
            //Consultamos todos los parametros que se encuentran almacenados dentro de la tabla cinventario
            DataSet dsParaInven = new DataSet();
            DBFunctions.Request(dsParaInven, IncludeSchema.NO, "SELECT * FROM cinventario");
            int anoI = Convert.ToInt16(dsParaInven.Tables[0].Rows[0]["pano_ano"].ToString());
            int mesI = Convert.ToInt16(dsParaInven.Tables[0].Rows[0]["pmes_mes"].ToString());
            int[,]anomes = new int[2,7];
            for (int i = 0; i < 7; i++)
            {
                anomes[0,i] = anoI;
                anomes[1,i] = mesI;
                if (mesI == 01)
                {
                    mesI = 12;
                    anoI = anoI - 1;
                }
                else
                    mesI = mesI - 1;
            }

            string nitEmpresa = DBFunctions.SingleData("SELECT MNIT_NIT FROM CEMPRESA");
            string condicionEmpresa = "";
            if (nitEmpresa == "800060067")  // FUJIYAMA DE BARRANQUILLA
                condicionEmpresa = " AND SUBSTR(MITE_CLASABC,1,2) IN ('A1','A2','B1','C1') ";
            //Ahora traemos todos una lista de todos los items a los cuales se les va a realizar pedido sugerido
            // msi.msal_cantasig se reemplaza por CERO porque no esta perfectamente manejado contra los pedidos
            string CONDICION = "(pln.plin_codigo = '" + linea + "' or  '" + linea + "' = 'zZ')"; 
            string CONSULTA_SUGERIDOS_DEMANDA =
           @"select mi.mite_codigo, mi.mnit_nit,  
             coalesce((msi.msal_cantactual - 0 + msi.msal_unidtrans),0) qactual,  
             coalesce(msi.msal_unidtrans,0) qtrans,  
             coalesce(msi.msal_unidpendi,0) qbo,  
             coalesce(msi.msal_costprom,0) msal_costprom,  
             coalesce(mpr.mpro_clicrepo,0) mpro_clicrepo,  
             coalesce(mpr.mpro_frecpedimes,0) mpro_frecpedimes,  
             coalesce(mpr.mpro_stocksegu,0) mpro_stocksegu,  
             pln.plin_tipo,  
             coalesce(mi.mite_stokmini,0) mite_stokmini  
             ,  coalesce(md0.mDEM_cantidad,0) AS D0, coalesce(md1.mDEM_cantidad,0) AS D1, coalesce(md2.mDEM_cantidad,0) AS D2, coalesce(md3.mDEM_cantidad,0) AS D3   
             ,  coalesce(md4.mDEM_cantidad,0) AS D4, coalesce(md5.mDEM_cantidad,0) AS D5, coalesce(md6.mDEM_cantidad,0) AS D6  
            from  plineaitem pln, mitems mi  
             left join msaldoitem msi on mi.mite_codigo=msi.mite_codigo  
             left join mproveedor mpr on mpr.mnit_nit=mi.mnit_nit  
             left join mDEMANDAitem md0 on mi.mite_codigo=md0.mite_codigo AND MD0.pano_ano=" + anomes[0, 0] + @" AND MD0.pmes_mes=" + anomes[1, 0] + @"
             left join mDEMANDAitem md1 on mi.mite_codigo=md1.mite_codigo AND MD1.pano_ano=" + anomes[0, 1] + @" AND MD1.pmes_mes=" + anomes[1, 1] + @"  
             left join mDEMANDAitem md2 on mi.mite_codigo=md2.mite_codigo AND MD2.pano_ano=" + anomes[0, 2] + @" AND MD2.pmes_mes=" + anomes[1, 2] + @" 
             left join mDEMANDAitem md3 on mi.mite_codigo=md3.mite_codigo AND MD3.pano_ano=" + anomes[0, 3] + @" AND MD3.pmes_mes=" + anomes[1, 3] + @" 
             left join mDEMANDAitem md4 on mi.mite_codigo=md4.mite_codigo AND MD4.pano_ano=" + anomes[0, 4] + @" AND MD4.pmes_mes=" + anomes[1, 4] + @"  
             left join mDEMANDAitem md5 on mi.mite_codigo=md5.mite_codigo AND MD5.pano_ano=" + anomes[0, 5] + @" AND MD5.pmes_mes=" + anomes[1, 5] + @" 
             left join mDEMANDAitem md6 on mi.mite_codigo=md6.mite_codigo AND MD6.pano_ano=" + anomes[0, 6] + @" AND MD6.pmes_mes=" + anomes[1, 6] + @"
             where mi.tvig_tipovige='V' AND mi.tORI_CODIGO NOT IN ('X','T') AND msi.pano_ano={0} AND pln.plin_codigo=mi.plin_codigo and "+CONDICION+" " + condicionEmpresa +" ";

            // SE DESEA UN SOLO ITEM PARA PRUEBA          and mi.mite_codigo = '263002Y500'   
        

			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,String.Format(CONSULTA_SUGERIDOS_DEMANDA,ano));
           	//Ahora Llamamos la funcion que nos trae el sugerido de cada item y ademas nos trae un entero
			//que nos indica que tipo pedido es
            DateTime h1 = System.DateTime.Now;
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				int sugerido = 0;
				double demandaPromedio = 0;
                int tipoSugerido = Referencias.ConsultarSugerido(ds.Tables[0].Rows[i], mes, ano, DateTime.Now.Day, ref sugerido, ref demandaPromedio, dsParaInven);
				sqlStrings.Add("INSERT INTO msugeridoitem VALUES('"+ds.Tables[0].Rows[i]["mite_codigo"].ToString()+"',"+tipoSugerido+","+sugerido+","+demandaPromedio+")");
			}
            DateTime h2 = System.DateTime.Now;
			if(DBFunctions.Transaction(sqlStrings))
				processMsg = "";
			else
				processMsg += "<br>ERROR : "+DBFunctions.exceptions;
			return processMsg;
		}
	}
}
