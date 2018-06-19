using System;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.Forms;
using AMS.DB;
using System.Data.Odbc;
using System.Configuration;
using AMS.Tools;


namespace AMS.Nomina
{
	
	/// <summary>
	///		Descripción breve de AMS_Nomina_PlanillaBanco.
	/// </summary>
	public class AMS_Nomina_PlanillaBanco : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Button btnGenerar;
		protected string nombreArchivo = "Conavi"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
		protected string nombreArchivoBogota = "Bank"+""+DateTime.Now.ToString("yyyyMMdd")+".txt";
        protected string ParaPagaVacacionesenPlanillaBanco = ConfigurationManager.AppSettings["PagaVacacionesenPlanillaBanco"];
        protected string PagaVacacionesenPlanillaBanco = "";

		protected	string directorioArchivo = ConfigurationManager.AppSettings["PathToDownloads"];
		protected	StreamWriter sw;
		//protected PlanillaPago comparacion = new PlanillaPago();
		//protected AnsiString  outputDatosConavi;
	
		string  outputDatosConavi;
		string  outputDatosBogota;
		protected PlanillaPago comparacion = new PlanillaPago();
		
		protected string numQuincena;
		protected System.Web.UI.WebControls.TextBox txtFechaAplicacion;
		protected System.Web.UI.WebControls.HyperLink hl;
        protected System.Web.UI.WebControls.ImageButton BtnImprimirExcel;
		protected System.Web.UI.WebControls.RadioButtonList rblBancos;
		protected System.Web.UI.WebControls.DropDownList DDLANO;
		protected System.Web.UI.WebControls.DropDownList DDLMES;
		protected System.Web.UI.WebControls.DropDownList DDLQUIN;
		DatasToControls bind = new DatasToControls();
		
		
		
		protected string path=ConfigurationManager.AppSettings["VirtualPathToDownloads"];

		private void Page_Load(object sender, System.EventArgs e)
		{
			if (!IsPostBack)
			{
				bind.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO, PANO_DETALLE FROM PANO ORDER BY 1 DESC");
				bind.PutDatasIntoDropDownList(DDLMES,"Select PMES_MES, PMES_NOMBRE from PMES ORDER BY 1 ");
				DDLQUIN.Items.Clear();
				bind.PutDatasIntoDropDownList(DDLQUIN,"SELECT TPER_PERIODO, TPER_DESCRIPCION FROM TPERIODONOMINA ORDER BY 1");
				ListItem ls = new ListItem("Prima de Servicios","P");
                DDLQUIN.Items.Add(ls); 
                ListItem lsv = new ListItem("Vacaciones Salamente", "V");
				DDLQUIN.Items.Add(lsv);
            }

            if (ParaPagaVacacionesenPlanillaBanco == "SI")
                PagaVacacionesenPlanillaBanco = " ";
            else
                PagaVacacionesenPlanillaBanco = " and DQUI.DQUI_VACACIONES NOT IN ('V') ";
        }

		protected void iniciarproceso()
		{
            if (DDLQUIN.SelectedValue.ToString() == "V")
            {
                PagaVacacionesenPlanillaBanco = " and DQUI.DQUI_VACACIONES IN ('V') ";
                DDLQUIN.SelectedValue = DBFunctions.SingleData("select mqui_tpernomi from dbxschema.mquincenas where mqui_anoquin=" + DDLANO.SelectedValue + " and mqui_mesquin=" + DDLMES.SelectedValue + " and mqui_ESTADO=1 ");
	        }

			if(rblBancos.SelectedValue=="CONAVI")
			{
				sw = File.CreateText(directorioArchivo+nombreArchivo);
				this.EscribirArchivoConavi();
			}
			else if(rblBancos.SelectedValue=="BOGOTA")
			{
				sw = File.CreateText(directorioArchivo+nombreArchivoBogota);
				this.EscribirArchivoBogota();
			}
			else
                Utils.MostrarAlerta(Response, "Seleccione un Banco por favor..");
		}

