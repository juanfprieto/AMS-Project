// created on 14/10/2004 at 10:17
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Inventarios;

namespace AMS.Automotriz
{
	public class Orden
	{
		#region Atributos
		protected string codigoPrefijo, numeroOrden, catalogo, vinIdentificacion, nitPropietario, tipoUsuario, estadoOrden,contacto;
		protected string cargo, tipoTrabajo, fechaEntrada, horaEntrada, fechaHoraCreacion, fechaEntrega, horaEntrega;
		protected string salida, numeroEntrada, kilometraje, vendedor, recepcionista, taller, estadoPrecios, numeroLocker, estadoLiquidacion;
		protected string obsrRecepcionista, obsrCliente, tp, tpp, listaPrecios, tipoPago, nivelCombustible, codigoEstadoCita, nitTransferencia;
		protected string tipoTransferencia, prefijoTransferencia, almacenTransferencia, processMsg="";
        protected string gtrasdere, gdelantizq, gtrasizq, gdelantdere, capo, puertadelantdere, puertatrasdere, maletero, puertatrasizq, puertadelantizq, techo;
        protected double totalTransferencia, totalTransferenciaSinIVA;
		protected DataTable operaciones, repuestos, accesorios, peritaje;
		protected ArrayList cargoDetalle,kitsAplicados;
        protected string aceptaEncuesta, revisionElevador, entregoPresupuesto, aceptaUsoDatos;
        protected bool grabarTransferencias;
        protected string tipid, numid, telfijo,txtdireccion,txtemail;
		#endregion
		
		#region Propiedades

		public string CodigoPrefijo{set{codigoPrefijo = value;}get{return codigoPrefijo;}}
        public string Capo { set { capo = value; } get { return capo; } }
        public string Puertadelantizq { set { puertadelantizq = value; } get { return puertadelantizq; } }
        public string Gdelantizq { set { gdelantizq = value; } get { return gdelantizq; } }
        public string Puertadelantdere { set { puertadelantdere = value; } get { return puertadelantdere; } }
        public string Gdelantdere { set { gdelantdere = value; } get { return gdelantdere; } }
        public string Puertatrasizq { set { puertatrasizq = value; } get { return puertatrasizq; } }
        public string Gtrasizq { set { gtrasizq = value; } get { return gtrasizq; } }

        public string Maletero { set { maletero = value; } get { return maletero; } }
        public string Techo { set { techo = value; } get { return techo; } }
        public string Puertatrasdere { set { puertatrasdere = value; } get { return puertatrasdere; } }
        public string Gtrasdere { set { gtrasdere = value; } get { return gtrasdere; } }


		public string NumeroOrden{set{numeroOrden = value;}get{return numeroOrden;}}
		public string Catalogo{set{catalogo = value;}get{return catalogo;}}
		public string VinIdentificacion{set{vinIdentificacion = value;}get{return vinIdentificacion;}}
		public string NitPropietario{set{nitPropietario = value;}get{return nitPropietario;}}
		public string TipoUsuario{set{tipoUsuario = value;}get{return tipoUsuario;}}
		public string EstadoOrden{set{estadoOrden = value;}get{return estadoOrden;}}
		public string Cargo{set{cargo = value;}get{return cargo;}}
		public string TipoTrabajo{set{tipoTrabajo = value;}get{return tipoTrabajo;}}
		public string FechaEntrada{set{fechaEntrada = value;}get{return fechaEntrada;}}
		public string HoraEntrada{set{horaEntrada = value;}get{return horaEntrada;}}
		public string FechaHoraCreacion{set{fechaHoraCreacion = value;}get{return fechaHoraCreacion;}}
		public string FechaEntrega{set{fechaEntrega = value;}get{return fechaEntrega;}}
		public string HoraEntrega{set{horaEntrega = value;}get{return horaEntrega;}}
		public string Salida{set{salida = value;}get{return salida;}}
		public string NumeroEntrada{set{numeroEntrada = value;}get{return numeroEntrada;}}
		public string Kilometraje{set{kilometraje = value;}get{return kilometraje;}}
        public string Vendedor { set { vendedor = value; } get { return vendedor; } }
		public string Recepcionista{set{recepcionista = value;}get{return recepcionista;}}
		public string Taller{set{taller = value;}get{return taller;}}
		public string EstadoPrecios{set{estadoPrecios = value;}get{return estadoPrecios;}}
		public string NumeroLocker{set{numeroLocker = value;}get{return numeroLocker;}}
		public string EstadoLiquidacion{set{estadoLiquidacion = value;}get{return estadoLiquidacion;}}
		public string ObsrRecepcionista{set{obsrRecepcionista = value;}get{return obsrRecepcionista;}}
		public string ObsrCliente{set{obsrCliente = value;}get{return obsrCliente;}}
		public string TP{set{tp = value;}get{return tp;}}
		public string TPP{set{tpp = value;}get{return tpp;}}
		public string ListaPrecios{set{listaPrecios = value;}get{return listaPrecios;}}
		public string TipoPago{set{tipoPago = value;}get{return tipoPago;}}
		public string NivelCombustible{set{nivelCombustible = value;}get{return nivelCombustible;}}
		public string CodigoEstadoCita{set{codigoEstadoCita = value;}get{return codigoEstadoCita;}}
		public string NitTransferencia{set{nitTransferencia = value;}get{return nitTransferencia;}}
		public string TipoTransferencia{set{tipoTransferencia = value;}get{return tipoTransferencia;}}
		public string PrefijoTransferencia{set{prefijoTransferencia = value;}get{return prefijoTransferencia;}}
		public string AlmacenTransferencia{set{almacenTransferencia = value;}get{return almacenTransferencia;}}
		public string ProcessMsg{get{return processMsg;}}
        public string Contacto { set { contacto = value; } get { return contacto; } }
        public string Tipid { set { tipid = value; } get { return tipid; } }
        public string Numid { set { numid = value; } get { return numid; } }
        public string Telfijo { set { telfijo = value; } get { return telfijo; } }
        public string Txtdireccion { set { txtdireccion = value; } get { return txtdireccion; } }
        public string Txtemail { set { txtemail = value; } get { return txtemail; } }
		public double TotalTransferencia{set{totalTransferencia = value;}get{return totalTransferencia;}}
		public double TotalTransferenciaSinIVA{set{totalTransferenciaSinIVA = value;}get{return totalTransferenciaSinIVA;}}
		public DataTable Operaciones{set{operaciones = value;}get{return operaciones;}}
		public DataTable Repuestos{set{repuestos = value;}get{return repuestos;}}
		public DataTable Accesorios{set{accesorios = value;}get{return accesorios;}}
		public DataTable Peritaje{set{peritaje = value;}get{return peritaje;}}
		public ArrayList CargoDetalle{get{return cargoDetalle;}}
		public ArrayList KitsAplicados{set{kitsAplicados=value;} get{return kitsAplicados;}}
		public bool AceptaEncuesta
		{
			set
			{
				bool valorEntrada = value;
				if(valorEntrada)
					aceptaEncuesta="S";
				else
					aceptaEncuesta="N";
			} 
			get
			{
				bool valorSalida = false;
				if(aceptaEncuesta == "S")
					valorSalida = true;
				return valorSalida;
			}
		}
        public bool AceptaUsoDatos
        {
            set
            {
                bool valorEntrada = value;
                if (valorEntrada)
                    aceptaUsoDatos = "S";
                else
                    aceptaUsoDatos = "N";
            }
            get
            {
                bool valorSalida = false;
                if (aceptaUsoDatos == "S")
                    valorSalida = true;
                return valorSalida;
            }
        }

        public bool RevisionElevador
		{
			set
			{
				bool valorEntrada = value;
				if(valorEntrada)
					revisionElevador="S";
				else
					revisionElevador="N";
			} 
			get
			{
				bool valorSalida = false;
				if(revisionElevador == "S")
					valorSalida = true;
				return valorSalida;
			}
		}
		public bool EntregoPresupuesto
		{
			set
			{
				bool valorEntrada = value;
				if(valorEntrada)
					entregoPresupuesto="S";
				else
					entregoPresupuesto="N";
			} 
			get
			{
				bool valorSalida = false;
				if(entregoPresupuesto == "S")
					valorSalida = true;
				return valorSalida;
			}
		}
		#endregion

