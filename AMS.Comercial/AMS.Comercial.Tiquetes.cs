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
	public class Tiquetes
	{
		//Tamaño del numero de tiquete(sin prefijo)
        public const int lenTiquete = 6; //anterior 6. pendiente a 7
		public const int anchoTiquete=30;
		public const int anchoTiqueteMovil=40;
		public const string nuevaLinea="`";
		public Tiquetes()
		{
			//
			// TODO: agregar aquí la lógica del constructor
			//
		}
		//Devuelve el descuento por dfefecto de prepagos
		public static double PorcentajePrepago()
		{
			try
			{
				return(Convert.ToDouble(DBFunctions.SingleData("SELECT VALOR_PORCENTAJE FROM DBXSCHEMA.TPORCENTAJESTRANSPORTES WHERE CLAVE='PREPAGO';")));
			}
			catch
			{
				return(0);}
		}
		//Devuelve siguiente tiquete virtual
		public static string TraerSiguienteTiqueteVirtual()
		{
			//Se usan los tres primeros digitos del cod agencia en el numero tiquete para diferenciar los virtuales [1 000000 - 1000 000000)
			string tiquete=DBFunctions.SingleData("SELECT NUM_DOCUMENTO+1 FROM DBXSCHEMA.MTIQUETE_VIAJE WHERE NUM_DOCUMENTO<"+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+"000 AND NUM_DOCUMENTO>="+((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)).ToString()+" ORDER BY NUM_DOCUMENTO DESC FETCH FIRST 1 ROWS ONLY;");
			if(tiquete.Length==0)tiquete=((long)Math.Pow(10,AMS.Comercial.Tiquetes.lenTiquete)+1).ToString();
			return(tiquete);
		}
		//Generar txt del tiquete
		/*public static void GenerarTiquete(string numero)
		{
			string pathTiquetes = ConfigurationManager.AppSettings["PathToPapeleria"];
			FileInfo t = new FileInfo(pathTiquetes + "\\TIQ_" + numero + ".txt");
			StreamWriter Tex = new StreamWriter(t.FullName,false,Encoding.UTF8);

			DataSet dsTiquete=new DataSet();
			DataSet dsInfoTiquete=new DataSet();
			DataSet dsViaje=new DataSet();
			DataSet dsRuta=new DataSet();
			DataSet dsPuestos=new DataSet();
			
			int precio=0;
			string ruta,rutaPrincipal,agencia,strPlanilla;
			string strNumero=numero;
			if(strNumero.Length>Tiquetes.lenTiquete)
				strNumero=Convert.ToInt32(strNumero.Substring(strNumero.Length-Tiquetes.lenTiquete)).ToString();
			strNumero=new string('0',Tiquetes.lenTiquete-strNumero.Length)+strNumero;
			
			//Validar responsable
			string nitResponsable=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.susuario where susu_login='"+HttpContext.Current.User.Identity.Name.ToString().ToLower()+"';");
			//Tex.WriteLine("1234567890123456789012345678901234567890");
			if(nitResponsable.Length==0)
			{
				Tex.WriteLine("NO TIENE PERMISO");
				Tex.Close();
				return;}
			//Info general
			DBFunctions.Request(dsInfoTiquete, IncludeSchema.NO, "select * from dbxschema.ccampos_tiquete;");
			
			//Consultar tiquete
			string nitPlanillaManual=DBFunctions.SingleData("select mnit_nit from dbxschema.mnit where mnit_nombres='Planilla Manual';");
			DBFunctions.Request(dsTiquete, IncludeSchema.NO, 
				"SELECT MV.MPLA_CODIGO, MV.MRUT_CODIGO, MV.VALOR_PASAJE, MV.MNIT_COMPRADOR AS MNIT_COMPRADOR, COALESCE(MN.MNIT_NOMBRES CONCAT ' ' CONCAT MN.MNIT_APELLIDOS, '') AS COMPRADOR "+
				"FROM DBXSCHEMA.MTIQUETE_VIAJE MV LEFT JOIN DBXSCHEMA.MNIT MN ON  MN.MNIT_NIT=MV.MNIT_COMPRADOR "+
				"WHERE MV.TDOC_CODIGO='TIQ' AND MV.NUM_DOCUMENTO="+numero+" AND MV.MNIT_RESPONSABLE='"+nitResponsable+"';");
			if(dsTiquete.Tables[0].Rows.Count==0){
				Tex.WriteLine("NO EXISTE EL TIQUETE");
				Tex.Close();
				return;}
			
			strPlanilla=dsTiquete.Tables[0].Rows[0]["MPLA_CODIGO"].ToString();
			ruta=dsTiquete.Tables[0].Rows[0]["MRUT_CODIGO"].ToString();

			//Consultar ruta
			DBFunctions.Request(dsRuta, IncludeSchema.NO,
				"SELECT po.PCIU_NOMBRE AS ORIGEN, pd.PCIU_NOMBRE AS DESTINO "+
				"FROM DBXSCHEMA.PCIUDAD po, DBXSCHEMA.PCIUDAD pd, DBXSCHEMA.MRUTAS mr "+
				"WHERE po.PCIU_CODIGO=mr.PCIU_COD AND pd.PCIU_CODIGO=mr.PCIU_CODDES AND mr.MRUT_CODIGO='"+ruta+"';");
			if(dsRuta.Tables[0].Rows.Count==0)
			{
				Tex.WriteLine("NO EXISTE LA RUTA");
				Tex.Close();
				return;}
			
			//Consultar planilla, viaje
			DBFunctions.Request(dsViaje, IncludeSchema.NO,
				"SELECT mv.MCAT_PLACA as PLACA, mn.MNIT_NOMBRES concat ' ' concat mn.MNIT_APELLIDOS AS CONDUCTOR , mb.MBUS_NUMERO AS NUMERO, mp.MRUT_CODIGO AS RUTAP, mp.MAG_CODIGO as AGENCIA, mv.TPROG_TIPOPROG as PROGRAMACION "+
				"FROM DBXSCHEMA.MPLANILLAVIAJE mp "+
				"left join DBXSCHEMA.MVIAJE mv on mp.MRUT_CODIGO=mv.MRUT_CODIGO and mp.MVIAJE_NUMERO=mv.MVIAJE_NUMERO "+
				"left join DBXSCHEMA.MNIT mn on mn.MNIT_NIT=mv.MNIT_CONDUCTOR "+
				"left join DBXSCHEMA.MBUSAFILIADO mb on mb.MCAT_PLACA=mv.MCAT_PLACA "+
				"WHERE mp.MPLA_CODIGO="+strPlanilla);
			if(dsViaje.Tables[0].Rows.Count==0)
			{
				Tex.WriteLine("NO EXISTE EL VIAJE");
				Tex.Close();
				return;}
			
			//Consultar puestos
			string strPuestos="";
			DBFunctions.Request(dsPuestos, IncludeSchema.NO,"SELECT MELE_NUMEPUES FROM DBXSCHEMA.MCONFIGURACIONPUESTO WHERE TDOC_CODIGO='TIQ' AND NUM_DOCUMENTO="+numero+" ORDER BY MELE_NUMEPUES;");
			if(dsPuestos.Tables[0].Rows.Count==0){
				Tex.WriteLine("NO HAY PUESTOS REGISTRADOS");
				Tex.Close();
				return;}
			//Agencia de paso? No tener en cuenta el puesto
			agencia=dsViaje.Tables[0].Rows[0]["AGENCIA"].ToString();
			rutaPrincipal=dsViaje.Tables[0].Rows[0]["RUTAP"].ToString();
			if(!DBFunctions.RecordExist(
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
			
			Tex.WriteLine(new String('*',anchoTiquete));
			if(dsInfoTiquete.Tables[0].Rows.Count>0){
				Tex.WriteLine(CentrarTexto(dsInfoTiquete.Tables[0].Rows[0]["NOMBRE_EMPRESA"].ToString(),' '));
				Tex.WriteLine(CentrarTexto("NIT. "+dsInfoTiquete.Tables[0].Rows[0]["NIT_EMPRESA"].ToString(),' '));}
			Tex.WriteLine(new String('*',anchoTiquete));
			Tex.WriteLine("");
			Tex.WriteLine("Número : "+strNumero);
			if(!dsTiquete.Tables[0].Rows[0]["MNIT_COMPRADOR"].ToString().Equals(nitPlanillaManual))
				Tex.WriteLine(CortarTexto("Nombre : "+CortarTexto(dsTiquete.Tables[0].Rows[0]["COMPRADOR"].ToString())));
			
			if(dsViaje.Tables[0].Rows[0]["PROGRAMACION"].ToString().Length>0)
				Tex.WriteLine("Fecha  :  "+DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
			else
				Tex.WriteLine("Fecha  :  "+DateTime.Now.ToString("dd/MM/yyyy"));

			Tex.WriteLine("Bus    :  "+dsViaje.Tables[0].Rows[0]["NUMERO"].ToString()+" ("+dsViaje.Tables[0].Rows[0]["PLACA"].ToString()+")");
			Tex.WriteLine(AjustarTexto("Puesto :  "+strPuestos));
			Tex.WriteLine("Origen :  "+dsRuta.Tables[0].Rows[0]["ORIGEN"].ToString());
			Tex.WriteLine("Destino:  "+dsRuta.Tables[0].Rows[0]["DESTINO"]);
			Tex.WriteLine("Pasajes:  "+dsPuestos.Tables[0].Rows.Count.ToString());
			Tex.WriteLine("Valor  :  "+precio.ToString("#,###"));
			Tex.WriteLine("Total  :  "+(dsPuestos.Tables[0].Rows.Count*precio).ToString("#,###"));
			Tex.Close();
		}*/

		public static string CentrarTexto(string original, char llenar){
			int l=(int)((anchoTiquete-original.Length)/2);
			if(original.Length>anchoTiquete)return(original);
			else return(new String(llenar,l)+original+new String(llenar,anchoTiquete-original.Length-l));
		}

		public static string CortarTexto(string original, int ancho)
		{
			string TextoCorto;
			if(original.Length>ancho)TextoCorto = original.Substring(0,ancho);
			else TextoCorto = original;
			return(TextoCorto.Replace("\r\n"," "));
		
		}
		public static string ReemplazarTexto(string original)
		{
			string TextoCorto = original;
			return(TextoCorto.Replace("\r\n"," "));
		}
		public static string CortarTexto(string original)
		{
			string TextoCorto;
			if(original.Length>anchoTiquete) TextoCorto = original.Substring(0,anchoTiquete);
			else TextoCorto = original;
			return(TextoCorto.Replace("\r\n"," "));
		}

		public static string AjustarTexto(string original)
		{
			string resultado="";
			for(int n=1;n<=original.Length;n++){
				resultado+=original[n-1];
				if(n!=original.Length && (n%anchoTiquete)==0)
					resultado+=nuevaLinea;
			}
			return(resultado);
		}
	}
}
