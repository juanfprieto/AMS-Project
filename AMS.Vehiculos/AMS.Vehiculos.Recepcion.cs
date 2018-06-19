// created on 31/03/2005 at 14:24
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Documentos;

namespace AMS.Vehiculos
{
	public class Recepcion
	{
		#region Atributos
		//Datos Genericos
		private string prefijoPedido, numeroPedido, ubicacion;
		//Datos Factura
        private string prefijoOrdenPago, numeroOrdenPago, nitProveedor, nitProveedor2, prefijoFacturaProveedor, numeroFacturaProveedor, fechaFactura, almacen, fechaIngreso, fechaVencimiento, fechaUltimoPago, valorFactura, valorRetencion, valorFletes, valorAbono, observacion, valorIva, valorIvaFletes, usuario;
        private string indicativoRetoma, processMsg, estadoFacturaProveedor;
		private ArrayList sqlStrings,sqlRels;
		private DataTable tbInformacionTecnica, tbInformacionComercial;
        private DataRow drInformacionTecnica, drInformacionComercial;
		private Retencion RetencionVehiculos;
		public DataTable dtRetenciones;

       
		#endregion
		
		#region Propiedades 
		public string PrefijoPedido{set{prefijoPedido = value;}get{return prefijoPedido;}}
		public string NumeroPedido{set{numeroPedido = value;}get{return numeroPedido;}}
		public string PrefijoOrdenPago{set{prefijoOrdenPago = value;}get{return prefijoOrdenPago;}}
		public string NumeroOrdenPago{set{numeroOrdenPago = value;}get{return numeroOrdenPago;}}
		public string NitProveedor{set{nitProveedor = value;}get{return nitProveedor;}}
        public string NitProveedor2 { set { nitProveedor2 = value; } get { return nitProveedor2; } }
		public string PrefijoFacturaProveedor{set{prefijoFacturaProveedor = value;}get{return prefijoFacturaProveedor;}}
		public string NumeroFacturaProveedor{set{numeroFacturaProveedor = value;}get{return numeroFacturaProveedor;}}
		public string FechaFactura{set{fechaFactura = value;}get{return fechaFactura;}}
		public string Almacen{set{almacen = value;}get{return almacen;}}
		public string FechaIngreso{set{fechaIngreso = value;}get{return fechaIngreso;}}
		public string FechaVencimiento{set{fechaVencimiento = value;}get{return fechaVencimiento;}}
		public string FechaUltimoPago{set{fechaUltimoPago = value;}get{return fechaUltimoPago;}}
		public string ValorFactura{set{valorFactura = value;}get{return valorFactura;}}
		public string ValorRetencion{set{valorRetencion = value;}get{return valorRetencion;}}
		public string ValorFletes{set{valorFletes = value;}get{return valorFletes;}}
		public string ValorAbono{set{valorAbono = value;}get{return valorAbono;}}
		public string Observacion{set{observacion = value;}get{return observacion;}}
		public string ValorIva{set{valorIva = value;}get{return valorIva;}}
		public string ValorIvaFletes{set{valorIvaFletes = value;}get{return valorIvaFletes;}}
		public string Usuario{set{usuario = value;}get{return usuario;}}
		public string EstadoFacturaProveedor{set{estadoFacturaProveedor = value;}get{return estadoFacturaProveedor;}}
		public string ProcessMsg{set{processMsg = value;}get{return processMsg;}}
		public DataTable TbInformacionTecnica{set{tbInformacionTecnica = value;}get{return tbInformacionTecnica;}}
        public DataTable TbInformacionComercial{set{tbInformacionComercial = value;}get{return tbInformacionComercial;}}
        public DataRow DrInformacionTenica { set { drInformacionTecnica = value; } get { return drInformacionTecnica; } }
        public DataRow DrInformacionComercial { set { drInformacionComercial = value; } get { return drInformacionComercial; } }
        public ArrayList SqlStrings{set{sqlStrings = value;}get{return sqlStrings;}}
		public ArrayList SqlRels{set{sqlRels = value;}get{return sqlRels;}
        
        
        }
		#endregion
				
		#region Constructor
		public Recepcion()
		{
			tbInformacionTecnica = new DataTable();
			tbInformacionComercial = new DataTable();
			sqlStrings = new ArrayList();
			sqlRels = new ArrayList();
		}
		
		public Recepcion(DataTable tbInfTc, DataTable tbInfCmc, string prefijoPedidoEX, string numeroPedidoEX, string ubicacionEX, string nitProveedorEX, string indicativoRetomaEX)
		{
			tbInformacionTecnica = new DataTable();
			tbInformacionComercial = new DataTable();
			//tbInformacionTecnica = tbInfTc;
			tbInformacionTecnica = tbInfTc.Copy();
			tbInformacionComercial = tbInfCmc;
			prefijoPedido = prefijoPedidoEX;
			numeroPedido = numeroPedidoEX;
			ubicacion = ubicacionEX;
			nitProveedor = nitProveedorEX;
			indicativoRetoma = indicativoRetomaEX;
			sqlStrings = new ArrayList();
			sqlRels = new ArrayList();
		}
        //para caso Excel(Recepción y facturación)
        public Recepcion(DataRow tbInfTc, DataRow tbInfCmc, string prefijoPedidoEX, string numeroPedidoEX, string ubicacionEX, string nitProveedorEX, string indicativoRetomaEX)
        {
            drInformacionComercial = tbInfCmc;
            drInformacionTecnica = tbInfTc;
            prefijoPedido = prefijoPedidoEX;
            numeroPedido = numeroPedidoEX;
            ubicacion = ubicacionEX;
            nitProveedor = nitProveedorEX;
            indicativoRetoma = indicativoRetomaEX;
            sqlStrings = new ArrayList();
            sqlRels = new ArrayList();
        }
        #endregion

