using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using AMS.DB;
using AMS.Forms;
using System.Configuration;
using System.Web.Mail;
using System.Globalization;
using AMS.Documentos;
using AMS.CriptoServiceProvider;
using AMS.Tools;


namespace AMS.Nomina
{
	public class LiquidacionNomina : System.Web.UI.UserControl
	{
		
		protected string table;
		protected Label  lbInfo;
		protected string path=ConfigurationManager.AppSettings["PathToPreviews"];
		
		protected Label  registros,lbperipago,lbsaldorojo,lberroresdetectados,lbtipopago;
        protected Button BTNCONFIRMACION, consulta, btnGenInforme,btnexel;
		protected DataGrid DataGrid1,DataGrid2,DataGrid3;
		protected TextBox TXTIDENTI,TXTCODIGOQUIN,TXTFECHA1,TXTPRUEBA;
		protected DataTable DataTable1 ,DataTable2,DataTable3,DataTable4,DataTableLiqFinal;
        protected DataSet empleados, liquidador, quincena, tsalarionomina, nueva, cnomina2; //cnomina
		protected DropDownList DDLANO,DDLQUIN,DDLMES;
        protected Label  lb,lb2,lb3,lbmas1,lbdocref,lbliquidador,prueba,prueba2,lbpag,lblError;
		protected PlaceHolder phGrilla,toolsHolder;
		protected ImageButton ibMail;
		protected TextBox tbEmail;
        protected DataSet ds = new DataSet();

        protected double pagosquincenaant = 0;
		
		double    acumuladoeps=0,acumuladocesantia=0,acumuladoprima=0,acumuladovacaciones=0,acumuladoretefuente=0, acumuladoretefuenteQnaAct=0;
        double    valorfondoquinanteriorA = 0, valorepsquinanteriorA = 0, valorepsempleado = 0, valorfondopensionvoluntaria = 0;
        double    acumuladoprovisiones = 0, acumuladoliqdefinitiva = 0, acumuladohorasextras = 0, diasexceptosauxtransp = 0, sumaLicenciasSuspPeriodo = 0;
		double    sumapagado=0,sumadescontado=0,sumapagadamasauxt=0,valorfondopensionempleado=0,fondosolidaridadpensional=0, valorPagadoAnt=0;
		int       diasvacaciones,numerodias=0,diasdescdelsueldo=0,diastrabajados=0,diastrabajadosmes=0,diastrabajadosTotalmes=0,idiasexceptosauxtransp,diasvaca,diav;
		int       errores=0, bandera=0,bandera1=0,banderaNombre=0,diassueldopagado=0,valid=0;
		string    empresa, peripago, mensaje = "";
         
        protected bool esSuspension, esIncapacidad, esLicSiRemunerada, esLicNoRemunerada;
		protected System.Web.UI.WebControls.Label titulo;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Label Label5;
		protected System.Web.UI.WebControls.Label Label8;
		protected System.Web.UI.WebControls.RegularExpressionValidator FromValidator2;
		protected System.Web.UI.WebControls.Label lbpag2;
		protected System.Web.UI.WebControls.DataGrid dgprueba;
		string    mainpage=ConfigurationManager.AppSettings["MainIndexPage"];
		protected System.Web.UI.WebControls.HyperLink hl;
		string    ConString;
		ArrayList activarEmp = new ArrayList();
		ArrayList sqlStrings= new ArrayList();

		// arreglos rafael angel
		protected CNomina o_CNomina = new CNomina();
		protected Varios o_Varios = new Varios();
		protected string mest;
		protected string anot;
		protected string diat;
		protected string tipoPeriodoPago;
		protected int mesv;
		protected int anov;
		protected string fecha3;
			 
		protected string RenderHtml()
		{
			StringBuilder SB= new StringBuilder();
			StringWriter SW= new StringWriter(SB);
			HtmlTextWriter htmlTW = new HtmlTextWriter(SW);
			phGrilla.RenderControl(htmlTW);
			return SB.ToString();
		}
		
		public void SendMail(Object Sender, ImageClickEventArgs E)
		{
            Utils.EnviarMail(tbEmail.Text,
                "Proceso : " + DBFunctions.SingleData("SELECT remarks FROM SYSIBM.SYSTABLES WHERE name='" + table + "'"), 
                RenderHtml(), 
                TipoCorreo.HTML, null);
		}

		protected void realizar_consulta2_new()
		{
			int i;
			o_Varios.AsignarParametrosNomina = o_CNomina;
		
			empleados   = new DataSet();
			liquidador  = new DataSet();
			quincena    = new DataSet();
			tsalarionomina= new DataSet();
			nueva       = new DataSet();
			lbdocref.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+o_CNomina.CNOM_QUINCENA;
		
			if (DDLQUIN.SelectedValue==o_CNomina.CNOM_QUINCENA && DDLMES.SelectedValue==o_CNomina.CNOM_MES && DDLANO.SelectedValue==o_CNomina.CNOM_ANO)
			{
                if (DBFunctions.RecordExist("SELECT MQUI_CODIQUIN FROM MQUINCENAS WHERE MQUI_ANOQUIN = " + DDLANO.SelectedValue + " AND MQUI_MESQUIN = " + DDLMES.SelectedValue + " AND MQUI_TPERNOMI = " + DDLQUIN.SelectedValue + " AND MQUI_ESTADO = 2;"))
                {
                    Utils.MostrarAlerta(Response, "Este período se regitra YA liquidado en el control de períodos y NO coincide con la configuración de la Nómina");
                    return;
                }
                else
                    Utils.MostrarAlerta(Response, "se selecciono período QUINCENA CORRECTO!!! ,MES CORRECTO!!!,ANO CORRECTO!!!");
					
				string Where = "";

				if (lbtipopago.Text=="QUINCENAL")
				{
		//			if (o_CNomina.CNOM_PAGOMENSTPER=="1")
                    if (DDLQUIN.SelectedValue == "1")
					{
						Where = "<> 'aa' and (a.test_estado='1' or a.test_estado='5') AND (a.memp_peripago=1 or  a.memp_peripago=2 or a.memp_peripago=3) ";
					}
					else
					{
						Where = "<> 'aa' and (a.test_estado='1' or a.test_estado='5') AND (a.memp_peripago=1 or a.memp_peripago=3) ";
					}
				}
				else
				{
		//			if (o_CNomina.CNOM_PAGOMENSTPER=="2")
                    if (DDLQUIN.SelectedValue == "2")
					{
						Where = "<> 'aa' and (a.test_estado='1' or a.test_estado='5') AND (a.memp_peripago=1 or  a.memp_peripago=2 or a.memp_peripago=3) ";
					}
					else
					{
						Where = "<> 'aa' and (a.test_estado='1' or a.test_estado='5') AND (a.memp_peripago=2 or  a.memp_peripago=3)";
					}
				}

				this.preparargrilla_empleadospago();
				this.preparargrilla_LiqFinal();
				this.preparargrilla_empleadosensaldorojo();

				Empleado o_Empleados = new Empleado(Where);
				o_Empleados.NominaQueProcesa = lbtipopago.Text;
				o_Empleados.PeridoDeProceso = Int32.Parse(DDLQUIN.SelectedValue.ToString());
				o_Empleados.p_NominaMensual = true; // hace referencia al proceso de nomina quincenal o mensual
				

				DateTime diaActual = new DateTime();
				mest = o_CNomina.TMES_NOMBRE;
				anot = o_CNomina.PANO_DETALLE;
				diat = diaActual.Day.ToString();
				mesv = Int32.Parse(o_CNomina.CNOM_MES);
				anov = Int32.Parse(o_CNomina.CNOM_ANO);
				diav = diaActual.Day;

				o_Empleados.p_DDLANORETI = DDLANO;
				o_Empleados.p_DDLMESRETI = DDLMES;
				
				o_Empleados.armarfechas();

				o_Empleados.AsignarParametrosNomina = o_CNomina;
				o_Empleados.AsignarVarios = o_Varios;
				o_Empleados.AsignarDataGrid = this.DataGrid2;
				o_Empleados.AsignarDataTable = DataTable1;

				if (o_Empleados.p_TotalFilas == 0)
				{
                    Utils.MostrarAlerta(Response, "No hay empleados para liquidar..");
				}
				else
				{
					int  DiasAplicar = 0;
					string liquidaNomina = "Normal";
					bool subsidioTransporte = true;
					if((Convert.ToDouble(o_Empleados.p_MEMP_SUELACTU))>=((Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU))*2))
						subsidioTransporte = false;
				
					for (i=0;i< o_Empleados.p_TotalFilas;i++)
					{ 
						if (lbtipopago.Text=="QUINCENAL")
						{
							DiasAplicar = 15;
						}
						else
						{
							DiasAplicar = 30;
						}
						o_Empleados.LiquidarMesEnCurso(mesv,anov,DiasAplicar,subsidioTransporte,liquidaNomina);
						o_Empleados.AsignarFila = i;
					}
				
					Session["DataTable1"]=DataTable1;//data table secundario
					Session["DataTableLiqFinal"]=DataTableLiqFinal;
					Session["DataTable2"]=DataTable2;//data table principal 
					Session["errores"]=errores; //errores
					Session["lb2"]=lb2.Text;
					BTNCONFIRMACION.Visible= true;
				}
			}
		}

        protected void ImprimirExcelGrid(Object Sender, EventArgs e)
        {
            try
            {
                DataTable session = (DataTable)Session["DataTable1"];
                DataSet datos = new DataSet ();
                datos.Tables.Add(session);
                ds = datos;
                Utils.ImprimeExcel(ds, "ConsultaTabla");
            }
            catch (Exception ex)
            {
                lblError.Text = "Couldn't create Excel file.\r\nException: " + ex.Message;
                return;
            }
        }


        protected void realizar_consulta2()
		{
			int i;
			double saldorojo;
            mensaje = "";
		
			empleados   = new DataSet();
			liquidador  = new DataSet();
			quincena    = new DataSet();
			tsalarionomina= new DataSet();
			//cnomina= new DataSet();
			nueva       = new DataSet();
			sqlStrings.Clear();
			fecha3      = string.Empty;
			//DBFunctions.NonQuery("delete from dpagaafeceps");
			//Traigo el ano,mes,y quincena vigente de CNOMINA.
			//DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4) as transactu2,cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");	
			//DBFunctions.NonQuery("insert into mquincenas values (default,"+cnomina.Tables[0].Rows[0][0]+","+cnomina.Tables[0].Rows[0][1]+","+cnomina.Tables[0].Rows[0][2]+","+cnomina.Tables[0].Rows[0][2]+")");
            lbdocref.Text = o_CNomina.CNOM_ANO + "-" + o_CNomina.CNOM_MES + "-" + o_CNomina.CNOM_QUINCENA;
		
			//Valido los datos escogidos por el usuario contra los de CNOMINA. 
            if (DDLQUIN.SelectedValue == o_CNomina.CNOM_QUINCENA && DDLMES.SelectedValue == o_CNomina.CNOM_MES && DDLANO.SelectedValue == o_CNomina.CNOM_ANO)
            {
                if (DBFunctions.RecordExist("SELECT MQUI_CODIQUIN FROM MQUINCENAS WHERE MQUI_ANOQUIN = " + DDLANO.SelectedValue + " AND MQUI_MESQUIN = " + DDLMES.SelectedValue + " AND MQUI_TPERNOMI = " + DDLQUIN.SelectedValue + " AND MQUI_ESTADO = 2;"))
                {
                    Utils.MostrarAlerta(Response, "Este período se regitra YA liquidado en el control de períodos y NO coincide con la configuración de la Nómina");
                    return;
                }
                else 
                    mensaje += "se seleccionó período QUINCENA CORRECTO!!!, MES CORRECTO!!!, ANO CORRECTO!!!  \\n";
			
				//DataSet mquincenas0= new DataSet();
				//DBFunctions.Request(mquincenas0,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
				int month = Int32.Parse(o_CNomina.CNOM_QUINCENA)+1;
				/*if (mquincenas0.Tables[0].Rows[0][0].ToString()==" ")
				{
					Response.Write("<script language:javascript>alert(' voy a ingresar en mquincenas caso inicio de NOMINA. ');/<script>");
					DBFunctions.NonQuery("insert into mquincenas values (default,"+cnomina.Tables[0].Rows[0][0]+","+cnomina.Tables[0].Rows[0][1]+","+month+","+1+")");
				}*/
				//si se reliquida y se vuelve a la ultima.
                DBFunctions.Request(nueva, IncludeSchema.NO, "select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin=" + o_CNomina.CNOM_ANO + " and mqui_mesquin=" + o_CNomina.CNOM_MES + " and mqui_tpernomi=" + o_CNomina.CNOM_QUINCENA + " ");
				
				int mes         = Int32.Parse(DDLMES.SelectedValue); 
				int anio        = Int32.Parse(DDLANO.SelectedValue);
				int quince      = Int32.Parse(DDLQUIN.SelectedValue);
				int diafinqui   = 0;
				int diainiqui   = 01;
				if (lbtipopago.Text=="QUINCENAL" && quince == 1)
					diafinqui   = 15;
				else
				{
					diafinqui = 30;
					if (lbtipopago.Text=="QUINCENAL" && quince == 2)  
						diainiqui = 16;
				}
				
				if(o_CNomina.CNOM_OPCIQUINOMENS=="1")
				{
					quince++;
					if(quince>2)
					{
						mes++;
						quince=1;
					}
				}
				else
				{
					mes++;
				}
				if(mes>12)
				{
					anio++;
					mes=1;
				}
				nueva=new DataSet();
				DBFunctions.Request(nueva,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+anio+" and mqui_mesquin="+mes+" and mqui_tpernomi="+quince+" ");
				if (nueva.Tables[0].Rows.Count==0)
				{
					//JFSC 25042008 debe estar cuando se liquida definitivamente
					//sqlStrings.Add("insert into mquincenas values (default,"+anio+","+mes+","+quince+",1)");
					//DBFunctions.NonQuery("insert into mquincenas values (default,"+anio+","+mes+","+quince+",1)");
				}		
	
  //  hector    restauro valores de la quincena actual - ver para que los incrementa aqui en una quincena ?
				mes    = Int32.Parse(DDLMES.SelectedValue); 
				anio   = Int32.Parse(DDLANO.SelectedValue);
				quince = Int32.Parse(DDLQUIN.SelectedValue);

                //borrar la tabla de dquincena
                //DBFunctions.NonQuery("DELETE from dquincena where mqui_codiquin="+mquincenas0.Tables[0].Rows[0][0]+"");
                //mquincenas0.Clear();
                //****** Si el usuario escogio procesar TIPO PAGO QUINCENAL********
                //Se incluiran los empleados VIGENTES pago QUINCENAL Y MENSUAL SI AFECTA.
                 
                ViewState["descConceptoSalarioBasico"]   = DBFunctions.SingleData("SELECT PCON_nombconc FROM pconceptonomina where PCON_CONCEPTO = '" + o_CNomina.CNOM_CONCSALACODI+ "' ").ToString();
                ViewState["descConceptoSalarioIntegral"] = DBFunctions.SingleData("SELECT PCON_nombconc FROM pconceptonomina where PCON_CONCEPTO = '" + o_CNomina.CNOM_CONCSALAINT + "' ").ToString();
                ViewState["descConceptoSalarioSena"]     = DBFunctions.SingleData("SELECT PCON_nombconc FROM pconceptonomina where PCON_CONCEPTO = '" + o_CNomina.CNOM_CONCSALASENA + "' ").ToString();
                ViewState["nombconcFondoSolidaridad"]    = DBFunctions.SingleData("SELECT PCON_nombconc FROM pconceptonomina where PCON_CONCEPTO = '" + o_CNomina.CNOM_CONCFONDSOLIPENS + "' ").ToString();
              
                string pagoQunicenal = " ORDER BY 1 ";
                string pagoMensual = " AND (M.memp_peripago in (2,4) ) ORDER BY 1 ";

                string sqlEmpleados =
                   @"Select m.MEMP_CODIEMPL, 
                            n.mnit_nombres,  
                            n.mnit_apellidos,   
                            COALESCE(n.mnit_apellido2,'') as mnit_apellido2, 
                            m.MEMP_SUELACTU, 
                            m.memp_suelactu, 
                            m.memp_fechingreso, 
                            CASE WHEN MEMP_CODIEMPL = M.MNIT_NIT THEN COALESCE(n.mnit_nombre2,'') ELSE COALESCE(n.mnit_nombre2,'') || ' - ' ||m.mnit_nit end as mnit_nombre2,  
                            m.tsub_codigo, 
                            m.tsal_salario,  
                            m.memp_peripago, 
                            M.memp_fechfincontrato, 
                            M.tcon_contrato, 
                            M.memp_peripago, 
                            m.test_estado,
                            pfon_nombpens as NOMBFONDPENS,
                            PEPS_NOMBEPS AS NOMBEPS  
                    from  dbxschema.mnit n, DBXSCHEMA.mempleado m   
                          left join dbxschema.pfondopension pfon on pfon.PFON_CODIPENS=m.PFON_CODIPENS
                          left join dbxschema.peps peps on peps.peps_CODIeps = m.peps_CODIeps
                 where m.mnit_nit=n.mnit_nit and m.test_estado in ('1','5') AND MEMP_CODIEMPL in ('51719039')
  
                    ";
                  
							
				if (lbtipopago.Text=="QUINCENAL")
				{
                    //Traigo todos los empleados independiente de cualquier cosa.
                    sqlEmpleados += pagoQunicenal;
               
                    DBFunctions.Request(empleados, IncludeSchema.NO, sqlEmpleados);   
		
				 	//Traigo los tipos de salario: no integral,integral,variable
					DBFunctions.Request(tsalarionomina,IncludeSchema.NO,"select * from dbxschema.tsalarionomina;");

                    //Configuro la fecha inicial y la final.
                    if (Convert.ToInt32(o_CNomina.CNOM_MES) >= 1 && Convert.ToInt32(o_CNomina.CNOM_MES) <= 9)
                    {
                        if (Convert.ToInt32(o_CNomina.CNOM_MES) == 2)
                        {
                            lb.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "01";
                            int diasMes = Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()) / 4;
                            diasMes = diasMes * 4;
                            if (diasMes == Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()))
                            {
                                lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                                lbmas1.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                            }
                            else
                            {
                                lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                                lbmas1.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                            }
                        }
                        else
                        {
                            lb.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "01";
                            lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "30";
                        }
                        fecha3 = lb2.Text;
                    }
                    else
                    {
                        lb.Text = o_CNomina.CNOM_ANO + "-" + o_CNomina.CNOM_MES + "-" + "01";
                        lb2.Text = o_CNomina.CNOM_ANO + "-" + o_CNomina.CNOM_MES + "-" + "30";
                        fecha3 = lb2.Text;
                    }
				}

