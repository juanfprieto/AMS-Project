using System;
using System.Data;
using System.Collections;
using AMS.DB;
using AMS.Forms;
using AMS.Inventarios;

namespace AMS.Produccion
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Produccion
	{
		#region Propiedades
		
		private string prefijoOrden,fecha,usuario,proceso,programaProduccion,mensajes,nit,vendedor,prefijoPedido,prefijoTransferencia,almacen,observacion,tipo,almacenMatPrima;
		private int numeroOrden;
		private DataTable dtProduccion,dtItems;
		private bool error,disponible;
		private double totalValorItems;
		private int anoInventario;

		public string PrefijoOrden{set{prefijoOrden=value;} get{return prefijoOrden;}}
		public string Fecha{set{fecha=value;} get{return fecha;}}
		public string Usuario{set{usuario=value;} get{return usuario;}}
		public string Proceso{set{proceso=value;} get{return proceso;}}
		public string ProgramaProduccion{set{programaProduccion=value;} get{return programaProduccion;}}
		public string Mensajes{get{return mensajes;}}
		public string Nit{set{nit=value;} get{return nit;}}
		public string Vendedor{set{vendedor=value;} get{return vendedor;}}
		public string PrefijoPedido{set{prefijoPedido=value;} get{return prefijoPedido;}}
		public string PrefijoTransferencia{set{prefijoTransferencia=value;} get{return prefijoTransferencia;}}
		public string Almacen{set{almacen=value;} get{return almacen;}}
		public string AlmacenMatPrima{set{almacenMatPrima=value;} get{return almacenMatPrima;}}
		public string Observacion{set{observacion=value;} get{return observacion;}}
		public int NumeroOrden{set{numeroOrden=value;} get{return numeroOrden;}}
		public DataTable DtProduccion{set{dtProduccion=value;} get{return dtProduccion;}}
		private bool Error{set{error=value;} get{return error;}}
		public bool Disponible{get{return disponible;}}
		public int numTransferencias=0;
		
		#endregion Propiedades

		#region Constructores
		public Produccion()
		{
			dtProduccion=null;
		}

		public Produccion(string preOrd, int numOrd, string fec,string usu, 
							string proc, string progProd, string nit, string vend, 
							string prefped, string preftrans, string alm, string almMP,
							string obs,DataTable dtp,string tp)
		{
			prefijoOrden=preOrd;
			numeroOrden=numOrd;
			fecha=fec;
			usuario=usu;
			proceso=proc;
			programaProduccion=progProd;
			this.nit=nit;
			vendedor=vend;
			prefijoPedido=prefped;
			prefijoTransferencia=preftrans;
			almacen=alm;
			almacenMatPrima=almMP;
			observacion=obs;
			dtProduccion=dtp;
			tipo=tp;
            anoInventario = Convert.ToInt16(DBFunctions.SingleData("select pano_ano from cinventario;"));
		}

		#endregion Constructores
		
		//Guarda la orden de produccion
		public bool GuardarOrdenProduccion()
		{
			bool estado=false;
			ArrayList sqlStrings=new ArrayList();
			DataSet ds;
			//Insertamos el registro en mordenproduccion
			uint numped=uint.Parse(DBFunctions.SingleData("SELECT pped_ultipedi+1 FROM ppedido WHERE pped_codigo='"+prefijoPedido+"'"));

            if (programaProduccion != null)
                sqlStrings.Add("INSERT INTO mordenproduccion VALUES('" + prefijoOrden + "'," + numeroOrden + ",'" + tipo + "','" + fecha + "','A','" + programaProduccion + "','" + nit + "','" + vendedor + "','" + usuario + "','" + proceso + "',NULL,NULL,'" + almacen + "','" + observacion + "')");
            else
                sqlStrings.Add("INSERT INTO mordenproduccion VALUES('" + prefijoOrden + "'," + numeroOrden + ",'" + tipo + "','" + fecha + "','A',NULL,'" + nit + "','" + vendedor + "','" + usuario + "','" + proceso + "',NULL,NULL,'" + almacen + "','" + observacion + "')");
			
            sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu="+numeroOrden+" WHERE pdoc_codigo='"+prefijoOrden+"'");
			//Insertamos el registro de cada catalogo q se va a ensamblar
			uint numTrans=uint.Parse(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoTransferencia+"'"));
			PedidoFactura miPedfacMP; //Pedido Transferencia materia prima
            PedidoFactura miPedfacO; //Pedido Transferencia items que ya estaban disponibles
			ConstruirDtItems();
			totalValorItems=0;
            string progConsec = programaProduccion != null ? DBFunctions.SingleData("SELECT MPROG_CONSECUTIVO FROM MPROGRAMAPRODUCCION WHERE MPROG_NUMERO='" + programaProduccion + "';") : "";
			for(int i=0;i<dtProduccion.Rows.Count;i++)
			{
				ds=new DataSet();
				if(tipo=="E")
				{ //Ensamble
					sqlStrings.Add(
						"INSERT INTO dordenproduccion "+
						"VALUES('"+prefijoOrden+"',"+numeroOrden+","+"'"+
						dtProduccion.Rows[i]["ENSAMBLE"].ToString()+"',"+
						"'"+dtProduccion.Rows[i]["CATALOGO"].ToString()+"',NULL,"+
						dtProduccion.Rows[i]["CANTPROC"].ToString()+",default);");

                    if (programaProduccion != null)
					    sqlStrings.Add(
						    "UPDATE DPROGRAMAPRODUCCION "+
						    "SET DPROG_TOTAL=DPROG_TOTAL+"+dtProduccion.Rows[i]["CANTPROC"].ToString()+" "+
						    "WHERE MPROG_CONSECUTIVO="+progConsec+" AND PCAT_CODIGO='"+dtProduccion.Rows[i]["CATALOGO"].ToString()+"';");
				}
				else
				{ //Produccion
					sqlStrings.Add(
						"INSERT INTO dordenproduccion "+
						"VALUES('"+prefijoOrden+"',"+numeroOrden+","+"'"+
						dtProduccion.Rows[i]["ENSAMBLE"].ToString()+"',"+
						"NULL,'"+dtProduccion.Rows[i]["CATALOGO"].ToString()+"',"+
						dtProduccion.Rows[i]["CANTPROC"].ToString()+",default);");

                    if (programaProduccion != null)
					    sqlStrings.Add(
						    "UPDATE DPROGRAMAPRODUCCION "+
						    "SET DPROG_TOTAL=DPROG_TOTAL+"+dtProduccion.Rows[i]["CANTPROC"].ToString()+" "+
						    "WHERE MPROG_CONSECUTIVO="+progConsec+" AND MITE_CODIGO='"+dtProduccion.Rows[i]["CATALOGO"].ToString()+"';");
				}

				//Agregar items a pedir del ensamble en tabla de pedidos
				AgregarTablaPedidos(
					dtProduccion.Rows[i]["ENSAMBLE"].ToString(),
					dtProduccion.Rows[i]["CATALOGO"].ToString(),
					Convert.ToDouble(dtProduccion.Rows[i]["CANTPROC"]));
			}

			bool facMP=false,facO=false;
			//Crear nuevo pedido factura para items sacados de materia prima
			miPedfacMP=new PedidoFactura("T",prefijoPedido,nit,almacenMatPrima,numped,prefijoOrden,uint.Parse(numeroOrden.ToString()),"C",DateTime.Parse(fecha),observacion,0,new string[] { },vendedor,prefijoTransferencia,numTrans,"",0,0,totalValorItems,totalValorItems);
			for(int i=0;i<dtItems.Rows.Count;i++){
				if(Convert.ToBoolean(dtItems.Rows[i]["MATERIAPRIMA"])){
					miPedfacMP.InsertaFila(dtItems.Rows[i][0].ToString(), double.Parse(dtItems.Rows[i][1].ToString()), double.Parse(dtItems.Rows[i][2].ToString()), 0, 0, double.Parse(dtItems.Rows[i][5].ToString()),"","");
					//                             Item                           Cantidad                                    Precio                           Iva Descuento    Cantidad Pedida                      
					facMP=true;
				}
			}
			//Actualizar pedido de la orden si se pide materia prima
			if(facMP)
				sqlStrings.Add("UPDATE mordenproduccion SET "+
					"PPED_CODIPEDI='"+prefijoPedido+"', MPED_NUMEPEDI="+numped+" "+
					"WHERE pdoc_codigo='"+prefijoOrden+"' and MORD_NUMEORDE="+numeroOrden+";");

			//Crear nuevo pedido factura para items sacados de una orden
            miPedfacO = new PedidoFactura("T", prefijoPedido, nit, almacen, (facMP ? numped + 1 : numped), prefijoOrden, uint.Parse(numeroOrden.ToString()), "C", DateTime.Parse(fecha), observacion, 0, new string[] { }, vendedor, prefijoTransferencia, (facMP ? numTrans + 1 : numTrans), "", 0, 0, totalValorItems, totalValorItems);
            for (int i = 0; i < dtItems.Rows.Count; i++)
            {
                if (!Convert.ToBoolean(dtItems.Rows[i]["MATERIAPRIMA"]))
                {
                    miPedfacO.InsertaFila(dtItems.Rows[i][0].ToString(), double.Parse(dtItems.Rows[i][1].ToString()), double.Parse(dtItems.Rows[i][2].ToString()), 0, 0, double.Parse(dtItems.Rows[i][5].ToString()),"","");
                    //                             Item                           Cantidad                                    Precio                         Iva Descuento    Cantidad Pedida                      
                    facO = true;
                }
            }

			if(facO&&facMP)numTransferencias=2;
			else numTransferencias=1;
           
			miPedfacMP.RealizarPedProdFac(false);
            miPedfacO.RealizarPedProdFac(false);

			if(facMP)
				for(int i=0;i<miPedfacMP.SqlStrings.Count;i++)
					sqlStrings.Add(miPedfacMP.SqlStrings[i].ToString());
            if (facO)
                for (int i = 0; i < miPedfacO.SqlStrings.Count; i++)
                    sqlStrings.Add(miPedfacO.SqlStrings[i].ToString());

			//Revisar disponibilidad
			double cantAlm;
			string sCantAlm="",almDisp="";
			disponible=true;
			for(int i=0;i<dtItems.Rows.Count;i++)
			{
				if(!Convert.ToBoolean(dtItems.Rows[i]["MATERIAPRIMA"]))
					almDisp=almacen;
				else
					almDisp=almacenMatPrima;
				sCantAlm=DBFunctions.SingleData(
						"SELECT msal_cantactual-msal_cantasig from "+
						"MSALDOITEMALMACEN where "+
						"PALM_ALMACEN='"+almDisp+"' AND "+
						"MITE_CODIGO='"+dtItems.Rows[i][0].ToString()+"' AND "+
						"PANO_ANO="+anoInventario+";");
				if(sCantAlm.Length==0)sCantAlm="0";
				cantAlm=Double.Parse(sCantAlm);
				if(cantAlm<Convert.ToDouble(dtItems.Rows[i][1]))
				{
					mensajes+="Hacen falta "+(Convert.ToDouble(dtItems.Rows[i][1])-cantAlm).ToString()+" unidades del item "+dtItems.Rows[i][0].ToString()+" en el almacén "+almDisp+".\\r\\n";
					disponible=false;
				}
			}
			if(!disponible)
				return false;
		
			if(DBFunctions.Transaction(sqlStrings))
			{
				estado=true;
				mensajes="Bien "+DBFunctions.exceptions;
			}
			else
			{
				mensajes+="Error al Guardar "+DBFunctions.exceptions;
            }
			return estado;
		}

		//Generar tabla de pedidos
		private void AgregarTablaPedidos(string ensamble, string codItem,double cantidad)
		{
			RecorrerEnsamble(codItem,cantidad,ensamble,1,"", false);
		}

		//Recorrer esambles para agregar elementos a tabla de pedidos (se agrega solo la materia prima)
		private void RecorrerEnsamble(string codItem, double cantidad, string ensamble, int contador, string itemsAnt, bool esSubItem)
		{
			string itemsAnteriores;
			DataSet dsSubItems=new DataSet();
			string ensambleA, cantDispStr;
			double cantSaldo=0,cantDisp=0;
			bool hijos=false;
			
			//Limitar niveles a 50 max.
			if(contador>50)return;
			//Evitar sumar el item varias veces
			itemsAnteriores=itemsAnt;
			//Evitar loop infinito
			if(itemsAnteriores.IndexOf("<"+codItem+"/>")>0)
				return;
			itemsAnteriores+="<"+codItem+"/>";

			//Se debe aprovechar lo que haya en la planta y no pedir mas de lo necesario SOLO PARA SUBITEMS
			//Consultar (cant actual-cant asignada) del item en la bodega

            if (esSubItem) // solo para los subitems del ensamble
            {
                cantDispStr = DBFunctions.SingleData(
                    "SELECT msal_cantactual-msal_cantasig FROM MSALDOITEMALMACEN " +
                    "WHERE PALM_ALMACEN='" + almacen + "' AND MITE_CODIGO='" + codItem + "' AND PANO_ANO=" + anoInventario + ";"
                );
                if (cantDispStr.Length > 0)
                    cantSaldo = Convert.ToDouble(cantDispStr);

                cantDisp = cantSaldo;
                //Si hay suficientes items disponibles, no agregarlo al pedido
                if (cantDisp >= cantidad)
                {
                    //Agregar items obtenido del mismo almacen
                    AgregarItemPedido(codItem, cantidad, cantidad, false);
                    return;
                }
                else if (cantDisp > 0)
                {
                    AgregarItemPedido(codItem, cantDisp, cantDisp, false);
                    cantidad = cantidad - cantDisp;
                }
            }

			//Subitems
            DBFunctions.Request(dsSubItems, IncludeSchema.NO,
                "SELECT mp.MITE_CODIGO CODIGO, mp.MENS_CANTIDAD CANTIDAD " +
                "FROM MENSAMBLEPRODUCCIONITEMS mp, PENSAMBLEPRODUCCION pe " +
                "WHERE pe.PENS_CODIGO=mp.PENS_CODIGO AND pe.PENS_CODIGO='" + ensamble + "' and pe.pens_vigente='S';");

            for (int n = 0; n < dsSubItems.Tables[0].Rows.Count; n++)
            {
                //Tiene ensamble?
                ensambleA = DBFunctions.SingleData(
                    "SELECT PENS_CODIGO " +
                    "FROM PENSAMBLEPRODUCCION " +
                    "WHERE MITE_CODIGO='" + dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString() + "' AND " +
                    "PENS_VIGENTE='S';");

                if (ensambleA.Length > 0)
                {
                    //Tipo del ensamble (no recorre tipo E)
                    string tipoP = DBFunctions.SingleData(
                        "SELECT TPRO_CODIGO " +
                        "FROM MENSAMBLEPRODUCCIONITEMS " +
                        "WHERE PENS_CODIGO='" + ensamble + "' AND " +
                        "MITE_CODIGO='" + dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString() + "'" +
                        ";");
                    if (tipoP.Equals("E"))
                    {
                        //Agregar Item (materia prima)
                        AgregarItemPedido(dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString(),
                            Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]),
                            cantidad * Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]), true);
                    }
                    else
                    {
                        //Recorrer Ensamble Hijo si existe
                        RecorrerEnsamble(dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString(),
                            cantidad * Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]),
                            ensambleA, contador + 1, itemsAnteriores, true);

                    }
                }
                else
                {
                    //Agregar Hijo (materia prima)
                    AgregarItemPedido(dsSubItems.Tables[0].Rows[n]["CODIGO"].ToString(),
                        Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]),
                        cantidad * Convert.ToDouble(dsSubItems.Tables[0].Rows[n]["CANTIDAD"]), true);
                }
                hijos = true;
            }

            //Ensamble sin hijos?
            if (!hijos)
                AgregarItemPedido(codItem, 1, cantidad, true);
		}

		//Agregar item a tabla de pedidos
		private void AgregarItemPedido(string codItem, double cantidadEnsamble, double cantidad, bool matPrima)
		{
			DataSet dsItem=new DataSet();
			double precio=0, cantSaldo=0;
			string cantDispStr;
			DBFunctions.Request(dsItem,IncludeSchema.NO,
				"SELECT ms.MSAL_COSTPROM "+
				"FROM MSALDOITEM ms "+
				"WHERE ms.MITE_CODIGO='"+codItem+"';"
			);
			
			//Leer precio item si lo encuentra en la lista de precios
			if(dsItem.Tables[0].Rows.Count>0)
				precio=Convert.ToDouble(dsItem.Tables[0].Rows[0]["MSAL_COSTPROM"]);

			//Se debe aprovechar lo que haya en la planta y no pedir mas de lo necesario
			//Consultar (cant actual-cant asignada) del item en la bodega
			if(matPrima){
				cantDispStr=DBFunctions.SingleData(
					"SELECT msal_cantactual-msal_cantasig FROM MSALDOITEMALMACEN "+
					"WHERE PALM_ALMACEN='"+almacen+"' AND MITE_CODIGO='"+codItem+"' AND PANO_ANO="+anoInventario+";"
					);
				if(cantDispStr.Length>0)
					cantSaldo=Convert.ToDouble(cantDispStr);
			}
			
			if(cantSaldo>=cantidad)
			{
				//Agregar items obtenido del mismo almacen
				AgregarItemDtItems(codItem, cantidadEnsamble, precio, 0, 0, cantidad, false);
			}
			else if(cantSaldo>0)
			{
				//Agregar items disponibles en el mismo almacen
				AgregarItemDtItems(codItem, cantidadEnsamble, precio, 0, 0, cantSaldo, false);
				//Pedir lo que falte como materia prima
				AgregarItemDtItems(codItem, cantidadEnsamble, precio, 0, 0, cantidad-cantSaldo, matPrima);
			}
			else
			{
				AgregarItemDtItems(codItem, cantidadEnsamble, precio, 0, 0, cantidad, matPrima);
			}

			totalValorItems+=precio*cantidad;
		}

		//Consulta items y operaciones de un ensamble
		private DataSet TraerInformacionKitsEnsamble(string kit)
		{
			DataSet ds=new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pens_codigo,mite_codigo,mens_cantidad FROM mensambleproduccionitems WHERE pens_codigo='"+kit+"';"+
													"SELECT pens_codigo,ptem_operacion FROM mensambleproduccionoperaciones WHERE pens_codigo='"+kit+"'");
			return ds;
		}

		//Construir tabla items del pedido
		private void ConstruirDtItems()
		{
			dtItems=new DataTable();
			dtItems.Columns.Add("ITEM",typeof(string));
			dtItems.Columns.Add("CANTIDAD",typeof(double));
			dtItems.Columns.Add("PRECIO",typeof(double));
			dtItems.Columns.Add("IVA",typeof(double));
			dtItems.Columns.Add("DESCUENTO",typeof(double));
			dtItems.Columns.Add("CANTPED",typeof(double));
			dtItems.Columns.Add("MATERIAPRIMA",typeof(bool));
		}

		//Agregar item a tabla items del pedido
		private void AgregarItemDtItems(string item,double cantidad,double precio,double iva,double descuento,double cantped,bool matPrima)
		{
			bool encontrado=false;
			if(dtItems.Rows.Count==0)
			{
				DataRow fila=dtItems.NewRow();
				fila["ITEM"]=item;
				fila["CANTIDAD"]=cantidad;
				fila["PRECIO"]=precio;
				fila["IVA"]=iva;
				fila["DESCUENTO"]=descuento;
				fila["CANTPED"]=cantped;
				fila["MATERIAPRIMA"]=matPrima;
				dtItems.Rows.Add(fila);
			}
			else
			{
				for(int i=0;i<dtItems.Rows.Count;i++)
				{
					if(dtItems.Rows[i]["ITEM"].ToString()==item && Convert.ToBoolean(dtItems.Rows[i]["MATERIAPRIMA"])==matPrima)
					{
						dtItems.Rows[i]["CANTIDAD"]=(double)dtItems.Rows[i]["CANTIDAD"]+cantidad;
						dtItems.Rows[i]["CANTPED"]=(double)dtItems.Rows[i]["CANTPED"]+cantped;
						encontrado=true;
						break;
					}
				}
				if(!encontrado){
					DataRow fila=dtItems.NewRow();
					fila["ITEM"]=item;
					fila["CANTIDAD"]=cantidad;
					fila["PRECIO"]=precio;
					fila["IVA"]=iva;
					fila["DESCUENTO"]=descuento;
					fila["CANTPED"]=cantped;
					fila["MATERIAPRIMA"]=matPrima;
					dtItems.Rows.Add(fila);
				}
			}
		}
	}
}
