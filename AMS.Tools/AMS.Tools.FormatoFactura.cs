using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using AMS.DB;

namespace AMS.Tools
{
	public class FormatoFactura
	{
		private string idFormato, tipoFactura, descripcionFormato, posIDX, posIDY, posFDX, posFDY;
		private string processMsg;
		private DataTable detalleFormato;
		
		public string IdFormato{get{return idFormato;}}
		public string TipoFactura{set{tipoFactura = value;}get{return tipoFactura;}}
		public string DescripcionFormato{set{descripcionFormato = value;}get{return descripcionFormato;}}
		public string PosIDX{set{posIDX = value;}get{return posIDX;}}
		public string PosIDY{set{posIDY = value;}get{return posIDY;}}
		public string PosFDX{set{posFDX = value;}get{return posFDX;}}
		public string PosFDY{set{posFDY = value;}get{return posFDY;}}
		public string ProcessMsg{set{processMsg = value;}get{return processMsg;}}
		public DataTable DetalleFormato{set{detalleFormato = value;}get{return detalleFormato;}}
		
		public FormatoFactura()
		{
			detalleFormato = new DataTable();
		}
		
		public FormatoFactura(DataTable dtfmt)
		{
			detalleFormato = new DataTable();
			detalleFormato = dtfmt;
		}
		
		public int Grabar_Formato()
		{
			ArrayList sqlStrings = new ArrayList();
			int status = -1;
			int i;
			int ultimoFormato = 0;
			try{ultimoFormato = System.Convert.ToInt32(DBFunctions.SingleData("SELECT MAX(sfpf_codigo) FROM sformatopreimpresofactura"));}
			catch{}
			//Primero creamos el registro de la tabla sformatopreimpresofactura
			sqlStrings.Add("INSERT INTO sformatopreimpresofactura VALUES("+(ultimoFormato+1).ToString()+",'"+this.tipoFactura+"','"+this.descripcionFormato+"',"+this.posIDX+","+this.posIDY+","+this.posFDX+","+this.posFDY+")");
			//Ahora ingresamos los detalles del formato
			for(i=0;i<detalleFormato.Rows.Count;i++)
			{
				string insert = "INSERT INTO sdetalleformatopreimpfact VALUES("+(ultimoFormato+1).ToString()+",'"+detalleFormato.Rows[i][0].ToString()+"','"+detalleFormato.Rows[i][1].ToString()+"','"+detalleFormato.Rows[i][9].ToString()+"','"+detalleFormato.Rows[i][2].ToString()+"',";
				if(detalleFormato.Rows[i][3].ToString()=="")
					insert += "null,";
				else
					insert += detalleFormato.Rows[i][3].ToString()+",";
				if(detalleFormato.Rows[i][4].ToString()=="")
					insert += "null,";
				else
					insert += detalleFormato.Rows[i][4].ToString()+",";
				insert += "'"+detalleFormato.Rows[i][5].ToString()+"','"+detalleFormato.Rows[i][10].ToString()+"',";
				if(detalleFormato.Rows[i][6].ToString()=="")
					insert += "null,";
				else
					insert += detalleFormato.Rows[i][6].ToString()+",";
				if(detalleFormato.Rows[i][8].ToString()=="RS")
					insert += "'S',";
				else
					insert += "'N',";
				if(detalleFormato.Rows[i][7].ToString()=="")
					insert += "null,";
				else
					insert += detalleFormato.Rows[i][7].ToString()+",";
				insert += "'"+detalleFormato.Rows[i][11]+"')";
				sqlStrings.Add(insert);
			}
			if(DBFunctions.Transaction(sqlStrings))
			{
				status = ultimoFormato+1;
				this.idFormato = (ultimoFormato+1).ToString();
				processMsg += DBFunctions.exceptions + "<br>";
			}
			else
			{
				processMsg += DBFunctions.exceptions + "<br>";
			}
			return status;
		}
	}
}
