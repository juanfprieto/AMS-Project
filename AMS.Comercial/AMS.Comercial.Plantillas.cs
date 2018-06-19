using System;
using System.Data;
using AMS.DB;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS.
	/// </summary>
	public class Plantillas
	{
		
		public Plantillas()
		{}

		// Generar el tiquete a partir de la plantilla
		public static string GenerarTiquete(string numero, string plantilla, string nlChar, string nitResponsable, int anchoTiquete)
		{
			DataSet dsTiquete=new DataSet();
			DataSet dsInfoTiquete=new DataSet();
			DataSet dsViaje=new DataSet();
			DataSet dsRuta=new DataSet();
			DataSet dsPuestos=new DataSet();
			
			int precio=0;
			string ruta,rutaPrincipal,agencia,strPlanilla;
			string strNumero=numero;
			//if(strNumero.Length>Tiquetes.lenTiquete)
			//    strNumero=Convert.ToInt32(strNumero.Substring(strNumero.Length-Tiquetes.lenTiquete)).ToString();
			//strNumero=new string('0',Tiquetes.lenTiquete-strNumero.Length)+strNumero;
			
			//Info general
			DBFunctions.Request(dsInfoTiquete, IncludeSchema.NO, "select * from dbxschema.ccampos_tiquete;");
			
			//Consultar tiquete
			string nitPlanillaManual=DBFunctions.SingleData("select mpas_nit from dbxschema.mpasajero where mpas_nombres='Planilla Manual';");
			DBFunctions.Request(dsTiquete, IncludeSchema.NO, 
				"SELECT MV.MPLA_CODIGO, MV.MRUT_CODIGO, MV.VALOR_PASAJE, MV.MNIT_COMPRADOR AS MNIT_COMPRADOR, "+
				"COALESCE(MN.MPAS_NOMBRES CONCAT ' ' CONCAT MN.MPAS_APELLIDOS, '') AS COMPRADOR, "+
				"COALESCE(MNV.MPAS_NOMBRES CONCAT ' ' CONCAT MNV.MPAS_APELLIDOS, '') AS VENDEDOR , MN.MPAS_TELEFONO "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV "+
				"LEFT JOIN DBXSCHEMA.MPASAJERO MN ON MN.MPAS_NIT=MV.MNIT_COMPRADOR "+
				"LEFT JOIN DBXSCHEMA.MPASAJERO MNV ON MNV.MPAS_NIT=MV.MNIT_RESPONSABLE "+
				"WHERE MV.TDOC_CODIGO='TIQ' AND MV.NUM_DOCUMENTO=" + numero + " AND MV.MNIT_RESPONSABLE='" + nitResponsable + "';");
			
			if(dsTiquete.Tables[0].Rows.Count==0)
				return("NO EXISTE EL TIQUETE");
			
			strPlanilla=dsTiquete.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			ruta=dsTiquete.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();

			//Consultar ruta
			DBFunctions.Request(dsRuta, IncludeSchema.NO,
				"SELECT po.PCIU_NOMBRE AS ORIGEN, pd.PCIU_NOMBRE AS DESTINO "+
				"FROM DBXSCHEMA.PCIUDAD po, DBXSCHEMA.PCIUDAD pd, DBXSCHEMA.MRUTAS mr "+
				"WHERE po.PCIU_CODIGO=mr.PCIU_COD AND pd.PCIU_CODIGO=mr.PCIU_CODDES AND mr.MRUT_CODIGO='"+ruta+"';");
			if(dsRuta.Tables[0].Rows.Count==0)
				return("NO EXISTE LA RUTA");
			
			//Consultar planilla, viaje
			DBFunctions.Request(dsViaje, IncludeSchema.NO,
				"SELECT coalesce(mv.MCAT_PLACA,'******') as PLACA, mn.MPAS_NOMBRES concat ' ' concat mn.MPAS_APELLIDOS AS CONDUCTOR , " +
				"coalesce(mb.MBUS_NUMERO,0) AS NUMERO, mp.MRUT_CODIGO AS RUTAP, mp.MAG_CODIGO as AGENCIA, mv.TPROG_TIPOPROG as PROGRAMACION, " +
				"mv.FECHA_SALIDA as FECHA_SALIDA, mv.HORA_SALIDA AS HORA_SALIDA, mv.mviaje_padre as viaje_padre " +
				"FROM DBXSCHEMA.MPLANILLAVIAJE mp " +
				"left join DBXSCHEMA.MVIAJE mv on mp.MRUT_CODIGO=mv.MRUT_CODIGO and mp.MVIAJE_NUMERO=mv.MVIAJE_NUMERO " +
				"left join DBXSCHEMA.MPASAJERO mn on mn.MPAS_NIT=mv.MNIT_CONDUCTOR " +
				"left join DBXSCHEMA.MBUSAFILIADO mb on mb.MCAT_PLACA=mv.MCAT_PLACA " +
				"WHERE mp.MPLA_CODIGO=" + strPlanilla + ";");

			if(dsViaje.Tables[0].Rows.Count==0)
				return("NO EXISTE EL VIAJE");
			
			//Consultar puestos
			string strPuestos="";
			DBFunctions.Request(dsPuestos, IncludeSchema.NO,"SELECT MELE_NUMEPUES FROM DBXSCHEMA.MCONFIGURACIONPUESTO WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+numero+" ORDER BY MELE_NUMEPUES;");
			if(dsPuestos.Tables[0].Rows.Count==0)
				return("NO HAY PUESTOS REGISTRADOS");
			//Agencia de paso? No tener en cuenta el puesto
			agencia=dsViaje.Tables[0].Rows[0]["AGENCIA"].ToString();
			rutaPrincipal=dsViaje.Tables[0].Rows[0]["RUTAP"].ToString();
			if(dsViaje.Tables[0].Rows[0]["VIAJE_PADRE"].ToString().Length==0 && 
				!DBFunctions.RecordExist(
				"select * from dbxschema.magencia ma, dbxschema.mrutas mr "+
				"where ma.pciu_codigo=mr.pciu_cod and mr.mrut_codigo='"+rutaPrincipal+"' and ma.mag_codigo="+agencia))
				strPuestos="***";
			else
			{
				for(int c=0;c<dsPuestos.Tables[0].Rows.Count;c++)
					strPuestos+=dsPuestos.Tables[0].Rows[c]["MELE_NUMEPUES"].ToString()+", ";
				if(strPuestos.EndsWith(", "))
					strPuestos=strPuestos.Substring(0,strPuestos.Length-2);
			}
			
			precio=Convert.ToInt32(dsTiquete.Tables[0].Rows[0]["VALOR_PASAJE"]);
			
			/*txtTiquete+=redChar+new String('-',anchoTiquete)+nlchar;
			if(dsInfoTiquete.Tables[0].Rows.Count>0){
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["NOMBRE_EMPRESA"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto("NIT. "+dsInfoTiquete.Tables[0].Rows[0]["NIT_EMPRESA"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TEXTO1"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TEXTO2"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TELEFONO_EMPRESA"].ToString(),' ')+nlchar;
				txtTiquete+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["TEXTO3"].ToString(),' ')+nlchar;}
			txtTiquete+=redChar+new String('-',anchoTiquete)+nlchar;
			txtTiquete+="Factura cambiaria de transporte"+nlchar;*/
			//txtTiquete+="número  : "+strNumero+nlchar;
			plantilla=plantilla.Replace("<FECHA_IMPRESION>",DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
			plantilla=plantilla.Replace("<NUMERO>",strNumero);
			if(!dsTiquete.Tables[0].Rows[0]["MNIT_COMPRADOR"].ToString().Equals(nitPlanillaManual))
			{
				plantilla=plantilla.Replace("<CLIENTE_NOMBRE>",Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["COMPRADOR"].ToString(),anchoTiquete-10));
				plantilla=plantilla.Replace("<CLIENTE_NIT>",Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["MNIT_COMPRADOR"].ToString(),anchoTiquete-10));
				plantilla = plantilla.Replace("<MPAS_TELEFONO>", Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["MPAS_TELEFONO"].ToString(), anchoTiquete - 10));
				//txtTiquete+=Tiquetes.CortarTexto("Nombre : "+Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["COMPRADOR"].ToString()))+nlchar;
				//txtTiquete+=Tiquetes.CortarTexto("         "+Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["MNIT_COMPRADOR"].ToString()))+nlchar;
			}
			else
			{
				plantilla=plantilla.Replace("<CLIENTE_NOMBRE>","");
				plantilla=plantilla.Replace("<CLIENTE_NIT>","");
			}
			int Minutos = Convert.ToInt16((dsViaje.Tables[0].Rows[0]["HORA_SALIDA"].ToString()));
			int Horas   = Minutos / 60;
			int HoraMinuto = Minutos%60;
			plantilla=plantilla.Replace("<FECHA_VIAJE>",Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd"));
			plantilla=plantilla.Replace("<HORA>",Horas.ToString());
			plantilla=plantilla.Replace("<MINUTO>",HoraMinuto.ToString());
			/*
			if(dsViaje.Tables[0].Rows[0]["PROGRAMACION"].ToString().Length>0)
				plantilla=plantilla.Replace("<FECHA_VIAJE>",DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
				//txtTiquete+="Fecha   :  "+DateTime.Now.ToString("dd/MM/yyyy hh:mm")+nlchar;
			else 
				plantilla=plantilla.Replace("<FECHA_VIAJE>",DateTime.Now.ToString("dd/MM/yyyy"));
			*/	
			//txtTiquete+="Fecha   :  "+DateTime.Now.ToString("dd/MM/yyyy ")+nlc har;
			//txtTiquete+="Bus     :  "+dsViaje.Tables[0].Rows[0]["NUMERO"].ToString()+" ("+dsViaje.Tables[0].Rows[0]["PLACA"].ToString()+")"+nlchar;
			plantilla=plantilla.Replace("<BUS_NUMERO>",dsViaje.Tables[0].Rows[0]["NUMERO"].ToString());
			plantilla=plantilla.Replace("<BUS_PLACA>",dsViaje.Tables[0].Rows[0]["PLACA"].ToString());
			//txtTiquete+=Tiquetes.AjustarTexto("Puesto  :  "+strPuestos)+nlchar;
			plantilla=plantilla=plantilla=plantilla.Replace("<PUESTOS>",strPuestos);
			//txtTiquete+="Origen  :  "+dsRuta.Tables[0].Rows[0]["ORIGEN"].ToString()+nlchar;
			plantilla=plantilla.Replace("<ORIGEN>",dsRuta.Tables[0].Rows[0]["ORIGEN"].ToString());
			//txtTiquete+="Destino :  "+dsRuta.Tables[0].Rows[0]["DESTINO"]+nlchar;
			plantilla=plantilla.Replace("<DESTINO>",dsRuta.Tables[0].Rows[0]["DESTINO"].ToString());
			//txtTiquete+="Pasajes :  "+dsPuestos.Tables[0].Rows.Count.ToString()+nlchar;
			plantilla=plantilla.Replace("<CANTIDAD>",dsPuestos.Tables[0].Rows.Count.ToString());
			//txtTiquete+="Valor   :  "+precio.ToString("#,###")+nlchar;
			plantilla=plantilla.Replace("<VALOR_UNIDAD>",precio.ToString("#,###"));
			//txtTiquete+="Total   :  "+(dsPuestos.Tables[0].Rows.Count*precio).ToString("#,###")+nlchar;
			plantilla=plantilla.Replace("<VALOR_TOTAL>",(dsPuestos.Tables[0].Rows.Count*precio).ToString("#,###"));
			plantilla=plantilla.Replace("<VENDEDOR>",Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["VENDEDOR"].ToString(),anchoTiquete-10));
			//txtTiquete+="Vendedor: "+Tiquetes.CortarTexto(dsTiquete.Tables[0].Rows[0]["VENDEDOR"].ToString())+nlchar;
			//txtTiquete+="Planilla: "+strPlanilla+nlchar+nlchar;
			plantilla=plantilla.Replace("<PLANILLA>",strPlanilla);
			return(plantilla);
		}

		//Generar la planilla a pertir de su plantilla
		public static string GenerarPlanilla(string numero, string plantilla, string nlchar, string redChar, int anchoTiquete)
		{
			DataSet dsPlanilla=new DataSet();
			DataSet dsViaje=new DataSet();
			DataSet dsInfoTiquete=new DataSet();

			//Info general
			DBFunctions.Request(dsInfoTiquete, IncludeSchema.NO, "select * from dbxschema.ccampos_tiquete;");

			//Planilla
			DBFunctions.Request(dsPlanilla,IncludeSchema.NO, "SELECT *  FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero+";");
			if(dsPlanilla.Tables[0].Rows.Count==0)return("NO HAY DATOS DE LA PLANILLA");
			
			string agencia=dsPlanilla.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			string rutaP=dsPlanilla.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();
			string viaje=dsPlanilla.Tables[0].Rows[0]["MVIAJE_NUMERO"].ToString();
			
			DBFunctions.Request(dsViaje,IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MVIAJE WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
			if(dsViaje.Tables[0].Rows.Count==0)return("NO HAY DATOS DEL VIAJE");

			int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"]);
			int horaDespacho=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_DESPACHO"]);
			string placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			string numVehiculo=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE	MCAT_PLACA='"+placa+"';");

			/*txtPlanilla+=redChar+new String('-',anchoTiquete)+nlchar;
			if(dsInfoTiquete.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["NOMBRE_EMPRESA"].ToString(),' ')+nlchar;
				txtPlanilla+=Tiquetes.CentrarTexto("NIT. "+dsInfoTiquete.Tables[0].Rows[0]["NIT_EMPRESA"].ToString(),' ')+nlchar;}
			txtPlanilla+=redChar+new String('-',anchoTiquete)+nlchar;
			txtPlanilla+=""+nlchar;*/

			//txtPlanilla+="Número:    "+dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString()+nlchar;
			plantilla=plantilla.Replace("<NUMERO>",dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString());
			//txtPlanilla+="Fecha:     "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+nlchar;
			plantilla=plantilla.Replace("<FECHA>",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			//txtPlanilla+="Bus:       "+numVehiculo+" ("+placa+")"+nlchar;
			plantilla=plantilla.Replace("<BUS_NUMERO>",numVehiculo);
			plantilla=plantilla.Replace("<BUS_PLACA>",placa);
			//txtPlanilla+="Origen :   "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";")+nlchar;
			plantilla=plantilla.Replace("<ORIGEN>",DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";"));
			//txtPlanilla+="Destino:   "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';")+nlchar;
			plantilla=plantilla.Replace("<DESTINO>",DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';"));
			plantilla=plantilla.Replace("<NUMERO_VIAJE>",viaje);
			//txtPlanilla+="Agencia:   "+DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";")+nlchar;
			plantilla=plantilla.Replace("<AGENCIA>",DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";"));
			//txtPlanilla+="Salida:    "+Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" "+Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00")+nlchar;
			plantilla=plantilla.Replace("<FECHA_SALIDA>",Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" "+(hora/60).ToString("00")+":"+(hora%60).ToString("00"));
			plantilla=plantilla.Replace("<HORA_DESPACHO>",((int)horaDespacho/60).ToString("00")+":"+(horaDespacho%60).ToString("00"));
			//txtPlanilla+=Tiquetes.CortarTexto("Conductor: "+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';"))+nlchar;
			plantilla=plantilla.Replace("<CONDUCTOR_NOMBRE>",DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';"));
			//txtPlanilla+=Tiquetes.CortarTexto("Relevador: "+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';"))+nlchar;
			plantilla=plantilla.Replace("<RELEVADOR_NOMBRE>",DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';"));
			
			string espacio="    ";
			string num="";
			//Tiquetes
			DataSet dsTiquetes=new DataSet();
			DBFunctions.Request(dsTiquetes,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.NUMERO_PUESTOS,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+numero+";");
			
			string txtPlanilla="";
			if(dsTiquetes.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=nlchar;
				txtPlanilla+="TIQUETES"+nlchar;
				txtPlanilla+=redChar+"--------"+nlchar;}
			double TotalPasajes= 0;
			double TotalPasajeros= 0;
			double TotalGiros= 0;
			double TotalIngresos= 0;
			double TotalEgresos= 0;
			double TotalPlanilla = 0;
			foreach (DataRow dr in dsTiquetes.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
				num=dr["NUMERO_PUESTOS"].ToString();
				num=new string(' ',Tiquetes.lenTiquete-num.Length)+num;
				txtPlanilla+="Vr.  "+num+espacio+dr["VALOR_PASAJE"].ToString()+nlchar;
				txtPlanilla+="Tot. "+new string(' ',Tiquetes.lenTiquete)+espacio+dr["VALOR_TOTAL"].ToString()+nlchar;
				TotalPasajes += Convert.ToDouble(dr["VALOR_TOTAL"].ToString());
				TotalPasajeros += Convert.ToDouble(dr["NUMERO_PUESTOS"].ToString());
				TotalIngresos += Convert.ToDouble(dr["VALOR_TOTAL"].ToString());
			}
			txtPlanilla+=""+nlchar;
			txtPlanilla+="Total Pasajes: "+ TotalPasajes.ToString("###,###,##0")+nlchar;

			//Tiquetes Prepago
			DataSet dsTiquetesPre=new DataSet();
			DBFunctions.Request(dsTiquetesPre,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE_PREPAGO MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+numero+";");
			if(dsTiquetesPre.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=""+nlchar;
				txtPlanilla+="TIQUETES PREPAGO"+nlchar;
				txtPlanilla+=redChar+"-------- -------"+nlchar;}
			foreach (DataRow dr in dsTiquetesPre.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
				num=new string(' ',Tiquetes.lenTiquete);
				txtPlanilla+="Vr.  "+num+espacio+dr["VALOR_PASAJE"].ToString()+nlchar;
				txtPlanilla+="Tot. "+num+espacio+dr["VALOR_TOTAL"].ToString()+nlchar;
				TotalPasajeros += 1;
			}

			//Encomiendas
			DataSet dsEncomiendas=new DataSet();
			DBFunctions.Request(dsEncomiendas,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_ENCOMIENDA,MV.VALOR_TOTAL,MV.DESCRIPCION_CONTENIDO "+
				"FROM DBXSCHEMA.MENCOMIENDAS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+numero+";");
			if(dsEncomiendas.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=""+nlchar;
				txtPlanilla+="ENCOMIENDAS"+nlchar;
				txtPlanilla+=redChar+"-----------"+nlchar;}
			foreach (DataRow dr in dsEncomiendas.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
				num=new string(' ',Tiquetes.lenTiquete);
				txtPlanilla+="Vr.  "+num+espacio+dr["COSTO_ENCOMIENDA"].ToString()+nlchar;
				TotalIngresos += Convert.ToDouble(dr["COSTO_ENCOMIENDA"].ToString());
				txtPlanilla+="Tot. "+num+espacio+dr["VALOR_TOTAL"].ToString()+nlchar;
				txtPlanilla+=Tiquetes.CortarTexto(espacio+dr["DESCRIPCION_CONTENIDO"].ToString())+nlchar;
			}
			
			//Giros
			DataSet dsGiros=new DataSet();
			DBFunctions.Request(dsGiros,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_GIRO,MV.VALOR_GIRO "+
				"FROM DBXSCHEMA.MGIROS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MAGENCIA MA "+
				"WHERE MA.MAG_CODIGO=MV.MAG_AGENCIA_DESTINO AND PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MV.MPLA_CODIGO="+numero+";");
			if(dsGiros.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=""+nlchar;
				txtPlanilla+="GIROS"+nlchar;
				txtPlanilla+=redChar+"-----"+nlchar;}
			foreach (DataRow dr in dsGiros.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
				num=new string(' ',Tiquetes.lenTiquete);
				txtPlanilla+="Cst. "+num+espacio+dr["COSTO_GIRO"].ToString()+nlchar;
				txtPlanilla+="Vr.  "+num+espacio+dr["VALOR_GIRO"].ToString()+nlchar;
				TotalIngresos += Convert.ToDouble(dr["COSTO_GIRO"].ToString());
				TotalGiros += Convert.ToDouble(dr["VALOR_GIRO"].ToString());
			}

			//Anticipos
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,TG.NOMBRE AS CONCEPTO,IMPUTACION_CONTABLE,VALOR_TOTAL_AUTORIZADO "+
				"FROM DBXSCHEMA.MGASTOS_TRANSPORTES MV, DBXSCHEMA.TCONCEPTOS_TRANSPORTES TG "+
				"WHERE MV.TCON_CODIGO=TG.TCON_CODIGO AND MV.MPLA_CODIGO="+numero+";");
			if(dsAnticipos.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=""+nlchar;
				txtPlanilla+="ANTICIPOS-OTROSINGRESOS-EGRESOS"+nlchar;
				txtPlanilla+=redChar+"---------"+nlchar;}
			foreach (DataRow dr in dsAnticipos.Tables[0].Rows)
			{
				num=dr["NUM_DOCUMENTO"].ToString().Trim();
				if(num.Length>Tiquetes.lenTiquete)
					num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
				num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
				txtPlanilla+="No.  "+dr["CONCEPTO"].ToString()+nlchar;
				num=new string(' ',Tiquetes.lenTiquete);
				txtPlanilla+="Vr.  "+num+espacio+dr["VALOR_TOTAL_AUTORIZADO"].ToString()+nlchar;
				if (dr["IMPUTACION_CONTABLE"].ToString() == "D")
					TotalIngresos += Convert.ToDouble(dr["VALOR_TOTAL_AUTORIZADO"].ToString());
				else
					TotalEgresos += Convert.ToDouble(dr["VALOR_TOTAL_AUTORIZADO"].ToString());

			}
			plantilla=plantilla.Replace("<CONTENIDO>",txtPlanilla);
			//txtPlanilla+=redChar+"INGRESOS: "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_INGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###")+nlchar;
			plantilla=plantilla.Replace("<INGRESOS>",TotalIngresos.ToString("###,###,##0"));
			//txtPlanilla+=redChar+"EGRESOS:  "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_EGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###")+nlchar;
			plantilla=plantilla.Replace("<EGRESOS>",TotalEgresos.ToString("###,###,##0"));
			TotalPlanilla = TotalIngresos - TotalEgresos;
			plantilla=plantilla.Replace("<TOTAL_PLANILLA>",TotalPlanilla.ToString("###,###,##0"));
			/*txtPlanilla+=redChar+"TOTAL PASAJEROS: "+DBFunctions.SingleData("Select sum(MV.NUMERO_PUESTOS) "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+numero+";")+nlchar;*/
			plantilla=plantilla.Replace("<PASAJEROS>",TotalPasajeros.ToString());
			plantilla=plantilla.Replace("<VALOR_GIROS>",TotalGiros.ToString("###,###,##0"));
			
			string ObservacionPlanilla = DBFunctions.SingleData("select OBSERVACION_PLANILLA from dbxschema.mplanilla_observacion where mpla_codigo="+numero+";");
			int longitud = ObservacionPlanilla.Length;
			int lineas = longitud/Tiquetes.anchoTiquete;
			txtPlanilla="";
			int subindice = 0;
			for(int i=1;i<=lineas;i++)
			{
				txtPlanilla+=Tiquetes.ReemplazarTexto(ObservacionPlanilla.Substring(subindice,Tiquetes.anchoTiquete).ToString())+nlchar;
				subindice += Tiquetes.anchoTiquete;
			}
			if (subindice < longitud)
				txtPlanilla+=Tiquetes.ReemplazarTexto(ObservacionPlanilla.Substring(subindice,longitud - subindice).ToString())+nlchar;
			plantilla=plantilla.Replace("<OBSERVACIONES>",txtPlanilla);
			return(plantilla);
		}

		//Generar el anticipo a partir de su plantilla 
		public static string GenerarAnticipo(string numero, string plantilla, string nlchar, string redChar, int anchoTiquete)
		{
			string agencia,fecha,busNumero,busPlaca,planilla,concepto,entregaNit,entregaNombre,recibeNit,recibeNombre,descripcion,cantidad,valorU,valorT;
			plantilla=plantilla.Replace("<RED>",redChar);

			DataSet dsAnticipo=new DataSet();
			DBFunctions.Request(dsAnticipo, IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MGASTOS_TRANSPORTES WHERE NUM_DOCUMENTO="+numero+";");
			if(dsAnticipo.Tables[0].Rows.Count==0)
			{
				return "NO EXISTE EL ANTICIPO";
			}

			agencia=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+dsAnticipo.Tables[0].Rows[0]["MAG_CODIGO"].ToString()+";");
			busPlaca=dsAnticipo.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			busNumero=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE	MCAT_PLACA='"+busPlaca+"';");
			planilla=dsAnticipo.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			concepto=DBFunctions.SingleData("SELECT NOMBRE from DBXSCHEMA.TCONCEPTOS_TRANSPORTES WHERE TCON_CODIGO="+dsAnticipo.Tables[0].Rows[0]["TCON_CODIGO"].ToString()+";");
			entregaNit=dsAnticipo.Tables[0].Rows[0]["MNIT_RESPONSABLE_ENTREGA"].ToString();
			entregaNombre=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+entregaNit+"';");			
			recibeNit=dsAnticipo.Tables[0].Rows[0]["MNIT_RESPONSABLE_RECIBE"].ToString();
			recibeNombre=DBFunctions.SingleData("SELECT MNIT_NOMBRES CONCAT ' ' CONCAT MNIT_APELLIDOS FROM DBXSCHEMA.MNIT where MNIT_NIT='"+recibeNit+"';");
			
			//descripcion=Tiquetes.CortarTexto(dsAnticipo.Tables[0].Rows[0]["DESCRIPCION"].ToString(),anchoTiquete);
			cantidad=Convert.ToDouble(dsAnticipo.Tables[0].Rows[0]["CANTIDAD_CONSUMO"]).ToString("##0");
			valorU=Convert.ToDouble(dsAnticipo.Tables[0].Rows[0]["VALOR_UNIDAD"]).ToString("###,###,##0");
			valorT=Convert.ToDouble(dsAnticipo.Tables[0].Rows[0]["VALOR_TOTAL_AUTORIZADO"]).ToString("###,###,##0");
			fecha=Convert.ToDateTime(dsAnticipo.Tables[0].Rows[0]["FECHA_DOCUMENTO"]).ToString("yyyy/MM/dd");

			plantilla = plantilla.Replace("<FECHA_IMPRESION>", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
			plantilla=plantilla.Replace("<NUMERO>",numero);
			plantilla=plantilla.Replace("<AGENCIA>",agencia);
			plantilla=plantilla.Replace("<BUS_NUMERO>",busNumero);
			plantilla=plantilla.Replace("<BUS_PLACA>",busPlaca);
			plantilla=plantilla.Replace("<PLANILLA>",planilla);
			plantilla=plantilla.Replace("<CONCEPTO>",concepto);
			plantilla=plantilla.Replace("<ENTREGA_NOMBRE>",entregaNombre);
			plantilla=plantilla.Replace("<ENTREGA_NIT>",entregaNit);
			plantilla=plantilla.Replace("<RECIBE_NOMBRE>",recibeNombre);
			plantilla=plantilla.Replace("<RECIBE_NIT>",recibeNit);
			descripcion = dsAnticipo.Tables[0].Rows[0]["DESCRIPCION"].ToString();
			int longitud = descripcion.Length;
			int lineas = longitud/Tiquetes.anchoTiquete;
			string txtDescripcion="";
			int subindice = 0;
			for(int i=1;i<=lineas;i++)
			{
				txtDescripcion+=Tiquetes.ReemplazarTexto(descripcion.Substring(subindice,Tiquetes.anchoTiquete).ToString())+nlchar;
				subindice += Tiquetes.anchoTiquete;
			}
			if (subindice < longitud)
				txtDescripcion+=Tiquetes.ReemplazarTexto(descripcion.Substring(subindice,longitud - subindice).ToString())+nlchar;
			plantilla=plantilla.Replace("<DESCRIPCION>",txtDescripcion);	

			//plantilla=plantilla.Replace("<DESCRIPCION>",descripcion);
			plantilla=plantilla.Replace("<CANTIDAD>",cantidad);
			plantilla=plantilla.Replace("<VALOR_UNIDAD>",valorU);
			plantilla=plantilla.Replace("<VALOR_TOTAL>",valorT);
			return plantilla.Replace("<FECHA>",fecha);
		}
	}
}
