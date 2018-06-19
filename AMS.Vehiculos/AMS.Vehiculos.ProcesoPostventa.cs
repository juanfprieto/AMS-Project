using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Inventarios;
using AMS.Documentos;

namespace AMS.Vehiculos
{
	/// <summary>
	/// Clase que se encarga de los procesos de postventa.
	/// </summary>
    public class ProcesoPostventa : System.Web.UI.UserControl
	{
		private string VIN;
		private string placa;
		public string kit;
		private string prefijoFactura,prefijoOrdenAsociada,fechaFactura,fechaProceso,nitDistribuidor,nitPropietario,nombrePropietario,observCliente,observTaller;
		private UInt32 numeroFactura,numeroOrdenAsociada;
		private UInt32 kilometraje;
		private DataTable dtOperaciones,dtRepuestos;
		public string prefijoOrden,numeroOrden,direccionPropietario,telefonoPropietario,celularPropietario,ciudadPropietario;
		private ProcesosPostventa tipoProceso;
		private bool pagaFabrica;
        //private FormatosDocumentos formatoFactura=new FormatosDocumentos();
       

		public ProcesoPostventa(ProcesosPostventa tProceso,string kitO, string vVIN,string vplaca,DataTable dtOpers,DataTable dtReps, string prefijoF, UInt32 numeroF, string prefijoOA, UInt32 numeroOA, string fechaF, string fechaP, string nitDist, string nitProp, string nomProp, string telProp, string dirProp, string celProp, string ciuProp, string obsCliente, string obsTaller,UInt32 kmtrj, bool pagaF)
		{
			tipoProceso=tProceso;
			kit=kitO;
			VIN=vVIN;
			placa=vplaca;
			prefijoFactura=prefijoF;
			numeroFactura=numeroF;
			dtOperaciones=dtOpers;
			dtRepuestos=dtReps;
			fechaFactura=fechaF;
			fechaProceso=fechaP;
			prefijoOrdenAsociada=prefijoOA;
			numeroOrdenAsociada=numeroOA;
			nitDistribuidor=nitDist;
			observCliente=obsCliente;
			observTaller=obsTaller;
			kilometraje=kmtrj;
			pagaFabrica=pagaF;
			nitPropietario=nitProp;
			nombrePropietario=nomProp;
			direccionPropietario=dirProp;
			telefonoPropietario=telProp;
			ciudadPropietario=ciuProp;
			celularPropietario=celProp;
		}

