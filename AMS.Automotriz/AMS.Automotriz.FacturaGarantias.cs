// created on 17/05/2005 at 15:01
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AMS.DB;

namespace AMS.Automotriz
{
	public class FacturaGarantias
	{
		protected string codigoPrefijo, numeroFactura, nit, almacen, fechaFactura, fechaVencimiento, valorFactura, valorIva, diasPlazo;
		protected string centroCosto, observacion, usuario, processMsg="";
		protected DataTable facturasRelacionadas;
		
		public string CodigoPrefijo{set{codigoPrefijo = value;}get{return codigoPrefijo;}}
		public string NumeroFactura{set{numeroFactura = value;}get{return numeroFactura;}}
		public string Nit{set{nit = value;}get{return nit;}}
		public string Almacen{set{almacen = value;}get{return almacen;}}
		public string FechaFactura{set{fechaFactura = value;}get{return fechaFactura;}}
		public string FechaVencimiento{set{fechaVencimiento = value;}get{return fechaVencimiento;}}
		public string ValorFactura{set{valorFactura = value;}get{return valorFactura;}}
		public string ValorIva{set{valorIva = value;}get{return valorIva;}}
		public string DiasPlazo{set{diasPlazo = value;}get{return diasPlazo;}}
		public string CentroCosto{set{centroCosto = value;}get{return centroCosto;}}
		public string Observacion{set{observacion = value;}get{return observacion;}}
		public string Usuario{set{usuario = value;}get{return usuario;}}
		public string ProcessMsg{set{processMsg = value;}get{return processMsg;}}
		public DataTable FacturasRelacionadas{set{facturasRelacionadas = value;}get{return facturasRelacionadas;}}
		
		public FacturaGarantias()
		{
			facturasRelacionadas = new DataTable();
		}
		
		public FacturaGarantias(DataTable dtInserts)
		{
			facturasRelacionadas = new DataTable();
			facturasRelacionadas = dtInserts;
		}
		
		public bool CommitValues()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			int i;
			int numeroFacturaT = System.Convert.ToInt32(this.numeroFactura);
			while(DBFunctions.RecordExist("SELECT * FROM mfacturacliente WHERE pdoc_codigo='"+this.codigoPrefijo+"' AND mfac_numedocu="+numeroFacturaT.ToString()+""))
				numeroFacturaT += 1;
			//Primero debemos de crear el registro en la tabla mfacturacliente 
			sqlStrings.Add("INSERT INTO mfacturacliente VALUES('"+this.codigoPrefijo+"',"+numeroFacturaT.ToString()+",'"+this.nit+"','"+this.almacen+"','F','V','"+this.fechaFactura+"','"+this.fechaVencimiento+"',null,"+this.valorFactura+","+this.valorIva+",0,0,0,0,0,"+this.diasPlazo+",'"+this.centroCosto+"','"+this.observacion+"',null,'"+this.usuario+"','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
			//Ahora debemos crear los registros correspondientes a la tabla dfacturagarantias
			//Y actualizamos la vigencia de esa factura
			for(i=0;i<facturasRelacionadas.Rows.Count;i++)
			{
				sqlStrings.Add("INSERT INTO dfacturagarantias VALUES('"+this.codigoPrefijo+"',"+this.numeroFactura+",'"+facturasRelacionadas.Rows[i][0].ToString()+"',"+facturasRelacionadas.Rows[i][1].ToString()+","+facturasRelacionadas.Rows[i][3].ToString()+")");
				sqlStrings.Add("UPDATE mfacturacliente SET tvig_vigencia='C' WHERE pdoc_codigo='"+facturasRelacionadas.Rows[i][0].ToString()+"' AND mfac_numedocu="+facturasRelacionadas.Rows[i][1].ToString()+"");
			}
			//Ahora actualizamos el consecutivo de la factura
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu = "+this.numeroFactura+" + 1 WHERE pdoc_codigo='"+this.codigoPrefijo+"'");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}

	}
}
