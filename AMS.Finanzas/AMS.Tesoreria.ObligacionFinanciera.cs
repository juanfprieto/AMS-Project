using AMS.DB;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;

namespace AMS.Finanzas
{
	public class ObligacionFinanciera
	{
		public string cuentaBancaria, numeroObligacion, documento, almacen, tipoCredito, tasaCredito, condicion, detalle, autoriza, numDocumento;
		public DateTime fechaDesembolso;
		public double montoPesos=0,montoDolares=0,montoPagado=0,interesPagado=0,interesCausado=0, tasaInteres=0, montoACuenta, interesCausacion=0;
		public int numCuotas=0;
		public DataTable dtPagos, dtDesembolsos;
		public string error="";

		//Constructor
		public ObligacionFinanciera(string cuentaB,string numOblig, DateTime fechaD, string doc, string numDoc, string alma,
			double mntPesos, double mntDolrs, int numCuota, double mntPagado, double intrPagado, double intrCausado,
			string tipCred, string tsaCred, double tsaIntr, string condi, string dtll, string autor, double mntCuenta,
			DataTable dtPgs, DataTable dtDsmbl)
		{
			cuentaBancaria=cuentaB;
			numeroObligacion=numOblig;
			fechaDesembolso=fechaD;
			documento=doc;
			numDocumento=numDoc;
			almacen=alma;
			montoPesos=mntPesos;
			montoDolares=mntDolrs;
			numCuotas=numCuota;
			montoPagado=mntPagado;
			interesPagado=intrPagado;
			interesCausado=intrCausado;
			tipoCredito=tipCred;
			tasaCredito=tsaCred;
			tasaInteres=tsaIntr;
			condicion=condi;
			detalle=dtll;
			autoriza=autor;
			montoACuenta=mntCuenta;
			dtPagos=dtPgs;
			dtDesembolsos=dtDsmbl;
			error="";
		}

		public ObligacionFinanciera(string cuentaB,string numOblig)
		{
			cuentaBancaria=cuentaB;
			numeroObligacion=numOblig;
			error="";
		}
		public ObligacionFinanciera()
		{
			error="";
		}

		//Causar Interes
		public void CausarInteres(int anoC, int mesC, string tipoDoc, string obserC, string almacen, DataTable dtObligaciones)
		{
			ArrayList arlSql=new ArrayList();
			double saldoC=0, interesC=0, interesF=0;
			string numOblig;
			//LEER PDOCNUMERO
			string numeDoc=DBFunctions.SingleData(
				"SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE PDOC_CODIGO='"+tipoDoc+"';");
			
			arlSql.Add(
				"INSERT INTO MOBLIGACIONFINANCIERACAUSACION VALUES("+
				"'"+tipoDoc+"',"+numeDoc+","+anoC+","+mesC+",'"+obserC+"',CURRENT DATE, "+
				"'"+almacen+"','"+HttpContext.Current.User.Identity.Name.ToLower()+"'"+
				");"
			);
			bool actualiza=false;
			for(int n=0;n<dtObligaciones.Rows.Count;n++)
			{
				//DETALLE
				saldoC=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_SALDO"]);
				interesC=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_INTERESCAUSADO"]);
				interesF=Convert.ToDouble(dtObligaciones.Rows[n]["MOBL_INTERESCALCULADO"]);
				numOblig=dtObligaciones.Rows[n]["MOBL_NUMERO"].ToString();
				if(interesF>0){
					arlSql.Add(
						"INSERT INTO DOBLIGACIONFINANCIERACAUSACION VALUES("+
						"'"+numOblig+"',"+anoC+","+mesC+",'"+tipoDoc+"',"+numeDoc+","+
						saldoC+","+interesC+","+interesF+");");
					//CONTAR INTERES
					arlSql.Add("UPDATE MOBLIGACIONFINANCIERA SET MOBL_INTERESCAUSADO=MOBL_INTERESCAUSADO+"+interesF+" "+
						"WHERE MOBL_NUMERO='"+numOblig+"';");
					actualiza=true;
				}
			}
			
			//ACTUALIZAR PDOCUMENTO
			arlSql.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU=PDOC_ULTIDOCU+1 WHERE PDOC_CODIGO='"+tipoDoc+"';");

			if(actualiza){
				if(!DBFunctions.Transaction(arlSql))
					error=DBFunctions.exceptions;}
			else
				error="No se ingresaron intereses";
		}

