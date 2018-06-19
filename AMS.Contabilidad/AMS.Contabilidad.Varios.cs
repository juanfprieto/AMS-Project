using System;
using System.Data;

using AMS.DB;
using AMS.Forms;


namespace AMS.Contabilidad
{
	

	public class Varios
	{
		private DatasToControls DTC = new DatasToControls();

		public Varios()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		public void llenarListaEmpleados (System.Web.UI.WebControls.DropDownList LB)
		{
            DTC.PutDatasIntoDropDownList(LB, "SELECT M.MEMP_CODIEMPL, M.MEMP_CODIEMPL CONCAT ' ' CONCAT N.MNIT_APELLIDOS || ' ' || coalesce(MNIT_apellido2,'') CONCAT ' ' CONCAT N.MNIT_NOMBRES || ' ' || coalesce(MNIT_NOMBRE2,'') FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT and M.test_estado='1' order by M.MEMP_CODIEMPL");
		}

		public void llenarListaAños (System.Web.UI.WebControls.DropDownList LB,string ano)
		{
			DTC.PutDatasIntoDropDownList(LB,"SELECT PANO_ANO,PANO_DETALLE FROM PANO ORDER BY 1 DESC");
			DatasToControls.EstablecerDefectoDropDownList(LB,ano);
		}

		public void llenarListaMeses (System.Web.UI.WebControls.DropDownList LB,string mes)
		{
			DTC.PutDatasIntoDropDownList(LB,"Select TMES_MES, TMES_NOMBRE from TMES ORDER BY 1");
			DatasToControls.EstablecerDefectoDropDownList(LB,mes);
		}

		public void llenarListaDias (System.Web.UI.WebControls.DropDownList LB,string dia)
		{
			DTC.PutDatasIntoDropDownList(LB,"Select TDIA_DIA, TDIA_NOMBRE from TDIA ORDER BY 1");
			DatasToControls.EstablecerDefectoDropDownList(LB,dia);
		}

		public void llenarMotivoRetiro (System.Web.UI.WebControls.DropDownList LB)
		{
			DTC.PutDatasIntoDropDownList(LB,"Select pmot_estado,pmot_descripcion from PMOTIVORETIRO");
		}

		public bool verificarcuenta(string cuenta)
		{
			bool retorno = false;
			string valorretorno = DBFunctions.SingleData("select mcue_codipuc from mcuenta where mcue_codipuc  = " + cuenta);
			if (valorretorno != "")
			{
				retorno = true;
			}
			else
			{
				retorno = false;
			}
			return retorno; 
		}

		public bool verificarnit(string nit)
		{
			bool retorno = false; 
			string valorretorno = DBFunctions.SingleData("select mnit_nit from mnit where mnit_nit = '" + nit + "'");
			if (valorretorno != "")
			{
				retorno = true;
			}
			else
			{
				retorno = false;
			}
			return retorno; 
		}
	}
}
