namespace AMS.Comercial
{
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

	/// <summary>
	/// Descripción breve de AMS_Comercial_Planilla1.
	/// </summary>
	public class AMS_Comercial_Planilla1 : System.Web.UI.Page
	{
		public string strPlanilla, strOrigen, strDestino, strAgencia, strFecha, strHora, strNumeroVehiculo, strPlacaVehiculo, strConductor, strRelevador, strTiquetes, strTiquetesPrepago, strEncomiendas, strGiros, strAnticipos, strIngresos, strEgresos, strNombreEmpresa, strNITEmpresa;
		public string txtPlanilla;
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Introducir aquí el código de usuario para inicializar la página
			if(!IsPostBack)
			{
				if(Request.QueryString["pln"]!=null)
					this.Generar_Planilla(Request.QueryString["pln"]);
			}
		}

		//Mostrar
		/*public void VerPlanilla(string planilla)
		{
			ViewState["Planilla"]=planilla;
			DataSet dsPlanilla=new DataSet();
			DataSet dsViaje=new DataSet();

			DBFunctions.Request(dsPlanilla,IncludeSchema.NO, "SELECT *  FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla+";");
			if(dsPlanilla.Tables[0].Rows.Count==0)return;
			
			string agencia=dsPlanilla.Tables[0].Rows[0]["MAG_CODIGO"].ToString();
			string rutaP=dsPlanilla.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();
			string viaje=dsPlanilla.Tables[0].Rows[0]["MVIAJE_NUMERO"].ToString();
			strOrigen=DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";");
			strDestino=DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';");
			strAgencia=DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";");

			DBFunctions.Request(dsViaje,IncludeSchema.NO, "SELECT * FROM DBXSCHEMA.MVIAJE WHERE MRUT_CODIGO='"+rutaP+"' AND MVIAJE_NUMERO="+viaje+";");
			if(dsViaje.Tables[0].Rows.Count==0)return;

			int hora=Convert.ToInt32(dsViaje.Tables[0].Rows[0]["HORA_SALIDA"]);
			string placa=dsViaje.Tables[0].Rows[0]["MCAT_PLACA"].ToString();
			strPlacaVehiculo=placa;
			string numVehiculo=DBFunctions.SingleData("SELECT MBUS_NUMERO FROM DBXSCHEMA.MBUSAFILIADO WHERE	MCAT_PLACA='"+placa+"';");
			strNumeroVehiculo=numVehiculo;
			strHora=Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00");
			strConductor=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';");
			strRelevador=DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';");
			strPlanilla=dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			strFecha=Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd");
			strIngresos=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_INGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla)).ToString("###,###,###");
			strEgresos=Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_EGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+planilla)).ToString("###,###,###");

			string espacio="&nbsp;&nbsp;&nbsp;&nbsp;";
			//Tiquetes
			DataSet dsTiquetes=new DataSet();
			DBFunctions.Request(dsTiquetes,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.NUMERO_PUESTOS,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+planilla+";");
			
			foreach (DataRow dr in dsTiquetes.Tables[0].Rows){
				strTiquetes+=dr["NUM_DOCUMENTO"].ToString()+espacio+dr["DESTINO"].ToString()+espacio+dr["NUMERO_PUESTOS"].ToString()+espacio+dr["VALOR_PASAJE"].ToString()+espacio+dr["VALOR_TOTAL"].ToString();
				strTiquetes+="<br>";}

			//Tiquetes Prepago
			DataSet dsTiquetesPre=new DataSet();
			DBFunctions.Request(dsTiquetesPre,IncludeSchema.NO, "Select cast(right(rtrim(char(MV.NUM_DOCUMENTO)),"+AMS.Comercial.Tiquetes.lenTiquete+") as integer) AS NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.VALOR_PASAJE,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE_PREPAGO MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE  MV.TESTADO_TIQUETE = 'V' and MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+planilla+";");
			foreach (DataRow dr in dsTiquetesPre.Tables[0].Rows){
				strTiquetesPrepago+=dr["NUM_DOCUMENTO"].ToString()+espacio+dr["DESTINO"].ToString()+espacio+dr["VALOR_PASAJE"].ToString()+espacio+dr["VALOR_TOTAL"].ToString();
				strTiquetesPrepago+="<br>";}


			//Encomiendas
			DataSet dsEncomiendas=new DataSet();
			DBFunctions.Request(dsEncomiendas,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_ENCOMIENDA,MV.VALOR_TOTAL "+
				"FROM DBXSCHEMA.MENCOMIENDAS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MV.TEST_CODIGO <> 'R' and MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.MPLA_CODIGO="+planilla+";");
			foreach (DataRow dr in dsEncomiendas.Tables[0].Rows){
				strEncomiendas+=dr["NUM_DOCUMENTO"].ToString()+espacio+dr["DESTINO"].ToString()+espacio+dr["COSTO_ENCOMIENDA"].ToString()+espacio+dr["VALOR_TOTAL"].ToString();
				strEncomiendas+="<br>";}

			//Giros
			DataSet dsGiros=new DataSet();
			DBFunctions.Request(dsGiros,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,PC.PCIU_NOMBRE AS DESTINO,MV.COSTO_GIRO,MV.VALOR_GIRO "+
				"FROM DBXSCHEMA.MGIROS MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MAGENCIA MA "+
				"WHERE MV.TEST_CODIGO <> 'R' AND MA.MAG_CODIGO=MV.MAG_AGENCIA_DESTINO AND PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MV.MPLA_CODIGO="+planilla+";");
			foreach (DataRow dr in dsGiros.Tables[0].Rows){
				strGiros+=dr["NUM_DOCUMENTO"].ToString()+espacio+dr["DESTINO"].ToString()+espacio+dr["COSTO_GIRO"].ToString()+espacio+dr["VALOR_GIRO"].ToString();
				strGiros+="<br>";}

			//Anticipos
			DataSet dsAnticipos=new DataSet();
			DBFunctions.Request(dsAnticipos,IncludeSchema.NO, "Select MV.NUM_DOCUMENTO,TG.NOMBRE AS CONCEPTO, MV.VALOR_TOTAL_AUTORIZADO "+
				"FROM DBXSCHEMA.MGASTOS_TRANSPORTES MV, DBXSCHEMA.TCONCEPTOS_TRANSPORTES TG "+
				"WHERE MV.TEST_CODIGO     <> 'R' AND MV.TCON_CODIGO=TG.TCON_CODIGO AND MV.MPLA_CODIGO="+planilla+";");
			foreach (DataRow dr in dsAnticipos.Tables[0].Rows){
				strAnticipos+=dr["NUM_DOCUMENTO"].ToString()+espacio+dr["CONCEPTO"].ToString()+espacio+dr["VALOR_TOTAL_AUTORIZADO"].ToString();
				strAnticipos+="<br>";}

		}
*/

