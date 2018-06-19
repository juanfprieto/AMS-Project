using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;
using AMS.Tools;

namespace AMS.Documentos
{
	public class Pedidos
	{
        public static string[] tramites = new string[2];
		//Parametros de entrada son un ArrayList que tendra como salida los sqlReferencias para el proceso que nos indique el flag
		//Si el flag es igual a 1 nos indica que es proceso es de facturacion y debemos construir los sqls referencia para este proceso
		//public static void PedidoVehiculosCliente(string prefijoPedido, uint numeroPedido, string tipoCliente, DataTable elmVnta, DataTable antVeh, ref ArrayList sqlRefs, uint flagProcess)

        public static void PedidoVehiculosCliente(string prefijoPedido, uint numeroPedido, string tipoCliente, 
			int numeroInventario, DataTable elmVnta, DataTable antVeh, ref ArrayList sqlRefs, uint flagProcess,
            string prenda, double valorAutomovil, double ivaAutomovil, string nombreFinanciera, string nit, string prefElementos, string numElementos)
		{
			int i = 0;
            double valorElementos = 0;
            double valorIvaElementos = 0;
            String vinVehiculo = "";

			sqlRefs = new ArrayList();
            if (flagProcess == 1)
            {
                if (prenda != "OrdenTramite")//Esta pregunta se crea para omitir facturas de pedidos cuando se crean ordenes de tramites por separado
                {
                    //Guardar Enlace entre el pedido y la factura mfacturapedidovehiculo                
                    sqlRefs.Add("UPDATE mpedidovehiculo SET test_tipoesta=30,mped_nombfinc='" + nombreFinanciera + "' WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + "");
                    sqlRefs.Add("INSERT INTO mfacturapedidovehiculo VALUES('@1',@2,'" + prefijoPedido + "'," + numeroPedido + ",'" + tipoCliente + "'," + numeroInventario + "," + valorAutomovil + "," + ivaAutomovil + ")");
                    //sqlRefs.Add("UPDATE mvehiculo SET test_tipoesta=40 WHERE mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+")");
                    sqlRefs.Add("UPDATE mvehiculo SET test_tipoesta=40,mveh_prenda='" + prenda + "',mnit_nit='" + nit + "' WHERE mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + ")");
                    sqlRefs.Add("UPDATE mcatalogovehiculo SET mnit_nit='" + nit + "' WHERE pcat_codigo='" + DBFunctions.SingleData("SELECT mcv.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mcv  WHERE  mv.mcat_vin = mcv.mcat_vin AND mv.mveh_inventario= (SELECT mveh_inventario FROM masignacionvehiculo  WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + ")") + "' AND mcat_vin='" + DBFunctions.SingleData("SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario= (SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + ")") + "'");
                    //  && 1==0   configurar si factura elementos en la venta
                    //DataSet tramite = new DataSet();
                    //DBFunctions.Request(tramite, IncludeSchema.NO, "SELECT PDOC_CODIGO, PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS'");
                    //vinVehiculo = DBFunctions.SingleData("SELECT MV.MCAT_VIN FROM MASIGNACIONVEHICULO MA , MVEHICULO MV WHERE MV.MVEH_INVENTARIO = " + numeroInventario + " AND MA.PDOC_CODIGO = '" + prefijoPedido + "' AND MA.MPED_NUMEPEDI = " + numeroPedido + " AND MA.MVEH_INVENTARIO = MV.MVEH_INVENTARIO;");
                    //String fechaEntregar = DBFunctions.SingleData("SELECT MPED_FECHPEDI FROM MPEDIDOVEHICULO WHERE MNIT_NIT = '" + nit + "';");
                    //string codalmacen = DBFunctions.SingleData("SELECT PA.PALM_ALMACEN, PA.PALM_ALMACEN CONCAT ' - ' CONCAT PA.PALM_DESCRIPCION FROM PALMACEN PA, PDOCUMENTOHECHO PH WHERE PA.PALM_ALMACEN = PH.PALM_ALMACEN AND PH.PDOC_CODIGO = '" + prefijoPedido + "';");   
                }
                else if (prenda == "OrdenTramite")
                {
                    String fechaEntregar = DBFunctions.SingleData("SELECT MPED_FECHPEDI FROM MPEDIDOVEHICULO WHERE MNIT_NIT = '" + nit + "';");
                    if (fechaEntregar == "")
                        fechaEntregar = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                    string codalmacen = DBFunctions.SingleData("SELECT PA.PALM_ALMACEN, PA.PALM_ALMACEN CONCAT ' - ' CONCAT PA.PALM_DESCRIPCION FROM PALMACEN PA, PDOCUMENTOHECHO PH WHERE PA.PALM_ALMACEN = PH.PALM_ALMACEN AND PH.PDOC_CODIGO = '" + prefijoPedido + "';");
                    if (codalmacen == "")
                    {
                        codalmacen = DBFunctions.SingleData("SELECT PALM_ALMACEN FROM PALMACEN FETCH FIRST 1 ROWS ONLY");
                    }
                    DataSet tramite = new DataSet();
                    DBFunctions.Request(tramite, IncludeSchema.NO, "SELECT PDOC_CODIGO, PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE TDOC_TIPODOCU = 'OS'");
                    vinVehiculo = DBFunctions.SingleData("SELECT MV.MCAT_VIN FROM MASIGNACIONVEHICULO MA , MVEHICULO MV WHERE MV.MVEH_INVENTARIO = " + numeroInventario + " AND MA.PDOC_CODIGO = '" + prefijoPedido + "' AND MA.MPED_NUMEPEDI = " + numeroPedido + " AND MA.MVEH_INVENTARIO = MV.MVEH_INVENTARIO;");
                    if (vinVehiculo == "") vinVehiculo = "null";

                    //La tabla elmVnta llega con sólo 3 filas [de 0 a 2] por lo que esto genera error:
                    //if (elmVnta.Rows[i][3].ToString() == "T")
                    //existe la posibilidad de que vengan 3 o 4 columnas, el data table puede variar en su definición y estructura, así que agrego esto:

                    if (elmVnta.Columns.Count > 3)//si viene con mas de tres columnas puede validar esto.
                    {
                        for (i = 0; i < elmVnta.Rows.Count; i++)
                        {
                            if (elmVnta.Rows[i][3].ToString() == "T")
                            {
                                sqlRefs.Add("INSERT INTO MORDENTRAMITE VALUES('" + tramite.Tables[0].Rows[0][0] + "'," + tramite.Tables[0].Rows[0][1] + ",'" + elmVnta.Rows[i][3].ToString() + "', " + vinVehiculo + ",'" + nit + "','A', '" + Convert.ToDateTime(DateTime.Now).GetDateTimeFormats()[93].ToString() + "', '" + Convert.ToDateTime(fechaEntregar).GetDateTimeFormats()[5].ToString() + "' , '12:00:00', '" + Convert.ToDateTime(fechaEntregar).GetDateTimeFormats()[93].ToString() + "','" + codalmacen + "', '" + nombreFinanciera + "','',null,'','','" + prefijoPedido + "'," + numeroPedido + ")");
                                tramites[0] = tramite.Tables[0].Rows[0][0].ToString();
                                tramites[1] = tramite.Tables[0].Rows[0][1].ToString();
                                break;
                            }
                        }

                        for (i = 0; i < elmVnta.Rows.Count; i++)
                        {
                            if (elmVnta.Rows[i][3].ToString() == "T")
                                sqlRefs.Add("INSERT INTO DORDENTRAMITE VALUES('" + tramite.Tables[0].Rows[0][0] + "'," + tramite.Tables[0].Rows[0][1] + ",'" + elmVnta.Rows[i][4].ToString() + "', 'C','C','',null, '', '' , null, ''," + elmVnta.Rows[i][1].ToString() + ", null, '" + elmVnta.Rows[i][2].ToString() + "', '" + elmVnta.Rows[i][5].ToString() + "' )");
                        }
                        int modelador = 0;
                        for (int j = 0; j < elmVnta.Rows.Count; j++)
                        {
                            if (elmVnta.Rows[j][3].ToString() == "T")
                            {
                                elmVnta.Rows[modelador].Delete();
                            }
                            else
                                modelador++;
                        }
                        elmVnta.AcceptChanges();
                        if (tramite.Tables.Count > 0 && tramite.Tables[0].Rows.Count > 0)
                            sqlRefs.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU = " + tramite.Tables[0].Rows[0][1].ToString() + " WHERE PDOC_CODIGO = '" + tramite.Tables[0].Rows[0][0].ToString() + "'");
                    }
                }
                    if (prenda != "OrdenTramite")//Esta pregunta se crea para omitir facturas de pedidos cuando se crean ordenes de tramites por separado
                    {
                        for (i = 0; i < elmVnta.Rows.Count; i++)
                        {

                            if (elmVnta.Rows.Count != 0)
                                sqlRefs.Add("INSERT INTO mfacturaelementosventa VALUES('" + prefElementos + "'," + numElementos + ",'" + DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='" + elmVnta.Rows[i][0].ToString() + "'") + "'," + elmVnta.Rows[i][1].ToString() + "," + elmVnta.Rows[i][2].ToString() + ")");
                            //Revisamos si este elemento no esta relacionado en el la tabla dpedidovehiculo, lo relacionamos
                            if (!DBFunctions.RecordExist("SELECT * FROM dpedidovehiculo WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND pite_codigo='" + DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='" + elmVnta.Rows[i][0].ToString() + "'").Trim() + "'"))
                                sqlRefs.Add("INSERT INTO dpedidovehiculo VALUES('" + prefijoPedido + "'," + numeroPedido + ",'" + DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='" + elmVnta.Rows[i][0].ToString() + "'") + "'," + elmVnta.Rows[i][1].ToString() + "," + elmVnta.Rows[i][2].ToString() + ")");

                            valorElementos += Convert.ToDouble(elmVnta.Rows[i][1].ToString());
                            valorIvaElementos += Convert.ToDouble(elmVnta.Rows[i][1].ToString()) * Convert.ToDouble(elmVnta.Rows[i][2].ToString()) / 100;
                        }
                    }


                    //Creacion factura para Elementos adicionales.
                    if (prenda != "OrdenTramite")//Esta pregunta se crea para omitir facturas de pedidos cuando se crean ordenes de tramites por separado
                    {
                        if (elmVnta.Rows.Count != 0 && 1 == 0)
                            sqlRefs.Add("INSERT INTO mfacturapedidovehiculo VALUES('" + prefElementos + "'," + numElementos + ",'" + prefijoPedido + "'," + numeroPedido + ",'A'," + numeroInventario + "," + valorElementos + "," + valorIvaElementos + " )");

                        for (i = 0; i < antVeh.Rows.Count; i++)
                            sqlRefs.Add("UPDATE manticipovehiculo SET test_estaDO = 30, pdoc_codfact='@1', mfac_numedocu=@2 WHERE pdoc_codigo='" + antVeh.Rows[i][0].ToString() + "' AND mcaj_numero=" + antVeh.Rows[i][1].ToString() + " AND mped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + "");
                    }
                }
            
            else if (flagProcess == 2)
            {
                String fechaEntregar = DBFunctions.SingleData("SELECT MPED_FECHPEDI FROM MPEDIDOVEHICULO WHERE MNIT_NIT = '" + nit + "';");
                if (fechaEntregar == "")
                    fechaEntregar = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd");
                string codalmacen = DBFunctions.SingleData("SELECT PA.PALM_ALMACEN, PA.PALM_ALMACEN CONCAT ' - ' CONCAT PA.PALM_DESCRIPCION FROM PALMACEN PA, PDOCUMENTOHECHO PH WHERE PA.PALM_ALMACEN = PH.PALM_ALMACEN AND PH.PDOC_CODIGO = '" + prefijoPedido + "';");
                if (codalmacen == "")
                {
                    codalmacen = DBFunctions.SingleData("SELECT PALM_ALMACEN FROM PALMACEN FETCH FIRST 1 ROWS ONLY");
                }
                
                vinVehiculo = DBFunctions.SingleData("SELECT MV.MCAT_VIN FROM MASIGNACIONVEHICULO MA , MVEHICULO MV WHERE MV.MVEH_INVENTARIO = " + numeroInventario + " AND MA.PDOC_CODIGO = '" + prefijoPedido + "' AND MA.MPED_NUMEPEDI = " + numeroPedido + " AND MA.MVEH_INVENTARIO = MV.MVEH_INVENTARIO;");
                if (vinVehiculo == "") vinVehiculo = "null";                

                sqlRefs.Add("DELETE FROM DORDENTRAMITE WHERE PDOC_CODIGO = '" + prefElementos + "' AND   MORD_NUMEORDE = " + numElementos + "");
                if (!DBFunctions.RecordExist("SELECT * FROM MORDENTRAMITE WHERE PDOC_CODIGO = '" + prefElementos + "' AND MORD_NUMEORDE = " + numElementos + ""))
                {
                    sqlRefs.Add("DELETE FROM MORDENTRAMITE WHERE PDOC_CODIGO = '" + prefElementos + "' AND   MORD_NUMEORDE = " + numElementos + "");
                }

                if (elmVnta.Columns.Count > 3)
                {

                    if(!DBFunctions.RecordExist("SELECT * FROM MORDENTRAMITE WHERE PDOC_CODIGO = '" + prefElementos + "' AND MORD_NUMEORDE = "+ numElementos +""))
                    { 
                        for (i = 0; i < elmVnta.Rows.Count; i++)
                        {
                            if (elmVnta.Rows[i][3].ToString() == "T")
                            {
                                sqlRefs.Add("INSERT INTO MORDENTRAMITE VALUES('" + prefElementos + "'," + numElementos + ",'" + elmVnta.Rows[i][3].ToString() + "', " + vinVehiculo + ",'" + nit + "','A', '" + Convert.ToDateTime(DateTime.Now).GetDateTimeFormats()[93].ToString() + "', '" + Convert.ToDateTime(fechaEntregar).GetDateTimeFormats()[5].ToString() + "' , '12:00:00', '" + Convert.ToDateTime(fechaEntregar).GetDateTimeFormats()[93].ToString() + "','" + codalmacen + "', '" + nombreFinanciera + "','',null,'','','" + prefijoPedido + "'," + numeroPedido + ")");
                                tramites[0] = prefElementos;
                                tramites[1] = numElementos;
                                break;
                            }
                        }
                    }
                    for (i = 0; i < elmVnta.Rows.Count; i++)
                    {
                        if (elmVnta.Rows[i][3].ToString() == "T")
                            sqlRefs.Add("INSERT INTO DORDENTRAMITE VALUES('" + prefElementos + "'," + numElementos + ",'" + elmVnta.Rows[i][4].ToString() + "', 'C','C','',null, '', '' , null, ''," + elmVnta.Rows[i][1].ToString() + ", null, '" + elmVnta.Rows[i][2].ToString() + "', '" + elmVnta.Rows[i][5].ToString() + "' )");
                    }
                    int modelador = 0;
                    for (int j = 0; j < elmVnta.Rows.Count; j++)
                    {
                        if (elmVnta.Rows[j][3].ToString() == "T")
                        {
                            elmVnta.Rows[modelador].Delete();
                        }
                        else
                            modelador++;
                    }
                }
            }
        }

        //Parametros de entrada son un ArrayList que tendra como salida los sqlReferencias para el proceso que nos indique el flag
        //Si el flag es igual a 1 nos indica que es proceso es de facturacion y debemos construir los sqls referencia para este proceso
        //public static void PedidoVehiculosCliente(string prefijoPedido, uint numeroPedido, string tipoCliente, DataTable elmVnta, DataTable antVeh, ref ArrayList sqlRefs, uint flagProcess)
        /// <summary>
        /// ESTE METODO ES SOLO PARA COMPATIBILIDAD CON VERSIONES ANTERIORES DE AMS.Vehiculos. No debe usarse en nuevos desarrollos
        /// </summary>
        public static void PedidoVehiculosCliente(string prefijoPedido, uint numeroPedido, string tipoCliente,
            int numeroInventario, DataTable elmVnta, DataTable antVeh, ref ArrayList sqlRefs, uint flagProcess,
            string prenda, double valorAutomovil, double ivaAutomovil, string nombreFinanciera, string nit)
        {
            int i = 0;
            sqlRefs = new ArrayList();
            if (flagProcess == 1)
            {
                //Guardar Enlace entre el pedido y la factura mfacturapedidovehiculo
                sqlRefs.Add("UPDATE mpedidovehiculo SET test_tipoesta=30,mped_nombfinc='" + nombreFinanciera + "' WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + "");
                sqlRefs.Add("INSERT INTO mfacturapedidovehiculo VALUES('@1',@2,'" + prefijoPedido + "'," + numeroPedido + ",'" + tipoCliente + "'," + numeroInventario + "," + valorAutomovil + "," + ivaAutomovil + ")");
                //sqlRefs.Add("UPDATE mvehiculo SET test_tipoesta=40 WHERE mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+")");
                sqlRefs.Add("UPDATE mvehiculo SET test_tipoesta=40,mveh_prenda='" + prenda + "',mnit_nit='" + nit + "' WHERE mveh_inventario=(SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + ")");
                sqlRefs.Add("UPDATE mcatalogovehiculo SET mnit_nit='" + nit + "' WHERE pcat_codigo='" + DBFunctions.SingleData("SELECT mcv.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mcv  WHERE  mv.mcat_vin = mcv.mcat_vin AND mv.mveh_inventario= (SELECT mveh_inventario FROM masignacionvehiculo  WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + ")") + "' AND mcat_vin='" + DBFunctions.SingleData("SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario= (SELECT mveh_inventario FROM masignacionvehiculo WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + ")") + "'");
                for (i = 0; i < elmVnta.Rows.Count; i++)
                {
                    sqlRefs.Add("INSERT INTO mfacturaelementosventa VALUES('@1',@2,'" + DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='" + elmVnta.Rows[i][0].ToString() + "'") + "'," + elmVnta.Rows[i][1].ToString() + "," + elmVnta.Rows[i][2].ToString() + ")");
                    //Revisamos si este elemento no esta relacionado en el la tabla dpedidovehiculo, lo relacionamos
                    if (!DBFunctions.RecordExist("SELECT * FROM dpedidovehiculo WHERE pdoc_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + " AND pite_codigo='" + DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='" + elmVnta.Rows[i][0].ToString() + "'").Trim() + "'"))
                        sqlRefs.Add("INSERT INTO dpedidovehiculo VALUES('" + prefijoPedido + "'," + numeroPedido + ",'" + DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='" + elmVnta.Rows[i][0].ToString() + "'") + "'," + elmVnta.Rows[i][1].ToString() + "," + elmVnta.Rows[i][2].ToString() + ")");
                }
                for (i = 0; i < antVeh.Rows.Count; i++)
                    sqlRefs.Add("UPDATE manticipovehiculo SET test_estaDO = 30, pdoc_codfact='@1', mfac_numedocu=@2 WHERE pdoc_codigo='" + DBFunctions.SingleData("SELECT pdoc_codigo FROM pdocumento WHERE pdoc_nombre='" + antVeh.Rows[i][0].ToString() + "'") + "' AND mcaj_numero=" + antVeh.Rows[i][1].ToString() + " AND mped_codigo='" + prefijoPedido + "' AND mped_numepedi=" + numeroPedido + "");
            }
        }


		//Pedidos clientes por mayor
		//Recibe tabla de vehiculos a facturar,arraylist para guardar consutas,(Si el flag es igual a 1 nos indica que es proceso es de facturacion),nit del cliente, totales, cadena con vins usados,nit cliente, fecha factura, usuario
		public static void PedidoVehiculosMayorCliente(DataRow[] drVehiculo, ref ArrayList sqlRefs, uint flagProcess,string cliente,ref double totalVehiculos, ref double totalDescuentos, ref double totalIVA, ref double totalFactura,string prefGuia, string numGuia, ref string vins,DateTime fechaFactura, int idUsuario)
		{
			sqlRefs = new ArrayList();
			if(flagProcess == 1)
			{
				//Recorrer vehiculos
				double valorV,ivaV,descV;
				int numPed;
				string prefPed,catV,vinV,colV;
				for(int n=0;n<drVehiculo.Length;n++)
				{
					prefPed=drVehiculo[n]["PDOC_CODIGO"].ToString();
					numPed=Convert.ToInt16(drVehiculo[n]["MPED_NUMEPEDI"]);
					valorV=Convert.ToDouble(drVehiculo[n]["PRECIO"]);
					descV=Convert.ToDouble(drVehiculo[n]["DESCUENTO"]);
					ivaV=Convert.ToDouble(drVehiculo[n]["PRECIO_IVA"]);
					catV=drVehiculo[n]["PCAT_CODIGO"].ToString();
					vinV=drVehiculo[n]["MCAT_VIN"].ToString();
					colV=drVehiculo[n]["PCOL_CODIGO"].ToString();
					vins+="'"+vinV+"',";
					totalVehiculos+=valorV-descV;
					totalDescuentos+=descV;
					totalIVA+=ivaV;
					totalFactura+=valorV+ivaV-descV;

					//ACTUALIZAR MVEHICULOS y mcatalogovehiculo (ESTADO,NIT)
					sqlRefs.Add("UPDATE MVEHICULO SET MNIT_NIT='"+cliente+"', TEST_TIPOESTA=40 where mcat_vin='"+vinV+"';");
					sqlRefs.Add("UPDATE mcatalogovehiculo SET MNIT_NIT='"+cliente+"' where pcat_codigo='"+catV+"' and mcat_vin='"+vinV+"';");
				
					//Insertar DFACTURAPEDIDOMAYORVEHICULO
					sqlRefs.Add("INSERT INTO DFACTURAPEDIDOMAYORVEHICULO VALUES('@1',@2,'"+prefPed+"',"+numPed+",'"+catV+"','"+vinV+"','"+colV+"',"+valorV+","+descV+","+ivaV+",'"+prefGuia+"','"+numGuia+"',NULL,NULL,'"+fechaFactura.ToString("yyyy-MM-dd hh:mm:ss")+"',"+idUsuario+");");
				
					//Agregar modelo a detalles del pedido con valores por defecto si no existia en el pedido
					sqlRefs.Add("INSERT INTO dpedidovehiculoclientemayor "+
						"select '"+prefPed+"',"+numPed+",mc.pcat_codigo,'"+colV+"','"+colV+"',0,0,"+valorV+",'"+DateTime.Now.ToString("yyyy-MM-dd")+"',0,0 "+
						"from pcatalogovehiculo mc where mc.pcat_codigo='"+catV+"' and "+
						"mc.pcat_codigo not in("+
						" select dp.pcat_codigo from dpedidovehiculoclientemayor dp "+
						" where dp.pdoc_codigo='"+prefPed+"' AND dp.mped_numepedi="+numPed+" AND "+
						" dp.pcat_codigo='"+catV+"'"+
						");");

					//Actualizar estado pedido->ya asignado, no completamente facturado
					sqlRefs.Add("UPDATE mpedidovehiculoclientemayor SET test_tipoesta=20 "+
						"where pdoc_codigo='"+prefPed+"' AND mped_numepedi="+numPed+";");

					//Actualizar cantidad y valor facturado dpedidovehiculoclientemayor 
					//del primer modelo coincidente encontrado (no completamente facturado)
					sqlRefs.Add("UPDATE dpedidovehiculoclientemayor dp SET "+
						"dp.dped_cantfact=dp.dped_cantfact+1,"+
						"dp.dped_valofact=dp.dped_valofact+"+(valorV+ivaV-descV)+" "+
						"where dp.pdoc_codigo='"+prefPed+"' AND dp.mped_numepedi="+numPed+" AND "+
						"dp.pcat_codigo='"+catV+"' and dp.pcol_codigo in("+
						" select dpa.pcol_codigo from dbxschema.dpedidovehiculoclientemayor dpa "+
						" where dpa.pdoc_codigo='"+prefPed+"' AND dpa.mped_numepedi="+numPed+" AND "+
						" dpa.pcat_codigo='"+catV+"' and "+
						" dpa.dped_cantpedi>dpa.dped_cantfact "+
						" fetch first 1 rows only);");
				}

				//Cerrar los pedidos completamente asignados
				sqlRefs.Add("UPDATE mpedidovehiculoclientemayor mp SET mp.test_tipoesta=30 "+
					"where mp.mnit_nit='"+cliente+"' and (mp.pdoc_codigo,mp.mped_numepedi) not in("+
					" select dp.pdoc_codigo,dp.mped_numepedi from dpedidovehiculoclientemayor dp "+
					" where dp.dped_cantpedi>dp.dped_cantfact and dp.pdoc_codigo=mp.pdoc_codigo "+
					" and dp.mped_numepedi=mp.mped_numepedi);");
			}
		}
		//Actualizaciones devolucion factura mayor vehiculos
		public static bool DevolucionVehiculosMayorCliente(ref ArrayList sqlStrings, string prefFacturaOrig,string numFacturaOrig,string prefFacturaDev,string numFacturaDev,DataTable dtVehiculos,string cliente,ref double totalVehiculos, ref double totalDescuentos, ref double totalIVA, ref double totalFactura,DateTime fechaDevolucion,int idUsuario)
		{
			sqlStrings = new ArrayList();
			bool status = false;
			double valorV,ivaV,descV;
			for(int n=0;n<dtVehiculos.Rows.Count;n++)
			{
				if(Convert.ToInt16(dtVehiculos.Rows[n]["USADO"])!=1)
					continue;
				valorV=Convert.ToDouble(dtVehiculos.Rows[n]["precio"]);
				ivaV=Convert.ToDouble(dtVehiculos.Rows[n]["iva"]);
				descV=Convert.ToDouble(dtVehiculos.Rows[n]["descuento"]);
				totalVehiculos+=+valorV;
				totalDescuentos+=descV;
				totalIVA+=ivaV;
				totalFactura+=valorV+ivaV-descV;

				//ACTUALIZAR ESTADO VEHICULOS: DISPONIBLES
				sqlStrings.Add("UPDATE DBXSCHEMA.MVEHICULO SET TEST_TIPOESTA=20 "+
					"where MCAT_VIN='"+dtVehiculos.Rows[n]["MCAT_VIN"].ToString()+"';");
				
				//INSERTAR DETALLES DEVOLUCION
				sqlStrings.Add("INSERT INTO DFACTURAPEDIDOMAYORVEHICULO VALUES("+
					"'@1',@2,"+
					"'"+dtVehiculos.Rows[n]["MPED_CODIGO"]+"',"+dtVehiculos.Rows[n]["mped_numepedi"]+","+
					"'"+dtVehiculos.Rows[n]["PCAT_CODIGO"]+"','"+dtVehiculos.Rows[n]["MCAT_VIN"]+"',"+
					"'"+dtVehiculos.Rows[n]["PCOL_CODIGO"]+"',"+
					valorV+","+descV+","+ivaV+","+
					"'"+dtVehiculos.Rows[n]["pdoc_guia"]+"','"+dtVehiculos.Rows[n]["mfac_numeguia"]+"',"+
					"'"+prefFacturaOrig+"',"+numFacturaOrig+","+
					"'"+fechaDevolucion.ToString("yyyy-MM-dd hh:mm:ss")+"',"+idUsuario+");");
				
				//ACTUALIZAR NUMERO DEVOLUCION DETALLES ORIGINALES
				sqlStrings.Add("UPDATE DBXSCHEMA.DFACTURAPEDIDOMAYORVEHICULO "+
					"SET PDOC_CODIGODEV='"+prefFacturaDev+"',MFAC_NUMEDOCUDEV="+numFacturaDev+" "+
					"WHERE "+
					"PDOC_CODIGO='"+prefFacturaOrig+"' AND MFAC_NUMEDOCU="+numFacturaOrig+" AND "+
					"MPED_CODIGO='"+dtVehiculos.Rows[n]["MPED_CODIGO"]+"' AND "+
					"MPED_NUMEPEDI="+dtVehiculos.Rows[n]["MPED_NUMEPEDI"]+" AND "+
					"MCAT_VIN='"+dtVehiculos.Rows[n]["MCAT_VIN"]+"';");
				status=true;
			}
			return(status);
		}

		//Si el flag es igual a 1 nos indica que es proceso es de facturacion y debemos construir los sqls referencia para este proceso
		//Si el flag es igual a 2 nos indicca que es proceso de devolucion de la factura de servicio de taller
		public static void PedidoServicioTaller(string prefijoOT, uint numeroOT, string cargoRel, string obs, double valorDeducible, double valorDeduSuminAse, double porcDeduMin, ref ArrayList sqlRefs, uint flagProcess, double valorDescuentoRepuestos, double valorDescuentoOperaciones)
		{
			sqlRefs = new ArrayList();
			if(flagProcess == 1)
			{
				//Se debe guardar el enlaze entre la orden de trabajo y la factura generada (mfacturaclientetaller)
				if(valorDeducible != 0)
					sqlRefs.Add("INSERT INTO mfacturaclientetaller VALUES('@1',@2,'"+prefijoOT+"',"+numeroOT+",'"+cargoRel+"','"+obs+"','"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',"+valorDeducible+","+valorDeduSuminAse+","+valorDescuentoOperaciones+","+valorDescuentoRepuestos+")");
				else
					sqlRefs.Add("INSERT INTO mfacturaclientetaller VALUES('@1',@2,'"+prefijoOT+"',"+numeroOT+",'"+cargoRel+"','"+obs+"','"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',null,null,"+valorDescuentoOperaciones+","+valorDescuentoRepuestos+")");
				//Ahora se actualizan las transferencias que esten relacionadas con este cargo (mordentransferencia ,mpedidotransferencia y mfacturacliente)
				sqlRefs.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE (pdoc_codigo,mfac_numedocu) IN (SELECT pdoc_factura,mfac_numero FROM mordentransferencia WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+" AND tcar_cargo='"+cargoRel+"')");
				sqlRefs.Add("UPDATE mpedidotransferencia SET tvig_vigencia='C' WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+" AND tcar_cargo='"+cargoRel+"'");
				if(cargoRel == "S")
					sqlRefs.Add("UPDATE dordenseguros SET mord_porcdeducible="+porcDeduMin.ToString()+", mord_deduminimo="+valorDeducible.ToString()+", mord_dedusumase="+valorDeduSuminAse.ToString()+" WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+"");
			}
		}

		//Pedido servicio con autorizacion
		public static void PedidoServicioTaller(string prefijoOT, uint numeroOT, string cargoRel, string obs, double valorMinimoDeducible, double valorDeducible, double valorDeduSuminAse, double porcDeduMin, ref ArrayList sqlRefs, uint flagProcess, double valorDescuentoRepuestos, double valorDescuentoOperaciones, string autoriza)
		
        {
			sqlRefs = new ArrayList();
			if(flagProcess == 1)
			{
				//Se debe guardar el enlaze entre la orden de trabajo y la factura generada (mfacturaclientetaller)
				if(valorDeducible != 0)
                    sqlRefs.Add("INSERT INTO mfacturaclientetaller VALUES('@1',@2,'" + prefijoOT + "'," + numeroOT + ",'" + cargoRel + "','" + obs + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "'," + valorDeducible + "," + valorDeduSuminAse + "," + valorDescuentoOperaciones + "," + valorDescuentoRepuestos + ",'" + autoriza + "',@3)");
				else
                    sqlRefs.Add("INSERT INTO mfacturaclientetaller VALUES('@1',@2,'" + prefijoOT + "'," + numeroOT + ",'" + cargoRel + "','" + obs + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "',null,null," + valorDescuentoOperaciones + "," + valorDescuentoRepuestos + ",'" + autoriza + "',@3)");
				//Ahora se actualizan las transferencias que esten relacionadas con este cargo (mordentransferencia ,mpedidotransferencia y mfacturacliente)
				sqlRefs.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE (pdoc_codigo,mfac_numedocu) IN (SELECT pdoc_factura,mfac_numero FROM mordentransferencia WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+" AND tcar_cargo='"+cargoRel+"')");
				sqlRefs.Add("UPDATE mpedidotransferencia SET tvig_vigencia='C' WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+" AND tcar_cargo='"+cargoRel+"'");
				if(cargoRel == "S")
					sqlRefs.Add("UPDATE dordenseguros SET mord_porcdeducible="+porcDeduMin.ToString()+", mord_deduminimo="+ valorMinimoDeducible.ToString()+", mord_dedusumase="+valorDeduSuminAse.ToString()+" WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+"");
			}
		}
	}
}
