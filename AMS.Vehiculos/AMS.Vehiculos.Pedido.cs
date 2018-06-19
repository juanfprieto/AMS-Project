using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Documentos;
using AMS.Utilidades;
using System.Configuration;
using System.Web;
using AMS.Contabilidad;

namespace AMS.Vehiculos
{
	public class PedidoCliente
	{
		#region Atributos
		protected string codigoPrefijo, numeroPedido,codigoCotizacion, numeroCotizacion, catalogo, colorPrimario, colorOpcional, fechaCreacion, estadoPedido, claseVehiculo, inventario;
		protected string anoModelo, fechaPedido, fechaProgEntrega, vendedor, nit, nitSolicita, nitOpcional, valorUnitario, valorDescuento, obsequios, opcionVehiculo, almacen;
		protected string valorEfectivo, valorCheques, nombreFinanciera, valorFinaciera, valorOtrosPagos, detalleOtrosPagos;
		protected string valorObsequios, tipoVenta, claseVenta, observaciones, prefijoNotaDevCli,credito;
        protected string prefijoNota, tipoServicio, prefijoNotaAcc = "", prefijoNotaDebito = "", fechaDevolucion;
        protected uint   numeroNota, numeroNotaAcc=0, numeroNotaDebito=0;
        protected string processMsg="", usuarioActual="";
		protected DataTable elementosVenta, vehiculosRetoma, tablaAnticipos;
		protected string user = HttpContext.Current.User.Identity.Name;
        protected NotaDevolucionCliente notaDevolucionClientes;
        ProceHecEco contaOnline = new ProceHecEco();
	    
		#endregion
		
		#region Propiedades
		public string CodigoPrefijo{set{codigoPrefijo = value;}get{return codigoPrefijo;}}
		public string NumeroPedido{set{numeroPedido = value;}get{return numeroPedido;}}
        public string CodigoCotizacion { set { codigoCotizacion = value; } get { return codigoCotizacion; } }
        public string NumeroCotizacion { set { numeroCotizacion = value; } get { return numeroCotizacion; } }
        public string PrefijoNota { set { prefijoNota = value; } get { return prefijoNota; } }
        public uint NumeroNota { set { numeroNota = value; } get { return numeroNota; } }
		public string Catalogo{set{catalogo = value;}get{return catalogo;}}
		public string ColorPrimario{set{colorPrimario = value;}get{return colorPrimario;}}
		public string ColorOpcional{set{colorOpcional = value;}get{return colorOpcional;}}
		public string FechaCreacion{set{fechaCreacion = value;}get{return fechaCreacion;}}
		public string EstadoPedido{set{estadoPedido = value;}get{return estadoPedido;}}
		public string ClaseVehiculo{set{claseVehiculo = value;}get{return claseVehiculo;}}
		public string AnoModelo{set{anoModelo = value;}get{return anoModelo;}}
		public string FechaPedido{set{fechaPedido = value;}get{return fechaPedido;}}
		public string FechaProgEntrega{set{fechaProgEntrega = value;}get{return fechaProgEntrega;}}
		public string Vendedor{set{vendedor = value;}get{return vendedor;}}
		public string Nit{set{nit = value;}get{return nit;}}
        public string NitSolicita { set { nitSolicita = value; } get { return nitSolicita; } }
		public string NitOpcional{set{nitOpcional = value;}get{return nitOpcional;}}
		public string ValorUnitario{set{valorUnitario = value;}get{return valorUnitario;}}
		public string ValorDescuento{set{valorDescuento = value;}get{return valorDescuento;}}
		public string Obsequios{set{obsequios = value;}get{return obsequios;}}
        public string OpcionVehiculo { set { opcionVehiculo = value; }get { return opcionVehiculo; } }
        public string ValorEfectivo{set{valorEfectivo = value;}get{return valorEfectivo;}}
		public string ValorCheques{set{valorCheques = value;}get{return valorCheques;}}
		public string NombreFinanciera{set{nombreFinanciera = value;}get{return nombreFinanciera;}}
		public string ValorFinaciera{set{valorFinaciera = value;}get{return valorFinaciera;}}
		public string ValorOtrosPagos{set{valorOtrosPagos = value;}get{return valorOtrosPagos;}}
		public string DetalleOtrosPagos{set{detalleOtrosPagos = value;}get{return detalleOtrosPagos;}}
		public string ValorObsequios{set{valorObsequios = value;}get{return valorObsequios;}}
		public string TipoVenta{set{tipoVenta = value;}get{return tipoVenta;}}
		public string ClaseVenta{set{claseVenta = value;}get{return claseVenta;}}
		public string Observaciones{set{observaciones = value;}get{return observaciones;}}
		public string Credito{set{credito=value;}get{return credito;}} 
		public string PrefijoNotaDevCli{set{prefijoNotaDevCli = value;}get{return prefijoNotaDevCli;}}
        public string Inventario { set { inventario = value; } get { return inventario; } }
        public DataTable ElementosVenta{set{elementosVenta = value;}get{return elementosVenta;}}
		public DataTable VehiculosRetoma{set{vehiculosRetoma = value;}get{return vehiculosRetoma;}}
		public DataTable TablaAnticipos{set{tablaAnticipos = value;}get{return tablaAnticipos;}}
		public string ProcessMsg{get{return processMsg;}}
        public string TipoServicio { set { tipoServicio = value; } get { return tipoServicio; } }
        public string PrefNotaAccesorios { set { prefijoNotaAcc = value; } get { return prefijoNotaAcc; } }
        public uint NumeNotaAccesorios { set { numeroNotaAcc = value; } get { return numeroNotaAcc; } }
        public string UsuarioActual { set { usuarioActual = value; } get { return usuarioActual; } }
        public string PrefNotaDebito { set { prefijoNotaDebito = value; } get { return prefijoNotaDebito; } }
        public uint NumeNotaDebito { set { numeroNotaDebito = value; } get { return numeroNotaDebito; } }
        

        #endregion
		
		#region Constructores
		//Constructor Por Defecto
		public PedidoCliente()
		{
			elementosVenta = null;
			vehiculosRetoma = null;
		}
		
		//Constructor para proceso de creacion
		public PedidoCliente(DataTable eleVent, DataTable vehRet)
		{
			elementosVenta = new DataTable();
			elementosVenta = eleVent;
			vehiculosRetoma = new DataTable();
			vehiculosRetoma = vehRet;
		}
		
