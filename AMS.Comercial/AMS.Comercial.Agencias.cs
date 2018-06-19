using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.DBManager;


namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Agencias
	{
		public Agencias()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Trae las agencias del usuario logueado
		public static void TraerAgenciasUsuario(DropDownList ddlAgencia)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlAgencia,"select ma.mag_codigo,ma.mage_nombre "+
				"from DBXSCHEMA.magencia ma, DBXSCHEMA.susuario su, DBXSCHEMA.susuario_transporte_agencia sa "+
				"where su.susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"' and su.susu_codigo=sa.susu_codigo and ma.mag_codigo=sa.mag_codigo order by mage_nombre");
		}
		//Trae los empleados de la agencia 	
		public static void TraerPersonalAgencia(DropDownList ddlEmpleados,int agencia)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlEmpleados,"SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NIT CONCAT ' - ' CONCAT MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' "+
			"CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'')  CONCAT ' - ' CONCAT PCAR_DESCRIPCION AS NOMBRE "+ 
			"from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP,DBXSCHEMA.PCARGOS_TRANSPORTES cp WHERE MP.MAG_CODIGO= "+agencia+" AND MP.MNIT_NIT=MNIT.MNIT_NIT AND MP.PCAR_CODIGO= cp.PCAR_CODIGO;");
		}
		//Trae los empleados de la agencia del cargo	
		public static void TraerEmpleadosAgencia(DropDownList ddlEmpleados,int agencia,int CargoEmpleado)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlEmpleados,"SELECT MNIT.MNIT_NIT AS NIT,MNIT.MNIT_NOMBRES CONCAT ' 'CONCAT coalesce(MNIT.MNIT_NOMBRE2,'') CONCAT ' ' "+
				"CONCAT MNIT.MNIT_APELLIDOS CONCAT ' ' CONCAT coalesce(MNIT.MNIT_APELLIDO2,'') AS NOMBRE "+ 
				"from DBXSCHEMA.MNIT MNIT, DBXSCHEMA.MPERSONAL_AGENCIA_TRANSPORTES MP WHERE MP.MAG_CODIGO= "+agencia+" AND MP.MNIT_NIT=MNIT.MNIT_NIT AND MP.PCAR_CODIGO="+CargoEmpleado+";");
		}
		//Trae las agencias del usuario logueado
		public static void TraerAgenciasPrefijoUsuario(DropDownList ddlAgencia)
		{
			
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlAgencia,"select rtrim(char(ma.mag_codigo)) concat case when prefijo='S' then '|' else '' end,ma.mage_nombre "+
				"from DBXSCHEMA.magencia ma, DBXSCHEMA.susuario su, DBXSCHEMA.susuario_transporte_agencia sa "+
				"where su.susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"' and su.susu_codigo=sa.susu_codigo and ma.mag_codigo=sa.mag_codigo");
		}
		//Trae los cargos existentes
		public static void TraerCargosEmpleados(DropDownList ddlCargo)
		{
			DatasToControls bind = new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlCargo,"Select pcar_codigo,pcar_descripcion from DBXSCHEMA.PCARGOS_TRANSPORTES order by pcar_descripcion;");
		}
		//Trae los Periodos existentes No cerrados
		public static void TraerPeriodo(DropDownList ddlperiodo)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlperiodo,"select numero_periodo "+ "from DBXSCHEMA.PERIODOS_CIERRE_TRANSPORTE ORDER BY NUMERO_PERIODO;");
			//bind.PutDatasIntoDropDownList(ddlperiodo,"select numero_periodo "+ "from DBXSCHEMA.PERIODOS_CIERRE_TRANSPORTE where estado_cierre <> 'C' ORDER BY NUMERO_PERIODO;");
		}
		//Trae las agencias del usuario logueado
		public static void TraerAgencias(DropDownList ddlAgenciaPapeleria)
		{
			DatasToControls bind=new DatasToControls();
			bind.PutDatasIntoDropDownList(ddlAgenciaPapeleria,"select ma.mag_codigo,ma.mage_nombre "+
				"from DBXSCHEMA.magencia ma order by mage_nombre");
				
		}
	}
}
