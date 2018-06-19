using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web.SessionState;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using AMS.Nomina;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.Mail;


namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de Empleado.
	/// </summary>
	public class Empleado
	{
		private Hashtable MEMP_CODIEMPL = new Hashtable();  
		private Hashtable MNIT_NIT = new Hashtable();     
		private Hashtable MEMP_FECHNACI = new Hashtable();      
		private Hashtable PCIU_LUGANACI = new Hashtable();         
		private Hashtable PALM_ALMACEN = new Hashtable();          
		private Hashtable PDEP_CODIDPTO = new Hashtable();         
		private Hashtable TEST_ESTADO = new Hashtable();           
		private Hashtable TSAL_SALARIO = new Hashtable();         
		private Hashtable TCON_CONTRATO = new Hashtable();           
		private Hashtable PCAR_CODICARGO = new Hashtable();          
		private Hashtable MEMP_FECHINGRESO = new Hashtable();        
		private Hashtable MEMP_FECHFINCONTRATO = new Hashtable();   
		private Hashtable MEMP_FECHRETIRO = new Hashtable();        
		private Hashtable MEMP_UNFAMILIAR = new Hashtable();         
		private Hashtable PESP_CODIGO = new Hashtable();             
		private Hashtable TRES_VIVIENDA = new Hashtable();           
		private Hashtable MEMP_TARJPROFES = new Hashtable();        
		private Hashtable MEMP_DISTLIBRMILI = new Hashtable();       
		private Hashtable MEMP_CLASELIBRMILI = new Hashtable();     
		private Hashtable MEMP_NUMELIBRMILI = new Hashtable();       
		private Hashtable TSEX_CODIGO = new Hashtable();            
		private Hashtable TEST_ESTACIVIL = new Hashtable();         
		private Hashtable MEMP_NUMEHIJOS = new Hashtable();          
		private Hashtable MEMP_PERSCARGO = new Hashtable();         
		private Hashtable MEMP_SUELACTU = new Hashtable();           
		private Hashtable MEMP_FECHSUELACTU = new Hashtable();       
		private Hashtable MEMP_SUELANTER = new Hashtable();          
		private Hashtable MEMP_FECSUELANTER = new Hashtable();       
		private Hashtable MEMP_SALAPROMEDIO = new Hashtable();       
		private Hashtable TSUB_CODIGO = new Hashtable();             
		private Hashtable PEPS_CODIEPS = new Hashtable();            
		private Hashtable MEMP_NUMEAFILEPS = new Hashtable();        
		private Hashtable PFON_CODIPENS = new Hashtable();           
		private Hashtable MEMP_NUMECONTFONDOPENS = new Hashtable();  
		private Hashtable PFON_CODIPENSVOLU = new Hashtable();       
		private Hashtable PFON_CODICESA = new Hashtable();           
		private Hashtable MEMP_NUMECONTFONDOCESA = new Hashtable();  
		private Hashtable PARP_CODIARP = new Hashtable();            
		private Hashtable MEMP_NUMECONTARP = new Hashtable();        
		private Hashtable PRIE_CODIRIES = new Hashtable();           
		private Hashtable TRES_DOTACION = new Hashtable();          
		private Hashtable MEMP_TARJRELOJ = new Hashtable();          
		private Hashtable MEMP_NUMPASE = new Hashtable();            
		private Hashtable MEMP_CATEGORIA = new Hashtable();         
		private Hashtable MEMP_PERIPAGO = new Hashtable();           
		private Hashtable MEMP_AJUSUELDO = new Hashtable();          
		private Hashtable MEMP_FORMPAGO = new Hashtable();           
		private Hashtable PBAN_CODIGO = new Hashtable();             
		private Hashtable MEMP_CODSUCUEMPL = new Hashtable();        
		private Hashtable MEMP_CUENNOMI = new Hashtable();           
		private Hashtable MEMP_TESTRETE = new Hashtable();           
		private Hashtable MEMP_VREXCESALUD = new Hashtable();        
		private Hashtable MEMP_VREXCEEDUC = new Hashtable();         
		private Hashtable MEMP_VREXCEAFC = new Hashtable();         
		private Hashtable MEMP_PORCRETE = new Hashtable();           
		private Hashtable MEMP_VREXCEVIVI = new Hashtable();        
		private Hashtable MEMP_INDCOMIS = new Hashtable();          
		private Hashtable TTIP_SECUENCIA = new Hashtable();
        private Hashtable PCAJ_CODICAJA  = new Hashtable();
        // de Mnits
        private Hashtable MNIT_NOMBRES = new Hashtable();     
		private Hashtable MNIT_NOMBRE2 = new Hashtable();    
		private Hashtable MNIT_APELLIDOS = new Hashtable();    
		private Hashtable MNIT_APELLIDO2 = new Hashtable();   
		private Hashtable MNIT_DIRECCION = new Hashtable();     
		private Hashtable PCIU_CODIGO = new Hashtable();        
		private Hashtable MNIT_TELEFONO = new Hashtable();      
		private Hashtable MNIT_CELULAR = new Hashtable();       
		private Hashtable MNIT_EMAIL = new Hashtable();         

		// de pciudad
		private Hashtable PCIU_NOMBRE = new Hashtable();     

		// de pCARGOEMPLEADO
		private Hashtable PCAR_NOMBCARG = new Hashtable();
		// de pDEPARTAMENTOEMPRESA
		private Hashtable PDEP_NOMBDPTO = new Hashtable();
		
		private string fechainicio;//ver como volver a fechas exactas
		private string fechafinal;
		private string fecharetiro; 
		private string NominaPorceso="";
		private int    PeriodoProceso = 0;
		private bool   liquidoCesantias, esPeriodoLiquidado,noTieneConceptos;
		private DataSet suspensiones;

		private System.Web.UI.WebControls.DropDownList DDLDIARETI;
		private System.Web.UI.WebControls.DropDownList DDLMESRETI;
		private System.Web.UI.WebControls.DropDownList DDLANORETI;

		private double promediocesantia=0;
		private double promedioprima=0;
		private double promediovacaciones=0;
		private int    factorprima=0;
		private int    factorcesantias= 0;
		private int    factorvacaciones = 0;
		private double devengadosmes = 0;
		private double deduccionesmes = 0;
		private System.Web.UI.WebControls.DataGrid DG;
		private DataTable DataTable1;
		private CNomina o_CNomina;
		private Varios o_Varios;
		private int    Licencias = 0;
		private double TotalPagadoSubtr = 0;
		private bool   AgregarAFormulario = true;
		private double Totaleps = 0;
		private bool   NominaMensual = false;

		private double BasePrima = 0;
		private double BaseCesantias = 0;
		private double BaseVacaciones = 0;

		private double VPrima = 0;
		private double VCesantias = 0;
		private double VVacaciones = 0;
		private double VInteCesantias = 0;
		private double VIndemnizacion = 0;
		private string docurefe;
		private string tipoEvento;

		private	TimeSpan resumendias = new TimeSpan();

		private int    NumeroFila=0;
		private int    TotalFilas=0;
		private string NomEmp = "";
		private bool   agregarnombre = true;
		
		public Empleado(string CedulaEmpleado)
		{
			this.suspensiones=new DataSet();
			this.liquidoCesantias=false;
			this.esPeriodoLiquidado=false;
			this.noTieneConceptos=false;
			DataSet empleado = new DataSet();
            string Sql = @"Select a.*,b.MNIT_NOMBRES,b.MNIT_NOMBRE2,b.MNIT_APELLIDOS,b.MNIT_APELLIDO2,b.MNIT_DIRECCION,b.PCIU_CODIGO,
                          b.MNIT_TELEFONO,b.MNIT_CELULAR,b.MNIT_EMAIL,c.PCIU_NOMBRE,d.pcar_nombcarg,e.pdep_nombdpto 
                          from  dbxschema.mempleado a,dbxschema.mnit b,dbxschema.pciudad c,dbxschema.pcargoempleado d,dbxschema.pdepartamentoempresa e 
                          where a.mnit_nit = b.mnit_nit and b.PCIU_CODIGO = c.PCIU_CODIGO and a.MEMP_CODIEMPL " + CedulaEmpleado + @" and  
                                a.PDEP_CODIDPTO = e.PDEP_CODIDPTO and a.PCAR_CODICARGO = d.PCAR_CODICARGO";
		
            DBFunctions.Request(empleado,IncludeSchema.NO,Sql);
			TotalFilas = empleado.Tables[0].Rows.Count;
			
            if (TotalFilas > 0)
			{
				for (int J=0;J<empleado.Tables[0].Rows.Count;J++)
				{

					MEMP_CODIEMPL.Add(J,empleado.Tables[0].Rows[0]["MEMP_CODIEMPL"].ToString());   
					MNIT_NIT.Add(J,empleado.Tables[0].Rows[0]["MNIT_NIT"].ToString());      
					MEMP_FECHNACI.Add(J,empleado.Tables[0].Rows[0]["MEMP_FECHNACI"].ToString());       
					PCIU_LUGANACI.Add(J,empleado.Tables[0].Rows[0]["PCIU_LUGANACI"].ToString());          
					PALM_ALMACEN.Add(J,empleado.Tables[0].Rows[0]["PALM_ALMACEN"].ToString());           
                    PDEP_CODIDPTO.Add(J,empleado.Tables[0].Rows[0]["PDEP_CODIDPTO"].ToString());          
					TEST_ESTADO.Add(J,empleado.Tables[0].Rows[0]["TEST_ESTADO"].ToString());            
					TSAL_SALARIO.Add(J,empleado.Tables[0].Rows[0]["TSAL_SALARIO"].ToString());          
					TCON_CONTRATO.Add(J,empleado.Tables[0].Rows[0]["TCON_CONTRATO"].ToString());            
					PCAR_CODICARGO.Add(J,empleado.Tables[0].Rows[0]["PCAR_CODICARGO"].ToString());           
					MEMP_FECHINGRESO.Add(J,empleado.Tables[0].Rows[0]["MEMP_FECHINGRESO"].ToString());         
					MEMP_FECHFINCONTRATO.Add(J,empleado.Tables[0].Rows[0]["MEMP_FECHFINCONTRATO"].ToString());    
					MEMP_FECHRETIRO.Add(J,empleado.Tables[0].Rows[0]["MEMP_FECHRETIRO"].ToString());         
					MEMP_UNFAMILIAR.Add(J,empleado.Tables[0].Rows[0]["MEMP_UNFAMILIAR"].ToString());          
					PESP_CODIGO.Add(J,empleado.Tables[0].Rows[0]["PESP_CODIGO"].ToString());              
					TRES_VIVIENDA.Add(J,empleado.Tables[0].Rows[0]["TRES_VIVIENDA"].ToString());            
					MEMP_TARJPROFES.Add(J,empleado.Tables[0].Rows[0]["MEMP_TARJPROFES"].ToString());         
					MEMP_DISTLIBRMILI.Add(J,empleado.Tables[0].Rows[0]["MEMP_DISTLIBRMILI"].ToString());        
					MEMP_CLASELIBRMILI.Add(J,empleado.Tables[0].Rows[0]["MEMP_CLASELIBRMILI"].ToString());      
					MEMP_NUMELIBRMILI.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMELIBRMILI"].ToString());        
					TSEX_CODIGO.Add(J,empleado.Tables[0].Rows[0]["TSEX_CODIGO"].ToString());             
					TEST_ESTACIVIL.Add(J,empleado.Tables[0].Rows[0]["TEST_ESTACIVIL"].ToString());          
					MEMP_NUMEHIJOS.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMEHIJOS"].ToString());           
					MEMP_PERSCARGO.Add(J,empleado.Tables[0].Rows[0]["MEMP_PERSCARGO"].ToString());          
					MEMP_SUELACTU.Add(J,empleado.Tables[0].Rows[0]["MEMP_SUELACTU"].ToString());            
					MEMP_FECHSUELACTU.Add(J,empleado.Tables[0].Rows[0]["MEMP_FECHSUELACTU"].ToString());        
					MEMP_SUELANTER.Add(J,empleado.Tables[0].Rows[0]["MEMP_SUELANTER"].ToString());           
					MEMP_FECSUELANTER.Add(J,empleado.Tables[0].Rows[0]["MEMP_FECSUELANTER"].ToString());        
					MEMP_SALAPROMEDIO.Add(J,empleado.Tables[0].Rows[0]["MEMP_SALAPROMEDIO"].ToString());        
					TSUB_CODIGO.Add(J,empleado.Tables[0].Rows[0]["TSUB_CODIGO"].ToString());              
					PEPS_CODIEPS.Add(J,empleado.Tables[0].Rows[0]["PEPS_CODIEPS"].ToString());             
					MEMP_NUMEAFILEPS.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMEAFILEPS"].ToString());         
					PFON_CODIPENS.Add(J,empleado.Tables[0].Rows[0]["PFON_CODIPENS"].ToString());            
					MEMP_NUMECONTFONDOPENS.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMECONTFONDOPENS"].ToString());   
					PFON_CODIPENSVOLU.Add(J,empleado.Tables[0].Rows[0]["PFON_CODIPENSVOLU"].ToString());        
					PFON_CODICESA.Add(J,empleado.Tables[0].Rows[0]["PFON_CODICESA"].ToString());            
					MEMP_NUMECONTFONDOCESA.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMECONTFONDOCESA"].ToString());   
					PARP_CODIARP.Add(J,empleado.Tables[0].Rows[0]["PARP_CODIARP"].ToString());             
					MEMP_NUMECONTARP.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMECONTARP"].ToString());         
					PRIE_CODIRIES.Add(J,empleado.Tables[0].Rows[0]["PRIE_CODIRIES"].ToString());            
					TRES_DOTACION.Add(J,empleado.Tables[0].Rows[0]["TRES_DOTACION"].ToString());           
					MEMP_TARJRELOJ.Add(J,empleado.Tables[0].Rows[0]["MEMP_TARJRELOJ"].ToString());           
					MEMP_NUMPASE.Add(J,empleado.Tables[0].Rows[0]["MEMP_NUMPASE"].ToString());             
					MEMP_CATEGORIA.Add(J,empleado.Tables[0].Rows[0]["MEMP_CATEGORIA"].ToString());          
					MEMP_PERIPAGO.Add(J,empleado.Tables[0].Rows[0]["MEMP_PERIPAGO"].ToString());            
					MEMP_AJUSUELDO.Add(J,empleado.Tables[0].Rows[0]["MEMP_AJUSUELDO"].ToString());           
					MEMP_FORMPAGO.Add(J,empleado.Tables[0].Rows[0]["MEMP_FORMPAGO"].ToString());            
					PBAN_CODIGO.Add(J,empleado.Tables[0].Rows[0]["PBAN_CODIGO"].ToString());              
					MEMP_CODSUCUEMPL.Add(J,empleado.Tables[0].Rows[0]["MEMP_CODSUCUEMPL"].ToString());         
					MEMP_CUENNOMI.Add(J,empleado.Tables[0].Rows[0]["MEMP_CUENNOMI"].ToString());            
                    MEMP_TESTRETE.Add(J,empleado.Tables[0].Rows[0]["MEMP_TESTRETE"].ToString());            
					MEMP_VREXCESALUD.Add(J,empleado.Tables[0].Rows[0]["MEMP_VREXCESALUD"].ToString());         
	//				MEMP_VREXCEEDUC.Add(J,empleado.Tables[0].Rows[0][52].ToString());          
					MEMP_VREXCEAFC.Add(J,empleado.Tables[0].Rows[0]["MEMP_VREXCEAFC"].ToString());          
					MEMP_PORCRETE.Add(J,empleado.Tables[0].Rows[0]["MEMP_PORCRETE"].ToString());            
					MEMP_VREXCEVIVI.Add(J,empleado.Tables[0].Rows[0]["MEMP_VREXCEVIVI"].ToString());         
					MEMP_INDCOMIS.Add(J,empleado.Tables[0].Rows[0]["MEMP_INDCOMIS"].ToString());           
					TTIP_SECUENCIA.Add(J,empleado.Tables[0].Rows[0]["TTIP_SECUENCIA"].ToString());
                    PCAJ_CODICAJA.Add(J, empleado.Tables[0].Rows[0]["PCAJ_CODICAJA"].ToString());
                    // de mnits
                    MNIT_NOMBRES.Add(J,empleado.Tables[0].Rows[0]["MNIT_NOMBRES"].ToString());      
					MNIT_NOMBRE2.Add(J,empleado.Tables[0].Rows[0]["MNIT_NOMBRE2"].ToString());     
					MNIT_APELLIDOS.Add(J,empleado.Tables[0].Rows[0]["MNIT_APELLIDOS"].ToString());     
					MNIT_APELLIDO2.Add(J,empleado.Tables[0].Rows[0]["MNIT_APELLIDO2"].ToString());    
					MNIT_DIRECCION.Add(J,empleado.Tables[0].Rows[0]["MNIT_DIRECCION"].ToString());      
					PCIU_CODIGO.Add(J,empleado.Tables[0].Rows[0]["PCIU_CODIGO"].ToString());         
					MNIT_TELEFONO.Add(J,empleado.Tables[0].Rows[0]["MNIT_TELEFONO"].ToString());       
					MNIT_CELULAR.Add(J,empleado.Tables[0].Rows[0]["MNIT_CELULAR"].ToString());        
					MNIT_EMAIL.Add(J,empleado.Tables[0].Rows[0]["MNIT_EMAIL"].ToString());          
					// de pciudad
					PCIU_NOMBRE.Add(J,empleado.Tables[0].Rows[0]["PCIU_NOMBRE"].ToString());  
					// de pCARGOEMPLEADO
					PCAR_NOMBCARG.Add(J,empleado.Tables[0].Rows[0]["PCAR_NOMBCARG"].ToString());  
					// de pDEPARTAMENTOEMPRESA
					PDEP_NOMBDPTO.Add(J,empleado.Tables[0].Rows[0]["PDEP_NOMBDPTO"].ToString());  
				}
			}
    
			empleado.Dispose();
		}


		public int p_TotalFilas
		{
			get
			{
				return TotalFilas;
			}
		}

		public int PeridoDeProceso
		{
			set
			{
				PeriodoProceso = value;
			}
			get
			{
				return PeriodoProceso;
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

		public string NominaQueProcesa
		{
			set
			{
				NominaPorceso = value;
			}
			get
			{
				return NominaPorceso;
			}
		}

		public bool p_AgregarAFormulario
		{
			get
			{
				return AgregarAFormulario;
			}
			set
			{
				AgregarAFormulario = value;
			}
		}

		public bool p_NominaMensual
		{
			get
			{
				return NominaMensual;
			}
			set
			{
				NominaMensual = value;
			}
		}

		public int p_LicenciasDelMes
		{
			get
			{
				return Licencias;
			}
		}

		public double p_TotalPagadoST
		{
			get
			{
				return TotalPagadoSubtr;
			}
		}

		public double p_VPrima
		{
			get
			{
				return VPrima;
			}
		}

		public double p_VIndemnizacion
		{
			get
			{
				return VIndemnizacion;
			}
		}


		public double p_VVacaciones
		{
			get
			{
				return VVacaciones;
			}
		}

		public double p_VCensantias
		{
			get
			{
				return VCesantias;
			}
		}

		public double p_VInteCensantias
		{
			get
			{
				return VInteCesantias;
			}
		}

		public double p_BasePrima
		{
			get
			{
				return BasePrima;
			}
		}

		public double p_BaseVacaciones
		{
			get
			{
				return BaseVacaciones;
			}
		}

		public double p_BaseCesantias
		{
			get
			{
				return BaseCesantias;
			}
		}

		public double p_deduccionesmes
		{
			get
			{
				return deduccionesmes;
			}
		}

		public double p_devengadosmes
		{
			get
			{
				return devengadosmes;
			}
		}


		public DataTable AsignarDataTable
		{
			set
			{
				DataTable1 = value;
			}
		}

		public Varios AsignarVarios
		{
			set
			{
				o_Varios = value;
			}
		}

		public CNomina AsignarParametrosNomina
		{
			set
			{
				o_CNomina = value;
			}
		}

		public System.Web.UI.WebControls.DataGrid AsignarDataGrid
		{
			set
			{
				DG = value;
			}
		}

		public int p_FactorVacaciones
		{
			get
			{
				return factorvacaciones;
			}
		}

		public int p_FactorCesantias
		{
			get
			{
				return factorcesantias;
			}
		}

		public int p_FactorPrima
		{
			get
			{
				return factorprima;
			}
		}
		public double p_PromedioVacaciones
		{
			get
			{
				return promediovacaciones;
			}
		}
		public double p_PromedioPrima
		{
			get
			{
				return promedioprima;
			}
		}

		
		public double p_PromedioCesantia
		{
			get
			{
				return promediocesantia;
			}
		}

		public string p_FechaInicio
		{
			get
			{
				armarfechas();
				return fechainicio;
			}
		}


		public string p_FechaRetiro
		{
			get
			{
				armarfechas();
				return fecharetiro;
			}
		}

		public string p_FechaFinal
		{
			get
			{
				armarfechas();
				return fechafinal;
			}
		}

		public System.Web.UI.WebControls.DropDownList p_DDLDIARETI
		{
			get
			{
				return DDLDIARETI;
			}
			set
			{
				DDLDIARETI = value;
			}
		}

		public System.Web.UI.WebControls.DropDownList p_DDLMESRETI
		{
			get
			{
				return DDLMESRETI;
			}
			set
			{
				DDLMESRETI = value;
			}
		}

		
		public System.Web.UI.WebControls.DropDownList p_DDLANORETI
		{
			get
			{
				return DDLANORETI;
			}
			set
			{
				DDLANORETI = value;
			}
		}

		public double LiquidarMesEnCurso(int mes, int ano, int dia, bool subsidia, string liquidaNomina)
		{
            if (o_CNomina.CNOM_ANO != ano.ToString() || o_CNomina.CNOM_MES != mes.ToString())
                return 0; // ya liquido el periodo donde se liquida la empleado
            else
            {
                if(o_CNomina.CNOM_OPCIQUINOMENS == "1" && (dia <= 15 ) && o_CNomina.CNOM_QUINCENA == "2")
                    return 0; // YA LIQUIDO LA QUINCENA donde se retira el empleado
            }

            
            bool liquidarDias = true;
            
            double TotalaPagar      = 0;
			double diasEps          = dia;
            double diasTrabajados   = dia;
			double diasTransporte   = dia;
            double diaNovedadinicio = dia;
            double diaNovedadfinal  = dia;

            int    diasenVacaciones = 0;
            double valoryapagadomes = 0;
            double valoryapagadomesTotal = 0;

            string estadoEmpleado = DB.DBFunctions.SingleData("select TEST_ESTADO FROM MEMPLEADO WHERE MEMP_CODIEMPL = '" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' ;").ToString();
            string consulta = @"select day(dvac_fechfinal) from mvacaciones m, dvacaciones d where m.memp_codiemp = '" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' " +
                               "and m.mvac_secuencia = d.mvac_secuencia AND year(dvac_fechfinal) = "+ ano +" AND MONTH(dvac_fechfinal) = "+ mes +"; ";
            if (estadoEmpleado == "5") // el empleado esta en vacaciones
            {
                int mesV = mes;
                int anoV = ano;
                int periodoQna  = Convert.ToInt32(DB.DBFunctions.SingleData("select CNOM_QUINCENA FROM CNOMINA;"));
                string empleadoV = MEMP_CODIEMPL[NumeroFila].ToString().Trim();
                int periodoPago = Convert.ToInt32(DB.DBFunctions.SingleData("select CNOM_OPCIQUINOMENS FROM CNOMINA;"));
                diasenVacaciones = calcularDiasenVacacionesPeriodo(mesV, anoV, periodoQna, empleadoV, periodoPago);
             }

              
            consulta = @"Select coalesce(sum(d.dqui_apagar  - d.dqui_adescontar),0) " +
                    " from  dbxschema.dquincena d, dbxschema.mquincenaS m, dbxschema.pconceptonomina C, cnomina cn " +
                    " Where d.mqui_codiquin = m.mqui_codiquin AND D.DQUI_VACACIONES <> 'V' " +
                    " 	AND d.MEMP_CODIEMPL = '" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' and c.tres_afec_eps = 'S' " +
                    " 	and c.pcon_concepto = d.pcon_concepto  and m.mqui_tpernomi = cn.cnom_quincena " +
                    " 	and m.mqui_anoquin  = " + ano + " and m.mqui_mesquin = " + mes + " ";
       
            valoryapagadomes = Double.Parse(DB.DBFunctions.SingleData(consulta));

            string consultaMestotal = @"Select coalesce(sum(d.dqui_apagar  - d.dqui_adescontar),0) "+
                   " from  dbxschema.dquincena d, dbxschema.mquincenaS m, dbxschema.pconceptonomina C, cnomina cn "+
                   " Where d.mqui_codiquin = m.mqui_codiquin "+
                   " 	AND d.MEMP_CODIEMPL = '" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' and c.tres_afec_eps = 'S' "+
                   " 	and c.pcon_concepto = d.pcon_concepto AND D.DQUI_VACACIONES <> 'V' "+
                   " 	and m.mqui_anoquin  = " + ano + " and m.mqui_mesquin = " + mes + " ;";

            valoryapagadomesTotal = Double.Parse(DB.DBFunctions.SingleData(consultaMestotal));
            
            DateTime fechaIngreso = Convert.ToDateTime(DBFunctions.SingleData("Select memp_fechingreso from mempleado where MEMP_CODIEMPL = '" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' "));
            
            // se valida si el empleado ingreso dendro del mes actual y se restan los dias no trabajados del inicio el mes
            // toca validar si es pago quincenal o mensual
            if (fechaIngreso.Year == ano && fechaIngreso.Month == mes)
            {
                diasEps        = diasEps        - fechaIngreso.Day + 1;
                diasTransporte = diasTransporte - fechaIngreso.Day + 1;
            }

            if (this.p_TCON_CONTRATO!="3")
            {
                if ((liquidaNomina == "Definitiva" || liquidaNomina == "Vacaciones") && o_CNomina.CNOM_EPSPERINOMI == "2")
                {
                }
                else
                {
                    if (diasEps > 15 && o_CNomina.CNOM_OPCIQUINOMENS == "1" && o_CNomina.CNOM_EPSPERINOMI == "1")
                    {
                        diasEps = diasEps - 15;
                    }
                }
                diasEps = diasEps - diasenVacaciones ;    
            }
            else
            {
                diasEps = 0;
            }

			//  liquidar subsidio de transporte
            string tipoSubsidioTransporte = DBFunctions.SingleData("Select TSUB_CODIGO from mempleado where MEMP_CODIEMPL = '" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' ");
            //if (diasEps > 0 && fechaIngreso.Year == ano && fechaIngreso.Month == mes)
            //    diasEps = diasEps - fechaIngreso.Day + 1;

            //if (fechaIngreso.Year == ano && fechaIngreso.Month == mes)
            //    diasTransporte = diasTransporte - fechaIngreso.Day + 1;
			if ((liquidaNomina == "Definitiva" || liquidaNomina == "Vacaciones") && (o_CNomina.CNOM_SUBSTRANPERINOMI=="2" || o_CNomina.CNOM_SUBSTRANPERINOMI=="4"))
			{
			}
			else
                if (dia > 15 && o_CNomina.CNOM_OPCIQUINOMENS == "1" && (o_CNomina.CNOM_SUBSTRANPERINOMI == "1" || o_CNomina.CNOM_SUBSTRANPERINOMI == "3"))  
					diasTransporte = diasTransporte - 15;

            if (tipoSubsidioTransporte == "3")  // sin subsidio NI auxilio
                diasTransporte = 0;
            else
                diasTransporte = diasTransporte - diasenVacaciones;
				
			if((o_CNomina.CNOM_OPCIQUINOMENS=="1" && MEMP_PERIPAGO[NumeroFila].ToString().Trim() == "1") || (MEMP_PERIPAGO[NumeroFila].ToString().Trim() == "4"))   //pago quincenal
			{
                if(dia >= 16)  // se considera ya pagada la quincena, hay que leer cnomina para saber cual es la qna vigente
                {
				    dia=dia-15;  // dias trabajados
                    diaNovedadinicio = 16;
                    diaNovedadfinal = 30;
                    
                    if (o_CNomina.CNOM_SUBSTRANPERINOMI == "1") // paga transporte en ambas quincenas
                        diasTransporte -= 15;
                    if (fechaIngreso.Year == ano && fechaIngreso.Month == mes && fechaIngreso.Day >= 16)
                        diasTrabajados = diasTrabajados - fechaIngreso.Day + 1;
                    else
                        diasTrabajados -= 15;   // ya se le pago la 1ra quincena
                }
                else
                {
                    if (Convert.ToInt16(o_CNomina.CNOM_ANO) == ano && Convert.ToInt16(o_CNomina.CNOM_MES) == mes && o_CNomina.CNOM_QUINCENA == "1")
                    {
                        diaNovedadinicio = 01;
                        diaNovedadfinal = 15;
                    }
                    else
                    {
                       // liquidarDias = false;
                        diaNovedadinicio = 01;
                        diaNovedadfinal = dia;
                    }
                    if (fechaIngreso.Year == ano && fechaIngreso.Month == mes)
                        diasTrabajados = diasTrabajados - fechaIngreso.Day + 1;
                }
            }
            else
                {
                    diaNovedadinicio = 01;
                    diaNovedadfinal = 30;
                    if (fechaIngreso.Year == ano && fechaIngreso.Month == mes)
                        diasTrabajados = diasTrabajados - fechaIngreso.Day + 1;
                }
                
            if(diaNovedadfinal == 30)
            {
                if (mes == 2)
                   diaNovedadfinal = 28;
                else
                {
                    if(mes == 01 || mes == 03 || mes == 05 || mes == 07 || mes == 08 || mes == 10 || mes == 12)
                        diaNovedadfinal = 31;
                }
            }

            string fechaInicioNovedad = ano + "-"+ mes +"-" + diaNovedadinicio; 
			//if (o_Varios.ProcesarEmpleado(TEST_ESTADO[NumeroFila].ToString(),this.MEMP_CODIEMPL[NumeroFila].ToString(),fechainicio))
			if(true)
			{

				//double valordiasalmin = Double.Parse(o_CNomina.CNOM_SALAMINIACTU.ToString())/30;
				double valordia = Double.Parse(this.MEMP_SUELACTU[NumeroFila].ToString())/30;
				//if (valordia < valordiasalmin) 	valordia = valordiasalmin;

                double valormes = 0;

                // RUTINA DUPLICADA, se debe llevar a O-VARIOS ? o en todo llamar esta clase ?
                //calcular dias de licencias-suspenciones-incapacidades liquidadas en dias y periodos anteriores con continuidad en este periodo  
                int i = 0;
                int DiasSusLicencias = 0;
                DataSet FechasSusplicencias = new DataSet();
                DBFunctions.Request(FechasSusplicencias, IncludeSchema.NO, "Select msus_desde,msus_hasta, MS.PCON_CONCEPTO, PC.PCON_SIGNOLIQ, PC.TRES_AFEC_EPS from DBXSCHEMA.MSUSPLICENCIAS MS, DBXSCHEMA.PCONCEPTONOMINA PC" + 
                    " where memp_codiempl='" + MEMP_CODIEMPL[NumeroFila].ToString().Trim() + "' AND MS.PCON_CONCEPTO = PC.PCON_CONCEPTO" +
                    " and (msus_desde <= '" + fecharetiro + "' AND  msus_hasta >= '" + fechaInicioNovedad + "' ) ");
                for (i = 0; i < FechasSusplicencias.Tables[0].Rows.Count; i++)
                {
                    if (DateTime.Compare(Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][0]), DateTime.Parse(fechaInicioNovedad)) < 0)
                        FechasSusplicencias.Tables[0].Rows[i][0] = fechaInicioNovedad; // si inicia antes del periodo
                    if (DateTime.Compare(Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][1]), DateTime.Parse(fecharetiro)) > 0)
                        FechasSusplicencias.Tables[0].Rows[i][1] = fecharetiro; // si finaliza despues del periodo
                    TimeSpan ts = Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][1]) - Convert.ToDateTime(FechasSusplicencias.Tables[0].Rows[i][0]);
                    int differenceInDays = ts.Days + 1;  // los dias de las licencias o incapacidades son Incluidos los dos topes
                    DiasSusLicencias += differenceInDays;
                    valormes         = valordia * differenceInDays;
                    if (Convert.ToString(FechasSusplicencias.Tables[0].Rows[i][3].ToString()) == "D")
                    {
                        ingresar_datos_datatable(FechasSusplicencias.Tables[0].Rows[i][2].ToString().Trim(), "Dias Susp_licen e Incaps", (double)(differenceInDays), valordia, valormes, 0, "4", 0, "");
                        devengadosmes = devengadosmes + Math.Round(valormes,0);
                        diasTrabajados -= differenceInDays;
                        if (Convert.ToString(FechasSusplicencias.Tables[0].Rows[i][4].ToString()) == "S")
                            Totaleps += valormes;
                    }
                    else
                    {
                        ingresar_datos_datatable(FechasSusplicencias.Tables[0].Rows[i][2].ToString().Trim(), "Dias Susp_licen e Incaps", (double)(differenceInDays), valordia, 0, valormes, "4", 0, "");
                        deduccionesmes = deduccionesmes + Math.Round(valormes, 0);
                        if (Convert.ToString(FechasSusplicencias.Tables[0].Rows[i][4].ToString()) == "S")
                            Totaleps -= valormes;
                    }
                }


                if (liquidarDias)
                    valormes = Math.Round(valordia * (diasTrabajados - diasenVacaciones) - valoryapagadomes, 0);
                else
                    valormes = 0;
                double valormesComple=valordia*diasEps;
				agregarnombre = true;
				ArrayList H_concepto  = new ArrayList();
				ArrayList H_cantidad  = new ArrayList();
				ArrayList H_factor    = new ArrayList();
				ArrayList H_afectaeps = new ArrayList();
				ArrayList H_descripconcepto = new ArrayList();
				NomEmp = MNIT_APELLIDOS[NumeroFila].ToString().Trim() + " " + MNIT_APELLIDO2[NumeroFila].ToString().Trim()+" " + MNIT_NOMBRES[NumeroFila].ToString().Trim() + " " + MNIT_NOMBRE2[NumeroFila].ToString().Trim();     
                if(valormes > 0)
                    ingresar_datos_datatable(o_CNomina.CNOM_CONCSALACODI, "Dias Trabajados", (double)(diasTrabajados - diasenVacaciones), valordia, valormes, 0, "4", 0, "");
			
				devengadosmes = devengadosmes + Math.Round(valormes,0);
                Totaleps = Totaleps + Math.Round(valormesComple, 0) + valoryapagadomes;
                //if (Convert.ToInt16(o_CNomina.CNOM_EPSPERINOMI) != 1 && Convert.ToInt16(o_CNomina.CNOM_QUINCENA) == 2)
                //    Totaleps = Totaleps + valoryapagadomesTotal;  // cuando se descuenta la salud y pension en la segunda quincena, su acumla todo el devengo del mes

                Novedades Nv=new Novedades();
				if(!this.esPeriodoLiquidado)
                    Nv = new Novedades(this.MEMP_CODIEMPL[NumeroFila].ToString(), ano.ToString() + "-" + mes.ToString() + "-" + diaNovedadinicio.ToString(), ano.ToString() + "-" + mes.ToString() + "-" + diaNovedadfinal.ToString());
				 
				double tem = 0;
				if (Nv.p_TotalFilas > 0)
				{
					for (int j=0;j<Nv.p_TotalFilas;j++)
					{
						Nv.AsignarFila = j;
						if (Nv.p_pcon_desccant == "2")
						{	
							H_concepto.Add(Nv.p_pcon_concepto);
							H_cantidad.Add(double.Parse(Nv.p_mnov_cantidad));
							H_factor.Add(double.Parse(Nv.p_pcon_factorliq));
							H_afectaeps.Add(Nv.p_tres_afec_eps);
							H_descripconcepto.Add(Nv.p_pcon_nombconc);
						}
						else
						{
							if (Nv.p_pcon_signoliq == "D")
							{
								tem = Math.Round(double.Parse(Nv.p_mnov_valrtotl),0);

                                ingresar_datos_datatable(Nv.p_pcon_concepto, Nv.p_pcon_nombconc, double.Parse(Nv.p_mnov_cantidad), tem, tem, 0, Nv.p_pcon_desccant, 0, "");
							
								devengadosmes = devengadosmes + tem;
								if (Nv.p_tres_afec_eps == "S")
								{
									Totaleps = Totaleps + tem;
								}
							}
							else
							{
								tem = Math.Round(double.Parse(Nv.p_mnov_valrtotl),0);
								if (Nv.p_tres_afec_eps == "S")
								{
									Totaleps = Totaleps - tem;
								}
                                ingresar_datos_datatable(Nv.p_pcon_concepto, Nv.p_pcon_nombconc, double.Parse(Nv.p_pcon_desccant), tem, 0, tem, Nv.p_pcon_desccant, 0, "");
							
								deduccionesmes = deduccionesmes + tem;
							}
						}
					}
				}
				// calculos para horas extras
				if (H_concepto.Count > 0)
				{
					double valorhora = valordia / 8;
                    
					for (i= 0;i<H_concepto.Count;i++)
					{
						string concepto = (string)H_concepto[i];
						double cantidad = (double)H_cantidad[i];
						string afectaeps = (string)H_afectaeps[i];
						double factor = (double)H_factor[i];
						string descconcepto = (string)H_descripconcepto[i];
						tem = Math.Round(valorhora*cantidad*factor);
		
						ingresar_datos_datatable(concepto,descconcepto,cantidad,valorhora*factor,tem,0,"4",0,"");
						devengadosmes = devengadosmes + tem;
						if (afectaeps == "S")
						{
							Totaleps = Totaleps + tem;
						}
					}
				}
				DateTime tem1 = DateTime.Parse(fechainicio);
				DateTime tem2 = DateTime.Parse(fecharetiro);
		
			//	LicenciasDelMes(tem2.Year.ToString(),tem2.Month.ToString(),tem2.Year.ToString(),tem2.Month.ToString());

				double valoreps = 0;				
				
				if (Totaleps <= (double.Parse(o_CNomina.CNOM_TOPEPAGOEPS) * double.Parse(o_CNomina.CNOM_SALAMINIACTU)))
				{
					valoreps = Totaleps * Double.Parse(o_CNomina.CNOM_PORCEPSEMPL) * 0.01;
				}
				else
				{
					valoreps = (double.Parse(o_CNomina.CNOM_TOPEPAGOEPS) * double.Parse(o_CNomina.CNOM_SALAMINIACTU)) * Double.Parse(o_CNomina.CNOM_PORCEPSEMPL) * 0.01;
				}
                if (valoreps!= 0)
				    ingresar_datos_datatable(o_CNomina.CNOM_CONCEPSCODI,"Eps",diasEps,Math.Round(valoreps,0),0,Math.Round(valoreps,0),"4",0,"");

				deduccionesmes = deduccionesmes + Math.Round(valoreps,0);
		  
				
				double valorfondo = 0;
			
				if (TCON_CONTRATO[NumeroFila].ToString() != "3")
				{
				
					valorfondo = Totaleps * Double.Parse(o_CNomina.CNOM_PORCFONDEMPL) * 0.01;
					ingresar_datos_datatable(o_CNomina.CNOM_CONCFONDCODI,"Fondo de Pensiones",diasEps,Math.Round(valorfondo,0),0,Math.Round(valorfondo,0),"4",0,"");
					deduccionesmes   = deduccionesmes + Math.Round(valorfondo,0);
	                double valorfondosolida = Totaleps * o_Varios.PorcentageFondoSolidaridad((valoryapagadomesTotal+devengadosmes).ToString()) * 0.01;
					if (valorfondosolida > 0)
					{
						ingresar_datos_datatable(o_CNomina.CNOM_CONCFONDSOLIPENS,"Fondo Solidaridad",diasEps,Math.Round(valorfondosolida,0),0,Math.Round(valorfondosolida,0),"4",0,"");
						deduccionesmes = deduccionesmes + Math.Round(valorfondosolida,0);
					}

				    if(subsidia==true)
				    {
					    if (devengadosmes <= (double.Parse(o_CNomina.CNOM_TOPEPAGOAUXITRAN) * Double.Parse(o_CNomina.CNOM_SALAMINIACTU)))
					    {
						    double totalsubtra;
						    if (dia != 30)
						    {
							    totalsubtra = double.Parse(o_CNomina.CNOM_SUBSTRANSACTU) /30 * diasTransporte;
						    }
						    else
						    {
							    totalsubtra = double.Parse(o_CNomina.CNOM_SUBSTRANSACTU);
						    }
                            if (totalsubtra > 0)
                            {
                                double valorDiaTransporte = totalsubtra;
                                if (diasTransporte > 0)
                                    valorDiaTransporte = Math.Round(totalsubtra / diasTransporte,0);
                                ingresar_datos_datatable(o_CNomina.CNOM_CONCSUBTCODI, "Subsidio de Transporte", diasTransporte, valorDiaTransporte, Math.Round(totalsubtra, 0), 0, "4", 0, "");
                                devengadosmes = devengadosmes + Math.Round(totalsubtra, 0);
                            }
                        }
					}
				}
				TotalaPagar = devengadosmes - deduccionesmes;
				
				ingresar_datos_datatable("--","--",1,0,devengadosmes,deduccionesmes," Total a Pagar",TotalaPagar,"--");
				
			}return TotalaPagar;
		}

        private int calcularDiasenVacacionesPeriodo(int mes, int anio, int periodo, string codempleado, int tipoPeriodo)
        {
            int retorna = 0;
            string fechaini, fechafin;
            int diaini = 0, diafin = 0;
            DateTime fechainicial, fechafinal, fechainicialvac, fechafinalvac;
            fechainicialvac = new DateTime();
            fechafinalvac = new DateTime();
            fechafin = "";
            fechafinal = new DateTime();
            switch (tipoPeriodo)
            {
                //Caso periodo pago mensual
                case 1:
                    diaini = 1;
                    if (mes != 2)
                    {
                        diafin = 30;
                        fechafinal = new DateTime(anio, mes, 30);
                        fechafin = fechafinal.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        diafin = 30;
                        fechafinal = new DateTime(anio, mes, 28);
                        fechafin = fechafinal.ToString("yyyy-MM-dd");
                    }
                    break;
                //Caso periodo pago quincenal
                case 2:
                    switch (periodo)
                    {
                        //Primera quincena
                        case 1:
                            diaini = 1;
                            fechafinal = new DateTime(anio, mes, 15);
                            fechafin = fechafinal.ToString("yyyy-MM-dd");
                            diafin = 15;
                            break;
                        //Segunda quincena
                        case 2:
                            diaini = 16;
                            if (mes != 2)
                            {
                                diafin = 30;
                                fechafinal = new DateTime(anio, mes, 30);
                                fechafin = fechafinal.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                diafin = 30;
                                fechafinal = new DateTime(anio, mes, 28);
                                fechafin = fechafinal.ToString("yyyy-MM-dd");
                            }
                            break;
                    }
                    break;
                default:
                    diafin = 0;
                    fechafinal = new DateTime();
                    fechafin = "";
                    break;
            }
            fechainicial = new DateTime(anio, mes, diaini);
            fechaini = fechainicial.ToString("yyyy-MM-dd");
            string query = @"select * from dbxschema.dvacaciones 
							where mvac_secuencia in (select mvac_secuencia 
									from dbxschema.mvacaciones 
									where memp_codiemp='" + codempleado + @"')
						and ('" + fechaini + @"' between dvac_fechinic and dvac_fechfinal 
						or '" + fechafin + @"' between dvac_fechinic and dvac_fechfinal)
                        and dvac_tiem > 0";
            DataSet vacaciones = new DataSet();
            DBFunctions.Request(vacaciones, IncludeSchema.NO, query);
            for (int i = 0; i < vacaciones.Tables[0].Rows.Count; i++)
            {
                fechainicialvac = Convert.ToDateTime(vacaciones.Tables[0].Rows[i]["dvac_fechinic"].ToString());
                fechafinalvac = Convert.ToDateTime(vacaciones.Tables[0].Rows[i]["dvac_fechfinal"].ToString());
                //debe validar el intervalo en que se encuentra
                if (fechafinalvac >= fechainicial && fechafinalvac <= fechafinal)
                {
                    //dos casos: fecha inicial de vacaciones dentro y fuera del intervalo
                    if (fechainicialvac >= fechainicial && fechainicialvac <= fechafinal)
                    {
                        retorna += fechafinalvac.Day - fechainicialvac.Day + 1;  // el dia final toca sumarlo porque es inclusive
                    }
                    else
                    {
                        retorna += fechafinalvac.Day;
                    }
                }
                else
                {
                    retorna += diafin - fechainicialvac.Day;
                }
            }
            return retorna;
        }

		private void ingresar_datos_datatable(string concepto,string descripcionconcepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref)
		{
			if (AgregarAFormulario)
			{
				DataRow fila = DataTable1.NewRow();
				if (NominaMensual)
				{
					if (agregarnombre)
					{
						fila["COD QUINCENA"] = "";
						fila["CODIGO"]=MEMP_CODIEMPL[NumeroFila];
						fila["NOMBRE"]=NomEmp;
						agregarnombre = false;
					}
					else
					{
						fila["COD QUINCENA"] = "";
						fila["CODIGO"]="";
						fila["NOMBRE"]="";
					}
				}
				fila["CONCEPTO"]=concepto;
				fila["DESCRIPCION"]=descripcionconcepto;
				fila["CANT EVENTOS"]=canteventos;
				fila["VALOR EVENTO"]=valorevento;
				fila["A PAGAR"]=apagar;
				fila["A DESCONTAR"]=adescontar;
				if(descripcion!=string.Empty && descripcion.Trim()!="0")
					fila["TIPO EVENTO"]=descripcion;
				else
					fila["TIPO EVENTO"]=this.TipoEvento;
				fila["SALDO"]=saldo;
				if(docref!=string.Empty)
					fila["DOC REFERENCIA"]=docref; 
				else
					fila["DOC REFERENCIA"]=this.docurefe; 
				DataTable1.Rows.Add(fila);
			    
				DG.DataSource = DataTable1;
				DG.DataBind();
				DatasToControls.Aplicar_Formato_Grilla(DG);
				DatasToControls.JustificacionGrilla(DG,DataTable1);
			}
			else
			{
				string texto = concepto + " " + canteventos.ToString() + " " + valorevento.ToString() + " " + apagar.ToString() + " " + adescontar.ToString() + " " + descripcion + " " + saldo.ToString() + docref;

				Console.WriteLine(texto);
			}

		}

		public int LiquidacionVacaciones(int DiasV,int MesV,int AnoV)
		{
			int diasVacaciones = 0;
			if (TCON_CONTRATO[NumeroFila].ToString() != "3")
			{
				//factorvacaciones = o_Varios.DiasVacaciones(MEMP_CODIEMPL[NumeroFila].ToString());
				diasVacaciones=this.calcularDiasVacaciones(DiasV,MesV,AnoV);
		//		if (DiasV >= 12)
		//		{
		//			factorvacaciones++;
		//		}
				if(!this.EsPeriodoLiquidado)
				{
					promediovacaciones = promediovacaciones + devengadosmes;  // que afecten vacaciones
		//			diasVacaciones+=DiasV;
				}
				//BaseVacaciones =  promediovacaciones / factorvacaciones * 30;
				//BaseVacaciones = BaseCesantias;
		//		VVacaciones = Convert.ToDouble(BaseVacaciones) / Convert.ToDouble(30) * Convert.ToDouble(factorvacaciones);
        //    aqui falta restar los dias disfrutados de vacaciones del empleado, porque calcula sobre todo el tiempo trabajando
				//VVacaciones = Convert.ToDouble(BaseVacaciones) / Convert.ToDouble(720) * Convert.ToDouble(factorcesantias);
		//		VVacaciones = Convert.ToDouble(promediovacaciones) /Convert.ToDouble(diasVacaciones)*30;
		//		VVacaciones=VVacaciones*Convert.ToDouble(diasVacaciones)/720;
				VVacaciones=Convert.ToDouble(promediovacaciones)/30*Convert.ToDouble(diasVacaciones);
				//JFSC 08052008 desplazado al final.
				factorvacaciones = o_Varios.DiasVacaciones(MEMP_CODIEMPL[NumeroFila].ToString());
			}
			else
			{
				VVacaciones = 0;
				factorvacaciones = 0;
			}
			return diasVacaciones;
		}

		
		public double liquidaCesantiasEMP(int diasT,string salarioEmp)
		{	
			double s;
			s=Convert.ToDouble(salarioEmp);
					
			double cesantiasApagar=(s*Convert.ToDouble(diasT))/(double)360;
			return cesantiasApagar;		
		}
		

		public double liquidaCesantiasEMP(int diasT,string salarioEmp,string subsidio)
		{				
			double s;
			s=Convert.ToDouble(salarioEmp);			
				
			double cesantiasApagar=((s+Convert.ToDouble(subsidio))*Convert.ToDouble(diasT))/(double)360;
			return cesantiasApagar;		
		}
		
		public void LiquidacionCesantias(int DiasPrimer,int DiasSegundo,int DiasV)
		{
			if (TCON_CONTRATO[NumeroFila].ToString() != "3")
			{
				//JFSC 07052008 si ya se liquidó, no se vuelve a sumar
				if(!this.esPeriodoLiquidado)
				{
					factorcesantias = factorcesantias + DiasV - Licencias;
			//		promediocesantia = promediocesantia + devengadosmes;  // que afecten cesantia
				}
				else
					factorcesantias = factorcesantias - Licencias;
				factorcesantias = factorcesantias + DiasPrimer + DiasSegundo;
				//promediocesantia = promediocesantia + devengadosmes;
                BaseCesantias = Math.Round(Convert.ToDouble((promediocesantia + (promediocesantia * factorcesantias / 30) )), 0);  // VER COMO CUADRA LOS PAGOS DE LOS DIAS 
                BaseCesantias = promediocesantia;
				VCesantias = Math.Round(Convert.ToDouble(BaseCesantias*(Convert.ToDouble(factorcesantias)/Convert.ToDouble(360))),0);
				VInteCesantias = Math.Round(Convert.ToDouble(VCesantias * 0.12 * factorcesantias /30),0);
			}
			else
			{
				factorcesantias =0;
				VCesantias = 0;
				VInteCesantias = 0;
			}
		}


		public double interesCesantiaEMP(double vcesantia,int diasTr)
		{
			double interes=(vcesantia*Convert.ToDouble(diasTr)*0.12)/(double)360;
			return interes;
		}


		public double liquidaPrimaEMP(int diasT,string salarioEmp,int MesV)
		{	
			if(MesV>6){diasT=diasT-180;}
			double s;
			s=Convert.ToDouble(salarioEmp);					
			double primaApagar=(s*Convert.ToDouble(diasT))/(double)360;
			return primaApagar;		
		}

		public double liquidaPrimaEMP(int diasT,string salarioEmp,string subsidio,int MesV)
		{		
			if(MesV>6){diasT=diasT-180;}
			double s;
			s=Convert.ToDouble(salarioEmp);					
			double primaApagar=((s+Convert.ToDouble(subsidio))*Convert.ToDouble(diasT))/(double)360;
			return primaApagar;		
		}

		public double liquidaVacacionesEMP(int diasT,string salarioEmp)
		{				
			double s;
			s=Convert.ToDouble(salarioEmp);					
			double vacacinesApagar=(s*Convert.ToDouble(diasT))/(double)720;
			return vacacinesApagar;		
		}

		public double pagosDescuentosEMP(int mes,int ano,int dia)
		{
			int quin=0;
			if(dia>15)
			{ quin=2;}
			else{quin=1;}

			double retorna=0;
            string query = "Select sum(mpag_valor) from DBXSCHEMA.MPAGOSYDTOSPER where memp_codiempl='" + this.p_MEMP_CODIEMPL + @"' and MPAG_FECHFINANO || MPAG_FECHFINMES >=" + ano + @" || " + mes + @" and (mpag_tperpag = " + quin + @" or mpag_tperpag = 3);"; 
			//trae los pagos permanentes a la fecha toca validar luego el mes
			
			string aux=DBFunctions.SingleData(query);
			if (aux!="")
				retorna=Convert.ToDouble(aux);
			else 
				retorna=0;
			 return retorna;
		}
		
		public double devengadoMesEMP(int dias,string sueldo,int quinOmes)
		{
			double retornar=(Convert.ToDouble(sueldo.ToString())/(double)30)*Convert.ToDouble(dias);
			return retornar;
		}

        public double novedades_total_EMP(string Tipo)
		{
			double retorna=0;
            string fecha = Tipo;
			string queryD=@" select COALESCE(sum(MNOV_valrtotl),0) FROM DBXSCHEMA.MNOVEDADESNOMINA MN,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE pcon.pcon_concepto=mn.pcon_concepto and MEMP_CODIEMPL ='"+this.p_MEMP_CODIEMPL+@"' AND pcon_signoliq='D' and mnov_fecha >= '"+fecha+"' and pcon.pcon_desccant = 4;";
			string queryH=@" select COALESCE(sum(MNOV_valrtotl),0) FROM DBXSCHEMA.MNOVEDADESNOMINA MN,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE pcon.pcon_concepto=mn.pcon_concepto and MEMP_CODIEMPL ='"+this.p_MEMP_CODIEMPL+@"' AND pcon_signoliq='H' and mnov_fecha >= '"+fecha+"' and pcon.pcon_desccant = 4;";

            double novedadesD = Convert.ToDouble(DBFunctions.SingleData(queryD));
            double novedadesH = Convert.ToDouble(DBFunctions.SingleData(queryH));
            double novedades = novedadesD - novedadesH;
            return novedades;		
		}

		public double prestamosEMP_total_EMP()
		{
			double retorna=0;
			string query=@"select sum (MPRE_VALORPRES - mpre_valopaga) FROM DBXSCHEMA.MPRESTAMOEMPLEADOS WHERE MEMP_CODIEMPL ='"+this.p_MEMP_CODIEMPL+@"';";								
			string aux=DBFunctions.SingleData(query);
			if (aux!="")
			    retorna= Convert.ToDouble(aux);
			else
				retorna=0;
			return retorna;		
		}
		public void LiquidacionPrima(int DiasPrimer,int DiasSegundo,int DiasV, int MesV, int diasPrima)
		{
			if (TCON_CONTRATO[NumeroFila].ToString() != "3")
			{
				//JFSC 07052008 si ya se liquidó, no se vuelve a sumar
				if(!this.esPeriodoLiquidado)
				{
					factorprima = factorprima + DiasV - Licencias ;
					promedioprima = promedioprima + devengadosmes;  // que afecten prima
				}
				else
					factorprima = factorprima - Licencias ;
				if (MesV <=6)
				{
					factorprima = factorprima + DiasPrimer;
				}
				else
				{
					factorprima = factorprima + DiasSegundo;
				}
				//Si no se han sumado conceptos, se tienen que sumar
				//if(this.noTieneConceptos)promedioprima+=this.calcularConceptosPrima(MesV);
				//promedioprima = promedioprima + devengadosmes;
				BasePrima = promedioprima*30/factorprima;
                diasPrima = factorprima;
				VPrima = BasePrima*Convert.ToDouble(factorprima)/Convert.ToDouble(360);  // 360
			}
			else
			{
				VPrima = 0;
				factorprima = 0;
			}
		}
		
		//Método que calcula los conceptos del semestre para prima
		//mes: mes en el que se encuentra
		//return: sumatoria de los conceptos del semestre
	/*	public double calcularConceptosPrima(int mes)//ppuntop Revisar por que 2008??
		{
			double retorna;         //  ojo campo fijo 070, verificar la variable
			string consulta=@"Select sum(a.dqui_apagar) 
								from dbxschema.dquincena a, 
									dbxschema.mquincenaS b, 
									dbxschema.pconceptonomina c 
								Where a.mqui_codiquin = b.mqui_codiquin 
									AND A.MEMP_CODIEMPL ";
					consulta+=@"+ CedulaEmpleado +"; 
					consulta+=@" and a.pcon_concepto = c.pcon_concepto";
			if(mes <=6)
			{
				consulta+=@" AND ((b.mqui_anoquin = 2008 and mqui_mesquin >= 1) 
							OR (b.mqui_anoquin = 2008 and mqui_mesquin <=6))";
			}
			else
			{
				consulta+=@" AND ((b.mqui_anoquin = 2008 and mqui_mesquin >= 7) 
							OR (b.mqui_anoquin = 2008 and mqui_mesquin <=12))";
			}
				consulta+=@" and c.tres_afecprima='S'
								and a.pcon_concepto not in(select cnom_concsalacodi 
															from dbxschema.cnomina)";
			try
			{
				retorna=Double.Parse(DB.DBFunctions.SingleData(consulta));
			}
			catch
			{
				retorna=0;
			}
			return retorna;
		}
*/
		public void LiquidarIndemnizacion()
		{
			if (TCON_CONTRATO[NumeroFila].ToString() != "3")
			{
				VIndemnizacion = 0;
				DateTime fechaing= new DateTime();
				DateTime fecharet= new DateTime();
				fechaing=Convert.ToDateTime(MEMP_FECHINGRESO[NumeroFila].ToString());
				if (TCON_CONTRATO[NumeroFila].ToString() == "2")
				{
				
					fechaing=Convert.ToDateTime(fecharetiro);
					fecharet=Convert.ToDateTime(MEMP_FECHFINCONTRATO[NumeroFila].ToString());
					if (fechaing > fecharet) VIndemnizacion = -1;
				}
				else
				{
					fechaing=Convert.ToDateTime(MEMP_FECHINGRESO[NumeroFila].ToString());
					fecharet=Convert.ToDateTime(fecharetiro);
				}

				if (VIndemnizacion == 0)
				{
					int ano1 = fechaing.Year;
					int ano2 = fecharet.Year;
					int mes1 = fechaing.Month;
					int mes2 = fecharet.Month;
					int dia1 = fechaing.Day;
					int dia2 = fecharet.Day;

					int saldodias = ((ano2 * 360) + (mes2 * 30) + dia2) - ((ano1 * 360) + (mes1 * 30) + dia1);
	
					// término indefinido  // primer año o fracción = 30 dias, a partir del segundo año son 20 dias por cada año y se pagan las facciones

                    if (TCON_CONTRATO[NumeroFila].ToString() == "1")  // termino indefinido
                    {
                        if (saldodias <= 360)
                        {
                            VIndemnizacion = BaseCesantias;
                            saldodias = 0;
                        }

                        while (saldodias > 360)
                        {
                            VIndemnizacion += (BaseCesantias / 30) * 20;
                            saldodias -= 360;
                        }
                        VIndemnizacion += (BaseCesantias / 30) * (20 * saldodias / 360);
                    }
                        
					if (TCON_CONTRATO[NumeroFila].ToString() == "2")  // termino fijo
					{
						int tot1 = saldodias /30; 
						VIndemnizacion = double.Parse(MEMP_SUELACTU[NumeroFila].ToString()) * tot1;
						VIndemnizacion = VIndemnizacion + (double.Parse(MEMP_SUELACTU[NumeroFila].ToString()) / 30 * (saldodias - (tot1 * 30)));
					}
				}
			}
			else
			{
				VIndemnizacion = 0;
			}
		}

		public void LicenciasDelMes(string ano1,string mes1,string ano2,string mes2)
		{
			Licencias Lc = new Licencias(this.MEMP_CODIEMPL[NumeroFila].ToString(),ano1,mes1,ano2,mes2);
			double valor = 0;
			if (Lc.p_TotalFilas > 0)
			{
				for (int j=0;j<Lc.p_TotalFilas;j++)
				{
					Lc.AsignarFila = j;
					valor = double.Parse(Lc.p_totaldias) * (double.Parse(this.MEMP_SUELACTU[NumeroFila].ToString()) / 30);
					double valt = double.Parse(this.MEMP_SUELACTU[NumeroFila].ToString())/30;
				//	ingresar_datos_datatable(Lc.p_pcon_concepto,Lc.p_pcon_nombconc,double.Parse(Lc.p_totaldias),Math.Round(valt,0),Math.Round(valor,0)*-1,0,Lc.p_tdes_descripcion,0,"");
                    ingresar_datos_datatable(Lc.p_pcon_concepto, Lc.p_pcon_nombconc, double.Parse(Lc.p_totaldias), Math.Round(valt, 0), Math.Round(valor, 0) * -1, 0, "4", 0, "");
                    Licencias = Licencias + Int32.Parse(Lc.p_totaldias);
					devengadosmes = devengadosmes - Math.Round(valor,0);
					if (Lc.p_tres_afec_eps == "S")
					{
						Totaleps = Totaleps - Math.Round(valor,0);
					}
				}
			}
		}


		public void PromedioUltimoAno(int ano1,int ano2,int dia,int mes)
		{			
			string tem = this.MEMP_CODIEMPL[NumeroFila].ToString();
			//JFSC 23042008 cargar suspensiones para validacion de cesantias
            DataSet susp = this.cargarSuspensiones(tem);
			Acumulados Ac = new Acumulados(tem,ano1,ano2,mes);
			double valor = 0;
			bool controlmesanoanterior = true;
			//JFSC 08052008 calcular la quincena en la que se encuentra el valor 
			string quincenaLiq=this.detectarQuincena(dia);
			// calcular lo ganado 
			
			for (int j=0;j<Ac.p_TotalFilas;j++)
			{
				Ac.AsignarFila = j;
				this.DocuRefe=Ac.p_DQUI_DOCREFE;
				this.TipoEvento=Ac.p_DQUI_DESCCANTIDAD;
				//JFSC24042008 Validar los dias por periodo
				//int cantidadeventos=this.validaSuspension(int.Parse(Ac.p_MQUI_MESQUIN),int.Parse(Ac.p_MQUI_ANOQUIN),int.Parse(Ac.p_DQUI_CANTEVENTOS));
				int cantidadeventos=int.Parse(Ac.p_DQUI_CANTEVENTOS==string.Empty?"0":Ac.p_DQUI_CANTEVENTOS);
				//JFSC 07052008 Si ya está liquidado este último periodo, se debe tener en cuenta para no sumarse nuevamente				
				if(Ac.p_MQUI_MESQUIN == mes.ToString() && Ac.p_MQUI_ANOQUIN==ano2.ToString() && Ac.p_MQUI_TPERNOMI==quincenaLiq && Ac.p_MQUI_ESTADO == "2")
					this.esPeriodoLiquidado=true;
				valor = double.Parse(Ac.p_DQUI_APAGAR);
				if (valor > 0)
				{
					if (ano1 < ano2)
					{
						if (controlmesanoanterior)
						{
							if (mes.ToString() != Ac.p_MQUI_MESQUIN)
							{
								if (TotalPagadoSubtr > 0)
								{
									TotalPagadoSubtr = TotalPagadoSubtr / 30 * (30-dia);
								}
								if (promedioprima > 0)
								{
									promedioprima = promedioprima / 30 * (30-dia);
								}
								if (factorprima > 0)
								{
									factorprima = 30 - dia;
								}
								if (promediovacaciones > 0)
								{
									promediovacaciones = promediovacaciones / 30 * (30-dia);
								}
								if (factorvacaciones > 0)
								{
									factorvacaciones = 30 - dia;
								}
								//								if (promediocesantia > 0)
								//								{
								//									promediocesantia = promediocesantia / 30 * (30-dia);
								//								}
								//								if (factorcesantias > 0)
								//								{
								//									factorcesantias = factorcesantias + 30 - dia;
								//								}
								controlmesanoanterior = false;
							}
						}
					}

		
					// sumo el total pagado del subsidio de transporte 
					if (Ac.p_PCON_CONCEPTO == o_CNomina.CNOM_CONCSUBTCODI)
					{
						TotalPagadoSubtr = TotalPagadoSubtr + valor;
					}
					// salario variable
					if (this.TSAL_SALARIO[NumeroFila].ToString() == "2")
					{
						// primas
						if (Ac.p_TRES_AFECPRIMA == "S")
						{
							if (mes <=6)
							{
								if ((Int32.Parse(Ac.p_MQUI_MESQUIN) <= 6) && (Ac.p_MQUI_ANOQUIN == ano2.ToString()))
								{
									if(this.LiquidoCesantias)
									{
										if(Ac.p_MQUI_ANOQUIN==ano2.ToString())
										{
											promedioprima = promedioprima + valor;
											if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
											{
												//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
												factorprima = factorprima + cantidadeventos;
											}
										}
									}
									else
									{
										promedioprima = promedioprima + valor;
										if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
										{
											//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
											factorprima = factorprima + cantidadeventos;
										}
									}
								}
							}
							else
							{
								if (Int32.Parse(Ac.p_MQUI_MESQUIN) > 6 && Ac.p_MQUI_ANOQUIN.Trim() == ano2.ToString())
								{
									/*promedioprima = promedioprima + valor;
									if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
									{
										//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
										factorprima = factorprima + cantidadeventos;
									}*/
									if(this.LiquidoCesantias)
									{
										if(Ac.p_MQUI_ANOQUIN==ano2.ToString())
										{
											promedioprima = promedioprima + valor;
											if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
											{
												//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
												factorprima = factorprima + cantidadeventos;
											}
										}
									}
									else
									{
										promedioprima = promedioprima + valor;
										if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
										{
											//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
											factorprima = factorprima + cantidadeventos;
										}
									}
								}
							}
						}
						// vacaciones
						if (Ac.p_TRES_AFECVACACION == "S")
						{
							promediovacaciones = promediovacaciones + valor;
							if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
							{
								factorvacaciones = factorvacaciones + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
								//factorvacaciones = factorvacaciones + cantidadeventos;
							}							
						}
						// cesantias
						if (Ac.p_TRES_AFECCESANTIA == "S")
						{
							//JFSC 21042008: acá debe existir la validación de las cesantías
							if(this.LiquidoCesantias)
							{
								//JFSC 21042008 se valida si es el año actual para efectos de cesantías
								if(Ac.p_MQUI_ANOQUIN==ano2.ToString())
								{
									promediocesantia = promediocesantia + valor;
									if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
									{
										//JFSC 23042008 aqui se está validando
										//int cantidadeventos=this.validaSuspension(int.Parse(Ac.p_MQUI_MESQUIN),int.Parse(Ac.p_MQUI_ANOQUIN),int.Parse(Ac.p_DQUI_CANTEVENTOS));
										factorcesantias = factorcesantias + cantidadeventos;
									}
								}
							}
							else
							{
								promediocesantia = promediocesantia + valor;
								if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
								{
									//JFSC 23042008 aqui se está validando
									//int cantidadeventos=this.validaSuspension(int.Parse(Ac.p_MQUI_MESQUIN),int.Parse(Ac.p_MQUI_ANOQUIN),int.Parse(Ac.p_DQUI_CANTEVENTOS));
									factorcesantias = factorcesantias + cantidadeventos;
								}
							}
						}
					}
					else
					{
						// FIJO
						// primas, falta revisar promedio salario cuando se liquida antes de marzo 30 y cambio el salario
						if (Ac.p_TRES_AFECPRIMA == "S")
						{  
							if (this.MEMP_FECHSUELACTU[NumeroFila].ToString() != "")
							{
								resumendias = Convert.ToDateTime(this.fecharetiro) - Convert.ToDateTime(this.MEMP_FECHSUELACTU[NumeroFila]);
								if ( resumendias.Days < 90)
								{
									if (Int32.Parse(Ac.p_MQUI_MESQUIN) >= mes - 3)
									{
										promedioprima = promedioprima + valor;
									}
								}
								else
								{
									promedioprima = double.Parse(this.MEMP_SUELACTU[NumeroFila].ToString());
									this.noTieneConceptos=true;
								}
							}
							else
							{
								promedioprima = double.Parse(this.MEMP_SUELACTU[NumeroFila].ToString());
								this.noTieneConceptos=true;
							}
							if (mes <=6)
							{
								if ((Int32.Parse(Ac.p_MQUI_MESQUIN) <= 6) && (Ac.p_MQUI_ANOQUIN == ano2.ToString()))
								{
									/*if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
									{
										//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);}
										factorprima = factorprima + cantidadeventos;
									}*/
									if(this.LiquidoCesantias)
									{
										if(Ac.p_MQUI_ANOQUIN==ano2.ToString())
										{
											if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
											{
												//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
												factorprima = factorprima + cantidadeventos;
											}
										}
									}
									else
									{
										if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
										{
											//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
											factorprima = factorprima + cantidadeventos;
										}
									}
								}
							}
							else
							{
								if (Int32.Parse(Ac.p_MQUI_MESQUIN) > 6 && Ac.p_MQUI_ANOQUIN.Trim() == ano2.ToString())
								{
									/*if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
									{
										//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
										factorprima = factorprima + cantidadeventos;
									}*/
									if(this.LiquidoCesantias)
									{
										if(Ac.p_MQUI_ANOQUIN==ano2.ToString())
										{
											if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
											{
												//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
												factorprima = factorprima + cantidadeventos;
											}
										}
									}
									else
									{
										if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT == "1")
										{
											//factorprima = factorprima + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
											factorprima = factorprima + cantidadeventos;
										}
									}
								}
							}
						}
						// vacaciones
						if (Ac.p_TRES_AFECVACACION == "S")
						{
							promediovacaciones = promediovacaciones + valor; 
							if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
							{
								factorvacaciones = factorvacaciones + Int32.Parse(Ac.p_DQUI_CANTEVENTOS);
								//factorvacaciones = factorvacaciones + cantidadeventos;
							}
						}
						// cesantias
						//JFSC 2103429008: acá debe existir la validación de las cesantías
						if (Ac.p_TRES_AFECCESANTIA == "S")
						{
							if(this.liquidoCesantias)
							{
								//JFSC 21042008 se valida si es el año actual para efectos de cesantías
								if(Ac.p_MQUI_ANOQUIN==ano2.ToString())
								{
									promediocesantia = promediocesantia + valor;
									if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
									{										
										factorcesantias = factorcesantias + cantidadeventos;
									}
								}
							}
							else
							{
								promediocesantia = promediocesantia + valor;
								if (Ac.p_TRES_AFECLIQUIDDEFINITIVA == "S" && Ac.p_PCON_DESCCANT=="1")
								{
									//JFSC 23042008 aqui se está validando
									//int cantidadeventos=this.validaSuspension(int.Parse(Ac.p_MQUI_MESQUIN),int.Parse(Ac.p_MQUI_ANOQUIN),int.Parse(Ac.p_DQUI_CANTEVENTOS));
									factorcesantias = factorcesantias + cantidadeventos;
								}
							}
						}
					}
				}
			}
		}
		
		//JFSC 08052008 Método que permite detectar en qué quincena se encuentra un dia, el dia de liquidación
		//dia: el día de liquidación
		//return el número de la quincena
		public string detectarQuincena(int dia)
		{
			string retorno;
			if(dia<=15 && o_CNomina.CNOM_OPCIQUINOMENS == "1")
				retorno="1";
			else
				retorno="2";
			return retorno;
		}
		
		//JFSC 08052008 Método que calcula los dias de vacaciones a los que tiene 
		//derecho una persona
		//return la cantidad de dias con los que se deben calcular vacaciones
		public int calcularDiasVacaciones(int dias, int mes, int anio)
		{
			int retorna;
			DateTime ingreso=DateTime.Parse(this.p_MEMP_FECHINGRESO);
			DateTime retiro=new DateTime(anio,mes,dias);
			//Calcular los dias totales (ingreso- fin)
			//TimeSpan diasVaca=retiro-ingreso;
			retorna=this.calcularDiasContables(retiro,ingreso);
			//restar las vacaciones disfrutadas, suspensiones y licencias no remuneradas
			string query=@"select sum(msus_hasta-msus_desde+1) 
							from dbxschema.msusplicencias 
							where memp_codiempl='"+this.p_MEMP_CODIEMPL+@"' 
								and ttip_coditipo in (1,4)";
			string aux=DBFunctions.SingleData(query);
			int suspensionesLicencias=int.Parse(aux==string.Empty?"0":aux);
			query=@"select sum(mvac_diasvacadisf) 
					from dbxschema.mvacaciones 
					where memp_codiemp='"+this.p_MEMP_CODIEMPL+"'";
			aux=DBFunctions.SingleData(query);
			int disfrutados=int.Parse(aux==string.Empty?"0":aux);
			//retorna=int.Parse(diasVaca.Days.ToString());
			retorna = retorna - suspensionesLicencias;
			retorna = ((retorna * 15) / 360) - disfrutados; 
			return retorna;
		}

		private int calcularDiasContables(DateTime retiro,DateTime ingreso)
		{
			int retorna;
			retorna =
				((retiro.Year*360 + retiro.Month*30 + retiro.Day)-(ingreso.Year*360 + ingreso.Month*30 + ingreso.Day) + 1);
			/*
			int mesAnio,mesi,mesf,diai,diaf;
			mesAnio=(retiro.Year-ingreso.Year)-1;
			mesi=(12-ingreso.Month);
			mesf=retiro.Month-1;
			retorna=mesAnio+mesi+mesf;
			retorna=retorna*30;
			diai=(30-ingreso.Day)+1;
			diaf=retiro.Day;
			retorna=retorna+diai+diaf;
			*/
			return retorna;
		}

		public void armarfechas()
		{
			if (NominaMensual)
			{
				if(Convert.ToInt32(o_CNomina.CNOM_MES)==2)
				{
					fechainicio=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-01";
					fechafinal=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-28";
					fecharetiro=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-28";

				}
				else
				{
					fechainicio=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-01";
					fechafinal=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-30";
					fecharetiro=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-30";
				}
			}
			else
			{
				fechainicio=DDLANORETI.SelectedValue+"-01"+"-01";
				if(Convert.ToInt32(DDLMESRETI.SelectedValue)>=1 && Convert.ToInt32(DDLMESRETI.SelectedValue)<=9)
				{
					if(Convert.ToInt32(DDLMESRETI.SelectedValue)==2)
					{
				
						fechafinal=DDLANORETI.SelectedValue+"-0"+DDLMESRETI.SelectedValue+"-"+"28";
				
					}
					else
					{
						if(Convert.ToInt32(DDLMESRETI.SelectedValue)< 10)
						{
							fechafinal=DDLANORETI.SelectedValue+"-0"+DDLMESRETI.SelectedValue+"-"+"30";
						}
						else
						{
							fechafinal=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-"+"30";
						}
					}
				}
				else
				{
					fechafinal=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-"+"30";
				}	
				fecharetiro=DDLANORETI.SelectedValue+"-"+DDLMESRETI.SelectedValue+"-"+DDLDIARETI.SelectedValue;
			}
		}


		public string p_MEMP_CODIEMPL 
		{
			get
			{
				return (string)MEMP_CODIEMPL[NumeroFila];
			}
		}
		public string p_MNIT_NIT
		{
			get
			{
				return (string)MNIT_NIT[NumeroFila];
			}
		}
		public string p_MEMP_FECHNACI       
		{
			get
			{
				return (string)MEMP_FECHNACI[NumeroFila];
			}
		}
		public string p_PCIU_LUGANACI
		{
			get
			{
				return (string)PCIU_LUGANACI[NumeroFila];
			}
		}
		public string p_PALM_ALMACEN           
		{
			get
			{
				return (string)PALM_ALMACEN[NumeroFila];
			}
		}
		public string p_PDEP_CODIDPTO          
		{
			get
			{
				return (string)PDEP_CODIDPTO[NumeroFila];
			}
		}
		public string p_TEST_ESTADO            
		{
			get
			{
				return (string)TEST_ESTADO[NumeroFila];
			}
		}
		public string p_TSAL_SALARIO          
		{
			get
			{
				return (string)TSAL_SALARIO[NumeroFila];
			}
		}
		
		public string p_TCON_CONTRATO          
		{
			get
			{
				return (string)TCON_CONTRATO[NumeroFila];
			}
		}
		public string p_PCAR_CODICARGO          
		{
			get
			{
				return (string)PCAR_CODICARGO[NumeroFila];
			}
		}
		
		public string p_MEMP_FECHINGRESO         
		{
			get
			{
				return (string)MEMP_FECHINGRESO[NumeroFila];
			}
		}
		public string p_MEMP_FECHFINCONTRATO
		{
			get
			{
				return (string)MEMP_FECHFINCONTRATO[NumeroFila];
			}
		}		
		public string p_MEMP_FECHRETIRO      
		{
			get
			{
				return (string)MEMP_FECHRETIRO[NumeroFila];
			}
		}
		public string p_MEMP_UNFAMILIAR          
		{
			get
			{
				return (string)MEMP_UNFAMILIAR[NumeroFila];
			}
		}
		public string p_PESP_CODIGO              
		{
			get
			{
				return (string)PESP_CODIGO[NumeroFila];
			}
		}
		public string p_TRES_VIVIENDA           
		{
			get
			{
				return (string)TRES_VIVIENDA[NumeroFila];
			}
		}
		public string p_MEMP_TARJPROFES         
		{
			get
			{
				return (string)MEMP_TARJPROFES[NumeroFila];
			}
		}
		public string p_MEMP_DISTLIBRMILI        
		{
			get
			{
				return (string)MEMP_DISTLIBRMILI[NumeroFila];
			}
		}
		public string p_MEMP_CLASELIBRMILI
		{
			get
			{
				return (string)MEMP_CLASELIBRMILI[NumeroFila];
			}
		}
		public string p_MEMP_NUMELIBRMILI
		{
			get
			{
				return (string)MEMP_NUMELIBRMILI[NumeroFila];
			}
		}
		public string p_TSEX_CODIGO         
		{
			get
			{
				return (string)TSEX_CODIGO[NumeroFila];
			}
		}
		public string p_TEST_ESTACIVIL         
		{
			get
			{
				return (string)TEST_ESTACIVIL[NumeroFila];
			}
		}
		public string p_MEMP_NUMEHIJOS           
		{
			get
			{
				return (string)MEMP_NUMEHIJOS[NumeroFila];
			}
		}
		public string p_MEMP_PERSCARGO       
		{
			get
			{
				return (string)MEMP_PERSCARGO[NumeroFila];
			}
		}
		public string p_MEMP_SUELACTU       
		{
			get
			{
				return (string)MEMP_SUELACTU[NumeroFila];
			}
		}
		public string p_MEMP_FECHSUELACTU        
		{
			get
			{
				return (string)MEMP_FECHSUELACTU[NumeroFila];
			}
		}
		public string p_MEMP_SUELANTER        
		{
			get
			{
				return (string)MEMP_SUELANTER[NumeroFila];
			}
		}
		public string p_MEMP_FECSUELANTER        
		{
			get
			{
				return (string)MEMP_FECSUELANTER[NumeroFila];
			}
		}
		public string p_MEMP_SALAPROMEDIO        
		{
			get
			{
				return (string)MEMP_SALAPROMEDIO[NumeroFila];
			}
		}
		public string p_TSUB_CODIGO              
		{
			get
			{
				return (string)TSUB_CODIGO[NumeroFila];
			}
		}
		public string p_PEPS_CODIEPS           
		{
			get
			{
				return (string)PEPS_CODIEPS[NumeroFila];
			}
		}
		public string p_MEMP_NUMEAFILEPS        
		{
			get
			{
				return (string)MEMP_NUMEAFILEPS[NumeroFila];
			}
		}
		public string p_PFON_CODIPENS            
		{
			get
			{
				return (string)PFON_CODIPENS[NumeroFila];
			}
		}
		public string p_MEMP_NUMECONTFONDOPENS   
		{
			get
			{
				return (string)MEMP_NUMECONTFONDOPENS[NumeroFila];
			}
		}
		public string p_PFON_CODIPENSVOLU        
		{
			get
			{
				return (string)PFON_CODIPENSVOLU[NumeroFila];
			}
		}
		public string p_PFON_CODICESA            
		{
			get
			{
				return (string)PFON_CODICESA[NumeroFila];
			}
		}
		public string p_MEMP_NUMECONTFONDOCESA
		{
			get
			{
				return (string)MEMP_NUMECONTFONDOCESA[NumeroFila];
			}
		}
		public string p_PARP_CODIARP    
		{
			get
			{
				return (string)PARP_CODIARP[NumeroFila];
			}
		}
		public string p_MEMP_NUMECONTARP     
		{
			get
			{
				return (string)MEMP_NUMECONTARP[NumeroFila];
			}
		}
		public string p_PRIE_CODIRIES         
		{
			get
			{
				return (string)PRIE_CODIRIES[NumeroFila];
			}
		}
		public string p_TRES_DOTACION     
		{
			get
			{
				return (string)TRES_DOTACION[NumeroFila];
			}
		}
		public string p_MEMP_TARJRELOJ           
		{
			get
			{
				return (string)MEMP_TARJRELOJ[NumeroFila];
			}
		}
		public string p_MEMP_NUMPASE            
		{
			get
			{
				return (string)MEMP_NUMPASE[NumeroFila];
			}
		}
		public string p_MEMP_CATEGORIA          
		{
			get
			{
				return (string)MEMP_CATEGORIA[NumeroFila];
			}
		}
		public string p_MEMP_PERIPAGO          
		{
			get
			{
				return (string)MEMP_PERIPAGO[NumeroFila];
			}
		}
		public string p_MEMP_AJUSUELDO         
		{
			get
			{
				return (string)MEMP_AJUSUELDO[NumeroFila];
			}
		}
		public string p_MEMP_FORMPAGO            
		{
			get
			{
				return (string)MEMP_FORMPAGO[NumeroFila];
			}
		}
		public string p_PBAN_CODIGO           
		{
			get
			{
				return (string)PBAN_CODIGO[NumeroFila];
			}
		}
		public string p_MEMP_CODSUCUEMPL        
		{
			get
			{
				return (string)MEMP_CODSUCUEMPL[NumeroFila];
			}
		}
		public string p_MEMP_CUENNOMI       
		{
			get
			{
				return (string)MEMP_CUENNOMI[NumeroFila];
			}
		}
		public string p_MEMP_TESTRETE          
		{
			get
			{
				return (string)MEMP_TESTRETE[NumeroFila];
			}
		}
		public string p_MEMP_VREXCESALUD         
		{
			get
			{
				return (string)MEMP_VREXCESALUD[NumeroFila];
			}
		}
		public string p_MEMP_VREXCEEDUC         
		{
			get
			{
				return (string)MEMP_VREXCEEDUC[NumeroFila];
			}
		}
		public string p_MEMP_VREXCEAFC         
		{
			get
			{
				return (string)MEMP_VREXCEAFC[NumeroFila];
			}
		}
		public string p_MEMP_PORCRETE   
		{
			get
			{
				return (string)MEMP_PORCRETE[NumeroFila];
			}
		}
		public string p_MEMP_VREXCEVIVI       
		{
			get
			{
				return (string)MEMP_VREXCEVIVI[NumeroFila];
			}
		}
		public string p_MEMP_INDCOMIS         
		{
			get
			{
				return (string)MEMP_INDCOMIS[NumeroFila];
			}
		}
		public string p_TTIP_SECUENCIA      
		{
			get
			{
				return (string)TTIP_SECUENCIA[NumeroFila];
			}
		}
        public string p_PCAJ_CODICAJA
        {
            get
            {
                return (string)PCAJ_CODICAJA[NumeroFila];
            }
        }
        public string p_MNIT_NOMBRES     
		{
			get
			{
				return (string)MNIT_NOMBRES[NumeroFila];
			}
		}
		public string p_MNIT_NOMBRE2
		{
			get
			{
				return (string)MNIT_NOMBRE2[NumeroFila];
			}
		}
		public string p_MNIT_APELLIDOS
		{
			get
			{
				return (string)MNIT_APELLIDOS[NumeroFila];
			}
		}
		public string p_MNIT_APELLIDO2
		{
			get
			{
				return (string)MNIT_APELLIDO2[NumeroFila];
			}
		}
		public string p_MNIT_DIRECCION
		{
			get
			{
				return (string)MNIT_DIRECCION[NumeroFila];
			}
		}
		public string p_PCIU_CODIGO
		{
			get
			{
				return (string)PCIU_CODIGO[NumeroFila];
			}
		}
		public string p_MNIT_TELEFONO 
		{
			get
			{
				return (string)MNIT_TELEFONO[NumeroFila];
			}
		}
		public string p_MNIT_CELULAR       
		{
			get
			{
				return (string)MNIT_CELULAR[NumeroFila];
			}
		}
		public string p_MNIT_EMAIL       
		{
			get
			{
				return (string)MNIT_EMAIL[NumeroFila];
			}
		}
		public string p_PCIU_NOMBRE
		{
			get
			{
				return (string)PCIU_NOMBRE[NumeroFila];
			}
		}
		public string p_PCAR_NOMBCARG  
		{
			get
			{
				return (string)PCAR_NOMBCARG[NumeroFila];
			}
		}
		public string p_PDEP_NOMBDPTO
		{
			get
			{
				return (string)PDEP_NOMBDPTO[NumeroFila];
			}
		}

		public bool LiquidoCesantias
		{
			get{return this.liquidoCesantias;}
			set{this.liquidoCesantias=value;}
		}
		
		public bool EsPeriodoLiquidado
		{
			get{return this.esPeriodoLiquidado;}
			set{this.esPeriodoLiquidado=value;}
		}
		
		public bool NoTieneConceptos
		{
			get{return this.noTieneConceptos;}
			set{this.noTieneConceptos=value;}
		}
		
		public String DocuRefe
		{
			get{return this.docurefe;}
			set{this.docurefe=value;}
		}

		public String TipoEvento
		{
			get{return this.tipoEvento;}
			set{this.tipoEvento=value;}
		}

		//JFSC método que carga las suspensiones de un empleado
		//codigoempleado el identificador del empleado
		public DataSet cargarSuspensiones(string codigoempleado)
		{
			string query= @"Select ttip_coditipo, day(msus_hasta-msus_desde)+1 as dia, year(msus_hasta) as anio, month(msus_hasta) as mes, PC.PCON_SIGNOLIQ, msus_desde, msus_hasta
							from DBXSCHEMA.MSUSPLICENCIAS ms, dbxschema.pconceptoNOMINA pc
							where memp_codiempl='" + codigoempleado+"' and ms.pcon_concepto = pc.pcon_CONCEPTO";
			DBFunctions.Request(this.suspensiones,IncludeSchema.NO,query);
            return suspensiones;
		}
		
		//JFSC método que valida si las suspensiones afectan o no al mes en cuestion
		//mes: el mes de la novedad
		//anio: el anño de la novedad
		//dias: los días que se extrajeron de la base de datos
		public int validaSuspension(int mes, int anio,int dias)
		{
			int retorna=0;
			bool termina=false;
			if(this.suspensiones.Tables.Count > 0 && this.suspensiones.Tables[0].Rows.Count >0)
			{
				for(int i=0;i<this.suspensiones.Tables[0].Rows.Count && !termina;i++)
				{
					if(mes!=int.Parse(this.suspensiones.Tables[0].Rows[i]["mes"].ToString()) || anio!=int.Parse(this.suspensiones.Tables[0].Rows[i]["anio"].ToString()))
					{
						retorna=30;
					}
					else
					{
						retorna=dias;
						termina=true;
					}
				}
			}
			if(retorna==0)
			{
				retorna=30;
			}
			return retorna;
		}
	}
}