		protected void Generar_Planilla(string numero)
		{
			string plantilla="";
			string nlchar="`",redChar="^";
			int anchoTiquete=Tiquetes.anchoTiquete;
			try
			{
				string strLinea="";
				StreamReader strArchivo;
				strArchivo=File.OpenText(ConfigurationManager.AppSettings["PathToPapeleria"]+"\\PlantillaPlanilla.txt");
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
                Utils.MostrarAlerta(Response, "  No se ha creado la plantilla de planillas, no se pudo imprimir.");
				return;
			}
			plantilla=plantilla.Replace("<RED>",redChar);


			//Info general

			//Planilla
			
			


			/*txtPlanilla+=redChar+new String('-',anchoTiquete)+nlchar;
			if(dsInfoTiquete.Tables[0].Rows.Count>0)
			{
				txtPlanilla+=Tiquetes.CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["NOMBRE_EMPRESA"].ToString(),' ')+nlchar;
				txtPlanilla+=Tiquetes.CentrarTexto("NIT. "+dsInfoTiquete.Tables[0].Rows[0]["NIT_EMPRESA"].ToString(),' ')+nlchar;}
			txtPlanilla+=redChar+new String('-',anchoTiquete)+nlchar;
			txtPlanilla+=""+nlchar;*/

			//txtPlanilla+="Número:    "+dsPlanilla.Tables[0].Rows[0]["MPLA_CODIGO"].ToString()+nlchar;
			//txtPlanilla+="Fecha:     "+DateTime.Now.ToString("yyyy-MM-dd HH:mm")+nlchar;
			//txtPlanilla+="Bus:       "+numVehiculo+" ("+placa+")"+nlchar;
			//txtPlanilla+="Origen :   "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MAGENCIA MA, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MA.PCIU_CODIGO AND MA.MAG_CODIGO="+agencia+";")+nlchar;
			//txtPlanilla+="Destino:   "+DBFunctions.SingleData("SELECT PC.PCIU_NOMBRE FROM DBXSCHEMA.MRUTAS MR, DBXSCHEMA.PCIUDAD PC WHERE PC.PCIU_CODIGO=MR.PCIU_CODDES AND MR.MRUT_CODIGO='"+rutaP+"';")+nlchar;
			//txtPlanilla+="Agencia:   "+DBFunctions.SingleData("SELECT MAGE_NOMBRE FROM DBXSCHEMA.MAGENCIA WHERE MAG_CODIGO="+agencia+";")+nlchar;
			//txtPlanilla+="Salida:    "+Convert.ToDateTime(dsViaje.Tables[0].Rows[0]["FECHA_SALIDA"]).ToString("yyyy-MM-dd")+" "+Math.Round((double)hora/60,0).ToString("00")+":"+(hora%60).ToString("00")+nlchar;
			//txtPlanilla+=Tiquetes.CortarTexto("Conductor: "+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_CONDUCTOR"]+"';"))+nlchar;
			//txtPlanilla+=Tiquetes.CortarTexto("Relevador: "+DBFunctions.SingleData("select mnit_nombres concat ' ' concat mnit_apellidos FROM DBXSCHEMA.MNIT WHERE MNIT_NIT='"+dsViaje.Tables[0].Rows[0]["MNIT_RELEVADOR1"]+"';"))+nlchar;
			
			//Tiquetes
			

			//Tiquetes Prepago

			//Encomiendas
			
			//Giros

			//Anticipos

			//txtPlanilla+=redChar+"INGRESOS: "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_INGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###")+nlchar;
			//txtPlanilla+=redChar+"EGRESOS:  "+Convert.ToDouble(DBFunctions.SingleData("SELECT coalesce(VALOR_EGRESOS,0) FROM DBXSCHEMA.MPLANILLAVIAJE WHERE MPLA_CODIGO="+numero)).ToString("###,###,###")+nlchar;
			/*txtPlanilla+=redChar+"TOTAL PASAJEROS: "+DBFunctions.SingleData("Select sum(MV.NUMERO_PUESTOS) "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV,DBXSCHEMA.PCIUDAD PC,DBXSCHEMA.MRUTAS MR "+
				"WHERE MR.MRUT_CODIGO=MV.MRUT_CODIGO AND PC.PCIU_CODIGO=MR.PCIU_CODDES AND MV.TEST_CODIGO='V' AND MV.MPLA_CODIGO="+numero+";")+nlchar;*/
			
			txtPlanilla=Comercial.Plantillas.GenerarPlanilla(numero,plantilla,nlchar,redChar,anchoTiquete);
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