		#region Constructores
		public Orden()
		{
			operaciones = new DataTable();
			repuestos = new DataTable();
			accesorios = new DataTable();
			peritaje = new DataTable();
			cargoDetalle = new ArrayList();
		}
		
		public Orden(DataTable opr, DataTable acc)
		{
			operaciones = opr;
			accesorios = acc;
			repuestos = new DataTable();
			cargoDetalle = null;
		}
		
		public Orden(DataTable opr, DataTable acc, ArrayList crg)
		{
			operaciones = opr;
			accesorios = acc;
			cargoDetalle = crg;
			repuestos = new DataTable();
		}
		
		public Orden(DataTable opr, DataTable acc, DataTable rpt, ArrayList crg)
		{
			operaciones = opr;
			repuestos = rpt;
			accesorios = acc;
			cargoDetalle = crg;
		}

        public Orden(DataTable opr, DataTable acc, DataTable prt, ArrayList crg, bool grabarTransferencias)
        //public Orden(DataTable opr, DataTable acc, DataTable prt)
        {
            operaciones = opr;
            accesorios = acc;
            peritaje = prt;
            this.grabarTransferencias = grabarTransferencias;
            repuestos = new DataTable();
        }

        public Orden(DataTable opr, DataTable acc, DataTable prt, ArrayList crg, DataTable rpt, bool grabarTransferencias)
		//public Orden(DataTable opr, DataTable acc, DataTable prt)
		{
			operaciones = opr;
			accesorios = acc;
			peritaje = prt;
            this.grabarTransferencias = grabarTransferencias;
			repuestos = rpt;
		}
		#endregion