		//Constructor para proceso de modificacion de pedido
		public PedidoCliente(string prefPed, string numPed)
		{
			int i=0;
			//Revisamos si existe el pedido dentro de la tabla mpedidovehiculo
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT * FROM mpedidovehiculo WHERE pdoc_codigo='"+prefPed+"' AND mped_numepedi="+numPed+";"+
				"SELECT * FROM dpedidovehiculo WHERE pdoc_codigo='"+prefPed+"' AND mped_numepedi="+numPed+";"+
				"SELECT * FROM dpedidovehiculoretoma WHERE pdoc_codigo='"+prefPed+"' AND mped_numepedi="+numPed+";"+
				//"SELECT pdoc_codigo, mcaj_numero, mant_valorecicaja FROM manticipovehiculo WHERE mped_codigo='"+prefPed+"' AND mped_numepedi="+numPed+";"+
				"SELECT MAV.pdoc_codigo, MAV.mcaj_numero, MAV.mant_valorecicaja FROM dbxschema.manticipovehiculo MAV, dbxschema.mcaja MCAJ WHERE MAV.mped_codigo='"+prefPed+"' AND MAV.mped_numepedi="+numPed+" AND MAV.pdoc_codfact IS NULL AND MAV.mfac_numedocu IS NULL AND MAV.pdoc_codigo = MCAJ.pdoc_codigo AND MAV.mcaj_numero = MCAJ.mcaj_numero AND test_estadodoc='A';"+
				"SELECT MFAC.pdoc_codigo, MFAC.mfac_numedocu, MFAC.mfac_valofact FROM mfacturacliente MFAC, mfacturapedidovehiculo MFPV, pdocumento PDOC WHERE MFAC.pdoc_codigo = PDOC.pdoc_codigo AND PDOC.tdoc_tipodocu='NC' AND MFAC.pdoc_codigo = MFPV.pdoc_codigo AND MFAC.mfac_numedocu = MFPV.mfac_numedocu AND MFPV.mped_codigo='"+prefPed+"' AND MFPV.mped_numepedi="+numPed+";"+
				"SELECT pcat_codigo, dped_numecont, pano_ano, dped_numeplaca, dped_cuenimpu, dped_valoreci FROM dpedidovehiculoretoma WHERE pdoc_codigo='"+prefPed+"' AND mped_numepedi="+numPed+";");
			if(ds.Tables[0].Rows.Count == 1)
			{
				codigoPrefijo   = prefPed;
				numeroPedido    = numPed;
				catalogo        = ds.Tables[0].Rows[0][2].ToString();
				colorPrimario   = ds.Tables[0].Rows[0][3].ToString();
				colorOpcional   = ds.Tables[0].Rows[0][4].ToString();
				fechaCreacion   = Convert.ToDateTime(ds.Tables[0].Rows[0][5]).ToString("yyyy-MM-dd");
				estadoPedido    = ds.Tables[0].Rows[0][6].ToString();
				claseVehiculo   = ds.Tables[0].Rows[0][7].ToString();
				anoModelo       = ds.Tables[0].Rows[0][8].ToString();
				fechaPedido     = Convert.ToDateTime(ds.Tables[0].Rows[0][9]).ToString("yyyy-MM-dd");
				fechaProgEntrega = Convert.ToDateTime(ds.Tables[0].Rows[0][10]).ToString("yyyy-MM-dd");
				vendedor        = ds.Tables[0].Rows[0][11].ToString();
				nit             = ds.Tables[0].Rows[0][12].ToString();
                nitSolicita     = ds.Tables[0].Rows[0][28].ToString();
				nitOpcional     = ds.Tables[0].Rows[0][13].ToString();
				valorUnitario   = ds.Tables[0].Rows[0][14].ToString();
				valorDescuento  = ds.Tables[0].Rows[0][15].ToString();
				obsequios       = ds.Tables[0].Rows[0][16].ToString();
				valorObsequios  = ds.Tables[0].Rows[0][17].ToString();
				valorEfectivo   = ds.Tables[0].Rows[0][18].ToString();
				valorCheques    = ds.Tables[0].Rows[0][19].ToString();
				nombreFinanciera = ds.Tables[0].Rows[0][20].ToString();
				valorFinaciera  = ds.Tables[0].Rows[0][21].ToString();
				valorOtrosPagos = ds.Tables[0].Rows[0][22].ToString();
				detalleOtrosPagos = ds.Tables[0].Rows[0][23].ToString();
				tipoVenta       = ds.Tables[0].Rows[0][24].ToString();
				claseVenta      = ds.Tables[0].Rows[0][25].ToString();
                observaciones   = ds.Tables[0].Rows[0][26].ToString();
                tipoServicio    = ds.Tables[0].Rows[0][29].ToString();

                //Ahora cargamos el datatable de los otros elementos de venta y de la retoma
				PrepararElementosVenta();
				for(i=0;i<ds.Tables[1].Rows.Count;i++)
				{
					DataRow fila = elementosVenta.NewRow();
					fila[0] = DBFunctions.SingleData("SELECT pite_nombre FROM pitemventavehiculo WHERE pite_codigo='"+ds.Tables[1].Rows[i][2].ToString()+"'");
					fila[1] = Convert.ToDouble(ds.Tables[1].Rows[i][3]);
					elementosVenta.Rows.Add(fila);
				}
				PrepararVehiculosRetoma();
				for(i=0;i<ds.Tables[2].Rows.Count;i++)
				{
					DataRow fila = vehiculosRetoma.NewRow();
					fila[0] = ds.Tables[2].Rows[i][2].ToString();
					fila[1] = ds.Tables[2].Rows[i][4].ToString();
					fila[2] = ds.Tables[2].Rows[i][5].ToString();
					fila[3] = ds.Tables[2].Rows[i][3].ToString();
					fila[4] = ds.Tables[2].Rows[i][6].ToString();
					fila[5] = Convert.ToDouble(ds.Tables[2].Rows[i][7]);
					vehiculosRetoma.Rows.Add(fila);
				}
				PrepararTablaAnticipos();
				for(i=0;i<ds.Tables[3].Rows.Count;i++)
				{
					//Revisamos si este anticipo esta relacionado con otra factura o no
					if(!DBFunctions.RecordExist("SELECT * FROM DDETALLEFACTURACLIENTE WHERE pdoc_codigo='"+ds.Tables[3].Rows[i][0].ToString()+"' AND mcaj_numero="+ds.Tables[3].Rows[i][1].ToString()+""))
					{
						DataRow fila = tablaAnticipos.NewRow();
						fila[0] = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[3].Rows[i][0].ToString()+"'");
						fila[1] = ds.Tables[3].Rows[i][1].ToString();
						fila[2] = Convert.ToDateTime(DBFunctions.SingleData("SELECT mcaj_fecha FROM mcaja WHERE pdoc_codigo='"+ds.Tables[3].Rows[i][0].ToString()+"' AND mcaj_numero="+ds.Tables[3].Rows[i][1].ToString()+"")).ToString("yyyy-MM-dd");
						fila[3] = Convert.ToDouble(ds.Tables[3].Rows[i][2]);
						fila[4] = DBFunctions.SingleData("SELECT pdoc_descripcion FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[3].Rows[i][0].ToString()+"'");
						tablaAnticipos.Rows.Add(fila);
					}
				}
				for(i=0;i<ds.Tables[4].Rows.Count;i++)
				{
					DataRow fila = tablaAnticipos.NewRow();
					fila[0] = DBFunctions.SingleData("SELECT pdoc_nombre FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[4].Rows[i][0].ToString()+"'");
					fila[1] = ds.Tables[4].Rows[i][1].ToString();
					fila[2] = Convert.ToDateTime(DBFunctions.SingleData("SELECT mfac_factura FROM mfacturacliente WHERE pdoc_codigo='"+ds.Tables[4].Rows[i][0].ToString()+"' AND mfac_numedocu="+ds.Tables[4].Rows[i][1].ToString()+"")).ToString("yyyy-MM-dd");
					fila[3] = Convert.ToDouble(ds.Tables[4].Rows[i][2]);
					fila[4] = DBFunctions.SingleData("SELECT pdoc_descripcion FROM pdocumento WHERE pdoc_codigo='"+ds.Tables[4].Rows[i][0].ToString()+"'");
					tablaAnticipos.Rows.Add(fila);
				}
				PrepararVehiculosRetoma();
				for(i=0;i<ds.Tables[5].Rows.Count;i++)
				{
					DataRow fila = vehiculosRetoma.NewRow();
					fila[0] = ds.Tables[5].Rows[i][0].ToString();
					fila[1] = ds.Tables[5].Rows[i][1].ToString();
					fila[2] = ds.Tables[5].Rows[i][2].ToString();
					fila[3] = ds.Tables[5].Rows[i][3].ToString();
					fila[4] = ds.Tables[5].Rows[i][4].ToString();
					fila[5] = Convert.ToDouble(ds.Tables[5].Rows[i][5]);
					vehiculosRetoma.Rows.Add(fila);
				}
			}
		}
		
		//Constructor para el proceso de devolucion 
        public PedidoCliente(string prefPed, string numPed, string prefNDC, string fechaNota)
		{
			//Asignamos los parametros de entrada a los atributos
			codigoPrefijo = prefPed;
			numeroPedido = numPed;
			prefijoNotaDevCli = prefNDC;
            fechaDevolucion = fechaNota;
		}

        #endregion