		//Guardar la obligacion
		public void GuardarObligacion()
		{
			ArrayList arlSql=new ArrayList();
			numDocumento=DBFunctions.SingleData("SELECT PDOC_ULTIDOCU+1 FROM PDOCUMENTO WHERE PDOC_CODIGO='"+this.documento+"';");
			arlSql.Add("INSERT INTO MOBLIGACIONFINANCIERA VALUES("+
				"'"+cuentaBancaria+"',"+"'"+numeroObligacion+"','"+fechaDesembolso.ToString("yyyy-MM-dd")+"',"+
				"'"+documento+"',"+numDocumento+",'"+almacen+"',"+montoPesos+","+montoDolares+","+numCuotas+","+montoPagado+","+
				interesPagado+","+interesCausado+",'"+tipoCredito+"','"+tasaCredito+"',"+tasaInteres+","+
				"'"+condicion+"','"+detalle+"','"+autoriza+"');");
			arlSql.Add("UPDATE PDOCUMENTO SET PDOC_ULTIDOCU=PDOC_ULTIDOCU+1 WHERE PDOC_CODIGO='"+this.documento+"';");
			GuardarTablasAsociadas(arlSql);
			if(!DBFunctions.Transaction(arlSql))
				error=DBFunctions.exceptions;
		}

		public void ActualizarObligacion()
		{
			ArrayList arlSql=new ArrayList();
			arlSql.Add("UPDATE MOBLIGACIONFINANCIERA SET "+
				"MOBL_FECHA="+"'"+fechaDesembolso.ToString("yyyy-MM-dd")+"',"+
				"PALM_ALMACEN='"+almacen+"',MOBL_MONTPESOS="+montoPesos+","+
				"MOBL_MONDOLARES="+montoDolares+",MOBL_NUMECUOTAS="+numCuotas+","+
				"PCRE_CODIGO='"+tipoCredito+"',PTAS_CODIGO='"+tasaCredito+"',MOBL_TASAINTERES="+tasaInteres+","+
				"TCON_CODIGO='"+condicion+"',MOBL_DETALLE='"+detalle+"',MOBL_AUTORIZA='"+autoriza+"' "+
				"WHERE PCUE_CODIGO='"+cuentaBancaria+"' AND MOBL_NUMERO='"+numeroObligacion+"';");
			GuardarTablasAsociadas(arlSql);
			if(!DBFunctions.Transaction(arlSql))
				error=DBFunctions.exceptions;
		}

		private void GuardarTablasAsociadas(ArrayList arlSql)
		{
			arlSql.Add("DELETE FROM DOBLIGACIONFINANCIERAPLANPAGO WHERE MOBL_NUMERO='"+numeroObligacion+"';");
			for(int n=0;n<dtPagos.Rows.Count;n++)
			{
				arlSql.Add("INSERT INTO DOBLIGACIONFINANCIERAPLANPAGO VALUES("+
					"'"+numeroObligacion+"',"+dtPagos.Rows[n]["DOBL_NUMEPAGO"].ToString()+","+
					"'"+Convert.ToDateTime(dtPagos.Rows[n]["MOBL_FECHA"]).ToString("yyyy-MM-dd")+"',"+
					dtPagos.Rows[n]["MOBL_MONTPESOS"].ToString()+","+
					dtPagos.Rows[n]["MOBL_MONTINTERES"].ToString()+",0,0);");
			}

			arlSql.Add("DELETE FROM DOBLIGACIONFINANCIERADESEMBOLSO WHERE MOBL_NUMERO='"+numeroObligacion+"';");
			for(int n=0;n<dtDesembolsos.Rows.Count;n++)
			{
				arlSql.Add(
					"INSERT INTO DOBLIGACIONFINANCIERADESEMBOLSO VALUES("+
					"'"+numeroObligacion+"',"+(n+1)+",'"+dtDesembolsos.Rows[n]["DOBL_RAZON"].ToString()+"',"+
					"'"+dtDesembolsos.Rows[n]["MCUE_CODIPUC"].ToString()+"',"+
					"'"+dtDesembolsos.Rows[n]["PDOC_CODIDOCUREFE"].ToString()+"',"+
					dtDesembolsos.Rows[n]["DOBL_NUMEDOCUREFE"].ToString()+","+
					"'"+dtDesembolsos.Rows[n]["MNIT_NIT"].ToString()+"',"+
					"'"+dtDesembolsos.Rows[n]["PALM_ALMACEN"].ToString()+"',"+
					"'"+dtDesembolsos.Rows[n]["PCEN_CODIGO"].ToString()+"',"+
					Convert.ToDouble(dtDesembolsos.Rows[n]["DOBL_VALODEBI"]).ToString()+","+
					Convert.ToDouble(dtDesembolsos.Rows[n]["DOBL_VALOCRED"]).ToString()+","+
					Convert.ToDouble(dtDesembolsos.Rows[n]["DOBL_VALOBASE"]).ToString()+
					");");
			}

			arlSql.Add(
                "DELETE FROM DTESORERIAOBLIGACIONFINANCIERA WHERE " +
				"MTES_CODIGO='"+this.documento+"' AND MTES_NUMERO="+this.numDocumento+";");
			arlSql.Add(
				"DELETE FROM MTESORERIASALDOS WHERE "+
				"MTES_CODIGO='"+this.documento+"' AND MTES_NUMERO="+this.numDocumento+";");
			arlSql.Add(
				"DELETE FROM MTESORERIA WHERE "+
				"PDOC_CODIGO='"+this.documento+"' AND MTES_NUMERO="+this.numDocumento+";");

			string detalleA=this.detalle;
			if (detalleA.Length>100)
				detalleA=detalleA.Substring(0,99);
			arlSql.Add(
				"INSERT INTO MTESORERIA VALUES("+
				"'"+this.cuentaBancaria+"','"+this.documento+"',"+this.numDocumento+","+
				"'"+this.almacen+"','"+this.fechaDesembolso.ToString("yyyy-MM-dd")+"',"+montoACuenta+",'"+HttpContext.Current.User.Identity.Name.ToLower()+"',"+
				"'"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"','"+detalleA+"','A');"
			);
			arlSql.Add(
				"INSERT INTO MTESORERIASALDOS VALUES(DEFAULT,"+
				"'"+this.documento+"',"+this.numDocumento+",'"+this.cuentaBancaria+"',"+
				montoACuenta+",0);");
			arlSql.Add(
				"INSERT INTO DTESORERIAOBLIGACIONFINANCIERA VALUES("+
				"'"+this.documento+"',"+this.numDocumento+",'"+this.cuentaBancaria+"','E',"+
				montoACuenta+");");
		}