		public string Ejecutar()
		{
			ArrayList sqlAct=new ArrayList();
			DataSet dsVehiculo=new DataSet();
			DataSet dsCtaller=new DataSet();
			DBFunctions.Request(dsVehiculo,IncludeSchema.NO,"SELECT PCAT_CODIGO,MNIT_NIT,MCAT_PLACA, MCAT_NUMEULTIKILO FROM MCATALOGOVEHICULO where MCAT_VIN='"+VIN+"';");
			DBFunctions.Request(dsCtaller,IncludeSchema.NO,"SELECT PDOC_CODIPOST,PVEN_CODIPOST,PALM_ALMAPOST,KILOPROM_MENSUAL FROM CTALLER;");
			if(dsVehiculo.Tables[0].Rows.Count==0)
				return("No se encontró el vehículo.");
          
			string catalogo=dsVehiculo.Tables[0].Rows[0]["PCAT_CODIGO"].ToString();
			string nitCliente=dsVehiculo.Tables[0].Rows[0]["MNIT_NIT"].ToString();
			string placaAnt=dsVehiculo.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			string nitRecepcionista=dsCtaller.Tables[0].Rows[0]["PVEN_CODIPOST"].ToString();
			string almacen=dsCtaller.Tables[0].Rows[0]["PALM_ALMAPOST"].ToString();
			UInt32 kiloAnt=Convert.ToUInt32(dsVehiculo.Tables[0].Rows[0]["MCAT_NUMEULTIKILO"]);
			string listaPrecio=DBFunctions.SingleData("SELECT PPRETA_CODIGO FROM DBXSCHEMA.PPRECIOTALLER;");
			string estadoOrden=(pagaFabrica?"A":"F");
			string proceso="";
			UInt32 kiloProm=Convert.ToUInt32(dsCtaller.Tables[0].Rows[0]["KILOPROM_MENSUAL"]);
			
			prefijoOrden=dsCtaller.Tables[0].Rows[0]["PDOC_CODIPOST"].ToString();
			numeroOrden="";
			
			if(tipoProceso==ProcesosPostventa.Ninguno)
				return("Proceso indefinido.");
			if(prefijoOrden.Length==0)
				return("Debe definir el prefijo de las ordenes de trabajo.");
         
			if(tipoProceso==ProcesosPostventa.Alistamiento)proceso="A";
			else if(tipoProceso==ProcesosPostventa.Revision)proceso="R";
			else if(tipoProceso==ProcesosPostventa.Garantia)proceso="G";
			//Actualizar placa
			if(placaAnt!=placa){
				if(DBFunctions.RecordExist("SELECT MCAT_PLACA FROM mcatalogovehiculo where MCAT_PLACA='"+placa+"';"))
					return("Ya existe la placa seleccionada.");
				sqlAct.Add("update mcatalogovehiculo set mcat_placa='"+placa.ToUpper()+"' where mcat_vin='"+VIN+"';");
			}

			//Actualizar Estado vehiculo (MVEHICULO)
			sqlAct.Add("update mvehiculo set test_tipoesta=60 where mcat_vin='"+VIN+"';");

			//Actualizar Propietario (MCATALOGOVEHICULO)
			//sqlAct.Add("update MCATALOGOVEHICULO set mnit_nit='"+nitPropietario+"' where mcat_vin='"+VIN+"';");

			//Actualizar Kilometraje (mcatalogovehiculo)
			if(kiloAnt>kilometraje)
				return("El nuevo kilometraje no puede ser menor al anterior.");
			if(kiloAnt<kilometraje){
                sqlAct.Add("update mcatalogovehiculo set mcat_numeultikilo=" + kilometraje + ",mcat_fechultikilo='"+fechaProceso+"' where mcat_vin='" + VIN + "';");
				if(tipoProceso!=ProcesosPostventa.Alistamiento)
				{
					//kilometraje al mes o default
					string fechaKilo=DBFunctions.SingleData("SELECT max(mord_creacion) from DBXSCHEMA.morden where mcat_vin='"+VIN+"';");
					if(fechaKilo.Length>0){
						fechaKilo=Convert.ToDateTime(fechaKilo).ToString("yyyy-MM-dd");
						if(!fechaKilo.Equals(fechaProceso))
							sqlAct.Add(
								"UPDATE MCATALOGOVEHICULO "+
								"SET MCAT_NUMEKILOPROM=(30*"+(kilometraje-kiloAnt)+")/ABS(DAYS('"+fechaProceso+"')-DAYS('"+fechaKilo+"')) "+
								"where mcat_vin='"+VIN+"';");
					}
					else
						sqlAct.Add(
							"UPDATE MCATALOGOVEHICULO "+
							"SET MCAT_NUMEKILOPROM="+kiloProm+" where mcat_vin='"+VIN+"';");
				}
			}

			//Actualizar Orden documentos
			numeroOrden=DBFunctions.SingleData("select pdoc_ultidocu+1 from dbxschema.pdocumento where pdoc_codigo='"+prefijoOrden+"';");
			sqlAct.Add("update pdocumento set pdoc_ultidocu=pdoc_ultidocu+1 where pdoc_codigo='"+prefijoOrden+"';");
			//Insertar orden de trabajo (MORDEN)
			sqlAct.Add("insert into morden values("+
				"'"+prefijoOrden+"',"+numeroOrden+",NULL,'"+VIN+"','"+nitCliente+"','P','"+estadoOrden+"','C',"+
				"'M','"+fechaProceso+"','00:00:00','"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"',"+
				"'"+fechaProceso+"','00:00:00',NULL,'0',"+kilometraje+",'"+nitRecepcionista+"','"+almacen+"',"+
				"NULL,NULL,NULL,'"+observTaller+"','"+observCliente+"',NULL,NULL,'"+listaPrecio+"','E','1','C',"+
				"NULL,NULL,NULL,NULL,'"+this.nombrePropietario.Trim()+"');");

			//Verificar secuencia
			bool alistado=DBFunctions.RecordExist("select * FROM MVEHICULOPOSTVENTA where mcat_vin='"+VIN+"';");
			if(tipoProceso==ProcesosPostventa.Alistamiento && alistado)
				return("Ya realizó el alistamiento del vehículo seleccionado.");
			if(tipoProceso!=ProcesosPostventa.Alistamiento && !alistado)
				return("No se ha realizado el alistamiento del vehículo seleccionado.");
			
			//Ingresar Kit-orden(dordenkit)
			//Revisar que no exista
			if(tipoProceso!=ProcesosPostventa.Garantia && DBFunctions.RecordExist(
				"SELECT d.pkit_codigo from DBXSCHEMA.DORDENKIT d, dbxschema.morden mo "+
				"where mcat_vin='"+VIN+"' and d.pdoc_codigo=mo.pdoc_codigo and d.mord_numeorde=mo.mord_numeorde and d.pkit_codigo='"+kit+"';"))
				return("Ya se realizó el mantenimiento seleccionado al vehículo.");
			 
			if(tipoProceso!=ProcesosPostventa.Garantia)
				sqlAct.Add("insert into dordenkit values('"+prefijoOrden+"',"+numeroOrden+",'"+kit+"');");
			
			//Ingresar Operaciones(dordenoperacion)
			string causal,incidente;
			for(int n=0;n<dtOperaciones.Rows.Count;n++){
				if(Convert.ToInt16(dtOperaciones.Rows[n]["USAR"])==1)
				{
					incidente=dtOperaciones.Rows[n]["INCIDENTE"].ToString();
					incidente=(incidente.Length>0)?"'"+incidente+"'":"NULL";
					causal=dtOperaciones.Rows[n]["CAUSAL"].ToString();
					causal=(causal.Length>0)?"'"+causal+"'":"NULL";
					sqlAct.Add("insert into dordenoperacion values('"+prefijoOrden+"',"+numeroOrden+",'"+dtOperaciones.Rows[n]["CODIGO"]+"','A','"+nitRecepcionista+"','"+fechaProceso+"','A',"+incidente+","+causal+",NULL,NULL,NULL,0,NULL,NULL);");
				}
			}
			
			//Ingresar Repuestos(DORDENITEMSPOSTVENTA)
			for(int n=0;n<dtRepuestos.Rows.Count;n++){
				if(Convert.ToInt16(dtRepuestos.Rows[n]["USAR"])==1)
					sqlAct.Add("insert into DORDENITEMSPOSTVENTA values('"+prefijoOrden+"',"+numeroOrden+",'"+dtRepuestos.Rows[n]["CODIGO"]+"',"+Convert.ToDouble(dtRepuestos.Rows[n]["PRECIO"]).ToString()+","+dtRepuestos.Rows[n]["CANTIDAD"].ToString()+",'A',0);");
			}

			//Ingresar orden-postventa
			sqlAct.Add("insert into mordenpostventa values('"+prefijoOrden+"',"+numeroOrden+",'"+proceso+"','"+nitDistribuidor+"','"+prefijoOrdenAsociada+"',"+numeroOrdenAsociada+",NULL);");

			//Actualizaciones especiales Alistamiento
            if (tipoProceso == ProcesosPostventa.Alistamiento)
            {
                //postventa(MVEHICULOPOSTVENTA)
                sqlAct.Insert(0, "INSERT INTO MVEHICULOPOSTVENTA VALUES('" + catalogo + "','" + VIN + "','" + nitDistribuidor + "','" + fechaFactura + "','" + prefijoFactura + "'," + numeroFactura + ",'" + nitPropietario + "','" + nombrePropietario + "','" + direccionPropietario + "','" + telefonoPropietario + "','" + celularPropietario + "','" + ciudadPropietario + "');");
                //Revisar estado vehiculo
                if (!DBFunctions.RecordExist("SELECT mv.mcat_vin VIN,mc.mcat_motor MOTOR FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin='" + VIN + "' and mv.mcat_vin=mc.mcat_vin and mv.test_tipoesta=40;"))
                    return ("El vehículo no se encuentra disponible.");
            }
            else
            {
                sqlAct.Insert(0, "UPDATE MVEHICULOPOSTVENTA " +
                "SET MNIT_NITCLIE='" + nitPropietario + "', MNIT_NOMBRECLIE='" + nombrePropietario + "', MNIT_DIRECCIONCLIE='" + direccionPropietario + "'," +
                "MNIT_TELEFONOCLIE='" + telefonoPropietario + "',MNIT_CELULARCLIE='" + celularPropietario + "',PCIU_CODIGOCLIE='" + ciudadPropietario + "' " +
                "WHERE MCAT_VIN='" + VIN + "';");

                //archiva el historial de propietarios del vehiculo.
                DataSet dsPropietario1 = new DataSet();
                DBFunctions.Request(dsPropietario1, IncludeSchema.NO,
                "SELECT * FROM MVEHICULOPOSTVENTAPROPIETARIO where MNIT_NOMBRECLIE='" + nombrePropietario + "';");

                if (dsPropietario1.Tables[0].Rows.Count == 0)
                {
                    sqlAct.Insert(0, "INSERT INTO MVEHICULOPOSTVENTAPROPIETARIO (MCAT_VIN,MVEH_FECHVENT,MNIT_NITCLIE,MNIT_NOMBRECLIE,MNIT_DIRECCIONCLIE,MNIT_TELEFONOCLIE,MNIT_CELULARCLIE,PCIU_CODIGOCLIE)  VALUES('" + VIN + "','" + fechaFactura + "','" + nitPropietario + "','" + nombrePropietario + "','" + direccionPropietario + "','" + telefonoPropietario + "','" + celularPropietario + "','" + ciudadPropietario + "');");
                }
            }
			if(DBFunctions.Transaction(sqlAct))
            {
                ImprimirFormato(prefijoOrden, numeroOrden);
				return("");
            }
			else
				return(DBFunctions.exceptions);
		}

