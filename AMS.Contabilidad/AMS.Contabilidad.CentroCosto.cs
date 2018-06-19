using System;
using System.Collections;
using System.Data;

using AMS.DB;
using AMS.Forms;

namespace AMS.Contabilidad
{
	/// <summary>
	/// Descripción breve de Catalogo.
	/// </summary>
	public class CentroCosto
	{
		private Hashtable PCEN_CODIGO= new Hashtable();
		private Hashtable PCEN_NOMBRE= new Hashtable();
		private Hashtable PCEN_CLASE= new Hashtable();
		private Hashtable Verificar = new Hashtable();

		private int NumeroFila=0;
		private int TotalFilas=0;
		private string pCentro = "";
				
		public CentroCosto()
		{
			DataSet CentroCostosDS = new DataSet();
            string Sql = "select * from PCENTROCOSTO where timp_codigo <> 'N' ";

			DBFunctions.Request(CentroCostosDS,IncludeSchema.NO,Sql);
			TotalFilas = CentroCostosDS.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<CentroCostosDS.Tables[0].Rows.Count;j++)
				{
					Verificar.Add(CentroCostosDS.Tables[0].Rows[j][0].ToString(),CentroCostosDS.Tables[0].Rows[j][0].ToString());
					PCEN_CODIGO.Add(CentroCostosDS.Tables[0].Rows[j][0].ToString(),CentroCostosDS.Tables[0].Rows[j][0].ToString());
					PCEN_NOMBRE.Add(CentroCostosDS.Tables[0].Rows[j][0].ToString(),CentroCostosDS.Tables[0].Rows[j][1].ToString());
					PCEN_CLASE.Add(CentroCostosDS.Tables[0].Rows[j][0].ToString(),CentroCostosDS.Tables[0].Rows[j][2].ToString());
				}
			}
		}

		public string p_AsignarCentro
		{ 
			get
			{
				return pCentro;
			}
			set
			{
				pCentro = value;
			}
		}

		public bool p_Verificar
		{  
			get
			{
				bool retorno = false;
				retorno = Verificar.ContainsKey(pCentro);
				return retorno;
			}
		}

		public int p_TotalFilas{get{return TotalFilas;}}
		public int AsignarFila{set{NumeroFila = value;}get{return NumeroFila;}}
		public string p_PCEN_CODIGO{get{return (string)PCEN_CODIGO[pCentro];}}
		public string p_PCEN_NOMBRE{get{return (string)PCEN_NOMBRE[pCentro];}}
		public string p_PCEN_CLASE{get{return (string)PCEN_CLASE[pCentro];}}
	}
}
