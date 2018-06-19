using System;
using System.Collections;
using System.Data;

using AMS.DB;
using AMS.Forms;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de Acumulados.
	/// </summary>
	public class Acumulados
	{
		private Hashtable MQUI_CODIQUIN = new Hashtable();
		private Hashtable MEMP_CODIEMPL= new Hashtable();  
		private Hashtable PCON_CONCEPTO= new Hashtable(); 
		private Hashtable DQUI_CANTEVENTOS= new Hashtable();  
		private Hashtable DQUI_VALEVENTO= new Hashtable();    
		private Hashtable DQUI_APAGAR= new Hashtable();     
		private Hashtable DQUI_ADESCONTAR= new Hashtable();  
		private Hashtable DQUI_DESCCANTIDAD= new Hashtable();   
		private Hashtable DQUI_SALDO= new Hashtable();      
		private Hashtable DQUI_DOCREFE= new Hashtable();    
		private Hashtable TACCIONC_SECUENCIA= new Hashtable();  

		private Hashtable MQUI_ANOQUIN= new Hashtable();
		private Hashtable MQUI_MESQUIN= new Hashtable();
		private Hashtable MQUI_TPERNOMI= new Hashtable();
		private Hashtable MQUI_ESTADO= new Hashtable();

		private Hashtable PCON_NOMBCONC= new Hashtable(); 
		private Hashtable PCON_FACTORLIQ= new Hashtable();    
		private Hashtable PCON_DESCCANT= new Hashtable();       
		private Hashtable PCON_CLASECONC= new Hashtable();    
		private Hashtable PCON_SIGNOLIQ= new Hashtable();        
		private Hashtable TRES_AFECHORAEXTR= new Hashtable();     
		private Hashtable TRES_AFECPRIMA= new Hashtable();     
		private Hashtable TRES_AFECVACACION= new Hashtable();      
		private Hashtable TRES_AFECCESANTIA= new Hashtable();     
		private Hashtable TRES_AFECRETEFTE= new Hashtable();      
		private Hashtable TRES_AFEC_EPS= new Hashtable();     
		private Hashtable TRES_AFECPROVISION= new Hashtable();      
		private Hashtable TRES_AFECLIQUIDDEFINITIVA= new Hashtable();  
		private Hashtable TRES_ESGRABADO= new Hashtable();     
		private Hashtable PCON_PORC_APF= new Hashtable();
		private Hashtable PCON_PORC_ARP= new Hashtable();
		private Hashtable TRES_AFECINDEMNIZACION= new Hashtable();
		private Hashtable PCON_CERTINGYRET= new Hashtable();

		private int NumeroFila=0;
		private int TotalFilas=0;


		public Acumulados(string Empleado,int AnoAnterior,int AnoCurso,int mes)
		{
			DataSet quincenas = new DataSet();
			string Sql = "Select a.*,b.*,c.*";  
			Sql = Sql + " from dquincena a, mquincenaS b, pconceptonomina c";
			Sql = Sql + " Where a.mqui_codiquin = b.mqui_codiquin AND A.MEMP_CODIEMPL = '" + Empleado + "' and a.pcon_concepto = c.pcon_concepto";
			if (AnoAnterior < AnoCurso)
			{
				Sql = Sql + " AND ((b.mqui_anoquin = " + AnoAnterior.ToString() + " and mqui_mesquin >= " + mes.ToString() + ")";
				Sql = Sql + " OR (b.mqui_anoquin = " + AnoCurso.ToString() + " and mqui_mesquin <=" + mes.ToString() + "))"; 
			}
			else
			{
				Sql = Sql + " AND (b.mqui_anoquin = " + AnoCurso.ToString() + " and mqui_mesquin <=" + mes.ToString() + ")"; 
			}
			Sql = Sql + "  order by b.mqui_anoquin,b.mqui_mesquin";
			
			DBFunctions.Request(quincenas,IncludeSchema.NO,Sql);
			TotalFilas = quincenas.Tables[0].Rows.Count;
			if (TotalFilas > 0)
			{
				for (int j=0;j<quincenas.Tables[0].Rows.Count;j++)
				{
					MQUI_CODIQUIN.Add(j,quincenas.Tables[0].Rows[j][01].ToString());
					MEMP_CODIEMPL.Add(j,quincenas.Tables[0].Rows[j][02].ToString());  
					PCON_CONCEPTO.Add(j,quincenas.Tables[0].Rows[j][03].ToString()); 
					DQUI_CANTEVENTOS.Add(j,quincenas.Tables[0].Rows[j][04].ToString());  
					DQUI_VALEVENTO.Add(j,quincenas.Tables[0].Rows[j][05].ToString());    
					DQUI_APAGAR.Add(j,quincenas.Tables[0].Rows[j][06].ToString());     
					DQUI_ADESCONTAR.Add(j,quincenas.Tables[0].Rows[j][07].ToString());  
					DQUI_DESCCANTIDAD.Add(j,quincenas.Tables[0].Rows[j][08].ToString());   
					DQUI_SALDO.Add(j,quincenas.Tables[0].Rows[j][09].ToString());      
					DQUI_DOCREFE.Add(j,quincenas.Tables[0].Rows[j][10].ToString());    
					TACCIONC_SECUENCIA.Add(j,quincenas.Tables[0].Rows[j][11].ToString());  

					MQUI_ANOQUIN.Add(j,quincenas.Tables[0].Rows[j][13].ToString());
					MQUI_MESQUIN.Add(j,quincenas.Tables[0].Rows[j][14].ToString());
					MQUI_TPERNOMI.Add(j,quincenas.Tables[0].Rows[j][15].ToString());
					MQUI_ESTADO.Add(j,quincenas.Tables[0].Rows[j][16].ToString());

					PCON_NOMBCONC.Add(j,quincenas.Tables[0].Rows[j][18].ToString()); 
					PCON_FACTORLIQ.Add(j,quincenas.Tables[0].Rows[j][19].ToString());    
					PCON_DESCCANT.Add(j,quincenas.Tables[0].Rows[j][20].ToString());       
					PCON_CLASECONC.Add(j,quincenas.Tables[0].Rows[j][21].ToString());    
					PCON_SIGNOLIQ.Add(j,quincenas.Tables[0].Rows[j][22].ToString());        
					TRES_AFECHORAEXTR.Add(j,quincenas.Tables[0].Rows[j][23].ToString());     
					TRES_AFECPRIMA.Add(j,quincenas.Tables[0].Rows[j][24].ToString());     
					TRES_AFECVACACION.Add(j,quincenas.Tables[0].Rows[j][25].ToString());      
					TRES_AFECCESANTIA.Add(j,quincenas.Tables[0].Rows[j][26].ToString());     
					TRES_AFECRETEFTE.Add(j,quincenas.Tables[0].Rows[j][27].ToString());      
					TRES_AFEC_EPS.Add(j,quincenas.Tables[0].Rows[j][28].ToString());     
					TRES_AFECPROVISION.Add(j,quincenas.Tables[0].Rows[j][29].ToString());      
					TRES_AFECLIQUIDDEFINITIVA.Add(j,quincenas.Tables[0].Rows[j][30].ToString());  
					TRES_ESGRABADO.Add(j,quincenas.Tables[0].Rows[j][31].ToString());     
					PCON_PORC_APF.Add(j,quincenas.Tables[0].Rows[j][32].ToString());
					PCON_PORC_ARP.Add(j,quincenas.Tables[0].Rows[j][33].ToString());
					TRES_AFECINDEMNIZACION.Add(j,quincenas.Tables[0].Rows[j][34].ToString());
					PCON_CERTINGYRET.Add(j,quincenas.Tables[0].Rows[j][35].ToString());
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
		
		public string p_MQUI_CODIQUIN
		{
			get
			{
				return (string)MQUI_CODIQUIN[NumeroFila];
			}
		}

		public string p_MEMP_CODIEMPL  
		{
			get
			{
				return (string)MEMP_CODIEMPL[NumeroFila];
			}
		}

		public string p_PCON_CONCEPTO 
		{
			get
			{
				return (string)PCON_CONCEPTO[NumeroFila];
			}
		}

		public string p_DQUI_CANTEVENTOS  
		{
			get
			{
				return (string)DQUI_CANTEVENTOS[NumeroFila];
			}
		}

		public string p_DQUI_VALEVENTO    
		{
			get
			{
				return (string)DQUI_VALEVENTO[NumeroFila];
			}
		}

		public string p_DQUI_APAGAR     
		{
			get
			{
				return (string)DQUI_APAGAR[NumeroFila];
			}
		}

		public string p_DQUI_ADESCONTAR  
		{
			get
			{
				return (string)DQUI_ADESCONTAR[NumeroFila];
			}
		}

		public string p_DQUI_DESCCANTIDAD   
		{
			get
			{
				return (string)DQUI_DESCCANTIDAD[NumeroFila];
			}
		}

		public string p_DQUI_SALDO      		
		{
			get
			{
				return (string)DQUI_SALDO[NumeroFila];
			}
		}

		public string p_DQUI_DOCREFE    		
		{
			get
			{
				return (string)DQUI_DOCREFE[NumeroFila];
			}
		}

		public string p_TACCIONC_SECUENCIA  
		{
			get
			{
				return (string)TACCIONC_SECUENCIA[NumeroFila];
			}
		}

		public string p_MQUI_ANOQUIN
		{
			get
			{
				return (string)MQUI_ANOQUIN[NumeroFila];
			}
		}

		public string p_MQUI_MESQUIN
		{
			get
			{
				return (string)MQUI_MESQUIN[NumeroFila];
			}
		}

		public string p_MQUI_TPERNOMI
		{
			get
			{
				return (string)MQUI_TPERNOMI[NumeroFila];
			}
		}

		public string p_MQUI_ESTADO
		{
			get
			{
				return (string)MQUI_ESTADO[NumeroFila];
			}
		}

		public string p_PCON_NOMBCONC
		{
			get
			{
				return (string)PCON_NOMBCONC[NumeroFila];
			}
		}

		public string p_PCON_FACTORLIQ    
		{
			get
			{
				return (string)PCON_FACTORLIQ [NumeroFila];
			}
		}

		public string p_PCON_DESCCANT       
		{
			get
			{
				return (string)PCON_DESCCANT[NumeroFila];
			}
		}

		public string p_PCON_CLASECONC    
		{
			get
			{
				return (string)PCON_CLASECONC[NumeroFila];
			}
		}

		public string p_PCON_SIGNOLIQ        
		{
			get
			{
				return (string)PCON_SIGNOLIQ[NumeroFila];
			}
		}

		public string p_TRES_AFECHORAEXTR     
		{
			get
			{
				return (string)TRES_AFECHORAEXTR[NumeroFila];
			}
		}

		public string p_TRES_AFECPRIMA     
		{
			get
			{
				return (string)TRES_AFECPRIMA[NumeroFila];
			}
		}

		public string p_TRES_AFECVACACION      
		{
			get
			{
				return (string)TRES_AFECVACACION[NumeroFila];
			}
		}

		public string p_TRES_AFECCESANTIA     
		{
			get
			{
				return (string)TRES_AFECCESANTIA[NumeroFila];
			}
		}

		public string p_TRES_AFECRETEFTE      
		{
			get
			{
				return (string)TRES_AFECRETEFTE[NumeroFila];
			}
		}

		public string p_TRES_AFEC_EPS     
		{
			get
			{
				return (string)TRES_AFEC_EPS[NumeroFila];
			}
		}

		public string p_TRES_AFECPROVISION      
		{
			get
			{
				return (string)TRES_AFECPROVISION[NumeroFila];
			}
		}

		public string p_TRES_AFECLIQUIDDEFINITIVA  
		{
			get
			{
				return (string)TRES_AFECLIQUIDDEFINITIVA[NumeroFila];
			}
		}

		public string p_TRES_ESGRABADO     
		{
			get
			{
				return (string)TRES_ESGRABADO[NumeroFila];
			}
		}

		public string p_PCON_PORC_APF
		{
			get
			{
				return (string)PCON_PORC_APF[NumeroFila];
			}
		}

		public string p_PCON_PORC_ARP
		{
			get
			{
				return (string)PCON_PORC_ARP[NumeroFila];
			}
		}

		public string p_TRES_AFECINDEMNIZACION
		{
			get
			{
				return (string)TRES_AFECINDEMNIZACION[NumeroFila];
			}
		}

		public string p_PCON_CERTINGYRET
		{
			get
			{
				return (string)PCON_CERTINGYRET[NumeroFila];
			}
		}
	}
}
