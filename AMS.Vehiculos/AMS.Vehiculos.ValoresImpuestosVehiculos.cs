using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.DB;
using System.Data;
using AMS.Tools;

namespace AMS.Vehiculos
{
    public class ValoresImpuestosVehiculos
    {
        private double valorBase, valorIvaPorc, valorIva, valorIpoConsumoPorc, valorVenta, valorImpuestos, valorIpoConsumo;
        
        public double ValorBase { get { return valorBase; } }
        public double ValorIvaPorc { get { return valorIvaPorc; } }
        public double ValorIpoConsumoPorc { get { return valorIpoConsumoPorc; } }
        public double ValorIpoConsumo { get { return valorIpoConsumo; } }
        public double ValorIva { get { return valorIva; } }
        public double ValorVenta { get { return valorVenta; } }
        public double ValorImpuestos { get { return valorImpuestos; } }

        //Constructor para la generacion de valores de creacion de pedido.
        public ValoresImpuestosVehiculos(string catalogo, bool aplicarIpo, string opcionVehiculo)
        {
            string sqlOpcion = "";
            if (opcionVehiculo != "")
                sqlOpcion = " AND PP.POPC_OPCIVEHI='" + opcionVehiculo + "'";

            DataSet dsValoresVehiculo = new DataSet();
            DBFunctions.Request(dsValoresVehiculo, IncludeSchema.NO, 
               @"SELECT PPRE_PRECIO as BASE, coalesce(PIVA_PORCIVA,0.00) as IVA, coalesce(PIPO_PORCIPOC,0) as IPOCONSUMO,  
                ROUND(PPRE_PRECIO * (1 + ROUND(COALESCE((PIVA_PORCIVA + PIPO_PORCIPOC),0)/100,2)),0) as VENTA,  
                ROUND(PPRE_PRECIO * (ROUND(COALESCE((PIVA_PORCIVA + PIPO_PORCIPOC),0)/100,2)),0) as IMPUESTOS  
                FROM PPRECIOVEHICULO PP, PCATALOGOVEHICULO PC  
                WHERE PP.PCAT_CODIGO = PC.PCAT_CODIGO AND PP.PCAT_CODIGO = '" + catalogo + "' " + sqlOpcion);

            valorBase = 0;
            valorIvaPorc = 0;
            valorIpoConsumoPorc = 0;
            valorVenta = 0;
            valorImpuestos = 0;
            valorIpoConsumo = 0;
            valorIva = 0;

            if (dsValoresVehiculo.Tables[0].Rows.Count != 0)
            {
                if(dsValoresVehiculo.Tables[0].Rows[0][0].ToString() == "" || dsValoresVehiculo.Tables[0].Rows[0][1].ToString() == "" || dsValoresVehiculo.Tables[0].Rows[0][2].ToString() == "" )
                    // Si falta algun parametro retorna todos los valores en ceros
                    valorIpoConsumo = 0;
                else
                {
                    valorBase = Math.Round( Convert.ToDouble( dsValoresVehiculo.Tables[0].Rows[0][0].ToString() ),0 );
                    valorIvaPorc = Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][1].ToString());
                    valorIva = Math.Round(valorBase * (valorIvaPorc / 100), 0);
                    valorIpoConsumoPorc = Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][2].ToString());
                    valorIpoConsumo = Math.Round(valorBase * (valorIpoConsumoPorc / 100), 0);
                    valorVenta = Math.Round(Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][3].ToString()), 0);
                    valorImpuestos = Math.Round(Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][4].ToString()), 0);

                    if (aplicarIpo == false)
                    {
                        valorVenta = valorVenta - valorIpoConsumo;
                        valorImpuestos = valorImpuestos - valorIpoConsumo;
                        valorIpoConsumoPorc = 0;
                        valorIpoConsumo = 0;
                    }
                }
            }
        }

        public ValoresImpuestosVehiculos(string catalogo, bool aplicarIpo, string opcionVehiculo, string ano)
        {
            string sqlOpcion = "";
            if (opcionVehiculo != "")
                sqlOpcion = " AND PP.POPC_OPCIVEHI='" + opcionVehiculo + "'";

            DataSet dsValoresVehiculo = new DataSet();
            DBFunctions.Request(dsValoresVehiculo, IncludeSchema.NO,
               @"SELECT PPRE_PRECIO as BASE, coalesce(PIVA_PORCIVA,0.00) as IVA, coalesce(PIPO_PORCIPOC,0) as IPOCONSUMO,  
                ROUND(PPRE_PRECIO * (1 + ROUND(COALESCE((PIVA_PORCIVA + PIPO_PORCIPOC),0)/100,2)),0) as VENTA,  
                ROUND(PPRE_PRECIO * (ROUND(COALESCE((PIVA_PORCIVA + PIPO_PORCIPOC),0)/100,2)),0) as IMPUESTOS  
                FROM PPRECIOVEHICULO PP, PCATALOGOVEHICULO PC  
                WHERE PP.PCAT_CODIGO = PC.PCAT_CODIGO AND PP.PCAT_CODIGO = '" + catalogo + "' " + sqlOpcion + " AND PP.PANO_ANO = '" + ano + "'");

            valorBase = 0;
            valorIvaPorc = 0;
            valorIpoConsumoPorc = 0;
            valorVenta = 0;
            valorImpuestos = 0;
            valorIpoConsumo = 0;
            valorIva = 0;

            if (dsValoresVehiculo.Tables[0].Rows.Count != 0)
            {
                if (dsValoresVehiculo.Tables[0].Rows[0][0].ToString() == "" || dsValoresVehiculo.Tables[0].Rows[0][1].ToString() == "" || dsValoresVehiculo.Tables[0].Rows[0][2].ToString() == "")
                    // Si falta algun parametro retorna todos los valores en ceros
                    valorIpoConsumo = 0;
                else
                {
                    valorBase = Math.Round(Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][0].ToString()), 0);
                    valorIvaPorc = Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][1].ToString());
                    valorIva = Math.Round(valorBase * (valorIvaPorc / 100), 0);
                    valorIpoConsumoPorc = Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][2].ToString());
                    valorIpoConsumo = Math.Round(valorBase * (valorIpoConsumoPorc / 100), 0);
                    valorVenta = Math.Round(Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][3].ToString()), 0);
                    valorImpuestos = Math.Round(Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][4].ToString()), 0);

                    if (aplicarIpo == false)
                    {
                        valorVenta = valorVenta - valorIpoConsumo;
                        valorImpuestos = valorImpuestos - valorIpoConsumo;
                        valorIpoConsumoPorc = 0;
                        valorIpoConsumo = 0;
                    }
                }
            }
        }

        //Constructor para la generacion de valores de facturacion.
        public ValoresImpuestosVehiculos(double precioVenta, string catalogo, bool aplicarIpo)
        {
            DataSet dsValoresVehiculo = new DataSet();
            DBFunctions.Request(dsValoresVehiculo, IncludeSchema.NO,
                "SELECT piva_porciva as piva_porciva, PIPO_PORCIPOC as PIPO_PORCIPOC FROM pcatalogovehiculo WHERE pcat_codigo='" + catalogo + "'");

            valorBase = 0;
            valorIvaPorc = 0;
            valorIpoConsumoPorc = 0;
            valorVenta = 0;
            valorImpuestos = 0;
            valorIpoConsumo = 0;
            valorIva = 0; 
            
            if (dsValoresVehiculo.Tables[0].Rows.Count != 0)
            {
                if (dsValoresVehiculo.Tables[0].Rows[0][0].ToString() == "" || dsValoresVehiculo.Tables[0].Rows[0][1].ToString() == "")
                      // Si falta algun parametro retorna todos los valores en ceros
                    valorIpoConsumo = 0;
                else
                {
                    valorIvaPorc = Math.Round( Convert.ToDouble( dsValoresVehiculo.Tables[0].Rows[0][0].ToString() ),0 );
                
                    if (aplicarIpo == true)
                        valorIpoConsumoPorc = Math.Round(Convert.ToDouble(dsValoresVehiculo.Tables[0].Rows[0][1].ToString()), 0);
                    else
                        valorIpoConsumoPorc = 0;

                    valorBase =  Math.Round( precioVenta / ( ( (valorIvaPorc + valorIpoConsumoPorc)/100 ) + 1), 0);
                    valorVenta = precioVenta;
                    valorImpuestos =  Math.Round( valorBase * ((valorIvaPorc + valorIpoConsumoPorc) / 100), 0);
                    valorIpoConsumo = Math.Round(valorBase * (valorIpoConsumoPorc / 100), 0);
                    valorIva = Math.Round(valorBase * (valorIvaPorc / 100), 0);
                }
            }
        }
   }

}