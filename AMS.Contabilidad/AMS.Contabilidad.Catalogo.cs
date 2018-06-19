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
	public class Catalogo
	{
		private Hashtable MCUE_CODIPUC= new Hashtable();
		private Hashtable MCUE_NOMBRE= new Hashtable();
		private Hashtable TNIV_CODIGO= new Hashtable();
		private Hashtable TIMP_CODIGO= new Hashtable();
		private Hashtable TCIE_CODIGO= new Hashtable();
		private Hashtable TCUE_CODIGO= new Hashtable();
		private Hashtable TBAS_CODIGO= new Hashtable();
		private Hashtable MCUE_BASEGRAV= new Hashtable();
        private Hashtable TVIG_CODIGO= new Hashtable();
		private Hashtable TNAT_CODIGO= new Hashtable();
		private Hashtable TCLA_CODIGO= new Hashtable();
		private Hashtable MCUE_ORDEPRES= new Hashtable();
		private Hashtable Verificar= new Hashtable();

		private int NumeroFila=0;
		private int TotalFilas=0;
		private string cuenta = "";

		public Catalogo()
		{
			DataSet catalogopuc = new DataSet();
			string Sql = "select * from mcuenta where timp_codigo != 'N'";

			DBFunctions.Request(catalogopuc,IncludeSchema.NO,Sql);
			TotalFilas = catalogopuc.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<catalogopuc.Tables[0].Rows.Count;j++)
				{
					Verificar.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][0].ToString());
					MCUE_CODIPUC.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][0].ToString());
					MCUE_NOMBRE.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][1].ToString());
					TNIV_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][2].ToString());
					TIMP_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][3].ToString());
					TCIE_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][4].ToString());
					TCUE_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][5].ToString());
					TBAS_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][6].ToString());
					MCUE_BASEGRAV.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][7].ToString());
					TVIG_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][8].ToString());
					TNAT_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][9].ToString());
					TCLA_CODIGO.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][10].ToString());
					MCUE_ORDEPRES.Add(catalogopuc.Tables[0].Rows[j][0].ToString(),catalogopuc.Tables[0].Rows[j][11].ToString());
				}
			}
		}
		public int p_TotalFilas{get{return TotalFilas;}}
		public int AsignarFila{set{NumeroFila = value;}get{return NumeroFila;}}

		public string p_AsignarCuenta
		{ 
			get
			{
				return cuenta;
			}
			set
			{
				cuenta = value;
			}
		}
		
		public bool p_Verificar
		{  
		   get
		   {
		      bool retorno = false;
		      retorno = Verificar.ContainsKey(cuenta);
		      return retorno;
		   }
		}

		public string p_MCUE_CODIPUC{get{return (string)MCUE_CODIPUC[cuenta];}}
		public string p_MCUE_NOMBRE{get{return (string)MCUE_NOMBRE[cuenta];}}
		public string p_TNIV_CODIGO{get{return (string)TNIV_CODIGO[cuenta];}}
		public string p_TIMP_CODIGO{get{return (string)TIMP_CODIGO[cuenta];}}
		public string p_TCIE_CODIGO{get{return (string)TCIE_CODIGO[cuenta];}}
		public string p_TCUE_CODIGO{get{return (string)TCUE_CODIGO[cuenta];}}
		public string p_TBAS_CODIGO{get{return (string)TBAS_CODIGO[cuenta];}}
		public string p_MCUE_BASEGRAV{get{return (string)MCUE_BASEGRAV[cuenta];}}
		public string p_TVIG_CODIGO{get{return (string)TVIG_CODIGO[cuenta];}}
		public string p_TNAT_CODIGO{get{return (string)TNAT_CODIGO[cuenta];}}
		public string p_TCLA_CODIGO{get{return (string)TCLA_CODIGO[cuenta];}}
		public string p_MCUE_ORDEPRES{get{return (string)MCUE_ORDEPRES[cuenta];}}

	}
}