        #region Metodos
        public bool Grabar_Pedido()
        {
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			int i;
          
            while (!status)
            {
                //Revisamos si existe algun pedido con este prefijo con este numero
                if (DBFunctions.RecordExist("SELECT * FROM mpedidovehiculo WHERE pdoc_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + ""))
                    numeroPedido = (Convert.ToInt32(numeroPedido) + 1).ToString();
                else
                    status = true;
	        }
				//Primero debemos ingresar una nueva entrada a la tabla mpedidovehiculo
            obsequios         = obsequios.Replace(",", ".");
            detalleOtrosPagos = detalleOtrosPagos.Replace(",", ".");
            observaciones     = observaciones.Replace(",", ".");
            if (nitSolicita.Length == 0) 
                nitSolicita = nit;

            if (claseVehiculo == "U")
            {
                inventario = DBFunctions.SingleData("SELECT MVEH_INVENTARIO FROM MVEHICULO WHERE MCAT_VIN='" + catalogo + "'  and test_tipoesta <=20");
                catalogo = DBFunctions.SingleData( String.Format("select pcat_codigo from mcatalogovehiculo where mcat_vin='{0}'", catalogo));
                sqlStrings.Add("INSERT INTO mpedidovehiculoUSADO VALUES('" + codigoPrefijo + "'," + numeroPedido + "," + inventario + ")");
            }
            if(!DBFunctions.RecordExist("SELECT POPC_OPCIVEHI FROM MPEDIDOVEHICULO FETCH FIRST ROW ONLY;") || opcionVehiculo == "")
                sqlStrings.Add(@"INSERT INTO mpedidovehiculo
                                (PDOC_CODIGO,MPED_NUMEPEDI,PCAT_CODIGO,PCOL_CODIGO,PCOL_CODIGOALTE,MPED_PEDIDO,TEST_TIPOESTA,TCLA_CLASE,PANO_ANO,MPED_FECHPEDI,
                                MPED_FECHENTREGA,PVEN_CODIGO,MNIT_NIT,MNIT_NIT2,MPED_VALOUNIT,MPED_VALODESC,MPED_OBSEQUIO,MPED_VALOOBSE,MPED_VALOEFEC,MPED_VALOCHEQ,
                                MPED_NOMBFINC,MPED_VALOFINC,MPED_VALOOTRPAGO,MPED_DETAOTRPAGO,PTIPO_CODITIPOVENTA,PCLAS_CODIGOVENTA,MPED_OBSERVACION,TRES_SINO,MNIT_NITSOLICITA,
                                TSER_TIPOSERV,SUSU_USUARIO)        
                                VALUES('" + codigoPrefijo + "'," + numeroPedido + ",'" + catalogo + "','" + colorPrimario + "','" + colorOpcional + "','" + fechaCreacion + "'," + estadoPedido + ",'" + claseVehiculo + "'," + anoModelo + ",'" + fechaPedido + "','" + fechaProgEntrega + "','" + vendedor + "','" + nit + "','" + nitOpcional + "'," + valorUnitario + "," + valorDescuento + ",'" + obsequios + "'," + valorObsequios + "," + valorEfectivo + "," + valorCheques + ",'" + nombreFinanciera + "'," + valorFinaciera + "," + valorOtrosPagos + ",'" + detalleOtrosPagos + "','" + tipoVenta + "','" + claseVenta + "','" + observaciones + "','" + credito + "'," + "'" + nitSolicita + "','" + tipoServicio + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "')");
            else
                sqlStrings.Add(@"INSERT INTO mpedidovehiculo
                                (PDOC_CODIGO,MPED_NUMEPEDI,PCAT_CODIGO,PCOL_CODIGO,PCOL_CODIGOALTE,MPED_PEDIDO,TEST_TIPOESTA,TCLA_CLASE,PANO_ANO,MPED_FECHPEDI,
                                MPED_FECHENTREGA,PVEN_CODIGO,MNIT_NIT,MNIT_NIT2,MPED_VALOUNIT,MPED_VALODESC,MPED_OBSEQUIO,MPED_VALOOBSE,MPED_VALOEFEC,MPED_VALOCHEQ,
                                MPED_NOMBFINC,MPED_VALOFINC,MPED_VALOOTRPAGO,MPED_DETAOTRPAGO,PTIPO_CODITIPOVENTA,PCLAS_CODIGOVENTA,MPED_OBSERVACION,TRES_SINO,MNIT_NITSOLICITA,
                                TSER_TIPOSERV,SUSU_USUARIO,POPC_OPCIVEHI)        
                                VALUES('" + codigoPrefijo + "'," + numeroPedido + ",'" + catalogo + "','" + colorPrimario + "','" + colorOpcional + "','" + fechaCreacion + "'," + estadoPedido + ",'" + claseVehiculo + "'," + anoModelo + ",'" + fechaPedido + "','" + fechaProgEntrega + "','" + vendedor + "','" + nit + "','" + nitOpcional + "'," + valorUnitario + "," + valorDescuento + ",'" + obsequios + "'," + valorObsequios + "," + valorEfectivo + "," + valorCheques + ",'" + nombreFinanciera + "'," + valorFinaciera + "," + valorOtrosPagos + ",'" + detalleOtrosPagos + "','" + tipoVenta + "','" + claseVenta + "','" + observaciones + "','" + credito + "'," + "'" + nitSolicita + "','" + tipoServicio + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "', '" + opcionVehiculo + "')");
            
             if (codigoCotizacion != "" && numeroCotizacion != "")
            {
                sqlStrings.Add("INSERT INTO MCOTIZACIONPEDIDOVEH( PDOC_CODICOTI, DVIS_NUMEVISI, PDOC_CODIPEDI, MPED_NUMEPEDI) VALUES( '" + codigoCotizacion + "'," + numeroCotizacion + ",'" + codigoPrefijo + "'," + numeroPedido + ");");
            }
            //Ahora vamos a ingresar los otros items de venta que se encuentran en la tabla elementosVenta
			for(i=0;i<elementosVenta.Rows.Count;i++)
				sqlStrings.Add("INSERT INTO dpedidovehiculo VALUES('"+codigoPrefijo+"',"+numeroPedido+",'"+DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='"+elementosVenta.Rows[i][1].ToString()+"'")+"',"+elementosVenta.Rows[i][2].ToString()+","+elementosVenta.Rows[i][3].ToString()+")");
			//Ahora vamos a ingresar los datos de los vehiculos de retoma
            for (i = 0; i < vehiculosRetoma.Rows.Count; i++)
            {
                //inventario = DBFunctions.SingleData("SELECT mveh_inventario FROM dbxschema.mvehiculo WHERE test_tipoesta>40 and mcat_vin=(SELECT mcat_vin FROM dbxschema.mcatalogovehiculo where mcat_placa='" + vehiculosRetoma.Rows[i][3].ToString() + "')" );
                //if (inventario.Equals(""))
                //    i = vehiculosRetoma.Rows.Count;
                sqlStrings.Add("INSERT INTO dpedidovehiculoretoma VALUES('" + this.codigoPrefijo + "'," + this.numeroPedido + ",'" + vehiculosRetoma.Rows[i][0].ToString() + "','" + vehiculosRetoma.Rows[i][3].ToString() + "','" + vehiculosRetoma.Rows[i][1].ToString() + "'," + vehiculosRetoma.Rows[i][2].ToString() + ",'" + vehiculosRetoma.Rows[i][4].ToString() + "'," + vehiculosRetoma.Rows[i][5].ToString() + ",null)");
            }         
            //Ahora Actualizamos el numero del consecutivo dentro de la tabla 
            sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu = "+this.numeroPedido+" WHERE pdoc_codigo ='" + this.codigoPrefijo + "'");
            if (DBFunctions.Transaction(sqlStrings))
            {
                status = true;
                processMsg += DBFunctions.exceptions + "<br>";
            }
            else
            {
                status = false;
                processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
            }
			return status;
		}
		
		public bool Modificar_Pedido(bool retoma)
		{
			//Primero debemos realizar la actualizacion a la tabla mpedidovehiculo
			ArrayList sqlStrings = new ArrayList();
			string updatePrincipal = "UPDATE mpedidovehiculo SET ";
			int numeroUpdate = 0;
			int i,j;
			ArrayList nombreCampo = new ArrayList();
			ArrayList valorActual = new ArrayList();
			ArrayList tipoDato = new ArrayList();
			this.Preparar_ArrayList_Update(nombreCampo,valorActual,tipoDato);
			bool status = false;
            int  valorFinanciado= 0;
            string nitFinanciera = "";
            string datosUpdate = "";

            if (nitSolicita.Length == 0) nitSolicita = nit;
			for(i=0;i<nombreCampo.Count;i++)
			{
                if (nombreCampo[i].ToString().ToUpper() == "MPED_NOMBFINC")
                    nitFinanciera = valorActual[i].ToString();
                else 
                    if (nombreCampo[i].ToString().ToUpper() == "MPED_VALOFINC")
                    {
                        if (valorActual[i].ToString() == "")
                            valorFinanciado = 0;
                        else
                            valorFinanciado = Convert.ToInt32(valorActual[i].ToString());
                    }

                

        		if(DBFunctions.SingleData("SELECT "+nombreCampo[i].ToString()+" FROM mpedidovehiculo WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+"")!=valorActual[i].ToString())
				{
					//this.processMsg +="<br>"+valorActual[i].ToString();
					if(numeroUpdate==0)
						numeroUpdate+=1;
					else
						updatePrincipal += " , ";
					if(tipoDato[i].ToString()=="C")
						updatePrincipal += nombreCampo[i].ToString()+"='"+valorActual[i].ToString()+"' ";
					else
						updatePrincipal += nombreCampo[i].ToString()+"="+valorActual[i].ToString()+" ";
                   
                    datosUpdate += valorActual[i].ToString() + ",";
                }
			}

            datosUpdate = datosUpdate.Substring(0, datosUpdate.Length - 1);
			updatePrincipal += " WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+"";
			if(numeroUpdate>0)
				sqlStrings.Add(updatePrincipal);
            if (claseVehiculo == "U")
            {
                inventario = DBFunctions.SingleData("SELECT MVEH_INVENTARIO FROM MVEHICULO WHERE MCAT_VIN='" + catalogo + "'  and test_tipoesta <=30");
                //catalogo = DBFunctions.SingleData(String.Format("select pcat_codigo from mcatalogovehiculo where mcat_vin='{0}'", catalogo));

                sqlStrings.Add("UPDATE mpedidovehiculoUSADO SET MVEH_INVENTARIO = " + inventario + " WHERE PDOC_CODIGO = '" + codigoPrefijo + "' AND MPED_NUMEPEDI = " + numeroPedido );
            }

            // Registra cambios en historial
            sqlStrings.Add("INSERT INTO MHISTORIAL_CAMBIOS VALUES(DEFAULT,'DPEDIDOVEHICULO','U','" + datosUpdate + "','" + HttpContext.Current.User.Identity.Name.ToLower() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "');");

            // Se actualiza la tabla de Creditos de Financieras
            if (nitFinanciera != "")
                sqlStrings.Add("UPDATE MCREDITOFINANCIERA SET MCRED_VALOSOLI =" + valorFinanciado.ToString() + ", MCRED_VALOAPROB =" + valorFinanciado.ToString() + ", MNIT_FINANCIERA =" + nitFinanciera + " WHERE pdoc_codipedi='" + this.codigoPrefijo + "' AND mped_numepedi=" + this.numeroPedido + " AND MCRED_VALOSOLI <> " + valorFinanciado.ToString() + " and testa_codigo in (1,2,3)");
            else
                sqlStrings.Add("UPDATE MCREDITOFINANCIERA SET testa_codigo = 0 WHERE pdoc_codipedi='" + this.codigoPrefijo + "' AND mped_numepedi=" + this.numeroPedido + " and testa_codigo in (1,2,3)");
       
            //Ahora vamos a reallizar la actualizacion de los otros elementos de venta 
			//Primero debemos traer los elementos de venta que se relacionan con este pedido
			DataSet elementosPedido = new DataSet();
			DBFunctions.Request(elementosPedido,IncludeSchema.NO,"SELECT pite_codigo, dped_valoitem FROM dpedidovehiculo WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+"");
			for(i=0;i<elementosVenta.Rows.Count;i++)
			{
				bool encontrado = false;
				int posicion = 0;
				for(j=0;j<elementosPedido.Tables[0].Rows.Count;j++)
				{
					if((DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='"+elementosVenta.Rows[i][1].ToString()+"'"))==(elementosPedido.Tables[0].Rows[j][0].ToString()))
					{
						encontrado = true;
						posicion = j;
					}
				}
				if(encontrado)
				{
					if((DBFunctions.SingleData("SELECT dped_valoitem FROM dpedidovehiculo WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND pite_codigo='"+DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='"+elementosVenta.Rows[i][1].ToString()+"'")+"'"))!=(elementosVenta.Rows[i][2].ToString()))
					{
						sqlStrings.Add("UPDATE dpedidovehiculo SET dped_valoitem="+elementosVenta.Rows[i][2].ToString()+" WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND pite_codigo='"+DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='"+elementosVenta.Rows[i][1].ToString()+"'")+"'");
					}
					elementosPedido.Tables[0].Rows.RemoveAt(posicion);
				}
				else
					sqlStrings.Add("INSERT INTO dpedidovehiculo VALUES('"+this.codigoPrefijo+"',"+this.numeroPedido+",'"+elementosVenta.Rows[i][0].ToString()+"',"+elementosVenta.Rows[i][2].ToString()+","+elementosVenta.Rows[i][3].ToString()+")");
			}
			for(i=0;i<elementosPedido.Tables[0].Rows.Count;i++)
				sqlStrings.Add("DELETE FROM dpedidovehiculo WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND pite_codigo='"+elementosPedido.Tables[0].Rows[i][0].ToString()+"' ");
			//sqlStrings.Add("DELETE FROM dpedidovehiculo WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND pite_codigo='"+DBFunctions.SingleData("SELECT pite_codigo FROM pitemventavehiculo WHERE pite_nombre='"+elementosPedido.Tables[0].Rows[i][0].ToString()+"'")+"'");
			
			//Ahora vamos a realizar la actualizacion de los vehiculos que se van a recibir como parte de pago
			//Primero traemos los vehiculos de retoma que ya se han guardado
			DataSet vehiculosRetomaPedido = new DataSet();
			DBFunctions.Request(vehiculosRetomaPedido,IncludeSchema.NO,"SELECT pcat_codigo, dped_numecont, pano_ano, dped_numeplaca, dped_cuenimpu, dped_valoreci FROM dpedidovehiculoretoma WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+"");
			if(retoma)
			{
				///////////////////////////////////////////////////////////////////
				for(i=0;i<vehiculosRetoma.Rows.Count;i++)
				{
					bool encontrado = false;
					int posicion = 0;
					for(j=0;j<vehiculosRetomaPedido.Tables[0].Rows.Count;j++)
					{
						if((vehiculosRetoma.Rows[i][3].ToString())==(vehiculosRetomaPedido.Tables[0].Rows[j][3].ToString()))
						{
							encontrado = true;
							posicion = j;
						}					
					}
					if(encontrado)
					{
						sqlStrings.Add("UPDATE dpedidovehiculoretoma SET pcat_codigo='"+vehiculosRetoma.Rows[i][0].ToString()+"', dped_numecont='"+vehiculosRetoma.Rows[i][1].ToString()+"', pano_ano="+vehiculosRetoma.Rows[i][2].ToString()+", dped_cuenimpu='"+vehiculosRetoma.Rows[i][4].ToString()+"', dped_valoreci="+vehiculosRetoma.Rows[i][5].ToString()+" WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND dped_numeplaca='"+vehiculosRetoma.Rows[i][3].ToString()+"'");
						vehiculosRetomaPedido.Tables[0].Rows.RemoveAt(posicion);
					}
					else
						sqlStrings.Add("INSERT INTO dpedidovehiculoretoma VALUES('"+this.codigoPrefijo+"',"+this.numeroPedido+",'"+vehiculosRetoma.Rows[i][0].ToString()+"','"+vehiculosRetoma.Rows[i][3].ToString()+"','"+vehiculosRetoma.Rows[i][1].ToString()+"',"+vehiculosRetoma.Rows[i][2].ToString()+",'"+vehiculosRetoma.Rows[i][4].ToString()+"',"+vehiculosRetoma.Rows[i][5].ToString()+",null)");
					//sqlStrings.Add("INSERT INTO dpedidovehiculoretoma VALUES('"+this.codigoPrefijo+"',"+this.numeroPedido+",'"+vehiculosRetoma.Rows[i][0].ToString()+"','"+vehiculosRetoma.Rows[i][1].ToString()+"','"+vehiculosRetoma.Rows[i][3].ToString()+"',"+vehiculosRetoma.Rows[i][2].ToString()+",'"+vehiculosRetoma.Rows[i][4].ToString()+"',"+vehiculosRetoma.Rows[i][5].ToString()+",null)");
				}
				for(i=0;i<vehiculosRetomaPedido.Tables[0].Rows.Count;i++)
					sqlStrings.Add("DELETE FROM dpedidovehiculoretoma WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND dped_numeplaca='"+vehiculosRetomaPedido.Tables[0].Rows[i][3].ToString()+"'");
				//////////////////////////////////////////////////////////////////
			}
			else
			{
				for(i=0;i<vehiculosRetomaPedido.Tables[0].Rows.Count;i++)
					sqlStrings.Add("DELETE FROM dpedidovehiculoretoma WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mped_numepedi="+this.numeroPedido+" AND dped_numeplaca='"+vehiculosRetomaPedido.Tables[0].Rows[i][3].ToString()+"'");
			}
			//Ahora vamos a ejecutar la transaccion 
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
               
				processMsg += DBFunctions.exceptions + "<br>";
                
            }
			else
				processMsg+="<BR> Error: Error en Actualizacion <br>"+DBFunctions.exceptions;
			return status;			
		}

		public bool RealizarDevolucion(string proceso)
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;

            // Validamos que el estado del vehículo este en FACTURADO.
            if ((Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.mvehiculo where test_tipoesta=40 and mveh_inventario = (Select mveh_inventario from dbxschema.masignacionvehiculo where pdoc_codigo='" + CodigoPrefijo + "' and mped_numepedi=" + NumeroPedido + ")")) > 0) &&
                // Validamos que el estado del pedido este en FACTURADO.
                (Convert.ToInt32(DBFunctions.SingleData("Select count(*) from dbxschema.mpedidovehiculo where test_tipoesta=30 and pdoc_codigo='" + CodigoPrefijo + "' and mped_numepedi=" + NumeroPedido)) > 0))
            {
                //Traemos la factura relacionada con este pedido
                string prefijoFactura = DBFunctions.SingleData("SELECT pdoc_codigo FROM mfacturapedidovehiculo WHERE mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + " and mfac_tipclie = 'C' ");
                uint numeroFactura = Convert.ToUInt32(DBFunctions.SingleData("SELECT mfac_numedocu FROM mfacturapedidovehiculo WHERE mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + " and mfac_tipclie = 'C'"));
                string prefAccesorios = "";
                uint numeroAccesorios = 0;
             
                //Trae facturas de Accesorios en caso de que existan.
                DataSet dsFacturaAccesorios = new DataSet();
                DBFunctions.Request(dsFacturaAccesorios, IncludeSchema.NO,
                    "SELECT mf.pdoc_codigo, mf.mfac_numedocu FROM mfacturapedidovehiculo mf, pdocumentohecho pd  WHERE " +
                    "mf.mped_codigo='" + codigoPrefijo + "' AND mf.mped_numepedi=" + numeroPedido + " AND mf.pdoc_codigo = pd.pdoc_codigo AND pd.tpro_proceso='VA';");

                if (dsFacturaAccesorios.Tables[0].Rows.Count > 0)
                {
                    prefAccesorios = dsFacturaAccesorios.Tables[0].Rows[0][0].ToString();
                    numeroAccesorios = Convert.ToUInt32(dsFacturaAccesorios.Tables[0].Rows[0][1].ToString());
                }

                uint contadorNotas = 0;
                double valorAbonosAnticipos = 0;

                //Traemos el vehiculo que se encuentra asignado a ese pedido
                string vehiculo = DBFunctions.SingleData("SELECT mveh_inventario FROM dbxschema.masignacionvehiculo WHERE pdoc_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + "");

                //Ahora traemos el detalle de los pagos de esta factura 
                DataSet dsDetallePgsFact = new DataSet();
                DBFunctions.Request(dsDetallePgsFact, IncludeSchema.NO, "SELECT pdoc_coddocref, ddet_numedocu, ddet_valodocu FROM ddetallefacturacliente  DF, MANTICIPOVEHICULO MA WHERE DF.pdoc_codigo='" + prefijoFactura + "' AND DF.mfac_numedocu=" + numeroFactura + " AND  MA.PDOC_CODIGO = DF.PDOC_CODDOCREF AND MA.MCAJ_NUMERO = DF.DDET_NUMEDOCU");

                if (dsDetallePgsFact.Tables[0].Rows.Count > 0)
                {
                    // guarda anticipos aplicados a factura    
                    if (proceso != "E")
                        sqlStrings.Add("INSERT INTO manticipovehiculoanulacion SELECT MAV.* FROM manticipovehiculo MAV, MCAJA MC WHERE mped_codigo='" + codigoPrefijo + "' AND mped_numepedi   =" + numeroPedido + " AND MAV.PDOC_CODIGO = MC.PDOC_CODIGO AND MAV.MCAJ_NUMERO = MC.MCAJ_NUMERO AND MC.TEST_ESTADODOC = 'A' ");
                    valorAbonosAnticipos = Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(sum(mant_valorecicaja),0) FROM manticipovehiculo WHERE mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + " "));
                    // Elimina la referencia de la factura del abono de la tabla de anticipos.
                    sqlStrings.Add("UPDATE manticipovehiculo SET TEST_ESTADO = 20, pdoc_codfact = NULL, mfac_numedocu = NULL WHERE mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + " ");
                    // Elimina el registro de la tabla de cruce entre el pedido y la factura.
                    sqlStrings.Add("DELETE FROM ddetallefacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " AND pdoc_coddocref || cast(ddet_numedocu as char(10)) in (select pdoc_codigo || cast(mcaj_numero as char (10)) from MANTICIPOVEHICULO WHERE MPED_CODIGO = '" + codigoPrefijo + "' AND MPED_NUMEPEDI = " + numeroPedido + ") ");

                    // Elimina el registro de la tabla de cruce entre el pedido y la factura de ACCESORIOS
                    sqlStrings.Add("DELETE FROM ddetallefacturacliente WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios);

                    // Actualiza el valor abonado, restando el valor de los anticipos del vehiculo al estado asignado.
                    //sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia = 'A', mfac_valoabon = mfac_valoabon - " + valorAbonosAnticipos.ToString() + " WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " ");
                    //DBFunctions.Transaction(sqlStrings);  // Realiza la transaccion de eliminar los anticipos a la factura actual y liberarlos para la proxima factura
                    //sqlStrings.Clear();                   // Limpia las transacciones para continuar con el proceso
                }

                double valorFactura = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " "));
                double valorIva = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva  FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " "));
                double mfac_valorete = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valorete FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " "));
                double valorFacAcce = 0;
                double valorIvaAcce = 0;

                //if (prefAccesorios != "")
                //{
                //    valorFacAcce = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact FROM mfacturacliente WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios + " "));
                //    valorIvaAcce = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoiva  FROM mfacturacliente WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios + " "));
                //}


                bool pagoVehiculo = false;
                double auxAnticipo = valorAbonosAnticipos;
                double valorPorPagar = valorFactura + valorIva + mfac_valorete;

                // Ajuste de devolucion considerando las facturas administrativas y nota a favor del cliente en un pedido
                if (auxAnticipo - valorPorPagar >= 0 && pagoVehiculo == false)
                {
                    sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia = 'A', mfac_valoabon = mfac_valoabon - " + valorPorPagar.ToString() + " WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " ");
                    auxAnticipo = auxAnticipo - valorPorPagar;
                    pagoVehiculo = true;
                }
                else if (pagoVehiculo == false)
                {
                        sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia = 'A', mfac_valoabon = mfac_valoabon - " + valorAbonosAnticipos.ToString() + " WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " ");
                        auxAnticipo = auxAnticipo - valorPorPagar;
                }

                if (auxAnticipo > 0)
                {
                    double valorAbonosAcce = 0;

                    if (prefAccesorios == "" || prefAccesorios == null)
                    {
                        valorAbonosAcce = 0;
                    }
                    else
                    {
                        try
                        {
                            valorAbonosAcce = Convert.ToDouble(DBFunctions.SingleData("select ddet_valodocu from ddetallefacturacliente where pdoc_codigo='" + prefAccesorios + "' and mfac_numedocu=" + numeroAccesorios + "; "));
                        }
                        catch
                        {
                            valorAbonosAcce = 0;
                        }
                    }
                    if (auxAnticipo - valorAbonosAcce >= 0)
                    {
                        sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia = 'A', mfac_valoabon = mfac_valoabon - " + valorAbonosAcce.ToString() + " WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios + " ");
                        auxAnticipo = auxAnticipo - valorAbonosAcce;
                    }
                    else
                    {
                        sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia = 'A', mfac_valoabon = mfac_valoabon - " + auxAnticipo.ToString() + " WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios + " ");
                        auxAnticipo = auxAnticipo - auxAnticipo;
                    }
                }

                if (dsDetallePgsFact.Tables[0].Rows.Count > 0)
                {
                    DBFunctions.Transaction(sqlStrings);
                    sqlStrings.Clear();
                }


                // Borra el registro que relaciona el pedido y la factura en MFACTURAPEDIDOVEHICULO.
                if (proceso != "E")
                    sqlStrings.Add("INSERT INTO mfacturapedidovehiculoANULACION SELECT * FROM mfacturapedidovehiculo WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " AND mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + "");
                sqlStrings.Add("DELETE FROM mfacturapedidovehiculo WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " AND mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + "");
                // Borra el registro de Accesorios que relaciona el pedido y la factura en MFACTURAPEDIDOVEHICULO.
                if (prefAccesorios != "")
                {
                    if (proceso != "E")
                        sqlStrings.Add("INSERT INTO mfacturapedidovehiculoANULACION SELECT * FROM mfacturapedidovehiculo WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios + " AND mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + "");
                    sqlStrings.Add("DELETE FROM mfacturapedidovehiculo WHERE pdoc_codigo='" + prefAccesorios + "' AND mfac_numedocu=" + numeroAccesorios + " AND mped_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + "");
                }

                // Actualiza el estado del pedido al estado asignado.
                sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta = 20 WHERE pdoc_codigo='" + codigoPrefijo + "' AND mped_numepedi=" + numeroPedido + "");

                // Actualiza el estado del vehiculo al estado asignado.
                sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta = 30 WHERE mveh_inventario=" + vehiculo + "");

                // Crea la nota de esta factura y cambiamos el estado de la factura, y ademas eliminamos el registro de la tabla mfacturapedidovehiculo
                double ipoPorc = 0;
                double valorIpo = 0;

                uint notaAccesorios = 0;
                if (proceso != "E")
                {   
                    notaDevolucionClientes = new NotaDevolucionCliente(prefijoNotaDevCli, prefijoFactura, contadorNotas, numeroFactura, "N", "FC", 0, "A", "C", Convert.ToDateTime(fechaDevolucion.ToString()), user, false);
                    notaDevolucionClientes.ObservacionDevolucion = observaciones;
                    notaDevolucionClientes.GrabarNotaDevolucionCliente(false);
                    notaAccesorios = notaDevolucionClientes.NumeroNota;
                    ManejoArrayList.InsertarArrayListEnArrayList(notaDevolucionClientes.SqlStrings, sqlStrings);
                    notaAccesorios++;
                }

                NotaDevolucionCliente notaDevolucionAccesorios = null;
                if (prefAccesorios != "" && proceso != "E")
                {
                    if(notaAccesorios > 0)
                        notaDevolucionAccesorios = new NotaDevolucionCliente(prefijoNotaDevCli, prefAccesorios, notaAccesorios, numeroAccesorios, "N", "FC", 0, "A", "C", Convert.ToDateTime(fechaDevolucion.ToString()), user, false);
                    else
                        notaDevolucionAccesorios = new NotaDevolucionCliente(prefijoNotaDevCli, prefAccesorios, 1, numeroAccesorios, "N", "FC", 0, "A", "C", Convert.ToDateTime(fechaDevolucion.ToString()), user, false);

                    notaDevolucionAccesorios.GrabarNotaDevolucionCliente (false);
                    ManejoArrayList.InsertarArrayListEnArrayList(notaDevolucionAccesorios.SqlStrings, sqlStrings);
                    prefijoNotaAcc = prefAccesorios;
                    numeroNotaAcc = numeroAccesorios;
                }

                try
                {
                    ipoPorc = Convert.ToDouble(DBFunctions.SingleData("SELECT PIPO_PORCIPOC FROM mfacturaclienteimpuesto WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " "));
                    valorIpo = Convert.ToDouble(DBFunctions.SingleData("SELECT MFAC_VALOIMPO FROM mfacturaclienteimpuesto WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura + " "));
                    sqlStrings.Add("INSERT INTO mfacturaclienteimpuesto VALUES('" + notaDevolucionClientes.PrefijoNota + "'," + notaDevolucionClientes.NumeroNota + "," + ipoPorc + "," + valorIpo + "," + valorFactura + ")");
                }
                catch { }

                if (proceso != "E")
                {
                    prefijoNota = notaDevolucionClientes.PrefijoNota;
                    numeroNota = notaDevolucionClientes.NumeroNota;
                }
                else
                {
                    prefijoNota = "";
                    numeroNota = 0;
                    // se anula la factura y se ponen todos los valores en cero
                    sqlStrings.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOFACT = 0, MFAC_VALOIVA = 0, MFAC_VALOFLET = 0, MFAC_VALOIVAFLET = 0, MFAC_VALOABON = 0, TVIG_VIGENCIA='C', mfac_observacion = mfac_observacion WHERE PDOC_CODIGO = '" + prefijoFactura + "' AND MFAC_NUMEDOCU = " + numeroFactura);
                }


                //Devolucion de nota a favor del cliente.
                string almacen = DBFunctions.SingleData("SELECT palm_almacen FROM mfacturacliente WHERE pdoc_codigo='" + prefijoFactura + "' AND mfac_numedocu=" + numeroFactura);
                DataSet dsNota = new DataSet();
                DataSet dsNotaValores = new DataSet();
                DBFunctions.Request(dsNota, IncludeSchema.NO, 
                    "select mf.pdoc_codigo, mf.mfac_numedocu from mfacturapedidovehiculo mf, pdocumentohecho ph " +
                    "where mf.mped_codigo='" + codigoPrefijo + "' and mf.mped_numepedi=" + numeroPedido + " and " +
                    "ph.pdoc_codigo=mf.pdoc_codigo and ph.palm_almacen='" + almacen + "' and ph.tpro_proceso='NC';");

                if(dsNota.Tables[0].Rows.Count > 0)
                {
                    DBFunctions.Request(dsNotaValores, IncludeSchema.NO, 
                    "SELECT mfac_valofact, mfac_valoabon, mnit_nit, pcen_codigo, pven_codigo FROM mfacturacliente WHERE pdoc_codigo='" + dsNota.Tables[0].Rows[0][0].ToString() + "' and mfac_numedocu=" + dsNota.Tables[0].Rows[0][1].ToString() );
                    double valorAbonado = Convert.ToDouble(dsNotaValores.Tables[0].Rows[0][1]);
                    double valorAFavor = Convert.ToDouble(dsNotaValores.Tables[0].Rows[0][0].ToString() );
                    string nitCliente = dsNotaValores.Tables[0].Rows[0][2].ToString();
                    string centroCosto = dsNotaValores.Tables[0].Rows[0][3].ToString();
                    string codigoVendedor = dsNotaValores.Tables[0].Rows[0][4].ToString();

                    if (proceso != "E")
                    {
                        //si no tiene abonos realizados.
                        if (dsNotaValores.Tables[0].Rows.Count > 0 && valorAbonado == 0)
                            sqlStrings.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOABON = " + valorAFavor + ", TVIG_VIGENCIA='C' WHERE PDOC_CODIGO = '" + dsNota.Tables[0].Rows[0][0].ToString() + "' AND MFAC_NUMEDOCU = " + dsNota.Tables[0].Rows[0][1].ToString());
                        else //else cuando ya hay abonos se suma el adicional a abonos y el restante como factura FC traida por parametrizacion
                        {
                            sqlStrings.Add("UPDATE MFACTURACLIENTE SET MFAC_VALOABON = " + valorAFavor + ", TVIG_VIGENCIA='C' WHERE PDOC_CODIGO = '" + dsNota.Tables[0].Rows[0][0].ToString() + "' AND MFAC_NUMEDOCU = " + dsNota.Tables[0].Rows[0][1].ToString());
                            double valorFaltante = valorAFavor - valorAbonado;

                            //Crear FC Nota Debido con valorAbonado
                            sqlStrings.Add("INSERT INTO mfacturacliente VALUES('" + prefijoNotaDebito + "'," + numeroNotaDebito + ",'" + nitCliente + "','" + almacen + "','F','V'," +
                                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + valorAbonado + ", 0, 0, 0" +
                                ", 0, 0, 0, 0,'" + centroCosto + "'," +
                                "'Nota Debito Cliente','" + codigoVendedor + "','" + usuarioActual + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");

                            sqlStrings.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU =" + numeroNotaDebito + " WHERE PDOC_CODIGO = '" + prefijoNotaDebito + "';");

                            //Crear Deatalle de factura valor restante para saldo de nota valorFaltante
                            sqlStrings.Add("INSERT INTO DDETALLEFACTURACLIENTE VALUES ('" + prefijoFactura + "'," + numeroFactura + ", '" + prefijoNotaDebito + "'," + numeroNotaDebito + "," + valorFaltante + ", 'Causacion de anticipo aplicado');");
                        }
                    }
                 }



                if (DBFunctions.Transaction(sqlStrings))
				{
                    sqlStrings.Clear();
                    //NotaDevolucionCliente notaDevolucionCliente = new NotaDevolucionCliente(prefijoNotaDevCli, prefijoFactura, contadorNotas, numeroFactura, "N", "FC", 0, "A", "C", DateTime.Now, user, false);
                    //notaDevolucionCliente.GrabarNotaDevolucionCliente(false);
                    //ManejoArrayList.InsertarArrayListEnArrayList(notaDevolucionCliente.SqlStrings, sqlStrings);
                    //if(DBFunctions.Transaction(sqlStrings))
                    //{
					status = true;
                    if (proceso != "E")
                        contaOnline.contabilizarOnline(prefijoNotaDevCli.ToString(), Convert.ToInt32(notaDevolucionClientes.NumeroNota.ToString()), Convert.ToDateTime(fechaDevolucion.ToString()), "");
                    else
                        contaOnline.anularComprobante(prefijoFactura.ToString(), Convert.ToInt32(numeroFactura.ToString()) );

                    if (prefAccesorios != "" && proceso != "E")
                        contaOnline.contabilizarOnline(prefijoNotaDevCli.ToString(), Convert.ToInt32(notaDevolucionAccesorios.NumeroNota.ToString()), DateTime.Now, "");
           			 else
                        if (prefAccesorios != "")
                        {
                            contaOnline.anularComprobante(prefAccesorios.ToString(), Convert.ToInt32(numeroAccesorios.ToString()));
                        }

                    processMsg += "<br>Bien : "+DBFunctions.exceptions;
                    //}
                    //else
                        //processMsg += "<br>Error : " + DBFunctions.exceptions;
				}
				else
					processMsg += "<br>Error : "+DBFunctions.exceptions;
			}
			return status;
		}
		
		protected void Preparar_ArrayList_Update(ArrayList nombreCampo, ArrayList valorActual, ArrayList tipoDato)
		{
            nombreCampo.Add("pcat_codigo");
            if (claseVehiculo == "U")
            {
                valorActual.Add(DBFunctions.SingleData(String.Format("select pcat_codigo from mcatalogovehiculo where mcat_vin='{0}'", catalogo)));
            }
            else
            {
                valorActual.Add(this.catalogo);
            }
            
            tipoDato.Add("C");
			nombreCampo.Add("pcol_codigo");
			valorActual.Add(this.colorPrimario);
			tipoDato.Add("C");
			nombreCampo.Add("pcol_codigoalte");
			valorActual.Add(this.colorOpcional);
			tipoDato.Add("C");
			nombreCampo.Add("tcla_clase");
			valorActual.Add(this.claseVehiculo);
			tipoDato.Add("C");
			nombreCampo.Add("pano_ano");
			valorActual.Add(this.anoModelo);
			tipoDato.Add("NC");
			nombreCampo.Add("mped_fechpedi");
			valorActual.Add(this.fechaPedido);
			tipoDato.Add("C");
			nombreCampo.Add("mped_fechentrega");
			valorActual.Add(this.fechaProgEntrega);
			tipoDato.Add("C");
			nombreCampo.Add("pven_codigo");
			valorActual.Add(this.vendedor);
			tipoDato.Add("C");
			nombreCampo.Add("mnit_nit");
			valorActual.Add(this.nit);
			tipoDato.Add("C");
			nombreCampo.Add("mnit_nit2");
			valorActual.Add(this.nitOpcional);
			tipoDato.Add("C");
			nombreCampo.Add("mped_valounit");
			valorActual.Add(this.valorUnitario);
			tipoDato.Add("NC");
			nombreCampo.Add("mped_valodesc");
			valorActual.Add(this.valorDescuento);
			tipoDato.Add("NC");
			nombreCampo.Add("mped_obsequio");
			valorActual.Add(this.obsequios);
			tipoDato.Add("C");
			///////////////////////////////////
			nombreCampo.Add("mped_valoobse");
			if(this.valorObsequios=="null")
				valorActual.Add("");
			else
				valorActual.Add(this.valorObsequios);
			tipoDato.Add("NC");
			////////////////////////////////////
			nombreCampo.Add("mped_valoefec");
			if(this.valorEfectivo=="null")
				valorActual.Add("");
			else
				valorActual.Add(this.valorEfectivo);
			tipoDato.Add("NC");
			/////////////////////////////////////
			nombreCampo.Add("mped_valocheq");
			if(this.valorCheques=="null")
				valorActual.Add("");
			else
				valorActual.Add(this.valorCheques);
			tipoDato.Add("NC");
			////////////////////////////////////
			nombreCampo.Add("mped_nombfinc");
			valorActual.Add(this.nombreFinanciera);
			tipoDato.Add("C");
			///////////////////////////////////
			nombreCampo.Add("mped_valofinc");
			if(this.valorFinaciera=="null")
				valorActual.Add("");
			else
				valorActual.Add(this.valorFinaciera);
			tipoDato.Add("NC");		
			///////////////////////////////////
			nombreCampo.Add("mped_valootrpago");
			if(this.valorOtrosPagos=="null")
				valorActual.Add("");
			else
				valorActual.Add(this.valorOtrosPagos);
			tipoDato.Add("NC");
			/////////////////////////////////////
			nombreCampo.Add("mped_detaotrpago");
			valorActual.Add(this.detalleOtrosPagos);
			tipoDato.Add("C");
			////////////////////////////////////
			nombreCampo.Add("mped_observacion");
			valorActual.Add(this.observaciones);
			tipoDato.Add("C");
            ////////////////////////////////////
            nombreCampo.Add("tres_sino");
            valorActual.Add(this.credito);
            tipoDato.Add("C");
            ///////////////////////////////////
            nombreCampo.Add("mnit_nitsolicita");
            valorActual.Add(this.nitSolicita);
            tipoDato.Add("C");

            nombreCampo.Add("pclas_codigoventa");
            valorActual.Add(this.claseVenta);
            tipoDato.Add("C");

            nombreCampo.Add("TSER_TIPOSERV");
            valorActual.Add(this.tipoServicio);
            tipoDato.Add("C");

            nombreCampo.Add("PTIPO_CODITIPOVENTA");
            valorActual.Add(this.tipoVenta);
            tipoDato.Add("C");
		}
		
		//Funcion que perimita realizar la unificacion de dos pedidos
		public bool Unificar_Pedidos(string prefijoOrigen, string numeroOrigen, string prefijoDestino, string numeroDestino)
		{
			bool status = false;
			ArrayList sqlStrings = new ArrayList();
			//Primero debemos traer todos los anticipos para el pedido de origen
			DataSet anticiposPedidoOrigen = new DataSet();
			DBFunctions.Request(anticiposPedidoOrigen,IncludeSchema.NO,"SELECT pdoc_codigo, mcaj_numero, mant_valorecicaja FROM manticipovehiculo WHERE mped_codigo='"+prefijoOrigen+"' AND mped_numepedi="+numeroOrigen+"");
			//Ahora debemos actualizar los anticipos para el pedido de destino
			for(int i=0;i<anticiposPedidoOrigen.Tables[0].Rows.Count;i++)
			{
				//Primero debemos verificar si existe un abono al pedido destino en el mismo recibo de pago que estamos revisando
				if(DBFunctions.RecordExist("SELECT * FROM manticipovehiculo WHERE pdoc_codigo='"+anticiposPedidoOrigen.Tables[0].Rows[i][0].ToString()+"' AND mcaj_numero="+anticiposPedidoOrigen.Tables[0].Rows[i][1].ToString()+" AND mped_codigo='"+prefijoDestino+"' AND mped_numepedi="+numeroDestino+""))
				{
					//En caso de que exista este abono debemos sumar el valor, realizar la actualizacion y eliminar la fila del pedido origen
					sqlStrings.Add("UPDATE manticipovehiculo SET mant_valorecicaja = mant_valorecicaja +"+anticiposPedidoOrigen.Tables[0].Rows[i][2].ToString()+" WHERE pdoc_codigo='"+anticiposPedidoOrigen.Tables[0].Rows[i][0].ToString()+"' AND mcaj_numero="+anticiposPedidoOrigen.Tables[0].Rows[i][1].ToString()+" AND mped_codigo='"+prefijoDestino+"' AND mped_numepedi="+numeroDestino+"");
					sqlStrings.Add("DELETE FROM manticipovehiculo WHERE pdoc_codigo='"+anticiposPedidoOrigen.Tables[0].Rows[i][0].ToString()+"' AND mcaj_numero="+anticiposPedidoOrigen.Tables[0].Rows[i][1].ToString()+" AND mped_codigo='"+prefijoOrigen+"' AND mped_numepedi="+numeroOrigen+"");
				}
				else
					sqlStrings.Add("UPDATE manticipovehiculo SET mped_codigo='"+prefijoDestino+"', mped_numepedi="+numeroDestino+" WHERE pdoc_codigo='"+anticiposPedidoOrigen.Tables[0].Rows[i][0].ToString()+"' AND mcaj_numero="+anticiposPedidoOrigen.Tables[0].Rows[i][1].ToString()+"");
			}
			//Actualizamos los vehiculos de retoma que tenia el pedido de origen al pedido de destino
			sqlStrings.Add("UPDATE dpedidovehiculoretoma SET pdoc_codigo='"+prefijoDestino+"',mped_numepedi="+numeroDestino+" WHERE pdoc_codigo='"+prefijoOrigen+"' AND mped_numepedi="+numeroOrigen+"");
			//Ahora debemos realizar la actualizacion del pedido origen a estado cancelado y hacemos la observacion que fue unificado con otro pedido
			sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=40, mped_observacion='Unificado con el Pedido : "+prefijoDestino+"-"+numeroDestino+"' WHERE pdoc_codigo='"+prefijoOrigen+"' AND mped_numepedi="+numeroOrigen+"");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				this.processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				this.processMsg+="<BR> Error: Error en Actualizacion <br>"+DBFunctions.exceptions;
			return status;
		}
		
		//Funcion que asigna un vehiculo a un pedido
		public bool Asignar_Vehiculo_Pedido(string prefijoPedido, string numeroPedido, string numeroInventario, String fechaHoy, String usuario)
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			//Primer se debe realizar la insercion en la tabla masignacionvehiculo del auto asignado
			sqlStrings.Add("INSERT INTO masignacionvehiculo VALUES("+numeroInventario+",'"+prefijoPedido+"',"+numeroPedido+",'" + fechaHoy + "','" + usuario + "')");
			//Ahora debemos realizar la actualizacion de los valores en las tablas mvehiculo y en la tabla mpedidovehiculo
			sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=30 WHERE mveh_inventario="+numeroInventario+"");
			sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=20 WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				this.processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				this.processMsg+="<BR> Error: Error en Actualizacion <br>"+DBFunctions.exceptions;
			return status;			
		}
		
		//Funcion para desasignar un vehiculo  de un pedido
		public bool Desasignar_Vehiculo_Pedido(string prefijoPedido ,string numeroPedido, string numeroInventario)
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			//Ahora eliminamos el registro que se encuentra en masignacionvehiculo
			sqlStrings.Add("DELETE FROM masignacionvehiculo WHERE mveh_inventario="+numeroInventario+" AND pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			//Ahora realizamos la actualizacion de la tabla mpedidovehiculo y de mvehiculo
			sqlStrings.Add("UPDATE mpedidovehiculo SET test_tipoesta=10 WHERE pdoc_codigo='"+prefijoPedido+"' AND mped_numepedi="+numeroPedido+"");
			if(DBFunctions.RecordExist("SELECT * FROM mvehiculo WHERE mveh_inventario="+numeroInventario+" AND pdoc_codiordepago IS NULL AND mfac_numeordepago IS NULL"))
				sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=10 WHERE mveh_inventario="+numeroInventario+"");
			else
				sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=20 WHERE mveh_inventario="+numeroInventario+"");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				this.processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				this.processMsg+="<BR> Error: Error en Actualizacion <br>"+DBFunctions.exceptions;
			return status;
		}
		
		protected void PrepararElementosVenta()
		{
			elementosVenta = new DataTable();
			elementosVenta.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));//0
			elementosVenta.Columns.Add(new DataColumn("COSTO",System.Type.GetType("System.Double")));//1
			
		}
		
		protected void PrepararVehiculosRetoma()
		{
			vehiculosRetoma = new DataTable();
			vehiculosRetoma.Columns.Add(new DataColumn("TIPOVEHICULO",System.Type.GetType("System.String")));//0
			vehiculosRetoma.Columns.Add(new DataColumn("NUMEROCONTRATO",System.Type.GetType("System.String")));//1
			vehiculosRetoma.Columns.Add(new DataColumn("ANOMODELO",System.Type.GetType("System.String")));//2
			vehiculosRetoma.Columns.Add(new DataColumn("NUMEROPLACA",System.Type.GetType("System.String")));//3
			vehiculosRetoma.Columns.Add(new DataColumn("CUENTAIMPUESTOS",System.Type.GetType("System.String")));//4
			vehiculosRetoma.Columns.Add(new DataColumn("VALORRECIBIDO",System.Type.GetType("System.Double")));//5
		}
		
		protected void PrepararTablaAnticipos()
		{
			tablaAnticipos = new DataTable();
			tablaAnticipos.Columns.Add(new DataColumn("PREFIJOANTICIPO",System.Type.GetType("System.String")));//0
			tablaAnticipos.Columns.Add(new DataColumn("NUMEROANTICIPO",System.Type.GetType("System.String")));//1
			tablaAnticipos.Columns.Add(new DataColumn("FECHAANTICIPO",System.Type.GetType("System.String")));//2
			tablaAnticipos.Columns.Add(new DataColumn("VALORANTICIPO",System.Type.GetType("System.Double")));//3
			tablaAnticipos.Columns.Add(new DataColumn("TIPODOCUMENTO",System.Type.GetType("System.String")));//4
		}
		#endregion
	}
}
