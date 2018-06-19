using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using AMS.DB;

namespace AMS.Vehiculos
{
	public class PedidoClienteMayor
	{
		protected string prefijoPedido, numeroPedido, fechaPedido, nitCliente, observacion, processMsg, codigoVendedor, numeroPedidoOriginal;
		protected DataTable elementosPedido;
		
		public string PrefijoPedido{set{prefijoPedido = value;}get{return prefijoPedido;}}
		public string NumeroPedido{set{numeroPedido = value;}get{return numeroPedido;}}
		public string FechaPedido{set{fechaPedido = value;}get{return fechaPedido;}}
		public string NitCliente{set{nitCliente = value;}get{return nitCliente;}}
		public string Observacion{set{observacion = value;}get{return observacion;}}
		public string CodigoVendedor{set{codigoVendedor = value;}get{return codigoVendedor;}}
		public string NumeroPedidoOriginal{set{numeroPedidoOriginal = value;}get{return numeroPedidoOriginal;}}
		public string ProcessMsg{get{return processMsg;}}
		
		public PedidoClienteMayor()
		{
			elementosPedido = null;
		}
		
		public PedidoClienteMayor(DataTable elePedi)
		{
			elementosPedido = new DataTable();
			elementosPedido = elePedi;
		}
		
		public bool Grabar_Pedido(string auto)
		{
			ArrayList sqlStrings = new ArrayList();
			bool status = false;
			bool validaM=false;
			int i;
			string fecha;
			int usuario=Convert.ToInt16(DBFunctions.SingleData("select susu_codigo from susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"'"));
			//Primero realizamos el registro en mpedidovehiculoclientemayor
			sqlStrings.Add("INSERT INTO mpedidovehiculoclientemayor VALUES('"+this.prefijoPedido+"',"+this.numeroPedido+",10,'"+this.fechaPedido+"','"+this.nitCliente+"','"+this.observacion+"','"+this.codigoVendedor+"','"+numeroPedidoOriginal+"',"+usuario+",'"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"');");
			//Ahora realizamos el registro de los elementos del pedido en dpedidovehiculoclientemayor
			
			//COMENTAR AUTORIZACION AUTOMATICA
			//double saldoMora=Utilidades.Clientes.ConsultarSaldoMora(nitCliente);
			//double saldo=Utilidades.Clientes.ConsultarSaldo(nitCliente);
			double cupo=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(MCLI_CUPOCRED,0) FROM mcliente WHERE mnit_nit='"+nitCliente+"'"));
			//validaM=(saldo<=cupo && saldoMora<=0);
				
			//Pedir autorizacion para todas
			//validaM=false;
			validaM=(cupo>0);
			
			for(i=0;i<elementosPedido.Rows.Count;i++)
			{
				if (elementosPedido.Rows[i][5].ToString()==String.Empty)
					fecha="null";
				else 
					fecha="'"+elementosPedido.Rows[i][5].ToString()+"'";
				sqlStrings.Add("INSERT INTO dpedidovehiculoclientemayor VALUES('"+this.prefijoPedido+"',"+this.numeroPedido+",'"+elementosPedido.Rows[i][0].ToString()+"','"+elementosPedido.Rows[i][1].ToString()+"','"+elementosPedido.Rows[i][2].ToString()+"',"+elementosPedido.Rows[i][3].ToString()+",0,"+elementosPedido.Rows[i][4].ToString()+","+fecha+",0,0)");
			}

			//Autorizacion pedido
			if(validaM)
				sqlStrings.Add("INSERT INTO MPEDIDOCLIENTEAUTORIZACION VALUES("+
					"'"+prefijoPedido+"',"+numeroPedido+",'"+nitCliente+"','S',"+
					"'"+DateTime.Now.ToString("yyyy-MM-dd")+"',"+
					"'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			else
				sqlStrings.Add("INSERT INTO MPEDIDOCLIENTEAUTORIZACION VALUES("+
					"'"+prefijoPedido+"',"+numeroPedido+",'"+nitCliente+"',NULL,"+
					"'"+DateTime.Now.ToString("yyyy-MM-dd")+"',"+
					"'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
				
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
			//Primero debemos realizar la actualizacion de los datos de los datos de mpedidovehiculoclientemayor
			sqlStrings.Add("UPDATE mpedidovehiculoclientemayor SET mnit_nit='"+this.nitCliente+"', mped_observacion='"+this.observacion+"',pven_codigo='"+this.codigoVendedor+"',mped_numepedi_original='"+numeroPedidoOriginal+"' WHERE  pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
			//Ahora debemos traer los elementos existentes de este pedido dpedidovehiculoclientemayor
			DataSet elementosPedidoEx = new DataSet();
			DBFunctions.Request(elementosPedidoEx,IncludeSchema.NO,"SELECT pcat_codigo, pcol_codigo FROM dpedidovehiculoclientemayor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
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
						sqlStrings.Add("UPDATE dpedidovehiculoclientemayor SET pcol_codigoalte='"+elementosPedido.Rows[i][2].ToString()+"', dped_cantpedi="+elementosPedido.Rows[i][3].ToString()+", dped_valounit="+elementosPedido.Rows[i][4].ToString()+",dped_fechallegada="+fecha+" WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+elementosPedido.Rows[i][0].ToString()+"' AND pcol_codigo='"+elementosPedido.Rows[i][1].ToString()+"'");
					}
				}
				if(encontrado)
					elementosPedidoEx.Tables[0].Rows.RemoveAt(posicion);
				if(!DBFunctions.RecordExist("SELECT * FROM dpedidovehiculoclientemayor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+elementosPedido.Rows[i][0].ToString()+"' AND pcol_codigo='"+elementosPedido.Rows[i][1].ToString()+"'"))
					sqlStrings.Add("INSERT INTO dpedidovehiculoclientemayor VALUES('"+this.prefijoPedido+"',"+this.numeroPedido+",'"+elementosPedido.Rows[i][0].ToString()+"','"+elementosPedido.Rows[i][1].ToString()+"','"+elementosPedido.Rows[i][2].ToString()+"',"+elementosPedido.Rows[i][3].ToString()+",0,"+elementosPedido.Rows[i][4].ToString()+","+fecha+",0,0)");
				
					
			}
			//Ahora debemos eliminar los registros que queden en el DataSet
			for(i=0;i<elementosPedidoEx.Tables[0].Rows.Count;i++)
				sqlStrings.Add("DELETE FROM dpedidovehiculoclientemayor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+" AND pcat_codigo='"+elementosPedidoEx.Tables[0].Rows[i][0].ToString()+"' AND pcol_codigo='"+elementosPedidoEx.Tables[0].Rows[i][1].ToString()+"'");
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
			sqlStrings.Add("DELETE FROM dpedidovehiculoclientemayor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
			//y luego eliminamos el registro de la tabla 
			sqlStrings.Add("DELETE FROM mpedidovehiculoclientemayor WHERE pdoc_codigo='"+this.prefijoPedido+"' AND mped_numepedi="+this.numeroPedido+"");
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


