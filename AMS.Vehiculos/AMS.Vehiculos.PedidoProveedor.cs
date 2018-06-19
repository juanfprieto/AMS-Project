// created on 17/03/2005 at 15:13
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;

namespace AMS.Vehiculos
{
	public class PedidoProveedor
	{
		protected string prefijoPedido, numeroPedido, fechaPedido, nitProveedor, observacion, processMsg;
		protected DataTable elementosPedido;
		
		public string PrefijoPedido{set{prefijoPedido = value;}get{return prefijoPedido;}}
		public string NumeroPedido{set{numeroPedido = value;}get{return numeroPedido;}}
		public string FechaPedido{set{fechaPedido = value;}get{return fechaPedido;}}
		public string NitProveedor{set{nitProveedor = value;}get{return nitProveedor;}}
		public string Observacion{set{observacion = value;}get{return observacion;}}
		public string ProcessMsg{get{return processMsg;}}
		
		public PedidoProveedor()
		{
			elementosPedido = null;
		}
		
		public PedidoProveedor(DataTable elePedi)
		{
			elementosPedido = new DataTable();
			elementosPedido = elePedi;
		}
		
		public bool Grabar_Pedido(string auto)
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			int i;
			string fecha;
			//Primero realizamos el registro en mpedidovehiculoproveedor
			sqlStrings.Add("INSERT INTO mpedidovehiculoproveedor VALUES('"+this.prefijoPedido+"',"+this.numeroPedido+",'"+this.fechaPedido+"','"+this.nitProveedor+"','"+this.observacion+"')");
			//Ahora realizamos el registro de los elementos del pedido en dpedidovehiculoproveedor
			
			for(i=0;i<elementosPedido.Rows.Count;i++)
			{
				if (elementosPedido.Rows[i][5].ToString()==String.Empty)
					fecha="null";
				else 
					fecha="'"+elementosPedido.Rows[i][5].ToString()+"'";
				sqlStrings.Add("INSERT INTO dpedidovehiculoproveedor VALUES('"+this.prefijoPedido+"',"+this.numeroPedido+",'"+elementosPedido.Rows[i][0].ToString()+"','"+elementosPedido.Rows[i][1].ToString()+"','"+elementosPedido.Rows[i][2].ToString()+"',"+elementosPedido.Rows[i][3].ToString()+",0,"+elementosPedido.Rows[i][4].ToString()+","+fecha+")");
			}
				
			if(auto=="A")
				sqlStrings.Add("UPDATE pdocumento SET pdoc_ultidocu=pdoc_ultidocu+1 WHERE pdoc_codigo='"+this.prefijoPedido+"'");
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}
		
		public bool Editar_Pedido()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			int i,j;
			string fecha;
			//Primero debemos realizar la actualizacion de los datos de los datos de mpedidovehiculoproveedor
			sqlStrings.Add("UPDATE mpedidovehiculoproveedor SET mnit_nit='"+this.nitProveedor+"', mped_observacion='"+this.observacion+"' WHERE  pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
			//Ahora debemos traer los elementos existentes de este pedido dpedidovehiculoproveedor
			DataSet elementosPedidoEx = new DataSet();
			DBFunctions.Request(elementosPedidoEx,IncludeSchema.NO,"SELECT pcat_codigo, pcol_codigo FROM dpedidovehiculoproveedor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
			for(i=0;i<elementosPedido.Rows.Count;i++)
			{
				if (elementosPedido.Rows[i][5].ToString()==String.Empty)
					fecha="null";
				else 
					fecha="'"+elementosPedido.Rows[i][5].ToString()+"'";
				bool encontrado = false;
				int posicion = 0;
				for(j=0;j<elementosPedidoEx.Tables[0].Rows.Count;j++)
				{
					if((elementosPedido.Rows[i][0].ToString()==elementosPedidoEx.Tables[0].Rows[j][0].ToString())&&(elementosPedido.Rows[i][1].ToString()==elementosPedidoEx.Tables[0].Rows[j][1].ToString()))
					{
						encontrado = true;
						posicion = j;
						sqlStrings.Add("UPDATE dpedidovehiculoproveedor SET pcol_codigoalte='"+elementosPedido.Rows[i][2].ToString()+"', dped_cantpedi="+elementosPedido.Rows[i][3].ToString()+", dped_valounit="+elementosPedido.Rows[i][4].ToString()+",dped_fechallegada="+fecha+" WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+elementosPedido.Rows[i][0].ToString()+"' AND pcol_codigo='"+elementosPedido.Rows[i][1].ToString()+"'");
					}
				}
				if(encontrado)
					elementosPedidoEx.Tables[0].Rows.RemoveAt(posicion);
				if(!DBFunctions.RecordExist("SELECT * FROM dpedidovehiculoproveedor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+elementosPedido.Rows[i][0].ToString()+"' AND pcol_codigo='"+elementosPedido.Rows[i][1].ToString()+"'"))
					sqlStrings.Add("INSERT INTO dpedidovehiculoproveedor VALUES('"+this.prefijoPedido+"',"+this.numeroPedido+",'"+elementosPedido.Rows[i][0].ToString()+"','"+elementosPedido.Rows[i][1].ToString()+"','"+elementosPedido.Rows[i][2].ToString()+"',"+elementosPedido.Rows[i][3].ToString()+",0,"+elementosPedido.Rows[i][4].ToString()+","+fecha+")");
				
					
			}
			//Ahora debemos eliminar los registros que queden en el DataSet
			for(i=0;i<elementosPedidoEx.Tables[0].Rows.Count;i++)
				sqlStrings.Add("DELETE FROM dpedidovehiculoproveedor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+elementosPedidoEx.Tables[0].Rows[i][0].ToString()+"' AND pcol_codigo='"+elementosPedidoEx.Tables[0].Rows[i][1].ToString()+"'");
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = true;
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
				processMsg += "Error: " + DBFunctions.exceptions + "<br><br>";
			return status;
		}
		
		public bool Eliminar_Pedido()
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			//primero debemos elimar todos los registro que se encuentren en dpedido
			sqlStrings.Add("DELETE FROM dpedidovehiculoproveedor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
			//y luego eliminamos el registro de la tabla 
			sqlStrings.Add("DELETE FROM mpedidovehiculoproveedor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
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

