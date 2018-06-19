using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.Forms;
using AMS.Tools;
using AMS.DB;

namespace AMS.Inventarios
{
	public class ListaEmpaque
	{
		private uint numeroLista;
		private string nitCliente, almacen, usuario, clasePedido, anoInv;
		private DateTime fechaCreacion;
		private ArrayList sqlStrings;
		private DataTable dtDetalle;
		private string processMsg;
		
		public string ProcessMsg{get{return processMsg;}}
		public ArrayList SqlStrings{get{return sqlStrings;}}
		
		//Constructor #1 : Creacion de Lista de empaque 
		public ListaEmpaque(uint numLis, string nitCli, DateTime fechCrea, string almac, string usuar, string clsPed, string kitPed)
		{
			this.numeroLista = numLis;
			this.nitCliente = nitCli;
			this.fechaCreacion = fechCrea;
			this.almacen = almac;
			this.usuario = usuar;
			this.clasePedido = clsPed;
            this.sqlStrings = new ArrayList();
			this.anoInv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			this.CreardtDetalle();
		}
		
		//Constructor #1 : Cargar una lista de empaque creada
		public ListaEmpaque(uint numLis)
		{
			this.numeroLista = numLis;
			this.nitCliente = DBFunctions.SingleData("SELECT mnit_nit FROM mlistaempaque WHERE mlis_numero="+this.numeroLista+"");
			this.fechaCreacion = Convert.ToDateTime(DBFunctions.SingleData("SELECT mlis_fechproc FROM mlistaempaque WHERE mlis_numero="+this.numeroLista+""));
			this.almacen = DBFunctions.SingleData("SELECT palm_almacen FROM mlistaempaque WHERE mlis_numero="+this.numeroLista+"");
			this.usuario = DBFunctions.SingleData("SELECT susu_usuario FROM mlistaempaque WHERE mlis_numero="+this.numeroLista+"");
            this.sqlStrings = new ArrayList();
			this.anoInv = DBFunctions.SingleData("SELECT pano_ano from cinventario");
			this.TraerDetalle();
		}
		
		//Funcion que inicializa la tabla del detalle de la lista de empaque
		private void CreardtDetalle()
		{
			dtDetalle = new DataTable();
			dtDetalle.Columns.Add(new DataColumn("CODIGOITEM",System.Type.GetType("System.String")));//0
			dtDetalle.Columns.Add(new DataColumn("PREFIJOPEDIDO",System.Type.GetType("System.String")));//1
			dtDetalle.Columns.Add(new DataColumn("NUMEROPEDIDO",System.Type.GetType("System.String")));//2
			dtDetalle.Columns.Add(new DataColumn("CANTIDADASIGNADA",System.Type.GetType("System.Int32")));//3
			dtDetalle.Columns.Add(new DataColumn("VALORPUBLICO",System.Type.GetType("System.Double")));//4
		}
		
		private void TraerDetalle()
		{
			this.CreardtDetalle();
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT mite_codigo, pped_codigo, mped_numepedi, dped_cantasig, dlis_valopubl FROM dlistaempaque WHERE mlis_numero="+this.numeroLista+"");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				DataRow fila = dtDetalle.NewRow();
				fila[0] = ds.Tables[0].Rows[i][0].ToString();
				fila[1] = ds.Tables[0].Rows[i][1].ToString();
				fila[2] = ds.Tables[0].Rows[i][2].ToString();
				fila[3] = Convert.ToInt32(ds.Tables[0].Rows[i][3]);
				fila[4] = Convert.ToDouble(ds.Tables[0].Rows[i][4]);
				dtDetalle.Rows.Add(fila);
			}
		}
		
		public void AgregarItem(string codigoItem, string prefijoPedido, string numeroPedido, int cantidadAsignada, double valorPublico)
		{
			DataRow fila = this.dtDetalle.NewRow();
			fila[0] = codigoItem;
			fila[1] = prefijoPedido;
			fila[2] = numeroPedido;
			fila[3] = cantidadAsignada;
			fila[4] = valorPublico;
			dtDetalle.Rows.Add(fila);
		}
		
