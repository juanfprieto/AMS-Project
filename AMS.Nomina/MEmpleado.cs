using System;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de MEmpleado.
	/// </summary>
	public class MEmpleado
	{
		//Constantes- Nombres de campos en la base de datos
		public const string CODIGOEMPLEADO="MEMP_CODIEMPL";
		public const string NIT="MNIT_NIT";
		public const string FECHANACIMIENTO="MEMP_FECHNACI";
		public const string LUGARNACIMIENTO="PCIU_LUGANACI";
		public const string ALMACEN="PALM_ALMACEN";
		public const string CODIGODEPTO="PDEP_CODIDEPTO";
		public const string ESTADO="TEST_ESTADO";
		public const string SALARIO="TSAL_SALARIO";
		public const string CONTRATO="TCON_CONTRATO";
		public const string CODICARGO="PCAR_CODICARGO";
		public const string FECHAINGRESO="MEMP_FECHINGRESO";
		public const string FECHAFINCONTRATO="MEMP_FECHFINCONTRATO";
		public const string FECHARETIRO="MEMP_FECHRETIRO";
		public const string UNFAMILIAR="MEMP_UNFAMILIAR";
		public const string CODIGOPROFESION="PESP_CODIGO";
		public const string VIVIENDA="TRES_VIVIENDA";
		public const string TARJETAPROFESIONAL="MEMP_TARJPROFES";
		public const string DISTRITOMILITAR="MEMP_DISTLIBRMILI";
		public const string CLASEMILITAR="MEMP_CLASELIBRMILI";
		public const string NUMEROMILITAR="MEMP_NUMELIBRMILI";
		public const string SEXO="TSEX_CODIGO";
		public const string ESTADOCIVIL="TEST_ESTACIVIL";
		public const string NUMEROHIJOS="MEMP_NUMEHIJOS";
		public const string PERSONASCARGO="MEMP_PERSCARGO";
		public const string SUELDOACTUAL="MEMP_SUELACTU";
		public const string FECHASUELDOACTUAL="MEMP_FECHSUELACTU";
		public const string SUELDOANTERIOR="MEMP_SUELANTER";
		public const string FECHASUELDOANTERIOR="MEMP_FECSUELANTER";
		public const string SALARIOPROMEDIO="MEMP_SALAPROMEDIO";
		public const string SUBSIDIOTRANSPORTE="TSUB_CODIGO";
		public const string EPS="PEPS_CODIEPS";
		public const string NUMEROAFILIACIONEPS="MEMP_NUMEAFILEPS";
		public const string CODIGOPENSION="PFON_CODIPENS";
		public const string NUMEROFONDOPENSIONES="PFON_NUMECONTFONDOPENS";
		public const string FONDOPENSIONVOLUNTARIO="PFON_CODICESA";
		public const string CODFONDOCESANTIAS="MEMP_NUMECONTFONDOCESA";
		public const string NUMEROCESANTIAS="PARP_CODIARP";
		public const string CODARP="PRIE_CODIRIES";
		public const string NUMEROARP="MEMP_NUMECONTARP";
		public const string RIESGOPROFESIONAL="PRIE_CODIRIES";
		public const string DOTACION="TRES_DOTACION";
		public const string NUMTARJETARELOJ="MEMP_TARJRELOJ";
		public const string PASE="MEMP_NUMPASE";
		public const string CATEGORIAPASE="MEMP_CATEGORIA";
		public const string PERIODOPAGO="MEMP_PERIPAGO";
		public const string AJUSTESUELDO="MEMP_AJUSUELDO";
		public const string FORMAPAGO="MEMP_FORMPAGO";
		public const string CODIGOBANCO="PBAN_CODIGO";
		public const string CODIGOSUCURSAL="MEMP_CODSUCUEMPL";
		public const string CUENTANOMINA="MEMP_CUENNOMI";
		public const string RETEFUENTE="MEMP_TESTRETE";
		public const string VALOREXCSALUD="MEMP_VREXCESALUD";
		public const string VALOREXCEDUCACION="MEMP_VREXCEEDUC";
		public const string VALOREXCAFC="MEMP_VREXCEAFC";
		public const string PORCRETEFUENTE="MEMP_PORCRETE";
		public const string VALOREXCVIVIENDA="MEMP_VREXCEVIVI";
		public const string INDCOMISIONES="MEMP_INDCOMIS";
		public const string TIPOSANGRE="TTIP_SECUENCIA";


		//Atributos
		private string codigoEmpleado;
		private string nit;
		private DateTime fechaNacimiento;
		private string lugarNacimiento;
		private string almacen;
		private string codigoDepto;
		private string estado;
		private string salario;
		private string contrato;
		private string codiCargo;
		private DateTime fechaIngreso;
		private DateTime fechaFinContrato;
		private DateTime fechaRetiro;
		private string unFamiliar;
		private string codigoProfesion;
		private string vivienda;
		private string tarjetaProfesional;
		private string distritoMilitar;
		private string claseMilitar;
		private string numeroMilitar;
		private string sexo;
		private string estadoCivil;
		private int numeroHijos;
		private int personasCargo;
		private double sueldoActual;
		private DateTime fechaSueldoActual;
		private double sueldoAnterior;
		private DateTime fechaSueldoAnterior;
		private double salarioPromedio;
		private string subsidioTransporte;
		private string eps;
		private int numeroAfiliacionEPS;
		private string codigoPension;
		private int numeroFondoPensiones;
		private string fondoPensionVoluntario;
		private string codFondoCesantias;
		private int numeroCesantias;
		private string codARP;
		private int numeroARP;
		private string riesgoProfesional;
		private string dotacion;
		private string numTarjetaReloj;
		private string pase;
		private string categoriaPase;
		private int periodoPago;
		private string ajusteSueldo;
		private string formaPago;
		private string codigoBanco;
		private string codigoSucursal;
		private string cuentaNomina;
		private string retefuente;
		private int valorExcSalud;
		private int valorExcEducacion;
		private int valorExcAFC;
		private double porcRetefuente;
		private int valorExcVivienda;
		private string indComisiones;
		private int tipoSangre;

		
		public MEmpleado()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

		public string CodigoEmpleado
		{
			get
			{
				return this.codigoEmpleado;
			}
			set
			{
				this.codigoEmpleado=value;
			}
		}
		
		public string Nit
		{
			get
			{
				return this.nit;
			}
			set
			{
				this.nit=value;
			}
		}
       
		public DateTime FechaNacimiento 
   
		{
			get
              
			{
				return this.fechaNacimiento;
			}
			set
			{
				this.fechaNacimiento=value;
			}
		}
         
		public string LugarNacimiento 
		{
			get
			{
    			return this.lugarNacimiento;
			}
			set
			{
				this.lugarNacimiento=value;
			}
		}

		public string Almacen
		{
			get
			{
				return this.almacen;
			}
			set
			{
				this.almacen=value;
			}
		}

		public string CodigoDepto
		{
			get
			{
				return this.codigoDepto;
			}
			set
			{
				this.codigoDepto=value;
			}
		}

		public string Estado
		{
			get
			{
				return this.estado;
			}
			set
			{
				this.estado=value;
			}
		}

		public string Salario
		{
			get
			{
				return this.salario;
			}
			set
			{
				this.salario=value;
			}
		}

		public string Contrato
		{
			get
			{
				return this.contrato;
			}
			set
			{
				this.contrato=value;
			}
		}

		public string CodiCargo 
		{
			get
			{
				return this.codiCargo;
			}
			set
			{
				this.codiCargo=value;
			}
		}

		public DateTime FechaIngreso 
		{
			get
			{
				return this.fechaIngreso;
			}
			set
			{
				this.fechaIngreso=value;
			}
		}

		public DateTime FechaFinContrato
		{
			get
			{
				return this.fechaFinContrato;
			}
			set
			{
				this.fechaFinContrato=value;
			}
		}

		public DateTime FechaRetiro
		{
			get
			{
				return this.fechaRetiro;
			}
			set
			{
				this.fechaRetiro=value;
			}
		}

		public string UnFamiliar
		{
			get
			{
				return this.unFamiliar;
			}
			set
			{
				this.unFamiliar=value;
			}
		}

		public string CodigoProfesion
		{
			get
			{
				return this.codigoProfesion;
			}
			set
			{
				this.codigoProfesion=value;
			}
		}

		public string Vivienda
		{
			get
			{
				return this.vivienda;
			}
			set
			{
				this.vivienda=value;
			}
		}

		public string TarjetaProfesional
		{
			get
			{
				return this.tarjetaProfesional;
			}
			set
			{
				this.tarjetaProfesional=value;
			}
		}

		public string DistritoMilitar
		{
			get
			{
				return this.distritoMilitar;
			}
			set
			{
				this.distritoMilitar=value;
			}
		}

		public string ClaseMilitar
		{
			get
			{
				return this.claseMilitar;
			}
			set
			{
				this.claseMilitar=value;
			}
		}

		public string NumeroMilitar
		{
			get
			{
				return this.numeroMilitar;
			}
			set
			{
				this.numeroMilitar=value;
			}
		}

		public string Sexo
		{
			get
			{
				return this.sexo;
			}
			set
			{
				this.sexo=value;
			}
		}

		public string EstadoCivil
		{
			get
			{
				return this.estadoCivil;
			}
			set
			{
				this.estadoCivil=value;
			}
		}

		public int NumeroHijos 
		{
			get
			{
				return this.numeroHijos;
			}
			set
			{
				this.numeroHijos=value;
			}
		}

		public int PersonasCargo
		{
			get
			{
				return this.personasCargo;
			}
			set
			{
				this.personasCargo=value;
			}
		}

		public double SueldoActual
		{
			get
			{
				return this.sueldoActual;
			}
			set
			{
				this.sueldoActual=value;
			}
		}

		public DateTime FechaSueldoActual 
		{
			get
			{
				return this.fechaSueldoActual;
			}
			set
			{
				this.fechaSueldoActual=value;
			}
		}

		public double SueldoAnterior 
		{
			get
			{
				return this.sueldoAnterior;
			}
			set
			{
				this.sueldoAnterior=value;
			}
		}

		public DateTime FechaSueldoAnterior 
		{
			get
			{
				return this.fechaSueldoAnterior;
			}
			set
			{
				this.fechaSueldoAnterior=value;
			}
		}

		public double SalarioPromedio 
		{
			get
			{
				return this.salarioPromedio;
			}
			set
			{
				this.salarioPromedio=value;
			}
		}

		public string SubsidioTransporte
		{
			get
			{
				return this.subsidioTransporte;
			}
			set
			{
				this.subsidioTransporte=value;
			}
		}

		public string Eps
		{
			get
			{
				return this.eps;
			}
			set
			{
				this.eps=value;
			}
		}

		public int NumeroAfiliacionesEPS 
		{
			get
			{
				return this.numeroAfiliacionEPS;
			}
			set
			{
				this.numeroAfiliacionEPS=value;
			}
		}

		public string CodigoPension
		{
			get
			{
				return this.codigoPension;
			}
			set
			{
				this.codigoPension=value;
			}
		}

		public int NumeroFondoPensiones
		{
			get
			{
				return this.numeroFondoPensiones;
			}
			set
			{
				this.numeroFondoPensiones=value;
			}
		}

		public string FondoPensionVoluntario
		{
			get
			{
				return this.fondoPensionVoluntario;
			}
			set
			{
				this.fondoPensionVoluntario=value;
			}
		}

		public string CodFondoCesantias
		{
			get
			{
				return this.codFondoCesantias;
			}
			set
			{
				this.codFondoCesantias=value;
			}
		}

		public int NumeroCesantias
		{
			get
			{
				return this.numeroCesantias;
			}
			set
			{
				this.numeroCesantias=value;
			}
		}

		public string CodARP
		{
			get
			{
				return this.codARP;
			}
			set
			{
				this.codARP=value;
			}
		}

		public int NumeroARP 
		{
			get
			{
				return this.numeroARP;
			}
			set
			{
				this.numeroARP=value;
			}
		}

		public string RiesgoProfesional
		{
			get
			{
				return this.riesgoProfesional;
			}
			set
			{
				this.riesgoProfesional=value;
			}
		}

		public string Dotacion
		{
			get
			{
				return this.dotacion;
			}
			set
			{
				this.dotacion=value;
			}
		}

		public string NumTarjetaReloj 
		{
			get
			{
				return this.numTarjetaReloj;
			}
			set
			{
				this.numTarjetaReloj=value;
			}
		}

		public string Pase
		{
			get
			{
				return this.pase;
			}
			set
			{
				this.pase=value;
			}
		}

		public string CategoriaPase
		{
			get
			{
				return this.categoriaPase;
			}
			set
			{
				this.categoriaPase=value;
			}
		}

		public int PeriodoPago 
		{
			get
			{
				return this.periodoPago;
			}
			set
			{
				this.periodoPago=value;
			}
		}

		public string AjusteSueldo
		{
			get
			{
				return this.ajusteSueldo;
			}
			set
			{
				this.ajusteSueldo=value;
			}
		}

		public string FormaPago
		{
			get
			{
				return this.formaPago;
			}
			set
			{
				formaPago=value;
			}
		}

		public string CodigoBanco
		{
			get
			{
				return this.codigoBanco;
			}
			set
			{
				this.codigoBanco=value;
			}
		}

		public string CodigoSucursal
		{
			get
			{
				return this.codigoSucursal;
			}
			set
			{
				this.codigoSucursal=value;
			}
		}

		public string CuentaNomina
		{
			get
			{
				return this.cuentaNomina;
			}
			set
			{
				this.cuentaNomina=value;
			}
		}

		public string Retefuente
		{
			get
			{
				return this.retefuente;
			}
			set
			{
				this.retefuente=value;
			}
		}

		public int ValorExcSalud 
		{
			get
			{
				return this.valorExcSalud;
			}
			set
			{
				this.valorExcSalud=value;
			}
		}

		public int ValorExcEducacion 
		{
			get
			{
				return this.valorExcEducacion;
			}
			set
			{
				this.valorExcEducacion=value;
			}
		}

		public int ValorExcAFC 
		{
			get
			{
				return this.valorExcAFC;
			}
			set
			{
				this.valorExcAFC=value;
			}
		}

		public double PorcRetefuente
		{
			get
			{
				return this.porcRetefuente;
			}
			set
			{
				this.porcRetefuente=value;
			}
		}

		public int ValorExcVivienda 
		{
			get
			{
				return this.valorExcVivienda;
			}
			set
			{
				this.valorExcVivienda=value;
			}
		}

		public string IndComisiones
		{
			get
			{
				return this.indComisiones;
			}
			set
			{
				this.indComisiones=value;
			}
		}

		public int TipoSangre 
		{
			get
			{
				return this.tipoSangre;
			}
			set
			{
				this.tipoSangre=value;
			}
		}


	}
}
