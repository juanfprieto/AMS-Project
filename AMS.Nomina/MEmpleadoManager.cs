using System;
using AMS.DB;
using AMS.DBManager;
using System.Data;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de MEmpleadoManager.
	/// </summary>
	public class MEmpleadoManager
	{
		private MEmpleado empleado;
		
		public MEmpleadoManager()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		
		//Método que carga todos los empleados presentes en la tabla de empleados
		public static MEmpleado[] cargarEmpleados()
		{
			MEmpleado[] retorno;
			//MEmpleado empleadoProv;
			DataSet empleados=new DataSet();
			DBFunctions.Request(empleados,IncludeSchema.NO,"Select * from dbxschema.mempleado");
			if(empleados.Tables[0].Rows.Count!=0)
			{
				retorno=new MEmpleado[empleados.Tables[0].Rows.Count];
				for(int i=0;i<empleados.Tables[0].Rows.Count;i++)
				{
					retorno[i]=new MEmpleado();
					retorno[i].AjusteSueldo=empleados.Tables[0].Rows[i][MEmpleado.AJUSTESUELDO].ToString();
					retorno[i].Almacen=empleados.Tables[0].Rows[i][MEmpleado.ALMACEN].ToString();
					retorno[i].CategoriaPase=empleados.Tables[0].Rows[i][MEmpleado.CATEGORIAPASE].ToString();
					retorno[i].ClaseMilitar=empleados.Tables[0].Rows[i][MEmpleado.CLASEMILITAR].ToString();
					retorno[i].CodARP=empleados.Tables[0].Rows[i][MEmpleado.CODARP].ToString();
					retorno[i].CodFondoCesantias=empleados.Tables[0].Rows[i][MEmpleado.CODFONDOCESANTIAS].ToString();
					retorno[i].CodiCargo=empleados.Tables[0].Rows[i][MEmpleado.CODICARGO].ToString();
					retorno[i].CodigoBanco=empleados.Tables[0].Rows[i][MEmpleado.CODIGOBANCO].ToString();
					retorno[i].CodigoDepto=empleados.Tables[0].Rows[i][MEmpleado.CODIGODEPTO].ToString();
					retorno[i].CodigoEmpleado=empleados.Tables[0].Rows[i][MEmpleado.CODIGOEMPLEADO].ToString();
					retorno[i].CodigoPension=empleados.Tables[0].Rows[i][MEmpleado.CODIGOPENSION].ToString();
					retorno[i].CodigoProfesion=empleados.Tables[0].Rows[i][MEmpleado.CODIGOPROFESION].ToString();
					retorno[i].CodigoSucursal=empleados.Tables[0].Rows[i][MEmpleado.CODIGOSUCURSAL].ToString();
					retorno[i].Contrato=empleados.Tables[0].Rows[i][MEmpleado.CONTRATO].ToString();
					retorno[i].CuentaNomina=empleados.Tables[0].Rows[i][MEmpleado.CUENTANOMINA].ToString();
					retorno[i].DistritoMilitar=empleados.Tables[0].Rows[i][MEmpleado.DISTRITOMILITAR].ToString();
					retorno[i].Dotacion=empleados.Tables[0].Rows[i][MEmpleado.DOTACION].ToString();
					retorno[i].Eps=empleados.Tables[0].Rows[i][MEmpleado.EPS].ToString();
					retorno[i].Estado=empleados.Tables[0].Rows[i][MEmpleado.ESTADO].ToString();
					retorno[i].EstadoCivil=empleados.Tables[0].Rows[i][MEmpleado.ESTADOCIVIL].ToString();
					retorno[i].FechaFinContrato=Convert.ToDateTime(empleados.Tables[0].Rows[i][MEmpleado.FECHAFINCONTRATO].ToString());
					retorno[i].FechaIngreso=Convert.ToDateTime(empleados.Tables[0].Rows[i][MEmpleado.FECHAINGRESO].ToString());
					retorno[i].FechaNacimiento=Convert.ToDateTime(empleados.Tables[0].Rows[i][MEmpleado.FECHANACIMIENTO].ToString());
					retorno[i].FechaRetiro=Convert.ToDateTime(empleados.Tables[0].Rows[i][MEmpleado.FECHARETIRO].ToString());
					retorno[i].FechaSueldoActual=Convert.ToDateTime(empleados.Tables[0].Rows[i][MEmpleado.FECHASUELDOACTUAL].ToString());
					retorno[i].FechaSueldoAnterior=Convert.ToDateTime(empleados.Tables[0].Rows[i][MEmpleado.FECHASUELDOANTERIOR].ToString());
					retorno[i].FondoPensionVoluntario=empleados.Tables[0].Rows[i][MEmpleado.FONDOPENSIONVOLUNTARIO].ToString();
					retorno[i].FormaPago=empleados.Tables[0].Rows[i][MEmpleado.FORMAPAGO].ToString();
					retorno[i].IndComisiones=empleados.Tables[0].Rows[i][MEmpleado.INDCOMISIONES].ToString();
					retorno[i].LugarNacimiento=empleados.Tables[0].Rows[i][MEmpleado.LUGARNACIMIENTO].ToString();
					retorno[i].Nit=empleados.Tables[0].Rows[i][MEmpleado.NIT].ToString();
					retorno[i].NumeroAfiliacionesEPS=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.NUMEROAFILIACIONEPS].ToString());
					retorno[i].NumeroARP=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.NUMEROARP].ToString());
					retorno[i].NumeroCesantias=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.NUMEROCESANTIAS].ToString());
					retorno[i].NumeroFondoPensiones=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.NUMEROFONDOPENSIONES].ToString());
					retorno[i].NumeroHijos=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.NUMEROHIJOS].ToString());
					retorno[i].NumeroMilitar=empleados.Tables[0].Rows[i][MEmpleado.NUMEROMILITAR].ToString();
					retorno[i].NumTarjetaReloj=empleados.Tables[0].Rows[i][MEmpleado.NUMTARJETARELOJ].ToString();
					retorno[i].Pase=empleados.Tables[0].Rows[i][MEmpleado.PASE].ToString();
					retorno[i].PeriodoPago=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.PERIODOPAGO].ToString());
					retorno[i].PersonasCargo=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.PERSONASCARGO].ToString());
					retorno[i].PorcRetefuente=Convert.ToDouble(empleados.Tables[0].Rows[i][MEmpleado.PORCRETEFUENTE].ToString());
					retorno[i].Retefuente=empleados.Tables[0].Rows[i][MEmpleado.RETEFUENTE].ToString();
					retorno[i].RiesgoProfesional=empleados.Tables[0].Rows[i][MEmpleado.RIESGOPROFESIONAL].ToString();
					retorno[i].Salario=empleados.Tables[0].Rows[i][MEmpleado.SALARIO].ToString();
					retorno[i].SalarioPromedio=Convert.ToDouble(empleados.Tables[0].Rows[i][MEmpleado.SALARIOPROMEDIO].ToString());
					retorno[i].Sexo=empleados.Tables[0].Rows[i][MEmpleado.SEXO].ToString();
					retorno[i].SubsidioTransporte=empleados.Tables[0].Rows[i][MEmpleado.SUBSIDIOTRANSPORTE].ToString();
					retorno[i].SueldoActual=Convert.ToDouble(empleados.Tables[0].Rows[i][MEmpleado.SUELDOACTUAL].ToString());
					retorno[i].SueldoAnterior=Convert.ToDouble(empleados.Tables[0].Rows[i][MEmpleado.SUELDOANTERIOR].ToString());
					retorno[i].TarjetaProfesional=empleados.Tables[0].Rows[i][MEmpleado.TARJETAPROFESIONAL].ToString();
					retorno[i].TipoSangre=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.TIPOSANGRE].ToString());
					retorno[i].UnFamiliar=empleados.Tables[0].Rows[i][MEmpleado.UNFAMILIAR].ToString();
					retorno[i].ValorExcAFC=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.VALOREXCAFC].ToString());
					retorno[i].ValorExcEducacion=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.VALOREXCEDUCACION].ToString());
					retorno[i].ValorExcSalud=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.VALOREXCSALUD].ToString());
					retorno[i].ValorExcVivienda=Int32.Parse(empleados.Tables[0].Rows[i][MEmpleado.VALOREXCVIVIENDA].ToString());
					retorno[i].Vivienda=empleados.Tables[0].Rows[i][MEmpleado.VIVIENDA].ToString();					
				}
			}
			else
			{
				retorno=new MEmpleado[0];
			}
			return retorno;
		}

		public MEmpleado Empleado 
		{
			get
			{
				return this.empleado;
			}
			set
			{
				this.empleado=value;
			}
		}
	}
}
