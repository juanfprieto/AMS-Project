// created on 28/04/2005 at 13:21
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;
using AMS.Contabilidad;

namespace AMS.Vehiculos
{
	public class FacturaGastos
	{
		protected string prefijoFactura, numeroFactura, nit, prefijoProveedor, numeroProveedor, fechaFactura, almacen;
		protected string fechaVencimiento, valorFactura, valorIva, observacion, usuario, processMsg;
		protected DataTable dtItems, dtGastos, dtElementos,dtRetenciones;
		
		public string PrefijoFactura{set{prefijoFactura = value;}get{return prefijoFactura;}}
		public string NumeroFactura{set{numeroFactura = value;}get{return numeroFactura;}}
		public string Nit{set{nit = value;}get{return nit;}}
		public string PrefijoProveedor{set{prefijoProveedor = value;}get{return prefijoProveedor;}}
		public string NumeroProveedor{set{numeroProveedor = value;}get{return numeroProveedor;}}
		public string FechaFactura{set{fechaFactura = value;}get{return fechaFactura;}}
		public string Almacen{set{almacen = value;}get{return almacen;}}
		public string FechaVencimiento{set{fechaVencimiento = value;}get{return fechaVencimiento;}}
		public string ValorFactura{set{valorFactura = value;}get{return valorFactura;}}
		public string ValorIva{set{valorIva = value;}get{return valorIva;}}
		public string Observacion{set{observacion = value;}get{return observacion;}}
		public string Usuario{set{usuario = value;}get{return usuario;}}
		public string ProcessMsg{set{processMsg = value;}get{return processMsg;}}
		public DataTable DtItems{set{dtItems = value;}get{return dtItems;}}
		public DataTable DtGastos{set{dtGastos = value;}get{return dtGastos;}}
		private string tipo;

        ProceHecEco contaOnline = new ProceHecEco();
		
		//Constructores
		public FacturaGastos()
		{
			dtItems = new DataTable();
			dtGastos = new DataTable();
		}
		
		public FacturaGastos(string tp, DataTable Items, DataTable Gastos, DataTable Elementos)
		{
			dtItems = new DataTable();
			dtItems = Items;
			dtGastos = new DataTable();
			dtGastos = Gastos;
			dtElementos = new DataTable();
			dtElementos = Elementos;
			tipo=tp;
		}
		
		public FacturaGastos(string tp, DataTable Items, DataTable Gastos, DataTable Elementos, DataTable Retenciones)
		{
			dtItems = new DataTable();
			dtItems = Items;
			dtGastos = new DataTable();
			dtGastos = Gastos;
			dtElementos = new DataTable();
			dtElementos = Elementos;
			dtRetenciones = Retenciones;
			tipo=tp;
		}
		
		public bool Guardar_Factura(bool items)	//Si el booleano es verdadero son items de vehículos sino embarques
		{
			ArrayList sqlStrings = new ArrayList();
			int i;
            double valoFac = 0;
			double retencion=0;
			bool status = false;
			string tabla="";
			if(tipo=="V")tabla="dfacturagastovehiculo";
			if(tipo=="E")tabla="dfacturagastoembarque";
			//Retenciones?
			if(dtRetenciones!=null)
				if(dtRetenciones.Rows.Count!=0)
					for(i=0;i<dtRetenciones.Rows.Count;i++)
						retencion+=Convert.ToDouble(dtRetenciones.Rows[i][3]);
			//Registro de mfacturaproveedor
			sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES("+
				"'"+this.prefijoFactura+"',"+this.numeroFactura+",'"+this.nit+"',"+
				"'"+this.prefijoProveedor+"',"+this.numeroProveedor+",'"+this.fechaFactura+"',"+
				"'"+this.almacen+"','"+this.fechaFactura+"','F','V','"+this.fechaVencimiento+"',null,"+
				this.valorFactura+","+this.valorIva+",0,0,"+retencion+",0,'"+this.observacion+"','"+this.usuario+"',"+
				"'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')");
			//Registro de gastos detallados
			for(i=0;i<dtGastos.Rows.Count;i++){
                sqlStrings.Add("INSERT INTO DFACTURAGASTO VALUES('" + prefijoFactura + "'," + numeroFactura + ",null,'" + dtGastos.Rows[i]["CODIGO"].ToString() + "'," + dtGastos.Rows[i]["ORIGINAL"].ToString() + "," + Convert.ToDouble(dtGastos.Rows[i]["TASA"]).ToString() + ")");
				
                if(items)
				{
                    for (int j = 0; j < dtElementos.Rows.Count; j++)
                    { 
                   //Registro de vehiculos                      
                        if (dtElementos.Rows.Count > dtGastos.Rows.Count)
                        {
                        valoFac = Math.Round(Convert.ToDouble(dtElementos.Rows[j]["VALORFACT"]), 2);
                                                }
                        else if (dtElementos.Rows.Count < dtGastos.Rows.Count)
                        {
                        valoFac = Math.Round(Convert.ToDouble(dtGastos.Rows[i]["ORIGINAL"]), 2);
                        }
                        else if (dtElementos.Rows.Count == dtGastos.Rows.Count)
                        {
                            valoFac = Math.Round(Convert.ToDouble(dtGastos.Rows[i]["ORIGINAL"]), 2);
                        }
						sqlStrings.Add("INSERT INTO "+tabla+" VALUES("+
							"'"+prefijoFactura+"',"+numeroFactura+","+
							dtItems.Rows[j][0].ToString()+","+
                            "'" + dtGastos.Rows[i]["CODIGO"].ToString() + "'," + valoFac + ")");
                    //Math.Round(Convert.ToDouble(dtElementos.Rows[j]["VALORFACT"]), 2) + ")");
                    //Math.Round(Convert.ToDouble(dtGastos.Rows[i]["ORIGINAL"]), 2) + ")"); 
                    }
				}
			}
			if(dtRetenciones!=null)
				if(dtRetenciones.Rows.Count!=0)
					for(i=0;i<dtRetenciones.Rows.Count;i++)
                        sqlStrings.Add("INSERT INTO MFACTURAPROVEEDORRETENCION VALUES('" + this.prefijoFactura + "'," + this.numeroFactura + ",'" + dtRetenciones.Rows[i][0].ToString() + "'," + Convert.ToDouble(dtRetenciones.Rows[i][3]).ToString() + "," + Convert.ToDouble(dtRetenciones.Rows[i][2]).ToString() + ")");
			
			sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+this.prefijoFactura+"'");
			if(DBFunctions.Transaction(sqlStrings))
			{
                contaOnline.contabilizarOnline(this.prefijoFactura.ToString(), Convert.ToInt32(this.numeroFactura.ToString()), Convert.ToDateTime(this.fechaFactura.ToString()), this.usuario.ToString());

				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}

