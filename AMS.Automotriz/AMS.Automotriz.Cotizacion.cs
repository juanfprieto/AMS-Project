// created on 30/12/2004 at 12:13
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.Forms;
using AMS.DB;

namespace AMS.Automotriz
{
	public class Cotizacion
	{
		protected string codigoPrefijo, numeroOrden, catalogo, vinIdentificacion, nitPropietario, tipoUsuario, estadoOrden;
		protected string cargo, tipoTrabajo, fechaEntrada, horaEntrada, fechaHoraCreacion, fechaEntrega, horaEntrega;
		protected string salida, numeroEntrada, kilometraje, recepcionista, taller, estadoPrecios, numeroLocker, estadoLiquidacion;
		protected string obsrRecepcionista, obsrCliente, tp, tpp, listaPrecios, tipoPago, nivelCombustible, codigoEstadoCita, processMsg="";
		protected DataTable operaciones, repuestos, peritaje;
				
		public string CodigoPrefijo{set{codigoPrefijo = value;}get{return codigoPrefijo;}}
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
		public string ProcessMsg{get{return processMsg;}}
		public DataTable Operaciones{set{operaciones = value;}get{return operaciones;}}
		public DataTable Repuestos{set{repuestos = value;}get{return repuestos;}}
		public DataTable Peritaje{set{peritaje = value;}get{return peritaje;}}
				
		//Constructor de la clase Orden
		
		public Cotizacion()
		{
			operaciones = new DataTable();
			repuestos = new DataTable();
			peritaje = new DataTable();			
		}
		
		public Cotizacion(DataTable opr, DataTable rpt)
		{
			operaciones = opr;
			repuestos = rpt;
		}
		
		public Cotizacion(DataTable opr, DataTable rpt, DataTable prt)
		{
			operaciones = opr;
			repuestos = rpt;
			peritaje = prt;			
		}
		
		//Funcion que guarda la orden de trabajo de taller en la base de datos
		
		public bool CommitValues()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			int i;
			//Primero vamos a verificar si la orden ya existe o no dentro de la tabla morden
			if(!DBFunctions.RecordExist("SELECT * FROM mordencotizacion WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mord_numeorde='"+this.numeroEntrada+"'"))
			{
				//Primero vamos a grabar la nueva orden dentro de morden
				sqlStrings.Add("INSERT INTO mordencotizacion VALUES('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+this.catalogo+"','"+this.vinIdentificacion+"','"+this.nitPropietario+"','"+this.tipoUsuario+"','"+this.estadoOrden+"','"+this.cargo+"','"+this.tipoTrabajo+"','"+this.fechaEntrada+"','"+this.horaEntrada+"','"+this.fechaHoraCreacion+"','"+this.fechaEntrega+"','"+this.horaEntrega+"',"+this.salida+",'"+this.numeroEntrada+"',"+this.kilometraje+",'"+this.recepcionista+"','"+this.taller+"','"+this.estadoPrecios+"','"+this.numeroLocker+"','"+this.estadoLiquidacion+"','"+this.obsrRecepcionista+"','"+this.obsrCliente+"','"+this.tp+"','"+this.tpp+"','"+this.listaPrecios+"','"+this.tipoPago+"','"+this.nivelCombustible+"','"+this.codigoEstadoCita+"',NULL)");
				
				//Ahora vamos a guardar las operaciones en dordenoperacion				
				for(i=0;i<operaciones.Rows.Count;i++)
				{
					sqlStrings.Add("INSERT INTO dordenoperacioncotizacion VALUES ('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+operaciones.Rows[i][2].ToString()+"','"+operaciones.Rows[i][3].ToString()+"','"+operaciones.Rows[i][4].ToString()+"',NULL,'"+operaciones.Rows[i][5].ToString()+"','"+operaciones.Rows[i][6].ToString()+"','"+operaciones.Rows[i][7].ToString()+"','"+operaciones.Rows[i][8].ToString()+"','"+operaciones.Rows[i][9].ToString()+"',NULL,"+operaciones.Rows[i][10].ToString()+",NULL)");
				}
				//Ahora vamos a guardar los items de la cotizacion
				for(i=0;i<repuestos.Rows.Count;i++)
				{
					sqlStrings.Add("INSERT INTO dordenitemcotizacion VALUES ('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+repuestos.Rows[i][2].ToString()+"',"+repuestos.Rows[i][3].ToString()+",'"+repuestos.Rows[i][4].ToString()+"','"+repuestos.Rows[i][5].ToString()+"')");
				}
				//Ahora Revisamos si el servicio es de peritaje y entonces grabamos la tabla dordenperitaje
				if(tipoTrabajo=="P")
				{
					for(i=0;i<peritaje.Rows.Count;i++)
					{
						sqlStrings.Add("INSERT INTO dordenperitajecotizacion VALUES('"+this.codigoPrefijo+"',"+this.numeroOrden+",'"+peritaje.Rows[i][2].ToString()+"','"+peritaje.Rows[i][3].ToString()+"','"+peritaje.Rows[i][4].ToString()+"','"+peritaje.Rows[i][5].ToString()+"',"+peritaje.Rows[i][6].ToString()+")");
					}
				}
				//Ahora Actualizamos el numero del consecutivo dentro dela tabla 
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu = "+this.numeroOrden+" WHERE pdoc_codigo ='" + this.codigoPrefijo + "'");
				if(DBFunctions.Transaction(sqlStrings))
				{
					status = true;
					processMsg += DBFunctions.exceptions + "<br>";
				}
				else
				{
					processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
				}
			}
			else
			{
				this.processMsg+="<BR> Error: Esta Orden Ya Existe";				
			}
			
			
			return status;
		}
		
		/*public bool Borrar_Orden(string CodigoPrefijo, string NumeroOrden)
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
		}*/
	}
}
