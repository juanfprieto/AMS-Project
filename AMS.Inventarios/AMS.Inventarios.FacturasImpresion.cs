using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.Mail;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;

namespace AMS.Inventarios{
//IMPRIME FACTURAS
public class Facturas{
	static Facturas(){
	}
	//Crea un archivo temporal de texto para el usuario con la factura
    public static bool GenerarFactura(string arch,string TM, string pdoc, uint ndoc){//Tabla Maestro, prefijo doc, numero doc
		string TablaM, TablaD, prefDoc,sec;
		uint numDoc;
    	int lin=0,n;
	    TablaM=TM;
		prefDoc=pdoc;
		numDoc=ndoc;
    	string mFdesc,Logo="";
    	//bool varPgs=false;//varias paginas?
		//SeleccionarMFACTIMP
		string prm=DBFunctions.SingleData("SELECT cast(mfac_secuencia as char(3)) concat ',' concat mfac_descripcion concat ',' concat mfac_maestra concat ',' concat mfac_detalle concat ',' concat mfac_logo concat ',' concat mfac_variaspags concat ',' concat cast(mfac_renglones as char(3)) concat ',' concat cast(mfac_limitefila as char(3)) concat ',' concat cast(mfac_columnas as char(3)) from MFACTURAIMPRESION where mfac_maestra='"+TablaM+"'");
  		int Rengs=0,LimF=0,Cols=0;
    	string[]prms=prm.Split(',');
  		/*if(prms.Length!=8)
    		return(false);*/
    	sec=prms[0];
    	mFdesc=prms[1];//Descripcion
    	TablaM=prms[2];//maestro
    	TablaD=prms[3];//detalle
    	Logo=prms[4];//logo
    	/*if(prms[5]=="S")
    		varPgs=true;*/
 		Rengs=Convert.ToInt16(prms[6]);//Numero renglones reporte (Alto hoja)
 		Cols=Convert.ToInt16(prms[8]);//Nunmero de Columnas del reporte
 		LimF=Convert.ToInt16(prms[7]);//Limite fila de la grilla(Detalle)

		//Crear Archivo
		string Arch=arch;
		StreamWriter sw=File.CreateText(Arch);
		//Literales:
		DataSet ds=new DataSet();
		DBFunctions.Request(ds, IncludeSchema.NO,"SELECT DFAC_DESCRIPCION,DFAC_CAMPO,DFAC_FILA,DFAC_COLUMNA,DFAC_ANCHO,DFAC_TIPO FROM DFACTURAIMPRESION WHERE MFAC_SECUENCIA="+sec+" ORDER BY DFAC_FILA,DFAC_COLUMNA");
		DBFunctions.Request(ds, IncludeSchema.NO,"SELECT name from syscolumns where keysec <> NULL and tbname='"+TablaM+"' order by keysec");
		DBFunctions.Request(ds, IncludeSchema.NO,"SELECT * FROM "+TablaM+" WHERE ");
		int ncol,nlin,ancho;
    	string texto,desc,campo,lineaact="";
    	string spc=String.Format("{"+Cols.ToString()+",0}","");
    	string tipo;
    	ArrayList Lineas=new ArrayList();
    	for(n=0;n<ds.Tables[0].Rows.Count;n++){
    		ncol=Convert.ToInt16(ds.Tables[0].Rows[n]["DFAC_COLUMNA"]);//columna
    		desc=ds.Tables[0].Rows[n]["DFAC_DESCRIPCION"].ToString();
    		nlin=Convert.ToInt16(ds.Tables[0].Rows[n]["DFAC_FILA"]);
    		ancho=Convert.ToInt16(ds.Tables[0].Rows[n]["DFAC_ANCHO"]);
    		campo=ds.Tables[0].Rows[n]["DFAC_CAMPO"].ToString();
    		tipo=ds.Tables[0].Rows[n]["DFAC_TIPO"].ToString();
    		if(lin<nlin){
    			while(lin<nlin){
	    			Lineas.Add("");
    				lin++;}
    			Lineas.Add(spc);
    		}
    		texto="";
    		switch (tipo){
    			case "E"://Encabezado
    			
    			break;
    			case "D"://Detalle
    			break;
    			case "L"://Literales
    				texto=desc;
    			break;
				case "T"://Totales
				break;
    		}
    		lineaact=Lineas[Lineas.Count-1].ToString();
    		Lineas[Lineas.Count-1]=lineaact.Substring(0,ncol)+texto+lineaact.Substring(ncol+texto.Length);
    	}
    	for(n=0;n<Lineas.Count;n++)
    		sw.WriteLine(lineaact);
		sw.Close();
		return(true);
    }
    private void BajarLinea(){
    	
    }
}
}