		#region Funciones de Commit
		//Funcion que guarda la orden de trabajo de taller en la base de datos
		public bool CommitValues()
		{
			ArrayList sqlStrings = new ArrayList();
            bool status = false;
			//bool status = true;
			int i,j;
			//Verficamos el numero de la orden trabajo
			while(DBFunctions.RecordExist("SELECT * FROM morden WHERE pdoc_codigo='"+codigoPrefijo+"' AND mord_numeorde="+numeroOrden))
				numeroOrden = (Convert.ToInt32(numeroOrden)+1).ToString();
			//Primero vamos a grabar la nueva orden dentro de morden
			//sqlStrings.Add("INSERT INTO morden VALUES('"+codigoPrefijo+"',"+numeroOrden+",'" + this.vendedor + "','"+vinIdentificacion+"','"+nitPropietario+"','"+tipoUsuario+"','"+estadoOrden+"','"+this.cargo+"','"+this.tipoTrabajo+"','"+this.fechaEntrada+"','"+this.horaEntrada+"','"+this.fechaHoraCreacion+"','"+this.fechaEntrega+"','"+this.horaEntrega+"',"+this.salida+",'"+this.numeroEntrada+"',"+this.kilometraje+",'"+this.recepcionista+"','"+this.taller+"','"+this.estadoPrecios+"','"+this.numeroLocker+"','"+this.estadoLiquidacion+"','"+this.obsrRecepcionista+"','"+this.obsrCliente+"','"+this.tp+"','"+this.tpp+"','"+this.listaPrecios+"','"+this.tipoPago+"','"+this.nivelCombustible+"','"+this.codigoEstadoCita+"',NULL,'"+this.aceptaEncuesta+"','"+this.revisionElevador+"','"+this.entregoPresupuesto+"','" + this.contacto + "')");
            sqlStrings.Add(
                "INSERT INTO morden VALUES('" + 
                codigoPrefijo + "'," + 
                numeroOrden + ",'" + 
                this.recepcionista + "','" + 
                vinIdentificacion + "','" + 
                nitPropietario + "','" + 
                tipoUsuario + "','" + 
                estadoOrden + "','" + 
                this.cargo + "','" + 
                this.tipoTrabajo + "','" + 
                this.fechaEntrada + "','" + 
                this.horaEntrada + "','" + 
                this.fechaHoraCreacion + "','" + 
                this.fechaEntrega + "','" + 
                this.horaEntrega + "'," + 
                this.salida + ",'" + 
                this.numeroEntrada + "'," + 
                this.kilometraje + ",'" + 
                this.recepcionista + "','" + 
                this.taller + "','" + 
                this.estadoPrecios + "','" + 
                this.numeroLocker + "','" + 
                this.estadoLiquidacion + "','" + 
                this.obsrRecepcionista + "','" + 
                this.obsrCliente + "','" + 
                this.tp + "','" + 
                this.tpp + "','" + 
                this.listaPrecios + "','" + 
                this.tipoPago + "','" + 
                this.nivelCombustible + "','" + 
                this.codigoEstadoCita + 
                "',NULL,'" + aceptaEncuesta + "','" + 
                this.revisionElevador + "','" + 
                this.entregoPresupuesto + "','" +
                this.contacto + "','" +
                this.tipid + "','" + 
                this.numid + "','" +
                this.telfijo + "','" +
                this.txtdireccion + "','" +
                this.txtemail + "')");

            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "1,'" + this.gdelantizq + "')");

            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo+ "','" +
            //      this.numeroOrden +"'," +
            //       "2,'" + this.puertadelantizq + "')");
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "3,'" + this.puertatrasizq + "')");
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "4,'" + this.gtrasizq + "')");
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "5,'" + this.capo + "')");
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "6,'" + this.techo + "')");
           
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "7,'" + this.maletero + "')");

            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "8,'" + this.gdelantdere + "')");

            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "9,'" + this.puertadelantdere + "')");
                                   
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "10,'" + this.puertatrasdere+ "')");
            //sqlStrings.Add("INSERT INTO DORDENCARROCERIA VALUES (DEFAULT,'" +
            //      this.codigoPrefijo + "','" +
            //      this.numeroOrden + "'," +
            //       "11,'" + this.puertatrasdere + "')");
           
            //Registro de Ley 1581 Privacidad de datos.
            //string tipoBase = DBFunctions.SingleData("select mnit_nit from mbasedatos where mnit_nit = '" + nitPropietario + "' and tbas_codigo='O' fetch first row only;");
            //if (tipoBase == "")
            //{
            //    sqlStrings.Add("INSERT INTO MBASEDATOS VALUES ('O' , '" + nitPropietario + "' , '" + aceptaUsoDatos + "')");
            //}

			//Actualizar kilometraje
            sqlStrings.Add("update mcatalogovehiculo set mcat_numeultikilo=" + this.kilometraje + ", mcat_fechultikilo='" + this.fechaEntrada + "' where mcat_vin='" + this.vinIdentificacion + "';");
			//kilometraje al mes o default (CTALLER.KILOPROM_MENSUAL)
			try
			{
				string fechaKilo=DBFunctions.SingleData("SELECT max(mord_creacion) from DBXSCHEMA.morden where mcat_vin='"+this.vinIdentificacion+"';");
				double kiloAnt  =Convert.ToDouble(DBFunctions.SingleData("SELECT MCAT_NUMEULTIKILO FROM mcatalogovehiculo where mcat_vin='"+this.vinIdentificacion+"';"));
				if(kiloAnt<Convert.ToDouble(this.kilometraje))
				{
					if(fechaKilo.Length>0)
					{
						fechaKilo=Convert.ToDateTime(fechaKilo).ToString("yyyy-MM-dd");
						if(!fechaKilo.Equals(this.fechaEntrada))
							sqlStrings.Add(
								"UPDATE MCATALOGOVEHICULO "+
								"SET MCAT_NUMEKILOPROM=(30*"+(Convert.ToDouble(this.kilometraje)-kiloAnt)+")/ABS(DAYS('"+this.fechaEntrada+"')-DAYS('"+fechaKilo+"')) "+
								"where mcat_vin='"+this.vinIdentificacion+"';");
					}
					else
					{
						double kiloProm=Convert.ToDouble(DBFunctions.SingleData("SELECT KILOPROM_MENSUAL FROM ctaller;"));
						sqlStrings.Add(
							"UPDATE MCATALOGOVEHICULO "+
							"SET MCAT_NUMEKILOPROM="+kiloProm+" where mcat_vin='"+this.vinIdentificacion+"';");
					}
				}
			}
			catch{}

			//Ahora vamos a guardar la informacion de los accesorios dentro de dordenaccesorio
            if (accesorios != null)
			    for(i=0;i<accesorios.Rows.Count;i++)
				    sqlStrings.Add("INSERT INTO dordenaccesorio VALUES('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+accesorios.Rows[i][2].ToString()+"','"+accesorios.Rows[i][3]+"')");
			//Cambio realizado el 2006-04-24
			//Ahora vamos a guardar los combos aplicados a esta orden de trabajo
			if(kitsAplicados!=null && kitsAplicados.Count!=0)
			{
				for(i=0;i<kitsAplicados.Count;i++)
					sqlStrings.Add("INSERT INTO dordenkit VALUES('"+codigoPrefijo+"',"+numeroOrden+",'"+kitsAplicados[i].ToString()+"')");
			}
			//Ahora vamos a guardar las operaciones en dordenoperacion
			string cod_incidente,cod_garantia,cod_remedio,cod_defecto;
			cod_incidente=cod_garantia=cod_remedio=cod_defecto= " ";
			for(i=0;i<operaciones.Rows.Count;i++)
			{
				if(operaciones.Rows[i][3].ToString() == "G")
				{
                    if (operaciones.Rows[i][6].ToString() != "Null")
					    cod_incidente="'"+operaciones.Rows[i][6].ToString()+"'";  //  incidente
                    else
                        cod_incidente = "Null";

                    if (operaciones.Rows[i][7].ToString() != "Null")
					    cod_garantia="'"+operaciones.Rows[i][7].ToString()+"'";   //  garantia
                    else
                        cod_garantia = "Null";

                    if (operaciones.Rows[i][8].ToString() != "Null")    
                        cod_remedio="'"+operaciones.Rows[i][8].ToString()+"'";  //  remedio
                    else
                        cod_remedio = "Null";

                    if (operaciones.Rows[i][9].ToString() != "Null")
                        cod_defecto="'"+operaciones.Rows[i][9].ToString()+"'";  //  defecto
                    else
                        cod_defecto = "Null";
				}
				else
				{
					cod_incidente="Null";  //  incidente
					cod_garantia="Null";   //  garantia
					cod_remedio="Null";  //  remedio
					cod_defecto="Null";  //  defecto
				}

                string iva = DBFunctions.SingleData("SELECT CEMP_PORCIVA FROM CEMPRESA");
                if (operaciones.Rows[i][3].ToString() == "G" && Convert.ToString(DBFunctions.SingleData("SELECT CTAL_IVALIQUGARA FROM CTALLER" )) == "N") 
                    iva = "0";

				if(grabarTransferencias)
				    if (operaciones.Rows[i][4].ToString() != "")
                        sqlStrings.Add("INSERT INTO dordenoperacion VALUES ('" + codigoPrefijo + "'," + numeroOrden + ",'" + operaciones.Rows[i][2].ToString() + "','" + operaciones.Rows[i][3].ToString() + "','" + operaciones.Rows[i][4].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','A'," + cod_incidente + "," + cod_garantia + "," + cod_remedio + "," + cod_defecto + ",NULL," + operaciones.Rows[i][10].ToString() + ",NULL," + operaciones.Rows[i][11].ToString() + ",0,'" + iva + "')");
                    //															   prefijo			      numero					tempario									cargo									mecanico				                                            fecha	             estado						incidente			garantia		remedio		defecto		tiempo					valor				Tiempo liq				tiempo dig
                    else
                        sqlStrings.Add("INSERT INTO dordenoperacion VALUES ('" + codigoPrefijo + "'," + numeroOrden + ",'" + operaciones.Rows[i][2].ToString() + "','" + operaciones.Rows[i][3].ToString() + "',NULL,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','S'," + cod_incidente + "," + cod_garantia + "," + cod_remedio + "," + cod_defecto + ",NULL," + operaciones.Rows[i][10].ToString() + ",NULL," + operaciones.Rows[i][11].ToString() + ",0,'" + iva + "')");
                    //															   prefijo			      numero					tempario									cargo					      mec	                                    fecha	           estado	incidente			garantia		remedio		defecto		tiempo				    valor				Tiempo Liq			tiempo dig
				else
                    if (operaciones.Rows[i][4].ToString() != "")
                        sqlStrings.Add("INSERT INTO dordenoperacion VALUES ('" + codigoPrefijo + "'," + numeroOrden + ",'" + operaciones.Rows[i][2].ToString() + "','" + operaciones.Rows[i][3].ToString() + "','" + operaciones.Rows[i][4].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','A'," + cod_incidente + "," + cod_garantia + "," + cod_remedio + "," + cod_defecto + ",NULL," + operaciones.Rows[i][10].ToString() + ",NULL," + operaciones.Rows[i][11].ToString() + ",0,'" + iva + "')");
                    //															prefijo			          numero					tempario									cargo									mecanico				                                                fecha	          estado	  incidente			     garantia		remedio		defecto		tiempo					valor				Tiempo liq				tiempo dig
                    else
                        sqlStrings.Add("INSERT INTO dordenoperacion VALUES ('" + codigoPrefijo + "'," + numeroOrden + ",'" + operaciones.Rows[i][2].ToString() + "','" + operaciones.Rows[i][3].ToString() + "',NULL,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','S'," + cod_incidente + "," + cod_garantia + "," + cod_remedio + "," + cod_defecto + ",NULL," + operaciones.Rows[i][10].ToString() + ",NULL," + operaciones.Rows[i][11].ToString() + ",0,'" + iva + "')");
                    //															prefijo			          numero					tempario									cargo					      mecanico                                      fecha	      estado	  incidente			     garantia		remedio		defecto		tiempo				    valor				Tiempo Liq			tiempo dig
				
                
                
                sqlStrings.Add("INSERT INTO destadisticaoperacion(pdoc_codigo,mord_numeorde,ptem_operacion,pven_codigo,test_estado,destoper_hora) "+
                         "VALUES ('"+codigoPrefijo+"',"+numeroOrden+",'"+operaciones.Rows[i][2].ToString()+"','"+operaciones.Rows[i][4].ToString()+"','"+operaciones.Rows[i][5].ToString()+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");

                //Observaciones
                if (operaciones.Rows[i][12].ToString() != "")
                    sqlStrings.Add("insert into DBXSCHEMA.DOBSERVACIONESOT values (default,'" + codigoPrefijo + "'," + numeroOrden + ",'" + operaciones.Rows[i][2].ToString() + "','" + operaciones.Rows[i][12].ToString() + "','" + recepcionista + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')  ");
                
                //Actualizar MPLANGARANTIA
                if (operaciones.Rows[i][3].ToString().Equals("G"))
                    sqlStrings.Add("UPDATE MPLANGARANTIA " +
                        "SET pdoc_codigo='" + codigoPrefijo + "', mord_numeorde=" + numeroOrden + " " +
                        "WHERE PDOC_CODIGO IS NULL AND MORD_NUMEORDE IS NULL AND " +
                        "PTEM_OPERACION='" + operaciones.Rows[i][2].ToString() + "' AND " +
                        "MCAT_VIN='" + vinIdentificacion + "';");
			}
			//Ahora guardamos los detalles del cargo si es que existen
			if(cargoDetalle!=null)
			{
				if(cargoDetalle[0].ToString()=="seguro")
					sqlStrings.Add("INSERT INTO dordenseguros VALUES('"+codigoPrefijo+"',"+numeroOrden+",'"+cargoDetalle[1].ToString()+"',"+cargoDetalle[3].ToString()+","+cargoDetalle[4].ToString()+",null,'"+cargoDetalle[2].ToString()+"','"+cargoDetalle[5].ToString()+"')");
				else if(cargoDetalle[0].ToString()=="garantia")
					sqlStrings.Add("INSERT INTO dordengarantia VALUES('"+codigoPrefijo+"',"+numeroOrden+",'"+cargoDetalle[1].ToString()+"','"+cargoDetalle[2].ToString()+"')");
			}
			//Ahora Revisamos si el servicio es de peritaje o cotizacion y entonces grabamos la tabla dordenperitaje
            if (tipoTrabajo == "P" || tipoTrabajo == "C")
			{
				for(i=0;i<peritaje.Rows.Count;i++)
					sqlStrings.Add("INSERT INTO dordenperitaje VALUES('"+codigoPrefijo+"',"+numeroOrden+",'"+peritaje.Rows[i][2].ToString()+"','"+peritaje.Rows[i][3].ToString()+"','"+peritaje.Rows[i][4].ToString()+"','"+peritaje.Rows[i][5].ToString()+"',"+peritaje.Rows[i][6].ToString()+")");
			}
			//Ahora Actualizamos el numero del consecutivo dentro de la tabla 
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu = "+numeroOrden+" WHERE pdoc_codigo ='" + codigoPrefijo + "'");
			
            //Ahora vamos a crear los pedidos y las transferencias de acuerdo a cada cargo que se encuentre 
            string pedidoItems = ""; // leemos de algun lado que si maneje los pedidos y las transferencias desde el almacen al taller
            if (tipoTrabajo != "P" && (pedidoItems == "P" || pedidoItems == "T") ) // P = Solo Pedido  T = Pedido Y Transferencia
			{
				ArrayList cargosRepuestos = new ArrayList();
				string stringCargos = "";
				//Primero miramos que cargos se tienen
				for(i=0;i<repuestos.Rows.Count;i++)
				{
					if(stringCargos.IndexOf(repuestos.Rows[i][6].ToString().Trim()) < 0)
					{
						cargosRepuestos.Add(repuestos.Rows[i][6].ToString().Trim());
						stringCargos += repuestos.Rows[i][6].ToString().Trim()+"-";
					}
				}



				uint numeroPedido = Convert.ToUInt32(DBFunctions.SingleData("SELECT pped_ultipedi + 1 FROM ppedido WHERE pped_codigo='"+tipoTransferencia+"'"));
				while(DBFunctions.RecordExist("SELECT * FROM mpedidoitem WHERE pped_codigo='"+tipoTransferencia+"' AND mped_numepedi="+numeroPedido+""))
					numeroPedido += 1;
				uint numeroOrdenT = Convert.ToUInt32(this.numeroOrden);
                uint numeroDocumentoTransferencia  = 0;
                if (prefijoTransferencia != "NULL" && pedidoItems == "T")
                {
				    numeroDocumentoTransferencia = Convert.ToUInt32(DBFunctions.SingleData("SELECT pdoc_ultidocu+1 FROM pdocumento WHERE pdoc_codigo='"+prefijoTransferencia+"'"));
				    while(DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='"+prefijoTransferencia+"' AND mfac_numedocu="+numeroDocumentoTransferencia+""))
					    numeroDocumentoTransferencia += 1;
                }
				//Ahora creamos la factura para cada cargo
				for(i=0;i<cargosRepuestos.Count;i++)
				{
					double valorTransSinIva = 0;
					//Traemos los repuestos a los cuales se van a transferir por el cargo en la posicion i
					DataRow[] itemsCargo = repuestos.Select("CARGOITEM='"+cargosRepuestos[i].ToString()+"'");
					for(j=0;j<itemsCargo.Length;j++)
						valorTransSinIva += Convert.ToDouble(itemsCargo[j][2])*Convert.ToDouble(itemsCargo[j][1]);
					PedidoFactura miPedido = new PedidoFactura("T",tipoTransferencia,nitTransferencia,almacenTransferencia,numeroPedido+Convert.ToUInt32(i),codigoPrefijo,numeroOrdenT,"C",Convert.ToDateTime(fechaEntrada),obsrRecepcionista,0, new string[0], recepcionista,prefijoTransferencia,numeroDocumentoTransferencia+Convert.ToUInt32(i),cargosRepuestos[i].ToString(),0,0,valorTransSinIva,valorTransSinIva);
					//											0         1                 2               3                    4                                5              6      7             8                                9     10 11     12                13                       14                                                 15             16 17      18               19

                    if(pedidoItems == "T") // genera transferencia
                        miPedido.GrabarTransferencias = grabarTransferencias;
                    
                    for(j=0;j<itemsCargo.Length;j++)
						miPedido.InsertaFila(itemsCargo[j][0].ToString(),Convert.ToDouble(itemsCargo[j][1]),Convert.ToDouble(itemsCargo[j][2]),0,Convert.ToDouble(itemsCargo[j][4]),Convert.ToDouble(itemsCargo[j][5]),"","");
					miPedido.RealizarPedFac(false);
					for(j=0;j<miPedido.SqlStrings.Count;j++)
						sqlStrings.Add(miPedido.SqlStrings[j].ToString());
				}
			}
			/*for(i=0;i<sqlStrings.Count;i++)
				processMsg += "<br><br>"+sqlStrings[i].ToString();*/
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}
		#endregion
		
		#region Borrar Orden
		public bool Borrar_Orden(string CodigoPrefijo, string NumeroOrden)
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			//Si la orden ya tiene una referencia a mfacturaclientetaller no la dejamos eliminar
			if(DBFunctions.RecordExist("SELECT * FROM mfacturaclientetaller WHERE mord_numeorde="+NumeroOrden+""))
				this.processMsg += "<BR>Error: La orden ya fue facturada";
			else
			{
				//Ahora vamos a eliminar todos los datos correspondientes a la orden de trabajo en la tabla dordenactividad
				sqlStrings.Add("DELETE FROM dordenactividad WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				//Ahora vamos a eliminar todos los datos correspondientes a la orden de trabajo en la tabla dordenseguros
				sqlStrings.Add("DELETE FROM dordenseguros WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				//Ahora borramos todos los datos correspondientes a la orden de trabajo en la tabla dordenaccesorio
				sqlStrings.Add("DELETE FROM dordenaccesorio WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				//Ahora borramos todos los datos correpondientes a la orden de trabajo en la tabla dordengarantia
				sqlStrings.Add("DELETE FROM dordengarantia WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				//Ahora borramos todos los datos correpondientes a la orden de trabajo en la tabla dordenoperacion
				sqlStrings.Add("DELETE FROM dordenoperacion WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				//Ahora borramos todos los datos correpondientes a la orden de trabajo en la tabla dordenperitaje
				sqlStrings.Add("DELETE FROM dordenperitaje WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				//Ahora podemos borrar los datos que se encuentran en morden y asi queda eliminada la orden de trabajo
				sqlStrings.Add("DELETE FROM morden WHERE pdoc_codigo='"+CodigoPrefijo+"' AND mord_numeorde="+NumeroOrden+"");
				if(DBFunctions.Transaction(sqlStrings))
				{
					status = true;
					this.processMsg += DBFunctions.exceptions + "<br>";
				}
				else
					this.processMsg+="<BR> Error: Esta Orden Ya Existe<br>"+DBFunctions.exceptions;
			}
			return status;			
		}
		#endregion
		
		#region Historial Devolucion Ordenes
		//Funcion que genera el sql neceario para almacenar un estado de la orden de trabajo cuando se realiza la devolucion de una ot
		public static ArrayList AlmacenarHistorialDevolucionOrden(string prefijoOT, string numeroOT, string prefijoFact, string numeroFact, string fechaAnulacion, string usuario)
		{
			ArrayList sqlStrings = new ArrayList();
            // busca el cargo de la factura a Anular
            string cargoFactura = DBFunctions.SingleData("SELECT tcar_cargo FROM mfacturaclientetaller WHERE pdoc_codigo='" + prefijoFact + "' AND mfac_numedocu=" + numeroFact + " AND pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numeroOT + " ");
			//Agregamos el registro de historial de anulacion de la orden en la tabla mordenanulacion
			sqlStrings.Add("INSERT INTO mordenanulacion(pdoc_codigo,mord_numeorde,pdoc_codifact,mfac_numedocu,mord_fechanul,mord_procesado,mord_usuario,mord_descitems,mord_descoper,tcar_cargo, MFAC_FACTORDEDUCIBLE) VALUES"+
                                                      "('" + prefijoOT + "'," + numeroOT + ",'" + prefijoFact + "'," + numeroFact + ",'" + fechaAnulacion + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + usuario + "'," + Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_descrepuestos FROM mfacturaclientetaller WHERE pdoc_codigo='" + prefijoFact + "' AND mfac_numedocu=" + numeroFact + " AND pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numeroOT + "")) + "," + Convert.ToDouble(DBFunctions.SingleData("SELECT mfac_descoperaciones FROM mfacturaclientetaller WHERE pdoc_codigo='" + prefijoFact + "' AND mfac_numedocu=" + numeroFact + " AND pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numeroOT + "")) + ",'" + cargoFactura + "' ," + (DBFunctions.SingleData("SELECT mfac_FACTORDEDUCIBLE FROM mfacturaclientetaller WHERE pdoc_codigo='" + prefijoFact + "' AND mfac_numedocu=" + numeroFact + " AND pdoc_prefordetrab='" + prefijoOT + "' AND mord_numeorde=" + numeroOT + "")) + " ) ");
			//Ahora consultamos que operaciones se han almacenado para esta ot en este momento y se guarda el estado de anulacion
			DataSet ds = new DataSet();
            DBFunctions.Request(ds, IncludeSchema.NO,
                @"SELECT distinct PTEM_OPERACION, DORD_VALOOPER, ROUND(DORD_VALOOPER*PIVA_PORCIVA*0.01,0) AS valor_iva, 
	                    PIVA_porciva, DP.TCAR_cargo, DORD_fechliqu, PVEN_CODIGO, DORD_tiemliqu, DORD_COSTOPER
	              from  dordenoperacion dp, mfacturaclientetallEr mft 
	              where dp.pdoc_codigo = mft.pdoc_prefordetrab and dp.mord_numeorde = mft.mord_numeorde and dp.tcAR_CARGO = MFT.TCAR_CARGO
	                AND MFT.PDOC_PREFORDETRAB = '" + prefijoOT + "' AND MFT.MORD_NUMEORDE = " + numeroOT + " AND MFT.PDOC_CODIGO = '" + prefijoFact + "' AND MFAC_NUMEDOCU = " + numeroFact + " "); 

            for (int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				if(ds.Tables[0].Rows[i][5].ToString() != string.Empty)
					sqlStrings.Add("INSERT INTO dordenoperacionanulacion(pdoc_codigo,mord_numeorde,pdoc_codifact,mfac_numedocu,ptem_operacion,dord_costo,dord_valoiva,piva_porciva,tcar_cargo,dord_fechliqu, PVEN_CODIGO, DORD_TIEMPLIQU, DORD_COSTOPER) VALUES"+
																	   "('"+prefijoOT+"',"+numeroOT+",'"+prefijoFact+"',"+numeroFact+",'"+ds.Tables[0].Rows[i][0]+"',"+ds.Tables[0].Rows[i][1]+","+ds.Tables[0].Rows[i][2]+","+ds.Tables[0].Rows[i][3]+",'"+ds.Tables[0].Rows[i][4]+"','"+Convert.ToDateTime(ds.Tables[0].Rows[i][5]).ToString("yyyy-MM-dd")+ "','" + ds.Tables[0].Rows[i][6] + "'," + ds.Tables[0].Rows[i][7] + "," + ds.Tables[0].Rows[i][8] + ")");
				else
					sqlStrings.Add("INSERT INTO dordenoperacionanulacion(pdoc_codigo,mord_numeorde,pdoc_codifact,mfac_numedocu,ptem_operacion,dord_costo,dord_valoiva,piva_porciva,tcar_cargo,dord_fechliqu, PVEN_CODIGO, DORD_TIEMPLIQU, DORD_COSTOPER) VALUES" +
																	   "('"+prefijoOT+"',"+numeroOT+",'"+prefijoFact+"',"+numeroFact+",'"+ds.Tables[0].Rows[i][0]+"',"+ds.Tables[0].Rows[i][1]+","+ds.Tables[0].Rows[i][2]+","+ds.Tables[0].Rows[i][3]+",'"+ds.Tables[0].Rows[i][4]+"','"+DateTime.Now.ToString("yyyy-MM-dd")+ "','" + ds.Tables[0].Rows[i][6] + "'," + ds.Tables[0].Rows[i][7] + "," + ds.Tables[0].Rows[i][8] + ")");
			}	
			// Ahora consultamos las transferencias que tienen asociadas la orden de trabajo
			ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT	pdoc_factura, mfac_numero, tcar_cargo FROM mordentransferencia WHERE pdoc_codigo='"+prefijoOT+"' AND mord_numeorde="+numeroOT+" AND tcar_CARGO = '"+ cargoFactura +"' ");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				sqlStrings.Add("INSERT INTO dordentransferenciaanulacion(pdoc_codigo,mord_numeorde,pdoc_codifact,mfac_numedocu,pdoc_coditran,mfac_numetran,tcar_cargo) VALUES('"+prefijoOT+"',"+numeroOT+",'"+prefijoFact+"',"+numeroFact+",'"+ds.Tables[0].Rows[i][0]+"',"+ds.Tables[0].Rows[i][1]+",'"+ds.Tables[0].Rows[i][2]+"') ");
			return sqlStrings;
		}
		#endregion
		
		#region Actualizar Ordenes
		//Con esta funcion se busca actualizar la orden de trabajo sin necesidad de borrarla y volverla a grabar en la base de datos
		//Los unicos datos que nunca se dejaran modificar seran el prefijo de la orden y el numero de la orden
		public bool Actualizar_Orden_Trabajo(bool tipoOperacion)
		{
			bool status = false;
			int numeroUpdate = 0, i, j;
			//este string es el update de morden, luego realizaremos los updates correspondientes a las diferentes tablas
			string updatePrincipal = "UPDATE morden SET ";
			ArrayList nombreColumna = new ArrayList();
			ArrayList valorActual = new ArrayList();
			ArrayList tipoDato = new ArrayList();
			ArrayList sqlStrings = new ArrayList();
			this.Preparar_Arrays_Update_Principal(nombreColumna,valorActual,tipoDato);
			//Comenzamos comparando los valores de morden
			for(i=0;i<nombreColumna.Count;i++)
			{
                //if(DBFunctions.SingleData("SELECT "+nombreColumna[i].ToString()+" FROM morden WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+"")!=valorActual[i].ToString())
                //{
					/////////////////////////////////////////////////////////////////////////////
					if(numeroUpdate==0)
						numeroUpdate+=1;
					else
						updatePrincipal += " , ";
					if(tipoDato[i].ToString()=="C")
						updatePrincipal += nombreColumna[i].ToString()+"='"+valorActual[i].ToString()+"' ";
					else
						updatePrincipal += nombreColumna[i].ToString()+"="+valorActual[i].ToString()+" ";
					/////////////////////////////////////////////////////////////////////////////
                //}
			}
			updatePrincipal += "WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+"";
			if(numeroUpdate>0)
				sqlStrings.Add(updatePrincipal);
			//Ahora vamos a realizar la actualizacion de los datos de accesorios
			//Primero debemos traer todos los accesorios que se encuentran grabados para esta orden de trabajo
			DataSet accesoriosOrden = new DataSet();
			DBFunctions.Request(accesoriosOrden,IncludeSchema.NO,"SELECT pacc_codigo, mord_estado FROM dordenaccesorio WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+"");
			//Ahora sabemos que tenemos un DataTable con todos los accesorios entonces lo vamos a empezar a recorrer
			for(i=0;i<accesorios.Rows.Count;i++)
			{
				bool encontrado = false;
				int posicion = 0;
				for(j=0;j<accesoriosOrden.Tables[0].Rows.Count;j++)
				{
					if((accesorios.Rows[i][2].ToString())==(accesoriosOrden.Tables[0].Rows[j][0].ToString()))
					{
						encontrado = true;
						posicion = j;
					}
				}
				if(encontrado)
				{
					//Si este accesorio ya habia sido grabado debemos mirar si sufrio algun cambio o sigue igual
					if((DBFunctions.SingleData("SELECT mord_estado FROM dordenaccesorio WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+" AND pacc_codigo='"+accesorios.Rows[i][2].ToString()+"'"))!=accesorios.Rows[i][3].ToString())
					{
						//primero realizamos la actualizacion
						sqlStrings.Add("UPDATE dordenaccesorio SET mord_estado='"+accesorios.Rows[i][3].ToString()+"' WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+" AND pacc_codigo='"+accesorios.Rows[i][2].ToString()+"'");
					}
					//Ahora Eliminamos esta fila del dataset de consulta sobre los accesorios en grabados en la base de datos
					accesoriosOrden.Tables[0].Rows.RemoveAt(posicion);
				}
				else
					sqlStrings.Add("INSERT INTO dordenaccesorio VALUES('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+accesorios.Rows[i][2].ToString()+"','"+accesorios.Rows[i][3].ToString()+"')");
			}
			//Ahora eliminamos los accesorios que ya no aplican para esta orden de trabajo			
			for(i=0;i<accesoriosOrden.Tables[0].Rows.Count;i++)
				sqlStrings.Add("DELETE FROM dordenaccesorio WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+" AND pacc_codigo='"+accesoriosOrden.Tables[0].Rows[i][0].ToString()+"'");			
			
			if(!tipoOperacion)
			{
				//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				///Ahora vamos a realizar la actualizacion de las operaciones///////////////////////////////////////////////////////////// 
				/// Primero Traemos las operaciones que hay en esta orden de trabajo
				DataSet operacionesOrden = new DataSet();
				DBFunctions.Request(operacionesOrden,IncludeSchema.NO,"SELECT ptem_operacion, tcar_cargo, pven_codigo, test_estado, pgar_codigo1, pgar_codigo2, pgar_codigo3, pgar_codigo4, dord_valooper FROM dordenoperacion WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+"");
				for(i=0;i<operaciones.Rows.Count;i++)
				{
					bool encontrado = false;
					int posicion = 0;
					for(j=0;j<operacionesOrden.Tables[0].Rows.Count;j++)
					{
						if((operacionesOrden.Tables[0].Rows[j][0].ToString())!=(operaciones.Rows[i][2].ToString()))
						{
							encontrado = true;
							posicion = j;
						}
					}
                    double costoOper = 0.00;
                    try
                    {
                        costoOper = Convert.ToDouble(DBFunctions.SingleData(@"select round((coalesce(me.memp_sUELaCtu,COALESCE(PVEN_SALABASI,0))/240 + coalesce(pven_valounidvent * dp.dord_tiemliqu,0) + (coalesce(pv.pven_porccomi,0)*0.01*dord_valooper)) * 1.5,0)
                                                             FROM dordenoperacion dp, PVENDEDOR PV left join  MEMPLEADO ME on PV.MEMP_CODIEMP = ME.MEMP_CODIEMPL
                                                            WHERE pv.pven_codigo = dp.pven_codigo AND DP.PDOC_CODIGO = '" + this.codigoPrefijo + "' AND DP.MORD_NUMEORDE = " + this.numeroOrden + " AND DP.PTEM_OPERACION = '" + operaciones.Rows[i][2].ToString() + "'; "));
                    }
                    catch
                    { }

                    if (encontrado)
					{
						sqlStrings.Add("UPDATE dordenoperacion SET tcar_cargo='"+operaciones.Rows[i][3].ToString()+"', pven_codigo='"+operaciones.Rows[i][4].ToString()+"', test_estado='"+operaciones.Rows[i][5].ToString()+"', pgar_codigo1='"+operaciones.Rows[i][6].ToString()+"', pgar_codigo2='"+operaciones.Rows[i][7].ToString()+"', pgar_codigo3='"+operaciones.Rows[i][8].ToString()+"', pgar_codigo4='"+operaciones.Rows[i][9].ToString()+"', dord_valooper="+operaciones.Rows[i][10].ToString()+", dord_costoper = "+ costoOper +" WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+" AND ptem_operacion='"+operaciones.Rows[i][2].ToString()+"'");
						operacionesOrden.Tables[0].Rows.RemoveAt(posicion);
					}
					else
						sqlStrings.Add("INSERT INTO dordenoperacion VALUES ('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+operaciones.Rows[i][2].ToString()+"','"+operaciones.Rows[i][3].ToString()+"','"+operaciones.Rows[i][4].ToString()+"',NULL,'"+operaciones.Rows[i][5].ToString()+"','"+operaciones.Rows[i][6].ToString()+"','"+operaciones.Rows[i][7].ToString()+"','"+operaciones.Rows[i][8].ToString()+"','"+operaciones.Rows[i][9].ToString()+"',NULL,"+operaciones.Rows[i][10].ToString()+ "," + costoOper + "," + operaciones.Rows[i][11].ToString()+")");
				}
				//Ahora eliminamos las operaciones que ya no aplican para esta orden de trabajo
				for(j=0;j<operacionesOrden.Tables[0].Rows.Count;j++)
					sqlStrings.Add("DELETE FROM dordenaccesorio WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+" AND ptem_operacion='"+operacionesOrden.Tables[0].Rows[i][0].ToString()+"'");
			}
			//Ahora debemos realizar la actualizacion de lo que tiene que ver con cargos especiales
			if(cargoDetalle!=null)
			{
				if(cargoDetalle[0].ToString()=="seguro")
				{
				//	sqlStrings.Add("UPDATE dordenseguros SET mnit_nitseguros='"+cargoDetalle[1].ToString()+"', mord_porcdeducible="+cargoDetalle[3].ToString()+", mord_deduminimo="+cargoDetalle[4].ToString()+", mord_siniestro='"+cargoDetalle[2].ToString()+"', mord_autorizacion='"+cargoDetalle[5].ToString()+"' WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+"");
				}
				else if(cargoDetalle[0].ToString()=="garantia")
				{
					    sqlStrings.Add("UPDATE dordengarantia SET mnit_nitfabrica='"+cargoDetalle[1].ToString()+"', mord_autorizacion='"+cargoDetalle[2].ToString()+"' WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+"");
				}
			}
			//Ahora revisamos si se necesita realizar algun cambio para las operaciones de peritaje
			if(tipoTrabajo=="P")
			{
				for(i=0;i<peritaje.Rows.Count;i++)
				{
					sqlStrings.Add("UPDATE dordenperitaje SET tespe_codigo='"+peritaje.Rows[i][4].ToString()+"', dorpe_detalle='"+peritaje.Rows[i][5].ToString()+"', dorpe_costo="+peritaje.Rows[i][6].ToString()+" WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde="+this.numeroOrden+" AND pgrp_codigo='"+peritaje.Rows[i][2].ToString()+"' AND pitp_codigo='"+peritaje.Rows[i][3].ToString()+"'");
				}
			}
			//Realizamos el llamado a la transaccion
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				this.processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				this.processMsg+="<BR> Error: Error en Actualizacion <br>"+DBFunctions.exceptions;
			return status;
		}
		#endregion
		
		#region Prepara Arrays Update
        
        
        
		private void Preparar_Arrays_Update_Principal(ArrayList nombreColumna, ArrayList valorActual, ArrayList tipoDato)
		{
            //DataSet dsMorden = new DataSet();
            //DBFunctions.Request(dsMorden, IncludeSchema.NO, "select * from morden");
            //dsMorden.Clear();
            string pven_Merc = DBFunctions.SingleData("select NAME from sysibm.syscolumns where tbname = 'MORDEN' AND COLNO ='2'");

            if (pven_Merc == "PVEN_MERCADERISTA")
            {
                nombreColumna.Add("PVEN_MERCADERISTA");// TEMPORAL PARA PVEN_MERCADEISTA QUE AHORA ES PVEN_CODIGO
                valorActual.Add(this.recepcionista);
            }
            else
            {
                nombreColumna.Add("PVEN_MERCADEISTA");// TEMPORAL PARA PVEN_MERCADEISTA QUE AHORA ES PVEN_CODIGO
                valorActual.Add(this.recepcionista);
            }
			//nombreColumna.Add("pven_mercadeista");// Vendedor
			//valorActual.Add(this.vendedor);
			//tipoDato.Add("C");
            //valorActual.Add(null);
            tipoDato.Add("C");
			nombreColumna.Add("mcat_vin");//vin de identificacion
			valorActual.Add(this.vinIdentificacion);
			tipoDato.Add("C");
			nombreColumna.Add("mnit_nit");//nit propietario
			valorActual.Add(this.nitPropietario);
			tipoDato.Add("C");
			nombreColumna.Add("tpro_codigo");//tipo de usuario
			valorActual.Add(this.tipoUsuario);
			tipoDato.Add("C");
			nombreColumna.Add("test_estado");//estado orden
			valorActual.Add(this.estadoOrden);
			tipoDato.Add("C");
			nombreColumna.Add("tcar_cargo");//cargo orden
			valorActual.Add(this.cargo);
			tipoDato.Add("C");
			nombreColumna.Add("ttra_codigo");//tipo trabajo
			valorActual.Add(this.tipoTrabajo);
			tipoDato.Add("C");
			nombreColumna.Add("mord_entrada");//fecha entrada
			valorActual.Add(this.fechaEntrada);
			tipoDato.Add("C");
			nombreColumna.Add("mord_horaentr");//hora entrada
			valorActual.Add(this.horaEntrada);
			tipoDato.Add("C");
			nombreColumna.Add("mord_entregar");//fecha  entrega
			valorActual.Add(this.fechaEntrega);
			tipoDato.Add("C");
			nombreColumna.Add("mord_horaentg");//hora entregar
			valorActual.Add(this.horaEntrega);
			tipoDato.Add("C");
			nombreColumna.Add("mord_numeentr");//numero entrada
			valorActual.Add(this.numeroEntrada);
			tipoDato.Add("C");
			nombreColumna.Add("mord_kilometraje");//kilometraje
			valorActual.Add(this.kilometraje);
			tipoDato.Add("I");
			nombreColumna.Add("pven_codigo");//codigo recepcionista
			valorActual.Add(this.recepcionista);
			tipoDato.Add("C");
			nombreColumna.Add("palm_almacen");//codigo taller
			valorActual.Add(this.taller);
			tipoDato.Add("C");
			nombreColumna.Add("mord_okprecio");//estado precios
			valorActual.Add(this.estadoPrecios);
			tipoDato.Add("C");
			nombreColumna.Add("mord_locker");//numero locker
			valorActual.Add(this.numeroLocker);
			tipoDato.Add("C");
			nombreColumna.Add("mord_estaliqu");//estado liquidacion
			valorActual.Add(this.estadoLiquidacion);
			tipoDato.Add("C");
			nombreColumna.Add("mord_obserece");//observaciones recepcionista
			valorActual.Add(this.obsrRecepcionista);
			tipoDato.Add("C");
			nombreColumna.Add("mord_obseclie");//observaciones cliente
			valorActual.Add(this.obsrCliente);
			tipoDato.Add("C");
			nombreColumna.Add("mord_tp");//tp
			valorActual.Add(this.tp);
			tipoDato.Add("C");
			nombreColumna.Add("mord_tpp");//tpp
			valorActual.Add(this.tpp);
			tipoDato.Add("C");
			nombreColumna.Add("ppreta_codigo");//lista de precios
			valorActual.Add(this.listaPrecios);
			tipoDato.Add("C");
			nombreColumna.Add("ttip_codigo");//tipo de pago
			valorActual.Add(this.tipoPago);
			tipoDato.Add("C");
			nombreColumna.Add("tnivcomb_codigo");//nivel combustible
			valorActual.Add(this.nivelCombustible);
			tipoDato.Add("C");
		}
		#endregion

		#region Almacenamiento Costos Factura de Orden Trabajo

		public static void AlmacenarCostosFacturaOT(string prefijoFactura, uint numeroFactura)
		{
			//Traemos el prefijo y número de orden, cargo de factura y valor de la factura
			string prefijoOrden     = DBFunctions.SingleData("SELECT pdoc_prefordetrab FROM mfacturaclientetaller WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura);
			int numeroOrden         = Convert.ToInt32(DBFunctions.SingleData("SELECT mord_numeorde FROM mfacturaclientetaller WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura));
			string cargoFactura     = DBFunctions.SingleData("SELECT tcar_cargo FROM mfacturaclientetaller WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura);
			string nitFactura       = DBFunctions.SingleData("SELECT mnit_nit FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura);
			double valorFactura     = Convert.ToDouble(DBFunctions.SingleData("SELECT (mfac_valofact+mfac_valoiva+mfac_valoflet+mfac_valoivaflet-mfac_valoabon) FROM mfacturacliente WHERE pdoc_codigo='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura));
			double porcentajeDeducible = 0, deducibleMinimo = 0, totalMovimientos = 0;
			string nitAseguradora   = DBFunctions.SingleData("SELECT mnit_nitseguros FROM dordenseguros WHERE pdoc_codigo='"+prefijoOrden+"' AND mord_numeorde="+numeroOrden);
			try{porcentajeDeducible = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_porcdeducible FROM dordenseguros WHERE pdoc_codigo='"+prefijoOrden+"' AND mord_numeorde="+numeroOrden));}catch{}
			try{deducibleMinimo     = Convert.ToDouble(DBFunctions.SingleData("SELECT mord_deduminimo FROM dordenseguros WHERE pdoc_codigo='"+prefijoOrden+"' AND mord_numeorde="+numeroOrden));}catch{}
			//Se trae el costo y valor public de las operaciones clasificadas por tipo de operacion de la orden de trabajo seleccionada
			DataSet ds = new DataSet();
            /*
             * este llamado funciona solamente , ya que la vista VTALLER_CONSULTACOSTOS_RUTINA
             * está dañada su base de datos (error SQL0083C, Se ha producido un error de asignación de memoria). 
             * Dejar en comentario para lo demás.
             */
            DBFunctions.Request(ds, IncludeSchema.NO, "select tipo_operacion,sum(valor_publico),sum(costo_total) " +
                                                    "from VTALLER_CONSULTACOSTOS_RUTINAH " +
                                                    "where prefijo_orden='" + prefijoOrden + "' AND numero_orden=" + numeroOrden + " AND cargo_relacionado='" + cargoFactura + "' " +
                                                    "group by tipo_operacion;" +
                                                    "SELECT codigo_orden, numero_orden, SUM((cantidad_original - COALESCE(cantidad_devuelta,0))*costo_promedio), SUM(total) " +
                                                    "FROM vtaller_consultaitemsotcont " +
                                                    "WHERE codigo_orden='" + prefijoOrden + "' AND numero_orden=" + numeroOrden + " AND cargo_factura='" + cargoFactura + "' " +
                                                    "GROUP BY codigo_orden, numero_orden");
            
            //este llamado funciona para los demás, incluido el servidor
            /*
            DBFunctions.Request(ds,IncludeSchema.NO,"select tipo_operacion,sum(valor_publico),sum(costo_total) "+
													"from VTALLER_CONSULTACOSTOS_RUTINA "+
													"where prefijo_orden='"+prefijoOrden+"' AND numero_orden="+numeroOrden+" AND cargo_relacionado='"+cargoFactura+"' "+
													"group by tipo_operacion;"+
													"SELECT codigo_orden, numero_orden, SUM((cantidad_original - COALESCE(cantidad_devuelta,0))*costo_promedio), SUM(total) "+
													"FROM vtaller_consultaitemsotcont "+
													"WHERE codigo_orden='"+prefijoOrden+"' AND numero_orden="+numeroOrden+" AND cargo_factura='"+cargoFactura+"' "+
													"GROUP BY codigo_orden, numero_orden");
			*/
            //Se recorre la consulta y se constryen los valores de cada tipo de operacion
			Hashtable valoresCostoTipoOperacion = Orden.ConstruirHashTableTipoOperaciones();
			Hashtable valoresPublicoTipoOperacion = Orden.ConstruirHashTableTipoOperaciones();
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
			{
				valoresCostoTipoOperacion[ds.Tables[0].Rows[i][0].ToString().Trim()] = Convert.ToDouble(ds.Tables[0].Rows[i][2]);
				valoresPublicoTipoOperacion[ds.Tables[0].Rows[i][0].ToString().Trim()] = Convert.ToDouble(ds.Tables[0].Rows[i][1]);
				totalMovimientos += Convert.ToDouble(ds.Tables[0].Rows[i][1]);
			}
			double costoRepuestos = 0;
			double valorPublicoRepuestos = 0;
			try{costoRepuestos = Convert.ToDouble(ds.Tables[1].Rows[0][2]);}catch{}
			try{valorPublicoRepuestos = Convert.ToDouble(ds.Tables[1].Rows[0][3]);totalMovimientos += valorPublicoRepuestos;}catch{}
			if(cargoFactura != "S")
			{
				//Ahora se realiza la insercion en la tabla de dordencostos
				DBFunctions.NonQuery("INSERT INTO dordencostos(  pdoc_codiorde,    mord_numeorde,     pdoc_codifac,    mfac_numedocu,        tcar_cargo,               dorc_eleccost,						dorc_elecvp,						dorc_electcost,							dorc_electvp,							dorc_latcost,							dorc_latvp,						dorc_mobcost,								dorc_mobvp,						dorc_pincost,							dorc_pinvp,								dorc_tercost,							dorc_tervp,						dorc_tyvcost,						dorc_tyvvp,									dorc_tvcost,						dorc_tvvp,						dorc_repcost,	dorc_repvp) VALUES"+
															 "('"+prefijoOrden+"',"+numeroOrden+",'"+prefijoFactura+"',"+numeroFactura+",'"+cargoFactura+"',"+valoresCostoTipoOperacion["ELE"]+","+valoresPublicoTipoOperacion["ELE"]+","+valoresCostoTipoOperacion["ETN"]+","+valoresPublicoTipoOperacion["ETN"]+","+valoresCostoTipoOperacion["LAT"]+","+valoresPublicoTipoOperacion["LAT"]+","+valoresCostoTipoOperacion["MOB"]+","+valoresPublicoTipoOperacion["MOB"]+","+valoresCostoTipoOperacion["PIN"]+","+valoresPublicoTipoOperacion["PIN"]+","+valoresCostoTipoOperacion["TER"]+","+valoresPublicoTipoOperacion["TER"]+","+valoresCostoTipoOperacion["TYV"]+","+valoresPublicoTipoOperacion["TYV"]+","+valoresCostoTipoOperacion["VAR"]+","+valoresPublicoTipoOperacion["VAR"]+","+costoRepuestos+","+valorPublicoRepuestos+")");
			}
			else
			{
				double valorDeducibleCalculado = totalMovimientos * (porcentajeDeducible/100);
				double valorMultiplicador = 0;
				if(valorDeducibleCalculado < deducibleMinimo)
					valorDeducibleCalculado = deducibleMinimo;
				if(nitFactura == nitAseguradora)
					valorMultiplicador = totalMovimientos - valorDeducibleCalculado;
				else
					valorMultiplicador = valorDeducibleCalculado;
				DBFunctions.NonQuery("INSERT INTO dordencostos(  pdoc_codiorde,    mord_numeorde,     pdoc_codifac,    mfac_numedocu,        tcar_cargo,               dorc_eleccost,						dorc_elecvp,						dorc_electcost,							dorc_electvp,							dorc_latcost,							dorc_latvp,						dorc_mobcost,								dorc_mobvp,						dorc_pincost,							dorc_pinvp,								dorc_tercost,							dorc_tervp,						dorc_tyvcost,						dorc_tyvvp,									dorc_tvcost,						dorc_tvvp,						dorc_repcost,	dorc_repvp) VALUES"+
															 "('"+prefijoOrden+"',"+numeroOrden+",'"+prefijoFactura+"',"+numeroFactura+",'"+cargoFactura+"',"+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["ELE"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["ELE"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["ETN"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["ETN"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["LAT"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["LAT"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["MOB"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["MOB"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["PIN"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["PIN"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["TER"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["TER"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["TYV"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["TYV"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresCostoTipoOperacion["VAR"])*100)/totalMovimientos)/100))+","+(valorMultiplicador*(((Convert.ToDouble(valoresPublicoTipoOperacion["VAR"])*100)/totalMovimientos)/100))+","+valorMultiplicador*(((costoRepuestos*100)/totalMovimientos)/100)+","+valorMultiplicador*(((valorPublicoRepuestos*100)/totalMovimientos)/100)+")");
			}
		}

		public static Hashtable ConstruirHashTableTipoOperaciones()
		{
			Hashtable tipoOperacion = new Hashtable();
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT tope_codigo FROM ttipooperaciontaller");
			for(int i=0;i<ds.Tables[0].Rows.Count;i++)
				tipoOperacion.Add(ds.Tables[0].Rows[i][0].ToString().Trim(),0.0);
			return tipoOperacion;
		}

		#endregion

		#region Almacenamiento Costos Devolucion de Orden Trabajo

		public static void AlmacenarCostosDevolucionOT(string prefijoFactura, uint numeroFactura, string prefijoDevolucion, uint numeroDevolucion)
		{
			//Se realiza la consulta de los costos almacenados por la factura original
			DataSet ds = new DataSet();
			DBFunctions.Request(ds,IncludeSchema.NO,"SELECT pdoc_codiorde,mord_numeorde,pdoc_codifac,mfac_numedocu,tcar_cargo,dorc_eleccost,dorc_elecvp,dorc_electcost,dorc_electvp,dorc_latcost,dorc_latvp,dorc_mobcost,dorc_mobvp,dorc_pincost,dorc_pinvp,dorc_tercost,dorc_tervp,dorc_tyvcost,dorc_tyvvp,dorc_tvcost,dorc_tvvp,dorc_repcost,dorc_repvp FROM dordencostos WHERE pdoc_codifac='"+prefijoFactura+"' AND mfac_numedocu="+numeroFactura);
			if(ds.Tables[0].Rows.Count > 0)
				DBFunctions.NonQuery("INSERT INTO dordencostos(			pdoc_codiorde,				mord_numeorde,				pdoc_codifac,		mfac_numedocu,		tcar_cargo,							dorc_eleccost,				dorc_elecvp,			dorc_electcost,				dorc_electvp,					dorc_latcost,				dorc_latvp,					dorc_mobcost,					dorc_mobvp,					dorc_pincost,				dorc_pinvp,					dorc_tercost,					dorc_tervp,					dorc_tyvcost,				dorc_tyvvp,					dorc_tvcost,					dorc_tvvp,					dorc_repcost,			dorc_repvp) VALUES"+
					"('"+ds.Tables[0].Rows[0][0]+"',"+ds.Tables[0].Rows[0][1]+",'"+prefijoDevolucion+"',"+numeroDevolucion+",'"+ds.Tables[0].Rows[0][4]+"',"+ds.Tables[0].Rows[0][5]+","+ds.Tables[0].Rows[0][6]+","+ds.Tables[0].Rows[0][7]+","+ds.Tables[0].Rows[0][8]+","+ds.Tables[0].Rows[0][9]+","+ds.Tables[0].Rows[0][10]+","+ds.Tables[0].Rows[0][11]+","+ds.Tables[0].Rows[0][12]+","+ds.Tables[0].Rows[0][13]+","+ds.Tables[0].Rows[0][14]+","+ds.Tables[0].Rows[0][15]+","+ds.Tables[0].Rows[0][16]+","+ds.Tables[0].Rows[0][17]+","+ds.Tables[0].Rows[0][18]+","+ds.Tables[0].Rows[0][19]+","+ds.Tables[0].Rows[0][20]+","+ds.Tables[0].Rows[0][21]+","+ds.Tables[0].Rows[0][22]+")");
		}
		#endregion
	}
}

