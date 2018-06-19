using System;
using System.Collections;
using System.Data;

using AMS.DB;
using AMS.Forms;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de Licencias.
	/// </summary>
	public class Licencias
	{
		private Hashtable pcon_concepto= new Hashtable();
		private Hashtable memp_codiempl= new Hashtable();
		private Hashtable msus_desde= new Hashtable();
		private Hashtable msus_hasta= new Hashtable();
		private Hashtable totaldias= new Hashtable();
		private Hashtable ttip_coditipo= new Hashtable();
		private Hashtable pcon_signoliq= new Hashtable();
		private Hashtable tdes_descripcion= new Hashtable();
		private Hashtable tres_afec_eps= new Hashtable();
		private Hashtable pcon_nombconc= new Hashtable();

		private int NumeroFila=0;
		private int TotalFilas=0;

		public Licencias(string Empleado,string AnoIni,string MesIni,string AnoFin,string MesFin)
		{
			DataSet novedades = new DataSet();
			int diaFin = 30;
            int diaIni = 01;
            string peripago = DBFunctions.SingleData("select CNOM_OPCIQUINOMENS FROM CNOMINA");
            string quincena = DBFunctions.SingleData("select CNOM_QUINCENA FROM CNOMINA");
            if (peripago == "1" && quincena == "2")
                diaIni = 16;
			if(Int32.Parse(MesFin)==2) diaFin=28;
			string Sql = "select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,(M.msus_hasta-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion,p.tres_afec_eps,pcon_nombconc";
			Sql = Sql + " from msusplicencias M,pconceptonomina P,tdesccantidad T";
			Sql = Sql + " where M.memp_codiempl='" + Empleado + "'and m.ttip_coditipo in (1,4) and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad)";
			Sql = Sql + " and (M.msus_desde between '" + AnoIni + "-" + MesIni + "-" +diaIni+ "' and '" + AnoFin + "-" + MesFin + "-"+diaFin+"')";

			DBFunctions.Request(novedades,IncludeSchema.NO,Sql);
			TotalFilas = novedades.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<novedades.Tables[0].Rows.Count;j++)
				{
					pcon_concepto.Add(j,novedades.Tables[0].Rows[j][0].ToString());
					memp_codiempl.Add(j,novedades.Tables[0].Rows[j][1].ToString());
					msus_desde.Add(j,novedades.Tables[0].Rows[j][2].ToString());
					msus_hasta.Add(j,novedades.Tables[0].Rows[j][3].ToString());
					totaldias.Add(j,novedades.Tables[0].Rows[j][4].ToString());
					ttip_coditipo.Add(j,novedades.Tables[0].Rows[j][5].ToString());
					pcon_signoliq.Add(j,novedades.Tables[0].Rows[j][6].ToString());
					tdes_descripcion.Add(j,novedades.Tables[0].Rows[j][7].ToString());
					tres_afec_eps.Add(j,novedades.Tables[0].Rows[j][8].ToString());
					pcon_nombconc.Add(j,novedades.Tables[0].Rows[j][9].ToString());
				}
			}
		}

		public int p_TotalFilas
		{
			get
			{
				return TotalFilas;
			}
		}

		public int AsignarFila
		{
			set
			{
				NumeroFila = value;
			}
			get
			{
				return NumeroFila;
			}
		}
	
		public string p_pcon_concepto
		{
			get
			{
				return (string)pcon_concepto[NumeroFila];
			}
		}

		public string p_memp_codiempl
		{
			get
			{
				return (string)memp_codiempl[NumeroFila];
			}
		}

		public string p_msus_desde
		{
			get
			{
				return (string)msus_desde[NumeroFila];
			}
		}

		public string p_msus_hasta
		{
			get
			{
				return (string)msus_hasta[NumeroFila];
			}
		}

		public string p_totaldias
		{
			get
			{
				return (string)totaldias[NumeroFila];
			}
		}

		public string p_ttip_coditipo
		{
			get
			{
				return (string)ttip_coditipo[NumeroFila];
			}
		}

		public string p_pcon_signoliq
		{
			get
			{
				return (string)pcon_signoliq[NumeroFila];
			}
		}

		public string p_tdes_descripcion
		{
			get
			{
				return (string)tdes_descripcion[NumeroFila];
			}
		}

		public string p_tres_afec_eps
		{
			get
			{
				return (string)tres_afec_eps[NumeroFila];
			}
		}

		public string p_pcon_nombconc
		{
			get
			{
				return (string)pcon_nombconc[NumeroFila];
			}
		}

	}
}
