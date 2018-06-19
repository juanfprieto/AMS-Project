using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
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
	public class Planillas
	{
		public Planillas()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Devuelve siguiente planilla virtual arranca en 1000000 hasta 2.999.999
		public static string TraerSiguientePlanillaVirtual()
		{
            string sqlC = "SELECT MPLA_CODIGO+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO<" + ((long)(Math.Pow(10, AMS.Comercial.Tiquetes.lenTiquete)) * 2).ToString() + " AND MPLA_CODIGO>=" + ((long)Math.Pow(10, AMS.Comercial.Tiquetes.lenTiquete)).ToString() + " ORDER BY MPLA_CODIGO DESC FETCH FIRST 1 ROWS ONLY;";
            string planilla=DBFunctions.SingleData("SELECT MPLA_CODIGO+1 FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO<"+((long)(Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete))*2).ToString()+" AND MPLA_CODIGO>="+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+" ORDER BY MPLA_CODIGO DESC FETCH FIRST 1 ROWS ONLY;");
			if(planilla.Length==0)planilla=((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)+1).ToString();
			return(planilla);
		}
		//Generar txt de la planilla
		public static void GenerarPlanilla(string numero)
		{
			/*string pathTiquetes = ConfigurationManager.AppSettings["PathToPapeleria"];
			FileInfo t = new FileInfo(pathTiquetes + "\\PLA_" + numero + ".txt");
			StreamWriter Tex = new StreamWriter(t.FullName,false,Encoding.UTF8);

			DataSet dsPlanilla=new DataSet();
			DataSet dsViaje=new DataSet();
			DataSet dsInfoTiquete=new DataSet();

			//Info general
			DBFunctions.Request(dsInfoTiquete, IncludeSchema.NO, "select * from dbxschema.ccampos_tiquete;");

			//Planilla
			DBFunctions.Request(dsPlanilla,IncludeSchema.NO, "SELECT *  FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero+";");
			if(dsPlanilla.Tables[0].Rows.Count==0)return;
			
			string agencia=dsPlanilla.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			string rutaP=dsPlanilla.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();
			string viaje=dsPlanilla.Tables[0].Rows[0]["MVIAJE_NUMERO"].ToString();
			
			DBFunctions.Request(dsViaje,IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MVIAJE WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
			if(dsViaje.Tables[0].Rows.Count==0)return;

			int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"]);
			string placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			string numVehiculo=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE	MCAT_PLACA='"+placa+"';");

			Tex.WriteLine(new String('*',anchoTiquete));
			if(dsInfoTiquete.Tables[0].Rows.Count>0)
			{
				Tex.WriteLine(Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["NOMBRE_EMPRESA"].ToString(),'*'));
				Tex.WriteLine(Tiquetes.CentrarTexto("NIT. "+dsInfoTiquete.Tables[0].Rows[0]["NIT_EMPRESA"].ToString(),'*'));}
			Tex.WriteLine(new String('*',anchoTiquete));
			Tex.WriteLine("");

			Tex.WriteLine("Número:   "+dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString());
			Tex.WriteLine("Fecha:    "+DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
			Tex.WriteLine("Bus:      "+numVehiculo+" ("+placa+")");
			Tex.WriteLine("Origen :  "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";"));
			Tex.WriteLine("Destino:  "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';"));
			Tex.WriteLine("Agencia:  "+DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";"));
			Tex.WriteLine("Salida:   "+Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" "+Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00"));
			Tex.WriteLine(Tiquetes.CortarTexto("Conductor:"+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';")));
			Tex.WriteLine(Tiquetes.CortarTexto("Relevador:"+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';")));
			
			string espacio="    ";
			string num="";
			//Tiquetes
			DataSet dsTiquetes=new DataSet();
			DBFunctions.Request(dsTiquetes,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.NUMERO_PUESTOS,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+numero+";");
			
			if(dsTiquetes.Tables[0].Rows.Count>0){
				Tex.WriteLine("");
				Tex.WriteLine("TIQUETES");
				Tex.WriteLine("--------");}
			foreach (DataRow dr in dsTiquetes.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				Tex.WriteLine(num+espacio+dr["DESTINO"].ToString());
				num=dr["NUMERO_PUESTOS"].ToString();
				num=new string(' ',Tiquetes.lenTiquete-num.Length)+num;
				Tex.WriteLine(num+espacio+dr["VALOR_PASAJE"].ToString());
				Tex.WriteLine(new string(' ',Tiquetes.lenTiquete)+espacio+dr["VALOR_TOTAL"].ToString());
			}

			//Tiquetes Prepago
			DataSet dsTiquetesPre=new DataSet();
			DBFunctions.Request(dsTiquetesPre,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE_PREPAGO MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+numero+";");
			if(dsTiquetesPre.Tables[0].Rows.Count>0){
				Tex.WriteLine("");
				Tex.WriteLine("TIQUETES PREPAGO");
				Tex.WriteLine("-------- -------");}
			foreach (DataRow dr in dsTiquetesPre.Tables[0].Rows){
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				Tex.WriteLine(num+espacio+dr["DESTINO"].ToString());
				num=new string(' ',Tiquetes.lenTiquete);
				Tex.WriteLine(num+espacio+dr["VALOR_PASAJE"].ToString());
				Tex.WriteLine(num+espacio+dr["VALOR_TOTAL"].ToString());
			}

			//Encomiendas
			DataSet dsEncomiendas=new DataSet();
			DBFunctions.Request(dsEncomiendas,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_ENCOMIENDA,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MENCOMIENDAS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+numero+";");
			if(dsEncomiendas.Tables[0].Rows.Count>0){
				Tex.WriteLine("");
				Tex.WriteLine("ENCOMIENDAS");
				Tex.WriteLine("-----------");}
			foreach (DataRow dr in dsEncomiendas.Tables[0].Rows){
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				Tex.WriteLine(num+espacio+dr["DESTINO"].ToString());
				num=new string(' ',Tiquetes.lenTiquete);
				Tex.WriteLine(num+espacio+dr["COSTO_ENCOMIENDA"].ToString());
				Tex.WriteLine(num+espacio+dr["VALOR_TOTAL"].ToString());
			}
			
			//Giros
			DataSet dsGiros=new DataSet();
			DBFunctions.Request(dsGiros,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_GIRO,MV.VALOR_GIRO "+
				"FROM DBXSCHEMA.MGIROS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MAGENCIA MA "+
				"WHERE MA.MAG_CODIGO=MV.MAG_AGENCIA_DESTINO AND PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MV.MPLA_CODIGO="+numero+";");
			if(dsGiros.Tables[0].Rows.Count>0){
				Tex.WriteLine("");
				Tex.WriteLine("GIROS");
				Tex.WriteLine("-----");}
			foreach (DataRow dr in dsGiros.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				Tex.WriteLine(num+espacio+dr["DESTINO"].ToString());
				num=new string(' ',Tiquetes.lenTiquete);
				Tex.WriteLine(num+espacio+dr["COSTO_GIRO"].ToString());
				Tex.WriteLine(num+espacio+dr["VALOR_GIRO"].ToString());
			}

			//Anticipos
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,TG.NOMBRE AS CONCEPTO, MV.VALOR_TOTAL_AUTORIZADO "+
				"FROM DBXSCHEMA.MGASTOS_TRANSPORTES MV, DBXSCHEMA.TCONCEPTOS_TRANSPORTES TG "+
				"WHERE MV.TCON_CODIGO=TG.TCON_CODIGO AND MV.MPLA_CODIGO="+numero+";");
			if(dsAnticipos.Tables[0].Rows.Count>0){
				Tex.WriteLine("");
				Tex.WriteLine("ANTICIPOS");
				Tex.WriteLine("---------");}
			foreach (DataRow dr in dsAnticipos.Tables[0].Rows){
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				Tex.WriteLine(num+espacio+dr["CONCEPTO"].ToString());
				num=new string(' ',Tiquetes.lenTiquete);
				Tex.WriteLine(num+espacio+dr["VALOR_TOTAL_AUTORIZADO"].ToString());
			}
			Tex.WriteLine("");
			Tex.WriteLine("INGRESOS: "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_INGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###"));
			Tex.WriteLine("EGRESOS:  "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_EGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###"));

			Tex.Close();*/
		}
	}
}