        protected void ImprimirFormato(Object prefijo, Object numero)
        {
            FormatosDocumentos formatoFactura = new FormatosDocumentos();
            try
            {
                formatoFactura.Prefijo = Convert.ToString(prefijo);
                formatoFactura.Numero = Convert.ToInt32(numero);
                formatoFactura.Codigo = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + formatoFactura.Prefijo + "'");
                if (formatoFactura.Codigo != string.Empty)
                {
                    if (formatoFactura.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                }
            }
            catch
            {
                //   lblError.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
                //Response.Write("<script language:javascript>alert('Error al generar el formato.');</script>");

            }
        }
	}
	//Tipos de procesos postventa
	public enum ProcesosPostventa{Ninguno,Alistamiento,Revision,Garantia};
	//Autorizacion garantias
    public class AutorizacionGarantias : System.Web.UI.UserControl
	{
		private string prefOrden;
		private UInt32 numOrden;
		private DateTime fechaProceso;
		private string nitConcesionario,personaSolicita,vendedor,autorizar,VIN,observacionG,observacionR;
		public string idGarantia;
        public DataTable dtRepuestos;
        public DataTable dtOperaciones;
		public AutorizacionGarantias(string prefO, UInt32 numO, DateTime fechaP, string nitC,string personaS, string vendC, string autorE, string vin, string obsG, string obsR, DataTable dtOperacionesO, DataTable dtRepuestosO)
		{
			prefOrden=prefO;
			numOrden=numO;
			fechaProceso=fechaP;
			nitConcesionario=nitC;
			personaSolicita=personaS;
			vendedor=vendC;
			autorizar=autorE;
			VIN=vin;
			observacionG=obsG;
			observacionR=obsR;
            dtOperaciones = dtOperacionesO;
            dtRepuestos = dtRepuestosO;
		}
		//Autorizar
		public string Autorizar(ref string strFormatos)
		{
			ArrayList arrSql=new ArrayList();
			string estado=(autorizar=="N")?"F":"A";
            double valorTransSinIva = 0;
			idGarantia="";
			arrSql.Add("INSERT INTO MGARANTIAAUTORIZACION VALUES("+
				"DEFAULT,'"+prefOrden+"',"+numOrden+",'"+fechaProceso.ToString("yyyy-MM-dd")+"','"+
				nitConcesionario+"','"+personaSolicita+"','"+vendedor+"','"+autorizar+"','"+VIN+"','"+
				observacionG+"','"+observacionR+"');");
			arrSql.Add("UPDATE MORDENPOSTVENTA SET MGAR_NUMERO=IDENTITY_VAL_LOCAL() "+
				"WHERE PDOC_CODIGO='"+prefOrden+"' AND MORD_NUMEORDE="+numOrden+";");
			if(autorizar=="N")
				arrSql.Add("UPDATE MORDEN SET TEST_ESTADO='F' "+
					"WHERE PDOC_CODIGO='"+prefOrden+"' AND MORD_NUMEORDE="+numOrden+";");
            string estadoA = "";
            double valA=0, valM=0;
            for (int i = 0; i < dtOperaciones.Rows.Count; i++)
            {
                if (Convert.ToInt16(dtOperaciones.Rows[i]["usar"]) == 1)
                    estadoA = "C";
                else
                    estadoA = "X";
                if (autorizar == "N")
                    estadoA = "X";
                arrSql.Add("UPDATE dordenoperacion SET TEST_ESTADO='"+estadoA+"' " +
                "WHERE PDOC_CODIGO='" + dtOperaciones.Rows[i]["PDOC_CODIGO"].ToString() + "' AND MORD_NUMEORDE=" + dtOperaciones.Rows[i]["MORD_NUMEORDE"].ToString() + " AND PTEM_OPERACION='" + dtOperaciones.Rows[i]["PTEM_OPERACION"].ToString() + "';");
            }
            for (int i = 0; i < dtRepuestos.Rows.Count; i++)
            {
                valM = Convert.ToDouble(dtRepuestos.Rows[i]["precio"]) * Convert.ToDouble(dtRepuestos.Rows[i]["cantidad"]);
                valA = Convert.ToDouble(dtRepuestos.Rows[i]["valaprueba"]);
                if (autorizar == "N")
                    valA = 0;
                if (valA == 0)
                    estadoA = "X";
                else
                {
                    if (valA == valM)
                    {
                        estadoA = "C";
                        valorTransSinIva += valM;
                    }
                    else
                        estadoA = "P";
                }
                arrSql.Add("UPDATE dordenitemspostventa SET TEST_ESTADO='" + estadoA + "', MITE_VALAPROB = " + valA + " "+
                "WHERE PDOC_CODIGO='" + dtRepuestos.Rows[i]["PDOC_CODIGO"].ToString() + "' AND MORD_NUMEORDE=" + dtRepuestos.Rows[i]["MORD_NUMEORDE"].ToString() + " AND MITE_CODIGO='" + dtRepuestos.Rows[i]["MITE_CODIGO"].ToString() + "';");
            }
            if (autorizar != "N" && valorTransSinIva>0)
            {
                DataSet dsPostventa = new DataSet();
                DBFunctions.Request(dsPostventa, IncludeSchema.NO,"SELECT * FROM CPOSTVENTA;");
                DataRow drPostventa = dsPostventa.Tables[0].Rows[0];
                DateTime fechaEntrada = Convert.ToDateTime(DBFunctions.SingleData("SELECT mord_entrada from morden where PDOC_CODIGO='" + prefOrden + "' and MORD_NUMEORDE=" + numOrden + ";"));
                uint numeroPedido = Convert.ToUInt32(DBFunctions.SingleData("SELECT pped_ultipedi + 1 FROM ppedido WHERE pped_codigo='" + drPostventa["PPED_CODIGO"].ToString() + "'"));
                while (DBFunctions.RecordExist("SELECT * FROM mpedidoitem WHERE pped_codigo='" + drPostventa["PPED_CODIGO"].ToString() + "' AND mped_numepedi=" + numeroPedido + ""))
                    numeroPedido += 1;
                uint numeroDocumentoTransferencia = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + drPostventa["PDOC_CODITRANS"].ToString() + "'"));
                while (DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='" + drPostventa["PDOC_CODITRANS"].ToString() + "' AND mfac_numedocu=" + numeroDocumentoTransferencia + ""))
                    numeroDocumentoTransferencia += 1;
                PedidoFactura miPedido = new PedidoFactura("T", drPostventa["PPED_CODIGO"].ToString(), drPostventa["MNIT_NITTRANS"].ToString(), drPostventa["PALM_ALMACENTRANS"].ToString(), numeroPedido, prefOrden, numOrden, "C", fechaEntrada, observacionR, 0, new String[0], vendedor, drPostventa["PDOC_CODITRANS"].ToString(), numeroDocumentoTransferencia, "G", 0, 0, valorTransSinIva, valorTransSinIva);
                for (int i = 0; i < dtRepuestos.Rows.Count; i++)
                {
                    valM = Convert.ToDouble(dtRepuestos.Rows[i]["precio"]) * Convert.ToDouble(dtRepuestos.Rows[i]["cantidad"]);
                    valA = Convert.ToDouble(dtRepuestos.Rows[i]["valaprueba"]);
                    if (valA>0 && valA == valM)
                        miPedido.InsertaFila(dtRepuestos.Rows[i]["mite_codigo"].ToString(), Convert.ToDouble(dtRepuestos.Rows[i]["cantidad"]), Convert.ToDouble(dtRepuestos.Rows[i]["precio"]), 0, 0, Convert.ToDouble(dtRepuestos.Rows[i]["cantidad"]),"","");
                }
                miPedido.RealizarPedFac(false);
                for (int j = 0; j < miPedido.SqlStrings.Count; j++)
                    arrSql.Add(miPedido.SqlStrings[j].ToString());
                strFormatos += "&tipoPED="+drPostventa["PPED_CODIGO"].ToString()+"&numPED="+numeroPedido;
                strFormatos += "&prefTRA=" + drPostventa["PDOC_CODITRANS"].ToString() + "&numTRA=" + numeroDocumentoTransferencia;
            }
			if(DBFunctions.Transaction(arrSql))
			{
				idGarantia=DBFunctions.SingleData("SELECT MAX(MGAR_NUMERO) FROM MGARANTIAAUTORIZACION;");
                return("");
			}
			else
				return(DBFunctions.exceptions);
		}
	}
	//Liquidacion postventa
    public class LiquidacionPostVenta : System.Web.UI.UserControl
	{
		private string prefijoAprobacion,nitConcesionario,vendedor;
		private UInt32 numeroAprobacion;
		private int ano,mes;
		private DateTime fechaProc;
		private double subtotalOriginal,ivaOriginal,totalOriginal;
		private double subtotal,iva,total;
		private double categoria,piva;
		private DataTable dtOrdenes;
		public string error,sqlError;
		public bool cambianDatos;
        protected System.Web.UI.WebControls.Label lblError;

		public LiquidacionPostVenta(string prefAprobacion,UInt32 numAprob,string nitConc,string vend,int a, int m, DateTime dtProc, double subtO, double iO, double totO,DataTable dtO,double categ, double pi)
		{
			prefijoAprobacion=prefAprobacion;
			numeroAprobacion=numAprob;
			nitConcesionario=nitConc;
			vendedor=vend;
			ano=a;
			mes=m;
			fechaProc=dtProc;
			subtotalOriginal=subtO;
			ivaOriginal=iO;
			totalOriginal=totO;
			dtOrdenes=dtO;
			subtotal=iva=total=0;
			categoria=categ;
			piva=pi;
		}

		public bool Liquidar()
		{
			ArrayList sqlUpd=new ArrayList();
			string proceso,estado,autoriza;
			bool facturar=false,info=false;
			double valorDetalle;
			cambianDatos=false;
			sqlError=error="";
            subtotal=iva=total=0;
			//Si hay garantias sin aprobar no continúa
			/*if(DBFunctions.RecordExist(
				"SELECT * "+
				"from (SELECT mo.PDOC_CODIGO, mo.MORD_NUMEORDE, mp.mgar_numero "+
				"from morden mo, mordenpostventa mp "+
				"where "+
				"mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND "+
				"mp.tproc_codigo='G' AND "+
				"mo.test_estado='A' and "+
				"month(mo.mord_entrada)="+mes+" and "+
				"year(mo.mord_entrada)="+ano+" and "+
				"mp.mnit_nitconc='"+nitConcesionario+"')as tb1 "+
				"LEFT JOIN MGARANTIAAUTORIZACION mg ON "+
				"mg.PDOC_CODIGO=tb1.PDOC_CODIGO and mg.MORD_NUMEORDEN=tb1.MORD_NUMEORDE "+
				"where tb1.mgar_numero is null;"))
			{
				error="Debe autorizar o rechazar las garantías pendientes (marcadas con amarillo).";
				return(false);
			}*/
			sqlUpd.Add("insert into mliquidacionpostventa values("+
				"default,'"+prefijoAprobacion+"',"+numeroAprobacion+",'"+nitConcesionario+"',"+
				"'"+vendedor+"',"+ano+","+mes+",'"+fechaProc.ToString("yyyy-MM-dd")+"',"+
				subtotalOriginal+","+ivaOriginal+","+totalOriginal+");");
			for(int n=0;n<dtOrdenes.Rows.Count;n++){
				proceso=dtOrdenes.Rows[n]["tproc_codigo"].ToString();
				estado=dtOrdenes.Rows[n]["test_estado"].ToString();
				autoriza=dtOrdenes.Rows[n]["tres_sino"].ToString();
				facturar=false;
				if(proceso=="G"){
					if(autoriza.Length==0){
						cambianDatos=true;
						error="Debe autorizar o rechazar todas las garantías.";
						return(false);
					}
					facturar=(autoriza=="S" && estado=="A");
				}
				else 
					facturar=(estado=="A");
				if(facturar){
					sqlUpd.Add(
						"update morden set test_estado='F' "+
						"where pdoc_codigo='"+dtOrdenes.Rows[n]["pdoc_codigo"]+"' and "+
						"mord_numeorde="+dtOrdenes.Rows[n]["mord_numeorde"]+";");
					valorDetalle=Convert.ToDouble(
						DBFunctions.SingleData(
						"select coalesce(sum(tbA.Precio),0) "+
						"from("+
						" select "+
						" CASE WHEN pt.PTEM_INDIGENERIC='S' THEN pt.PTEM_TIEMPOESTANDAR "+
						" ELSE (SELECT PTIE_TIEMGARA FROM PTIEMPOTALLER PTG "+
						" WHERE PTG.PTIE_GRUPCATA=pc.PGRU_GRUPO AND PTG.PTIE_TEMPARIO=PT.PTEM_OPERACION) "+
						" END * "+categoria+" Precio "+
						"from morden mo, mordenpostventa mp, dordenoperacion dr, ptempario pt, pcatalogovehiculo pc "+
						"where mo.pcat_codigo=pc.pcat_codigo and "+
						"mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND "+
						"mp.tproc_codigo='"+proceso+"' AND "+
						"dr.PDOC_CODIGO=mp.PDOC_CODIGO and dr.MORD_NUMEORDE=mp.MORD_NUMEORDE AND "+
						"mo.PDOC_CODIGO='"+dtOrdenes.Rows[n]["pdoc_codigo"]+"' AND "+
						"mo.MORD_NUMEORDE="+dtOrdenes.Rows[n]["mord_numeorde"]+" AND "+
						"dr.ptem_operacion=pt.ptem_operacion AND mo.test_estado='A' and "+
						"month(mo.mord_entrada)="+mes+" and "+
						"year(mo.mord_entrada)="+ano+" and "+
                        ((proceso=="G")?"dr.test_estado='C' and ":"")+
						"mp.mnit_nitconc='"+nitConcesionario+"')as tbA;")
					);
                    if (proceso == "G"){
                        valorDetalle+=Convert.ToDouble(
                        DBFunctions.SingleData(
                        "select coalesce(sum(mite_valaprob),0) " +
                        "from morden mo, mordenpostventa mp, dordenitemspostventa dr, pcatalogovehiculo pc, MGARANTIAAUTORIZACION mg " +
                        "where mo.pcat_codigo=pc.pcat_codigo and " +
                        "mo.PDOC_CODIGO=mp.PDOC_CODIGO and mo.MORD_NUMEORDE=mp.MORD_NUMEORDE AND mp.tproc_codigo='G' AND " +
                        "dr.PDOC_CODIGO=mp.PDOC_CODIGO and dr.MORD_NUMEORDE=mp.MORD_NUMEORDE AND " +
                        "mg.PDOC_CODIGO=mo.PDOC_CODIGO and mg.MORD_NUMEORDEN=mo.MORD_NUMEORDE AND mg.tres_sino='S' AND " +
                        "mp.tproc_codigo='" + proceso + "' AND " +
                        "mo.PDOC_CODIGO='" + dtOrdenes.Rows[n]["pdoc_codigo"] + "' AND " +
                        "mo.MORD_NUMEORDE=" + dtOrdenes.Rows[n]["mord_numeorde"] + " AND " +
                        "mo.test_estado='A' and  dr.test_estado='P' and " +
                        "month(mo.mord_entrada)=" + mes + " and " +
                        "year(mo.mord_entrada)=" + ano + " and " +
                        "mp.mnit_nitconc='" + nitConcesionario + "';"));
                    }
					subtotal+=valorDetalle;
					sqlUpd.Add("insert into DLIQUIDACIONPOSTVENTA values("+
						"IDENTITY_VAL_LOCAL(),'"+dtOrdenes.Rows[n]["pdoc_codigo"]+"',"+
						dtOrdenes.Rows[n]["mord_numeorde"]+","+valorDetalle+");");
					info=true;
				}
			}
			iva=Math.Round((subtotal*piva)/100);
			total=subtotal+iva;
			if(!info){
				error="No hay órdenes para liquidar.";
				return(false);
			}
			if(subtotal!=subtotalOriginal || iva!=ivaOriginal || total!=totalOriginal)
			{
				cambianDatos=true;
				error="Las órdenes han cambiado desde que las consultó, por favor revíselas nuevamente.";
				return(false);
			}
			if(!DBFunctions.Transaction(sqlUpd))
			{
				error="No se pudo realizar la liquidación. Revise los detalles al final de la página.";
				sqlError=DBFunctions.exceptions;
				return(false);
			}
            else
            {
                ImprimirFormato(dtOrdenes.Rows[0]["pdoc_codigo"], dtOrdenes.Rows[0]["mord_numeorde"]);
                return (true);
            }
		}

        protected void ImprimirFormato(Object prefijo, Object numero)
        {
            FormatosDocumentos formatoFactura = new FormatosDocumentos();
            try
            {
                formatoFactura.Prefijo = Convert.ToString (prefijo);
                formatoFactura.Numero  = Convert.ToInt32(numero);
                formatoFactura.Codigo  = DBFunctions.SingleData("SELECT sfor_codigo FROM dbxschema.pdocumento WHERE pdoc_codigo='" + formatoFactura.Prefijo + "'");
                if (formatoFactura.Codigo != string.Empty)
                {
                    if (formatoFactura.Cargar_Formato())
                        Response.Write("<script language:javascript>w=window.open('" + formatoFactura.Documento + "','','HEIGHT=600,WIDTH=800');</script>");
                }
            }
            catch
            {
              //   lblError.Text = "Error al generar el formato. Detalles : <br>" + formatoFactura.Mensajes;
              //  Response.Write("<script language:javascript>alert('Error al generar el formato.');</script>");
	
            }
        }
    }

}