        public bool Devolucion_Factura(bool items)	//Si el booleano es verdadero son items de vehículos sino embarques
        {
            ArrayList sqlStrings = new ArrayList();
            int i;
            double retencion = 0;
            bool status = false;
            string tabla = "";
            if (tipo == "V") tabla = "DFACTURAGASTOVEHICULOANULACION";
            //Retenciones?
            if (dtRetenciones != null)
                if (dtRetenciones.Rows.Count != 0)
                    for (i = 0; i < dtRetenciones.Rows.Count; i++)
                        retencion += Convert.ToDouble(dtRetenciones.Rows[i][4]);
            //Registro de mfacturaproveedor
            sqlStrings.Add("INSERT INTO mfacturaproveedor VALUES(" +
                "'" + this.prefijoFactura + "'," + this.numeroFactura + ",'" + this.nit + "'," +
                "'" + this.prefijoProveedor + "'," + this.numeroProveedor + ",'" + this.fechaFactura + "'," +
                "'" + this.almacen + "','" + this.fechaFactura + "','N','V','" + this.fechaVencimiento + "',null," +
                this.valorFactura + "," + this.valorIva + ",0,0," + retencion + ",0,'" + this.observacion + "','" + this.usuario + "'," +
                "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')");
            //NOTA PROVEEDOR
            sqlStrings.Add("INSERT INTO MNOTAPROVEEDOR VALUES('" + prefijoFactura + "'," + numeroFactura + ", '" + prefijoProveedor + "', " + numeroProveedor + " )");
            //Registro de gastos detallados devuletos
            //for (i = 0; i < dtGastos.Rows.Count; i++)
            //{
                //for (int j = 0; j < dtElementos.Rows.Count; j++)
                //sqlStrings.Add("INSERT INTO DFACTURAGASTOVEHICULOANULACION VALUES('" + prefijoFactura + "'," + numeroFactura + "," + dtItems.Rows[j][0].ToString() + ",'" + dtGastos.Rows[i]["CODIGO"].ToString() + "'," + dtGastos.Rows[i]["VALOR"].ToString() + ")");
                if (items)
                {
                    //Registro de vehiculos
                    for (int j = 0; j < dtElementos.Rows.Count; j++)
                        sqlStrings.Add("INSERT INTO " + tabla + " VALUES(" +
                            "'" + prefijoFactura + "'," + numeroFactura + "," +
                            dtItems.Rows[j][0].ToString() + "," +
                            "'" + dtElementos.Rows[j]["GASTO"] + "'," +
                          Math.Round(Convert.ToDouble(dtElementos.Rows[j]["VALORFACT"]), 2) + ")");
                }
            //}

            if (dtRetenciones != null)
                if (dtRetenciones.Rows.Count != 0)
                    for (i = 0; i < dtRetenciones.Rows.Count; i++)
                        sqlStrings.Add("INSERT INTO MFACTURAPROVEEDORRETENCION VALUES('" + this.prefijoFactura + "'," + this.numeroFactura + ",'" + dtRetenciones.Rows[i][0].ToString() + "'," + Convert.ToDouble(dtRetenciones.Rows[i][4]).ToString() + "," + Convert.ToDouble(dtRetenciones.Rows[i][3]).ToString() + ")");

            if (tipo == "V")
            { 
                sqlStrings.Add("DELETE FROM DFACTURAGASTOVEHICULO WHERE PDOC_CODIORDEPAGO = '" + this.prefijoProveedor + "' AND   MFAC_NUMEORDEPAGO = '" + this.numeroProveedor + "'; ");
            }

            if (tipo == "E")
            {
                sqlStrings.Add("DELETE FROM DFACTURAGASTO WHERE PDOC_CODIORDEPAGO = '" + this.prefijoProveedor + "' AND   MFAC_NUMEORDEPAGO = '" + this.numeroProveedor + "'; ");
            }

            sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='" + this.prefijoFactura + "'");
            if (DBFunctions.Transaction(sqlStrings))
            {
                contaOnline.contabilizarOnline(this.prefijoFactura.ToString(), Convert.ToInt32(this.numeroFactura.ToString()), Convert.ToDateTime(this.fechaFactura.ToString()), this.usuario.ToString());

                status = true;
                processMsg += DBFunctions.exceptions + "<br>";
            }
            else
                processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
            return status;
        }
	}
}
