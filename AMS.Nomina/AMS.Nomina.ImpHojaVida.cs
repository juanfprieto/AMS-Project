using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using AMS.DB;
using AMS.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using AMS.Tools;
using System.Configuration;
using System.Web;

namespace AMS.Nomina
{

	public class ImpHojaVida : System.Web.UI.UserControl
	{
		protected DropDownList DDLEMPLEADO;
		protected Button BTNMOSTRAR;
		protected CrystalDecisions.CrystalReports.Engine.ReportDocument reporte;
		protected System.Web.UI.WebControls.Button BTNDETALLADA;
		protected System.Web.UI.WebControls.Button BTNSIMPLE;
		protected CrystalDecisions.Web.CrystalReportViewer visor;
	
		protected void generarrpt(object sender, EventArgs e)
		{
		    string select="SELECT MEMP.MEMP_CODIEMPL,MNIT.MNIT_NOMBRES,MNIT.MNIT_NOMBRE2,MNIT.MNIT_APELLIDOS,MNIT.MNIT_APELLIDO2,MNIT.MNIT_DIGITO,MNIT.MNIT_DIRECCION,MNIT.MNIT_EMAIL,MNIT.MNIT_TELEFONO,MNIT.MNIT_CELULAR, MEMP.MEMP_FECHNACI,PCIU.PCIU_NOMBRE,MNIT.MNIT_NIT,MNIT.MNIT_DIGITO,MEMP.MEMP_NUMELIBRMILI,MEMP.MEMP_CLASELIBRMILI,DBXSCHEMA.TSEXO.TSEX_NOMBRE,YEAR(CURRENT DATE) - YEAR(MEMP.MEMP_FECHNACI),MEMP.MEMP_NUMEHIJOS,MEMP.TRES_VIVIENDA,MEMP.MEMP_FECHINGRESO,MEMP.MEMP_SUELACTU,MEMP.MEMP_SUELANTER,TCON.TCON_NOMBRE,TSUBT.TSUB_NOMBRE, PDEPTO.PDEP_NOMBDPTO,TESTEMP.TEST_NOMBRE,DBXSCHEMA.TFORMAPAGO.TFOR_DESCRIPCION,DBXSCHEMA.PBANCO.PBAN_NOMBRE,MEMP.MEMP_CUENNOMI,DBXSCHEMA.PEPS.PEPS_NOMBEPS,DBXSCHEMA.PARP.PARP_NOMBARP,PFCESA.PFON_NOMBFOND,PFPENS.PFON_NOMBPENS,DBXSCHEMA.TRETEFTE.TEST_DESCRIPCION, MEMP.MEMP_PORCRETE,TSANGRE.TTIP_TIPOSANG ,PCAR.pcar_nombcarg,TCIVIL.test_nombre,MEMP.memp_fecsuelanter,memp_peripago,TPAGO.TPER_DESCP,TFPAGO.tfor_descripcion,MEMP.memp_cuennomi,PFPENS2.pfon_nombpens as pensvol";
		    string from=" FROM DBXSCHEMA.MEMPLEADO MEMP,DBXSCHEMA.MNIT MNIT,DBXSCHEMA.PCIUDAD PCIU,DBXSCHEMA.TSEXO,DBXSCHEMA.TCONTRATONOMINA TCON,DBXSCHEMA.TSUBSIDIOTRANSPORTE TSUBT ,DBXSCHEMA.PDEPARTAMENTOEMPRESA PDEPTO,DBXSCHEMA.TESTADOEMPLEADO TESTEMP, DBXSCHEMA.TFORMAPAGO,DBXSCHEMA.PBANCO,DBXSCHEMA.PEPS,DBXSCHEMA.PARP,DBXSCHEMA.PFONDOPENSION PFPENS,DBXSCHEMA.PFONDOPENSION PFPENS2, DBXSCHEMA.PFONDOCESANTIAS PFCESA,DBXSCHEMA.TRETEFTE,DBXSCHEMA.TTIPOSANGRE TSANGRE,DBXSCHEMA.PCARGOEMPLEADO PCAR,dbxschema.testadocivil TCIVIL,dbxschema.TPERIPAGO TPAGO,DBXSCHEMA.TFORMAPAGO TFPAGO";	
		    string where=" WHERE MEMP.MEMP_CODIEMPL='"+DDLEMPLEADO.SelectedValue+"' AND PFPENS.pfon_codipens=MEMP.pfon_codipens AND PFPENS2.pfon_codipens=MEMP.pfon_codipensvolu AND TFPAGO.tfor_pago=memp.memp_formpago AND TPAGO.tper_peri=MEMP.memp_peripago AND TCIVIL.test_estacivil=memp.test_estacivil AND MEMP.PCAR_CODICARGO=PCAR.PCAR_CODICARGO AND MEMP.MNIT_NIT=MNIT.MNIT_NIT  and memp.ttip_secuencia= TSANGRE.ttip_secuencia  AND MEMP.PCIU_LUGANACI=PCIU.PCIU_CODIGO  AND MEMP.TSEX_CODIGO=DBXSCHEMA.TSEXO.TSEX_CODIGO AND MEMP.TCON_CONTRATO= TCON.TCON_CONTRATO AND MEMP.TSUB_CODIGO=TSUBT.TSUB_CODIGO AND MEMP.PDEP_CODIDPTO= PDEPTO.PDEP_CODIDPTO AND MEMP.TEST_ESTADO= TESTEMP.TEST_ESTADO AND MEMP.MEMP_FORMPAGO=DBXSCHEMA.TFORMAPAGO.TFOR_PAGO  AND MEMP.PBAN_CODIGO=DBXSCHEMA.PBANCO.PBAN_CODIGO AND MEMP.PEPS_CODIEPS=DBXSCHEMA.PEPS.PEPS_CODIEPS AND MEMP.PARP_CODIARP=DBXSCHEMA.PARP.PARP_CODIARP AND MEMP.PFON_CODICESA=PFCESA.PFON_CODIFOND AND MEMP.MEMP_TESTRETE=DBXSCHEMA.TRETEFTE.TEST_RETE";
            string nomEmpleado = DBFunctions.SingleData("SELECT NOMBRE FROM VMNIT VM, MEMPLEADO ME WHERE VM.MNIT_NIT = ME.MNIT_NIT AND ME.MEMP_CODIEMPL = '" + DDLEMPLEADO.SelectedValue + "'");
		    DataSet ds= new DataSet();
		
		    DBFunctions.Request(ds,IncludeSchema.NO,select+from+where+";select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa");
            if(ds.Tables[0].Rows.Count > 0)
            {
                ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath, "schemas/Nomina.InfyConsultas.rpte_ImpHojaVida.xsd"));
                reporte = new ReportDocument();
                reporte.Load(Path.Combine(Request.PhysicalApplicationPath, "rpt/Nomina.InfyConsultas.rpte_ImpHojaVida.rpt"));
                reporte.SetDataSource(ds);
                visor.ReportSource = reporte;
                visor.DataBind();
                reporte.ExportToDisk(ExportFormatType.WordForWindows, Path.Combine(Request.PhysicalApplicationPath, "rptgen/Nomina.InfyConsultas.rpte_ImpHojaVida.doc"));
                Response.Clear();
                Response.ContentType = "application/msword";
                Response.AddHeader("Content-Disposition", "attachment; filename= " + nomEmpleado + ".doc");
                Response.ContentType = "application/msword";
                Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath, "rptgen/Nomina.InfyConsultas.rpte_ImpHojaVida.doc"));
                Response.Flush();
                Response.Close();
            }
            else
            {
                Utils.MostrarAlerta(Response, "Existe un prolema con la consulta, el vendedor no tiene la información completa");
            }
		    
		}
		
		
		protected void generarrptdetallado ( object sender, EventArgs e)
		{
            TableLogOnInfo InfoConexBd;
            ExportOptions exportOpts = new ExportOptions();
            PdfRtfWordFormatOptions PDFFormatOpts = new PdfRtfWordFormatOptions();
            DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();

            string tem = "";
            //SACAR EL NIT 
            string nit=DBFunctions.SingleData("select MNIT.mnit_nit from dbxschema.mempleado MEMP, dbxschema.mnit MNIT where MEMP.mnit_nit=MNIT.mnit_nit and MEMP.memp_codiempl='"+DDLEMPLEADO.SelectedValue+"'");
		
		    string select="SELECT MEMP.MEMP_CODIEMPL,MNIT.MNIT_NOMBRES,MNIT.MNIT_NOMBRE2,MNIT.MNIT_APELLIDOS,MNIT.MNIT_APELLIDO2,MNIT.MNIT_DIGITO,MNIT.MNIT_DIRECCION,MNIT.MNIT_EMAIL,MNIT.MNIT_TELEFONO,MNIT.MNIT_CELULAR, MEMP.MEMP_FECHNACI,PCIU.PCIU_NOMBRE,MNIT.MNIT_NIT,MNIT.MNIT_DIGITO,MEMP.MEMP_NUMELIBRMILI,MEMP.MEMP_CLASELIBRMILI,DBXSCHEMA.TSEXO.TSEX_NOMBRE,YEAR(CURRENT DATE)- YEAR(MEMP.MEMP_FECHNACI),MEMP.MEMP_NUMEHIJOS,MEMP.TRES_VIVIENDA,MEMP.MEMP_FECHINGRESO,MEMP.MEMP_SUELACTU,MEMP.MEMP_SUELANTER,TCON.TCON_NOMBRE,TSUBT.TSUB_NOMBRE, PDEPTO.PDEP_NOMBDPTO,TESTEMP.TEST_NOMBRE,DBXSCHEMA.TFORMAPAGO.TFOR_DESCRIPCION,DBXSCHEMA.PBANCO.PBAN_NOMBRE,MEMP.MEMP_CUENNOMI,DBXSCHEMA.PEPS.PEPS_NOMBEPS,DBXSCHEMA.PARP.PARP_NOMBARP,PFCESA.PFON_NOMBFOND,PFPENS.PFON_NOMBPENS,DBXSCHEMA.TRETEFTE.TEST_DESCRIPCION, MEMP.MEMP_PORCRETE,TSANGRE.TTIP_TIPOSANG ,PCAR.pcar_nombcarg,TCIVIL.test_nombre,MEMP.memp_fecsuelanter,memp_peripago,TPAGO.TPER_DESCP,TFPAGO.tfor_descripcion,MEMP.memp_cuennomi,PFPENS2.pfon_nombpens as pensvol";
		    string from=" FROM DBXSCHEMA.MEMPLEADO MEMP,DBXSCHEMA.MNIT MNIT,DBXSCHEMA.PCIUDAD PCIU,DBXSCHEMA.TSEXO,DBXSCHEMA.TCONTRATONOMINA TCON,DBXSCHEMA.TSUBSIDIOTRANSPORTE TSUBT ,DBXSCHEMA.PDEPARTAMENTOEMPRESA PDEPTO,DBXSCHEMA.TESTADOEMPLEADO TESTEMP, DBXSCHEMA.TFORMAPAGO,DBXSCHEMA.PBANCO,DBXSCHEMA.PEPS,DBXSCHEMA.PARP,DBXSCHEMA.PFONDOPENSION PFPENS,DBXSCHEMA.PFONDOPENSION PFPENS2, DBXSCHEMA.PFONDOCESANTIAS PFCESA,DBXSCHEMA.TRETEFTE,DBXSCHEMA.TTIPOSANGRE TSANGRE,DBXSCHEMA.PCARGOEMPLEADO PCAR,dbxschema.testadocivil TCIVIL,dbxschema.TPERIPAGO TPAGO,DBXSCHEMA.TFORMAPAGO TFPAGO";	
		    string where=" WHERE MEMP.MEMP_CODIEMPL='"+DDLEMPLEADO.SelectedValue+"' AND PFPENS.pfon_codipens=MEMP.pfon_codipens AND PFPENS2.pfon_codipens=MEMP.pfon_codipensvolu AND TFPAGO.tfor_pago=memp.memp_formpago AND TPAGO.tper_peri=MEMP.memp_peripago AND TCIVIL.test_estacivil=memp.test_estacivil AND MEMP.PCAR_CODICARGO=PCAR.PCAR_CODICARGO AND MEMP.MNIT_NIT=MNIT.MNIT_NIT  and memp.ttip_secuencia= TSANGRE.ttip_secuencia  AND MEMP.PCIU_LUGANACI=PCIU.PCIU_CODIGO  AND MEMP.TSEX_CODIGO=DBXSCHEMA.TSEXO.TSEX_CODIGO AND MEMP.TCON_CONTRATO= TCON.TCON_CONTRATO AND MEMP.TSUB_CODIGO=TSUBT.TSUB_CODIGO AND MEMP.PDEP_CODIDPTO= PDEPTO.PDEP_CODIDPTO AND MEMP.TEST_ESTADO= TESTEMP.TEST_ESTADO AND MEMP.MEMP_FORMPAGO=DBXSCHEMA.TFORMAPAGO.TFOR_PAGO  AND MEMP.PBAN_CODIGO=DBXSCHEMA.PBANCO.PBAN_CODIGO AND MEMP.PEPS_CODIEPS=DBXSCHEMA.PEPS.PEPS_CODIEPS AND MEMP.PARP_CODIARP=DBXSCHEMA.PARP.PARP_CODIARP AND MEMP.PFON_CODICESA=PFCESA.PFON_CODIFOND AND MEMP.MEMP_TESTRETE=DBXSCHEMA.TRETEFTE.TEST_RETE";
		    DataSet ds= new DataSet();

            //original
            //string detalle1 = "select MNOM.mnom_fecha,PEXA.pexa_nombre,MNOM.mnom_descripc,MNOM.mnom_resultado,MNOM.mnom_comentario,MNOM.mnom_evaluador from dbxschema.mnominaseleccion MNOM,dbxschema.pexamen PEXA where MNOM.mnit_nit='" + nit + "' and MNOM.pexa_codigo=PEXA.pexa_codigo;";
            //string detalle2 = "SELECT MNOM.mnom_nombre,PPAR.ppar_detalle,MNOM.mnon_nitfami,MNOM.tres_sino,PPROF.ppro_nombre,MNOM.mnom_direccion,PCIU.pciu_nombre,MNOM.mnom_telefono FROM dbxschema.mnominafamilia MNOM,dbxschema.pciudad PCIU,dbxschema.mnit MNIT,dbxschema.pprofesion PPROF,dbxschema.pparentesco PPAR,dbxschema.trespuestasino TRES WHERE MNOM.mnit_nit='" + nit + "' and MNOM.mnit_nit=MNIT.mnit_nit and MNOM.pciu_codigo=PCIU.pciu_codigo and MNOM.ppro_codigo=PPROF.ppro_codigo and MNOM.ppar_codigo=PPAR.ppar_codigo and MNOM.tres_sino=TRES.tres_sino;";
            //string detalle3 = "select PPROF.ppro_nombre,MNOM.mnon_estanivel,MNOM.tres_sino,MNOM.mnom_fechnaci,MNOM.mnom_universidad,PCIU.pciu_nombre from dbxschema.mnominanivelacademico MNOM, dbxschema.pciudad PCIU, dbxschema.mnit MNIT,dbxschema.pprofesion PPROF,dbxschema.trespuestasino TRES where  MNOM.mnit_nit='" + nit + "' AND MNIT.mnit_nit=MNOM.mnit_nit AND MNOM.pciu_codigo=PCIU.pciu_codigo AND PPROF.ppro_codigo=MNOM.ppro_codigo AND MNOM.tres_sino=TRES.tres_sino;";
            //string detalle4 = "select MSEG.mnom_fecha, MSEG.mnom_descripc from dbxschema.mnominaseguimiento MSEG,dbxschema.paccionnomina PACC,dbxschema.palmacen PALM,dbxschema.mnit MNIT where MSEG.mnit_nit='" + nit + "' and MNIT.mnit_nit=MSEG.mnit_nit and PACC.pacc_codiacci=MSEG.pacc_codiacci and MSEG.palm_codigo=PALM.palm_almacen;";
            //string detalle5 = "select dvac_fechinic,dvac_fechfinal,dvac_tiem,dvac_dine from dbxschema.dvacaciones DVACA, dbxschema.mvacaciones MVACA where DVACA.mvac_secuencia=MVACA.mvac_secuencia and MVACA.memp_codiemp='" + DDLEMPLEADO.SelectedValue + "';";
            //string detalle6 = "select MPRE.mpre_numelibr,MPRE.mpre_fechprest,MPRE.mpre_valorpres,MPRE.mpre_numecuot,MPRE.mpre_porcinte,MPRE.mpre_valopaga  from dbxschema.mprestamoempleados MPRE,dbxschema.testadoprestamo TPRES  where TPRES.test_estprestamo=1 and MPRE.memp_codiempL='" + DDLEMPLEADO.SelectedValue + "'";s

            string detalle1 = "select MNOM.mnom_fecha,PEXA.pexa_nombre,MNOM.mnom_descripc,MNOM.mnom_resultado,MNOM.mnom_comentario,MNOM.mnom_evaluador from dbxschema.mnominaseleccion MNOM,dbxschema.pexamen PEXA where MNOM.MEMP_CODIEMPL='" + DDLEMPLEADO.SelectedValue + "' and MNOM.pexa_codigo=PEXA.pexa_codigo;";
            string detalle2 = "SELECT MNOM.mnom_nombre,PPAR.ppar_detalle,MNOM.mnon_nitfami,MNOM.tres_sino,PPROF.ppro_nombre,MNOM.mnom_direccion,PCIU.pciu_nombre,MNOM.mnom_telefono FROM dbxschema.mnominafamilia MNOM,dbxschema.pciudad PCIU,dbxschema.pprofesion PPROF,dbxschema.pparentesco PPAR,dbxschema.trespuestasino TRES WHERE MNOM.MEMP_CODIEMPL='" + DDLEMPLEADO.SelectedValue + "'  and MNOM.pciu_codigo=PCIU.pciu_codigo and MNOM.ppro_codigo=PPROF.ppro_codigo and MNOM.ppar_codigo=PPAR.ppar_codigo and MNOM.tres_sino=TRES.tres_sino;";
            string detalle3 = "select PPROF.ppro_nombre,MNOM.mnon_estanivel,MNOM.tres_sino,MNOM.mnom_fechnaci,MNOM.mnom_universidad,PCIU.pciu_nombre from dbxschema.mnominanivelacademico MNOM, dbxschema.pciudad PCIU, dbxschema.pprofesion PPROF,dbxschema.trespuestasino TRES where  MNOM.MEMP_CODIEMPL='" + DDLEMPLEADO.SelectedValue + "'  AND MNOM.pciu_codigo=PCIU.pciu_codigo AND PPROF.ppro_codigo=MNOM.ppro_codigo AND MNOM.tres_sino=TRES.tres_sino;";
            string detalle4 = "select MSEG.mnom_fecha, MSEG.mnom_descripc from dbxschema.mnominaseguimiento MSEG,dbxschema.paccionnomina PACC,dbxschema.palmacen PALM where MSEG.MEMP_CODIEMPL='" + DDLEMPLEADO.SelectedValue + "'  and PACC.pacc_codiacci=MSEG.pacc_codiacci and MSEG.palm_codigo=PALM.palm_almacen;";
            string detalle5 = "select dvac_fechinic,dvac_fechfinal,dvac_tiem,dvac_dine from dbxschema.dvacaciones DVACA, dbxschema.mvacaciones MVACA where DVACA.mvac_secuencia=MVACA.mvac_secuencia and MVACA.memp_codiemp='" + DDLEMPLEADO.SelectedValue + "';";
            string detalle6 = "select MPRE.mpre_numelibr,MPRE.mpre_fechprest,MPRE.mpre_valorpres,MPRE.mpre_numecuot,MPRE.mpre_porcinte,MPRE.mpre_valopaga  from dbxschema.mprestamoempleados MPRE,dbxschema.testadoprestamo TPRES  where TPRES.test_estprestamo=1 and MPRE.memp_codiempL='" + DDLEMPLEADO.SelectedValue + "'";

            DBFunctions.Request(ds,IncludeSchema.NO,select+from+where+";select cemp_nombre,cemp_nombcome,mnit_nit from dbxschema.cempresa;"+detalle1+detalle2+detalle3+detalle4+detalle5+detalle6);
		    ds.WriteXmlSchema(Path.Combine(Request.PhysicalApplicationPath,"schemas/Nomina.InfyConsultas.rpte_ImpHojaVidaDetallada2.xsd"));
		    reporte=new ReportDocument();
            reporte.Load(Path.Combine(Request.PhysicalApplicationPath, "rpt/Nomina.InfyConsultas.rpte_ImpHojaVidaDetallada2.rpt"));
            reporte.SetDataSource(ds);
            visor.ReportSource = reporte;
            visor.DataBind();
            reporte.ExportToDisk(ExportFormatType.WordForWindows, Path.Combine(Request.PhysicalApplicationPath, "rptgen/Nomina.InfyConsultas.rpte_ImpHojaVidaDetallada.doc"));
            //TableLogOnInfo crTableLogOnInfo = new TableLogOnInfo();
            //ConnectionInfo crConnectionInfo = new ConnectionInfo();
            //crConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];// para 2015

            //CrystalDecisions.CrystalReports.Engine.Database crDatabase;
            //CrystalDecisions.CrystalReports.Engine.Tables crTables;

            //crDatabase = reporte.Database;
            //crTables = crDatabase.Tables;
            //foreach (CrystalDecisions.CrystalReports.Engine.Table tabla1 in reporte.Database.Tables)
            //{
            //    tem = tabla1.Name.ToString();
            //    InfoConexBd = tabla1.LogOnInfo;
            //    //InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase"];
            //    InfoConexBd.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()]; // para 2015
            //    InfoConexBd.ConnectionInfo.UserID = "db2admin";
            //    InfoConexBd.ConnectionInfo.Password = ".ecas2010.";
            //    InfoConexBd.TableName = "DBXSCHEMA." + tem;
            //    tabla1.ApplyLogOnInfo(InfoConexBd);
            //}

            //exportOpts.ExportFormatType = ExportFormatType.WordForWindows;
            //exportOpts.FormatOptions = PDFFormatOpts;
            //exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
            //string nomA =  "_" + HttpContext.Current.User.Identity.Name.ToLower() + ".doc";
            //string documento = ConfigurationManager.AppSettings["PathToPreviews"] + nomA;
            //diskOpts.DiskFileName = ConfigurationManager.AppSettings["PathToReports"] + nomA;
            //exportOpts.DestinationOptions = diskOpts;
            //reporte.Export();
            //reporte.SetDatabaseLogon("db2admin", ".ecas2010.", "AUTOPRUE", "AUTOPRUE");


            Response.ClearContent();
		    Response.ClearHeaders();
		    Response.ContentType = "application/msword";
		    Response.WriteFile(Path.Combine(Request.PhysicalApplicationPath,"rptgen/Nomina.InfyConsultas.rpte_ImpHojaVidaDetallada2.doc"));
		}
		
		
		protected void Page_Load(object Sender,EventArgs e)
		{
			if (!IsPostBack)
			{
				DatasToControls param = new DatasToControls();
				param.PutDatasIntoDropDownList(DDLEMPLEADO,"SELECT M.MEMP_CODIEMPL, M.MEMP_CODIEMPL CONCAT ' ' CONCAT N.MNIT_APELLIDOS CONCAT ' ' CONCAT N.MNIT_NOMBRES FROM DBXSCHEMA.MEMPLEADO M, DBXSCHEMA.MNIT N WHERE M.MNIT_NIT=N.MNIT_NIT");
				
			}
			visor.Visible=false;
		}
		
		
		////////////////////////////////////////////////
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		
		
		
		
		
	}
	
	
	
}