				//mensual
				if (lbtipopago.Text=="MENSUAL")
				{

                    if (o_CNomina.CNOM_OPCIQUINOMENS == "2" && DDLQUIN.SelectedValue == "2")
					{
                        mensaje += "Se seleccionó mensual \\n";
                        sqlEmpleados += pagoMensual;

                        DBFunctions.Request(empleados, IncludeSchema.NO, sqlEmpleados);
                     	DBFunctions.Request(tsalarionomina,IncludeSchema.NO,"select * from dbxschema.tsalarionomina;");
						DataSet nomensuales= new DataSet();
						DBFunctions.Request(nomensuales,IncludeSchema.NO,"Select E.MEMP_CODIEMPL,N.MNIT_NOMBRES,N.MNIT_APELLIDOS, N.MNIT_APELLIDOS ,N.MNIT_NOMBRES,T.tper_descp from DBXSCHEMA.mempleado E,DBXSCHEMA.CNOMINA C,dbxschema.tperipago T, dbxschema.mnit N  where E.mnit_nit=N.mnit_nit  and E.test_estado='1' AND E.memp_peripago=1   and E.memp_peripago=T.tper_peri ORDER BY E.memp_codiempl");
							
						if(Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
						{
							if(Convert.ToInt32(o_CNomina.CNOM_MES)==2)
							{
								lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"01";
								lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"28";
								lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"29";

                                int diasMes = Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()) / 4;
                                diasMes = diasMes * 4;
                                if (diasMes == Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()))
                                {
                                    lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                                    lbmas1.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                                }
                                else
                                {
                                    lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                                    lbmas1.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                                }
     						}
							else
							{
								lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"01";
								lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
								
							}
							fecha3=lb2.Text;
						}
						else
						{
							lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"01";
							lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"30";
							fecha3=lb2.Text;
						}		
						if (nomensuales.Tables[0].Rows.Count>0)
						{
							errores=0;
                            mensaje += "Se encontró empleados a los cuales se les paga quincenal o jornal, ud esta procesando liquidacion mensual estos empleados no seran tenidos en cuenta, por favor Revise el listado posterior al de la liquidación.. \\n";
							this.preparargrilla_empleadosdiferenteperipago();
							for(i=0;i<nomensuales.Tables[0].Rows.Count;i++)
							{
								this.ingresar_datos_empleadosdiferenteperipago(nomensuales.Tables[0].Rows[i][0].ToString(),nomensuales.Tables[0].Rows[i][2].ToString(),nomensuales.Tables[0].Rows[i][3].ToString(),nomensuales.Tables[0].Rows[i][1].ToString(),nomensuales.Tables[0].Rows[i][4].ToString(),nomensuales.Tables[0].Rows[i][5].ToString());
								errores+=1;
							}
							//DataGrid1.DataSource=nomensuales.Tables[0];
							//DataGrid1.DataBind();
							//Session["dos"]=nomensuales.Tables[0];
							lbperipago.Visible=true;
						}
					}
				}
		
				DataSet mquincenas= new DataSet();
				//Traigo el ultimo registro del maestro de Quincenas
				//DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			    DBFunctions.Request(mquincenas,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+o_CNomina.CNOM_ANO+" and mqui_mesquin="+o_CNomina.CNOM_MES+" and mqui_tpernomi="+o_CNomina.CNOM_QUINCENA+" ");
				if (mquincenas.Tables.Count==0)
				{
                    mensaje += "Error tabla Período Quincena para liquidar : " + o_CNomina.CNOM_ANO + " \\n";
					return;
				}
				else 
					if (mquincenas.Tables[0].Rows.Count==0)
					{
                        Utils.MostrarAlerta(Response, "No hay quincena Quincena para liquidar : " + o_CNomina.CNOM_ANO + "");
						return;
					}
		 		//Guardar Fechas Originales
				string FechaOrigI=lb.Text;
				string FechaOrigF= lb2.Text;
				//funcion para llenar columnas en grilla
				this.preparargrilla_empleadospago();
				this.preparargrilla_LiqFinal();
				this.preparargrilla_empleadosensaldorojo();
				//Recorro cada empleado vigente
				//errores=0;
				if (empleados.Tables.Count==0)
				{
                    Utils.MostrarAlerta(Response, "No hay empleados para liquidar..");
				}
				else
				{
					//Traer campo de cnomina para validar combinacion.
                    string combinacion= o_CNomina.CNOM_LIQCOMBINADA;

                    combinacion = "N";  // LIQUIDACION COMBINADA...SIEMPRE DEBEN VENIR en NO
					
					for(i=0;i<empleados.Tables[0].Rows.Count;i++)
					{
                        // descarto los empleados que estan en vacaciones 
						int entra=1;
                        tipoPeriodoPago = empleados.Tables[0].Rows[i][10].ToString();
                    
                        // descarto los empleados que estan en vacaciones //
                        if (empleados.Tables[0].Rows[i][14].ToString().Trim() == "5")
                        {
                            DataSet fechafinvacaciones = new DataSet();
                            DBFunctions.Request(fechafinvacaciones, IncludeSchema.NO, "select COALESCE(max(dvac_fechfinal),'2050-01-01') from dbxschema.dvacaciones DVAC,dbxschema.mvacaciones MVAC where DVAC.mvac_secuencia=MVAC.mvac_secuencia and MVAC.memp_codiemp='" + empleados.Tables[0].Rows[i][0].ToString().Trim() + "' and dvac.dvac_tiem > 0");
                            if (fechafinvacaciones.Tables[0].Rows[0][0] == null || fechafinvacaciones.Tables[0].Rows[0][0].ToString() == "2050-01-01")
                            {
                                fechafinvacaciones.Tables[0].Rows[0][0] = "2050-01-01";
                                mensaje += "El empleado  " + empleados.Tables[0].Rows[i][0].ToString() + " Nombre " + empleados.Tables[0].Rows[i][1].ToString() + "  " + empleados.Tables[0].Rows[i][2].ToString() + "  " + empleados.Tables[0].Rows[i][3].ToString() + " está en vacaciones y NO ESTA LIQUIDADA EN EL SISTEMA se define 2050-01-01 \\n";
                            }
                            DateTime FechaVacaF = Convert.ToDateTime(fechafinvacaciones.Tables[0].Rows[0][0]);
                            if (mes == 2 && diafinqui == 30)
                            {
                                int diasMes = Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()) / 4;
                                diasMes = diasMes * 4;
                                if (diasMes == Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()))
                                    diafinqui = 29;
                                else
                                    diafinqui = 28;
                            }
                     
                            int mesp = Convert.ToInt16(DDLMES.SelectedValue);
                            DateTime FechaFinLiq = new DateTime(anio, mesp, diafinqui);
                            DateTime FechaIniLiq = new DateTime(anio, mesp, diainiqui);

                            if (FechaVacaF >= FechaFinLiq)
                            {
                                entra = 0;
                               mensaje += "El empleado  " + empleados.Tables[0].Rows[i][0].ToString() + " Nombre " + empleados.Tables[0].Rows[i][1].ToString() + "  " + empleados.Tables[0].Rows[i][2].ToString() + "  " + empleados.Tables[0].Rows[i][3].ToString() + " está en vacaciones hasta " + FechaVacaF + " \\n";
                            }
                        }


                        string sueldo = empleados.Tables[0].Rows[i][4].ToString();
                        lb.Text = FechaOrigI.ToString();
                        lb2.Text = FechaOrigF.ToString();
                        //fecha3=lb2.Text;
                        //reingreso=0;
                        diasexceptosauxtransp = 0;
                        idiasexceptosauxtransp = 0;
                        numerodias = 0;
                        banderaNombre = 0;
                        sumapagado = 0;
                        sumadescontado = 0;
                        DataSet fechavacaciones = new DataSet();
                        //Si es quincenal,armo fechas
                        //if (cnomina.Tables[0].Rows[0][11].ToString()=="1")
                        //{
                        //PERIODO DE PAGO
                        peripago = empleados.Tables[0].Rows[i][10].ToString();
                        this.fechaliquidacion(empleados.Tables[0].Rows[i][10].ToString(), empleados.Tables[0].Rows[i][13].ToString());
                        //}

                        if (entra == 1)
                        {
                            //Quincenal=1, Mensual 2da Qna=2, Mensual 1ra Qna=4
                            if (empleados.Tables[0].Rows[i][10].ToString() == "1")
                            {
                                valid = 15;
                            }
                            else if (this.DDLQUIN.SelectedValue == "1")
                            {
                                if (empleados.Tables[0].Rows[i][10].ToString() == "2")
                                {
                                    valid = 0;
                                }
                                else
                                {
                                    valid = 30;
                                }
                            }
                            else
                            {
                                if (empleados.Tables[0].Rows[i][10].ToString() == "2")
                                {
                                    valid = 30;
                                }
                                else
                                {
                                    valid = 0;
                                }
                            }

                            //	if (diasexceptosauxtransp!=valid)
                            //	{
                            //Reinicio de todas las variables globales.
                            diasdescdelsueldo = 0;
                            fondosolidaridadpensional = 0;
                            valorfondopensionvoluntaria = 0;
                            valorfondopensionempleado = 0;
                            acumuladohorasextras = 0;
                            acumuladoliqdefinitiva = 0;
                            acumuladovacaciones = 0;
                            acumuladoprima = 0;
                            acumuladocesantia = 0;
                            acumuladoprovisiones = 0;
                            diasvacaciones = 0;
                            acumuladoeps = 0;
                            acumuladoretefuente = 0;
                            acumuladoretefuenteQnaAct = 0;
                            pagosquincenaant = 0;
                            this.diastrabajados = 0;
                            this.diastrabajadosmes = 0;
                            this.diastrabajadosTotalmes = 0;
                            //sumapagado=0;
                            //sumadescontado=0;
                            saldorojo = 0;
                            sumapagadamasauxt = 0;

                            if (combinacion == "S")
                            {
                                if (tipoPeriodoPago == "4" && DDLQUIN.SelectedValue == "1" || tipoPeriodoPago == "2" && DDLQUIN.SelectedValue == "2")
                                {
                                    //lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"15";
                                    //cambiar lb2 a año-mes-15
                                    this.actualiza_novedades(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][10].ToString().Trim());
                                    //aqui estaba msusplicencias..
                                    this.actuliza_registro_mpagosydtosper(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    sumapagadamasauxt = sumapagado;
                                    this.insertarsaldos(empleados.Tables[0].Rows[i][0].ToString(), sumapagado, sumadescontado, acumuladoeps, mquincenas.Tables[0].Rows[0][0].ToString(), diasexceptosauxtransp, sumapagadamasauxt, empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString());
                                    //dgprueba.DataSource = DataTableLiqFinal;
                                    //dgprueba.DataBind();
                                    //DatasToControls.Aplicar_Formato_Grilla(dgprueba);

                                }
                                else
                                {
                                 //   lbtipopago.Text = "MENSUAL";

                                    this.actualiza_registro_msusplicencias(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString());
                                    this.validardiasdepago(i, empleados.Tables[0].Rows[i][6].ToString(), lb.Text, lb2.Text, empleados.Tables[0].Rows[i][0].ToString(), o_CNomina.CNOM_CONCSALACODI, empleados.Tables[0].Rows[i][4].ToString(), lbdocref.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][10].ToString(), mquincenas.Tables[0].Rows[0][0].ToString(), Int32.Parse(empleados.Tables[0].Rows[i]["test_estado"].ToString()));
                                    this.validarfincontrato(i, empleados.Tables[0].Rows[i][0].ToString(), empleados.Tables[0].Rows[i][11].ToString(), lb2.Text, empleados.Tables[0].Rows[i][12].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][2].ToString());
                                    this.actualiza_novedades(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][10].ToString());
                                    this.actuliza_registro_mpagosydtosper(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    this.actuliza_prestamoempledos(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    this.liquidar_subsidiodetransporte(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, diasexceptosauxtransp.ToString(), empleados.Tables[0].Rows[i][8].ToString(), empleados.Tables[0].Rows[i][9].ToString());
                                    this.ajuste_minimo(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, diasexceptosauxtransp.ToString(), empleados.Tables[0].Rows[i][8].ToString(), empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_epsfondo(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_fondosolidaridadpensional(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString(), double.Parse(sueldo));
                                    this.liquidar_apropiaciones(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString());
                                    this.insertarretefuente(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), acumuladoeps, o_CNomina.CNOM_EPSPERINOMI);// empleados.Tables[0].Rows[i]["cnom_epsperinomi"].ToString()
                                    this.liquidar_provisiones(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), acumuladoprovisiones, empleados.Tables[0].Rows[i][9].ToString());
                                    this.insertarsaldos(empleados.Tables[0].Rows[i][0].ToString(), sumapagado, sumadescontado, acumuladoeps, mquincenas.Tables[0].Rows[0][0].ToString(), diasexceptosauxtransp, sumapagadamasauxt, empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString());

                                 //    lbtipopago.Text = "QUINCENAL";
                                }
                            }                 //termina Combinada
                            else
                            {
                                bool pasar = false;
                                {
                                    if (lbtipopago.Text == "QUINCENAL")
                                    {
                                        if (empleados.Tables[0].Rows[i][10].ToString() == "1" || (empleados.Tables[0].Rows[i][10].ToString() == "2" && this.DDLQUIN.SelectedValue == "2") || (empleados.Tables[0].Rows[i][10].ToString() == "4" && this.DDLQUIN.SelectedValue == "1"))
                                        {
                                            pasar = true;
                                        }
                                    }
                                  //  else
                                    {
                                        if (empleados.Tables[0].Rows[i][10].ToString() == "2" )  // PAGO MENSUAL  
                                            pasar = true;
                                    }
                                }
                                if (empleados.Tables[0].Rows[i][10].ToString() == "3")  //  JORNAL = CERO DIAS
                                    pasar = true;
                                if (peripago == "4")  //  mensual pago en la primera qna
                                    pasar = true;
                                if (pasar)
                                {
                                    this.actualiza_registro_msusplicencias(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString());
                                    this.validardiasdepago(i, empleados.Tables[0].Rows[i][6].ToString(), lb.Text, lb2.Text, empleados.Tables[0].Rows[i][0].ToString(), o_CNomina.CNOM_CONCSALACODI, empleados.Tables[0].Rows[i][4].ToString(), lbdocref.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][10].ToString(), mquincenas.Tables[0].Rows[0][0].ToString(), Int32.Parse(empleados.Tables[0].Rows[i]["test_estado"].ToString()));
                                    this.validarfincontrato(i, empleados.Tables[0].Rows[i][0].ToString(), empleados.Tables[0].Rows[i][11].ToString(), lb2.Text, empleados.Tables[0].Rows[i][12].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][2].ToString());
                                    this.actualiza_novedades(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][10].ToString());
                                    this.actuliza_registro_mpagosydtosper(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    this.actuliza_prestamoempledos(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    this.liquidar_subsidiodetransporte(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, diasexceptosauxtransp.ToString(), empleados.Tables[0].Rows[i][8].ToString(), empleados.Tables[0].Rows[i][9].ToString());
                                    this.ajuste_minimo(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, diasexceptosauxtransp.ToString(), empleados.Tables[0].Rows[i][8].ToString(), empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_epsfondo(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_fondosolidaridadpensional(i, empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString(), double.Parse(sueldo));
                                    this.liquidar_apropiaciones(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString());
                                    this.insertarretefuente(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), acumuladoeps, empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_provisiones(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), acumuladoprovisiones, empleados.Tables[0].Rows[i][9].ToString());
                                    this.insertarsaldos(empleados.Tables[0].Rows[i][0].ToString(), sumapagado, sumadescontado, acumuladoeps, mquincenas.Tables[0].Rows[0][0].ToString(), diasexceptosauxtransp, sumapagadamasauxt, empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString());


                                    if (sumapagadamasauxt < sumadescontado)
                                    {
                                        saldorojo = sumapagadamasauxt - sumadescontado;
                                        mensaje += "Al empleado " + empleados.Tables[0].Rows[i][0].ToString() + " Nombre " + empleados.Tables[0].Rows[i][1].ToString() + "  " + empleados.Tables[0].Rows[i][2].ToString() + "  " + empleados.Tables[0].Rows[i][3].ToString() + " se le está descontando más de lo que devengo, por favor corrija.. \\n";
                                        this.ingresar_datos_empleadosensaldorojo(empleados.Tables[0].Rows[i][0].ToString(), empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), Math.Round(sumapagado, 0), Math.Round(sumadescontado, 0), Math.Round(saldorojo, 0));
                                        lbsaldorojo.Visible = true;
                                        errores += 1;

                                    }
                                }
                                else
                                {
                                    /*  no debe entrar aqui 
                                    this.actualiza_novedades(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), empleados.Tables[0].Rows[i][10].ToString());
                                    this.actuliza_registro_mpagosydtosper(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    this.actuliza_prestamoempledos(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI);
                                    this.liquidar_subsidiodetransporte(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, diasexceptosauxtransp.ToString(), empleados.Tables[0].Rows[i][8].ToString(), empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_epsfondo(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_fondosolidaridadpensional(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString(), double.Parse(sueldo));
                                    this.liquidar_apropiaciones(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), o_CNomina.CNOM_EPSPERINOMI, empleados.Tables[0].Rows[i][9].ToString());
                                    this.insertarretefuente(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), acumuladoeps, empleados.Tables[0].Rows[i][9].ToString());
                                    this.liquidar_provisiones(empleados.Tables[0].Rows[i][0].ToString(), lb.Text, lb2.Text, mquincenas.Tables[0].Rows[0][0].ToString(), Convert.ToDouble(empleados.Tables[0].Rows[i][5]), lbdocref.Text, lbmas1.Text, empleados.Tables[0].Rows[i][2].ToString(), empleados.Tables[0].Rows[i][3].ToString(), empleados.Tables[0].Rows[i][1].ToString(), empleados.Tables[0].Rows[i][7].ToString(), acumuladoprovisiones, empleados.Tables[0].Rows[i][9].ToString());
                                //    sumapagadamasauxt += sumapagado;
                                    this.insertarsaldos(empleados.Tables[0].Rows[i][0].ToString(), sumapagado, sumadescontado, acumuladoeps, mquincenas.Tables[0].Rows[0][0].ToString(), diasexceptosauxtransp, sumapagadamasauxt);
                                */
                                }
                            }
                            consulta.Visible = false;
                            btnGenInforme.Visible = true;
                           //btnexel.Visible = true;
                           
                            	//}
                        }
					
					}//acaba el for
		
				}
				//inserto el total de la liquidacion de nomina.
				double sumatotalapagar=0,sumatotaladescontar=0,saldototal=0;
				for (i=0;i<DataTable1.Rows.Count;i++)
				{
					
					if (DataTable1.Rows[i][3].ToString()=="--")
					{
						string tem          = DataTable1.Rows[i][7].ToString();
						sumatotalapagar     +=Convert.ToDouble(tem);
						sumatotaladescontar +=Convert.ToDouble(DataTable1.Rows[i][8].ToString());
						saldototal          =sumatotalapagar - sumatotaladescontar;
					}
				}
				this.ingresar_datos_datatable("--","--","--",0,0,sumatotalapagar,sumatotaladescontar,"SALDO",saldototal,"--","--","--","--","--","TOTAL EMPRESA");
		
				if (errores>=0)
				{
					lberroresdetectados.Text="";
					lberroresdetectados.Text+="Errores Detectados  "+errores.ToString();
					lberroresdetectados.Visible=true;
				}
			
				Session["DataTable1"]       =DataTable1;//data table secundario
				Session["DataTableLiqFinal"]=DataTableLiqFinal;
				Session["DataTable2"]       =DataTable2;//data table principal 
				Session["errores"]          =errores; //errores
				Session["lb2"]              =lb2.Text;
				BTNCONFIRMACION.Visible     =true;
		
				//procesar liquidacion mensual********
			
				if (lbtipopago.Text=="JORNAL")
				{
                    mensaje += "se seleccionó jornal bajo construcción \\n \\n";
				}
			}
			else
                mensaje += "Esta tratando de liquidar otra quincena \\n \\n";

			if (DBFunctions.Transaction(sqlStrings) == false)
			{
                mensaje += "hubo errores en la liquidación preliminar, por favor revise los Logs del sistema. \\n \\n ";
			}
			else
			{
                mensaje += "El periodo de liquidación preliminar se realizó satisfactoriamente. \\n \\n";
			}
		}
		
		
		protected void realizar_consulta(Object  Sender, EventArgs e)
		{
			this.realizar_consulta2();
            if (mensaje.Length > 1)
                Utils.MostrarAlerta(Response, mensaje);
            Session["DataTable1"] = DataTable1;
			Session["DataTableLiqFinal"] = DataTableLiqFinal;
		}
		
		protected void generarInforme(object sender , EventArgs e)
		{ 
			ConString=System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
			Label lbvacio= new Label();
			string[] Formulas = new string[8];
			string[] ValFormulas = new string[8];
			string header = "AMS_HEADER.rpt";
			string footer = "AMS_FOOTER.rpt";
			DataSet tempDS = new DataSet();
			 
			Formulas[0] = "CLIENTE";
			Formulas[1] = "NIT";
			Formulas[2] = "TITULO";
			Formulas[3] = "TITULO1";
			Formulas[4] = "SELECCION1";
			Formulas[5] = "SELECCION2";
			Formulas[6] = "VERSION";
			Formulas[7] = "REPORTE";

			string empresa= DBFunctions.SingleData("select  cemp_nombre from dbxschema.cempresa");
			ValFormulas[0] = ""+empresa+""; //nombre empresa
			string nit= DBFunctions.SingleData("select  mnit_nit from dbxschema.cempresa");
			
			DataSet datosReporte= new DataSet();
			DBFunctions.Request(datosReporte,IncludeSchema.NO,"SELECT CNOM_NOMBPAGA,PANO_DETALLE,TMES_NOMBRE,TPER_DESCRIPCION,TOPCI_DESCRIPCION FROM DBXSCHEMA.PANO P, DBXSCHEMA.CNOMINA C,DBXSCHEMA.TMES  T,DBXSCHEMA.TPERIODONOMINA N,DBXSCHEMA.TOPCIQUINOMES O WHERE C.CNOM_ANO=P.PANO_ANO AND C.CNOM_MES=T.TMES_MES AND C.CNOM_QUINCENA=N.TPER_PERIODO AND C.CNOM_OPCIQUINOMENS=O.TOPCI_PERIODO");
			string quincena=datosReporte.Tables[0].Rows[0][3].ToString();
			string mes=datosReporte.Tables[0].Rows[0][2].ToString();
			string ano=datosReporte.Tables[0].Rows[0][1].ToString();
			ValFormulas[1] = ""+nit+"" ;
			ValFormulas[2] = "COMPROBANTES DE PAGO"; //titulo rpt
			ValFormulas[3] = "SISTEMA DE NOMINA"; //subtitulo Sistema de Nomina
			ValFormulas[4] = "AÑO:"+ano+" MES:"+mes+" QUINCENA:"+quincena+" "; //año mes quince
			ValFormulas[5] = ""; //
			ValFormulas[6] = "ECAS - AMS VER 3.0.1";

			int i,j,desccantidad=0;
			DBFunctions.NonQuery("delete from dbxschema.dquincenatemp");
			Imprimir funcion=new Imprimir();
			
            //lista de empleados
			ArrayList EmpleadosRevisados= new ArrayList();
            //recorro el datatable 
            for (i=0;i<DataTableLiqFinal.Rows.Count;i++)
			{
                //hago una busqueda en el array, si no esta lo ingreso a la lista
                //si esta es porque ya fue liquidado.

                if (EmpleadosRevisados.BinarySearch(DataTableLiqFinal.Rows[i][1].ToString())<0)
				{
                    //valido que el codigo sea de un empleado
                    if (DataTableLiqFinal.Rows[i][1].ToString()!="--")
					{
                        //ingreso el codigo en el array.
                        EmpleadosRevisados.Add(DataTableLiqFinal.Rows[i][1].ToString());
						//actualizo el maestro de vacaciones							
						DataSet fechaingreso= new DataSet();
						DBFunctions.Request(fechaingreso,IncludeSchema.NO,"select memp_fechingreso from dbxschema.mempleado where MEMP_CODIEMPL='"+DataTableLiqFinal.Rows[i][1].ToString()+"' ");	
						string estadoempleado=DBFunctions.SingleData("select test_estado from dbxschema.mempleado where MEMP_CODIEMPL='"+DataTableLiqFinal.Rows[i][1].ToString()+"' ");	
						//this.actualizarmaestrovacaciones(DataTableLiqFinal.Rows[i][1].ToString(),fechaingreso.Tables[0].Rows[0][0].ToString(),lb2,int.Parse(DataTableLiqFinal.Rows[i][0].ToString()));
					
						//query que trae los conceptos de cada empleado.				
						DataRow[]seleccion= DataTableLiqFinal.Select("CODIGO='"+DataTableLiqFinal.Rows[i][1].ToString()+"'");
						

						//string mqui_codiquin=seleccion[0][0].ToString();
						//string memp_codiempl=seleccion[0][1].ToString();
								
						for (j=0;j<seleccion.Length;j++)
						{
                            //segun la desc avergiaru el numero.
                            if (seleccion[j][8].ToString()=="DIAS")
								desccantidad=1;
							if (seleccion[j][8].ToString()=="HORAS")
								desccantidad=2;    
							if (seleccion[j][8].ToString()=="MINUTOS")
								desccantidad=3;
							if (seleccion[j][8].ToString()=="PESOS M/CTE")
								desccantidad=4;
								
							//inserto en la tabla de detalles de quincena.
							//DBFunctions.NonQuery("insert into dquincenatemp values("+seleccion[j][0].ToString()+",'"+seleccion[j][1].ToString()+"','"+seleccion[j][3].ToString()+"','"+seleccion[j][4].ToString()+"',"+seleccion[j][5].ToString()+","+seleccion[j][6].ToString()+","+seleccion[j][7].ToString()+","+desccantidad+" ,"+seleccion[j][9].ToString()+",'"+seleccion[j][10].ToString()+"','N')");
							DBFunctions.NonQuery("insert into dquincenatemp values("+seleccion[j][0].ToString()+",'"+seleccion[j][1].ToString()+"','"+seleccion[j][3].ToString()+"',"+seleccion[j][5].ToString()+","+seleccion[j][6].ToString()+","+seleccion[j][7].ToString()+","+seleccion[j][8].ToString()+","+desccantidad.ToString()+" ,"+seleccion[j][10].ToString()+",'"+seleccion[j][11].ToString()+"','N')");
                        }
					}
				}
			}
			string servidor=ConfigurationManager.AppSettings["Server" + GlobalData.getEMPRESA()];
			string database=ConfigurationManager.AppSettings["DataBase" + GlobalData.getEMPRESA()];
			string usuario =ConfigurationManager.AppSettings["UID"];
			string password=ConfigurationManager.AppSettings["PWD" + GlobalData.getEMPRESA()];
			AMS.CriptoServiceProvider.Crypto miCripto=new Crypto(AMS.CriptoServiceProvider.Crypto.CryptoProvider.TripleDES);
			miCripto.IV    =ConfigurationManager.AppSettings["VectorInicialEncriptacion"];
			miCripto.Key   =ConfigurationManager.AppSettings["ValorConcatClavePrivada"];
			string newPwd  =miCripto.DescifrarCadena(password);
			//funcion.PreviewReport("AMS.Nomina.InformeComprobantesPagosTemp",header,footer,1,1,1,"","","Preliquidacion Informativa","pdf",servidor,database,usuario,newPwd,Formulas,ValFormulas,lbvacio);
			funcion.PreviewReport2("AMS.Nomina.InformeComprobantesPagosTemp",header,footer,1,1,1,"","","Preliquidacion Informativa","pdf",usuario,newPwd,Formulas,ValFormulas,lbvacio);
			funcion.ReportUnload();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/pdf";
            //Response.WriteFile(Path.Combine(ConfigurationManager.AppSettings["PathToReports"],"AMS.Nomina.InformeComprobantesPagosTemp.pdf"));
            hl.NavigateUrl = path + "Preliquidacion Informativa_" + HttpContext.Current.User.Identity.Name + ".pdf";
            //hl.NavigateUrl = "../../rptgen/" + HttpContext.Current.User.Identity.Name + ".pdf";
            hl.Visible=true;
					
		}
		
		protected void revisarempleadosenvacaciones(string codigoempleado,string iniquin, string finquin, string periodopago)
		{
			int i=0,dia=0;
			string lbf=lb.Text;
			
			DataSet fechavacaciones = new DataSet();
			DateTime fecha= new DateTime();
			//revisar si el empleado tiene el estado de vacaciones.
			string estado=DBFunctions.SingleData("select test_estado from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
			if (estado=="5" ||  estado == "1")
			{
				
				//reviso si la fecha de ingreso de vacaciones es anterior al final de la liquidacion.
				//DBFunctions.Request(fechavacaciones,IncludeSchema.NO,"select DVAC.mvac_secuencia,DVAC.dvac_fechinic,DVAC.dvac_fechfinal,MVAC.memp_codiemp,DVAC.dvac_continuidad from dbxschema.dvacaciones DVAC,dbxschema.mvacaciones MVAC where DVAC.mvac_secuencia=MVAC.mvac_secuencia and MVAC.memp_codiemp='"+codigoempleado+"' and (DVAC.dvac_fechfinal between '"+iniquin.ToString()+"' and '"+finquin.ToString()+"')  " ); 
                DBFunctions.Request(fechavacaciones, IncludeSchema.NO, "select DVAC.mvac_secuencia,DVAC.dvac_fechinic,DVAC.dvac_fechfinal,MVAC.memp_codiemp,DVAC.dvac_continuidad from dbxschema.dvacaciones DVAC,dbxschema.mvacaciones MVAC where DVAC.mvac_secuencia=MVAC.mvac_secuencia and MVAC.memp_codiemp='" + codigoempleado + "' and DVAC.dvac_fechfinal>='" + iniquin.ToString() + "' and dvac.dvac_tiem > 0"); 
				if (fechavacaciones.Tables[0].Rows.Count>0)
				{
					if (fechavacaciones.Tables[0].Rows[i][4].ToString()=="C")
					{
						fecha=Convert.ToDateTime(fechavacaciones.Tables[0].Rows[fechavacaciones.Tables[0].Rows.Count-1][2].ToString());
						if (fecha>Convert.ToDateTime(finquin.ToString()))
							return;
					}
					else
					{
						fecha=Convert.ToDateTime(fechavacaciones.Tables[0].Rows[0][2].ToString());
						if (fecha>Convert.ToDateTime(finquin.ToString()))
							return;
					}
                    Utils.MostrarAlerta(Response, "El empleado con Codigo: " + codigoempleado + " reingreso a trabajar");
				
					for (i=0;i<fechavacaciones.Tables[0].Rows.Count;i++)
					{
						//armar las nuevas fechas de liquidacion para este empleado.
						fecha=Convert.ToDateTime(fechavacaciones.Tables[0].Rows[i][2].ToString());
						dia=fecha.Day;
						//SI ESCOGIO PAGO PRIMERA QUINCENA
						if (DDLQUIN.SelectedValue=="1")
						{
		    				if (periodopago=="1")
							{
			
								if(Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
								{
									lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+dia;
									lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"15";
									lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"16";
								}
								else
								{
									lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+dia;
									lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"15";
									lbmas1.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"16";
								}
								fecha3=lb2.Text;
							}
		  	            }
		
						//SI ESCOGIO PAGO SEGUNDA QUINCENA
						if (DDLQUIN.SelectedValue=="2")
						{
							//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd 
							if (periodopago=="1")
							{
								if(Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
								{
									if(Convert.ToInt32(o_CNomina.CNOM_MES)==2)
									{
										lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+dia;
										lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"28";
										lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"29";
									}
									else
									{
										lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+dia;
										lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
										lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"31";	
									}
								}
								else
								{
									lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+dia;
									lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"30";
									lbmas1.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"31";
								}	
							fecha3=lb2.Text;
							}
						}	
						//mensual
						if (periodopago=="2")
						{
							//DBFunctions.Request(empleados,IncludeSchema.NO,"Select MEMP_CODIEMPL as Codigo,MEMP_NOMBRE1 as Nombre,MEMP_APELLIDO1 as Apellido, MEMP_APELLIDO2 as SegundoApellido,MEMP_SUELACTU as SueldoQuincena, memp_suelactu as SueldoMes,memp_fechingreso as fecha,memp_nombre2,tsub_codigo,tsal_salario from DBXSCHEMA.mempleado ,DBXSCHEMA.CNOMINA  where DBXSCHEMA.mempleado.test_estado='1' AND DBXSCHEMA.mempleado.memp_peripago=2 ORDER BY memp_codiempl;");
							DBFunctions.Request(tsalarionomina,IncludeSchema.NO,"select * from dbxschema.tsalarionomina;");
							//DataSet nomensuales= new DataSet();
							//DBFunctions.Request(nomensuales,IncludeSchema.NO,"Select MEMP_CODIEMPL,MEMP_NOMBRE1,MEMP_APELLIDO1, MEMP_APELLIDO2 ,memp_nombre2,tper_descp from DBXSCHEMA.mempleado E,DBXSCHEMA.CNOMINA C,dbxschema.tperipago T  where E.test_estado='1' AND (E.memp_peripago=1 or E.memp_peripago=3) and memp_peripago=tper_peri ORDER BY memp_codiempl");
							if(Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
							{
		   					    if(Convert.ToInt32(o_CNomina.CNOM_MES)==2)
								{
							        lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+dia;
									lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"28";
									lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"29";
								}
								else
								{
						            lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+dia;
									lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
					
								}
							}
							else
							{
								lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+dia;
								lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"30";
			
							}
								fecha3=lb2.Text;
						}
					}
				}

				DateTime fechainif= new DateTime();
				DateTime fechafinalliquidacion= new DateTime();
				DateTime fechainicioliquidacion= new DateTime();
				fechafinalliquidacion=Convert.ToDateTime(lb2.Text);
				fechainicioliquidacion=Convert.ToDateTime(lb.Text);
				fechainif=Convert.ToDateTime(lbf);
				
				TimeSpan diaspago=fechafinalliquidacion-fechainicioliquidacion;
				numerodias=diaspago.Days;
				if (numerodias==0)
					activarEmp.Add(codigoempleado);
				
				TimeSpan diasexceptos=fechainicioliquidacion-fechainif;
				diasexceptosauxtransp=diasexceptos.Days;
				diasexceptosauxtransp+=1;
				
			    Session["activarEmp"]=activarEmp;
			}
		}
		
		protected void validarfincontrato(int i_emp, string codigoempleado,string fechafincontrato,string lb2,string tipocontrato,string nombres,string apellidos)
		{
						
			if (tipocontrato=="2")
			{
				string infoempleado;
				int diasfaltfincontrato2=0;
				TimeSpan diasfaltfincontrato = new TimeSpan();
				DateTime fcontrato = new DateTime();
				DateTime ffinliquidacion = new DateTime();
				if(fechafincontrato == String.Empty)
				{
                    Utils.MostrarAlerta(Response, "EL empleado " + codigoempleado + " no tiene ninguna fecha definida.");
					return;
				}
				fcontrato = Convert.ToDateTime(fechafincontrato);
				ffinliquidacion = Convert.ToDateTime(lb2);
				infoempleado = nombres+" "+apellidos;
				string codigoempleado2 = codigoempleado.Trim();
				//Mostrar cuantos dias de contrato le quedan al empleado.
				if (ffinliquidacion<fcontrato)
				{
					diasfaltfincontrato = fcontrato-ffinliquidacion;
					diasfaltfincontrato2= diasfaltfincontrato.Days;
				}
				 
				/*if (diasfaltfincontrato2<60)
				{
					Response.Write("<script language:javascript>alert('ATENCION: Al empleado "+codigoempleado2+" "+infoempleado+" le faltan "+diasfaltfincontrato2+" dias para la finalizacion de su contrato fijo.');</script>");
				}*/
			}
		}
		
		
		protected void actualizarmaestrovacaciones(string codigoempleado,string fechaingempl,string lb2,int codigoquincena)
		{
			//sacamos la fecha final de la quincena que estoy liquidando y se la restamos a la fecha 
			//de ingreso del empleado actualizada un año menos a la primera quincena liquidada.

			DataSet anoprimeraquincenaliquidada = new DataSet();
			DateTime fechaemping=Convert.ToDateTime(fechaingempl);
			DateTime fquin = Convert.ToDateTime(lb2);
			//le rebajo un dia a la fecha final
			DBFunctions.Request(anoprimeraquincenaliquidada,IncludeSchema.NO,"select min(mqui_anoquin) from dbxschema.mquincenas");
			
			int anoinical,mesingempl,diaingempl,diasingempl,periodo;
			int diastrabajados2,diasvacaciones=0,diasactuales;
			int numperiodos,saldodias,ano,mes,dia,j;
			string fechauno;
			
			//actualizar fecha de ingreso empleado,saco mes,dia le coloco un año menos. 
			mesingempl=int.Parse(fechaemping.Month.ToString());
			diaingempl=int.Parse(fechaemping.Day.ToString());
			anoinical =int.Parse(anoprimeraquincenaliquidada.Tables[0].Rows[0][0].ToString())-1;
							
			//armo la fecha en formato datetime para posterior ingreso a BD.
			fechauno=anoinical+"-"+mesingempl+"-"+diaingempl;
			DateTime mvac_desde=Convert.ToDateTime(fechauno);
						
			if (fechaemping > mvac_desde)
			{
				//Armo la fecha desde con la fecha de ingeso del empleado (comienzan)
				mesingempl=int.Parse(fechaemping.Month.ToString());
				diaingempl=int.Parse(fechaemping.Day.ToString());
				anoinical=fechaemping.Year;
				fechauno=anoinical+"-"+mesingempl+"-"+diaingempl;
				mvac_desde=Convert.ToDateTime(fechauno);
			}
			
			//calcular los dias de la primera fecha (por año del empleado).
			diasingempl=(anoinical*360)+(mesingempl*30)+diaingempl;
			
			//calcular dias de la segunda fecha final de la liquidacion
			mesingempl=int.Parse(fquin.Month.ToString());
			diaingempl=int.Parse(fquin.Day.ToString());
			anoinical=fquin.Year;
			
			diasactuales=(anoinical*360)+(mesingempl*30)+diaingempl;
					
			//dias trabajados trancurridos fecha liquidacion quincena- fecha ing empleado convertida
			
			diastrabajados2=diasactuales-diasingempl;
			//calculo los dias causados de vacaciones
			diasvacaciones=diastrabajados2*15/360;
			//ingresar los dias a la tabla mvacaciones
			DataSet periodosvaca = new DataSet();
			DataSet diascausados = new DataSet();
				
			DBFunctions.Request(periodosvaca,IncludeSchema.NO,"select  count(mvac_periodo) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'");
			//no se ha ingresado ningun periodo de vacaciones nunca .
			if (periodosvaca.Tables[0].Rows[0][0].ToString()=="0")
			{
				periodo=1;
				if (diasvacaciones<=14)
				{                                     //Inserta mvacaciones por primera vez
					sqlStrings.Add("insert into dbxschema.mvacaciones values (default,"+periodo+",'"+codigoempleado+"',"+diasvacaciones+",0,'"+mvac_desde.ToString("yyyy-MM-dd")+"','"+fquin.ToString("yyyy-MM-dd")+"') ");	
					
					sqlStrings.Add("insert into dbxschema.dquinvacadias values ("+codigoquincena+",(select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'),"+diasvacaciones+")");
				}
				else
				{
					//averiguar cuantos periodos años Hay que ingresarle
					numperiodos=diasvacaciones/15;
					saldodias=diasvacaciones%15;
					                           
					for (j=1;j<=numperiodos;j++)
					{
						//partir las fechas, año en año.
						ano=mvac_desde.Year;
						mes=int.Parse(mvac_desde.Month.ToString());
						dia=int.Parse(mvac_desde.Day.ToString());
						ano+=1;
							
						//armar la fecha final
						string fechafinal=ano+"-"+mes+"-"+dia;
                        Utils.MostrarAlerta(Response, "fecha periodo un año finaliza en " + fechafinal + "");
						DateTime fechafin=Convert.ToDateTime(fechafinal);
						fechafin=fechafin.AddDays(-1);
						sqlStrings.Add("insert into dbxschema.mvacaciones values (default,"+j+",'"+codigoempleado+"',"+15+",0,'"+mvac_desde.ToString("yyyy-MM-dd")+"','"+fechafin.ToString("yyyy-MM-dd")+"') ");	
						//						DBFunctions.NonQuery("insert into dbxschema.mvacaciones values (default,"+j+",'"+codigoempleado+"',"+15+",0,'"+mvac_desde.ToString("yyyy-MM-dd")+"','"+fechafin.ToString("yyyy-MM-dd")+"') ");
						//						secuencia=DBFunctions.SingleData("select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'");
						sqlStrings.Add("insert into dbxschema.dquinvacadias values ("+codigoquincena+",(select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'),15)");
						//						DBFunctions.NonQuery("insert into dbxschema.dquinvacadias values ("+codigoquincena+",select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"',15)");
						mvac_desde=Convert.ToDateTime(fechafinal);
					}
					//insertar en el siguiente perido los dias que sobraron,insertar saldo dias
					//numero de periodos igua a j 						
					sqlStrings.Add("insert into dbxschema.mvacaciones values (default,"+j+",'"+codigoempleado+"',"+saldodias+",0,'"+mvac_desde.ToString("yyyy-MM-dd")+"','"+fquin.ToString("yyyy-MM-dd")+"') ");
					//					DBFunctions.NonQuery("insert into dbxschema.mvacaciones values (default,"+j+",'"+codigoempleado+"',"+saldodias+",0,'"+mvac_desde.ToString("yyyy-MM-dd")+"','"+fquin.ToString("yyyy-MM-dd")+"') ");
					//					secuencia=DBFunctions.SingleData("select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'");
					sqlStrings.Add("insert into dbxschema.dquinvacadias values ("+codigoquincena+",(select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'),"+saldodias+")");
					//					DBFunctions.NonQuery("insert into dbxschema.dquinvacadias values ("+codigoquincena+","+secuencia+","+saldodias+") ");					
				}
			}
			else
			{
				DataSet perxcompletar = new DataSet();
				DBFunctions.Request(perxcompletar,IncludeSchema.NO,"select mvac_diascaus,memp_codiemp,mvac_desde,mvac_periodo from dbxschema.mvacaciones where mvac_diascaus<15 and memp_codiemp='"+codigoempleado+"' " );
				// OJO puede quedar mal debe ser el periodo del empleado que corresponda a la quincena
				// calcular dias fecha inicial
					
				//validar los periodos inconclusos
				if (perxcompletar.Tables[0].Rows.Count>0)
				{
					diasingempl=0;
					diasactuales=0;
						
					DateTime mvac_desde2=Convert.ToDateTime(perxcompletar.Tables[0].Rows[0][2].ToString());
					mesingempl=int.Parse(mvac_desde2.Month.ToString());
					diaingempl=int.Parse(mvac_desde2.Day.ToString());
					anoinical=mvac_desde2.Year;
					diasingempl=(anoinical*360)+(mesingempl*30)+diaingempl;
					
					//calcular dias fecha final.
					mes=int.Parse(fquin.Month.ToString());
					dia=int.Parse(fquin.Day.ToString());
					anoinical=fquin.Year;
					diasactuales=(anoinical*360)+(mes*30)+dia;
					//sacar los dias trabajados fecha liquidacion quincena- fecha ing empleado convertida
					diastrabajados2=diasactuales-diasingempl;
					//calculo los dias causados de vacaciones
					diasvacaciones=diastrabajados2*15/360;
					//si dias calculados vacaciones <=15 actualizo el registro en BD.
					if (diasvacaciones<=14)
					{
						//Response.Write("<script language:java>alert('encontro periodo y actualizo...dio 14 o menos   ');</script>");
						sqlStrings.Add("update dbxschema.mvacaciones set mvac_diascaus="+diasvacaciones+",mvac_hasta='"+fquin.ToString("yyyy-MM-dd")+"' where mvac_diascaus<15 and memp_codiemp='"+codigoempleado+"' ");
						//						DBFunctions.NonQuery("update dbxschema.mvacaciones set mvac_diascaus="+diasvacaciones+",mvac_hasta='"+fquin.ToString("yyyy-MM-dd")+"' where mvac_diascaus<15 and memp_codiemp='"+codigoempleado+"' ");
						//						secuencia=DBFunctions.SingleData("select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'");
						sqlStrings.Add("insert into dbxschema.dquinvacadias values ("+codigoquincena+",(select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'),"+diasvacaciones+") ");
						//						DBFunctions.NonQuery("insert into dbxschema.dquinvacadias values ("+codigoquincena+","+secuencia+","+diasvacaciones+") ");
					}
					else
					{
      					//paso de 15 dias o igual ,ingreso 15 en el periodo y abro un nuevo periodo
						//armar fecha 1 año despues del mvac_desde2
						//armar fecha de fin del periodo.
						mvac_desde2=mvac_desde2.AddDays(-1);
						ano=mvac_desde2.Year;
						ano+=1;
						mes=mvac_desde2.Month;
						dia=mvac_desde2.Day;
                        if (dia - 29 == mes - 2) dia = 28;
						string fechanew=ano+"-"+mes+"-"+dia;
						DateTime mvac_hasta=Convert.ToDateTime(fechanew);
						mvac_desde2=mvac_desde2.AddDays(1);
						sqlStrings.Add("update dbxschema.mvacaciones set mvac_diascaus=15,mvac_hasta='"+mvac_hasta.ToString("yyyy-MM-dd")+"' where mvac_diascaus<15 and memp_codiemp='"+codigoempleado+"' ");
						//						DBFunctions.NonQuery("update dbxschema.mvacaciones set mvac_diascaus=15,mvac_hasta='"+mvac_hasta.ToString("yyyy-MM-dd")+"' where mvac_diascaus<15 and memp_codiemp='"+codigoempleado+"' ");
						//						secuencia=DBFunctions.SingleData("select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'");
						sqlStrings.Add("insert into dbxschema.dquinvacadias values ("+codigoquincena+",(select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'),15)");
						//						DBFunctions.NonQuery("insert into dbxschema.dquinvacadias values ("+codigoquincena+","+secuencia+","+15+") ");
						//armar fecha siguiente periodo
						//calcular dias desde
						mesingempl=int.Parse(mvac_desde2.Month.ToString());
						diaingempl=int.Parse(mvac_desde2.Day.ToString());
						anoinical=mvac_desde2.Year;
						anoinical+=1;
						fechanew=anoinical+"-"+mesingempl+"-"+diaingempl;
						mvac_desde2=Convert.ToDateTime(fechanew);
						diasingempl=(anoinical*360)+(mesingempl*30)+diaingempl;
						//calcular dias fecha final.
						mes=int.Parse(fquin.Month.ToString());
						dia=int.Parse(fquin.Day.ToString());
						anoinical=fquin.Year;
						diasactuales=(anoinical*360)+(mes*30)+dia;
						
						diastrabajados2=diasactuales-diasingempl;
						diasvacaciones=diastrabajados2*15/360;
						
						//nuevo periodo
							
						periodo=int.Parse(perxcompletar.Tables[0].Rows[0][3].ToString())+1;
						sqlStrings.Add("insert into dbxschema.mvacaciones values (default,"+periodo+",'"+codigoempleado+"',"+diasvacaciones+",0,'"+mvac_desde2.ToString("yyyy-MM-dd")+"','"+fquin.ToString("yyyy-MM-dd")+"') ");
						//						DBFunctions.NonQuery("insert into dbxschema.mvacaciones values (default,"+periodo+",'"+codigoempleado+"',"+diasvacaciones+",0,'"+mvac_desde2.ToString("yyyy-MM-dd")+"','"+fquin.ToString("yyyy-MM-dd")+"') ");
						//						secuencia=DBFunctions.SingleData("select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'");
						sqlStrings.Add("insert into dbxschema.dquinvacadias values ("+codigoquincena+",(select max(mvac_secuencia) from dbxschema.mvacaciones where memp_codiemp='"+codigoempleado+"'),"+diasvacaciones+") ");
						//						DBFunctions.NonQuery("insert into dbxschema.dquinvacadias values ("+codigoquincena+","+secuencia+","+diasvacaciones+") ");
					}
				}
				else
				{
					//no encontro periodos incompletos abrir un nuevo periodo.
					//periodo=int.Parse(perxcompletar.Tables[0].Rows[0][3].ToString())+1;
				}
			}
		}	
		
		protected void fechaliquidacion(string periodopago, string periodo)
		{
			if(periodo == "1")
			{
				//SI ESCOGIO PAGO PRIMERA QUINCENA
				if (DDLQUIN.SelectedValue=="1")
				{
					//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd para cada empleado
					if (periodopago=="1" || periodopago=="3" )
					{
						if(Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
						{
							lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"01";
							lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"15";
							lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"16";
						}
						else
						{
							lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"01";
							lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"15";
							lbmas1.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"16";
						}
						fecha3=lb2.Text;
					}
		        }
				
			   //SI ESCOGIO PAGO SEGUNDA QUINCENA
			   if (DDLQUIN.SelectedValue=="2")
			   {
				//conversion fecha de cnomina para la quincena vigente yyyy-mm-dd a yyyy-mm-dd 
				  if (periodopago=="1" || periodopago=="3" )
				  {
				      if (Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
				      {
						 if(Convert.ToInt32(o_CNomina.CNOM_MES)==2)
						 {
						   lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"16";
                           int diasMes = Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()) / 4;
                           diasMes = diasMes * 4;
                           if (diasMes == Convert.ToInt32(o_CNomina.CNOM_ANO.ToString()))
                           {
                               diasMes = 29;
                               lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                               lbmas1.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                               fecha3 = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "29";
                           }
                           else
                           {
                               lb2.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                               lbmas1.Text = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                               fecha3 = o_CNomina.CNOM_ANO + "-0" + o_CNomina.CNOM_MES + "-" + "28";
                           }
						 }
						 else
							 {
								lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"16";
								lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
								lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"31";
								fecha3=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
							 }
							//fecha3=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"15";
				       }
				    else
					   {
						lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"16";
						lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"30";
						fecha3=lb2.Text;
						lbmas1.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"31";
					   }	
				   }
			   }	
			}
			else
			{
				//mensual
				if (periodopago=="2" || periodopago=="3")
				{
					//DBFunctions.Request(empleados,IncludeSchema.NO,"Select MEMP_CODIEMPL as Codigo,MEMP_NOMBRE1 as Nombre,MEMP_APELLIDO1 as Apellido, MEMP_APELLIDO2 as SegundoApellido,MEMP_SUELACTU as SueldoQuincena, memp_suelactu as SueldoMes,memp_fechingreso as fecha,memp_nombre2,tsub_codigo,tsal_salario from DBXSCHEMA.mempleado ,DBXSCHEMA.CNOMINA  where DBXSCHEMA.mempleado.test_estado='1' AND DBXSCHEMA.mempleado.memp_peripago=2 ORDER BY memp_codiempl;");
					DBFunctions.Request(tsalarionomina,IncludeSchema.NO,"select * from dbxschema.tsalarionomina;");
					//DataSet nomensuales= new DataSet();
					//DBFunctions.Request(nomensuales,IncludeSchema.NO,"Select MEMP_CODIEMPL,MEMP_NOMBRE1,MEMP_APELLIDO1, MEMP_APELLIDO2 ,memp_nombre2,tper_descp from DBXSCHEMA.mempleado E,DBXSCHEMA.CNOMINA C,dbxschema.tperipago T  where E.test_estado='1' AND (E.memp_peripago=1 or E.memp_peripago=3) and memp_peripago=tper_peri ORDER BY memp_codiempl");
					if(Convert.ToInt32(o_CNomina.CNOM_MES)>=1 && Convert.ToInt32(o_CNomina.CNOM_MES)<=9)
					{
						if(Convert.ToInt32(o_CNomina.CNOM_MES)==2)
						{
							lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"01";
							lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"28";
							lbmas1.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"29";
							fecha3=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"28";
						}
						else
						{
							lb.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"01";
							lb2.Text=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
							fecha3=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"30";
						}
						if(DDLQUIN.SelectedValue=="1")
						{
							fecha3=o_CNomina.CNOM_ANO+"-0"+o_CNomina.CNOM_MES+"-"+"15";
						}							
					}
					else
					{
						lb.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"01";
						lb2.Text=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"30";
						if(DDLQUIN.SelectedValue=="1")
						{
							fecha3=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"15";
						}
						else
						{
							fecha3=o_CNomina.CNOM_ANO+"-"+o_CNomina.CNOM_MES+"-"+"30";
						}
					}
		
					/*
					if (nomensuales.Tables[0].Rows.Count>0)
						{
							Response.Write("<script language:javascript>alert('Se encontro empleados a los cuales se les paga quincenal o jornal, ud esta procesando liquidacion mensual estos empleados no seran tenidos en cuenta, porfavor Revise el listado posterior al de la liquidacion.. ');</script>");
							this.preparargrilla_empleadosdiferenteperipago();
							for(i=0;i<nomensuales.Tables[0].Rows.Count;i++)
							{
								this.ingresar_datos_empleadosdiferenteperipago(nomensuales.Tables[0].Rows[i][0].ToString(),nomensuales.Tables[0].Rows[i][2].ToString(),nomensuales.Tables[0].Rows[i][3].ToString(),nomensuales.Tables[0].Rows[i][1].ToString(),nomensuales.Tables[0].Rows[i][4].ToString(),nomensuales.Tables[0].Rows[i][5].ToString());
							}
							//DataGrid1.DataSource=nomensuales.Tables[0];
							//DataGrid1.DataBind();
							//Session["dos"]=nomensuales.Tables[0];
							lbperipago.Visible=true;
				
						}
					*/
				}
			}
		}
		
		
		protected void liquidar_provisiones(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,double acumuladoprovisiones,string tiposalario)
		{
			string tcontrato=DBFunctions.SingleData("select  tcon_contrato from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"' ");
			int i;
			double provcesantias,provinteresescesantia,provprimadeservicios,provvacaciones,provextralegal;
			DataSet pprovisionnomina= new DataSet();
			//DataSet cnomina= new DataSet();	
			DBFunctions.Request(pprovisionnomina,IncludeSchema.NO,"select ppro_codiprov,ppro_nombprov,ppro_porcprov,ppro_ttipo from dbxschema.pprovisionnomina");
			//DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_substransactu,cnom_baseporcsalainteg from dbxschema.cnomina");			
			string porcCesa=DBFunctions.SingleData("Select ppro_porcprov  from DBXSCHEMA.PPROVISIONNOMINA where ppro_ttipo=1");

			for (i=0;i<pprovisionnomina.Tables[0].Rows.Count;i++)
			{
				//provision cesantias
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="1")
				{
					if (tcontrato!="3" && tcontrato != "5")
					{
						if (tiposalario=="1")
						{
							provcesantias=0;
						}
						else
						{
							provcesantias= acumuladoprovisiones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						}
					
						sqlStrings.Add("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provcesantias+")");
						//						DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provcesantias+")");
					}
					
					//TXTPRUEBA.Text+="insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"',"+pprovisionnomina.Tables[0].Rows[0][0].ToString()+","+provcesantias+" ";
				}
				//provision intereses de cesantia
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="2")
				{
					if (tcontrato!="3" && tcontrato != "5")
					{
						if (tiposalario=="1")
						{
							provinteresescesantia=0;
						}
						else
						{
							//antes lo tenia asi.
							provcesantias= acumuladoprovisiones * double.Parse(porcCesa)/100;
							provinteresescesantia=provcesantias* double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
							//provinteresescesantia=acumuladoprovisiones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
					
						}

						sqlStrings.Add("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provinteresescesantia+")");
						//						DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provinteresescesantia+")");
					}
				}
				//provision prima de servicios
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="3")
				{
					if (tcontrato!="3" && tcontrato != "5")
					{
						if (tiposalario=="1")
						{
							provprimadeservicios=0;
						}
						else
						{
							provprimadeservicios=acumuladoprovisiones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						}
						sqlStrings.Add("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provprimadeservicios+")");
						//						DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provprimadeservicios+")");
					}
				}
				//provision vacaciones
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="4")
				{
					if (tcontrato!="3" && tcontrato != "5")
					{
						if (tiposalario=="1")
						{
							provvacaciones=(acumuladovacaciones*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100) * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						}
						else
						{
							provvacaciones=acumuladovacaciones * double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						}
						sqlStrings.Add("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provvacaciones+")");					
						//DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provvacaciones+")");
					}
				}
				//provision extralegal
				if (pprovisionnomina.Tables[0].Rows[i][3].ToString()=="5")
				{
					if (tcontrato!="3" && tcontrato != "5")
					{
						if (tiposalario=="1")
						{
							provextralegal=0;
						}
						else
						{
							provextralegal=acumuladoprovisiones *double.Parse(pprovisionnomina.Tables[0].Rows[i][2].ToString())/100;
						}
						sqlStrings.Add("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provextralegal+")");
						//						DBFunctions.NonQuery("insert into mprovisiones values ( default,"+codquincena+",'"+codigoempleado+"','"+pprovisionnomina.Tables[0].Rows[i][0].ToString()+"',"+provextralegal+")");
					}
				}
			}
		}
		
		
		protected void liquidar_fondosolidaridadpensional(int i_emp, string codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario, double sueldomes)
		{
            /*
            if ((diasvaca + diassueldopagado) >= 30 && lbtipopago.Text == "MENSUAL") return;
            if ((diasvaca + diassueldopagado) >= 15 && lbtipopago.Text == "QUINCENAL" && tipoPeriodoPago == "1") 
				return;
            */
            string tcontrato = empleados.Tables[0].Rows[i_emp]["TCON_CONTRATO"].ToString(); // DBFunctions.SingleData("select tcon_contrato from dbxschema.mempleado where memp_codiempl='" + codigoempleado + "' ");
            if (tcontrato == "4") return;  // contrato tipo pensionado NO APORTA FONDO DE SOLIDARIDAD

            //jfsc aqui debe venir una validación para los dias que se descuentan en fondo solidaridad

            if(this.diastrabajadosmes<=15)
			{
				if(diasvaca==0 || idiasexceptosauxtransp==0)
				{
					this.diastrabajadosmes=30;
					if(idiasexceptosauxtransp!=0)
						this.diastrabajadosmes-=idiasexceptosauxtransp;
					if(diasvaca!=0)
						this.diastrabajadosmes-=diasvaca;
				}
			}
  //ver          if(DDLQUIN.SelectedValue=="2" && peripago=="1")
  //ver			   acumuladoeps=acumuladoeps-double.Parse(pagosquincenaant.ToString()==String.Empty?"0":pagosquincenaant.ToString());
			DateTime fechaquin= new DateTime();
			DataSet mquincenas= new DataSet();
			//DataSet cnomina= new DataSet();
			//DataSet cnomina2= new DataSet();
			DataSet validar= new DataSet();
			//DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_concfondsolipens,cnom_porcfondsolipens,cnom_topepagofondsolipens,cnom_salaminiactu,cnom_baseporcsalainteg from dbxschema.cnomina");
			//DBFunctions.Request(cnomina2,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");	
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+o_CNomina.CNOM_ANO+" and mqui_mesquin="+o_CNomina.CNOM_MES+" and mqui_tpernomi="+o_CNomina.CNOM_QUINCENA+" ");
			DBFunctions.Request(validar, IncludeSchema.NO,"select * from dbxschema.pfondosolipens");
            string descripcionconcepto = ViewState["nombconcFondoSolidaridad"].ToString(); //DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+o_CNomina.CNOM_CONCFONDSOLIPENS+"' ");
            //DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
            double topesalminimos,salariocompletomes,descuentosolipens,baseint;
			topesalminimos=double.Parse(o_CNomina.CNOM_SALAMINIACTU)*double.Parse(o_CNomina.CNOM_TOPEPAGOFONDSOLIPENS);
            double topesalmaximos = (double.Parse(o_CNomina.CNOM_TOPEPAGOEPS) * double.Parse(o_CNomina.CNOM_SALAMINIACTU));
			int numquincenaanterior=0;
			string porcentaje;
			//validar que la tabla contenga datos
			if (validar.Tables[0].Rows.Count==0)
			{
                Utils.MostrarAlerta(Response, "Por favor Ingrese la tabla de Descuentos de Solidaridad Pensional");
			}
			else
			{
				if (codquincena=="")
				{
					numquincenaanterior=0;
				}
				else
				{
					//numquincenaanterior=int.Parse(codquincena)-1;
					if (o_CNomina.CNOM_QUINCENA=="2")
					{
						if (lbtipopago.Text=="QUINCENAL")
						{
							numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=1 "));
						}
					}
					else
					{
						DataSet conteo= new DataSet();
						DBFunctions.Request(conteo,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");
						if(conteo.Tables.Count > 0 && conteo.Tables[0].Rows.Count>0)
						  {
							fechaquin=Convert.ToDateTime(lb);
							fechaquin= fechaquin.AddMonths(-1);
							numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
						  }
					}
				}
				
				DataSet valepsquinanterior= new DataSet();
                DBFunctions.Request(valepsquinanterior, IncludeSchema.NO, "SELECT COALESCE(SUM(DQUI_ADESCONTAR),0) FROM DQUINCENA D, CNOMINA CN WHERE MEMP_CODIEMPL = '" + codigoempleado + "' and mqui_codiquin IN (" + numquincenaanterior + "," + codquincena + ") AND D.PCON_CONCEPTO = CN.CNOM_CONCFONDSOLIPENS;");
			 	//solo reviso si se esta liquidando la segunda quincena.
				if (DDLQUIN.SelectedValue=="2")
				{
					//validar la liquidacion de la segunda quincena sin registros anteriores				
					if (lbtipopago.Text=="QUINCENAL")
					{
						if (valepsquinanterior.Tables[0].Rows.Count==0)
						{
							bandera1+=1;
							if (bandera1==1)
							{
                                Utils.MostrarAlerta(Response, "Atencion:No se encontro registro de la primera quincena para liquidar Fondo de Solidaridad Pensional Empleado: " + codigoempleado + " ,solo se tomara en cuenta el salario devengado en la segunda quincena,pudiendo afectar el calculo del tope. ");
							}
							salariocompletomes=acumuladoeps;
                            if (salariocompletomes > topesalmaximos)
                                salariocompletomes = topesalmaximos;
							/*string query=@"SELECT coalesce(SUM(DQUI.DQUI_APAGAR),0) 
											FROM DBXSCHEMA.DQUINCENA DQUI,
												DBXSCHEMA.PCONCEPTONOMINA PCON 
											WHERE MQUI_CODIQUIN="+numquincenaanterior+  
												@" AND MEMP_CODIEMPL='"+codigoempleado+
												@"' 
												AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO 
												AND PCON.TRES_AFEC_EPS='S'";
							string quinanterior=DBFunctions.SingleData(query);
							double posibleTope;
							double posibleQuinanterior=Double.Parse(quinanterior==string.Empty?"0":quinanterior);
							posibleTope=acumuladoeps+posibleQuinanterior;*/
							double posibleTope=acumuladoeps;
							if (posibleTope>topesalminimos)
							{
								//sacar el valor de porcentaje de la tabla PFONDOSOLIPENS.
								if (tiposalario=="1")
								{
									baseint=(posibleTope*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG))/100;
									porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, CNOMINA where ("+baseint+" / CNOM_SALAMINIACTU) between pfon_intinicio and pfon_intfinal");
								}
								else
								{
									baseint=0;
									porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens, CNOMINA where ("+posibleTope+ "/ CNOM_SALAMINIACTU) between pfon_intinicio and pfon_intfinal");
									if(porcentaje==string.Empty) porcentaje="0";
								}
								//ingreso el descuento si es integral sobre el 70% variable tomada de cnomina.
								if (tiposalario=="1")
								{
									descuentosolipens=((salariocompletomes*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(porcentaje)/100);
                                    descuentosolipens = Math.Round(descuentosolipens, 0) - Convert.ToDouble(valepsquinanterior.Tables[0].Rows[0][0].ToString()); 
						// ya se%	descuentosolipens=(descuentosolipens*double.Parse(porcentaje)/100);
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);
									//Response.Write("<script language:javascript>alert(' valor solidaridad descontado : "+fondosolidaridadpensional+"   ');</script>");				
								}
								else
								{
									descuentosolipens=(salariocompletomes*double.Parse(porcentaje)/100);
                                    descuentosolipens = Math.Round(descuentosolipens, 0) - Convert.ToDouble(valepsquinanterior.Tables[0].Rows[0][0].ToString()); 
									/*descuentosolipens=(descuentosolipens*double.Parse(porcentaje)/100);
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);*/
								}
								if (porcentaje!="0")
								{
				// hac				descuentosolipens=(descuentosolipens/30)*this.diastrabajadosmes;
									this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									if (banderaNombre==0)
									{
										this.ingresar_datos_datatable    (codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									//	banderaNombre+=1;
									}
									else
									{
										this.ingresar_datos_datatable    (codquincena,"",o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
									}
								
								}
								sumadescontado+=descuentosolipens;
							}
						}
						if (valepsquinanterior.Tables[0].Rows.Count>0)
						{
							//salariocompletomes=double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString());
							//salariocompletomes+=acumuladoeps;
                            salariocompletomes = acumuladoeps;
                            if (tiposalario == "1")
                            {
                                if (Math.Round(salariocompletomes*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)*0.01) > topesalmaximos)
                                    salariocompletomes = topesalmaximos;
                            }
                            else
                            {
                                if (salariocompletomes > topesalmaximos)
                                    salariocompletomes = topesalmaximos;
                           
                            }

							// porcentaje=DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens where "+salariocompletomes+" between pfon_intinicio and pfon_intfinal");
							//si se le pago mas de (lo que diga cnomina) salarios minimos descontar lo que diga cnomina
							//salariocompletomes+=acumuladoeps;
							if (salariocompletomes>topesalminimos)
							{
								//ingreso el descuento si es integral sobre el 70%
								if (tiposalario=="1")   // 1 = integral
								{
									baseint=Math.Round((salariocompletomes*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)*0.01)/Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU),4);
                                    porcentaje = DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens where " + baseint + " between pfon_intinicio and pfon_intfinal");
								}
								else
								{
                                    baseint = Math.Round(salariocompletomes/Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU),4);
                                    porcentaje = DBFunctions.SingleData("select pfon_porcentaje from dbxschema.pfondosolipens where " + baseint + " between pfon_intinicio and pfon_intfinal");
								}
					
								//ingreso el descuento si es integral sobre el 70% variable tomada de cnomina.
								if (tiposalario=="1")
								{
						
									descuentosolipens=((salariocompletomes*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(porcentaje)/100);
                            //        descuentosolipens = Math.Round(descuentosolipens, 0) - Convert.ToDouble(valepsquinanterior.Tables[0].Rows[0][0].ToString());
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);
								}
								else
								{
									descuentosolipens=(salariocompletomes*double.Parse(porcentaje)/100);
                            //        descuentosolipens = Math.Round(descuentosolipens, 0) - Convert.ToDouble(valepsquinanterior.Tables[0].Rows[0][0].ToString()); 
									fondosolidaridadpensional=Math.Round(descuentosolipens,0);					
								}
                                double soliDesc = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(SUM(DQUI_ADESCONTAR - DQUI_APAGAR),0) FROM DQUINCENA D, MQUINCENAS M, CNOMINA CN WHERE MQUI_ANOQUIN = CN.CNOM_ANO AND MQUI_MESQUIN = CN.CNOM_MES AND M.MQUI_CODIQUIN = D.MQUI_CODIQUIN AND D.MEMP_CODIEMPL = '" + codigoempleado + "' AND D.PCON_CONCEPTO = CN.CNOM_CONCFONDSOLIPENS"));
                                fondosolidaridadpensional -= soliDesc;

                                if (porcentaje!="0" && salariocompletomes>topesalminimos)
								{
						// hac		descuentosolipens=(descuentosolipens/30)*this.diastrabajadosmes;
									this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									if (banderaNombre==0)
									{
										this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
									//	banderaNombre+=1;
									}
									else
									{
										this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
									}
								}
								sumadescontado+=descuentosolipens;
							}
						}
					}
			
					if (lbtipopago.Text=="MENSUAL")
					{
						salariocompletomes=acumuladoeps;
                        if (tiposalario == "1")
                        {
                            if (Math.Round(salariocompletomes * double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG) * 0.01) > topesalmaximos)
                                salariocompletomes = topesalmaximos;
                        }
                        else
                        {
                            if (salariocompletomes > topesalmaximos)
                                salariocompletomes = topesalmaximos;

                        }
						if (salariocompletomes>topesalminimos)
						{
                            //ingreso el descuento si es integral sobre el 70%
                            double baseSalario = Math.Round(Convert.ToDouble(salariocompletomes/double.Parse(o_CNomina.CNOM_SALAMINIACTU)),3);
							porcentaje=DBFunctions.SingleData("select COALESCE(pfon_porcentaje,0) from dbxschema.pfondosolipens where "+ baseSalario + " between pfon_intinicio and (pfon_intfinal - 0.00001)");
                            if (tiposalario == "1")  // SALARIO INTEGRAL
                            {
                                baseint = (salariocompletomes * double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)) / 100;
                                baseSalario = Math.Round(Convert.ToDouble(baseint / double.Parse(o_CNomina.CNOM_SALAMINIACTU)),3);
                                porcentaje = DBFunctions.SingleData("select COALESCE(pfon_porcentaje,0) from dbxschema.pfondosolipens where " + baseSalario + " between pfon_intinicio and (pfon_intfinal - 0.00001)");
                                descuentosolipens = (baseint * double.Parse(porcentaje) * 0.01);
                            }
                            else
                                {
                                    descuentosolipens = (salariocompletomes * double.Parse(porcentaje) * 0.01);
                                }
                            double soliDesc = Convert.ToDouble(DBFunctions.SingleData("SELECT COALESCE(SUM(DQUI_ADESCONTAR - DQUI_APAGAR),0) FROM DQUINCENA D, MQUINCENAS M, CNOMINA CN WHERE MQUI_ANOQUIN = CN.CNOM_ANO AND MQUI_MESQUIN = CN.CNOM_MES AND M.MQUI_CODIQUIN = D.MQUI_CODIQUIN AND D.MEMP_CODIEMPL = '"+ codigoempleado + "' AND D.PCON_CONCEPTO = CN.CNOM_CONCFONDSOLIPENS"));
                            //     descuentosolipens = Math.Round(descuentosolipens, 0) - Convert.ToDouble(valepsquinanterior.Tables[0].Rows[0][0].ToString()); 
                            fondosolidaridadpensional = Math.Round(descuentosolipens - soliDesc, 0);
                            descuentosolipens = Math.Round(descuentosolipens - soliDesc, 0);


                            if (porcentaje!="0")
							{
							//	descuentosolipens=(descuentosolipens/30)*this.diastrabajadosmes;
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDSOLIPENS,1,double.Parse(descuentosolipens.ToString("N")),0,double.Parse(descuentosolipens.ToString("N")),"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
								}							
							}
					
							sumadescontado+=descuentosolipens;
    					}	
					}
				}
			}
		}
		
				
		protected void insertarretefuente(string codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,double acumuladoeps,string tiposalario)
		{
            int     quince  = Int32.Parse(DDLQUIN.SelectedValue);
            string  tipoPago = lbtipopago.Text;
            string  descripcionconcepto = o_CNomina.CNOM_CONCRFTEDESC;

            string unidUVT  = o_CNomina.CNOM_UNIDVALOTRIB;      //Sacar la Unidad de Valor Tributario
            if (unidUVT == "" || unidUVT == "0")
                return;  // SI la UVT esta vacia o es cero no calcula retencion en la fuente
       
            //                                                                   fech fech                                                                                                                  1 0 2   MENSUAL          
            //                                                                   ini  fin  Nro qna                                                                                                          period  QUINCENAL
            double valorReteFuente = o_Varios.calcular_ReteFuente(codigoempleado, lb, lb2, codquincena, sueldo, docref, lbmas1, apellido1, apellido2, nombre1, nombre2, acumuladoeps, tiposalario, mensaje, quince, tipoPago, valorfondopensionempleado, fondosolidaridadpensional, valorepsempleado, valorepsquinanteriorA, valorfondopensionvoluntaria, unidUVT, o_CNomina);

            //if (mensaje != "")
            //{
            //    Utils.MostrarAlerta(Response, "" + mensaje + "");
            //}
	
            if (valorReteFuente != 0)
            {
                this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, o_CNomina.CNOM_CONCRFTECODI, 1, valorReteFuente, 0, valorReteFuente, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                this.ingresar_datos_datatable(codquincena, codigoempleado, o_CNomina.CNOM_CONCRFTECODI, 1, valorReteFuente, 0, valorReteFuente, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                sumadescontado += valorReteFuente;
            }
	        /*
            //Averiguar los pagos extemporaneos (fuera de nomina) que tiene el empleado.
			double pagado=0;
			DataSet pagosfnomina= new DataSet();
			 
			DBFunctions.Request(pagosfnomina,IncludeSchema.NO,"select M.pcon_concepto,M.MPAGO_valrtotl,M.memp_codiempl,P.pcon_signoliq,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc from dbxschema.MPAGOSFUERANOMINA M,dbxschema.pconceptonomina P,dbxschema.tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='N' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mpago_fecha between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
			int j;
			//Sacar la Unidad de Valor Tributario
		    string unidT = o_CNomina.CNOM_UNIDVALOTRIB;  

			if (pagosfnomina.Tables[0].Rows.Count!=0)
			{
				for(j=0;j<pagosfnomina.Tables[0].Rows.Count;j++)
				{
					if (pagosfnomina.Tables[0].Rows[j][1].ToString() + "" == "")
					{
						Response.Write("<script language:javascript>alert('Por favor ingrese un valor para los Pagos extemporaneos del empleado "+codigoempleado+"  ');</script>");
					}
					else
					{
						pagado+=double.Parse(pagosfnomina.Tables[0].Rows[j][1].ToString());
					}
		        }
				pagado=Math.Round(pagado,0);
			}
			
			DataSet estadorfte = new DataSet();
			DataSet valrftequinanterior = new DataSet();
			 
			int     numquincenaanterior=0;
            double  retefuenteDescontada = 0;

            DBFunctions.Request(estadorfte, IncludeSchema.NO, "select memp_testrete, coalesce(memp_porcrete,0), tcon_contrato, tSAL_SALARIO from dbxschema.mempleado where memp_codiempl='" + codigoempleado + "'");
			DataSet mquincenas= new DataSet();
			DataSet primertope= new DataSet();
           
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			
			DBFunctions.Request(primertope,IncludeSchema.NO, "select pret_intfinal from pretefuente ");
         
            if (primertope.Tables[0].Rows.Count == 0 )
			{
				Response.Write("<script language:javascript>alert('Por favor Ingrese la Tabla de Retención en la Fuente');</script>");
			}
			else
			{
				string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+o_CNomina.CNOM_CONCRFTECODI+"' ");
                string retencion = o_CNomina.CNOM_CONCRFTECODI;
                int quince=Int32.Parse(DDLQUIN.SelectedValue);
                if (lbtipopago.Text=="QUINCENAL" && quince == 2)
				{
					numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT coalesce(MQUI.mqui_codiquin,0) from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=1 and mqui_estado = 2"));
				}
               			
				DataSet descuentos = new DataSet();

                DBFunctions.Request(descuentos, IncludeSchema.NO, "select coalesce(memp_vrexcesalud,0),coalesce(memp_vrexceeduc,0),coalesce(memp_vrexceafc,0),coalesce(memp_vrexcevivi,0), coalesce(memp_perscargo,0) from  dbxschema.mempleado where memp_codiempl='" + codigoempleado + "'  ");		
			
				double acumuladoretefuente2=0,valoradescontar2=0,retefuentemensual=0;
                try
                {
                    retefuenteDescontada = Convert.ToInt32(DBFunctions.SingleData("SELECT coalesce(dqui_adescontar,0) from dbxschema.dquincena MQUI wHERE mqui_codiquin=" + numquincenaanterior + " and memp_codiempl='" + o_CNomina.CNOM_MES + "' and pcon_concepto = '" + codigoempleado + "' "));
                }
                catch
                {
                    retefuenteDescontada = 0;
                }

                if (o_CNomina.CNOM_RETEFETEPERINOMI == "1")
				{
					bandera+=1;	
					if (bandera==1)
						Response.Write("<script language=javascript>alert('Apreciado Usuario el sistema solo calcula retención en la fuente para la segunda quincena, modifique este parámetro en la configuración de Nómina');</script>");
			    }
                else if (o_CNomina.CNOM_RETEFETEPERINOMI == "3")
				{
					bandera+=1;	
					if (bandera==1)
						Response.Write("<script language=javascript>alert('Apreciado Usuario el sistema solo calcula retención en la fuente para la segunda quincena, modifique este parámetro en la configuración de Nómina');</script>");
				}

                else if (o_CNomina.CNOM_RETEFETEPERINOMI == "2")
				{
				
					if (DDLQUIN.SelectedValue=="2")
					{
						//validar si el empleado esta por porcentaje
						double deduc  = double.Parse(descuentos.Tables[0].Rows[0][2].ToString()); //afc
						double desto1 = double.Parse(descuentos.Tables[0].Rows[0][0].ToString()); //salud
						double desto2 = double.Parse(descuentos.Tables[0].Rows[0][1].ToString()); //educacion
						double desto3 = double.Parse(descuentos.Tables[0].Rows[0][3].ToString()); //vivienda
						double desto4 = desto1 + desto2 + desto3; //salud+educacion+vivienda
                        double personasAcargo = double.Parse(descuentos.Tables[0].Rows[0][4].ToString()); //personas a cargo

                        if((desto1 + desto2) > 0 && desto3 > 0)
                            Response.Write("<script language:java>alert('Error en retenciones del empleado '" + codigoempleado + "', solo puede aplicar descuentos por vivienda o por salud y-o educacion);</script>");
                        
						string query = @"SELECT coalesce(SUM(DQUI.DQUI_APAGAR),0) FROM DBXSCHEMA.DQUINCENA DQUI,"+
										" DBXSCHEMA.PCONCEPTONOMINA PCON WHERE MQUI_CODIQUIN=" +numquincenaanterior+" AND "+
                                        " MEMP_CODIEMPL='"+codigoempleado+"' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO "+
                                        " AND PCON.tres_afecretefte='S'";
						string quinanterior=DBFunctions.SingleData(query);
						retefuentemensual+=Double.Parse(quinanterior==string.Empty?"0":quinanterior);

                        string queryD = @"SELECT coalesce(SUM(DQUI.DQUI_ADESCONTAR),0)FROM DBXSCHEMA.DQUINCENA DQUI,"+
										" DBXSCHEMA.PCONCEPTONOMINA PCON WHERE MQUI_CODIQUIN=" + numquincenaanterior +" AND "+
                                        " MEMP_CODIEMPL='" + codigoempleado +"' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO "+
										" AND PCON.tres_afecretefte='S'";
                        double descquinanterior = Convert.ToDouble(DBFunctions.SingleData(queryD));
                             //   PARECE QUE YA NO SE netea
                       // if (estadorfte.Tables[0].Rows[0][3].ToString() == "1") // Salario Integral
                       //     retefuentemensual += acumuladoeps - sueldo + sueldo*0.7 - descquinanterior;
                       // else
                             
                            retefuentemensual += acumuladoeps - descquinanterior;
                           
                        if (estadorfte.Tables[0].Rows[0][2].ToString() == "4") // pensionado
                            valorfondopensionempleado = 0;
                        
                        // 1. pagos brutos al empleado
                        // 2. menos: ingresos del mes no gravables para el trabajor
                        double ingresosNogravables = 0;  // ver como se selecciona esa informacion y acumularla aqui

                        // 3. menos: deducciones

                        // AFC  =  Ahorro para el Fomento de la Construcción
                        if (deduc > (30 / 100) * retefuentemensual)  // maximo el 30% de ingreso bruto
                            deduc = (30 / 100) * retefuentemensual;
                        if (deduc > (3800 / 12) * Convert.ToDouble(unidT))  // maximo 3.800 uvs anuales en meses
                            deduc = (3800 / 12) * Convert.ToDouble(unidT);
                        // vivienda
                        if (desto3 > 100 * Convert.ToDouble(unidT))
                            desto3 = 100 * Convert.ToDouble(unidT);
                        // medicina prepagada
                        if (desto1 > 16 * Convert.ToDouble(unidT))
                            desto1 = 16 * Convert.ToDouble(unidT);
                        // personas a cargo
                        double desto5 = 0;
                        if (personasAcargo > 0)
                        {
                            if (retefuentemensual * 0.10 > 32 * Convert.ToDouble(unidT))
                                desto5 = 32 * Convert.ToDouble(unidT);
                            else
                                desto5 = Math.Round(retefuentemensual * 0.10, 0);
                        }
                        // aportes obligatorios a salud del año gravable anterior
                        // se toma el pago a la eps del mes presente, porque la del año anterior no es segura si el empleado no trabajo en la empresa todo el tiempo
                        //  el acumuladoEPS ya lo tiene descontado de las base 
                       
                        // double deductotal = valorfondopensionempleado + fondosolidaridadpensional + valorepsempleado + deduc + descquinanterior + acumuladoretefuenteQnaAct ;
                        //                  vivienda medici  pnasCgo     salud
                        double deducciones = desto3 + desto1 + desto5 + valorepsempleado + valorepsquinanteriorA;

                        // 4. Rentas Exentas
                        //                    afc         pension
                        double rentaExenta = deduc + valorfondopensionempleado + fondosolidaridadpensional + descquinanterior ;
                        double base25   = retefuentemensual - deducciones - rentaExenta;
                        base25 = base25 * 0.25;   // 25% del deducible
                        if (base25 > 240 * Convert.ToDouble(unidT))
                            base25 = 240 * Convert.ToDouble(unidT);

                        double deductotal = deducciones + rentaExenta + base25;
                      
                        acumuladoretefuente2 = retefuentemensual - deductotal;  // base para cálculo de retención
						                                 
                        ///    REGIMEN ORDINARIO
                        /// procedimiento 1   -    se trabaja con las uvt en la tabla  R
                        double valorRetencionOrdinaria = calcular_reteFuente(estadorfte, "R", unidT, acumuladoretefuente2, codigoempleado);

                        ///    REGIMEN IMAN = Impuesto Mínimo Alternativo Nacional
                        /// procedimiento 1   -    se trabaja con las uvt en la tabla  I
                        acumuladoretefuente2 =  acumuladoeps - descquinanterior - valorfondopensionempleado - fondosolidaridadpensional - valorepsempleado ;
                        double valorRetencionIman = calcular_reteFuente(estadorfte, "I", unidT, acumuladoretefuente2, codigoempleado); 

                        if (valorRetencionOrdinaria > valorRetencionIman)
                            valoradescontar2 = valorRetencionOrdinaria;
                        else
                            valoradescontar2 = valorRetencionIman;

                        valoradescontar2     = valoradescontar2 - retefuenteDescontada;
						valoradescontar2     = Math.Round(valoradescontar2/1000,0)*1000;
						if (valoradescontar2 !=0)
						{
							this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCRFTECODI,1,valoradescontar2,0,valoradescontar2,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto); 
							this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCRFTECODI,1,valoradescontar2,0,valoradescontar2,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto); 
							sumadescontado+=valoradescontar2;
						}
					}
				}	
			}	
		}

        protected double calcular_reteFuente(DataSet estadorfte, string tipoRegimen, string unidT, double acumuladoretefuente2, string codigoempleado)
        {
            DataSet valoradescontar = new DataSet();
            double valoradescontar2 = 0;
            if (estadorfte.Tables[0].Rows[0][0].ToString() == "1" || tipoRegimen == "I")
            {
                double uvt = Double.Parse(unidT);

                if (uvt < acumuladoretefuente2)
                {
                    double valorBaseUVT = 0;
                    if(tipoRegimen == "I")
                        valorBaseUVT = (acumuladoretefuente2 / uvt);
                    else
                        valorBaseUVT = acumuladoretefuente2 / uvt;

                    DBFunctions.Request(valoradescontar, IncludeSchema.NO, "select pret_intinicio,pret_tarimarginal,pret_sumafinal from dbxschema.pretefuente where  (" + valorBaseUVT.ToString() + " between pret_intinicio +0.01 and pret_intfinal) and ttip_retefuen = '"+ tipoRegimen +"' ");
                    if (valoradescontar.Tables[0].Rows.Count > 0)
                    {
                        if (tipoRegimen == "I")
                            valoradescontar2 = double.Parse(valoradescontar.Tables[0].Rows[0][1].ToString()) ;
                        else
                            valoradescontar2 = ((valorBaseUVT - double.Parse(valoradescontar.Tables[0].Rows[0][0].ToString())) * double.Parse(valoradescontar.Tables[0].Rows[0][1].ToString()) / 100) + double.Parse(valoradescontar.Tables[0].Rows[0][2].ToString());
                        valoradescontar2 = valoradescontar2 * uvt;
                    }
                    else
                    {
                        valoradescontar2 = 0;
                        Response.Write("<script language:javascript>alert('No HA definido los intervalos de Retención en la Fuente REGIMEN ORDINARIO para el empleado " + codigoempleado + "  ');</script>");
                    }
                }
                else
                {
                    valoradescontar2 = 0;
                }
            }
            else
            {
                // procedimiento 2
                // es decir se trabaja con porcentaje calculado para el semestre
                if (estadorfte.Tables[0].Rows[0][0].ToString() == "2")
                {
                    if (estadorfte.Tables[0].Rows[0][1].ToString() == null)
                    {
                        valoradescontar2 = 0;
                        Response.Write("<script language:javascript>alert('Al empleado " + codigoempleado + " le falta definirle el % de retención, se asume 0  !!! Corrijalo ');</script>");
                    }
                    else valoradescontar2 = acumuladoretefuente2 * double.Parse(estadorfte.Tables[0].Rows[0][1].ToString()) / 100;
                }
                else
                {
                    valoradescontar2 = 0;
                }
            }
            return valoradescontar2;
        */
        }
			

		protected void insertarsaldos(string  codigoempleado, double sumapagado,double sumadescontado,double acumuladoeps,string codquincena,double diasexceptosauxtransp,double sumapagadamasauxt, string nombre1, string nombre2, string apellido1, string apellido2)
		{
			double saldo;
			saldo=sumapagadamasauxt-sumadescontado;
			
			if (saldo!=0)
			 {
				this.ingresar_datos_datatable("0", codigoempleado, "--",0,0,Math.Round(sumapagadamasauxt,0),Math.Round(sumadescontado,0),"SALDO",Math.Round(saldo,0),"--", apellido1, apellido2, nombre1, nombre2, "LIQUIDACION");
                sqlStrings.Add("insert into dpagaafeceps values (default,"+codquincena+",'"+codigoempleado+"',"+acumuladoretefuente+","+acumuladoprima+","+acumuladovacaciones+","+acumuladoprovisiones+","+acumuladoliqdefinitiva+","+acumuladohorasextras+","+acumuladocesantia+","+acumuladoeps+","+diasexceptosauxtransp+")");					
			 }
		}
	
		protected void ajuste_minimo(int i_emp, string codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string diasexceptosauxtransp,string periodosubt,string tiposalario) 
		{
			if (DDLQUIN.SelectedValue.ToString() == "1") return;  // el ajuste al sueldo solo se aplica en la 2da qna
			string conceptoajuste = "";;
			DataSet afectaciones= new DataSet();	
			double pagadomes=0;	
			//Averiguar cuanto se le pago en el periodo si tuvo vacaciones.
			string pagadovacaciones=DBFunctions.SingleData("select  sum(dqui_apagar) from dbxschema.dquincena where mqui_codiquin="+codquincena+" and memp_codiempl='"+codigoempleado+"'");

            string salaminiactu = o_CNomina.CNOM_SALAMINIACTU;
			double valordia=double.Parse(salaminiactu)/30;
            int diastrabajados = diastrabajadosTotalmes - int.Parse(diasexceptosauxtransp);
            //diassueldopagado = numerodias;
            double valorproporcionalminimo=valordia*diastrabajados;
            if(empleados.Tables[0].Select("MEMP_CODIEMPL = '" + codigoempleado + "' AND TCON_CONTRATO = '5'").Length > 0) // SENA ETAPA LECTIVA se la pago el 50% del minimo
                valorproporcionalminimo = Math.Round(valorproporcionalminimo *0.5,0);

            conceptoajuste = o_CNomina.CNOM_CONCAJUSUELDO.ToString();       //,IncludeSchema.NO,"select cnom_concajusueldo from dbxschema.cnomina");
            string descripcionconcepto = o_CNomina.CNOM_CONCAJUSUELDODESC.ToString();  //DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+conceptoajuste.Tables[0].Rows[0][0].ToString()+"' ");
            pagadomes = sumapagado + pagosquincenaant;

			DBFunctions.Request(afectaciones,IncludeSchema.NO,"select M.cnom_concajusueldo,P.pcon_signoliq,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.cnomina M,dbxschema.pconceptonomina P where M.cnom_concajusueldo=P.pcon_concepto");			
			string valido;
			if (pagadovacaciones!=String.Empty)
			{
				pagadomes+=double.Parse(pagadovacaciones.ToString());	
				
			}
			
			//validar que no se gane menos del minimo proporcionalmente
            if ((acumuladoeps + pagosquincenaant) < valorproporcionalminimo)
			{
				//Mirar que tenga permitido el ajuste de sueldo
				valido=DBFunctions.SingleData("select memp_ajusueldo from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
				
				if (valido=="1")
				{
                    double ajuste = valorproporcionalminimo - acumuladoeps - pagosquincenaant;
					ajuste=Math.Round(ajuste,0);
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE" ,0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][2].ToString(),"Novedad de Descuento Permanente que afecta EPS? posible error..",double.Parse(ajuste.ToString("N")),0,0,0,0,0,0,0);
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][3].ToString(),"Novedad de descuento Permanente que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(ajuste.ToString("N")));
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][4].ToString(),"Novedad de descuento Permanente que afecta Primas? posible error..",0,0,double.Parse(ajuste.ToString("N")),0,0,0,0,0);											
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][5].ToString(),"Novedad de descuento Permanente que afecta Vacaciones? posible error..",0,0,0,double.Parse(ajuste.ToString("N")),0,0,0,0);
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][6].ToString(),"Novedad de descuento Permanente que afecta Cesantias? posible error..",0,double.Parse(ajuste.ToString("N")),0,0,0,0,0,0);
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][7].ToString(),"Novedad de descuento Permanente que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(ajuste.ToString("N")),0,0,0);
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][8].ToString(),"Novedad de descuento Permanente que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(ajuste.ToString("N")),0,0);
					this.validacionafectaciones(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),afectaciones.Tables[0].Rows[0][9].ToString(),"Novedad de descuento Permanente que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(ajuste.ToString("N")),0);
					if (ajuste != 0)
		            {
					    this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
					    if (banderaNombre==0)
					    {
						    //this.ingresar_datos_datatable(codquincena,codigoempleado,conceptoajuste.Tables[0].Rows[0][0].ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						    //this.validaciondebitaacredita(codquincena,codigoempleado,conceptoajuste.Tables[0].Rows[0][0].ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectaciones.Tables[0].Rows[0][1].ToString(),descripcionconcepto);
    									
						//    banderaNombre+=1;
					    //}
					    //else
					    //{
						    this.ingresar_datos_datatable(codquincena, codigoempleado, conceptoajuste.ToString(),1,double.Parse(ajuste.ToString("N")),double.Parse(ajuste.ToString("N")),0,"PESOS M/CTE",0,docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
					    }
    				    sumapagado+=ajuste;
					    sumapagadamasauxt+=ajuste;
                    }
				}
			}
	    }

		protected void liquidar_subsidiodetransporte(int i_emp, string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string diasexceptosauxtransp,string periodosubt,string tiposalario) 
		{
		//Periodo de Pago en el cual se cancela el subsidio de Transporte - se toma de CNOMINA.CNOM_SUBSTRANSPERINO
			idiasexceptosauxtransp = Int32.Parse(this.diasexceptosauxtransp.ToString());						
		//	idiasexceptosauxtransp = idiasexceptosauxtransp + diassueldopagado; // arreglo hector
			
			string SubsidioTransPagado = "select coalesce(sum(dqui_canteventos),0) from dbxschema.mquincenas m, dquincena d, cnomina cn where d.memp_codiempl = '"+codigoempleado+@"' 
                                          and m.mqui_codiquin = d.mqui_codiquin and m.mqui_anoquin = cn.cnom_ano and m.mqui_mesquin = cn.cnom_mes  
                                          and d.pcon_concepto = cn.cnom_concsubtcodi ; ";
            //      LINEA ANTERIOS A LA ULTIMA                             and m.mqui_tpernomi = cn.cnom_quincena 
  
            int diasSubsTransPagados = Convert.ToInt32 (DBFunctions.SingleData(SubsidioTransPagado));
		
			DataSet mquincenas  = new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			//DataSet cnomina= new DataSet();
			//DataSet cnomina2= new DataSet();
			DateTime fechaquin  = new DateTime();
			DataSet  valorepsprimeraquinanterior= new DataSet();
			DataSet  valorepssegundaquinanterior= new DataSet();
			//DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,cnom_substransactu,cnom_topepagoauxitran,cnom_salaminiactu from dbxschema.cnomina");
            string periaplica = o_CNomina.CNOM_SUBSTRANPERINOMI;// DBFunctions.SingleData("select CNOM_SUBSTRANPERINOMI from dbxschema.cnomina");
			//DBFunctions.Request(cnomina2,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,cnom_substransactu,cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");												 
			string fpagoEmpleado=DBFunctions.SingleData("select memp_peripago from dbxschema.mempleado A, dbxschema.tperipago B where A.memp_peripago=B.tper_peri and memp_codiempl='"+codigoempleado+"' ");
			//JFSC 14/02/2008
			string tiposubsidio =DBFunctions.SingleData("select tsub_codigo from dbxschema.mempleado A, dbxschema.tperipago B where A.memp_peripago=B.tper_peri and memp_codiempl='"+codigoempleado+"' ");
			double valortope    =double.Parse(o_CNomina.CNOM_TOPEPAGOAUXITRAN)* double.Parse(o_CNomina.CNOM_SALAMINIACTU);
            string descripcionconcepto = o_CNomina.CNOM_CONCSUBTDESC;
         //  DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+o_CNomina.CNOM_CONCSUBTCODI+"' ");
			double valordiatransporte=(double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)/30);
			double valoradescontar=valordiatransporte*int.Parse(diasexceptosauxtransp.ToString());
			double valortotalmesanterior,valoradescontarpq,valoradescontarsq,valoradescontartotal;
		    double auxiliotransporte=0;
			int numquincenaanterior=0;
            int numquincenaactual=0;
            int diasqnaanter = 0;

			DataSet valepsquinanterior = null;
			//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
			//si es tipo de subsidio 3, nunca se paga
            valepsquinanterior = new DataSet();
            if (DDLQUIN.SelectedValue == "2")
            {
                //int numquincenaanterior=int.Parse(codquincena)-1;
                if (o_CNomina.CNOM_QUINCENA == "2")
                {
                    try
                    {
                        numquincenaanterior = Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + o_CNomina.CNOM_ANO + " and MQUI.mqui_mesquin=" + o_CNomina.CNOM_MES + " and MQUI.mqui_tpernomi=1 "));
                    }
                    catch
                    {
                        numquincenaanterior = 0;
                    }
                    numquincenaactual   = Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin=" + o_CNomina.CNOM_ANO + " and MQUI.mqui_mesquin=" + o_CNomina.CNOM_MES + " and MQUI.mqui_tpernomi=2 "));
                }
            }
				
            string query1 = @"select coalesce(sum(dqui_apagar - dqui_adescontar),0) 
										from dbxschema.dquincena d, dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '" + codigoempleado + @"' and 
											d.mqui_codiquin in ( "+ numquincenaanterior + @" , "+ numquincenaactual + @") and
											p.tres_afec_eps='S'";
            DBFunctions.Request(valepsquinanterior, IncludeSchema.NO, query1);
            
            if (tiposubsidio == "3")
			{
				auxiliotransporte=0;
                sumapagadamasauxt = sumapagado;
				//this.diastrabajadosmes=this.diastrabajados;
				this.diastrabajadosmes=this.diastrabajados;
                //if (DDLQUIN.SelectedValue == "2" && valepsquinanterior.Tables[0].Rows.Count > 0)
                //{
                     acumuladoeps += double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString());
                //}
				return;
			}

            acumuladoeps += double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString()); // siempre acumula la qna anterior al acumualdoeps

            if ((diasvaca + diassueldopagado) >= 30 && lbtipopago.Text == "MENSUAL") return;
            if ((diasvaca + diassueldopagado) >= 15 && lbtipopago.Text == "QUINCENAL" && tipoPeriodoPago == "1") return;

			switch (periaplica)
			{
				case "1" : 	//         1 = Ambas Qnas sobre Acumulado Mes Actual,  
					if (DDLQUIN.SelectedValue=="1")
					{
						//si al empleado se le paga el auxilio legal o subsidio igual.
						if (periodosubt=="1"|| periodosubt=="2" )
						{
							//mirar que no sobrepase el tope
							if (acumuladoeps<=valortope / 2)
							{
								//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
								//JFSC TODO POS 7 ¿Es necesario utilizar transactu2?
								//auxiliotransporte=double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)-valoradescontar;
                                auxiliotransporte = (double.Parse(o_CNomina.CNOM_SUBSTRANSACTU))/30 * diastrabajados - valoradescontar;
                                //auxiliotransporte = (double.Parse(o_CNomina.CNOM_SUBSTRANSACTU))/2-valoradescontar;
                    //            auxiliotransporte = (diastrabajados + diasvaca - idiasexceptosauxtransp - diasSubsTransPagados) * valordiatransporte;
                                diasSubsTransPagados = (diastrabajados - idiasexceptosauxtransp - diasSubsTransPagados);
                			}
						}
					}
					//para segunda quincena
					if (DDLQUIN.SelectedValue=="2")
					{
						//int numquincenaanterior=int.Parse(codquincena)-1;
						if (o_CNomina.CNOM_QUINCENA=="2")
						{
							numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=1 "));
						}
						else
						{
							fechaquin=Convert.ToDateTime(lb);
			//				fechaquin= fechaquin.AddMonths(-1);
							numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "));
						}			    	
						//DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
						//si no tiene valor de eps anterior , es mensual.
						if (valepsquinanterior.Tables[0].Rows.Count==0)
						{
							//mensual
							if (fpagoEmpleado=="2" )
							{
								auxiliotransporte = (double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)) / 2-valoradescontar;
                                auxiliotransporte = (this.diastrabajados - idiasexceptosauxtransp) * valordiatransporte; //descuenta incapacidades
                                diasSubsTransPagados = (diastrabajados - idiasexceptosauxtransp - diasSubsTransPagados);

                            }
						}
						else
						{
							if (periodosubt=="1" || periodosubt=="2" )
							{
								//mirar que no sobrepase el tope sumando salario 1q+2q(actual)
						//		acumuladoeps += double.Parse(valepsquinanterior.Tables[0].Rows[0][0].ToString()); ya esta acumulado
								if (acumuladoeps<=valortope)
								{
									//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
									//JFSC TODO POS 7... ¿Debe ser transactu2?
									auxiliotransporte = (double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)) / 2-valoradescontar; // hector verificar
                                    auxiliotransporte = (this.diastrabajados - idiasexceptosauxtransp) * valordiatransporte; //descuenta incapacidades
                                    diasSubsTransPagados = (diastrabajados - idiasexceptosauxtransp - diasSubsTransPagados);

                                }
							}	
						}
					}

