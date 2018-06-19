using System;
using System.IO;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using AMS.DB;
using AMS.Forms;
using AMS.Tools;

namespace AMS.Comercial
{
	/// <summary>
	/// Descripción breve de AMS_Comercial_PlanillaGemela.
	/// </summary>
	public class AMS_Comercial_PlanillaGemela : System.Web.UI.Page
	{
		public string strPlanilla, strOrigen, strDestino, strAgencia, strFecha, strHora, strNumeroVehiculo, strPlacaVehiculo, strConductor, strRelevador, strTiquetes, strTiquetesPrepago, strEncomiendas, strGiros, strAnticipos, strIngresos, strEgresos, strNombreEmpresa, strNITEmpresa;
		public string txtPlanilla;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request.QueryString["pln"]!=null && Request.QueryString["tp"]!=null)
					this.Generar_Planilla(Request.QueryString["pln"],Request.QueryString["tp"]);
			}
		}

		protected void Generar_Planilla(string numero,string tipo)
		{
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
						
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaPlanillaGemela.txt");
				strLinea=strArchivo.ReadLine();
				//La primera linea puede contener el ancho del tiquete
				try
				{
					anchoTiquete=Int16.Parse(strLinea);
					strLinea=strArchivo.ReadLine();}
				catch{}

				while(strLinea!=null)
				{
					plantilla+=strLinea+nlchar;
					strLinea=strArchivo.ReadLine();
				}
				strArchivo.Close();
			}
			catch
			{
                Utils.MostrarAlerta(Response, "No se ha creado la plantilla de planillas, no se pudo imprimir.");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);
			txtPlanilla="";

			DataSet dsPlanilla=new DataSet();
			DataSet dsViaje=new DataSet();
			DataSet dsInfoTiquete=new DataSet();
			int  SecuenciaRuta1,SecuenciaRutaT;
			
			//Info general
			DBFunctions.Request(dsInfoTiquete, IncludeSchema.NO, "select * from dbxschema.ccampos_tiquete;");

			//Planilla
			DBFunctions.Request(dsPlanilla,IncludeSchema.NO, "SELECT *  FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero+";");
			if(dsPlanilla.Tables[0].Rows.Count==0)return;
			
			string agencia=dsPlanilla.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			string rutaP=dsPlanilla.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();
			string viaje=dsPlanilla.Tables[0].Rows[0]["MVIAJE_NUMERO"].ToString();
			double ValorTiquete,TotalIngresos,TotalEgresos,Pasajeros,TotalTiquetes,PorcentajeRuta1,ValorEncomienda;
			         
			//Gemelas
			if(!DBFunctions.RecordExist("SELECT MPLA_CODIGO FROM DBXSCHEMA.MPLANILLA_VIAJE_GEMELA WHERE MPLA_CODIGO="+numero+";"))
				return;
            
			string rutaG="",origenG,destinoG,rutaI,CodigoRuta1,DestinoRuta1;
			DataSet dsGemela=new DataSet();
			DBFunctions.Request(dsGemela,IncludeSchema.NO,"SELECT MRUT_CODIGO,MRUT_CODIGO1,MRUT_CODIGO2,CODIGO_INTERNO1,CODIGO_INTERNO2,PORCENTAJE_RUTA1 FROM DBXSCHEMA.MRUTAS_DOBLE_PLANILLA WHERE MRUT_CODIGO='"+rutaP+"';");
			CodigoRuta1=dsGemela.Tables[0].Rows[0]["MRUT_CODIGO1"].ToString();
			SecuenciaRuta1=Convert.ToInt32(DBFunctions.SingleData(" Select MRUTA_SECUENCIA from DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL = '"+rutaP+"' AND MRUTA_SECUNDARIA = '"+CodigoRuta1+"';"));
			PorcentajeRuta1= Convert.ToDouble(dsGemela.Tables[0].Rows[0]["PORCENTAJE_RUTA1"]);

			if(dsGemela.Tables[0].Rows.Count>0)
			{
				if(tipo=="1")
				{
					rutaG=dsGemela.Tables[0].Rows[0]["MRUT_CODIGO1"].ToString();
					rutaI=dsGemela.Tables[0].Rows[0]["CODIGO_INTERNO1"].ToString();}
				else if(tipo=="2")
				{
					rutaG=dsGemela.Tables[0].Rows[0]["MRUT_CODIGO2"].ToString();
					rutaI=dsGemela.Tables[0].Rows[0]["CODIGO_INTERNO2"].ToString();}
				else return;
			}
			else return;

			origenG=DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_COD AND MR.MRUT_CODIGO='"+rutaG+"';");
			destinoG=DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaG+"';");
	
			plantilla=plantilla.Replace("<ORIGEN_GEMELA>",origenG);
			plantilla=plantilla.Replace("<DESTINO_GEMELA>",destinoG);
			plantilla=plantilla.Replace("<RUTA_INTERNA>",rutaI);
			
			
			DBFunctions.Request(dsViaje,IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MVIAJE WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
			if(dsViaje.Tables[0].Rows.Count==0)return;

			int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"]);
			string placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			string numVehiculo=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE	MCAT_PLACA='"+placa+"';");

			//txtPlanilla+="Número:    "+dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString()+nlchar;
			plantilla=plantilla.Replace("<NUMERO>",dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString());
			//txtPlanilla+="Fecha:     "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+nlchar;
			plantilla=plantilla.Replace("<FECHA>",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			//txtPlanilla+="Bus:       "+numVehiculo+" ("+placa+")"+nlchar;
			plantilla=plantilla.Replace("<BUS_NUMERO>",numVehiculo);
			plantilla=plantilla.Replace("<BUS_PLACA>",placa);
			//txtPlanilla+="Origen :   "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";")+nlchar;
			//plantilla=plantilla.Replace("<ORIGEN>",DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";"));
			//txtPlanilla+="Destino:   "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';")+nlchar;
			//plantilla=plantilla.Replace("<DESTINO>",DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';"));
			//txtPlanilla+="Agencia:   "+DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";")+nlchar;
			//plantilla=plantilla.Replace("<AGENCIA>",DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";"));
			plantilla=plantilla.Replace("<AGENCIA>",agencia);
			//txtPlanilla+="Salida:    "+Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" "+Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00")+nlchar;
			plantilla=plantilla.Replace("<FECHA_SALIDA>",Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" "+Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00"));
			//txtPlanilla+=Tiquetes.CortarTexto("Conductor: "+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';"))+nlchar;
			plantilla=plantilla.Replace("<CONDUCTOR_NOMBRE>",DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';"));
			//txtPlanilla+=Tiquetes.CortarTexto("Relevador: "+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';"))+nlchar;
			plantilla=plantilla.Replace("<RELEVADOR_NOMBRE>",DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';"));
			
			string espacio="    ";
			string num="";
			string Pasajes="";
			//Tiquetes
			DataSet dsTiquetes=new DataSet();
			DBFunctions.Request(dsTiquetes,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.MRUT_CODIGO AS CODIGO_RUTA,MV.NUMERO_PUESTOS,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+numero+";");
			
			txtPlanilla="";
			TotalIngresos= 0;
			TotalEgresos = 0;
			Pasajeros    = 0;
			if(dsTiquetes.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=nlchar;
				txtPlanilla+="TIQUETES"+nlchar;
				txtPlanilla+=redChar+"--------"+nlchar;
			}
			double TotalPasajes= 0;
			foreach (DataRow dr in dsTiquetes.Tables[0].Rows)
			{
				SecuenciaRutaT=Convert.ToInt32(DBFunctions.SingleData(" Select MRUTA_SECUENCIA from DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL = '"+rutaP+"' AND MRUTA_SECUNDARIA = '"+dr["CODIGO_RUTA"].ToString()+"';"));
				// Condicion para saber si lista el tiquete de acuerdo a la planilla 1 o 2
				if(tipo=="1" || (tipo=="2" && SecuenciaRutaT > SecuenciaRuta1))
				{
					num=dr["NUM_DOCUMENTO"].ToString().Trim();
					if(num.Length>Tiquetes.lenTiquete)
						num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
					num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
					
					Pasajes=dr["NUMERO_PUESTOS"].ToString();
					Pasajes=new string(' ',Tiquetes.lenTiquete-num.Length)+Pasajes;
				 
					if(tipo=="1")
					{
						if(SecuenciaRutaT > SecuenciaRuta1)
						{
							DestinoRuta1 = DBFunctions.SingleData("Select PC.PCIU_NOMBRE AS DESTINO FROM DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
								"WHERE MR.MRUT_CODIGO='"+CodigoRuta1+"' AND PC.PCIU_CODIGO=MR.PCIU_CODDES;");
						
							txtPlanilla+="No."+num+espacio+Pasajes+espacio+DestinoRuta1+nlchar;
							ValorTiquete = Math.Round((Convert.ToDouble(dr["VALOR_PASAJE"].ToString()) * PorcentajeRuta1/100),0);
						}
						else 
						{
							ValorTiquete = Convert.ToDouble(dr["VALOR_PASAJE"].ToString());
							txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
						}
					}
					else 
					{
						ValorTiquete = Convert.ToDouble(dr["VALOR_PASAJE"].ToString()) - Math.Round((Convert.ToDouble(dr["VALOR_PASAJE"].ToString()) * PorcentajeRuta1/100),0);
						txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
					}
					TotalTiquetes = ValorTiquete *  Convert.ToInt32(dr["NUMERO_PUESTOS"].ToString());
					txtPlanilla+="Vr.  "+num+espacio+ValorTiquete.ToString()+nlchar;
					txtPlanilla+="Tot. "+new string(' ',Tiquetes.lenTiquete)+espacio+TotalTiquetes.ToString()+nlchar;
					TotalIngresos += TotalTiquetes;
					TotalPasajes  += TotalTiquetes;
					Pasajeros     += Convert.ToInt32(dr["NUMERO_PUESTOS"].ToString());
				}
			}
			txtPlanilla+=""+nlchar;
			txtPlanilla+="Total Pasajes: "+ TotalPasajes.ToString("###,###,##0")+nlchar;
			 
			//Tiquetes Prepago
			DataSet dsTiquetesPre=new DataSet();
			DBFunctions.Request(dsTiquetesPre,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.MRUT_CODIGO AS CODIGO_RUTA,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE_PREPAGO MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+numero+";");
			if(dsTiquetesPre.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=""+nlchar;
				txtPlanilla+="TIQUETES PREPAGO"+nlchar;
				txtPlanilla+=redChar+"-------- -------"+nlchar;
			}
			foreach (DataRow dr in dsTiquetesPre.Tables[0].Rows)
			{
				SecuenciaRutaT=Convert.ToInt32(DBFunctions.SingleData(" Select MRUTA_SECUENCIA from DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL = '"+rutaP+"' AND MRUTA_SECUNDARIA = '"+dr["CODIGO_RUTA"].ToString()+"';"));
				// Condicion para saber si lista el tiquete de acuerdo a la planilla
				if(tipo=="1" || (tipo=="2" && SecuenciaRutaT > SecuenciaRuta1))
				{
					num=dr["NUM_DOCUMENTO"].ToString().Trim();
					if(num.Length>Tiquetes.lenTiquete)
						num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
					num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
					if(tipo=="1")
					{
						if(SecuenciaRutaT > SecuenciaRuta1)
						{
							DestinoRuta1 = DBFunctions.SingleData("Select PC.PCIU_NOMBRE AS DESTINO FROM DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
								"WHERE MR.MRUT_CODIGO='"+CodigoRuta1+"' AND PC.PCIU_CODIGO=MR.PCIU_CODDES;");
						
							txtPlanilla+="No.  "+num+espacio+DestinoRuta1+nlchar;
							ValorTiquete = Math.Round((Convert.ToDouble(dr["VALOR_PASAJE"].ToString()) * PorcentajeRuta1/100),0);
						}
						else 
						{
							ValorTiquete = Convert.ToDouble(dr["VALOR_PASAJE"].ToString());
							txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
						}
					}
					else 
					{
						ValorTiquete = Convert.ToDouble(dr["VALOR_PASAJE"].ToString()) - Math.Round((Convert.ToDouble(dr["VALOR_PASAJE"].ToString()) * PorcentajeRuta1/100),0);
						txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
					}
				
					txtPlanilla+="Vr.  "+num+espacio+ValorTiquete.ToString()+nlchar;
					txtPlanilla+="Tot. "+num+espacio+ValorTiquete.ToString()+nlchar;
					TotalIngresos += ValorTiquete;
					Pasajeros     += 1;
				}
			}
			 
			//Encomiendas
			DataSet dsEncomiendas=new DataSet();
			DBFunctions.Request(dsEncomiendas,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.MRUT_CODIGO AS CODIGO_RUTA,MV.COSTO_ENCOMIENDA,MV.VALOR_TOTAL,MV.DESCRIPCION_CONTENIDO "+
				"FROM DBXSCHEMA.MENCOMIENDAS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+numero+";");
			if(dsEncomiendas.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=""+nlchar;
				txtPlanilla+="ENCOMIENDAS"+nlchar;
				txtPlanilla+=redChar+"-----------"+nlchar;
			}
			foreach (DataRow dr in dsEncomiendas.Tables[0].Rows)
			{
				SecuenciaRutaT=Convert.ToInt32(DBFunctions.SingleData(" Select coalesce(MRUTA_SECUENCIA,0) from DBXSCHEMA.MRUTA_INTERMEDIA WHERE MRUTA_PRINCIPAL = '"+rutaP+"' AND MRUTA_SECUNDARIA = '"+dr["CODIGO_RUTA"].ToString()+"';"));
				// Condicion para saber si lista el tiquete de acuerdo a la planilla
				if(tipo=="1" || (tipo=="2" && SecuenciaRutaT > SecuenciaRuta1))
				{
					num=dr["NUM_DOCUMENTO"].ToString().Trim();
					if(num.Length>Tiquetes.lenTiquete)
						num=Convert.ToInt32(num.Substring(num.Length-Tiquetes.lenTiquete)).ToString();
					num=new string('0',Tiquetes.lenTiquete-num.Length)+num;
					
					if(tipo=="1")
					{
						if(SecuenciaRutaT > SecuenciaRuta1)
						{
							DestinoRuta1 = DBFunctions.SingleData("Select PC.PCIU_NOMBRE AS DESTINO FROM DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
								"WHERE MR.MRUT_CODIGO='"+CodigoRuta1+"' AND PC.PCIU_CODIGO=MR.PCIU_CODDE;");
						
							txtPlanilla+="No.  "+num+espacio+DestinoRuta1+nlchar;
							ValorEncomienda = Math.Round((Convert.ToDouble(dr["COSTO_ENCOMIENDA"].ToString()) * PorcentajeRuta1/100),0);
						}
						else 
						{
							ValorEncomienda = Convert.ToDouble(dr["COSTO_ENCOMIENDA"].ToString());
							txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
						}
					}
					else 
					{
						ValorEncomienda = Convert.ToDouble(dr["COSTO_ENCOMIENDA"].ToString()) - Math.Round((Convert.ToDouble(dr["COSTO_ENCOMIENDA"].ToString()) * PorcentajeRuta1/100),0);
						txtPlanilla+="No.  "+num+espacio+dr["DESTINO"].ToString()+nlchar;
					}
				
					txtPlanilla+="Vr.  "+num+espacio+ValorEncomienda.ToString()+nlchar;
					txtPlanilla+="Tot. "+num+espacio+ValorEncomienda.ToString()+nlchar;
					txtPlanilla+=Tiquetes.CortarTexto(espacio+dr["DESCRIPCION_CONTENIDO"].ToString())+nlchar;
					TotalIngresos += ValorEncomienda;
				}
			}
			//Giros
			if(tipo=="1")
			{
				DataSet dsGiros=new DataSet();
				DBFunctions.Request(dsGiros,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_GIRO,MV.VALOR_GIRO "+
					"FROM DBXSCHEMA.MGIROS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MAGENCIA MA "+
					"WHERE MA.MAG_CODIGO=MV.MAG_AGENCIA_DESTINO AND PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MV.MPLA_CODIGO="+numero+";");
				if(dsGiros.Tables[0].Rows.Count>0)
				{
					txtPlanilla+=""+nlchar;
					txtPlanilla+="GIROS"+nlchar;
					txtPlanilla+=redChar+"-----"+nlchar;
				}
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
					TotalIngresos +=Convert.ToDouble(dr["COSTO_GIRO"].ToString());
				}
			}
			//Anticipos
			if(tipo=="1")
			{
				DataSet dsAnticipos=new DataSet();
				DBFunctions.Request(dsAnticipos,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,TG.NOMBRE AS CONCEPTO,MV.VALOR_TOTAL_AUTORIZADO "+
					"FROM DBXSCHEMA.MGASTOS_TRANSPORTES MV, DBXSCHEMA.TCONCEPTOS_TRANSPORTES TG "+
					"WHERE MV.TCON_CODIGO=TG.TCON_CODIGO AND MV.MPLA_CODIGO="+numero+";");
				if(dsAnticipos.Tables[0].Rows.Count>0)
				{
					txtPlanilla+=""+nlchar;
					txtPlanilla+="ANTICIPOS-INGRESOS-EGRESOS"+nlchar;
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
					TotalEgresos +=Convert.ToDouble(dr["VALOR_TOTAL_AUTORIZADO"].ToString());
				}
				
			}
			plantilla=plantilla.Replace("<CONTENIDO>",txtPlanilla);
			//txtPlanilla+=redChar+"INGRESOS: "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_INGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###")+nlchar;
			plantilla=plantilla.Replace("<INGRESOS>",Convert.ToDouble(TotalIngresos).ToString("###,###,##0"));
			//txtPlanilla+=redChar+"EGRESOS:  "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_EGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###")+nlchar;
			plantilla=plantilla.Replace("<EGRESOS>",Convert.ToDouble(TotalEgresos).ToString("###,###,##0"));
			/*txtPlanilla+=redChar+"TOTAL PASAJEROS: "+DBFunctions.SingleData("Select sum(MV.NUMERO_PUESTOS) "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+numero+";")+nlchar;*/
			plantilla=plantilla.Replace("<PASAJEROS>",Convert.ToDouble(Pasajeros).ToString());
			txtPlanilla=plantilla;
		}
		#region Código generado por el Diseñador de Web Forms
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: llamada requerida por el Diseñador de Web Forms ASP.NET.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
