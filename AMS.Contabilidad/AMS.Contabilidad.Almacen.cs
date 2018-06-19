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
	public class Almacen
	{
      	private Hashtable PALM_ALMACEN= new Hashtable();
		private Hashtable PALM_DESCRIPCION= new Hashtable();
		private Hashtable PALM_DIRECCION= new Hashtable();
		private Hashtable PCIU_CODIGO= new Hashtable();
		private Hashtable PALM_TELEFONO1= new Hashtable();
		private Hashtable PALM_TELEFONO2= new Hashtable();
		private Hashtable TALM_TIPOALMA= new Hashtable();
		private Hashtable PCEN_CENTINV= new Hashtable();
		private Hashtable PCEN_CENTTAL= new Hashtable();
		private Hashtable PCEN_CENTVVN= new Hashtable();
		private Hashtable PCEN_CENTVVU= new Hashtable();
		private Hashtable PCEN_CENTCART= new Hashtable();
		private Hashtable PCEN_CENTTESO= new Hashtable();
		private Hashtable PCEN_CENTADM= new Hashtable();
		private Hashtable Verificar = new Hashtable();

		private int NumeroFila=0;
		private int TotalFilas=0;
		private string pAlmacen = "";

		public Almacen()
		{
			DataSet AlmacenDS = new DataSet();
			string Sql = "select * from palmacen";

			DBFunctions.Request(AlmacenDS,IncludeSchema.NO,Sql);
			TotalFilas = AlmacenDS.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<AlmacenDS.Tables[0].Rows.Count;j++)
				{
					Verificar.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][0].ToString());
					PALM_ALMACEN.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][0].ToString());
					PALM_DESCRIPCION.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][1].ToString());
					PALM_DIRECCION.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][2].ToString());
					PCIU_CODIGO.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][3].ToString());
					PALM_TELEFONO1.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][4].ToString());
					PALM_TELEFONO2.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][5].ToString());
					TALM_TIPOALMA.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][6].ToString());
					PCEN_CENTINV.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][7].ToString());
					PCEN_CENTTAL.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][8].ToString());
					PCEN_CENTVVN.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][9].ToString());
					PCEN_CENTVVU.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][10].ToString());
					PCEN_CENTCART.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][11].ToString());
					PCEN_CENTTESO.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][12].ToString());
					PCEN_CENTADM.Add(AlmacenDS.Tables[0].Rows[j][0].ToString(),AlmacenDS.Tables[0].Rows[j][13].ToString());
				}
			}
		}

		public string p_AsignarAlmacen
		{ 
			get
			{
				return pAlmacen;
			}
			set
			{
				pAlmacen = value;
			}
		}

		public bool p_Verificar
		{  
			get
			{
				bool retorno = false;
				retorno = Verificar.ContainsKey(pAlmacen);
				return retorno;
			}
		}

		public int p_TotalFilas{get{return TotalFilas;}}
		public int AsignarFila{set{NumeroFila = value;}get{return NumeroFila;}}
		public string p_PALM_ALMACEN{get{return (string)PALM_ALMACEN[pAlmacen];}}
		public string p_PALM_DESCRIPCION{get{return (string)PALM_DESCRIPCION[pAlmacen];}}
		public string p_PALM_DIRECCION{get{return (string)PALM_DIRECCION[pAlmacen];}}
		public string p_PCIU_CODIGO{get{return (string)PCIU_CODIGO[pAlmacen];}}
		public string p_PALM_TELEFONO1{get{return (string)PALM_TELEFONO1[pAlmacen];}}
		public string p_PALM_TELEFONO2{get{return (string)PALM_TELEFONO2[pAlmacen];}}
		public string p_TALM_TIPOALMA{get{return (string)TALM_TIPOALMA[pAlmacen];}}
		public string p_PCEN_CENTINV{get{return (string)PCEN_CENTINV[pAlmacen];}}
		public string p_PCEN_CENTTAL{get{return (string)PCEN_CENTTAL[pAlmacen];}}
		public string p_PCEN_CENTVVN{get{return (string)PCEN_CENTVVN[pAlmacen];}}
		public string p_PCEN_CENTVVU{get{return (string)PCEN_CENTVVU[pAlmacen];}}
		public string p_PCEN_CENTCART{get{return (string)PCEN_CENTCART[pAlmacen];}}
		public string p_PCEN_CENTTESO{get{return (string)PCEN_CENTTESO[pAlmacen];}}
		public string p_PCEN_CENTADM{get{return (string)PCEN_CENTADM[pAlmacen];}}
	}
}