//					if (acumuladoeps<=valortope)
//					{
//						auxiliotransporte=(double.Parse(cnomina.Tables[0].Rows[0][7].ToString())-valoradescontar) /2;
//					}
					break;
				case "2" :  //         2 = Segunda Qna sobre Acumulado Mes Actual,
					numquincenaanterior=0;
				/*	if (codquincena=="")
					{
						numquincenaanterior=0;
					}
					else
					{  */
						//numquincenaanterior=int.Parse(codquincena)-1;
						if (o_CNomina.CNOM_QUINCENA=="2")
						{
						    fechaquin=Convert.ToDateTime(lb);	
							if (lbtipopago.Text=="QUINCENAL")
							{
								numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=1 "));
							}
							else
							{
								//fechaquin= fechaquin.AddMonths(-1);
								numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
							}
						}
						else
						{
							fechaquin=Convert.ToDateTime(lb);
							fechaquin= fechaquin.AddMonths(-1);
							try
							{
								numquincenaanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
							}
							catch
							{
								numquincenaanterior=-10;
							}
						}
					//}
    //      calculo de los dias trabajados en la primera quincena
					DataSet diastrabajadosqnaanterior = new DataSet();
					string querydt= @"select coalesce(sum(dqui_canteventos),0) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p, cnomina c
							    		where d.pcon_concepto=p.pcon_concepto and 
											d.memp_codiempl= '" + codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanterior+@" and
											(p.pcon_concepto=c.cnom_concsalacodi or p.pcon_concepto=c.cnom_concsalasena) ";
                    string querydnoTrab = @"select coalesce(sum(dqui_canteventos),0) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p, cnomina c
							    		where d.pcon_concepto=p.pcon_concepto and  
											d.memp_codiempl= '" + codigoempleado + @"' and 
											d.mqui_codiquin=" + numquincenaanterior + @" and pcon_cLASECONC = 'L' AND D.DQUI_ADESCONTAR > 0";
					 
                    diasqnaanter = Convert.ToInt32(DBFunctions.SingleData(querydt));
         //           diasqnaanter -= Convert.ToInt32(DBFunctions.SingleData(querydnoTrab)); 
                    if (lbtipopago.Text == "MENSUAL") diasqnaanter = 0;
					valepsquinanterior = new DataSet();
					string query=@"select sum(dqui_apagar) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanterior+@" and
											p.tres_afec_eps='S'";
					DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,query);
					//DBFunctions.Request(valepsquinanterior,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanterior+"");
					valorPagadoAnt=Convert.ToDouble(valepsquinanterior.Tables[0].Rows[0][0].ToString()==string.Empty?"0":valepsquinanterior.Tables[0].Rows[0][0].ToString());
					sumapagadamasauxt=sumapagado;	
					//
					if (DDLQUIN.SelectedValue=="2")
					{
						int mesAnt,anioAnt,perAnt;
						string auxm,auxa,auxp;
						DataSet quinAnt=new DataSet();
						query="select * from dbxschema.mquincenas where mqui_codiquin="+numquincenaanterior;
						DBFunctions.Request(quinAnt,IncludeSchema.NO,query);
					//	if (quinAnt.Tables[0].Rows.Count ==0)
					//	{
					//		Tools.General.mostrarMensaje("Falta quincena Anterior "+numquincenaanterior);
					//		return;
					//	}
						auxm   = quinAnt.Tables[0].Rows[0]["mqui_mesquin"].ToString();
						auxa   = quinAnt.Tables[0].Rows[0]["mqui_anoquin"].ToString();
						auxp   = quinAnt.Tables[0].Rows[0]["mqui_tpernomi"].ToString();
						mesAnt = int.Parse(auxm==string.Empty?"0":auxm);
						anioAnt= int.Parse(auxa==string.Empty?"0":auxa);
						perAnt = int.Parse(auxp==string.Empty?"0":auxp);
						//si es quincenal y es segunda quincena
                        if (DDLQUIN.SelectedValue == "2" && fpagoEmpleado == "2")
						   {
                        //    valorPagadoAnt= 0;
                           }
						// VGCR ojo Hay que revisar los dias trabajados del mes 
						//if(DDLQUIN.SelectedValue=="2" && o_CNomina.CNOM_PAGOMENSTPER=="1")
						//  {
						//	idiasexceptosauxtransp+=this.calcularDiasVacacionesPeriodo(mesAnt,anioAnt,perAnt,codigoempleado,2);
						//  }
						if (valepsquinanterior.Tables[0].Rows.Count==0)
						{
							//necesito averiguar si el pago es mensual o quincenal
							if (lbtipopago.Text=="MENSUAL")
							{
								if (periodosubt=="1"|| periodosubt=="2" )
								{
									//mirar que no sobrepase el tope
									if (acumuladoeps<=valortope)
									{
										//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
										//valoradescontar+=valordiatransporte*double.Parse(valepsquinanterior.Tables[0].Rows[0][1].ToString());
										//auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][6].ToString())-valoradescontar;
										//Si es
										//diasexceptosauxtransp+=this.calcularDiasVacacionesPeriodo(mesv,anov,peripago,codigoempleado
										//if (idiasexceptosauxtransp >=4)
										//{
											auxiliotransporte=(30-idiasexceptosauxtransp-diasSubsTransPagados)*valordiatransporte;
											this.diastrabajados=(30-idiasexceptosauxtransp-diasSubsTransPagados);
                                            diasSubsTransPagados = (diastrabajados - idiasexceptosauxtransp - diasSubsTransPagados);

										//}
										//else
										//{
										//	auxiliotransporte=30*valordiatransporte;
										//	this.diastrabajados=30;
										//}
									}
								}								
							}
							else
							{
								if (periodosubt=="1" || periodosubt=="2" )
								{
									//mirar que no sobrepase el tope
							//		if ((acumuladoeps+valorPagadoAnt)<=valortope)
                                    if (acumuladoeps  <= valortope)
									{
										//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
										//valoradescontar+=valordiatransporte*double.Parse(valepsquinanterior.Tables[0].Rows[0][1].ToString());
										//auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][6].ToString())-valoradescontar;
										//if (diastrabajados==15)diastrabajados=30;
										//if (idiasexceptosauxtransp >=4 || empresa!="AYCO LTDA")
										if (idiasexceptosauxtransp >=4 )
										{
											if(this.esSuspension || this.esIncapacidad)
											{
												auxiliotransporte=(30-idiasexceptosauxtransp-diasSubsTransPagados)*valordiatransporte;
												this.diastrabajados=(30-idiasexceptosauxtransp-diasSubsTransPagados);
											}
											else
											{
												auxiliotransporte=this.diastrabajados*valordiatransporte;
											}
										}
										else
										{
											if(this.esSuspension)
											{
												auxiliotransporte=(30-idiasexceptosauxtransp-diasSubsTransPagados)*valordiatransporte;
												this.diastrabajados=(30-idiasexceptosauxtransp-diasSubsTransPagados);
											}
											else
											{
												this.diastrabajados=(30-idiasexceptosauxtransp-diasSubsTransPagados);
												auxiliotransporte=this.diastrabajados*valordiatransporte;											
												//this.diastrabajados=30;
											}
										}
									}
								}
							}
						}
						else
						{
							//si al empleado se le paga el auxilio legal o subsidio igual.
							if (periodosubt=="1" || periodosubt=="2")
							{
								//mirar que no sobrepase el tope
                             //   if ((acumuladoeps + valorPagadoAnt) <= valortope)
                                if (acumuladoeps  <= valortope)
								{
									//descontar si tiene dias que no fue al trabajo, dias que no se le paga aux de transp.
									//valoradescontar+=valordiatransporte*double.Parse(valepsquinanterior.Tables[0].Rows[0][1].ToString());
									//auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][6].ToString())-valoradescontar;
									//validar cuando sea para todo el mes o los dias trabajados
						//			if   (idiasexceptosauxtransp < 4 && empresa == "AYCO LTDA" && Convert.ToInt16(codigoempleado) < 461)
                        //                 idiasexceptosauxtransp = idiasexceptosauxtransp * -1;
                        //           else idiasexceptosauxtransp = 0;  // se toman los dias trabajados
                                   
						// hector	idiasexceptosauxtransp = 0; // hector...solo se paga transport para  venir a la empresa 
									if(this.diastrabajados<15)
									{
										//if (idiasexceptosauxtransp >=4 || empresa!="AYCO LTDA")
  //H     TENIA 15 COMO VARIABLE FIJA, SE CAMBIO POR diastrabajados 
										if (idiasexceptosauxtransp >=4)
										{
											if(this.esSuspension || this.esIncapacidad)
											{
                                                auxiliotransporte = (diastrabajados + diasvaca - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados) * valordiatransporte;
                                                this.diastrabajados = (diastrabajados + diasvaca - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados);
											}
											else
											{
												auxiliotransporte=this.diastrabajados*valordiatransporte;
										 		//auxiliotransporte=(diastrabajados+diasvaca-idiasexceptosauxtransp+diasqnaanter-diasSubsTransPagados)*valordiatransporte;
										 		//this.diastrabajados=(diastrabajados+diasvaca-idiasexceptosauxtransp+diasqnaanter-diasSubsTransPagados);
                                   		
                                            }
										}
										else
										{
											if(this.esSuspension)
											{
                                                auxiliotransporte = (diastrabajados + diasvaca - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados) * valordiatransporte;
                                                this.diastrabajados = (diastrabajados + diasvaca - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados);
											}
											else
											{
                                                auxiliotransporte = (this.diastrabajados + diasqnaanter - diasSubsTransPagados) * valordiatransporte;
                                           //   auxiliotransporte=(this.diastrabajados+diasqnaanter-diasSubsTransPagados)*valordiatransporte;
											}
											//this.diastrabajados=30;
										}
									}
									else
									{
                                        // diastrabajados cambie por 30
                                    //  auxiliotransporte = (diastrabajados + diasvaca - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados) * valordiatransporte;
                                        auxiliotransporte = (diastrabajados            - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados) * valordiatransporte;
                                     //    this.diastrabajados = (diastrabajados + diasvaca - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados);										
									}
                                    diasSubsTransPagados = (diastrabajados - idiasexceptosauxtransp + diasqnaanter - diasSubsTransPagados);
                                 //   auxiliotransporte=(diastrabajados + diasqnaanter)*valordiatransporte;
                               }
							}	
						}						
					}
					