        #region Metodo
        //Funcion de Recepcion
        //Recibe como parametro un booleano que le indica que genere la facturacion o no
        public bool 
        Realizar_Recepcion(bool factura, bool grabar)
		{
			sqlStrings = new ArrayList();
			bool status = false;
			int i;
            //Se cambia por el primer NIT del proceso.
            String nitSeleccionado = nitProveedor2;
            if (nitProveedor2 == "" || nitProveedor2 == null)
                nitSeleccionado = nitProveedor;
			if(factura)
			{
				observacion += " "+tbInformacionTecnica.Rows.Count;
				//Construir Observacion Catalogo+Vin
				for (i=0;i<tbInformacionTecnica.Rows.Count;i++)
					observacion+=" "+tbInformacionTecnica.Rows[i][0].ToString()+"-"+tbInformacionTecnica.Rows[i][1].ToString()+" ";
							
				FacturaProveedor facturaRepuestos = new FacturaProveedor("FPR" , prefijoOrdenPago , prefijoFacturaProveedor , nitProveedor , Almacen , "F" , Convert.ToUInt64(numeroOrdenPago) , Convert.ToUInt64(numeroFacturaProveedor),
					//0			1					2						3			4		5		6										7									
					estadoFacturaProveedor, Convert.ToDateTime(fechaFactura) , Convert.ToDateTime(fechaVencimiento) , Convert.ToDateTime(null) , Convert.ToDateTime(fechaIngreso) ,
					//8									9											10							11								
					Convert.ToDouble(valorFactura), Convert.ToDouble(valorIva),Convert.ToDouble(valorFletes),Convert.ToDouble(valorIvaFletes),Convert.ToDouble(valorRetencion),
					//12								13								14							15								16							
					observacion,usuario);
				//17			18
				if(valorAbono != null && valorAbono != "")
					facturaRepuestos.ValorAbonos = Convert.ToDouble(valorAbono);
				facturaRepuestos.GrabarFacturaProveedor(false);
				numeroOrdenPago = facturaRepuestos.NumeroFactura.ToString();
				for(i=0;i<facturaRepuestos.SqlStrings.Count;i++)
					 sqlStrings.Add(facturaRepuestos.SqlStrings[i].ToString());
				//Cálculo de retenciones
                
                RetencionVehiculos = new Retencion(nitProveedor2, facturaRepuestos.PrefijoFactura,
					Convert.ToInt32(facturaRepuestos.NumeroFactura),(Convert.ToDouble(valorFactura)+Convert.ToDouble(valorFletes)),
					(Convert.ToDouble(valorIva)+Convert.ToDouble(valorIvaFletes)),"V",false);
				if(dtRetenciones==null)
					RetencionVehiculos.Guardar_Retenciones(false);
				else
					RetencionVehiculos.Guardar_Retenciones(false, dtRetenciones);
				for(i=0;i<RetencionVehiculos.Sqls.Count;i++)
					sqlStrings.Add(RetencionVehiculos.Sqls[i].ToString());
			}

			//Primero debemos grabar los registros de la tabla mcatalogovehiculo
			for(i=0;i<tbInformacionTecnica.Rows.Count;i++)
			{
                //observacion += "<br/>SELECT * FROM mcatalogovehiculo WHERE mcat_vin='"+tbInformacionTecnica.Rows[i][1].ToString()+"'<br/>";
                string fechaVenta = DateTime.Now.ToString("yyyy-MM-dd");
                if (tbInformacionComercial.Rows[i][7].ToString() == "U") // En vehículo usado de toma la fecha de matriculo inicial en la fecha de venta
                    fechaVenta = tbInformacionComercial.Rows[i][9].ToString();
                if (!DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_vin='"+tbInformacionTecnica.Rows[i][1].ToString()+"' "))
                    sqlStrings.Add("INSERT INTO mcatalogovehiculo VALUES('" + tbInformacionTecnica.Rows[i][0].ToString() + "','" + tbInformacionTecnica.Rows[i][1].ToString() + "','" + tbInformacionTecnica.Rows[i][8].ToString() + "','" + tbInformacionTecnica.Rows[i][2].ToString() + "','" + DBFunctions.SingleData("SELECT mnit_nit FROM cempresa") + "','" + tbInformacionTecnica.Rows[i][3].ToString() + "','" + tbInformacionTecnica.Rows[i][4].ToString() + "','" + tbInformacionTecnica.Rows[i][5].ToString() + "'," + tbInformacionTecnica.Rows[i][6].ToString() + ",'" + tbInformacionTecnica.Rows[i][7].ToString() + "',null,null,'" + fechaVenta + "',0,null,0,1000,null,null,'" + DateTime.Now.ToString("yyyy-MM-dd") + "',null)");
			}
			//Ahora Agregamos los registros de la tabla mvehiculo
			for(i=0;i<tbInformacionComercial.Rows.Count;i++)
			{
                string nitPropietario = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
                if (tbInformacionComercial.Rows[i][13].ToString()!="P")
                    nitPropietario = nitProveedor;  // En las Consignaciones o Retomas de Vehiculos, se deja el nit del proveedor como propietario
			    if(!factura)
               	{
					if (valorIva==null)
						valorIva="0";
					if((prefijoPedido == "" && numeroPedido == "") || indicativoRetoma == "S")
                        sqlStrings.Add("INSERT INTO mvehiculo (MVEH_INVENTARIO,PVEN_MERCADEISTA,MCAT_VIN,TEST_TIPOESTA,PDOC_CODIGOPEDIPROV,MPED_NUMERO,MNIT_NIT,MVEH_NUMERECE,MVEH_FECHRECE,MVEH_FECHDISP,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_FECHMANI,MVEH_ADUANA,MVEH_NUMED_O,MVEH_NUMELEVANTE,MVEH_VALOGAST,MVEH_VALOINFL,TCOM_CODIGO,PDOC_CODIORDEPAGO,MFAC_NUMEORDEPAGO,MVEH_VALOCOMP,MVEH_FECHENTR,MPRO_NIT,MVEH_PRENDA,MVEH_VALOIVA) " +
                            "VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',10,null,null,'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "',null,null," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitSeleccionado + "',null," + valorIva + ")");
                        //sqlStrings.Add("INSERT INTO mvehiculo VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',10,null,null,'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "',null,null," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitProveedor + "',null," + valorIva + ")");
					else
					{
                        sqlStrings.Add("INSERT INTO mvehiculo (MVEH_INVENTARIO,PVEN_MERCADEISTA,MCAT_VIN,TEST_TIPOESTA,PDOC_CODIGOPEDIPROV,MPED_NUMERO,MNIT_NIT,MVEH_NUMERECE,MVEH_FECHRECE,MVEH_FECHDISP,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_FECHMANI,MVEH_ADUANA,MVEH_NUMED_O,MVEH_NUMELEVANTE,MVEH_VALOGAST,MVEH_VALOINFL,TCOM_CODIGO,PDOC_CODIORDEPAGO,MFAC_NUMEORDEPAGO,MVEH_VALOCOMP,MVEH_FECHENTR,MPRO_NIT,MVEH_PRENDA,MVEH_VALOIVA) VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',10,'" + prefijoPedido + "'," + numeroPedido + ",'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "',null,null," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitSeleccionado + "',null," + valorIva + ")");
                        //sqlStrings.Add("INSERT INTO mvehiculo VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',10,'" + prefijoPedido + "'," + numeroPedido + ",'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "',null,null," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitProveedor + "',null," + valorIva + ")");
						sqlStrings.Add("UPDATE dpedidovehiculoproveedor SET dped_cantingr = dped_cantingr + 1 WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+tbInformacionComercial.Rows[i][1].ToString()+"'");	
					}
					//Ahora creamos el registro en la tabla mubicacionvehiculo
					sqlStrings.Add("INSERT INTO mubicacionvehiculo VALUES(default,'"+tbInformacionComercial.Rows[i][1].ToString()+"','"+tbInformacionComercial.Rows[i][2].ToString()+"','"+this.ubicacion+"','"+DateTime.Now.ToString("yyyy-MM-dd")+ "',null,null,null,null)");
				}
				else
				{
					if((prefijoPedido == "" && numeroPedido == "") || indicativoRetoma == "S")
                        sqlStrings.Add("INSERT INTO mvehiculo (MVEH_INVENTARIO,PVEN_MERCADEISTA,MCAT_VIN,TEST_TIPOESTA,PDOC_CODIGOPEDIPROV,MPED_NUMERO,MNIT_NIT,MVEH_NUMERECE,MVEH_FECHRECE,MVEH_FECHDISP,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_FECHMANI,MVEH_ADUANA,MVEH_NUMED_O,MVEH_NUMELEVANTE,MVEH_VALOGAST,MVEH_VALOINFL,TCOM_CODIGO,PDOC_CODIORDEPAGO,MFAC_NUMEORDEPAGO,MVEH_VALOCOMP,MVEH_FECHENTR,MPRO_NIT,MVEH_PRENDA,MVEH_VALOIVA) VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',20,null,null,'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "','" + prefijoOrdenPago + "'," + numeroOrdenPago + "," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitSeleccionado + "',null," + valorIva + ")");
                        //sqlStrings.Add("INSERT INTO mvehiculo VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',20,null,null,'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "','" + prefijoOrdenPago + "'," + numeroOrdenPago + "," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitProveedor + "',null," + valorIva + ")");
					else
					{
                        sqlStrings.Add("INSERT INTO mvehiculo (MVEH_INVENTARIO,PVEN_MERCADEISTA,MCAT_VIN,TEST_TIPOESTA,PDOC_CODIGOPEDIPROV,MPED_NUMERO,MNIT_NIT,MVEH_NUMERECE,MVEH_FECHRECE,MVEH_FECHDISP,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_FECHMANI,MVEH_ADUANA,MVEH_NUMED_O,MVEH_NUMELEVANTE,MVEH_VALOGAST,MVEH_VALOINFL,TCOM_CODIGO,PDOC_CODIORDEPAGO,MFAC_NUMEORDEPAGO,MVEH_VALOCOMP,MVEH_FECHENTR,MPRO_NIT,MVEH_PRENDA,MVEH_VALOIVA) VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',20,'" + prefijoPedido + "'," + numeroPedido + ",'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "','" + prefijoOrdenPago + "'," + numeroOrdenPago + "," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitSeleccionado + "',null," + valorIva + ")");
                        //sqlStrings.Add("INSERT INTO mvehiculo VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',20,'" + prefijoPedido + "'," + numeroPedido + ",'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "','" + prefijoOrdenPago + "'," + numeroOrdenPago + "," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitProveedor + "',null," + valorIva + ")");
						sqlStrings.Add("UPDATE dpedidovehiculoproveedor SET dped_cantingr = dped_cantingr + 1 WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+tbInformacionComercial.Rows[i][1].ToString()+"'");
					}
					//Ahora creamos el registro en la tabla mubicacionvehiculo
					sqlStrings.Add("INSERT INTO mubicacionvehiculo VALUES(default,'"+tbInformacionComercial.Rows[i][1].ToString()+"','"+tbInformacionComercial.Rows[i][2].ToString()+"','"+this.ubicacion+"','"+DateTime.Now.ToString("yyyy-MM-dd")+ "',null,null,null,null)");
				}
			}
            if (indicativoRetoma == "S")
			{
				for(i=0;i<tbInformacionTecnica.Rows.Count;i++)
					sqlStrings.Add("UPDATE dpedidovehiculoretoma SET mveh_inventario="+tbInformacionComercial.Rows[i][0].ToString()+" WHERE pcat_codigo='"+tbInformacionTecnica.Rows[i][0].ToString()+"' AND dped_numeplaca='"+tbInformacionTecnica.Rows[i][8].ToString()+"'");
			}
			for(i=0;i<sqlRels.Count;i++)
				sqlStrings.Add(sqlRels[i].ToString());
			if(grabar)
			{
				if(DBFunctions.Transaction(sqlStrings))
				{
					status = true;
					processMsg += DBFunctions.exceptions + "<br>";
				}
				else
					processMsg += "Error" + DBFunctions.exceptions + "<br><br>";
			}
			else
				status = true;
			return status;
		}
        //factura por vehículo(Facturación Excel)
        public bool Realizar_Recepcion_Excel(bool factura, bool grabar)
        {
            sqlStrings = new ArrayList();

            bool status = false;
            int i;
            //Se cambia por el primer NIT del proceso.
            String nitSeleccionado = nitProveedor2;
            if (nitProveedor2 == "" || nitProveedor2 == null)
                nitSeleccionado = nitProveedor;
            if (factura)
            {
                FacturaProveedor facturaRepuestos = new FacturaProveedor("FPR", prefijoOrdenPago, prefijoFacturaProveedor, nitProveedor, Almacen, "F", Convert.ToUInt64(numeroOrdenPago), Convert.ToUInt64(numeroFacturaProveedor),
                    //0			1					2						3			4		5		6										7									
                    estadoFacturaProveedor, Convert.ToDateTime(fechaFactura), Convert.ToDateTime(fechaVencimiento), Convert.ToDateTime(null), Convert.ToDateTime(fechaIngreso),
                    //8									9											10							11								
                    Convert.ToDouble(valorFactura), Convert.ToDouble(valorIva), Convert.ToDouble(valorFletes), Convert.ToDouble(valorIvaFletes), Convert.ToDouble(valorRetencion),
                    //12								13								14							15								16							
                    observacion, usuario);
                    //17		    18
                if (valorAbono != null && valorAbono != "")
                    facturaRepuestos.ValorAbonos = Convert.ToDouble(valorAbono);
                facturaRepuestos.GrabarFacturaProveedorExcel(false);
                numeroOrdenPago = facturaRepuestos.NumeroFactura.ToString();
                for (i = 0; i < facturaRepuestos.SqlStrings.Count; i++)
                    sqlStrings.Add(facturaRepuestos.SqlStrings[i].ToString());
                //Cálculo de retenciones

                RetencionVehiculos = new Retencion(nitProveedor2, facturaRepuestos.PrefijoFactura,
                    Convert.ToInt32(facturaRepuestos.NumeroFactura), (Convert.ToDouble(valorFactura) + Convert.ToDouble(valorFletes)),
                    (Convert.ToDouble(valorIva) + Convert.ToDouble(valorIvaFletes)), "V", false);
                //if (dtRetenciones == null)
                RetencionVehiculos.Guardar_Retenciones(false);
            }
            if (!DBFunctions.RecordExist("SELECT * FROM mcatalogovehiculo WHERE mcat_vin='" + drInformacionTecnica[1] + "' "))
                sqlStrings.Add("INSERT INTO mcatalogovehiculo VALUES('" + drInformacionTecnica[0] + "','" + drInformacionTecnica[1] + "','" + drInformacionTecnica[8] + "','" + drInformacionTecnica[2] + "','" + DBFunctions.SingleData("SELECT mnit_nit FROM cempresa") + "','" + drInformacionTecnica[3] + "','" + drInformacionTecnica[4] + "','" + drInformacionTecnica[5] + "'," + drInformacionTecnica[6] + ",'" + drInformacionTecnica[7] + "',null,null,'" + DateTime.Now.ToString("yyyy-MM-dd") + "',0,null,0,1000,null,null,'" + DateTime.Now.ToString("yyyy-MM-dd") + "',null)");
            
            //Ahora Agregamos EL REGISTRO A MVEHICULO
            string nitPropietario = DBFunctions.SingleData("SELECT mnit_nit FROM cempresa");
            //string i = drInformacionComercial[1];
            if (drInformacionComercial[13].ToString() != "P")
                nitPropietario = nitProveedor;  // En las Consignaciones o Retomas de Vehiculos, se deja el nit del proveedor como propietario
            if (factura)
            {
                sqlStrings.Add("INSERT INTO mvehiculo (MVEH_INVENTARIO,PVEN_MERCADEISTA,MCAT_VIN,TEST_TIPOESTA,PDOC_CODIGOPEDIPROV,MPED_NUMERO,MNIT_NIT,MVEH_NUMERECE,MVEH_FECHRECE,MVEH_FECHDISP,MVEH_KILOMETR,TCLA_CODIGO,MVEH_NUMEMANI,MVEH_FECHMANI,MVEH_ADUANA,MVEH_NUMED_O,MVEH_NUMELEVANTE,MVEH_VALOGAST,MVEH_VALOINFL,TCOM_CODIGO,PDOC_CODIORDEPAGO,MFAC_NUMEORDEPAGO,MVEH_VALOCOMP,MVEH_FECHENTR,MPRO_NIT,MVEH_PRENDA,MVEH_VALOIVA) VALUES(" 
                    + drInformacionComercial[0] + ",null,'" + drInformacionComercial[2] + "',20,'null',null,'" + nitPropietario + "'," + drInformacionComercial[3] + ",'" + drInformacionTecnica[9] + "','" + drInformacionComercial[5] + "'," + drInformacionComercial[6] + ",'" + drInformacionComercial[7] + "','" + drInformacionComercial[8] + "','" + drInformacionComercial[9] + "','" + drInformacionComercial[10] + "','" + drInformacionComercial[11] + "','" + drInformacionComercial[12] + "',0,0,'" + drInformacionComercial[13] + "','" + prefijoOrdenPago + "'," + numeroOrdenPago + "," + drInformacionComercial[14] + ",null,'" + nitSeleccionado + "',null," + valorIva + ")");
                
                //sqlStrings.Add("INSERT INTO mvehiculo VALUES(" + tbInformacionComercial.Rows[i][0].ToString() + ",null,'" + tbInformacionComercial.Rows[i][2].ToString() + "',20,'" + prefijoPedido + "'," + numeroPedido + ",'" + nitPropietario + "'," + tbInformacionComercial.Rows[i][3].ToString() + ",'" + tbInformacionComercial.Rows[i][4].ToString() + "','" + tbInformacionComercial.Rows[i][5].ToString() + "'," + tbInformacionComercial.Rows[i][6].ToString() + ",'" + tbInformacionComercial.Rows[i][7].ToString() + "','" + tbInformacionComercial.Rows[i][8].ToString() + "','" + tbInformacionComercial.Rows[i][9].ToString() + "','" + tbInformacionComercial.Rows[i][10].ToString() + "','" + tbInformacionComercial.Rows[i][11].ToString() + "','" + tbInformacionComercial.Rows[i][12].ToString() + "',0,0,'" + tbInformacionComercial.Rows[i][13].ToString() + "','" + prefijoOrdenPago + "'," + numeroOrdenPago + "," + tbInformacionComercial.Rows[i][14].ToString() + ",null,'" + nitProveedor + "',null," + valorIva + ")");
                sqlStrings.Add("UPDATE dpedidovehiculoproveedor SET dped_cantingr = dped_cantingr + 1 WHERE pdoc_codigo='" + this.prefijoPedido + "' AND mped_numepedi=" + this.numeroPedido + " AND pcat_codigo='" + drInformacionComercial[1] + "'");
                    
                //Ahora creamos el registro en la tabla mubicacionvehiculo
                sqlStrings.Add("INSERT INTO mubicacionvehiculo VALUES(default,'" + drInformacionComercial[1] + "','" + drInformacionComercial[2] + "','" + this.ubicacion + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',null,null,null,null)");
            }
            
            for (i = 0; i < sqlRels.Count; i++)
                sqlStrings.Add(sqlRels[i].ToString());
            if (grabar)
            {
                if (DBFunctions.Transaction(sqlStrings))
                {
                    status = true;
                    processMsg += DBFunctions.exceptions + "<br>";
                }
                else
                    processMsg += "Error" + DBFunctions.exceptions + "<br><br>";
            }
            else
                status = true;
            return status;
        }

        //Funcion para facturar un vehiculo ya recibido
        public bool Facturar_Vehiculo_Recibido(string catalogoVehiculo, string vinVehiculo)
		{
			int i;
			sqlStrings = new ArrayList();
			bool status = false;
			FacturaProveedor facturaVehiculo = new FacturaProveedor("FPR" , prefijoOrdenPago , prefijoFacturaProveedor , nitProveedor , Almacen , "F" , Convert.ToUInt64(numeroOrdenPago) , Convert.ToUInt64(numeroFacturaProveedor),
				//0			1					2						3			4		5		6										7									
				estadoFacturaProveedor, Convert.ToDateTime(fechaFactura) , Convert.ToDateTime(fechaVencimiento) , Convert.ToDateTime(null) , Convert.ToDateTime(fechaIngreso) ,
				//8									9											10							11								
				Convert.ToDouble(valorFactura), Convert.ToDouble(valorIva),Convert.ToDouble(valorFletes),Convert.ToDouble(valorIvaFletes),Convert.ToDouble(valorRetencion),
				//12								13								14							15								16							
				observacion,usuario);
			//17			18
			facturaVehiculo.GrabarFacturaProveedor(false);
			numeroOrdenPago = facturaVehiculo.NumeroFactura.ToString();
			for(i=0;i<facturaVehiculo.SqlStrings.Count;i++)
				sqlStrings.Add(facturaVehiculo.SqlStrings[i].ToString());

			//Cálculo de retenciones
			RetencionVehiculos=new Retencion(facturaVehiculo.NitProveedor,facturaVehiculo.PrefijoFactura,
				Convert.ToInt32(facturaVehiculo.NumeroFactura),(Convert.ToDouble(valorFactura)+Convert.ToDouble(valorFletes)),
				(Convert.ToDouble(valorIva)+Convert.ToDouble(valorIvaFletes)),"V",false);
			
			if(dtRetenciones==null)
				RetencionVehiculos.Guardar_Retenciones(false);
			else
				RetencionVehiculos.Guardar_Retenciones(false, dtRetenciones);
			
			for(i=0;i<RetencionVehiculos.Sqls.Count;i++)
				sqlStrings.Add(RetencionVehiculos.Sqls[i].ToString());

			//Ahora debemos actualizar el registro de mvehiculo y asociarlo a la factura del proveedor
			//Si el vehiculo esta facturado o asignado no se modifica el estado del vehiculo.
            string estadoVehiculo = DBFunctions.SingleData("SELECT test_tipoesta FROM dbxschema.mvehiculo WHERE mcat_vin='" + vinVehiculo + "' order by mveh_inventario desc FETCH FIRST 1 ROWS ONLY;");
            Int32 numeroVehiculo  = Convert.ToInt32(DBFunctions.SingleData("SELECT coalesce(mveh_inventario,0) FROM dbxschema.mvehiculo WHERE mcat_vin='" + vinVehiculo + "' order by mveh_inventario desc FETCH FIRST 1 ROWS ONLY;"));
            if (estadoVehiculo == "10")
                sqlStrings.Add("UPDATE mvehiculo SET pdoc_codiordepago='" + prefijoOrdenPago + "', mfac_numeordepago=" + numeroOrdenPago + ", mveh_valocomp=" + valorFactura + ", MVEH_VALOIVA=" + valorIva + ", test_tipoesta=20 WHERE mveh_inventario = " + numeroVehiculo + " ");
	        else
                sqlStrings.Add("UPDATE mvehiculo SET pdoc_codiordepago='" + prefijoOrdenPago + "', mfac_numeordepago=" + numeroOrdenPago + ", mveh_valocomp=" + valorFactura + ", MVEH_VALOIVA=" + valorIva + " WHERE mveh_inventario = " + numeroVehiculo + " ");				
			
            //Ahora Actualizamos el consecutivo
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}
		
		//Funcion para factuarar un vehiculos ya recibido
		public bool Facturar_Vehiculo_Recibido(DataTable vehiculos)
		{
			int i;
			sqlStrings = new ArrayList();
			bool status = false;
			//Debemos crear el registro de mfacturaproveedor
			FacturaProveedor facturaRepuestos = new FacturaProveedor("FPR" , prefijoOrdenPago , prefijoFacturaProveedor , nitProveedor , Almacen , "F" , Convert.ToUInt64(numeroOrdenPago) , Convert.ToUInt64(numeroFacturaProveedor),
				//0			1					2						3			4		5		6										7									
				estadoFacturaProveedor, Convert.ToDateTime(fechaFactura) , Convert.ToDateTime(fechaVencimiento) , Convert.ToDateTime(null) , Convert.ToDateTime(fechaIngreso) ,
				//8									9											10							11								
				Convert.ToDouble(valorFactura), Convert.ToDouble(valorIva),Convert.ToDouble(valorFletes),Convert.ToDouble(valorIvaFletes),Convert.ToDouble(valorRetencion),
				//12								13								14							15								16							
				observacion,usuario);
			//17			18
			facturaRepuestos.GrabarFacturaProveedor(false);
			numeroOrdenPago = facturaRepuestos.NumeroFactura.ToString();
			for(i=0;i<facturaRepuestos.SqlStrings.Count;i++)
				sqlStrings.Add(facturaRepuestos.SqlStrings[i].ToString());
			
			//Cálculo de retenciones
			RetencionVehiculos=new Retencion(facturaRepuestos.NitProveedor,facturaRepuestos.PrefijoFactura,
				Convert.ToInt32(facturaRepuestos.NumeroFactura),(Convert.ToDouble(valorFactura)+Convert.ToDouble(valorFletes)),
				(Convert.ToDouble(valorIva)+Convert.ToDouble(valorIvaFletes)),"V",false);
			RetencionVehiculos.Guardar_Retenciones(false);
			for(i=0;i<RetencionVehiculos.Sqls.Count;i++)
				sqlStrings.Add(RetencionVehiculos.Sqls[i].ToString());

			//Ahora debemos actualizar los registros de mvehiculo y asociarlo a la factura del proveedor
			for(i=0;i<vehiculos.Rows.Count;i++)
                sqlStrings.Add("UPDATE mvehiculo SET pdoc_codiordepago='" + prefijoOrdenPago + "', mfac_numeordepago=" + numeroOrdenPago + ", test_tipoesta=20 WHERE mcat_vin='" + vehiculos.Rows[i][2].ToString() + "' and test_tipoesta = 10");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}
		
		//Funcion que nos permite devolver un vehiculo
		public static string DevolverVehiculoProveedor(string numInventario, string prefijoNDProv, string usuario, ref uint numND, string observaNota, DateTime fechaDevolucion)
		{
            string fechaDev = fechaDevolucion.ToString("yyyy-MM-dd HH:mm:ss");           
            string error = "";
            string prefijoFacturaRelacionada = DBFunctions.SingleData("SELECT pdoc_codiordepago FROM mvehiculo WHERE mveh_inventario="+numInventario);
			string numeroFacturaRelacionada = DBFunctions.SingleData("SELECT mfac_numeordepago FROM mvehiculo WHERE mveh_inventario="+numInventario);
            Double porcentajeIva = Convert.ToDouble (DBFunctions.SingleData("SELECT piva_porciva FROM pcatalogovehiculo WHERE pcat_codigo=(SELECT mc.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin = mc.mcat_vin and mveh_inventario="+numInventario+")").Trim());
			ArrayList sqlStrings = new ArrayList();			
			//Determinamos si es necesario grabar la nota credito o no
			int estVeh = Convert.ToInt32(DBFunctions.SingleData("SELECT test_tipoesta FROM mvehiculo WHERE mveh_inventario="+numInventario).Trim());

          
            if(estVeh == 10)
			{
				if (prefijoFacturaRelacionada!=String.Empty)
				{
					//Cuando el estado del vehiculo es Transito(10) solo debemos cambiar el estado del vehiculo a 90
					sqlStrings.Add("UPDATE mvehiculo SET test_tipoesta=90 WHERE mveh_inventario="+numInventario);
				//	sqlStrings.Add("insert into DBXSCHEMA.MDEVOLUCIONPEDIDOPROVEEDOR values (null,null,'"+prefijoFacturaRelacionada+"',"+numeroFacturaRelacionada+","+(valorFactura + valorIVA + valorFletes + valorIVAFletes - valorRetenciones).ToString()+",null,'"+usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
                    sqlStrings.Add("insert into DBXSCHEMA.MDEVOLUCIONPEDIDOPROVEEDOR values (null,null,'" + prefijoFacturaRelacionada + "'," + numeroFacturaRelacionada + ",null,null,'" + usuario + "','" + fechaDev + "')");
                    numND = 0;	
				}
				else
					sqlStrings.Add("DELETE FROM mvehiculo WHERE mveh_inventario="+numInventario);
		    }
			else if(estVeh == 20)
			{
                // Valores iniciales de la factura de compra
                DataSet dsFactura = new DataSet();
                double valorFactura , valorIVA , valorFletes , valorIVAFletes , valorRetenciones , valorAbono ; 
                try
                {
                    DBFunctions.Request(dsFactura, IncludeSchema.NO, "SELECT mnit_nit, palm_almacen, mfac_valofact, mfac_valoiva, mfac_valoflet, mfac_valoivaflet, mfac_valorete, mfac_valoabon, mfac_fechentrada FROM mfacturaproveedor WHERE pdoc_codiordepago='" + prefijoFacturaRelacionada + "' AND mfac_numeordepago=" + numeroFacturaRelacionada + "");
                    valorFactura     = Convert.ToDouble(dsFactura.Tables[0].Rows[0][2]);
                    valorIVA         = Convert.ToDouble(dsFactura.Tables[0].Rows[0][3]);
                    valorFletes      = Convert.ToDouble(dsFactura.Tables[0].Rows[0][4]);
                    valorIVAFletes   = Convert.ToDouble(dsFactura.Tables[0].Rows[0][5]);
                    valorRetenciones = Convert.ToDouble(dsFactura.Tables[0].Rows[0][6]);
                    valorAbono       = Convert.ToDouble(dsFactura.Tables[0].Rows[0][7]);
                }
                catch
                {
                    valorFactura    = valorIVA = valorFletes = valorIVAFletes = valorRetenciones = valorAbono = 0; 
                }

                uint numNDProv = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='" + prefijoNDProv + "'"));
                
                //Cuando el estado del vehiculo es Disponible(20) debemos cambiar el estado del vehiculo y crear la nota de devolucion al proveedor
				//uint numNDProv       = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoNDProv+"'"));
				double valorVehiculo = Convert.ToDouble(DBFunctions.SingleData("SELECT mveh_valocomp FROM mvehiculo WHERE mveh_inventario="+numInventario));
				double valorIVAVeh   = Convert.ToDouble(valorVehiculo)*(porcentajeIva/100);
			//	double valorAbonoVeh = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valoabon FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFacturaRelacionada+"' AND mfac_numeordepago="+numeroFacturaRelacionada+""));
                double valorAbonoVeh = valorAbono;
                NotaDevolucionProveedor notaDevolucionPro = new NotaDevolucionProveedor();
				if (prefijoFacturaRelacionada!=String.Empty)
				{
					if(valorAbono == 0)
					{
						string vigenciaFactura = "";
						//Revisamos si se esta factura tiene relacionados mas de un vehiculo en estado 10 o 20
						if(Convert.ToDouble(DBFunctions.SingleData("SELECT COUNT(*) FROM mvehiculo WHERE pdoc_codiordepago='"+prefijoFacturaRelacionada+"' AND mfac_numeordepago="+numeroFacturaRelacionada+" AND (test_tipoesta=10 OR test_tipoesta=20)")) <= 1)
							vigenciaFactura = "C";
						else
							vigenciaFactura = "V";
						notaDevolucionPro = new NotaDevolucionProveedor(prefijoNDProv,prefijoFacturaRelacionada,numNDProv,Convert.ToUInt32(numeroFacturaRelacionada),"N","FP",
							(valorVehiculo+valorIVAVeh),"C",vigenciaFactura,fechaDevolucion,usuario,true);
					}
					else
					{
						double valorFacturaVeh = Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_valofact + mfac_valoiva + mfac_valoflet + mfac_valoivaflet - mfac_valorete FROM mfacturaproveedor WHERE pdoc_codiordepago='"+prefijoFacturaRelacionada+"' AND mfac_numeordepago="+numeroFacturaRelacionada+""));
						if(valorAbono == valorFacturaVeh)
						{
							notaDevolucionPro = new NotaDevolucionProveedor(prefijoNDProv,prefijoFacturaRelacionada,numNDProv,Convert.ToUInt32(numeroFacturaRelacionada),"N","FP",
                                (valorVehiculo + valorIVAVeh), "V", "C", fechaDevolucion, usuario, true);
						}
						else
						{
							if((valorAbono + valorVehiculo + valorIVAVeh) == valorFacturaVeh)
							{
								notaDevolucionPro = new NotaDevolucionProveedor(prefijoNDProv,prefijoFacturaRelacionada,numNDProv,Convert.ToUInt32(numeroFacturaRelacionada),"N","FP",
                                    (valorVehiculo + valorIVAVeh), "C", "C", fechaDevolucion, usuario, true);
							}
							else
							{
								notaDevolucionPro = new NotaDevolucionProveedor(prefijoNDProv,prefijoFacturaRelacionada,numNDProv,Convert.ToUInt32(numeroFacturaRelacionada),"N","FP",
                                    (valorVehiculo + valorIVAVeh), "C", "A", fechaDevolucion, usuario, true);
							}
						}
					}
                    notaDevolucionPro.Observacion = observaNota;
					notaDevolucionPro.GrabarNotaDevolucionProveedor(false);
					numND = notaDevolucionPro.NumeroNota;
					for(int i=0;i<notaDevolucionPro.SqlStrings.Count;i++)
						sqlStrings.Add(notaDevolucionPro.SqlStrings[i].ToString());
					//Ahora Actualizamos el estado del vehiculo
					sqlStrings.Add("UPDATE mvehiculo SET pdoc_codiordepago='"+prefijoNDProv+"', mfac_numeordepago="+numNDProv+" , test_tipoesta=90 WHERE mveh_inventario="+numInventario);
                    sqlStrings.Add("insert into DBXSCHEMA.MDEVOLUCIONPEDIDOPROVEEDOR values ('" + prefijoNDProv + "'," + numNDProv + ",'" + prefijoFacturaRelacionada + "'," + numeroFacturaRelacionada + "," + (valorVehiculo + valorIVAVeh + valorFletes + valorIVAFletes - valorRetenciones).ToString() + "," + valorAbono + " ,'" + usuario + "','" + fechaDev + "')");
                    

                }
				else
				{
					string pcatcodigo = DBFunctions.SingleData("SELECT mc.pcat_codigo FROM mvehiculo mv, mcatalogovehiculo mc WHERE mv.mcat_vin = mc.mcat_vin and mveh_inventario="+numInventario);
					string mcatvin = DBFunctions.SingleData("SELECT mcat_vin FROM mvehiculo WHERE mveh_inventario="+numInventario);
					sqlStrings.Add("DELETE FROM masignacionvehiculo WHERE mveh_inventario="+numInventario);
					sqlStrings.Add("DELETE FROM mvehiculo WHERE mveh_inventario="+numInventario);
					sqlStrings.Add("DELETE FROM mcatalogovehiculo WHERE pcat_codigo='"+pcatcodigo+"' and mcat_vin='"+mcatvin+"' ");
				}
			}
			else
			{
				error += "El estado del vehiculo "+DBFunctions.SingleData("SELECT test_nomesta FROM testadovehiculo WHERE test_tipoesta="+estVeh)+" es invalido. El estado debe ser Transito o Disponible";
				numND = 0;
			}
			if(!DBFunctions.Transaction(sqlStrings))
				error += "Error no se ha podido realizar la devolución del vehiculo"+"*"+DBFunctions.exceptions;
            return error;
		}
		#endregion
	}
}

