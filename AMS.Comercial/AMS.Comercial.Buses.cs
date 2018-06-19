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
using AMS.DB;
using AMS.Forms;
using Ajax;
using AMS.DBManager;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Buses
	{
		public Buses()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Despachar un bus
		public static string Despachar(string planilla,string agencia, string placa)
		{
			DatasToControls bind=new DatasToControls();
			string ruta=DBFunctions.SingleData("SELECT MRUT_CODIGO FROM DBXSCHEMA.MPLANILLAVIAJE where mpla_codigo="+planilla+"");
			string numero=DBFunctions.SingleData("SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MPLANILLAVIAJE where mpla_codigo="+planilla+"");
			string ciudad=DBFunctions.SingleData("SELECT PCIU_CODIGO FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");

			//Primera planilla y despacho?(hora_despacho=null)
			bool primerDespacho=DBFunctions.RecordExist(
				"SELECT MVIAJE_NUMERO FROM DBXSCHEMA.MVIAJE "+
				"where mrut_codigo='"+ruta+"' and mviaje_numero="+numero+" and hora_despacho is null;");

			//Ya despachó?
			if(DBFunctions.RecordExist(
				"SELECT FECHA_LIQUIDACION from dbxschema.MPLANILLAVIAJE WHERE FECHA_LIQUIDACION IS NOT NULL AND MPLA_CODIGO="+planilla+";"))
				return("La planilla ya ha sido liquidada.");

			//No ha arribado despues del primer despacho?
			if(!primerDespacho)
				placa=DBFunctions.SingleData("SELECT mcat_placa FROM dbxschema.MVIAJE mv where mrut_codigo='"+ruta+"' and mviaje_numero="+numero+";");
			else{
				if(placa.Length==0)
					return("Debe Modificar el viaje Seleccionando la placa de bus.");}
			//Responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			if(nitResponsable.Length==0)
				return("El usuario actual (responsable) no tiene un NIT asociado.");

			//Validar configuracion
			if(!DBFunctions.RecordExist("SELECT mb.mcat_placa "+
				"FROM dbxschema.MVIAJE mv, dbxschema.mplanillaviaje mp, dbxschema.mbusafiliado mb "+
				"WHERE mp.mpla_codigo="+planilla+" and mp.mviaje_numero=mv.mviaje_numero and mb.mcon_cod=mv.mcon_cod;"))
				return("La configuracion del bus no coincide con la configuracion del viaje original.");

			ArrayList sqlD=new ArrayList();
			//Cambiar estado del bus a 'en ruta'
			sqlD.Add("update dbxschema.mbusafiliado set testa_codigo=4 where mcat_placa='"+placa+"';");
			
			//Cambiar tabla de localizacion
			sqlD.Add("update dbxschema.mbus_localizacion set testa_codigo=4 where mcat_placa='"+placa+"';");
			
			//Actualizar viaje si es el primer despacho: Hora despacho y responsable despacho,estado(en venta)
			int hora=DateTime.Now.Minute+(60*DateTime.Now.Hour);
			if(primerDespacho)
			{
				sqlD.Add("update dbxschema.mviaje mv "+
					"set mv.hora_despacho="+(DateTime.Now.Minute+(DateTime.Now.Hour*60))+", mv.mnit_despachador='"+nitResponsable+"', ESTADO_VIAJE='R', mcat_placa='"+placa+"' "+
					"where mv.mrut_codigo='"+ruta+"' and mviaje_numero="+numero+";");
			}


			//Totalizar, liquidar planilla
			/*
			double totalTiquetes=Convert.ToDouble(DBFunctions.SingleData("Select coalesce(sum(valor_total),0) from DBXSCHEMA.MTIQUETE_VIAJE where TEST_CODIGO='V' AND mpla_codigo="+planilla+";"));
			double totalEncomiendas=Convert.ToDouble(DBFunctions.SingleData("Select coalesce(sum(valor_total),0) from DBXSCHEMA.MENCOMIENDAS where mpla_codigo="+planilla+";"));
			double totalGiros=Convert.ToDouble(DBFunctions.SingleData("Select coalesce(sum(valor_giro),0) from DBXSCHEMA.MGIROS where mpla_codigo="+planilla+";"));
			double totalGastos=Convert.ToDouble(DBFunctions.SingleData("Select coalesce(sum(valor_total_autorizado),0) from DBXSCHEMA.MGASTOS_TRANSPORTES where mpla_codigo="+planilla+";"));
			double totalIngresos=totalGiros+totalEncomiendas+totalTiquetes;
			double totalEgresos=totalGastos;
			*/
			
			sqlD.Add("CALL DBXSCHEMA.ACTUALIZA_PLANILLA("+planilla+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"');");
			sqlD.Add("UPDATE DBXSCHEMA.MPLANILLAVIAJE SET FECHA_LIQUIDACION='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE MPLA_CODIGO="+planilla+";");

			if(!DBFunctions.Transaction(sqlD))
				return(DBFunctions.exceptions);
			return("");
		}
	}
}