//					if (acumuladoeps<=valortope)
//					{
//						if (DDLQUIN.SelectedValue=="2")
//						{ 
//							auxiliotransporte=double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)-valoradescontar;
//						}
//					}
					break;
				case "3" : //          3 = Ambas Qnas sobre Acumulado Mes Anterior,
					if (DDLQUIN.SelectedValue=="1")
					{
						//validar la primera q + la segunda del mes anterior con el tope
						//int numquincenaanteriorprimera=int.Parse(codquincena)-2;
						fechaquin=Convert.ToDateTime(lb);
						fechaquin= fechaquin.AddMonths(-1);
						int numquincenaanteriorprimera=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "));
						//int numquincenaanteriorsegunda=(int.Parse(codquincena)-1);
						int numquincenaanteriorsegunda=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
						DataSet valepsquinanteriorprimera= new DataSet();
						DataSet valepsquinanteriorsegunda= new DataSet();
						query1=@"select coalesce(sum(dqui_apagar),0) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanteriorprimera+@" and
											p.tres_afec_eps='S'";
						//DBFunctions.Request(valepsquinanteriorprimera,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorprimera+"");
						DBFunctions.Request(valepsquinanteriorprimera,IncludeSchema.NO,query1);
						string query2=@"select coalesce(sum(dqui_apagar),0) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanteriorsegunda+@" and
											p.tres_afec_eps='S'";
						//DBFunctions.Request(valepsquinanteriorsegunda,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorsegunda+"");	
						DBFunctions.Request(valepsquinanteriorsegunda,IncludeSchema.NO,query2);	
				
						if (valepsquinanteriorprimera.Tables[0].Rows.Count!=0)
						{
							//si al empleado se le paga el auxilio legal o subsidio igual.
							if (periodosubt=="1"|| periodosubt=="2" )
							{
								valortotalmesanterior=double.Parse(valepsquinanteriorprimera.Tables[0].Rows[0][0].ToString())+double.Parse(valepsquinanteriorsegunda.Tables[0].Rows[0][0].ToString());
								valoradescontarpq=valordiatransporte* double.Parse(valepsquinanteriorprimera.Tables[0].Rows[0][0].ToString()); // tenia 0,1
								valoradescontarsq=valordiatransporte* double.Parse(valepsquinanteriorsegunda.Tables[0].Rows[0][0].ToString()); // tenia 0,1
								//mirar que no sobrepase el tope
								if (valortotalmesanterior<=valortope)
								{
									// sacar las ausencias del mes pasado..
									//JFSC TODO POS 7... ¿Debe ser transactu2?
									auxiliotransporte=(double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)) / 2-valoradescontarpq;
								}
							}
						}
					}
					if (DDLQUIN.SelectedValue=="2")
					{
						//Response.Write("<script language:javascript>alert('entre al if de la 2da quincena. ');</script>");
						//int numquincenaanteriorsegunda2=(int.Parse(codquincena)-2);
						int numquincenaanteriorsegunda2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
						//int numquincenaanteriorprimera2=(int.Parse(codquincena)-3);
						int numquincenaanteriorprimera2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "));
						DataSet valepsquinanteriorprimera2= new DataSet();
						DataSet valepsquinanteriorsegunda2= new DataSet();
						query1=@"select sum(dqui_apagar) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanteriorprimera2+@" and
											p.tres_afec_eps='S'";
						DBFunctions.Request(valepsquinanteriorprimera2,IncludeSchema.NO,query1);
						string query2=@"select sum(dqui_apagar) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanteriorsegunda2+@" and
											p.tres_afec_eps='S'";
						DBFunctions.Request(valepsquinanteriorsegunda2,IncludeSchema.NO,query2);	
						//DBFunctions.Request(valepsquinanteriorprimera2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorprimera2+"");
						//DBFunctions.Request(valepsquinanteriorsegunda2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorsegunda2+"");	
						if (valepsquinanteriorsegunda2.Tables[0].Rows.Count==0 || valepsquinanteriorprimera2.Tables[0].Rows.Count!=0)
						{
							valortotalmesanterior=double.Parse(valepsquinanteriorprimera2.Tables[0].Rows[0][0].ToString())+double.Parse(valepsquinanteriorsegunda2.Tables[0].Rows[0][0].ToString());
							valoradescontarpq=valordiatransporte* double.Parse(valepsquinanteriorprimera2.Tables[0].Rows[0][1].ToString());
							valoradescontarsq=valordiatransporte* double.Parse(valepsquinanteriorsegunda2.Tables[0].Rows[0][1].ToString());
							if (periodosubt=="1"|| periodosubt=="2" )
								//mirar que no sobrepase el tope
							{
								if (valortotalmesanterior<=valortope)
								{
									if (valortotalmesanterior<=valortope)
									auxiliotransporte=(double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)) / 2-valoradescontarsq;
								}
							}
						}
					}
//					
//					if (acumuladomesant<=valortope)
//					{
//						auxiliotransporte=(double.Parse(cnomina.Tables[0].Rows[0][7].ToString())-valoradescontar) /2;
//					}
					break;
				case "4" : //          4 = Segunda Qna sobre Acumulado Mes Anterior,
					if (DDLQUIN.SelectedValue=="2")
					{
						//int numquincenaanteriorsegunda2=(int.Parse(codquincena)-2);
						int numquincenaanteriorsegunda2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
						//int numquincenaanteriorprimera2=(int.Parse(codquincena)-3);
						int numquincenaanteriorprimera2=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 "));
						DataSet valepsquinanteriorprimera2= new DataSet();
						DataSet valepsquinanteriorsegunda2= new DataSet();
						query1=@"select sum(dqui_apagar) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanteriorprimera2+@" and
											p.tres_afec_eps='S'";
						DBFunctions.Request(valepsquinanteriorprimera2,IncludeSchema.NO,query1);
						string query2=@"select sum(dqui_apagar) 
										from dbxschema.dquincena d,
											dbxschema.pconceptonomina p
										where d.pcon_concepto=p.pcon_concepto and
											d.memp_codiempl= '"+codigoempleado+@"' and 
											d.mqui_codiquin="+numquincenaanteriorsegunda2+@" and
											p.tres_afec_eps='S'";
						DBFunctions.Request(valepsquinanteriorsegunda2,IncludeSchema.NO,query2);	
						//DBFunctions.Request(valepsquinanteriorprimera2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorprimera2+"");
						//DBFunctions.Request(valepsquinanteriorsegunda2,IncludeSchema.NO,"select  distinct dpaga_afeps,dpaga_ausencias from dbxschema.dpagaafeceps D,dbxschema.mempleado M where D.memp_codiempl= '"+codigoempleado+"' and D.mqui_codiquin="+numquincenaanteriorsegunda2+"");	
						if (valepsquinanteriorsegunda2.Tables[0].Rows.Count==0 || valepsquinanteriorprimera2.Tables[0].Rows.Count!=0)
						{
							valortotalmesanterior=double.Parse(valepsquinanteriorprimera2.Tables[0].Rows[0][0].ToString())+double.Parse(valepsquinanteriorsegunda2.Tables[0].Rows[0][0].ToString());
							valoradescontarpq=valordiatransporte* double.Parse(valepsquinanteriorprimera2.Tables[0].Rows[0][1].ToString());
							valoradescontarsq=valordiatransporte* double.Parse(valepsquinanteriorsegunda2.Tables[0].Rows[0][1].ToString());
							valoradescontartotal=valoradescontarpq+valoradescontarsq;
							if (periodosubt=="1"|| periodosubt=="2" )
								//mirar que no sobrepase el tope
							{
								if (valortotalmesanterior<=valortope)
								{
									auxiliotransporte=double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)-valoradescontartotal;
								}
							}
						}
					}
//					
//					if (acumuladomesant<=valortope)
//					{
//						if (DDLQUIN.SelectedValue=="2")
//						{ 
//							auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][7].ToString())-valoradescontar;
//						}
//					}
					break;
				case "5" :  //         5 = Primera Qna sobre Acumulado Mes Actual,
					if (acumuladoeps<=valortope)
					{
						if (DDLQUIN.SelectedValue=="1")
						{ 
							//JFSC TODO pos 7 ¿Es necesario usar transactu2?
							auxiliotransporte=double.Parse(o_CNomina.CNOM_SUBSTRANSACTU)-valoradescontar;
						}
					}
					break;
				case "6" : //  		    6 = Primera Qna sobre Acumulado Mes Anterior