		protected void GenerarPlanillaBanco(object sender, EventArgs e)
		{
            bool errorFecha = false;
            if (txtFechaAplicacion.Text.Length != 6)
                errorFecha = true;
            if (Convert.ToInt32(txtFechaAplicacion.Text.Substring(0, 2)) == 0)
                errorFecha = true;
            if (Convert.ToInt32(txtFechaAplicacion.Text.Substring(2, 2)) > 12)
                errorFecha = true;
            if (Convert.ToInt32(txtFechaAplicacion.Text.Substring(2, 2)) == 0)
                errorFecha = true;
            if (Convert.ToInt32(txtFechaAplicacion.Text.Substring(4, 2)) > 31)
                errorFecha = true;
            if (Convert.ToInt32(txtFechaAplicacion.Text.Substring(4, 2)) == 0)
                errorFecha = true;
            if (errorFecha)
                Utils.MostrarAlerta(Response, "La fecha de aplicación está errada, corrijala por favor..");
            else
			    this.iniciarproceso();			
		}
		 
		private string quitarLineas(string cadena)
		{
			string retorna;
			retorna=cadena.Replace("-","");
			return retorna;
		}

		protected void EscribirArchivoBogota()
		{
			string numQuincena  = "";
			string Sql = "";
			if (DDLQUIN.SelectedValue.ToString() != "P")
			{
				numQuincena=DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+DDLANO.SelectedValue+" and mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" ");
			}
			else
			{
				numQuincena = "PRIMA";
			}

			if (numQuincena!=String.Empty)
			{
				int i=0;
				int registros=0;
				double sumapagado=0,sumapagadoemp=0;
				string tipoCuenta,tipoTransaccion;
				double contador=0;
				//Traigo todos los datos necesarios para la cabezera.
				DataSet DatosCabecera= new DataSet();
				DataSet DatosCuerpo= new DataSet();
                string CODBANCO = DBFunctions.SingleData("SELECT PBAN_CODIGO FROM DBXSCHEMA.PBANCO pb, DBXSCHEMA.cnomina cn WHERE pB.pban_codigo = CN.CNOM_CODENTBANCARIA;");
				if(CODBANCO!=String.Empty)
				{
					DBFunctions.Request(DatosCabecera,IncludeSchema.NO,"select cnom_numclientebanco,cnom_codentbancaria,cnom_codsucursal,cnom_numcuentabanco,cnom_formpago from dbxschema.cnomina");
					//Escribir el encabezado
					//Tipo registro	numérico	valor fijo “1”	1
					outputDatosBogota+="1";
					//Fecha a aplicar la dispersion		8		*
					outputDatosBogota+=comparacion.Completar_Campos(""+txtFechaAplicacion.Text+"",8,"0",false);
					//espacio en BLANCO 24
					outputDatosBogota+=comparacion.Completar_Campos("",24,"0",false);
					//Tipo cuenta cliente a debitar	alfanumérico	 aho 2 /   cte	1
					outputDatosBogota+=comparacion.Completar_Campos(""+DatosCabecera.Tables[0].Rows[0][4].ToString()+"",1,"0",false);
					//espacio en BLANCO 8
					outputDatosBogota+=comparacion.Completar_Campos("",8,"0",false);
					//Cuenta cliente a debitar	numérico 9	*
					outputDatosBogota+=comparacion.Completar_Campos(DatosCabecera.Tables[0].Rows[0][3].ToString(),9,"0",false);
					//Nombre entidad que envia	alfanumérico		40
					string nombreentidad=DBFunctions.SingleData("select cemp_nombre from DBXSCHEMA.CEMPRESA");
					outputDatosBogota+=comparacion.Completar_Campos(""+nombreentidad+"",40," ",true);
					//nit entidad 11
					string nitentidad=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.CEMPRESA");
					outputDatosBogota+=comparacion.Completar_Campos(""+nitentidad+"",11,"0",false);
					//tipo de movimiento 001-nomina,002-proveedores,003-transferencias,995-otros
					outputDatosBogota+=comparacion.Completar_Campos("001",3," ",true);
					//codigo de la ciudad segun tabla.
					outputDatosBogota+=comparacion.Completar_Campos("0001",4," ",true);
					//Fecha elaboracion	numérico	AAAAMMDD	8
					outputDatosBogota+=comparacion.Completar_Campos(""+DateTime.Now.ToString("yyMMdd")+"",8,"0",false);
					//CODIGO DE LA OFICINA
					outputDatosBogota+=comparacion.Completar_Campos(DatosCabecera.Tables[0].Rows[0][2].ToString(),3,"0",false);
					//TIPO DE IDENTIFICACION CLIENTE N:NIT
					outputDatosBogota+="N";
					//espacio en BLANCO 48
					outputDatosBogota+=comparacion.Completar_Campos("",48," ",false);
					//INDICADOR ADICIONAL SI:S,NO : ESP BLANCO
					outputDatosConavi+=" ";
					//espacio en BLANCO 80
					outputDatosBogota+=comparacion.Completar_Campos("",80," ",false);
			
					sw.WriteLine(outputDatosBogota);
					outputDatosBogota="";
					//2 REGISTRO MOVIMIENTO INF DEL BENEFICIARIO
			
					if (DDLQUIN.SelectedValue.ToString() != "P")
					{
                        Sql = "SELECT  MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CODSUCUEMPL,MEMP_CUENNOMI,MEMP_FORMPAGO,SUM(DQUI_APAGAR)-SUM(DQUI_ADESCONTAR),REPLACE(MNIT.mnit_apellidos,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_apellido2,''),'Ñ','N') concat ' ' concat REPLACE(MNIT.mnit_nombres,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_nombre2,''),'Ñ','N'), PBAN.PBAN_CODACH,MNIT.TNIT_TIPONIT    FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.MEMPLEADO MEMPL,dbxschema.mnit MNIT,dbxschema.pbanco PBAN WHERE MQUI_CODIQUIN=" + numQuincena + " AND  MEMPL.PBAN_CODIGO='" + CODBANCO + "'  AND PBAN.PBAN_CODIGO= MEMPL.PBAN_CODIGO  AND MEMPL.MEMP_CODIEMPL=DQUI.MEMP_CODIEMPL  AND MNIT.mnit_nit=MEMPL.mnit_nit  and memp_formpago in ('1','2') and test_estado in ('1','5') " + PagaVacacionesenPlanillaBanco + @" GROUP BY MEMPL.MEMP_CODIEMPL,MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CUENNOMI,MEMP_CODSUCUEMPL,MEMP_FORMPAGO,MNIT.mnit_nombres,MNIT.mnit_nombre2,MNIT.mnit_apellidos,MNIT.mnit_apellido2,PBAN.PBAN_CODACH,MNIT.TNIT_TIPONIT having SUM(DQUI_APAGAR)-SUM(DQUI_ADESCONTAR) > 0";
                        string datoscuerpo = "SELECT  MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CODSUCUEMPL,MEMP_CUENNOMI,MEMP_FORMPAGO,SUM(DQUI_APAGAR)-SUM(DQUI_ADESCONTAR),REPLACE(MNIT.mnit_apellidos,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_apellido2,''),'Ñ','N') concat ' ' concat REPLACE(MNIT.mnit_nombres,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_nombre2,''),'Ñ','N'), PBAN.PBAN_CODACH,MNIT.TNIT_TIPONIT    FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.MEMPLEADO MEMPL,dbxschema.mnit MNIT,dbxschema.pbanco PBAN WHERE MQUI_CODIQUIN=" + numQuincena + " AND  MEMPL.PBAN_CODIGO='" + CODBANCO + "'  AND PBAN.PBAN_CODIGO= MEMPL.PBAN_CODIGO  AND MEMPL.MEMP_CODIEMPL=DQUI.MEMP_CODIEMPL  AND MNIT.mnit_nit=MEMPL.mnit_nit  and memp_formpago in ('1','2')                                                      GROUP BY MEMPL.MEMP_CODIEMPL,MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CUENNOMI,MEMP_CODSUCUEMPL,MEMP_FORMPAGO,MNIT.mnit_nombres,MNIT.mnit_nombre2,MNIT.mnit_apellidos,MNIT.mnit_apellido2,PBAN.PBAN_CODACH,MNIT.TNIT_TIPONIT";
						DBFunctions.Request(DatosCuerpo,IncludeSchema.NO,Sql);			
					}
					else
					{
						int maximo = Int32.Parse(DBFunctions.SingleData("Select max(MPRI_SECUENCIA) from mprimas"));
                        Sql = "SELECT  MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CODSUCUEMPL,MEMP_CUENNOMI,MEMP_FORMPAGO,DPRI.DPRI_VALORPRIMA, REPLACE(MNIT.mnit_apellidos,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_apellido2,''),'Ñ','N') concat ' ' concat REPLACE(MNIT.mnit_nombres,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_nombre2,''),'Ñ','N'), PBAN.PBAN_CODACH,MNIT.TNIT_TIPONIT    FROM DBXSCHEMA.DPRIMAS DPRI,DBXSCHEMA.MEMPLEADO MEMPL,dbxschema.mnit MNIT,dbxschema.pbanco PBAN WHERE DPRI.DPRI_SECUENCIA=" + maximo + " AND  MEMPL.PBAN_CODIGO='" + CODBANCO + "'  AND PBAN.PBAN_CODIGO= MEMPL.PBAN_CODIGO  AND MEMPL.MEMP_CODIEMPL=DPRI.DPRI_CODIEMP AND MNIT.mnit_nit=MEMPL.mnit_nit and memp_formpago in ('1','2') order by DPRI.DPRI_CODIEMP HAVING DPRI.DPRI_VALORPRIMA > 0";
						DBFunctions.Request(DatosCuerpo,IncludeSchema.NO,Sql);			
					}
					
					for (i=0;i<DatosCuerpo.Tables[0].Rows.Count;i++)
					{
						//Tipo registro	numérico	valor fijo “2”	1
						outputDatosBogota+="2";
						//TIPO IDENTIFICACION C:CEDULA CIUDADDANIA,N:NIT,T:TARJ IDENTIDAD,E:CED EXTRANJERIA
						outputDatosBogota+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][8].ToString(),1,"0",false);
						//Nit beneficiario 	numérico 11	*
						outputDatosBogota+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][0].ToString(),11,"0",false);
						//Nombre Beneficiario 40
						outputDatosBogota+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][6].ToString(),40," ",true);
						//VALOR EN CERO
						outputDatosBogota+="0";
						//Tipo cuenta beneficiario	 aho 2 /   cte	1
						outputDatosBogota+=comparacion.Completar_Campos(""+DatosCuerpo.Tables[0].Rows[i][4].ToString().Trim()+"",1,"0",false);
						//Número cuenta beneficiario numérico  17
						string cuenta=this.quitarLineas(DatosCuerpo.Tables[0].Rows[i][3].ToString());
						outputDatosBogota+=comparacion.Completar_Campos(cuenta.Trim(),17," ",true);
						//Valor de transacción numérico $$$$$$$$$$  16	2 ultimas posiciones decimales	*
						sumapagadoemp=double.Parse(DatosCuerpo.Tables[0].Rows[i][5].ToString());
						sumapagadoemp=sumapagadoemp*100;
						outputDatosBogota+=comparacion.Completar_Campos(""+sumapagadoemp+"",18,"0",false);
						//forma de pago fijo A
						outputDatosBogota+="A";
						//espacio en BLANCO 3
						outputDatosBogota+=comparacion.Completar_Campos("",3,"0",false);
						//codigo de compensacion
						outputDatosBogota+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][1].ToString(),3,"0",false);
						//codigo de la ciudad
						outputDatosBogota+=comparacion.Completar_Campos("0001",4,"0",false);
						//SIGLA ORIGINADOR O NOMBRE ABREVIADO
						outputDatosBogota+=comparacion.Completar_Campos("can.qna",9," ",true);
						//espacio en BLANCO 1
						outputDatosBogota+=comparacion.Completar_Campos("",1," ",false);
						//COMENTARIOS
						outputDatosBogota+=comparacion.Completar_Campos("Pago Nomina",70," ",true);
						//VALOR EN CERO
						outputDatosBogota+="0";
						//numero de factura o comprobante
						contador+=1;
						outputDatosBogota+=comparacion.Completar_Campos(contador.ToString(),10,"0",false);
						//indicador fax o correo N: ni por fax ni por correo
						outputDatosBogota+="N";
						//espacio en BLANCO 48
						outputDatosBogota+=comparacion.Completar_Campos("",48," ",false);
						//indicador de envio Mensaje Adicional
						outputDatosBogota+="N";
						//espacio en BLANCO 8
						outputDatosBogota+=comparacion.Completar_Campos("",8," ",false);

                        if (i == DatosCuerpo.Tables[0].Rows.Count - 1)
                            sw.Write(outputDatosBogota);
                        else
                            sw.WriteLine(outputDatosBogota);

						outputDatosBogota="";
					}

                    Session["DataTable1"] = DatosCuerpo;
					sw.Close();	
                    Utils.MostrarAlerta(Response, "Archivo Generado con Exito.");
					hl.NavigateUrl=path+nombreArchivoBogota;
					hl.Visible=true;
                    BtnImprimirExcel.Visible = true;
					hl.Text="Descargar Archivo";
			    }
				else
				{
					sw.Close();	
                    Utils.MostrarAlerta(Response, "No se encuentra el código del Banco de Bogotá, por favor verifique en la configuración de Nónima la cuenta del Banco de Bogotá...");
				}

			}
				else
			{
				sw.Close();		
                Utils.MostrarAlerta(Response, "No se encontro ninguna Quincena con los datos especificados.");
			}
		}



        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                DateTime fecha = DateTime.Now;
                string nombreArchivo = "PlanillaBancoEmpleados" + "_" + fecha.ToShortDateString() + "_" + fecha.ToShortTimeString();
                base.Response.Clear();
                base.Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo + ".xls");
                base.Response.Charset = "Unicode";
                base.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                base.Response.ContentType = "application/vnd.xls";
                StringWriter stringWrite = new StringWriter();
                HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);

                DataGrid dgAux = new DataGrid();
                dgAux.DataSource = (DataSet)Session["DataTable1"];
                dgAux.DataBind();
                dgAux.RenderControl(htmlWrite);

                base.Response.Write(stringWrite.ToString());
                base.Response.End();
            }
            catch (Exception ex)
            {
                //LBPRUEBAS.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }
        }






		protected void EscribirArchivoConavi()
		{
			string numQuincena  = "";
			string Sql = "";
			if (DDLQUIN.SelectedValue.ToString() != "P")
			{
				numQuincena=DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+DDLANO.SelectedValue+" and mqui_mesquin="+DDLMES.SelectedValue+" and mqui_tpernomi="+DDLQUIN.SelectedValue+" ");
			}
			else if (DDLQUIN.SelectedValue.ToString() == "P")
			{
				    numQuincena = "PRIMA";
          	}
			if (numQuincena!=String.Empty)
			{
				int i=0;
				int registros=0;
				double sumapagado=0,sumapagadoemp=0;
				string tipoCuenta,tipoTransaccion;
				//Traer el codigo del banco
			    string CODBANCOLOMBIA = DBFunctions.SingleData("SELECT PBAN_CODIGO FROM DBXSCHEMA.PBANCO pb, DBXSCHEMA.cnomina cn WHERE pB.pban_codigo = CN.CNOM_CODENTBANCARIA;");
				if(CODBANCOLOMBIA!=String.Empty)
				{
					//Traer los datos para el segundo registro
					DataSet DatosCuerpo= new DataSet();
					//pilas esta es la buena ..DBFunctions.Request(DatosCuerpo,IncludeSchema.NO,"SELECT  MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CODSUCUEMPL,MEMP_CUENNOMI,MEMP_FORMPAGO,SUM(DQUI_APAGAR)-SUM(DQUI_ADESCONTAR),MNIT.mnit_nombres concat ' ' concat coalesce(MNIT.mnit_nombre2,' ') concat ' ' concat MNIT.mnit_apellidos concat ' ' concat  coalesce(MNIT.mnit_apellido2,' '),PBAN.PBAN_CODACH  FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.MEMPLEADO MEMPL,dbxschema.mnit MNIT,dbxschema.pbanco PBAN WHERE MQUI_CODIQUIN="+numQuincena+" AND  MEMPL.PBAN_CODIGO IN ('"+CODBANCONAVI+"','"+CODBANCOLOMBIA+"') AND PBAN.PBAN_CODIGO= MEMPL.PBAN_CODIGO  AND MEMPL.MEMP_CODIEMPL=DQUI.MEMP_CODIEMPL  AND MNIT.mnit_nit=MEMPL.mnit_nit and memp_formpago in ('1','2') and test_estado='1' GROUP BY MEMPL.MEMP_CODIEMPL,MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CUENNOMI,MEMP_CODSUCUEMPL,MEMP_FORMPAGO,MNIT.mnit_nombres,MNIT.mnit_nombre2,MNIT.mnit_apellidos,MNIT.mnit_apellido2,PBAN.PBAN_CODACH");			
					
					if (DDLQUIN.SelectedValue.ToString() != "P")
					{
                        Sql = @"SELECT  MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CODSUCUEMPL,MEMP_CUENNOMI,MEMP_FORMPAGO,SUM(DQUI_APAGAR)-SUM(DQUI_ADESCONTAR) as VALOR,  
                              replace(MNIT.mnit_apellidos,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_apellido2,''),'Ñ','N') concat ' ' concat  REPLACE(MNIT.mnit_nombres,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_nombre2,''),'Ñ','N') AS NOMBRE, PBAN.PBAN_CODACH    
                              FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.MEMPLEADO MEMPL,dbxschema.mnit MNIT,dbxschema.pbanco PBAN 
                              WHERE MQUI_CODIQUIN=" + numQuincena + " AND  MEMPL.PBAN_CODIGO IN ('" + CODBANCOLOMBIA + @"') AND PBAN.PBAN_CODIGO= MEMPL.PBAN_CODIGO  
                               AND MEMPL.MEMP_CODIEMPL=DQUI.MEMP_CODIEMPL  AND MNIT.mnit_nit=MEMPL.mnit_nit and memp_formpago in ('1','2') and test_estado in ('1','5') " + PagaVacacionesenPlanillaBanco + @"  
                              GROUP BY MEMPL.MEMP_CODIEMPL,MEMPL.MNIT_NIT,MEMPL.PBAN_CODIGO,MEMP_CUENNOMI,MEMP_CODSUCUEMPL,MEMP_FORMPAGO,MNIT.mnit_nombres,MNIT.mnit_nombre2,MNIT.mnit_apellidos,MNIT.mnit_apellido2,PBAN.PBAN_CODACH
                              ORDER BY MEMPL.MNIT_NIT";
						DBFunctions.Request(DatosCuerpo,IncludeSchema.NO,Sql);			
					}
                    else
					{
                        Utils.MostrarAlerta(Response, "Archivo de Primas.");
						int maximo = Int32.Parse(DBFunctions.SingleData("Select max(MPRI_SECUENCIA) from mprimas"));
                        Sql = "SELECT MEMPL.MNIT_NIT, MEMPL.PBAN_CODIGO, MEMP_CODSUCUEMPL, MEMP_CUENNOMI, MEMP_FORMPAGO, DPRI.DPRI_VALORPRIMA, REPLACE(MNIT.mnit_apellidos,'Ñ','N') concat ' ' concat  REPLACE(coalesce(MNIT.mnit_apellido2,''),'Ñ','N') concat ' ' concat REPLACE(MNIT.mnit_nombres,'Ñ','N') concat ' ' concat REPLACE(coalesce(MNIT.mnit_nombre2,''),'Ñ','N'), PBAN.PBAN_CODACH  FROM DBXSCHEMA.DPRIMAS DPRI,DBXSCHEMA.MEMPLEADO MEMPL,dbxschema.mnit MNIT,dbxschema.pbanco PBAN WHERE DPRI_SECUENCIA=" + maximo + " AND  MEMPL.PBAN_CODIGO IN ('" + CODBANCOLOMBIA + "') AND PBAN.PBAN_CODIGO= MEMPL.PBAN_CODIGO  AND MEMPL.MEMP_CODIEMPL=DPRI.DPRI_CODIEMP AND MNIT.mnit_nit=MEMPL.mnit_nit and memp_formpago in ('1','2') order BY DPRI.DPRI_CODIEMP";
                        DBFunctions.Request(DatosCuerpo, IncludeSchema.NO, Sql);			
					}
					
					for (i=0;i<DatosCuerpo.Tables[0].Rows.Count;i++)
					{
						registros+=1;
						sumapagado+=double.Parse(DatosCuerpo.Tables[0].Rows[i][5].ToString());
					}
					//Traigo todos los datos necesarios para la cabezera.
					DataSet DatosCabecera= new DataSet();
					DBFunctions.Request(DatosCabecera,IncludeSchema.NO,"select cnom_numclientebanco,cnom_codentbancaria,cnom_codsucursal,cnom_numcuentabanco,cnom_formpago from dbxschema.cnomina");
					//Escribir el encabezado
					//Tipo registro	numérico	valor fijo “1”	1
					outputDatosConavi+="1";
					//Nit entidad que envia	numérico		10		*
					string nitentidad=DBFunctions.SingleData("select mnit_nit from DBXSCHEMA.CEMPRESA");
					outputDatosConavi+=comparacion.Completar_Campos(""+nitentidad+"",10,"0",false);
					//Nombre entidad que envia	alfanumérico		16
					string nombreentidad=DBFunctions.SingleData("select cemp_nombre from DBXSCHEMA.CEMPRESA");
					outputDatosConavi+=comparacion.Completar_Campos(""+nombreentidad+"",16," ",true);
					//Clase transacciones contenidas	numérico		3 	225 Pago Nomina
					outputDatosConavi+=comparacion.Completar_Campos("225",3," ",true);
					//Descripcion propósito transacciones	alfanumérico	“a criterio del cliente”	10
					outputDatosConavi+=comparacion.Completar_Campos("Pago Nomina",10," ",true);
					//Fecha transmision de lote	numérico(ENVIO AL BANCO)AAMMDD	6
					outputDatosConavi+=comparacion.Completar_Campos(""+DateTime.Now.ToString("yyMMdd")+"",6,"0",false);
					//Secuencia envio de lotes ese dia	alfanumérico (solo 1 diario siempre A)	“A B C D E..............”	1
					outputDatosConavi+=comparacion.Completar_Campos("A",1,"0",false);
					//Fecha aplicacion transacciones(FECHA DE PAGO)	numérico	AAMMDD	6
					outputDatosConavi+=comparacion.Completar_Campos(""+txtFechaAplicacion.Text+"",6,"0",false);
					//Número registros (detalle y documen.)	numérico	Sumatoria número registros	6		*
					outputDatosConavi+=comparacion.Completar_Campos(""+registros+"",6,"0",false);
					//Sumatoria de creditos	numérico(siempre 0)	$$$$$$$$$$$$	12	*
					outputDatosConavi+=comparacion.Completar_Campos("0",12,"0",false);
					//Sumatoria de débitos	numérico	valor fijo 0(ceros)	12	*
					outputDatosConavi+=comparacion.Completar_Campos(""+sumapagado+"",12,"0",false);
					//Cuenta cliente a debitar	numérico 11	*
					outputDatosConavi+=comparacion.Completar_Campos(DatosCabecera.Tables[0].Rows[0][3].ToString(),11,"0",false);
					//Tipo cuenta cliente a debitar	alfanumérico	S =2: aho  /  D=1 : cte	1
					if (DatosCabecera.Tables[0].Rows[0][4].ToString()=="1")
						tipoCuenta="D";
					else
						tipoCuenta="S";
					outputDatosConavi+=""+tipoCuenta+"";

		
					sw.WriteLine(outputDatosConavi);
					outputDatosConavi="";

                    string refeNomina = DBFunctions.SingleData("select CNOM_CODSUCURSAL from DBXSCHEMA.CNOMINA");

					for (i=0;i<DatosCuerpo.Tables[0].Rows.Count;i++)
					{
						//Tipo registro		numérico valor fijo “6”  	1
						outputDatosConavi+="6";
				
						//Nit beneficiario 	numérico 15	*
						outputDatosConavi+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][0].ToString().Trim(),15,"0",false);
				
						//Nombre Beneficiario 18
						outputDatosConavi+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][6].ToString(),18," ",true);
				
						//Banco cuenta del beneficiario numérico Ver nota # 1   9		*
                        //outputDatosConavi+=comparacion.Completar_Campos(DatosCuerpo.Tables[0].Rows[i][7].ToString(),9,"0",false);
                        outputDatosConavi += comparacion.Completar_Campos(refeNomina, 9, "0", false);
                        

						//Número cuenta beneficiario numérico Ver nota # 2 17	*
						//antes va el codigo de la sucursal
						string sucuenta=DatosCuerpo.Tables[0].Rows[i][2].ToString().Trim()+DatosCuerpo.Tables[0].Rows[i][3].ToString().Trim();
						outputDatosConavi+=comparacion.Completar_Campos(sucuenta,17,"0",false);
				
						//Indicador lugar de pago alfanumérico Ver nota # 3  1	
						//Si el tipo de transacción es “Abono a Cuenta” (27 o 37), el “Indicador Lugar de Pago” debe ir con “S”.
						outputDatosConavi+="S";

						//Tipo transacción numérico
						//27 : Abono a cuenta corriente
						//37 : Abono a cuenta ahorros
						if (DatosCuerpo.Tables[0].Rows[0][4].ToString()=="1")
							tipoTransaccion="27";
						else
							tipoTransaccion="37";
						outputDatosConavi+=""+tipoTransaccion+"";

						//Valor de transacción numérico $$$$$$$$$$  10		*
						sumapagadoemp=double.Parse(DatosCuerpo.Tables[0].Rows[i][5].ToString());
						outputDatosConavi+=comparacion.Completar_Campos(""+sumapagadoemp+"",10,"0",false);
				
						//Concepto alfanumérico		Ver nota # 4 		9	
                        outputDatosConavi += comparacion.Completar_Campos("PAGNOMINA", 9, " ", true);

						//Referencia	alfanumérico Ver nota # 5 12	
                        outputDatosConavi += comparacion.Completar_Campos(txtFechaAplicacion.Text, 12, "0", false);
				
						//Relleno alfanumérico	“espacios en blanco”  1
						outputDatosConavi+=comparacion.Completar_Campos(" ",1,"0",false);
                        //outputDatosConavi=outputDatosConavi.Trim();					
                        if (i == DatosCuerpo.Tables[0].Rows.Count - 1)
                            sw.Write(outputDatosConavi);
                        else
                            sw.WriteLine(outputDatosConavi);

						outputDatosConavi="";
						//outputDatosConavi+="";
					}
					string prueba=sw.NewLine.ToString();
                    Session["DataTable1"] = DatosCuerpo;
                    sw.Close();	
                    Utils.MostrarAlerta(Response, "Archivo Generado con Exito.");
					hl.NavigateUrl=path+nombreArchivo;
					hl.Visible=true;
                    BtnImprimirExcel.Visible = true;
                    hl.Text="Descargar Archivo";
				}
				else
				{
					sw.Close();	
                    Utils.MostrarAlerta(Response, "No se encuentra el codigo del Banco, por favor verifique en la Configuración de Nómina la cuenta del Banco BANCOLOMBIA");
				}
			}
			else
			{
				sw.Close();		
                Utils.MostrarAlerta(Response, "No se encontró ninguna Quincena con los datos especificados.");
			}
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
		///		Método necesario para admitir el Diseñador. No se puede modificar
		///		el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnGenerar.Click += new System.EventHandler(this.GenerarPlanillaBanco);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
