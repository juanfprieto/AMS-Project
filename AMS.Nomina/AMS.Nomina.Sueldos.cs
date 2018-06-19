using System;
using System.Web;
using System.Collections;
using AMS.DB;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Sueldos
	{
		private string mensajes;
		
		public string Mensajes {set{mensajes=value;}get{return mensajes;}}
		
		public Sueldos()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}

        public bool ActSueldoTodos(double newSal, double oldSal, double newSubsidioTra, double oldSubsidioTra, string FechaModificacion)
		{
			bool i=true;
			ArrayList actualizar = new ArrayList();
            actualizar.Add("update dbxschema.mempleado set memp_suelANTER=memp_suelactu,MEMP_FECSUELANTER=MEMP_FECHSUELACTU,MEMP_FECHSUELACTU = '" + FechaModificacion+"', memp_suelactu=" + newSal + " where memp_suelactu=" + oldSal + " and TEST_ESTADO <> '4' ");
			actualizar.Add("update dbxschema.cnomina set cnom_salaminiante="+oldSal+"");
			actualizar.Add("update dbxschema.cnomina set cnom_salaminiactu="+newSal+"");
            actualizar.Add("update dbxschema.cnomina set cnom_SUBSTRANante=" + oldSubsidioTra + "");
            actualizar.Add("update dbxschema.cnomina set cnom_SUBSTRANsactu=" + newSubsidioTra + "");
			if (!DBFunctions.Transaction(actualizar))
			{
				i=false;
				mensajes=DBFunctions.exceptions;

			}
			return i;	
		}


        public bool ActSueldoEmp(string CodEmpleado, double newSal, string FechaModificacion)
		{
			bool i=true;
			ArrayList actualizar = new ArrayList();
            actualizar.Add("update dbxschema.mempleado set memp_suelactu=" + newSal + ", MEMP_FECHSUELACTU = '"+FechaModificacion+"'  where memp_codiempl='" + CodEmpleado + "' ");
			if (!DBFunctions.Transaction(actualizar))
			{
				i=false;
				mensajes=DBFunctions.exceptions;

			}
			return i;	
		}

	}
}