//					if (acumuladomesant<=valortope)
//					{
//						if (DDLQUIN.SelectedValue=="1")
//						{ 
//							auxiliotransporte=double.Parse(cnomina.Tables[0].Rows[0][7].ToString())-valoradescontar;
//						}
//					}
					break;
			}
		//	if (auxiliotransporte>0 && sueldo<valortope && (auxiliotransporte > 0 || sumapagado > 0) && (acumuladoeps+valorPagadoAnt) <= valortope)
            if (auxiliotransporte > 0 && sueldo < valortope && (auxiliotransporte > 0 || sumapagado > 0) && (acumuladoeps ) <= valortope)
                {
                ////    definir una variable unica para los dias de subsidio de tranpsorte independiente de los dias trabajados   se quito - idiasexceptosauxtransp de  this.diastrabajados
                //     int diastransporte = this.diastrabajados - idiasexceptosauxtransp;
                //     if((o_CNomina.CNOM_SUBSTRANPERINOMI == "2" || o_CNomina.CNOM_SUBSTRANPERINOMI == "4") && lbtipopago.Text=="QUINCENAL" && DDLQUIN.SelectedValue=="2")
                //         diastransporte += diasqnaanter;
                //     this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, diastransporte == 0 ? 1 : diastransporte, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                ////  this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, this.diastrabajados == 0 ? 1 : this.diastrabajados , double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                //if (banderaNombre == 0)
                //{
                //    this.ingresar_datos_datatable(codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, diastransporte == 0 ? 1 : diastransporte, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                //    banderaNombre+=1;
                //}
                //else
                //{
                //    this.ingresar_datos_datatable(codquincena, "", o_CNomina.CNOM_CONCSUBTCODI, diastransporte == 0 ? 1 : diastransporte, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, "", "", "", "", descripcionconcepto);
                //}
				//sumapagado+=auxiliotransporte;
				sumapagadamasauxt=sumapagado+auxiliotransporte;
                this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, diasSubsTransPagados == 0 ? 1 : diasSubsTransPagados, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                this.ingresar_datos_datatable(codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, diasSubsTransPagados == 0 ? 1 : diasSubsTransPagados, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                //   banderaNombre += 1;
		
				}
			else
			{
				sumapagadamasauxt=sumapagado;
			}

            //if ((acumuladoeps + valorPagadoAnt) < valortope && diasSubsTransPagados > 0)  // Se descuenta el subsidio transporte si ya se había pagado días en el mes
           // if ((acumuladoeps) < valortope && diasSubsTransPagados > 0)
           //     {
           //     auxiliotransporte = diasSubsTransPagados * valordiatransporte;
           //     this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, diasSubsTransPagados == 0 ? 1 : diasSubsTransPagados, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
           //     this.ingresar_datos_datatable        (codquincena, codigoempleado, o_CNomina.CNOM_CONCSUBTCODI, diasSubsTransPagados == 0 ? 1 : diasSubsTransPagados, double.Parse(auxiliotransporte.ToString("N")), double.Parse(auxiliotransporte.ToString("N")), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
           //      banderaNombre += 1;
			//     }   
			//this.diastrabajadosmes=this.diastrabajados;
			this.diastrabajadosmes=30-idiasexceptosauxtransp-diasSubsTransPagados;
		}

				
		protected void actuliza_prestamoempledos(int i_emp, string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps) 
		{
            /*
            if ((diasvaca + diassueldopagado) >= 30 && lbtipopago.Text == "MENSUAL") return;
            if ((diasvaca + diassueldopagado) >= 15 && lbtipopago.Text == "QUINCENAL" && tipoPeriodoPago == "1") return;
            */
			DataSet mquincenas= new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			double valorcuota,intereses,capital,saldo=0,parte2,cuotamasinteres=0,saldoporpagar=0;
			DataSet prestamoempledos= new DataSet();
			int i,j;
	//		string descripcionconcepto="";
						
			// traigo los prestamos para el empleado que afecten al periodo de pago elegido y que esten  activos,descuento primera o ambas quincenas
			
			//if (DDLQUIN.SelectedValue=="1")
			//{
                DBFunctions.Request(prestamoempledos, IncludeSchema.NO, "SELECT M.pcon_concepto,MPRE_SECUENCIA,MPRE_NUMELIBR,MPRE_TPERNOMI,MPRE_FECHPREST,COALESCE(MPRE_NUMECUOT,0),COALESCE(MPRE_VALORPRES,0),COALESCE(MPRE_CUOTPAGA,0),COALESCE(MPRE_PORCINTE,0),COALESCE(MPRE_VALOPAGA,0), p.pcon_nombconc  FROM DBXSCHEMA.MPRESTAMOEMPLEADOS M,DBXSCHEMA.PCONCEPTONOMINA P WHERE M.pcon_concepto=P.pcon_concepto and M.memp_codiempl='" + codigoempleado + "' and P.pcon_claseconc='F' and mpre_estdpres=1 and ((mpre_tpernomi=1 AND "+ DDLQUIN.SelectedValue +" = 1) OR (mpre_tpernomi=2 AND "+ DDLQUIN.SelectedValue +" = 2 )  or mpre_tpernomi=3) AND (MPRE_FECHPREST<'" + lb2.ToString() + "') and mpre_valorpres > mpre_valopaga AND   M.PCON_CONCEPTO||M.MPRE_NUMELIBR||M.MEMP_CODIEMPL NOT IN (SELECT  D.PCON_CONCEPTO||D.DQUI_DOCREFE||D.MEMP_CODIEMPL FROM MQUINCENAS M, DQUINCENA D, CNOMINA CN WHERE M.MQUI_CODIQUIN = D.MQUI_CODIQUIN AND M.MQUI_ANOQUIN = CN.CNOM_ANO AND M.MQUI_MESQUIN = CN.CNOM_MES) ;");
			//}
			//if (DDLQUIN.SelectedValue=="2")
			//{
   //             DBFunctions.Request(prestamoempledos, IncludeSchema.NO, "SELECT M.pcon_concepto,MPRE_SECUENCIA,MPRE_NUMELIBR,MPRE_TPERNOMI,MPRE_FECHPREST,COALESCE(MPRE_NUMECUOT,0),COALESCE(MPRE_VALORPRES,0),COALESCE(MPRE_CUOTPAGA,0),COALESCE(MPRE_PORCINTE,0),COALESCE(MPRE_VALOPAGA,0), p.pcon_nombconc  FROM DBXSCHEMA.MPRESTAMOEMPLEADOS M,DBXSCHEMA.PCONCEPTONOMINA P WHERE M.pcon_concepto=P.pcon_concepto and M.memp_codiempl='" + codigoempleado + "' and P.pcon_claseconc='F' and mpre_estdpres=1 and (mpre_tpernomi=2 or mpre_tpernomi=3)  AND (MPRE_FECHPREST < '" + lb2.ToString() + "') and mpre_valorpres > mpre_valopaga ");
			//}
			for (i=0;i<prestamoempledos.Tables[0].Rows.Count;i++)
			{
				valorcuota=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())/int.Parse(prestamoempledos.Tables[0].Rows[i][5].ToString());
				parte2=double.Parse(prestamoempledos.Tables[0].Rows[i][7].ToString())*valorcuota;
			
				if (parte2==0)
				{
					intereses=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())*double.Parse(prestamoempledos.Tables[0].Rows[i][8].ToString())/100;
					cuotamasinteres = valorcuota+intereses;
					capital = valorcuota;
					saldo=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())-capital;
                    this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, prestamoempledos.Tables[0].Rows[i][0].ToString(), 1, cuotamasinteres, 0, cuotamasinteres, "PESOS M/CTE", saldo, prestamoempledos.Tables[0].Rows[i][2].ToString(), apellido1, apellido2, nombre1, nombre2, prestamoempledos.Tables[0].Rows[i][10].ToString());
					if (banderaNombre==0)
					{
                        this.ingresar_datos_datatable(codquincena, codigoempleado, prestamoempledos.Tables[0].Rows[i][0].ToString(), 1, cuotamasinteres, 0, cuotamasinteres, "PESOS M/CTE", saldo, prestamoempledos.Tables[0].Rows[i][2].ToString(), apellido1, apellido2, nombre1, nombre2, prestamoempledos.Tables[0].Rows[i][10].ToString());
					//	banderaNombre+=1;
					}
					else
					{
                        this.ingresar_datos_datatable(codquincena, "", prestamoempledos.Tables[0].Rows[i][0].ToString(), 1, cuotamasinteres, 0, cuotamasinteres, "PESOS M/CTE", saldo, prestamoempledos.Tables[0].Rows[i][2].ToString(), "", "", "", "", prestamoempledos.Tables[0].Rows[i][10].ToString());
					}
					sqlStrings.Add("insert into dprestamoempleados values ("+codquincena+","+prestamoempledos.Tables[0].Rows[i][2].ToString()+",1,"+valorcuota+","+intereses+","+capital+","+saldo+",default)");
					//					DBFunctions.NonQuery("insert into dprestamoempleados values ("+codquincena+","+prestamoempledos.Tables[0].Rows[0][2].ToString()+",1,"+valorcuota+","+intereses+","+capital+","+saldo+",default)");
					sumadescontado+=cuotamasinteres;
				}
				else
				{
					//prueba2.Text=parte2.ToString();
					saldoporpagar=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())-parte2;
					if (saldoporpagar>0)
					{
						intereses=saldoporpagar*double.Parse(prestamoempledos.Tables[0].Rows[i][8].ToString())/100;
						capital = parte2+valorcuota;
						cuotamasinteres=valorcuota+intereses;
						saldo=double.Parse(prestamoempledos.Tables[0].Rows[i][6].ToString())-capital;
                        this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, prestamoempledos.Tables[0].Rows[i][0].ToString(), 1, cuotamasinteres, 0, cuotamasinteres, "PESOS M/CTE", saldo, prestamoempledos.Tables[0].Rows[i][2].ToString(), apellido1, apellido2, nombre1, nombre2, prestamoempledos.Tables[0].Rows[i][10].ToString());
						if (banderaNombre==0)
						{
                            this.ingresar_datos_datatable(codquincena, codigoempleado, prestamoempledos.Tables[0].Rows[i][0].ToString(), 1, cuotamasinteres, 0, cuotamasinteres, "PESOS M/CTE", saldo, prestamoempledos.Tables[0].Rows[i][2].ToString(), apellido1, apellido2, nombre1, nombre2, prestamoempledos.Tables[0].Rows[i][10].ToString());
						//	banderaNombre+=1;
						}
						else
						{
                            this.ingresar_datos_datatable(codquincena, "", prestamoempledos.Tables[0].Rows[i][0].ToString(), 1, cuotamasinteres, 0, cuotamasinteres, "PESOS M/CTE", saldo, prestamoempledos.Tables[0].Rows[i][2].ToString(), "", "", "", "", prestamoempledos.Tables[0].Rows[i][10].ToString());
						}
						sqlStrings.Add("insert into dprestamoempleados values ("+codquincena+","+prestamoempledos.Tables[0].Rows[i][2].ToString()+",1,"+valorcuota+","+intereses+","+capital+","+saldo+",default)");						
						//						DBFunctions.NonQuery("insert into dprestamoempleados values ("+codquincena+","+prestamoempledos.Tables[0].Rows[0][2].ToString()+",1,"+valorcuota+","+intereses+","+capital+","+saldo+",default)");
						sumadescontado+=cuotamasinteres;
					}	
				}
			}
		}
		
		
		protected void actuliza_registro_mpagosydtosper(int i_emp, string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps) 
		{


            DataSet mpagosydtosper = new DataSet();
            string descripcionconcepto="";
            o_Varios.Actualiza_mpagosydtosper(codigoempleado, lb, lb2, codquincena, sueldo, docref, lbmas1, apellido1, apellido2, nombre1, nombre2, periodoeps, mpagosydtosper );


                    if (mpagosydtosper.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < mpagosydtosper.Tables[0].Rows.Count; i++)
                        {
                            docref = mpagosydtosper.Tables[0].Rows[i]["MPAG_SECUENCIA"].ToString();
                            this.validaciondebitaacredita(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(), mpagosydtosper.Tables[0].Rows[i]["PCON_NOMBCONC"].ToString());
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE" ,0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][7].ToString(),"Novedad de Descuento Permanente que afecta EPS? posible error..",double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0,0,0,0);
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][8].ToString(),"Novedad de descuento Permanente que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()));
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][9].ToString(),"Novedad de descuento Permanente que afecta Primas? posible error..",0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0,0);											
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][10].ToString(),"Novedad de descuento Permanente que afecta Vacaciones? posible error..",0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0);
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][11].ToString(),"Novedad de descuento Permanente que afecta Cesantias? posible error..",0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0,0,0,0);
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][12].ToString(),"Novedad de descuento Permanente que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0,0);
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][13].ToString(),"Novedad de descuento Permanente que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0,0);
						    this.validacionafectaciones(codquincena,codigoempleado,mpagosydtosper.Tables[0].Rows[i][0].ToString(),1,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,mpagosydtosper.Tables[0].Rows[i][5].ToString(),mpagosydtosper.Tables[0].Rows[i][14].ToString(),"Novedad de descuento Permanente que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(mpagosydtosper.Tables[0].Rows[i][6].ToString()),0);
		    		    }
				    }
		}
		 
		 
		protected void liquidar_apropiaciones(string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario)
		{
			string tcontrato=DBFunctions.SingleData("select  tcon_contrato from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"' ");
			
			double aportesena=0;
			string porcEmpresa =DBFunctions.SingleData("select papo_porcapor from dbxschema.paportepatronal where papo_tipoaporte=6");
			string porcEmpleado=DBFunctions.SingleData("select cnom_porcepsempl from dbxschema.cnomina");

            if (porcEmpresa.Equals(""))
            {
                Utils.MostrarAlerta(Response, "Proceso cancelado... No se han paremetrizado los aportes patronales.");
                return;
            }
            aportesena=double.Parse(porcEmpresa)+double.Parse(porcEmpleado);
			
			int i;	
			double apropiacionARP,apropiacionICBF,apropiacionSENA,apropiacionCAJACOMPENSACION,apropiacionFONDOPENSIONES,apropiacionEPS=0;
			DataSet mquincenas= new DataSet();
			//DataSet cnomina= new DataSet();
			DataSet paportepatronal= new DataSet();
			DBFunctions.Request(paportepatronal,IncludeSchema.NO,"select papo_codiapor,papo_nombapor,mnit_nit,papo_porcapor,papo_tipoaporte from dbxschema.paportepatronal");
			//calculo del porcentaje ARP
			string porcentajeArp=DBFunctions.SingleData("select B.prie_porcentaje from dbxschema.mempleado A,dbxschema.priesgoprofesional B where A.prie_codiries=B.prie_codiries and a.memp_codiempl='"+codigoempleado+"' ");


			if (paportepatronal.Tables[0].Rows.Count==0)
			{
                mensaje += "Porfavor Configure la tabla correspondiente a los Aportes Patronales  \\n" ;
			}
			else
			{
				//DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_baseporcsalainteg from dbxschema.cnomina");
				DBFunctions.Request(mquincenas,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+o_CNomina.CNOM_ANO+" and mqui_mesquin="+o_CNomina.CNOM_MES+" and mqui_tpernomi="+o_CNomina.CNOM_QUINCENA+" ");
				
				//DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
				
				string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+paportepatronal.Tables[0].Rows[0][1].ToString()+"' ");
				for (i=0;i<paportepatronal.Tables[0].Rows.Count;i++)
				{
					//icbf
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="1")
					{
						if (tcontrato!="3" && tcontrato != "5")
						{
							if (tiposalario=="1")
							{
								apropiacionICBF=((acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionICBF=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							sqlStrings.Add("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionICBF+")");
							//  						DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionICBF+")");
						}
											
						//bandera+=1;
						//Response.Write(ahy mas de un concepto computando para apoertes???preguntar si meter esto)
						
						//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionICBF,docref,apellido1,apellido2,nombre1,nombre2);
					}
					//SENA
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="2")
					{
						if (tcontrato!="3" && tcontrato != "5")
						{
							if (tiposalario=="1")
							{
								apropiacionSENA=((acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionSENA=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
					
							//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionSENA,docref,apellido1,apellido2,nombre1,nombre2);	
							
							sqlStrings.Add("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionSENA+")");
							//							DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionSENA+")");
						}
					}
					//ARP
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="3")
					{
						if (tiposalario=="1")
						{
							apropiacionARP=((acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(porcentajeArp))/100;
						}
						else
						{
							apropiacionARP=(acumuladoeps*double.Parse(porcentajeArp))/100;
						}
						//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionARP,docref,apellido1,apellido2,nombre1,nombre2);
						sqlStrings.Add("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionARP+")");
						//						DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionARP+")");
					}
					//CAJA
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="4")
					{
						if (tcontrato!="3" && tcontrato != "5")
						{
							if (tiposalario=="1")
							{
								apropiacionCAJACOMPENSACION=((acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionCAJACOMPENSACION=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
					
							//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionCAJACOMPENSACION,docref,apellido1,apellido2,nombre1,nombre2);	
							sqlStrings.Add("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionCAJACOMPENSACION+")");
							//	DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionCAJACOMPENSACION+")");
						}
					}
					//FONDO DE PENSIONES
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="5")
					{
						if (tcontrato!="3" && tcontrato != "5")
						{
							if (tiposalario=="1")
							{
								apropiacionFONDOPENSIONES=((acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionFONDOPENSIONES=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
					
							//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionFONDOPENSIONES,docref,apellido1,apellido2,nombre1,nombre2);	
							sqlStrings.Add("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionFONDOPENSIONES+")");
							//							DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionFONDOPENSIONES+")");
						}
					}
					//EPS
					if (paportepatronal.Tables[0].Rows[i][4].ToString()=="6")
					{
						if (tcontrato=="3" || tcontrato == "5")
						{
							apropiacionEPS=(acumuladoeps*aportesena)/100;
						}
						else
						{
							if (tiposalario=="1")
							{
								apropiacionEPS=((acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
							else
							{
								apropiacionEPS=(acumuladoeps*double.Parse(paportepatronal.Tables[0].Rows[i][3].ToString()))/100;
							}
						}
						//this.ingresar_datos_datatable(mquincenas.Tables[0].Rows[0][0].ToString(),codigoempleado,paportepatronal.Tables[0].Rows[i][0].ToString(),1,0,0,0,"PESOS M/CTE",apropiacionEPS,docref,apellido1,apellido2,nombre1,nombre2);		
						sqlStrings.Add("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionEPS+")");
						//						DBFunctions.NonQuery("insert into dprovapropiaciones values (default, "+mquincenas.Tables[0].Rows[0][0].ToString()+",'"+codigoempleado+"','"+paportepatronal.Tables[0].Rows[i][0].ToString()+"',"+apropiacionEPS+")");
					}
				}
			}
		}
		
		//Método que revisa si el valor de eps calculado es menor al establecido por el salario mínimo
		//En caso de ser así, retorna el valor calculado del mínimo.
		private double minimoEps(double eps)
		{
       //   si se esta en la primera quincena no se debe aplicar esta rutina
	   //   porque en la segunda quincena debe ajustar el valor sobre todo lo devengado en el mes
			double retorna,calculadoMinimo,calculadoMinDia;
			if (lbtipopago.Text=="QUINCENAL" && DDLQUIN.SelectedValue=="1" && tipoPeriodoPago == "1")
			{
				retorna=eps;
				return retorna;
			}	
			
			double minimo=Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU);
			double porcentaje=Convert.ToDouble(o_CNomina.CNOM_PORCEPSEMPL);
			calculadoMinimo=(porcentaje*minimo)/100;
			calculadoMinDia=calculadoMinimo/30;
			calculadoMinimo=calculadoMinDia*(this.diastrabajados-diasexceptosauxtransp);
            if (calculadoMinimo > eps + valorepsquinanteriorA)
			{
				/*if(this.diastrabajados == 30)
					retorna=calculadoMinimo;
				else
					retorna=(calculadoMinimo/30)*this.diastrabajados;*/
                retorna = calculadoMinimo - valorepsquinanteriorA;
			}
			else
			{
				retorna=eps;
			}
			return retorna;
		}
		
		//Método que revisa si el valor del fondo calculado es menor al establecido por el salario mínimo
		//En caso de ser así, retorna el valor calculado del mínimo.
		private double minimoFondo(double fondo)
		{
		//   si se esta en la primera quincena no se debe aplicar esta rutina
		//   porque en la segunda quincena debe ajustar el valor sobre todo lo devengado en el mes
			double retorna, calculadoMinimo,calculadoMinDia;
			if (lbtipopago.Text=="QUINCENAL" && DDLQUIN.SelectedValue=="1" && tipoPeriodoPago == "1")
			{
				retorna=fondo;
				return retorna;
			}	
		
			double minimo=Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU);
			double porcentaje=Convert.ToDouble(o_CNomina.CNOM_PORCFONDEMPL);
			calculadoMinimo=(porcentaje*minimo)/100;
			calculadoMinDia=calculadoMinimo/30;
			calculadoMinimo=calculadoMinDia*(this.diastrabajados-diasexceptosauxtransp);
            if (calculadoMinimo > fondo + valorfondoquinanteriorA)
			{
				/*if(this.diastrabajados == 30)
					retorna=calculadoMinimo;
				else
					retorna=(calculadoMinimo/30)*this.diastrabajados;*/
                retorna = calculadoMinimo - valorfondoquinanteriorA;
			}
			else
			{
				retorna=fondo;
			}
			return retorna;
		}

		protected void liquidar_epsfondo(int i_emp, string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string periodoeps,string tiposalario)
		{
            /*
            if ((diasvaca + diassueldopagado) >= 30 && lbtipopago.Text == "MENSUAL") return;
            if ((diasvaca + diassueldopagado) >= 15 && lbtipopago.Text == "QUINCENAL" && tipoPeriodoPago == "1") return;
            */
			//Sacar valor máximo de liquidación
		    string cantsal   = o_CNomina.CNOM_TOPEPAGOEPS;
            string salminimo = o_CNomina.CNOM_SALAMINIACTU;
            double valor = (double.Parse(cantsal) * double.Parse(salminimo));

            valorfondoquinanteriorA = 0; valorepsquinanteriorA = 0;

			int     numquincenaanterior=0;
			int     numquincenaactual=0;
			DataSet mquincenas= new DataSet();	
			DataSet valorfondoquinanterior= new DataSet();	
			DateTime fechaquin= new DateTime();
			double  acumulado1=0,acumulado3=0,valorepsempresa=0,valorfondopensionempresa=0,periodoanterior=0;
            valorepsempleado = 0;
            //Traigo el ultimo registro del maestro de Quincenas
	        string  fondo = o_CNomina.CNOM_CONCFONDCODI;
            string  eps   = o_CNomina.CNOM_CONCEPSCODI;
            string descripcionconceptofondo = o_CNomina.CNOM_CONCFONDDESC;
        //        DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+fondo+"' ");
			string  numquincenaante,numquincenaact;
            string nombreFondoPension = empleados.Tables[0].Rows[i_emp]["NOMBFONDPENS"].ToString(); // DBFunctions.SingleData("select pfon_nombpens from dbxschema.pfondopension pfon,dbxschema.mempleado memp where pfon.PFON_CODIPENS=memp.PFON_CODIPENS and memp_codiempl='"+codigoempleado+"'");
			descripcionconceptofondo+="("+nombreFondoPension+")";
            string  descripcionconceptoeps = o_CNomina.CNOM_CONCEPSDESC;
        //        DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+eps+"' ");
			string  nombreEps = empleados.Tables[0].Rows[i_emp]["NOMBEPS"].ToString();  //DBFunctions.SingleData("select PEPS_NOMBEPS from dbxschema.peps peps,dbxschema.mempleado memp where peps.peps_CODIeps=memp.peps_CODIeps and memp_codiempl='"+codigoempleado+"'");
			descripcionconceptoeps+="("+nombreEps+")";
			string  tipocontrato = empleados.Tables[0].Rows[i_emp]["TCON_CONTRATO"].ToString();  //DBFunctions.SingleData("select T.tcon_contrato from dbxschema.tcontratonomina T,dbxschema.mempleado M where memp_codiempl='"+codigoempleado+"' and T.tcon_contrato=M.tcon_contrato ");
                                                                                           //    string  periodoPago = DBFunctions.SingleData("select memp_peripago from dbxschema.mempleado M where memp_codiempl='" + codigoempleado + "' ");

            //DBFunctions.Request(cnomina2,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_pagomenstper as periodopagomensual,cnom_opciquinomens as quinomes from dbxschema.cnomina");

            //*********************************que solo mire esto para pagos quincenales
            /*if(tiposalario!="1")
			{
				if (acumuladoeps>valor)
					acumuladoeps=valor;
			}*/

            #region Mirar si tuvo incapacidad periodo pasado.
            if (lbtipopago.Text=="MENSUAL")
			{
				fechaquin=Convert.ToDateTime(lb);

				numquincenaact =DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 ");
				if(numquincenaact!=String.Empty)
				{
					numquincenaactual=Convert.ToInt32(numquincenaact);
				}

				// fechaquin= fechaquin.AddMonths(-1);
				numquincenaante=DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=1 ");
				if(numquincenaante!=String.Empty)
				{
					numquincenaanterior=Convert.ToInt32(numquincenaante);
	
				}
				
							
				//string encontroInc=DBFunctions.SingleData("Select DQUI.* from DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.PCONCEPTONOMINA PCON where memp_codiempl='"+codigoempleado+"' AND PCON.pcon_concepto=DQUI.pcon_concepto AND PCON_CLASECONC='L' AND PCON_SIGNOLIQ='D' AND MQUI_CODIQUIN="+numquincenaanterior+"");
				//string encontroInc=DBFunctions.SingleData("select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,(M.msus_hasta-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion,P.pcon_nombconc,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from msusplicencias M,pconceptonomina P,tdesccantidad T where M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad)  and (M.msus_desde between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
				
				//if (encontroInc!=String.Empty)
				//{
					//buscar base mes pasado
					//string basemesA=DBFunctions.SingleData("Select dpaga_afeps from DBXSCHEMA.DPAGAAFECEPS where mqui_codiquin="+numquincenaanterior+" and memp_codiempl='"+codigoempleado+"'");
					//acumuladoeps=double.Parse(basemesA==string.Empty?"0":basemesA);

				//}


//				string pagosquincenaant = "";
			
				if (numquincenaactual != 0)
				{
                    valorfondoquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE mqui_codiquin in (" + numquincenaanterior + "," + numquincenaact + ") AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + fondo + "'"));
                    valorepsquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE mqui_codiquin in (" + numquincenaanterior + "," + numquincenaact + ") AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + eps + "'"));
                    pagosquincenaant = double.Parse(DBFunctions.SingleData("SELECT coalesce(SUM(DQUI.DQUI_APAGAR - DQUI.DQUI_ADESCONTAR),0) FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE mqui_codiquin in (" + numquincenaanterior + "," + numquincenaact + ")  AND MEMP_CODIEMPL='" + codigoempleado + "' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFEC_EPS='S'"));
				}
			
			}
			#endregion

			#region Quincenal
			if (lbtipopago.Text=="QUINCENAL")
			{
				if (o_CNomina.CNOM_QUINCENA=="2")
				{
					
					numquincenaact =DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=2 ");
					if(numquincenaact!=String.Empty)
					{
						numquincenaactual=Convert.ToInt32(numquincenaact);
		            }

					numquincenaante=DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=1 ");
					if(numquincenaante!=String.Empty)
					{
						numquincenaanterior=Convert.ToInt32(numquincenaante);
					}
					fechaquin=Convert.ToDateTime(lb);
	//h				fechaquin= fechaquin.AddMonths(-1);

					try 
					{
						periodoanterior=Convert.ToInt32(DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 "));
					}
					catch {periodoanterior=0;}

					string encontroInc=DBFunctions.SingleData("select distinct M.pcon_concepto,M.memp_codiempl,M.msus_desde,M.msus_hasta,(M.msus_hasta-M.msus_desde)+1,M.ttip_coditipo,P.pcon_signoliq,T.tdes_descripcion,P.pcon_nombconc,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from msusplicencias M,pconceptonomina P,tdesccantidad T where M.memp_codiempl='"+codigoempleado+"' and P.pcon_claseconc='L' and (M.pcon_concepto=P.pcon_concepto) and (P.pcon_desccant=T.tdes_cantidad)  and (M.msus_desde between '"+lb.ToString()+"' and '"+lb2.ToString()+"')");
				
					if (encontroInc!=String.Empty)
					{
                        mensaje += "Encontró Incapacidad para el empleado " + codigoempleado + " en el periodo, se tomara como base para EPS el anterior acumulado. \\n";
						//buscar base mes pasado
						//						string basemesA=DBFunctions.SingleData("Select dpaga_afeps from DBXSCHEMA.DPAGAAFECEPS where mqui_codiquin="+periodoanterior+" and memp_codiempl='"+codigoempleado+"'");
						//						acumuladoeps=double.Parse(basemesA);

					}					
				}
				else
				{
					numquincenaact =DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+o_CNomina.CNOM_ANO+" and MQUI.mqui_mesquin="+o_CNomina.CNOM_MES+" and MQUI.mqui_tpernomi=1 ");
					if(numquincenaact!=String.Empty)
					{
						numquincenaactual=Convert.ToInt32(numquincenaact);	
					}

					fechaquin=Convert.ToDateTime(lb);
		// h			fechaquin= fechaquin.AddMonths(-1);
					numquincenaante=DBFunctions.SingleData("SELECT MQUI.mqui_codiquin from dbxschema.mquincenas MQUI wHERE MQUI.mqui_anoquin="+fechaquin.Year.ToString()+" and MQUI.mqui_mesquin="+fechaquin.Month.ToString()+" and MQUI.mqui_tpernomi=2 ");
					if(numquincenaante!=String.Empty)
					{
						numquincenaanterior=Convert.ToInt32(numquincenaante);
					}						
				}			    	

				if (numquincenaactual != 0)
				{
                    valorfondoquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE (mqui_codiquin=" + numquincenaanterior + " or mqui_codiquin=" + numquincenaact + ") AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + fondo + "' ")); // and d.dqui_vacaciones not in 'V'
                    valorepsquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE (mqui_codiquin=" + numquincenaanterior + " or mqui_codiquin=" + numquincenaact + ") AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + eps + "' ")); // and d.dqui_vacaciones not in 'V'
                    pagosquincenaant = double.Parse(DBFunctions.SingleData("SELECT coalesce(SUM(DQUI.DQUI_APAGAR - DQUI_ADESCONTAR),0) FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE (MQUI_CODIQUIN=" + numquincenaanterior + " or mqui_codiquin=" + numquincenaact + ")  AND MEMP_CODIEMPL='" + codigoempleado + "' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFEC_EPS='S' ")); //and dqui.dqui_vacaciones not in 'V'	
				}
				else
				{
                    valorfondoquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE mqui_codiquin=" + numquincenaanterior + " AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + fondo + "' "));  // and d.dqui_vacaciones not in 'V'
                    valorepsquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE mqui_codiquin=" + numquincenaanterior + " AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + eps + "' ")); // and d.dqui_vacaciones not in 'V'
                    pagosquincenaant = double.Parse(DBFunctions.SingleData("SELECT coalesce(SUM(DQUI.DQUI_APAGAR - DQUI_ADESCONTAR),0) FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE MQUI_CODIQUIN=" + numquincenaanterior + " AND MEMP_CODIEMPL='" + codigoempleado + "' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFEC_EPS='S' ")); // and dqui.dqui_vacaciones not in 'V'	
				}

			//	if(pagosquincenaant!=String.Empty)
			//	{
			//		acumuladoeps=acumuladoeps+double.Parse(pagosquincenaant.ToString());
			//	}
			}
							
			
			//DBFunctions.Request(mquincenas,IncludeSchema.NO,"select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+o_CNomina.CNOM_ANO+" and mqui_mesquin="+o_CNomina.CNOM_MES+" and mqui_tpernomi="+cnomina.Tables[0].Rows[0][2].ToString()+" ");
						
			//DBFunctions.Request(cnomina,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena,cnom_concsalacodi,cnom_concsubtcodi,cnom_substranperinomi,cnom_substransactu,truncate(decimal(cnom_substransactu/2),4),cnom_conchenocodi,cnom_epsperinomi,cnom_porcepsempl,cnom_concepscodi,cnom_epsperinomi,cnom_concfondcodi,cnom_porcfondempl,cnom_baseporcsalainteg from dbxschema.cnomina");	
			prueba.Text=acumuladoeps.ToString();

            //  acumuladoeps -= sumaLicenciasSuspPeriodo;  // las  suspensiones y licencias se descuenta el aporte completo del mes y asi se paga a la EPS
					
			if (lbtipopago.Text=="QUINCENAL")
			{
				
				//validar si se descuenta en ambas quincenas =1
				if (o_CNomina.CNOM_EPSPERINOMI=="1")
				{
					//averiguar en que quincena estoy, primera calculo normal
					if (o_CNomina.CNOM_QUINCENA=="1")
					{
						//si es salario integral se calcula sobre el 70% del salario.=1
						if (tiposalario=="1")
						{
							//si el empleado esta pensionado no le liquido fondo de pensiones 4.
							if (acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100>valor)
							{
								valorfondopensionempleado=valor*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
								valorepsempleado=valor*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
							}
							else
							{
								valorfondopensionempleado=(acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
								valorepsempleado=(acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
							}										
						}
							//no integral o variable 3,2
						else
						{
							if ((acumuladoeps + pagosquincenaant) > valor) // cuando es mayor que el topo de salario minimos
							{
                                valorfondopensionempleado = valor * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100;
                                valorepsempleado = valor * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100;
						    }
                            else
                            {
							    valorfondopensionempleado = (acumuladoeps + pagosquincenaant)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
							    valorepsempleado = (acumuladoeps + pagosquincenaant)*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
						    }
                        }
						valorfondopensionempleado = valorfondopensionempleado - valorfondoquinanteriorA;
						valorepsempleado = valorepsempleado - valorepsquinanteriorA;                        
						
						//validar que no descuente eps para SENA.
                        if (tipocontrato != "3" && tipocontrato != "5")
						{	
                            if(acumuladoeps>=double.Parse(salminimo))
                            {
							    valorepsempleado=this.minimoEps(valorepsempleado);
                            }
							this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
							if (banderaNombre==0)
							{
								this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
							//	banderaNombre+=1;
							}
							else
							{
								this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptoeps);
							}
						
							sumadescontado+=valorepsempleado;
						}	
						//						}
						//4=pensionado,3=sena.
                        if (tipocontrato != "4" && tipocontrato != "3" && tipocontrato != "5")
						{
                            if (acumuladoeps >= double.Parse(salminimo))
                            {
                                valorfondopensionempleado = this.minimoFondo(valorfondopensionempleado);
                            }
							this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
							if (banderaNombre==0)
							{
								this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
							//	banderaNombre+=1;
							}
							else
							{
								this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptofondo);
							}
							
							sumadescontado+=valorfondopensionempleado;	
						}
					}
					
					//si estoy en la segunda, acumulo el valor acumulado de la primera mas la segunda,calculo y resto el resultado de la primera
					if  (o_CNomina.CNOM_QUINCENA=="2")
					{
						if (valorepsquinanteriorA<0)
						{
							//Response.Write("<script language:javascript>alert('No se encontro registro de anteriores liquidaciones');</script>");
							if (tiposalario=="1")
							{
								
								if (acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100>valor)
								{
									valorfondopensionempleado=valor*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
									valorepsempleado         =valor*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
								
								}
								else
								{
									valorfondopensionempleado=(acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
									valorepsempleado         =(acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
								}
							}
							else
							{
								//Response.Write("<script language:java>alert('LA SUMA SOBRE LA K SACO EPS ES "+acumuladoeps+" ');</script>");
								valorfondopensionempleado=acumuladoeps*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
								valorepsempleado         =acumuladoeps*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
							}
                            
                            valorfondopensionempleado = valorfondopensionempleado - valorfondoquinanteriorA;
                            valorepsempleado = valorepsempleado - valorepsquinanteriorA;
														
							if (tipocontrato!="3" && tipocontrato != "5")
							{	
								valorepsempleado=this.minimoEps(valorepsempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
							
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptoeps);
								}
							
								sumadescontado+=valorepsempleado;
							}
							//							}
							
							//4=pensionado,3=sena.
							if (tipocontrato!="4" && tipocontrato!="3" && tipocontrato != "5")
							{
								valorfondopensionempleado=this.minimoFondo(valorfondopensionempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptofondo);
								}

								sumadescontado+=valorfondopensionempleado;	
							}
						}
						else
						{
							acumulado1=valorepsquinanteriorA*100/double.Parse(o_CNomina.CNOM_PORCEPSEMPL);
						//	acumulado3=acumulado1+acumuladoeps;
                            acumulado3 = acumuladoeps; // +pagosquincenaant; ya esta acumuladas las 2 qnas
                        //  acumulado3 = acumuladoeps+pagosquincenaant; // ya esta acumuladas las 2 qnas
					
							if (tiposalario=="1")
							{
								acumulado1 = (acumulado1*100)/double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG);
							//	acumulado3 = acumulado1+acumuladoeps;  
                            //  acumulado3 = acumuladoeps;// +pagosquincenaant;
								valorepsempleado=(((acumulado3*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCEPSEMPL))/100)-valorepsquinanteriorA;
								
								if (tipocontrato!="4")
								{
									DBFunctions.Request(valorfondoquinanterior,IncludeSchema.NO,"SELECT D.mqui_codiquin,D.dqui_adescontar,D.pcon_concepto FROM dbxschema.dquincena D WHERE mqui_codiquin="+numquincenaanterior+" AND D.memp_codiempl='"+codigoempleado+"'AND D.pcon_concepto='"+fondo+"'");
								    if (valorfondoquinanterior.Tables[0].Rows.Count!=0)
										valorfondopensionempleado=(((acumulado3*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL))/100)-double.Parse(valorfondoquinanterior.Tables[0].Rows[0][1].ToString());
									else
										valorfondopensionempleado=(((acumulado3*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL))/100);
								}	
							}
							else
							{
                            //    if ((acumuladoeps + pagosquincenaant) > valor) // cuando es mayor que el topo de salario minimos
                                if ((acumulado3 ) > valor) // cuando es mayor que el topo de salario minimos
                                    valorepsempleado = ((valor * double.Parse(o_CNomina.CNOM_PORCEPSEMPL)) / 100) - valorepsquinanteriorA;
                                else
							  	    valorepsempleado = ((acumulado3*double.Parse(o_CNomina.CNOM_PORCEPSEMPL))/100) - valorepsquinanteriorA;
                              
								if (tipocontrato!="4")
								{
                                    DBFunctions.Request(valorfondoquinanterior, IncludeSchema.NO, "SELECT D.mqui_codiquin,sum(D.dqui_adescontar),D.pcon_concepto FROM dbxschema.dquincena D WHERE mqui_codiquin=" + numquincenaanterior + " AND D.memp_codiempl='" + codigoempleado + "'AND D.pcon_concepto='" + fondo + "' group by  D.mqui_codiquin, D.pcon_concepto");  //  and d.dqui_vacaciones not in 'V'");
									string fondoaux;
									try
									{
										fondoaux=valorfondoquinanterior.Tables[0].Rows[0][1].ToString();
									}
									catch(Exception e)
									{
										fondoaux="0";
									}
                               //    if ((acumuladoeps + pagosquincenaant) > valor) // cuando es mayor que el topo de salario minimos
                                    if ((acumulado3 ) > valor) // cuando es mayor que el topo de salario minimos
                                        valorfondopensionempleado = ((valor * double.Parse(o_CNomina.CNOM_PORCEPSEMPL)) / 100) - double.Parse(fondoaux);
                                    else
									    valorfondopensionempleado=((acumulado3*double.Parse(o_CNomina.CNOM_PORCFONDEMPL))/100)-double.Parse(fondoaux);
								}
							}
                        
                            valorfondopensionempleado = valorfondopensionempleado; // -valorfondoquinanteriorA; ya se desconto
                            valorepsempleado = valorepsempleado; // -valorepsquinanteriorA; ya se desconto
						
							if (tipocontrato!="3" && tipocontrato != "5")
							{
								valorepsempleado=this.minimoEps(valorepsempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptoeps);
								}
								sumadescontado+=valorepsempleado;
							}
							//4=pensionado,3=sena.
							if (tipocontrato!="4" && tipocontrato!="3" && tipocontrato != "5")
							{
								valorfondopensionempleado=this.minimoFondo(valorfondopensionempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptofondo);
								}
								
								sumadescontado+=valorfondopensionempleado;	
							}
						}
					}
                }
			}
			else if (lbtipopago.Text=="MENSUAL" && o_CNomina.CNOM_EPSPERINOMI=="1")
			{   
				bandera+=1;
				if (bandera==1)
				{
					errores+=1;
                    Utils.MostrarAlerta(Response, "Apreciado Usuario:Modifique el parametro de pago de E.P.S a la segunda quincena ya que el pago de nomina esta MENSUAL.");
				}
			}

			#endregion

			#region validar si se descuenta en la segunda =2
			if (o_CNomina.CNOM_EPSPERINOMI.Trim()=="2") // DESCUENTA EPS EN LA 2DA QUINA SOLAMENTE
			{
				//averiguar en que quincena estoy, primera no calculo nada, segunda si.
				
				DataSet valepsquinanterior= new DataSet();
                DBFunctions.Request(valepsquinanterior, IncludeSchema.NO, "select COALESCE(SUM(DQUI_ADESCONTAR),0) from dbxschema.dQUINCENA D, DBXSCHEMA.CNOMINA CN where D.memp_codiempl= '" + codigoempleado + "' and D.mqui_codiquin IN (" + numquincenaanterior + ", "+ numquincenaactual +") AND D.PCON_CONCEPTO = CN.CNOM_CONCEPSCODI; ");
							
				if (o_CNomina.CNOM_QUINCENA=="2")
				{
					//dividir procesos para quincenal y mensual.
					if (lbtipopago.Text=="QUINCENAL")
					{
						//si encontro registros,aveiguo cuanto le pagaron en la quincena pasada.
						if (valepsquinanterior.Tables[0].Rows.Count>0)
						{
                            acumulado1 = acumuladoeps; // +pagosquincenaant;  // ESTA QUINCENA MAS PAGOS DE LA PRIMERA QNA (ACUMulado eps ya tiene el pago de la qna anterior)
                            acumuladoeps = acumuladoeps; // +pagosquincenaant;  // ESTA QUINCENA MAS PAGOS DE LA PRIMERA QNA
						    if (tiposalario=="1")
							{
								if (acumulado1*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100>valor)
								{
                                    valorfondopensionempleado = valor * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100 - valorfondoquinanteriorA;
                                    valorepsempleado = valor * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100 - valorepsquinanteriorA;
								}
								else
								{
                                    valorepsempleado = ((acumulado1 * double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG) / 100) * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100) - valorepsquinanteriorA;
									valorfondopensionempleado=((acumulado1*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100) - valorfondoquinanteriorA;
								}
							}
							else
							{
                                if (acumuladoeps > valor) // cuando es mayor que el tope de salario minimos
                                {
                                    valorfondopensionempleado = valor * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100 - valorfondoquinanteriorA;
                                    valorepsempleado = valor * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100 - valorepsquinanteriorA;
                                }
                                else
                                {
                                    valorfondopensionempleado = acumulado1 * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100 - valorfondoquinanteriorA;
                                    valorepsempleado = acumulado1 * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100 - valorepsquinanteriorA;
							    }
                            }
							
							// ***

							//valorfondopensionempleado = valorfondopensionempleado - valorfondoquinanteriorA;
							//valorepsempleado = valorepsempleado - valorepsquinanteriorA;
					
							if (tipocontrato!="3" && tipocontrato != "5")
							{
								valorepsempleado=this.minimoEps(valorepsempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptoeps);
								}
								sumadescontado+=valorepsempleado;
							}
													
							//							}

							//4=pensionado,3=sena.
							if (tipocontrato!="4" && tipocontrato!="3" && tipocontrato != "5")
							{
								valorfondopensionempleado=this.minimoFondo(valorfondopensionempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptofondo);
								}
								sumadescontado+=valorfondopensionempleado;	
							}
						}
								//sino encontro pago en la quincena anterior, calcula sobre el pago de la segunda solamente.				
						else
						{
							bandera+=1;
							if (bandera==1)
							{
                                Utils.MostrarAlerta(Response, "ATENCION:No se encontro registros de la primera quincena de liquidacion,se calculara solamente sobre el salario devengado en la segunda quincena E.P.S.. empleado '" + codigoempleado + "'");
							}
                    
                            acumulado1 = acumuladoeps + pagosquincenaant;  // ESTA QUINCENA MAS PAGOS DE LA PRIMERA QNA
                            acumuladoeps = acumuladoeps + pagosquincenaant;  // ESTA QUINCENA MAS PAGOS DE LA PRIMERA QNA
					
							if (tiposalario=="1")
							{
								if (acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100>valor)
								{
									valorfondopensionempleado=valor*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
									valorepsempleado=valor*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
								
								}
								else
								{
									valorfondopensionempleado=(acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
									valorepsempleado=(acumuladoeps*double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG)/100)*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
								}
							}
							else
                            {
                                if (acumuladoeps > valor) // cuando es mayor que el topo de salario minimos
                                {
                                    valorfondopensionempleado = valor * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100;
                                    valorepsempleado = valor * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100;
							    }
							    else
							    {
                                    valorfondopensionempleado = acumuladoeps * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100;
								    valorepsempleado=acumuladoeps*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
                                }
																
							}
						
							valorfondopensionempleado = valorfondopensionempleado - valorfondoquinanteriorA;
							valorepsempleado = valorepsempleado - valorepsquinanteriorA;
					
							if (tipocontrato!="3" && tipocontrato != "5")
							{
								valorepsempleado=this.minimoEps(valorepsempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptoeps);
								}
								sumadescontado+=valorepsempleado;
							}
							//4=pensionado,3=sena.
							if (tipocontrato!="4" && tipocontrato!="3" && tipocontrato != "5")
							{
								valorfondopensionempleado=this.minimoFondo(valorfondopensionempleado);
								this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								if (banderaNombre==0)
								{
									this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
								//	banderaNombre+=1;
								}
								else
								{
									this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptofondo);
								}
								
								sumadescontado+=valorfondopensionempleado;	
							}
						}
					}
				
					if (lbtipopago.Text=="MENSUAL")
					{
                        //  acumuladoeps    COMPRENDE TODO LO DEVENGDO EN EL MES  
						if (tiposalario=="1")
						{
                            if ((acumuladoeps) * double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG) / 100 > valor)
							{
								valorfondopensionempleado=valor*double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
								valorepsempleado=valor*double.Parse(o_CNomina.CNOM_PORCEPSEMPL)/100;
							}
							else
							{
                                valorfondopensionempleado = ((acumuladoeps ) * double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG) / 100) * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100;
                                valorepsempleado = ((acumuladoeps) * double.Parse(o_CNomina.CNOM_BASEPORCSALAINTEG) / 100) * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100;
							}
						}
						else
						{
                        //    valorepsempleado = (acumuladoeps + pagosquincenaant) * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100;
						//	valorfondopensionempleado=( acumuladoeps + pagosquincenaant) * double.Parse(o_CNomina.CNOM_PORCFONDEMPL)/100;
                            valorepsempleado = (acumuladoeps ) * double.Parse(o_CNomina.CNOM_PORCEPSEMPL) / 100;
                            valorfondopensionempleado = (acumuladoeps) * double.Parse(o_CNomina.CNOM_PORCFONDEMPL) / 100;
                        }

						/*
						string pagosquincenaant = "";
			
						if (numquincenaactual != 0)
						{
							valorfondoquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE (mqui_codiquin="+numquincenaanterior+" or mqui_codiquin="+numquincenaact+") AND D.memp_codiempl='"+codigoempleado+"'AND D.pcon_concepto='"+fondo+"'"));
							valorepsquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE (mqui_codiquin="+numquincenaanterior+" or mqui_codiquin="+numquincenaact+") AND D.memp_codiempl='"+codigoempleado+"'AND D.pcon_concepto='"+eps+"'"));
							pagosquincenaant=DBFunctions.SingleData("SELECT coalesce(SUM(DQUI.DQUI_APAGAR),0) FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE (MQUI_CODIQUIN="+numquincenaanterior+" or mqui_codiquin="+numquincenaact+")  AND MEMP_CODIEMPL='"+codigoempleado+"' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFEC_EPS='S'");	
						}
						else
						{
							valorfondoquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE mqui_codiquin="+numquincenaanterior+" AND D.memp_codiempl='"+codigoempleado+"'AND D.pcon_concepto='"+fondo+"'"));
							valorepsquinanteriorA = double.Parse(DBFunctions.SingleData("SELECT coalesce(sum(D.dqui_adescontar),0) FROM dbxschema.dquincena D WHERE mqui_codiquin="+numquincenaanterior+" AND D.memp_codiempl='"+codigoempleado+"'AND D.pcon_concepto='"+eps+"'"));
							pagosquincenaant=DBFunctions.SingleData("SELECT coalesce(SUM(DQUI.DQUI_APAGAR),0) FROM DBXSCHEMA.DQUINCENA DQUI,DBXSCHEMA.PCONCEPTONOMINA PCON WHERE MQUI_CODIQUIN="+numquincenaanterior+" AND MEMP_CODIEMPL='"+codigoempleado+"' AND DQUI.PCON_CONCEPTO=PCON.PCON_CONCEPTO AND PCON.TRES_AFEC_EPS='S'");	
						}
						*/

						valorfondopensionempleado = valorfondopensionempleado - valorfondoquinanteriorA;
						valorepsempleado = valorepsempleado - valorepsquinanteriorA;

						if (tipocontrato!="3" && tipocontrato != "5")
						{
							valorepsempleado=this.minimoEps(valorepsempleado);
							this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
						
							if (banderaNombre==0)
							{
								this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptoeps);
							//	banderaNombre+=1;
							}
							else
							{
								this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCEPSCODI,1,Math.Round(valorepsempleado,0),0,Math.Round(valorepsempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptoeps);
							}
						
							sumadescontado+=valorepsempleado;
						
						}
						//4=pensionado,3=sena.
						if (tipocontrato!="4" && tipocontrato!="3" && tipocontrato != "5")
						{
							valorfondopensionempleado=this.minimoFondo(valorfondopensionempleado);
							this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
							if (banderaNombre==0)
							{
								this.ingresar_datos_datatable(codquincena,codigoempleado,o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconceptofondo);
							//	banderaNombre+=1;
							}
							else
							{
								this.ingresar_datos_datatable(codquincena,"",o_CNomina.CNOM_CONCFONDCODI,1,Math.Round(valorfondopensionempleado,0),0,Math.Round(valorfondopensionempleado,0),"PESOS M/CTE",0,docref,"","","","",descripcionconceptofondo);
							}
							sumadescontado+=valorfondopensionempleado;	
						}
					}
				}
			}
			#endregion

			//validar si se descuenta en comisiones (primera quincenas+segunda+comisiones)
			if (o_CNomina.CNOM_CONCFONDCODI=="3")
			{
				//mriara cuando liquide comisiones.
			}
		}
		
		protected void actualiza_novedades(int i_emp, string  codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2,string peripago )
		{
            /*
            if ((diasvaca + diassueldopagado) >= 30 && lbtipopago.Text == "MENSUAL") 
				return;
            if ((diasvaca + diassueldopagado) >= 15 && lbtipopago.Text == "QUINCENAL" && tipoPeriodoPago == "1")
					return;
            */
			int j;
			double valordiatrabajo=0;
			double valorhoratrabajonormal=0;
			double valorminuto=0;
			double valorhorasextras,valorevento,valorpagado;
		

   //    los empleados de pago MENSUAL cuando se liquida QUINCENAL, solo debe traer las novedades para la QUINCENA ACTUAL
			
			string lb1 = lb;
			if (lbtipopago.Text=="QUINCENAL" && DDLQUIN.SelectedValue=="2")
			{
                if(Convert.ToInt16(lb1.Length) == 10)
				    lb1 = lb1.Substring(0,7)+"-16";
                else
                    lb1 = lb1.Substring(0, 6) + "-16";
			}

            string fechreinvaca = DBFunctions.SingleData("SELECT CAST(MAX(DVAC_FECHFINAL) + 1 DAY AS CHAR(10)) FROM DVACACIONES DV, MVACACIONES MV, MEMPLEADO ME WHERE ME.MEMP_CODIEMPL = MV.MEMP_CODIEMP AND MV.MVAC_SECUENCIA = DV.MVAC_SECUENCIA AND ME.MEMP_CODIEMPL = '" + codigoempleado + "' AND ME.TEST_ESTADO = '5';");
            if (String.Compare(lb1, fechreinvaca, false) == -1 && fechreinvaca != null)
                lb1 = fechreinvaca.Substring(0,10);  // solo se toman las novedades correspondientes a la vigencia de los dias de pago efectivos porque las demas se liquidaron en las vacaciones

            DataSet afectahorasextras= new DataSet();
			string query="select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and P.tres_afechoraextr='S' and (M.mnov_fecha between '"+lb1+"' and '"+lb2.ToString()+"')";
			DBFunctions.Request(afectahorasextras,IncludeSchema.NO,query);
			if (afectahorasextras.Tables[0].Rows.Count==0)
			{
				valordiatrabajo=sueldo/30;
				valorhoratrabajonormal=sueldo/240;
				valorminuto=sueldo/14400;					
			}
			else 
			{
				if (afectahorasextras.Tables[0].Rows.Count!=0)
				{
					for (j=0;j<afectahorasextras.Tables[0].Rows.Count;j++)
					{
						sueldo=sueldo+double.Parse(afectahorasextras.Tables[0].Rows[j][1].ToString());
					}
				}
				
				valordiatrabajo=sueldo/30;
				valorhoratrabajonormal=sueldo/240;
				valorminuto=sueldo/14400;
			}
		
			DataSet mquincenas= new DataSet();
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			DataSet novedades= new DataSet();
			DataSet licenciasennovedades =new DataSet();
			novedades.Clear();	
			//Traigo las novedades para cada empleado vigente entre las fechas de la quincena vigente

			string sql = "select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='N' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lb1+"' and '"+fecha3+"')";
		 
            //sql = sql + " and M.pcon_concepto NOT in (select pcon_concepto from dquincena where memp_codiempl = '"+codigoempleado+"' and dqui_docrefe= '"+ docref + "' and (dqui_apagar + dqui_adescontar) = M.MNOV_VALRTOTL)";

			DBFunctions.Request(novedades,IncludeSchema.NO,sql);
			//validar si existen licencias en novedades
			DBFunctions.Request(licenciasennovedades,IncludeSchema.NO,"select M.pcon_concepto,M.mnov_valrtotl,M.memp_codiempl,P.pcon_signoliq,M.mnov_cantidad,P.pcon_desccant,P.pcon_factorliq,T.tdes_descripcion,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,P.pcon_nombconc  from mnovedadesnomina M,pconceptonomina P,tdesccantidad T where M.pcon_concepto=P.pcon_concepto and P.pcon_claseconc='L' and P.pcon_desccant=T.tdes_cantidad and M.memp_codiempl='"+codigoempleado+"' and (M.mnov_fecha between '"+lb1+"' and '"+fecha3+"')");
			//mostrar mensaje de adevertencia
			for(j=0;j<licenciasennovedades.Tables[0].Rows.Count;j++)
			{
                mensaje += "Al empleado " + codigoempleado + ", se detectó el ingreso del concepto "+ licenciasennovedades.Tables[0].Rows[j][0].ToString() + " tipo Licencia en Novedades, este concepto NO será tenido en cuenta para la liquidacion, por favor corrija e ingrese este concepto en Licencias.. \\n";
				errores+=1;
			}			
			
			//Recorro cada novedad
			try
			{
				for(j=0;j<novedades.Tables[0].Rows.Count;j++)
				{				
				
					//si es novedad de horas=2
				
					if (novedades.Tables[0].Rows[j][5].ToString()=="2")
					{
					
						valorevento=valorhoratrabajonormal*double.Parse(novedades.Tables[0].Rows[j][6].ToString());
						valorhorasextras= valorhoratrabajonormal*double.Parse(novedades.Tables[0].Rows[j][4].ToString())* double.Parse(novedades.Tables[0].Rows[j][6].ToString());
						this.validaciondebitaacredita(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad en Horas de descuento que afecta EPS? posible error..",valorhorasextras,0,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad en Horas de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorhorasextras);											
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad en Horas de descuento que afecta Primas? posible error..",0,0,valorhorasextras,0,0,0,0,0);											
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad en Horas de descuento que afecta Vacaciones? posible error..",0,0,0,valorhorasextras,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad en Horas de descuento que afecta Cesantias? posible error..",0,valorhorasextras,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad en Horas de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorhorasextras,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad en Horas de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorhorasextras,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorevento.ToString("N")),Math.Round(valorhorasextras,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad en Horas de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorhorasextras,0);
		     		
					}
					//si es novedad de dias=1
					if (novedades.Tables[0].Rows[j][5].ToString()=="1")
					{
						valorpagado=valordiatrabajo*double.Parse(novedades.Tables[0].Rows[j][4].ToString()) * double.Parse(novedades.Tables[0].Rows[j][6].ToString());
						this.validaciondebitaacredita(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad de dias de descuento que afecta EPS? posible error..",valorpagado,0,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad de dias de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorpagado);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad de dias de descuento que afecta Primas? posible error..",0,0,valorpagado,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad de dias de descuento que afecta Vacaciones? posible error..",0,0,0,valorpagado,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad de dias de descuento que afecta Cesantias?  posible error..",0,valorpagado,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad de dias de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorpagado,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad de dias de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorpagado,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad de dias de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorpagado,0);
					
						
					}
		     	
					//si es novedad de minutos=3
					if (novedades.Tables[0].Rows[j][5].ToString()=="3")
					{
						valorpagado=valorminuto*double.Parse(novedades.Tables[0].Rows[j][4].ToString());
						this.validaciondebitaacredita(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][16].ToString());
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad de minutos de descuento que afecta EPS? posible error..",valorpagado,0,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad de minutos de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,valorpagado);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad de minutos de descuento que afecta Primas? posible error..",0,0,valorpagado,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad de minutos de descuento que afecta Vacaciones? posible error..",0,0,0,valorpagado,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad de minutos de descuento que afecta Cesantias? posible error..",0,valorpagado,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad de minutos de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,valorpagado,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad de minutos de descuento que afecta Provisiones? posible error..",0,0,0,0,0,valorpagado,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),double.Parse(novedades.Tables[0].Rows[j][4].ToString()),double.Parse(valorminuto.ToString("N")),Math.Round(valorpagado,0),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad de minutos de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,valorpagado,0);
					}
				
					//si es novedad en pesos=4
					if (novedades.Tables[0].Rows[j][5].ToString()=="4")
					{
                        if(novedades.Tables[0].Rows[j][3].ToString()=="D")
                            sumapagadamasauxt += double.Parse(novedades.Tables[0].Rows[j][1].ToString());
                  //      else
                  //          sumadescontado += double.Parse(novedades.Tables[0].Rows[j][1].ToString());

                        this.validaciondebitaacredita(codquincena, codigoempleado, novedades.Tables[0].Rows[j][0].ToString(), 1, 0, Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())), 0, novedades.Tables[0].Rows[j][7].ToString(), 0, docref, apellido1, apellido2, nombre1, nombre2, novedades.Tables[0].Rows[j][3].ToString(), novedades.Tables[0].Rows[j][16].ToString());
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][8].ToString(),"Novedad pesos m/cte de descuento que afecta EPS? posible error..",Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][9].ToString(),"Novedad pesos m/cte de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())));
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][10].ToString(),"Novedad pesos m/cte de descuento que afecta Primas? posible error..",0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][11].ToString(),"Novedad pesos m/cte de descuento que afecta Vacaciones?  posible error..",0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][12].ToString(),"Novedad pesos m/cte de descuento que afecta Cesantias? posible error..",0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0,0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][13].ToString(),"Novedad pesos m/cte de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][14].ToString(),"Novedad pesos m/cte de descuento que afecta Provisiones? posible error..",0,0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,0);
						this.validacionafectaciones(codquincena,codigoempleado,novedades.Tables[0].Rows[j][0].ToString(),1,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0,novedades.Tables[0].Rows[j][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,novedades.Tables[0].Rows[j][3].ToString(),novedades.Tables[0].Rows[j][15].ToString(),"Novedad pesos m/cte de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,Math.Round(double.Parse(novedades.Tables[0].Rows[j][1].ToString())),0);
					
					}				
				}
			}
			catch
			{
				novedades= new DataSet();
			}                   
		}

		protected void validaciondebitaacredita(string codquincena,string codempleado,string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string camposumaresta,string descripcionconcepto)
		{
			if (camposumaresta=="D")
			{
				this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,canteventos,valorevento,apagar,0,descripcion,saldo,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);	
				if (banderaNombre==0)
				{
					this.ingresar_datos_datatable(codquincena,codempleado,concepto,canteventos,valorevento,apagar,0,descripcion,saldo,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);	
				//	banderaNombre+=1;
				}
				else
				{
					this.ingresar_datos_datatable(codquincena,"",concepto,canteventos,valorevento,apagar,0,descripcion,saldo,docref,"","","","",descripcionconcepto);	
            	}
				
				sumapagado+=apagar;
			}
		                                         	
			else
			{
				this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,canteventos,valorevento,0,apagar,descripcion,saldo,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
				if (banderaNombre==0)
				{
					this.ingresar_datos_datatable(codquincena,codempleado,concepto,canteventos,valorevento,0,apagar,descripcion,saldo,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
				//	banderaNombre+=1;
				}
				else
				{
					this.ingresar_datos_datatable(codquincena,"",concepto,canteventos,valorevento,0,apagar,descripcion,saldo,docref,"","","","",descripcionconcepto);
				}				
				sumadescontado+=apagar;
                if (concepto == o_CNomina.CNOM_CONCFONDPENSVOLU)
                    valorfondopensionvoluntaria += apagar;
			}
		}

		protected void validacionafectaciones(string codquincena,string codempleado,string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string camposumaresta,string campocomparacion,string mensaje,double valorpagadoeps,double valorpagadocesantia,double valorpagadoprima,double valorpagadovacaciones,double valorpagadoretefuente,double valorpagadoprovisiones,double valorpagadoliqdefinitiva,double valorpagadohorasextras)
		{
			//Clasifico cada novedad si suma o resta al empleado
			if (camposumaresta=="D" && campocomparacion=="S")
			{
				acumuladoeps+=Math.Round(valorpagadoeps,0);
				acumuladocesantia+=Math.Round(valorpagadocesantia,0);
				acumuladoprima+=Math.Round(valorpagadoprima,0);
				acumuladovacaciones+=Math.Round(valorpagadovacaciones,0);
				acumuladoretefuente+=Math.Round(valorpagadoretefuente,0);
				acumuladoprovisiones+=Math.Round(valorpagadoprovisiones,0);
				acumuladoliqdefinitiva+=Math.Round(valorpagadoliqdefinitiva,0);
				acumuladohorasextras+=Math.Round(valorpagadohorasextras,0);	
			}
					
	 		else if(camposumaresta=="H" && campocomparacion=="S" )
	 		{
                    acumuladoeps -= Math.Round(valorpagadoeps, 0);
                    acumuladoretefuenteQnaAct+=Math.Round(valorpagadoretefuente,0); // Ver si se toman n los demas items
	 		}
		}

		//JFSC 10042008 calcula dias para descontar en subsidio de transporte y (posiblemente) sueldo
		//fechaInicio= la fecha de inicio de la quincena actual
		//codigoEmpleado el código del empleado
		protected double calcularDiasLicQuincenaAnterior(string fechaInicio, string codigoEmpleado)
		{
			double retorna=0;
			//variables auxiliares
			string aux1,aux2;
			//traer fechas quincena anterior
			DateTime quincenaAntFin=Convert.ToDateTime(fechaInicio);
			DateTime quincenaAntIni;
			DataSet quinAnt=new DataSet(); 
			quincenaAntFin=quincenaAntFin.AddDays(-1);
			quincenaAntIni=Convert.ToDateTime(quincenaAntFin.AddDays(-14));
			//traer licencias y suspensiones (días)
			string query=@"select distinct (M.msus_hasta-M.msus_desde)+1 as diasLic,
							m.ttip_coditipo as suspension
							from dbxschema.msusplicencias M,
								dbxschema.pconceptonomina P,
								dbxschema.tdesccantidad T  
							where M.memp_codiempl='"+codigoEmpleado+@"'
								and P.pcon_claseconc='L' 
								and (M.pcon_concepto=P.pcon_concepto) 
								and (P.pcon_desccant=T.tdes_cantidad)
								and (M.msus_desde between '"+quincenaAntIni.ToString("yyyy-MM-dd")+@"' and '"+quincenaAntFin.ToString("yyyy-MM-dd")+@"')
								and (M.msus_hasta between '"+quincenaAntIni.ToString("yyyy-MM-dd")+@"' and '"+quincenaAntFin.ToString("yyyy-MM-dd")+@"')";
			DBFunctions.Request(quinAnt,IncludeSchema.NO,query);
			if(quinAnt.Tables[0].Rows.Count!=0)
			{
				aux1=quinAnt.Tables[0].Rows[0]["diasLic"].ToString();
				//string retorno=DBFunctions.SingleData(query);
				retorna= Double.Parse(aux1==string.Empty?"0":aux1);
				aux2=quinAnt.Tables[0].Rows[0]["suspension"].ToString();
				this.esSuspension=(aux2=="4"?true:false);
			}
			return retorna;
		}

		protected void actualiza_registro_msusplicencias(int i_emp, string codigoempleado,string lb,string lb2,string codquincena, double sueldo,string docref,string lbmas1,string apellido1,string apellido2,string nombre1,string nombre2)
		{
			int i,j;

            double diapromsueldo=0,valordiatrabajo=0;
			double valormsusplicencias=0;
			double sumasueldopromedio=0;
			this.esSuspension  = false;
			this.esIncapacidad = false;
	
			DataSet mquincenas = new DataSet();
			//Averiguar Forma de pago Incapacidades
			string formapagoInc= o_CNomina.CNOM_FORMAPAGOINCAP;   // DBFunctions.SingleData("select cnom_formapagoincap from dbxschema.cnomina");
			//Porcentaje Salario integral
            string porcInteg   = o_CNomina.CNOM_BASEPORCSALAINTEG; // DBFunctions.SingleData("SELECT cnom_baseporcsalainteg FROM DBXSCHEMA.cnomina");
                                                                   // TIPO SALARIO
            string tipoSalario = empleados.Tables[0].Rows[i_emp]["TSAL_SALARIO"].ToString();                                      // DBFunctions.SingleData("SELECT  TSAL_SALARIO FROM DBXSCHEMA.MEMPLEADO WHERE MEMP_CODIEMPL='"+codigoempleado+"'");
			double SalarioBase = Convert.ToDouble(empleados.Tables[0].Rows[i_emp]["MEMP_SUELACTU"].ToString());                  // Convert.ToDouble(DBFunctions.SingleData("SELECT memp_suelactu FROM DBXSCHEMA.MEMPLEADO WHERE MEMP_CODIEMPL='"+codigoempleado+"'"));
            string retorno     = (Math.Round(SalarioBase / 30,0)).ToString();                                                   // DBFunctions.SingleData("select memp_suelactu/30 from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");

            valordiatrabajo    = Double.Parse(retorno==string.Empty?"0":retorno);
            double valorMinimodia     = Math.Round(Convert.ToDouble(o_CNomina.CNOM_SALAMINIACTU.ToString()) / 30,0);
            if (formapagoInc   != "3")
            {
                //IBC
                //Sacar valor del dia promediado a 12 meses.
                diapromsueldo = 0;
                DateTime fechaprom = new DateTime();
                fechaprom = Convert.ToDateTime(lb.ToString());
                int anoPeriAnte = fechaprom.Year;
                int mesPeriAnte = fechaprom.Month;
                if (mesPeriAnte == 1)
                {
                    anoPeriAnte -= 1;
                    mesPeriAnte = 12;
                }
                else
                    mesPeriAnte -= 1;
                string fechapromedio = fechaprom.ToString("yyyy-MM-dd");
                DataSet valorsueldopromedio = new DataSet();
                string promsalar = @"SELECT SUM(D.DQUI_APAGAR),D.MEMP_CODIEMPL,M.MQUI_ANOQUIN,m.MQUI_MESQUIN,
                       CASE WHEN M.MQUI_MESQUIN<9 THEN CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-0' CONCAT CAST(MQUI_MESQUIN AS CHAR(2)) CONCAT '-' CONCAT '15' 
                            ELSE CAST(MQUI_ANOQUIN AS CHAR(4)) CONCAT '-' CONCAT CAST(MQUI_MESQUIN AS CHAR(2)) CONCAT '-' CONCAT '15' END AS FECHA 
                       FROM DBXSCHEMA.DQUINCENA D,DBXSCHEMA.MQUINCENAS M,dbxschema.pconceptonomina PCON 
                       WHERE D.MQUI_CODIQUIN=M.MQUI_CODIQUIN AND D.pcon_concepto=PCON.pcon_concepto AND PCON.tres_afec_eps = 'S'  
                       AND D.memp_codiempl='" + codigoempleado + @"'  AND M.MQUI_MESQUIN = "+ mesPeriAnte + @" and m.mqui_anoquin = "+ anoPeriAnte + @"   
                       GROUP BY  D.MEMP_CODIEMPL,M.MQUI_ANOQUIN,m.MQUI_MESQUIN ";
                DBFunctions.Request(valorsueldopromedio, IncludeSchema.NO, promsalar);

                if (valorsueldopromedio.Tables[0].Rows.Count != 0 && tipoSalario == "2")
                {
                    for (i = 0; i < valorsueldopromedio.Tables[0].Rows.Count; i++)
                    {
                        sumasueldopromedio += double.Parse(valorsueldopromedio.Tables[0].Rows[i][0].ToString());
                    }
                    if (o_CNomina.CNOM_QUINCENA == "2")
                    {
                        //JFSC 13062008 Validar si se tienen dias en la quincena anterior si es quincenal
                        if (peripago != "2")
                        {
                            double diasQuinAnt = this.calcularDiasLicQuincenaAnterior(lb.ToString(), codigoempleado);
                            diasexceptosauxtransp += diasQuinAnt;
                            if (peripago == "2")
                                diasdescdelsueldo += int.Parse(diasQuinAnt.ToString());
                        }
                    }
                    if (formapagoInc == "3")   // se paga con el salario básico actual
                        diapromsueldo = sueldo / 30;
                    else
                    {
                        sumasueldopromedio = sumasueldopromedio / valorsueldopromedio.Tables[0].Rows.Count;
                        diapromsueldo = sumasueldopromedio / 30;
                        valordiatrabajo = diapromsueldo;
                    }
                    //sueldopromediado=diapromsueldo*int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString());
                }
                else
                {
                    //sueldopromediado=valordiatrabajo*int.Parse(msusplicencias2.Tables[0].Rows[i][4].ToString());					
                    sumasueldopromedio = SalarioBase;
                    diapromsueldo = valordiatrabajo = SalarioBase / 30;
                    valordiatrabajo = diapromsueldo;
                }
                if (formapagoInc == "2")
                {
                    sumasueldopromedio = sumasueldopromedio / 3;
                    sumasueldopromedio = sumasueldopromedio * 2;
                    diapromsueldo = sumasueldopromedio / 30;
                }
            }
            else if (formapagoInc == "3")
            {
                if (tipoSalario == "1")
                {
                    sueldo = (sueldo * double.Parse(porcInteg)) / 100;
                    valordiatrabajo = sueldo / 30;
                    diapromsueldo = sueldo / 30;
                }
                else
                {
                    //Salario. Base
                    valordiatrabajo = sueldo / 30;
                    diapromsueldo = sueldo / 30;
                }
            }
            else if (formapagoInc == "4")  // se paga por el ultimo reporte a las EPSs
            {
                    int anoActual = Convert.ToUInt16(DBFunctions.SingleData(@"select mqui_anoquin from mquincenas where mqui_estado = 1").ToString());
                    int mesActual = Convert.ToUInt16(DBFunctions.SingleData(@"select mqui_mesquin from mquincenas where mqui_estado = 1").ToString());
                    int anoAnterior = anoActual;
                    int mesAnterior = mesActual - 1;
                    if (mesAnterior == 0)
                    {
                        anoAnterior -= 1;
                        mesAnterior = 12;
                    }
               
                    string ultimoIBC = DBFunctions.SingleData(@"select CASE WHEN year(memp_fechingreso) = "+ anoActual+ @" and month(memp_fechingreso) = " + mesActual + @" then sum(me.memp_suelactu) else sum(dqui_apagar - dqui_adescontar) end
                                        from mquincenas m, dquincena d, mempleado me, pconceptonomina p
                                        where d.mqui_codiquin = m.mqui_codiquin and mqui_anoquin = " + anoAnterior +@" and mqui_mesquin = "+ mesAnterior +@" and d.memp_codiempl = '2' 
                                        and d.pcon_concepto = p.pcon_concepto and TRES_afec_eps = 'S' and d.memp_codiempl = me.memp_codiempl
                                        group by memp_fechingreso;"); 
                    if (ultimoIBC == "") ultimoIBC = "0";
                    sumasueldopromedio = Convert.ToDouble(ultimoIBC.ToString());
                    sumasueldopromedio = Math.Round(sumasueldopromedio * 2 / 3,0);
                    diapromsueldo      = Math.Round(sumasueldopromedio / 30,0);
            }
            if (valorMinimodia > diapromsueldo)
                diapromsueldo = valorMinimodia; // NO se puede pagar menos del mínimo

            DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
			//Traigo el ultimo registro del maestro de Quincenas			
		
            //mirar si hay suspension,licencias o incapacidades entre la quincena exacta y de cuantos dias
			
			DataSet msusplicencias3= new DataSet();
            sumaLicenciasSuspPeriodo = 0;
		
            int DiasSusLicencias = 0;
            string sqlLicencias = @"Select M.pcon_concepto, M.memp_codiempl, CAST(M.msus_desde AS CHAR(10)), CAST(M.msus_hasta AS CHAR(10)), (M.msus_hasta-M.msus_desde)+1 AS DIAS, M.ttip_coditipo, P.pcon_signoliq, T.tdes_descripcion,
                                           P.pcon_nombconc,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima, P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva,ME.MEMP_periPAGO, p.pcon_factorliq
                                from DBXSCHEMA.MSUSPLICENCIAS M, pconceptonomina p,  tdesccantidad T, MEMPLEADO ME 
                                where M.memp_codiempl='" + codigoempleado + "' and  msus_desde <= '" + lb2.ToString() + "' and msus_hasta >= '" + lb.ToString() + "' and m.pcon_concepto = p.pcon_concepto and (P.pcon_desccant=T.tdes_cantidad) AND ME.memp_codiempl=M.memp_codiempl ";

            //   D = A PAGAR    H = A DESCONTAR

            string fechaIniQuin =lb.ToString();
            string fechaFinQuin = lb2.ToString();
            DBFunctions.Request(msusplicencias3, IncludeSchema.NO, sqlLicencias);
            for (i = 0; i < msusplicencias3.Tables[0].Rows.Count; i++)
            {
                int diasLic = Convert.ToInt32(msusplicencias3.Tables[0].Rows[i][4].ToString());
                if ((String.Compare(msusplicencias3.Tables[0].Rows[i][2].ToString(), fechaIniQuin.ToString()) <= 0) && (String.Compare(msusplicencias3.Tables[0].Rows[i][3].ToString(), fechaFinQuin.ToString()) > 0))
                {
                    if (diasLic >= 30 && (msusplicencias3.Tables[0].Rows[i][17].ToString() == "2" || msusplicencias3.Tables[0].Rows[i][17].ToString() == "4"))
                        diasLic = 30;                                                           // pago mensual
                    else
                        if (diasLic >= 15 && msusplicencias3.Tables[0].Rows[i][17].ToString() == "1") // pago quincenal
                            diasLic = 15;
                }
                else
                {
                    if (String.Compare(msusplicencias3.Tables[0].Rows[i][2].ToString(), fechaIniQuin.ToString()) < 0)
                        msusplicencias3.Tables[0].Rows[i][2] = fechaIniQuin; // si inicia antes del periodo
                    if (String.Compare(msusplicencias3.Tables[0].Rows[i][3].ToString(), fechaFinQuin.ToString()) > 0)
                        msusplicencias3.Tables[0].Rows[i][3] = fechaFinQuin; // si finaliza despues del periodo
                    TimeSpan ts = Convert.ToDateTime(msusplicencias3.Tables[0].Rows[i][3]) - Convert.ToDateTime(msusplicencias3.Tables[0].Rows[i][2]);
                    diasLic = ts.Days + 1;  // los dias de las licencias o incapacidades son Incluidos los dos topes
                }
                DateTime mesFeb = Convert.ToDateTime(lb2.ToString());
                DateTime finIncap = Convert.ToDateTime(msusplicencias3.Tables[0].Rows[i][3].ToString());
                if (mesFeb.Month == 02 && (mesFeb.Day == 28 || mesFeb.Day == 29) && (finIncap.Day == 28 || finIncap.Day == 29))
                {
                    if(mesFeb.Day == 28 && finIncap.Day == 28)
                        diasLic = diasLic + 2;
                    else
                        diasLic = diasLic + 1;
                }
                double factorLiq = Convert.ToDouble(msusplicencias3.Tables[0].Rows[i]["PCON_FACTORLIQ"].ToString());
                valormsusplicencias = valordiatrabajo * diasLic * factorLiq;
                DiasSusLicencias = DiasSusLicencias + diasLic;

			   esLicNoRemunerada = (msusplicencias3.Tables[0].Rows[i][5].ToString() == "1");
               esLicSiRemunerada = (msusplicencias3.Tables[0].Rows[i][5].ToString() == "2");
               esIncapacidad     = (msusplicencias3.Tables[0].Rows[i][5].ToString() == "3");
               esSuspension      = (msusplicencias3.Tables[0].Rows[i][5].ToString() == "4");
                
               if (esIncapacidad || esLicSiRemunerada)  // Se resta a los dias de pago y se pagan los dias de la incapacidad
                    diasdescdelsueldo += diasLic;
               if (esLicNoRemunerada || esSuspension) 
                  diasexceptosauxtransp += diasLic;
              
               if (esLicNoRemunerada || esSuspension)  //  se descuenta 
			   {
                    valormsusplicencias = diasLic * valordiatrabajo * factorLiq; // al sueldo actual
                    this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(valordiatrabajo.ToString("N")),0,Math.Round(valormsusplicencias, 0),msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][8].ToString());
					if (banderaNombre==0)
					{
                        this.ingresar_datos_datatable(codquincena, codigoempleado, msusplicencias3.Tables[0].Rows[i][0].ToString(), diasLic, double.Parse(valordiatrabajo.ToString("N")), 0, Math.Round(valormsusplicencias, 0), msusplicencias3.Tables[0].Rows[i][7].ToString(), 0, docref, apellido1, apellido2, nombre1, nombre2, msusplicencias3.Tables[0].Rows[i][8].ToString());
					//	banderaNombre+=1;
					}
					else
					{
                        this.ingresar_datos_datatable(codquincena, "", msusplicencias3.Tables[0].Rows[i][0].ToString(), diasLic, double.Parse(valordiatrabajo.ToString("N")), 0, Math.Round(valormsusplicencias, 0), msusplicencias3.Tables[0].Rows[i][7].ToString(), 0, docref, "", "", "", "", msusplicencias3.Tables[0].Rows[i][8].ToString());
					}
				    sumadescontado+=valormsusplicencias;
                    sumaLicenciasSuspPeriodo += valormsusplicencias;
			   }

               else if (esLicSiRemunerada || esIncapacidad) // se paga
			   {
                    if (esLicSiRemunerada)
                    {
                        if(tipoSalario == "3")
                             diapromsueldo = SalarioBase / 30;
                        else
                            diapromsueldo = sumasueldopromedio / 30;
                    }
                    valormsusplicencias = Math.Round(diasLic * diapromsueldo * factorLiq, 0);  // al sueldo promedio

                    this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, msusplicencias3.Tables[0].Rows[i][0].ToString(), diasLic, double.Parse(valordiatrabajo.ToString("N")), Math.Round(valormsusplicencias, 0), 0, msusplicencias3.Tables[0].Rows[i][7].ToString(), 0, docref, apellido1, apellido2, nombre1, nombre2, msusplicencias3.Tables[0].Rows[i][8].ToString());
					if (banderaNombre==0)
					{
                        this.ingresar_datos_datatable(codquincena, codigoempleado, msusplicencias3.Tables[0].Rows[i][0].ToString(), diasLic, double.Parse(valordiatrabajo.ToString("N")), Math.Round(valormsusplicencias, 0), 0, msusplicencias3.Tables[0].Rows[i][7].ToString(), 0, docref, apellido1, apellido2, nombre1, nombre2, msusplicencias3.Tables[0].Rows[i][8].ToString());
					//	banderaNombre+=1;
					}
					else
					{
                        this.ingresar_datos_datatable(codquincena, "", msusplicencias3.Tables[0].Rows[i][0].ToString(), diasLic, double.Parse(valordiatrabajo.ToString("N")), Math.Round(valormsusplicencias, 0), 0, msusplicencias3.Tables[0].Rows[i][7].ToString(), 0, docref, "", "", "", "", msusplicencias3.Tables[0].Rows[i][8].ToString());
					}
					sumapagado+=valormsusplicencias;
				}
					
				else
				{
					this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,"Error de incongruencia en susplic",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][8].ToString());
					if (banderaNombre==0)
					{
						this.ingresar_datos_datatable(codquincena,codigoempleado,"Error de incongruencia en susplic",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][8].ToString());
					//	banderaNombre+=1;
					}
					else
					{
						this.ingresar_datos_datatable(codquincena,"","Error de incongruencia en susplic",0,0,0,0,"0",0,docref,"","","","",msusplicencias3.Tables[0].Rows[i][8].ToString());
					}
					errores+=1;
				}
				//validacion
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][9].ToString(),"Novedad en Horas de descuento que afecta EPS? posible error..",Math.Round(valormsusplicencias, 0),0,0,0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][10].ToString(),"Novedad en Horas de descuento que afecta Horas extras? posible error..",0,0,0,0,0,0,0,Math.Round(valormsusplicencias, 0));											
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][11].ToString(),"Novedad en Horas de descuento que afecta Primas? posible error..",0,0,Math.Round(valormsusplicencias, 0),0,0,0,0,0);											
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][12].ToString(),"Novedad en Horas de descuento que afecta Vacaciones? posible error..",0,0,0,Math.Round(valormsusplicencias, 0),0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][13].ToString(),"Novedad en Horas de descuento que afecta Cesantias? posible error..",0,Math.Round(valormsusplicencias, 0),0,0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][14].ToString(),"Novedad en Horas de descuento que afecta Retencion en la fuente? posible error..",0,0,0,0,Math.Round(valormsusplicencias, 0),0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][15].ToString(),"Novedad en Horas de descuento que afecta Provisiones? posible error..",0,0,0,0,0,Math.Round(valormsusplicencias, 0),0,0);
				this.validacionafectaciones(codquincena,codigoempleado,msusplicencias3.Tables[0].Rows[i][0].ToString(),int.Parse(msusplicencias3.Tables[0].Rows[i][4].ToString()),double.Parse(diapromsueldo.ToString("N")),Math.Round(valormsusplicencias,0),0,msusplicencias3.Tables[0].Rows[i][7].ToString(),0,docref,apellido1,apellido2,nombre1,nombre2,msusplicencias3.Tables[0].Rows[i][6].ToString(),msusplicencias3.Tables[0].Rows[i][16].ToString(),"Novedad en Horas de descuento que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,Math.Round(valormsusplicencias, 0),0);
			}
		}
		
		protected void revisarvacaciones(string codquincena,string codempleado,int canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string lb,string lb2)
		{
			string concepto;
			int i=0,diasf,dias;
			
			double sueldopagado=0;
			//Traer todos los periodos de vacaciones para el empleado.
			DataSet vacaciones   = new DataSet();
			DataSet conceptovaca = new DataSet();
            DBFunctions.Request(vacaciones, IncludeSchema.NO, "select D.dvac_tiem,D.dvac_dine,D.dvac_fechinic,D.dvac_fechfinal from dbxschema.mvacaciones M,dbxschema.dvacaciones D where M.mvac_secuencia=D.mvac_secuencia and  M.memp_codiemp='" + codempleado + "' and d.dvac_tiem > 0");
			DBFunctions.Request(conceptovaca,IncludeSchema.NO,"select cnom_concvacacodi from dbxschema.cnomina ");
			concepto=conceptovaca.Tables[0].Rows[0][0].ToString();
			string descripcionconcepto=DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+concepto+"' ");
			DateTime iniquin = new DateTime();
			DateTime finquin = new DateTime();
			DateTime inivaca = new DateTime();
			DateTime finvaca = new DateTime();
			//asigno para comparar fechas despues
			iniquin=Convert.ToDateTime(lb);
			finquin=Convert.ToDateTime(lb2);
			
			//revisar desde k dia salio de vacas pa ver cuantos dias le desccuento en la quincena
			//revisar si entra en los dias de vacas pa ver cuantos dias le pago.
			
			for (i=0;i<vacaciones.Tables[0].Rows.Count;i++)
			{
				inivaca=Convert.ToDateTime(vacaciones.Tables[0].Rows[i][2].ToString());
				finvaca=Convert.ToDateTime(vacaciones.Tables[0].Rows[i][3].ToString());
				//Si el periodo de vacaciones esta en la quincena
				if (inivaca>=iniquin && finvaca<=finquin)
				{
					//si las vacaciones fueron dadas en tiempo
					if (vacaciones.Tables[0].Rows[i][0].ToString()!="")
					{
						sueldopagado=double.Parse(vacaciones.Tables[0].Rows[i][0].ToString())*valorevento;
						this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,int.Parse(vacaciones.Tables[0].Rows[i][0].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						if (banderaNombre==0)
						{
							this.ingresar_datos_datatable(codquincena,codempleado,concepto,int.Parse(vacaciones.Tables[0].Rows[i][0].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						//	banderaNombre+=1;
						}
						else
						{
							this.ingresar_datos_datatable(codquincena,"",concepto,int.Parse(vacaciones.Tables[0].Rows[i][0].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
						}
											
						diasvacaciones+=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString());
						sumapagado+=double.Parse(sueldopagado.ToString());
						diasexceptosauxtransp+=diasvacaciones;
					}
					//si son dadas en dinero OJO REVISAR QUE ESTA GRABANDO CON cero EN VEZ DE NULL.
					if (vacaciones.Tables[0].Rows[i][1].ToString()!="0")
					{
                        mensaje += "vacaciones en quincena dinero" + vacaciones.Tables[0].Rows[i][0].ToString() + " dias para el empleado  " + codempleado + " \\n";
						sueldopagado=double.Parse(vacaciones.Tables[0].Rows[i][1].ToString())*valorevento;
						this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,int.Parse(vacaciones.Tables[0].Rows[i][1].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						if (banderaNombre==0)
						{
							this.ingresar_datos_datatable(codquincena,codempleado,concepto,int.Parse(vacaciones.Tables[0].Rows[i][1].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						//	banderaNombre+=1;
						}
						else
						{
							this.ingresar_datos_datatable(codquincena,"",concepto,int.Parse(vacaciones.Tables[0].Rows[i][1].ToString()),valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
						}
						
						sumapagado+=double.Parse(sueldopagado.ToString());
					}
					acumuladoeps+=double.Parse(sueldopagado.ToString());
				}
				//Si el periodo de vacaciones inicia en la quincena pero se pasa de la quincena.
				if ((inivaca>=iniquin && inivaca<finquin) && finvaca>finquin)
				{
					TimeSpan diasfuera=finvaca-finquin;
					diasf=diasfuera.Days;
					diasf+=1;
					if (vacaciones.Tables[0].Rows[i][0].ToString()!="")
					{
						sueldopagado=(double.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf)*valorevento;
						dias=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf;
						this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						if (banderaNombre==0)
						{
							this.ingresar_datos_datatable(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						//	banderaNombre+=1;
						}
						else
						{
							this.ingresar_datos_datatable(codquincena,"",concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
						}
						diasvacaciones+=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf;
						sumapagado+=double.Parse(sueldopagado.ToString());
						diasexceptosauxtransp+=diasvacaciones;
					}
					
					//si son dadas en dinero
					if (vacaciones.Tables[0].Rows[i][1].ToString()!="")
					{
						sueldopagado=(double.Parse(vacaciones.Tables[0].Rows[i][1].ToString())-diasf)*valorevento;
						dias=int.Parse(vacaciones.Tables[0].Rows[i][1].ToString())-diasf;
						this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						if (banderaNombre==0)
						{
							this.ingresar_datos_datatable(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						//	banderaNombre+=1;
						}
						else
						{
							this.ingresar_datos_datatable(codquincena,"",concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
						}
											
						sumapagado+=double.Parse(sueldopagado.ToString());
					}
				}
				//Si el periodo de vacaciones inicia antes de la quincena pero termina en la quincena.
				if (inivaca<iniquin && (finvaca<=finquin && finvaca>iniquin))
				{
					TimeSpan diasfuera=iniquin-inivaca;
					diasf=diasfuera.Days;
					diasf+=1;
					if (vacaciones.Tables[0].Rows[i][0].ToString()!="")
					{
						sueldopagado=(double.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf)*valorevento;
						dias=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf;
						this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						if (banderaNombre==0)
						{
							this.ingresar_datos_datatable(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						//	banderaNombre+=1;
						}
						else
						{
							this.ingresar_datos_datatable(codquincena,"",concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
						}
											
						diasvacaciones+=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf;
						sumapagado+=double.Parse(sueldopagado.ToString());
						diasexceptosauxtransp+=diasvacaciones;
					}
					//si son dadas en dinero
					if (vacaciones.Tables[0].Rows[i][1].ToString()!="")
					{
						sueldopagado=(double.Parse(vacaciones.Tables[0].Rows[i][1].ToString())-diasf)*valorevento;
						dias=int.Parse(vacaciones.Tables[0].Rows[i][1].ToString())-diasf;
						this.ingresar_datos_datatableLiqFinal(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						if (banderaNombre==0)
						{
							this.ingresar_datos_datatable(codquincena,codempleado,concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
						//	banderaNombre+=1;
						}
						else
						{
							this.ingresar_datos_datatable(codquincena,"",concepto,dias,valorevento,Math.Round(sueldopagado,0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
						}
						sumapagado+=double.Parse(sueldopagado.ToString());
					}
				}
			}
		}

		protected int revisarvacaciones1(string codquincena,string codempleado,string docref,string lb,string lb2, string peripago )
		{
			int i=0;
			int diasretorno = 0;
			//Traer todos los periodos de vacaciones para el empleado.
			DataSet vacaciones = new DataSet();
			DataSet conceptovaca = new DataSet();
            DBFunctions.Request(vacaciones, IncludeSchema.NO, "select D.dvac_tiem,D.dvac_dine,D.dvac_fechinic,D.dvac_fechfinal from dbxschema.mvacaciones M,dbxschema.dvacaciones D where M.mvac_secuencia=D.mvac_secuencia and  M.memp_codiemp='" + codempleado + "' and d.dvac_tiem > 0 And D.dvac_fechfinal >= '"+ lb +"' ");
			DateTime iniquin = new DateTime();
			DateTime finquin = new DateTime();
			DateTime inivaca = new DateTime();
			DateTime finvaca = new DateTime();
            DateTime finvacaUltimo = new DateTime();  // para buscar el ultimo periodo de vacaciones 
            string lbh = o_CNomina.CNOM_ANO + "-";
            if (Convert.ToInt16(o_CNomina.CNOM_MES.ToString()) < 10) 
                lbh = lbh + "0";
            lbh = lbh + o_CNomina.CNOM_MES+"-"; 
			// si el empleado es quincenal y es segunda quincena, empieza en 16 del mes y ano vigente
            if (o_CNomina.CNOM_OPCIQUINOMENS == "1" && o_CNomina.CNOM_QUINCENA == "2" && peripago == "1")
                lbh = lbh + "16";
            else lbh = lbh + "01";  // defino el inicio del periodo a liquidar

            iniquin = Convert.ToDateTime(lbh);  
			finquin=Convert.ToDateTime(lb2);
			
			//revisar desde k dia salio de vacas pa ver cuantos dias le desccuento en la quincena
			//revisar si entra en los dias de vacas pa ver cuantos dias le pago.
			
			for (i=0;i<vacaciones.Tables[0].Rows.Count;i++)
			{
				inivaca=Convert.ToDateTime(vacaciones.Tables[0].Rows[i]["dvac_fechinic"].ToString());
				finvaca=Convert.ToDateTime(vacaciones.Tables[0].Rows[i]["dvac_fechfinal"].ToString());
				//Si el periodo de vacaciones esta en la quincena
                if (inivaca < iniquin)
				    inivaca = iniquin;
				if (finvaca > finquin)
                    finvaca = finquin;
                //Si el periodo de vacaciones inicia en la quincena pero se pasa de la quincena.
				//if ((inivaca>=iniquin && inivaca<finquin) && finvaca>finquin)
					//TimeSpan diasfuera=finvaca-finquin;
//					if(inivaca>=iniquin && inivaca<finquin)diasf+=1;
					if (vacaciones.Tables[0].Rows[i]["dvac_tiem"].ToString()!="")
				//Si el periodo de vacaciones inicia antes de la quincena pero termina en la quincena.
				{
                    if (finvaca > finvacaUltimo && finvaca >= iniquin) // solo analiza el ultimo periodo mas reciente de vacaciones 
					//					diasf=diasfuera.Days;
					//					diasf+=1;
					//					if (vacaciones.Tables[0].Rows[i][0].ToString()!="")
					//					{
					//						dias=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf;
					//						diasretorno+=int.Parse(vacaciones.Tables[0].Rows[i][0].ToString())-diasf;
					//					}
					//si las vacaciones fueron dadas en tiempo
					{
                       finvacaUltimo = finvaca;
						/*if(peripago=="1")
							diasretorno+=15;
						else
							diasretorno+=30;*/
                 //      diasretorno = finvaca.Day - iniquin.Day + 1;
                       diasretorno += (finvaca.Day - inivaca.Day + 1);
							//diasretorno+= finvaca.Day;
					}
				}
			}
			return diasretorno;
		}


		protected void validardiasdepago(int i, string fecha,string lb,string lb2,string codigoempleado,string concepto,string sueldomes,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string periodopago,string codquincena, int estado)
		{
            if (valid == 0)
            {
                numerodias = 0;
                diassueldopagado = 0;
                return;  // empleado no devenga dias ene ste periodo
            }
            diasvaca = 0;
			DataSet  afectacionessueldo= new DataSet();
			double   descSala=0;
			DBFunctions.Request(afectacionessueldo,IncludeSchema.NO,"select P.pcon_signoliq,P.tres_afec_eps,P.tres_afechoraextr,P.tres_afecprima,P.tres_afecvacacion,P.tres_afeccesantia,P.tres_afecretefte,P.tres_afecprovision,P.tres_afecliquiddefinitiva from dbxschema.cnomina C,dbxschema.pconceptonomina P where C.cnom_concsalacodi=P.pcon_concepto");
			TimeSpan diastrabajados = new TimeSpan();
			TimeSpan diasnotrabajados = new TimeSpan();
			DateTime fechaingresoempleado = new DateTime();
			DateTime fechainicioliquidacion = new DateTime(); 
			DateTime fechafinalliquidacion = new DateTime();
			DataSet  mquincenas = new DataSet();
            string conceptointegral = o_CNomina.CNOM_CONCSALAINT; // DBFunctions.SingleData("select cnom_concsalaint from dbxschema.cnomina");
            string conceptosalariosena = o_CNomina.CNOM_CONCSALASENA; // DBFunctions.SingleData("select cnom_concsalasena from dbxschema.cnomina");//Aqui hay un error que no se ha arreglado aun.
            double factorsalariosena;
            if (conceptosalariosena.Equals(""))
                factorsalariosena = 1;
            else
                factorsalariosena = Convert.ToDouble(DBFunctions.SingleData("select coalesce(pcon_factorliq,1) from dbxschema.pconceptonomina where pcon_concepto = '" + conceptosalariosena + "' "));
            if (empleados.Tables[0].Select("MEMP_CODIEMPL = '" + codigoempleado + "' AND TCON_CONTRATO = '5'").Length > 0) // SENA ETAPA LECTIVA se la pago el 50% del minimo
                factorsalariosena = Math.Round(factorsalariosena * 0.5, 2); // SENA ETAPA LECTIVA solo se paga la mitad de mínimo

            string tiposalario      = empleados.Tables[0].Rows[i][9].ToString(); // DBFunctions.SingleData("select tsal_salario  from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
            string tipocontrato     = empleados.Tables[0].Rows[i][12].ToString(); // DBFunctions.SingleData("select tcon_contrato from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
            string fechaFinContrato = empleados.Tables[0].Rows[i][11].ToString(); // DBFunctions.SingleData("select memp_fechfincontrato from dbxschema.mempleado where memp_codiempl='"+codigoempleado+"'");
            string descripcionconcepto = ViewState["descConceptoSalarioBasico"].ToString(); //DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='" + concepto + "' ");
            string porcInteg        = o_CNomina.CNOM_BASEPORCSALAINTEG; //DBFunctions.SingleData("SELECT cnom_baseporcsalainteg FROM DBXSCHEMA.cnomina");
			int dias;
            if (tiposalario=="1")
			{
				if(diasdescdelsueldo>0)
				{
					descSala=(((double.Parse(sueldomes)*double.Parse(porcInteg))/100)/30)*diasdescdelsueldo;
				}
			}
			
			double valordiatrabajo=double.Parse(sueldomes)/30;
			double sueldodias=0,sueldoquincena,sueldoapagarmes;
			string sueldopagado="";
			diassueldopagado=0;

            diasvaca = this.revisarvacaciones1(codquincena, codigoempleado, docref, lb, lb2, periodopago);

            //Averiguar cuanto se le pago en el periodo si tuvo vacaciones y se le pago salario.
         
            diassueldopagado = Convert.ToInt32(DBFunctions.SingleData("select coalesce(sum(dqui_CANTEVENTOS),0) from dbxschema.dquincena where mqui_codiquin=" + codquincena + " and pcon_concepto='" + concepto + "' and memp_codiempl='" + codigoempleado + "' ")); // and dqui_vacaciones <> 'V'
            int quincenanter = 0;
            if (tipoPeriodoPago == "2" && this.DDLQUIN.SelectedValue == "2" && o_CNomina.CNOM_OPCIQUINOMENS == "1")
            {
                quincenanter = Convert.ToInt32(DBFunctions.SingleData("select coalesce(m1.mqui_codiquin,0) from mquincenas m1, mquincenas m2 where m1.mqui_anoquin = m2.mqui_anoquin and m1.mqui_mesquin = m2.mqui_mesquin and m1.mqui_tpernomi = 1 and m2.mqui_codiquin = " + codquincena + " and m1.mqui_codiquin <> m2.mqui_codiquin;"));
                diassueldopagado += Convert.ToInt32(DBFunctions.SingleData("select coalesce(sum(dqui_CANTEVENTOS),0) from dbxschema.dquincena where mqui_codiquin=" + quincenanter + " and pcon_concepto='" + concepto + "' and memp_codiempl='" + codigoempleado + "'  "));
            }
            sueldoquincena  = valordiatrabajo * (15 - diasvaca - this.diasdescdelsueldo - diassueldopagado);
			sueldoapagarmes = valordiatrabajo * (30 - diasvaca - this.diasdescdelsueldo - diassueldopagado);
            if (sueldoquincena  < 0) sueldoquincena = 0;
            if (sueldoapagarmes < 0) sueldoapagarmes = 0;
			sueldoquincena  = Math.Abs(sueldoquincena);
			sueldoapagarmes = Math.Abs(sueldoapagarmes);

            if (tipoPeriodoPago == "3")// Jornal = cero dias se ingresan por novedad
            {
                sueldopagado = "0";
                numerodias = 0;
            }
            else if (tipoPeriodoPago == "1")//Quincenal
            {
                sueldopagado = sueldoapagarmes.ToString();
                numerodias = 15 - diasdescdelsueldo - diasvaca - diassueldopagado;

                sueldopagado = sueldoquincena.ToString();
                if (tiposalario != "1")
                {
                    if (numerodias == 0)
                        numerodias = 15 - diasdescdelsueldo - diasvaca - diassueldopagado;
                }
                else
                    numerodias = 15 - diasdescdelsueldo - diasvaca - diassueldopagado;
            }
            else if (this.DDLQUIN.SelectedValue == "1")//Mensual primera quincena
            {
                if (tipoPeriodoPago == "2")//Segunda quincena
                {
                    //valid = 0;
                }
                else
                {
                    sueldopagado = sueldoapagarmes.ToString();
                    numerodias = 30 - diasdescdelsueldo - diasvaca - diassueldopagado;
                }
            }
            else//Mensual segunda quincena
            {
                if (tipoPeriodoPago == "4")//Primera quincena
                {
                    sueldopagado = sueldoapagarmes.ToString();
                    numerodias = 30 - diasdescdelsueldo - diasvaca - diassueldopagado;
                }
                else
                {
                    //valid = 0;
                }
            }

            if (diasvaca > 15)
            {
                if (tipoPeriodoPago == "2")
                {
                    sueldopagado = sueldoapagarmes.ToString();
                    if (tiposalario != "1")
                    {
                        if (numerodias == 0)
                            numerodias = 30 - diasdescdelsueldo - diasvaca - diassueldopagado;
                    }
                    else
                        numerodias = 30 - diasvaca - diasdescdelsueldo - diassueldopagado;
                    if (peripago == "2")
                        numerodias = 30 - diasvaca - diasdescdelsueldo - diassueldopagado;
                }
            }
            else
            {
                if (tipoPeriodoPago == "2")
                {

                    if (tiposalario != "1")
                    {
                        if (numerodias == 0)
                            numerodias = 15 - diasdescdelsueldo - diasvaca - diassueldopagado;
                    }
                    else
                        numerodias = 15 - diasvaca - diassueldopagado;
                    if (peripago == "2")
                    {
                        sueldopagado = sueldoapagarmes.ToString();
                        numerodias = 30 - diasdescdelsueldo - diasvaca - diassueldopagado;
                    }
                    else
                    {
                        sueldopagado = sueldoquincena.ToString();
                    }
                }
            }


            //if (lbtipopago.Text == "QUINCENAL")
            //{
            //    if (o_CNomina.CNOM_QUINCENA == "1")
            //    {
            //        sueldopagado = sueldoquincena.ToString();
            //        if (tiposalario != "1")
            //        {
            //            if (numerodias == 0)
            //                numerodias = 15 - diasdescdelsueldo - diasvaca - diassueldopagado;
            //        }
            //        else
            //            numerodias = 15 - diasvaca - diassueldopagado;
            //    }
            //    if (peripago == "2")
            //    {
            //        sueldopagado = sueldoapagarmes.ToString();
            //        numerodias = 30 - diasvaca - diassueldopagado;
            //    }

            //    if (diasvaca > 15)
            //    {
            //        if (o_CNomina.CNOM_QUINCENA == "2")
            //        {
            //            sueldopagado = sueldoapagarmes.ToString();
            //            if (tiposalario != "1")
            //            {
            //                if (numerodias == 0)
            //                    numerodias = 30 - diasdescdelsueldo - diasvaca - diassueldopagado;
            //            }
            //            else
            //                numerodias = 30 - diasvaca - diassueldopagado;
            //            if (peripago == "2")
            //                numerodias = 30 - diasvaca - diasdescdelsueldo - diassueldopagado;
            //        }
            //    }
            //    else
            //    {
            //        if (o_CNomina.CNOM_QUINCENA == "2")
            //        {
            //            //sueldopagado=sueldoapagarmes.ToString();
            //            //si el empleado es mensual pero el periodo pago es quincenal

            //            if (tiposalario != "1")
            //            {
            //                if (numerodias == 0)
            //                    numerodias = 15 - diasdescdelsueldo - diasvaca - diassueldopagado;
            //            }
            //            else
            //                numerodias = 15 - diasvaca - diassueldopagado;
            //            if (peripago == "2")
            //            {
            //                sueldopagado = sueldoapagarmes.ToString();
            //                numerodias = 30 - diasvaca - diassueldopagado;
            //            }
            //            else
            //            {
            //                sueldopagado = sueldoquincena.ToString();
            //            }
            //        }
            //    }
            //    /*if (periodopago=="3")
            //    {
            //        sueldopagado="0";
            //        numerodias=0;
									
            //    }*/

            //}


			if(numerodias!=0)this.diastrabajados=numerodias;
            diastrabajadosTotalmes = numerodias;
			
			if (lbtipopago.Text=="MENSUAL")
			{
				sueldopagado=sueldoapagarmes.ToString();
				if(tiposalario!="1")
				{
					if (numerodias==0)
						numerodias=30-diasdescdelsueldo - diasvaca- diassueldopagado;
				}
				else
                    numerodias = 30 - diasdescdelsueldo - diasvaca - diassueldopagado;
				if (periodopago=="3")
				{
					sueldopagado="0";
					numerodias=0;
				}
                if (numerodias < 0) numerodias = 0;
				this.diastrabajados=numerodias;
			}			
			//Traigo el ultimo registro del maestro de Quincenas
			DBFunctions.Request(mquincenas,IncludeSchema.NO,"select max(mqui_codiquin) from mquincenas");
									
			//validar la fecha de ingreso
			fechaingresoempleado=Convert.ToDateTime(fecha);
			fechainicioliquidacion=Convert.ToDateTime(lb.ToString());
			fechafinalliquidacion=Convert.ToDateTime(lb2.ToString());
			
			if(tiposalario=="1")
			{
                descripcionconcepto = ViewState["descConceptoSalarioIntegral"].ToString(); // DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+conceptointegral+"' ");
				concepto=conceptointegral;
			}
			if(tipocontrato=="3" || tipocontrato == "5")  // los practicantes del SENA (PRODUCTIVO=3, LECTIVO=5)
            {
				descripcionconcepto= ViewState["descConceptoSalarioSena"].ToString(); //DBFunctions.SingleData("select pcon_nombconc from dbxschema.pconceptonomina where pcon_concepto='"+conceptosalariosena+"' ");
                concepto =conceptosalariosena;
                valordiatrabajo = Math.Round(valordiatrabajo * factorsalariosena,2) ; // se recalcual por el factor que se pague a los aprendices.
			}
		
			
			//si ingreso antes de la fecha de inicio de liquidacion
			if (fechaingresoempleado< fechainicioliquidacion)
			{
				//			    diasvaca = this.revisarvacaciones1(codquincena,codigoempleado,docref,lb,lb2);
				//				this.revisarvacaciones(codquincena,codigoempleado,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,lb,lb2);

				//insertar pago de sueldo

                if (fechaingresoempleado.Month != fechafinalliquidacion.Month || fechaingresoempleado.Year != fechafinalliquidacion.Year || lbtipopago.Text == "QUINCENAL")
				{
                    diasexceptosauxtransp +=  diassueldopagado;  // no desconatar los dias de vacaciones porque slo se toman los disa trabajandos en el metodo de liquidar subsidio transporte
                    //diasexceptosauxtransp+= diasvaca + diasdescdelsueldo;
					if(tiposalario=="1")
					{
						sueldopagado=((numerodias*valordiatrabajo)-descSala).ToString();
					}
					else
						sueldopagado=Math.Round(numerodias*valordiatrabajo,0).ToString();
				}
				else
				{
					diastrabajados=(fechafinalliquidacion-fechainicioliquidacion);
					TimeSpan treinta  = new TimeSpan(30,0,0,0);
					TimeSpan diastrabajadosmes=(fechafinalliquidacion-fechaingresoempleado);
					int numerodiasmes = int.Parse(diastrabajadosmes.Days.ToString())+1;
					numerodias=int.Parse(diastrabajados.Days.ToString())+1-diasvaca;
					//jfsc 15042008 adicionar dispara febrero
					switch(fechafinalliquidacion.Day)
					{
						case 28:
							numerodias=numerodias+2;
							numerodiasmes+=2;
							break;
						case 29:
							numerodias=numerodias+1;
							numerodiasmes+=1;
							break;
						default:
							break;
					}
					//diasnotrabajados=treinta-new TimeSpan();
					sueldopagado=Math.Round(numerodias*valordiatrabajo,0).ToString();
					//diasexceptosauxtransp+=double.Parse(diasnotrabajados.Days.ToString());
					diasexceptosauxtransp+=30-numerodiasmes;
				}
                if (numerodias != 0 || double.Parse(sueldopagado) != 0)
                {
                    this.ingresar_datos_datatableLiqFinal(codquincena, codigoempleado, concepto, numerodias, double.Parse(valordiatrabajo.ToString("n")), Math.Round(double.Parse(sueldopagado), 0), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                    if (banderaNombre == 0)
                    {
                        this.ingresar_datos_datatable(codquincena, codigoempleado, concepto, numerodias, double.Parse(valordiatrabajo.ToString("n")), Math.Round(double.Parse(sueldopagado), 0), 0, "PESOS M/CTE", 0, docref, apellido1, apellido2, nombre1, nombre2, descripcionconcepto);
                    //    banderaNombre += 1;
                    }
                    else
                    {
                        this.ingresar_datos_datatable(codquincena, "", concepto, numerodias, double.Parse(valordiatrabajo.ToString("n")), Math.Round(double.Parse(sueldopagado), 0), 0, "PESOS M/CTE", 0, docref, "", "", "", "", descripcionconcepto);
                    }
                }
				
				sumapagado+=double.Parse(sueldopagado.ToString());
				
				//diasexceptosauxtransp+=diasvacaciones;
				
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][1].ToString(),"Salario credito que afecta EPS? posible error..",double.Parse(sueldopagado),0,0,0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][2].ToString(),"Salario credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(sueldopagado));
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][3].ToString(),"Salario credito que afecta Primas? posible error..",0,0,double.Parse(sueldopagado),0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][4].ToString(),"Salario credito que afecta Vacaciones? posible error..",0,0,0,double.Parse(sueldopagado),0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][5].ToString(),"Salario credito que afecta Cesantias? posible error..",0,double.Parse(sueldopagado),0,0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][6].ToString(),"Salario credito que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(sueldopagado),0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][7].ToString(),"Salario credito que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(sueldopagado),0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][8].ToString(),"Salario credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(sueldopagado),0);

				diastrabajados=(fechafinalliquidacion-fechaingresoempleado);
				//if(numerodias!=0)this.diastrabajados=numerodias;
				/*if(diastrabajados.Days<30)this.diastrabajados=diastrabajados.Days+1;
				else this.diastrabajados=30;*/
			}
				
			if (fechaingresoempleado>= fechainicioliquidacion && fechaingresoempleado<=fechafinalliquidacion)
			{
				//ingreso despues de la fecha de inicio de pago,pagarle los dias que trabajo.
                diastrabajados = (fechafinalliquidacion - fechaingresoempleado);
				diasnotrabajados=(fechafinalliquidacion-fechainicioliquidacion)-(fechafinalliquidacion-fechaingresoempleado);
				
			//	diasexceptosauxtransp+=double.Parse(diasnotrabajados.Days.ToString());
			
				prueba.Text=diastrabajados.Days.ToString();
                dias = int.Parse(prueba.Text) + 1 - diasvaca - diasdescdelsueldo;  // resta las vacaciones y las incapacidades ;
                switch (fechafinalliquidacion.Day)
				{
					case 28:
						numerodias=dias+2;
						break;
					case 29:
						numerodias=dias+1;
						break;
					default:
						numerodias=dias;
						break;
				}
				//numerodias=dias;   //arreglo Julian-Hector
				if(numerodias!=0)this.diastrabajados=numerodias;
                diastrabajadosTotalmes = numerodias;
				//if(diastrabajados.Days<=30)this.diastrabajados=diastrabajados.Days+1;
				//else this.diastrabajados=30;

				if(tiposalario=="1")
				{
					sueldopagado=((numerodias*valordiatrabajo)-descSala).ToString();

				}
				else
					sueldopagado=(numerodias*valordiatrabajo).ToString();
				this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
				if (banderaNombre==0)
				{
					this.ingresar_datos_datatable(codquincena,codigoempleado,concepto,numerodias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
				//	banderaNombre+=1;
				}
				else
				{
					this.ingresar_datos_datatable(codquincena,"",concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,"","","","",descripcionconcepto);
				}
				sumapagado+=double.Parse(sueldopagado.ToString());
				//sumapagado+=sueldodias;
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][1].ToString(),"Salario credito que afecta EPS? posible error..",double.Parse(sueldopagado),0,0,0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][2].ToString(),"Salario credito que afecta Horas extras? posible error..",0,0,0,0,0,0,0,double.Parse(sueldopagado));
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][3].ToString(),"Salario credito que afecta Primas? posible error..",0,0,double.Parse(sueldopagado),0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][4].ToString(),"Salario credito que afecta Vacaciones? posible error..",0,0,0,double.Parse(sueldopagado),0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][5].ToString(),"Salario credito que afecta Cesantias? posible error..",0,double.Parse(sueldopagado),0,0,0,0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][6].ToString(),"Salario credito que afecta Retencion en la fuente? posible error..",0,0,0,0,double.Parse(sueldopagado),0,0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][7].ToString(),"Salario credito que afecta Provisiones? posible error..",0,0,0,0,0,double.Parse(sueldopagado),0,0);
				this.validacionafectaciones(codquincena,codigoempleado,concepto,dias,double.Parse(valordiatrabajo.ToString("n")),Math.Round(double.Parse(sueldopagado),0),0,"PESOS M/CTE",0,docref,apellido1,apellido2,nombre1,nombre2,afectacionessueldo.Tables[0].Rows[0][0].ToString(),afectacionessueldo.Tables[0].Rows[0][8].ToString(),"Salario credito que afecta Liquidacion Definitiva? posible error..",0,0,0,0,0,0,double.Parse(sueldopagado),0);
			}
			
			if (fechaingresoempleado> fechafinalliquidacion)
			{
				//ingreso despues de la fecha de finalizacion de pago, error en el ingreso.
                Utils.MostrarAlerta(Response, "Fecha Ingreso de Empleado " + codigoempleado + " Erronea..");
				this.ingresar_datos_datatableLiqFinal(codquincena,codigoempleado,"Error Fecha de ingreso futura",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
				if (banderaNombre==0)
				{
					this.ingresar_datos_datatable(codquincena,codigoempleado,"Error Fecha de ingreso futura",0,0,0,0,"0",0,docref,apellido1,apellido2,nombre1,nombre2,descripcionconcepto);
				//	banderaNombre+=1;
				}
				else
				{
					this.ingresar_datos_datatable(codquincena,"","Error Fecha de ingreso futura",0,0,0,0,"0",0,docref,"","","","",descripcionconcepto);
				}
				errores+=1;
			}
		}
		
		/*JFSC 
		 * Método que calcula los dias que se han tenido de vacaciones por un periodo.
		 * mes: el mes a evaluar
		 * anio: el año a evaluar
		 * periodo: el periodo a evaluar
		 * codempleado: el código del empleado
		 * tipoPeriodo: el tipo periodo: 1.mensual o 2.quincenal
		 * */
		public int calcularDiasVacacionesPeriodo(int mes, int anio, int periodo, string codempleado, int tipoPeriodo)
		{
			int retorna=0;
			string fechaini, fechafin;
			int diaini=0, diafin=0;
			DateTime fechainicial,fechafinal,fechainicialvac,fechafinalvac;
			fechainicialvac=new DateTime();
			fechafinalvac=new DateTime();
			fechafin="";
			fechafinal=new DateTime();
			switch (tipoPeriodo)
			{
				//Caso periodo pago mensual
				case 1:
					diaini=1;
					if(mes!=2)
					{
						diafin=30;
						fechafinal=new DateTime(anio,mes,30);
						fechafin=fechafinal.ToString("yyyy-MM-dd");
					}
					else
					{
						diafin=30;
						fechafinal=new DateTime(anio,mes,28);
						fechafin=fechafinal.ToString("yyyy-MM-dd");
					}
					break;
				//Caso periodo pago quincenal
				case 2:
				switch(periodo)
				{
					//Primera quincena
					case 1:
						diaini=1;
						fechafinal=new DateTime(anio,mes,15);
						fechafin=fechafinal.ToString("yyyy-MM-dd");
						diafin=15;
						break;
					//Segunda quincena
					case 2:
						diaini=16;
						if(mes!=2)
						{
							diafin=30;
							fechafinal=new DateTime(anio,mes,30);
							fechafin=fechafinal.ToString("yyyy-MM-dd");
						}
						else
						{
							diafin=30;
							fechafinal=new DateTime(anio,mes,28);
							fechafin=fechafinal.ToString("yyyy-MM-dd");
						}
						break;
				}
					break;
					default:
						diafin=0;
						fechafinal=new DateTime();
						fechafin="";
						break;
			}			
			fechainicial=new DateTime(anio,mes,diaini);
			fechaini=fechainicial.ToString("yyyy-MM-dd");
			string query=@"select * from dbxschema.dvacaciones 
							where mvac_secuencia in (select mvac_secuencia 
									from dbxschema.mvacaciones 
									where memp_codiemp='"+codempleado+@"')
						and ('"+fechaini+@"' between dvac_fechinic and dvac_fechfinal 
						or '"+fechafin+@"' between dvac_fechinic and dvac_fechfinal)
                        and dvac_tiem > 0";
			DataSet vacaciones= new DataSet();
			DBFunctions.Request(vacaciones,IncludeSchema.NO,query);
			for(int i=0;i<vacaciones.Tables[0].Rows.Count;i++)
			{
				fechainicialvac = Convert.ToDateTime(vacaciones.Tables[0].Rows[i]["dvac_fechinic"].ToString());
				fechafinalvac   = Convert.ToDateTime(vacaciones.Tables[0].Rows[i]["dvac_fechfinal"].ToString());			
				//debe validar el intervalo en que se encuentra
				if(fechafinalvac>=fechainicial && fechafinalvac<=fechafinal)
				{
					//dos casos: fecha inicial de vacaciones dentro y fuera del intervalo
					if(fechainicialvac>=fechainicial && fechainicialvac<=fechafinal)
					{
						retorna+=fechafinalvac.Day-fechainicialvac.Day;
					}
					else
					{
						retorna+=fechafinalvac.Day;
					}
				}
				else
				{
					retorna+=diafin-fechainicialvac.Day;
				}
			}
			return retorna;
		}

		protected void ingresar_datos_datatable(string codquincena,string codempleado,string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string descripcionconcepto)
		{
			DataRow fila = DataTable1.NewRow();
			fila["COD QUINCENA"]=codquincena;
			fila["CODIGO"]=codempleado;
            fila["NOMBRE"] = apellido1 + " " + apellido2 + " " + nombre1 + " " + nombre2;
			fila["CONCEPTO"]=concepto;
			fila["DESCRIPCION"]=descripcionconcepto;
			fila["CANT EVENTOS"]=canteventos;
			//fila["VALOR EVENTO"]=valorevento;
			fila["A PAGAR"]=Math.Round(apagar,0);
			fila["A DESCONTAR"]=Math.Round(adescontar,0);
			//fila["TIPO EVENTO"]=descripcion;
			fila["SALDO"]=saldo;
			//fila["DOC REFERENCIA"]=docref; 
			DataTable1.Rows.Add(fila);
			    
			DataGrid2.DataSource = DataTable1;
			DataGrid2.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DataGrid2);
			//DataGrid2.Items[1].BackColor = Color.Red;
			DatasToControls.JustificacionGrilla(DataGrid2,DataTable1);
		}

		protected void ingresar_datos_datatableLiqFinal(string codquincena,string codempleado,string concepto,double canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2,string descripcionconcepto)
		{
			DataRow fila = DataTableLiqFinal.NewRow();
			fila["COD QUINCENA"]=codquincena;
			fila["CODIGO"]=codempleado;
            fila["NOMBRE"] = apellido1 + " " + apellido2 + " " + nombre1 + " " + nombre2;
			fila["CONCEPTO"]=concepto;
			fila["DESCRIPCION"]=descripcionconcepto;
			fila["CANT EVENTOS"]=canteventos;
			fila["VALOR EVENTO"]=Math.Round(valorevento,0);
			fila["A PAGAR"]=Math.Round(apagar,0);
			fila["A DESCONTAR"]=Math.Round(adescontar,0);
			fila["TIPO EVENTO"]=descripcion;
			fila["SALDO"]=saldo;
			fila["DOC REFERENCIA"]=docref; 
			DataTableLiqFinal.Rows.Add(fila);
			 
			//dgprueba.DataSource = DataTableLiqFinal;
			//dgprueba.DataBind();
			//DatasToControls.Aplicar_Formato_Grilla(dgprueba);
			
			//DatasToControls.JustificacionGrilla(DataGrid2,DataTable1);
		}
		
		/*
		protected void ingresar_datos_datatableapropiaciones(string codquincena,string codempleado,string concepto,int canteventos,double valorevento,double apagar,double adescontar,string descripcion,double saldo,string docref,string apellido1,string apellido2,string nombre1,string nombre2)
			 {
				DataRow fila = DataTable4.NewRow();
				fila["COD QUINCENA"]=codquincena;
				fila["CODIGO"]=codempleado;
				fila["NOMBRE"]=apellido1+" "+apellido2+" "+nombre1+" "+nombre2;
				fila["CONCEPTO"]=concepto;
				fila["CANT EVENTOS"]=canteventos;
				fila["VALOR EVENTO"]=valorevento;
				fila["A PAGAR"]=apagar;
				fila["A DESCONTAR"]=adescontar;
				fila["TIPO EVENTO"]=descripcion;
				fila["SALDO"]=saldo;
				fila["DOC REFERENCIA"]=docref; 
				DataTable4.Rows.Add(fila);
			    
				//DataGrid2.DataSource = DataTable1;
				//DataGrid2.DataBind();
				//DatasToControls.Aplicar_Formato_Grilla(DataGrid2);
			 }
		*/
		
		
		protected void ingresar_datos_empleadosdiferenteperipago(string codempleado,string apellido1,string apellido2,string nombre1,string nombre2,string periodopago)
		{
			DataRow fila = DataTable2.NewRow();
            fila["EMPLEADO"] = codempleado + " " + apellido1 + " " + apellido2 + " " + nombre1 + " " + nombre2;
			fila["PERIODO DE PAGO"]=periodopago;
			DataTable2.Rows.Add(fila);
			DataGrid1.DataSource = DataTable2;
			DataGrid1.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DataGrid1);
		}
		
		protected void ingresar_datos_empleadosensaldorojo(string codempleado,string apellido1,string apellido2,string nombre1,string nombre2,double devengados, double descuentos, double saldo )
		{
			DataRow fila = DataTable3.NewRow();
            fila["EMPLEADO"]  = codempleado + " " + apellido1 + " " + apellido2 + "  " + nombre1 + "  " + nombre2;
			fila["DEVENGADOS"]=devengados;
			fila["DESCUENTOS"]=descuentos;
			fila["SALDO"]=saldo;
			DataTable3.Rows.Add(fila);
			DataGrid3.DataSource = DataTable3;
			DataGrid3.DataBind();
			DatasToControls.Aplicar_Formato_Grilla(DataGrid3);
			DatasToControls.JustificacionGrilla(DataGrid3,DataTable3);
		}
			
		protected void preparargrilla_empleadosensaldorojo()
		{
			DataTable3 = new DataTable();
			DataTable3.Columns.Add(new DataColumn("EMPLEADO",System.Type.GetType("System.String")));
			DataTable3.Columns.Add(new DataColumn("DEVENGADOS",System.Type.GetType("System.Double")));
			DataTable3.Columns.Add(new DataColumn("DESCUENTOS",System.Type.GetType("System.Double")));
			DataTable3.Columns.Add(new DataColumn("SALDO",System.Type.GetType("System.Double")));
		}
		
		protected void preparargrilla_empleadospago()
		{
			DataTable1 = new DataTable();
			DataTable1.Columns.Add(new DataColumn("COD QUINCENA",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CONCEPTO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("CANT EVENTOS",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("VALOR EVENTO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("A PAGAR",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("A DESCONTAR",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("TIPO EVENTO",System.Type.GetType("System.String")));
			DataTable1.Columns.Add(new DataColumn("SALDO",System.Type.GetType("System.Double")));
			DataTable1.Columns.Add(new DataColumn("DOC REFERENCIA",System.Type.GetType("System.String")));
		}

		protected void preparargrilla_LiqFinal()
		{
			DataTableLiqFinal = new DataTable();
			DataTableLiqFinal.Columns.Add(new DataColumn("COD QUINCENA",System.Type.GetType("System.String")));
			DataTableLiqFinal.Columns.Add(new DataColumn("CODIGO",System.Type.GetType("System.String")));
			DataTableLiqFinal.Columns.Add(new DataColumn("NOMBRE",System.Type.GetType("System.String")));
			DataTableLiqFinal.Columns.Add(new DataColumn("CONCEPTO",System.Type.GetType("System.String")));
			DataTableLiqFinal.Columns.Add(new DataColumn("DESCRIPCION",System.Type.GetType("System.String")));
			DataTableLiqFinal.Columns.Add(new DataColumn("CANT EVENTOS",System.Type.GetType("System.Double")));
			DataTableLiqFinal.Columns.Add(new DataColumn("VALOR EVENTO",System.Type.GetType("System.Double")));
			DataTableLiqFinal.Columns.Add(new DataColumn("A PAGAR",System.Type.GetType("System.Double")));
			DataTableLiqFinal.Columns.Add(new DataColumn("A DESCONTAR",System.Type.GetType("System.Double")));
			DataTableLiqFinal.Columns.Add(new DataColumn("TIPO EVENTO",System.Type.GetType("System.String")));
			DataTableLiqFinal.Columns.Add(new DataColumn("SALDO",System.Type.GetType("System.Double")));
			DataTableLiqFinal.Columns.Add(new DataColumn("DOC REFERENCIA",System.Type.GetType("System.String")));
		}
		
		protected void preparargrilla_empleadosdiferenteperipago()
		{
			DataTable2 = new DataTable();
			DataTable2.Columns.Add(new DataColumn("EMPLEADO",System.Type.GetType("System.String")));
			DataTable2.Columns.Add(new DataColumn("PERIODO DE PAGO",System.Type.GetType("System.String")));
		}
		
		protected void Page_Load(object Sender,EventArgs e)
		{
            //Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            //Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            //Response.Cache.SetNoStore();

            //BTNCONFIRMACION.Attributes.Add("language", "javascript");
            //BTNCONFIRMACION.Attributes.Add("onclick", "return confirm('Esta a punto de realizar la liquidacion definitiva. Tenga en cuenta que si lo hace, ya no habra marcha atras. Esta seguro de que desea realizar la liquidacion definitiva?');");

			//Nombre de la empresa para validar empleados jornal
			empresa=DBFunctions.SingleData("Select cemp_nombre from DBXSCHEMA.CEMPRESA");
			empresa=empresa.Trim();
		//	cnomina= new DataSet();
			string query=@"select cnom_ano,
								cnom_mes,
								cnom_quincena,
								cnom_concsalacodi,
								cnom_concsubtcodi,
								cnom_substranperinomi,
								cnom_substransactu,
								truncate(decimal(cnom_substransactu/2),4) as transactu2,
								cnom_conchenocodi,
								cnom_epsperinomi,
								cnom_pagomenstper as periodopagomensual,
								cnom_opciquinomens as quinomes,
								cnom_topepagoauxitran,
								cnom_salaminiactu,
								cnom_baseporcsalainteg,
								cnom_concrftecodi,
								cnom_retefeteperinomi,
								cnom_salaminiactu,
								cnom_topepagoauxitran,
								cnom_concfondsolipens,
								cnom_porcfondsolipens,
								cnom_topepagofondsolipens,
								cnom_concepscodi,
								cnom_concfondcodi,
								cnom_porcfondempl,
								cnom_porcepsempl 
						from dbxschema.cnomina";
	//		DBFunctions.Request(cnomina,IncludeSchema.NO,query);					
			//this.calcularDiasVacacionesPeriodo("2",2008,1,"031",1);
			if(!IsPostBack)
			{
				Session.Clear();
				liquidador= new DataSet();				
				cnomina2=new DataSet();
				btnGenInforme.Visible=false;
				string quincena;
				string mes;
				string ano;
				string tipopago;				
				DBFunctions.Request(cnomina2,IncludeSchema.NO,"SELECT CNOM_NOMBPAGA,PANO_DETALLE,TMES_NOMBRE,TPER_DESCRIPCION,TOPCI_DESCRIPCION FROM DBXSCHEMA.PANO P, DBXSCHEMA.CNOMINA C,DBXSCHEMA.TMES  T,DBXSCHEMA.TPERIODONOMINA N,DBXSCHEMA.TOPCIQUINOMES O WHERE C.CNOM_ANO=P.PANO_ANO AND C.CNOM_MES=T.TMES_MES AND C.CNOM_QUINCENA=N.TPER_PERIODO AND C.CNOM_OPCIQUINOMENS=O.TOPCI_PERIODO");
				
				if (cnomina2.Tables[0].Rows.Count==0)
				{
                    Utils.MostrarAlerta(Response, "Estimado Usuario no ha configurado su nomina aun..");
					consulta.Visible=false;
				}
				else 
				{
					quincena=cnomina2.Tables[0].Rows[0][3].ToString();
					mes=cnomina2.Tables[0].Rows[0][2].ToString();
					ano=cnomina2.Tables[0].Rows[0][1].ToString();
					tipopago=cnomina2.Tables[0].Rows[0][4].ToString();
                    DBFunctions.Request(liquidador, IncludeSchema.NO, "SELECT MNIT_NOMBRES ||' '|| COALESCE(MNIT_NOMBRE2,''),MNIT_APELLIDOS||' '|| COALESCE(MNIT_APELLIDO2,'') FROM MNIT WHERE MNIT_NIT='" + cnomina2.Tables[0].Rows[0][0].ToString() + "'");
					if (tipopago=="MENSUAL" && quincena=="Primera Quincena")
					{
						DBFunctions.NonQuery("update cnomina set cnom_quincena=2");
					}
					DatasToControls bind = new DatasToControls();
					bind.PutDatasIntoDropDownList(DDLQUIN,"SELECT TPER_PERIODO, TPER_DESCRIPCION FROM TPERIODONOMINA");
					DatasToControls.EstablecerDefectoDropDownList(DDLQUIN,quincena);
					bind.PutDatasIntoDropDownList(DDLMES,"Select TMES_MES, TMES_NOMBRE from TMES");
					DatasToControls.EstablecerDefectoDropDownList(DDLMES,mes);
					bind.PutDatasIntoDropDownList(DDLANO,"SELECT PANO_ANO, PANO_DETALLE FROM PANO");
					DatasToControls.EstablecerDefectoDropDownList(DDLANO,ano);
					lbtipopago.Text=tipopago;
					lbliquidador.Text=liquidador.Tables[0].Rows[0][0].ToString()+" "+liquidador.Tables[0].Rows[0][1].ToString();
						
					//DataSet cnom= new DataSet();
					//DBFunctions.Request(cnom,IncludeSchema.NO,"select cnom_ano,cnom_mes,cnom_quincena from dbxschema.cnomina");	
					string numquincenaactual=DBFunctions.SingleData("select mqui_codiquin from dbxschema.mquincenas where mqui_anoquin="+o_CNomina.CNOM_ANO+" and mqui_mesquin="+o_CNomina.CNOM_MES+" and mqui_tpernomi="+o_CNomina.CNOM_QUINCENA+"");
					string estadoquincena=DBFunctions.SingleData("select mqui_estado from dbxschema.mquincenas where mqui_anoquin="+o_CNomina.CNOM_ANO+" and mqui_mesquin="+o_CNomina.CNOM_MES+" and mqui_tpernomi="+o_CNomina.CNOM_QUINCENA+"");
					if (numquincenaactual=="")
					{
						numquincenaactual="0";
					}
						
					if (estadoquincena=="1")
					{
						DBFunctions.NonQuery("delete from dbxschema.dpagaafeceps where mqui_codiquin="+numquincenaactual+"");
						DBFunctions.NonQuery("delete from dbxschema.dprovapropiaciones where mqui_codiquin="+numquincenaactual+"");
						DBFunctions.NonQuery("delete from dbxschema.DPRESTAMOEMPLEADOS where mqui_codiquin="+numquincenaactual+"");
						DBFunctions.NonQuery("delete from dbxschema.mprovisiones where mqui_codiquin="+numquincenaactual+"");
					}
				}
			}
			else 
			{

				if (Session["DataTable1"]!=null)
					DataTable1=(DataTable)Session["DataTable1"];
			
				if (Session["DataTableLiqFinal"]!=null)
					DataTableLiqFinal=(DataTable)Session["DataTableLiqFinal"];
			
				if (Session["DataTable2"]!=null)
					DataTable2=(DataTable)Session["DataTable2"];
			
				if (Session["errores"]!=null)
					errores=(int)Session["errores"];
			
				if (Session["lb2"]!=null)
					lb2.Text=(string) Session["lb"];
							
				if (Session["activarEmp"]!=null)
					activarEmp=(ArrayList)Session["activarEmp"];				
			}
            this.DataGrid2.PageSize = 500;
        }


		protected void Grid_Change(Object sender, DataGridPageChangedEventArgs e) 
		{
			DataGrid2.CurrentPageIndex = e.NewPageIndex;
			DataGrid2.DataSource = DataTable1;
			DataGrid2.DataBind();
		}

		protected void Grid_Change2(Object sender, DataGridPageChangedEventArgs e) 
		{
			DataGrid1.CurrentPageIndex = e.NewPageIndex;
			DataGrid1.DataSource = DataTable2;
			DataGrid1.DataBind();
		}
		
		private void realizar_liquidacion()
		{
			string lb2,existe="0",existe2="0";
			lb2=(string)Session["lb2"];
			//cnomina= new DataSet();
			int errores;
			int i,reliquidacion=0,y;
			DataTable1= new DataTable();
			DataTable1=(DataTable)Session["DataTable1"];
			DataTableLiqFinal=(DataTable)Session["DataTableLiqFinal"];
			errores=(int)Session["errores"];
			activarEmp=(ArrayList)Session["activarEmp"];
			
			int desccantidad=0,messiguiente=0,anosiguiente=0;
			//JFSC 26042008 manejo de la quincena siguiente
			int quincesiguiente=0;
			anosiguiente=int.Parse(DDLANO.SelectedValue);
			//valido que la liquidacion no tenga errores.
			if (errores>0)
			{
				if(int.Parse(DDLMES.SelectedValue)==6 || (int.Parse(DDLMES.SelectedValue)==12))
				{
                    Utils.MostrarAlerta(Response, "Apreciado Usuario, existen ERRORES en la liquidación. En esta liquidacion SI SE ACEPTAN para compensar con la prima... ");
					errores=0;
				}
				else
                    Utils.MostrarAlerta(Response, "Apreciado Usuario, existen ERRORES en la liquidación porfavor corrijalos para liquidar definitivamente.");
	  		
			}
			if (errores>0)
			{
                Utils.MostrarAlerta(Response, "La liquidación NO SE HA ejecutado. ");
			}
	  	
			else
	  		{
				//existe=DBFunctions.SingleData("select count(pcon_concepto) from dbxschema.dquincena where mqui_codiquin="+DataTableLiqFinal.Rows[0][0].ToString()+"");
				existe=DBFunctions.SingleData("Select MQUI_ESTADO from DBXSCHEMA.MQUINCENAS WHERE mqui_codiquin="+DataTableLiqFinal.Rows[0][0].ToString()+"");
				
				if (existe=="2")
				{
                    Utils.MostrarAlerta(Response, "El periodo de liquidación YA fue liquidado!, si activo la quincena debe borrar los datos anteriores.");
				}
				else
				{
					//Actualizar estado de empleados con finalizacion de vacaciones exacta al fin de periodo.
					if (activarEmp != null)
					{
						for (i=0;i<activarEmp.Count;i++)
						{
							sqlStrings.Add("update dbxschema.mempleado set test_estado='1'where MEMP_CODIEMPL='"+activarEmp[i].ToString()+"' ");
							//							DBFunctions.NonQuery();
						}
					}
                    Utils.MostrarAlerta(Response, "Nómina Liquidada Satisfactoriamente");
					//si ya esta en mquincenas significa q es una reliquidacion, y no ingreso nuevo perido.
					existe2=DBFunctions.SingleData("select count(pcon_concepto) from dbxschema.dquincena where mqui_codiquin="+DataTableLiqFinal.Rows[0][0].ToString()+"");
					if (int.Parse(existe2)>0)
					{
                        Utils.MostrarAlerta(Response, "Se reliquidará esta quincena ");
						reliquidacion+=1;
					}
			
					//lista de empleados
					ArrayList EmpleadosRevisados= new ArrayList();
					//recorro el datatable 
					for (i=0;i<DataTableLiqFinal.Rows.Count;i++)
					{
						//hago una busqueda en el array, si no esta lo ingreso a la lista
						//si esta es porque ya fue liquidado.
						if (EmpleadosRevisados.BinarySearch(DataTableLiqFinal.Rows[i][1].ToString())<0)
						{
							//valido que el codigo sea de un empleado
							if (DataTableLiqFinal.Rows[i][1].ToString()!="--" && DataTableLiqFinal.Rows[i][0].ToString().Length > 0 && DataTableLiqFinal.Rows[i][1].ToString().Length > 0)
							{
								//ingreso el codigo en el array.
								EmpleadosRevisados.Add(DataTableLiqFinal.Rows[i][1].ToString());
								//actualizo el maestro de vacaciones							
								DataSet fechaingreso = new DataSet();
								DBFunctions.Request(fechaingreso,IncludeSchema.NO,"select memp_fechingreso from dbxschema.mempleado where MEMP_CODIEMPL='"+DataTableLiqFinal.Rows[i][1].ToString()+"' ");	
								string estadoempleado=DBFunctions.SingleData("select test_estado from dbxschema.mempleado where MEMP_CODIEMPL='"+DataTableLiqFinal.Rows[i][1].ToString()+"' ");
                                this.actualizarmaestrovacaciones(DataTableLiqFinal.Rows[i][1].ToString(),fechaingreso.Tables[0].Rows[0][0].ToString(),lb2,int.Parse(DataTableLiqFinal.Rows[i][0].ToString()));
					
								//query que trae los conceptos de cada empleado.				
								DataRow[]seleccion= DataTableLiqFinal.Select("CODIGO='"+DataTableLiqFinal.Rows[i][1].ToString()+"'");
								if (estadoempleado=="5")
								{
									sqlStrings.Add("update dbxschema.mempleado set test_estado='1'where MEMP_CODIEMPL='"+DataTableLiqFinal.Rows[i][1].ToString()+"' ");
									//									DBFunctions.NonQuery();
                                    Utils.MostrarAlerta(Response, "Reintegre al empleado con codigo:" + DataTableLiqFinal.Rows[i][1].ToString() + "  ");
								}
				

								//string mqui_codiquin=seleccion[0][0].ToString();
								//string memp_codiempl=seleccion[0][1].ToString();
								
								for (y=0;y<seleccion.Length;y++)
								{
									//segun la desc avergiaru el numero.
									if (seleccion[y][9].ToString()=="DIAS")
										desccantidad=1;
									if (seleccion[y][9].ToString()=="HORAS")
										desccantidad=2;    
									if (seleccion[y][9].ToString()=="MINUTOS")
										desccantidad=3;
									if (seleccion[y][9].ToString()=="PESOS M/CTE")
										desccantidad=4;
									
									//inserto en la tabla de detalles de quincena.
									sqlStrings.Add("insert into dquincena values(DEFAULT,"+seleccion[y][0].ToString()+",'"+seleccion[y][1].ToString()+"','"+seleccion[y][3].ToString()+"',"+seleccion[y][5].ToString()+","+seleccion[y][6].ToString()+","+seleccion[y][7].ToString()+","+seleccion[y][8].ToString()+","+desccantidad+" ,"+seleccion[y][10].ToString()+",'"+seleccion[y][11].ToString()+"','N')");
									//									DBFunctions.NonQuery("insert into dquincena values("+seleccion[y][0].ToString()+",'"+seleccion[y][1].ToString()+"','"+seleccion[y][3].ToString()+"',"+seleccion[y][4].ToString()+","+seleccion[y][5].ToString()+","+seleccion[y][6].ToString()+","+seleccion[y][7].ToString()+","+desccantidad+" ,"+seleccion[y][9].ToString()+",'"+seleccion[y][10].ToString()+"','N')");
									//valido si el concepto es un préstamo.
									if (seleccion[y][10].ToString()!= lbdocref.Text && seleccion[y][10].ToString()!="--")
									{
                                        sqlStrings.Add("update mprestamoempleados set mpre_cuotpaga =mpre_cuotpaga+1 ,mpre_valopaga=mpre_valopaga+" + seleccion[y][8].ToString() + " where memp_codiempl = '" + seleccion[y][1].ToString() + "' and PCON_CONCEPTO = '" + seleccion[y][3].ToString() + "' and mpre_numelibr=" + seleccion[y][11].ToString() + " ");
										//actualizar las cuotas pagadas de mprestamo
										//										DBFunctions.NonQuery("update mprestamoempleados set mpre_cuotpaga =mpre_cuotpaga+1 ,mpre_valopaga=mpre_valopaga+"+seleccion[y][7].ToString()+" where mpre_numelibr="+seleccion[y][10].ToString()+" ");
									}
								}
							}
						}
					}
					//**********pasar al siguiente período de liquidación****************
					//si estoy en primera quincena paso a la segunda, segunda a primera
					//cierro la quincena 2 es cerrada.
					if (lbtipopago.Text=="QUINCENAL")
					{
						sqlStrings.Add("update mquincenas set mqui_estado = 2");
						if (DDLQUIN.SelectedValue=="1")
						{
							sqlStrings.Add("update cnomina set cnom_quincena = 2");
							//JFSC 26042008 tener quincena siguiente
							quincesiguiente=2;
							messiguiente=int.Parse(DDLMES.SelectedValue);
							anosiguiente=int.Parse(DDLANO.SelectedValue);
						}
						else 
						{
							quincesiguiente=1;
							sqlStrings.Add("update cnomina set cnom_quincena = 1");
							if (int.Parse(DDLMES.SelectedValue)>=1 && int.Parse(DDLMES.SelectedValue)<=11)
							{
								messiguiente=int.Parse(DDLMES.SelectedValue)+1;
								anosiguiente=int.Parse(DDLANO.SelectedValue);
								sqlStrings.Add("update cnomina set cnom_mes = "+messiguiente+"");
							}
							else
							{
                                messiguiente = 1;
                                anosiguiente = int.Parse(DDLANO.SelectedValue) + 1;
						        sqlStrings.Add("update cnomina set cnom_mes =1, cnom_ano =" + anosiguiente + " ");
							}
						}
					//JFSC 26042008 se debe insertar la nueva quincena hasta liquidar definitivamente
					sqlStrings.Add("insert into mquincenas values (default,"+anosiguiente+","+messiguiente+","+quincesiguiente+",1)");
					}
					if (lbtipopago.Text=="MENSUAL")
					{
						sqlStrings.Add("update mquincenas set mqui_estado = 2");
						quincesiguiente=2;
						if (DDLQUIN.SelectedValue=="2")
						{
							if (int.Parse(DDLMES.SelectedValue)>=1 && int.Parse(DDLMES.SelectedValue)<=11)
							{
								messiguiente=int.Parse(DDLMES.SelectedValue)+1;
								sqlStrings.Add("update cnomina set cnom_mes = "+messiguiente+"");
								//quincesiguiente=1;
							}
							else
							{
								anosiguiente = int.Parse(DDLANO.SelectedValue)+1;
                                messiguiente = 1; // del mes 12 se pasa al mes 01
                                sqlStrings.Add("update cnomina set cnom_mes =1, cnom_ano ="+anosiguiente+" ");
								//quincesiguiente=2;
							}
						}
						//JFSC 26042008 se debe insertar la nueva quincena hasta liquidar definitivamente
					sqlStrings.Add("insert into mquincenas values (default,"+anosiguiente+","+messiguiente+","+quincesiguiente+",1)");
					}
				 
					if (DBFunctions.Transaction(sqlStrings) == false)
					{
                        Utils.MostrarAlerta(Response, "hubo errores en la liquidación , por favor revise los Logs del sistema");
                        lblError.Text = DBFunctions.exceptions;
					}
					else
					{
                        Utils.MostrarAlerta(Response, "El período de liquidación  se liquidado Ok!");
					}
					// Response.Redirect(mainpage+"?process=Nomina.LiquidacionNomina");
				}
			}
		}
		
		protected void editar(Object sender , DataGridCommandEventArgs e)
		{
			DataGrid2.EditItemIndex = e.Item.DataSetIndex;
			DataGrid2.DataSource    = DataTable1;
			DataGrid2.DataBind();
			Session["DataTable1"]   = DataTable1;
		}
		
		protected void eliminar(Object sender , DataGridCommandEventArgs e)
		{
			DataTable1.Rows[e.Item.DataSetIndex].Delete();
			DataGrid2.DataSource    = DataTable1;
			DataGrid2.DataBind();
			Session["DataTable1"]   = DataTable1;
            Utils.MostrarAlerta(Response, "Este concepto se elimina solo en esta grilla, no en la base de datos  ");
		}
		
		protected void cancelar(Object sender , DataGridCommandEventArgs e)
		{
			DataGrid2.EditItemIndex =-1;
			DataGrid2.DataSource    = DataTable1;
			DataGrid2.DataBind();
			Session["DataTable1"]   = DataTable1;
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
			this.btnGenInforme.Click += new System.EventHandler(this.generarInforme);
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

        protected void BTNCONFIRMACION_Click(object sender, EventArgs e)
        {
            BTNCONFIRMACION.Enabled = false;
            this.realizar_liquidacion();    
        }
     
       }    
    }
     

