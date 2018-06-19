using System;
using AMS.DB;
using System.Data;

namespace AMS.Nomina
{
	/// <summary>
	/// Descripción breve de CNomina.
	/// </summary>
	public class CNomina
	{
		public string CNOM_SECUENCIA;           
		public string CNOM_SALAMINIACTU;        
		public string CNOM_SUBSTRANPERINOMI;     
		public string CNOM_PORCEPSEMPL;          
		public string CNOM_PORCFONDEMPL;       
		public string CNOM_PORCFONDSOLIPENS;    
		public string CNOM_ANO;               
		public string CNOM_MES;                
		public string CNOM_QUINCENA;              
		public string CNOM_TOPEPAGOAUXITRAN;    
		public string CNOM_TOPEPAGOFONDSOLIPENS; 
		public string CNOM_SUBSTRANANTE;          
		public string CNOM_SALAMINIANTE;          
		public string CNOM_EPSPERINOMI;           
		public string CNOM_RETEFETEPERINOMI;      
		public string CNOM_SUBSTRANSACTU;        
		public string CNOM_NOMBPAGA;              
		public string CNOM_CARGPAGA;              
		public string CNOM_IDENPAGA;            
		public string CNOM_CONCSALACODI;         
		public string CNOM_CONCSALAINT;          
		public string CNOM_CONCSALASENA;    
		public string CNOM_CONCRTDOCODI;          
		public string CNOM_CONCRFTECODI;          
		public string CNOM_CONCCESACODI;         
		public string CNOM_CONCHEDICODI;        
		public string CNOM_CONCHEFNCODI;          
		public string CNOM_CONCHEFDCODI;          
		public string CNOM_CONCFONDCODI;         
		public string CNOM_CONCFONDPENSVOLU;     
		public string CNOM_CONCSUBTCODI;          
		public string CNOM_CONCEPSCODI;           
		public string CNOM_CONCINTECESACODI;      
		public string CNOM_CONCPRIMNORMCODI;      
		public string CNOM_CONCPRIMAJUSCODI;      
		public string CNOM_CONCVACACODI;         
		public string CNOM_CONCPRESCODI;          
		public string CNOM_CONCPERMCODI;          
		public string CNOM_CONCFONDSOLIPENS;      
		public string CNOM_CONCRECNCODI;          
		public string CNOM_CONCHENOCODI;          
		public string CNOM_CONCAJUSUELDO;         
		public string CNOM_OPCIQUINOMENS;         
	//	public string CNOM_PAGOMENSTPER;         
		public string CNOM_BASEPORCSALAINTEG;     
		public string CNOM_CUENTAPUC;             
		public string CNOM_FORMPAGO;             
		public string CNOM_NUMCLIENTEBANCO;       
		public string CNOM_CODENTBANCARIA;       
		public string CNOM_NUMCUENTABANCO;        
		public string CNOM_CODSUCURSAL;           
		public string CNOM_LIQCOMBINADA;          
		public string CNOM_TOPEPAGOEPS;           
		public string CNOM_FORMAPAGOINCAP;        
		public string CNOM_UNIDVALOTRIB;         
		public string CNOM_CONCINDEMNIZACION;     

		public string PANO_DETALLE;
		public string TMES_NOMBRE;
		public string TPER_DESCRIPCION;
		public string TOPCI_DESCRIPCION;

        public string CNOM_CONCSALADESC;
        public string CNOM_CONSALAINTEDESC;
        public string CNOM_CONCSALASENADESC;
        public string CNOM_CONCRTDODESC;
        public string CNOM_CONCRFTEDESC;
        public string CNOM_CONCCESADESC;
        public string CNOM_CONCINTECESACDESC;
        public string CNOM_CONCFONDDESC;
        public string CNOM_CONCFONDPENSVOLUDESC;
        public string CNOM_CONCFONDSOLOPENSDESC;
        public string CNOM_CONCSUBTDESC;
        public string CNOM_CONCEPSDESC;
        public string CNOM_CONCPRIMDESC;
        public string CNOM_CONPRIMAJUSDESC;
        public string CNOM_CONCVACADESC;
        public string CNOM_CONCAJUSUELDODESC;
        public string CNOM_CONCINDEMNIZACIONDESC;
 