		public void ConsultarObligacion()
		{
			DataSet dsObligacion=new DataSet();
			DataTable dtObligacion;
			DBFunctions.Request(dsObligacion,IncludeSchema.NO,
				"SELECT * FROM MOBLIGACIONFINANCIERA WHERE "+
				"MOBL_NUMERO='"+numeroObligacion+"';"+
				"SELECT D.*, cast(0 as decimal) MOBL_MONTCAPI FROM DOBLIGACIONFINANCIERAPLANPAGO D WHERE "+
				"MOBL_NUMERO='"+numeroObligacion+"';"+
				"SELECT * FROM DOBLIGACIONFINANCIERADESEMBOLSO WHERE "+
				"MOBL_NUMERO='"+numeroObligacion+"';");
			if(dsObligacion.Tables.Count==0)
				return;
			dtObligacion=dsObligacion.Tables[0];
			
			fechaDesembolso=Convert.ToDateTime(dtObligacion.Rows[0]["MOBL_FECHA"]);
			documento=dtObligacion.Rows[0]["PDOC_CODIGO"].ToString();
			numDocumento=dtObligacion.Rows[0]["MOBL_NUMEDOCU"].ToString();
			almacen=dtObligacion.Rows[0]["PALM_ALMACEN"].ToString();
			montoPesos=Convert.ToDouble(dtObligacion.Rows[0]["MOBL_MONTPESOS"]);
			montoDolares=Convert.ToDouble(dtObligacion.Rows[0]["MOBL_MONDOLARES"]);
			numCuotas=Convert.ToInt16(dtObligacion.Rows[0]["MOBL_NUMECUOTAS"]);
			montoPagado=Convert.ToDouble(dtObligacion.Rows[0]["MOBL_MONTPAGADO"]);
			interesPagado=Convert.ToDouble(dtObligacion.Rows[0]["MOBL_INTERESPAGADO"]);
			interesCausado=Convert.ToDouble(dtObligacion.Rows[0]["MOBL_INTERESCAUSADO"]);
			tipoCredito=dtObligacion.Rows[0]["PCRE_CODIGO"].ToString();
			tasaCredito=dtObligacion.Rows[0]["PTAS_CODIGO"].ToString();
			tasaInteres=Convert.ToDouble(dtObligacion.Rows[0]["MOBL_TASAINTERES"]);
			condicion=dtObligacion.Rows[0]["TCON_CODIGO"].ToString();
			detalle=dtObligacion.Rows[0]["MOBL_DETALLE"].ToString();
			autoriza=dtObligacion.Rows[0]["MOBL_AUTORIZA"].ToString();

			try{
				montoACuenta=Convert.ToDouble(DBFunctions.SingleData(
					"SELECT MTES_CREDITOS FROM MTESORERIA WHERE "+
					"PDOC_CODIGO='"+this.documento+"' AND MTES_NUMERO="+this.numDocumento+";"));}
			catch{montoACuenta=0;}

			dtPagos=dsObligacion.Tables[1];
			dtDesembolsos=dsObligacion.Tables[2];
		}
	}
}
