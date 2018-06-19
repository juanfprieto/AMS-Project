using System;
using System.Collections;
using System.Data;

using AMS.DB;
using AMS.Forms;

namespace AMS.Nomina
{
	public class Novedades
	{
		private Hashtable pcon_concepto= new Hashtable();
		private Hashtable mnov_valrtotl= new Hashtable();
		private Hashtable memp_codiempl= new Hashtable();
		private Hashtable pcon_signoliq= new Hashtable();
		private Hashtable mnov_cantidad= new Hashtable();
		private Hashtable pcon_desccant= new Hashtable();
		private Hashtable pcon_factorliq= new Hashtable();
		private Hashtable tdes_descripcion= new Hashtable();
		private Hashtable tres_afec_eps= new Hashtable();
		private Hashtable tres_afechoraextr= new Hashtable();
		private Hashtable pcon_nombconc= new Hashtable();

		private int NumeroFila=0;
		private int TotalFilas=0;
		
		//JFSC 07052008 Constructor por defecto
		public Novedades()
		{
		
		}

		public Novedades(string Empleado,string FechaInicio,string FechaFinal)
		{
			DataSet novedades = new DataSet();
			string Sql = "select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.pcon_nombconc";
			Sql = Sql + " from mnovedadesnomina M,pconceptonomina P,tdesccantidad T";
			Sql = Sql + " where M.pcon_concepto=P.pcon_concepto and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='" + Empleado + "'"; 
			Sql = Sql + " and (M.mnov_fecha between '" + FechaInicio + "' and '" + FechaFinal + "')";
			
			DBFunctions.Request(novedades,IncludeSchema.NO,Sql);
			TotalFilas = novedades.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<novedades.Tables[0].Rows.Count;j++)
				{
					pcon_concepto.Add(j,novedades.Tables[0].Rows[j][0].ToString());
					mnov_valrtotl.Add(j,novedades.Tables[0].Rows[j][1].ToString());
					memp_codiempl.Add(j,novedades.Tables[0].Rows[j][2].ToString());
					pcon_signoliq.Add(j,novedades.Tables[0].Rows[j][3].ToString());
					mnov_cantidad.Add(j,novedades.Tables[0].Rows[j][4].ToString());
					pcon_desccant.Add(j,novedades.Tables[0].Rows[j][5].ToString());
					pcon_factorliq.Add(j,novedades.Tables[0].Rows[j][6].ToString());
					tdes_descripcion.Add(j,novedades.Tables[0].Rows[j][7].ToString());
					tres_afec_eps.Add(j,novedades.Tables[0].Rows[j][8].ToString());
					tres_afechoraextr.Add(j,novedades.Tables[0].Rows[j][9].ToString());
					pcon_nombconc.Add(j,novedades.Tables[0].Rows[j][10].ToString());
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

		public string p_mnov_valrtotl
		{
			get
			{
				return (string)mnov_valrtotl[NumeroFila];
			}
		}

		public string p_pcon_signoliq
		{
			get
			{
				return (string)pcon_signoliq[NumeroFila];
			}
		}

		public string p_memp_codiempl
		{
			get
			{
				return (string)memp_codiempl[NumeroFila];
			}
		}

		public string p_mnov_cantidad
		{
			get
			{
				return (string)mnov_cantidad[NumeroFila];
			}
		}

		public string p_pcon_desccant
		{
			get
			{
				return (string)pcon_desccant[NumeroFila];
			}
		}

		public string p_tdes_descripcion
		{
			get
			{
				return (string)tdes_descripcion[NumeroFila];
			}
		}

		public string p_pcon_factorliq
		{
			get
			{
				return (string)pcon_factorliq[NumeroFila];
			}
		}

		public string p_tres_afec_eps
		{
			get
			{
				return (string)tres_afec_eps[NumeroFila];
			}
		}

		public string p_tres_afechoraextr
		{
			get
			{
				return (string)tres_afechoraextr[NumeroFila];
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