		public CNomina()
		{
			string Sql = "Select a.*,P.PANO_DETALLE,T.TMES_NOMBRE,N.TPER_DESCRIPCION,O.TOPCI_DESCRIPCION, ";
            Sql = Sql + " pcSALA.pcon_nombconc as CNOM_CONCSALADESC, pcSint.pcon_nombconc as CNOM_CONSALAINTEDESC, pcSENA.pcon_nombconc as CNOM_CONCSALASENADESC, ";
            Sql = Sql + " pcRTDO.pcon_nombconc as CNOM_CONCRTDODESC, pcrfte.pcon_nombconc as CNOM_CONCRFTEDESC,   pcCESA.pcon_nombconc as CNOM_CONCCESADESC, ";
            Sql = Sql + " pcINTECESA.pcon_nombconc as CNOM_CONCINTECESACDESC, pcFPEN.pcon_nombconc as CNOM_CONCFONDDESC, pcFPVO.pcon_nombconc as CNOM_CONCFONDPENSVOLUDESC, ";
            Sql = Sql + " pcFSPE.pcon_nombconc as CNOM_CONCFONDSOLOPENSDESC, pcSUBT.pcon_nombconc as CNOM_CONCSUBTDESC, pcEPS.pcon_nombconc as CNOM_CONCEPSDESC, ";
            Sql = Sql + " pcPRIM.pcon_nombconc as CNOM_CONCPRIMDESC, pcPRIMAJUS.pcon_nombconc as CNOM_CONPRIMAJUSDESC, pcVACA.pcon_nombconc as CNOM_CONCVACADESC, ";
            Sql = Sql + " pcAJUS.pcon_nombconc as CNOM_CONCAJUSUELDODESC, pcINDE.pcon_nombconc as CNOM_CONCINDEMNIZACIONDESC ";

            Sql = Sql + "from DBXSCHEMA.PANO P,DBXSCHEMA.TMES T,DBXSCHEMA.TPERIODONOMINA N,DBXSCHEMA.TOPCIQUINOMES O, dbxschema.cnomina a";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcSALA on a.CNOM_CONCSALACODI = pcSALA.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcSINT on a.CNOM_CONCSALAINT  = pcSINT.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcSENA on a.CNOM_CONCSALASENA = pcSENA.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcRTDO on a.CNOM_CONCRTDOCODI = pcRTDO.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcRFTE on a.CNOM_CONCRFTECODI = pcRFTE.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcCESA on a.CNOM_CONCCESACODI = pcCESA.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcINTECESA on a.CNOM_CONCINTECESACODI = pcINTECESA.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcFPEN on a.CNOM_CONCFONDCODI = pcFPEN.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcFPVO on a.CNOM_CONCFONDPENSVOLU = pcFPVO.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcFSPE on a.CNOM_CONCFONDSOLIPENS = pcFSPE.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcSUBT on a.CNOM_CONCSUBTCODI = pcSUBT.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcEPS  on a.CNOM_CONCEPSCODI  = pcEPS.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcPRIM on a.CNOM_CONCPRIMNORMCODI = pcPRIM.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcPRIMAJUS on a.CNOM_CONCPRIMAJUSCODI  = pcPRIMAJUS.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcVACA on a.CNOM_CONCVACACODI = pcVACA.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcAJUS on a.CNOM_CONCAJUSUELDO = pcAJUS.pcon_concepto ";
            Sql = Sql + " left join DBXSCHEMA.pconceptonomina as pcINDE on a.CNOM_CONCINDEMNIZACION = pcINDE.pcon_concepto ";
            
            Sql = Sql + " Where A.CNOM_ANO=P.PANO_ANO AND A.CNOM_MES=T.TMES_MES AND A.CNOM_QUINCENA=N.TPER_PERIODO AND A.CNOM_OPCIQUINOMENS=O.TOPCI_PERIODO";
			DataSet cnomina = new DataSet();

			DBFunctions.Request(cnomina,IncludeSchema.NO,Sql);

            CNOM_SECUENCIA      = cnomina.Tables[0].Rows[0]["CNOM_SECUENCIA"].ToString();
            CNOM_SALAMINIACTU   = cnomina.Tables[0].Rows[0]["CNOM_SALAMINIACTU"].ToString();
            CNOM_SUBSTRANPERINOMI = cnomina.Tables[0].Rows[0]["CNOM_SUBSTRANPERINOMI"].ToString();
            CNOM_PORCEPSEMPL    = cnomina.Tables[0].Rows[0]["CNOM_PORCEPSEMPL"].ToString();
            CNOM_PORCFONDEMPL   = cnomina.Tables[0].Rows[0]["CNOM_PORCFONDEMPL"].ToString();
            CNOM_PORCFONDSOLIPENS = cnomina.Tables[0].Rows[0]["CNOM_PORCFONDSOLIPENS"].ToString();
            CNOM_ANO            = cnomina.Tables[0].Rows[0]["CNOM_ANO"].ToString();
            CNOM_MES            = cnomina.Tables[0].Rows[0]["CNOM_MES"].ToString();
            CNOM_QUINCENA       = cnomina.Tables[0].Rows[0]["CNOM_QUINCENA"].ToString();
            CNOM_TOPEPAGOAUXITRAN = cnomina.Tables[0].Rows[0]["CNOM_TOPEPAGOAUXITRAN"].ToString();
            CNOM_TOPEPAGOFONDSOLIPENS = cnomina.Tables[0].Rows[0]["CNOM_TOPEPAGOFONDSOLIPENS"].ToString();
            CNOM_SUBSTRANANTE   = cnomina.Tables[0].Rows[0]["CNOM_SUBSTRANANTE"].ToString();
            CNOM_SALAMINIANTE   = cnomina.Tables[0].Rows[0]["CNOM_SALAMINIANTE"].ToString();
            CNOM_EPSPERINOMI    = cnomina.Tables[0].Rows[0]["CNOM_EPSPERINOMI"].ToString();
            CNOM_RETEFETEPERINOMI = cnomina.Tables[0].Rows[0]["CNOM_RETEFETEPERINOMI"].ToString();
            CNOM_SUBSTRANSACTU  = cnomina.Tables[0].Rows[0]["CNOM_SUBSTRANSACTU"].ToString();
            CNOM_NOMBPAGA       = cnomina.Tables[0].Rows[0]["CNOM_NOMBPAGA"].ToString();
            CNOM_CARGPAGA       = cnomina.Tables[0].Rows[0]["CNOM_CARGPAGA"].ToString();
            CNOM_IDENPAGA       = cnomina.Tables[0].Rows[0]["CNOM_IDENPAGA"].ToString();
            CNOM_CONCSALACODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCSALACODI"].ToString();
            CNOM_CONCSALAINT    = cnomina.Tables[0].Rows[0]["CNOM_CONCSALAINT"].ToString();
            CNOM_CONCSALASENA   = cnomina.Tables[0].Rows[0]["CNOM_CONCSALASENA"].ToString();   
            CNOM_CONCRTDOCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCRTDOCODI"].ToString();
            CNOM_CONCRFTECODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCRFTECODI"].ToString();
            CNOM_CONCCESACODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCCESACODI"].ToString();
            CNOM_CONCHEDICODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCHEDICODI"].ToString();
            CNOM_CONCHEFNCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCHEFNCODI"].ToString();
            CNOM_CONCHEFDCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCHEFDCODI"].ToString();
            CNOM_CONCFONDCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCFONDCODI"].ToString();
            CNOM_CONCFONDPENSVOLU = cnomina.Tables[0].Rows[0]["CNOM_CONCFONDPENSVOLU"].ToString();
            CNOM_CONCSUBTCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCSUBTCODI"].ToString();
            CNOM_CONCEPSCODI    = cnomina.Tables[0].Rows[0]["CNOM_CONCEPSCODI"].ToString();
            CNOM_CONCINTECESACODI = cnomina.Tables[0].Rows[0]["CNOM_CONCINTECESACODI"].ToString();
            CNOM_CONCPRIMNORMCODI = cnomina.Tables[0].Rows[0]["CNOM_CONCPRIMNORMCODI"].ToString();
            CNOM_CONCPRIMAJUSCODI = cnomina.Tables[0].Rows[0]["CNOM_CONCPRIMAJUSCODI"].ToString();
            CNOM_CONCVACACODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCVACACODI"].ToString();
            CNOM_CONCPRESCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCPRESCODI"].ToString();
            CNOM_CONCPERMCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCPERMCODI"].ToString();
            CNOM_CONCFONDSOLIPENS = cnomina.Tables[0].Rows[0]["CNOM_CONCFONDSOLIPENS"].ToString();
            CNOM_CONCRECNCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCRECNCODI"].ToString();
            CNOM_CONCHENOCODI   = cnomina.Tables[0].Rows[0]["CNOM_CONCHENOCODI"].ToString();
            CNOM_CONCAJUSUELDO  = cnomina.Tables[0].Rows[0]["CNOM_CONCAJUSUELDO"].ToString();
            CNOM_OPCIQUINOMENS  = cnomina.Tables[0].Rows[0]["CNOM_OPCIQUINOMENS"].ToString();
     //       CNOM_PAGOMENSTPER   = cnomina.Tables[0].Rows[0]["CNOM_PAGOMENSTPER"].ToString();
            CNOM_BASEPORCSALAINTEG = cnomina.Tables[0].Rows[0]["CNOM_BASEPORCSALAINTEG"].ToString();
            CNOM_CUENTAPUC      = cnomina.Tables[0].Rows[0]["CNOM_CUENTAPUC"].ToString();
            CNOM_FORMPAGO       = cnomina.Tables[0].Rows[0]["CNOM_FORMPAGO"].ToString();
            CNOM_NUMCLIENTEBANCO = cnomina.Tables[0].Rows[0]["CNOM_NUMCLIENTEBANCO"].ToString();
            CNOM_CODENTBANCARIA = cnomina.Tables[0].Rows[0]["CNOM_CODENTBANCARIA"].ToString();
            CNOM_NUMCUENTABANCO = cnomina.Tables[0].Rows[0]["CNOM_NUMCUENTABANCO"].ToString();
            CNOM_CODSUCURSAL    = cnomina.Tables[0].Rows[0]["CNOM_CODSUCURSAL"].ToString();
            CNOM_LIQCOMBINADA   = cnomina.Tables[0].Rows[0]["CNOM_LIQCOMBINADA"].ToString();
            CNOM_TOPEPAGOEPS    = cnomina.Tables[0].Rows[0]["CNOM_TOPEPAGOEPS"].ToString();
            CNOM_FORMAPAGOINCAP = cnomina.Tables[0].Rows[0]["CNOM_FORMAPAGOINCAP"].ToString();
            CNOM_UNIDVALOTRIB   = cnomina.Tables[0].Rows[0]["CNOM_UNIDVALOTRIB"].ToString();
            CNOM_CONCINDEMNIZACION = cnomina.Tables[0].Rows[0]["CNOM_CONCINDEMNIZACION"].ToString();

            PANO_DETALLE        = cnomina.Tables[0].Rows[0]["PANO_DETALLE"].ToString();
            TMES_NOMBRE         = cnomina.Tables[0].Rows[0]["TMES_NOMBRE"].ToString();
            TPER_DESCRIPCION    = cnomina.Tables[0].Rows[0]["TPER_DESCRIPCION"].ToString();
            TOPCI_DESCRIPCION   = cnomina.Tables[0].Rows[0]["TOPCI_DESCRIPCION"].ToString();

            //  Nombre o Descripcion de los conceptos Fijos de Nomina
            CNOM_CONCSALADESC   = cnomina.Tables[0].Rows[0]["CNOM_CONCSALADESC"].ToString();
            CNOM_CONSALAINTEDESC = cnomina.Tables[0].Rows[0]["CNOM_CONSALAINTEDESC"].ToString();
            CNOM_CONCSALASENADESC = cnomina.Tables[0].Rows[0]["CNOM_CONCSALASENADESC"].ToString();
            CNOM_CONCRTDODESC = cnomina.Tables[0].Rows[0]["CNOM_CONCRTDODESC"].ToString();
            CNOM_CONCRFTEDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCRFTEDESC"].ToString();
            CNOM_CONCCESADESC = cnomina.Tables[0].Rows[0]["CNOM_CONCCESADESC"].ToString();
            CNOM_CONCINTECESACDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCINTECESACDESC"].ToString();
            CNOM_CONCFONDDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCFONDDESC"].ToString();
            CNOM_CONCFONDPENSVOLUDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCFONDPENSVOLUDESC"].ToString();
            CNOM_CONCFONDSOLOPENSDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCFONDSOLOPENSDESC"].ToString();
            CNOM_CONCSUBTDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCSUBTDESC"].ToString();
            CNOM_CONCEPSDESC  = cnomina.Tables[0].Rows[0]["CNOM_CONCEPSDESC"].ToString();
            CNOM_CONCPRIMDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCPRIMDESC"].ToString();
            CNOM_CONPRIMAJUSDESC = cnomina.Tables[0].Rows[0]["CNOM_CONPRIMAJUSDESC"].ToString();
            CNOM_CONCVACADESC = cnomina.Tables[0].Rows[0]["CNOM_CONCVACADESC"].ToString();
            CNOM_CONCAJUSUELDODESC = cnomina.Tables[0].Rows[0]["CNOM_CONCAJUSUELDODESC"].ToString();
            CNOM_CONCINDEMNIZACIONDESC = cnomina.Tables[0].Rows[0]["CNOM_CONCINDEMNIZACIONDESC"].ToString();
 	
			cnomina.Dispose();
			
		}
	}
}