		public bool BuscarItemAgregado(string codI)
		{
			bool encontrado = false;
			DataRow[] selection = dtDetalle.Select("CODIGOITEM='"+codI+"'");
			if(selection.Length>0)
				encontrado = true;
			return encontrado;
		}
		
		public bool AlmacenarLista(bool almacenarLista)
		{
			bool status = true;
			sqlStrings.Clear();
			//Creamos el registro en la tabla maestra de listas de empaque
			sqlStrings.Add("INSERT INTO mlistaempaque VALUES("+this.numeroLista.ToString()+",'"+this.nitCliente+"','"+this.fechaCreacion.ToString("yyyy-MM-dd")+"','"+this.almacen+"','"+this.usuario+ "',null)");
			//Ahora debemos modificar el el valor en las tablas por cada item ingresado:
			//Dpedidoitem : Aumentar la cantidad asignada
			//Msaldoitem :  Aumentar la cantidad asignada
			//Msaldoitemalmacen :  Aumentar la cantidad asignada
			//Ademas agregamos la 
			for(int i=0;i<dtDetalle.Rows.Count;i++)
			{
				sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig+"+dtDetalle.Rows[i][3].ToString()+"  WHERE mnit_nit='"+this.nitCliente+"' AND pped_codigo='"+dtDetalle.Rows[i][1].ToString()+"' AND mped_numepedi="+dtDetalle.Rows[i][2].ToString()+" AND mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"'");
				sqlStrings.Add("UPDATE msaldoitem SET msal_cantasig=msal_cantasig+"+dtDetalle.Rows[i][3].ToString()+"  WHERE mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"' AND pano_ano="+this.anoInv+"");
				sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig=msal_cantasig+"+dtDetalle.Rows[i][3].ToString()+"  WHERE mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"' AND palm_almacen='"+this.almacen+"' AND pano_ano="+this.anoInv+"");
				string porcentajeDescuento = DBFunctions.SingleData("SELECT coalesce(dped_porcdesc,0) FROM dpedidoitem WHERE mnit_nit='" + this.nitCliente+"' AND pped_codigo='"+dtDetalle.Rows[i][1].ToString()+"' AND mped_numepedi="+dtDetalle.Rows[i][2].ToString()+" AND mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"'");
				string porcentajeIva = DBFunctions.SingleData("SELECT coalesce(piva_porciva,0) FROM dpedidoitem WHERE mnit_nit='" + this.nitCliente+"' AND pped_codigo='"+dtDetalle.Rows[i][1].ToString()+"' AND mped_numepedi="+dtDetalle.Rows[i][2].ToString()+" AND mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"'");
				string costoPromedio = DBFunctions.SingleData("SELECT coalesce(msal_costprom,0) FROM msaldoitem WHERE mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"' AND pano_ano="+this.anoInv);
				if(porcentajeDescuento.Length==0)porcentajeDescuento="NULL";
				if(porcentajeIva.Length==0)porcentajeIva="NULL";
                if (costoPromedio.Length == 0) costoPromedio = "0";
                double DOcostoPromedio = Convert.ToDouble(costoPromedio);
                
				sqlStrings.Add("INSERT INTO dlistaempaque VALUES("+this.numeroLista+",'"+dtDetalle.Rows[i][0].ToString()+"','"+this.clasePedido+"','"+this.nitCliente+"','"+dtDetalle.Rows[i][1].ToString()+"',"+
                    dtDetalle.Rows[i][2].ToString()+","+dtDetalle.Rows[i][3].ToString()+","+porcentajeDescuento+","+porcentajeIva+","+dtDetalle.Rows[i][4].ToString()+","+DOcostoPromedio.ToString()+")");
			}
			//Ahora revisamo si se va almacenar esta lista de empaque
			if(almacenarLista)
			{
				if(DBFunctions.Transaction(sqlStrings))
					this.processMsg +="<br>Bien <br>"+ DBFunctions.exceptions;
				else
				{
					status = false;
					this.processMsg +="<br>ERROR<br>"+ DBFunctions.exceptions;
				}
			}
			return status;
		}
		
		public bool ModificarLista(DataTable dtModificacion, DataTable dtNuevos)
		{
            
			int i,j;
			bool status = true;
			int mesInv;
			DataSet dsDLista=new DataSet();
			DataRow drDLista;
			sqlStrings.Clear();
			//INSERCIONES
			DBFunctions.Request(dsDLista,IncludeSchema.NO,
				"SELECT DP.MPED_CLASREGI,DP.MNIT_NIT,DP.PPED_CODIGO,DP.MPED_NUMEPEDI FROM DLISTAEMPAQUE DL,DPEDIDOITEM DP WHERE DP.PPED_CODIGO=DL.PPED_CODIGO AND DP.MPED_NUMEPEDI=DL.MPED_NUMEPEDI AND DL.MLIS_NUMERO="+this.numeroLista+" ORDER BY DP.MPED_NUMEPEDI DESC FETCH FIRST 1 ROWS ONLY;");
			//No hay detalles, no se pueden agregar items
			if(dtModificacion.Rows.Count>0 && dsDLista.Tables[0].Rows.Count==0)
				return(false);
			mesInv=Convert.ToInt16(DBFunctions.SingleData("SELECT pmes_mes from cinventario"));
            if (dsDLista.Tables[0].Rows.Count == 0)
            {
                this.processMsg="No se encontró el detalle de la lista de empaque!";
                return false;
            }
			drDLista=dsDLista.Tables[0].Rows[0];
            //Agregar nuevos a ultimo pedido de lista


            for (int n = 0; n < dtNuevos.Rows.Count; n++)
            {
                string codI = dtNuevos.Rows[n][0].ToString();	//Codigo del Item 
                //hace la verificacion y en dado caso lo convierte al item Normal 
                string ItemC = "";
                Referencias.Guardar(codI, ref ItemC, DBFunctions.SingleData("SELECT plin_tipo FROM plineaitem WHERE plin_codigo= " + dtNuevos.Rows[n][11].ToString() + ";"));
                double CantPed = Convert.ToDouble(dtNuevos.Rows[n][2]); //Cantidad	Pedida 
                double ca = Referencias.ConsultarAsignacion(ItemC, this.almacen, CantPed);//Cantidad Asig
                double IValU = Convert.ToDouble(dtNuevos.Rows[n][4]);//Valor del item Unitario
                double IIva = Convert.ToDouble(dtNuevos.Rows[n][5]);//Valor del Iva para	este Item
                double IDesc = Convert.ToDouble(dtNuevos.Rows[n][6]);//Porcentaje de Descuento	para este Item
                string dem = DBFunctions.SingleData("SELECT	tped_demanda from tpedido where	tped_codigo=(SELECT	tped_codigo	FROM ppedido WHERE pped_codigo='" + drDLista["PPED_CODIGO"] + "')");//Demanda?
                string bko = DBFunctions.SingleData("SELECT	tped_backorder from	tpedido	where tped_codigo=(SELECT tped_codigo FROM ppedido WHERE pped_codigo='" + drDLista["PPED_CODIGO"] + "')");//BackOrder?


                //if(!(ca>0))continue;
                //DPEDIDOITEM
                if (!DBFunctions.RecordExist("SELECT MITE_CODIGO FROM DPEDIDOITEM WHERE MPED_CLASREGI = '" + drDLista["MPED_CLASREGI"] + "' AND MNIT_NIT = '" + drDLista["MNIT_NIT"] + "' AND PPED_CODIGO = '" + drDLista["PPED_CODIGO"] + "' AND MPED_NUMEPEDI = " + drDLista["MPED_NUMEPEDI"] + " AND MITE_CODIGO = '" + ItemC + "' "))
                {
                    sqlStrings.Add(
                        "insert into DPEDIDOITEM " +
                        "(MPED_CLASREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,MITE_CODIGO,DPED_CANTPEDI,DPED_CANTASIG,DPED_CANTFACT,DPED_VALOUNIT,DPED_PORCDESC,PIVA_PORCIVA) VALUES (" +
                        "'" + drDLista["MPED_CLASREGI"] + "','" + drDLista["MNIT_NIT"] + "','" + drDLista["PPED_CODIGO"] + "'," + drDLista["MPED_NUMEPEDI"] + ",'" + ItemC + "'," + CantPed.ToString() + "," + ca.ToString() + ",0," + IValU.ToString() + "," + IDesc.ToString() + "," + IIva.ToString() + ")");
                    //SALDOS
                    if (!DBFunctions.RecordExist("SELECT	* FROM msaldoitem WHERE	mite_codigo	= '" + ItemC + "' AND PANO_ANO=" + this.anoInv))
                        sqlStrings.Add("insert into	msaldoitem (mite_codigo,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist,msal_costhist,msal_costhisthist,msal_cantinveinic,msal_ultivent,msal_ultiingr,msal_ulticost,msal_ultiprov,msal_abcd,msal_peditrans,msal_unidtrans,msal_pedipendi,msal_unidpendi)	values ('" + ItemC + "'," + this.anoInv + ",0,0,0,0,0,0,0,null,null,0,null,null,0,0,0,0)");

                    if (!DBFunctions.RecordExist("SELECT	* FROM msaldoitemalmacen WHERE mite_codigo = '" + ItemC + "' AND PALM_ALMACEN='" + this.almacen + "'	and	pano_ano=" + this.anoInv))
                        sqlStrings.Add("insert into	msaldoitemalmacen (mite_codigo,palm_almacen,pano_ano,msal_cantactual,msal_cantasig,msal_costprom,msal_costpromhist)	values ('" + ItemC + "','" + this.almacen + "'," + this.anoInv + ",0,0,0,0)");

                    if (!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEMALMACEN WHERE pano_ano=" + this.anoInv + " and pmes_mes=" + mesInv + " and tmov_tipomovi=70	AND	mite_codigo	= '" + ItemC + "' AND PALM_ALMACEN='" + this.almacen + "'"))
                        sqlStrings.Add("insert into	MACUMULADOITEMALMACEN (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,PALM_ALMACEN,macu_cantidad,macu_costo,macu_precio) values ('" + ItemC + "',70," + this.anoInv + "," + mesInv + ",'" + this.almacen + "',0,0,0)");

                    if (!DBFunctions.RecordExist("SELECT	* FROM MACUMULADOITEM WHERE	pano_ano=" + this.anoInv + " and pmes_mes=" + mesInv + "	and	tmov_tipomovi=70 AND mite_codigo = '" + ItemC + "'"))
                        sqlStrings.Add("insert into	MACUMULADOITEM (mite_codigo,TMOV_TIPOMOVI,PANO_ANO,PMES_MES,macu_cantidad,macu_costo,macu_precio) values ('" + ItemC + "',70," + this.anoInv + "," + mesInv + ",0,0,0)");

                    if (bko == "S" && (CantPed > 0)) //Genera backorder
                    {
                        sqlStrings.Add("update msaldoitemalmacen set msal_cantpendiente=msal_cantpendiente+" + (CantPed).ToString() + " where mite_codigo='" + ItemC + "' and pano_ano=" + this.anoInv + " and palm_almacen='" + this.almacen + "'");
                        sqlStrings.Add("update msaldoitem set msal_unidpendi=msal_unidpendi+" + (CantPed).ToString() + ", msal_pedipendi=msal_pedipendi+1 where mite_codigo='" + ItemC + "' and pano_ano=" + this.anoInv);
                    }
                    if (dem == "S")
                    {   //Genera demanda
                        sqlStrings.Add("update macumuladoitemalmacen set macu_cantidad=macu_cantidad+" + CantPed.ToString() + "	WHERE pano_ano=" + this.anoInv + "	and	pmes_mes=" + mesInv + " and tmov_tipomovi=70 AND mite_codigo = '" + ItemC + "' AND PALM_ALMACEN='" + this.almacen + "'");
                        sqlStrings.Add("update macumuladoitem set macu_cantidad=macu_cantidad+" + CantPed.ToString() + " WHERE pano_ano=" + this.anoInv + " and pmes_mes=" + mesInv + " and tmov_tipomovi=70 AND mite_codigo	= '" + ItemC + "'");
                    }

                    if (ca > 0)
                    {
                        //MLIS_NUMERO, MITE_CODIGO,CLASEREGI,		  NIT,		   PEDICOD,	NUMEPEDI,	  DPED_CANTASIG,							 DLIS_PORCDESC,								  PIVA_PORCIVA,							   VALOPUBL,							  VALOUNIT
                        //Genera Demanda?
                        if (dem == "S")
                        {
                            if (!DBFunctions.RecordExist("SELECT	* FROM MDEMANDAITEMALMACEN WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + this.anoInv + "	and	pmes_mes=" + mesInv + "	and	palm_almacen='" + almacen + "'"))
                                sqlStrings.Add("insert into	MDEMANDAITEMALMACEN	(MITE_CODIGO,pano_ano,pmes_mes,palm_almacen,mdem_cantidad) values ('" + ItemC + "'," + this.anoInv + "," + mesInv + ",'" + almacen + "'," + CantPed.ToString() + ")");
                            else
                                sqlStrings.Add("update MDEMANDAITEMALMACEN set mdem_cantidad=mdem_cantidad+" + CantPed.ToString() + " WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + this.anoInv + "	and	pmes_mes=" + mesInv + "	and	palm_almacen='" + almacen + "'");
                            if (!DBFunctions.RecordExist("SELECT	* FROM MDEMANDAITEM	WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + this.anoInv + " and pmes_mes=" + mesInv + ""))
                                sqlStrings.Add("insert into	MDEMANDAITEM (MITE_CODIGO,pano_ano,pmes_mes,mdem_cantidad) values ('" + ItemC + "'," + this.anoInv + "," + mesInv + "," + CantPed.ToString() + ")");
                            else
                                sqlStrings.Add("update MDEMANDAITEM	set	mdem_cantidad=mdem_cantidad+" + CantPed.ToString() + "	WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + this.anoInv + " and pmes_mes=" + mesInv + "");
                        }
                    }
                }
                else
                    sqlStrings.Add(("UPDATE DPEDIDOITEM SET DPED_CANTASIG = DPED_CANTASIG + " + ca.ToString() + ", DPED_VALOUNIT = "+ IValU +", DPED_PORCDESC = "+ IDesc + " WHERE MPED_CLASREGI = '" + drDLista["MPED_CLASREGI"] + "' AND MNIT_NIT = '" + drDLista["MNIT_NIT"] + "' AND PPED_CODIGO = '" + drDLista["PPED_CODIGO"] + "' AND MPED_NUMEPEDI = " + drDLista["MPED_NUMEPEDI"] + " AND MITE_CODIGO = '" + ItemC + "' "));

                // genéricos independientemente que el registro exista o sea nuevo
                sqlStrings.Add("update msaldoitem        set msal_cantasig=msal_cantasig + " + ca.ToString() + " where mite_codigo='" + ItemC + "' and pano_ano=" + this.anoInv);
                sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig=msal_cantasig + " + ca.ToString() + " WHERE mite_codigo='" + ItemC + "' AND pano_ano=" + this.anoInv + " AND palm_almacen='" + this.almacen + "'");
                if (ca > 0)
                {
                    double vp = Convert.ToDouble(DBFunctions.SingleData("SELECT msal_costprom	FROM MSALDOITEM	WHERE MITE_CODIGO='" + ItemC + "' and pano_ano=" + this.anoInv));
                    sqlStrings.Add("insert into	DLISTAEMPAQUE (MLIS_NUMERO,MITE_CODIGO,MPED_CLASEREGI,MNIT_NIT,PPED_CODIGO,MPED_NUMEPEDI,DPED_CANTASIG,DLIS_PORCDESC,PIVA_PORCIVA,DLIS_VALOPUBL,DLIS_VALOUNIT) values (" +
                    this.numeroLista + ",'" + ItemC + "','" + drDLista["MPED_CLASREGI"] + "','" + drDLista["MNIT_NIT"] + "','" + drDLista["PPED_CODIGO"] + "'," + drDLista["MPED_NUMEPEDI"] + "," + ca.ToString() + "," + IDesc.ToString() + "," + IIva.ToString() + "," + IValU.ToString() + "," + vp.ToString() + ")");
                }
            }
                //ACTUALIZACIONES
                //Empezamos a recorrer la tabla que acaba de ingresar y realizamos la actualizacion en la base de datos
                //Y eliminamos el registro de la tabla dtDetalle
            for (i=0;i<dtModificacion.Rows.Count;i++)
			{
				int cantidadModificada = Convert.ToInt32(dtModificacion.Rows[i][8]) - Convert.ToInt32(dtModificacion.Rows[i][9]);
                int cantidadPendiente = Convert.ToInt32(dtModificacion.Rows[i]["CANTIDADPENDIENTE"]);

                double cantidadAsignada = 0;
				string clsPed = DBFunctions.SingleData("SELECT mped_claseregi FROM mpedidoitem WHERE pped_codigo='"+dtModificacion.Rows[i][4].ToString()+"' AND mped_numepedi="+dtModificacion.Rows[i][5].ToString()+"");
				if(cantidadModificada>0)
				{
					double cantD = Referencias.ConsultarDisponibilidad(dtModificacion.Rows[i][0].ToString(),this.almacen,0);
					if(cantidadModificada>cantD)
						cantidadAsignada=cantD;
					else
						cantidadAsignada=cantidadModificada;
                    cantidadPendiente = cantidadPendiente * -1;
                    sqlStrings.Add("UPDATE dlistaempaque SET dped_cantasig=dped_cantasig+"+cantidadAsignada+" WHERE mlis_numero="+this.numeroLista+" AND mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"' AND pped_codigo='"+dtModificacion.Rows[i][4].ToString()+"' AND mped_numepedi="+dtModificacion.Rows[i][5].ToString()+"");
					sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig+"+cantidadAsignada+"  WHERE mped_clasregi='"+clsPed+"' AND mnit_nit='"+this.nitCliente+"' AND pped_codigo='"+dtModificacion.Rows[i][4].ToString()+"' AND mped_numepedi="+dtModificacion.Rows[i][5].ToString()+" AND mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"'");
					sqlStrings.Add("UPDATE msaldoitem SET msal_cantasig=msal_cantasig+"+cantidadAsignada+ ", msal_unidpendi=msal_unidpendi+" + cantidadPendiente + "  WHERE mite_codigo='" + dtModificacion.Rows[i][0].ToString()+"' AND pano_ano="+this.anoInv+"");
					sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig=msal_cantasig+"+cantidadAsignada+ ", msal_cantPENDIENTE=msal_cantPENDIENTE+" + cantidadPendiente + "  WHERE mite_codigo='" + dtModificacion.Rows[i][0].ToString()+"' AND palm_almacen='"+this.almacen+"' AND pano_ano="+this.anoInv+"");
				}
				else if(cantidadModificada<0)
				{
					cantidadModificada = cantidadModificada*(-1);
					sqlStrings.Add("UPDATE dlistaempaque SET dped_cantasig=dped_cantasig-"+cantidadModificada+" WHERE mlis_numero="+this.numeroLista+" AND mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"' AND pped_codigo='"+dtModificacion.Rows[i][4].ToString()+"' AND mped_numepedi="+dtModificacion.Rows[i][5].ToString()+"");
					sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig-"+cantidadModificada+"  WHERE mped_clasregi='"+clsPed+"' AND mnit_nit='"+this.nitCliente+"' AND pped_codigo='"+dtModificacion.Rows[i][4].ToString()+"' AND mped_numepedi="+dtModificacion.Rows[i][5].ToString()+" AND mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"'");
					sqlStrings.Add("UPDATE msaldoitem SET msal_cantasig=msal_cantasig-"+cantidadModificada+"  WHERE mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"' AND pano_ano="+this.anoInv+"");
					sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig=msal_cantasig-"+cantidadModificada+"  WHERE mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"' AND palm_almacen='"+this.almacen+"' AND pano_ano="+this.anoInv+"");
				}
				sqlStrings.Add("UPDATE dpedidoitem SET dped_cantpedi=dped_cantasig+dped_cantfact where (dped_cantasig+dped_cantfact)>dped_cantpedi and mped_clasregi='"+clsPed+"' AND mnit_nit='"+this.nitCliente+"' AND pped_codigo='"+dtModificacion.Rows[i][4].ToString()+"' AND mped_numepedi="+dtModificacion.Rows[i][5].ToString()+" AND mite_codigo='"+dtModificacion.Rows[i][0].ToString()+"'");
				//Ahora eliminamos el registro de la tabla dtDetalle
				DataRow[] selection = dtDetalle.Select("CODIGOITEM='"+dtModificacion.Rows[i][0].ToString()+"' AND PREFIJOPEDIDO='"+dtModificacion.Rows[i][4].ToString()+"' AND NUMEROPEDIDO='"+dtModificacion.Rows[i][5].ToString()+"'");
				for(j=0;j<selection.Length;j++)
					dtDetalle.Rows.Remove(selection[j]);
			}
			//Ahora los registros que permanecieron en la tabla de dtDetalle son los que fueron eliminados
			for(i=0;i<dtDetalle.Rows.Count;i++)
			{
				string clsPed = DBFunctions.SingleData("SELECT mped_claseregi FROM mpedidoitem WHERE pped_codigo='"+dtDetalle.Rows[i][1].ToString()+"' AND mped_numepedi="+dtDetalle.Rows[i][2].ToString()+"");
				sqlStrings.Add("UPDATE dpedidoitem SET dped_cantasig=dped_cantasig-"+dtDetalle.Rows[i][3].ToString()+"  WHERE mped_clasregi='"+clsPed+"' AND mnit_nit='"+this.nitCliente+"' AND pped_codigo='"+dtDetalle.Rows[i][1].ToString()+"' AND mped_numepedi="+dtDetalle.Rows[i][2].ToString()+" AND mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"'");
				sqlStrings.Add("UPDATE msaldoitem SET msal_cantasig=msal_cantasig-"+dtDetalle.Rows[i][3].ToString()+"  WHERE mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"' AND pano_ano="+this.anoInv+"");
				sqlStrings.Add("UPDATE msaldoitemalmacen SET msal_cantasig=msal_cantasig-"+dtDetalle.Rows[i][3].ToString()+"  WHERE mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"' AND palm_almacen='"+this.almacen+"' AND pano_ano="+this.anoInv+"");
				sqlStrings.Add("DELETE FROM dlistaempaque WHERE mlis_numero="+this.numeroLista+" AND mite_codigo='"+dtDetalle.Rows[i][0].ToString()+"' AND pped_codigo='"+dtDetalle.Rows[i][1].ToString()+"' AND mped_numepedi="+dtDetalle.Rows[i][2].ToString()+"");
			}
			if(DBFunctions.Transaction(sqlStrings))
				this.processMsg +="<br>Bien <br>"+ DBFunctions.exceptions;
			else
			{
				status = false;
				this.processMsg +="<br>ERROR<br>"+ DBFunctions.exceptions;
			}
			dtDetalle.Clear();
			this.TraerDetalle();
			return status;
		}
	}
}
